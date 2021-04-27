Imports log4net
''' <summary>
''' Simile all’attuale funzione di modifiche tributarie, che sarà dismessa, ma non essendo più in essere la gestione immobiliare da Territorio, dalla videata saranno tolti:
''' •	i filtri su posizione Trattata/Non Trattata;
''' •	il pulsante per settare tutto come trattato;
''' •	la colonna di selezione e la colonna di apertura situazione Territorio sulla griglia.
''' Le causali saranno ricodificate ed aggiunte per gestire le seguenti azioni:
''' •	Nuova Anagrafica
''' •	Variazione Anagrafica
''' •	Cancellazione Anagrafica
''' •	Nuovo immobile
''' •	Variazione immobile
''' •	Cancellazione immobile
''' •	Elaborazione ruolo (viene considerata la sola data calcolo)
''' •	Nuovo sgravio
''' •	Nuovo Provvedimento (che comprende la generazione di Autotutela di Rettifica)
''' •	Variazione Provvedimento (per tutti gli inserimenti di date)
''' •	Cancellazione Provvedimento (ossia generazione di Autotutela di Annullamento)
''' •	Nuova rateizzazione su Provvedimento
''' •	Nuovo Pagamento
''' •	Variazione Pagamento
''' •	Cancellazione Pagamento
''' Sopra la griglia dei risultati a destra sarà inserito il totalizzatore delle posizioni.
''' Sarà aggiunto il pulsante “Stampa” per l'estrazione in formato Excel del risultato della ricerca ed il pulsante “Analisi” per avere una rappresentazione aggregata del risultato della ricerca; il risultato sarà raggruppato per:
''' •	Operatore, solo nel caso non sia stato selezionato un operatore specifico;
''' •	Tributo, solo nel caso non sia stato selezionato un tributo specifico;
''' •	Tempo, di default settimana ma con possibilità di modifica in giorni o mesi.
''' La ricerca sarà abilitata anche In caso di rappresentazione aggregata.
''' </summary>
Public Class AnalisiEventi
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(AnalisiEventi))

    Private Sub AnalisiEventi_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            lblTitolo.Text = COSTANTValue.ConstSession.DescrizioneEnte
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.AnalisiEventi_Init.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim oLoadCombo As New OPENgovTIA.generalClass.generalFunction
                oLoadCombo.LoadComboGenerale(DdlOperatore, " exec prc_GetUtentiEnte @IdEnte='" + COSTANTValue.ConstSession.IdEnte + "'", COSTANTValue.ConstSession.StringConnectionOPENgov, True, OPENgovTIA.Costanti.TipoDefaultCmb.STRINGA)
                oLoadCombo.LoadComboGenerale(DdlTributo, "exec prc_GetTributi", COSTANTValue.ConstSession.StringConnectionOPENgov, True, OPENgovTIA.Costanti.TipoDefaultCmb.STRINGA)
                Try
                    DdlTributo.Items.Add("Accertamenti")
                    DdlTributo.Items(DdlTributo.Items.Count - 1).Value = "9999"

                    DdlCausali.Items.Clear()
                    DdlCausali.Items.Add("...")
                    DdlCausali.Items(0).Value = ""
                    Dim myObj As New Utility.Costanti.LogEventArgument()
                    For Each p As System.Reflection.PropertyInfo In myObj.GetType().GetProperties()
                        If p.CanRead Then
                            For x As Integer = 0 To 2
                                Dim myAction As String = ""
                                Select Case x
                                    Case Utility.Costanti.AZIONE_NEW
                                        myAction = "Inserimento"
                                    Case Utility.Costanti.AZIONE_UPDATE
                                        myAction = "Modifica"
                                    Case Utility.Costanti.AZIONE_DELETE
                                        myAction = "Cancellazione"
                                End Select
                                DdlCausali.Items.Add((p.GetValue(myObj, Nothing)).replace("RettificaAvviso", "Rettifica Ordinario") + " " + myAction)
                                DdlCausali.Items(DdlCausali.Items.Count - 1).Value = p.GetValue(myObj, Nothing) + "|" + x.ToString
                            Next
                        End If
                    Next
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.loadComboCausali.errore: ", ex)
                End Try
                Try
                    ddlTipoQta.Items.Clear()
                    ddlTipoQta.Items.Add("Giorni")
                    ddlTipoQta.Items(0).Value = "G"
                    ddlTipoQta.Items.Add("Settimane")
                    ddlTipoQta.Items(1).Value = "S"
                    ddlTipoQta.Items.Add("Mesi")
                    ddlTipoQta.Items(2).Value = "M"
                    ddlTipoQta.SelectedIndex = 1
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.loadComboTipoQta.errore: ", ex)
                End Try
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub CmdSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim Dal, Al As DateTime
            Dim nPosizioni As Integer = 0
            Dim sScript As String = ""

            If TxtDal.Text.Trim <> "" Then
                Dal = TxtDal.Text.Trim
            Else
                Dal = Now.AddMonths(-1)
            End If
            If TxtAl.Text.Trim <> "" Then
                Al = TxtAl.Text.Trim
            Else
                Al = Now
            End If
            Dim myDataset As DataSet = New clsInterrogazioni().GetAnalisiEventi(COSTANTValue.ConstSession.IdEnte, DdlTributo.SelectedValue, DdlCausali.SelectedValue, DdlOperatore.SelectedValue, Dal, Al, COSTANTValue.ConstSession.StringConnectionOPENgov)
            If Not IsNothing(myDataset) Then
                GrdResult.DataSource = myDataset.Tables(0)
                GrdResult.DataBind()
                nPosizioni = myDataset.Tables(0).Rows.Count
            End If
            If nPosizioni > 0 Then
                LblResult.Text = "Sono stati trovati " + nPosizioni.ToString + " eventi."
                GrdResult.Visible = True
            Else
                LblResult.Text = "Non sono stati trovati eventi."
                GrdResult.Visible = False
                sScript += "$('#Chart').addClass('DisableBtn');$('#Chart').prop('disabled', true);"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.btnSearch_click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    Protected Sub CmdChart_Click(sender As Object, e As System.EventArgs) Handles btnChart.Click
        Try
            Dim Dal, Al As DateTime
            Dim nPosizioni As Integer = 0
            Dim sScript As String = ""

            If TxtDal.Text.Trim <> "" Then
                Dal = TxtDal.Text.Trim
            Else
                Dal = Now.AddMonths(-1)
            End If
            If TxtAl.Text.Trim <> "" Then
                Al = TxtAl.Text.Trim
            Else
                Al = Now
            End If
            If txtQta.Text = "" Then
                txtQta.Text = "1"
            End If
            Dim myDataset As DataSet = New clsInterrogazioni().GetChartAnalisiEventi(COSTANTValue.ConstSession.IdEnte, DdlTributo.SelectedValue, DdlCausali.SelectedValue, DdlOperatore.SelectedValue, Dal, Al, CInt(txtQta.Text), ddlTipoQta.SelectedValue, COSTANTValue.ConstSession.StringConnectionOPENgov)
            sScript += "$('#divChart').html('"
            If Not IsNothing(myDataset) Then
                sScript += "<span class=\'heading NormalBold_title\'>Raffronto Eventi</span>"
                Dim nEventi As Integer
                For Each myRow As DataRow In myDataset.Tables("operatori").Rows
                    nEventi += Utility.StringOperation.FormatInt(myRow("valore"))
                Next
                sScript += "<p class=\'Input_Emphasized Input_Padding\'>" + nEventi.ToString + " eventi totali</p>"
                If DdlOperatore.SelectedValue = "" Then
                    sScript += GetChart("Analisi per Operatore", myDataset.Tables("operatori"))
                End If
                If DdlTributo.SelectedValue = "" Then
                    sScript += GetChart("Analisi per Tributo", myDataset.Tables("tributo"))
                End If
                sScript += GetChart("Analisi per Tempo", myDataset.Tables("tempo"))
            Else
                sScript += "<span class='heading'>Raffronto Eventi</span>"
                sScript += "<p>Nessun evento trovato.</p>"
            End If
            sScript += "');"
            sScript += "$('#Search').addClass('DisableBtn');$('#Search').prop('disabled', true);"
            sScript += "$('#divResult').hide();$('#divChart').show();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.CmdChart_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    Private Function GetChart(TitleChart As String, myTable As DataTable) As String
        Dim sScript As String = ""
        Try
            sScript += "<hr>"
            sScript += "<span class=\'heading NormalBold_title\'>" + TitleChart + "</span>"
            Dim nRow As Integer = 0
            For Each myRow As DataRow In myTable.Rows
                nRow += 1
                sScript += "<div class=\'row\'>"
                sScript += "<div class=\'side\'>"
                sScript += "<div class=\'Input_Label\'>" + myRow("descrizione").ToString.Replace("'", "&#39;") + "</div>"
                sScript += "</div>"
                sScript += "<div class=\'middle\'>"
                sScript += "<div class=\'bar-container\'>"
                sScript += "<div class=\'bar-chart bar-" + nRow.ToString + "\' style=\'width:" + myRow("valore").ToString + "%\'></div>"
                sScript += "</div>"
                sScript += "</div>"
                sScript += "<div class=\'side right\'>"
                sScript += "<div class=\'bar-val Input_Label\'> " + myRow("valore").ToString + "</div>"
                sScript += "</div>"
                sScript += "</div>"
            Next
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Interrogazioni.AnalisiEventi.GetChart.errore: ", ex)
            sScript = ""
        End Try
        Return sScript
    End Function
End Class