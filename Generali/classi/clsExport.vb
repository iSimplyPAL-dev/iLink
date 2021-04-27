Imports System.IO
Imports System.Threading
Imports System.Web.Caching
Imports log4net
''' <summary>
''' 
''' </summary>
Public Class clsExport
    Private Shared Log As ILog = LogManager.GetLogger(GetType(clsExport))
    Dim _IdEnte As String
    Dim _TypeDB As String
    Dim _StringConnection As String
    Dim _StringConnectionAnag As String
    Dim _StringConnectionICI As String
    Dim _StringConnectionTARSU As String
    Dim _StringConnectionOSAP As String
    Dim _StringConnectionH2O As String
    Dim _StringConnectionProvv As String
    Dim _ApplicationsEnabled As String
    Dim _Ambiente As String
    Dim _Operatore As String
    Dim _IsFromVariabile As String
    Dim _HasNotifiche As Boolean
    Dim _myPath As String
    Dim myFileName As String
    Dim DtDatiStampa As New DataTable
    Dim aListColonne As ArrayList
    Dim aMyHeaders As String()
    Dim nCol As Integer
    Dim ListProgress As New ArrayList

    ''' <summary>
    ''' Metodo che richiama la funzione asincrona di estrazione dati
    ''' </summary>
    ''' <param name="myDBConn"></param>
    ''' <param name="myApplicationsEnabled"></param>
    ''' <param name="myAmbiente"></param>
    ''' <param name="myIdEnte"></param>
    ''' <param name="myIsFromVariabile"></param>
    ''' <param name="myHasNotifiche"></param>
    ''' <param name="myOperatore"></param>
    ''' <param name="myPath"></param>
    Public Sub StartExport(myDBConn As Utility.DBUtility.objDBConnection, myApplicationsEnabled As String, myAmbiente As String, ByVal myIdEnte As String, myIsFromVariabile As String, myHasNotifiche As Boolean, myOperatore As String, myPath As String)
        Dim threadDelegate As ThreadStart = New ThreadStart(AddressOf StartExportThreadEntryPoint)
        Dim t As Thread = New Thread(threadDelegate)
        _IdEnte = myIdEnte
        _TypeDB = myDBConn.TypeDB
        _StringConnection = myDBConn.StringConnection
        _StringConnectionAnag = myDBConn.StringConnectionAnag
        _StringConnectionICI = myDBConn.StringConnectionICI
        _StringConnectionTARSU = myDBConn.StringConnectionTARSU
        _StringConnectionOSAP = myDBConn.StringConnectionOSAP
        _StringConnectionH2O = myDBConn.StringConnectionH2O
        _StringConnectionProvv = myDBConn.StringConnectionProvv
        _ApplicationsEnabled = myApplicationsEnabled
        _Ambiente = myAmbiente
        _Operatore = myOperatore
        _IsFromVariabile = myIsFromVariabile
        _HasNotifiche = myHasNotifiche
        _myPath = myPath
        t.Start()
    End Sub
    ''' <summary>
    ''' Funzione che ciclando su un cartella zippa i files al suo interno; se lo zip è andato a buon fine il file viene cancellato.
    ''' </summary>
    ''' <param name="IdEnte">string idente</param>
    ''' <param name="SourceDir">string percorso della directory in cui sono presenti i files da zippare</param>
    ''' <returns>ArrayList elenco dei files zippati</returns>
    Public Function GetRiepilogoExport(IdEnte As String, SourceDir As String) As ArrayList
        Dim myList As New ArrayList
        Dim fncZip As New Provvedimenti.clsCoattivo.cls290()
        Try
            Utility.Costanti.CreateDir(SourceDir)
            Dim ListFileNames() As FileInfo = New DirectoryInfo(SourceDir).GetFiles()

            For Each myFile As FileInfo In ListFileNames
                Dim ListToZip As New ArrayList
                If myFile.Extension = ".xls" Then
                    ListToZip.Add(myFile.FullName)
                    If fncZip.ZipFile(SourceDir, myFile.Name.ToLower().Replace(".xls", ".zip"), ListToZip) = False Then
                        myList = Nothing
                        Exit For
                    Else
                        File.Delete(myFile.FullName)
                    End If
                    myList.Add(myFile.Name)
                End If
            Next
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgov.clsExport.GetRiepilogoExport.errore: ", ex)
            myList = Nothing
        End Try
        Return myList
    End Function
    ''' <summary>
    ''' Metodo che richiama le esportazioni, ma prima di tutto elimina tutti i files già presenti nelle cartella; al termine richiama la funzione di zip.
    ''' </summary>
    Private Sub StartExportThreadEntryPoint()
        Dim MaxExport As Integer = 24
        Dim nExtract As Integer = 1

        CacheManager.SetExportInCorso(_IdEnte)

        Try
            For Each myFile As FileInfo In New DirectoryInfo(_myPath).GetFiles()
                myFile.Delete()
            Next

            ListProgress.Add("Anagrafe")
            ListProgress.Add("0")
            ListProgress.Add(FormatNumber(((nExtract * 100) / MaxExport), 0).Replace(",", "."))
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExtract += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportAnagrafe()
            If _StringConnectionICI <> "" Then
                nExtract = ExportDatiIMUTASI(nExtract, MaxExport)
            End If
            If _StringConnectionTARSU <> "" Then
                nExtract = ExportDatiTARI(nExtract, MaxExport)
            End If
            If _StringConnectionOSAP <> "" Then
                nExtract = exportdatiosapscuole(nExtract, MaxExport)
            End If
            If _StringConnectionH2O <> "" Then
                nExtract = exportdatih2o(nExtract, MaxExport)
            End If
            If _StringConnectionProvv <> "" Then
                nExtract = ExportdatiAtti(nExtract, MaxExport)
            End If
            ListProgress(0) = "Zip files"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExtract * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            GetRiepilogoExport(_IdEnte, _myPath)

            Dim fncActionEvent As New Utility.DBUtility(_TypeDB, _StringConnection)
            fncActionEvent.LogActionEvent(DateTime.Now, _Operatore, New Utility.Costanti.LogEventArgument().Elaborazioni, "StartExportThreadEntryPoint", Utility.Costanti.AZIONE_NEW, "", _IdEnte, -1)
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.clsExport.StartExportThreadEntryPoint.errore: ", ex)
        Finally
            CacheManager.RemoveExportInCorso()
            CacheManager.RemoveAvanzamentoExport()
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che racchiude tutte le estrazioni IMU/TASI
    ''' </summary>
    ''' <param name="nExtract">int</param>
    ''' <param name="MaxExport">int</param>
    ''' <returns>int</returns>
    Private Function ExportDatiIMUTASI(nExtract As Integer, MaxExport As Integer) As Integer
        Dim nExport As Integer = nExtract
        Try
            ListProgress(0) = "Dichiarazioni IMU"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExtract += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportDichIMU()
            ListProgress(0) = "Versamenti IMU/TASI"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExtract += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportVersamentiIMUTASI()
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.clsExport.ExportDatiIMUTASI.errore: ", ex)
            nExport = nExtract
        End Try
        Return nExport
    End Function
    ''' <summary>
    ''' Funzione che racchiude tutte le estrazioni TARI
    ''' </summary>
    ''' <param name="nExtract">int</param>
    ''' <param name="MaxExport">int</param>
    ''' <returns>int</returns>
    Private Function ExportDatiTARI(nExtract As Integer, MaxExport As Integer) As Integer
        Dim nExport As Integer = nExtract
        Try
            ListProgress(0) = "Dichiarazioni TARI"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportDichTARI()
            If _IsFromVariabile = "1" Then
                ListProgress(0) = "Pesature TARI"
                ListProgress(1) = "0"
                ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
                Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
                CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                ExportPesatureTARI()
            End If
            nExport += 1
            ListProgress(0) = "Avvisi Bonari TARI"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportAvvisiTARI()
            ListProgress(0) = "Rate Avvisi Bonari TARI"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportRateTARI()
            ListProgress(0) = "Pagamenti TARI"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportPagamentiTARI()
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.clsExport.ExportDatiTARI.errore: ", ex)
            nExport = nExtract
        End Try
        Return nExport
    End Function
    ''' <summary>
    ''' Funzione che racchiude tutte le estrazioni OSAP/SCUOLE
    ''' </summary>
    ''' <param name="nExtract">int</param>
    ''' <param name="MaxExport">int</param>
    ''' <returns>int</returns>
    Private Function ExportDatiOSAPSCUOLE(nExtract As Integer, MaxExport As Integer) As Integer
        Dim nExport As Integer = nExtract
        Try
            ListProgress(0) = "Dichiarazioni TOSAP/COSAP"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportDichOSAPSCUOLE(Utility.Costanti.TRIBUTO_OSAP)
            ListProgress(0) = "Avvisi Bonari TOSAP/COSAP"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportAvvisiOSAPSCUOLE(Utility.Costanti.TRIBUTO_OSAP)
            ListProgress(0) = "Rate Avvisi Bonari TOSAP/COSAP"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportRateOSAPSCUOLE(Utility.Costanti.TRIBUTO_OSAP)
            ListProgress(0) = "Pagamenti TOSAP/COSAP"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportPagamentiOSAPSCUOLE(Utility.Costanti.TRIBUTO_OSAP)
            ListProgress(0) = "Avvisi Bonari SCUOLE"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportAvvisiOSAPSCUOLE(Utility.Costanti.TRIBUTO_SCUOLE)
            ListProgress(0) = "Rate Avvisi Bonari SCUOLE"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportRateOSAPSCUOLE(Utility.Costanti.TRIBUTO_SCUOLE)
            ListProgress(0) = "Pagamenti SCUOLE"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportPagamentiOSAPSCUOLE(Utility.Costanti.TRIBUTO_SCUOLE)
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.clsExport.ExportDatiOSAPSCUOLE.errore: ", ex)
            nExport = nExtract
        End Try
        Return nExport
    End Function
    ''' <summary>
    ''' Funzione che racchiude tutte le estrazioni H2O
    ''' </summary>
    ''' <param name="nExtract">int</param>
    ''' <param name="MaxExport">int</param>
    ''' <returns>int</returns>
    Private Function ExportDatiH2O(nExtract As Integer, MaxExport As Integer) As Integer
        Dim nExport As Integer = nExtract
        Try
            ListProgress(0) = "Contatori Acquedotto"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportContatoriH2O()
            ListProgress(0) = "Letture Acquedotto"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportLettureH2O()
            ListProgress(0) = "Fatture/Note Acquedotto"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportAvvisiH2O()
            ListProgress(0) = "Rate Avvisi Bonari Acquedotto"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportRateH2O()
            ListProgress(0) = "Pagamenti Acquedotto"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportPagamentiH2O()
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.clsExport.ExportDatiH2O.errore: ", ex)
            nExport = nExtract
        End Try
        Return nExport
    End Function
    ''' <summary>
    ''' Funzione che racchiude tutte le estrazioni degli Atti
    ''' </summary>
    ''' <param name="nExtract">int</param>
    ''' <param name="MaxExport">int</param>
    ''' <returns>int</returns>
    Private Function ExportDatiAtti(nExtract As Integer, MaxExport As Integer) As Integer
        Dim nExport As Integer = nExtract
        Try
            ListProgress(0) = "Atti Accertamenti"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportAtti()
            ListProgress(0) = "Rate Atti Accertamenti"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportRateAtti()
            ListProgress(0) = "Pagamenti Atti Accertamenti"
            ListProgress(1) = "0"
            ListProgress(2) = FormatNumber(((nExport * 100) / MaxExport), 0).Replace(",", ".")
            Log.Debug("Avanzamento export->" + nExtract.ToString + " su tot " + MaxExport.ToString + "=" + ListProgress(2) + "%")
            nExport += 1
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            ExportPagamentiAtti()
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.clsExport.ExportDatiAtti.errore: ", ex)
            nExport = nExtract
        End Try
        Return nExport
    End Function
#Region "Anagrafe"
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati anagrafici e ne produce un excel
    ''' </summary>
    Private Sub ExportAnagrafe()
        Try
            DtDatiStampa = New DataTable
            nCol = 26
            DtDatiStampa = GetDatiAnagrafe(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "Anagrafe_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function GetDatiAnagrafe(ByVal nCampi As Integer) As DataTable
        Dim dvDati As DataView
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim ListAnagraficaExcel As ANAGRAFICAWEB.ListAnagrafica
            Dim objAnagraficaExcel As New ANAGRAFICAWEB.AnagraficaDB("")
            Dim dsAnagrafica As New DataSet

            ListAnagraficaExcel = objAnagraficaExcel.GetListAnagragrafica(_StringConnectionAnag, True, "", "", "", "", _Ambiente, _IdEnte, "", 0, "", "", "", "", "", "", False, -1, -1, -1)
            dsAnagrafica = ListAnagraficaExcel.p_dsItemsANAGRAFICA
            dvDati = dsAnagrafica.Tables(0).DefaultView
            If dvDati.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Sesso"
                sDatiStampa += "|Data Nascita"
                sDatiStampa += "|Luogo Nascita"
                sDatiStampa += "|Data Morte"
                sDatiStampa += "|Indirizzo"
                sDatiStampa += "|CAP"
                sDatiStampa += "|Città"
                sDatiStampa += "|Provincia"
                sDatiStampa += "|Tributo Invio"
                sDatiStampa += "|Nominativo Invio"
                sDatiStampa += "|Indirizzo Invio"
                sDatiStampa += "|CAP Invio"
                sDatiStampa += "|Città Invio"
                sDatiStampa += "|Provincia Invio"
                sDatiStampa += "|Codice Famiglia"
                sDatiStampa += "|Tipo Contatto"
                sDatiStampa += "|Contatto"
                sDatiStampa += "|Data validità invio"
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_ICI) > 0 Then
                    sDatiStampa += "|IMU"
                End If
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_TARSU) > 0 Then
                    sDatiStampa += "|TARI"
                End If
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_OSAP) > 0 Then
                    sDatiStampa += "|OSAP"
                End If
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_SCUOLE) > 0 Then
                    sDatiStampa += "|SCUOLA"
                End If
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_H2O) > 0 Then
                    sDatiStampa += "|H2O"
                End If
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_OSAP) > 0 Then
                    sDatiStampa += "|OSAP"
                End If
                If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_Accertamento) > 0 Then
                    sDatiStampa += "|PROV"
                End If
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvDati
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvDati.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("COGNOME_DENOMINAZIONE"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NOME"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CFPIVA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("SESSO"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DATA_NASCITA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("LUOGO_NASCITA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DATA_MORTE"))
                    sDatiStampa += "|" + (Utility.StringOperation.FormatString(myRow("VIA_RES")) + " " + Utility.StringOperation.FormatString(myRow("CIVICO_RES")) + " " + Utility.StringOperation.FormatString(myRow("ESPONENTE_CIVICO_RES")) + " " + Utility.StringOperation.FormatString(myRow("INTERNO_CIVICO_RES"))).Trim
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("CAP_RES"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("COMUNE_RES"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("PROVINCIA_RES"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("tributo_invio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("nome_invio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("via_invio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("cap_invio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("comune_invio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("pv_invio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("numerofamiglia"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("descrcontatto"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("datiriferimento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("datavaliditainviomail")).Replace("Data validità invio", "")
                    If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_ICI) > 0 Then
                        If Utility.StringOperation.FormatString(myRow("ICI")) = "1" Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_TARSU) > 0 Then
                        If Utility.StringOperation.FormatString(myRow("TARSU")) = "1" Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_OSAP) > 0 Then
                        If Utility.StringOperation.FormatString(myRow("OSAP")) = "1" Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_SCUOLE) > 0 Then
                        If Utility.StringOperation.FormatString(myRow("SCUOLA")) = "1" Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_H2O) > 0 Then
                        If Utility.StringOperation.FormatString(myRow("H2O")) = "1" Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If _ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_Accertamento) > 0 Then
                        If Utility.StringOperation.FormatString(myRow("PROVVEDIMENTI")) = "1" Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiAnagrafe.errore: ", ex)
            Return Nothing
        End Try
    End Function
#End Region
#Region "IMU/TASI"
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dichiarativi IMU e ne produce un excel
    ''' </summary>
    Private Sub ExportDichIMU()
        Try
            DtDatiStampa = New DataTable
            nCol = 29
            DtDatiStampa = GetDatiDichIMU(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "DichiarazioniIMU_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiDichIMU(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim Tabella As DataTable = New DichiarazioniICI.Database.ContribuentiImmobileView().ListContribuenti(_StringConnectionICI, _Ambiente, "", -1, "", "", "", "", "", "", "", -1, "", "", "", "-1", "-1", _IdEnte, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0)
            If Tabella.Rows.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Data Inizio"
                sDatiStampa += "|Data Fine"
                sDatiStampa += "|Caratteristica"
                sDatiStampa += "|Indirizzo"
                sDatiStampa += "|Partita Cat."
                sDatiStampa += "|Sezione"
                sDatiStampa += "|Foglio"
                sDatiStampa += "|Numero"
                sDatiStampa += "|Subalterno"
                sDatiStampa += "|Cat. Cat."
                sDatiStampa += "|Classe"
                sDatiStampa += "|Consistenza"
                sDatiStampa += "|Valore Euro"
                sDatiStampa += "|Rendita Euro"
                sDatiStampa += "|% Poss."
                sDatiStampa += "|Mesi Poss."
                sDatiStampa += "|Tipo Utilizzo"
                sDatiStampa += "|Tipo Possesso"
                sDatiStampa += "|Mesi Rid."
                sDatiStampa += "|Mesi Esc. Esen."
                sDatiStampa += "|Abit. Principale"
                sDatiStampa += "|N. Utilizzatori"
                sDatiStampa += "|Pertinenza"
                sDatiStampa += "|N.Figli Minori"
                sDatiStampa += "|Note"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRow In Tabella.Rows
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / Tabella.Rows.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
                    If Utility.StringOperation.FormatString(myRow("partitaIva")) = "" Then
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("codiceFiscale"))
                    Else
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("partitaIva"))
                    End If
                    sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DataInizio")))
                    sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DataFine")))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Caratteristica"))
                    sDatiStampa += "|" + (Utility.StringOperation.FormatString(myRow("Via")) + " " + Utility.StringOperation.FormatString(myRow("NumeroCivico")) + " " + Utility.StringOperation.FormatString(myRow("espcivico")) + " " + Utility.StringOperation.FormatString(myRow("scala")) + " " + Utility.StringOperation.FormatString(myRow("interno")) + " " + Utility.StringOperation.FormatString(myRow("piano"))).Trim
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("partitaCatastale"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Sezione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Foglio"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Numero"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("subalterno"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Categoria"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Classe"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Consistenza"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ValoreImmobile"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("rendita"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("PercPossesso"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("MesiPossesso"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("descTipoUtilizzo"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescTipoPossesso"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("mesiRiduzione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("mesiEsclusioneEsenzione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("abitazioneprincipaleattuale"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("numeroUtilizzatori"))
                    If Utility.StringOperation.FormatInt(myRow("idimmobilepertinente")) > 0 And Utility.StringOperation.FormatInt(myRow("idimmobilepertinente")) <> Utility.StringOperation.FormatInt(myRow("id")) Then
                        sDatiStampa += "|X"
                    Else
                        sDatiStampa += "|"
                    End If
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NUMEROFIGLI"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NOTEICI")).Replace("|", "-")
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiDichIMU.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dei versamenti IMU/TASI e ne produce un excel
    ''' </summary>
    Private Sub ExportVersamentiIMUTASI()
        Try
            DtDatiStampa = New DataTable
            nCol = 32
            DtDatiStampa = GetDatiVersamentiIMUTASI(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "VersamentiIMUTASI_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiVersamentiIMUTASI(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New DichiarazioniICI.Database.VersamentiView().List(_StringConnectionICI, "", 0, "", "", "", "", "", _IdEnte, 0, 0, 0, 0, "", "", "")
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Anno Riferimento"
                sDatiStampa += "|Tributo"
                sDatiStampa += "|Data Pagamento"
                sDatiStampa += "|Data Riversamento"
                sDatiStampa += "|Tipo Pagamento"
                sDatiStampa += "|N. Fabb."
                sDatiStampa += "|Importo Abitaz. Principale"
                sDatiStampa += "|Importo Altri Fabbricati Comune"
                sDatiStampa += "|Importo Altri Fabbricati Stato"
                sDatiStampa += "|Importo Aree Fabbricabili Comune"
                sDatiStampa += "|Importo Aree Fabbricabili Stato"
                sDatiStampa += "|Importo Terreni Agr. Comune"
                sDatiStampa += "|Importo Terreni Agr. Stato"
                sDatiStampa += "|Importo Fabbricati Rurali Uso Strumentale"
                sDatiStampa += "|Importo Fabbricati Rurali Uso Strumentale Stato"
                sDatiStampa += "|Importo Uso Prod.Cat.D"
                sDatiStampa += "|Importo Uso Prod.Cat.D Stato"
                sDatiStampa += "|Det. Abitaz. Principale"
                sDatiStampa += "|Importo Pagato"
                sDatiStampa += "|Importo Imposta"
                sDatiStampa += "|Importo Soprattassa"
                sDatiStampa += "|Importo PenaPecuniaria"
                sDatiStampa += "|Interessi"
                sDatiStampa += "|Data Provvedimento"
                sDatiStampa += "|Numero Provvedimento"
                sDatiStampa += "|Arrotondamento"
                sDatiStampa += "|Provenienza"
                sDatiStampa += "|Tipo versamento"
                sDatiStampa += "|Note"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
                    If Utility.StringOperation.FormatString(myRow("partitaIva")) = "" Then
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("codiceFiscale"))
                    Else
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("partitaIva"))
                    End If
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ANNORIFERIMENTO"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRTRIBUTO"))
                    sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("Datapagamento")))
                    sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DataRiversamento")))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdAccontoSaldo(myRow("Saldo"), myRow("Acconto"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NumeroFabbricatiPosseduti"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoAbitazPrincipale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoAltriFabbric"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoAltriFabbricStatale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoAreeFabbric"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoAreeFabbricStatale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImpoTerreni"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoTerreniStatale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoFabRurUsoStrum"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoFabRurUsoStrumStatale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoUsoProdCatD"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoUsoProdCatDStatale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("DetrazioneAbitazPrincipale"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoPagato"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoImposta"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoSoprattassa"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoPenaPecuniaria"))
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("Interessi"))
                    If IsDBNull(myRow("DataProvvedimentoViolazione")) = False Then
                        sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DataProvvedimentoViolazione"))
                    Else
                        sDatiStampa += "|"
                    End If
                    If IsDBNull(myRow("NumeroAttoAccertamento")) = False Then
                        sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NumeroAttoAccertamento"))
                    Else
                        sDatiStampa += "|"
                    End If
                    sDatiStampa += "|" + Business.CoreUtility.FormattaGrdEuro(myRow("ImportoPagatoArrotondamento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescProvenienza"))
                    If Utility.StringOperation.FormatBool(myRow("violazione")) = False And Utility.StringOperation.FormatBool(myRow("ravvedimentooperoso")) = False Then
                        sDatiStampa += "|Ordinario"
                    End If
                    If Utility.StringOperation.FormatBool(myRow("violazione")) = True And Utility.StringOperation.FormatBool(myRow("ravvedimentooperoso")) = False Then
                        sDatiStampa += "|Violazione"
                    End If
                    If Utility.StringOperation.FormatBool(myRow("violazione")) = False And Utility.StringOperation.FormatBool(myRow("ravvedimentooperoso")) = True Then
                        sDatiStampa += "|Ravvedimento operoso"
                    End If
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("note"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiVersamentiIMUTASI.errore: ", ex)
            Return Nothing
        End Try
    End Function
#End Region
#Region "TARI"
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dichiarativi TARI e ne produce un excel
    ''' </summary>
    Private Sub ExportDichTARI()
        Try
            DtDatiStampa = New DataTable
            nCol = 47
            DtDatiStampa = GetDatiDichTARI(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "DichiarazioniTARI_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiDichTARI(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim mySearch As New RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata
            mySearch.IdEnte = _IdEnte
            Dim dvStampa As DataView = New OPENgovTIA.ClsDichiarazione().GetStampaDichiarazioni(_StringConnectionTARSU, OPENgovTIA.ClsDichiarazione.TipoStampaEsportazione, mySearch)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Residente|Provenienza Dichiarazione"
                If _IsFromVariabile = "1" Then
                    sDatiStampa += "|Tipo Tessera|N.Tessera|Cod.Interno|Cod.Utente|Data Rilascio|Data Cessazione"
                End If
                sDatiStampa += "|Via|Civico|Esponente|Interno|Data Inizio Occupazione|Data Fine Occupazione|Foglio|Numero|Subalterno"
                sDatiStampa += "|Stato Occupazione"
                sDatiStampa += "|Categoria Catastale|MQ Totali"
                sDatiStampa += "|MQ Tassabili|Categoria Tariffaria|N.Componenti|N.Componenti PV"
                sDatiStampa += "|Forza Calcolo PV"
                sDatiStampa += "|Tipo Vano|MQ Vano"
                sDatiStampa += "|Vani Esenti"
                sDatiStampa += "|Tarsu Giornaliera|Giorni|Riduzioni|Agevolazioni|Occupazione/Detenzione|Singolo Nucleo|Destinazione d'uso|Tipo Unità"
                sDatiStampa += "|Note"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("COGNOME_DENOMINAZIONE"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NOME")).ToUpper()
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("cfpiva"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("residente"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("provdich"))
                    If _IsFromVariabile = "1" Then
                        sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("TIPO_TESSERA"))
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("NUMERO_TESSERA"))
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODICE_INTERNO"))
                        sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODICE_UTENTE"))
                        sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DATA_RILASCIO")))
                        sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DATA_CESSAZIONE")))
                    End If
                    sDatiStampa += "|" + (Utility.StringOperation.FormatString(myRow("Via")) + " " + Utility.StringOperation.FormatString(myRow("Civico")) + " " + Utility.StringOperation.FormatString(myRow("esponente")) + " " + Utility.StringOperation.FormatString(myRow("interno"))).Trim
                    sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DATA_INIZIO")))
                    sDatiStampa += "|" + New FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("DATA_FINE")))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("FOGLIO"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NUMERO"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("SUBALTERNO"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("statooccupaz"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("CODCATEGORIACATASTALE"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("mqImmobile"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("mqtassabili"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("cattares"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ncomponenti"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ncomponenti_pv"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("FORZA_CALCOLAPV"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescTipoVano"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("mq"))
                    If Utility.StringOperation.FormatBool(myRow("esente")) = True Then
                        sDatiStampa += "|X"
                    Else
                        sDatiStampa += "|"
                    End If
                    If Utility.StringOperation.FormatInt(myRow("GGTARSU")) <= 0 Then
                        sDatiStampa += "|No|"
                    Else
                        sDatiStampa += "|Si|" + Utility.StringOperation.FormatString(myRow("GGTARSU"))
                    End If
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("idriduzione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("iddetassazione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescTitoloOccupazione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescNaturaOccupante"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescTipoUnita"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DescDestUso"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("notedich"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiDichTARI.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati degli avvisi bonari TARI e ne produce un excel
    ''' </summary>
    Private Sub ExportAvvisiTARI()
        Try
            DtDatiStampa = New DataTable
            nCol = 69
            DtDatiStampa = GetDatiAvvisiTARI(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "AvvisiTARI_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiAvvisiTARI(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa() As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso = New OPENgovTIA.ClsGestRuolo().GetRuoloMinuta(_StringConnectionTARSU, _IdEnte, -1)
            If dvStampa.GetUpperBound(0) > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza"
                sDatiStampa += "|Nominativo Invio|Indirizzo Invio|Civico Invio|CAP Invio|Comune Invio|Provincia Invio"
                sDatiStampa += "|N.Avviso"
                sDatiStampa += "|Imp.Netto"
                sDatiStampa += "|Imp.Addizionali EX ECA|Imp.Tributo EX ECA|Imp.Aggio per Ente|Imp.Addizionale Provinciale per Ente|Imp.Addizionale Provinciale"
                sDatiStampa += "|Imp.Maggiorazione"
                If _IsFromVariabile = "1" Then
                    sDatiStampa += "|Imp.Conferimenti"
                End If
                sDatiStampa += "|Imp.Arrotondamento|Imp.Totale|Imp.Originale"
                If _HasNotifiche Then
                    sDatiStampa += "|Imp.Sanzioni|Imp.Spese Notifica"
                End If
                sDatiStampa += "|Tipo Utenza"
                sDatiStampa += "|Anno|Ubicazione|Civico|Esponente|Interno"
                sDatiStampa += "|Foglio|Numero|Subalterno"
                sDatiStampa += "|Tipo Partita|Categoria|N.Componenti|MQ|Tempo"
                sDatiStampa += "|Tariffa|Imp.Articoli (tassa base)|Imp.Riduzioni|Imp.Detassazioni|Imp.Netto"
                sDatiStampa += "|Riduzioni"
                If _HasNotifiche Then
                    sDatiStampa += "|Data Notifica"
                End If
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Length)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    For Each myArticolo As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjArticolo In myRow.oArticoli
                        sDatiStampa = myRow.sCognome
                        sDatiStampa += "|" + myRow.sNome
                        sDatiStampa += "|'" + myRow.sCodFiscale.ToUpper()
                        sDatiStampa += "|" + myRow.sIndirizzoRes
                        If myRow.sCivicoRes <> "-1" Then
                            sDatiStampa += "|'" + myRow.sCivicoRes
                        Else
                            sDatiStampa += "|"
                        End If
                        sDatiStampa += "|" + myRow.sCAPRes
                        sDatiStampa += "|" + myRow.sComuneRes
                        sDatiStampa += "|" + myRow.sProvRes
                        Dim y As Integer
                        Dim impNetto As Double = 0
                        Dim impECA As Double = 0
                        Dim impMECA As Double = 0
                        Dim impAggioPerEnte As Double = 0
                        Dim impProvincialePerEnte As Double = 0
                        Dim impProvinciale As Double = 0
                        Dim impSpese As Double = 0
                        Dim impSanzioni As Double = 0

                        sDatiStampa += "|'" + myRow.sNominativoCO
                        sDatiStampa += "|" + myRow.sIndirizzoCO
                        If myRow.sCivicoCO <> "-1" Then
                            sDatiStampa += "|'" + myRow.sCivicoCO
                        Else
                            sDatiStampa += "|"
                        End If
                        sDatiStampa += "|" + myRow.sCAPCO
                        sDatiStampa += "|" + myRow.sComuneCO
                        sDatiStampa += "|" + myRow.sProvCO

                        sDatiStampa += "|'" + myRow.sCodiceCartella
                        For Each myArt As RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjArticolo In myRow.oArticoli
                            If myArt.TipoPartita.ToUpper.IndexOf("MAGG") < 0 Then
                                If myArt.TipoPartita.ToUpper.IndexOf("CREDIT") < 0 Then
                                    impNetto += myArt.impNetto
                                Else
                                    impNetto -= myArt.impNetto
                                End If
                            End If
                        Next
                        If Not IsNothing(myRow.oDetVoci) Then
                            For y = 0 To myRow.oDetVoci.GetUpperBound(0)
                                Select Case myRow.oDetVoci(y).sCapitolo
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.ProvincialeEnte
                                        impProvincialePerEnte += myRow.oDetVoci(y).impDettaglio
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.AggioEnte
                                        impAggioPerEnte += myRow.oDetVoci(y).impDettaglio
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.ECA
                                        impECA += myRow.oDetVoci(y).impDettaglio
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.MECA
                                        impMECA += myRow.oDetVoci(y).impDettaglio
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.Provinciale
                                        impProvinciale += myRow.oDetVoci(y).impDettaglio
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.SpeseNotifica
                                        impSpese += myRow.oDetVoci(y).impDettaglio
                                    Case RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.Sanzione
                                        impSanzioni += myRow.oDetVoci(y).impDettaglio
                                    Case Else
                                End Select
                            Next
                        Else
                            impProvincialePerEnte += 0
                            impAggioPerEnte += 0
                            impECA += 0
                            impMECA += 0
                            impProvinciale += 0
                            impSpese += 0
                            impSanzioni += 0
                        End If
                        sDatiStampa += "|" + FormatNumber(impNetto.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impECA.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impMECA.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impAggioPerEnte.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impProvincialePerEnte.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impProvinciale.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(myRow.impPM.ToString, 2)
                        If _IsFromVariabile = "1" Then
                            sDatiStampa += "|" + FormatNumber(myRow.impPC.ToString, 2)
                        End If
                        sDatiStampa += "|" + FormatNumber(myRow.impArrotondamento.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(myRow.impCarico.ToString, 2)
                        If myRow.impPRESgravio >= 0 Then
                            sDatiStampa += "|" + FormatNumber(myRow.impPRESgravio.ToString, 2)
                        Else
                            sDatiStampa += "|"
                        End If
                        If _HasNotifiche Then
                            sDatiStampa += "|" + FormatNumber(impSanzioni.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(impSpese.ToString, 2)
                        End If
                        sDatiStampa += "|" + myArticolo.sIdTipoUnita
                        sDatiStampa += "|" + myRow.sAnnoRiferimento
                        sDatiStampa += "|" + myArticolo.sVia
                        sDatiStampa += "|'" + myArticolo.sCivico
                        sDatiStampa += "|" + " " & myArticolo.sEsponente
                        sDatiStampa += "|" + " " & myArticolo.sInterno & " " & myArticolo.sScala
                        sDatiStampa += "|" + myArticolo.sFoglio
                        sDatiStampa += "|" + myArticolo.sNumero
                        sDatiStampa += "|" + myArticolo.sSubalterno
                        sDatiStampa += "|" + myArticolo.TipoPartita
                        sDatiStampa += "|" + myArticolo.sDescrCategoria
                        If myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") Then
                            sDatiStampa += "|" + myArticolo.nComponenti.ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If myArticolo.nMQ > 0 Then
                            sDatiStampa += "|" + myArticolo.nMQ.ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        sDatiStampa += "|" + myArticolo.nBimestri.ToString
                        sDatiStampa += "|" + myArticolo.impTariffa.ToString
                        sDatiStampa += "|" + FormatNumber(myArticolo.impRuolo, 2)
                        If myArticolo.impRiduzione > 0 Then
                            sDatiStampa += "|" + FormatNumber(myArticolo.impRiduzione, 2)
                        Else
                            sDatiStampa += "|"
                        End If
                        If myArticolo.impDetassazione > 0 Then
                            sDatiStampa += "|" + FormatNumber(myArticolo.impDetassazione, 2)
                        Else
                            sDatiStampa += "|"
                        End If
                        sDatiStampa += "|" + FormatNumber(myArticolo.impNetto, 2)
                        sDatiStampa += "|'"
                        Dim oRid As New RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati
                        If Not myArticolo.oRiduzioni Is Nothing Then
                            For Each oRid In myArticolo.oRiduzioni
                                sDatiStampa += "-" + oRid.sDescrizione + " " + oRid.sDescrTipo + " " + oRid.sValore
                            Next
                        End If
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Next
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiAvvisiTARI.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle rate degli avvisi bonari TARI e ne produce un excel
    ''' </summary>
    Private Sub ExportRateTARI()
        Try
            DtDatiStampa = New DataTable
            nCol = 19
            DtDatiStampa = GetDatiRateTARI(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "RateTARI_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiRateTARI(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New OPENgovTIA.ClsGestRuolo().GetMinutaRate(_StringConnectionTARSU, _IdEnte, -1)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "N.Avviso"
                sDatiStampa += "|Cod.Tributo|Anno|Cod.Ente|Sezione|Acconto|Saldo|Num.Fab."
                sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata"
                sDatiStampa += "|Codice Bollettino|Codeline|Barcode"
                sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2"
                sDatiStampa += "|Tipo Utenza"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = New OPENgovTIA.ClsStampaXLS().PrintMinutaRateRowDati(myRow)
                    If sDatiStampa <> "" Then
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiRateTARI.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dei pagamenti TARI e ne produce un excel
    ''' </summary>
    Private Sub ExportPagamentiTARI()
        Try
            DtDatiStampa = New DataTable
            nCol = 14
            DtDatiStampa = GetDatiPagamentiTARI(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "PagamentiTARI_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiPagamentiTARI(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim mySearch As New OPENgovTIA.ObjSearchPagamenti
            mySearch.sEnte = _IdEnte
            mySearch.IdTributo = Utility.Costanti.TRIBUTO_TARSU
            Dim dvStampa As DataView = New OPENgovTIA.ClsGestPag().GetStampaPagamenti(mySearch, _StringConnectionTARSU)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Ente"
                sDatiStampa = "|Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Anno|N.Avviso"
                sDatiStampa += "|Imp.Emesso Comune|Imp.Emesso Maggiorazione"
                sDatiStampa += "|Imp.Emesso"
                sDatiStampa += "|Data Accredito|Data Pagamento"
                sDatiStampa += "|Imp.Pagato Comume|Imp.Pagato Maggiorazione|Imp.Pagato"
                sDatiStampa += "|Provenienza"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = New OPENgovTIA.ClsStampaXLS().PrintPagamentiRowDati(_IdEnte, myRow, OPENgovTIA.ClsGestPag.TipoStampaPagamenti, Utility.Costanti.TRIBUTO_TARSU)
                    If sDatiStampa <> "" Then
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiPagamentiTARI.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle pesature e ne produce un excel
    ''' </summary>
    Private Sub ExportPesatureTARI()
        Try
            DtDatiStampa = New DataTable
            nCol = 9
            DtDatiStampa = GetDatiPesatureTARI(_IdEnte, _IsFromVariabile, nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "PesatureTARI_" & Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiPesatureTARI(IdEnte As String, IsFromVariabile As String, ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New OPENgovTIA.GestPesatura().GetExportPesature(_StringConnectionTARSU, _IdEnte)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|N.Tessera|Cod.Utente|Data Ora|Q.tà|Tipo"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("Cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("CFPIva"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NTessera"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("CodUtente"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("dataora"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("qta"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("tipo"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgov.cslExport.GetDatiPesatureTARI.errore: ", ex)
            Return Nothing
        End Try
    End Function
#End Region
#Region "OSAP"
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dichiarativi OSAP e ne produce un excel
    ''' </summary>
    ''' <param name="IdTributo"></param>
    Private Sub ExportDichOSAPSCUOLE(IdTributo As String)
        Try
            DtDatiStampa = New DataTable
            nCol = 21
            DtDatiStampa = GetDatiDichOSAP(IdTributo, nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "Dichiarazioni" + IdTributo + "_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiDichOSAP(IdTributo As String, ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim mySearch As New DTO.DichiarazioneSearch
            mySearch.IdEnte = _IdEnte
            mySearch.CodTributo = IdTributo
            Dim dvStampa As DataView = New DAO.DichiarazioniDAO().PrintDichiarazioni(_StringConnectionOSAP, mySearch)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA"
                sDatiStampa += "|Via Res.|Civico Res.|Cap Res.|Comune Res.|Provincia Res."
                sDatiStampa += "|N.Autoriz./Conces.|Data Autoriz./Conces."
                sDatiStampa += "|Ubicazione|Tipologia Occupazione|Categoria|Consistenza|Data Inizio|Data Fine|Durata"
                sDatiStampa += "|Attrazione|Agevolazioni|Imp.Magg.|% Magg.|Imp.Detraz."
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = DTO.MetodiDichiarazioneTosapCosap.PrintDichiarazioniRowDati(_IdEnte, myRow)
                    If sDatiStampa <> "" Then
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiDichOSAP.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati degli avvisi bonari OSAP/SCUOLE e ne produce un excel
    ''' </summary>
    ''' <param name="IdTributo"></param>
    Private Sub ExportAvvisiOSAPSCUOLE(IdTributo As String)
        Try
            DtDatiStampa = New DataTable
            nCol = 28
            DtDatiStampa = GetDatiAvvisiOSAPSCUOLE(IdTributo, nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "Avvisi" + IdTributo + "_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiAvvisiOSAPSCUOLE(IdTributo As String, ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = DTO.MetodiMinuta.GetMinutaRecords(_StringConnectionOSAP, _IdEnte, IdTributo, -1, False)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA"
                sDatiStampa += "|Via Res.|Civico Res.|Cap Res.|Comune Res.|Provincia Res."
                sDatiStampa += "|Nominativo C/O|Indirizzo C/O|Civico C/O|Cap C/O|Comune C/O|Pv C/O"
                sDatiStampa += "|Codice Cartella|Imponibile|Arrotondamento|Carico"
                sDatiStampa += "|Descrizione|Consistenza|Data Inizio|Durata|Data Fine|Tipologia Occupazionec|Categoria|Tariffa|Importo|"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = DTO.MetodiMinuta.GetMinutaRowDati(myRow)
                    If sDatiStampa <> "" Then
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiAvvisiOSAP.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle rate degli avvisi bonari OSAP/SCUOLA e ne produce un excel
    ''' </summary>
    ''' <param name="IdTributo"></param>
    Private Sub ExportRateOSAPSCUOLE(IdTributo As String)
        Try
            DtDatiStampa = New DataTable
            nCol = 12
            DtDatiStampa = GetDatiRateOSAPSCUOLE(IdTributo, nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "Rate" + IdTributo + "_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiRateOSAPSCUOLE(IdTributo As String, ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = DTO.MetodiMinuta.GetMinutaRecords(_StringConnectionOSAP, _IdEnte, IdTributo, -1, True)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Codice Cartella"
                sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata"
                sDatiStampa += "|Codice Bollettino|Codeline|Barcode"
                sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2|"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = DTO.MetodiMinuta.GetMinutaRateRowDati(myRow)
                    If sDatiStampa <> "" Then
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiRateOSAPSCUOLE.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dei pagamenti OSAP/SCUOLA e ne produce un excel
    ''' </summary>
    ''' <param name="IdTributo"></param>
    Private Sub ExportPagamentiOSAPSCUOLE(IdTributo As String)
        Try
            DtDatiStampa = New DataTable
            nCol = 10
            DtDatiStampa = GetDatiPagamentiOSAP(_StringConnectionOSAP, IdTributo, nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "Pagamenti" + IdTributo + "_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiPagamentiOSAP(myStringConnection As String, IdTributo As String, ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim mySearch As New OPENgovTIA.ObjSearchPagamenti
            mySearch.sEnte = _IdEnte
            mySearch.IdTributo = IdTributo
            Dim dvStampa As DataView = New OPENgovTIA.ClsGestPag().GetStampaPagamenti(mySearch, myStringConnection)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Anno|N.Avviso"
                sDatiStampa += "|Imp.Emesso"
                sDatiStampa += "|Data Accredito|Data Pagamento"
                sDatiStampa += "|Imp.Pagato"
                sDatiStampa += "|Provenienza"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = New OPENgovTIA.ClsStampaXLS().PrintPagamentiRowDati(_IdEnte, myRow, OPENgovTIA.ClsGestPag.TipoStampaPagamenti, IdTributo)
                    If sDatiStampa <> "" Then
                        If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiPagamentiOSAP.errore: ", ex)
            Return Nothing
        End Try
    End Function
#End Region
#Region "H2O"
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dei contatori H2O e ne produce un excel
    ''' </summary>
    Private Sub ExportContatoriH2O()
        Try
            DtDatiStampa = New DataTable
            nCol = 32
            DtDatiStampa = GetDatiContatoriH2O(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "ContatoriH2O_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Function GetDatiContatoriH2O(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New OpenUtenze.GestContatori().GetElencoContrattiContatori(_StringConnectionH2O, _IdEnte, "", "", "", "", "", "", "", -1, -1, False, -1)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa += "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||||"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco le intestazioni di colonna
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|Matricola|Via|Civico|Esponente|Foglio|Numero|Subalterno|Tipo Utenza|N.Utenze|Tipo Contatore|Data Attivazione|Data Sostituzione|Data Cessazione"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    sDatiStampa = New OpenUtenze.ClsStampaXLS().PrintElencoContatoriRowDati(_IdEnte, myRow)
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiContatoriH2O.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Private Function GetDatiContatoriH2O(ByVal nCampi As Integer) As DataTable
    '    Dim DtDatiStampa As New DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable

    '    Try
    '        Dim dvStampa As DataView = New OpenUtenze.GestContatori().GetElencoContrattiContatori(_StringConnectionH2O, _IdEnte, "", "", "", "", "", "", "", -1, -1, False)
    '        If dvStampa.Count > 0 Then
    '            'carico il dataset
    '            DsStampa.Tables.Add("STAMPA")
    '            'carico le colonne nel dataset
    '            For x As Integer = 0 To nCampi + 1
    '                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '            Next

    '            'carico il datatable
    '            DtStampa = DsStampa.Tables("STAMPA")
    '            'inserisco l'intestazione dell'ente
    '            sDatiStampa = ListProgress(0)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '            If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            'inserisco una riga vuota
    '            sDatiStampa = ""
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '            If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            sDatiStampa += "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||||"
    '            If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            'inserisco le intestazioni di colonna
    '            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|Matricola|Via|Civico|Esponente|Foglio|Numero|Subalterno|Tipo Utenza|N.Utenze|Tipo Contatore|Data Attivazione|Data Sostituzione|Data Cessazione"
    '            If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If

    '            For Each myRow As DataRowView In dvStampa
    '                'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
    '                'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
    '                sDatiStampa = New OpenUtenze.ClsStampaXLS().PrintElencoContatoriRowDati(_IdEnte, myRow)
    '                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                    Return Nothing
    '                End If
    '            Next
    '        End If
    '        Return DtStampa
    '    Catch ex As Exception
    '        Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgov.cslExport.GetDatiContatoriH2O.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle letture H2O e ne produce un excel
    ''' </summary>
    Private Sub ExportLettureH2O()
        Try
            DtDatiStampa = New DataTable
            nCol = 30
            DtDatiStampa = GetDatiLettureH2O(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "LettureH2O_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiLettureH2O(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New OpenUtenze.GestLetture().getTableLetture(_StringConnectionH2O, _IdEnte, -1, "", "", "", "", "", -1, False)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco le intestazioni di colonna
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
                sDatiStampa += "N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
                sDatiStampa += "Via|Civico|Esponente|Matricola|Tipo Utenza|N.Utenze|Data Lettura Precedente|Lettura Precedente|Data Lettura Attuale|Lettura Attuale|Prima Lettura"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = New OpenUtenze.ClsStampaXLS().PrintLettureRowDati(myRow)
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiLettureH2O.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle Fatture/Note H2O e ne produce un excel
    ''' </summary>
    Private Sub ExportAvvisiH2O()
        Try
            DtDatiStampa = New DataTable
            nCol = 45
            DtDatiStampa = GetDatiAvvisiH2O(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "FattureNoteH2O_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiAvvisiH2O(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim IdContribPrec, IdIntestPrec As Integer

        Try
            Dim dvStampa() As RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.ObjFattura = New OpenUtenze.ClsFatture().GetFattura(_StringConnectionH2O, _IdEnte, -1, -1, False)
            If dvStampa.GetUpperBound(0) > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Nominativo Intestatario|N.Utente|Nominativo Utente|Cod.Fiscale/P.Iva"
                sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza|Belfiore Residenza"
                sDatiStampa += "|Ubicazione Via|Ubicazione Civico|Matricola|Tipo Utenza|N.Utenze|Esente H2O|Esente QF Acqua|Esente Depurazione|Esente QF Depurazione|Esente Fognatura|Esente QF Fognatura"
                sDatiStampa += "|Tipo Documento|Data Lettura Prec.|Lettura Prec.|Data Lettura Att.|Lettura Att.|Consumo|Giorni"
                sDatiStampa += "|Importo Scaglioni Euro|Importo Canone Depurazione Euro|Importo Canone Fognatura Euro"
                sDatiStampa += "|Importo UI1 Acqua Euro|Importo UI1 Depurazione Euro|Importo UI1 Fognatura Euro"
                sDatiStampa += "|Importo Nolo Euro|Importo Quote Fisse Acqua Euro|Importo Quote Fisse Depurazione Euro|Importo Quote Fisse Fognatura Euro|Importo Addizionali Euro"
                sDatiStampa += "|Importo Imponibile Euro|Importo Iva Euro|Importo Esente Euro|Importo Arrotondamento Euro|Importo Documento Euro|Data Documento|N.Documento"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.ObjFattura In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Length)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = New OpenUtenze.ClsStampaXLS().PrintMinutaRuoloH2ORowDati(myRow, IdContribPrec, IdIntestPrec, True)
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                    IdIntestPrec = myRow.nIdIntestatario
                    IdContribPrec = myRow.nIdUtente
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiAvvisiH2O.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle rate H2O e ne produce un excel
    ''' </summary>
    Private Sub ExportRateH2O()
        Try
            DtDatiStampa = New DataTable
            nCol = 13
            DtDatiStampa = GetDatiRateH2O(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "RateH2O_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiRateH2O(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New OpenUtenze.ClsFatture().GetMinutaRate(_StringConnectionH2O, _IdEnte, -1)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "N.Fattura|Data Emissione"
                sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata"
                sDatiStampa += "|Codice Bollettino|Codeline|Barcode"
                sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRow In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = ""
                    sDatiStampa += "'" + Utility.StringOperation.FormatString(myRow("IDDOC"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DATADOC"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NUMERO_RATA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRIZIONE_RATA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DATA_SCADENZA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("IMPORTO_RATA"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODICE_BOLLETTINO"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODELINE"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODICE_BARCODE"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CONTO_CORRENTE"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRIZIONE_1_RIGA"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRIZIONE_2_RIGA"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiRateH2O.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dei pagamenti H2O e ne produce un excel
    ''' </summary>
    Private Sub ExportPagamentiH2O()
        Try
            DtDatiStampa = New DataTable
            nCol = 19
            DtDatiStampa = GetDatiPagamentiH2O(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "PagamentiH2O_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiPagamentiH2O(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim mySearchParam As New RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.OggettoPagamento
            mySearchParam.IDEnte = _IdEnte
            'Dim dvStampa As DataView = New OpenUtenze.ClsPagamenti().GetStampaPagamenti(_StringConnectionH2O, _IdEnte, New RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.OggettoPagamento)
            Dim dvStampa As DataView = New OpenUtenze.ClsPagamenti().GetStampaPagamenti(OpenUtenze.ClsPagamenti.TypeStampa.Pagamenti, mySearchParam)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Data Fattura|N.Fattura|Importo Emesso|Data Accredito|Data Pagamento|Provenienza|Importo Pagato"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
                    sDatiStampa = Utility.StringOperation.FormatString(myRow("cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("nome"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("cfpiva"))
                    sDatiStampa += "|" + New UtilityOPENgov().GiraDataFromDB(Utility.StringOperation.FormatString(myRow("data_fattura")), "G")
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("numero_fattura"))
                    sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("impemesso")), 2)
                    sDatiStampa += "|'" + New UtilityOPENgov().GiraDataFromDB(Utility.StringOperation.FormatString(myRow("data_accredito")), "G")
                    sDatiStampa += "|'" + New UtilityOPENgov().GiraDataFromDB(Utility.StringOperation.FormatString(myRow("data_pagamento")), "G")
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("provenienza"))
                    sDatiStampa += "|" + FormatNumber(myRow("importo"), 2)
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiPagamentiH2O.errore: ", ex)
            Return Nothing
        End Try
    End Function
#End Region
#Region "Provvedimenti"
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati degli atti di accertamento e delle ingiunzioni e ne produce un excel
    ''' </summary>
    Private Sub ExportAtti()
        Try
            DtDatiStampa = New DataTable
            nCol = 30
            DtDatiStampa = GetDatiAtti(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "Atti_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiAtti(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New Provvedimenti.ClsRicercaAtti().GetExportAtti(_StringConnectionProvv, _IdEnte)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|N. Atto"
                sDatiStampa += "|Anno"
                sDatiStampa += "|Tipologia Atto"
                sDatiStampa += "|Stato"
                sDatiStampa += "|Data Elaborazione"
                sDatiStampa += "|Data Stampa"
                sDatiStampa += "|Data Consegna"
                sDatiStampa += "|Data Notifica"
                sDatiStampa += "|Data Rettifica|Data Annullamento"
                sDatiStampa += "|Data Coattivo"
                sDatiStampa += "|Differenza Imposta"
                sDatiStampa += "|Sanzioni"
                sDatiStampa += "|Sanzioni Non Riducibili"
                sDatiStampa += "|Sanzioni Ridotte"
                sDatiStampa += "|Interessi"
                sDatiStampa += "|Addizionali"
                sDatiStampa += "|Spese"
                sDatiStampa += "|Arrotondamento"
                sDatiStampa += "|Totale"
                sDatiStampa += "|Arrotondamento Ridotto"
                sDatiStampa += "|Totale Ridotto"
                sDatiStampa += "|Dati Dichiarato"
                sDatiStampa += "|Dati Accertato"
                sDatiStampa += "|Descrizione Sanzioni"
                sDatiStampa += "|Rateizzato"
                sDatiStampa += "|Note"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("cfpiva"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("numero_atto"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("anno"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("tipoatto"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("stato"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_elaborazione"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_stampa"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_consegna"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_notifica"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_rettifica"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_annullamento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_coattivo"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("diffimposta"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("sanzioni"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("sanzioninonrid"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("sanzionirid"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("interessi"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("addizionali"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("spese"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("arrotondamento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("totale"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("arrotondamentorid"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("totalerid"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("dichiarato"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("accertato"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("descrsanzioni"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("rateizzato"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("note"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiAtti.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati delle rateizzazioni/accorpamenti degli atti di accertamento e delle ingiunzioni e ne produce un excel
    ''' </summary>
    Private Sub ExportRateAtti()
        Try
            DtDatiStampa = New DataTable
            nCol = 11
            DtDatiStampa = GetDatiRateAtti(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "RateAtti_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiRateAtti(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New Provvedimenti.clsPagamenti(_StringConnectionProvv).GetExportRate(_IdEnte)

            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|N. Accorpamento"
                sDatiStampa += "|N. Rata|Data Scadenza|Importo Rata|Importo Interesse|Importo Totale"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("cfpiva"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("naccorpamento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("nrata"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_scadenza"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("importorata"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("importointeressi"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("importototale"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiRateAtti.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che richiama l'estrazione dei dati dei pagamenti degli atti di accertamento e delle ingiunzioni e ne produce un excel
    ''' </summary>
    Private Sub ExportPagamentiAtti()
        Try
            DtDatiStampa = New DataTable
            nCol = 9
            DtDatiStampa = GetDatiPagamentiAtti(nCol)
            CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))
            If Not IsNothing(DtDatiStampa) Then
                If DtDatiStampa.Rows.Count > 0 Then
                    'valorizzo il nome del file
                    myFileName = "PagamentiAtti_" + Format(Now, "yyyyMMdd") & ".xls"
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x As Integer = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                    Dim MyCol() As Integer = New Integer(nCol) {}
                    For x As Integer = 0 To nCol
                        MyCol(x) = x
                    Next
                    SaveXLS(_myPath + myFileName, DtDatiStampa, MyCol, aMyHeaders)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che preleva i dati e costruisce il datatable da stampare
    ''' </summary>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    Private Function GetDatiPagamentiAtti(ByVal nCampi As Integer) As DataTable
        Dim DtDatiStampa As New DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable

        Try
            Dim dvStampa As DataView = New Provvedimenti.clsPagamenti(_StringConnectionProvv).GetExportPagamenti(_IdEnte)
            If dvStampa.Count > 0 Then
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x As Integer = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = ListProgress(0)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|N. Accorpamento"
                sDatiStampa += "|N. Rata|Importo Pagato|Data Pagamento|Data Accredito|Provenienza"
                If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If

                For Each myRow As DataRowView In dvStampa
                    'ListProgress(1) = FormatNumber(CDbl(ListProgress(1) + (100 / dvStampa.Count)), 2).Replace(",", ".")
                    'CacheManager.SetAvanzamentoExport(CType(ListProgress.ToArray(GetType(String)), String()))

                    sDatiStampa = Utility.StringOperation.FormatString(myRow("cognome"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
                    sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("cfpiva"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("naccorpamento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("nrata"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("importo_pagato"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_pagamento"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("data_valuta"))
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("provenienza"))
                    If New ClsStampaXLS().AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
            End If
            Return DtStampa
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgov.cslExport.GetDatiPagamentiAtti.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione che salva l'oggetto in ingresos in file Excel
    ''' </summary>
    ''' <param name="SourceFile"></param>
    ''' <param name="dtDati"></param>
    ''' <param name="ListCol"></param>
    ''' <param name="ListHeaders"></param>
    Private Sub SaveXLS(SourceFile As String, dtDati As DataTable, ListCol() As Integer, ListHeaders() As String)
        Dim objExport As New RKLib.ExportData.Export("Win")
        objExport.ExportDetails(dtDati, ListCol, ListHeaders, RKLib.ExportData.Export.ExportFormat.Excel, SourceFile)
    End Sub

#End Region
    Public Class CacheManager
        Private Shared IISCache As System.Web.Caching.Cache = HttpRuntime.Cache
        Private Shared ExportInCorsoKey As String = "ExportInCorso"
        Private Shared AvanzamentoExportKey As String = "AvanzamentoExport"
        Private Shared Log As ILog = LogManager.GetLogger(GetType(CacheManager))

        Private Sub New()
            MyBase.New()
        End Sub

#Region "Export in corso"
        Public Shared Function GetExportInCorso() As Integer
            Try
                If (Not (IISCache(ExportInCorsoKey)) Is Nothing) Then
                    Return CType(IISCache(ExportInCorsoKey), Integer)
                Else
                    Return -1
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsExport.CacheManger.GetExportInCorso.errore: ", ex)
            End Try
        End Function
        Public Shared Sub SetExportInCorso(ByVal Anno As Integer)
            IISCache.Insert(ExportInCorsoKey, Anno, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
        End Sub
        Public Shared Sub RemoveExportInCorso()
            IISCache.Remove(ExportInCorsoKey)
        End Sub
#End Region
#Region "Avanzamento Export"
        Public Shared Function GetAvanzamentoExport() As String()
            Try
                If (Not (IISCache(AvanzamentoExportKey)) Is Nothing) Then
                    Return CType(IISCache(AvanzamentoExportKey), String())
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.clsExport.CacheManger.GetAvanzamentoExport.errore: ", ex)
                Return Nothing
            End Try
        End Function
        Public Shared Sub SetAvanzamentoExport(ByVal sMyDati() As String)
            IISCache.Insert(AvanzamentoExportKey, sMyDati, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
        End Sub
        Public Shared Sub RemoveAvanzamentoExport()
            IISCache.Remove(AvanzamentoExportKey)
        End Sub
#End Region
    End Class
End Class
