Imports log4net
Imports OPENgov_AgenziaEntrate
Imports OPENUtility

Partial Class FrameInserisciDatoCatasto
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("FrameInserisciDatoCatasto")


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnElimina As System.Web.UI.WebControls.Button
    Protected WithEvents LnkNewUIAnater As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdRibaltaUIAnater As System.Web.UI.WebControls.Button

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
        info.Text = "Acquedotto - " & Session("precedentecatasto") & " - Gestione " & Session("precedentecatasto") & " - Gestione Dati Catastali"
        Try
            If Not Page.IsPostBack Then
                '***** GESTIONE DATI AGENZIA ENTRATE ******
                Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
                Dim dvDati As New DataView
                'Dim WFSessione As OPENUtility.CreateSessione
                'Dim WFErrore As String
                Dim oLoadCombo As New ClsGenerale.Generale

                'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG"))
                'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
                '    Throw New Exception("LoadCatasto::" & "Errore durante l'apertura della sessione di WorkFlow")
                '    Exit Sub
                'End If

                'dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "9000", WFSessione)
                dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "9000", ConstSession.StringConnectionOPENgov)
                oLoadCombo.loadCombo(ddlTipoParticella, dvDati)
                ddlTipoParticella.SelectedValue = "E"
                ddlTipoParticella.Enabled = False
                '**** FINE CARICAMENTO DATI PER AGENZIA ENTRATE **** 
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameInserisciDatoCatastato.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEvento.Click
        Try
            Dim mioData As DataTable
            mioData = CType(Session("datacatasto"), System.Data.DataTable)
            Dim mioRiga As DataRow

            Dim DBContatori As GestContatori = New GestContatori
            If CInt(Request.Params("IDContatore")) <> 0 Then
                DBContatori.SetDatiCatastali(CStr(txtInterno.Text.Replace("'", "")), CStr(txtPiano.Text.Replace("'", "")), CStr(txtFoglio.Text.Replace("'", "")), CStr(txtNumero.Text.Replace("'", "")), CStr(txtSubalterno.Text), -1, CInt(Request.Params("IDContatore")), CStr(txtSezione.Text.Replace("'", "''")), CStr(txtEstParticella.Text.Replace("'", "''")), CStr(ddlTipoParticella.SelectedValue))
                RegisterScript("GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente');", Me.GetType)

                mioRiga = mioData.NewRow()

                mioRiga.Item(1) = mioData.Rows.Count + 1
                mioRiga.Item(2) = CStr(txtInterno.Text)
                mioRiga.Item(3) = CStr(txtPiano.Text)
                mioRiga.Item(4) = CStr(txtFoglio.Text)
                mioRiga.Item(5) = CStr(txtNumero.Text)
                If txtSubalterno.Text <> "" Then
                    mioRiga.Item(6) = CStr(txtSubalterno.Text)
                Else
                    mioRiga.Item(6) = -1
                End If

                '*** salvataggio dati agenzia entrate
                mioRiga.Item(7) = CStr(txtSezione.Text)
                mioRiga.Item(8) = CStr(txtEstParticella.Text)
                mioRiga.Item(9) = CStr(ddlTipoParticella.SelectedValue)
                '*** /salvataggio dati agenzia entrate

                mioData.Rows.Add(mioRiga)

                Session("datacatasto") = mioData

            Else

                mioRiga = mioData.NewRow()

                '====================================

                'Dim row() As DataRow

                'row = mioData.Select("IDCONT_CATAS='" & CInt(Request.Params("IDCatasto")) & "'")

                'Dim miaprova As String = row(0)("PIANO")

                'detailsCatasto.foglio = row(0)("FOGLIO")
                '====================================
                If mioData.Rows.Count = 0 Then
                    mioRiga.Item(0) = 1
                Else
                    mioRiga.Item(0) = CInt(CType(mioData.Rows(mioData.Rows.Count - 1), System.Data.DataRow).ItemArray(0)) + 1
                End If

                mioRiga.Item(1) = 1
                mioRiga.Item(2) = CStr(txtInterno.Text)
                mioRiga.Item(3) = CStr(txtPiano.Text)
                mioRiga.Item(4) = CStr(txtFoglio.Text)
                mioRiga.Item(5) = CStr(txtNumero.Text)
                If txtSubalterno.Text <> "" Then
                    mioRiga.Item(6) = CStr(txtSubalterno.Text)
                Else
                    mioRiga.Item(6) = -1
                End If

                '*** salvataggio dati agenzia entrate
                mioRiga.Item(7) = CStr(txtSezione.Text)
                mioRiga.Item(8) = CStr(txtEstParticella.Text)
                mioRiga.Item(9) = CStr(ddlTipoParticella.SelectedValue)
                '*** /salvataggio dati agenzia entrate
                mioData.Rows.Add(mioRiga)

                'caricamento del valore del tipo particella per la visualizzazione nella griglia
                DBContatori.GetTipoParticella(mioData)

                'Session("datacatasto") = mioData
                RegisterScript("GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente');", Me.GetType)
                'Response.Write("script language='javascript' type='text/javascript'>alert(parent.opener.loadGrid.document.getElementById('nullo'));")
                'Response.Write("<script language='javascript' type='text/javascript'>parent.opener.loadGrid.nullo.click();")
                'Response.Write("<script language='javascript' type='text/javascript'>window.opener.loadGrid.location.reload();")
                'Response.Write("<script language='javascript' type='text/javascript'>window.close();")
            End If
            'Response.Write("<script language='javascript' type='text/javascript'>window.opener.loadGrid.location.reload();")
            Response.Write("<script language='javascript' type='text/javascript'>window.opener.document.getElementById('loadGrid').src='searchResultsCatasto.aspx?ContatoreID=" & Request.Params("IDContatore") & "';")
            Response.Write("<script language='javascript' type='text/javascript'>window.close();")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameInserisciDatoCatastato.btnEvento_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
