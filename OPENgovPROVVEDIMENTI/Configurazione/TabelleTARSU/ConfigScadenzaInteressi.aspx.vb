Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione della scadenza da cui far partire gli interessi TARI.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigScadenzaInteressi
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigScadenzaInteressi))
    Private ANNO, DATA, NOTE, Operazione As String
    Dim ModDate As New ModificaDate

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
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Dim blnResult As Boolean = False
        Dim objHashTable As Hashtable = New Hashtable

        Try
            Operazione = Trim(txtOperazione.Text)
            ANNO = Trim(ddlAnno.SelectedValue)
            DATA = Trim(txtData.Text)
            NOTE = Trim(txtNote.Text)

            If Not Page.IsPostBack Then
                CaricaGriglia(False)

                PopolaComboAnno()
            End If

            sScript += "parent.Comandi.location.href='CScadenzaInteressi.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.PageLoad.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="prdStatus"></param>
    ''' <returns></returns>
    Protected Function GiraData(ByVal prdStatus As Object) As String
        Dim objUtility As New MyUtility
        GiraData = ModDate.GiraDataFromDB(objUtility.CToStr(prdStatus))
        Return GiraData
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Pulisci"></param>
    ''' <param name="npage"></param>
    Sub CaricaGriglia(ByVal Pulisci As Boolean, Optional ByVal npage As Integer? = 0)
        Dim dw As DataView
        Dim objDSTipiInteressi As DataSet
        Dim objHashTable As Hashtable = New Hashtable
        Try

            'carico la hash table
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("ANNO", "")
            objHashTable.Add("DATA", "")
            objHashTable.Add("NOTE", "")
            objHashTable.Add("CODTRIBUTO", Utility.Costanti.TRIBUTO_TARSU)

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipiInteressi = objCOMTipoVoci.GetScadenzaInteressi(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            If Not objDSTipiInteressi Is Nothing Then
                dw = objDSTipiInteressi.Tables(0).DefaultView
            End If
            If Page.IsPostBack = False Or Pulisci Then
                GrdInteressi.DataSource = dw
                GrdInteressi.DataBind()
                If Pulisci Then
                    GrdInteressi.SelectedIndex = -1
                End If
            Else
                dw = objDSTipiInteressi.Tables(0).DefaultView
                GrdInteressi.DataSource = dw
                If npage.HasValue Then
                    GrdInteressi.PageIndex = npage.Value
                End If
                GrdInteressi.DataBind()
            End If
            Select Case CInt(GrdInteressi.Rows.Count)
                Case 0
                    GrdInteressi.Visible = False
                    lblMessage.Text = "Nessuna Data Scadenza trovata"
                    lblMessage.Visible = True
                Case Is > 0
                    GrdInteressi.Visible = True
                    lblMessage.Visible = False
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.CaricaGriglia.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

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
                For Each myRow As GridViewRow In GrdInteressi.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        ANNO = myRow.Cells(0).Text()

                        DATA = CType(myRow.FindControl("hfDATA_SCADENZA"), HiddenField).Value

                        DATA = FormattaDataGrd(DATA)
                        NOTE = replaceSpace(myRow.Cells(3).Text())
                        Session("dataold") = DATA
                        Session("annoold") = ANNO

                        ddlAnno.SelectedValue = ANNO
                        txtData.Text = DATA
                        txtNote.Text = NOTE
                        txtOperazione.Text = "M"
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        CaricaGriglia(False, e.NewPageIndex)
    End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="valore"></param>
    ''' <returns></returns>
    Function replaceSpace(ByVal valore As String) As String
        Return valore.Replace("&nbsp;", "")
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tDataGrd"></param>
    ''' <returns></returns>
    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        Try
            If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
                Return ""
            Else
                Return tDataGrd.ToShortDateString.ToString
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.FormattaDataGrd.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    Sub PopolaComboAnno()
        Dim UtilProvv As New MyUtility
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
        Try
            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

            'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
            ddlAnno.Items.Clear()
            UtilProvv.FillDropDownSQLSingleString(ddlAnno, objGestOPENgovProvvedimenti.GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, Utility.Costanti.TRIBUTO_TARSU), -1, "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.PopolaComboAnno.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Dim objHashTable As Hashtable = New Hashtable

        Dim sScript As String = ""
        Try
            If DATA.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'La data Data Scadenza è obbligatoria');"
                RegisterScript(sScript, Me.GetType())
            ElseIf ANNO.CompareTo("-1") = 0 And ANNO.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare l\'anno');"
                RegisterScript(sScript, Me.GetType())
            Else
                If Operazione.CompareTo("M") = 0 Then
                    'Modifica
                    objHashTable.Add("INSUP", "U")
                Else
                    'Nuovo inserimento
                    objHashTable.Add("INSUP", "I")
                End If

                'carico la hash table
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("USER", ConstSession.UserName)
                objHashTable.Add("CODENTE", ConstSession.IdEnte)
                objHashTable.Add("ANNO", ANNO)
                objHashTable.Add("CODTRIBUTO", Utility.Costanti.TRIBUTO_TARSU)
                objHashTable.Add("DATA", DATA)

                'Dim objCOMValoriVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
                'objCOMValoriVoci.DelScadenzaInteressi(objHashTable)

                PulisciCampi()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.btnElimina_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PulisciCampi()
        ddlAnno.SelectedValue = -1
        txtData.Text = ""
        txtNote.Text = ""
        txtOperazione.Text = ""
        Session("dataold") = Nothing : Session("annoold") = Nothing
        RicaricaPagina()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub RicaricaPagina()
        Dim sScript As String = ""
        sScript += "RicaricaPagina();" & vbCrLf
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        PulisciCampi()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim objHashTable As Hashtable = New Hashtable
        Dim sScript As String = ""
        Dim EmptyAL As Boolean = False
        Dim objCOMTipiInteressi As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
        Dim oUtility As New DLL.ProvvUtility

        Try

            If DATA.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'La data Data Scadenza è obbligatoria');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If
            If ANNO.CompareTo("") = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'L\'Anno è obbligatorio');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If
            'carico la hash table
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("ANNO", ANNO)
            objHashTable.Add("ANNOOLD", Session("annoold"))
            objHashTable.Add("DATA", "")
            objHashTable.Add("DATAOLD", ModDate.GiraData(Session("dataold")))
            objHashTable.Add("NOTE", "")
            objHashTable.Add("CODTRIBUTO", Utility.Costanti.TRIBUTO_TARSU)
            objHashTable.Add("CODTRIBUTOOLD", Session("tributoold"))

            If Operazione.CompareTo("M") = 0 Then
                'Modifica
                objHashTable.Add("INSUP", "U")
            Else
                'Nuovo inserimento
                objHashTable.Add("INSUP", "I")
            End If
            objHashTable("DATA") = ModDate.GiraData(DATA)
            objHashTable("NOTE") = NOTE

            objCOMTipiInteressi.SetScadenzaInteressi(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            PulisciCampi()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ConfigScadenzaInteressi.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
