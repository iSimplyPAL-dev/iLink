Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports AnagInterface
Imports OggettiComuniStrade
Imports Utility
Imports log4net

Partial Class SearchResultsContatori
  Inherits BasePage
  ''''Accesso alla Classe DBAccess
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultsContatori))
        Protected FncGrd As New ClsGenerale.FunctionGrd
    Private iDB As New DBAccess.getDBobject
    Private ModDate As New ClsGenerale.Generale
    'Private oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
    Private oDettaglioAnagraficaUtente As New DettaglioAnagrafica

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
    '*******************************************************
    ' l'evento  Page_Load di questa pagina
    ' ricava secondo dei parametri di ricerca 
    ' i contatori da estrarre o da inserire 
    ' utilizzando la strored procedure sp_RicercaContatori.
    ' L'estrazione della lista avviene nella classe  DECOntatori.vb
    '*******************************************************
    Dim strMatricola, nomeIntestatario, nomeUtente, strNumeroUtente, strNominativo, Ubicazione, strIntestatario, strUtente, sFoglio, sNumero, IdEnte, sContratto, stringaUbicazione As String
    Dim lngRecordCount, intIDVia, statoCont, boolSub As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        Try
            Log.Debug("entro SearchResultsContatori")
            intIDVia = -1
            Session("datacatasto") = Nothing
            strMatricola = RTrim(LTrim(Utility.StringOperation.FormatString(Request("matricola"))))
            scontratto = Utility.StringOperation.FormatString(Request("Contratto"))
            strNumeroUtente = RTrim(LTrim(Utility.StringOperation.FormatString(Request("numeroutente"))))
            If Not Request("ubicazione") Is Nothing Then
                If IsNumeric(Request("ubicazione")) Then
                    intIDVia = StringOperation.FormatInt(Request("ubicazione"))
                End If
            End If
            strIntestatario = RTrim(LTrim(Utility.StringOperation.FormatString(Request("intestatario"))))
            nomeIntestatario = RTrim(LTrim(Utility.StringOperation.FormatString(Request("nomeIntestatario"))))
            strUtente = RTrim(LTrim(Utility.StringOperation.FormatString(Request("utente"))))
            nomeUtente = RTrim(LTrim(Utility.StringOperation.FormatString(Request("nomeUtente"))))
            statoCont = Utility.StringOperation.FormatInt(Request("statoContatore"))
            boolSub = Utility.StringOperation.FormatInt(Request("sub"))
            sFoglio = Utility.StringOperation.FormatString(Request.Item("Foglio"))
            sNumero = Utility.StringOperation.FormatString(Request.Item("Numero"))
            '*** 201511 - Funzioni Sovracomunali ***
            If Not Request("IdEnte") Is Nothing Then
                IdEnte = Utility.StringOperation.FormatString(Request("IdEnte"))
            Else
                IdEnte = ConstSession.IdEnte
            End If
            '*** ***
            Log.Debug("prelevato tutti i request")
            Dim ListParamSearch As New ArrayList
            ListParamSearch.Add(strMatricola)
            ListParamSearch.Add(strNumeroUtente)
            ListParamSearch.Add(intIDVia)
            ListParamSearch.Add(strIntestatario)
            ListParamSearch.Add(nomeIntestatario)
            ListParamSearch.Add(strUtente)
            ListParamSearch.Add(nomeUtente)
            ListParamSearch.Add(statoCont)
            ListParamSearch.Add(boolSub)
            ListParamSearch.Add(sFoglio)
            ListParamSearch.Add(sNumero)
            ListParamSearch.Add(sContratto)
            '*** 201511 - Funzioni Sovracomunali ***
            ListParamSearch.Add(IdEnte)
            '*** ***
            Session("ParamSearchContatori") = ListParamSearch
            Log.Debug("valorizzato Session(ParamSearchContatori)")
            Dim DEContatori As New GestContatori
            '*** 201511 - Funzioni Sovracomunali ***
            Dim GetList As DataSet = DEContatori.GetListaContatori(strMatricola, boolSub, strNumeroUtente, intIDVia, strIntestatario, strUtente, ConstSession.Ambiente, IdEnte, nomeUtente, nomeIntestatario, sFoglio, sNumero, statoCont, -1, sContratto)
            '*** ***
            Log.Debug("ricerca fatta")
            Session("datacatasto") = Nothing
            Session.Remove("DS_ANA")
            Session("DS_ANA") = GetList
            Dim dv As New DataView

            If Not Page.IsPostBack Then
                If Not IsNothing(GetList) Then
                    dv = GetList.Tables(0).DefaultView
                End If

                If dv.Count <= 0 Then
                    GrdContatori.Visible = False
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    lblMessage.Visible = True
                Else
                    GrdContatori.Visible = True
                    Session("vistaAna") = dv
                    GrdContatori.DataSource = dv
                    GrdContatori.DataBind()
                    Session("gridsource2") = dv
                End If
                sScript = "parent.parent.Visualizza.DivAttesa.style.display='none';"
                RegisterScript(sScript, Me.GetType())
            Else
                '*** 20140923 - GIS ***
                SetGrdCheck()
                '*** ***
            End If
            '*** 201511 - Funzioni Sovracomunali ***
            If ConstSession.IdEnte = "" Then
                GrdContatori.Columns(0).Visible = True
            Else
                GrdContatori.Columns(0).Visible = False
            End If
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatori.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub CalPageaspx(ByVal IDContatore As Integer)
        Dim sScript As String = ""
        Try
            sScript += "document.getElementById('IDContatore').value='" & IDContatore & "';"
            sScript += "document.getElementById('hdCodiceVia').value='" & intIDVia & "';"
            sScript += "document.getElementById('hdIntestatario').value='" & strIntestatario & "';"
            sScript += "document.getElementById('hdUtente').value='" & strUtente & "';"
            sScript += "document.getElementById('hdNumeroUtente').value='" & strNumeroUtente & "';"
            sScript += "document.getElementById('hdMatricola').value='" & strMatricola & "';"
            sScript += "document.getElementById('hdAvviaRicerca').value='" & 1 & "';"
            sScript += "frmHidden.submit();"

            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.SearchResultsContatori.CalPageaspx.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                '*** X UNIONE CON BANCADATI CMGC ***
                If ConstSession.VisualGIS = False Then
                    GrdContatori.Columns(10).Visible = False
                End If
                '*** ***
                If ConstSession.IdEnte = "" Then
                    e.Row.Cells(11).Enabled = False
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatori.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                '*** 201511 - Funzioni Sovracomunali ***'*** 20140923 - GIS ***
                If ConstSession.IdEnte <> "" Then
                    sScript = "ApriContatore(" + IDRow.ToString + ");"
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla funzione sovracomunale');"
                End If
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.GrdRowCommand.errore: ", ex)
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
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdContatori.DataSource = CType(Session("vistaAna"), DataView)
            If page.HasValue Then
                GrdContatori.PageIndex = page.Value
            End If
            GrdContatori.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatori.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    Public Function TrovaStradario(ByVal codiceVia As Object) As String
        Dim nomeStrada As String = ""
        Try
            If Not IsDBNull(codiceVia) Then
                Dim ArrStrade() As OggettiComuniStrade.OggettoStrada
                ArrStrade = Nothing
                If Not ArrStrade Is Nothing Then
                    If ArrStrade.Length.CompareTo(1) = 0 Then
                        nomeStrada = ArrStrade(0).TipoStrada.ToString() + " " + ArrStrade(0).DenominazioneStrada.ToString()
                    Else
                        nomeStrada = ""
                    End If
                Else
                    nomeStrada = ""
                End If
            Else
                nomeStrada = ""
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatori.TrovaStradario.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return nomeStrada
    End Function
    '*** 20140923 - GIS ***
    Private Sub SetGrdCheck()
        Try
            Dim myDvResult As DataView = CType(Session("vistaAna"), DataView)
            'nCol = 12 '11'*** 201511 - Funzioni Sovracomunali ***
            For Each itemGrid As GridViewRow In GrdContatori.Rows
                'prendo l'ID da aggiornare
                For Each myItem As DataRowView In myDvResult
                    If myItem("CODCONTATORE") = CType(CType(itemGrid.FindControl("hfCODCONTATORE"), HiddenField).Value, Long) Then
                        myItem("bSel") = CType(itemGrid.FindControl("chkSel"), CheckBox).Checked
                        Exit For
                    End If
                Next
            Next
            Session("vistaAna") = myDvResult
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatori.SetGrdCheck.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '***  ***
End Class
