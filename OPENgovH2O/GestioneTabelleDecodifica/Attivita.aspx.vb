Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class ATTIVITA
    Inherits BasePage

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

    Dim Generali As New Generali()
    Dim _Const As New Costanti()
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ATTIVITA))

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
        'Dim paginacomandi As String = Request("paginacomandi")
        'Dim parametri As String
        'parametri = "?title=Dizionario - " & Session("DESC_TIPO_PROC_SERV") & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

        'dim sScript as string=""
        '
        'sscript+="document.getElementById('paginacomandi').value='" & paginacomandi & "';")
        '
        'RegisterScript(sScript , Me.GetType())
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
        Try
            Dim IDTIPOATTIVITA As Integer

            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then
                IDTIPOATTIVITA = Int32.Parse(Request.QueryString("IDTIPOATTIVITA"))

                If IDTIPOATTIVITA <> -1 Then
                    lblOperation.Text = "Dati Attivit� - Modifica Attivit�"
                Else
                    lblOperation.Text = "Dati Attivit� - Inserimento Attivit�"
                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())
                End If

                Dim oAttivita As New TabelleDiDecodifica.DBAttivita
                Dim DetailAttivita As New TabelleDiDecodifica.DetailAttivita

                DetailAttivita = oAttivita.GetAttivita(IDTIPOATTIVITA)

                txtDescrizione.Text = DetailAttivita.Descrizione
                txtNote.Text = DetailAttivita.Note
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub
    ''' <summary>
    ''' pulsante per il salvataggio dei dati della videata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim sScript As String = ""
        Dim IDTIPOATTIVITA As Integer

        IDTIPOATTIVITA = Int32.Parse(Request.QueryString("IDTIPOATTIVITA"))

        Dim oAttivita As TabelleDiDecodifica.DBAttivita = New TabelleDiDecodifica.DBAttivita
        Dim oDetail As New TabelleDiDecodifica.DetailAttivita
        lblErrorMessage.Text = ""

        oDetail.IDAttivita = IDTIPOATTIVITA
        oDetail.Descrizione = txtDescrizione.Text
        oDetail.Note = txtNote.Text
        Try
            oAttivita.SetATTIVITA(oDetail)
            If oAttivita.IDAttivita = -1 Then
                sScript += "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente');"
            Else
                sScript += "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente');"
            End If
            RegisterScript(sscript, Me.GetType)
        Catch ex As SqlException
            Select Case ex.Number
                Case 547
                    sScript += "Aggiorna();"
                    RegisterScript(sScript, Me.GetType())
                Case Else
                    lblErrorMessage.Text = ex.Message
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ATTIVITA.btnSalva_Click.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
            End Select
        Catch ex As Exception
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Len(lblErrorMessage.Text) = 0 Then
            sScript += "parent.location.href='SearchAttivita.aspx?paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        End If

    End Sub
    ''' <summary>
    ''' pulsante per la cancellazione dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click

        Dim IDTIPOATTIVITA As Integer

        lblErrorMessage.Text = ""

        IDTIPOATTIVITA = Int32.Parse(Request.QueryString("IDTIPOATTIVITA"))
        Dim oAttivita As TabelleDiDecodifica.DBAttivita = New TabelleDiDecodifica.DBAttivita

        Try

            oAttivita.EliminaAttivita(IDTIPOATTIVITA)

        Catch ex As SqlException
            Select Case ex.Number
                Case 547

                    lblErrorMessage.Text = "L'elemento che si desidera eliminare � usato da altre tabelle.Impossibile la cancellazione!"
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnCancella_Click.errore: ", ex)
                Case Else
                    lblErrorMessage.Text = ex.Message
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnCancella_Click.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
            End Select
        Catch ex As Exception
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnCancella_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Len(lblErrorMessage.Text) = 0 Then
            Dim sScript As String = ""

            sScript += "parent.location.href='SearchAttivita.aspx?paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
    ''' <summary>
    ''' pulsante di annullamento operativa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click
        Try
            Dim _const As New Costanti

            dim sScript as string=""

            sScript += "parent.location.href='SearchAttivita.aspx?paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnAnnulla.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")

        End Try
    End Sub
    ''' <summary>
    ''' pulsante per la modifica dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnForzaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaModifica.Click

        Dim IDTIPOATTIVITA As Integer

        IDTIPOATTIVITA = Int32.Parse(Request.QueryString("IDTIPOATTIVITA"))

        Dim oAttivita As TabelleDiDecodifica.DBAttivita = New TabelleDiDecodifica.DBAttivita
        Dim oDetail As New TabelleDiDecodifica.DetailAttivita
        lblErrorMessage.Text = ""

        oDetail.IDAttivita = IDTIPOATTIVITA
        oDetail.Descrizione = txtDescrizione.Text
        oDetail.Note = txtNote.Text

        Try
            oAttivita.UpdateForzato(oDetail)
        Catch ex As SqlException
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")

        Catch ex As Exception
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.ATTIVITA.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Len(lblErrorMessage.Text) = 0 Then
            Dim sScript As String = ""
            sScript += "parent.location.href='SearchAttivita.aspx?paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
End Class
