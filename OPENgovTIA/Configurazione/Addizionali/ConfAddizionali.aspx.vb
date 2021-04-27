Imports log4net
''' <summary>
''' Pagina per la gestione delle addizionali.
''' Le possibili opzioni sono:
''' - Elimina
''' - Salva
''' - Torna alla videata precedente
''' - Inserisci nuovo
''' - Ricerca''' 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' Sarà possibile configurare le spese che saranno conteggiate solo in caso di RUOLO INSOLUTI.
''' Nella tabella è aggiunto il campo TIPOCALCOLO che definisce se la voce è a FORMULA {1} o IMPORTO FISSO {2}.
''' Viene aggiunta la tabella TBLADDIZIONALIRUOLI di abbinamento Voce->Tipo Ruolo per ora pre-configurata che abbina le singole addizionali alla tipologia di ruolo calcolabile; per il tipo RUOLO INSOLUTI sarà presente solo la voce delle spese di spedizione, che saranno associate solo a questo tipo di ruolo.
''' </revision>
''' </revisionHistory>
Partial Public Class ConfAddizionali
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfAddizionali))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ConfAddizionali_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Put user code to initialize the page here
        lblTitolo.Text = Session("DESCRIZIONE_ENTE")
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.Text = "TARES " + info.Text
            Else
                info.Text = "TARSU " + info.Text
            End If

            Dim sScript As String
            sScript = "$('#divGest').hide();"
            sScript += "$('#Delete').hide();"
            sScript += "$('#Insert').hide();"
            sScript += "$('#Cancel').hide();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.ConfAddizionali_Init.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim clsAddizionali As New ClsAddizionali
                clsAddizionali.LoadComboAddizionali(ddlRicAddizionali)
                clsAddizionali.LoadComboAddizionali(ddlGestAddizionali)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSearch_Click(sender As Object, e As EventArgs) Handles CmdSearch.Click
        Try
            Dim listAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale

            listAddizionali = New GestAddizionali().GetAddizionale(ConstSession.StringConnection, ConstSession.IdEnte, txtRicAnno.Text, ddlRicAddizionali.SelectedValue, "")

            If Not listAddizionali Is Nothing > 0 Then
                Session("dsAddizionaliEnte") = listAddizionali
                GrdResult.DataSource = listAddizionali
                GrdResult.DataBind()
                LblResult.Text = "Risultati della Ricerca"
            Else
                LblResult.Text = "La ricerca non ha prodotto risultati."
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.CmdSearch_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSalva_Click(sender As Object, e As EventArgs) Handles CmdSalva.Click
        Try
            Dim ClsAddizionali As New ClsAddizionali
            Dim str As String
            Dim strErrore As String = ""
            Dim objAdd As New ObjAddizionali
            Dim objAddOld As New ObjAddizionali
            Dim TipoCalcolo As Integer = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale.Tipo.Formula
            If optFissa.Checked Then
                TipoCalcolo = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale.Tipo.Importo
            End If

            If hfIdAddizionale.Value <> "" Then
                objAdd.ID = hfIdAddizionale.Value
                objAdd.sIDcapitolo = hfIdCapitolo.Value
                objAdd.sValore = txtPercentuale.Text
                objAdd.sAnno = txtAnno.Text

                objAddOld.sAnno = txtAnno.Text
                objAddOld.sIDcapitolo = hfIdCapitolo.Value
                If ClsAddizionali.UpdateAddizionali(objAddOld, objAdd, TipoCalcolo, strErrore) = True Then
                    str = "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente!'); parent.Search();"
                    RegisterScript(str, Me.GetType)
                    str = "UscitaDopoOperazione();"
                    RegisterScript(str, Me.GetType)
                Else
                    str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(str, Me.GetType)
                    str = "UscitaDopoOperazione();"
                    RegisterScript(str, Me.GetType)
                End If
            Else
                objAdd.sIDcapitolo = ddlGestAddizionali.SelectedValue
                objAdd.sValore = txtPercentuale.Text
                objAdd.sAnno = txtAnno.Text
                If ClsAddizionali.SetAddizionali(objAdd, TipoCalcolo, strErrore) = True Then
                    str = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!'); parent.Search();"
                    RegisterScript(str, Me.GetType)
                    str = "UscitaDopoOperazione();"
                    RegisterScript(str, Me.GetType)
                Else
                    str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(str, Me.GetType)
                    str = "UscitaDopoOperazione();"
                    RegisterScript(str, Me.GetType)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.CmdSalva_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdElimina_Click(sender As Object, e As EventArgs) Handles CmdElimina.Click
        Try
            Dim FncAddizionale As New ClsAddizionali
            Dim str As String
            Dim strErrore As String = ""
            Dim myAddizionale As New ObjAddizionali

            myAddizionale.ID = hfIdAddizionale.Value
            myAddizionale.sIDcapitolo = hfIdCapitolo.Value
            myAddizionale.sAnno = txtAnno.Text

            If FncAddizionale.DeleteAddizionale(myAddizionale, strErrore) = True Then
                str = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!'); parent.Search();"
                RegisterScript(str, Me.GetType)
                str = "UscitaDopoOperazione();"
                RegisterScript(str, Me.GetType)
            Else
                str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                RegisterScript(str, Me.GetType)
                str = "UscitaDopoOperazione();"
                RegisterScript(str, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.CmdElimina_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdBack_Click(sender As Object, e As EventArgs) Handles CmdBack.Click
        Try
            hfIdAddizionale.Value = ""
            hfIdCapitolo.Value = ""
            txtPercentuale.Text = ""
            txtAnno.Text = ""
            ddlGestAddizionali.SelectedValue = ""
            Dim sScript As String
            sScript = "$('#divRic').show();$('#divGest').hide();"
            sScript += "$('#Delete').hide();"
            sScript += "$('#Insert').hide();"
            sScript += "$('#Cancel').hide();"
            sScript += "$('#NewInsert').show();$('#Search').show();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.CmdBack_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdNew_Click(sender As Object, e As EventArgs) Handles CmdNew.Click
        Try
            Dim sScript As String
            sScript = "$('#divRic').hide();$('#divGest').show();"
            sScript += "$('#Delete').show();"
            sScript += "$('#Insert').show();"
            sScript += "$('#Cancel').show();"
            sScript += "$('#NewInsert').hide();$('#Search').hide();"
            sScript += "Setfocus(document.getElementById('txtAnno'));"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.CmdNew_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdResult.Rows
                    If IDRow = CType(myRow.FindControl("hfIDADDIZIONALE"), HiddenField).Value Then
                        LoadGestAddizionale(IDRow, myRow.Cells(2).Text, CType(myRow.FindControl("hfIDCAPITOLO"), HiddenField).Value, myRow.Cells(0).Text, CType(myRow.FindControl("hfTipoCalcolo"), HiddenField).Value, "M")
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
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
            GrdResult.DataSource = CType(Session("dsAddizionaliEnte"), DataSet)
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdAddizionale"></param>
    ''' <param name="Percentuale"></param>
    ''' <param name="IdCapitolo"></param>
    ''' <param name="Anno"></param>
    ''' <param name="TipoCalcolo"></param>
    ''' <param name="Operazione"></param>
    Private Sub LoadGestAddizionale(IdAddizionale As Integer, Percentuale As Double, IdCapitolo As String, Anno As String, TipoCalcolo As Integer, Operazione As String)
        Try
            hfIdAddizionale.Value = IdAddizionale
            hfIdCapitolo.Value = IdCapitolo
            txtPercentuale.Text = Percentuale
            txtAnno.Text = Anno
            If TipoCalcolo = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale.Tipo.Importo Then
                optFissa.Checked = True
            Else
                optPercentuale.Checked = True
            End If

            Dim ClsAddizionali As New ClsAddizionali
            ClsAddizionali.LoadComboAddizionali(ddlGestAddizionali)

            ddlGestAddizionali.SelectedValue = IdCapitolo
            If Operazione = "modifica" Then
                ddlGestAddizionali.Enabled = False
            End If

            Dim sScript As String
            sScript = "$('#divRic').hide();$('#divGest').show();"
            sScript += "$('#Delete').show();"
            sScript += "$('#Insert').show();"
            sScript += "$('#Cancel').show();"
            sScript += "$('#NewInsert').hide();$('#Search').hide();"
            sScript += "Setfocus(document.getElementById('txtAnno'));"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfAddizionali.LoadGestAddizionale.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class