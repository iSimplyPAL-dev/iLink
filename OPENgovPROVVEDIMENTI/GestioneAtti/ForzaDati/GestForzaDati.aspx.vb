Imports System.Runtime.Remoting.Channels.Http
Imports ComPlusInterface
Imports log4net
Imports Utility
''' <summary>
''' Pagina dei comandi per la forzatura dei dati del provvedimento.
''' Contiene i parametri di gestione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class GestForzaDati
    Inherits BasePage
    Const LIQUIDAZIONE As String = "L"
    Const ACCERTAMENTO As String = "A"
    Const QUESTIONARIO As String = "Q"
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestForzaDati))

    Private objUtility As New MyUtility

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataStampa As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataConsegna As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataNotifica As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

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
    ''' <revision date="04/07/2012">
    ''' <strong>IMU</strong>
    ''' passaggio tributo da ICI a IMU
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim objHashTable As New Hashtable
        Dim strIndirizzo As String
        Dim strCodFiscalePI As String
        Dim sScript As String = ""
        Dim objDS As DataSet

        Try
            objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
            objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

            If Not Page.IsPostBack Then
                txtAnno.Text = StringOperation.FormatString(Request("ANNO"))
                txtNumeroProvvedimento.Text = StringOperation.FormatString(Request("NUMEROATTO"))

                ViewState.Add("TIPO_PROVVEDIMENTO", StringOperation.FormatString(Request("TIPO_PROVVEDIMENTO")))

                Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
                Dim myAtto As New OggettoAtto
                objDS = objCOMRicerca.GetDatiProvvedimenti(ConstSession.StringConnection, objHashTable, myAtto)

                Session.Remove("PROVVEDIMENTI_DA_STAMPARE")
                Session.Add("PROVVEDIMENTI_DA_STAMPARE", objDS)

                Dim myDataset As DataSet
                Try
                    myDataset = New DBPROVVEDIMENTI.ProvvedimentiDB().GetTRIBUTIProvvedimentiAnno(Utility.StringOperation.FormatString(ConstSession.IdEnte), "-1")
                    For Each myRow As DataRow In myDataset.Tables(0).Rows
                        txtTributo.Text = Utility.StringOperation.FormatString(myRow.Item(1))
                    Next
                Catch Err As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.Page_Load.lbldescrdtributo.errore: ", Err)
                Finally
                    If Not myDataset Is Nothing Then
                        myDataset.Dispose()
                    End If
                End Try

                'carico combo 
                Dim oLoadCombo As New MyUtility
                oLoadCombo.FillDropDownSQL(DdlTipoProvvedimento, New DBPROVVEDIMENTI.ProvvedimentiDB().GetTIPOTRIBUTIProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, StringOperation.FormatString(objDS.Tables("PROVVEDIMENTO").Rows(0)("cod_tributo")).Replace("TASI", Costanti.TRIBUTO_ICI), txtAnno.Text), Request("IDTIPOPROVVEDIMENTO"), "...")

                For Each rowPROVVEDIMENTO As DataRow In objDS.Tables("PROVVEDIMENTO").Rows
                    objHashTable.Add("NUMERO_ATTO", StringOperation.FormatString(rowPROVVEDIMENTO("NUMERO_ATTO")))
                    objHashTable.Add("ANNO_AVVISO", StringOperation.FormatString(Request("ANNO")))
                    objHashTable.Add("COD_CONTRIBUENTE", StringOperation.FormatString(rowPROVVEDIMENTO("COD_CONTRIBUENTE")))

                    txtCOD_CONTRIBUENTE.Text = StringOperation.FormatString(rowPROVVEDIMENTO("COD_CONTRIBUENTE"))
                    txtID_PROVVEDIMENTO.Text = StringOperation.FormatString(rowPROVVEDIMENTO("ID_PROVVEDIMENTO"))
                    txtTIPO_OPERAZIONE.Text = "RETTIFICA"
                    txtDataGenerazione.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ELABORAZIONE")))
                    '*** anagrafica ***
                    hdIdContribuente.Value = StringOperation.FormatInt(rowPROVVEDIMENTO("cod_contribuente"))
                    If ConstSession.HasPlainAnag Then
                        ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & StringOperation.FormatInt(rowPROVVEDIMENTO("cod_contribuente")) & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
                    Else
                        lblCognomeNome.Text = StringOperation.FormatString(rowPROVVEDIMENTO("COGNOME")) & " " & StringOperation.FormatString(rowPROVVEDIMENTO("NOME"))
                        If Len(StringOperation.FormatString(rowPROVVEDIMENTO("PARTITA_IVA"))) = 0 Then
                            strCodFiscalePI = StringOperation.FormatString(rowPROVVEDIMENTO("CODICE_FISCALE"))
                        Else
                            strCodFiscalePI = StringOperation.FormatString(rowPROVVEDIMENTO("PARTITA_IVA"))
                        End If
                        lblCfPiva.Text = strCodFiscalePI
                        If Len(StringOperation.FormatString(rowPROVVEDIMENTO("PROVINCIA_RES"))) = 0 Then
                            strIndirizzo = StringOperation.FormatString(rowPROVVEDIMENTO("VIA_RES")) & " " & StringOperation.FormatString(rowPROVVEDIMENTO("CIVICO_RES")) & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & StringOperation.FormatString(rowPROVVEDIMENTO("CITTA_RES")) & " " & StringOperation.FormatString(rowPROVVEDIMENTO("CAP_RES"))
                        Else
                            strIndirizzo = StringOperation.FormatString(rowPROVVEDIMENTO("VIA_RES")) & " " & StringOperation.FormatString(rowPROVVEDIMENTO("CIVICO_RES")) & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & StringOperation.FormatString(rowPROVVEDIMENTO("CITTA_RES")) & " " & StringOperation.FormatString(rowPROVVEDIMENTO("CAP_RES")) & " " & "(" & StringOperation.FormatString(rowPROVVEDIMENTO("PROVINCIA_RES")) & ")"
                        End If
                        lblResidenza.Text = strIndirizzo
                    End If
                    '*** importi ***
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")).Length = 0 Then
                        txtImportoDifferenzaImposta.Text = "0"
                    Else
                        txtImportoDifferenzaImposta.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI")).Length = 0 Then
                        txtImportoSanzioni.Text = "0"
                    Else
                        txtImportoSanzioni.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTO")).Length = 0 Then
                        txtImportoSanzRid.Text = "0"
                    Else
                        txtImportoSanzRid.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTO")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")).Length = 0 Then
                        txtImportoSanzNonRid.Text = "0"
                    Else
                        txtImportoSanzNonRid.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI")).Length = 0 Then
                        txtImportoInteressi.Text = "0"
                    Else
                        txtImportoInteressi.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ALTRO")).Length = 0 Then
                        txtAltroImporto.Text = "0"
                    Else
                        txtAltroImporto.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ALTRO")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SPESE")).Length = 0 Then
                        txtImportoSpese.Text = "0"
                    Else
                        txtImportoSpese.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SPESE")), 2)
                    End If
                    txtDichiarato.Text = FormatNumber(StringOperation.FormatDouble(rowPROVVEDIMENTO("IMPORTO_DICHIARATO_F2")), 2)
                    txtVersato.Text = FormatNumber(StringOperation.FormatDouble(rowPROVVEDIMENTO("IMPORTO_VERSATO_F2")), 2)
                    txtAccertato.Text = FormatNumber(StringOperation.FormatDouble(rowPROVVEDIMENTO("IMPORTO_ACCERTATO_ACC")), 2)
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO")).Length = 0 Then
                        txtImportoArrotond.Text = "0"
                    Else
                        txtImportoArrotond.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE")).Length = 0 Then
                        txtImportoTotale.Text = "0"
                    Else
                        txtImportoTotale.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE_RIDOTTO")).Length = 0 Then
                        txtImportoTotRid.Text = "0"
                    Else
                        txtImportoTotRid.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE_RIDOTTO")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO_RIDOTTO")).Length = 0 Then
                        txtImportoArrotondRid.Text = "0"
                    Else
                        txtImportoArrotondRid.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO_RIDOTTO")), 2)
                    End If
                    If StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_RUOLO_COATTIVO")).Length = 0 Then
                        txtImportoRuoloCoattivoICI.Text = "0,00"
                    Else
                        txtImportoRuoloCoattivoICI.Text = FormatNumber(StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_RUOLO_COATTIVO")), 2)
                    End If
                    '*** date ***
                    txtDataConfermaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_CONFERMA")))
                    txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_STAMPA")))
                    txtDataConsegnaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_CONSEGNA_AVVISO")))
                    txtDataNotificaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_NOTIFICA_AVVISO")))
                    txtDataRettificaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_RETTIFICA_AVVISO")))
                    txtDataAnnullamentoAvviso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ANNULLAMENTO_AVVISO")))
                    txtDataSospensioneAvvisoAutotutela.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_AVVISO_AUTOTUTELA")))
                    txtDataIrreperibile.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_IRREPERIBILE")))

                    txtDataRicorsoProvinciale.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO")))
                    txtSospensioneProvinciale.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA")))
                    txtSentenzaProvinciale.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SENTENZA")))
                    txtNoteProvinciale.Text = StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_PROVINCIALE"))

                    txtNoteConcGiudiz.Text = StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_CONCILIAZIONE_G"))
                    If IsNothing(rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G")) Or rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G") Is DBNull.Value Then
                        ckConcGiudiz.Checked = False
                    Else
                        If rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G") = 0 Then
                            ckConcGiudiz.Checked = False
                        Else
                            ckConcGiudiz.Checked = True
                        End If
                    End If

                    txtDataRicorsoRegionale.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO_REGIONALE")))
                    txtSospensioneRegionale.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE")))
                    txtSentenzaRegionale.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SENTENZA_REGIONALE")))
                    txtNoteRegionale.Text = StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_REGIONALE"))

                    If rowPROVVEDIMENTO("ESITO_ACCERTAMENTO") Is DBNull.Value Or IsNothing(rowPROVVEDIMENTO("ESITO_ACCERTAMENTO")) Then
                        ddlEsitoAccertamenti.SelectedValue = -1
                    Else
                        ddlEsitoAccertamenti.SelectedValue = StringOperation.FormatInt(rowPROVVEDIMENTO("ESITO_ACCERTAMENTO"))
                    End If
                    txtTermineRicorso.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("TERMINE_RICORSO_ACC")))
                    txtNoteAccertamenti.Text = StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_ACCERTAMENTO"))
                    If IsNothing(rowPROVVEDIMENTO("FLAG_ACCERTAMENTO")) Or rowPROVVEDIMENTO("FLAG_ACCERTAMENTO") Is DBNull.Value Then
                        ckAccertamento.Checked = False
                    Else
                        If rowPROVVEDIMENTO("FLAG_ACCERTAMENTO") = 0 Then
                            ckAccertamento.Checked = False
                        Else
                            ckAccertamento.Checked = True
                        End If
                    End If

                    txtSospensioneCassazione.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE")))
                    txtNoteCassazione.Text = StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_CASSAZIONE"))

                    txtDataAttoDefinitivo.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ATTO_DEFINITIVO")))
                    txtDataRuoloOrdinario.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_RUOLO_ORDINARIO_TARSU")))
                    txtDataRuoloCoattivoICI.Text = objUtility.GiraDataFromDB(StringOperation.FormatString(rowPROVVEDIMENTO("DATA_COATTIVO")))

                    txtNoteGenerali.Text = StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_GENERALI_ATTO"))
                Next
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "ForzaDati", Utility.Costanti.AZIONE_LETTURA.ToString(), Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, Request("IDPROVVEDIMENTO"))
            End If

            txtTIPO_PROCEDIMENTO.Text = StringOperation.FormatString(Request("TIPOPROCEDIMENTO")).ToUpper
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim objHashTable As Hashtable = New Hashtable
    '    'Dim objSessione As CreateSessione
    '    Dim intCount As Integer
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim strTIPO_PROCEDIMENTO, sSQL As String
    '    Dim strIndirizzo As String
    '    Dim strCodFiscalePI As String
    '    Dim sScript As String = ""
    '    Dim strConnectionStringOPENgovICI As String = ""
    '    Dim objDS As DataSet

    '    objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
    '    objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")

    '    objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConfigurationManager.AppSettings("connectionStringOpenGovICI"))

    '    Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVP"

    '    'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '    'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '    '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    'End If

    '    Try
    '        If Not Page.IsPostBack Then

    '            If Not Request("PROVENIENZA") Is Nothing Then
    '                Session("PROVENIENZA") = Request("PROVENIENZA")
    '            Else
    '                Session("PROVENIENZA") = Nothing
    '            End If
    '            If Not Request("CODCONTRIBUENTE") Is Nothing Then
    '                Session("COD_CONTRIBUENTE") = Request("CODCONTRIBUENTE")
    '            Else
    '                Session("COD_CONTRIBUENTE") = Nothing
    '            End If
    '            If Not Request("ANNO") Is Nothing Then
    '                Session("ANNO") = Request("ANNO")
    '            Else
    '                Session("PROVENIENZA") = Nothing
    '            End If

    '            ViewState.Add("TIPO_PROVVEDIMENTO", StringOperation.Formatstring(Request("TIPO_PROVVEDIMENTO")))

    '            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)


    '            Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

    '            objDS = objCOMRicerca.getDATI_PROVVEDIMENTI(objHashTable)

    '            Session.Remove("PROVVEDIMENTI_DA_STAMPARE")
    '            Session.Add("PROVVEDIMENTI_DA_STAMPARE", objDS)

    '            txtNumeroProvvedimento.Text = CType(Request("NUMEROATTO"), String)

    '            'carico combo 
    '            Dim oLoadCombo As New MyUtility
    '            Dim drMyDati As SqlClient.SqlDataReader
    '            Dim cmdMyCommand As New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                cmdMyCommand.Connection.Open()
    '            End If
    '            cmdMyCommand.CommandTimeout = 0
    '            '*** 20120704 - IMU ***
    '            sSQL = "SELECT COD_TIPO_PROVVEDIMENTO AS CODICE, DESCRIZIONE + ' ' + CASE WHEN COD_TRIBUTO = '8852' THEN 'ICI/IMU' ELSE 'TARSU' END AS DESCRIZIONE"
    '            sSQL += " FROM TAB_TIPO_PROVVEDIMENTO"
    '            sSQL += " WHERE (COD_TRIBUTO='" & StringOperation.Formatstring(objDS.Tables("PROVVEDIMENTO").Rows(0)("cod_tributo")) & "')"
    '            sSQL += " ORDER BY DESCRIZIONE"
    '            cmdMyCommand.CommandType = CommandType.Text
    '            cmdMyCommand.CommandText = sSQL
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            drMyDati = cmdMyCommand.ExecuteReader
    '            oLoadCombo.FillDropDownSQL(DdlTipoProvvedimento, drMyDati, Request("IDTIPOPROVVEDIMENTO"), "...")

    '            txtAnno.Text = CType(Request("ANNO"), String)
    '            strTIPO_PROCEDIMENTO = StringOperation.Formatstring(Request("TIPOPROCEDIMENTO"))

    '            For intCount = 0 To objDS.Tables("PROVVEDIMENTO").Rows.Count - 1
    '                Dim rowPROVVEDIMENTO As DataRow = objDS.Tables("PROVVEDIMENTO").Rows(intCount)

    '                objHashTable.Add("NUMERO_ATTO", StringOperation.Formatstring(rowPROVVEDIMENTO("NUMERO_ATTO")))
    '                objHashTable.Add("ANNO_AVVISO", StringOperation.Formatstring(Request("ANNO")))
    '                objHashTable.Add("COD_CONTRIBUENTE", StringOperation.Formatstring(rowPROVVEDIMENTO("COD_CONTRIBUENTE")))

    '                txtCOD_CONTRIBUENTE.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("COD_CONTRIBUENTE"))
    '                txtID_PROVVEDIMENTO.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("ID_PROVVEDIMENTO"))
    '                txtTIPO_OPERAZIONE.Text = "RETTIFICA"
    '                txtDataGenerazione.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_ELABORAZIONE")))
    '                '*** anagrafica ***
    '                lblCognomeNome.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("COGNOME")) & " " & StringOperation.Formatstring(rowPROVVEDIMENTO("NOME"))
    '                If Len(StringOperation.Formatstring(rowPROVVEDIMENTO("PARTITA_IVA"))) = 0 Then
    '                    strCodFiscalePI = StringOperation.Formatstring(rowPROVVEDIMENTO("CODICE_FISCALE"))
    '                Else
    '                    strCodFiscalePI = StringOperation.Formatstring(rowPROVVEDIMENTO("PARTITA_IVA"))
    '                End If
    '                lblCfPiva.Text = strCodFiscalePI
    '                If Len(StringOperation.Formatstring(rowPROVVEDIMENTO("PROVINCIA_RES"))) = 0 Then
    '                    strIndirizzo = StringOperation.Formatstring(rowPROVVEDIMENTO("VIA_RES")) & " " & StringOperation.Formatstring(rowPROVVEDIMENTO("CIVICO_RES")) & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & StringOperation.Formatstring(rowPROVVEDIMENTO("CITTA_RES")) & " " & StringOperation.Formatstring(rowPROVVEDIMENTO("CAP_RES"))
    '                Else
    '                    strIndirizzo = StringOperation.Formatstring(rowPROVVEDIMENTO("VIA_RES")) & " " & StringOperation.Formatstring(rowPROVVEDIMENTO("CIVICO_RES")) & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & StringOperation.Formatstring(rowPROVVEDIMENTO("CITTA_RES")) & " " & StringOperation.Formatstring(rowPROVVEDIMENTO("CAP_RES")) & " " & "(" & StringOperation.Formatstring(rowPROVVEDIMENTO("PROVINCIA_RES")) & ")"
    '                End If
    '                lblResidenza.Text = strIndirizzo
    '                '*** importi ***
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")).Length = 0 Then
    '                    txtImportoDifferenzaImposta.Text = "0"
    '                Else
    '                    txtImportoDifferenzaImposta.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_SANZIONI")).Length = 0 Then
    '                    txtImportoSanzioni.Text = "0"
    '                Else
    '                    txtImportoSanzioni.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_SANZIONI")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTO")).Length = 0 Then
    '                    txtImportoSanzRid.Text = "0"
    '                Else
    '                    txtImportoSanzRid.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTO")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")).Length = 0 Then
    '                    txtImportoSanzNonRid.Text = "0"
    '                Else
    '                    txtImportoSanzNonRid.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_INTERESSI")).Length = 0 Then
    '                    txtImportoInteressi.Text = "0"
    '                Else
    '                    txtImportoInteressi.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_INTERESSI")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_ALTRO")).Length = 0 Then
    '                    txtAltroImporto.Text = "0"
    '                Else
    '                    txtAltroImporto.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_ALTRO")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_SPESE")).Length = 0 Then
    '                    txtImportoSpese.Text = "0"
    '                Else
    '                    txtImportoSpese.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_SPESE")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO")).Length = 0 Then
    '                    txtImportoArrotond.Text = "0"
    '                Else
    '                    txtImportoArrotond.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_TOTALE")).Length = 0 Then
    '                    txtImportoTotale.Text = "0"
    '                Else
    '                    txtImportoTotale.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_TOTALE")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_TOTALE_RIDOTTO")).Length = 0 Then
    '                    txtImportoTotRid.Text = "0"
    '                Else
    '                    txtImportoTotRid.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_TOTALE_RIDOTTO")), 2)
    '                End If
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO_RIDOTTO")).Length = 0 Then
    '                    txtImportoArrotondRid.Text = "0"
    '                Else
    '                    txtImportoArrotondRid.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_ARROTONDAMENTO_RIDOTTO")), 2)
    '                End If
    '                'txtImportoTotRid.Text = CDbl(txtImportoDifferenzaImposta.Text) + CDbl(txtImportoInteressi.Text) + CDbl(txtImportoSanzRid.Text) + CDbl(txtImportoSpese.Text) + CDbl(txtAltroImporto.Text)
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_PAGATO")).Length = 0 Then
    '                    txtImportoPagato.Text = "0"
    '                Else
    '                    txtImportoPagato.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_PAGATO")), 2)
    '                End If
    '                '*** date ***
    '                txtPervenutoQuestionari.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_PERVENUTO_IL")))
    '                txtDataConfermaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_CONFERMA")))
    '                txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_STAMPA")))
    '                txtDataConsegnaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_CONSEGNA_AVVISO")))
    '                txtDataNotificaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_NOTIFICA_AVVISO")))
    '                txtDataRettificaAvviso.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_RETTIFICA_AVVISO")))
    '                txtDataAnnullamentoAvviso.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_ANNULLAMENTO_AVVISO")))
    '                txtDataSospensioneAvvisoAutotutela.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SOSPENSIONE_AVVISO_AUTOTUTELA")))

    '                txtDataRicorsoProvinciale.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO")))
    '                txtSospensioneProvinciale.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA")))
    '                txtSentenzaProvinciale.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SENTENZA")))
    '                txtNoteProvinciale.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("NOTE_PROVINCIALE"))

    '                txtNoteConcGiudiz.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("NOTE_CONCILIAZIONE_G"))
    '                If IsNothing(rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G")) Or rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G") Is DBNull.Value Then
    '                    ckConcGiudiz.Checked = False
    '                Else
    '                    If rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G") = 0 Then
    '                        ckConcGiudiz.Checked = False
    '                    Else
    '                        ckConcGiudiz.Checked = True
    '                    End If
    '                End If

    '                txtDataRicorsoRegionale.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO_REGIONALE")))
    '                txtSospensioneRegionale.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE")))
    '                txtSentenzaRegionale.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SENTENZA_REGIONALE")))
    '                txtNoteRegionale.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("NOTE_REGIONALE"))

    '                txtDataRicorsoCassazione.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO_CASSAZIONE")))
    '                txtSospensioneCassazione.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE")))
    '                txtSentenzaCassazione.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SENTENZA_CASSAZIONE")))
    '                txtNoteCassazione.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("NOTE_CASSAZIONE"))

    '                If rowPROVVEDIMENTO("ESITO_ACCERTAMENTO") Is DBNull.Value Or IsNothing(rowPROVVEDIMENTO("ESITO_ACCERTAMENTO")) Then
    '                    ddlEsitoAccertamenti.SelectedValue = -1
    '                Else
    '                    ddlEsitoAccertamenti.SelectedValue = cToInt(rowPROVVEDIMENTO("ESITO_ACCERTAMENTO"))
    '                End If
    '                txtTermineRicorso.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("TERMINE_RICORSO_ACC"))
    '                txtNoteAccertamenti.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("NOTE_ACCERTAMENTO"))
    '                If IsNothing(rowPROVVEDIMENTO("FLAG_ACCERTAMENTO")) Or rowPROVVEDIMENTO("FLAG_ACCERTAMENTO") Is DBNull.Value Then
    '                    ckAccertamento.Checked = False
    '                Else
    '                    If rowPROVVEDIMENTO("FLAG_ACCERTAMENTO") = 0 Then
    '                        ckAccertamento.Checked = False
    '                    Else
    '                        ckAccertamento.Checked = True
    '                    End If
    '                End If

    '                txtDataAttoDefinitivo.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_ATTO_DEFINITIVO")))
    '                txtDataVersamentoUnicaSoluzione.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_VERSAMENTO_SOLUZIONE_UNICA")))
    '                txtDataConcessioneRateizzazione.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_CONCESSIONE_RATEIZZAZIONE")))
    '                txtDataSollecitoBonario.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_SOLLECITO_BONARIO")))
    '                txtDataRuoloOrdinario.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_RUOLO_ORDINARIO_TARSU")))
    '                txtDataRuoloCoattivoICI.Text = objUtility.GiraDataFromDB(StringOperation.Formatstring(rowPROVVEDIMENTO("DATA_COATTIVO")))
    '                If StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_RUOLO_COATTIVO")).Length = 0 Then
    '                    txtImportoRuoloCoattivoICI.Text = "0,00"
    '                Else
    '                    txtImportoRuoloCoattivoICI.Text = FormatNumber(StringOperation.Formatstring(rowPROVVEDIMENTO("IMPORTO_RUOLO_COATTIVO")), 2)
    '                End If

    '                txtNoteGenerali.Text = StringOperation.Formatstring(rowPROVVEDIMENTO("NOTE_GENERALI_ATTO"))
    '            Next
    '            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "ForzaDati", Utility.Costanti.AZIONE_LETTURA.ToString() + "|" + Request("IDPROVVEDIMENTO").ToString, ConstSession.CodTributo, ConstSession.IdEnte)
    '        End If

    '        txtTIPO_PROCEDIMENTO.Text = StringOperation.Formatstring(Request("TIPOPROCEDIMENTO")).ToUpper
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'If Not IsNothing(objSessione) Then
    '        '    objSessione.Kill()
    '        '    objSessione = Nothing
    '        'End If
    '    End Try
    'End Sub
    '''' <summary>
    '''' 
    '''' </summary>
    '''' <param name="objInput"></param>
    '''' <returns></returns>
    'Public Function cToInt(ByVal objInput As Object) As Integer
    '    cToInt = 0
    '    Try
    '        If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
    '            If IsNumeric(objInput) Then
    '                cToInt = Convert.ToInt32(objInput)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.cToInt.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Function
    ''' <summary>
    ''' Pulsante di salvataggio dati
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
        Dim sSQL, sScript As String
        Dim myDataView As New DataView
        Dim myID As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_SetForzaDati", "IDPROVVEDIMENTO", "CODTIPO", "IMPORTO_DIFFERENZA_IMPOSTA", "IMPORTO_SANZIONI", "IMPORTO_SANZIONI_RIDOTTO", "IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI", "IMPORTO_INTERESSI", "IMPORTO_ALTRO", "IMPORTO_SPESE", "IMPORTO_DICHIARATO_F2", "IMPORTO_VERSATO_F2", "IMPORTO_ACCERTATO_ACC", "IMPORTO_ARROTONDAMENTO_RIDOTTO", "IMPORTO_TOTALE_RIDOTTO", "IMPORTO_ARROTONDAMENTO", "IMPORTO_TOTALE", "IMPORTO_RUOLO_COATTIVO", "DATA_ELABORAZIONE", "DATA_CONFERMA", "DATA_STAMPA", "DATA_CONSEGNA_AVVISO", "DATA_NOTIFICA_AVVISO", "DATA_RETTIFICA_AVVISO", "DATA_ANNULLAMENTO_AVVISO", "DATA_SOSPENSIONE_AVVISO_AUTOTUTELA", "DATA_IRREPERIBILE", "DATA_PRESENTAZIONE_RICORSO", "DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA", "DATA_SENTENZA", "NOTE_PROVINCIALE", "FLAG_CONCILIAZIONE_G", "NOTE_CONCILIAZIONE_G", "DATA_PRESENTAZIONE_RICORSO_REGIONALE", "DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE", "DATA_SENTENZA_REGIONALE", "NOTE_REGIONALE", "FLAG_ACCERTAMENTO", "ESITO_ACCERTAMENTO", "TERMINE_RICORSO_ACC", "NOTE_ACCERTAMENTO", "DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE", "NOTE_CASSAZIONE", "DATA_ATTO_DEFINITIVO", "DATA_RUOLO_ORDINARIO_TARSU", "DATA_COATTIVO", "NOTE_GENERALI_ATTO", "OPERATORE")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPROVVEDIMENTO", txtID_PROVVEDIMENTO.Text) _
                        , ctx.GetParam("CODTIPO", DdlTipoProvvedimento.SelectedValue) _
                        , ctx.GetParam("IMPORTO_DIFFERENZA_IMPOSTA", StringOperation.FormatDouble(txtImportoDifferenzaImposta.Text)) _
                        , ctx.GetParam("IMPORTO_SANZIONI", StringOperation.FormatDouble(txtImportoSanzioni.Text)) _
                        , ctx.GetParam("IMPORTO_SANZIONI_RIDOTTO", StringOperation.FormatDouble(txtImportoSanzRid.Text)) _
                        , ctx.GetParam("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI", StringOperation.FormatDouble(txtImportoSanzNonRid.Text)) _
                        , ctx.GetParam("IMPORTO_INTERESSI", StringOperation.FormatDouble(txtImportoInteressi.Text)) _
                        , ctx.GetParam("IMPORTO_ALTRO", StringOperation.FormatDouble(txtAltroImporto.Text)) _
                        , ctx.GetParam("IMPORTO_SPESE", StringOperation.FormatDouble(txtImportoSpese.Text)) _
                        , ctx.GetParam("IMPORTO_DICHIARATO_F2", StringOperation.FormatDouble(txtDichiarato.Text)) _
                        , ctx.GetParam("IMPORTO_VERSATO_F2", StringOperation.FormatDouble(txtVersato.Text)) _
                        , ctx.GetParam("IMPORTO_ACCERTATO_ACC", StringOperation.FormatDouble(txtAccertato.Text)) _
                        , ctx.GetParam("IMPORTO_ARROTONDAMENTO_RIDOTTO", StringOperation.FormatDouble(txtImportoArrotondRid.Text)) _
                        , ctx.GetParam("IMPORTO_TOTALE_RIDOTTO", StringOperation.FormatDouble(txtImportoTotRid.Text)) _
                        , ctx.GetParam("IMPORTO_ARROTONDAMENTO", StringOperation.FormatDouble(txtImportoArrotond.Text)) _
                        , ctx.GetParam("IMPORTO_TOTALE", StringOperation.FormatDouble(txtImportoTotale.Text)) _
                        , ctx.GetParam("IMPORTO_RUOLO_COATTIVO", StringOperation.FormatDouble(txtImportoRuoloCoattivoICI.Text)) _
                        , ctx.GetParam("DATA_ELABORAZIONE", objUtility.GiraData(StringOperation.FormatDateTime(txtDataGenerazione.Text))) _
                        , ctx.GetParam("DATA_CONFERMA", objUtility.GiraData(StringOperation.FormatDateTime(txtDataConfermaAvviso.Text))) _
                        , ctx.GetParam("DATA_STAMPA", objUtility.GiraData(StringOperation.FormatDateTime(txtDataStampaAvviso.Text))) _
                        , ctx.GetParam("DATA_CONSEGNA_AVVISO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataConsegnaAvviso.Text))) _
                        , ctx.GetParam("DATA_NOTIFICA_AVVISO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataNotificaAvviso.Text))) _
                        , ctx.GetParam("DATA_RETTIFICA_AVVISO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataRettificaAvviso.Text))) _
                        , ctx.GetParam("DATA_ANNULLAMENTO_AVVISO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataAnnullamentoAvviso.Text))) _
                        , ctx.GetParam("DATA_SOSPENSIONE_AVVISO_AUTOTUTELA", objUtility.GiraData(StringOperation.FormatDateTime(txtDataSospensioneAvvisoAutotutela.Text))) _
                        , ctx.GetParam("DATA_IRREPERIBILE", objUtility.GiraData(StringOperation.FormatDateTime(txtDataIrreperibile.Text))) _
                        , ctx.GetParam("DATA_PRESENTAZIONE_RICORSO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataRicorsoProvinciale.Text))) _
                        , ctx.GetParam("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA", objUtility.GiraData(StringOperation.FormatDateTime(txtSospensioneProvinciale.Text))) _
                        , ctx.GetParam("DATA_SENTENZA", objUtility.GiraData(StringOperation.FormatDateTime(txtSentenzaProvinciale.Text))) _
                        , ctx.GetParam("NOTE_PROVINCIALE", txtNoteProvinciale.Text) _
                        , ctx.GetParam("FLAG_CONCILIAZIONE_G", objUtility.CBoolToDB(ckConcGiudiz.Checked)) _
                        , ctx.GetParam("NOTE_CONCILIAZIONE_G", txtNoteConcGiudiz.Text) _
                        , ctx.GetParam("DATA_PRESENTAZIONE_RICORSO_REGIONALE", objUtility.GiraData(StringOperation.FormatDateTime(txtDataRicorsoRegionale.Text))) _
                        , ctx.GetParam("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE", objUtility.GiraData(StringOperation.FormatDateTime(txtSospensioneRegionale.Text))) _
                        , ctx.GetParam("DATA_SENTENZA_REGIONALE", objUtility.GiraData(StringOperation.FormatDateTime(txtSentenzaRegionale.Text))) _
                        , ctx.GetParam("NOTE_REGIONALE", txtNoteRegionale.Text) _
                        , ctx.GetParam("FLAG_ACCERTAMENTO", objUtility.CBoolToDB(ckAccertamento.Checked)) _
                        , ctx.GetParam("ESITO_ACCERTAMENTO", ddlEsitoAccertamenti.SelectedValue) _
                        , ctx.GetParam("TERMINE_RICORSO_ACC", objUtility.GiraData(StringOperation.FormatDateTime(txtTermineRicorso.Text))) _
                        , ctx.GetParam("NOTE_ACCERTAMENTO", txtNoteAccertamenti.Text) _
                        , ctx.GetParam("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE", objUtility.GiraData(StringOperation.FormatDateTime(txtSospensioneCassazione.Text))) _
                        , ctx.GetParam("NOTE_CASSAZIONE", txtNoteCassazione.Text) _
                        , ctx.GetParam("DATA_ATTO_DEFINITIVO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataAttoDefinitivo.Text))) _
                        , ctx.GetParam("DATA_RUOLO_ORDINARIO_TARSU", objUtility.GiraData(StringOperation.FormatDateTime(txtDataRuoloOrdinario.Text))) _
                        , ctx.GetParam("DATA_COATTIVO", objUtility.GiraData(StringOperation.FormatDateTime(txtDataRuoloCoattivoICI.Text))) _
                        , ctx.GetParam("NOTE_GENERALI_ATTO", txtNoteGenerali.Text) _
                        , ctx.GetParam("OPERATORE", ConstSession.UserName)
                    )
                For Each myRow As DataRowView In myDataView
                    myid = StringOperation.FormatInt(myRow("id"))
                Next
                ctx.Dispose()
            End Using
            If myID <= 0 Then
                sScript = "GestAlert('a', 'warning', '', '', 'Inserimento non effettuato!');"
            Else
                Dim strPARAMETRI As String = "?IDPROVVEDIMENTO=" & myID
                strPARAMETRI += "&CODCONTRIBUENTE=" & Replace(hdIdContribuente.Value, "'", "&quot;")
                strPARAMETRI += "&TIPOPROVVEDIMENTO=" & DdlTipoProvvedimento.SelectedItem.Text
                strPARAMETRI += "&IDTIPOPROVVEDIMENTO=" & DdlTipoProvvedimento.SelectedValue
                strPARAMETRI += "&ANNO=" & txtAnno.Text
                strPARAMETRI += "&NUMEROATTO=" & txtNumeroProvvedimento.Text
                strPARAMETRI += "&PROVENIENZA=" & StringOperation.FormatString(Session("PROVENIENZA"))
                strPARAMETRI += "&DESCTRIBUTO=" & StringOperation.FormatString(Request("DESCTRIBUTO"))
                strPARAMETRI += "&TIPOPROCEDIMENTO=" & StringOperation.FormatString(Session("TIPOPROCEDIMENTO"))
                strPARAMETRI += "&PAGINAPRECEDENTE=" & StringOperation.FormatString(Request("PAGINAPRECEDENTE"))
                Session("ParamGestioneAtti") = strPARAMETRI
                sScript = "GestAlert('a', 'success', '', '', 'Forzatura registrata con successo!');location.href='../GestioneAtti.aspx" & strPARAMETRI.Replace("'", "\'") & "';"
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "CmdSalva", Utility.Costanti.AZIONE_UPDATE, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, txtID_PROVVEDIMENTO.Text)
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.CmdSalva_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            myDataView.Dispose()
        End Try
    End Sub
    'Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim sSQL, sScript As String
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Try
    '        If DdlTipoProvvedimento.SelectedValue <> CStr(Request("TIPOPROVVEDIMENTO")) Then
    '            sSQL = "UPDATE TAB_PROCEDIMENTI SET "
    '            sSQL += "COD_TIPO_PROVVEDIMENTO=" & DdlTipoProvvedimento.SelectedValue
    '            sSQL += " WHERE (ID_PROVVEDIMENTO=" & txtID_PROVVEDIMENTO.Text & ")"
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                cmdMyCommand.Connection.Open()
    '            End If
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.Text
    '            cmdMyCommand.CommandText = sSQL
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            cmdMyCommand.ExecuteNonQuery()
    '        End If

    '        sSQL = "UPDATE PROVVEDIMENTI SET "
    '        sSQL += "IMPORTO_DIFFERENZA_IMPOSTA=" & objUtility.CDoubleToDB(CDbl(txtImportoDifferenzaImposta.Text)) & ","
    '        sSQL += "IMPORTO_INTERESSI=" & objUtility.CDoubleToDB(CDbl(txtImportoInteressi.Text)) & ","
    '        sSQL += "IMPORTO_PAGATO=" & objUtility.CDoubleToDB(CDbl(txtImportoPagato.Text)) & ","
    '        sSQL += "IMPORTO_SANZIONI=" & objUtility.CDoubleToDB(CDbl(txtImportoSanzioni.Text)) & ","
    '        sSQL += "IMPORTO_SANZIONI_RIDOTTO=" & objUtility.CDoubleToDB(CDbl(txtImportoSanzRid.Text)) & ","
    '        sSQL += "IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI=" & objUtility.CDoubleToDB(CDbl(txtImportoSanzNonRid.Text)) & ","
    '        sSQL += "IMPORTO_SPESE=" & objUtility.CDoubleToDB(CDbl(txtImportoSpese.Text)) & ","
    '        sSQL += "IMPORTO_ARROTONDAMENTO=" & objUtility.CDoubleToDB(CDbl(txtImportoArrotond.Text)) & ","
    '        sSQL += "IMPORTO_TOTALE=" & objUtility.CDoubleToDB(CDbl(txtImportoTotale.Text)) & ","
    '        sSQL += "IMPORTO_TOTALE_RIDOTTO=" & objUtility.CDoubleToDB(CDbl(txtImportoTotRid.Text)) & ","
    '        sSQL += "IMPORTO_ARROTONDAMENTO_RIDOTTO=" & objUtility.CDoubleToDB(CDbl(txtImportoArrotondRid.Text)) & ","
    '        sSQL += "IMPORTO_ALTRO=" & objUtility.CDoubleToDB(CDbl(txtAltroImporto.Text)) & ","
    '        sSQL += "IMPORTO_RUOLO_COATTIVO=" & objUtility.CDoubleToDB(CDbl(txtImportoRuoloCoattivoICI.Text)) & ","
    '        sSQL += "DATA_ELABORAZIONE=" & objUtility.CStrToDB(objUtility.GiraData(txtDataGenerazione.Text)) & ","
    '        sSQL += "DATA_PERVENUTO_IL=" & objUtility.CStrToDB(objUtility.GiraData(txtPervenutoQuestionari.Text)) & ","
    '        sSQL += "DATA_CONFERMA=" & objUtility.CStrToDB(objUtility.GiraData(txtDataConfermaAvviso.Text)) & ","
    '        sSQL += "DATA_STAMPA=" & objUtility.CStrToDB(objUtility.GiraData(txtDataStampaAvviso.Text)) & ","
    '        sSQL += "DATA_CONSEGNA_AVVISO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataConsegnaAvviso.Text)) & ","
    '        sSQL += "DATA_NOTIFICA_AVVISO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataNotificaAvviso.Text)) & ","
    '        sSQL += "DATA_RETTIFICA_AVVISO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataRettificaAvviso.Text)) & ","
    '        sSQL += "DATA_ANNULLAMENTO_AVVISO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataAnnullamentoAvviso.Text)) & ","
    '        sSQL += "DATA_SOSPENSIONE_AVVISO_AUTOTUTELA=" & objUtility.CStrToDB(objUtility.GiraData(txtDataSospensioneAvvisoAutotutela.Text)) & ","
    '        sSQL += "DATA_PRESENTAZIONE_RICORSO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataRicorsoProvinciale.Text)) & ","
    '        sSQL += "DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA=" & objUtility.CStrToDB(objUtility.GiraData(txtSospensioneProvinciale.Text)) & ","
    '        sSQL += "DATA_SENTENZA=" & objUtility.CStrToDB(objUtility.GiraData(txtSentenzaProvinciale.Text)) & ","
    '        sSQL += "NOTE_PROVINCIALE=" & objUtility.CStrToDB(txtNoteProvinciale.Text) & ","
    '        sSQL += "NOTE_CONCILIAZIONE_G=" & objUtility.CStrToDB(txtNoteConcGiudiz.Text) & ","
    '        sSQL += "FLAG_CONCILIAZIONE_G=" & objUtility.CBoolToDB(ckConcGiudiz.Checked) & ","
    '        sSQL += "DATA_PRESENTAZIONE_RICORSO_REGIONALE=" & objUtility.CStrToDB(objUtility.GiraData(txtDataRicorsoRegionale.Text)) & ","
    '        sSQL += "DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE=" & objUtility.CStrToDB(objUtility.GiraData(txtSospensioneRegionale.Text)) & ","
    '        sSQL += "DATA_SENTENZA_REGIONALE=" & objUtility.CStrToDB(objUtility.GiraData(txtSentenzaRegionale.Text)) & ","
    '        sSQL += "NOTE_REGIONALE=" & objUtility.CStrToDB(txtNoteRegionale.Text) & ","
    '        sSQL += "DATA_PRESENTAZIONE_RICORSO_CASSAZIONE=" & objUtility.CStrToDB(objUtility.GiraData(txtDataRicorsoCassazione.Text)) & ","
    '        sSQL += "DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE=" & objUtility.CStrToDB(objUtility.GiraData(txtSospensioneCassazione.Text)) & ","
    '        sSQL += "DATA_SENTENZA_CASSAZIONE=" & objUtility.CStrToDB(objUtility.GiraData(txtSentenzaCassazione.Text)) & ","
    '        sSQL += "NOTE_CASSAZIONE=" & objUtility.CStrToDB(txtNoteCassazione.Text) & ","
    '        sSQL += "ESITO_ACCERTAMENTO=" & objUtility.CStrToDB(ddlEsitoAccertamenti.SelectedValue) & ","
    '        sSQL += "TERMINE_RICORSO_ACC=" & objUtility.CStrToDB(objUtility.GiraData(txtTermineRicorso.Text)) & ","
    '        sSQL += "NOTE_ACCERTAMENTO=" & objUtility.CStrToDB(txtNoteAccertamenti.Text) & ","
    '        sSQL += "FLAG_ACCERTAMENTO=" & objUtility.CBoolToDB(ckAccertamento.Checked) & ","
    '        sSQL += "DATA_ATTO_DEFINITIVO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataAttoDefinitivo.Text)) & ","
    '        sSQL += "DATA_VERSAMENTO_SOLUZIONE_UNICA=" & objUtility.CStrToDB(objUtility.GiraData(txtDataVersamentoUnicaSoluzione.Text)) & ","
    '        sSQL += "DATA_CONCESSIONE_RATEIZZAZIONE=" & objUtility.CStrToDB(objUtility.GiraData(txtDataConcessioneRateizzazione.Text)) & ","
    '        sSQL += "DATA_SOLLECITO_BONARIO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataSollecitoBonario.Text)) & ","
    '        sSQL += "DATA_RUOLO_ORDINARIO_TARSU=" & objUtility.CStrToDB(objUtility.GiraData(txtDataRuoloOrdinario.Text)) & ","
    '        sSQL += "DATA_COATTIVO=" & objUtility.CStrToDB(objUtility.GiraData(txtDataRuoloCoattivoICI.Text)) & ","
    '        sSQL += "NOTE_GENERALI_ATTO=" & objUtility.CStrToDB(txtNoteGenerali.Text) & ""
    '        sSQL += " WHERE (ID_PROVVEDIMENTO=" & txtID_PROVVEDIMENTO.Text & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Inserimento non effettuato!');"
    '        Else
    '            sScript = "GestAlert('a', 'success', '', '', 'Forzatura registrata con successo!');location.href='../GestioneAtti.aspx" & Session("ParamGestioneAtti") & "';"
    '        End If
    '        RegisterScript(sScript, Me.GetType())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.CmdSalva_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante di cancellazione atto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDelete.Click
        Dim sScript As String

        Try
            If New ClsGestioneAccertamenti().DeleteAtto(txtID_PROVVEDIMENTO.Text) = False Then
                sScript = "GestAlert('a', 'warning', '', '', 'Cancellazione non effettuata!');"
            Else
                sScript = "GestAlert('a', 'success', '', '', 'Cancellazione registrata con successo!');location.href='../RicercaSemplice/RicercaSemplice.aspx';"
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "CmdDelete", Utility.Costanti.AZIONE_DELETE, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, txtID_PROVVEDIMENTO.Text)
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.CmdDelete_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDelete.Click
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim sSQL, sScript As String

    '    Try
    '        'non cancello veramente ma cambio l'ente in modo che posso ripristinarlo quando voglio
    '        sSQL = "UPDATE PROVVEDIMENTI SET "
    '        sSQL += "COD_ENTE='X" & CStr(ConstSession.IdEnte).Substring(1, CStr(ConstSession.IdEnte).Length - 1) & "'"
    '        sSQL += ",NOTE_GENERALI_ATTO='CANCELLATO DA " & ConstSession.UserName & " IL " & Now.ToShortDateString & " '+NOTE_GENERALI_ATTO"
    '        sSQL += " WHERE (ID_PROVVEDIMENTO=" & txtID_PROVVEDIMENTO.Text & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Cancellazione non effettuata!');"
    '        Else
    '            sScript = "GestAlert('a', 'success', '', '', 'Cancellazione registrata con successo!');location.href='../RicercaSemplice/RicercaSemplice.aspx';"
    '        End If
    '        RegisterScript(sScript, Me.GetType())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestForzaDati.CmdDelete_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Sub
End Class
