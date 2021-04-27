Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class SearchContatori
  Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchContatori))
    Private ModDate As New ClsGenerale.Generale
    Private _Const As New Costanti
    Private iDB As New DBAccess.getDBobject
    Protected UrlStradario As String = ConstSession.UrlStradario

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents grdContatori As RibesDataGrid.RibesDataGrid.RibesDataGrid
    Protected WithEvents txtNomeVia As System.Web.UI.WebControls.TextBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region

    ''' <summary>
    ''' Gestione del contatore
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sOrigineRichiamo As String = ""

        Try
            If TxtViaRibaltata.Text <> "" And TxtViaRibaltata.Text <> " " Then
                TxtVia.Text = TxtViaRibaltata.Text
            Else
                TxtVia.Text = ""
            End If
            Dim FncGen As New ClsGenerale.Generale
            FncGen.GetPeriodoAttuale()
            '*** 20140923 - GIS ***
            txtNominativoIntestatario.Attributes.Add("onkeydown", "keyPress();")
            txtNomeIntestatario.Attributes.Add("onkeydown", "keyPress();")
            txtNominativoUtente.Attributes.Add("onkeydown", "keyPress();")
            txtNomeUtente.Attributes.Add("onkeydown", "keyPress();")
            txtMatricola.Attributes.Add("onkeydown", "keyPress();")
            txtContratto.Attributes.Add("onkeydown", "keyPress();")
            txtNumeroUtente.Attributes.Add("onkeydown", "keyPress();")
            TxtVia.Attributes.Add("onkeydown", "keyPress();")
            cboStato.Attributes.Add("onkeydown", "keyPress();")
            chksub.Attributes.Add("onkeydown", "keyPress();")

            Session("datacatasto") = Nothing
            Session("oListSubContatori") = Nothing
            Session("myContatore") = Nothing
            If Not Page.IsPostBack Then
                '*** Fabi
                '*** Aggancio stradario
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
                LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
                '*** /Fabi
                LoadCombo()
                If Not Request.Item("Org") Is Nothing Then
                    sOrigineRichiamo = Request.Item("Org").ToString
                End If
                If sOrigineRichiamo = "GIS" Then
                    Dim sRif() As String
                    sRif = Request.Item("RifCat").ToString.Split("-")
                    txtFoglio.Text = sRif(0)
                    txtNumero.Text = sRif(1)
                    Dim sScript As String = ""
                    sScript += "Search(1);"
                    RegisterScript(sScript, Me.GetType())
                Else
                    If Not Session("ParamSearchContatori") Is Nothing Then
                        Dim ListParamSearch As New ArrayList
                        Dim x As Integer = 0
                        ListParamSearch = Session("ParamSearchContatori")
                        txtMatricola.Text = ListParamSearch(x) : x += 1
                        txtNumeroUtente.Text = ListParamSearch(x) : x += 1
                        TxtCodVia.Text = ListParamSearch(x) : x += 1
                        txtNominativoIntestatario.Text = ListParamSearch(x) : x += 1
                        txtNomeIntestatario.Text = ListParamSearch(x) : x += 1
                        txtNominativoUtente.Text = ListParamSearch(x) : x += 1
                        txtNomeUtente.Text = ListParamSearch(x) : x += 1
                        cboStato.SelectedValue = ListParamSearch(x) : x += 1
                        If ListParamSearch(x) = 1 Then
                            chksub.Checked = True
                        End If
                        x += 1
                        txtFoglio.Text = ListParamSearch(x) : x += 1
                        txtNumero.Text = ListParamSearch(x) : x += 1
                        txtContratto.Text = ListParamSearch(x) : x += 1
                        '*** 201511 - Funzioni Sovracomunali ***
                        ddlEnti.SelectedValue = ListParamSearch(x)
                        '*** ***
                        Session("ParamSearchContatori") = Nothing
                        Dim sScript As String = ""
                        sScript += "Search();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If
            End If
            '*** ***
            '*** 201511 - Funzioni Sovracomunali ***
            If ConstSession.IdEnte <> "" Then
                ddlEnti.SelectedValue = ConstSession.IdEnte
                Dim strScript As String = ""
                strScript += "document.getElementById ('lblEnti').style.display='none';"
                strScript += "document.getElementById ('ddlEnti').style.display='none';"
                RegisterScript(strScript, Me.GetType())
            End If
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContatori.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' pulsante per l'inserimento di una nuova posizione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        CalPageaspx(0)
    End Sub
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="IDContatore">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal IDContatore As Integer)
        Dim sScript As String = ""
        sScript += "location.href='DatiGenerali.aspx?IDCONTATORE=" & IDContatore & "';"
        RegisterScript(sScript, Me.GetType())
    End Sub

    ''' <summary>
    ''' Estrazione file xls con elenco selezione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' <strong>Funzioni Sovracomunali</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub StampaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim FncContatori As New GestContatori
        Dim DtDatiStampa As New DataTable
        Dim dvStampa As DataView
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim IdEnte As String = ""
        If ddlEnti.SelectedValue <> "" Then
            IdEnte = ddlEnti.SelectedValue
        End If
        nCol = 32
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ELENCO_CONTATORI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        'prelevo i dati da stampare
        dvStampa = FncContatori.GetElencoContrattiContatori(ConstSession.StringConnection, IdEnte, txtMatricola.Text, txtNumeroUtente.Text, TxtVia.Text, txtNominativoIntestatario.Text, txtNomeIntestatario.Text, txtNominativoUtente.Text, txtNomeUtente.Text, -1, cboStato.SelectedIndex, chksub.Checked, -1)
        DtDatiStampa = FncStampa.PrintElencoContatori(dvStampa, IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
        If Not DtDatiStampa Is Nothing Then
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
        End If
    End Sub
    ''' <summary>
    ''' pulsante di richiamo al servizio di cartografia esterno
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdGIS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdGIS.Click
        Dim CodeGIS, sScript, sRifPrec As String
        Dim fncGIS As New RemotingInterfaceAnater.GIS
        Dim listRifCat As New Generic.List(Of Anater.Oggetti.RicercaUnitaImmobiliareAnater)
        Dim myRifCat As New Anater.Oggetti.RicercaUnitaImmobiliareAnater
        Try
            sRifPrec = ""
            If Not Session("vistaAna") Is Nothing Then
                Dim listUI As DataView = CType(Session("vistaAna"), DataView)
                Dim sListContrib As String = ""
                For Each myUI As DataRowView In listUI
                    If myUI("bSel") Then
                        sListContrib += myUI("CODCONTATORE").ToString + ","
                    End If
                Next
                If sListContrib.Length > 1 Then
                    sListContrib = sListContrib.Substring(0, sListContrib.Length - 1)
                End If
                If sListContrib <> "" Then
                    Dim FncContatori As New GestContatori
                    Dim ListDatiCat() As objDatiCatastali
                    ListDatiCat = FncContatori.GetListaCatasto(ConstSession.IdEnte, sListContrib)
                    For Each myRif As objDatiCatastali In ListDatiCat
                        If myRif.sFoglio <> "" Then
                            If sRifPrec <> myRif.sFoglio + "|" + myRif.sNumero + "|" + myRif.nSubalterno.ToString Then
                                myRifCat = New Anater.Oggetti.RicercaUnitaImmobiliareAnater
                                myRifCat.Foglio = myRif.sFoglio
                                myRifCat.Mappale = myRif.sNumero
                                myRifCat.Subalterno = myRif.nSubalterno.ToString
                                myRifCat.CodiceRicerca = ConstSession.Belfiore
                                listRifCat.Add(myRifCat)
                            End If
                        End If
                        sRifPrec = myRif.sFoglio + "|" + myRif.sNumero + "|" + myRif.nSubalterno.ToString
                    Next
                    If listRifCat.ToArray.Length > 0 Then
                        CodeGIS = fncGIS.getGIS(ConstSession.UrlWSGIS, listRifCat.ToArray())
                        If Not CodeGIS Is Nothing Then
                            sScript = "window.open('" & ConstSession.UrlWebGIS & CodeGIS & "','wdwGIS')"
                            RegisterScript(sScript, Me.GetType())
                        Else
                            sScript = "GestAlert('a', 'danger', '', '', 'Errore in interrogazione Cartografia!');"
                            RegisterScript(sScript, Me.GetType())
                        End If
                    Else
                        sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContatori.CmdGIS_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***

    Private Sub LoadCombo()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboStato.Items.Add(myListItem)
            myDataReader = iDB.GetDataReader("Select * From TP_STATI")
            While myDataReader.Read()
                myListItem = New ListItem
                myListItem.Text = myDataReader.GetString(1)
                myListItem.Value = myDataReader.GetInt32(0)
                cboStato.Items.Add(myListItem)
            End While
            myDataReader.Close()

            '*** 201511 - Funzioni Sovracomunali ***
            iDB.m_Connection = ConstSession.StringConnectionOPENgov
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "ENTI_S"
            cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.NVarChar).Value = ConstSession.Ambiente
            cmdMyCommand.Parameters.Clear()
            myDataReader = iDB.GetDataReader(cmdMyCommand) 'cmdMyCommand.ExecuteReader
            Try
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
            Catch ex As Exception
                Log.Debug("loadCombos::si è verificato il seguente errore::" + ex.Message)
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContatori.LoadCombo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di LoadCombo " + ex.Message)
            '*** ***
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
End Class
