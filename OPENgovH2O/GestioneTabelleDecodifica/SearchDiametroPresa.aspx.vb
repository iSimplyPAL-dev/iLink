Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchDiametroPresa
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchDiametroPresa))
    Dim DBDiametroPresa As TabelleDiDecodifica.DBDiametroPresa = New TabelleDiDecodifica.DBDiametroPresa()
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

    End Sub

#End Region

    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' Configurazione delle diverse tipologie di diametro della presa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())

            'Dim paginacomandi As String = Request("paginacomandi")
            'Dim parametri As String
            'parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            'dim sScript as string=""
            '
            'sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            'sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            '
            'RegisterScript(sScript , Me.GetType())

            Dim dv As DataView = DBDiametroPresa.GetListaDiametroPresa
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    GrdDiametroPresa.Visible = False
                Else
                    GrdDiametroPresa.Visible = True
                    GrdDiametroPresa.DataSource = dv
                    GrdDiametroPresa.DataBind()
                End If
                Session("vistaDiametroPresa") = dv
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDiametroPresa.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '      Try

    '          Dim lngRecordCount As Long

    '          dim sScript as string=""()
    '          sScript +="")
    '          sScript +="document.getElementById('hdEnteAppartenenza').value='" & Session("CODCOMUNEENTE") & "';"
    '          sScript +="")
    '          RegisterScript(sscript,me.gettype())


    '          Dim paginacomandi As String = Request("paginacomandi")
    '          Dim parametri As String
    '          parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"


    '          dim sScript as string=""
    '          
    '          sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';"
    '          sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';"
    '          
    '          RegisterScript(sScript , Me.GetType())


    '          Dim GetListaDiametroPresa As TabelleDiDecodifica.GetLista = DBDiametroPresa.GetListaDiametroPresa


    '          lngRecordCount = GetListaDiametroPresa.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La ricerca non ha prodotto risultati"
    '              Case Is > 0

    '                  GrdDiametroPresa.sqlComm = GetListaDiametroPresa.oComm
    '                  GrdDiametroPresa.cnnConn = GetListaDiametroPresa.oConn
    '                  GrdDiametroPresa._NumberRecord = lngRecordCount
    '                  GrdDiametroPresa.DataKeyField = "CODDIAMETROPRESA"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdDiametroPresa.Rows.Count = 0
    '                      GrdDiametroPresa.BindData()
    '                  End If

    '                  GrdDiametroPresa.MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDiametroPresa.Page_Load.errore: ", ex)
    '         Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub
    '*******************************************************
    '
    ' GrdDiametroPresa_SelectedIndexChanged
    '
    ' Richiamata dalla Griglia GrdDiametroPresa per effettuare un  redirect 
    ' 
    '
    '*******************************************************

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                CalPageaspx(IDRow)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDiametroPresa.SearchDiametroPresa.errore: ", ex)
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
    'Public Sub GrdDiametroPresa_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdDiametroPresa.SelectedIndexChanged
    '    CalPageaspx(GrdDiametroPresa.DataKeys.Item(GrdDiametroPresa.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdDiametroPresa.DataSource = CType(Session("vistaDiametroPresa"), DataView)
            If page.HasValue Then
                GrdDiametroPresa.PageIndex = page.Value
            End If
            GrdDiametroPresa.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDiametroPresa.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="CODDIAMETROPRESA">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal CODDIAMETROPRESA As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Diametro Presa" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        dim sScript as string=""
        Try
            
            'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            'sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="iframetabella.location.href='DiametroPresa.aspx?CODDIAMETROPRESA=" & CODDIAMETROPRESA & "&paginacomandi=" & Request("paginacomandi") & "';"
            

            RegisterScript(sScript , Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDiametroPresa.CalPageaspx.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per l'inserimento di una nuova posizione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        Try
            Dim _const As New Costanti
            Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Diametro Presa" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

            dim sScript as string=""

            
            'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="iframetabella.location.href='DiametroPresa.aspx?CODDIAMETROPRESA=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
            

            RegisterScript(sScript , Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDiametroPresa.btnNuovo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Protected Function GetImporto(ByVal prdStatus As Object) As String
    '    GetImporto = DBDiametroPresa.GetImporto(MyUtility.CIdFromDB(prdStatus))
    '    Return GetImporto
    'End Function
End Class
