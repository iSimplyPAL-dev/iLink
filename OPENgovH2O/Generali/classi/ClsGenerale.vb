'Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Namespace ClsGenerale
    Public Class Generale
        Private Shared Log As ILog = LogManager.GetLogger(GetType(Generale))
        Private iDB As New DBAccess.getDBobject

        Public Function GetPeriodoAttuale() As Boolean
            Dim dvMyDati As New DataView
            Dim sSQL As String
            Try
                '**************************************
                'modifica del 13/02/2007
                'devo estrarre il periodo di fatturazione valido - con ATTUALE = 1
                sSQL = "SELECT *"
                sSQL += " FROM TP_PERIODO"
                sSQL += " WHERE ATTUALE=1"
                sSQL += " AND (COD_ENTE='" & ConstSession.IdEnte & "')"
                'eseguo la query per vedere se c'è un periodo di fatturazione attivo
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        HttpContext.Current.Session("PERIODO") = Utility.StringOperation.FormatString(myRow("PERIODO"))
                        HttpContext.Current.Session("PERIODOID") = Utility.StringOperation.FormatString(myRow("CODPERIODO"))
                    Next
                    Return True
                Else
                    HttpContext.Current.Session("PERIODO") = ""
                    Return False
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.GetPeriodoAttuale.errore: ", ex)
                Return False
            Finally
                dvMyDati.Dispose()
            End Try
        End Function

        Public Function ControllaData(ByRef DataControllo As String, ByVal Tipo As Integer) As Boolean
            'SUB CHE CONTROLLA SE E' STATA INSERITA UNA DATA CORRETTA
            'Tipo
            '1 --> GGMMAAAA      2 --> AAAAMMGG

            Try
                Dim Mese, Giorno, Anno As Integer
                Dim Bisestile As Integer

                ControllaData = True

                DataControllo = DataControllo.Replace("/", "")
                DataControllo = DataControllo.Replace("-", "")
                DataControllo = DataControllo.Replace(" 0.00.00", "")
                'controllo la lunghezza
                If DataControllo.Length <> 8 Then
                    ControllaData = False : Exit Function
                End If

                If Tipo = 2 Then
                    DataControllo = DataControllo.Substring(6, 2) & DataControllo.Substring(4, 2) & DataControllo.Substring(0, 4)
                End If

                Giorno = CInt(DataControllo.Substring(0, 2))
                Mese = CInt(DataControllo.Substring(2, 2))
                Anno = CInt(DataControllo.Substring(4, 4))

                If Len(Anno) = 4 Then
                    Bisestile = CInt(Anno) Mod 4
                Else
                    ControllaData = False : Exit Function
                End If

                'controllo del giorno
                If Mese = 2 And Bisestile = 0 Then 'controllo giorni di feb.quando anno bisestile
                    If Giorno < 1 Or Giorno > 29 Then
                        ControllaData = False : Exit Function
                    End If
                ElseIf Mese = 2 And Bisestile <> 0 Then  'controllo giorni difeb. quando anno non bisestile
                    If Giorno < 1 Or Giorno > 28 Then
                        ControllaData = False : Exit Function
                    End If
                ElseIf Mese = 11 Or Mese = 4 Or Mese = 6 Or Mese = 9 Then
                    'controllo giorni se il mese ne deve avere 30
                    If Giorno < 1 Or Giorno > 30 Then
                        ControllaData = False : Exit Function
                    End If
                ElseIf Mese <> 11 And Mese <> 4 And Mese <> 6 And Mese <> 9 Then
                    'altri mesi
                    If Giorno < 1 Or Giorno > 31 Then
                        ControllaData = False : Exit Function
                    End If
                End If

                'controllo mese
                If Mese < 1 Or Mese > 12 Then
                    ControllaData = False : Exit Function
                End If

            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.ControlloData.errore: ", ex)
                ControllaData = False
                Throw ex
                Exit Function
            End Try
        End Function

        Public Sub DeleteFile(ByVal FileName As String)
            Try
                If IO.File.Exists(FileName) = True Then
                    IO.File.Delete(FileName)
                End If
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.DeleteFile.errore: ", Err)
            End Try
        End Sub

        Public Function ReplaceCharsForSearch(ByVal myString As String) As String
            Dim sReturn As String

            sReturn = ReplaceChar(myString)
            Return sReturn
        End Function

        Public Function ReplaceChar(ByVal myString As String) As String
            Dim sReturn As String

            sReturn = Replace(myString, "'", "''")
            sReturn = Replace(sReturn, "*", "%")
            sReturn = Replace(sReturn, "&nbsp;", " ")
            sReturn = Trim(sReturn)
            Return sReturn
        End Function

        Public Function ReplaceNumberForDB(ByVal sNumber As String) As String
            Try
                Dim sFormatNumber As String

                sFormatNumber = sNumber.Replace(",", ".")

                Return sFormatNumber
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.ReplaceNumberForDB.errore: ", ex)
                Throw ex
                Exit Function
            End Try
        End Function

        Public Function ReplaceDataForDB(ByVal myString As String) As String
            Dim sReturn As String
            Try
                sReturn = CDate(myString).ToString(ConfigurationManager.AppSettings("lingua_date")).Replace(".", ":")
                Return sReturn
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.ReplaceDataForDB.errore: ", ex)
                Throw ex
                Exit Function
            End Try
        End Function

        Public Function GiraData(ByVal data As String) As String
            'leggo la data nel formato gg/mm/aaaa e la metto nel formato aaaammgg
            Dim Giorno As String
            Dim Mese As String
            Dim Anno As String
            GiraData = ""
            Try
                If data <> "" Then
                    Giorno = Mid(data, 1, 2)
                    Mese = Mid(data, 4, 2)
                    Anno = Mid(data, 7, 4)
                    GiraData = Anno & Mese & Giorno
                    If GiraData = "00.0." Then
                        GiraData = ""
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.GiraData.errore: ", ex)
            End Try
        End Function

        Public Function GiraDataFromDB(ByVal data As String) As String
            'leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
            Dim Giorno As String
            Dim Mese As String
            Dim Anno As String
            Try
                Log.Debug("GiraDataFromDB.da girare->" + data)
                If data <> "" Then
                    Giorno = Mid(data, 7, 2)
                    Mese = Mid(data, 5, 2)
                    Anno = Mid(data, 1, 4)
                    If Anno <> "9999" Then
                        GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
                    Else
                        GiraDataFromDB = ""
                    End If
                Else
                    GiraDataFromDB = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.GiraDataFromDB.errore: ", ex)
                GiraDataFromDB = ""
            End Try
        End Function

        Public Function FormattaData(ByVal TypeFormat As String, ByVal CharFormatta As String, ByVal DataFormattare As String, ByVal DataSistema As Boolean) As String
            Dim GG As String
            Dim MM As String
            Dim AAAA As String
            Dim TmpDate As String
            FormattaData = ""
            'TypeFORMAT vale A se la data deve essere girata in AAAAMMGG, vale G se deve essere girata in GGMMAAAA
            Try
                If DataFormattare <> "" Or DataSistema = True Then
                    If TypeFormat = "A" Then
                        If DataSistema = True Then
                            Dim DataOdierna As String

                            If Len(Today.Day.ToString) < 2 Then
                                TmpDate = CStr("00" & Today.Day)
                                DataOdierna = TmpDate.Substring(TmpDate.Length - 2, 2)
                            Else
                                DataOdierna = CStr(Today.Day)
                            End If
                            If Len(Today.Month.ToString) < 2 Then
                                TmpDate = CStr("00" & Today.Month)
                                DataOdierna += TmpDate.Substring(TmpDate.Length - 2, 2)
                            Else
                                DataOdierna += CStr(Today.Month)
                            End If
                            If Len(Today.Year.ToString) < 4 Then
                                TmpDate = CStr("0000" & Today.Year)
                                DataOdierna += TmpDate.Substring(TmpDate.Length - 4, 4)
                            Else
                                DataOdierna += CStr(Today.Year)
                            End If

                            DataFormattare = DataOdierna
                        End If

                        DataFormattare = Replace(Replace(DataFormattare, "/", ""), "-", "")

                        GG = DataFormattare.Substring(0, 2)
                        MM = DataFormattare.Substring(2, 2)
                        AAAA = DataFormattare.Substring(DataFormattare.Length - 4, 4)

                        FormattaData = AAAA & MM & GG
                    Else
                        If DataSistema = True Then
                            Dim DataOdierna As String

                            If Len(Today.Year.ToString) < 4 Then
                                TmpDate = CStr("0000" & Today.Year.ToString)
                                DataOdierna = TmpDate.Substring(TmpDate.Length - 4, 4)
                            Else
                                DataOdierna = CStr(Today.Year)
                            End If
                            If Len(Today.Month.ToString) < 2 Then
                                'DataOdierna = DataOdierna & CStr("00" & Today.Month)
                                TmpDate = CStr("00" & Today.Month)
                                DataOdierna += TmpDate.Substring(TmpDate.Length - 2, 2)
                            Else
                                DataOdierna += CStr(Today.Month)
                            End If
                            If Len(Today.Day.ToString) < 2 Then
                                'DataOdierna = DataOdierna & CStr("00" & Today.Day.ToString)
                                TmpDate = CStr("0" & Today.Day)
                                DataOdierna += TmpDate.Substring(TmpDate.Length - 2, 2)
                            Else
                                DataOdierna += CStr(Today.Day)
                            End If

                            DataFormattare = DataOdierna
                        End If
                        DataFormattare = Replace(Replace(DataFormattare, "/", ""), "-", "")

                        GG = DataFormattare.Substring(DataFormattare.Length - 2, 2)
                        MM = DataFormattare.Substring(4, 2)
                        AAAA = DataFormattare.Substring(0, 4)

                        FormattaData = GG & CharFormatta & MM & CharFormatta & AAAA
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.FormattaData.errore: ", ex)
            End Try
        End Function

        Public Function ReplaceCharForFile(ByVal myString As String) As String
            Dim sReturn As String

            sReturn = myString
            sReturn = sReturn.Replace("à", "a'")
            sReturn = sReturn.Replace("é", "e'")
            sReturn = sReturn.Replace("è", "e'")
            sReturn = sReturn.Replace("ì", "i'")
            sReturn = sReturn.Replace("ò", "o'")
            sReturn = sReturn.Replace("ù", "u'")

            sReturn = sReturn.Replace("à".ToUpper, "a'".ToUpper)
            sReturn = sReturn.Replace("é".ToUpper, "e'".ToUpper)
            sReturn = sReturn.Replace("è".ToUpper, "e'".ToUpper)
            sReturn = sReturn.Replace("ì".ToUpper, "i'".ToUpper)
            sReturn = sReturn.Replace("ò".ToUpper, "o'".ToUpper)
            sReturn = sReturn.Replace("ù".ToUpper, "u'".ToUpper)

            sReturn = sReturn.Replace("ç", "c")
            sReturn = sReturn.Replace("°", "")
            sReturn = sReturn.Replace("€", "euro")
            sReturn = sReturn.Replace("£", "lire")

            Return sReturn
        End Function
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Estrae i dati dalle tabelle che contengono un codice e una descrizione.
        ''' Passando come parametri l'ID e/o la descrizione esegue la ricerca.
        ''' </summary>
        ''' <param name="IdDescr">ID tabella</param>
        ''' <param name="Descrizione">Descrizione tabella</param>
        ''' <param name="Tabella">Nome tabella</param>
        ''' <returns>Dataset</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetDescrizioni(ByVal IdDescr As String, ByVal Descrizione As String, ByVal Tabella As String) As DataSet
            Dim DsDati As New DataSet
            Dim cmdMyCommand As New SqlCommand

            Try
                'eseguo la query
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "prc_DescrTabCalcolo_S"
                cmdMyCommand.Parameters.Add(New SqlParameter("@TABELLA", Tabella))
                cmdMyCommand.Parameters.Add(New SqlParameter("@ID", IdDescr))
                cmdMyCommand.Parameters.Add(New SqlParameter("@DESCRIZIONE", Descrizione))
                DsDati = iDB.GetDataSet(cmdMyCommand)
                Return DsDati

            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.GetDescrizioni.errore: ", ex)
                Throw New Exception("Problemi nell'esecuzione di GetAddizionali " + ex.Message)
            End Try
        End Function
        '*** ***
        ''' <summary>
        ''' Verifica se un addizionale è già associata ad almeno un ente.
        ''' </summary>
        ''' <param name="Id">ID tabella</param>
        ''' <param name="DescrAddizionale">Descrizione tabella</param>
        ''' <param name="Tabella">Nome tabella</param>
        ''' <returns>Dataset</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Function GetDescrizioniAssociate(ByVal Id As String, ByVal DescrAddizionale As String, ByVal Tabella As String) As DataSet
            Dim sSQL As String = ""
            Dim dsAddizionali As DataSet

            Try
                If Tabella = "TP_ADDIZIONALI" Then
                    sSQL = "SELECT ID_ADDIZIONALE as ID FROM TP_ADDIZIONALI_ENTE"
                    'SSQL+=" WHERE TP_ADDIZIONALI.IDENTE='" & ConstSession.IdEnte & "'"
                    If Id.CompareTo("") <> 0 And Id.CompareTo("").ToString() <> "..." Then
                        SSQL+=" WHERE ID_ADDIZIONALE='" & Id.Replace("'", "''") & "'"
                    End If
                ElseIf Tabella = "TP_TIPOLOGIE_CANONI" Then
                    sSQL = "SELECT ID_TIPO_CANONE as ID FROM TP_CANONI"
                    'SSQL+=" WHERE TP_TIPOLOGIE_CANONI.IDENTE='" & ConstSession.IdEnte & "'"
                    If Id.CompareTo("") <> 0 And Id.CompareTo("").ToString() <> "..." Then
                        SSQL+=" WHERE ID_TIPO_CANONE='" & Id.Replace("'", "''") & "'"
                    End If
                End If
                'eseguo la query
                dsAddizionali = iDB.GetDataSet(sSQL)
                Return dsAddizionali
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.GetDescrizioniAssociate.errore: ", ex)
                Throw New Exception("Problemi nell'esecuzione di GetAddizionali " + ex.Message)
            End Try
        End Function
        'Public Function GetDescrizioniAssociate(ByVal Id As String, ByVal DescrAddizionale As String, ByVal Tabella As String) As DataSet
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

        '        If Tabella = "TP_ADDIZIONALI" Then
        '            SQL = "SELECT ID_ADDIZIONALE as ID FROM TP_ADDIZIONALI_ENTE"
        '            'SSQL+=" WHERE TP_ADDIZIONALI.IDENTE='" & ConstSession.IdEnte & "'"
        '            If Id.CompareTo("") <> 0 And Id.CompareTo("").ToString() <> "..." Then
        '                SSQL+=" WHERE ID_ADDIZIONALE='" & Id.Replace("'", "''") & "'"
        '            End If
        '        ElseIf Tabella = "TP_TIPOLOGIE_CANONI" Then
        '            SQL = "SELECT ID_TIPO_CANONE as ID FROM TP_CANONI"
        '            'SSQL+=" WHERE TP_TIPOLOGIE_CANONI.IDENTE='" & ConstSession.IdEnte & "'"
        '            If Id.CompareTo("") <> 0 And Id.CompareTo("").ToString() <> "..." Then
        '                SSQL+=" WHERE ID_TIPO_CANONE='" & Id.Replace("'", "''") & "'"
        '            End If
        '        End If
        '        'eseguo la query
        '        Dim dsAddizionali As DataSet
        '        dsAddizionali = WFSessione.oSession.oAppDB.GetPrivateDataSet(SQL)
        '        Return dsAddizionali


        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generale.GetDescrizioniAssociate.errore: ", ex)
        '        Throw New Exception("Problemi nell'esecuzione di GetAddizionali " + ex.Message)
        '    Finally
        '        'chiudo la connessione
        '        If Not WFSessione.oSession Is Nothing Then
        '            WFSessione.Kill()
        '        End If
        '    End Try

        'End Function


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Verifica se la descrizione è già presente. Se non è presente effettua l'inserimento altrimenti 
        ''' comunica all'operatore di procedere all'aggiornamento di quella già presente.
        ''' </summary>
        ''' <param name="DESCRIZIONE">stringa contenente la descrizione da inserire</param>
        ''' <param name="Tabella">stringa contenente il nome della tabella</param>
        ''' <param name="strError">stringa</param>
        ''' <returns>
        ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function SetDescrizioni(ByVal IdDescr As Integer, ByVal DESCRIZIONE As String, ByVal Tabella As String, ByVal nServizioApplicazione As Integer, ByRef strError As String) As Boolean
            Dim ds As DataSet
            Dim sSQL As String = ""

            Try
                If IdDescr > 0 Then
                    ds = GetDescrizioniAssociate(IdDescr, DESCRIZIONE, Tabella)
                    If ds.Tables(0).Rows.Count > 0 Then
                        strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata!"
                        Return False
                    End If
                Else
                    ds = GetDescrizioni("", DESCRIZIONE, Tabella)
                    If Not ds Is Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                            Return False
                        End If
                    End If
                End If

                '*** 20141125 - Componente aggiuntiva sui consumi ***
                'eseguo la query
                If iDB.ExecuteNonQuery("prc_DescrTabCalcolo_IU", Nothing, New SqlParameter("@TABELLA", Tabella) _
                                       , New SqlParameter("@ID", IdDescr) _
                                       , New SqlParameter("@DESCRIZIONE", DESCRIZIONE) _
                                       , New SqlParameter("@IDSERVIZIO", nServizioApplicazione)) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
                Return True
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.SetDescrizioni.errore: ", ex)
                Throw New Exception("Problemi nell'esecuzione di SetDescrizioni " + ex.Message)
                Return False
            End Try
        End Function

        '''' -----------------------------------------------------------------------------
        '''' <summary>
        '''' Verifica se la descrizione è già associata a un ente. Se non è associata effettua l'aggiornamento altrimenti 
        '''' comunica all'operatore che l'aggiornamento non sarà effettuato.
        '''' </summary>
        '''' <param name="ID">ID tabella</param>
        '''' <param name="DESCRIZIONE">stringa contenente la descrizione nuova</param>
        '''' <param name="Tabella">stringa contenente il nome della tabella</param>
        '''' <param name="strError">stringa</param>
        '''' <returns>    
        '''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
        '''' <remarks>
        '''' </remarks>
        '''' <history>
        '''' </history>
        '''' -----------------------------------------------------------------------------
        'Public Function UpdateDescrizioni(ByVal ID As Integer, ByVal DESCRIZIONE As String, ByVal Tabella As String, ByRef strError As String) As Boolean
        '    Dim culture As IFormatProvider
        '    culture = New System.Globalization.CultureInfo("it-IT", True)
        '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
        '    Dim WFErrore As String
        '    Dim WFSessione As CreateSessione
        '    Dim myIdentity As Integer
        '    Dim obj As OggettoAddizionaleEnte

        '    Try

        '        UpdateDescrizioni = False
        '        'inizializzo la connessione
        '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
        '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
        '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
        '        End If


        '        Dim ds As DataSet
        '        ds = GetDescrizioniAssociate(ID, DESCRIZIONE, Tabella)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata!"
        '            UpdateDescrizioni = False
        '            Exit Function
        '        End If

        '        Dim SQL As String
        '        Dim DsCat As DataSet

        '        SQL = "UPDATE " & Tabella
        '        SSQL+=" SET DESCRIZIONE ='" & DESCRIZIONE.Replace("'", "''") & "'"
        '        If Tabella = "TP_ADDIZIONALI" Then
        '            SSQL+=" WHERE ID_ADDIZIONALE=" & ID
        '        ElseIf Tabella = "TP_TIPOLOGIE_CANONI" Then
        '            SSQL+=" WHERE ID_TIPO_CANONE=" & ID
        '        End If

        '        'eseguo la query
        '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))
        '        UpdateDescrizioni = True

        '    Catch ex As Exception
        '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generale.UpdateDescrizioni.errore: ", ex)
        '        Throw New Exception("Problemi nell'esecuzione di UpdateDescrizioni " + ex.Message)
        '        UpdateDescrizioni = False
        '    Finally
        '        'chiudo la connessione
        '        If Not WFSessione.oSession Is Nothing Then
        '            WFSessione.Kill()
        '        End If

        '    End Try

        'End Function

        'Public Function UpdateDescrizioni(ByVal ID As Integer, ByVal DESCRIZIONE As String, ByVal Tabella As String, ByRef strError As String) As Boolean
        '    Dim myIdentity As Integer
        '    Dim obj As OggettoAddizionaleEnte
        '    Dim ds As DataSet
        '    dim sSQL as string
        '    Dim DsCat As DataSet

        '    Try
        '        ds = GetDescrizioniAssociate(ID, DESCRIZIONE, Tabella)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strError = "Impossibile AGGIORNARE la posizione selezionata! E\' già stata associata!"
        '            UpdateDescrizioni = False
        '            Exit Function
        '        End If

        '        sSQL = "UPDATE " & Tabella
        '        sSQL+=" SET DESCRIZIONE ='" & DESCRIZIONE.Replace("'", "''") & "'"
        '        If Tabella = "TP_ADDIZIONALI" Then
        '            sSQL+=" WHERE ID_ADDIZIONALE=" & ID
        '        ElseIf Tabella = "TP_TIPOLOGIE_CANONI" Then
        '            sSQL+=" WHERE ID_TIPO_CANONE=" & ID
        '        End If
        '        'eseguo la query
        '        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
        '            Throw New Exception("errore in::" & sSQL)
        '        End If
        '        Return True
        '    Catch ex As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Generale.UpdateDescrizioni.errore: ", ex)
        '        Throw New Exception("Problemi nell'esecuzione di UpdateDescrizioni " + ex.Message)
        '        Return False
        '    End Try
        'End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Verifica se la descrizione è già presente. Se non è presente effettua l'eliminazione altrimenti 
        ''' comunica all'operatore che l'eliminazione non sarà effettuata.
        ''' </summary>
        ''' <param name="IdDescr">ID tabella</param>
        ''' <param name="Tabella">stringa contenente il nome della tabella</param>
        ''' <param name="strError">stringa</param>
        ''' <returns>''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function DeleteDescrizioni(ByVal IdDescr As Integer, ByVal Tabella As String, ByRef strError As String) As Boolean
            Dim myIdentity As Integer
            Dim ds As DataSet

            Try
                ds = GetDescrizioniAssociate(IdDescr, "", Tabella)
                If ds.Tables(0).Rows.Count > 0 Then
                    strError = "Impossibile ELIMINARE la posizione selezionata! E\' già stata associata!"
                    Return False
                End If
                'eseguo la query
                myIdentity = iDB.ExecuteNonQuery("prc_DescrTabCalcolo_D", Nothing, New SqlParameter("@TABELLA", Tabella) _
                                       , New SqlParameter("@ID", IdDescr))
                Return True

            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.DeleteDescrizioni.errore: ", ex)
                Throw New Exception("Problemi nell'esecuzione di DeleteDescrizioni " + ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' Estrae le voci di tabelle contenenti un codice e una descrizionee carica un oggetto di tipo DropDownList(combo)
        ''' richiamando la funzione "loadCombo".
        ''' </summary>
        ''' <param name="ddl">DropDownList</param>
        ''' <param name="SQL">stringa contenente la query</param>
        Public Sub LoadComboGenerale(ByVal ddl As DropDownList, ByVal SQL As String)
            Try
                'eseguo la query
                Dim dvMyDati As New DataView
                dvMyDati = iDB.GetDataView(SQL)
                loadCombo(ddl, dvMyDati)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.LoadComboGenerale.errore: ", ex)
                Throw New Exception("Problemi nell'esecuzione di LoadComboCategorie " + ex.Message)
            End Try
        End Sub

        Public Sub loadCombo(ByVal objCombo As DropDownList, ByVal objDR As SqlClient.SqlDataReader)
            objCombo.Items.Clear()
            objCombo.Items.Add("...")
            objCombo.Items(0).Value = ""
            Try
                If Not objDR Is Nothing Then
                    Do While objDR.Read
                        If Not IsDBNull(objDR(0)) Then
                            objCombo.Items.Add(objDR(0))
                            objCombo.Items(objCombo.Items.Count - 1).Value = objDR(1)
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.loadCombo.errore: ", ex)
            End Try
        End Sub

        Public Sub loadCombo(ByVal objCombo As DropDownList, ByVal objDW As DataView)
            objCombo.Items.Clear()
            objCombo.Items.Add("...")
            objCombo.Items(0).Value = ""

            Try
                If Not objDW Is Nothing Then
                    If objDW.Count <> 0 Then
                        Dim iConteggio As Integer = 0
                        Do While iConteggio <= objDW.Count - 1
                            objCombo.Items.Add(objDW.Item(iConteggio)(0))
                            objCombo.Items(objCombo.Items.Count - 1).Value = objDW.Item(iConteggio)(1)
                            iConteggio = iConteggio + 1
                        Loop
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.loadCombo.errore: ", ex)
            End Try
        End Sub

        Public Function WriteFile(ByVal sFile As String, ByVal sDatiFile As String) As Integer
            Try
                Dim MyFileToWrite As IO.StreamWriter = IO.File.AppendText(sFile)

                MyFileToWrite.WriteLine(sDatiFile)
                MyFileToWrite.Flush()

                MyFileToWrite.Close()
                Return 1
            Catch Err As Exception

                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.WriteFile.errore: ", Err)
                Return 0
            End Try
        End Function
    End Class
    ''' <summary>
    ''' Classe per la gestione delle funzioni di formattazione per le griglie
    ''' </summary>
    Public Class FunctionGrd
        Private Shared Log As ILog = LogManager.GetLogger(GetType(FunctionGrd))

        Public Function FormatNumberInGrd(ByVal nMyNumber As Integer) As String
            Try
                If nMyNumber > 0 Then
                    Return nMyNumber.ToString
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.FunctionGrd.FormatNumbrInGrd.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
            Try
                If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Or tDataGrd = DateTime.MaxValue.ToShortDateString Or tDataGrd.ToShortDateString = DateTime.MaxValue.ToShortDateString Then
                    Return ""
                Else
                    Return tDataGrd.ToShortDateString
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.FunctionGrd.FormattaDataGrd.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function GiraAnagrafiche(ByVal stringa As Object) As String
            Try
                If IsDBNull(stringa) Then
                    Return "(ANAGRAFICA DA ASSOCIARE)"
                Else
                    Dim a As String = CType(stringa, String)
                    Return a
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.FunctionGrd.GiraAnagrafiche.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormatGrdData(ByVal prdStatus As Object) As String
            Dim ModDate As New ClsGenerale.Generale
            FormatGrdData = ModDate.GiraDataFromDB(utility.stringoperation.formatstring(prdStatus))
            Return FormatGrdData
        End Function
        Public Function FormattaPeriodo(ByVal prdStatus As Object) As String
            FormattaPeriodo = utility.stringoperation.formatstring(utility.stringoperation.formatstring(prdStatus))
            Return FormattaPeriodo
        End Function
        Public Function IntForGridView(ByVal Input As Object) As String
            Dim ret As String = String.Empty
            Try
                If Not IsDBNull(Input) Then
                    If Input.ToString() = "-1" Or Input.ToString() = "-1.00" Then
                        ret = String.Empty
                    Else
                        ret = CStr(Input)
                    End If
                Else
                    ret = String.Empty
                End If
                Return ret
            Catch ex As Exception
                Log.Debug("OPENgovH2O.FunctionGrd.IntForGridView.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaIsSubContatore(ByVal nIsSubContatore As Object) As String
            Try
                If Not IsDBNull(nIsSubContatore) Then
                    If nIsSubContatore > 0 Then
                        Return "..\..\images\Bottoni\visto.png"
                    Else
                        Return "..\..\images\Bottoni\trasparente.png"
                    End If
                Else
                    Return "..\..\images\Bottoni\trasparente.png"
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.FunctionGrd.FormattalsSubContatore.errore: ", ex)
                Return "..\..\images\Bottoni\trasparente.png"
            End Try
        End Function
        Public Function FormattaToolTipSubContatore(ByVal sMatricolaPrincipale As Object) As String
            Try
                If Not IsDBNull(sMatricolaPrincipale) Then
                    If sMatricolaPrincipale <> "" Then
                        Return "Contatore Principale " & sMatricolaPrincipale
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug("OPENgovH2O.FunctionGrd.FormattaToolTipSubContatore.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaVia(ByVal Via As Object, ByVal Civico As Object, ByVal Interno As Object, ByVal Esponente As Object, ByVal Scala As Object, ByVal Foglio As Object, ByVal Numero As Object, ByVal Subalterno As Object) As String
            Dim ret As String = String.Empty
            Try
                If Not IsDBNull(Via) Then
                    ret = Via
                Else
                    ret = ""
                End If
                If Not IsDBNull(Civico) Then
                    If CStr(Civico) <> "0" Then
                        ret += " " + CStr(Civico)
                    Else
                        ret += ""
                    End If
                End If
                If Not IsDBNull(Esponente) Then
                    If CStr(Esponente) <> "" Then
                        ret += " " + CStr(Esponente)
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Scala) Then
                    If CStr(Scala) <> "" Then
                        ret += " " + CStr(Scala)
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Interno) Then
                    If CStr(Interno) <> "" Then
                        ret += " " + CStr(Interno)
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Foglio) Then
                    If CStr(Foglio) <> "" Then
                        ret += " (" + Foglio
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Numero) Then
                    If CStr(Numero) <> "" Then
                        ret += "/" + Numero
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Subalterno) Then
                    If CStr(Subalterno) <> "" Then
                        ret += "/" + Subalterno
                    Else
                        ret += ""
                    End If
                Else
                    ret += ""
                End If
                If Not IsDBNull(Foglio) Then
                    If CStr(Foglio) <> "" Then
                        ret += ")"
                    Else
                        ret += ""
                    End If
                End If

                Return ret
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.FunctionGrd.FormattaVia.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaUbicazione(ByVal oMyObj() As Object) As String
            Try
                Dim oMyImmobili() As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata

                oMyImmobili = oMyObj
                If oMyImmobili.GetUpperBound(0) = 0 Then
                    Return FormattaVia(oMyImmobili(0).sVia, oMyImmobili(0).sCivico, oMyImmobili(0).sInterno, oMyImmobili(0).sEsponente, oMyImmobili(0).sScala, oMyImmobili(0).sFoglio, oMyImmobili(0).sNumero, oMyImmobili(0).sSubalterno)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.FunctionGrd.FormattaUbicazione.errore: ", ex)
                Return ""
            End Try
        End Function
        Public Function FormattaLettura(ByVal Lettura As Object) As String
            If Not IsDBNull(Lettura) Then
                'Return "..\images\Bottoni\visto.png"
                'Modifica Christian, visto.gif non visto.png
                Return "..\..\..\images\Bottoni\visto.gif"
            Else
                'Return "..\images\Bottoni\trasparente.png"
                Return "..\..\..\images\Bottoni\trasparente.png"
            End If
        End Function
        Public Function CheckStatus(ByVal prdStatus As Object) As String
            If Utility.StringOperation.FormatInt(prdStatus) = 0 Then
                Return "No"
            Else
                Return "Si"
            End If
        End Function
        Public Function DescriAnomalia(ByVal prdStatus As Object) As String
            DescriAnomalia = New GestLetture().DescrizioneAnomalie(prdStatus)
            Return DescriAnomalia
        End Function
        ''' <summary>
        ''' Esegue la formattazione di un importo in formato valuta (es. 1,5 ----> 1,50) per la visualizzazione dei dati.
        ''' </summary>
        ''' <param name="iInput"></param>
        ''' <returns>stringa</returns>
        Public Function EuroForGridView(ByVal iInput As Object) As String
            Dim ret As String = String.Empty
            Try
                If (iInput.ToString() = "-1") Or (iInput.ToString() = "-1,00") Then

                    ret = String.Empty
                Else

                    ret = Convert.ToDecimal(iInput).ToString("N")
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Generale.EuroForGridView.errore: ", ex)
            End Try
            Return ret
        End Function
        Public Function GetTipologiaDocumento(ByVal TipoRuolo As String) As String
            Try
                If TipoRuolo = "N" Then
                    Return "NOTA DI CREDITO"
                Else
                    Return "FATTURA"
                    End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.GetTipologiaDocumento.errore: ", ex)
                Return ""
            End Try
        End Function
    End Class
End Namespace