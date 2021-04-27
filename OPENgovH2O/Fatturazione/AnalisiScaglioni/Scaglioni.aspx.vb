Imports log4net
Imports System.Data.SqlClient
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports System.Collections
Imports System.Collections.Comparer
Imports Utility
Imports RemotingInterfaceMotoreH2O.RemotingInterfaceMotoreH2O
''' <summary>
''' Pagina per il calcolo dei consumi su scaglione in un dato periodo di fatturazione.
''' Il calcolo viene fatto sugli scaglioni configurati, ma è anche possibile variarne la misura al momento, tramite apposito pulsante. Le variazioni di scaglioni non saranno registrate in banca dati.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class Scaglioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Scaglioni))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents AddScaglioni As System.Web.UI.HtmlControls.HtmlGenericControl

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
    ''' Al caricamento della pagina carica la combo con i possibili periodi da elaborare.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If (Not Page.IsPostBack) Then
                Dim paginacomandi As String = Request("paginacomandi")
                Dim parametri As String = "?"
                'parametri = "?title=Acquedotto - " & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("DE ", "") & " - " & "Ricerca" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
                If Len(paginacomandi) = 0 Then
                    'paginacomandi = "/OpenUtenzeGC/ComandiRicerca/ComandiRicerca.aspx"
                    paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/Fatturazione/AnalisiScaglioni/ComandiScaglioni.aspx"
                End If

                Dim sScript As String = ""
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())

                GrdAddScaglioni.Visible = False
                Session("UsaScaglioniMod") = False
                LoadDDL()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Bottoni"
    ''' <summary>
    ''' Richiama la ricerca Scaglioni e consumo per periodo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Ricerca()
    End Sub
    Private Sub btnPulisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        Session("UsaScaglioniMod") = False
        Session.Remove("Scaglioni")
        ddlPeriodo.SelectedItem.Selected = False
        ddlPeriodo.Items(0).Selected = True
        GrdScaglioni.Visible = False
        GrdAddScaglioni.Visible = False
        DivAttesa.Style.Add("display", "none")
        divModScaglioni.Attributes.Add("style", "display='none'")
    End Sub
    Private Sub btnModificaScaglioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificaScaglioni.Click
        Dim sScript As String
        Dim oMyTariffe As OggettoScaglione()

        Try
            If Not Session("Scaglioni") Is Nothing Then
                oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())

                GrdScaglioni.Visible = False
                GrdAddScaglioni.DataSource = oMyTariffe
                GrdAddScaglioni.DataBind()
                GrdAddScaglioni.Visible = True
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Effettuare la ricerca prima di eseguire la modifica degli scaglioni!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.btnModificaScaglioni_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnSalvaScaglioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaScaglioni.Click
        'Salvataggio di tutti i valori modificati
        Try
            Session("UsaScaglioniMod") = True
            GrdAddScaglioni.Visible = True

            Ricerca()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.btnSalvaScaglioni_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnInserisceScaglione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInserisceScaglione.Click
        'Modifica del valore selezionato
        'Dim sSQL As String
        Dim sScript As String
        Dim dvMyDati As New DataView

        Try
            Session("newScaglione") = True

            txtDA.Text = ""
            txtA.Text = ""

            Dim FncCanoni As New ClsCanoni
            FncCanoni.LoadComboUtenze(ddlDescrizioneTU, Left(ddlPeriodo.SelectedItem.Text, 4))
            'sSQL = "SELECT * FROM  DBO.TP_TIPIUTENZA"
            'sSQL += " WHERE DESCRIZIONE<>''"
            'sSQL += " AND COD_ENTE=" & ConstSession.IdEnte
            'sSQL += " ORDER BY DESCRIZIONE"
            'dvMyDati = iDB.GetDataView(sSQL)
            'ddlDescrizioneTU.Items.Clear()
            'If Not dvMyDati Is Nothing Then
            '    For Each myRow As DataRowView In dvMyDati
            '        If Not IsDBNull(myRow(0)) Then
            '            ddlDescrizioneTU.Items.Add(myRow(1))
            '            ddlDescrizioneTU.Items(ddlDescrizioneTU.Items.Count - 1).Value = myRow(0)
            '        End If
            '    Next
            'End If

            sScript = "VievMod('')"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.btnInserisceScaglione_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnAnnullaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnullaModifica.Click
        divModScaglioni.Attributes.Add("style", "display='none'")
        GrdAddScaglioni.Visible = False
        Ricerca()
    End Sub
    Private Sub btnSalvaModS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaModS.Click
        'Salva la modifica effettuata nelle griglia
        Dim idRiga, idDescUt As Integer
        Dim sDa, sA, sDescUT, sScript As String
        Dim oMyTariffe As OggettoScaglione()
        Dim oMyTariffeTemp As OggettoScaglione
        Dim x, cont As Integer
        Dim arrayListoDatiRata As New ArrayList
        Try
            If Not Session("newScaglione") Is Nothing Then
                'Nuovo inserimento

                idDescUt = ddlDescrizioneTU.SelectedValue
                sDescUT = ddlDescrizioneTU.SelectedItem.Text
                sDa = txtDA.Text
                sA = txtA.Text

                oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())
                For x = 0 To oMyTariffe.Length - 1
                    ID = oMyTariffe(x).idTipoUtenza
                    If ID = idDescUt Then
                        cont += 1
                    End If
                Next
                If cont < 5 Then
                    oMyTariffeTemp = New OggettoScaglione
                    oMyTariffeTemp.DA = sDa
                    oMyTariffeTemp.A = sA
                    oMyTariffeTemp.idTipoUtenza = idDescUt
                    oMyTariffeTemp.sDescrizioneTU = sDescUT
                    oMyTariffeTemp.sIdEnte = ConstSession.IdEnte

                    'Inserisco il nuovo valore all'interno dell'array
                    For x = 0 To oMyTariffe.Length - 1
                        arrayListoDatiRata.Add(oMyTariffe(x))
                    Next
                    arrayListoDatiRata.Add(oMyTariffeTemp)
                    'ordino l'array
                    arrayListoDatiRata.Sort(New ScaglioniComparer)

                    oMyTariffe = CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglione)), OggettoScaglione())

                    Session("Scaglioni") = oMyTariffe

                    GrdAddScaglioni.DataSource = oMyTariffe
                    GrdAddScaglioni.DataBind()
                    GrdAddScaglioni.Visible = True
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Sono già presenti 5 scaglioni di tipo " & sDescUT & "\nNon è possibile inserirne altri'); "
                    RegisterScript(sScript, Me.GetType())
                End If

            Else
                'modifica scaglione esistente
                idRiga = CInt(txtRiga.Text)
                sDa = txtDA.Text
                sA = txtA.Text

                idDescUt = ddlDescrizioneTU.SelectedValue
                sDescUT = ddlDescrizioneTU.SelectedItem.Text
                cont = 0
                oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())

                oMyTariffeTemp = New OggettoScaglione
                oMyTariffeTemp.A = oMyTariffe(idRiga).A
                oMyTariffeTemp.DA = oMyTariffe(idRiga).DA
                oMyTariffeTemp.idTipoUtenza = oMyTariffe(idRiga).idTipoUtenza
                oMyTariffeTemp.sDescrizioneTU = oMyTariffe(idRiga).sDescrizioneTU

                oMyTariffe(idRiga).A = sA
                oMyTariffe(idRiga).DA = sDa
                oMyTariffe(idRiga).idTipoUtenza = idDescUt
                oMyTariffe(idRiga).sDescrizioneTU = sDescUT

                For x = 0 To oMyTariffe.Length - 1
                    ID = oMyTariffe(x).idTipoUtenza
                    If ID = idDescUt Then
                        cont += 1
                    End If

                Next
                If cont <= 5 Then

                    For x = 0 To oMyTariffe.Length - 1
                        arrayListoDatiRata.Add(oMyTariffe(x))
                    Next
                    'ordino l'array
                    arrayListoDatiRata.Sort(New ScaglioniComparer)
                    oMyTariffe = CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglione)), OggettoScaglione())

                    Session("Scaglioni") = oMyTariffe

                    GrdAddScaglioni.DataSource = oMyTariffe
                    GrdAddScaglioni.DataBind()
                    GrdAddScaglioni.Visible = True

                    sScript = "VievMod('none')"
                    RegisterScript(sScript, Me.GetType())
                Else

                    oMyTariffe(idRiga).A = oMyTariffeTemp.A
                    oMyTariffe(idRiga).DA = oMyTariffeTemp.DA
                    oMyTariffe(idRiga).idTipoUtenza = oMyTariffeTemp.idTipoUtenza
                    oMyTariffe(idRiga).sDescrizioneTU = oMyTariffeTemp.sDescrizioneTU

                    Session("Scaglioni") = oMyTariffe

                    GrdAddScaglioni.DataSource = oMyTariffe
                    GrdAddScaglioni.DataBind()
                    GrdAddScaglioni.Visible = True

                    sScript = "GestAlert('a', 'warning', '', '', 'Sono già presenti 5 scaglioni di tipo " & sDescUT & "\nNon è possibile inserirne altri'); "
                    RegisterScript(sScript, Me.GetType())
                End If
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.btnSalvaModS_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Dim sScript, sPathProspetti, sNameXLS As String
        Dim oFinale As OggettoScaglioneFinale()
        Dim aMyHeaders As String()
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x As Integer
        Dim numCol As Integer = 4
        'Se ho la sessione di risultato ricerca valorizzata:
        If Not Session("AnalisiScaglioni") Is Nothing Then
            oFinale = CType(Session("AnalisiScaglioni"), OggettoScaglioneFinale())

            'valorizzo il nome e percorso del file di stampa;
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_ANALISISCAGLIONI_" & ddlPeriodo.SelectedItem.Text & "_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'popolo il datatable di stampa 
            DtDatiStampa = PrintAnalisiScaglioni(ddlPeriodo.SelectedItem.Text, oFinale, numCol)

            If Not DtDatiStampa Is Nothing Then
                'se ho il datatable popolato:
                'definisco l'arraylist di colonne;
                aListColonne = New ArrayList
                For x = 0 To numCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4}
                'esporto in excel tramite RKLib.ExportData.Export("Web")
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
                sScript = sPathProspetti & sNameXLS
            End If

        Else
            sScript = "GestAlert('a', 'warning', '', '', 'Effettuare la ricerca prima di eseguire la stampa!');"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
