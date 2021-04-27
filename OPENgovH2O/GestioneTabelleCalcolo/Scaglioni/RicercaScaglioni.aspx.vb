Imports log4net
Imports Utility

Partial Class RicercaScaglioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaScaglioni))

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
    ''' Configurazione per anno e per singola tipologia di utenza degli scaglioni di consumo 
    ''' possibilità di applicare o meno l'iva 
    ''' possibilità di esprimere una quota minima
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim clsCanoni As New ClsCanoni
        Dim sScript As String
        ddlTipoUtenza.Enabled = False
        'clsCanoni.LoadComboUtenze(ddlTipoUtenza)
        'If Request.Item("EffettuaRicerca") = "si" Then
        sScript = "Search();"
        RegisterScript(sScript, Me.GetType())

        sScript = "parent.parent.Comandi.location.href='./CRicercaScaglioni.aspx';"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' caricamento delle sole tipologie di utenze attive
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtAnno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAnno.TextChanged
        Dim clsCanoni As New ClsCanoni
        If (IsNumeric(txtAnno.Text) And (txtAnno.Text.Length = 4)) Then
            clsCanoni.LoadComboUtenze(ddlTipoUtenza, txtAnno.Text)
            ddlTipoUtenza.Enabled = True
        End If
    End Sub

    Protected Sub btnRibalta_Click(sender As Object, e As EventArgs) Handles btnRibalta.Click
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim MyId As Integer = 0
        Dim sScript As String = ""
        Try
            If StringOperation.FormatInt(txtAnno.Text) > 0 Then
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_RibaltaScaglioni", "IDENTE", "ANNOFROM", "ANNOTO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                        , ctx.GetParam("ANNOFROM", txtAnno.Text) _
                        , ctx.GetParam("ANNOTO", txtAnno.Text + 1)
                    )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        MyId = Utility.StringOperation.FormatInt(myRow("ID"))
                    Next
                End If
                If MyId <= 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Errore in ribaltamento!');"
                Else
                    sScript = "GestAlert('a', 'success', '', '', 'Ribaltamento effettuato correttamente!'); Search();"
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare anno!');"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.prc_RibaltaScaglioni.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
    End Sub
End Class
