Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class Minimi

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

    Private Shared Log As ILog = LogManager.GetLogger(GetType(Minimi))

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""

        Try
            Dim IDMINIMO As Integer
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")

            If Not Page.IsPostBack Then

                IDMINIMO = Int32.Parse(Request.QueryString("IDMINIMO"))

                If IDMINIMO <> -1 Then
                    lblOperation.Text = "Modifica Minimo"
                Else

                    lblOperation.Text = "Inserimento Minimo"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oMinimo As New TabelleDiDecodifica.DBMinimiFatturabili()
                Dim DetailMinimo As New TabelleDiDecodifica.MinimiFatturabili()

                DetailMinimo = oMinimo.GetMinimiFattiurabile(IDMINIMO)

                LoadDropDownList(cboTariffaUtilizzo, DetailMinimo.dsTipoUtenza, "IDTIPOUTENZA", "DESCRIZIONE", DetailMinimo.TipoUtenza)
                txtDescrizione.Text = DetailMinimo.Descrizione
                txtMinimo.Text = DetailMinimo.Minimo
                txtNote.Text = DetailMinimo.Note

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' caricamento combo
    ''' </summary>
    ''' <param name="cboTemp">DropDownList</param>
    ''' <param name="dsTemp">DataSet</param>
    ''' <param name="DataValueField">stringa</param>
    ''' <param name="DataTextField">stringa</param>
    ''' <param name="strSelectedValue">stringa</param>
    ''' <remarks></remarks>
    Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal strSelectedValue As String)
        Try
            Dim dt As DataTable = dsTemp.Tables(0)
            Dim rowNull As DataRow = dt.NewRow()
            rowNull(DataTextField) = "..."
            rowNull(DataValueField) = "-1"
            dsTemp.Tables(0).Rows.InsertAt(rowNull, 0)

            cboTemp.DataSource = dsTemp
            cboTemp.DataValueField = DataValueField
            cboTemp.DataTextField = DataTextField
            cboTemp.DataBind()


            Dim blnFindElement As Boolean = False
            Dim intCount As Integer = 1
            Dim intNumberElements As Integer = cboTemp.Items.Count
            Do While intCount < intNumberElements
                cboTemp.SelectedIndex = intCount
                If cboTemp.SelectedItem.Value = strSelectedValue Then
                    cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
                    blnFindElement = True
                    Exit Do
                End If
                intCount = intCount + 1
            Loop
            If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.LoadDropDownList.errore: ", ex)
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
            Dim IDMINIMO As Integer

            IDMINIMO = Int32.Parse(Request.QueryString("IDMINIMO"))

            Dim oMinimo As TabelleDiDecodifica.DBMinimiFatturabili = New TabelleDiDecodifica.DBMinimiFatturabili()
            Dim oDetail As New TabelleDiDecodifica.MinimiFatturabili()
            lblErrorMessage.Text = ""

            oDetail.IDMinimo = IDMINIMO
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.TipoUtenza = cboTariffaUtilizzo.SelectedItem.Value
            oDetail.Minimo = txtMinimo.Text
            oDetail.Note = txtNote.Text

            Try
                oMinimo.SetMinimiFatturabili(oDetail)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        Dim sScript As String = ""
                        sScript += "Aggiorna();"
                        RegisterScript(sScript, Me.GetType())
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnSalva_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchMinimo.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnSalva_Click.errore: ", ex)
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
            Dim IDMINIMO As Integer

            IDMINIMO = Int32.Parse(Request.QueryString("IDMINIMO"))
            Dim oMinimo As TabelleDiDecodifica.DBMinimiFatturabili = New TabelleDiDecodifica.DBMinimiFatturabili()
            Try
                oMinimo.EliminaMinimoFatturabile(IDMINIMO)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnCancella_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnCancella_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchMinimo.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnCancella_Click.errore: ", ex)
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


        sScript += "parent.location.href='SearchMinimo.aspx?paginacomandi=" & Request("paginacomandi") & "';"


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
            Dim IDMINIMO As Integer

            IDMINIMO = Int32.Parse(Request.QueryString("IDMINIMO"))

            Dim oMinimo As TabelleDiDecodifica.DBMinimiFatturabili = New TabelleDiDecodifica.DBMinimiFatturabili()
            Dim oDetail As New TabelleDiDecodifica.MinimiFatturabili()
            lblErrorMessage.Text = ""

            oDetail.IDMinimo = IDMINIMO
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.TipoUtenza = cboTariffaUtilizzo.SelectedItem.Value
            oDetail.Minimo = txtMinimo.Text
            oDetail.Note = txtNote.Text

            Try
                oMinimo.UpdateForzatoMinimoFatturabile(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchMinimo.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Minimi.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
