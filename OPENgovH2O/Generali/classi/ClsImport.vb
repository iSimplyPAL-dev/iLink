Imports log4net
Imports OPENUtility
Imports System.Data
Imports System.Data.SqlClient
Imports System
Imports System.Configuration
Imports System.IO
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports AnagInterface
Imports Utility

Public Class ObjImportazione
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjImportazione))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale
    Private _Id As Integer = -1
    Private _IdEnte As String = ""
    Private _sFileAcq As String = ""
    Private _nStatoAcq As Integer = -1
    Private _sEsito As String = ""
    Private _sFileScarti As String = ""
    Private _nRcFile As Integer = 0
    Private _nRcImport As Integer = 0
    Private _nRcScarti As Integer = 0
    Private _tDataAcq As Date = Date.MaxValue
    Private _sProvenienza As String = ""
    Private _sOperatore As String = ""

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal Value As Integer)
            _Id = Value
        End Set
    End Property
    Public Property IdEnte() As String
        Get
            Return _IdEnte
        End Get
        Set(ByVal Value As String)
            _IdEnte = Value
        End Set
    End Property
    Public Property sFileAcq() As String
        Get
            Return _sFileAcq
        End Get
        Set(ByVal Value As String)
            _sFileAcq = Value
        End Set
    End Property
    Public Property nStatoAcq() As Integer
        Get
            Return _nStatoAcq
        End Get
        Set(ByVal Value As Integer)
            _nStatoAcq = Value
        End Set
    End Property
    Public Property sEsito() As String
        Get
            Return _sEsito
        End Get
        Set(ByVal Value As String)
            _sEsito = Value
        End Set
    End Property
    Public Property sFileScarti() As String
        Get
            Return _sFileScarti
        End Get
        Set(ByVal Value As String)
            _sFileScarti = Value
        End Set
    End Property
    Public Property nRcFile() As Integer
        Get
            Return _nRcFile
        End Get
        Set(ByVal Value As Integer)
            _nRcFile = Value
        End Set
    End Property
    Public Property nRcImport() As Integer
        Get
            Return _nRcImport
        End Get
        Set(ByVal Value As Integer)
            _nRcImport = Value
        End Set
    End Property
    Public Property nRcScarti() As Integer
        Get
            Return _nRcScarti
        End Get
        Set(ByVal Value As Integer)
            _nRcScarti = Value
        End Set
    End Property
    Public Property tDataAcq() As Date
        Get
            Return _tDataAcq
        End Get
        Set(ByVal Value As Date)
            _tDataAcq = Value
        End Set
    End Property
    Public Property sProvenienza() As String
        Get
            Return _sProvenienza
        End Get
        Set(ByVal Value As String)
            _sProvenienza = Value
        End Set
    End Property
    Public Property sOperatore() As String
        Get
            Return _sOperatore
        End Get
        Set(ByVal Value As String)
            _sOperatore = Value
        End Set
    End Property

    Public Structure DatiLetture
        Dim CodLettura As Integer
        Dim CodContatore As Integer
        Dim CodContatoreprecedente As Integer
        Dim CodPeriodo As Integer
        Dim DataLettura As String
        Dim Lettura As Integer
        Dim CodmMdalitaLettura As Integer
        Dim Fatturazione As Integer
        Dim Consumo As Integer
        Dim ConsumoEffettivo As Integer
        Dim Note As String
        Dim GiornidiConsumo As String
        Dim Incongruente As Integer
        Dim IncongruenteForzato As Integer
        Dim ConsumoTeorico As String
        Dim FatturazioneSospesa As Integer
        Dim NumeroUtente As String
        Dim DataLetturaPrecedente As String
        Dim LetturaPrecedente As Integer
        Dim PrimaLettura As Integer
        Dim CodUtente As Integer
        Dim UltimaLettura As Integer
        Dim ConsumoTotalePrec As Integer
        Dim GiorniDiConsumoTotPrec As Integer
        Dim DataFatturazione As String
        Dim IdStatoLettura As Integer
        Dim DataDiPassaggio As String
        Dim Cod_anomalia1 As Integer
        Dim Cod_anomalia2 As Integer
        Dim Cod_anomalia3 As Integer
        Dim LetturaTeorica As String
        Dim GiroContatore As Integer
        Dim Storica As Integer
        Dim Smat As Integer
        Dim Storicizzata As Integer
        Dim Provenienza As String
        Dim sNote As String
    End Structure

    'Public Function SetAcquisizione(ByVal oTabAcqui As ObjImportazione, ByVal DbOperation As Integer, ByVal WFSessione As CreateSessione) As Integer
    '    Try
    '        dim sSQL as string
    '        Dim myIdentity As Integer

    '        Select Case DbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TBLIMPORTAZIONE "
    '                sSQL += "(IDENTE, DATA, NOME_FILE, NOME_FILE_SCARTI, N_REC_DA_IMPORTARE, N_REC_IMPORTATI, N_REC_SCARTATI, STATO, ESISTO, "
    '                sSQL += "TIPO_IMPORTAZIONE, OPERATORE) "
    '                sSQL += "VALUES ('" & oTabAcqui.IdEnte & "', '" & oReplace.GiraData(oTabAcqui.tDataAcq) & "', '" & oTabAcqui.sFileAcq & "', "
    '                sSQL += "'" & oTabAcqui.sFileScarti & "', " & oTabAcqui.nRcFile & ", " & oTabAcqui.nRcImport & ", " & oTabAcqui.nRcScarti & ", "
    '                sSQL += oTabAcqui.nStatoAcq & ", '" & oTabAcqui.sEsito & "', '" & oTabAcqui.sProvenienza & "', '" & oTabAcqui.sOperatore & "') "
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TBLIMPORTAZIONE SET "
    '                sSQL += "IdEnte = '" & oTabAcqui.IdEnte & "', "
    '                sSQL += "Data = '" & oReplace.GiraData(oTabAcqui.tDataAcq) & "', "
    '                sSQL += "NOME_FILE = '" & oTabAcqui.sFileAcq & "', "
    '                sSQL += "NOME_FILE_SCARTI = '" & oTabAcqui.sFileScarti & "', "
    '                sSQL += "N_REC_DA_IMPORTARE = " & oTabAcqui.nRcFile & ", "
    '                sSQL += "N_REC_IMPORTATI = " & oTabAcqui.nRcImport & ", "
    '                sSQL += "N_REC_SCARTATI = " & oTabAcqui.nRcScarti & ", "
    '                sSQL += "STATO = " & oTabAcqui.nStatoAcq & ", "
    '                sSQL += "ESISTO = '" & oTabAcqui.sEsito.Replace("'", "''") & "', "
    '                sSQL += "TIPO_IMPORTAZIONE = '" & oTabAcqui.sProvenienza & "', "
    '                sSQL += "OPERATORE = '" & oTabAcqui.sOperatore & "'"
    '                sSQL += " WHERE (Id = " & oTabAcqui.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oTabAcqui.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TBLIMPORTAZIONE"
    '                sSQL += " WHERE (ID= " & oTabAcqui.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oTabAcqui.Id
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ObjImportazione.SetAcquisizione.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function MaxIdImport(ByVal idEnte As String, ByVal WFSessione As CreateSessione) As Integer
    '    Try
    '        dim sSQL as string
    '        Dim dvMyDati As new dataview
    '        Dim myIdentity As Integer = -1

    '        sSQL = "SELECT MAX(ID) AS MAXID"
    '        sSQL += " FROM TBLIMPORTAZIONE"
    '        sSQL += " WHERE (IDENTE='" & idEnte & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.HasRows Then
    '            Do While dvMyDati.Read
    '                If Not IsDBNull(myrow("maxid")) Then
    '                    myIdentity = StringOperation.FormatInt(myrow("maxid"))
    '                End If

    '            Loop
    '            dvmydati.dispose()
    '        End If

    '        Return myIdentity

    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ObjImportazione.MaxIdImport.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Public Function GetAcquisizione(ByVal WFSessione As CreateSessione, ByVal nStato As Integer, ByVal sEnte As String, Optional ByVal sProvAcq As String = "") As ObjImportazione
    '    'STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}
    '    Try
    '        dim sSQL as string
    '        Dim dvMyDati As new dataview
    '        Dim oTotAcq As New ObjImportazione

    '        sSQL = "SELECT TOP 1 *"
    '        sSQL += " FROM TBLIMPORTAZIONE"
    '        sSQL += " WHERE (IDENTE ='" & sEnte & "')"
    '        If sProvAcq <> "" Then
    '            sSQL += " AND (TIPO_IMPORTAZIONE='" & sProvAcq & "')"
    '        Else
    '            sSQL += " AND (TIPO_IMPORTAZIONE<>'TXT CMGC')"
    '        End If
    '        sSQL += " ORDER BY ID DESC"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            If StringOperation.FormatInt(myrow("STATO")) = nStato Or nStato <> 1 Then
    '                oTotAcq.Id = StringOperation.FormatInt(myrow("ID"))
    '                oTotAcq.IdEnte =StringOperation.Formatstring(myrow("IDENTE"))
    '                oTotAcq.sFileAcq =StringOperation.Formatstring(myrow("NOME_FILE"))
    '                oTotAcq.nStatoAcq = StringOperation.FormatInt(myrow("STATO"))
    '                oTotAcq.sEsito =StringOperation.Formatstring(myrow("ESISTO"))
    '                oTotAcq.sFileScarti =StringOperation.Formatstring(myrow("NOME_FILE_SCARTI"))
    '                oTotAcq.nRcFile = StringOperation.FormatInt(myrow("N_REC_DA_IMPORTARE"))
    '                oTotAcq.nRcImport = StringOperation.FormatInt(myrow("N_REC_IMPORTATI"))
    '                oTotAcq.nRcScarti = StringOperation.FormatInt(myrow("N_REC_SCARTATI"))
    '                oTotAcq.tDataAcq = oReplace.GiraDataFromDB(myrow("DATA"))
    '                oTotAcq.sOperatore =StringOperation.Formatstring(myrow("OPERATORE"))
    '                oTotAcq.sProvenienza =StringOperation.Formatstring(myrow("TIPO_IMPORTAZIONE"))
    '            End If
    '        Loop
    '        dvmydati.dispose()

    '        Return oTotAcq
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ObjImportazione.GetAcquisizione.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function SetAcquisizione(ByVal oTabAcqui As ObjImportazione, ByVal DbOperation As Integer) As Integer
        Try
            Dim sSQL As String
            Dim myIdentity As Integer

            Select Case DbOperation
                Case 0
                    sSQL = "INSERT INTO TBLIMPORTAZIONE "
                    sSQL += "(IDENTE, DATA, NOME_FILE, NOME_FILE_SCARTI, N_REC_DA_IMPORTARE, N_REC_IMPORTATI, N_REC_SCARTATI, STATO, ESISTO, "
                    sSQL += "TIPO_IMPORTAZIONE, OPERATORE) "
                    sSQL += "VALUES ('" & oTabAcqui.IdEnte & "', '" & oReplace.GiraData(oTabAcqui.tDataAcq) & "', '" & oTabAcqui.sFileAcq & "', "
                    sSQL += "'" & oTabAcqui.sFileScarti & "', " & oTabAcqui.nRcFile & ", " & oTabAcqui.nRcImport & ", " & oTabAcqui.nRcScarti & ", "
                    sSQL += oTabAcqui.nStatoAcq & ", '" & oTabAcqui.sEsito & "', '" & oTabAcqui.sProvenienza & "', '" & oTabAcqui.sOperatore & "') "
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TBLIMPORTAZIONE SET "
                    'sSQL += "IdEnte = '" & oTabAcqui.IdEnte & "', "
                    'sSQL += "Data = '" & oReplace.GiraData(oTabAcqui.tDataAcq) & "', "
                    sSQL += "NOME_FILE = '" & oTabAcqui.sFileAcq & "', "
                    sSQL += "NOME_FILE_SCARTI = '" & oTabAcqui.sFileScarti & "', "
                    sSQL += "N_REC_DA_IMPORTARE = " & oTabAcqui.nRcFile & ", "
                    sSQL += "N_REC_IMPORTATI = " & oTabAcqui.nRcImport & ", "
                    sSQL += "N_REC_SCARTATI = " & oTabAcqui.nRcScarti & ", "
                    sSQL += "STATO = " & oTabAcqui.nStatoAcq & ", "
                    sSQL += "ESISTO = '" & oTabAcqui.sEsito.Replace("'", "''") & "', "
                    sSQL += "TIPO_IMPORTAZIONE = '" & oTabAcqui.sProvenienza & "', "
                    sSQL += "OPERATORE = '" & oTabAcqui.sOperatore & "'"
                    sSQL += " WHERE (Id = " & oTabAcqui.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Return 0
                    End If
                    myIdentity = oTabAcqui.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TBLIMPORTAZIONE"
                    sSQL += " WHERE (ID= " & oTabAcqui.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Return 0
                    End If
                    myIdentity = oTabAcqui.Id
            End Select
            Return myIdentity
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ObjImportazione.SetAcquisizione.errore: ", Err)
            Return 0
        End Try
    End Function
    Public Function MaxIdImport(ByVal idEnte As String) As Integer
        Try
            Dim sSQL As String
            Dim dvMyDati As New DataView
            Dim myIdentity As Integer = -1

            sSQL = "SELECT MAX(ID) AS MAXID"
            sSQL += " FROM TBLIMPORTAZIONE"
            sSQL += " WHERE (IDENTE='" & idEnte & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    If Not IsDBNull(myRow("maxid")) Then
                        myIdentity = StringOperation.FormatInt(myRow("maxid"))
                    End If
                Next
            End If
            dvMyDati.Dispose()
            Return myIdentity

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ObjImportazione.MaxIdImport.errore: ", Err)
            Return -1
        End Try
    End Function
    Public Function GetAcquisizione(ByVal nStato As Integer, ByVal sEnte As String, Optional ByVal sProvAcq As String = "") As ObjImportazione
        'STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}
        Try
            Dim sSQL As String
            Dim dvMyDati As New DataView
            Dim oTotAcq As New ObjImportazione

            sSQL = "SELECT TOP 1 *"
            sSQL += " FROM TBLIMPORTAZIONE"
            sSQL += " WHERE (IDENTE ='" & sEnte & "')"
            If sProvAcq <> "" Then
                sSQL += " AND (TIPO_IMPORTAZIONE='" & sProvAcq & "')"
            Else
                sSQL += " AND (TIPO_IMPORTAZIONE<>'TXT CMGC')"
            End If
            sSQL += " ORDER BY ID DESC"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    If StringOperation.FormatInt(myRow("STATO")) = nStato Or nStato <> 1 Then
                        oTotAcq.Id = StringOperation.FormatInt(myRow("ID"))
                        oTotAcq.IdEnte = StringOperation.FormatString(myRow("IDENTE"))
                        oTotAcq.sFileAcq = StringOperation.FormatString(myRow("NOME_FILE"))
                        oTotAcq.nStatoAcq = StringOperation.FormatInt(myRow("STATO"))
                        oTotAcq.sEsito = StringOperation.FormatString(myRow("ESISTO"))
                        oTotAcq.sFileScarti = StringOperation.FormatString(myRow("NOME_FILE_SCARTI"))
                        oTotAcq.nRcFile = StringOperation.FormatInt(myRow("N_REC_DA_IMPORTARE"))
                        oTotAcq.nRcImport = StringOperation.FormatInt(myRow("N_REC_IMPORTATI"))
                        oTotAcq.nRcScarti = StringOperation.FormatInt(myRow("N_REC_SCARTATI"))
                        oTotAcq.tDataAcq = oReplace.GiraDataFromDB(myRow("DATA"))
                        oTotAcq.sOperatore = StringOperation.FormatString(myRow("OPERATORE"))
                        oTotAcq.sProvenienza = StringOperation.FormatString(myRow("TIPO_IMPORTAZIONE"))
                    End If
                Next
            End If
            dvMyDati.Dispose()

            Return oTotAcq
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ObjImportazione.GetAcquisizione.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class