#End Region
    Private Sub ddlPeriodo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPeriodo.SelectedIndexChanged
        Session("UsaScaglioniMod") = False
        Session.Remove("Scaglioni")
        GrdScaglioni.Visible = False
        GrdAddScaglioni.Visible = False
        DivAttesa.Style.Add("display", "none")
        GrdAddScaglioni.Visible = False
        divModScaglioni.Attributes.Add("style", "display='none'")
    End Sub

#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim oMyTariffe As OggettoScaglione()
            Dim dvMyDati As New DataView
            Dim bSelect As Boolean = False
            Dim arrayListoDatiRata As New ArrayList
            Dim x As Integer
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowEdit" Then
                For Each myGrRow As GridViewRow In GrdAddScaglioni.Rows
                    If IDRow = CType(myGrRow.FindControl("hfid"), HiddenField).Value Then
                        Session.Remove("newScaglione")
                        oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())

                        txtDA.Text = CType(myGrRow.FindControl("lblDa"), TextBox).Text
                        txtA.Text = CType(myGrRow.FindControl("lblA"), TextBox).Text
                        txtRiga.Text = myGrRow.RowIndex
                        ID = myGrRow.Cells(1).Text

                        Dim sSQL As String = "SELECT * FROM  DBO.TP_TIPIUTENZA"
                        sSQL += " WHERE DESCRIZIONE<>''"
                        sSQL += " AND COD_ENTE=" & ConstSession.IdEnte
                        sSQL += " ORDER BY DESCRIZIONE"
                        dvMyDati = iDB.GetDataView(sSQL)
                        ddlDescrizioneTU.Items.Clear()
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                If Not IsDBNull(myRow(0)) Then
                                    ddlDescrizioneTU.Items.Add(myRow(1))
                                    ddlDescrizioneTU.Items(ddlDescrizioneTU.Items.Count - 1).Value = myRow(0)
                                    If ID = myRow(0) And Not bSelect Then
                                        ddlDescrizioneTU.Items(ddlDescrizioneTU.Items.Count - 1).Selected = True
                                        bSelect = True
                                    End If
                                End If
                            Next
                        End If
                        Dim sScript As String = "VievMod('')"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "Grd" Then
                    For Each myGrRow As GridViewRow In GrdAddScaglioni.Rows
                        If IDRow = CType(myGrRow.FindControl("hfid"), HiddenField).Value Then
                            oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())
                            For x = 0 To oMyTariffe.Length - 1
                                If x <> IDRow Then
                                    arrayListoDatiRata.Add(oMyTariffe(x))
                                End If
                            Next
                            oMyTariffe = CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglione)), OggettoScaglione())
                            Session("Scaglioni") = oMyTariffe
                            GrdAddScaglioni.DataSource = oMyTariffe
                            GrdAddScaglioni.DataBind()
                            GrdAddScaglioni.Visible = True
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdAddScaglioni_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAddScaglioni.DeleteCommand
    '    Dim oMyTariffe As OggettoScaglione()
    '    Dim x, id As Integer
    '    Dim arrayListoDatiRata As New ArrayList
    '    Try

    '        oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())

    '        id = e.Item.ItemIndex

    '        For x = 0 To oMyTariffe.Length - 1
    '            If x <> id Then
    '                arrayListoDatiRata.Add(oMyTariffe(x))
    '            End If
    '        Next

    '        oMyTariffe = CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglione)), OggettoScaglione())
    '        Session("Scaglioni") = oMyTariffe

    '        GrdAddScaglioni.DataSource = oMyTariffe
    '        GrdAddScaglioni.DataBind()
    '        GrdAddScaglioni.Visible = True

    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GrdAddScaglioni_DeleteCommand.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx") 
    '    End Try
    'End Sub

    'Private Sub GrdAddScaglioni_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAddScaglioni.EditCommand
    '    'Modifica del valore selezionato
    '    Dim oMyTariffe As OggettoScaglione()
    '    Dim x, id As Integer
    '    Dim desc, olddesc, sSQL As String
    '    Dim bSelect As Boolean = False
    '    Dim objlabel As Label
    '    Dim str As String
    '    Dim DrDati As SqlDataReader

    '    Try
    '        Session.Remove("newScaglione")
    '        oMyTariffe = CType(Session("Scaglioni"), OggettoScaglione())

    '        objlabel = e.Item.Cells(3).FindControl("lblDa")
    '        txtDA.Text = objlabel.Text
    '        objlabel = e.Item.Cells(4).FindControl("lblA")
    '        txtA.Text = objlabel.Text
    '        txtRiga.Text = e.Item.ItemIndex
    '        id = e.Item.Cells(1).Text

    '        sSQL = "SELECT * FROM  DBO.TP_TIPIUTENZA"
    '        sSQL += " WHERE DESCRIZIONE<>''"
    '        sSQL += " AND COD_ENTE=" & ConstSession.IdEnte
    '        sSQL += " ORDER BY DESCRIZIONE"
    '        DrDati = iDB.GetDataReader(sSQL)
    '        ddlDescrizioneTU.Items.Clear()
    '        If Not DrDati Is Nothing Then
    '            Do While DrDati.Read
    '                If Not IsDBNull(DrDati(0)) Then
    '                    ddlDescrizioneTU.Items.Add(DrDati(1))
    '                    ddlDescrizioneTU.Items(ddlDescrizioneTU.Items.Count - 1).Value = DrDati(0)
    '                    If id = DrDati(0) And Not bSelect Then
    '                        ddlDescrizioneTU.Items(ddlDescrizioneTU.Items.Count - 1).Selected = True
    '                        bSelect = True
    '                    End If
    '                End If
    '            Loop
    '        End If
    '        str = "VievMod('')"
    '        RegisterScript(sScript , Me.GetType())"stampa", str)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GrdAddScaglioni_EditCommand.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    Private Sub LoadDDL()
        Dim sSQL As String
        Try
            sSQL = "SELECT PERIODO , CODPERIODO "
            sSQL += " FROM TP_PERIODO "
            sSQL += " WHERE COD_ENTE=" & ConstSession.IdEnte
            sSQL += " ORDER BY PERIODO DESC"
            oReplace.LoadComboGenerale(ddlPeriodo, sSQL)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.LoadDDL.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' Funzione che preleva tutte le letture registrate per il periodo; per ogni lettura deve ricalcolare il consumo rapportandolo alla utenze e ai giorni di consumo e sottrarne l'eventuale consumo del subcontatore.
    ''' Il calcolo del consumo e dei giorni viene fatto richiamando le stesse funzioni utilizzate in fase di fatturazione; internamente viene rapportato il consumo alle utenze perché in fase di fatturazione vengono rapportati gli scaglioni non fattibile quì.
    ''' Il consumo così ottenuto deve essere "spalmato" sui vari scaglioni presenti per la tipologia di utenza.
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory><revision date="23/02/2021">Rifacimento procedura perché prelevava letture errate e non rapportava a giorni e subconsumo</revision></revisionHistory>
    Private Sub Ricerca()
        Dim sScript As String
        Dim Anno, codperiodo As String
        Dim oListContatori As objContatore()
        Dim oMyTariffe As OggettoScaglione()
        Dim oMyFattura As ObjFattura
        Dim FunctionTariffe As New ClsTariffe
        Dim oListAliquote() As oDettaglioAliquote
        Dim oFinale As OggettoScaglioneFinale()
        Dim oRuoloH2O As ObjTotRuoloFatture

        Try
            oMyTariffe = Nothing : oListAliquote = Nothing
            If ddlPeriodo.SelectedValue = "" Then
                sScript = "alert ('Selezionare il periodo di riferimento.');"
                sScript += "document.getElementById ('ddlPeriodo').focus();"
                RegisterScript(sScript, Me.GetType())
            Else
                DivAttesa.Style.Add("display", "")
                GrdAddScaglioni.Visible = False
                'AddScaglioni.Attributes.Add("style", "display='none'")
                divModScaglioni.Attributes.Add("style", "display='none'")

                Session.Remove("AnalisiScaglioni")

                Session("PeriodoSelezionato") = ddlPeriodo.SelectedItem.Text
                Anno = Left(ddlPeriodo.SelectedItem.Text, 4)
                codperiodo = ddlPeriodo.SelectedItem.Value.ToString

                'Verifico se utilizzare gli scaglioni modificati o quelli presi da ricerca
                If Session("UsaScaglioniMod") <> True Then
                    oRuoloH2O = New ObjTotRuoloFatture
                    Try
                        oMyTariffe = FunctionTariffe.GetTariffe(ConstSession.IdEnte, Anno, oRuoloH2O).oScaglioni
                        Session("Scaglioni") = oMyTariffe
                    Catch ex As Exception
                        lblError.Text = oRuoloH2O.sNote
                        lblError.Visible = True
                        GrdScaglioni.Visible = False
                        Exit Sub
                    End Try
                Else
                    oMyTariffe = Session("Scaglioni")
                End If

                If Not oMyTariffe Is Nothing Then
                    oFinale = CaricaOggettoFinale(oMyTariffe)
                    If codperiodo < 1 Then
                        codperiodo = ConstSession.IdPeriodo
                    End If
                    'prelevo le letture per il periodo selezionato
                    oListContatori = GetLettureScaglioni(ConstSession.IdEnte, codperiodo, ConstSession.CodTributo, 0, Nothing)
                    If Not oListContatori Is Nothing Then
                        For Each myCont As objContatore In oListContatori
                            oMyFattura = New ObjFattura
                            If CalcoloScaglioni(myCont, oMyTariffe, ConstSession.UserName, oMyFattura, oListAliquote) <= 0 Then
                                Throw New Exception("Errore in calcolo scaglioni")
                            Else
                                If Not oMyFattura.oScaglioni Is Nothing Then
                                    For Each myScaglione As ObjTariffeScaglione In oMyFattura.oScaglioni
                                        'Ciclo sugli scaglioni e aumento la relativa quantità nell'oggetto finale
                                        ValorizzaOggettoFinale(myScaglione.nQuantita, oMyFattura.nUtenze, myScaglione.nIdScaglione, myScaglione.nDa, myScaglione.nA, oFinale)
                                    Next
                                End If
                            End If
                        Next
                        lblError.Visible = False
                        Session("AnalisiScaglioni") = oFinale
                        GrdScaglioni.DataSource = oFinale
                        GrdScaglioni.DataBind()
                        GrdScaglioni.Visible = True
                    Else
                        lblError.Text = "Nessun Scaglione trovato"
                        lblError.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.Ricerca.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
            divModScaglioni.Style.Add("display", "none")
        End Try
    End Sub
    'Private Sub Ricerca()
    '    Dim i, j As Integer
    '    Dim sScript As String
    '    Dim Anno, codperiodo As String
    '    Dim oListContatori As objContatore()
    '    Dim oMyTariffe As OggettoScaglione()
    '    Dim oMyFattura As ObjFattura
    '    Dim FunctionTariffe As New ClsTariffe
    '    Dim oListAliquote() As oDettaglioAliquote
    '    Dim oFinale As OggettoScaglioneFinale()
    '    Dim oRuoloH2O As ObjTotRuoloFatture

    '    Try
    '        oMyTariffe = Nothing : oListAliquote = Nothing
    '        If ddlPeriodo.SelectedValue = "" Then
    '            sScript = "alert ('Selezionare il periodo di riferimento.');"
    '            sScript += "document.getElementById ('ddlPeriodo').focus();"
    '            RegisterScript(sScript, Me.GetType())
    '        Else
    '             DivAttesa.Style.Add("display", "")
    '            GrdAddScaglioni.Visible = False
    '            'AddScaglioni.Attributes.Add("style", "display='none'")
    '            divModScaglioni.Attributes.Add("style", "display='none'")

    '            Session.Remove("AnalisiScaglioni")

    '            Session("PeriodoSelezionato") = ddlPeriodo.SelectedItem.Text
    '            Anno = Left(ddlPeriodo.SelectedItem.Text, 4)
    '            codperiodo = ddlPeriodo.SelectedItem.Value.ToString

    '            'Verifico se utilizzare gli scaglioni modificati o quelli presi da ricerca
    '            If Session("UsaScaglioniMod") <> True Then
    '                oRuoloH2O = New ObjTotRuoloFatture
    '                Try
    '                    oMyTariffe = FunctionTariffe.GetTariffe(ConstSession.IdEnte, Anno, oRuoloH2O).oScaglioni
    '                    Session("Scaglioni") = oMyTariffe
    '                Catch ex As Exception
    '                    lblError.Text = oRuoloH2O.sNote
    '                    lblError.Visible = True
    '                    GrdScaglioni.Visible = False
    '                End Try
    '            Else
    '                oMyTariffe = Session("Scaglioni")
    '            End If

    '            If Not oMyTariffe Is Nothing Then
    '                oFinale = CaricaOggettoFinale(oMyTariffe)
    '                If codperiodo < 1 Then
    '                    codperiodo = ConstSession.IdPeriodo
    '                End If
    '                'prelevo le letture per il periodo selezionato
    '                oListContatori = GetLettureScaglioni(ConstSession.IdEnte, codperiodo, ConstSession.CodTributo, 0, Nothing)
    '                '*** 20121213 - per nuova gestione analisi scaglioni ***
    '                'prelevo le letture precedenti di contatori attivi che non hanno lettura per il periodo selezionato
    '                Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo
    '                Dim FncPeriodo As New TabelleDiDecodifica.DBPeriodo
    '                oPeriodo = FncPeriodo.GetPeriodo(codperiodo)
    '                oListContatori = GetLettureScaglioniPrec(oListContatori, ConstSession.IdEnte, oPeriodo, ConstSession.CodTributo, 0)
    '                oListContatori = GetLettureScaglioniContOrg(oListContatori, ConstSession.IdEnte, oPeriodo, ConstSession.CodTributo, 0)
    '                '*** ***
    '                If Not oListContatori Is Nothing Then
    '                    Dim nUtenzeNoConsumo, nUtenze, nTotUtenze, x, nUtenzeFatt As Integer
    '                    Dim nTotConsumo, nConsumo, nConsumoFatt, nConsumoNeg As Double
    '                    For i = 0 To oListContatori.Length - 1
    '                        oMyFattura = New ObjFattura
    '                        CalcoloScaglioni(oListContatori(i), oMyTariffe, 0, ConstSession.UserName, oMyFattura, oListAliquote)
    '                        '***
    '                        nTotUtenze += oMyFattura.nUtenze
    '                        If oListContatori(i).bEsenteAcqua = False Then
    '                            For x = 0 To oListContatori(i).oListLetture.GetUpperBound(0)
    '                                nTotConsumo += oListContatori(i).oListLetture(x).nConsumo
    '                                If oListContatori(i).oListLetture(x).nConsumo > 0 Then
    '                                    nConsumoFatt += oListContatori(i).oListLetture(x).nConsumo
    '                                    nUtenzeFatt += oMyFattura.nUtenze
    '                                Else
    '                                    nConsumoNeg += oListContatori(i).oListLetture(x).nConsumo
    '                                End If
    '                            Next
    '                        End If
    '                        '***
    '                        If Not oMyFattura.oScaglioni Is Nothing Then
    '                            For j = 0 To oMyFattura.oScaglioni.Length - 1
    '                                'Ciclo sugli scaglioni e aumento la relativa quantità nell'oggetto finale
    '                                ValorizzaOggettoFinale(oMyFattura.oScaglioni(j).nQuantita, oMyFattura.nUtenze, oMyFattura.oScaglioni(j).nIdScaglione, oMyFattura.oScaglioni(j).nDa, oMyFattura.oScaglioni(j).nA, oFinale)
    '                                '***
    '                                nConsumo += oMyFattura.oScaglioni(j).nQuantita
    '                                nUtenze += oMyFattura.nUtenze
    '                                '***
    '                            Next
    '                        Else
    '                            nUtenzeNoConsumo += oMyFattura.nUtenze
    '                        End If
    '                    Next
    '                    lblError.Visible = False
    '                    Session("AnalisiScaglioni") = oFinale
    '                    GrdScaglioni.DataSource = oFinale
    '                    GrdScaglioni.DataBind()
    '                    GrdScaglioni.Visible = True
    '                Else
    '                    lblError.Text = "Nessun Scaglione trovato"
    '                    lblError.Visible = True
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.Ricerca.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '         DivAttesa.Style.Add("display", "none")
    '    End Try
    'End Sub

    'Valorizzazione oggetto finale
    Private Sub ValorizzaOggettoFinale(ByVal quantita As Integer, ByVal nUtenze As Integer, ByVal id As Long, ByVal da As Integer, ByVal a As Integer, ByRef oFinale As OggettoScaglioneFinale())
        Dim i As Integer
        Try
            For i = 0 To oFinale.Length - 1
                If oFinale(i).ID = id And oFinale(i).DA = da And oFinale(i).A = a Then
                    oFinale(i).dQuantita += quantita
                    '*** 20121213 - per nuova gestione analisi scaglioni ***
                    oFinale(i).nUtenze += nUtenze
                    '*** ***
                    Exit For
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.ValorizzaOggettoFinale.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Function CaricaOggettoFinale(ByVal oMyTariffe As OggettoScaglione()) As OggettoScaglioneFinale()
        Dim i As Integer
        Dim arrayListoDatiRata As New ArrayList
        Dim oFinale As OggettoScaglioneFinale
        Try
            For i = 0 To oMyTariffe.Length - 1
                oFinale = New OggettoScaglioneFinale
                oFinale.A = oMyTariffe(i).A
                oFinale.DA = oMyTariffe(i).DA
                oFinale.dQuantita = 0
                '*** 20121213 - per nuova gestione analisi scaglioni ***
                oFinale.nUtenze = 0
                '*** ***
                oFinale.ID = oMyTariffe(i).ID
                oFinale.idTipoUtenza = oMyTariffe(i).idTipoUtenza
                oFinale.sAnno = oMyTariffe(i).sAnno
                oFinale.sDescrizioneTU = oMyTariffe(i).sDescrizioneTU
                oFinale.sIdEnte = oMyTariffe(i).sIdEnte
                arrayListoDatiRata.Add(oFinale)
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.CaricaOggettoFinale.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglioneFinale)), OggettoScaglioneFinale())
    End Function
    Public Function GetLettureScaglioni(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal nTipoArrotondConsumo As Integer, ByRef oRuoloH2O As ObjTotRuoloFatture, Optional ByVal nContatore As Integer = -1) As objContatore()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim nListCont As Integer = -1
        Dim nListLett As Integer = -1
        Dim nContatorePrec As Integer = -1
        Dim oListContatori() As objContatore
        Dim oMyContatore As New objContatore
        Dim oMyLettura As ObjLettura

        oListContatori = Nothing
        Try
            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_AnalisiScaglioni_Letture", "IDENTE", "IDPERIODO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                                               , ctx.GetParam("IDPERIODO", nPeriodo))
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetLettureScaglion.errorequery: ", ex)
                Return Nothing
            End Try
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    'incremento l'indice
                    nListLett += 1
                    oMyLettura = New ObjLettura

                    oMyLettura.IdLettura = StringOperation.FormatInt(myRow("codlettura"))
                    oMyContatore.sIdEnte = StringOperation.FormatString(myRow("codente"))
                    oMyLettura.nIdPeriodo = StringOperation.FormatInt(myRow("codperiodo"))
                    oMyLettura.nIdContatore = StringOperation.FormatInt(myRow("codcontatore"))
                    oMyContatore.nTipoUtenza = StringOperation.FormatInt(myRow("idtipoutenza"))
                    oMyContatore.nNumeroUtenze = StringOperation.FormatInt(myRow("numeroutenze"))
                    oMyContatore.sMatricola = StringOperation.FormatString(myRow("matricola"))
                    oMyContatore.bEsenteAcqua = StringOperation.FormatBool(myRow("esenteacqua"))
                    oMyContatore.nFondoScala = StringOperation.FormatInt(myRow("valorefondoscala"))
                    oMyLettura.tDataLetturaAtt = New ClsGenerale.Generale().GiraDataFromDB(StringOperation.FormatString(myRow("datalettura")))
                    oMyLettura.tDataLetturaPrec = New ClsGenerale.Generale().GiraDataFromDB(StringOperation.FormatString(myRow("dataletturaprecedente")))
                    oMyLettura.nLetturaAtt = StringOperation.FormatInt(myRow("lettura"))
                    oMyLettura.nLetturaPrec = StringOperation.FormatInt(myRow("letturaprecedente"))
                    oMyLettura.nConsumoSubContatore = StringOperation.FormatInt(myRow("subconsumo"))
                    oMyLettura.nTipoArrotondConsumo = nTipoArrotondConsumo
                    ReDim Preserve oMyContatore.oListLetture(nListLett)
                    oMyContatore.oListLetture(nListLett) = oMyLettura
                    If StringOperation.FormatInt(myRow("codcontatore")) <> nContatorePrec Then
                        'dimensiono l'array
                        nListCont += 1
                        ReDim Preserve oListContatori(nListCont)
                        'memorizzo i dati nell'array
                        oListContatori(nListCont) = oMyContatore
                        oMyContatore = New objContatore
                        nListLett = -1
                    End If
                    nContatorePrec = StringOperation.FormatInt(myRow("codcontatore"))
                Next
            End If
            'dimensiono l'array
            If nListLett <> -1 Then
                nListCont += 1
                ReDim Preserve oListContatori(nListCont)
                'memorizzo i dati nell'array
                oListContatori(nListCont) = oMyContatore
            End If
            Return oListContatori
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetLettureScaglioni.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetLettureScaglioni(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal nTipoArrotondConsumo As Integer, ByRef oRuoloH2O As ObjTotRuoloFatture, Optional ByVal nContatore As Integer = -1) As objContatore()
    '    'dim sSQL as string
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim nListCont As Integer = -1
    '    Dim nListLett As Integer = -1
    '    Dim nContatorePrec As Integer = -1
    '    Dim oListContatori() As objContatore
    '    Dim oMyContatore As New objContatore
    '    Dim oMyLettura As ObjLettura
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    oListContatori = Nothing : DrDati = Nothing
    '    Try
    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_AnalisiScaglioni_Letture"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nPeriodo
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        'eseguo la query
    '        'DrDati = iDB.GetDataReader(sSQL)
    '        DrDati = iDB.GetDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            'incremento l'indice
    '            nListLett += 1
    '            oMyLettura = New ObjLettura

    '            oMyLettura.IdLettura = CInt(DrDati("codlettura"))
    '            oMyContatore.sIdEnte = CStr(DrDati("codente"))
    '            oMyLettura.nIdPeriodo = CInt(DrDati("codperiodo"))
    '            oMyLettura.nIdContatore = CInt(DrDati("codcontatore"))
    '            If Not IsDBNull(DrDati("idtipoutenza")) Then
    '                oMyContatore.nTipoUtenza = CInt(DrDati("idtipoutenza"))
    '            End If
    '            If Not IsDBNull(DrDati("numeroutenze")) Then
    '                oMyContatore.nNumeroUtenze = CInt(DrDati("numeroutenze"))
    '            End If
    '            If Not IsDBNull(DrDati("matricola")) Then
    '                oMyContatore.sMatricola = CStr(DrDati("matricola"))
    '            End If
    '            If Not IsDBNull(DrDati("lettura")) Then
    '                oMyLettura.nLetturaAtt = CInt(DrDati("lettura"))
    '            End If
    '            If Not IsDBNull(DrDati("consumo")) Then
    '                If CInt(DrDati("consumo")) <> 0 Then
    '                    oMyLettura.nConsumo = CInt(DrDati("consumo"))
    '                End If
    '            End If
    '            If Not IsDBNull(DrDati("giornidiconsumo")) Then
    '                If CInt(DrDati("giornidiconsumo")) <> 0 Then
    '                    oMyLettura.nGiorni = CInt(DrDati("giornidiconsumo"))
    '                End If
    '            End If
    '            If Not IsDBNull(DrDati("esenteacqua")) Then
    '                oMyContatore.bEsenteAcqua = CBool(DrDati("esenteacqua"))
    '            End If
    '            oMyLettura.nTipoArrotondConsumo = nTipoArrotondConsumo
    '            ReDim Preserve oMyContatore.oListLetture(nListLett)
    '            oMyContatore.oListLetture(nListLett) = oMyLettura
    '            If CInt(DrDati("codcontatore")) <> nContatorePrec Then
    '                'dimensiono l'array
    '                nListCont += 1
    '                ReDim Preserve oListContatori(nListCont)
    '                'memorizzo i dati nell'array
    '                oListContatori(nListCont) = oMyContatore
    '                oMyContatore = New objContatore
    '                nListLett = -1
    '            End If
    '            nContatorePrec = CInt(DrDati("codcontatore"))
    '        Loop
    '        'dimensiono l'array
    '        If nListLett <> -1 Then
    '            nListCont += 1
    '            ReDim Preserve oListContatori(nListCont)
    '            'memorizzo i dati nell'array
    '            oListContatori(nListCont) = oMyContatore
    '        End If
    '        Return oListContatori
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetLettureScaglioni.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return Nothing
    '    Finally
    '        If Not DrDati Is Nothing Then
    '            DrDati.Close()
    '        End If
    '    End Try
    'End Function

    '*** 20121213 - per nuova gestione analisi scaglioni ***
    'Public Function GetLettureScaglioniPrec(ByVal oListContatori() As objContatore, ByVal sEnte As String, ByVal oPeriodo As TabelleDiDecodifica.DetailPeriodo, ByVal sTributo As String, ByVal nTipoArrotondConsumo As Integer) As objContatore()
    '    Dim sSQL As String
    '    Dim dvMyDati As New DataView
    '    Dim nListLett As Integer = -1
    '    Dim nListCont As Integer = -1
    '    Dim nContatorePrec As Integer = -1
    '    Dim oListContatoriPrec() As objContatore
    '    Dim oMyContatore As New objContatore
    '    Dim oMyLettura As ObjLettura

    '    oListContatoriPrec = Nothing : dvMyDati = Nothing
    '    Try
    '        If Not IsNothing(oListContatori) Then
    '            nListCont = oListContatori.GetUpperBound(0)
    '            oListContatoriPrec = oListContatori
    '        End If

    '        sSQL = "SELECT *"
    '        sSQL += " FROM ("
    '        sSQL += " 	SELECT CODLETTURA, CODENTE, TP_LETTURE.CODPERIODO, TP_CONTATORI.CODCONTATORE"
    '        sSQL += " 	, TP_CONTATORI.CODCONTATOREPRECEDENTE, DATAATTIVAZIONE, DATACESSAZIONE"
    '        sSQL += " 	, TR_CONTATORI_INTESTATARIO.COD_CONTRIBUENTE AS IDINTESTATARIO, CODUTENTE ,IDTIPOCONTATORE, MYIDTIPOUTENZA AS IDTIPOUTENZA, TP_CONTATORI.NUMEROUTENTE, NUMEROUTENZE "
    '        sSQL += " 	,MATRICOLA"
    '        sSQL += " 	,VIA_UBICAZIONE, CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE "
    '        sSQL += " 	,DATALETTURAPRECEDENTE, LETTURAPRECEDENTE, DATALETTURA, LETTURA, CONSUMO, GIORNIDICONSUMO "
    '        sSQL += " 	,ESENTEACQUA, CODDEPURAZIONE, ESENTEDEPURAZIONE, CODFOGNATURA, ESENTEFOGNATURA "
    '        sSQL += " 	FROM TP_LETTURE  "
    '        sSQL += " 	INNER JOIN TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE "
    '        sSQL += " 	INNER JOIN TR_CONTATORI_INTESTATARIO ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_INTESTATARIO.CODCONTATORE "
    '        sSQL += " 	LEFT JOIN ("
    '        sSQL += "  		SELECT L.CODCONTATORE"
    '        sSQL += "   		FROM TP_LETTURE L 	"
    '        sSQL += " 		INNER JOIN TP_CONTATORI C ON L.CODCONTATORE=C.CODCONTATORE AND L.CODPERIODO= " & oPeriodo.CodPeriodo
    '        sSQL += " 		WHERE (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        sSQL += " 	) F ON TP_LETTURE.CODCONTATORE=F.CODCONTATORE "
    '        sSQL += " 	INNER JOIN ("
    '        sSQL += "  		SELECT CODCONTATORE, MAX(DATALETTURA) AS LASTLETTURA"
    '        sSQL += "  		FROM TP_LETTURE"
    '        sSQL += "  		GROUP BY CODCONTATORE "
    '        sSQL += " 	) LL ON TP_LETTURE.CODCONTATORE=LL.CODCONTATORE AND TP_LETTURE.DATALETTURA=LL.LASTLETTURA	 "
    '        sSQL += " 	INNER JOIN TP_PERIODO P ON TP_LETTURE.CODPERIODO=P.CODPERIODO "
    '        sSQL += " 	INNER JOIN ("
    '        sSQL += " 		SELECT MATRICOLA AS MYMATRICOLA , IDTIPOUTENZA AS MYIDTIPOUTENZA"
    '        sSQL += " 		FROM TP_CONTATORI"
    '        sSQL += " 		WHERE 1=1"
    '        sSQL += " 		AND ((DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE <> '' AND DATAATTIVAZIONE <='" & oReplace.GiraData(oPeriodo.AData) & "') "
    '        sSQL += " 		AND ((DATACESSAZIONE IS NULL OR DATACESSAZIONE ='') OR (DATACESSAZIONE >='" & oReplace.GiraData(oPeriodo.DaData) & "')))"
    '        sSQL += " 	) X ON TP_CONTATORI.MATRICOLA=X.MYMATRICOLA"
    '        sSQL += " 	WHERE 1=1"
    '        sSQL += " 	AND F.CODCONTATORE IS NULL "
    '        'sSQL += " 	AND TP_LETTURE.DATA_VARIAZIONE IS NULL "
    '        sSQL += " 	AND (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        sSQL += " ) X"
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND (CODENTE='" & sEnte & "') "
    '        sSQL += " AND ((DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE <> '' AND DATAATTIVAZIONE <='" & oReplace.GiraData(oPeriodo.AData) & "') "
    '        sSQL += " AND ((DATACESSAZIONE IS NULL OR DATACESSAZIONE ='') OR (DATACESSAZIONE >='" & oReplace.GiraData(oPeriodo.DaData) & "')))"
    '        sSQL += " ORDER BY CODUTENTE, DATALETTURA"
    '        'eseguo la query
    '        dvMyDati = iDB.GetDataView(sSQL)
    '        If Not dvMyDati Is Nothing Then
    '            For Each myRow As DataRowView In dvMyDati
    '                'incremento l'indice
    '                nListLett += 1
    '                oMyLettura = New ObjLettura

    '                oMyLettura.IdLettura = StringOperation.FormatInt(myRow("codlettura"))
    '                oMyContatore.sIdEnte = StringOperation.FormatString(myRow("codente"))
    '                oMyLettura.nIdPeriodo = StringOperation.FormatInt(myRow("codperiodo"))
    '                oMyLettura.nIdContatore = StringOperation.FormatInt(myRow("codcontatore"))
    '                oMyContatore.nTipoUtenza = StringOperation.FormatInt(myRow("idtipoutenza"))
    '                oMyContatore.nNumeroUtenze = StringOperation.FormatInt(myRow("numeroutenze"))
    '                oMyContatore.sMatricola = StringOperation.FormatString(myRow("matricola"))
    '                oMyLettura.nLetturaAtt = StringOperation.FormatInt(myRow("lettura"))
    '                If StringOperation.FormatInt(myRow("consumo")) <> 0 Then
    '                    oMyLettura.nConsumo = StringOperation.FormatInt(myRow("consumo"))
    '                End If
    '                If StringOperation.FormatInt(myRow("giornidiconsumo")) <> 0 Then
    '                    oMyLettura.nGiorni = StringOperation.FormatInt(myRow("giornidiconsumo"))
    '                End If
    '                oMyContatore.bEsenteAcqua = StringOperation.FormatBool(myRow("esenteacqua"))
    '                oMyLettura.nTipoArrotondConsumo = nTipoArrotondConsumo
    '                ReDim Preserve oMyContatore.oListLetture(nListLett)
    '                oMyContatore.oListLetture(nListLett) = oMyLettura
    '                If StringOperation.FormatInt(myRow("codcontatore")) <> nContatorePrec Then
    '                    'dimensiono l'array
    '                    nListCont += 1
    '                    ReDim Preserve oListContatoriPrec(nListCont)
    '                    'memorizzo i dati nell'array
    '                    oListContatoriPrec(nListCont) = oMyContatore
    '                    oMyContatore = New objContatore
    '                    nListLett = -1
    '                End If
    '                nContatorePrec = StringOperation.FormatInt(myRow("codcontatore"))
    '            Next
    '        End If
    '        If nListLett <> -1 Then
    '            'dimensiono l'array
    '            nListCont += 1
    '            ReDim Preserve oListContatoriPrec(nListCont)
    '            'memorizzo i dati nell'array
    '            oListContatoriPrec(nListCont) = oMyContatore
    '        End If
    '        Return oListContatoriPrec
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetLettureScaglioniPrec.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvMyDati.Dispose()
    '    End Try
    'End Function
    'Public Function GetLettureScaglioniContOrg(ByVal oListContatori() As objContatore, ByVal sEnte As String, ByVal oPeriodo As TabelleDiDecodifica.DetailPeriodo, ByVal sTributo As String, ByVal nTipoArrotondConsumo As Integer) As objContatore()
    '    Dim sSQL As String
    '    Dim dvMyDati As New DataView
    '    Dim nListLett As Integer = -1
    '    Dim nListCont As Integer = -1
    '    Dim nContatorePrec As Integer = -1
    '    Dim oListContatoriPrec() As objContatore
    '    Dim oMyContatore As New objContatore
    '    Dim oMyLettura As ObjLettura

    '    oListContatoriPrec = Nothing : dvMyDati = Nothing
    '    Try
    '        If Not IsNothing(oListContatori) Then
    '            nListCont = oListContatori.GetUpperBound(0)
    '            oListContatoriPrec = oListContatori
    '        End If

    '        sSQL = "SELECT DISTINCT DATASOSTITUZIONE, CODLETTURA, CODENTE, TP_LETTURE.CODPERIODO, TP_CONTATORI.CODCONTATORE 	"
    '        sSQL += " , TP_CONTATORI.CODCONTATOREPRECEDENTE, DATAATTIVAZIONE, DATACESSAZIONE 	"
    '        sSQL += " , TR_CONTATORI_INTESTATARIO.COD_CONTRIBUENTE AS IDINTESTATARIO, CODUTENTE ,IDTIPOCONTATORE, MYIDTIPOUTENZA AS IDTIPOUTENZA, TP_CONTATORI.NUMEROUTENTE, NUMEROUTENZE  	"
    '        sSQL += " ,MATRICOLA 	,VIA_UBICAZIONE, CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE  	,DATALETTURAPRECEDENTE, LETTURAPRECEDENTE, DATALETTURA, LETTURA, CONSUMO, GIORNIDICONSUMO  	,ESENTEACQUA, CODDEPURAZIONE, ESENTEDEPURAZIONE, CODFOGNATURA, ESENTEFOGNATURA  	"
    '        sSQL += " FROM TP_LETTURE"
    '        sSQL += "    	INNER JOIN TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE"
    '        sSQL += "    	INNER JOIN TR_CONTATORI_INTESTATARIO ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_INTESTATARIO.CODCONTATORE  	"
    '        sSQL += " 	INNER JOIN ("
    '        sSQL += "  		SELECT CODCONTATORE, MAX(DATALETTURA) AS LASTLETTURA"
    '        sSQL += "  		FROM TP_LETTURE"
    '        sSQL += "  		GROUP BY CODCONTATORE "
    '        sSQL += " 	) LL ON TP_LETTURE.CODCONTATORE=LL.CODCONTATORE AND TP_LETTURE.DATALETTURA=LL.LASTLETTURA   	"
    '        sSQL += " 	INNER JOIN ("
    '        sSQL += " 		SELECT MATRICOLA AS MYMATRICOLA , IDTIPOUTENZA AS MYIDTIPOUTENZA"
    '        sSQL += " 		FROM TP_CONTATORI"
    '        sSQL += " 		WHERE 1=1"
    '        sSQL += " 		AND ((DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE <> '' AND DATAATTIVAZIONE <='" & oReplace.GiraData(oPeriodo.AData) & "') "
    '        sSQL += " 		AND ((DATACESSAZIONE IS NULL OR DATACESSAZIONE ='') OR (DATACESSAZIONE >='" & oReplace.GiraData(oPeriodo.DaData) & "')))"
    '        sSQL += " 	) X ON TP_CONTATORI.MATRICOLA=X.MYMATRICOLA"
    '        sSQL += " 	WHERE 1=1 	"
    '        'sSQL += "    	AND TP_LETTURE.DATA_VARIAZIONE IS NULL  "
    '        sSQL += "    	AND (NOT DATASOSTITUZIONE IS NULL AND DATASOSTITUZIONE<>'')"
    '        sSQL += "    	AND (TP_CONTATORI.CODENTE='" & sEnte & "')"
    '        sSQL += "    	AND (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        sSQL += " AND MATRICOLA NOT IN (SELECT MATRICOLA"
    '        sSQL += " FROM ("
    '        sSQL += " 	SELECT CODLETTURA, CODENTE, TP_LETTURE.CODPERIODO, TP_CONTATORI.CODCONTATORE"
    '        sSQL += " 	, TP_CONTATORI.CODCONTATOREPRECEDENTE, DATAATTIVAZIONE, DATACESSAZIONE"
    '        sSQL += " 	, TR_CONTATORI_INTESTATARIO.COD_CONTRIBUENTE AS IDINTESTATARIO, CODUTENTE ,IDTIPOCONTATORE, IDTIPOUTENZA, TP_CONTATORI.NUMEROUTENTE, NUMEROUTENZE "
    '        sSQL += " 	,MATRICOLA"
    '        sSQL += " 	,VIA_UBICAZIONE, CIVICO_UBICAZIONE, FRAZIONE_UBICAZIONE "
    '        sSQL += " 	,DATALETTURAPRECEDENTE, LETTURAPRECEDENTE, DATALETTURA, LETTURA, CONSUMO, GIORNIDICONSUMO "
    '        sSQL += " 	,ESENTEACQUA, CODDEPURAZIONE, ESENTEDEPURAZIONE, CODFOGNATURA, ESENTEFOGNATURA "
    '        sSQL += " 	FROM TP_LETTURE  "
    '        sSQL += " 	INNER JOIN TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE "
    '        sSQL += " 	INNER JOIN TR_CONTATORI_INTESTATARIO ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_INTESTATARIO.CODCONTATORE "
    '        sSQL += " 	LEFT JOIN ("
    '        sSQL += "  		SELECT L.CODCONTATORE"
    '        sSQL += "   		FROM TP_LETTURE L 	"
    '        sSQL += " 		INNER JOIN TP_CONTATORI C ON L.CODCONTATORE=C.CODCONTATORE AND L.CODPERIODO= " & oPeriodo.CodPeriodo
    '        sSQL += " 		WHERE (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        sSQL += " 	) F ON TP_LETTURE.CODCONTATORE=F.CODCONTATORE "
    '        sSQL += " 	INNER JOIN ("
    '        sSQL += "  		SELECT CODCONTATORE, MAX(DATALETTURA) AS LASTLETTURA"
    '        sSQL += "  		FROM TP_LETTURE"
    '        sSQL += "  		GROUP BY CODCONTATORE "
    '        sSQL += " 	) LL ON TP_LETTURE.CODCONTATORE=LL.CODCONTATORE AND TP_LETTURE.DATALETTURA=LL.LASTLETTURA	 "
    '        sSQL += " 	INNER JOIN TP_PERIODO P ON TP_LETTURE.CODPERIODO=P.CODPERIODO "
    '        sSQL += " 	WHERE 1=1"
    '        sSQL += " 	AND F.CODCONTATORE IS NULL "
    '        'sSQL += " 	AND TP_LETTURE.DATA_VARIAZIONE IS NULL "
    '        sSQL += " 	AND (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        sSQL += " ) X"
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND (CODENTE='" & sEnte & "') "
    '        sSQL += " AND ((DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE <> '' AND DATAATTIVAZIONE <='" & oReplace.GiraData(oPeriodo.AData) & "') "
    '        sSQL += " AND ((DATACESSAZIONE IS NULL OR DATACESSAZIONE ='') OR (DATACESSAZIONE >='" & oReplace.GiraData(oPeriodo.DaData) & "'))))"
    '        sSQL += " AND MATRICOLA NOT IN ("
    '        sSQL += " 	SELECT MATRICOLA"
    '        sSQL += " 	FROM TP_LETTURE INNER JOIN"
    '        sSQL += " 	TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE"
    '        sSQL += " 	INNER JOIN TR_CONTATORI_INTESTATARIO ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_INTESTATARIO.CODCONTATORE"
    '        sSQL += " 	WHERE (CODENTE='" & sEnte & "') AND (CODPERIODO= " & oPeriodo.CodPeriodo & " ) "
    '        sSQL += " 	AND (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        sSQL += " )"
    '        sSQL += " ORDER BY CODUTENTE, DATALETTURA"
    '        'eseguo la query
    '        Log.Debug("GetLettureScaglioniContOrg::SQL::" + sSQL)
    '        dvMyDati = iDB.GetDataView(sSQL)
    '        If Not dvMyDati Is Nothing Then
    '            For Each myRow As DataRowView In dvMyDati
    '                'incremento l'indice
    '                nListLett += 1
    '                oMyLettura = New ObjLettura

    '                oMyLettura.IdLettura = StringOperation.FormatInt(myRow("codlettura"))
    '                oMyContatore.sIdEnte = StringOperation.FormatString(myRow("codente"))
    '                oMyLettura.nIdPeriodo = StringOperation.FormatInt(myRow("codperiodo"))
    '                oMyLettura.nIdContatore = StringOperation.FormatInt(myRow("codcontatore"))
    '                oMyContatore.nTipoUtenza = StringOperation.FormatInt(myRow("idtipoutenza"))
    '                oMyContatore.nNumeroUtenze = StringOperation.FormatInt(myRow("numeroutenze"))
    '                oMyContatore.sMatricola = StringOperation.FormatString(myRow("matricola"))
    '                oMyLettura.nLetturaAtt = StringOperation.FormatInt(myRow("lettura"))
    '                If StringOperation.FormatInt(myRow("consumo")) <> 0 Then
    '                    oMyLettura.nConsumo = StringOperation.FormatInt(myRow("consumo"))
    '                End If
    '                If StringOperation.FormatInt(myRow("giornidiconsumo")) <> 0 Then
    '                    oMyLettura.nGiorni = StringOperation.FormatInt(myRow("giornidiconsumo"))
    '                End If
    '                oMyContatore.bEsenteAcqua = StringOperation.FormatBool(myRow("esenteacqua"))
    '                oMyLettura.nTipoArrotondConsumo = nTipoArrotondConsumo
    '                ReDim Preserve oMyContatore.oListLetture(nListLett)
    '                oMyContatore.oListLetture(nListLett) = oMyLettura
    '                If StringOperation.FormatInt(myRow("codcontatore")) <> nContatorePrec Then
    '                    'dimensiono l'array
    '                    nListCont += 1
    '                    ReDim Preserve oListContatoriPrec(nListCont)
    '                    'memorizzo i dati nell'array
    '                    oListContatoriPrec(nListCont) = oMyContatore
    '                    oMyContatore = New objContatore
    '                    nListLett = -1
    '                End If
    '                nContatorePrec = StringOperation.FormatInt(myRow("codcontatore"))
    '            Next
    '        End If
    '        If nListLett <> -1 Then
    '            'dimensiono l'array
    '            nListCont += 1
    '            ReDim Preserve oListContatoriPrec(nListCont)
    '            'memorizzo i dati nell'array
    '            oListContatoriPrec(nListCont) = oMyContatore
    '        End If
    '        Return oListContatoriPrec
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetLettureScaglioniContOrg.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvMyDati.Dispose()
    '    End Try
    'End Function
    '*** ***
