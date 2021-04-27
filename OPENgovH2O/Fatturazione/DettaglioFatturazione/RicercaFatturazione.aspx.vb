Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class RicercaFatturazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaFatturazione))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents PaginaChiamante As System.Web.UI.WebControls.TextBox

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
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
        Dim paginacomandi As String = Request("paginacomandi")
        Dim parametri As String
        parametri = "?title=Acquedotto - Fatturazione - Dettaglio&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&Provenienza=" & Request.Item("Provenienza")
        If Len(paginacomandi) = 0 Then
            paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/Fatturazione/DettaglioFatturazione/ComandiRicFatturazione.aspx"
        End If
        dim sScript as string=""
        sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
        sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
        RegisterScript(sScript , Me.GetType())

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
        Try
            Dim FncGen As New ClsGenerale.Generale
            FncGen.GetPeriodoAttuale
            dim sSQL as string
            Dim oLoadCombo As New ClsGenerale.Generale
            Dim oMyRicerca As New ObjRicercaDoc

            DdlPeriodo.Attributes.Add("onkeydown", "keyPress();")
            DdlTipoDoc.Attributes.Add("onkeydown", "keyPress();")
            TxtCognome.Attributes.Add("onkeydown", "keyPress();")
            TxtNome.Attributes.Add("onkeydown", "keyPress();")
            TxtCfPIva.Attributes.Add("onkeydown", "keyPress();")
            TxtDataDoc.Attributes.Add("onkeydown", "keyPress();")
            TxtNDoc.Attributes.Add("onkeydown", "keyPress();")
            TxtMatricola.Attributes.Add("onkeydown", "keyPress();")
            If Page.IsPostBack = False Then
                TxtProvenienza.Text = Request.Item("Provenienza")
                'carico gli anni a ruolo
                sSQL = "SELECT PERIODO, CODPERIODO"
                sSQL += " FROM TP_PERIODO "
                sSQL += " WHERE (COD_ENTE='" & ConstSession.IdEnte & "')"
                sSQL += " ORDER BY CODPERIODO"
                oLoadCombo.LoadComboGenerale(DdlPeriodo, sSQL)
                'carico i tipi ruolo
                sSQL = "SELECT 'FATTURA','F'"
                sSQL += " UNION"
                sSQL += " SELECT 'NOTA DI CREDITO','N'"
                oLoadCombo.LoadComboGenerale(DdlTipoDoc, sSQL)
                'controllo se l'oggetto ricerca è valorizzato
                If Not Session("oRicercaDoc") Is Nothing Then
                    'carico la ricerca già effettuata
                    oMyRicerca = Session("oRicercaDoc")
                    If oMyRicerca.nPeriodo <> -1 Then
                        DdlPeriodo.SelectedValue = oMyRicerca.nPeriodo
                    End If
                    If oMyRicerca.sTipoDocumento <> "" Then
                        DdlTipoDoc.SelectedValue = oMyRicerca.sTipoDocumento
                    End If
                    If oMyRicerca.sCognome <> "" Then
                        TxtCognome.Text = oMyRicerca.sCognome
                    End If
                    If oMyRicerca.sNome <> "" Then
                        TxtNome.Text = oMyRicerca.sNome
                    End If
                    If oMyRicerca.sCFPIva <> "" Then
                        TxtCfPIva.Text = oMyRicerca.sCFPIva
                    End If
                    If oMyRicerca.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        TxtDataDoc.Text = oMyRicerca.tDataDocumento
                    End If
                    If oMyRicerca.sNDocumento <> "" Then
                        TxtNDoc.Text = oMyRicerca.sNDocumento
                    End If
                    If oMyRicerca.sMatricola <> "" Then
                        TxtMatricola.Text = oMyRicerca.sMatricola
                    End If
                    'ricarico la ricerca
                    sScript = "Search();"
                    RegisterScript(sScript, Me.GetType())
                ElseIf Request.Item("IdPeriodo") <> "" Then
                    oMyRicerca.sProvenienza = Request.Item("Provenienza")
                    oMyRicerca.nPeriodo = Request.Item("IdPeriodo")
                    DdlPeriodo.SelectedValue = oMyRicerca.nPeriodo
                    'ricarico la ricerca
                    sScript = "Search();"
                    RegisterScript(sScript, Me.GetType())
                End If
                If Request.Item("Provenienza") = "E" Then
                    DdlPeriodo.Enabled = False
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaFatturazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdClearDati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdClearDati.Click
        Try
            'aggiorno la pagina chiamante
            Dim sScript As String
            'ripulisco tutti i dati di sessioni
            Session("oRicercaDoc") = Nothing
            'richiamo le relative pagine
            sScript = "parent.Visualizza.location.href='../Fatturazione.aspx?paginacomandi=ComandiFatturazione.aspx';" & vbCrLf
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaFatturazione.CmdClearDati_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