Public Class ClsImport
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsImport))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale

    Public Sub New()
    End Sub

    'Public Sub AvviaImportazione(ByVal sParamEnv As String, ByVal sUserEnv As String, ByVal sIdApplicativoEnv As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdImport As Integer, ByVal idPeriodo As String)
    '    Dim WFSessione As CreateSessione
    '    Dim WFErrore As String = ""
    '    Dim FunctionImport As New ObjImportazione
    '    Dim oMyTotAcq As New ObjImportazione
    '    Dim pathFileSpostato As String = ""

    '    Try
    '        Dim nCheckFile As Integer
    '        Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim sErrCheckFile As String

    '        'apro la connessione
    '        WFSessione = New CreateSessione(sParamEnv, sUserEnv, sIdApplicativoEnv)
    '        If Not WFSessione.CreaSessione(sUserEnv, WFErrore) Then
    '            Throw New Exception("StartImport::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Sub
    '        End If

    '        'controllo che il formato sia corretto
    '        nCheckFile = ControllaFile(sEnteImport, sFileImport, sNameImport, sErrCheckFile)
    '        Select Case nCheckFile
    '            Case -1 'errore
    '                'sposto il file nella cartella non acquisiti
    '                pathFileSpostato = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                'System.IO.File.Delete(sFileImport)
    '                'registro l'errore acquisizione
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione:" + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sProvenienza = "Letture"
    '                FunctionImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '            Case 0 'formato non corretto
    '                'sposto il file nella cartella non acquisiti
    '                pathFileSpostato = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                'System.IO.File.Delete(sFileImport)
    '                'registro il formato non corretto di acquisizione
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sProvenienza = "Letture"
    '                FunctionImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '            Case 1 'formato corretto
    '                If AcquisizioneLetture(WFSessione, sEnteImport, sFileImport, nIdImport, sUserEnv, oMyTotAcq, idPeriodo) <= 0 Then
    '                    'sposto il file nella cartella non acquisiti
    '                    pathFileSpostato = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                    'System.IO.File.Delete(sFileImport)
    '                    'registro l'errore acquisizione
    '                    oMyTotAcq.Id = nIdImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = -1
    '                    oMyTotAcq.sEsito = "Errore durante l'importazione."
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sOperatore = HttpContext.Current.Session("username")
    '                    oMyTotAcq.sProvenienza = "Letture"
    '                    FunctionImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                Else
    '                    'sposto il file nella cartella acquisiti
    '                    pathFileSpostato = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString())
    '                    'System.IO.File.Delete(sFileImport)
    '                    'registro l'avvenuta acquisizione
    '                    oMyTotAcq.Id = nIdImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = 0
    '                    oMyTotAcq.sEsito = "Acquisizione terminata con successo!"
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sProvenienza = "Letture"
    '                    FunctionImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                End If
    '                'End If
    '            Case 2 'dati obbligatori mancanti
    '                'sposto il file nella cartella non acquisiti
    '                pathFileSpostato = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_NON_ACQUISITO").ToString())
    '                'System.IO.File.Delete(sFileImport)
    '                'registro la mancanza di dati obbligatori
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Dati obbligatori mancanti." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                FunctionImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '        End Select

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AvviaImportazione.errore: ", Err)
    '        'registro l'errore acquisizione
    '        oMyTotAcq.IdEnte = sEnteImport
    '        oMyTotAcq.sFileAcq = sFileImport
    '        oMyTotAcq.nStatoAcq = -1
    '        oMyTotAcq.sEsito = "Errore durante l'importazione."
    '        FunctionImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    'Private Function AcquisizioneLetture(ByVal WFSessione As CreateSessione, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal nIdFlussoAcq As Integer, ByVal sMyOperatore As String, ByRef oImport As ObjImportazione, ByVal idPeriodo As String) As Integer
    '    Try
    '        Dim oLettura As New ObjImportazione.DatiLetture
    '        Dim oReplace As New ClsGenerale.Generale
    '        Dim codiceUtente As Integer = -1
    '        Dim sTesseraPrec As String
    '        Dim oMyFileInfo = New System.IO.FileInfo(sFileAcq)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim sCnXLS As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileAcq + ";Extended Properties=Excel 8.0;"
    '        Dim cnXLS As New OleDb.OleDbConnection(sCnXLS)
    '        Dim salvato As Boolean
    '        Dim sqlConn As New SqlConnection
    '        Dim oDatiContatore As New objcontatore

    '        'prelevo tutti i dati dal foglio 1
    '        Dim cmXLS As New OleDb.OleDbCommand("SELECT * FROM [LETTURE$]", cnXLS)
    '        Dim dvMyDatiXLS As OleDb.OleDbDataReader
    '        Dim culture As IFormatProvider
    '        culture = New System.Globalization.CultureInfo("it-IT", True)
    '        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        'apro il file
    '        cnXLS.Open()
    '        Try
    '            dvMyDatiXLS = cmXLS.ExecuteReader(CommandBehavior.CloseConnection)
    '            Do While dvMyDatiXLS.Read()

    '                '*** conta il numero di record nel file excel
    '                oImport.nRcFile += 1

    '                'prelevo i dati dal file
    '                codiceUtente = GetCodiceUtente(WFSessione, StringOperation.FormatInt(myrowXLS("CODICE CONTATORE")))
    '                oLettura.CodContatore = StringOperation.FormatInt(myrowXLS("CODICE CONTATORE"))
    '                oLettura.CodUtente = codiceUtente
    '                oLettura.Lettura = StringOperation.FormatInt(myrowXLS("LETTURA"))
    '                oLettura.DataLettura = oReplace.GiraData(StringOperation.Formatstring(myrowXLS("DATA LETTURA")))

    '                If Not IsDBNull(dvMyDatiXLS("NUMERO UTENTE")) Then
    '                    oLettura.NumeroUtente = StringOperation.Formatstring(myrowXLS("NUMERO UTENTE"))
    '                Else
    '                    oLettura.NumeroUtente = ""
    '                End If

    '                oLettura.Provenienza = "Data Entry Massivo"
    '                oLettura.CodPeriodo = idPeriodo
    '                oLettura.CodLettura = -1
    '                oLettura.Storica = 0
    '                oLettura.Cod_anomalia1 = -1
    '                oLettura.Cod_anomalia2 = -1
    '                oLettura.Cod_anomalia3 = -1

    '                If CInt(oLettura.CodContatore) > 0 And CInt(oLettura.CodContatore) > -1 Then
    '                    '*** recupero i dati del contatore attraverso il codice contatore della stampa
    '                    oDatiContatore = TrovaContatore(oLettura.CodContatore, "", sEnteAcq, sqlConn)

    '                    '*** salvataggio dati lettura
    '                    salvato = setLettureAttuali(oDatiContatore, oLettura, sqlConn, WFSessione)

    '                    If salvato Then
    '                        '*** incrementa il numero di record salvati
    '                        oImport.nRcImport += 1
    '                    Else
    '                        oImport.nRcScarti += 1
    '                        oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport
    '                        '*** scrive il file degli scarti
    '                        If WriteScartiLetture(oLettura, sFileAcq, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport, sEnteAcq) = 0 Then
    '                            Return -1
    '                        End If
    '                    End If

    '                Else
    '                    '*** incrementa il numero di record scartati
    '                    oImport.nRcScarti += 1
    '                    oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport
    '                    '*** scrive il file degli scarti
    '                    If WriteScartiLetture(oLettura, sFileAcq, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport, sEnteAcq) = 0 Then
    '                        Return -1
    '                    End If
    '                End If
    '            Loop
    '        Catch Err As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '            Return -1
    '        Finally
    '            dvMyDatiXLS.Close()
    '            cnXLS.Close()

    '            sqlConn.Close()
    '        End Try
    '        Return 1
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Public Function setLettureAttuali(ByVal DetailContatore As objContatore, ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal conn As SqlConnection, ByVal WFSessione As CreateSessione) As Boolean
    '    '=================================================================
    '    'LETTURE NORMALI
    '    '=================================================================
    '    Try
    '        dim sSQL, dataLetturaprecedente, strValoreFondoScala As String
    '        Dim PrimaLettura As Boolean = True
    '        Dim lngConsumoAppoggio, lngValoreFondoScala, lngCifreContatore, lngCount, lngGiorniDiConsumo, lngConsumoTeoricoAppoggio, lngLetturaTeoricaAppoggio, lngLetturaPrecedente As Long
    '        Dim ModDate As New ClsGenerale.Generale
    '        Dim sqlTrans As SqlTransaction
    '        Dim sqlCmdInsert As SqlCommand
    '        Dim blnGiroContatore As Boolean = False
    '        Dim blnLETTURAVUOTA As Boolean = False
    '        Dim lngTipoOp As Long = Enumeratore.UpdateRecordStatus.Insert
    '        Dim GiorniConsumo, letturateorica, ConsumoEffettivo, ConsumoTeorico As String
    '        Dim blnLAsciatoAvviso As Boolean = False
    '        Dim blnConsumoNegativo As Boolean = False
    '        Dim IncongruenteForzato As Boolean = False
    '        Dim girocontatore As Boolean = False

    '        setLettureAttuali = True

    '        If oDatiLettura.CodLettura > 0 Then
    '            lngTipoOp = Enumeratore.UpdateRecordStatus.Updated
    '        End If

    '        If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then

    '            lngLetturaPrecedente = getLetturaPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente, WFSessione)
    '            dataLetturaprecedente = getDataPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente, WFSessione)

    '            If Len(oDatiLettura.Lettura.ToString().Trim()) = 0 Then
    '                blnLETTURAVUOTA = True
    '            End If

    '            '            If lngLetturaPrecedente <> -1 Then
    '            If lngLetturaPrecedente <> -1 Then
    '                PrimaLettura = False
    '                If Not blnLETTURAVUOTA Then
    '                    lngConsumoAppoggio = utility.stringoperation.formatint(oDatiLettura.Lettura) - lngLetturaPrecedente
    '                Else
    '                    lngConsumoAppoggio = 0
    '                End If
    '                '=================================================
    '                'se blnConsumoNegativo e true considera il consumo negativo
    '                If Not blnConsumoNegativo Then
    '                    If lngConsumoAppoggio < 0 Then
    '                        'Verifico e considero  il Giro Contatore
    '                        lngCifreContatore = utility.stringoperation.formatint(DetailContatore.sCifreContatore)
    '                        If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                            '*************************************************************
    '                            'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                            'Dall'Utente e flaggare il flag Giro Contatore
    '                            '*************************************************************
    '                            For lngCount = 1 To lngCifreContatore
    '                                strValoreFondoScala = strValoreFondoScala & "9"
    '                            Next
    '                            lngValoreFondoScala = utility.stringoperation.formatint(strValoreFondoScala)
    '                            lngConsumoAppoggio = lngValoreFondoScala - lngLetturaPrecedente
    '                            lngConsumoAppoggio = (utility.stringoperation.formatint(oDatiLettura.Lettura) - 0) + lngConsumoAppoggio
    '                            blnGiroContatore = True
    '                        End If
    '                    End If
    '                End If

    '                If IsDate(oReplace.GiraDataFromDB(utility.stringoperation.formatstring(dataLetturaprecedente))) Then
    '                    lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(oReplace.GiraDataFromDB(dataLetturaprecedente)), CDate(oReplace.GiraDataFromDB(oDatiLettura.DataLettura)))
    '                End If
    '                '***************************************************************
    '                'Se il calcolo dei giorni di consumo automatico presenta delle anomalie allora
    '                'viene forzato
    '                '***************************************************************
    '                If CStr(lngGiorniDiConsumo) <> GiorniConsumo Then
    '                    GiorniConsumo = CStr(lngGiorniDiConsumo)
    '                End If
    '                '***************************************************************
    '                'Se il calcolo del ConsumoEffettivo automatico presenta delle anomalie allora
    '                'viene forzato solo se non si tratta di giro contatore
    '                '***************************************************************
    '                If CStr(lngConsumoAppoggio) <> ConsumoEffettivo Then
    '                    If blnGiroContatore Then
    '                        ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                    Else
    '                        If girocontatore = False And IncongruenteForzato = False Then
    '                            ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                        End If
    '                    End If
    '                End If
    '                '*********************************************************************************************************************
    '                'Forzatura del consumo teorico e della lettura teorica se il calcolo automatico presenta delle
    '                'anomalie
    '                'LetturaTeorica= letturaPrecedente + consumo teorico
    '                'Calcolo Del Consumo teorico
    '                lngConsumoTeoricoAppoggio = CalcolaConsumoTeorico(lngGiorniDiConsumo, oDatiLettura.CodContatore, oDatiLettura.CodUtente, conn)
    '                lngLetturaTeoricaAppoggio = CalcolaLetturaTeorica(lngLetturaPrecedente, lngConsumoTeoricoAppoggio, DetailContatore)
    '                If CStr(lngConsumoTeoricoAppoggio) <> ConsumoTeorico Then
    '                    ConsumoTeorico = CStr(lngConsumoTeoricoAppoggio)
    '                End If
    '                If CStr(lngLetturaTeoricaAppoggio) <> letturateorica Then
    '                    letturateorica = CStr(lngLetturaTeoricaAppoggio)
    '                End If
    '                '*********************************************************************************************************************
    '            Else
    '                ConsumoEffettivo = 0
    '                GiorniConsumo = 0
    '                ConsumoTeorico = 0
    '                letturateorica = 0
    '            End If
    '        End If

    '        Try
    '            If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
    '                sSQL="INSERT INTO TP_LETTURE" & vbCrLf
    '                sSQL+="(CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA," & vbCrLf
    '                sSQL+="CODMODALITALETTURA,CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE," & vbCrLf
    '                sSQL+="IDSTATOLETTURA,INCONGRUENTE,FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE," & vbCrLf
    '                'modifica del 14/02/2007
    '                'sSQL+="STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO)" & vbCrLf
    '                sSQL+="STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)" & vbCrLf
    '                sSQL+="VALUES ( " & vbCrLf
    '                sSQL+= oDatiLettura.CodContatore & "," & vbCrLf
    '                sSQL+= oDatiLettura.CodPeriodo & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(oDatiLettura.DataLettura) & "," & vbCrLf
    '                If blnLETTURAVUOTA Then
    '                    sSQL+="Null" & "," & vbCrLf
    '                Else
    '                    sSQL+= utility.stringoperation.formatint(oDatiLettura.Lettura) & "," & vbCrLf
    '                End If
    '                sSQL+= utility.stringoperation.formatint(letturateorica) & "," & vbCrLf
    '                sSQL+="Null," & vbCrLf
    '                sSQL+= utility.stringoperation.formatint(ConsumoEffettivo) & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(oDatiLettura.sNote) & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(GiorniConsumo) & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(ConsumoTeorico) & "," & vbCrLf
    '                sSQL+= oDatiLettura.CodUtente & "," & vbCrLf
    '                sSQL+="Null," & vbCrLf
    '                sSQL+="Null" & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica e quindi e da considerarsi come fatturata
    '                'sSQL+="Null" & "," & vbCrLf
    '                sSQL+= "0 , " & vbCrLf
    '                '**********************************************************
    '                sSQL+="0," & vbCrLf
    '                sSQL+="0," & vbCrLf
    '                If blnGiroContatore Then
    '                    girocontatore = True
    '                End If
    '                sSQL+= utility.stringoperation.formatbool(girocontatore) & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica Flag Storica Alzato
    '                sSQL+="1" & "," & vbCrLf
    '                '**********************************************************
    '                sSQL+= utility.stringoperation.formatstring(dataLetturaprecedente) & "," & vbCrLf
    '                'If lngLetturaPrecedente = -1 Then
    '                If lngLetturaPrecedente = -1 Then
    '                    sSQL+= 0 & "," & vbCrLf
    '                Else
    '                    sSQL+= lngLetturaPrecedente & "," & vbCrLf
    '                End If
    '                If PrimaLettura Then
    '                    sSQL+= utility.stringoperation.formatbool(PrimaLettura) & "," & vbCrLf
    '                Else
    '                    sSQL+="Null" & "," & vbCrLf
    '                End If
    '                '*** salvataggio codice anomalia
    '                If oDatiLettura.Cod_anomalia1 = -1 Then
    '                    sSQL+="Null,"
    '                Else
    '                    sSQL+= oDatiLettura.Cod_anomalia1 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia2 = -1 Then
    '                    sSQL+="Null,"
    '                Else
    '                    sSQL+= oDatiLettura.Cod_anomalia2 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia3 = -1 Then
    '                    sSQL+="Null,"
    '                Else
    '                    sSQL+= oDatiLettura.Cod_anomalia3 & ","
    '                End If
    '                sSQL+= utility.stringoperation.formatstring(oReplace.GiraData(oDatiLettura.DataDiPassaggio)) & "," & vbCrLf
    '                sSQL+="0,'Data Entry Massivo'" & vbCrLf
    '                sSQL+=" )" & vbCrLf
    '                sqlTrans = conn.BeginTransaction
    '                sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)

    '                sqlCmdInsert.ExecuteNonQuery()

    '                If blnLAsciatoAvviso Then
    '                    sSQL="UPDATE TP_CONTATORI SET LASCIATOAVVISO=1" & vbCrLf
    '                    sSQL+="WHERE CODCONTATORE=" & oDatiLettura.CodContatore & vbCrLf

    '                    sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)
    '                    sqlCmdInsert.ExecuteNonQuery()
    '                End If
    '            End If
    '            sqlTrans.Commit()
    '            Return setLettureAttuali

    '        Catch er As Exception
    '            sqlTrans.Rollback()
    '            Return False
    '            Throw
    '        Finally
    '            sqlTrans.Dispose()
    '        End Try
    '    Catch Err As Exception
    '        Dim MyErr As String = Err.Message
    '        MyErr = MyErr + "male, male, male!!!"
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuali.errore: ", Err)
    '    End Try
    'End Function

    'Private Function GetCodiceUtente(ByVal WFSessione As CreateSessione, ByVal codiceContatore As Integer) As Integer
    '    Dim sql As String = ""
    '    Dim codUtente As Integer = 0
    '    Dim dvMyDati as sqldatareader=nothing

    '    Try
    '        sql = "SELECT * FROM TR_CONTATORI_UTENTE WHERE (CODCONTATORE = " & codiceContatore & ")"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sql)

    '        If dvMyDati.HasRows() Then
    '            Do While dvMyDati.Read()
    '                codiceContatore = dvMyDati("COD_CONTRIBUENTE").ToString()
    '            Loop
    '        End If

    '        Return codiceContatore

    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetCodiceUtente.errore: ", Err)

    '        Return -1
    '    End Try
    'End Function

    Public Sub AvviaImportazione(ByVal myStringConnection As String, Operatore As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdImport As Integer, ByVal idPeriodo As String)
        Dim FunctionImport As New ObjImportazione
        Dim oMyTotAcq As New ObjImportazione
        Dim nCheckFile As Integer
        Dim oMyFileInfo As New System.IO.FileInfo(sFileImport)
        Dim sNameImport As String = oMyFileInfo.Name
        Dim sErrCheckFile As String = ""

        Try
            'controllo che il formato sia corretto
            nCheckFile = ControllaFile(sEnteImport, sFileImport, sNameImport, sErrCheckFile)
            Log.Debug(sEnteImport + " - OPENgovH2O.ClsImport.AvviaImportazione.il controllo ha dato:" + nCheckFile.ToString())
            Select Case nCheckFile
                Case -1 'errore
                    'sposto il file nella cartella non acquisiti
                    SpostaFile(sFileImport, ConstSession.PathRepository + ConstSession.PathScarti)
                    'System.IO.File.Delete(sFileImport)
                    'registro l'errore acquisizione
                    oMyTotAcq.Id = nIdImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione:" + vbCrLf + sErrCheckFile
                    oMyTotAcq.tDataAcq = Now.Date
                    oMyTotAcq.sProvenienza = "Letture"
                    FunctionImport.SetAcquisizione(oMyTotAcq, 1)
                Case 0 'formato non corretto
                    'sposto il file nella cartella non acquisiti
                    SpostaFile(sFileImport, ConstSession.PathRepository + ConstSession.PathScarti)
                    'System.IO.File.Delete(sFileImport)
                    'registro il formato non corretto di acquisizione
                    oMyTotAcq.Id = nIdImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido." + vbCrLf + sErrCheckFile
                    oMyTotAcq.tDataAcq = Now.Date
                    oMyTotAcq.sProvenienza = "Letture"
                    FunctionImport.SetAcquisizione(oMyTotAcq, 1)
                Case 1 'formato corretto
                    If AcquisizioneLetture(myStringConnection, sEnteImport, sFileImport, nIdImport, Operatore, oMyTotAcq, idPeriodo) <= 0 Then
                        'sposto il file nella cartella non acquisiti
                        SpostaFile(sFileImport, ConstSession.PathRepository + ConstSession.PathScarti)
                        'System.IO.File.Delete(sFileImport)
                        'registro l'errore acquisizione
                        oMyTotAcq.Id = nIdImport
                        oMyTotAcq.IdEnte = sEnteImport
                        oMyTotAcq.sFileAcq = sFileImport
                        oMyTotAcq.nStatoAcq = -1
                        oMyTotAcq.sEsito = "Errore durante l'importazione."
                        oMyTotAcq.tDataAcq = Now.Date
                        oMyTotAcq.sOperatore = HttpContext.Current.Session("username")
                        oMyTotAcq.sProvenienza = "Letture"
                        FunctionImport.SetAcquisizione(oMyTotAcq, 1)
                    Else
                        'sposto il file nella cartella acquisiti
                        SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString())
                        'System.IO.File.Delete(sFileImport)
                        'registro l'avvenuta acquisizione
                        oMyTotAcq.Id = nIdImport
                        oMyTotAcq.IdEnte = sEnteImport
                        oMyTotAcq.sFileAcq = sFileImport
                        oMyTotAcq.nStatoAcq = 0
                        oMyTotAcq.sEsito = "Acquisizione terminata con successo!"
                        oMyTotAcq.tDataAcq = Now.Date
                        oMyTotAcq.sProvenienza = "Letture"
                        FunctionImport.SetAcquisizione(oMyTotAcq, 1)
                    End If
                    'End If
                Case 2 'dati obbligatori mancanti
                    'sposto il file nella cartella non acquisiti
                    SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_NON_ACQUISITO").ToString())
                    'System.IO.File.Delete(sFileImport)
                    'registro la mancanza di dati obbligatori
                    oMyTotAcq.Id = nIdImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Dati obbligatori mancanti." + vbCrLf + sErrCheckFile
                    oMyTotAcq.tDataAcq = Now.Date
                    FunctionImport.SetAcquisizione(oMyTotAcq, 1)
            End Select

        Catch Err As Exception
            Log.Debug(sEnteImport + " - OPENgovH2O.ClsImport.AvviaImportazione.errore: ", Err)
            'registro l'errore acquisizione
            oMyTotAcq.IdEnte = sEnteImport
            oMyTotAcq.sFileAcq = sFileImport
            oMyTotAcq.nStatoAcq = -1
            oMyTotAcq.sEsito = "Errore durante l'importazione."
            FunctionImport.SetAcquisizione(oMyTotAcq, 1)
        End Try
    End Sub

    'Private Function ControllaFile(ByVal sEnteCheck As String, ByVal sFileCheck As String, ByVal sNameFileCheck As String, ByRef sErrCheck As String) As Integer
    '    '{1= formato corretto; 0= formato non corretto; 2= dati obbligatori mancanti; -1= errore}
    '    Try
    '        Dim iRiga As Integer = 1
    '        'non è un file excel
    '        If Not sNameFileCheck.ToLower.EndsWith(".xls") Then
    '            sErrCheck = "Il file non è un EXCEL."
    '            Return 0
    '        End If
    '        'non è dell’ente in esame
    '        If Not sNameFileCheck.ToLower.StartsWith(sEnteCheck) Then
    '            sErrCheck = "L'ente del file non corrisponde con l'ente in esame."
    '            Return 0
    '        End If

    '        'controllo che la struttura sia corretta
    '        Dim sCnXLS As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileCheck + ";Extended Properties=Excel 8.0;"
    '        Dim cnXLS As New OleDb.OleDbConnection(sCnXLS)
    '        'prelevo tutti i dati dal foglio 1
    '        Dim cmXLS As New OleDb.OleDbCommand("SELECT * FROM [LETTURE$]", cnXLS)
    '        Dim dvMyDatiXLS As OleDb.OleDbDataReader
    '        'apro il file
    '        cnXLS.Open()
    '        Log.Debug("ClsImport::ControllaFile::aperto il file al percorso::" & sFileCheck)
    '        Try
    '            dvMyDatiXLS = cmXLS.ExecuteReader(CommandBehavior.CloseConnection)
    '            Do While dvMyDatiXLS.Read()
    '                Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.riga:" + iRiga.ToString())
    '                If Not IsDBNull(dvMyDatiXLS("CODICE CONTATORE")) Then
    '                    If StringOperation.Formatstring(myrowXLS("CODICE CONTATORE")) = "" Then
    '                        sErrCheck = "Codice contatore mancante. Riga " & iRiga
    '                        Return 2
    '                    End If
    '                Else
    '                    sErrCheck = "Manca il campo CODICE CONTATORE. Riga " & iRiga
    '                    Return 0
    '                End If
    '                If Not IsDBNull(dvMyDatiXLS("DATA LETTURA")) Then
    '                    If StringOperation.Formatstring(myrowXLS("DATA LETTURA")) = "" Then
    '                        sErrCheck = "Data Lettura non valorizzato. Riga " & iRiga
    '                        Return 2
    '                    End If
    '                Else
    '                    sErrCheck = "Manca il campo DATA LETTURA. Riga " & iRiga
    '                    Return 0
    '                End If
    '                If Not IsDBNull(dvMyDatiXLS("LETTURA")) Then
    '                    If StringOperation.Formatstring(myrowXLS("LETTURA")) = "" Then
    '                        sErrCheck = "Lettura non valorizzato Riga " & iRiga
    '                        Return 2
    '                    End If
    '                Else
    '                    sErrCheck = "Manca il campo LETTURA. Riga " & iRiga
    '                    Return 0
    '                End If
    '                iRiga += 1
    '            Loop
    '            dvMyDatiXLS.Close()
    '            Return 1
    '        Catch Err As Exception
    '            Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.errore: ", Err)
    '            sErrCheck = Err.StackTrace
    '            Return -1
    '        Finally
    '            cnXLS.Close()
    '        End Try
    '    Catch Err As Exception
    '        Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.errore: ", Err)
    '        sErrCheck = sFileCheck & "::" & Err.StackTrace & "::" & Err.Message
    '        Return -1
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnteCheck"></param>
    ''' <param name="sFileCheck"></param>
    ''' <param name="sNameFileCheck"></param>
    ''' <param name="sErrCheck"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' <revision date="15/11/2022">
    ''' Abbinamento per Matricola invece che per codice contatore
    ''' </revision>
    ''' </revisionHistory>
    Private Function ControllaFile(ByVal sEnteCheck As String, ByVal sFileCheck As String, ByVal sNameFileCheck As String, ByRef sErrCheck As String) As Integer
        '{1= formato corretto; 0= formato non corretto; 2= dati obbligatori mancanti; -1= errore}
        Try
            Dim iRiga As Integer = 1
            'non è un file excel
            If Not sNameFileCheck.ToLower.EndsWith(".csv") Then
                sErrCheck = "Non è un file CSV."
                Return 0
            End If
            'non è dell’ente in esame
            If Not sNameFileCheck.ToLower.StartsWith(sEnteCheck) Then
                sErrCheck = "L'ente del file non corrisponde con l'ente in esame."
                Return 0
            End If

            'controllo che la struttura sia corretta
            Log.Debug("ClsImport::ControllaFile::aperto il file al percorso::" & sFileCheck)
            Try
                'MATRICOLA|DATA LETTURA|LETTURA|NUMERO UTENTE
                Dim myLine As String
                Dim PostedFile() As Byte = System.IO.File.ReadAllBytes(sFileCheck)
                If Not PostedFile Is Nothing Then
                    Dim MS As New MemoryStream(PostedFile, 0, PostedFile.Length)
                    Dim MyFileToRead As New StreamReader(MS)
                    Try
                        Do While MyFileToRead.Peek > 1
                            myLine = MyFileToRead.ReadLine()
                            Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.riga:" + iRiga.ToString())
                            Try
                                If myLine = String.Empty Then
                                    sErrCheck = "Presenza riga vuota."
                                    Return 0
                                Else
                                    Dim ListDati As String()
                                    ListDati = myLine.Split(CChar(";"))
                                    If ListDati.GetUpperBound(0) < 3 Then
                                        sErrCheck = "Numero di Campi non coerenti."
                                        Return 0
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(0) <> "MATRICOLA" Then
                                            sErrCheck = "Manca il campo MATRICOLA. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(0) = "" Then
                                            sErrCheck = "Matricola mancante. Riga " & iRiga
                                            Return 2
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(1) <> "DATA LETTURA" Then
                                            sErrCheck = "Manca il campo DATA LETTURA. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(1) = "" Then
                                            sErrCheck = "Data Lettura non valorizzato. Riga " & iRiga
                                            Return 2
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(2) <> "LETTURA" Then
                                            sErrCheck = "Manca il campo LETTURA. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(2) = "" Then
                                            sErrCheck = "Lettura non valorizzato Riga " & iRiga
                                            Return 2
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(3) <> "NUMERO UTENTE" Then
                                            sErrCheck = "Manca il campo NUMERO UTENTE. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(3) = "" Then
                                            sErrCheck = "Numero Utente non valorizzato Riga " & iRiga
                                            Return 2
                                        End If
                                    End If
                                End If
                            Catch ex As Exception
                                sErrCheck = "Errore lettura file."
                                Return 0
                            End Try
                            iRiga += 1
                        Loop
                    Catch ex As Exception
                        sErrCheck = "Errore nel file."
                        Return 0
                    Finally
                        MyFileToRead.Close()
                        MS.Close()
                    End Try
                Else
                    sErrCheck = "Il file è vuoto."
                    Return 0
                End If
                Return 1
            Catch Err As Exception
                Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.errore: ", Err)
                sErrCheck = Err.StackTrace
                Return -1
            End Try
        Catch Err As Exception
            Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.errore: ", Err)
            sErrCheck = sFileCheck & "::" & Err.StackTrace & "::" & Err.Message
            Return -1
        End Try
    End Function
    'Private Function ControllaFile(ByVal sEnteCheck As String, ByVal sFileCheck As String, ByVal sNameFileCheck As String, ByRef sErrCheck As String) As Integer
    '    '{1= formato corretto; 0= formato non corretto; 2= dati obbligatori mancanti; -1= errore}
    '    Try
    '        Dim iRiga As Integer = 1
    '        'non è un file excel
    '        If Not sNameFileCheck.ToLower.EndsWith(".csv") Then
    '            sErrCheck = "Non è un file CSV."
    '            Return 0
    '        End If
    '        'non è dell’ente in esame
    '        If Not sNameFileCheck.ToLower.StartsWith(sEnteCheck) Then
    '            sErrCheck = "L'ente del file non corrisponde con l'ente in esame."
    '            Return 0
    '        End If

    '        'controllo che la struttura sia corretta
    '        Log.Debug("ClsImport::ControllaFile::aperto il file al percorso::" & sFileCheck)
    '        Try
    '            'CODICE CONTATORE|DATA LETTURA|LETTURA|NUMERO UTENTE
    '            Dim myLine As String
    '            Dim PostedFile() As Byte = System.IO.File.ReadAllBytes(sFileCheck)
    '            If Not PostedFile Is Nothing Then
    '                Dim MS As New MemoryStream(PostedFile, 0, PostedFile.Length)
    '                Dim MyFileToRead As New StreamReader(MS)
    '                Try
    '                    Do While MyFileToRead.Peek > 1
    '                        myLine = MyFileToRead.ReadLine()
    '                        Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.riga:" + iRiga.ToString())
    '                        Try
    '                            If myLine = String.Empty Then
    '                                sErrCheck = "Presenza riga vuota."
    '                                Return 0
    '                            Else
    '                                Dim ListDati As String()
    '                                ListDati = myLine.Split(CChar(";"))
    '                                If ListDati.GetUpperBound(0) < 3 Then
    '                                    sErrCheck = "Numero di Campi non coerenti."
    '                                    Return 0
    '                                End If
    '                                If iRiga = 1 Then
    '                                    If ListDati(0) <> "CODICE CONTATORE" Then
    '                                        sErrCheck = "Manca il campo CODICE CONTATORE. Riga " & iRiga
    '                                        Return 0
    '                                    End If
    '                                Else
    '                                    If ListDati(0) = "" Then
    '                                        sErrCheck = "Codice contatore mancante. Riga " & iRiga
    '                                        Return 2
    '                                    End If
    '                                End If
    '                                If iRiga = 1 Then
    '                                    If ListDati(1) <> "DATA LETTURA" Then
    '                                        sErrCheck = "Manca il campo DATA LETTURA. Riga " & iRiga
    '                                        Return 0
    '                                    End If
    '                                Else
    '                                    If ListDati(1) = "" Then
    '                                        sErrCheck = "Data Lettura non valorizzato. Riga " & iRiga
    '                                        Return 2
    '                                    End If
    '                                End If
    '                                If iRiga = 1 Then
    '                                    If ListDati(2) <> "LETTURA" Then
    '                                        sErrCheck = "Manca il campo LETTURA. Riga " & iRiga
    '                                        Return 0
    '                                    End If
    '                                Else
    '                                    If ListDati(2) = "" Then
    '                                        sErrCheck = "Lettura non valorizzato Riga " & iRiga
    '                                        Return 2
    '                                    End If
    '                                End If
    '                                If iRiga = 1 Then
    '                                    If ListDati(3) <> "NUMERO UTENTE" Then
    '                                        sErrCheck = "Manca il campo NUMERO UTENTE. Riga " & iRiga
    '                                        Return 0
    '                                    End If
    '                                Else
    '                                    If ListDati(3) = "" Then
    '                                        sErrCheck = "Numero Utente non valorizzato Riga " & iRiga
    '                                        Return 2
    '                                    End If
    '                                End If
    '                            End If
    '                        Catch ex As Exception
    '                            sErrCheck = "Errore lettura file."
    '                            Return 0
    '                        End Try
    '                        iRiga += 1
    '                    Loop
    '                Catch ex As Exception
    '                    sErrCheck = "Errore nel file."
    '                    Return 0
    '                Finally
    '                    MyFileToRead.Close()
    '                    MS.Close()
    '                End Try
    '            Else
    '                sErrCheck = "Il file è vuoto."
    '                Return 0
    '            End If
    '            Return 1
    '        Catch Err As Exception
    '            Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.errore: ", Err)
    '            sErrCheck = Err.StackTrace
    '            Return -1
    '        End Try
    '    Catch Err As Exception
    '        Log.Debug(sEnteCheck + " - OPENgovH2O.ClsImport.ControllaFile.errore: ", Err)
    '        sErrCheck = sFileCheck & "::" & Err.StackTrace & "::" & Err.Message
    '        Return -1
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdEnte"></param>
    ''' <param name="Matricola"></param>
    ''' <param name="DataLettura"></param>
    ''' <param name="CodContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' <revision date="15/11/2022">
    ''' Abbinamento per Matricola invece che per codice contatore
    ''' </revision>
    ''' </revisionHistory>
    Private Function GetCodiceUtente(IdEnte As String, ByVal Matricola As String, DataLettura As Date, ByRef CodContatore As Integer) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim myRet As Integer

        Try
            myRet = -1
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetContatoriLetture", "IDENTE", "IDPERIODO", "DAL", "AL", "GIRO", "NUMEROUTENTE", "IDVIA", "ISSUB", "INTESTATARIO", "UTENTE", "MATRICOLA", "LETTURAPRESENTE", "LETTURAMANCANTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte) _
                        , ctx.GetParam("IDPERIODO", 0) _
                        , ctx.GetParam("DAL", StringOperation.FormatDateTime(DataLettura)) _
                        , ctx.GetParam("AL", StringOperation.FormatDateTime(DataLettura)) _
                        , ctx.GetParam("GIRO", -1) _
                        , ctx.GetParam("NUMEROUTENTE", "") _
                        , ctx.GetParam("IDVIA", -1) _
                        , ctx.GetParam("ISSUB", 0) _
                        , ctx.GetParam("INTESTATARIO", "") _
                        , ctx.GetParam("UTENTE", "") _
                        , ctx.GetParam("MATRICOLA", Matricola.Replace("'", "''").Replace("*", "%")) _
                        , ctx.GetParam("LETTURAPRESENTE", 0) _
                        , ctx.GetParam("LETTURAMANCANTE", 0)
                    )
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myRet = StringOperation.FormatInt(myRow("COD_CONTRIBUENTE"))
                    CodContatore = StringOperation.FormatInt(myRow("CODCONTATORE"))
                Next
            End If
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovH2O.ClsImport.GetCodiceUtente.errore: ", Err)
        End Try
        Return myRet
    End Function
    'Private Function GetCodiceUtente(ByVal codiceContatore As Integer) As Integer
    '    Dim sql As String = ""
    '    Dim dvMyDati As New DataView

    '    Try
    '        sql = "SELECT * FROM TR_CONTATORI_UTENTE WHERE (CODCONTATORE = " & codiceContatore & ")"
    '        dvMyDati = iDB.GetDataView(sql)
    '        If Not dvMyDati Is Nothing Then
    '            For Each myRow As DataRowView In dvMyDati
    '                codiceContatore = StringOperation.FormatInt(myRow("COD_CONTRIBUENTE"))
    '            Next
    '        End If
    '        Return codiceContatore
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetCodiceUtente.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Private Function AcquisizioneLetture(ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal nIdFlussoAcq As Integer, ByVal sMyOperatore As String, ByRef oImport As ObjImportazione, ByVal idPeriodo As String) As Integer
    '    Try
    '        Dim oLettura As New ObjImportazione.DatiLetture
    '        Dim oReplace As New ClsGenerale.Generale
    '        Dim codiceUtente As Integer = -1
    '        Dim sTesseraPrec As String
    '        Dim oMyFileInfo = New System.IO.FileInfo(sFileAcq)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim sCnXLS As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileAcq + ";Extended Properties=Excel 8.0;"
    '        Dim cnXLS As New OleDb.OleDbConnection(sCnXLS)
    '        Dim salvato As Boolean
    '        Dim sqlConn As New SqlConnection
    '        Dim oDatiContatore As New objContatore

    '        'prelevo tutti i dati dal foglio 1
    '        Dim cmXLS As New OleDb.OleDbCommand("SELECT * FROM [LETTURE$]", cnXLS)
    '        Dim dvMyDatiXLS As OleDb.OleDbDataReader
    '        Dim culture As IFormatProvider
    '        culture = New System.Globalization.CultureInfo("it-IT", True)
    '        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '        Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.Inizio")
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()

    '        'apro il file
    '        cnXLS.Open()
    '        Try
    '            dvMyDatiXLS = cmXLS.ExecuteReader(CommandBehavior.CloseConnection)
    '            Do While dvMyDatiXLS.Read()

    '                '*** conta il numero di record nel file excel
    '                oImport.nRcFile += 1
    '                Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.leggo record:" + oImport.nRcFile.ToString())
    '                'prelevo i dati dal file
    '                codiceUtente = GetCodiceUtente(StringOperation.FormatInt(myrowXLS("CODICE CONTATORE")))
    '                oLettura.CodContatore = StringOperation.FormatInt(myrowXLS("CODICE CONTATORE"))
    '                oLettura.CodUtente = codiceUtente
    '                oLettura.Lettura = StringOperation.FormatInt(myrowXLS("LETTURA"))
    '                oLettura.DataLettura = oReplace.GiraData(StringOperation.Formatstring(myrowXLS("DATA LETTURA")))

    '                If Not IsDBNull(dvMyDatiXLS("NUMERO UTENTE")) Then
    '                    oLettura.NumeroUtente = StringOperation.Formatstring(myrowXLS("NUMERO UTENTE"))
    '                Else
    '                    oLettura.NumeroUtente = ""
    '                End If

    '                oLettura.Provenienza = "Data Entry Massivo"
    '                oLettura.CodPeriodo = idPeriodo
    '                oLettura.CodLettura = -1
    '                oLettura.Storica = 0
    '                oLettura.Cod_anomalia1 = -1
    '                oLettura.Cod_anomalia2 = -1
    '                oLettura.Cod_anomalia3 = -1

    '                If CInt(oLettura.CodContatore) > 0 And CInt(oLettura.CodContatore) > -1 Then
    '                    '*** recupero i dati del contatore attraverso il codice contatore della stampa
    '                    oDatiContatore = TrovaContatore(oLettura.CodContatore, "", sEnteAcq, sqlConn)

    '                    '*** salvataggio dati lettura
    '                    salvato = setLettureAttuali(oDatiContatore, oLettura, sqlConn)

    '                    If salvato Then
    '                        '*** incrementa il numero di record salvati
    '                        oImport.nRcImport += 1
    '                    Else
    '                        oImport.nRcScarti += 1
    '                        oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport
    '                        '*** scrive il file degli scarti
    '                        If WriteScartiLetture(oLettura, sFileAcq, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport, sEnteAcq) = 0 Then
    '                            Return -1
    '                        End If
    '                    End If

    '                Else
    '                    '*** incrementa il numero di record scartati
    '                    oImport.nRcScarti += 1
    '                    oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport
    '                    '*** scrive il file degli scarti
    '                    If WriteScartiLetture(oLettura, sFileAcq, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameImport, sEnteAcq) = 0 Then
    '                        Return -1
    '                    End If
    '                End If
    '            Loop
    '        Catch Err As Exception
    '            Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '            Return -1
    '        Finally
    '            dvMyDatiXLS.Close()
    '            cnXLS.Close()

    '            sqlConn.Close()
    '        End Try
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnteAcq"></param>
    ''' <param name="sFileAcq"></param>
    ''' <param name="nIdFlussoAcq"></param>
    ''' <param name="sMyOperatore"></param>
    ''' <param name="oImport"></param>
    ''' <param name="idPeriodo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' <revision date="15/11/2022">
    ''' Abbinamento per Matricola invece che per codice contatore
    ''' </revision>
    ''' </revisionHistory>
    Private Function AcquisizioneLetture(myStringConnection As String, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal nIdFlussoAcq As Integer, ByVal sMyOperatore As String, ByRef oImport As ObjImportazione, ByVal idPeriodo As String) As Integer
        Try
            Dim oLettura As New ObjImportazione.DatiLetture
            Dim oReplace As New ClsGenerale.Generale
            Dim codiceUtente As Integer = -1
            Dim oMyFileInfo As New System.IO.FileInfo(sFileAcq)
            Dim sNameImport As String = oMyFileInfo.Name
            Dim salvato As Boolean
            Dim oDatiContatore As New objContatore

            'prelevo tutti i dati dal foglio 1
            Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.Inizio")
            'apro il file
            Try
                Utility.Costanti.CreateDir(ConstSession.PathRepository + ConstSession.PathScarti)
                oImport.sFileScarti = ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport
                Dim line As String
                Dim PostedFile() As Byte = System.IO.File.ReadAllBytes(sFileAcq)
                If Not PostedFile Is Nothing Then
                    Dim MS As New MemoryStream(PostedFile, 0, PostedFile.Length)
                    Dim MyFileToRead As New StreamReader(MS)
                    MyFileToRead = IO.File.OpenText(sFileAcq)
                    Try
                        Do While MyFileToRead.Peek > 1
                            line = MyFileToRead.ReadLine()
                            Try
                                Dim ListDati As String()
                                ListDati = line.Split(CChar(";"))
                                'prelevo i dati dal file
                                If ListDati(0) <> "MATRICOLA" Then
                                    '*** conta il numero di record nel file excel
                                    oImport.nRcFile += 1
                                    oLettura = New ObjImportazione.DatiLetture
                                    oLettura.Provenienza = "Data Entry Massivo"
                                    oLettura.CodPeriodo = idPeriodo
                                    oLettura.CodLettura = -1
                                    oLettura.Storica = 0
                                    oLettura.Cod_anomalia1 = -1
                                    oLettura.Cod_anomalia2 = -1
                                    oLettura.Cod_anomalia3 = -1
                                    Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.leggo record:" + oImport.nRcFile.ToString())
                                    codiceUtente = GetCodiceUtente(sEnteAcq, ListDati(0), StringOperation.FormatDateTime(ListDati(1)), oLettura.CodContatore)
                                    If codiceUtente <= 0 Then
                                        '*** incrementa il numero di record scartati
                                        oImport.nRcScarti += 1
                                        '*** scrive il file degli scarti
                                        If WriteScartiLetture(line, sFileAcq, oImport.sFileScarti, sEnteAcq, "Utente non trovato") = 0 Then
                                            Return -1
                                        End If
                                    Else
                                        oLettura.CodUtente = codiceUtente
                                        oLettura.DataLettura = oReplace.GiraData(StringOperation.FormatDateTime(ListDati(1)))
                                        oLettura.Lettura = StringOperation.FormatInt(ListDati(2))
                                        oLettura.NumeroUtente = ListDati(3)
                                        If CInt(oLettura.CodContatore) > 0 Then
                                            '*** recupero i dati del contatore attraverso il codice contatore della stampa
                                            oDatiContatore = TrovaContatore(oLettura.CodContatore, "", sEnteAcq, myStringConnection)
                                            If oDatiContatore.sMatricola <> "" Then
                                                '*** salvataggio dati lettura
                                                salvato = setLettureAttuali(oDatiContatore, oLettura, myStringConnection)
                                                If salvato Then
                                                    '*** incrementa il numero di record salvati
                                                    oImport.nRcImport += 1
                                                Else
                                                    oImport.nRcScarti += 1
                                                    '*** scrive il file degli scarti
                                                    If WriteScartiLetture(line, sFileAcq, oImport.sFileScarti, sEnteAcq, "Errore in registrazione lettura") = 0 Then
                                                        Return -1
                                                    End If
                                                End If
                                            Else
                                                '*** incrementa il numero di record scartati
                                                oImport.nRcScarti += 1
                                                '*** scrive il file degli scarti
                                                If WriteScartiLetture(line, sFileAcq, oImport.sFileScarti, sEnteAcq, "Contatore non trovato") = 0 Then
                                                    Return -1
                                                End If
                                            End If
                                        Else
                                            '*** incrementa il numero di record scartati
                                            oImport.nRcScarti += 1
                                            '*** scrive il file degli scarti
                                            If WriteScartiLetture(line, sFileAcq, oImport.sFileScarti, sEnteAcq, "Contatore non trovato") = 0 Then
                                                Return -1
                                            End If
                                        End If
                                    End If
                                End If
                            Catch ex As Exception
                                Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore.lettura: ", ex)
                                Return -1
                            End Try
                        Loop
                    Catch Err As Exception
                        Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
                        Return -1
                    Finally
                        MyFileToRead.Close()
                        MS.Close()
                    End Try
                End If
            Catch Err As Exception
                Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
                Return -1
            End Try
            Return 1
        Catch Err As Exception
            Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
            Return -1
        End Try
    End Function
    'Private Function AcquisizioneLetture(myStringConnection As String, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal nIdFlussoAcq As Integer, ByVal sMyOperatore As String, ByRef oImport As ObjImportazione, ByVal idPeriodo As String) As Integer
    '    Try
    '        Dim oLettura As New ObjImportazione.DatiLetture
    '        Dim oReplace As New ClsGenerale.Generale
    '        Dim codiceUtente As Integer = -1
    '        Dim oMyFileInfo As New System.IO.FileInfo(sFileAcq)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim salvato As Boolean
    '        Dim oDatiContatore As New objContatore

    '        'prelevo tutti i dati dal foglio 1
    '        Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.Inizio")
    '        'apro il file
    '        Try
    '            Utility.Costanti.CreateDir(ConstSession.PathRepository + ConstSession.PathScarti)
    '            Dim line As String
    '            Dim PostedFile() As Byte = System.IO.File.ReadAllBytes(sFileAcq)
    '            If Not PostedFile Is Nothing Then
    '                Dim MS As New MemoryStream(PostedFile, 0, PostedFile.Length)
    '                Dim MyFileToRead As New StreamReader(MS)
    '                MyFileToRead = IO.File.OpenText(sFileAcq)
    '                Try
    '                    Do While MyFileToRead.Peek > 1
    '                        line = MyFileToRead.ReadLine()
    '                        Try
    '                            Dim ListDati As String()
    '                            ListDati = line.Split(CChar(";"))
    '                            'prelevo i dati dal file
    '                            If ListDati(0) <> "CODICE CONTATORE" Then
    '                                '*** conta il numero di record nel file excel
    '                                oImport.nRcFile += 1
    '                                Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.leggo record:" + oImport.nRcFile.ToString())
    '                                If IsNumeric(ListDati(0)) Then
    '                                    codiceUtente = GetCodiceUtente(CInt(ListDati(0)))
    '                                    oLettura.CodContatore = CInt(ListDati(0))
    '                                    oLettura.CodUtente = codiceUtente
    '                                    oLettura.DataLettura = oReplace.GiraData(ListDati(1))
    '                                    oLettura.Lettura = CInt(ListDati(2))
    '                                    oLettura.NumeroUtente = ListDati(3)

    '                                    oLettura.Provenienza = "Data Entry Massivo"
    '                                    oLettura.CodPeriodo = idPeriodo
    '                                    oLettura.CodLettura = -1
    '                                    oLettura.Storica = 0
    '                                    oLettura.Cod_anomalia1 = -1
    '                                    oLettura.Cod_anomalia2 = -1
    '                                    oLettura.Cod_anomalia3 = -1

    '                                    If CInt(oLettura.CodContatore) > 0 Then
    '                                        '*** recupero i dati del contatore attraverso il codice contatore della stampa
    '                                        oDatiContatore = TrovaContatore(oLettura.CodContatore, "", sEnteAcq, myStringConnection)
    '                                        If oDatiContatore.sMatricola <> "" Then
    '                                            '*** salvataggio dati lettura
    '                                            salvato = setLettureAttuali(oDatiContatore, oLettura, myStringConnection)
    '                                            If salvato Then
    '                                                '*** incrementa il numero di record salvati
    '                                                oImport.nRcImport += 1
    '                                            Else
    '                                                oImport.nRcScarti += 1
    '                                                oImport.sFileScarti = ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport
    '                                                '*** scrive il file degli scarti
    '                                                If WriteScartiLetture(oLettura, sFileAcq, ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport, sEnteAcq, "Errore in registrazione lettura") = 0 Then
    '                                                    Return -1
    '                                                End If
    '                                            End If
    '                                        Else
    '                                            '*** incrementa il numero di record scartati
    '                                            oImport.nRcScarti += 1
    '                                            oImport.sFileScarti = ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport
    '                                            '*** scrive il file degli scarti
    '                                            If WriteScartiLetture(oLettura, sFileAcq, ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport, sEnteAcq, "Contatore non trovato") = 0 Then
    '                                                Return -1
    '                                            End If
    '                                        End If
    '                                    Else
    '                                        '*** incrementa il numero di record scartati
    '                                        oImport.nRcScarti += 1
    '                                        oImport.sFileScarti = ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport
    '                                        '*** scrive il file degli scarti
    '                                        If WriteScartiLetture(oLettura, sFileAcq, ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport, sEnteAcq, "Contatore non trovato") = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    End If
    '                                Else
    '                                    '*** incrementa il numero di record scartati
    '                                    oImport.nRcScarti += 1
    '                                    oImport.sFileScarti = ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport
    '                                    '*** scrive il file degli scarti
    '                                    If WriteScartiLetture(oLettura, sFileAcq, ConstSession.PathRepository + ConstSession.PathScarti + "SCARTI_" + sNameImport, sEnteAcq, "Contatore non numerico") = 0 Then
    '                                        Return -1
    '                                    End If
    '                                End If
    '                            End If
    '                        Catch ex As Exception
    '                            Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore.lettura: ", ex)
    '                            Return -1
    '                        End Try
    '                    Loop
    '                Catch Err As Exception
    '                    Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '                    Return -1
    '                Finally
    '                    MyFileToRead.Close()
    '                    MS.Close()
    '                End Try
    '            End If
    '        Catch Err As Exception
    '            Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '            Return -1
    '        End Try
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(sEnteAcq + " - OPENgovH2O.ClsImport.AcquisizioneLetture.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

#Region "Acquisizione CMGC"
    'Private Function AcquisizioneCMGC(ByVal WFSessione As CreateSessione, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal sNameFileAcq As String, ByVal nIdFlussoAcq As Integer, ByRef oImport As ObjImportazione, ByVal sIdPeriodo As String, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer) As Integer
    '    Dim MyFileToRead As IO.StreamReader
    '    Dim sLine, sLineRead(79) As String
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim FncContratti As New GestContratti
    '    Dim FncContatori As New GestContatori
    '    Dim oNewContratto As objContratto
    '    Dim oNewContatore As objContatore
    '    Dim oNewLettura As New ObjImportazione.DatiLetture
    '    Dim nIntestatario, nUtente, nContratto As Integer
    '    Dim sqlConn As New SqlConnection

    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()
    '        oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameFileAcq
    '        'apro il file
    '        MyFileToRead = IO.File.OpenText(sFileAcq)
    '        Do While MyFileToRead.Peek > 1
    '            'conta il numero di record nel file excel
    '            oImport.nRcFile += 1
    '            'leggo la riga
    '            sLine = MyFileToRead.ReadLine
    '            sLineRead = sLine.Split(";")
    '            'controllo l'univocità del numero utente
    '            If sLineRead(0).Trim() <> "" Then
    '                If FncContratti.GetEsistente(sEnteAcq, sLineRead(0).Trim, -1) Then
    '                    oImport.nRcScarti += 1
    '                    'scrive il file degli scarti
    '                    If WriteFile(oImport.sFileScarti, "Numero Utente " & sLineRead(0).Trim & " già esistente. Il contratto non può essere salvato!") = 0 Then
    '                        Return -1
    '                    End If
    '                Else
    '                    'Non si puo inserire un codice contratto uguale ad uno precedentemente inserito
    '                    'se passa di qua, non verrà inserito un nuovo contratto in quanto il codice contratto di tipo stringa è già esistente
    '                    If sLineRead(70).Trim() = "" Or sLineRead(70).Trim() = "0000000" Or sLineRead(70).Trim() = "0" Then
    '                        nContratto += 1
    '                        sLineRead(70) = "OGU" & nContratto.ToString.PadLeft(5, "0")
    '                    End If
    '                    If FncContratti.ControllaCodice(sEnteAcq, sLineRead(70).Trim) <> "-1" Then
    '                        oImport.nRcScarti += 1
    '                        'scrive il file degli scarti
    '                        If WriteFile(oImport.sFileScarti, "Codice Contratto " & sLineRead(70).Trim & " già esistente. Il contratto non può essere salvato!") = 0 Then
    '                            Return -1
    '                        End If
    '                        'valorizzo i dati del contatore
    '                        oNewContatore = ValDatiContatore(sLineRead, nIntestatario, nUtente, -1, sEnteAcq, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto, WFSessione)
    '                        If oNewContatore.sMatricola = "Err" Then
    '                            oImport.nRcScarti += 1
    '                            'scrive il file degli scarti
    '                            If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                Return -1
    '                            End If
    '                        Else
    '                            'inserisco il contatore
    '                            If FncContatori.SetDatiContatore(0, oNewContatore, True) = False Then
    '                                oImport.nRcScarti += 1
    '                                'scrive il file degli scarti
    '                                If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                    Return -1
    '                                End If
    '                            Else
    '                                'inserisco i dati catastali
    '                                SetDatiCatastaliCMGC(oNewContatore, sqlConn)
    '                                'valorizzo i dati della lettura
    '                                oNewLettura = ValDatiLettura(sLineRead, nUtente, oNewContatore.nIdContatore, sIdPeriodo)
    '                                If oNewLettura.Provenienza = "Err" Then
    '                                    oImport.nRcScarti += 1
    '                                    'scrive il file degli scarti
    '                                    If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione della lettura per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                        Return -1
    '                                    End If
    '                                Else
    '                                    If SetLettureCMGC(oNewContatore, oNewLettura, sqlConn) = False Then
    '                                        oImport.nRcScarti += 1
    '                                        'scrive il file degli scarti
    '                                        If WriteFile(oImport.sFileScarti, "Errore nell'importazione della lettura per il Numero Utente " & sLineRead(0)) = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    Else
    '                                        oImport.nRcImport += 1
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    Else
    '                        'prelevo l'intestatario
    '                        nIntestatario = GetIdAnagrafico(sLineRead, sEnteAcq, MyUtility.TRIBUTO_H2O, 20, WFSessione)
    '                        If nIntestatario = -1 Then
    '                            oImport.nRcScarti += 1
    '                            'scrive il file degli scarti
    '                            If WriteFile(oImport.sFileScarti, "Intestario " & sLineRead(23).Trim & " " & sLineRead(24).Trim & " non trovato/inserito!") = 0 Then
    '                                Return -1
    '                            End If
    '                        Else
    '                            'prelevo l'utente
    '                            nUtente = GetIdAnagrafico(sLineRead, sEnteAcq, MyUtility.TRIBUTO_H2O, 0, WFSessione)
    '                            If nUtente = -1 Then
    '                                oImport.nRcScarti += 1
    '                                'scrive il file degli scarti
    '                                If WriteFile(oImport.sFileScarti, "Utente " & sLineRead(3).Trim & " " & sLineRead(4).Trim & " non trovato/inserito!") = 0 Then
    '                                    Return -1
    '                                End If
    '                            Else
    '                                'valorizzo i dati del contratto
    '                                oNewContratto = ValDatiContratto(sLineRead, sEnteAcq, nIntestatario, nUtente, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto, WFSessione)
    '                                If oNewContratto.sCodiceContratto = "Err" Then
    '                                    oImport.nRcScarti += 1
    '                                    'scrive il file degli scarti
    '                                    If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contratto per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                        Return -1
    '                                    End If
    '                                Else
    '                                    'inserisco il contratto
    '                                    FncContratti.SetContratto(0, sIdPeriodo, oNewContratto, True)
    '                                    'valorizzo i dati del contatore
    '                                    oNewContatore = ValDatiContatore(sLineRead, nIntestatario, nUtente, oNewContratto.nIdContratto, sEnteAcq, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto, WFSessione)
    '                                    If oNewContatore.sMatricola = "Err" Then
    '                                        oImport.nRcScarti += 1
    '                                        'scrive il file degli scarti
    '                                        If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    Else
    '                                        'inserisco il contatore
    '                                        If FncContatori.SetDatiContatore(0, oNewContatore, True) = False Then
    '                                            oImport.nRcScarti += 1
    '                                            'scrive il file degli scarti
    '                                            If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                                Return -1
    '                                            End If
    '                                        Else
    '                                            'inserisco i dati catastali
    '                                            SetDatiCatastaliCMGC(oNewContatore, sqlConn)
    '                                            'valorizzo i dati della lettura
    '                                            oNewLettura = ValDatiLettura(sLineRead, nUtente, oNewContatore.nIdContatore, sIdPeriodo)
    '                                            If oNewLettura.Provenienza = "Err" Then
    '                                                oImport.nRcScarti += 1
    '                                                'scrive il file degli scarti
    '                                                If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione della lettura per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                                    Return -1
    '                                                End If
    '                                            Else
    '                                                If SetLettureCMGC(oNewContatore, oNewLettura, sqlConn) = False Then
    '                                                    oImport.nRcScarti += 1
    '                                                    'scrive il file degli scarti
    '                                                    If WriteFile(oImport.sFileScarti, "Errore nell'importazione della lettura per il Numero Utente " & sLineRead(0)) = 0 Then
    '                                                        Return -1
    '                                                    End If
    '                                                Else
    '                                                    oImport.nRcImport += 1
    '                                                End If
    '                                            End If
    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Loop
    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AcquisizioneCMGC.errore: ", Err)
    '        Return -1
    '    Finally
    '        MyFileToRead.Close()
    '        sqlConn.Dispose()
    '    End Try
    'End Function

    'Public Sub StartImportCMGC(ByVal WFSessione As CreateSessione, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdImport As Integer, ByVal sPeriodo As String, ByVal sOperatore As String, ByVal sISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer)
    '    Dim FncImport As New ObjImportazione
    '    Dim oMyTotAcq As New ObjImportazione
    '    Dim sFileScarti As String = ""
    '    Dim nRcToImport As Integer

    '    Try
    '        Dim nCheckFile As Integer
    '        Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim sErrCheckFile As String

    '        'controllo che il formato sia corretto
    '        nCheckFile = ControllaFileCMGC(sEnteImport, sFileImport, sNameImport, nRcToImport, sErrCheckFile)
    '        Select Case nCheckFile
    '            Case -1 'errore
    '                'sposto il file nella cartella non acquisiti
    '                sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                System.IO.File.Delete(sFileImport)
    '                'registro l'errore acquisizione
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sProvenienza = "TXT CMGC"
    '                FncImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '            Case 0 'formato non corretto
    '                'sposto il file nella cartella non acquisiti
    '                sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                System.IO.File.Delete(sFileImport)
    '                'registro il formato non corretto di acquisizione
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sProvenienza = "TXT CMGC"
    '                FncImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '            Case 1 'formato corretto
    '                If AcquisizioneCMGC(1, WFSessione, sEnteImport, sFileImport, sNameImport, nIdImport, oMyTotAcq, sPeriodo, sOperatore, sISTAT, sEnteAppartenenza, nMyIDImpianto) <= 0 Then
    '                    'sposto il file nella cartella non acquisiti
    '                    sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                    System.IO.File.Delete(sFileImport)
    '                    'registro l'errore acquisizione
    '                    oMyTotAcq.Id = nIdImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = -1
    '                    oMyTotAcq.sEsito = "Errore durante l'importazione."
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sProvenienza = "TXT CMGC"
    '                    FncImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                Else
    '                    'sposto il file nella cartella acquisiti
    '                    sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString())
    '                    System.IO.File.Delete(sFileImport)
    '                    'registro l'avvenuta acquisizione
    '                    oMyTotAcq.Id = nIdImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = 0
    '                    oMyTotAcq.sEsito = "Acquisizione terminata con successo!"
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sProvenienza = "TXT CMGC"
    '                    FncImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                End If
    '                'End If
    '            Case 2 'dati obbligatori mancanti
    '                'sposto il file nella cartella non acquisiti
    '                sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_NON_ACQUISITO").ToString())
    '                System.IO.File.Delete(sFileImport)
    '                'registro la mancanza di dati obbligatori
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Dati obbligatori mancanti." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                FncImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '        End Select

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.StartImportCMGC.errore: ", Err)
    '        'registro l'errore acquisizione
    '        oMyTotAcq.IdEnte = sEnteImport
    '        oMyTotAcq.sFileAcq = sFileImport
    '        oMyTotAcq.nStatoAcq = -1
    '        oMyTotAcq.sEsito = "Errore durante l'importazione."
    '        FncImport.SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '    End Try
    'End Sub

    'Private Function AcquisizioneCMGC(ByVal IsTMP As Integer, ByVal WFSessione As CreateSessione, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal sNameFileAcq As String, ByVal nIdFlussoAcq As Integer, ByRef oImport As ObjImportazione, ByVal sIdPeriodo As String, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer) As Integer
    '    Dim MyFileToRead As IO.StreamReader
    '    Dim sLine, sLineRead(79) As String
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim FncContratti As New GestContratti
    '    Dim FncContatori As New GestContatori
    '    Dim oNewContratto As New objContratto
    '    Dim oNewContatore As objContatore
    '    Dim oNewLettura As New ObjImportazione.DatiLetture
    '    Dim nIntestatario, nUtente, nContratto As Integer
    '    Dim sqlConn As New SqlConnection
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim salvato As Boolean

    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()
    '        oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameFileAcq
    '        'apro il file
    '        MyFileToRead = IO.File.OpenText(sFileAcq)
    '        Do While MyFileToRead.Peek > 1
    '            'conta il numero di record nel file excel
    '            oImport.nRcFile += 1
    '            'leggo la riga
    '            sLine = MyFileToRead.ReadLine
    '            sLineRead = sLine.Split(";")
    '            'prelevo l'intestatario
    '            nIntestatario = GetIdAnagrafico(sLineRead, sEnteAcq, MyUtility.TRIBUTO_H2O, 20, WFSessione)
    '            If nIntestatario = -1 Then
    '                oImport.nRcScarti += 1
    '                'scrive il file degli scarti
    '                If WriteFile(oImport.sFileScarti, "Intestario " & sLineRead(23).Trim & " " & sLineRead(24).Trim & " non trovato/inserito!") = 0 Then
    '                    Return -1
    '                End If
    '            Else
    '                'prelevo l'utente
    '                nUtente = GetIdAnagrafico(sLineRead, sEnteAcq, MyUtility.TRIBUTO_H2O, 0, WFSessione)
    '                If nUtente = -1 Then
    '                    oImport.nRcScarti += 1
    '                    'scrive il file degli scarti
    '                    If WriteFile(oImport.sFileScarti, "Utente " & sLineRead(3).Trim & " " & sLineRead(4).Trim & " non trovato/inserito!") = 0 Then
    '                        Return -1
    '                    End If
    '                Else
    '                    'valorizzo i dati del contratto
    '                    oNewContratto = ValDatiContratto(sLineRead, sEnteAcq, nIntestatario, nUtente, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto, WFSessione)
    '                    If oNewContratto.sCodiceContratto = "Err" Then
    '                        oImport.nRcScarti += 1
    '                        'scrive il file degli scarti
    '                        If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contratto per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                            Return -1
    '                        End If
    '                    Else
    '                        'inserisco il contratto
    '                        FncContratti.SetContratto(0, sIdPeriodo, oNewContratto, True)
    '                        'valorizzo i dati del contatore
    '                        oNewContatore = ValDatiContatore(sLineRead, nIntestatario, nUtente, oNewContratto.nIdContratto, sEnteAcq, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto, WFSessione)
    '                        If oNewContatore.sMatricola = "Err" Then
    '                            oImport.nRcScarti += 1
    '                            'scrive il file degli scarti
    '                            If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                Return -1
    '                            End If
    '                        Else
    '                            'inserisco il contatore
    '                            If FncContatori.SetDatiContatore(0, oNewContatore, True) = False Then
    '                                oImport.nRcScarti += 1
    '                                'scrive il file degli scarti
    '                                If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                    Return -1
    '                                End If
    '                            Else
    '                                'inserisco i dati catastali
    '                                SetDatiCatastaliCMGC(oNewContatore, sqlConn)
    '                                'valorizzo i dati della lettura
    '                                oNewLettura = ValDatiLettura(sLineRead, nUtente, oNewContatore.nIdContatore, sIdPeriodo)
    '                                If oNewLettura.Provenienza = "Err" Then
    '                                    oImport.nRcScarti += 1
    '                                    'scrive il file degli scarti
    '                                    If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione della lettura per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                        Return -1
    '                                    End If
    '                                Else
    '                                    If SetLettureCMGC(oNewContatore, oNewLettura, sqlConn) = False Then
    '                                        oImport.nRcScarti += 1
    '                                        'scrive il file degli scarti
    '                                        If WriteFile(oImport.sFileScarti, "Errore nell'importazione della lettura per il Numero Utente " & sLineRead(0)) = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    Else
    '                                        oImport.nRcImport += 1
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Loop
    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AcquisizioneCMGC.errore: ", Err) 
    '        Return -1
    '    Finally
    '        MyFileToRead.Close()
    '        sqlConn.Dispose()
    '    End Try
    'End Function

    'Private Function GetIdAnagrafico(ByVal sDatiAnag() As String, ByVal sEnte As String, ByVal sTributo As String, ByVal x As Integer, ByVal WFSessione As CreateSessione) As Integer
    '    Dim FncAnagrafica As New GestioneAnagrafica()
    '    Dim oMyAnagrafica As New DettaglioAnagrafica
    '    Dim oMyAnagraficaRet As New DettaglioAnagraficaReturn

    '    Try
    '        oMyAnagrafica.COD_CONTRIBUENTE = "-1"
    '        oMyAnagrafica.ID_DATA_ANAGRAFICA = "-1"
    '        oMyAnagrafica.CodEnte = sEnte
    '        oMyAnagrafica.CodiceFiscale = sDatiAnag(x + 1).Trim
    '        If sDatiAnag(x + 2).Trim <> "00000000000" And sDatiAnag(x + 2).Trim <> "0" Then
    '            oMyAnagrafica.PartitaIva = sDatiAnag(x + 2).Trim
    '        End If
    '        oMyAnagrafica.Cognome = sDatiAnag(x + 3).Trim
    '        oMyAnagrafica.Nome = sDatiAnag(x + 4).Trim
    '        oMyAnagrafica.CodViaResidenza = sDatiAnag(x + 5).Trim
    '        If oMyAnagrafica.CodViaResidenza = "-1" Or oMyAnagrafica.CodViaResidenza = "" Then
    '            oMyAnagrafica.CodViaResidenza = -1
    '            oMyAnagrafica.ViaResidenza = sDatiAnag(x + 6).Trim + " " + sDatiAnag(x + 7).Trim
    '        Else
    '            oMyAnagrafica.ViaResidenza = GetVia(oMyAnagrafica.CodViaResidenza, sEnte)
    '            If oMyAnagrafica.ViaResidenza = "" Then
    '                oMyAnagrafica.CodViaResidenza = -1
    '                oMyAnagrafica.ViaResidenza = sDatiAnag(x + 6).Trim + " " + sDatiAnag(x + 7).Trim
    '            End If
    '        End If
    '        If sDatiAnag(x + 8).Trim <> "00000" And sDatiAnag(x + 8).Trim <> "0" Then
    '            oMyAnagrafica.CivicoResidenza = sDatiAnag(x + 8).Trim
    '        Else
    '            oMyAnagrafica.CivicoResidenza = ""
    '        End If
    '        oMyAnagrafica.CapResidenza = sDatiAnag(x + 9).Trim
    '        oMyAnagrafica.CodiceComuneResidenza = "" 'CInt(sDatiAnag(x + 10).Trim)
    '        oMyAnagrafica.ComuneResidenza = sDatiAnag(x + 11).Trim
    '        oMyAnagrafica.ProvinciaResidenza = sDatiAnag(x + 12).Trim
    '        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '        'oMyAnagrafica.ID_DATA_SPEDIZIONE = "-1"
    '        'oMyAnagrafica.CodTributo = sTributo
    '        'oMyAnagrafica.CodViaRCP = sDatiAnag(x + 13).Trim
    '        'If oMyAnagrafica.CodViaRCP = "-1" Or oMyAnagrafica.CodViaRCP = "" Then
    '        '    oMyAnagrafica.CodViaRCP = -1
    '        '    oMyAnagrafica.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
    '        '    If oMyAnagrafica.ViaRCP.Trim <> "" Then
    '        '        oMyAnagrafica.CognomeInvio = oMyAnagrafica.Cognome
    '        '        oMyAnagrafica.NomeInvio = oMyAnagrafica.Nome
    '        '    End If
    '        'Else
    '        '    oMyAnagrafica.ViaRCP = GetVia(oMyAnagrafica.CodViaRCP, sEnte)
    '        '    If oMyAnagrafica.ViaRCP.Trim = "" Then
    '        '        oMyAnagrafica.CodViaRCP = -1
    '        '        oMyAnagrafica.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
    '        '    Else
    '        '        oMyAnagrafica.CognomeInvio = oMyAnagrafica.Cognome
    '        '        oMyAnagrafica.NomeInvio = oMyAnagrafica.Nome
    '        '    End If
    '        'End If
    '        'If sDatiAnag(x + 16).Trim <> "00000" And sDatiAnag(x + 16).Trim <> "0" Then
    '        '    oMyAnagrafica.CivicoRCP = sDatiAnag(x + 16).Trim
    '        'Else
    '        '    oMyAnagrafica.CivicoRCP = ""
    '        'End If
    '        'oMyAnagrafica.CapRCP = sDatiAnag(x + 17).Trim
    '        'oMyAnagrafica.CodComuneRCP = "" 'CInt(sDatiAnag(x + 18).Trim)
    '        'oMyAnagrafica.ComuneRCP = sDatiAnag(x + 19).Trim
    '        'oMyAnagrafica.ProvinciaRCP = sDatiAnag(x + 20).Trim
    '        Dim mySped As New ObjIndirizziSpedizione
    '        mySped.CodTributo = MyUtility.TRIBUTO_H2O
    '        mySped.CodViaRCP = sDatiAnag(x + 13).Trim
    '        If mySped.CodViaRCP = "-1" Or mySped.CodViaRCP = "" Then
    '            mySped.CodViaRCP = -1
    '            mySped.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
    '            If mySped.ViaRCP.Trim <> "" Then
    '                mySped.CognomeInvio = oMyAnagrafica.Cognome
    '                mySped.NomeInvio = oMyAnagrafica.Nome
    '            End If
    '        Else
    '            mySped.ViaRCP = GetVia(mySped.CodViaRCP, sEnte)
    '            If mySped.ViaRCP.Trim = "" Then
    '                mySped.CodViaRCP = -1
    '                mySped.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
    '            Else
    '                mySped.CognomeInvio = oMyAnagrafica.Cognome
    '                mySped.NomeInvio = oMyAnagrafica.Nome
    '            End If
    '        End If
    '        If sDatiAnag(x + 16).Trim <> "00000" And sDatiAnag(x + 16).Trim <> "0" Then
    '            mySped.CivicoRCP = sDatiAnag(x + 16).Trim
    '        Else
    '            mySped.CivicoRCP = ""
    '        End If
    '        mySped.CapRCP = sDatiAnag(x + 17).Trim
    '        mySped.CodComuneRCP = "" 'CInt(sDatiAnag(x + 18).Trim)
    '        mySped.ComuneRCP = sDatiAnag(x + 19).Trim
    '        mySped.ProvinciaRCP = sDatiAnag(x + 20).Trim
    '        oMyAnagrafica.ListSpedizioni.Add(mySped)

    '        oMyAnagraficaRet = FncAnagrafica.GestisciAnagrafica(oMyAnagrafica, ConstSession.StringConnectionAnagrafica, True) 'FncAnagrafica.GestisciAnagrafica(oMyAnagrafica, ConstSession.StringConnectionAnagrafica)
    '        '*** ***
    '        Return CInt(oMyAnagraficaRet.COD_CONTRIBUENTE)
    '    Catch Err As Exception
    '        Try
    '            Dim IDContribuente As Long = FncAnagrafica.SetAnagrafica(oMyAnagrafica, ConstSession.StringConnectionAnagrafica)
    '            If IDContribuente <> -1 Then
    '                oMyAnagraficaRet.COD_CONTRIBUENTE = IDContribuente.ToString
    '            End If
    '            Return CInt(oMyAnagraficaRet.COD_CONTRIBUENTE)

    '        Catch ex As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdAnagrafico.errore: ", Err)
    '            Return -1
    '        End Try
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdAnagrafico.errore: ", Err)
    '        Return -1
    '    End Try

    'End Function

    'Private Function ValDatiContatore(ByVal sLineDati() As String, ByVal nIdIntestatario As Integer, ByVal nIdUtente As Integer, ByVal nIdContratto As Integer, ByVal sEnte As String, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer, ByVal WFSessione As CreateSessione) As objContatore
    '    Dim oMyContatore As New objContatore
    '    Dim oMyDatiCatastali As New objDatiCatastali
    '    Dim oListDatiCatastali(0) As objDatiCatastali

    '    Try
    '        oMyContatore.nIdIntestatario = nIdIntestatario
    '        oMyContatore.nIdUtente = nIdUtente
    '        oMyContatore.nIdContratto = nIdContratto
    '        If sLineDati(42).Trim <> "" Then
    '            oMyContatore.nIdVia = CInt(sLineDati(42).Trim)
    '        Else
    '            oMyContatore.nIdVia = -1
    '        End If
    '        If oMyContatore.nIdVia <= 0 Then
    '            oMyContatore.nIdVia = -1
    '            oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
    '        Else
    '            oMyContatore.sUbicazione = GetVia(oMyContatore.nIdVia, sEnte)
    '            If oMyContatore.sUbicazione = "" Then
    '                oMyContatore.nIdVia = -1
    '                oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
    '            End If
    '        End If

    '        If sLineDati(45).Trim <> "0" And sLineDati(45).Trim <> "-1" And sLineDati(45).Trim() <> "00000" And sLineDati(45).Trim <> "" Then
    '            oMyContatore.sCivico = CInt(sLineDati(45).Trim())
    '        Else
    '            oMyContatore.sCivico = ""
    '        End If

    '        'oMyContatore.sEsponente = sLineDati(46).Trim
    '        'oMyContatore.sInterno = sLineDati(47).Trim
    '        'oMyContatore.sFoglio = sLineDati(48).Trim
    '        'oMyContatore.sNumero = sLineDati(49).Trim
    '        'If sLineDati(50).Trim <> "" Then
    '        '    oMyContatore.nsubalterno = sLineDati(50).Trim
    '        'Else
    '        '    oMyContatore.nsubalterno = 0
    '        'End If

    '        'Dati agenzia entrate per i dati catastali
    '        oMyContatore.sSezioneCatast = sLineDati(51).Trim
    '        oMyContatore.sParticellaCatast = "E"
    '        oMyContatore.sEstensioneParticellaCatast = ""
    '        '/Dati agenzia entrate per i dati catastali

    '        oMyContatore.sMatricola = sLineDati(41).Trim
    '        oMyContatore.sIdEnte = sEnte
    '        oMyContatore.sIdEnteAppartenenza = sEnteAppartenenza

    '        If sLineDati(59).Trim = "S" Or sLineDati(59).Trim = "SI" Then
    '            oMyContatore.nCodFognatura = 2
    '            oMyContatore.bEsenteFognatura = 0
    '        Else
    '            oMyContatore.nCodFognatura = -1
    '            oMyContatore.bEsenteFognatura = 1
    '        End If
    '        If sLineDati(60).Trim = "S" Or sLineDati(60).Trim = "SI" Then
    '            oMyContatore.nCodDepurazione = 1
    '            oMyContatore.bEsenteDepurazione = 0
    '        Else
    '            oMyContatore.nCodDepurazione = -1
    '            oMyContatore.bEsenteDepurazione = 1
    '        End If

    '        oMyContatore.nGiro = GetIdGiro(sLineDati(52).Trim, sEnte, WFSessione)

    '        If sLineDati(53).Trim <> "0000000" And sLineDati(53).Trim <> "0" Then
    '            oMyContatore.sSequenza = sLineDati(53).Trim
    '        Else
    '            oMyContatore.sSequenza = ""
    '        End If

    '        oMyContatore.nPosizione = GetIdPosizioneContatore(sLineDati(54).Trim, WFSessione)

    '        If sLineDati(55).Trim <> "0000" And sLineDati(55).Trim <> "0" Then
    '            oMyContatore.sProgressivo = sLineDati(55).Trim
    '        Else
    '            oMyContatore.sProgressivo = ""
    '        End If

    '        oMyContatore.sLatoStrada = sLineDati(56).Trim

    '        oMyContatore.sNumeroUtente = CStr(sLineDati(0).Trim())
    '        If oMyContatore.sMatricola = "" Or oMyContatore.sMatricola = "0" Then
    '            oMyContatore.sMatricola = "OGU" & oMyContatore.sNumeroUtente.PadLeft(5, "0")
    '        End If

    '        If sLineDati(67).Trim <> "00000000" And sLineDati(67).Trim <> "0" Then
    '            If sLineDati(67).Trim.IndexOf("/") > 0 Then
    '                oMyContatore.sDataAttivazione = sLineDati(67).Trim
    '            Else
    '                oMyContatore.sDataAttivazione = oReplace.GiraDataFromDB(sLineDati(67).Trim)
    '            End If
    '        Else
    '            oMyContatore.sDataAttivazione = ""
    '        End If
    '        If oMyContatore.sDataAttivazione <> "" Then
    '            oMyContatore.bIsPendente = 0
    '        End If
    '        If sLineDati(68).Trim <> "00000000" And sLineDati(68).Trim <> "0" Then
    '            If sLineDati(68).Trim.IndexOf("/") > 0 Then
    '                oMyContatore.sDataCessazione = sLineDati(68).Trim
    '            Else
    '                oMyContatore.sDataCessazione = oReplace.GiraDataFromDB(sLineDati(68).Trim)
    '            End If
    '        Else
    '            oMyContatore.sDataCessazione = ""
    '        End If
    '        If sLineDati(66).Trim <> "" Then
    '            oMyContatore.nNumeroUtenze = sLineDati(66).Trim
    '        Else
    '            oMyContatore.nNumeroUtenze = 0
    '        End If
    '        oMyContatore.nTipoUtenza = GetIdTipoUtenza(sLineDati(63).Trim, sEnte, WFSessione)

    '        If sLineDati(61).Trim <> "000" And sLineDati(61).Trim <> "0" Then
    '            oMyContatore.nDiametroContatore = GetIdDiametroContatore(sLineDati(61).Trim, sEnte, WFSessione)
    '        Else
    '            oMyContatore.nDiametroContatore = -1
    '        End If

    '        If sLineDati(62).Trim <> "000" And sLineDati(62).Trim <> "0" Then
    '            oMyContatore.nDiametroPresa = GetIdDiametroPresa(sLineDati(62).Trim, WFSessione)
    '        Else
    '            oMyContatore.nDiametroPresa = -1
    '        End If

    '        oMyContatore.sCodiceFabbricante = sLineDati(65).Trim
    '        oMyContatore.sCifreContatore = sLineDati(58).Trim
    '        oMyContatore.nIdAttivita = GetIdAttivita(sLineDati(64).Trim, WFSessione)
    '        oMyContatore.sCodiceISTAT = sCodISTAT
    '        oMyContatore.sStatoContatore = "ATT"

    '        If sLineDati(57).Trim <> "" Then
    '            oMyContatore.nTipoContatore = GetIdTipoContatore(sLineDati(57).Trim, WFSessione)
    '        Else
    '            oMyContatore.nTipoContatore = -1
    '        End If

    '        oMyDatiCatastali.sInterno = sLineDati(47).Trim
    '        oMyDatiCatastali.sFoglio = sLineDati(48).Trim
    '        oMyDatiCatastali.sNumero = sLineDati(49).Trim
    '        If sLineDati(50).Trim <> "" Then
    '            oMyDatiCatastali.nSubalterno = sLineDati(50).Trim
    '        Else
    '            oMyDatiCatastali.nSubalterno = 0
    '        End If
    '        oListDatiCatastali(0) = oMyDatiCatastali

    '        '***DA CAMBIARE TUTTE LE VOLTE IN BASE AL COMUNE CHE SI DEVE IMPORTARE ***
    '        oMyContatore.nIdImpianto = nMyIDImpianto

    '        oMyContatore.tDataInserimento = Now
    '        oMyContatore.oDatiCatastali = oListDatiCatastali
    '        'oMyContatore.nIDSubAssociato = 0
    '        ''oMyContatore.sPiano = ""
    '        'oMyContatore.nSpesa = 0
    '        'oMyContatore.nDiritti = 0
    '        'oMyContatore.nCodIva = -1
    '        'oMyContatore.nContatorePrec = 0
    '        'oMyContatore.nContatoreSucc = 0
    '        'oMyContatore.bEsenteAcqua = 0
    '        'oMyContatore.bIgnoraMora = 0
    '        'oMyContatore.sNote = ""
    '        'oMyContatore.sDataSostituzione = ""
    '        'oMyContatore.sDataRimTemp = ""
    '        'oMyContatore.nConsumoMinimo = 0
    '        'oMyContatore.nIdContatore = 0
    '        'oMyContatore.sDataSospensioneUtenza = ""
    '        'oMyContatore.bUtenteSospeso = 0
    '        'oMyContatore.sQuoteAgevolate = ""
    '        'oMyContatore.sPenalita = ""
    '        'oMyContatore.nIdMinimo = 0
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.ValDatiContatore.errore: ", Err)
    '        oMyContatore.sMatricola = "Err"
    '    End Try
    '    Return oMyContatore
    'End Function

    'Private Function ValDatiContratto(ByVal sLineDati() As String, ByVal sEnte As String, ByVal nIdIntestatario As Integer, ByVal nIdUtente As Integer, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer, ByVal WFSessione As CreateSessione) As objContratto
    '    Dim oMyContratto As New objContratto
    '    Dim oMyContatore As New objContatore
    '    Dim oMyDatiCatastali As New objDatiCatastali
    '    Dim oListDatiCatastali(0) As objDatiCatastali

    '    Try
    '        oMyContatore.sNumeroUtente = CStr(sLineDati(0).Trim())

    '        If sLineDati(70).Trim() = "" Or sLineDati(70).Trim() = "0000000" Or sLineDati(70).Trim() = "0" Then
    '            sLineDati(70) = "OGU" & oMyContatore.sNumeroUtente.PadLeft(10, "0")
    '        End If
    '        oMyContratto.sCodiceContratto = sLineDati(70).Trim

    '        oMyContratto.nIdIntestatario = nIdIntestatario
    '        oMyContratto.nIdUtente = nIdUtente
    '        oMyContatore.nIdIntestatario = nIdIntestatario
    '        oMyContatore.nIdUtente = nIdUtente
    '        If sLineDati(42).Trim <> "" Then
    '            oMyContatore.nIdVia = sLineDati(42).Trim
    '        Else
    '            oMyContatore.nIdVia = -1
    '        End If
    '        If oMyContatore.nIdVia <= 0 Then
    '            oMyContatore.nIdVia = -1
    '            oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
    '        Else
    '            oMyContatore.sUbicazione = GetVia(oMyContatore.nIdVia, sEnte)
    '            If oMyContatore.sUbicazione = "" Then
    '                oMyContatore.nIdVia = -1
    '                oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
    '            End If
    '        End If

    '        If sLineDati(45).Trim <> "0" And sLineDati(45).Trim <> "-1" And sLineDati(45).Trim() <> "00000" And sLineDati(45).Trim <> "" Then
    '            oMyContatore.sCivico = CInt(sLineDati(45).Trim)
    '        Else
    '            oMyContatore.sCivico = ""
    '        End If

    '        oMyContatore.sEsponenteCivico = sLineDati(46).Trim

    '        'se la data sottoscrizione contratto non è valorizzata prende la data attivazione contatore
    '        If sLineDati(69).Trim() <> "00000000" And sLineDati(69).Trim() <> "0" Then
    '            If sLineDati(69).Trim.IndexOf("/") > 0 Then
    '                oMyContratto.sDataSottoscrizione = oReplace.FormattaData("A", "", sLineDati(69).Trim, False)
    '            Else
    '                oMyContratto.sDataSottoscrizione = oReplace.GiraDataFromDB(sLineDati(69).Trim)
    '            End If
    '        Else
    '            If sLineDati(67).Trim() <> "00000000" And sLineDati(67).Trim() <> "0" Then
    '                If sLineDati(67).Trim.IndexOf("/") > 0 Then
    '                    oMyContratto.sDataSottoscrizione = oReplace.FormattaData("A", "", sLineDati(67).Trim, False)
    '                Else
    '                    oMyContratto.sDataSottoscrizione = oReplace.GiraDataFromDB(sLineDati(67).Trim)
    '                End If
    '            Else
    '                oMyContratto.sDataSottoscrizione = ""
    '            End If
    '        End If

    '        If sLineDati.Length > 76 Then
    '            If IsNumeric(sLineDati(76).Trim) Then
    '                oMyContatore.nProprietario = sLineDati(76).Trim
    '            End If
    '        End If
    '        oMyDatiCatastali.sInterno = sLineDati(47).Trim
    '        oMyDatiCatastali.sFoglio = sLineDati(48).Trim
    '        oMyDatiCatastali.sNumero = sLineDati(49).Trim
    '        If sLineDati(50).Trim <> "" Then
    '            oMyDatiCatastali.nSubalterno = sLineDati(50).Trim
    '        Else
    '            oMyDatiCatastali.nSubalterno = 0
    '        End If
    '        oListDatiCatastali(0) = omydaticatastali
    '        oMyContatore.nGiro = GetIdGiro(sLineDati(52).Trim, sEnte, WFSessione)

    '        If sLineDati(53).Trim() <> "0000000" And sLineDati(53).Trim() <> "0" Then
    '            oMyContatore.sSequenza = sLineDati(53).Trim
    '        Else
    '            oMyContatore.sSequenza = ""
    '        End If

    '        oMyContatore.nPosizione = GetIdPosizioneContatore(sLineDati(54).Trim, WFSessione)

    '        If sLineDati(55).Trim() <> "0000" And sLineDati(55).Trim() <> "0" Then
    '            oMyContatore.sProgressivo = sLineDati(55).Trim
    '        Else
    '            oMyContatore.sProgressivo = ""
    '        End If

    '        oMyContatore.sLatoStrada = sLineDati(56).Trim
    '        If sLineDati(59).Trim = "S" Or sLineDati(59).Trim = "SI" Then
    '            oMyContatore.nCodFognatura = 2
    '            oMyContatore.bEsenteFognatura = 0
    '        Else
    '            oMyContatore.nCodFognatura = -1
    '            oMyContatore.bEsenteFognatura = 2
    '        End If
    '        If sLineDati(60).Trim = "S" Or sLineDati(60).Trim = "SI" Then
    '            oMyContatore.nCodDepurazione = 1
    '            oMyContatore.bEsenteDepurazione = 0
    '        Else
    '            oMyContatore.nCodDepurazione = -1
    '            oMyContatore.bEsenteDepurazione = 1
    '        End If
    '        If sLineDati(67).Trim <> "00000000" And sLineDati(67).Trim <> "0" Then
    '            If sLineDati(67).Trim.IndexOf("/") > 0 Then
    '                oMyContatore.sDataAttivazione = oReplace.FormattaData("A", "", sLineDati(67).Trim, False)
    '            Else
    '                oMyContatore.sDataAttivazione = oReplace.GiraDataFromDB(sLineDati(67).Trim)
    '            End If
    '        Else
    '            oMyContatore.sDataAttivazione = ""
    '        End If
    '        If oMyContatore.sDataAttivazione <> "" Then
    '            oMyContatore.bIsPendente = 0
    '        End If
    '        If sLineDati(68).Trim <> "00000000" And sLineDati(68).Trim <> "0" Then
    '            If sLineDati(68).Trim.IndexOf("/") > 0 Then
    '                oMyContratto.sDataCessazione = oReplace.FormattaData("A", "", sLineDati(68).Trim, False)
    '            Else
    '                oMyContratto.sDataCessazione = oReplace.GiraDataFromDB(sLineDati(68).Trim)
    '            End If
    '        Else
    '            oMyContratto.sDataCessazione = ""
    '        End If
    '        oMyContatore.nTipoUtenza = GetIdTipoUtenza(sLineDati(63).Trim, sEnte, WFSessione)

    '        If sLineDati(61).Trim <> "000" And sLineDati(61).Trim <> "0" Then
    '            oMyContatore.nDiametroContatore = GetIdDiametroContatore(sLineDati(61).Trim, sEnte, WFSessione)
    '        Else
    '            oMyContatore.nDiametroContatore = -1
    '        End If

    '        If sLineDati(62).Trim <> "000" And sLineDati(62).Trim <> "0" Then
    '            oMyContatore.nDiametroPresa = GetIdDiametroPresa(sLineDati(62).Trim, WFSessione)
    '        Else
    '            oMyContatore.nDiametroPresa = -1
    '        End If

    '        If sLineDati(66).Trim <> "" Then
    '            oMyContatore.nNumeroUtenze = sLineDati(66).Trim
    '        End If
    '        oMyContatore.sCodiceFabbricante = sLineDati(65).Trim
    '        oMyContatore.sCifreContatore = sLineDati(58).Trim
    '        oMyContatore.sCodiceISTAT = sCodISTAT
    '        oMyContatore.nIdAttivita = GetIdAttivita(sLineDati(64).Trim, WFSessione)
    '        oMyContratto.sIdEnte = sEnte
    '        oMyContatore.sIdEnteAppartenenza = sEnteAppartenenza
    '        oMyContatore.sStatoContatore = "ATT"
    '        '***DA CAMBIARE TUTTE LE VOLTE***
    '        oMyContatore.nIdImpianto = nMyIDImpianto

    '        'oMyContratto.sCodiceContratto = 0
    '        'oMyContatore.nTipoContatore = -1
    '        'oMyContatore.bEsenteAcqua = 0
    '        'oMyContatore.bIgnoraMora = 0
    '        'oMyContatore.bUtenteSospeso = 0
    '        'oMyContatore.sDataSospensioneUtenza = ""
    '        'oMyContratto.sNote = ""
    '        'oMyContatore.nCodIva = -1
    '        'oMyContatore.sPenalita = ""

    '        oMyContatore.oDatiCatastali = oListDatiCatastali
    '        oMyContratto.oContatore = oMyContatore
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.ValDatiContratto.errore: ", Err)
    '        oMyContratto.sCodiceContratto = "Err"
    '    End Try
    '    Return oMyContratto
    'End Function

    'Private Function GetIdGiro(ByVal sGiro As String, ByVal sEnte As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Dim a As Integer = 0

    '    Try
    '        If sGiro = "FRAZ. CHANTE'" Then
    '            a = a + 1
    '        End If
    '        sSQL = "SELECT TP_GIRI.IDGIRO AS MYID"
    '        sSQL += " FROM TP_GIRI"
    '        sSQL += " WHERE (TP_GIRI.CODENTE='" & sEnte & "') AND (TP_GIRI.DESCRIZIONE='" & sGiro.Replace("'", "''") & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_GIRI(DESCRIZIONE,CODENTE)"
    '            sSQL += " VALUES ('" & sGiro.Replace("'", "''") & "','" & sEnte & "')"
    '            sSQL += " SELECT @@IDENTITY"
    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '                sSQL = "UPDATE TP_GIRI SET TP_GIRI.COD_GIRO_EST=" & myIdentity
    '                sSQL += " WHERE (TP_GIRI.IDGIRO=" & myIdentity & ")"
    '                WFSessione.oSession.oAppDB.Execute(sSQL)
    '            Loop
    '            dvmydati.dispose()
    '        End If
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdGiro.errore: ", Err)
    '        Return -1
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Private Function GetIdPosizioneContatore(ByVal sPosizione As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Try
    '        sSQL = "SELECT TP_POSIZIONECONTATORE.CODPOSIZIONE AS MYID"
    '        sSQL += " FROM TP_POSIZIONECONTATORE"
    '        sSQL += " WHERE (TP_POSIZIONECONTATORE.DESCRIZIONE='" & sPosizione & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_POSIZIONECONTATORE(DESCRIZIONE)"
    '            sSQL += " VALUES ('" & sPosizione & "')"
    '            sSQL += " SELECT @@IDENTITY"
    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '                sSQL = "UPDATE TP_POSIZIONECONTATORE SET TP_POSIZIONECONTATORE.POSIZIONE=" & myIdentity
    '                sSQL += " WHERE (TP_POSIZIONECONTATORE.CODPOSIZIONE=" & myIdentity & ")"
    '                WFSessione.oSession.oAppDB.Execute(sSQL)
    '            Loop
    '            dvmydati.dispose()
    '        End If
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdPosizioneContatore.errore: ", Err)
    '        Return -1
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Private Function GetIdTipoUtenza(ByVal sUtenza As String, ByVal sEnte As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Try
    '        sSQL = "SELECT TP_TIPIUTENZA.IDTIPOUTENZA AS MYID"
    '        sSQL += " FROM TP_TIPIUTENZA"
    '        sSQL += " WHERE (TP_TIPIUTENZA.COD_ENTE='" & sEnte & "') AND (TP_TIPIUTENZA.DESCRIZIONE='" & sUtenza & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_TIPIUTENZA(DESCRIZIONE,COD_ENTE)"
    '            sSQL += " VALUES ('" & sUtenza & "','" & sEnte & "')"
    '            sSQL += " SELECT @@IDENTITY"
    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '            Loop
    '            dvmydati.dispose()
    '        End If
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdTipoUtenza.errore: ", Err)
    '        Return -1
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Private Function GetIdDiametroContatore(ByVal sDiametro As String, ByVal sEnte As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Try
    '        sSQL = "SELECT TP_DIAMETROCONTATORE.CODDIAMETROCONTATORE AS MYID"
    '        sSQL += " FROM TP_DIAMETROCONTATORE"
    '        sSQL += " WHERE (TP_DIAMETROCONTATORE.CODICE_ISTAT ='00" & sEnte & "') AND (TP_DIAMETROCONTATORE.DESCRIZIONE='" & sDiametro & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_DIAMETROCONTATORE(DESCRIZIONE,DIAMETROCONTATORE,CODICE_ISTAT,PREVALENTE)"
    '            sSQL += " VALUES ('" & sDiametro & "','" & sDiametro & "','00" & sEnte & "',0)"
    '            sSQL += " SELECT @@IDENTITY"
    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '            Loop
    '            dvmydati.dispose()
    '        End If
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdDiametroContatore.errore: ", Err)
    '        Return -1
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Private Function GetIdDiametroPresa(ByVal sPresa As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Try
    '        sSQL = "SELECT TP_DIAMETROPRESA.CODDIAMETROPRESA AS MYID"
    '        sSQL += " FROM TP_DIAMETROPRESA"
    '        sSQL += " WHERE (TP_DIAMETROPRESA.DESCRIZIONE='" & sPresa & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_DIAMETROPRESA(DESCRIZIONE,DIAMETROPRESA)"
    '            sSQL += " VALUES ('" & sPresa & "','" & sPresa & "')"
    '            sSQL += " SELECT @@IDENTITY"
    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '            Loop
    '            dvmydati.dispose()
    '        End If
    '        Return myIdentity
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdDiametroPresa.errore: ", Err)
    '        Return -1
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Private Function GetIdAttivita(ByVal sAttivita As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Try
    '        sSQL = "SELECT TP_TIPOATTIVITA.IDTIPOATTIVITA AS MYID"
    '        sSQL += " FROM TP_TIPOATTIVITA"
    '        sSQL += " WHERE (TP_TIPOATTIVITA.DESCRIZIONE='" & sAttivita & "')"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_TIPOATTIVITA(DESCRIZIONE)"
    '            sSQL += " VALUES ('" & sAttivita & "')"
    '            sSQL += " SELECT @@IDENTITY"
    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '            Loop
    '            dvmydati.dispose()
    '        End If
    '        Return myIdentity
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdAttivita.errore: ", Err)
    '        Return -1
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Private Function GetIdTipoContatore(ByVal sTipoContatore As String, ByVal WFSessione As CreateSessione) As Integer
    '    dim sSQL as string
    '    Dim dvMyDati as sqldatareader=nothing
    '    Dim myIdentity As Integer

    '    Try
    '        sSQL = "SELECT IDTIPOCONTATORE AS MYID "
    '        sSQL += " FROM TP_TIPOCONTATORE "
    '        sSQL += "WHERE (DESCRIZIONE = '" & sTipoContatore & "')"

    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        If dvMyDati.Read Then
    '            myIdentity = StringOperation.FormatInt(myrow("myid"))
    '        Else
    '            sSQL = "INSERT INTO TP_TIPOCONTATORE (DESCRIZIONE)"
    '            sSQL += " VALUES('" & sTipoContatore & "')"
    '            sSQL += " SELECT @@IDENTITY"


    '            'eseguo la query
    '            Dim dvMyDati As new dataview
    '            dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '            Do While dvMyDati.Read
    '                myIdentity = myrow(0)
    '            Loop
    '            dvmydati.dispose()
    '        End If

    '        Return myIdentity

    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdTipoContatore.errore: ", Err)
    '        Return -1

    '    Finally
    '        dvmydati.dispose()
    '    End Try

    'End Function

    'Public Function getLetturaPrecedente(ByVal DataLetturaAttuale As String, ByVal IDContatore As Long, ByVal IDUtente As Long, ByVal WFSessione As CreateSessione) As Long
    '    dim sSQL as string
    '    Dim DrTemp as sqldatareader=nothing
    '    Dim lngLetturaPrecedente As Long = -1

    '    sSQL = "SELECT TOP 1 LETTURA,DATALETTURA "
    '    sSQL += "FROM TP_LETTURE "
    '    sSQL += " WHERE "
    '    sSQL += "CODCONTATORE=" & IDContatore
    '    'sSQL += " AND  CODUTENTE=" & IDUtente
    '    sSQL += " AND  (STORICIZZATA=0 OR STORICIZZATA IS NULL)"
    '    sSQL += " AND  DATALETTURA < " & utility.stringoperation.formatstring(DataLetturaAttuale)
    '    sSQL += " ORDER BY DATALETTURA DESC"
    '    DrTemp = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '    If DrTemp.Read Then
    '        If Not IsDBNull(DrTemp("lettura")) Then
    '            lngLetturaPrecedente = CLng(DrTemp("lettura"))
    '        End If
    '    End If
    '    getLetturaPrecedente = lngLetturaPrecedente
    '    DrTemp.Close()

    'End Function

    'Public Function getDataPrecedente(ByVal DataLetturaAttuale As String, ByVal IDContatore As Long, ByVal IDUtente As Long, ByVal WFSessione As CreateSessione) As String
    '    dim sSQL as string
    '    Dim DrTemp as sqldatareader=nothing
    '    Dim dataLetturaprecedente As String = ""

    '    getDataPrecedente = ""

    '    sSQL = "SELECT TOP 1 LETTURA,DATALETTURA"
    '    sSQL += " FROM TP_LETTURE"
    '    sSQL += " WHERE"
    '    sSQL += " CODCONTATORE=" & IDContatore
    '    'sSQL += " AND CODUTENTE=" & IDUtente
    '    sSQL += " AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)"
    '    sSQL += " AND DATALETTURA < " & utility.stringoperation.formatstring(DataLetturaAttuale)
    '    sSQL += " ORDER BY DATALETTURA DESC"
    '    DrTemp = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '    If DrTemp.Read Then
    '        If Not IsDBNull(DrTemp("datalettura")) Then
    '            dataLetturaprecedente = CStr(DrTemp("datalettura"))
    '        End If
    '    End If
    '    DrTemp.Close()

    '    getDataPrecedente = dataLetturaprecedente
    'End Function

    'Public Function setLettureAttuali(ByVal ForzaLett As Boolean, ByVal DetailContatore As objContatore, ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal conn As SqlConnection, ByVal WFSessione As CreateSessione) As Boolean
    '    '=================================================================
    '    'LETTURE NORMALI
    '    '=================================================================
    '    Try
    '        dim sSQL, dataLetturaprecedente, strValoreFondoScala As String
    '        Dim PrimaLettura As Boolean = True
    '        Dim lngConsumoAppoggio, lngValoreFondoScala, lngCifreContatore, lngCount, lngGiorniDiConsumo, lngConsumoTeoricoAppoggio, lngLetturaTeoricaAppoggio, lngLetturaPrecedente As Long
    '        Dim ModDate As New ClsGenerale.Generale
    '        Dim sqlTrans As SqlTransaction
    '        Dim sqlCmdInsert As SqlCommand
    '        Dim blnGiroContatore As Boolean = False
    '        Dim blnLETTURAVUOTA As Boolean = False
    '        Dim lngTipoOp As Long = Enumeratore.UpdateRecordStatus.Insert
    '        Dim GiorniConsumo, letturateorica, ConsumoEffettivo, ConsumoTeorico As String
    '        Dim blnLAsciatoAvviso As Boolean = False
    '        Dim blnConsumoNegativo As Boolean = False
    '        Dim IncongruenteForzato As Boolean = False
    '        Dim girocontatore As Boolean = False

    '        setLettureAttuali = True

    '        If oDatiLettura.CodLettura > 0 Then
    '            lngTipoOp = Enumeratore.UpdateRecordStatus.Updated
    '        End If

    '        If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
    '            If ForzaLett = True Then
    '                lngLetturaPrecedente = oDatiLettura.LetturaPrecedente
    '                dataLetturaprecedente = oDatiLettura.DataLetturaPrecedente
    '            Else
    '                lngLetturaPrecedente = getLetturaPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente, WFSessione)
    '                dataLetturaprecedente = getDataPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente, WFSessione)
    '            End If
    '            If Len(oDatiLettura.Lettura.ToString().Trim()) = 0 Then
    '                blnLETTURAVUOTA = True
    '            End If

    '            If lngLetturaPrecedente <> -1 Then
    '                PrimaLettura = False
    '                If Not blnLETTURAVUOTA Then
    '                    lngConsumoAppoggio = utility.stringoperation.formatint(oDatiLettura.Lettura) - lngLetturaPrecedente
    '                Else
    '                    lngConsumoAppoggio = 0
    '                End If
    '                '=================================================
    '                'se blnConsumoNegativo e true considera il consumo negativo
    '                If Not blnConsumoNegativo Then
    '                    If lngConsumoAppoggio < 0 Then
    '                        'Verifico e considero  il Giro Contatore
    '                        lngCifreContatore = utility.stringoperation.formatint(DetailContatore.sCifreContatore)
    '                        If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                            '*************************************************************
    '                            'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                            'Dall'Utente e flaggare il flag Giro Contatore
    '                            '*************************************************************
    '                            For lngCount = 1 To lngCifreContatore
    '                                strValoreFondoScala = strValoreFondoScala & "9"
    '                            Next
    '                            lngValoreFondoScala = utility.stringoperation.formatint(strValoreFondoScala)
    '                            lngConsumoAppoggio = lngValoreFondoScala - lngLetturaPrecedente
    '                            lngConsumoAppoggio = (utility.stringoperation.formatint(oDatiLettura.Lettura) - 0) + lngConsumoAppoggio
    '                            blnGiroContatore = True
    '                        End If
    '                    End If
    '                End If

    '                If IsDate(oReplace.GiraDataFromDB(utility.stringoperation.formatstring(dataLetturaprecedente))) Then
    '                    lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(oReplace.GiraDataFromDB(dataLetturaprecedente)), CDate(oReplace.GiraDataFromDB(oDatiLettura.DataLettura)))
    '                End If
    '                '***************************************************************
    '                'Se il calcolo dei giorni di consumo automatico presenta delle anomalie allora
    '                'viene forzato
    '                '***************************************************************
    '                If CStr(lngGiorniDiConsumo) <> GiorniConsumo Then
    '                    GiorniConsumo = CStr(lngGiorniDiConsumo)
    '                End If
    '                '***************************************************************
    '                'Se il calcolo del ConsumoEffettivo automatico presenta delle anomalie allora
    '                'viene forzato solo se non si tratta di giro contatore
    '                '***************************************************************
    '                If CStr(lngConsumoAppoggio) <> ConsumoEffettivo Then
    '                    If blnGiroContatore Then
    '                        ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                    Else
    '                        If girocontatore = False And IncongruenteForzato = False Then
    '                            ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                        End If
    '                    End If
    '                End If
    '                '*********************************************************************************************************************
    '                'Forzatura del consumo teorico e della lettura teorica se il calcolo automatico presenta delle
    '                'anomalie
    '                'LetturaTeorica= letturaPrecedente + consumo teorico
    '                'Calcolo Del Consumo teorico
    '                lngConsumoTeoricoAppoggio = CalcolaConsumoTeorico(lngGiorniDiConsumo, oDatiLettura.CodContatore, oDatiLettura.CodUtente, conn)
    '                lngLetturaTeoricaAppoggio = CalcolaLetturaTeorica(lngLetturaPrecedente, lngConsumoTeoricoAppoggio, DetailContatore)
    '                If CStr(lngConsumoTeoricoAppoggio) <> ConsumoTeorico Then
    '                    ConsumoTeorico = CStr(lngConsumoTeoricoAppoggio)
    '                End If
    '                If CStr(lngLetturaTeoricaAppoggio) <> letturateorica Then
    '                    letturateorica = CStr(lngLetturaTeoricaAppoggio)
    '                End If
    '                '*********************************************************************************************************************
    '            Else
    '                ConsumoEffettivo = 0
    '                GiorniConsumo = 0
    '                ConsumoTeorico = 0
    '                letturateorica = 0
    '            End If
    '        End If
    '        Try
    '            If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
    '                sSQL="INSERT INTO TP_LETTURE" & vbCrLf
    '                sSQL+="(CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA," & vbCrLf
    '                sSQL+="CODMODALITALETTURA,CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE," & vbCrLf
    '                sSQL+="IDSTATOLETTURA,INCONGRUENTE,FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE," & vbCrLf
    '                'modifica del 14/02/2007
    '                'sSQL+="STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO)" & vbCrLf
    '                sSQL+="STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)" & vbCrLf
    '                sSQL+="VALUES ( " & vbCrLf
    '                sSQL+= oDatiLettura.CodContatore & "," & vbCrLf
    '                sSQL+= oDatiLettura.CodPeriodo & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(oDatiLettura.DataLettura) & "," & vbCrLf
    '                If blnLETTURAVUOTA Then
    '                    sSQL+="Null" & "," & vbCrLf
    '                Else
    '                    sSQL+= utility.stringoperation.formatint(oDatiLettura.Lettura) & "," & vbCrLf
    '                End If
    '                sSQL+= utility.stringoperation.formatint(letturateorica) & "," & vbCrLf
    '                sSQL+="Null," & vbCrLf
    '                sSQL+= utility.stringoperation.formatint(ConsumoEffettivo) & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(oDatiLettura.sNote) & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(GiorniConsumo) & "," & vbCrLf
    '                sSQL+= utility.stringoperation.formatstring(ConsumoTeorico) & "," & vbCrLf
    '                sSQL+= oDatiLettura.CodUtente & "," & vbCrLf
    '                sSQL+="Null," & vbCrLf
    '                sSQL+="Null" & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica e quindi e da considerarsi come fatturata
    '                'sSQL+="Null" & "," & vbCrLf
    '                sSQL+= "0 , " & vbCrLf
    '                '**********************************************************
    '                sSQL+="0," & vbCrLf
    '                sSQL+="0," & vbCrLf
    '                If blnGiroContatore Then
    '                    girocontatore = True
    '                End If
    '                sSQL+= utility.stringoperation.formatbool(girocontatore) & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica Flag Storica Alzato
    '                sSQL+="1" & "," & vbCrLf
    '                '**********************************************************
    '                sSQL+= utility.stringoperation.formatstring(dataLetturaprecedente) & "," & vbCrLf
    '                'If lngLetturaPrecedente = -1 Then
    '                If lngLetturaPrecedente = -1 Then
    '                    sSQL+= 0 & "," & vbCrLf
    '                Else
    '                    sSQL+= lngLetturaPrecedente & "," & vbCrLf
    '                End If
    '                If PrimaLettura Then
    '                    sSQL+= utility.stringoperation.formatbool(PrimaLettura) & "," & vbCrLf
    '                Else
    '                    sSQL+="Null" & "," & vbCrLf
    '                End If
    '                '*** salvataggio codice anomalia
    '                If oDatiLettura.Cod_anomalia1 = -1 Then
    '                    sSQL+="Null,"
    '                Else
    '                    sSQL+= oDatiLettura.Cod_anomalia1 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia2 = -1 Then
    '                    sSQL+="Null,"
    '                Else
    '                    sSQL+= oDatiLettura.Cod_anomalia2 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia3 = -1 Then
    '                    sSQL+="Null,"
    '                Else
    '                    sSQL+= oDatiLettura.Cod_anomalia3 & ","
    '                End If
    '                sSQL+= utility.stringoperation.formatstring(oReplace.GiraData(oDatiLettura.DataDiPassaggio)) & "," & vbCrLf
    '                sSQL+="0,'Data Entry Massivo'" & vbCrLf
    '                sSQL+=" )" & vbCrLf
    '                sqlTrans = conn.BeginTransaction
    '                sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)

    '                sqlCmdInsert.ExecuteNonQuery()

    '                If blnLAsciatoAvviso Then
    '                    sSQL="UPDATE TP_CONTATORI SET LASCIATOAVVISO=1" & vbCrLf
    '                    sSQL+="WHERE CODCONTATORE=" & oDatiLettura.CodContatore & vbCrLf

    '                    sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)
    '                    sqlCmdInsert.ExecuteNonQuery()
    '                End If
    '            End If
    '            sqlTrans.Commit()
    '            Return setLettureAttuali

    '        Catch er As Exception
    '            sqlTrans.Rollback()
    '            Return False
    '            Throw
    '        Finally
    '            sqlTrans.Dispose()
    '        End Try
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuale.errore: ", Err)
    '        Dim MyErr As String = Err.Message
    '        MyErr = MyErr + "male, male, male!!!"
    '    End Try
    'End Function

    'Public Sub StartImportCMGC(ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdImport As Integer, ByVal sPeriodo As String, ByVal sOperatore As String, ByVal sISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer)
    '    Dim FncImport As New ObjImportazione
    '    Dim oMyTotAcq As New ObjImportazione
    '    Dim sFileScarti As String = ""
    '    Dim nRcToImport As Integer

    '    Try
    '        Dim nCheckFile As Integer
    '        Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim sErrCheckFile As String

    '        'controllo che il formato sia corretto
    '        nCheckFile = ControllaFileCMGC(sEnteImport, sFileImport, sNameImport, nRcToImport, sErrCheckFile)
    '        Select Case nCheckFile
    '            Case -1 'errore
    '                'sposto il file nella cartella non acquisiti
    '                sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                System.IO.File.Delete(sFileImport)
    '                'registro l'errore acquisizione
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sProvenienza = "TXT CMGC"
    '                FncImport.SetAcquisizione(oMyTotAcq, 1)
    '            Case 0 'formato non corretto
    '                'sposto il file nella cartella non acquisiti
    '                sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                System.IO.File.Delete(sFileImport)
    '                'registro il formato non corretto di acquisizione
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sProvenienza = "TXT CMGC"
    '                FncImport.SetAcquisizione(oMyTotAcq, 1)
    '            Case 1 'formato corretto
    '                If AcquisizioneCMGC(1, sEnteImport, sFileImport, sNameImport, nIdImport, oMyTotAcq, sPeriodo, sOperatore, sISTAT, sEnteAppartenenza, nMyIDImpianto) <= 0 Then
    '                    'sposto il file nella cartella non acquisiti
    '                    sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString())
    '                    System.IO.File.Delete(sFileImport)
    '                    'registro l'errore acquisizione
    '                    oMyTotAcq.Id = nIdImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = -1
    '                    oMyTotAcq.sEsito = "Errore durante l'importazione."
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sProvenienza = "TXT CMGC"
    '                    FncImport.SetAcquisizione(oMyTotAcq, 1)
    '                Else
    '                    'sposto il file nella cartella acquisiti
    '                    sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString())
    '                    System.IO.File.Delete(sFileImport)
    '                    'registro l'avvenuta acquisizione
    '                    oMyTotAcq.Id = nIdImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = 0
    '                    oMyTotAcq.sEsito = "Acquisizione terminata con successo!"
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sProvenienza = "TXT CMGC"
    '                    FncImport.SetAcquisizione(oMyTotAcq, 1)
    '                End If
    '                'End If
    '            Case 2 'dati obbligatori mancanti
    '                'sposto il file nella cartella non acquisiti
    '                sFileScarti = SpostaFile(sFileImport, ConfigurationManager.AppSettings("PATH_SPOSTA_NON_ACQUISITO").ToString())
    '                System.IO.File.Delete(sFileImport)
    '                'registro la mancanza di dati obbligatori
    '                oMyTotAcq.Id = nIdImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Dati obbligatori mancanti." + vbCrLf + sErrCheckFile
    '                oMyTotAcq.tDataAcq = Now
    '                FncImport.SetAcquisizione(oMyTotAcq, 1)
    '        End Select

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.StartImportCMGC.errore: ", Err)
    '        'registro l'errore acquisizione
    '        oMyTotAcq.IdEnte = sEnteImport
    '        oMyTotAcq.sFileAcq = sFileImport
    '        oMyTotAcq.nStatoAcq = -1
    '        oMyTotAcq.sEsito = "Errore durante l'importazione."
    '        FncImport.SetAcquisizione(oMyTotAcq, 1)
    '    End Try
    'End Sub

    'Private Function ControllaFileCMGC(ByVal sEnteCheck As String, ByVal sFileCheck As String, ByVal sNameFileCheck As String, ByRef x As Integer, ByRef sErrCheck As String) As Integer
    '    '{1= formato corretto; 0= formato non corretto; 2= dati obbligatori mancanti; -1= errore}
    '    Try
    '        Dim MyFileToRead As IO.StreamReader
    '        Dim sLine, sLineRead(79) As String

    '        'non è un file excel
    '        If Not sNameFileCheck.ToLower.EndsWith(".txt") Then
    '            sErrCheck = "Il file non è un TXT."
    '            Return 0
    '        End If
    '        'non è dell’ente in esame
    '        If Not sNameFileCheck.ToLower.StartsWith(sEnteCheck) Then
    '            sErrCheck = "L'ente del file non corrisponde con l'ente in esame."
    '            Return 0
    '        End If
    '        'controllo che la struttura sia corretta
    '        Try
    '            'apro il file
    '            MyFileToRead = IO.File.OpenText(sFileCheck)
    '            Do While MyFileToRead.Peek > 1
    '                x += 1
    '                'leggo la riga
    '                sLine = MyFileToRead.ReadLine
    '                sLineRead = sLine.Split(";")
    '            Loop
    '        Catch ex As Exception

    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.ControllaFileCMGC.errore: ", ex)
    '            sErrCheck = ex.Message
    '            Return -1
    '        Finally
    '            MyFileToRead.Close()
    '        End Try
    '        Return 1
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.ControllaFileCMGC.errore: ", Err)
    '        sErrCheck = Err.Message
    '        Return -1
    '    End Try
    'End Function

    'Private Function AcquisizioneCMGC(ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal sNameFileAcq As String, ByVal nIdFlussoAcq As Integer, ByRef oImport As ObjImportazione, ByVal sIdPeriodo As String, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer) As Integer
    '    Dim MyFileToRead As IO.StreamReader
    '    Dim sLine, sLineRead(79) As String
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim FncContratti As New GestContratti
    '    Dim FncContatori As New GestContatori
    '    Dim oNewContratto As objContratto
    '    Dim oNewContatore As objContatore
    '    Dim oNewLettura As New ObjImportazione.DatiLetture
    '    Dim nIntestatario, nUtente, nContratto As Integer
    '    Dim sqlConn As New SqlConnection

    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()
    '        oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameFileAcq
    '        'apro il file
    '        MyFileToRead = IO.File.OpenText(sFileAcq)
    '        Do While MyFileToRead.Peek > 1
    '            'conta il numero di record nel file excel
    '            oImport.nRcFile += 1
    '            'leggo la riga
    '            sLine = MyFileToRead.ReadLine
    '            sLineRead = sLine.Split(";")
    '            'controllo l'univocità del numero utente
    '            If sLineRead(0).Trim() <> "" Then
    '                If FncContratti.GetEsistente(sEnteAcq, sLineRead(0).Trim, -1) Then
    '                    oImport.nRcScarti += 1
    '                    'scrive il file degli scarti
    '                    If WriteFile(oImport.sFileScarti, "Numero Utente " & sLineRead(0).Trim & " già esistente. Il contratto non può essere salvato!") = 0 Then
    '                        Return -1
    '                    End If
    '                Else
    '                    'Non si puo inserire un codice contratto uguale ad uno precedentemente inserito
    '                    'se passa di qua, non verrà inserito un nuovo contratto in quanto il codice contratto di tipo stringa è già esistente
    '                    If sLineRead(70).Trim() = "" Or sLineRead(70).Trim() = "0000000" Or sLineRead(70).Trim() = "0" Then
    '                        nContratto += 1
    '                        sLineRead(70) = "OGU" & nContratto.ToString.PadLeft(5, "0")
    '                    End If
    '                    If FncContratti.ControllaCodice(sEnteAcq, sLineRead(70).Trim) <> "-1" Then
    '                        oImport.nRcScarti += 1
    '                        'scrive il file degli scarti
    '                        If WriteFile(oImport.sFileScarti, "Codice Contratto " & sLineRead(70).Trim & " già esistente. Il contratto non può essere salvato!") = 0 Then
    '                            Return -1
    '                        End If
    '                        'valorizzo i dati del contatore
    '                        oNewContatore = ValDatiContatore(sLineRead, nIntestatario, nUtente, -1, sEnteAcq, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto)
    '                        If oNewContatore.sMatricola = "Err" Then
    '                            oImport.nRcScarti += 1
    '                            'scrive il file degli scarti
    '                            If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                Return -1
    '                            End If
    '                        Else
    '                            'inserisco il contatore
    '                            If FncContatori.SetDatiContatore(0, oNewContatore, True) = False Then
    '                                oImport.nRcScarti += 1
    '                                'scrive il file degli scarti
    '                                If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                    Return -1
    '                                End If
    '                            Else
    '                                'inserisco i dati catastali
    '                                SetDatiCatastaliCMGC(oNewContatore, sqlConn)
    '                                'valorizzo i dati della lettura
    '                                oNewLettura = ValDatiLettura(sLineRead, nUtente, oNewContatore.nIdContatore, sIdPeriodo)
    '                                If oNewLettura.Provenienza = "Err" Then
    '                                    oImport.nRcScarti += 1
    '                                    'scrive il file degli scarti
    '                                    If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione della lettura per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                        Return -1
    '                                    End If
    '                                Else
    '                                    If SetLettureCMGC(oNewContatore, oNewLettura, sqlConn) = False Then
    '                                        oImport.nRcScarti += 1
    '                                        'scrive il file degli scarti
    '                                        If WriteFile(oImport.sFileScarti, "Errore nell'importazione della lettura per il Numero Utente " & sLineRead(0)) = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    Else
    '                                        oImport.nRcImport += 1
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    Else
    '                        'prelevo l'intestatario
    '                        nIntestatario = GetIdAnagrafico(sLineRead, sEnteAcq, "9000", 20)
    '                        If nIntestatario = -1 Then
    '                            oImport.nRcScarti += 1
    '                            'scrive il file degli scarti
    '                            If WriteFile(oImport.sFileScarti, "Intestario " & sLineRead(23).Trim & " " & sLineRead(24).Trim & " non trovato/inserito!") = 0 Then
    '                                Return -1
    '                            End If
    '                        Else
    '                            'prelevo l'utente
    '                            nUtente = GetIdAnagrafico(sLineRead, sEnteAcq, "9000", 0)
    '                            If nUtente = -1 Then
    '                                oImport.nRcScarti += 1
    '                                'scrive il file degli scarti
    '                                If WriteFile(oImport.sFileScarti, "Utente " & sLineRead(3).Trim & " " & sLineRead(4).Trim & " non trovato/inserito!") = 0 Then
    '                                    Return -1
    '                                End If
    '                            Else
    '                                'valorizzo i dati del contratto
    '                                oNewContratto = ValDatiContratto(sLineRead, sEnteAcq, nIntestatario, nUtente, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto)
    '                                If oNewContratto.sCodiceContratto = "Err" Then
    '                                    oImport.nRcScarti += 1
    '                                    'scrive il file degli scarti
    '                                    If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contratto per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                        Return -1
    '                                    End If
    '                                Else
    '                                    'inserisco il contratto
    '                                    FncContratti.SaveContratto(0, sIdPeriodo, oNewContratto, True)
    '                                    'valorizzo i dati del contatore
    '                                    oNewContatore = ValDatiContatore(sLineRead, nIntestatario, nUtente, oNewContratto.nIdContratto, sEnteAcq, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto)
    '                                    If oNewContatore.sMatricola = "Err" Then
    '                                        oImport.nRcScarti += 1
    '                                        'scrive il file degli scarti
    '                                        If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    Else
    '                                        'inserisco il contatore
    '                                        If FncContatori.SetDatiContatore(0, oNewContatore, True) = False Then
    '                                            oImport.nRcScarti += 1
    '                                            'scrive il file degli scarti
    '                                            If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                                Return -1
    '                                            End If
    '                                        Else
    '                                            'inserisco i dati catastali
    '                                            SetDatiCatastaliCMGC(oNewContatore, sqlConn)
    '                                            'valorizzo i dati della lettura
    '                                            oNewLettura = ValDatiLettura(sLineRead, nUtente, oNewContatore.nIdContatore, sIdPeriodo)
    '                                            If oNewLettura.Provenienza = "Err" Then
    '                                                oImport.nRcScarti += 1
    '                                                'scrive il file degli scarti
    '                                                If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione della lettura per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                                    Return -1
    '                                                End If
    '                                            Else
    '                                                If SetLettureCMGC(oNewContatore, oNewLettura, sqlConn) = False Then
    '                                                    oImport.nRcScarti += 1
    '                                                    'scrive il file degli scarti
    '                                                    If WriteFile(oImport.sFileScarti, "Errore nell'importazione della lettura per il Numero Utente " & sLineRead(0)) = 0 Then
    '                                                        Return -1
    '                                                    End If
    '                                                Else
    '                                                    oImport.nRcImport += 1
    '                                                End If
    '                                            End If
    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Loop
    '        Return 1
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AcquisizioneCMGC.errore: ", Err)
    '        Return -1
    '    Finally
    '        MyFileToRead.Close()
    '        sqlConn.Dispose()
    '    End Try
    'End Function

    'Private Function AcquisizioneCMGC(ByVal IsTMP As Integer, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal sNameFileAcq As String, ByVal nIdFlussoAcq As Integer, ByRef oImport As ObjImportazione, ByVal sIdPeriodo As String, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer) As Integer
    '    Dim MyFileToRead As IO.StreamReader
    '    Dim sLine, sLineRead(79) As String
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim FncContratti As New GestContratti
    '    Dim FncContatori As New GestContatori
    '    Dim oNewContratto As New objContratto
    '    Dim oNewContatore As objContatore
    '    Dim oNewLettura As New ObjImportazione.DatiLetture
    '    Dim nIntestatario, nUtente As Integer
    '    Dim sqlConn As New SqlConnection

    '    Try
    '        sqlConn.ConnectionString = ConstSession.StringConnection
    '        sqlConn.Open()
    '        oImport.sFileScarti = ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + "SCARTI_" + sNameFileAcq
    '        'apro il file
    '        MyFileToRead = IO.File.OpenText(sFileAcq)
    '        Do While MyFileToRead.Peek > 1
    '            'conta il numero di record nel file excel
    '            oImport.nRcFile += 1
    '            'leggo la riga
    '            sLine = MyFileToRead.ReadLine
    '            sLineRead = sLine.Split(";")
    '            'prelevo l'intestatario
    '            nIntestatario = GetIdAnagrafico(sLineRead, sEnteAcq, "9000", 20)
    '            If nIntestatario = -1 Then
    '                oImport.nRcScarti += 1
    '                'scrive il file degli scarti
    '                If WriteFile(oImport.sFileScarti, "Intestario " & sLineRead(23).Trim & " " & sLineRead(24).Trim & " non trovato/inserito!") = 0 Then
    '                    Return -1
    '                End If
    '            Else
    '                'prelevo l'utente
    '                nUtente = GetIdAnagrafico(sLineRead, sEnteAcq, "9000", 0)
    '                If nUtente = -1 Then
    '                    oImport.nRcScarti += 1
    '                    'scrive il file degli scarti
    '                    If WriteFile(oImport.sFileScarti, "Utente " & sLineRead(3).Trim & " " & sLineRead(4).Trim & " non trovato/inserito!") = 0 Then
    '                        Return -1
    '                    End If
    '                Else
    '                    'valorizzo i dati del contratto
    '                    oNewContratto = ValDatiContratto(sLineRead, sEnteAcq, nIntestatario, nUtente, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto)
    '                    If oNewContratto.sCodiceContratto = "Err" Then
    '                        oImport.nRcScarti += 1
    '                        'scrive il file degli scarti
    '                        If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contratto per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                            Return -1
    '                        End If
    '                    Else
    '                        'inserisco il contratto
    '                        FncContratti.SaveContratto(0, sIdPeriodo, oNewContratto, True)
    '                        'valorizzo i dati del contatore
    '                        oNewContatore = ValDatiContatore(sLineRead, nIntestatario, nUtente, oNewContratto.nIdContratto, sEnteAcq, sOperatore, sCodISTAT, sEnteAppartenenza, nMyIDImpianto)
    '                        If oNewContatore.sMatricola = "Err" Then
    '                            oImport.nRcScarti += 1
    '                            'scrive il file degli scarti
    '                            If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                Return -1
    '                            End If
    '                        Else
    '                            'inserisco il contatore
    '                            If FncContatori.SetDatiContatore(0, oNewContatore, True) = False Then
    '                                oImport.nRcScarti += 1
    '                                'scrive il file degli scarti
    '                                If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione del contatore per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                    Return -1
    '                                End If
    '                            Else
    '                                'inserisco i dati catastali
    '                                SetDatiCatastaliCMGC(oNewContatore, sqlConn)
    '                                'valorizzo i dati della lettura
    '                                oNewLettura = ValDatiLettura(sLineRead, nUtente, oNewContatore.nIdContatore, sIdPeriodo)
    '                                If oNewLettura.Provenienza = "Err" Then
    '                                    oImport.nRcScarti += 1
    '                                    'scrive il file degli scarti
    '                                    If WriteFile(oImport.sFileScarti, "Errore nella valorizzazione della lettura per il Numero Utente " & sLineRead(0).Trim) = 0 Then
    '                                        Return -1
    '                                    End If
    '                                Else
    '                                    If SetLettureCMGC(oNewContatore, oNewLettura, sqlConn) = False Then
    '                                        oImport.nRcScarti += 1
    '                                        'scrive il file degli scarti
    '                                        If WriteFile(oImport.sFileScarti, "Errore nell'importazione della lettura per il Numero Utente " & sLineRead(0)) = 0 Then
    '                                            Return -1
    '                                        End If
    '                                    Else
    '                                        oImport.nRcImport += 1
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Loop
    '        Return 1
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.AcquisizioneCMGC.errore: ", Err)
    '        Return -1
    '    Finally
    '        MyFileToRead.Close()
    '        sqlConn.Dispose()
    '    End Try
    'End Function

    Public Function GeneraLetPrecFromAtt(ByVal sIdPeriodo As String) As Integer
        Dim oReplace As New ClsGenerale.Generale
        Dim oNewLettura As New ObjImportazione.DatiLetture
        Dim oListLetture() As ObjImportazione.DatiLetture
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim sLastDataLettura, sLastPeriodo, sDataLettura As String
        Dim nLastLettura, nIDContatorePrec, x As Integer
        Dim bInsertForzatura As Boolean

        Try
            sLastPeriodo = "" : sLastDataLettura = "" : oListLetture = Nothing
            x = 0
            sSQL = "SELECT *"
            sSQL += " FROM TP_LETTURE"
            sSQL += " ORDER BY CODCONTATORE, DATALETTURA, DATALETTURAPRECEDENTE"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    'se sono sullo stesso contatore del precedente
                    If StringOperation.FormatInt(myRow("codcontatore")) <> nIDContatorePrec Then
                        bInsertForzatura = False
                        nIDContatorePrec = -1
                        sLastDataLettura = ""
                        nLastLettura = -1
                        sLastPeriodo = ""
                    End If
                    'se sono sulla lettura del periodo attuale devo generare una lettura con i dati dei campi lettura precedente
                    If StringOperation.FormatString(myRow("codperiodo")) = sIdPeriodo Then
                        If nIDContatorePrec <> -1 Then
                            'se la data lettura è uguale all'ultima presente la incremento di 1gg
                            If StringOperation.FormatString(myRow("dataletturaprecedente")) = sLastDataLettura Then
                                sDataLettura = DateAdd(DateInterval.Day, 1, CDate(oReplace.FormattaData("G", "/", sLastDataLettura, False)))
                            Else
                                sDataLettura = oReplace.FormattaData("G", "/", sLastDataLettura, False)
                            End If
                            'se la lettura è diversa dall'ultima presente la inserisco
                            If StringOperation.FormatInt(myRow("letturaprecedente")) <> nLastLettura And bInsertForzatura = False Then
                                oNewLettura = New ObjImportazione.DatiLetture
                                'valorizzo i dati della lettura
                                oNewLettura.CodContatore = StringOperation.FormatInt(myRow("codcontatore"))
                                oNewLettura.CodUtente = StringOperation.FormatInt(myRow("codutente"))
                                oNewLettura.Lettura = StringOperation.FormatInt(myRow("letturaprecedente"))
                                oNewLettura.DataLettura = oReplace.FormattaData("A", "", sDataLettura, False)
                                oNewLettura.LetturaPrecedente = 0
                                oNewLettura.DataLetturaPrecedente = ""
                                oNewLettura.ConsumoEffettivo = 0
                                If Not IsDBNull(dvMyDati("numeroutente")) Then
                                    oNewLettura.NumeroUtente = StringOperation.FormatString(myRow("numeroutente"))
                                End If
                                oNewLettura.Provenienza = "FORZA PER INCONGRUENZA CON CARICO PREC"
                                oNewLettura.CodPeriodo = sLastPeriodo
                                oNewLettura.Fatturazione = 1
                                oNewLettura.CodLettura = -1

                                oNewLettura.Storica = 0
                                oNewLettura.Cod_anomalia1 = -1
                                oNewLettura.Cod_anomalia2 = -1
                                oNewLettura.Cod_anomalia3 = -1

                                ReDim Preserve oListLetture(x)
                                oListLetture(x) = oNewLettura
                                bInsertForzatura = True
                                x += 1
                            End If
                        End If
                    End If
                    nIDContatorePrec = StringOperation.FormatInt(myRow("codcontatore"))
                    sLastDataLettura = StringOperation.FormatString(myRow("datalettura"))
                    If Not IsDBNull(dvMyDati("lettura")) Then
                        nLastLettura = StringOperation.FormatInt(myRow("lettura"))
                    Else
                        nLastLettura = 0
                    End If
                    sLastPeriodo = StringOperation.FormatString(myRow("codperiodo"))
                Next
            End If
            dvMyDati.Dispose()
            'ciclo sull'array per inserire le letture forzate
            For x = 0 To oListLetture.GetUpperBound(0)
                '*** salvataggio dati lettura ***
                If SetLetture(oListLetture(x)) = False Then
                    Return -1
                End If
            Next
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GeneraLetPrecFromAtt.errore: ", Err)
            Return -1
        End Try
    End Function

    Private Function GetIdAnagrafico(ByVal sDatiAnag() As String, ByVal sEnte As String, ByVal sTributo As String, ByVal x As Integer) As Integer
        Dim FncAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oMyAnagrafica As New DettaglioAnagrafica
        Dim oMyAnagraficaRet As New DettaglioAnagraficaReturn

        Try
            oMyAnagrafica.COD_CONTRIBUENTE = "-1"
            oMyAnagrafica.ID_DATA_ANAGRAFICA = "-1"
            oMyAnagrafica.CodEnte = sEnte
            oMyAnagrafica.CodiceFiscale = sDatiAnag(x + 1).Trim
            If sDatiAnag(x + 2).Trim <> "00000000000" And sDatiAnag(x + 2).Trim <> "0" Then
                oMyAnagrafica.PartitaIva = sDatiAnag(x + 2).Trim
            End If
            oMyAnagrafica.Cognome = sDatiAnag(x + 3).Trim
            oMyAnagrafica.Nome = sDatiAnag(x + 4).Trim
            oMyAnagrafica.CodViaResidenza = sDatiAnag(x + 5).Trim
            If oMyAnagrafica.CodViaResidenza = "-1" Or oMyAnagrafica.CodViaResidenza = "" Then
                oMyAnagrafica.CodViaResidenza = -1
                oMyAnagrafica.ViaResidenza = sDatiAnag(x + 6).Trim + " " + sDatiAnag(x + 7).Trim
            Else
                oMyAnagrafica.ViaResidenza = GetVia(oMyAnagrafica.CodViaResidenza, sEnte)
                If oMyAnagrafica.ViaResidenza = "" Then
                    oMyAnagrafica.CodViaResidenza = -1
                    oMyAnagrafica.ViaResidenza = sDatiAnag(x + 6).Trim + " " + sDatiAnag(x + 7).Trim
                End If
            End If
            If sDatiAnag(x + 8).Trim <> "00000" And sDatiAnag(x + 8).Trim <> "0" Then
                oMyAnagrafica.CivicoResidenza = sDatiAnag(x + 8).Trim
            Else
                oMyAnagrafica.CivicoResidenza = ""
            End If
            oMyAnagrafica.CapResidenza = sDatiAnag(x + 9).Trim
            oMyAnagrafica.CodiceComuneResidenza = "" 'CInt(sDatiAnag(x + 10).Trim)
            oMyAnagrafica.ComuneResidenza = sDatiAnag(x + 11).Trim
            oMyAnagrafica.ProvinciaResidenza = sDatiAnag(x + 12).Trim
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            'oMyAnagrafica.ID_DATA_SPEDIZIONE = "-1"
            'oMyAnagrafica.CodTributo = sTributo
            'oMyAnagrafica.CodViaRCP = sDatiAnag(x + 13).Trim
            'If oMyAnagrafica.CodViaRCP = "-1" Or oMyAnagrafica.CodViaRCP = "" Then
            '    oMyAnagrafica.CodViaRCP = -1
            '    oMyAnagrafica.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
            '    If oMyAnagrafica.ViaRCP.Trim <> "" Then
            '        oMyAnagrafica.CognomeInvio = oMyAnagrafica.Cognome
            '        oMyAnagrafica.NomeInvio = oMyAnagrafica.Nome
            '    End If
            'Else
            '    oMyAnagrafica.ViaRCP = GetVia(oMyAnagrafica.CodViaRCP, sEnte)
            '    If oMyAnagrafica.ViaRCP.Trim = "" Then
            '        oMyAnagrafica.CodViaRCP = -1
            '        oMyAnagrafica.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
            '    Else
            '        oMyAnagrafica.CognomeInvio = oMyAnagrafica.Cognome
            '        oMyAnagrafica.NomeInvio = oMyAnagrafica.Nome
            '    End If
            'End If
            'If sDatiAnag(x + 16).Trim <> "00000" And sDatiAnag(x + 16).Trim <> "0" Then
            '    oMyAnagrafica.CivicoRCP = sDatiAnag(x + 16).Trim
            'Else
            '    oMyAnagrafica.CivicoRCP = ""
            'End If
            'oMyAnagrafica.CapRCP = sDatiAnag(x + 17).Trim
            'oMyAnagrafica.CodComuneRCP = "" 'CInt(sDatiAnag(x + 18).Trim)
            'oMyAnagrafica.ComuneRCP = sDatiAnag(x + 19).Trim
            'oMyAnagrafica.ProvinciaRCP = sDatiAnag(x + 20).Trim
            Dim mySped As New ObjIndirizziSpedizione
            mySped.CodTributo = "9000"
            mySped.CodViaRCP = sDatiAnag(x + 13).Trim
            If mySped.CodViaRCP = "-1" Or mySped.CodViaRCP = "" Then
                mySped.CodViaRCP = -1
                mySped.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
                If mySped.ViaRCP.Trim <> "" Then
                    mySped.CognomeInvio = oMyAnagrafica.Cognome
                    mySped.NomeInvio = oMyAnagrafica.Nome
                End If
            Else
                mySped.ViaRCP = GetVia(mySped.CodViaRCP, sEnte)
                If mySped.ViaRCP.Trim = "" Then
                    mySped.CodViaRCP = -1
                    mySped.ViaRCP = sDatiAnag(x + 14).Trim + " " + sDatiAnag(x + 15).Trim
                Else
                    mySped.CognomeInvio = oMyAnagrafica.Cognome
                    mySped.NomeInvio = oMyAnagrafica.Nome
                End If
            End If
            If sDatiAnag(x + 16).Trim <> "00000" And sDatiAnag(x + 16).Trim <> "0" Then
                mySped.CivicoRCP = sDatiAnag(x + 16).Trim
            Else
                mySped.CivicoRCP = ""
            End If
            mySped.CapRCP = sDatiAnag(x + 17).Trim
            mySped.CodComuneRCP = "" 'CInt(sDatiAnag(x + 18).Trim)
            mySped.ComuneRCP = sDatiAnag(x + 19).Trim
            mySped.ProvinciaRCP = sDatiAnag(x + 20).Trim
            oMyAnagrafica.ListSpedizioni.Add(mySped)

            oMyAnagraficaRet = FncAnagrafica.GestisciAnagrafica(oMyAnagrafica, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, True, True) 'FncAnagrafica.GestisciAnagrafica(oMyAnagrafica, ConstSession.StringConnectionAnagrafica)
            '*** ***
            Return CInt(oMyAnagraficaRet.COD_CONTRIBUENTE)
        Catch Err As Exception
            Try
                Dim IDContribuente As Long = FncAnagrafica.SetAnagrafica(oMyAnagrafica, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)
                If IDContribuente <> -1 Then
                    oMyAnagraficaRet.COD_CONTRIBUENTE = IDContribuente.ToString
                End If
                Return CInt(oMyAnagraficaRet.COD_CONTRIBUENTE)

            Catch ex As Exception

                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdTipoAnagrafico.errore: ", Err)
                Return -1
            End Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdTipoAnagrafico.errore: ", Err)
            Return -1
        End Try
    End Function

    Private Function ValDatiContratto(ByVal sLineDati() As String, ByVal sEnte As String, ByVal nIdIntestatario As Integer, ByVal nIdUtente As Integer, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer) As objContratto
        Dim oMyContratto As New objContratto
        Dim oMyContatore As New objContatore
        Dim oMyDatiCatastali As New objDatiCatastali
        Dim oListDatiCatastali(0) As objDatiCatastali

        Try
            oMyContatore.sNumeroUtente = CStr(sLineDati(0).Trim())

            If sLineDati(70).Trim() = "" Or sLineDati(70).Trim() = "0000000" Or sLineDati(70).Trim() = "0" Then
                sLineDati(70) = "OGU" & oMyContatore.sNumeroUtente.PadLeft(10, "0")
            End If
            oMyContratto.sCodiceContratto = sLineDati(70).Trim

            oMyContratto.nIdIntestatario = nIdIntestatario
            oMyContratto.nIdUtente = nIdUtente
            oMyContatore.nIdIntestatario = nIdIntestatario
            oMyContatore.nIdUtente = nIdUtente
            If sLineDati(42).Trim <> "" Then
                oMyContatore.nIdVia = sLineDati(42).Trim
            Else
                oMyContatore.nIdVia = -1
            End If
            If oMyContatore.nIdVia <= 0 Then
                oMyContatore.nIdVia = -1
                oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
            Else
                oMyContatore.sUbicazione = GetVia(oMyContatore.nIdVia, sEnte)
                If oMyContatore.sUbicazione = "" Then
                    oMyContatore.nIdVia = -1
                    oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
                End If
            End If

            If sLineDati(45).Trim <> "0" And sLineDati(45).Trim <> "-1" And sLineDati(45).Trim() <> "00000" And sLineDati(45).Trim <> "" Then
                oMyContatore.sCivico = CInt(sLineDati(45).Trim)
            Else
                oMyContatore.sCivico = ""
            End If

            oMyContatore.sEsponenteCivico = sLineDati(46).Trim

            'se la data sottoscrizione contratto non è valorizzata prende la data attivazione contatore
            If sLineDati(69).Trim() <> "00000000" And sLineDati(69).Trim() <> "0" Then
                If sLineDati(69).Trim.IndexOf("/") > 0 Then
                    oMyContratto.sDataSottoscrizione = oReplace.FormattaData("A", "", sLineDati(69).Trim, False)
                Else
                    oMyContratto.sDataSottoscrizione = oReplace.GiraDataFromDB(sLineDati(69).Trim)
                End If
            Else
                If sLineDati(67).Trim() <> "00000000" And sLineDati(67).Trim() <> "0" Then
                    If sLineDati(67).Trim.IndexOf("/") > 0 Then
                        oMyContratto.sDataSottoscrizione = oReplace.FormattaData("A", "", sLineDati(67).Trim, False)
                    Else
                        oMyContratto.sDataSottoscrizione = oReplace.GiraDataFromDB(sLineDati(67).Trim)
                    End If
                Else
                    oMyContratto.sDataSottoscrizione = ""
                End If
            End If

            If sLineDati.Length > 76 Then
                If IsNumeric(sLineDati(76).Trim) Then
                    oMyContatore.nProprietario = sLineDati(76).Trim
                End If
            End If
            oMyDatiCatastali.sInterno = sLineDati(47).Trim
            oMyDatiCatastali.sFoglio = sLineDati(48).Trim
            oMyDatiCatastali.sNumero = sLineDati(49).Trim
            If sLineDati(50).Trim <> "" Then
                oMyDatiCatastali.nSubalterno = sLineDati(50).Trim
            Else
                oMyDatiCatastali.nSubalterno = 0
            End If
            oListDatiCatastali(0) = oMyDatiCatastali
            oMyContatore.nGiro = GetIdGiro(sLineDati(52).Trim, sEnte)

            If sLineDati(53).Trim() <> "0000000" And sLineDati(53).Trim() <> "0" Then
                oMyContatore.sSequenza = sLineDati(53).Trim
            Else
                oMyContatore.sSequenza = ""
            End If

            oMyContatore.nPosizione = GetIdPosizioneContatore(sLineDati(54).Trim)

            If sLineDati(55).Trim() <> "0000" And sLineDati(55).Trim() <> "0" Then
                oMyContatore.sProgressivo = sLineDati(55).Trim
            Else
                oMyContatore.sProgressivo = ""
            End If

            oMyContatore.sLatoStrada = sLineDati(56).Trim
            If sLineDati(59).Trim = "S" Or sLineDati(59).Trim = "SI" Then
                oMyContatore.nCodFognatura = 2
                oMyContatore.bEsenteFognatura = 0
            Else
                oMyContatore.nCodFognatura = -1
                oMyContatore.bEsenteFognatura = 2
            End If
            If sLineDati(60).Trim = "S" Or sLineDati(60).Trim = "SI" Then
                oMyContatore.nCodDepurazione = 1
                oMyContatore.bEsenteDepurazione = 0
            Else
                oMyContatore.nCodDepurazione = -1
                oMyContatore.bEsenteDepurazione = 1
            End If
            If sLineDati(67).Trim <> "00000000" And sLineDati(67).Trim <> "0" Then
                If sLineDati(67).Trim.IndexOf("/") > 0 Then
                    oMyContatore.sDataAttivazione = oReplace.FormattaData("A", "", sLineDati(67).Trim, False)
                Else
                    oMyContatore.sDataAttivazione = oReplace.GiraDataFromDB(sLineDati(67).Trim)
                End If
            Else
                oMyContatore.sDataAttivazione = ""
            End If
            If oMyContatore.sDataAttivazione <> "" Then
                oMyContatore.bIsPendente = 0
            End If
            If sLineDati(68).Trim <> "00000000" And sLineDati(68).Trim <> "0" Then
                If sLineDati(68).Trim.IndexOf("/") > 0 Then
                    oMyContratto.sDataCessazione = oReplace.FormattaData("A", "", sLineDati(68).Trim, False)
                Else
                    oMyContratto.sDataCessazione = oReplace.GiraDataFromDB(sLineDati(68).Trim)
                End If
            Else
                oMyContratto.sDataCessazione = ""
            End If
            oMyContatore.nTipoUtenza = GetIdTipoUtenza(sLineDati(63).Trim, sEnte)

            If sLineDati(61).Trim <> "000" And sLineDati(61).Trim <> "0" Then
                oMyContatore.nDiametroContatore = GetIdDiametroContatore(sLineDati(61).Trim, sEnte)
            Else
                oMyContatore.nDiametroContatore = -1
            End If

            If sLineDati(62).Trim <> "000" And sLineDati(62).Trim <> "0" Then
                oMyContatore.nDiametroPresa = GetIdDiametroPresa(sLineDati(62).Trim)
            Else
                oMyContatore.nDiametroPresa = -1
            End If

            If sLineDati(66).Trim <> "" Then
                oMyContatore.nNumeroUtenze = sLineDati(66).Trim
            End If
            oMyContatore.sCodiceFabbricante = sLineDati(65).Trim
            oMyContatore.sCifreContatore = sLineDati(58).Trim
            oMyContatore.sCodiceISTAT = sCodISTAT
            oMyContatore.nIdAttivita = GetIdAttivita(sLineDati(64).Trim)
            oMyContratto.sIdEnte = sEnte
            oMyContatore.sIdEnteAppartenenza = sEnteAppartenenza
            oMyContatore.sStatoContatore = "ATT"
            '***DA CAMBIARE TUTTE LE VOLTE***
            oMyContatore.nIdImpianto = nMyIDImpianto

            'oMyContratto.sCodiceContratto = 0
            'oMyContatore.nTipoContatore = -1
            'oMyContatore.bEsenteAcqua = 0
            'oMyContatore.bIgnoraMora = 0
            'oMyContatore.bUtenteSospeso = 0
            'oMyContatore.sDataSospensioneUtenza = ""
            'oMyContratto.sNote = ""
            'oMyContatore.nCodIva = -1
            'oMyContatore.sPenalita = ""

            oMyContatore.oDatiCatastali = oListDatiCatastali
            oMyContratto.oContatore = oMyContatore
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.ValDatiContratto.errore: ", Err)
            oMyContratto.sCodiceContratto = "Err"
        End Try
        Return oMyContratto
    End Function

    Private Function ValDatiContatore(ByVal sLineDati() As String, ByVal nIdIntestatario As Integer, ByVal nIdUtente As Integer, ByVal nIdContratto As Integer, ByVal sEnte As String, ByVal sOperatore As String, ByVal sCodISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer) As objContatore
        Dim oMyContatore As New objContatore
        Dim oMyDatiCatastali As New objDatiCatastali
        Dim oListDatiCatastali(0) As objDatiCatastali

        Try
            oMyContatore.nIdIntestatario = nIdIntestatario
            oMyContatore.nIdUtente = nIdUtente
            oMyContatore.nIdContratto = nIdContratto
            If sLineDati(42).Trim <> "" Then
                oMyContatore.nIdVia = CInt(sLineDati(42).Trim)
            Else
                oMyContatore.nIdVia = -1
            End If
            If oMyContatore.nIdVia <= 0 Then
                oMyContatore.nIdVia = -1
                oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
            Else
                oMyContatore.sUbicazione = GetVia(oMyContatore.nIdVia, sEnte)
                If oMyContatore.sUbicazione = "" Then
                    oMyContatore.nIdVia = -1
                    oMyContatore.sUbicazione = sLineDati(43).Trim + " " + sLineDati(44).Trim
                End If
            End If

            If sLineDati(45).Trim <> "0" And sLineDati(45).Trim <> "-1" And sLineDati(45).Trim() <> "00000" And sLineDati(45).Trim <> "" Then
                oMyContatore.sCivico = CInt(sLineDati(45).Trim())
            Else
                oMyContatore.sCivico = ""
            End If

            'oMyContatore.sEsponente = sLineDati(46).Trim
            'oMyContatore.sInterno = sLineDati(47).Trim
            'oMyContatore.sFoglio = sLineDati(48).Trim
            'oMyContatore.sNumero = sLineDati(49).Trim
            'If sLineDati(50).Trim <> "" Then
            '    oMyContatore.nsubalterno = sLineDati(50).Trim
            'Else
            '    oMyContatore.nsubalterno = 0
            'End If

            'Dati agenzia entrate per i dati catastali
            oMyContatore.sSezioneCatast = sLineDati(51).Trim
            oMyContatore.sParticellaCatast = "E"
            oMyContatore.sEstensioneParticellaCatast = ""
            '/Dati agenzia entrate per i dati catastali

            oMyContatore.sMatricola = sLineDati(41).Trim
            oMyContatore.sIdEnte = sEnte
            oMyContatore.sIdEnteAppartenenza = sEnteAppartenenza

            If sLineDati(59).Trim = "S" Or sLineDati(59).Trim = "SI" Then
                oMyContatore.nCodFognatura = 2
                oMyContatore.bEsenteFognatura = 0
            Else
                oMyContatore.nCodFognatura = -1
                oMyContatore.bEsenteFognatura = 1
            End If
            If sLineDati(60).Trim = "S" Or sLineDati(60).Trim = "SI" Then
                oMyContatore.nCodDepurazione = 1
                oMyContatore.bEsenteDepurazione = 0
            Else
                oMyContatore.nCodDepurazione = -1
                oMyContatore.bEsenteDepurazione = 1
            End If

            oMyContatore.nGiro = GetIdGiro(sLineDati(52).Trim, sEnte)

            If sLineDati(53).Trim <> "0000000" And sLineDati(53).Trim <> "0" Then
                oMyContatore.sSequenza = sLineDati(53).Trim
            Else
                oMyContatore.sSequenza = ""
            End If

            oMyContatore.nPosizione = GetIdPosizioneContatore(sLineDati(54).Trim)

            If sLineDati(55).Trim <> "0000" And sLineDati(55).Trim <> "0" Then
                oMyContatore.sProgressivo = sLineDati(55).Trim
            Else
                oMyContatore.sProgressivo = ""
            End If

            oMyContatore.sLatoStrada = sLineDati(56).Trim

            oMyContatore.sNumeroUtente = CStr(sLineDati(0).Trim())
            If oMyContatore.sMatricola = "" Or oMyContatore.sMatricola = "0" Then
                oMyContatore.sMatricola = "OGU" & oMyContatore.sNumeroUtente.PadLeft(5, "0")
            End If

            If sLineDati(67).Trim <> "00000000" And sLineDati(67).Trim <> "0" Then
                If sLineDati(67).Trim.IndexOf("/") > 0 Then
                    oMyContatore.sDataAttivazione = sLineDati(67).Trim
                Else
                    oMyContatore.sDataAttivazione = oReplace.GiraDataFromDB(sLineDati(67).Trim)
                End If
            Else
                oMyContatore.sDataAttivazione = ""
            End If
            If oMyContatore.sDataAttivazione <> "" Then
                oMyContatore.bIsPendente = 0
            End If
            If sLineDati(68).Trim <> "00000000" And sLineDati(68).Trim <> "0" Then
                If sLineDati(68).Trim.IndexOf("/") > 0 Then
                    oMyContatore.sDataCessazione = sLineDati(68).Trim
                Else
                    oMyContatore.sDataCessazione = oReplace.GiraDataFromDB(sLineDati(68).Trim)
                End If
            Else
                oMyContatore.sDataCessazione = ""
            End If
            If sLineDati(66).Trim <> "" Then
                oMyContatore.nNumeroUtenze = sLineDati(66).Trim
            Else
                oMyContatore.nNumeroUtenze = 0
            End If
            oMyContatore.nTipoUtenza = GetIdTipoUtenza(sLineDati(63).Trim, sEnte)

            If sLineDati(61).Trim <> "000" And sLineDati(61).Trim <> "0" Then
                oMyContatore.nDiametroContatore = GetIdDiametroContatore(sLineDati(61).Trim, sEnte)
            Else
                oMyContatore.nDiametroContatore = -1
            End If

            If sLineDati(62).Trim <> "000" And sLineDati(62).Trim <> "0" Then
                oMyContatore.nDiametroPresa = GetIdDiametroPresa(sLineDati(62).Trim)
            Else
                oMyContatore.nDiametroPresa = -1
            End If

            oMyContatore.sCodiceFabbricante = sLineDati(65).Trim
            oMyContatore.sCifreContatore = sLineDati(58).Trim
            oMyContatore.nIdAttivita = GetIdAttivita(sLineDati(64).Trim)
            oMyContatore.sCodiceISTAT = sCodISTAT
            oMyContatore.sStatoContatore = "ATT"

            If sLineDati(57).Trim <> "" Then
                oMyContatore.nTipoContatore = GetIdTipoContatore(sLineDati(57).Trim)
            Else
                oMyContatore.nTipoContatore = -1
            End If

            oMyDatiCatastali.sInterno = sLineDati(47).Trim
            oMyDatiCatastali.sFoglio = sLineDati(48).Trim
            oMyDatiCatastali.sNumero = sLineDati(49).Trim
            If sLineDati(50).Trim <> "" Then
                oMyDatiCatastali.nSubalterno = sLineDati(50).Trim
            Else
                oMyDatiCatastali.nSubalterno = 0
            End If
            oListDatiCatastali(0) = oMyDatiCatastali

            '***DA CAMBIARE TUTTE LE VOLTE IN BASE AL COMUNE CHE SI DEVE IMPORTARE ***
            oMyContatore.nIdImpianto = nMyIDImpianto

            oMyContatore.tDataInserimento = Now
            oMyContatore.oDatiCatastali = oListDatiCatastali
            'oMyContatore.nIDSubAssociato = 0
            ''oMyContatore.sPiano = ""
            'oMyContatore.nSpesa = 0
            'oMyContatore.nDiritti = 0
            'oMyContatore.nCodIva = -1
            'oMyContatore.nContatorePrec = 0
            'oMyContatore.nContatoreSucc = 0
            'oMyContatore.bEsenteAcqua = 0
            'oMyContatore.bIgnoraMora = 0
            'oMyContatore.sNote = ""
            'oMyContatore.sDataSostituzione = ""
            'oMyContatore.sDataRimTemp = ""
            'oMyContatore.nConsumoMinimo = 0
            'oMyContatore.nIdContatore = 0
            'oMyContatore.sDataSospensioneUtenza = ""
            'oMyContatore.bUtenteSospeso = 0
            'oMyContatore.sQuoteAgevolate = ""
            'oMyContatore.sPenalita = ""
            'oMyContatore.nIdMinimo = 0
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.ValDatiContatore.errore: ", Err)
            oMyContatore.sMatricola = "Err"
        End Try
        Return oMyContatore
    End Function

    Private Function ValDatiLettura(ByVal sLineDati() As String, ByVal nIdUtente As Integer, ByVal nIdContatore As Integer, ByVal sIdPeriodo As String) As ObjImportazione.DatiLetture
        Dim oMyLettura As New ObjImportazione.DatiLetture
        Try
            oMyLettura.CodContatore = nIdContatore
            oMyLettura.CodUtente = nIdUtente
            oMyLettura.Lettura = sLineDati(74).Trim
            If sLineDati(73).Trim <> "00000000" And sLineDati(73).Trim <> "0" Then
                If sLineDati(73).Trim.IndexOf("/") > 0 Then
                    oMyLettura.DataLettura = oReplace.FormattaData("A", "", sLineDati(73).Trim, False)
                Else
                    oMyLettura.DataLettura = sLineDati(73).Trim
                End If
            Else
                oMyLettura.DataLettura = ""
            End If
            If sLineDati(72).Trim <> "" Then
                oMyLettura.LetturaPrecedente = sLineDati(72).Trim
            End If
            If sLineDati(71).Trim <> "00000000" And sLineDati(71).Trim <> "0" And sLineDati(71).Trim <> "" Then
                If sLineDati(71).Trim.IndexOf("/") > 0 Then
                    oMyLettura.DataLetturaPrecedente = oReplace.FormattaData("A", "", sLineDati(71).Trim, False)
                Else
                    oMyLettura.DataLetturaPrecedente = sLineDati(71).Trim
                End If
            Else
                oMyLettura.DataLetturaPrecedente = ""
            End If
            oMyLettura.ConsumoEffettivo = sLineDati(75).Trim
            oMyLettura.NumeroUtente = sLineDati(0)
            oMyLettura.Provenienza = "TXT CMGC"
            oMyLettura.CodPeriodo = sIdPeriodo
            oMyLettura.CodLettura = -1

            oMyLettura.Storica = 0
            oMyLettura.Cod_anomalia1 = -1
            oMyLettura.Cod_anomalia2 = -1
            oMyLettura.Cod_anomalia3 = -1
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.ValDatiLettura.errore: ", Err)
            oMyLettura.Provenienza = "Err"
        End Try
        Return oMyLettura
    End Function

    Private Function GetVia(ByVal nIdVia As Integer, ByVal sEnte As String) As String
        Try
            Dim oMyStrada As New Anater.Oggetti.RicercaStradario
            Dim DrStrade As OleDb.OleDbDataReader
            Dim typeofRI As Type = GetType(RemotingInterfaceAnater.IRemotingInterfaceICI)
            Dim remObject As RemotingInterfaceAnater.IRemotingInterfaceICI = Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings("URLanaterICI").ToString())
            Dim sIdVia As String = ""

            oMyStrada.CodEnte = sEnte
            oMyStrada.CodStrada = nIdVia
            oMyStrada.DescrizioneStrada = ""
            oMyStrada.TipoStrada = ""

            DrStrade = remObject.GetStradarioANATER(oMyStrada)
            If DrStrade.Read Then
                sIdVia = CStr(DrStrade("VieSpecieVie")) + " " + CStr(DrStrade("VieDescrizioneVia"))
            End If
            DrStrade.Close()

            Return sIdVia
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetVia.errore: ", Err)
            Return ""
        End Try
    End Function

    Private Function GetIdGiro(ByVal sGiro As String, ByVal sEnte As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Dim a As Integer = 0

        Try
            If sGiro = "FRAZ. CHANTE'" Then
                a = a + 1
            End If
            sSQL = "SELECT TP_GIRI.IDGIRO AS MYID"
            sSQL += " FROM TP_GIRI"
            sSQL += " WHERE (TP_GIRI.CODENTE='" & sEnte & "') AND (TP_GIRI.DESCRIZIONE='" & sGiro.Replace("'", "''") & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_GIRI(DESCRIZIONE,CODENTE)"
                sSQL += " VALUES ('" & sGiro.Replace("'", "''") & "','" & sEnte & "')"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                        sSQL = "UPDATE TP_GIRI SET TP_GIRI.COD_GIRO_EST=" & myIdentity
                        sSQL += " WHERE (TP_GIRI.IDGIRO=" & myIdentity & ")"
                        iDB.ExecuteNonQuery(sSQL)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdGiro.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Function GetIdPosizioneContatore(ByVal sPosizione As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            sSQL = "SELECT TP_POSIZIONECONTATORE.CODPOSIZIONE AS MYID"
            sSQL += " FROM TP_POSIZIONECONTATORE"
            sSQL += " WHERE (TP_POSIZIONECONTATORE.DESCRIZIONE='" & sPosizione & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_POSIZIONECONTATORE(DESCRIZIONE)"
                sSQL += " VALUES ('" & sPosizione & "')"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                        sSQL = "UPDATE TP_POSIZIONECONTATORE SET TP_POSIZIONECONTATORE.POSIZIONE=" & myIdentity
                        sSQL += " WHERE (TP_POSIZIONECONTATORE.CODPOSIZIONE=" & myIdentity & ")"
                        iDB.ExecuteNonQuery(sSQL)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdPosizioneContatore.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Function GetIdTipoUtenza(ByVal sUtenza As String, ByVal sEnte As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            sSQL = "SELECT TP_TIPIUTENZA.IDTIPOUTENZA AS MYID"
            sSQL += " FROM TP_TIPIUTENZA"
            sSQL += " WHERE (TP_TIPIUTENZA.COD_ENTE='" & sEnte & "') AND (TP_TIPIUTENZA.DESCRIZIONE='" & sUtenza & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_TIPIUTENZA(DESCRIZIONE,COD_ENTE)"
                sSQL += " VALUES ('" & sUtenza & "','" & sEnte & "')"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdTipoUtenza.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Function GetIdDiametroContatore(ByVal sDiametro As String, ByVal sEnte As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            sSQL = "SELECT TP_DIAMETROCONTATORE.CODDIAMETROCONTATORE AS MYID"
            sSQL += " FROM TP_DIAMETROCONTATORE"
            sSQL += " WHERE (TP_DIAMETROCONTATORE.CODICE_ISTAT ='00" & sEnte & "') AND (TP_DIAMETROCONTATORE.DESCRIZIONE='" & sDiametro & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_DIAMETROCONTATORE(DESCRIZIONE,DIAMETROCONTATORE,CODICE_ISTAT,PREVALENTE)"
                sSQL += " VALUES ('" & sDiametro & "','" & sDiametro & "','00" & sEnte & "',0)"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdDiametroContatore.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Function GetIdTipoContatore(ByVal sTipoContatore As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            sSQL = "SELECT IDTIPOCONTATORE AS MYID "
            sSQL += " FROM TP_TIPOCONTATORE "
            sSQL += "WHERE (DESCRIZIONE = '" & sTipoContatore & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_TIPOCONTATORE (DESCRIZIONE)"
                sSQL += " VALUES('" & sTipoContatore & "')"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdTipoContatore.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Function GetIdDiametroPresa(ByVal sPresa As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            sSQL = "SELECT TP_DIAMETROPRESA.CODDIAMETROPRESA AS MYID"
            sSQL += " FROM TP_DIAMETROPRESA"
            sSQL += " WHERE (TP_DIAMETROPRESA.DESCRIZIONE='" & sPresa & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_DIAMETROPRESA(DESCRIZIONE,DIAMETROPRESA)"
                sSQL += " VALUES ('" & sPresa & "','" & sPresa & "')"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdDiametroPresa.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Function GetIdAttivita(ByVal sAttivita As String) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            sSQL = "SELECT TP_TIPOATTIVITA.IDTIPOATTIVITA AS MYID"
            sSQL += " FROM TP_TIPOATTIVITA"
            sSQL += " WHERE (TP_TIPOATTIVITA.DESCRIZIONE='" & sAttivita & "')"
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("myid"))
                Next
            Else
                dvMyDati.Dispose()
                sSQL = "INSERT INTO TP_TIPOATTIVITA(DESCRIZIONE)"
                sSQL += " VALUES ('" & sAttivita & "')"
                sSQL += " SELECT @@IDENTITY"
                'eseguo la query
                dvMyDati = New DataView
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        myIdentity = myRow(0)
                    Next
                End If
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.GetIdAttivita.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    'Private Function SetLettureCMGC(ByVal DetailContatore As objContatore, ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal sqlMyConn As SqlConnection) As Boolean
    '    '=================================================================
    '    'LETTURE NORMALI
    '    '=================================================================
    '    Try
    '        Dim sqlMyTrans As SqlTransaction
    '        Dim strValoreFondoScala As String
    '        Dim lngConsumoAppoggio, lngValoreFondoScala, lngCifreContatore, lngCount, lngGiorniDiConsumo, lngConsumoTeoricoAppoggio, lngLetturaTeoricaAppoggio As Long
    '        Dim ModDate As New ClsGenerale.Generale
    '        Dim blnConsumoNegativo As Boolean = False
    '        Dim IncongruenteForzato As Boolean = False

    '        SetLettureCMGC = True

    '        If oDatiLettura.LetturaPrecedente <> 0 Then
    '            oDatiLettura.PrimaLettura = 0
    '            If oDatiLettura.Lettura <> 0 Then
    '                lngConsumoAppoggio = oDatiLettura.Lettura - oDatiLettura.LetturaPrecedente
    '            Else
    '                lngConsumoAppoggio = 0
    '            End If
    '            '=================================================
    '            'se blnConsumoNegativo e true considera il consumo negativo
    '            If Not blnConsumoNegativo Then
    '                If lngConsumoAppoggio < 0 Then
    '                    'Verifico e considero  il Giro Contatore
    '                    lngCifreContatore = utility.stringoperation.formatint(DetailContatore.sCifreContatore)
    '                    If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                        '*************************************************************
    '                        'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                        'Dall'Utente e flaggare il flag Giro Contatore
    '                        '*************************************************************
    '                        For lngCount = 1 To lngCifreContatore
    '                            strValoreFondoScala = strValoreFondoScala & "9"
    '                        Next
    '                        lngValoreFondoScala = utility.stringoperation.formatint(strValoreFondoScala)
    '                        lngConsumoAppoggio = lngValoreFondoScala - oDatiLettura.LetturaPrecedente
    '                        lngConsumoAppoggio = (utility.stringoperation.formatint(oDatiLettura.Lettura) - 0) + lngConsumoAppoggio
    '                        oDatiLettura.GiroContatore = 1
    '                    End If
    '                End If
    '            End If

    '            If IsDate(oReplace.GiraDataFromDB(oDatiLettura.DataLetturaPrecedente)) And IsDate(oReplace.GiraDataFromDB(oDatiLettura.DataLettura)) Then
    '                lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(oReplace.GiraDataFromDB(oDatiLettura.DataLetturaPrecedente)), CDate(oReplace.GiraDataFromDB(oDatiLettura.DataLettura)))
    '            Else
    '                lngGiorniDiConsumo = 0
    '            End If
    '            '***************************************************************
    '            'Se il calcolo dei giorni di consumo automatico presenta delle anomalie allora
    '            'viene forzato
    '            '***************************************************************
    '            'If lngGiorniDiConsumo <> oDatiLettura.GiornidiConsumo Then
    '            oDatiLettura.GiornidiConsumo = lngGiorniDiConsumo
    '            'End If
    '            '***************************************************************
    '            'Se il calcolo del ConsumoEffettivo automatico presenta delle anomalie allora
    '            'viene forzato solo se non si tratta di giro contatore
    '            '***************************************************************
    '            If lngConsumoAppoggio <> oDatiLettura.ConsumoEffettivo Then
    '                If oDatiLettura.GiroContatore = 1 Then
    '                    oDatiLettura.ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                Else
    '                    If oDatiLettura.GiroContatore = 1 And IncongruenteForzato = False Then
    '                        oDatiLettura.ConsumoEffettivo = lngConsumoAppoggio
    '                    End If
    '                End If
    '            End If
    '            '*********************************************************************************************************************
    '            'Forzatura del consumo teorico e della lettura teorica se il calcolo automatico presenta delle
    '            'anomalie
    '            'LetturaTeorica= letturaPrecedente + consumo teorico
    '            'Calcolo Del Consumo teorico
    '            lngConsumoTeoricoAppoggio = CalcolaConsumoTeorico(lngGiorniDiConsumo, oDatiLettura.CodContatore, oDatiLettura.CodUtente, sqlMyConn)
    '            lngLetturaTeoricaAppoggio = CalcolaLetturaTeorica(oDatiLettura.LetturaPrecedente, lngConsumoTeoricoAppoggio, DetailContatore)
    '            If lngConsumoTeoricoAppoggio <> oDatiLettura.ConsumoTeorico Then
    '                oDatiLettura.ConsumoTeorico = lngConsumoTeoricoAppoggio
    '            Else
    '                oDatiLettura.ConsumoTeorico = 0
    '            End If
    '            If lngLetturaTeoricaAppoggio <> oDatiLettura.LetturaTeorica Then
    '                oDatiLettura.LetturaTeorica = lngLetturaTeoricaAppoggio
    '            Else
    '                oDatiLettura.LetturaTeorica = 0
    '            End If
    '            '*********************************************************************************************************************
    '        Else
    '            'oDatiLettura.ConsumoEffettivo = 0
    '            oDatiLettura.GiornidiConsumo = 0
    '            oDatiLettura.ConsumoTeorico = 0
    '            oDatiLettura.LetturaTeorica = 0
    '        End If

    '        Try
    '            sqlMyTrans = sqlMyConn.BeginTransaction
    '            'inserisco la lettura del file
    '            If InsertLettureCMGC(oDatiLettura, sqlMyConn, sqlMyTrans) = -1 Then
    '                sqlMyTrans.Rollback()
    '                Return False
    '            End If
    '            'creo la lettura precedente e la inserisco
    '            If oDatiLettura.DataLetturaPrecedente <> "" Then
    '                oDatiLettura.DataLettura = oDatiLettura.DataLetturaPrecedente
    '                oDatiLettura.Lettura = oDatiLettura.LetturaPrecedente
    '                oDatiLettura.PrimaLettura = 1
    '                oDatiLettura.DataLetturaPrecedente = ""
    '                oDatiLettura.LetturaPrecedente = -1
    '                oDatiLettura.GiornidiConsumo = 0
    '                oDatiLettura.Consumo = 0
    '                oDatiLettura.ConsumoEffettivo = 0
    '                oDatiLettura.ConsumoTeorico = 0
    '                oDatiLettura.LetturaTeorica = 0
    '                oDatiLettura.GiroContatore = 0
    '                If InsertLettureCMGC(oDatiLettura, sqlMyConn, sqlMyTrans) = -1 Then
    '                    sqlMyTrans.Rollback()
    '                    Return False
    '                End If
    '            End If
    '            sqlMyTrans.Commit()
    '            Return SetLettureCMGC

    '        Catch er As Exception
    '            sqlMyTrans.Rollback()
    '            Return False
    '            Throw
    '        Finally
    '            sqlMyTrans.Dispose()
    '        End Try
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.SetLettureCMGC.errore: ", Err)
    '    End Try
    'End Function

    'Private Function InsertLettureCMGC(ByVal oMyLettura As ObjImportazione.DatiLetture, ByVal sqlMyConn As SqlConnection, ByVal sqlMyTrans As SqlTransaction)
    '    Dim sSQL As String
    '    Dim sqlMyCmdInsert As SqlCommand

    '    Try
    '        sSQL = "INSERT INTO TP_LETTURE"
    '        sSQL += "(CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA,"
    '        sSQL += "CODMODALITALETTURA,CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE,NUMEROUTENTE,"
    '        sSQL += "IDSTATOLETTURA,INCONGRUENTE,FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE,"
    '        sSQL += "STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)"
    '        sSQL += "VALUES (" & oMyLettura.CodContatore & "," & oMyLettura.CodPeriodo & ",'" & oMyLettura.DataLettura & "',"
    '        If oMyLettura.Lettura = 0 Then
    '            sSQL += "Null,"
    '        Else
    '            sSQL += oMyLettura.Lettura & ","
    '        End If
    '        sSQL += oMyLettura.LetturaTeorica & ",Null," & oMyLettura.ConsumoEffettivo & ",'" & oMyLettura.sNote & "',"
    '        sSQL += oMyLettura.GiornidiConsumo & "," & oMyLettura.ConsumoTeorico & "," & oMyLettura.CodUtente & ",'" & oMyLettura.NumeroUtente & "',"
    '        sSQL += "Null,Null,1,0,0,"
    '        sSQL += Utility.StringOperation.FormatBool(oMyLettura.GiroContatore) & ",0,'" & oMyLettura.DataLetturaPrecedente & "',"
    '        If oMyLettura.LetturaPrecedente = -1 Then
    '            sSQL += "0,"
    '        Else
    '            sSQL += oMyLettura.LetturaPrecedente & ","
    '        End If
    '        If oMyLettura.PrimaLettura Then
    '            sSQL += Utility.StringOperation.FormatBool(oMyLettura.PrimaLettura) & ","
    '        Else
    '            sSQL += "Null" & ","
    '        End If
    '        If oMyLettura.Cod_anomalia1 = -1 Then
    '            sSQL += "Null,"
    '        Else
    '            sSQL += oMyLettura.Cod_anomalia1 & ","
    '        End If
    '        If oMyLettura.Cod_anomalia2 = -1 Then
    '            sSQL += "Null,"
    '        Else
    '            sSQL += oMyLettura.Cod_anomalia2 & ","
    '        End If
    '        If oMyLettura.Cod_anomalia3 = -1 Then
    '            sSQL += "Null,"
    '        Else
    '            sSQL += oMyLettura.Cod_anomalia3 & ","
    '        End If
    '        sSQL += "Null,0,'" & oMyLettura.Provenienza & "'"
    '        sSQL += " )"
    '        sqlMyCmdInsert = New SqlCommand(sSQL, sqlMyConn, sqlMyTrans)
    '        sqlMyCmdInsert.ExecuteNonQuery()

    '        Return 1
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.InsertLettureCMGC.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Public Sub SetDatiCatastaliCMGC(ByVal DetailContatore As objContatore, ByVal sqlConn As SqlConnection)
    '    Dim sSQL As String
    '    Dim sqlCmdInsert As SqlCommand
    '    Dim x As Integer

    '    Try
    '        'elimino tutti i dati catastali precedentemente inseriti per il codcontatore
    '        sSQL = "DELETE"
    '        sSQL += " FROM TR_CONTATORI_CATASTALI"
    '        sSQL += " WHERE (TR_CONTATORI_CATASTALI.CODCONTATORE=" & DetailContatore.nIdContatore & ")"
    '        sqlCmdInsert = New SqlCommand(sSQL, sqlConn)
    '        sqlCmdInsert.ExecuteNonQuery()
    '        'inserisco tutti i dati catastali attuali
    '        For x = 0 To DetailContatore.oDatiCatastali.GetUpperBound(0)
    '            If DetailContatore.oDatiCatastali(x).sInterno <> "" Or DetailContatore.oDatiCatastali(x).sPiano <> "" Or DetailContatore.oDatiCatastali(x).sFoglio <> "" Or DetailContatore.oDatiCatastali(x).sNumero <> "" Or DetailContatore.oDatiCatastali(x).nSubalterno <> 0 Then
    '                sSQL = "INSERT INTO TR_CONTATORI_CATASTALI"
    '                sSQL += " (CODCONTATORE,INTERNO,PIANO,FOGLIO,NUMERO,SUBALTERNO"
    '                sSQL += ",SEZIONE,ESTENSIONE_PARTICELLA,ID_TIPO_PARTICELLA)"
    '                sSQL += " VALUES(" & DetailContatore.nIdContatore & ",'" & DetailContatore.oDatiCatastali(x).sInterno.Replace("'", "") & "',"
    '                sSQL += "'" & DetailContatore.oDatiCatastali(x).sPiano.Replace("'", "") & "','" & DetailContatore.oDatiCatastali(x).sFoglio.Replace("'", "") & "',"
    '                sSQL += "'" & DetailContatore.oDatiCatastali(x).sNumero.Replace("'", "") & "',"
    '                If DetailContatore.oDatiCatastali(x).nSubalterno <> 0 Then
    '                    sSQL += DetailContatore.oDatiCatastali(x).nSubalterno.ToString & ","
    '                Else
    '                    sSQL += "-1,"
    '                End If
    '                sSQL += "'" & DetailContatore.sSezioneCatast.Replace("'", "''") & "',"
    '                sSQL += "'" & DetailContatore.sParticellaCatast.Replace("'", "''") & "',"
    '                sSQL += "'" & DetailContatore.sEstensioneParticellaCatast.Replace("'", "''") & "')"
    '                sqlCmdInsert = New SqlCommand(sSQL, sqlConn)
    '                sqlCmdInsert.ExecuteNonQuery()
    '            End If
    '        Next
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.SetDatiCatastaliCMGC.errore: ", Err)
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RejectLine"></param>
    ''' <param name="sFileOrg"></param>
    ''' <param name="sFileScarti"></param>
    ''' <param name="codEnte"></param>
    ''' <param name="Causale"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' <revision date="15/11/2022">
    ''' Abbinamento per Matricola invece che per codice contatore
    ''' </revision>
    ''' </revisionHistory>
    Private Function WriteScartiLetture(ByVal RejectLine As String, ByVal sFileOrg As String, ByVal sFileScarti As String, ByVal codEnte As String, Causale As String) As Integer
        Try
            Dim sDatiScarti As String = "Data Acquisizione: " & Now.Date() & ";File Origine: " & sFileOrg & ";Riga: " & RejectLine
            sDatiScarti += ";Causale Scarto: " + Causale

            If New ClsGenerale.Generale().WriteFile(sFileScarti, sDatiScarti) = 0 Then
                Return 0
            Else
                Return 1
            End If
        Catch Err As Exception
            Log.Debug(codEnte + " - OPENgovH2O.ClsImport.WriteScartiLetture.errore: ", Err)
            Return -1
        End Try
    End Function
    'Private Function WriteScartiLetture(ByVal oLettura As ObjImportazione.DatiLetture, ByVal sFileOrg As String, ByVal sFileScarti As String, ByVal codEnte As String, Causale As String) As Integer
    '    Try
    '        Dim sDatiScarti As String = "Data Acquisizione: " & Now.Date() & ";File Origine: " & sFileOrg & ";Cod.Ente: " & codEnte & ";Cod.Utente: " & CInt(oLettura.CodUtente.ToString()) & ";Data Lettura: " & oReplace.GiraDataFromDB(oLettura.DataLettura.ToString()) & ";Lettura: " & oLettura.Lettura.ToString()
    '        sDatiScarti += ";Causale Scarto: " + Causale

    '        If New ClsGenerale.Generale().WriteFile(sFileScarti, sDatiScarti) = 0 Then
    '            Return 0
    '        Else
    '            Return 1
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(codEnte + " - OPENgovH2O.ClsImport.WriteScartiLetture.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Public Function WriteFile(ByVal sFile As String, ByVal sDatiFile As String) As Integer
    '    Try
    '        Dim MyFileToWrite As IO.StreamWriter = IO.File.AppendText(sFile)

    '        MyFileToWrite.WriteLine(sDatiFile)
    '        MyFileToWrite.Flush()

    '        MyFileToWrite.Close()
    '        Return 1
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.WriteFile.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DetailContatore"></param>
    ''' <param name="oDatiLettura"></param>
    ''' <param name="myStringConnection"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function setLettureAttuali(ByVal DetailContatore As objContatore, ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal myStringConnection As String) As Boolean
        '=================================================================
        'LETTURE NORMALI
        '=================================================================
        Try
            Dim myCultureInfo As New System.Globalization.CultureInfo("it-IT", True)
            Dim strValoreFondoScala As String
            Dim lngConsumoAppoggio, lngValoreFondoScala, lngCifreContatore, lngCount, lngGiorniDiConsumo, lngConsumoTeoricoAppoggio, lngLetturaTeoricaAppoggio As Integer
            Dim ModDate As New ClsGenerale.Generale
            Dim blnGiroContatore As Boolean = False
            Dim blnLETTURAVUOTA As Boolean = False
            Dim lngTipoOp As Integer = Enumeratore.UpdateRecordStatus.Insert
            Dim GiorniConsumo, letturateorica, ConsumoEffettivo, ConsumoTeorico As String
            Dim blnLAsciatoAvviso As Boolean = False
            Dim blnConsumoNegativo As Boolean = False
            Dim IncongruenteForzato As Boolean = False
            Dim girocontatore As Boolean = False
            Dim myLetturaPrec As New ObjLettura

            strValoreFondoScala = "" : GiorniConsumo = "" : letturateorica = "" : ConsumoEffettivo = "" : ConsumoTeorico = ""

            If oDatiLettura.CodLettura > 0 Then
                lngTipoOp = Enumeratore.UpdateRecordStatus.Updated
            End If

            If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
                myLetturaPrec = getLetturaPrecedente(myStringConnection, oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente)

                If Len(oDatiLettura.Lettura.ToString().Trim()) = 0 Then
                    blnLETTURAVUOTA = True
                End If

                If myLetturaPrec.nLetturaPrec <> -1 Then
                    If Not blnLETTURAVUOTA Then
                        lngConsumoAppoggio = oDatiLettura.Lettura - myLetturaPrec.nLetturaPrec
                    Else
                        lngConsumoAppoggio = 0
                    End If
                    'se blnConsumoNegativo e true considera il consumo negativo
                    If Not blnConsumoNegativo Then
                        If lngConsumoAppoggio < 0 Then
                            'Verifico e considero  il Giro Contatore
                            lngCifreContatore = DetailContatore.sCifreContatore
                            If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
                                '*************************************************************
                                'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato Dall'Utente e flaggare il flag Giro Contatore
                                '*************************************************************
                                For lngCount = 1 To lngCifreContatore
                                    strValoreFondoScala += "9"
                                Next
                                lngValoreFondoScala = strValoreFondoScala
                                lngConsumoAppoggio = lngValoreFondoScala - myLetturaPrec.nLetturaPrec
                                lngConsumoAppoggio = (oDatiLettura.Lettura - 0) + lngConsumoAppoggio
                                blnGiroContatore = True
                            End If
                        End If
                    End If

                    Try
                        lngGiorniDiConsumo = DateDiff(DateInterval.Day, myLetturaPrec.tDataLetturaPrec, DateTime.Parse(oReplace.GiraDataFromDB(oDatiLettura.DataLettura), myCultureInfo))
                    Catch ex As Exception
                        Log.Debug("errore in calcolo giorni consumo-prec->" + myLetturaPrec.tDataLetturaPrec.ToString + "-att->" + oDatiLettura.DataLettura, ex)
                    End Try
                    '***************************************************************
                    'Se il calcolo dei giorni di consumo automatico presenta delle anomalie allora viene forzato
                    '***************************************************************
                    If CStr(lngGiorniDiConsumo) <> GiorniConsumo Then
                        GiorniConsumo = CStr(lngGiorniDiConsumo)
                    End If
                    '***************************************************************
                    'Se il calcolo del ConsumoEffettivo automatico presenta delle anomalie allora viene forzato solo se non si tratta di giro contatore
                    '***************************************************************
                    If CStr(lngConsumoAppoggio) <> ConsumoEffettivo Then
                        If blnGiroContatore Then
                            ConsumoEffettivo = CStr(lngConsumoAppoggio)
                        Else
                            If girocontatore = False And IncongruenteForzato = False Then
                                ConsumoEffettivo = CStr(lngConsumoAppoggio)
                            End If
                        End If
                    End If
                    '*********************************************************************************************************************
                    'Forzatura del consumo teorico e della lettura teorica se il calcolo automatico presenta delle anomalie
                    'LetturaTeorica= letturaPrecedente + consumo teorico
                    'Calcolo Del Consumo teorico
                    lngConsumoTeoricoAppoggio = CalcolaConsumoTeorico(lngGiorniDiConsumo, oDatiLettura.CodContatore, oDatiLettura.CodUtente, myStringConnection)
                    lngLetturaTeoricaAppoggio = CalcolaLetturaTeorica(myLetturaPrec.nLetturaPrec, lngConsumoTeoricoAppoggio, DetailContatore)
                    If CStr(lngConsumoTeoricoAppoggio) <> ConsumoTeorico Then
                        ConsumoTeorico = CStr(lngConsumoTeoricoAppoggio)
                    End If
                    If CStr(lngLetturaTeoricaAppoggio) <> letturateorica Then
                        letturateorica = CStr(lngLetturaTeoricaAppoggio)
                    End If
                    '*********************************************************************************************************************
                Else
                    ConsumoEffettivo = 0
                    GiorniConsumo = 0
                    ConsumoTeorico = 0
                    letturateorica = 0
                End If
            End If

            Try
                If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
                    Dim myLettura As New ObjLettura
                    myLettura.bIsStorica = True
                    myLettura.bIsGiroContatore = blnGiroContatore
                    myLettura.nConsumo = ConsumoEffettivo
                    myLettura.nConsumoTeorico = ConsumoTeorico
                    myLettura.nGiorni = GiorniConsumo
                    myLettura.nIdContatore = oDatiLettura.CodContatore
                    myLettura.nIdPeriodo = oDatiLettura.CodPeriodo
                    myLettura.nIdUtente = oDatiLettura.CodUtente
                    myLettura.nLetturaAtt = oDatiLettura.Lettura
                    myLettura.nLetturaPrec = myLetturaPrec.nLetturaPrec
                    myLettura.sLetturaTeorica = letturateorica
                    myLettura.sNote = oDatiLettura.sNote
                    myLettura.sNUtente = oDatiLettura.NumeroUtente
                    myLettura.sProvenienza = "Data Entry Massivo"
                    myLettura.tDataInserimento = Now
                    myLettura.tDataLetturaAtt = DateTime.Parse(oReplace.GiraDataFromDB(oDatiLettura.DataLettura), myCultureInfo)
                    myLettura.tDataLetturaPrec = myLetturaPrec.tDataLetturaPrec
                    Log.Debug("valorizzo date oggetto da lettura tDataLetturaAtt->" + oDatiLettura.DataLettura + " - tDataLetturaPrec->" + myLetturaPrec.tDataLetturaPrec.ToString)
                    Log.Debug("a tDataLetturaAtt->" + myLettura.tDataLetturaAtt.ToString + " - tDataLetturaPrec->" + myLettura.tDataLetturaPrec.ToString)
                    myLettura.IdLettura = New GestLetture().SetLettura(myLettura)
                    If myLettura.IdLettura <= 0 Then
                        Return False
                    End If
                End If
                Return True
            Catch er As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuali.errore:PRIMO-> ", er)
                Return False
            End Try
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuali.errore: ", Err)
            Return False
        End Try
    End Function
    'Public Function setLettureAttuali(ByVal DetailContatore As objContatore, ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal conn As SqlConnection) As Boolean
    '    '=================================================================
    '    'LETTURE NORMALI
    '    '=================================================================
    '    Try
    '        Dim myCultureInfo As New System.Globalization.CultureInfo("it-IT", True)
    '        Dim sSQL, dataLetturaprecedente, strValoreFondoScala As String
    '        Dim PrimaLettura As Boolean = True
    '        Dim lngConsumoAppoggio, lngValoreFondoScala, lngCifreContatore, lngCount, lngGiorniDiConsumo, lngConsumoTeoricoAppoggio, lngLetturaTeoricaAppoggio, lngLetturaPrecedente As Long
    '        Dim ModDate As New ClsGenerale.Generale
    '        Dim sqlTrans As SqlTransaction
    '        Dim sqlCmdInsert As SqlCommand
    '        Dim blnGiroContatore As Boolean = False
    '        Dim blnLETTURAVUOTA As Boolean = False
    '        Dim lngTipoOp As Long = Enumeratore.UpdateRecordStatus.Insert
    '        Dim GiorniConsumo, letturateorica, ConsumoEffettivo, ConsumoTeorico As String
    '        Dim blnLAsciatoAvviso As Boolean = False
    '        Dim blnConsumoNegativo As Boolean = False
    '        Dim IncongruenteForzato As Boolean = False
    '        Dim girocontatore As Boolean = False

    '        setLettureAttuali = True

    '        If oDatiLettura.CodLettura > 0 Then
    '            lngTipoOp = Enumeratore.UpdateRecordStatus.Updated
    '        End If

    '        If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then

    '            lngLetturaPrecedente = getLetturaPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente)
    '            dataLetturaprecedente = getDataPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente)

    '            If Len(oDatiLettura.Lettura.ToString().Trim()) = 0 Then
    '                blnLETTURAVUOTA = True
    '            End If

    '            '            If lngLetturaPrecedente <> -1 Then
    '            If lngLetturaPrecedente <> -1 Then
    '                PrimaLettura = False
    '                If Not blnLETTURAVUOTA Then
    '                    lngConsumoAppoggio = utility.stringoperation.formatint(oDatiLettura.Lettura) - lngLetturaPrecedente
    '                Else
    '                    lngConsumoAppoggio = 0
    '                End If
    '                '=================================================
    '                'se blnConsumoNegativo e true considera il consumo negativo
    '                If Not blnConsumoNegativo Then
    '                    If lngConsumoAppoggio < 0 Then
    '                        'Verifico e considero  il Giro Contatore
    '                        lngCifreContatore = utility.stringoperation.formatint(DetailContatore.sCifreContatore)
    '                        If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                            '*************************************************************
    '                            'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                            'Dall'Utente e flaggare il flag Giro Contatore
    '                            '*************************************************************
    '                            For lngCount = 1 To lngCifreContatore
    '                                strValoreFondoScala = strValoreFondoScala & "9"
    '                            Next
    '                            lngValoreFondoScala = utility.stringoperation.formatint(strValoreFondoScala)
    '                            lngConsumoAppoggio = lngValoreFondoScala - lngLetturaPrecedente
    '                            lngConsumoAppoggio = (utility.stringoperation.formatint(oDatiLettura.Lettura) - 0) + lngConsumoAppoggio
    '                            blnGiroContatore = True
    '                        End If
    '                    End If
    '                End If

    '                If IsDate(oReplace.GiraDataFromDB(utility.stringoperation.formatstring(dataLetturaprecedente))) Then
    '                    'lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(oReplace.GiraDataFromDB(dataLetturaprecedente)), CDate(oReplace.GiraDataFromDB(oDatiLettura.DataLettura)))
    '                    lngGiorniDiConsumo = DateDiff(DateInterval.Day, DateTime.Parse(oReplace.GiraDataFromDB(dataLetturaprecedente), myCultureInfo), DateTime.Parse(oReplace.GiraDataFromDB(oDatiLettura.DataLettura), myCultureInfo))
    '                End If
    '                '***************************************************************
    '                'Se il calcolo dei giorni di consumo automatico presenta delle anomalie allora
    '                'viene forzato
    '                '***************************************************************
    '                If CStr(lngGiorniDiConsumo) <> GiorniConsumo Then
    '                    GiorniConsumo = CStr(lngGiorniDiConsumo)
    '                End If
    '                '***************************************************************
    '                'Se il calcolo del ConsumoEffettivo automatico presenta delle anomalie allora
    '                'viene forzato solo se non si tratta di giro contatore
    '                '***************************************************************
    '                If CStr(lngConsumoAppoggio) <> ConsumoEffettivo Then
    '                    If blnGiroContatore Then
    '                        ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                    Else
    '                        If girocontatore = False And IncongruenteForzato = False Then
    '                            ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                        End If
    '                    End If
    '                End If
    '                '*********************************************************************************************************************
    '                'Forzatura del consumo teorico e della lettura teorica se il calcolo automatico presenta delle
    '                'anomalie
    '                'LetturaTeorica= letturaPrecedente + consumo teorico
    '                'Calcolo Del Consumo teorico
    '                lngConsumoTeoricoAppoggio = CalcolaConsumoTeorico(lngGiorniDiConsumo, oDatiLettura.CodContatore, oDatiLettura.CodUtente, conn)
    '                lngLetturaTeoricaAppoggio = CalcolaLetturaTeorica(lngLetturaPrecedente, lngConsumoTeoricoAppoggio, DetailContatore)
    '                If CStr(lngConsumoTeoricoAppoggio) <> ConsumoTeorico Then
    '                    ConsumoTeorico = CStr(lngConsumoTeoricoAppoggio)
    '                End If
    '                If CStr(lngLetturaTeoricaAppoggio) <> letturateorica Then
    '                    letturateorica = CStr(lngLetturaTeoricaAppoggio)
    '                End If
    '                '*********************************************************************************************************************
    '            Else
    '                ConsumoEffettivo = 0
    '                GiorniConsumo = 0
    '                ConsumoTeorico = 0
    '                letturateorica = 0
    '            End If
    '        End If

    '        Try
    '            If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
    '                sSQL = "INSERT INTO TP_LETTURE" & vbCrLf
    '                sSQL += "(CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA," & vbCrLf
    '                sSQL += "CODMODALITALETTURA,CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE," & vbCrLf
    '                sSQL += "IDSTATOLETTURA,INCONGRUENTE,FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE," & vbCrLf
    '                'modifica del 14/02/2007
    '                'sSQL+="STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO)" & vbCrLf
    '                sSQL += "STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)" & vbCrLf
    '                sSQL += "VALUES ( " & vbCrLf
    '                sSQL += oDatiLettura.CodContatore & "," & vbCrLf
    '                sSQL += oDatiLettura.CodPeriodo & "," & vbCrLf
    '                sSQL += utility.stringoperation.formatstring(oDatiLettura.DataLettura) & "," & vbCrLf
    '                If blnLETTURAVUOTA Then
    '                    sSQL += "Null" & "," & vbCrLf
    '                Else
    '                    sSQL += utility.stringoperation.formatint(oDatiLettura.Lettura) & "," & vbCrLf
    '                End If
    '                sSQL += utility.stringoperation.formatint(letturateorica) & "," & vbCrLf
    '                sSQL += "Null," & vbCrLf
    '                sSQL += utility.stringoperation.formatint(ConsumoEffettivo) & "," & vbCrLf
    '                sSQL += utility.stringoperation.formatstring(oDatiLettura.sNote) & "," & vbCrLf
    '                sSQL += utility.stringoperation.formatstring(GiorniConsumo) & "," & vbCrLf
    '                sSQL += utility.stringoperation.formatstring(ConsumoTeorico) & "," & vbCrLf
    '                sSQL += oDatiLettura.CodUtente & "," & vbCrLf
    '                sSQL += "Null," & vbCrLf
    '                sSQL += "Null" & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica e quindi e da considerarsi come fatturata
    '                'sSQL+="Null" & "," & vbCrLf
    '                sSQL += "0 , " & vbCrLf
    '                '**********************************************************
    '                sSQL += "0," & vbCrLf
    '                sSQL += "0," & vbCrLf
    '                If blnGiroContatore Then
    '                    girocontatore = True
    '                End If
    '                sSQL += utility.stringoperation.formatbool(girocontatore) & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica Flag Storica Alzato
    '                sSQL += "1" & "," & vbCrLf
    '                '**********************************************************
    '                sSQL += utility.stringoperation.formatstring(dataLetturaprecedente) & "," & vbCrLf
    '                'If lngLetturaPrecedente = -1 Then
    '                If lngLetturaPrecedente = -1 Then
    '                    sSQL += 0 & "," & vbCrLf
    '                Else
    '                    sSQL += lngLetturaPrecedente & "," & vbCrLf
    '                End If
    '                If PrimaLettura Then
    '                    sSQL += utility.stringoperation.formatbool(PrimaLettura) & "," & vbCrLf
    '                Else
    '                    sSQL += "Null" & "," & vbCrLf
    '                End If
    '                '*** salvataggio codice anomalia
    '                If oDatiLettura.Cod_anomalia1 = -1 Then
    '                    sSQL += "Null,"
    '                Else
    '                    sSQL += oDatiLettura.Cod_anomalia1 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia2 = -1 Then
    '                    sSQL += "Null,"
    '                Else
    '                    sSQL += oDatiLettura.Cod_anomalia2 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia3 = -1 Then
    '                    sSQL += "Null,"
    '                Else
    '                    sSQL += oDatiLettura.Cod_anomalia3 & ","
    '                End If
    '                sSQL += utility.stringoperation.formatstring(oReplace.GiraData(oDatiLettura.DataDiPassaggio)) & "," & vbCrLf
    '                sSQL += "0,'Data Entry Massivo'" & vbCrLf
    '                sSQL += " )" & vbCrLf
    '                sqlTrans = conn.BeginTransaction
    '                sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)

    '                sqlCmdInsert.ExecuteNonQuery()

    '                If blnLAsciatoAvviso Then
    '                    sSQL = "UPDATE TP_CONTATORI SET LASCIATOAVVISO=1" & vbCrLf
    '                    sSQL += "WHERE CODCONTATORE=" & oDatiLettura.CodContatore & vbCrLf

    '                    sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)
    '                    sqlCmdInsert.ExecuteNonQuery()
    '                End If
    '            End If
    '            sqlTrans.Commit()
    '            Return setLettureAttuali

    '        Catch er As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuali.errore:PRIMO-> ", er)
    '            sqlTrans.Rollback()
    '            Return False
    '            Throw
    '        Finally
    '            sqlTrans.Dispose()
    '        End Try
    '    Catch Err As Exception
    '        Dim MyErr As String = Err.Message
    '        MyErr = MyErr + "male, male, male!!!"
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuali.errore: ", Err)
    '    End Try
    'End Function

    'Public Function setLettureAttuali(ByVal ForzaLett As Boolean, ByVal DetailContatore As objContatore, ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal conn As SqlConnection) As Boolean
    '    '=================================================================
    '    'LETTURE NORMALI
    '    '=================================================================
    '    Try
    '        Dim sSQL, dataLetturaprecedente, strValoreFondoScala As String
    '        Dim PrimaLettura As Boolean = True
    '        Dim lngConsumoAppoggio, lngValoreFondoScala, lngCifreContatore, lngCount, lngGiorniDiConsumo, lngConsumoTeoricoAppoggio, lngLetturaTeoricaAppoggio, lngLetturaPrecedente As Long
    '        Dim ModDate As New ClsGenerale.Generale
    '        Dim sqlTrans As SqlTransaction
    '        Dim sqlCmdInsert As SqlCommand
    '        Dim blnGiroContatore As Boolean = False
    '        Dim blnLETTURAVUOTA As Boolean = False
    '        Dim lngTipoOp As Long = Enumeratore.UpdateRecordStatus.Insert
    '        Dim GiorniConsumo, letturateorica, ConsumoEffettivo, ConsumoTeorico As String
    '        Dim blnLAsciatoAvviso As Boolean = False
    '        Dim blnConsumoNegativo As Boolean = False
    '        Dim IncongruenteForzato As Boolean = False
    '        Dim girocontatore As Boolean = False

    '        setLettureAttuali = True

    '        If oDatiLettura.CodLettura > 0 Then
    '            lngTipoOp = Enumeratore.UpdateRecordStatus.Updated
    '        End If

    '        If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
    '            If ForzaLett = True Then
    '                lngLetturaPrecedente = oDatiLettura.LetturaPrecedente
    '                dataLetturaprecedente = oDatiLettura.DataLetturaPrecedente
    '            Else
    '                lngLetturaPrecedente = getLetturaPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente)
    '                dataLetturaprecedente = getDataPrecedente(oDatiLettura.DataLettura, oDatiLettura.CodContatore, oDatiLettura.CodUtente)
    '            End If
    '            If Len(oDatiLettura.Lettura.ToString().Trim()) = 0 Then
    '                blnLETTURAVUOTA = True
    '            End If

    '            If lngLetturaPrecedente <> -1 Then
    '                PrimaLettura = False
    '                If Not blnLETTURAVUOTA Then
    '                    lngConsumoAppoggio = utility.stringoperation.formatint(oDatiLettura.Lettura) - lngLetturaPrecedente
    '                Else
    '                    lngConsumoAppoggio = 0
    '                End If
    '                '=================================================
    '                'se blnConsumoNegativo e true considera il consumo negativo
    '                If Not blnConsumoNegativo Then
    '                    If lngConsumoAppoggio < 0 Then
    '                        'Verifico e considero  il Giro Contatore
    '                        lngCifreContatore = utility.stringoperation.formatint(DetailContatore.sCifreContatore)
    '                        If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                            '*************************************************************
    '                            'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                            'Dall'Utente e flaggare il flag Giro Contatore
    '                            '*************************************************************
    '                            For lngCount = 1 To lngCifreContatore
    '                                strValoreFondoScala = strValoreFondoScala & "9"
    '                            Next
    '                            lngValoreFondoScala = utility.stringoperation.formatint(strValoreFondoScala)
    '                            lngConsumoAppoggio = lngValoreFondoScala - lngLetturaPrecedente
    '                            lngConsumoAppoggio = (utility.stringoperation.formatint(oDatiLettura.Lettura) - 0) + lngConsumoAppoggio
    '                            blnGiroContatore = True
    '                        End If
    '                    End If
    '                End If

    '                If IsDate(oReplace.GiraDataFromDB(Utility.StringOperation.FormatString(dataLetturaprecedente))) Then
    '                    lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(oReplace.GiraDataFromDB(dataLetturaprecedente)), CDate(oReplace.GiraDataFromDB(oDatiLettura.DataLettura)))
    '                End If
    '                '***************************************************************
    '                'Se il calcolo dei giorni di consumo automatico presenta delle anomalie allora
    '                'viene forzato
    '                '***************************************************************
    '                If CStr(lngGiorniDiConsumo) <> GiorniConsumo Then
    '                    GiorniConsumo = CStr(lngGiorniDiConsumo)
    '                End If
    '                '***************************************************************
    '                'Se il calcolo del ConsumoEffettivo automatico presenta delle anomalie allora
    '                'viene forzato solo se non si tratta di giro contatore
    '                '***************************************************************
    '                If CStr(lngConsumoAppoggio) <> ConsumoEffettivo Then
    '                    If blnGiroContatore Then
    '                        ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                    Else
    '                        If girocontatore = False And IncongruenteForzato = False Then
    '                            ConsumoEffettivo = CStr(lngConsumoAppoggio)
    '                        End If
    '                    End If
    '                End If
    '                '*********************************************************************************************************************
    '                'Forzatura del consumo teorico e della lettura teorica se il calcolo automatico presenta delle
    '                'anomalie
    '                'LetturaTeorica= letturaPrecedente + consumo teorico
    '                'Calcolo Del Consumo teorico
    '                lngConsumoTeoricoAppoggio = CalcolaConsumoTeorico(lngGiorniDiConsumo, oDatiLettura.CodContatore, oDatiLettura.CodUtente, myStringConnection)
    '                lngLetturaTeoricaAppoggio = CalcolaLetturaTeorica(lngLetturaPrecedente, lngConsumoTeoricoAppoggio, DetailContatore)
    '                If CStr(lngConsumoTeoricoAppoggio) <> ConsumoTeorico Then
    '                    ConsumoTeorico = CStr(lngConsumoTeoricoAppoggio)
    '                End If
    '                If CStr(lngLetturaTeoricaAppoggio) <> letturateorica Then
    '                    letturateorica = CStr(lngLetturaTeoricaAppoggio)
    '                End If
    '                '*********************************************************************************************************************
    '            Else
    '                ConsumoEffettivo = 0
    '                GiorniConsumo = 0
    '                ConsumoTeorico = 0
    '                letturateorica = 0
    '            End If
    '        End If
    '        Try
    '            If lngTipoOp = Enumeratore.UpdateRecordStatus.Insert Then
    '                sSQL = "INSERT INTO TP_LETTURE" & vbCrLf
    '                sSQL += "(CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA," & vbCrLf
    '                sSQL += "CODMODALITALETTURA,CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE," & vbCrLf
    '                sSQL += "IDSTATOLETTURA,INCONGRUENTE,FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE," & vbCrLf
    '                'modifica del 14/02/2007
    '                'sSQL+="STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO)" & vbCrLf
    '                sSQL += "STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)" & vbCrLf
    '                sSQL += "VALUES ( " & vbCrLf
    '                sSQL += oDatiLettura.CodContatore & "," & vbCrLf
    '                sSQL += oDatiLettura.CodPeriodo & "," & vbCrLf
    '                sSQL += Utility.StringOperation.FormatString(oDatiLettura.DataLettura) & "," & vbCrLf
    '                If blnLETTURAVUOTA Then
    '                    sSQL += "Null" & "," & vbCrLf
    '                Else
    '                    sSQL += utility.stringoperation.formatint(oDatiLettura.Lettura) & "," & vbCrLf
    '                End If
    '                sSQL += utility.stringoperation.formatint(letturateorica) & "," & vbCrLf
    '                sSQL += "Null," & vbCrLf
    '                sSQL += utility.stringoperation.formatint(ConsumoEffettivo) & "," & vbCrLf
    '                sSQL += Utility.StringOperation.FormatString(oDatiLettura.sNote) & "," & vbCrLf
    '                sSQL += Utility.StringOperation.FormatString(GiorniConsumo) & "," & vbCrLf
    '                sSQL += Utility.StringOperation.FormatString(ConsumoTeorico) & "," & vbCrLf
    '                sSQL += oDatiLettura.CodUtente & "," & vbCrLf
    '                sSQL += "Null," & vbCrLf
    '                sSQL += "Null" & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica e quindi e da considerarsi come fatturata
    '                'sSQL+="Null" & "," & vbCrLf
    '                sSQL += "0 , " & vbCrLf
    '                '**********************************************************
    '                sSQL += "0," & vbCrLf
    '                sSQL += "0," & vbCrLf
    '                If blnGiroContatore Then
    '                    girocontatore = True
    '                End If
    '                sSQL += Utility.StringOperation.FormatBool(girocontatore) & "," & vbCrLf
    '                '**********************************************************
    '                'Si tratta di Lettura Storica Flag Storica Alzato
    '                sSQL += "1" & "," & vbCrLf
    '                '**********************************************************
    '                sSQL += Utility.StringOperation.FormatString(dataLetturaprecedente) & "," & vbCrLf
    '                'If lngLetturaPrecedente = -1 Then
    '                If lngLetturaPrecedente = -1 Then
    '                    sSQL += 0 & "," & vbCrLf
    '                Else
    '                    sSQL += lngLetturaPrecedente & "," & vbCrLf
    '                End If
    '                If PrimaLettura Then
    '                    sSQL += Utility.StringOperation.FormatBool(PrimaLettura) & "," & vbCrLf
    '                Else
    '                    sSQL += "Null" & "," & vbCrLf
    '                End If
    '                '*** salvataggio codice anomalia
    '                If oDatiLettura.Cod_anomalia1 = -1 Then
    '                    sSQL += "Null,"
    '                Else
    '                    sSQL += oDatiLettura.Cod_anomalia1 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia2 = -1 Then
    '                    sSQL += "Null,"
    '                Else
    '                    sSQL += oDatiLettura.Cod_anomalia2 & ","
    '                End If
    '                If oDatiLettura.Cod_anomalia3 = -1 Then
    '                    sSQL += "Null,"
    '                Else
    '                    sSQL += oDatiLettura.Cod_anomalia3 & ","
    '                End If
    '                sSQL += Utility.StringOperation.FormatString(oReplace.GiraData(oDatiLettura.DataDiPassaggio)) & "," & vbCrLf
    '                sSQL += "0,'Data Entry Massivo'" & vbCrLf
    '                sSQL += " )" & vbCrLf
    '                sqlTrans = conn.BeginTransaction
    '                sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)

    '                sqlCmdInsert.ExecuteNonQuery()

    '                If blnLAsciatoAvviso Then
    '                    sSQL = "UPDATE TP_CONTATORI SET LASCIATOAVVISO=1" & vbCrLf
    '                    sSQL += "WHERE CODCONTATORE=" & oDatiLettura.CodContatore & vbCrLf

    '                    sqlCmdInsert = New SqlCommand(sSQL, conn, sqlTrans)
    '                    sqlCmdInsert.ExecuteNonQuery()
    '                End If
    '            End If
    '            sqlTrans.Commit()
    '            Return setLettureAttuali

    '        Catch er As Exception
    '            sqlTrans.Rollback()
    '            Return False
    '            Throw
    '        Finally
    '            sqlTrans.Dispose()
    '        End Try
    '    Catch Err As Exception
    '        Dim MyErr As String = Err.Message
    '        MyErr = MyErr + "male, male, male!!!"
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.setLettureAttuali.errore: ", Err)

    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idContatore"></param>
    ''' <param name="sMatricola"></param>
    ''' <param name="codEnte"></param>
    ''' <param name="myStringConnection"></param>
    ''' <returns></returns>
    Public Function TrovaContatore(ByVal idContatore As Integer, ByVal sMatricola As String, ByVal codEnte As String, ByVal myStringConnection As String) As objContatore
        Dim dvMyDati As New DataView
        Dim ModDate As New ClsGenerale.Generale
        Dim oDatiContatore As New objContatore

        Try
            dvMyDati = New GestContatori().GetElencoContrattiContatori(myStringConnection, codEnte, sMatricola, "", "", "", "", "", "", -1, 1, False, idContatore)
            For Each myRow As DataRowView In dvMyDati
                oDatiContatore.nIdContatore = StringOperation.FormatString(myRow("CODCONTATORE"))
                oDatiContatore.nIdUtente = StringOperation.FormatInt(myRow("cod_contribuente"))
                oDatiContatore.nIdContatorePrec = StringOperation.FormatInt(myRow("CODCONTATOREPRECEDENTE"))
                oDatiContatore.nIdContatoreSucc = StringOperation.FormatInt(myRow("CODCONTATORESUCCESSIVO"))
                oDatiContatore.nDiametroContatore = StringOperation.FormatInt(myRow("CODDIAMETROCONTATORE"))
                oDatiContatore.nDiametroPresa = StringOperation.FormatInt(myRow("CODDIAMETROPRESA"))
                oDatiContatore.nIdImpianto = StringOperation.FormatInt(myRow("CODIMPIANTO"))
                oDatiContatore.nIdAttivita = StringOperation.FormatInt(myRow("IDTIPOATTIVITA"))
                oDatiContatore.nTipoContatore = StringOperation.FormatInt(myRow("IDTIPOCONTATORE"))
                oDatiContatore.sSequenza = StringOperation.FormatString(myRow("SEQUENZA"))
                oDatiContatore.sIdEnte = StringOperation.FormatInt(myRow("CODENTE"))
                oDatiContatore.sMatricola = StringOperation.FormatString(myRow("MATRICOLA"))
                oDatiContatore.nGiro = StringOperation.FormatInt(myRow("IDGIRO"))
                oDatiContatore.nNumeroUtenze = StringOperation.FormatInt(myRow("NUMEROUTENZE"))
                oDatiContatore.sNumeroUtente = StringOperation.FormatString(myRow("NUMEROUTENTE"))
                oDatiContatore.sStatoContatore = StringOperation.FormatString(myRow("STATOCONTATORE"))
                oDatiContatore.sCifreContatore = StringOperation.FormatString(myRow("CIFRECONTATORE"))
                oDatiContatore.sPenalita = StringOperation.FormatString(myRow("PENALITA"))
                oDatiContatore.sDataAttivazione = StringOperation.FormatString(myRow("DATAATTIVAZIONE"))
                oDatiContatore.sDataCessazione = StringOperation.FormatString(myRow("DATACESSAZIONE"))
                oDatiContatore.nIdVia = StringOperation.FormatInt(myRow("COD_STRADA"))
                oDatiContatore.sUbicazione = StringOperation.FormatString(myRow("VIA_UBICAZIONE"))
                oDatiContatore.sCivico = StringOperation.FormatString(myRow("CIVICO_UBICAZIONE"))
                oDatiContatore.sCodiceISTAT = StringOperation.FormatString(myRow("CODICE_ISTAT"))
                oDatiContatore.sIdEnteAppartenenza = StringOperation.FormatString(myRow("CODENTEAPPARTENENZA1"))
            Next
            Return oDatiContatore
        Catch ex As Exception
            Log.Debug(codEnte + " - OPENgovH2O.ClsImport.TrovaContatore.errore: ", ex)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function TrovaContatore(ByVal idContatore As Integer, ByVal sMatricola As String, ByVal codEnte As String, ByVal conn As SqlConnection) As objContatore
    '    Dim sSQL As String = ""
    '    Dim cmdCont As New SqlCommand
    '    Dim ModDate As New ClsGenerale.Generale
    '    Dim drDetailsContatore as sqldatareader=nothing
    '    Dim oDatiContatore As New objContatore

    '    sSQL = "SELECT TP_CONTATORI.*, TR_CONTATORI_UTENTE.COD_CONTRIBUENTE"
    '    sSQL += " FROM TP_CONTATORI WITH (NOLOCK)"
    '    sSQL += " INNER JOIN TR_CONTATORI_UTENTE ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_UTENTE.CODCONTATORE"
    '    sSQL += " WHERE (TP_CONTATORI.CODENTE = " & codEnte & ")"
    '    If sMatricola <> "" Then
    '        sSQL += " AND (TP_CONTATORI.MATRICOLA ='" & sMatricola & "')"
    '    Else
    '        sSQL += " AND (TP_CONTATORI.CODCONTATORE = " & idContatore & ")"
    '    End If
    '    sSQL += " AND (TP_CONTATORI.DATACESSAZIONE IS NULL OR TP_CONTATORI.DATACESSAZIONE='')"
    '    Try
    '        cmdCont.CommandType = CommandType.Text
    '        cmdCont.CommandText = sSQL
    '        cmdCont.Connection = conn
    '        If cmdCont.Connection.State = ConnectionState.Closed Then
    '            cmdCont.Connection.Open()
    '        End If
    '        drDetailsContatore = cmdCont.ExecuteReader()

    '        If drDetailsContatore.Read Then
    '            oDatiContatore.nIdContatore = CStr(drDetailsContatore.Item("CODCONTATORE"))
    '            oDatiContatore.nIdUtente = CInt(drDetailsContatore.Item("cod_contribuente"))
    '            If Not IsDBNull(drDetailsContatore.Item("CODCONTATOREPRECEDENTE")) Then
    '                oDatiContatore.nIdContatorePrec = CInt(drDetailsContatore.Item("CODCONTATOREPRECEDENTE").ToString())
    '            Else
    '                oDatiContatore.nIdContatorePrec = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CODCONTATORESUCCESSIVO")) Then
    '                oDatiContatore.nIdContatoreSucc = CInt(drDetailsContatore.Item("CODCONTATORESUCCESSIVO").ToString())
    '            Else
    '                oDatiContatore.nIdContatoreSucc = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CODDIAMETROCONTATORE")) Then
    '                oDatiContatore.nDiametroContatore = CInt(drDetailsContatore.Item("CODDIAMETROCONTATORE").ToString())
    '            Else
    '                oDatiContatore.nDiametroContatore = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CODDIAMETROPRESA")) Then
    '                oDatiContatore.nDiametroPresa = CInt(drDetailsContatore.Item("CODDIAMETROPRESA").ToString())
    '            Else
    '                oDatiContatore.nDiametroPresa = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CODIMPIANTO")) Then
    '                oDatiContatore.nIdImpianto = drDetailsContatore.Item("CODIMPIANTO").ToString
    '            Else
    '                oDatiContatore.nIdImpianto = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("IDTIPOATTIVITA")) Then
    '                oDatiContatore.nIdAttivita = CInt(drDetailsContatore.Item("IDTIPOATTIVITA").ToString())
    '            Else
    '                oDatiContatore.nIdAttivita = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("IDTIPOCONTATORE")) Then
    '                oDatiContatore.nTipoContatore = CInt(drDetailsContatore.Item("IDTIPOCONTATORE").ToString())
    '            Else
    '                oDatiContatore.nTipoContatore = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("SEQUENZA")) Then
    '                oDatiContatore.sSequenza = drDetailsContatore.Item("SEQUENZA").ToString()
    '            Else
    '                oDatiContatore.sSequenza = ""
    '            End If
    '            oDatiContatore.sIdEnte = CInt(drDetailsContatore.Item("CODENTE").ToString())
    '            If Not IsDBNull(drDetailsContatore.Item("MATRICOLA")) Then
    '                oDatiContatore.sMatricola = drDetailsContatore.Item("MATRICOLA").ToString()
    '            Else
    '                oDatiContatore.sMatricola = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("IDGIRO")) Then
    '                oDatiContatore.nGiro = CInt(drDetailsContatore.Item("IDGIRO").ToString())
    '            Else
    '                oDatiContatore.nGiro = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("NUMEROUTENZE")) Then
    '                oDatiContatore.nNumeroUtenze = CInt(drDetailsContatore.Item("NUMEROUTENZE").ToString())
    '            Else
    '                oDatiContatore.nNumeroUtenze = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("NUMEROUTENTE")) Then
    '                oDatiContatore.sNumeroUtente = drDetailsContatore.Item("NUMEROUTENTE").ToString()
    '            Else
    '                oDatiContatore.sNumeroUtente = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("STATOCONTATORE")) Then
    '                oDatiContatore.sStatoContatore = drDetailsContatore.Item("STATOCONTATORE").ToString()
    '            Else
    '                oDatiContatore.sStatoContatore = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CIFRECONTATORE")) Then
    '                oDatiContatore.sCifreContatore = drDetailsContatore.Item("CIFRECONTATORE").ToString()
    '            Else
    '                oDatiContatore.sCifreContatore = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("PENALITA")) Then
    '                oDatiContatore.sPenalita = drDetailsContatore.Item("PENALITA").ToString()
    '            Else
    '                oDatiContatore.sPenalita = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("DATAATTIVAZIONE")) Then
    '                oDatiContatore.sDataAttivazione = drDetailsContatore.Item("DATAATTIVAZIONE").ToString()
    '            Else
    '                oDatiContatore.sDataAttivazione = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("DATACESSAZIONE")) Then
    '                oDatiContatore.sDataCessazione = drDetailsContatore.Item("DATACESSAZIONE").ToString()
    '            Else
    '                oDatiContatore.sDataCessazione = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("COD_STRADA")) Then
    '                oDatiContatore.nIdVia = CInt(drDetailsContatore.Item("COD_STRADA").ToString())
    '            Else
    '                oDatiContatore.nIdVia = -1
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("VIA_UBICAZIONE")) Then
    '                oDatiContatore.sUbicazione = drDetailsContatore.Item("VIA_UBICAZIONE").ToString()
    '            Else
    '                oDatiContatore.sUbicazione = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CIVICO_UBICAZIONE")) Then
    '                oDatiContatore.sCivico = drDetailsContatore.Item("CIVICO_UBICAZIONE").ToString()
    '            Else
    '                oDatiContatore.sCivico = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CODICE_ISTAT")) Then
    '                oDatiContatore.sCodiceISTAT = drDetailsContatore.Item("CODICE_ISTAT").ToString()
    '            Else
    '                oDatiContatore.sCodiceISTAT = ""
    '            End If
    '            If Not IsDBNull(drDetailsContatore.Item("CODENTEAPPARTENENZA1")) Then
    '                oDatiContatore.sIdEnteAppartenenza = drDetailsContatore.Item("CODENTEAPPARTENENZA1").ToString()
    '            Else
    '                oDatiContatore.sIdEnteAppartenenza = ""
    '            End If

    '            Return oDatiContatore
    '        End If

    '    Catch ex As Exception

    '        Log.Debug(codEnte + " - OPENgovH2O.ClsImport.TrovaContatore.errore: ", ex)
    '        Return Nothing
    '    Finally
    '        drDetailsContatore.Close()
    '    End Try
    'End Function

    ''' <summary>
    ''' VERIFICA SE ESISTE UNA LETTURA PRECEDENTE
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="DataLetturaAttuale"></param>
    ''' <param name="IDContatore"></param>
    ''' <param name="IDUtente"></param>
    ''' <returns></returns>
    Public Function getLetturaPrecedente(myStringConnection As String, ByVal DataLetturaAttuale As String, ByVal IDContatore As Integer, ByVal IDUtente As Integer) As ObjLettura
        Dim dvMyDati As New DataView
        Dim myLettura As New ObjLettura
        Dim myCultureInfo As New System.Globalization.CultureInfo("it-IT", True)

        Try
            dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, myStringConnection, 1, IDContatore, DataLetturaAttuale, "<")
            For Each myRow As DataRowView In dvMyDati
                myLettura.nIdContatore = IDContatore
                myLettura.tDataLetturaPrec = DateTime.Parse(oReplace.GiraDataFromDB(myRow("datalettura")), myCultureInfo)
                myLettura.nLetturaPrec = StringOperation.FormatInt(myRow("lettura"))
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.getLetturaPrecedente.errore: ", Err)
            myLettura = New ObjLettura
        Finally
            dvMyDati.Dispose()
        End Try
        Return myLettura
    End Function
    'Public Function getLetturaPrecedente(ByVal DataLetturaAttuale As String, ByVal IDContatore As Long, ByVal IDUtente As Long) As Long
    '    Dim sSQL As String
    '    Dim DrTemp as sqldatareader=nothing
    '    Dim lngLetturaPrecedente As Long = -1

    '    sSQL = "SELECT TOP 1 LETTURA,DATALETTURA "
    '    sSQL += "FROM TP_LETTURE "
    '    sSQL += " WHERE "
    '    sSQL += "CODCONTATORE=" & IDContatore
    '    'sSQL += " AND  CODUTENTE=" & IDUtente
    '    sSQL += " AND  (STORICIZZATA=0 OR STORICIZZATA IS NULL)"
    '    sSQL += " AND  DATALETTURA < " & Utility.StringOperation.FormatString(DataLetturaAttuale)
    '    sSQL += " ORDER BY DATALETTURA DESC"
    '    DrTemp = iDB.getdataview(sSQL)
    '    Try
    '        If DrTemp.Read Then
    '            If Not IsDBNull(DrTemp("lettura")) Then
    '                lngLetturaPrecedente = CLng(DrTemp("lettura"))
    '            End If
    '        End If
    '        getLetturaPrecedente = lngLetturaPrecedente
    '        DrTemp.Close()
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.getLetturaPrecedente.errore: ", Err)
    '    End Try
    'End Function

    '*************************************************
    'VERIFICA SE ESISTE UNA DATA PRECEDENTE
    '*************************************************
    'Public Function getDataPrecedente(ByVal DataLetturaAttuale As String, ByVal IDContatore As Long, ByVal IDUtente As Long) As String
    '    Dim sSQL As String
    '    Dim DrTemp as sqldatareader=nothing
    '    Dim dataLetturaprecedente As String = ""

    '    getDataPrecedente = ""

    '    sSQL = "SELECT TOP 1 LETTURA,DATALETTURA"
    '    sSQL += " FROM TP_LETTURE"
    '    sSQL += " WHERE"
    '    sSQL += " CODCONTATORE=" & IDContatore
    '    'sSQL += " AND CODUTENTE=" & IDUtente
    '    sSQL += " AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)"
    '    sSQL += " AND DATALETTURA < " & Utility.StringOperation.FormatString(DataLetturaAttuale)
    '    sSQL += " ORDER BY DATALETTURA DESC"
    '    DrTemp = iDB.getdataview(sSQL)
    '    Try
    '        If DrTemp.Read Then
    '            If Not IsDBNull(DrTemp("datalettura")) Then
    '                dataLetturaprecedente = CStr(DrTemp("datalettura"))
    '            End If
    '        End If
    '        DrTemp.Close()

    '        getDataPrecedente = dataLetturaprecedente
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.getDataPrecedente.errore: ", Err)
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lngGiorniDiConsumo"></param>
    ''' <param name="IDContatore"></param>
    ''' <param name="IDUtente"></param>
    ''' <param name="myStringConnection"></param>
    ''' <returns></returns>
    Private Function CalcolaConsumoTeorico(ByVal lngGiorniDiConsumo As Long, ByVal IDContatore As Long, ByVal IDUtente As Long, ByVal myStringConnection As String) As Integer
        Dim lngRecordCount, lngConsumoTeorico As Integer
        Dim dblConsumoTeorico, dblResult, dblMediaConsumo, dblRapportoCGG As Double
        Dim dvMyDati As New DataView
        lngConsumoTeorico = 0
        Try
            dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, myStringConnection, 5, IDContatore, "", "")
            For Each myRow As DataRowView In dvMyDati
                If StringOperation.FormatInt(myRow("GIORNIDICONSUMO")) = 0 Then
                    'Giorni di Consumo =0 situazione anomala
                    dblRapportoCGG = 0
                Else
                    dblRapportoCGG = StringOperation.FormatInt(myRow("CONSUMO")) / StringOperation.FormatInt(myRow("GIORNIDICONSUMO"))
                End If
                dblResult += dblRapportoCGG
                lngRecordCount += 1
            Next

            If lngRecordCount > 0 Then
                Try
                    dblMediaConsumo = dblResult / lngRecordCount
                Catch ex As Exception When lngRecordCount = 0

                Finally
                    dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
                End Try
                'Approssimo per eccesso dblConsumoTeorico
                lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.CalcolaConsumoTeorico.errore: ", Err)
            lngConsumoTeorico = 0
        Finally
            dvMyDati.Dispose()
        End Try
        Return lngConsumoTeorico
    End Function
    'Private Function CalcolaConsumoTeorico(ByVal lngGiorniDiConsumo As Long, ByVal IDContatore As Long, ByVal IDUtente As Long, ByVal conn As SqlConnection) As Long

    '    Dim lngRecordCount, lngConsumoTeorico As Long

    '    Dim drConsumo as sqldatareader=nothing
    '    Dim dblConsumoTeorico, dblResult, dblMediaConsumo, dblRapportoCGG As Double
    '    Dim cmdConsumo As New SqlCommand

    '    Dim sSQL As String = ""

    '    sSQL = GetConsumoTeorico(IDContatore, IDUtente)

    '    cmdConsumo.CommandType = CommandType.Text
    '    cmdConsumo.CommandText = sSQL
    '    cmdConsumo.Connection = conn
    '    drConsumo = cmdConsumo.ExecuteReader()
    '    Try
    '        While drConsumo.Read

    '            dblRapportoCGG = utility.stringoperation.formatint(drConsumo.Item("CONSUMO")) / utility.stringoperation.formatint(drConsumo.Item("GIORNIDICONSUMO"))
    '            If utility.stringoperation.formatint(drConsumo.Item("GIORNIDICONSUMO")) = 0 Then
    '                'Giorni di Consumo =0 situazione anomala
    '                dblRapportoCGG = 0
    '            End If
    '            dblResult = dblResult + dblRapportoCGG
    '            lngRecordCount = lngRecordCount + 1

    '        End While

    '        If lngRecordCount > 0 Then
    '            Try
    '                dblMediaConsumo = dblResult / lngRecordCount
    '            Catch ex As Exception When lngRecordCount = 0

    '            Finally
    '                dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
    '            End Try
    '            'Approssimo per eccesso dblConsumoTeorico
    '            lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)


    '        End If
    '        CalcolaConsumoTeorico = lngConsumoTeorico
    '        drConsumo.Close()
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.CalcolaConsumoTeorico.errore: ", Err)
    '    End Try
    'End Function

    Protected Function CalcolaLetturaTeorica(ByVal lngLetturaPrecedente As Long, ByVal lngConsumoTeorico As Long, ByVal DetailContatore As objContatore) As Long

        Dim strValoreFondoScala As String = ""

        Dim lngValoreFondoScala, lngCifreContatore As Long
        Dim lngCount As Long

        Try
            CalcolaLetturaTeorica = lngLetturaPrecedente + lngConsumoTeorico

            If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
                lngCifreContatore = utility.stringoperation.formatint(DetailContatore.sCifreContatore)
                For lngCount = 1 To lngCifreContatore
                    strValoreFondoScala = strValoreFondoScala & "9"
                Next
                lngValoreFondoScala = utility.stringoperation.formatint(strValoreFondoScala)
                If CalcolaLetturaTeorica > lngValoreFondoScala Then
                    CalcolaLetturaTeorica = CalcolaLetturaTeorica - lngValoreFondoScala
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.CalcolaLetturaTeorica.errore: ", Err)
        End Try
    End Function

    Protected Function ApprossimaNumero(ByVal dblNumber As Double) As Long
        ApprossimaNumero = System.Math.Ceiling(dblNumber)
    End Function

    Private Function GetConsumoTeorico(ByVal IDContatore As Long, ByVal IDUtente As Long) As String

        Dim sSQL As String = ""
        GetConsumoTeorico = ""

        sSQL = "SELECT TOP 5 TP_LETTURE.*  FROM TP_LETTURE " & vbCrLf
        sSQL += "WHERE" & vbCrLf
        sSQL += "CODCONTATORE=" & IDContatore & vbCrLf
        sSQL += "AND" & vbCrLf
        sSQL += "CODUTENTE=" & IDUtente & vbCrLf
        sSQL += "AND" & vbCrLf
        sSQL += "PRIMALETTURA IS NULL" & vbCrLf
        sSQL += "AND" & vbCrLf
        sSQL += "(STORICIZZATA=0 OR STORICIZZATA IS NULL)" & vbCrLf
        sSQL += "AND" & vbCrLf
        sSQL += "(INCONGRUENTEFORZATO IS NULL  OR INCONGRUENTEFORZATO =0)" & vbCrLf
        sSQL += "AND" & vbCrLf
        sSQL += "(DATADIPASSAGGIO IS NULL OR DATADIPASSAGGIO='')" & vbCrLf
        sSQL += "ORDER BY DATALETTURA DESC"

        GetConsumoTeorico = sSQL

    End Function

    Public Function SpostaFile(ByVal fileName As String, ByVal percorsodestinazione As String) As String
        Log.Debug("Funzione SpostaFile di Utility")
        Dim nomefilespostato As String = String.Empty

        Try
            Utility.Costanti.CreateDir(percorsodestinazione)
            Dim data As String = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
            Dim oraminuti As String = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
            Dim infoFile As FileInfo = New FileInfo(fileName)
            Dim nomeFile As String = infoFile.Name
            Dim estensione As String = infoFile.Extension
            Dim nomeFileSenzaEstensione As String = nomeFile.Substring(0, (nomeFile.Length - estensione.Length))
            nomefilespostato = (percorsodestinazione + nomeFileSenzaEstensione + data + "-" + oraminuti + estensione)
            File.Move(fileName, nomefilespostato)
            Log.Debug("File spostato: " + (percorsodestinazione + nomeFile + "_" + oraminuti + estensione))
        Catch ex As Exception
            Log.Error("On SpostaFile", ex)
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.SpostaFile.errore: ", ex)
        End Try

        Return nomefilespostato

    End Function

    Private Function SetLetture(ByVal oDatiLettura As ObjImportazione.DatiLetture) As Boolean
        Dim sSQL As String

        Try
            sSQL = "INSERT INTO TP_LETTURE"
            sSQL += " (CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA,"
            sSQL += " CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE,"
            sSQL += " FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE,"
            sSQL += " STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)"
            sSQL += " VALUES ( "
            sSQL += oDatiLettura.CodContatore & ","
            sSQL += oDatiLettura.CodPeriodo & ","
            sSQL += Utility.StringOperation.FormatString(oDatiLettura.DataLettura) & ","
            sSQL += Utility.StringOperation.FormatInt(oDatiLettura.Lettura) & ","
            sSQL += Utility.StringOperation.FormatInt(oDatiLettura.Lettura) & ","
            sSQL += Utility.StringOperation.FormatInt(oDatiLettura.Consumo) & ","
            sSQL += Utility.StringOperation.FormatString(oDatiLettura.sNote) & ","
            sSQL += Utility.StringOperation.FormatString(oDatiLettura.GiornidiConsumo) & ","
            sSQL += Utility.StringOperation.FormatString(oDatiLettura.Consumo) & ","
            sSQL += oDatiLettura.CodUtente & "," & oDatiLettura.Fatturazione & ","
            sSQL += " 0,0,0,1,"
            sSQL += Utility.StringOperation.FormatString(oDatiLettura.DataLetturaPrecedente) & ","
            sSQL += oDatiLettura.LetturaPrecedente & ","
            sSQL += " Null,"
            If oDatiLettura.Cod_anomalia1 = -1 Then
                sSQL += " Null,"
            Else
                sSQL += oDatiLettura.Cod_anomalia1 & ","
            End If
            If oDatiLettura.Cod_anomalia2 = -1 Then
                sSQL += " Null,"
            Else
                sSQL += oDatiLettura.Cod_anomalia2 & ","
            End If
            If oDatiLettura.Cod_anomalia3 = -1 Then
                sSQL += " Null,"
            Else
                sSQL += oDatiLettura.Cod_anomalia3 & ","
            End If
            sSQL += Utility.StringOperation.FormatString(oReplace.GiraData(oDatiLettura.DataDiPassaggio)) & ","
            sSQL += " 0,'" & oDatiLettura.Provenienza & "')"
            iDB.ExecuteNonQuery(sSQL)
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsImport.SetLetture.errore: ", Err)
            Return False
        End Try
    End Function
    'Private Function SetLetture(ByVal oDatiLettura As ObjImportazione.DatiLetture, ByVal oMyConn As SqlConnection, ByVal oMyCommand As SqlCommand) As Boolean
    '    Dim sSQL As String

    '    Try
    '        sSQL = "INSERT INTO TP_LETTURE"
    '        sSQL += " (CODCONTATORE,CODPERIODO,DATALETTURA,LETTURA,LETTURATEORICA,"
    '        sSQL += " CONSUMO, NOTE, GIORNIDICONSUMO,CONSUMOTEORICO,CODUTENTE,"
    '        sSQL += " FATTURAZIONE,FATTURAZIONESOSPESA,INCONGRUENTEFORZATO,GIROCONTATORE,"
    '        sSQL += " STORICA,DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,DATADIPASSAGGIO, STORICIZZATA, PROVENIENZA)"
    '        sSQL += " VALUES ( "
    '        sSQL += oDatiLettura.CodContatore & ","
    '        sSQL += oDatiLettura.CodPeriodo & ","
    '        sSQL += Utility.StringOperation.FormatString(oDatiLettura.DataLettura) & ","
    '        sSQL += utility.stringoperation.formatint(oDatiLettura.Lettura) & ","
    '        sSQL += utility.stringoperation.formatint(oDatiLettura.Lettura) & ","
    '        sSQL += utility.stringoperation.formatint(oDatiLettura.Consumo) & ","
    '        sSQL += Utility.StringOperation.FormatString(oDatiLettura.sNote) & ","
    '        sSQL += Utility.StringOperation.FormatString(oDatiLettura.GiornidiConsumo) & ","
    '        sSQL += Utility.StringOperation.FormatString(oDatiLettura.Consumo) & ","
    '        sSQL += oDatiLettura.CodUtente & "," & oDatiLettura.Fatturazione & ","
    '        sSQL += " 0,0,0,1,"
    '        sSQL += Utility.StringOperation.FormatString(oDatiLettura.DataLetturaPrecedente) & ","
    '        sSQL += oDatiLettura.LetturaPrecedente & ","
    '        sSQL += " Null,"
    '        If oDatiLettura.Cod_anomalia1 = -1 Then
    '            sSQL += " Null,"
    '        Else
    '            sSQL += oDatiLettura.Cod_anomalia1 & ","
    '        End If
    '        If oDatiLettura.Cod_anomalia2 = -1 Then
    '            sSQL += " Null,"
    '        Else
    '            sSQL += oDatiLettura.Cod_anomalia2 & ","
    '        End If
    '        If oDatiLettura.Cod_anomalia3 = -1 Then
    '            sSQL += " Null,"
    '        Else
    '            sSQL += oDatiLettura.Cod_anomalia3 & ","
    '        End If
    '        sSQL += Utility.StringOperation.FormatString(oReplace.GiraData(oDatiLettura.DataDiPassaggio)) & ","
    '        sSQL += " 0,'" & oDatiLettura.Provenienza & "')"
    '        oMyCommand = New SqlCommand(sSQL, oMyConn)
    '        oMyCommand.ExecuteNonQuery()
    '        Return True
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsImport.SetLetture.errore: ", Err)
    '        Return False
    '    End Try
    'End Function
End Class
