Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Anagrafica.DLL
Imports AnagInterface
Imports log4net

Namespace AnagrafeAnater
    Partial Class BasePageAnater
        Inherits System.Web.UI.Page
        Private Const CONCURRENCY_STAMP As String = "ConcurrencyStamp"
        Private Const CONCURRENCY_OBJECT As String = "ConcurrencyObject"

        Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePageAnater))
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

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        End Sub

        Protected Sub SetConcurrencyObject(ByVal obj As DettaglioAnagrafica)

            ViewState.Add(CONCURRENCY_OBJECT, obj)
        End Sub

        Protected Function GetConcurrencyObject() As DettaglioAnagrafica
            Return CType(ViewState(CONCURRENCY_OBJECT), DettaglioAnagrafica)
        End Function

        '        Protected Function ShowConcurrencyFields(ByVal controls As Hashtable, ByRef operatore As String) As Boolean
        '            Dim Cod_ContribuenteFiglio As Integer
        '            Dim ID_DataAnagraficaFiglio As Integer
        '            ShowConcurrencyFields = False
        '            Dim userObject As DettaglioAnagrafica = CType(ViewState(CONCURRENCY_OBJECT), DettaglioAnagrafica)
        '            Dim TYPE As Type = userObject.GetType

        '            'workflow
        '            'Dim WFErrore As String
        '            'Dim WFSessione As New OPENUtility.CreateSessione(Session("PARAMETROENV"), Session("username"), Session("IDENTIFICATIVOAPPLICAZIONE"))
        '            'If Not WFSessione.CreaSessione(Session("username"), WFErrore) Then
        '            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
        '            'End If
        '            'workflow

        '            'Dim oAnagrafica As New GestioneAnagrafica(WFSessione.oSession, Session("ANAGRAFICA"))
        '            Dim oAnagrafica As New GestioneAnagrafica()

        '            Dim dbObject As DettaglioAnagrafica = CType(TYPE.Assembly.CreateInstance(TYPE.FullName), DettaglioAnagrafica)

        '            oAnagrafica.GetAnagraficaConcurrency(userObject.COD_CONTRIBUENTE, userObject.ID_DATA_ANAGRAFICA, Cod_ContribuenteFiglio)
        'Try
        '            'If Cod_ContribuenteFiglio = 0 And ID_DataAnagraficaFiglio = 0 Then
        '            '    dbObject = oAnagrafica.GetAnagrafica(userObject.COD_CONTRIBUENTE, userObject.CodTributo)
        '            'Else
        '            '    dbObject = oAnagrafica.GetAnagrafica(Cod_ContribuenteFiglio, userObject.CodTributo)
        '            'End If
        '            If Cod_ContribuenteFiglio = 0 And ID_DataAnagraficaFiglio = 0 Then
        '                dbObject = oAnagrafica.GetAnagrafica(userObject.COD_CONTRIBUENTE, userObject.CodTributo, COSTANTValue.Costanti.INIT_VALUE_NUMBER, COSTANTValue.ConstSession.StringConnectionAnagrafica)
        '            Else
        '                dbObject = oAnagrafica.GetAnagrafica(Cod_ContribuenteFiglio, userObject.CodTributo, COSTANTValue.Costanti.INIT_VALUE_NUMBER, COSTANTValue.ConstSession.StringConnectionAnagrafica)
        '            End If

        '            operatore = dbObject.Operatore

        '            If Not dbObject.dsContatti Is userObject.dsContatti Then
        '                Dim grd As System.Web.UI.WebControls.DataGrid
        '                grd = controls("Contatti")
        '                grd.DataKeyField = "IDRIFERIMENTO"
        '                grd.DataSource = dbObject.dsContatti
        '                grd.DataBind()
        '            End If

        '            Dim differences As IList

        '            differences = DLL.ObjectDifference.GetDifferences(dbObject, userObject)

        '            ClearPageConcurrency(controls)

        '            Dim diff As DLL.ObjectDifference
        '            For Each diff In differences

        '                Dim ctrl As WebControl = CType(controls(diff.PropertyName), WebControl)
        '                Dim txtBox As System.Web.UI.WebControls.TextBox
        '                Dim cmbBox As System.Web.UI.WebControls.DropDownList

        '                If Not (ctrl Is Nothing) Then

        '                    ctrl.BackColor = Color.LightGray

        '                    If TypeOf ctrl Is TextBox Then
        '                        txtBox = ctrl
        '                        If Not (txtBox Is Nothing) Then
        '                            ShowConcurrencyFields = True
        '                            txtBox.Text = diff.FirstValue.ToString()
        '                            GoTo ContinueForEach1
        '                        End If
        '                    End If

        '                    If TypeOf ctrl Is DropDownList Then
        '                        cmbBox = ctrl
        '                        If Not (cmbBox Is Nothing) Then
        '                            ShowConcurrencyFields = True
        '                            SelectIndexDropDownList(cmbBox, diff.FirstValue.ToString())
        '                            GoTo ContinueForEach1
        '                        End If
        '                    End If
        '                End If

        'ContinueForEach1:
        '            Next diff
        ' Catch ex As Exception
        '  Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.BasePageAnater.ShowConcurrencyFields.errore: ", ex)
        '  Response.Redirect("../../../PaginaErrore.aspx")
        ' End Try
        '        End Function
        Public Function ShowConcurrencyFields(ByVal controls As Hashtable, ByRef operatore As String, DBType As String, ByVal StringConnection As String) As Boolean
            Dim Cod_ContribuenteFiglio As Integer
            Dim ID_DataAnagraficaFiglio As Integer
            ShowConcurrencyFields = False
            Dim userObject As DettaglioAnagrafica = CType(ViewState(CONCURRENCY_OBJECT), DettaglioAnagrafica)
            Dim TYPE As Type = userObject.GetType
            Dim oAnagrafica As New GestioneAnagrafica()

            Dim dbObject As DettaglioAnagrafica = CType(TYPE.Assembly.CreateInstance(TYPE.FullName), DettaglioAnagrafica)
            Try
                'oAnagrafica.WF_GetAnagraficaConcurrency(userObject.COD_CONTRIBUENTE, userObject.ID_DATA_ANAGRAFICA, Cod_ContribuenteFiglio)
                If Cod_ContribuenteFiglio = 0 And ID_DataAnagraficaFiglio = 0 Then
                    dbObject = oAnagrafica.GetAnagrafica(userObject.COD_CONTRIBUENTE, Utility.Costanti.INIT_VALUE_NUMBER, "", COSTANTValue.ConstSession.DBType, StringConnection, False) ', userObject.CodTributo
                Else
                    dbObject = oAnagrafica.GetAnagrafica(Cod_ContribuenteFiglio, Utility.Costanti.INIT_VALUE_NUMBER, "", COSTANTValue.ConstSession.DBType, StringConnection, False) ', userObject.CodTributo
                End If
                operatore = dbObject.Operatore

                If Not dbObject.dsContatti Is userObject.dsContatti Then
                    Dim grd As System.Web.UI.WebControls.DataGrid
                    grd = controls("Contatti")
                    grd.DataKeyField = "IDRIFERIMENTO"
                    grd.DataSource = dbObject.dsContatti
                    grd.DataBind()
                End If

                Dim differences As IList

                differences = ObjectDifference.GetDifferences(dbObject, userObject)

                ClearPageConcurrency(controls)

                Dim diff As ObjectDifference
                For Each diff In differences

                    Dim ctrl As WebControl = CType(controls(diff.PropertyName), WebControl)
                    Dim txtBox As System.Web.UI.WebControls.TextBox
                    Dim cmbBox As System.Web.UI.WebControls.DropDownList

                    If Not (ctrl Is Nothing) Then

                        ctrl.BackColor = Color.LightGray

                        If TypeOf ctrl Is TextBox Then
                            txtBox = ctrl
                            If Not (txtBox Is Nothing) Then
                                ShowConcurrencyFields = True
                                txtBox.Text = diff.FirstValue.ToString()
                                GoTo ContinueForEach1
                            End If
                        End If

                        If TypeOf ctrl Is DropDownList Then
                            cmbBox = ctrl
                            If Not (cmbBox Is Nothing) Then
                                ShowConcurrencyFields = True
                                SelectIndexDropDownList(cmbBox, diff.FirstValue.ToString())
                                GoTo ContinueForEach1
                            End If
                        End If
                    End If
