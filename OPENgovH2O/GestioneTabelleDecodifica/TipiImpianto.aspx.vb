Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class TipiImpianto

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

    Private Shared Log As ILog = LogManager.GetLogger(GetType(TipiImpianto))

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""

        Dim IDIMPIANTO As Integer

        Try
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then

                IDIMPIANTO = Int32.Parse(Request.QueryString("IDIMPIANTO"))

                If IDIMPIANTO <> -1 Then
                    lblOperation.Text = "Dati Tipi Impianto - Modifica Tipi Impianto"
                Else

                    lblOperation.Text = "Inserimento Tipi Impianto"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oTipiImpianto As New TabelleDiDecodifica.DBImpianti
                Dim DetailTipiImpianto As New TabelleDiDecodifica.Impianti

                DetailTipiImpianto = oTipiImpianto.GetImpianti(IDIMPIANTO)

                txtCodiceImpianto.Text = DetailTipiImpianto.CodiceImpianto
                txtDescrizione.Text = DetailTipiImpianto.Descrizione
                txtNote.Text = DetailTipiImpianto.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.Page_Load.errore: ", ex)
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
        Try
            Dim IDIMPIANTO As Integer

            IDIMPIANTO = Int32.Parse(Request.QueryString("IDIMPIANTO"))

            Dim oTipiImpianto As TabelleDiDecodifica.DBImpianti = New TabelleDiDecodifica.DBImpianti
            Dim oDetail As New TabelleDiDecodifica.Impianti
            lblErrorMessage.Text = ""

            oDetail.IDImpianto = IDIMPIANTO
            oDetail.CodiceImpianto = txtCodiceImpianto.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oTipiImpianto.SetImpianto(oDetail)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        Dim sScript As String = ""
                        sScript += "Aggiorna();"
                        RegisterScript(sScript, Me.GetType())
                    Case Else
                        lblErrorMessage.Text = ex.Message
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiImpianto.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.btnSalva_Click.errore: ", ex)
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
            Dim IDIMPIANTO As Integer

            IDIMPIANTO = Int32.Parse(Request.QueryString("IDIMPIANTO"))
            Dim oTipiImpianto As TabelleDiDecodifica.DBImpianti = New TabelleDiDecodifica.DBImpianti
            Try
                oTipiImpianto.EliminaImpianto(IDIMPIANTO)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiImpianto.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.btnCancella_Click.errore: ", ex)
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


        sScript += "parent.location.href='SearchTipiImpianto.aspx?paginacomandi=" & Request("paginacomandi") & "';"


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
            Dim IDIMPIANTO As Integer

            IDIMPIANTO = Int32.Parse(Request.QueryString("IDIMPIANTO"))

            Dim oTipiImpianto As TabelleDiDecodifica.DBImpianti = New TabelleDiDecodifica.DBImpianti
            Dim oDetail As New TabelleDiDecodifica.Impianti
            lblErrorMessage.Text = ""

            oDetail.IDImpianto = IDIMPIANTO
            oDetail.CodiceImpianto = txtCodiceImpianto.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oTipiImpianto.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchTipiImpianto.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.TipiImpianto.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
