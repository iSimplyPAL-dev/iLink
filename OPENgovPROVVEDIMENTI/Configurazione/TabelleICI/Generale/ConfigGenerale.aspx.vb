Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione confronto dichiarato/versato IMU.
''' Contiene i parametri di definizione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigGenerale
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigGenerale))
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ddlTributo As System.Web.UI.WebControls.DropDownList

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
            If Not Page.IsPostBack Then
                PopolaComboAnno()
            End If
            sScript += "parent.Comandi.location.href='ComandiGenerale.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigGenerale.Page_Load.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboAnno()
        Dim FncMyUtility As New MyUtility
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
        Try
            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

            'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
            ddlAnno.Items.Clear()
            FncMyUtility.FillDropDownSQLSingleString(ddlAnno, objGestOPENgovProvvedimenti.GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, Utility.Costanti.TRIBUTO_ICI), -1, "...")


            'ddlCalcolata.Items.Clear()
            'Utility.FillDropDownSQL(ddlCalcolata, objGestOPENgovProvvedimenti.GetTipoBaseCalcolo("", ""), -1, "...")


        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigGenerale.PopolaComboAnno.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlAnno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlAnno.SelectedIndexChanged
        Dim strAnno As String
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim dw As DataView
        Dim objDSGenerale As DataSet
        Dim RIENTRO_LIQ_CONF_AVVISO, RIENTRO_LIQ_ATTO_DEF, INT_ACCONTO_SALDO, INT_SALDO As String
        strAnno = ddlAnno.SelectedValue

        If strAnno.CompareTo("-1") <> 0 Then


            Try
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("USER", ConstSession.UserName)
                objHashTable.Add("CodENTE", ConstSession.IdEnte)
                objHashTable.Add("ANNODA", strAnno)

                Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                objDSGenerale = objCOMTipoVoci.GetGeneraleICI(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
                If Not objDSGenerale Is Nothing Then
                    dw = objDSGenerale.Tables(0).DefaultView
                End If


                If dw.Table.Rows.Count > 0 Then


                    RIENTRO_LIQ_CONF_AVVISO = dw.Item(0).Row("RIENTRO_LIQ_CONF_AVVISO")
                    RIENTRO_LIQ_ATTO_DEF = dw.Item(0).Row("RIENTRO_LIQ_ATTO_DEF")
                    INT_ACCONTO_SALDO = dw.Item(0).Row("INT_ACCONTO_SALDO")
                    INT_SALDO = dw.Item(0).Row("INT_SALDO")

                    If RIENTRO_LIQ_CONF_AVVISO = "True" Then
                        radioAtto0.Checked = True
                        radioAtto1.Checked = False
                    End If
                    If RIENTRO_LIQ_ATTO_DEF = "True" Then
                        radioAtto0.Checked = False
                        radioAtto1.Checked = True
                    End If

                    If INT_ACCONTO_SALDO = "True" Then
                        RadioSaldo0.Checked = True
                        RadioSaldo1.Checked = False
                    End If
                    If INT_SALDO = "True" Then
                        RadioSaldo0.Checked = False
                        RadioSaldo1.Checked = True
                    End If

                    lblMessage.Text = ""
                    lblMessage.Visible = False

                Else
                    radioAtto0.Checked = False
                    radioAtto1.Checked = False
                    RadioSaldo0.Checked = False
                    RadioSaldo1.Checked = False

                    lblMessage.Text = "Parametri Generali non ancora configurati per l'anno " + ddlAnno.SelectedItem.Text
                    lblMessage.Visible = True

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigGenerale.SelectedIndexChanged.errore: ", ex)
                Response.Redirect("../../../../PaginaErrore.aspx")

            Finally
                dw.Dispose()
                'If Not IsNothing(objSessione) Then
                '    objSessione.Kill()
                '    objSessione = Nothing
                'End If
            End Try
        Else

            radioAtto0.Checked = False
            radioAtto1.Checked = False
            RadioSaldo0.Checked = False
            RadioSaldo1.Checked = False

            lblMessage.Text = ""
            lblMessage.Visible = False

        End If

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim strAnno As String
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim strRIENTRO_LIQ_CONF_AVVISO, strRIENTRO_LIQ_ATTO_DEF, strINT_ACCONTO_SALDO, strINT_SALDO, strInsUp As String

        Try
            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            strAnno = ddlAnno.SelectedValue

            If radioAtto0.Checked Then
                strRIENTRO_LIQ_CONF_AVVISO = 1
            Else
                strRIENTRO_LIQ_CONF_AVVISO = 0
            End If

            If radioAtto1.Checked Then
                strRIENTRO_LIQ_ATTO_DEF = 1
            Else
                strRIENTRO_LIQ_ATTO_DEF = 0
            End If

            If RadioSaldo0.Checked Then
                strINT_ACCONTO_SALDO = 1
            Else
                strINT_ACCONTO_SALDO = 0
            End If

            If RadioSaldo1.Checked Then
                strINT_SALDO = 1
            Else
                strINT_SALDO = 0
            End If
            'solo update
            strInsUp = "U"

            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("ANNO", strAnno)
            objHashTable.Add("RIENTRO_LIQ_CONF_AVVISO", strRIENTRO_LIQ_CONF_AVVISO)
            objHashTable.Add("RIENTRO_LIQ_ATTO_DEF", strRIENTRO_LIQ_ATTO_DEF)
            objHashTable.Add("INT_ACCONTO_SALDO", strINT_ACCONTO_SALDO)
            objHashTable.Add("INT_SALDO", strINT_SALDO)
            objHashTable.Add("INSUP", strInsUp)

            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            Dim bRetVal As Boolean
            bRetVal = objCOMValoriVoci.SetGeneraleICI(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            If bRetVal = True Then
                lblMessage.Text = ""
                lblMessage.Visible = False
            End If
            'Pulisci()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigGenerale.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        Finally
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
        End Try


    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        Pulisci()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub Pulisci()
        ddlAnno.SelectedValue = -1
        radioAtto0.Checked = False
        radioAtto1.Checked = False
        RadioSaldo0.Checked = False
        RadioSaldo1.Checked = False
    End Sub
End Class
