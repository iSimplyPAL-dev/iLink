Imports System
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports log4net
Imports AnagInterface
Imports Anagrafica.DLL
''' <summary>
''' Pagina per la consultazione/gestione dei residenti.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestioneAnagrafeResidenti
    Inherits BasePage

    Private bHasH2O As Boolean = False
    Private bHasOSAP As Boolean = False

    'Protected GrdRes As EO.Web.Grid
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestioneAnagrafeResidenti))
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
        'IsCallbackByMe condition is similar to checking 
        'Page.IsPostBack in a page without using 
        'CallbackPanel. It is not necessary to repopulate
        'the grid every time when view state is enabled
        Dim strTRIBUTI As String = ""
        Try
            If Not IsNothing(ConfigurationManager.AppSettings("APPLICATIONS_ENABLED")) Then
                strTRIBUTI = ConfigurationManager.AppSettings("APPLICATIONS_ENABLED").ToString()
            End If
            'OPENGOVA,OPENGOVG,OPENGOVI,OPENGOVTA,OPENGOVP,OPENGOVTIA
            If InStr(strTRIBUTI, "OPENGOVU") Then
                bHasH2O = True
            Else
                bHasH2O = False
            End If
            If InStr(strTRIBUTI, "OPENGOVTOCO") Then
                bHasOSAP = True
            Else
                bHasOSAP = False
            End If
            If Page.IsPostBack Then
                Dim DsAnag As New DataSet
                If Not IsNothing(Session("DsAnag")) Then
                    DsAnag = CType(Session("DsAnag"), DataSet)
                    GrdRes.DataSource = DsAnag.Tables(0)
                    If bHasH2O = False Then
                        GrdRes.Columns(6).Visible = False
                    Else
                        GrdRes.Columns(6).Visible = True
                    End If
                    If bHasOSAP = False Then
                        GrdRes.Columns(5).Visible = False
                    Else
                        GrdRes.Columns(5).Visible = True
                    End If
                End If
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "Residenti", "GestioneResidenti", Utility.Costanti.AZIONE_LETTURA.ToString, "", COSTANTValue.ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Dim FncRes As New ClsAnagrafeResidenti
        Dim DsAnag As New DataSet
        Dim nIsTrattato As Integer = 2
        Dim nVSTributo As Integer = 2

        Try
            'Log.Debug("GestioneAnagrafeResidenti::btnRicerca_Click::inizio ricerca")
            If optTrattato.Checked = True Then
                nIsTrattato = 1
            ElseIf optNonTrattato.Checked = True Then
                nIsTrattato = 0
            End If
            If optSiSuTributo.Checked = True Then
                nVSTributo = 1
            ElseIf optNoSuTributo.Checked = True Then
                nVSTributo = 0
            End If

            DsAnag = FncRes.getAnagrafeResidentiTributi(COSTANTValue.ConstSession.IdEnte, txtCognome.Text, txtNome.Text, txtCodiceFiscale.Text, txtNumFamiglia.Text, nIsTrattato, nVSTributo)
            If Not IsNothing(DsAnag) Then
                GrdRes.DataSource = DsAnag.Tables(0)
                GrdRes.DataBind()
                If bHasH2O = False Then
                    GrdRes.Columns(6).Visible = False
                Else
                    GrdRes.Columns(6).Visible = True
                End If
                If bHasOSAP = False Then
                    GrdRes.Columns(5).Visible = False
                Else
                    GrdRes.Columns(5).Visible = True
                End If
                Session("DsAnag") = DsAnag
                ViewState.Add("SortKey", "Cognome")
                ViewState.Add("OrderBy", "ASC")
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.btnRicerca_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    'Private Sub btnRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
    '    Dim FncRes As New ClsAnagrafeResidenti
    '    Dim DsAnag As New DataSet
    '    Dim nIsTrattato As Integer = 2
    '    Dim nVSTributo As Integer = 2

    '    Try
    '        'Log.Debug("GestioneAnagrafeResidenti::btnRicerca_Click::inizio ricerca")
    '        If optTrattato.Checked = True Then
    '            nIsTrattato = 1
    '        ElseIf optNonTrattato.Checked = True Then
    '            nIsTrattato = 0
    '        End If
    '        If optSiSuTributo.Checked = True Then
    '            nVSTributo = 1
    '        ElseIf optNoSuTributo.Checked = True Then
    '            nVSTributo = 0
    '        End If

    '        DsAnag = FncRes.getAnagrafeResidentiTributi(COSTANTValue.ConstSession.IdEnte, txtCognome.Text, txtNome.Text, txtCodiceFiscale.Text, nIsTrattato, nVSTributo)
    '        If Not IsNothing(DsAnag) Then
    '            GrdRes.DataSource = DsAnag.Tables(0)
    '            GrdRes.DataBind()
    '            If bHasH2O = False Then
    '                GrdRes.Columns(6).Visible = False
    '            Else
    '                GrdRes.Columns(6).Visible = True
    '            End If
    '            If bHasOSAP = False Then
    '                GrdRes.Columns(5).Visible = False
    '            Else
    '                GrdRes.Columns(5).Visible = True
    '            End If
    '            Session("DsAnag") = DsAnag
    '            ViewState.Add("SortKey", "Cognome")
    '            ViewState.Add("OrderBy", "ASC")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.btnRicerca_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        DivAttesa.Style.Add("display", "none")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim FncAnagRes As New ClsAnagrafeResidenti
        Dim nTrattato As Integer = 0
        Dim myDs As New DataSet
        Dim x As Integer

        Try
            If Not IsNothing(Session("DsAnag")) Then
                myDs = CType(Session("DsAnag"), DataSet)
                For x = 0 To myDs.Tables(0).Rows.Count - 1
                    If myDs.Tables(0).Rows(x)("trattato").ToString = "1" Then
                        nTrattato = 1
                    Else
                        nTrattato = 0
                    End If
                    'intReturn = ClsAnagRes.updateAnagrafeResidenti(Session("COD_ENTE"), WFSessione, Session("PARAMETROENV"), Session("username"), strCodIndividuale)
                    If FncAnagRes.updateAnagrafeResidenti(CInt(myDs.Tables(0).Rows(x)("MOVIMENTOID").ToString), nTrattato) <= 0 Then
                        Log.Debug("Si è verificato un errore in GestioneAnagrafeResidenti::btnSalva_Click:: IDMOVIMENTO Non Aggiornato::" & myDs.Tables(0).Rows(x)("MOVIMENTOID").ToString)
                    End If
                Next

                btnRicerca_Click(Nothing, Nothing)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.btnSalva_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' Funzione per la gestione degli eventi sulla griglia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String = ""
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdRes.Rows
                    If IDRow = CType(myRow.FindControl("hfCOD_INDIVIDUALE"), HiddenField).Value Then
                        'apro il popup passandogli l'indice dell'array da visualizzare
                        LoadDatiResidente(myRow)
                        Exit For
                    End If
                Next
            ElseIf e.CommandName = "RowEdit" Then
                Dim myContrib As Integer = -1
                Try
                    myContrib = CInt(IDRow)
                Catch ex As Exception
                    myContrib = -1
                End Try
                If myContrib > 0 Then
                    For Each myRow As GridViewRow In GrdRes.Rows
                        If IDRow = CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value Then
                            If Not ViewState("sessionName") Is Nothing Then
                                Session.Remove(ViewState("sessionName").ToString)
                            End If

                            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
                            oDettaglioAnagrafica.COD_CONTRIBUENTE = IDRow
                            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = CType(myRow.FindControl("hfiddataanagrafica"), HiddenField).Value

                            Dim myResidente As New AnagraficaResidente
                            myResidente.CodIndividuale = CInt(CType(myRow.FindControl("hfCOD_INDIVIDUALE"), HiddenField).Value)
                            myResidente.Azione = "il " & myRow.Cells(11).Text & " " & myRow.Cells(10).Text
                            myResidente.CodiceFiscale = myRow.Cells(2).Text
                            myResidente.Cognome = myRow.Cells(0).Text
                            myResidente.Nome = myRow.Cells(1).Text
                            myResidente.Sesso = CType(myRow.FindControl("hfsesso"), HiddenField).Value
                            myResidente.ComuneNascita = CType(myRow.FindControl("hfluogo_nascita"), HiddenField).Value
                            If Not CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value Is Nothing And CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value <> "" Then
                                myResidente.DataNascita = CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value 'Left(e.Item.Cells(16).Text, 2) + "/" + Mid(e.Item.Cells(16).Text, 3, 2) + "/" + Right(e.Item.Cells(16).Text, 4)
                            End If
                            If Not CType(myRow.FindControl("hfdata_morte"), HiddenField).Value Is Nothing And CType(myRow.FindControl("hfdata_morte"), HiddenField).Value <> "" And CType(myRow.FindControl("hfdata_morte"), HiddenField).Value <> "31129999" Then
                                myResidente.DataMorte = CType(myRow.FindControl("hfdata_morte"), HiddenField).Value 'Left(e.Item.Cells(17).Text, 2) + "/" + Mid(e.Item.Cells(17).Text, 3, 2) + "/" + Right(e.Item.Cells(17).Text, 4)
                            End If
                            myResidente.ViaResidenza = CType(myRow.FindControl("hfvia"), HiddenField).Value
                            myResidente.CivicoResidenza = CType(myRow.FindControl("hfnumero"), HiddenField).Value
                            myResidente.EsponenteCivicoResidenza = CType(myRow.FindControl("hflettera"), HiddenField).Value
                            myResidente.InternoCivicoResidenza = CType(myRow.FindControl("hfinterno"), HiddenField).Value
                            myResidente.CodFamiglia = myRow.Cells(8).Text
                            myResidente.DescrParentela = myRow.Cells(9).Text
                            Session("DatiResidenti") = myResidente

                            ViewState("sessionName") = "codContribuente"
                            Session(ViewState("sessionName").ToString) = oDettaglioAnagrafica
                            sScript = "ApriRicercaAnagrafe('" & ViewState("sessionName").ToString & "')"
                            RegisterScript(sScript, Me.GetType())
                            Exit For
                        End If
                    Next
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Contribuente non presente in banca dati tributaria!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        Dim IDRow As String = e.CommandArgument.ToString()
    '        If e.CommandName = "RowOpen" Then
    '            For Each myRow As GridViewRow In GrdRes.Rows
    '                If IDRow = CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value Then
    '                    'apro il popup passandogli l'indice dell'array da visualizzare
    '                    Dim codfiscale As String = myRow.Cells(2).Text
    '                    Dim nome As String = myRow.Cells(1).Text
    '                    Dim cognome As String = myRow.Cells(0).Text
    '                    Dim sesso As String = CType(myRow.FindControl("hfsesso"), HiddenField).Value
    '                    Dim datanascita As String = CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value
    '                    Dim datamorte As String = CType(myRow.FindControl("hfdata_morte"), HiddenField).Value
    '                    Dim luogonascita As String = CType(myRow.FindControl("hfluogo_nascita"), HiddenField).Value
    '                    Dim nfamiglia As String = myRow.Cells(8).Text
    '                    Dim posizione As String = myRow.Cells(9).Text
    '                    Dim codvia As String = CType(myRow.FindControl("hfCOD_VIA"), HiddenField).Value
    '                    Dim indirizzo As String = CType(myRow.FindControl("hfvia"), HiddenField).Value + " " + CType(myRow.FindControl("hfnumero"), HiddenField).Value + " " + CType(myRow.FindControl("hflettera"), HiddenField).Value + " " + CType(myRow.FindControl("hfinterno"), HiddenField).Value
    '                    Dim azione As String = myRow.Cells(10).Text

    '                    Dim sScript As String = "ApriDatiAnagrafeResidenti('" & codfiscale & "','" & nome & "','" & cognome & "','" & indirizzo & "','" & sesso & "','" & datanascita & "','" & luogonascita & "','" & datamorte & "','" & nfamiglia & "','" & posizione & "','" & azione & "','" & codvia & "')"
    '                    RegisterScript(sScript, Me.GetType())
    '                End If
    '            Next
    '        ElseIf e.CommandName = "RowEdit" Then
    '            For Each myRow As GridViewRow In GrdRes.Rows
    '                If IDRow = CType(myRow.FindControl("hfcod_contribuente"), HiddenField).Value Then
    '                    Session.Remove(ViewState("sessionName").ToString)

    '                    'Dim oAnagrafica As New GestioneAnagrafica
    '                    Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '                    oDettaglioAnagrafica.COD_CONTRIBUENTE = IDRow
    '                    oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = CType(myRow.FindControl("hfiddataanagrafica"), HiddenField).Value

    '                    Dim oAnagResidente As New AnagraficaResidente

    '                    oAnagResidente.Azione = "il " & myRow.Cells(11).Text & " " & myRow.Cells(10).Text
    '                    oAnagResidente.CodiceFiscale = myRow.Cells(2).Text
    '                    oAnagResidente.Cognome = myRow.Cells(0).Text
    '                    oAnagResidente.Nome = myRow.Cells(1).Text
    '                    oAnagResidente.Sesso = CType(myRow.FindControl("hfsesso"), HiddenField).Value
    '                    oAnagResidente.ComuneNascita = CType(myRow.FindControl("hfluogo_nascita"), HiddenField).Value
    '                    If Not CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value Is Nothing And CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value <> "" Then
    '                        oAnagResidente.DataNascita = CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value 'Left(e.Item.Cells(16).Text, 2) + "/" + Mid(e.Item.Cells(16).Text, 3, 2) + "/" + Right(e.Item.Cells(16).Text, 4)
    '                    End If
    '                    If Not CType(myRow.FindControl("hfdata_morte"), HiddenField).Value Is Nothing And CType(myRow.FindControl("hfdata_morte"), HiddenField).Value <> "" And CType(myRow.FindControl("hfdata_morte"), HiddenField).Value <> "31129999" Then
    '                        oAnagResidente.DataMorte = CType(myRow.FindControl("hfdata_morte"), HiddenField).Value 'Left(e.Item.Cells(17).Text, 2) + "/" + Mid(e.Item.Cells(17).Text, 3, 2) + "/" + Right(e.Item.Cells(17).Text, 4)
    '                    End If
    '                    oAnagResidente.ViaResidenza = CType(myRow.FindControl("hfvia"), HiddenField).Value
    '                    oAnagResidente.CivicoResidenza = CType(myRow.FindControl("hfnumero"), HiddenField).Value
    '                    oAnagResidente.EsponenteCivicoResidenza = CType(myRow.FindControl("hfesponente"), HiddenField).Value
    '                    oAnagResidente.InternoCivicoResidenza = CType(myRow.FindControl("hfinterno"), HiddenField).Value
    '                    oAnagResidente.CodFamiglia = myRow.Cells(8).Text
    '                    oAnagResidente.DescrParentela = myRow.Cells(9).Text
    '                    Session("DatiResidenti") = oAnagResidente

    '                    ViewState("sessionName") = "codContribuente"
    '                    Session(ViewState("sessionName").ToString) = oDettaglioAnagrafica
    '                    Dim sScript As String = "ApriRicercaAnagrafe('" & ViewState("sessionName").ToString & "')"
    '                    RegisterScript(sScript, Me.GetType())
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRowCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    Private Sub LoadDatiResidente(myRow As GridViewRow)
        Dim sScript As String = ""
        Try
            sScript += "$('#divDatiRes').append('"
            sScript += "<div class=\'modal-content\'>"
            sScript += "<div class=\'modal-header\'>"
            sScript += "<span class=\'closebtnalert\' onclick=\'GestModal(""divDatiRes"")\'>&times;</span>"
            sScript += "<h2>Dati Acquisiti - " + myRow.Cells(10).Text + "</h2>"
            sScript += "</div>"
            sScript += "<div class=\'col-md-11 modal-body\'>"
            sScript += "<p>"
            sScript += "<label class=\'Input_Emphasized\'>Cognome </label> "
            sScript += "<label class=\'Input_Label\'> " + myRow.Cells(0).Text.Replace("'", "&#39") + " </label> "
            sScript += "<label class=\'Input_Emphasized\'> Nome </label> "
            sScript += "<label class=\'Input_Label\'> " + myRow.Cells(1).Text.Replace("'", "&#39") + " </label> "
            sScript += "</p>"
            sScript += "<p>"
            sScript += "<label class=\'Input_Emphasized\'>Codice Fiscale </label> "
            sScript += "<label class=\'Input_Label\'> " + myRow.Cells(2).Text + " </label> "
            sScript += "<label class=\'Input_Emphasized\'> Sesso </label> "
            sScript += "<label class=\'Input_Label\'> " + CType(myRow.FindControl("hfsesso"), HiddenField).Value + " </label> "
            sScript += "</p>"
            sScript += "<p>"
            sScript += "<label class=\'Input_Emphasized\'>Nato/a a </label> "
            sScript += "<label class=\'Input_Label\'> " + CType(myRow.FindControl("hfluogo_nascita"), HiddenField).Value.Replace("'", "&#39") + " </label> "
            sScript += "<label class=\'Input_Emphasized\'> il </label> "
            sScript += "<label class=\'Input_Label\'>" + CType(myRow.FindControl("hfdata_nascita"), HiddenField).Value + "</label> "
            If Utility.StringOperation.FormatDateTime(CType(myRow.FindControl("hfdata_morte"), HiddenField).Value).Year <> 9999 Then
                sScript += "<label class=\'Input_Emphasized\'> Morto/a il </label> "
                sScript += "<label class=\'Input_Label\'>" + CType(myRow.FindControl("hfdata_morte"), HiddenField).Value + "</label> "
            End If
            sScript += "</p>"
            sScript += "<p>"
            sScript += "<label class=\'Input_Emphasized\'>Indirizzo </label> "
            sScript += "<label class=\'Input_Label\'> " + (CType(myRow.FindControl("hfvia"), HiddenField).Value + " " + CType(myRow.FindControl("hfnumero"), HiddenField).Value + " " + CType(myRow.FindControl("hflettera"), HiddenField).Value + " " + CType(myRow.FindControl("hfinterno"), HiddenField).Value).Replace("'", "&#39") + " </label> "
            sScript += "</p>"
            sScript += "<p>"
            sScript += "<label class=\'Input_Emphasized\'>Numero Famiglia </label> "
            sScript += "<label class=\'Input_Label\'> " + myRow.Cells(8).Text + " </label> "
            sScript += "<label class=\'Input_Emphasized\'> Posizione Famiglia </label> "
            sScript += "<label class=\'Input_Label\'> " + myRow.Cells(9).Text.Replace("'", "&#39") + " </label> "
            sScript += "</p>"
            sScript += "</div>"
            sScript += "</div>"
            sScript += "');"
            sScript += "$('#divDatiRes').removeClass();$('#divDatiRes').addClass('active');"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug("OPENgov.LeggiParametriEnte.LoadForm.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myDs As New DataSet
        Dim x As Integer

        Try
            myDs = CType(Session("DsAnag"), DataSet)
            For Each myRow As GridViewRow In GrdRes.Rows
                If CType(myRow.FindControl("ChkVerificato"), CheckBox).Checked = True Then
                    For x = 0 To myDs.Tables(0).Rows.Count - 1
                        If myDs.Tables(0).Rows(x)("MOVIMENTOID").ToString = CType(myRow.FindControl("hfmovimentoid"), HiddenField).Value Then
                            myDs.Tables(0).Rows(x)("trattato") = 1
                            Exit For
                        End If
                    Next
                Else
                    For x = 0 To myDs.Tables(0).Rows.Count - 1
                        If myDs.Tables(0).Rows(x)("MOVIMENTOID").ToString = CType(myRow.FindControl("hfmovimentoid"), HiddenField).Value Then
                            myDs.Tables(0).Rows(x)("trattato") = 0
                            Exit For
                        End If
                    Next
                End If
            Next
            Session("DsAnag") = myDs

        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRes_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdSorting(sender As Object, e As GridViewSortEventArgs)
        Try
            Dim myDataSet As New DataSet
            myDataSet = CType(Session("DsAnag"), DataSet)

            If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
                Select Case ViewState("OrderBy").ToString
                    Case "DESC"
                        ViewState("OrderBy") = "ASC"

                    Case "ASC"
                        ViewState("OrderBy") = "DESC"
                End Select
            Else
                ViewState("SortKey") = e.SortExpression
                ViewState("OrderBy") = "ASC"
            End If

            myDataSet.Tables(0).DefaultView.Sort = ViewState("SortKey").ToString & " " & ViewState("OrderBy").ToString
            Session("DsAnag") = myDataSet

            GrdRes.DataSource = myDataSet.Tables(0).DefaultView
            GrdRes.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdSorting.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdRes_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdRes.UpdateCommand
    '    Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '    Try
    '        'apro il popup passandogli l'indice dell'array da visualizzare
    '        Dim codfiscale As String = e.Item.Cells(2).Text
    '        Dim nome As String = e.Item.Cells(1).Text
    '        Dim cognome As String = e.Item.Cells(0).Text
    '        Dim sesso As String = e.Item.Cells(15).Text
    '        Dim datanascita As String = e.Item.Cells(16).Text
    '        Dim datamorte As String = e.Item.Cells(17).Text
    '        Dim luogonascita As String = e.Item.Cells(18).Text
    '        Dim nfamiglia As String = e.Item.Cells(8).Text
    '        Dim posizione As String = e.Item.Cells(9).Text
    '        Dim codvia As String = e.Item.Cells(19).Text
    '        Dim indirizzo As String = e.Item.Cells(20).Text + " " + e.Item.Cells(21).Text + " " + e.Item.Cells(22).Text + " " + e.Item.Cells(23).Text
    '        Dim azione As String = e.Item.Cells(10).Text

    '        Dim sScript As String = "ApriDatiAnagrafeResidenti('" & codfiscale & "','" & nome & "','" & cognome & "','" & indirizzo & "','" & sesso & "','" & datanascita & "','" & luogonascita & "','" & datamorte & "','" & nfamiglia & "','" & posizione & "','" & azione & "','" & codvia & "')"
    '        RegisterScript((Me.GetType(), "aprianag", "<script languageas string ='javascript'>" & sScript & "</script>")
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRes_Update.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdRes_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdRes.EditCommand
    '    'uso questo comando per aprire direttamente l'immobile senza passare dalla tessera
    '    Try
    '        Session.Remove(ViewState("sessionName"))

    '        'Dim oAnagrafica As New GestioneAnagrafica
    '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '        oDettaglioAnagrafica.COD_CONTRIBUENTE = e.Item.Cells(27).Text
    '        oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = e.Item.Cells(29).Text

    '        Dim oAnagResidente As New AnagraficaResidente

    '        oAnagResidente.Azione = "il " & e.Item.Cells(11).Text & " " & e.Item.Cells(10).Text
    '        oAnagResidente.CodiceFiscale = e.Item.Cells(2).Text
    '        oAnagResidente.Cognome = e.Item.Cells(0).Text
    '        oAnagResidente.Nome = e.Item.Cells(1).Text
    '        oAnagResidente.Sesso = e.Item.Cells(15).Text
    '        oAnagResidente.ComuneNascita = e.Item.Cells(18).Text
    '        If Not e.Item.Cells(16).Text Is Nothing And e.Item.Cells(16).Text <> "" Then
    '            oAnagResidente.DataNascita = e.Item.Cells(16).Text 'Left(e.Item.Cells(16).Text, 2) + "/" + Mid(e.Item.Cells(16).Text, 3, 2) + "/" + Right(e.Item.Cells(16).Text, 4)
    '        End If
    '        If Not e.Item.Cells(17).Text Is Nothing And e.Item.Cells(17).Text <> "" And e.Item.Cells(17).Text <> "31129999" Then
    '            oAnagResidente.DataMorte = e.Item.Cells(17).Text 'Left(e.Item.Cells(17).Text, 2) + "/" + Mid(e.Item.Cells(17).Text, 3, 2) + "/" + Right(e.Item.Cells(17).Text, 4)
    '        End If
    '        oAnagResidente.ViaResidenza = e.Item.Cells(20).Text
    '        oAnagResidente.CivicoResidenza = e.Item.Cells(21).Text
    '        oAnagResidente.EsponenteCivicoResidenza = e.Item.Cells(22).Text
    '        oAnagResidente.InternoCivicoResidenza = e.Item.Cells(23).Text
    '        oAnagResidente.CodFamiglia = e.Item.Cells(8).Text
    '        oAnagResidente.DescrParentela = e.Item.Cells(9).Text
    '        Session("DatiResidenti") = oAnagResidente

    '        ViewState("sessionName") = "codContribuente"
    '        Session(ViewState("sessionName")) = oDettaglioAnagrafica
    '        Dim sScript As String = "ApriRicercaAnagrafe('" & ViewState("sessionName").ToString & "')"
    '        RegisterScript((Me.GetType(), "aprianag", "<script languageas string ='javascript'>" & sScript & "</script>")
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRes_EditCommand.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdRes_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdRes.SortCommand
    '    Try
    '        Dim myDataSet As New DataSet
    '        myDataSet = CType(Session("DsAnag"), DataSet)

    '        If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
    '            Select Case ViewState("OrderBy")
    '                Case "DESC"
    '                    ViewState("OrderBy") = "ASC"

    '                Case "ASC"
    '                    ViewState("OrderBy") = "DESC"
    '            End Select
    '        Else
    '            ViewState("SortKey") = e.SortExpression
    '            ViewState("OrderBy") = "ASC"
    '        End If

    '        myDataSet.Tables(0).DefaultView.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '        Session("DsAnag") = myDataSet

    '        GrdRes.start_index = Convert.ToString(GrdRes.CurrentPageIndex)
    '        GrdRes.AllowCustomPaging = False
    '        GrdRes.DataSource = myDataSet.Tables(0).DefaultView
    '        GrdRes.DataBind()
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRes_SortCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdRes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim FncAnagRes As New ClsAnagrafeResidenti
    '    Dim itemGrid As GridViewRow
    '    Dim myDs As New DataSet
    '    Dim x As Integer

    '    Try
    '        myDs = CType(Session("DsAnag"), DataSet)
    '        For Each itemGrid In GrdRes.Items
    '            If CType(itemGrid.FindControl("ChkVerificato"), CheckBox).Checked = True Then
    '                For x = 0 To myDs.Tables(0).Rows.Count - 1
    '                    If myDs.Tables(0).Rows(x)("MOVIMENTOID").ToString = itemGrid.Cells(28).Text Then
    '                        myDs.Tables(0).Rows(x)("trattato") = 1
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                For x = 0 To myDs.Tables(0).Rows.Count - 1
    '                    If myDs.Tables(0).Rows(x)("MOVIMENTOID").ToString = itemGrid.Cells(28).Text Then
    '                        myDs.Tables(0).Rows(x)("trattato") = 0
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        Next
    '        Session("DsAnag") = myDs

    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRes_CheckedChanged.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        DivAttesa.Style.Add("display", "none")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdRes.DataSource = CType(Session("DsAnag"), DataSet)
            If page.HasValue Then
                GrdRes.PageIndex = page.Value
            End If
            GrdRes.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    '  Private Sub MyImgButton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles MyImgButton.Click
    '      Try
    '          Dim WFErrore As String = ""
    '          Dim WFSessione As New OPENUtility.CreateSessione(Session("PARAMETROENV"), Session("username"), Session("IDENTIFICATIVOAPPLICAZIONE"))
    '          If Not WFSessione.CreaSessione(Session("username"), WFErrore) Then
    '              Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '          End If

    '          Session.Remove(ViewState("sessionName"))

    '          Dim oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))

    '          Dim oDettaglioAnagrafica = New DettaglioAnagrafica

    '	If GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(6).Value <> 0 And Not GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(26).Value Is Nothing Then
    '		oDettaglioAnagrafica.COD_CONTRIBUENTE = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(26).Value

    '		Dim oAnagResidente As New DLL.AnagraficaResidente

    '		oAnagResidente.Azione = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(9).Value
    '		oAnagResidente.CodiceFiscale = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(2).Value
    '		oAnagResidente.Cognome = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(0).Value
    '		oAnagResidente.Nome = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(1).Value
    '		oAnagResidente.Sesso = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(14).Value
    '		oAnagResidente.ComuneNascita = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(17).Value
    '		oAnagResidente.DataNascita = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(15).Value
    '		oAnagResidente.DataMorte = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(16).Value
    '		oAnagResidente.ViaResidenza = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(19).Value
    '		oAnagResidente.CivicoResidenza = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(20).Value
    '		oAnagResidente.EsponenteCivicoResidenza = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(21).Value
    '		oAnagResidente.InternoCivicoResidenza = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(22).Value
    '		oAnagResidente.CodFamiglia = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(7).Value
    '		oAnagResidente.DescrParentela = GrdRes.Items(GrdRes.SelectedItemIndex.ToString()).Cells(8).Value
    '		Session("DatiResidenti") = oAnagResidente
    '	Else
    '		oDettaglioAnagrafica.COD_CONTRIBUENTE = -1
    '		Session("DatiResidenti") = Nothing
    '	End If
    '	oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = -1

    '	ViewState("sessionName") = "codContribuente"

    '	Session(ViewState("sessionName")) = oDettaglioAnagrafica

    '	writeJavascriptAnagrafica(ViewState("sessionName"))

    '	If Not IsNothing(WFSessione) Then
    '		WFSessione.Kill()
    '		WFSessione = Nothing
    '	End If
    'Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.MyImgButton_Click.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '      End Try
    '  End Sub


    'Private Sub GrdRes_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdRes.EditCommand
    '    'uso questo comando per aprire direttamente l'immobile senza passare dalla tessera
    '    Dim WFErrore As String
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(COSTANTValue.ConstSession.ParametroEnv, COSTANTValue.ConstSession.UserName, COSTANTValue.ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(COSTANTValue.ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Session.Remove(ViewState("sessionName"))

    '        Dim oAnagrafica As New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
    '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '        oDettaglioAnagrafica.COD_CONTRIBUENTE = e.Item.Cells(27).Text
    '        oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = e.Item.Cells(29).Text

    '        Dim oAnagResidente As New AnagraficaResidente

    '        oAnagResidente.Azione = "il " & e.Item.Cells(11).Text & " " & e.Item.Cells(10).Text
    '        oAnagResidente.CodiceFiscale = e.Item.Cells(2).Text
    '        oAnagResidente.Cognome = e.Item.Cells(0).Text
    '        oAnagResidente.Nome = e.Item.Cells(1).Text
    '        oAnagResidente.Sesso = e.Item.Cells(15).Text
    '        oAnagResidente.ComuneNascita = e.Item.Cells(18).Text
    '        If Not e.Item.Cells(16).Text Is Nothing And e.Item.Cells(16).Text <> "" Then
    '            oAnagResidente.DataNascita = e.Item.Cells(16).Text 'Left(e.Item.Cells(16).Text, 2) + "/" + Mid(e.Item.Cells(16).Text, 3, 2) + "/" + Right(e.Item.Cells(16).Text, 4)
    '        End If
    '        If Not e.Item.Cells(17).Text Is Nothing And e.Item.Cells(17).Text <> "" And e.Item.Cells(17).Text <> "31129999" Then
    '            oAnagResidente.DataMorte = e.Item.Cells(17).Text 'Left(e.Item.Cells(17).Text, 2) + "/" + Mid(e.Item.Cells(17).Text, 3, 2) + "/" + Right(e.Item.Cells(17).Text, 4)
    '        End If
    '        oAnagResidente.ViaResidenza = e.Item.Cells(20).Text
    '        oAnagResidente.CivicoResidenza = e.Item.Cells(21).Text
    '        oAnagResidente.EsponenteCivicoResidenza = e.Item.Cells(22).Text
    '        oAnagResidente.InternoCivicoResidenza = e.Item.Cells(23).Text
    '        oAnagResidente.CodFamiglia = e.Item.Cells(8).Text
    '        oAnagResidente.DescrParentela = e.Item.Cells(9).Text
    '        Session("DatiResidenti") = oAnagResidente

    '        ViewState("sessionName") = "codContribuente"
    '        Session(ViewState("sessionName")) = oDettaglioAnagrafica
    '        Dim sScript As String = "ApriRicercaAnagrafe('" & ViewState("sessionName").ToString & "')"
    '        RegisterScript((Me.GetType(),"aprianag", "<script languageas string ='javascript'>" & sScript & "</script>")
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.GrdRes_EditCommand.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Sub



    'Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click

    '    Dim WFSessioneAnagrafica As CreateSessione
    '    Dim WFSessione As CreateSessione
    '    Dim WFErrore As String = ""

    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(Session("PARAMETROENV"), Session("username"), Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(Session("username"), WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))

    '        Dim oDettaglioAnagrafica = New DettaglioAnagrafica

    '        oDettaglioAnagrafica = Session(ViewState("sessionName"))

    '        WFSessioneAnagrafica = New CreateSessione(Session("PARAMETROENV"), Session("username"), "OPENGOVA")
    '        If Not WFSessioneAnagrafica.CreaSessione(Session("username"), WFErrore) Then
    '            Throw New Exception("GestioneAnagrafeResidenti btnRibalta_Click::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Sub
    '        End If

    '        Dim ClsAnagRes As New ClsAnagrafeResidenti

    '        Dim objAnag As New OggettoAnagrafeResidenti

    '        Dim selindx As Integer
    '        selindx = GrdRes.SelectedIndex
    '        'popolo l'oggetto da utilizzare per l'update
    '        objAnag = New OggettoAnagrafeResidenti
    '        objAnag.COD_ENTE = Session("COD_ENTE")                                                        ''Codice Ente
    '        objAnag.COD_INDIVIDUALE = CInt(GrdRes.Items(selindx).Cells("COD_INDIVIDUALE").Text)       ''Codice Contribuente
    '        objAnag.COGNOME = CStr(GrdRes.Items(selindx).Cells("COGNOME").Text)                       ''Cognome
    '        objAnag.NOME = CStr(GrdRes.Items(selindx).Cells("NOME").Text)                               ''nome
    '        objAnag.SESSO = CStr(GrdRes.Items(selindx).Cells("SESSO").Text)                           ''sesso
    '        objAnag.DATA_NASCITA = CStr(GrdRes.Items(selindx).Cells("DATA_NASCITA").Text)               ''data nascita ddmmyyyy
    '        objAnag.DATA_MORTE = CStr(GrdRes.Items(selindx).Cells("DATA_MORTE").Text)                   ''data morte ddmmyyyy
    '        objAnag.LUOGO_NASCITA = CStr(GrdRes.Items(selindx).Cells("LUOGO_NASCITA").Text)           ''luogo nascita
    '        objAnag.COD_FISCALE = CStr(GrdRes.Items(selindx).Cells("COD_FISCALE").Text)               ''codice fiscale
    '        objAnag.COD_VIA = CInt(GrdRes.Items(selindx).Cells("COD_VIA").Text)                       ''codice via
    '        objAnag.NUMERO = CStr(GrdRes.Items(selindx).Cells("NUMERO").Text)                           ''numero
    '        objAnag.LETTERA = CStr(GrdRes.Items(selindx).Cells("LETTERA").Text)                       ''lettera
    '        objAnag.INTERNO = CStr(GrdRes.Items(selindx).Cells("INTERNO").Text)                       ''numero
    '        objAnag.NUMERO_FAMIGLIA = CInt(GrdRes.Items(selindx).Cells("NUMERO_FAMIGLIA").Text)       ''numero famiglia
    '        objAnag.CODICE_POSIZIONE_FAMIGLIA = CInt(GrdRes.Items(selindx).Cells("CODICE_POSIZIONE_FAMIGLIA").Text)              ''codice posizione famigliare
    '        objAnag.CODICE_AZIONE = CInt(GrdRes.Items(selindx).Cells("CODICE_AZIONE").Text)              ''codice azione

    '        ClsAnagRes.SetAnagrafeResidenti(objAnag, 1, WFSessione, WFSessioneAnagrafica, oDettaglioAnagrafica.COD_CONTRIBUENTE)

    '        objAnag = Nothing
    '        Session("DatiResidenti") = Nothing
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.btnRibalta_Click.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessioneAnagrafica.Kill()
    '        WFSessioneAnagrafica = Nothing
    '        WFSessione.Kill()
    '        WFSessione = Nothing
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
        Dim FncAnagRes As New ClsAnagrafeResidenti
        Dim selindx As Integer
        Dim sScript As String

        Try
            oDettaglioAnagrafica = Session(ViewState("sessionName").ToString)
            selindx = GrdRes.SelectedIndex
            If FncAnagRes.SetResidentiVSTributi(CInt(GrdRes.Rows(selindx).Cells("COD_INDIVIDUALE").Text), oDettaglioAnagrafica.COD_CONTRIBUENTE) = False Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in abbinamento Residente ad Anagrafe Tributi!');"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'success', '', '', 'Abbinamento Residente ad Anagrafe Tributi effettuato con successo!');"
                RegisterScript(sScript, Me.GetType())
            End If
            Session("DatiResidenti") = Nothing
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="04/07/2012">
    ''' <strong>IMU</strong>
    ''' passaggio tributo da ICI a IMU
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnStampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaExcel.Click
        Dim sNameXLS As String
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim nCol As Integer = 19
        Dim x, i As Integer
        Dim dvDati As DataView

        Try
            If Not IsNothing(Session("DsAnag")) Then
                ds = CType(Session("DsAnag"), DataSet)
                dvDati = ds.Tables(0).DefaultView
                ds = New DataSet
                ds.Tables.Add("STAMPA_ANAGRAFICHE")
                For x = 1 To nCol + 1
                    ds.Tables("STAMPA_ANAGRAFICHE").Columns.Add("Col" & x.ToString.PadLeft(3, CChar("0")))
                Next

                DtDatiStampa = ds.Tables("STAMPA_ANAGRAFICHE")
                dr = DtDatiStampa.NewRow
                dr(0) = "Prospetto Anagrafiche"
                dr(2) = "Data Stampa:" & DateTime.Now.Date
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow

                x = 0
                dr(x) = "Cognome"
                x += 1
                dr(x) = "Nome"
                x += 1
                dr(x) = "Codice Fiscale"
                x += 1
                dr(x) = "Indirizzo"
                x += 1
                dr(x) = "Data Nascita"
                x += 1
                dr(x) = "Luogo Nascita"
                x += 1
                dr(x) = "Sesso"
                '*** 20120704 - IMU ***
                x += 1
                dr(x) = "ICI/IMU"
                '*** ***
                x += 1
                dr(x) = "TARSU"
                If bHasOSAP = True Then
                    x += 1
                    dr(x) = "OSAP"
                End If
                If bHasH2O = True Then
                    x += 1
                    dr(x) = "H2O"
                End If
                x += 1
                dr(x) = "PROV"
                x += 1
                dr(x) = "Numero Famiglia"
                x += 1
                dr(x) = "Posizione"
                x += 1
                dr(x) = "Cod Individuale"
                x += 1
                dr(x) = "Azione"
                x += 1
                dr(x) = "Data Movimento"
                DtDatiStampa.Rows.Add(dr)
                For i = 0 To dvDati.Count - 1
                    dr = DtDatiStampa.NewRow
                    x = 0
                    dr(x) = CStr(dvDati.Item(i)("COGNOME"))
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("NOME"))
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("COD_FISCALE"))
                    x += 1
                    dr(x) = (CStr(dvDati.Item(i)("VIA")) + " " + CStr(dvDati.Item(i)("NUMERO")) + " " + CStr(dvDati.Item(i)("LETTERA")) + " " + CStr(dvDati.Item(i)("INTERNO"))).Trim
                    x += 1
                    If CStr(dvDati.Item(i)("DATA_NASCITA")) <> "" Then
                        dr(x) = Left(CStr(dvDati.Item(i)("DATA_NASCITA")), 2) + "/" + Mid(CStr(dvDati.Item(i)("DATA_NASCITA")), 3, 2) + "/" + Right(CStr(dvDati.Item(i)("DATA_NASCITA")), 4)
                    Else
                        dr(x) = CStr(dvDati.Item(i)("DATA_NASCITA"))
                    End If
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("LUOGO_NASCITA"))
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("SESSO"))
                    x += 1
                    If CStr(dvDati.Item(i)("ICI")) = "1" Then
                        dr(x) = "X"
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If CStr(dvDati.Item(i)("TARSU")) = "1" Then
                        dr(x) = "X"
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If bHasOSAP = True Then
                        If CStr(dvDati.Item(i)("OSAP")) = "1" Then
                            dr(x) = "X"
                        Else
                            dr(x) = ""
                        End If
                    End If
                    If bHasH2O = True Then
                        If CStr(dvDati.Item(i)("H2O")) = "1" Then
                            dr(x) = "X"
                        Else
                            dr(x) = ""
                        End If
                    End If
                    x += 1
                    If CStr(dvDati.Item(i)("PROVVEDIMENTI")) = "1" Then
                        dr(x) = "X"
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("NUMERO_FAMIGLIA"))
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("DESCRIZIONE_POS"))
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("COD_INDIVIDUALE"))
                    x += 1
                    dr(x) = CStr(dvDati.Item(i)("DESCRIZIONE"))
                    x += 1
                    If IsDate(dvDati.Item(i)("DATA_MOVIMENTO")) Then
                        dr(x) = CDate(dvDati.Item(i)("DATA_MOVIMENTO")).ToShortDateString
                    Else
                        dr(x) = CStr(dvDati.Item(i)("DATA_MOVIMENTO"))
                    End If
                    DtDatiStampa.Rows.Add(dr)
                Next i

                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                dr(0) = "Totale Anagrafiche Residenti: " & (dvDati.Count)
                DtDatiStampa.Rows.Add(dr)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.GestioneAnagrafeResidenti.btnStampaExcel_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(dvDati) Then
            'valorizzo il nome del file
            sNameXLS = COSTANTValue.ConstSession.IdEnte & "_ANAGRAFICHE_RESIDENTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub

End Class
