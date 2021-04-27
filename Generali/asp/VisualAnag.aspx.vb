Imports log4net
Imports AnagInterface
Imports Anagrafica.DLL
''' <summary>
''' Pagina per la visualizzazione della posizione anagrafica.
''' Le possibili opzioni sono:
''' Contiene i dati della posizione e le funzioni per la ricerca e l'inserimento in residenti.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Public Class VisualAnag
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("VisualAnag")
    Protected FncGrd As New FunctionGrd
    Private oDettaglioAnagrafica As DettaglioAnagrafica
    Private FncAnagRes As New ClsAnagrafeResidenti
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                If Not Session("oAnagrafe") Is Nothing Then
                    oDettaglioAnagrafica = Session("oAnagrafe")
                    LoadAnagrafica(oDettaglioAnagrafica)
                ElseIf Not Request.Item("IdContribuente") Is Nothing Then
                    If IsNumeric(Request.Item("IdContribuente")) Then
                        Dim oMyAnagrafica As New GestioneAnagrafica()
                        Session("myFamigliaResidenti") = Nothing
                        oDettaglioAnagrafica = oMyAnagrafica.GetAnagrafica(CInt(Request.Item("IdContribuente")), Utility.Costanti.INIT_VALUE_NUMBER, "", COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica, False)
                        LoadAnagrafica(oDettaglioAnagrafica)
                        Session("oAnagrafe") = oDettaglioAnagrafica
                    End If
                End If
                'se la sessione non coindice con la request ricarico da request
                If Not Request.Item("IdContribuente") Is Nothing Then
                    If oDettaglioAnagrafica.COD_CONTRIBUENTE <> Utility.StringOperation.FormatInt(Request.Item("IdContribuente")) Then
                        Dim oMyAnagrafica As New GestioneAnagrafica()
                        Session("myFamigliaResidenti") = Nothing
                        oDettaglioAnagrafica = oMyAnagrafica.GetAnagrafica(CInt(Request.Item("IdContribuente")), Utility.Costanti.INIT_VALUE_NUMBER, "", COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica, False)
                        LoadAnagrafica(oDettaglioAnagrafica)
                        Session("oAnagrafe") = oDettaglioAnagrafica
                    End If
                End If
            End If
                LnkPulisciContr.Attributes.Add("onclick", "return ClearDatiContrib();")
            LnkAnagAnater.Attributes.Add("onclick", "ApriRicAnater();")

            If Not Request.Item("Azione") Is Nothing Then
                If Request.Item("Azione") = Utility.Costanti.AZIONE_LETTURA Then
                    LnkAnagTributi.Style.Add("display", "none")
                    LnkAnagAnater.Style.Add("display", "none")
                    LnkPulisciContr.Style.Add("display", "none")
                ElseIf Request.Item("Azione") = Utility.Costanti.AZIONE_SELEZIONE Then
                    LnkPulisciContr.Style.Add("display", "none")
                End If
            End If

            If COSTANTValue.ConstSession.HasANATER = False Then
                LnkAnagAnater.Style.Add("display", "none")
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Try
    '        If Page.IsPostBack = False Then
    '            If Not Session("oAnagrafe") Is Nothing Then
    '                oDettaglioAnagrafica = Session("oAnagrafe")
    '                LoadAnagrafica(oDettaglioAnagrafica)
    '            ElseIf Not Request.Item("IdContribuente") Is Nothing Then
    '                If IsNumeric(Request.Item("IdContribuente")) Then
    '                    Dim oMyAnagrafica As New GestioneAnagrafica()
    '                    Session("myFamigliaResidenti") = Nothing
    '                    oDettaglioAnagrafica = oMyAnagrafica.GetAnagrafica(CInt(Request.Item("IdContribuente")), Utility.Costanti.INIT_VALUE_NUMBER, "", COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica, False)
    '                    LoadAnagrafica(oDettaglioAnagrafica)
    '                    Session("oAnagrafe") = oDettaglioAnagrafica
    '                End If
    '            End If
    '        End If
    '        LnkPulisciContr.Attributes.Add("onclick", "return ClearDatiContrib();")
    '        LnkAnagAnater.Attributes.Add("onclick", "ApriRicAnater();")

    '        If Not Request.Item("Azione") Is Nothing Then
    '            If Request.Item("Azione") = Utility.Costanti.AZIONE_LETTURA Then
    '                LnkAnagTributi.Style.Add("display", "none")
    '                LnkAnagAnater.Style.Add("display", "none")
    '                LnkPulisciContr.Style.Add("display", "none")
    '            End If
    '        End If

    '        If COSTANTValue.ConstSession.HasANATER = False Then
    '            LnkAnagAnater.Style.Add("display", "none")
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkAnagTributi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagTributi.Click
        Try
            If Not ViewState("sessionName") Is Nothing Then
                Session.Remove(ViewState("sessionName").ToString)
            End If
            oDettaglioAnagrafica = New DettaglioAnagrafica
            oDettaglioAnagrafica.COD_CONTRIBUENTE = hfCodContribuente.Value
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = hfIdDataAnagrafica.Value
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName").ToString) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName").ToString)
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.LnkAnagTributi-Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Try
            If Not Session(ViewState("sessionName").ToString) Is Nothing Then
                oDettaglioAnagrafica = Session(ViewState("sessionName").ToString)
                Session("oAnagrafe") = oDettaglioAnagrafica
                LoadAnagrafica(oDettaglioAnagrafica)
                ViewState("sessionName") = ""
                Session("SEARCHPARAMETRES") = Nothing
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.btnRibalta_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibaltaAnagAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaAnagAnater.Click
        Try
            If Not IsNothing(Session("AnagrafeAnaterRibaltata")) Then
                oDettaglioAnagrafica = CType(Session("AnagrafeAnaterRibaltata"), DettaglioAnagrafica)
                Session("oAnagrafe") = oDettaglioAnagrafica

                LoadAnagrafica(oDettaglioAnagrafica)
                ' Pulisco la variabile di sessione della anagrafica di anater.
                Session.Remove("AnagrafeAnaterRibaltata")
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.btnRibaltaAnagAnater.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ibNewRes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibNewRes.Click
        Dim sScript As String = "<script language='javascript'>"
        Dim IdContribuente As Integer = 0
        Try
            If hfCodContribuente.Value <> "" Then
                IdContribuente = Integer.Parse(hfCodContribuente.Value)
            End If
            If IdContribuente > 0 Then
                FncAnagRes.SetResidentiFromTributi(COSTANTValue.ConstSession.StringConnectionAnagrafica, COSTANTValue.ConstSession.IdEnte, IdContribuente)
            Else
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare un contribuente!');"
            End If
            sScript += "</script>"
            ClientScript.RegisterClientScriptBlock(Me.[GetType](), "anag", sScript)
            Dim dvAnagRes As DataView = FncAnagRes.GetFamigliaResidenti(COSTANTValue.ConstSession.StringConnectionAnagrafica, oDettaglioAnagrafica.COD_CONTRIBUENTE)
            If Not dvAnagRes Is Nothing Then
                If dvAnagRes.Table.Rows.Count > 0 Then
                    GrdFamiglia.DataSource = dvAnagRes
                    GrdFamiglia.DataBind()
                    GrdFamiglia.SelectedIndex = -1
                    LblResultFamiglia.Style.Add("display", "none")
                    ibNewRes.Style.Add("display", "none")
                Else
                    LblResultFamiglia.Style.Add("display", "")
                    ibNewRes.Style.Add("display", "")
                End If
            Else
                LblResultFamiglia.Style.Add("display", "")
                ibNewRes.Style.Add("display", "")
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.ibNewRes_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nomeSessione"></param>
    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
        Try
            Dim sScript As String
            Dim csType As Type = Me.[GetType]()
            sScript = "<script language='javascript'>"
            sScript += "ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
            sScript += "</script>"
            ClientScript.RegisterClientScriptBlock(csType, "anag", sScript)
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.writeJavascriptAnagrafica.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDettaglioAnagrafica"></param>
    Private Sub LoadAnagrafica(ByVal oDettaglioAnagrafica As DettaglioAnagrafica)
        Dim dvAnagRes As DataView
        Dim sScript As String = "<script language='javascript'>"
        Dim csType As Type = Me.[GetType]()

        Try
            txtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale
            txtPIVA.Text = oDettaglioAnagrafica.PartitaIva
            txtCognome.Text = oDettaglioAnagrafica.Cognome
            txtNome.Text = oDettaglioAnagrafica.Nome
            Select Case oDettaglioAnagrafica.Sesso
                Case "F"
                    rdbFemmina.Checked = True
                Case "G"
                    rdbGiuridica.Checked = True
                Case "M"
                    rdbMaschio.Checked = True
            End Select
            txtDataNasc.Text = oDettaglioAnagrafica.DataNascita
            txtComNasc.Text = oDettaglioAnagrafica.ComuneNascita
            txtProvNasc.Text = oDettaglioAnagrafica.ProvinciaNascita
            txtViaRes.Text = oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza
            If oDettaglioAnagrafica.EsponenteCivicoResidenza <> "" Then
                txtViaRes.Text += " " & oDettaglioAnagrafica.EsponenteCivicoResidenza
            End If
            If oDettaglioAnagrafica.ScalaCivicoResidenza <> "" Then
                txtViaRes.Text += " Sc." & oDettaglioAnagrafica.ScalaCivicoResidenza
            End If
            If oDettaglioAnagrafica.InternoCivicoResidenza <> "" Then
                txtViaRes.Text += " Int." & oDettaglioAnagrafica.InternoCivicoResidenza
            End If
            txtCapRes.Text = oDettaglioAnagrafica.CapResidenza
            txtComuneRes.Text = oDettaglioAnagrafica.ComuneResidenza
            txtProvRes.Text = oDettaglioAnagrafica.ProvinciaResidenza

            hfCodContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString
            hfIdDataAnagrafica.Value = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA.ToString
            If Session("myFamigliaResidenti") Is Nothing Then
                dvAnagRes = FncAnagRes.GetFamigliaResidenti(COSTANTValue.ConstSession.StringConnectionAnagrafica, oDettaglioAnagrafica.COD_CONTRIBUENTE)
                Session("myFamigliaResidenti") = dvAnagRes
            Else
                dvAnagRes = Session("myFamigliaResidenti")
            End If
            If Not dvAnagRes Is Nothing Then
                If dvAnagRes.Table.Rows.Count > 0 Then
                    GrdFamiglia.DataSource = dvAnagRes
                    GrdFamiglia.DataBind()
                    GrdFamiglia.SelectedIndex = -1
                    LblResultFamiglia.Style.Add("display", "none")
                    ibNewRes.Style.Add("display", "none")
                Else
                    LblResultFamiglia.Style.Add("display", "")
                    ibNewRes.Style.Add("display", "")
                End If
            Else
                LblResultFamiglia.Style.Add("display", "")
                ibNewRes.Style.Add("display", "")
            End If
            If oDettaglioAnagrafica.COD_CONTRIBUENTE > 0 Then
                sScript += "parent.document.getElementById('hdIdContribuente').value=" & oDettaglioAnagrafica.COD_CONTRIBUENTE
                sScript += "</script>"
                ClientScript.RegisterClientScriptBlock(csType, "idcontrpar" & Now.ToLongDateString, sScript)
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.VisualAnag.LoadAnagrafica.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class