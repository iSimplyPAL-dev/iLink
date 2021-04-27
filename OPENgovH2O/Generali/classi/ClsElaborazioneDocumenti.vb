Imports OPENUtility
Imports RemotingInterfaceMotoreH2O.RemotingInterfaceMotoreH2O
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
Imports log4net
Imports RIBESElaborazioneDocumentiInterface
Imports AnagInterface
Imports Utility
Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip
Imports System.Xml
Imports System.Collections.Generic

Public Class ClsElaborazioneDocumenti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsElaborazioneDocumenti))
    Private iDB As New DBAccess.getDBobject
    Private iDBRepository As New DBAccess.getDBobject(ConstSession.StringConnectionOPENgov)
    Private ClsModificaDate As New ClsGenerale.Generale
    '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
    'Private Delegate Sub StampaMassivaAsync(ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer, ByVal bSendByMail As Boolean)
    Private Delegate Sub StampaMassivaAsync(ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, PathTemplate As String, PathTemplateVirtual As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer, ByVal bSendByMail As Boolean, IsSoloBollettino As Boolean)
    '*** ***
    'metodi per la gestione delle tabelle guida per l'elaborazione dei doc
    '*****************************************************
#Region "ELABORAZIONE DOC"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="nElab"></param>
    ''' <param name="nDaElab"></param>
    Public Sub GetNDoc(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByRef nElab As Integer, ByRef nDaElab As Integer)
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            dtMyDati = iDB.GetDataTable("prc_GetNDoc", New SqlClient.SqlParameter("@IdEnte", sIdEnte), New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", nIdRuolo))
            For Each dtMyRow In dtMyDati.Rows
                nElab = CInt(dtMyRow("docelaborati"))
                nDaElab = CInt(dtMyRow("docdaelaborare"))
            Next
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.GetNDoc.errore: ", Err)
        Finally
            dtMyDati.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="sIdEnte"></param>
    ''' <returns></returns>
    Public Function GetNumFileDocDaElaborare(ByVal nIdRuolo As Integer, ByVal sIdEnte As String) As Integer
        Dim nRet As Integer = 0
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            dtMyDati = iDBRepository.GetDataTable("prc_GeNumDocDaElaborare", New SqlClient.SqlParameter("@IdEnte", sIdEnte), New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", nIdRuolo))
            For Each dtMyRow In dtMyDati.Rows
                nRet = CInt(dtMyRow("nret"))
            Next
            Return nRet
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.GetNumFileDocDaElaborare.errore: ", Err)
            Return -1
        Finally
            dtMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ConnessioneRepository"></param>
    ''' <param name="ANNO"></param>
    ''' <param name="COD_ENTE"></param>
    ''' <param name="COD_TRIBUTO"></param>
    ''' <param name="sNOTE"></param>
    ''' <param name="IDFLUSSORUOLO"></param>
    ''' <returns></returns>
    Public Function InElaborazione(ByVal ConnessioneRepository As String, ByVal ANNO As String, ByVal COD_ENTE As String, ByVal COD_TRIBUTO As String, ByRef sNOTE As String, ByVal IDFLUSSORUOLO As Integer) As Integer
        Dim SelectCommand As New SqlClient.SqlCommand
        Dim objDR As SqlClient.SqlDataReader
        Dim bELABORAZIONE As Boolean = False, bERRORI As Boolean = False

        Try
            SelectCommand.CommandText = "SELECT * FROM TP_TASK_REPOSITORY"
            SelectCommand.CommandText += " WHERE COD_ENTE=@COD_ENTE"
            SelectCommand.CommandText += " AND COD_TRIBUTO=@COD_TRIBUTO"
            SelectCommand.CommandText += " AND ANNO=@ANNO"
            SelectCommand.CommandText += " AND IDFLUSSORUOLOTARSU=@IDFLUSSORUOLOTARSU"
            SelectCommand.CommandText += " ORDER BY ID_TASK_REPOSITORY DESC ,DATA_ELABORAZIONE DESC"

            SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_ENTE", SqlDbType.NVarChar)).Value = COD_ENTE
            SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_TRIBUTO", SqlDbType.NVarChar)).Value = COD_TRIBUTO
            SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = ANNO
            SelectCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSORUOLOTARSU", SqlDbType.Int)).Value = IDFLUSSORUOLO

            objDR = iDBRepository.GetDataReader(SelectCommand)
            If (objDR.HasRows) Then
                objDR.Read()
                bELABORAZIONE = Boolean.Parse(objDR("ELABORAZIONE").ToString())
                bERRORI = Boolean.Parse(objDR("ERRORI").ToString())
                If objDR("NOTE") Is DBNull.Value Then
                    sNOTE = ""
                Else
                    sNOTE = objDR("NOTE").ToString()
                End If
            End If
            objDR.Close()
            If Not bELABORAZIONE Then
                If (bERRORI) Then
                    Return 2
                Else
                    Return 0
                End If
            Else
                Return 1
            End If
        Catch Ex As Exception
            sNOTE = Ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.InElaborazione.errore: ", Ex)
            Return -1
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <returns></returns>
    Public Function GetDocDaElaborare(ByVal sIdEnte As String, ByVal nIdRuolo As Integer) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati()
        Dim nList As Integer
        Dim oListDocElab() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati
        Dim oDocElab As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        oListDocElab = Nothing
        Try
            dtMyDati = iDB.GetDataTable("prc_GetDocDaElaborare", New SqlClient.SqlParameter("@IdEnte", sIdEnte), New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", nIdRuolo))
            For Each dtMyRow In dtMyDati.Rows
                oDocElab = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati

                oDocElab.IdFlusso = nIdRuolo
                oDocElab.IdContribuente = CInt(dtMyRow("tipodocumento"))
                oDocElab.IdEnte = CStr(dtMyRow("IDENTE"))
                oDocElab.CodiceCartella = CStr(dtMyRow("CODICE_CARTELLA"))
                oDocElab.DataEmissione = CDate(dtMyRow("DATA_EMISSIONE"))
                oDocElab.IdModello = -1
                oDocElab.CampoOrdinamento = ""
                oDocElab.NumeroProgressivo = -1
                oDocElab.NumeroFile = -1
                oDocElab.Elaborato = False

                nList += 1
                ReDim Preserve oListDocElab(nList)
                oListDocElab(nList) = oDocElab
            Next

            Return oListDocElab
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.GetDocDaElaborare.errore: ", Err)
            Return Nothing
        Finally
            dtMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CodTributo"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="StringConnection"></param>
    ''' <param name="StringConnectionOPENgov"></param>
    ''' <param name="StringConnectionAnagrafica"></param>
    ''' <param name="PathTemplate"></param>
    ''' <param name="PathTemplateVirtual"></param>
    ''' <param name="nDocDaElaborare"></param>
    ''' <param name="nDocElaborati"></param>
    ''' <param name="nTipoElab"></param>
    ''' <param name="sTypeOrd"></param>
    ''' <param name="sNameModello"></param>
    ''' <param name="nMaxDocPerFile"></param>
    ''' <param name="bElabBollettini"></param>
    ''' <param name="oListAvvisi"></param>
    ''' <param name="oListDocStampati"></param>
    ''' <param name="bCreaPDF"></param>
    ''' <param name="nDecimal"></param>
    ''' <param name="TipoStampaBollettini"></param>
    ''' <param name="TipoBollettino"></param>
    ''' <param name="bSendByMail"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="10/12/2019"><strong>Fatturazione Elettronica</strong>
    ''' Con l’obbligo di far pervenire, per via telematica, tutte le fatture al Sistema di Interscambio (SdI), si rende necessaria l’implementazione dell’estrazione delle fatture sia verso il SdI sia verso il sistema gestionale Grand Combin.
    ''' La fase di creazione documenti viene modificata per generare il tracciato XML da inviare all’SdI e i tracciati XML da inviare al gestionale di Grand Combin. Al termine della creazione di questi flussi partirà la normale procedura di produzione documenti.
    ''' se si sta lanciando l’elaborazione effettiva, prima di richiamare la funzione asincrona per la stampa massiva, viene richiamata, in modalità sincrona, la funzione di generazione XML. La nuova funzione crea prima i flussi ministeriali e successivamente i flussi gestionali di Grand Combin. Se la procedura termina con successo viene richiamata l'elaborazione dei documenti come già in essere
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="05/11/2020">
    ''' devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
    ''' </revision>
    ''' </revisionHistory>
    Public Function ElaboraDocumenti(CodTributo As String, ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByVal sAnno As String, StringConnection As String, StringConnectionOPENgov As String, StringConnectionAnagrafica As String, PathTemplate As String, PathTemplateVirtual As String, ByVal nDocDaElaborare As Integer, ByVal nDocElaborati As Integer, ByVal nTipoElab As Integer, ByVal sTypeOrd As String, ByVal sNameModello As String, ByVal nMaxDocPerFile As Integer, ByVal bElabBollettini As Boolean, ByVal oListAvvisi() As ObjAnagDocumenti, ByRef oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL, ByVal bCreaPDF As Boolean, ByVal nDecimal As Integer, ByVal TipoStampaBollettini As String, ByVal TipoBollettino As String, ByVal bSendByMail As Boolean) As Integer
        Dim nFileDaElab, x, nList As Integer
        Dim sListCodCartella() As String = Nothing
        Dim IdDocToElab(,) As Integer
        Dim Esclusione() As String

        Try
            IdDocToElab = Nothing : Esclusione = Nothing
            '**************************************************************
            'devo risalire all'ultimo file usato per l'elaborazione effettiva in corso
            '**************************************************************
            nFileDaElab = GetNumFileDocDaElaborare(nIdRuolo, sIdEnte)
            If nFileDaElab <> -1 Then
                nFileDaElab += 1
            End If

            nList = -1
            If Not IsNothing(oListAvvisi) Then
                For x = 0 To oListAvvisi.GetUpperBound(0)
                    If oListAvvisi(x).Selezionato = True Then
                        nList += 1
                        ReDim Preserve sListCodCartella(nList)
                        sListCodCartella(nList) = oListAvvisi(x).sNDocumento
                        ReDim Preserve IdDocToElab(2, nList)
                        IdDocToElab(0, nList) = oListAvvisi(x).nIdDocumento
                        IdDocToElab(1, nList) = oListAvvisi(x).sVariato
                        IdDocToElab(2, nList) = oListAvvisi(x).sTipoDocumento
                    End If
                Next
                ' recupero i dati per la chiamata al servizio di elaborazione delle stampe
                Dim strConnessioneH2O As String = ConstSession.StringConnection

                Log.Debug("GestDocumenti::ElaboraDocumenti::connessione" & strConnessioneH2O)
                If nTipoElab = 1 Then
                    'elaborazione di prova
                    Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
                    oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
                    '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
                    Log.Debug("devo richiamare UrlServizioStampeICI::" & ConstSession.UrlServizioStampeICI)
                    Log.Debug("parametri::" & CodTributo & ",IdDocToElab," & sAnno & "," & sIdEnte & "," & nIdRuolo & ",Esclusione," & StringConnection & "," & StringConnectionOPENgov & "," & StringConnectionAnagrafica & "," & nMaxDocPerFile & ",PROVA," & TipoStampaBollettini & ",H2O," & TipoBollettino & "," & bElabBollettini & "," & bCreaPDF & "," & False & "," & nDecimal & "," & bSendByMail)
                    'oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, nMaxDocPerFile, "PROVA", TipoStampaBollettini, "H2O", TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail)
                    '**** 201810 - Calcolo puntuale ****
                    oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, PathTemplate, PathTemplateVirtual, nMaxDocPerFile, "PROVA", TipoStampaBollettini, "H2O", TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, False, "", CodTributo)
                    '*** ***
                Else
                    'elaborazione effettiva
                    If New clsFatturaElettronica(nIdRuolo, ConstSession.IdEnte, ConstSession.UserName).CreaXMLFatture() Then
                        Dim del As New StampaMassivaAsync(AddressOf ChiamaElaborazioneAsincrona)
                        '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
                        'del.BeginInvoke(ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, nMaxDocPerFile, "EFFETTIVO", TipoStampaBollettini, "H2O", TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, Nothing, Nothing)
                        del.BeginInvoke(CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, StringConnection, StringConnectionOPENgov, StringConnectionAnagrafica, PathTemplate, PathTemplateVirtual, nMaxDocPerFile, "EFFETTIVO", TipoStampaBollettini, "H2O", TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, False, Nothing, Nothing)
                        '*** ***
                        Return 2
                    Else
                        Return 0
                    End If
                End If
                Return 1
            Else
                Return 0
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.ElaboraDocumenti.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Tributo"></param>
    ''' <param name="IdDocToElab"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sIDEnte"></param>
    ''' <param name="IdFlussoRuolo"></param>
    ''' <param name="Esclusione"></param>
    ''' <param name="ConnessioneICI"></param>
    ''' <param name="ConnessioneRepository"></param>
    ''' <param name="ConnessioneAnagrafica"></param>
    ''' <param name="PathTemplate"></param>
    ''' <param name="PathTemplateVirtual"></param>
    ''' <param name="ContribuentiPerGruppo"></param>
    ''' <param name="TipoElaborazione"></param>
    ''' <param name="ImpostazioniBollettini"></param>
    ''' <param name="TipoCalcolo"></param>
    ''' <param name="TipoBollettino"></param>
    ''' <param name="bStampaBollettino"></param>
    ''' <param name="bCreaPDF"></param>
    ''' <param name="nettoVersato"></param>
    ''' <param name="nDecimal"></param>
    ''' <param name="bSendByMail"></param>
    ''' <param name="IsSoloBollettino"></param>
    ''' <revisionHistory>
    ''' <revision date="05/11/2020">
    ''' devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
    ''' </revision>
    ''' </revisionHistory>
    Private Sub ChiamaElaborazioneAsincrona(ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, PathTemplate As String, PathTemplateVirtual As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer, ByVal bSendByMail As Boolean, IsSoloBollettino As Boolean)
        Try
            '// faccio partire l'elaborazione
            '// chiamo il servizio di elaborazione delle stampe massive.
            Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
            oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
            'oElaborazioneDati.ElaborazioneMassivaDocumenti(Tributo, IdDocToElab, sAnno, sIDEnte, IdFlussoRuolo, Esclusione, ConnessioneICI, ConnessioneRepository, ConnessioneAnagrafica, ContribuentiPerGruppo, TipoElaborazione, ImpostazioniBollettini, TipoCalcolo, TipoBollettino, bStampaBollettino, bCreaPDF, nettoVersato, nDecimal, bSendByMail)
            '**** 201810 - Calcolo puntuale ****
            oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, Tributo, IdDocToElab, sAnno, sIDEnte, IdFlussoRuolo, Esclusione, ConnessioneICI, ConnessioneRepository, ConnessioneAnagrafica, PathTemplate, PathTemplateVirtual, ContribuentiPerGruppo, TipoElaborazione, ImpostazioniBollettini, TipoCalcolo, TipoBollettino, bStampaBollettino, bCreaPDF, nettoVersato, nDecimal, bSendByMail, IsSoloBollettino, "", Tributo)
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ChiamaElaborazioneAsincrona.GetNDoc.errore: ", Err)
            Throw Err
        End Try
    End Sub
