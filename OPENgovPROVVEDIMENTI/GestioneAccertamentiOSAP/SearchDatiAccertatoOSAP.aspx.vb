Imports log4net
Imports ComPlusInterface
''' <summary>
''' Pagina per la generazione dei provvedimenti OSAP.
''' Contiene le funzioni della comandiera e la griglia per la gestione dell'accertato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class SearchDatiAccertatoOSAP
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchDatiAccertatoOSAP))
    Protected FncForGrd As New Formatta.SharedGrd
    Private sScript As String = ""

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
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
        Dim ListAccertato() As OSAPAccertamentoArticolo
        Dim MyAccertato As OSAPAccertamentoArticolo
        Dim intProgressivo As Integer
        Dim intGrigliaAccertati As Integer

        Try
            hfAnno.Value = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO")
            hfIdContribuente.Value = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE")
            btnInsManuale.Attributes.Add("onclick", "return ApriInserimentoImmobile();return false;")
            btnAccertato.Attributes.Add("onclick", "return ApriRicercaAccertato();return false;")

            If Page.IsPostBack = False Then
                If Not Session("oAccertato") Is Nothing Then
                    'devo caricare l'oogetto ArticoloRuoloAccertamento per popolare la griglia
                    MyAccertato = CType(Session("oAccertato"), OSAPAccertamentoArticolo)
                    If Not Session("oAccertatiGriglia") Is Nothing And MyAccertato.Progressivo <> 0 Then
                        ListAccertato = Session("oAccertatiGriglia")
                        For intGrigliaAccertati = 0 To ListAccertato.Length - 1
                            If ListAccertato(intGrigliaAccertati).Progressivo = MyAccertato.Progressivo Then
                                ListAccertato(intGrigliaAccertati) = MyAccertato
                            End If
                        Next
                    ElseIf MyAccertato.Progressivo <> 0 Then
                        ListAccertato = Session("oAccertatiGriglia")
                        intProgressivo = ListAccertato.Length + 1

                        ReDim Preserve ListAccertato(ListAccertato.Length)
                        ListAccertato(ListAccertato.Length - 1) = MyAccertato
                        ListAccertato(ListAccertato.Length - 1).Progressivo = intProgressivo
                        'Svuota la Session con la dichirazione precedente
                    ElseIf MyAccertato.Progressivo = 0 Then
                        If Not Session("oAccertatiGriglia") Is Nothing Then
                            ListAccertato = Session("oAccertatiGriglia")
                            intProgressivo = ListAccertato.Length + 1

                            ReDim Preserve ListAccertato(ListAccertato.Length)
                            ListAccertato(ListAccertato.Length - 1) = MyAccertato
                            ListAccertato(ListAccertato.Length - 1).Progressivo = intProgressivo
                        Else
                            intProgressivo = 1
                            ReDim Preserve ListAccertato(0)
                            ListAccertato(0) = MyAccertato
                            ListAccertato(0).Progressivo = intProgressivo
                        End If
                    End If
                    GrdDatiAcc.DataSource = ListAccertato
                    Session("oAccertatiGriglia") = ListAccertato
                    GrdDatiAcc.DataBind()
                ElseIf Not Session("oAccertatiGriglia") Is Nothing Then
                    ListAccertato = Session("oAccertatiGriglia")
                    GrdDatiAcc.DataSource = ListAccertato
                    Session("oAccertatiGriglia") = ListAccertato
                    GrdDatiAcc.DataBind()
                End If
                Session("ListAccertato") = Nothing

                Select Case CInt(GrdDatiAcc.Rows.Count)
                    Case 0
                        GrdDatiAcc.Visible = False
                    Case Is > 0
                        GrdDatiAcc.Visible = True
                End Select
            End If

            sScript = "parent.document.getElementById('attesaCarica').style.display='none';"
            sScript += "parent.document.getElementById('loadGridAccertato').style.display='' ;"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
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
                Dim chkSanzioni As CheckBox
                Dim idSanzioni As String
                Dim IdProgressivo As Integer
                Dim intXsanzioni As Integer
                Dim IdLegame As Integer
                Dim obj As OSAPAccertamentoArticolo
                Dim objXsanzioni As OSAPAccertamentoArticolo()
                Dim bFound As Boolean = False

                obj = CType(e.Row.DataItem, OSAPAccertamentoArticolo)

                IdLegame = obj.IdLegame
                IdProgressivo = obj.Progressivo

                chkSanzioni = e.Row.FindControl("chkSanzioni")
                idSanzioni = obj.Sanzioni
                If idSanzioni <> "-1" And idSanzioni <> "" Then
                    chkSanzioni.Checked = True
                End If

                intXsanzioni = 0
                If Not Session("oAccertatiGriglia") Is Nothing Then
                    objXsanzioni = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
                    For intXsanzioni = 0 To objXsanzioni.Length - 1
                        If objXsanzioni(intXsanzioni).IdLegame = IdLegame And objXsanzioni(intXsanzioni).Progressivo = IdProgressivo Then
                            bFound = True
                            Exit For
                        Else
                            bFound = False
                        End If
                    Next
                    If Not bFound Then
                        ReDim Preserve objXsanzioni(objXsanzioni.Length)
                        objXsanzioni(objXsanzioni.Length - 1) = obj
                    End If
                Else
                    ReDim Preserve objXsanzioni(intXsanzioni)
                    objXsanzioni(intXsanzioni) = obj
                End If
                Session.Add("oAccertatiGriglia", objXsanzioni)
                e.Row.Cells(10).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & IdLegame & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
                e.Row.Cells(15).Attributes.Add("onClick", "ApriModificaImmobileAnater('" & IdProgressivo & "','" & obj.IdArticolo & "')")
                'per il binddata del wuc
                Dim ListArticoliDic As New ArrayList
                Dim ArticoloSingle As New IRemInterfaceOSAP.Articolo
                Dim FncGest As New ClsGestioneAccertamenti
                FncGest.OSAPCastProvArtIntoArt(obj, ArticoloSingle)
                If Not IsNothing(Session("ListArticoliDic")) Then
                    ListArticoliDic = Session("ListArticoliDic")
                End If
                ListArticoliDic.Add(ArticoloSingle)
                Session("ListArticoliDic") = ListArticoliDic
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim myRow As GridViewRow
            Dim IDRow As Integer = -1
            If e.CommandArgument.ToString() <> "" Then
                IDRow = CInt(e.CommandArgument.ToString())
            End If
            If e.CommandName = "Edit" Then
                For Each myRow In GrdDatiAcc.Rows
                    If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
                        GrdDatiAcc.EditIndex = myRow.RowIndex
                        GrdDatiAcc.DataSource = Session("oAccertatiGriglia")
                        GrdDatiAcc.DataBind()
                        Return
                    End If
                Next
            ElseIf e.CommandName = "Update" Then
                For Each myRow In GrdDatiAcc.Rows
                    If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
                        Dim oAccertamentoUpdate() As OSAPAccertamentoArticolo
                        Dim intArticoliUpdate As Integer
                        Dim IdLegameGrid As TextBox = myRow.FindControl("txtLegame")
                        Dim IdGridUpdate As String = myRow.Cells(0).Text

                        If IdLegameGrid.Text = "" Then
                            sScript = "msgLegameVuoto();"
                            RegisterScript(sScript, Me.GetType())
                        Else
                            oAccertamentoUpdate = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
                            For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
                                If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
                                    oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
                                End If
                            Next
                            GrdDatiAcc.EditIndex = -1
                            GrdDatiAcc.DataSource = oAccertamentoUpdate
                            GrdDatiAcc.DataBind()
                            Session("oAccertatiGriglia") = oAccertamentoUpdate
                        End If
                        Return
                    End If
                Next
            ElseIf e.CommandName = "Cancel" Then
                For Each myRow In GrdDatiAcc.Rows
                    If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
                        If Not Session("oAccertatiGriglia") Is Nothing Then
                            GrdDatiAcc.EditIndex = -1
                            GrdDatiAcc.DataSource = Session("oAccertatiGriglia")
                            GrdDatiAcc.DataBind()
                        End If
                        Return
                    End If
                Next
            ElseIf e.CommandName = "Delete" Then
                For Each myRow In GrdDatiAcc.Rows
                    If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
                        Dim oAccertamentoDelete() As OSAPAccertamentoArticolo
                        Dim oAccertamentoReturn() As OSAPAccertamentoArticolo
                        Dim intArticoliDelete As Integer
                        Dim intArticoliReturn As Integer

                        Dim IdGridDelete As String = myRow.Cells(0).Text

                        oAccertamentoDelete = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())

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
                                GrdDatiAcc.EditIndex = -1
                                GrdDatiAcc.DataSource = oAccertamentoReturn
                                GrdDatiAcc.DataBind()
                                Session("oAccertatiGriglia") = oAccertamentoReturn
                            Else
                                GrdDatiAcc.EditIndex = -1
                                GrdDatiAcc.DataSource = Nothing
                                GrdDatiAcc.DataBind()
                                GrdDatiAcc.Style.Add("display", "none")
                                Session("oAccertatiGriglia") = Nothing
                            End If
                        Else
                            GrdDatiAcc.EditIndex = -1
                            GrdDatiAcc.DataSource = Nothing
                            GrdDatiAcc.DataBind()
                            GrdDatiAcc.Style.Add("display", "none")
                            Session("oAccertatiGriglia") = Nothing
                        End If
                        Return
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '   Protected Sub GrdDatiAcc_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDatiAcc.ItemDataBound
    'Try
    '	If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '		Dim lblLegame As Label
    '		Dim chkSanzioni As CheckBox
    '		Dim idSanzioni As String
    '		Dim check As CheckBox
    '		Dim ChkInteressi As CheckBox
    '		Dim IdProgressivo As Integer
    '		Dim intXsanzioni As Integer
    '		Dim IdLegame As Integer
    '		Dim obj As OSAPAccertamentoArticolo
    '		Dim objXsanzioni As OSAPAccertamentoArticolo()
    '		Dim bFound As Boolean = False

    '		obj = CType(e.Item.DataItem, OSAPAccertamentoArticolo)

    '		IdLegame = obj.IdLegame
    '		IdProgressivo = obj.Progressivo

    '		lblLegame = e.Item.Cells(16).FindControl("lblLegame")
    '		chkSanzioni = e.Item.Cells(10).FindControl("chkSanzioni")
    '		ChkInteressi = e.Item.Cells(11).FindControl("ChkInteressi")

    '		idSanzioni = obj.Sanzioni
    '		If idSanzioni <> "-1" And idSanzioni <> "" Then
    '			chkSanzioni.Checked = True
    '		End If

    '		intXsanzioni = 0
    '		If Not Session("oAccertatiGriglia") Is Nothing Then
    '			objXsanzioni = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
    '			For intXsanzioni = 0 To objXsanzioni.Length - 1
    '				If objXsanzioni(intXsanzioni).IdLegame = IdLegame And objXsanzioni(intXsanzioni).Progressivo = IdProgressivo Then
    '					bFound = True
    '					Exit For
    '				Else
    '					bFound = False
    '				End If
    '			Next
    '			If Not bFound Then
    '				ReDim Preserve objXsanzioni(objXsanzioni.Length)
    '				objXsanzioni(objXsanzioni.Length - 1) = obj
    '			End If

    '		Else
    '			ReDim Preserve objXsanzioni(intXsanzioni)
    '			objXsanzioni(intXsanzioni) = obj
    '		End If
    '		Session.Add("oAccertatiGriglia", objXsanzioni)
    '		e.Item.Cells(10).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & IdLegame & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
    '		e.Item.Cells(15).Attributes.Add("onClick", "ApriModificaImmobileAnater('" & IdProgressivo & "','" & obj.IdArticolo & "')")
    '		'per il binddata del wuc
    '		Dim ListArticoliDic As New ArrayList
    '		Dim ArticoloSingle As New IRemInterfaceOSAP.Articolo
    '		Dim FncGest As New ClsGestioneAccertamenti
    '		FncGest.OSAPCastProvArtIntoArt(obj, ArticoloSingle)
    '		If Not IsNothing(Session("ListArticoliDic")) Then
    '			ListArticoliDic = Session("ListArticoliDic")
    '		End If
    '		ListArticoliDic.Add(ArticoloSingle)
    '		Session("ListArticoliDic") = ListArticoliDic
    '	End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.GrdDatiAcc_ItemDataBound.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    '   Protected Sub GrdDatiAcc_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDatiAcc.EditCommand
    '	GrdDatiAcc.EditItemIndex = e.Item.ItemIndex
    '	GrdDatiAcc.DataSource = Session("oAccertatiGriglia")
    '	GrdDatiAcc.DataBind()
    'End Sub

    '   Protected Sub GrdDatiAcc_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDatiAcc.UpdateCommand
    '       Dim oAccertamentoUpdate() As OSAPAccertamentoArticolo
    '       Dim oAccertamentoReturn() As OSAPAccertamentoArticolo
    '       Dim intArticoliUpdate As Integer
    '       Dim IdLegameGrid As TextBox = e.Item.Cells(0).FindControl("txtLegame")
    '       Dim IdGridUpdate As String = e.Item.Cells(0).Text
    'Try
    '       If IdLegameGrid.Text = "" Then
    '           dim sScript as string=""
    '           sscript+= "msgLegameVuoto();" & vbCrLf
    '           RegisterScript(sScript , Me.GetType())
    '       Else
    '           oAccertamentoUpdate = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())

    '           Dim arrayListImmobili As New ArrayList
    '           Dim oAccertamentoSingolo As OSAPAccertamentoArticolo
    '           oAccertamentoSingolo = New OSAPAccertamentoArticolo
    '           For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
    '               If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
    '                   oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
    '               End If
    '           Next

    '           GrdDatiAcc.EditItemIndex = -1
    '           GrdDatiAcc.DataSource = oAccertamentoUpdate
    '           GrdDatiAcc.DataBind()
    '           Session("oAccertatiGriglia") = oAccertamentoUpdate
    '       End If
    '   End Sub
    '   Protected Sub GrdDatiAcc_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDatiAcc.CancelCommand
    '       If Not Session("oAccertatiGriglia") Is Nothing Then
    '           GrdDatiAcc.EditItemIndex = -1
    '           GrdDatiAcc.DataSource = Session("oAccertatiGriglia")
    '           GrdDatiAcc.DataBind()
    '       End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.GrdDatiAcc_Update.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '   End Sub

    '   Sub GrdDatiAcc_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDatiAcc.DeleteCommand
    '       Dim oAccertamentoDelete() As OSAPAccertamentoArticolo
    '       Dim oAccertamentoReturn() As OSAPAccertamentoArticolo
    '       Dim intArticoliDelete As Integer
    '       Dim intArticoliReturn As Integer

    '       Dim IdGridDelete As String = e.Item.Cells(0).Text

    '       oAccertamentoDelete = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
    'Try
    '       intArticoliReturn = -1
    '       If Not oAccertamentoDelete Is Nothing Then
    '           For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
    '               If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
    '                   intArticoliReturn += 1
    '                   ReDim Preserve oAccertamentoReturn(intArticoliReturn)
    '                   oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
    '               End If
    '           Next

    '           If Not oAccertamentoReturn Is Nothing Then
    '               GrdDatiAcc.EditItemIndex = -1
    '               GrdDatiAcc.DataSource = oAccertamentoReturn
    '               GrdDatiAcc.DataBind()
    '               Session("oAccertatiGriglia") = oAccertamentoReturn
    '           Else
    '               GrdDatiAcc.EditItemIndex = -1
    '               GrdDatiAcc.DataSource = Nothing
    '               GrdDatiAcc.DataBind()
    '               GrdDatiAcc.Style.Add("display", "none")
    '               Session("oAccertatiGriglia") = Nothing
    '           End If
    '       Else
    '           GrdDatiAcc.EditItemIndex = -1
    '           GrdDatiAcc.DataSource = Nothing
    '           GrdDatiAcc.DataBind()
    '           GrdDatiAcc.Style.Add("display", "none")
    '           Session("oAccertatiGriglia") = Nothing
    '       End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.GrdDatiAcc_DeleteCommand.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '   End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ChkInteressi_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ck1 As CheckBox = CType(sender, CheckBox)
        Dim oAccertato() As OSAPAccertamentoArticolo
        Try
            oAccertato = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
            For Each myRow As GridViewRow In GrdDatiAcc.Rows
                For Each myAcc As OSAPAccertamentoArticolo In oAccertato
                    If myAcc.IdLegame = CInt(CType(myRow.FindControl("hfIdLegame"), HiddenField).Value) Then
                        myAcc.Calcola_Interessi = CType(myRow.FindControl("ChkInteressi"), CheckBox).Checked
                        Exit For
                    End If
                Next
            Next
            Session("oAccertatiGriglia") = oAccertato
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.ChkInteressi_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per la creazione dell'accertamento
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>   
    Private Sub btnAccertamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
        Dim oAccertato() As OSAPAccertamentoArticolo
        Dim oDichiarato() As OSAPAccertamentoArticolo
        Dim intAccertato, intDichiarato As Integer
        Dim sDescTipoAvviso, sScript As String
        Dim a As OggettoAttoOSAP
        Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
        Dim objHashTable As Hashtable

        Try
            Dim oDettaglioAnagrafica As New AnagInterface.DettaglioAnagrafica

            oDettaglioAnagrafica = Session("codContribuente")
            objHashTable = Session("HashTableRettificaAccertamenti")
            oAccertato = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
            oDichiarato = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())

            If Not oAccertato Is Nothing Then
                'controllo presenza legame Accertato
                For intAccertato = 0 To oAccertato.Length - 1
                    If oAccertato(intAccertato).IdLegame = 0 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                Next
            End If
            If Not oDichiarato Is Nothing Then
                'controllo presenza legame Dichiarato
                Log.Debug("ho " & oDichiarato.Length.ToString & " posizioni dichiarate")
                For intDichiarato = 0 To oDichiarato.Length - 1
                    If oDichiarato(intDichiarato).IdLegame = 0 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                Next
            End If
            If Not oAccertato Is Nothing Then
                'controllo coerenza date Accertato
                Dim bError As Boolean = False
                Log.Debug("ho " & oAccertato.Length.ToString & " posizioni accertate")
                For intAccertato = 0 To oAccertato.Length - 1
                    'se l'anno della data fine<>anno accertamento restituisco l'errore
                    oAccertato(intAccertato).Anno = objHashTable("ANNOACCERTAMENTO")
                    If oAccertato(intAccertato).DataInizioOccupazione.Year > CInt(objHashTable("ANNOACCERTAMENTO")) Then
                        bError = True
                    ElseIf oAccertato(intAccertato).DataInizioOccupazione.Year < CInt(objHashTable("ANNOACCERTAMENTO")) Then
                        oAccertato(intAccertato).DataInizioOccupazione = CDate("01/01/" & oAccertato(intAccertato).Anno)
                    End If
                    'Forzo la data fine a fine anno accertamento 
                    If oAccertato(intAccertato).DataFineOccupazione = Date.MinValue Or oAccertato(intAccertato).DataFineOccupazione.ToString() = "" Then
                        oAccertato(intAccertato).DataFineOccupazione = CDate("31/12/" & oAccertato(intAccertato).Anno)
                    End If
                Next
                If bError Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Verificare che la data Inizio Immobile sia coerente con l\'anno di accertamento!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
            End If

            a = FncAnnulloNoAcc.OSAPConfrontoAccertatoDichiarato(ConstSession.IdEnte, objHashTable("CODCONTRIBUENTE"), objHashTable("ANNOACCERTAMENTO"), oDichiarato, oAccertato, CType(Session("HashTableRettificaAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableRettificaAccertamenti"), Session("DataSetSanzioni"), odettaglioanagrafica.datamorte, sDescTipoAvviso, sScript)
            If IsNothing(a) Then
                Throw New Exception("Errore in calcolo accertamento")
            End If
            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "btnAccertamento", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, a.ID_PROVVEDIMENTO)
            Session("TipoAvviso") = sDescTipoAvviso
            If sScript <> "" Then
                RegisterScript(sScript, Me.GetType())
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.btnAccertamento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnAccertamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
    '    Dim oAccertato() As OSAPAccertamentoArticolo
    '    Dim oDichiarato() As OSAPAccertamentoArticolo
    '    Dim intAccertato, intDichiarato As Integer
    '    Dim sDescTipoAvviso, sScript As String
    '    Dim a As OggettoAttoOSAP
    '    Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
    '    Dim objHashTable As Hashtable

    '    Try
    '        objHashTable = Session("HashTableRettificaAccertamenti")
    '        oAccertato = CType(Session("oAccertatiGriglia"), OSAPAccertamentoArticolo())
    '        oDichiarato = CType(Session("DataSetDichiarazioni"), OSAPAccertamentoArticolo())

    '        If Not oAccertato Is Nothing Then
    '            'controllo presenza legame Accertato
    '            For intAccertato = 0 To oAccertato.Length - 1
    '                If oAccertato(intAccertato).IdLegame = 0 Then
    '                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
    '                    RegisterScript(sScript, Me.GetType())
    '                    Exit Sub
    '                End If
    '            Next
    '        End If
    '        If Not oDichiarato Is Nothing Then
    '            'controllo presenza legame Dichiarato
    '            Log.Debug("ho " & oDichiarato.Length.ToString & " posizioni dichiarate")
    '            For intDichiarato = 0 To oDichiarato.Length - 1
    '                If oDichiarato(intDichiarato).IdLegame = 0 Then
    '                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
    '                    RegisterScript(sScript, Me.GetType())
    '                    Exit Sub
    '                End If
    '            Next
    '        End If
    '        If Not oAccertato Is Nothing Then
    '            'controllo coerenza date Accertato
    '            Dim bError As Boolean = False
    '            Log.Debug("ho " & oAccertato.Length.ToString & " posizioni accertate")
    '            For intAccertato = 0 To oAccertato.Length - 1
    '                'se l'anno della data fine<>anno accertamento restituisco l'errore
    '                oAccertato(intAccertato).Anno = objHashTable("ANNOACCERTAMENTO")
    '                If oAccertato(intAccertato).DataInizioOccupazione.Year > CInt(objHashTable("ANNOACCERTAMENTO")) Then
    '                    bError = True
    '                ElseIf oAccertato(intAccertato).DataInizioOccupazione.Year < CInt(objHashTable("ANNOACCERTAMENTO")) Then
    '                    oAccertato(intAccertato).DataInizioOccupazione = CDate("01/01/" & oAccertato(intAccertato).Anno)
    '                End If
    '                'Forzo la data fine a fine anno accertamento 
    '                If oAccertato(intAccertato).DataFineOccupazione = Date.MinValue Or oAccertato(intAccertato).DataFineOccupazione.ToString() = "" Then
    '                    oAccertato(intAccertato).DataFineOccupazione = CDate("31/12/" & oAccertato(intAccertato).Anno)
    '                End If
    '            Next
    '            If bError Then
    '                sScript = "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Verificare che la data Inizio Immobile sia coerente con l\'anno di accertamento!');"
    '                RegisterScript(sScript, Me.GetType())
    '                Exit Sub
    '            End If
    '        End If

    '        a = FncAnnulloNoAcc.OSAPConfrontoAccertatoDichiarato(ConstSession.IdEnte, objHashTable("CODCONTRIBUENTE"), objHashTable("ANNOACCERTAMENTO"), oDichiarato, oAccertato, CType(Session("HashTableRettificaAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableRettificaAccertamenti"), Session("DataSetSanzioni"), sDescTipoAvviso, sScript)
    '        If Not IsNothing(a) Then
    '            Throw New Exception("Errore in calcolo accertamento")
    '        End If
    '        Session("TipoAvviso") = sDescTipoAvviso
    '        If sScript <> "" Then
    '            RegisterScript(sScript, Me.GetType())
    '        End If

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertatoOSAP.btnAccertamento_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
