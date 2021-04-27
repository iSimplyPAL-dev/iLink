Imports ComPlusInterface
Imports System.Data.SqlClient
Imports log4net
''' <summary>
''' Pagina per la gestione delle voci.
''' Contiene i parametri di ricerca, le funzioni della comandiera e iframe per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class configuraVoci
    Inherits BasePage
	Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(configuraVoci))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents anno As System.Web.UI.WebControls.TextBox
    Protected WithEvents valore As System.Web.UI.WebControls.TextBox
    Protected WithEvents Minimo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCODTRIBUTO As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCODCAPITOLO As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCODVOCE As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim CODTRIBUTO, CODCAPITOLO, CODTIPOPROVVEDIMENTO, CODMISURA, CODCALCOLATO, CODVOCE, CODFASE, VOCEATTRIBUITA, ViewAlert, IDTIPOVOCE As String
    Dim TrovataVoce As Boolean
    Dim parametri As String
    Dim AnnoOLD As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            CODTRIBUTO = Request.Item("CODTRIBUTO")
            CODCAPITOLO = Request.Item("CODCAPITOLO")
            CODVOCE = Request.Item("CODVOCE")
            CODMISURA = Request.Item("CODMISURA")
            IDTIPOVOCE = Request.Item("IDTIPOVOCE")
            CODTIPOPROVVEDIMENTO = Request.Item("CODTIPOPROVVEDIMENTO")
            CODFASE = Request.Item("CODFASE")
            VOCEATTRIBUITA = Request.Item("VOCEATTRIBUITA")
            PopolaCombo()
            PopolaLabel()

            parametri = "CODTRIBUTO=" & CODTRIBUTO & "&CODCAPITOLO=" & CODCAPITOLO & "&CODTIPOPROVVEDIMENTO=" & CODTIPOPROVVEDIMENTO
            parametri = parametri & "&CODMISURA=" & CODMISURA & "&CODVOCE=" & CODVOCE & "&IDTIPOVOCE=" & IDTIPOVOCE
            parametri = parametri & "&CODFASE=" & CODFASE & "&VOCEATTRIBUITA=" & VOCEATTRIBUITA & "&Nuovo=U"
            If Page.IsPostBack Then
                ViewAlert = "false"
            End If

            'sscript+= "ifrmVoci.location.href='VisualizzaValoriVoci.aspx?" & parametri & "'" & vbCrLf
            'RegisterScript(sScript , Me.GetType())
            CaricaGriglia(False)
            If CODMISURA.CompareTo("I") = 0 Then
                txtvalore.Enabled = False
                txtMinimo.Enabled = False
                chkCumulabile.Enabled = False
                chkRiducibile.Enabled = False
                ddlCalcolata.Enabled = False
                ddlParametro.Enabled = False
                ddlBaseRaffronto.Enabled = False
                txtCondizione.Enabled = False
                ddlTipoInteresse.Enabled = True

                ddlParametro_Intr.Enabled = False
                ddlBaseRaffronto_Intr.Enabled = False
                txtCondizione_Intr.Enabled = False

                lblvalore.CssClass = "Input_Label_disabled"
                lblMinimo.CssClass = "Input_Label_disabled"
                lblcalcSu.CssClass = "Input_Label_disabled"
                lblParametro.CssClass = "Input_Label_disabled"
                lblCondizione.CssClass = "Input_Label_disabled"
                lblBaseRaffronto.CssClass = "Input_Label_disabled"
                lblTipoInteresse.CssClass = "Input_Label"

                lblParametro_Intr.CssClass = "Input_Label_disabled"
                lblCondizione_intr.CssClass = "Input_Label_disabled"
                lblBaseRaffronto_Intr.CssClass = "Input_Label_disabled"

            ElseIf CODCAPITOLO.CompareTo("0004") = 0 Then

                txtvalore.Enabled = True
                lblvalore.CssClass = "Input_Label"

                txtMinimo.Enabled = False
                chkCumulabile.Enabled = False
                chkRiducibile.Enabled = False
                ddlCalcolata.Enabled = False
                ddlParametro.Enabled = False
                ddlBaseRaffronto.Enabled = False
                txtCondizione.Enabled = False
                ddlTipoInteresse.Enabled = False

                ddlParametro_Intr.Enabled = False
                ddlBaseRaffronto_Intr.Enabled = False
                txtCondizione_Intr.Enabled = False

                lblMinimo.CssClass = "Input_Label_disabled"
                lblcalcSu.CssClass = "Input_Label_disabled"
                lblParametro.CssClass = "Input_Label_disabled"
                lblCondizione.CssClass = "Input_Label_disabled"
                lblBaseRaffronto.CssClass = "Input_Label_disabled"
                lblTipoInteresse.CssClass = "Input_Label_disabled"

                lblParametro_Intr.CssClass = "Input_Label_disabled"
                lblCondizione_intr.CssClass = "Input_Label_disabled"
                lblBaseRaffronto_Intr.CssClass = "Input_Label_disabled"

            Else
                txtvalore.Enabled = True
                txtMinimo.Enabled = True
                chkCumulabile.Enabled = True
                chkRiducibile.Enabled = True
                ddlCalcolata.Enabled = True
                ddlParametro.Enabled = True
                ddlBaseRaffronto.Enabled = True
                txtCondizione.Enabled = True
                ddlTipoInteresse.Enabled = False

                ddlParametro_Intr.Enabled = True
                ddlBaseRaffronto_Intr.Enabled = True
                txtCondizione_Intr.Enabled = True

                lblvalore.CssClass = "Input_Label"
                lblMinimo.CssClass = "Input_Label"
                lblcalcSu.CssClass = "Input_Label"
                lblParametro.CssClass = "Input_Label"
                lblCondizione.CssClass = "Input_Label"
                lblBaseRaffronto.CssClass = "Input_Label"
                lblTipoInteresse.CssClass = "Input_Label_disabled"

                lblParametro_Intr.CssClass = "Input_Label"
                lblCondizione_intr.CssClass = "Input_Label"
                lblBaseRaffronto_Intr.CssClass = "Input_Label"
            End If

            'sscript+= "parent.Comandi.location.href='ComandiInserimentoVoci.aspx'" & vbCrLf
            'RegisterScript(sScript , Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.Page_Load.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="valore"></param>
    ''' <returns></returns>
    Protected Function SetValoreSINO(ByVal valore As Object) As String
        Try
            If IsDBNull(valore) Then
                Return "NO"
            Else
                If valore = 1 Then
                    Return "SI"
                Else
                    Return "NO"
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.SetValoreSINO.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    Sub PopolaLabel()
        Dim Utility As New MyUtility
        Dim fncProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim oDR As SqlDataReader
        Dim DESCTIPOPROVVEDIMENTO, DESCTRIBUTO, DESCCAPITOLO, DESCVOCE, DESCMISURA As String
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            Log.Debug("configuraVoci::PopolaLabel::devo prelevare tipo tributo")
            oDR = fncProvvedimenti.GetTributi(CODTRIBUTO, "", cmdMyCommand)
            While oDR.Read()
                DESCTRIBUTO = oDR.GetString(1)
            End While
            oDR.Close()
            'Il datareader viene chiuso all'interno della funzione 
            Log.Debug("configuraVoci::PopolaLabel::devo prelevare tipo provvedimento")
            oDR = fncProvvedimenti.GetTipoProvvedimento(CODTRIBUTO, CODTIPOPROVVEDIMENTO, "", "", cmdMyCommand)
            While oDR.Read()
                DESCTIPOPROVVEDIMENTO = oDR.GetString(1)
            End While
            oDR.Close()

            lblTipoProvvedimento.Text = DESCTIPOPROVVEDIMENTO & " - " & DESCTRIBUTO
            Log.Debug("configuraVoci::PopolaLabel::devo prelevare tipo capitolo")
            oDR = fncProvvedimenti.GetCapitoli(CODTRIBUTO, CODCAPITOLO, "", cmdMyCommand)
            While oDR.Read()
                DESCCAPITOLO = oDR.GetString(1)
            End While
            oDR.Close()
            Log.Debug("configuraVoci::PopolaLabel::devo prelevare tipo sanzioni")
            oDR = fncProvvedimenti.GetTipologieSanzioni(CODTRIBUTO, CODVOCE, "", cmdMyCommand)
            'oDR = objGestOPENgovProvvedimenti.GetTipoVoci(constsession.idente, CODTRIBUTO, CODVOCE, "")
            While oDR.Read()
                DESCVOCE = oDR.GetString(1)
            End While
            oDR.Close()
            lblVoce.Text = DESCCAPITOLO & " - " & DESCVOCE
            Log.Debug("configuraVoci::PopolaLabel::devo prelevare tipo misura")
            oDR = fncProvvedimenti.GetTipoMisura(CODMISURA, "", cmdMyCommand)
            While oDR.Read()
                DESCMISURA = oDR.GetString(1)
            End While
            oDR.Close()
            lblMisura.Text = DESCMISURA

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.PopolaLabe.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim strAnno, strValore,
        strMinimo, strRiducibile,
        strCumulabile, strCalcolataSu, strCondizione,
        strParametro, strBaseRaffronto, strCOD_TIPO_INTERESSE As String
            Dim strCondizione_intr, strParametro_intr, strBaseRaffront_intr As String
            Dim intID_VALORE_VOCE As Integer
            Dim sScript As String = ""
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdVoci.Rows
                    If IDRow = CType(myRow.FindControl("hfID_VALORE_VOCE"), HiddenField).Value Then
                        strAnno = myRow.Cells(0).Text()
                        strValore = Trim(myRow.Cells(1).Text())
                        strMinimo = Trim(myRow.Cells(2).Text())
                        strRiducibile = Trim(myRow.Cells(3).Text())
                        strCumulabile = Trim(myRow.Cells(4).Text())
                        strCondizione = Trim(myRow.Cells(8).Text())

                        strCalcolataSu = CType(myRow.FindControl("hfCALCOLATA_SU"), HiddenField).Value
                        strParametro = CType(myRow.FindControl("hfParametro"), HiddenField).Value
                        strBaseRaffronto = CType(myRow.FindControl("hfBase_raffronto"), HiddenField).Value

                        strCOD_TIPO_INTERESSE = CType(myRow.FindControl("hfCOD_TIPO_INTERESSE"), HiddenField).Value
                        intID_VALORE_VOCE = IDRow

                        strParametro_intr = CType(myRow.FindControl("hfParametro_intr"), HiddenField).Value
                        strBaseRaffront_intr = CType(myRow.FindControl("hfBase_raffronto_intr"), HiddenField).Value
                        strCondizione_intr = CType(myRow.FindControl("hfCondizione_intr"), HiddenField).Value

                        strAnno = replaceSpace(strAnno)

                        strValore = replaceSpace(strValore)
                        strMinimo = replaceSpace(strMinimo)

                        strCondizione = replaceSpace(strCondizione)
                        strCalcolataSu = replaceSpace(strCalcolataSu)
                        strParametro = replaceSpace(strParametro)
                        strBaseRaffronto = replaceSpace(strBaseRaffronto)

                        strParametro_intr = replaceSpace(strParametro_intr)
                        strBaseRaffront_intr = replaceSpace(strBaseRaffront_intr)
                        strCondizione_intr = replaceSpace(strCondizione_intr)

                        txtanno.Text = strAnno
                        AnnoOLD = strAnno
                        txtHiddenIDVALOREVOCE.Text = intID_VALORE_VOCE

                        If strCOD_TIPO_INTERESSE <> "&nbsp;" And strCOD_TIPO_INTERESSE <> "" Then
                            ddlTipoInteresse.SelectedValue = strCOD_TIPO_INTERESSE
                        Else
                            txtvalore.Text = strValore
                            txtMinimo.Text = strMinimo
                            txtCondizione.Text = strCondizione
                            If strCalcolataSu = "0" Or strCalcolataSu = "" Then strCalcolataSu = -1
                            ddlCalcolata.SelectedValue = strCalcolataSu
                            If strParametro = "0" Or strParametro = "" Then strParametro = -1
                            ddlParametro.SelectedValue = strParametro
                            If strBaseRaffronto = "0" Or strBaseRaffronto = "" Then strBaseRaffronto = -1
                            ddlBaseRaffronto.SelectedValue = strBaseRaffronto

                            If strParametro_intr = "0" Or strParametro_intr = "" Then strParametro_intr = -1
                            ddlParametro_Intr.SelectedValue = strParametro_intr
                            If strBaseRaffront_intr = "0" Or strBaseRaffront_intr = "" Then strBaseRaffront_intr = -1
                            ddlBaseRaffronto_Intr.SelectedValue = strBaseRaffront_intr
                            txtCondizione_Intr.Text = strCondizione_intr

                            If strRiducibile = "1" Then
                                chkRiducibile.Checked = True
                            Else
                                chkRiducibile.Checked = False
                            End If
                            If strCumulabile = "1" Then
                                chkCumulabile.Checked = True
                            Else
                                chkCumulabile.Checked = False
                            End If
                        End If
                        txtInsUp.Text = "U"

                        RegisterScript("AbilitaPulsanti();", Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdVoci_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdVoci.SelectedIndexChanged
    '    Dim strAnno, strValore,
    '    strMinimo, strRiducibile,
    '    strCumulabile, strCalcolataSu, strCondizione,
    '    strParametro, strBaseRaffronto, strCOD_TIPO_INTERESSE As String
    '    Dim strCondizione_intr, strParametro_intr, strBaseRaffront_intr As String
    '    Dim intID_VALORE_VOCE As Integer
    '    dim sScript as string=""
    '    Try


    '        strAnno = GrdVoci.SelectedItem.Cells(0).Text()
    '        strValore = Trim(GrdVoci.SelectedItem.Cells(1).Text())
    '        strMinimo = Trim(GrdVoci.SelectedItem.Cells(2).Text())
    '        strRiducibile = Trim(GrdVoci.SelectedItem.Cells(3).Text())
    '        strCumulabile = Trim(GrdVoci.SelectedItem.Cells(4).Text())

    '        strCondizione = Trim(GrdVoci.SelectedItem.Cells(10).Text())
    '        strCalcolataSu = Trim(GrdVoci.SelectedItem.Cells(11).Text())
    '        strParametro = Trim(GrdVoci.SelectedItem.Cells(12).Text())
    '        strBaseRaffronto = Trim(GrdVoci.SelectedItem.Cells(13).Text())

    '        strCOD_TIPO_INTERESSE = Trim(GrdVoci.SelectedItem.Cells(14).Text())
    '        intID_VALORE_VOCE = Trim(GrdVoci.SelectedItem.Cells(15).Text())


    '        strParametro_intr = Trim(GrdVoci.SelectedItem.Cells(17).Text())
    '        strBaseRaffront_intr = Trim(GrdVoci.SelectedItem.Cells(18).Text())
    '        strCondizione_intr = Trim(GrdVoci.SelectedItem.Cells(19).Text())


    '        strAnno = replaceSpace(strAnno)

    '        strValore = replaceSpace(strValore)
    '        strMinimo = replaceSpace(strMinimo)

    '        strCondizione = replaceSpace(strCondizione)
    '        strCalcolataSu = replaceSpace(strCalcolataSu)
    '        strParametro = replaceSpace(strParametro)
    '        strBaseRaffronto = replaceSpace(strBaseRaffronto)

    '        strParametro_intr = replaceSpace(strParametro_intr)
    '        strBaseRaffront_intr = replaceSpace(strBaseRaffront_intr)
    '        strCondizione_intr = replaceSpace(strCondizione_intr)

    '        txtanno.Text = strAnno
    '        txtHiddenIDVALOREVOCE.Text = intID_VALORE_VOCE

    '        If strCOD_TIPO_INTERESSE <> "&nbsp;" And strCOD_TIPO_INTERESSE <> "" Then
    '            ddlTipoInteresse.SelectedValue = strCOD_TIPO_INTERESSE
    '        Else

    '            txtvalore.Text = strValore
    '            txtMinimo.Text = strMinimo
    '            txtCondizione.Text = strCondizione
    '            If strCalcolataSu = "0" Or strCalcolataSu = "" Then strCalcolataSu = -1
    '            ddlCalcolata.SelectedValue = strCalcolataSu
    '            If strParametro = "0" Or strParametro = "" Then strParametro = -1
    '            ddlParametro.SelectedValue = strParametro
    '            If strBaseRaffronto = "0" Or strBaseRaffronto = "" Then strBaseRaffronto = -1
    '            ddlBaseRaffronto.SelectedValue = strBaseRaffronto

    '            If strParametro_intr = "0" Or strParametro_intr = "" Then strParametro_intr = -1
    '            ddlParametro_Intr.SelectedValue = strParametro_intr
    '            If strBaseRaffront_intr = "0" Or strBaseRaffront_intr = "" Then strBaseRaffront_intr = -1
    '            ddlBaseRaffronto_Intr.SelectedValue = strBaseRaffront_intr
    '            txtCondizione_Intr.Text = strCondizione_intr


    '            If strRiducibile = "1" Then
    '                chkRiducibile.Checked = True
    '            Else
    '                chkRiducibile.Checked = False
    '            End If
    '            If strCumulabile = "1" Then
    '                chkCumulabile.Checked = True
    '            Else
    '                chkCumulabile.Checked = False
    '            End If
    '        End If
    '        txtInsUp.Text = "U"



    '        sscript+="<script language='javascript'>" & vbCrLf)
    '        sscript+="AbilitaPulsanti();" & vbCrLf)
    '        
    '        RegisterScript(sScript , Me.GetType())

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.GrdVoci_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
    '        Response.End()
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="valore"></param>
    ''' <returns></returns>
    Function replaceSpace(ByVal valore As String) As String
        replaceSpace = valore.Replace("&nbsp;", "")
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Pulisci"></param>
    Private Sub CaricaGriglia(ByVal Pulisci As Boolean)
        Dim dw As DataView
        Dim objDSTipoVoci As DataSet
        Dim blnResult As Boolean = False
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable

        Dim sScript As String = ""
        Try

            'Recupero la hash table
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

            'CODTRIBUTO = Request.Item("CODTRIBUTO")
            'CODCAPITOLO = Request.Item("CODCAPITOLO")
            'CODVOCE = Request.Item("CODVOCE")
            'ViewAlert = Request.Item("ViewAlert")

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
            objHashTable.Add("CODCAPITOLO", CODCAPITOLO)
            objHashTable.Add("IDTIPOVOCE", IDTIPOVOCE)
            objHashTable.Add("CODVOCE", CODVOCE)
            objHashTable.Add("CODTIPOPROVVEDIMENTO", CODTIPOPROVVEDIMENTO)
            objHashTable.Add("CODFASE", CODFASE)


            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipoVoci = objCOMTipoVoci.GetValoriVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            If Page.IsPostBack = False Or Pulisci = True Then

                If Not objDSTipoVoci Is Nothing Then
                    dw = objDSTipoVoci.Tables(0).DefaultView
                End If
                GrdVoci.DataSource = dw
                GrdVoci.DataBind()

                'If objDSTipoVoci.Tables(0).Rows.Count <> 0 And ViewAlert = "" Then
                '    
                '    sscript+="alert('Voce già presente a sistema');")
                '    

                '    RegisterScript(sScript , Me.GetType())
                'End If
            End If
            Select Case CInt(GrdVoci.Rows.Count)
                'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
                Case 0
                    TrovataVoce = False
                    GrdVoci.Visible = False

                    lblMessage.Text = "Nessuna Voce trovata"
                    lblMessage.Visible = True
                Case Is > 0
                    TrovataVoce = True
                    GrdVoci.Visible = True
                    lblMessage.Visible = False
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.CaricaGriglia.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per il popolamento degli elenchi a discesa.
    ''' Come interessi si gestiscono solo più gli interessi legali quindi la relativa tendina è prepopolata.
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub PopolaCombo()
        Dim Utility As New MyUtility
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            'Il datareader viene chiuso all'interno della funzione 

            ddlCalcolata.Items.Clear()
            Utility.FillDropDownSQL(ddlCalcolata, objGestOPENgovProvvedimenti.GetTipoBaseCalcolo("", "", cmdMyCommand), -1, "...")

            ddlBaseRaffronto.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlBaseRaffronto, objGestOPENgovProvvedimenti.GetBaseRaffronto("", "", cmdMyCommand), -1, "...")

            ddlParametro.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlParametro, objGestOPENgovProvvedimenti.GetParametro("", "", cmdMyCommand), -1, "...")
            ddlTipoInteresse.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlTipoInteresse, objGestOPENgovProvvedimenti.GetTipoInteresse(CODTRIBUTO, "", cmdMyCommand), -1, "...")
            ddlTipoInteresse.SelectedIndex = ddlTipoInteresse.Items.Count - 1

            'Instrasmissibilità
            ddlParametro_Intr.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlParametro_Intr, objGestOPENgovProvvedimenti.GetParametro("", "", cmdMyCommand), -1, "...")

            ddlBaseRaffronto_Intr.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlBaseRaffronto_Intr, objGestOPENgovProvvedimenti.GetBaseRaffronto_Instrasmissibilita("", "", cmdMyCommand), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.PopolaCombo.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    'Private Sub PopolaCombo()
    '    Dim Utility As New MyUtility
    '    Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0

    '        'Il datareader viene chiuso all'interno della funzione 

    '        ddlCalcolata.Items.Clear()
    '        Utility.FillDropDownSQL(ddlCalcolata, objGestOPENgovProvvedimenti.GetTipoBaseCalcolo("", "", cmdMyCommand), -1, "...")

    '        ddlBaseRaffronto.Items.Clear()
    '        Utility.FillDropDownSQLValueString(ddlBaseRaffronto, objGestOPENgovProvvedimenti.GetBaseRaffronto("", "", cmdMyCommand), -1, "...")

    '        ddlParametro.Items.Clear()
    '        Utility.FillDropDownSQLValueString(ddlParametro, objGestOPENgovProvvedimenti.GetParametro("", "", cmdMyCommand), -1, "...")
    '        ddlTipoInteresse.Items.Clear()
    '        Utility.FillDropDownSQLValueString(ddlTipoInteresse, objGestOPENgovProvvedimenti.GetTipoInteresse(CODTRIBUTO, "", cmdMyCommand), -1, "...")

    '        'Instrasmissibilità
    '        ddlParametro_Intr.Items.Clear()
    '        Utility.FillDropDownSQLValueString(ddlParametro_Intr, objGestOPENgovProvvedimenti.GetParametro("", "", cmdMyCommand), -1, "...")

    '        ddlBaseRaffronto_Intr.Items.Clear()
    '        Utility.FillDropDownSQLValueString(ddlBaseRaffronto_Intr, objGestOPENgovProvvedimenti.GetBaseRaffronto_Instrasmissibilita("", "", cmdMyCommand), -1, "...")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.PopolaCombo.errore: ", ex)
    '        Response.Redirect("../../../../PaginaErrore.aspx")
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim sScript As String = ""
        Dim stranno, strvalore, strminimo, strInsUp As String
        Dim strCondizione, strParametro, strBaseRaffronto, strCalcolata, strTipoInteresse As String
        Dim strCondizione_intr, strParametro_intr, strBaseRaffronto_intr As String
        Dim strID_VALORE_VOCI As String
        Dim bchkCum, bchkRid As Boolean
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        'Dim strWFErrore As String
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Try
            stranno = txtanno.Text
            strvalore = txtvalore.Text
            strminimo = txtMinimo.Text
            strID_VALORE_VOCI = txtHiddenIDVALOREVOCE.Text
            If strID_VALORE_VOCI.CompareTo("") = 0 Then
                strID_VALORE_VOCI = "-1"
            End If

            bchkCum = chkCumulabile.Checked
            bchkRid = chkRiducibile.Checked

            strCondizione = txtCondizione.Text
            strParametro = Request.Item("ddlParametro")
            strBaseRaffronto = Request.Item("ddlBaseRaffronto")
            strCalcolata = Request.Item("ddlCalcolata")
            strTipoInteresse = Request.Item("ddlTipoInteresse")

            strCondizione_intr = txtCondizione_Intr.Text
            strParametro_intr = Request.Item("ddlParametro_Intr")
            strBaseRaffronto_intr = Request.Item("ddlBaseRaffronto_Intr")

            'Serve per determinare se sto facendo un'insert di una nuova voce oppure un update
            'strInsUp=I -> insert
            'strInsUp=U -> Update
            strInsUp = txtInsUp.Text
            If strInsUp.CompareTo("") = 0 Then
                strInsUp = "I"
            End If
            If stranno.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'L'anno è obbligatorio');"
                sScript += "document.getElementById (""txtanno"").focus();"
                RegisterScript(sScript, Me.GetType())
            Else
                'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("USER", ConstSession.UserName)

                objHashTable.Add("CODENTE", ConstSession.IdEnte)
                objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
                objHashTable.Add("CODCAPITOLO", CODCAPITOLO)
                objHashTable.Add("CODVOCE", CODVOCE)
                objHashTable.Add("CODTIPOPROVVEDIMENTO", CODTIPOPROVVEDIMENTO)
                objHashTable.Add("ID_VALORE_VOCI", strID_VALORE_VOCI)
                objHashTable.Add("IDTIPOVOCE", IDTIPOVOCE)

                'objHashTable.Add("CODTIPOPROVVEDIMENTO",)

                If strInsUp = "U" Then
                    objHashTable.Add("ANNO_OLD", AnnoOLD)
                End If

                objHashTable.Add("ANNO", stranno)
                objHashTable.Add("VALORE", strvalore)
                objHashTable.Add("MINIMO", strminimo)
                objHashTable.Add("CUMULABILE", bchkCum)
                objHashTable.Add("RIDUCIBILE", bchkRid)

                objHashTable.Add("CONDIZIONE", strCondizione)
                objHashTable.Add("PARAMETRO", strParametro)
                objHashTable.Add("BASERAFFRONTO", strBaseRaffronto)
                objHashTable.Add("CALCOLATASU", strCalcolata)
                objHashTable.Add("TIPOINTERESSE", strTipoInteresse)

                objHashTable.Add("CONDIZIONE_INTR", strCondizione_intr)
                objHashTable.Add("PARAMETRO_INTR", strParametro_intr)
                objHashTable.Add("BASERAFFRONTO_INTR", strBaseRaffronto_intr)


                objHashTable.Add("INSUP", strInsUp)

                If Not TrovataVoce Then
                    'non ho trovato la voce devo salvarla nella tabella Tipo_Voci
                End If
                Dim intRetVal As Integer
                Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                If Not objCOMValoriVoci.SetValoriVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, intRetVal) Then
                    If intRetVal <> 1 Then
                        Throw New Exception(intRetVal)
                    End If
                End If

                txtanno.Text = ""
                txtvalore.Text = ""
                txtMinimo.Text = ""
                txtInsUp.Text = ""
                chkCumulabile.Checked = False
                chkRiducibile.Checked = False
                txtCondizione.Text = ""
                ddlBaseRaffronto.SelectedValue = -1
                ddlCalcolata.SelectedValue = -1
                ddlParametro.SelectedValue = -1
                txtHiddenIDVALOREVOCE.Text = ""


                txtCondizione_Intr.Text = ""
                ddlBaseRaffronto_Intr.SelectedValue = -1
                ddlParametro_Intr.SelectedValue = -1

                CaricaGriglia(True)

                'refresh 

            End If
        Catch ex As Exception
            If InStr(ex.Message, "2627") > 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Il valore voce inserito è già presente');"
                RegisterScript(sScript, Me.GetType())
            Else
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.btnSalva_Click.errore: ", ex)
                Response.Redirect("../../../../PaginaErrore.aspx")
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCancella_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Dim sScript As String = ""
        Dim stranno, strvalore, strminimo As String
        Dim strID_VALORE_VOCI As String
        Dim bchkCum, bchkRid As Boolean
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Try
            stranno = txtanno.Text
            strvalore = txtvalore.Text
            strminimo = txtMinimo.Text
            strID_VALORE_VOCI = txtHiddenIDVALOREVOCE.Text

            bchkCum = chkCumulabile.Checked
            bchkRid = chkRiducibile.Checked

            If stranno.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Mancano i parametri che identificano la voce');"
                sScript += "document.getElementById (""txtanno"").focus();"
                RegisterScript(sScript, Me.GetType())
            Else
                'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("USER", ConstSession.UserName)

                objHashTable.Add("CODENTE", ConstSession.IdEnte)
                objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
                objHashTable.Add("CODCAPITOLO", CODCAPITOLO)
                objHashTable.Add("CODVOCE", CODVOCE)

                objHashTable.Add("ANNO", stranno)
                objHashTable.Add("VALORE", strvalore)
                objHashTable.Add("MINIMO", strminimo)
                objHashTable.Add("ID_VALORE_VOCI", strID_VALORE_VOCI)

                Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                objCOMValoriVoci.DelValoriVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)


                Pulisci()

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.configuraVoci.btnCancella_Click.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim sScript As String = ""
        sScript += "parent.Comandi.location.href='ComandiInserimentoTipoVoci.aspx';"
        sScript += "parent.Visualizza.location.href='NuovoInserimentoTipoVoci.aspx?" & parametri & "';"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Pulisci()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Sub Pulisci()
        txtanno.Text = ""
        txtvalore.Text = ""
        txtMinimo.Text = ""
        txtInsUp.Text = ""
        txtHiddenIDVALOREVOCE.Text = ""
        chkCumulabile.Checked = False
        chkRiducibile.Checked = False
        ddlCalcolata.SelectedValue = -1
        ddlParametro.SelectedValue = -1
        ddlBaseRaffronto.SelectedValue = -1
        txtCondizione.Text = ""
        ddlTipoInteresse.SelectedValue = -1
        GrdVoci.SelectedIndex = -1
        CaricaGriglia(True)
    End Sub
End Class
