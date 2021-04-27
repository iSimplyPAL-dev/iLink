Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la generazione massiva dei provvedimenti per differenze fra dichiarato e versato (Fase 2).
''' Le possibili opzioni sono:
''' - Ricerca
''' - Approva minuta
''' - Stampa minuta
''' - Calcola
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GenMassiva
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GenMassiva))
    Protected FncGrd As New OPENgovTIA.Formatta.FunctionGrd
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        lblTitolo.Text = ConstSession.DescrizioneEnte
        info.InnerText = "Generazione Massiva Atti"
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Page.IsPostBack = False Then
                Dim myUtil As New MyUtility

                myUtil.FillDropDownSQLString(ddlAnno, New DBPROVVEDIMENTI.ProvvedimentiDB().GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, ""), -1, "TUTTI")
                'controllo se stro facendo un'elaborazione
                Dim CalcoloInCorso As Integer = CacheManager.GetElaborazioneMassivaInCorso
                If (CalcoloInCorso <> -1) Then
                    ShowCalcoloInCorso()
                ElseIf (Not (Session("AttiMassiviInCorso")) Is Nothing) Then
                    ddlAnno.SelectedValue = CType(Session("AttiMassiviInCorso"), Integer)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Bottoni"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub CmdCalcola_Click(sender As Object, e As EventArgs)
        Dim ListTributi As String = ""
        Dim FncElab As New ClsMassiva
        Try
            CacheManager.RemoveRiepilogoElaborazioneMassiva()
            CacheManager.RemoveAvanzamentoElaborazioneMassiva()
            If chk8852.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_ICI
            End If
            If chkTASI.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_TASI
            End If
            If chk0434.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_TARSU
            End If
            If chk0453.Checked Then
                ListTributi += "," + Utility.Costanti.TRIBUTO_OSAP
            End If
            ListTributi = ListTributi.Substring(1, ListTributi.Length - 1)
            ShowCalcoloInCorso()
            FncElab.StartElaborazione(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.StringConnectionICI, ConstSession.StringConnectionTARSU, ConstSession.StringConnectionOSAP, ConstSession.IdEnte, ConstSession.UserName, ddlAnno.SelectedValue, ListTributi, CInt(txtSoglia.Text), CInt(txtGGScad.Text))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.Page_Load.errore: ", ex)
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
    Protected Sub CmdSearch_Click(sender As Object, e As EventArgs)
        Dim listAtti() As OggettoAtto

        Try
            listAtti = New ClsMassiva().GetAtti(ConstSession.StringConnection, ConstSession.IdEnte)
            If Not IsNothing(listAtti) Then
                If listAtti.Length > 0 Then
                    Session.Add("listAttiMassivi", listAtti)
                    GrdResult.DataSource = listAtti
                    GrdResult.DataBind()
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.Page_Load.errore: ", ex)
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
    Protected Sub CmdPrintMinuta_Click(sender As Object, e As EventArgs)
        Try
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.Page_Load.errore: ", ex)
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
    Protected Sub CmdOKMinuta_Click(sender As Object, e As EventArgs)
        Try
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
#End Region
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Dim myParam As String = "?IDPROVVEDIMENTO=" & IDRow
                myParam += "&TIPOTRIBUTO="
                myParam += "&ANNO="
                myParam += "&NUMEROATTO="
                myParam += "&IDTIPOPROVVEDIMENTO="
                myParam += "&DESCTRIBUTO="
                myParam += "&TIPOPROCEDIMENTO="
                myParam += "&PAGINAPRECEDENTE=MASSIVA"
                Session("ParamGestioneAtti") = myParam
                Dim sScript As String = "location.href='../GestioneAtti/GestioneAtti.aspx" & myParam & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        Try
            LoadSearch(e.NewPageIndex)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.GrdPageIndexChanging.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdResult.DataSource = CType(Session("listAttiMassivi"), OggettoAtto())
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovProvvedimenti.GenMassiva.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ShowCalcoloInCorso()
        DivAttesa.Style.Add("display", "")
        Response.AppendHeader("refresh", "5")
        Session("AttiMassiviInCorso") = ddlAnno.SelectedValue
        LblAvanzamento.Text = CacheManager.GetAvanzamentoElaborazioneMassiva
    End Sub
End Class