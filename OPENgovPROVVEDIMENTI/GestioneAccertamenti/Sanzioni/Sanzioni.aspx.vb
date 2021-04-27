Imports System.Threading
'Imports ComPlusInterface.ProvvedimentiTarsu.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports log4net
Imports ComPlusInterface
''' <summary>
''' Pagina per la gestione delle sanzioni in generazione provvedimento.
''' Contiene i parametri di gestione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class Sanzioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Sanzioni))
   

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents ChkIntGlob As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private idLegame As Integer
    'Private IdSanzioni() As Integer
    'Private IdSanzioni() As String
    Private strSanzioni As String
    Private anno As String
    Private bloccaCheck As String
    Public id_provvedimento As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            anno = Request.Item("anno")
            strSanzioni = Request.Item("strSanzioni")
            bloccaCheck = Request.Item("bloccaCheck")
            id_provvedimento = Request.Item("id_provvedimento")
            If Not Page.IsPostBack Then
                ViewState.Add("idGrid", "")

                If Trim(Request.Item("idLegame")) <> "" Then
                    ViewState.Add("idLegame", Request.Item("idLegame"))
                Else
                    ViewState.Add("idLegame", " ")
                End If

                System.Threading.Thread.Sleep(300)
                If StrComp(bloccaCheck, "1") = 0 Then
                    Dim sScript As String = ""
                    sScript += "parent.Comandi.location.href='ComandiGestioneSanzioni2.aspx'" & vbCrLf
                    RegisterScript(sScript, Me.GetType())
                Else
                    Dim sScript As String = ""
                    sScript += "parent.Comandi.location.href='ComandiGestioneSanzioni.aspx'" & vbCrLf
                    RegisterScript(sScript, Me.GetType())
                End If

                Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB

                Dim objDS As DataSet
                If Session("DataSetSanzioni") Is Nothing Then
                    '*** 20130308 - devo prelevare anche la motivazione ***
                    If id_provvedimento <> "" Then
                        objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, id_provvedimento)
                    Else
                        objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, "")
                    End If
                    Session("DataSetSanzioni") = objDS
                    '*** ***
                Else
                    objDS = CType(Session("DataSetSanzioni"), DataSet)
                End If
                If Not IsNothing(objDS) Then
                    If objDS.Tables.Count > 0 Then
                        If objDS.Tables(0).Rows.Count > 0 Then
                            GrdSanzioni.DataSource = objDS
                            GrdSanzioni.DataBind()
                            GrdSanzioni.Visible = True
                            lblSanzioni.Visible = False
                        Else
                            GrdSanzioni.Visible = False
                            lblSanzioni.Text = "Non è stata trovata alcuna sanzione configurata."
                            lblSanzioni.Visible = True
                        End If
                        chkCalcolaInteressi.Checked = GetChkCalcolaInteressi(ViewState("idLegame"))
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IDLEGAME"></param>
    ''' <returns></returns>
    Private Function GetChkCalcolaInteressi(ByVal IDLEGAME As String) As Boolean
        Dim blnRetVal As Boolean = False
        Dim intTARSU As Integer
        Try
            If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_ICI Then
                Dim ListAcc() As ComPlusInterface.objUIICIAccert
                ListAcc = CType(Session("DataTableImmobiliDaAccertare"), ComPlusInterface.objUIICIAccert())
                For Each myAcc As ComPlusInterface.objUIICIAccert In ListAcc
                    If myAcc.IdLegame = IDLEGAME Then
                        If myAcc.CalcolaInteressi Then
                            blnRetVal = True
                        End If
                    End If
                Next
            ElseIf ConstSession.CodTributo = Utility.Costanti.TRIBUTO_TARSU Then
                '*** 20140701 - IMU/TARES ***
                Dim workTableTARSU As ObjArticoloAccertamento()
                workTableTARSU = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                '*** ***
                For intTARSU = 0 To workTableTARSU.Length - 1
                    If workTableTARSU(intTARSU).IdLegame = IDLEGAME Then
                        If workTableTARSU(intTARSU).Calcola_Interessi = True Then
                            blnRetVal = True
                        End If
                    End If
                Next
                '*** 20130801 - accertamento OSAP ***
            ElseIf ConstSession.CodTributo = Utility.Costanti.TRIBUTO_OSAP Then
                Dim workTableOSAP As ComPlusInterface.OSAPAccertamentoArticolo()
                workTableOSAP = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())
                For intTARSU = 0 To workTableOSAP.Length - 1
                    If workTableOSAP(intTARSU).IdLegame = IDLEGAME Then
                        If workTableOSAP(intTARSU).Calcola_Interessi = True Then
                            blnRetVal = True
                        End If
                    End If
                Next
                '*** ***
            End If
            Return blnRetVal
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.GetChkCalcolaInteressi.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function

    '*** 20140701 - IMU/TARES ***
    'Private Sub btnAggiornaGriglia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAggiornaGriglia.Click
    '    Dim workTable As New DataTable("IMMOBILI")
    '    Dim workTableImmobiliManuali As New DataTable("IMMOBILIMANUALI")
    '    'Dim oAccertatiGriglia As OggettoArticoloRuoloAccertamento()
    '    Dim rowsArray() As DataRow
    '    Dim intAccertatiGriglia As Integer
    '    Dim bInteressi As Boolean
    'Try
    '    If CONSTSESSION.CODTRIBUTO = "8852" Then
    '        If Not Session("DataTableImmobili") Is Nothing Then
    '            workTable = CType(Session("DataTableImmobili"), DataTable)
    '            If Not ViewState("idLegame") Is Nothing Then
    '                idLegame = ViewState("idLegame")
    '            Else
    '                idLegame = " "
    '            End If

    '            bInteressi = False
    '            If ChkIntGlobal.Checked = True Then
    '                'aggiorno il flag calcolainteressi per immobile
    '                rowsArray = workTable.Select("1=1")
    '                bInteressi = True
    '            Else
    '                If chkCalcolaInteressi.Checked = True Then
    '                    bInteressi = True
    '                End If
    '                'aggiorno il flag calcolainteressi per ogni gruppo di immobili
    '                rowsArray = workTable.Select("IDLEGAME='" & idLegame & "'")
    '            End If
    '            Dim k As Integer
    '            For k = 0 To rowsArray.Length - 1
    '                rowsArray(k)("CALCOLA_INTERESSI") = bInteressi
    '                rowsArray(k).AcceptChanges()
    '            Next
    '            workTable.AcceptChanges()

    '            '#####################################################################################
    '            '''alep 14022008
    '            '''rowsArray = workTable.Select("PROGRESSIVO='" & idLegame & "'")
    '            If ChkSanzGlobal.Checked = False Then
    '                'aggiorno il flag calcolasanzioni per ogni gruppo di immobili
    '                rowsArray = workTable.Select("IDLEGAME='" & idLegame & "'")
    '            Else
    '                'aggiorno il flag calcolasanzioni per immobile
    '                rowsArray = workTable.Select("1=1")
    '            End If

    '            creaArrayIdSanzioni(grdsanzioni.Rows.Count)

    '            Dim i As Integer
    '            Dim x As Integer

    '            If Not IdSanzioni Is Nothing Then
    '                If UBound(IdSanzioni) >= 0 Then
    '                    For x = 0 To rowsArray.Length - 1
    '                        rowsArray(x).Item("IDSanzioni") = ""
    '                        rowsArray(x).AcceptChanges()
    '                    Next
    '                End If

    '                For x = 0 To rowsArray.Length - 1
    '                    For i = 0 To UBound(IdSanzioni)
    '                        rowsArray(x).Item("IDSanzioni") = rowsArray(x).Item("IDSanzioni") & IdSanzioni(i) & ","
    '                        'rowsArray(x).AcceptChanges()
    '                    Next
    '                Next

    '                If UBound(IdSanzioni) >= 0 Then
    '                    For x = 0 To rowsArray.Length - 1
    '                        rowsArray(x).Item("IDSanzioni") = Left(rowsArray(x).Item("IDSanzioni"), Len(rowsArray(x).Item("IDSanzioni")) - 1)
    '                        rowsArray(x).AcceptChanges()
    '                    Next
    '                End If

    '                If UBound(IdSanzioni) < 0 Then
    '                    For x = 0 To rowsArray.Length - 1
    '                        rowsArray(x).Item("IDSanzioni") = "-1"
    '                        rowsArray(x).AcceptChanges()
    '                    Next
    '                End If
    '            Else

    '                For x = 0 To rowsArray.Length - 1
    '                    rowsArray(x).Item("IDSanzioni") = "-1"
    '                    rowsArray(x).AcceptChanges()
    '                Next
    '            End If

    '            workTable.AcceptChanges()

    '            Session("DataTableImmobili") = workTable

    '            dim sScript as string=""
    '            sscript+= "parent.parent.opener.location.href='../grdAccertato.aspx';" & vbCrLf
    '            sscript+= "parent.window.close();"

    '            RegisterScript(sScript , Me.GetType())
    '        End If

    '    ElseIf CONSTSESSION.CODTRIBUTO = "0434" Then
    '        Dim oAccertatiGriglia As OggettoArticoloRuolo()

    '        If Not Session("oAccertatiGriglia") Is Nothing Then
    '            If Not ViewState("idLegame") Is Nothing Then
    '                idLegame = ViewState("idLegame")
    '            Else
    '                idLegame = ""
    '            End If

    '            oAccertatiGriglia = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
    '            creaArrayIdSanzioni(grdsanzioni.Rows.Count)
    '            For intAccertatiGriglia = 0 To oAccertatiGriglia.Length - 1
    '                If oAccertatiGriglia(intAccertatiGriglia).IdLegame = idLegame Or ChkSanzGlobal.Checked = True Then
    '                    Dim i As Integer
    '                    If UBound(IdSanzioni) >= 0 Then
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = ""
    '                    End If
    '                    For i = 0 To UBound(IdSanzioni)
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = oAccertatiGriglia(intAccertatiGriglia).Sanzioni & IdSanzioni(i) & ","
    '                    Next

    '                    If UBound(IdSanzioni) >= 0 Then
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = Left(oAccertatiGriglia(intAccertatiGriglia).Sanzioni, Len(oAccertatiGriglia(intAccertatiGriglia).Sanzioni) - 1)
    '                    End If

    '                    If UBound(IdSanzioni) < 0 Then
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
    '                    End If
    '                    'aggiorno il flag calcolainteressi per ogno gruppo di immobili########################
    '                    oAccertatiGriglia(intAccertatiGriglia).Calcola_Interessi = chkCalcolaInteressi.Checked
    '                End If
    '            Next

    '            Session("oAccertatiGriglia") = oAccertatiGriglia

    '            dim sScript as string=""
    '            sscript+= "parent.parent.opener.location.href='../../GestioneAccertamentiTARSU/SearchAccertatiTARSU.aspx';" & vbCrLf
    '            sscript+= "parent.window.close();"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '        '*** 20130801 - accertamento OSAP ***
    '    ElseIf CONSTSESSION.CODTRIBUTO = Costanti.TRIBUTO_OSAP Then
    '        Dim oAccertatiGriglia() As ComPlusInterface.OSAPAccertamentoArticolo
    '        If Not Session("oAccertatiGriglia") Is Nothing Then
    '            If Not ViewState("idLegame") Is Nothing Then
    '                idLegame = ViewState("idLegame")
    '            Else
    '                idLegame = ""
    '            End If

    '            oAccertatiGriglia = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())

    '            creaArrayIdSanzioni(grdsanzioni.Rows.Count)

    '            For intAccertatiGriglia = 0 To oAccertatiGriglia.Length - 1
    '                If oAccertatiGriglia(intAccertatiGriglia).IdLegame = idLegame Or ChkSanzGlobal.Checked = True Then
    '                    Dim i As Integer
    '                    If UBound(IdSanzioni) >= 0 Then
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = ""
    '                    End If
    '                    For i = 0 To UBound(IdSanzioni)
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = oAccertatiGriglia(intAccertatiGriglia).Sanzioni & IdSanzioni(i) & ","
    '                    Next

    '                    If UBound(IdSanzioni) >= 0 Then
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = Left(oAccertatiGriglia(intAccertatiGriglia).Sanzioni, Len(oAccertatiGriglia(intAccertatiGriglia).Sanzioni) - 1)
    '                    End If

    '                    If UBound(IdSanzioni) < 0 Then
    '                        oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
    '                    End If
    '                    'aggiorno il flag calcolainteressi per ogno gruppo di immobili########################
    '                    oAccertatiGriglia(intAccertatiGriglia).Calcola_Interessi = chkCalcolaInteressi.Checked
    '                End If
    '            Next

    '            Session("oAccertatiGriglia") = oAccertatiGriglia

    '            dim sScript as string=""
    '            sscript+= "parent.parent.opener.location.href='../../GestioneAccertamentiOSAP/SearchDatiAccertatoOSAP.aspx';" & vbCrLf
    '            sscript+= "parent.window.close();"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '        '*** ***
    '    End If
    'Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.btnAggiornaGriglia_Click.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAggiornaGriglia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAggiornaGriglia.Click
        Dim intAccertatiGriglia As Integer
        Dim IdSanzioni() As String

        Try
            If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_ICI Or ConstSession.CodTributo = Utility.Costanti.TRIBUTO_TASI Then
                If Not ViewState("idLegame") Is Nothing Then
                    idLegame = ViewState("idLegame")
                Else
                    idLegame = " "
                End If
                IdSanzioni = creaArrayIdSanzioni()
                If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                    Dim ListAcc() As ComPlusInterface.objUIICIAccert
                    ListAcc = CType(Session("DataTableImmobiliDaAccertare"), ComPlusInterface.objUIICIAccert())
                    For Each myAcc As ComPlusInterface.objUIICIAccert In ListAcc
                        If chkCalcolaInteressi.Checked = True Then
                            myAcc.CalcolaInteressi = True
                        Else
                            myAcc.CalcolaInteressi = False
                        End If
                        If ChkSanzGlobal.Checked = False Then
                            If myAcc.IdLegame = idLegame Then
                                For i As Integer = 0 To UBound(IdSanzioni)
                                    myAcc.IdSanzioni += IdSanzioni(i) & ","
                                Next
                            End If
                        Else
                            For i As Integer = 0 To UBound(IdSanzioni)
                                myAcc.IdSanzioni += IdSanzioni(i) & ","
                            Next
                        End If
                    Next
                    Session("DataTableImmobiliDaAccertare") = ListAcc

                    Dim sScript As String = ""
                    If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_TASI Then
                        sScript += "parent.parent.opener.location.href='../../GestioneAccertamentiTASI/SearchDatiAccertato.aspx';"
                    Else
                        sScript += "parent.parent.opener.location.href='../grdAccertato.aspx';"
                    End If
                    sScript += "parent.window.close();"
                    RegisterScript(sScript, Me.GetType())
                End If

            ElseIf ConstSession.CodTributo = Utility.Costanti.TRIBUTO_TARSU Then
                Dim oAccertatiGriglia() As ObjArticoloAccertamento

                If Not Session("oAccertatiGriglia") Is Nothing Then
                    If Not ViewState("idLegame") Is Nothing Then
                        idLegame = ViewState("idLegame")
                    Else
                        idLegame = ""
                    End If

                    oAccertatiGriglia = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                    IdSanzioni = creaArrayIdSanzioni()
                    For intAccertatiGriglia = 0 To oAccertatiGriglia.Length - 1
                        If oAccertatiGriglia(intAccertatiGriglia).IdLegame = idLegame Or ChkSanzGlobal.Checked = True Then
                            Dim i As Integer
                            If IdSanzioni Is Nothing Then
                                oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
                            Else
                                If UBound(IdSanzioni) >= 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = ""
                                End If
                                For i = 0 To UBound(IdSanzioni)
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = oAccertatiGriglia(intAccertatiGriglia).Sanzioni & IdSanzioni(i) & ","
                                    oAccertatiGriglia(intAccertatiGriglia).sDescrSanzioni = txtMotivazioni.Text
                                Next

                                If UBound(IdSanzioni) >= 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = Left(oAccertatiGriglia(intAccertatiGriglia).Sanzioni, Len(oAccertatiGriglia(intAccertatiGriglia).Sanzioni) - 1)
                                End If

                                If UBound(IdSanzioni) < 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
                                End If
                            End If
                            'aggiorno il flag calcolainteressi per ogno gruppo di immobili########################
                            oAccertatiGriglia(intAccertatiGriglia).Calcola_Interessi = chkCalcolaInteressi.Checked
                        End If
                    Next
                    Session("oAccertatiGriglia") = oAccertatiGriglia

                    If Not Session("oAccertatiDaDichiarazione") Is Nothing And ChkSanzGlobal.Checked = True Then
                        oAccertatiGriglia = CType(Session("oAccertatiDaDichiarazione"), ObjArticoloAccertamento())
                        IdSanzioni = creaArrayIdSanzioni()
                        For intAccertatiGriglia = 0 To oAccertatiGriglia.Length - 1
                            Dim i As Integer
                            If IdSanzioni Is Nothing Then
                                oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
                            Else
                                If UBound(IdSanzioni) >= 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = ""
                                End If
                                For i = 0 To UBound(IdSanzioni)
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = oAccertatiGriglia(intAccertatiGriglia).Sanzioni & IdSanzioni(i) & ","
                                    oAccertatiGriglia(intAccertatiGriglia).sDescrSanzioni = txtMotivazioni.Text
                                Next

                                If UBound(IdSanzioni) >= 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = Left(oAccertatiGriglia(intAccertatiGriglia).Sanzioni, Len(oAccertatiGriglia(intAccertatiGriglia).Sanzioni) - 1)
                                End If

                                If UBound(IdSanzioni) < 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
                                End If
                            End If
                            'aggiorno il flag calcolainteressi per ogno gruppo di immobili########################
                            oAccertatiGriglia(intAccertatiGriglia).Calcola_Interessi = chkCalcolaInteressi.Checked
                        Next
                        Session("oAccertatiDaDichiarazione") = oAccertatiGriglia
                    End If
                    Dim sScript As String = ""
                    sScript += "parent.parent.opener.location.href='../../GestioneAccertamentiTARSU/SearchAccertatiTARSU.aspx';" & vbCrLf
                    sScript += "parent.window.close();"
                    RegisterScript(sScript, Me.GetType())
                End If
                '*** 20130801 - accertamento OSAP ***
            ElseIf ConstSession.CodTributo = Utility.Costanti.TRIBUTO_OSAP Then
                Dim oAccertatiGriglia() As ComPlusInterface.OSAPAccertamentoArticolo
                If Not Session("oAccertatiGriglia") Is Nothing Then
                    If Not ViewState("idLegame") Is Nothing Then
                        idLegame = ViewState("idLegame")
                    Else
                        idLegame = ""
                    End If

                    oAccertatiGriglia = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())

                    IdSanzioni = creaArrayIdSanzioni()

                    For intAccertatiGriglia = 0 To oAccertatiGriglia.Length - 1
                        If oAccertatiGriglia(intAccertatiGriglia).IdLegame = idLegame Or ChkSanzGlobal.Checked = True Then
                            Dim i As Integer
                            If IdSanzioni Is Nothing Then
                                oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
                            Else
                                If UBound(IdSanzioni) >= 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = ""
                                End If
                                For i = 0 To UBound(IdSanzioni)
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = oAccertatiGriglia(intAccertatiGriglia).Sanzioni & IdSanzioni(i) & ","
                                Next

                                If UBound(IdSanzioni) >= 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = Left(oAccertatiGriglia(intAccertatiGriglia).Sanzioni, Len(oAccertatiGriglia(intAccertatiGriglia).Sanzioni) - 1)
                                End If

                                If UBound(IdSanzioni) < 0 Then
                                    oAccertatiGriglia(intAccertatiGriglia).Sanzioni = "-1"
                                End If
                            End If
                            'aggiorno il flag calcolainteressi per ogno gruppo di immobili########################
                            oAccertatiGriglia(intAccertatiGriglia).Calcola_Interessi = chkCalcolaInteressi.Checked
                        End If
                    Next

                    Session("oAccertatiGriglia") = oAccertatiGriglia

                    Dim sScript As String = ""
                    sScript += "parent.parent.opener.location.href='../../GestioneAccertamentiOSAP/SearchDatiAccertatoOSAP.aspx';" & vbCrLf
                    sScript += "parent.window.close();"
                    RegisterScript(sScript, Me.GetType())
                End If
                '*** ***
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.btnAggiornaGriglia_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function creaArrayIdSanzioni() As String()
        Dim IdSanzioni As New ArrayList
        Try
            For Each myRow As GridViewRow In GrdSanzioni.Rows
                If (CType(myRow.FindControl("chkSanzioni"), CheckBox)).Checked Then
                    Dim myDescr As String = ""
                    myDescr += CType(myRow.FindControl("hfCodVoce"), HiddenField).Value
                    myDescr += "#" + CType(myRow.FindControl("hfCodTipoProv"), HiddenField).Value
                    If ChkSanzGlobal.Checked Then
                        myDescr += "#1"
                    Else
                        myDescr += "#"
                    End If
                    IdSanzioni.Add(myDescr)
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.creaArrayIdSanzioni.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return IdSanzioni.ToArray(GetType(String))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalvaMotivazioni_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalvaMotivazioni.Click
        Try
            If GrdSanzioni.SelectedIndex >= 0 Then
                '***************************************************
                Dim workTable() As objUIICIAccert
                Dim intAccertatiGriglia As Integer

                If Not ViewState("idLegame") Is Nothing Then
                    idLegame = ViewState("idLegame")
                Else
                    idLegame = " "
                End If

                'ICI
                If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                    workTable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                    For Each rowsArray As objUIICIAccert In workTable
                        'aggiorno la descrizione della motivazione per l'immobile selezionato
                        If rowsArray.IdLegame = idLegame Then
                            rowsArray.DescrSanzioni = txtMotivazioni.Text
                        End If
                    Next
                    Session("DataTableImmobiliDaAccertare") = workTable
                End If

                        'TARSU/OSAP
                        If Not IsNothing(Session("oAccertatiGriglia")) Then
                    '*** 20130801 - accertamento OSAP ***
                    If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_OSAP Then
                        Dim oaccertato() As ComPlusInterface.OSAPAccertamentoArticolo
                        oaccertato = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())
                        For intAccertatiGriglia = 0 To oaccertato.Length - 1
                            If oaccertato(intAccertatiGriglia).IdLegame = idLegame Then
                                oaccertato(intAccertatiGriglia).DescrSanzioni = txtMotivazioni.Text
                                Exit For
                            End If
                        Next
                        Session("oAccertatiGriglia") = oaccertato
                    Else
                        '*** 20140701 - IMU/TARES ***
                        'Dim oAccertato() As OggettoArticoloRuolo
                        'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
                        Dim oAccertato() As ObjArticoloAccertamento
                        oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                        '*** ***
                        'Dim oAccertato() As OggettoArticoloRuoloAccertamento
                        'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
                        For intAccertatiGriglia = 0 To oAccertato.Length - 1
                            If oAccertato(intAccertatiGriglia).IdLegame = idLegame Then
                                oAccertato(intAccertatiGriglia).sDescrSanzioni = txtMotivazioni.Text
                                Exit For
                            End If
                        Next
                        Session("oAccertatiGriglia") = oAccertato
                    End If
                    '*** ***
                End If

                '********************************************

                'Salvo il DataSet con le nuove modifiche
                Dim sCodVoce As String
                Dim objDS As DataSet
                Dim drow() As DataRow

                'For i = 0 To grdsanzioni.Items.Count
                For Each myRow As GridViewRow In GrdSanzioni.Rows
                    If myRow.RowIndex = GrdSanzioni.SelectedIndex Then
                        sCodVoce = myRow.Cells(2).Text
                        'sCodVoce = GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(2).Text
                        objDS = Session("DataSetSanzioni")
                        drow = objDS.Tables(0).Select("COD_VOCE='" & sCodVoce & "'")
                        drow(0)("Motivazione") = txtMotivazioni.Text
                        'drow(0)("CHECKSANZIONE") = grdsanzioni.Items(grdsanzioni.SelectedIndex).Cells(4).Text
                        drow(0).AcceptChanges()
                        Return
                    End If
                Next
                ' Next

                objDS.Tables(0).AcceptChanges()

                Session("DataSetSanzioni") = objDS

                GrdSanzioni.SelectedIndex = -1
                GrdSanzioni.DataSource = objDS
                GrdSanzioni.DataBind()
            Else
                Dim sScript As String = ""
                sScript += "GestAlert('a', 'warning', '', '', 'Attenzione, selezionare una sanzione per salvare la relativa motivazione!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.btnSalvaMotivazioni_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim chkSanzioni As CheckBox
                Dim idSanzione As String
                Dim arraySanzioni() As String
                Dim i As Integer

                arraySanzioni = Split(strSanzioni, ",")
                chkSanzioni = e.Row.FindControl("chkSanzioni")
                e.Row.Attributes.Remove("OnClick")
                e.Row.Cells(0).Attributes.Add("OnClick", "javascript:__doPostBack('" & e.Row.UniqueID & ":mouseclick','')")
                idSanzione = CType(e.Row.FindControl("hfCodVoce"), HiddenField).Value
                If StrComp(bloccaCheck, "1") = 0 Then
                    chkSanzioni.Enabled = False
                    txtMotivazioni.ReadOnly = True
                End If

                For i = 0 To UBound(arraySanzioni)
                    If idSanzione = arraySanzioni(i) Then
                        chkSanzioni.Checked = True
                        CType(e.Row.FindControl("hfCheckSanzione"), HiddenField).Value = 1
                    End If
                Next

                If UBound(arraySanzioni) = 0 Then
                    If CType(e.Row.FindControl("hfCheckSanzione"), HiddenField).Value = 1 Then
                        chkSanzioni.Checked = True
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Select Case e.CommandName
                Case "RowViewMotivazioni"
                    Dim sMotivazioniSalvate As String = ""
                    Dim sCodVoceSalvato As String = ""
                    Dim arrcodvoce As Object

                    If Not ViewState("idLegame") Is Nothing Then
                        idLegame = ViewState("idLegame")
                        If Not IsNothing(Session("DataTableImmobiliDaAccertare")) Then
                            For Each myItem As objUIICIAccert In CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                                If myItem.IdLegame = idLegame Then
                                    sMotivazioniSalvate = myItem.DescrSanzioni
                                    sCodVoceSalvato = myItem.IdSanzioni
                                    arrcodvoce = sCodVoceSalvato.Split("#")
                                    sCodVoceSalvato = arrcodvoce(0)
                                End If
                            Next
                        End If

                        If Not IsNothing(Session("oAccertatiGriglia")) Then
                            '*** 20130801 - accertamento OSAP ***
                            If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_OSAP Then
                                Dim oAccertato() As ComPlusInterface.OSAPAccertamentoArticolo
                                oAccertato = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())
                                For intGrigliaAccertati As Integer = 0 To oAccertato.Length - 1
                                    If oAccertato(intGrigliaAccertati).IdLegame = idLegame Then
                                        sMotivazioniSalvate = oAccertato(intGrigliaAccertati).DescrSanzioni
                                        sCodVoceSalvato = oAccertato(intGrigliaAccertati).Sanzioni
                                        arrcodvoce = sCodVoceSalvato.Split("#")
                                        sCodVoceSalvato = arrcodvoce(0)
                                        Exit For
                                    End If
                                Next
                            Else
                                '*** 20140701 - IMU/TARES ***
                                Dim oAccertato() As ObjArticoloAccertamento
                                oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                                For intGrigliaAccertati As Integer = 0 To oAccertato.Length - 1
                                    If oAccertato(intGrigliaAccertati).IdLegame = idLegame Then
                                        sMotivazioniSalvate = oAccertato(intGrigliaAccertati).sDescrSanzioni
                                        sCodVoceSalvato = oAccertato(intGrigliaAccertati).Sanzioni
                                        arrcodvoce = sCodVoceSalvato.Split("#")
                                        sCodVoceSalvato = arrcodvoce(0)
                                        Exit For
                                    End If
                                Next
                            End If
                            '*** ***
                        End If
                    End If
                    If sMotivazioniSalvate <> "" And sCodVoceSalvato = IDRow Then
                        txtMotivazioni.Text = sMotivazioniSalvate
                    Else
                        'Se le motivazioni sono vuote le carico da DB altrimenti prendo il campo 
                        If sMotivazioniSalvate = "" Then
                            Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB

                            Dim objDataReaderMotivazioni As SqlClient.SqlDataReader
                            If StrComp(bloccaCheck, "1") = 0 And id_provvedimento <> "" Then
                                objDataReaderMotivazioni = objGestOPENgovProvvedimenti.getMotivazioni(IDRow, id_provvedimento)
                            Else
                                objDataReaderMotivazioni = objGestOPENgovProvvedimenti.getMotivazioni(IDRow)
                            End If

                            Dim sElencoMotivazioni As String = String.Empty
                            txtMotivazioni.Text = ""
                            If Not objDataReaderMotivazioni Is Nothing Then
                                While objDataReaderMotivazioni.Read()
                                    If Not IsDBNull(objDataReaderMotivazioni("DESCRIZIONE_MOTIVAZIONE")) Then
                                        sElencoMotivazioni = sElencoMotivazioni & objDataReaderMotivazioni("DESCRIZIONE_MOTIVAZIONE") & vbCrLf
                                    End If
                                End While
                            End If

                            txtMotivazioni.Text = sElencoMotivazioni
                        End If
                    End If
                Case "RowMotivazioni"
                    Dim workTable() As objUIICIAccert
                    Dim intAccertatiGriglia As Integer

                    If Not ViewState("idLegame") Is Nothing Then
                        idLegame = ViewState("idLegame")
                    Else
                        idLegame = " "
                    End If

                    'ICI
                    If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                        workTable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                        For Each rowsArray As objUIICIAccert In workTable
                            'aggiorno la descrizione della motivazione per l'immobile selezionato
                            If rowsArray.IdLegame = idLegame Then
                                rowsArray.DescrSanzioni = txtMotivazioni.Text
                            End If
                        Next
                        Session("DataTableImmobiliDaAccertare") = workTable
                    End If

                    'TARSU/OSAP
                    If Not IsNothing(Session("oAccertatiGriglia")) Then
                        '*** 20130801 - accertamento OSAP ***
                        If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_OSAP Then
                            Dim oaccertato() As ComPlusInterface.OSAPAccertamentoArticolo
                            oaccertato = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())
                            For intAccertatiGriglia = 0 To oaccertato.Length - 1
                                If oaccertato(intAccertatiGriglia).IdLegame = idLegame Then
                                    oaccertato(intAccertatiGriglia).DescrSanzioni = txtMotivazioni.Text
                                    Exit For
                                End If
                            Next
                            Session("oAccertatiGriglia") = oaccertato
                        Else
                            '*** 20140701 - IMU/TARES ***
                            Dim oAccertato() As ObjArticoloAccertamento
                            oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                            '*** ***
                            For intAccertatiGriglia = 0 To oAccertato.Length - 1
                                If oAccertato(intAccertatiGriglia).IdLegame = idLegame Then
                                    oAccertato(intAccertatiGriglia).sDescrSanzioni = txtMotivazioni.Text
                                    Exit For
                                End If
                            Next
                            Session("oAccertatiGriglia") = oAccertato
                        End If
                        '*** ***
                    End If
                    '********************************************
                    'Salvo il DataSet con le nuove modifiche
                    Dim objDS As DataSet
                    Dim drow() As DataRow

                    objDS = Session("DataSetSanzioni")
                    drow = objDS.Tables(0).Select("COD_VOCE='" & IDRow & "'")
                    drow(0)("Motivazione") = txtMotivazioni.Text
                    drow(0).AcceptChanges()

                    objDS.Tables(0).AcceptChanges()

                    Session("DataSetSanzioni") = objDS

                    GrdSanzioni.SelectedIndex = -1
                    GrdSanzioni.DataSource = objDS
                    GrdSanzioni.DataBind()
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowIndexChanged(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim sMotivazioniSalvate As String = ""
            Dim sCodVoceSel, sCodVoceSalvato As String
            Dim arrcodvoce As Object
            Dim intGrigliaAccertati As Integer

            If GrdSanzioni.SelectedIndex >= 0 Then
                For Each myRow As GridViewRow In GrdSanzioni.Rows
                    If myRow.RowIndex = GrdSanzioni.SelectedIndex Then
                        sCodVoceSalvato = ""
                        sCodVoceSel = myRow.Cells(2).Text

                        If Not ViewState("idLegame") Is Nothing Then
                            idLegame = ViewState("idLegame")
                            If Not IsNothing(Session("DataTableImmobiliDaAccertare")) Then
                                For Each myItem As objUIICIAccert In CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                                    If myItem.IdLegame = idLegame Then
                                        sMotivazioniSalvate = myItem.DescrSanzioni
                                        sCodVoceSalvato = myItem.IdSanzioni
                                        arrcodvoce = sCodVoceSalvato.Split("#")
                                        sCodVoceSalvato = arrcodvoce(0)
                                    Else
                                        sMotivazioniSalvate = ""
                                        sCodVoceSalvato = ""
                                    End If
                                Next
                            End If

                            If Not IsNothing(Session("oAccertatiGriglia")) Then
                                '*** 20130801 - accertamento OSAP ***
                                If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_OSAP Then
                                    Dim oAccertato() As ComPlusInterface.OSAPAccertamentoArticolo
                                    oAccertato = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())
                                    For intGrigliaAccertati = 0 To oAccertato.Length - 1
                                        If oAccertato(intGrigliaAccertati).IdLegame = idLegame Then
                                            sMotivazioniSalvate = oAccertato(intGrigliaAccertati).DescrSanzioni
                                            sCodVoceSalvato = oAccertato(intGrigliaAccertati).Sanzioni
                                            arrcodvoce = sCodVoceSalvato.Split("#")
                                            sCodVoceSalvato = arrcodvoce(0)
                                            Exit For
                                        End If
                                    Next
                                Else
                                    '*** 20140701 - IMU/TARES ***
                                    'Dim oAccertato() As OggettoArticoloRuolo
                                    'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
                                    Dim oAccertato() As ObjArticoloAccertamento
                                    oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                                    'Dim oAccertato() As OggettoArticoloRuoloAccertamento
                                    'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
                                    For intGrigliaAccertati = 0 To oAccertato.Length - 1
                                        If oAccertato(intGrigliaAccertati).IdLegame = idLegame Then
                                            sMotivazioniSalvate = oAccertato(intGrigliaAccertati).sDescrSanzioni
                                            sCodVoceSalvato = oAccertato(intGrigliaAccertati).Sanzioni
                                            arrcodvoce = sCodVoceSalvato.Split("#")
                                            sCodVoceSalvato = arrcodvoce(0)
                                            Exit For
                                        End If
                                    Next
                                End If
                                '*** ***
                            End If
                        End If
                        If sMotivazioniSalvate <> "" And sCodVoceSalvato = sCodVoceSel Then
                            txtMotivazioni.Text = sMotivazioniSalvate
                            myRow.Cells(3).Text = sMotivazioniSalvate
                        Else
                            'Se le motivazioni sono vuote le carico da DB altrimenti prendo il campo 
                            If myRow.Cells(3).Text = "&nbsp;" Then
                                myRow.Cells(3).Text = ""
                            Else
                                txtMotivazioni.Text = myRow.Cells(3).Text
                            End If
                            If myRow.Cells(3).Text = "" Then
                                Dim idSanzione As String = myRow.Cells(2).Text
                                Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB

                                Dim objDataReaderMotivazioni As SqlClient.SqlDataReader
                                If StrComp(bloccaCheck, "1") = 0 And id_provvedimento <> "" Then
                                    objDataReaderMotivazioni = objGestOPENgovProvvedimenti.getMotivazioni(idSanzione, id_provvedimento)
                                Else
                                    objDataReaderMotivazioni = objGestOPENgovProvvedimenti.getMotivazioni(idSanzione)
                                End If

                                Dim sElencoMotivazioni As String = String.Empty
                                txtMotivazioni.Text = ""
                                If Not objDataReaderMotivazioni Is Nothing Then
                                    While objDataReaderMotivazioni.Read()
                                        If Not IsDBNull(objDataReaderMotivazioni("DESCRIZIONE_MOTIVAZIONE")) Then
                                            sElencoMotivazioni = sElencoMotivazioni & objDataReaderMotivazioni("DESCRIZIONE_MOTIVAZIONE") & vbCrLf
                                        End If
                                    End While
                                End If

                                txtMotivazioni.Text = sElencoMotivazioni
                                myRow.Cells(3).Text = sElencoMotivazioni
                            End If
                        End If
                        Dim chk As CheckBox
                        chk = myRow.FindControl("chkSanzioni")

                        If chk.Checked = True Then
                            myRow.Cells(4).Text = 1
                        Else
                            myRow.Cells(4).Text = 0
                        End If
                        Return
                    End If
                Next
            Else
                Dim sScript As String = ""
                sScript += "GestAlert('a', 'warning', '', '', 'Attenzione, selezionare una sanzione per salvare la relativa motivazione!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.GrdRowIndexChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub grdsanzioni_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdSanzioni.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni As CheckBox
    '        Dim idSanzione As String
    '        Dim arraySanzioni() As String
    '        Dim i As Integer

    '        arraySanzioni = Split(strSanzioni, ",")
    '        chkSanzioni = e.Item.Cells(1).FindControl("chkSanzioni")
    '        e.Item.Attributes.Remove("OnClick")
    '        e.Item.Cells(0).Attributes.Add("OnClick", "javascript:__doPostBack('" & e.Item.UniqueID & ":mouseclick','')")
    '        idSanzione = e.Item.Cells(2).Text

    '        If StrComp(bloccaCheck, "1") = 0 Then
    '            chkSanzioni.Enabled = False
    '            txtMotivazioni.ReadOnly = True
    '        End If

    '        For i = 0 To UBound(arraySanzioni)
    '            If idSanzione = arraySanzioni(i) Then
    '                chkSanzioni.Checked = True
    '                e.Item.Cells(4).Text = 1
    '            End If
    '        Next

    '        If UBound(arraySanzioni) = 0 Then
    '            If e.Item.Cells(4).Text = 1 Then
    '                chkSanzioni.Checked = True
    '            End If
    '        End If
    '    End If
    'Catch ex As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.grdsanzioni_ItemDataBound.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub grdsanzioni_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdSanzioni.SelectedIndexChanged
    '    ' ViewState("idGrid") = grdsanzioni.SelectedIndex
    '    Dim sMotivazioniSalvate As String = ""
    '    Dim sCodVoceSel, sCodVoceSalvato As String
    '    Dim arrcodvoce As Object
    '    Dim intGrigliaAccertati As Integer
    'Try
    '    If GrdSanzioni.SelectedIndex >= 0 Then
    '        sCodVoceSalvato = ""
    '        sCodVoceSel = GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(2).Text

    '        If Not ViewState("idLegame") Is Nothing Then
    '            idLegame = ViewState("idLegame")
    '            If Not IsNothing(Session("DataTableImmobili")) Then
    '                If Not IsDBNull(Session("DataTableImmobili").Select("IDLEGAME='" & idLegame & "'")(0).itemarray(35)) Then
    '                    sMotivazioniSalvate = Session("DataTableImmobili").Select("IDLEGAME='" & idLegame & "'")(0).itemarray(35)
    '                    sCodVoceSalvato = Session("DataTableImmobili").Select("IDLEGAME='" & idLegame & "'")(0).itemarray(12)
    '                    arrcodvoce = sCodVoceSalvato.Split("#")
    '                    sCodVoceSalvato = arrcodvoce(0)
    '                Else
    '                    sMotivazioniSalvate = ""
    '                    sCodVoceSalvato = ""
    '                End If
    '            End If

    '            If Not IsNothing(Session("oAccertatiGriglia")) Then
    '                '*** 20130801 - accertamento OSAP ***
    '                If CONSTSESSION.CODTRIBUTO = Utility.Costanti.TRIBUTO_OSAP Then
    '                    Dim oAccertato() As ComPlusInterface.OSAPAccertamentoArticolo
    '                    oAccertato = CType(Session("oAccertatiGriglia"), ComPlusInterface.OSAPAccertamentoArticolo())
    '                    For intGrigliaAccertati = 0 To oAccertato.Length - 1
    '                        If oAccertato(intGrigliaAccertati).IdLegame = idLegame Then
    '                            sMotivazioniSalvate = oAccertato(intGrigliaAccertati).DescrSanzioni
    '                            sCodVoceSalvato = oAccertato(intGrigliaAccertati).Sanzioni
    '                            arrcodvoce = sCodVoceSalvato.Split("#")
    '                            sCodVoceSalvato = arrcodvoce(0)
    '                            Exit For
    '                        End If
    '                    Next
    '                Else
    '                    '*** 20140701 - IMU/TARES ***
    '                    'Dim oAccertato() As OggettoArticoloRuolo
    '                    'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
    '                    Dim oAccertato() As ObjArticoloAccertamento
    '                    oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
    '                    'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '                    'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
    '                    For intGrigliaAccertati = 0 To oAccertato.Length - 1
    '                        If oAccertato(intGrigliaAccertati).IdLegame = idLegame Then
    '                            sMotivazioniSalvate = oAccertato(intGrigliaAccertati).sDescrSanzioni
    '                            sCodVoceSalvato = oAccertato(intGrigliaAccertati).Sanzioni
    '                            arrcodvoce = sCodVoceSalvato.Split("#")
    '                            sCodVoceSalvato = arrcodvoce(0)
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '                '*** ***
    '            End If
    '        End If
    '        If sMotivazioniSalvate <> "" And sCodVoceSalvato = sCodVoceSel Then
    '            txtMotivazioni.Text = sMotivazioniSalvate
    '            GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(3).Text = sMotivazioniSalvate
    '        Else

    '            'Se le motivazioni sono vuote le carico da DB altrimenti prendo il campo 
    '            If GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(3).Text = "&nbsp;" Then
    '                GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(3).Text = ""
    '            Else
    '                txtMotivazioni.Text = GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(3).Text
    '            End If
    '            If GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(3).Text = "" Then
    '                'Dim idSanzione As Integer = grdsanzioni.Items(grdsanzioni.SelectedIndex).Cells(2).Text
    '                Dim idSanzione As String = GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(2).Text
    '                Dim objGestOPENgovProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB

    '                Dim objDataReaderMotivazioni As SqlClient.SqlDataReader
    '                If StrComp(bloccaCheck, "1") = 0 And id_provvedimento <> "" Then
    '                    objDataReaderMotivazioni = objGestOPENgovProvvedimenti.getMotivazioni(idSanzione, id_provvedimento)
    '                Else
    '                    objDataReaderMotivazioni = objGestOPENgovProvvedimenti.getMotivazioni(idSanzione)
    '                End If

    '                Dim sElencoMotivazioni As String = String.Empty
    '                txtMotivazioni.Text = ""
    '                If Not objDataReaderMotivazioni Is Nothing Then
    '                    While objDataReaderMotivazioni.Read()
    '                        If Not IsDBNull(objDataReaderMotivazioni("DESCRIZIONE_MOTIVAZIONE")) Then
    '                            sElencoMotivazioni = sElencoMotivazioni & objDataReaderMotivazioni("DESCRIZIONE_MOTIVAZIONE") & vbCrLf
    '                        End If
    '                    End While
    '                End If

    '                txtMotivazioni.Text = sElencoMotivazioni
    '                GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(3).Text = sElencoMotivazioni
    '            End If
    '        End If
    '        Dim chk As CheckBox
    '        chk = GrdSanzioni.Items(GrdSanzioni.SelectedIndex).FindControl("chkSanzioni")

    '        If chk.Checked = True Then
    '            GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(4).Text = 1
    '        Else
    '            GrdSanzioni.Items(GrdSanzioni.SelectedIndex).Cells(4).Text = 0
    '        End If
    '    Else
    '        dim sScript as string=""
    '        
    '        sscript+="alert('Attenzione, selezionare una sanzione per salvare la relativa motivazione!');")
    '        
    '        RegisterScript(sScript , Me.GetType())
    '    End If
    'Catch ex As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Sanzioni.grdsanzioni_SelectedIndexChanged.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    'End Try

    'End Sub
#End Region
End Class
