Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione delle tessere.
''' Le possibili opzioni sono:
''' - Cambia Tessera
''' - Modifica
''' - Elimina Tessera
''' - Salva
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiGestTessere
    Inherits BaseEnte

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

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiGestTessere))
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim MyFunction As New generalClass.generalFunction

        lblTitolo.Text = ConstSession.DescrizioneEnte
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI " + info.InnerText
            Else
                info.InnerText = "TARSU " + info.InnerText
            End If
            'controllo se devo abilitare i pulsanti di modifica/eliminazione
            If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
                ReDim Preserve oListCmd(1)
                oListCmd(0) = "Modifica"
                oListCmd(1) = "Delete"
                sScript += "$('#" + oListCmd(0).ToString() + "').addClass('DisableBtn');"
                sScript += "$('#" + oListCmd(1).ToString() + "').addClass('DisableBtn');"
                'sScript = MyFunction.PopolaJSdisabilita(oListCmd)
                RegisterScript(sScript, Me.GetType)
            Else
                ReDim Preserve oListCmd(0)
                oListCmd(0) = "Salva"
                sScript += "$('#" + oListCmd(0).ToString() + "').addClass('DisableBtn');"
                'sScript = MyFunction.PopolaJSdisabilita(oListCmd)
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiGestTessere.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sScript As String
    '    Dim oListCmd() As Object
    '    Dim MyFunction As New generalClass.generalFunction

    '    lblTitolo.Text = ConstSession.DescrizioneEnte
    '    Try
    '        If ConstSession.IsFromTARES = "1" Then
    '            info.InnerText = "TARI " + info.InnerText
    '        Else
    '            info.InnerText = "TARSU " + info.InnerText
    '        End If
    '        'controllo se devo abilitare i pulsanti di modifica/eliminazione
    '        If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
    '            ReDim Preserve oListCmd(1)
    '            oListCmd(0) = "Modifica"
    '            oListCmd(1) = "Delete"
    '            sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '            RegisterScript(sScript, Me.GetType)
    '        Else
    '            ReDim Preserve oListCmd(0)
    '            oListCmd(0) = "Salva"
    '            sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiGestTessere.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub
End Class
