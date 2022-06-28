Imports ComPlusInterface
Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina dei comandi per la generazione del ruolo coattivo.
''' Le possibili opzioni sono:
''' - Ruoli Precedenti
''' - Crea 290
''' - Approva
''' - Stampa
''' - Calcola
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' <list type="bullet">
''' <item>
''' <em>Generazione Ruolo</em>
''' La nuova videata permetterà la selezione di:
''' - anno (possibilità di selezionare un anno o tutti)
''' - tributo (possibilità di selezionare più di un tributo)
''' - periodo di notifica (obbligatorio)
''' - provenienza
''' - applica interessi
''' - applica spese di notifica
''' cliccando il pulsante di CALCOLO il sistema calcolerà un RUOLO. I nuovi atti così ricalcolati saranno memorizzati nella nuova tabella TBLCOATTIVO che conterrà solo il riferimento al ruolo, i nuovi importi e il riferimento all'atto da cui deriva; il dettaglio degli interessi per ogni atto sarà memorizzato nella tabella TBLCOATTIVOINTERESSI.
''' Dalla griglia sarà possibile eliminare le posizioni per le quali non si vuole procedere, ossia sarà valorizzata la DATA_VARIAZIONE; una volta eliminate tutte le posizioni desiderate bisognerà cliccare sul pulsante di APPROVAZIONE per consolidarne l'elaborazione. 
''' Cliccando sul pulsante di CREAZIONE 290 il sistema genererà il/i flusso/i 290 In base alla suddivisione per tributi di Agenzia delle Entrate. La creazione del flusso 290 setterà sugli avvisi la data coattivo.
''' </item>
''' <item>
''' <em>Consultazione Ruolo</em>
''' All 'ingresso della funzione il sistema controllerà che non ci siano ruoli in sospeso, ossia che non ci siano ruoli per i quali non è stato prodotto il flusso 290; in caso contrario il sistema visualizzerà una griglia riepilogativa del ruolo e da qui si potrà eliminarlo o procedere al suo completamento; sulla griglia saranno quindi presenti 3 pulsanti:
''' - approvazione posizioni
''' - creazione flusso 290
''' - eliminazione
''' Una volta generato il flusso 290 non sarà più possibile agire sul ruolo ma sarà solo possibile agire puntualmente sui singoli avvisi dalle normali funzioni di gestione avvisi.
''' </item>
''' </list>
''' </revision>
''' </revisionHistory>
Public Class GestRuolo
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestRuolo))
    Protected FncGrd As New Formatta.FunctionGrd
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        lblTitolo.Text = ConstSession.DescrizioneEnte
        info.InnerText = "Gestione Atti - Coattivo"
        Dim sScript As String = ""
        sScript += "$('#GrdRuoliPrec').hide();$('#Precedenti').hide();$('#FileEstratto').hide();"
        RegisterScript(sScript, Me.GetType())
        'FileEstratto.Visible = False
        DivRiepilogoDaElab.Visible = False
        visualizzadatiDelibera()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myUtil As New MyUtility
        Dim ListRuoli() As ObjRuolo
        Try
            If Page.IsPostBack = False Then
                LblFile290.Attributes.Add("onclick", "CmdScarica.click()")
                myUtil.FillDropDownSQLString(ddlAnno, New DBPROVVEDIMENTI.ProvvedimentiDB().GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, ""), -1, "TUTTI")
                myUtil.FillDropDownSQLString(ddlProvenienza, New DBPROVVEDIMENTI.ProvvedimentiDB().GetProvenienzaProvvedimenti(), -1, "TUTTI")
                Dim CalcoloInCorso As String = CacheManager.GetCalcoloCoattivoInCorso
                If (CalcoloInCorso <> "") Then
                    ShowCalcoloInCorso()
                Else
                    If Not IsNothing(CacheManager.GetRiepilogoCalcoloCoattivo) Then
                        VisualizzaRiepilogo(CacheManager.GetRiepilogoCalcoloCoattivo)
                        CacheManager.RemoveRiepilogoCalcoloCoattivo()
                    Else
                        'prelevo il ruolo in elaborazione
                        ListRuoli = New clsCoattivo().GetRuolo(ConstSession.IdEnte, 0, ConstSession.StringConnection)
                        '*** ***
                        VisualizzaRiepilogo(ListRuoli)
                    End If
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "Coattivo", Utility.Costanti.AZIONE_LETTURA.ToString, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdCalcola_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCalcola.Click
        Dim ListTributi As String = ""
        Dim fncElab As New clsCoattivo

        Try
            CacheManager.RemoveRiepilogoCalcoloCoattivo()
            CacheManager.GetAvanzamentoCalcoloCoattivo()
            If chk8852.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_ICI
            End If
            If chkTASI.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_TASI
            End If
            If chk0434.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_TARSU
            End If
            If chk0453.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_OSAP
            End If
            ListTributi = ListTributi.Substring(1, ListTributi.Length - 1)
            ShowCalcoloInCorso()
            fncElab.StartCoattivo(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, ConstSession.UserName, ddlAnno.SelectedValue, ListTributi, CDate(txtDalNotifica.Text), CDate(txtAlNotifica.Text), ddlProvenienza.SelectedValue, chkInteressi.Checked, chkSpese.Checked)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.CmdCalcola_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdApprova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdApprova.Click
        Dim sScript As String = ""
        Dim myRuolo As New ObjRuolo
        Try
            If CInt(hfIdRuolo.Value) > 0 Then
                myRuolo = CType(Session("RuoloCoattivo"), ObjRuolo)
                myRuolo.tDataOKMinuta = DateTime.Now
                myRuolo.tDataElabDoc = DateTime.MaxValue
                If New clsCoattivo().SetRuolo(Utility.Costanti.AZIONE_UPDATE, myRuolo, ConstSession.StringConnection, Nothing) = False Then
                    sScript += "GestAlert('a', 'danger', '', '', 'Errore in Approvazione ruolo!');"
                Else
                    sScript += "GestAlert('a', 'success', '', '', 'Approvazione effettuata con successo!');"
                    DivRiepilogoDaElab.Visible = False
                    GrdDateElaborazione.Visible = False
                End If
                Session("RuoloCoattivo") = myRuolo
                Dim myList As New ArrayList
                myList.Add(myRuolo)
                VisualizzaRiepilogo(CType(myList.ToArray(GetType(ObjRuolo)), ObjRuolo()))
            Else
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare un ruolo!');"
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.CmdApprova_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sNameXLS As String
        Dim sScript As String = ""
        Dim x, nCol As Integer
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList

        sNameXLS = ConstSession.IdEnte + "_COATTIVI_" + Format(CType(Session("RuoloCoattivo"), ObjRuolo).tDataInizioConf, "yyyyMMdd") + "-" + Format(CType(Session("RuoloCoattivo"), ObjRuolo).tDataFineConf, "yyyyMMdd") + "_" + Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        If CInt(hfIdRuolo.Value) <= 0 Then
            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
            RegisterScript(sScript, Me.GetType)
        Else
            Try
                nCol = 14
                DtDatiStampa = New clsCoattivo.clsStampa().PrintMinuta(Session("ListCoattivi"), ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, nCol, CType(Session("RuoloCoattivo"), ObjRuolo).tDataInizioConf, CType(Session("RuoloCoattivo"), ObjRuolo).tDataFineConf)
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.StampaMinuta_Click.errore: ", Err)
                Response.Redirect("../../../PaginaErrore.aspx")
            End Try
            If Not DtDatiStampa Is Nothing Then
                Try
                    Dim myList As New ArrayList
                    myList.Add(Session("RuoloCoattivo"))
                    VisualizzaRiepilogo(CType(myList.ToArray(GetType(ObjRuolo)), ObjRuolo()))
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.StampaMinuta_Click.errore: ", ex)
                    Response.Redirect("../../../PaginaErrore.aspx")
                End Try
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdEstrai_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEstrai.Click
        Dim sScript As String = ""
        Dim nReturnEstraz As Integer
        Dim sPathFile290, sNameFile290, myErr As String
        Dim ListFile As New ArrayList
        Try
            If CInt(hfIdRuolo.Value) > 0 Then
                'controllo che siano stati configurati i codici enti
                If ConstSession.IdEnteCredBen <> "" And ConstSession.Belfiore <> "" Then
                    If New clsCoattivo().SetDelibera(ConstSession.IdEnte, ConstSession.StringConnection, TxtNDelibera.Text, txtInizioDelibera.Text, txtFineDelibera.Text) = False Then
                    End If
                    'valorizzo il perco rso e il nome del file
                    sPathFile290 = ConstSession.PathEstrazione290
                    sNameFile290 = ConstSession.IdEnteCredBen & Format(Now, "yy")
                    'richiamo la funzione di estrazione
                    nReturnEstraz = New clsCoattivo.cls290().Crea290(ConstSession.StringConnection, ConstSession.IdEnteCNC, "", "", hfIdRuolo.Value, ConstSession.IdEnteCredBen, 1, sPathFile290, sNameFile290, myErr, ListFile)
                    If nReturnEstraz = -2 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Sono presenti delle anagrafiche con data nascita errata e/o senza l\'indicazione del sesso!\n" + myErr + "\nEstrazione non effettuata!');"
                    ElseIf nReturnEstraz < 0 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Estrazione non effettuata!');"
                    ElseIf nReturnEstraz = 0 Then
                        sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un errore!');"
                    Else
                        If New clsCoattivo().SetDataCoattivo(hfIdRuolo.Value, ConstSession.StringConnection) <= 0 Then
                            sScript += "GestAlert('a', 'danger', '', '', 'Errore in Estrazione ruolo!');"
                            RegisterScript(sScript, Me.GetType)
                        Else
                            Dim myRuolo As ObjRuolo = CType(Session("RuoloCoattivo"), ObjRuolo)
                            myRuolo.tDataElabDoc = DateTime.Now
                            If New clsCoattivo().SetRuolo(Utility.Costanti.AZIONE_UPDATE, myRuolo, ConstSession.StringConnection, Nothing) = False Then
                                sScript += "GestAlert('a', 'danger', '', '', 'Errore in Estrazione ruolo!');"
                                RegisterScript(sScript, Me.GetType)
                            Else
                                Session("RuoloCoattivo") = myRuolo
                                Dim myList As New ArrayList
                                myList.Add(myRuolo)
                                VisualizzaRiepilogo(CType(myList.ToArray(GetType(ObjRuolo)), ObjRuolo()))
                                sScript = "GestAlert('a', 'success', '', '', 'Estrazione 290 terminata con successo!');"
                                sScript += "$('LblFile290').show();$('#FileEstratto').show();" 'sScript += "document.getElementById('LblFile290').style.display = '';$('#FileEstratto').show();"
                                LblFile290.Text = sNameFile290 & ".zip"
                            End If
                        End If
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Manca la configurazione del codice ente impositore.\nImpossibile proseguire!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                RegisterScript("GestAlert('a', 'warning', '', '', 'Selezionare un ruolo!');", Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.CmdEstrai_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdOld.Click
        Dim sScript As String = ""
        Try
            GrdRuoliPrec.Visible = True
            GrdRuoliPrec.DataSource = New clsCoattivo().GetRuolo(ConstSession.IdEnte, 1, ConstSession.StringConnection)
            GrdRuoliPrec.DataBind()
            GrdRuoliPrec.SelectedIndex = -1
            DivRiepilogoDaElab.Visible = False
            sScript += ";$('#Precedenti').hide();$('#GrdRuoliPrec').show();"
            sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.CmdOld_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", "attachment; filename=" + LblFile290.Text)
        Response.WriteFile(ConstSession.PathEstrazione290 + LblFile290.Text)
        Response.End()
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' Metodo per la gestione degli eventi sulla griglia.
    ''' Dalla griglia della lavorazione in corso sono stati tolti tutti i pulsanti tranne la visualizzazione perché fuorvianti nei passaggi da fare.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim sScript As String = ""
        Dim IDRow As Integer = CInt(e.CommandArgument.ToString())

        DivRiepilogoDaElab.Visible = False
        Select Case e.CommandName
            Case "RowOK"
                Try
                    If Not IsNothing(Session("RuoloCoattivo")) Then
                        If CType(Session("RuoloCoattivo"), ObjRuolo).tDataElabDoc.Date <> DateTime.MaxValue.Date And CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID <> "GrdRuoliPrec" Then
                            sScript += "GestAlert('a', 'warning', '', '', 'Ruolo gia\' estratto impossibile modificarlo!');"
                            sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
                            Exit Try
                        End If
                    End If
                    Dim ListCoattivi() As ObjCoattivo
                    ListCoattivi = New clsCoattivo().GetListCoattivi(ConstSession.StringConnection, IDRow)
                    Session("ListCoattivi") = ListCoattivi
                    LoadSearch(0)
                    hfIdRuolo.Value = IDRow
                    GrdDateElaborazione.Visible = False
                    GrdPosizioni.Visible = True
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
                    Response.Redirect("../../../PaginaErrore.aspx")
                End Try
            Case "RowPrint"
                Try
                    Dim ListCoattivi() As ObjCoattivo
                    ListCoattivi = New clsCoattivo().GetListCoattivi(ConstSession.StringConnection, IDRow)
                    Session("ListCoattivi") = ListCoattivi
                    LoadSearch(0)
                    hfIdRuolo.Value = IDRow
                    DivRiepilogoDaElab.Visible = False
                    sScript += "$('#ElaboraRuolo').hide();"
                    GrdDateElaborazione.Visible = False
                    GrdPosizioni.Visible = True
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
                    Response.Redirect("../../../PaginaErrore.aspx")
                End Try
                CmdStampa_Click(Nothing, Nothing)
            Case "RowDel"
                Try
                    If IsNothing(Session("RuoloCoattivo")) Then
                        sScript += "GestAlert('a', 'warning', '', '', 'Ruolo gia\' estratto impossibile proseguire!');"
                        sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').show();$('#CreaFile').hide();"
                    Else
                        If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdPosizioni" Then
                            If CType(Session("RuoloCoattivo"), ObjRuolo).tDataOKMinuta.Date <> DateTime.MaxValue.Date Then
                                sScript += "GestAlert('a', 'warning', '', '', 'Ruolo gia\' approvato impossibile proseguire!');"
                                sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').show();$('#CreaFile').show();"
                            Else
                                If New clsCoattivo().DeleteCoattivo(ConstSession.StringConnection, IDRow) = False Then
                                    sScript += "GestAlert('a', 'danger', '', '', 'Errore in cancellazione!');$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
                                Else
                                    Dim ListCoattivi As New ArrayList
                                    For Each myItem As ObjCoattivo In CType(Session("ListCoattivi"), ObjCoattivo())
                                        If myItem.Id <> IDRow Then
                                            ListCoattivi.Add(myItem)
                                        End If
                                    Next
                                    Session("ListCoattivi") = CType(ListCoattivi.ToArray(GetType(ObjCoattivo)), ObjCoattivo())
                                    LoadSearch(0)
                                    sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata con successo!');"
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
                    Response.Redirect("../../../PaginaErrore.aspx")
                End Try
        End Select
        RegisterScript(sScript, Me.GetType)
    End Sub
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Dim sScript As String = ""
    '    Dim IDRow As Integer = CInt(e.CommandArgument.ToString())

    '    DivRiepilogoDaElab.Visible = False
    '    Select Case e.CommandName
    '        Case "RowOK"
    '            Try
    '                If Not IsNothing(Session("RuoloCoattivo")) Then
    '                    If CType(Session("RuoloCoattivo"), ObjRuolo).tDataElabDoc.Date <> DateTime.MaxValue.Date And CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID <> "GrdRuoliPrec" Then
    '                        sScript += "GestAlert('a', 'warning', '', '', 'Ruolo gia\' estratto impossibile modificarlo!');"
    '                        sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
    '                        Exit Try
    '                    End If
    '                End If
    '                Dim ListCoattivi() As ObjCoattivo
    '                ListCoattivi = New clsCoattivo().GetListCoattivi(ConstSession.StringConnection, IDRow)
    '                Session("ListCoattivi") = ListCoattivi
    '                LoadSearch(0)
    '                hfIdRuolo.Value = IDRow
    '                GrdDateElaborazione.Visible = False
    '                GrdPosizioni.Visible = True
    '            Catch ex As Exception
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
    '                Response.Redirect("../../../PaginaErrore.aspx")
    '            End Try
    '        Case "RowPrint"
    '            Try
    '                Dim ListCoattivi() As ObjCoattivo
    '                ListCoattivi = New clsCoattivo().GetListCoattivi(ConstSession.StringConnection, IDRow)
    '                Session("ListCoattivi") = ListCoattivi
    '                LoadSearch(0)
    '                hfIdRuolo.Value = IDRow
    '                DivRiepilogoDaElab.Visible = False
    '                sScript += "$('#ElaboraRuolo').hide();"
    '                GrdDateElaborazione.Visible = False
    '                GrdPosizioni.Visible = True
    '            Catch ex As Exception
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
    '                Response.Redirect("../../../PaginaErrore.aspx")
    '            End Try
    '            CmdStampa_Click(Nothing, Nothing)
    '        Case "RowExtract"
    '            Try
    '                If CType(Session("RuoloCoattivo"), ObjRuolo).tDataOKMinuta.Date = DateTime.MaxValue.Date Then
    '                    sScript += "GestAlert('a', 'warning', '', '', 'Ruolo non ancora approvato impossibile proseguire!');"
    '                    sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
    '                Else
    '                    Dim ListCoattivi() As ObjCoattivo
    '                    ListCoattivi = New clsCoattivo().GetListCoattivi(ConstSession.StringConnection, IDRow)
    '                    Session("ListCoattivi") = ListCoattivi
    '                    LoadSearch(0)
    '                    hfIdRuolo.Value = IDRow
    '                    DivRiepilogoDaElab.Visible = False
    '                    sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();"
    '                    GrdDateElaborazione.Visible = False
    '                    GrdPosizioni.Visible = True
    '                    CmdEstrai_Click(Nothing, Nothing)
    '                End If
    '            Catch ex As Exception
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
    '                Response.Redirect("../../../PaginaErrore.aspx")
    '            End Try
    '        Case "RowDel"
    '            Try
    '                If IsNothing(Session("RuoloCoattivo")) Then
    '                    sScript += "GestAlert('a', 'warning', '', '', 'Ruolo gia\' estratto impossibile proseguire!');"
    '                    sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').show();$('#CreaFile').hide();"
    '                Else
    '                    If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdDateElaborazione" Then
    '                        If New clsCoattivo().DeleteRuolo(ConstSession.StringConnection, IDRow) = False Then
    '                            sScript += "GestAlert('a', 'danger', '', '', 'Errore in cancellazione ruolo!');$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
    '                        Else
    '                            DivRiepilogoDaElab.Visible = True
    '                            sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata con successo!');$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
    '                            GrdDateElaborazione.Visible = False
    '                        End If
    '                    Else
    '                        If CType(Session("RuoloCoattivo"), ObjRuolo).tDataOKMinuta.Date <> DateTime.MaxValue.Date Then
    '                            sScript += "GestAlert('a', 'warning', '', '', 'Ruolo gia\' approvato impossibile proseguire!');"
    '                            sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').show();$('#CreaFile').show();"
    '                        Else
    '                            If New clsCoattivo().DeleteCoattivo(ConstSession.StringConnection, IDRow) = False Then
    '                                sScript += "GestAlert('a', 'danger', '', '', 'Errore in cancellazione!');$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
    '                            Else
    '                                Dim ListCoattivi As New ArrayList
    '                                For Each myItem As ObjCoattivo In CType(Session("ListCoattivi"), ObjCoattivo())
    '                                    If myItem.Id <> IDRow Then
    '                                        ListCoattivi.Add(myItem)
    '                                    End If
    '                                Next
    '                                Session("ListCoattivi") = CType(ListCoattivi.ToArray(GetType(ObjCoattivo)), ObjCoattivo())
    '                                LoadSearch(0)
    '                                sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata con successo!');"
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Catch ex As Exception
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GrdRowCommand.errore: ", ex)
    '                Response.Redirect("../../../PaginaErrore.aspx")
    '            End Try
    '    End Select
    '    RegisterScript(sScript, Me.GetType)
    'End Sub
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
        Dim sScript As String = ""
        Try
            DivRiepilogoDaElab.Visible = False
            sScript += "$('#Precedenti').hide();"
            If Not IsNothing(Session("RuoloCoattivo")) Then
                If CType(Session("RuoloCoattivo"), ObjRuolo).tDataOKMinuta.Date <> DateTime.MaxValue.Date Then
                    sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();"
                Else
                    sScript += "$('#ElaboraRuolo').hide();$('#CreaFile').hide();"
                End If
            Else
                sScript += "$('#ElaboraRuolo').hide();$('#Stampa').show();$('#Approva').hide();$('#CreaFile').hide();"
            End If
            RegisterScript(sScript, Me.GetType)
            GrdPosizioni.DataSource = CType(Session("ListCoattivi"), ObjCoattivo())
            If page.HasValue Then
                GrdPosizioni.PageIndex = page.Value
            End If
            GrdPosizioni.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ShowCalcoloInCorso()
        DivAttesa.Style.Add("display", "")
        Response.AppendHeader("refresh", "5")
        Session("CoattivoAnnoCalcoloInCorso") = ddlAnno.SelectedValue
        LblAvanzamento.Text = CacheManager.GetAvanzamentoCalcoloCoattivo
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ListRuoli"></param>
    Private Sub VisualizzaRiepilogo(ByVal ListRuoli() As ObjRuolo)
        Dim sScript As String = ""
        Try
            hfIdRuolo.Value = -1
            sScript += "$('#GrdRuoliPrec').hide();$('#Precedenti').hide();"
            DivRiepilogoDaElab.Visible = True
            GrdDateElaborazione.Visible = False
            GrdPosizioni.Visible = False
            If Not IsNothing(ListRuoli) Then
                If ListRuoli.GetUpperBound(0) >= 0 Then
                    hfIdRuolo.Value = ListRuoli(0).IdFlusso
                    Session("RuoloCoattivo") = ListRuoli(0)
                    DivRiepilogoDaElab.Visible = False
                    sScript += "$('#ElaboraRuolo').hide();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
                    GrdDateElaborazione.Visible = True
                    GrdDateElaborazione.DataSource = ListRuoli
                    GrdDateElaborazione.DataBind()
                    GrdDateElaborazione.SelectedIndex = -1
                Else
                    sScript += "$('#Precedenti').show();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
                End If
            Else
                sScript += "$('#Precedenti').show();$('#Approva').hide();$('#Stampa').hide();$('#CreaFile').hide();"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.VisualizzaRiepilogo.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub VisualizzaDatiDelibera()
        Dim Numero, Dal, Al As String
        Try
            If New clsCoattivo().GetDelibera(ConstSession.IdEnte, ConstSession.StringConnection, Numero, Dal, Al) Then
                TxtNDelibera.Text = Numero
                txtInizioDelibera.Text = Dal
                txtFineDelibera.Text = Al
            Else
                RegisterScript("GestAlert('a', 'warning', '', '', 'Delibera Mancante! Impossibile proseguire!');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.VisualizzaDatiDelibera.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class