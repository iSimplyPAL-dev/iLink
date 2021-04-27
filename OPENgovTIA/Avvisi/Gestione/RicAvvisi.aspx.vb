Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la ricerca avvisi.
''' Contiene i parametri di ricerca e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' Al fianco della data di emissione dell’avviso saranno visualizzate anche le date di notifica e accertamento quando valorizzate. Le spese di spedizione saranno visualizzate nel dettaglio avviso. La stampa produrrà lo stesso documento e gli stessi bollettini prodotti in fase di iter ruolo. Lo sgravio sarà possibile fintanto che non ci sarà la data di accertamento valorizzata.
''' </revision>
''' </revisionHistory>
Partial Class RicAvvisi
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicAvvisi))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label

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
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Dim oMyTotRuolo() As ObjRuolo

        Try
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If
            hfIsFromVariabile.Value = Request.Item("IsFromVariabile")
            Session("oAnagrafe") = Nothing
            TxtCognome.Attributes.Add("onkeydown", "keyPress();")
            TxtNome.Attributes.Add("onkeydown", "keyPress();")
            TxtCFPIVA.Attributes.Add("onkeydown", "keyPress();")
            TxtCodCartella.Attributes.Add("onkeydown", "keyPress();")
            DdlAnno.Attributes.Add("onkeydown", "keyPress();")
            DdlTipoRuolo.Attributes.Add("onkeydown", "keyPress();")
            DdlNProgRuolo.Attributes.Add("onkeydown", "keyPress();")
            'Put user code to initialize the page here
            If Page.IsPostBack = False Then
                oMyTotRuolo = Session("oRuoloTIA")
                'popolo combo Anno
                sSQL = "SELECT *"
                sSQL += " FROM V_GETANNIRUOLO"
                sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
                sSQL += " ORDER BY DESCRIZIONE DESC"
                oLoadCombo.LoadComboGenerale(DdlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                'popolo combo tiporuolo
                sSQL = "SELECT DISTINCT DESCRIZIONE, IDTIPORUOLO, ORDINAMENTO"
                sSQL += " FROM V_GETTIPORUOLO"
                If ConstSession.HasNotifiche = False Then
                    sSQL += " WHERE IDTIPORUOLO<>'I'"
                End If
                sSQL += " ORDER BY ORDINAMENTO"
                oLoadCombo.LoadComboGenerale(DdlTipoRuolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                If Not IsNothing(oMyTotRuolo) Then
                    TxtIdElab.Text = oMyTotRuolo(0).IdFlusso
                    DdlAnno.SelectedValue = oMyTotRuolo(0).sAnno
                    DdlTipoRuolo.SelectedValue = oMyTotRuolo(0).sTipoRuolo
                    DdlAnno.Enabled = False
                    DdlTipoRuolo.Enabled = False
                End If
                If Not Session("mySearchForAvvisi") Is Nothing Then
                    Dim mySearchForAvvisi As ArrayList
                    Dim x As Integer = 0
                    mySearchForAvvisi = CType(Session("mySearchForAvvisi"), ArrayList)
                    If mySearchForAvvisi(x) <> "" Then
                        DdlAnno.SelectedValue = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        DdlTipoRuolo.SelectedValue = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        TxtCognome.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        TxtNome.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        TxtCFPIVA.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        TxtCodCartella.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        ChkSgravate.Checked = CBool(mySearchForAvvisi(x))
                    End If
                    Dim sScript As String
                    sScript = "Search();"
                    RegisterScript(sScript, Me.GetType)

                    Session("mySearchForAvvisi") = Nothing
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Sgravio, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicAvvisi.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sSQL As String
    '    Dim oLoadCombo As New generalClass.generalFunction
    '    Dim oMyTotRuolo() As ObjRuolo

    '    Try
    '        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
    '        End If
    '        Session("oAnagrafe") = Nothing
    '        TxtCognome.Attributes.Add("onkeydown", "keyPress();")
    '        TxtNome.Attributes.Add("onkeydown", "keyPress();")
    '        TxtCFPIVA.Attributes.Add("onkeydown", "keyPress();")
    '        TxtCodCartella.Attributes.Add("onkeydown", "keyPress();")
    '        DdlAnno.Attributes.Add("onkeydown", "keyPress();")
    '        DdlTipoRuolo.Attributes.Add("onkeydown", "keyPress();")
    '        DdlNProgRuolo.Attributes.Add("onkeydown", "keyPress();")
    '        'Put user code to initialize the page here
    '        If Page.IsPostBack = False Then
    '            oMyTotRuolo = Session("oRuoloTIA")
    '            'popolo combo Anno
    '            'sSQL = "SELECT DISTINCT ANNO, ANNO AS IDANNO"
    '            'sSQL += "  FROM TBLTARIFFE"
    '            'sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
    '            'sSQL += " ORDER BY IDANNO DESC"
    '            sSQL = "SELECT *"
    '            sSQL += " FROM V_GETANNIRUOLO"
    '            sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
    '            sSQL += " ORDER BY DESCRIZIONE DESC"
    '            oLoadCombo.LoadComboGenerale(DdlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
    '            'popolo combo tiporuolo
    '            sSQL = "SELECT DISTINCT DESCRIZIONE, IDTIPORUOLO, ORDINAMENTO"
    '            sSQL += " FROM V_GETTIPORUOLO"
    '            If ConstSession.HasNotifiche = False Then
    '                sSQL += " WHERE IDTIPORUOLO<>'I'"
    '            End If
    '            sSQL += " ORDER BY ORDINAMENTO"
    '            oLoadCombo.LoadComboGenerale(DdlTipoRuolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
    '            If Not IsNothing(oMyTotRuolo) Then
    '                TxtIdElab.Text = oMyTotRuolo(0).IdFlusso
    '                DdlAnno.SelectedValue = oMyTotRuolo(0).sAnno
    '                DdlTipoRuolo.SelectedValue = oMyTotRuolo(0).sTipoRuolo
    '                DdlAnno.Enabled = False
    '                DdlTipoRuolo.Enabled = False
    '            End If
    '            If Not Session("mySearchForAvvisi") Is Nothing Then
    '                Dim mySearchForAvvisi As ArrayList
    '                Dim x As Integer = 0
    '                mySearchForAvvisi = CType(Session("mySearchForAvvisi"), ArrayList)
    '                If mySearchForAvvisi(x) <> "" Then
    '                    DdlAnno.SelectedValue = mySearchForAvvisi(x)
    '                End If
    '                x += 1
    '                If mySearchForAvvisi(x) <> "" Then
    '                    DdlTipoRuolo.SelectedValue = mySearchForAvvisi(x)
    '                End If
    '                x += 1
    '                If mySearchForAvvisi(x) <> "" Then
    '                    TxtCognome.Text = mySearchForAvvisi(x)
    '                End If
    '                x += 1
    '                If mySearchForAvvisi(x) <> "" Then
    '                    TxtNome.Text = mySearchForAvvisi(x)
    '                End If
    '                x += 1
    '                If mySearchForAvvisi(x) <> "" Then
    '                    TxtCFPIVA.Text = mySearchForAvvisi(x)
    '                End If
    '                x += 1
    '                If mySearchForAvvisi(x) <> "" Then
    '                    TxtCodCartella.Text = mySearchForAvvisi(x)
    '                End If
    '                x += 1
    '                If mySearchForAvvisi(x) <> "" Then
    '                    ChkSgravate.Checked = CBool(mySearchForAvvisi(x))
    '                End If
    '                Dim sScript As String
    '                sScript = "Search();"
    '                RegisterScript(sScript, Me.GetType)

    '                Session("mySearchForAvvisi") = Nothing
    '            End If
    '            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Sgravio, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, -1)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicAvvisi.Page_Load.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sNameXLS As String
        Dim DvDati As New DataView
        Dim FncAvvisi As New GestAvviso
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        nCol = 13
        Try
            DvDati = FncAvvisi.GetStampaAvvisi(ConstSession.StringConnection, ConstSession.IdEnte, DdlAnno.SelectedValue, DdlTipoRuolo.SelectedValue, TxtCognome.Text, TxtNome.Text, TxtCFPIVA.Text, TxtCodCartella.Text, ChkSgravate.Checked, "")
            DtDatiStampa = FncStampa.PrintAvvisi(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES, nCol)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicAvvisi.CmdStampa_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DvDati.Dispose()
        End Try

        If Not DtDatiStampa Is Nothing Then
            sNameXLS = ConstSession.IdEnte & "_ELENCO_AVVISI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        Else
            Dim sScript As String = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType)
        End If
    End Sub
End Class
