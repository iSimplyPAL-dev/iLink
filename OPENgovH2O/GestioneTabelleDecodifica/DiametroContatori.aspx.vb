Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class DiametroContatori

    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(DiametroContatori))
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
        Try
            Dim sScript As String = ""

            Dim CODDIAMETROCONTATORE As Integer

            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then

                CODDIAMETROCONTATORE = Int32.Parse(Request.QueryString("CODDIAMETROCONTATORE"))

                If CODDIAMETROCONTATORE <> -1 Then
                    lblOperation.Text = "Dati Diametro Contatore - Modifica Diametro Contatore"
                Else

                    lblOperation.Text = "Dati Diametro Contatore - Inserimento Diametro Contatore"

                    sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                    sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                    RegisterScript(sScript, Me.GetType())

                End If

                Dim oDiametroContatore As New TabelleDiDecodifica.DBDiametroContatore
                Dim DetailDiametroContatore As New TabelleDiDecodifica.DiametroContatore

                DetailDiametroContatore = oDiametroContatore.GetDiametroContatore(CODDIAMETROCONTATORE)

                'LoadDropDownList(cboTariffeContatori, DetailDiametroContatore.Importo, "CODTARIFFACONTATORE", "IMPORTO", DetailDiametroContatore.CodTariffaContatore)
                txtDescrizione.Text = DetailDiametroContatore.Descrizione
                txtDiametroContatore.Text = DetailDiametroContatore.DiametroContatore
                txtNote.Text = DetailDiametroContatore.Note
                chkPrevalente.Checked = DetailDiametroContatore.Prevalente

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            Dim paginacomandi As String = Request("paginacomandi")
            Dim parametri As String
            parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.Page_Load.errore: ", ex)
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
    ''' <param name="lngSelectedID">numerico</param>
    ''' <remarks></remarks>
    Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
        Try
            Dim dt As DataTable = dsTemp.Tables(0)
            Dim rowNull As DataRow = dt.NewRow()
            rowNull(DataTextField) = 0
            rowNull(DataValueField) = "-1"
            dsTemp.Tables(0).Rows.InsertAt(rowNull, 0)

            cboTemp.DataSource = dsTemp
            cboTemp.DataValueField = DataValueField
            cboTemp.DataTextField = DataTextField
            cboTemp.DataBind()

            If lngSelectedID <> -1 Then
                cboTemp.SelectedIndex = lngSelectedID
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.LoadDropDownList.errore: ", ex)
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
            Dim CODDIAMETROCONTATORE As Integer

            CODDIAMETROCONTATORE = Int32.Parse(Request.QueryString("CODDIAMETROCONTATORE"))

            Dim oDiametroContatore As TabelleDiDecodifica.DBDiametroContatore = New TabelleDiDecodifica.DBDiametroContatore
            Dim oDetail As New TabelleDiDecodifica.DiametroContatore
            lblErrorMessage.Text = ""

            oDetail.CodDiametroContatore = CODDIAMETROCONTATORE
            oDetail.CodTariffaContatore = 0
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.DiametroContatore = txtDiametroContatore.Text
            oDetail.Note = txtNote.Text
            oDetail.Prevalente = chkPrevalente.Checked
            oDetail.CodiceISTAT = ConstSession.CodIstat

            Try
                oDiametroContatore.SetDiametroContatore(oDetail)
                If oDetail.CodDiametroContatore = -1 Then
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
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnSalva_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                sScript += "parent.location.href='SearchDiametroContatori.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnSalva_Click.errore: ", ex)
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
            Dim CODDIAMETROCONTATORE As Integer

            CODDIAMETROCONTATORE = Int32.Parse(Request.QueryString("CODDIAMETROCONTATORE"))
            Dim oDiametroContatore As TabelleDiDecodifica.DBDiametroContatore = New TabelleDiDecodifica.DBDiametroContatore
            Try
                oDiametroContatore.EliminaDiametroContatore(CODDIAMETROCONTATORE)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnCancella_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""


                sScript += "parent.location.href='SearchDiametroContatori.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())

            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnCancella_Click.errore: ", ex)
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


        sScript += "parent.location.href='SearchDiametroContatori.aspx?paginacomandi=" & Request("paginacomandi") & "';"


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
            Dim CODDIAMETROCONTATORE As Integer

            CODDIAMETROCONTATORE = Int32.Parse(Request.QueryString("CODDIAMETROCONTATORE"))

            Dim oDiametroContatore As TabelleDiDecodifica.DBDiametroContatore = New TabelleDiDecodifica.DBDiametroContatore
            Dim oDetail As New TabelleDiDecodifica.DiametroContatore
            lblErrorMessage.Text = ""

            oDetail.CodDiametroContatore = CODDIAMETROCONTATORE
            oDetail.CodTariffaContatore = 0 'cboTariffeContatori.SelectedItem.Value
            oDetail.Descrizione = txtDescrizione.Text
            oDetail.DiametroContatore = txtDiametroContatore.Text
            oDetail.Note = txtNote.Text
            oDetail.Prevalente = chkPrevalente.Checked
            oDetail.CodiceISTAT = ConstSession.CodIstat

            Try
                oDiametroContatore.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then

                Dim sScript As String = ""


                sScript += "parent.location.href='SearchDiametroContatori.aspx?paginacomandi=" & Request("paginacomandi") & "';"


                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DiametroContatori.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
