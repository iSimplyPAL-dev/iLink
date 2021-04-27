Imports log4net

Partial Class RicercaDatiMancanti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("RicercaDatiMancanti")
    Public Structure ObjParamRicMancanti
        Dim sAnno As String
        Dim sCognome As String
        Dim sNome As String
        Dim nTipoRicerca As Integer
    End Structure

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            'Se non sto ricaricando la pagina da postback:
            If Page.IsPostBack = False Then
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
                Dim paginacomandi As String = "ComandiRicDatiMancanti.aspx"
                Dim parametri As String = "?"

                'parametri = "?title=Acquedotto - Agenzia Entrate - Dati Mancanti&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

                Dim sScript As String = ""
                sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript , Me.GetType())
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
                Dim FncGenerale As New ClsAEDatiMancanti
                'Dim WFSessione As OPENUtility.CreateSessione
                'Dim WFErrore As String
                Dim oMyParam As ObjParamRicMancanti

                'popolo combo Anno
                FncGenerale.LoadComboAnnoPeriodi(DdlAnno, ConstSession.IdEnte)
                If Not Session("ParamSearchDatiMancanti") Is Nothing Then
                    oMyParam = CType(Session("ParamSearchDatiMancanti"), ObjParamRicMancanti)
                    If oMyParam.sAnno <> "" Then
                        DdlAnno.SelectedValue = oMyParam.sAnno
                    End If
                    If oMyParam.sCognome <> "" Then
                        TxtCognome.Text = oMyParam.sCognome
                    End If
                    If oMyParam.sNome <> "" Then
                        TxtNome.Text = oMyParam.sNome
                    End If
                    Select Case oMyParam.nTipoRicerca
                        Case 1
                            OptDatiAnagrafici.Checked = True : OptDatiImmobile.Checked = False : OptEntrambi.Checked = False
                        Case 2
                            OptDatiAnagrafici.Checked = False : OptDatiImmobile.Checked = True : OptEntrambi.Checked = False
                        Case 3
                            OptDatiAnagrafici.Checked = False : OptDatiImmobile.Checked = False : OptEntrambi.Checked = True
                    End Select
                    sScript = "Search()"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaDatiMancanti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sNameXLS, sScript As String
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim x, nCol As Integer
        Dim oListDatiMancanti() As ObjAEDatiMancanti
        Dim FncDatiMancanti As New ClsAEDatiMancanti

        nCol = 18
        'Se ho la sessione di risultato ricerca valorizzata:
        If Not Session("ResultRicercaDatiMancanti") Is Nothing Then
            oListDatiMancanti = CType(Session("ResultRicercaDatiMancanti"), ObjAEDatiMancanti())
            'valorizzo il nome e percorso del file di stampa;
            sNameXLS = ConstSession.IdEnte & "_ELENCO_DATIMANCANTIAGENZIAENTRATE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'popolo il datatable di stampa tramite la funzione PrintDatiMancanti(oListDatiMancanti);
            DtDatiStampa = FncDatiMancanti.PrintDatiMancanti(oListDatiMancanti)
            If Not DtDatiStampa Is Nothing Then
                'se ho il datatable popolato:
                'definisco l'arraylist di colonne;
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                'Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18}
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto in excel tramite RKLib.ExportData.Export("Web")
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        Else
            sScript = "GestAlert('a', 'warning', '', '', 'Effettuare la ricerca prima di eseguire la stampa!');"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
End Class
