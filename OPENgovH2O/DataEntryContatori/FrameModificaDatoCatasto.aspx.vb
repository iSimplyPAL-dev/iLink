Imports OPENgov_AgenziaEntrate
Imports OPENUtility
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class FrameModificaDatoCatasto
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(FrameModificaDatoCatasto))

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
    'Try
    '    'Put user code to initialize the page here
    '    info.Text = "Acquedotto - " & Session("precedentecatasto") & " - Gestione " & Session("precedentecatasto") & " - Gestione Dati Catastali"

    '    Dim DEContatori As GestContatori = New GestContatori
    '    Dim detailsCatasto() As objDatiCatastali

    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    '    Dim oLoadCombo As New ClsGenerale.Generale

    '    Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
    '    Dim dvDati As DataView

    '    WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG"))
    '    If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        Throw New Exception("LoadContatori::" & "Errore durante l'apertura della sessione di WorkFlow")
    '        Exit Sub
    '    End If

    '    dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", MyUtility.TRIBUTO_H2O, WFSessione)
    '    oLoadCombo.loadCombo(ddlTipoParticella, dvDati)
    '    ddlTipoParticella.SelectedValue = "E"
    '    ddlTipoParticella.Enabled = False

    '    If Not IsNothing(Session("datacatasto")) Then

    '        '================================================
    '        'POPOLAZIONE OGGETTO DA DATATABLE
    '        '================================================

    '        Dim mioData As DataTable = New DataTable

    '        mioData = CType(Session("datacatasto"), System.Data.DataTable)


    '        Dim row() As DataRow

    '        row = mioData.Select("IDCONT_CATAS='" & CInt(Request.Params("IDCatasto")) & "'")

    '        If Not Page.IsPostBack Then
    '            If Not IsDBNull(row(0)("SEZIONE")) Then
    '                txtSezione.Text = row(0)("SEZIONE")
    '            End If
    '            If Not IsDBNull(row(0)("ESTENSIONE_PARTICELLA")) Then
    '                txtEstParticella.Text = row(0)("ESTENSIONE_PARTICELLA")
    '            End If
    '            If IsDBNull(row(0)("ID_TIPO_PARTICELLA")) Then
    '                ddlTipoParticella.SelectedValue = "E"
    '            Else
    '                ddlTipoParticella.SelectedValue = row(0)("ID_TIPO_PARTICELLA").Substring(0, 1)
    '            End If
    '            If Not IsDBNull(row(0)("FOGLIO")) Then
    '                txtFoglio.Text = row(0)("FOGLIO")
    '            End If
    '            If Not IsDBNull(row(0)("NUMERO")) Then
    '                txtNumero.Text = row(0)("NUMERO")
    '            End If
    '            If Not IsDBNull(row(0)("SUBALTERNO")) Then
    '                txtSubalterno.Text = row(0)("SUBALTERNO")
    '            End If
    '            If Not IsDBNull(row(0)("INTERNO")) Then
    '                txtInterno.Text = row(0)("INTERNO")
    '            End If
    '            If Not IsDBNull(row(0)("PIANO")) Then
    '                txtPiano.Text = row(0)("PIANO")
    '            End If
    '        End If
    '        '================================================
    '        'FINE POPOLAZIONE OGGETTO DA DATATABLE
    '        '================================================
    '    Else
    '        '==========================================
    '        'POPOLAZIONE OGGETTO DA DATABASE
    '        '==========================================
    '        If Not Page.IsPostBack Then
    '            detailsCatasto = DEContatori.GetDetailsCatasto(Request.Params("IDCatasto"), -1, WFSessione)
    '            txtFoglio.Text = detailsCatasto(0).sFoglio
    '            txtNumero.Text = detailsCatasto(0).sNumero
    '            txtSubalterno.Text = detailsCatasto(0).nSubalterno
    '            txtInterno.Text = detailsCatasto(0).sInterno
    '            txtPiano.Text = detailsCatasto(0).sPiano
    '            txtSezione.Text = detailsCatasto(0).sSezione
    '            txtEstParticella.Text = detailsCatasto(0).sEstensioneParticella
    '            ddlTipoParticella.SelectedValue = detailsCatasto(0).sIdTipoParticella
    '        End If
    '        '==========================================
    '        'FINE POPOLAZIONE OGGETTO DA DATABASE
    '        '==========================================
    '    End If

    '    If txtSubalterno.Text = "-1" Then
    '        txtSubalterno.Text = ""
    '    End If
    ' Catch ex As Exception

    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameModificaDatoCatastato.Page_Load.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    '      End Try

    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim DEContatori As New GestContatori
            Dim detailsCatasto() As objDatiCatastali

            Dim oLoadCombo As New ClsGenerale.Generale

            Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
            Dim dvDati As DataView

            info.Text = "Acquedotto - " & Session("precedentecatasto") & " - Gestione " & Session("precedentecatasto") & " - Gestione Dati Catastali"

            'Dim WFSessione As OPENUtility.CreateSessione
            'Dim WFErrore As String
            'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG"))
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("LoadContatori::" & "Errore durante l'apertura della sessione di WorkFlow")
            '    Exit Sub
            'End If
            'dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", MyUtility.TRIBUTO_H2O, WFSessione)
            dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "9000", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(ddlTipoParticella, dvDati)
            ddlTipoParticella.SelectedValue = "E"
            ddlTipoParticella.Enabled = False

            If Not IsNothing(Session("datacatasto")) Then
                '================================================
                'POPOLAZIONE OGGETTO DA DATATABLE
                '================================================
                Dim mioData As New DataTable
                mioData = CType(Session("datacatasto"), System.Data.DataTable)

                Dim row() As DataRow
                row = mioData.Select("IDCONT_CATAS='" & CInt(Request.Params("IDCatasto")) & "'")

                If Not Page.IsPostBack Then
                    If Not IsDBNull(row(0)("SEZIONE")) Then
                        txtSezione.Text = row(0)("SEZIONE")
                    End If
                    If Not IsDBNull(row(0)("ESTENSIONE_PARTICELLA")) Then
                        txtEstParticella.Text = row(0)("ESTENSIONE_PARTICELLA")
                    End If
                    If IsDBNull(row(0)("ID_TIPO_PARTICELLA")) Then
                        ddlTipoParticella.SelectedValue = "E"
                    Else
                        ddlTipoParticella.SelectedValue = row(0)("ID_TIPO_PARTICELLA").Substring(0, 1)
                    End If
                    If Not IsDBNull(row(0)("FOGLIO")) Then
                        txtFoglio.Text = row(0)("FOGLIO")
                    End If
                    If Not IsDBNull(row(0)("NUMERO")) Then
                        txtNumero.Text = row(0)("NUMERO")
                    End If
                    If Not IsDBNull(row(0)("SUBALTERNO")) Then
                        txtSubalterno.Text = row(0)("SUBALTERNO")
                    End If
                    If Not IsDBNull(row(0)("INTERNO")) Then
                        txtInterno.Text = row(0)("INTERNO")
                    End If
                    If Not IsDBNull(row(0)("PIANO")) Then
                        txtPiano.Text = row(0)("PIANO")
                    End If
                End If
                '================================================
                'FINE POPOLAZIONE OGGETTO DA DATATABLE
                '================================================
            Else
                '==========================================
                'POPOLAZIONE OGGETTO DA DATABASE
                '==========================================
                If Not Page.IsPostBack Then
                    detailsCatasto = DEContatori.GetDetailsCatasto(Request.Params("IDCatasto"), -1)
                    txtFoglio.Text = detailsCatasto(0).sFoglio
                    txtNumero.Text = detailsCatasto(0).sNumero
                    txtSubalterno.Text = detailsCatasto(0).nSubalterno
                    txtInterno.Text = detailsCatasto(0).sInterno
                    txtPiano.Text = detailsCatasto(0).sPiano
                    txtSezione.Text = detailsCatasto(0).sSezione
                    txtEstParticella.Text = detailsCatasto(0).sEstensioneParticella
                    ddlTipoParticella.SelectedValue = detailsCatasto(0).sIdTipoParticella
                End If
                '==========================================
                'FINE POPOLAZIONE OGGETTO DA DATABASE
                '==========================================
            End If

            If txtSubalterno.Text = "-1" Then
                txtSubalterno.Text = ""
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameModificaDatoCatastato.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEvento.Click
        Try
            Dim foglio As String = CStr(txtFoglio.Text.Replace("'", ""))
            Dim numero As String = CStr(txtNumero.Text.Replace("'", ""))
            Dim subalterno As String = CStr(txtSubalterno.Text)
            Dim interno As String = CStr(txtInterno.Text.Replace("'", ""))
            Dim piano As String = CStr(txtPiano.Text.Replace("'", ""))
            Dim sezione As String = CStr(txtSezione.Text.Replace("'", ""))
            Dim estensioneParticella As String = CStr(txtEstParticella.Text.Replace("'", ""))
            Dim tipoParticella As String = CStr(ddlTipoParticella.SelectedValue)
            Dim MyIDCatasto As Integer = Request.Params("IDCatasto")
            Dim myIdContatore As Integer = Request.Params("IDContatore")
            Dim DBContatori As GestContatori = New GestContatori
            Dim sScript As String = ""

            If DBContatori.SetDatiCatastali(interno, piano, foglio, numero, subalterno, MyIDCatasto, myIdContatore, sezione, estensioneParticella, tipoParticella) > 0 Then
                sScript += "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente!');"
                sScript += "window.opener.location.href='searchResultsCatasto.aspx?ContatoreID=" & Request.Params("IDContatore") & "';"
                sScript += "window.close();"
            Else
                sScript += "GestAlert('a', 'danger', '', '', 'Si è verificato un\'errore.');"
            End If
            RegisterScript(sscript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameModificaDatoCatastato.btnEvento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Try
            Dim MyIDCatasto As Integer = Request.Params("IDCatasto")
            Dim DBContatori As New GestContatori
            Dim sScript As String = ""

            DBContatori.EliminaDatiCatastali(MyIDCatasto)
            sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente');"
            sScript += "window.opener.location.href='searchResultsCatasto.aspx?ContatoreID=" & Request.Params("IDContatore") & "';"
            sScript += "window.close();"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameModificaDatoCatastato.btnElimina_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class

Public Class detailCatasto
    Public foglio As String
    Public numero As String
    Public subalterno As String
    Public interno As String
    Public piano As String
    Public sezione As String
    Public estensioneParticella As String
    Public tipoParticella As String
End Class