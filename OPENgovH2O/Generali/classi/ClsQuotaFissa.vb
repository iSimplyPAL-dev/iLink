Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Public Class ClsQuotaFissa
	Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsQuotaFissa))
    Private iDB As New DBAccess.getDBobject

    'Public Function DeleteQuotaFissaEnte(ByVal OBJ As OggettoQuotaFissa, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim NrataOld, DescrizioneOld, DataScadenzaOld As String
    '    Dim SQL As String

    '    Try
    '        DeleteQuotaFissaEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If


    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA QUOTA FISSA NON SIA STATA ASSOCIATA
    '        Dim DsQuotaFissa As DataSet
    '        DsQuotaFissa = GetQuotaFissaAssociati(OBJ)
    '        If DsQuotaFissa.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            DeleteQuotaFissaEnte = False
    '            Exit Function
    '        End If

    '        'SE NON è STATA ASSOCIATA LA CANCELLO
    '        SQL = "DELETE FROM TP_QUOTA_FISSA"
    '        SQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""
    '        'eseguo la query
    '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        DeleteQuotaFissaEnte = True
    '    Catch ex As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.DeleteQuotaFissaEnte.errore: ", ex) 
    '        Throw New Exception("Problemi nell'esecuzione di DeleteQuotaFissaEnte " + ex.Message)
    '        DeleteQuotaFissaEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function UpdateQuotaFissaEnte(ByVal sCodEnte As String, ByVal objOld As OggettoQuotaFissa, ByVal objNew As OggettoQuotaFissa, ByRef strError As String, ByVal WFSessione As CreateSessione) As Boolean
    '    Dim WFErrore As String
    '    Dim myIdentity As Integer
    '    dim sSQL as string
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Try
    '        UpdateQuotaFissaEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA QUOTA FISSA NON SIA STATA ASSOCIATA
    '        Dim DsQuotaFissa As DataSet
    '        DsQuotaFissa = GetQuotaFissaAssociati(objOld)
    '        If DsQuotaFissa.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            UpdateQuotaFissaEnte = False
    '            Exit Function
    '        End If

    '        'PRELEVO I DATI DELLO SCAGLIONE DA MODIFICARE
    '        Dim DST As OggettoQuotaFissa()
    '        DST = GetQuotaFissaEnte(objOld)
    '        If DST.Length > 0 Then
    '            objOld.ID = DST(0).ID
    '            objOld.idTipoUtenza = DST(0).idTipoUtenza
    '            objOld.sAnno = DST(0).sAnno
    '            objOld.dImportoDep = DST(0).dImportoDep
    '            objOld.dImportoFog = DST(0).dImportoFog
    '            objOld.dImportoH2O = DST(0).dImportoH2O
    '            objOld.dAliquota = DST(0).dAliquota
    '            objOld.DA = DST(0).DA
    '            objOld.A = DST(0).A
    '            objOld.bIsAGiorni = DST(0).bIsAGiorni
    '        Else
    '            strError = "Non sono presenti dati per la Quota Fissa Selezionata."
    '            UpdateQuotaFissaEnte = False
    '            Exit Function
    '        End If

    '        If (objNew.dAliquota <> objOld.dAliquota Or objNew.dImportoDep <> objOld.dImportoDep Or objNew.dImportoFog <> objOld.dImportoFog Or objNew.dImportoH2O <> objOld.dImportoH2O Or objNew.DA <> objOld.DA Or objNew.A <> objOld.A Or objOld.sAnno = objNew.sAnno Or objOld.idTipoUtenza = objNew.idTipoUtenza Or objOld.bIsAGiorni <> objNew.bIsAGiorni) Then
    '            ''verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
    '            ''se è presente blocco l'operazione
    '            'Dim ds As OggettoQuotaFissa()
    '            'ds = GetQuotaFissaEnte(objNew)
    '            'If Not ds Is Nothing Then
    '            '    If ds.Length > 0 Then
    '            '        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '            '        UpdateQuotaFissaEnte = False
    '            '        Exit Function
    '            '    End If
    '            'End If

    '            'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE

    '            sSQL = "UPDATE TP_QUOTA_FISSA SET IMPORTOH2O=" & objNew.dImportoH2O.ToString.Replace(",", ".") & ","
    '            sSQL += " IMPORTOFOG=" & objNew.dImportoFog & ","
    '            sSQL += " IMPORTODEP=" & objNew.dImportoDep & ","
    '            sSQL += " ANNO='" & objNew.sAnno & "',"
    '            sSQL += " ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '            sSQL += " ID_TIPO_UTENZA=" & objNew.idTipoUtenza & ","
    '            sSQL += " DA=" & objNew.DA.ToString.Replace(",", ".") & ","
    '            sSQL += " A=" & objNew.A.ToString.Replace(",", ".") & ","
    '            sSQL += " ISAGIORNI=" & CInt(objNew.bIsAGiorni)
    '            sSQL += " WHERE (ID=" & objNew.ID & ")"
    '            sSQL += " AND (IDENTE='" & sCodEnte & "')"
    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(sSQL))


    '        End If
    '        UpdateQuotaFissaEnte = True
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.UpdateQuotaFissaEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di UpdateQuotaFissaEnte " + ex.Message)
    '        UpdateQuotaFissaEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try

    'End Function

    'Public Function SetQuotaFissaEnte(ByVal obj As OggettoQuotaFissa, ByRef strError As String, ByVal WFSessione As CreateSessione) As Boolean
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFErrore As String
    '    Dim myIdentity As Integer

    '    Try

    '        SetQuotaFissaEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim ds As OggettoQuotaFissa()
    '        ds = GetQuotaFissaEnte(obj)
    '        If Not ds Is Nothing Then
    '            If ds.Length > 0 Then
    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
    '                SetQuotaFissaEnte = False
    '                Exit Function
    '            End If
    '        End If

    '        Dim SQL As String
    '        Dim DsCat As DataSet

    '        If obj.dImportoDep > 0 Then
    '            SQL = "INSERT INTO TP_QUOTA_FISSA"
    '            SSQL+=" (IDENTE, ANNO, ID_TIPO_UTENZA, IMPORTOH2O, IMPORTOFOG, IMPORTODEP, ALIQUOTA, DA, A, ISAGIORNI)"
    '            SSQL+=" VALUES('" & ConstSession.IdEnte & "','" & obj.sAnno.Replace("'", "''") & "'," & obj.idTipoUtenza & "," & obj.dImportoH2O.ToString.Replace(",", ".") & "," & obj.dImportoFog.ToString.Replace(",", ".") & "," & obj.dImportoDep.ToString.Replace(",", ".") & "," & obj.dAliquota.ToString.Replace(",", ".") & "," & obj.DA.ToString.Replace(",", ".") & "," & obj.A.ToString.Replace(",", ".") & "," & CInt(obj.bIsAGiorni) & ")"

    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
    '        End If

    '        SetQuotaFissaEnte = True

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.SetQuotaFissaEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di SetScaglioniEnte " + ex.Message)
    '        SetQuotaFissaEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function GetQuotaFissaAssociati(ByVal obj As OggettoQuotaFissa, ByVal WFSessione As CreateSessione) As DataSet
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Dim DsDati As New DataSet
    '    Dim WFErrore As String

    '    Try

    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        SQL = "SELECT * FROM TP_FATTURE_NOTE_QUOTA_FISSA"
    '        SSQL+=" WHERE TP_FATTURE_NOTE_QUOTA_FISSA.IDENTE='" & ConstSession.IdEnte & "'"
    '        If obj.ID <> 0 Then
    '            SSQL+=" AND TP_FATTURE_NOTE_QUOTA_FISSA.ID_QUOTAFISSA=" & obj.ID
    '        End If

    '        'eseguo la query
    '        Dim dsQuotaFissaAssociati As DataSet
    '        dsQuotaFissaAssociati = WFSessione.oSession.oAppDB.GetPrivateDataSet(SQL)
    '        Return dsQuotaFissaAssociati

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.GetQuotaFissaAssociati.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di GetQuotaFissaAssociati " + ex.Message)
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function GetQuotaFissaEnte(ByVal obj As OggettoQuotaFissa, ByVal WFSessione As CreateSessione) As OggettoQuotaFissa()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    dim sSQL as string
    '    Dim DsDati As New DataSet
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        sSQL = "SELECT TP_QUOTA_FISSA.ANNO, TP_TIPIUTENZA.DESCRIZIONE, TP_QUOTA_FISSA.DA, TP_QUOTA_FISSA.A, TP_QUOTA_FISSA.IMPORTOH2O, TP_QUOTA_FISSA.IMPORTODEP, TP_QUOTA_FISSA.IMPORTOFOG, "
    '        sSQL += "TP_QUOTA_FISSA.ALIQUOTA, TP_QUOTA_FISSA.ID_TIPO_UTENZA, TP_QUOTA_FISSA.ID, TP_QUOTA_FISSA.IDENTE, TP_QUOTA_FISSA.ISAGIORNI"
    '        sSQL += " FROM TP_QUOTA_FISSA"
    '        sSQL += " INNER JOIN TP_TIPIUTENZA ON TP_QUOTA_FISSA.ID_TIPO_UTENZA = TP_TIPIUTENZA.IDTIPOUTENZA"
    '        sSQL += " WHERE TP_QUOTA_FISSA.IDENTE='" & ConstSession.IdEnte & "'"
    '        If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
    '            sSQL += " AND TP_QUOTA_FISSA.ID=" & obj.ID & ""
    '        End If
    '        If obj.idTipoUtenza.CompareTo(-1) <> 0 And obj.idTipoUtenza.CompareTo(0) <> 0 Then
    '            sSQL += " AND TP_TIPIUTENZA.IDTIPOUTENZA=" & obj.idTipoUtenza
    '        End If
    '        If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
    '            sSQL += " AND TP_QUOTA_FISSA.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
    '        End If
    '        If obj.DA.CompareTo(0) <> 0 Then
    '            sSQL += " AND TP_QUOTA_FISSA.DA=" & obj.DA.ToString().Replace(",", ".") & ""
    '        End If
    '        If obj.A.CompareTo(0) <> 0 Then
    '            sSQL += " AND TP_QUOTA_FISSA.A=" & obj.A.ToString().Replace(",", ".") & ""
    '        End If
    '        ''If obj.dImporto.CompareTo(CDbl(0)) <> 0 Then
    '        ''    ssql+="AND TP_QUOTA_FISSA.IMPORTO=" & obj.dImporto.ToString().Replace(",", ".") & ""
    '        ''End If
    '        ''If obj.dAliquota.CompareTo(CDbl(0)) <> 0 Then
    '        ''    ssql+="AND TP_QUOTA_FISSA.ALIQUOTA=" & obj.dAliquota.ToString().Replace(",", ".") & ""
    '        ''End If

    '        'eseguo la query
    '        Dim dsCanoniEnte As DataSet
    '        Log.Debug("GetQuotaFissaEnte::SQL::" & sSQL)
    '        dsCanoniEnte = WFSessione.oSession.oAppDB.GetPrivateDataSet(sSQL)
    '        If dsCanoniEnte.Tables(0).Rows.Count > 0 Then
    '            Dim oDatiQF As OggettoQuotaFissa
    '            Dim arrayListoDatiRata As New ArrayList
    '            Dim iCount As Integer
    '            For iCount = 0 To dsCanoniEnte.Tables(0).Rows.Count - 1
    '                oDatiQF = New OggettoQuotaFissa

    '                oDatiQF.ID = dsCanoniEnte.Tables(0).Rows(iCount)("ID")
    '                oDatiQF.idTipoUtenza = dsCanoniEnte.Tables(0).Rows(iCount)("ID_TIPO_UTENZA")
    '                oDatiQF.sDescrizioneTU = dsCanoniEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
    '                oDatiQF.DA = dsCanoniEnte.Tables(0).Rows(iCount)("DA")
    '                oDatiQF.A = dsCanoniEnte.Tables(0).Rows(iCount)("A")
    '                oDatiQF.dAliquota = dsCanoniEnte.Tables(0).Rows(iCount)("ALIQUOTA")
    '                If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTODEP")) Then
    '                    oDatiQF.dImportoDep = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTODEP")
    '                Else
    '                    oDatiQF.dImportoDep = 0
    '                End If
    '                If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOFOG")) Then
    '                    oDatiQF.dImportoFog = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOFOG")
    '                Else
    '                    oDatiQF.dImportoFog = 0
    '                End If
    '                If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOH2O")) Then
    '                    oDatiQF.dImportoH2O = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOH2O")
    '                Else
    '                    oDatiQF.dImportoH2O = 0
    '                End If
    '                oDatiQF.sAnno = dsCanoniEnte.Tables(0).Rows(iCount)("ANNO")
    '                oDatiQF.sIdEnte = dsCanoniEnte.Tables(0).Rows(iCount)("IDENTE")
    '                oDatiQF.bIsAGiorni = CBool(dsCanoniEnte.Tables(0).Rows(iCount)("ISAGIORNI"))
    '                arrayListoDatiRata.Add(oDatiQF)
    '            Next
    '            Return CType(arrayListoDatiRata.ToArray(GetType(OggettoQuotaFissa)), OggettoQuotaFissa())
    '        End If
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.GetQuotaFissaEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di GetQuotaFissaEnte " + ex.Message)
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function


    ''' <summary>
    ''' Estrae le quote fisse configurate per l'Ente in esame.
    ''' Può estrarre tutte le quote fisse oppure le quote fisse in base ad un anno, ad un ID, ad un tipo di contatore o/e un intervallo 
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoQuotaFissa</param>
    ''' <returns>array di oggetti di tipo OggettoQuotaFissa</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetQuotaFissaEnte(ByVal obj As OggettoQuotaFissa) As OggettoQuotaFissa()
        Dim arrayListoDatiRata As New ArrayList
        Dim sSQL As String

        Try
            sSQL = "SELECT TP_QUOTA_FISSA.ANNO, TP_TIPIUTENZA.DESCRIZIONE, TP_QUOTA_FISSA.DA, TP_QUOTA_FISSA.A, TP_QUOTA_FISSA.IMPORTOH2O, TP_QUOTA_FISSA.IMPORTODEP, TP_QUOTA_FISSA.IMPORTOFOG, "
            sSQL += "TP_QUOTA_FISSA.ALIQUOTA, TP_QUOTA_FISSA.ID_TIPO_UTENZA, TP_QUOTA_FISSA.ID, TP_QUOTA_FISSA.IDENTE, TP_QUOTA_FISSA.ISAGIORNI"
            sSQL += " FROM TP_QUOTA_FISSA"
            sSQL += " INNER JOIN TP_TIPIUTENZA ON TP_QUOTA_FISSA.ID_TIPO_UTENZA = TP_TIPIUTENZA.IDTIPOUTENZA"
            sSQL += " WHERE TP_QUOTA_FISSA.IDENTE='" & ConstSession.IdEnte & "'"
            If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
                sSQL += " AND TP_QUOTA_FISSA.ID=" & obj.ID & ""
            End If
            If obj.idTipoUtenza.CompareTo(-1) <> 0 And obj.idTipoUtenza.CompareTo(0) <> 0 Then
                sSQL += " AND TP_TIPIUTENZA.IDTIPOUTENZA=" & obj.idTipoUtenza
            End If
            If obj.sAnno.CompareTo("") <> 0 And obj.sAnno.CompareTo("").ToString() <> "..." Then
                sSQL += " AND TP_QUOTA_FISSA.ANNO='" & obj.sAnno.Replace("'", "''") & "'"
            End If
            If obj.DA.CompareTo(0) <> 0 Then
                sSQL += " AND TP_QUOTA_FISSA.DA=" & obj.DA.ToString().Replace(",", ".") & ""
            End If
            If obj.A.CompareTo(0) <> 0 Then
                sSQL += " AND TP_QUOTA_FISSA.A=" & obj.A.ToString().Replace(",", ".") & ""
            End If

            'eseguo la query
            Dim dsCanoniEnte As DataSet
            Log.Debug("GetQuotaFissaEnte::SQL::" & sSQL)
            dsCanoniEnte = iDB.GetDataSet(sSQL)
            If dsCanoniEnte.Tables(0).Rows.Count > 0 Then
                Dim oDatiQF As OggettoQuotaFissa
                Dim iCount As Integer
                For iCount = 0 To dsCanoniEnte.Tables(0).Rows.Count - 1
                    oDatiQF = New OggettoQuotaFissa

                    oDatiQF.ID = dsCanoniEnte.Tables(0).Rows(iCount)("ID")
                    oDatiQF.idTipoUtenza = dsCanoniEnte.Tables(0).Rows(iCount)("ID_TIPO_UTENZA")
                    oDatiQF.sDescrizioneTU = dsCanoniEnte.Tables(0).Rows(iCount)("DESCRIZIONE")
                    oDatiQF.DA = dsCanoniEnte.Tables(0).Rows(iCount)("DA")
                    oDatiQF.A = dsCanoniEnte.Tables(0).Rows(iCount)("A")
                    oDatiQF.dAliquota = dsCanoniEnte.Tables(0).Rows(iCount)("ALIQUOTA")
                    If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTODEP")) Then
                        oDatiQF.dImportoDep = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTODEP")
                    Else
                        oDatiQF.dImportoDep = 0
                    End If
                    If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOFOG")) Then
                        oDatiQF.dImportoFog = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOFOG")
                    Else
                        oDatiQF.dImportoFog = 0
                    End If
                    If Not IsDBNull(dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOH2O")) Then
                        oDatiQF.dImportoH2O = dsCanoniEnte.Tables(0).Rows(iCount)("IMPORTOH2O")
                    Else
                        oDatiQF.dImportoH2O = 0
                    End If
                    oDatiQF.sAnno = dsCanoniEnte.Tables(0).Rows(iCount)("ANNO")
                    oDatiQF.sIdEnte = dsCanoniEnte.Tables(0).Rows(iCount)("IDENTE")
                    oDatiQF.bIsAGiorni = CBool(dsCanoniEnte.Tables(0).Rows(iCount)("ISAGIORNI"))
                    arrayListoDatiRata.Add(oDatiQF)
                Next
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.GetQuotaFissaEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetQuotaFissaEnte " + ex.Message)
        End Try
        Return CType(arrayListoDatiRata.ToArray(GetType(OggettoQuotaFissa)), OggettoQuotaFissa())
    End Function

    ''' <summary>
    ''' Verifica se una quota fissa condifurata è già utilizzata per il calcolo delle fatture.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoQuotaFissa</param>
    ''' <returns>Dataset</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetQuotaFissaAssociati(ByVal obj As OggettoQuotaFissa) As DataSet
        Try
            Dim sSQL As String

            sSQL = "SELECT * FROM TP_FATTURE_NOTE_QUOTA_FISSA"
            sSQL +=" WHERE TP_FATTURE_NOTE_QUOTA_FISSA.IDENTE='" & ConstSession.IdEnte & "'"
            If obj.ID <> 0 Then
                SSQL+=" AND TP_FATTURE_NOTE_QUOTA_FISSA.ID_QUOTAFISSA=" & obj.ID
            End If
            'eseguo la query
            Dim dsQuotaFissaAssociati As DataSet
            dsQuotaFissaAssociati = iDB.GetDataSet(sSQL)
            Return dsQuotaFissaAssociati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.GetQuotaFissaAssociati.errore: ", ex)

            Throw New Exception("Problemi nell'esecuzione di GetQuotaFissaAssociati " + ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Inserisce una quota fissa.
    ''' </summary>
    ''' <param name="obj">un oggetto di tipo OggettoQuotaFissa</param>
    ''' <param name="strError"></param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function SetQuotaFissaEnte(ByVal obj As OggettoQuotaFissa, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try
            Dim ds As OggettoQuotaFissa()
            ds = GetQuotaFissaEnte(obj)
            If Not ds Is Nothing Then
                If ds.Length > 0 Then
                    strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                    SetQuotaFissaEnte = False
                    Exit Function
                End If
            End If

            Dim sSQL As String
            sSQL = "INSERT INTO TP_QUOTA_FISSA"
            sSQL +=" (IDENTE, ANNO, ID_TIPO_UTENZA, IMPORTOH2O, IMPORTOFOG, IMPORTODEP, ALIQUOTA, DA, A, ISAGIORNI)"
            SSQL+=" VALUES('" & ConstSession.IdEnte & "','" & obj.sAnno.Replace("'", "''") & "'," & obj.idTipoUtenza & "," & obj.dImportoH2O.ToString.Replace(",", ".") & "," & obj.dImportoFog.ToString.Replace(",", ".") & "," & obj.dImportoDep.ToString.Replace(",", ".") & "," & obj.dAliquota.ToString.Replace(",", ".") & "," & obj.DA.ToString.Replace(",", ".") & "," & obj.A.ToString.Replace(",", ".") & "," & CInt(obj.bIsAGiorni) & ")"
            'eseguo la query
            myIdentity = iDB.ExecuteNonQuery(sSQL)

            Return True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.SetQuotaFissaEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di SetScaglioniEnte " + ex.Message)
            SetQuotaFissaEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Esegue l'aggiornamento di una determinata quota fissa configurata già a sistema SOLO nel caso
    ''' in cui non è ancora utilizzata dalla fatturazione.
    ''' </summary>
    ''' <param name="objOld">oggetto di tipo OggettoQuotaFissa precedente</param>
    ''' <param name="objNew">oggetto di tipo OggettoQuotaFissa attuale</param>
    ''' <param name="strError"></param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function UpdateQuotaFissaEnte(ByVal sCodEnte As String, ByVal objOld As OggettoQuotaFissa, ByVal objNew As OggettoQuotaFissa, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Dim sSQL As String

        Try
            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA QUOTA FISSA NON SIA STATA ASSOCIATA
            Dim DsQuotaFissa As DataSet
            DsQuotaFissa = GetQuotaFissaAssociati(objOld)
            If DsQuotaFissa.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                UpdateQuotaFissaEnte = False
                Exit Function
            End If

            'PRELEVO I DATI DELLO SCAGLIONE DA MODIFICARE
            Dim DST As OggettoQuotaFissa()
            DST = GetQuotaFissaEnte(objOld)
            If DST.Length > 0 Then
                objOld.ID = DST(0).ID
                objOld.idTipoUtenza = DST(0).idTipoUtenza
                objOld.sAnno = DST(0).sAnno
                objOld.dImportoDep = DST(0).dImportoDep
                objOld.dImportoFog = DST(0).dImportoFog
                objOld.dImportoH2O = DST(0).dImportoH2O
                objOld.dAliquota = DST(0).dAliquota
                objOld.DA = DST(0).DA
                objOld.A = DST(0).A
                objOld.bIsAGiorni = DST(0).bIsAGiorni
            Else
                strError = "Non sono presenti dati per la Quota Fissa Selezionata."
                UpdateQuotaFissaEnte = False
                Exit Function
            End If

            If (objNew.dAliquota <> objOld.dAliquota Or objNew.dImportoDep <> objOld.dImportoDep Or objNew.dImportoFog <> objOld.dImportoFog Or objNew.dImportoH2O <> objOld.dImportoH2O Or objNew.DA <> objOld.DA Or objNew.A <> objOld.A Or objOld.sAnno = objNew.sAnno Or objOld.idTipoUtenza = objNew.idTipoUtenza Or objOld.bIsAGiorni <> objNew.bIsAGiorni) Then
                'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
                sSQL = "UPDATE TP_QUOTA_FISSA SET IMPORTOH2O=" & objNew.dImportoH2O.ToString.Replace(",", ".") & ","
                sSQL += " IMPORTOFOG=" & objNew.dImportoFog.ToString.Replace(",", ".") & ","
                sSQL += " IMPORTODEP=" & objNew.dImportoDep.ToString.Replace(",", ".") & ","
                sSQL += " ANNO='" & objNew.sAnno & "',"
                sSQL += " ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
                sSQL += " ID_TIPO_UTENZA=" & objNew.idTipoUtenza & ","
                sSQL += " DA=" & objNew.DA.ToString.Replace(",", ".") & ","
                sSQL += " A=" & objNew.A.ToString.Replace(",", ".") & ","
                sSQL += " ISAGIORNI=" & CInt(objNew.bIsAGiorni)
                sSQL += " WHERE (ID=" & objNew.ID & ")"
                sSQL += " AND (IDENTE='" & sCodEnte & "')"
                'eseguo la query
                myIdentity = iDB.ExecuteNonQuery(sSQL)
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.UpdateQuotaFissaEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di UpdateQuotaFissaEnte " + ex.Message)
            UpdateQuotaFissaEnte = False
        End Try
    End Function

    ''' <summary>
    ''' Esegue l'eliminazione di una determinata quota fissa configurata già a sistema SOLO nel caso
    ''' in cui non è ancora utilizzata dalla fatturazione.
    ''' </summary>
    ''' <param name="OBJ">oggetto di tipo OggettoQuotaFissa</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function DeleteQuotaFissaEnte(ByVal OBJ As OggettoQuotaFissa, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Dim SQL As String

        Try
            DeleteQuotaFissaEnte = False

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA QUOTA FISSA NON SIA STATA ASSOCIATA
            Dim DsQuotaFissa As DataSet
            DsQuotaFissa = GetQuotaFissaAssociati(OBJ)
            If DsQuotaFissa.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                DeleteQuotaFissaEnte = False
                Exit Function
            End If

            'SE NON è STATA ASSOCIATA LA CANCELLO
            SQL = "DELETE FROM TP_QUOTA_FISSA"
            SQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""
            'eseguo la query
            myIdentity = CInt(iDB.ExecuteNonQuery(SQL))

            DeleteQuotaFissaEnte = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsQuotaFissa.DeleteQuotaFissaEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeleteQuotaFissaEnte " + ex.Message)
            DeleteQuotaFissaEnte = False
        End Try
    End Function
End Class
