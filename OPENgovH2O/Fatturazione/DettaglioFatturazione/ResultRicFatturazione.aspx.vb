Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class ResultRicFatturazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicFatturazione))
    Protected oReplace As New ClsGenerale.Generale

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

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Try
    '        Dim FunctionGest As New ClsFatture
    '        Dim oMyRicerca As New ObjRicercaDoc
    '        Dim oListAnagDoc() As ObjAnagDocumenti
    '        Dim oMyTotali As New ObjTotalizzatoriDocumenti
    '        Dim WFSessione As OPENUtility.CreateSessione
    '        Dim WFErrore As String

    '        If Page.IsPostBack = False Then
    '            TxtProvenienza.Text = Request.Item("Provenienza")
    '            oMyRicerca.sEnte = ConstSession.IdEnte
    '            If Request.Item("DdlPeriodo") <> "" Then
    '                oMyRicerca.nPeriodo = Request.Item("DdlPeriodo")
    '            End If
    '            oMyRicerca.sTipoDocumento = Request.Item("DdlTipoDoc")
    '            oMyRicerca.sCognome = Request.Item("TxtCognome")
    '            oMyRicerca.sNome = Request.Item("TxtNome")
    '            oMyRicerca.sCFPIva = Request.Item("TxtCfPIva")
    '            If Request.Item("TxtDataDoc") <> "" Then
    '                oMyRicerca.tDataDocumento = Request.Item("TxtDataDoc")
    '            End If
    '            oMyRicerca.sNDocumento = Request.Item("TxtNDoc")
    '            oMyRicerca.sMatricola = Request.Item("TxtMatricola")
    '            oMyRicerca.sProvenienza = TxtProvenienza.Text
    '            Session("oRicercaDoc") = oMyRicerca
    '            Try
    '                'inizializzo la connessione
    '                WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '                If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '                    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '                End If

    '                oListAnagDoc = FunctionGest.GetRicercaDocumenti(oMyRicerca, WFSessione, TxtProvenienza.Text)
    '                If Not oListAnagDoc Is Nothing Then
    '                    'calcolo i totalizzatori
    '                    oMyTotali = FunctionGest.GetTotaliRicDoc(oMyRicerca, WFSessione)
    '                    LblNContribuenti.Text = FormatNumber(oMyTotali.nContribuenti, 0)
    '                    LblNFatture.Text = FormatNumber(oMyTotali.nFatture, 0)
    '                    LblNNote.Text = FormatNumber(oMyTotali.nNote, 0)
    '                    LblImpFatture.Text = FormatNumber(oMyTotali.impFatture, 2)
    '                    LblImpNote.Text = FormatNumber(oMyTotali.impNote, 2)
    '                    'popolo la griglia
    '                    GrdDocumenti.DataSource = oListAnagDoc
    '                    GrdDocumenti.start_index = GrdDocumenti.CurrentPageIndex
    '                    GrdDocumenti.DataBind()
    '                    GrdDocumenti.SelectedIndex = -1
    '                Else
    '                    LblResult.Text = "La ricerca non ha prodotto risultati."
    '                    LblResult.Style.Add("display", "")
    '                End If
    '            Catch Err As Exception
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.Page_Load.errore: ", Err)
    '                Response.Redirect("../../PaginaErrore.aspx")
    '            Finally
    '                WFSessione.kill()
    '            End Try
    '            Session("oAnagDocumenti") = oListAnagDoc
    '        Else
    '            GrdDocumenti.AllowCustomPaging = False
    '            oListAnagDoc = CType(Session("oAnagDocumenti"), ObjAnagDocumenti())
    '            GrdDocumenti.Rows.Count = oListAnagDoc.Length
    '            GrdDocumenti.DataSource = oListAnagDoc
    '            GrdDocumenti.start_index = GrdDocumenti.CurrentPageIndex.ToString()
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim FunctionGest As New ClsFatture
            Dim oMyRicerca As New ObjRicercaDoc
            Dim oListAnagDoc() As ObjAnagDocumenti
            Dim oMyTotali As New ObjTotalizzatoriDocumenti

            oListAnagDoc = Nothing
            If Page.IsPostBack = False Then
                TxtProvenienza.Text = Request.Item("Provenienza")
                Log.Debug(".GrdRowCommand::errore::REQUEST.ITEM:" + Request.Item("Provenienza"))
                oMyRicerca.sEnte = ConstSession.IdEnte
                If Request.Item("DdlPeriodo") <> "" Then
                    oMyRicerca.nPeriodo = Request.Item("DdlPeriodo")
                End If
                oMyRicerca.sTipoDocumento = Request.Item("DdlTipoDoc")
                oMyRicerca.sCognome = Request.Item("TxtCognome")
                oMyRicerca.sNome = Request.Item("TxtNome")
                oMyRicerca.sCFPIva = Request.Item("TxtCfPIva")
                If Request.Item("TxtDataDoc") <> "" Then
                    oMyRicerca.tDataDocumento = Request.Item("TxtDataDoc")
                End If
                oMyRicerca.sNDocumento = Request.Item("TxtNDoc")
                oMyRicerca.sMatricola = Request.Item("TxtMatricola")
                oMyRicerca.sProvenienza = TxtProvenienza.Text
                Session("oRicercaDoc") = oMyRicerca
                Try
                    oListAnagDoc = FunctionGest.GetRicercaDocumenti(oMyRicerca, TxtProvenienza.Text)
                    If Not oListAnagDoc Is Nothing Then
                        'calcolo i totalizzatori
                        oMyTotali = FunctionGest.GetTotaliRicDoc(oMyRicerca)
                        LblNContribuenti.Text = FormatNumber(oMyTotali.nContribuenti, 0)
                        LblNFatture.Text = FormatNumber(oMyTotali.nFatture, 0)
                        LblNNote.Text = FormatNumber(oMyTotali.nNote, 0)
                        LblImpFatture.Text = FormatNumber(oMyTotali.impFatture, 2)
                        LblImpNote.Text = FormatNumber(oMyTotali.impNote, 2)
                        'popolo la griglia
                        GrdDocumenti.DataSource = oListAnagDoc
                        GrdDocumenti.DataBind()
                        GrdDocumenti.SelectedIndex = -1
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                    End If
                Catch Err As Exception

                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.Page_Load.errore: ", Err)
                    Response.Redirect("../../PaginaErrore.aspx")
                End Try
                Session("oAnagDocumenti") = oListAnagDoc
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"

    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdDocumenti.Rows
                    If IDRow = CType(myRow.FindControl("hfnIdDocumento"), HiddenField).Value Then
                        Dim sIdDocumento, sProvenienza As String
                        sIdDocumento = CType(myRow.FindControl("hfnIdDocumento"), HiddenField).Value
                        sProvenienza = CType(myRow.FindControl("hfsVariato"), HiddenField).Value
                        If sProvenienza = "" Or sProvenienza = "&nbsp;" Or sProvenienza.ToUpper = "X" Then
                            sProvenienza = TxtProvenienza.Text
                        Else
                            sProvenienza = "C"
                        End If
                        Dim sScript As String = "DettaglioDocumento('" & sIdDocumento & "','" & sProvenienza & "');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'aggiunta per test
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        Dim IDRow As String = e.CommandArgument.ToString()
    '        If e.CommandName = "RowOpen" Then
    '            For Each myRow As GridViewRow In GrdDocumenti.Rows

    '                If IDRow = CType(myRow.FindControl("hfnIdDocumento"), HiddenField).Value Then
    '                    Dim sIdDocumento, sProvenienza As String
    '                    sIdDocumento = CType(myRow.FindControl("hfnIdDocumento"), HiddenField).Value
    '                    sProvenienza = CType(myRow.FindControl("hfsVariato"), HiddenField).Value
    '                    If sProvenienza = "" Or sProvenienza = "&nbsp;" Or sProvenienza.ToUpper = "X" Then
    '                        sProvenienza = TxtProvenienza.Text
    '                        Log.Debug(".GrdRowCommand::errore::IF" + sProvenienza)
    '                    Else
    '                        sProvenienza = "C"
    '                        Log.Debug(".GrdRowCommand::errore::ELSE" + sProvenienza)
    '                    End If
    '                    myRow.Attributes.Add("onClick", "DettaglioDocumento('" & sIdDocumento & "','" & sProvenienza & "')")
    '                End If
    '            Next
    '        ElseIf e.CommandName = "RowDelete" Then
    '            'If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "Grd" Then
    '            '    For Each myRow As GridViewRow In GrdDocumenti.Rows
    '            '        If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then

    '            '        End If
    '            '    Next
    '            'End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(".GrdRowCommand::errore::", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub




    'Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    Try
    '        If (e.Row.RowType = DataControlRowType.DataRow) Then
    '            Dim sIdDocumento, sProvenienza As String
    '            sIdDocumento = CType(e.Row.FindControl("hfnIdDocumento"), HiddenField).Value
    '            sProvenienza = CType(e.Row.FindControl("hfsVariato"), HiddenField).Value
    '            If sProvenienza = "" Or sProvenienza = "&nbsp;" Or sProvenienza.ToUpper = "X" Then
    '                sProvenienza = TxtProvenienza.Text
    '            Else
    '                sProvenienza = "C"
    '            End If
    '            e.Row.Attributes.Add("onClick", "DettaglioDocumento('" & sIdDocumento & "','" & sProvenienza & "')")
    '        End If
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.GrdRowDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub


    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdDocumenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDocumenti.ItemDataBound
    ' Try
    'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella
    '        Dim sIdDocumento, sProvenienza As String
    '        sIdDocumento = e.Item.Cells(10).Text
    '        sProvenienza = e.Item.Cells(11).Text
    '        If sProvenienza = "" Or sProvenienza = "&nbsp;" Or sProvenienza.ToUpper = "X" Then
    '            sProvenienza = TxtProvenienza.Text
    '        Else
    '            sProvenienza = "C"
    '        End If

    '        e.Item.Attributes.Add("onClick", "DettaglioDocumento('" & sIdDocumento & "','" & sProvenienza & "')")
    '    End If
    ' Catch ex As Exception
    '
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.GrdDocumenti_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdDocumenti.DataSource = CType(Session("oAnagDocumenti"), ObjAnagDocumenti())
            If page.HasValue Then
                GrdDocumenti.PageIndex = page.Value
            End If
            GrdDocumenti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    Public Function FormattaBlocco(ByVal sIsVariato As String) As String
        If sIsVariato <> "" Then
            If TxtProvenienza.Text = "E" Then
                Return "..\..\..\images\Bottoni\Lucchetto.png"
            Else
                Return "..\..\..\images\Bottoni\Add.png"
            End If
        Else
                Return ""
            End If
    End Function

    Public Function FormattaToolTipBlocco(ByVal sIsVariato As String) As String
        If sIsVariato <> "" Then
            If TxtProvenienza.Text = "E" Then
                Return "Bloccato da Minuta Approvata"
            Else
                Return "Presenza Nota di Credito"
            End If
        Else
            Return ""
        End If
    End Function
End Class
