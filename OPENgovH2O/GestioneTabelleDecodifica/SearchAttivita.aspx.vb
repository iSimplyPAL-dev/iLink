Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchAttivita
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchAttivita))


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
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            RegisterScript("document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';", Me.GetType())

            Dim paginacomandi As String = Request("paginacomandi")
            If Len(paginacomandi) = 0 Then
                paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/ComandiGestioneTabelle.aspx"
            End If
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            Dim sScript As String = ""
            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            RegisterScript(sScript, Me.GetType())

            Dim DBAttivita As New TabelleDiDecodifica.DBAttivita()
            Dim dv As DataView = DBAttivita.GetListaAttivita
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    GrdAttivita.Visible = False
                Else
                    GrdAttivita.Visible = True
                    GrdAttivita.DataSource = dv
                    GrdAttivita.DataBind()
                End If
                Session("vistaAttivita") = dv
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchAttivita.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '      Try

    '          Dim lngRecordCount As Long

    '          dim sScript as string=""()
    '          sScript +="")
    '          sScript +="document.getElementById('hdEnteAppartenenza').value='" & Session("CODCOMUNEENTE") & "';")
    '          sScript +="")
    '          RegisterScript("Hidden", strBuilderHidden.ToString())


    '          Dim paginacomandi As String = Request("paginacomandi")
    '          Dim parametri As String
    '          parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"


    '          dim sScript as string=""
    '          
    '          sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';")
    '          sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';")
    '          
    '          RegisterScript("Comandi", strBuilderComandi.ToString())

    '          Dim DBAttivita As TabelleDiDecodifica.DBAttivita = New TabelleDiDecodifica.DBAttivita()
    '          Dim GetListaAttivita As TabelleDiDecodifica.GetLista = DBAttivita.GetListaAttivita


    '          lngRecordCount = GetListaAttivita.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La ricerca non ha prodotto risultati"
    '              Case Is > 0

    '                  GrdAttivita .sqlComm = GetListaAttivita.oComm
    '                  GrdAttivita .cnnConn = GetListaAttivita.oConn
    '                  GrdAttivita ._NumberRecord = lngRecordCount
    '                  GrdAttivita .DataKeyField = "IDTIPOATTIVITA"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdAttivita .Rows.Count = 0
    '                      GrdAttivita .BindData()
    '                  End If

    '                  GrdAttivita .MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                CalPageaspx(IDRow)
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdAttivita_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdAttivita.SelectedIndexChanged
    '    CalPageaspx(GrdAttivita.DataKeys.Item(GrdAttivita.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAttivita.DataSource = CType(Session("vistaAttivita"), DataView)
            If page.HasValue Then
                GrdAttivita.PageIndex = page.Value
            End If
            GrdAttivita.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="IDTIPOATTIVITA">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal IDTIPOATTIVITA As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Attivita" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""

        'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        'sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sScript += "iframetabella.location.href='Attivita.aspx?IDTIPOATTIVITA=" & IDTIPOATTIVITA & "&paginacomandi=" & Request("paginacomandi") & "';"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' pulsante per l'inserimento di una nuova posizione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click

        Dim _const As New Costanti
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Attivita" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""

        'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sScript += "iframetabella.location.href='Attivita.aspx?IDTIPOATTIVITA=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
        RegisterScript(sScript, Me.GetType())
    End Sub
End Class
