Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Public Class ClsCanoni
    ''' <summary>
    ''' Gestione dei canoni per l'Ente
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsCanoni))
    Private iDB As New DBAccess.getDBobject
    Dim clsGenerale As New ClsGenerale.Generale

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Estrae i canoni configurati per l'Ente in esame.
    ''' Può estrarre tutti i canoni oppure un canone specifico se viene passato l'ID della tabella, il tipo, la tipologia dell'utenza, l'anno tramite l'oggetto OggettoCanone.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoCanone</param>
    ''' <param name="sOperazione">stringa che indica se filtrare anche per id univoco</param>
    ''' <returns>lista di oggetti di tipo OggettoCanone</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function GetCanoniEnte(ByVal obj As OggettoCanone, Optional ByVal sOperazione As String = "") As OggettoCanone()
        Dim oDatiCanone As OggettoCanone
        Dim arrayListoDatiRata As New ArrayList
        Dim DsDati As New DataSet
        Dim cmdMyCommand As New SqlCommand

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetCanoniEnte"
            cmdMyCommand.Parameters.Add(New SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = obj.sIdEnte
            cmdMyCommand.Parameters.Add(New SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = obj.sAnno
            cmdMyCommand.Parameters.Add(New SqlParameter("@AZIONE", SqlDbType.NVarChar)).Value = sOperazione
            cmdMyCommand.Parameters.Add(New SqlParameter("@ID", SqlDbType.NVarChar)).Value = obj.ID
            cmdMyCommand.Parameters.Add(New SqlParameter("@IDTIPOCANONE", SqlDbType.NVarChar)).Value = obj.idTipoCanone
            cmdMyCommand.Parameters.Add(New SqlParameter("@IDTIPOUTENZA", SqlDbType.NVarChar)).Value = obj.idTipoUtenza
            Log.Debug("GetCanoniEnte::SQL::" & cmdMyCommand.CommandText)
            DsDati = iDB.GetDataSet(cmdMyCommand)
            For Each myRow As DataRow In DsDati.Tables(0).Rows
                oDatiCanone = New OggettoCanone

                oDatiCanone.ID = myRow("ID_CANONE_ENTE")
                oDatiCanone.idTipoCanone = myRow("ID_TIPO_CANONE")
                oDatiCanone.idTipoUtenza = myRow("IDTIPOUTENZA")
                oDatiCanone.idServizio = myRow("IDSERVIZIO")
                oDatiCanone.sDescrizioneTC = myRow("DESCRIZIONE_CANONE")
                oDatiCanone.sDescrizioneTU = myRow("DESCRIZIONE_UTENZA")
                oDatiCanone.dAliquota = myRow("ALIQUOTA")
                oDatiCanone.dPercentualeSuConsumo = myRow("PERCENTUALE_SUL_CONSUMO")
                oDatiCanone.dTariffa = myRow("TARIFFA")
                oDatiCanone.sAnno = myRow("ANNO")
                oDatiCanone.sIdEnte = myRow("IDENTE")
                arrayListoDatiRata.Add(oDatiCanone)
            Next
            Return CType(arrayListoDatiRata.ToArray(GetType(OggettoCanone)), OggettoCanone())
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsCanoni.GetCanoniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetCanoniEnte " + ex.Message)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Verifica se un canone configurato è già utilizzato per il calcolo delle fatture.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoCanone</param>
    ''' <returns>Dataset</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function GetCanoniAssociati(ByVal obj As OggettoCanone) As DataSet
        Dim sSQL As String
        Dim DsDati As New DataSet

        Try
            sSQL = "SELECT * FROM TP_FATTURE_NOTE_CANONI"
            sSQL +=" WHERE TP_FATTURE_NOTE_CANONI.IDENTE='" & ConstSession.IdEnte & "'"
            If obj.ID <> 0 Then
                SSQL += " AND TP_FATTURE_NOTE_CANONI.ID_CANONE=" & obj.ID
            End If
            DsDati = iDB.GetDataSet(sSQL)
            Return DsDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsCanoni.GetCanoniAssociati.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetCanoniAssociati " + ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Inserisce/modifica un canone.
    ''' Esegue l'aggiornamento di un determinato canone configurato già a sistema SOLO nel caso in cui non è ancora utilizzato dalla fatturazione.
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoCanone attuale</param>
    ''' <param name="objOld">oggetto di tipo OggettoCanone precedente</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function SetCanoniEnte(ByVal obj As OggettoCanone, ByVal objOld As OggettoCanone, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Dim nId As Integer = -1

        Try
            If Not objOld Is Nothing Then
                Dim DsCanone As DataSet
                DsCanone = GetCanoniAssociati(objOld)
                If DsCanone.Tables(0).Rows.Count > 0 Then
                    'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                    strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                    Return False
                End If

                'PRELEVO I DATI DELLA TARIFFA DA MODIFICARE
                Dim DST As OggettoCanone()
                DST = GetCanoniEnte(objOld)
                If DST.Length > 0 Then
                    objOld.ID = DST(0).ID
                    objOld.idTipoCanone = DST(0).idTipoCanone
                    objOld.idTipoUtenza = DST(0).idTipoUtenza
                    objOld.sAnno = DST(0).sAnno
                    objOld.dPercentualeSuConsumo = DST(0).dPercentualeSuConsumo
                    objOld.dTariffa = DST(0).dTariffa
                    objOld.dAliquota = DST(0).dAliquota
                Else
                    strError = "Non sono presenti dati per il Canone Selezionato."
                    Return False
                End If

                If (obj.dAliquota <> objOld.dAliquota Or obj.dPercentualeSuConsumo <> objOld.dPercentualeSuConsumo Or obj.dTariffa <> objOld.dTariffa Or objOld.sAnno <> obj.sAnno Or objOld.idTipoCanone <> obj.idTipoCanone Or objOld.idTipoUtenza <> obj.idTipoUtenza) Then
                    Dim ds As OggettoCanone()
                    ds = GetCanoniEnte(obj, "modifica")
                    If Not ds Is Nothing Then
                        If ds.Length > 0 Then
                            strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
                            Return False
                        End If
                    End If
                    'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
                    nId = obj.ID
                    myIdentity = iDB.ExecuteNonQuery("prc_TP_CANONI_IU", New SqlParameter("@ID", nId) _
                        , New SqlParameter("@IDENTE", obj.sIdEnte) _
                        , New SqlParameter("@ANNO", obj.sAnno) _
                        , New SqlParameter("@ID_TIPO_CANONE", obj.idTipoCanone) _
                        , New SqlParameter("@ID_TIPO_UTENZA", obj.idTipoUtenza) _
                        , New SqlParameter("@TARIFFA", obj.dTariffa) _
                        , New SqlParameter("@ALIQUOTA", obj.dAliquota) _
                        , New SqlParameter("@PERCENTUALE_SUL_CONSUMO", obj.dPercentualeSuConsumo))
                End If
            Else
                Dim ds As OggettoCanone()
                ds = GetCanoniEnte(obj)
                If Not ds Is Nothing Then
                    If ds.Length > 0 Then
                        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                        Return False
                    End If
                End If

                myIdentity = iDB.ExecuteNonQuery("prc_TP_CANONI_IU", New SqlParameter("@ID", nId) _
                    , New SqlParameter("@IDENTE", obj.sIdEnte) _
                    , New SqlParameter("@ANNO", obj.sAnno) _
                    , New SqlParameter("@ID_TIPO_CANONE", obj.idTipoCanone) _
                    , New SqlParameter("@ID_TIPO_UTENZA", obj.idTipoUtenza) _
                    , New SqlParameter("@TARIFFA", obj.dTariffa) _
                    , New SqlParameter("@ALIQUOTA", obj.dAliquota) _
                    , New SqlParameter("@PERCENTUALE_SUL_CONSUMO", obj.dPercentualeSuConsumo))
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsCanoni.SetCanoniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di SetCanoniEnte " + ex.Message)
        End Try
    End Function

    'Public Function UpdateCanoniEnte(ByVal objOld As OggettoCanone, ByVal objNew As OggettoCanone, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    Try
    '        UpdateCanoniEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE IL CANONE NON SIA STATO ASSOCIATO
    '        Dim DsCanone As DataSet
    '        DsCanone = GetCanoniAssociati(objOld)

    '        If DsCanone.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
    '            UpdateCanoniEnte = False
    '            Exit Function
    '        End If

    '        'PRELEVO I DATI DELLA TARIFFA DA MODIFICARE
    '        Dim DST As OggettoCanone()

    '        DST = GetCanoniEnte(objOld)
    '        If DST.Length > 0 Then
    '            objOld.ID = DST(0).ID
    '            objOld.idTipoCanone = DST(0).idTipoCanone
    '            objOld.idTipoUtenza = DST(0).idTipoUtenza
    '            objOld.sAnno = DST(0).sAnno
    '            objOld.dPercentualeSuConsumo = DST(0).dPercentualeSuConsumo
    '            objOld.dTariffa = DST(0).dTariffa
    '            objOld.dAliquota = DST(0).dAliquota
    '        Else
    '            strError = "Non sono presenti dati per il Canone Selezionato."
    '            UpdateCanoniEnte = False
    '            Exit Function
    '        End If

    '        If (objNew.dAliquota <> objOld.dAliquota Or objNew.dPercentualeSuConsumo <> objOld.dPercentualeSuConsumo Or objNew.dTariffa <> objOld.dTariffa Or objOld.sAnno <> objNew.sAnno Or objOld.idTipoCanone <> objNew.idTipoCanone Or objOld.idTipoUtenza <> objNew.idTipoUtenza) Then
    '            Dim ds As OggettoCanone()
    '            ds = GetCanoniEnte(objNew, "modifica")
    '            If Not ds Is Nothing Then
    '                If ds.Length > 0 Then
    '                    strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '                    UpdateCanoniEnte = False
    '                    Exit Function
    '                End If
    '            End If
    '            'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '            SQL = "UPDATE TP_CANONI SET TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
    '            SSQL+=" ANNO='" & objNew.sAnno & "',"
    '            SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '            SSQL+=" PERCENTUALE_SUL_CONSUMO=" & objNew.dPercentualeSuConsumo.ToString.Replace(",", ".") & ","
    '            SSQL+=" ID_TIPO_CANONE=" & objNew.idTipoCanone & ","
    '            SSQL+=" ID_TIPO_UTENZA=" & objNew.idTipoUtenza & ""
    '            SSQL+=" WHERE ID=" & objNew.ID & ""
    '            SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
    '        End If

    '        'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
    '        'If objOld.sAnno <> objNew.sAnno Then 'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then
    '        '    'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
    '        '    'se è presente blocco l'operazione
    '        '    Dim ds As OggettoCanone()
    '        '    ds = GetCanoniEnte(objNew)
    '        '    If ds.Length > 0 Then
    '        '        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '        '        UpdateCanoniEnte = False
    '        '        Exit Function
    '        '    End If

    '        '    'verifico se è già presente una tariffa con gli stessi dati storicizzata in data odierna
    '        '    'se è presente do il messaggio e blocco l'operazione
    '        '    'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
    '        '    SQL = "UPDATE TP_CANONI SET ANNO='" & objNew.sAnno & "',"
    '        '    SSQL+=" TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" PERCENTUALE_SUL_CONSUMO=" & objNew.dPercentualeSuConsumo.ToString.Replace(",", ".") & ""
    '        '    SSQL+=" WHERE TP_CANONI.ID=" & objOld.ID & ""
    '        '    SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '        '    'eseguo la query
    '        '    myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        'ElseIf (objNew.dAliquota <> objOld.dAliquota Or objNew.dPercentualeSuConsumo <> objOld.dPercentualeSuConsumo Or objNew.dTariffa <> objOld.dTariffa) And objOld.sAnno = objNew.sAnno Then
    '        '    'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '        '    'SQL = "UPDATE TBLADDIZIONALIENTE SET VALORE='" & objNew.sValore.Replace(",", ".") & "'"
    '        '    SQL = "UPDATE TP_CANONI SET TARIFFA=" & objNew.dTariffa.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" ALIQUOTA=" & objNew.dAliquota.ToString.Replace(",", ".") & ","
    '        '    SSQL+=" PERCENTUALE_SUL_CONSUMO=" & objNew.dPercentualeSuConsumo.ToString.Replace(",", ".") & ""
    '        '    SSQL+=" WHERE ID=" & objNew.ID & ""
    '        '    SSQL+=" AND IDENTE='" & ConstSession.IdEnte & "'"
    '        '    'eseguo la query
    '        '    myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
    '        'End If
    '        UpdateCanoniEnte = True
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCanoni.UpdateCanoniEnte.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di UpdateCanoniEnte " + ex.Message)
    '        UpdateCanoniEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Esegue l'eliminazione di un determinato canone configurato già a sistema SOLO nel caso in cui non è ancora utilizzato dalla fatturazione.
    ''' </summary>
    ''' <param name="OBJ">oggetto di tipo OggettoCanone</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'aggiornamento è andato a buon fine, FALSE se l'aggiornamento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Function DeleteCanoniEnte(ByVal OBJ As OggettoCanone, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Try
            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE L' ADDIZIONALE NON SIA STATO ASSOCIATO
            Dim DsAddizionale As DataSet
            DsAddizionale = GetCanoniAssociati(OBJ)
            If DsAddizionale.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata alla Fatturazione!"
                Return False
            End If

            myIdentity = iDB.ExecuteNonQuery("prc_TP_CANONI_D", Nothing, New SqlParameter("@ID", OBJ.ID))
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsCanoni.DeleteCanoniEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeleteCanoniEnte " + ex.Message)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Estrae le voci dei canoni configurati a livello generale e carica un oggetto di tipo DropDownList(combo)
    ''' </summary>
    ''' <param name="ddlCanoni">oggetto da popolare</param>
    ''' <remarks>
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Sub LoadComboCanoni(ByVal ddlCanoni As DropDownList)
        Try
            Dim SQL As String

            SQL = "SELECT DESCRIZIONE AS DESCRIZIONE, ID_TIPO_CANONE FROM TP_TIPOLOGIE_CANONI"
            '            SSQL+=" WHERE IDENTE='" & ConstSession.IdEnte & "'"
            'eseguo la query
            clsGenerale.LoadComboGenerale(ddlCanoni, SQL)
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsCanoni.LoadComboCanoni.errore: ", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Estrae le voci delle tipologie di utenza configurate a livello generale e carica un oggetto di tipo DropDownList(combo).
    ''' </summary>
    ''' <param name="ddlUtenze">oggetto da popolare</param>
    ''' <param name="Anno">anno di validita da estrarre</param>
    Public Sub LoadComboUtenze(ByVal ddlUtenze As DropDownList, ByVal Anno As String)
        Try
            If Anno <> "" Then
                Dim sSQL As String

                sSQL = "SELECT DESCRIZIONE AS DESCRIZIONE, IDTIPOUTENZA FROM TP_TIPIUTENZA"
                sSQL +=" WHERE COD_ENTE='" & ConstSession.IdEnte & "' AND ((CASE WHEN YEAR(AL) IS NULL THEN '9999' ELSE YEAR(AL) END)>=" & Anno & ") AND (YEAR(DAL)<=" & Anno & ")"
                'eseguo la query
                clsGenerale.LoadComboGenerale(ddlUtenze, sSQL)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsCanoni.LoadComboUtenze.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di LoadComboUtenze " + ex.Message)
        End Try
    End Sub
End Class
