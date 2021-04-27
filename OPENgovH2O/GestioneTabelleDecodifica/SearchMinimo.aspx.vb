Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchMinimo
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchMinimo))

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

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '      Try
    '          Dim lngRecordCount As Long

    '          dim sScript as string=""()
    '          sScript +="")
    '          sScript +="document.getElementById('hdEnteAppartenenza').value='" & Session("CODCOMUNEENTE") & "';")
    '          sScript +="")
    '          RegisterScript(sscript,me.gettype())


    '          Dim paginacomandi As String = Request("paginacomandi")
    '          Dim parametri As String
    '          parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"


    '          dim sScript as string=""
    '          
    '          sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';")
    '          sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';")
    '          
    '          RegisterScript(sScript , Me.GetType())

    '          Dim DBMinimo As TabelleDiDecodifica.DBMinimiFatturabili = New TabelleDiDecodifica.DBMinimiFatturabili()
    '          Dim GetListaMinimi As TabelleDiDecodifica.GetLista = DBMinimo.GetListaMinimiFatturabili


    '          lngRecordCount = GetListaMinimi.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La tabella in oggetto non contiene dati"
    '              Case Is > 0

    '                  GrdMinimo.sqlComm = GetListaMinimi.oComm
    '                  GrdMinimo.cnnConn = GetListaMinimi.oConn
    '                  GrdMinimo._NumberRecord = lngRecordCount
    '                  GrdMinimo.DataKeyField = "IDMINIMO"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdMinimo.Rows.Count = 0
    '                      GrdMinimo.BindData()
    '                  End If

    '                  GrdMinimo.MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '              Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchMinimo.Page_Load.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript = "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            RegisterScript(sScript, Me.GetType())

            Dim DBMinimo As New TabelleDiDecodifica.DBMinimiFatturabili()
            Dim dv As DataView = DBMinimo.GetListaMinimiFatturabili(-1)
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    lblMessage.Text = "La tabella in oggetto non contiene dati"
                    GrdMinimo.Visible = False
                Else
                    GrdMinimo.Visible = True
                    GrdMinimo.DataSource = dv
                    GrdMinimo.DataBind()
                End If
                Session("vistaMinimo") = dv
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchMinimo.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                CalPageaspx(IDRow)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchMinimo.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdMinimo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdMinimo.SelectedIndexChanged
    '    'Ridirige la pagina nella gestione nuovo TITOLO SOGGETTO passando come parametro L' IDTITOLOSOGGETTO selezionato
    '    CalPageaspx(GrdMinimo.DataKeys.Item(GrdMinimo.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdMinimo.DataSource = CType(Session("vistaMinimo"), DataView)
            If page.HasValue Then
                GrdMinimo.PageIndex = page.Value
            End If
            GrdMinimo.DataBind()
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchMinimo.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="IDMINIMO">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal IDMINIMO As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Minimi Fatturabili" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""
        Try

            'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            'sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sScript += "iframetabella.location.href='Minimi.aspx?IDMINIMO=" & IDMINIMO & "&paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchMinimo.CalPageaspx.errore: ", ex)
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

        Dim _const As New Costanti
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica  Minimi Fatturabili" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""
        Try
            'sscript+="parent.Comandi.location.href='" & "/OpenUtenzeGC/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sScript += "iframetabella.location.href='Minimi.aspx?IDMINIMO=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchMinimo.btnNuovo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
