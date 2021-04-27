Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class Giri

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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(CConfiguraAddizionali))
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

        Dim IDGIRO As Integer
        Try
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")

            If Not Page.IsPostBack Then

                IDGIRO = Int32.Parse(Request.QueryString("IDGIRO"))

                If IDGIRO <> -1 Then
                    lblOperation.Text = "Dati Giri - Modifica Giro"
                Else
                    lblOperation.Text = "Dati Giri - Inserimento Giro"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())
                End If

                Dim oGiri As New TabelleDiDecodifica.DBGiri
                Dim DetailGiri As New TabelleDiDecodifica.Giri

                DetailGiri = oGiri.GetGiri(IDGIRO)

                txtDescrizione.Text = DetailGiri.Descrizione
                txtCodiceGiro.Text = DetailGiri.CodiceGiro
                txtNote.Text = DetailGiri.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.Page_Load.errore: ", ex)
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
            Dim IDGIRO As Integer

            IDGIRO = Int32.Parse(Request.QueryString("IDGIRO"))

            Dim oGiri As TabelleDiDecodifica.DBGiri = New TabelleDiDecodifica.DBGiri
            Dim oDetail As New TabelleDiDecodifica.Giri
            lblErrorMessage.Text = ""

            oDetail.IDGIRO = IDGIRO
            oDetail.CODENTE = ConstSession.IdEnte
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.CodiceGiro = txtCodiceGiro.Text
            oDetail.Note = txtNote.Text
            Try
                oGiri.SetGiri(oDetail)
                If oDetail.IDGIRO = -1 Then
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
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.btnSalva_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                sScript += "parent.location.href='SearchGiri.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.btnSalva_Click.errore: ", ex)
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
            Dim IDGIRO As Integer

            IDGIRO = Int32.Parse(Request.QueryString("IDGIRO"))
            Dim oGiri As TabelleDiDecodifica.DBGiri = New TabelleDiDecodifica.DBGiri
            Try
                oGiri.EliminaGiro(IDGIRO)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.btnCancella_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchGiri.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Giri.btnCancella_Click.errore: ", ex)
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


        sScript += "parent.location.href='SearchGiri.aspx?paginacomandi=" & Request("paginacomandi") & "';"


        RegisterScript(sScript, Me.GetType())

    End Sub
End Class
