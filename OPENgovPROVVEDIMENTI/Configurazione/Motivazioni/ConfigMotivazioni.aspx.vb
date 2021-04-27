Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione delle motivazioni.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigMotivazioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigMotivazioni))

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

    Dim strCOD_TRIBUTO, strCOD_VOCE, strCOD_MOTIVAZIONE, strDESC_MOTIVAZIONE, strID_MOTIVAZIONE, strOperazione As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        dim sScript as string=""
        Try
            strOperazione = Request.Item("txtOperazione")
            If Not Page.IsPostBack Then
                sscript+="parent.Comandi.location.href='ComandiConfigMotivazioni.aspx';"
                RegisterScript(sScript , Me.GetType())
                CaricaGriglia(False)
                PopolaComboTributo()
            End If
            Select Case strOperazione
                Case "E", "M", "S", "U"
                    strCOD_TRIBUTO = Trim(ddlTributo.SelectedValue)
                    strCOD_VOCE = Trim(ddlTipoVoci.SelectedValue)
                    strCOD_MOTIVAZIONE = Trim(txtCodMotivazione.Text)
                    strDESC_MOTIVAZIONE = Trim(txtDescMotivazione.Text)
                    strID_MOTIVAZIONE = Trim(txtID_MOTIVAZIONE.Text)


                Case "N"
                    strCOD_TRIBUTO = Trim(ddlTributo.SelectedValue)

                    PopolaComboVoce()

                Case Else
                    'CaricaGriglia(True)
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Pulisci"></param>
    ''' <param name="npage"></param>
    Sub CaricaGriglia(ByVal Pulisci As Boolean, Optional ByVal npage As Integer? = 0)
        Dim dw As DataView
        Dim objDSTipiInteressi As DataSet
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable
        Try
            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            'carico la hash table
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipiInteressi = objCOMTipoVoci.GetMotivazioni(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            If Not objDSTipiInteressi Is Nothing Then
                dw = objDSTipiInteressi.Tables(0).DefaultView
            End If

            GrdMotivazioni.DataSource = Nothing

            If Page.IsPostBack = False Or Pulisci Then
                GrdMotivazioni.DataSource = dw
                GrdMotivazioni.DataBind()
                If Pulisci Then
                    GrdMotivazioni.SelectedIndex = -1
                End If
            Else
                dw = objDSTipiInteressi.Tables(0).DefaultView
                GrdMotivazioni.DataSource = dw
                If npage.HasValue Then
                    GrdMotivazioni.PageIndex = npage.Value
                End If
                GrdMotivazioni.DataBind()
            End If
            Select Case CInt(GrdMotivazioni.Rows.Count)
                'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
                Case 0
                    GrdMotivazioni.Visible = False

                    lblMessage.Text = "Nessuna motivazione trovata"
                    lblMessage.Visible = True
                Case Is > 0
                    GrdMotivazioni.Visible = True
                    lblMessage.Visible = False
            End Select
        Catch ex As Exception
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.CaricaGriglia.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
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
                For Each myRow As GridViewRow In GrdMotivazioni.Rows
                    If IDRow = CType(myRow.FindControl("hfID_MOTIVAZIONE"), HiddenField).Value Then

                        '<asp:ImageButton runat = "server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_MOTIVAZIONE") %>' alt=""></asp:ImageButton>
                        '    <asp:HiddenField runat = "server" ID="" Value='<%# Eval("COD_TRIBUTO") %>' />
                        '    <asp:HiddenField runat = "server" ID="" Value='<%# Eval("") %>' />
                        '    <asp:HiddenField runat = "server" ID="" Value='<%# Eval("ID_MOTIVAZIONE") %>' />

                        'strCOD_TRIBUTO = myRow.Cells(4).Text
                        strCOD_TRIBUTO = CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value

                        'strCOD_VOCE = myRow.Cells(5).Text
                        strCOD_VOCE = CType(myRow.FindControl("hfCOD_VOCE"), HiddenField).Value

                        strCOD_MOTIVAZIONE = myRow.Cells(2).Text


                        strDESC_MOTIVAZIONE = myRow.Cells(3).Text

                        'strID_MOTIVAZIONE = myRow.Cells(6).Text
                        strID_MOTIVAZIONE = CType(myRow.FindControl("hfID_MOTIVAZIONE"), HiddenField).Value

                        strDESC_MOTIVAZIONE = replaceSpace(strDESC_MOTIVAZIONE)
                        strCOD_MOTIVAZIONE = replaceSpace(strCOD_MOTIVAZIONE)

                        txtCodMotivazione.Text = strCOD_MOTIVAZIONE
                        txtDescMotivazione.Text = strDESC_MOTIVAZIONE
                        PopolaComboTributo()
                        ddlTributo.SelectedValue = strCOD_TRIBUTO
                        PopolaComboVoce()
                        ddlTipoVoci.SelectedValue = strCOD_VOCE
                        txtID_MOTIVAZIONE.Text = strID_MOTIVAZIONE

                        Dim sScript As String = ""
                        sScript += "Modifica();"
                        sScript += "DisabilitaPulsanti();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        CaricaGriglia(False, e.NewPageIndex)
    End Sub
    'Private Sub GrdMotivazioni_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdMotivazioni.SelectedIndexChanged

    '    dim sScript as string=""

    '    Try
    '        strCOD_TRIBUTO = GrdMotivazioni.SelectedItem.Cells(4).Text
    '        strCOD_VOCE = GrdMotivazioni.SelectedItem.Cells(5).Text
    '        strCOD_MOTIVAZIONE = GrdMotivazioni.SelectedItem.Cells(2).Text
    '        strDESC_MOTIVAZIONE = GrdMotivazioni.SelectedItem.Cells(3).Text
    '        strID_MOTIVAZIONE = GrdMotivazioni.SelectedItem.Cells(6).Text
    '        strDESC_MOTIVAZIONE = replaceSpace(strDESC_MOTIVAZIONE)
    '        strCOD_MOTIVAZIONE = replaceSpace(strCOD_MOTIVAZIONE)

    '        txtCodMotivazione.Text = strCOD_MOTIVAZIONE
    '        txtDescMotivazione.Text = strDESC_MOTIVAZIONE
    '        PopolaComboTributo()
    '        ddlTributo.SelectedValue = strCOD_TRIBUTO
    '        PopolaComboVoce()
    '        ddlTipoVoci.SelectedValue = strCOD_VOCE
    '        txtID_MOTIVAZIONE.Text = strID_MOTIVAZIONE


    '        sscript+="<script language='javascript'>" & vbCrLf
    '        sscript+= "Modifica();" & vbCrLf
    '        sscript+= "DisabilitaPulsanti();" & vbCrLf
    '        
    '        RegisterScript(sScript , Me.GetType())

    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.GrdMotivazioni_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../Styles.css"))
    '        Response.End()
    '    End Try
    'End Sub
#End Region
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.PopolaComboTributo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboVoce()
        Dim Utility As New MyUtility
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
        Try
            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

            'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
            ddlTipoVoci.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlTipoVoci, objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.IdEnte, strCOD_TRIBUTO, "", ""), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.PopolaComboVoce.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'inserimento/modifica di una motivazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim objHashTable As New Hashtable
        Dim sScript As String = ""

        Try
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            If strCOD_TRIBUTO.CompareTo("-1") = 0 Or strCOD_VOCE.CompareTo("-1") = 0 Or strCOD_MOTIVAZIONE.CompareTo("") = 0 Or strDESC_MOTIVAZIONE.CompareTo("") = 0 Then
                CambiaVideata()
                sScript += "GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');"
                RegisterScript(sScript, Me.GetType())
                txtOperazione.Text = "N"
            Else
                objHashTable.Add("CODENTE", ConstSession.IdEnte)
                objHashTable.Add("COD_TRIBUTO", ddlTributo.SelectedValue)
                objHashTable.Add("COD_VOCE", ddlTipoVoci.SelectedValue)
                objHashTable.Add("COD_MOTIVAZIONE", txtCodMotivazione.Text)
                objHashTable.Add("DESC_MOTIVAZIONE", txtDescMotivazione.Text)

                If txtOperazione.Text.CompareTo("S") = 0 Then
                    'Nuovo inserimento
                    objHashTable.Add("INSUP", "I")
                    objHashTable.Add("COD_TRIBUTO_OLD", "")
                    objHashTable.Add("COD_VOCE_OLD", "")
                    objHashTable.Add("COD_MOTIVAZIONE_OLD", "")
                    objHashTable.Add("DESC_MOTIVAZIONE_OLD", "")
                Else
                    'Modifica
                    For Each myRow As GridViewRow In GrdMotivazioni.Rows
                        If CType(myRow.FindControl("hfID_MOTIVAZIONE"), HiddenField).Value = Utility.StringOperation.FormatInt(txtID_MOTIVAZIONE.Text) Then
                            objHashTable.Add("INSUP", txtOperazione.Text)
                            objHashTable.Add("ID_MOTIVAZIONE", Utility.StringOperation.FormatInt(txtID_MOTIVAZIONE.Text))
                            objHashTable.Add("COD_TRIBUTO_OLD", CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value)
                            objHashTable.Add("COD_VOCE_OLD", CType(myRow.FindControl("hfCOD_VOCE"), HiddenField).Value)
                            objHashTable.Add("COD_MOTIVAZIONE_OLD", myRow.Cells(2).Text)
                            objHashTable.Add("DESC_MOTIVAZIONE_OLD", myRow.Cells(3).Text)
                        End If
                    Next
                End If

                Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                objCOMValoriVoci.SetMotivazioni(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
                RicaricaPagina()
            End If
        Catch ex As Exception
            If InStr(ex.Message, "MOTIVAZIONEPRESENTE") > 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'La motivazione è già presente a sistema');"
                RegisterScript(sScript, Me.GetType())
            Else
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.btnSalva_Click.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End If
        End Try
    End Sub
    'Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
    '    Dim objHashTable As Hashtable = New Hashtable
    '    'Dim objSessione As CreateSessione
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim sScript As String = ""
    '    Dim strInsUp As String

    '    Try
    '        'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("USER", ConstSession.UserName)

    '        If strCOD_TRIBUTO.CompareTo("-1") = 0 Or strCOD_VOCE.CompareTo("-1") = 0 Or strCOD_MOTIVAZIONE.CompareTo("") = 0 Or strDESC_MOTIVAZIONE.CompareTo("") = 0 Then
    '            CambiaVideata()
    '            sScript += "GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');"
    '            RegisterScript(sScript, Me.GetType())
    '            txtOperazione.Text = "N"
    '        Else
    '            objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '            objHashTable.Add("COD_TRIBUTO", strCOD_TRIBUTO)
    '            objHashTable.Add("COD_VOCE", strCOD_VOCE)
    '            objHashTable.Add("COD_MOTIVAZIONE", strCOD_MOTIVAZIONE)
    '            objHashTable.Add("DESC_MOTIVAZIONE", strDESC_MOTIVAZIONE)

    '            strInsUp = txtOperazione.Text
    '            If strInsUp.CompareTo("S") = 0 Then
    '                'Nuovo inserimento
    '                objHashTable.Add("INSUP", "I")
    '                objHashTable.Add("COD_TRIBUTO_OLD", "")
    '                objHashTable.Add("COD_VOCE_OLD", "")
    '                objHashTable.Add("COD_MOTIVAZIONE_OLD", "")
    '                objHashTable.Add("DESC_MOTIVAZIONE_OLD", "")
    '            Else
    '                'Modifica
    '                For Each myRow As GridViewRow In GrdMotivazioni.Rows
    '                    If myRow.RowIndex = GrdMotivazioni.SelectedRow.RowIndex Then
    '                        objHashTable.Add("INSUP", strInsUp)
    '                        objHashTable.Add("ID_MOTIVAZIONE", myRow.Cells(6).Text)
    '                        objHashTable.Add("COD_TRIBUTO_OLD", myRow.Cells(4).Text)
    '                        objHashTable.Add("COD_VOCE_OLD", myRow.Cells(5).Text)
    '                        objHashTable.Add("COD_MOTIVAZIONE_OLD", myRow.Cells(2).Text)
    '                        objHashTable.Add("DESC_MOTIVAZIONE_OLD", myRow.Cells(3).Text)
    '                    End If
    '                Next
    '            End If

    '            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '            objCOMValoriVoci.SetMotivazioni(objHashTable)
    '            'ModificaIndietro()
    '            RicaricaPagina()
    '        End If
    '    Catch ex As SqlClient.SqlException
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.btnSalva_Click.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        'If Not IsNothing(objSessione) Then
    '        '    objSessione.Kill()
    '        '    objSessione = Nothing
    '        'End If
    '        If InStr(ex.Message, "MOTIVAZIONEPRESENTE") > 0 Then
    '            sScript += "GestAlert('a', 'warning', '', '', 'La motivazione è già presente a sistema');"
    '            'sscript+= "abilitaConfigura();" & vbCrLf
    '            RegisterScript(sScript, Me.GetType())
    '        Else
    '            Response.Redirect("../../../PaginaErrore.aspx")
    '        End If
    '    Finally
    '        'If Not IsNothing(objSessione) Then
    '        '    objSessione.Kill()
    '        '    objSessione = Nothing
    '        'End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Dim objHashTable As Hashtable = New Hashtable

        Try
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            'Per eliminare una note prendo i dati dalla griglia
            objHashTable.Add("ID_MOTIVAZIONE", strID_MOTIVAZIONE)

            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objCOMValoriVoci.DelMotivazioni(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            RicaricaPagina()

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigMotivazioni.btnElimina_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        CambiaVideata()
        ddlTributo.SelectedIndex = -1
        ddlTipoVoci.SelectedIndex = -1
        txtCodMotivazione.Text = ""
        txtDescMotivazione.Text = ""
        txtID_MOTIVAZIONE.Text = ""

        txtOperazione.Text = "N"
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        PopolaComboTributo()
        ddlTributo.Enabled = True
        ddlTipoVoci.Enabled = True
        txtCodMotivazione.Enabled = True
        txtDescMotivazione.Enabled = True
        txtOperazione.Text = "N"
        CambiaVideata()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAbilita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbilita.Click
        CambiaVideata()
        ddlTributo.Enabled = True
        ddlTipoVoci.Enabled = True
        txtCodMotivazione.Enabled = True
        txtDescMotivazione.Enabled = True

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnIndietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIndietro.Click

        RicaricaPagina()
        'txtOperazione.Text = ""
        'ddlTributo.Items.Clear()
        'ddlTipoVoci.Items.Clear()
        'txtCodMotivazione.Text = ""
        'txtDescMotivazione.Text = ""
        'txtID_MOTIVAZIONE.Text = ""

        'ddlTributo.Enabled = False
        'ddlTipoVoci.Enabled = False
        'txtCodMotivazione.Enabled = False
        'txtDescMotivazione.Enabled = False
        'CaricaGriglia(True)
        ''GrdMotivazioni.SelectedIndex = -1

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CambiaVideata()
        Dim sScript As String = ""
        sScript += "Modifica();"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub RicaricaPagina()
        Dim sScript As String = ""
        sScript += "RicaricaPagina();"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ModificaIndietro()

        Dim sScript As String = ""
        txtOperazione.Text = ""
        ddlTributo.Items.Clear()
        ddlTipoVoci.Items.Clear()
        txtCodMotivazione.Text = ""
        txtDescMotivazione.Text = ""
        txtID_MOTIVAZIONE.Text = ""

        ddlTributo.Enabled = False
        ddlTipoVoci.Enabled = False
        txtCodMotivazione.Enabled = False
        txtDescMotivazione.Enabled = False

        CaricaGriglia(True)
        GrdMotivazioni.SelectedIndex = -1
        sScript += "ModificaIndietro();"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTributo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTributo.SelectedIndexChanged
        CambiaVideata()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="valore"></param>
    ''' <returns></returns>
    Private Function replaceSpace(ByVal valore As String) As String
        replaceSpace = valore.Replace("&nbsp;", "")
    End Function

End Class
