Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports WsStradario
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net
Imports Utility

Partial Class RicercaContratti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaContratti))
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Dim _Const As New Costanti
    Dim DBAccess As New DBAccess.getDBobject
    Private ModDate As New ClsGenerale.Generale
    Protected UrlStradario As String = ConstSession.UrlStradario

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
    ''' Gestione del contratto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        'Put user code to initialize the page here
        Try
            If Not Page.IsPostBack Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale()
                '*** Fabi
                '*** Aggancio stradario
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
                LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
                '*** /Fabi
                Dim paginacomandi As String = Request("paginacomandi")
                If Len(paginacomandi) = 0 Then
                    paginacomandi = "../ComandiRicerca/ComandiRicerca.aspx"
                End If

                Dim parametri As String
                parametri = "?title=Acquedotto - " & ConstSession.DescrTipoProcServ & " - " & "Ricerca" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())

                If Request.Params("eliminato") = 1 Then
                    sScript += "GestAlert('a', 'success', '', '', 'Il contratto e\' stato eliminato correttamente');"
                    RegisterScript(sScript, Me.GetType)
                End If

                Dim myListItem As ListItem
                myListItem = New ListItem
                myListItem.Text = "..."
                myListItem.Value = "-1"
                cboStato.Items.Add(myListItem)

                Dim dvMyDati As New DataView
                dvMyDati = DBAccess.GetDataView("Select * From TP_STATI")
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myListItem = New ListItem
                        myListItem.Text = StringOperation.FormatString(myRow(1))
                        myListItem.Value = StringOperation.FormatInt(myRow(0))
                        cboStato.Items.Add(myListItem)
                    Next
                End If
                dvMyDati.Dispose()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaContratti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

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
    '  Catch Err As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContratti.SelectIndexDropDownList.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Sub FillDropDownSQLStrade(ByVal cboTemp As DropDownList, ByVal dr As new dataview, ByVal lngSelectedID As Long)
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

    '    Catch myException As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContratti.FillDropDownSQLStrade.errore: ", myException)
    '    Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not dr Is Nothing Then
    '            dr.Close()
    '        End If
    '    End Try

    'End Sub

    'HERE ALE CAO
    ''' <summary>
    ''' Richiama la Pagina di gestione
    ''' </summary>
    ''' <param name="IDContratto">intero</param>
    ''' <remarks></remarks>
    Protected Sub CalPageaspx(ByVal IDContratto As Integer)

        Dim sScript As String = ""


        sScript += "location.href='DatiGeneraliContr.aspx?IDCONTRATTO=" & IDContratto & "';"
        'sscript+="GestAlert('a', 'warning', '', '', 'ciao');")
        RegisterScript(sScript, Me.GetType())

    End Sub
    ''' <summary>
    ''' pulsante per l'inserimento di una nuova posizione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        CalPageaspx(0)
    End Sub

    'Private Sub cboUbicazione_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUbicazione.SelectedIndexChanged

    'End Sub

    'Private Sub cboStato_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStato.SelectedIndexChanged
    '    'Dim prova As Integer = cboStato.SelectedIndex
    'End Sub

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
    'Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContratti.TrovaStradario.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Private Sub StampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StampaExcel.Click
    '    Dim DEContRATTIPR As GestContratti = New GestContratti

    '    Dim dtStampaRiassuntiva As DataTable = DEContRATTIPR.getTableStampaRiassuntiva()


    '    Dim dr As DataRow
    '    dr = dtStampaRiassuntiva.NewRow()

    '    Dim mioPeriodo As String = ConstSession.IdPeriodo

    '    Dim datatablevuoto As New DataTable
    '    Dim datarowvuoto As DataRow
    '    'dr(0) = "Stradario.tipostrada"
    '    'dr(1) = "Stradario.strada"
    '    'dr(2) = "tp_contratti.codice_istat"
    '    'dr(3) = "tp_contratti.sesponentecivico"
    '    'dtStampaRiassuntiva.Rows.Add(dr)
    '    Dim var1stringa, var2stringa, var3stringa, var4stringa As String

    '    Dim arrayListNomiColonne As ArrayList
    '    arrayListNomiColonne = New ArrayList

    '    Dim i As Int32
    '    Dim var1 As String
    '    Dim var2 As String
    '    Dim var3 As String
    '    Dim var4 As String
    '    Dim stato As String

    '    Dim miaData As String = DateTime.Now.ToShortDateString()
    '    Dim prova As String = CStr(DateTime.Now.ToString("dd-MM-yyyy"))

    '    Dim sDatiStampa, sUbicazione As String

    '    Dim comune As String
    '    Dim miocodice As String = HttpContext.Current.ConstSession.CodIstat
    '    miocodice = Right(miocodice, Len(miocodice) - 2)
    '    'nUMERO COLONNE
    '    Dim ilenght As Integer
    '    ilenght = 28
    'Try
    '    For i = 1 To ilenght + 1
    '        datatablevuoto.Columns.Add(Space(i))
    '    Next
    '    comune = DEContRATTIPR.GetEnte(CInt(miocodice))
    '    'intestazione ente
    '    sDatiStampa = "Comune di " & comune
    '    If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '        Exit Sub
    '    End If
    '    'riga vuota
    '    If AddRowStampa(datatablevuoto, "|") = 0 Then
    '        Exit Sub
    '    End If
    '    'Data Stampa 
    '    sDatiStampa = "Data Stampa " & DateTime.Now.Date
    '    If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '        Exit Sub
    '    End If
    '    'riga vuota
    '    If AddRowStampa(datatablevuoto, "|") = 0 Then
    '        Exit Sub
    '    End If
    '    'periodo
    '    sDatiStampa = "Periodo " & mioPeriodo
    '    If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '        Exit Sub
    '    End If
    '    'riga vuota
    '    If AddRowStampa(datatablevuoto, "|") = 0 Then
    '        Exit Sub
    '    End If
    '    'titolo report
    '    sDatiStampa = "Elenco Contratti"
    '    If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '        Exit Sub
    '    End If
    '    'riga vuota
    '    If AddRowStampa(datatablevuoto, "|") = 0 Then
    '        Exit Sub
    '    End If

    '    'intestazioni di colonna
    '    sDatiStampa = "|||DATI INTESTATARIO||||||||DATI UTENTE|||||||||UBICAZIONE|||"
    '    If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '        Exit Sub
    '    End If
    '    sDatiStampa = "CODICE CONTRATTO|MATRICOLA|DATA SOTTOSCRIZIONE|"
    '    sDatiStampa += "COGNOME|NOME|COD. FISCALE/P.IVA|VIA|CIVICO|CAP|COMUNE|PROVINCIA|"
    '    sDatiStampa += "NUMERO|COGNOME|NOME|COD. FISCALE/P.IVA|VIA|CIVICO|CAP|COMUNE|PROVINCIA|"
    '    sDatiStampa += "VIA|CIVICO|FOGLIO|NUMERO|SUBALTERNO|"
    '    sDatiStampa += "STATO DEL CONTRATTO|"
    '    sDatiStampa += "TIPOLOGIA UTENZA|NUMERO UTENZE"
    '    If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '        Exit Sub
    '    End If

    '    For i = 0 To dtStampaRiassuntiva.Rows.Count - 1
    '        datarowvuoto = datatablevuoto.NewRow()
    '        sDatiStampa = ""
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("CODCONTRATTO")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("CODCONTRATTO").ToString()
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("MATRICOLA")) Then
    '            sDatiStampa += "'" & dtStampaRiassuntiva.Rows(i)("MATRICOLA")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("DATASOTTOSCRIZIONE")) Then
    '            sDatiStampa += ModDate.GiraDataFromDB(dtStampaRiassuntiva.Rows(i)("DATASOTTOSCRIZIONE"))
    '        End If

    '        'dati intestatario
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("COGNOME_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("COGNOME_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("NOME_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("NOME_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("COD_FISCALE_INT")) Then
    '            sDatiStampa += "'" & dtStampaRiassuntiva.Rows(i)("COD_FISCALE_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("VIA_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("VIA_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("CIVICO_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("CIVICO_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("CAP_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("CAP_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("COMUNE_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("COMUNE_INT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("PROVINCIA_INT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("PROVINCIA_INT")
    '        End If
    '        'dati utente
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("NUMEROUTENTE")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("NUMEROUTENTE")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("COGNOME_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("COGNOME_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("NOME_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("NOME_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("COD_FISCALE_UT")) Then
    '            sDatiStampa += "'" & dtStampaRiassuntiva.Rows(i)("COD_FISCALE_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("VIA_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("VIA_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("CIVICO_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("CIVICO_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("CAP_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("CAP_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("COMUNE_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("COMUNE_UT")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("PROVINCIA_UT")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("PROVINCIA_UT")
    '        End If
    '        'Dati Ubicazione
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("via_ubicazione")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("via_ubicazione")
    '        End If
    '        sDatiStampa += "|"
    '        sUbicazione = ""
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("civico_ubicazione")) Then
    '            sUbicazione += " " & dtStampaRiassuntiva.Rows(i)("civico_ubicazione")
    '        End If
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("esponente_civico")) And dtStampaRiassuntiva.Rows(i)("esponente_civico") <> "" Then
    '            sUbicazione += "/" + dtStampaRiassuntiva.Rows(i)("esponente_civico")
    '        End If
    '        sDatiStampa += sUbicazione.Trim
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("FOGLIO")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("FOGLIO")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("NUMERO")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("NUMERO")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("SUBALTERNO")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("SUBALTERNO").ToString()
    '        End If
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("DATACESSAZIONE")) Then
    '            If dtStampaRiassuntiva.Rows(i)("DATACESSAZIONE") <> "" Then
    '                stato = "CESSATO"
    '            Else
    '                stato = "NON CESSATO"
    '            End If
    '        Else
    '            stato = "NON CESSATO"
    '        End If
    '        If stato = "NON CESSATO" Then
    '            If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("DATAATTIVAZIONE")) Then
    '                If dtStampaRiassuntiva.Rows(i)("DATAATTIVAZIONE") = "" Then
    '                    stato = "PENDENTE"
    '                Else
    '                    stato = "ATTIVO"
    '                End If
    '            Else
    '                stato = "PENDENTE"
    '            End If
    '        End If
    '        sDatiStampa += "|" & stato
    '        'Tipologia utenza
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("DESCRIZIONE")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("DESCRIZIONE")
    '        End If
    '        sDatiStampa += "|"
    '        If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("NUMEROUTENZE")) Then
    '            sDatiStampa += dtStampaRiassuntiva.Rows(i)("NUMEROUTENZE").ToString()
    '        End If

    '        If AddRowStampa(datatablevuoto, sDatiStampa) = 0 Then
    '            Exit Sub
    '        End If


    '        ''old version

    '        ''Codice Contratto
    '        'datarowvuoto.Item(0) = dtStampaRiassuntiva.Rows(i)("CODICECONTRATTO")
    '        ''Data Sottoscrizione
    '        'datarowvuoto.Item(1) = GiraData(dtStampaRiassuntiva.Rows(i)("DATASOTTOSCRIZIONE"))
    '        ''Cognome Utente
    '        'datarowvuoto.Item(6) = dtStampaRiassuntiva.Rows(i)("COGNOME")
    '        ''Nome Utente
    '        'datarowvuoto.Item(7) = dtStampaRiassuntiva.Rows(i)("NOME")
    '        ''Cognome Intestatario
    '        'datarowvuoto.Item(2) = dtStampaRiassuntiva.Rows(i)("COGNOME2")
    '        ''Nome Intestatario
    '        'datarowvuoto.Item(3) = dtStampaRiassuntiva.Rows(i)("NOME2")
    '        ''Tipologia Utenza
    '        'Dim mySQL As String = "select codcontatore from tp_contatori where codcontratto=" & dtStampaRiassuntiva.Rows(i)("IDCONTRATTO")
    '        'Dim dataReaderContatore As new dataview
    '        'Dim contatoreComodo As Int32
    '        'contatoreComodo = 0
    '        'dataReaderContatore = DBAccess.GetDataReader(mySQL)
    '        'While dataReaderContatore.Read()
    '        '    contatoreComodo = dataReaderContatore("CODCONTATORE")
    '        'End While
    '        'dataReaderContatore.Close()

    '        'mySQL = "SELECT TP_TIPIUTENZA.DESCRIZIONE AS DESCRIZIONE, TP_CONTATORI.NUMEROUTENZE AS NUMBER"
    '        'mySQL = mySQL & " FROM TP_CONTATORI LEFT JOIN"
    '        'mySQL = mySQL & " TP_TIPIUTENZA ON TP_CONTATORI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA"
    '        'mySQL = mySQL & " WHERE TP_CONTATORI.CODCONTATORE = " & CInt(contatoreComodo)
    '        'dataReaderContatore = DBAccess.GetDataReader(mySQL)
    '        'While dataReaderContatore.Read()
    '        '    datarowvuoto.Item(11) = dataReaderContatore("DESCRIZIONE")
    '        '    datarowvuoto.Item(12) = dataReaderContatore("NUMBER")
    '        'End While
    '        'dataReaderContatore.Close()
    '        ''Numero Utenze

    '        ''Numero Utente
    '        'mySQL = "select numeroutente from tp_contatori where codcontatore=" & CInt(contatoreComodo)
    '        'Dim drNUtente As new dataview
    '        'drNUtente = DBAccess.GetDataReader(mySQL)
    '        'datarowvuoto.Item(5) = ""
    '        'While drNUtente.Read()
    '        '    If IsDBNull(drNUtente("numeroutente")) Then
    '        '        datarowvuoto.Item(5) = ""
    '        '    Else
    '        '        datarowvuoto.Item(5) = drNUtente("numeroutente")
    '        '    End If
    '        'End While
    '        'drNUtente.Close()

    '        'mySQL = ""

    '        'Dim drstrada As new dataview
    '        'Dim mioindirizzo As String = ""
    '        'mySQL = "SELECT COD_STRADA,esponente_civico,via_ubicazione, CIVICO_UBICAZIONE FROM TP_CONTATORI WHERE CODCONTRATTO=" & dtStampaRiassuntiva.Rows(i)("IDCONTRATTO")
    '        'drstrada = DBAccess.GetDataReader(mySQL)
    '        'While drstrada.Read()
    '        '    If Not IsDBNull(drstrada("via_ubicazione")) Then
    '        '        mioindirizzo = drstrada("via_ubicazione")
    '        '    End If
    '        '    If Not IsDBNull(drstrada("CIVICO_UBICAZIONE")) Then
    '        '        mioindirizzo = mioindirizzo & " " & drstrada("CIVICO_UBICAZIONE")
    '        '    End If
    '        '    If Not IsDBNull(drstrada("esponente_civico")) Then
    '        '        mioindirizzo = mioindirizzo & " " & drstrada("esponente_civico")
    '        '    End If
    '        'End While
    '        'drstrada.Close()

    '        'datarowvuoto.Item(9) = mioindirizzo



    '        'If Not IsDBNull(dtStampaRiassuntiva.Rows(i)("DATACESSAZIONE")) Then
    '        '    If dtStampaRiassuntiva.Rows(i)("DATACESSAZIONE") <> "" Then
    '        '        stato = "CESSATO"
    '        '    Else
    '        '        stato = "NON CESSATO"
    '        '    End If
    '        'Else
    '        '    stato = "NON CESSATO"
    '        'End If

    '        'If stato = "NON CESSATO" Then
    '        '    dim sSQL as string
    '        '    Dim dataAttiv As String
    '        '    Dim DContatore As new dataview
    '        '    sSQL="SELECT CODCONTATORE FROM TP_CONTATORI WHERE CODCONTRATTO=" & dtStampaRiassuntiva.Rows(i)("CODCONTRATTO")
    '        '    DContatore = DBAccess.GetDataReader(sSQL)
    '        '    Dim Conta As Int32
    '        '    While DContatore.Read()
    '        '        Conta = DContatore("CODCONTATORE")
    '        '    End While
    '        '    DContatore.Close()

    '        '    sSQL="SELECT DATAATTIVAZIONE FROM TP_CONTATORI WHERE CODCONTATORE=" & Conta
    '        '    DContatore = DBAccess.GetDataReader(sSQL)
    '        '    While DContatore.Read()
    '        '        If Not IsDBNull(DContatore("DATAATTIVAZIONE")) Then
    '        '            If DContatore("DATAATTIVAZIONE") = "" Then
    '        '                dataAttiv = "PENDENTE"
    '        '            Else
    '        '                dataAttiv = "ATTIVO"
    '        '            End If

    '        '        Else
    '        '            dataAttiv = "PENDENTE"
    '        '        End If
    '        '    End While
    '        '    DContatore.Close()

    '        '    stato = dataAttiv
    '        'End If

    '        'datarowvuoto.Item(10) = stato

    '        'Dim mioContribuente As Int64
    '        'mioContribuente = dtStampaRiassuntiva.Rows(i)("CONTRIBUENTE_INTESTATARIO")

    '        'Dim drSpedizione As new dataview
    '        'drSpedizione = DEContRATTIPR.GetParametriResidenzaSpedizione(mioContribuente)
    '        ''=======================================
    '        ''INDIRIZZO INTESTATARIO
    '        ''=======================================
    '        'Dim indirizzoIntestatario As String
    '        'indirizzoIntestatario = ""
    '        'If drSpedizione.HasRows Then
    '        '    'POPOLATO
    '        '    While drSpedizione.Read()
    '        '        If Not IsDBNull(drSpedizione("FRAZIONE_RCP")) Then
    '        '            If drSpedizione("FRAZIONE_RCP") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drSpedizione("FRAZIONE_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("VIA_RCP")) Then
    '        '            If drSpedizione("VIA_RCP") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drSpedizione("VIA_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("CIVICO_RCP")) Then
    '        '            If drSpedizione("CIVICO_RCP") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drSpedizione("CIVICO_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("COMUNE_RCP")) Then
    '        '            If drSpedizione("COMUNE_RCP") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drSpedizione("COMUNE_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("CAP_RCP")) Then
    '        '            If drSpedizione("CAP_RCP") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drSpedizione("CAP_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("PROVINCIA_RCP")) Then
    '        '            If drSpedizione("PROVINCIA_RCP") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drSpedizione("PROVINCIA_RCP")
    '        '            End If
    '        '        End If

    '        '    End While
    '        'Else
    '        '    Dim drResidenza As new dataview
    '        '    'NON POPOLATO
    '        '    drResidenza = DEContRATTIPR.getParametriResidenza(mioContribuente)
    '        '    While drResidenza.Read()
    '        '        If Not IsDBNull(drResidenza("FRAZIONE_RES")) Then
    '        '            If drResidenza("FRAZIONE_RES") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drResidenza("FRAZIONE_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("VIA_RES")) Then
    '        '            If drResidenza("VIA_RES") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drResidenza("VIA_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("CIVICO_RES")) Then
    '        '            If drResidenza("CIVICO_RES") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drResidenza("CIVICO_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("COMUNE_RES")) Then
    '        '            If drResidenza("COMUNE_RES") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drResidenza("COMUNE_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("CAP_RES")) Then
    '        '            If drResidenza("CAP_RES") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drResidenza("CAP_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("PROVINCIA_RES")) Then
    '        '            If drResidenza("PROVINCIA_RES") <> "" Then
    '        '                indirizzoIntestatario = indirizzoIntestatario & " " & drResidenza("PROVINCIA_RES")
    '        '            End If
    '        '        End If
    '        '    End While
    '        '    drResidenza.Close()
    '        'End If
    '        'drSpedizione.Close()
    '        'datarowvuoto.Item(4) = indirizzoIntestatario
    '        ''=======================================
    '        ''FINE INDIRIZZO INTESTATARIO
    '        ''=======================================

    '        ''=======================================
    '        ''INDIRIZZO UTENTE
    '        ''=======================================
    '        'mioContribuente = dtStampaRiassuntiva.Rows(i)("CONTRIBUENTE_UTENTE")

    '        'drSpedizione = DEContRATTIPR.GetParametriResidenzaSpedizione(mioContribuente)

    '        'Dim indirizzoUtente As String
    '        'indirizzoUtente = ""
    '        'If drSpedizione.HasRows Then
    '        '    'POPOLATO
    '        '    While drSpedizione.Read()
    '        '        If Not IsDBNull(drSpedizione("FRAZIONE_RCP")) Then
    '        '            If drSpedizione("FRAZIONE_RCP") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drSpedizione("FRAZIONE_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("VIA_RCP")) Then
    '        '            If drSpedizione("VIA_RCP") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drSpedizione("VIA_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("CIVICO_RCP")) Then
    '        '            If drSpedizione("CIVICO_RCP") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drSpedizione("CIVICO_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("COMUNE_RCP")) Then
    '        '            If drSpedizione("COMUNE_RCP") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drSpedizione("COMUNE_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("CAP_RCP")) Then
    '        '            If drSpedizione("CAP_RCP") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drSpedizione("CAP_RCP")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drSpedizione("PROVINCIA_RCP")) Then
    '        '            If drSpedizione("PROVINCIA_RCP") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drSpedizione("PROVINCIA_RCP")
    '        '            End If
    '        '        End If

    '        '    End While
    '        'Else
    '        '    Dim drResidenza As new dataview
    '        '    'NON POPOLATO
    '        '    drResidenza = DEContRATTIPR.getParametriResidenza(mioContribuente)
    '        '    While drResidenza.Read()
    '        '        If Not IsDBNull(drResidenza("FRAZIONE_RES")) Then
    '        '            If drResidenza("FRAZIONE_RES") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drResidenza("FRAZIONE_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("VIA_RES")) Then
    '        '            If drResidenza("VIA_RES") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drResidenza("VIA_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("CIVICO_RES")) Then
    '        '            If drResidenza("CIVICO_RES") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drResidenza("CIVICO_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("COMUNE_RES")) Then
    '        '            If drResidenza("COMUNE_RES") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drResidenza("COMUNE_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("CAP_RES")) Then
    '        '            If drResidenza("CAP_RES") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drResidenza("CAP_RES")
    '        '            End If
    '        '        End If

    '        '        If Not IsDBNull(drResidenza("PROVINCIA_RES")) Then
    '        '            If drResidenza("PROVINCIA_RES") <> "" Then
    '        '                indirizzoUtente = indirizzoUtente & " " & drResidenza("PROVINCIA_RES")
    '        '            End If
    '        '        End If
    '        '    End While
    '        '    drResidenza.Close()
    '        'End If
    '        'drSpedizione.Close()

    '        ''=======================================
    '        ''INDIRIZZO UTENTE
    '        ''=======================================

    '        'datarowvuoto.Item(8) = indirizzoUtente
    '        'datatablevuoto.Rows.Add(datarowvuoto)
    '    Next

    '    Dim nomefile As String = "ELENCOCONTRATTI_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".xls"

    '    If dtStampaRiassuntiva.Rows.Count > 0 Then
    '        Dim objExport As RKLib.ExportData.Export
    '        objExport = New RKLib.ExportData.Export("Web")
    '        objExport.ExportDetails(datatablevuoto, RKLib.ExportData.Export.ExportFormat.Excel, nomefile)
    '    Else
    '        Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non esistono elementi da estrarre per i criteri selezionati');")
    '    End If
    'Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaContratti.StampaExcel_Click.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
    ''' <summary>
    ''' Estrazione file xls con elenco selezione
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
    Private Sub StampaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim FncContatori As New GestContatori
        Dim DtDatiStampa As New DataTable
        Dim dvStampa As DataView
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()

        nCol = 32
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ELENCO_CONTRATTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        'prelevo i dati da stampare
        dvStampa = FncContatori.GetElencoContrattiContatori(ConstSession.StringConnection, ConstSession.IdEnte, "", "", TxtVia.Text, txtNominativoIntestatario.Text, NomeIntestatario.Text, txtNominativoUtente.Text, NomeUtente.Text, cboStato.SelectedIndex, -1, False, -1)
        DtDatiStampa = FncStampa.PrintElencoContratti(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
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
    'Private Sub StampaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StampaExcel.Click
    '    Dim sPathProspetti, sNameXLS, Str As String
    '    Dim x As Integer
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim FncContatori As New GestContatori
    '    Dim DtDatiStampa As New DataTable
    '    Dim dvStampa As DataView
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()

    '    'valorizzo il nome del file
    '    sPathProspetti = ConstSession.PathProspetti
    '    sNameXLS = ConstSession.IdEnte & "_ELENCO_CONTRATTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '    'prelevo i dati da stampare
    '    'dvStampa = FncContatori.GetElencoContrattiContatori(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione, ConstSession.IdEnte, "", "", TxtVia.Text, txtNominativoIntestatario.Text, NomeIntestatario.Text, txtNominativoUtente.Text, NomeUtente.Text, cboStato.SelectedIndex, -1, False)
    '    dvStampa = FncContatori.GetElencoContrattiContatori(ConstSession.IdEnte, "", "", TxtVia.Text, txtNominativoIntestatario.Text, NomeIntestatario.Text, txtNominativoUtente.Text, NomeUtente.Text, cboStato.SelectedIndex, -1, False)
    '    DtDatiStampa = FncStampa.PrintElencoContratti(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
    '    If Not DtDatiStampa Is Nothing Then
    '        'definisco le colonne
    '        aListColonne = New ArrayList
    '        For x = 0 To 32
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '        'definisco l'insieme delle colonne da esportare
    '        Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32}
    '        'esporto i dati in excel
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '        Str = sPathProspetti & sNameXLS
    '    End If
    'End Sub
End Class
