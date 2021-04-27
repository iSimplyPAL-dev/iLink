'*** 20120921 - pagamenti ***
Imports System.Configuration
Imports System.Globalization
Imports log4net
''' <summary>
''' Pagina per la ricerca pagamenti.
''' Contiene i parametri di ricerca e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicPag
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicPag))
    'Private WFSessione As OPENUtility.CreateSessione
    'Private WFErrore As String

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oMySearchPag As New ObjSearchPagamenti
        Try
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If
            hfIsFromVariabile.Value = Request.Item("IsFromVariabile")
            Dim myStringConnection As String = ConstSession.StringConnection
            Dim IdTributo As String = Request.Item("TRIBUTO")
            Session("oAnagrafe") = Nothing
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                hdTributo.Value = IdTributo
                myStringConnection = ConstSession.StringConnectionOSAP
            Else
                hdTributo.Value = Utility.Costanti.TRIBUTO_TARSU
            End If
            txtCognome.Attributes.Add("onkeydown", "keyPress();")
            txtNome.Attributes.Add("onkeydown", "keyPress();")
            txtAnnoRif.Attributes.Add("onkeydown", "keyPress();")
            txtCFPIva.Attributes.Add("onkeydown", "keyPress();")
            txtNAvviso.Attributes.Add("onkeydown", "keyPress();")
            txtDataPagamentoDal.Attributes.Add("onkeydown", "keyPress();")
            txtDataPagamentoAl.Attributes.Add("onkeydown", "keyPress();")
            txtDataAccreditoDal.Attributes.Add("onkeydown", "keyPress();")
            txtDataAccreditoAl.Attributes.Add("onkeydown", "keyPress();")
            txtFlusso.Attributes.Add("onkeydown", "keyPress();")
            ddlProvenienza.Attributes.Add("onkeydown", "keyPress();")
            If Page.IsPostBack = False Then
                Dim oLoadCombo As New generalClass.generalFunction
                dim sSQL as string = "EXEC PRC_GETPAGPROVENIENZE"
                oLoadCombo.LoadComboGenerale(ddlProvenienza, sSQL, myStringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                If Not Session("oMySearchPag") Is Nothing Then
                    oMySearchPag = CType(Session("oMySearchPag"), ObjSearchPagamenti)
                    txtAnnoRif.Text = oMySearchPag.sAnnoRif
                    txtCFPIva.Text = oMySearchPag.sCFPIVA
                    txtCognome.Text = oMySearchPag.sCognome
                    txtNAvviso.Text = oMySearchPag.sNAvviso
                    txtNome.Text = oMySearchPag.sNome
                    If oMySearchPag.tDataPagamentoDal <> Date.MinValue And oMySearchPag.tDataPagamentoDal <> DateTime.MaxValue Then
                        txtDataPagamentoDal.Text = oMySearchPag.tDataPagamentoDal
                    End If
                    If oMySearchPag.tDataPagamentoAl <> Date.MinValue And oMySearchPag.tDataPagamentoAl <> DateTime.MaxValue Then
                        txtDataPagamentoAl.Text = oMySearchPag.tDataPagamentoAl
                    End If
                    If oMySearchPag.tDataAccreditoDal <> Date.MinValue And oMySearchPag.tDataAccreditoDal <> DateTime.MaxValue Then
                        txtDataAccreditoDal.Text = oMySearchPag.tDataAccreditoDal
                    End If
                    If oMySearchPag.tDataAccreditoAl <> Date.MinValue And oMySearchPag.tDataAccreditoAl <> DateTime.MaxValue Then
                        txtDataAccreditoAl.Text = oMySearchPag.tDataAccreditoAl
                    End If
                    chkNonAbb.Checked = oMySearchPag.IsNonAbbinati
                    txtFlusso.Text = oMySearchPag.Flusso
                    ddlProvenienza.SelectedValue = oMySearchPag.Provenienza
                    DisplayCmd()
                    Dim sScript As String
                    sScript = "Search();"
                    RegisterScript(sScript, Me.GetType)

                    Session("myObjectForTestataSearch") = Nothing
                Else
                    txtAnnoRif.Text = ""
                    txtCFPIva.Text = ""
                    txtCognome.Text = ""
                    txtNAvviso.Text = ""
                    txtNome.Text = ""
                    txtDataPagamentoDal.Text = ""
                    txtDataPagamentoAl.Text = ""
                    txtDataAccreditoDal.Text = ""
                    txtDataAccreditoAl.Text = ""
                    txtFlusso.Text = ""
                    ddlProvenienza.SelectedValue = ""
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If

            If ConstSession.Ambiente.ToUpper() = "CMGC" And ConstSession.IsFromVariabile <> "1" Then
                Dim sScript As String = "$('#NewInsert', parent.frames['Comandi'].document).addClass('DisableBtn');"
                sScript += "$('#chkNonAbb').addClass('DisableBtn');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicPag.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnStampaNonPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaNonPag.Click
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sPathProspetti, sNameXLS As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x, nCol As Integer
        Dim dvDati As New DataView
        Dim FncPag As New ClsGestPag

        Dim myStringConnection As String = ConstSession.StringConnection
        Dim IdTributo As String = Request.Item("TRIBUTO")
        If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
            myStringConnection = ConstSession.StringConnectionOSAP
        ElseIf IdTributo = Nothing Then
            IdTributo = ""
        End If
        nCol = 12
        Try
            oMySearchPag = ValParamSearch(IdTributo, 1)
            dvDati = FncPag.GetStampaPagamentiNonPres(oMySearchPag, myStringConnection)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicPag.btnStampaNonPag_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        If Not IsNothing(dvDati) Then
            'valorizzo il nome del file
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_SOGGETTI_NON_PAGATO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            DtDatiStampa = FncStampa.PrintPagamenti(oMySearchPag.nTipoStampa, IdTributo, dvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, "Soggetti che non hanno pagato")
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub
    Private Sub btnStampaPMag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaPMag.Click
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sPathProspetti, sNameXLS As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x, nCol As Integer
        Dim dvDati As New DataView
        Dim FncPag As New ClsGestPag
        Dim myStringConnection As String = ConstSession.StringConnection
        Dim IdTributo As String = Request.Item("TRIBUTO")
        If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
            myStringConnection = ConstSession.StringConnectionOSAP
        ElseIf IdTributo = Nothing Then
            IdTributo = ""
        End If
        nCol = 12
        sPathProspetti = "" : sNameXLS = ""
        Try
            DivAttesa.Style.Add("display", "")
            oMySearchPag = ValParamSearch(IdTributo, 3)
            dvDati = FncPag.GetStampaPagamenti(oMySearchPag, myStringConnection)
            If Not IsNothing(dvDati) Then
                'valorizzo il nome del file
                sPathProspetti = ConstSession.PathProspetti
                sNameXLS = ConstSession.IdEnte & "_SOGGETTI_PAGATO_MAG_EMESSO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

                DtDatiStampa = FncStampa.PrintPagamenti(oMySearchPag.nTipoStampa, IdTributo, dvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, "Soggetti con importo pagato maggiore dell'importo emesso")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicPag.btnStampaPMag.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not DtDatiStampa Is Nothing Then
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub
    Private Sub btnStampaPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaPag.Click
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sPathProspetti, sNameXLS As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x, nCol As Integer
        Dim dvDati As New DataView
        Dim FncPag As New ClsGestPag

        Dim myStringConnection As String = ConstSession.StringConnection
        Dim IdTributo As String = Request.Item("TRIBUTO")
        If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
            myStringConnection = ConstSession.StringConnectionOSAP
        ElseIf IdTributo = Nothing Then
            IdTributo = ""
        End If
        nCol = 13
        Try
            DivAttesa.Style.Add("display", "")
            oMySearchPag = ValParamSearch(IdTributo, 0)
            dvDati = FncPag.GetStampaPagamenti(oMySearchPag, myStringConnection)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicPag.btnStampaPag_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(dvDati) Then
            'valorizzo il nome del file
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_ELENCOPAGAMENTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            DtDatiStampa = FncStampa.PrintPagamenti(oMySearchPag.nTipoStampa, IdTributo, dvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, "Elenco pagamenti")
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub
    Private Sub btnStampaPMin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaPMin.Click
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sPathProspetti, sNameXLS As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x, nCol As Integer
        Dim dvDati As New DataView
        Dim FncPag As New ClsGestPag
        Dim myStringConnection As String = ConstSession.StringConnection
        Dim IdTributo As String = Request.Item("TRIBUTO")
        If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
            myStringConnection = ConstSession.StringConnectionOSAP
        ElseIf IdTributo = Nothing Then
            IdTributo = ""
        End If
        nCol = 12
        Try
            DivAttesa.Style.Add("display", "")
            oMySearchPag = ValParamSearch(IdTributo, 2)
            dvDati = FncPag.GetStampaPagamenti(oMySearchPag, myStringConnection)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicPag.btnStampaPMin.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(dvDati) Then
            'valorizzo il nome del file
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_SOGGETTI_PAGATO_MIN_EMESSO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            DtDatiStampa = FncStampa.PrintPagamenti(oMySearchPag.nTipoStampa, IdTributo, dvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, "Soggetti con importo pagato minore dell'importo emesso")
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub
    Private Sub btnStampaRiversamento_Click(sender As Object, e As System.EventArgs) Handles btnStampaRiversamento.Click
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sPathProspetti, sNameXLS As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x, nCol As Integer
        Dim dvDati As New DataView
        Dim FncPag As New ClsGestPag

        Dim myStringConnection As String = ConstSession.StringConnection
        Dim IdTributo As String = Request.Item("TRIBUTO")
        If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
            myStringConnection = ConstSession.StringConnectionOSAP
        ElseIf IdTributo = Nothing Then
            IdTributo = ""
        End If
        nCol = 4
        Try
            DivAttesa.Style.Add("display", "")
            oMySearchPag = ValParamSearch(IdTributo, -1)
            dvDati = FncPag.GetStampaRiversamento(oMySearchPag, myStringConnection)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicPag.btnStampaRiversamento_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(dvDati) Then
            'valorizzo il nome del file
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_RIVERSAMENTO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            DtDatiStampa = FncStampa.PrintRiversamento(dvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, "Riversamento")
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                'Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4}
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub
    Private Sub btnStampaQuadratura_Click(sender As Object, e As System.EventArgs) Handles btnStampaQuadratura.Click
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sPathProspetti, sNameXLS As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x, nCol As Integer
        Dim dvDati As New DataView
        Dim FncPag As New ClsGestPag

        Dim myStringConnection As String = ConstSession.StringConnection
        Dim IdTributo As String = Request.Item("TRIBUTO")
        If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
            myStringConnection = ConstSession.StringConnectionOSAP
        ElseIf IdTributo = Nothing Then
            IdTributo = ""
        End If
        nCol = 2
        Try
            DivAttesa.Style.Add("display", "")
            oMySearchPag = ValParamSearch(IdTributo, -1)
            dvDati = FncPag.GetStampaQuadratura(oMySearchPag, myStringConnection)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicPag.btnStampaQuadratura_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(dvDati) Then
            'valorizzo il nome del file
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_QUADRATURA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            DtDatiStampa = FncStampa.PrintQuadratura(dvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, "Quadratura")
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub

    Private Sub chkNonAbb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNonAbb.CheckedChanged
        displaycmd()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub DisplayCmd()
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer
        Try
            ReDim Preserve oListCmd(5)
            oListCmd(0) = "StampaNonP"
            oListCmd(1) = "StampaPmag"
            oListCmd(2) = "StampaPMin"
            oListCmd(3) = "NewInsert"
            oListCmd(4) = "Quadratura"
            oListCmd(5) = "Riversamento"
            For x = 0 To oListCmd.Length - 1
                If chkNonAbb.Checked = True Then
                    sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).addClass('DisableBtn');"
                Else
                    sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).removeClass('DisableBtn');"
                End If
            Next
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicPag.DisplayCmd.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub DisplayCmd()
    '    Dim sScript As String = ""
    '    Dim oListCmd() As Object
    '    Dim x As Integer
    '    Dim myDisplay As String
    '    Try
    '        If chkNonAbb.Checked = True Then
    '            myDisplay = "none"
    '        Else
    '            myDisplay = ""
    '        End If

    '        ReDim Preserve oListCmd(5)
    '        oListCmd(0) = "StampaNonP"
    '        oListCmd(1) = "StampaPmag"
    '        oListCmd(2) = "StampaPMin"
    '        oListCmd(3) = "NewInsert"
    '        oListCmd(4) = "Quadratura"
    '        oListCmd(5) = "Riversamento"
    '        For x = 0 To oListCmd.Length - 1
    '            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').style.display='" & myDisplay & "';"
    '        Next
    '        RegisterScript( sScript,Me.GetType)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicPag.DisplayCmd.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    Private Function ValParamSearch(IdTributo As String, TipoStampa As Integer) As ObjSearchPagamenti
        Dim mySearch As New ObjSearchPagamenti
        Try
            mySearch.sEnte = ConstSession.IdEnte
            mySearch.IdTributo = IdTributo
            mySearch.sCFPIVA = txtCFPIva.Text
            mySearch.sCognome = txtCognome.Text
            mySearch.sNAvviso = txtNAvviso.Text
            mySearch.sNome = txtNome.Text
            mySearch.sAnnoRif = txtAnnoRif.Text
            If txtDataPagamentoDal.Text <> "" Then
                mySearch.tDataPagamentoDal = txtDataPagamentoDal.Text
            End If
            If txtDataPagamentoAl.Text <> "" Then
                mySearch.tDataPagamentoAl = txtDataPagamentoAl.Text
            End If
            If txtDataAccreditoDal.Text <> "" Then
                mySearch.tDataAccreditoDal = txtDataAccreditoDal.Text
            End If
            If txtDataAccreditoAl.Text <> "" Then
                mySearch.tDataAccreditoAl = txtDataAccreditoAl.Text
            End If
            mySearch.IsNonAbbinati = chkNonAbb.Checked
            mySearch.Flusso = txtFlusso.Text
            mySearch.Provenienza = ddlProvenienza.SelectedValue
            mySearch.nTipoStampa = TipoStampa '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicPag.ValParamSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            mySearch = New ObjSearchPagamenti
        End Try
        Return mySearch
    End Function
End Class
'*** ***