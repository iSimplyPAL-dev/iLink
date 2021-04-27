Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Public Class ClsAddizionali
    ''' <summary>
    ''' Gestione delle addizionali per Ente
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsAddizionali))
    Private iDB As New DBAccess.getDBobject
    Dim clsGenerale As New ClsGenerale.Generale

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Estrae le addizionali configurate per l'Ente in esame.
    ''' Può estrarre tutte le addizionali oppure un addizionale specifica se viene passato l'ID della tabella
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoAddizionaleEnte</param>
    ''' <returns>un array di oggetti di tipo OggettoAddizionaleEnte</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function GetAddizionaliEnte(ByVal obj As OggettoAddizionaleEnte) As OggettoAddizionaleEnte()  'As DataSet
        Dim oDatiAddizionaleEnte As OggettoAddizionaleEnte
        Dim arrayListoDatiRata As New ArrayList
        Dim iCount As Integer
        Dim sSQL As String
        Dim dsAddizionaliEnte As DataSet

        Try
            sSQL = "SELECT TP_ADDIZIONALI_ENTE.ANNO, TP_ADDIZIONALI.DESCRIZIONE, TP_ADDIZIONALI_ENTE.IMPORTO, TP_ADDIZIONALI_ENTE.ALIQUOTA, "
            sSQL +=" TP_ADDIZIONALI_ENTE.ID_ADDIZIONALE, TP_ADDIZIONALI_ENTE.ID AS IDADDIZIONALEENTE, TP_ADDIZIONALI_ENTE.IDENTE"
            SSQL+=" FROM TP_ADDIZIONALI_ENTE INNER JOIN TP_ADDIZIONALI ON TP_ADDIZIONALI_ENTE.ID_ADDIZIONALE = TP_ADDIZIONALI.ID_ADDIZIONALE"
            SSQL+=" WHERE TP_ADDIZIONALI_ENTE.IDENTE='" & ConstSession.IdEnte & "'"
            If obj.IDaddizionale.CompareTo(-1) <> 0 And obj.IDaddizionale.CompareTo(0) <> 0 Then
                SSQL+=" AND TP_ADDIZIONALI_ENTE.ID_ADDIZIONALE=" & obj.IDaddizionale & ""
            End If
            If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
                SSQL+=" AND TP_ADDIZIONALI_ENTE.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
            End If
            'eseguo la query
            Log.Debug("GetAddizionaliEnte::SQL::" & sSQL)
            dsAddizionaliEnte = iDB.GetDataSet(sSQL)
            If dsAddizionaliEnte.Tables(0).Rows.Count > 0 Then
                For iCount = 0 To dsAddizionaliEnte.Tables(0).Rows.Count - 1
                    oDatiAddizionaleEnte = New OggettoAddizionaleEnte
                    oDatiAddizionaleEnte.ID = dsAddizionaliEnte.Tables(0).Rows(iCount)("IDADDIZIONALEENTE")
                    oDatiAddizionaleEnte.IDaddizionale = dsAddizionaliEnte.Tables(0).Rows(iCount)("ID_ADDIZIONALE")
                    oDatiAddizionaleEnte.Aliquota = dsAddizionaliEnte.Tables(0).Rows(iCount)("ALIQUOTA")
                    oDatiAddizionaleEnte.dImporto = dsAddizionaliEnte.Tables(0).Rows(iCount)("IMPORTO")
                    oDatiAddizionaleEnte.sAnno = dsAddizionaliEnte.Tables(0).Rows(iCount)("ANNO")
                    oDatiAddizionaleEnte.sIdEnte = dsAddizionaliEnte.Tables(0).Rows(iCount)("IDENTE")
                    oDatiAddizionaleEnte.sDescrizione = dsAddizionaliEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
                    arrayListoDatiRata.Add(oDatiAddizionaleEnte)
                Next
            End If
            Return CType(arrayListoDatiRata.ToArray(GetType(OggettoAddizionaleEnte)), OggettoAddizionaleEnte())
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAddizionali.GetAddizionaliEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetAddizionaliEnte " + ex.Message)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Verifica se un addizionale condifurata è già utilizzata per il calcolo delle fatture.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoAddizionaleEnte</param>
    ''' <returns>Dataset</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function GetAddizionaliAssociati(ByVal obj As OggettoAddizionaleEnte) As DataSet
        Dim DsDati As New DataSet
        Dim sSQL As String

        Try
            sSQL = "SELECT * FROM TP_FATTURE_NOTE_ADDIZIONALI"
            sSQL +=" WHERE IDENTE='" & ConstSession.IdEnte & "'"
            If obj.ID <> 0 Then
                SSQL+=" AND ID_ADDIZIONALE=" & obj.ID
            End If
            DsDati = iDB.GetDataSet(sSQL)
            Return DsDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAddizionali.GetAddizionaliAssociati.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetAddizionaliAssociati " + ex.Message)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Inserisce/modifica un addizionale.
    ''' Esegue l'aggiornamento di una determinata addizionale configurata già a sistema SOLO nel caso in cui non è ancora utilizzata dalla fatturazione.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoAddizionaleEnte attuale</param>
    ''' <param name="objOld">oggetto di tipo OggettoAddizionaleEnte precedente</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function SetAddizionaliEnte(ByVal obj As OggettoAddizionaleEnte, ByVal objOld As OggettoAddizionaleEnte, ByRef strError As String) As Boolean
        Dim ds As OggettoAddizionaleEnte()
        Dim myIdentity As Integer
        Dim nId As Integer = -1

        Try
            If Not objOld Is Nothing Then
                'PRIMA DI ELIMINARE DEVO VERIFICARE CHE L' ADDIZIONALE NON SIA STATO ASSOCIATO
                Dim DsAddizionale As DataSet
                DsAddizionale = GetAddizionaliAssociati(obj)
                If DsAddizionale.Tables(0).Rows.Count > 0 Then
                    'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                    strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                    Return False
                End If

                'PRELEVO I DATI DELLA TARIFFA DA MODIFICARE
                Dim DST As OggettoAddizionaleEnte()
                DST = GetAddizionaliEnte(objOld)
                If DST.Length > 0 Then
                    objOld.ID = DST(0).IDaddizionale
                    objOld.IDaddizionale = DST(0).IDaddizionale
                    objOld.dImporto = DST(0).dImporto
                    objOld.sAnno = DST(0).sAnno
                    objOld.Aliquota = DST(0).Aliquota
                Else
                    strError = "Non sono presenti dati per l\'Addizionale Selezionato."
                    Return False
                End If
                'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
                If objOld.sAnno <> obj.sAnno Then          'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then
                    'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
                    'se è presente blocco l'operazione
                    ds = GetAddizionaliEnte(obj)
                    If Not ds Is Nothing Then
                        If ds.Length > 0 Then
                            strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                            Return False
                        End If
                    End If
                    'verifico se è già presente una tariffa con gli stessi dati storicizzata in data odierna
                    'se è presente do il messaggio e blocco l'operazione
                    'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
                    nId = objOld.ID
                    'eseguo la query
                    myIdentity = iDB.ExecuteNonQuery("prc_TP_ADDIZIONALI_ENTE_IU", New SqlParameter("@ID", nId) _
                         , New SqlParameter("@IDENTE", ConstSession.IdEnte) _
                         , New SqlParameter("@ID_ADDIZIONALE", obj.IDaddizionale) _
                         , New SqlParameter("@ANNO", obj.sAnno) _
                         , New SqlParameter("@IMPORTO", obj.dImporto) _
                         , New SqlParameter("@ALIQUOTA", obj.Aliquota))

                ElseIf (obj.dImporto <> objOld.dImporto Or obj.Aliquota <> objOld.Aliquota) And objOld.sAnno = obj.sAnno Then
                    'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
                    'SQL = "UPDATE TBLADDIZIONALIENTE SET VALORE='" & obj.sValore.Replace(",", ".") & "'"
                    nId = obj.ID
                    'eseguo la query
                    myIdentity = iDB.ExecuteNonQuery("prc_TP_ADDIZIONALI_ENTE_IU", New SqlParameter("@ID", nId) _
                         , New SqlParameter("@IDENTE", ConstSession.IdEnte) _
                         , New SqlParameter("@ID_ADDIZIONALE", obj.IDaddizionale) _
                         , New SqlParameter("@ANNO", obj.sAnno) _
                         , New SqlParameter("@IMPORTO", obj.dImporto) _
                         , New SqlParameter("@ALIQUOTA", obj.Aliquota))
                End If
            Else
                ds = GetAddizionaliEnte(obj)
                If Not ds Is Nothing Then
                    If ds.Length > 0 Then
                        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                        Return False
                    End If
                End If

                'eseguo la query
                myIdentity = iDB.ExecuteNonQuery("prc_TP_ADDIZIONALI_ENTE_IU", New SqlParameter("@ID", nId) _
                     , New SqlParameter("@IDENTE", ConstSession.IdEnte) _
                     , New SqlParameter("@ID_ADDIZIONALE", obj.IDaddizionale) _
                     , New SqlParameter("@ANNO", obj.sAnno) _
                     , New SqlParameter("@IMPORTO", obj.dImporto) _
                     , New SqlParameter("@ALIQUOTA", obj.Aliquota))
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAddizionali.SetAddizionaliEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di SetAddizionali " + ex.Message)
        End Try
    End Function

    'Public Function UpdateAddizionaliEnte(ByVal objOld As OggettoAddizionaleEnte, ByVal objNew As OggettoAddizionaleEnte, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Try

    '        UpdateAddizionaliEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE L' ADDIZIONALE NON SIA STATO ASSOCIATO
    '        Dim DsAddizionale As DataSet
    '        DsAddizionale = GetAddizionaliAssociati(objOld)

    '        If DsAddizionale.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            UpdateAddizionaliEnte = False
    '            Exit Function
    '        End If

    '        'PRELEVO I DATI DELLA TARIFFA DA MODIFICARE
    '        Dim DST As OggettoAddizionaleEnte()

    '        DST = GetAddizionaliEnte(objOld)
    '        If DST.Length > 0 Then
    '            objOld.ID = DST(0).IDaddizionale
    '            objOld.IDaddizionale = DST(0).IDaddizionale
    '            objOld.dImporto = DST(0).dImporto
    '            objOld.sAnno = DST(0).sAnno
    '            objOld.Aliquota = DST(0).Aliquota
    '        Else
    '            strError = "Non sono presenti dati per l\'Addizionale Selezionato."
    '            UpdateAddizionaliEnte = False
    '            Exit Function
    '        End If
    '        'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
    '        If objOld.sAnno <> objNew.sAnno Then          'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then
    '            'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
    '            'se è presente blocco l'operazione
    '            Dim ds As OggettoAddizionaleEnte()
    '            ds = GetAddizionaliEnte(objNew)
    '            If ds.Length > 0 Then
    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '                UpdateAddizionaliEnte = False
    '                Exit Function
    '            End If
    '            'verifico se è già presente una tariffa con gli stessi dati storicizzata in data odierna
    '            'se è presente do il messaggio e blocco l'operazione
    '            'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
    '            SQL = "UPDATE TP_ADDIZIONALI_ENTE SET ANNO='" & objNew.sAnno & "',"
    '            SSQL+=" IMPORTO=" & objNew.dImporto.ToString.Replace(",", ".") & ","
    '            SSQL+=" ALIQUOTA=" & objNew.Aliquota.ToString.Replace(",", ".") & ""
    '            SSQL+=" WHERE TBLADDIZIONALIENTE.ID=" & objOld.ID & ""
    '            SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        ElseIf (objNew.dImporto <> objOld.dImporto Or objNew.Aliquota <> objOld.Aliquota) And objOld.sAnno = objNew.sAnno Then
    '            'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '            'SQL = "UPDATE TBLADDIZIONALIENTE SET VALORE='" & objNew.sValore.Replace(",", ".") & "'"
    '            SQL = "UPDATE TP_ADDIZIONALI_ENTE SET IMPORTO=" & objNew.dImporto.ToString.Replace(",", ".") & ","
    '            SSQL+=" ALIQUOTA=" & objNew.Aliquota.ToString.Replace(",", ".") & ""
    '            SSQL+=" WHERE ID=" & objNew.ID & ""
    '            SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
    '        End If
    '        UpdateAddizionaliEnte = True
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAddizionali.DeleteAddizionaliEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di UpdateAddizionali " + ex.Message)
    '        UpdateAddizionaliEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try

    'End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Esegue l'eliminazione di una determinata addizionale configurata già a sistema SOLO nel casoin cui non è ancora utilizzata dalla fatturazione.
    ''' </summary>
    ''' <param name="OBJ">oggetto di tipo OggettoAddizionaleEnte</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'aggiornamento è andato a buon fine, FALSE se l'aggiornamento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function DeleteAddizionaleEnte(ByVal OBJ As OggettoAddizionaleEnte, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Try
            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE L' ADDIZIONALE NON SIA STATO ASSOCIATO
            Dim DsAddizionale As DataSet
            DsAddizionale = GetAddizionaliAssociati(OBJ)
            If DsAddizionale.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                Return False
            End If

            myIdentity = iDB.ExecuteNonQuery("prc_TP_ADDIZIONALI_ENTE_D", Nothing, New SqlParameter("@ID", OBJ.ID))
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAddizionali.DeleteAddizionaliEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeleteAddizionale " + ex.Message)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Estrae le voci delle addizionali configurate a livello generale e carica un oggetto di tipo DropDownList(combo).
    ''' </summary>
    ''' <param name="ddlAddizionali">DropDownList</param>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Sub LoadComboAddizionali(ByVal ddlAddizionali As DropDownList)
        Try
            Dim SQL As String

            '*** 20141117 - voce di costo specifica per utente ***
            'SQL = "SELECT DESCRIZIONE AS DESCRIZIONE, ID_ADDIZIONALE FROM TP_ADDIZIONALI"
            SQL = "SELECT * FROM V_GETADDIZIONALI"
            '*** ***
            'eseguo la query
            clsGenerale.LoadComboGenerale(ddlAddizionali, SQL)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAddizionali.LoadComboAddizionali.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di LoadComboAddizionali " + ex.Message)
        End Try
    End Sub
End Class

