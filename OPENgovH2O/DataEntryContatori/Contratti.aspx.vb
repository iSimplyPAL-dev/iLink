Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Partial Class Contratti
    Inherits BasePage
    Dim _Const As New Costanti()
    Dim ContrattoID As Integer

    Private Shared Log As ILog = LogManager.GetLogger(GetType(Contratti))


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
    '*******************************************************
    '
    ' l'evento  Page_Load di questa pagina
    ' ricava il dettaglio del Contatore selezionato
    ' L'estrazione dati avviene nella classe  DEContatori.vb
    '
    '*******************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ContrattoID = CInt(Request.Params("idcontratto"))
        Dim DBContratti As DBContratti = New DBContratti()
        Dim DetailContratto As DetailsContratti = DBContratti.GetDetailsContratti(ContrattoID)

        Try
            If ContrattoID > 0 Then
                Dim sScript As String = ""
                sScript = "document.getElementById('hdVirtualIDContratto').value='" & DetailContratto.lngVirtualIDContratto & "';"
                RegisterScript(sScript, Me.GetType())
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
                If Not Page.IsPostBack Then
                    'LoadDropDownList(cboTipoUtenze, DetailContratto.dsTipoUtenza, "IDTIPOUTENZA", "DESCRIZIONE", DetailContratto.lngTipoUtenza)
                    'LoadDropDownList(cboDiametroContatore, DetailContratto.dsDiametroContatore, "CODDIAMETROCONTATORE", "DESCRIZIONE", DetailContratto.lngDiametroContatore)
                    'LoadDropDownList(cboDiametroPresa, DetailContratto.dsDiametroPresa, "CODDIAMETROPRESA", "DESCRIZIONE", DetailContratto.lngDiametroPresa)
                    'FillDropDownSQL(cboDiametroContatore, DetailContratto.drDiametroContatore, DetailContratto.lngDiametroContatore)
                    'FillDropDownSQL(cboDiametroPresa, DetailContratto.drDiametroPresa, DetailContratto.lngDiametroPresa)

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
                    txtCodiceContratto.Text = DetailContratto.CodiceContratto
                    txtDataSottoscrizione.Text = DetailContratto.DataSottoscrizione
                    'txtNumeroUtenze.Text = DetailContratto.NumeroUtenze

                End If
            Else
                If Not Page.IsPostBack Then
                    Dim sScript As String = ""
                    sScript = "document.getElementById('hdVirtualIDContratto').value='" & -1 & "';"
                    RegisterScript(sScript, Me.GetType())

                    'LoadDropDownList(cboTipoUtenze, DetailContratto.dsTipoUtenza, "IDTIPOUTENZA", "DESCRIZIONE", -1)
                    '	LoadDropDownList(cboDiametroContatore, DetailContratto.dsDiametroContatore, "CODDIAMETROCONTATORE", "DESCRIZIONE", -1)
                    '	LoadDropDownList(cboDiametroPresa, DetailContratto.dsDiametroPresa, "CODDIAMETROPRESA", "DESCRIZIONE", -1)
                    'FillDropDownSQL(cboDiametroContatore, DetailContratto.drDiametroContatore, -1)
                    '	FillDropDownSQL(cboDiametroPresa, DetailContratto.drDiametroPresa, -1)

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

                    txtCodiceContratto.Text = Request.Params("hdCodiceContratto")
                    txtDataSottoscrizione.Text = Request.Params("hdDataSottoScrizione")
                    'txtNumeroUtenze.Text = Request.Params("hdNumeroUtenzeContratto")
                    'cboTipoUtenze.SelectedIndex = CInt(Request.Params("hdTipoUtenzaContratto"))
                    'cboDiametroContatore.SelectedIndex = CInt(Request.Params("hdIdDiametroContatoreContratto"))
                    'cboDiametroPresa.SelectedIndex = CInt(Request.Params("hdIdDiametroPresaContratto"))
                End If

            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Contratti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '
    ' LoadDropDownList
    ' Carica i controlli DropDownList con una storedProcedure 
    ' 
    'L'estrazione dati avviene nella classe  DEContatori.vb
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
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

            If lngSelectedID <> -1 Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Contratti.LoadDropDownList.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Private Sub btnEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEvento.Click

        Dim hdVirtualIDContratto As Integer = CInt(Request.Params("hdVirtualIDContratto"))
        Dim sScript As String = ""
        sScript += "parent.opener.parent.Visualizza.frmDatigenerali.hdCodiceContratto.value='" & txtCodiceContratto.Text & "';"
        sScript += "partent.opener.parent.Visualizza.frmDatigenerali.hdDataSottoScrizione.value='" & txtDataSottoscrizione.Text & "';"
        'sscript+="parent.opener.parent.Visualizza.frmDatigenerali.hdTipoUtenzaContratto.value='" & cboTipoUtenze.SelectedItem.Value & "';")
        'sscript+="parent.opener.parent.Visualizza.frmDatigenerali.hdIdDiametroContatoreContratto.value='" & cboDiametroContatore.SelectedItem.Value & "';")
        'sscript+="parent.opener.parent.Visualizza.frmDatigenerali.hdIdDiametroPresaContratto.value='" & cboDiametroPresa.SelectedItem.Value & "';")
        'sscript+="parent.opener.parent.Visualizza.frmDatigenerali.hdNumeroUtenzeContratto.value='" & txtNumeroUtenze.Text & "';")
        sScript += "parent.opener.parent.Visualizza.frmDatigenerali.hdVirtualIDContratto.value='" & hdVirtualIDContratto & "';"
        sScript += "parent.window.close();"
        RegisterScript(sScript, Me.GetType())
    End Sub
    Private Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)

        Dim blnFindElement As Boolean = False
        Dim intCount As Integer = 1
        Dim intNumberElements As Integer = cboTemp.Items.Count
        Try
            Do While intCount < intNumberElements
                cboTemp.SelectedIndex = intCount
                If cboTemp.SelectedItem.Value = strValue Then
                    cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
                    blnFindElement = True
                    Exit Do
                End If
                intCount = intCount + 1
            Loop
            If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Contratti.SelectIndexDropDownList.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Sub FillDropDownSQL(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long)

        Try
            Dim myListItem As ListItem
            myListItem = New ListItem()
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem()

                myListItem.Text = dr.GetString(1) & "--" & "[" & dr.GetString(2) & "]"
                myListItem.Value = dr.GetInt32(0)
                cboTemp.Items.Add(myListItem)

            End While

            If lngSelectedID <> -1 Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If


        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Contratti.FillDropDownSQL.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")

        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try

    End Sub
End Class
