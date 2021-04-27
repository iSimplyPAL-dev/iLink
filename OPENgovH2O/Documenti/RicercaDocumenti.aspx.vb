Imports RIBESElaborazioneDocumentiInterface
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports OPENUtility
Imports System.Data.SqlClient
Imports log4net
Imports ElaborazioneDatiStampeInterface

Partial Class RicercaDocumenti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaDocumenti))
    Private iDB As New DBAccess.getDBobject
    Private Anno As String = ""
    Private Data As String = ""
    Private DataApprovazione As String = ""
    Private TipoRuolo As String = ""
    Private strscript As String = ""
    Private ClsFatturazione As New ClsFatture
    Private oArrayDocDaElaborare() As OggettoDocumentiElaborati
    Private oArrayOggettoDocumentiElaborati() As OggettoDocumentiElaborati
    Private nDocDaElab, nDocElab As Integer
    Private FncDoc As New ClsElaborazioneDocumenti

    Delegate Sub StampaMassivaH2OAsync(ByVal ConnessioneUTENZE As String, ByVal ConnessioneRepository As String, ByVal NomedbAnag As String, ByVal NomedbUtenze As String, ByVal sTipoElab As String, ByVal oAnagDoc() As ObjAnagDocumenti, ByVal nDocDaElaborare As Long, ByVal nDocElaborati As Long, ByVal OrdinamentoDoc As Integer, ByVal idFlussoRuolo As Integer, ByVal NomeEnte As String, ByVal CodiceEnte As String, ByVal nDocPerFile As Integer)

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
        Dim ElaborazioneInCorso As Integer
        Dim oMyRuolo As New ObjTotRuoloFatture
        Dim sScript As String = ""

        Try
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloH2O")
            lblElaborazioniEffettuate.Text = ""
            If Page.IsPostBack = False Then
                txtIdRuolo.Text = oMyRuolo.IdFlusso
                If Request.Item("TipoRuolo") <> "" Then
                    TipoRuolo = Request.Item("TipoRuolo")
                End If
                lblAnnoRuolo.Text = Year(CType(Session("oRuoloH2O"), ObjTotRuoloFatture).tDataEmissioneFattura)
                lblDataCartellazione.Text = oMyRuolo.tDataEmissioneFattura

                FncDoc.GetNDoc(ConstSession.IdEnte, oMyRuolo.IdFlusso, nDocElab, nDocDaElab)
                lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " + nDocElab.ToString
                lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  " + nDocDaElab.ToString
                Session("nDocDaElaborare") = nDocDaElab

                ElaborazioneInCorso = FncDoc.InElaborazione(ConstSession.StringConnectionOPENgov, Now.Year.ToString(), ConstSession.IdEnte, ConstSession.CodTributo, "", oMyRuolo.IdFlusso)
                If ElaborazioneInCorso = 1 Then
                    lblElaborazioniEffettuate.Text = "Elaborazione in corso..."
                    ' visualizzo le pagine di elaborazione in corso
                    sScript = "parent.Comandi.location.href='ComandiVuoto.aspx';"
                    sScript += "document.location.href='StampaInCorso.aspx';"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                ElseIf ElaborazioneInCorso = 0 And nDocDaElab = 0 Then
                    lblElaborazioniEffettuate.Text = "Elaborazione Terminata con successo."
                ElseIf ElaborazioneInCorso = 2 Then
                    lblElaborazioniEffettuate.Text = "Elaborazione Terminata con errori."
                End If
            End If
            If Utility.StringOperation.FormatInt(Session("nDocDaElaborare")) = 0 Then
                sScript += "$('.BottoneCreaFile', parent.frames['Comandi'].document).removeClass('DisableBtn');"
            Else
                sScript += "$('.BottoneCreaFile', parent.frames['Comandi'].document).addClass('DisableBtn');"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdElaborazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaborazione.Click
        Dim sScript, sTypeOrd, sNameModello As String
        Dim nTipoElab As Integer = -1
        Dim FncDoc As New ClsElaborazioneDocumenti
        Dim nReturn, nMaxDocPerFile As Integer
        Dim oMyRuolo As New ObjTotRuoloFatture
        Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
        Dim bCreaPDF As Boolean = False

        Try
            oListDocStampati = Nothing
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloH2O")

            sScript = "Search();"
            RegisterScript(sScript, Me.GetType())
            '********************************************************
            'testo se sto facendo delle prove oppure un elaborazione effettiva
            '********************************************************
            If Session("ElaboraDocumenti") = 1 Then
                nTipoElab = 1
            ElseIf Session("ElaboraDocumenti") = 2 Then
                nTipoElab = 0
                '****************************************************
                'stiamo effettuando l'elaborazione effettiva
                'controllo se ci sono ancora dei doc da elaborare
                '****************************************************
                If Session("nDocDaElaborare") = 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'I documenti sono gia\' stati tutti elaborati in effettivo!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
            End If

            '********************************************************
            'elaborazione prove dei documenti
            '********************************************************
            'verifico gli elementi selezionati all'interno della griglia
            '********************************************************
            'se l'operatore non ha selezionato alcun elemento ----->do un messaggio
            '********************************************************
            If nTipoElab = 1 And IsNothing(Session("ListFatture")) Then
                sScript = "GestAlert('a', 'warning', '', '', 'Non sono stati selezionati documenti da elaborare!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            If nTipoElab <> -1 Then
                If Not Session("ListFatture") Is Nothing Then
                    lblElaborazioniEffettuate.Text = "Elaborazione in corso..."
                    Session.Remove("ELENCO_DOCUMENTI_STAMPATI")
                    If Session("OrdinamentoDoc") = 0 Then
                        sTypeOrd = "Indirizzo"
                    Else
                        sTypeOrd = "Nominativo"
                    End If

                    sNameModello = "Modello_Fattura_Acquedotto"

                    If Not Session("nMaxDocPerFile") Is Nothing Then
                        nMaxDocPerFile = Integer.Parse(Session("nMaxDocPerFile"))
                    Else
                        nMaxDocPerFile = ConfigurationManager.AppSettings("NDocPerFile")
                    End If

                    Try
                        Dim nDecimal As Integer = 2
                        Dim TipoStampaBollettini As String = "BOLLETTINISTANDARD"
                        If Session("ElaboraBollettini") = False Then
                            TipoStampaBollettini = ""
                        End If
                        '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
                        Dim bSendByMail As Boolean = False
                        If Not Session("bSendByMail") Is Nothing Then
                            bSendByMail = Session("bSendByMail")
                        End If
                        'nReturn = FncDoc.ElaboraDocumenti(ConstSession.IdEnte, oMyRuolo.IdFlusso, ConstSession.DescrPeriodo.Substring(0, 4), nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), Session("ListFatture"), oListDocStampati, bCreaPDF, nDecimal, TipoStampaBollettini, Session("tipobollettino"), bSendByMail)
                        nReturn = FncDoc.ElaboraDocumenti(ConstSession.CodTributo, ConstSession.IdEnte, oMyRuolo.IdFlusso, ConstSession.DescrPeriodo.Substring(0, 4), ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), Session("ListFatture"), oListDocStampati, bCreaPDF, nDecimal, TipoStampaBollettini, Session("tipobollettino"), bSendByMail)
                        '*** ***
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.CmdElaborazione_Click.errore: ", Err)
                        Response.Redirect("../../PaginaErrore.aspx")
                    End Try

                    If Not oListDocStampati Is Nothing Then
                        Session.Add("ELENCO_DOCUMENTI_STAMPATI", oListDocStampati)
                    End If

                    If nReturn = 0 Then
                        '********************************************************
                        'si è verificato uin errore
                        '********************************************************
                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in estrazione fatture elettroniche!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    ElseIf nReturn = 2 Then
                        '********************************************************
                        ' ho chiamato l'elaborazione asincrona, sostituisco le pagine
                        ' dei comandi e quella principale
                        '********************************************************
                        sScript = ""
                        sScript += "parent.Comandi.location.href='ComandiDocumentiElaborati.aspx';"
                        sScript += "document.location.href='StampaInCorso.aspx';"
                        sScript += ""
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Non ci sono documenti da elaborare!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
                Session("ListFatture") = Nothing

                sScript = "parent.Visualizza.location.href='ViewDocumentiElaborati.aspx';"
                sScript += "parent.Comandi.location.href='ComandiDocumentiElaborati.aspx';"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Effettuare una ricerca!');"
                RegisterScript(sScript, Me.GetType())
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.CmdElaborazione_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub ChiamaElaborazioneAsincrona(ByVal ConnessioneUTENZE As String, ByVal ConnessioneRepository As String, ByVal NomedbAnag As String, ByVal NomedbUtenze As String, ByVal sTipoElab As String, ByVal oAnagDoc() As ObjAnagDocumenti, ByVal nDocDaElaborare As Long, ByVal nDocElaborati As Long, ByVal OrdinamentoDoc As Integer, ByVal idFlussoRuolo As Integer, ByVal NomeEnte As String, ByVal CodiceEnte As String, ByVal nDocPerFile As Integer)
    '    Try
    '        ' faccio partire l'elaborazione asincrona
    '        ' chiamo il servizio di elaborazione delle stampe massive.
    '        Dim arrayretStampaDocumenti As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL() = Nothing
    '        Dim remObject As IElaborazioneStampeH2O = Activator.GetObject(GetType(IElaborazioneStampeH2O), ConfigurationManager.AppSettings("URLElaborazioneDatiStampeUTENZE").ToString())
    '        '***in produzione***
    '        arrayretStampaDocumenti = remObject.ElaborazioneMassivaUTENZE(ConnessioneUTENZE, ConnessioneRepository, NomedbAnag, NomedbUtenze, sTipoElab, oAnagDoc, nDocDaElaborare, nDocElaborati, OrdinamentoDoc, idFlussoRuolo, NomeEnte, CodiceEnte, nDocPerFile, True, False)

    '    Catch ex As Exception
    '                  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.ChiamaElaborazioneAsincrona.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        Throw ex
    '    End Try
    'End Sub

    Private Sub CmdApprovaDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdApprovaDoc.Click
        Dim Ndoc As Integer = 0
        Dim oMyRuolo As New ObjTotRuoloFatture
        Dim sScript As String = ""
        Try
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloH2O")
            '********************************************************
            'è necessario controllare se ci sono ancora delle cartelle per 
            'le quali ci sono ancora dei documenti da elaorare
            '********************************************************
            'Ndoc = clsElabDoc.GetNumFileDocDaElaborare(oMyRuolo.IdFlusso, ConstSession.IdEnte)
            If Not Session("nDocDaElaborare") Is Nothing Then
                Ndoc = CInt(Session("nDocDaElaborare"))
            End If
            If Ndoc > 0 Then
                sScript = "GestAlert('a', 'warning', '', '', 'L\'elaborazione del ruolo selezionato non è ancora terminata!Ci sono ancora " & Ndoc & " documenti da elaborare!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If
            '********************************************************
            'se i documenti sono stati tutti elaborati
            'chiedere conferma prima di procedere
            'se l'operatore conferma
            'inserire la data di elaborazione doc nella tabella 
            'TBLRUOLI_GENERATI
            'spostare i record da TBLGUIDA_COMUNICO
            'a TBLGUIDA_COMUNICO_STORICO
            If New ClsRuoloH2O().SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 5, "I") = 0 Then
                'si è verificato un errore
                sScript = "GestAlert('a', 'warning', '', '', 'Errore in approvazione documenti!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If

            Page_Load(sender, e)

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.CmdApprovaDoc_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdEliminaDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEliminaDoc.Click
        '********************************************************
        'un'elaborazione si può eliminare solo se non è stata data l'approvazione 
        '********************************************************
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            Dim oMyRuolo As New ObjTotRuoloFatture
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloH2O")

            If FncDoc.DeleteTabGuidaComunico(ConstSession.IdEnte, "TBLGUIDA_COMUNICO", oMyRuolo.IdFlusso, cmdMyCommand) = 0 Then
                Log.Debug("Si è verificato un errore in RicercaDoc::CmdEliminaDoc_Click::errore in DeleteTabGuidaComunico")
                Response.Redirect("../../PaginaErrore.aspx")
                Exit Sub
            End If
            If FncDoc.DeleteTabGuidaComunico(ConstSession.IdEnte, "TBLDOCUMENTI_ELABORATI", oMyRuolo.IdFlusso, cmdMyCommand) = 0 Then
                Log.Debug("Si è verificato un errore in RicercaDoc::CmdEliminaDoc_Click::errore in DeleteTabFilesComunicoElab")
                Response.Redirect("../../PaginaErrore.aspx")
                Exit Sub
            End If

            Page_Load(sender, e)

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.CmdEliminaDoc_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub

    Private Function InElaborazione(ByVal iIdFlusso As Integer, ByVal COD_ENTE As String, ByVal COD_TRIBUTO As String, ByRef sNOTE As String, ByRef sDataElab As String) As Integer
        Dim SelectCommand As New SqlCommand
        Dim objDR As SqlDataReader
        Dim bELABORAZIONE As Boolean = False
        Dim bERRORI As Boolean = False
        Dim ModData As New ClsGenerale.Generale

        Try
            '_oDbManagerRepository = New DBManager(ConnessioneRepository)
            SelectCommand.CommandText = "SELECT * FROM TP_TASK_REPOSITORY"
            SelectCommand.CommandText = SelectCommand.CommandText & " WHERE COD_ENTE=@COD_ENTE"
            SelectCommand.CommandText = SelectCommand.CommandText & " AND COD_TRIBUTO=@COD_TRIBUTO"
            SelectCommand.CommandText = SelectCommand.CommandText & " AND IDFLUSSORUOLO=@IDFLUSSO "
            SelectCommand.CommandText = SelectCommand.CommandText & " ORDER BY ID_TASK_REPOSITORY DESC ,DATA_ELABORAZIONE DESC"

            SelectCommand.Parameters.Add(New SqlParameter("@COD_ENTE", SqlDbType.NVarChar)).Value = COD_ENTE
            SelectCommand.Parameters.Add(New SqlParameter("@COD_TRIBUTO", SqlDbType.NVarChar)).Value = COD_TRIBUTO
            SelectCommand.Parameters.Add(New SqlParameter("@IdFlusso ", SqlDbType.Int)).Value = iIdFlusso

            'objDR = _oDbManagerRepository.GetDataReader(SelectCommand)

            objDR = idb.GetDataReader(SelectCommand)

            If objDR.HasRows Then

                objDR.Read()
                bELABORAZIONE = Boolean.Parse(objDR("ELABORAZIONE").ToString())
                bERRORI = Boolean.Parse(objDR("ERRORI").ToString())
                sDataElab = ModData.GiraDataFromDB(objDR("DATA_ELABORAZIONE").ToString())

                If objDR("NOTE") Is System.DBNull.Value Then
                    sNOTE = ""
                Else
                    sNOTE = objDR("NOTE").ToString()
                End If
            Else
                Return 3
            End If
            objDR.Close()
            If bELABORAZIONE = False Then

                If bERRORI = True Then
                    Return 2
                Else
                    Return 0
                End If
            Else
                Return 1
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.InElaborazione.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            sNOTE = ex.Message
            Return -1

        End Try

    End Function

    Private Function ControlloElementiSelezionati() As Integer
        Dim oArrayOggetto() As ObjAnagDocumenti
        Dim x, nSel As Integer

        Try
            nSel = 0
            oArrayOggetto = CType(Session("ListFatture"), ObjAnagDocumenti())
            For x = 0 To oArrayOggetto.GetUpperBound(0)
                If oArrayOggetto(x).Selezionato = True Then
                    nSel += 1
                End If
            Next

            Return nSel
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.ControlloElementiSelezionati.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function

    Private Sub CmdExpFat_Click(sender As Object, e As EventArgs) Handles CmdExpFat.Click
        Dim sScript As String
        Dim oMyRuolo As New ObjTotRuoloFatture

        Try
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloH2O")
            If Not oMyRuolo Is Nothing Then
                If New clsFatturaElettronica(oMyRuolo.IdFlusso, ConstSession.IdEnte, ConstSession.UserName).CreaXMLFatture() Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Estrazione fatture elettroniche effettuata con successo!');"
                    lblElaborazioniEffettuate.Text = "Estrazione fatture elettroniche effettuata con successo!"
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Errore in estrazione fatture elettroniche!');"
                    lblElaborazioniEffettuate.Text = "Errore in estrazione fatture elettroniche!"
                End If
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RicercaDocumenti.CmdExpFat_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Visible = False
        End Try
    End Sub
End Class

Public Class ObjVisualRuolo
    Private _IdFlussoRuolo As Integer = -1
    Private _TipoRuolo As String = ""
    Private _AnnoRuolo As String = ""
    Private _DataCartellazione As String = Date.MaxValue

    Public Property DataCartellazione() As Date
        Get
            Return _DataCartellazione
        End Get
        Set(ByVal Value As Date)
            _DataCartellazione = Value
        End Set
    End Property
    Public Property sAnno() As String
        Get
            Return _AnnoRuolo
        End Get
        Set(ByVal Value As String)
            _AnnoRuolo = Value
        End Set
    End Property

    Public Property sTiporuolo() As String
        Get
            Return _TipoRuolo
        End Get
        Set(ByVal Value As String)
            _TipoRuolo = Value
        End Set
    End Property

    Public Property IdFlussoRuolo() As Integer
        Get
            Return _IdFlussoRuolo
        End Get
        Set(ByVal Value As Integer)
            _IdFlussoRuolo = Value
        End Set
    End Property


End Class
