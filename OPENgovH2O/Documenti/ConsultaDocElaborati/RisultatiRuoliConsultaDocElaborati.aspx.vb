Imports Utility
Imports Anagrafica
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Partial Class RisultatiRuoliConsultaDocElaborati
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

    Private Shared Log As ILog = LogManager.GetLogger("RisultatiRuoliConsultaDocElaborati1")
    Private DataFattura As String = ""
    'Private sTipoRuolo As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sCodiceEnte As String
        '********************************************************
        'eseguo la ricerca e carico la griglia
        '********************************************************
        Dim ListRuoliGenerati() As ObjTotRuoloFatture
        Dim oclsElabDoc As New ClsElaborazioneDocumenti
        Dim clsModDate As New ClsGenerale.Generale

        Try
            '********************************************************
            'leggo i parametri di ricerca
            '********************************************************
            DataFattura = Server.UrlDecode(Request.Item("DataFattura"))
            'sTipoRuolo = Server.UrlDecode(Request.Item("TipoRuolo"))
            sCodiceEnte = ConstSession.IdEnte

            If Page.IsPostBack = False Then

                viewstate("SortKey") = "sDataFattura"
                ViewState("OrderBy") = TipoOrdinamento.Decrescente

                '********************************************************
                'dalla query ottengo un'array di oggetti di tipo ObjTotRuolo
                'che assegno direttamente al datasource della griglia
                '********************************************************
                ListRuoliGenerati = oclsElabDoc.GetRuoliconDocElaborati(sCodiceEnte, clsModDate.GiraDataFromDB(DataFattura), 1)
                '********************************************************
                ' ordino per anno 
                '********************************************************
                Dim objComparer As New Comparatore(New String() {"tDataEmissioneFattura"}, New Boolean() {ViewState("OrderBy")})
                If Not ListRuoliGenerati Is Nothing Then
                    Array.Sort(ListRuoliGenerati, objComparer)
                    '********************************************************
                    'carico in una variabile di sessione l'array di oggetti in modo
                    'da non dover rieseguire la query tutte le volte che viene ricaricata la pagina
                    '********************************************************
                    GrdDocumentiElaborati.DataSource = ListRuoliGenerati
                    Session.Add("ListRuoliGenerati", ListRuoliGenerati)

                    GrdDocumentiElaborati.DataBind()
                End If
            Else
                '********************************************************
                'assegno al datasource la variabile di sessione precedentemente caricata
                '********************************************************
                If Not Session("ListRuoliGenerati") Is Nothing Then
                    GrdDocumentiElaborati.DataSource = CType(Session("ListRuoliGenerati"), ObjTotRuoloFatture())
                    GrdDocumentiElaborati.DataBind()
                End If
            End If

            Select Case CInt(GrdDocumentiElaborati.Rows.Count)
                Case 0
                    GrdDocumentiElaborati.Visible = False
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                Case Is > 0
                    GrdDocumentiElaborati.Visible = True
            End Select

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiRuoliConsultaDocElaborati.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
            Return ""
        Else
            Return tDataGrd.ToShortDateString.ToString
        End If
    End Function

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Session("idFlussoRuolo") = CInt(IDRow)

                Dim sScript As String
                'sScript = "parent.parent.Visualizza.location.href='../Documenti/ViewDocumentiElaborati.aspx?Provenienza='1'&';" & vbCrLf
                'sScript = "loadGridDocumenti.location.href='ConsultaDocElaborati.aspx?IdFlussoRuolo=" & Session("idFlussoRuolo") & "';" & vbCrLf
                sScript = "CaricaDocEffettivi();"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiRuoliConsultaDocElaborati.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdDocumentiElaborati_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdDocumentiElaborati.SelectedIndexChanged
    '    '****************************************
    '    'devo aprire la videata dove sono presenti l'elenco dettagliato dei doc
    '    '****************************************
    '    Session("idFlussoRuolo") = CInt(GrdDocumentiElaborati.SelectedItem.Cells(4).Text)

    '    Dim sScript As String
    '    'sScript = "parent.parent.Visualizza.location.href='../Documenti/ViewDocumentiElaborati.aspx?Provenienza='1'&';" & vbCrLf
    '    'sScript = "loadGridDocumenti.location.href='ConsultaDocElaborati.aspx?IdFlussoRuolo=" & Session("idFlussoRuolo") & "';" & vbCrLf
    '    RegisterScript(sScript , Me.GetType())"apri_doc", "CaricaDocEffettivi();")
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdDocumentiElaborati.DataSource = CType(Session("ListRuoliGenerati"), ObjTotRuoloFatture())
            If page.HasValue Then
                GrdDocumentiElaborati.PageIndex = page.Value
            End If
            GrdDocumentiElaborati.DataBind()
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiRuoliConsultaDocElaborati.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