#Region "Suddivisione in scaglioni"
    Protected Structure oDettaglioAliquote
        Dim nAliquota As Double
        Dim impBase As Double
    End Structure
    ''' <summary>
    ''' Funzione che somma per ogni scaglione i consumi di competenza.
    ''' Vengono calcolati al momento i giorni ed il consumo richiamando le funzioni del motore; dopo il calcolo si rapporta il consumo ai giorni per uniformarlo agli scaglioni che sono su base annua.
    ''' Il consumo viene rapportato secondo la seguente formula: (CONSUMO/GIORNI)*BASE TEMPO
    ''' La quantità esposta sullo scaglione però deve concidere con il consumo reale quindi viene secondo la seguente formula: (CONSUMO SU SCAGLIONE/365)*GIORNI
    ''' </summary>
    ''' <param name="oMyContatore"></param>
    ''' <param name="ListScaglioni"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="oMyFattura"></param>
    ''' <param name="oMyListAliquote"></param>
    ''' <returns></returns>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory><revision date="23/02/2021">Rifacimento procedura perché prelevava letture errate e non rapportava a giorni e subconsumo</revision></revisionHistory>
    Private Function CalcoloScaglioni(ByVal oMyContatore As objContatore, ByVal ListScaglioni As OggettoScaglione(), ByVal sOperatore As String, ByRef oMyFattura As ObjFattura, ByRef oMyListAliquote() As oDettaglioAliquote) As Integer
        Dim TypeOfRI As Type = GetType(IH2O)
        Dim RemoRuoloH2O As IH2O
        Dim oScaglione As ObjTariffeScaglione
        Dim oListScaglioni() As ObjTariffeScaglione
        Dim nList As Integer = -1
        Dim nConsumoDa, nConsumoTot, nConsumoApplicato, nConsumoCalcolo As Double

        oListScaglioni = Nothing
        Try
            If oMyContatore.bEsenteAcqua = False Then
                RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConstSession.UrlMotoreH2O)
                For Each myItem As ObjLettura In oMyContatore.oListLetture
                    myItem.nGiorni = RemoRuoloH2O.CalcolaGiorni(myItem.tDataLetturaPrec, myItem.tDataLetturaAtt, ConstSession.BaseTempo)
                    nConsumoTot = RemoRuoloH2O.CalcolaConsumo(myItem.nLetturaPrec, myItem.nLetturaAtt, myItem.nConsumoSubContatore, oMyContatore.nFondoScala)
                    nConsumoTot = FormatNumber((((nConsumoTot / myItem.nGiorni) * ConstSession.BaseTempo) / oMyContatore.nNumeroUtenze), 2)
                    nConsumoApplicato = 0 : nConsumoDa = 1
                    If Not ListScaglioni Is Nothing Then
                        For Each myTar As OggettoScaglione In ListScaglioni
                            nConsumoCalcolo = GetConsumoCalcolo(nConsumoTot, nConsumoDa, oMyContatore.nTipoUtenza, myTar)
                            If nConsumoCalcolo > 0 Then
                                'creo il nuovo oggetto dovuto scaglione
                                oScaglione = New ObjTariffeScaglione
                                oScaglione.sIdEnte = oMyContatore.sIdEnte
                                oScaglione.sAnno = myTar.sAnno
                                oScaglione.nIdScaglione = myTar.ID
                                oScaglione.nDa = myTar.DA
                                oScaglione.nA = myTar.A
                                oScaglione.nQuantita = FormatNumber(((nConsumoCalcolo / ConstSession.BaseTempo) * myItem.nGiorni), 2) * oMyContatore.nNumeroUtenze
                                'aggiorno il consumo da utilizzare
                                nConsumoApplicato += nConsumoCalcolo
                                nConsumoDa = nConsumoApplicato + 1
                                'aggiorno l'array
                                nList += 1
                                ReDim Preserve oListScaglioni(nList)
                                oListScaglioni(nList) = oScaglione
                                Log.Debug("OPENgovH2O.AnalisiScaglioni.CalcoloScaglioni.Matricola=" + oMyContatore.sMatricola + "|DataLet=" + myItem.tDataLetturaAtt.ToString + "|DataLetPrec=" + myItem.tDataLetturaPrec.ToString + "|Lett=" + myItem.nLetturaAtt.ToString + "|LettPrec=" + myItem.nLetturaPrec.ToString + "|NUtenze=" + oMyContatore.nNumeroUtenze.ToString + "|SubConsumo=" + myItem.nConsumoSubContatore.ToString + "|Consumo=" + nConsumoTot.ToString + "|GG=" + myItem.nGiorni.ToString + "|TipoUtenza=" + oMyContatore.nTipoUtenza.ToString + "|Da=" + oScaglione.nDa.ToString + "|A=" + oScaglione.nA.ToString + "|Qta=" + oScaglione.nQuantita.ToString)
                            End If
                        Next
                        oMyFattura.oScaglioni = oListScaglioni
                        '*** 20121213 - per nuova gestione analisi scaglioni ***
                        oMyFattura.nUtenze = oMyContatore.nNumeroUtenze
                        '*** ***
                    End If
                Next
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.CalcoloScaglioni.errore: ", Err)
            Return 0
        End Try
    End Function
    'Private Function CalcoloScaglioni(ByVal oMyContatore As objContatore, ByVal oMyTariffe As OggettoScaglione(), ByVal nMyBaseTempo As Integer, ByVal sOperatore As String, ByRef oMyFattura As ObjFattura, ByRef oMyListAliquote() As oDettaglioAliquote) As Integer
    '    Dim oScaglione As ObjTariffeScaglione
    '    Dim oListScaglioni() As ObjTariffeScaglione
    '    Dim nList As Integer = -1
    '    Dim x, y As Integer
    '    Dim nConsumoDa, nConsumoTot, nConsumoApplicato, nConsumoCalcolo As Double

    '    oListScaglioni = Nothing
    '    Try
    '        If oMyContatore.bEsenteAcqua = False Then
    '            For x = 0 To oMyContatore.oListLetture.GetUpperBound(0)
    '                nConsumoTot = GetConsumo(oMyContatore.oListLetture(x).nConsumo, oMyContatore.nNumeroUtenze, oMyContatore.oListLetture(x).nTipoArrotondConsumo)
    '                nConsumoApplicato = 0 : nConsumoDa = 1
    '                If Not oMyTariffe Is Nothing Then
    '                    For y = 0 To oMyTariffe.GetUpperBound(0)
    '                        nConsumoCalcolo = GetConsumoCalcolo(nConsumoTot, nConsumoDa, oMyContatore.nTipoUtenza, oMyTariffe(y))
    '                        If nConsumoCalcolo > 0 Then
    '                            'creo il nuovo oggetto dovuto scaglione
    '                            oScaglione = New ObjTariffeScaglione
    '                            oScaglione.sIdEnte = oMyContatore.sIdEnte
    '                            oScaglione.sAnno = oMyTariffe(y).sAnno
    '                            oScaglione.nIdScaglione = oMyTariffe(y).ID
    '                            oScaglione.nDa = oMyTariffe(y).DA
    '                            oScaglione.nA = oMyTariffe(y).A
    '                            oScaglione.nQuantita = nConsumoCalcolo * oMyContatore.nNumeroUtenze
    '                            'aggiorno il consumo da utilizzare
    '                            nConsumoApplicato += nConsumoCalcolo
    '                            nConsumoDa = nConsumoApplicato + 1
    '                            'aggiorno l'array
    '                            nList += 1
    '                            ReDim Preserve oListScaglioni(nList)
    '                            oListScaglioni(nList) = oScaglione
    '                        End If
    '                    Next
    '                    oMyFattura.oScaglioni = oListScaglioni
    '                    '*** 20121213 - per nuova gestione analisi scaglioni ***
    '                    oMyFattura.nUtenze = oMyContatore.nNumeroUtenze
    '                    '*** ***
    '                End If
    '            Next
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.CalcoloScaglioni.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return 0
    '    End Try
    'End Function

    'Private Function GetConsumo(ByVal nMyConsumo As Integer, ByVal nMyUtenze As Integer, ByVal nMyArrotondamento As Integer) As Double
    '    Dim dMyConsumo As Double = -1

    '    Try
    '        dMyConsumo = (nMyConsumo / nMyUtenze)
    '        Select Case nMyArrotondamento
    '            Case 0
    '                dMyConsumo = FormatNumber(dMyConsumo, 2)
    '            Case 1
    '                dMyConsumo = CInt(dMyConsumo)
    '            Case 2
    '                dMyConsumo = CInt(dMyConsumo + 0.5)
    '        End Select
    '    Catch Err As Exception
    '        dMyConsumo = 0
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetConsumo.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    '    Return dMyConsumo
    'End Function
    ''' <summary>
    ''' Funzione che restituisce il valore del consumo da calcolare in base allo scaglione di riferimento
    ''' </summary>
    ''' <param name="nConsumoARif"></param>
    ''' <param name="nConsumoDaRif"></param>
    ''' <param name="nTipoUtenzaRif"></param>
    ''' <param name="oScaglioneRif"></param>
    ''' <returns></returns>
    Private Function GetConsumoCalcolo(ByVal nConsumoARif As Double, ByRef nConsumoDaRif As Double, ByVal nTipoUtenzaRif As Integer, ByVal oScaglioneRif As OggettoScaglione) As Double
        Try
            Dim nConsumoCalcolo As Double = 0

            If nTipoUtenzaRif = oScaglioneRif.idTipoUtenza Then
                If nConsumoDaRif >= oScaglioneRif.DA Then
                    If nConsumoARif <= oScaglioneRif.A Then
                        nConsumoCalcolo = (nConsumoARif - nConsumoDaRif) + 1
                    Else
                        nConsumoCalcolo = (oScaglioneRif.A - nConsumoDaRif) + 1
                    End If
                End If
            End If
            Return nConsumoCalcolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.GetConsumoCalcolo.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return 0
        End Try
    End Function
