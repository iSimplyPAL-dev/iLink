Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports OggettiComuniStrade
Imports log4net
Imports Utility

Partial Class SearchResultsContatoriLetture
    Inherits BasePage
    Protected FncGrd As New ClsGenerale.FunctionGrd
    ''''Accesso alla Classe DBAccess
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultsContatoriLetture))
    Private clsDBAccess As New DBAccess.getDBobject
    Private ModDate As New ClsGenerale.Generale
    Private _Const As New Costanti
    Private Generali As New Generali

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents grdLetture As RIDataGrid.RibesGrid

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

    End Sub

#End Region
    '*******************************************************
    '
    ' l'evento  Page_Load di questa pagina
    ' ricava secondo dei parametri di ricerca 
    ' i contatori da estrarre o da inserire 
    ' utilizzando la strored procedure sp_RicercaContatori.
    ' L'estrazione della lista avviene nella classe  DECOntatori.vb
    '
    '*******************************************************
    Dim Intestatario, Utente, NumeroUtente, Ubicazione, Matricola As String
    Dim lngRecordCount, IDVia, Giro, Cessati As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        '//Parametri di Ricerca
        Try
            Dim _sub, IsLetturaPresente, IsLetturaMancante As Boolean
            If Not Page.IsPostBack Then
                Intestatario = RTrim(LTrim(Request("intestatario")))
                Utente = RTrim(LTrim(Request("utente")))
                IDVia = StringOperation.FormatInt(Request("ubicazione"))
                Ubicazione = RTrim(LTrim(Request("ubicazionetext")))
                If Len(Intestatario) > 0 Then
                    Intestatario = Replace(Intestatario, "'", "''")
                End If
                If Len(Utente) > 0 Then
                    Utente = Replace(Utente, "'", "''")
                End If
                Giro = stringoperation.formatint(request("giro"))
                NumeroUtente = RTrim(LTrim(Request("numeroUtente")))
                Cessati = stringoperation.formatint(request("cessati"))
                Matricola = RTrim(LTrim(Request("matricola")))
                _sub = Request("sub")
                IsLetturaPresente = Request.Item("LetturaPresente")
                IsLetturaMancante = Request.Item("LetturaMancante")
                Dim ListParamSearch As New ArrayList
                ListParamSearch.Add(Intestatario)
                ListParamSearch.Add(Utente)
                ListParamSearch.Add(IDVia)
                ListParamSearch.Add(Ubicazione)
                ListParamSearch.Add(Giro)
                ListParamSearch.Add(NumeroUtente)
                ListParamSearch.Add(Cessati)
                ListParamSearch.Add(Matricola)
                ListParamSearch.Add(_sub)
                ListParamSearch.Add(IsLetturaPresente)
                ListParamSearch.Add(IsLetturaMancante)
                Session("ParamSearchLetture") = ListParamSearch
                Dim GestLetture As New GestLetture
                Dim dv As DataView = GestLetture.GetListaContatori(Intestatario, Utente, IDVia, Giro, NumeroUtente, Cessati, ConstSession.IdEnte, Matricola, _sub, IsLetturaPresente, IsLetturaMancante)
                If Not IsNothing(dv) Then
                    If dv.Count <= 0 Then
                        GrdContatoriLetture.Visible = False
                        lblMessage.Text = "La ricerca non ha prodotto risultati"
                        lblMessage.Visible = True
                    Else
                        Session("vistaLetture") = dv
                        GrdContatoriLetture.Visible = True
                        GrdContatoriLetture.DataSource = dv
                        GrdContatoriLetture.DataBind()
                    End If
                End If
                sScript = "parent.parent.Visualizza.DivAttesa.style.display='none';"
                RegisterScript(sScript, Me.GetType())
                'Dim GetListContatori As objDBListSQL = GestLetture.GetListaContatori(Intestatario, Utente, IDVia, Giro, NumeroUtente, Cessati, ConstSession.CodIstat, Matricola, _sub, IsLetturaPresente)
                'lngRecordCount = GetListContatori.RecordCount
                'Select lngRecordCount
                '        Case 0
                '            lblMessage.Text = "Non sono stati trovati contatori"
                '        Case Is > 0
                'GrdContatoriLetture.cnnConn = GetListContatori.oConn
                'GrdContatoriLetture.sSQL= GetListContatori.Query
                'GrdContatoriLetture.strSqlCountRecord = GetListContatori.QueryCount
                'GrdContatoriLetture.DataKeyField = "CODCONTATORE"
                ''Carico il Controllo Nella Tabella
                'GrdContatoriLetture.Rows.Count = 0
                'GrdContatoriLetture.BindData()
                'GrdContatoriLetture.MouseSelectableDataGrid()
                'End Select
            Else

                Dim dv As DataView = Session("vistaLetture")
                If Not IsNothing(dv) Then
                    GrdContatoriLetture.DataSource = dv
                End If
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatoriLetture.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*******************************************************
    '
    ' GrdContatoriLetture_SelectedIndexChanged
    '
    ' Richiamata dalla Griglia GrdContatoriLetture per effettuare un  redirect 
    ' su Letture/letture.aspx
    ' Parametri:GrdContatoriLetture.DataKeys.Item(GrdContatoriLetture.SelectedIndex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatoriLetture.GrdRowCommand.errore: ", ex)
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
    'Public Sub GrdContatoriLetture_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GrdContatoriLetture.SelectedIndexChanged
    '    'Ridirige la pagina nella gestione nuovo contatore passando come parametro L'id Del contatore selezionato
    '    CalPageaspx(GrdContatoriLetture.DataKeys.Item(GrdContatoriLetture.SelectedIndex))
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdContatoriLetture.DataSource = CType(Session("vistaLetture"), DataView)
            If page.HasValue Then
                GrdContatoriLetture.PageIndex = page.Value
            End If
            GrdContatoriLetture.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatoriLetture.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    '*******************************************************
    '
    ' CalPageaspx
    '
    ' Richiama la Pagina DatiGenerali.aspx (Letture/Letture.aspx)
    ' Parametri: CODCONTATORE l'id del Contatore selezionato
    ' 
    '
    '*******************************************************
    'Protected Sub CalPageaspx(ByVal IDContatore As Integer)
    '    Dim parametri As String
    '    dim sScript as string=""
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    'Try
    '    'inizializzo la connessione
    '    WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '    If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If

    '    '*** controllo se ho una variazione in corso per il contatore ***
    '    Log.Debug("controllo se ho una variazione in corso per il contatore")
    '    Dim FncVar As New ClsRibaltaVar
    '    Dim bIsInVar As Boolean
    '    If Not IsNothing(FncVar.GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, WFSessione, IDContatore)) Then
    '        bIsInVar = True
    '    Else
    '        bIsInVar = False
    '    End If
    '    '*** ***
    '    '*** se ho una variazione in corso la provenienza diventa R in questo modo la videata è bloccata alle variazioni ***
    '    parametri = "?title=Acquedotto - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Inserimento-Modifica Letture" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
    '    If bIsInVar = True Then
    '        parametri += "&sProvenienza=AE"
    '    Else
    '        parametri += "&sProvenienza=" & Request.Item("sProvenienza")
    '    End If
    '    '*** ***

    '    
    '    'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/DataEntryLetture/ComandiDataEntry.aspx" & parametri & "';")
    '    sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/DataEntryLetture/ComandiDataEntry.aspx" & parametri & "';")

    '    sscript+="frmHidden.bIsInVar.value='" & bIsInVar & "';")
    '    sscript+="frmHidden.hdIDContatore.value='" & IDContatore & "';")
    '    sscript+="frmHidden.hdCodiceVia.value='" & IDVia & "';")
    '    sscript+="frmHidden.hdIntestatario.value='" & Intestatario & "';")
    '    sscript+="frmHidden.hdUtente.value='" & Utente & "';")
    '    sscript+="frmHidden.hdUbicazioneText.value='" & Ubicazione & "';")
    '    sscript+="frmHidden.hdGiro.value='" & Giro & "';")
    '    sscript+="frmHidden.hdNumeroUtente.value='" & NumeroUtente & "';")
    '    sscript+="frmHidden.hdCessati.value='" & Cessati & "';")
    '    sscript+="frmHidden.hdMatricola.value='" & Matricola & "';")
    '    sscript+="frmHidden.paginacomandi.value='" & Request("paginacomandi") & "';")
    '    sscript+="frmHidden.PAG_PREC.value='" & costanti.enmcontesto.DELETTURE & "';")
    '    sscript+="frmHidden.submit();")

    '    

    '    RegisterScript(sScript , Me.GetType())
    ' Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatoriLetture.CalPageaspx.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    Protected Sub CalPageaspx(ByVal IDContatore As Integer)
        Dim parametri As String
        Dim sScript As String = ""

        '*** controllo se ho una variazione in corso per il contatore ***

        Log.Debug("controllo se ho una variazione in corso per il contatore")
        Dim FncVar As New ClsRibaltaVar
        Dim bIsInVar As Boolean
        Try
            If Not IsNothing(FncVar.GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, IDContatore, -1)) Then
                bIsInVar = True
            Else
                bIsInVar = False
            End If
            '*** ***
            '*** se ho una variazione in corso la provenienza diventa R in questo modo la videata è bloccata alle variazioni ***
            parametri = "?title=Acquedotto - " & ConstSession.DescrTipoProcServ & " - " & "Inserimento-Modifica Letture" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
            If bIsInVar = True Then
                parametri += "&sProvenienza=AE"
            Else
                parametri += "&sProvenienza=" & Request.Item("sProvenienza")
            End If
            '*** ***


            'sscript+="parent.parent.Comandi.location.href='" & "/OpenUtenze/DataEntryLetture/ComandiDataEntry.aspx" & parametri & "';")
            'sscript+="parent.parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/DataEntryLetture/ComandiDataEntry.aspx" & parametri & "';")
            sScript += "parent.parent.Comandi.location.href='ComandiDataEntry.aspx" & parametri & "';"

            parametri = "?hdIDContatore=" & IDContatore
            parametri += "&PAG_PREC=" & Costanti.enmContesto.DELETTURE
            parametri += "&hdCodiceVia=" & IDVia
            parametri += "&hdIntestatario=" & Intestatario
            parametri += "&hdUtente=" & Utente
            parametri += "&hdGiro=" & Giro
            parametri += "&hdNumeroUtente=" & NumeroUtente
            parametri += "&hdUbicazioneText=" & Ubicazione
            parametri += "&hdCessati=" & Cessati
            parametri += "&hdMatricola=" & Matricola
            parametri += "&paginacomandi=" & Request("paginacomandi")
            sScript += "parent.parent.Visualizza.location.href='../Letture/Letture.aspx" & parametri & "';"

            'sscript+="frmHidden.bIsInVar.value='" & bIsInVar & "';")
            'sscript+="frmHidden.hdIDContatore.value='" & IDContatore & "';")
            'sscript+="frmHidden.hdCodiceVia.value='" & IDVia & "';")
            'sscript+="frmHidden.hdIntestatario.value='" & Intestatario & "';")
            'sscript+="frmHidden.hdUtente.value='" & Utente & "';")
            'sscript+="frmHidden.hdUbicazioneText.value='" & Ubicazione & "';")
            'sscript+="frmHidden.hdGiro.value='" & Giro & "';")
            'sscript+="frmHidden.hdNumeroUtente.value='" & NumeroUtente & "';")
            'sscript+="frmHidden.hdCessati.value='" & Cessati & "';")
            'sscript+="frmHidden.hdMatricola.value='" & Matricola & "';")
            'sscript+="frmHidden.paginacomandi.value='" & Request("paginacomandi") & "';")
            'sscript+="frmHidden.PAG_PREC.value='" & costanti.enmcontesto.DELETTURE & "';")
            'sscript+="frmHidden.submit();")
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsContatoriLetture.CalPageaspx.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
