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
Imports log4net
''' <summary>
''' Pagina per la consultazione dei provvedimenti.
''' Contiene i parametri di ricerca, le funzioni della comandiera e iframe per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaSemplice
    Inherits BaseEnte

    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaSemplice))
   
    'Private WFSessione As New CreateSession
    Private WFErrore As String

    'Dim oAnagrafica As GestioneAnagrafica

    Protected WithEvents Clear As System.Web.UI.WebControls.Button
    Protected WithEvents Search As System.Web.UI.WebControls.Button
    Protected WithEvents CreaLettera As System.Web.UI.WebControls.Button
    Protected WithEvents opt1 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents opt2 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents opt3 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents opt4 As System.Web.UI.WebControls.RadioButton

    Dim oDettaglioAnagrafica As DettaglioAnagrafica
    Protected WithEvents optManuale As System.Web.UI.WebControls.RadioButton
    Private Const SEARCH_PARAMETRES_GESTIONE_ATTI As String = "SEARCH_PARAMETRES_GESTIONE_ATTI"

    Private Const SEARCH_PARAMETRES_RICERCA_SEMPLICE As String = "SEARCH_PARAMETRES_RICERCA_SEMPLICE"

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
        Dim sScript As String = ""
        Try

            Session("modificaDiretta") = "False"
            Session("ComplitData") = ""
            Session("HashTableData") = ""
            Session("oAnagrafe") = Nothing

            Dim hdSearchParametres As Hashtable = New Hashtable

            If Page.IsPostBack = False Then

                ViewState("sessionName") = ""

                txtRicercaAttiva.Text = "-1"
                'carico i tributi
                PopolaComboTributo()

                If Not Request.Item("IdEnte") Is Nothing Then
                    Session("cod_ente") = Request.Item("IdEnte")
                    txtCognome.Text = Request.Item("Cognome")
                    txtNome.Text = Request.Item("Nome")
                    txtCodiceFiscale.Text = Request.Item("CodiceFiscale")
                    txtPartitaIva.Text = Request.Item("PartitaIVA")
                    sScript = "document.getElementById('loadGrid').src='SearchAtti.aspx?COGNOME=" & Request.Item("Cognome") & "&NOME=" & Request.Item("Nome") & "&CODICEFISCALE=" & Request.Item("CodiceFiscale") & "&PARTITAIVA=" & Request.Item("PartitaIVA") & "';"
                    sScript += "parent.Comandi.location.href='ComandiRicercaSempice.aspx';"
                    RegisterScript(sScript, Me.GetType())
                Else
                    hdSearchParametres = CType(Session(SEARCH_PARAMETRES_RICERCA_SEMPLICE), Hashtable)

                    If Not hdSearchParametres Is Nothing Then
                        txtCognome.Text = hdSearchParametres("COGNOME")
                        txtNome.Text = hdSearchParametres("NOME")
                        txtCodiceFiscale.Text = hdSearchParametres("CODICEFISCALE")
                        txtPartitaIva.Text = hdSearchParametres("PARTITAIVA")
                    Else
                        txtCognome.Text = ""
                        txtNome.Text = ""
                        txtCodiceFiscale.Text = ""
                        txtPartitaIva.Text = ""
                    End If
                    sScript += "parent.Comandi.location.href='ComandiRicercaSempice.aspx';"
                    RegisterScript(sScript, Me.GetType())
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "RicercaSemplice", Utility.Costanti.AZIONE_LETTURA.ToString, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, -1)
            End If

            If Not hdSearchParametres Is Nothing Then
                sScript += "Search();"
                RegisterScript(sScript, Me.GetType())
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaSemplice.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboTributo()
        Dim myUtil As New MyUtility()
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
        Try
            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

            'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
            DdlCodTributo.Items.Clear()
            myUtil.FillDropDownSQLValueString(DdlCodTributo, objGestOPENgovProvvedimenti.GetTRIBUTIProvvedimentiAnno(Utility.StringOperation.FormatString(ConstSession.IdEnte), "-1"), -1, "TUTTI")

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaSemplice.PopolaComboTributo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    '// -----------------------------------------------------------------------------
    '// <summary>
    '// Gestione Anagrafica
    '// </summary>
    '// <param name="sender"></param>
    '// <param name="e"></param>
    '// <remarks>
    '// </remarks>
    '// <history>
    '// 	[antonello]	09/03/2005	Created
    '// </history>
    '// -----------------------------------------------------------------------------
    'Private Sub Imagebutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    'Try
    '  If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '    Response.Write(WFErrore)
    '    Response.End()
    '  End If

    '  oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
    '  oDettaglioAnagrafica = New DettaglioAnagrafica
    '  oDettaglioAnagrafica.COD_CONTRIBUENTE = txtHiddenCodContribuente.Text
    '  oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = txtHiddenIdDataAnagrafica.Text
    '  ViewState("sessionName") = "codContribuente"
    '  Session(ViewState("sessionName")) = oDettaglioAnagrafica
    '  writeJavascriptAnagrafica(ViewState("sessionName"))
    'Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RicercaSemplice.Imagebutton_Click.errore: ", ex)
    '     Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
    '// -----------------------------------------------------------------------------
    '// <summary>
    '// Gestione Anagrafica
    '// </summary>
    '// <param name="nomeSessione"></param>
    '// <remarks>
    '// </remarks>
    '// <history>
    '// 	[antonello]	09/03/2005	Created
    '// </history>
    '// -----------------------------------------------------------------------------
    'Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
    '  dim sScript as string=""

    '  sscript+=""
    '  sscript+="ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
    '  RegisterScript(sScript , Me.GetType())

    'End Sub
    '// -----------------------------------------------------------------------------
    '// <summary>
    '// Gestione Anagrafica
    '// </summary>
    '// <param name="sender"></param>
    '// <param name="e"></param>
    '// <remarks>
    '// </remarks>
    '// <history>
    '// 	[antonello]	09/03/2005	Created
    '// </history>
    '// -----------------------------------------------------------------------------
    'Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '  oDettaglioAnagrafica = Session(ViewState("sessionName"))

    '  txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome

    '  txtHiddenCodContribuente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '  txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
    '  ViewState("sessionName") = ""

    'End Sub



End Class
