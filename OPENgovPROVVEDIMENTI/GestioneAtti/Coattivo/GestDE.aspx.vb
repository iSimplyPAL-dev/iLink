Imports ComPlusInterface
Imports log4net

Public Class GestDE
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestDE))
    Protected FncGrd As New Formatta.FunctionGrd

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        infoEnte.InnerText = ConstSession.DescrizioneEnte
        info.InnerText = "Gestione Atti - Coattivo - Ricerca Data Entry"
        infoEntePunt.InnerText = ConstSession.DescrizioneEnte
        infoPunt.InnerText = "Gestione Atti - Coattivo - Data Entry"
        Dim sScript As String = ""
        sScript += "$('#divDE').hide();"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim myUtil As New MyUtility
        Try
            If Page.IsPostBack = False Then
                myUtil.FillDropDownSQLValueString(DdlRicTributo, New DBPROVVEDIMENTI.ProvvedimentiDB().GetTributi("", "", Nothing), -1, "...")
                myUtil.FillDropDownSQLValueString(DdlTributo, New DBPROVVEDIMENTI.ProvvedimentiDB().GetTributi("", "", Nothing), -1, "...")
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "CoattivoRicercaDE", Utility.Costanti.AZIONE_LETTURA.ToString, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Bottoni"
    ''' <summary>
    ''' Pulsante per la ricerca dei coattivi inseriti manualmente e non ancora facenti parte di un ruolo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSearch.Click
        Dim fncElab As New clsCoattivo

        Try
            Dim ListCoattivi() As ObjCoattivo
            ListCoattivi = New clsCoattivo().GetListCoattivi(ConstSession.StringConnection, -1)
            Session("ListCoattiviDE") = ListCoattivi
            LoadSearch(0)
            Dim sScript As String = ""
            If Not ListCoattivi Is Nothing Then
                If ListCoattivi.Length <= 0 Then
                    sScript = "$('#LblResult').show();"
                Else
                    sScript = "$('#LblResult').hide();"
                End If
            End If
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.CmdSearch_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la ricerca dei coattivi inseriti manualmente e non ancora facenti parte di un ruolo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdNewInsert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdNewInsert.Click
        Dim fncElab As New clsCoattivo

        Try
            LoadCoattivo(New ObjCoattivo)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.CmdNewInsert_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per il salvataggio dei dati inseriti in videata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSave.Click
        Dim myItem As New ObjCoattivo
        Dim sScript As String = ""

        Try
            If ControlloDati(sScript) = False Then
                sScript = "GestAlert('a', 'warning', '', '', '" & sScript & "');"
                RegisterScript(sScript, Me.GetType)
            Else
                'carico i dati
                myItem = ReadCoattivo()
                If myItem.COD_CONTRIBUENTE > 1 Then
                    If New clsCoattivo().SetDECoattivo(ConstSession.StringConnection, myItem, Utility.Costanti.AZIONE_NEW) <= 0 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in salvataggio');"
                        RegisterScript(sScript, Me.GetType)
                    Else
                        'ripulisco tutti i dati e torno alla ricerca
                        LoadCoattivo(New ObjCoattivo)
                        sScript = "GestAlert('a', 'info', '', '', 'Salvataggio effettuato con successo!');"
                        sScript += "divDE.style.display = 'none';divRicerca.style.display = '';"
                        RegisterScript(sScript, Me.GetType)
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Errore in salvataggio');"
                    RegisterScript(sScript, Me.GetType)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.CmdSave_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la cancellazione del singolo coattivo a video
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDelete.Click
        Dim myItem As New ObjCoattivo
        Dim sScript As String = ""

        Try
            'carico i dati
            myItem = ReadCoattivo()
            myItem.DataVariazione = Now
            If myItem.COD_CONTRIBUENTE > 1 Then
                If New clsCoattivo().SetDECoattivo(ConstSession.StringConnection, myItem, Utility.Costanti.AZIONE_DELETE) <= 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Errore in salvataggio');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    'ripulisco tutti i dati e torno alla ricerca
                    LoadCoattivo(New ObjCoattivo)
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Errore in salvataggio');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.CmdDelete_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sNameXLS As String
        Dim sScript As String = ""
        Dim x, nCol As Integer
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList

        If CType(Session("ListCoattiviDE"), ObjCoattivo()).Length <= 0 Then
            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario effettuare una ricerca!');"
            RegisterScript(sScript, Me.GetType)
        Else
            Try
                nCol = 14
                DtDatiStampa = New clsCoattivo.clsStampa().PrintDataEntry(CType(Session("ListCoattiviDE"), ObjCoattivo()), ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, nCol)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestRuolo.StampaMinuta_Click.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End Try

            If Not DtDatiStampa Is Nothing Then
                sNameXLS = ConstSession.IdEnte + "_DATAENTRYCOATTIVI_" + Format(Now, "yyyyMMdd_hhmmss") & ".xls"
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
            Else
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
                sScript += "DivAttesa.style.display='none';"
                RegisterScript(sScript, Me.GetType)
            End If
        End If
    End Sub