#End Region
#Region "ELABORAZIONE OK"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdRuolo"></param>
    ''' <returns></returns>
    Public Function GetNumFileDocDaElaborare(ByVal nIdRuolo As Integer) As Integer
        Dim nidProgDoc As Integer = 0
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GeNumDocDaElaborare", "IDENTE", "IDFLUSSO_RUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                        , ctx.GetParam("IDFLUSSO_RUOLO", nIdRuolo)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                nidProgDoc = Utility.StringOperation.FormatInt(myRow("id_file"))
            Next
            dvMyDati.Dispose()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.GetNumFileDocDaElaborare.errore: ", Err)
            nidProgDoc = -1
        End Try
        Return nidProgDoc
    End Function
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sNomeTabella"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="cmdMyCommandOut"></param>
    ''' <returns></returns>
    Public Function DeleteTabGuidaComunico(ByVal sIdEnte As String, ByVal sNomeTabella As String, ByVal nIdRuolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_" + sNomeTabella + "_D"
            cmdMyCommand.ExecuteNonQuery()
            Return 1
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsElaborazioneDocumenti.DeleteTabGuidaComunico.errore: ", Err)
            Return 0
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Funzione per la conversione dell'oggetto fattura in oggetto per stampa
    ''' </summary>
    ''' <param name="oListAvvisi"></param>
    ''' <returns></returns>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory><revision date="16/03/2021">Aggiunta la possibilità di ristampa fattura</revision></revisionHistory>
    Public Function ConvAvvisi(ByVal oListAvvisi() As ObjFattura) As ObjAnagDocumenti()
        Dim oConv() As ObjAnagDocumenti = Nothing
        Dim oMyConv As ObjAnagDocumenti
        Dim nList As Integer

        Try
            nList = -1
            For Each myItem As ObjFattura In oListAvvisi
                oMyConv = New ObjAnagDocumenti
                oMyConv.sCognome = myItem.oAnagrafeUtente.Cognome
                oMyConv.sNome = myItem.oAnagrafeUtente.Nome
                oMyConv.nIdDocumento = myItem.Id
                oMyConv.sNDocumento = myItem.sNDocumento
                oMyConv.impDocumento = myItem.impFattura
                If myItem.oAnagrafeUtente.PartitaIva <> "" Then
                    oMyConv.sCodFiscalePIva = myItem.oAnagrafeUtente.PartitaIva
                Else
                    oMyConv.sCodFiscalePIva = myItem.oAnagrafeUtente.CodiceFiscale
                End If
                oMyConv.Selezionato = True
                oMyConv.sMatricola = myItem.sMatricola
                oMyConv.sPeriodo = myItem.sPeriodo
                If myItem.sTipoDocumento = "N" Then
                    oMyConv.sTipoDocumento = 2
                Else
                    oMyConv.sTipoDocumento = 1
                End If
                oMyConv.tDataDocumento = myItem.tDataDocumento
                oMyConv.sVariato = myItem.nIdUtente
                nList += 1
                ReDim Preserve oConv(nList)
                oConv(nList) = oMyConv
            Next
            Return oConv
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.ConvAvvisi.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class

#Region "Fatturazione Elettronica"
''' <summary>
''' classe per la gestione dei flussi per la fatturazione elettronica
''' </summary>
Public Class clsFatturaElettronica
    Private Shared Log As ILog = LogManager.GetLogger(GetType(clsFatturaElettronica))
    Private Enum TypeFile
        Nullo
        SdI
        Gestionale
    End Enum
    Private Const PathXML As String = "FatturaElettronica\"
    Private _nId As Integer
    Private _IdEnte As String
    Private _UserName As String
    ''' <summary>
    ''' funzione di inizializzazione
    ''' </summary>
    ''' <param name="nIdRuolo"></param>
    Public Sub New(nIdRuolo As Integer, myEnte As String, myUser As String)
        _nId = nIdRuolo
        _IdEnte = myEnte
        _UserName = myUser
    End Sub
    ''' <summary>
    ''' La funzione richiama due sotto routine distinte:
    ''' - Crea XML Ministeriale
    ''' - Crea XML Gestionale di Grand Combin
    ''' I tracciati per il gestionale interno vengono creati solo se la prima estrazione è andata a buon fine
    ''' Le routine restituiranno il nome dello zip estratto; i nomi degli zip estratti saranno registrati in una nuova tabella apposita in modo che ad ogni ingresso in videata il sistema presenti i flussi da scaricare.
    ''' </summary>
    ''' <returns></returns>
    ''' <revisionHistory><revision date="20210128">Aggiunto controlli dati mancanti pre estrazione</revision></revisionHistory>
    Public Function CreaXMLFatture() As Boolean
        Dim bRet As Boolean = False
        Dim sFileZip As String = ""
        Dim dvMyDati As New DataView
        Dim bDatiMancanti As Boolean = False
        Try
            Utility.Costanti.CreateDir(ConstSession.PathRepository + PathXML + _IdEnte + "\")
            dvMyDati = GetControlloDati()
            For Each myrow As DataRowView In dvMyDati
                If StringOperation.FormatString(myrow("Esito")) <> "" Then
                    Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.CreaXMLFatture.ControlloDati.Esito: " + StringOperation.FormatString(myrow("Esito")))
                    bDatiMancanti = True
                End If
            Next
            If bDatiMancanti = False Then
                sFileZip = GetXMLSdI()
                If sFileZip <> "" Then
                    If SetFileFattEle(TypeFile.SdI, sFileZip, Now, _UserName) Then
                        sFileZip = GetXMLGestionale()
                        If sFileZip <> "" Then
                            If SetFileFattEle(TypeFile.Gestionale, sFileZip, Now, _UserName) Then
                                bRet = True
                            End If
                        End If
                    End If
                End If
            Else
                bRet = False
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.CreaXMLFatture.errore: ", ex)
            bRet = False
        End Try
        Return bRet
    End Function
    ''' <summary>
    ''' funzione per la creazione dei singoli xml delle fatture in formato SdI
    ''' </summary>
    ''' <returns></returns>
    Private Function GetXMLSdI() As String
        Dim myFileName As String = ""
        Dim sXMLName As String = ""
        Dim ListFile As New ArrayList
        Dim dvMyDati As New DataView
        Dim fncSdI As New clsSdI(_IdEnte, _UserName)
        Dim IsPrivato, IsPubblico As Boolean

        Try
            IsPrivato = False : IsPubblico = False
            dvMyDati = GetFatture()
            For Each myrow As DataRowView In dvMyDati
                Dim myFatEle As New FatturaElettronicaType
                If StringOperation.FormatString(myrow("CodiceDestinatario")).Length = 7 Then
                    myFatEle.versione = FormatoTrasmissioneType.FPR12 'verso privati
                    IsPrivato = True
                Else
                    myFatEle.versione = FormatoTrasmissioneType.FPA12 'verso PA
                    IsPubblico = True
                End If
                sXMLName = StringOperation.FormatString(myrow("idpaese")) + StringOperation.FormatString(myrow("idtrasmittente")) + "_" + StringOperation.FormatString(myrow("yearletter")).PadLeft(1, " ") + StringOperation.FormatString(myrow("numerodocumento")).PadLeft(4, "0") + ".xml"
                myFileName = StringOperation.FormatString(myrow("idpaese")) + StringOperation.FormatString(myrow("idtrasmittente")) + "_" + StringOperation.FormatString(myrow("yearletter")).PadLeft(1, " ") + StringOperation.FormatString(myrow("idflussoruolo")).PadLeft(4, "0") + ".zip"
                myFatEle.FatturaElettronicaHeader = fncSdI.GetHeader(myrow)
                Log.Debug("scritto header devo scrivere body")
                myFatEle.FatturaElettronicaBody = fncSdI.GetBody(StringOperation.FormatInt(myrow("idcontribuente")), _nId)

                Using writer As New StreamWriter(ConstSession.PathRepository + PathXML + _IdEnte + "\" + sXMLName)
                    Dim serializer As New Serialization.XmlSerializer(myFatEle.GetType())
                    serializer.Serialize(writer, myFatEle)
                    writer.Flush()
                    writer.Close()
                End Using
                'divido zip per split e non split
                If IsPrivato = True And IsPubblico = True Then
                    If ZipFile(ConstSession.PathRepository + PathXML + _IdEnte + "\", myFileName.Replace(".zip", "Split.zip"), ListFile) = False Then
                        myFileName = ""
                    End If
                    ListFile.Clear() : IsPrivato = False : IsPubblico = False
                End If
                ListFile.Add(ConstSession.PathRepository + PathXML + _IdEnte + "\" + sXMLName)
            Next
            If ListFile.Count > 0 Then
                If ZipFile(ConstSession.PathRepository + PathXML + _IdEnte + "\", myFileName.Replace(".zip", "NoSplit.zip"), ListFile) = False Then
                    myFileName = ""
                End If
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.GetXMLSdI.errore: ", ex)
            myFileName = ""
        End Try
        Return myFileName
    End Function

    ''' <summary>
    ''' funzione per la creazione degli xml delle fatture per il gestionale
    ''' </summary>
    ''' <returns></returns>
    Private Function GetXMLGestionale() As String
        Dim myFileName As String = ""
        Dim sXMLName As String = ""
        Dim ListFile As New ArrayList
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            myFileName = _IdEnte + "_" + _nId.ToString + ".zip"
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                'H2ODatiFattura
                sXMLName = "H2ODatiFattura.xml"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GestionaleGetDatiFatture", "IDFLUSSORUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId))
                ctx.Dispose()
                If CreateXMLDatiFattura(dvMyDati, sXMLName) = False Then
                    Return ""
                End If
                ListFile.Add(ConstSession.PathRepository + PathXML + _IdEnte + "\" + sXMLName)
                'H2ODettaglioFattura
                sXMLName = "H2ODettaglioFattura.xml"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GestionaleGetDettaglioFatture", "IDFLUSSORUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId))
                ctx.Dispose()
                If CreateXMLDettaglioFattura(dvMyDati, sXMLName) = False Then
                    Return ""
                End If
                ListFile.Add(ConstSession.PathRepository + PathXML + _IdEnte + "\" + sXMLName)
                'H2ORiepilogoIVA
                sXMLName = "H2ORiepilogoIVA.xml"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GestionaleGetRiepilogoFatture", "IDFLUSSORUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId))
                ctx.Dispose()
                If CreateXMLRiepilogoIVA(dvMyDati, sXMLName) = False Then
                    Return ""
                End If
                ListFile.Add(ConstSession.PathRepository + PathXML + _IdEnte + "\" + sXMLName)
                'H2OScadenzeFattura
                sXMLName = "H2OScadenzeFattura.xml"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GestionaleGetScadenzeFatture", "IDFLUSSORUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId))
                ctx.Dispose()
                If CreateXMLScadenzeFattura(dvMyDati, sXMLName) = False Then
                    Return ""
                End If
                ListFile.Add(ConstSession.PathRepository + PathXML + _IdEnte + "\" + sXMLName)
            End Using

            If ZipFile(ConstSession.PathRepository + PathXML + _IdEnte + "\", myFileName, ListFile) = False Then
                myFileName = ""
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.GetXMLGestionale.errore: ", ex)
            myFileName = ""
        End Try
        Return myFileName
    End Function
    Private Function CreateXMLDatiFattura(dvMyDati As DataView, sFileName As String) As Boolean
        Dim bRet As Boolean = False
        Dim sLine As String = ""
        Dim fncGen As New ClsGenerale.Generale()
        Try
            Dim sPathNameFile As String = ConstSession.PathRepository + PathXML + _IdEnte + "\" + sFileName
            sLine = "<xml xmlns:s='uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:rs='urn:schemas-microsoft-com:rowset'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:z='#RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#Region "RowSetSchema"
            sLine = "<s:Schema id='RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<s:ElementType name='row' content='eltOnly' rs:updatable='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='IDMovimento' rs:number='1' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='i8' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='TipoMovimento' rs:number='2' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='int' dt:maxLength='4' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='FlagEU' rs:number='3' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='1' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='NumeroFattura' rs:number='4' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='53' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='DataFattura' rs:number='5' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='dateTime' rs:dbtype='variantdate' dt:maxLength='16' rs:precision='0' rs:fixedlength='true'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			 rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='AnnoMovimento' rs:number='6' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='int' dt:maxLength='4' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CausaleFattura' rs:number='7' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='255' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='ImportoFattura' rs:number='8' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceFiscaleCessionario' rs:number='9' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='16' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='RagioneSocialeCessionario' rs:number='10' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='80' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='IndirizzoCessionario' rs:number='11' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='80' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='ProvinciaCessionario' rs:number='12' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='3' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CAPCessionario' rs:number='13' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='10' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='c13' rs:name='CittàCessionario' rs:number='14' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='50' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='DataScadenzaFattura' rs:number='15' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='10' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='RegistrazioneFattura' rs:number='16' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='dateTime' rs:dbtype='variantdate' dt:maxLength='16' rs:precision='0' rs:fixedlength='true'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			 rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='ImportoBollo' rs:number='17' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='NumeroBollo' rs:number='18' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='1' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='PartitaIVACessionario' rs:number='19' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='16' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='NumeroTelefonoCessionario' rs:number='20' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='50' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='EmailCessionario' rs:number='21' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='50' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceIPAFatture' rs:number='22' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='7' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceAreaGestione' rs:number='23' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='10' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='DescrizioneAreaGestione' rs:number='24' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='100' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceCIG' rs:number='25' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='25' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceCUP' rs:number='26' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='15' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='NumeroOrdine' rs:number='27' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='1' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceIBAN' rs:number='28' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='27' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='PECCreditore' rs:number='29' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='255' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='EntePrivato' rs:number='30' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='boolean' dt:maxLength='2' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='PercorsoAllegato' rs:number='31' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='255' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='TipoIVA' rs:number='32' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='1' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:extends type='rs:rowbase'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	</s:ElementType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "</s:Schema>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
#Region "Data"
            sLine = "<rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<rs:insert>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            For Each myRow As DataRowView In dvMyDati
                sLine = "		<z:row IDMovimento='" + Utility.StringOperation.FormatString(myRow("IDMovimento")) + "'"
                sLine += " TipoMovimento='" + Utility.StringOperation.FormatString(myRow("TipoMovimento")) + "'"
                sLine += " FlagEU='" + Utility.StringOperation.FormatString(myRow("FlagEU")) + "'"
                sLine += " NumeroFattura='" + Utility.StringOperation.FormatString(myRow("NumeroFattura")) + "'"
                sLine += " DataFattura='" + Utility.StringOperation.FormatString(myRow("DataFattura")) + "'"
                sLine += " AnnoMovimento='" + Utility.StringOperation.FormatString(myRow("AnnoMovimento")) + "'"
                sLine += " CausaleFattura='" + Utility.StringOperation.FormatString(myRow("CausaleFattura")) + "'"
                sLine += " ImportoFattura='" + Utility.StringOperation.FormatString(myRow("ImportoFattura")) + "'"
                sLine += " CodiceFiscaleCessionario='" + Utility.StringOperation.FormatString(myRow("CodiceFiscaleCessionario")) + "'"
                sLine += " RagioneSocialeCessionario='" + Utility.StringOperation.FormatString(myRow("RagioneSocialeCessionario")) + "'"
                sLine += " IndirizzoCessionario='" + Utility.StringOperation.FormatString(myRow("IndirizzoCessionario")) + "'"
                sLine += " ProvinciaCessionario='" + Utility.StringOperation.FormatString(myRow("ProvinciaCessionario")) + "'"
                sLine += " CAPCessionario='" + Utility.StringOperation.FormatString(myRow("CAPCessionario")) + "'"
                sLine += " c13='" + Utility.StringOperation.FormatString(myRow("CittàCessionario")) + "'"
                sLine += " DataScadenzaFattura='" + Utility.StringOperation.FormatString(myRow("DataScadenzaFattura")) + "'"
                sLine += " RegistrazioneFattura='" + Utility.StringOperation.FormatString(myRow("RegistrazioneFattura")) + "'"
                sLine += " ImportoBollo='" + Utility.StringOperation.FormatString(myRow("ImportoBollo")) + "'"
                sLine += " NumeroBollo='" + Utility.StringOperation.FormatString(myRow("NumeroBollo")) + "'"
                sLine += " PartitaIVACessionario='" + Utility.StringOperation.FormatString(myRow("PartitaIVACessionario")) + "'"
                sLine += " NumeroTelefonoCessionario='" + Utility.StringOperation.FormatString(myRow("NumeroTelefonoCessionario")) + "'"
                sLine += " EmailCessionario='" + Utility.StringOperation.FormatString(myRow("EmailCessionario")) + "'"
                sLine += " CodiceIPAFatture='" + Utility.StringOperation.FormatString(myRow("CodiceIPAFatture")) + "'"
                sLine += " CodiceAreaGestione='" + Utility.StringOperation.FormatString(myRow("CodiceAreaGestione")) + "'"
                sLine += " DescrizioneAreaGestione='" + Utility.StringOperation.FormatString(myRow("DescrizioneAreaGestione")) + "'"
                sLine += " CodiceCIG='" + Utility.StringOperation.FormatString(myRow("CodiceCIG")) + "'"
                sLine += " CodiceCUP='" + Utility.StringOperation.FormatString(myRow("CodiceCUP")) + "'"
                sLine += " NumeroOrdine='" + Utility.StringOperation.FormatString(myRow("NumeroOrdine")) + "'"
                sLine += " CodiceIBAN='" + Utility.StringOperation.FormatString(myRow("CodiceIBAN")) + "'"
                sLine += " PECCreditore='" + Utility.StringOperation.FormatString(myRow("PECCreditore")) + "'"
                sLine += " EntePrivato='" + Utility.StringOperation.FormatString(myRow("EntePrivato")) + "'"
                sLine += " PercorsoAllegato='" + Utility.StringOperation.FormatString(myRow("PercorsoAllegato")) + "'"
                sLine += " TipoIVA='" + Utility.StringOperation.FormatString(myRow("TipoIVA")) + "'"
                sLine += "/>"
                If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                    Return False
                End If
            Next
            sLine = "	       	</rs:insert> "
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "  </rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
            sLine = "	</xml>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            bRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.CreateXMLDatiFattura.errore:  ", ex)
            bRet = False
        End Try
        Return bRet
    End Function
    Private Function CreateXMLDettaglioFattura(dvMyDati As DataView, sFileName As String) As Boolean
        Dim bRet As Boolean = False
        Dim sLine As String = ""
        Dim fncGen As New ClsGenerale.Generale()
        Try
            Dim sPathNameFile As String = ConstSession.PathRepository + PathXML + _IdEnte + "\" + sFileName
            sLine = "<xml xmlns:s='uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:rs='urn:schemas-microsoft-com:rowset'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:z='#RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#Region "RowSetSchema"
            sLine = "<s:Schema id='RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<s:ElementType name='row' content='eltOnly' rs:updatable='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='IDMovimento' rs:number='1' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='i8' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='PrezzoUnitarioRiga' rs:number='2' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='Iva2V' rs:number='3' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='DescrizioneRiga' rs:number='4' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='255' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='c4' rs:name='QuantitàRiga' rs:number='5' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='PrezzoTotaleRiga' rs:number='6' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='AliquotaIvaRiga' rs:number='7' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='CodiceAliquotaIvaRiga' rs:number='8' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='4' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='DataInizioPeriodo' rs:number='9' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='10' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='DataFinePeriodo' rs:number='10' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='string' rs:dbtype='str' dt:maxLength='10' rs:precision='0' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:extends type='rs:rowbase'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	</s:ElementType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "</s:Schema>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
#Region "Data"
            sLine = "<rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<rs:insert>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            For Each myRow As DataRowView In dvMyDati
                sLine = "		<z:row IDMovimento='" + Utility.StringOperation.FormatString(myRow("IDMovimento")) + "'"
                sLine += " PrezzoUnitarioRiga='" + Utility.StringOperation.FormatString(myRow("PrezzoUnitarioRiga")) + "'"
                sLine += " Iva2V='" + Utility.StringOperation.FormatString(myRow("Iva2V")) + "'"
                sLine += " DescrizioneRiga='" + Utility.StringOperation.FormatString(myRow("DescrizioneRiga")) + "'"
                sLine += " c4='" + Utility.StringOperation.FormatString(myRow("qta")) + "'"
                sLine += " PrezzoTotaleRiga='" + Utility.StringOperation.FormatString(myRow("PrezzoTotaleRiga")) + "'"
                sLine += " AliquotaIvaRiga='" + Utility.StringOperation.FormatString(myRow("AliquotaIvaRiga")) + "'"
                sLine += " CodiceAliquotaIvaRiga='" + Utility.StringOperation.FormatString(myRow("CodiceAliquotaIvaRiga")) + "'"
                sLine += " DataInizioPeriodo='" + Utility.StringOperation.FormatString(myRow("DataInizioPeriodo")) + "'"
                sLine += " DataFinePeriodo='" + Utility.StringOperation.FormatString(myRow("DataFinePeriodo")) + "'"
                sLine += "/>"
                If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                    Return False
                End If
            Next
            sLine = "	       	</rs:insert> "
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "  </rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
            sLine = "	</xml>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            bRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.CreateXMLDettaglioFattura.errore:  ", ex)
            bRet = False
        End Try
        Return bRet
    End Function
    Private Function CreateXMLRiepilogoIVA(dvMyDati As DataView, sFileName As String) As Boolean
        Dim bRet As Boolean = False
        Dim sLine As String = ""
        Dim fncGen As New ClsGenerale.Generale()
        Try
            Dim sPathNameFile As String = ConstSession.PathRepository + PathXML + _IdEnte + "\" + sFileName
            sLine = "<xml xmlns:s='uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:rs='urn:schemas-microsoft-com:rowset'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:z='#RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#Region "RowSetSchema"
            sLine = "<s:Schema id='RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<s:ElementType name='row' content='eltOnly' rs:updatable='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='IDMovimento' rs:number='1' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='i8' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='Imponibile' rs:number='2' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='Aliquota' rs:number='3' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='IVA' rs:number='4' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='Imposta' rs:number='5' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:extends type='rs:rowbase'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	</s:ElementType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "</s:Schema>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
#Region "Data"
            sLine = "<rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<rs:insert>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            For Each myRow As DataRowView In dvMyDati
                sLine = "		<z:row IDMovimento='" + Utility.StringOperation.FormatString(myRow("IDMovimento")) + "'"
                sLine += " Imponibile='" + Utility.StringOperation.FormatString(myRow("Imponibile")) + "'"
                sLine += " Aliquota='" + Utility.StringOperation.FormatString(myRow("Aliquota")) + "'"
                sLine += " IVA='" + Utility.StringOperation.FormatString(myRow("IVA")) + "'"
                sLine += " Imposta='" + Utility.StringOperation.FormatString(myRow("Imposta")) + "'"
                sLine += "/>"
                If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                    Return False
                End If
            Next
            sLine = "	       	</rs:insert> "
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "  </rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
            sLine = "	</xml>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            bRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.CreateXMLRiepilogoIVA.errore:  ", ex)
            bRet = False
        End Try
        Return bRet
    End Function
    Private Function CreateXMLScadenzeFattura(dvMyDati As DataView, sFileName As String) As Boolean
        Dim bRet As Boolean = False
        Dim sLine As String = ""
        Dim fncGen As New ClsGenerale.Generale()
        Try
            Dim sPathNameFile As String = ConstSession.PathRepository + PathXML + _IdEnte + "\" + sFileName
            sLine = "<xml xmlns:s='uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:rs='urn:schemas-microsoft-com:rowset'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	xmlns:z='#RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#Region "RowSetSchema"
            sLine = "<s:Schema id='RowsetSchema'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<s:ElementType name='row' content='eltOnly' rs:updatable='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='IDMovimento' rs:number='1' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='i8' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='ImportoRata' rs:number='2' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='float' dt:maxLength='8' rs:precision='0' rs:fixedlength='true' rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:AttributeType name='ScadenzaRata' rs:number='3' rs:write='true'>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			<s:datatype dt:type='dateTime' rs:dbtype='variantdate' dt:maxLength='16' rs:precision='0' rs:fixedlength='true'"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "			 rs:maybenull='false'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		</s:AttributeType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "		<s:extends type='rs:rowbase'/>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	</s:ElementType>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "</s:Schema>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
#Region "Data"
            sLine = "<rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "	<rs:insert>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            For Each myRow As DataRowView In dvMyDati
                sLine = "		<z:row IDMovimento='" + Utility.StringOperation.FormatString(myRow("IDMovimento")) + "'"
                sLine += " ImportoRata='" + Utility.StringOperation.FormatString(myRow("ImportoRata")) + "'"
                sLine += " ScadenzaRata='" + Utility.StringOperation.FormatString(myRow("ScadenzaRata")) + "'"
                sLine += "/>"
                If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                    Return False
                End If
            Next
            sLine = "	       	</rs:insert> "
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            sLine = "  </rs:data>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
#End Region
            sLine = "	</xml>"
            If fncGen.WriteFile(sPathNameFile, sLine) = 0 Then
                Return False
            End If
            bRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.CreateXMLScadenzeFattura.errore:  ", ex)
            bRet = False
        End Try
        Return bRet
    End Function
#Region "DB"
    ''' <summary>
    ''' Funzione per il salvataggio del flusso nell'apposita tabella
    ''' </summary>
    ''' <param name="TypeFile"></param>
    ''' <param name="sFile"></param>
    ''' <param name="dInsert"></param>
    ''' <param name="sOperatore"></param>
    ''' <returns></returns>
    Private Function SetFileFattEle(TypeFile As Integer, sFile As String, dInsert As DateTime, sOperatore As String) As Boolean
        Dim bRet As Boolean = False
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLZIPFATTUREELETTRONICHE_IU", "IDFLUSSORUOLO", "TIPO", "NAMEFILE", "DATA_INSERIMENTO", "OPERATORE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId) _
                        , ctx.GetParam("TIPO", TypeFile) _
                        , ctx.GetParam("NAMEFILE", sFile) _
                        , ctx.GetParam("DATA_INSERIMENTO", dInsert) _
                        , ctx.GetParam("OPERATORE", sOperatore)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                If StringOperation.FormatInt(myRow("id")) > 0 Then
                    bRet = True
                End If
            Next
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.SetFileFattEle.errore: ", ex)
            bRet = False
        End Try
        Return bRet
    End Function
    ''' <summary>
    ''' Funzione per la selezione dei flussi nell'apposita tabella
    ''' </summary>
    ''' <param name="TypeFile"></param>
    ''' <returns></returns>
    Private Function GetFileFattEle(TypeFile As Integer) As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLZIPFATTUREELETTRONICHE_S", "IDFLUSSORUOLO", "TIPO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId) _
                        , ctx.GetParam("TIPO", TypeFile)
                    )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.GetFileFattEle.errore: ", ex)
            dvMyDati = New DataView
        End Try
        Return dvMyDati
    End Function
    ''' <summary>
    ''' Funzione per la selezione delle fatture da estrarre
    ''' </summary>
    ''' <returns></returns>
    Private Function GetFatture() As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SdIGetFatture", "IDFLUSSORUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId))
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.GetFatture.errore: ", ex)
            dvMyDati = New DataView
        End Try
        Return dvMyDati
    End Function
    ''' <summary>
    ''' Funzione per il controllo della presenza dei dati obbligatori mancanti
    ''' </summary>
    ''' <returns></returns>
    Private Function GetControlloDati() As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SdIControlloDati", "IDFLUSSORUOLO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSORUOLO", _nId))
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovH2O.clsFatturaElettronica.ControlloDati.errore: ", ex)
            dvMyDati = New DataView
        End Try
        Return dvMyDati
    End Function
