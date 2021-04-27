Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchTipiImpianto
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchTipiImpianto))

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
    '          paginacomandi = paginacomandi.Replace("ComandiGestioneTabelle.aspx", "ComandiGestioneTabelleSistema.aspx")
    '          Dim parametri As String
    '          parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"


    '          dim sScript as string=""
    '          
    '          sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';")
    '          sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';")
    '          
    '          RegisterScript(sScript , Me.GetType())

    '          Dim DBTipiImpianto As TabelleDiDecodifica.DBImpianti = New TabelleDiDecodifica.DBImpianti()
    '          Dim GetListaImpianti As TabelleDiDecodifica.GetLista = DBTipiImpianto.GetListaImpianti


    '          lngRecordCount = GetListaImpianti.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La ricerca non ha prodotto risultati"
    '              Case Is > 0

    '                  GrdTipiImpianto.sqlComm = GetListaImpianti.oComm
    '                  GrdTipiImpianto.cnnConn = GetListaImpianti.oConn
    '                  GrdTipiImpianto._NumberRecord = lngRecordCount
    '                  GrdTipiImpianto.DataKeyField = "IDIMPIANTO"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdTipiImpianto.Rows.Count = 0
    '                      GrdTipiImpianto.BindData()
    '                  End If

    '                  GrdTipiImpianto.MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiImpianto.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' Codifica per la Descrizione del tipo impianto per singolo comune gestito dalla piattaforma
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

                Dim paginacomandi As String = Request("paginacomandi")
                If Len(paginacomandi) = 0 Then
                    paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/GestioneTabelleDecodifica/ComandiGestioneTabelleSistema.aspx"
                Else
                    paginacomandi = paginacomandi.Replace("ComandiGestioneTabelle.aspx", "ComandiGestioneTabelleSistema.aspx")
                End If
                Dim parametri As String
                parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

                sScript = "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())

                Dim DBTipiImpianto As New TabelleDiDecodifica.DBImpianti()
                Dim dv As DataView = DBTipiImpianto.GetListaImpianti
                If Not IsNothing(dv) Then
                    If dv.Count <= 0 Then
                        lblMessage.Text = "La ricerca non ha prodotto risultati"
                        GrdTipiImpianto.Visible = False
                    Else
                        GrdTipiImpianto.Visible = True
                        GrdTipiImpianto.DataSource = dv
                        GrdTipiImpianto.DataBind()
                    End If
                End If
                Session("vistaImpianti") = dv
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiImpianto.Page_Load.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiImpianto.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdTipiImpianto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdTipiImpianto.SelectedIndexChanged
    '    CalPageaspx(GrdTipiImpianto.DataKeys.Item(GrdTipiImpianto.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdTipiImpianto.DataSource = CType(Session("vistaImpianti"), DataView)
            If page.HasValue Then
                GrdTipiImpianto.PageIndex = page.Value
            End If
            GrdTipiImpianto.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiImpianto.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="IDIMPIANTO">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal IDIMPIANTO As Integer)
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Tipi Impianto" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
        Dim sScript As String = ""
        Dim paginaComandi As String
        Try
            paginaComandi = CType(Request("paginacomandi"), String).Replace("ComandiOperazioniSuTabelle.aspx", "ComandiOperazioniSuTabelleSistema.aspx")

            'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelleSistema.aspx" & parametri & "';"
            sScript += "iframetabella.location.href='TipiImpianto.aspx?IDIMPIANTO=" & IDIMPIANTO & "&paginacomandi=" & paginaComandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiImpianto.CalPageaspx.errore: ", ex)
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
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Tipi Impianto" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""
        Try
            'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            sScript += "parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sScript += "iframetabella.location.href='TipiImpianto.aspx?IDIMPIANTO=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchTipiImpianto.btnNuovo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
