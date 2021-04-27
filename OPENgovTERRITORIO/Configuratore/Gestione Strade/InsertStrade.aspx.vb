Imports log4net
''' <summary>
''' Pagina per la configurazione dello stradario.
''' Contiene i parametri di configurazione e le funzioni della comandiera. 
''' </summary>
Partial Class InsertStrade
    Inherits BaseEnte
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(InsertStrade))
    Private WFErrore As String
    Private FncDB As New ClsDB

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TxtNomeStrada As System.Web.UI.WebControls.TextBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim dtMyDati As New DataTable()
        Dim oLoadCombo As New ClsUtilities

        Try
            btnAreeStrade.Attributes.Add("onclick", "apriAreeStrade()")
            btnCappario.Attributes.Add("onclick", "apriCappario()")

            If Page.IsPostBack = False Then
                'Dim sScript As String = "parent.Comandi.location.href='ComandiInserimento.aspx'" & vbCrLf
                'sScript = sScript & "document.Form1.ddlToponimo.focus()"
                'RegisterScript("", "<script language='javascript'>" & sScript & "</script>")

                oLoadCombo.PopolaComboVieFrazioni(ConstSession.IdEnte, ddlToponimo, ddlFrazione, ConstSession.StringConnection)
                btnAreeStrade.Enabled = False
                btnCappario.Enabled = False
                txtCap.Text = Session("CAP_ENTE")

                If Not IsNothing(Request.Item("CodStrada")) Then
                    If Trim(Request.Item("CodStrada")) <> "" Then
                        dtMyDati = FncDB.GetSQLRicercaStrade(ConstSession.StringConnection, ConstSession.IdEnte, Request.Item("CodStrada"), -1, -1, "", "")
                        For Each dtMyRow As DataRow In dtMyDati.Rows
                            TxtCodStrada.Text = dtMyRow("id_via")
                            ddlToponimo.SelectedValue = dtMyRow("id_toponimo")
                            ddlFrazione.SelectedValue = dtMyRow("id_frazione")
                            txtDenominazione.Text = dtMyRow("descrizione_via")
                            If Not IsDBNull(dtMyRow("cap")) Then
                                txtCap.Text = dtMyRow("cap")
                            End If
                            If Not IsDBNull(dtMyRow("lunghezza")) Then
                                txtLunghezza.Text = dtMyRow("lunghezza")
                            End If
                            If Not IsDBNull(dtMyRow("larghezza")) Then
                                txtLarghezza.Text = dtMyRow("larghezza")
                            End If
                            If Not IsDBNull(dtMyRow("ex_denominazione")) Then
                                txtExDenominazione.Text = dtMyRow("ex_denominazione")
                            End If
                            If Not IsDBNull(dtMyRow("note")) Then
                                txtNote.Text = dtMyRow("note")
                            End If
                        Next
                        dtMyDati.Dispose()
                    End If
                End If
            End If

        Catch ex As Exception
            Log.Error("Si è verificato un errore in InsertStrade::Page_Load::" & ex.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub Insert_Strada()
        Try
            Dim sScript As String
            Dim CodStrada As Integer
            Dim nMyReturn, nTypeOperation As Integer
            Dim dtMyDati As New DataTable()

            'prelevo i valori inseriti dall'operatore
            If Trim(txtLunghezza.Text) = "" Then txtLunghezza.Text = 0
            If Trim(txtLarghezza.Text) = "" Then txtLarghezza.Text = 0

            txtDenominazione.Text = txtDenominazione.Text.ToUpper()

            'INSERIMENTO IN STRADE
            If TxtCodStrada.Text > 0 Then
                nTypeOperation = FncDB.TYPEOPERATION_UPDATE
                CodStrada = TxtCodStrada.Text
            Else
                nTypeOperation = FncDB.TYPEOPERATION_INSERT
                'controllo se strada esiste già (toponimo + nome strada)
                dtMyDati = FncDB.GetSQLRicercaStrade(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlToponimo.SelectedValue, ddlFrazione.SelectedValue, txtDenominazione.Text, txtCap.Text)
                For Each dtMyRow As DataRow In dtMyDati.Rows
                    sScript = "GestAlert('a', 'warning', '', '', 'Strada gia\' presente!\nImpossibile proseguire con l\'inserimento!');"
                    sScript = sScript & "document.getElementById('ddlToponimo').focus()"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                Next
                dtMyDati.Dispose()
            End If

            dtMyDati = FncDB.GetSQLGestioneStrade(ConstSession.StringConnection, nTypeOperation, ConstSession.IdEnte, TxtCodStrada.Text, ddlToponimo.SelectedValue, ddlFrazione.SelectedValue, txtDenominazione.Text)
            For Each dtMyRow As DataRow In dtMyDati.Rows
                CodStrada = CStr(dtMyRow(0))
            Next
            dtMyDati.Dispose()

            TxtCodStrada.Text = CodStrada
            btnAreeStrade.Enabled = True
            btnCappario.Enabled = True

            dtMyDati = FncDB.GetSQLGestioneDettaglioStrade(ConstSession.StringConnection, nTypeOperation, CodStrada, CodStrada, txtExDenominazione.Text, txtCap.Text, txtLunghezza.Text, txtLarghezza.Text, txtNote.Text)
            For Each dtMyRow As DataRow In dtMyDati.Rows
                nMyReturn = CInt(dtMyRow(0))
            Next
            dtMyDati.Dispose()

            sScript = "GestAlert('a', 'warning', '', '', 'Strada inserita con successo!');"
            sScript = sScript & "location.href='RicercaStrade.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Error("Si è verificato un errore in InsertStrade::Insert_Strada::" & ex.Message)
            Throw
        End Try
    End Sub

    Private Sub ClearControls()
        Try
            btnCappario.Enabled = False : btnAreeStrade.Enabled = False
            TxtCodStrada.Text = "" : TxtNomeStrada.Text = ""
            ddlToponimo.SelectedIndex = 0 : txtDenominazione.Text = "" : txtCap.Text = Session("CAP_ENTE") ' txtCap.Text = ""
            txtLunghezza.Text = "" : txtLarghezza.Text = "" : txtExDenominazione.Text = "" : txtNote.Text = ""
            RegisterScript("document.getElementById('ddlToponimo').focus();", Me.GetType())
        Catch ex As Exception
            Log.Error("Si è verificato un errore in InsertStrade::ClearControls::" & ex.Message)
            Throw
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("RicercaStrade.aspx")
    End Sub

    Private Sub Insert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Insert.Click
        Try
            Call Insert_Strada()
        Catch ex As Exception
            Log.Error("Si è verificato un errore in InsertStrade::Insert_Click::" & ex.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Clear.Click
        Call ClearControls()
    End Sub
End Class
