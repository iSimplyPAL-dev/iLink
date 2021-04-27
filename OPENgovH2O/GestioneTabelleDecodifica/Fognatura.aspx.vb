Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class Fognatura

    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Fognatura))



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
        Dim sScript As String = ""
        Try
            Dim CODFOGNATURA As Integer

            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then

                CODFOGNATURA = Int32.Parse(Request.QueryString("CODFOGNATURA"))

                If CODFOGNATURA <> -1 Then
                    lblOperation.Text = "Dati Codici Fognatura - Modifica Codici Fognatura"
                Else

                    lblOperation.Text = "Dati Codici Fognatura - Inserimento Codici Fognatura"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oFognatura As New TabelleDiDecodifica.DBFognatura
                Dim DetailFognatura As New TabelleDiDecodifica.DetailFognatura

                DetailFognatura = oFognatura.GetFognatura(CODFOGNATURA)

                txtCodiceFognatura.Text = DetailFognatura.CodiceFognatura
                txtDescrizione.Text = DetailFognatura.Descrizione
                txtNote.Text = DetailFognatura.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Try
            Dim CODFOGNATURA As Integer

            CODFOGNATURA = Int32.Parse(Request.QueryString("CODFOGNATURA"))

            Dim oFognatura As TabelleDiDecodifica.DBFognatura = New TabelleDiDecodifica.DBFognatura
            Dim oDetail As New TabelleDiDecodifica.DetailFognatura
            lblErrorMessage.Text = ""

            oDetail.CodFognatura = CODFOGNATURA
            oDetail.CodiceFognatura = txtCodiceFognatura.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oFognatura.SetFOGNATURA(oDetail)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        Dim sScript As String = ""
                        sScript += "Aggiorna();"
                        RegisterScript(sScript, Me.GetType())
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnSalva_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchFognatura.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Try
            Dim CODFOGNATURA As Integer

            CODFOGNATURA = Int32.Parse(Request.QueryString("CODFOGNATURA"))
            Dim oFognatura As TabelleDiDecodifica.DBFognatura = New TabelleDiDecodifica.DBFognatura
            Try
                oFognatura.EliminaFognatura(CODFOGNATURA)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnCancella_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchFognatura.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnCancella_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click

        Dim _const As New Costanti

        Dim sScript As String = ""


        sScript += "parent.location.href='SearchFognatura.aspx?paginacomandi=" & Request("paginacomandi") & "';"


        RegisterScript(sScript, Me.GetType())

    End Sub

    Private Sub btnForzaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaModifica.Click
        Try
            Dim CODFOGNATURA As Integer

            CODFOGNATURA = Int32.Parse(Request.QueryString("CODFOGNATURA"))

            Dim oFognatura As TabelleDiDecodifica.DBFognatura = New TabelleDiDecodifica.DBFognatura
            Dim oDetail As New TabelleDiDecodifica.DetailFognatura
            lblErrorMessage.Text = ""

            oDetail.CodFognatura = CODFOGNATURA
            oDetail.CodiceFognatura = txtCodiceFognatura.Text
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.Note = txtNote.Text
            Try
                oFognatura.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchFognatura.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fognatura.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


End Class
