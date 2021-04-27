Imports DichiarazioniICI.Database
Imports Business
'Imports OPENUtility
'Imports RIBESFrameWork
Imports log4net
Imports System.Web.HttpContext

''' <summary>
''' Pagina dei comandi per la gestione degli incroci fra banca dati tributaria e ANATER.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class IncrocioBancaDati
    Inherits BasePage

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

    Private Shared Log As ILog = LogManager.GetLogger(GetType(IncrocioBancaDati))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Page.IsPostBack = False Then
                loadDDL()
            Else
                If Not Session("dvIncrocioTerTrib") Is Nothing Then
                    Dim dvMydati As New DataView
                    dvMydati = CType(Session("dvIncrocioTerTrib"), DataView)
                    GrdUI.DataSource = dvMydati
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub loadDDL()
        Try
            'valorizzazionde ddl Tributi
            Dim dtTributi As DataTable = New TributiTable(ConstWrapper.sUsername).TributiList()
            ddlTributo.Items.Clear()
            If dtTributi.Rows.Count > 0 Then
                Dim li As New ListItem("...", "-1")
                ddlTributo.Items.Add(li)

                For Each dr As DataRow In dtTributi.Rows
                    If dr("COD_TRIBUTO").ToString = Utility.Costanti.TRIBUTO_ICI Or dr("COD_TRIBUTO").ToString = Utility.Costanti.TRIBUTO_TARSU Then
                        'solo tributo ICI e TARSU
                        Dim li1 As New ListItem(dr("DESCRIZIONE").ToString, dr("COD_TRIBUTO").ToString)
                        ddlTributo.Items.Add(li1)
                    End If
                Next
            End If

            Dim x As Integer = DateTime.Today.Year
            Do While (x > 2005)
                Dim myListItem As New ListItem
                myListItem.Text = x.ToString
                myListItem.Value = x.ToString
                ddlAnno.Items.Add(myListItem)
                x -= 1
            Loop
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.loadDDL.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cod_tributo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="04/07/2012">
    ''' <strong>IMU</strong>
    ''' passaggio tributo da ICI a IMU
    ''' </revision>
    ''' </revisionHistory>
    Protected Function SetTributo(ByVal cod_tributo As String) As String
        SetTributo = ""
        '*** 20120704 - IMU ***
        Select Case cod_tributo
            Case "8852" : SetTributo = "ICI/IMU"
            Case "0434" : SetTributo = "TARSU"
            Case "0465" : SetTributo = "TIA"
            Case "9000" : SetTributo = "H2O"
        End Select
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvMyDati As New DataView
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim sMyProcedure As String

        Try
            lblNotFound.Visible = False
            If radioGA.Checked Then
                'Tutte le UI presenti in GEC e non in ANATER
                sMyProcedure = "prc_RIFDICHnoRIFTER"
            ElseIf radioAG.Checked Then
                'Tutte le UI presenti in ANATER e non in GEC
                sMyProcedure = "prc_RIFTERnoRIFDICH"
            ElseIf radioSUP.Checked Then
                'Tutte le UI presenti in entrambe le banche dati ma con superfici incoerenti
                sMyProcedure = "prc_RIFDICHvsRIFTER"
            End If
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = sMyProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = COSTANTValue.ConstSession.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Tributo", SqlDbType.VarChar)).Value = ddlTributo.SelectedValue
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            dvMyDati = dtMyDati.DefaultView
            Session("dvIncrocioTerTrib") = dvMyDati
            If Not dvMyDati Is Nothing Then
                If dvMyDati.Count > 0 Then
                    VisualizzaExcel()
                    ViewState("SortKey") = "foglio"
                    ViewState("OrderBy") = "ASC"

                    GrdUI.Visible = True
                    dvMyDati.Sort = ViewState("SortKey").ToString & " " & ViewState("OrderBy").ToString

                    GrdUI.DataSource = dvMyDati
                    GrdUI.DataBind()
                    lblNotFound.Visible = False
                Else
                    lblNotFound.Visible = True
                End If
            Else
                Log.Debug("UIAnaterNoGEC::Nessun record trovato")
                GrdUI.Visible = False
                lblNotFound.Visible = True
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.btnRicerca_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdUI_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdUI.SortCommand
    '    Dim strSortKey As String
    '    Dim dvMyDati As DataView
    '    Dim objDS As DataSet

    '    Try
    '        If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
    '            Select Case ViewState("OrderBy").ToString()
    '                Case "ASC"
    '                    ViewState("OrderBy") = "DESC"

    '                Case "DESC"
    '                    ViewState("OrderBy") = "ASC"
    '            End Select
    '        Else
    '            ViewState("SortKey") = e.SortExpression
    '            ViewState("OrderBy") = "ASC"
    '        End If

    '        dvMyDati = CType(Session("dvIncrocioTerTrib"), DataView)
    '        dvMyDati.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")

    '        GrdUI.DataSource = dvMyDati
    '        GrdUI.DataBind()
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.GrdUI_SortCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../Styles.css"))
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
            GrdUI.DataSource = CType(Session("dvIncrocioTerTrib"), DataView)
            If page.HasValue Then
                GrdUI.PageIndex = page.Value
            End If
            GrdUI.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnModificaRadio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificaRadio.Click
        Try
            Pulisci()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.btnModificaRadio_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
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
        GrdUI.Visible = False
        Try
            If ddlTributo.SelectedValue = "8852" Then
                radioGA.Enabled = True
                radioAG.Enabled = True
                radioSUP.Enabled = False
            Else
                radioGA.Enabled = True
                radioAG.Enabled = True
                radioSUP.Enabled = True
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.Pulisci.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub VisualizzaExcel()
        Dim strscript As String = "parent.Comandi.document.getElementById ('Excel').style.display=''"
        RegisterScript(strscript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub radioGA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioGA.CheckedChanged
        Pulisci()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub radioAG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioAG.CheckedChanged
        Pulisci()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub radioSUP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioSUP.CheckedChanged
        Pulisci()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Dim desc_tributo As String 'cod_tributo, 
        Dim arraylistNomiColonne As ArrayList
        Dim ds As DataSet
        Dim dr As DataRow
        Dim dtDatiAttuali As DataTable
        Dim dvMyDati As DataView
        Dim i As Integer
        Dim sNameXLS As String
        Dim arraystr As String()

        Try
            arraylistNomiColonne = New ArrayList

            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")
            arraylistNomiColonne.Add("")

            arraystr = CType(arraylistNomiColonne.ToArray(GetType(String)), String())

            ds = CreateDataSetDati()
            dtDatiAttuali = ds.Tables("DATI")

            '**** INTESTAZIONE ****
            dr = dtDatiAttuali.NewRow
            dr(0) = Session("DESCRIZIONE_ENTE")
            dr(6) = "Stampato il " & DateTime.Now.ToString()
            dtDatiAttuali.Rows.Add(dr)

            dr = dtDatiAttuali.NewRow
            dtDatiAttuali.Rows.Add(dr)
            dr = dtDatiAttuali.NewRow
            dtDatiAttuali.Rows.Add(dr)

            dvMyDati = CType(Session("dvIncrocioTerTrib"), DataView)
            desc_tributo = SetTributo(ddlTributo.SelectedValue)
            If radioGA.Checked Then
                'Tutte le UI presenti in GEC e non in ANATER
                sNameXLS = "UI_GEC_No_ANATER_" & desc_tributo
            End If
            If radioAG.Checked Then
                'Tutte le UI presenti in ANATER e non in GEC
                sNameXLS = "UI_ANATER_No_GEC_" & desc_tributo
            End If
            If radioSUP.Checked Then
                'Tutte le UI presenti in entrambe le banche dati ma con superfici incoerenti
                sNameXLS = "UI_SUP_INC_" & desc_tributo
            End If

            dr = dtDatiAttuali.NewRow
            dr(0) = "Tributo"
            dr(3) = desc_tributo.ToUpper
            dtDatiAttuali.Rows.Add(dr)
            dr = dtDatiAttuali.NewRow
            dtDatiAttuali.Rows.Add(dr)

            If Not dvMyDati Is Nothing Then
                If radioGA.Checked Then
                    'Tutte le UI presenti in GEC e non in ANATER
                    dr = dtDatiAttuali.NewRow
                    dr(0) = "Ubicazione"
                    dr(3) = "Unità Immobiliare"
                    dtDatiAttuali.Rows.Add(dr)
                    dr = dtDatiAttuali.NewRow
                    dr(0) = "Via"
                    dr(1) = "Civico"
                    dr(2) = "Interno"
                    dr(3) = "Foglio"
                    dr(4) = "Numero"
                    dr(5) = "Subalterno"
                    dtDatiAttuali.Rows.Add(dr)
                End If
                If radioAG.Checked Then
                    'Tutte le UI presenti in ANATER e non in GEC
                    dr = dtDatiAttuali.NewRow
                    dr(0) = "Unità Immobiliare"
                    dtDatiAttuali.Rows.Add(dr)
                    dr = dtDatiAttuali.NewRow
                    dr(0) = "Foglio"
                    dr(1) = "Numero"
                    dr(2) = "Subalterno"
                    dr(3) = "Stato Utilizzo Anater"
                    dr(4) = "Stato Utilizzo GEC"
                    dtDatiAttuali.Rows.Add(dr)
                End If
                If radioSUP.Checked Then
                    'Tutte le UI presenti in entrambe le banche dati ma con superfici incoerenti
                    dr = dtDatiAttuali.NewRow
                    dr(0) = "Ubicazione"
                    dr(3) = "Unità Immobiliare"
                    dtDatiAttuali.Rows.Add(dr)
                    dr = dtDatiAttuali.NewRow
                    dr(0) = "Via"
                    dr(1) = "Civico"
                    dr(2) = "Interno"
                    dr(3) = "Foglio"
                    dr(4) = "Numero"
                    dr(5) = "Subalterno"
                    dr(6) = "Stato Utilizzo Anater"
                    dr(7) = "Sup. Anater"
                    dr(8) = "Sup. GEC"
                    dr(9) = "" 'Stato Utilizzo GEC 
                    dtDatiAttuali.Rows.Add(dr)
                End If

                If dvMyDati.Count > 0 Then
                    For i = 0 To dvMyDati.Count - 1
                        dr = dtDatiAttuali.NewRow
                        If radioGA.Checked Then
                            'Tutte le UI presenti in GEC e non in ANATER
                            dr(0) = dvMyDati.Item(i)("Via").ToString
                            dr(1) = dvMyDati.Item(i)("Civico").ToString
                            dr(2) = ". " & dvMyDati.Item(i)("Interno").ToString
                            dr(3) = dvMyDati.Item(i)("Foglio").ToString
                            dr(4) = dvMyDati.Item(i)("Numero").ToString
                            dr(5) = dvMyDati.Item(i)("Subalterno").ToString
                            dr(6) = ""
                            dr(7) = ""
                            dr(8) = ""
                            dr(9) = ""
                        End If
                        If radioAG.Checked Then
                            'Tutte le UI presenti in ANATER e non in GEC
                            dr(0) = dvMyDati.Item(i)("Foglio").ToString
                            dr(1) = dvMyDati.Item(i)("Numero").ToString
                            dr(2) = dvMyDati.Item(i)("Subalterno").ToString
                            dr(3) = dvMyDati.Item(i)("StatoUtilizzoANA").ToString
                            dr(4) = ""
                            dr(5) = ""
                            dr(6) = ""
                            dr(7) = ""
                            dr(8) = ""
                            dr(9) = ""
                        End If
                        If radioSUP.Checked Then
                            'Tutte le UI presenti in entrambe le banche dati ma con superfici incoerenti
                            dr(0) = dvMyDati.Item(i)("Via").ToString
                            dr(1) = dvMyDati.Item(i)("Civico").ToString
                            dr(2) = ". " & dvMyDati.Item(i)("Interno").ToString
                            dr(3) = dvMyDati.Item(i)("Foglio").ToString
                            dr(4) = dvMyDati.Item(i)("Numero").ToString
                            dr(5) = dvMyDati.Item(i)("Subalterno").ToString
                            dr(6) = dvMyDati.Item(i)("StatoUtilizzoANA").ToString
                            dr(7) = dvMyDati.Item(i)("SupANATER").ToString
                            dr(8) = dvMyDati.Item(i)("SupGEC").ToString
                            dr(9) = ""
                        End If
                        dtDatiAttuali.Rows.Add(dr)
                    Next
                End If

                '**** SALVATAGGIO PERCORSO ****
                sNameXLS = COSTANTValue.ConstSession.IdEnte & "_" & sNameXLS & "_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            End If
        Catch ex As Exception
            sNameXLS = ""
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.btnExcel_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            arraylistNomiColonne = Nothing
            ds.Dispose()
            dr = Nothing
            dtDatiAttuali.Dispose()
            dvMyDati.Dispose()
            arraystr = Nothing
        End Try
        If sNameXLS <> "" Then
            Dim iColumns As Integer() = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(dtDatiAttuali, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateDataSetDati() As DataSet

        Dim dsTmp As New DataSet

        dsTmp.Tables.Add("DATI")

        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)
        dsTmp.Tables("DATI").Columns.Add("").DataType = GetType(System.String)

        CreateDataSetDati = dsTmp

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdUpdateFigli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdUpdateFigli.Click
        'Dim WFSessione As OPENUtility.CreateSessione
        'Dim WFErrore As String
        Dim FncUtility As New UtilityOPENgov
        Dim sSQL, sFinePrec, sInizio As String
        Dim dvMyDati, dvFigli, dvUI As DataView
        Dim x, y, z, w, nFigli, nFamigliaPrec, nCambi As Integer
        Dim ListCarico() As Utility.DichManagerICI.CaricoFigliRow
        Dim CaricoFigli As Utility.DichManagerICI.CaricoFigliRow
        Dim retVal As Boolean
        Dim FncICI As New DichiarazioniICI.Database.DettaglioTestataTable(Session("username").ToString)
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            'prelevo per ogni famiglia il cambio temporale 
            sSQL = "SELECT DISTINCT CODICE_COMUNE, NUMERO_FAMIGLIA"
            sSQL += " , CAST(CASE WHEN YEAR(GETDATE())-YEAR(DATA_NASCITA)=26 OR YEAR(GETDATE())-YEAR(DATA_NASCITA)=0 THEN CAST(YEAR(GETDATE()) AS NVARCHAR)+RIGHT('00'+CAST(MONTH(DATA_NASCITA) AS NVARCHAR),2)+RIGHT('00'+CAST(DAY(DATA_NASCITA) AS NVARCHAR),2) ELSE CAST(YEAR(GETDATE()) AS NVARCHAR)+'1231' END AS DATETIME) AS CAMBIO"
            sSQL += " FROM V_GET_IMU_FIGLIMINORI26"
            sSQL += " WHERE (1=1)"
            sSQL += " ORDER BY CODICE_COMUNE, NUMERO_FAMIGLIA, CAMBIO"

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov) 'WFSessioneAnagrafica.oSession.oAppDB.GetConnection()
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            dvMyDati = myDataSet.Tables(0).DefaultView 'WFSessione.oSession.oAppDB.GetPrivateDataview(sSQL)
            If dvMyDati.Count > 0 Then
                For x = 0 To dvMyDati.Count - 1
                    If nFamigliaPrec <> CInt(dvMyDati.Item(x)("NUMERO_FAMIGLIA")) Then
                        nCambi = 0
                    End If
                    nCambi += 1
                    'prelevo per ogni famiglia+cambio il numero di figli
                    sSQL = "SELECT NFIGLI"
                    sSQL += " FROM V_GET_IMU_NUMEROFIGLIMINORI26"
                    sSQL += " WHERE (1=1)"
                    sSQL += " AND (CODICE_COMUNE=@CODICE_COMUNE)"
                    sSQL += " AND (NUMERO_FAMIGLIA=@NUMERO_FAMIGLIA)"
                    sSQL += " AND (INIZIO<@CAMBIO)"
                    sSQL += " AND (FINE>=@CAMBIO)"
                    Try
                        cmdMyCommand.CommandText = sSQL
                        cmdMyCommand.Parameters.Clear()
                        cmdMyCommand.Parameters.AddWithValue("@CODICE_COMUNE", CStr(dvMyDati.Item(x)("CODICE_COMUNE")))
                        cmdMyCommand.Parameters.AddWithValue("@NUMERO_FAMIGLIA", CStr(dvMyDati.Item(x)("NUMERO_FAMIGLIA")))
                        cmdMyCommand.Parameters.AddWithValue("@CAMBIO", FncUtility.GiraData(CStr(dvMyDati.Item(x)("CAMBIO"))))
                        myAdapter.SelectCommand = cmdMyCommand
                        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                        myAdapter.Fill(myDataSet, "Create DataView")
                        dvFigli = myDataSet.Tables(0).DefaultView
                        For y = 0 To dvFigli.Count - 1
                            nFigli = CInt(dvFigli.Item(y)("NFIGLI"))
                            'prelevo gli identificativi per famiglia da aggiornare
                            sSQL = "SELECT *"
                            sSQL += " FROM V_GET_IMU_FIGLIMINORI26IMMOBILI"
                            sSQL += " WHERE 1=1"
                            sSQL += " AND (CODICE_COMUNE=@CODICE_COMUNE)"
                            sSQL += " AND (NUMERO_FAMIGLIA=@NUMERO_FAMIGLIA)"
                            Try
                                cmdMyCommand.CommandText = sSQL
                                cmdMyCommand.Parameters.Clear()
                                cmdMyCommand.Parameters.AddWithValue("@CODICE_COMUNE", CStr(dvMyDati.Item(x)("CODICE_COMUNE")))
                                cmdMyCommand.Parameters.AddWithValue("@NUMERO_FAMIGLIA", CStr(dvMyDati.Item(x)("NUMERO_FAMIGLIA")))
                                myAdapter.SelectCommand = cmdMyCommand
                                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                                myAdapter.Fill(myDataSet, "Create DataView")
                                dvUI = myDataSet.Tables(0).DefaultView
                                For z = 0 To dvUI.Count - 1
                                    'se il cambio è fine anno ed è l'unico aggiorno semplicemente altrimenti devo spezzare i periodi dell'immobile
                                    If nCambi = 1 Then 'update
                                        If SetNewDettaglio(1, sInizio, sFinePrec, CStr(dvUI.Item(z)("IDOGGETTO")), CStr(dvUI.Item(z)("IDDETTAGLIO")), nFigli) = False Then
                                            Exit Sub
                                        End If
                                    Else 'new
                                        If SetNewDettaglio(0, sInizio, sFinePrec, CStr(dvUI.Item(z)("IDOGGETTO")), CStr(dvUI.Item(z)("IDDETTAGLIO")), nFigli) = False Then
                                            Exit Sub
                                        End If
                                    End If
                                    'setto le % di carico
                                    ReDim Preserve ListCarico((nFigli) - 1)
                                    For w = 0 To nFigli - 1
                                        CaricoFigli = New Utility.DichManagerICI.CaricoFigliRow
                                        CaricoFigli.IdDettaglioTestata = CInt(dvUI.Item(z)("IDDETTAGLIO"))
                                        CaricoFigli.nFiglio = w + 1
                                        CaricoFigli.Percentuale = CDbl(dvUI.Item(z)("Percpossesso"))
                                        ListCarico(w) = CaricoFigli
                                    Next
                                    'elimino le % vecchie
                                    retVal = FncICI.SetPercentualeCaricoFigli(2, Nothing, CInt(dvUI.Item(z)("IDDETTAGLIO")))
                                    If (retVal = True) Then
                                        '*** 20120629 - IMU ***
                                        If (Not (ListCarico) Is Nothing) Then
                                            'inserisco le % correnti
                                            retVal = FncICI.SetPercentualeCaricoFigli(0, ListCarico, CInt(dvUI.Item(z)("IDDETTAGLIO")))
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.CmdUpdateFigli_Click.errore: ", ex)
                                Response.Redirect("../../PaginaErrore.aspx")
                                Exit Sub
                            Finally
                                dvUI.Dispose()
                            End Try
                        Next
                    Catch ex As Exception
                        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.CmdUpdateFigli_Click.errore: ", ex)
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Sub
                    Finally
                        dvFigli.Dispose()
                    End Try
                    nFamigliaPrec = CInt(dvMyDati.Item(x)("NUMERO_FAMIGLIA"))
                    sFinePrec = FncUtility.GiraData(dvMyDati.Item(x)("CAMBIO").ToString)
                    sInizio = FncUtility.GiraData(DateAdd(DateInterval.Day, 1, CDate(dvMyDati.Item(x)("CAMBIO"))).ToString)
                Next
            End If
            RegisterScript("alert('Finito!')", Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.CmdUpdateFigli_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            dvMyDati.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Tipo"></param>
    ''' <param name="sDataInizio"></param>
    ''' <param name="sDataFine"></param>
    ''' <param name="nIdOggetto"></param>
    ''' <param name="nIdDettaglio"></param>
    ''' <param name="nFigli"></param>
    ''' <returns></returns>
    Private Function SetNewDettaglio(ByVal Tipo As Integer, ByVal sDataInizio As String, ByVal sDataFine As String, ByVal nIdOggetto As Integer, ByVal nIdDettaglio As Integer, ByVal nFigli As Integer) As Boolean
        'Dim WFSessione As OPENUtility.CreateSessione
        'Dim WFErrore As String
        Dim FncUtility As New UtilityOPENgov
        dim sSQL as string
        Dim myIdentity As Integer
        Dim DrReturn As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            If Tipo = 0 Then
                'inserisco nuova riga in tbloggetti
                sSQL = "INSERT INTO TBLOGGETTI(ENTE, IDTESTATA, NUMEROORDINE, NUMEROMODELLO"
                sSQL += " , CODUI, TIPOIMMOBILE, PARTITACATASTALE, FOGLIO, NUMERO, SUBALTERNO, CARATTERISTICA, SEZIONE, NUMEROPROTCATASTALE, ANNODENUNCIACATASTALE"
                sSQL += " , CODCATEGORIACATASTALE, CODCLASSE, CODRENDITA, STORICO, VALOREIMMOBILE, IDVALUTA, FLAGVALOREPROVV"
                sSQL += " , CODCOMUNE, COMUNE, CODVIA, VIA, NUMEROCIVICO, ESPCIVICO, SCALA, INTERNO, PIANO, BARRATO, NUMEROECOGRAFICO"
                sSQL += " , TITOLOACQUISTO, TITOLOCESSIONE, DESCRUFFREGISTRO"
                sSQL += " , DATAINIZIOVALIDITÀ, DATAFINEVALIDITÀ, BONIFICATO, ANNULLATO, DATAULTIMAMODIFICA, OPERATORE"
                sSQL += " , DATAINIZIO, DATAFINE, IDIMMOBILEPERTINENTE, NOTEICI, ZONA, RENDITA, CONSISTENZA, EXRURALE)"
                sSQL += " SELECT ENTE, IDTESTATA, NUMEROORDINE, NUMEROMODELLO"
                sSQL += " , CODUI, TIPOIMMOBILE, PARTITACATASTALE, FOGLIO, NUMERO, SUBALTERNO, CARATTERISTICA, SEZIONE, NUMEROPROTCATASTALE, ANNODENUNCIACATASTALE"
                sSQL += " , CODCATEGORIACATASTALE, CODCLASSE, CODRENDITA, STORICO, VALOREIMMOBILE, IDVALUTA, FLAGVALOREPROVV"
                sSQL += " , CODCOMUNE, COMUNE, CODVIA, VIA, NUMEROCIVICO, ESPCIVICO, SCALA, INTERNO, PIANO, BARRATO, NUMEROECOGRAFICO"
                sSQL += " , TITOLOACQUISTO, TITOLOCESSIONE, DESCRUFFREGISTRO"
                sSQL += " , DATAINIZIOVALIDITÀ, DATAFINEVALIDITÀ, BONIFICATO, ANNULLATO, DATAULTIMAMODIFICA, 'MINORI' AS OPERATORE"
                sSQL += " , " + FncUtility.CStrToDB(sDataInizio) + " AS DATAINIZIO, DATAFINE, IDIMMOBILEPERTINENTE, NOTEICI, ZONA, RENDITA, CONSISTENZA, EXRURALE"
                sSQL += " FROM TBLOGGETTI"
                sSQL += " WHERE (ID=" + nIdOggetto.ToString + ") "
                sSQL += " SELECT @@IDENTITY AS ID"
                Try
                    'eseguo la query
                    cmdMyCommand = New SqlClient.SqlCommand
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
                    cmdMyCommand.CommandType = CommandType.Text
                    cmdMyCommand.CommandText = sSQL
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    DrReturn = cmdMyCommand.ExecuteReader()
                    Do While DrReturn.Read
                        myIdentity = DrReturn(0)
                    Loop
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.SetNewDettaglio.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
                    Return False
                Finally
                    DrReturn.Close()
                End Try

                'setto data chiusura in riga originale
                sSQL = "UPDATE TBLOGGETTI SET DATAFINE=" + FncUtility.CStrToDB(sDataFine)
                sSQL += " WHERE (ID=" + nIdOggetto.ToString + ") "
                Try
                    cmdMyCommand.CommandText = sSQL
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    If cmdMyCommand.ExecuteNonQuery() < 0 Then 'If Not WFSessione.oSession.oAppDB.Execute(sSQL) Then
                        Log.Debug("Start Up IMU::SetNewDettaglio::" + vbTab & "Errore query: " & sSQL)
                    End If
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.SetNewDettaglio.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
                    Return False
                End Try
                'inserisco nuova riga in tbldettagliotestata con possesso variato
                sSQL = "INSERT INTO TBLDETTAGLIOTESTATA (ENTE, IDTESTATA, NUMEROORDINE, NUMEROMODELLO"
                sSQL += " , IDOGGETTO, IDSOGGETTO, TIPOPOSSESSO, PERCPOSSESSO, MESIPOSSESSO, MESIESCLUSIONEESENZIONE, MESIRIDUZIONE, IMPDETRAZABITAZPRINCIPALE"
                sSQL += " , CONTITOLARE, ABITAZIONEPRINCIPALE"
                sSQL += " , BONIFICATO, ANNULLATO, DATAINIZIOVALIDITÀ, DATAFINEVALIDITÀ, OPERATORE"
                sSQL += " , RIDUZIONE, POSSESSO, ESCLUSIONEESENZIONE"
                sSQL += " , ABITAZIONEPRINCIPALEATTUALE, NUMEROUTILIZZATORI"
                sSQL += " , COLTIVATOREDIRETTO, NUMEROFIGLI)"
                sSQL += " SELECT ENTE, IDTESTATA, NUMEROORDINE, NUMEROMODELLO"
                sSQL += " , " + myIdentity.ToString() + " AS IDOGGETTO, IDSOGGETTO, TIPOPOSSESSO, PERCPOSSESSO, MESIPOSSESSO, MESIESCLUSIONEESENZIONE, MESIRIDUZIONE, IMPDETRAZABITAZPRINCIPALE"
                sSQL += " , CONTITOLARE, ABITAZIONEPRINCIPALE"
                sSQL += " , BONIFICATO, ANNULLATO, " + FncUtility.CStrToDB(sDataInizio) + ", DATAFINEVALIDITÀ, 'MINORI' AS OPERATORE"
                sSQL += " , RIDUZIONE, POSSESSO, ESCLUSIONEESENZIONE"
                sSQL += " , ABITAZIONEPRINCIPALEATTUALE, NUMEROUTILIZZATORI"
                sSQL += " , COLTIVATOREDIRETTO, " + nFigli.ToString + " AS NUMEROFIGLI"
                sSQL += " FROM TBLDETTAGLIOTESTATA"
                sSQL += " WHERE (ID=" + nIdDettaglio.ToString + ") "
                sSQL += " SELECT @@IDENTITY AS ID"
                Try
                    'eseguo la query
                    cmdMyCommand.CommandText = sSQL
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    DrReturn = cmdMyCommand.ExecuteReader
                    Do While DrReturn.Read
                        myIdentity = DrReturn(0)
                    Loop
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.SetNewDettaglio.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
                    Return False
                Finally
                    DrReturn.Close()
                End Try
            Else
                'aggiorno il numero di figli originale
                sSQL = "UPDATE TBLDETTAGLIOTESTATA SET NUMEROFIGLI=" + nFigli.ToString
                sSQL += " WHERE (ID=" + nIdDettaglio.ToString + ") "
                Try
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    If cmdMyCommand.ExecuteNonQuery() < 0 Then 'If Not WFSessione.oSession.oAppDB.Execute(sSQL) Then
                        Log.Debug("Start Up IMU::SetNewDettaglio::" + vbTab & "Errore query: " & sSQL)
                    End If
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.SetNewDettaglio.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
                    Return False
                End Try
            End If
            Return True
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.IncrocioBancaDati.SetNewDettaglio.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
End Class
