Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione delle voci.
''' Contiene la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class VisualizzaValoriVoci
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(VisualizzaValoriVoci))

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
        Dim dw As DataView
        Dim objDSTipoVoci As DataSet
        Dim sScript As String = ""
        Dim objHashTable As Hashtable = New Hashtable
        Dim CODTRIBUTO, CODCAPITOLO, CODVOCE, ViewAlert As String

        Try
            CODTRIBUTO = Request.Item("CODTRIBUTO")
            CODCAPITOLO = Request.Item("CODCAPITOLO")
            CODVOCE = Request.Item("CODVOCE")
            ViewAlert = Request.Item("ViewAlert")

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CODTRIBUTO", CODTRIBUTO)
            objHashTable.Add("CODCAPITOLO", CODCAPITOLO)
            objHashTable.Add("CODVOCE", CODVOCE)

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipoVoci = objCOMTipoVoci.GetValoriVoci(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

            If Page.IsPostBack = False Then
                If Not objDSTipoVoci Is Nothing Then
                    dw = objDSTipoVoci.Tables(0).DefaultView
                End If

                GrdVoci.DataSource = dw
                GrdVoci.DataBind()
                Session("objDSTipoVoci") = dw
                If objDSTipoVoci.Tables(0).Rows.Count <> 0 And ViewAlert = "" Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Voce già presente a sistema');"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                dw = objDSTipoVoci.Tables(0).DefaultView
                'dw.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")
                GrdVoci.DataSource = dw
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.VisualizzaValoriVoci.Page_Load.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
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
                For Each myRow As GridViewRow In GrdVoci.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        Dim strAnno, strValore,
        strMinimo, strRiducibile,
        strCumulabile, strCalcolataSu, strCondizione,
        strParametro, strBaseRaffronto As String
                        Dim chkRid, chkCum As String
                        Dim sScript As String = ""

                        strAnno = myRow.Cells(0).Text()
                        strValore = Trim(myRow.Cells(1).Text())
                        strMinimo = Trim(myRow.Cells(2).Text())
                        strRiducibile = Trim(myRow.Cells(3).Text())
                        strCumulabile = Trim(myRow.Cells(4).Text())
                        strCondizione = Trim(myRow.Cells(9).Text())
                        strCalcolataSu = Trim(myRow.Cells(11).Text())
                        strParametro = Trim(myRow.Cells(12).Text())
                        strBaseRaffronto = Trim(myRow.Cells(13).Text())

                        strAnno = replaceSpace(strAnno)
                        strValore = replaceSpace(strValore)
                        strMinimo = replaceSpace(strMinimo)
                        strCondizione = replaceSpace(strCondizione)
                        strCalcolataSu = replaceSpace(strCalcolataSu)
                        strParametro = replaceSpace(strParametro)
                        strBaseRaffronto = replaceSpace(strBaseRaffronto)
                        If strRiducibile = "1" Then
                            chkRid = "true"
                        Else
                            chkRid = "false"
                        End If
                        If strCumulabile = "1" Then
                            chkCum = "true"
                        Else
                            chkCum = "false"
                        End If
                        sScript += "parent.document.getElementById('txtanno').value='" & strAnno & "';"
                        sScript += "parent.document.getElementById('txtvalore').value='" & strValore & "';"
                        sScript += "parent.document.getElementById('txtMinimo').value='" & strMinimo & "';"
                        sScript += "parent.document.getElementById('chkRiducibile').checked=" & chkRid & ";"
                        sScript += "parent.document.getElementById('chkCumulabile').checked=" & chkCum & ";"
                        sScript += "parent.document.getElementById('txtCondizione').value='" & strCondizione & "';"
                        If strCalcolataSu.CompareTo("") <> 0 Then
                            sScript += "parent.document.getElementById('ddlCalcolata.options['" & strCalcolataSu & "']').selected=true;"
                        End If
                        If strParametro.CompareTo("") <> 0 Then
                            sScript += "parent.document.getElementById('ddlParametro.options['" & strParametro & "']').selected=true;"
                        End If
                        If strBaseRaffronto.CompareTo("") <> 0 Then
                            sScript += "parent.document.getElementById('ddlBaseRaffronto.options['" & strBaseRaffronto & "']').selected=true;"
                        End If
                        sScript += "parent.AbilitaPulsanti();"
                        sScript += "parent.document.getElementById('txtInsUp').value='U';"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.VisualizzaValoriVoci.GrdRowCommand.errore: ", ex)
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
    'Private Sub GrdVoci_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdVoci.SelectedIndexChanged
    '    Dim strAnno, strValore,
    '    strMinimo, strRiducibile,
    '    strCumulabile, strCalcolataSu, strCondizione,
    '    strParametro, strBaseRaffronto As String
    '    Dim chkRid, chkCum As String
    '    Dim strPARAMETRI, strscript As String
    '    dim sScript as string=""
    '    Try

    '        strAnno = GrdVoci.SelectedItem.Cells(0).Text()
    '        strValore = Trim(GrdVoci.SelectedItem.Cells(1).Text())
    '        strMinimo = Trim(GrdVoci.SelectedItem.Cells(2).Text())
    '        strRiducibile = Trim(GrdVoci.SelectedItem.Cells(3).Text())
    '        strCumulabile = Trim(GrdVoci.SelectedItem.Cells(4).Text())

    '        strCondizione = Trim(GrdVoci.SelectedItem.Cells(9).Text())
    '        strCalcolataSu = Trim(GrdVoci.SelectedItem.Cells(11).Text())
    '        strParametro = Trim(GrdVoci.SelectedItem.Cells(12).Text())
    '        strBaseRaffronto = Trim(GrdVoci.SelectedItem.Cells(13).Text())

    '        strAnno = replaceSpace(strAnno)
    '        strValore = replaceSpace(strValore)
    '        strMinimo = replaceSpace(strMinimo)

    '        strCondizione = replaceSpace(strCondizione)
    '        strCalcolataSu = replaceSpace(strCalcolataSu)
    '        strParametro = replaceSpace(strParametro)
    '        strBaseRaffronto = replaceSpace(strBaseRaffronto)

    '        If strRiducibile = "1" Then
    '            chkRid = "true"
    '        Else
    '            chkRid = "false"
    '        End If
    '        If strCumulabile = "1" Then
    '            chkCum = "true"
    '        Else
    '            chkCum = "false"
    '        End If


    '        sscript+="<script language='javascript'>" & vbCrLf)
    '        sscript+="parent.formVoci.txtanno.value='" & strAnno & "';" & vbCrLf)
    '        sscript+="parent.formVoci.txtvalore.value='" & strValore & "';" & vbCrLf)
    '        sscript+="parent.formVoci.txtMinimo.value='" & strMinimo & "';" & vbCrLf)
    '        sscript+="parent.formVoci.chkRiducibile.checked=" & chkRid & ";" & vbCrLf)
    '        sscript+="parent.formVoci.chkCumulabile.checked=" & chkCum & ";" & vbCrLf)

    '        sscript+="parent.formVoci.txtCondizione.value='" & strCondizione & "';" & vbCrLf)

    '        If strCalcolataSu.CompareTo("") <> 0 Then
    '            sscript+="parent.formVoci.ddlCalcolata.options['" & strCalcolataSu & "'].selected=true;" & vbCrLf)
    '        End If
    '        If strParametro.CompareTo("") <> 0 Then
    '            sscript+="parent.formVoci.ddlParametro.options['" & strParametro & "'].selected=true;" & vbCrLf)
    '        End If
    '        If strBaseRaffronto.CompareTo("") <> 0 Then
    '            sscript+="parent.formVoci.ddlBaseRaffronto.options['" & strBaseRaffronto & "'].selected=true;" & vbCrLf)
    '        End If
    '        sscript+="parent.AbilitaPulsanti();" & vbCrLf)

    '        sscript+="parent.formVoci.txtInsUp.value='U';" & vbCrLf)

    '        

    '        RegisterScript(sScript , Me.GetType())

    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.VisualizzaValoriVoci.GrdVoci_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../../Styles.css"))
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.VisualizzaValoriVoci.LoadSearch.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
            Throw ex
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.VisualizzaValoriVoci.SetValoreSINO.errore: ", ex)
            Response.Redirect("../../../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="valore"></param>
    ''' <returns></returns>
    Function replaceSpace(ByVal valore As String) As String
        Return valore.Replace("&nbsp;", "")
    End Function
End Class
