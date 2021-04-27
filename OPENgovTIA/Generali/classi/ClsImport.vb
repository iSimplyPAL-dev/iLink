Imports System.IO
Imports log4net
Imports OPENUtility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Classe importazione flussi pesature
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsImport
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsImport))
    Private oReplace As New generalClass.generalFunction

    Public Sub New()
        ' Costruttore della classe ClsImport
    End Sub

    Public Sub StartImport(DBType As String, ByVal myConnectionString As String, ByVal sUserEnv As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdFlussoImport As Integer)
        Dim FunctionImport As New ObjTotAcquisizione
        Dim oMyTotAcq As New ObjTotAcquisizione
        Dim FunctionPesature As New ObjPesatura
        Dim nCheckFile As Integer
        Dim sErrCheckFile As String = ""
        Dim sNameCampoTessera, sNameCampoData, sNameCampoVolume As String

        Try
            sNameCampoTessera = "" : sNameCampoData = "" : sNameCampoVolume = ""
            Dim oMyFileInfo As New System.IO.FileInfo(sFileImport)
            Dim sNameImport As String = oMyFileInfo.Name

            'controllo che il formato sia corretto
            Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.controllo che il formato sia corretto")
            nCheckFile = CheckFilePesature(sEnteImport, sFileImport, sNameImport, sNameCampoTessera, sNameCampoData, sNameCampoVolume, sErrCheckFile)
            Select Case nCheckFile
                Case -1 'errore
                    Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.nCheckFile=-1.Errore nei controlli formali di Acquisizione.")
                    'sposto il file nella cartella non acquisiti
                    System.IO.File.Copy(sFileImport, ConstSession.PathSpostaNoImport + sNameImport)
                    System.IO.File.Delete(sFileImport)
                    'registro l'errore acquisizione
                    oMyTotAcq.Id = nIdFlussoImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione." + vbCrLf + sErrCheckFile
                    oMyTotAcq.tDataAcq = Now
                    FunctionImport.SetAcquisizione(myConnectionString, oMyTotAcq, Utility.Costanti.AZIONE_UPDATE)
                Case 0 'formato non corretto
                    Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.nCheckFile=0.Formato di Acquisizione non valido.")
                    'sposto il file nella cartella non acquisiti
                    System.IO.File.Copy(sFileImport, ConstSession.PathSpostaNoImport + sNameImport)
                    System.IO.File.Delete(sFileImport)
                    'registro il formato non corretto di acquisizione
                    oMyTotAcq.Id = nIdFlussoImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido." + vbCrLf + sErrCheckFile
                    oMyTotAcq.tDataAcq = Now
                    FunctionImport.SetAcquisizione(myConnectionString, oMyTotAcq, Utility.Costanti.AZIONE_UPDATE)
                Case 1 'formato corretto
                    Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.Acquisizione.")
                    If AcquisisciPesature(DBType, myConnectionString, sEnteImport, sFileImport, nIdFlussoImport, sNameImport.ToLower.Substring(sEnteImport.Length + 1, 7), sUserEnv, sNameCampoTessera, sNameCampoData, sNameCampoVolume, oMyTotAcq) <= 0 Then
                        Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.Errore in importazione.")
                        'sposto il file nella cartella non acquisiti
                        System.IO.File.Copy(sFileImport, ConstSession.PathSpostaNoImport + sNameImport)
                        System.IO.File.Delete(sFileImport)
                        'registro l'errore acquisizione
                        oMyTotAcq.Id = nIdFlussoImport
                        oMyTotAcq.IdEnte = sEnteImport
                        oMyTotAcq.sFileAcq = sFileImport
                        oMyTotAcq.nStatoAcq = -1
                        oMyTotAcq.sEsito = "Errore durante l'importazione."
                        oMyTotAcq.tDataAcq = Now
                        FunctionImport.SetAcquisizione(myConnectionString, oMyTotAcq, Utility.Costanti.AZIONE_UPDATE)
                    Else
                        Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.Acquisizione terminata con successo.")
                        'sposto il file nella cartella acquisiti
                        System.IO.File.Copy(sFileImport, ConstSession.PathSpostaImport + sNameImport)
                        System.IO.File.Delete(sFileImport)
                        'registro l'avvenuta acquisizione
                        oMyTotAcq.Id = nIdFlussoImport
                        oMyTotAcq.IdEnte = sEnteImport
                        oMyTotAcq.sFileAcq = sFileImport
                        oMyTotAcq.nStatoAcq = 0
                        oMyTotAcq.sEsito = "Acquisizione terminata con successo!"
                        oMyTotAcq.tDataAcq = Now
                        FunctionImport.SetAcquisizione(myConnectionString, oMyTotAcq, Utility.Costanti.AZIONE_UPDATE)
                    End If
                Case 2 'dati obbligatori mancanti
                    Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.nCheckFile=2. Dati obbligatori mancanti.")
                    'sposto il file nella cartella non acquisiti
                    System.IO.File.Copy(sFileImport, ConstSession.PathSpostaNoImport + sNameImport)
                    System.IO.File.Delete(sFileImport)
                    'registro la mancanza di dati obbligatori
                    oMyTotAcq.Id = nIdFlussoImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Dati obbligatori mancanti." + vbCrLf + sErrCheckFile
                    oMyTotAcq.tDataAcq = Now
                    FunctionImport.SetAcquisizione(myConnectionString, oMyTotAcq, Utility.Costanti.AZIONE_UPDATE)
            End Select

        Catch Err As Exception
            Log.Debug(sEnteImport + " - OPENgovTIA.ClsImport.StartImport.errore: ", Err)
            'registro l'errore acquisizione
            oMyTotAcq.Id = nIdFlussoImport
            oMyTotAcq.IdEnte = sEnteImport
            oMyTotAcq.sFileAcq = sFileImport
            oMyTotAcq.nStatoAcq = -1
            oMyTotAcq.sEsito = "Errore durante l'importazione."
            oMyTotAcq.tDataAcq = Now
            FunctionImport.SetAcquisizione(myConnectionString, oMyTotAcq, Utility.Costanti.AZIONE_UPDATE)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sEnteCheck"></param>
    ''' <param name="sPeriodoCheck">{1=periodo non presente ; 0= periodo presente; -1= errore}</param>
    ''' <returns></returns>
    Private Function CheckPeriodoPesature(ByVal myConnectionString As String, ByVal sEnteCheck As String, ByVal sPeriodoCheck As String) As Integer
        '
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim DrReturn As SqlClient.SqlDataReader = Nothing

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT DISTINCT PERIODO"
            cmdMyCommand.CommandText += " FROM TBLPESATURE"
            cmdMyCommand.CommandText += " WHERE (IDENTE ='" & sEnteCheck & "') AND (PERIODO='" & sPeriodoCheck & "')"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            If DrReturn.Read Then
                Return 0
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsImport.CheckPeriodoPesature.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            DrReturn.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    Private Function AcquisisciPesature(DBType As String, ByVal myConnectionString As String, ByVal sEnteAcq As String, ByVal sFileAcq As String, ByVal nIdFlussoAcq As Integer, ByVal sPeriodoAcq As String, ByVal sMyOperatore As String, sNameCampoTessera As String, sNameCampoData As String, sNameCampoVolume As String, ByRef oTotAcq As ObjTotAcquisizione) As Integer
        Dim oMyPesatura As ObjPesatura
        Dim FncPesature As New GestPesatura
        Dim FncTessere As New Utility.DichManagerTARSU(DBType, myConnectionString, "", "") 'As New GestTessera
        Dim FncGest As New GestTessera
        Dim oMyTessera As New ObjTessera
        Dim sTesseraPrec As String = ""
        Dim bUpdateTessera As Boolean
        Dim oMyFileInfo As New System.IO.FileInfo(sFileAcq)
        Dim sNameImport As String = oMyFileInfo.Name
        Dim myCultureInfo As New System.Globalization.CultureInfo("it-IT", True)

        'apro il file
        Try
            Dim myLine As String
            Dim PostedFile() As Byte = System.IO.File.ReadAllBytes(sFileAcq)
            If Not PostedFile Is Nothing Then
                Dim MS As New MemoryStream(PostedFile, 0, PostedFile.Length)
                Dim MyFileToRead As New StreamReader(MS)
                Try
                    Do While MyFileToRead.Peek > 1
                        myLine = MyFileToRead.ReadLine()
                        Dim ListDati As String()
                        ListDati = myLine.Split(CChar(";"))
                        If ListDati(0) <> "CODICE TESSERA" Then
                            oTotAcq.nRcFile += 1 : bUpdateTessera = False
                            'prelevo i dati dal file
                            oMyPesatura = New ObjPesatura
                            oMyPesatura.IdEnte = sEnteAcq
                            oMyPesatura.IdFlusso = nIdFlussoAcq
                            oMyPesatura.tDataOraConferimento = DateTime.Parse(ListDati(2), myCultureInfo)
                            oMyPesatura.sNumeroTessera = ListDati(0)
                            '*** 201712 - gestione tipo conferimento ***
                            oMyPesatura.sTipoConferimento = ListDati(1)
                            '*** ***
                            oMyPesatura.nVolume = CDbl(ListDati(3))
                            oMyPesatura.tDataInserimento = Now
                            oMyPesatura.sOperatore = sMyOperatore

                            If oMyPesatura.sNumeroTessera <> sTesseraPrec Then
                                oTotAcq.nTessereFile += 1
                            End If

                            'cerco la tessera alla quale si riferisce il conferimento per N° TESSERA o CODICE INTERNO
                            oMyTessera = FncGest.AbbinaTessera(myConnectionString, oMyPesatura)
                            If oMyTessera Is Nothing Then
                                'se non ho trovato scarto
                                oTotAcq.nRcScarti += 1
                                oTotAcq.sFileScarti = ConstSession.PathSpostaImport + "SCARTI_" + sNameImport.ToLower.Replace(".csv", ".txt")
                                If WriteScartiPesature(oMyPesatura, sFileAcq, ConstSession.PathSpostaImport + "SCARTI_" + sNameImport.ToLower.Replace(".csv", ".txt")) = 0 Then
                                    Return -1
                                End If
                            Else
                                'inserisco il conferimento
                                oMyPesatura.IdTessera = oMyTessera.Id
                                If oMyTessera.IdEnte <> "" Then
                                    oMyPesatura.IdEnte = oMyTessera.IdEnte
                                End If
                                If FncPesature.SetPesatura(myConnectionString, oMyPesatura, Utility.Costanti.AZIONE_NEW) <= 0 Then
                                    Return -1
                                End If
                                'aggiorno gli eventuali dati presenti nel file e mancanti da db
                                If oMyTessera.sNumeroTessera = ObjTessera.TESSERA_BIDONE And oMyPesatura.sNumeroTessera <> ObjTessera.TESSERA_BIDONE Then
                                    oMyTessera.sNumeroTessera = oMyPesatura.sNumeroTessera
                                    bUpdateTessera = True
                                End If
                                If oMyTessera.sCodInterno = "" And oMyPesatura.sCodInterno <> "" Then
                                    oMyTessera.sCodInterno = oMyPesatura.sCodInterno
                                    bUpdateTessera = True
                                End If
                                If oMyTessera.sCodUtente = "" And oMyPesatura.sCodUtente <> "" Then
                                    oMyTessera.sCodUtente = oMyPesatura.sCodUtente
                                    bUpdateTessera = True
                                End If
                                If bUpdateTessera = True Then
                                    If FncTessere.SetTessera(Utility.Costanti.AZIONE_UPDATE, oMyTessera, oMyTessera.IdContribuente) < 1 Then
                                        Return -1
                                    End If
                                End If
                                oTotAcq.nConferimentiImport += 1
                                oTotAcq.nLitriImport += oMyPesatura.nLitri
                                oTotAcq.nRcImport += 1
                            End If
                            sTesseraPrec = oMyPesatura.sNumeroTessera
                        End If
                    Loop
                Catch ex As Exception
                    Log.Debug(sEnteAcq + " - OPENgovTIA.ClsImport.AcquisisciPesture.errore: ", ex)
                    Return 0
                Finally
                    MyFileToRead.Close()
                    MS.Close()
                End Try
            Else
                Log.Debug(sEnteAcq + " - OPENgovTIA.ClsImport.AcquisisciPesture.errore: Il file è vuoto.")
                Return 0
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(sEnteAcq + " - OPENgovTIA.ClsImport.AcquisisciPesture.errore: ", Err)
            Return -1
        End Try
    End Function
    Private Function CheckFilePesature(ByVal sEnteCheck As String, ByVal sFileCheck As String, ByVal sNameFileCheck As String, ByRef sNameCampoTessera As String, ByRef sNameCampoData As String, ByRef sNameCampoVolume As String, ByRef sErrCheck As String) As Integer
        '{1= formato corretto; 0= formato non corretto; 2= dati obbligatori mancanti; -1= errore}
        Dim myCultureInfo As New System.Globalization.CultureInfo("it-IT", True)

        Try
            Dim sMesi As String = "010203040506070809101112"
            'non è un file excel
            If Not sNameFileCheck.ToLower.EndsWith(".csv") Then
                Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè non è csv")
                sErrCheck = "Non è un file CSV."
                Return 0
            End If
            'non è dell’ente in esame
            If Not sNameFileCheck.ToLower.StartsWith(sEnteCheck) Then
                Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè ente sbagliato")
                sErrCheck = "L'ente del file non corrisponde con l'ente in esame."
                Return 0
            End If
            'non è un periodo valido
            If sMesi.IndexOf(sNameFileCheck.ToLower.Substring(sEnteCheck.Length + 1, 2)) < 0 Then
                Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè periodo no valido")
                sErrCheck = "Il periodo indicato non è valido."
                Return 0
            End If
            'controllo che la struttura sia corretta
            Try
                'CODICE TESSERA|TIPO CONFERIMENTO|DATA ORA|LITRI|NOTE
                Dim iRiga As Integer = 1
                Dim myLine As String
                Dim PostedFile() As Byte = System.IO.File.ReadAllBytes(sFileCheck)
                If Not PostedFile Is Nothing Then
                    Dim MS As New MemoryStream(PostedFile, 0, PostedFile.Length)
                    Dim MyFileToRead As New StreamReader(MS)
                    Try
                        Do While MyFileToRead.Peek > 1
                            myLine = MyFileToRead.ReadLine()
                            Try
                                If myLine = String.Empty Then
                                    Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè riga vuota. Riga " & iRiga)
                                    sErrCheck = "Presenza riga vuota. Riga " & iRiga
                                    Return 0
                                Else
                                    Dim ListDati As String()
                                    ListDati = myLine.Split(CChar(";"))
                                    If ListDati.GetUpperBound(0) < 4 Then
                                        Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè campi non coerenti. Riga " & iRiga)
                                        sErrCheck = "Numero di Campi non coerenti. Riga " & iRiga
                                        Return 0
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(0) <> "CODICE TESSERA" Then
                                            sErrCheck = "Manca il campo CODICE TESSERA. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(0) = "" Then
                                            Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè manca codice tessera. Riga " & iRiga)
                                            sErrCheck = "Codice Tessera mancante. Riga " & iRiga
                                            Return 2
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(1).ToUpper <> "TIPO CONFERIMENTO" Then
                                            sErrCheck = "Manca il campo TIPO CONFERIMENTO. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(1) = "" Then
                                            Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè manca tipo conferimento. Riga " & iRiga)
                                            sErrCheck = "Tipo Conferimento non valorizzato. Riga " & iRiga
                                            Return 2
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(2).ToUpper <> "DATA ORA" Then
                                            sErrCheck = "Manca il campo DATA ORA. Riga " & iRiga
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(2) = "" Then
                                            Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè manca data. Riga " & iRiga)
                                            sErrCheck = "Data non valorizzata. Riga " & iRiga
                                            Return 2
                                        Else
                                            Try
                                                If IsDate(DateTime.Parse(ListDati(2), myCultureInfo)) = False Then
                                                    sErrCheck = "Data non correttamente valorizzata. Riga " & iRiga & " valore letto " & DateTime.Parse(ListDati(2), myCultureInfo) & "."
                                                    Return 2
                                                End If
                                            Catch
                                                sErrCheck = "Data non correttamente valorizzata. Riga " & iRiga & " valore letto " & ListDati(2) & "."
                                                Return 2
                                            End Try
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(3).ToUpper <> "LITRI" Then
                                            sErrCheck = "Manca il campo LITRI."
                                            Return 0
                                        End If
                                    Else
                                        If ListDati(3).ToUpper = "" Then
                                            Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè manca litri. Riga " & iRiga)
                                            sErrCheck = "Litri non valorizzati. Riga " & iRiga
                                            Return 2
                                        Else
                                            If IsNumeric(ListDati(3)) = False Then
                                                Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè litri no corretti. Riga " & iRiga)
                                                sErrCheck = "Litri non correttamente valorizzati. Riga " & iRiga & " valore letto " & ListDati(3) & "."
                                                Return 2
                                            End If
                                        End If
                                    End If
                                    If iRiga = 1 Then
                                        If ListDati(4).ToUpper <> "NOTE" Then
                                            sErrCheck = "Manca il campo NOTE."
                                            Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè manca note. Riga " & iRiga)
                                            Return 0
                                        End If
                                    End If
                                End If
                            Catch ex As Exception
                                Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè errore lettura file->" + ex.Message)
                                sErrCheck = "Errore lettura file."
                                Return 0
                            End Try
                            iRiga += 1
                        Loop
                    Catch ex As Exception
                        Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè errore nel file->" + ex.Message)
                        sErrCheck = "Errore nel file."
                        Return 0
                    Finally
                        MyFileToRead.Close()
                        MS.Close()
                    End Try
                Else
                    Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.NO valido perchè file vuoto")
                    sErrCheck = "Il file è vuoto."
                    Return 0
                End If
                Return 1
            Catch Err As Exception
                Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.errore: ", Err)
                sErrCheck = Err.StackTrace
                Return -1
            End Try
        Catch Err As Exception
            Log.Debug(sEnteCheck + " - OPENgovTIA.ClsImport.CheckFilePesature.errore: ", Err)
            sErrCheck = sFileCheck & "::" & Err.StackTrace & "::" & Err.Message
            Return -1
        End Try
    End Function
    Private Function WriteScartiPesature(ByVal oPesatura As ObjPesatura, ByVal sFileOrg As String, ByVal sFileScarti As String) As Integer
        Try
            Dim FunctionGen As New generalClass.generalFunction
            Dim sDatiScarti As String

            sDatiScarti = "Data Acquisizione: " + Now + ";File Origine: " + sFileOrg + ";"
            sDatiScarti += "DATA E ORA: " + oPesatura.tDataOraConferimento + ";"
            sDatiScarti += "SISTEMA:" + oPesatura.sPuntoConferimento + ";"
            sDatiScarti += "CODICE INTERNO:" + oPesatura.sCodInterno + ";"
            sDatiScarti += "PESO:" + oPesatura.nLitri.ToString + ";"
            sDatiScarti += "VOLUME:" + oPesatura.nVolume.ToString + ";"
            sDatiScarti += "N° TESSERA:" + oPesatura.sNumeroTessera + ";"
            sDatiScarti += "CODICE UTENTE:" + oPesatura.sCodUtente + ";"
            sDatiScarti += "COMUNE:" + oPesatura.sComune + ";"

            If FunctionGen.WriteFile(sFileScarti, sDatiScarti) = 0 Then
                Return 0
            Else
                Return 1
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsImport.WriteScartiPesature.errore: ", Err)
            Return -1
        End Try
    End Function
End Class