#End Region

#Region "Stampa"
    Public Function PrintAnalisiScaglioni(ByVal sPeriodo As String, ByVal oFinale() As OggettoScaglioneFinale, ByVal numCol As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'Carico il dataset con relative colonne;
            DsStampa.Tables.Add("STAMPA")
            For x = 0 To numCol
                DsStampa.Tables("STAMPA").Columns.Add(Space(x))
            Next
            'inserisco l’intestazione del report tramite la funzione AddRowStampa(DtStampa, sDatiStampa);
            DtStampa = DsStampa.Tables("STAMPA")
            sDatiStampa = "Analisi Scaglioni||Data Stampa " & DateTime.Now.Date
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            RigaVuota(DtStampa, numCol)
            'Descrizione Ente
            DtStampa = DsStampa.Tables("STAMPA")
            sDatiStampa = "Ente " & Session("DESCRIZIONE_ENTE")
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + numCol, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            RigaVuota(DtStampa, numCol)
            'Periodo
            DtStampa = DsStampa.Tables("STAMPA")
            sDatiStampa = "Periodo " & sPeriodo
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + numCol, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            RigaVuota(DtStampa, numCol)

            'inserisco le intestazioni di colonna tramite la funzione AddRowStampa(DtStampa, sDatiStampa);
            sDatiStampa = "Tipo Utenza|Scaglione|Consumo|N.Utenze"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To oFinale.GetUpperBound(0)
                'valorizzo il record da stampare e o inserisco tramite la funzione AddRowStampa(DtStampa, sDatiStampa);
                sDatiStampa = oFinale(x).sDescrizioneTU.ToString
                sDatiStampa += "|'" + oFinale(x).DA.ToString + " - " + oFinale(x).A.ToString
                sDatiStampa += "|" + oFinale(x).dQuantita.ToString
                '*** 20121213 - per nuova gestione analisi scaglioni ***
                sDatiStampa += "|" + oFinale(x).nUtenze.ToString
                '*** ***
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.PrintAnalisiScaglioni.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw Err
        End Try
    End Function

    Private Function RigaVuota(ByRef DtStampa As DataTable, ByVal numCol As Integer) As String
        Dim sDatiStampa As String
        sDatiStampa = ""
        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + numCol, "|")
        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
            Return Nothing
        End If
        Return ""
    End Function

    Private Function AddRowStampa(ByRef DtAddRow As DataTable, ByVal sValueRow As String) As Integer
        Dim sTextRow() As String
        Dim DrAddRow As DataRow
        Dim x As Integer = 0

        Try
            'aggiungo una nuova riga nel datarow
            DrAddRow = DtAddRow.NewRow
            'controllo se la riga e\' scritta
            If sValueRow <> "" Then
                sTextRow = sValueRow.Split(CChar("|"))
                For x = 0 To UBound(sTextRow)
                    'popolo la riga nel datarow
                    DrAddRow(x) = sTextRow(x)
                Next
            End If
            'aggiorno la riga al datatable
            DtAddRow.Rows.Add(DrAddRow)

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AnalisiScaglioni.AddRowStampa.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return 0
        End Try
    End Function
