Imports log4net
''' <summary>
''' Pagina per la gestione della modificha tributaria.
''' Contiene i parametri di gestione e le funzioni della comandiera. 
''' Le possibili opzioni sono:
''' - Inserisci variazione tributaria
''' </summary>
Public Class IntestazVarTrib
    Inherits BaseEnte
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(IntestazVarTrib))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SearchParams As New objVariazioniTributiSearch
        Dim idRif As Integer = 0

        Try
            If Page.IsPostBack = False Then
                Log.Debug("IntestazVarTrib::Page_Load::entrata")
                VariazioneTributaria.Style.Add("display", "none")
                If Not Request.Item("IdVariazioneTributaria") Is Nothing Then
                    ConstSession.IdVariazioneTributaria = Request.Item("IdVariazioneTributaria")
                End If
                Log.Debug("ho IdVariazioneTributaria::" & ConstSession.IdVariazioneTributaria)
                If ConstSession.IdVariazioneTributaria > 0 Then
                    SearchParams.IdVariazione = ConstSession.IdVariazioneTributaria
                    Log.Debug("IntestazVarTrib::Page_Load::devo prelevare variazione")
                    Dim dvMyDati As DataView = MetodiVariazioniTributi.SearchVariazioniTributi(SearchParams, ConstSession.StringConnectionOPENgov)
                    Log.Debug("IntestazVarTrib::Page_Load::prelevata")
                    If Not dvMyDati Is Nothing Then
                        For Each MyRow As DataRowView In dvMyDati
                            LblTributo.Text = Utility.StringOperation.FormatString(MyRow("tributo"))
                            LblFg.Text = Utility.StringOperation.FormatString(("foglio"))
                            LblNum.Text = Utility.StringOperation.FormatString(("numero"))
                            LblSub.Text = Utility.StringOperation.FormatString(("subalterno"))
                            LblCausale.Text = Utility.StringOperation.FormatDateTime(("causale"))
                            If Utility.StringOperation.FormatDateTime(("datatrattato")) = Date.MaxValue.ToShortDateString Then
                                LblTrattato.Text = " già trattata"
                            Else
                                LblTrattato.Text = " da trattare"
                            End If
                            LblDataVariazione.Text = Utility.StringOperation.FormatString(("datavariazione_v"))
                            LblOperatore.Text = Utility.StringOperation.FormatString(("operatore"))
                            idRif = Utility.StringOperation.FormatInt(("idrifui"))
                            LblInfoDich.Text = Utility.StringOperation.FormatString(("infodich"))
                        Next
                        VariazioneTributaria.Style.Add("display", "")
                    End If
                End If
                If idRif <= 0 Then
                    InsertVarTrib.Style.Add("display", "")
                Else
                    InsertVarTrib.Style.Add("display", "none")
                End If
                Log.Debug("IntestazVarTrib::Page_Load::uscita")
            End If
        Catch ex As Exception
            Log.Error("Si è verificato un errore in IntestazVarTrib::Page_Load::" & ex.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdInsertVarTrib_Click(sender As Object, e As System.EventArgs) Handles CmdInsertVarTrib.Click
        Dim FncDB As New ClsDB
        Dim oMyFab As New ObjFabbricato
        Dim FncVarTrib As New MetodiVariazioniTributi
        Dim sScript As String = ""
        Dim FncModTrib As New Utility.ModificheTributarie

        Try
            If ConstSession.IdVariazioneTributaria > 0 Then
                oMyFab = FncVarTrib.GetFabToTerr(ConstSession.IdVariazioneTributaria, ConstSession.StringConnectionOPENgov)
                If Not oMyFab Is Nothing Then
                    If FncVarTrib.SetFabToTerr(ConstSession.StringConnection, oMyFab) Then
                        If FncModTrib.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Update, ConstSession.IdVariazioneTributaria, "", "", -1, "", "", "", DateTime.MaxValue, "", -1, Now) = False Then
                            sScript += "GestAlert('a', 'danger', '', '', 'Errore in aggiornamento variazione trattata!');"
                        Else
                            sScript = "GestAlert('a', 'success', '', '', 'Salvataggio effettuato con successo!');"
                            sScript += "parent.parent.Comandi.location.href = '../Generali/asp/aspVuotaRemoveComandi.aspx';"
                            sScript += "parent.parent.Visualizza.location.href = 'VariazioniTributiSearch.aspx'"
                        End If
                    Else
                        sScript = "GestAlert('a', 'danger', '', '', 'Salvataggio non effettuato a causa di un errore!')"
                    End If
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Salvataggio non effettuato a causa di un errore!')"
                End If
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Error("Si è verificato un errore in IntestazVarTrib::CmdInsertVarTrib_Click::" & ex.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
End Class