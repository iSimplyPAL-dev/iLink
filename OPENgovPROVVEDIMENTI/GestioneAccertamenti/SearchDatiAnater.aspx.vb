Imports System
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports ComPlusInterface
Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports log4net

Partial Class SearchDatiAnater
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchDatiAnater))


    Private workTable As New DataTable
   
    Private dw As DataView
    Private ds1 As DataSet

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim objHashTable As Hashtable
            Dim objRicercaUI As New RicercaUnitaImmobiliareAnater
            Dim oArrayUIanater As OggettoUnitaImmobiliareAnater()

            If Page.IsPostBack = False Then

                objHashTable = Session("HashTableDichiarazioniAccertamentiANATER")

                Dim RemobjectICI As IRemotingInterfaceICI
                Dim TypeOfRI As Type = GetType(IRemotingInterfaceICI)
                RemobjectICI = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLanaterICI"))

                '******************************************************************
                'Cerco tutte le dichiarazioni per l'anno selezionato
                '******************************************************************

                objRicercaUI.Cod_comune_istat = ConstSession.IdEnte

                If CStr(objHashTable("FOGLIO")).CompareTo("") = 0 Then
                    objRicercaUI.Foglio = "-1"
                Else
                    objRicercaUI.Foglio = objHashTable("FOGLIO")
                End If

                If CStr(objHashTable("NUMERO")).CompareTo("") = 0 Then
                    objRicercaUI.Mappale = "-1"
                Else
                    objRicercaUI.Mappale = objHashTable("NUMERO")
                End If

                If CStr(objHashTable("SUBALTERNO")).CompareTo("") = 0 Then
                    objRicercaUI.Subalterno = "-1"
                Else
                    objRicercaUI.Subalterno = objHashTable("SUBALTERNO")
                End If

                If CStr(objHashTable("CODICERICERCA")).CompareTo("") = 0 Then
                    objRicercaUI.CodiceRicerca = "-1"
                Else
                    objRicercaUI.CodiceRicerca = objHashTable("CODICERICERCA")
                End If

                oArrayUIanater = RemobjectICI.GetUnitaImmobiliariICI(objRicercaUI)

                'Salvo il data set delle dichiarazioni
                'Rifacciamo la query anche se è da rivedere (trovare soluzione
                'ottimale per evitare di rifare ogni volta la query)


                'carico i dati in griglia

                Session.Remove("ArrayUIanater")
                Session.Add("ArrayUIanater", oArrayUIanater)

                If oArrayUIanater.Length > 0 Then
                    GrdAnagrafica.DataSource = oArrayUIanater
                    GrdAnagrafica.DataBind()

                    GrdAnagrafica.Visible = True
                    lblMessage.Text = ""

                Else

                    lblMessage.Text = "Non sono stati trovati Immobili"
                    GrdAnagrafica.Visible = False

                End If

            Else
                ControllaCheckbox()
                ' eseguo il cambio di pagina se la pagina ha fatto il postback
                GrdAnagrafica.DataSource = CType(Session("ArrayUIanater"), OggettoUnitaImmobiliareAnater())
                GrdAnagrafica.DataBind()
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Response.Write("<script language='javascript'>")
            Response.Write("</script>")

        End Try
    End Sub

    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim oArrayUI() As OggettoUnitaImmobiliareAnater
        Dim x As Integer

        Try
            oArrayUI = CType(Session("ArrayUIanater"), OggettoUnitaImmobiliareAnater())
            For Each itemGrid In GrdAnagrafica.Rows
                'prendo l'IDriga da aggiornare
                For x = 0 To oArrayUI.GetUpperBound(0)
                    If oArrayUI(x).IDProgressivo = CType(itemGrid.Cells(19).Text, Long) Then
                        oArrayUI(x).Selezionato = CType(itemGrid.FindControl("chkSeleziona"), CheckBox).Checked
                        Exit For
                    End If
                Next
            Next
            Session("ArrayUIanater") = oArrayUI

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.ControllaChechbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    Private Function maxLegame(ByVal campo As String) As Integer
        Dim workTable As DataTable
        Try
            If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
            End If
            If workTable.DefaultView.Count = 0 Then
                maxLegame = 0
            Else
                Dim dw As DataView
            dw = workTable.DefaultView
            dw.Sort = "idLegame desc"
            maxLegame = dw.Item(0).Item("idLegame")
        End If

            Return maxLegame

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.maxLegame.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function

    Private Sub creaDataTableImmobili(ByVal objUI As OggettoUnitaImmobiliareAnater())
        Dim workTable As New DataTable("IMMOBILI")
        Try
            If Session("DataTableImmobiliDaAccertare") Is Nothing Then
                workTable.Columns.Add("DAL")
                workTable.Columns.Add("AL")
                workTable.Columns.Add("FOGLIO")
                workTable.Columns.Add("NUMERO")
                workTable.Columns.Add("SUBALTERNO")
                workTable.Columns.Add("CATEGORIA")
                workTable.Columns.Add("CLASSE")
                workTable.Columns.Add("CONSISTENZA")
                workTable.Columns.Add("TR")
                workTable.Columns.Add("RENDITA_VALORE")
                workTable.Columns.Add("IDSANZIONI")
                workTable.Columns.Add("IDLEGAME")
                workTable.Columns.Add("ICICALCOLATO")
                workTable.Columns.Add("PROGRESSIVO")
                workTable.Columns.Add("SANZIONI")
                workTable.Columns.Add("INTERESSI")
                workTable.Columns.Add("PERCPOSSESSO")
                workTable.Columns.Add("TITPOSSESSO")
                workTable.Columns.Add("FLAG_PRINCIPALE")
                workTable.Columns.Add("FLAG_PERTINENZA")
                workTable.Columns.Add("FLAG_RIDOTTO")
                workTable.Columns.Add("IDIMMOBILEPERTINENZA")
                workTable.Columns.Add("SEZIONE")
                workTable.Columns.Add("INDIRIZZO")
                workTable.Columns.Add("CODTITPOSSESSO")
                workTable.Columns.Add("CODTIPORENDITA")
                workTable.Columns.Add("ICICALCOLATOACCONTO")
                workTable.Columns.Add("ICICALCOLATOSALDO")
                workTable.Columns.Add("NUMEROUTILIZZATORI")
                workTable.Columns.Add("ID_IMMOBILE_ORIGINALE_DICHIARATO")
                workTable.Columns.Add("CODICE_VIA")
                workTable.Columns.Add("CALCOLA_INTERESSI")

                Session("DataTableImmobiliDaAccertare") = workTable
            Else
                workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
            End If
            'chkRibalta = e.Item.DataItem.row.itemarray(1).FindControl("chkSeleziona")
            Dim i As Integer = 0

            Dim idLegame As Integer
            Dim idProgressivo As Integer

            idLegame = maxLegame("idLegame") + 1
            idProgressivo = maxLegame("Progressivo") + 1

            For i = 0 To objUI.Length - 1
                If objUI(i).Selezionato = True Then

                    Dim rowsArray() As DataRow
                    rowsArray = workTable.Select("FOGLIO='" & objUI(i).UniFoglio & "'" _
                     & "AND NUMERO='" & objUI(i).UniNumMapp & "'" _
                     & "AND SUBALTERNO='" & objUI(i).UniSubalterno & "'")
                    'Controllo se ho già inserito l'immobile nella varibile di sessione degli immobili.
                    'Se ne ho associato uno nuovo lo aggiungo altrimento non modifico la Session
                    'dove compariribbero gli stessi immobili piu volte
                    If UBound(rowsArray) < 0 Then

                        Dim tempRow As DataRow
                        tempRow = workTable.NewRow()
                        tempRow(0) = objUI(i).UniDataInizio
                        tempRow(1) = objUI(i).UniDataFine
                        tempRow(2) = objUI(i).UniFoglio
                        tempRow(3) = objUI(i).UniNumMapp
                        tempRow(4) = objUI(i).UniSubalterno
                        tempRow(5) = FormattaCategoria(objUI(i).UniCategCatastale)
                        tempRow(6) = objUI(i).UniClasseCatastale
                        tempRow(7) = objUI(i).UniConsistenza
                        tempRow(8) = objUI(i).TipoRendita

                        '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
                        Dim ValoreDich As Double = 0
                        If objUI(i).UniValoreCatastale > 0 Then
                            ValoreDich = objUI(i).UniValoreCatastale
                        End If

                        Dim Valore As Double
                        'Valore = objDBProvv.CalcoloValoredaRendita(objUI(i).UniRenditaCatastale, objUI(i).TipoRendita, objUI(i).UniCategCatastale, objHashTable("ANNOACCERTAMENTO"))
                        '*** 20120530 - IMU ***
                        Dim FncValore As New ComPlusInterface.FncICI
                        Valore = FncValore.CalcoloValore(ConstSession.DBType, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(objUI(i).UniDataInizio), objUI(i).TipoRendita, objUI(i).UniCategCatastale, objUI(i).UniClasseCatastale, "", ValoreDich, objUI(i).UniRenditaCatastale, objUI(i).UniConsistenza, CDate(objUI(i).UniDataInizio), False)
                        If Valore <= 0 Then
                            Valore = objUI(i).UniValoreCatastale
                        End If
                        '*** ***
                        tempRow(9) = Valore 'objUI(i).UniRenditaCatastale 'UniValoreCatastale

                        'Se ho -1 non ho applicato nessuna sanzione
                        tempRow(10) = "-1"
                        tempRow(11) = idLegame
                        tempRow(12) = "-1"
                        tempRow(13) = idProgressivo
                        tempRow(14) = -1
                        tempRow(15) = -1

                        tempRow(16) = 0 '"" 'ds1.Tables(0).Rows(i).Item("PERCENTUALE_POSSESSO")
                        tempRow(17) = "" 'ds1.Tables(0).Rows(i).Item("COD_TIPO_POSSESSO")
                        'If ds1.Tables(0).Rows(i).Item("FLAG_PRINCIPALE") = "True" Then
                        '    tempRow(18) = "1"
                        'Else
                        tempRow(18) = "0"
                        'End If

                        'If ds1.Tables(0).Rows(i).Item("FLAG_PERTINENZA") = "True" Then
                        '    tempRow(19) = "1"
                        'Else
                        tempRow(19) = "0"
                        'End If

                        'If ds1.Tables(0).Rows(i).Item("FLAG_RIDOTTO") = "True" Then
                        '    tempRow(20) = "1"
                        'Else
                        tempRow(20) = "0"
                        'End If

                        tempRow(21) = ""
                        tempRow(22) = ""
                        tempRow(23) = objUI(i).UniDescrizioneVia & " " & objUI(i).UniNumeroCiv    '"INDIRIZZO"
                        tempRow(24) = "0"
                        tempRow(25) = getIDrendita(objUI(i).TipoRendita)
                        tempRow(26) = ""
                        tempRow(27) = ""
                        tempRow(28) = "0" 'numero utilizzatori
                        tempRow(29) = "-1" 'ID IMMOBILE ORIGINALE DICHIARATO
                        tempRow(30) = objUI(i).UniCodVia
                        tempRow(31) = False


                        workTable.Rows.Add(tempRow)
                        idLegame = idLegame + 1
                        idProgressivo = idProgressivo + 1
                        workTable.DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME"
                        Session("DataTableImmobiliDaAccertare") = workTable

                    Else
                        ''Alep 25102007 commentato per mantenere gli immobili precedentemente selezionati
                        ''If rowsArray(0).Item("IDLEGAME") <> idLegame Then
                        ''    rowsArray(0).Item("IDLEGAME") = idLegame
                        ''    idLegame = idLegame + 1
                        ''    rowsArray(0).AcceptChanges()
                        ''    workTable.AcceptChanges()
                        ''    workTable.DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME"
                        ''    Session("DataTableImmobili") = workTable
                        ''End If
                    End If
                    ''Alep 25102007 commentato per mantenere gli immobili precedentemente selezionati
                    ''Else

                    ''    'Se Trovo l'immobile vuol dire che l'ho deckeccato e lo rimouvo dalla 
                    ''    'workTable
                    ''    Dim rowsArray() As DataRow
                    ''    'GIULIA 17082005
                    ''    Dim objUtility As New myUtility
                    ''    rowsArray = workTable.Select("FOGLIO='" & objUI(i).UniFoglio & "'" _
                    ''     & "AND NUMERO='" & objUI(i).UniNumMapp & "'" _
                    ''     & "AND SUBALTERNO='" & objUI(i).UniSubalterno & "'")

                    ''    If UBound(rowsArray) >= 0 Then
                    ''        workTable.Rows.Remove(rowsArray(0))
                    ''        workTable.AcceptChanges()
                    ''        workTable.DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME"
                    ''        Session("DataTableImmobili") = workTable
                    ''    End If
                End If
            Next

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.creaDataTableImmobili.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    Private Function getIDrendita(ByVal strRendita As String) As String
        getIDrendita = ""
        Try
            Select Case strRendita.ToUpper

                Case "RE"
                    getIDrendita = "1"
                Case "RP"
                    getIDrendita = "2"
                Case "RPM"
                    getIDrendita = "3"
                Case "LC"
                    getIDrendita = "4"
                Case "AF"
                    getIDrendita = "5"
                Case "TA"
                    getIDrendita = "6"
                Case Else
                    getIDrendita = "0"

            End Select

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.getIDrendita.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function

    Protected Function FormatDataEmpty(ByVal objtemp As DateTime) As String

        Try
            If objtemp = Date.MinValue Or objtemp = Date.MaxValue Then
            Return ""
        Else
            Return objtemp
        End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.FormatDataEmpty.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function

    Protected Function FormattaIndirizzo(ByVal objVia As Object, ByVal objCivico As Object) As String

        Dim strOutput As String = String.Empty
        Try
            If Not IsDBNull(objVia) Then
            If (objVia) <> "" Then
                strOutput = objVia
            End If
        End If
        If Not IsDBNull(objCivico) Then
            If CStr(objCivico) <> "" Then
                strOutput = strOutput & " " & objCivico
            End If
        End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.FormattaIndirizzo.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strOutput
    End Function

    Protected Function FormattaCategoria(ByVal objtemp As Object) As String

        Dim strOutput As String = String.Empty
        Try
            If Not IsDBNull(objtemp) Then
            If (objtemp) <> "" Then
                strOutput = Left(objtemp, 1) & "/" & Right(objtemp, Len(objtemp) - 1)
            End If
        End If


        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.FormattaCategoria.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strOutput
    End Function
    Protected Function annoBarra(ByVal objtemp As Object) As String
        Dim clsGeneralFunction As New MyUtility
        Dim strTemp As String = ""
        Try
            If Not IsDBNull(objtemp) Then
            strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
        End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAnater.annoBarra.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strTemp
    End Function

    Private Sub btnAssocia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssocia.Click

        Dim oArrayUI As OggettoUnitaImmobiliareAnater()
        oArrayUI = CType(Session("ArrayUIanater"), OggettoUnitaImmobiliareAnater())
        creaDataTableImmobili(oArrayUI)

        dim sScript as string=""
        sscript+= "parent.parent.opener.document.getElementById('loadGridAccertato').src='grdAccertato.aspx';" & vbCrLf
        sscript+= "parent.parent.window.close();"
        RegisterScript(sScript , Me.GetType())


    End Sub
End Class
