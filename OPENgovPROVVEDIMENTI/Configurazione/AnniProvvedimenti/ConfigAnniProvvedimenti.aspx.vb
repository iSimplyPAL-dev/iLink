Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione degli anni abilitati per i provvedimenti.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigAnniProvvedimenti
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigAnniProvvedimenti))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnElimina As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim COD_TRIBUTO, COD_TIPO_PROVVEDIENTO, COD_MISURA As String
    Dim strCodTributo, strCodTipoProvvedimento, strAnno, strSogliaMinima, strInsUp As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Try
            COD_TIPO_PROVVEDIENTO = ""
            strCodTributo = ddlTributo.SelectedValue
            strCodTipoProvvedimento = ddlProvvedimenti.SelectedValue
            strAnno = txtAnno.Text
            strSogliaMinima = txtSoglia.Text
            strInsUp = txtOperazione.Text
            If Page.IsPostBack = False Then
                COD_TRIBUTO = ""
                PopolaComboTributo()
                CaricaGriglia(True, False)
            Else
                COD_TRIBUTO = ddlTributo.SelectedItem.Value()
                PopolaCombo()
                CaricaGriglia(False, False)
            End If
            sScript += "parent.Comandi.location.href='ComandiConfigAnniProvvedimenti.aspx'"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaCombo()
        Dim Utility As New MyUtility
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            ddlProvvedimenti.Items.Clear()
            Utility.FillDropDownSQL(ddlProvvedimenti, objGestOPENgovProvvedimenti.GetTipoProvvedimento(COD_TRIBUTO, COD_TIPO_PROVVEDIENTO, "", "AnniProvvedimento", cmdMyCommand), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.PopolaCombo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
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
            Utility.FillDropDownSQLValueString(ddlTributo, objGestOPENgovProvvedimenti.GetTributi(COD_TRIBUTO, "", cmdMyCommand), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.PopolaComboTributo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Pulisci"></param>
    ''' <param name="RiCarica"></param>
    Sub CaricaGriglia(ByVal Pulisci As Boolean, ByVal RiCarica As Boolean)
        Dim dw As DataView
        Dim objDSTipiInteressi As DataSet
        Dim objHashTable As Hashtable = New Hashtable

        Try
            'carico la hash table
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CODTRIBUTO", "-1")
            objHashTable.Add("CODTIPOPROVVEDIMENTO", "-1")
            objHashTable.Add("ANNO", "")
            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipiInteressi = objCOMTipoVoci.GetAnniProvvedimenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            ' GrdAnniProvvedimenti.DataSource = Nothing
            If Page.IsPostBack = False Or RiCarica = True Then
                If Not objDSTipiInteressi Is Nothing Then
                    dw = objDSTipiInteressi.Tables(0).DefaultView
                End If
                Session("dwConfAnniProv") = dw
                'GrdAnniProvvedimenti.Rows.Count = 0
                GrdAnniProvvedimenti.DataSource = dw
                GrdAnniProvvedimenti.DataBind()
                'If Pulisci Then
                '    GrdAnniProvvedimenti.SelectedIndex = -1
                'End If
            Else
                dw = objDSTipiInteressi.Tables(0).DefaultView
                GrdAnniProvvedimenti.DataSource = dw
            End If
            If Pulisci Then
                GrdAnniProvvedimenti.SelectedIndex = -1
            End If
            Select Case CInt(GrdAnniProvvedimenti.Rows.Count)
                'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
                Case 0
                    GrdAnniProvvedimenti.Visible = False
                    lblMessage.Text = "Nessun anno trovato"
                    lblMessage.Visible = True
                Case Is > 0
                    GrdAnniProvvedimenti.Visible = True
                    lblMessage.Visible = False
            End Select
            'dw.Dispose()
            'objDSTipiInteressi.Dispose()
        Catch ex As Exception
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.CaricaGriglia.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            'Finally
            '    If Not IsNothing(objSessione) Then
            '        objSessione.Kill()
            '        objSessione = Nothing
            '    End If
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
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdAnniProvvedimenti.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then

                        'fanno parte della vecchia versione delle tabelle ma NON vanno ancora bene
                        'strCodTributo = myRow.Cells(4).Text
                        'strCodTipoProvvedimento = myRow.Cells(5).Text
                        'txtQuotaRiduzioneSanzioni.Text = myRow.Cells(6).Text

                        'fanno parte della vecchia versione delle tabelle ma vanno ancora bene
                        strAnno = myRow.Cells(2).Text
                        strSogliaMinima = myRow.Cells(3).Text
                        'nuova versione
                        strCodTributo = CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value
                        strCodTipoProvvedimento = CType(myRow.FindControl("hfCOD_TIPO_PROVVEDIMENTO"), HiddenField).Value
                        txtQuotaRiduzioneSanzioni.Text = CType(myRow.FindControl("hfQUOTARIDUZIONESANZIONI"), HiddenField).Value

                        If strAnno.CompareTo("&nbsp;") = 0 Then strAnno = ""
                        If strSogliaMinima.CompareTo("&nbsp;") = 0 Then strSogliaMinima = ""
                        txtAnno.Text = strAnno
                        txtSoglia.Text = strSogliaMinima
                        txtOperazione.Text = "U"

                        COD_TRIBUTO = strCodTributo
                        PopolaCombo()

                        ddlTributo.SelectedValue = strCodTributo
                        ddlProvvedimenti.SelectedValue = strCodTipoProvvedimento
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdAnniProvvedimenti_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdAnniProvvedimenti.SelectedIndexChanged
    '    dim sScript as string=""
    '    Try
    '        strAnno = GrdAnniProvvedimenti.SelectedItem.Cells(2).Text
    '        strSogliaMinima = GrdAnniProvvedimenti.SelectedItem.Cells(3).Text
    '        strCodTributo = GrdAnniProvvedimenti.SelectedItem.Cells(4).Text
    '        strCodTipoProvvedimento = GrdAnniProvvedimenti.SelectedItem.Cells(5).Text
    '        '*** 20140701 - IMU/TARES ***
    '        txtQuotaRiduzioneSanzioni.Text = GrdAnniProvvedimenti.SelectedItem.Cells(6).Text
    '        '*** ***

    '        If strAnno.CompareTo("&nbsp;") = 0 Then strAnno = ""
    '        If strSogliaMinima.CompareTo("&nbsp;") = 0 Then strSogliaMinima = ""
    '        txtAnno.Text = strAnno
    '        txtSoglia.Text = strSogliaMinima
    '        txtOperazione.Text = "U"

    '        COD_TRIBUTO = strCodTributo
    '        PopolaCombo()

    '        ddlTributo.SelectedValue = strCodTributo
    '        ddlProvvedimenti.SelectedValue = strCodTipoProvvedimento
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.GrdAnniProvvedimenti_SelectedIndexChanged.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../Styles.css"))
    '        Response.End()
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAnniProvvedimenti.DataSource = CType(Session("dwConfAnniProv"), DataView)
            If page.HasValue Then
                GrdAnniProvvedimenti.PageIndex = page.Value
            End If
            GrdAnniProvvedimenti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim objHashTable As Hashtable = New Hashtable
        ''Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim sScript As String = "" 'strWFErrore, 

        Try
            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            'strConnectionStringOPENgovProvvedimenti =  objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO", strCodTributo)
            objHashTable.Add("CODTIPOPROVVEDIMENTO", strCodTipoProvvedimento)
            objHashTable.Add("ANNO", strAnno)
            objHashTable.Add("SOGLIAMINIMA", strSogliaMinima)
            '*** 20140701 - IMU/TARES ***
            objHashTable.Add("QUOTARIDUZIONESANZIONI", txtQuotaRiduzioneSanzioni.Text)
            '*** ***
            If strInsUp.CompareTo("") = 0 Then
                'Nuovo inserimento
                objHashTable.Add("INSUP", "I")
            Else
                'Modifica
                For Each myRow As GridViewRow In GrdAnniProvvedimenti.Rows
                    'originale
                    'If myRow.RowIndex = GrdAnniProvvedimenti.SelectedRow.RowIndex Then

                    If myRow.ID = CType(myRow.FindControl("hfid"), HiddenField).Value Then

                        objHashTable.Add("INSUP", strInsUp)
                        objHashTable.Add("CODTRIBUTO_OLD", myRow.Cells(4).Text)
                        objHashTable.Add("CODTIPOPROVVEDIMENTO_OLD", myRow.Cells(5).Text)
                        objHashTable.Add("ANNO_OLD", myRow.Cells(2).Text)
                    End If
                Next
            End If

            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objCOMValoriVoci.SetAnniProvvedimenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            PulisciCampi()

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
            If InStr(ex.Message, "ANNOPRESENTE") > 0 Then
                ddlProvvedimenti.SelectedValue = strCodTipoProvvedimento
                sScript += "GestAlert('a', 'warning', '', '', 'L'anno inserito per il tributo e il provvedimento selezionati è già presente.');"
                RegisterScript(sScript, Me.GetType())
            Else
                Log.Debug("ConfigAnniProvvedimenti::btnSalva_Click::si è verificato il seguente errore::", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End If
            'Finally
            '    If Not IsNothing(objSessione) Then
            '        objSessione.Kill()
            '        objSessione = Nothing
            '    End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Dim objHashTable As Hashtable = New Hashtable
        ''Dim objSessione As CreateSessione
        'Dim strWFErrore As String
        'Dim strConnectionStringOPENgovProvvedimenti As String

        Try
            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            'strConnectionStringOPENgovProvvedimenti =  objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            For Each myRow As GridViewRow In GrdAnniProvvedimenti.Rows
                If myRow.RowIndex = GrdAnniProvvedimenti.SelectedRow.RowIndex Then
                    strAnno = myRow.Cells(2).Text
                    strSogliaMinima = myRow.Cells(3).Text
                    strCodTributo = myRow.Cells(4).Text
                    strCodTipoProvvedimento = myRow.Cells(5).Text
                End If
            Next
            objHashTable.Add("CODTRIBUTO", strCodTributo)
            objHashTable.Add("CODTIPOPROVVEDIMENTO", strCodTipoProvvedimento)
            objHashTable.Add("ANNO", strAnno)
            objHashTable.Add("SOGLIAMINIMA", strSogliaMinima)

            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objCOMValoriVoci.DelAnniProvvedimenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            PulisciCampi()


        Catch ex As Exception
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigAnniProvvedimenti.btnCancella_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            'Finally
            '    If Not IsNothing(objSessione) Then
            '        objSessione.Kill()
            '        objSessione = Nothing
            '    End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        PulisciCampi()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PulisciCampi()
        ddlTributo.SelectedValue = -1
        ddlProvvedimenti.SelectedValue = -1
        txtAnno.Text = ""
        txtSoglia.Text = ""
        txtOperazione.Text = ""
        txtQuotaRiduzioneSanzioni.Text = "1"
        CaricaGriglia(True, True)
    End Sub
End Class
