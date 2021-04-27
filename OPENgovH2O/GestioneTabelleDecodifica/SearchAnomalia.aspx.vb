Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchAnomalia
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchAnomalia))
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
    ''' Configurazione delle diverse tipologie di anomalie associabili ad una lettura
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
            ''parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.IdPeriodo & "&PAG_PREC=STRADE"
            'parametri = "?title=Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            'dim sScript as string=""
            '
            'sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            'sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            '
            'RegisterScript(sScript , Me.GetType())

            Dim DBAnomalie As TabelleDiDecodifica.DBAnomalie = New TabelleDiDecodifica.DBAnomalie()
            Dim dv As DataView = DBAnomalie.GetListaAnomalie
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    GrdAnomalia.Visible = False
                Else
                    GrdAnomalia.Visible = True
                    GrdAnomalia.DataSource = dv
                    GrdAnomalia.DataBind()
                End If
                Session("vistaAnomalia") = dv
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.SearchAnomalia.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ' Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    'Dim lngRecordCount As Long

    'dim sScript as string=""()
    'sScript +="")
    'sScript +="document.getElementById('hdEnteAppartenenza').value='" & Session("CODCOMUNEENTE") & "';"
    'sScript +="")
    'RegisterScript(sscript,me.gettype())


    'Dim paginacomandi As String = Request("paginacomandi")
    '       Dim parametri As String
    '       'parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.IdPeriodo & "&PAG_PREC=STRADE"

    '       parametri = "?title=Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

    '       Dim prova As String
    '       prova = CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")

    '       dim sScript as string=""
    '       
    '       sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';"
    '       sscript+="parent.Comandi.location.href='" & paginacomandi & parametri & "';"
    '       
    '       RegisterScript(sScript , Me.GetType())

    '       Try
    '           Dim DBAnomalie As TabelleDiDecodifica.DBAnomalie = New TabelleDiDecodifica.DBAnomalie()
    '           Dim GetListaAnomalie As TabelleDiDecodifica.GetLista = DBAnomalie.GetListaAnomalie


    '           lngRecordCount = GetListaAnomalie.lngRecordCount

    '           Select Case lngRecordCount
    '               Case 0
    '                   lblMessage.Text = "La ricerca non ha prodotto risultati"
    '               Case Is > 0

    '                   GrdAnomalia.sqlComm = GetListaAnomalie.oComm
    '                   GrdAnomalia.cnnConn = GetListaAnomalie.oConn
    '                   GrdAnomalia._NumberRecord = lngRecordCount
    '                   GrdAnomalia.DataKeyField = "CODANOMALIA"
    '                   'Carico il Controllo Nella Tabella

    '                   If Not Page.IsPostBack Then
    '                       GrdAnomalia.Rows.Count = 0
    '                       GrdAnomalia.BindData()
    '                   End If

    '                   GrdAnomalia.MouseSelectableDataGrid()

    '           End Select
    '          Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchAnomalia.Page_Load.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        End Try
    '   End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                CalPageaspx(IDRow)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchAnomalia.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdAnomalia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdAnomalia.SelectedIndexChanged
    '    CalPageaspx(GrdAnomalia.DataKeys.Item(GrdAnomalia.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAnomalia.DataSource = CType(Session("vistaAnomalia"), DataView)
            If page.HasValue Then
                GrdAnomalia.PageIndex = page.Value
            End If
            GrdAnomalia.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchAnomalia.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="CODANOMALIA">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal CODANOMALIA As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Anomalie" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        dim sScript as string=""

        
        'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        'sscript+="parent.parent.Comandi.location.href='" & ConstSession.PathApplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sscript+="parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sscript+="iframetabella.location.href='Anomalie.aspx?CODANOMALIA=" & CODANOMALIA & "&paginacomandi=" & Request("paginacomandi") & "';"
        

        RegisterScript(sScript , Me.GetType())

    End Sub
    ''' <summary>
    ''' pulsante per l'inserimento di una nuova posizione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click

        Dim _const As New Costanti
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Anomalie" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        dim sScript as string=""

        
        'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sscript+="parent.Comandi.location.href='" & ConstSession.PathApplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sscript+="parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sscript+="iframetabella.location.href='Anomalie.aspx?CODANOMALIA=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
        

        RegisterScript(sScript , Me.GetType())

    End Sub
End Class
