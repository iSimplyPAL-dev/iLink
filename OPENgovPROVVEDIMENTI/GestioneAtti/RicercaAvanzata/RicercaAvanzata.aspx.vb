Imports AnagInterface
Imports System
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Remoting.ObjRef
Imports System.Threading
Imports System.Collections
Imports System.Web.UI.Control
Imports ComPlusInterface
Imports log4net
Imports ICSharpCode.SharpZipLib.Zip
Imports Utility
''' <summary>
''' Pagina per la consultazione avanzata dei provvedimenti.
''' Contiene i parametri di ricerca, le funzioni della comandiera e i frame per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaAvanzata
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(RicercaAvanzata))
    'Private WFErrore As String
    Protected WithEvents opt1 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents opt2 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents opt3 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents opt4 As System.Web.UI.WebControls.RadioButton

    Dim oDettaglioAnagrafica As DettaglioAnagrafica

    Protected WithEvents chkDataPagamentoParziale As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtDataPagamentoParzialeDal As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataPagamentoParzialeAl As System.Web.UI.WebControls.TextBox

    Dim myUtil As New MyUtility
    Private Const SESSION_DATE As String = "SESSION_DATE"

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim sScript As String = ""

        Try
            Dim hdSearchParametres As New Hashtable
            hdSearchParametres = CType(Session(SESSION_DATE), Hashtable)
            If Page.IsPostBack = False Then
                Log.Debug("inizio")
                txtAMData.Text = DateTime.Now.ToShortDateString
                txtRicercaAttiva.Text = "-1"
                LblFile290.Attributes.Add("onclick", "CmdScarica.click()")
                Log.Debug("disabilito controlli")
                Dim ctlControl As Control
                For Each ctlControl In Form1.Controls
                    If TypeOf ctlControl Is TextBox Then
                        CType(ctlControl, TextBox).Enabled = False
                    End If
                Next
                txtCheckBox.Enabled = True
                txtDIVDate.Enabled = True
                txtCheckBox.Text = "-1"
                txtDIVDate.Text = "-1"
                Log.Debug("loadenti")
                '*** 201511 - Funzioni Sovracomunali ***
                LoadEnti()
                '*** ***
                myUtil.FillDropDownSQLString(ddlAnno, objGestOPENgovProvvedimenti.GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, ""), -1, "TUTTI")
                myUtil.FillDropDownSQL(ddlTributo, objGestOPENgovProvvedimenti.GetTRIBUTIProvvedimentiAnno(Utility.StringOperation.FormatString(ConstSession.IdEnte), "-1"), -1, "TUTTI")
                myUtil.FillDropDownSQL(ddlTipologiaTributo, objGestOPENgovProvvedimenti.GetTIPOTRIBUTIProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, "-1", "-1"), -1, "TUTTI")
                Log.Debug("combo caricate")
                sScript += "parent.Comandi.location.href='ComandiRicercaAvanzata.aspx';"
                RegisterScript(sScript, Me.GetType())

                If Not hdSearchParametres Is Nothing Then
                    Log.Debug("ricarico parametri")
                    ddlAnno.SelectedValue = hdSearchParametres("ANNO")
                    ddlTributo.SelectedValue = hdSearchParametres("TRIBUTO")
                    ddlTipologiaTributo.SelectedValue = hdSearchParametres("TIPOPROVVEDIMENTO")

                    hdSearchParametres = GetParameters()
                    If Not hdSearchParametres Is Nothing Then
                        sScript += "Search();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "RicercaAvanzata", Utility.Costanti.AZIONE_LETTURA.ToString, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, -1)
            Else
                sScript += "attesaGestioneAtti.style.display='';"
                EnableOption()
            End If
            txtAMData.Enabled = True
            If ConstSession.IdEnte <> "" Then
                ddlEnti.SelectedValue = ConstSession.IdEnte
                sScript += "document.getElementById('divEnti').style.display='none';"
                sScript += "document.getElementById('lblEnti').style.display='none';"
                sScript += "document.getElementById('ddlEnti').style.display='none';"
                RegisterScript(sScript, Me.GetType())
            End If
            Log.Debug("fine")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlAnno_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAnno.SelectedIndexChanged
        Try
            myUtil = New MyUtility
            Dim strAnno As String
            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

            strAnno = ddlAnno.SelectedValue
            ddlTributo.Items.Clear()
            myUtil.FillDropDownSQLValueString(ddlTributo, objGestOPENgovProvvedimenti.GetTRIBUTIProvvedimentiAnno(ConstSession.IdEnte, strAnno), -1, "TUTTI")

            ddlTipologiaTributo.Items.Clear()
            myUtil.FillDropDownSQL(ddlTipologiaTributo, objGestOPENgovProvvedimenti.GetTIPOTRIBUTIProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, "-1", strAnno), -1, "TUTTI")

            Dim sScript As String = ""
            sScript += "VerificaDIV();"
            sScript += "attesaGestioneAtti.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.ddlAnno_SelectedIndexChanged.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTributo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTributo.SelectedIndexChanged
        Try
            myUtil = New MyUtility
            Dim strTributo As String
            Dim strAnno As String
            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

            strTributo = ddlTributo.SelectedValue
            strAnno = ddlAnno.SelectedValue
            ddlTipologiaTributo.Items.Clear()
            myUtil.FillDropDownSQLValueString(ddlTipologiaTributo, objGestOPENgovProvvedimenti.GetTIPOTRIBUTIProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, strTributo, strAnno), -1, "TUTTI")
            Dim sScript As String = ""
            sScript += "VerificaDIV();"
            sScript += "attesaGestioneAtti.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.ddlTributo_SelectedIndexChanged.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadEnti()
        Dim sSQL As String
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                ConstSession.nTry = 0
ReDo:
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "ENTI_S", "IDENTE", "AMBIENTE", "BELFIORE")
                    myDataReader = ctx.GetDataReader(sSQL _
                            , ctx.GetParam("IDENTE", "") _
                            , ctx.GetParam("AMBIENTE", ConstSession.Ambiente) _
                            , ctx.GetParam("BELFIORE", "")
                        )
                    ddlEnti.Items.Clear()
                    ddlEnti.Items.Add("...")
                    ddlEnti.Items(0).Value = ""
                    If Not myDataReader Is Nothing Then
                        Do While myDataReader.Read
                            If Not IsDBNull(myDataReader(0)) Then
                                ddlEnti.Items.Add(myDataReader(1))
                                ddlEnti.Items(ddlEnti.Items.Count - 1).Value = myDataReader(0)
                            End If
                        Loop
                    End If
                Catch ex As Exception
                    If ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") And ConstSession.nTry <= 3 Then
                        ConstSession.nTry += 1
                        GoTo ReDo
                    End If
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.LoadEnti.errore: ", ex)
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.LoadEnti.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            myDataReader.Close()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub EnableOption()
        Dim ValRicDate As String = "disabled"
        Try
            AbilitaDisabilitaOptionElaborazione(myUtil.CToStr(Request("DataGenerazione")),
                                                       ValRicDate,
                                                       txtDataGenerazioneDal,
                                                       txtDataGenerazioneAL)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataConfermaAvviso")),
                                                   myUtil.CToStr(Request("DataConfermaAvviso")),
                                                   ValRicDate,
                                                   ValRicDate,
                                                   txtDataConfermaAvvisoDal,
                                                   txtDataConfermaAvvisoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataStampaAvviso")),
                                                           myUtil.CToStr(Request("DataStampaAvviso")),
                                                           ValRicDate,
                                                           ValRicDate,
                                                           txtDataStampaAvvisoDal,
                                                           txtDataStampaAvvisoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataConsegnaAvviso")),
                                                   myUtil.CToStr(Request("DataConsegnaAvviso")),
                                                   ValRicDate,
                                                   ValRicDate,
                                                   txtDataConsegnaAvvisoDal,
                                                   txtDataConsegnaAvvisoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataNotificaAvviso")),
                                                   myUtil.CToStr(Request("DataNotificaAvviso")),
                                                   ValRicDate,
                                                   ValRicDate,
                                                   txtDataNotificaAvvisoDal,
                                                   txtDataNotificaAvvisoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataRettificaAvviso")),
                                                   myUtil.CToStr(Request("DataRettificaAvviso")),
                                                   ValRicDate,
                                                   ValRicDate,
                                                   txtDataRettificaAvvisoDal,
                                                   txtDataRettificaAvvisoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataAnnulamentoAvviso")),
                                                   myUtil.CToStr(Request("DataAnnulamentoAvviso")),
                                                   ValRicDate,
                                                   ValRicDate,
                                                   txtDataAnnulamentoAvvisoDal,
                                                   txtDataAnnulamentoAvvisoAl)

            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSopensioneAutotutela")),
                                                myUtil.CToStr(Request("DataSopensioneAutotutela")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataSopensioneAutotutelaDal,
                                                txtDataSopensioneAutotutelaAl)

            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataRicorsoProvinciale")),
                                                        myUtil.CToStr(Request("DataRicorsoProvinciale")),
                                                        ValRicDate,
                                                        ValRicDate,
                                                        txtDataRicorsoProvincialeDal,
                                                        txtDataRicorsoProvincialeAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSopensioneProvinciale")),
                                                myUtil.CToStr(Request("DataSopensioneProvinciale")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataSopensioneProvincialeDal,
                                                txtDataSopensioneProvincialeAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSentenzaProvinciale")),
                                              myUtil.CToStr(Request("DataSentenzaProvinciale")),
                                              ValRicDate,
                                              ValRicDate,
                                              txtDataSentenzaProvincialeDal,
                                              txtDataSentenzaProvincialeAl)

            '**********************************************************************************************************

            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataRicorsoRegionale")),
                                                        myUtil.CToStr(Request("DataRicorsoRegionale")),
                                                        ValRicDate,
                                                        ValRicDate,
                                                        txtDataRicorsoRegionaleDal,
                                                        txtDataRicorsoRegionaleAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSopensioneRegionale")),
                                                myUtil.CToStr(Request("DataSopensioneRegionale")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataSopensioneRegionaleDal,
                                                txtDataSopensioneRegionaleAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSentenzaRegionale")),
                                              myUtil.CToStr(Request("DataSentenzaRegionale")),
                                              ValRicDate,
                                              ValRicDate,
                                              txtDataSentenzaRegionaleDal,
                                              txtDataSentenzaRegionaleAl)

            '**********************************************************************************************************
            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataRicorsoCassazione")),
                                                        myUtil.CToStr(Request("DataRicorsoCassazione")),
                                                        ValRicDate,
                                                        ValRicDate,
                                                        txtDataRicorsoCassazioneDal,
                                                        txtDataRicorsoCassazioneAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSopensioneCassazione")),
                                                myUtil.CToStr(Request("DataSopensioneCassazione")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataSopensioneCassazioneDal,
                                                txtDataSopensioneCassazioneAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSentenzaCassazione")),
                                              myUtil.CToStr(Request("DataSentenzaCassazione")),
                                              ValRicDate,
                                              ValRicDate,
                                              txtDataSentenzaCassazioneDal,
                                              txtDataSentenzaCassazioneAl)

            '**********************************************************************************************************

            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataAttoDefinitivo")),
                                                        myUtil.CToStr(Request("DataAttoDefinitivo")),
                                                        ValRicDate,
                                                        ValRicDate,
                                                        txtDataAttoDefinitivoDal,
                                                        txtDataAttoDefinitivoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataPagamento")),
                                                myUtil.CToStr(Request("DataPagamento")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataPagamentoDal,
                                                txtDataPagamentoAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataSollecitoBonario")),
                                              myUtil.CToStr(Request("DataSollecitoBonario")),
                                              ValRicDate,
                                              ValRicDate,
                                              txtDataSollecitoBonarioDal,
                                              txtDataSollecitoBonarioAl)

            '**********************************************************************************************************

            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataRuoloOrdinario")),
                                                        myUtil.CToStr(Request("DataRuoloOrdinario")),
                                                        ValRicDate,
                                                        ValRicDate,
                                                        txtDataRuoloOrdinarioDal,
                                                        txtDataRuoloOrdinarioAl)

            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataCoattivo")),
                                                myUtil.CToStr(Request("DataCoattivo")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataCoattivoDal,
                                                txtDataCoattivoAl)
            '**********************************************************************************************************
            AbilitaDisabilitaOption(myUtil.CToStr(Request("DataIrreperibile")),
                                                myUtil.CToStr(Request("DataIrreperibile")),
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataIrreperibileDal,
                                                txtDataIrreperibileAl)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.EnableOption.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strRequestDATA"></param>
    ''' <param name="strRequestNESSUNA"></param>
    ''' <param name="valoreOptDATA"></param>
    ''' <param name="valoreOptNESSUNA"></param>
    ''' <param name="txtDATADAL"></param>
    ''' <param name="txtDATAAL"></param>
    ''' <param name="blnELABORAZIONE"></param>
    Private Sub AbilitaDisabilitaOption(ByVal strRequestDATA As String, ByVal strRequestNESSUNA As String, ByRef valoreOptDATA As String, ByRef valoreOptNESSUNA As String, ByVal txtDATADAL As TextBox, ByVal txtDATAAL As TextBox, Optional ByVal blnELABORAZIONE As Boolean = False)

        valoreOptNESSUNA = "disabled"
        valoreOptDATA = "disabled"
        txtDATADAL.Enabled = False
        txtDATAAL.Enabled = False
        Try
            If blnELABORAZIONE Then
                If strRequestDATA.CompareTo("NESSUNA") = 0 Then
                    valoreOptDATA = "enabled"
                    Exit Sub
                End If
            End If
            If strRequestDATA.CompareTo("DATA") = 0 Then
                valoreOptDATA = "enabled checked"
                valoreOptNESSUNA = "enabled"
                txtDATADAL.Enabled = True
                txtDATAAL.Enabled = True
            End If

            If strRequestNESSUNA.CompareTo("NESSUNA") = 0 Then
                valoreOptNESSUNA = "enabled checked"
                valoreOptDATA = "enabled"
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.AbilitaDisabilitaOption.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strRequestDATA"></param>
    ''' <param name="valoreOptDATA"></param>
    ''' <param name="txtDATADAL"></param>
    ''' <param name="txtDATAAL"></param>
    Private Sub AbilitaDisabilitaOptionElaborazione(ByVal strRequestDATA As String, ByRef valoreOptDATA As String, ByVal txtDATADAL As TextBox, ByVal txtDATAAL As TextBox)

        valoreOptDATA = "disabled"
        txtDATADAL.Enabled = False
        txtDATAAL.Enabled = False
        Try
            If strRequestDATA.CompareTo("DATA") = 0 Then
                valoreOptDATA = "enabled checked"
                txtDATADAL.Enabled = True
                txtDATAAL.Enabled = True
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.AbilitaDisabilitaOptionElaborazione.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="blnHASHTableCheck"></param>
    ''' <param name="blnHASHTableTXT_DATA_DAL"></param>
    ''' <param name="blnHASHTableTXT_DATA_AL"></param>
    ''' <param name="blnHASHTableOPTION"></param>
    ''' <param name="chkSelecData"></param>
    ''' <param name="valoreOptDATA"></param>
    ''' <param name="valoreOptNESSUNA"></param>
    ''' <param name="txtDATA_DAL"></param>
    ''' <param name="txtDATA_AL"></param>
    ''' <param name="blnELABORAZIONE"></param>
    Private Sub gestSearchParametres(ByVal blnHASHTableCheck As Boolean, ByVal blnHASHTableTXT_DATA_DAL As String, ByVal blnHASHTableTXT_DATA_AL As String, ByVal blnHASHTableOPTION As String, ByVal chkSelecData As CheckBox, ByRef valoreOptDATA As String, ByRef valoreOptNESSUNA As String, ByVal txtDATA_DAL As TextBox, ByVal txtDATA_AL As TextBox, Optional ByVal blnELABORAZIONE As Boolean = False)

        Try
            If blnHASHTableCheck Then

                chkSelecData.Checked = True

                If blnHASHTableOPTION.CompareTo("DATA") = 0 Then
                    valoreOptDATA = "enabled checked"
                    valoreOptNESSUNA = "enabled"
                    txtDATA_DAL.Enabled = True
                    txtDATA_AL.Enabled = True
                    txtDATA_DAL.Text = blnHASHTableTXT_DATA_DAL
                    txtDATA_AL.Text = blnHASHTableTXT_DATA_AL
                Else

                    valoreOptNESSUNA = "enabled checked"
                    valoreOptDATA = "enabled"

                End If

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.gestSearchParametres.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSearch.Click
        Dim sScript As String = ""
        Try
            Dim mySearch As New ObjSearchAtti
            mySearch = ValSearchParam()
            If mySearch Is Nothing Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in ricerca');"
                RegisterScript(sScript, Me.GetType())
            Else
                Session("oSearchAttiAvanzata") = mySearch
                sScript = "document.getElementById('loadGrid').src = 'SearchAttiRicercaAvanzata.aspx'"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.CmdSearch_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnStampaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaExcel.Click
        Dim arraystr As String()
        Dim dtDatiMassivi As DataTable
        Dim x As Integer

        ReDim Preserve arraystr(30)
        x = 0
        arraystr(x) = "Ente"
        x += 1
        arraystr(x) = "Cognome"
        x += 1
        arraystr(x) = "Nome"
        x += 1
        arraystr(x) = "Cod.Fiscale/Partita IVA"
        x += 1
        arraystr(x) = "Indirizzo di residenza/invio"
        x += 1
        arraystr(x) = "N. Atto"
        x += 1
        arraystr(x) = "Anno"
        x += 1
        arraystr(x) = "Tipologia Atto"
        x += 1
        arraystr(x) = "Stato Avviso"
        x += 1
        arraystr(x) = "Data Elaborazione"
        x += 1
        arraystr(x) = "Data Stampa"
        x += 1
        arraystr(x) = "Data Consegna"
        x += 1
        arraystr(x) = "Data Notifica"
        x += 1
        arraystr(x) = "Data Annullamento"
        x += 1
        arraystr(x) = "Data Pagamento"
        x += 1
        arraystr(x) = "Note Generali"
        x += 1
        arraystr(x) = "MQ Dichiarati"
        x += 1
        arraystr(x) = "MQ Accertati"
        x += 1
        arraystr(x) = "Differenza Imposta €"
        x += 1
        arraystr(x) = "Sanzioni €"
        x += 1
        arraystr(x) = "Sanzioni Non Riducibili €"
        x += 1
        arraystr(x) = "Sanzioni Ridotte €"
        x += 1
        arraystr(x) = "Interessi €"
        x += 1
        arraystr(x) = "Addizionali €"
        x += 1
        arraystr(x) = "Spese €"
        x += 1
        arraystr(x) = "Arrotondamento €"
        x += 1
        arraystr(x) = "Totale €"
        x += 1
        arraystr(x) = "Arrotondamento Ridotto €"
        x += 1
        arraystr(x) = "Totale Ridotto €"
        x += 1
        arraystr(x) = "Pagato €"
        x += 1
        arraystr(x) = "Rateizzato"

        dtDatiMassivi = Session("TP_RICERCA_AVANZATA_PER_STAMPA")
        If Not IsNothing(dtDatiMassivi) Then
            Log.Debug("RicercaAvanzata::btnStampaExcel_Click::richiamo RKLIB")
            Dim sPathProspetti As String
            Dim NameXLS As String

            '**** SALVATAGGIO PERCORSO ****
            sPathProspetti = System.Configuration.ConfigurationManager.AppSettings("PATH_DATI_ATTUALI_EXCEL")
            NameXLS = ConstSession.IdEnte & "_PROVVEDIMENTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            '*** 20130116 - aggiungere il parametro di pagato su rateizzo ***
            Dim iColumns As Integer() = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30}
            '***  ***
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(dtDatiMassivi, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function GetParameters() As Hashtable
        Dim hdParametres As New Hashtable
        Dim ValRicDate As String = "disabled"
        Try
            gestSearchParametres(CType(hdParametres("CHEK_DATAELABORAZIONE"), Boolean),
                                           hdParametres("DATAELABORAZIONE_DAL"),
                                           hdParametres("DATAELABORAZIONE_AL"),
                                           hdParametres("OPT_DATAELABORAZIONE"),
                                           chkDataGenerazione,
                                           ValRicDate,
                                           "",
                                           txtDataGenerazioneDal,
                                           txtDataGenerazioneAL)

            gestSearchParametres(CType(hdParametres("CHEK_DATACONFERMAAVVISO"), Boolean),
                                 hdParametres("DATACONFERMAAVVISO_DAL"),
                                 hdParametres("DATACONFERMAAVVISO_AL"),
                                 hdParametres("OPT_DATACONFERMAAVVISO"),
                                 chkDataConfermaAvviso,
                                 ValRicDate,
                                 ValRicDate,
                                 txtDataConfermaAvvisoDal,
                                 txtDataConfermaAvvisoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATASTAMPAAVVISO"), Boolean),
                                          hdParametres("DATASTAMPAAVVISO_DAL"),
                                          hdParametres("DATASTAMPAAVVISO_AL"),
                                          hdParametres("OPT_DATASTAMPAAVVISO"),
                                          chkDataStampaAvviso,
                                          ValRicDate,
                                          ValRicDate,
                                          txtDataStampaAvvisoDal,
                                          txtDataStampaAvvisoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATACONSEGNAAVVISO"), Boolean),
                                        hdParametres("DATACONSEGNAAVVISO_DAL"),
                                        hdParametres("DATACONSEGNAAVVISO_AL"),
                                        hdParametres("OPT_DATACONSEGNAAVVISO"),
                                        chkDataConsegnaAvviso,
                                        ValRicDate,
                                        ValRicDate,
                                        txtDataConsegnaAvvisoDal,
                                        txtDataConsegnaAvvisoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATANOTIFICAAVVISO"), Boolean),
                                        hdParametres("DATANOTIFICAAVVISO_DAL"),
                                        hdParametres("DATANOTIFICAAVVISO_AL"),
                                        hdParametres("OPT_DATANOTIFICAAVVISO"),
                                        chkDataNotificaAvviso,
                                        ValRicDate,
                                        ValRicDate,
                                        txtDataNotificaAvvisoDal,
                                        txtDataNotificaAvvisoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATARETTIFICAAVVISO"), Boolean),
                                                 hdParametres("DATARETTIFICAAVVISO_DAL"),
                                                 hdParametres("DATARETTIFICAAVVISO_AL"),
                                                 hdParametres("OPT_DATARETTIFICAAVVISO"),
                                                 chkDataRettificaAvviso,
                                                 ValRicDate,
                                                 ValRicDate,
                                                 txtDataRettificaAvvisoDal,
                                                 txtDataRettificaAvvisoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATAANNULLAMENTOAVVISO"), Boolean),
                                                hdParametres("DATAANNULLAMENTOAVVISO_DAL"),
                                                hdParametres("DATAANNULLAMENTOAVVISO_AL"),
                                                hdParametres("OPT_DATAANNULLAMENTOAVVISO"),
                                                chkDataAnnulamentoAvviso,
                                                ValRicDate,
                                                ValRicDate,
                                                txtDataAnnulamentoAvvisoDal,
                                                txtDataAnnulamentoAvvisoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATAAUTOTUTELAAVVISO"), Boolean),
                                            hdParametres("DATAAUTOTUTELAAVVISO_DAL"),
                                            hdParametres("DATAANNULLAMENTOAVVISO_AL"),
                                            hdParametres("DATAAUTOTUTELAAVVISO_AL"),
                                            chkDataSopensioneAutotutela,
                                            ValRicDate,
                                            ValRicDate,
                                            txtDataSopensioneAutotutelaDal,
                                            txtDataSopensioneAutotutelaAl)
            '***********************************************DATE RICORSO PROVINCIALE***************************
            gestSearchParametres(CType(hdParametres("CHEK_RICORSOPROVINCIALE"), Boolean),
                                                      hdParametres("RICORSOPROVINCIALE_DAL"),
                                                      hdParametres("RICORSOPROVINCIALE_AL"),
                                                      hdParametres("OPT_RICORSOPROVINCIALE"),
                                                      chkDataRicorsoProvinciale,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataRicorsoProvincialeDal,
                                                      txtDataRicorsoProvincialeAl)

            gestSearchParametres(CType(hdParametres("CHEK_SOSPENSIONEPROVINCIALE"), Boolean),
                                                      hdParametres("SOSPENSIONEPROVINCIALE_DAL"),
                                                      hdParametres("SOSPENSIONEPROVINCIALE_AL"),
                                                      hdParametres("OPT_SOSPENSIONEPROVINCIALE"),
                                                      chkDataSopensioneProvinciale,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSopensioneProvincialeDal,
                                                      txtDataSopensioneProvincialeAl)

            gestSearchParametres(CType(hdParametres("CHEK_SENTENZAPROVINCIALE"), Boolean),
                                                      hdParametres("SENTENZAPROVINCIALE_DAL"),
                                                      hdParametres("SENTENZAPROVINCIALE_AL"),
                                                      hdParametres("OPT_SENTENZAPROVINCIALE"),
                                                      chkDataSentenzaProvinciale,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSentenzaProvincialeDal,
                                                      txtDataSentenzaProvincialeAl)
            '***********************************************FINE DATE RICORSO PROVINCIALE***************************
            '***********************************************DATE RICORSO REGIONALE***************************
            gestSearchParametres(CType(hdParametres("CHEK_RICORSOREGIONALE"), Boolean),
                                                      hdParametres("RICORSOREGIONALE_DAL"),
                                                      hdParametres("RICORSOREGIONALE_AL"),
                                                      hdParametres("OPT_RICORSOREGIONALE"),
                                                      chkDataRicorsoRegionale,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataRicorsoRegionaleDal,
                                                      txtDataRicorsoRegionaleAl)

            gestSearchParametres(CType(hdParametres("CHEK_SOSPENSIONEREGIONALE"), Boolean),
                                                      hdParametres("SOSPENSIONEREGIONALE_DAL"),
                                                      hdParametres("SOSPENSIONEREGIONALE_AL"),
                                                      hdParametres("OPT_SOSPENSIONEREGIONALE"),
                                                      chkDataSopensioneRegionale,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSopensioneRegionaleDal,
                                                      txtDataSopensioneRegionaleAl)

            gestSearchParametres(CType(hdParametres("CHEK_SENTENZAREGIONALE"), Boolean),
                                                      hdParametres("SENTENZAREGIONALE_DAL"),
                                                      hdParametres("SENTENZAREGIONALE_AL"),
                                                      hdParametres("OPT_SENTENZAREGIONALE"),
                                                      chkDataSentenzaRegionale,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSentenzaRegionaleDal,
                                                      txtDataSentenzaRegionaleAl)
            '***********************************************FINE DATE RICORSO PROVINCIALE***************************
            '***********************************************DATE RICORSO CASSAZIONE***************************
            gestSearchParametres(CType(hdParametres("CHEK_RICORSOCASSAZIONE"), Boolean),
                                                      hdParametres("RICORSOCASSAZIONE_DAL"),
                                                      hdParametres("RICORSOCASSAZIONE_AL"),
                                                      hdParametres("OPT_RICORSOCASSAZIONE"),
                                                      chkDataRicorsoCassazione,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataRicorsoCassazioneDal,
                                                      txtDataRicorsoCassazioneAl)

            gestSearchParametres(CType(hdParametres("CHEK_SOSPENSIONECASSAZIONE"), Boolean),
                                                      hdParametres("SOSPENSIONECASSAZIONE_DAL"),
                                                      hdParametres("SOSPENSIONECASSAZIONE_AL"),
                                                      hdParametres("OPT_SOSPENSIONECASSAZIONE"),
                                                      chkDataSopensioneCassazione,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSopensioneCassazioneDal,
                                                      txtDataSopensioneCassazioneAl)

            gestSearchParametres(CType(hdParametres("CHEK_SENTENZACASSAZIONE"), Boolean),
                                                      hdParametres("SENTENZACASSAZIONE_DAL"),
                                                      hdParametres("SENTENZACASSAZIONE_AL"),
                                                      hdParametres("OPT_SENTENZACASSAZIONE"),
                                                      chkDataSentenzaCassazione,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSentenzaCassazioneDal,
                                                      txtDataSentenzaCassazioneAl)
            '***********************************************FINE DATE RICORSO CASSAZIONE***************************

            gestSearchParametres(CType(hdParametres("CHEK_ATTODEFINITIVO"), Boolean),
                                                               hdParametres("ATTODEFINITIVO_DAL"),
                                                               hdParametres("ATTODEFINITIVO_AL"),
                                                               hdParametres("OPT_ATTODEFINITIVO"),
                                                               chkDataAttoDefinitivo,
                                                               ValRicDate,
                                                               ValRicDate,
                                                               txtDataAttoDefinitivoDal,
                                                               txtDataAttoDefinitivoAl)

            gestSearchParametres(CType(hdParametres("CHEK_PAGAMENTO"), Boolean),
                                                      hdParametres("PAGAMENTO_DAL"),
                                                      hdParametres("PAGAMENTO_AL"),
                                                      hdParametres("OPT_PAGAMENTO"),
                                                      chkDataPagamento,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataPagamentoDal,
                                                      txtDataPagamentoAl)

            gestSearchParametres(CType(hdParametres("CHEK_SOLLECITOBONARIO"), Boolean),
                                                      hdParametres("SOLLECITOBONARIO_DAL"),
                                                      hdParametres("SOLLECITOBONARIO_AL"),
                                                      hdParametres("OPT_SOLLECITOBONARIO"),
                                                      chkDataSollecitoBonario,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataSollecitoBonarioDal,
                                                      txtDataSollecitoBonarioAl)

            gestSearchParametres(CType(hdParametres("CHEK_RUOLOORDINARIO"), Boolean),
                                                      hdParametres("RUOLOORDINARIO_DAL"),
                                                      hdParametres("RUOLOORDINARIO_AL"),
                                                      hdParametres("OPT_RUOLOORDINARIO"),
                                                      chkDataRuoloOrdinario,
                                                      ValRicDate,
                                                      ValRicDate,
                                                      txtDataRuoloOrdinarioDal,
                                                      txtDataRuoloOrdinarioAl)

            gestSearchParametres(CType(hdParametres("CHEK_COATTIVO"), Boolean),
                                            hdParametres("COATTIVO_DAL"),
                                            hdParametres("COATTIVO_AL"),
                                            hdParametres("OPT_COATTIVO"),
                                            chkDataCoattivo,
                                            ValRicDate,
                                            ValRicDate,
                                            txtDataCoattivoDal,
                                            txtDataCoattivoAl)

            gestSearchParametres(CType(hdParametres("CHEK_DATAIRREPERIBILE"), Boolean),
                        hdParametres("DATAIRREPERIBILE_DAL"),
                        hdParametres("DATAIRREPERIBILE_AL"),
                        hdParametres("OPT_DATAIRREPERIBILE"),
                        chkDataIrreperibile,
                        ValRicDate,
                        ValRicDate,
                        txtDataIrreperibileDal,
                        txtDataIrreperibileAl)
            Return hdParametres
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.GetParameters.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdAggMassivoDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdAggMassivoDate.Click
        Try
            Dim sScript As String = ""
            Dim fncGest As New ClsGestioneAccertamenti
            Dim mySearch As New ObjSearchAtti
            Dim TipoAgg As Integer = 0
            Dim DataAgg As String = ""

            mySearch = ValSearchParam()
            If mySearch Is Nothing Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in ricerca');"
                RegisterScript(sScript, Me.GetType())
            Else
                If optAMConsegna.Checked Then
                    TipoAgg = 1
                ElseIf optAMNotifica.Checked Then
                    TipoAgg = 2
                ElseIf optAMSollecito.Checked Then
                    TipoAgg = 3
                ElseIf optAMCoattivo.Checked Then
                    TipoAgg = 4
                ElseIf optAMIrreperibile.Checked Then
                    TipoAgg = 5
                End If
                If txtAMData.Text <> "" Then
                    DataAgg = New OPENgovTIA.generalClass.generalFunction().FormattaData(txtAMData.Text, "A")
                End If
                If fncGest.AggMassivoDate(ConstSession.StringConnection, mySearch, TipoAgg, DataAgg) Then
                    Session("oSearchAttiAvanzata") = mySearch
                    sScript = "GestAlert('a', 'success', '', '', 'Aggiornamento terminato correttamente.');"
                    sScript += "attesaGestioneAtti.style.display='none';document.getElementById('Date').style.display ='none';document.getElementById('divAggMassivo').style.display ='none';"
                    sScript += "document.getElementById('loadGrid').src = 'SearchAttiRicercaAvanzata.aspx'"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in Aggiornamento.');attesaGestioneAtti.style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.CmdAggMassivoDate_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function ValSearchParam() As ObjSearchAtti
        Dim myParam As New ObjSearchAtti
        Try
            myParam.IdEnte = ddlEnti.SelectedValue
            myParam.Anno = ddlAnno.SelectedValue
            myParam.Tributo = ddlTributo.SelectedValue
            myParam.TipoProv = ddlTipologiaTributo.SelectedValue
            If txtDataGenerazioneDal.Text <> "" Then
                myParam.Generazione.Dal = txtDataGenerazioneDal.Text
            End If
            If txtDataGenerazioneAL.Text <> "" Then
                myParam.Generazione.Al = txtDataGenerazioneAL.Text
            End If
            If optConfermaAvvisoNoDate.Checked Then
                myParam.ConfermaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataConfermaAvvisoDal.Text <> "" Then
                    myParam.ConfermaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.ConfermaAvviso.Al = txtDataConfermaAvvisoDal.Text
                End If
                If txtDataConfermaAvvisoAl.Text <> "" Then
                    myParam.ConfermaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.ConfermaAvviso.Al = txtDataConfermaAvvisoAl.Text
                End If
            End If
            If optStampaAvvisoNoDate.Checked Then
                myParam.StampaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataStampaAvvisoDal.Text <> "" Then
                    myParam.StampaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.StampaAvviso.Dal = txtDataStampaAvvisoDal.Text
                End If
                If txtDataStampaAvvisoAl.Text <> "" Then
                    myParam.StampaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.StampaAvviso.Al = txtDataStampaAvvisoAl.Text
                End If
            End If
            If optConsegnaAvvisoNoDate.Checked Then
                myParam.ConsegnaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataConsegnaAvvisoDal.Text <> "" Then
                    myParam.ConsegnaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.ConsegnaAvviso.Dal = txtDataConsegnaAvvisoDal.Text
                End If
                If txtDataConsegnaAvvisoAl.Text <> "" Then
                    myParam.ConsegnaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.ConsegnaAvviso.Al = txtDataConsegnaAvvisoAl.Text
                End If
            End If
            If optNotificaAvvisoNoDate.Checked Then
                myParam.NotificaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataNotificaAvvisoDal.Text <> "" Then
                    myParam.NotificaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.NotificaAvviso.Dal = txtDataNotificaAvvisoDal.Text
                End If
                If txtDataNotificaAvvisoAl.Text <> "" Then
                    myParam.NotificaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.NotificaAvviso.Al = txtDataNotificaAvvisoAl.Text
                End If
            End If
            If optRettificaAvvisoNoDate.Checked Then
                myParam.RettificaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataRettificaAvvisoDal.Text <> "" Then
                    myParam.RettificaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.RettificaAvviso.Dal = txtDataRettificaAvvisoDal.Text
                End If
                If txtDataRettificaAvvisoAl.Text <> "" Then
                    myParam.RettificaAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.RettificaAvviso.Al = txtDataRettificaAvvisoAl.Text
                End If
            End If
            If optAnnullamentoAvvisoNoDate.Checked Then
                myParam.AnnullamentoAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataAnnulamentoAvvisoDal.Text <> "" Then
                    myParam.AnnullamentoAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.AnnullamentoAvviso.Dal = txtDataAnnulamentoAvvisoDal.Text
                End If
                If txtDataAnnulamentoAvvisoAl.Text <> "" Then
                    myParam.AnnullamentoAvviso.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.AnnullamentoAvviso.Al = txtDataAnnulamentoAvvisoAl.Text
                End If
            End If
            If optSospensioneAutotutelaNoDate.Checked Then
                myParam.SospensioneAutotutela.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataSopensioneAutotutelaDal.Text <> "" Then
                    myParam.SospensioneAutotutela.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.SospensioneAutotutela.Dal = txtDataSopensioneAutotutelaDal.Text
                End If
                If txtDataSopensioneAutotutelaAl.Text <> "" Then
                    myParam.SospensioneAutotutela.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.SospensioneAutotutela.Al = txtDataSopensioneAutotutelaAl.Text
                End If
            End If
            If optAttoDefinitivoNoDate.Checked Then
                myParam.AttoDefinitivo.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataAttoDefinitivoDal.Text <> "" Then
                    myParam.AttoDefinitivo.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.AttoDefinitivo.Dal = txtDataAttoDefinitivoDal.Text
                End If
                If txtDataAttoDefinitivoAl.Text <> "" Then
                    myParam.AttoDefinitivo.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.AttoDefinitivo.Al = txtDataAttoDefinitivoAl.Text
                End If
            End If
            If optPagamentoNoDate.Checked Then
                myParam.Pagamento.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataPagamentoDal.Text <> "" Then
                    myParam.Pagamento.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.Pagamento.Dal = txtDataPagamentoDal.Text
                End If
                If txtDataPagamentoAl.Text <> "" Then
                    myParam.Pagamento.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.Pagamento.Al = txtDataPagamentoAl.Text
                End If
            End If
            If optSollecitoBonarioNoDate.Checked Then
                myParam.SollecitoBonario.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataSollecitoBonarioDal.Text <> "" Then
                    myParam.SollecitoBonario.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.SollecitoBonario.Dal = txtDataSollecitoBonarioDal.Text
                End If
                If txtDataSollecitoBonarioAl.Text <> "" Then
                    myParam.SollecitoBonario.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.SollecitoBonario.Al = txtDataSollecitoBonarioAl.Text
                End If
            End If
            If optCoattivoNoDate.Checked Then
                myParam.Coattivo.TipoRic = ObjSearchAttiAvanzataDate.DateNessuna
            Else
                If txtDataCoattivoDal.Text <> "" Then
                    myParam.Coattivo.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.Coattivo.Dal = txtDataCoattivoDal.Text
                End If
                If txtDataCoattivoAl.Text <> "" Then
                    myParam.Coattivo.TipoRic = ObjSearchAttiAvanzataDate.DateSelezione
                    myParam.Coattivo.Al = txtDataCoattivoAl.Text
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.ValSearchParam.errore: ", ex)
            myParam = Nothing
        End Try
        Return myParam
    End Function
#Region "290"
    '*** 20171219 - estrazione 290 non ancora da rilasciare perché non ancora comprata da nessuno ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdEstrazione290_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEstrazione290.Click
        Dim sScript As String = ""
        'Dim FunctionEstrazioni As New SalvataggioRuolo.SalvataggioRuolo
        Dim nReturnEstraz As Integer
        Dim sPathFile290, sNameFile290, myErr As String
        Dim ListFile As New ArrayList

        Try
            'controllo che siano stati configurati i codici enti
            If ConstSession.IdEnteCredBen <> "" And ConstSession.Belfiore <> "" Then
                'valorizzo il perco rso e il nome del file
                sPathFile290 = ConstSession.PathEstrazione290
                sNameFile290 = ConstSession.IdEnteCredBen & Format(Now, "yy")
                'richiamo la funzione di estrazione
                nReturnEstraz = New clsCoattivo.cls290().Crea290(ConstSession.StringConnection, ConstSession.IdEnteCNC, txtDataCoattivoDal.Text, txtDataCoattivoAl.Text, -1, ConstSession.IdEnteCredBen, 1, sPathFile290, sNameFile290, myErr, ListFile)
                If nReturnEstraz = -2 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Sono presenti delle anagrafiche con data nascita errata e/o senza l\'indicazione del sesso!\n" + myErr + "\nEstrazione non effettuata!');"
                    sScript += "attesaGestioneAtti.style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                ElseIf nReturnEstraz < 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Estrazione non effettuata!');"
                    sScript += "attesaGestioneAtti.style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                ElseIf nReturnEstraz = 0 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un errore!');"
                    sScript += "attesaGestioneAtti.style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'success', '', '', 'Estrazione 290 terminata con successo!\nN.B.\nControllare che il campo informazioni non superi i 75chr sforando nel filler!');"
                    sScript += "document.getElementById('Date').style.display ='';document.getElementById('txtDIVDate').value ='1';"
                    sScript += "document.getElementById('LblIntest290').style.display = '';document.getElementById('LblFile290').style.display = '';"
                    sScript += "attesaGestioneAtti.style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                    LblFile290.Text = sNameFile290 & ".zip"
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Manca la configurazione del codice ente impositore.\nImpossibile proseguire!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaAvanzata.CmdEstrazione290_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
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
#End Region
    '*** ***

End Class