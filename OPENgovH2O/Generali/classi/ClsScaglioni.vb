Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Public Class ClsScaglioni
	Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsScaglioni))
    Private iDB As New DBAccess.getDBobject

    'Public Function GetScaglioniEnte(ByVal obj As OggettoScaglione, Optional ByVal operazione As String = "") As OggettoScaglione()
    '	Dim culture As IFormatProvider
    '	culture = New System.Globalization.CultureInfo("it-IT", True)
    '	System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '	Dim DsDati As New DataSet
    '	Dim WFErrore As String
    '	Dim WFSessione As CreateSessione

    '	Try

    '		'inizializzo la connessione
    '           WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '           If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '               Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '           End If

    '           Dim SQL As String

    '           SQL = "SELECT TP_SCAGLIONI.ANNO, TP_TIPIUTENZA.DESCRIZIONE, TP_SCAGLIONI.DA, TP_SCAGLIONI.A, TP_SCAGLIONI.TARIFFA, TP_SCAGLIONI.MINIMO, "
    '           SSQL+=" TP_SCAGLIONI.ALIQUOTA, TP_SCAGLIONI.ID_TIPO_UTENZA, TP_SCAGLIONI.ID, TP_SCAGLIONI.IDENTE"
    '           SSQL+=" FROM TP_SCAGLIONI INNER JOIN TP_TIPIUTENZA ON TP_SCAGLIONI.ID_TIPO_UTENZA = TP_TIPIUTENZA.IDTIPOUTENZA"
    '           SSQL+=" WHERE TP_SCAGLIONI.IDENTE='" & ConstSession.IdEnte & "'"
    '           If operazione = "modifica" Then
    '               SQL += " AND (TP_SCAGLIONI.ID<>" & obj.ID & ")"
    '           Else
    '               If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
    '                   SSQL+=" AND TP_SCAGLIONI.ID=" & obj.ID & ""
    '               End If
    '           End If
    '           If obj.idTipoUtenza.CompareTo(-1) <> 0 And obj.idTipoUtenza.CompareTo(0) <> 0 Then
    '               SSQL+=" AND TP_TIPIUTENZA.IDTIPOUTENZA=" & obj.idTipoUtenza & ""
    '           End If
    '           If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
    '               SSQL+=" AND TP_SCAGLIONI.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
    '           End If
    '           If obj.DA.CompareTo(0) <> 0 Then
    '               SSQL+=" AND TP_SCAGLIONI.DA=" & obj.DA.ToString().Replace(",", ".") & ""
    '           End If
    '           If obj.A.CompareTo(0) <> 0 Then
    '               SSQL+=" AND TP_SCAGLIONI.A=" & obj.A.ToString().Replace(",", ".") & ""
    '           End If
    '           SQL += " ORDER BY TP_SCAGLIONI.ID_TIPO_UTENZA, TP_SCAGLIONI.DA, TP_SCAGLIONI.A"
    '           'eseguo la query
    '           Dim dsCanoniEnte As DataSet
    '           Log.Debug("GetScaglioniEnte::SQL::" & SQL)
    '           dsCanoniEnte = WFSessione.oSession.oAppDB.GetPrivateDataSet(SQL)
    '           If dsCanoniEnte.Tables(0).Rows.Count > 0 Then
    '               Dim oDatiScaglione As OggettoScaglione
    '               Dim arrayListoDatiRata As New ArrayList
    '               Dim iCount As Integer
    '               For iCount = 0 To dsCanoniEnte.Tables(0).Rows.Count - 1
    '                   oDatiScaglione = New OggettoScaglione

    '                   oDatiScaglione.ID = dsCanoniEnte.Tables(0).Rows(iCount)("ID")
    '                   oDatiScaglione.idTipoUtenza = dsCanoniEnte.Tables(0).Rows(iCount)("ID_TIPO_UTENZA")
    '                   oDatiScaglione.sDescrizioneTU = dsCanoniEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
    '                   oDatiScaglione.DA = dsCanoniEnte.Tables(0).Rows(iCount)("DA")
    '                   oDatiScaglione.A = dsCanoniEnte.Tables(0).Rows(iCount)("A")
    '                   oDatiScaglione.dAliquota = dsCanoniEnte.Tables(0).Rows(iCount)("ALIQUOTA")
    '                   If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("MINIMO")) Then
    '                       oDatiScaglione.dMinimo = dsCanoniEnte.Tables(0).Rows(iCount)("MINIMO")
    '                   End If
    '                   oDatiScaglione.dTariffa = dsCanoniEnte.Tables(0).Rows(iCount)("TARIFFA")
    '                   oDatiScaglione.sAnno = dsCanoniEnte.Tables(0).Rows(iCount)("ANNO")
    '                   oDatiScaglione.sIdEnte = dsCanoniEnte.Tables(0).Rows(iCount)("IDENTE")
    '                   arrayListoDatiRata.Add(oDatiScaglione)
    '               Next
    '               Return CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglione)), OggettoScaglione())
    '           End If

    '       Catch ex As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.GetScaglioniEnte.errore: ", ex)
    '           Throw New Exception("Problemi nell'esecuzione di GetScaglioniEnte " + ex.Message)
    '       Finally
    '           'chiudo la connessione
    '           If Not WFSessione.oSession Is Nothing Then
    '               WFSessione.Kill()
    '           End If
    '       End Try
    '   End Function

    'Public Function DeleteScaglioniEnte(ByVal OBJ As OggettoScaglione, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim NrataOld, DescrizioneOld, DataScadenzaOld As String

    '    Try

    '        DeleteScaglioniEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LO SCAGLIONE NON SIA STATO ASSOCIATO
    '        Dim DsScaglioni As DataSet
    '        DsScaglioni = GetScaglioniAssociati(OBJ)

    '        If DsScaglioni.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            DeleteScaglioniEnte = False
    '            Exit Function
    '        End If

    '        'SE NON è STATA ASSOCIATA LA CANCELLO
    '        SQL = "DELETE FROM TP_SCAGLIONI"
    '        SQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""

    '        'eseguo la query
    '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        DeleteScaglioniEnte = True
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.DeleteScaglioniEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di DeleteScaglioniEnte " + ex.Message)
    '        DeleteScaglioniEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try

    'End Function
    'Public Function UpdateScaglioniEnte(ByVal objOld As OggettoScaglione, ByVal objNew As OggettoScaglione, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Try

    '        UpdateScaglioniEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LO SCAGLIONE NON SIA STATO ASSOCIATO
    '        Dim DsScaglioni As DataSet
    '        DsScaglioni = GetScaglioniAssociati(objOld)

    '        If DsScaglioni.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            UpdateScaglioniEnte = False
    '            Exit Function
    '        End If


    '        'PRELEVO I DATI DELLO SCAGLIONE DA MODIFICARE
    '        Dim DST As OggettoScaglione()

    '        DST = GetScaglioniEnte(objOld)
    '        If DST.Length > 0 Then
    '            objOld.ID = DST(0).ID
    '            objOld.idTipoUtenza = DST(0).idTipoUtenza
    '            objOld.sAnno = DST(0).sAnno
    '            objOld.dMinimo = DST(0).dMinimo
    '            objOld.dTariffa = DST(0).dTariffa
    '            objOld.dAliquota = DST(0).dAliquota
    '            objOld.DA = DST(0).DA
    '            objOld.A = DST(0).A
    '        Else
    '            strError = "Non sono presenti dati per il Canone Selezionato."
    '            UpdateScaglioniEnte = False
    '            Exit Function
    '        End If

    '        If (objNew.dAliquota <> objOld.dAliquota Or objNew.dMinimo <> objOld.dMinimo Or objNew.dTariffa <> objOld.dTariffa Or objNew.DA <> objOld.DA Or objNew.A <> objOld.A Or objOld.sAnno <> objNew.sAnno Or objOld.idTipoUtenza <> objNew.idTipoUtenza) Then
    '            'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
    '            'se è presente blocco l'operazione
    '            Dim ds As OggettoScaglione()
    '            ds = GetScaglioniEnte(objNew, "modifica")
    '            If Not ds Is Nothing Then
    '                If ds.Length > 0 Then
    '                    strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '                    UpdateScaglioniEnte = False
    '                    Exit Function
    '                End If
    '            End If

    '            'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '            SQL = "UPDATE TP_SCAGLIONI SET TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
    '            SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '            SSQL+=" ANNO='" & objNew.sAnno & "',"
    '            SSQL+=" DA=" & objNew.DA.ToString.Replace(",", ".") & ","
    '            SSQL+=" A=" & objNew.A.ToString.Replace(",", ".") & ","
    '            SSQL+=" ID_TIPO_UTENZA=" & objNew.idTipoUtenza & ","
    '            SSQL+=" MINIMO=" & objNew.dMinimo.ToString.Replace(",", ".") & ""
    '            SSQL+=" WHERE ID=" & objNew.ID & ""
    '            SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        End If

    '        'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
    '        'If objOld.sAnno <> objNew.sAnno Then 'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then

    '        '    'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
    '        '    'se è presente blocco l'operazione
    '        '    Dim ds As OggettoScaglione()
    '        '    ds = GetScaglioniEnte(objNew)
    '        '    If ds.Length > 0 Then
    '        '        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '        '        UpdateScaglioniEnte = False
    '        '        Exit Function
    '        '    End If

    '        '    'verifico se è già presente uno scaglione con gli stessi dati storicizzata in data odierna
    '        '    'se è presente do il messaggio e blocco l'operazione
    '        '    'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
    '        '    SQL = "UPDATE TP_SCAGLIONI SET ANNO='" & objNew.sAnno & "',"
    '        '    SSQL+=" TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" MINIMO=" & objNew.dMinimo.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" DA=" & objNew.DA.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" A=" & objNew.A.ToString.Replace(",", ".") & ""
    '        '    SSQL+=" WHERE TP_SCAGLIONI.ID=" & objOld.ID & ""
    '        '    SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '        '    'eseguo la query
    '        '    myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        'ElseIf (objNew.dAliquota <> objOld.dAliquota Or objNew.dMinimo <> objOld.dMinimo Or objNew.dTariffa <> objOld.dTariffa Or objNew.DA <> objOld.DA Or objNew.A <> objOld.A) And objOld.sAnno = objNew.sAnno Then
    '        '    'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '        '    SQL = "UPDATE TP_SCAGLIONI SET TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" DA=" & objNew.DA.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" A=" & objNew.A.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" MINIMO=" & objNew.dMinimo.ToString.Replace(",", ".") & ""
    '        '    SSQL+=" WHERE ID=" & objNew.ID & ""
    '        '    SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '        '    'eseguo la query
    '        '    myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
    '        'End If
    '        UpdateScaglioniEnte = True
    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.UpdateScaglioniEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di UpdateScaglioniEnte " + ex.Message)
    '        UpdateScaglioniEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try

    'End Function

    'Public Function SetScaglioniEnte(ByVal obj As OggettoScaglione, ByRef strError As String) As Boolean
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer

    '    Try

    '        SetScaglioniEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim ds As OggettoScaglione()
    '        ds = GetScaglioniEnte(obj)
    '        If Not ds Is Nothing Then
    '            If ds.Length > 0 Then
    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
    '                SetScaglioniEnte = False
    '                Exit Function
    '            End If
    '        End If

    '        Dim SQL As String
    '        Dim DsCat As DataSet

    '        SQL = "INSERT INTO TP_SCAGLIONI"
    '        SSQL+=" (IDENTE, ANNO, ID_TIPO_UTENZA, TARIFFA, ALIQUOTA, MINIMO, DA, A)"
    '        SSQL+=" VALUES('" & ConstSession.IdEnte & "','" & obj.sAnno.Replace("'", "''") & "'," & obj.idTipoUtenza & "," & obj.dTariffa.ToString.Replace(",", ".") & "," & obj.dAliquota.ToString.Replace(",", ".") & "," & obj.dMinimo.ToString.Replace(",", ".") & "," & obj.DA.ToString.Replace(",", ".") & "," & obj.A.ToString.Replace(",", ".") & ")"

    '        'eseguo la query
    '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
    '        SetScaglioniEnte = True

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.SetScaglioniEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di SetScaglioniEnte " + ex.Message)
    '        SetScaglioniEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If

    '    End Try

    'End Function

    'Public Function GetScaglioniAssociati(ByVal obj As OggettoScaglione) As DataSet
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

    '        Dim SQL As String

    '        SQL = "SELECT * FROM TP_FATTURE_NOTE_SCAGLIONI"
    '        SSQL+=" WHERE TP_FATTURE_NOTE_SCAGLIONI.IDENTE='" & ConstSession.IdEnte & "'"
    '        If obj.ID <> 0 Then
    '            SSQL+=" AND TP_FATTURE_NOTE_SCAGLIONI.ID_SCAGLIONE=" & obj.ID
    '        End If

    '        'eseguo la query
    '        Dim dsScaglioniAssociati As DataSet
    '        dsScaglioniAssociati = WFSessione.oSession.oAppDB.GetPrivateDataSet(SQL)
    '        Return dsScaglioniAssociati

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.GetQuotaFissaAssociati.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di GetQuotaFissaAssociati " + ex.Message)
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try

    'End Function

    ''' <summary>
    ''' Estrae gli scaglioni configurati per l'Ente in esame.
    ''' Può estrarre tutti gli scaglioni oppure gli scaglioni in base ad un anno, ad un ID, ad un tipo di utenza o/e un intervallo 
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoScaglione</param>
    ''' <param name="operazione"></param>
    ''' <returns>array di oggetti di tipo OggettoScaglione</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetScaglioniEnte(ByVal obj As OggettoScaglione, Optional ByVal operazione As String = "") As OggettoScaglione()
        Dim DsDati As New DataSet
        Dim sSQL As String
        Dim oDatiScaglione As OggettoScaglione
        Dim arrayListoDatiRata As New ArrayList
        Dim iCount As Integer

        Try
            sSQL = "SELECT TP_SCAGLIONI.ANNO, TP_TIPIUTENZA.DESCRIZIONE, TP_SCAGLIONI.DA, TP_SCAGLIONI.A, TP_SCAGLIONI.TARIFFA, TP_SCAGLIONI.MINIMO, "
            sSQL += " TP_SCAGLIONI.ALIQUOTA, TP_SCAGLIONI.ID_TIPO_UTENZA, TP_SCAGLIONI.ID, TP_SCAGLIONI.IDENTE"
            SSQL += " FROM TP_SCAGLIONI INNER JOIN TP_TIPIUTENZA ON TP_SCAGLIONI.ID_TIPO_UTENZA = TP_TIPIUTENZA.IDTIPOUTENZA"
            SSQL += " WHERE TP_SCAGLIONI.IDENTE='" & ConstSession.IdEnte & "'"
            If operazione = "modifica" Then
                sSQL += " AND (TP_SCAGLIONI.ID<>" & obj.ID & ")"
            Else
                If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
                    SSQL += " AND TP_SCAGLIONI.ID=" & obj.ID & ""
                End If
            End If
            If obj.idTipoUtenza.CompareTo(-1) <> 0 And obj.idTipoUtenza.CompareTo(0) <> 0 Then
                SSQL += " AND TP_TIPIUTENZA.IDTIPOUTENZA=" & obj.idTipoUtenza & ""
            End If
            If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
                SSQL += " AND TP_SCAGLIONI.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
            End If
            If obj.DA.CompareTo(0) <> 0 Then
                SSQL += " AND TP_SCAGLIONI.DA=" & obj.DA.ToString().Replace(",", ".") & ""
            End If
            If obj.A.CompareTo(0) <> 0 Then
                SSQL += " AND TP_SCAGLIONI.A=" & obj.A.ToString().Replace(",", ".") & ""
            End If
            sSQL += " ORDER BY TP_SCAGLIONI.ANNO DESC, TP_SCAGLIONI.ID_TIPO_UTENZA, TP_SCAGLIONI.DA, TP_SCAGLIONI.A"
            'eseguo la query
            Dim dsCanoniEnte As DataSet
            Log.Debug("GetScaglioniEnte::SQL::" & sSQL)
            dsCanoniEnte = iDB.GetDataSet(sSQL)
            If dsCanoniEnte.Tables(0).Rows.Count > 0 Then
                For iCount = 0 To dsCanoniEnte.Tables(0).Rows.Count - 1
                    oDatiScaglione = New OggettoScaglione

                    oDatiScaglione.ID = dsCanoniEnte.Tables(0).Rows(iCount)("ID")
                    oDatiScaglione.idTipoUtenza = dsCanoniEnte.Tables(0).Rows(iCount)("ID_TIPO_UTENZA")
                    oDatiScaglione.sDescrizioneTU = dsCanoniEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
                    oDatiScaglione.DA = dsCanoniEnte.Tables(0).Rows(iCount)("DA")
                    oDatiScaglione.A = dsCanoniEnte.Tables(0).Rows(iCount)("A")
                    oDatiScaglione.dAliquota = dsCanoniEnte.Tables(0).Rows(iCount)("ALIQUOTA")
                    If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("MINIMO")) Then
                        oDatiScaglione.dMinimo = dsCanoniEnte.Tables(0).Rows(iCount)("MINIMO")
                    End If
                    oDatiScaglione.dTariffa = dsCanoniEnte.Tables(0).Rows(iCount)("TARIFFA")
                    oDatiScaglione.sAnno = dsCanoniEnte.Tables(0).Rows(iCount)("ANNO")
                    oDatiScaglione.sIdEnte = dsCanoniEnte.Tables(0).Rows(iCount)("IDENTE")
                    arrayListoDatiRata.Add(oDatiScaglione)
                Next
            End If
            Return CType(arrayListoDatiRata.ToArray(GetType(OggettoScaglione)), OggettoScaglione())
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.GetScaglioniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetScaglioniEnte " + ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Verifica se uno scaglione condifurato è già utilizzato per il calcolo delle fatture.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoScaglione</param>
    ''' <returns>Dataset</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetScaglioniAssociati(ByVal obj As OggettoScaglione) As DataSet
        Dim DsDati As New DataSet

        Try
            Dim sSQL As String

            sSQL = "SELECT * FROM TP_FATTURE_NOTE_SCAGLIONI"
            sSQL += " WHERE TP_FATTURE_NOTE_SCAGLIONI.IDENTE='" & ConstSession.IdEnte & "'"
            If obj.ID <> 0 Then
                SSQL += " AND TP_FATTURE_NOTE_SCAGLIONI.ID_SCAGLIONE=" & obj.ID
            End If

            'eseguo la query
            Dim dsScaglioniAssociati As DataSet
            dsScaglioniAssociati = iDB.GetDataSet(sSQL)
            Return dsScaglioniAssociati

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.GetScaglioniAssociati.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetQuotaFissaAssociati " + ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Inserisce uno scaglione.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoScaglione</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function SetScaglioniEnte(ByVal obj As OggettoScaglione, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try

            SetScaglioniEnte = False

            Dim ds As OggettoScaglione()
            ds = GetScaglioniEnte(obj)
            If Not ds Is Nothing Then
                If ds.Length > 0 Then
                    strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                    SetScaglioniEnte = False
                    Exit Function
                End If
            End If

            Dim sSQL As String

            sSQL = "INSERT INTO TP_SCAGLIONI"
            sSQL += " (IDENTE, ANNO, ID_TIPO_UTENZA, TARIFFA, ALIQUOTA, MINIMO, DA, A)"
            SSQL += " VALUES('" & ConstSession.IdEnte & "','" & obj.sAnno.Replace("'", "''") & "'," & obj.idTipoUtenza & "," & obj.dTariffa.ToString.Replace(",", ".") & "," & obj.dAliquota.ToString.Replace(",", ".") & "," & obj.dMinimo.ToString.Replace(",", ".") & "," & obj.DA.ToString.Replace(",", ".") & "," & obj.A.ToString.Replace(",", ".") & ")"

            'eseguo la query
            myIdentity = CInt(iDB.ExecuteNonQuery(sSQL))
            SetScaglioniEnte = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.SetScaglioniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di SetScaglioniEnte " + ex.Message)
            SetScaglioniEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Esegue l'aggiornamento di un determinato scaglione configurato già a sistema SOLO nel caso
    ''' in cui non è ancora utilizzato dalla fatturazione.
    ''' </summary>
    ''' <param name="objOld">oggetto di tipo OggettoScaglione precedente</param>
    ''' <param name="objNew">oggetto di tipo OggettoScaglione attuale</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function UpdateScaglioniEnte(ByVal objOld As OggettoScaglione, ByVal objNew As OggettoScaglione, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try
            UpdateScaglioniEnte = False
            Dim sSQL As String

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LO SCAGLIONE NON SIA STATO ASSOCIATO
            Dim DsScaglioni As DataSet
            DsScaglioni = GetScaglioniAssociati(objOld)

            If DsScaglioni.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                UpdateScaglioniEnte = False
                Exit Function
            End If


            'PRELEVO I DATI DELLO SCAGLIONE DA MODIFICARE
            Dim DST As OggettoScaglione()

            DST = GetScaglioniEnte(objOld)
            If DST.Length > 0 Then
                objOld.ID = DST(0).ID
                objOld.idTipoUtenza = DST(0).idTipoUtenza
                objOld.sAnno = DST(0).sAnno
                objOld.dMinimo = DST(0).dMinimo
                objOld.dTariffa = DST(0).dTariffa
                objOld.dAliquota = DST(0).dAliquota
                objOld.DA = DST(0).DA
                objOld.A = DST(0).A
            Else
                strError = "Non sono presenti dati per il Canone Selezionato."
                UpdateScaglioniEnte = False
                Exit Function
            End If

            If (objNew.dAliquota <> objOld.dAliquota Or objNew.dMinimo <> objOld.dMinimo Or objNew.dTariffa <> objOld.dTariffa Or objNew.DA <> objOld.DA Or objNew.A <> objOld.A Or objOld.sAnno <> objNew.sAnno Or objOld.idTipoUtenza <> objNew.idTipoUtenza) Then
                'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
                'se è presente blocco l'operazione
                Dim ds As OggettoScaglione()
                ds = GetScaglioniEnte(objNew, "modifica")
                If Not ds Is Nothing Then
                    If ds.Length > 0 Then
                        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
                        UpdateScaglioniEnte = False
                        Exit Function
                    End If
                End If

                'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
                sSQL = "UPDATE TP_SCAGLIONI SET TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
                sSQL += " ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
                SSQL += " ANNO='" & objNew.sAnno & "',"
                SSQL += " DA=" & objNew.DA.ToString.Replace(",", ".") & ","
                SSQL += " A=" & objNew.A.ToString.Replace(",", ".") & ","
                SSQL += " ID_TIPO_UTENZA=" & objNew.idTipoUtenza & ","
                SSQL += " MINIMO=" & objNew.dMinimo.ToString.Replace(",", ".") & ""
                SSQL += " WHERE ID=" & objNew.ID & ""
                SSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
                'eseguo la query
                myIdentity = CInt(iDB.ExecuteNonQuery(sSQL))

            End If

            'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
            'If objOld.sAnno <> objNew.sAnno Then 'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then

            '    'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
            '    'se è presente blocco l'operazione
            '    Dim ds As OggettoScaglione()
            '    ds = GetScaglioniEnte(objNew)
            '    If ds.Length > 0 Then
            '        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
            '        UpdateScaglioniEnte = False
            '        Exit Function
            '    End If

            '    'verifico se è già presente uno scaglione con gli stessi dati storicizzata in data odierna
            '    'se è presente do il messaggio e blocco l'operazione
            '    'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
            '    SQL = "UPDATE TP_SCAGLIONI SET ANNO='" & objNew.sAnno & "',"
            '    SSQL+=" TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
            '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
            '    SSQL+=" MINIMO=" & objNew.dMinimo.ToString.Replace(",", ".") & ","
            '    SSQL+=" DA=" & objNew.DA.ToString.Replace(",", ".") & ","
            '    SSQL+=" A=" & objNew.A.ToString.Replace(",", ".") & ""
            '    SSQL+=" WHERE TP_SCAGLIONI.ID=" & objOld.ID & ""
            '    SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
            '    'eseguo la query
            '    myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

            'ElseIf (objNew.dAliquota <> objOld.dAliquota Or objNew.dMinimo <> objOld.dMinimo Or objNew.dTariffa <> objOld.dTariffa Or objNew.DA <> objOld.DA Or objNew.A <> objOld.A) And objOld.sAnno = objNew.sAnno Then
            '    'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
            '    SQL = "UPDATE TP_SCAGLIONI SET TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
            '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
            '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
            '    SSQL+=" DA=" & objNew.DA.ToString.Replace(",", ".") & ","
            '    SSQL+=" A=" & objNew.A.ToString.Replace(",", ".") & ","
            '    SSQL+=" MINIMO=" & objNew.dMinimo.ToString.Replace(",", ".") & ""
            '    SSQL+=" WHERE ID=" & objNew.ID & ""
            '    SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
            '    'eseguo la query
            '    myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
            'End If
            UpdateScaglioniEnte = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.UpdateScaglioniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di UpdateScaglioniEnte " + ex.Message)
            UpdateScaglioniEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Esegue l'eliminazione di un determinato scaglione configurato già a sistema SOLO nel caso
    ''' in cui non è ancora utilizzato dalla fatturazione.
    ''' </summary>
    ''' <param name="OBJ">oggetto di tipo OggettoScaglione</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function DeleteScaglioniEnte(ByVal OBJ As OggettoScaglione, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try
            DeleteScaglioniEnte = False
            Dim SQL As String

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LO SCAGLIONE NON SIA STATO ASSOCIATO
            Dim DsScaglioni As DataSet
            DsScaglioni = GetScaglioniAssociati(OBJ)

            If DsScaglioni.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                DeleteScaglioniEnte = False
                Exit Function
            End If

            'SE NON è STATA ASSOCIATA LA CANCELLO
            SQL = "DELETE FROM TP_SCAGLIONI"
            SQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""

            'eseguo la query
            myIdentity = CInt(iDB.ExecuteNonQuery(SQL))

            DeleteScaglioniEnte = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsScaglioni.DeleteScaglioniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeleteScaglioniEnte " + ex.Message)
            DeleteScaglioniEnte = False
        End Try
    End Function
End Class