#End Region
    ''' <summary>
    ''' funzione per la creazione di uno zip
    ''' </summary>
    ''' <param name="SourceDir"></param>
    ''' <param name="ZipFileName"></param>
    ''' <param name="ListFile"></param>
    ''' <returns></returns>
    Public Function ZipFile(SourceDir As String, ZipFileName As String, ListFile As ArrayList) As Boolean
        Try
            Dim astrFileNames() As String = Directory.GetFiles(SourceDir)
            Dim strmZipOutputStream As New ZipOutputStream(File.Create(SourceDir + ZipFileName))
            For Each myfile As String In astrFileNames
                For Each NameFile As String In ListFile
                    If myfile.ToLower = NameFile.ToLower Then
                        Dim entry As New ZipEntry(Path.GetFileName(myfile))
                        entry.DateTime = DateTime.Now
                        strmZipOutputStream.PutNextEntry(entry)
                        strmZipOutputStream.Write(File.ReadAllBytes(myfile), 0, File.ReadAllBytes(myfile).Length)
                        Exit For
                    End If
                Next
            Next
            strmZipOutputStream.Finish()
            strmZipOutputStream.Close()
            Return True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.ZipFile.errore: ", ex)
            Return False
        End Try
    End Function

    Public Class clsSdI
        Private _IdEnte As String
        Private _Username As String
        Public Sub New(myEnte As String, myUser As String)
            _IdEnte = myEnte
            _Username = myUser
        End Sub

        Public Enum TypeAnagrafe
            Nullo
            Cedente
            Cessionario
        End Enum
#Region "Header"
        Public Function GetHeader(myRow As DataRowView) As FatturaElettronicaHeaderType
            Dim myItem As New FatturaElettronicaHeaderType
            Try
                myItem.DatiTrasmissione = GetDatiTrasmissione(myRow)
                myItem.CedentePrestatore = GetCedentePrestatore(myRow)
                myItem.CessionarioCommittente = GetCessionarioCommittente(myRow)
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetHeader.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetHeader" + ex.Message)
            End Try
            Return myItem
        End Function
#Region "DatiTrasmissione"
        Private Function GetDatiTrasmissione(myRow As DataRowView) As DatiTrasmissioneType
            Dim myItem As New DatiTrasmissioneType
            Try
                myItem.IdTrasmittente = GetIdFiscale(myRow, TypeAnagrafe.Nullo)
                myItem.ProgressivoInvio = StringOperation.FormatString(myRow("proginvio")).PadLeft(5, "0")
                If StringOperation.FormatString(myRow("CodiceDestinatario")).Length = 7 Then
                    myItem.FormatoTrasmissione = FormatoTrasmissioneType.FPR12 'verso privati
                Else
                    myItem.FormatoTrasmissione = FormatoTrasmissioneType.FPA12 'verso PA
                End If
                myItem.CodiceDestinatario = StringOperation.FormatString(myRow("CodiceDestinatario")) 'nei casi di fattura destinata ad un soggetto per il quale non si conosce il canale telematico (PEC o altro) sul quale recapitare il File;
                '"XXXXXXX" nei casi di fattura emessa verso soggetti non residenti, non stabiliti, non identificati In Italia, e inviata al Sistema di Interscambio al fine di trasmettere i dati di tali fatture.
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiTrasmissione.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiTrasmissione" + ex.Message)
            End Try
            Return myItem
        End Function
#End Region
#Region "CedentePrestatore"
        Private Function GetCedentePrestatore(myRow As DataRowView) As CedentePrestatoreType
            Dim myItem As New CedentePrestatoreType

            Try
                myItem.DatiAnagrafici = GetDatiAnagraficiCedente(myRow)
                myItem.Sede = GetIndirizzo(myRow, TypeAnagrafe.Cedente)
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetCedentePrestatore.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetCedentePrestatore" + ex.Message)
            End Try
            Return myItem
        End Function
        Private Function GetDatiAnagraficiCedente(myRow As DataRowView) As DatiAnagraficiCedenteType
            Dim myItem As New DatiAnagraficiCedenteType

            Try
                myItem.IdFiscaleIVA = GetIdFiscale(myRow, TypeAnagrafe.Cedente)
                If StringOperation.FormatString(myRow("cfcedente")) <> StringOperation.FormatString(myRow("pivacedente")) Then
                    myItem.CodiceFiscale = StringOperation.FormatString(myRow("cfcedente"))
                End If
                myItem.Anagrafica = GetAnagrafica(myRow, TypeAnagrafe.Cedente)
                myItem.RegimeFiscale = RegimeFiscaleType.RF01
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiAnagraficiCedente.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiAnagraficiCedente" + ex.Message)
            End Try
            Return myItem
        End Function
#End Region
#Region "CessionarioCommittente"
        Private Function GetCessionarioCommittente(myRow As DataRowView) As CessionarioCommittenteType
            Dim myItem As New CessionarioCommittenteType

            Try
                myItem.DatiAnagrafici = GetDatiAnagraficiCessionario(myRow)
                myItem.Sede = GetIndirizzo(myRow, TypeAnagrafe.Cessionario)
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetCessionarioCommittente.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetCessionarioCommittente" + ex.Message)
            End Try
            Return myItem
        End Function
        Private Function GetDatiAnagraficiCessionario(myRow As DataRowView) As DatiAnagraficiCessionarioType
            Dim myItem As New DatiAnagraficiCessionarioType

            Try
                If StringOperation.FormatString(myRow("sessofiscale")) = "G" Then
                    myItem.IdFiscaleIVA = GetIdFiscale(myRow, TypeAnagrafe.Cessionario)
                Else
                    myItem.CodiceFiscale = StringOperation.FormatString(myRow("cfpivacessionario"))
                End If
                myItem.Anagrafica = GetAnagrafica(myRow, TypeAnagrafe.Cessionario)
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiAnagraficiCessionario.errore:  ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiAnagraficiCessionario" + ex.Message)
            End Try
            Return myItem
        End Function
#End Region
#End Region
#Region "Body"
        Public Function GetBody(nIdContribuente As Integer, nIdFlussoRuolo As Integer) As FatturaElettronicaBodyType()
            Dim ListItem As New ArrayList
            Dim myItem As New FatturaElettronicaBodyType
            Dim dvMyDati As New DataView
            Dim sSQL As String = ""

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SdIGetBody", "IDCONTRIBUENTE", "IDFLUSSORUOLO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDCONTRIBUENTE", nIdContribuente) _
                            , ctx.GetParam("IDFLUSSORUOLO", nIdFlussoRuolo)
                        )
                    ctx.Dispose()
                End Using
                For Each myRow As DataRowView In dvMyDati
                    Log.Debug("GetDatiGenerali")
                    myItem.DatiGenerali = GetDatiGenerali(myRow)
                    Log.Debug("GetBeniServizi")
                    myItem.DatiBeniServizi = GetBeniServizi(myRow)
                    myItem.DatiPagamento = GetDatiPagamento(StringOperation.FormatInt(myRow("iddoc")))
                    ListItem.Add(myItem)
                Next
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetBody.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetBody" + ex.Message)
            End Try
            Return CType(ListItem.ToArray(GetType(FatturaElettronicaBodyType)), FatturaElettronicaBodyType())
        End Function
#Region "DatiGenerali"
        Private Function GetDatiGenerali(myRow As DataRowView) As DatiGeneraliType
            Dim myItem As New DatiGeneraliType
            Try
                myItem.DatiGeneraliDocumento = GetDatiGeneraliDocumento(myRow)
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiGenerali.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiGenerali" + ex.Message)
            End Try
            Return myItem
        End Function
        Private Function GetDatiGeneraliDocumento(myRow As DataRowView) As DatiGeneraliDocumentoType
            Dim myItem As New DatiGeneraliDocumentoType
            Dim ListCausale As New ArrayList
            Try
                If StringOperation.FormatString(myRow("tipodocumento")) = "N" Then
                    myItem.TipoDocumento = TipoDocumentoType.TD04
                Else
                    myItem.TipoDocumento = TipoDocumentoType.TD01
                End If
                myItem.Divisa = StringOperation.FormatString(myRow("divisa"))
                myItem.Data = StringOperation.FormatDateTime(myRow("datadocumento"))
                myItem.Numero = StringOperation.FormatString(myRow("numerodocumento"))
                myItem.ImportoTotaleDocumento = StringOperation.FormatDouble(myRow("imptotale")).ToString("0.00").Replace(",", ".")
                myItem.ImportoTotaleDocumentoSpecified = True
                myItem.Arrotondamento = StringOperation.FormatDouble(myRow("imparrotondamento")).ToString("0.00").Replace(",", ".")
                myItem.ArrotondamentoSpecified = True
                If StringOperation.FormatString(myRow("causale")).Length > 200 Then
                    ListCausale.Add(StringOperation.FormatString(myRow("causale")).Substring(0, 200))
                    ListCausale.Add(StringOperation.FormatString(myRow("causale")).Substring(200))
                Else
                    ListCausale.Add(StringOperation.FormatString(myRow("causale")))
                End If
                myItem.Causale = CType(ListCausale.ToArray(GetType(String)), String())
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiGeneraliDocumento.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiGeneraliDocumento" + ex.Message)
            End Try
            Return myItem
        End Function

#End Region
#Region "DatiBeniServizi"
        Private Function GetBeniServizi(myRow As DataRowView) As DatiBeniServiziType
            Dim myItem As New DatiBeniServiziType
            Try
                Log.Debug("GetDatiDettaglioLinee")
                myItem.DettaglioLinee = GetDatiDettaglioLinee(StringOperation.FormatInt(myRow("iddoc")))
                Log.Debug("GetDatiRiepilogo")
                myItem.DatiRiepilogo = GetDatiRiepilogo(StringOperation.FormatInt(myRow("iddoc")))
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetBeniServizi.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetBeniServizi" + ex.Message)
            End Try
            Return myItem
        End Function
        Private Function GetDatiDettaglioLinee(nIdDoc As Integer) As DettaglioLineeType()
            Dim ListItem As New ArrayList
            Dim myItem As New DettaglioLineeType
            Dim dvMyDati As New DataView
            Dim sSQL As String = ""

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SdIGetDettaglioLinee", "IDDOC")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDDOC", nIdDoc))
                    ctx.Dispose()
                End Using
                For Each myRow As DataRowView In dvMyDati
                    myItem = New DettaglioLineeType
                    myItem.NumeroLinea = StringOperation.FormatString(myRow("nriga"))
                    myItem.Descrizione = StringOperation.FormatString(myRow("descrizione"))
                    myItem.Quantita = StringOperation.FormatDouble(myRow("qta")).ToString("0.00").Replace(",", ".")
                    myItem.QuantitaSpecified = True
                    myItem.PrezzoUnitario = StringOperation.FormatDouble(myRow("tariffa")).ToString("0.000000").Replace(",", ".")
                    myItem.PrezzoTotale = StringOperation.FormatDouble(myRow("implinea")).ToString("0.00").Replace(",", ".")
                    myItem.AliquotaIVA = StringOperation.FormatDouble(myRow("iva")).ToString("0.00").Replace(",", ".")
                    If StringOperation.FormatString(myRow("natura")) <> "" Then
                        myItem.NaturaSpecified = True
                        myItem.Natura = NaturaType.N4
                    End If
                    ListItem.Add(myItem)
                Next
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiDettaglioLinee.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiDettaglioLinee" + ex.Message)
            End Try
            Return CType(ListItem.ToArray(GetType(DettaglioLineeType)), DettaglioLineeType())
        End Function
        Private Function GetDatiRiepilogo(nIdDoc As Integer) As DatiRiepilogoType()
            Dim ListItem As New ArrayList
            Dim myItem As New DatiRiepilogoType
            Dim dvMyDati As New DataView
            Dim sSQL As String = ""

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SdIGetDatiRiepilogo", "IDDOC")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDDOC", nIdDoc))
                    ctx.Dispose()
                End Using
                For Each myRow As DataRowView In dvMyDati
                    myItem.AliquotaIVA = StringOperation.FormatDouble(myRow("iva")).ToString("0.00").Replace(",", ".")
                    If StringOperation.FormatString(myRow("natura")) <> "" Then
                        myItem.NaturaSpecified = True
                        myItem.Natura = NaturaType.N4
                    End If
                    myItem.ImponibileImporto = StringOperation.FormatDouble(myRow("impimponibile")).ToString("0.00").Replace(",", ".")
                    myItem.Imposta = StringOperation.FormatDouble(myRow("impiva")).ToString("0.00").Replace(",", ".")
                    If StringOperation.FormatString(myRow("EsigibilitaIVA")) <> "" Then
                        myItem.EsigibilitaIVASpecified = True
                        myItem.EsigibilitaIVA = EsigibilitaIVAType.S
                        myItem.RiferimentoNormativo = StringOperation.FormatString(myRow("RiferimentoNormativo"))
                    End If
                    ListItem.Add(myItem)
                Next
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiRiepilogo.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiRiepilogo" + ex.Message)
            End Try
            Return CType(ListItem.ToArray(GetType(DatiRiepilogoType)), DatiRiepilogoType())
        End Function
#End Region
#Region "DatiPagamento"
        Private Function GetDatiPagamento(nIdDoc As Integer) As DatiPagamentoType()
            Dim ListItem As New ArrayList
            Dim myItem As New DatiPagamentoType

            Try
                myItem.CondizioniPagamento = CondizioniPagamentoType.TP02
                myItem.DettaglioPagamento = GetDettaglioPagamento(nIdDoc)
                If myItem.DettaglioPagamento.GetLength(0) > 0 Then
                    ListItem.Add(myItem)
                End If
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDatiPagamento.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDatiPagamento" + ex.Message)
            End Try
            Return CType(ListItem.ToArray(GetType(DatiPagamentoType)), DatiPagamentoType())
        End Function
        Private Function GetDettaglioPagamento(nIdDoc As Integer) As DettaglioPagamentoType()
            Dim ListItem As New ArrayList
            Dim myItem As New DettaglioPagamentoType
            Dim dvMyDati As New DataView
            Dim sSQL As String = ""

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SdIGetPagamento", "IDDOC")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDDOC", nIdDoc))
                    ctx.Dispose()
                End Using
                For Each myRow As DataRowView In dvMyDati
                    myItem.Beneficiario = StringOperation.FormatString(myRow("Beneficiario"))
                    If StringOperation.FormatString(myRow("tipopagamento")) = "MP05" Then
                        myItem.ModalitaPagamento = ModalitaPagamentoType.MP05
                        myItem.IBAN = StringOperation.FormatString(myRow("IBAN"))
                        myItem.ABI = StringOperation.FormatString(myRow("ABI"))
                        myItem.CAB = StringOperation.FormatString(myRow("CAB"))
                        myItem.BIC = StringOperation.FormatString(myRow("BIC"))
                    Else
                        myItem.ModalitaPagamento = ModalitaPagamentoType.MP18
                    End If
                    myItem.DataRiferimentoTerminiPagamento = StringOperation.FormatDateTime(myRow("DataRiferimentoTerminiPagamento"))
                    myItem.DataRiferimentoTerminiPagamentoSpecified = True
                    myItem.GiorniTerminiPagamento = StringOperation.FormatInt(myRow("GiorniTerminiPagamento"))
                    myItem.DataScadenzaPagamento = StringOperation.FormatDateTime(myRow("DataScadenzaPagamento"))
                    myItem.DataScadenzaPagamentoSpecified = True
                    myItem.ImportoPagamento = StringOperation.FormatDouble(myRow("ImportoPagamento")).ToString("F")
                    ListItem.Add(myItem)
                Next
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetDettaglioPagamento.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetDettaglioPagamento" + ex.Message)
            End Try
            Return CType(ListItem.ToArray(GetType(DettaglioPagamentoType)), DettaglioPagamentoType())
        End Function
#End Region
#End Region
        Private Function GetIdFiscale(myRow As DataRowView, nType As Integer) As IdFiscaleType
            Dim myItem As New IdFiscaleType
            Dim Suffisso As String = ""
            Dim Prefisso As String = "cfpiva"
            Try
                Select Case nType
                    Case TypeAnagrafe.Cedente
                        Suffisso = "Cedente"
                        If StringOperation.FormatString(myRow("cfcedente")) <> StringOperation.FormatString(myRow("pivacedente")) Then
                            Prefisso = "piva"
                        End If
                    Case TypeAnagrafe.Cessionario
                        Suffisso = "Cessionario"
                    Case TypeAnagrafe.Nullo
                        Suffisso = "Cedente"
                End Select
                myItem.IdPaese = StringOperation.FormatString(myRow("idpaese"))
                myItem.IdCodice = StringOperation.FormatString(myRow(Prefisso + Suffisso))
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetIdFiscale.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetIdFiscale" + ex.Message)
            End Try
            Return myItem
        End Function
        Private Function GetAnagrafica(myRow As DataRowView, nType As Integer) As AnagraficaType
            Dim myItem As New AnagraficaType
            Dim ListChoise As New List(Of ItemsChoiceType)
            Dim ListValue As New List(Of String)
            Dim Suffisso As String
            Try
                Select Case nType
                    Case TypeAnagrafe.Cedente
                        Suffisso = "Cedente"
                    Case TypeAnagrafe.Cessionario
                        Suffisso = "Cessionario"
                    Case Else
                        Suffisso = ""
                End Select
                If StringOperation.FormatString(myRow("sesso" + Suffisso)) = "G" Then
                    ListChoise.Add(ItemsChoiceType.Denominazione)
                    ListValue.Add(StringOperation.FormatString(myRow("cognome" + Suffisso)))
                Else
                    ListChoise.Add(ItemsChoiceType.Nome)
                    ListValue.Add(StringOperation.FormatString(myRow("nome" + Suffisso)))
                    ListChoise.Add(ItemsChoiceType.Cognome)
                    ListValue.Add(StringOperation.FormatString(myRow("cognome" + Suffisso)))
                End If
                myItem.ItemsElementName = ListChoise.ToArray()
                myItem.Items = ListValue.ToArray()
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetAnagrafica.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetAnagrafica" + ex.Message)
            End Try
            Return myItem
        End Function
        Private Function GetIndirizzo(myRow As DataRowView, nType As Integer) As IndirizzoType
            Dim myItem As New IndirizzoType
            Dim Suffisso As String
            Try
                Select Case nType
                    Case TypeAnagrafe.Cedente
                        Suffisso = "Cedente"
                    Case TypeAnagrafe.Cessionario
                        Suffisso = "Cessionario"
                    Case Else
                        Suffisso = ""
                End Select
                myItem.Indirizzo = StringOperation.FormatString(myRow("indirizzo" + Suffisso))
                myItem.CAP = StringOperation.FormatString(myRow("cap" + Suffisso))
                myItem.Comune = StringOperation.FormatString(myRow("comune" + Suffisso))
                If StringOperation.FormatString(myRow("pv" + Suffisso)) <> "" Then
                    myItem.Provincia = StringOperation.FormatString(myRow("pv" + Suffisso))
                End If
                myItem.Nazione = StringOperation.FormatString(myRow("nazione" + Suffisso))
            Catch ex As Exception
                Log.Debug(_IdEnte + "." + _Username + " - OPENgovH2O.clsFatturaElettronica.clsSdI.GetIndirizzo.errore: ", ex)
                Throw New Exception("clsFatturaElettronica.clsSdI.GetIndirizzo" + ex.Message)
            End Try
            Return myItem
        End Function
    End Class
End Class
#End Region
