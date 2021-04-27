Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports ComPlusInterface
Imports System.Globalization
''' <summary>
''' Pagina per la generazione dei provvedimenti TARI.
''' Contiene le funzioni della comandiera e la griglia per la gestione dell'accertato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class SearchAccertatiTARSU
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchAccertatiTARSU))
    Protected FncGrd As New Formatta.FunctionGrd

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTesto As System.Web.UI.WebControls.Label
    Protected WithEvents chkSelTutti As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    '*** 20140701 - IMU/TARES ***
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim oAccertato() As OggettoArticoloRuolo
    '    Dim oAccertatoInserito As OggettoArticoloRuolo
    '    'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertatoInserito As OggettoArticoloRuoloAccertamento
    '    Dim intProgressivo As Integer
    '    Dim intGrigliaAccertati As Integer
    '    Dim x As Integer

    '    Try
    '        If Page.IsPostBack = False Then
    '            If Not Session("oAccertato") Is Nothing Then
    '                'devo caricare l'oogetto ArticoloRuoloAccertamento per popolare la griglia
    '                oAccertatoInserito = CType(Session("oAccertato"), OggettoArticoloRuolo)
    '                'oAccertatoInserito = CType(Session("oAccertato"), OggettoArticoloRuoloAccertamento)
    '                If Not Session("oAccertatiGriglia") Is Nothing And oAccertatoInserito.Progressivo <> 0 Then
    '                    oAccertato = Session("oAccertatiGriglia")
    '                    For intGrigliaAccertati = 0 To oAccertato.Length - 1
    '                        If oAccertato(intGrigliaAccertati).Progressivo = oAccertatoInserito.Progressivo Then
    '                            oAccertato(intGrigliaAccertati) = oAccertatoInserito
    '                        End If
    '                        oAccertato(intGrigliaAccertati).Ente = ConstSession.IdEnte
    '                    Next
    '                ElseIf oAccertatoInserito.Progressivo <> 0 Then
    '                    oAccertato = Session("oAccertatiGriglia")
    '                    intProgressivo = oAccertato.Length + 1

    '                    ReDim Preserve oAccertato(oAccertato.Length)
    '                    oAccertato(oAccertato.Length - 1) = oAccertatoInserito
    '                    oAccertato(oAccertato.Length - 1).Progressivo = intProgressivo
    '                    'Svuota la Session con la dichirazione precedente
    '                ElseIf oAccertatoInserito.Progressivo = 0 Then
    '                    If Not Session("oAccertatiGriglia") Is Nothing Then
    '                        oAccertato = Session("oAccertatiGriglia")
    '                        intProgressivo = oAccertato.Length + 1

    '                        ReDim Preserve oAccertato(oAccertato.Length)
    '                        oAccertato(oAccertato.Length - 1) = oAccertatoInserito
    '                        oAccertato(oAccertato.Length - 1).Progressivo = intProgressivo
    '                    Else
    '                        intProgressivo = 1
    '                        ReDim Preserve oAccertato(0)
    '                        oAccertato(0) = oAccertatoInserito
    '                        oAccertato(0).Progressivo = intProgressivo
    '                    End If
    '                End If
    '                GrdAccertato.start_index = 0
    '                GrdAccertato.DataSource = oAccertato
    '                Session("oAccertatiGriglia") = oAccertato
    '                GrdAccertato.DataBind()

    '            ElseIf Not Session("oAccertatiGriglia") Is Nothing Then
    '                'If Not Session("oAcceratoXsanzioni") Is Nothing Then
    '                '    Dim oAccertatoSanzioni As OggettoArticoloRuoloAccertamento()
    '                '    Dim intoAccertatoSanzioni As Integer

    '                '    oAccertatoSanzioni = CType(Session("oAcceratoXsanzioni"), OggettoArticoloRuoloAccertamento())
    '                '    oAccertato = Session("oAccertatiGriglia")

    '                '    For x = 0 To oAccertato.Length - 1
    '                '        For intoAccertatoSanzioni = 0 To oAccertatoSanzioni.Length - 1
    '                '            If oAccertatoSanzioni(intoAccertatoSanzioni).IdLegame = oAccertato(x).IdLegame Then
    '                '                oAccertato(x).Sanzioni = oAccertatoSanzioni(intoAccertatoSanzioni).Sanzioni
    '                '                oAccertato(x).Calcola_Interessi = oAccertatoSanzioni(intoAccertatoSanzioni).Calcola_Interessi
    '                '            End If
    '                '            oAccertato(x).Ente = constsession.idente
    '                '        Next
    '                '    Next
    '                'Else
    '                '    oAccertato = Session("oAccertatiGriglia")
    '                'End If
    '                oAccertato = Session("oAccertatiGriglia")

    '                GrdAccertato.start_index = 0
    '                GrdAccertato.DataSource = oAccertato
    '                Session("oAccertatiGriglia") = oAccertato
    '                GrdAccertato.DataBind()

    '            End If
    '            Session("oAccertato") = Nothing


    '            Select Case CInt(GrdAccertato.Rows.Count)
    '                Case 0
    '                    GrdAccertato.Visible = False
    '                    'lblMessage.Text = "Non sono presenti Immobili Accertati"
    '                Case Is > 0

    '                    GrdAccertato.Visible = True
    '            End Select
    '        End If

    '        dim sScript as string=""
    '        
    '        sscript+="parent.document.getElementById('attesaCarica').style.display='none';")
    '        sscript+="parent.document.getElementById('loadGridAccertato').style.display='' ;")
    '        
    '        RegisterScript(sScript , Me.GetType())

    '    Catch Err As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU
        Dim oAccertato() As ObjArticoloAccertamento
        Dim ListPartiteAccertato() As ObjArticoloAccertamento
        Dim oAccertatoInserito As ObjArticoloAccertamento
        Dim intProgressivo As Integer
        Dim intGrigliaAccertati As Integer
        Dim objHashTable As Hashtable

        Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: inizio")
            Try
                hfAnno.Value = Utility.StringOperation.FormatString(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"))
                hfIdContribuente.Value = Utility.StringOperation.FormatString(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"))
                btnInsManuale.Attributes.Add("onclick", "return ApriInserimentoImmobile();return false;")
                btnAnater.Attributes.Add("onclick", "return ApriRicercaAnater(); return false;")
                btnAnater.Text = ConstSession.NameSistemaTerritorio
                btnAnater.ToolTip = "Ricerca e Selezione Immobile da " & ConstSession.NameSistemaTerritorio
                btnTerritorio.Attributes.Add("onclick", "return ApriRicercaTerritorio(); return false;")
                btnTerritorio.Text = ConstSession.NameSistemaTerritorio
                btnTerritorio.ToolTip = "Ricerca e Selezione Immobile da " & ConstSession.NameSistemaTerritorio
                btnCatasto.Attributes.Add("onclick", "return msg();")
                btnAccertato.Attributes.Add("onclick", "return ApriRicercaAccertato();return false;")
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore:inizio: ", ex)
            End Try
            objHashTable = Session("HashTableDichAccertamentiTARSU")
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: popolato bottoni e hiddenfield")
                If Page.IsPostBack = False Then
                    If Not Session("oAccertato") Is Nothing Then
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: ho session oaccertato")
                        'devo caricare l'oogetto ArticoloRuoloAccertamento per popolare la griglia
                        oAccertatoInserito = CType(Session("oAccertato"), ObjArticoloAccertamento)
                        If Not Session("oAccertatiGriglia") Is Nothing And oAccertatoInserito.Progressivo <> 0 Then
                            oAccertato = Session("oAccertatiGriglia")
                            For intGrigliaAccertati = 0 To oAccertato.Length - 1
                                If oAccertato(intGrigliaAccertati).Progressivo = oAccertatoInserito.Progressivo Then
                                    oAccertato(intGrigliaAccertati) = oAccertatoInserito
                                End If
                                oAccertato(intGrigliaAccertati).IdEnte = ConstSession.IdEnte
                            Next
                        ElseIf oAccertatoInserito.Progressivo <> 0 Then
                            oAccertato = Session("oAccertatiGriglia")
                            intProgressivo = oAccertato.Length + 1

                            ReDim Preserve oAccertato(oAccertato.Length)
                            oAccertato(oAccertato.Length - 1) = oAccertatoInserito
                            oAccertato(oAccertato.Length - 1).Progressivo = intProgressivo
                            'Svuota la Session con la dichirazione precedente
                        ElseIf oAccertatoInserito.Progressivo = 0 Then
                            If Not Session("oAccertatiGriglia") Is Nothing Then
                                oAccertato = Session("oAccertatiGriglia")
                                intProgressivo = oAccertato.Length + 1

                                ReDim Preserve oAccertato(oAccertato.Length)
                                oAccertato(oAccertato.Length - 1) = oAccertatoInserito
                                oAccertato(oAccertato.Length - 1).Progressivo = intProgressivo
                            Else
                                intProgressivo = 1
                                ReDim Preserve oAccertato(0)
                                oAccertato(0) = oAccertatoInserito
                                oAccertato(0).Progressivo = intProgressivo
                            End If
                        End If
                    ElseIf Not Session("oAccertatiGriglia") Is Nothing Then
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: ho session accertatogriglia")
                        oAccertato = Session("oAccertatiGriglia")
                    End If
                    Session("oAccertatiGriglia") = oAccertato
                    Session("oAccertato") = Nothing

                    'la griglia deve essere popolata con le partite di calcolo + le partite negative dell'emesso
                    If Not oAccertato Is Nothing Then
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: popolo listpartiteaccertato")
                        Dim myArray As New ArrayList
                        For Each oAccertatoInserito In oAccertato
                            myArray.Add(oAccertatoInserito)
                        Next
                        If Not IsNothing(Session("oAccertatiDaDichiarazione")) Then
                            oAccertato = Session("oAccertatiDaDichiarazione")
                            For Each oAccertatoInserito In oAccertato
                                myArray.Add(oAccertatoInserito)
                            Next
                        End If
                        ListPartiteAccertato = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
                        'nel calcolo TARES il progressivo ed il legame non hanno + senso quindi sono dei semplici contatori
                        If objHashTable("TipoTassazione") = ObjRuolo.TipoCalcolo.TARES Then
                            Dim nCount As Integer = 0
                            For Each oAccertatoInserito In ListPartiteAccertato
                                nCount += 1
                                oAccertatoInserito.Progressivo = nCount
                                oAccertatoInserito.IdLegame = nCount
                            Next
                        End If
                    End If
                    Session("PartiteAccertato") = ListPartiteAccertato
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: popolo griglia")
                    GrdAccertato.DataSource = ListPartiteAccertato
                    GrdAccertato.DataBind()
                    Select Case CInt(GrdAccertato.Rows.Count)
                        Case 0
                            GrdAccertato.Visible = False
                        'lblMessage.Text = "Non sono presenti Immobili Accertati"
                        Case Is > 0
                            GrdAccertato.Visible = True
                            If objHashTable("TipoTassazione") <> ObjRuolo.TipoCalcolo.TARES Then
                                GrdAccertato.Columns(10).Visible = False
                                GrdAccertato.Columns(11).Visible = False
                                GrdAccertato.Columns(12).Visible = False
                            End If
                    End Select
                End If

                Dim sScript As String = ""
                sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
                sScript += "parent.document.getElementById('loadGridAccertato').style.display='' ;"
                RegisterScript(sScript, Me.GetType())
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: fine")
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Protected Sub ChkInteressi_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim ck1 As CheckBox = CType(sender, CheckBox)
    '    Dim dgItem As GridViewRow = CType(ck1.NamingContainer, GridViewRow)
    '    Dim idlegame As Integer
    '    Dim oAccertato() As OggettoArticoloRuolo
    '    'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '    Dim intAccertato As Integer

    '    Try
    '        'reperisco idlegame
    '        idlegame = CInt(dgItem.Cells(12).Text)

    '        oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
    '        'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())

    '        For intAccertato = 0 To oAccertato.Length - 1
    '            If oAccertato(intAccertato).IdLegame = idlegame Then
    '                oAccertato(intAccertato).Calcola_Interessi = Not oAccertato(intAccertato).Calcola_Interessi
    '                Exit For
    '            End If
    '        Next
    '        Session("oAccertatiGriglia") = oAccertato

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.ChkInteressi_CheckedChanged.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ChkInteressi_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ck1 As CheckBox = CType(sender, CheckBox)
        Dim dgItem As GridViewRow = CType(ck1.NamingContainer, GridViewRow)
        Dim idlegame As Integer
        Dim oAccertato() As ObjArticoloAccertamento
        Dim intAccertato As Integer

        Try
            'reperisco idlegame
            idlegame = CInt(dgItem.Cells(15).Text)

            oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
            For intAccertato = 0 To oAccertato.Length - 1
                If oAccertato(intAccertato).IdLegame = idlegame Then
                    oAccertato(intAccertato).Calcola_Interessi = Not oAccertato(intAccertato).Calcola_Interessi
                    Exit For
                End If
            Next
            Session("oAccertatiGriglia") = oAccertato
            oAccertato = CType(Session("oAccertatiDaDichiarazione"), ObjArticoloAccertamento())
            For intAccertato = 0 To oAccertato.Length - 1
                If oAccertato(intAccertato).IdLegame = idlegame Then
                    oAccertato(intAccertato).Calcola_Interessi = Not oAccertato(intAccertato).Calcola_Interessi
                    Exit For
                End If
            Next
            Session("oAccertatiDaDichiarazione") = oAccertato
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.ChkInteressi_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Protected Sub GrdAccertato_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertato.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim lblLegame As Label
    '        Dim chkSanzioni As CheckBox
    '        Dim idSanzioni As String
    '        Dim check As CheckBox
    '        Dim ChkInteressi As CheckBox
    '        Dim IdProgressivo As Integer
    '        Dim intXsanzioni As Integer
    '        Dim IdLegame As Integer
    '        Dim obj As OggettoArticoloRuolo
    '        Dim objXsanzioni As OggettoArticoloRuolo()
    '        'Dim obj As OggettoArticoloRuoloAccertamento
    '        'Dim objXsanzioni As OggettoArticoloRuoloAccertamento()
    '        Dim bFound As Boolean = False

    '        'obj = CType(e.Item.DataItem, OggettoArticoloRuoloAccertamento)
    '        obj = CType(e.Item.DataItem, OggettoArticoloRuolo)
    '        IdLegame = obj.IdLegame
    '        IdProgressivo = obj.Progressivo

    '        lblLegame = e.Item.Cells(23).FindControl("lblLegame")
    '        chkSanzioni = e.Item.Cells(22).FindControl("chkSanzioni")
    '        ChkInteressi = e.Item.Cells(22).FindControl("ChkInteressi")

    '        idSanzioni = obj.Sanzioni
    '        If idSanzioni <> "-1" And idSanzioni <> "" Then
    '            chkSanzioni.Checked = True
    '        End If

    '        intXsanzioni = 0

    '        If Not Session("oAccertatiGriglia") Is Nothing Then
    '            'objXsanzioni = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
    '            objXsanzioni = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
    '            For intXsanzioni = 0 To objXsanzioni.Length - 1
    '                If objXsanzioni(intXsanzioni).IdLegame = IdLegame And objXsanzioni(intXsanzioni).Progressivo = IdProgressivo Then
    '                    bFound = True
    '                    Exit For
    '                Else
    '                    bFound = False
    '                End If
    '            Next
    '            If Not bFound Then
    '                ReDim Preserve objXsanzioni(objXsanzioni.Length)
    '                objXsanzioni(objXsanzioni.Length - 1) = obj
    '            End If

    '        Else
    '            ReDim Preserve objXsanzioni(intXsanzioni)
    '            objXsanzioni(intXsanzioni) = obj
    '        End If
    '        Session.Add("oAccertatiGriglia", objXsanzioni)

    '        'If Not Session("oAcceratoXsanzioni") Is Nothing Then
    '        '    objXsanzioni = CType(Session("oAcceratoXsanzioni"), OggettoArticoloRuoloAccertamento())
    '        '    For intXsanzioni = 0 To objXsanzioni.Length - 1
    '        '        If objXsanzioni(intXsanzioni).IdLegame = IdLegame And objXsanzioni(intXsanzioni).Progressivo = IdProgressivo Then
    '        '            bFound = True
    '        '            Exit For
    '        '        Else
    '        '            bFound = False
    '        '        End If
    '        '    Next
    '        '    If Not bFound Then
    '        '        ReDim Preserve objXsanzioni(objXsanzioni.Length)
    '        '        objXsanzioni(objXsanzioni.Length - 1) = obj
    '        '    End If

    '        'Else
    '        '    ReDim Preserve objXsanzioni(intXsanzioni)
    '        '    objXsanzioni(intXsanzioni) = obj
    '        'End If

    '        'Session.Add("oAcceratoXsanzioni", objXsanzioni)

    '        e.Item.Cells(22).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & IdLegame & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
    '        e.Item.Cells(27).Attributes.Add("onClick", "ApriModificaImmobileAnater('" & IdProgressivo & "')")
    '    End If
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_ItemDataBound.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                If e.Row.Cells(3).Text.Contains("DA DICHIARAZIONE") = False Then
                    If CType(e.Row.FindControl("hfTipoPartita"), HiddenField).Value = ObjArticolo.PARTEFISSA Or CType(e.Row.FindControl("hfTipoPartita"), HiddenField).Value = ObjArticolo.PARTECONFERIMENTI Then
                        Dim chkSanzioni As CheckBox
                        Dim idSanzioni As String
                        Dim IdProgressivo As Integer
                        Dim intXsanzioni As Integer
                        Dim IdLegame As Integer
                        Dim obj As ObjArticoloAccertamento
                        Dim objXsanzioni As ObjArticoloAccertamento()
                        Dim bFound As Boolean = False

                        obj = CType(e.Row.DataItem, ObjArticoloAccertamento)
                        IdLegame = obj.IdLegame
                        IdProgressivo = obj.Progressivo

                        chkSanzioni = e.Row.FindControl("chkSanzioni")

                        idSanzioni = obj.Sanzioni
                        If idSanzioni <> "-1" And idSanzioni <> "" Then
                            chkSanzioni.Checked = True
                        End If

                        intXsanzioni = 0
                        If Not Session("oAccertatiGriglia") Is Nothing Then
                            objXsanzioni = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                            For intXsanzioni = 0 To objXsanzioni.Length - 1
                                If objXsanzioni(intXsanzioni).IdLegame = IdLegame And objXsanzioni(intXsanzioni).Progressivo = IdProgressivo Then
                                    bFound = True
                                    Exit For
                                Else
                                    bFound = False
                                End If
                            Next
                            If Not bFound Then
                                ReDim Preserve objXsanzioni(objXsanzioni.Length)
                                objXsanzioni(objXsanzioni.Length - 1) = obj
                            End If
                        Else
                            ReDim Preserve objXsanzioni(intXsanzioni)
                            objXsanzioni(intXsanzioni) = obj
                        End If
                        Session.Add("oAccertatiGriglia", objXsanzioni)

                        e.Row.Cells(15).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & IdLegame & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
                    Else
                        e.Row.Attributes.Add("onClick", "GestAlert('a', 'warning', '', '', 'Per modificare questa partita agire sugli immobili.');")
                    End If
                Else
                    e.Row.Attributes.Add("onClick", "GestAlert('a', 'warning', '', '', 'Partita non modificabile!');")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdRowDataBound.errore: ", ex)
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
                Case "RowOpen"
                    RegisterScript("ApriModificaImmobileAnater('" & IDRow & "')", Me.GetType)
                Case "RowUpdate"
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            Dim oAccertamentoUpdate() As ObjArticoloAccertamento
                            Dim intArticoliUpdate As Integer
                            If CType(myRow.FindControl("txtLegame"), TextBox).Text = "" Then
                                Dim sScript As String = ""
                                sScript += "msgLegameVuoto();" & vbCrLf
                                RegisterScript(sScript, Me.GetType())
                            Else
                                oAccertamentoUpdate = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())

                                For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
                                    If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IDRow Then
                                        oAccertamentoUpdate(intArticoliUpdate).IdLegame = CType(myRow.FindControl("txtLegame"), TextBox).Text
                                    End If
                                Next

                                GrdAccertato.EditIndex = -1
                                GrdAccertato.DataSource = oAccertamentoUpdate
                                GrdAccertato.DataBind()
                                Session("oAccertatiGriglia") = oAccertamentoUpdate
                            End If
                        End If
                    Next
                Case "RowDelete"
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            Dim oAccertamentoDelete() As ObjArticoloAccertamento
                            Dim oAccertamentoReturn() As ObjArticoloAccertamento
                            Dim myArticolo As New ObjArticolo
                            Dim oListArticoli() As ObjArticolo
                            Dim myArray As New ArrayList
                            Dim myArtAcc As New ObjArticoloAccertamento
                            Dim FncAcc As New ClsGestioneAccertamenti
                            Dim FncRicalcolo As New OPENgovTIA.ClsElabRuolo
                            Dim sScript As String = ""
                            Dim objHashTable As Hashtable = Session("HashTableDichAccertamentiTARSU")

                            oAccertamentoDelete = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())

                            If Not oAccertamentoDelete Is Nothing Then
                                For Each myArt As ObjArticoloAccertamento In oAccertamentoDelete
                                    If myArt.Progressivo <> IDRow Then
                                        myArray.Add(myArt)
                                    End If
                                Next
                                oAccertamentoReturn = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())

                                oListArticoli = FncAcc.PrepareArticoliForCalcolo(oAccertamentoReturn, myArticolo, objHashTable("TipoTassazione"), False)
                                If FncRicalcolo.RicalcoloAvviso(ConstSession.StringConnectionTARSU, oListArticoli, sScript, objHashTable("TipoTassazione"), objHashTable("HasMaggiorazione"), objHashTable("HasConferimenti"), "P") = False Then
                                    RegisterScript(sScript, Me.GetType())
                                    Exit Sub
                                End If

                                Dim CurrentItem As New ObjArticoloAccertamento
                                myArray.Clear()
                                'prelevo gli articoli determinati dal ricalcolo
                                oListArticoli = Session("oListArticoli")
                                For Each myArticolo In oListArticoli
                                    CurrentItem = FncAcc.ArticoloTOArticoloAccertamento(myArticolo, True)
                                    If Not CurrentItem Is Nothing Then
                                        CurrentItem.IdEnte = ConstSession.IdEnte
                                        If Not oAccertamentoDelete Is Nothing Then
                                            For Each myArtAcc In oAccertamentoDelete
                                                If CurrentItem.Id = myArtAcc.Progressivo Then
                                                    CurrentItem.Sanzioni = myArtAcc.Sanzioni
                                                    CurrentItem.sDescrSanzioni = myArtAcc.sDescrSanzioni
                                                    CurrentItem.Calcola_Interessi = myArtAcc.Calcola_Interessi
                                                End If
                                            Next
                                        End If
                                    Else
                                        Throw New Exception("errore in caricamento articolo")
                                    End If
                                    myArray.Add(CurrentItem)
                                Next
                                oAccertamentoDelete = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
                                Session("oAccertatiGriglia") = oAccertamentoDelete

                                Dim ListPartiteAccertato() As ObjArticoloAccertamento
                                'la griglia deve essere popolata con le partite di calcolo + le partite negative dell'emesso
                                If Not IsNothing(Session("oAccertatiDaDichiarazione")) Then
                                    oAccertamentoDelete = Session("oAccertatiDaDichiarazione")
                                    For Each oAccertamento As ObjArticoloAccertamento In oAccertamentoDelete
                                        myArray.Add(oAccertamento)
                                    Next
                                End If
                                ListPartiteAccertato = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
                                'nel calcolo TARES il progressivo ed il legame non hanno + senso quindi sono dei semplici contatori
                                If objHashTable("TipoTassazione") = ObjRuolo.TipoCalcolo.TARES Then
                                    Dim nCount As Integer = 0
                                    For Each oAccertamento As ObjArticoloAccertamento In ListPartiteAccertato
                                        nCount += 1
                                        oAccertamento.Progressivo = nCount
                                        oAccertamento.IdLegame = nCount
                                    Next
                                End If
                                Session("PartiteAccertato") = ListPartiteAccertato
                                GrdAccertato.DataSource = ListPartiteAccertato
                                GrdAccertato.DataBind()
                                Select Case CInt(GrdAccertato.Rows.Count)
                                    Case 0
                                        GrdAccertato.Visible = False
                                    Case Is > 0
                                        GrdAccertato.Visible = True
                                        If objHashTable("TipoTassazione") <> ObjRuolo.TipoCalcolo.TARES Then
                                            GrdAccertato.Columns(10).Visible = False
                                            GrdAccertato.Columns(11).Visible = False
                                            GrdAccertato.Columns(12).Visible = False
                                        End If
                                End Select
                            Else
                                GrdAccertato.EditIndex = -1
                                GrdAccertato.DataSource = Nothing
                                GrdAccertato.DataBind()
                                GrdAccertato.Style.Add("display", "none")
                                Session("oAccertatiGriglia") = Nothing
                            End If
                        End If
                    Next
                Case "RowCancel"
                    If Not Session("oAccertatiGriglia") Is Nothing Then
                        GrdAccertato.EditIndex = -1
                        GrdAccertato.DataSource = Session("oAccertatiGriglia")
                        GrdAccertato.DataBind()
                    End If
                Case "RowEdit"
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            GrdAccertato.EditIndex = myRow.RowIndex
                            GrdAccertato.DataSource = Session("oAccertatiGriglia")
                            GrdAccertato.DataBind()
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdAccertato_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertato.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            If e.Item.Cells(3).Text.Contains("DA DICHIARAZIONE") = False Then
    '                If e.Item.Cells(33).Text = ObjArticolo.PARTEFISSA Or e.Item.Cells(33).Text = ObjArticolo.PARTECONFERIMENTI Then
    '                    Dim lblLegame As Label
    '                    Dim chkSanzioni As CheckBox
    '                    Dim idSanzioni As String
    '                    Dim check As CheckBox
    '                    Dim ChkInteressi As CheckBox
    '                    Dim IdProgressivo As Integer
    '                    Dim intXsanzioni As Integer
    '                    Dim IdLegame As Integer
    '                    Dim obj As ObjArticoloAccertamento
    '                    Dim objXsanzioni As ObjArticoloAccertamento()
    '                    Dim bFound As Boolean = False

    '                    obj = CType(e.Item.DataItem, ObjArticoloAccertamento)
    '                    IdLegame = obj.IdLegame
    '                    IdProgressivo = obj.Progressivo

    '                    lblLegame = e.Item.Cells(26).FindControl("lblLegame")
    '                    chkSanzioni = e.Item.Cells(25).FindControl("chkSanzioni")
    '                    ChkInteressi = e.Item.Cells(25).FindControl("ChkInteressi")

    '                    idSanzioni = obj.Sanzioni
    '                    If idSanzioni <> "-1" And idSanzioni <> "" Then
    '                        chkSanzioni.Checked = True
    '                    End If

    '                    intXsanzioni = 0
    '                    If Not Session("oAccertatiGriglia") Is Nothing Then
    '                        objXsanzioni = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
    '                        For intXsanzioni = 0 To objXsanzioni.Length - 1
    '                            If objXsanzioni(intXsanzioni).IdLegame = IdLegame And objXsanzioni(intXsanzioni).Progressivo = IdProgressivo Then
    '                                bFound = True
    '                                Exit For
    '                            Else
    '                                bFound = False
    '                            End If
    '                        Next
    '                        If Not bFound Then
    '                            ReDim Preserve objXsanzioni(objXsanzioni.Length)
    '                            objXsanzioni(objXsanzioni.Length - 1) = obj
    '                        End If
    '                    Else
    '                        ReDim Preserve objXsanzioni(intXsanzioni)
    '                        objXsanzioni(intXsanzioni) = obj
    '                    End If
    '                    Session.Add("oAccertatiGriglia", objXsanzioni)

    '                    e.Item.Cells(25).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & IdLegame & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
    '                    e.Item.Cells(30).Attributes.Add("onClick", "ApriModificaImmobileAnater('" & IdProgressivo & "')")
    '                Else
    '                    e.Item.Attributes.Add("onClick", "alert('Per modificare questa partita agire sugli immobili.')")
    '                End If
    '            Else
    '                e.Item.Attributes.Add("onClick", "alert('Partita non modificabile!')")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdAccertato_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.UpdateCommand
    '    Dim oAccertamentoUpdate() As ObjArticoloAccertamento
    '    Dim oAccertamentoReturn() As ObjArticoloAccertamento
    '    Dim intArticoliUpdate As Integer
    '    'Prendo l'idLegame
    '    Dim IdLegameGrid As TextBox = e.Item.Cells(0).FindControl("txtLegame")
    '    Dim IdGridUpdate As String = e.Item.Cells(0).Text

    '    Try
    '        If IdLegameGrid.Text = "" Then
    '            dim sScript as string=""
    '            sscript+= "msgLegameVuoto();" & vbCrLf
    '            RegisterScript(sScript , Me.GetType())
    '        Else
    '            oAccertamentoUpdate = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())

    '            Dim arrayListImmobili As New ArrayList
    '            Dim oAccertamentoSingolo As ObjArticoloAccertamento
    '            oAccertamentoSingolo = New ObjArticoloAccertamento

    '            For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
    '                If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
    '                    oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
    '                End If
    '            Next

    '            GrdAccertato.EditItemIndex = -1
    '            GrdAccertato.DataSource = oAccertamentoUpdate
    '            GrdAccertato.DataBind()
    '            Session("oAccertatiGriglia") = oAccertamentoUpdate
    '        End If
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_Update.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdAccertato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.DeleteCommand
    '    Try
    '        Dim oAccertamentoDelete() As ObjArticoloAccertamento
    '        Dim oAccertamentoReturn() As ObjArticoloAccertamento

    '        Dim IdGridDelete As String = e.Item.Cells(0).Text
    '        Dim nMq As Double = 0
    '        Dim myArticolo As New ObjArticolo
    '        Dim oListArticoli() As ObjArticolo
    '        Dim x As Integer
    '        Dim myArray As New ArrayList
    '        Dim myArtAcc As New ObjArticoloAccertamento
    '        Dim FncAcc As New ClsGestioneAccertamenti
    '        Dim FncRicalcolo As New OPENgovPROVVEDIMENTI.ClsElabRuolo
    '        Dim sScript As String = ""
    '        Dim objHashTable As Hashtable = Session("HashTableDichAccertamentiTARSU")

    '        oAccertamentoDelete = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())

    '        If Not oAccertamentoDelete Is Nothing Then
    '            For Each myArt As ObjArticoloAccertamento In oAccertamentoDelete
    '                If myArt.Progressivo <> IdGridDelete Then
    '                    myArray.Add(myArt)
    '                End If
    '            Next
    '            oAccertamentoReturn = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())

    '            oListArticoli = FncAcc.PrepareArticoliForCalcolo(oAccertamentoReturn, myArticolo, objHashTable("TipoTassazione"), False)
    '            If FncRicalcolo.RicalcoloAvviso(oListArticoli, sScript, objHashTable("TipoTassazione"), objHashTable("HasMaggiorazione"), objHashTable("HasConferimenti")) = False Then
    '                RegisterScript(sScript , Me.GetType())
    '                Exit Sub
    '            End If

    '            Dim CurrentItem As New ObjArticoloAccertamento
    '            myArray.Clear()
    '            'prelevo gli articoli determinati dal ricalcolo
    '            oListArticoli = Session("oListArticoli")
    '            For Each myArticolo In oListArticoli
    '                CurrentItem = FncAcc.ArticoloTOArticoloAccertamento(myArticolo, True)
    '                If Not CurrentItem Is Nothing Then
    '                    CurrentItem.IdEnte = ConstSession.IdEnte
    '                    If Not oAccertamentoDelete Is Nothing Then
    '                        For Each myArtAcc In oAccertamentoDelete
    '                            If CurrentItem.Id = myArtAcc.Progressivo Then
    '                                CurrentItem.Sanzioni = myArtAcc.Sanzioni
    '                                CurrentItem.sDescrSanzioni = myArtAcc.sDescrSanzioni
    '                                CurrentItem.Calcola_Interessi = myArtAcc.Calcola_Interessi
    '                            End If
    '                        Next
    '                    End If
    '                Else
    '                    Throw New Exception("errore in caricamento articolo")
    '                End If
    '                myArray.Add(CurrentItem)
    '            Next
    '            oAccertamentoDelete = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
    '            Session("oAccertatiGriglia") = oAccertamentoDelete

    '            Dim ListPartiteAccertato() As ObjArticoloAccertamento
    '            'la griglia deve essere popolata con le partite di calcolo + le partite negative dell'emesso
    '            If Not IsNothing(Session("oAccertatiDaDichiarazione")) Then
    '                oAccertamentoDelete = Session("oAccertatiDaDichiarazione")
    '                For Each oAccertamento As ObjArticoloAccertamento In oAccertamentoDelete
    '                    myArray.Add(oAccertamento)
    '                Next
    '            End If
    '            ListPartiteAccertato = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
    '            'nel calcolo TARES il progressivo ed il legame non hanno + senso quindi sono dei semplici contatori
    '            If objHashTable("TipoTassazione") = ObjRuolo.TipoCalcolo.TARES Then
    '                Dim nCount As Integer = 0
    '                For Each oAccertamento As ObjArticoloAccertamento In ListPartiteAccertato
    '                    nCount += 1
    '                    oAccertamento.Progressivo = nCount
    '                    oAccertamento.IdLegame = nCount
    '                Next
    '            End If
    '            Session("PartiteAccertato") = ListPartiteAccertato
    '            GrdAccertato.start_index = 0
    '            GrdAccertato.DataSource = ListPartiteAccertato
    '            GrdAccertato.DataBind()
    '            Select Case CInt(GrdAccertato.Rows.Count)
    '                Case 0
    '                    GrdAccertato.Visible = False
    '                    'lblMessage.Text = "Non sono presenti Immobili Accertati"
    '                Case Is > 0
    '                    GrdAccertato.Visible = True
    '                    If objHashTable("TipoTassazione") <> ObjRuolo.TipoCalcolo.TARES Then
    '                        GrdAccertato.Columns(10).Visible = False
    '                        GrdAccertato.Columns(11).Visible = False
    '                        GrdAccertato.Columns(12).Visible = False
    '                    End If
    '            End Select

    '            ''la griglia deve essere popolata con le partite di calcolo + le partite negative dell'emesso
    '            'Dim myArray As New ArrayList
    '            'Dim oAccertamentoDelete As New ObjArticoloAccertamento
    '            'For Each oAccertamentoDelete In oAccertamentoReturn
    '            '    myArray.Add(oAccertamentoDelete)
    '            'Next
    '            'If Not IsNothing(Session("oAccertatiDaDichiarazione")) Then
    '            '    oAccertamentoReturn = Session("oAccertatiDaDichiarazione")
    '            '    For Each oAccertamentoDelete In oAccertamentoReturn
    '            '        myArray.Add(oAccertamentoDelete)
    '            '    Next
    '            'End If
    '            'oAccertamentoReturn = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
    '            'If Not oAccertamentoReturn Is Nothing Then
    '            '    GrdAccertato.EditItemIndex = -1
    '            '    GrdAccertato.DataSource = oAccertamentoReturn
    '            '    GrdAccertato.DataBind()
    '            '    Session("oAccertatiGriglia") = oAccertamentoReturn
    '            'Else
    '            '    GrdAccertato.EditItemIndex = -1
    '            '    GrdAccertato.DataSource = Nothing
    '            '    GrdAccertato.DataBind()
    '            '    GrdAccertato.Style.Add("display", "none")
    '            '    Session("oAccertatiGriglia") = Nothing
    '            'End If
    '        Else
    '            GrdAccertato.EditItemIndex = -1
    '            GrdAccertato.DataSource = Nothing
    '            GrdAccertato.DataBind()
    '            GrdAccertato.Style.Add("display", "none")
    '            Session("oAccertatiGriglia") = Nothing
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_DeleteCommand.errore: ", ex))
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdAccertato_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.EditCommand
    '    GrdAccertato.EditItemIndex = e.Item.ItemIndex
    '    GrdAccertato.DataSource = Session("oAccertatiGriglia")
    '    GrdAccertato.DataBind()
    'End Sub

    'Protected Sub GrdAccertato_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.CancelCommand
    'Try
    '    If Not Session("oAccertatiGriglia") Is Nothing Then
    '        GrdAccertato.EditItemIndex = -1
    '        GrdAccertato.DataSource = Session("oAccertatiGriglia")
    '        GrdAccertato.DataBind()
    '    End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_Cancel.errore: ", ex))
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    'Protected Sub GrdAccertato_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.UpdateCommand
    '    Dim oAccertamentoUpdate() As OggettoArticoloRuolo
    '    Dim oAccertamentoReturn() As OggettoArticoloRuolo
    '    'Dim oAccertamentoUpdate() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertamentoReturn() As OggettoArticoloRuoloAccertamento
    '    Dim intArticoliUpdate As Integer
    '    'Prendo l'idLegame
    '    Dim IdLegameGrid As TextBox = e.Item.Cells(0).FindControl("txtLegame")
    '    Dim IdGridUpdate As String = e.Item.Cells(0).Text
    'Try
    '    If IdLegameGrid.Text = "" Then
    '        dim sScript as string=""
    '        sscript+= "msgLegameVuoto();" & vbCrLf
    '        RegisterScript(sScript , Me.GetType())
    '    Else
    '        'oAccertamentoUpdate = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
    '        oAccertamentoUpdate = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())

    '        Dim arrayListImmobili As New ArrayList
    '        Dim oAccertamentoSingolo As OggettoArticoloRuolo
    '        oAccertamentoSingolo = New OggettoArticoloRuolo
    '        'Dim oAccertamentoSingolo As OggettoArticoloRuoloAccertamento
    '        'oAccertamentoSingolo = New OggettoArticoloRuoloAccertamento

    '        For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
    '            If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
    '                oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
    '            End If
    '        Next

    '        GrdAccertato.EditItemIndex = -1
    '        GrdAccertato.DataSource = oAccertamentoUpdate
    '        GrdAccertato.DataBind()
    '        Session("oAccertatiGriglia") = oAccertamentoUpdate
    '    End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_Update.errore: ", ex))
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Sub GrdAccertato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.DeleteCommand
    '    Dim oAccertamentoDelete() As OggettoArticoloRuolo
    '    Dim oAccertamentoReturn() As OggettoArticoloRuolo
    '    'Dim oAccertamentoDelete() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertamentoReturn() As OggettoArticoloRuoloAccertamento

    '    Dim intArticoliDelete As Integer
    '    Dim intArticoliReturn As Integer

    '    Dim IdGridDelete As String = e.Item.Cells(0).Text

    '    oAccertamentoDelete = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
    '    'oAccertamentoDelete = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
    'Try
    '    intArticoliReturn = -1
    '    If Not oAccertamentoDelete Is Nothing Then
    '        For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
    '            If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
    '                intArticoliReturn += 1
    '                ReDim Preserve oAccertamentoReturn(intArticoliReturn)
    '                oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
    '            End If
    '        Next

    '        If Not oAccertamentoReturn Is Nothing Then
    '            GrdAccertato.EditItemIndex = -1
    '            GrdAccertato.DataSource = oAccertamentoReturn
    '            GrdAccertato.DataBind()
    '            Session("oAccertatiGriglia") = oAccertamentoReturn
    '        Else
    '            GrdAccertato.EditItemIndex = -1
    '            GrdAccertato.DataSource = Nothing
    '            GrdAccertato.DataBind()
    '            GrdAccertato.Style.Add("display", "none")
    '            Session("oAccertatiGriglia") = Nothing
    '        End If
    '    Else
    '        GrdAccertato.EditItemIndex = -1
    '        GrdAccertato.DataSource = Nothing
    '        GrdAccertato.DataBind()
    '        GrdAccertato.Style.Add("display", "none")
    '        Session("oAccertatiGriglia") = Nothing
    '    End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.GrdAccertato_DeleteCommand.errore: ", ex))
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub

    'Private Sub btnAccertamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
    '    Dim oAccertato() As OggettoArticoloRuolo
    '    Dim oDichiarato() As OggettoArticoloRuolo
    '    'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '    'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
    '    Dim intAccertato, intDichiarato As Integer
    '    Dim sDescTipoAvviso, sScript As String
    '    Dim a As OggettoAttoTARSU
    '    Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
    '    Dim objHashTable As Hashtable
    '    objHashTable = Session("HashTableDichAccertamentiTARSU")

    '    Dim WFSessione As CreateSessione
    '    Dim strWFErrore As String

    '    Try
    '        WFSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuolo())
    '        oDichiarato = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuolo())
    '        'oAccertato = CType(Session("oAccertatiGriglia"), OggettoArticoloRuoloAccertamento())
    '        'oDichiarato = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuoloAccertamento())

    '        If Not oAccertato Is Nothing Then
    '            'controllo presenza legame Accertato
    '            For intAccertato = 0 To oAccertato.Length - 1
    '                If oAccertato(intAccertato).IdLegame = 0 Then
    '                    Dim str1 As String
    '                    str1 = "<script>"
    '                    str1 = str1 & "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
    '                    str1 = str1 & "</script>"
    '                    RegisterScript(sScript , Me.GetType())
    '                    Exit Sub
    '                End If
    '            Next
    '        End If
    '        If Not oDichiarato Is Nothing Then
    '            'controllo presenza legame Dichiarato
    '            For intDichiarato = 0 To oDichiarato.Length - 1
    '                If oDichiarato(intDichiarato).IdLegame = 0 Then
    '                    Dim str2 As String

    '                    str2 = "<script>"
    '                    str2 = str2 & "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
    '                    str2 = str2 & "</script>"
    '                    RegisterScript(sScript , Me.GetType())
    '                    Exit Sub
    '                End If
    '            Next
    '        End If
    '        If Not oAccertato Is Nothing Then
    '            'controllo coerenza date Accertato
    '            Dim bError As Boolean = False
    '            For intAccertato = 0 To oAccertato.Length - 1
    '                'se l'anno della data fine<>anno accertamento restituisco l'errore
    '                If oAccertato(intAccertato).DataInizio.Year > CInt(objHashTable("ANNOACCERTAMENTO")) Then
    '                    bError = True
    '                ElseIf oAccertato(intAccertato).DataInizio.Year < CInt(objHashTable("ANNOACCERTAMENTO")) Then
    '                    oAccertato(intAccertato).DataInizio = CDate("01/01/" & oAccertato(intAccertato).Anno)
    '                End If
    '                'Forzo la data fine a fine anno accertamento 
    '                If oAccertato(intAccertato).DataFine = Date.MinValue Or oAccertato(intAccertato).DataFine.ToString() = "" Then
    '                    oAccertato(intAccertato).DataFine = CDate("31/12/" & oAccertato(intAccertato).Anno)
    '                End If
    '            Next
    '            If bError Then
    '                Dim str1 As String
    '                str1 = "<script>"
    '                str1 = str1 & "alert('Impossibile proseguire! Verificare che la data Inizio Immobile sia coerente con l\'anno di accertamento!');"
    '                str1 = str1 & "</script>"
    '                RegisterScript(sScript , Me.GetType())
    '                Exit Sub
    '            End If

    '            a = FncAnnulloNoAcc.ConfrontoAccertatoDichiarato(ConstSession.IdEnte, WFSessione, objHashTable("ANNOACCERTAMENTO"), oDichiarato, oAccertato, CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableDichAccertamentiTARSU"), Session("CalcolaAddizionali"), Session("DataSetSanzioni"), Session("VALORE_RITORNO_ACCERTAMENTO"), sDescTipoAvviso, sScript)
    '            Session("TipoAvviso") = sDescTipoAvviso
    '            If sScript <> "" Then
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '        End If

    '    Catch ex As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.btnAccertamento_Click.errore: ", ex))
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAccertamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
        Dim objHashTable As Hashtable

        Try
            objHashTable = Session("HashTableDichAccertamentiTARSU")
            If objHashTable("TipoTassazione") = ObjRuolo.TipoCalcolo.TARES Then
                AccertaTARES(objHashTable)
            Else
                AccertaTARSU(objHashTable)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.btnAccertamento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per la creazione di atti di accertamento TARSU
    ''' </summary>
    ''' <param name="objHashTable"></param>
    Private Sub AccertaTARSU(ByVal objHashTable As Hashtable)
        Dim oAccertato() As ObjArticoloAccertamento
        Dim oDichiarato() As ObjArticoloAccertamento
        Dim intAccertato, intDichiarato As Integer
        Dim sDescTipoAvviso, sScript As String
        Dim myAtto As New OggettoAttoTARSU
        Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
        Dim oDettaglioAnagrafica As New AnagInterface.DettaglioAnagrafica

        Try
            oDettaglioAnagrafica = Session("codContribuente")
            oAccertato = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
            oDichiarato = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

            If Not oAccertato Is Nothing Then
                'controllo presenza legame Accertato
                For intAccertato = 0 To oAccertato.Length - 1
                    If oAccertato(intAccertato).IdLegame = 0 Then
                        sScript += "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                Next
            End If
            If Not oDichiarato Is Nothing Then
                'controllo presenza legame Dichiarato
                For intDichiarato = 0 To oDichiarato.Length - 1
                    If oDichiarato(intDichiarato).IdLegame = 0 Then
                        sScript += "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Valorizzare tutti i legami!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                Next
            End If
            If Not oAccertato Is Nothing Then
                'controllo coerenza date Accertato
                Dim bError As Boolean = False
                For intAccertato = 0 To oAccertato.Length - 1
                    'se l'anno della data fine<>anno accertamento restituisco l'errore
                    If oAccertato(intAccertato).tDataInizio.Year > CInt(objHashTable("ANNOACCERTAMENTO")) Then
                        bError = True
                    ElseIf oAccertato(intAccertato).tDataInizio.Year < CInt(objHashTable("ANNOACCERTAMENTO")) Then
                        oAccertato(intAccertato).tDataInizio = CDate("01/01/" & oAccertato(intAccertato).sAnno)
                    End If
                    'Forzo la data fine a fine anno accertamento 
                    If oAccertato(intAccertato).tDataFine = Date.MinValue Or oAccertato(intAccertato).tDataFine.ToString() = "" Then
                        oAccertato(intAccertato).tDataFine = CDate("31/12/" & oAccertato(intAccertato).sAnno)
                    End If
                Next
                If bError Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Verificare che la data Inizio Immobile sia coerente con l\'anno di accertamento!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If

                myAtto = FncAnnulloNoAcc.TARSUConfrontoAccertatoDichiarato(objHashTable("TipoTassazione"), ConstSession.IdEnte, objHashTable("ANNOACCERTAMENTO"), oDichiarato, oAccertato, CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableDichAccertamentiTARSU"), Session("CalcolaAddizionali"), Session("DataSetSanzioni"), False, oDettaglioAnagrafica.datamorte, sDescTipoAvviso, sScript)
                If IsNothing(myAtto) Then
                    Throw New Exception("Errore in calcolo accertamento")
                Else
                    Session("TipoAvviso") = sDescTipoAvviso
                    If sScript <> "" Then
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.AccertaTARSU.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("AccertaTARSU::" & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per la creazione di atti di accertamento TARES
    ''' </summary>
    ''' <param name="objHashTable"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub AccertaTARES(ByVal objHashTable As Hashtable)
        Dim oAccertato() As ObjArticoloAccertamento
        Dim oDichiarato() As ObjArticoloAccertamento
        Dim intAccertato As Integer
        Dim sDescTipoAvviso, sScript As String
        Dim myAtto As New OggettoAttoTARSU
        Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti

        Try
            Dim oDettaglioAnagrafica As New AnagInterface.DettaglioAnagrafica

            oDettaglioAnagrafica = Session("codContribuente")
            sScript = "" : sDescTipoAvviso = ""
            oAccertato = CType(Session("PartiteAccertato"), ObjArticoloAccertamento())
            oDichiarato = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

            If Not oAccertato Is Nothing Then
                'controllo coerenza date Accertato
                Dim bError As Boolean = False
                For intAccertato = 0 To oAccertato.Length - 1
                    'se l'anno della data fine<>anno accertamento restituisco l'errore
                    If oAccertato(intAccertato).tDataInizio.Year > CInt(objHashTable("ANNOACCERTAMENTO")) Then
                        bError = True
                    ElseIf oAccertato(intAccertato).tDataInizio.Year < CInt(objHashTable("ANNOACCERTAMENTO")) Then
                        oAccertato(intAccertato).tDataInizio = CDate("01/01/" & oAccertato(intAccertato).sAnno)
                    End If
                    'Forzo la data fine a fine anno accertamento 
                    If oAccertato(intAccertato).tDataFine = Date.MinValue Or oAccertato(intAccertato).tDataFine.ToString() = "" Then
                        oAccertato(intAccertato).tDataFine = CDate("31/12/" & oAccertato(intAccertato).sAnno)
                    End If
                Next
                If bError Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Verificare che la data Inizio Immobile sia coerente con l\'anno di accertamento!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If

                myAtto = FncAnnulloNoAcc.TARSUConfrontoAccertatoDichiarato(objHashTable("TipoTassazione"), ConstSession.IdEnte, objHashTable("ANNOACCERTAMENTO"), oDichiarato, oAccertato, CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableDichAccertamentiTARSU"), Session("CalcolaAddizionali"), Session("DataSetSanzioni"), False,odettaglioanagrafica.datamorte, sDescTipoAvviso, sScript)
                If IsNothing(myAtto) Then
                    Throw New Exception("Errore in calcolo accertamento")
                Else
                    Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                    fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "AccertaTARES", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, myAtto.ID_PROVVEDIMENTO)
                    Session("TipoAvviso") = sDescTipoAvviso
                    If sScript <> "" Then
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.AccertaTARES.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("AccertaTARSU::" & ex.Message)
        End Try
    End Sub
    'Private Sub AccertaTARES(ByVal objHashTable As Hashtable)
    '    Dim oAccertato() As ObjArticoloAccertamento
    '    Dim oDichiarato() As ObjArticoloAccertamento
    '    Dim intAccertato As Integer
    '    Dim sDescTipoAvviso, sScript As String
    '    Dim myAtto As New OggettoAttoTARSU
    '    Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti

    '    Try
    '        oAccertato = CType(Session("PartiteAccertato"), ObjArticoloAccertamento())
    '        oDichiarato = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

    '        If Not oAccertato Is Nothing Then
    '            'controllo coerenza date Accertato
    '            Dim bError As Boolean = False
    '            For intAccertato = 0 To oAccertato.Length - 1
    '                'se l'anno della data fine<>anno accertamento restituisco l'errore
    '                If oAccertato(intAccertato).tDataInizio.Year > CInt(objHashTable("ANNOACCERTAMENTO")) Then
    '                    bError = True
    '                ElseIf oAccertato(intAccertato).tDataInizio.Year < CInt(objHashTable("ANNOACCERTAMENTO")) Then
    '                    oAccertato(intAccertato).tDataInizio = CDate("01/01/" & oAccertato(intAccertato).sAnno)
    '                End If
    '                'Forzo la data fine a fine anno accertamento 
    '                If oAccertato(intAccertato).tDataFine = Date.MinValue Or oAccertato(intAccertato).tDataFine.ToString() = "" Then
    '                    oAccertato(intAccertato).tDataFine = CDate("31/12/" & oAccertato(intAccertato).sAnno)
    '                End If
    '            Next
    '            If bError Then
    '                sScript += "GestAlert('a', 'warning', '', '', 'Impossibile proseguire! Verificare che la data Inizio Immobile sia coerente con l\'anno di accertamento!');"
    '                RegisterScript(sScript, Me.GetType())
    '                Exit Sub
    '            End If

    '            myAtto = FncAnnulloNoAcc.ConfrontoAccertatoDichiarato(objHashTable("TipoTassazione"), ConstSession.IdEnte, objHashTable("ANNOACCERTAMENTO"), oDichiarato, oAccertato, CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableDichAccertamentiTARSU"), Session("CalcolaAddizionali"), Session("DataSetSanzioni"), False, sDescTipoAvviso, sScript)
    '            If IsNothing(myAtto) Then
    '                Throw New Exception("Errore in calcolo accertamento")
    '            Else
    '                Session("TipoAvviso") = sDescTipoAvviso
    '                If sScript <> "" Then
    '                    RegisterScript(sScript, Me.GetType())
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.AccertaTARES.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Throw New Exception("AccertaTARSU::" & ex.Message)
    '    End Try
    'End Sub
    '*** ***

    'Protected Function annoBarra(ByVal objtemp As Object) As String
    '    Dim clsGeneralFunction As New myUtility
    '    Dim strTemp As String = ""
    'Try
    '    If Not IsDBNull(objtemp) Then

    '        If CDate(objtemp).Date = Date.MinValue.Date Or CDate(objtemp).Date = Date.MaxValue.Date Then
    '            strTemp = ""
    '        Else
    '            Dim MiaData As String = CType(objtemp, DateTime).ToString("yyyy/MM/dd")
    '            strTemp = clsGeneralFunction.GiraDataCompletaFromDB(MiaData)
    '        End If
    '    End If
    '    Return strTemp
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.annoBarra.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    ' End Try
    'End Function



    'Public Function FormattaNumeriGrd(ByVal iNumGrd As Integer) As String
    'Try
    '    If iNumGrd <= 0 Then
    '        Return ""
    '    Else
    '        Return iNumGrd.ToString
    '    End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.FormattaNumeroGrid.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    ' End Try
    'End Function

    'Public Function ConfrontoAccertatoDichiarato_old(ByVal oDichiarato() As OggettoArticoloRuolo, ByVal oAccertato() As OggettoArticoloRuolo) As OggettoAttoTARSU
    'Dim x, y, nList, i As Integer
    'Dim nLegamePrec As Integer
    'Dim oDettaglioAtto() As OggettoDettaglioAtto
    'Dim oListDettaglioAtto As OggettoDettaglioAtto

    'Dim ImpTotAccert As Double = 0
    'Dim ImpTotDich As Double = 0
    'Dim oAtto As OggettoAttoTARSU
    'Dim oPagatoTARSU() As OggettoPagamenti
    'Dim ClsPagato As New OPENgovTARSU.ClsPagamenti
    'Dim oClsRuolo As New OPENgovTARSU.ClsRuolo
    'Dim oClsAddizionali As New OPENgovTARSU.ClsAddizionali

    'Dim DiffTotaleSanzioni As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    'Dim DiffTotaleInteressi As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    'Dim DiffTotaleSanzInt As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso

    'Dim ImportoTotaleSanzioniAccertato As Double
    'Dim ImportoTotaleInteressiAccertato As Double
    'Dim queryResult As Boolean
    'Dim OggettoRiepilogoAccertamento As OggettoArticoloRuolo()
    'Dim blnCalcolaInteressi As Boolean

    'Dim iID_PROVVEDIMENTO As Integer

    'Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")

    ''Dim objSessione As CreateSessione

    'Try
    '    Dim culture As IFormatProvider

    '    culture = New CultureInfo("it-IT", True)

    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    If Not oAccertato Is Nothing Then
    '        '**********************************************************************
    '        'Determino il Tipo di Avviso (Accertamento o Ufficio)
    '        'Se trovo 1 immobile di accertato che non è in dichiarato
    '        'ho avviso di Tipo = 4 altrimenti avviso di tipo 3
    '        '**********************************************************************
    '        Dim DichiaratoNothing As New OggettoArticoloRuolo
    '        If oDichiarato Is Nothing Then
    '            ReDim Preserve oDichiarato(0)
    '            oDichiarato(0) = DichiaratoNothing
    '        End If
    '        Dim TipoAvviso As Integer

    '        TipoAvviso = Costanti.PROVVEDIMENTO_ACCERTAMENTO_RETTIFICA   '4

    '        'Prendo immobili di accertato
    '        For i = 0 To oAccertato.Length - 1
    '            Dim Trovato As Boolean = False
    '            'Cerco l'immobili in tutti gli immobili di dichiarato'
    '            'Se lo trovo esco dal ciclo e proseguo a cercare con immobili
    '            'successivo di accertamento
    '            For y = 0 To oDichiarato.Length - 1
    '                If oAccertato(i).IdLegame = oDichiarato(y).IdLegame Then
    '                    'devo aggiornare FOGLIO, NUMERO, SUBALTERNO dell'accertato con quelli del dichiarato
    '                    oAccertato(i).Foglio = oDichiarato(y).Foglio
    '                    oAccertato(i).Numero = oDichiarato(y).Numero
    '                    oAccertato(i).Subalterno = oDichiarato(y).Subalterno
    '                    'devo aggiornare FOGLIO, NUMERO, SUBALTERNO dell'accertato con quelli del dichiarato
    '                    Trovato = True
    '                    Exit For
    '                End If
    '            Next

    '            'Se trovato = False vuol dire che nn ho trovato l'immobile
    '            If Trovato = False Then
    '                'Avviso D'ufficio
    '                TipoAvviso = Costanti.PROVVEDIMENTO_ACCERTAMENTO_UFFICIO    '3
    '                Exit For
    '            End If
    '        Next
    '        '**************************************************************************
    '        'Fine Tipo avviso
    '        '**************************************************************************

    '        nList = -1
    '        '''CONFRONTO ACCERTATO E DICHIARATO'''
    '        'ciclo su tutti i record accertati
    '        For x = 0 To oAccertato.GetUpperBound(0)
    '            'sommo gli importi a parità di livello di legame
    '            If oAccertato(x).IdLegame <> nLegamePrec Then
    '                'aggiungo un record
    '                nList += 1
    '                oListDettaglioAtto = New OggettoDettaglioAtto
    '                oListDettaglioAtto.IdLegame = oAccertato(x).IdLegame
    '                oListDettaglioAtto.Progressivo = oAccertato(x).Progressivo
    '                oListDettaglioAtto.Sanzioni = oAccertato(x).Sanzioni
    '                oListDettaglioAtto.Interessi = oAccertato(x).Interessi
    '                oListDettaglioAtto.Calcola_Interessi = oAccertato(x).Calcola_Interessi
    '            End If
    '            'sommo l'importo
    '            oListDettaglioAtto.ImpAccertato += oAccertato(x).ImportoNetto

    '            ReDim Preserve oDettaglioAtto(nList)
    '            oDettaglioAtto(nList) = oListDettaglioAtto
    '        Next

    '        'ciclo su tutti i record dichiarati
    '        For x = 0 To oDichiarato.GetUpperBound(0)
    '            'cerco il corrispettivo legame nell'oggetto di confronto
    '            For y = 0 To oDettaglioAtto.GetUpperBound(0)
    '                If oDettaglioAtto(y).IdLegame = oDichiarato(x).IdLegame Then
    '                    oDettaglioAtto(y).ImpDichiarato += oDichiarato(x).ImportoNetto
    '                    Exit For
    '                End If
    '            Next
    '        Next

    '        'ciclo sull'oggetto totale per verificare il tipo di atto
    '        nList = -1
    '        For x = 0 To oDettaglioAtto.GetUpperBound(0)
    '            ImpTotAccert += oDettaglioAtto(x).ImpAccertato
    '            ImpTotDich += oDettaglioAtto(x).ImpDichiarato
    '        Next
    '        nList += 1
    '        If ImpTotAccert > ImpTotDich Then
    '            'accertato maggiore dichiarato
    '            oAtto = PopolaAttoTARSU(oDettaglioAtto)
    '            oAtto.sAnno = oAccertato(0).Anno
    '            If Not oAtto Is Nothing Then
    '                ''''SE HO UN ATTO''''
    '                '''CONFRONTO ACCERTATO E PAGATO
    '                '''popolo differenza d'imposta
    '                oPagatoTARSU = ClsPagato.GetPagamenti(oAtto.nIdContribuente, "", oAtto.sAnno)
    '                If Not oPagatoTARSU Is Nothing Then
    '                    Dim IPagato As Integer
    '                    For IPagato = 0 To oPagatoTARSU.Length - 1
    '                        oAtto.ImpPagato += oPagatoTARSU(IPagato).dImportoPagamento
    '                    Next
    '                Else
    '                    oAtto.ImpPagato = 0
    '                End If

    '                Dim TipoAvvisoRimborso As Integer = -1
    '                Dim objHashTable As Hashtable
    '                '*******************************************************************
    '                'Rimborso. Calcolo gli interessi Attivi sul singolo immobile.
    '                'Al giro dopo la var viene azzerrata a -1.
    '                '*******************************************************************
    '                If oAtto.ImpImposta < 0 Then
    '                    TipoAvvisoRimborso = Costanti.PROVVEDIMENTO_RIMBORSO '"5"
    '                End If

    '                Dim objDSCalcoloSanzioniInteressi As DataSet
    '                Dim dtInteressi As New DataTable
    '                Dim dtSanzioni As New DataTable
    '                Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet

    '                If oAtto.ImpImposta <> 0 Then
    '                    '*******************************************************************
    '                    'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo
    '                    'delle sanzioni le calcola solo	se l'importo è positivo)
    '                    '*******************************************************************
    '                    Dim blnResult As Boolean = False
    '                    Dim strWFErrore As String
    '                    Dim strConnectionStringAnagrafica As String
    '                    Dim strConnectionStringTARSU As String
    '                    'Dim strConnectionStringOPENgovProvvedimenti As String = String.Empty

    '                    objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '                    If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '                        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '                    End If

    '                    Dim TotImportoSanzioniACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
    '                    Dim TotImportoSanzioniRidottoACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
    '                    Dim TotImportoInteressiACCERTAMENTO As Double 'Totale Interessi atto di accertamento
    '                    Dim TotDiffImpostaACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto di accertamento
    '                    Dim TotDiffImpostaAccontoACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto ACCONTO di accertamento
    '                    Dim TotDiffImpostaSaldoACCERTAMENTO As Double 'Importo Totale Differenza di imposta SALDO atto di accertamento

    '                    ''*******************************************************************
    '                    'HashTable per calcolo Sanzioni e interessi
    '                    '*******************************************************************
    '                    objHashTable = Session("HashTableDichAccertamentiTARSU")

    '                    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '                    If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = True Then
    '                        objHashTable.Remove("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI")
    '                    End If
    '                    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '                    objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA") = ConfigurationManager.AppSettings("OPENGOVA")
    '                    strConnectionStringAnagrafica = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
    '                    objHashTable("CONNECTIONSTRINGANAGRAFICA") = strConnectionStringAnagrafica
    '                    objHashTable("CODTRIBUTO") = "0434"
    '                    objHashTable("CODENTE") = constsession.idente
    '                    If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
    '                        objHashTable.Remove("TIPOPROVVEDIMENTO")
    '                    End If
    '                    'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso
    '                    'Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no
    '                    'Se è rimborso ho solo interssi attivi
    '                    If TipoAvvisoRimborso = -1 Then
    '                        objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '                    Else
    '                        objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
    '                    End If
    '                    If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
    '                        objHashTable("COD_TIPO_PROCEDIMENTO") = Costanti.PROCEDIMENTO_ACCERTAMENTO
    '                    Else
    '                        objHashTable.Add("COD_TIPO_PROCEDIMENTO", Costanti.PROCEDIMENTO_ACCERTAMENTO)
    '                    End If
    '                    If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
    '                        objHashTable.Add("ANNOACCERTAMENTO", CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"))
    '                    End If
    '                    'PER CONNESSIONE TARSU
    '                    objHashTable("IDSOTTOAPPLICAZIONETARSU") = ConfigurationManager.AppSettings("OPENGOVTA")
    '                    strConnectionStringTARSU = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONETARSU")).GetConnection.ConnectionString
    '                    objHashTable("CONNECTIONSTRINGTARSU") = strConnectionStringTARSU

    '                    '*******************************************************************
    '                    'Fine HashTable
    '                    '*******************************************************************
    '                    Dim xDettaglio As Integer
    '                    Dim objDSSanzioni As DataSet
    '                    Dim objDSInteressi As DataSet

    '                    For xDettaglio = 0 To oDettaglioAtto.Length - 1
    '                        Dim objHashTableDati As New Hashtable
    '                        If InStr(oDettaglioAtto(xDettaglio).Sanzioni, "#") Then
    '                            objHashTableDati.Add("IDSANZIONI", oDettaglioAtto(xDettaglio).Sanzioni)
    '                        Else
    '                            objHashTableDati.Add("IDSANZIONI", oDettaglioAtto(xDettaglio).Sanzioni & "#" & objHashTable("TIPOPROVVEDIMENTO"))
    '                        End If

    '                        'L'Id Immobile è il Progressivo
    '                        objHashTableDati.Add("IDIMMOBILE", oDettaglioAtto(xDettaglio).Progressivo)
    '                        objHashTableDati.Add("IDLEGAME", oDettaglioAtto(xDettaglio).IdLegame)
    '                        '******************************************************************
    '                        'Calcolo le sanzioni per i singoli Immobili
    '                        '******************************************************************
    '                        objDSCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato, 0, 0)
    '                        objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(objHashTable("ANNOACCERTAMENTO"), oDettaglioAtto(xDettaglio).ImpAccertato - oDettaglioAtto(xDettaglio).ImpDichiarato)

    '                        Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                        objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(objHashTable, objHashTableDati, objDSCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio)

    '                        'Creo una copia della struttura
    '                        If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
    '                            dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
    '                            dtSanzioni.Clear()
    '                        End If

    '                        Dim intCount As Integer
    '                        Dim copyRow As DataRow

    '                        For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
    '                            copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
    '                            dtSanzioni.ImportRow(copyRow)
    '                        Next

    '                        Dim objDSMotivazioniSanzioni As DataSet
    '                        objDSMotivazioniSanzioni = Session("DataSetSanzioni")

    '                        If Not objDSMotivazioniSanzioni Is Nothing Then
    '                            For intCount = 0 To objDSMotivazioniSanzioni.Tables(0).Rows.Count - 1
    '                                Dim rows() As DataRow
    '                                rows = dtSanzioni.Select("COD_VOCE='" & objDSMotivazioniSanzioni.Tables(0).Rows(intCount).Item("COD_VOCE") & "'")
    '                                For y = 0 To UBound(rows)
    '                                    rows(y).Item("Motivazioni") = objDSMotivazioniSanzioni.Tables(0).Rows(intCount).Item("MOTIVAZIONE")
    '                                Next
    '                                dtSanzioni.AcceptChanges()
    '                            Next
    '                        End If

    '                        'Aggiorno il DataSet con le sanzioni
    '                        Dim dt As DataTable
    '                        dt = objDSSanzioni.Tables(1)
    '                        objDSCalcoloSanzioniInteressi.Dispose()
    '                        objDSCalcoloSanzioniInteressi = New DataSet
    '                        objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

    '                        'Aggiorno il DS con l'importo delle sanzioni calcolate
    '                        oDettaglioAtto(xDettaglio).ImpSanzioni = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI")
    '                        oDettaglioAtto(xDettaglio).ImpSanzioniRidotto = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO")

    '                        'totale sanzione dell'atto di accertamento
    '                        TotImportoSanzioniACCERTAMENTO = TotImportoSanzioniACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpSanzioni
    '                        TotImportoSanzioniRidottoACCERTAMENTO = TotImportoSanzioniRidottoACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpSanzioniRidotto

    '                        'Somma algebrica per determinare Tipo Avviso
    '                        DiffTotaleSanzioni = oDettaglioAtto(xDettaglio).ImpSanzioni 'objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI")
    '                        DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleSanzioni

    '                        '******************************************************************
    '                        'CALCOLO INTERESSI
    '                        blnCalcolaInteressi = oDettaglioAtto(xDettaglio).Calcola_Interessi

    '                        If blnCalcolaInteressi = True Then
    '                            objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                            objDSInteressi = objCOMDichiarazioniAccertamenti.getInteressiTARSU(objHashTable, objHashTableDati, objDSCalcoloSanzioniInteressi, oDettaglioAtto(xDettaglio).Progressivo, oDettaglioAtto(xDettaglio).IdLegame)

    '                            If Not IsNothing(objDSInteressi.Tables("INTERESSI")) Then
    '                                If dtInteressi.Rows.Count = 0 And objDSInteressi.Tables.Count > 0 Then
    '                                    dtInteressi = objDSInteressi.Tables("INTERESSI").Copy
    '                                    dtInteressi.Clear()
    '                                End If
    '                                For intCount = 0 To objDSInteressi.Tables("INTERESSI").Rows.Count - 1
    '                                    copyRow = objDSInteressi.Tables("INTERESSI").Rows(intCount)
    '                                    dtInteressi.ImportRow(copyRow)
    '                                Next

    '                                'Aggiorno il DataSet con gli interessi
    '                                dt = objDSInteressi.Tables(1)
    '                                objDSCalcoloSanzioniInteressi.Dispose()
    '                                objDSCalcoloSanzioniInteressi = New DataSet
    '                                objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)
    '                            End If
    '                            'Aggiorno il DS con l'importo delle interessi calcolati
    '                            oDettaglioAtto(xDettaglio).ImpInteressi = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI")
    '                            'totale interessi dell'atto di accertamento
    '                            TotImportoInteressiACCERTAMENTO = TotImportoInteressiACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpInteressi

    '                            'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                            DiffTotaleInteressi = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI")
    '                            DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                        Else
    '                            'Aggiorno il DataSet con gli interessi
    '                            objDSCalcoloSanzioniInteressi = New DataSet
    '                            objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)
    '                            oDettaglioAtto(xDettaglio).ImpInteressi = 0
    '                            'totale interessi dell'atto di accertamento
    '                            TotImportoInteressiACCERTAMENTO = TotImportoInteressiACCERTAMENTO + oDettaglioAtto(xDettaglio).ImpInteressi

    '                            'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                            DiffTotaleInteressi = 0
    '                            DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                        End If
    '                    Next

    '                    For i = 0 To oAccertato.Length - 1
    '                        For x = 0 To oDettaglioAtto.Length - 1
    '                            If oAccertato(i).Progressivo = oDettaglioAtto(x).Progressivo Then
    '                                If oAccertato(i).IdLegame = oDettaglioAtto(x).IdLegame Then
    '                                    oAccertato(i).ImpSanzioni = oDettaglioAtto(x).ImpSanzioni
    '                                    oAccertato(i).ImpSanzioniRidotto = oDettaglioAtto(x).ImpSanzioniRidotto
    '                                    oAccertato(i).ImpInteressi = oDettaglioAtto(x).ImpInteressi
    '                                End If
    '                            End If
    '                        Next
    '                    Next
    '                    'GIULIA 09082005
    '                    'reperisco soglia
    '                    '*****************************************************
    '                    Dim soglia As Double
    '                    Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
    '                    soglia = 0
    '                    soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), "0434", objHashTable("CODENTE"), TipoAvviso)
    '                    '******************************************************

    '                    'GIULIA 09082005
    '                    Dim ImportoTotaleAvviso As Double
    '                    ImportoTotaleAvviso = oAtto.ImpImposta + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO

    '                    If Not objDSCalcoloSanzioniInteressi Is Nothing Then
    '                        If objDSCalcoloSanzioniInteressi.Tables.Count > 0 Then
    '                            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = TotImportoSanzioniACCERTAMENTO
    '                            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = TotImportoSanzioniRidottoACCERTAMENTO

    '                            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = TotImportoInteressiACCERTAMENTO
    '                            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = oAtto.ImpImposta
    '                        End If
    '                    End If

    '                    If ImportoTotaleAvviso < 0 Then
    '                        If ImportoTotaleAvviso * (-1) < soglia Then
    '                            'Non emetto Avviso
    '                            Dim str1 As String
    '                            str1 = "<script>"
    '                            str1 = str1 & "alert('Importo Avviso inferiore alla soglia. L\'avviso non verrà emesso')"
    '                            str1 = str1 & "</script>"
    '                            RegisterScript("msgSoglia", str1)
    '                            Exit Function
    '                        End If
    '                    Else
    '                        If ImportoTotaleAvviso = 0 Then
    '                            Session("TipoAvviso") = "Nessun avviso emesso."
    '                            'Non emetto Avviso
    '                            Dim str1 As String
    '                            str1 = "<script>"
    '                            str1 = str1 & "alert('La posizione è corretta, nessun avviso emesso.');"
    '                            str1 = str1 & "RiepilogoAccertato();"
    '                            str1 = str1 & "</script>"
    '                            RegisterScript("msgSoglia", str1)
    '                            Exit Function
    '                        ElseIf ImportoTotaleAvviso < soglia Then
    '                            Session("TipoAvviso") = "Nessun avviso emesso. Importo inferiore alla soglia"
    '                            'Non emetto Avviso
    '                            Dim str1 As String
    '                            str1 = "<script>"
    '                            str1 = str1 & "alert('Importo Avviso inferiore alla soglia. L\'avviso non verrà emesso');"
    '                            str1 = str1 & "RiepilogoAccertato();"
    '                            str1 = str1 & "</script>"
    '                            RegisterScript("msgSoglia", str1)
    '                            'Session.Remove("cercaDichiarato")
    '                            Exit Function
    '                        End If
    '                    End If
    '                    'CONTROLLARE IL TIPO AVVISO
    '                    'Avviso in Rettifica

    '                    'Rimborso
    '                    If ImportoTotaleAvviso < 0 Then
    '                        'If DiffTotaleSanzInt < 0 Then 'GIULIA 09082005
    '                        TipoAvviso = Costanti.PROVVEDIMENTO_RIMBORSO '5
    '                    End If

    '                    'giulia 09082005

    '                    'Calcolo le spese
    '                    Dim spese As Double
    '                    spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), "0434", objHashTable("CODENTE"), TipoAvviso)

    '                    '***************************************************************************
    '                    'PROCEDURA DA TERMINARE EMA 28/12/2007
    '                    '***************************************************************************
    '                    '***************************************************************************
    '                    ' Aggiorno il DB dopo procedura di accertamento

    '                    '***************************************************************************
    '                    '***************************************************************************
    '                    'PROCEDURA DA TERMINARE EMA 28/12/2007
    '                    '***************************************************************************
    '                    '***************************************************************************
    '                    Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)

    '                    Dim ObjAddizionaleServizio() As OggettoAddizionale = Nothing
    '                    '**********ADDIZIONALI***********'
    '                    'estraggo le addizionali che devono essere applicate
    '                    If Session("CalcolaAddizionali") = True Then

    '                        Dim IntAddizionaleServizio As Integer
    '                        Dim objSearch As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '                        Dim DsAddizionali As DataSet
    '                        Dim ArrayList As OggettoAddizionale

    '                        DsAddizionali = New DataSet
    '                        objSearch = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale

    '                        objSearch.sAnno = oAtto.sAnno

    '                        DsAddizionali = oClsAddizionali.GetAddizionaliEnte(objSearch)
    '                        If DsAddizionali Is Nothing Then
    '                            dim sScript as string=""
    '                            sscript+="<script language='javascript'>alert('Non sono state configurate le addizionali!Non si può procedere con la procedura di Accertamento!');</script>"
    '                            RegisterScript("msg", strscript)
    '                            Exit Function
    '                        Else
    '                            Dim IntAddizionale As Integer
    '                            oAtto.ImpAltro = 0
    '                            Dim ImpAddizionale As Double

    '                            For IntAddizionale = 0 To DsAddizionali.Tables(0).Rows.Count - 1
    '                                ImpAddizionale = 0
    '                                ImpAddizionale = CDbl(oAtto.ImpImposta * DsAddizionali.Tables(0).Rows(IntAddizionale).Item("PERCENTUALE") / 100)
    '                                oAtto.ImpAltro += ImpAddizionale
    '                                ArrayList = New OggettoAddizionale

    '                                ArrayList.Anno = DsAddizionali.Tables(0).Rows(IntAddizionale).Item("ANNO")
    '                                ArrayList.CodiceCapitolo = DsAddizionali.Tables(0).Rows(IntAddizionale).Item("IDCAPITOLO")
    '                                ArrayList.idAddizionale = DsAddizionali.Tables(0).Rows(IntAddizionale).Item("IDADDIZIONALE")
    '                                ArrayList.Valore = DsAddizionali.Tables(0).Rows(IntAddizionale).Item("PERCENTUALE")
    '                                ArrayList.ImportoCalcolato = ImpAddizionale
    '                                ArrayList.Descrizione = DsAddizionali.Tables(0).Rows(IntAddizionale).Item("DESCRIZIONE")

    '                                ReDim Preserve ObjAddizionaleServizio(IntAddizionale)

    '                                ObjAddizionaleServizio(IntAddizionale) = ArrayList
    '                            Next
    '                        End If
    '                    End If
    '                    'se il check è selezionato
    '                    'if Parent.
    '                    '********************************************************
    '                    '**********ADDIZIONALI***********'
    '                    If Not objDSSanzioni Is Nothing Then
    '                        objDSSanzioni.Dispose()
    '                    End If
    '                    If Not objDSInteressi Is Nothing Then
    '                        objDSInteressi.Dispose()
    '                    End If

    '                    objDSSanzioni = New DataSet
    '                    objDSInteressi = New DataSet

    '                    objDSSanzioni.Tables.Add(dtSanzioni.Copy)
    '                    objDSInteressi.Tables.Add(dtInteressi.Copy)

    '                    oAtto.CAP_CO = ""
    '                    oAtto.CAP_RES = ""
    '                    oAtto.CITTA_CO = ""
    '                    oAtto.CITTA_RES = ""
    '                    oAtto.CIVICO_CO = ""
    '                    oAtto.CIVICO_RES = ""
    '                    oAtto.CO = ""
    '                    oAtto.CODICE_FISCALE = ""
    '                    oAtto.COGNOME = ""
    '                    oAtto.ESPONENTE_CIVICO_CO = ""
    '                    oAtto.ESPONENTE_CIVICO_RES = ""
    '                    oAtto.FRAZIONE_CO = ""
    '                    oAtto.FRAZIONE_RES = ""
    '                    oAtto.NOME = ""
    '                    oAtto.PARTITA_IVA = ""
    '                    oAtto.POSIZIONE_CIVICO_CO = ""
    '                    oAtto.POSIZIONE_CIVICO_RES = ""
    '                    oAtto.PROVINCIA_CO = ""
    '                    oAtto.PROVINCIA_RES = ""
    '                    oAtto.VIA_CO = ""
    '                    oAtto.VIA_RES = ""
    '                    oAtto.nIdContribuente = CInt(objHashTable("CODCONTRIBUENTE"))

    '                    oAtto.ImpSanzioni = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI"))
    '                    oAtto.ImpSanzioniRid = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO"))
    '                    oAtto.ImpInteressi = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI"))
    '                    oAtto.ImpSpese = spese
    '                    oAtto.ImpTotale = (oAtto.ImpImposta + oAtto.ImpSanzioni + oAtto.ImpSanzioniNonRid + oAtto.ImpInteressi + oAtto.ImpAltro + oAtto.ImpSpese)
    '                    oAtto.ImpArrotondamento = CDbl(ImportoArrotondato(oAtto.ImpTotale)) - oAtto.ImpTotale
    '                    'aggiorno l'importo totale comprensivo di arrotondamento
    '                    oAtto.ImpTotale = oAtto.ImpTotale + oAtto.ImpArrotondamento
    '                    oAtto.ImpTotaleRid = oAtto.ImpImposta + oAtto.ImpSanzioniRid + oAtto.ImpSanzioniNonRid + oAtto.ImpInteressi + oAtto.ImpAltro + oAtto.ImpSpese
    '                    oAtto.ImpArrotondamentoRid = CDbl(ImportoArrotondato(oAtto.ImpTotaleRid)) - oAtto.ImpTotaleRid
    '                    oAtto.ImpTotaleRid = oAtto.ImpTotaleRid + oAtto.ImpArrotondamentoRid
    '                    For i = 0 To oAccertato.Length - 1
    '                        If Year(oAccertato(i).DataInizio) < oAtto.sAnno Then
    '                            oAccertato(i).DataInizio = "01/01/" & oAtto.sAnno
    '                        End If
    '                    Next
    '                    For i = 0 To oDichiarato.Length - 1
    '                        If Year(oDichiarato(i).DataInizio) < oAtto.sAnno Then
    '                            oDichiarato(i).DataInizio = "01/01/" & oAtto.sAnno
    '                        End If
    '                    Next
    '                    If objHashTable.ContainsKey("VALORE_RITORNO_ACCERTAMENTO") = True Then
    '                        objHashTable.Remove("VALORE_RITORNO_ACCERTAMENTO")
    '                    End If

    '                    'If Session("VALORE_RITORNO_ACCERTAMENTO") Then
    '                    'aggiungo anche VALORE_RITORNO_ACCERTAMENTO=0 per creare un accertamento
    '                    'per un anno in cui è presente un acc definitivo.
    '                    objHashTable.Add("VALORE_RITORNO_ACCERTAMENTO", Session("VALORE_RITORNO_ACCERTAMENTO"))
    '                    'End If
    '                    OggettoRiepilogoAccertamento = objCOMUpdateDBAccertamenti.updateDBAccertamentiTARSU(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, oAtto, oDettaglioAtto, oDichiarato, oAccertato, spese, ObjAddizionaleServizio)
    '                    'Aggiorno la Griglia con le sanzioni e interessi

    '                    'recupero id Provvedimento
    '                    iID_PROVVEDIMENTO = OggettoRiepilogoAccertamento(0).Id
    '                    'e lo assegno anche ai singoli immobili
    '                    For i = 0 To oAccertato.Length - 1
    '                        oAccertato(i).Id = iID_PROVVEDIMENTO
    '                    Next

    '                    '***** SOLO PER DEBUG *********
    '                    GrdAccertato.DataSource = oAccertato
    '                    GrdAccertato.DataBind()
    '                    '******************************

    '                    objDSSanzioni.Dispose()
    '                    objDSInteressi.Dispose()
    '                    objDSSanzioni.Dispose()

    '                    '*** 20100930 - NOOOOOOOOO in impaltro ho le addizionali!!! ***
    '                    '***prima di passare l'atto alla videata di riepilogo gli popolo gli importi mancanti***
    '                    'popolando temporaneamente per l'Atto ->ImpAltro=IMPORTO_SANZIONI_RIDOTTO
    '                    'per l'Accertato -> ImportoNetto=ImpAccertato-ImpDichiarato
    '                    'oAtto.ImpSanzioni = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI"))
    '                    ''oAtto.ImpAltro = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO"))
    '                    'oAtto.ImpSanzionirid = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO"))
    '                    'oAtto.ImpInteressi = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI"))
    '                    'oAtto.ImpArrotondamento = CDbl(ImportoArrotondato(oAtto.ImpImposta + oAtto.ImpSanzioni + oAtto.ImpSanzioninonrid + oAtto.ImpInteressi + oAtto.ImpAltro + oAtto.ImpSpese))
    '                    'oAtto.ImpTotale = oAtto.ImpImposta + oAtto.ImpSanzioni + oAtto.ImpSanzioninonrid + oAtto.ImpInteressi + oAtto.ImpAltro + oAtto.ImpSpese + oAtto.ImpArrotondamento
    '                    'oAtto.Impsanzioninonrid = CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO"))
    '                    'oAtto.ImpArrotondamentorid = CDbl(ImportoArrotondato(oAtto.ImpImposta + oAtto.ImpSanzionirid + oAtto.ImpSanzioninonrid + oAtto.ImpInteressi + oAtto.ImpAltro + oAtto.ImpSpese))
    '                    'oAtto.ImpTotaleird = oAtto.ImpImposta + oAtto.ImpSanzionirid + oAtto.ImpSanzioninonrid + oAtto.ImpInteressi + oAtto.ImpAltro + oAtto.ImpSpese + oAtto.ImpArrotondamentorid
    '                    'For i = 0 To oAccertato.Length - 1
    '                    '    For x = 0 To oDettaglioAtto.Length - 1
    '                    '        If oAccertato(i).Progressivo = oDettaglioAtto(x).Progressivo Then
    '                    '            If oAccertato(i).IdLegame = oDettaglioAtto(x).IdLegame Then
    '                    '                oAccertato(i).ImportoNetto = oDettaglioAtto(x).ImpAccertato - oDettaglioAtto(x).ImpDichiarato
    '                    '            End If
    '                    '        End If
    '                    '    Next
    '                    'Next
    '                    Session.Add("oSituazioneAtto", oAtto)
    '                    Session.Add("oSituazioneDichiarato", oDichiarato)
    '                    Session.Add("oSituazioneAccertato", oAccertato)
    '                    '******
    '                    Dim str As String
    '                    str = "<script>"
    '                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                        str = str & "FineElaborazioneAccertamento();" & vbCrLf
    '                    End If
    '                    str = str & "RiepilogoAccertato()"
    '                    str = str & "</script>"
    '                    RegisterScript("msgRiepilogo", str)
    '                    'Elimino la varibile che mi dice che ho cercato in dichiarato
    '                    Session.Remove("cercaDichiarato")

    '                    If TipoAvviso = Costanti.PROVVEDIMENTO_RIMBORSO Then '5 Then
    '                        Session("TipoAvviso") = "Avviso di rimborso"
    '                    ElseIf TipoAvviso = Costanti.PROVVEDIMENTO_ACCERTAMENTO_RETTIFICA Then '"4" Then
    '                        Session("TipoAvviso") = "Avviso di accertamento in rettifica" '"Avviso di rettifica"
    '                    Else
    '                        Session("TipoAvviso") = "Avviso di accertamento d'ufficio" '"Avviso d'ufficio"
    '                    End If
    '                    txtRiaccerta.Text = "0"
    '                Else
    '                    '*******************************************************************
    '                    'Se sono qui l'ici dichiarato è uguale a quello accertato
    '                    'Sanzioni e Interessi a 0
    '                    '*******************************************************************
    '                    oAtto.ImpInteressi = 0
    '                    oAtto.ImpSanzioni = 0
    '                End If
    '            Else
    '                Return Nothing
    '            End If
    '        ElseIf ImpTotAccert < ImpTotDich Then
    '            'Non emetto Avviso
    '            Dim strmsg As String
    '            strmsg = "<script>"
    '            strmsg = strmsg & "alert('Importo Accertato inferiore all\' Importo Dichiarato. L\'avviso non verrà emesso');"
    '            strmsg = strmsg & "</script>"
    '            RegisterScript("msgImpTotAccert", strmsg)
    '            Exit Function
    '            'accertato minore dichiarato
    '        End If
    '    Else
    '        'manca accertato
    '        Return Nothing
    '    End If
    'Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertatiTARSU.ConfrontoAccertatoDichiarato_old.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'Finally
    '    If Not IsNothing(objSessione) Then
    '        objSessione.Kill()
    '        objSessione = Nothing
    '    End If
    'End Try
    'End Function
End Class
