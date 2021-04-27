Imports log4net
Imports IRemInterfaceOSAP
Imports OPENGovTOCO
''' <summary>
''' UserControl per la generazione accertamenti OSAP.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class WucArticolo
	Inherits System.Web.UI.UserControl
	Dim Log As ILog = LogManager.GetLogger(GetType(WucArticolo))
	Public OpType As OSAPConst.OperationType
	Dim objDichiarazione As DichiarazioneTosapCosap
	Dim wucContribuente As WucDatiContribuente
	Dim _idArticolo As Integer
	Dim _idContribuente As Integer

	Public Property IdArticolo() As Integer
		Get
			Return _idArticolo
		End Get
		Set(ByVal Value As Integer)
			_idArticolo = Value
		End Set
	End Property
	Public Property IdContribuente() As Integer
		Get
			Return _idContribuente
		End Get
		Set(ByVal Value As Integer)
			_idContribuente = Value
		End Set
	End Property
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
        SetEvent()
        Try
            If TxtViaRibaltata.Text <> "" Then
                TxtVia.Text = TxtViaRibaltata.Text
            End If
            objDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap
            If Not Page.IsPostBack Then
                BindData()
                If (Me.OpType = OSAPConst.OperationType.VIEW) Then
                    Dim wuc As WucArticolo = CType(Page.FindControl("wucArticolo"), WucArticolo)
                    SharedFunction.ChangeControlStatus(False, wuc)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucArticolo.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub SetEvent()
        'LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidEse()");
        'LnkDelRid.Attributes.Add("OnClick", "DeleteRidEse(\'R\')");
        'LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidEse('" & ObjCodDescr.TIPO_ESENZIONI & "')")
        'LnkDelDet.Attributes.Add("OnClick", "return DeleteMaggiorazioni('" & txtMaggiorazioni.ClientID & "')")
        Dim UrlStradario As String = ConstSession.UrlStradario
        LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('" + UrlStradario + "','RibaltaStrada','" + constsession.idente + "')")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BindData()
        wucContribuente = CType(Me.FindControl("wucContribuente"), WucDatiContribuente)
        Try
            If (Not (wucContribuente) Is Nothing) Then
                If Not objDichiarazione Is Nothing Then
                    wucContribuente.oAnagrafica = objDichiarazione.AnagraficaContribuente
                Else
                    Dim angraficaDAO As New DAO.AnagraficheDAO
                    wucContribuente.oAnagrafica = angraficaDAO.GetAnagraficaContribuente(IdContribuente)
                End If
            End If
            'Bind DropDown Categorie
            cmbCategoria.DataSource = DTO.MetodiCategorie.GetCategorie(ConstSession.StringConnectionOSAP, ConstSession.AnnoAccertamentoOSAP(""), True, ConstSession.IdEnte, Utility.Costanti.TRIBUTO_OSAP)
            cmbCategoria.DataValueField = "IdCategoria"
            cmbCategoria.DataTextField = "Descrizione"
            cmbCategoria.DataBind()
            cmbCategoria.SelectedValue = "-1"
            'Bind DropDown Tipologie Occupazioni
            cmbTipologiaOccupazione.DataSource = DTO.MetodiTipologieOccupazioni.GetTipologieOccupazioni(ConstSession.AnnoAccertamentoOSAP(""), True, ConstSession.IdEnte)
            cmbTipologiaOccupazione.DataValueField = "IdTipologiaOccupazione"
            cmbTipologiaOccupazione.DataTextField = "Descrizione"
            cmbTipologiaOccupazione.DataBind()
            cmbTipologiaOccupazione.SelectedValue = "-1"
            'Bind DropDown Tipi consistenza
            cmbTipoConsistenza.DataSource = DTO.MetodiTipoConsistenza.GetTipiConsistenza(ConstSession.StringConnectionOSAP, True)
            cmbTipoConsistenza.DataValueField = "IdTipoConsistenza"
            cmbTipoConsistenza.DataTextField = "Descrizione"
            cmbTipoConsistenza.DataBind()
            cmbTipoConsistenza.SelectedValue = "-1"
            'Bind DropDown Durata
            cmbTipoDurata.DataSource = DTO.MetodiDurata.GetDurate(ConstSession.StringConnectionOSAP, True)
            cmbTipoDurata.DataValueField = "IdDurata"
            cmbTipoDurata.DataTextField = "Descrizione"
            cmbTipoDurata.DataBind()
            cmbTipoDurata.SelectedValue = "-1"

            Dim ListArticoliDic() As Articolo
            If Not IsNothing(Session("ListArticoliDic")) Then
                ListArticoliDic = CType(Session("ListArticoliDic").toarray(GetType(Articolo)), Articolo())
            Else
                ReDim Preserve ListArticoliDic(0)
                ListArticoliDic(0) = New Articolo
            End If

            If ((Me.OpType = OSAPConst.OperationType.VIEW) OrElse (Me.OpType = OSAPConst.OperationType.EDIT)) Then
                For Each o As Articolo In ListArticoliDic
                    If (o.IdArticolo = Me.IdArticolo) Then
                        Session("Agevolazioni") = DTO.MetodiAgevolazione.GetAgevolazioni("", o.IdArticolo, ConstSession.IdEnte)
                        GrdAgevolazioni.DataSource = Session("Agevolazioni")
                        GrdAgevolazioni.DataBind()
                        If o.Civico > 0 Then
                            Me.TxtCivico.Text = o.Civico
                        End If
                        Me.TxtCodVia.Text = o.CodVia.ToString
                        Me.txtConsistenza.Text = o.Consistenza.ToString
                        Me.TxtDataInizio.Text = o.DataInizioOccupazione.ToString("dd/MM/yyyy")
                        Me.txtDataFine.Text = o.DataFineOccupazione.ToString("dd/MM/yyyy")
                        Me.txtDetrazioni.Text = o.DetrazioneImporto.ToString
                        Me.txtDurata.Text = o.DurataOccupazione.ToString
                        Me.TxtEsponente.Text = o.Esponente
                        Me.TxtInterno.Text = o.Interno
                        Me.txtMaggiorazioni.Text = o.MaggiorazioneImporto.ToString
                        Me.txtMaggiorazioniPerc.Text = o.MaggiorazionePerc.ToString
                        Me.TxtNoteUI.Text = o.Note
                        Me.TxtScala.Text = o.Scala
                        Me.TxtVia.Text = o.SVia
                        Me.cmbCategoria.SelectedValue = o.Categoria.IdCategoria.ToString
                        Me.cmbTipoConsistenza.SelectedValue = o.TipoConsistenzaTOCO.IdTipoConsistenza.ToString
                        Me.cmbTipoDurata.SelectedValue = o.TipoDurata.IdDurata.ToString
                        Me.cmbTipologiaOccupazione.SelectedValue = o.TipologiaOccupazione.IdTipologiaOccupazione.ToString
                        Me.chkAttrazione.Checked = Boolean.Parse(o.Attrazione.ToString)
                        '*** 20130610 - ruolo supplettivo ***
                        Me.TxtIdArticoloPadre.Text = o.IdArticoloPadre.ToString
                        '*** ***
                    End If
                Next
            Else
                Session("Agevolazioni") = DTO.MetodiAgevolazione.GetAgevolazioni("", -1, constsession.idente)
                GrdAgevolazioni.DataSource = Session("Agevolazioni")
                GrdAgevolazioni.DataBind()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucArticolo.BlindData.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Private Method"

    'Public Sub SalvaArticolo()
    '	Dim nRet As Integer = -1
    '	Dim sScript As String = ""
    '	Try
    '		If ((OpType = OSAPConst.OperationType.ADD) _
    '		   OrElse (OpType = OSAPConst.OperationType.ADDFROMEDIT)) Then
    '			If (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione Is Nothing) Then
    '				Dim arrayArticolo() As Articolo = New Articolo((1) - 1) {}
    '				arrayArticolo(0) = SetArticoliObject()
    '				'Usato per eliminare gli articoli;
    '				objDichiarazione.ArticoliDichiarazione = arrayArticolo
    '			Else
    '				objDichiarazione.ArticoliDichiarazione = GetMyArray()
    '			End If
    '			DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione
    '			If (OpType = OSAPConst.OperationType.ADD) Then
    '				Response.Redirect((OSAPPages.DichiarazioniAdd + "?FromArticoli=true"))
    '			ElseIf (OpType = OSAPConst.OperationType.ADDFROMEDIT) Then
    '				DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now
    '				DTO.MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap)
    '				DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.MinValue
    '				Response.Redirect((OSAPPages.DichiarazioniAdd + "?FromArticoli=true"))
    '				sScript = ("alert (\""Nuovo Articolo aggiunto correttamente alla dichiarazione\"");location.href = '" _
    '				   + (OSAPPages.DichiarazioniEdit + "?FromArticoli=true';"))
    '				RegisterScript("", ("<script language=\'javascript\'>" _
    '				 + (sScript + "</script>")))
    '				Response.Redirect((OSAPPages.DichiarazioniEdit + "?FromArticoli=true"))
    '			End If
    '		ElseIf (OpType = OSAPConst.OperationType.EDIT) Then
    '			Dim FncArticolo As DAO.ArticoliDAO = New DAO.ArticoliDAO
    '			Dim arrayArticolo() As Articolo = New Articolo((1) - 1) {}
    '			arrayArticolo(0) = SetArticoliObject()
    '			'Usato per eliminare gli articoli;
    '			arrayArticolo(0).IdArticolo = Me.IdArticolo
    '			'Log.Debug("SalvaArticolo::IdArticoloPadre::"+arrayArticolo[0].IdArticoloPadre.ToString());
    '			Dim i As Integer = 0
    '			Do While (i < DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione.Length)
    '				If (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione(i).IdArticolo = Me.IdArticolo) Then
    '					DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione(i) = arrayArticolo(0)
    '					Dim MyDBEngine As DBEngine = Nothing
    '					MyDBEngine = DAO.DBEngineFactory.GetDBEngine
    '					MyDBEngine.OpenConnection()
    '					MyDBEngine.BeginTransaction()
    '					'Salva Dichiarazione e modifica articolo
    '					DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione(i).DataVariazione = DateTime.Now
    '					nRet = FncArticolo.UpdateArticolo(MyDBEngine, DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione(i))
    '					If (nRet < 1) Then
    '						MyDBEngine.RollbackTransaction()
    '						MyDBEngine.CloseConnection()
    '						Exit Do
    '					End If
    '					DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione(i).DataVariazione = DateTime.MinValue
    '					nRet = FncArticolo.SetArticolo(MyDBEngine, DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione(i))
    '					If (nRet < 1) Then
    '						MyDBEngine.RollbackTransaction()
    '						MyDBEngine.CloseConnection()
    '						Exit Do
    '					End If
    '					MyDBEngine.CommitTransaction()
    '					MyDBEngine.CloseConnection()
    '					Exit Do
    '				End If
    '				i = (i + 1)
    '			Loop
    '			If (nRet > 1) Then
    '				sScript = ("alert (\""Articolo modificato correttamente\"");location.href = '" _
    '				   + (OSAPPages.DichiarazioniEdit + "?FromArticoli=true';"))
    '			Else
    '				sScript = "alert (\""Errore durante la modifica\"")"
    '			End If
    '			RegisterScript("", ("<script language=\'javascript\'>" _
    '			 + (sScript + "</script>")))
    '		End If
    '	Catch ex As Exception
    '		
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucArticolo.SalvaArticolo.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")

    '		Throw
    '	End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function SetArticoliObject() As Articolo
		Dim CurrentItem As Articolo = New Articolo
		Try
			'Immobile Dichiarazione
			CurrentItem.Operatore = DichiarazioneSession.sOperatore
			CurrentItem.DataInserimento = DateTime.Now
            If (TxtCivico.Text.CompareTo("") <> 0) Then
                CurrentItem.Civico = Integer.Parse(TxtCivico.Text)
            Else
                CurrentItem.Civico = -1
            End If
            If TxtCodVia.Text <> "" Then
                CurrentItem.CodVia = Integer.Parse(TxtCodVia.Text)
            Else
                CurrentItem.CodVia = -1
            End If
            CurrentItem.DataInizioOccupazione = DateTime.Parse(TxtDataInizio.Text)
			CurrentItem.Esponente = TxtEsponente.Text
            CurrentItem.IdArticolo = -1
            If DichiarazioneSession.SessionDichiarazioneTosapCosap Is Nothing Then
                DichiarazioneSession.SessionDichiarazioneTosapCosap = New DichiarazioneTosapCosap()
            End If
            CurrentItem.IdDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.IdDichiarazione
			CurrentItem.Interno = TxtInterno.Text
			CurrentItem.MaggiorazioneImporto = SharedFunction.FormatDoubleToDb(txtMaggiorazioni.Text)
			CurrentItem.MaggiorazionePerc = SharedFunction.FormatDoubleToDb(txtMaggiorazioniPerc.Text)
			CurrentItem.Consistenza = SharedFunction.FormatDoubleToDb(txtConsistenza.Text)
			Dim objTipoConsistenza As TipoConsistenza = New TipoConsistenza
			objTipoConsistenza.IdTipoConsistenza = Integer.Parse(cmbTipoConsistenza.SelectedValue)
			objTipoConsistenza.Descrizione = cmbTipoConsistenza.SelectedItem.Text
			CurrentItem.TipoConsistenzaTOCO = objTipoConsistenza
			CurrentItem.Note = TxtNoteUI.Text
			CurrentItem.Scala = TxtScala.Text
			CurrentItem.SVia = TxtVia.Text
			CurrentItem.DetrazioneImporto = SharedFunction.FormatDoubleToDb(txtDetrazioni.Text)
			Dim objCategoria As Categorie = New Categorie
			objCategoria.IdCategoria = Integer.Parse(cmbCategoria.SelectedValue)
			objCategoria.Descrizione = cmbCategoria.SelectedItem.Text
			CurrentItem.Categoria = objCategoria
			Dim objTipologiaOccupazione As TipologieOccupazioni = New TipologieOccupazioni
			objTipologiaOccupazione.IdTipologiaOccupazione = Integer.Parse(cmbTipologiaOccupazione.SelectedValue)
			objTipologiaOccupazione.Descrizione = cmbTipologiaOccupazione.SelectedItem.Text
			CurrentItem.TipologiaOccupazione = objTipologiaOccupazione
            Dim oListArtVSAgev As ArrayList = New ArrayList
            For Each MyItemGrd As GridViewRow In GrdAgevolazioni.Rows
                If (CType(MyItemGrd.FindControl("ChkSelezionato"), CheckBox).Checked = True) Then
                    Dim oMyAgevolazione As Agevolazione = New Agevolazione
                    oMyAgevolazione.IdAgevolazione = CInt(CType(MyItemGrd.FindControl("hfIdAgevolazione"), HiddenField).Value)
                    oListArtVSAgev.Add(oMyAgevolazione)
                End If
            Next
            CurrentItem.ListAgevolazioni = CType(oListArtVSAgev.ToArray(GetType(Agevolazione)), Agevolazione())
			CurrentItem.DurataOccupazione = Integer.Parse(txtDurata.Text)
			Dim objDurata As Durata = New Durata
			objDurata.IdDurata = Integer.Parse(cmbTipoDurata.SelectedValue)
			objDurata.Descrizione = cmbTipoDurata.SelectedItem.Text
			CurrentItem.TipoDurata = objDurata
			'se la durata è annuale allora sono in occupazione permanente altrimenti è temporanea
			If ((objDurata.Descrizione = "ORE") _
			   OrElse (objDurata.Descrizione = "GIORNI")) Then
                CurrentItem.IdTributo = Utility.Costanti.TRIBUTO_OccupazioneTemporanea
			Else
                CurrentItem.IdTributo = Utility.Costanti.TRIBUTO_OccupazionePermanente
			End If
			'ArtCava: aggiunta compilazione del campo
			If SharedFunction.IsNullOrEmpty(txtDataFine.Text) Then
				Select Case (objDurata.Descrizione)
					Case "ORE"
						CurrentItem.DataFineOccupazione = CurrentItem.DataInizioOccupazione.AddHours(CurrentItem.DurataOccupazione)
					Case "GIORNI"
						CurrentItem.DataFineOccupazione = CurrentItem.DataInizioOccupazione.AddDays(CurrentItem.DurataOccupazione)
					Case Else
						CurrentItem.DataFineOccupazione = CurrentItem.DataInizioOccupazione.AddYears(CurrentItem.DurataOccupazione)
				End Select
			Else
				CurrentItem.DataFineOccupazione = DateTime.Parse(txtDataFine.Text)
			End If
			CurrentItem.Attrazione = chkAttrazione.Checked
			'*** 20130610 - ruolo supplettivo ***
			If (TxtIdArticoloPadre.Text <> String.Empty) Then
				CurrentItem.IdArticoloPadre = Integer.Parse(TxtIdArticoloPadre.Text)
			End If
			'Log.Debug("WucArticolo::SetArticoliObject::IdArticoloPadre::"+CurrentItem.IdArticoloPadre.ToString());
			'*** ***
			'Setto un IdArticolo all'immobile per poterlo elimare
			CurrentItem.IdArticolo = Me.IdArticolo
			Return CurrentItem
		Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucArticolo.SetArticoliObject.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw
		End Try
	End Function

    '
    ''' <summary>
    ''' Get Articoli dichiarazione da Dichiarazione session object
    ''' </summary>
    ''' <returns></returns>
    Private Function GetMyArray() As Articolo()
		Dim MyArray As ArrayList = New ArrayList
        MyArray = DTO.MetodiArticolo.GetMyArray(ConstSession.IdEnte)
        Try
            If ((OpType = OSAPConst.OperationType.ADD) _
           OrElse (OpType = OSAPConst.OperationType.ADDFROMEDIT)) Then
                MyArray.Add(SetArticoliObject)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucArticolo.GetMyArray.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return CType(MyArray.ToArray(GetType(Articolo)), Articolo())
    End Function
