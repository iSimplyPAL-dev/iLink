Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class Periodo
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Periodo))
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

    Dim Generali As New Generali
    Dim _Const As New Costanti

    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        Dim CODPERIODO As Integer

        Try
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            RegisterScript(sScript, Me.GetType())
            btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
            If Not Page.IsPostBack Then
                If Not Request.QueryString("CODPERIODO") Is Nothing Then
                    CODPERIODO = Utility.StringOperation.FormatInt(Request.QueryString("CODPERIODO"))
                    If CODPERIODO <> -1 Then
                        lblOperation.Text = "Dati Periodo - Modifica Periodo"
                    Else
                        lblOperation.Text = "Dati Periodo - Inserimento Periodo"

                        sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
                        sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
                        RegisterScript(sScript, Me.GetType())
                    End If

                    Dim oPeriodo As New TabelleDiDecodifica.DBPeriodo
                    Dim DetailPeriodo As New TabelleDiDecodifica.DetailPeriodo

                    DetailPeriodo = oPeriodo.GetPeriodo(CODPERIODO)

                    txtPerido.Text = DetailPeriodo.Periodo
                    txtDataA.Text = DetailPeriodo.AData
                    txtDaData.Text = DetailPeriodo.DaData
                    txtNote.Text = DetailPeriodo.Note
                    chkStorico.Checked = DetailPeriodo.Storico
                    chkAttuale.Checked = DetailPeriodo.Attuale
                    Select Case DetailPeriodo.nTipoArrotondamentoConsumo
                        Case 0
                            OptNoArrotond.Checked = True : OptArrotondMatematico.Checked = False : OptArrotondEccesso.Checked = False
                        Case 1
                            OptNoArrotond.Checked = False : OptArrotondMatematico.Checked = True : OptArrotondEccesso.Checked = False
                        Case 2
                            OptNoArrotond.Checked = False : OptArrotondMatematico.Checked = False : OptArrotondEccesso.Checked = True
                    End Select
                    sScript += "document.getElementById('paginacomandi').value='" & Utility.StringOperation.FormatString(Request("paginacomandi")) & "?title=Periodo';"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim sScript As String = ""
    '    Dim CODPERIODO As Integer

    '    Try
    '        sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
    '        RegisterScript(sScript, Me.GetType())
    '        btnSalva.Attributes.Add("OnClick", "return VerificaCampi();")
    '        If Not Page.IsPostBack Then
    '            CODPERIODO = Int32.Parse(Request.QueryString("CODPERIODO"))
    '            If CODPERIODO <> -1 Then
    '                lblOperation.Text = "Dati Periodo - Modifica Periodo"
    '            Else
    '                lblOperation.Text = "Dati Periodo - Inserimento Periodo"

    '                sScript = "parent.parent.Comandi.document.getElementById('CANCELLA').disabled=true;"
    '                sScript += "parent.parent.Comandi.document.getElementById('CANCELLA').title='';"
    '                RegisterScript(sScript, Me.GetType())
    '            End If

    '            Dim oPeriodo As New TabelleDiDecodifica.DBPeriodo
    '            Dim DetailPeriodo As New TabelleDiDecodifica.DetailPeriodo

    '            DetailPeriodo = oPeriodo.GetPeriodo(CODPERIODO)

    '            txtPerido.Text = DetailPeriodo.Periodo
    '            txtDataA.Text = DetailPeriodo.AData
    '            txtDaData.Text = DetailPeriodo.DaData
    '            txtNote.Text = DetailPeriodo.Note
    '            chkStorico.Checked = DetailPeriodo.Storico
    '            chkAttuale.Checked = DetailPeriodo.Attuale
    '            Select Case DetailPeriodo.nTipoArrotondamentoConsumo
    '                Case 0
    '                    OptNoArrotond.Checked = True : OptArrotondMatematico.Checked = False : OptArrotondEccesso.Checked = False
    '                Case 1
    '                    OptNoArrotond.Checked = False : OptArrotondMatematico.Checked = True : OptArrotondEccesso.Checked = False
    '                Case 2
    '                    OptNoArrotond.Checked = False : OptArrotondMatematico.Checked = False : OptArrotondEccesso.Checked = True
    '            End Select
    '        End If
    '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

    '        Dim paginacomandi As String = Request("paginacomandi")
    '        Dim parametri As String
    '        parametri = "?title=Dizionario - " & ConstSession.DescrTipoProcServ & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&PAG_PREC=STRADE"

    '        sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' pulsante per il salvataggio dei dati della videata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click

        Dim CODPERIODO As Integer
        Dim sMessaggio As String = ""
        Dim sScript As String = ""
        Dim ModDate As New ClsGenerale.Generale
        Try
            'controllo se è stato selezionato il periodo attuale o storico
            If chkAttuale.Checked = False And chkStorico.Checked = False Then
                sScript = ""
                sScript = sScript & "GestAlert('a', 'warning', '', '', 'E\' necessario specificare se il periodo è attuale oppure storico!');"
                sScript = sScript & ""

                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            If chkAttuale.Checked = True And chkStorico.Checked = True Then
                sScript = ""
                sScript = sScript & "GestAlert('a', 'warning', '', '', 'Verificare, non è possibile che un periodo sia attuale e storico!');"
                sScript = sScript & ""

                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            If txtDaData.Text = "" Or txtDataA.Text = "" Then
                sScript = ""
                sScript = sScript & "GestAlert('a', 'warning', '', '', 'Verificare. E\' necessario specificare la data di inizio e di fine periodo!');"
                sScript = sScript & ""

                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            If ModDate.GiraData(txtDaData.Text) >= ModDate.GiraData(txtDataA.Text) Then
                sScript = ""
                sScript = sScript & "GestAlert('a', 'warning', '', '', 'Verificare. La data di inizio periodo deve essere inferiore alla data di fine periodo!');"
                sScript = sScript & ""

                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            CODPERIODO = Utility.StringOperation.FormatInt(Request.QueryString("CODPERIODO"))

            Dim oPeriodo As TabelleDiDecodifica.DBPeriodo = New TabelleDiDecodifica.DBPeriodo
            Dim oDetail As New TabelleDiDecodifica.DetailPeriodo
            lblErrorMessage.Text = ""

            oDetail.CodPeriodo = CODPERIODO
            oDetail.Periodo = txtPerido.Text
            oDetail.DaData = txtDaData.Text
            oDetail.AData = txtDataA.Text
            oDetail.Storico = chkStorico.Checked
            oDetail.Attuale = chkAttuale.Checked
            oDetail.Note = txtNote.Text
            If OptArrotondEccesso.Checked = True Then
                oDetail.nTipoArrotondamentoConsumo = 2
            ElseIf OptNoArrotond.Checked = True Then
                oDetail.nTipoArrotondamentoConsumo = 0
            Else
                oDetail.nTipoArrotondamentoConsumo = 1
            End If

            Try
                oPeriodo.SetPERIODO(oDetail, sScript)
                If oDetail.CodPeriodo = -1 Then
                    sScript += "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente');"
                Else
                    sScript += "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente');"
                End If
                If sScript <> "" Then
                    RegisterScript(sScript, Me.GetType())
                    Session("PERIODO") = ""
                    Session("PERIODOID") = ""
                Else
                    Session("PERIODO") = oDetail.Periodo
                    Session("PERIODOID") = oDetail.CodPeriodo.ToString
                End If

            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        sScript = "Aggiorna();"
                        RegisterScript(sScript, Me.GetType())
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnSalva_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnSalva_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                sScript += "parent.location.href='SearchPeriodo.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnSalva_Click.errore: ", ex)
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
            Dim CODPERIODO As Integer = Utility.StringOperation.FormatInt(Request.QueryString("CODPERIODO"))
            Dim oPeriodo As TabelleDiDecodifica.DBPeriodo = New TabelleDiDecodifica.DBPeriodo

            Try
                oPeriodo.EliminaPERIODO(CODPERIODO)
            Catch ex As SqlException
                Select Case ex.Number
                    Case 547
                        lblErrorMessage.Text = "L'elemento che si desidera eliminare è usato da altre tabelle.Impossibile la cancellazione!"
                    Case Else
                        lblErrorMessage.Text = ex.Message
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnCancella_Click.errore: ", ex)
                End Select
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnCancella_Click.errore: ", ex)
            End Try
            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""
                sScript += "parent.location.href='SearchPeriodo.aspx?paginacomandi=" & Request("paginacomandi") & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnCancella_Click.errore: ", ex)
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
        sScript += "parent.location.href='SearchPeriodo.aspx?paginacomandi=" & Request("paginacomandi") & "';"
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
            Dim CODPERIODO As Integer = Utility.StringOperation.FormatInt(Request.QueryString("CODPERIODO"))

            Dim oPeriodo As TabelleDiDecodifica.DBPeriodo = New TabelleDiDecodifica.DBPeriodo
            Dim oDetail As New TabelleDiDecodifica.DetailPeriodo
            lblErrorMessage.Text = ""

            oDetail.CodPeriodo = CODPERIODO
            oDetail.Periodo = txtPerido.Text
            oDetail.DaData = txtDaData.Text
            oDetail.AData = txtDataA.Text
            oDetail.Storico = chkStorico.Checked
            oDetail.Attuale = chkAttuale.Checked
            oDetail.Note = txtNote.Text
            If OptArrotondEccesso.Checked = True Then
                oDetail.nTipoArrotondamentoConsumo = 2
            ElseIf OptNoArrotond.Checked = True Then
                oDetail.nTipoArrotondamentoConsumo = 0
            Else
                oDetail.nTipoArrotondamentoConsumo = 1
            End If

            Try
                oPeriodo.UpdateForzato(oDetail)
            Catch ex As SqlException
                lblErrorMessage.Text = ex.Message
            Catch ex As Exception
                lblErrorMessage.Text = ex.Message
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnForzaModifica_Click.errore: ", ex)
            End Try

            If Len(lblErrorMessage.Text) = 0 Then
                Dim sScript As String = ""

                sScript += "parent.location.href='SearchPeriodo.aspx?paginacomandi=" & Request("paginacomandi") & "';"

                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Periodo.btnForzaModifica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub chkAttuale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAttuale.CheckedChanged

    '    If chkAttuale.Checked = True Then
    '        chkStorico.Checked = False
    '    Else
    '        chkStorico.Checked = True
    '    End If

    'End Sub

    'Private Sub chkStorico_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStorico.CheckedChanged

    '    If chkStorico.Checked = True Then
    '        chkAttuale.Checked = False
    '    Else
    '        chkAttuale.Checked = True
    '    End If

    'End Sub
End Class
