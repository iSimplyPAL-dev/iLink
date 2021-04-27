Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione delle sanzioni.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigTipologieSanzioni
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigTipologieSanzioni))

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

    Dim strCOD_TRIBUTO, strCOD_VOCE, strDESC_SANZIONE, strOperazione As String

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
                sscript+="parent.Comandi.location.href='ComandiConfigTipologieSanzioni.aspx';"
                RegisterScript(sScript, Me.GetType())
                CaricaGriglia(False)
                PopolaComboTributo()
                Select Case strOperazione
                    Case "E", "M", "S", "U"
                        strCOD_TRIBUTO = Trim(ddlTributo.SelectedValue)
                        'strCOD_VOCE = Trim(ddlTipoVoci.SelectedValue)
                        strCOD_VOCE = Trim(txtCodVoce.Text)
                        strDESC_SANZIONE = Trim(txtDescSanzione.Text)
                    Case "N"
                        strCOD_TRIBUTO = Trim(ddlTributo.SelectedValue)
                        PopolaComboVoce()
                    Case Else
                        CaricaGriglia(False)
                End Select
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.Page_Load.errore: ", ex)
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
        Dim objHashTable As Hashtable = New Hashtable
        Try
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipiInteressi = objCOMTipoVoci.GetTipologieVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            If Not objDSTipiInteressi Is Nothing Then
                dw = objDSTipiInteressi.Tables(0).DefaultView
            End If

            GrdSanzioni.DataSource = Nothing

            If Page.IsPostBack = False Or Pulisci Then
                GrdSanzioni.DataSource = dw
                GrdSanzioni.DataBind()
                If Pulisci Then
                    GrdSanzioni.SelectedIndex = -1
                End If
            Else
                dw = objDSTipiInteressi.Tables(0).DefaultView
                GrdSanzioni.DataSource = dw
                If npage.HasValue Then
                    GrdSanzioni.PageIndex = npage.Value
                End If
                GrdSanzioni.DataBind()
            End If
            Select Case CInt(GrdSanzioni.Rows.Count)
                Case 0
                    GrdSanzioni.Visible = False

                    lblMessage.Text = "Nessuna Tipologia voce trovata"
                    lblMessage.Visible = True
                Case Is > 0
                    GrdSanzioni.Visible = True
                    lblMessage.Visible = False
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.CaricaGriglia.errore: ", ex)
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
                For Each myRow As GridViewRow In GrdSanzioni.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        hfIdRow.Value = IDRow
                        strDESC_SANZIONE = myRow.Cells(2).Text
                        'old
                        'strCOD_TRIBUTO = myRow.Cells(3).Text
                        strCOD_TRIBUTO = CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value
                        strCOD_VOCE = myRow.Cells(1).Text
                        strDESC_SANZIONE = replaceSpace(strDESC_SANZIONE)
                        txtDescSanzione.Text = strDESC_SANZIONE
                        PopolaComboTributo()
                        ddlTributo.SelectedValue = strCOD_TRIBUTO
                        PopolaComboVoce()
                        txtCodVoce.Text = strCOD_VOCE
                        'ddlTipoVoci.SelectedValue = strCOD_VOCE
                        Dim sScript As String = ""
                        sScript += "Modifica();" & vbCrLf
                        sScript += "DisabilitaPulsanti();" & vbCrLf
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.GrdRowCommand.errore: ", ex)
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
    'Private Sub GrdSanzioni_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdSanzioni.SelectedIndexChanged
    '    dim sScript as string=""
    '    Try
    '        strDESC_SANZIONE = GrdSanzioni.SelectedItem.Cells(2).Text
    '        strCOD_TRIBUTO = GrdSanzioni.SelectedItem.Cells(3).Text
    '        strCOD_VOCE = GrdSanzioni.SelectedItem.Cells(1).Text

    '        strDESC_SANZIONE = replaceSpace(strDESC_SANZIONE)

    '        txtDescSanzione.Text = strDESC_SANZIONE
    '        PopolaComboTributo()
    '        ddlTributo.SelectedValue = strCOD_TRIBUTO
    '        PopolaComboVoce()
    '        txtCodVoce.Text = strCOD_VOCE
    '        'ddlTipoVoci.SelectedValue = strCOD_VOCE


    '        sscript+="<script language='javascript'>" & vbCrLf
    '        sscript+= "Modifica();" & vbCrLf
    '        sscript+= "DisabilitaPulsanti();" & vbCrLf
    '        
    '        RegisterScript(sScript , Me.GetType())
    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.GrdSanzioni_SelectedIndexChanged.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.PopolaComboTributo.errore: ", ex)
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
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim sScript As String = ""
        Dim strInsUp As String

        Try
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)

            'M.B.
            strCOD_TRIBUTO = Trim(ddlTributo.SelectedValue)
            'strCOD_VOCE = Trim(ddlTipoVoci.SelectedValue)
            strCOD_VOCE = Trim(txtCodVoce.Text)
            strDESC_SANZIONE = Trim(txtDescSanzione.Text)

            If strCOD_TRIBUTO.CompareTo("-1") = 0 Or strCOD_VOCE.CompareTo("-1") = 0 Or
                strDESC_SANZIONE.CompareTo("") = 0 Then
                CambiaVideata()

                sScript += "GestAlert('a', 'warning', '', '', 'Tutti i dati sono obbligatori');"
                RegisterScript(sScript, Me.GetType())
                txtOperazione.Text = "N"
            Else
                objHashTable.Add("CODENTE", ConstSession.IdEnte)
                objHashTable.Add("COD_TRIBUTO", strCOD_TRIBUTO)
                objHashTable.Add("COD_VOCE", strCOD_VOCE)
                objHashTable.Add("DESC_SANZIONE", strDESC_SANZIONE)

                strInsUp = txtOperazione.Text
                If strInsUp.CompareTo("S") = 0 Then
                    'Nuovo inserimento
                    objHashTable.Add("INSUP", "I")
                    objHashTable.Add("COD_TRIBUTO_OLD", "")
                    objHashTable.Add("COD_VOCE_OLD", "")
                    objHashTable.Add("DESC_SANZIONE_OLD", "")
                Else
                    'Modifica
                    For Each myRow As GridViewRow In GrdSanzioni.Rows
                        If CType(myRow.FindControl("hfid"), HiddenField).Value = hfIdRow.Value Then
                            objHashTable.Add("INSUP", strInsUp)
                            objHashTable.Add("COD_TRIBUTO_OLD", CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value)
                            objHashTable.Add("COD_VOCE_OLD", myRow.Cells(1).Text)
                            objHashTable.Add("DESC_SANZIONE_OLD", myRow.Cells(2).Text)
                        End If
                    Next
                End If

                Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                objCOMValoriVoci.SetTipologieVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

                'ModificaIndietro()
                RicaricaPagina()

            End If
        Catch ex As Exception
            If InStr(ex.Message, "Sanzione già presente") > 0 Then
                sScript += "Modifica();"
                sScript += "GestAlert('a', 'warning', '', '', 'La Tipologia voce inserita è già presente');"
                RegisterScript(sScript, Me.GetType())
            ElseIf InStr(ex.Message, "PRESENTE_TIPO_VOCE") > 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Esistono dei valori voci configurati con la voce presa in esame.\nLe modifiche non verranno effettuate.');"
                RegisterScript(sScript, Me.GetType())
            Else
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.btnSalva_Click.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End If
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
    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Dim objHashTable As Hashtable = New Hashtable
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim sScript As String = ""
        Try

            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            strCOD_TRIBUTO = Trim(ddlTributo.SelectedValue)
            strCOD_VOCE = Trim(txtCodVoce.Text)
            objHashTable.Add("COD_TRIBUTO", strCOD_TRIBUTO)
            objHashTable.Add("COD_VOCE", strCOD_VOCE)


            Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objCOMValoriVoci.DelTipologieVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            ModificaIndietro()
        Catch ex As Exception
            If (InStr(ex.Message, "TipologiaVoceUtilizzata") > 0) Then
                sScript += "Modifica();"
                sScript += "GestAlert('a', 'warning', '', '', 'La tipologia voce da eliminare è utilizzata');"
                RegisterScript(sScript, Me.GetType())
            Else
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.btnElimina_Click.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End If
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
        Try
            CambiaVideata()
            ddlTributo.SelectedIndex = -1
            'ddlTipoVoci.SelectedIndex = -1
            txtDescSanzione.Text = ""
            txtCodVoce.Text = ""
            txtOperazione.Text = "N"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.btnPulisci_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        Try
            PopolaComboTributo()
            ddlTributo.Enabled = True
            'ddlTipoVoci.Enabled = True
            txtDescSanzione.Enabled = True
            txtCodVoce.Enabled = True
            txtOperazione.Text = "N"
            CambiaVideata()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.btnNuova_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAbilita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbilita.Click
        Try
            CambiaVideata()
            ddlTributo.Enabled = True
            'ddlTipoVoci.Enabled = True
            txtDescSanzione.Enabled = True
            txtCodVoce.Enabled = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.btnAbilita_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnIndietro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIndietro.Click
        Try
            txtOperazione.Text = ""
            ddlTributo.Items.Clear()
            'ddlTipoVoci.Items.Clear()
            txtDescSanzione.Text = ""
            ddlTributo.Enabled = False
            'ddlTipoVoci.Enabled = False
            txtDescSanzione.Enabled = False
            txtCodVoce.Enabled = False
            txtCodVoce.Text = ""

            CaricaGriglia(True)
            'GrdSanzioni.SelectedIndex = -1
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.btnIndietro_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CambiaVideata()
        Dim sScript As String = ""

        sScript += "Modifica();" & vbCrLf

        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ModificaIndietro()
        Dim sScript As String = ""
        Try
            txtOperazione.Text = ""
            ddlTributo.Items.Clear()
            'ddlTipoVoci.Items.Clear()
            txtDescSanzione.Text = ""

            ddlTributo.Enabled = False
            'ddlTipoVoci.Enabled = False
            txtDescSanzione.Enabled = False
            txtCodVoce.Enabled = False
            txtCodVoce.Text = ""

            CaricaGriglia(True)
            GrdSanzioni.SelectedIndex = -1

            sScript += "ModificaIndietro();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTipologieSanzioni.ModificaIndietro.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
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