#End Region
#Region "Public Method"
    '
    ''' <summary>
    ''' Set mandatory form fields
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMandatoryFields() As String()
        Dim returnValue() As String = New String((2) - 1) {}
        Dim clientControlsID As String = ""
        Dim clientMandatoryMessagge As String = ""
        Try
            clientControlsID = (Me.TxtCodVia.ClientID + ";")
            clientControlsID = (clientControlsID _
               + (Me.TxtDataInizio.ClientID + ";"))
            clientControlsID = (clientControlsID _
               + (Me.cmbCategoria.ClientID + ";"))
            clientControlsID = (clientControlsID _
               + (Me.cmbTipoDurata.ClientID + ";"))
            clientControlsID = (clientControlsID _
               + (Me.txtDurata.ClientID + ";"))
            clientControlsID = (clientControlsID _
               + (Me.cmbTipologiaOccupazione.ClientID + ";"))
            clientControlsID = (clientControlsID _
               + (Me.txtConsistenza.ClientID + ";"))
            clientControlsID = (clientControlsID _
               + (Me.cmbTipoConsistenza.ClientID + ";"))
            '*** 20130325 - aggiunto controllo obligatorietà su data fine ***
            clientControlsID = (clientControlsID + Me.txtDataFine.ClientID)
            '*** ***
            clientMandatoryMessagge = (" - Via" + ";")
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Data Inizio" + ";"))
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Categoria" + ";"))
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Tipo Durata" + ";"))
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Durata" + ";"))
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Tipologia Occupazione" + ";"))
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Consistenza" + ";"))
            clientMandatoryMessagge = (clientMandatoryMessagge + (" - Tipo Consistenza" + ";"))
            '*** 20130325 - aggiunto controllo obligatorietà su data fine ***
            clientMandatoryMessagge = (clientMandatoryMessagge + " - Data Fine;")
            '*** ***
            returnValue(0) = clientControlsID
            returnValue(1) = clientMandatoryMessagge
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucArticolo.GetMandatoryFields.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return returnValue
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function RemoveArticolo() As Articolo()
		Return GetMyArray()
	End Function
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtDurata_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDurata.TextChanged
		txtDataFine.Text = ""
	End Sub
End Class
