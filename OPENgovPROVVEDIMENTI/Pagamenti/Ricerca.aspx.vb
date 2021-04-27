Imports log4net
''' <summary>
''' Pagina per la gestione ricerca di accorpamenti/rateizzazioni/pagamenti.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class Ricerca
    Inherits BaseEnte

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
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(Ricerca))

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim objDS As DataSet

        Try
            Session("oAnagrafe") = Nothing
            optAccorpamenti.Attributes.Add("onclick", "dataaccredito(true)")
            optPagamenti.Attributes.Add("onclick", "dataaccredito(false)")
            txtCognome.Attributes.Add("onkeydown", "keyPress();")
            txtNome.Attributes.Add("onkeydown", "keyPress();")
            txtCodFisc.Attributes.Add("onkeydown", "keyPress();")
            txtPiva.Attributes.Add("onkeydown", "keyPress();")
            txtDataAl.Attributes.Add("onkeydown", "keyPress();")
            txtDataDal.Attributes.Add("onkeydown", "keyPress();")
            txtNumAtto.Attributes.Add("onkeydown", "keyPress();")
            If Page.IsPostBack Then
                If optAccorpamenti.Checked Then
                    RegisterScript("dataaccredito(true)", Me.GetType())
                End If
                If optPagamenti.Checked Then
                    RegisterScript("dataaccredito(false)", Me.GetType())
                End If

                If Not IsNothing(Session("grdAccorpamenti")) Then
                    objDS = CType(Session("grdAccorpamenti"), DataSet)
                    GrdAccorpamenti.DataSource = objDS.Tables(0).DefaultView
                End If
                If Not IsNothing(Session("grdPagamenti")) Then
                    objDS = CType(Session("grdPagamenti"), DataSet)
                    GrdPagamenti.DataSource = objDS.Tables(0).DefaultView
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "Pagamenti", Utility.Costanti.AZIONE_LETTURA.ToString(), Utility.Costanti.TRIBUTO_accertaMENTO, ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Dim objPagamenti As clsPagamenti
        Dim objDS As DataSet
        Dim cognome, nome, cod_fiscale, partita_iva, NumAtto As String
        Dim DataAl, DataDal As DateTime
        Try
            DataDal = DateTime.MaxValue : DataAl = DateTime.MaxValue

            Session("grdPagamenti") = Nothing
            Session("grdAccorpamenti") = Nothing
            objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            cognome = txtCognome.Text
            nome = txtNome.Text
            cod_fiscale = txtCodFisc.Text
            partita_iva = txtPiva.Text
            NumAtto = txtNumAtto.Text
            If txtDataAl.Text <> "" Then
                DataAl = CDate(txtDataAl.Text)
            End If
            If txtDataDal.Text <> "" Then
                DataDal = CDate(txtDataDal.Text)
            End If

            If optAccorpamenti.Checked Then
                'ricerca accorpamenti
                objDS = objPagamenti.getAccorpamenti(ConstSession.IdEnte, cognome, nome, cod_fiscale, partita_iva)
                Session("grdAccorpamenti") = objDS
                If Not IsNothing(objDS) Then
                    If objDS.Tables(0).Rows.Count > 0 Then
                        GrdAccorpamenti.SelectedIndex = -1
                        GrdAccorpamenti.DataSource = objDS.Tables(0).DefaultView
                        GrdAccorpamenti.DataBind()
                        lblInfo.Text = GetTotalizzatori(objDS.Tables(0).DefaultView, "A")
                        lblInfo.Visible = True
                        GrdAccorpamenti.Visible = True
                    Else
                        lblInfo.Text = "Nessun Accorpamento/Rateizzazione trovato"
                        lblInfo.Visible = True
                        GrdAccorpamenti.Visible = False
                    End If
                Else
                    lblInfo.Text = "Nessun Accorpamento/Rateizzazione trovato"
                    lblInfo.Visible = True
                    GrdAccorpamenti.Visible = False
                End If
            End If
            If optPagamenti.Checked Then
                'ricerca pagamenti
                objDS = objPagamenti.getPagamenti(ConstSession.IdEnte, cognome, nome, cod_fiscale, partita_iva, -1, NumAtto, DataDal, DataAl)
                Session("grdPagamenti") = objDS
                If Not IsNothing(objDS) Then
                    If objDS.Tables(0).Rows.Count > 0 Then
                        GrdPagamenti.SelectedIndex = -1
                        GrdPagamenti.DataSource = objDS.Tables(0).DefaultView
                        GrdPagamenti.DataBind()
                        lblInfo.Text = GetTotalizzatori(objDS.Tables(0).DefaultView, "P")
                        lblInfo.Visible = True
                        GrdPagamenti.Visible = True
                    Else
                        lblInfo.Text = "Nessun Pagamento trovato"
                        lblInfo.Visible = True
                        GrdPagamenti.Visible = False
                    End If
                Else
                    lblInfo.Text = "Nessun Pagamento trovato"
                    lblInfo.Visible = True
                    GrdPagamenti.Visible = False
                End If

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.btnRicerca_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            objPagamenti.kill()
        End Try
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
                e.Row.Attributes.Add("title", "Visualizza il dettaglio dell'accorpamento")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.GrdRowDataBound.errore: ", ex)
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
            If e.CommandName = "RowOpen" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdAccorpamenti" Then
                    For Each myRow As GridViewRow In GrdAccorpamenti.Rows
                        If IDRow = CType(myRow.FindControl("hfid_accorpamento"), HiddenField).Value Then
                            Dim id_accorpamento, cod_contribuente As Integer
                            Dim tipo As String
                            Dim detthashtable As New Hashtable
                            Try
                                Session.Remove("detthashtable")
                                id_accorpamento = IDRow
                                cod_contribuente = CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value
                                tipo = CType(myRow.FindControl("hfTIPO"), HiddenField).Value

                                detthashtable.Add("id_accorpamento", id_accorpamento)
                                detthashtable.Add("cod_contribuente", cod_contribuente)
                                detthashtable.Add("id_provvedimento", 0)
                                detthashtable.Add("tipo", tipo)
                                Session.Add("detthashtable", detthashtable)
                                RegisterScript("ApriDettaglio(" & cod_contribuente & "," & id_accorpamento & ")", Me.GetType())
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.GrdRowCommand.errore: ", ex)
                                Response.Redirect("../../PaginaErrore.aspx")
                            Finally
                                detthashtable = Nothing
                            End Try
                        End If
                    Next
                Else
                    For Each myRow As GridViewRow In GrdPagamenti.Rows
                        If IDRow = CType(myRow.FindControl("hfid_accorpamento"), HiddenField).Value Then
                            Dim id_provvedimento, cod_contribuente, id_accorpamento As Integer
                            Dim tipo As String
                            Dim detthashtable As New Hashtable
                            Try
                                Session.Remove("detthashtable")
                                id_provvedimento = CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value
                                id_accorpamento = IDRow
                                cod_contribuente = CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value
                                tipo = CType(myRow.FindControl("hftipo"), HiddenField).Value

                                detthashtable.Add("id_accorpamento", id_accorpamento)
                                detthashtable.Add("cod_contribuente", cod_contribuente)
                                detthashtable.Add("id_provvedimento", id_provvedimento)
                                detthashtable.Add("tipo", tipo)
                                Session.Add("detthashtable", detthashtable)
                                Dim sScript As String = ""
                                sScript = "parent.Comandi.location.href='cmdPagamenti.aspx?from=Dettaglio';"
                                sScript += "parent.Visualizza.location.href='Pagamenti.aspx?from=Dettaglio&NRataSel=" & CType(myRow.FindControl("hfnrata"), HiddenField).Value & "&IdPagamento=" & CType(myRow.FindControl("hfid_pagato"), HiddenField).Value & "&IdAccorpamento=" & IDRow & "&IdProvvedimento=" & CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value & "&Tipo=" & CType(myRow.FindControl("hftipo"), HiddenField).Value & "&IdContribuente=" & CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value & "';"
                                RegisterScript(sScript, Me.GetType())
                                'RegisterScript("ApriDettaglio()", Me.GetType())
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.GrdRowCommand.errore: ", ex)
                                Response.Redirect("../../PaginaErrore.aspx")
                            End Try
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(sender, e.NewPageIndex)
    End Sub
    'Private Sub grdAccorpamenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccorpamenti.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            e.Item.Attributes.Add("title", "Visualizza il dettaglio dell'accorpamento")
    '        End If
    '    Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.grdAccorpamenti_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub grdAccorpamenti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdAccorpamenti.SelectedIndexChanged
    '    Dim id_accorpamento, cod_contribuente As Integer
    '    Dim tipo As String
    '    Dim detthashtable As New Hashtable
    '    Try
    '        Session.Remove("detthashtable")
    '        id_accorpamento = GrdAccorpamenti.SelectedItem.Cells(grdAccorpamentiPosition.id_accorpamento).Text
    '        cod_contribuente = GrdAccorpamenti.SelectedItem.Cells(grdAccorpamentiPosition.cod_contribuente).Text
    '        tipo = GrdAccorpamenti.SelectedItem.Cells(grdAccorpamentiPosition.tipo).Text

    '        detthashtable.Add("id_accorpamento", id_accorpamento)
    '        detthashtable.Add("cod_contribuente", cod_contribuente)
    '        detthashtable.Add("id_provvedimento", 0)
    '        detthashtable.Add("tipo", tipo)
    '        Session.Add("detthashtable", detthashtable)
    '        RegisterScript("dettaglio", "<script language='javascript'>ApriDettaglio()</script>")

    '    Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.grdAccorpamenti_SelectedIndexChanged.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../Styles.css"))
    '        Response.End()
    '    Finally
    '        detthashtable = Nothing
    '    End Try
    'End Sub

    'Private Sub grdPagamenti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdPagamenti.SelectedIndexChanged
    '    Dim id_provvedimento, cod_contribuente, id_accorpamento As Integer
    '    Dim tipo As String
    '    Dim detthashtable As New Hashtable
    '    Try
    '        Session.Remove("detthashtable")
    '        id_provvedimento = GrdPagamenti.SelectedItem.Cells(grdPagamentiPosition.id_provvedimento).Text
    '        id_accorpamento = GrdPagamenti.SelectedItem.Cells(grdPagamentiPosition.id_accorpamento).Text
    '        cod_contribuente = GrdPagamenti.SelectedItem.Cells(grdPagamentiPosition.cod_contribuente).Text
    '        tipo = GrdPagamenti.SelectedItem.Cells(grdPagamentiPosition.tipo).Text

    '        detthashtable.Add("id_accorpamento", id_accorpamento)
    '        detthashtable.Add("cod_contribuente", cod_contribuente)
    '        detthashtable.Add("id_provvedimento", id_provvedimento)
    '        detthashtable.Add("tipo", tipo)
    '        Session.Add("detthashtable", detthashtable)
    '        RegisterScript("dettaglioPag", "<script language='javascript'>ApriDettaglio()</script>")

    '    Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.LoadSearch.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../Styles.css"))
    '        Response.End()
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(myGrd As Ribes.OPENgov.WebControls.RibesGridView, Optional ByVal page As Integer? = 0)
        Try
            If CType(myGrd, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdAccorpamenti" Then
                GrdAccorpamenti.DataSource = CType(Session("grdAccorpamenti"), DataSet)
                If page.HasValue Then
                    GrdAccorpamenti.PageIndex = page.Value
                End If
                GrdAccorpamenti.DataBind()
            Else
                GrdPagamenti.DataSource = CType(Session("grdPagamenti"), DataSet)
                If page.HasValue Then
                    GrdPagamenti.PageIndex = page.Value
                End If
                GrdPagamenti.DataBind()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
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
                If CStr(objtemp).CompareTo("") <> 0 Then
                    If CDate(objtemp).Date = DateTime.MinValue.Date Or CDate(objtemp).Date = DateTime.MaxValue.Date Then
                        strTemp = ""
                    Else
                        Dim MiaData As String = CType(objtemp, DateTime).ToString("yyyy/MM/dd")
                        strTemp = clsGeneralFunction.GiraDataFromDB(MiaData)
                    End If
                Else
                    strTemp = ""
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.annoBarra.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Protected Function GiraDataFromDB(ByVal data As Object) As String
        'leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Dim objUtility As New myUtility

        GiraDataFromDB = ""
        data = objUtility.CToStr(data)
        Try
            If Not IsDBNull(data) And Not IsNothing(data) Then

                If data <> "" Then
                    Giorno = Mid(data, 7, 2)
                    Mese = Mid(data, 5, 2)
                    Anno = Mid(data, 1, 4)
                    GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
                Else
                    GiraDataFromDB = ""
                End If

                If IsDate(GiraDataFromDB) = False And GiraDataFromDB <> "" Then
                    Giorno = Mid(data, 7, 2)
                    Mese = Mid(data, 5, 2)
                    Anno = Mid(data, 1, 4)
                    GiraDataFromDB = Mese & "/" & Giorno & "/" & Anno
                End If
            End If
            Return GiraDataFromDB
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.GiraDataFromDB.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objCF"></param>
    ''' <param name="objPIVA"></param>
    ''' <returns></returns>
    Protected Function CF_PIVA(ByVal objCF As Object, ByVal objPIVA As Object) As String
        Dim strTemp As String = ""
        Try
            If Not IsDBNull(objCF) Then
                If CStr(objCF).CompareTo("") <> 0 Then
                    strTemp = objCF.ToString()
                End If
            End If
            If CStr(strTemp).CompareTo("") = 0 Then
                If Not IsDBNull(objPIVA) Then
                    If CStr(objPIVA).CompareTo("") <> 0 Then
                        strTemp = objPIVA.ToString()
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.CF_PIVA.errore: ", ex)
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
    Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As String, ByVal numDec As Integer) As String
        FormattaNumero = ""
        Try
            If IsDBNull(NumeroDaFormattareParam) Or NumeroDaFormattareParam = "" Or NumeroDaFormattareParam = "-1" Or NumeroDaFormattareParam = "-1,00" Then
                NumeroDaFormattareParam = ""
            Else
                FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.FormattaNumeri.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub optAccorpamenti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optAccorpamenti.CheckedChanged
        grdAccorpamenti.Visible = False
        grdPagamenti.Visible = False
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub optPagamenti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPagamenti.CheckedChanged
        grdAccorpamenti.Visible = False
        grdPagamenti.Visible = False
    End Sub
    ''' <summary>
    ''' posizionamento delle colonne della griglia grdAccorpamenti
    ''' </summary>
    Enum grdAccorpamentiPosition
        '
        cognome = 0
        nome = 1
        cf_piva = 2
        sum_valore_rata = 3
        sum_valore_interesse = 4
        rate = 5
        sum_importo_pagato = 6
        id_accorpamento = 7
        cod_contribuente = 8
        tipo = 9
    End Enum
    ''' <summary>
    ''' posizionamento delle colonne della griglia grdPagamenti
    ''' </summary>
    Enum grdPagamentiPosition
        '
        cognome = 0
        nome = 1
        cf_piva = 2
        data_pagamento = 3
        importo_pagato = 4
        provenienza = 5
        id_provvedimento = 6
        id_accorpamento = 7
        cod_contribuente = 8
        tipo = 9
    End Enum
    ''' <summary>
    ''' Funzione che ciclando sul recordset in ingresso totalizza gl iimporti ed il numero di posizioni
    ''' </summary>
    ''' <param name="myView">DataView recordset con i dati</param>
    ''' <param name="myType">String può assumere A in caso di accorpamento, P in caso di pagamento</param>
    ''' <returns>String testo da visualizzare nella label</returns>
    Private Function GetTotalizzatori(myView As DataView, myType As String) As String
        Dim nPosizioni As Integer = 0
        Dim impTotale As Double = 0
        Try
            For Each myRow As DataRow In myView.Table.Rows
                nPosizioni += 1
                If myType = "A" Then
                    impTotale += CDbl(myRow("sum_valore_rata"))
                Else
                    impTotale += CDbl(myRow("importo_pagato"))
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Ricerca.GetTotalizzatori.errore: ", ex)
            nPosizioni = 0 : impTotale = 0
        End Try
        Return nPosizioni.ToString + " posizioni per € " + impTotale.ToString("#,##0.00")
    End Function
End Class

