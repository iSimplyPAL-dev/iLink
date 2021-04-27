Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione delle pertinenze in generazione provvedimento.
''' Contiene i parametri di gestione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class Pertinenze
    Inherits BasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnSearchDichiarazioni As System.Web.UI.WebControls.Button
    Protected WithEvents idImmobile As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtidImmobile As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Pertinenze))
    Private idLegame As Integer
    Private IdImmobileDiPertitenza As Integer

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Not Page.IsPostBack Then
                Dim sScript As String = ""
                sScript += "parent.Comandi.location.href='ComandiGestionePertinenze.aspx'" & vbCrLf
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pertinenze.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function searchImmobileDiPertinenza() As Boolean
        Try
            Dim workTable() As objUIICIAccert

            workTable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())

            Dim Trovato As Boolean = False
            '*** 20130116 - non devo trasformarlo in numero altrimenti darà errore nel test successivo
            'Dim sSub As Integer = -1
            'If txtSubalterno.Text <> "" Then
            '          sSub = txtSubalterno.Text
            'End If
            '*** ***
            If Not workTable Is Nothing Then
                'workTable.Select("")
                For each myItem As objUIICIAccert In  workTable
                    '*** 20130116 - non devo trasformarlo in numero altrimenti darà errore nel test successivo
                    'If (workTable.Rows(i).Item("Flag_Principale") = "True" Or workTable.Rows(i).Item("Flag_Principale").ToString() = "1") And workTable.Rows(i).Item("Foglio") = txtFoglio.Text And workTable.Rows(i).Item("Numero") = txtNumero.Text And workTable.Rows(i).Item("Subalterno") = sSub Then
                    If myItem.FlagPrincipale = 1 And myItem.Foglio = txtFoglio.Text And myItem.Numero = txtNumero.Text And myItem.Subalterno = txtSubalterno.Text Then
                        '*** ***
                        'alep 22112007
                        Session("IdImmobileDiPertinenza") = myItem.IdImmobileDichiarato
                        Trovato = True
                        Exit For
                    End If
                Next
            End If

            Return Trovato
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pertinenze.searchImmobileDiPertinenza.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCercaImmobile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCercaImmobile.Click
        Dim sScript As String = ""
        Session("IdImmobileDiPertinenza") = "System.DBNull"
        'Se è una pertinenza devo inserire l'id dell'immobile di pertinenza
        Try
            If searchImmobileDiPertinenza() = False Then
                sScript += "GestAlert('a', 'warning', '', '', 'Impossibile Inserire la Pertinenza, non è stato trovato nessun immobile a cui associarla.');"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript += "GestAlert('a', 'success', '', '', 'Pertinenza associata correttamente.');"
                sScript += "parent.window.close();"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pertinenze.btnCercaImmobile_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
