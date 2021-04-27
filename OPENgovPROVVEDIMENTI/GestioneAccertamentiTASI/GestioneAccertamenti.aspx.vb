Imports AnagInterface
Imports Anagrafica.DLL
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la generazione dei provvedimenti TASI.
''' Contiene i parametri di gestione, le funzioni della comandiera e la griglia per la visualizzazione del dichiarato.  
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestioneAccertamenti1
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestioneAccertamenti))
   
    Protected COD_CONTRIB_RETTIFICA, ID_PROVVEDIMENTO_RETTIFICA As Long
    Protected TIPO_OPERAZIONE_RETTIFICA As Boolean
    Protected NOMINATIVO_RETTIFICA, COD_TRIBUTO_RETTIFICA, ANNO_RETTIFICA, DATA_ELABORAZIONE_RETTIFICA, DATA_RETTIFICA As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TASI
        Session("oAnagrafe") = Nothing
        Log.Debug("ConstSession.CodTributo=" & ConstSession.CodTributo)
        Dim sScript As String
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            btnFocus.Attributes.Add("onclick", "return false;")
            Dim MyUtility As New MyUtility

            '---------------------------GESTIONE ACCERTAMENTI DA RETTIFICA-------------------------------------------
            Dim myHasTbl As New Hashtable
            Try
                If Not Session("HashTableRettificaAccertamenti") Is Nothing Then
                    myHasTbl = Session("HashTableRettificaAccertamenti")
                End If

                COD_CONTRIB_RETTIFICA = myHasTbl("CODCONTRIBUENTE")
                ID_PROVVEDIMENTO_RETTIFICA = myHasTbl("ID_PROVVEDIMENTO_RETTIFICA")
                COD_TRIBUTO_RETTIFICA = myHasTbl("COD_TRIBUTO")

                If myHasTbl("ID_PROVVEDIMENTO_RETTIFICA") = 0 Then
                    TIPO_OPERAZIONE_RETTIFICA = False
                Else
                    TIPO_OPERAZIONE_RETTIFICA = True
                End If
                Session("ID_PROVVEDIMENTO_RETTIFICA") = ID_PROVVEDIMENTO_RETTIFICA

                ANNO_RETTIFICA = myHasTbl("ANNOACCERTAMENTO")
                DATA_ELABORAZIONE_RETTIFICA = myHasTbl("DATA_ELABORAZIONE_PER_RETTIFICA")
            Catch err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti.Page_Load.errore: ", err)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
            '--------------------------------------------------------------------------------------------------------
            Session("modificaDiretta") = "False"
            Session("HashTableData") = ""

            If Page.IsPostBack = False Then
                'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                'Log.Debug("GestioneAccertamenti::Page_Load::devo caricare anni")
                'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
                ' carico la combo con gli anni 
                MyUtility.FillDropDownSQLString(ddlAnno, objGestOPENgovProvvedimenti.GetAnniProvvedimentiICI(ConstSession.IdEnte, Utility.Costanti.TRIBUTO_ICI, cmdMyCommand), -1, "...")
                'Log.Debug("GestioneAccertamenti::Page_Load::caricati")
                If Request.Item("DaVersamenti") <> Nothing Then
                    'Log.Debug("GestioneAccertamenti::Page_Load::daversamenti")
                    If Request.Item("DaVersamenti").ToString().ToLower() = "true" Then
                        ' verifico se è stata già selezionata un'anagrafica
                        If Not Session("codContribuente") Is Nothing Then
                            Dim oDettAnag As New DettaglioAnagrafica
                            oDettAnag = CType(Session("codContribuente"), DettaglioAnagrafica)
                            '*** 201504 - Nuova Gestione anagrafica con form unico ***
                            If ConstSession.HasPlainAnag Then
                                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                            Else
                                txtNominativo.Text = oDettAnag.Cognome & " " & oDettAnag.Nome
                            End If
                            ViewState.Add("COGNOME", oDettAnag.Cognome)
                            ViewState.Add("NOME", oDettAnag.Nome)

                            hdIdContribuente.Value = oDettAnag.COD_CONTRIBUENTE
                            txtHiddenIdDataAnagrafica.Text = oDettAnag.ID_DATA_ANAGRAFICA
                        End If

                        If Not Session("AnnoSelezionato") Is Nothing Then
                            ddlAnno.SelectedValue = Session("AnnoSelezionato")
                        End If

                        If Not Session("AnnoSelezionato") Is Nothing And Not Session("CodContribuente") Is Nothing Then
                            btnSearchDichiarazioniEffettivo_Click(sender, e)
                        End If

                        If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                            sScript = "document.getElementById('loadGridAccertato').src='grdAccertato.aspx';"
                            RegisterScript(sScript, Me.GetType)
                        End If
                    End If
                Else
                    ViewState("sessionName") = ""
                    Session("DataSetSanzioni") = Nothing

                    hdIdContribuente.Value = "-1"
                    txtHiddenIdDataAnagrafica.Text = "-1"

                    sScript = "parent.Comandi.location.href='ComandiGestioneAccertamenti.aspx'" & vbCrLf
                    RegisterScript(sScript, Me.GetType)
                    If Not Request.Item("codcontribuente") Is Nothing Then
                        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
                        Dim oAnagrafica As New GestioneAnagrafica
                        oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(Request.Item("codcontribuente"), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'oAnagrafica.GetAnagrafica(Request.Item("codcontribuente"), ConstSession.CodTributo, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)
                        hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                        '*** 201504 - Nuova Gestione anagrafica con form unico ***
                        If ConstSession.HasPlainAnag Then
                            ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                        Else
                            txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
                        End If
                        ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
                        ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

                        Session("codContribuente") = oDettaglioAnagrafica
                        ViewState("sessionName") = ""
                    Else
                        '*** 201504 - Nuova Gestione anagrafica con form unico ***
                        If ConstSession.HasPlainAnag Then
                            ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                        End If
                    End If
                End If
            Else
                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                End If
            End If
            If TIPO_OPERAZIONE_RETTIFICA = True Then
                GestisciRettifica()
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti1.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlAnno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlAnno.SelectedIndexChanged
        Try
            If ddlAnno.SelectedIndex <> 0 And hdIdContribuente.Value <> "" Then
                If CInt(hdIdContribuente.Value) > 0 Then
                    'MySessionParam.Provv_Tributo = Utility.Costanti.TRIBUTO_TASI
                    'MySessionParam.Provv_IdContribuente = hdIdContribuente.Value
                    'MySessionParam.Provv_Anno = ddlAnno.SelectedValue
                    'MySessionParam.Provv_Nominativo = txtNominativo.Text

                    btnSearchDichiarazioni_Click(Nothing, Nothing)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti1.ddlAnno_SelctedIndexChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnSearchDichiarazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchDichiarazioni.Click
        Dim objHashTable As New Hashtable
        Try
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", Session("ID_PROVVEDIMENTO_RETTIFICA"))

            Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
            Dim iRetValControllo As Integer

            'effettuato accertamento con data di conferma presente (definitivo) RETURN 0
            'msgbox impossibile proseguire, già presente un accertamento definitivo

            'effettuato un accertamento con data di conferma non presente (atto potenziale) RETURN 4
            'msgbox Vuoi proseguire, già presente un accertamento NON definitivo

            'effettuato un pre-accertamento con data di conferma presente (definitivo) RETURN 1
            'eseguo accertamento (senza ri-eseguire la fase 2 del preaccertamento)

            'effettuato un pre-accertamento con data di conferma non presente (atto potenziale) RETURN 2
            'msgbox Vuoi proseguire, già presente un pre-accertamento NON definitivo

            'non effettuato nè ACCERTAMENTO nè PRE-ACCERTAMENTO RETURN 3

            iRetValControllo = objCOMDichiarazioniAccertamenti.GetControlliAccertamento(ConstSession.StringConnection, ddlAnno.SelectedValue, ConstSession.IdEnte, hdIdContribuente.Value, ConstSession.CodTributo, objHashTable)

            Session.Remove("VALORE_RITORNO_ACCERTAMENTO")
            Session.Add("ESCLUDI_PREACCERTAMENTO", False)

            If TIPO_OPERAZIONE_RETTIFICA = False Then
                Session.Add("VALORE_RITORNO_ACCERTAMENTO", iRetValControllo)
                If iRetValControllo = 0 Then
                    Session("ESCLUDI_PREACCERTAMENTO") = True
                    'effettuato accertamento definitivo, impossibile proseguire
                    RegisterScript("FoundAccDefinitivo();", Me.GetType)
                    Exit Sub
                End If

                If iRetValControllo = 2 Then
                    'effettuato pre-accertamento NON definitivo, Vuoi proseguire?
                    RegisterScript("FoundPreAccNONDefinitivo();", Me.GetType)
                    Exit Sub
                End If

                If iRetValControllo = 4 Then
                    'effettuato accertamento NON definitivo, Vuoi proseguire?
                    RegisterScript("FoundAccNONDefinitivo();", Me.GetType)
                    Exit Sub
                End If

                If iRetValControllo = 5 Then
                    'effettuato accertamento NON definitivo e pre accertamento non definitivo, Vuoi proseguire?
                    RegisterScript("FoundAccPreAccNONDefinitivi();", Me.GetType)
                    Exit Sub
                End If
            Else
                iRetValControllo = 6
                'Session.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
                'Session.Remove("VALORE_RITORNO_ACCERTAMENTO")
                'Session.Add("VALORE_RITORNO_ACCERTAMENTO", iRetValControllo)
                'Session.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objHashTable("DATA_ELABORAZIONE_PER_RETTIFICA"))
            End If

            btnSearchDichiarazioniEffettivo_Click(Nothing, Nothing)

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti1.btnSearchDichiarazioni_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnSearchDichiarazioniEffettivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchDichiarazioniEffettivo.Click
        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTASI.btnSearchDichiarazioniEffettivo_Click.errore.IN")
        Try
            Dim objHashTable As New Hashtable
            Dim sScript As String

            If TIPO_OPERAZIONE_RETTIFICA = True Then
                'carico nell'oggetto hashtable i dati per gestire la rettifica
                Dim objUtility As New MyUtility

                If objHashTable.ContainsKey("DATA_ELABORAZIONE_PER_RETTIFICA") Then
                    objHashTable.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
                    objHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
                Else
                    objHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
                End If
                objHashTable.Add("DATA_ELABORAZIONE", objUtility.GiraData(DATA_ELABORAZIONE_RETTIFICA))
                objHashTable.Add("COD_TRIBUTO", COD_TRIBUTO_RETTIFICA)
                objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", ID_PROVVEDIMENTO_RETTIFICA)
                DATA_RETTIFICA = DateTime.Now.ToString("yyyyMMdd")
                objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA)
                objHashTable.Add("TIPO_OPERAZIONE_RETTIFICA", TIPO_OPERAZIONE_RETTIFICA)

                objHashTable.Add("CODCONTRIBUENTE", CStr(COD_CONTRIB_RETTIFICA))
                objHashTable.Add("ANNOACCERTAMENTO", ANNO_RETTIFICA)
                Session("ID_PROVVEDIMENTO_RETTIFICA") = ID_PROVVEDIMENTO_RETTIFICA
            Else
                Session("ID_PROVVEDIMENTO_RETTIFICA") = ""
                objHashTable.Add("CODCONTRIBUENTE", hdIdContribuente.Value)
                objHashTable.Add("ANNOACCERTAMENTO", ddlAnno.SelectedValue)
            End If

            If Request.Item("DaVersamenti") = Nothing Then
                Session.Remove("DataTableImmobiliDaAccertare")
            End If
            Session("HashTableDichiarazioniAccertamenti") = objHashTable
            Session("HashTableRettificaAccertamenti") = objHashTable

            sScript = "document.getElementById('attesaCarica').style.display='';"
            sScript += "document.getElementById('loadGridDichiarato').src='SearchDatiDichiarato.aspx';"
            sScript += "document.getElementById('loadGridAccertato').src='SearchDatiAccertato.aspx';"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti1.btnSearchDichiarazioniEffettivo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTASI.btnSearchDichiarazioniEffettivo_Click.errore.OUT")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub GestisciRettifica()
        Try
            Dim oAnagrafica As New GestioneAnagrafica()
            Dim oDettaglioAnagrafica As New DettaglioAnagrafica

            'Dim WFSessione As New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            'oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
            'oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, Session("COD_TRIBUTO"))
            oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, ConstSession.CodTributo, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)

            hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
            txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString())
            Else
                txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
            End If
            ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
            ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

            Session("codContribuente") = oDettaglioAnagrafica
            ViewState("sessionName") = ""

            ddlAnno.SelectedValue = ANNO_RETTIFICA

            Imagebutton.Enabled = False
            ddlAnno.Enabled = False

            btnSearchDichiarazioni_Click(Nothing, Nothing)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti1.GestisciRettifica.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class