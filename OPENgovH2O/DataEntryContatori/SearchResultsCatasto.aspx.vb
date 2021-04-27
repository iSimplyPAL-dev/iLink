Imports log4net
Imports OPENUtility

Partial Class SearchResultsCatasto
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultsCatasto))
        Protected FncGrd As New ClsGenerale.FunctionGrd
    Private DBContatori As New GestContatori

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnEvento As System.Web.UI.WebControls.Button

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
        Dim dv As DataView

        Try
            If CInt(Request.Params("ContatoreID")) <> 0 Then
                'prelevo i dati catastali
                Dim ds As DataSet = DBContatori.getListaCatastali(-1, Request.Params("ContatoreID"))
                Dim dt As DataTable = ds.Tables(0)
                'memorizzo in variabile di sessione
                Session("datacatasto") = dt

                DBContatori.GetTipoParticella(CType(Session("datacatasto"), System.Data.DataTable))
                dt = Session("datacatasto")
                dv = dt.DefaultView

                'Carico il Controllo Nella Tabella
                Select Case dv.Count
                    Case 0
                        lblMessage.Text = "Non sono presenti riferimenti catastali"
                    Case Is > 0
                        If Not Page.IsPostBack Then
                            GrdCatasto.DataSource = dv
                            GrdCatasto.DataBind()
                        Else
                            If Not IsNothing(dv) Then
                                GrdCatasto.DataSource = dv
                            End If
                        End If
                End Select
            Else
                lblMessage.Text = "Non sono presenti riferimenti catastali"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.SearchResultsCatasto.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    GrdCatasto.SelectedIndex = -1

    '    Dim lngRecordCount As Int32
    '    '=====================================
    '    'PROVA GRIGLIA VETTORE
    '    '=====================================
    '    Dim mioData As New DataTable
    '    Dim mioRiga As DataRow
    '    Dim dv As DataView
    '    Dim descTipoParticella As String = String.Empty
    '    Dim i As Integer = 0

    '    '=======================================
    '    'CONTROLLO CONTATORE ESISTENTE O NUOVO
    '    '=======================================
    'Try
    '    If CInt(Request.Params("ContatoreID")) = 0 Then
    '        If Not Page.IsPostBack Then
    '            If IsNothing(Session("datacatasto")) Then
    '                Session("datacatasto") = mioData
    '                dv = mioData.DefaultView
    '            Else
    '                mioData = CType(Session("datacatasto"), System.Data.DataTable)
    '                dv = mioData.DefaultView
    '            End If
    '            If mioData.Columns.Count = 0 Then
    '                mioData.Columns.Add("IDCONT_CATAS")
    '                mioData.Columns.Add("CODCONTATORE")
    '                mioData.Columns.Add("INTERNO")
    '                mioData.Columns.Add("PIANO")
    '                mioData.Columns.Add("FOGLIO")
    '                mioData.Columns.Add("NUMERO")
    '                mioData.Columns.Add("SUBALTERNO")
    '                mioData.Columns.Add("SEZIONE")
    '                mioData.Columns.Add("ESTENSIONE_PARTICELLA")
    '                mioData.Columns.Add("ID_TIPO_PARTICELLA")
    '            End If
    '            GrdCatasto.start_index = 0
    '            GrdCatasto.DataSource = dv
    '            GrdCatasto.DataBind()
    '            lngRecordCount = dv.Count
    '            Select Case lngRecordCount
    '                Case 0
    '                    lblMessage.Text = "Non sono presenti riferimenti catastali"
    '                    GrdCatasto.Visible = False
    '            End Select
    '        Else
    '            mioData = CType(Session("datacatasto"), System.Data.DataTable)
    '            GrdCatasto.AllowCustomPaging = False
    '            GrdCatasto.start_index = Convert.ToString(GrdCatasto.CurrentPageIndex)
    '            dv = mioData.DefaultView
    '            If Not IsNothing(dv) Then
    '                GrdCatasto.Rows.Count = dv.Count
    '                GrdCatasto.DataSource = dv
    '            End If
    '        End If

    '    Else
    '        If Not IsNothing(Session("datacatasto")) Then
    '            DBContatori.GetTipoParticella(CType(Session("datacatasto"), System.Data.DataTable))

    '            mioData = CType(Session("datacatasto"), System.Data.DataTable)

    '            dv = mioData.DefaultView
    '            'Carico il Controllo Nella Tabella
    '            Select Case dv.Count
    '                Case 0
    '                    lblMessage.Text = "Non sono presenti riferimenti catastali"
    '                Case Is > 0
    '                    If Not Page.IsPostBack Then
    '                        GrdCatasto.start_index = 0
    '                        GrdCatasto.Rows.Count = dv.Count
    '                        GrdCatasto.DataSource = dv
    '                        GrdCatasto.DataBind()
    '                    Else
    '                        GrdCatasto.AllowCustomPaging = False
    '                        GrdCatasto.start_index = Convert.ToString(GrdCatasto.CurrentPageIndex)
    '                        If Not IsNothing(dv) Then
    '                            GrdCatasto.Rows.Count = dv.Count
    '                            GrdCatasto.DataSource = dv
    '                        End If
    '                    End If
    '            End Select
    '        Else
    '            If CInt(Request.Params("ContatoreID")) <> 0 Then
    '                'prelevo i dati catastali
    '                'Dim dsCat As New DataSet
    '                Dim dt As DataTable = DBContatori.getListaCatastali(Request.Params("ContatoreID"))
    '                'dv = dt.DefaultView
    '                'memorizzo in variabile di sessione
    '                Session("datacatasto") = dt

    '                'fabi
    '                'dsCat.Tables.Add(dt)
    '                DBContatori.GetTipoParticella(CType(Session("datacatasto"), System.Data.DataTable))
    '                dt = Session("datacatasto")
    '                dv = dt.DefaultView
    '                '/fabi

    '                'Carico il Controllo Nella Tabella
    '                Select Case dv.Count
    '                    Case 0
    '                        lblMessage.Text = "Non sono presenti riferimenti catastali"
    '                    Case Is > 0
    '                        If Not Page.IsPostBack Then
    '                            GrdCatasto.start_index = 0
    '                            GrdCatasto.Rows.Count = dv.Count
    '                            GrdCatasto.DataSource = dv
    '                            GrdCatasto.DataBind()
    '                        Else
    '                            GrdCatasto.AllowCustomPaging = False
    '                            GrdCatasto.start_index = Convert.ToString(GrdCatasto.CurrentPageIndex)
    '                            If Not IsNothing(dv) Then
    '                                GrdCatasto.Rows.Count = dv.Count
    '                                GrdCatasto.DataSource = dv
    '                            End If
    '                        End If
    '                End Select
    '            Else
    '                lblMessage.Text = "Non sono presenti riferimenti catastali"
    '            End If
    '        End If
    '        End If

    '    '===========================================
    '    'FINE GRIGLIA CATASTALI
    '    '===========================================
    '    GrdCatasto.SelectedIndex = -1
    ' Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsCatasto.Page_Load.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                sScript = "ApriModifica(" & IDRow & "," & Request.Params("ContatoreID") & ");"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.GrdRowCommand.errore: ", ex)
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
    'Private Sub GrdCatasto_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdCatasto.ItemDataBound
    ' Try  
    'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Cells(0).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(1).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(2).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(3).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(4).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(5).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(6).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(7).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(8).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '        e.Item.Cells(9).Attributes.Add("Onclick", "ApriModifica(" & e.Item.Cells(1).Text & "," & Request.Params("ContatoreID") & ");")
    '    End If
    'Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsCatasto.GrdCatasto_ItemDataBound.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdCatasto.DataSource = CType(Session("datacatasto"), DataView)
            If page.HasValue Then
                GrdCatasto.PageIndex = page.Value
            End If
            GrdCatasto.DataBind()
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsCatasto.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

End Class
