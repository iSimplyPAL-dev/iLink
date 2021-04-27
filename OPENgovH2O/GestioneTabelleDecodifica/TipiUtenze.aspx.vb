Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class TipiUtenze

    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(TipiUtenze))
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

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""

        Dim IDTIPOUTENZA As Integer

        Try
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then

                IDTIPOUTENZA = Int32.Parse(Request.QueryString("IDTIPOUTENZA"))

                If IDTIPOUTENZA <> -1 Then
                    lblOperation.Text = "Dati Tipo Utenza - Modifica Tipo Utenza"
                Else

                    lblOperation.Text = "Dati Tipo Utenza - Inserimento Tipo Utenza"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oTipoUtenza As New TabelleDiDecodifica.DBTipiUtenza
                Dim DetailTipoUtenza As New TabelleDiDecodifica.DetailTipiUtenza

                DetailTipoUtenza = oTipoUtenza.GetTipoUtenza(IDTIPOUTENZA)

                txtDescrizione.Text = DetailTipoUtenza.Descrizione
                'txtCodiceTariffa.Text = DetailTipoUtenza.CodiceEsterno
                'txtConsumoMinimoAnnuo.Text = DetailTipoUtenza.ConsumoMinimoAnnuo
                'txtSogliaPositiva.Text = DetailTipoUtenza.SogliaPositiva
                'txtSogliaNegativa.Text = DetailTipoUtenza.SogliaNegativa

                txtNote.Text = DetailTipoUtenza.Note

                txtDal.Text = DetailTipoUtenza.Dal.ToString()
                txtAl.Text = DetailTipoUtenza.Al.ToString()


            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiUtenze.Page_Load.errore: ", ex)
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
            Dim IDTIPOUTENZA As Integer

            IDTIPOUTENZA = Int32.Parse(Request.QueryString("IDTIPOUTENZA"))

            Dim oTipoUtenza As New TabelleDiDecodifica.DBTipiUtenza
            Dim oDetail As New TabelleDiDecodifica.DetailTipiUtenza

            lblErrorMessage.Text = ""

            oDetail.IDTipiUtenza = IDTIPOUTENZA
            oDetail.Descrizione = txtDescrizione.Text
            'oDetail.CodiceEsterno = txtCodiceTariffa.Text
            'oDetail.ConsumoMinimoAnnuo = txtConsumoMinimoAnnuo.Text
            'oDetail.SogliaPositiva = txtSogliaPositiva.Text
            'oDetail.SogliaNegativa = txtSogliaNegativa.Text
            oDetail.Dal = txtDal.Text
            oDetail.Al = txtAl.Text

            oDetail.Note = txtNote.Text
            Try
                oTipoUtenza.SetTipiUtenza(oDetail, ConstSession.IdEnte)
                If oDetail.IDTipiUtenza = -1 Then
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
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiUtenze.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                sScript += "parent.location.href='SearchTipiUtenze.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiUtenze.btnSalva_Click.errore: ", ex)
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
            Dim IDTIPOUTENZA As Integer

            IDTIPOUTENZA = Int32.Parse(Request.QueryString("IDTIPOUTENZA"))
            Dim oTipoUtenza As TabelleDiDecodifica.DBTipiUtenza = New TabelleDiDecodifica.DBTipiUtenza

            Try
                oTipoUtenza.EliminaTipoUtenza(IDTIPOUTENZA)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiUtenze.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiUtenze.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiUtenze.btnCancella_Click.errore: ", ex)
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

        Dim sScript As String = ""


        sScript += "parent.location.href='SearchTipiUtenze.aspx?paginacomandi=" & Request("paginacomandi") & "';"


        RegisterScript(sScript, Me.GetType())

    End Sub
    ''' <summary>
    ''' pulsante per la modifica dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnForzaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaModifica.Click

        Dim IDTIPOUTENZA As Integer

        IDTIPOUTENZA = Int32.Parse(Request.QueryString("IDTIPOUTENZA"))

        Dim oTipoUtenza As TabelleDiDecodifica.DBTipiUtenza = New TabelleDiDecodifica.DBTipiUtenza
        Dim oDetail As New TabelleDiDecodifica.DetailTipiUtenza
        Try
            lblErrorMessage.Text = ""

            oDetail.IDTipiUtenza = IDTIPOUTENZA
            oDetail.Descrizione = txtDescrizione.Text
            'oDetail.CodiceEsterno = txtCodiceTariffa.Text
            'oDetail.ConsumoMinimoAnnuo = txtConsumoMinimoAnnuo.Text
            'oDetail.SogliaPositiva = txtSogliaPositiva.Text
            'oDetail.SogliaNegativa = txtSogliaNegativa.Text
            oDetail.Dal = txtDal.Text
            oDetail.Al = txtAl.Text

            oDetail.Note = txtNote.Text
            Try
                oTipoUtenza.UpdateForzatoTipiUtenze(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiUtenze.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiUtenze.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
