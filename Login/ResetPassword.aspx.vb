Imports log4net
''' <summary>
''' Pagina per la gestione della password.
''' Le possibili opzioni sono:
''' - Salva
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/06/2018">
''' <strong>Adeguamento GDPR</strong>
''' <em>Cambio password</em>
''' Sarà creata una nuova voce di menù sotto Utilità -> Cambio password per permettere all'utente di cambiare la password in qualsiasi momento.
''' <em>Accettazione informativa</em>
''' Al primo cambio password, l'utente dovrà dare l’accettazione all’Informativa sul trattamento dei dati personali.
''' <em>Informativa sul trattamento dei dati personali</em>
''' Bisognerà creare la pagina per illustrare l'Informativa sul trattamento dei dati personali.
''' </revision>
''' </revisionHistory>
Public Class ResetPassword
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResetPassword))

    Private Sub ResetPassword_Init(sender As Object, e As EventArgs) Handles Me.Init
        infoEnte.InnerText = COSTANTValue.ConstSession.DescrizioneEnte
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sScript As String = ""
        Try
            If COSTANTValue.ConstSession.Profilo <> COSTANTValue.ConstSession.TipoProfilo.Amministratore Then
                sScript += "$('#txtLoginName').attr('disabled','disabled');$('#RecapNew').hide();"
                txtLoginName.Text = COSTANTValue.ConstSession.UserName
            End If
            If Not Page.IsPostBack Then
                If Not Request.Params("utente") Is Nothing Then
                    txtLoginName.Text = Request.Params("utente")
                    sScript += "$('.lstTabRow').text('Password scaduta - Immettere la nuova password');"
                Else
                    txtLoginName.Text = COSTANTValue.ConstSession.UserName
                sScript += "$('.lstTabRow').text('Immettere la nuova password');"
            End If
            If Not Session("LASTCHANGEPWD") Is Nothing Then
                If CDate(Session("LASTCHANGEPWD")).Year < DateTime.Now.Year And DateDiff(DateInterval.Day, CDate(Session("LASTCHANGEPWD")), DateTime.Now) > 90 Then
                    chkAccept.Checked = False
                Else
                    chkAccept.Checked = True
                    sScript += "$('#PrivacyPolicy').hide;"
                End If
            End If
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug("OPENgov.ResetPassword.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Dim sScript As String = ""
    '    Try
    '        If Not Request.Params("utente") Is Nothing Then
    '            txtLoginName.Text = Request.Params("utente")
    '            sScript += "$('.lstTabRow').text('Password scaduta - Immettere la nuova password');"
    '        Else
    '            txtLoginName.Text = COSTANTValue.ConstSession.UserName
    '            sScript += "$('.lstTabRow').text('Immettere la nuova password');"
    '        End If
    '        If Not Page.IsPostBack Then
    '            If Not Session("LASTCHANGEPWD") Is Nothing Then
    '                If CDate(Session("LASTCHANGEPWD")).Year < DateTime.Now.Year And DateDiff(DateInterval.Day, CDate(Session("LASTCHANGEPWD")), DateTime.Now) > 90 Then
    '                    chkAccept.Checked = False
    '                Else
    '                    chkAccept.Checked = True
    '                    sScript += "$('#PrivacyPolicy').hide;"
    '                End If
    '            End If
    '        End If
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug("OPENgov.ResetPassword.Page_Load.errore: ", ex)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdReset.Click
        Dim sScript As String = ""
        Try
            Dim IP As String = Request.UserHostAddress.ToString
            IP = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            'when user is behind proxy server
            If (IP Is Nothing) Then
                IP = Request.ServerVariables("REMOTE_ADDR")
                'Without proxy
            End If
            Log.Debug("Indirizzo IP client : " & IP)
            If chkAccept.Checked = False Then
                sScript = "GestAlert('a', 'warning', '', '', 'Per procedere bisogna accettare l’Informativa sul trattamento dei dati personali!');"
                RegisterScript(sScript, Me.GetType())
            Else
                If New UtilityOPENgov().PasswordValidator(txtPassword.Text) = False Then
                    sScript = "GestAlert('a', 'warning', '', '', 'La password impostata non rispetta le regole di sicurezza!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    If New UtilityOPENgov().PasswordIsInHistory(txtLoginName.Text, txtPassword.Text) = False Then
                        sScript = "GestAlert('a', 'danger', '', '', 'Impossibile inserire una Password gia\' utilizzata.');"
                        RegisterScript(sScript, Me.GetType())
                    Else
                        If New UtilityOPENgov().ResetPassword(txtLoginName.Text, txtPassword.Text) = False Then
                            sScript = "GestAlert('a', 'danger', '', '', 'Errore nel cambio password.');"
                            RegisterScript(sScript, Me.GetType())
                        Else
                            If COSTANTValue.ConstSession.Profilo <> COSTANTValue.ConstSession.TipoProfilo.Amministratore Then
                                If New UtilityOPENgov().LogOff(txtLoginName.Text) Then
                                    sScript = New UtilityOPENgov().CheckLogin(txtLoginName.Text, txtPassword.Text, IP, lblMessage)
                                Else
                                    sScript = "GestAlert('a', 'warning', '', '', 'Errore in logout!');"
                                End If
                                RegisterScript(sScript, Me.GetType())
                            Else
                                sScript = "GestAlert('a', 'success', '', '', 'Registrazione effettuata con successo!');"
                                RegisterScript(sScript, Me.GetType())
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug("OPENgov.ResetPassword.Reset_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdReset.Click
    '    Dim sScript As String = ""
    '    Try
    '        Dim IP As String = Request.UserHostAddress.ToString
    '        IP = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
    '        'when user is behind proxy server
    '        If (IP Is Nothing) Then
    '            IP = Request.ServerVariables("REMOTE_ADDR")
    '            'Without proxy
    '        End If
    '        Log.Debug("Indirizzo IP client : " & IP)
    '        If chkAccept.Checked = False Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Per procedere bisogna accettare l’Informativa sul trattamento dei dati personali!');"
    '            RegisterScript(sScript, Me.GetType())
    '        Else
    '            If New UtilityOPENgov().PasswordValidator(txtPassword.Text) = False Then
    '                sScript = "GestAlert('a', 'warning', '', '', 'La password impostata non rispetta le regole di sicurezza!');"
    '                RegisterScript(sScript, Me.GetType())
    '            Else
    '                If New UtilityOPENgov().ResetPassword(txtLoginName.Text, txtPassword.Text) = False Then
    '                    sScript = "GestAlert('a', 'danger', '', '', 'Errore nel cambio password.');"
    '                    RegisterScript(sScript, Me.GetType())
    '                Else
    '                    If New UtilityOPENgov().LogOff(txtLoginName.Text) Then
    '                        sScript = New UtilityOPENgov().CheckLogin(txtLoginName.Text, txtPassword.Text, IP, lblMessage)
    '                    Else
    '                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in logout!');"
    '                    End If
    '                    RegisterScript(sScript, Me.GetType())
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug("OPENgov.ResetPassword.Reset_Click.errore: ", ex)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class