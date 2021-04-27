'Imports System.Xml
'Imports System.Data
'Imports System.Text
Imports System.Data.SqlClient
Imports Utility
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class VecchioContatore
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(VecchioContatore))
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private ContatoreID As Integer
    Private ModDate As New ClsGenerale.Generale
    Private DBContatori As GestContatori = New GestContatori
    Private GestLetture As GestLetture = New GestLetture
    Private clsLetture As New clsLetture
    Private DBAccess As New DBAccess.getDBobject
    Private blnUpdateGrid As Boolean
    Private strCodContatorePrecedente As String
    Private m_lngIDPADRE As Long
    Private strCodUtente, strCodContratto As String
    Private _Const As New Costanti
    Private getListaLetture as objDBListSQL

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtDescrizionePosizione As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnAnnulla As System.Web.UI.WebControls.Button
    Protected WithEvents BTN As System.Web.UI.WebControls.Button

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    dim sSQL as string
    '    Dim DetailContatore As New objContatore
    '    Dim DetailLetture As DetailsLetture
    '    Dim drTipoContatore As SqlDataReader
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        ContatoreID = stringoperation.formatint(request("CODCONTATORE"))

    '        If Not Page.IsPostBack Then
    '            DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)
    '            DetailLetture = GestLetture.GetDetailsLetture(ContatoreID)

    '            txtEnteAppartenenza.Text = ConstSession.DescrizioneEnte
    '            If DetailContatore.nIdImpianto <> -1 Then
    '                txtImpianto.Text = DetailContatore.nIdImpianto
    '            End If
    '            txtUbicazione.Text = DetailContatore.sUbicazione
    '            txtNCivico.Text = DetailContatore.sCivico
    '            txtSequenza.Text = DetailContatore.sSequenza
    '            txtLatoStrada.Text = DetailContatore.sLatoStrada
    '            txtProgressivo.Text = DetailContatore.sProgressivo
    '            txtMatricola.Text = DetailContatore.sMatricola
    '            txtNoteContatore.Text = DetailContatore.sNote
    '            txtDataAttivazione.Text = DetailContatore.sDataAttivazione
    '            txtDataSostituzione.Text = DetailContatore.sDataSostituzione
    '            txtDataRimTemp.Text = DetailContatore.sDataRimTemp
    '            txtDataCessazione.Text = DetailContatore.sDataCessazione
    '            txtNumeroUtenze.Text = DetailContatore.nNumeroUtenze
    '            txtNUtente.Text = DetailContatore.sNumeroUtente
    '            txtTipoUtenza.Text = DetailLetture.TipoUtenza
    '            txtMinFatt.Text = DetailLetture.MinimoFatturabile
    '            txtMinFattRim.Text = DetailLetture.MinFattRim

    '            sSQL="SELECT DESCRIZIONE"
    '            sSQL+= " FROM TP_TIPOCONTATORE"
    '            sSQL+= " WHERE (IDTIPOCONTATORE = " & CInt(DetailContatore.nTipoContatore) & ")"
    '            drTipoContatore = DBAccess.GetDataReader(sSQL)
    '            If drTipoContatore.Read() Then
    '                If Not IsDBNull(drTipoContatore("DESCRIZIONE")) Then
    '                    txtTipoContatore.Text = drTipoContatore("DESCRIZIONE")
    '                End If
    '            End If
    '            drTipoContatore.Close()

    '            sSQL="SELECT DESCRIZIONE"
    '            sSQL+= " FROM TP_POSIZIONECONTATORE"
    '            sSQL+= " WHERE (CODPOSIZIONE = " & CInt(DetailContatore.nPosizione) & ")"
    '            drTipoContatore = DBAccess.GetDataReader(sSQL)
    '            If drTipoContatore.Read() Then
    '                If Not IsDBNull(drTipoContatore("DESCRIZIONE")) Then
    '                    txtPosizioneContatore.Text = drTipoContatore("DESCRIZIONE")
    '                End If
    '            End If
    '            drTipoContatore.Close()

    '            sSQL="SELECT DESCRIZIONE"
    '            sSQL+= " FROM TP_GIRI"
    '            sSQL+= " WHERE (IDGIRO = " & CInt(DetailContatore.nGiro) & ")"
    '            drTipoContatore = DBAccess.GetDataReader(sSQL)
    '            If drTipoContatore.Read Then
    '                If Not IsDBNull(drTipoContatore("DESCRIZIONE")) Then
    '                    txtGiroContatore.Text = drTipoContatore("DESCRIZIONE")
    '                End If
    '            End If
    '            drTipoContatore.Close()
    '            cboTipoContatore.Style.Add("display", "none")
    '            strCodUtente = DetailContatore.nIdUtente
    '        End If

    '        lblContatore.Text = "  " & " Letture associate al Contatore Matricola:  " & DetailContatore.sMatricola & " - Utente:  " & DetailLetture.NomeUtente
    '        'prelevo le letture
    '        getListaLetture = GestLetture.BindData(ContatoreID, DetailContatore.nIdUtente)
    '        grdLetture.DataKeyField = "CODLETTURA"
    '        Select Case getListaLetture.RecordCount
    '            Case 0
    '                info.Text = "Non sono presenti Letture"
    '            Case Is > 0
    '                grdLetture.cnnConn = getListaLetture.oConn
    '                grdLetture.sSQL= getListaLetture.Query
    '                grdLetture.strSqlCountRecord = ""
    '                grdLetture._NumberRecord = getListaLetture.RecordCount
    '                grdLetture.Rows.Count = 0
    '                grdLetture.BindData()
    '        End Select
    '    Catch Err As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DetailContatore As New objContatore
        Dim DetailLetture As DetailsLetture
        Dim drTipoContatore As SqlDataReader
        Dim sSQL As String

        Try
            ContatoreID = stringoperation.formatint(request("CODCONTATORE"))

            If Not Page.IsPostBack Then
                DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, -1)
                DetailLetture = GestLetture.GetDetailsLetture(ContatoreID)

                txtEnteAppartenenza.Text = ConstSession.DescrizioneEnte
                If DetailContatore.nIdImpianto <> -1 Then
                    txtImpianto.Text = DetailContatore.nIdImpianto
                End If
                txtUbicazione.Text = DetailContatore.sUbicazione
                txtNCivico.Text = DetailContatore.sCivico
                txtSequenza.Text = DetailContatore.sSequenza
                txtLatoStrada.Text = DetailContatore.sLatoStrada
                txtProgressivo.Text = DetailContatore.sProgressivo
                txtMatricola.Text = DetailContatore.sMatricola
                txtNoteContatore.Text = DetailContatore.sNote
                txtDataAttivazione.Text = DetailContatore.sDataAttivazione
                txtDataSostituzione.Text = DetailContatore.sDataSostituzione
                txtDataRimTemp.Text = DetailContatore.sDataRimTemp
                txtDataCessazione.Text = DetailContatore.sDataCessazione
                txtNumeroUtenze.Text = DetailContatore.nNumeroUtenze
                txtNUtente.Text = DetailContatore.sNumeroUtente
                txtTipoUtenza.Text = DetailLetture.TipoUtenza
                txtMinFatt.Text = DetailLetture.MinimoFatturabile
                txtMinFattRim.Text = DetailLetture.MinFattRim

                sSQL = "SELECT DESCRIZIONE"
                sSQL += " FROM TP_TIPOCONTATORE"
                sSQL += " WHERE (IDTIPOCONTATORE = " & CInt(DetailContatore.nTipoContatore) & ")"
                drTipoContatore = DBAccess.GetDataReader(sSQL)
                If drTipoContatore.Read() Then
                    If Not IsDBNull(drTipoContatore("DESCRIZIONE")) Then
                        txtTipoContatore.Text = drTipoContatore("DESCRIZIONE")
                    End If
                End If
                drTipoContatore.Close()

                sSQL = "SELECT DESCRIZIONE"
                sSQL += " FROM TP_POSIZIONECONTATORE"
                sSQL += " WHERE (CODPOSIZIONE = " & CInt(DetailContatore.nPosizione) & ")"
                drTipoContatore = DBAccess.GetDataReader(sSQL)
                If drTipoContatore.Read() Then
                    If Not IsDBNull(drTipoContatore("DESCRIZIONE")) Then
                        txtPosizioneContatore.Text = drTipoContatore("DESCRIZIONE")
                    End If
                End If
                drTipoContatore.Close()

                sSQL = "SELECT DESCRIZIONE"
                sSQL += " FROM TP_GIRI"
                sSQL += " WHERE (IDGIRO = " & CInt(DetailContatore.nGiro) & ")"
                drTipoContatore = DBAccess.GetDataReader(sSQL)
                If drTipoContatore.Read Then
                    If Not IsDBNull(drTipoContatore("DESCRIZIONE")) Then
                        txtGiroContatore.Text = drTipoContatore("DESCRIZIONE")
                    End If
                End If
                drTipoContatore.Close()
                cboTipoContatore.Style.Add("display", "none")
                strCodUtente = DetailContatore.nIdUtente
                lblContatore.Text = "  " & " Letture associate al Contatore Matricola:  " & DetailContatore.sMatricola & " - Utente:  " & DetailLetture.NomeUtente
                'prelevo le letture
                Dim dv As DataView = GestLetture.BindData(ContatoreID)
                If Not IsNothing(dv) Then
                    If dv.Count <= 0 Then
                        info.Text = "Non sono presenti Letture"
                        GrdLetture.Visible = False
                    Else
                        GrdLetture.Visible = True
                        GrdLetture.DataSource = dv
                        GrdLetture.DataBind()
                    End If
                End If
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Protected Function CheckStatus(ByVal prdStatus As Object) As String
        If utility.stringoperation.formatbool(prdStatus) = False Then
            Return "No"
        Else
            Return "Si"
        End If
    End Function

    Protected Function DescriAnomalia(ByVal prdStatus As Object) As String
        DescriAnomalia = GestLetture.DescrizioneAnomalie(prdStatus)
        Return DescriAnomalia
    End Function

    Protected Function PopulateDropDownList() As DataSet
        Try
            Dim dataAdapter As SqlDataAdapter  ' sqldatasetcommand
            Dim DS As New DataSet
            Dim sqlConn As New SqlConnection
            Dim sSQL As String
            sqlConn.ConnectionString = ConstSession.StringConnection
            sqlConn.Open()
            sSQL = "SELECT * FROM TP_MODALITALETTURA ORDER BY CODMODALITALETTURA"
            dataAdapter = New SqlDataAdapter(sSQL, sqlConn)
            dataAdapter.SelectCommand.CommandType =
               CommandType.Text

            dataAdapter.Fill(DS, "TP_MODALITALETTURA")  'filldataset

            Return DS
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.PopulateDropDownList.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function

    'Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
    'Try
    '    Dim dt As DataTable = dsTemp.Tables(0)
    '    Dim rowNull As DataRow = dt.NewRow()
    '    rowNull(DataTextField) = "..."
    '    rowNull(DataValueField) = "-1"
    '    dsTemp.Tables(0).Rows.InsertAt(rowNull, 0)

    '    cboTemp.DataSource = dsTemp
    '    cboTemp.DataValueField = DataValueField
    '    cboTemp.DataTextField = DataTextField
    '    cboTemp.DataBind()

    '    If lngSelectedID <> -1 Then
    '        cboTemp.SelectedIndex = lngSelectedID
    '    End If
    ' Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.LoadDropDownList.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try

    'End Sub



    'Protected Sub CaricaCombo(ByVal cbotemp As DropDownList)
    'Try
    '    dim sSQL as string
    '    Dim sqlConn As New SqlConnection

    '    Dim sqlReader As SqlDataReader
    '    sqlConn.ConnectionString = ConstSession.StringConnection
    '    sqlConn.Open()

    '    sSQL="SELECT * FROM TP_MODALITALETTURA ORDER BY CODMODALITALETTURA"
    '    Dim cmd As New SqlCommand(sSQL, sqlConn)
    '    sqlReader = cmd.ExecuteReader()
    '    cbotemp.Items.Add("")
    '    cbotemp.Items(0).Value = -1
    '    While sqlReader.Read()
    '        cbotemp.Items.Add(utility.stringoperation.formatstring(sqlReader.Item("DESCRIZIONE")))
    '        If utility.stringoperation.formatstring(sqlReader.Item("DESCRIZIONE")) = "MANUALE" Then
    '            cbotemp.SelectedIndex = sqlReader.Item("CODMODALITALETTURA")
    '        End If
    '        cbotemp.Items(cbotemp.Items.Count - 1).Value = CStr(sqlReader.Item("CODMODALITALETTURA"))
    '    End While


    '    sqlReader.Close()
    '    sqlConn.Close()
    ' Catch Err As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.CaricaCombo.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    '    Private Sub grdLetture_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdLetture.ItemDataBound

    '        Dim intCell As TableCell
    '        Static lngTemp, lngTemp1 As Long
    '        Dim blnTemp As Boolean = DataBinder.Eval(e.Item.DataItem, "FATTURATA")
    'Try
    '        If e.Item.ItemType = ListItemType.Item Or _
    '          e.Item.ItemType = ListItemType.AlternatingItem Then
    '            If lngTemp = 0 Then
    '                lngTemp = DataBinder.Eval(e.Item.DataItem, "DATALETTURA")
    '            End If

    '            If blnTemp = True Then
    '                'Declare string variable
    '                'Assign the relevant data to a variable 
    '                'To convert the value of Type String to System.Drawing.Color
    '                lngTemp1 = DataBinder.Eval(e.Item.DataItem, "DATALETTURA")
    '                e.Item.BackColor = System.Drawing.Color.MistyRose
    '                e.Item.Cells(9).Visible = False
    '                e.Item.Cells(10).Visible = False
    '            Else
    '                If DataBinder.Eval(e.Item.DataItem, "DATALETTURA") < lngTemp Then
    '                    If DataBinder.Eval(e.Item.DataItem, "DATALETTURA") < lngTemp1 Then
    '                        e.Item.Cells(9).Visible = False
    '                        e.Item.Cells(10).Visible = False
    '                    End If
    '                End If
    '                e.Item.BackColor = System.Drawing.Color.White
    '            End If

    '            Dim btnDelete As LinkButton = CType(e.Item.FindControl("btnDelete"), LinkButton)
    '            btnDelete.Attributes("onClick") = "return(GestAlert('c', 'question', 'Sei sicuro di voler Eliminare la Lettura selezionata !! '))"
    '        End If

    '        If e.Item.ItemType = ListItemType.EditItem Then

    '            Dim btnDelete As LinkButton = CType(e.Item.FindControl("btnDelete"), LinkButton)


    '            Dim txtDataLettura As TextBox = CType(e.Item.FindControl("txtDataLettura"), TextBox)
    '            Dim txtLettura As TextBox = CType(e.Item.FindControl("txtLettura"), TextBox)
    '            Dim txtConsumoEffettivo As TextBox = CType(e.Item.FindControl("txtConsumoEffettivo"), TextBox)

    '            txtDataLettura.Attributes.Add("onblur", "txtDateLostfocus(this);VerificaData(this);")
    '            txtDataLettura.Attributes.Add("onfocus", "txtDateGotfocus(this);")


    '            txtLettura.Attributes.Add("onfocus", "this.select();")
    '            txtLettura.Attributes.Add("onKeyUp", "disableLetterChar(this);")
    '            txtConsumoEffettivo.Attributes.Add("onfocus", "this.select();")
    '            txtConsumoEffettivo.Attributes.Add("onKeyUp", "disableLetterChar(this);")


    '            Dim btnAnnulla As LinkButton = CType(e.Item.FindControl("btnAnnulla"), LinkButton)
    '            btnAnnulla.Attributes("onClick") = "return(GestAlert('c', 'question', 'Annullare le Modifiche?'))"
    '            btnDelete.Visible = False

    '        End If
    '  Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.grdLetture_ItemDataBound.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub

    '#Region "Salvataggio dei dati Della Griglia"

    '    Private Sub grdLetture_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdLetture.UpdateCommand
    '        dim sSQL, _
    '           strDataLettura, _
    '           strLettura, _
    '           strConsumo, _
    '           strFatturazioneSospesa As String
    'Try
    '        Dim jScriptFunction As String
    '        Dim sqlConn As New SqlConnection
    '        Dim sqlComm As New SqlCommand
    '        Dim lngConsumoTeorico, lngLetturaTeorica, lngGiorniDiConsumo, lngConsumoEffettivo As Long
    '        Dim txtDataLettura As TextBox
    '        Dim txtLettura As TextBox
    '        Dim txtConsumoEffettivo As TextBox
    '        Dim chkFatt As CheckBox
    '        Dim intKey As Integer
    '        Dim blnDataValida As Boolean
    '        blnDataValida = False
    '        'Chiave CodLettura e CodContatore 

    '        Dim TempList As DropDownList
    '        Dim strTempValue As String

    '        TempList = e.Item.FindControl("cboModalitaDiLettura")
    '        strTempValue = TempList.SelectedItem.Value



    '        txtDataLettura = CType(e.Item.FindControl("txtDataLettura"), TextBox)
    '        strDataLettura = txtDataLettura.Text
    '        txtLettura = CType(e.Item.FindControl("txtLettura"), TextBox)
    '        strLettura = txtLettura.Text
    '        txtConsumoEffettivo = CType(e.Item.FindControl("txtConsumoEffettivo"), TextBox)

    '        If Not Page.IsClientScriptBlockRegistered("VerificaDatiGriglia") Then

    '            jScriptFunction = "<script language=""JavaScript"">" + vbCrLf + _
    '           "function VerificaDatiGriglia() " + vbCrLf + _
    '           "{" + vbCrLf + _
    '           "if (confirm('Confermare le Modifiche?'))" + vbCrLf + _
    '             "{" + vbCrLf + _
    '           "if(IsBlank(document.frmModifica." & txtDataLettura.ClientID & ".value) )" + vbCrLf + _
    '           "{" + vbCrLf + _
    '             "GestAlert('a', 'warning', '', '', 'Attenzione :Data di Lettura Obbligatoria!');" + vbCrLf + _
    '            "Setfocus(frmModifica." & txtDataLettura.ClientID & ");" + vbCrLf + _
    '            "return false;" + vbCrLf + _
    '           "}" + vbCrLf + _
    '           "if(IsBlank(document.frmModifica." & txtLettura.ClientID & ".value) )" + vbCrLf + _
    '           "{" + vbCrLf + _
    '            "GestAlert('a', 'warning', '', '', 'Attenzione :Lettura Obbligatoria!');" + vbCrLf + _
    '            "Setfocus(frmModifica." & txtLettura.ClientID & ");" + vbCrLf + _
    '            "return false;" + vbCrLf + _
    '           "}" + vbCrLf + _
    '           "if(IsBlank(document.frmModifica." & txtConsumoEffettivo.ClientID & ".value) )" + vbCrLf + _
    '           "{" + vbCrLf + _
    '            "GestAlert('a', 'warning', '', '', 'Attenzione :Consumo Obbligatorio !');" + vbCrLf + _
    '            "Setfocus(frmModifica." & txtConsumoEffettivo.ClientID & ");" + vbCrLf + _
    '            "return false;" + vbCrLf + _
    '           "}" + vbCrLf + _
    '           "return true; " + vbCrLf + _
    '             "}" + vbCrLf + _
    '             "else" + vbCrLf + _
    '            "{" + vbCrLf + _
    '             "Setfocus(frmModifica." & txtDataLettura.ClientID & ");" + vbCrLf + _
    '             "return false;" + vbCrLf + _
    '            "}" + vbCrLf + _
    '             "}" + vbCrLf + _
    '           ""

    '            Page.RegisterClientScriptBlock("VerificaDatiGriglia", jScriptFunction)
    '        End If

    '        clsLetture.ControllaDataLettura(ContatoreID, strDataLettura, lngConsumoTeorico, lngGiorniDiConsumo, lngLetturaTeorica, blnDataValida, strCodUtente, strCodContratto)
    '        If Not blnDataValida = True Then
    '            txtConsumoEffettivo = CType(e.Item.FindControl("txtConsumoEffettivo"), TextBox)
    '            ' strConsumo = txtConsumoEffettivo.Text

    '            chkFatt = CType(e.Item.FindControl("chkFatt"), CheckBox)
    '            If chkFatt.Checked Then
    '                strFatturazioneSospesa = "-1"
    '            Else
    '                strFatturazioneSospesa = "0"
    '            End If
    '            intKey = grdLetture.DataKeys.Item(e.Item.ItemIndex)

    '            'Calcolo del Consumo Effettivo
    '            'Lettura Attuale - Lettura fatturata precedente 
    '            clsLetture.CalcolaConsumoEffettivo(strLettura, lngConsumoEffettivo, _
    '             strCodUtente, ContatoreID)

    '            sSQL=""
    '            sSQL="UPDATE TP_LETTURE SET " & vbCrLf
    '            sSQL+="DATALETTURA = " & ModDate.GiraData(strDataLettura) & "," & vbCrLf
    '            sSQL+="LETTURA = " & strLettura & "," & vbCrLf
    '            sSQL+="CONSUMO = " & CStr(lngConsumoEffettivo) & "," & vbCrLf
    '            sSQL+="CONSUMOTEORICO = " & CStr(lngConsumoTeorico) & "," & vbCrLf
    '            sSQL+="GIORNIDICONSUMO = " & CStr(lngGiorniDiConsumo) & "," & vbCrLf
    '            'FLAG INCONGRUENTE 
    '            If Not VerificaTolleranzaConsumo(lngConsumoEffettivo, lngConsumoTeorico) Then
    '                sSQL+="INCONGRUENTE =1 " & "," & vbCrLf
    '            End If
    '            sSQL+="CODMODALITALETTURA = " & strTempValue & "," & vbCrLf
    '            sSQL+="FATTURAZIONESOSPESA = " & strFatturazioneSospesa & vbCrLf
    '            sSQL+="WHERE" & vbCrLf
    '            sSQL+="TP_LETTURE.CODLETTURA=" & CStr(intKey) & vbCrLf
    '            sSQL+="AND" & vbCrLf
    '            sSQL+="TP_LETTURE.CODCONTATORE=" & ContatoreID

    '            sqlConn.ConnectionString = ConstSession.StringConnection
    '            sqlConn.Open()

    '            sqlComm.CommandText =sSQL
    '            sqlComm.CommandType = CommandType.Text
    '            sqlComm.Connection = sqlConn
    '            sqlComm.ExecuteNonQuery()

    '            sqlConn.Close()
    '            grdLetture.EditItemIndex = -1
    '            'Ricarico la Griglia
    '            AblitaObject(False)
    '            'prelevo le letture
    '            GetListaLetture = GestLetture.BindData(ContatoreID, strCodUtente)
    '            grdLetture.DataKeyField = "CODLETTURA"
    '            Select Case GetListaLetture.RecordCount
    '                Case 0
    '                    info.Text = "Non sono presenti Letture"
    '                Case Is > 0
    '                    grdLetture.Rows.Count = 0
    '                    grdLetture.DataBind()
    '            End Select
    '            blnUpdateGrid = False
    '        Else
    '            Dim strNameFieldData As String
    '            strNameFieldData = txtDataLettura.ClientID.ToString
    '            VerificaData(strNameFieldData)
    '        End If
    'Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.grdLetture_UpdateCommand.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub
    '#End Region

    '#Region "Evento di Modifica Della Griglia"

    '    Private Sub grdLetture_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdLetture.EditCommand
    '        dim sSQL as string

    '        Dim sqlConn As New SqlConnection
    '        Dim sqlComm As New SqlCommand

    '        grdLetture.EditItemIndex = CInt(e.Item.ItemIndex)

    '        'prelevo le letture
    '        GetListaLetture = GestLetture.BindData(ContatoreID, strCodUtente)
    '        grdLetture.DataKeyField = "CODLETTURA"
    '        Select Case GetListaLetture.RecordCount
    '            Case 0
    '                info.Text = "Non sono presenti Letture"
    '            Case Is > 0
    '                grdLetture.Rows.Count = 0
    '                grdLetture.DataBind()
    '        End Select
    '        blnUpdateGrid = True

    '        Dim txtDataLettura As TextBox
    '        Dim txtLettura As TextBox
    '        Dim txtConsumoEffettivo As TextBox
    '        Dim jScriptFunction As String
    '        Dim btnAggiorna As LinkButton
    '        Dim strCodLettura As String
    'Try
    '        btnAggiorna = grdLetture.Items(e.Item.ItemIndex).Cells(9).FindControl("btnAggiorna")
    '        strCodLettura = grdLetture.DataKeys(e.Item.ItemIndex)

    '        txtDataLettura = grdLetture.Items(e.Item.ItemIndex).Cells(2).FindControl("txtDataLettura")
    '        txtLettura = grdLetture.Items(e.Item.ItemIndex).Cells(3).FindControl("txtLettura")
    '        txtConsumoEffettivo = grdLetture.Items(e.Item.ItemIndex).Cells(4).FindControl("txtConsumoEffettivo")


    '        AblitaObject(True)
    '        sSQL="SELECT * FROM TP_LETTURE WHERE CODLETTURA =" & strCodLettura
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        sqlComm.CommandText =sSQL
    '        sqlComm.CommandType = CommandType.Text
    '        sqlComm.Connection = sqlConn

    '        Dim sqlReader As SqlDataReader
    '        sqlReader = sqlComm.ExecuteReader
    '        If sqlReader.Read() Then
    '            txtGiornidiConsumoGrid.Text = utility.stringoperation.formatstring(sqlReader.Item("GIORNIDICONSUMO"))
    '            txtConsumoTeoricoGrid.Text = utility.stringoperation.formatstring(sqlReader.Item("CONSUMOTEORICO"))
    '        End If
    '        sqlReader.Close()
    '        sqlConn.Close()


    '        If Not Page.IsClientScriptBlockRegistered("VerificaDatiGriglia") Then

    '            jScriptFunction = "<script language=""JavaScript"">" + vbCrLf + _
    '           "function VerificaDatiGriglia() " + vbCrLf + _
    '           "{" + vbCrLf + _
    '           "if (confirm('Confermare le Modifiche?'))" + vbCrLf + _
    '             "{" + vbCrLf + _
    '           "if(IsBlank(document.frmModifica." & txtDataLettura.ClientID & ".value) )" + vbCrLf + _
    '           "{" + vbCrLf + _
    '             "GestAlert('a', 'warning', '', '', 'Attenzione :Data di Lettura Obbligatoria!');" + vbCrLf + _
    '            "Setfocus(frmModifica." & txtDataLettura.ClientID & ");" + vbCrLf + _
    '            "return false;" + vbCrLf + _
    '           "}" + vbCrLf + _
    '           "if(IsBlank(document.frmModifica." & txtLettura.ClientID & ".value) )" + vbCrLf + _
    '           "{" + vbCrLf + _
    '            "GestAlert('a', 'warning', '', '', 'Attenzione :Lettura Obbligatoria!');" + vbCrLf + _
    '            "Setfocus(frmModifica." & txtLettura.ClientID & ");" + vbCrLf + _
    '            "return false;" + vbCrLf + _
    '           "}" + vbCrLf + _
    '           "if(IsBlank(document.frmModifica." & txtConsumoEffettivo.ClientID & ".value) )" + vbCrLf + _
    '           "{" + vbCrLf + _
    '            "GestAlert('a', 'warning', '', '', 'Attenzione :Consumo Obbligatorio !');" + vbCrLf + _
    '            "Setfocus(frmModifica." & txtConsumoEffettivo.ClientID & ");" + vbCrLf + _
    '            "return false;" + vbCrLf + _
    '           "}" + vbCrLf + _
    '           "return true; " + vbCrLf + _
    '             "}" + vbCrLf + _
    '             "else" + vbCrLf + _
    '            "{" + vbCrLf + _
    '             "Setfocus(frmModifica." & txtDataLettura.ClientID & ");" + vbCrLf + _
    '             "return false;" + vbCrLf + _
    '            "}" + vbCrLf + _
    '             "}" + vbCrLf + _
    '           ""

    '            Page.RegisterClientScriptBlock("VerificaDatiGriglia", jScriptFunction)
    '            btnAggiorna.Attributes("OnClick") = "return VerificaDatiGriglia();"

    '        End If

    '        'Set the script to focus and select the TextBox
    '        RegisterScript(sscript,me.gettype())"focus", "<script language=""JavaScript"">" & vbCrLf & _
    '           vbTab & "frmModifica." & txtDataLettura.ClientID & ".focus();" & _
    '           vbCrLf & vbTab & "frmModifica." & txtDataLettura.ClientID & ".select();" & _
    '           vbCrLf & "<" & "/script>")
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.grdLetture_EditCommand.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub
    '#End Region

    '    Private Sub grdLetture_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdLetture.CancelCommand
    '        grdLetture.EditItemIndex = -1
    '        blnUpdateGrid = False
    '        AblitaObject(False)
    '        'prelevo le letture
    'Try
    '        GetListaLetture = GestLetture.BindData(ContatoreID, strCodUtente)
    '        grdLetture.DataKeyField = "CODLETTURA"
    '        Select Case GetListaLetture.RecordCount
    '            Case 0
    '                info.Text = "Non sono presenti Letture"
    '            Case Is > 0
    '                grdLetture.Rows.Count = 0
    '                grdLetture.DataBind()
    '        End Select
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.grdLetture_CancelCommand.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub

    '    Private Sub grdLetture_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdLetture.DeleteCommand
    '        dim sSQL as string

    '        Dim sqlConn As New SqlConnection
    '        Dim sqlComm As New SqlCommand

    '        Dim intKey As Integer
    '        intKey = grdLetture.DataKeys.Item(e.Item.ItemIndex)
    '       Try
    '        sSQL=""
    '        sSQL="DELETE  FROM TP_LETTURE  " & vbCrLf
    '        sSQL+="WHERE" & vbCrLf
    '        sSQL+="TP_LETTURE.CODLETTURA=" & CStr(intKey) & vbCrLf

    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        sqlComm.CommandText =sSQL
    '        sqlComm.CommandType = CommandType.Text
    '        sqlComm.Connection = sqlConn
    '        sqlComm.ExecuteNonQuery()

    '        sqlConn.Close()
    '        grdLetture.EditItemIndex = -1
    '        'Ricarico la Griglia
    '        'prelevo le letture
    '        GetListaLetture = GestLetture.BindData(ContatoreID, strCodUtente)
    '        grdLetture.DataKeyField = "CODLETTURA"
    '        Select Case GetListaLetture.RecordCount
    '            Case 0
    '                info.Text = "Non sono presenti Letture"
    '            Case Is > 0
    '                grdLetture.Rows.Count = 0
    '                grdLetture.DataBind()
    '        End Select
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.grdLetture_DeleteCommand.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub


    '#Region "Controlla Coerenza Lettura"

    '    Private Sub CalcolaConsumoEffettivo(ByVal strLettura As String, ByRef lngConsumoEffettivo As Long)

    '        dim sSQL as string
    '        Dim sqlConn As New SqlConnection
    '        Dim sqlComm As New SqlCommand
    '        Dim sqlDataReader As SqlDataReader
    '        Dim intRecordCount As Integer
    '        Dim strGiorniDiConsumo As String
    '        Dim lngValoreFondoScala, _
    '          lngLetturaPrecedente, _
    '         lngTemp As Long
    '        'Ricavo se Esiste L'ultima Lettura Eseguita
    'Try
    '        sSQL="SELECT TOP 1 TP_LETTURE.*  FROM TP_LETTURE " & vbCrLf
    '        sSQL+="WHERE" & vbCrLf
    '        sSQL+="CODUTENTE=" & strCodUtente & vbCrLf
    '        sSQL+="AND" & vbCrLf
    '        sSQL+="STORICIZZATA =0" & vbCrLf

    '        sSQL+="AND" & vbCrLf
    '        sSQL+="CODCONTATORE=" & ContatoreID & vbCrLf
    '        sSQL+="ORDER BY DATALETTURA DESC"

    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        sqlComm.CommandText =sSQL
    '        sqlComm.CommandType = CommandType.Text
    '        sqlComm.Connection = sqlConn

    '        sqlDataReader = sqlComm.ExecuteReader
    '        If sqlDataReader.Read() Then

    '            lngLetturaPrecedente = utility.stringoperation.formatstring(sqlDataReader.Item("LETTURA"))
    '            lngConsumoEffettivo = CLng(strLettura) - CLng(utility.stringoperation.formatstring(sqlDataReader.Item("LETTURA")))
    '            lngTemp = lngConsumoEffettivo
    '            sqlDataReader.Close()
    '            sqlConn.Close()

    '            If lngConsumoEffettivo < 0 Then

    '                'Verifico e considero  il Giro Contatore
    '                sSQL="SELECT TP_TIPOCONTATORE.VALOREFONDOSCALA" & vbCrLf
    '                sSQL+="FROM  TP_CONTATORI INNER JOIN" & vbCrLf
    '                sSQL+="TP_TIPOCONTATORE ON TP_CONTATORI.IDTIPOCONTATORE = TP_TIPOCONTATORE.IDTIPOCONTATORE" & vbCrLf
    '                sSQL+="WHERE TP_CONTATORI.CODCONTATORE = " & ContatoreID
    '                sqlConn.Open()

    '                sqlComm.CommandText =sSQL
    '                sqlComm.CommandType = CommandType.Text
    '                sqlComm.Connection = sqlConn

    '                sqlDataReader = sqlComm.ExecuteReader
    '                sqlDataReader.Read()

    '                lngValoreFondoScala = CLng(utility.stringoperation.formatstring(sqlDataReader.Item("VALOREFONDOSCALA")))
    '                lngTemp = lngValoreFondoScala - lngLetturaPrecedente
    '                lngTemp = lngTemp + CLng(strLettura)

    '                sqlDataReader.Close()
    '                sqlConn.Close()

    '            End If
    '        End If

    '        lngConsumoEffettivo = lngTemp
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.CalcolaConsumoEffettivo.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub

    '#End Region

    '    Private Sub txtSequenza_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSequenza.PreRender
    '        txtSequenza.Attributes.Add("onfocus", "this.select();")
    '        txtSequenza.Attributes.Add("onKeyUp", "disableLetterChar(this);")
    '    End Sub

    '    Private Sub txtProgressivo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProgressivo.PreRender
    '        txtProgressivo.Attributes.Add("onfocus", "this.select();")
    '        txtProgressivo.Attributes.Add("onKeyUp", "disableLetterChar(this);")

    '    End Sub

    '    Private Sub txtNCivico_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNCivico.PreRender
    '        txtNCivico.Attributes.Add("onfocus", "this.select();")
    '        txtNCivico.Attributes.Add("onKeyUp", "disableLetterChar(this);")
    '    End Sub

    '    Private Sub btnConferma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'Try
    '        If blnUpdateGrid Then
    '            RegisterScript(sscript,me.gettype())"Message", "<script language=""JavaScript"">" & vbCrLf & _
    '             vbTab & "GestAlert('a', 'warning', '', '', 'Confermare o Annullare le modifiche alle Letture (Vedi Griglia) prima di Confermare !');" & _
    '             vbCrLf & "<" & "/script>")
    '            Exit Sub
    '        End If
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.btnConferma_Click.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub

    '    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        Dim strHTML As String
    'Try
    '        If blnUpdateGrid Then
    '            RegisterScript(sscript,me.gettype())"Message", "<script language=""JavaScript"">" & vbCrLf & _
    '             vbTab & "GestAlert('a', 'warning', '', '', 'Confermare o Annullare le modifiche alle Letture (Vedi Griglia) prima di Confermare !');" & _
    '             vbCrLf & "<" & "/script>")
    '            Exit Sub
    '        End If
    '        strHTML = "<script>" & vbCrLf
    '        strHTML = strHTML & "parent.Visualizza.location.href='" & ConstSession.pathapplicazione & "/DataEntryLetture/RicercaLetture.aspx" & "';" & vbCrLf
    '        strHTML = strHTML & "parent.Comandi.location.href='" & ConstSession.pathapplicazione & "/DataEntryLetture/ComandiLetture.aspx" & "';"
    '        strHTML = strHTML & ""
    '        Response.Write(strHTML)
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.btnClose_Click.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub

    '    'Determina il confronto fra due date
    '    Private Sub AblitaObject(ByVal blnCondition As Boolean)
    '        lblGiorniDiConsumo.Visible = blnCondition
    '        lblConsumoTeorico.Visible = blnCondition
    '        txtGiornidiConsumoGrid.Visible = blnCondition
    '        txtConsumoTeoricoGrid.Visible = blnCondition
    '    End Sub

    '    Private Function VerificaTolleranzaConsumo(ByRef lngConsumoEffettivo As Long, ByRef lngConsumoTeorico As Long) As Boolean

    '        dim sSQL as string
    '        Dim sqlConn As New SqlConnection
    '        Dim sqlComm As New SqlCommand
    '        Dim sqlDataReader As SqlDataReader
    '        Dim lngSogliaTolleranza As Long
    '        Dim dblConsumoTollerato As Double

    '        VerificaTolleranzaConsumo = False

    '        sSQL="SELECT  TP_TIPIUTENZA.SOGLIA" & vbCrLf
    '        sSQL+="FROM TP_CONTATORI INNER JOIN" & vbCrLf
    '        sSQL+="TP_TIPIUTENZA ON TP_CONTATORI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA" & vbCrLf
    '        sSQL+="WHERE" & vbCrLf
    '        sSQL+="CODCONTATORE=" & ContatoreID
    'Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        sqlComm.CommandText =sSQL
    '        sqlComm.CommandType = CommandType.Text
    '        sqlComm.Connection = sqlConn

    '        sqlDataReader = sqlComm.ExecuteReader
    '        If sqlDataReader.Read() Then
    '            lngSogliaTolleranza = CLng(utility.stringoperation.formatstring(sqlDataReader.Item("SOGLIA")))
    '        End If
    '        sqlDataReader.Close()
    '        sqlConn.Close()
    '        dblConsumoTollerato = (lngConsumoTeorico * lngSogliaTolleranza) / 100
    '        dblConsumoTollerato = FormatNumber(dblConsumoTollerato, 2)
    '        If lngConsumoEffettivo >= (lngConsumoTeorico - dblConsumoTollerato) And lngConsumoEffettivo <= (lngConsumoTeorico + dblConsumoTollerato) Then
    '            VerificaTolleranzaConsumo = True
    '        End If
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.VerificaTolleranzaConsumo.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Function

    ''Determina il confronto fra due date
    'Private Sub VerificaData(ByVal strNameFieldData)
    'Try
    '    'Meesaggio d'Errore PER INCONGRUENZA DATE
    '    RegisterScript(sscript,me.gettype())"ErrorMessage", "<script language=""JavaScript"">" & vbCrLf & _
    '      "GestAlert('a', 'warning', '', '', 'Attenzione:La Data di lettura Inserita è minore della lettura Ultima Fatturata !!');" & _
    '      vbCrLf & vbTab & "frmModifica." & strNameFieldData & ".focus();" & vbCrLf & _
    '      vbCrLf & vbTab & "frmModifica." & strNameFieldData & ".value='';" & vbCrLf & _
    '      "<" & "/script>")
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.VecchioContatore.VerificaData.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
End Class
