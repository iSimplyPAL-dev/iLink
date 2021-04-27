Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class Anomalie
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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Anomalie))
    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim CODANOMALIA As Integer
            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then

                CODANOMALIA = Int32.Parse(Request.QueryString("CODANOMALIA"))

                If CODANOMALIA <> -1 Then
                    lblOperation.Text = "Dati Anomalie - Modifica Anomalie"
                Else

                    lblOperation.Text = "Dati Anomalie - Inserimento Anomalie"

                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())
                End If

                Dim oAnomalie As New TabelleDiDecodifica.DBAnomalie
                Dim DetailAnomalie As New TabelleDiDecodifica.DetailAnomalie

                DetailAnomalie = oAnomalie.GetAnomalie(CODANOMALIA)

                txtCodiceAnomalia.Text = DetailAnomalie.CodiceAnomalia
                txtDescrizione.Text = DetailAnomalie.Descrizione
                txtNote.Text = DetailAnomalie.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.Page_Load.errore: ", Err)
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

        Dim CODANOMALIA As Integer

        CODANOMALIA = Int32.Parse(Request.QueryString("CODANOMALIA"))

        Dim oAnomalia As TabelleDiDecodifica.DBAnomalie = New TabelleDiDecodifica.DBAnomalie
        Dim oDetail As New TabelleDiDecodifica.DetailAnomalie
        lblErrorMessage.Text = ""

        oDetail.CodAnomalia = CODANOMALIA
        oDetail.CodiceAnomalia = txtCodiceAnomalia.Text
        oDetail.Descrizione = txtDescrizione.Text
        oDetail.Note = txtNote.Text
        Try
            oAnomalia.SetANOMALIE(oDetail)
            If oDetail.CodAnomalia = -1 Then
                RegisterScript("GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente');", Me.GetType)
            Else
                RegisterScript("GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente');", Me.GetType)
            End If
        Catch ex As SqlException
            Select Case ex.Number
                Case 547
                    Dim sScript As String = ""
                    sScript += "Aggiorna();"
                    RegisterScript(sScript, Me.GetType())
                Case Else
                    lblErrorMessage.Text = ex.Message
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Anomalie.btnSalva_Click.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
            End Select
        Catch ex As Exception
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Len(lblErrorMessage.Text) = 0 Then

            Dim sScript As String = ""

            sScript += "parent.location.href='SearchAnomalia.aspx?paginacomandi=" & Request("paginacomandi") & "';"
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

        Dim CODANOMALIA As Integer

        CODANOMALIA = Int32.Parse(Request.QueryString("CODANOMALIA"))
        Dim oAnomalia As TabelleDiDecodifica.DBAnomalie = New TabelleDiDecodifica.DBAnomalie

        Try
            oAnomalia.EliminaAnomalia(CODANOMALIA)
        Catch ex As SqlException
            Select Case ex.Number
                Case 547
                    lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnCancella_Click.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
                Case Else
                    lblErrorMessage.Text = ex.Message
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnCancella_Click.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
            End Select
        Catch ex As Exception
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If Len(lblErrorMessage.Text) = 0 Then
            Dim sScript As String = ""
            sScript += "parent.location.href='SearchAnomalia.aspx?paginacomandi=" & Request("paginacomandi") & "';"
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

            Dim sScript As String = ""
            sScript += "parent.location.href='SearchAnomalia.aspx?paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnSalva_Click.errore: ", ex)
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

        Dim CODANOMALIA As Integer

        CODANOMALIA = Int32.Parse(Request.QueryString("CODANOMALIA"))

        Dim oAnomalia As TabelleDiDecodifica.DBAnomalie = New TabelleDiDecodifica.DBAnomalie
        Dim oDetail As New TabelleDiDecodifica.DetailAnomalie
        lblErrorMessage.Text = ""

        oDetail.CodAnomalia = CODANOMALIA
        oDetail.CodiceAnomalia = txtCodiceAnomalia.Text
        oDetail.Descrizione = txtDescrizione.Text
        oDetail.Note = txtNote.Text

        Try
            oAnomalia.UpdateForzato(oDetail)
        Catch ex As SqlException
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Catch ex As Exception
            lblErrorMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.Anomalie.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Len(lblErrorMessage.Text) = 0 Then

            Dim sScript As String = ""
            sScript += "parent.location.href='SearchAnomalia.aspx?paginacomandi=" & Request("paginacomandi") & "';"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
End Class
