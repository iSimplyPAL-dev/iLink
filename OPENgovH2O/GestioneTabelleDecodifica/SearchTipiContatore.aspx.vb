Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchTipoContatore
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale


    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchTipoContatore))
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

    '          Dim DBTipoContatore As TabelleDiDecodifica.DBTipoContatore = New TabelleDiDecodifica.DBTipoContatore()
    '          Dim GetListaTipoContatore As TabelleDiDecodifica.GetLista = DBTipoContatore.GetListaTipoContatore


    '          lngRecordCount = GetListaTipoContatore.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La ricerca non ha prodotto risultati"
    '              Case Is > 0

    '                  GrdTipiContatore.sqlComm = GetListaTipoContatore.oComm
    '                  GrdTipiContatore.cnnConn = GetListaTipoContatore.oConn
    '                  GrdTipiContatore._NumberRecord = lngRecordCount
    '                  GrdTipiContatore.DataKeyField = "IDTIPOCONTATORE"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdTipiContatore.Rows.Count = 0
    '                      GrdTipiContatore.BindData()
    '                  End If

    '                  GrdTipiContatore.MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '             Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiContatore.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' Configurazione delle diverse tipologie di un contatore
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
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

                Dim DBTipoContatore As New TabelleDiDecodifica.DBTipoContatore()
                Dim dv As DataView = DBTipoContatore.GetListaTipoContatore
                If Not IsNothing(dv) Then
                    If dv.Count <= 0 Then
                        lblMessage.Text = "La ricerca non ha prodotto risultati"
                        GrdTipiContatore.Visible = False
                    Else
                        GrdTipiContatore.Visible = True
                        GrdTipiContatore.DataSource = dv
                        GrdTipiContatore.DataBind()
                    End If
                End If
                Session("vistaTipiContatore") = dv
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiContatore.Page_Load.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiContatore.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdTipiContatore_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdTipiContatore.SelectedIndexChanged
    '    CalPageaspx(GrdTipiContatore.DataKeys.Item(GrdTipiContatore.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdTipiContatore.DataSource = CType(Session("vistaTipiContatore"), DataView)
            If page.HasValue Then
                GrdTipiContatore.PageIndex = page.Value
            End If
            GrdTipiContatore.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiContatore.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="IDTIPOCONTATORE">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal IDTIPOCONTATORE As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Tipo Contatore" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        dim sScript as string=""
        Try
            
            'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            'sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="iframetabella.location.href='TipoContatore.aspx?IDTIPOCONTATORE=" & IDTIPOCONTATORE & "&paginacomandi=" & Request("paginacomandi") & "';"
            

            RegisterScript(sScript , Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiContatore.CalPageaspx.errore: ", ex)
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

        Dim _const As New Costanti()
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Tipo Contatore" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        dim sScript as string=""
        Try
            
            'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sscript+="iframetabella.location.href='TipoContatore.aspx?IDTIPOCONTATORE=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
            

            RegisterScript(sScript , Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiContatore.btnNuovo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub
End Class