#End Region
End Class

Public Class ScaglioniComparer
    Implements IComparer
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ScaglioniComparer))
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Try
            If x.idTipoUtenza < y.idTipoUtenza Then
                Return -1
            ElseIf x.idTipoUtenza = y.idTipoUtenza Then
                If x.DA < y.DA Then
                    Return -1
                ElseIf x.DA = y.DA Then
                    Return 0
                Else
                    Return 1
                End If
                Return 0
            Else
                Return 1
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ScaglioniComparer.Compare.errore: ", Err)
        End Try
    End Function
End Class

Public Class OggettoScaglioneFinale
    Private _id As Integer = 0
    Private _idTipoUtenza As Integer = 0
    Private _sDescrizioneTU As String = ""
    Private _idEnte As String = ""
    Private _da As Integer = 0
    Private _a As Integer = 0
    Private _anno As String = ""
    Private _quantita As Double = 0
    Private _nUtenze As Integer = 0

    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal Value As Integer)
            _id = Value
        End Set
    End Property
    Public Property idTipoUtenza() As Integer
        Get
            Return _idTipoUtenza
        End Get
        Set(ByVal Value As Integer)
            _idTipoUtenza = Value
        End Set
    End Property
    Public Property sDescrizioneTU() As String
        Get
            Return _sDescrizioneTU
        End Get
        Set(ByVal Value As String)
            _sDescrizioneTU = Value
        End Set
    End Property
    Public Property DA() As Integer
        Get
            Return _da
        End Get
        Set(ByVal Value As Integer)
            _da = Value
        End Set
    End Property
    Public Property A() As Integer
        Get
            Return _a
        End Get
        Set(ByVal Value As Integer)
            _a = Value
        End Set
    End Property
    Public Property sIdEnte() As String
        Get
            Return _idEnte
        End Get
        Set(ByVal Value As String)
            _idEnte = Value
        End Set
    End Property
    Public Property sAnno() As String
        Get
            Return _anno
        End Get
        Set(ByVal Value As String)
            _anno = Value
        End Set
    End Property
    Public Property dQuantita() As Double
        Get
            Return _quantita
        End Get
        Set(ByVal Value As Double)
            _quantita = Value
        End Set
    End Property
    '*** 20121213 - per nuova gestione analisi scaglioni ***
    Public Property nUtenze() As Integer
        Get
            Return _nUtenze
        End Get
        Set(ByVal Value As Integer)
            _nUtenze = Value
        End Set
    End Property
    '*** ***
End Class