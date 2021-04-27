Imports log4net
Imports AnagInterface
''' <summary>
''' Pagina per la visualizzazione unificata in avvisi dell'anagrafica
''' </summary>
Partial Class DatiContribuente
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DatiContribuente))
    'Private WFErrore As String
    'Private WFSessione As OPENUtility.CreateSessione

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label

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
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim DatiContribuente As New DettaglioAnagrafica

        Try
            LnkAnagAnater.Attributes.Add("onclick", "ApriRicAnater();")
            LnkPulisciContr.Attributes.Add("onclick", "return ClearDatiContrib();")

            'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            If Not Page.IsPostBack Then
                'prelevo i dati anagrafici
                'Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConfigurationManager.AppSettings("ParametroAnagrafica"))
                'DatiContribuente = oMyAnagrafica.GetAnagrafica(CInt(Request.Item("IdContribuente")), ConstSession.CodTributo)
                Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
                '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                'DatiContribuente = oMyAnagrafica.GetAnagrafica(CInt(Request.Item("IdContribuente")), ConstSession.CodTributo, Costanti.INIT_VALUE_NUMBER, ConstSession.StringConnectionAnagrafica)
                DatiContribuente = oMyAnagrafica.GetAnagrafica(CInt(Request.Item("IdContribuente")), Utility.Costanti.INIT_VALUE_NUMBER, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                VisualDati(DatiContribuente)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.DatiContribuente.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub

    'Private Sub LnkAnagTributi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagTributi.Click
    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Session.Remove(ViewState("sessionName"))

    '        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
    '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '        oDettaglioAnagrafica.COD_CONTRIBUENTE = TxtCodContribuente.Text
    '        oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = TxtIdDataAnagrafica.Text
    '        ViewState("sessionName") = "codContribuente"
    '        Session(ViewState("sessionName")) = oDettaglioAnagrafica
    '        writeJavascriptAnagrafica(ViewState("sessionName"))
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.DatiContribuente.LnkAnagTributi_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Sub
    Private Sub LnkAnagTributi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagTributi.Click
        Try
            Session.Remove(ViewState("sessionName"))

            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
            oDettaglioAnagrafica.COD_CONTRIBUENTE = TxtCodContribuente.Text
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = TxtIdDataAnagrafica.Text
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName")) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName"))
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.DatiContribuente.LnkAnagTributi_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Dim oDettaglioAnagrafica As DettaglioAnagrafica
        Try
            If Not Session(ViewState("sessionName")) Is Nothing Then
                oDettaglioAnagrafica = Session(ViewState("sessionName"))
                Session("oAnagrafe") = oDettaglioAnagrafica

                VisualDati(oDettaglioAnagrafica)

                TxtCodContribuente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                TxtIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                ViewState("sessionName") = ""
                Session("SEARCHPARAMETRES") = Nothing
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.DatiContribuente.btnRibalta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnRibaltaAnagAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaAnagAnater.Click
        Dim oDettaglioAnagrafica As DettaglioAnagrafica

        Try
            If Not IsNothing(Session("AnagrafeAnaterRibaltata")) Then
                oDettaglioAnagrafica = CType(Session("AnagrafeAnaterRibaltata"), DettaglioAnagrafica)
                Session("oAnagrafe") = oDettaglioAnagrafica

                VisualDati(oDettaglioAnagrafica)

                ' Pulisco la variabile di sessione della anagrafica di anater.
                Session.Remove("AnagrafeAnaterRibaltata")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.DatiContribuente.btnRibaltaAnagAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
        Dim sScript As String

        sScript = "ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
        RegisterScript( sScript,Me.GetType)
    End Sub

    Private Sub VisualDati(ByVal oMyAnag As DettaglioAnagrafica)
        Try
            If oMyAnag.COD_CONTRIBUENTE > 0 Then
                TxtCodContribuente.Text = oMyAnag.COD_CONTRIBUENTE
                TxtIdDataAnagrafica.Text = oMyAnag.ID_DATA_ANAGRAFICA
                If oMyAnag.Cognome <> "" Then
                    LblNominativo.Text = oMyAnag.Cognome
                    LblNominativo.Text += Space(3) & oMyAnag.Nome
                Else
                    LblNominativo.Text = "ANAGRAFICA DA ASSOCIARE"
                End If
                If oMyAnag.PartitaIva <> "" Then
                    LblNascita.Text = "Cod.Fiscale/P.Iva:" & Space(3) & oMyAnag.PartitaIva
                Else
                    LblNascita.Text = "Cod.Fiscale/P.Iva:" & Space(3) & oMyAnag.CodiceFiscale
                End If
                If oMyAnag.ComuneNascita <> "" Then
                    LblNascita.Text += Space(20) & "nato/a a" & Space(3) & oMyAnag.ComuneNascita
                Else
                    LblNascita.Text += Space(20) & "nato/a"
                End If
                If oMyAnag.DataNascita <> "00/00/1900" Then
                    LblNascita.Text += Space(20) & "il" & Space(3) & oMyAnag.DataNascita
                End If
                LblResidenza.Text = "Residente in:" & Space(3) & oMyAnag.ViaResidenza + ", " + oMyAnag.CivicoResidenza
                LblResidenza.Text += Space(3) & " - " & oMyAnag.CapResidenza + Space(3) + oMyAnag.ComuneResidenza + " (" + oMyAnag.ProvinciaResidenza + ")"
            Else
                LblNominativo.Text = "" : LblNascita.Text = "" : LblResidenza.Text = ""
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.DatiContribuente.VisualDati.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub LnkPulisciContr_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkPulisciContr.Click
        LblNominativo.Text = "" : LblNascita.Text = "" : LblResidenza.Text = ""
        TxtCodContribuente.Text = -1 : TxtIdDataAnagrafica.Text = -1
    End Sub
End Class
