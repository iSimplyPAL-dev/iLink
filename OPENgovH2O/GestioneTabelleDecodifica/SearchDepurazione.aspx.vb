Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class SearchDepurazione
    Inherits BasePage
    ''''Accesso alla Classe DBAccess
    Private clsDBAccess As New DBAccess.getDBobject()
    Private ModDate As New ClsGenerale.Generale
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchDepurazione))
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

    '          Dim DBDepurazione As TabelleDiDecodifica.DBDepurazione = New TabelleDiDecodifica.DBDepurazione()
    '          Dim GetListaDepurazione As TabelleDiDecodifica.GetLista = DBDepurazione.GetListaDepurazione


    '          lngRecordCount = GetListaDepurazione.lngRecordCount

    '          Select Case lngRecordCount
    '              Case 0
    '                  lblMessage.Text = "La ricerca non ha prodotto risultati"
    '              Case Is > 0

    '                  GrdDepurazione.sqlComm = GetListaDepurazione.oComm
    '                  GrdDepurazione.cnnConn = GetListaDepurazione.oConn
    '                  GrdDepurazione._NumberRecord = lngRecordCount
    '                  GrdDepurazione.DataKeyField = "CODDEPURAZIONE"
    '                  'Carico il Controllo Nella Tabella

    '                  If Not Page.IsPostBack Then
    '                      GrdDepurazione.Rows.Count = 0
    '                      GrdDepurazione.BindData()
    '                  End If

    '                  GrdDepurazione.MouseSelectableDataGrid()

    '          End Select
    '      Catch ex As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDepurazione.Page_Load.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())

            Dim paginacomandi As String = Request("paginacomandi")
            paginacomandi = paginacomandi.Replace("ComandiGestioneTabelle.aspx", "ComandiGestioneTabelleSistema.aspx")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript = "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            RegisterScript(sScript, Me.GetType())

            Dim DBDepurazione As New TabelleDiDecodifica.DBDepurazione()
            Dim dv As DataView = DBDepurazione.GetListaDepurazione
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    GrdDepurazione.Visible = False
                Else
                    GrdDepurazione.Visible = True
                    GrdDepurazione.DataSource = dv
                    GrdDepurazione.DataBind()
                End If
                Session("vistaDepurazione") = dv
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDepurazione.Page_Load.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDepurazione.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdDepurazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdDepurazione.SelectedIndexChanged
    '    CalPageaspx(GrdDepurazione.DataKeys.Item(GrdDepurazione.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdDepurazione.DataSource = CType(Session("vistaDepurazione"), DataView)
            If page.HasValue Then
                GrdDepurazione.PageIndex = page.Value
            End If
            GrdDepurazione.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchDepurazione.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    '
    ' CalPageaspx
    '
    ' Richiama la Pagina 
    '
    ' 
    '
    '*******************************************************
    Protected Sub CalPageaspx(ByVal CODDEPURAZIONE As Integer)

        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Codici Depurazione" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""

        Dim paginaComandi As String
        paginaComandi = CType(Request("paginacomandi"), String).Replace("ComandiOperazioniSuTabelle.aspx", "ComandiOperazioniSuTabelleSistema.aspx")

        'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        sScript += "parent.parent.Comandi.location.href='" & ConstSession.PathApplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelleSistema.aspx" & parametri & "';"
        sScript += "iframetabella.location.href='Depurazione.aspx?CODDEPURAZIONE=" & CODDEPURAZIONE & "&paginacomandi=" & paginaComandi & "';"
        RegisterScript(sScript, Me.GetType())

    End Sub



    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click

        Dim _const As New Costanti
        Dim parametri As String = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Codici Depurazione" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim sScript As String = ""
        'sscript+="parent.Comandi.location.href='" & "/OpenUtenze/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        'sscript+="parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/GestioneTabelleDecodifica/ComandiOperazioniSuTabelle.aspx" & parametri & "';")
        sScript += "parent.parent.Comandi.location.href='ComandiOperazioniSuTabelle.aspx" & parametri & "';"
        sScript += "iframetabella.location.href='Depurazione.aspx?CODDEPURAZIONE=" & -1 & "&paginacomandi=" & Request("paginacomandi") & "';"
        RegisterScript(sScript, Me.GetType())

    End Sub

End Class
