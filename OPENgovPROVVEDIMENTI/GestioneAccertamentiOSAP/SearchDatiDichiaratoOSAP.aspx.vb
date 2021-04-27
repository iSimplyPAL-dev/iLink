Imports log4net
Imports ComPlusInterface
''' <summary>
''' Pagina per la generazione dei provvedimenti OSAP.
''' Contiene le funzioni della comandiera e la griglia per la gestione del dichiarato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class SearchDatiAccertamentiOSAP
	Inherits BasePage
	Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchDatiAccertamentiOSAP))
    Protected FncForGrd As New Formatta.SharedGrd

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
		Dim objHashTable As Hashtable
		Dim ListDichiarato() As OSAPAccertamentoArticolo
		Dim MyDichiarato As OSAPAccertamentoArticolo
		Dim FncAcc As New ClsGestioneAccertamenti
		Dim ListCateg() As IRemInterfaceOSAP.Categorie
		Dim ListTipiOcc() As IRemInterfaceOSAP.TipologieOccupazioni
		Dim ListAgevolazioni() As IRemInterfaceOSAP.Agevolazione
		Dim ListTariffe() As IRemInterfaceOSAP.Tariffe
		Dim MyMotore As IRemInterfaceOSAP.IRemotingInterfaceOSAP
		Dim ResultCalcolato As IRemInterfaceOSAP.CalcoloResult
		Dim TipoRuolo As IRemInterfaceOSAP.Ruolo.E_TIPO = IRemInterfaceOSAP.Ruolo.E_TIPO.ORDINARIO
		Dim x As Integer
		Dim nList As Integer = -1
		Dim ListIdDichToExam() As Integer
		Dim Dichiarazione As IRemInterfaceOSAP.DichiarazioneTosapCosap
        'Dim MyDBEngine As DAL.DBEngine
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim sScript As String = ""

        Try
			'Viene Salvata da GestioneAccertamenti.aspx.vb
			'per avere il codice contribuente e anno di accertamento
			objHashTable = Session("HashTableRettificaAccertamenti")

			If Not Page.IsPostBack Then
				If Not Session("DataSetDichiarazioni") Is Nothing Then
					ListDichiarato = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())
                    GrdDichiarato.DataSource = ListDichiarato
                    GrdDichiarato.DataBind()
                Else
                    'MyDBEngine = DAO.DBEngineFactory.GetDBEngine(ConfigurationManager.AppSettings("connectionStringOPENgovOSAP"))
                    'MyDBEngine.OpenConnection()
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionOSAP)
                    cmdMyCommand.Connection.Open()

                    'carico i parametri per il calcolo
                    FncAcc.OSAPLoadParamCalcolo(CInt(objHashTable("ANNOACCERTAMENTO")), ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe, ConstSession.IdEnte)

                    SetHashTable(objHashTable, ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe)
                    'attivo il servizio
                    MyMotore = CType(Activator.GetObject(GetType(IRemInterfaceOSAP.IRemotingInterfaceOSAP), ConstSession.urlMotoreOSAP), IRemInterfaceOSAP.IRemotingInterfaceOSAP)

                    'ListIdDichToExam = DTO.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno(CInt(objHashTable("ANNOACCERTAMENTO")), constsession.idente, objHashTable("CODCONTRIBUENTE"), MyDBEngine)
                    ListIdDichToExam = DTO.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno(CInt(objHashTable("ANNOACCERTAMENTO")), ConstSession.IdEnte, Utility.Costanti.TRIBUTO_OSAP, objHashTable("CODCONTRIBUENTE"), -1, "", cmdMyCommand)
                    If ListIdDichToExam.Length > 0 Then
                        For x = 0 To ListIdDichToExam.GetUpperBound(0)
                            ' Ottengo la dichiarazione con tutti gli articoli
                            'Dichiarazione = DTO.MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore(ListIdDichToExam(x), constsession.idente, Costanti.TRIBUTO_OSAP, CInt(objHashTable("ANNOACCERTAMENTO")), MyDBEngine)
                            Dichiarazione = DTO.MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore(ListIdDichToExam(x), ConstSession.IdEnte, Utility.Costanti.TRIBUTO_OSAP, CInt(objHashTable("ANNOACCERTAMENTO")), "", cmdMyCommand)
                            ' Scorro tutti gli articoli della dichiarazione e memorizzo i ruoli
                            For Each MyUI As IRemInterfaceOSAP.Articolo In Dichiarazione.ArticoliDichiarazione
                                nList += 1 : MyDichiarato = New OSAPAccertamentoArticolo
                                ResultCalcolato = MyMotore.CalcolaOSAP(TipoRuolo, MyUI, ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe, Nothing)
                                If (ResultCalcolato.Result <> IRemInterfaceOSAP.E_CALCOLORESULT.OK) Then
                                    Response.Redirect("../../PaginaErrore.aspx")
                                Else
                                    FncAcc.OSAPCastArtIntoProvArt(MyUI, MyDichiarato, objHashTable("CODCONTRIBUENTE"))
                                    MyDichiarato.Anno = CStr(objHashTable("ANNOACCERTAMENTO"))
                                    MyDichiarato.Calcolo = ResultCalcolato
                                    'carico il progressivo della griglia
                                    MyDichiarato.Progressivo = nList + 1
                                    MyDichiarato.IdLegame = MyDichiarato.Progressivo
                                End If
                                ReDim Preserve ListDichiarato(nList)
                                ListDichiarato(nList) = MyDichiarato
                            Next
                        Next
                    End If
                    'Svuota la Session con la dichirazione precedente
                    Session.Remove("DataSetDichiarazioni")
                    GrdDichiarato.DataSource = ListDichiarato
                    GrdDichiarato.DataBind()
                    Session("DataSetDichiarazioni") = ListDichiarato

                    If Session("ESCLUDI_PREACCERTAMENTO") Then
                        Session("DataSetDichiarazioni") = Nothing
                    End If

                    Select Case CInt(GrdDichiarato.Rows.Count)
                        Case 0
                            GrdDichiarato.Visible = False
                            lblMessage.Text = "Non sono presenti Immobili Dichiarati"
                        Case Is > 0
                            GrdDichiarato.Visible = True
                            If Session("ESCLUDI_PREACCERTAMENTO") Then
                                lblMessage.Text = "Gli immobili dichiarati non verranno presi in considerazione per il calcolo di questo accertamento in quanto già calcolati in altro accertamento definitivo"
                                lblTesto.Visible = False
                                chkSelTutti.Visible = False
                                sscript += "parent.parent.Comandi.document.getElementById('btnRibaltaImmobile').style.display='none';"
                                RegisterScript(sScript, Me.GetType())
                            Else
                                lblMessage.Visible = False
                                chkSelTutti.Visible = True
                                lblTesto.Visible = True
                            End If

                    End Select
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
            sScript += "parent.document.getElementById('loadGridDichiarato').style.display='' ;"
            RegisterScript(sScript, Me.GetType())
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
            Select Case e.CommandName
                Case "RowEdit"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            GrdDichiarato.EditIndex = myRow.RowIndex
                            GrdDichiarato.DataSource = Session("DataSetDichiarazioni")
                            GrdDichiarato.DataBind()
                        End If
                    Next
                Case "RowUpdate"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            Dim oAccertamentoUpdate() As OSAPAccertamentoArticolo
                            Dim intArticoliUpdate As Integer

                            'Prendo l'idLegame
                            Dim IdLegameGrid As TextBox = myRow.FindControl("txtLegame")
                            Dim IdGridUpdate As String = IDRow

                            If IdLegameGrid.Text = "" Then
                                Dim sScript As String = ""
                                sScript += "msgLegameVuoto();" & vbCrLf
                                RegisterScript(sScript, Me.GetType())
                            Else
                                oAccertamentoUpdate = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())

                                For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
                                    If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
                                        oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
                                    End If
                                Next
                                GrdDichiarato.EditIndex = -1
                                GrdDichiarato.DataSource = oAccertamentoUpdate
                                GrdDichiarato.DataBind()
                                Session("DataSetDichiarazioni") = oAccertamentoUpdate
                            End If
                        End If
                    Next
                Case "RowDelete"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            Dim oAccertamentoDelete() As OSAPAccertamentoArticolo
                            Dim oAccertamentoReturn() As OSAPAccertamentoArticolo

                            Dim intArticoliDelete As Integer
                            Dim intArticoliReturn As Integer

                            Dim IdGridDelete As String = IDRow

                            oAccertamentoDelete = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())

                            intArticoliReturn = -1
                            If Not oAccertamentoDelete Is Nothing Then
                                For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
                                    If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
                                        intArticoliReturn += 1
                                        ReDim Preserve oAccertamentoReturn(intArticoliReturn)
                                        oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
                                    End If
                                Next

                                If Not oAccertamentoReturn Is Nothing Then
                                    GrdDichiarato.EditIndex = -1
                                    GrdDichiarato.DataSource = oAccertamentoReturn
                                    GrdDichiarato.DataBind()
                                Else
                                    GrdDichiarato.EditIndex = -1
                                    GrdDichiarato.DataSource = Nothing
                                    GrdDichiarato.DataBind()
                                    GrdDichiarato.Style.Add("display", "none")
                                End If
                            Else
                                GrdDichiarato.EditIndex = -1
                                GrdDichiarato.DataSource = Nothing
                                GrdDichiarato.DataBind()
                                GrdDichiarato.Style.Add("display", "none")
                            End If
                            Session("DataSetDichiarazioni") = oAccertamentoReturn
                            Log.Debug("lavorerò su " & oAccertamentoReturn.Length.ToString & " posizione dichiarate")
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdDichiarato_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.EditCommand
    '    GrdDichiarato.EditItemIndex = e.Item.ItemIndex
    '    GrdDichiarato.DataSource = Session("DataSetDichiarazioni")
    '    GrdDichiarato.DataBind()
    'End Sub


    'Protected Sub GrdDichiarato_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.UpdateCommand
    '    Dim oAccertamentoUpdate() As OSAPAccertamentoArticolo
    '    Dim oAccertamentoReturn() As OSAPAccertamentoArticolo
    '    Dim intArticoliUpdate As Integer

    '    'Prendo l'idLegame
    '    Dim IdLegameGrid As TextBox = e.Item.Cells(0).FindControl("txtLegame")
    '    Dim IdGridUpdate As String = e.Item.Cells(0).Text
    'Try
    '    If IdLegameGrid.Text = "" Then
    '        dim sScript as string=""
    '        sscript+= "msgLegameVuoto();" & vbCrLf
    '        RegisterScript(sScript , Me.GetType())
    '    Else
    '        oAccertamentoUpdate = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())

    '        Dim arrayListImmobili As New ArrayList
    '        Dim oAccertamentoSingolo As OSAPAccertamentoArticolo
    '        oAccertamentoSingolo = New OSAPAccertamentoArticolo

    '        For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
    '            If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
    '                oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
    '            End If
    '        Next
    '        GrdDichiarato.EditItemIndex = -1
    '        GrdDichiarato.DataSource = oAccertamentoUpdate
    '        GrdDichiarato.DataBind()
    '        Session("DataSetDichiarazioni") = oAccertamentoUpdate
    '    End If
    '  Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.GrdDichiarato_Update.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Protected Sub GrdDichiarato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.DeleteCommand
    '    Dim oAccertamentoDelete() As OSAPAccertamentoArticolo
    '    Dim oAccertamentoReturn() As OSAPAccertamentoArticolo

    '    Dim intArticoliDelete As Integer
    '    Dim intArticoliReturn As Integer

    '    Dim IdGridDelete As String = e.Item.Cells(0).Text

    '    oAccertamentoDelete = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())

    '    intArticoliReturn = -1
    'Try
    '    If Not oAccertamentoDelete Is Nothing Then
    '        For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
    '            If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
    '                intArticoliReturn += 1
    '                ReDim Preserve oAccertamentoReturn(intArticoliReturn)
    '                oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
    '            End If
    '        Next

    '        If Not oAccertamentoReturn Is Nothing Then
    '            GrdDichiarato.EditItemIndex = -1
    '            GrdDichiarato.DataSource = oAccertamentoReturn
    '            GrdDichiarato.DataBind()
    '            Session("DataSetDichiarazioni") = oAccertamentoReturn
    '        Else
    '            GrdDichiarato.EditItemIndex = -1
    '            GrdDichiarato.DataSource = Nothing
    '            GrdDichiarato.DataBind()
    '            GrdDichiarato.Style.Add("display", "none")
    '            Session("DataSetDichiarazioni") = Nothing
    '        End If
    '    Else
    '        GrdDichiarato.EditItemIndex = -1
    '        GrdDichiarato.DataSource = Nothing
    '        GrdDichiarato.DataBind()
    '        GrdDichiarato.Style.Add("display", "none")
    '        Session("DataSetDichiarazioni") = Nothing
    '    End If
    '  Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.GrdDichiarato_DeleteCommand.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkSelTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelTutti.CheckedChanged

        Try
            If chkSelTutti.Checked = True Then
                '** ciclare la griglia e selezionare tutti gli immobili
                For Each myRow As GridViewRow In GrdDichiarato.Rows
                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True
                Next
            Else
                '** nessuna selezione -> selezione manuale dell'operatore
                For Each myRow As GridViewRow In GrdDichiarato.Rows
                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = False
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.chkSelTutti_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibaltaImmobiliAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaImmobiliAcc.Click
        Try
            Dim y As Integer
            Dim sScript As String
            Dim sIDSel As String
            Dim oListDaAccertare() As OSAPAccertamentoArticolo
            Dim oListAccertato() As OSAPAccertamentoArticolo

            oListDaAccertare = Session("DataSetDichiarazioni")

            For Each myRow As GridViewRow In GrdDichiarato.Rows
                If CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True Then
                    sIDSel = myRow.Cells(0).Text
                    For y = 0 To oListDaAccertare.Length - 1
                        If oListDaAccertare(y).Progressivo = sIDSel Then
                            ReDim Preserve oListAccertato(y)
                            oListDaAccertare(y).Calcola_Interessi = True
                            oListAccertato(y) = oListDaAccertare(y)
                            Exit For
                        End If
                    Next
                End If
            Next

            Session("oAccertatiGriglia") = oListAccertato
            If Not IsNothing(oListAccertato) Then
                sScript = "parent.document.getElementById('loadGridAccertato').src='SearchDatiAccertatoOSAP.aspx';"
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare almeno un immobile per il ribaltamento!');"
            End If
            RegisterScript(sScript, Me.GetType())

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.btnRibaltaImmobiliAcc_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objHashTable"></param>
    ''' <param name="ListCateg"></param>
    ''' <param name="ListTipiOcc"></param>
    ''' <param name="ListAgevolazioni"></param>
    ''' <param name="ListTariffe"></param>
    Private Sub SetHashTable(ByRef objHashTable As Hashtable, ByVal ListCateg() As IRemInterfaceOSAP.Categorie, ByVal ListTipiOcc() As IRemInterfaceOSAP.TipologieOccupazioni, ByVal ListAgevolazioni() As IRemInterfaceOSAP.Agevolazione, ByVal ListTariffe() As IRemInterfaceOSAP.Tariffe)
        'Dim strConnectionStringOPENgovProvvedimenti As String
        'Dim strConnectionStringOPENgovOSAP As String

        Try
            'Recupero la hash table
            If objHashTable.ContainsKey("IDSOTTOAPPLICAZIONEICI") = False Then
                objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVTA"))
            Else
                objHashTable("IDSOTTOAPPLICAZIONEICI") = ConfigurationManager.AppSettings("OPENGOVTA")
            End If
            'strConnectionStringOPENgovProvvedimenti = ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI")
            'strConnectionStringOPENgovOSAP = ConfigurationManager.AppSettings("connectionStringOPENgovOSAP")

            'Aggiungo gli altri campi nella hash table
            If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = False Then
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            Else
                objHashTable("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = ConstSession.StringConnection
            End If
            If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVOSAP") = False Then
                objHashTable.Add("CONNECTIONSTRINGOPENGOVOSAP", ConstSession.StringConnectionOSAP)
            Else
                objHashTable("CONNECTIONSTRINGOPENGOVOSAP") = ConstSession.StringConnectionOSAP
            End If
            If objHashTable.ContainsKey("USER") = False Then
                objHashTable.Add("USER", ConstSession.UserName)
            Else
                objHashTable("USER") = ConstSession.UserName
            End If
            If objHashTable.ContainsKey("CODENTE") = False Then
                objHashTable.Add("CODENTE", constsession.idente)
            Else
                objHashTable("CODENTE") = constsession.idente
            End If
            If objHashTable.ContainsKey("CodENTE") = False Then
                objHashTable.Add("CodENTE", constsession.idente)
            Else
                objHashTable("CodENTE") = constsession.idente
            End If
            If objHashTable.ContainsKey("COD_TRIBUTO") = False Then
                objHashTable.Add("COD_TRIBUTO", Utility.Costanti.TRIBUTO_OSAP)
            Else
                objHashTable("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
            End If
            If objHashTable.ContainsKey("CODTIPOPROCEDIMENTO") = False Then
                objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
            Else
                objHashTable("CODTIPOPROCEDIMENTO") = "L"
            End If

            If objHashTable.ContainsKey("ListCateg") = False Then
                objHashTable.Add("ListCateg", ListCateg)
            Else
                objHashTable("ListCateg") = ListCateg
            End If
            If objHashTable.ContainsKey("ListTipiOcc") = False Then
                objHashTable.Add("ListTipiOcc", ListTipiOcc)
            Else
                objHashTable("ListTipiOcc") = ListTipiOcc
            End If
            If objHashTable.ContainsKey("ListAgevolazioni") = False Then
                objHashTable.Add("ListAgevolazioni", ListAgevolazioni)
            Else
                objHashTable("ListAgevolazioni") = ListAgevolazioni
            End If
            If objHashTable.ContainsKey("ListTariffe") = False Then
                objHashTable.Add("ListTariffe", ListTariffe)
            Else
                objHashTable("ListTariffe") = ListTariffe
            End If
            Session("HashTableRettificaAccertamenti") = objHashTable
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiaratoOSAP.SetHashTable.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Response.Write("<script language='javascript'>")
            Response.Write("</script>")
        End Try
    End Sub
End Class
