Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione degli interessi.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigTassiInteressi
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigTassiInteressi))
    Dim ModDate As New ModificaDate
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

    Dim CODTRIBUTO, CODTIPOINTERESSE, DAL, AL, TASSO, Operazione, DESCTIPOINTERESSE As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Dim dw As DataView
        Dim blnResult As Boolean = False
        'Dim objSessione As CreateSessione
        Dim strWFErrore As String
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable

        Try
            Operazione = Trim(txtOperazione.Text)
            CODTIPOINTERESSE = Trim(ddlTipoInteresse.SelectedValue)
            If Not IsNothing(ddlTipoInteresse.SelectedItem) Then
                DESCTIPOINTERESSE = Trim(ddlTipoInteresse.SelectedItem.Text)
            Else
                DESCTIPOINTERESSE = ""
            End If
            DAL = Trim(txtDal.Text)
            AL = Trim(txtAl.Text)
            TASSO = Trim(txtTasso.Text)
            CODTRIBUTO = Trim(ddlTributo.SelectedValue)

            If Not Page.IsPostBack Then
                Session("dvTassiInteresse") = Nothing
                CaricaGriglia(False)

                ddlTipoInteresse.Items.Clear()
                PopolaComboTributo()
                PopolaComboTipoInteresse()
            Else
                dw = CType(Session("dvTassiInteresse"), DataView)
                GrdInteressi.DataSource = dw
            End If
            sScript += "parent.Comandi.location.href='ComandiConfigTassiInteressi.aspx'" & vbCrLf
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            strWFErrore = ""
        End Try
    End Sub

    'Function CaricaGriglia(ByVal Pulisci As Boolean)
    '    Dim dw As DataView
    '    Dim objDSTipiInteressi As DataSet
    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim objHashTable As Hashtable = New Hashtable

    '    Try
    '        objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'carico la hash table
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("USER", ConstSession.UserName)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)

    '        objHashTable.Add("CODTIPOINTERESSE", "-1")
    '        objHashTable.Add("DESCTIPOINTERESSE", "")
    '        objHashTable.Add("DAL", "")
    '        objHashTable.Add("AL", "")
    '        objHashTable.Add("TASSO", "")
    '        If Not CODTRIBUTO Is Nothing And CODTRIBUTO.CompareTo("-1") <> 0 Then
    '            objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
    '        Else
    '            objHashTable.Add("CODTRIBUTO", "")
    '        End If

    '        Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '        objDSTipiInteressi = objCOMTipoVoci.GetTipoInteresse(objHashTable)
    '        If Not objDSTipiInteressi Is Nothing Then
    '            dw = objDSTipiInteressi.Tables(0).DefaultView
    '        End If
    '        If Page.IsPostBack = False Or Pulisci Then
    '            GrdInteressi.Rows.Count = 0
    '            GrdInteressi.start_index = 0
    '            GrdInteressi.DataSource = dw
    '            GrdInteressi.DataBind()
    '            If Pulisci Then
    '                GrdInteressi.SelectedIndex = -1
    '            End If
    '            Session("dvTassiInteresse") = dw
    '        Else
    '            GrdInteressi.AllowCustomPaging = False
    '            GrdInteressi.start_index = GrdInteressi.CurrentPageIndex
    '            dw = objDSTipiInteressi.Tables(0).DefaultView
    '            GrdInteressi.DataSource = dw
    '        End If

    '        Select Case CInt(GrdInteressi.Rows.Count)
    '            'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
    '            Case 0
    '                GrdInteressi.Visible = False
    '                lblMessage.Text = "Nessun Tasso di interesse trovato"
    '                lblMessage.Visible = True
    '            Case Is > 0
    '                GrdInteressi.Visible = True
    '                lblMessage.Visible = False
    '        End Select
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.CaricaGriglia.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    '        If Not IsNothing(objSessione) Then
    '            objSessione.Kill()
    '            objSessione = Nothing
    '        End If
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
    '        Response.End()
    '    Finally
    '        If Not IsNothing(objSessione) Then
    '            objSessione.Kill()
    '            objSessione = Nothing
    '        End If
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Pulisci"></param>
    ''' <param name="npage"></param>
    Sub CaricaGriglia(ByVal Pulisci As Boolean, Optional ByVal npage As Integer? = 0)
        Dim dw As DataView
        Dim objDSTipiInteressi As DataSet
        'Dim strWFErrore As String
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable

        Try
            'carico la hash table
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CODTIPOINTERESSE", "-1")
            objHashTable.Add("DESCTIPOINTERESSE", "")
            objHashTable.Add("DAL", "")
            objHashTable.Add("AL", "")
            objHashTable.Add("TASSO", "")
            If Not CODTRIBUTO Is Nothing And CODTRIBUTO.CompareTo("-1") <> 0 Then
                objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
            Else
                objHashTable.Add("CODTRIBUTO", "")
            End If

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipiInteressi = objCOMTipoVoci.GetTipoInteresse(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            If Not objDSTipiInteressi Is Nothing Then
                dw = objDSTipiInteressi.Tables(0).DefaultView
            End If
            If Page.IsPostBack = False Or Pulisci Then
                GrdInteressi.DataSource = dw
                GrdInteressi.DataBind()
                If Pulisci Then
                    GrdInteressi.SelectedIndex = -1
                End If
                Session("dvTassiInteresse") = dw
            Else
                dw = objDSTipiInteressi.Tables(0).DefaultView
                GrdInteressi.DataSource = dw
                If npage.HasValue Then
                    GrdInteressi.PageIndex = npage.Value
                End If
                GrdInteressi.DataBind()
            End If

            Select Case CInt(GrdInteressi.Rows.Count)
                'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
                Case 0
                    GrdInteressi.Visible = False
                    lblMessage.Text = "Nessun Tasso di interesse trovato"
                    lblMessage.Visible = True
                Case Is > 0
                    GrdInteressi.Visible = True
                    lblMessage.Visible = False
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.CaricaGriglia.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboTipoInteresse()
        Dim Utility As New MyUtility
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            If ddlTributo.SelectedValue <> "-1" Then
                CODTRIBUTO = ddlTributo.SelectedValue
            End If
            'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
            ddlTipoInteresse.Items.Clear()
            'Utility.FillDropDownSQLValueString(ddlTipoInteresse, objGestOPENgovProvvedimenti.GetTipoInteresse("", ""), -1, "...")
            Utility.FillDropDownSQLValueString(ddlTipoInteresse, objGestOPENgovProvvedimenti.GetTipoInteresse(CODTRIBUTO, "", cmdMyCommand), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.PopolaComboTipoInteresse.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="prdStatus"></param>
    ''' <returns></returns>
    Protected Function GiraData(ByVal prdStatus As Object) As String
        Dim objUtility As New MyUtility
        GiraData = ModDate.GiraDataFromDB(objUtility.CToStr(prdStatus))
        Return GiraData
    End Function

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim objUtility As New MyUtility
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdInteressi.Rows
                    If IDRow = CType(myRow.FindControl("hfUNICO"), HiddenField).Value Then
                        Dim strCodTipoInteresse, strTipoInteresse, strDal, strAl, strTasso, strCodTributo As String

                        'Dim ciccio = CType(myRow.FindControl("hfUNICO"), HiddenField).Value

                        'strCodTipoInteresse = myRow.Cells(8).Text()
                        strCodTipoInteresse = CType(myRow.FindControl("hfCOD_TIPO_INTERESSE"), HiddenField).Value


                        'strCodTributo = myRow.Cells(5).Text()
                        strCodTributo = CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value

                        strTipoInteresse = myRow.Cells(0).Text()

                        'strDal = myRow.Cells(6).Text()
                        strDal = CType(myRow.FindControl("hfDAL"), HiddenField).Value
                        strDal = replaceSpace(strDal)
                        strDal = ModDate.GiraDataFromDB(strDal)

                        'strAl = myRow.Cells(7).Text()
                        strAl = CType(myRow.FindControl("hfAL"), HiddenField).Value
                        strAl = replaceSpace(strAl)
                        strAl = ModDate.GiraDataFromDB(strAl)

                        strTasso = myRow.Cells(3).Text()
                        strTasso = replaceSpace(strTasso)
                        ddlTipoInteresse.SelectedValue = strCodTipoInteresse
                        ddlTributo.SelectedValue = strCodTributo
                        txtDal.Text = strDal
                        txtAl.Text = strAl
                        txtTasso.Text = strTasso
                        txtOperazione.Text = "M"
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.GrdRowCommand.errore: ", ex)
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
    'Private Sub GrdInteressi_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdInteressi.SelectedIndexChanged
    '    Dim strCodTipoInteresse, strTipoInteresse, strDal, strAl, strTasso, strCodTributo As String
    '    dim sScript as string=""
    '    Dim objUtility As New MyUtility
    '    Try
    '        'strCodTipoInteresse = GrdInteressi.SelectedItem.Cells(6).Text()
    '        strCodTipoInteresse = GrdInteressi.SelectedItem.Cells(8).Text()
    '        strTipoInteresse = GrdInteressi.SelectedItem.Cells(0).Text()
    '        strCodTributo = GrdInteressi.SelectedItem.Cells(5).Text()
    '        'strDal = GrdInteressi.SelectedItem.Cells(4).Text()
    '        strDal = GrdInteressi.SelectedItem.Cells(6).Text()
    '        strDal = replaceSpace(strDal)
    '        strDal = ModDate.GiraDataFromDB(objUtility.CToStr(strDal))

    '        'strAl = GrdInteressi.SelectedItem.Cells(5).Text()
    '        strAl = GrdInteressi.SelectedItem.Cells(7).Text()
    '        strAl = replaceSpace(strAl)
    '        strAl = ModDate.GiraDataFromDB(objUtility.CToStr(strAl))


    '        strTasso = GrdInteressi.SelectedItem.Cells(3).Text()
    '        strTasso = replaceSpace(strTasso)

    '        ddlTipoInteresse.SelectedValue = strCodTipoInteresse
    '        ddlTributo.SelectedValue = strCodTributo
    '        txtDal.Text = strDal
    '        txtAl.Text = strAl
    '        txtTasso.Text = strTasso
    '        txtOperazione.Text = "M"
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.GrInteressi_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
    '        Response.End()
    '    End Try

    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable
        Dim objDSTipiInteressi As DataSet
        Dim sScript As String = ""
        Dim EmptyAL As Boolean = False
        Dim objCOMTipiInteressi As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
        Dim oUtility As New DLL.ProvvUtility

        Try

            If DAL.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'La data Dal è obbligatoria');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If
            If CODTIPOINTERESSE.CompareTo("-1") = 0 And CODTIPOINTERESSE.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare il tipo di interesse');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            If AL.CompareTo("") <> 0 Then
                If DateDiff(DateInterval.Day, CDate(AL), CDate(DAL)) > 0 Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Data DAL deve essere maggiore della Data AL');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
            End If

            'carico la hash table
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CODTIPOINTERESSE", CODTIPOINTERESSE)
            objHashTable.Add("DESCTIPOINTERESSE", DESCTIPOINTERESSE)
            objHashTable.Add("DAL", "")
            objHashTable.Add("AL", "")
            objHashTable.Add("TASSO", "")
            objHashTable.Add("CODTRIBUTO", CODTRIBUTO)

            If Operazione.CompareTo("M") = 0 Then
                'Modifica
                objHashTable.Add("INSUP", "U")
            Else
                'Nuovo inserimento
                objHashTable.Add("INSUP", "I")
            End If

            objDSTipiInteressi = objCOMTipiInteressi.GetTipoInteresse(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            Dim dw As DataView

            'Dim NewColumn As New DataColumn
            'NewColumn.ColumnName = "RECORDDAINSERIRE"
            'NewColumn.DataType = GetType(System.Boolean)
            'NewColumn.DefaultValue = 0
            'objDSTipiInteressi.Tables("0").Columns.Add(NewColumn)

            dw = objDSTipiInteressi.Tables("TASSI_INTERESSE").DefaultView

            If Operazione.CompareTo("M") <> 0 Then
                Dim NewRow As DataRow
                NewRow = dw.Table.NewRow()

                NewRow.Item("COD_ENTE") = ConstSession.IdEnte
                NewRow.Item("DAL") = ModDate.GiraData(DAL).ToString()
                NewRow.Item("AL") = ModDate.GiraData(AL).ToString()
                NewRow.Item("TASSO_ANNUALE") = TASSO
                NewRow.Item("COD_TIPO_INTERESSE") = objHashTable("CODTIPOINTERESSE")
                NewRow.Item("COD_TRIBUTO") = objHashTable("CODTRIBUTO")
                'NewRow.Item("RECORDDAINSERIRE") = 1

                dw.Table.Rows.Add(NewRow)
            End If


            dw.Sort = "DAL ASC"
            dw.Table.AcceptChanges()

            Dim i As Integer
            Dim sDAL, sAL As String

            For i = 0 To dw.Count - 1
                sDAL = dw.Item(i)("DAL")
                sAL = oUtility.CToString(dw.Item(i)("AL"))

                If sAL = "" Then
                    'se non sono sull'ultimo record, ERRORE!
                    If i <> dw.Count - 1 Then
                        sScript += "GestAlert('a', 'warning', '', '', 'Attenzione! Le date degli interessi \'" & DESCTIPOINTERESSE & "\'devono essere consecutive e coerenti.\nIn caso contrario l\'importo interessi verra\' calcolato in modo errato.');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If

                End If
                If i <> 0 Then
                    Dim miadataDAL As Date = New Date(CInt(Left(dw.Item(i)("DAL"), 4)), CInt(Mid(dw.Item(i)("DAL"), 5, 2)), CInt(Right(dw.Item(i)("DAL"), 2)))
                    Dim miadataAL As Date = New Date(CInt(Left(dw.Item(i - 1)("AL"), 4)), CInt(Mid(dw.Item(i - 1)("AL"), 5, 2)), CInt(Right(dw.Item(i - 1)("AL"), 2)))

                    If DateDiff(DateInterval.Day, miadataDAL, miadataAL) <> -1 Then
                        sScript += "GestAlert('a', 'warning', '', '', 'Attenzione! Le date degli interessi \'" & DESCTIPOINTERESSE & "\'devono essere consecutive e coerenti.\nIn caso contrario l\'importo interessi verra\' calcolato in modo errato.');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                End If
            Next

            objHashTable("DAL") = ModDate.GiraData(DAL)
            objHashTable("AL") = ModDate.GiraData(AL)
            objHashTable("TASSO") = TASSO

            objCOMTipiInteressi.SetTassiInteresse(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            PulisciCampi()

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.btbSalva_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub


    'Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click


    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objDSTipiInteressi As DataSet
    '    dim sScript as string=""
    '    Dim EmptyAL As Boolean = False
    '    Dim objCOMTipiInteressi As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '    Dim dsDate As DataSet
    '    Dim maxDate, maxDateOR, strDalSum As String
    '    Dim DalSum As Date

    '    Try

    '        If DAL.CompareTo("") = 0 Then
    '            
    '            sscript+="alert(""La data 'Dal' è obbligatoria"");")
    '            
    '            RegisterScript(sScript , Me.GetType())
    '            Exit Sub
    '        End If
    '        If CODTIPOINTERESSE.CompareTo("-1") = 0 And CODTIPOINTERESSE.CompareTo("") = 0 Then
    '            
    '            sscript+="alert(""Selezionare il tipo di interesse"");")
    '            
    '            RegisterScript(sScript , Me.GetType())
    '            Exit Sub
    '        End If

    '        If AL.CompareTo("") <> 0 Then
    '            If DateDiff(DateInterval.Day, CDate(AL), CDate(DAL)) <> 0 Then
    '                
    '                sscript+="alert(""Data DAL deve essere maggiore della Data AL"");")
    '                
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '        End If

    '        objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'carico la hash table
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("USER", ConstSession.UserName)
    '        objHashTable.Add("CODENTE", constsession.idente)

    '        objHashTable.Add("CODTIPOINTERESSE", CODTIPOINTERESSE)
    '        objHashTable.Add("DAL", ModDate.GiraData(DAL))
    '        objHashTable.Add("AL", ModDate.GiraData(AL))
    '        objHashTable.Add("TASSO", TASSO)

    '        If Operazione.CompareTo("M") = 0 Then
    '            'Modifica
    '            objHashTable.Add("INSUP", "U")
    '        Else
    '            'Nuovo inserimento
    '            objHashTable.Add("INSUP", "I")

    '        End If


    '        objDSTipiInteressi = objCOMTipiInteressi.GetTipoInteresse(objHashTable)

    '        'controllo che ci siano dei record 
    '        If objDSTipiInteressi.Tables(0).Rows.Count > 0 Then
    '            'se si, ciclo sui record per effettualre i controlli di congruenza per l'inserimento



    '        End If



    '        If objDSTipiInteressi.Tables(0).Rows.Count > 0 And Operazione.CompareTo("I") = 0 Then
    '            
    '            sscript+="alert(""Tipo di interesse già presente"");")
    '            
    '            RegisterScript(sScript , Me.GetType())
    '        Else

    '            'Controllo che la nuova data DAL da inserire sia valida
    '            'cioè che sia maggiore di un giorno rispetto all'ultima data
    '            'AL inserita per COD_ENTE E COD_TIPO_INTERESSE

    '            dsDate = objCOMTipiInteressi.GetMaxDateTassiInteresse(objHashTable)
    '            If dsDate.Tables(0).Rows.Count = 1 Then

    '                If Not IsDBNull(dsDate.Tables(0).Rows(0).Item("maxDateOR")) Then
    '                    maxDateOR = dsDate.Tables(0).Rows(0).Item("maxDateOR")

    '                    If DateDiff(DateInterval.Day, CDate(ModDate.GiraDataFromDB(maxDateOR)), CDate(DAL)) <> 1 Then
    '                        
    '                        sscript+="alert(""La data di inizio periodo deve essere maggiore di un giorno\nrispetto alla data di fine periodo '" & ModDate.GiraDataFromDB(maxDateOR) & "'"");")
    '                        sscript+="document.getElementById('txtDal').focus()")
    '                        
    '                        RegisterScript(sScript , Me.GetType())
    '                        ddlTipoInteresse.SelectedValue = CODTIPOINTERESSE
    '                    Else
    '                        objCOMTipiInteressi.SetTassiInteresse(objHashTable)
    '                        PulisciCampi()
    '                    End If
    '                Else
    '                    objCOMTipiInteressi.SetTassiInteresse(objHashTable)
    '                    PulisciCampi()
    '                End If
    '            End If
    '        End If



    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.btnElimina_Click.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
    '        Response.End()

    '    End Try


    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        'Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable

        Dim sScript As String = ""
        Try
            If DAL.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'La data Dal è obbligatoria');"
                RegisterScript(sScript, Me.GetType())
            ElseIf CODTIPOINTERESSE.CompareTo("-1") = 0 And CODTIPOINTERESSE.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare il tipo di interesse');"
                RegisterScript(sScript, Me.GetType())
            Else
                If Operazione.CompareTo("M") = 0 Then
                    'Modifica
                    objHashTable.Add("INSUP", "U")
                Else
                    'Nuovo inserimento
                    objHashTable.Add("INSUP", "I")
                End If

                'carico la hash table
                'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("USER", ConstSession.UserName)
                objHashTable.Add("CODENTE", ConstSession.IdEnte)
                objHashTable.Add("CODTIPOINTERESSE", CODTIPOINTERESSE)
                objHashTable.Add("DAL", ModDate.GiraData(DAL))


                Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                objCOMValoriVoci.DelTassiInteresse(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

                PulisciCampi()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.btnElimina_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PulisciCampi()
        ddlTipoInteresse.SelectedValue = -1
        txtDal.Text = ""
        txtAl.Text = ""
        txtTasso.Text = ""
        txtOperazione.Text = ""
        ddlTributo.SelectedValue = -1
        'CaricaGriglia(True)
        RicaricaPagina()
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
    Private Sub btnPulisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        PulisciCampi()
    End Sub
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigTassiInteressi.PopolaComboTributo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
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
    Private Sub ddlTributo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTributo.SelectedIndexChanged

        PopolaComboTipoInteresse()

    End Sub

End Class
