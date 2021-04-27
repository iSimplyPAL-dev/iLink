Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class TipoContatore

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

    Private Shared Log As ILog = LogManager.GetLogger(GetType(TipoContatore))

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        Try

            Dim IDTIPOCONTATORE As Integer

            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then

                IDTIPOCONTATORE = Int32.Parse(Request.QueryString("IDTIPOCONTATORE"))

                If IDTIPOCONTATORE <> -1 Then
                    lblOperation.Text = "Dati Tipo Contatore - Modifica Tipo Contatore"
                Else

                    lblOperation.Text = "Dati Tipo Contatore - Inserimento Tipo Contatore"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oTipoContatore As New TabelleDiDecodifica.DBTipoContatore()
                Dim DetailTipoContatore As New TabelleDiDecodifica.TipoContatore()

                DetailTipoContatore = oTipoContatore.GetTipoContatore(IDTIPOCONTATORE)
                If DetailTipoContatore.FondoScala = 0 Or DetailTipoContatore.FondoScala = -1 Then
                    txtValoreFondoScala.Text = ""
                Else
                    txtValoreFondoScala.Text = DetailTipoContatore.FondoScala
                End If

                txtDescrizione.Text = DetailTipoContatore.Descrizione
                txtNote.Text = DetailTipoContatore.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.Page_Load.errore: ", ex)
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
            Dim IDTIPOCONTATORE As Integer

            IDTIPOCONTATORE = Int32.Parse(Request.QueryString("IDTIPOCONTATORE"))

            Dim oTipoContatore As TabelleDiDecodifica.DBTipoContatore = New TabelleDiDecodifica.DBTipoContatore()
            Dim oDetail As New TabelleDiDecodifica.TipoContatore()
            lblErrorMessage.Text = ""

            oDetail.IDTIPOCONTATORE = IDTIPOCONTATORE
            If Len(txtValoreFondoScala.Text) = 0 Then
                oDetail.FondoScala = 0
            Else
                oDetail.FondoScala = txtValoreFondoScala.Text
            End If

            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oTipoContatore.SetTipoContatore(oDetail)
                If oDetail.IDTIPOCONTATORE = -1 Then
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
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                sScript += "parent.location.href='SearchTipiContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.btnSalva_Click.errore: ", ex)
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
            Dim IDTIPOCONTATORE As Integer

            IDTIPOCONTATORE = Int32.Parse(Request.QueryString("IDTIPOCONTATORE"))
            Dim oTipoContatore As TabelleDiDecodifica.DBTipoContatore = New TabelleDiDecodifica.DBTipoContatore()
            Try
                oTipoContatore.EliminaTipoContatore(IDTIPOCONTATORE)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.btnCancella_Click.errore: ", ex)
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

        Dim _const As New Costanti()

        Dim sScript As String = ""


        sScript += "parent.location.href='SearchTipiContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"


        RegisterScript(sScript, Me.GetType())

    End Sub
    ''' <summary>
    ''' pulsante per la modifica dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnForzaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaModifica.Click
        Try
            Dim IDTIPOCONTATORE As Integer

            IDTIPOCONTATORE = Int32.Parse(Request.QueryString("IDTIPOCONTATORE"))

            Dim oTipoContatore As TabelleDiDecodifica.DBTipoContatore = New TabelleDiDecodifica.DBTipoContatore()
            Dim oDetail As New TabelleDiDecodifica.TipoContatore()
            lblErrorMessage.Text = ""

            oDetail.IDTIPOCONTATORE = IDTIPOCONTATORE
            oDetail.FondoScala = txtValoreFondoScala.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oTipoContatore.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiContatore.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipoContatore.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
