Imports System
Imports System.Data.SqlClient
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione del tipo voci.
''' Contiene i parametri di ricerca, le funzioni della comandiera e iframe per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigTipoVoci
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigTipoVoci))
    Dim COD_TRIBUTO, COD_MISURA As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    'Protected WithEvents RibesGridVoci As RIDataGrid.RibesGrid


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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Try
            If Page.IsPostBack = False Then
                PopolaComboTributo()
                PopolaCombo()
            Else
                COD_TRIBUTO = ddlTributo.SelectedItem.Value()
                PopolaCombo()
            End If
            sScript += "parent.Comandi.location.href='ComandiConfigTipoVoci.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.Page_Load.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    '''
    Private Sub PopolaCombo()
        Dim Utility As New MyUtility
        Dim fncProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
            ddlCapitolo.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlCapitolo, fncProvvedimenti.GetCapitoli(COD_TRIBUTO, "", "", cmdMyCommand), -1, "...")

            ddlProvvedimenti.Items.Clear()
            Utility.FillDropDownSQL(ddlProvvedimenti, fncProvvedimenti.GetTipoProvvedimento(COD_TRIBUTO, "", "", "VisualizzaValoriVoci", cmdMyCommand), -1, "...")

            ddlVoce.Items.Clear()
            Utility.FillDropDownSQLValueStringCodDesc(ddlVoce, fncProvvedimenti.GetTipologieSanzioni(COD_TRIBUTO, "", "", cmdMyCommand), -1, "...")

            ddlFase.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlFase, fncProvvedimenti.GetFase("", "", cmdMyCommand), -1, "...")

            ddlMisura.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlMisura, fncProvvedimenti.GetTipoMisura("", "", cmdMyCommand), -1, "...")

            'ddlCalcolata.Items.Clear()
            'Utility.FillDropDownSQL(ddlCalcolata, objGestOPENgovProvvedimenti.GetTipoBaseCalcolo("", ""), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.PopolaCombo.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.PopolaComboTributo.errore: ", ex)
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
    Private Sub btnRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Dim sScript As String = ""
        Dim objHashTable As Hashtable = New Hashtable

        Try
            'CaricaGriglia()
            objHashTable.Add("COD_TRIBUTO", ddlTributo.SelectedValue)
            objHashTable.Add("COD_CAPITOLO", ddlTributo.SelectedValue)
            objHashTable.Add("COD_PROVVEDIMENTI", ddlProvvedimenti.SelectedValue)
            objHashTable.Add("COD_VOCE", ddlVoce.SelectedValue)
            objHashTable.Add("COD_MISURA", ddlMisura.SelectedValue)
            objHashTable.Add("COD_FASE", ddlFase.SelectedValue)
            'objHashTable.Add("COD_CALCOLATA", ddlCalcolata.SelectedValue)

            Session("HashTableTipoVoci") = objHashTable
            RegisterScript("frames.item('ifrmRicerca').location.href ='RicercaConfigTipoVoci.aspx';", Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.btnRicerca_Click.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CaricaGriglia()
        Dim blnResult As Boolean = False
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable
        Try
            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If


            'Recupero la hash table
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString


            'Aggiungo gli altri campi nella hash table

            'objHashTable.Add("COD_TRIBUTO", Request.Item("COD_TRIBUTO"))
            'objHashTable.Add("COD_CAPITOLO", Request.Item("COD_CAPITOLO"))
            'objHashTable.Add("COD_PROVVEDIMENTI", Request.Item("COD_PROVVEDIMENTI"))
            'objHashTable.Add("COD_VOCE", Request.Item("COD_VOCE"))
            'objHashTable.Add("COD_MISURA", Request.Item("COD_MISURA"))
            'objHashTable.Add("COD_FASE", Request.Item("COD_FASE"))

            objHashTable.Add("COD_TRIBUTO", ddlTributo.SelectedValue)
            objHashTable.Add("COD_CAPITOLO", ddlCapitolo.SelectedValue)
            objHashTable.Add("COD_PROVVEDIMENTI", ddlProvvedimenti.SelectedValue)
            objHashTable.Add("COD_VOCE", ddlVoce.SelectedValue)
            objHashTable.Add("COD_MISURA", ddlMisura.SelectedValue)
            objHashTable.Add("COD_FASE", ddlFase.SelectedValue)

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            'Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            'objDSTipoVoci = objCOMTipoVoci.GetTipoVoci(objHashTable)
            'RibesGridVoci.DataSource = Nothing
            'If Page.IsPostBack = False Then
            '    If Not objDSTipoVoci Is Nothing Then
            '        dw = objDSTipoVoci.Tables(0).DefaultView
            '    End If
            '    'RibesGridVoci.Rows.Count = 0
            '    RibesGridVoci.start_index = 0
            '    RibesGridVoci.DataSource = dw
            '    RibesGridVoci.DataBind()
            'Else
            '    RibesGridVoci.AllowCustomPaging = False
            '    RibesGridVoci.start_index = RibesGridVoci.CurrentPageIndex
            '    dw = objDSTipoVoci.Tables(0).DefaultView
            '    'dw.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")
            '    RibesGridVoci.DataSource = dw
            'End If
            'Select Case CInt(RibesGridVoci.Rows.Count)
            '    'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
            'Case 0

            '        RibesGridVoci.Visible = False

            '        lblMessage.Text = "Nessuna Voce trovata"
            '        lblMessage.Visible = True
            '    Case Is > 0
            '        RibesGridVoci.Visible = True
            '        lblMessage.Visible = False
            'End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.CaricaGriglia.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
        End Try
    End Sub

    'Private Sub RibesGridVoci_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibesGridVoci.SelectedIndexChanged
    '    Dim strCodTributo, strCodCapitolo, _
    '    strCodTipoProvvedimento, strCodMisura, _
    '    strCodCalcolato, strCodVoce, strCodFase As String
    '    Dim strPARAMETRI, strscript As String
    '    dim sScript as string=""
    '    Try

    '        strCodTributo = RibesGridVoci.SelectedItem.Cells(7).Text()
    '        strCodCapitolo = RibesGridVoci.SelectedItem.Cells(8).Text()
    '        strCodTipoProvvedimento = RibesGridVoci.SelectedItem.Cells(9).Text()
    '        strCodMisura = RibesGridVoci.SelectedItem.Cells(10).Text()
    '        'strCodCalcolato = RibesGridVoci.SelectedItem.Cells(11).Text()
    '        strCodFase = RibesGridVoci.SelectedItem.Cells(11).Text()
    '        strCodVoce = RibesGridVoci.SelectedItem.Cells(12).Text()

    '        strPARAMETRI = "?CODTRIBUTO=" & strCodTributo
    '        strPARAMETRI = strPARAMETRI & "&CODCAPITOLO=" & strCodCapitolo
    '        strPARAMETRI = strPARAMETRI & "&CODTIPOPROVVEDIMENTO=" & strCodTipoProvvedimento
    '        strPARAMETRI = strPARAMETRI & "&CODMISURA=" & strCodMisura
    '        'strPARAMETRI = strPARAMETRI & "&CODCALCOLATO=" & strCodCalcolato
    '        strPARAMETRI = strPARAMETRI & "&CODFASE=" & strCodFase
    '        strPARAMETRI = strPARAMETRI & "&CODVOCE=" & strCodVoce
    '        strPARAMETRI = strPARAMETRI & "&Nuovo=N"

    '        
    '        sscript+="alert ('NuovoInserimentoVoci.aspx');//parent.location.href='NuovoInserimentoTipoVoci.aspx" & strPARAMETRI & "';")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '        ' CalPageaspx(strCodTributo, strCodCapitolo, strCodTipoProvvedimento, strCodMisura, strCodCalcolato)
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.RibesGridVoci_SelectedIndexChanged.errore: ", ex)
    '       Response.Redirect("../../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
    '        Response.End()
    '    End Try
    'End Sub
    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        Dim strBuilder As String
        Try
            ddlCalcolata.SelectedValue = -1
            ddlCapitolo.SelectedValue = -1
            ddlFase.SelectedValue = -1
            ddlMisura.SelectedValue = -1
            ddlProvvedimenti.SelectedValue = -1
            ddlTributo.SelectedValue = -1
            ddlVoce.SelectedValue = -1
            strBuilder = "document.getElementById ('ifrmRicerca').src='../../../aspVuota.aspx';"
            RegisterScript(strBuilder, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.btnPulisci_Click.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub Ricerca()
        Dim sScript As String = ""
        Dim objHashTable As Hashtable = New Hashtable
        Try
            objHashTable.Add("COD_TRIBUTO", ddlTributo.SelectedValue)
            objHashTable.Add("COD_CAPITOLO", ddlTributo.SelectedValue)
            objHashTable.Add("COD_PROVVEDIMENTI", ddlProvvedimenti.SelectedValue)
            objHashTable.Add("COD_VOCE", ddlVoce.SelectedValue)
            objHashTable.Add("COD_MISURA", ddlMisura.SelectedValue)
            objHashTable.Add("COD_FASE", ddlFase.SelectedValue)
            'objHashTable.Add("COD_CALCOLATA", ddlCalcolata.SelectedValue)

            Session("HashTableTipoVoci") = objHashTable
            RegisterScript("ifrmRicerca.location.href='RicercaConfigTipoVoci.aspx';", Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipoVoci.Ricerca.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
