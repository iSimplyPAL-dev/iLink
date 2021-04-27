Imports System
Imports System.Web.SessionState
Imports log4net

Namespace Provvedimenti.usercontrol
    ''' <summary>
    ''' UserControl per la gestione rate
    ''' </summary>
    Partial Class WUCRate
        
        Inherits System.Web.UI.UserControl

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
        Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(WUCRate))
        Private lcl_id_accorpamento As Integer
        Private lcl_id_provvedimento As Integer
        Private tipo As String
        Private bInserisciPagamento As Boolean
        ''' <summary>
        ''' posizionamento delle colonne della griglia
        ''' </summary>
        Enum GridPosition
            '
            n_rata = 0
            data_scadenza = 1
            valore_rata = 2
            valore_interesse = 3
            importo_totale_rata = 4
            importo_pagato = 5
            data_accredito = 6
            data_pagamento = 7
            Provenienza = 8
            btnDelete = 9
            id_accorpamento = 10
            id_provvedimento = 11
            id_rata_acc = 12
            id_rata_provv = 13
            id_provenienza = 14
            id_pagato = 15
            tipo = 16
        End Enum

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'getRateAccorpamento()
        End Sub

#Region "Griglie"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
            Try
                If (e.Row.RowType = DataControlRowType.DataRow) Then
                    Dim objlbl, objlblRata As Label
                    Dim myBtn As ImageButton
                    Dim id_rata_acc, id_rata_provv, NumRata As String
                    objlbl = e.Row.Cells(GridPosition.importo_pagato).FindControl("lblImportoPagato")
                    objlblRata = e.Row.Cells(GridPosition.n_rata).FindControl("lblRata")
                    If objlblRata.Text <> "" Then
                        NumRata = objlblRata.Text
                    Else
                        NumRata = 0
                    End If

                    id_rata_acc = CType(e.Row.FindControl("hfid_rata_acc"), HiddenField).Value
                    id_rata_provv = CType(e.Row.FindControl("hfid_rata_provv"), HiddenField).Value
                    If objlbl.Text <> "" Then
                        'rata pagata
                        'txtRatePagate.Text = txtRatePagate.Text & id_rata_acc & "," & e.row.Cells(GridPosition.n_rata).Text & ",P#"
                        txtRatePagate.Text = txtRatePagate.Text & id_rata_acc & "," & id_rata_provv & "," & NumRata & ",P#"
                    Else
                        txtRatePagate.Text = txtRatePagate.Text & id_rata_acc & "," & id_rata_provv & "," & NumRata & ",#"
                    End If
                    'Bottone cancella rata
                    myBtn = e.Row.FindControl("imgDelete")
                    myBtn.ToolTip = "Premere questo Bottone per eliminare la rata"
                    'If CType(e.Row.FindControl("hfid_pagato"), HiddenField).Value = "0" Then
                    '    objbtn.Visible = False
                    'Else
                    myBtn.Visible = True
                    'End If
                    'If bInserisciPagamento Then
                    '    e.Row.Attributes.Add("title", "Inserisci/Cancella Pagamento")
                    '    e.Row.Attributes.Add("onclick", "InserisciNuovoPagamento();")
                    'End If
                    If CInt(CType(e.Row.FindControl("hfid_pagato"), HiddenField).Value) <= 0 Then
                        myBtn = CType(e.Row.FindControl("imgOpenRata"), ImageButton)
                        myBtn.CssClass = "BottoneGrd BottoneNewInsertGrd"
                        myBtn.ToolTip = "Nuovo"
                    Else
                        myBtn = CType(e.Row.FindControl("imgOpenRata"), ImageButton)
                        myBtn.CssClass = "BottoneGrd BottoneApriGrd"
                        myBtn.ToolTip = "Modifica"
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.GrdRowDataBound.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
            Try
                Dim IDRow As String = e.CommandArgument.ToString()
                If e.CommandName = "RowDelete" Then
                    For Each myRow As GridViewRow In GrdRate.Rows
                        If IDRow = CType(myRow.FindControl("hfid_rata_acc"), HiddenField).Value Then
                            Dim id_pagato As Integer
                            Dim id_rata_provv As Integer
                            Dim sTipo As String
                            Dim objPagamenti As clsPagamenti
                            Try
                                objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(HttpContext.Current.Session("PARAMETROENV"), ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
                                id_pagato = CType(myRow.FindControl("hfid_pagato"), HiddenField).Value
                                sTipo = CType(myRow.FindControl("hftipo"), HiddenField).Value
                                If sTipo = "P" Then
                                    id_rata_provv = CType(myRow.FindControl("hfid_rata_provv"), HiddenField).Value
                                End If
                                'M.B. inserito = 0 xke gestito come int ma gli arriva una stringa vuota
                                If IDRow = "" Then
                                    IDRow = "0"
                                End If
                                objPagamenti.deleteRata(id_pagato, IDRow, id_rata_provv)
                                'Log.Debug("L'utente " & ConstSession.UserName & " ha cancellato la rata '" & myRow.Cells(GridPosition.n_rata).Text & "' relativa all'accorpamento " & myRow.Cells(GridPosition.id_accorpamento).Text & ".")
                                lcl_id_accorpamento = CType(myRow.FindControl("hfid_accorpamento"), HiddenField).Value
                                'Ricarico la griglia dei pagamenti
                                getRateAccorpamento(ConstSession.IdEnte)
                                'visualizzo i campi del pagamento
                                RegisterScript("document.getElementById ('fldPagamento').className ='';", Me.GetType())
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.GrdRowCommand.errore: ", ex)
                                Response.Redirect("../../PaginaErrore.aspx")
                                Throw New Exception(ex.Message, ex)
                            Finally
                                objPagamenti.kill()
                            End Try
                        End If
                    Next
                ElseIf e.CommandName = "RowAdd" Then
                    Dim sScript As String
                    For Each myRow As GridViewRow In GrdRate.Rows
                        If CType(myRow.FindControl("lblRata"), Label).Text = "" Then
                            CType(myRow.FindControl("lblRata"), Label).Text = "0"
                        End If
                        If IDRow = CType(myRow.FindControl("lblRata"), Label).Text Then
                            sScript = "parent.Comandi.location.href='cmdPagamenti.aspx?from=Dettaglio';"
                            sScript += "parent.Visualizza.location.href='Pagamenti.aspx?from=Dettaglio&NRataSel=" & IDRow & "&IdPagamento=" & CType(myRow.FindControl("hfid_pagato"), HiddenField).Value & "&IdAccorpamento=" & CType(myRow.FindControl("hfid_accorpamento"), HiddenField).Value & "&IdProvvedimento=" & CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value & "&Tipo=" & CType(myRow.FindControl("hftipo"), HiddenField).Value & "&IdContribuente=" & IdContribuente & "';"
                            RegisterScript(sScript, Me.GetType())
                            'RegisterScript("InserisciNuovoPagamento();", Me.GetType())
                        End If
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.GrdRowCommand.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
        End Sub
        'Private Sub grdRate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdRate.ItemDataBound
        '    Dim objlbl, objlblRata As Label
        '    Dim objbtn As ImageButton
        '    Dim id_rata_acc, id_rata_provv, nRata As String
        '    Try
        '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
        '            objlbl = e.Item.Cells(GridPosition.importo_pagato).FindControl("lblImportoPagato")
        '            objlblRata = e.Item.Cells(GridPosition.n_rata).FindControl("lblRata")
        '            If objlblRata.Text <> "" Then
        '                nRata = objlblRata.Text
        '            Else
        '                nRata = 0
        '            End If

        '            id_rata_acc = e.Item.Cells(GridPosition.id_rata_acc).Text
        '            id_rata_provv = e.Item.Cells(GridPosition.id_rata_provv).Text
        '            If objlbl.Text <> "" Then
        '                'rata pagata
        '                'txtRatePagate.Text = txtRatePagate.Text & id_rata_acc & "," & e.Item.Cells(GridPosition.n_rata).Text & ",P#"
        '                txtRatePagate.Text = txtRatePagate.Text & id_rata_acc & "," & id_rata_provv & "," & nRata & ",P#"
        '            Else
        '                txtRatePagate.Text = txtRatePagate.Text & id_rata_acc & "," & id_rata_provv & "," & nRata & ",#"
        '            End If
        '            'Bottone cancella rata
        '            objbtn = e.Item.Cells(GridPosition.btnDelete).FindControl("imgDelete")
        '            objbtn.ToolTip = "Premere questo Bottone per eliminare la rata"
        '            If e.Item.Cells(GridPosition.id_pagato).Text = "0" Then
        '                objbtn.Visible = False
        '            Else
        '                objbtn.Visible = True
        '            End If
        '            If bInserisciPagamento Then
        '                e.Item.Attributes.Add("title", "Inserisci/Cancella Pagamento")
        '                e.Item.Attributes.Add("onclick", "InserisciNuovoPagamento();")
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.grdRate_ItemDataBound.errore: ", ex)
        '        Response.Redirect("../../PaginaErrore.aspx")
        '        Throw New Exception(ex.Message, ex)
        '    End Try

        'End Sub
        'Private Sub grdRate_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdRate.DeleteCommand
        '    Dim id_pagato As Integer
        '    Dim id_rata_acc, id_rata_provv As Integer
        '    Dim sString, sTipo As String
        '    Dim objPagamenti As clsPagamenti
        '    Try
        '        objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(HttpContext.Current.Session("PARAMETROENV"), ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
        '        id_pagato = e.Item.Cells(GridPosition.id_pagato).Text
        '        sTipo = e.Item.Cells(GridPosition.tipo).Text
        '        If sTipo = "P" Then
        '            id_rata_acc = e.Item.Cells(GridPosition.id_rata_acc).Text
        '            id_rata_provv = e.Item.Cells(GridPosition.id_rata_provv).Text
        '        End If

        '        objPagamenti.deleteRata(id_pagato, id_rata_acc, id_rata_provv)
        '        Log.Debug("L'utente " & ConstSession.UserName & " ha cancellato la rata '" & e.Item.Cells(GridPosition.n_rata).Text & "' relativa all'accorpamento " & e.Item.Cells(GridPosition.id_accorpamento).Text & ".")
        '        lcl_id_accorpamento = e.Item.Cells(GridPosition.id_accorpamento).Text
        '        'Ricarico la griglia dei pagamenti
        '        getRateAccorpamento(ConstSession.IdEnte)
        '        'visualizzo i campi del pagamento
        '        RegisterScript("aaa", "<script>document.getElementById ('fldPagamento').className ='';</script>")
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.grdRate_DeleteCommand.errore: ", ex)
        '        Response.Redirect("../../PaginaErrore.aspx")
        '        Throw New Exception(ex.Message, ex)
        '    Finally
        '        objPagamenti.kill()
        '    End Try
        'End Sub
#End Region
        '
        ''' <summary>
        ''' Reperimento rate provvedimento
        ''' </summary>
        ''' <param name="sIdEnte"></param>
        ''' <returns></returns>
        Public Function getRateProvvedimento(ByVal sIdEnte As String) As Integer
            Dim objPagamenti As clsPagamenti
            Dim objDS As DataSet
            Dim iNumRate As Integer = 0
            Try

                objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(HttpContext.Current.Session("PARAMETROENV"), ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
                objDS = objPagamenti.getRateProvvedimento(lcl_id_provvedimento, sIdEnte, False)
                If Not IsNothing(objDS) Then
                    If objDS.Tables(0).Rows.Count > 0 Then
                        GrdRate.SelectedIndex = -1
                        GrdRate.DataSource = objDS.Tables(0).DefaultView
                        GrdRate.DataBind()
                        lblInfoRate.Visible = False
                        GrdRate.Visible = True
                        iNumRate = objDS.Tables(0).Rows.Count
                    Else
                        lblInfoRate.Text = "Nessuna Rata trovata"
                        lblInfoRate.Visible = True
                        GrdRate.Visible = False
                    End If
                Else
                    lblInfoRate.Text = "Nessuna Rata trovata"
                    lblInfoRate.Visible = True
                    GrdRate.Visible = False
                End If
                Return iNumRate
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.getRateProvvedimento.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
                Throw New Exception(ex.Message, ex)
            Finally
                objPagamenti.kill()
                objPagamenti = Nothing
                objDS.Dispose()
            End Try
        End Function
        '
        ''' <summary>
        ''' Reperimento rate Accorpamento
        ''' </summary>
        ''' <param name="sIdEnte"></param>
        ''' <returns></returns>
        Public Function getRateAccorpamento(ByVal sIdEnte As String) As Integer
            Dim objPagamenti As clsPagamenti
            Dim objDS As DataSet
            Dim iNumRate As Integer = 0
            Try
                txtRatePagate.Text = ""
                objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(HttpContext.Current.Session("PARAMETROENV"), ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
                objDS = objPagamenti.getRateAccorpamento(lcl_id_accorpamento, sIdEnte)
                If Not IsNothing(objDS) Then
                    If objDS.Tables(0).Rows.Count > 0 Then
                        GrdRate.SelectedIndex = -1
                        GrdRate.DataSource = objDS.Tables(0).DefaultView
                        GrdRate.DataBind()
                        lblInfoRate.Visible = False
                        GrdRate.Visible = True
                        iNumRate = objDS.Tables(0).Rows.Count
                    Else
                        lblInfoRate.Text = "Nessuna Rata trovata"
                        lblInfoRate.Visible = True
                        GrdRate.Visible = False
                    End If
                Else
                    lblInfoRate.Text = "Nessuna Rata trovata"
                    lblInfoRate.Visible = True
                    GrdRate.Visible = False
                End If
                Return iNumRate
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.getRateAccorpamento.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
                Throw New Exception(ex.Message, ex)
            Finally
                objPagamenti.kill()
                objPagamenti = Nothing
                objDS.Dispose()
            End Try
        End Function
        'proprietà
        Public Property IdContribuente() As String
            Get
                Return hdIdContribuente.Value
            End Get
            Set(ByVal Value As String)
                hdIdContribuente.Value = Value
            End Set
        End Property
        Public Property id_accorpamento() As Integer
            Get
                Return lcl_id_accorpamento
            End Get
            Set(ByVal Value As Integer)
                lcl_id_accorpamento = Value
            End Set
        End Property
        Public Property id_provvedimento() As Integer
            Get
                Return lcl_id_provvedimento
            End Get
            Set(ByVal Value As Integer)
                lcl_id_provvedimento = Value
            End Set
        End Property
        Public Property txtlegend() As String
            Get
                Return lbbLegend.InnerText
            End Get
            Set(ByVal Value As String)
                lbbLegend.InnerText = Value
            End Set
        End Property
        Public Property sRatePagate() As String
            Get
                Return txtRatePagate.Text
            End Get
            Set(ByVal Value As String)
                txtRatePagate.Text = Value
            End Set
        End Property
        Public WriteOnly Property sTipo() As String
            Set(ByVal Value As String)
                tipo = Value
            End Set
        End Property

        Public ReadOnly Property ImportoTotaleRata(ByVal n_rata As Integer) As String
            Get
                Try
                    If n_rata > 0 Then
                        Dim objlbl As Label
                        Dim myVal As Double = 0
                        Double.TryParse(CType(GrdRate.Rows(n_rata - 1).FindControl("lblImportoPagato"), Label).Text, myVal)
                        If myVal > 0 Then
                            Return myVal.ToString
                        Else
                            objlbl = GrdRate.Rows(n_rata - 1).FindControl("lblImportoTotale")
                            Return objlbl.Text
                        End If
                    Else
                        Return ""
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.ImportoTotaleRata.errore: ", ex)
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property DataPagamentoRata(ByVal n_rata As Integer) As String
            Get
                Dim myData As String = New MyUtility().GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
                Try
                    If n_rata > 0 Then
                        Dim objlbl As Label
                        objlbl = GrdRate.Rows(n_rata - 1).FindControl("lblDataPag")
                        If objlbl.Text <> "" Then
                            myData = objlbl.Text
                        End If
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.DataPagamentoRata.errore: ", ex)
                End Try
                Return myData
            End Get
        End Property
        Public ReadOnly Property DataAccreditoRata(ByVal n_rata As Integer) As String
            Get
                Dim myData As String = New MyUtility().GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
                Try
                    If n_rata > 0 Then
                        Dim objlbl As Label
                        objlbl = GrdRate.Rows(n_rata - 1).FindControl("lblDataAcc")
                        If objlbl.Text <> "" Then
                            myData = objlbl.Text
                        End If
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.DataAccreditoRata.errore: ", ex)
                End Try
                Return myData
            End Get
        End Property
        Public ReadOnly Property IdRata(ByVal n_rata As Integer) As Integer
            Get
                Dim myVal As Integer = -1
                Try
                    If n_rata > 0 Then
                        myVal = CInt(CType(GrdRate.Rows(n_rata - 1).FindControl("hfid_rata_acc"), HiddenField).Value)
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.IdRata.errore: ", ex)
                End Try
                Return myVal
            End Get
        End Property
        Public ReadOnly Property ProvenienzaRata(ByVal n_rata As Integer) As String
            Get
                Dim myString As String = "-1"
                Try
                    If n_rata > 0 Then
                        myString = CType(GrdRate.Rows(n_rata - 1).FindControl("hfPROVENIENZA"), HiddenField).Value
                    End If
                    If myString = "" Then
                        myString = "-1"
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.ProvenienzaRata.errore: ", ex)
                End Try
                Return myString
            End Get
        End Property

        Public WriteOnly Property viewDelete() As Boolean
            Set(ByVal Value As Boolean)
                grdRate.Columns(GridPosition.btnDelete).Visible = Value
            End Set
        End Property

        Public WriteOnly Property InserisciPagamento() As Boolean
            Set(ByVal Value As Boolean)
                bInserisciPagamento = Value
            End Set
        End Property

        '
        ''' <summary>
        ''' Funzioni per formattazione griglia
        ''' </summary>
        ''' <param name="objtemp"></param>
        ''' <returns></returns>
        Protected Function annoBarra(ByVal objtemp As Object) As String
            Dim clsGeneralFunction As New MyUtility
            Dim strTemp As String = ""
            Try
                If Not IsDBNull(objtemp) Then
                    If CStr(objtemp).CompareTo("") <> 0 And CStr(objtemp).CompareTo("99991231") <> 0 Then
                        strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
                    Else
                        strTemp = ""
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.annoBarra.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
            Return strTemp
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="NumeroDaFormattareParam"></param>
        ''' <param name="numDec"></param>
        ''' <returns></returns>
        Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As Object, ByVal numDec As Integer) As String
            FormattaNumero = ""
            Try
                If IsDBNull(NumeroDaFormattareParam) Then
                    NumeroDaFormattareParam = ""
                ElseIf NumeroDaFormattareParam.ToString() = "" Or NumeroDaFormattareParam.ToString() = "-1" Or NumeroDaFormattareParam.ToString() = "-1,00" Then
                    NumeroDaFormattareParam = ""
                Else
                    FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.FormattaNumero.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
                Throw New Exception(ex.Message, ex)
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="rata"></param>
        ''' <returns></returns>
        Protected Function nRata(ByVal rata As Object) As String
            Try
                If IsDBNull(rata) Then
                    nRata = ""
                ElseIf rata.ToString() = "0" Then
                    nRata = ""
                Else
                    nRata = rata
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WUCRate.nRata.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
                Throw New Exception(ex.Message, ex)
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="script"></param>
        ''' <param name="type"></param>
        Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
            ConstSession.CountScript = (ConstSession.CountScript + 1)
            Try
                Dim uniqueId As String = ("spc_" _
                    + (ConstSession.CountScript.ToString _
                    + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
                Dim sScript As String = "<script language='javascript'>"
                sScript += script
                sScript += "</script>"
                Page.RegisterStartupScript(uniqueId, sScript)
            Catch ex As Exception
                Log.Debug(" - OPENgovPROVVEDIMENTI.BaseRegisterScript.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace