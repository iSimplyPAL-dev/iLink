Imports log4net
Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
'Imports WsStradario

Partial Class RicercaLetture
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaLetture))
    Private usControl As UserControl
        Protected UrlStradario As String = ConstSession.UrlStradario
    Private Generali As New Generali
    Private _Const As New Costanti

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnEvento As System.Web.UI.WebControls.Button
    Protected WithEvents btnClear As System.Web.UI.WebControls.Button
    Protected WithEvents tblUserContrl As System.Web.UI.WebControls.Table
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ''' <summary>
    ''' Gestione delle letture
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Letture As New GestLetture

        'txtIntestatario.Attributes.Add("onkeydown", "keyPress();")
        'txtUtente.Attributes.Add("onkeydown", "keyPress();")
        'txtNumeroUtente.Attributes.Add("onkeydown", "keyPress();")
        'txtMatricola.Attributes.Add("onkeydown", "keyPress();")

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
            Dim FncGen As New ClsGenerale.Generale
        FncGen.GetPeriodoAttuale
        Dim paginacomandi As String = Request("paginacomandi")
        Try
            If Len(paginacomandi) = 0 Then
                paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/DataEntryLetture/ComandiLetture.aspx"
            End If
            Dim parametri As String
            parametri = "?title=Acquedotto - " & ConstSession.DescrTipoProcServ & " - " & "Ricerca" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

            dim sScript as string=""
            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            RegisterScript(sScript , Me.GetType())
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            sScript += "document.getElementById('hdCodiceVia').value='" & "-1" & "';"
            RegisterScript(sScript, Me.GetType())

            Dim DetailsSearchLetture As DetailsSearchLetture = Letture.GetDetailsSearchLetture(CInt(ConstSession.IdEnte), ConstSession.CodIstat)
            If Not Page.IsPostBack Then
                'Caricamento del Combo dei Giri 
                Generali.LoadDropDownList(cboGiro, DetailsSearchLetture.dsGiro, "IDGIRO", "DESCRIZIONE", DetailsSearchLetture.lngIDGiro)

                '*** Fabi
                '*** Aggancio stradario
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
                LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
                '*** /Fabi
                If Not Session("ParamSearchLetture") Is Nothing Then
                    Dim ListParamSearch As New ArrayList
                    Dim x As Integer = 0
                    ListParamSearch = Session("ParamSearchLetture")
                    txtIntestatario.Text = ListParamSearch(x) : x += 1
                    txtUtente.Text = ListParamSearch(x) : x += 1
                    TxtCodVia.Text = ListParamSearch(x) : x += 1
                    TxtVia.Text = ListParamSearch(x) : x += 1
                    cboGiro.SelectedValue = ListParamSearch(x) : x += 1
                    txtNumeroUtente.Text = ListParamSearch(x) : x += 1
                    cboContatoriCessati.SelectedValue = ListParamSearch(x) : x += 1
                    txtMatricola.Text = ListParamSearch(x) : x += 1
                    If ListParamSearch(x) = 1 Then
                        chksub.Checked = True
                    End If
                    x += 1
                    If ListParamSearch(x) = 1 Then
                        chkLetturaPresente.Checked = True
                    End If
                    x += 1
                    If ListParamSearch(x) = 1 Then
                        chkLetturaMancante.Checked = True
                    End If
                    Session("ParamSearchLetture") = Nothing
                    sScript = "Search();"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If

            'txtIntestatario.Text = Request("hdIntestatario")
            'txtUtente.Text = Request("hdUtente")
            'txtNumeroUtente.Text = Request("hdNumeroUtente")
            'txtMatricola.Text = Request("hdMATRICOLA")

            'If Len(Request("hdCessati")) > 0 Then
            '    Dim B As ListItem = cboContatoriCessati.Items.FindByValue(Request("hdCessati"))

            '    If Not B Is Nothing Then
            '        B.Selected = True
            '    End If

            'End If
            'If Len(Request("hdGiro")) > 0 Then
            '    Dim B As ListItem = cboGiro.Items.FindByValue(Request("hdGiro"))

            '    If Not B Is Nothing Then
            '        B.Selected = True
            '    End If
            'End If

            If Request("PAG_PREC") = Costanti.enmContesto.DELETTURE Then
                sScript = "Search();"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaLetture.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub

    'Public Function TrovaStradario(ByVal codiceVia As Object) As String
    '    Dim nomeStrada As String
    'Try
    '    If Not IsDBNull(codiceVia) Then
    '        Dim objStradario As Stradario = New Stradario
    '        Dim ArrStrade() As OggettiComuniStrade.OggettoStrada
    '        Dim strada As New OggettiComuniStrade.OggettoStrada
    '        strada.CodiceStrada = codiceVia
    '        strada.CodiceEnte = CInt(ConstSession.IdEnte)
    '        ArrStrade = objStradario.GetStrade(strada)
    '        If Not ArrStrade Is Nothing Then
    '            If ArrStrade.Length.CompareTo(1) = 0 Then
    '                nomeStrada = ArrStrade(0).TipoStrada.ToString() + " " + ArrStrade(0).DenominazioneStrada.ToString()
    '            Else
    '                nomeStrada = ""
    '            End If
    '        Else
    '            nomeStrada = ""
    '        End If
    '    Else
    '        nomeStrada = ""
    '    End If

    '    Return nomeStrada
    '  Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaLetture.TrovaStradario.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Sub FillDropDownSQLStrade(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long)

    '    Try
    '        Dim myListItem As ListItem
    '        myListItem = New ListItem
    '        myListItem.Text = "..."
    '        myListItem.Value = "-1"
    '        cboTemp.Items.Add(myListItem)
    '        While dr.Read()
    '            myListItem = New ListItem

    '            myListItem.Text = dr.GetString(2) & "--" & dr.GetString(1)
    '            myListItem.Value = dr.GetInt32(0)
    '            cboTemp.Items.Add(myListItem)

    '        End While

    '        If lngSelectedID <> -1 Then
    '            SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
    '        End If

    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaLetture.FillDropDownSQLStrade.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not dr Is Nothing Then
    '            dr.Close()
    '        End If
    '    End Try

    'End Sub

    'Private Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)
    '    Dim blnFindElement As Boolean = False
    '    Dim intCount As Integer = 1
    '    Dim intNumberElements As Integer = cboTemp.Items.Count
    'Try
    '    Do While intCount < intNumberElements
    '        cboTemp.SelectedIndex = intCount
    '        If cboTemp.SelectedItem.Value = strValue Then
    '            cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
    '            blnFindElement = True
    '            Exit Do
    '        End If
    '        intCount = intCount + 1
    '    Loop
    '    If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaLetture.SelectIndexDropDownList.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

#Region "Stampa per Letturista"
    ''' <summary>
    ''' Estrazione file xls per letturista
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StampaExcel.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x As Integer
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim FncCont As New GestContatori
        Dim dvStampa As DataView

        Log.Debug("Entro in RicercaLetture::StampaExcel_Click::Stampa per Letturista::" & Now.ToString)
        'inizializzo la connessione
        Log.Debug("Sono in RicercaLetture::StampaExcel_Click::Stampa per Letturista:: devo valorizzare il datatable::" & Now.ToString)
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ELENCO_LETTURISTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        dvStampa = FncCont.GetTableContatoriAttivi(cboGiro.SelectedItem.Value, CInt(TxtCodVia.Text), ConstSession.IdEnte, ConstSession.IdPeriodo)
        DtDatiStampa = FunctionStampa.PrintStampaLetturista(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
        Log.Debug("Sono in RicercaLetture::StampaExcel_Click::Stampa per Letturista:: ho valorizzato il datatable::" & Now.ToString)
        If Not DtDatiStampa Is Nothing Then
            'definisco le colonne
            Log.Debug("Sono in RicercaLetture::StampaExcel_Click::Stampa per Letturista:: definisco le colonne::" & Now.ToString)
            aListColonne = New ArrayList
            For x = 0 To 35
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35}
            'esporto i dati in excel
            Log.Debug("Sono in RicercaLetture::StampaExcel_Click::Stampa per Letturista:: esporto i dati in excel::" & Now.ToString)
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            Log.Debug("Esco da RicercaLetture::StampaExcel_Click::Stampa per Letturista::" & Now.ToString)
        End If
    End Sub
#End Region

#Region "Stampa Contatori Cessati, Sostituiti, Sospesi"
    ''' <summary>
    ''' Estrazione file xls per i contatori sospesi, cessati e sostituiti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StampaExcel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel2.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x As Integer
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim FncCont As New GestContatori
        Dim dvStampa As DataView
        Log.Debug("Entro in RicercaLetture::StampaExcel2_Click::Stampa Contatori Cessati, Sostituiti, Sospesi::" & Now.ToString)
        'inizializzo la connessione
        Log.Debug("Sono in RicercaLetture::StampaExcel2_Click::Stampa Contatori Cessati, Sostituiti, Sospesi:: devo valorizzare il datatable::" & Now.ToString)
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ELENCO_CESSATI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        dvStampa = FncCont.GetTableContatoriCessati(ConstSession.IdEnte, ConstSession.IdPeriodo, txtIntestatario.Text, txtUtente.Text, TxtVia.Text, txtNumeroUtente.Text, txtMatricola.Text, cboGiro.SelectedValue, chksub.Checked)
        DtDatiStampa = FunctionStampa.PrintContatoriCessati(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
        Log.Debug("Sono in RicercaLetture::StampaExcel2_Click::Stampa Contatori Cessati, Sostituiti, Sospesi:: ho valorizzato il datatable::" & Now.ToString)
        If Not DtDatiStampa Is Nothing Then
            'definisco le colonne
            Log.Debug("Sono in RicercaLetture::StampaExcel2_Click::Stampa Contatori Cessati, Sostituiti, Sospesi:: definisco le colonne::" & Now.ToString)
            aListColonne = New ArrayList
            For x = 0 To 29
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29}
            'esporto i dati in excel
            Log.Debug("Sono in RicercaLetture::StampaExcel2_Click::Stampa Contatori Cessati, Sostituiti, Sospesi:: esporto i dati in excel::" & Now.ToString)
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            Log.Debug("Esco da RicercaLetture::StampaExcel2_Click::Stampa Contatori Cessati, Sostituiti, Sospesi::" & Now.ToString)
        End If
    End Sub
#End Region

#Region "Stampa Letture"
    ''' <summary>
    ''' Estrazione file xls per letture
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub StampaExcel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel3.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x As Integer
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim FncLet As New GestLetture
        Dim dvStampa As DataView
        Log.Debug("Entro in RicercaLetture::StampaExcel3_Click::Stampa Letture::" & Now.ToString)
        'inizializzo la connessione
        Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: devo valorizzare il datatable::" & Now.ToString)
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ELENCO_LETTURE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        dvStampa = FncLet.getTableLetture(ConstSession.StringConnection, ConstSession.IdEnte, ConstSession.IdPeriodo, txtIntestatario.Text, txtUtente.Text, TxtVia.Text, txtNumeroUtente.Text, txtMatricola.Text, cboGiro.SelectedValue, chksub.Checked)
        DtDatiStampa = FunctionStampa.PrintLetture(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
        Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: ho valorizzato il datatable::" & Now.ToString)
        If Not DtDatiStampa Is Nothing Then
            'definisco le colonne
            Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: definisco le colonne::" & Now.ToString)
            aListColonne = New ArrayList
            For x = 0 To 30
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30}
            'esporto i dati in excel
            Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: esporto i dati in excel::" & Now.ToString)
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            Log.Debug("Esco da RicercaLetture::StampaExcel3_Click::Stampa Letture::" & Now.ToString)
        End If
    End Sub
    'Private Sub StampaExcel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel3.Click
    '    Dim sPathProspetti, sNameXLS, Str As String
    '    Dim x As Integer
    '    Dim FunctionStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()
    '    Dim FncLet As New GestLetture
    '    Dim dvStampa As DataView
    '    Log.Debug("Entro in RicercaLetture::StampaExcel3_Click::Stampa Letture::" & Now.ToString)
    '    'inizializzo la connessione
    '    Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: devo valorizzare il datatable::" & Now.ToString)
    '    'valorizzo il nome del file
    '    sPathProspetti = ConstSession.PathProspetti
    '    sNameXLS = ConstSession.IdEnte & "_ELENCO_LETTURE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '    dvStampa = FncLet.getTableLetture(ConstSession.IdEnte, ConstSession.IdPeriodo, txtIntestatario.Text, txtUtente.Text, TxtVia.Text, txtNumeroUtente.Text, txtMatricola.Text, cboGiro.SelectedValue, chksub.Checked)
    '    DtDatiStampa = FunctionStampa.PrintLetture(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
    '    Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: ho valorizzato il datatable::" & Now.ToString)
    '    If Not DtDatiStampa Is Nothing Then
    '        'definisco le colonne
    '        Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: definisco le colonne::" & Now.ToString)
    '        aListColonne = New ArrayList
    '        For x = 0 To 30
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '        'definisco l'insieme delle colonne da esportare
    '        Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30}
    '        'esporto i dati in excel
    '        Log.Debug("Sono in RicercaLetture::StampaExcel3_Click::Stampa Letture:: esporto i dati in excel::" & Now.ToString)
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '        Str = sPathProspetti & sNameXLS
    '        Log.Debug("Esco da RicercaLetture::StampaExcel3_Click::Stampa Letture::" & Now.ToString)
    '        End If
    'End Sub
#End Region

#Region "Stampa Letture Non Presenti"
    ''' <summary>
    ''' Estrazione file xls per letture non presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StampaExcel4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel4.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x As Integer
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim FncLet As New GestLetture
        Dim dvStampa As DataView
        Log.Debug("Entro in RicercaLetture::StampaExcel4_Click::Stampa Letture Non Presenti::" & Now.ToString)
        'inizializzo la connessione
        Log.Debug("Sono in RicercaLetture::StampaExcel4_Click::Stampa Letture Non Presenti:: devo valorizzare il datatable::" & Now.ToString)
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ELENCO_LETTURE_NON_PRESENTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        dvStampa = FncLet.getTableLettureNonPresenti(ConstSession.IdEnte, ConstSession.IdPeriodo, txtIntestatario.Text, txtUtente.Text, TxtVia.Text, txtNumeroUtente.Text, txtMatricola.Text, cboGiro.SelectedValue, chksub.Checked)
        DtDatiStampa = FunctionStampa.PrintLettureMancanti(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
        Log.Debug("Sono in RicercaLetture::StampaExcel4_Click::Stampa Letture Non Presenti:: ho valorizzato il datatable::" & Now.ToString)
        If Not DtDatiStampa Is Nothing Then
            'definisco le colonne
            Log.Debug("Sono in RicercaLetture::StampaExcel4_Click::Stampa Letture Non Presenti:: definisco le colonne::" & Now.ToString)
            aListColonne = New ArrayList
            For x = 0 To 25
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25}
            'esporto i dati in excel
            Log.Debug("Sono in RicercaLetture::StampaExcel4_Click::Stampa Letture Non Presenti:: esporto i dati in excel::" & Now.ToString)
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            Log.Debug("Esco da RicercaLetture::StampaExcel4_Click::Stampa Letture Non Presenti::" & Now.ToString)
        End If
    End Sub
#End Region
End Class
