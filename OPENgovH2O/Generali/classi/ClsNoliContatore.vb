Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
'Imports System.Data.OleDb
'Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Public Class ClsNoliContatore
    Private Shared Log As ILog = LogManager.GetLogger("ClsNoliContatore")
    Private iDB As New DBAccess.getDBobject
    Dim clsGenerale As New ClsGenerale.Generale

    'Public Function GetNoliContatoreEnte(ByVal obj As OggettoNoloContatore) As OggettoNoloContatore()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Dim DsDati As New DataSet
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        dim sSQL as string

    '        sSQL = "SELECT TP_NOLO.ANNO, TP_TIPOCONTATORE.DESCRIZIONE, TP_NOLO.IMPORTO, TP_NOLO.ALIQUOTA, TP_NOLO.ID_TIPO_CONTATORE, TP_NOLO.ID, TP_NOLO.IDENTE, TP_NOLO.ISUNATANTUM"
    '        sSQL += " FROM TP_NOLO"
    '        sSQL += " INNER JOIN TP_TIPOCONTATORE ON TP_NOLO.ID_TIPO_CONTATORE = TP_TIPOCONTATORE.IDTIPOCONTATORE"
    '        sSQL += " WHERE TP_NOLO.IDENTE='" & ConstSession.IdEnte & "'"
    '        If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
    '            sSQL += " AND TP_NOLO.ID=" & obj.ID & ""
    '        End If
    '        If obj.idTipoContatore.CompareTo(-1) <> 0 And obj.idTipoContatore.CompareTo(0) <> 0 Then
    '            sSQL += " AND TP_TIPOCONTATORE.IDTIPOCONTATORE=" & obj.idTipoContatore & ""
    '            sSQL += " AND (TP_NOLO.ISUNATANTUM=" & CInt(obj.bIsUnaTantum) & ")"
    '        End If
    '        If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
    '            sSQL += " AND TP_NOLO.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
    '        End If
    '        'eseguo la query
    '        Dim dsCanoniEnte As DataSet
    '        Log.Debug("GetNoliContatoreEnte::SQL::" & sSQL)
    '        dsCanoniEnte = WFSessione.oSession.oAppDB.GetPrivateDataSet(sSQL)
    '        If dsCanoniEnte.Tables(0).Rows.Count > 0 Then
    '            Dim oDatiNoloContatore As OggettoNoloContatore
    '            Dim arrayListoDatiRata As New ArrayList
    '            Dim iCount As Integer
    '            For iCount = 0 To dsCanoniEnte.Tables(0).Rows.Count - 1
    '                oDatiNoloContatore = New OggettoNoloContatore

    '                oDatiNoloContatore.ID = dsCanoniEnte.Tables(0).Rows(iCount)("ID")
    '                oDatiNoloContatore.idTipoContatore = dsCanoniEnte.Tables(0).Rows(iCount)("ID_TIPO_CONTATORE")
    '                oDatiNoloContatore.sDescrizioneTContatore = dsCanoniEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
    '                oDatiNoloContatore.dAliquota = dsCanoniEnte.Tables(0).Rows(iCount)("ALIQUOTA")
    '                oDatiNoloContatore.dImporto = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTO")
    '                oDatiNoloContatore.sAnno = dsCanoniEnte.Tables(0).Rows(iCount)("ANNO")
    '                oDatiNoloContatore.sIdEnte = dsCanoniEnte.Tables(0).Rows(iCount)("IDENTE")
    '                If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("ISUNATANTUM")) Then
    '                    oDatiNoloContatore.bIsUnaTantum = CBool(dsCanoniEnte.Tables(0).Rows(iCount)("ISUNATANTUM"))
    '                End If
    '                arrayListoDatiRata.Add(oDatiNoloContatore)
    '            Next
    '            Return CType(arrayListoDatiRata.ToArray(GetType(OggettoNoloContatore)), OggettoNoloContatore())
    '        End If

    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.GetNoliContatoreEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di GetNoliContatoreEnte " + ex.Message)
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function DeleteNoliContatoreEnte(ByVal OBJ As OggettoNoloContatore, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim NrataOld, DescrizioneOld, DataScadenzaOld As String

    '    Try
    '        DeleteNoliContatoreEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        dim sSQL as string

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE IL NOLO NON SIA STATO ASSOCIATO
    '        Dim DsNoliContatore As DataSet
    '        DsNoliContatore = GetNoliContatoreAssociati(OBJ)

    '        If DsNoliContatore.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            DeleteNoliContatoreEnte = False
    '            Exit Function
    '        End If

    '        'SE NON è STATA ASSOCIATA LA CANCELLO
    '        sSQL = "DELETE FROM TP_NOLO"
    '        sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""
    '        'eseguo la query
    '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(sSQL))

    '        DeleteNoliContatoreEnte = True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.DeleteNoliContatoreEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di DeleteNoliContatoreEnte " + ex.Message)
    '        DeleteNoliContatoreEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function UpdateNoliContatoreEnte(ByVal objOld As OggettoNoloContatore, ByVal objNew As OggettoNoloContatore, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Try
    '        UpdateNoliContatoreEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        dim sSQL as string

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE IL NOLO NON SIA STATO ASSOCIATO
    '        Dim DsNoliContatore As DataSet
    '        DsNoliContatore = GetNoliContatoreAssociati(objOld)

    '        If DsNoliContatore.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            UpdateNoliContatoreEnte = False
    '            Exit Function
    '        End If

    '        'PRELEVO I DATI DEL NoloContatore DA MODIFICARE
    '        Dim ds As OggettoNoloContatore()
    '        ds = GetNoliContatoreEnte(objOld)
    '        If Not ds Is Nothing Then
    '            If ds.Length > 0 Then
    '                objOld.ID = ds(0).ID
    '                objOld.idTipoContatore = ds(0).idTipoContatore
    '                objOld.dImporto = ds(0).dImporto
    '                objOld.sAnno = ds(0).sAnno
    '                objOld.dAliquota = ds(0).dAliquota
    '                objOld.bIsUnaTantum = ds(0).bIsUnaTantum
    '            Else
    '                strError = "Non sono presenti dati per il Nolo Contatore Selezionato."
    '                UpdateNoliContatoreEnte = False
    '                Exit Function
    '            End If
    '        End If

    '        If (objNew.dImporto <> objOld.dImporto Or objNew.dAliquota <> objOld.dAliquota Or objOld.sAnno <> objNew.sAnno Or objOld.idTipoContatore <> objNew.idTipoContatore Or objOld.bIsUnaTantum <> objNew.bIsUnaTantum) Then
    '            'verifico se è già presente un NoloContatore con data fine validita valorizzata per i dati inseriti
    '            'se è presente blocco l'operazione
    '            ds = GetNoliContatoreEnte(objNew)
    '            If Not ds Is Nothing Then
    '                If ds.Length > 0 Then
    '                    strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '                    UpdateNoliContatoreEnte = False
    '                    Exit Function
    '                End If
    '            End If

    '            'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '            sSQL = "UPDATE TP_NOLO SET IMPORTO=" & objNew.dImporto.ToString.Replace(",", ".") & ","
    '            sSQL += " ANNO='" & objNew.sAnno & "',"
    '            sSQL += " ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '            sSQL += " ISUNATANTUM=" & CInt(objNew.bIsUnaTantum)
    '            sSQL += " WHERE ID=" & objNew.ID & ""
    '            sSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(sSQL))
    '        End If

    '        UpdateNoliContatoreEnte = True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.UpdateNoliContatoreEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di UpdateNoliContatoreEnte " + ex.Message)
    '        UpdateNoliContatoreEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function SetNoliContatoreEnte(ByVal obj As OggettoNoloContatore, ByRef strError As String) As Boolean
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer

    '    Try
    '        SetNoliContatoreEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim ds As OggettoNoloContatore()
    '        ds = GetNoliContatoreEnte(obj)
    '        If Not ds Is Nothing Then
    '            If ds.Length > 0 Then
    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
    '                SetNoliContatoreEnte = False
    '                Exit Function
    '            End If
    '        End If

    '        dim sSQL as string
    '        Dim DsCat As DataSet

    '        sSQL = "INSERT INTO TP_NOLO"
    '        sSQL += " (IDENTE, ID_TIPO_CONTATORE, ANNO, IMPORTO, ALIQUOTA, ISUNATANTUM)"
    '        sSQL += " VALUES('" & ConstSession.IdEnte & "'," & obj.idTipoContatore & ",'" & obj.sAnno.Replace("'", "''") & "'," & obj.dImporto.ToString.Replace(",", ".") & "," & obj.dAliquota.ToString.Replace(",", ".") & "," & CInt(obj.bIsUnaTantum) & ")"
    '        'eseguo la query
    '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(sSQL))
    '        SetNoliContatoreEnte = True
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.SetNoliContatoreEnte.errore: ", ex) 
    '        Throw New Exception("Problemi nell'esecuzione di SetNoliContatoreEnte " + ex.Message)
    '        SetNoliContatoreEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function GetNoliContatoreAssociati(ByVal obj As OggettoNoloContatore) As DataSet
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Dim DsDati As New DataSet
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        dim sSQL as string

    '        sSQL = "SELECT *"
    '        sSQL += " FROM TP_FATTURE_NOTE_NOLO"
    '        sSQL += " INNER JOIN TP_FATTURE_NOTE ON TP_FATTURE_NOTE.IDFATTURANOTA=TP_FATTURE_NOTE_NOLO.IDFATTURANOTA"
    '        sSQL += " WHERE (TP_FATTURE_NOTE_NOLO.IDENTE='" & ConstSession.IdEnte & "')"
    '        sSQL += " AND (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL)"
    '        If obj.ID <> 0 Then
    '            sSQL += " AND (TP_FATTURE_NOTE_NOLO.ID_NOLO=" & obj.ID & ")"
    '        End If

    '        'eseguo la query
    '        Dim dsNoliContatoreAssociati As DataSet
    '        dsNoliContatoreAssociati = WFSessione.oSession.oAppDB.GetPrivateDataSet(sSQL)
    '        Return dsNoliContatoreAssociati

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.GetNoliContatoreAssociati.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di GetNoliContatoreAssociati " + ex.Message)
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function


    ''' <summary>
    ''' Estrae i noli configurati per l'Ente in esame.
    ''' Può estrarre tutti i noli oppure i noli in base ad un anno, ad un ID o/e a un tipo di contatore
    ''' </summary>
    ''' <param name="obj">un oggetto di tipo OggettoNoloContatore</param>
    ''' <returns>un array di oggetti di tipo OggettoNoloContatore</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetNoliContatoreEnte(ByVal obj As OggettoNoloContatore) As OggettoNoloContatore()
        Dim oDatiNoloContatore As OggettoNoloContatore
        Dim arrayListoDatiRata As New ArrayList
        Dim iCount As Integer
        Dim sSQL As String

        Try
            sSQL = "SELECT TP_NOLO.ANNO, TP_TIPOCONTATORE.DESCRIZIONE, TP_NOLO.IMPORTO, TP_NOLO.ALIQUOTA, TP_NOLO.ID_TIPO_CONTATORE, TP_NOLO.ID, TP_NOLO.IDENTE, TP_NOLO.ISUNATANTUM"
            sSQL += " FROM TP_NOLO"
            sSQL += " INNER JOIN TP_TIPOCONTATORE ON TP_NOLO.ID_TIPO_CONTATORE = TP_TIPOCONTATORE.IDTIPOCONTATORE"
            sSQL += " WHERE TP_NOLO.IDENTE='" & ConstSession.IdEnte & "'"
            If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
                sSQL += " AND TP_NOLO.ID=" & obj.ID & ""
            End If
            If obj.idTipoContatore.CompareTo(-1) <> 0 And obj.idTipoContatore.CompareTo(0) <> 0 Then
                sSQL += " AND TP_TIPOCONTATORE.IDTIPOCONTATORE=" & obj.idTipoContatore & ""
                sSQL += " AND (TP_NOLO.ISUNATANTUM=" & CInt(obj.bIsUnaTantum) & ")"
            End If
            If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
                sSQL += " AND TP_NOLO.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
            End If
            'eseguo la query
            Dim dsCanoniEnte As DataSet
            Log.Debug("GetNoliContatoreEnte::SQL::" & sSQL)
            dsCanoniEnte = iDB.GetDataSet(sSQL)
            If dsCanoniEnte.Tables(0).Rows.Count > 0 Then
                For iCount = 0 To dsCanoniEnte.Tables(0).Rows.Count - 1
                    oDatiNoloContatore = New OggettoNoloContatore

                    oDatiNoloContatore.ID = dsCanoniEnte.Tables(0).Rows(iCount)("ID")
                    oDatiNoloContatore.idTipoContatore = dsCanoniEnte.Tables(0).Rows(iCount)("ID_TIPO_CONTATORE")
                    oDatiNoloContatore.sDescrizioneTContatore = dsCanoniEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
                    oDatiNoloContatore.dAliquota = dsCanoniEnte.Tables(0).Rows(iCount)("ALIQUOTA")
                    oDatiNoloContatore.dImporto = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTO")
                    oDatiNoloContatore.sAnno = dsCanoniEnte.Tables(0).Rows(iCount)("ANNO")
                    oDatiNoloContatore.sIdEnte = dsCanoniEnte.Tables(0).Rows(iCount)("IDENTE")
                    If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("ISUNATANTUM")) Then
                        oDatiNoloContatore.bIsUnaTantum = CBool(dsCanoniEnte.Tables(0).Rows(iCount)("ISUNATANTUM"))
                    End If
                    arrayListoDatiRata.Add(oDatiNoloContatore)
                Next
            End If
            Return CType(arrayListoDatiRata.ToArray(GetType(OggettoNoloContatore)), OggettoNoloContatore())
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.GetNoliContatoreEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetNoliContatoreEnte " + ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Verifica se un nolo condifurato è già utilizzato per il calcolo delle fatture.
    ''' </summary>
    ''' <param name="obj">un oggetto di tipo OggettoNoloContatore</param>
    ''' <returns>Dataset</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetNoliContatoreAssociati(ByVal obj As OggettoNoloContatore) As DataSet
        Dim DsDati As New DataSet

        Try

            Dim sSQL As String

            sSQL = "SELECT *"
            sSQL += " FROM TP_FATTURE_NOTE_NOLO"
            sSQL += " INNER JOIN TP_FATTURE_NOTE ON TP_FATTURE_NOTE.IDFATTURANOTA=TP_FATTURE_NOTE_NOLO.IDFATTURANOTA"
            sSQL += " WHERE (TP_FATTURE_NOTE_NOLO.IDENTE='" & ConstSession.IdEnte & "')"
            sSQL += " AND (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL)"
            If obj.ID <> 0 Then
                sSQL += " AND (TP_FATTURE_NOTE_NOLO.ID_NOLO=" & obj.ID & ")"
            End If

            'eseguo la query
            Dim dsNoliContatoreAssociati As DataSet
            dsNoliContatoreAssociati = iDB.GetDataSet(sSQL)
            Return dsNoliContatoreAssociati

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.GetNoliContatoreAssociati.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetNoliContatoreAssociati " + ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Inserisce un nolo.
    ''' </summary>
    ''' <param name="obj">un oggetto di tipo OggettoNoloContatore</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function SetNoliContatoreEnte(ByVal obj As OggettoNoloContatore, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Dim sSQL As String

        Try
            SetNoliContatoreEnte = False

            Dim ds As OggettoNoloContatore()
            ds = GetNoliContatoreEnte(obj)
            If Not ds Is Nothing Then
                If ds.Length > 0 Then
                    strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                    SetNoliContatoreEnte = False
                    Exit Function
                End If
            End If

            sSQL = "INSERT INTO TP_NOLO"
            sSQL += " (IDENTE, ID_TIPO_CONTATORE, ANNO, IMPORTO, ALIQUOTA, ISUNATANTUM)"
            sSQL += " VALUES('" & ConstSession.IdEnte & "'," & obj.idTipoContatore & ",'" & obj.sAnno.Replace("'", "''") & "'," & obj.dImporto.ToString.Replace(",", ".") & "," & obj.dAliquota.ToString.Replace(",", ".") & "," & CInt(obj.bIsUnaTantum) & ")"
            'eseguo la query
            myIdentity = CInt(iDB.ExecuteNonQuery(sSQL))
            SetNoliContatoreEnte = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.SetNoliContatoreEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di SetNoliContatoreEnte " + ex.Message)
            SetNoliContatoreEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Esegue l'aggiornamento di un determinato nolo configurato già a sistema SOLO nel caso
    ''' in cui non è ancora utilizzato dalla fatturazione.
    ''' </summary>
    ''' <param name="objOld">oggetto di tipo OggettoNoloContatore precedente</param>
    ''' <param name="objNew">oggetto di tipo OggettoNoloContatore attuale</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function UpdateNoliContatoreEnte(ByVal objOld As OggettoNoloContatore, ByVal objNew As OggettoNoloContatore, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try
            UpdateNoliContatoreEnte = False

            Dim sSQL As String

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE IL NOLO NON SIA STATO ASSOCIATO
            Dim DsNoliContatore As DataSet
            DsNoliContatore = GetNoliContatoreAssociati(objOld)

            If DsNoliContatore.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                UpdateNoliContatoreEnte = False
                Exit Function
            End If

            'PRELEVO I DATI DEL NoloContatore DA MODIFICARE
            Dim ds As OggettoNoloContatore()
            ds = GetNoliContatoreEnte(objOld)
            If Not ds Is Nothing Then
                If ds.Length > 0 Then
                    objOld.ID = ds(0).ID
                    objOld.idTipoContatore = ds(0).idTipoContatore
                    objOld.dImporto = ds(0).dImporto
                    objOld.sAnno = ds(0).sAnno
                    objOld.dAliquota = ds(0).dAliquota
                    objOld.bIsUnaTantum = ds(0).bIsUnaTantum
                Else
                    strError = "Non sono presenti dati per il Nolo Contatore Selezionato."
                    UpdateNoliContatoreEnte = False
                    Exit Function
                End If
            End If

            If (objNew.dImporto <> objOld.dImporto Or objNew.dAliquota <> objOld.dAliquota Or objOld.sAnno <> objNew.sAnno Or objOld.idTipoContatore <> objNew.idTipoContatore Or objOld.bIsUnaTantum <> objNew.bIsUnaTantum) Then
                'verifico se è già presente un NoloContatore con data fine validita valorizzata per i dati inseriti
                'se è presente blocco l'operazione
                ds = GetNoliContatoreEnte(objNew)
                If Not ds Is Nothing Then
                    If ds.Length > 0 Then
                        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
                        UpdateNoliContatoreEnte = False
                        Exit Function
                    End If
                End If

                'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
                sSQL = "UPDATE TP_NOLO SET IMPORTO=" & objNew.dImporto.ToString.Replace(",", ".") & ","
                sSQL += " ANNO='" & objNew.sAnno & "',"
                sSQL += " ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
                sSQL += " ISUNATANTUM=" & CInt(objNew.bIsUnaTantum)
                sSQL += " WHERE ID=" & objNew.ID & ""
                sSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
                'eseguo la query
                myIdentity = CInt(iDB.ExecuteNonQuery(sSQL))
            End If

            UpdateNoliContatoreEnte = True
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.UpdateNoliContatoreEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di UpdateNoliContatoreEnte " + ex.Message)
            UpdateNoliContatoreEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Esegue l'eliminazione di un determinato nolo configurato già a sistema SOLO nel caso
    ''' in cui non è ancora utilizzato dalla fatturazione.
    ''' </summary>
    ''' <param name="OBJ">oggetto di tipo OggettoNoloContatore</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function DeleteNoliContatoreEnte(ByVal OBJ As OggettoNoloContatore, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try
            DeleteNoliContatoreEnte = False

            Dim sSQL As String

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE IL NOLO NON SIA STATO ASSOCIATO
            Dim DsNoliContatore As DataSet
            DsNoliContatore = GetNoliContatoreAssociati(OBJ)

            If DsNoliContatore.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                DeleteNoliContatoreEnte = False
                Exit Function
            End If

            'SE NON è STATA ASSOCIATA LA CANCELLO
            sSQL = "DELETE FROM TP_NOLO"
            sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""
            'eseguo la query
            myIdentity = CInt(iDB.ExecuteNonQuery(sSQL))

            DeleteNoliContatoreEnte = True
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.DeleteNoliContatoreEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeleteNoliContatoreEnte " + ex.Message)
            DeleteNoliContatoreEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Estrae le voci dei tipi contatori configurati a livello generale e carica un oggetto di tipo DropDownList(combo).
    ''' </summary>
    ''' <param name="ddlTipoContatore">DropDownList</param>
    ''' <remarks>
    ''' </remarks>
    Public Sub LoadComboTipoContatore(ByVal ddlTipoContatore As DropDownList)
        Try
            Dim sSQL As String

            sSQL = "SELECT DESCRIZIONE AS DESCRIZIONE, IDTIPOCONTATORE"
            sSQL += " FROM TP_TIPOCONTATORE"
            'eseguo la query
            clsGenerale.LoadComboGenerale(ddlTipoContatore, sSQL)
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsNoliContatore.LoadComboTipoContatore.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di LoadComboTipoContatore " + ex.Message)
        End Try
    End Sub
End Class
