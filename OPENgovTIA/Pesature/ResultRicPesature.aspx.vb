Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca pesature.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ResultRicPesature
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicPesature))

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim MyFunction As New ObjRicercaUtentePesature
            Dim oMyRicerca As New ObjRicercaUtentePesature
            Dim oMyUtentiPesature() As ObjUtentePesature
            Dim nContrib As Integer = 0
            Dim nConferimenti As Integer = 0
            Dim nKG As Double = 0
            Dim nVolume As Double = 0
            Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

            If Page.IsPostBack = False Then
                oMyRicerca.sEnte = ConstSession.IdEnte
                oMyRicerca.sCognome = Request.Item("TxtCognome")
                oMyRicerca.sNome = Request.Item("TxtNome")
                oMyRicerca.sCodFiscale = Request.Item("TxtCodFiscale")
                oMyRicerca.sPIva = Request.Item("TxtPIva")
                oMyRicerca.sCodUtente = Request.Item("TxtCodUtente")
                oMyRicerca.sNumTessera = Request.Item("TxtNTessera")
                If Request.Item("TxtCodTessera") <> "" Then
                    oMyRicerca.sNumTessera = Request.Item("TxtCodTessera")
                End If
                If Request.Item("TxtDal") <> "" Then
                    oMyRicerca.tPeriodoDal = Request.Item("TxtDal")
                End If
                If Request.Item("TxtAl") <> "" Then
                    oMyRicerca.tPeriodoAl = Request.Item("TxtAl")
                End If
                If Not Request.Item("IsFatturato") Is Nothing Then
                    oMyRicerca.IsFatturato = CInt(Request.Item("IsFatturato"))
                End If
                If Not Request.Item("TxtFileImport") Is Nothing Then
                    oMyRicerca.sFileImport = Request.Item("TxtFileImport")
                End If
                Session("oRicercaUtentePesature") = oMyRicerca
                oMyUtentiPesature = MyFunction.GetRicercaPesature(oMyRicerca, cmdMyCommand)
                If Not oMyUtentiPesature Is Nothing Then
                    'calcolo i totalizzatori
                    MyFunction.GetTotalizzatori(oMyUtentiPesature, nContrib, nKG, nVolume, nConferimenti)
                    LblNContribuenti.Text = "N.Utenti " + FormatNumber(nContrib, 0)
                    LblNConferimenti.Text = "N.Conferimenti " + FormatNumber(nConferimenti, 0)
                    LblKg.Text = "Kg. " + FormatNumber(nKG, 2)
                    LblVolume.Text = "Litri " + FormatNumber(nVolume, 2)
                    'popolo la griglia
                    GrdTessere.DataSource = oMyUtentiPesature
                    GrdTessere.DataBind()
                    LblResult.Style.Add("display", "none")
                Else
                    LblResult.Style.Add("display", "")
                    LblNContribuenti.Style.Add("display", "")
                    LblNConferimenti.Style.Add("display", "")
                    LblKg.Style.Add("display", "")
                    LblVolume.Style.Add("display", "")
                End If
                Session("oUtentiPesature") = oMyUtentiPesature
            Else
                oMyUtentiPesature = CType(Session("oUtentiPesature"), ObjUtentePesature())
                GrdTessere.DataSource = oMyUtentiPesature
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicPesature.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDelete.Click
    '    Dim MyFunction As New GestPesatura
    '    Dim oMyUtentiPesature() As ObjUtentePesature
    '    Dim WFErrore As String = ""
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim x As Integer
    '    Dim MyFunctionRic As New ObjRicercaUtentePesature
    '    Dim oMyRicerca As New ObjRicercaUtentePesature
    '    Dim nContrib As Integer = 0
    '    Dim nConferimenti As Integer = 0
    '    Dim nKG As Double = 0
    '    Dim nVolume As Double = 0

    '    Try
    '        If Request.Item("TxtDal") <> "" And Request.Item("TxtAl") <> "" Then
    '            'inizializzo la connessione
    '            WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '            If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '                Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            End If

    '            oMyUtentiPesature = CType(Session("oUtentiPesature"), ObjUtentePesature())
    '            For x = 0 To oMyUtentiPesature.GetUpperBound(0)
    '                If MyFunction.SetPesatura(Nothing, 2, WFSessione, oMyUtentiPesature(x).nIdTessera, Request.Item("TxtDal"), Request.Item("TxtAl")) <= 0 Then
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                End If
    '            Next
    '            'rieffettuo la ricerca
    '            oMyRicerca = CType(Session("oRicercaUtentePesature"), ObjRicercaUtentePesature)
    '            oMyUtentiPesature = MyFunctionRic.GetRicercaPesature(oMyRicerca)
    '            If Not oMyUtentiPesature Is Nothing Then
    '                'calcolo i totalizzatori
    '                MyFunctionRic.GetTotalizzatori(oMyUtentiPesature, nContrib, nKG, nvolume, nConferimenti)
    '                LblNContribuenti.Text = FormatNumber(nContrib, 0)
    '                LblNConferimenti.Text = FormatNumber(nConferimenti, 0)
    '                LblKg.Text = FormatNumber(nKG, 2)
    '                LblVolume.Text = FormatNumber(nVolume, 2)
    '                'popolo la griglia
    '                GrdTessere.DataSource = oMyUtentiPesature
    '                GrdTessere.start_index = GrdTessere.CurrentPageIndex
    '                GrdTessere.DataBind()
    '                GrdTessere.SelectedIndex = -1
    '                LblResult.Style.Add("display", "none")
    '            Else
    '                LblResult.Style.Add("display", "")
    '                GrdTessere.Style.Add("display", "none")
    '            End If
    '            Session("oUtentiPesature") = oMyUtentiPesature
    '            RegisterScript(me.gettype(),"del", "<script language='javascript'>alert('Eliminazione effettuata con successo!');</script>")
    '        Else
    '            RegisterScript(me.gettype(),"del", "<script language='javascript'>alert('Effettuare la ricerca per le date\nprima di procedere alla cancellazione!');</script>")
    '        End If
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicPesature.CmdDelete_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub CmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDelete.Click
    '    Dim MyFunction As New GestPesatura
    '    Dim oMyUtentiPesature() As ObjUtentePesature
    '    Dim x As Integer
    '    Dim MyFunctionRic As New ObjRicercaUtentePesature
    '    Dim oMyRicerca As New ObjRicercaUtentePesature
    '    Dim nContrib As Integer = 0
    '    Dim nConferimenti As Integer = 0
    '    Dim nKG As Double = 0
    '    Dim nVolume As Double = 0

    '    Try
    '        If Request.Item("TxtDal") <> "" And Request.Item("TxtAl") <> "" Then
    '            oMyUtentiPesature = CType(Session("oUtentiPesature"), ObjUtentePesature())
    '            For x = 0 To oMyUtentiPesature.GetUpperBound(0)
    '                If MyFunction.SetPesatura(ConstSession.StringConnection, Nothing, 2, oMyUtentiPesature(x).nIdTessera, Request.Item("TxtDal"), Request.Item("TxtAl")) <= 0 Then
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                End If
    '            Next
    '            'rieffettuo la ricerca
    '            oMyRicerca = CType(Session("oRicercaUtentePesature"), ObjRicercaUtentePesature)
    '            oMyUtentiPesature = MyFunctionRic.GetRicercaPesature(oMyRicerca)
    '            If Not oMyUtentiPesature Is Nothing Then
    '                'calcolo i totalizzatori
    '                MyFunctionRic.GetTotalizzatori(oMyUtentiPesature, nContrib, nKG, nvolume, nConferimenti)
    '                LblNContribuenti.Text = FormatNumber(nContrib, 0)
    '                LblNConferimenti.Text = FormatNumber(nConferimenti, 0)
    '                LblKg.Text = FormatNumber(nKG, 2)
    '                LblVolume.Text = FormatNumber(nVolume, 2)
    '                'popolo la griglia
    '                GrdTessere.DataSource = oMyUtentiPesature
    '                GrdTessere.start_index = GrdTessere.CurrentPageIndex
    '                GrdTessere.DataBind()
    '                GrdTessere.SelectedIndex = -1
    '                LblResult.Style.Add("display", "none")
    '            Else
    '                LblResult.Style.Add("display", "")
    '                GrdTessere.Style.Add("display", "none")
    '            End If
    '            Session("oUtentiPesature") = oMyUtentiPesature
    '            RegisterScript(Me.gettype(), "del", "<script language='javascript'>alert('Eliminazione effettuata con successo!');</script>")
    '        Else
    '            RegisterScript(Me.gettype(), "del", "<script language='javascript'>alert('Effettuare la ricerca per le date\nprima di procedere alla cancellazione!');</script>")
    '        End If
    '    Catch Err As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicPesature.CmdDelete_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                Dim sScript As String = ""
                'sScript = "parent.parent.Comandi.location.href='ComandiGestPesature.aspx';"
                sScript += "parent.parent.Visualizza.location.href='GestionePesature.aspx?IdTessera=" + IDRow.ToString + "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicPesature.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub

    'Private Sub GrdTessere_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdTessere.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            'Gestione Riga Tabella
    '            Dim IdTessera As String
    '            IdTessera = e.Item.Cells(8).Text

    '            e.Item.Attributes.Add("onClick", "DettaglioPesature('" & IdTessera & "')")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicPesature.GrdTessere_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Public Function FormattaNumeroGrd(ByVal nNumeroGrd As Integer) As String
    '    If nNumeroGrd < 0 Then
    '        Return ""
    '    Else
    '        Return nNumeroGrd
    '    End If
    'End Function
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdTessere.DataSource = CType(Session("oUtentiPesature"), ObjUtentePesature())
            If page.HasValue Then
                GrdTessere.PageIndex = page.Value
            End If
            GrdTessere.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicPesature.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
