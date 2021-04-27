Imports log4net
Imports ComPlusInterface
''' <summary>
''' Pagina per la gestione manuale di un immobile in provvedimento.
''' Contiene le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class InserimentoManualeImmobileOSAP
	Inherits BasePage
	Private Shared Log As ILog = LogManager.GetLogger(GetType(InserimentoManualeImmobileOSAP))
    Private MyWuc As New WucArticolo

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents Label1 As System.Web.UI.WebControls.Label
	Protected WithEvents Label3 As System.Web.UI.WebControls.Label
	Protected WithEvents Label2 As System.Web.UI.WebControls.Label
	Protected WithEvents LnkOpenStradario As System.Web.UI.WebControls.ImageButton
	Protected WithEvents LnkPulisciStrada As System.Web.UI.WebControls.ImageButton
	Protected WithEvents TxtVia As System.Web.UI.WebControls.TextBox
	Protected WithEvents TxtCodVia As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label4 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtCivico As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label5 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtEsponente As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label6 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtInterno As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label8 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtScala As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label10 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtFoglio As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label11 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtNumero As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label23 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtSub As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label7 As System.Web.UI.WebControls.Label
	Protected WithEvents Label9 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtDataInizio As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label21 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtDataFine As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label22 As System.Web.UI.WebControls.Label
	Protected WithEvents ChkIsGiornaliera As System.Web.UI.WebControls.CheckBox
	Protected WithEvents Label36 As System.Web.UI.WebControls.Label
	Protected WithEvents Label37 As System.Web.UI.WebControls.Label
	Protected WithEvents Label19 As System.Web.UI.WebControls.Label
	Protected WithEvents Label12 As System.Web.UI.WebControls.Label
	Protected WithEvents Label54 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtBimestri As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label13 As System.Web.UI.WebControls.Label
	Protected WithEvents Label55 As System.Web.UI.WebControls.Label
	Protected WithEvents DdlCategorie As System.Web.UI.WebControls.DropDownList
	Protected WithEvents Label14 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtTariffa As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label15 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtNComponenti As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label16 As System.Web.UI.WebControls.Label
	Protected WithEvents Label56 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtMq As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label17 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtImpArticolo As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label18 As System.Web.UI.WebControls.Label
	Protected WithEvents TxtImpNetto As System.Web.UI.WebControls.TextBox
	Protected WithEvents Label20 As System.Web.UI.WebControls.Label
	Protected WithEvents ChkImpForzato As System.Web.UI.WebControls.CheckBox
	Protected WithEvents Label31 As System.Web.UI.WebControls.Label
	Protected WithEvents LnkNewRid As System.Web.UI.WebControls.ImageButton
	Protected WithEvents LnkDelRid As System.Web.UI.WebControls.ImageButton
	Protected WithEvents Label38 As System.Web.UI.WebControls.Label
	Protected WithEvents LblResultRid As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents LnkNewDet As System.Web.UI.WebControls.ImageButton
	Protected WithEvents LnkDelDet As System.Web.UI.WebControls.ImageButton
	Protected WithEvents Label39 As System.Web.UI.WebControls.Label
	Protected WithEvents LblResultDet As System.Web.UI.WebControls.Label
    Protected WithEvents txtIdDettaglioTestata As System.Web.UI.WebControls.TextBox
    'Protected WithEvents btnRibalta As System.Web.UI.WebControls.Button
    Protected WithEvents CmdSalvaDati As System.Web.UI.WebControls.Button
	Protected WithEvents TxtIdTariffa As System.Web.UI.WebControls.TextBox
	Protected WithEvents btnGestioneTariffa As System.Web.UI.WebControls.Button
	'Protected WithEvents PanelWUC As System.Web.UI.WebControls.Panel

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
		Try
            'Dim uc As UserControl = CType(Page.LoadControl("../Wuc/WucArticolo.ascx"), UserControl)
            ''Dim ClientId As String = uc.ClientID
            'PanelWUC.Controls.Add(uc)
            'Dim FncGest As New ClsGestioneAccertamenti
            'Dim oListAcc() As OSAPAccertamentoArticolo
            'Dim MyDic As New IRemInterfaceOSAP.DichiarazioneTosapCosap
            'Dim MyArt As New IRemInterfaceOSAP.Articolo
            'For Each oMyAcc As OSAPAccertamentoArticolo In oListAcc
            '	If oMyAcc.Progressivo = Request.Item("IdProgressivo") Then
            '		FncGest.CastPROVArtIntoOSAPArt(MyArt, oMyAcc)
            '	End If
            'Next
            'MyDic.ArticoliDichiarazione(0) = MyArt

            'Dim ciccio As New OPENgovTOCO.DichiarazioneSession
            'ciccio.SessionDichiarazioneTosapCosap = MyDic
            Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
            MyWuc = CType(Me.FindControl("wucArticolo"), WucArticolo)
			MyWuc.IdArticolo = CInt(Request("IdArticolo").ToString())
			MyWuc.idcontribuente = CInt(Request("codContribuente").ToString())
			If Request.Item("provenienza") = "Griglia" Then
				MyWuc.OpType = OPENgovTOCO.OSAPConst.OperationType.EDIT
			End If
			'Add nuovo articolo da edit dichiarazione
			If Request.Item("anno") <> "" Then
                txtAnnoAccertamento.Text = ConstSession.AnnoAccertamentoOSAP(Request.Item("anno"))
            End If
			If Request.Item("idprogressivo") <> "-1" Then
				TxtProgGriglia.Text = Request.Item("idprogressivo")
			End If
			If Not Page.IsPostBack Then
				Dim clientArrayControl() As String = MyWuc.GetMandatoryFields
				btnRibalta.Attributes.Add("onClick", "return ValidateForm('" + (clientArrayControl(0) + ("','" + (clientArrayControl(1) + "')"))))
				Cancel.Attributes.Add("onClick", "parent.window.close()")
			End If
		Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeOSAP.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
	End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnRibalta.Click
		Dim oAccertato As New OSAPAccertamentoArticolo
		Dim MyUI As IRemInterfaceOSAP.Articolo
		Dim oAccertatoGriglia() As OSAPAccertamentoArticolo
		Dim ListCateg() As IRemInterfaceOSAP.Categorie
		Dim ListTipiOcc() As IRemInterfaceOSAP.TipologieOccupazioni
		Dim ListAgevolazioni() As IRemInterfaceOSAP.Agevolazione
		Dim ListTariffe() As IRemInterfaceOSAP.Tariffe
		Dim TipoRuolo As IRemInterfaceOSAP.Ruolo.E_TIPO = IRemInterfaceOSAP.Ruolo.E_TIPO.ORDINARIO
		Dim MyMotore As IRemInterfaceOSAP.IRemotingInterfaceOSAP
		Dim ResultCalcolato As IRemInterfaceOSAP.CalcoloResult
		Dim FncAcc As New ClsGestioneAccertamenti
		Dim MyHashTable As Hashtable

		Try
			Dim Anno As Integer = CInt(txtAnnoAccertamento.Text)

			MyUI = MyWuc.SetArticoliObject
			MyHashTable = Session("HashTableRettificaAccertamenti")
			If MyHashTable.ContainsKey("ListCateg") Then
				ListCateg = MyHashTable("ListCateg")
				ListTipiOcc = MyHashTable("ListTipiOcc")
				ListAgevolazioni = MyHashTable("ListAgevolazioni")
				ListTariffe = MyHashTable("ListTariffe")
			Else
				'carico i parametri per il calcolo
                FncAcc.OSAPLoadParamCalcolo(Anno, ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe, constsession.idente)
			End If
			'attivo il servizio
			MyMotore = CType(Activator.GetObject(GetType(IRemInterfaceOSAP.IRemotingInterfaceOSAP), ConstSession.urlMotoreOSAP), IRemInterfaceOSAP.IRemotingInterfaceOSAP)
			ResultCalcolato = MyMotore.CalcolaOSAP(TipoRuolo, MyUI, ListCateg, ListTipiOcc, ListAgevolazioni, ListTariffe, Nothing)
			If (ResultCalcolato.Result <> IRemInterfaceOSAP.E_CALCOLORESULT.OK) Then
				Response.Redirect("../../PaginaErrore.aspx")
			Else
				FncAcc.OSAPCastArtIntoProvArt(MyUI, oAccertato, Request.Item("IdContribuente"))
				oAccertato.Calcolo = ResultCalcolato
                oAccertato.Calcola_Interessi = True
                oAccertato.Anno = Anno
                'carico il progressivo della griglia
                If TxtProgGriglia.Text <> "-1" And TxtProgGriglia.Text <> "" Then
                    oAccertato.Progressivo = TxtProgGriglia.Text
                End If
                If Not Session("oAccertatiGriglia") Is Nothing Then
                    oAccertato.IdLegame = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo()).GetLength(0) + 1
                End If
            End If

			'assegno sanzione,descrsanzione e calcolointeressi all'immobile se è in modifica
			If Not Session("oAccertatiGriglia") Is Nothing Then
				oAccertatoGriglia = Session("oAccertatiGriglia")
				Dim x As Integer
				For x = 0 To oAccertatoGriglia.GetUpperBound(0)
					If oAccertato.Progressivo = oAccertatoGriglia(x).Progressivo Then
						oAccertato.Anno = Anno
						oAccertato.IdLegame = oAccertatoGriglia(x).IdLegame
						oAccertato.Sanzioni = oAccertatoGriglia(x).Sanzioni
						oAccertato.DescrSanzioni = oAccertatoGriglia(x).DescrSanzioni
						oAccertato.Calcola_Interessi = oAccertatoGriglia(x).Calcola_Interessi
						oAccertatoGriglia(x) = oAccertato
						Exit For
					End If
				Next
			End If

			Session("oAccertato") = oAccertato : Session("oAccertatiGriglia") = oAccertatoGriglia
            Dim sScript As String = ""
            sScript = "parent.parent.opener.location.href='SearchDatiAccertatoOSAP.aspx';"
            sScript += "parent.window.close();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeOSAP.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
	End Sub
End Class
