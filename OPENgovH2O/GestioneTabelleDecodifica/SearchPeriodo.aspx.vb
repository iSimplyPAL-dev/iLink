Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchPeriodo
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
        Protected FncGrd As New ClsGenerale.FunctionGrd
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchPeriodo))
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

    '          Dim DBPeriodo As TabelleDiDecodifica.DBPeriodo = New TabelleDiDecodifica.DBPeriodo()
    '          Dim GetListaPeriodi As TabelleDiDecodifica.GetLista = DBPeriodo.GetListaPeriodi


    '          lngRecordCount = GetListaPeriodi.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La ricerca non ha prodotto risultati"
    '              Case Is > 0

    '                  GrdPeriodo.sqlComm = GetListaPeriodi.oComm
    '                  GrdPeriodo.cnnConn = GetListaPeriodi.oConn
    '                  GrdPeriodo._NumberRecord = lngRecordCount
    '                  GrdPeriodo.DataKeyField = "CODPERIODO"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdPeriodo.Rows.Count = 0
    '                      GrdPeriodo.BindData()
    '                  End If

    '                  GrdPeriodo.MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchPeriodo.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' Configurazione del periodo all'interno dell'anno a cui si legherà una determinata fatturazione (puo' anche coincidere con l'anno solare)	
    ''' Possibilità di configurare la tipologia di arrotondamento del consumo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                RegisterScript("document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';", Me.GetType())

                Dim paginacomandi As String = Request("paginacomandi")
                If Len(paginacomandi) = 0 Then
                    paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/GestioneTabelleDecodifica/ComandiGestioneTabelle.aspx"
                End If
                Dim parametri As String
                parametri = "?title=Periodo"

                Dim sScript As String = ""
                sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())

                Dim DBPeriodo As New TabelleDiDecodifica.DBPeriodo()
                Dim dv As DataView = DBPeriodo.GetListaPeriodi
                If Not IsNothing(dv) Then
                    If dv.Count <= 0 Then
                        lblMessage.Text = "La ricerca non ha prodotto risultati"
                        GrdPeriodo.Visible = False
                    Else
                        GrdPeriodo.Visible = True
                        GrdPeriodo.DataSource = dv
                        GrdPeriodo.DataBind()
                    End If
                End If
                Session("vistaPeriodi") = dv
                RegisterScript("$('#iframetabella').hide();", Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchPeriodo.Page_Load.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchPeriodo.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdPeriodo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdPeriodo.SelectedIndexChanged
    '    CalPageaspx(GrdPeriodo.DataKeys.Item(GrdPeriodo.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdPeriodo.DataSource = CType(Session("vistaPeriodi"), DataView)
            If page.HasValue Then
                GrdPeriodo.PageIndex = page.Value
            End If
            GrdPeriodo.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchPeriodo.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="CODPERIODO">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal CODPERIODO As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Periodo" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""
        Try
            'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            'sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sScript += "iframetabella.location.href='Periodo.aspx?CODPERIODO=" & CODPERIODO & "&paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchPeriodo.CalPageaspx.errore: ", ex)
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
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Periodo" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""
        Try
            'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
            sScript += "parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
            sScript += "iframetabella.location.href='Periodo.aspx?CODPERIODO=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchPeriodo.btnNuovo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' trasformazione in stringa del valore booleano dell'oggetto passato
    ''' </summary>
    ''' <param name="prdStatus">oggetto</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckStatus(ByVal prdStatus As Object) As String
        If utility.stringoperation.formatbool(prdStatus) = False Then
            Return "No"
        Else
            Return "Si"
        End If
    End Function
End Class