ContinueForEach1:
                Next diff
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.BasePageAnater.ShowConcurrencyFields.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End Try
        End Function
        Private Sub ClearPageConcurrency(ByVal controls As Hashtable)
            Try
                Dim obj As Object
                For Each obj In controls.Values
                    Dim ctrl As WebControl = obj
                    If Not (ctrl Is Nothing) Then
                        ctrl.BackColor = Color.White
                    End If
                Next obj
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.BasePageAnater.ClearPageConcurrency.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End Try
        End Sub

        Public Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)

            Dim blnFindElement As Boolean = False
            Dim intCount As Integer = 1
            Dim intNumberElements As Integer = cboTemp.Items.Count
            Try
                Do While intCount < intNumberElements
                    cboTemp.SelectedIndex = intCount
                    If UCase(cboTemp.SelectedItem.Value) = UCase(strValue) Then
                        cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
                        blnFindElement = True
                        Exit Do
                    End If
                    intCount = intCount + 1
                Loop
                If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.BasePageAnater.SelectIndexDropDownList.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            End Try
        End Sub

        Protected Function DataView(ByVal DataSet As DataSet) As DataView

            Dim Table As DataTable = DataSet.Tables("CONTATTI")
            Dim View As DataView = Table.DefaultView
            View.Sort = "TipoRiferimento"
            Return View

        End Function
    End Class

End Namespace