Imports log4net
''' <summary>
''' Pagina per la consultazione del raffronto fra fatturato ed incassato
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class AnalisiEconomiche
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(AnalisiEconomiche))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AnalisiEconomiche_Init(sender As Object, e As EventArgs) Handles Me.Init
        lblTitolo.Text = Session("DESCRIZIONE_ENTE")
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARES "
            Else
                info.InnerText = "TARSU "
            End If
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Analisi - Economiche"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.AnlisiEconomiche_Init.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Al caricamento della pagina carica la combo con i possibili anni e i possibili tipi di ruolo da filtrare.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction

        Try
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If

            If Page.IsPostBack = False Then
                'popolo combo Anno
                sSQL = "SELECT *"
                sSQL += " FROM V_GETANNIRUOLO"
                sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
                sSQL += " ORDER BY DESCRIZIONE DESC"
                oLoadCombo.LoadComboGenerale(DdlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                'popolo combo tiporuolo
                sSQL = "SELECT DISTINCT DESCRIZIONE, IDTIPORUOLO, ORDINAMENTO"
                sSQL += " FROM V_GETTIPORUOLO"
                If ConstSession.HasNotifiche = False Then
                    sSQL += " WHERE IDTIPORUOLO<>'I'"
                End If
                sSQL += " ORDER BY ORDINAMENTO"
                oLoadCombo.LoadComboGenerale(DdlTipoRuolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'analisi del raffronto.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdAnalisi_Click(sender As Object, e As EventArgs) Handles CmdAnalisi.Click
        Try
            Dim AnnoRuolo As String = DdlAnno.SelectedValue
            Dim TipoRuolo As String = DdlTipoRuolo.SelectedValue
            Dim DataAccreditoDal As String = TxtValutaDal.Text
            Dim DataAccreditoAl As String = TxtValutaAl.Text

            If DataAccreditoDal <> "" Then
                DataAccreditoDal = CDate(DataAccreditoDal).ToString("yyyyMMdd")
            End If
            If DataAccreditoAl <> "" Then
                DataAccreditoAl = CDate(DataAccreditoAl).ToString("yyyyMMdd")
            End If
            If LoadRiepilogoEmesso(ConstSession.IsFromVariabile, ConstSession.IdEnte, TipoRuolo, AnnoRuolo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                Response.End()
            End If
            If LoadDettaglioEmesso(ConstSession.IsFromVariabile, ConstSession.IdEnte, TipoRuolo, AnnoRuolo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                Response.End()
            End If
            If LoadDettaglioIncassato(ConstSession.IsFromVariabile, ConstSession.IdEnte, TipoRuolo, AnnoRuolo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                Response.End()
            End If

            If LoadDettaglioEmessoVSCat(ConstSession.IsFromVariabile, ConstSession.IdEnte, TipoRuolo, AnnoRuolo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                Response.End()
            End If
            If LoadDettaglioIncassatoVSCat(ConstSession.IsFromVariabile, ConstSession.IdEnte, TipoRuolo, AnnoRuolo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                Response.End()
            End If

            DivAnalisi.Style.Add("display", "")
            DivAttesa.Style.Add("display", "none")
            Dim sScript As String = ""
            sScript = "parent.parent.Comandi.document.getElementById('Grafico').disabled=false"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.CmdAnalisi_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(sender As Object, e As EventArgs) Handles CmdStampa.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()

        nCol = 6
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ANALISIECONOMICHE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        Try
            DtDatiStampa = FncStampa.PrintAnalisiEconomiche(ConstSession.IdEnte & "-" & Session("DESCRIZIONE_ENTE"), Session("dvEmessoVSCat"), Session("dvIncassatoVSCat"), LblNArticoli.Text, LblTotImpRuolo.Text, LblNAvvisi.Text, LblTotImpAvvisi.Text, LblNPagTot.Text, LblImpPagTot.Text, LblNPagParz.Text, LblImpPagParz.Text, LblIncassato.Text, LblNUtenti.Text, LblInsoluto.Text, LblPercentualeInsoluto.Text, LblEmessoSanzioni.Text, LblIncassatoSanzioni.Text, LblEmessoInteressi.Text, LblIncassatoInteressi.Text, LblEmessoECA.Text, LblIncassatoECA.Text, LblEmessoMECA.Text, LblIncassatoMECA.Text, LblEmessoAggio.Text, LblIncassatoAggio.Text, LblEmessoProvEnte.Text, LblIncassatoProvEnte.Text, LblEmessoProv.Text, LblIncassatoProv.Text, LblEmessoTot.Text, LblIncassatoTot.Text, LblEmessoMagg.Text, LblIncassatoMagg.Text, LblInsolutoStat.Text, LblPercentualeInsolutoStat.Text, LblEmessoImposta.Text, LblIncassatoImposta.Text)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.CmdStampa_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
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
    ''' <summary>
    ''' Pulsante per la visualizzazione del raffronto in formato grafico a torta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdGrafico_Click(sender As Object, e As EventArgs) Handles CmdGrafico.Click
        Try
            Dim sScript As String = ""
            'Load the Visualization API And the corechart package.
            sScript += "google.charts.load('current', { 'packages': ['corechart']});"

            'Set a callback to run when the Google Visualization API Is loaded.
            sScript += "google.charts.setOnLoadCallback(drawChart);"

            'Callback that creates And populates a data table,
            'instantiates the pie chart, passes in the data And
            'draws it.
            sScript += "function drawChart()"
            sScript += "{"
            'Create the data table.
            sScript += "var data = new google.visualization.arrayToDataTable(["
            sScript += "['', '']"
            sScript += ", ['Incassato', " + TxtIncassato.Text.Replace(".", "").Replace(",", ".") + "]"
            sScript += ", ['Insoluto', " + TxtInsoluto.Text.Replace(".", "").Replace(",", ".") + "]"
            sScript += ", ['Incassato Stat.', " + TxtIncassatoStat.Text.Replace(".", "").Replace(",", ".") + "]"
            sScript += ", ['Insoluto Stat.', " + TxtInsolutoStat.Text.Replace(".", "").Replace(",", ".") + "]"
            sScript += "]);"
            'Set chart options
            sScript += "var options = { 'title': ''"
            sScript += ",is3D: true,"
            sScript += "};"

            'Instantiate And draw our chart, passing in some options.
            sScript += "var chart = new google.visualization.PieChart(document.getElementById('chart_div'));"
            sScript += "chart.draw(data, options);"
            sScript += "}"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.CmdGrafico_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che estrae i dati generali del fatturato in base ai criteri impostati. I dati ottenuti vengono caricati nella pagina.
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyEnte"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="AccreditoDal"></param>
    ''' <param name="AccreditoAl"></param>
    ''' <returns></returns>
    Private Function LoadRiepilogoEmesso(ByVal IsFromVariabile As String, ByVal sMyEnte As String, ByVal TipoRuolo As String, ByVal Anno As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim x As Integer
        Dim FncAnalisi As New ClsAnalisi

        Try
            'azzero le text
            LblNUtenti.Text = "0"
            LblNArticoli.Text = "0" : LblTotImpRuolo.Text = FormatNumber("0", 2)
            LblNAvvisi.Text = "0" : LblTotImpAvvisi.Text = FormatNumber("0", 2)
            LblNPagTot.Text = "0" : LblImpPagTot.Text = FormatNumber("0", 2)
            LblNPagParz.Text = "0" : LblImpPagParz.Text = FormatNumber("0", 2)
            LblInsoluto.Text = FormatNumber("0", 2) : LblPercentualeInsoluto.Text = FormatNumber("0", 2)
            'prelevo i dati dal db dell'emesso
            dvMyDati = FncAnalisi.GetRiepilogoEmesso(IsFromVariabile, sMyEnte, Anno, TipoRuolo, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    If Not IsDBNull(dvMyDati.Item(x)("nutenti")) Then
                        LblNUtenti.Text = FormatNumber(dvMyDati.Item(x)("nutenti"), 0)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("narticoli")) Then
                        LblNArticoli.Text = FormatNumber(dvMyDati.Item(x)("narticoli"), 0)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("imparticoli")) Then
                        LblTotImpRuolo.Text = FormatNumber(dvMyDati.Item(x)("imparticoli"), 2)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("navvisi")) Then
                        LblNAvvisi.Text = FormatNumber(dvMyDati.Item(x)("navvisi"), 0)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("impavvisi")) Then
                        LblTotImpAvvisi.Text = FormatNumber(dvMyDati.Item(x)("impavvisi"), 2)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("nevasetot")) Then
                        LblNPagTot.Text = FormatNumber(dvMyDati.Item(x)("nevasetot"), 0)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("evasetot")) Then
                        LblImpPagTot.Text = FormatNumber(dvMyDati.Item(x)("evasetot"), 2)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("nevasepar")) Then
                        LblNPagParz.Text = FormatNumber(dvMyDati.Item(x)("nevasepar"), 0)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("evasepar")) Then
                        LblImpPagParz.Text = FormatNumber(dvMyDati.Item(x)("evasepar"), 2)
                    End If

                    If Not IsDBNull(dvMyDati.Item(x)("emessostat")) Then
                        LblEmessoStat.Text = FormatNumber(dvMyDati.Item(x)("emessostat"), 2)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("incassatostat")) Then
                        LblIncassatoStat.Text = FormatNumber(dvMyDati.Item(x)("incassatostat"), 2)
                    End If
                Next
            End If
            LblTotIncassato.Text = FormatNumber((CDbl(LblImpPagParz.Text) + CDbl(LblImpPagTot.Text)), 2)
            LblEmesso.Text = LblTotImpAvvisi.Text : LblIncassato.Text = LblTotIncassato.Text
            LblInsoluto.Text = FormatNumber(CDbl(LblTotImpAvvisi.Text) - (CDbl(LblTotIncassato.Text)), 2)
            LblPercentualeInsoluto.Text = FormatNumber(((CDbl(LblInsoluto.Text) * 100) / CDbl(LblTotImpAvvisi.Text)), 2)
            TxtIncassato.Text = LblTotIncassato.Text : TxtInsoluto.Text = LblInsoluto.Text
            If LblEmessoStat.Text = "" Then
                LblEmessoStat.Text = "0"
            End If
            If LblIncassatoStat.Text = "" Then
                LblIncassatoStat.Text = "0"
            End If
            If LblInsolutoStat.Text = "" Then
                LblInsolutoStat.Text = "0"
            End If
            LblInsolutoStat.Text = FormatNumber(CDbl(LblEmessoStat.Text) - (CDbl(LblIncassatoStat.Text)), 2)
            LblPercentualeInsolutoStat.Text = FormatNumber(((CDbl(LblInsolutoStat.Text) * 100) / CDbl(LblEmessoStat.Text)), 2)
            TxtIncassatoStat.Text = LblIncassatoStat.Text : TxtInsolutoStat.Text = LblInsolutoStat.Text

            Return 1
        Catch err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.LoadRiepilogoEmesso.errore: ", err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Funzione che estrae il dettaglio del fatturato in base ai criteri impostati. I dati ottenuti vengono caricati nella pagina.
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyEnte"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="AccreditoDal"></param>
    ''' <param name="AccreditoAl"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioEmesso(ByVal IsFromVariabile As String, ByVal sMyEnte As String, ByVal TipoRuolo As String, ByVal Anno As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim x As Integer
        Dim Imposta, Sanzioni, Interessi, Eca, Meca, Aggio, Prov, ProvEnte, Arrotondamento, impStat, impConf As Double
        Dim FncAnalisi As New ClsAnalisi

        Try
            'azzero le text
            LblEmessoImposta.Text = FormatNumber(0, 2) : LblEmessoSanzioni.Text = FormatNumber(0, 2) : LblEmessoInteressi.Text = FormatNumber(0, 2)
            LblEmessoConferimenti.Text = FormatNumber(0, 2) : LblIncassatoConferimenti.Text = FormatNumber(0, 2)
            LblEmessoRuolo.Text = FormatNumber(0, 2)
            LblEmessoECA.Text = FormatNumber(0, 2) : LblEmessoMECA.Text = FormatNumber(0, 2)
            LblEmessoAggio.Text = FormatNumber(0, 2) : LblEmessoProvEnte.Text = FormatNumber(0, 2) : LblEmessoProv.Text = FormatNumber(0, 2)
            LblEmessoTot.Text = FormatNumber(0, 2)
            LblEmessoMagg.Text = FormatNumber(0, 2)
            'prelevo i dati dal db dell'emesso
            dvMyDati = FncAnalisi.GetDettaglioEmesso(IsFromVariabile, sMyEnte, TipoRuolo, Anno, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
                        Case "0000"
                            Select Case dvMyDati.Item(x)("codice_voce").ToString()
                                Case "1"
                                    Imposta += dvMyDati.Item(x)("importo")
                                Case "2"
                                    Interessi += dvMyDati.Item(x)("importo")
                                Case "8"
                                    Sanzioni += dvMyDati.Item(x)("importo")
                                Case Else
                                    Imposta += dvMyDati.Item(x)("importo")
                            End Select
                        Case "9986"
                            Eca += dvMyDati.Item(x)("importo")
                        Case "9987"
                            Meca += dvMyDati.Item(x)("importo")
                        Case "0094"
                            ProvEnte += dvMyDati.Item(x)("importo")
                        Case "0095"
                            Aggio += dvMyDati.Item(x)("importo")
                        Case "9994"
                            Prov += dvMyDati.Item(x)("importo")
                        Case "9999"
                            Arrotondamento += dvMyDati.Item(x)("importo")
                        Case "3955"
                            impStat += dvMyDati.Item(x)("importo")
                        Case "CONF"
                            impConf += dvMyDati.Item(x)("importo")
                    End Select
                Next
            End If

            LblEmessoImposta.Text = FormatNumber(Imposta + Arrotondamento, 2)
            LblEmessoSanzioni.Text = FormatNumber(Sanzioni, 2) : LblEmessoInteressi.Text = FormatNumber(Interessi, 2)
            If impConf <> 0 Then
                LblEmessoConferimenti.Text = FormatNumber(impConf, 2)
            Else
                LblEmessoConferimenti.Visible = False : Label38.Visible = False : Label70.Visible = False
            End If
            LblEmessoRuolo.Text = FormatNumber(Imposta + Sanzioni + Interessi + Arrotondamento, 2)
            LblEmessoECA.Text = FormatNumber(Eca, 2)
            LblEmessoMECA.Text = FormatNumber(Meca, 2)
            LblEmessoAggio.Text = FormatNumber(Aggio, 2)
            LblEmessoProvEnte.Text = FormatNumber(ProvEnte, 2)
            LblEmessoProv.Text = FormatNumber(Prov, 2)
            LblEmessoTot.Text = FormatNumber(Imposta + Sanzioni + Interessi + Eca + Meca + Aggio + ProvEnte + Prov + Arrotondamento, 2)
            LblEmessoMagg.Text = FormatNumber(impStat, 2)

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.LoadDettaglioEmesso.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Funzione che estrae il dettaglio dell'incassato in base ai criteri impostati. I dati ottenuti vengono caricati nella pagina.
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyEnte"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="AccreditoDal"></param>
    ''' <param name="AccreditoAl"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioIncassato(ByVal IsFromVariabile As String, ByVal sMyEnte As String, ByVal TipoRuolo As String, ByVal Anno As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim x As Integer
        Dim Imposta, Sanzioni, Interessi, Eca, Meca, Aggio, Prov, ProvEnte, Arrotondamento, impStat, impConf As Double
        Dim FncAnalisi As New ClsAnalisi

        Try
            'azzero le text
            LblIncassatoImposta.Text = FormatNumber(0, 2) : LblIncassatoSanzioni.Text = FormatNumber(0, 2) : LblIncassatoInteressi.Text = FormatNumber(0, 2)
            LblIncassatoConferimenti.Text = FormatNumber(0, 2)
            LblIncassatoRuolo.Text = FormatNumber(0, 2)
            LblIncassatoECA.Text = FormatNumber(0, 2) : LblIncassatoMECA.Text = FormatNumber(0, 2)
            LblIncassatoAggio.Text = FormatNumber(0, 2) : LblIncassatoProvEnte.Text = FormatNumber(0, 2) : LblIncassatoProv.Text = FormatNumber(0, 2)
            LblIncassatoTot.Text = FormatNumber(0, 2)
            LblIncassatoMagg.Text = FormatNumber(0, 2)
            'prelevo i dati dal db dell'Incassato
            dvMyDati = FncAnalisi.GetDettaglioIncassato(IsFromVariabile, sMyEnte, TipoRuolo, Anno, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
                        Case "0000"
                            Select Case dvMyDati.Item(x)("codice_voce").ToString()
                                Case "1"
                                    Imposta += dvMyDati.Item(x)("importo")
                                Case "2"
                                    Interessi += dvMyDati.Item(x)("importo")
                                Case "8"
                                    Sanzioni += dvMyDati.Item(x)("importo")
                                Case Else
                                    Imposta += dvMyDati.Item(x)("importo")
                            End Select
                        Case "9986"
                            Eca += dvMyDati.Item(x)("importo")
                        Case "9987"
                            Meca += dvMyDati.Item(x)("importo")
                        Case "0094"
                            ProvEnte += dvMyDati.Item(x)("importo")
                        Case "0095"
                            Aggio += dvMyDati.Item(x)("importo")
                        Case "9994"
                            Prov += dvMyDati.Item(x)("importo")
                        Case "9999"
                            Arrotondamento += dvMyDati.Item(x)("importo")
                        Case "3955"
                            impStat += dvMyDati.Item(x)("importo")
                        Case "CONF"
                            impConf += dvMyDati.Item(x)("importo")
                    End Select
                Next
            End If

            LblIncassatoImposta.Text = FormatNumber(Imposta + Arrotondamento, 2)
            LblIncassatoSanzioni.Text = FormatNumber(Sanzioni, 2) : LblIncassatoInteressi.Text = FormatNumber(Interessi, 2)
            If impConf <> 0 Then
                LblIncassatoConferimenti.Text = FormatNumber(impConf, 2)
            Else
                LblIncassatoConferimenti.Visible = False : Label71.Visible = False : Label73.Visible = False
            End If
            LblIncassatoRuolo.Text = FormatNumber(Imposta + Sanzioni + Interessi + Arrotondamento, 2)
            LblIncassatoECA.Text = FormatNumber(Eca, 2)
            LblIncassatoMECA.Text = FormatNumber(Meca, 2)
            LblIncassatoAggio.Text = FormatNumber(Aggio, 2)
            LblIncassatoProvEnte.Text = FormatNumber(ProvEnte, 2)
            LblIncassatoProv.Text = FormatNumber(Prov, 2)
            LblIncassatoTot.Text = FormatNumber(Imposta + Sanzioni + Interessi + Eca + Meca + Aggio + ProvEnte + Prov + Arrotondamento, 2)
            LblIncassatoMagg.Text = FormatNumber(impStat, 2)

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.LoadDettaglioIncassato.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Funzione che estrae il dettaglio del fatturato per Categoria in base ai criteri impostati. I dati ottenuti vengono caricati nella pagina.
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyEnte"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="AccreditoDal"></param>
    ''' <param name="AccreditoAl"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioEmessoVSCat(ByVal IsFromVariabile As String, ByVal sMyEnte As String, ByVal TipoRuolo As String, ByVal Anno As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim FncAnalisi As New ClsAnalisi

        Try
            'prelevo i dati dal db dell'emesso
            dvMyDati = FncAnalisi.GetEmessoImpostaVSCategoria(IsFromVariabile, sMyEnte, TipoRuolo, Anno, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                'popolo la griglia
                GrdEmessoDettCategoria.DataSource = dvMyDati
                GrdEmessoDettCategoria.DataBind()
                Session("dvEmessoVSCat") = dvMyDati
            End If

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.LoadDettaglioEmessoVSCat.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Funzione che estrae il dettaglio dell'incassato per Categoria in base ai criteri impostati. I dati ottenuti vengono caricati nella pagina.
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyEnte"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="AccreditoDal"></param>
    ''' <param name="AccreditoAl"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioIncassatoVSCat(ByVal IsFromVariabile As String, ByVal sMyEnte As String, ByVal TipoRuolo As String, ByVal Anno As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim FncAnalisi As New ClsAnalisi

        Try
            'prelevo i dati dal db dell'Incassato
            dvMyDati = FncAnalisi.GetIncassatoImpostaVSCategoria(IsFromVariabile, sMyEnte, TipoRuolo, Anno, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                'popolo la griglia
                GrdIncassatoDettCategoria.DataSource = dvMyDati
                GrdIncassatoDettCategoria.DataBind()
                Session("dvIncassatoVSCat") = dvMyDati
            End If

            Return 1
        Catch Err As Exception
Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AnalisiEconomiche.LoadDettaglioIncassatoVSCat.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function
End Class