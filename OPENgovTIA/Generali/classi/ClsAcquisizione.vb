Imports log4net
Imports OPENUtility
''' <summary>
''' Definizione oggetto riepilogo acquisizione flussi
''' </summary>
Public Class ObjTotAcquisizione
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjTotAcquisizione))
    Private oReplace As New generalClass.generalFunction
    Private _Id As Integer = -1
    Private _IdEnte As String = ""
    Private _sFileAcq As String = ""
    Private _nStatoAcq As Integer = -1
    Private _sEsito As String = ""
    Private _sFileScarti As String = ""
    Private _nRcFile As Integer = 0
    Private _nTessereFile As Integer = 0
    Private _nTessereImport As Integer = 0
    Private _nLitriImport As Double = 0
    Private _nRcImport As Integer = 0
    Private _nRcScarti As Integer = 0
    Private _tDataAcq As Date = Date.MinValue
    Private _sOperatore As String = ""
    Private _nConferimentiImport As Integer = 0

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
    Public Property nTessereFile() As Integer
        Get
            Return _nTessereFile
        End Get
        Set(ByVal Value As Integer)
            _nTessereFile = Value
        End Set
    End Property
    Public Property nTessereImport() As Integer
        Get
            Return _nTessereImport
        End Get
        Set(ByVal Value As Integer)
            _nTessereImport = Value
        End Set
    End Property
    Public Property nLitriImport() As Double
        Get
            Return _nLitriImport
        End Get
        Set(ByVal Value As Double)
            _nLitriImport = Value
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
    Public Property sOperatore() As String
        Get
            Return _sOperatore
        End Get
        Set(ByVal Value As String)
            _sOperatore = Value
        End Set
    End Property
    Public Property nConferimentiImport() As Integer
        Get
            Return _nConferimentiImport
        End Get
        Set(ByVal Value As Integer)
            _nConferimentiImport = Value
        End Set
    End Property

    'Public Function GetAcquisizione(ByVal WFSessione As CreateSessione, ByVal nStato As Integer, ByVal sEnte As String) As ObjTotAcquisizione
    '    'STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}
    '    Try
    '        dim sSQL as string
    '        Dim DrReturn As SqlClient.SqlDataReader
    '        Dim oTotAcq As New ObjTotAcquisizione

    '        sSQL = "SELECT TOP 1 *"
    '        sSQL += " FROM TBLIMPORTAZIONE"
    '        sSQL += " WHERE (IDENTE ='" & sEnte & "')"
    '        sSQL += " ORDER BY ID DESC"
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While DrReturn.Read
    '            If CInt(DrReturn("stato_importazione")) = nStato Or nStato <> 1 Then
    '                oTotAcq.Id = CInt(DrReturn("id"))
    '                oTotAcq.IdEnte = CStr(DrReturn("idente"))
    '                oTotAcq.sFileAcq = CStr(DrReturn("file_import"))
    '                oTotAcq.nStatoAcq = CInt(DrReturn("stato_importazione"))
    '                oTotAcq.sEsito = CStr(DrReturn("esito"))
    '                oTotAcq.sFileScarti = CStr(DrReturn("file_scarti"))
    '                oTotAcq.nRcFile = CInt(DrReturn("rc_file"))
    '                oTotAcq.nTessereFile = CInt(DrReturn("ntessere_file"))
    '                oTotAcq.nTessereImport = CInt(DrReturn("ntessere_import"))
    '                oTotAcq.nKgImport = CDbl(DrReturn("kg_import"))
    '                oTotAcq.nRcImport = CInt(DrReturn("rc_import"))
    '                oTotAcq.nRcScarti = CInt(DrReturn("rc_scartati"))
    '                oTotAcq.tDataAcq = CDate(DrReturn("data_import"))
    '                oTotAcq.sOperatore = CStr(DrReturn("operatore"))
    '            End If
    '        Loop
    '        DrReturn.Close()

    '        Return oTotAcq
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.GetAcquizione.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function SetAcquisizione(ByVal oTotAcq As ObjTotAcquisizione, ByVal DbOperation As Integer, ByVal WFSessione As CreateSessione) As Integer
    '    Try
    '        dim sSQL as string
    '        Dim myIdentity As Integer

    '        Select Case DbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TBLIMPORTAZIONE (IDENTE,FILE_IMPORT,STATO_IMPORTAZIONE,ESITO,FILE_SCARTI,"
    '                sSQL += "NTESSERE_FILE,RC_FILE,NTESSERE_IMPORT,KG_IMPORT,RC_IMPORT,RC_SCARTATI,DATA_IMPORT,OPERATORE)"
    '                sSQL += " VALUES ('" & oTotAcq.IdEnte & "','" & oReplace.ReplaceChar(oTotAcq.sFileAcq) & "'," & oTotAcq.nStatoAcq & ",'" & oReplace.ReplaceChar(oTotAcq.sEsito) & "','" & oReplace.ReplaceChar(oTotAcq.sFileScarti) & "',"
    '                sSQL += oTotAcq.nTessereFile & "," & oTotAcq.nRcFile & "," & oTotAcq.nTessereImport & "," & oReplace.ReplaceNumberForDB(oTotAcq.nKgImport) & "," & oTotAcq.nRcImport & "," & oTotAcq.nRcScarti & ","
    '                sSQL += "'" & oReplace.ReplaceDataForDB(oTotAcq.tDataAcq) & "','" & oReplace.ReplaceChar(oTotAcq.sOperatore) & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case 1
    '                sSQL = "UPDATE TBLIMPORTAZIONE SET IDENTE='" & oTotAcq.IdEnte & "',"
    '                sSQL += " FILE_IMPORT='" & oReplace.ReplaceChar(oTotAcq.sFileAcq) & "',"
    '                sSQL += " STATO_IMPORTAZIONE=" & oTotAcq.nStatoAcq & ","
    '                sSQL += " ESITO='" & oReplace.ReplaceChar(oTotAcq.sEsito) & "',"
    '                sSQL += " FILE_SCARTI='" & oReplace.ReplaceChar(oTotAcq.sFileScarti) & "',"
    '                sSQL += " RC_FILE=" & oTotAcq.nRcFile & ","
    '                sSQL += " NTESSERE_FILE=" & oTotAcq.nTessereFile & ","
    '                sSQL += " NTESSERE_IMPORT=" & oTotAcq.nTessereImport & ","
    '                sSQL += " KG_IMPORT=" & oReplace.ReplaceNumberForDB(oTotAcq.nKgImport) & ","
    '                sSQL += " RC_IMPORT=" & oTotAcq.nRcImport & ","
    '                sSQL += " RC_SCARTATI=" & oTotAcq.nRcScarti & ","
    '                sSQL += " DATA_IMPORT='" & oReplace.ReplaceDataForDB(oTotAcq.tDataAcq) & "',"
    '                sSQL += " OPERATORE='" & oReplace.ReplaceChar(oTotAcq.sOperatore) & "'"
    '                sSQL += " WHERE (ID= " & oTotAcq.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oTotAcq.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TBLIMPORTAZIONE"
    '                sSQL += " WHERE (ID= " & oTotAcq.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oTotAcq.Id
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.SetAcquizione.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function MaxIdImport(ByVal sIdEnte As String, ByVal WFSessione As CreateSessione) As Integer
    '    Try
    '        dim sSQL as string
    '        Dim DrReturn As SqlClient.SqlDataReader
    '        Dim myIdentity As Integer = -1

    '        sSQL = "SELECT MAX(ID) AS MAXID"
    '        sSQL += " FROM TBLIMPORTAZIONE"
    '        sSQL += " WHERE (IDENTE='" & sIdEnte & "')"
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While DrReturn.Read
    '            myIdentity = CInt(DrReturn("maxid"))
    '        Loop
    '        DrReturn.Close()

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.MaxIdImport.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function
    'Public Function GetAcquisizione(ByVal myConnectionString As String, ByVal nStato As Integer, ByVal sEnte As String) As ObjTotAcquisizione
    '    'STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Dim oTotAcq As New ObjTotAcquisizione
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = "SELECT TOP 1 *"
    '        cmdMyCommand.CommandText += " FROM TBLIMPORTAZIONE"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE ='" & sEnte & "')"
    '        cmdMyCommand.CommandText += " ORDER BY ID DESC"
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        Do While DrReturn.Read
    '            If CInt(DrReturn("stato_importazione")) = nStato Or nStato <> 1 Then
    '                oTotAcq.Id = CInt(DrReturn("id"))
    '                oTotAcq.IdEnte = CStr(DrReturn("idente"))
    '                oTotAcq.sFileAcq = CStr(DrReturn("file_import"))
    '                oTotAcq.nStatoAcq = CInt(DrReturn("stato_importazione"))
    '                oTotAcq.sEsito = CStr(DrReturn("esito"))
    '                oTotAcq.sFileScarti = CStr(DrReturn("file_scarti"))
    '                oTotAcq.nRcFile = CInt(DrReturn("rc_file"))
    '                oTotAcq.nTessereFile = CInt(DrReturn("ntessere_file"))
    '                oTotAcq.nTessereImport = CInt(DrReturn("ntessere_import"))
    '                oTotAcq.nConferimentiImport = CInt(DrReturn("Conferimenti_Import"))
    '                oTotAcq.nLitriImport = CDbl(DrReturn("kg_import"))
    '                oTotAcq.nRcImport = CInt(DrReturn("rc_import"))
    '                oTotAcq.nRcScarti = CInt(DrReturn("rc_scartati"))
    '                oTotAcq.tDataAcq = CDate(DrReturn("data_import"))
    '                oTotAcq.sOperatore = CStr(DrReturn("operatore"))
    '            End If
    '        Loop

    '        Return oTotAcq
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.GetAcquizione.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    Public Function GetAcquisizione(ByVal myConnectionString As String, ByVal nStato As Integer, ByVal sEnte As String) As ObjTotAcquisizione
        'STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}
        Dim DrReturn As SqlClient.SqlDataReader = Nothing
        Dim oTotAcq As New ObjTotAcquisizione
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetImportPesature"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sEnte
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                If CInt(DrReturn("stato_importazione")) = nStato Or nStato <> 1 Then
                    oTotAcq.Id = CInt(DrReturn("id"))
                    oTotAcq.IdEnte = CStr(DrReturn("idente"))
                    oTotAcq.sFileAcq = CStr(DrReturn("file_import"))
                    oTotAcq.nStatoAcq = CInt(DrReturn("stato_importazione"))
                    oTotAcq.sEsito = CStr(DrReturn("esito"))
                    oTotAcq.sFileScarti = CStr(DrReturn("file_scarti"))
                    oTotAcq.nRcFile = CInt(DrReturn("rc_file"))
                    oTotAcq.nTessereFile = CInt(DrReturn("ntessere_file"))
                    oTotAcq.nTessereImport = CInt(DrReturn("ntessere_import"))
                    oTotAcq.nConferimentiImport = CInt(DrReturn("Conferimenti_Import"))
                    oTotAcq.nLitriImport = CDbl(DrReturn("kg_import"))
                    oTotAcq.nRcImport = CInt(DrReturn("rc_import"))
                    oTotAcq.nRcScarti = CInt(DrReturn("rc_scartati"))
                    oTotAcq.tDataAcq = CDate(DrReturn("data_import"))
                    oTotAcq.sOperatore = CStr(DrReturn("operatore"))
                End If
            Loop

            Return oTotAcq
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.GetAcquizione.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            DrReturn.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function SetAcquisizione(ByVal myConnectionString As String, ByVal oTotAcq As ObjTotAcquisizione, ByVal DbOperation As Integer) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myIdentity As Integer

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text

            Select Case DbOperation
                Case 0
                    cmdMyCommand.CommandText = "INSERT INTO TBLIMPORTAZIONE (IDENTE,FILE_IMPORT,STATO_IMPORTAZIONE,ESITO,FILE_SCARTI,"
                    cmdMyCommand.CommandText += "NTESSERE_FILE,RC_FILE,NTESSERE_IMPORT, CONFERIMENTI_IMPORT,KG_IMPORT,RC_IMPORT,RC_SCARTATI,DATA_IMPORT,OPERATORE)"
                    cmdMyCommand.CommandText += " VALUES ('" & oTotAcq.IdEnte & "','" & oReplace.ReplaceChar(oTotAcq.sFileAcq) & "'," & oTotAcq.nStatoAcq & ",'" & oReplace.ReplaceChar(oTotAcq.sEsito) & "','" & oReplace.ReplaceChar(oTotAcq.sFileScarti) & "',"
                    cmdMyCommand.CommandText += oTotAcq.nTessereFile & "," & oTotAcq.nRcFile & "," & oTotAcq.nTessereImport & "," & oReplace.ReplaceNumberForDB(oTotAcq.nConferimentiImport) & "," & oReplace.ReplaceNumberForDB(oTotAcq.nLitriImport) & "," & oTotAcq.nRcImport & "," & oTotAcq.nRcScarti & ","
                    cmdMyCommand.CommandText += "'" & oReplace.ReplaceDataForDB(oTotAcq.tDataAcq) & "','" & oReplace.ReplaceChar(oTotAcq.sOperatore) & "')"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim DrReturn As SqlClient.SqlDataReader
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    DrReturn = cmdMyCommand.ExecuteReader
                    Do While DrReturn.Read
                        myIdentity = DrReturn(0)
                    Loop
                    DrReturn.Close()
                Case 1
                    cmdMyCommand.CommandText = "UPDATE TBLIMPORTAZIONE SET IDENTE='" & oTotAcq.IdEnte & "',"
                    cmdMyCommand.CommandText += " FILE_IMPORT='" & oReplace.ReplaceChar(oTotAcq.sFileAcq) & "',"
                    cmdMyCommand.CommandText += " STATO_IMPORTAZIONE=" & oTotAcq.nStatoAcq & ","
                    cmdMyCommand.CommandText += " ESITO='" & oReplace.ReplaceChar(oTotAcq.sEsito) & "',"
                    cmdMyCommand.CommandText += " FILE_SCARTI='" & oReplace.ReplaceChar(oTotAcq.sFileScarti) & "',"
                    cmdMyCommand.CommandText += " RC_FILE=" & oTotAcq.nRcFile & ","
                    cmdMyCommand.CommandText += " NTESSERE_FILE=" & oTotAcq.nTessereFile & ","
                    cmdMyCommand.CommandText += " NTESSERE_IMPORT=" & oTotAcq.nTessereImport & ","
                    cmdMyCommand.CommandText += " CONFERIMENTI_IMPORT=" & oTotAcq.nConferimentiImport & ","
                    cmdMyCommand.CommandText += " KG_IMPORT=" & oReplace.ReplaceNumberForDB(oTotAcq.nLitriImport) & ","
                    cmdMyCommand.CommandText += " RC_IMPORT=" & oTotAcq.nRcImport & ","
                    cmdMyCommand.CommandText += " RC_SCARTATI=" & oTotAcq.nRcScarti & ","
                    cmdMyCommand.CommandText += " DATA_IMPORT='" & oReplace.ReplaceDataForDB(oTotAcq.tDataAcq) & "',"
                    cmdMyCommand.CommandText += " OPERATORE='" & oReplace.ReplaceChar(oTotAcq.sOperatore) & "'"
                    cmdMyCommand.CommandText += " WHERE (ID= " & oTotAcq.Id & ")"
                    'eseguo la query
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    If cmdMyCommand.ExecuteNonQuery <> 1 Then
                        Return 0
                    End If
                    myIdentity = oTotAcq.Id
                Case 2
                    cmdMyCommand.CommandText = "DELETE"
                    cmdMyCommand.CommandText += " FROM TBLIMPORTAZIONE"
                    cmdMyCommand.CommandText += " WHERE (ID= " & oTotAcq.Id & ")"
                    'eseguo la query
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    If cmdMyCommand.ExecuteNonQuery <> 1 Then
                        Return 0
                    End If
                    myIdentity = oTotAcq.Id
            End Select
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.SetAcquizione.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function MaxIdImport(ByVal myConnectionString As String, ByVal sIdEnte As String) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim DrReturn As SqlClient.SqlDataReader = Nothing
        Dim myIdentity As Integer = -1

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT MAX(ID) AS MAXID"
            cmdMyCommand.CommandText += " FROM TBLIMPORTAZIONE"
            cmdMyCommand.CommandText += " WHERE (IDENTE='" & sIdEnte & "')"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                myIdentity = CInt(DrReturn("maxid"))
            Loop

            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTotAcquisizione.MaxIdImport.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            DrReturn.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
End Class