#End Region
#Region "Griglie"
    ''' <summary>
    ''' Metodo per la gestione degli eventi sulla griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim sScript As String = ""
        Dim IDRow As Integer

        Try
            IDRow = CInt(e.CommandArgument.ToString())
            Select Case e.CommandName
                Case "RowOpen"
                    Dim myCoattivo As New ObjCoattivo
                    myCoattivo = New clsCoattivo().GetCoattivo(ConstSession.StringConnection, IDRow)
                    If Not myCoattivo Is Nothing Then
                        LoadCoattivo(myCoattivo)
                    Else
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore in visualizzazione!');"
                        sScript += "DivAttesa.style.display='none';"
                        RegisterScript(sScript, Me.GetType)
                    End If
            End Select
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
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
            GrdSearch.DataSource = CType(Session("ListCoattiviDE"), ObjCoattivo())
            If page.HasValue Then
                GrdSearch.PageIndex = page.Value
            End If
            GrdSearch.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            RegisterScript("$('#divRicerca').show();$('#divDE').hide();", Me.GetType())
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per il popolamento dei dati del singolo coattivo
    ''' </summary>
    ''' <param name="myItem"></param>
    Private Sub LoadCoattivo(myItem As ObjCoattivo)
        Dim ActAnag As Integer
        Try
            hdIdContribuente.Value = myItem.COD_CONTRIBUENTE
            If myItem.COD_CONTRIBUENTE > 0 Then
                ActAnag = Utility.Costanti.AZIONE_LETTURA
                hfIdCoattivo.Value = myItem.Id
                DdlTributo.SelectedValue = myItem.COD_TRIBUTO
                TxtAnno.Text = myItem.ANNO
                TxtDataAtto.Text = myItem.DATA_ATTO_DEFINITIVO
                TxtDataNotifica.Text = myItem.DATA_NOTIFICA_AVVISO
                TxtNumAtto.Text = myItem.NUMERO_ATTO
                If myItem.DescrTributo.ToUpper.IndexOf("UFFICIO") > 1 Then
                    RbUfficio.Checked = True
                Else
                    RbRettifica.Checked = True
                End If
                TxtImpDiffImposta.Text = myItem.ImportoCoattivo
                TxtImpSanzioni.Text = myItem.IMPORTO_SANZIONI
                TxtImpInteressi.Text = myItem.InteressiCoattivo
                TxtImpSpeseNot.Text = myItem.SpeseCoattivo
                TxtImpTotale.Text = myItem.TotaleCoattivo
            Else
                ActAnag = Utility.Costanti.AZIONE_NEW
            End If
            ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & hdIdContribuente.Value & "&Azione=" & ActAnag)
            hfIdCoattivo.Value = myItem.Id
            RegisterScript("$('#divRicerca').hide();$('#divDE').show();", Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function ReadCoattivo() As ObjCoattivo
        Dim myItem As New ObjCoattivo
        Try
            myItem.Id = Utility.StringOperation.FormatInt(hfIdCoattivo.Value)
            myItem.COD_CONTRIBUENTE = Utility.StringOperation.FormatInt(hdIdContribuente.Value)
            myItem.COD_TRIBUTO = Utility.StringOperation.FormatString(DdlTributo.SelectedValue)
            myItem.ANNO = Utility.StringOperation.FormatString(TxtAnno.Text)
            myItem.DATA_ATTO_DEFINITIVO = Utility.StringOperation.FormatString(TxtDataAtto.Text)
            myItem.DATA_NOTIFICA_AVVISO = Utility.StringOperation.FormatString(TxtDataNotifica.Text)
            myItem.NUMERO_ATTO = Utility.StringOperation.FormatString(TxtNumAtto.Text)
            myItem.ImportoCoattivo = Utility.StringOperation.FormatDouble(TxtImpDiffImposta.Text)
            myItem.IMPORTO_SANZIONI = Utility.StringOperation.FormatDouble(TxtImpSanzioni.Text)
            myItem.InteressiCoattivo = Utility.StringOperation.FormatDouble(TxtImpInteressi.Text)
            myItem.SpeseCoattivo = Utility.StringOperation.FormatDouble(TxtImpSpeseNot.Text)
            myItem.TotaleCoattivo = Utility.StringOperation.FormatDouble(TxtImpTotale.Text)
            If RbUfficio.Checked Then
                myItem.DescrTributo = RbUfficio.Text
            Else
                myItem.DescrTributo = RbRettifica.Text
            End If
            myItem.DataInserimento = Now
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.ReadCoattivo.errore: ", ex)
            myItem = New ObjCoattivo
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return myItem
    End Function
    Private Function ControlloDati(ByRef sMyErr As String) As Boolean
        Try
            If Utility.StringOperation.FormatInt(hdIdContribuente.Value) <= 0 Then
                sMyErr = "E' necessario selezionare il Contribuente!"
                Return False
            End If
            If DdlTributo.SelectedValue = "-1" Then
                sMyErr = "E' necessario inserire il Tributo!"
                Return False
            End If
            If TxtAnno.Text = "" Then
                sMyErr = "E' necessario inserire l'Anno!"
                Return False
            End If
            If TxtNumAtto.Text = "" Then
                sMyErr = "E' necessario inserire il Numero dell'Atto!"
                Return False
            End If
            If TxtDataAtto.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Data Atto!"
                Return False
            End If
            If TxtImpDiffImposta.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Importo Differenza d'Imposta!"
                Return False
            End If
            If TxtImpSanzioni.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Importo Sanzioni!"
                Return False
            End If
            If TxtImpInteressi.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Importo Interessi!"
                Return False
            End If
            If TxtImpSpeseNot.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Importo Spese Notifica!"
                Return False
            End If
            If TxtImpTotale.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Importo Totale!"
                Return False
            End If
            If TxtDataNotifica.Text = "" Then
                sMyErr = "E' necessario valorizzare il campo Data Notifica!"
                Return False
            Else
                'controllo che la Notifica sia dal 2020 in poi
                If Utility.StringOperation.FormatDateTime(TxtDataNotifica.Text).Year < 2020 Then
                    sMyErr = "La Data di Notifica deve essere maggiore di 31/12/2019!"
                    Return False
                End If
            End If
            'controllo che siano coerenti Notifica e Data Atto
            If CDate(TxtDataNotifica.Text) <= Utility.StringOperation.FormatDateTime(TxtDataAtto.Text) Then
                sMyErr = "La Data di Notifica e' minore/uguale alla Data dell'Atto!"
            End If
            'il totale deve essere coerente
            If Utility.StringOperation.FormatDouble(TxtImpTotale.Text) <> Utility.StringOperation.FormatDouble(TxtImpDiffImposta.Text) + Utility.StringOperation.FormatDouble(TxtImpSanzioni.Text) + Utility.StringOperation.FormatDouble(TxtImpInteressi.Text) + Utility.StringOperation.FormatDouble(TxtImpSpeseNot.Text) Then
                sMyErr = "Totale non coerente con somma di Importi!"
                Return False
            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Coattivo.GestDE.ControlloDati_Click.errore: ", ex)
            sMyErr = "ControlloDati::" & ex.Message
            Return False
        End Try
    End Function
End Class