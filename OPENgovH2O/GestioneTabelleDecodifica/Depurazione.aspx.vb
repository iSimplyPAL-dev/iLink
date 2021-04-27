Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class Depurazione

    Inherits BasePage



    Private Shared Log As ILog = LogManager.GetLogger(GetType(CConfiguraAddizionali))

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


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
    Dim Generali As New Generali()

    Dim _Const As New Costanti()
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim CODDEPURAZIONE As Integer
            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")

            If Not Page.IsPostBack Then

                CODDEPURAZIONE = Int32.Parse(Request.QueryString("CODDEPURAZIONE"))

                If CODDEPURAZIONE <> -1 Then
                    lblOperation.Text = "Dati Codici Depurazione - Modifica Dati Codici Depurazione"
                Else

                    lblOperation.Text = "Dati Codici Depurazione - Inserimento Codici Depurazione"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oDepurazione As New TabelleDiDecodifica.DBDepurazione
                Dim DetailDepurazione As New TabelleDiDecodifica.DetailDepurazione

                DetailDepurazione = oDepurazione.GetDepurazione(CODDEPURAZIONE)

                txtCodiceDepurazione.Text = DetailDepurazione.CodiceDepurazione
                txtDescrizione.Text = DetailDepurazione.Descrizione
                txtNote.Text = DetailDepurazione.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Depurazione.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Try
            Dim CODDEPURAZIONE As Integer

            CODDEPURAZIONE = Int32.Parse(Request.QueryString("CODDEPURAZIONE"))

            Dim oDepurazione As TabelleDiDecodifica.DBDepurazione = New TabelleDiDecodifica.DBDepurazione
            Dim oDetail As New TabelleDiDecodifica.DetailDepurazione
            lblErrorMessage.Text = ""

            oDetail.CodDepurazione = CODDEPURAZIONE
            oDetail.CodiceDepurazione = txtCodiceDepurazione.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oDepurazione.SetDEPURAZIONE(oDetail)
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
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""
                sScript += "parent.location.href='SearchDepurazione.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Depurazione.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Try
            Dim CODDEPURAZIONE As Integer

            CODDEPURAZIONE = Int32.Parse(Request.QueryString("CODDEPURAZIONE"))
            Dim oDepurazione As TabelleDiDecodifica.DBDepurazione = New TabelleDiDecodifica.DBDepurazione
            Try
                oDepurazione.EliminaDepurazione(CODDEPURAZIONE)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Depurazione.btnCancella_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""
                sScript += "parent.location.href='SearchDepurazione.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Depurazione.btnCancella_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click

        Dim _const As New Costanti

        Dim sScript As String = ""
        sScript += "parent.location.href='SearchDepurazione.aspx?paginacomandi=" & Request("paginacomandi") & "';"
        RegisterScript(sScript, Me.GetType())

    End Sub

    Private Sub btnForzaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaModifica.Click
        Try
            Dim CODDEPURAZIONE As Integer

            CODDEPURAZIONE = Int32.Parse(Request.QueryString("CODDEPURAZIONE"))

            Dim oDepurazione As TabelleDiDecodifica.DBDepurazione = New TabelleDiDecodifica.DBDepurazione
            Dim oDetail As New TabelleDiDecodifica.DetailDepurazione
            lblErrorMessage.Text = ""

            oDetail.CodDepurazione = CODDEPURAZIONE
            oDetail.CodiceDepurazione = txtCodiceDepurazione.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oDepurazione.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Depurazione.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""
                sScript += "parent.location.href='SearchDepurazione.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Depurazione.btnForzaModifica_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
