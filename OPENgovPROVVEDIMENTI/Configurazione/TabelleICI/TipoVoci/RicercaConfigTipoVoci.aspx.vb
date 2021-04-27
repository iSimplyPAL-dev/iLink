Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione del tipo voci.
''' Contiene la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaConfigTipoVoci
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaConfigTipoVoci))
    Dim objDS As DataSet
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim dw As DataView
        Dim objDSTipoVoci As DataSet
        Dim objHashTable As Hashtable = New Hashtable
        Try
            objHashTable.Add("COD_TRIBUTO", Request.Item("COD_TRIBUTO"))
            objHashTable.Add("COD_CAPITOLO", Request.Item("COD_CAPITOLO"))
            objHashTable.Add("COD_PROVVEDIMENTI", Request.Item("COD_PROVVEDIMENTI"))
            objHashTable.Add("COD_VOCE", Request.Item("COD_VOCE"))
            objHashTable.Add("COD_MISURA", Request.Item("COD_MISURA"))
            objHashTable.Add("COD_FASE", Request.Item("COD_FASE"))
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipoVoci = objCOMTipoVoci.GetTipoVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            If Page.IsPostBack = False Then
                If Not objDSTipoVoci Is Nothing Then
                    dw = objDSTipoVoci.Tables(0).DefaultView
                End If

                GrdVoci.DataSource = dw
                GrdVoci.DataBind()
                Session("objDSTipoVoci") = dw
            End If
            Select Case CInt(GrdVoci.Rows.Count)
                'Select Case CInt(objDSTipiInteressi.Tables.Item(0).Rows.Count)
                Case 0
                    GrdVoci.Visible = False

                    lblMessage.Text = "Nessuna Voce trovata"
                    lblMessage.Visible = True
                Case Is > 0
                    GrdVoci.Visible = True
                    lblMessage.Visible = False
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaConfigTipoVoci.gestSearchParametres.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
            '    If Not IsNothing(objSessione) Then
            '        objSessione.Kill()
            '        objSessione = Nothing
            '    End If
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
                For Each myRow As GridViewRow In GrdVoci.Rows
                    If IDRow = CType(myRow.FindControl("hfID_TIPO_VOCE"), HiddenField).Value Then
                        Dim strCodTributo, strCodCapitolo, strCodTipoProvvedimento, strCodMisura, strCodVoce, strCodFase, strVoceAttribuita, strIDTipoVoce As String
                        Dim strPARAMETRI As String

                        strVoceAttribuita = myRow.Cells(4).Text()
                        strCodTributo = CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value
                        strCodCapitolo = CType(myRow.FindControl("hfCOD_CAPITOLO"), HiddenField).Value
                        strCodTipoProvvedimento = CType(myRow.FindControl("hfCOD_TIPO_PROVVEDIMENTO"), HiddenField).Value
                        strCodMisura = CType(myRow.FindControl("hfMISURA"), HiddenField).Value
                        'strCodCalcolato = myrow.Cells(11).Text()
                        strCodFase = CType(myRow.FindControl("hfCOD_FASE"), HiddenField).Value
                        strCodVoce = CType(myRow.FindControl("hfCOD_VOCE"), HiddenField).Value
                        strIDTipoVoce = CType(myRow.FindControl("hfID_TIPO_VOCE"), HiddenField).Value

                        strPARAMETRI = "?CODTRIBUTO=" & strCodTributo
                        strPARAMETRI = strPARAMETRI & "&CODCAPITOLO=" & strCodCapitolo
                        strPARAMETRI = strPARAMETRI & "&CODTIPOPROVVEDIMENTO=" & strCodTipoProvvedimento
                        strPARAMETRI = strPARAMETRI & "&CODMISURA=" & strCodMisura
                        'strPARAMETRI = strPARAMETRI & "&CODCALCOLATO=" & strCodCalcolato
                        strPARAMETRI = strPARAMETRI & "&CODFASE=" & strCodFase
                        strPARAMETRI = strPARAMETRI & "&IDTIPOVOCE=" & strIDTipoVoce
                        strPARAMETRI = strPARAMETRI & "&CODVOCE=" & strCodVoce
                        strPARAMETRI = strPARAMETRI & "&VOCEATTRIBUITA=" & strVoceAttribuita.Replace("&#224;", "a")
                        strPARAMETRI = strPARAMETRI & "&Nuovo=U"
                        RegisterScript("parent.location.href=""NuovoInserimentoTipoVoci.aspx" & strPARAMETRI & """;", Me.GetType)
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaConfigTipoVoci.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
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
    'Private Sub GrdVoci_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdVoci.SortCommand
    '    Dim strSortKey As String
    '    Dim dt As DataView
    'Try
    '    If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
    '        Select Case ViewState("OrderBy").ToString()
    '            Case "ASC"
    '                ViewState("OrderBy") = "DESC"

    '            Case "DESC"
    '                ViewState("OrderBy") = "ASC"
    '        End Select
    '    Else
    '        ViewState("SortKey") = e.SortExpression
    '        ViewState("OrderBy") = "ASC"
    '    End If

    '    dt = objDS.Tables(0).DefaultView
    '    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")

    '    GrdVoci.DataSource = dt
    '    GrdVoci.DataBind()
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaConfigTipoVoci.GrdVoci_SortCommand.errore: ", ex)
    '   Response.Redirect("../../../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdVoci_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdVoci.SelectedIndexChanged
    '    Dim strCodTributo, strCodCapitolo,
    '    strCodTipoProvvedimento, strCodMisura,
    '    strCodCalcolato, strCodVoce, strCodFase, strVoceAttribuita, strIDTipoVoce As String
    '    Dim strPARAMETRI, strscript As String
    '    dim sScript as string=""
    '    Try

    '        strVoceAttribuita = GrdVoci.SelectedItem.Cells(4).Text()
    '        strCodTributo = GrdVoci.SelectedItem.Cells(7).Text()
    '        strCodCapitolo = GrdVoci.SelectedItem.Cells(8).Text()
    '        strCodTipoProvvedimento = GrdVoci.SelectedItem.Cells(9).Text()
    '        strCodMisura = GrdVoci.SelectedItem.Cells(10).Text()
    '        'strCodCalcolato = GrdVoci.SelectedItem.Cells(11).Text()
    '        strCodFase = GrdVoci.SelectedItem.Cells(11).Text()
    '        strCodVoce = GrdVoci.SelectedItem.Cells(12).Text()
    '        strIDTipoVoce = GrdVoci.SelectedItem.Cells(13).Text()


    '        strPARAMETRI = "?CODTRIBUTO=" & strCodTributo
    '        strPARAMETRI = strPARAMETRI & "&CODCAPITOLO=" & strCodCapitolo
    '        strPARAMETRI = strPARAMETRI & "&CODTIPOPROVVEDIMENTO=" & strCodTipoProvvedimento
    '        strPARAMETRI = strPARAMETRI & "&CODMISURA=" & strCodMisura
    '        'strPARAMETRI = strPARAMETRI & "&CODCALCOLATO=" & strCodCalcolato
    '        strPARAMETRI = strPARAMETRI & "&CODFASE=" & strCodFase
    '        strPARAMETRI = strPARAMETRI & "&IDTIPOVOCE=" & strIDTipoVoce
    '        strPARAMETRI = strPARAMETRI & "&CODVOCE=" & strCodVoce
    '        strPARAMETRI = strPARAMETRI & "&VOCEATTRIBUITA=" & strVoceAttribuita

    '        strPARAMETRI = strPARAMETRI & "&Nuovo=U"

    '        
    '        sscript+="parent.location.href=""NuovoInserimentoTipoVoci.aspx" & strPARAMETRI & """;")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '        ' CalPageaspx(strCodTributo, strCodCapitolo, strCodTipoProvvedimento, strCodMisura, strCodCalcolato)
    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaConfigTipoVoci.GrdVoci_SelectedIndexChanged.errore: ", ex)
    '   Response.Redirect("../../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
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
            GrdVoci.DataSource = CType(Session("objDSTipoVoci"), DataView)
            If page.HasValue Then
                GrdVoci.PageIndex = page.Value
            End If
            GrdVoci.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaConfigTipoVoci.LoadSearch.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strCodTributo"></param>
    ''' <param name="strCodCapitolo"></param>
    ''' <param name="strCodTipoProvvedimento"></param>
    ''' <param name="strCodMisura"></param>
    ''' <param name="strCodCalcolato"></param>
    Private Sub CalPageaspx(ByVal strCodTributo As String,
                          ByVal strCodCapitolo As String,
                          ByVal strCodTipoProvvedimento As String,
                          ByVal strCodMisura As String,
                          ByVal strCodCalcolato As String)
        Dim strPARAMETRI As String
        Dim sScript As String = ""
        Try
            strPARAMETRI = "?CODTRIBUTO=" & strCodTributo
            strPARAMETRI = strPARAMETRI & "&CODCAPITOLO=" & strCodCapitolo
            strPARAMETRI = strPARAMETRI & "&CODTIPOPROVVEDIMENTO=" & strCodTipoProvvedimento
            strPARAMETRI = strPARAMETRI & "&CODMISURA=" & strCodMisura
            strPARAMETRI = strPARAMETRI & "&CODCALCOLATO=" & strCodCalcolato

            sScript += "parent.location.href='NuovoInserimentoTipoVoci.aspx" & strPARAMETRI & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaConfigTipoVoci.CallPageaspx.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
