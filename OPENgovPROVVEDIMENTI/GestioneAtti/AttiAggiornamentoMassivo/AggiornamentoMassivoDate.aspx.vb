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
Imports log4net
''' <summary>
''' Pagina per la gestione massiva delle date.
''' Contiene i parametri di gestione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class AggiornamentoMassivoDate
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(AggiornamentoMassivoDate))
    Private WFErrore As String
    Dim oDettaglioAnagrafica As DettaglioAnagrafica
    Dim Utility As New MyUtility
    Protected WithEvents txtDIVDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateRicorso As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCheckBox As System.Web.UI.WebControls.TextBox

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

        Utility = New MyUtility
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
        objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim sScript As String = ""

        Try
            If Not Page.IsPostBack Then

                txtSALVATAGGIOINCORSO.Text = "-1"

                If Utility.CToStr(Request("TIPORICERCA")).CompareTo("SEMPLICE") = 0 Then
                    txtTIPORICERCA.Text = "1"
                End If

                If Utility.CToStr(Request("TIPORICERCA")).CompareTo("MASSIVA") = 0 Then
                    txtTIPORICERCA.Text = "2"
                End If

                txtAggiornaIN.Enabled = False
                'sscript+= "parent.Comandi.location.href='ComandiAggiornamentoMassivo.aspx'" & vbCrLf
                'RegisterScript(sScript , Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.AggiornamentoMassivoDate.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
