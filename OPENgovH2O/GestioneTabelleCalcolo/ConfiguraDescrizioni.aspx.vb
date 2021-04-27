Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class ConfiguraDescrizioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfiguraDescrizioni))
    Public Codice, Descrizione, Tabella, Operazione As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ''' <summary>
    ''' Pagina per l'inserimento/modifica/cancellazione delle tipologie addizionali e canoni
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            If Request.Item("Codice") <> "" Then
                Codice = Request.Item("Codice").Replace("*pl*", "+")
            End If
            If Request.Item("Descrizione") <> "" Then
                Descrizione = Request.Item("Descrizione").Replace("*pl*", "+")
            End If
            Tabella = Request.Item("Tabella")
            Operazione = Request.Item("Operazione")
            txtTabella.Style.Add("display", "none")
            txtTabella.Text = Tabella
            If Not Page.IsPostBack Then
                If IsNothing(Operazione) = True Then
                    'lblDescrizioneOperazione.Text = "Dati " + CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "") + " - Inserimento " + CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
                    lblDescrizioneOperazione.Text = "Dati - Inserimento "
                Else
                    'lblDescrizioneOperazione.Text = "Dati " + CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "") + " - Modifica " + CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
                    lblDescrizioneOperazione.Text = "Dati - Modifica "
                End If
                If Request.Item("Codice") <> "" Then
                    txtCodice.Text = Request.Item("Codice").Replace("*pl*", "+")
                End If
                If Request.Item("Descrizione") <> "" Then
                    txtDescrizione.Text = Request.Item("Descrizione").Replace("*pl*", "+")
                End If
                '*** 20141125 - Componente aggiuntiva sui consumi ***
                If Not Request.Item("Applica") Is Nothing Then
                    Select Case Request.Item("Applica")
                        Case OggettoCanone.Canone_H2O
                            optH2O.Checked = True : divApplicaA.Style.Add("display", "")
                        Case OggettoCanone.Canone_Depurazione
                            optDep.Checked = True : divApplicaA.Style.Add("display", "")
                        Case OggettoCanone.Canone_Fognatura
                            optFog.Checked = True : divApplicaA.Style.Add("display", "")
                        Case Else
                            divApplicaA.Style.Add("display", "none")
                    End Select
                Else
                    If Tabella.IndexOf("CANONI") <= 0 Then
                        divApplicaA.Style.Add("display", "none")
                    End If
                End If
                '*** ***
            End If

            Dim sScript As String
            sScript = "Setfocus(document.getElementById('txtDescrizioneCat'));"
            RegisterScript(sScript , Me.GetType())

            ''carico array degli oggetti
            ''e li disabilito se i diritti dell'operatore sono in sola lettura
            ''Dim sArray() As Object

            ''ReDim Preserve sArray(2)

            ''sArray(0) = "txtCodiceCat"
            ''sArray(1) = "txtDescrizioneCat"
            ''sArray(2) = "txtDataInizio"

            'Dim ClsGenerale As New ClsGenerale.Generale

            'If Session("dirittioperatore") = "LETTURA" Then
            '    str = ClsGenerale.PopolaJSdisabilita(sArray)
            '    RegisterScript(sScript , Me.GetType())"load", str)
            'End If

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraDescrizioni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per il salvataggio dei dati della videata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory><revision date="25/11/2014">Componente aggiuntiva sui consumi</revision></revisionHistory>
    Private Sub BtnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSalva.Click
        Dim ClsGenerale As New ClsGenerale.Generale
        Dim sScript As String
        Dim strErrore As String = ""
        Dim nApplicaA As Integer = OggettoCanone.Canone_H2O

        Try
            If optFog.Checked Then
                nApplicaA = OggettoCanone.Canone_Fognatura
            ElseIf optDep.Checked Then
                nApplicaA = OggettoCanone.Canone_Depurazione
            End If
            If Operazione = "modifica" Then
                If ClsGenerale.SetDescrizioni(txtCodice.Text, txtDescrizione.Text, Tabella, nApplicaA, strErrore) = True Then
                    sScript = "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente!'); parent.Search();"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                If ClsGenerale.SetDescrizioni(-1, txtDescrizione.Text, Tabella, nApplicaA, strErrore) = True Then
                    sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!'); parent.Search();"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ConfiguraDescrizioni.BtnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' pulsante per la cancellazione dell'oggetto selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnElimina.Click
        Dim ClsGenerale As New ClsGenerale.Generale
        Dim sScript As String
        Dim strErrore As String=""
        Try
            If ClsGenerale.DeleteDescrizioni(txtCodice.Text, Tabella, strErrore) = True Then
                sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!'); parent.Search();"
                RegisterScript(sScript, Me.GetType())
                sScript = "UscitaDopoOperazione();"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                RegisterScript(sScript, Me.GetType())
                sScript = "UscitaDopoOperazione();"
                RegisterScript(sScript , Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraDescrizioni.BtnElimina_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub
End Class
