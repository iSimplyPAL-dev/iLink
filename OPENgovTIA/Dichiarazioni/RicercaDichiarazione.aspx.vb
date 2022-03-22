Imports System.IO
Imports System.Net
Imports ICSharpCode.SharpZipLib.Zip
Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility
''' <summary>
''' Pagina per la ricerca dichiarazioni.
''' Contiene i parametri di ricerca e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaDichiarazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("RicercaDichiarazione")
    Public UrlStradario As String = ConstSession.UrlStradario
    Private myObjSearch As New ObjSearchTestata

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
        Dim sScript As String = ""
        Dim sOrigineRichiamo As String = ""

        Try
            LblFileToDownload.Attributes.Add("onclick", "CmdScarica.click()")
            If TxtViaRibaltata.Text <> "" And TxtViaRibaltata.Text <> " " Then
                TxtVia.Text = TxtViaRibaltata.Text
            Else
                TxtVia.Text = ""
            End If
            ClearSession()
            If Not Request.Item("IsFromVariabile") Is Nothing Then
                If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                End If
            End If
            Log.Debug("RicercaDichiarazione::IsFromVariabile::" & ConstSession.IsFromVariabile)
            Log.Debug("RicercaDichiarazione::DB::" & ConstSession.StringConnection)
            AddKeyPress()
            LoadPageCombos(Page.IsPostBack)
            If Page.IsPostBack = False Then
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
                LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
                If Not Request.Item("Org") Is Nothing Then
                    sOrigineRichiamo = Request.Item("Org").ToString
                End If
                If sOrigineRichiamo = "GIS" Then
                    RbImmobile.Checked = True
                    RbSoggetto.Checked = False
                    PanelImmobile.Visible = True
                    PanelSoggetto.Visible = False
                    Dim sRif() As String
                    sRif = Request.Item("RifCat").ToString.Split("-")
                    TxtFoglio.Text = sRif(0)
                    TxtNumero.Text = sRif(1)
                    Log.Debug("Richiamato ricerca da GIS per rif::" & Request.Item("RifCat").ToString)
                    sScript += "Search();"
                    RegisterScript(sScript, Me.GetType)

                    Session("myObjectForTestataSearch") = Nothing
                Else
                    If Not Request.Item("IdEnte") Is Nothing Then
                        Dim mySearchParam As New ObjSearchTestata
                        mySearchParam.IdEnte = Request.Item("IdEnte")
                        mySearchParam.rbSoggetto = True
                        mySearchParam.sCognome = Request.Item("Cognome")
                        mySearchParam.sNome = Request.Item("Nome")
                        mySearchParam.sCF = Request.Item("CodiceFiscale")
                        mySearchParam.sPIVA = Request.Item("PartitaIVA")
                        Session("myObjectForTestataSearch") = mySearchParam
                    End If

                    If Not Session("myObjectForTestataSearch") Is Nothing Then
                        myObjSearch = CType(Session("myObjectForTestataSearch"), ObjSearchTestata)
                        LoadParamSearch()
                        sScript += "Search();"
                        RegisterScript(sScript, Me.GetType)

                        Session("myObjectForTestataSearch") = Nothing
                    Else
                        If RbSoggetto.Checked = True Then
                            PanelSoggetto.Visible = True
                            PanelImmobile.Visible = False
                        ElseIf RbImmobile.Checked = True Then
                            PanelImmobile.Visible = True
                            PanelSoggetto.Visible = False
                        End If
                    End If
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Immobile, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If
            If ConstSession.IdEnte <> "" Then
                Try
                    ddlEnti.SelectedValue = ConstSession.IdEnte
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.Page_Load.reloaddllenti.errore: ", ex)
                End Try
                sScript += "document.getElementById('trEnte').style.display='none';"
                sScript += "$('span.Input_Emphasized').hide();$('#FileToDownload').hide();"
                RegisterScript(sScript, Me.GetType)
            End If
            HideParamSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim sScript As String = ""
    '    Dim sOrigineRichiamo As String = ""

    '    'Put user code to initialize the page here
    '    Try
    '        If TxtViaRibaltata.Text <> "" And TxtViaRibaltata.Text <> " " Then
    '            TxtVia.Text = TxtViaRibaltata.Text
    '        Else
    '            TxtVia.Text = ""
    '        End If
    '        ClearSession()
    '        If Not Request.Item("IsFromVariabile") Is Nothing Then
    '            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
    '            End If
    '        End If
    '        Log.Debug("RicercaDichiarazione::IsFromVariabile::" & ConstSession.IsFromVariabile)
    '        Log.Debug("RicercaDichiarazione::DB::" & ConstSession.StringConnection)
    '        AddKeyPress()
    '        If Page.IsPostBack = False Then
    '            LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
    '            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
    '            LoadPageCombos()
    '            '*** 20140923 - GIS ***
    '            If Not Request.Item("Org") Is Nothing Then
    '                sOrigineRichiamo = Request.Item("Org").ToString
    '            End If
    '            If sOrigineRichiamo = "GIS" Then
    '                RbImmobile.Checked = True
    '                RbSoggetto.Checked = False
    '                PanelImmobile.Visible = True
    '                PanelSoggetto.Visible = False
    '                Dim sRif() As String
    '                sRif = Request.Item("RifCat").ToString.Split("-")
    '                TxtFoglio.Text = sRif(0)
    '                TxtNumero.Text = sRif(1)
    '                Log.Debug("Richiamato ricerca da GIS per rif::" & Request.Item("RifCat").ToString)
    '                sScript += "Search();"
    '                RegisterScript(sScript, Me.GetType)

    '                Session("myObjectForTestataSearch") = Nothing
    '            Else
    '                If Not Request.Item("IdEnte") Is Nothing Then
    '                    Dim mySearchParam As New ObjSearchTestata
    '                    mySearchParam.IdEnte = Request.Item("IdEnte")
    '                    mySearchParam.rbSoggetto = True
    '                    mySearchParam.sCognome = Request.Item("Cognome")
    '                    mySearchParam.sNome = Request.Item("Nome")
    '                    mySearchParam.sCF = Request.Item("CodiceFiscale")
    '                    mySearchParam.sPIVA = Request.Item("PartitaIVA")
    '                    Session("myObjectForTestataSearch") = mySearchParam
    '                End If

    '                If Not Session("myObjectForTestataSearch") Is Nothing Then
    '                    myObjSearch = CType(Session("myObjectForTestataSearch"), ObjSearchTestata)
    '                    LoadParamSearch()
    '                    sScript += "Search();"
    '                    RegisterScript(sScript, Me.GetType)

    '                    Session("myObjectForTestataSearch") = Nothing
    '                Else
    '                    'Put user code to initialize the page here
    '                    If RbSoggetto.Checked = True Then
    '                        PanelSoggetto.Visible = True
    '                        PanelImmobile.Visible = False
    '                    ElseIf RbImmobile.Checked = True Then
    '                        PanelImmobile.Visible = True
    '                        PanelSoggetto.Visible = False
    '                    End If
    '                End If
    '            End If
    '            '*** ***
    '            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Immobile, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstSession.CodTributo, ConstSession.IdEnte, -1)
    '        End If
    '        '*** 201511 - Funzioni Sovracomunali ***
    '        If ConstSession.IdEnte <> "" Then
    '            ddlEnti.SelectedValue = ConstSession.IdEnte
    '            sScript = "document.getElementById ('lblEnti').style.display='none';"
    '            sScript += "document.getElementById ('ddlEnti').style.display='none';"
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '        '*** ***
    '        HideParamSearch()
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RbImmobile_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbImmobile.CheckedChanged
        Try
            If Not Page.IsPostBack = False Then
                PanelImmobile.Visible = True
                PanelSoggetto.Visible = False
                RbSoggetto.Checked = False
                RbImmobile.Checked = True
            End If
            HideParamSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.RbImmobile_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub RbSoggetto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbSoggetto.CheckedChanged
        Try
            If Not Page.IsPostBack = False Then
                PanelSoggetto.Visible = True
                PanelImmobile.Visible = False
                RbImmobile.Checked = False
                RbSoggetto.Checked = True
            End If
            HideParamSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.RbSoggetto_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSearch.Click
        Dim sScript As String = ""
        Try
            myObjSearch = New ObjSearchTestata
            '*** 201511 - Funzioni Sovracomunali ***
            If ConstSession.IdEnte <> "" Then
                myObjSearch.IdEnte = ConstSession.IdEnte
            Else
                myObjSearch.IdEnte = ddlEnti.SelectedValue
            End If
            '*** ***
            myObjSearch.Dal = TxtDal.Text
            myObjSearch.Al = TxtAl.Text
            myObjSearch.sProvenienza = DdlProv.SelectedValue
            myObjSearch.Chiusa = DdlDichiarazioni.SelectedValue
            If RbSoggetto.Checked = True Then
                myObjSearch.rbSoggetto = True
                myObjSearch.sCognome = TxtCognome.Text
                myObjSearch.sNome = TxtNome.Text
                myObjSearch.sCF = TxtCodFiscale.Text
                myObjSearch.sPIVA = TxtPIva.Text
                myObjSearch.sNTessera = TxtNTessera.Text
                If OptRes.Checked = True Then
                    myObjSearch.TypeSogRes = ObjSearchTestata.Sog_Res
                ElseIf OptNoRes.Checked = True Then
                    myObjSearch.TypeSogRes = ObjSearchTestata.Sog_NoRes
                Else
                    myObjSearch.TypeSogRes = ObjSearchTestata.Sog_ALL
                End If
            Else
                myObjSearch.rbImmobile = True
                myObjSearch.sVia = TxtVia.Text.Trim
                myObjSearch.sCivico = TxtCivico.Text
                myObjSearch.sInterno = TxtInterno.Text
                myObjSearch.sFoglio = TxtFoglio.Text
                myObjSearch.sNumero = TxtNumero.Text
                myObjSearch.sSubalterno = TxtSubalterno.Text
                myObjSearch.IdCatCatastale = DdlCatastale.SelectedValue
                If DDlCatTARES.SelectedValue <> "" Then
                    myObjSearch.IdCatTARES = DDlCatTARES.SelectedValue
                End If
                If TxtNComponenti.Text <> "" Then
                    myObjSearch.nComponenti = TxtNComponenti.Text
                End If
                myObjSearch.IsPF = ChkPF.Checked
                myObjSearch.IsPV = ChkPV.Checked
                myObjSearch.IsEsente = ChkEsente.Checked
                myObjSearch.HasMoreUI = ChkMoreUI.Checked
                myObjSearch.IdRiduzione = DdlRid.SelectedValue
                myObjSearch.IdDetassazione = DdlDet.SelectedValue
                myObjSearch.IdStatoOccupazione = DdlStatoOccupazione.SelectedValue
            End If
            Session("myObjectForTestataSearch") = myObjSearch
            'sScript = "loadGrid.location.href = 'ResultRicDichiarazione.aspx'"
            sScript = "document.getElementById('loadGrid').src = 'ResultRicDichiarazione.aspx'"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.CmdSearch_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdAggMassivo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdAggMassivo.Click
        Dim sScript As String = ""
        Dim listParam As New Generic.List(Of String)

        Try
            listParam.Add(TxtVia.Text)
            listParam.Add(TxtCivico.Text)
            listParam.Add(TxtFoglio.Text)
            listParam.Add(TxtNumero.Text)
            listParam.Add(TxtSubalterno.Text)
            listParam.Add(DdlCatastale.SelectedValue)
            listParam.Add(TxtDal.Text)
            listParam.Add(TxtAl.Text)
            listParam.Add(DDlCatTARES.SelectedValue)
            listParam.Add(TxtNComponenti.Text)
            listParam.Add(ChkPF.Checked.ToString())
            listParam.Add(ChkPV.Checked.ToString())
            listParam.Add(DdlRid.SelectedValue)
            listParam.Add(DdlDet.SelectedValue)
            listParam.Add(DdlStatoOccupazione.SelectedValue)
            If OptRes.Checked = True Then
                listParam.Add(ObjSearchTestata.Sog_Res)
            ElseIf OptNoRes.Checked = True Then
                listParam.Add(ObjSearchTestata.Sog_NoRes)
            Else
                listParam.Add(ObjSearchTestata.Sog_ALL)
            End If
            listParam.Add(ChkEsente.Checked.ToString())
            listParam.Add(ChkMoreUI.Checked.ToString())
            Session("ParamSearchAggMassivo") = listParam

            sScript = "location.href = '../../20/TARES/AggMassivo/AggMassivo.aspx?ente=" & ConstSession.IdEnte & "&IsFromVariabile=" & ConstSession.IsFromVariabile & "';"
            sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Bssso.location.href='../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.CmdAggMassivo_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 20140923 - GIS ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdGIS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdGIS.Click
        Dim CodeGIS, sScript, sRifPrec, sListContrib As String
        Dim fncGIS As New RemotingInterfaceAnater.GIS
        Dim listRifCat As New Generic.List(Of Anater.Oggetti.RicercaUnitaImmobiliareAnater)
        Dim myRifCat As New Anater.Oggetti.RicercaUnitaImmobiliareAnater
        Dim listUI() As ObjTestataSearch = Nothing
        Dim FncRic As New ClsDichiarazione

        Try
            sRifPrec = "" : sListContrib = ""
            If RbSoggetto.Checked Then
                If Not Session("myDvResult") Is Nothing Then
                    listUI = CType(Session("myDvResult"), ObjTestataSearch())
                    For Each myUI As ObjTestataSearch In listUI
                        If myUI.bSel Then
                            sListContrib += myUI.IdContribuente.ToString + ","
                        End If
                    Next
                    If sListContrib.Length > 1 Then
                        sListContrib = sListContrib.Substring(0, sListContrib.Length - 1)
                    End If
                    myObjSearch.IdEnte = ConstSession.IdEnte
                    myObjSearch.sListContrib = sListContrib
                    '*** 201511 - Funzioni Sovracomunali ***
                    'listUI = FncRic.GetSoggettiFromImmobili(ConstSession.StringConnection, ConstSession.IdEnte, myObjSearch, "")
                    listUI = FncRic.GetSoggettiFromImmobili(ConstSession.StringConnection, myObjSearch, "")
                End If
            Else
                listUI = CType(Session("myDvResult"), ObjTestataSearch())
            End If
            If Not listUI Is Nothing Then
                For Each myUI As ObjTestataSearch In listUI
                    If myUI.bSel And myUI.sFoglio <> "" Then
                        If sRifPrec <> myUI.sFoglio + "|" + myUI.sNumero + "|" + myUI.sSubalterno Then
                            myRifCat = New Anater.Oggetti.RicercaUnitaImmobiliareAnater
                            myRifCat.Foglio = myUI.sFoglio
                            myRifCat.Mappale = myUI.sNumero
                            myRifCat.Subalterno = myUI.sSubalterno
                            myRifCat.CodiceRicerca = ConstSession.Belfiore
                            listRifCat.Add(myRifCat)
                        End If
                    End If
                    sRifPrec = myUI.sFoglio + "|" + myUI.sNumero + "|" + myUI.sSubalterno
                Next
                If listRifCat.ToArray.Length > 0 Then
                    CodeGIS = fncGIS.getGIS(ConstSession.UrlWSGIS, listRifCat.ToArray())
                    If Not CodeGIS Is Nothing Then
                        sScript = "window.open('" & ConstSession.UrlWebGIS & CodeGIS & "','wdwGIS')"
                        RegisterScript(sScript, Me.GetType)
                    Else
                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in interrogazione Cartografia!');"
                        RegisterScript(sScript, Me.GetType)
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', ('Per accedere alla Cartografia avere almeno un foglio!');"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.CmdGIS_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdIsolaEco_Click(sender As Object, e As EventArgs) Handles CmdIsolaEco.Click
        'Stampa Dichiarazioni TARSU Analitico
        Dim sNameXLS, sNameZIP, sScript As String
        Dim DtDatiAnagrafe As New DataTable
        Dim DtDatiUtenze As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()
        Dim MyCol() As Integer
        Dim MyStampa As New RKLib.ExportData.Export("Win")
        Dim ListFile As New ArrayList

        Try
            nCol = 8
            DtDatiAnagrafe = New ClsDichiarazione().GetIsolaEcologicaAnagrafe(ConstSession.StringConnection, ConstSession.IdEnte)
            DtDatiUtenze = New ClsDichiarazione().GetIsolaEcologicaUtenze(ConstSession.StringConnection, ConstSession.IdEnte)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.CmdIsolaEco_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Not DtDatiAnagrafe Is Nothing Then
            sNameXLS = ConstSession.PathFileIsolaEcologica & "ANAGRAFE_" & Format(Now, "yyyyMMdd_hhmmss") & ".csv"

            'definisco le colonne
            aListColonne = New ArrayList
            aListColonne.Add("NUMEROFAMIGLIA")
            aListColonne.Add("CODICEFISCALE")
            aListColonne.Add("COGNOME")
            aListColonne.Add("NOME")
            aListColonne.Add("DATANASCITA")
            aListColonne.Add("COMUNENASCITA")
            aListColonne.Add("INDIRIZZO")
            aListColonne.Add("COMUNERESIDENZA")
            aListColonne.Add("NUMEROPERSONA")
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            MyCol = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            MyStampa.ExportDetails(DtDatiAnagrafe, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.CSV, sNameXLS)
            ListFile.Add(sNameXLS)

            If Not DtDatiUtenze Is Nothing Then
                sNameXLS = ConstSession.PathFileIsolaEcologica & "UTENZE_" & Format(Now, "yyyyMMdd_hhmmss") & ".csv"
                nCol = 14

                'definisco le colonne
                aListColonne = New ArrayList
                aListColonne.Add("CODCONTR")
                aListColonne.Add("UTENZA")
                aListColonne.Add("COGNOME_DENOM")
                aListColonne.Add("NOME")
                aListColonne.Add("COD_FISCALE")
                aListColonne.Add("PARTITA_IVA")
                aListColonne.Add("CODICE_VIA_UTE")
                aListColonne.Add("INDIRIZZO_UTE")
                aListColonne.Add("NUMERO_CIVICO_UTE")
                aListColonne.Add("ESPONENTE_CIVICO_UTE")
                aListColonne.Add("DATA_DECO_TASSAZIONE")
                aListColonne.Add("CATEGORIA")
                aListColonne.Add("DESC_CATEGORIA")
                aListColonne.Add("TIPO_CATEGORIA")
                aListColonne.Add("ID_UTERIGHE")
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                'definisco l'insieme delle colonne da esportare
                MyCol = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                MyStampa.ExportDetails(DtDatiUtenze, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.CSV, sNameXLS)
                ListFile.Add(sNameXLS)
                Try
                    sNameZIP = "ISOLAECOLOGICA_" & Format(Now, "yyyyMMdd_hhmmss") & ".zip"
                    Dim astrFileNames() As String = Directory.GetFiles(ConstSession.PathFileIsolaEcologica)
                    Dim strmZipOutputStream As New ZipOutputStream(File.Create(ConstSession.PathFileIsolaEcologica + sNameZIP))
                    For Each myfile As String In astrFileNames
                        For Each NameFile As String In ListFile
                            If myfile.ToLower = NameFile.ToLower Then
                                Dim entry As New ZipEntry(Path.GetFileName(myfile))
                                entry.DateTime = DateTime.Now
                                strmZipOutputStream.PutNextEntry(entry)
                                strmZipOutputStream.Write(File.ReadAllBytes(myfile), 0, File.ReadAllBytes(myfile).Length)
                                Exit For
                            End If
                        Next
                    Next
                    strmZipOutputStream.Finish()
                    strmZipOutputStream.Close()
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.CmdIsolaEco_Click.ZipFile.errore: ", ex)
                End Try
                For Each myFile As String In ListFile
                    File.Delete(myFile)
                Next
                sScript = "GestAlert('a', 'success', '', '', 'Estrazione terminata con successo!');"
                sScript += "$('LblFileToDownload').show();$('#FileToDownload').show();"
                LblFileToDownload.Text = sNameZIP
                RegisterScript(sScript, Me.GetType)
                ''1 - The most trivial way to upload a binary file to an FTP server using .NET framework Is using WebClient.UploadFile
                'Dim client As WebClient = New WebClient
                'client.Credentials = New NetworkCredential("username", "password")
                'client.UploadFile("ftp://ftp.example.com/remote/path/file.zip", "C:\local\path\file.zip")
                ''2 - If you Then need a greater control, that WebClient does Not offer (Like TLS/SSL encryption, ascii/text transfer mode, active mode, transfer resuming, etc), use FtpWebRequest. Easy way Is To just copy a FileStream To FTP stream Using Stream.CopyTo
                'Dim request As FtpWebRequest = WebRequest.Create("ftp://ftp.example.com/remote/path/file.zip")
                'request.Credentials = New NetworkCredential("username", "password")
                'request.Method = WebRequestMethods.Ftp.UploadFile
                'Using fileStream As Stream = File.OpenRead("C:\local\path\file.zip"), ftpStream As Stream = request.GetRequestStream()
                '    fileStream.CopyTo(ftpStream)
                'End Using
                '3 - If you Then need To monitor an upload progress, you have To copy the contents by chunks yourself
                'Dim request As FtpWebRequest = WebRequest.Create("ftp://ftp.example.com/remote/path/file.zip")
                'request.Credentials = New NetworkCredential("username", "password")
                'request.Method = WebRequestMethods.Ftp.UploadFile
                'Using fileStream As Stream = File.OpenRead("C:\local\path\file.zip"), ftpStream As Stream = request.GetRequestStream()
                '    Dim read As Integer
                '    Do
                '        Dim buffer() As Byte = New Byte(10240) {}
                '        read = fileStream.Read(buffer, 0, buffer.Length)
                '        If read > 0 Then
                '            ftpStream.Write(buffer, 0, read)
                '            Console.WriteLine("Uploaded {0} bytes", fileStream.Position)
                '        End If
                '    Loop While read > 0
                'End Using
            Else
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
                sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
                RegisterScript(sScript, Me.GetType)
            End If
        Else
            sScript = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
            sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", "attachment; filename=" + LblFileToDownload.Text)
        Response.WriteFile(ConstSession.PathFileIsolaEcologica + LblFileToDownload.Text)
        Response.End()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsPostBack"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>  
    Private Sub LoadPageCombos(IsPostBack As Boolean)
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Dim myEnte As String = ""
        Dim myDataReader As SqlClient.SqlDataReader = Nothing

        Try
            If ConstSession.IdEnte <> "" Then
                myEnte = ConstSession.IdEnte
            Else
                myEnte = ddlEnti.SelectedValue
            End If
            If IsPostBack = False Or ConstSession.IdEnte = "" Then
                If DdlProv.SelectedValue = "" Then
                    sSQL = "SELECT DISTINCT DESCRIZIONE, IDPROVENIENZA"
                    sSQL += " FROM TBLPROVENIENZADICHIARAZIONE"
                    sSQL += " ORDER BY DESCRIZIONE"
                    oLoadCombo.LoadComboGenerale(DdlProv, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                End If
                If DdlCatastale.SelectedValue = "" Then
                    sSQL = "SELECT DISTINCT CODCATEGORIA AS DESCRIZIONE, CODCATEGORIA AS CODICE"
                    sSQL += " FROM V_GETCATEGORIACATASTALEENTE"
                    sSQL += " ORDER BY DESCRIZIONE"
                    oLoadCombo.LoadComboGenerale(DdlCatastale, sSQL, ConstSession.StringConnectionAnagrafica, True, Costanti.TipoDefaultCmb.STRINGA)
                End If
                '*** 20130228 - gestione categoria Ateco per TARES ***
                If DDlCatTARES.SelectedValue = "" Then
                    sSQL = "SELECT DISTINCT CODICECATEGORIA+' '+DEFINIZIONE, CODICECATEGORIA,CASE WHEN ISNUMERIC(CODICECATEGORIA)=1 THEN CODICECATEGORIA ELSE 0 END, DEFINIZIONE"
                    sSQL += " FROM V_CATEGORIE_ATECO"
                    sSQL += " WHERE ((fk_IdTypeAteco=0) OR (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
                    sSQL += " AND (ENTE='" & myEnte & "')"
                    sSQL += " ORDER BY CASE WHEN ISNUMERIC(CODICECATEGORIA)=1 THEN CODICECATEGORIA ELSE 0 END, DEFINIZIONE"
                    Log.Debug(sSQL)
                    oLoadCombo.LoadComboGenerale(DDlCatTARES, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                End If
                '*** ***
                If DdlRid.SelectedValue = "" Then
                    sSQL = "SELECT DISTINCT DESCRIZIONE, CODICE"
                    sSQL += " FROM V_GETRIDESE"
                    sSQL += " WHERE MYTYPE='R'"
                    If myEnte <> "" Then
                        sSQL += " AND (IDENTE='" & myEnte & "')"
                    End If
                    sSQL += " ORDER BY DESCRIZIONE"
                    oLoadCombo.LoadComboGenerale(DdlRid, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                End If
                If DdlDet.SelectedValue = "" Then
                    sSQL = "SELECT DISTINCT DESCRIZIONE, CODICE"
                    sSQL += " FROM V_GETRIDESE"
                    sSQL += " WHERE MYTYPE='D'"
                    If myEnte <> "" Then
                        sSQL += " AND (IDENTE='" & myEnte & "')"
                    End If
                    sSQL += " ORDER BY DESCRIZIONE"
                    oLoadCombo.LoadComboGenerale(DdlDet, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                End If
                If DdlStatoOccupazione.SelectedValue = "" Then
                    sSQL = "SELECT DISTINCT DESCRIZIONE, IDSTATOOCCUPAZIONE"
                    sSQL += " FROM TBLSTATOOCCUPAZIONE"
                    sSQL += " ORDER BY DESCRIZIONE"
                    oLoadCombo.LoadComboGenerale(DdlStatoOccupazione, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                End If
            End If
            If IsPostBack = False Then
                Try
                    Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "ENTI_S", "IDENTE", "AMBIENTE", "BELFIORE")
                        myDataReader = ctx.GetDataReader(sSQL _
                                    , ctx.GetParam("IDENTE", "") _
                                    , ctx.GetParam("AMBIENTE", ConstSession.Ambiente) _
                                    , ctx.GetParam("BELFIORE", "")
                                )
                        ddlEnti.Items.Clear()
                        ddlEnti.Items.Add("...")
                        ddlEnti.Items(0).Value = ""
                        If Not myDataReader Is Nothing Then
                            Do While myDataReader.Read
                                If Not IsDBNull(myDataReader(0)) Then
                                    ddlEnti.Items.Add(myDataReader(1))
                                    ddlEnti.Items(ddlEnti.Items.Count - 1).Value = myDataReader(0)
                                End If
                            Loop
                        End If
                        ctx.Dispose()
                    End Using
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.LoadPageCombos.errore: ", ex)
                    Response.Redirect("../../PaginaErrore.aspx")
                Finally
                    myDataReader.Close()
                End Try
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.LoadPageCombos.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadParamSearch()
        Try
            '*** 201511 - Funzioni Sovracomunali ***
            ddlEnti.SelectedValue = myObjSearch.IdEnte
            '*** ***
            TxtDal.Text = myObjSearch.Dal
            TxtAl.Text = myObjSearch.Al
            DdlProv.SelectedValue = myObjSearch.sProvenienza
            DdlDichiarazioni.SelectedValue = myObjSearch.Chiusa
            If myObjSearch.rbSoggetto = True Then
                RbSoggetto.Checked = True : RbImmobile.Checked = False
                PanelSoggetto.Visible = True : PanelImmobile.Visible = False
                TxtCognome.Text = myObjSearch.sCognome
                TxtNome.Text = myObjSearch.sNome
                TxtCodFiscale.Text = myObjSearch.sCF
                TxtPIva.Text = myObjSearch.sPIVA
                TxtNTessera.Text = myObjSearch.sNTessera
                Select Case myObjSearch.TypeSogRes
                    Case CInt(ObjSearchTestata.Sog_Res)
                        OptRes.Checked = True
                    Case CInt(ObjSearchTestata.Sog_NoRes)
                        OptNoRes.Checked = True
                    Case Else
                        OptAll.Checked = True
                End Select
            ElseIf myObjSearch.rbImmobile = True Then
                RbImmobile.Checked = True : RbSoggetto.Checked = False
                PanelImmobile.Visible = True : PanelSoggetto.Visible = False
                TxtVia.Text = myObjSearch.sVia
                TxtCivico.Text = myObjSearch.sCivico
                TxtInterno.Text = myObjSearch.sInterno
                TxtFoglio.Text = myObjSearch.sFoglio
                TxtNumero.Text = myObjSearch.sNumero
                TxtSubalterno.Text = myObjSearch.sSubalterno
                DdlCatastale.SelectedValue = myObjSearch.IdCatCatastale
                DDlCatTARES.SelectedValue = myObjSearch.IdCatTARES
                If myObjSearch.nComponenti >= 0 Then
                    TxtNComponenti.Text = myObjSearch.nComponenti
                End If
                ChkPF.Checked = myObjSearch.IsPF
                ChkPV.Checked = myObjSearch.IsPV
                If myObjSearch.IsEsente Then
                    ChkEsente.Checked = myObjSearch.IsEsente
                End If
                ChkMoreUI.Checked = myObjSearch.HasMoreUI
                DdlRid.SelectedValue = myObjSearch.IdRiduzione
                DdlDet.SelectedValue = myObjSearch.IdDetassazione
                DdlStatoOccupazione.SelectedValue = myObjSearch.IdStatoOccupazione
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.LoadParamSearch.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ClearSession()
        Session.Remove("sOldMyDich")
        Session.Remove("sOldListTessera")
        Session.Remove("oTestataOrg")
        Session.Remove("oAnagrafe")
        Session.Remove("oTestata")
        Session.Remove("oListTessere")
        Session.Remove("oImmobili")
        Session.Remove("oListUITessera")
        Session.Remove("oDatiVani")
        Session.Remove("oDatiRid")
        Session.Remove("oDatiDet")
        Session.Remove("oDatiFamiglia")
        Session.Remove("oDatiFamigliaRes")
        Session("oRiepilogo") = Nothing
        Session("oArticoliSingoloContribuente") = Nothing
        Session.Remove("oTesUI")
        Session.Remove("oTessera")
        Session.Remove("oListImmobili")
        Session.Remove("oDatiRidTessera")
        Session.Remove("oDatiDetTessera")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub AddKeyPress()
        TxtCognome.Attributes.Add("onkeydown", "keyPress();")
        TxtNome.Attributes.Add("onkeydown", "keyPress();")
        TxtCodFiscale.Attributes.Add("onkeydown", "keyPress();")
        TxtPIva.Attributes.Add("onkeydown", "keyPress();")
        TxtVia.Attributes.Add("onkeydown", "keyPress();")
        TxtCivico.Attributes.Add("onkeydown", "keyPress();")
        TxtInterno.Attributes.Add("onkeydown", "keyPress();")
        TxtNTessera.Attributes.Add("onkeydown", "keyPress();")
        TxtFoglio.Attributes.Add("onkeydown", "keyPress();")
        TxtNumero.Attributes.Add("onkeydown", "keyPress();")
        TxtSubalterno.Attributes.Add("onkeydown", "keyPress();")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub HideParamSearch()
        Dim sScript As String = ""
        Try
            If ConstSession.IsFromVariabile <> "1" Then
                LblNTessera.Style.Add("display", "none")
                TxtNTessera.Style.Add("display", "none")
            Else
                LblNTessera.Style.Add("display", "")
                TxtNTessera.Style.Add("display", "")
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If Not ConstSession.HasDummyDich Then
                sScript = "document.getElementById('LblProv').style.display='none';"
                sScript += "document.getElementById('DdlProv').style.display='none';"
                If RbSoggetto.Checked Then
                    sScript += "document.getElementById('TDRes').style.display='none';"
                Else
                    sScript += "document.getElementById('TDCatCat').style.display='none';"
                    sScript += "document.getElementById('TRCat').style.display='none';"
                    sScript += "document.getElementById('TRRidDet').style.display='none';"
                End If
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDichiarazione.HideParamSearch.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
