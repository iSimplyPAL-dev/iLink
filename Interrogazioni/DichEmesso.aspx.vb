Imports log4net
''' <summary>
''' Pagina la consultazione da cruscotto delle posizioni per contribuente/immobile.
''' Le possibili opzioni sono:
''' - Stampa
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Public Class DichEmesso
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DichEmesso))
    Public UrlStradario As String = COSTANTValue.ConstSession.UrlStradario
    Protected FncGrd As New FunctionGrd
    Dim myParamSearch As New ObjInterGenSearch
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sMyEnte As String = ""
        Try
            lblTitolo.Text = COSTANTValue.ConstSession.DescrizioneEnte
            Session("FromSovracomunali") = "S"
            If Page.IsPostBack Then
                'Dim DsAnag As New DataSet
                If Not IsNothing(Session("DsDichICI")) Then
                    LoadGrd(CType(Session("DsDichICI"), DataSet), GrdDichICI)
                End If
                If Not IsNothing(Session("DsEmessoICI")) Then
                    LoadGrd(CType(Session("DsEmessoICI"), DataSet), GrdEmessoICI)
                End If

                If Not IsNothing(Session("DsDichTARSU")) Then
                    LoadGrd(CType(Session("DsDichTARSU"), DataSet), GrdDichTARSU)
                End If
                If Not IsNothing(Session("DsEmessoTARSU")) Then
                    LoadGrd(CType(Session("DsEmessoTARSU"), DataSet), GrdEmessoTARSU)
                End If

                If Not IsNothing(Session("DsDichOSAP")) Then
                    LoadGrd(CType(Session("DsDichOSAP"), DataSet), GrdDichOSAP)
                End If
                If Not IsNothing(Session("DsEmessoOSAP")) Then
                    LoadGrd(CType(Session("DsEmessoOSAP"), DataSet), GrdEmessoOSAP)
                End If

                If Not IsNothing(Session("DsDichSCUOLA")) Then
                    LoadGrd(CType(Session("DsDichSCUOLA"), DataSet), GrdDichSCUOLA)
                End If
                If Not IsNothing(Session("DsEmessoSCUOLA")) Then
                    LoadGrd(CType(Session("DsEmessoSCUOLA"), DataSet), GrdEmessoSCUOLA)
                End If

                If Not IsNothing(Session("DsEmessoPROVV")) Then
                    LoadGrd(CType(Session("DsEmessoPROVV"), DataSet), GrdEmessoPROVV)
                End If

                If Not IsNothing(Session("DsTESSERE")) Then
                    LoadGrd(CType(Session("DsTESSERE"), DataSet), GrdTessere)
                End If

                If Not IsNothing(Session("DsDichH2O")) Then
                    LoadGrd(CType(Session("DsDichH2O"), DataSet), GrdDichH2O)
                End If
                If Not IsNothing(Session("DsEmessoH2O")) Then
                    LoadGrd(CType(Session("DsEmessoH2O"), DataSet), GrdEmessoH2O)
                End If
            Else
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
                txtCognome.Attributes.Add("onkeydown", "keyPress();")
                txtNome.Attributes.Add("onkeydown", "keyPress();")
                txtCodiceFiscale.Attributes.Add("onkeydown", "keyPress();")
                txtPartIva.Attributes.Add("onkeydown", "keyPress();")
                txtVia.Attributes.Add("onkeydown", "keyPress();")
                txtNumCiv.Attributes.Add("onkeydown", "keyPress();")
                txtInterno.Attributes.Add("onkeydown", "keyPress();")
                txtFoglio.Attributes.Add("onkeydown", "keyPress();")
                txtNumero.Attributes.Add("onkeydown", "keyPress();")
                txtSubalterno.Attributes.Add("onkeydown", "keyPress();")
                txtDal.Attributes.Add("onkeydown", "keyPress();")
                txtAl.Attributes.Add("onkeydown", "keyPress();")
                TxtNTessera.Attributes.Add("onkeydown", "keyPress();")
                TxtConfDal.Attributes.Add("onkeydown", "keyPress();")
                TxtConfAl.Attributes.Add("onkeydown", "keyPress();")
                LoadCombos()
                If Not Session("mySearchInterGen") Is Nothing Then
                    myParamSearch = CType(Session("mySearchInterGen"), ObjInterGenSearch)
                    If myParamSearch.IdEnte <> String.Empty Then
                        ddlEnti.SelectedValue = myParamSearch.IdEnte
                    End If
                    If myParamSearch.Dal <> Date.MaxValue Then
                        txtDal.Text = myParamSearch.Dal.ToString
                    End If
                    If myParamSearch.Al <> Date.MaxValue Then
                        txtAl.Text = myParamSearch.Al.ToString
                    End If
                    txtCognome.Text = myParamSearch.sCognome
                    txtNome.Text = myParamSearch.sNome
                    txtCodiceFiscale.Text = myParamSearch.sCF
                    txtPartIva.Text = myParamSearch.sPIVA
                    txtVia.Text = myParamSearch.sVia
                    txtNumCiv.Text = myParamSearch.sCivico
                    txtInterno.Text = myParamSearch.sInterno
                    txtFoglio.Text = myParamSearch.sFoglio
                    txtNumero.Text = myParamSearch.sNumero
                    txtSubalterno.Text = myParamSearch.sSubalterno
                    If myParamSearch.ConfDal <> Date.MaxValue Then
                        TxtConfDal.Text = myParamSearch.ConfDal.ToString
                    End If
                    If myParamSearch.ConfAl <> Date.MaxValue Then
                        TxtConfAl.Text = myParamSearch.ConfAl.ToString
                    End If
                    RegisterScript("ControlloParametri();", Me.GetType())
                    Session("mySearchInterGen") = Nothing
                End If
            End If
            sMyEnte = COSTANTValue.ConstSession.IdEnte
            If Not Request.Item("Ente") Is Nothing Then
                If Request.Item("Ente") <> "" Then
                    sMyEnte = Request.Item("Ente")
                End If
            End If
            Dim strscript As String = ""
            If sMyEnte <> "" Then
                ddlEnti.SelectedValue = sMyEnte
                strscript += "document.getElementById('lblEnti').style.display='none';"
                strscript += "document.getElementById('ddlEnti').style.display='none';"
            End If
            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ddlEnti.SelectedValue & "')")
            If Request.Item("Provenienza") = "TESSERE" Then
                strscript += "document.getElementById('trInterGen').style.display='none';"
                strscript += "document.getElementById('trTes').style.display='';"
            Else
                strscript += "document.getElementById('trInterGen').style.display='';"
                strscript += "document.getElementById('trTes').style.display='none';"
            End If
            RegisterScript(strscript, Me.GetType())
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.Page_Load.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Dim DsAnag As New DataSet
        Dim FncRes As New clsInterrogazioni

        Try
            Try
                myParamSearch = New ObjInterGenSearch
                myParamSearch.IdEnte = ddlEnti.SelectedValue
                If txtDal.Text <> "" Then
                    myParamSearch.Dal = txtDal.Text
                End If
                If txtAl.Text <> "" Then
                    myParamSearch.Al = txtAl.Text
                End If
                'myObjSearch.sProvenienza = DdlProv.SelectedValue
                'myObjSearch.Chiusa = DdlDichiarazioni.SelectedValue
                'If RbSoggetto.Checked = True Then
                'myObjSearch.rbSoggetto = True
                myParamSearch.sCognome = txtCognome.Text
                myParamSearch.sNome = txtNome.Text
                myParamSearch.sCF = txtCodiceFiscale.Text
                myParamSearch.sPIVA = txtPartIva.Text
                'myObjSearch.sNTessera = TxtNTessera.Text
                'If OptRes.Checked = True Then
                '    myObjSearch.TypeSogRes = ObjForTestataSearch.Sog_Res
                'ElseIf OptNoRes.Checked = True Then
                '    myObjSearch.TypeSogRes = ObjForTestataSearch.Sog_NoRes
                'Else
                '    myObjSearch.TypeSogRes = ObjForTestataSearch.Sog_ALL
                'End If
                'Else
                'myObjSearch.rbImmobile = True
                myParamSearch.sVia = txtVia.Text
                myParamSearch.sCivico = txtNumCiv.Text
                myParamSearch.sInterno = txtInterno.Text
                myParamSearch.sFoglio = txtFoglio.Text
                myParamSearch.sNumero = txtNumero.Text
                myParamSearch.sSubalterno = txtSubalterno.Text
                'myObjSearch.IdCatCatastale = DdlCatastale.SelectedValue
                'If DDlCatTARES.SelectedValue <> "" Then
                '    myObjSearch.IdCatTARES = DDlCatTARES.SelectedValue
                'End If
                'If TxtNComponenti.Text <> "" Then
                '    myObjSearch.nComponenti = TxtNComponenti.Text
                'End If
                'myObjSearch.IsPF = ChkPF.Checked
                'myObjSearch.IsPV = ChkPV.Checked
                'myObjSearch.IsEsente = ChkEsente.Checked
                'myObjSearch.HasMoreUI = ChkMoreUI.Checked
                'myObjSearch.IdRiduzione = DdlRid.SelectedValue
                'myObjSearch.IdDetassazione = DdlDet.SelectedValue
                'myObjSearch.IdStatoOccupazione = DdlStatoOccupazione.SelectedValue
                'End If
                myParamSearch.sNTessera = TxtNTessera.Text
                If optTutti.Checked Then
                    myParamSearch.TipoConf = 0
                ElseIf optSi.Checked Then
                    myParamSearch.TipoConf = 1
                Else
                    myParamSearch.TipoConf = 2
                End If
                If TxtConfDal.Text <> "" Then
                    myParamSearch.ConfDal = TxtConfDal.Text
                End If
                If TxtConfAl.Text <> "" Then
                    myParamSearch.ConfAl = TxtConfAl.Text
                End If
                Session("mySearchInterGen") = myParamSearch
            Catch Err As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.btnRicerca_Click.errore: ", Err)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
            If Request.Item("Provenienza") = "TESSERE" Then
                DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionTARSU, FncRes.TipoInterrogazione_Tessere, myParamSearch, "")
                If Not IsNothing(DsAnag) Then
                    GrdTessere.DataSource = DsAnag.Tables(0)
                    GrdTessere.DataBind()
                    Session("DsTESSERE") = DsAnag
                    If Request.Item("Ente") = "" Then
                        GrdTessere.Columns(0).Visible = True
                    Else
                        GrdTessere.Columns(0).Visible = False
                    End If
                End If
                If GrdTessere.Rows.Count > 0 Then
                    GrdTessere.Visible = True
                Else
                    GrdTessere.Visible = False
                End If
            Else
                DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionICI, clsInterrogazioni.TipoInterrogazione_Dich, myParamSearch, "")
                If Not IsNothing(DsAnag) Then
                    GrdDichICI.DataSource = DsAnag.Tables(0)
                    GrdDichICI.DataBind()
                    Session("DsDichICI") = DsAnag
                    If Request.Item("Ente") = "" Then
                        GrdDichICI.Columns(0).Visible = True
                    Else
                        GrdDichICI.Columns(0).Visible = False
                    End If
                End If
                If GrdDichICI.Rows.Count > 0 Then
                    GrdDichICI.Visible = True
                Else
                    GrdDichICI.Visible = False
                End If
                DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionICI, clsInterrogazioni.TipoInterrogazione_Emesso, myParamSearch, "")
                If Not IsNothing(DsAnag) Then
                    GrdEmessoICI.DataSource = DsAnag.Tables(0)
                    GrdEmessoICI.DataBind()
                    Session("DsEmessoICI") = DsAnag
                    If Request.Item("Ente") = "" Then
                        GrdEmessoICI.Columns(0).Visible = True
                    Else
                        GrdEmessoICI.Columns(0).Visible = False
                    End If
                End If
                If GrdEmessoICI.Rows.Count > 0 Then
                    GrdEmessoICI.Visible = True
                Else
                    GrdEmessoICI.Visible = False
                End If

                DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionTARSU, clsInterrogazioni.TipoInterrogazione_Dich, myParamSearch, "")
                If Not IsNothing(DsAnag) Then
                    GrdDichTARSU.DataSource = DsAnag.Tables(0)
                    GrdDichTARSU.DataBind()
                    Session("DsDichTARSU") = DsAnag
                    If Request.Item("Ente") = "" Then
                        GrdDichTARSU.Columns(0).Visible = True
                    Else
                        GrdDichTARSU.Columns(0).Visible = False
                    End If
                End If
                If GrdDichTARSU.Rows.Count > 0 Then
                    GrdDichTARSU.Visible = True
                Else
                    GrdDichTARSU.Visible = False
                End If
                DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionTARSU, clsInterrogazioni.TipoInterrogazione_Emesso, myParamSearch, "")
                If Not IsNothing(DsAnag) Then
                    GrdEmessoTARSU.DataSource = DsAnag.Tables(0)
                    GrdEmessoTARSU.DataBind()
                    Session("DsEmessoTARSU") = DsAnag
                    If Request.Item("Ente") = "" Then
                        GrdEmessoTARSU.Columns(0).Visible = True
                    Else
                        GrdEmessoTARSU.Columns(0).Visible = False
                    End If
                    If COSTANTValue.ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                        GrdEmessoTARSU.Columns(6).Visible = True
                    Else
                        GrdEmessoTARSU.Columns(6).Visible = False
                    End If
                End If
                If GrdEmessoTARSU.Rows.Count > 0 Then
                    GrdEmessoTARSU.Visible = True
                Else
                    GrdEmessoTARSU.Visible = False
                End If

                If InStr(COSTANTValue.ConstSession.ApplicationsEnabled, Utility.Costanti.TRIBUTO_H2O) Then
                    DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionH2O, clsInterrogazioni.TipoInterrogazione_Dich, myParamSearch, Utility.Costanti.TRIBUTO_H2O)
                    If Not IsNothing(DsAnag) Then
                        GrdDichH2O.DataSource = DsAnag.Tables(0)
                        GrdDichH2O.DataBind()
                        Session("DsDichH2O") = DsAnag
                        If Request.Item("Ente") = "" Then
                            GrdDichH2O.Columns(0).Visible = True
                        Else
                            GrdDichH2O.Columns(0).Visible = False
                        End If
                    End If
                    If GrdDichH2O.Rows.Count > 0 Then
                        GrdDichH2O.Visible = True
                    Else
                        GrdDichH2O.Visible = False
                    End If

                    DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionH2O, clsInterrogazioni.TipoInterrogazione_Emesso, myParamSearch, Utility.Costanti.TRIBUTO_H2O)
                    If Not IsNothing(DsAnag) Then
                        GrdEmessoH2O.DataSource = DsAnag.Tables(0)
                        GrdEmessoH2O.DataBind()
                        Session("DsEmessoH2O") = DsAnag
                        If Request.Item("Ente") = "" Then
                            GrdEmessoH2O.Columns(0).Visible = True
                        Else
                            GrdEmessoH2O.Columns(0).Visible = False
                        End If
                    End If
                    If GrdEmessoH2O.Rows.Count > 0 Then
                        GrdEmessoH2O.Visible = True
                    Else
                        GrdEmessoH2O.Visible = False
                    End If
                End If

                If InStr(COSTANTValue.ConstSession.ApplicationsEnabled, Utility.Costanti.TRIBUTO_OSAP) Then
                    DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionOSAP, clsInterrogazioni.TipoInterrogazione_Dich, myParamSearch, Utility.Costanti.TRIBUTO_OSAP)
                    If Not IsNothing(DsAnag) Then
                        GrdDichOSAP.DataSource = DsAnag.Tables(0)
                        GrdDichOSAP.DataBind()
                        Session("DsDichOSAP") = DsAnag
                        If Request.Item("Ente") = "" Then
                            GrdDichOSAP.Columns(0).Visible = True
                        Else
                            GrdDichOSAP.Columns(0).Visible = False
                        End If
                    End If
                    If GrdDichOSAP.Rows.Count > 0 Then
                        GrdDichOSAP.Visible = True
                    Else
                        GrdDichOSAP.Visible = False
                    End If
                    DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionOSAP, clsInterrogazioni.TipoInterrogazione_Emesso, myParamSearch, Utility.Costanti.TRIBUTO_OSAP)
                    If Not IsNothing(DsAnag) Then
                        GrdEmessoOSAP.DataSource = DsAnag.Tables(0)
                        GrdEmessoOSAP.DataBind()
                        Session("DsEmessoOSAP") = DsAnag
                        If Request.Item("Ente") = "" Then
                            GrdEmessoOSAP.Columns(0).Visible = True
                        Else
                            GrdEmessoOSAP.Columns(0).Visible = False
                        End If
                    End If
                    If GrdEmessoOSAP.Rows.Count > 0 Then
                        GrdEmessoOSAP.Visible = True
                    Else
                        GrdEmessoOSAP.Visible = False
                    End If
                End If
                If InStr(COSTANTValue.ConstSession.ApplicationsEnabled, Utility.Costanti.TRIBUTO_SCUOLE) Then
                    DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionOSAP, clsInterrogazioni.TipoInterrogazione_Dich, myParamSearch, Utility.Costanti.TRIBUTO_SCUOLE)
                    If Not IsNothing(DsAnag) Then
                        GrdDichSCUOLA.DataSource = DsAnag.Tables(0)
                        GrdDichSCUOLA.DataBind()
                        Session("DsDichSCUOLA") = DsAnag
                        If Request.Item("Ente") = "" Then
                            GrdDichSCUOLA.Columns(0).Visible = True
                        Else
                            GrdDichSCUOLA.Columns(0).Visible = False
                        End If
                    End If
                    If GrdDichSCUOLA.Rows.Count > 0 Then
                        GrdDichSCUOLA.Visible = True
                    Else
                        GrdDichSCUOLA.Visible = False
                    End If
                    DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionOSAP, clsInterrogazioni.TipoInterrogazione_Emesso, myParamSearch, Utility.Costanti.TRIBUTO_SCUOLE)
                    If Not IsNothing(DsAnag) Then
                        GrdEmessoSCUOLA.DataSource = DsAnag.Tables(0)
                        GrdEmessoSCUOLA.DataBind()
                        Session("DsEmessoSCUOLA") = DsAnag
                        If Request.Item("Ente") = "" Then
                            GrdEmessoSCUOLA.Columns(0).Visible = True
                        Else
                            GrdEmessoSCUOLA.Columns(0).Visible = False
                        End If
                    End If
                    If GrdEmessoSCUOLA.Rows.Count > 0 Then
                        GrdEmessoSCUOLA.Visible = True
                    Else
                        GrdEmessoSCUOLA.Visible = False
                    End If
                End If

                DsAnag = FncRes.GetInterrogazioneGenerale(COSTANTValue.ConstSession.StringConnectionPROVVEDIMENTI, clsInterrogazioni.TipoInterrogazione_Emesso, myParamSearch, "")
                If Not IsNothing(DsAnag) Then
                    GrdEmessoPROVV.DataSource = DsAnag.Tables(0)
                    GrdEmessoPROVV.DataBind()
                    Session("DsEmessoPROVV") = DsAnag
                    If Request.Item("Ente") = "" Then
                        GrdEmessoPROVV.Columns(0).Visible = True
                    Else
                        GrdEmessoPROVV.Columns(0).Visible = False
                    End If
                End If
                If GrdEmessoPROVV.Rows.Count > 0 Then
                    GrdEmessoPROVV.Visible = True
                Else
                    GrdEmessoPROVV.Visible = False
                End If
            End If

            Dim sScript As String = ""
            Dim hasRes As Boolean = False
            If GrdDichICI.Rows.Count > 0 Or GrdEmessoICI.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetICI').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetICI').style.display='none';"
            End If
            If GrdDichTARSU.Rows.Count > 0 Or GrdEmessoTARSU.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetTARSU').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetTARSU').style.display='none';"
            End If
            If GrdDichOSAP.Rows.Count > 0 Or GrdEmessoOSAP.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetOSAP').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetOSAP').style.display='none';"
            End If
            If GrdDichSCUOLA.Rows.Count > 0 Or GrdEmessoSCUOLA.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetSCUOLA').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetSCUOLA').style.display='none';"
            End If
            If GrdDichH2O.Rows.Count > 0 Or GrdEmessoH2O.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetH2O').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetH2O').style.display='none';"
            End If
            If GrdEmessoPROVV.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetPROVV').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetPROVV').style.display='none';"
            End If
            If GrdTessere.Rows.Count > 0 Then
                sScript += "document.getElementById ('fieldsetTESSERE').style.display='';"
                hasRes = True
            Else
                sScript += "document.getElementById ('fieldsetTESSERE').style.display='none';"
            End If
            If hasRes = False Then
                sScript += "GestAlert('a', 'warning', '', '', 'Nessun risultato trovato');"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.btnRicerca_Click.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnStampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaExcel.Click
        Dim sPathProspetti As String
        Dim sNameXLS As String
        Dim ds As New DataSet
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim nCol As Integer = 20
        Dim x As Integer
        Dim dvDati As DataView

        Try
            ds = New DataSet
            ds.Tables.Add("STAMPA_ANAGRAFICHE")
            For x = 1 To nCol + 1
                ds.Tables("STAMPA_ANAGRAFICHE").Columns.Add("Col" & x.ToString.PadLeft(3, CChar("0")))
            Next
            DtDatiStampa = ds.Tables("STAMPA_ANAGRAFICHE")

            If Not IsNothing(Session("DsDichICI")) Then
                ds = CType(Session("DsDichICI"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaDichICI(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If

            '***  griglia emessoICI (query su connessione ICI) ***
            If Not IsNothing(Session("DsEmessoICI")) Then
                ds = CType(Session("DsEmessoICI"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaEmessoICI(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia DichTARSU (query su connessione TARSU)***
            If Not IsNothing(Session("DsDichTARSU")) Then
                ds = CType(Session("DsDichTARSU"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaDichTARSU(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia EmessoTARSU  (query su connessione TARSU) ***
            If Not IsNothing(Session("DsEmessoTARSU")) Then
                ds = CType(Session("DsEmessoTARSU"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaEmessoTARSU(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia DichOSAP  (query su connessione OSAP)***
            If Not IsNothing(Session("DsDichOSAP")) Then
                ds = CType(Session("DsDichOSAP"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaDichOSAP(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia EmessoOSAP (query su connessione OSAP)***
            If Not IsNothing(Session("DsEmessoOSAP")) Then
                ds = CType(Session("DsEmessoOSAP"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaEmessoOSAP(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia DichSCUOLA  (query su connessione SCUOLA)***
            If Not IsNothing(Session("DsDichSCUOLA")) Then
                ds = CType(Session("DsDichSCUOLA"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaDichOSAP(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia EmessoSCUOLA (query su connessione SCUOLA)***
            If Not IsNothing(Session("DsEmessoSCUOLA")) Then
                ds = CType(Session("DsEmessoSCUOLA"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaEmessoOSAP(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia EmessoPROVV  (query su connessione PROVVEDIMENTI)***
            If Not IsNothing(Session("DsEmessoPROVV")) Then
                ds = CType(Session("DsEmessoPROVV"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaEmessoPROVV(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia Tessre (query su connessione TARSU)***
            If Not IsNothing(Session("DsTessere")) Then
                ds = CType(Session("DsTessere"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaTessere(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia DichH2O (query su connessione H2O)***
            If Not IsNothing(Session("DsDichH2O")) Then
                ds = CType(Session("DsDichH2O"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaDichH2O(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
            '***  griglia EmessoH2O  (query su connessione H2O) ***
            If Not IsNothing(Session("DsEmessoH2O")) Then
                ds = CType(Session("DsEmessoH2O"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = StampaEmessoH2O(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.btnStampaExcel_Click.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(DtDatiStampa) Then
            'valorizzo il nome del file
            sPathProspetti = System.Configuration.ConfigurationManager.AppSettings("PATH_PROSPETTI_EXCEL")
            sNameXLS = COSTANTValue.ConstSession.IdEnte & "_INTERROGAZIONEGENERALE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'Dim MyCol As Integer() = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20}
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript, sParam As String
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Select Case CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID
                    Case "GrdDichICI"
                        For Each myRow As GridViewRow In GrdDichICI.Rows
                            If IDRow = CType(myRow.FindControl("hfIdOggetto"), HiddenField).Value Then
                                sParam = "IDTestata=" + CType(myRow.FindControl("hfIdTestata"), HiddenField).Value + "&IDImmobile=" + IDRow + "&IdAttoCompraVendita=0&IdDOCFA=0&TYPEOPERATION=DETTAGLIO&Provenienza=INTERGEN&ParamRitorno="
                                'sScript = "parent.Comandi.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/CImmobileDettaglioMod.aspx?Operation=DETTAGLIO';"
                                sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Visualizza.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/ImmobileDettaglio.aspx?" & sParam & "';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdEmessoICI"
                        For Each myRow As GridViewRow In GrdEmessoICI.Rows
                            If IDRow = CType(myRow.FindControl("hfID"), HiddenField).Value Then
                                sParam = "COD_CONTRIB=" + CType(myRow.FindControl("hfCOD_CONTRIBUENTE"), HiddenField).Value + "&anno=" + myRow.Cells(3).Text + "&Provenienza=INTERGEN"
                                sScript = "parent.Comandi.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/CalcoloICI/CCalcoloICIPuntuale.aspx';"
                                sScript += "parent.Visualizza.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/CalcoloICI/CalcoloICIPuntuale.aspx?" & sParam & "';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdDichTARSU"
                        For Each myRow As GridViewRow In GrdDichTARSU.Rows
                            If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                                sParam = "IdContribuente=" + CType(myRow.FindControl("hfIdcontribuente"), HiddenField).Value + "&IdTessera=" + CType(myRow.FindControl("hfIdTessera"), HiddenField).Value + "&IdUniqueUI=" + IDRow + "&AzioneProv=1&IdList=-1&Provenienza=6"
                                sParam += "&IsFromVariabile=" + COSTANTValue.ConstSession.IsFromVariabile
                                sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi';" 'sScript = "parent.Comandi.location.href = '.." + COSTANTValue.ConstSession.Path_TARSU + "/Dichiarazioni/ComandiGestImmobili.aspx?Provenienza=6&AzioneProv=3';"
                                sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_TARSU + "/Dichiarazioni/GestImmobili.aspx?" + sParam + "';"
                                sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdEmessoTARSU"
                        For Each myRow As GridViewRow In GrdEmessoTARSU.Rows
                            If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                                sScript = "parent.Comandi.location.href = '.." & COSTANTValue.ConstSession.Path_TARSU & "/Avvisi/Gestione/ComandiGestAvvisi.aspx';"
                                sParam = "IdUniqueAvviso=" + IDRow + "&IdRuolo=" + CType(myRow.FindControl("hfIdflusso"), HiddenField).Value + "&AzioneProv=1&Provenienza=6"
                                sParam += "&IsFromVariabile=" + COSTANTValue.ConstSession.IsFromVariabile
                                sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_TARSU + "/Avvisi/Gestione/GestAvvisi.aspx?" + sParam + "';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdDichOSAP"
                        For Each myRow As GridViewRow In GrdDichOSAP.Rows
                            If IDRow = CType(myRow.FindControl("hfIDDICHIARAZIONE"), HiddenField).Value Then
                                sParam = "IdDichiarazione=" + IDRow + "&Provenienza=INTERGEN"
                                sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_OSAP + "/Dichiarazioni/DichiarazioniView.aspx?" + sParam + "';"
                                sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdEmessoOSAP"
                        For Each myRow As GridViewRow In GrdEmessoOSAP.Rows
                            If IDRow = CType(myRow.FindControl("hfIDCARTELLA"), HiddenField).Value Then
                                sParam = "IdCartella=" + IDRow + "&Provenienza=INTERGEN"
                                sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_OSAP + "/SituazioneContribuente/SituazioneAvvisi.aspx?" + sParam + "';"
                                sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdEmessoSCUOLA"
                        For Each myRow As GridViewRow In GrdEmessoSCUOLA.Rows
                            If IDRow = CType(myRow.FindControl("hfIDCARTELLA"), HiddenField).Value Then
                                sParam = "IdCartella=" + IDRow + "&Provenienza=INTERGEN"
                                sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_OSAP + "/SituazioneContribuente/SituazioneAvvisi.aspx?" + sParam + "';"
                                sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                    Case "GrdEmessoPROVV"
                        For Each myRow As GridViewRow In GrdEmessoPROVV.Rows
                            If IDRow = CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value Then
                                sParam = "IDPROVVEDIMENTO=" & IDRow
                                sParam += "&TIPOTRIBUTO=" & Replace(myRow.Cells(4).Text, "'", "&quot;")
                                sParam += "&TIPOPROVVEDIMENTO="
                                sParam += "&ANNO=" & myRow.Cells(3).Text
                                sParam += "&NUMEROATTO=" & myRow.Cells(5).Text
                                sParam += "&IDTIPOPROVVEDIMENTO=" & CType(myRow.FindControl("hfcod_tipo_provvedimento"), HiddenField).Value
                                sParam += "&DESCTRIBUTO=" & Replace(myRow.Cells(4).Text, "'", "&quot;")
                                sParam += "&TIPOPROCEDIMENTO=" & CType(myRow.FindControl("hfcod_tipo_procedimento"), HiddenField).Value
                                sParam += "&PAGINAPRECEDENTE=INTERGEN"
                                Session("COD_TRIBUTO") = CType(myRow.FindControl("hfcod_tributo"), HiddenField).Value
                                Session("ParamGestioneAtti") = sParam
                                Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE") = CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value

                                sScript = "parent.Comandi.location.href = '../aspVuota.aspx';"
                                sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_PROVVEDIMENTI + "/GestioneAtti/GestioneAtti.aspx?" + sParam + "';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
                                RegisterScript(sScript, Me.GetType())
                                Exit For
                            End If
                        Next
                End Select
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdRowCommand.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdDichICI_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdDichICI.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(16).Text) > 0 Then
    '            sParam = "IDTestata=" + e.Item.Cells(17).Text + "&IDImmobile=" + e.Item.Cells(16).Text + "&IdAttoCompraVendita=0&IdDOCFA=0&TYPEOPERATION=DETTAGLIO&Provenienza=INTERGEN&ParamRitorno="
    '            sScript = "parent.Comandi.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/CImmobileDettaglioMod.aspx?Operation=DETTAGLIO';"
    '            sScript += "parent.Visualizza.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/ImmobileDettaglio.aspx?" & sParam & "';"
    '            sScript += "parent.Basso.location.href='../aspVuota.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
    '            RegisterScript(Me.GetType(), "loadici", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdDichICI_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdEmessoICI_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdEmessoICI.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(21).Text) > 0 Then
    '            sParam = "COD_CONTRIB=" + e.Item.Cells(21).Text + "&Provenienza=INTERGEN"
    '            sScript = "parent.Comandi.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/CalcoloICI/CCalcoloICIPuntuale.aspx';"
    '            sScript += "parent.Visualizza.location.href = '.." & COSTANTValue.ConstSession.Path_ICI & "/CalcoloICI/CalcoloICIPuntuale.aspx?" & sParam & "';"
    '            sScript += "parent.Basso.location.href='../aspVuota.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
    '            RegisterScript(Me.GetType(), "loadici", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdEmessoICI_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")   
    '    End Try
    'End Sub

    'Private Sub GrdDichTARSU_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdDichTARSU.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(14).Text) > 0 Then
    '            sParam = "IdContribuente=" + e.Item.Cells(14).Text + "&IdTessera=" + e.Item.Cells(15).Text + "&IdUniqueUI=" + e.Item.Cells(16).Text + "&AzioneProv=1&IdList=-1&Provenienza=6"
    '            sParam += "&IsFromVariabile=" + COSTANTValue.ConstSession.IsFromVariabile
    '            sScript = "parent.Comandi.location.href = '.." + COSTANTValue.ConstSession.Path_TARSU + "/Dichiarazioni/ComandiGestImmobili.aspx?Provenienza=6&AzioneProv=3';"
    '            sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_TARSU + "/Dichiarazioni/GestImmobili.aspx?" + sParam + "';"
    '            sScript += "parent.Basso.location.href='../aspVuota.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
    '            RegisterScript(Me.GetType(), "loadtarsu", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdDichTARSU_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdEmessoTARSU_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdEmessoTARSU.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(12).Text) > 0 Then
    '            sScript = "parent.Comandi.location.href = '.." & COSTANTValue.ConstSession.Path_TARSU & "/Avvisi/Gestione/ComandiGestAvvisi.aspx';"
    '            sParam = "IdUniqueAvviso=" + e.Item.Cells(12).Text + "&IdRuolo=" + e.Item.Cells(13).Text + "&AzioneProv=1&Provenienza=6"
    '            sParam += "&IsFromVariabile=" + COSTANTValue.ConstSession.IsFromVariabile
    '            sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_TARSU + "/Avvisi/Gestione/GestAvvisi.aspx?" + sParam + "';"
    '            sScript += "parent.Basso.location.href='../aspVuota.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
    '            RegisterScript(Me.GetType(), "loadtarsuavv", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdEmessoTARSU_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdDichOSAP_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdDichOSAP.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(11).Text) > 0 Then
    '            sParam = "IdDichiarazione=" + e.Item.Cells(11).Text + "&Provenienza=INTERGEN"
    '            sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
    '            sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_OSAP + "/Dichiarazioni/DichiarazioniView.aspx?" + sParam + "';"
    '            sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
    '            RegisterScript(Me.GetType(), "loadosap", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdDichOSAP_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdEmessoOSAP_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdEmessoOSAP.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(8).Text) > 0 Then
    '            sParam = "IdCartella=" + e.Item.Cells(8).Text + "&Provenienza=INTERGEN"
    '            sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
    '            sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_OSAP + "/SituazioneContribuente/SituazioneAvvisi.aspx?" + sParam + "';"
    '            sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
    '            RegisterScript(Me.GetType(), "loadosapavv", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdEmessoOSAP_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdEmessoSCUOLA_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdEmessoSCUOLA.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(8).Text) > 0 Then
    '            sParam = "IdCartella=" + e.Item.Cells(8).Text + "&Provenienza=INTERGEN"
    '            sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';"
    '            sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_OSAP + "/SituazioneContribuente/SituazioneAvvisi.aspx?" + sParam + "';"
    '            sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';"
    '            RegisterScript(Me.GetType(), "loadSCUOLAavv", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '      Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdEmessoSCUOLA_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdEmessoPROVV_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdEmessoPROVV.UpdateCommand
    '    Dim sScript, sParam As String
    '    Try
    '        'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '        If CInt(e.Item.Cells(11).Text) > 0 Then
    '            sParam = "IDPROVVEDIMENTO=" & e.Item.Cells(11).Text
    '            sParam += "&TIPOTRIBUTO=" & Replace(e.Item.Cells(4).Text, "'", "&quot;")
    '            sParam += "&TIPOPROVVEDIMENTO="
    '            sParam += "&ANNO=" & e.Item.Cells(3).Text
    '            sParam += "&NUMEROATTO=" & e.Item.Cells(5).Text
    '            sParam += "&IDTIPOPROVVEDIMENTO=" & e.Item.Cells(12).Text
    '            sParam += "&DESCTRIBUTO=" & Replace(e.Item.Cells(4).Text, "'", "&quot;")
    '            sParam += "&TIPOPROCEDIMENTO=" & e.Item.Cells(13).Text
    '            sParam += "&PAGINAPRECEDENTE=INTERGEN"
    '            Session("COD_TRIBUTO") = e.Item.Cells(15).Text
    '            Session("ParamGestioneAtti") = sParam
    '            Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE") = e.Item.Cells(14).Text

    '            sScript = "parent.Comandi.location.href = '../aspVuota.aspx';"
    '            sScript += "parent.Visualizza.location.href = '.." + COSTANTValue.ConstSession.Path_PROVVEDIMENTI + "/GestioneAtti/GestioneAtti.aspx?" + sParam + "';"
    '            sScript += "parent.Basso.location.href='../aspVuota.aspx';"
    '            sScript += "parent.Nascosto.location.href='../aspVuota.aspx';"
    '            RegisterScript(Me.GetType(), "loadprovv", "<script language='javascript'>" & sScript & "</script>")
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.GrdEmessoPROVV_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dsTemp"></param>
    ''' <param name="DataValueField"></param>
    ''' <param name="DataTextField"></param>
    ''' <param name="lngSelectedID"></param>
    Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
        cboTemp.DataSource = dsTemp
        cboTemp.DataValueField = DataValueField
        cboTemp.DataTextField = DataTextField
        cboTemp.DataBind()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadCombos()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try

            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "ENTI_S"
            cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.NVarChar).Value = COSTANTValue.ConstSession.Ambiente
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlEnti.Items.Clear()
                ddlEnti.Items.Add("...")
                ddlEnti.Items(0).Value = "-1"
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlEnti.Items.Add(myDataReader(1).ToString)
                            ddlEnti.Items(ddlEnti.Items.Count - 1).Value = myDataReader(0).ToString
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.LoadCombos.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.LoadCombos.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di LoadComboGenerale " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DsAnag"></param>
    ''' <param name="Griglia"></param>
    Private Sub LoadGrd(ByVal DsAnag As DataSet, ByVal Griglia As Ribes.OPENgov.WebControls.RibesGridView)
        Try
            If Not IsNothing(DsAnag) Then
                Griglia.DataSource = DsAnag.Tables(0)
                If Request.Item("Ente") = "" Then
                    Griglia.Columns(0).Visible = True
                Else
                    Griglia.Columns(0).Visible = False
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.LoadGrd.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di Denominazioni " + ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaDichICI(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "Dichiarazione ICI"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Via"
            x += 1
            dr(x) = "Foglio"
            x += 1
            dr(x) = "Numero"
            x += 1
            dr(x) = "Subalterno"
            x += 1
            dr(x) = "datainizio"
            x += 1
            dr(x) = "datafine"
            x += 1
            dr(x) = "categoria"
            x += 1
            dr(x) = "classe"
            x += 1
            dr(x) = "Valoreimmobile"
            x += 1
            dr(x) = "Consistenza"
            x += 1
            dr(x) = "PercPossesso"
            x += 1
            dr(x) = "carat"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1

                End If
                If Not IsDBNull(dvDati.Item(i)("Nominativo")) Then
                    dr(x) = CStr(dvDati.Item(i)("Nominativo"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Via")) Then
                    dr(x) = CStr(dvDati.Item(i)("Via"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Foglio")) Then
                    dr(x) = CStr(dvDati.Item(i)("Foglio"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Numero")) Then
                    dr(x) = CStr(dvDati.Item(i)("Numero"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Subalterno")) Then
                    dr(x) = CStr(dvDati.Item(i)("Subalterno"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("datainizio")) Then
                    dr(x) = CStr(dvDati.Item(i)("datainizio"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("datafine")) Then
                    dr(x) = CStr(dvDati.Item(i)("datafine"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("categoria")) Then
                    dr(x) = CStr(dvDati.Item(i)("categoria"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("classe")) Then
                    dr(x) = CStr(dvDati.Item(i)("classe"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Valoreimmobile")) Then
                    dr(x) = CStr(dvDati.Item(i)("Valoreimmobile"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Consistenza")) Then
                    dr(x) = CStr(dvDati.Item(i)("Consistenza"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("PercPossesso")) Then
                    dr(x) = CStr(dvDati.Item(i)("PercPossesso"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("carat")) Then
                    dr(x) = CStr(dvDati.Item(i)("carat"))
                Else
                    dr(x) = ""
                End If
                x += 1

                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            'dr(0) = "Totale Contribuenti: " & (dsAnagrafica.Tables(0).Rows.Count)
            'dr(0) = "Emesso Provvedimenti: " & (dvDati.Count)
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaDichICI.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaEmessoICI(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "Emesso ICI"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Anno"
            x += 1
            dr(x) = "Tributo"
            x += 1
            dr(x) = "Imp. Abi. Princ. (3912-3958) €"
            x += 1
            dr(x) = "Imp. Altri Fab. (3918-3961) €"
            x += 1
            dr(x) = "Imp. Altri Fab. Stato (3919) €"
            x += 1
            dr(x) = "Imp. Aree Fab. (3916-3960) €"
            x += 1
            dr(x) = "Imp. Aree Fab. Stato (3917) €"
            x += 1
            dr(x) = "Imp. Terreni (3914) €"
            x += 1
            dr(x) = "Imp. Terreni Stato (3915) €"
            x += 1
            dr(x) = "Imp. Fab.Rur. (3913-3959) €"
            x += 1
            dr(x) = "Imp. Fab.Rur. Stato (3919) €"
            x += 1
            dr(x) = "Imp. Uso Prod.Cat.D (3930) €"
            x += 1
            dr(x) = "Imp. Uso Prod.Cat.D Stato (3925) €"
            x += 1
            dr(x) = "Detraz. €"
            x += 1
            dr(x) = "Totale €"
            x += 1
            dr(x) = "Num. Fab."
            x += 1
            dr(x) = "Pagato €"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If
                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ANNO")) Then
                    dr(x) = CStr(dvDati.Item(i)("ANNO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TRIBUTO")) Then
                    dr(x) = CStr(dvDati.Item(i)("TRIBUTO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ABIPRINC")) Then
                    dr(x) = CStr(dvDati.Item(i)("ABIPRINC"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ALTRIFAB")) Then
                    dr(x) = CStr(dvDati.Item(i)("ALTRIFAB"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ALTRIFABSTATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("ALTRIFABSTATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("AREEFAB")) Then
                    dr(x) = CStr(dvDati.Item(i)("AREEFAB"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("AREEFABSTATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("AREEFABSTATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TERRENI")) Then
                    dr(x) = CStr(dvDati.Item(i)("TERRENI"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TERRENISTATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("TERRENISTATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("FABRUR")) Then
                    dr(x) = CStr(dvDati.Item(i)("FABRUR"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("FABRURSTATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("FABRURSTATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("USOPRODCATD")) Then
                    dr(x) = CStr(dvDati.Item(i)("USOPRODCATD"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("USOPRODCATDSTATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("USOPRODCATDSTATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DETRAZ")) Then
                    dr(x) = CStr(dvDati.Item(i)("DETRAZ"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TOTALE")) Then
                    dr(x) = CStr(dvDati.Item(i)("TOTALE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NFABB")) Then
                    dr(x) = CStr(dvDati.Item(i)("NFABB"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("PAGATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("PAGATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaEmessoICI.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaDichTARSU(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "Dichiarazione TARSU"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Via"
            x += 1
            dr(x) = "Foglio"
            x += 1
            dr(x) = "Numero"
            x += 1
            dr(x) = "Sub."
            x += 1
            dr(x) = "Dal"
            x += 1
            dr(x) = "Al"
            x += 1
            dr(x) = "Cat."
            x += 1
            dr(x) = "MQ"
            x += 1
            dr(x) = "MQ Tassabili"
            x += 1
            dr(x) = "N.Vani"
            'x += 1
            'dr(x) = "IdContribuente"
            'x += 1
            'dr(x) = "IdTessera"
            'x += 1
            'dr(x) = "Id"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If

                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("VIA")) Then
                    dr(x) = CStr(dvDati.Item(i)("VIA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("FOGLIO")) Then
                    dr(x) = CStr(dvDati.Item(i)("FOGLIO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NUMERO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NUMERO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("SUB")) Then
                    dr(x) = CStr(dvDati.Item(i)("SUB"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Dal")) Then
                    dr(x) = CStr(dvDati.Item(i)("Dal"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Al")) Then
                    dr(x) = CStr(dvDati.Item(i)("Al"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CAT")) Then
                    dr(x) = CStr(dvDati.Item(i)("CAT"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("MQ")) Then
                    dr(x) = CStr(dvDati.Item(i)("MQ"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("MQTASSABILI")) Then
                    dr(x) = CStr(dvDati.Item(i)("MQTASSABILI"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NVANI")) Then
                    dr(x) = CStr(dvDati.Item(i)("NVANI"))
                Else
                    dr(x) = ""
                End If
                x += 1
                'If Not IsDBNull(dvDati.Item(i)("IDCONTRIBUENTE")) Then
                '    dr(x) = CStr(dvDati.Item(i)("IDCONTRIBUENTE"))
                'Else
                '    dr(x) = ""
                'End If
                'x += 1
                'If Not IsDBNull(dvDati.Item(i)("IDTESSERA")) Then
                '    dr(x) = CStr(dvDati.Item(i)("IDTESSERA"))
                'Else
                '    dr(x) = ""
                'End If
                'x += 1
                'If Not IsDBNull(dvDati.Item(i)("ID")) Then
                '    dr(x) = CStr(dvDati.Item(i)("ID"))
                'Else
                '    dr(x) = ""
                'End If
                'x += 1
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaDichTARSU.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaEmessoTARSU(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer
        Try

            dr = DtDatiStampa.NewRow
            dr(0) = "Emesso TARSU"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Anno"
            x += 1
            dr(x) = "Imp.Fissa €"
            x += 1
            dr(x) = "Imp.Variabile €"
            x += 1
            dr(x) = "Imp.Conferimenti €"
            x += 1
            dr(x) = "Imp.Maggiorazione €"
            x += 1
            dr(x) = "N.Avviso"
            x += 1
            dr(x) = "Carico €"
            x += 1
            dr(x) = "Pagato €"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If

                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ANNO")) Then
                    dr(x) = CStr(dvDati.Item(i)("ANNO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("IMPFISSA")) Then
                    dr(x) = CStr(dvDati.Item(i)("IMPFISSA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("IMPVARIABILE")) Then
                    dr(x) = CStr(dvDati.Item(i)("IMPVARIABILE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("IMPCONFERIMENTI")) Then
                    dr(x) = CStr(dvDati.Item(i)("IMPCONFERIMENTI"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("IMPMAGGIORAZIONE")) Then
                    dr(x) = CStr(dvDati.Item(i)("IMPMAGGIORAZIONE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NAVVISO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NAVVISO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CARICO")) Then
                    dr(x) = CStr(dvDati.Item(i)("CARICO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("PAGATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("PAGATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaEmessoTARSU.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaDichOSAP(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer

        Try

            dr = DtDatiStampa.NewRow
            dr(0) = "Dichiarazione OSAP"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Via"
            x += 1
            dr(x) = "Dal"
            x += 1
            dr(x) = "Al"
            x += 1
            dr(x) = "Durata"
            x += 1
            dr(x) = "Occupazione"
            x += 1
            dr(x) = "Consistenza"
            x += 1
            dr(x) = "Cat."
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If

                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("VIA")) Then
                    dr(x) = CStr(dvDati.Item(i)("VIA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("datainizio")) Then
                    dr(x) = CStr(dvDati.Item(i)("datainizio"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("datafine")) Then
                    dr(x) = CStr(dvDati.Item(i)("datafine"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DURATA")) Then
                    dr(x) = CStr(dvDati.Item(i)("DURATA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("OCCUPAZIONE")) Then
                    dr(x) = CStr(dvDati.Item(i)("OCCUPAZIONE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CONSISTENZA")) Then
                    dr(x) = CStr(dvDati.Item(i)("CONSISTENZA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CAT")) Then
                    dr(x) = CStr(dvDati.Item(i)("CAT"))
                Else
                    dr(x) = ""
                End If
                x += 1
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaDichOSAP.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaEmessoOSAP(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "Emesso OSAP"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Anno"
            x += 1
            dr(x) = "N.Avviso"
            x += 1
            dr(x) = "Carico €"
            x += 1
            dr(x) = "Pagato €"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If
                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ANNO")) Then
                    dr(x) = CStr(dvDati.Item(i)("ANNO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NAVVISO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NAVVISO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CARICO")) Then
                    dr(x) = CStr(dvDati.Item(i)("CARICO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("PAGATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("PAGATO"))
                Else
                    dr(x) = ""
                End If
                x += 1

                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaEmessoOSAP.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaEmessoPROVV(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer
        Try

            dr = DtDatiStampa.NewRow
            dr(0) = "Emesso PROVVEDIMENTI"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1

            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Anno"
            x += 1
            dr(x) = "Tipo"
            x += 1
            dr(x) = "N. Atto"
            x += 1
            dr(x) = "Data Creazione"
            x += 1
            dr(x) = "Importo Totale Rid €"
            x += 1
            dr(x) = "Importo Totale €"
            x += 1
            dr(x) = "Stato Avviso"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If
                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("ANNO")) Then
                    dr(x) = CStr(dvDati.Item(i)("ANNO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TIPO")) Then
                    dr(x) = CStr(dvDati.Item(i)("TIPO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NATTO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NATTO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DATACREAZIONE")) Then
                    dr(x) = CStr(dvDati.Item(i)("DATACREAZIONE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TOTALERID")) Then
                    dr(x) = CStr(dvDati.Item(i)("TOTALERID"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TOTALE")) Then
                    dr(x) = CStr(dvDati.Item(i)("TOTALE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("STATOAVVISO")) Then
                    dr(x) = CStr(dvDati.Item(i)("STATOAVVISO"))
                Else
                    dr(x) = ""
                End If
                x += 1


                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaEmessoPROVV.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaTessere(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim nCol As Integer = 20
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "TESSERE"
            dr(2) = "Data Stampa:" & DateTime.Now.Date
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Tipo Tessera"
            x += 1
            dr(x) = "N.Tessera"
            x += 1
            dr(x) = "Cod.Interno"
            x += 1
            dr(x) = "Cod.Utente"
            x += 1
            dr(x) = "Dal"
            x += 1
            dr(x) = "Al"
            x += 1
            dr(x) = "Conferimenti"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If

                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DESCRTIPOTESSERA")) Then
                    dr(x) = CStr(dvDati.Item(i)("DESCRTIPOTESSERA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NUMERO_TESSERA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("NUMERO_TESSERA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CODICE_INTERNO")) Then
                    dr(x) = CStr(dvDati.Item(i)("CODICE_INTERNO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CODICE_UTENTE")) Then
                    dr(x) = CStr(dvDati.Item(i)("CODICE_UTENTE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DATA_RILASCIO")) Then
                    dr(x) = CStr(dvDati.Item(i)("DATA_RILASCIO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DATA_CESSAZIONE")) Then
                    dr(x) = CStr(dvDati.Item(i)("DATA_CESSAZIONE"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TOTVOLUME")) Then
                    dr(x) = CStr(dvDati.Item(i)("TOTVOLUME"))
                Else
                    dr(x) = ""
                End If
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaTessere.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaDichH2O(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim x, i As Integer

        Try
            dr = DtDatiStampa.NewRow
            dr(0) = "Contatore Acquedotto"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Intestatario"
            x += 1
            dr(x) = "Utente"
            x += 1
            dr(x) = "Contratto"
            x += 1
            dr(x) = "N.Utente"
            x += 1
            dr(x) = "Matricola"
            x += 1
            dr(x) = "Ubicazione"
            x += 1
            dr(x) = "Installazione"
            x += 1
            dr(x) = "Cessazione"
            x += 1
            dr(x) = "Ultima Lettura"
            x += 1
            dr(x) = "Sub."
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If

                If Not IsDBNull(dvDati.Item(i)("Intestatario")) Then
                    dr(x) = CStr(dvDati.Item(i)("Intestatario"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Utente")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("Utente"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Contratto")) Then
                    dr(x) = CStr(dvDati.Item(i)("Contratto"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NUtente")) Then
                    dr(x) = CStr(dvDati.Item(i)("NUtente"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Matricola")) Then
                    dr(x) = CStr(dvDati.Item(i)("Matricola"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Ubicazione")) Then
                    dr(x) = CStr(dvDati.Item(i)("Ubicazione"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Installazione")) Then
                    dr(x) = FncGrd.FormattaDataGrd(CStr(dvDati.Item(i)("Installazione")))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("Cessazione")) Then
                    dr(x) = FncGrd.FormattaDataGrd(CStr(dvDati.Item(i)("Cessazione")))
                Else
                    dr(x) = ""
                End If
                x += 1
                dr(x) = Utility.StringOperation.FormatString(dvDati.Item(i)("UltimaLettura"))
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CODCONTATORESUB")) Then
                    dr(x) = FncGrd.FormattaIsSubContatore(dvDati.Item(i)("CODCONTATORESUB")) & " " & FncGrd.FormattaToolTipSubContatore(dvDati.Item(i)("MATRICOLAPRINCIPALE"))
                Else
                    dr(x) = ""
                End If
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaDichH2O.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DtDatiStampa"></param>
    ''' <param name="dvDati"></param>
    ''' <returns></returns>
    Private Function StampaEmessoH2O(ByVal DtDatiStampa As DataTable, ByVal dvDati As DataView) As DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim x, i As Integer
        Try

            dr = DtDatiStampa.NewRow
            dr(0) = "Emesso Acquedotto"
            dr(2) = "Data Stampa:" & DateTime.Now.Date            'DateTime.Today.Day & "/" & DateTime.Today.Month & "/" & DateTime.Today.Year
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            x = 0
            If Request.Item("Ente") = "" Then
                dr(x) = "Descrizione Ente"
                x += 1
            End If
            dr(x) = "Nominativo"
            x += 1
            dr(x) = "Cod.Fiscale/P.IVA"
            x += 1
            dr(x) = "Periodo"
            x += 1
            dr(x) = "Matricola"
            x += 1
            dr(x) = "Tipo"
            x += 1
            dr(x) = "Data Documento"
            x += 1
            dr(x) = "N.Documento"
            x += 1
            dr(x) = "Emesso €"
            x += 1
            dr(x) = "Pagato €"
            DtDatiStampa.Rows.Add(dr)
            For i = 0 To dvDati.Count - 1
                dr = DtDatiStampa.NewRow
                x = 0
                If Request.Item("Ente") = "" Then
                    dr(x) = CStr(dvDati.Item(i)("Descrizione_Ente"))
                    x += 1
                End If

                If Not IsDBNull(dvDati.Item(i)("NOMINATIVO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NOMINATIVO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("CFPIVA")) Then
                    dr(x) = "'" & CStr(dvDati.Item(i)("CFPIVA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("PERIODO")) Then
                    dr(x) = CStr(dvDati.Item(i)("PERIODO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("MATRICOLA")) Then
                    dr(x) = CStr(dvDati.Item(i)("MATRICOLA"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("TIPO")) Then
                    dr(x) = CStr(dvDati.Item(i)("TIPO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("DATADOCUMENTO")) Then
                    dr(x) = CStr(dvDati.Item(i)("DATADOCUMENTO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("NDOCUMENTO")) Then
                    dr(x) = CStr(dvDati.Item(i)("NDOCUMENTO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("EMESSO")) Then
                    dr(x) = CStr(dvDati.Item(i)("EMESSO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                If Not IsDBNull(dvDati.Item(i)("PAGATO")) Then
                    dr(x) = CStr(dvDati.Item(i)("PAGATO"))
                Else
                    dr(x) = ""
                End If
                x += 1
                DtDatiStampa.Rows.Add(dr)
            Next i
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)
            dr = DtDatiStampa.NewRow
            DtDatiStampa.Rows.Add(dr)

            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.DichEmesso.StampaEmessoH2O.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function

End Class

