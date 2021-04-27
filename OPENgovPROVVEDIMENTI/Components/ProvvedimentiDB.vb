Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.HttpContext
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Collections
Imports ComPlusInterface
Imports log4net
Imports Utility

Namespace DBPROVVEDIMENTI

    ''' <summary>
    ''' Classe Business/Data Logic che incapsula tutti i dati logici necessari e reperibili da l database OPENgovProvvedimenti.
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class ProvvedimentiDB
        Private Shared Log As ILog = LogManager.GetLogger(GetType(ProvvedimentiDB))
        'Dim objSessione As OPENUtility.CreateSessione
        Dim objUtility As MyUtility
        Private idProcedimento, idProvvedimento As Long
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cod_contribuente"></param>
        ''' <param name="anno"></param>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strCODTRIBUTO"></param>
        ''' <returns></returns>
        Public Function checkSePossibileRiaccertare(ByVal cod_contribuente As String, ByVal anno As String, ByVal strCODENTE As String, ByVal strCODTRIBUTO As String) As Boolean
            Dim cmdMyCommand As New SqlCommand
            Try
                Dim sSQL As String
                'Creazione di una istanza all' oggetto DBManager
                sSQL = "SELECT PROVVEDIMENTI.ID_PROVVEDIMENTO,TAB_PROCEDIMENTI.ID_PROCEDIMENTO FROM PROVVEDIMENTI INNER JOIN TAB_PROCEDIMENTI ON PROVVEDIMENTI.ID_PROVVEDIMENTO = TAB_PROCEDIMENTI.ID_PROVVEDIMENTO"
                sSQL += " WHERE TAB_PROCEDIMENTI.COD_ENTE='" & strCODENTE & "' AND TAB_PROCEDIMENTI.COD_TRIBUTO='" & strCODTRIBUTO & "'"
                sSQL += " AND TAB_PROCEDIMENTI.COD_CONTRIBUENTE = '" & cod_contribuente & "' AND TAB_PROCEDIMENTI.ANNO ='" & anno & "'"
                sSQL += " AND TAB_PROCEDIMENTI.COD_TIPO_PROCEDIMENTO = 'A' AND NOT DATA_ATTO_DEFINITIVO IS NULL"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()

                While result.Read()
                    idProcedimento = result("id_procedimento")
                    idProvvedimento = result("id_provvedimento")
                End While
                result.Close()

                sSQL = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED "
                sSQL += "SELECT * FROM TP_SITUAZIONE_FINALE_ICI (READPAST) WHERE ID_PROCEDIMENTO = " & idProcedimento & " AND RITORNATA = 0"
                Dim result1 As SqlDataReader
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result1 = cmdMyCommand.ExecuteReader()

                While result1.Read()
                    result1.Close()
                    Return False
                End While

                result1.Close()
                Return True
            Catch err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.checkSelPossibileRiaccertare.errore: ", err)
                Throw New Exception(err.Message)
            Finally
                cmdMyCommand.Dispose()
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cod_contribuente"></param>
        ''' <param name="anno"></param>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strCODTRIBUTO"></param>
        Public Sub deleteOldAccertamento(ByVal cod_contribuente As String, ByVal anno As String, ByVal strCODENTE As String, ByVal strCODTRIBUTO As String)
            Dim sSQL As String
            Dim cmdMyCommand As New SqlCommand
            Try
                If checkSePossibileRiaccertare(cod_contribuente, anno, strCODENTE, strCODTRIBUTO) = True And idProvvedimento <> 0 Then
                    sSQL = "DELETE FROM TP_SITUAZIONE_FINALE WHERE ID_PROCEDIMENTO = " & idProcedimento & " AND RITORNATA = 0"
                    cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                    cmdMyCommand.CommandTimeout = 0
                    If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                        cmdMyCommand.Connection.Open()
                    End If
                    cmdMyCommand.CommandType = CommandType.Text
                    cmdMyCommand.CommandText = sSQL
                    cmdMyCommand.Parameters.Clear()
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    'objSessione.oSession.oAppDB.Execute(sSQL)

                    'Cancello in Provvedimenti e a cascata vengono cancellati tutti i dati relazionati 
                    sSQL = "DELETE FROM PROVVEDIMENTI WHERE ID_PROVVEDIMENTO = " & idProvvedimento
                    cmdMyCommand.CommandText = sSQL
                    cmdMyCommand.Parameters.Clear()
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    'objSessione.oSession.oAppDB.Execute(sSQL)

                    'If Not IsNothing(objSessione) Then
                    '    objSessione.Kill()
                    '    objSessione = Nothing
                    'End If
                End If
            Catch err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.deleteOldAccertamento.errore: ", err)
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSanzione"></param>
        ''' <returns></returns>
        Public Function getMotivazioni(ByVal idSanzione As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Dim result As SqlDataReader
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                'Creazione di una istanza all' oggetto DBManager
                Dim SQL As String
                SQL = "SELECT  DISTINCT DESCRIZIONE_MOTIVAZIONE"
                SQL = SQL & " FROM TAB_MOTIVAZIONI INNER JOIN"
                SQL = SQL & " TIPO_VOCI ON TAB_MOTIVAZIONI.COD_VOCE = TIPO_VOCI.COD_VOCE "
                SQL = SQL & " AND TAB_MOTIVAZIONI.COD_TRIBUTO = TIPO_VOCI.COD_TRIBUTO AND TAB_MOTIVAZIONI.COD_ENTE = TIPO_VOCI.COD_ENTE"
                SQL = SQL & " WHERE "
                SQL = SQL & " (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = " & OggettoAtto.Provvedimento.AccertamentoUfficio
                SQL = SQL & " OR TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = " & OggettoAtto.Provvedimento.AccertamentoRettifica & ")"
                SQL = SQL & " AND TIPO_VOCI.COD_CAPITOLO ='" & OggettoAtto.Capitolo.Sanzioni & "'"
                SQL = SQL & " AND TAB_MOTIVAZIONI.COD_VOCE = " & idSanzione
                SQL = SQL & " AND TAB_MOTIVAZIONI.COD_ENTE = '" & ConstSession.IdEnte & "'"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = SQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader()


            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getMotivazioni.errore: ", Err)
            Finally
                cmdMyCommand.Dispose()
            End Try
            Return Result

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSanzione"></param>
        ''' <param name="id_provvedimento"></param>
        ''' <returns></returns>
        Public Function getMotivazioni(ByVal idSanzione As String, ByVal id_provvedimento As Integer) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                'Creazione di una istanza all' oggetto DBManager
                Dim SQL As String
                SQL = "SELECT MOTIVAZIONE as DESCRIZIONE_MOTIVAZIONE "
                SQL += " FROM DETTAGLIO_VOCI_ACCERTAMENTI"
                SQL += " where id_provvedimento = " & id_provvedimento
                SQL += " and cod_voce='" & idSanzione & "'"
                'Ritorna come risultato un datareader 
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = SQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()

                Return result

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getMotivazioni.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                cmdMyCommand.Dispose()
            End Try

        End Function


        'Tipologie di Sanzioni per accertamento
        'Public Function GetTipologieSanzioni(Optional ByVal cod_voce As String = "") As DataSet
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        'Creazione di una istanza all' oggetto DBManager
        '        Dim objDBApp As New DBManager
        '        Dim SQL As String
        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        'SQL = " SELECT DISTINCT TIPO_VOCI.COD_VOCE,TIPO_VOCI.COD_TIPO_PROVVEDIMENTO, TIPOLOGIE_SANZIONI.DESCRIZIONE, TIPO_VOCI.COD_CAPITOLO, '' AS MOTIVAZIONE,'0' as CHECKSANZIONE"
        '        'SQL = SQL & " FROM TIPOLOGIE_SANZIONI INNER JOIN TIPO_VOCI ON TIPOLOGIE_SANZIONI.COD_VOCE = TIPO_VOCI.COD_VOCE AND TIPOLOGIE_SANZIONI.COD_TRIBUTO = TIPO_VOCI.COD_TRIBUTO"
        '        'SQL = SQL & " WHERE (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = 3) AND (TIPO_VOCI.COD_CAPITOLO = N'0002') OR (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = 4) AND (TIPO_VOCI.COD_CAPITOLO = N'0002')"

        '        'AleP 13112007
        '        SQL = " SELECT DISTINCT TIPO_VOCI.COD_VOCE,TIPO_VOCI.COD_TIPO_PROVVEDIMENTO, TIPOLOGIE_SANZIONI.DESCRIZIONE, TIPO_VOCI.COD_CAPITOLO, '' AS MOTIVAZIONE,'0' as CHECKSANZIONE"
        '        SQL = SQL & " FROM TIPOLOGIE_SANZIONI INNER JOIN TIPO_VOCI ON TIPOLOGIE_SANZIONI.COD_VOCE = TIPO_VOCI.COD_VOCE AND TIPOLOGIE_SANZIONI.COD_TRIBUTO = TIPO_VOCI.COD_TRIBUTO "
        '        SQL = SQL & " AND TIPOLOGIE_SANZIONI.COD_ENTE = TIPO_VOCI.COD_ENTE "
        '        SQL = SQL & " WHERE ((TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = " & Costanti.PROVVEDIMENTO_ACCERTAMENTO_UFFICIO & " AND TIPO_VOCI.COD_CAPITOLO = '" & Costanti.COD_CAPITOLO_SANZIONE & "' AND TIPOLOGIE_SANZIONI.COD_ENTE='" & ConstSession.IdEnte & "' AND (TIPO_VOCI.FASE=-1) AND TIPO_VOCI.COD_TRIBUTO='" & HttpContext.Current.Session("COD_TRIBUTO") & "')"
        '        SQL = SQL & " OR (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = " & Costanti.PROVVEDIMENTO_ACCERTAMENTO_RETTIFICA & " AND TIPO_VOCI.COD_CAPITOLO = '" & Costanti.COD_CAPITOLO_SANZIONE & "' AND TIPOLOGIE_SANZIONI.COD_ENTE='" & ConstSession.IdEnte & "' AND (TIPO_VOCI.FASE=-1) AND TIPO_VOCI.COD_TRIBUTO='" & HttpContext.Current.Session("COD_TRIBUTO") & "')"
        '        SQL = SQL & " OR (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = " & Costanti.PROVVEDIMENTO_RIMBORSO & " AND TIPO_VOCI.COD_CAPITOLO = '" & Costanti.COD_CAPITOLO_SANZIONE & "' AND TIPOLOGIE_SANZIONI.COD_ENTE='" & ConstSession.IdEnte & "' AND (TIPO_VOCI.FASE=-1) AND TIPO_VOCI.COD_TRIBUTO='" & HttpContext.Current.Session("COD_TRIBUTO") & "'))"
        '        If cod_voce <> "" Then
        '            'Se valorizzato allora devo visualizzare le sole sanzioni applicate all'immobile
        '            'è utilizzata quando viene visualizzato il riepilogo degli immobili accertati
        '            SQL = SQL & " and(TIPO_VOCI.cod_voce='" & cod_voce & "')"
        '        End If
        '        'SQL = "SELECT DISTINCT TIPO_VOCI.COD_VOCE,TIPO_VOCI.COD_TIPO_PROVVEDIMENTO, TIPOLOGIE_SANZIONI.DESCRIZIONE, TIPO_VOCI.COD_CAPITOLO"
        '        'SQL += " , TIPO_VOCI.DESCRIZIONE_VOCE_ATTRIBUITA, SUM(DETTAGLIO_VOCI_ACCERTAMENTI.IMPORTO) AS TOT_IMPORTO_SANZ ,VALORE_VOCI.VALORE "
        '        'SQL += " , MOTIVAZIONE"
        '        'SQL += " FROM DETTAGLIO_VOCI_ACCERTAMENTI"
        '        'SQL += " INNER JOIN TIPO_VOCI ON DETTAGLIO_VOCI_ACCERTAMENTI.COD_ENTE = TIPO_VOCI.COD_ENTE"
        '        'SQL += " 	AND DETTAGLIO_VOCI_ACCERTAMENTI.COD_VOCE = TIPO_VOCI.COD_VOCE"
        '        'SQL += " INNER JOIN VALORE_VOCI ON DETTAGLIO_VOCI_ACCERTAMENTI.COD_ENTE = VALORE_VOCI.COD_ENTE"
        '        'SQL += " 	AND DETTAGLIO_VOCI_ACCERTAMENTI.COD_VOCE = VALORE_VOCI.COD_VOCE"
        '        'SQL += " 	AND VALORE_VOCI.ID_TIPO_VOCE = TIPO_VOCI.ID_TIPO_VOCE"
        '        'SQL += " INNER JOIN TIPOLOGIE_SANZIONI ON TIPOLOGIE_SANZIONI.COD_VOCE = TIPO_VOCI.COD_VOCE"
        '        'SQL += " 	AND TIPOLOGIE_SANZIONI.COD_TRIBUTO = TIPO_VOCI.COD_TRIBUTO"
        '        'SQL += " 	AND TIPOLOGIE_SANZIONI.COD_ENTE = TIPO_VOCI.COD_ENTE"
        '        'SQL += " WHERE (TIPO_VOCI.COD_CAPITOLO = '" & Costanti.COD_CAPITOLO_SANZIONE & "')"
        '        'SQL += " AND (DETTAGLIO_VOCI_ACCERTAMENTI.ID_PROVVEDIMENTO=" & IdProvvedimento & ")"
        '        'SQL += " GROUP BY TIPO_VOCI.DESCRIZIONE_VOCE_ATTRIBUITA,VALORE_VOCI.VALORE,MOTIVAZIONE"
        '        'SQL += " ,TIPO_VOCI.COD_VOCE,TIPO_VOCI.COD_TIPO_PROVVEDIMENTO, TIPOLOGIE_SANZIONI.DESCRIZIONE, TIPO_VOCI.COD_CAPITOLO"
        '        Dim result As DataSet = objSessione.oSession.oAppDB.GetPrivateDataSet(SQL)
        '        'Ritorna come risultato un datareader 
        '        Return result
        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipologieSanzioni.errore: ", err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="IdTributo"></param>
        ''' <param name="sCodVoce"></param>
        ''' <returns></returns>
        Public Function GetTipologieSanzioni(IdTributo As String, ByVal sCodVoce As String) As DataSet
            Dim cmdMyCommand As New SqlCommand
            Dim myAdapter As New SqlDataAdapter
            Dim myDataSet As New DataSet

            Try
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "prc_GetTipologieSanzioni"
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.AddWithValue("@IDENTE", ConstSession.IdEnte)
                'per la TASI valgono le stesse configurazioni di ICI quindi sostituisco il codice tributo
                cmdMyCommand.Parameters.AddWithValue("@IDTRIBUTO", IdTributo.Replace(Utility.Costanti.TRIBUTO_TASI, Utility.Costanti.TRIBUTO_ICI))
                cmdMyCommand.Parameters.AddWithValue("@IDVOCE", sCodVoce)
                'Log.Debug("GetTipologieSanzioni::query::" & cmdMyCommand.CommandText & Utility.Costanti.GetValParamCmd(cmdMyCommand))
                myAdapter.SelectCommand = cmdMyCommand
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myAdapter.Fill(myDataSet, "GetTipologieSanzioni")
                Return myDataSet
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipologieSanzioni.errore: ", Err)
                Return Nothing
            Finally
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End Try
        End Function

        ''' <summary>
        ''' Il metodo GetNoteProvvedimenti ritorna un data reader a forward-only, read-only . Mostra la lista di tutti gli elementi contenuti nella tabella TAB_NOTE_PROVVEDIMENTI del database OPENgovProvvedimenti
        ''' </summary>
        ''' <param name="strCODENTE"></param>
        ''' <returns></returns>
        Public Function GetNoteProvvedimenti(ByVal strCODENTE As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = "SELECT * FROM TAB_NOTE_PROVVEDIMENTI WHERE COD_ENTE=" & objUtility.CStrToDB(strCODENTE)
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()

                Return result

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetNoteProvvedimenti.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                cmdMyCommand.Dispose()
            End Try

        End Function

        'Public Function GetAnniProvvedimentiICI(ByVal strCODENTE As String, ByVal strCODTRIBUTO As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If
        '        'forzo il codice tributo a 8852 (ICI)
        '        'dim sSQL as string = "SELECT * FROM ANNI_PROVVEDIMENTI WHERE COD_ENTE=" & objUtility.CStrToDB(strCODENTE) & " AND COD_TRIBUTO=" & objUtility.CStrToDB(strCODTRIBUTO)
        '        dim sSQL as string = "SELECT DISTINCT ANNO FROM ANNI_PROVVEDIMENTI WHERE COD_ENTE='" & strCODENTE & "' AND COD_TRIBUTO='" & strCODTRIBUTO & "' ORDER BY ANNO DESC"
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        'objSessione = Nothing

        '        Return result

        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetAnniProvvedimentiICI.errore: ", err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>Il metodo GetAnniProvvedimentiICI ritorna un data reader a forward-only, read-only . Mostra la lista di tutti gli anni provvedimenti nella tabella ANNI_PROVVEDIMENTI del database OPENgovProvvedimenti
        ''' 
        ''' </summary>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strCODTRIBUTO"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetAnniProvvedimentiICI(ByVal strCODENTE As String, ByVal strCODTRIBUTO As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim result As SqlDataReader
            Try
                'forzo il codice tributo a 8852 (ICI)
                'dim sSQL as string = "SELECT * FROM ANNI_PROVVEDIMENTI WHERE COD_ENTE=" & objUtility.CStrToDB(strCODENTE) & " AND COD_TRIBUTO=" & objUtility.CStrToDB(strCODTRIBUTO)
                Dim sSQL As String = "SELECT DISTINCT ANNO FROM ANNI_PROVVEDIMENTI WHERE COD_ENTE='" & strCODENTE & "' AND COD_TRIBUTO='" & strCODTRIBUTO & "' ORDER BY ANNO DESC"
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetAnniProvvedimentiICI.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strCODTRIBUTO"></param>
        ''' <returns></returns>
        Public Function GetControlVersamentoTardivo(ByVal strCODENTE As String, ByVal strCODTRIBUTO As String) As Boolean
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                'forzo il codice tributo a 8852 (ICI)
                Dim sSQL As String
                sSQL = "SELECT * FROM TIPO_VOCI"
                sSQL += " WHERE (COD_TIPO_PROVVEDIMENTO = " & OggettoAtto.Provvedimento.AccertamentoRettifica & ") AND "

                sSQL += " (COD_TRIBUTO ='" & strCODTRIBUTO & "') AND "
                sSQL += " (COD_CAPITOLO = '" & OggettoAtto.Capitolo.Sanzioni & "') AND "
                sSQL += " (FASE = 1) AND"
                sSQL += " (COD_ENTE = '" & strCODENTE & "')"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()

                If result.Read Then
                    GetControlVersamentoTardivo = True
                Else
                    GetControlVersamentoTardivo = False
                End If

                Return GetControlVersamentoTardivo

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetControlVersamentoTardivo.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                cmdMyCommand.Dispose()
            End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strCODTRIBUTO"></param>
        ''' <param name="strAnnoDA"></param>
        ''' <param name="strAnnoA"></param>
        ''' <returns></returns>
        Public Function GetViewVersamentoTardivo(ByVal strCODENTE As String, ByVal strCODTRIBUTO As String, ByVal strAnnoDA As String, ByVal strAnnoA As String) As DataSet
            Dim cmdMyCommand As New SqlClient.SqlCommand
            Dim myAdapter As New SqlClient.SqlDataAdapter
            Dim dsMyDati As New DataSet
            Dim sSQL As String

            Try
                objUtility = New MyUtility

                sSQL = " SELECT VALORE_VOCI.ANNO, TAB_BASE_RAFFRONTO.DESC_BASE_RAFFRONTO, VALORE_VOCI.PARAMETRO, VALORE_VOCI.CONDIZIONE, TIPO_BASE_CALCOLO.DESCRIZIONE"
                sSQL += " FROM TIPO_VOCI INNER JOIN VALORE_VOCI ON TIPO_VOCI.COD_VOCE = VALORE_VOCI.COD_VOCE AND TIPO_VOCI.COD_CAPITOLO = VALORE_VOCI.COD_CAPITOLO AND "
                sSQL += " TIPO_VOCI.COD_TRIBUTO = VALORE_VOCI.COD_TRIBUTO AND TIPO_VOCI.COD_ENTE = VALORE_VOCI.COD_ENTE INNER JOIN"
                sSQL += " TIPO_BASE_CALCOLO ON VALORE_VOCI.CALCOLATA_SU = TIPO_BASE_CALCOLO.TIPO INNER JOIN"
                sSQL += " TAB_BASE_RAFFRONTO ON VALORE_VOCI.BASE_RAFFRONTO = TAB_BASE_RAFFRONTO.COD_BASE_RAFFRONTO"
                sSQL += " WHERE (TIPO_VOCI.COD_ENTE = '" & strCODENTE & "') AND (TIPO_VOCI.COD_TRIBUTO ='" & strCODTRIBUTO & "') AND (TIPO_VOCI.COD_CAPITOLO = '0002') AND "
                'sSQL+=" (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO = 2) AND (TIPO_VOCI.FASE = '1')"
                sSQL += " (TIPO_VOCI.FASE = '1')"
                sSQL += " AND (VALORE_VOCI.ANNO IN"
                sSQL += "  (SELECT max(anno)"
                sSQL += " FROM TIPO_VOCI INNER JOIN TIPO_MISURA ON TIPO_VOCI.MISURA = TIPO_MISURA.COD_MISURA INNER JOIN"
                sSQL += " VALORE_VOCI ON TIPO_VOCI.COD_TRIBUTO = VALORE_VOCI.COD_TRIBUTO AND TIPO_VOCI.COD_CAPITOLO = VALORE_VOCI.COD_CAPITOLO AND "
                sSQL += " TIPO_VOCI.COD_VOCE = VALORE_VOCI.COD_VOCE INNER JOIN TIPO_BASE_CALCOLO ON VALORE_VOCI.CALCOLATA_SU = TIPO_BASE_CALCOLO.TIPO"
                sSQL += " WHERE  (TIPO_VOCI.COD_ENTE = " & objUtility.CStrToDB(strCODENTE) & ") AND (TIPO_VOCI.COD_CAPITOLO = '0002') AND "
                sSQL += " (TIPO_VOCI.COD_TRIBUTO = " & objUtility.CStrToDB(strCODTRIBUTO) & ") AND (TIPO_VOCI.FASE='1')"
                'If strAnnoDA.CompareTo(strAnnoA) = 0 Then
                sSQL += " AND ANNO <= " & objUtility.CStrToDB(strAnnoDA) & "))"
                'Else

                '    sSQL+=" AND (ANNO >= " & objUtility.CStrToDB(strAnnoDA) & "  AND ANNO <= " & objUtility.CStrToDB(strAnnoA) & ")))"

                'End If
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandTimeout = 0
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL

                myAdapter.SelectCommand = cmdMyCommand
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myAdapter.Fill(dsMyDati, "Create DataView")
                myAdapter.Dispose()
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetViewVersamentoTardivo.errore: ", ex)
            Finally
                cmdMyCommand.Connection.Close()
                cmdMyCommand.Dispose()
            End Try
            Return dsMyDati
        End Function

        'Public Function GetSpese(ByVal strANNO As String, ByVal strCODTRIBUTO As String, ByVal strCODENTE As String, ByVal strCODTIPOPROVVEDIMENTO As String) As Double
        '    dim sSQL as string
        '    Dim strWFErrore As String
        '    Dim objContext As HttpContext = HttpContext.Current
        '    Dim objUtility As New MyUtility
        '    'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '    objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        'Try
        '    If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '        Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '    End If


        '    sSQL=" SELECT DISTINCT VALORE_VOCI.ANNO, VALORE_VOCI.VALORE"
        '    sSQL+=" FROM TIPO_VOCI "
        '    sSQL+=" INNER JOIN TIPO_MISURA ON TIPO_VOCI.MISURA = TIPO_MISURA.COD_MISURA "
        '    sSQL+=" INNER JOIN VALORE_VOCI ON TIPO_VOCI.COD_TRIBUTO = VALORE_VOCI.COD_TRIBUTO "
        '    sSQL+=" AND TIPO_VOCI.COD_CAPITOLO = VALORE_VOCI.COD_CAPITOLO "
        '    sSQL+=" AND TIPO_VOCI.COD_VOCE = VALORE_VOCI.COD_VOCE"
        '    sSQL+=" AND TIPO_VOCI.COD_ENTE = VALORE_VOCI.COD_ENTE "
        '    sSQL+=" WHERE  (VALORE_VOCI.ANNO IN ("
        '    sSQL+= "   SELECT MAX(ANNO)"
        '    sSQL+= "    FROM TIPO_VOCI"
        '    sSQL+= "    INNER JOIN VALORE_VOCI ON TIPO_VOCI.COD_TRIBUTO = VALORE_VOCI.COD_TRIBUTO  AND TIPO_VOCI.COD_CAPITOLO = VALORE_VOCI.COD_CAPITOLO  AND TIPO_VOCI.COD_VOCE = VALORE_VOCI.COD_VOCE AND TIPO_VOCI.COD_ENTE = VALORE_VOCI.COD_ENTE  "
        '    sSQL+= "    WHERE (ANNO <= " & objUtility.CStrToDB(strANNO) & ")"
        '    sSQL+= "    AND VALORE_VOCI.COD_CAPITOLO IN ('0004') "
        '    sSQL+= "    AND VALORE_VOCI.COD_TRIBUTO = " & objUtility.CStrToDB(strCODTRIBUTO)
        '    sSQL+= "    AND (TIPO_VOCI.COD_ENTE = " & objUtility.CStrToDB(strCODENTE) & ")"
        '    sSQL+= "))"
        '    sSQL+=" AND (TIPO_VOCI.COD_TRIBUTO = " & objUtility.CStrToDB(strCODTRIBUTO) & ") AND (TIPO_VOCI.COD_CAPITOLO = '0004') "
        '    sSQL+=" AND (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO IN (" & objUtility.CStrToDBForIn(strCODTIPOPROVVEDIMENTO) & "))"
        '    sSQL+=" AND (TIPO_VOCI.COD_ENTE = " & objUtility.CStrToDB(strCODENTE) & ")"

        '    Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
        '    GetSpese = 0
        '    While result.Read()
        '        GetSpese = result.Item("VALORE")
        '    End While

        '    If Not IsNothing(objSessione) Then
        '        objSessione.Kill()
        '        objSessione = Nothing
        '    End If
        '    Return GetSpese
        ' Catch ex As Exception
        '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetSpese.errore: ", ex)
        'End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Anno"></param>
        ''' <param name="IdTributo"></param>
        ''' <param name="IdEnte"></param>
        ''' <param name="CodTipoProvvedimento"></param>
        ''' <returns></returns>
        Public Function GetSpese(ByVal Anno As String, ByVal IdTributo As String, ByVal IdEnte As String, ByVal CodTipoProvvedimento As String) As Double
            Dim cmdMyCommand As New SqlCommand
            Dim myAdapter As New SqlDataAdapter
            Dim myDataSet As New DataSet
            Dim impSpese As Double = 0

            Try
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "prc_GetSpese"
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.AddWithValue("@IDENTE", IdEnte)
                cmdMyCommand.Parameters.AddWithValue("@IDTRIBUTO", IdTributo)
                cmdMyCommand.Parameters.AddWithValue("@ANNO", Anno)
                cmdMyCommand.Parameters.AddWithValue("@CODTIPOPROVVEDIMENTO", CodTipoProvvedimento)
                myAdapter.SelectCommand = cmdMyCommand
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myAdapter.Fill(myDataSet, "GetSpese")
                myAdapter.Dispose()
                For Each dtMyRow As DataRow In myDataSet.Tables(0).Rows
                    impSpese = CDbl(dtMyRow("VALORE"))
                Next
                Return impSpese
            Catch Err As Exception
                Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetSpese.errore: ", Err)
                Return Nothing
            Finally
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End Try
        End Function

        'Public Function GetSogliaMinima(ByVal strANNO As String, ByVal strCODTRIBUTO As String, ByVal strCODENTE As String, ByVal strCODTIPOPROVVEDIMENTO As String) As Double
        '    dim sSQL as string
        '    Dim strWFErrore As String
        '    Dim objContext As HttpContext = HttpContext.Current
        '    Dim objUtility As New MyUtility
        '    'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '    objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        'Try
        '    If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '        Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '    End If

        '    sSQL="select importo_minimo_anno from anni_provvedimenti"
        '    sSQL+= " where cod_ente=" & objUtility.CStrToDB(strCODENTE)
        '    sSQL+= " and cod_tributo=" & objUtility.CStrToDB(strCODTRIBUTO)
        '    sSQL+= " and anno=" & objUtility.CStrToDB(strANNO)
        '    sSQL+= " and cod_tipo_provvedimento=" & objUtility.CStrToDBForIn(strCODTIPOPROVVEDIMENTO)

        '    'sSQL=" SELECT VALORE_VOCI.ANNO, VALORE_VOCI.VALORE"
        '    'sSQL+=" FROM TIPO_VOCI INNER JOIN TIPO_MISURA ON TIPO_VOCI.MISURA = TIPO_MISURA.COD_MISURA INNER JOIN"
        '    'sSQL+=" VALORE_VOCI ON TIPO_VOCI.COD_TRIBUTO = VALORE_VOCI.COD_TRIBUTO AND TIPO_VOCI.COD_CAPITOLO = VALORE_VOCI.COD_CAPITOLO And TIPO_VOCI.COD_VOCE = VALORE_VOCI.COD_VOCE"
        '    'sSQL+=" WHERE  (VALORE_VOCI.ANNO IN"
        '    'sSQL+="  (SELECT     MAX(anno)"
        '    'sSQL+=" FROM VALORE_VOCI "
        '    'sSQL+=" WHERE ANNO <=" & objUtility.CStrToDB(strANNO) & " AND COD_CAPITOLO IN ('0004')))"
        '    'sSQL+=" AND (TIPO_VOCI.COD_TRIBUTO = " & objUtility.CStrToDB(strCODTRIBUTO) & ") AND (TIPO_VOCI.COD_CAPITOLO = '0004') "
        '    'sSQL+=" AND (TIPO_VOCI.COD_TIPO_PROVVEDIMENTO IN (" & objUtility.CStrToDBForIn(strCODTIPOPROVVEDIMENTO) & "))"
        '    'sSQL+=" AND (TIPO_VOCI.COD_ENTE = " & objUtility.CStrToDB(strCODENTE) & ")"

        '    Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
        '    GetSogliaMinima = 0
        '    While result.Read()
        '        GetSogliaMinima = result.Item("importo_minimo_anno")
        '    End While


        '    If Not IsNothing(objSessione) Then
        '        objSessione.Kill()
        '        objSessione = Nothing
        '    End If

        '    Return GetSogliaMinima
        ' Catch ex As Exception
        '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetSogliaMinima.errore: ", ex)
        'End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Anno"></param>
        ''' <param name="IdTributo"></param>
        ''' <param name="IdEnte"></param>
        ''' <param name="CodTipoProvvedimento"></param>
        ''' <returns></returns>
        Public Function GetSogliaMinima(ByVal Anno As String, ByVal IdTributo As String, ByVal IdEnte As String, ByVal CodTipoProvvedimento As String) As Double
            Dim cmdMyCommand As New SqlCommand
            Dim myAdapter As New SqlDataAdapter
            Dim myDataSet As New DataSet
            Dim impSoglia As Double = 0

            Try
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "prc_GetSogliaMinima"
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.AddWithValue("@IDENTE", IdEnte)
                cmdMyCommand.Parameters.AddWithValue("@IDTRIBUTO", IdTributo)
                cmdMyCommand.Parameters.AddWithValue("@ANNO", Anno)
                cmdMyCommand.Parameters.AddWithValue("@CODTIPOPROVVEDIMENTO", CodTipoProvvedimento)
                myAdapter.SelectCommand = cmdMyCommand
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myAdapter.Fill(myDataSet, "GetSogliaMinima")
                myAdapter.Dispose()
                For Each dtMyRow As DataRow In myDataSet.Tables(0).Rows
                    impSoglia = CDbl(dtMyRow("importo_minimo_anno"))
                Next
                Return impSoglia
            Catch Err As Exception
                Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetSogliaMinima.errore: ", Err)
                Return Nothing
            Finally
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cod_contribuente"></param>
        ''' <param name="anno"></param>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strCODTRIBUTO"></param>
        ''' <returns></returns>
        Public Function controllaAnnoAccertamento(ByVal cod_contribuente As String, ByVal anno As String, ByVal strCODENTE As String, ByVal strCODTRIBUTO As String) As Boolean
            Dim cmdMyCommand As New SqlCommand
            Dim sSQL As String
            Try
                sSQL = "SELECT * FROM TAB_PROCEDIMENTI"
                sSQL += " WHERE COD_ENTE='" & strCODENTE & "' AND COD_TRIBUTO='" & strCODTRIBUTO & "'"
                sSQL += " AND COD_CONTRIBUENTE = '" & cod_contribuente & "' AND ANNO ='" & anno & "'"
                sSQL += " AND COD_TIPO_PROCEDIMENTO = 'A'"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()

                While result.Read()
                    result.Close()
                    Return True
                End While

                Return False
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.controllaAnnoAccertamento.errore: ", Err)
            End Try
        End Function
#Region "STAMPA LETTERE"
        '************************************************************************
        'RITORNA I DATI ANAGRAFICI DEL CONTRIBUENTE 
        'SFRUTTA LA FUNZIONE DELLA GESTIONE atti GETANAGRAFICA
        'I DATI ANAGRAFICI SONO PRELEVATI DALL'ANAGRAFICA IN LINEA

        '************************************************************************
        'Public Function getAnagrafica(ByVal objDSAnagrafico As DataSet) As DataSet

        '    Try
        '        Dim objUtility As New MyUtility
        '        Dim objDSAnagraficaLettere As DataSet = Nothing
        '        Dim objHashTable As Hashtable = New Hashtable
        '        Dim intCount As Integer = 0
        '        Dim strCOD_CONTRIBUENTE As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        Dim strWFErrore As String
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        'Dim strConnectionStringOPENgovProvvedimenti As String
        '        'Dim strConnectionStringAnagrafica As String

        '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
        '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
        '        objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
        '        objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
        '        objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
        '        objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))


        '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
        '        'StrConnectionStringAnagrafica =  objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
        '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

        '        objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", ConstSession.StringConnectionAnagrafica)

        '        For intCount = 0 To objDSAnagrafico.Tables("ANAGRAFICA").Rows.Count - 1
        '            strCOD_CONTRIBUENTE = objUtility.CToStr(objDSAnagrafico.Tables("ANAGRAFICA").Rows(intCount).Item("COD_CONTRIBUENTE"))
        '        Next

        '        objHashTable.Add("CodContribuente", strCOD_CONTRIBUENTE)
        '        objHashTable.Add("Manuale", True)
        '        objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
        '        objHashTable.Add("DA", "")
        '        objHashTable.Add("A", "")

        '        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)


        '        objDSAnagraficaLettere = objCOMRicerca.getAnagrafica(objHashTable)

        '        Return objDSAnagraficaLettere

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getAnagrafica.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        'IL DATA SET DA PASSARE E' SALVATO NELLA VARIABILE DI SESSIONE Session("DATA_SET_ANAGRAFE_LETTERE")
        'PER PASSARLO USARE CType(Session("DATA_SET_ANAGRAFE_LETTERE"), DataSet)
        'Function getDICHIARAZIONI(ByVal objDSAnagrafico As DataSet) As DataSet
        '    Try
        '        Dim objUtility As New MyUtility
        '        Dim objDSDICHIARAZIONI As DataSet = Nothing
        '        Dim intCount As Integer = 0
        '        Dim strCOD_CONTRIBUENTE As String
        '        Dim strConnectionStringOPENgovICI As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        Dim objDA As SqlDataAdapter
        '        Dim objDBManager As DBManager
        '        Dim objCommand As SqlCommand
        '        Dim strCODENTE As String = ConstSession.IdEnte
        '        Dim strSottoApplicazione As String
        '        Dim strWFErrore As String

        '        'objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        'If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '        '    Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        'End If

        '        strSottoApplicazione = ConfigurationManager.AppSettings("OPENGOVI")
        '        strConnectionStringOPENgovICI = ConstSession.StringConnectionICI 'objSessione.oSession.GetPrivateDBManager(strSottoApplicazione).GetConnection.ConnectionString()
        '        'objCommand = New SqlCommand
        '        dim sSQL as string

        '        For intCount = 0 To objDSAnagrafico.Tables("ANAGRAFICA").Rows.Count - 1
        '            strCOD_CONTRIBUENTE = objUtility.CToStr(objDSAnagrafico.Tables("ANAGRAFICA").Rows(intCount).Item("COD_CONTRIBUENTE"))
        '        Next

        '        'objCommand = getCommandStoreProcedure(strCOD_CONTRIBUENTE, strCODENTE, _
        '        '                                      "sp_DatiDichiarazioni")

        '        sSQL=""
        '        sSQL="SELECT * FROM tblTestata" & vbCrLf
        '        sSQL+="WHERE" & vbCrLf
        '        sSQL+="IDContribuente=" & strCOD_CONTRIBUENTE & vbCrLf
        '        sSQL+="AND" & vbCrLf
        '        sSQL+="Ente=" & objUtility.CStrToDB(strCODENTE) & vbCrLf

        '        objDBManager = New DBManager

        '        objDSDICHIARAZIONI = New DataSet

        '        objDBManager.Initialize(strConnectionStringOPENgovICI)
        '        ' objCommand.Connection = objDBManager.GetConnection()
        '        objDA = objDBManager.GetPrivateDataAdapter(sSQL)

        '        objDA.Fill(objDSDICHIARAZIONI, "DICHIARAZIONI")

        '        objDBManager.DisposeConnection()
        '        objDBManager.Dispose()

        '        Return objDSDICHIARAZIONI

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getDICHIARAZIONI.errore: ", Err)
        '        'If Not IsNothing(objSessione) Then
        '        '    objSessione.Kill()
        '        '    objSessione = Nothing
        '        'End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        'If Not IsNothing(objSessione) Then
        '        '    objSessione.Kill()
        '        '    objSessione = Nothing
        '        'End If
        '    End Try

        'End Function


        'IL DATA SET DA PASSARE E' SALVATO NELLA VARIABILE DI SESSIONE Session("DATA_SET_ANAGRAFE_LETTERE")
        'PER PASSARLO USARE CType(Session("DATA_SET_ANAGRAFE_LETTERE"), DataSet)
        'Function getIMMOBILI_DICHIARAZIONI(ByVal strIDDICHIARAZIONE As String) As DataSet
        '    Try
        '        Dim objUtility As New MyUtility
        '        Dim objDSIMMOBILI As DataSet = Nothing
        '        Dim intCount As Integer = 0
        '        Dim strCOD_CONTRIBUENTE As String
        '        Dim strConnectionStringOPENgovICI As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        Dim objDA As SqlDataAdapter
        '        Dim objDBManager As DBManager
        '        Dim objCommand As SqlCommand
        '        Dim strCODENTE As String = ConstSession.IdEnte
        '        Dim strSottoApplicazione As String
        '        Dim strWFErrore As String

        '        'objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        'If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '        '    Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        'End If

        '        strSottoApplicazione = ConfigurationManager.AppSettings("OPENGOVI")
        '        strConnectionStringOPENgovICI = ConstSession.StringConnectionICI 'objSessione.oSession.GetPrivateDBManager(strSottoApplicazione).GetConnection.ConnectionString()

        '        dim sSQL as string

        '        sSQL="SELECT TblOggetti.*, TblDettaglioTestata.*"
        '        sSQL+=" FROM TblDettaglioTestata INNER JOIN"
        '        sSQL+=" TblOggetti ON TblDettaglioTestata.IdOggetto = TblOggetti.ID"
        '        sSQL+=" WHERE (TblDettaglioTestata.IdTestata = " & strIDDICHIARAZIONE & ") AND (TblDettaglioTestata.Ente = '" & strCODENTE & "')"

        '        objDBManager = New DBManager

        '        objDSIMMOBILI = New DataSet

        '        objDBManager.Initialize(strConnectionStringOPENgovICI)
        '        objDA = objDBManager.GetPrivateDataAdapter(sSQL)

        '        objDA.Fill(objDSIMMOBILI, "IMMOBILI_DICHIARAZIONI")

        '        objDBManager.DisposeConnection()
        '        objDBManager.Dispose()

        '        Return objDSIMMOBILI

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getIMMOBILI_DICHIARAZIONI.errore: ", Err)
        '        'If Not IsNothing(objSessione) Then
        '        '    objSessione.Kill()
        '        '    objSessione = Nothing
        '        'End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        'If Not IsNothing(objSessione) Then
        '        '    objSessione.Kill()
        '        '    objSessione = Nothing
        '        'End If
        '    End Try

        'End Function

        'Function getVERSAMENTI(ByVal objDSAnagrafico As DataSet) As DataSet
        '    Dim objUtility As New MyUtility
        '    Dim objDSVERSAMENTI As DataSet = Nothing
        '    Dim intCount As Integer = 0
        '    Dim strCOD_CONTRIBUENTE As String
        '    Dim strConnectionStringOPENgovICI As String
        '    Dim objContext As HttpContext = HttpContext.Current
        '    Dim objDA As SqlDataAdapter
        '    Dim objDBManager As DBManager
        '    Dim objCommand As SqlCommand
        '    Dim strCODENTE As String = ConstSession.IdEnte
        '    Dim strSottoApplicazione As String
        '    dim sSQL as string
        '    Dim strWFErrore As String
        '    Try
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        'objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        'If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '        '    Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        'End If
        '        strSottoApplicazione = ConfigurationManager.AppSettings("OPENGOVI")
        '        strConnectionStringOPENgovICI = ConstSession.StringConnectionICI 'objSessione.oSession.GetPrivateDBManager(strSottoApplicazione).GetConnection.ConnectionString()
        '        'objCommand = New SqlCommand


        '        For intCount = 0 To objDSAnagrafico.Tables("ANAGRAFICA").Rows.Count - 1
        '            strCOD_CONTRIBUENTE = objUtility.CToStr(objDSAnagrafico.Tables("ANAGRAFICA").Rows(intCount).Item("COD_CONTRIBUENTE"))
        '        Next

        '        'objCommand = getCommandStoreProcedure(strCOD_CONTRIBUENTE, strCODENTE, _
        '        '                                      "sp_DatiVersamenti")

        '        sSQL=""
        '        sSQL="SELECT * FROM tblVersamenti" & vbCrLf
        '        sSQL+="WHERE" & vbCrLf
        '        sSQL+="IDAnagrafico=" & strCOD_CONTRIBUENTE & vbCrLf
        '        sSQL+="AND" & vbCrLf
        '        sSQL+="Ente=" & objUtility.CStrToDB(strCODENTE) & vbCrLf


        '        objDSVERSAMENTI = New DataSet

        '        objDBManager = New DBManager
        '        objDBManager.Initialize(strConnectionStringOPENgovICI)

        '        objDA = objDBManager.GetPrivateDataAdapter(sSQL)

        '        objDA.Fill(objDSVERSAMENTI, "VERSAMENTI")


        '        Return objDSVERSAMENTI

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getVERSAMENTI.errore: ", Err)
        '        'If Not IsNothing(objSessione) Then
        '        '    objSessione.Kill()
        '        '    objSessione = Nothing
        '        'End If
        '        If Not IsNothing(objDBManager) Then
        '            objDBManager.Kill()
        '            objDBManager = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        'If Not IsNothing(objSessione) Then
        '        '    objSessione.Kill()
        '        '    objSessione = Nothing
        '        'End If
        '        If Not IsNothing(objDBManager) Then
        '            objDBManager.Kill()
        '            objDBManager = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCodContribuente"></param>
        ''' <param name="strCOD_ENTE"></param>
        ''' <param name="strNameStoreProcedure"></param>
        ''' <returns></returns>
        Private Function getCommandStoreProcedure(ByVal strCodContribuente As String, ByVal strCOD_ENTE As String, ByVal strNameStoreProcedure As String) As SqlCommand
            Dim objCommand As New SqlCommand
            Dim objUtility As New MyUtility
            Try
                objCommand.CommandType = CommandType.StoredProcedure
                objCommand.CommandText = strNameStoreProcedure

                Dim parameterCustomerid As SqlParameter
                parameterCustomerid = New SqlParameter("@CodEnte", SqlDbType.NVarChar, 6)
                parameterCustomerid.Value = strCOD_ENTE

                objCommand.Parameters.Add(parameterCustomerid)

                parameterCustomerid = New SqlParameter("@CodContribuente", SqlDbType.NVarChar, 50)
                parameterCustomerid.Value = strCodContribuente

                objCommand.Parameters.Add(parameterCustomerid)

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getCommandStoreProcedure.errore: ", Err)
            End Try
            Return objCommand
        End Function
#End Region

#Region "GESTIONE ATTI"
        'Public Function SalvaDatiAttoProvvedimento(ByVal objHashTable As Hashtable) As Boolean
        '    Try
        '        SalvaDatiAttoProvvedimento = False
        '        Dim Costant As New CostantiProvv
        '        'Creazione di una istanza alla Libreria del Framwork Ribes

        '        Dim objCOMSETAtti As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

        '        SalvaDatiAttoProvvedimento = objCOMSETAtti.setPROVVEDIMENTO_ATTO(objHashTable)

        '        Return SalvaDatiAttoProvvedimento

        '    Catch Err As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.SalvaDatiAttoProvvedimento.errore: ", Err)
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objHashTable"></param>
        ''' <param name="NUMERO_ATTO"></param>
        ''' <returns></returns>
        Public Function SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(StringConnectionProvv As String, ByVal objHashTable As Hashtable, ByRef NUMERO_ATTO As String) As Boolean
            Try
                SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA = False
                'Creazione di una istanza alla Libreria del Framwork Ribes

                Dim objCOMSETAtti As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

                SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA = objCOMSETAtti.setPROVVEDIMENTO_ATTO_LIQUIDAZIONE_STAMPA(StringConnectionProvv, objHashTable, NUMERO_ATTO)

                Return SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.SetPROVVEDIMENTOATTO_LIQUIDAZIONI_STAMPA.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objHashTable"></param>
        ''' <returns></returns>
        Public Function SetPROVVEDIMENTOATTO_ANNULAMENTO_AVVISO(StringConnectionProvv As String, ByVal objHashTable As Hashtable) As Boolean
            Try

                SetPROVVEDIMENTOATTO_ANNULAMENTO_AVVISO = False
                'Creazione di una istanza alla Libreria del Framwork Ribes

                Dim objCOMSETAtti As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

                SetPROVVEDIMENTOATTO_ANNULAMENTO_AVVISO = objCOMSETAtti.SetPROVVEDIMENTOATTO_ANNULAMENTO_AVVISO(StringConnectionProvv, objHashTable)

                Return SetPROVVEDIMENTOATTO_ANNULAMENTO_AVVISO

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.SetPROVVEDIMENTOATTO_ANNULLAMENTO_AVVISO.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try

        End Function
        ''' <summary>
        ''' Funzione che richiama il servizio per l'aggiornamento dei dati del provvedimento
        ''' </summary>
        ''' <param name="myAtto">OggettoAtto oggetto da gestire</param>
        ''' <returns></returns>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' <strong>Qualificazione AgID-analisi_rel01</strong>
        ''' <em>Analisi eventi</em>
        ''' </revision>
        ''' </revisionHistory>
        Public Function SetPROVVEDIMENTOATTO_LIQUIDAZIONE(ByVal myAtto As OggettoAtto) As Integer
            Try
                Dim objCOMSETAtti As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
                Return objCOMSETAtti.SetProvvedimentoAttoLiquidazione(ConstSession.DBType, ConstSession.StringConnection, myAtto, ConstSession.UserName)
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.SetPROVVEDIMENTOATTO_LIQUIDAZIONE.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try
        End Function
        'Public Function SetPROVVEDIMENTOATTO_LIQUIDAZIONE(ByVal objHashTable As Hashtable) As Boolean
        '    Try
        '        SetPROVVEDIMENTOATTO_LIQUIDAZIONE = False
        '        Dim Costant As New CostantiProvv
        '        'Creazione di una istanza alla Libreria del Framwork Ribes

        '        Dim objCOMSETAtti As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

        '        SetPROVVEDIMENTOATTO_LIQUIDAZIONE = objCOMSETAtti.setPROVVEDIMENTO_ATTO_LIQUIDAZIONE(objHashTable)

        '        Return SetPROVVEDIMENTOATTO_LIQUIDAZIONE

        '    Catch Err As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.SetPROVVEDIMENTOATTO_LIQUIDAZIONE.errore: ", Err)
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    End Try

        'End Function
        'Public Function GetAnniProvvedimenti(ByVal strCODENTE As String) As SqlDataReader
        'Public Function GetAnniProvvedimenti(ByVal strCODENTE As String, ByVal strTributo As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        objUtility = New MyUtility
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If
        '        'forzo il codice tributo a 8852 (ICI)

        '        'dim sSQL as string = "SELECT DISTINCT ANNO FROM ANNI_PROVVEDIMENTI WHERE COD_ENTE=" & objUtility.CStrToDB(strCODENTE) & " and COD_TRIBUTO='8852' order by anno desc"
        '        dim sSQL as string = "SELECT DISTINCT ANNO FROM ANNI_PROVVEDIMENTI WHERE COD_ENTE=" & objUtility.CStrToDB(strCODENTE)
        '        If strTributo <> "" Then
        '            sSQL+=" and COD_TRIBUTO='" & strTributo & "'"
        '        End If
        '        sSQL+=" order by anno desc"

        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetAnniProvvedimenti.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' Il metodo GetAnniProvvedimentiICI ritorna un data reader a forward-only, read-only . Mostra la lista di tutti gli anni provvedimenti nella tabella ANNI_PROVVEDIMENTI del database OPENgovProvvedimenti
        ''' </summary>
        ''' <param name="strAmbiente"></param>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strTributo"></param>
        ''' <returns></returns>
        Public Function GetAnniProvvedimenti(strAmbiente As String, ByVal strCODENTE As String, ByVal strTributo As String) As SqlDataReader
            Dim myDataReader As SqlDataReader
            Dim cmdMyCommand As New SqlCommand

            Try
                Dim sSQL As String
                sSQL = "SELECT DISTINCT ANNO"
                sSQL += " FROM V_ANNI_PROVVEDIMENTI"
                sSQL += " WHERE 1=1"
                '*** 201511 - Funzioni Sovracomunali ***
                sSQL += " AND ('" & strAmbiente & "'='' OR AMBIENTE='" & strAmbiente & "')"
                sSQL += " AND ('" & strCODENTE & "'='' OR COD_ENTE='" & strCODENTE & "')"
                '*** ***
                If strTributo <> "" Then
                    sSQL += " AND COD_TRIBUTO='" & strTributo & "'"
                End If
                sSQL += " ORDER BY ANNO DESC"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandTimeout = 0

                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myDataReader = cmdMyCommand.ExecuteReader
                Return myDataReader
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetAnniProvvedimenti.errore: ", ex)
                Return Nothing
            Finally
                cmdMyCommand.Dispose()
            End Try
        End Function
        '**** 201809 - Cartelle Insoluti ***
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Function GetProvenienzaProvvedimenti() As SqlDataReader
            Dim myDataReader As SqlDataReader
            Dim cmdMyCommand As New SqlCommand

            Try
                Dim sSQL As String
                sSQL = "SELECT *"
                sSQL += " FROM V_PROVENIENZA_PROVVEDIMENTI"
                sSQL += " WHERE 1=1"
                sSQL += " ORDER BY DESCRIZIONE DESC"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
                cmdMyCommand.CommandTimeout = 0
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myDataReader = cmdMyCommand.ExecuteReader
                Return myDataReader
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetProvenienzaProvvedimenti.errore: ", ex)
                Return Nothing
            Finally
                cmdMyCommand.Dispose()
            End Try
        End Function
        ''' <summary>
        ''' Il metodo GetTRIBUTIProvvedimenti ritorna un data reader a forward-only, read-only . Mostra la lista di tutti gli anni provvedimenti nella tabella TAB_TRIBUTI del database OPENgovProvvedimenti
        ''' </summary>
        ''' <param name="strCODENTE"></param>
        ''' <returns></returns>
        Public Function GetTRIBUTIProvvedimenti(ByVal strCODENTE As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                objUtility = New MyUtility
                'forzo il codice tributo a 8852 (ICI)

                Dim sSQL As String = "SELECT DESCRIZIONE FROM TAB_TRIBUTI"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()


                Return result

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTRIBUTIProvvedimenti.errore: ", Err)
                'If Not IsNothing(objSessione) Then
                '    objSessione.Kill()
                '    objSessione = Nothing
                'End If
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                'If Not IsNothing(objSessione) Then
                '    objSessione.Kill()
                '    objSessione = Nothing
                'End If
                cmdMyCommand.Dispose()
            End Try

        End Function
        'Public Function GetTIPOTRIBUTIProvvedimenti(ByVal strCODENTE As String, _
        ' ByVal strCodTributo As String, _
        ' ByVal strAnno As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        objUtility = New MyUtility
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT DISTINCT TAB_TIPO_PROVVEDIMENTO.COD_TIPO_PROVVEDIMENTO ,TAB_TIPO_PROVVEDIMENTO.DESCRIZIONE, TAB_TIPO_PROVVEDIMENTO.COD_TRIBUTO" & vbCrLf
        '        sSQL+="FROM ANNI_PROVVEDIMENTI INNER JOIN" & vbCrLf
        '        sSQL+="TAB_TRIBUTI ON ANNI_PROVVEDIMENTI.COD_TRIBUTO = TAB_TRIBUTI.COD_TRIBUTO INNER JOIN" & vbCrLf
        '        sSQL+="TAB_TIPO_PROVVEDIMENTO ON TAB_TRIBUTI.COD_TRIBUTO = TAB_TIPO_PROVVEDIMENTO.COD_TRIBUTO" & vbCrLf
        '        sSQL+="WHERE" & vbCrLf
        '        sSQL+="ANNI_PROVVEDIMENTI.COD_ENTE=" & objUtility.CStrToDB(strCODENTE) & vbCrLf
        '        sSQL+="AND" & vbCrLf
        '        sSQL+="TAB_TIPO_PROVVEDIMENTO.COD_TIPO_PROVVEDIMENTO <>0" & vbCrLf

        '        If strCodTributo.CompareTo("-1") <> 0 Then

        '            sSQL+="AND ANNI_PROVVEDIMENTI.COD_TRIBUTO =" & objUtility.CStrToDB(strCodTributo) & vbCrLf

        '        End If
        '        If strAnno.CompareTo("-1") <> 0 Then
        '            sSQL+="AND ANNI_PROVVEDIMENTI.ANNO = " & objUtility.CStrToDB(strAnno) & vbCrLf
        '        End If


        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 
        '        Return result

        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTIPOTRIBUTIProvvedimenti.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' Il metodo GetTIPOTRIBUTIProvvedimenti ritorna un data reader a forward-only, read-only . Mostra la lista di tutti gli anni provvedimenti nella tabella TAB_TIPO_PROVVEDIMENTO del database OPENgovProvvedimenti
        ''' </summary>
        ''' <param name="Ambiente"></param>
        ''' <param name="IdEnte"></param>
        ''' <param name="IdTributo"></param>
        ''' <param name="Anno"></param>
        ''' <returns></returns>
        ''' <revisionHistory>
        ''' <revision date="11/2015">
        ''' Funzioni Sovracomunali
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' Modifiche da revisione manuale
        ''' </revision>
        ''' </revisionHistory>
        Public Function GetTIPOTRIBUTIProvvedimenti(Ambiente As String, ByVal IdEnte As String, ByVal IdTributo As String, ByVal Anno As String) As DataSet
            Dim sSQL As String = ""
            Dim myDataSet As DataSet
            Try
                'Valorizzo la connessione
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetTipoProvvedimenti", "AMBIENTE", "IDENTE", "TRIBUTO", "ANNO")
                    myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("AMBIENTE", Ambiente) _
                        , ctx.GetParam("IDENTE", IdEnte) _
                        , ctx.GetParam("TRIBUTO", IdTributo) _
                        , ctx.GetParam("ANNO", Anno)
                    )
                    ctx.Dispose()
                End Using

                Return myDataSet
            Catch ex As Exception
                Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTIPOTRIBUTIProvvedimenti.errore: ", ex)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' Il metodo GetTIPOTRIBUTIProvvedimenti ritorna un data reader a forward-only, read-only . Mostra la lista di tutti gli anni provvedimenti nella tabella TAB_TIPO_PROVVEDIMENTO del database OPENgovProvvedimenti
        ''' </summary>
        ''' <param name="strCODENTE"></param>
        ''' <param name="strAnno"></param>
        ''' <returns></returns>
        Public Function GetTRIBUTIProvvedimentiAnno(ByVal strCODENTE As String, ByVal strAnno As String) As DataSet
            Dim sSQL As String = ""
            Dim myDataSet As DataSet

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    ConstSession.ntry = 0
ReDo:
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetTributiProvvAnno", "IDENTE", "ANNO")
                        myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", strCODENTE), ctx.GetParam("ANNO", strAnno))
                    Catch ex As Exception
                        If ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") And ConstSession.nTry <= 3 Then
                            ConstSession.nTry += 1
                            GoTo ReDo
                        End If
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTRIBUTIProvvedimentiAnno.errore: ", ex)
                    Finally
                        ctx.Dispose()
                    End Try
                End Using
                Return myDataSet
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTRIBUTIProvvedimentiAnno.errore::", ex)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strIDProvvedimento"></param>
        ''' <param name="strTIPO_PROVVEDIMENTO"></param>
        ''' <param name="objDS"></param>
        ''' <returns></returns>
        Public Function get_Stato(ByVal strIDProvvedimento As String, ByVal strTIPO_PROVVEDIMENTO As String, ByVal objDS As DataSet) As String
            Dim objDSDClone As DataSet
            Dim intCount As Integer
            Dim objUtility As New MyUtility
            objDSDClone = objDS.Copy
            Dim objRows() As DataRow
            Dim myStato As String = "ELABORATO"
            objRows = objDSDClone.Tables(0).Select("ID_PROVVEDIMENTO=" & strIDProvvedimento)

            Try
                For intCount = 0 To objRows.Length - 1
                    'QUESTIONARI
                    If strTIPO_PROVVEDIMENTO.CompareTo("1") = 0 Then
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_STAMPA"))) > 0 Then
                            myStato = "STAMPATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_CONSEGNA_AVVISO"))) > 0 Then
                            myStato = "CONSEGNATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_NOTIFICA_AVVISO"))) > 0 Then
                            myStato = "NOTIFICATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_PERVENUTO_IL"))) > 0 Then
                            myStato = "CHIUSO"
                        End If
                    Else
                        'ALTRI AVVISI
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_CONFERMA"))) > 0 Then
                            myStato = "CONFERMATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_STAMPA"))) > 0 Then
                            myStato = "STAMPATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_CONSEGNA_AVVISO"))) > 0 Then
                            myStato = "CONSEGNATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_NOTIFICA_AVVISO"))) > 0 Then
                            myStato = "NOTIFICATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_SOSPENSIONE_AVVISO_AUTOTUTELA"))) > 0 Then
                            myStato = "SOSPESO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_RETTIFICA_AVVISO"))) > 0 Then
                            myStato = "RETTIFICATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_ANNULLAMENTO_AVVISO"))) > 0 Then
                            myStato = "ANNULLATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_PAGAMENTO"))) > 0 Or Len(objUtility.CToStr(objRows(intCount)("DATA_VERSAMENTO_SOLUZIONE_UNICA"))) > 0 Then
                            myStato = "PAGATO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_PRESENTAZIONE_RICORSO"))) > 0 Or Len(objUtility.CToStr(objRows(intCount)("DATA_PRESENTAZIONE_RICORSO_REGIONALE"))) > 0 Or Len(objUtility.CToStr(objRows(intCount)("DATA_PRESENTAZIONE_RICORSO_CASSAZIONE"))) > 0 Then
                            myStato = "IN RICORSO"
                        End If
                        If Len(objUtility.CToStr(objRows(intCount)("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA"))) > 0 Or Len(objUtility.CToStr(objRows(intCount)("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE"))) > 0 Or Len(objUtility.CToStr(objRows(intCount)("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE"))) > 0 Then
                            myStato = "INGIUNZIONE" 'SOSPESO
                        End If
                    End If
                Next
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.getStato.errore: ", ex)
                myStato = ""
            End Try
            Return myStato
        End Function
#End Region

#Region "GESTIONE CONFIGURAZIONE"
        'Public Function GetTributi(ByVal strCODTributo As String, ByVal strDESCtributo As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_TRIBUTO,DESCRIZIONE FROM TAB_TRIBUTI WHERE 1=1 "
        '        If strCODTributo <> "" Then
        '            sSQL+=" and COD_TRIBUTO='" & strCODTributo & "'"
        '        End If
        '        If strDESCtributo <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCtributo & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTributi.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODTributo"></param>
        ''' <param name="strDESCtributo"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetTributi(ByVal strCODTributo As String, ByVal strDESCtributo As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_TRIBUTO,DESCRIZIONE FROM TAB_TRIBUTI WHERE 1=1 "
                If strCODTributo <> "" Then
                    sSQL += " and COD_TRIBUTO='" & strCODTributo & "'"
                End If
                If strDESCtributo <> "" Then
                    sSQL += " and DESCRIZIONE='" & strDESCtributo & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTributi.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try
        End Function
        'Public Function GetTipoInteresse(ByVal strCODInteresse As String, ByVal strDESCInteresse As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_TIPO_INTERESSE,DESCRIZIONE FROM TAB_TIPI_INTERESSE WHERE 1=1 "
        '        If strCODInteresse <> "" Then
        '            '*** 20130801 - accertamento OSAP ***
        '            sSQL+=" and (COD_TRIBUTO='" & strCODInteresse & "')"
        '            '*** ***
        '        End If
        '        If strDESCInteresse <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCInteresse & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoInteresse.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODInteresse"></param>
        ''' <param name="strDESCInteresse"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetTipoInteresse(ByVal strCODInteresse As String, ByVal strDESCInteresse As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_TIPO_INTERESSE,DESCRIZIONE FROM TAB_TIPI_INTERESSE WHERE 1=1 "
                If strCODInteresse <> "" Then
                    '*** 20130801 - accertamento OSAP ***
                    sSQL += " and (COD_TRIBUTO='" & strCODInteresse & "')"
                    '*** ***
                End If
                If strDESCInteresse <> "" Then
                    sSQL += " and DESCRIZIONE='" & strDESCInteresse & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoInteresse.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try
        End Function

        'Public Function GetCapitoli(ByVal COD_TRIBUTO As String, ByVal strCODCapitolo As String, ByVal strDESCCapitolo As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_CAPITOLO,DESCRIZIONE FROM TAB_CAPITOLI WHERE COD_TRIBUTO='" & COD_TRIBUTO & "'"
        '        If strCODCapitolo <> "" Then
        '            sSQL+=" and COD_CAPITOLO='" & strCODCapitolo & "'"
        '        End If
        '        If strDESCCapitolo <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCCapitolo & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetCapitoli.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="COD_TRIBUTO"></param>
        ''' <param name="strCODCapitolo"></param>
        ''' <param name="strDESCCapitolo"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetCapitoli(ByVal COD_TRIBUTO As String, ByVal strCODCapitolo As String, ByVal strDESCCapitolo As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_CAPITOLO,DESCRIZIONE FROM TAB_CAPITOLI WHERE COD_TRIBUTO='" & COD_TRIBUTO & "'"
                If strCODCapitolo <> "" Then
                    sSQL += " AND COD_CAPITOLO='" & strCODCapitolo & "'"
                End If
                If strDESCCapitolo <> "" Then
                    sSQL += " AND DESCRIZIONE='" & strDESCCapitolo & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetCapitoli.errore: ", Err)
                Return Nothing
            End Try
        End Function

        'Public Function GetTipoProvvedimento(ByVal COD_TRIBUTO As String, ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal CampoVisualizza As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        sSQL = "SELECT COD_TIPO_PROVVEDIMENTO,DESCRIZIONE FROM TAB_TIPO_PROVVEDIMENTO WHERE COD_TRIBUTO='" & COD_TRIBUTO & "'"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_TIPO_PROVVEDIMENTO=" & strCODICE
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCRIZIONE & "'"
        '        End If
        '        If CampoVisualizza <> "" Then
        '            sSQL+=" and " & CampoVisualizza & "=1"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
        '        'Ritorna come risultato un datareader 
        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoProvvedimento.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="COD_TRIBUTO"></param>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="CampoVisualizza"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetTipoProvvedimento(ByVal COD_TRIBUTO As String, ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal CampoVisualizza As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_TIPO_PROVVEDIMENTO,DESCRIZIONE FROM TAB_TIPO_PROVVEDIMENTO WHERE COD_TRIBUTO='" & COD_TRIBUTO & "'"
                If strCODICE <> "" Then
                    sSQL += " and COD_TIPO_PROVVEDIMENTO=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " and DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                If CampoVisualizza <> "" Then
                    sSQL += " and " & CampoVisualizza & "=1"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoProvvedimento.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            End Try
        End Function

        'Public Function GetTipologieSanzioni(ByVal COD_TRIBUTO As String, ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try

        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_VOCE,DESCRIZIONE FROM TIPOLOGIE_SANZIONI WHERE COD_TRIBUTO='" & COD_TRIBUTO & "'"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_VOCE='" & strCODICE & "'"
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCRIZIONE & "'"
        '        End If
        '        sSQL+=" and COD_ENTE='" & ConstSession.IdEnte & "'"
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 
        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTiplogieSanzioni.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="COD_TRIBUTO"></param>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetTipologieSanzioni(ByVal COD_TRIBUTO As String, ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_VOCE,DESCRIZIONE FROM TIPOLOGIE_SANZIONI WHERE COD_TRIBUTO='" & COD_TRIBUTO & "'"
                If strCODICE <> "" Then
                    sSQL += " AND COD_VOCE='" & strCODICE & "'"
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                sSQL += " AND COD_ENTE='" & ConstSession.IdEnte & "'"
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipologieSanzioni.errore: ", Err)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="COD_ENTE"></param>
        ''' <param name="COD_TRIBUTO"></param>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <returns></returns>
        Public Function GetTipoVoci(ByVal COD_ENTE As String, ByVal COD_TRIBUTO As String, ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                Dim sSQL As String


                sSQL = "SELECT COD_VOCE,DESCRIZIONE_VOCE FROM TIPO_VOCI "
                sSQL += " WHERE COD_ENTE ='" & COD_ENTE & "' "
                sSQL += " and COD_TRIBUTO='" & COD_TRIBUTO & "'"
                If strCODICE <> "" Then
                    sSQL += " and COD_VOCE=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " and DESCRIZIONE_VOCE='" & strDESCRIZIONE & "'"
                End If
                sSQL += " GROUP BY COD_VOCE, DESCRIZIONE_VOCE"
                sSQL += " ORDER BY DESCRIZIONE_VOCE"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()
                Return result

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoVoci.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                cmdMyCommand.Dispose()
            End Try

        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="COD_ENTE"></param>
        ''' <param name="COD_TRIBUTO"></param>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <returns></returns>
        Public Function GetTipologieSanzioni(ByVal COD_ENTE As String, ByVal COD_TRIBUTO As String, ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                Dim sSQL As String

                sSQL = "SELECT COD_VOCE,DESCRIZIONE FROM TIPOLOGIE_SANZIONI "
                sSQL += " WHERE COD_ENTE ='" & COD_ENTE & "' "
                sSQL += " and COD_TRIBUTO='" & COD_TRIBUTO & "'"
                If strCODICE <> "" Then
                    sSQL += " and COD_VOCE=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " and DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                sSQL += " GROUP BY COD_VOCE, DESCRIZIONE"
                sSQL += " ORDER BY DESCRIZIONE"
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()

                Return result

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipologieSanzioni.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                cmdMyCommand.Dispose()
            End Try

        End Function

        'Public Function GetTipoMisura(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_MISURA,DESCRIZIONE FROM TIPO_MISURA WHERE 1=1"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_MISURA='" & strCODICE & "'"
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCRIZIONE & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoMisura.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetTipoMisura(ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_MISURA,DESCRIZIONE FROM TIPO_MISURA WHERE 1=1"
                If strCODICE <> "" Then
                    sSQL += " AND COD_MISURA='" & strCODICE & "'"
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoMisura.errore: ", Err)
                Return Nothing
            End Try
        End Function

        'Public Function GetFase(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_FASE,DESCRIZIONE FROM TAB_FASI WHERE 1=1"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_FASE=" & strCODICE
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCRIZIONE & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetFase.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetFase(ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_FASE,DESCRIZIONE FROM TAB_FASI WHERE 1=1"
                If strCODICE <> "" Then
                    sSQL += " AND COD_FASE=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetFase.errore: ", Err)
                Return Nothing
            End Try
        End Function

        'Public Function GetTipoBaseCalcolo(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT TIPO,DESCRIZIONE FROM TIPO_BASE_CALCOLO WHERE 1=1"
        '        If strCODICE <> "" Then
        '            sSQL+=" and TIPO=" & strCODICE
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESCRIZIONE='" & strDESCRIZIONE & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoBaseCalcolo.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetTipoBaseCalcolo(ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT TIPO,DESCRIZIONE FROM TIPO_BASE_CALCOLO WHERE 1=1"
                If strCODICE <> "" Then
                    sSQL += " AND TIPO=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoBaseCalcolo.errore: ", Err)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <returns></returns>
        Public Function GetTipoBaseDati(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand
            Try
                'Creazione di una istanza alla Libreria del Framwork Ribes
                Dim sSQL As String
                sSQL = "SELECT ID_BASE_DATI,DESCRIZIONE FROM TP_BASE_DATI WHERE 1=1 AND FLAG_VISIBILE=1 "
                If strCODICE <> "" Then
                    sSQL += " and ID_BASE_DATI=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " and DESCRIZIONE='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.CommandTimeout = 0
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Dim result As SqlDataReader = cmdMyCommand.ExecuteReader()
                Return result

            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetTipoBaseDati.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
            Finally
                cmdMyCommand.Dispose()
            End Try

        End Function

        'Public Function GetParametro(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current

        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_PARAMETRO,DESC_PARAMETRO FROM TAB_PARAMETRO WHERE 1=1"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_PARAMETRO=" & strCODICE
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESC_PARAMETRO='" & strDESCRIZIONE & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetParametro.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetParametro(ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_PARAMETRO,DESC_PARAMETRO FROM TAB_PARAMETRO WHERE 1=1"
                If strCODICE <> "" Then
                    sSQL += " AND COD_PARAMETRO=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESC_PARAMETRO='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetParametro.errore: ", Err)
                Return Nothing
            End Try
        End Function

        'Public Function GetBaseRaffronto(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_BASE_RAFFRONTO,DESC_BASE_RAFFRONTO FROM TAB_BASE_RAFFRONTO WHERE 1=1"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_BASE_RAFFRONTO=" & strCODICE
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESC_BASE_RAFFRONTO='" & strDESCRIZIONE & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetBaseRaffronto.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try

        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetBaseRaffronto(ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_BASE_RAFFRONTO,DESC_BASE_RAFFRONTO FROM TAB_BASE_RAFFRONTO WHERE 1=1"
                If strCODICE <> "" Then
                    sSQL += " AND COD_BASE_RAFFRONTO=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESC_BASE_RAFFRONTO='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetBaseRaffronto.errore: ", Err)
                Return Nothing
            End Try
        End Function
        'Public Function GetBaseRaffronto_Instrasmissibilita(ByVal strCODICE As String, ByVal strDESCRIZIONE As String) As SqlDataReader
        '    Try
        '        'Creazione di una istanza alla Libreria del Framwork Ribes
        '        Dim strWFErrore As String
        '        Dim objContext As HttpContext = HttpContext.Current
        '        'objSessione = CType(objContext.Session("objSessione"), OPENUtility.CreateSessione)
        '        dim sSQL as string

        '        objSessione = New OPENUtility.CreateSessione(objContext.Session("Applicazione"), objContext.ConstSession.UserName, "OPENGOVP")
        '        If Not objSessione.CreaSessione(objContext.ConstSession.UserName, strWFErrore) Then
        '            Throw New Exception("Errore nell'apertura della sessione di WorkFlow " & strWFErrore)
        '        End If

        '        SSQL = "SELECT COD_BASE_RAFFRONTO,DESC_BASE_RAFFRONTO FROM TAB_BASE_RAFFRONTO_INTRASMISSIBILITA WHERE 1=1"
        '        If strCODICE <> "" Then
        '            sSQL+=" and COD_BASE_RAFFRONTO=" & strCODICE
        '        End If
        '        If strDESCRIZIONE <> "" Then
        '            sSQL+=" and DESC_BASE_RAFFRONTO='" & strDESCRIZIONE & "'"
        '        End If
        '        Dim result As SqlDataReader = objSessione.oSession.oAppDB.GetPrivateDataReader(SSQL)
        '        'Ritorna come risultato un datareader 

        '        Return result

        '    Catch Err As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetBaseRaffronto_Instramissibilita.errore: ", Err)
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '        Throw New Exception(Err.Source & "::" & Err.StackTrace)
        '    Finally
        '        If Not IsNothing(objSessione) Then
        '            objSessione.Kill()
        '            objSessione = Nothing
        '        End If
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strCODICE"></param>
        ''' <param name="strDESCRIZIONE"></param>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        Public Function GetBaseRaffronto_Instrasmissibilita(ByVal strCODICE As String, ByVal strDESCRIZIONE As String, ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Dim sSQL As String
            Dim result As SqlDataReader

            Try
                sSQL = "SELECT COD_BASE_RAFFRONTO,DESC_BASE_RAFFRONTO FROM TAB_BASE_RAFFRONTO_INTRASMISSIBILITA WHERE 1=1"
                If strCODICE <> "" Then
                    sSQL += " AND COD_BASE_RAFFRONTO=" & strCODICE
                End If
                If strDESCRIZIONE <> "" Then
                    sSQL += " AND DESC_BASE_RAFFRONTO='" & strDESCRIZIONE & "'"
                End If
                cmdMyCommand.CommandType = CommandType.Text
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                result = cmdMyCommand.ExecuteReader
                Return result
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.GetBaseRaffronto_Instrasmissibilita.errore: ", Err)
                Return Nothing
            End Try
        End Function

        'Public Function CalcoloValoredaRendita(ByVal dblRendita As Double, ByVal strTipoImm As String, ByVal strCateg As String, ByVal strAnno As String) As Double

        '    Dim AppoggioValore As Double
        'Try
        '    If dblRendita <> 0 Then
        '        Select Case strTipoImm

        '            Case "TA"

        '                AppoggioValore = dblRendita * 75
        '                If strAnno >= "1997" Then
        '                    AppoggioValore = AppoggioValore + ((AppoggioValore * 25) / 100)
        '                End If

        '            Case "AF", "LC"

        '                AppoggioValore = dblRendita

        '            Case Else
        '                AppoggioValore = dblRendita

        '                If strCateg.ToUpper = "A/10" Or InStr(strCateg.ToUpper, "D") <> 0 Then
        '                    '              AppoggioValore = dblRendita * 1.05
        '                    AppoggioValore = AppoggioValore * 50
        '                ElseIf strCateg.ToUpper = "C/1" Then
        '                    '             AppoggioValore = dblRendita * 1.05
        '                    AppoggioValore = AppoggioValore * 34
        '                Else
        '                    '            AppoggioValore = dblRendita * 1.05
        '                    AppoggioValore = AppoggioValore * 100
        '                End If
        '                If strAnno >= "1997" Then
        '                    'GIULIA 12082005
        '                    AppoggioValore = AppoggioValore * 1.05
        '                    'AppoggioValore = dblRendita * 1.05
        '                End If

        '        End Select

        '    End If


        '    'giulia 18082005
        '    AppoggioValore = AppoggioValore * 1000
        '    If InStr(AppoggioValore, ",") <> 0 Then
        '        AppoggioValore = AppoggioValore + 0.5
        '        If InStr(AppoggioValore, ",") <> 0 Then
        '            AppoggioValore = Mid(AppoggioValore, 1, InStr(AppoggioValore, ","))
        '        Else
        '            AppoggioValore = AppoggioValore
        '        End If
        '    Else
        '        AppoggioValore = AppoggioValore
        '    End If

        '    AppoggioValore = AppoggioValore / 1000


        '    'AppoggioValore = AppoggioValore * 100
        '    'If InStr(AppoggioValore, ",") <> 0 Then
        '    '  AppoggioValore = AppoggioValore + 0.5
        '    '  If InStr(AppoggioValore, ",") <> 0 Then
        '    '    AppoggioValore = Mid(AppoggioValore, 1, InStr(AppoggioValore, ","))
        '    '  Else
        '    '    AppoggioValore = AppoggioValore
        '    '  End If
        '    'Else
        '    '  AppoggioValore = AppoggioValore
        '    'End If

        '    ''GIULIA 12082005
        '    'AppoggioValore = AppoggioValore / 100
        '    ''AppoggioValore = AppoggioValore / 100

        '    Return AppoggioValore
        ' Catch Err As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.CalcoloValoreRendita.errore: ", Err)
        ' End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nValore"></param>
        ''' <param name="strTipoImm"></param>
        ''' <param name="strCateg"></param>
        ''' <param name="strAnno"></param>
        ''' <returns></returns>
        Public Function CalcoloRenditadaValore(ByVal nValore As Double, ByVal strTipoImm As String, ByVal strCateg As String, ByVal strAnno As String) As Double
            Dim nRendita As Double
            Try
                If nValore <> 0 Then
                    Select Case strTipoImm
                        Case "TA"
                            If strAnno >= "1997" Then
                                nRendita = (nValore / 125) * 100
                            End If
                            nRendita = nValore / 75
                        Case "AF", "LC"
                            nRendita = nValore
                        Case Else
                            nRendita = nValore
                            If strAnno >= "1997" Then
                                nRendita = nRendita / 1.05
                            End If
                            If strCateg.ToUpper = "A/10" Or InStr(strCateg.ToUpper, "D") <> 0 Then
                                nRendita = nRendita / 50
                            ElseIf strCateg.ToUpper = "C/1" Then
                                nRendita = nRendita / 34
                            Else
                                nRendita = nRendita / 100
                            End If
                    End Select
                End If

                nRendita = nRendita * 1000
                If InStr(nRendita, ",") <> 0 Then
                    nRendita = nRendita + 0.5
                    If InStr(nRendita, ",") <> 0 Then
                        nRendita = Mid(nRendita, 1, InStr(nRendita, ","))
                    Else
                        nRendita = nRendita
                    End If
                Else
                    nRendita = nRendita
                End If

                nRendita = nRendita / 1000

                Return nRendita
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ProvvedimentiDB.CalcoloRenditaValore.errore: ", Err)
                Throw New Exception(Err.Source & "::" & Err.StackTrace)
                Return 0
            End Try
        End Function
#End Region
    End Class
End Namespace