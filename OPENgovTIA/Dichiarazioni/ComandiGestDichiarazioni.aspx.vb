Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione delle dichiarazioni.
''' Le possibili opzioni sono:
''' - Visualizza GIS
''' - Stampa ricevuta dichiarazione
''' - Calcolo Ruolo
''' - Modifica dichiarazione
''' - Elimina dichiarazione
''' - Salva
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiGestDichiarazioni
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiGestDichiarazioni))
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
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String
        'Dim oListCmd() As Object
        'Dim MyFunction As New generalClass.generalFunction

        lblTitolo.Text = ConstSession.DescrizioneEnte
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            If ConstSession.IsFromVariabile = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Dichiarazioni - Gestione"
            ''controllo se devo abilitare i pulsanti di modifica/eliminazione
            'If Request.Item("sProvenienza") = "N" Then
            '    ReDim Preserve oListCmd(1)
            '    oListCmd(0) = "Modifica"
            '    oListCmd(1) = "Delete"
            '    sScript = MyFunction.PopolaJSdisabilita(oListCmd)
            '    RegisterScript(me.gettype(),"load", sScript)
            'Else
            '    ReDim Preserve oListCmd(0)
            '    oListCmd(0) = "Salva"
            '    sScript = MyFunction.PopolaJSdisabilita(oListCmd)
            '    RegisterScript(me.gettype(),"load", sScript)
            '    sScript = "document.getElementById(Of'Calcolo').style.display='';"
                    '    RegisterScript(me.gettype(),"loadCalcolo", "<script language='javascript'>" & sScript & "</script>")
                    'End If
                    '*** 20140923 - GIS ***
                    If ConstSession.VisualGIS = False Then
                sScript = "document.getElementById('GIS').style.display='none';"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiGestDichiarazioni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
