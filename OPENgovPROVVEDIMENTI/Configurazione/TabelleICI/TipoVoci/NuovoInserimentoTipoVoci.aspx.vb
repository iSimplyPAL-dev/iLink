Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per l'inserimento tipo voci.
''' Contiene i parametri di definizione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class NuovoInserimentoTipoVoci
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(NuovoInserimentoTipoVoci))

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
    Dim CODTRIBUTO, CODCAPITOLO, CODTIPOPROVVEDIMENTO, DESCVOCE, CODMISURA, CODCALCOLATO, CODVOCE, CODFASE, VOCEATTRIBUITA, IDTIPOVOCE As String
    Dim CODTRIBUTO_OLD, CODCAPITOLO_OLD, CODTIPOPROVVEDIMENTO_OLD, CODVOCE_OLD As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""

        Try
            sScript = "parent.Comandi.location.href='ComandiInserimentoTipoVoci.aspx';"
            RegisterScript(sScript, Me.GetType())

            sScript = ""
            CODTRIBUTO = Request.Item("CODTRIBUTO")
            CODCAPITOLO = Request.Item("CODCAPITOLO")
            CODTIPOPROVVEDIMENTO = Request.Item("CODTIPOPROVVEDIMENTO")
            CODMISURA = Request.Item("CODMISURA")
            'CODCALCOLATO = Request.Item("CODCALCOLATO")
            If Not Request.Item("IDTIPOVOCE") Is Nothing Then
                IDTIPOVOCE = Request.Item("IDTIPOVOCE")
            Else
                IDTIPOVOCE = ""
            End If
            CODVOCE = Request.Item("CODVOCE")
            CODFASE = Request.Item("CODFASE")
            VOCEATTRIBUITA = Request.Item("VOCEATTRIBUITA")
            'Setto l'id univoco della voce
            txtIDTIPOVOCE.Text = IDTIPOVOCE

            If txtIDTIPOVOCE.Text <> "" Then
                CODTRIBUTO_OLD = CODTRIBUTO
                CODCAPITOLO_OLD = CODCAPITOLO
                CODTIPOPROVVEDIMENTO_OLD = CODTIPOPROVVEDIMENTO
                CODVOCE_OLD = CODVOCE
            Else
                CODTRIBUTO_OLD = ""
                CODCAPITOLO_OLD = ""
                CODTIPOPROVVEDIMENTO_OLD = ""
                CODVOCE_OLD = ""
            End If

            If Not Page.IsPostBack Then
                If txtIDTIPOVOCE.Text <> "" Then
                    sScript += "DisabilitaCombo();"
                Else
                    sScript += "AbilitaCombo();"
                    sScript += "window.setTimeout('disabilitaConfigura()',1000);"
                End If

                If CODTRIBUTO.CompareTo("-1") = 0 Then
                    PopolaComboTributo()
                Else
                    PopolaComboTributo()
                    PopolaComboCapitolo()
                    PopolaCombo()
                    txtVoceAttribuita.Text = VOCEATTRIBUITA
                    ddlTributo.SelectedValue = CODTRIBUTO
                    'sscript+= "ifrmConfigura.location.href='configuraVoci.aspx?CODTRIBUTO=" & CODTRIBUTO & "&CODCAPITOLO=" & CODCAPITOLO & "&CODTIPOPROVVEDIMENTO=" & CODTIPOPROVVEDIMENTO & "&CODMISURA=" & CODMISURA & "&CODCALCOLATO=" & CODCALCOLATO & "&CODVOCE=" & CODVOCE & "'" & vbCrLf
                End If
            Else
                CODTRIBUTO = ddlTributo.SelectedItem.Value()
                CODCAPITOLO = -1
                CODTIPOPROVVEDIMENTO = -1
                CODMISURA = -1
                'CODCALCOLATO = -1
                CODVOCE = -1
                CODFASE = -1

                'PopolaCombo()
                'PopolaComboCapitolo()
            End If
            'sscript+= "parent.Comandi.location.href='ComandiInserimentoTipoVoci.aspx';" & vbCrLf
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.Page_Load.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaCombo()
        Dim FncMyUtility As New MyUtility
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            '08/02/2008 Dipe decommentato popolamento combo campitolo
            ddlCapitolo.Items.Clear()
            FncMyUtility.FillDropDownSQLValueString(ddlCapitolo, objGestOPENgovProvvedimenti.GetCapitoli(CODTRIBUTO, "", "", cmdMyCommand), CODCAPITOLO, "...")

            ddlProvvedimenti.Items.Clear()
            FncMyUtility.FillDropDownSQL(ddlProvvedimenti, objGestOPENgovProvvedimenti.GetTipoProvvedimento(CODTRIBUTO, "", "", "VisualizzaValoriVoci", cmdMyCommand), CODTIPOPROVVEDIMENTO, "...")

            ddlVoce.Items.Clear()
            FncMyUtility.FillDropDownSQLValueStringCodDesc(ddlVoce, objGestOPENgovProvvedimenti.GetTipologieSanzioni(CODTRIBUTO, "", "", cmdMyCommand), CODVOCE, "...")

            ddlMisura.Items.Clear()
            FncMyUtility.FillDropDownSQLValueString(ddlMisura, objGestOPENgovProvvedimenti.GetTipoMisura("", "", cmdMyCommand), CODMISURA, "...")

            ddlFase.Items.Clear()
            FncMyUtility.FillDropDownSQLValueString(ddlFase, objGestOPENgovProvvedimenti.GetFase("", "", cmdMyCommand), CODFASE, "...")

            If CODTRIBUTO = Utility.Costanti.TRIBUTO_ICI Or CODTRIBUTO = Utility.Costanti.TRIBUTO_OSAP Then
                ddlFase.Enabled = True
            Else
                ddlFase.Enabled = False
            End If
            'ddlFase.Items.Clear()
            'Utility.FillDropDownSQLValueString(ddlFase, objGestOPENgovProvvedimenti.GetFase("", ""), CODFASE, "...")

            'ddlCalcolata.Items.Clear()
            'Utility.FillDropDownSQL(ddlCalcolata, objGestOPENgovProvvedimenti.GetTipoBaseCalcolo("", ""), CODCALCOLATO, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.PopolaCombo.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboCapitolo()
        Dim Utility As New MyUtility
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            'ddlCapitolo.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlCapitolo, objGestOPENgovProvvedimenti.GetCapitoli(CODTRIBUTO, "", "", cmdMyCommand), CODCAPITOLO, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.PopolaComboCapitolo.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboTributo()
        Dim Utility As New MyUtility
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            ddlTributo.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlTributo, objGestOPENgovProvvedimenti.GetTributi("", "", cmdMyCommand), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.PopolaComboTributo.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        ddlCapitolo.Items.Clear()
        ddlMisura.Items.Clear()
        ddlProvvedimenti.Items.Clear()
        ddlVoce.Items.Clear()
        ddlFase.Items.Clear()
        ddlFase.Enabled = True

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim sScript As String = ""
        'Dim strCapitolo, strFase, strMisura, strProvvedimenti,
        'strTributo, strVoce, strVoceAttribuita As String
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objDSTipoVoci As DataSet
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String

        Try
            CODTRIBUTO = ddlTributo.SelectedItem.Value 'Request.Item("ddlTributo")
            CODCAPITOLO = ddlCapitolo.SelectedItem.Value 'Request.Item("ddlCapitolo")
            CODFASE = ddlFase.SelectedItem.Value  'Request.Item("ddlFase")
            CODMISURA = ddlMisura.SelectedItem.Value  'Request.Item("ddlMisura")
            CODTIPOPROVVEDIMENTO = ddlProvvedimenti.SelectedItem.Value  'Request.Item("ddlProvvedimenti")

            CODVOCE = ddlVoce.SelectedItem.Value  'Request.Item("ddlVoce")
            DESCVOCE = ""
            VOCEATTRIBUITA = txtVoceAttribuita.Text
            If IsNumeric(IDTIPOVOCE) = False Then
                IDTIPOVOCE = "-1"
            End If

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
            objHashTable.Add("CODCAPITOLO", CODCAPITOLO)
            objHashTable.Add("IDTIPOVOCE", IDTIPOVOCE)
            objHashTable.Add("CODVOCE", CODVOCE)
            objHashTable.Add("CODPROVVEDIMENTI", CODTIPOPROVVEDIMENTO)
            objHashTable.Add("CODMISURA", CODMISURA)
            objHashTable.Add("CODFASE", CODFASE)
            objHashTable.Add("VOCEATTRIBUITA", VOCEATTRIBUITA)
            objHashTable.Add("DESCVOCE", DESCVOCE)

            objHashTable.Add("CODENTE_OLD", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO_OLD", CODTRIBUTO_OLD)
            objHashTable.Add("CODCAPITOLO_OLD", CODCAPITOLO_OLD)
            objHashTable.Add("CODVOCE_OLD", CODVOCE_OLD)
            objHashTable.Add("CODPROVVEDIMENTI_OLD", CODTIPOPROVVEDIMENTO_OLD)
            If txtIDTIPOVOCE.Text <> "" Then
                objHashTable.Add("INSUP", "I")
            Else
                objHashTable.Add("INSUP", "U")
            End If

            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objCOMValoriVoci.SetTipoVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDTIPOVOCE)
            txtIDTIPOVOCE.Text = IDTIPOVOCE
            'refresh 

            settavalori()

            sScript += "GestAlert('a', 'success', '', '', 'Voce inserita correttamente!');"
            sScript += "abilitaConfigura();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub settavalori()
        ddlTributo.SelectedValue = CODTRIBUTO
        ddlCapitolo.SelectedValue = CODCAPITOLO
        ddlFase.SelectedValue = CODFASE
        ddlMisura.SelectedValue = CODMISURA
        ddlProvvedimenti.SelectedValue = CODTIPOPROVVEDIMENTO
        ddlVoce.SelectedValue = CODVOCE
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCancella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Dim sScript As String = ""
        'Dim strCapitolo, strFase, strMisura, strProvvedimenti,
        'strTributo, strVoce, strVoceAttribuita As String
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objDSTipoVoci As DataSet
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String

        Try
            CODTRIBUTO = ddlTributo.SelectedItem.Value 'Request.Item("ddlTributo")
            CODCAPITOLO = ddlCapitolo.SelectedItem.Value 'Request.Item("ddlCapitolo")
            CODFASE = ddlFase.SelectedItem.Value  'Request.Item("ddlFase")
            CODMISURA = ddlMisura.SelectedItem.Value  'Request.Item("ddlMisura")
            CODTIPOPROVVEDIMENTO = ddlProvvedimenti.SelectedItem.Value  'Request.Item("ddlProvvedimenti")

            CODVOCE = ddlVoce.SelectedItem.Value  'Request.Item("ddlVoce")

            DESCVOCE = ""
            VOCEATTRIBUITA = txtVoceAttribuita.Text

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO", CODTRIBUTO_OLD)
            objHashTable.Add("CODCAPITOLO", CODCAPITOLO_OLD)
            objHashTable.Add("CODVOCE", CODVOCE_OLD)
            objHashTable.Add("CODPROVVEDIMENTI", CODTIPOPROVVEDIMENTO_OLD)
            objHashTable.Add("ANNO", "")
            objHashTable.Add("INSUP", "U")
            objHashTable.Add("IDTIPOVOCE", IDTIPOVOCE)

            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objCOMValoriVoci.DelTipoVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            'refresh 
            objCOMValoriVoci.DelValoriVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            txtIDTIPOVOCE.Text = ""

            sScript += "GestAlert('a', 'success', '', '', 'Voce eliminata correttamente!');"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.btnCancella_Click.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlCapitolo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCapitolo.SelectedIndexChanged
        Try
            If ddlCapitolo.SelectedItem.Value = "0004" Then
                ddlFase.Enabled = False
                ddlFase.SelectedIndex = 0
                ddlMisura.SelectedIndex = 1
                ddlMisura.Enabled = False
            Else
                ddlFase.Enabled = True
                'ddlMisura.SelectedIndex = 0
                ddlMisura.Enabled = True
                '**** 201809 - Cartelle Insoluti ***
                'If CODTRIBUTO = Utility.Costanti.TRIBUTO_ICI Or CODTRIBUTO = Utility.Costanti.TRIBUTO_OSAP Then
                '    ddlFase.Enabled = True
                'Else
                '    ddlFase.Enabled = False
                'End If
                '*** ***
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.NuovoInserimentoTipoVoci.ddlCapitolo_SelectedIndexChanged.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTributo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTributo.SelectedIndexChanged
        PopolaCombo()
    End Sub
End Class
