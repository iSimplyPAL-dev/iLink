Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class PosizioneContatore

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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(PosizioneContatore))

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim CODPOSIZIONE As Integer

        Dim sScript As String = ""
        sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
        RegisterScript(sScript, Me.GetType())
        btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
        Try
            If Not Page.IsPostBack Then

                CODPOSIZIONE = Utility.StringOperation.FormatInt(Request.QueryString("CODPOSIZIONE"))

                If CODPOSIZIONE <> -1 Then
                    lblOperation.Text = "Dati Posizione Contatore - Modifica Posizione Contatore"
                Else
                    lblOperation.Text = "Dati Posizione Contatore - Inserimento Posizione Contatore"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())
                End If

                Dim oPosizioneContatore As New TabelleDiDecodifica.DBPosizioneContatore
                Dim DetailPosizioneContatore As New TabelleDiDecodifica.PosizioneContatore

                DetailPosizioneContatore = oPosizioneContatore.GetPosizioneContatore(CODPOSIZIONE)
                If DetailPosizioneContatore.Posizione = -1 Then
                    txtPosizione.Text = ""
                Else
                    txtPosizione.Text = DetailPosizioneContatore.Posizione
                End If

                txtDescrizione.Text = DetailPosizioneContatore.Descrizione
                txtNote.Text = DetailPosizioneContatore.Note
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.PosizioneContatore.Page_Load.errore: ", ex)
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
        Try
            Dim CODPOSIZIONE As Integer

            CODPOSIZIONE = Utility.StringOperation.FormatInt(Request.QueryString("CODPOSIZIONE"))

            Dim oPosizioneContatore As TabelleDiDecodifica.DBPosizioneContatore = New TabelleDiDecodifica.DBPosizioneContatore
            Dim oDetail As New TabelleDiDecodifica.PosizioneContatore
            lblErrorMessage.Text = ""

            oDetail.CODPOSIZIONE = CODPOSIZIONE
            If Len(txtPosizione.Text) = 0 Then
                oDetail.Posizione = 0
            Else
                oDetail.Posizione = txtPosizione.Text
            End If

            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oPosizioneContatore.SetPosizioneContatore(oDetail)
                If oDetail.CODPOSIZIONE = -1 Then
                    sScript += "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente');"
                Else
                    sScript += "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente');"
                End If
                RegisterScript(sScript, Me.GetType)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        sScript += "Aggiorna();"
                        RegisterScript(sScript, Me.GetType())
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnSalva_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                sScript += "parent.location.href='SearchPosizioneContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per la cancellazione dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Try
            Dim CODPOSIZIONE As Integer

            CODPOSIZIONE = Int32.Parse(Request.QueryString("CODPOSIZIONE"))
            Dim oPosizioneContatore As TabelleDiDecodifica.DBPosizioneContatore = New TabelleDiDecodifica.DBPosizioneContatore
            Try
                oPosizioneContatore.EliminaPosizioneContatore(CODPOSIZIONE)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                dim sScript as string=""

                
                sscript+="parent.location.href='SearchPosizioneContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                

                RegisterScript(sScript , Me.GetType())

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnCancella_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante di annullamento operativa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click

        Dim _const As New Costanti

        dim sScript as string=""

        
        sscript+="parent.location.href='SearchPosizioneContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"
        

        RegisterScript(sScript , Me.GetType())

    End Sub
    ''' <summary>
    ''' pulsante per la modifica dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnForzaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaModifica.Click
        Try
            Dim CODPOSIZIONE As Integer

            CODPOSIZIONE = Int32.Parse(Request.QueryString("CODPOSIZIONE"))

            Dim oPosizioneContatore As TabelleDiDecodifica.DBPosizioneContatore = New TabelleDiDecodifica.DBPosizioneContatore
            Dim oDetail As New TabelleDiDecodifica.PosizioneContatore
            lblErrorMessage.Text = ""

            oDetail.CODPOSIZIONE = CODPOSIZIONE
            oDetail.Posizione = txtPosizione.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oPosizioneContatore.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                dim sScript as string=""

                
                sscript+="parent.location.href='SearchPosizioneContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                

                RegisterScript(sScript , Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
