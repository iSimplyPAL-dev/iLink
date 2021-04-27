Imports log4net

Partial Class RicercaAnalisiEconomiche
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaAnalisiEconomiche))

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
        dim sSQL as string
        Dim oLoadCombo As New ClsGenerale.Generale

        Try
            If Page.IsPostBack = False Then
                'carico gli anni
                sSQL = "SELECT DISTINCT TP_FATTURE_NOTE.ANNO_RIFERIMENTO, TP_FATTURE_NOTE.ANNO_RIFERIMENTO AS ANNO"
                sSQL += " FROM TP_FATTURE_NOTE"
                sSQL += " INNER JOIN TP_FATTURAZIONI_GENERATE ON TP_FATTURE_NOTE.IDFLUSSO=TP_FATTURAZIONI_GENERATE.ID_FLUSSO"
                sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL)"
                sSQL += " AND (TP_FATTURAZIONI_GENERATE.IDENTE='" & ConstSession.IdEnte & "')"
                sSQL += " AND (NOT TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI IS NULL AND TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI<>'')"
                sSQL += " ORDER BY TP_FATTURE_NOTE.ANNO_RIFERIMENTO"
                oLoadCombo.LoadComboGenerale(DdlAnno, sSQL)
                'carico i periodi
                sSQL = "SELECT PERIODO, PERIODO AS DESCR"
                sSQL += " FROM TP_PERIODO "
                sSQL += " WHERE (COD_ENTE='" & ConstSession.IdEnte & "')"
				sSQL += " ORDER BY PERIODO"
                oLoadCombo.LoadComboGenerale(DdlPeriodo, sSQL)
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaAnalisiEconomiche.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
