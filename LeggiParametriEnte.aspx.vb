Imports OPENUtility
Imports Business
Imports RemotingInterfaceAnater
Imports System.Messaging
Imports log4net
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
''' <summary>
''' Pagina nascosta per il login diretto da ANATER
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="10/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Lista Rilasci</em>
''' Il sistema mostrerà gli ultimi 5 aggiornamenti effettuati; per consultare la lista completa degli aggiornamenti sarà reso disponibile a seguire il link “vedi tutti gli aggiornamenti”. Per ogni aggiornamento sarà visualizzata la data di esecuzione ed una breve descrizione.
''' L'elenco sarà l’insieme di righe cliccabili; cliccando sulla riga prescelta sarà visualizzata la descrizione completa e dettagliata delle modifiche apportate; sarà inoltre reso disponibile il manuale specifico, ove presente, per il download. 
''' La pagina di lettura dati ente (LeggiParametriEnte.aspx) sarà integrata con il div per la visualizzazione storico revisioni. La lista dei rilasci sarà creata automaticamente leggendo i files XML presenti nell'apposita cartella \DATI\OPENGOV\RILASCI\.
''' Il file XML dovrà essere così formato
''' <releasehistory>
''' <revision>
''' <item></item>	Numero identificativo
''' <date></date>	Data del rilascio
''' <title> Information</title>	Descrizione breve visualizzata
''' <description></description>	Dettaglio delle modifiche apportate comprensivo di eventuale formattazione specifica
''' <document> </document>	Nome del manuale da scaricare
''' </revision>	
''' </releasehistory>	
''' Ogni file dovrà essere riferito ad un solo rilascio. Il nome del file sarà l'identificativo del rilascio. Ogni file di tipo .xml presente nella directory sarà letto e convalidato rispetto alla struttura sopra; se non coerente sarà scartato senza evidenziare errori a sistema; il file sarà caricato in una lista di oggetti avente la stessa definizione del file; la lista sarà successivamente ordinata in modo decrescente per data e se avrà più di 5 elementi saranno visualizzati solo i primi 5 elementi con la dicitura “vedi tutti gli aggiornamenti” detta “Lista Breve”, altrimenti saranno visualizzati tutti gli elementi presenti. Cliccando sull’ultimo elemento per vedere tutti gli aggiornamenti sarà sostituita la visualizzazione della “Lista Breve” con la “Lista Completa”.
''' Attenzione la “Lista Completa” comprenderà solo gli ultimi 100 aggiornamenti.
''' Se si desiderasse vedere aggiornamenti pregressi si dovrà contattare l’assistenza; questa opzione sarà indicata, a piè della lista, in caso di necessità.
''' Per poter inserire nel tag xml il testo comprensivo di formattazione HTML si dovrà usare l'XML DOM CDATASection Object.
''' </revision>
''' </revisionHistory>
Partial Class LeggiParametriEnte
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(LeggiParametriEnte))

    Private GestErrore As New VisualizzaErrore
    Private ListShortRel As ArrayList
    Private ListCompleteRel As ArrayList

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="10/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Lista Rilasci</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                'carico liste da files
                If GetListRelease(ListShortRel, ListCompleteRel) Then
                    'carico dati nella pagina
                    LoadForm("divListRelShort", ListShortRel)
                    LoadForm("divListRelComplete", ListCompleteRel)
                    RegisterScript("$('#divListRelComplete').hide();", Me.GetType)
                End If

                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "Login", "LeggiParametriEnte", "lettura lista rilasci", "", "", -1)
            End If
        Catch ex As Exception
            Log.Debug("OPENgov.LeggiParametriEnte.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la gestione corretta della disconnesione utente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub CmdLogOut_Click(sender As Object, e As EventArgs)
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
                RegisterScript("parent.location.href = '" & Request.Url.GetLeftPart(UriPartial.Authority) & "/" & Request.ApplicationPath & "/Default.aspx';", Me.GetType)
            Else
                RegisterScript("GestAlert('a', 'warning', '', '', 'Errore in logout!');", Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.LeggiParametriEnte.LogOut.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per il caricamento dell'elenco dei rilasci effettuati
    ''' </summary>
    ''' <param name="ListShort">ByRef ArrayList lista breve</param>
    ''' <param name="ListComplete">ByRef ArrayList lista completa</param>
    ''' <returns>bool false in caso di errore altrimenti true</returns>
    Private Function GetListRelease(ByRef ListShort As ArrayList, ByRef ListComplete As ArrayList) As Boolean
        ListShort = New ArrayList
        ListComplete = New ArrayList
        Try
            Dim myRelDetail As New ReleaseHistory
            Dim x As Integer = 0
            'carico lista completa dai files
            Dim myDir As New DirectoryInfo(COSTANTValue.ConstSession.GetRepository + "\" + COSTANTValue.ConstSession.GetRepositoryRilasci + "\")
            Dim ListFiles As FileInfo() = myDir.GetFiles()
            'ordino decrescente per nome
            Array.Sort(ListFiles, New Utility.Comparatore(New String() {"Name"}, New Boolean() {Utility.TipoOrdinamento.Decrescente}))
            For Each myFile As FileInfo In ListFiles
                myRelDetail = New ReleaseHistory
                If myFile.Extension.ToLower = ".xml" Then
                    Dim myFileStream As New FileStream(myFile.FullName, FileMode.Open)
                    Try
                        myRelDetail = (New XmlSerializer(GetType(ReleaseHistory))).Deserialize(myFileStream)
                    Catch err As Exception
                        Log.Debug("OPENgov.LeggiParametriEnte.GetListRelease.Deserialize.errore: ", err)
                    Finally
                        myFileStream.Close()
                    End Try
                End If
                If myRelDetail.ItemID <> "" Then
                    ListComplete.Add(myRelDetail)
                End If
            Next
            'ne carico al massimo 100 nella lista completa e se serve carico dicitura per visualizzare ulteriori
            If ListComplete.Count >= 100 Then
                ListComplete.RemoveRange(100, ListComplete.Count - 100)
                myRelDetail = New ReleaseHistory
                myRelDetail.ItemID = "MORE"
                myRelDetail.Title = "Contattare l'assistenza per vedere aggiornamenti pregressi"
                ListComplete.Add(myRelDetail)
            End If
            'carico la lista breve
            For Each myRelDetail In ListComplete
                If x < 5 Then
                    ListShort.Add(myRelDetail)
                Else
                    'se serve carico dicitura per visualizzare lista completa
                    myRelDetail = New ReleaseHistory
                    myRelDetail.ItemID = "ALL"
                    myRelDetail.Title = "Vedi tutti gli aggiornamenti"
                    ListShort.Add(myRelDetail)
                    Exit For
                End If
                x += 1
            Next
            Return True
        Catch ex As Exception
            Log.Debug("OPENgov.LeggiParametriEnte.GetListRelease.errore: ", ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Funzione per il caricamento a video della lista degli aggiornamenti
    ''' </summary>
    ''' <param name="TypeList">String nome del div della lista da visualizzare</param>
    ''' <param name="ListRel">ArrayList lista degli aggiornamenti da visualizzare</param>
    Private Sub LoadForm(TypeList As String, ListRel As ArrayList)
        Dim sScript As String = ""
        Try
            If ListRel.Count > 0 Then
                sScript += "$('#" + TypeList + "').append('<h2>Cronologia versioni</h2>');"
                For Each myRel As ReleaseHistory In ListRel
                    sScript += "$('#" + TypeList + "').append('"
                    sScript += "<div class=\'grouprel\'>"
                    sScript += "<div id=\'btn" + myRel.ItemID.Replace("-", "") + "\' class=\'collapsible"
                    If myRel.ItemID.IndexOf("-") < 0 Then
                        sScript += " success"
                    End If
                    sScript += "\' onclick=\'GestRelease(""btn" + myRel.ItemID.Replace("-", "") + """)\'>"
                    If myRel.DateRel <> "" Then
                        sScript += myRel.DateRel + ": "
                    End If
                    sScript += myRel.Title + "</div>"
                    sScript += "<div class=\'content\'>"
                    sScript += myRel.Description
                    If myRel.DocumentRef <> "" Then
                        sScript += "<div class=\'tooltip\'>"
                        sScript += "<a href=\'" + COSTANTValue.ConstSession.GetRepositoryRilasci + "/" + myRel.DocumentRef + "\' download=\'Manuale_" + myRel.ItemID.Replace("-", "") + "\'>"
                        sScript += "<img class=\'Bottone BottonePDF\' alt=\'\'>"
                        sScript += "</a>"
                        sScript += "<span class=\'tooltiptext\'>Scarica il manuale</span>"
                        sScript += "</div>"
                    End If
                    sScript += "</div>"
                    sScript += "</div>"
                    sScript += "');"
                Next
            End If
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug("OPENgov.LeggiParametriEnte.LoadForm.errore: ", ex)
        End Try
    End Sub
End Class