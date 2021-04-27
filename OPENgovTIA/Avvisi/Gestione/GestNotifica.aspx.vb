Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la gestione delle date di notifica.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
'''
''' Sarà possibile impostare una data su ogni singola riga; per salvare la data di notifica bisognerà cliccare sul pulsante di salvataggio. La data sarà automaticamente inserita anche sulle ingiunzioni. Sarà utilizzata la stessa logica per la cancellazione delle date. Sarà inoltre possibile stampare l’elenco delle posizioni in griglia.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestNotifica
    Inherits BaseEnte
    Private Log As ILog = LogManager.GetLogger(GetType(GestNotifica))
    Protected FncGrd As New Formatta.FunctionGrd

    Public Class Action
        Public Const Unique As Integer = 1
        Public Const Copy As Integer = 2
        Public Const Delete As Integer = 3
    End Class
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        lblTitolo.Text = ConstSession.DescrizioneEnte
        If ConstSession.IsFromTARES = "1" Then
            info.InnerText = "TARI "
        Else
            info.InnerText = "TARSU "
        End If
        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            info.InnerText += "Variabile "
        End If
        info.InnerText += " - Avvisi - Notifiche"
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Try
            txtCognome.Attributes.Add("onkeydown", "keyPress();")
            txtNome.Attributes.Add("onkeydown", "keyPress();")
            txtCFPIVA.Attributes.Add("onkeydown", "keyPress();")
            txtCodCartella.Attributes.Add("onkeydown", "keyPress();")
            txtDataEmissione.Attributes.Add("onkeydown", "keyPress();")
            ddlAnno.Attributes.Add("onkeydown", "keyPress();")
            ddlTipoRuolo.Attributes.Add("onkeydown", "keyPress();")
            'Put user code to initialize the page here
            If Page.IsPostBack = False Then
                sSQL = "SELECT *"
                sSQL += " FROM V_GETANNIRUOLO"
                sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
                sSQL += " ORDER BY DESCRIZIONE DESC"
                oLoadCombo.LoadComboGenerale(ddlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                'popolo combo tiporuolo
                sSQL = "SELECT DISTINCT DESCRIZIONE, IDTIPORUOLO, ORDINAMENTO"
                sSQL += " FROM V_GETTIPORUOLO"
                sSQL += " ORDER BY ORDINAMENTO"
                oLoadCombo.LoadComboGenerale(ddlTipoRuolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                If Not Session("mySearchForAvvisi") Is Nothing Then
                    Dim mySearchForAvvisi As ArrayList
                    Dim x As Integer = 0
                    mySearchForAvvisi = CType(Session("mySearchForAvvisi"), ArrayList)
                    If mySearchForAvvisi(x) <> "" Then
                        txtCognome.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        txtNome.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        txtCFPIVA.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        ddlAnno.SelectedValue = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        ddlTipoRuolo.SelectedValue = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        txtDataEmissione.Text = mySearchForAvvisi(x)
                    End If
                    x += 1
                    If mySearchForAvvisi(x) <> "" Then
                        txtCodCartella.Text = mySearchForAvvisi(x)
                    End If
                    CmdSearch_Click(Nothing, Nothing)
                    Session("mySearchForAvvisi") = Nothing
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Sgravio, "Notifica", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Bottoni"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSearch.Click
        Dim listAvvisi() As ObjAvviso
        Dim FncAvvisi As New GestAvviso

        Try
            listAvvisi = FncAvvisi.GetAvviso(ConstSession.StringConnection, -1, ConstSession.IdEnte, -1, ddlAnno.SelectedValue, ddlTipoRuolo.SelectedValue, "", txtCognome.Text, txtNome.Text, txtCFPIVA.Text, txtCodCartella.Text, "", "", False, False, False, False, txtDataEmissione.Text, -1, Nothing)
            If Not IsNothing(listAvvisi) Then
                If listAvvisi.Length > 0 Then
                    Session.Add("ListAvvisiNotif", listAvvisi)
                    GrdAvvisi.DataSource = listAvvisi
                    GrdAvvisi.DataBind()
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                End If
            End If
            Dim mySearchForAvvisi As New ArrayList
            mySearchForAvvisi.Add(txtCognome.Text)
            mySearchForAvvisi.Add(txtNome.Text)
            mySearchForAvvisi.Add(txtCFPIVA.Text)
            mySearchForAvvisi.Add(ddlAnno.SelectedValue)
            mySearchForAvvisi.Add(ddlTipoRuolo.SelectedValue)
            mySearchForAvvisi.Add(txtDataEmissione.Text)
            mySearchForAvvisi.Add(txtCodCartella.Text)
            Session("mySearchForAvvisi") = mySearchForAvvisi
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.CmdSearch_Click.errore: ", ex)
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
    Private Sub CmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSave.Click
        Dim listAvvisi() As ObjAvviso
        Dim sScript As String

        Try
            LoadNotifiche(Action.Unique)
            listAvvisi = CType(Session("ListAvvisiNotif"), ObjAvviso())
            For Each myAvviso As ObjAvviso In listAvvisi
                If New GestAvviso().SetNotifica(ConstSession.StringConnection, myAvviso) = False Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in salvataggio!')"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            Next
            sScript = "GestAlert('a', 'success', '', '', 'Salvataggio effettuato con successo!')"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.CmdSave_Click.errore: ", ex)
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
    Private Sub CmdPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdPrint.Click
        Dim sNameXLS As String
        Dim DvDati As New DataView
        Dim FncAvvisi As New GestAvviso
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        nCol = 13
        Try
            DvDati = FncAvvisi.GetStampaAvvisi(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue, ddlTipoRuolo.SelectedValue, txtCognome.Text, txtNome.Text, txtCFPIVA.Text, txtCodCartella.Text, False, txtDataEmissione.Text)
            DtDatiStampa = FncStampa.PrintAvvisi(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES, nCol)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.CmdPrint_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DvDati.Dispose()
        End Try

        If Not DtDatiStampa Is Nothing Then
            sNameXLS = ConstSession.IdEnte & "_ELENCO_NOTIFICHE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        Else
            Dim sScript As String = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType)
        End If
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
                Dim sScript As String = "LoadAvvisi('" & ConstSession.IsFromVariabile & "','" & IDRow & "','-1','" & Utility.Costanti.AZIONE_UPDATE & "')"
                RegisterScript(sScript, Me.GetType())
            ElseIf e.CommandName = "RowCopy" Then
                LoadNotifiche(Action.Copy)
                LoadSearch(0)
            ElseIf e.CommandName = "RowDelete" Then
                LoadNotifiche(Action.Delete)
                LoadSearch(0)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        Try
            LoadNotifiche(Action.Unique)
            LoadSearch(e.NewPageIndex)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.GrdPageIndexChanging.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAvvisi.DataSource = CType(Session("ListAvvisiNotif"), ObjAvviso())
            If page.HasValue Then
                GrdAvvisi.PageIndex = page.Value
            End If
            GrdAvvisi.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myAction"></param>
    Private Sub LoadNotifiche(myAction As Integer)
        Try
            Dim listAvvisi() As ObjAvviso
            listAvvisi = CType(Session("ListAvvisiNotif"), ObjAvviso())
            Select Case myAction
                Case Action.Unique
                    For Each myRow As GridViewRow In GrdAvvisi.Rows
                        For Each myAvviso As ObjAvviso In listAvvisi
                            If CType(myRow.FindControl("hfid"), HiddenField).Value = myAvviso.ID Then
                                If CType(myRow.FindControl("txtDataNotifica"), TextBox).Text <> "" Then
                                    myAvviso.tDataNotifica = CDate(CType(myRow.FindControl("txtDataNotifica"), TextBox).Text)
                                Else
                                    myAvviso.tDataNotifica = DateTime.MaxValue
                                End If
                                Exit For
                            End If
                        Next
                    Next
                Case Action.Copy
                    If CType(GrdAvvisi.HeaderRow.FindControl("txtDataNotificaAll"), TextBox).Text <> "" Then
                        For Each myAvviso As ObjAvviso In listAvvisi
                            myAvviso.tDataNotifica = CDate(CType(GrdAvvisi.HeaderRow.FindControl("txtDataNotificaAll"), TextBox).Text)
                        Next
                    Else
                        RegisterScript("GestAlert('a', 'warning', '', '', 'Inserire una data di notifica!');", Me.GetType)
                    End If
                Case Action.Delete
                    For Each myAvviso As ObjAvviso In listAvvisi
                        myAvviso.tDataNotifica = DateTime.MaxValue
                    Next
            End Select
            Session("ListAvvisiNotif") = listAvvisi
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestNotifica.LoadNotifiche.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class