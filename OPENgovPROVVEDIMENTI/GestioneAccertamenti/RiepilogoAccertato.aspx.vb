Imports log4net
Imports ComPlusInterface
''' <summary>
''' Pagina per la visualizzazione del provvedimento generato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RiepilogoAccertato
    Inherits BaseEnte
    Protected FncGrd As New Formatta.FunctionGrd
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(RiepilogoAccertato))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblLegame As System.Web.UI.WebControls.Label
    Protected WithEvents txtLegame As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'Private idCelle As New DataGridIndex

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public id_provvedimento As Integer
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim ListDic() As objUIICIAccert
        Dim ListAcc() As objUIICIAccert

        Try
            Session.Remove("DataSetSanzioni")

            id_provvedimento = Request.Item("id_provvedimento")
            Log.Debug("devo caricare ui accertate")
            ListAcc = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
            GrdACC.DataSource = ListAcc
            GrdACC.DataBind()
            Log.Debug("devo caricare ui dichiarate")
            If Not Session("DataSetDichiarazioni") Is Nothing Then
                ListDic = CType(Session("DataSetDichiarazioni"), objUIICIAccert())
                GrdDICH.DataSource = ListDic
                GrdDICH.DataBind()
                lblDich.Visible = False
                lblDich.Text = ""
            Else
                lblDich.Visible = True
                lblDich.Text = "Nessun Immobile Dichiarato"
            End If
            Log.Debug("carico anagrafica")
            If ConstSession.HasPlainAnag Then
                ifrmAnagRiepilogo.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + Request.Item("IdContribuente") + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString())
            End If
            Log.Debug("carico riepilogo")
            PopolaRiepilogo() '(objhashtableRIEPILOGOAccDic)
            Log.Debug("carico anno e tipo avviso")
            Dim intRETTIFICATO As Integer = Request.Item("RETTIFICATO")
            Dim sScript As String = ""
            If intRETTIFICATO = 0 Then
                sScript += "parent.Comandi.location.href='ComandiRiepilogoAccertamento.aspx?Tributo=" + Utility.StringOperation.FormatString(Request.Item("tributo")) + "';"
            ElseIf intRETTIFICATO = 1 Then
                sScript += "parent.Comandi.location.href='ComandiRiepilogoAccertamentoRettifica.aspx?Tributo=" + Utility.StringOperation.FormatString(Request.Item("tributo")) + "';"
            End If
            RegisterScript(sScript, Me.GetType())
            Log.Debug("anno")
            lblAnnoAccertamento.Text = Request.Item("anno")
            Log.Debug("tipoavviso")
            lblTipoAvviso.Text = CStr(Session("TipoAvviso")).ToUpper().ToString()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdBack_Click(sender As Object, e As EventArgs) Handles CmdBack.Click
        Dim sScript As String = ""
        Session.Remove("DataTableImmobiliDaAccertare")
        sScript = "location.href='"
        If Utility.StringOperation.FormatString(Request.Item("Tributo")) = Utility.Costanti.TRIBUTO_TASI Then
            sScript += "../GestioneAccertamentiTASI/GestioneAccertamenti"
        Else
            sScript += "GestioneAccertamenti"
        End If
        sScript += ".aspx'"
        RegisterScript(sScript, Me.GetType())
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
                e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                e.Row.Cells(0).Font.Bold = True
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdDICH_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDICH.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Cells(0).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(0).Font.Bold = True
    '    End If
    '  Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.GrdDICH_ItemDataBound.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub


    'Protected Sub GrdACC_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdACC.ItemDataBound
    '    Dim objUtility As New MyUtility
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni As CheckBox
    '        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '        Dim check As CheckBox
    '        Dim arraySanzioni() As String

    '        e.Item.Cells(0).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(0).Font.Bold = True

    '        chkSanzioni = e.Item.Cells(23).FindControl("chkSanzioni")
    '        idSanzioni = e.Item.Cells(12).Text
    '        If idSanzioni <> "-1" Then
    '            chkSanzioni.Checked = True
    '            arraySanzioni = Split(idSanzioni, "#")
    '            idSanzioniPar = arraySanzioni(0)

    '            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '            Dim objDS As DataSet
    '            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
    '            DescSanzione = ""
    '            If Not IsNothing(objDS) Then
    '                If objDS.Tables.Count > 0 Then
    '                    If objDS.Tables(0).Rows.Count > 0 Then
    '                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                    End If
    '                End If
    '            End If
    '            DescSanzione = DescSanzione.Replace("'", "\'")
    '            objDS.Dispose()
    '            objGestOPENgovProvvedimenti = Nothing
    '            Motivazione = e.Item.Cells(27).Text
    '            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
    '            'e.Item.Cells(23).Attributes.Add("onClick", "ApriDettaglioSanzioni('" & e.Item.Cells(0).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1'," & id_provvedimento & ")")
    '            'e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).Attributes.Add("onClick", "ApriDettaglioSanzioni('" & e.Item.Cells(0).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1')")
    '        Else
    '            chkSanzioni.Checked = False
    '            chkSanzioni.Enabled = False
    '        End If

    '        If e.Item.Cells(idCelle.grdSanzioniSanzioni.cellICICalcolato).Text < 0 Then
    '            e.Item.Cells(idCelle.grdSanzioniSanzioni.cellICICalcolato).Text = FormatNumber(0, 2)
    '        Else
    '            e.Item.Cells(idCelle.grdSanzioniSanzioni.cellICICalcolato).Text = FormatNumber(e.Item.Cells(idCelle.grdSanzioniSanzioni.cellICICalcolato).Text, 2)
    '        End If
    '        'DIPE 03/08/2005
    '        'Aggiunta somma importo 
    '        'e.Item.Cells(24).Text = "€ " & FormatNumber(objUtility.cToDbl(e.Item.Cells(14).Text) + CStr(objUtility.cToDbl(e.Item.Cells(15).Text) + objUtility.cToDbl(e.Item.Cells(16).Text)), 2)
    '        'e.Item.Cells(25).Text = "€ " & FormatNumber(objUtility.cToDbl(e.Item.Cells(15).Text) + CStr(objUtility.cToDbl(e.Item.Cells(16).Text) + objUtility.cToDbl(e.Item.Cells(17).Text)), 2)

    '        'Formattazione numeri
    '        e.Item.Cells(10).Text = FormatNumber(objUtility.cToDbl(e.Item.Cells(10).Text), 2)
    '        e.Item.Cells(14).Text = FormatNumber(objUtility.cToDbl(e.Item.Cells(14).Text), 2)
    '        e.Item.Cells(15).Text = FormatNumber(objUtility.cToDbl(e.Item.Cells(15).Text), 2)
    '        e.Item.Cells(16).Text = FormatNumber(objUtility.cToDbl(e.Item.Cells(16).Text), 2)
    '        'e.Item.Cells(25).Text = FormatNumber(objUtility.cToDbl(e.Item.Cells(25).Text), 2)
    '    End If
    '  Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.GrdACC_ItemDataBound.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaRiepilogo() '(ByVal objhashtableRIEPILOGOAccDic As Hashtable)
        Try
            If Not Session("HTRIEPILOGO") Is Nothing Then
                Dim objhashtableRIEPILOGO As New Hashtable

                objhashtableRIEPILOGO = CType(Session("HTRIEPILOGO"), Hashtable)
                'Convert.ToDecimal(iInput).ToString("N")

                lblDIF2.Text = Convert.ToDecimal(objhashtableRIEPILOGO("DIFASE2")).ToString("N")
                lblSANZF2.Text = Convert.ToDecimal(objhashtableRIEPILOGO("SANZFASE2")).ToString("N")
                'lblSANZRIDOTTOF2.Text = Convert.ToDecimal(objhashtableRIEPILOGO("SANZRIDOTTOFASE2")).ToString("N")
                lblINTF2.Text = Convert.ToDecimal(objhashtableRIEPILOGO("INTFASE2")).ToString("N")
                lblTOTF2.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TOTFASE2")).ToString("N")

                lblDIAVVISO.Text = Convert.ToDecimal(objhashtableRIEPILOGO("DIAVVISO")).ToString("N")
                lblSANZAVVISO.Text = Convert.ToDecimal(objhashtableRIEPILOGO("SANZAVVISO")).ToString("N")
                lblSANZRIDOTTOAVVISO.Text = Convert.ToDecimal(objhashtableRIEPILOGO("SANZRIDOTTOAVVISO")).ToString("N")
                lblINTAVVISO.Text = Convert.ToDecimal(objhashtableRIEPILOGO("INTAVVISO")).ToString("N")
                lblTOTALEAVVISO.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TOTAVVISO")).ToString("N")

                lblTotDich1.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotImpICIDichiarato")).ToString("N")
                lblTotDich2.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotImpICIDichiarato")).ToString("N")
                lblTotVers.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotVersamenti")).ToString("N")

                lbltotImpICI.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotImpICIACCERTAMENTO")).ToString("N")
                lbltotDiffImp.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotDiffImpostaACCERTAMENTO")).ToString("N")
                lbltotImpSanz.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotImportoSanzioniACCERTAMENTO")).ToString("N")
                lbltotImpSanzRid.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotImportoSanzioniRidottoACCERTAMENTO")).ToString("N")
                lbltotInteressi.Text = Convert.ToDecimal(objhashtableRIEPILOGO("TotImportoInteressiACCERTAMENTO")).ToString("N")
                lbltotTotale.Text = Convert.ToDecimal(objhashtableRIEPILOGO("ImportoTotaleAvviso")).ToString("N")

                'Dim FncGestAccert As New ClsGestioneAccertamenti
                ''FncGestAccert.UpdateRiepilogo(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ConstSession.IdEnte, Costanti.TRIBUTO_ICI, id_provvedimento, objhashtableRIEPILOGO)
                'FncGestAccert.UpdateRiepilogo(ConstSession.IdEnte, Utility.Costanti.TRIBUTO_ICI, id_provvedimento, objhashtableRIEPILOGO)
                'FncGestAccert = Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.PopolaRiepilogo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objtemp"></param>
    ''' <returns></returns>
    Protected Function annoBarra(ByVal objtemp As Object) As String
        Dim clsGeneralFunction As New MyUtility
        Dim strTemp As String = ""
        Try
            If Not IsDBNull(objtemp) Then
                strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.annoBarra.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objtemp"></param>
    ''' <returns></returns>
    Protected Function ParseDate(ByVal objtemp As Object) As String
        Dim strTemp As String = ""
        Dim objdate As Date
        Try
            If Not IsDBNull(objtemp) Then
                If objtemp.ToString() <> "" Then
                    objdate = CType(objtemp, Date)
                    If objdate < Date.MaxValue.ToString("dd/MM/yyyy") Then
                        strTemp = Utility.StringOperation.FormatDateTime(objtemp).ToString("dd/MM/yyyy")
                    End If
                    'strTemp = objtemp
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.ParseDate.errore: ", ex)
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
                FormattaNumero = FormatNumber(0, numDec)
            ElseIf NumeroDaFormattareParam.ToString() = "" Or NumeroDaFormattareParam.ToString() = "-1" Or NumeroDaFormattareParam.ToString() = "-1,00" Then
                FormattaNumero = FormatNumber(0, numDec)
            Else
                FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.FormattaNumero.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iInput"></param>
    ''' <returns></returns>
    Public Function IntForGridView(ByVal iInput As Object) As String
        Dim ret As String = String.Empty
        Try
            If iInput.ToString() = "-1" Or iInput.ToString() = "-1,00" Then
                ret = String.Empty
            Else
                ret = Convert.ToString(iInput)
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertato.IntForGridView.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return ret
    End Function
End Class
