Imports log4net
''' <summary>
''' Pagina per la gestione dei dati dell'ente.
''' Le possibili opzioni sono:
''' - salva
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' Sarà aggiunta la gestione del flag RUOLO INSOLUTI, se attivo sarà visualizzata la tipologia ruoli specifica e la voce di menù per la gestione date di notifica.
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="15/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Clausole contrattuali</em>
''' Per questioni di sicurezza non è meglio non visualizzare un file dentro ad una pagina ma è preferibile farlo scaricare dall’utente; per questo motivo le clausole contrattuali non saranno visibili da voce di menù ad hoc ma saranno invece scaricabili da apposito pulsante nella pagina di gestione dell’ente.
''' Il PDF delle condizioni contrattuali da visualizzare dovrà essere presente nella directory \DATI\OPENGOV\CONTRATTI\, il nome del file sarà configurato in nuovo campo nella tabella di gestione dell’ente; il campo non sarà gestibile in autonomia dagli operatori ma la gestione ed il caricamento del file sarà a cura dello sviluppo.
''' </revision>
''' </revisionHistory>
Partial Class ConfiguraEnte
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfiguraEnte))
    Private FncEnti As New SelectEnti
    Private FncCC As New OPENUtility.ClsContiCorrenti
    Protected FncGrd As New FunctionGrd

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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="15/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Clausole contrattuali</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim dvMyDati As New DataView
        Dim oContoCorrente() As OPENUtility.objContoCorrente

        Try
            If Page.IsPostBack = False Then
                LoadCombos()
                Session("ListCC") = Nothing
                dvMyDati = FncEnti.GetEnte(COSTANTValue.ConstSession.IdEnte, "")
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        txtIdEnte.Text = myRow("cod_ente").ToString
                        txtDescrEnte.Text = myRow("descrizione_ente").ToString
                        If Not IsDBNull(myRow("cod_ente_cnc")) Then
                            txtCodEnteCNC.Text = myRow("cod_ente_cnc").ToString
                        End If
                        If Not IsDBNull(myRow("IdEnte_CredBen")) Then
                            txtIdEnteCredBen.Text = myRow("IdEnte_CredBen").ToString
                        End If
                        If Not IsDBNull(myRow("denominazione")) Then
                            txtDenominazione.Text = myRow("denominazione").ToString
                        End If
                        If Not IsDBNull(myRow("ambiente")) Then
                            txtAmbiente.Text = myRow("ambiente").ToString
                        End If
                        If Not IsDBNull(myRow("cod_belfiore")) Then
                            txtBelfiore.Text = myRow("cod_belfiore").ToString
                        End If
                        If Not IsDBNull(myRow("indirizzo_civico")) Then
                            txtIndirizzo.Text = myRow("indirizzo_civico").ToString
                        End If
                        If Not IsDBNull(myRow("localita")) Then
                            txtComune.Text = myRow("localita").ToString
                        End If
                        If Not IsDBNull(myRow("cap")) Then
                            txtCap.Text = myRow("cap").ToString
                        End If
                        If Not IsDBNull(myRow("provincia_sigla")) Then
                            txtPv.Text = myRow("provincia_sigla").ToString
                        End If
                        If Not IsDBNull(myRow("provincia_estesa")) Then
                            txtPvEstesa.Text = myRow("provincia_estesa").ToString
                        End If
                        If Not IsDBNull(myRow("e_mail")) Then
                            txtEMail.Text = myRow("e_mail").ToString
                        End If
                        If Not IsDBNull(myRow("telefono")) Then
                            txtTel.Text = myRow("telefono").ToString
                        End If
                        If Not IsDBNull(myRow("fax")) Then
                            txtFax.Text = myRow("fax").ToString
                        End If
                        If Not IsDBNull(myRow("num_abitanti")) Then
                            txtNumAbitanti.Text = myRow("num_abitanti").ToString
                        End If
                        If Not IsDBNull(myRow("num_nuclei_fam")) Then
                            txtNucleiFam.Text = myRow("num_nuclei_fam").ToString
                        End If
                        If Not IsDBNull(myRow("posizionegeografica")) Then
                            txtPosGeo.Text = myRow("posizionegeografica").ToString
                        End If
                        If Not IsDBNull(myRow("fk_IdTypeAteco")) Then
                            If myRow("fk_IdTypeAteco") = 1 Then
                                opt1.Checked = True
                            Else
                                opt2.Checked = True
                            End If
                        End If
                        If Not IsDBNull(myRow("hasgis")) Then
                            chkGis.Checked = myRow("hasgis")
                        End If
                        '**** 201809 - Cartelle Insoluti ***
                        If Not IsDBNull(myRow("hasruoloinsoluti")) Then
                            chkRuoloInsoluti.Checked = myRow("hasruoloinsoluti")
                        End If
                        If Not IsDBNull(myRow("TributiBollettinoF24")) Then
                            txtTributiBollettinoF24.Text = myRow("TributiBollettinoF24").ToString
                        End If
                        '*** ***
                        If Not IsDBNull(myRow("perccontributoancicnc")) Then
                            txtPercAnciCnc.Text = myRow("perccontributoancicnc").ToString
                        End If
                        If Not IsDBNull(myRow("note_ente")) Then
                            txtNote.Text = myRow("note_ente").ToString
                        End If
                        If Not IsDBNull(myRow("cf_piva")) Then
                            txtAECFPIVA.Text = myRow("cf_piva").ToString
                        End If
                        If Not IsDBNull(myRow("piva")) Then
                            txtAEPIVA.Text = myRow("piva").ToString
                        End If
                        If Not IsDBNull(myRow("cognome")) Then
                            txtAECognome.Text = myRow("cognome").ToString
                        End If
                        If Not IsDBNull(myRow("nome")) Then
                            txtAENome.Text = myRow("nome").ToString
                        End If
                        If Not IsDBNull(myRow("sesso")) Then
                            ddlAESex.SelectedValue = myRow("sesso").ToString
                        End If
                        If Not IsDBNull(myRow("data_nascita")) Then
                            txtAEData.Text = myRow("data_nascita").ToString
                        End If
                        If Not IsDBNull(myRow("comune_nascitasede")) Then
                            txtAEComune.Text = myRow("comune_nascitasede").ToString
                        End If
                        If Not IsDBNull(myRow("pv_nascitasede")) Then
                            txtAEPV.Text = myRow("pv_nascitasede").ToString
                        End If
                        Dim sScript As String = ""
                        If myRow("clausolecontrattuali").ToString <> "" Then
                            sScript = "$('#btnDownload').click(function(){window.open('../../" + COSTANTValue.ConstSession.GetRepositoryContratti + "/" + myRow("clausolecontrattuali").ToString + "');});"
                            sScript += "$('#divDownloadXLS').show();"
                            hfClausoleContrattuali.Value = "1"
                        Else
                            sScript += "$('#divDownloadXLS').hide();"
                            hfClausoleContrattuali.Value = "0"
                        End If
                        RegisterScript(sScript, Me.GetType())
                    Next
                    oContoCorrente = FncCC.GetContoCorrente(COSTANTValue.ConstSession.IdEnte, COSTANTValue.ConstSession.StringConnectionOPENgov)
                    If Not oContoCorrente Is Nothing Then
                        GrdCC.DataSource = oContoCorrente
                        GrdCC.DataBind()
                        Session("ListCC") = oContoCorrente
                    End If
                End If
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "Ente", "ConfiguraEnte", Utility.Costanti.AZIONE_LETTURA.ToString, "", COSTANTValue.ConstSession.IdEnte, -1)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(sender As Object, e As System.EventArgs) Handles btnSalva.Click
        Dim myEnte As New Ente
        Dim sScript As String = ""

        Try
            myEnte.IdEnte = txtIdEnte.Text
            myEnte.DescrizioneEnte = txtDescrEnte.Text
            myEnte.Ambiente = txtAmbiente.Text
            myEnte.Denominazione = txtDenominazione.Text
            myEnte.CodBelfiore = txtBelfiore.Text
            myEnte.CodEnteCNC = txtCodEnteCNC.Text
            myEnte.IdEnteCredBen = txtIdEnteCredBen.Text
            myEnte.Indirizzo = txtIndirizzo.Text
            myEnte.CAP = txtCap.Text
            myEnte.Localita = txtComune.Text
            myEnte.PVSigla = txtPv.Text
            myEnte.PVEstesa = txtPvEstesa.Text
            myEnte.Telefono = txtTel.Text
            myEnte.Fax = txtFax.Text
            myEnte.EMail = txtEMail.Text
            If txtNumAbitanti.Text = "" Then
                myEnte.NumAbitanti = 0
            Else
                myEnte.NumAbitanti = Integer.Parse(txtNumAbitanti.Text)
            End If

            If txtNucleiFam.Text = "" Then
                myEnte.NumNucleiFam = 0
            Else
                myEnte.NumNucleiFam = Integer.Parse(txtNucleiFam.Text)
            End If
            'myEnte.NumNucleiFam = txtNucleiFam.Text
            If opt1.Checked Then
                myEnte.IdTypeATECO = 1
            Else
                myEnte.IdTypeATECO = 2
            End If
            myEnte.PosizioneGeografica = txtPosGeo.Text
            myEnte.hasGIS = chkGis.Checked
            '**** 201809 - Cartelle Insoluti ***
            myEnte.HasRuoloInsoluti = chkRuoloInsoluti.Checked
            myEnte.TributiBollettinoF24 = txtTributiBollettinoF24.Text
            '*** ***
            myEnte.PercContributoANCICNC = txtPercAnciCnc.Text
            myEnte.CFPIva = txtAECFPIVA.Text
            myEnte.PIva = txtAEPIVA.Text
            myEnte.Cognome = txtAECognome.Text
            myEnte.Nome = txtAENome.Text
            myEnte.Sesso = ddlAESex.SelectedValue
            myEnte.DataNascita = txtAEData.Text
            myEnte.ComuneNascita = txtAEComune.Text
            myEnte.PVNascita = txtAEPV.Text
            myEnte.Note = txtNote.Text

            If FncEnti.SetEnte(COSTANTValue.ConstSession.StringConnectionOPENgov, myEnte) = False Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in salvataggio dati!')"
                RegisterScript(sScript, Me.GetType())
            Else
                If Not Session("ListCC") Is Nothing Then
                    For Each myCC As OPENUtility.objContoCorrente In Session("ListCC")
                        If myCC.CodTributo <> "" Then
                            If FncCC.SetContoCorrente(COSTANTValue.ConstSession.StringConnectionOPENgov, myCC) <= 0 Then
                                sScript = "GestAlert('a', 'danger', '', '', 'Errore in salvataggio dati Conto!')"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        End If
                    Next
                End If
                sScript = "GestAlert('a', 'success', '', '', 'Salvataggio avvenuto con successo!')"
                RegisterScript(sScript, Me.GetType())
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "Ente", "ConfiguraEnte", Utility.Costanti.AZIONE_UPDATE.ToString, "", COSTANTValue.ConstSession.IdEnte, -1)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.btnSalva_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            If hfClausoleContrattuali.Value = "1" Then
                sScript = "$('#btnDownload').click(function(){window.open('../../" + COSTANTValue.ConstSession.GetRepositoryContratti + "/" + hfClausoleContrattuali.Value + "');});"
                sScript += "$('#divDownloadXLS').show();"
            Else
                sScript += "$('#divDownloadXLS').hide();"
            End If
            RegisterScript(sScript, Me.GetType())
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
                For Each myRow As GridViewRow In GrdCC.Rows
                    If IDRow = CType(myRow.FindControl("hfidcc"), HiddenField).Value Then
                        Try
                            DivCC.Style.Add("display", "")
                            For Each myCC As OPENUtility.objContoCorrente In Session("ListCC")
                                If myCC.IdCC.ToString = IDRow Then
                                    hdIdCC.Value = myCC.IdCC.ToString
                                    If myCC.CodTributo <> "" Then
                                        ddlTributo.SelectedValue = myCC.CodTributo.PadLeft(4, CChar("0"))
                                    End If
                                    txtConto.Text = myCC.ContoCorrente
                                    txtIBAN.Text = myCC.IBAN
                                    txtRiga1.Text = myCC.Intestazione_1
                                    txtRiga2.Text = myCC.Intestazione_2
                                    If myCC.DescrTipologiaConto = "Violazioni" Then
                                        ddlTipo.SelectedValue = "V"
                                    Else
                                        ddlTipo.SelectedValue = "O"
                                    End If
                                    txtAutorizzazione.Text = myCC.Autorizzazione
                                    chkInStampa.Checked = myCC.ContoInStampa
                                    If myCC.DataFineValidita <> Date.MinValue And myCC.DataFineValidita <> Date.MaxValue And myCC.DataFineValidita.ToShortDateString <> DateTime.MaxValue.ToShortDateString And myCC.DataFineValidita <> DateTime.MaxValue.ToShortDateString Then
                                        txtDataFineValidita.Text = myCC.DataFineValidita.ToString
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.GrdRowCommand.errore: ", ex)
                            Response.Redirect("../../../PaginaErrore.aspx")
                        End Try
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSaveCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSaveCC.Click
        Dim mySped As New OPENUtility.objContoCorrente
        Dim ListCC As New Generic.List(Of OPENUtility.objContoCorrente)
        Dim ListCCConf() As OPENUtility.objContoCorrente
        Dim bTrovato As Boolean = False

        Try
            'carico i dati della videata
            mySped = LoadCCFromForm()
            ListCCConf = Session("ListCC")
            If mySped.IdCC = -1 Then
                'carico un id fittizio per non sovrascrivermi se inserisco + di 1 indirizzo nuovo
                mySped.IdCC = (ListCCConf.GetUpperBound(0) + 1 * -100)
            End If
            For Each oSped As OPENUtility.objContoCorrente In ListCCConf
                If oSped.IdCC = mySped.IdCC Then
                    oSped = mySped
                    bTrovato = True
                End If
                If oSped.IdCC > 0 And (oSped.CodTributo = "" Or oSped.ContoCorrente = "") Then
                    Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Impossibile salvare il conto!')"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
                If oSped.CodTributo <> "" Then
                    'il rigo vuoto si aggiunge in coda
                    ListCC.Add(oSped)
                End If
            Next
            If bTrovato = False Then
                ListCC.Add(mySped)
            End If
            ListCC.Add(New OPENUtility.objContoCorrente)
            'controllo che non ci sia un doppio tributo
            For Each oSped As OPENUtility.objContoCorrente In ListCC
                If oSped.CodTributo.PadLeft(4, "0") = mySped.CodTributo And oSped.DataFineValidita = Date.MaxValue And oSped.IdCC <> mySped.IdCC Then
                    SvuotaCC()
                    Dim strBuild As New System.Text.StringBuilder
                    strBuild.Append("GestAlert('a', 'warning', '', '', 'Conto per Tributo gia\' presente!\nImpossibile inserirlo come nuovo!')")
                    RegisterScript(strBuild.ToString(), Me.GetType())
                    Exit Sub
                End If
            Next
            'ricarico la griglia
            Session("ListCC") = ListCC.ToArray
            GrdCC.DataSource = ListCC.ToArray
            GrdCC.DataBind()
            SvuotaCC()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.CmdSaveCC_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadCombos()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetTributi"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlTributo.Items.Clear()
                ddlTributo.Items.Add("...")
                ddlTributo.Items(0).Value = "-1"
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlTributo.Items.Add(myDataReader(0).ToString)
                            ddlTributo.Items(ddlTributo.Items.Count - 1).Value = myDataReader(1).ToString
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.LoadCombos.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End Try
        Catch ex As Exception
            Throw New Exception("Problemi nell'esecuzione di LoadCombo " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function LoadCCFromForm() As OPENUtility.objContoCorrente
        Dim myCC As New OPENUtility.objContoCorrente

        Try
            myCC.IdEnte = COSTANTValue.ConstSession.IdEnte
            myCC.IdCC = hdIdCC.Value
            myCC.CodTributo = ddlTributo.SelectedValue
            myCC.DescrTributo = ddlTributo.SelectedItem.Text
            myCC.DescrTipologiaConto = ddlTipo.SelectedItem.Text
            myCC.ContoCorrente = txtConto.Text
            myCC.IBAN = txtIBAN.Text
            myCC.Intestazione_1 = txtRiga1.Text
            myCC.Intestazione_2 = txtRiga2.Text
            myCC.Autorizzazione = txtAutorizzazione.Text
            myCC.ContoInStampa = chkInStampa.Checked
            If txtDataFineValidita.Text <> "" Then
                myCC.DataFineValidita = txtDataFineValidita.Text
            End If
            Return myCC
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.LoadCCFromForm.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw New Exception("LoadCCFromForm::errore::", ex)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub SvuotaCC()
        Try
            'svuoto le text
            hdIdCC.Value = "-1"
            txtConto.Text = ""
            txtIBAN.Text = ""
            txtRiga1.Text = ""
            txtRiga2.Text = ""
            txtAutorizzazione.Text = ""
            chkInStampa.Checked = False
            txtDataFineValidita.Text = ""
            DivCC.Style.Add("display", "none")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfiguraEnte.SvuotaCC.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdUnloadCC_Click(sender As Object, e As System.EventArgs) Handles CmdUnloadCC.Click
        SvuotaCC()
    End Sub

End Class
