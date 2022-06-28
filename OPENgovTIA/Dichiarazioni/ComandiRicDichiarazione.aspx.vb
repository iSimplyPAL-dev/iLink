Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione .
''' Le possibili opzioni sono:
''' - Aggiornamento Massivo
''' - Visualizza GIS
''' - Stampa Dichiarazioni Sintetica
''' - Stampa Dichiarazioni Analitica
''' - Elenco Dichiarazioni
''' - Inserimento nuova denuncia
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiRicDichiarazione
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicDichiarazione))
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
    ''' <revisionHistory>
    ''' <revision date="23/09/2014">
    ''' GIS
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' Funzioni Sovracomunali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""

        lblTitolo.Text = ConstSession.DescrizioneEnte
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            If Not Request.Item("IsFromVariabile") Is Nothing Then
                If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                    info.InnerText += "Variabile"
                End If
            End If
            info.InnerText += " - Dichiarazioni - Ricerca"
            If ConstSession.VisualGIS = False Then
                sScript += "$('#GIS').addClass('DisableBtn');"
                sScript += "$('#GIS').addClass('hidden');"
                RegisterScript(sScript, Me.GetType)
            End If
            If ConstSession.IsolaEcologicaPathFile = "" Then
                sScript += "$('#IsolaEco').addClass('DisableBtn');"
                sScript += "$('#IsolaEco').addClass('hidden');"
                RegisterScript(sScript, Me.GetType)
            End If
            If ConstSession.IdEnte = "" Then
                Dim oListCmd() As Object
                ReDim Preserve oListCmd(3)
                oListCmd(0) = "AggMassivo"
                oListCmd(1) = "GIS"
                oListCmd(2) = "NewInsert"
                oListCmd(3) = "IsolaEco"
                sScript = ""
                For x As Integer = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
                Next
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiRicDichiarazione.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sScript As String

    '    lblTitolo.Text = ConstSession.DescrizioneEnte
    '    Try
    '        If ConstSession.IsFromTARES = "1" Then
    '            info.InnerText = "TARI "
    '        Else
    '            info.InnerText = "TARSU "
    '        End If
    '        If Not Request.Item("IsFromVariabile") Is Nothing Then
    '            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
    '                info.InnerText += "Variabile"
    '            End If
    '        End If
    '        info.InnerText += " - Dichiarazioni - Ricerca"
    '        '*** 20140923 - GIS ***
    '        If ConstSession.VisualGIS = False Then
    '            sScript = "document.getElementById('GIS').style.display='none';"
    '            RegisterScript( sScript,Me.GetType)
    '        End If
    '        '*** ***
    '        '*** 201511 - Funzioni Sovracomunali ***
    '        If ConstSession.IdEnte = "" Then
    '            Dim oListCmd() As Object
    '            ReDim Preserve oListCmd(2)
    '            oListCmd(0) = "AggMassivo"
    '            oListCmd(1) = "GIS"
    '            oListCmd(2) = "NewInsert"
    '            sScript = ""
    '            For i As Integer = 0 To oListCmd.Length - 1
    '                sScript += "document.getElementById('" & oListCmd(i).ToString() & "').style.display='none';"
    '            Next
    '            RegisterScript( sScript,Me.GetType)
    '        End If
    '        '*** ***
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiRicDichiarazione.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub
End Class
