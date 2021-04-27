Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class GestRibaltaVar
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestRibaltaVar))

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

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Try
    '        Dim WFSessione As OPENUtility.CreateSessione
    '        Dim WFErrore As String
    '        Dim FncGest As New ClsRibaltaVar
    '        Dim oListVariazioni() As objRicercaVariazione

    '        If Page.IsPostBack = False Then
    '            Try
    '                Dim paginacomandi As String = Request("paginacomandi")
    '                Dim parametri As String
    '                parametri = "?title=Acquedotto - " & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("DE ", "") & " - " & "Ricerca" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
    '                If Len(paginacomandi) = 0 Then
    '                    'paginacomandi = "/OpenUtenzeGC/ComandiRicerca/ComandiRicerca.aspx"
    '                    paginacomandi = ConstSession.PathApplicazione & "/Fatturazione/RibaltaVariazioni/ComandiRibaltaVar.aspx"
    '                End If

    '                dim sScript as string=""
    '                
    '                sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "'")
    '                
    '                RegisterScript(sScript , Me.GetType())
    '                'inizializzo la connessione
    '                WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '                If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '                    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '                End If
    '                'popolo la tabella di appoggio
    '                If FncGest.SetVariazioni(WFSessione) = False Then
    '                    Throw New Exception("Errore durante il popolamento della tabella d'appoggio delle variazioni")
    '                End If

    '                oListVariazioni = FncGest.GetRicercaVariazioni(ConstSession.IdEnte, FncGest.sTypeRicerca.VARFATT_DAGESTIRE, WFSessione)
    '                If Not oListVariazioni Is Nothing Then
    '                    'popolo la griglia
    '                    GrdRibaltaVar.DataSource = oListVariazioni
    '                    GrdRibaltaVar.start_index = GrdRibaltaVar.CurrentPageIndex
    '                    GrdRibaltaVar.DataBind()
    '                    GrdRibaltaVar.SelectedIndex = -1
    '                Else
    '                    LblResult.Text = "La ricerca non ha prodotto risultati."
    '                    LblResult.Style.Add("display", "")
    '                End If
    '            Catch Err As Exception
    '              Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.Page_Load.errore: ", Err)
    '                Response.Redirect("../../../PaginaErrore.aspx")
    '            Finally
    '                WFSessione.Kill()
    '            End Try
    '            Session("oListVariazioni") = oListVariazioni
    '        Else
    '            ControllaCheckbox()
    '            GrdRibaltaVar.AllowCustomPaging = False
    '            oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
    '            GrdRibaltaVar.Rows.Count = oListVariazioni.Length
    '            GrdRibaltaVar.DataSource = oListVariazioni
    '            GrdRibaltaVar.start_index = GrdRibaltaVar.CurrentPageIndex.ToString()
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim FncGest As New ClsRibaltaVar
            Dim oListVariazioni() As objRicercaVariazione
            oListVariazioni = Nothing

            If Page.IsPostBack = False Then
                Try
                    Dim paginacomandi As String = Request("paginacomandi")
                    Dim parametri As String = ""
                    'parametri = "?title=Acquedotto - " & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("DE ", "") & " - " & "Ricerca" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
                    If Len(paginacomandi) = 0 Then
                        'paginacomandi = "/OpenUtenzeGC/ComandiRicerca/ComandiRicerca.aspx"
                        paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/Fatturazione/RibaltaVariazioni/ComandiRibaltaVar.aspx"
                    End If

                    dim sScript as string=""
                    sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                    RegisterScript(sScript , Me.GetType())
                    'popolo la tabella di appoggio
                    If FncGest.SetVariazioni() = False Then
                        Throw New Exception("Errore durante il popolamento della tabella d'appoggio delle variazioni")
                    End If

                    oListVariazioni = FncGest.GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, -1, -1)
                    If Not oListVariazioni Is Nothing Then
                        'popolo la griglia
                        GrdRibaltaVar.DataSource = oListVariazioni
                        GrdRibaltaVar.DataBind()
                        GrdRibaltaVar.SelectedIndex = -1
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                    End If
                Catch Err As Exception

                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.Page_Load.errore: ", Err)
                    Response.Redirect("../../PaginaErrore.aspx")
                End Try
                Session("oListVariazioni") = oListVariazioni
            Else
                ControllaCheckbox()
                'oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
                'GrdRibaltaVar.DataSource = oListVariazioni
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdRibaltaVar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaVar.Click
    '    'calcolo una fattura per ogni lettura
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    '    Dim FncGest As New ClsRibaltaVar
    '    Dim oListVariazioni() As objRicercaVariazione
    '    Dim sScript, sNoteAgg As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'ricarico i dati dalla sessione
    '        oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
    '        'richiamo la funzione di ribaltamento variazioni
    '        oListVariazioni = FncGest.RibaltaVariazioni(oListVariazioni, WFSessione, sNoteAgg)
    '        If oListVariazioni Is Nothing Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'L\'elaborazione e\' stata terminata a causa di un errore!')"
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'L\'elaborazione e\' stata terminata correttamente!"
    '            If sNoteAgg <> "" Then
    '                sScript += "\nAggiornare manualmente le seguenti matricole perche\' hanno una lettura successiva alla variazione gia\' fatturata:"
    '                sScript += sNoteAgg
    '            End If
    '            sScript += "')"
    '        End If
    '        'ripopolo la griglia
    '        oListVariazioni = FncGest.GetRicercaVariazioni(ConstSession.IdEnte, FncGest.sTypeRicerca.VARFATT_DAGESTIRE, WFSessione)
    '        If Not oListVariazioni Is Nothing Then
    '            'popolo la griglia
    '            GrdRibaltaVar.DataSource = oListVariazioni
    '            GrdRibaltaVar.start_index = GrdRibaltaVar.CurrentPageIndex
    '            GrdRibaltaVar.DataBind()
    '            GrdRibaltaVar.SelectedIndex = -1
    '        Else
    '            LblResult.Text = "La ricerca non ha prodotto risultati."
    '            LblResult.Style.Add("display", "")
    '        End If
    '        RegisterScript(sScript , Me.GetType())
    '        DivAttesa.Style.Add("display", "none")
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.CmdRibaltaVar_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    Private Sub CmdRibaltaVar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaVar.Click
        'calcolo una fattura per ogni lettura
        Dim FncGest As New ClsRibaltaVar
        Dim oListVariazioni() As objRicercaVariazione
        Dim sScript, sNoteAgg As String

        sNoteAgg = ""
        Try
            'ricarico i dati dalla sessione
            oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
            'richiamo la funzione di ribaltamento variazioni
            oListVariazioni = FncGest.RibaltaVariazioni(oListVariazioni, sNoteAgg)
            If oListVariazioni Is Nothing Then
                sScript = "GestAlert('a', 'warning', '', '', 'L\'elaborazione e\' stata terminata a causa di un errore!');"
            Else
                sScript = "GestAlert('a', 'success', '', '', 'L\'elaborazione e\' stata terminata correttamente!);"
                If sNoteAgg <> "" Then
                    sScript += "\nAggiornare manualmente le seguenti matricole perche\' hanno una lettura successiva alla variazione gia\' fatturata:"
                    sScript += sNoteAgg
                End If
                sScript += "')"
            End If
            'ripopolo la griglia
            oListVariazioni = FncGest.GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, -1, -1)
            If Not oListVariazioni Is Nothing Then
                'popolo la griglia
                GrdRibaltaVar.DataSource = oListVariazioni
                GrdRibaltaVar.DataBind()
                GrdRibaltaVar.SelectedIndex = -1
            Else
                LblResult.Text = "La ricerca non ha prodotto risultati."
                LblResult.Style.Add("display", "")
            End If
            RegisterScript(sScript , Me.GetType())
            DivAttesa.Style.Add("display", "none")
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.CmdRibaltaVar_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdRibaltaVar.DataSource = CType(Session("oListVariazioni"), DataView)
            If page.HasValue Then
                GrdRibaltaVar.PageIndex = page.Value
            End If
            GrdRibaltaVar.DataBind()
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    Private Sub chkElaboraTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkElaboraTutti.CheckedChanged
        Dim itemGrid As GridViewRow
        Dim oListVariazioni() As objRicercaVariazione
        Dim x As Integer
        Try
            If chkElaboraTutti.Checked = True Then
                '*****************************************************
                'seleziono tutti gli elementi
                '*****************************************************
                oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
                If Not oListVariazioni Is Nothing Then
                    For x = 0 To oListVariazioni.Length - 1
                        oListVariazioni(x).bIsSel = True
                    Next
                    For Each itemGrid In GrdRibaltaVar.Rows
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = True
                    Next
                End If
                Session("oListVariazioni") = oListVariazioni
            Else
                oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
                If Not oListVariazioni Is Nothing Then
                    For x = 0 To oListVariazioni.Length - 1
                        oListVariazioni(x).bIsSel = False
                    Next
                    For Each itemGrid In GrdRibaltaVar.Rows
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = False
                    Next
                End If
                Session("oListVariazioni") = oListVariazioni
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.chkElaboraTutti_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim oListVariazioni() As objRicercaVariazione
        Dim x As Integer








        Try
            oListVariazioni = CType(Session("oListVariazioni"), objRicercaVariazione())
            For Each itemGrid In GrdRibaltaVar.Rows
                'prendo l'ID da aggiornare
                For x = 0 To oListVariazioni.GetUpperBound(0)
                    'confronto il codice cartella per individuare l'elemento all'interno della griglia

                    'originale
                    'If oListVariazioni(x).nIdVariazione = CType(itemGrid.Cells(7).Text, String) Then

                    If oListVariazioni(x).nIdVariazione = CType(itemGrid.FindControl("hfnIdVariazione"), HiddenField).Value Then
                        oListVariazioni(x).bIsSel = CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked
                    End If

                Next
            Next
            Session("oListVariazioni") = oListVariazioni
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestRibaltaVar.ControllaCheckbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Function FormattaVariazione(ByVal sIsVariato As String) As String
        If sIsVariato <> "" Then
            Return "..\..\images\Bottoni\visto.png"
        Else
            Return ""
        End If
    End Function
End Class
