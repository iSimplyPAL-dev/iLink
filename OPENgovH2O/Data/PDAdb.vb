Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Namespace OpenUtenze
  'La classe Gestisce il carico/scarico Da PDA
  Public Class PDAdb

	Dim clsDBSql As New DBAccess.getDBobject()
        Dim ClsData As New ClsGenerale.Generale
	Protected lngDataSotituzione, _
	lngCodPDA, _
	lngDataCessazione, _
	lngCodContatoreSucc, _
	lngCodContatore, _
	lngCodContatorePrec, _
	   lngConsumoEffettivo, _
	   lngGiorniDiConsumo, _
	  lngConsumoTeorico, lngLetturaIncongruente As Long

	Enum TypeData
	  DATASOSTITUZIONE = 1
	  DATACESSAZIONE = 2
	End Enum
	Public lngIdPeriodo As Long
	Dim blnInconguente As Boolean
	Dim strNameTable As String

	'Carica la lista dei Palmari
#Region "GetPDA"
	'==============================================================
	Public Function GetPDA() As SqlDataReader
	  Dim strSql As String

	  strSql = "SELECT * FROM TP_PALMARI ORDER BY CODICE"
	  ' Create Instance of Connection and Command Object
	  Dim result As SqlDataReader = clsDBSql.GetDataReader(strSql)

	  ' Return the datareader result
	  Return result

	End Function
#End Region
	'*********************************************************************************
	'Ritorna il Dettaglio del Palmare selezionato
#Region "GetDetailsPDA"
	Public Function GetDetailsPDA(ByVal strCodice As String) As SqlDataReader
	  Dim strSql As String


	  strSql = "SELECT * FROM TP_PALMARI WHERE CODICE=" & strCodice
	  ' Create Instance of Connection and Command Object
	  Dim result As SqlDataReader = clsDBSql.GetDataReader(strSql)

	  ' Return the datareader result
	  Return result

	End Function
#End Region
	'*********************************************************************************
	'Ritorna i dati contenuti nella TAbella di Access che e stata caricata
	'dopo la sincronizzazione con il PDA


	Public Function GetLetturePDA(ByVal strCodPDA As String) As OleDbDataReader

	  Dim strSql, _
	  strOleDBString As String
	
	  Dim _dr As SqlDataReader

	  _dr = GetDetailsPDA(strCodPDA)
	  If _dr.Read Then
                strNameTable = MyUtility.GetParametro(_dr.Item("NOMETABELLA"))
            End If
            _dr.Close()

            strOleDBString = ConfigurationSettings.AppSettings("connStringDBAccess")
            Dim clsDBAcces As New DBAccess.getDBobject(strOleDBString)
            strSql = "SELECT * FROM " & strNameTable & " ORDER BY COD_CONTATORE"
            Dim result As OleDbDataReader = clsDBAcces.GetDataReaderAccess(strSql)

            Return result

        End Function


        '*********************************************************************************
        'Function CaricoDAPDA

        Public Function CaricoDaPDA(ByVal strCodPDA As String) As String


            Dim _drAccess As OleDbDataReader = GetLetturePDA(strCodPDA)

            While _drAccess.Read

                lngCodContatore = MyUtility.cTolng(_drAccess.Item("COD_CONTATORE"))
                'Verifico se Esite il contatore
                If GetContatoreOpenUtenze(MyUtility.GetParametro(lngCodContatore), _
                 lngDataCessazione, _
                 lngDataSotituzione, _
                 lngCodPDA, _
                 lngCodContatoreSucc) Then ControlValidate(_drAccess) Else setTransito(lngCodContatore, _drAccess)

            End While

            _drAccess.Close()

            Dim oleDBConn As New OleDbConnection
            Dim OleDbCommand As New OleDbCommand
            Dim strSql As String
            oleDBConn.ConnectionString = ConfigurationSettings.AppSettings("connStringDBAccess")
            oleDBConn.Open()

            strSql = ""
            strSql = "DELETE FROM " & strNameTable

            OleDbCommand.CommandType = CommandType.Text
            OleDbCommand.CommandText = strSql
            OleDbCommand.Connection = oleDBConn
            OleDbCommand.ExecuteNonQuery()

            oleDBConn.Close()

            strSql = ""
            strSql = "DELETE FROM " & strNameTable
            clsDBSql.RunActionQuery(strSql)

        End Function

        '*******************************************************
        ' PDAdb.GetContatoreOpenUtenze() Function (Protected)
        '
        '  GetContatoreOpenUtenze  Function ritorna i campi necessari per i Controlli 
        ' database openUtenze Tabella TP_CONTATORI.
        '
        ' La Funzione è Interna alla Classe (Protected)
        '   
        '*******************************************************
        Protected Function GetContatoreOpenUtenze(ByVal strIDContatoreFromPDA As String, _
          ByRef lngDataCessazione As Long, _
          ByRef lngDataSotituzione As Long, _
          ByRef lngCodPDA As Long, _
          ByRef lngCodContatoreSucc As Long) As Boolean

            Dim strSql As String
            GetContatoreOpenUtenze = False

            Dim result As SqlDataReader = clsDBSql.RunSPReturnRS("DetailContatori", _
             New SqlParameter("@CodContatore", strIDContatoreFromPDA))

            If result.Read Then
                'Restituisce i Valori Che servono per il Confronto
                lngDataSotituzione = MyUtility.cTolng(result.Item("DATASOSTITUZIONE"))
                lngDataCessazione = MyUtility.cTolng(result.Item("DATACESSAZIONE"))
                lngCodPDA = MyUtility.cTolng(result.Item("CODPDA"))
                lngCodContatoreSucc = MyUtility.cTolng(result.Item("CODCONTATORESUCCESSIVO"))
                GetContatoreOpenUtenze = True
            End If

            result.Close()

        End Function

        '*******************************************************
        ' PDAdb.addTpLetture() Method (Protected)
        '
        '  addTpLetture method aggiunge una nuova Lettura nel 
        ' database openUtenze Tabella TP_LETTUre.
        '
        ' Il Metodo è Interno Alla Classe (Protected)
        '   
        '*******************************************************
        Protected Sub ControlValidate(ByVal drTemp As OleDbDataReader)
            Dim _dr As SqlDataReader

            '****************************Data Sostituzione*****************************************
            GetDataSostituzione(lngDataSotituzione, _
            drTemp)
            '******************************Data Cessazione****************************************
            GetDataCessazione(lngDataCessazione, _
            drTemp)
            '************************************************************************************
            'Verifico se è presente già una lettura per il contatore in questione e per il periodo in questione ----> se si scrivo nel Transito 

            Dim result As SqlDataReader = clsDBSql.RunSPReturnRS("DetailLetture", New SqlParameter("@CODCONTATORE ", lngCodContatore), _
            New SqlParameter("@CODPERIODO ", lngIdPeriodo))

            If result.Read Then
                setTransito(lngCodContatore, drTemp)
                result.Close()
                Exit Sub
            End If
            'verifica se il codpda è eguale al codpda della tabella contatori di OpenUtenze -----> se no scrive nel Transito 
            If lngCodPDA <> MyUtility.cTolng(drTemp.Item("CODICE_PDA")) Then
                setTransito(lngCodContatore, drTemp)
                Exit Sub
            End If

            'verifica dei campi lettura anomalie lasciato avviso se nulli -----> scrive nel Transito 
            '********************************************************************************************
            If Not drTemp.Item("DATA_LET_ATTUALE") Is DBNull.Value Then
                If MyUtility.cTolng(drTemp.Item("COD_ANOMALIA1")) = 0 And MyUtility.cTolng(drTemp.Item("COD_ANOMALIA2")) = 0 And MyUtility.cTolng(drTemp.Item("COD_ANOMALIA3")) = 0 Then
                    If MyUtility.cTolng(drTemp.Item("LET_ATTUALE")) = 0 Then
                        If MyUtility.CToBit(drTemp.Item("LASCIATO_AVVISO")) = 0 Then
                            setTransito(lngCodContatore, drTemp)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            '********************************************************************************************

            'Aggiorno la tabella contatori letto=true 
            Dim clsLetture As New clsLetture

            clsLetture.CalcolaConsumoEffettivo(drTemp.Item("codlettura"), MyUtility.GetParametro(drTemp.Item("LET_ATTUALE")), lngConsumoEffettivo, _
             MyUtility.GetParametro(drTemp.Item("CODICE_UTENTE")), _
           MyUtility.GetParametro(lngCodContatore))

            'Calcolo consumo Teorico
            '*********************************************************************************************
            'Verifica se il Contatore in Questione ha un contatore Precedente 
            Dim dblConsumoTeorico, _
             dblResult, _
             dblMediaConsumo, dblRapportoCGG As Double
            Dim lngRecordCount As Long


            _dr = clsDBSql.RunSPReturnRS("DetailContatori", _
            New SqlParameter("@CODCONTATORE ", lngCodContatore))

            Dim intRecordCount As Integer
            If _dr.Read Then lngCodContatorePrec = MyUtility.cTolng(_dr.Item("CODCONTATOREPRECEDENTE"))
            _dr.Close()
            '***********************************************************************************
            If lngCodContatorePrec > 0 Then
                '**********************************************************************************
                _dr = clsDBSql.RunSPReturnRS("TopOneLetture", _
                New SqlParameter("@CODCONTATORE ", lngCodContatore), _
                New SqlParameter("@codutente ", MyUtility.cTolng(drTemp.Item("CODICE_UTENTE"))))
                If _dr.Read Then
                    lngGiorniDiConsumo = getGiorniDiConsumo(ClsData.GiraDataFromDB(MyUtility.GetParametro(_dr.Item("DATALETTURA"))), _
                 MyUtility.GetParametro(drTemp.Item("DATA_LET_ATTUALE")))
                End If
                _dr.Close()
                '**********************************************************************************
                '**********************************************************************************
                intRecordCount = clsDBSql.RunSPReturnRowCount("TopFiveLetture", _
               New SqlParameter("@CODCONTATORE ", lngCodContatore), _
               New SqlParameter("@codutente ", MyUtility.cTolng(drTemp.Item("CODICE_UTENTE"))))

                If intRecordCount < 5 Then
                    _dr = clsDBSql.RunSPReturnRS("TopFiveLetture", _
                    New SqlParameter("@CODCONTATORE ", lngCodContatore), _
                    New SqlParameter("@codutente ", MyUtility.cTolng(drTemp.Item("CODICE_UTENTE"))))

                    While _dr.Read
                        Try
                            dblRapportoCGG = MyUtility.cTolng(_dr.Item("CONSUMO")) / MyUtility.cTolng(_dr.Item("GIORNIDICONSUMO"))
                        Catch ex As Exception When MyUtility.cTolng(_dr.Item("GIORNIDICONSUMO")) = 0
                            'Giorni di Consumo =0 situazione anomala
                        Finally
                            dblResult = dblResult + dblRapportoCGG
                        End Try
                        lngRecordCount = lngRecordCount + 1
                    End While

                End If
                _dr.Close()
                '**********************************************************************************
                Try
                    Dim strTop As String = MyUtility.GetParametro(5 - intRecordCount)
                    _dr = clsDBSql.RunSPReturnRS("LettureContatorePrecedente", _
                   New SqlParameter("@CODCONTATORE ", lngCodContatorePrec), _
                   New SqlParameter("@codutente ", MyUtility.cTolng(drTemp.Item("CODICE_UTENTE"))), _
                   New SqlParameter("@intTop", strTop))
                Catch ex As Exception
                    Dim str As String = ex.Message
                End Try
                'Top 5 -intRecordCount
                While _dr.Read
                    Try
                        dblRapportoCGG = MyUtility.cTolng(_dr.Item("CONSUMO")) / MyUtility.cTolng(_dr.Item("GIORNIDICONSUMO"))
                    Catch ex As Exception When MyUtility.cTolng(_dr.Item("GIORNIDICONSUMO")) = 0
                        'Giorni di Consumo =0 situazione anomala
                    Finally
                        dblResult = dblResult + dblRapportoCGG
                    End Try
                    lngRecordCount = lngRecordCount + 1
                End While

                Try
                    dblMediaConsumo = dblResult / lngRecordCount
                Catch ex As Exception When lngRecordCount = 0
                Finally
                    dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
                End Try
                ' Approssimo per eccesso dblConsumoTeorico
                lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
                '**********************************************************************************
            Else

                lngGiorniDiConsumo = getGiorniDiConsumo(MyUtility.GetParametro(drTemp.Item("DATA_LET_PREC")), _
                MyUtility.GetParametro(drTemp.Item("DATA_LET_ATTUALE")))

                _dr = clsDBSql.RunSPReturnRS("TopFiveLetture", _
                  New SqlParameter("@CODCONTATORE ", lngCodContatore), _
                  New SqlParameter("@codutente ", MyUtility.cTolng(drTemp.Item("CODICE_UTENTE"))))

                While _dr.Read
                    Try
                        dblRapportoCGG = MyUtility.cTolng(_dr.Item("CONSUMO")) / MyUtility.cTolng(_dr.Item("GIORNIDICONSUMO"))
                    Catch ex As Exception When MyUtility.cTolng(_dr.Item("GIORNIDICONSUMO")) = 0
                        'Giorni di Consumo =0 situazione anomala
                    Finally
                        dblResult = dblResult + dblRapportoCGG
                    End Try
                    lngRecordCount = lngRecordCount + 1
                End While

                Try
                    dblMediaConsumo = dblResult / lngRecordCount
                Catch ex As Exception When lngRecordCount = 0
                Finally
                    dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
                End Try
                ' Approssimo per eccesso dblConsumoTeorico
                lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)

            End If
            '*********************************************************************************************
            If Not clsLetture.VerificaTolleranzaConsumo(lngConsumoEffettivo, _
             lngConsumoTeorico, MyUtility.GetParametro(lngCodContatore)) Then
                blnInconguente = True
            End If
            '*********************************************************************************************
            'Transazione
            clsDBSql.RunSP("UpdateContatori", New SqlParameter("@CodContatore ", lngCodContatore), New SqlParameter("@Letto", 1))
            'inserisco la lettura per il Contaore se ha superato tutti i controlli
            setLetture(lngCodContatore, drTemp)
            'Fine Transazione
            'Svuoto la Tabella Access
            Dim strSql, _
            strOleDBString As String

            'Dim oleDBConn As New OleDbConnection()
            'Dim OleDbCommand As New OleDbCommand()

            'oleDBConn.ConnectionString = ConfigurationSettings.AppSettings("connStringDBAccess")
            'oleDBConn.Open()

            'strSql = ""
            'strSql = "DELETE FROM " & strNameTable

            'OleDbCommand.CommandType = CommandType.Text
            'OleDbCommand.CommandText = strSql
            'OleDbCommand.Connection = oleDBConn
            'OleDbCommand.ExecuteNonQuery()

            'oleDBConn.Close()

        End Sub

        'Verifica la Data di lettura
        '*******************************************************
        Protected Function VerificaDataDiLettura(ByVal lngData As Long, _
        ByVal lngDataLettura As Long, ByVal data As TypeData) As Boolean
            VerificaDataDiLettura = False
            Select Case data
                Case TypeData.DATACESSAZIONE
                    If lngDataLettura < lngData Then VerificaDataDiLettura = True
                Case TypeData.DATASOSTITUZIONE
                    If lngDataLettura <= lngData Then VerificaDataDiLettura = True
            End Select
        End Function

        '*******************************************************

        '*******************************************************
        ' setLetture() Method (Protected)
        '
        '  setLetturemethod aggiunge una nuova Lettura nel 
        ' database openUtenze Tabella TP_LETTURE.
        '
        ' Il Metodo è Interno Alla Classe (Protected)
        '   
        '*******************************************************
        Protected Sub setLetture(ByVal lngCodContatore As Long, _
        ByVal drTemp As OleDbDataReader)

            Dim RetVal As Integer = clsDBSql.RunSPReturnInteger("LettureAdd", _
           New SqlParameter("@CodContatore ", lngCodContatore), _
           New SqlParameter("@CodPeriodo", lngIdPeriodo), _
           New SqlParameter("@DataLettura", ClsData.GiraData(MyUtility.GetParametro(drTemp.Item("DATA_LET_ATTUALE")))), _
           New SqlParameter("@Lettura", MyUtility.GetParametro(drTemp.Item("LET_ATTUALE"))), _
           New SqlParameter("@IdStatoLettura", "1"), _
           New SqlParameter("@CodModalitaLettura", "1"), _
           New SqlParameter("@CodUtente", MyUtility.GetParametro(drTemp.Item("CODICE_UTENTE"))))

        End Sub
        '*******************************************************
        ' setTransito() Method (Protected)
        '
        ' setTransito method aggiunge un Record nella tabella di transito 
        ' database openUtenze Tabella TP_CONTATORILETTI_TRANSITO.
        '
        ' Il Metodo è Interno Alla Classe (Protected)
        '   
        '*******************************************************
        Protected Sub setTransito(ByVal lngIDContatore As Long, _
         ByVal drTemp As OleDbDataReader)



            clsDBSql.RunSP("AddTransito", _
           New SqlParameter("@COD_CONTATORE ", lngIDContatore), _
           New SqlParameter("@MATRICOLA", MyUtility.GetParametro(drTemp.Item("MATRICOLA"))), _
           New SqlParameter("@COD_COMUNE_COMPETENZA", MyUtility.GetParametro(drTemp.Item("COD_COMUNE_COMPETENZA"))), _
           New SqlParameter("@CODICE_UTENTE", MyUtility.cTolng(drTemp.Item("CODICE_UTENTE"))), _
           New SqlParameter("@UTENTE", MyUtility.GetParametro(drTemp.Item("UTENTE"))), _
           New SqlParameter("@INDIRIZZO", MyUtility.GetParametro(drTemp.Item("INDIRIZZO"))), _
           New SqlParameter("@COD_COMUNE", MyUtility.GetParametro(drTemp.Item("COD_COMUNE"))), _
           New SqlParameter("@COD_GIRO", MyUtility.cTolng(drTemp.Item("COD_GIRO"))), _
           New SqlParameter("@SEQUENZA", MyUtility.cTolng(drTemp.Item("SEQUENZA"))), _
           New SqlParameter("@UBICAZIONE_CONTATORE", MyUtility.GetParametro(drTemp.Item("UBICAZIONE_CONTATORE"))), _
           New SqlParameter("@TIPO_CONTATORE", MyUtility.cTolng(drTemp.Item("TIPO_CONTATORE"))), _
           New SqlParameter("@DATA_LET_PREC", ClsData.GiraData(MyUtility.GetParametro(drTemp.Item("DATA_LET_PREC")))), _
           New SqlParameter("@LET_PREC", MyUtility.cTolng(drTemp.Item("LET_PREC"))), _
           New SqlParameter("@DATA_LET_ATTUALE", ClsData.GiraData(MyUtility.GetParametro(drTemp.Item("DATA_LET_ATTUALE")))), _
           New SqlParameter("@LET_ATTUALE", MyUtility.cTolng(drTemp.Item("LET_ATTUALE"))), _
           New SqlParameter("@CONSUMO_PREVISTO", MyUtility.cTolng(drTemp.Item("CONSUMO_PREVISTO"))), _
           New SqlParameter("@LASCIATO_AVVISO", MyUtility.CToBit(drTemp.Item("LASCIATO_AVVISO"))), _
           New SqlParameter("@NOTE", MyUtility.GetParametro(drTemp.Item("NOTE"))), _
           New SqlParameter("@COD_ANOMALIA1", MyUtility.cTolng(drTemp.Item("COD_ANOMALIA1"))), _
           New SqlParameter("@COD_ANOMALIA2", MyUtility.cTolng(drTemp.Item("COD_ANOMALIA2"))), _
           New SqlParameter("@COD_ANOMALIA3", MyUtility.cTolng(drTemp.Item("COD_ANOMALIA3"))), _
           New SqlParameter("@CODICE_LETTURISTA", MyUtility.cTolng(drTemp.Item("CODICE_LETTURISTA"))), _
           New SqlParameter("@CODICE_PDA", MyUtility.cTolng(drTemp.Item("CODICE_PDA"))), _
           New SqlParameter("@COD_POSIZIONE_CONTATORE", MyUtility.cTolng(drTemp.Item("COD_POSIZIONE_CONTATORE"))), _
           New SqlParameter("@PROGRESSIVO", MyUtility.cTolng(drTemp.Item("PROGRESSIVO"))), _
           New SqlParameter("@SEGNALIBRO", MyUtility.CToBit(drTemp.Item("SEGNALIBRO"))), _
           New SqlParameter("@DATA_VERIFICA", ClsData.GiraData(MyUtility.GetParametro(drTemp.Item("DATA_VERIFICA")))), _
           New SqlParameter("@FL_NUOVO_CONTATORE", MyUtility.CToBit(drTemp.Item("FL_NUOVO_CONTATORE"))))


        End Sub
        '**********************************************************************************************************
        Protected Sub GetDataCessazione(ByVal lngDataCessazione As Long, _
        ByVal drTemp As OleDbDataReader)

            If lngDataCessazione > 0 Then
                Select Case VerificaDataDiLettura(lngDataCessazione, _
                 MyUtility.cTolng(ClsData.GiraDataFromDB(MyUtility.GetParametro(drTemp.Item("DATA_LET_ATTUALE")))), _
                 TypeData.DATACESSAZIONE)
                    Case True
                        'Calcolo Consumo Teorico ,Consumo Effettivo,Tolleranza consumo(Incongruente)
                        setLetture(lngCodContatore, drTemp)
                        Exit Sub
                    Case False
                        setTransito(lngCodContatore, _
                           drTemp)
                End Select
            End If

        End Sub
        '*******************************************************
        ' GetDataSostituzione() Method (Protected)
        '
        ' GetDataSostituzione method Merge fra Date	
        ' Il Metodo è Interno Alla Classe (Protected)
        '   
        '*******************************************************
        Protected Sub GetDataSostituzione(ByVal lngDataSostituzione As Long, _
        ByVal drTemp As OleDbDataReader)

            If lngDataSotituzione > 0 Then
                Select Case VerificaDataDiLettura(lngDataSotituzione, _
                 MyUtility.cTolng(ClsData.GiraData(MyUtility.GetParametro(drTemp.Item("DATA_LET_ATTUALE")))), _
                 TypeData.DATASOSTITUZIONE)
                    Case True
                        'Calcolo Consumo Teorico ,Consumo Effettivo,Tolleranza consumo(Incongruente)
                        setLetture(lngCodContatore, drTemp)
                        Exit Sub
                    Case False

                        lngCodContatore = lngCodContatoreSucc
                        GetContatoreOpenUtenze(MyUtility.GetParametro(lngCodContatoreSucc), _
                          lngDataCessazione, _
                          lngDataSotituzione, _
                          lngCodPDA, _
                          lngCodContatoreSucc)


                        GetDataSostituzione(lngDataSotituzione, drTemp)

                End Select
            End If

        End Sub
        Protected Function getGiorniDiConsumo(ByVal strDataPrecedente As String, _
         ByVal strDataAttuale As String) As Long

            Dim lngGiorniDiConsumo As Long

            lngGiorniDiConsumo = 0
            lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(strDataPrecedente), CDate(strDataAttuale))
            Return lngGiorniDiConsumo

        End Function
        Protected Function ApprossimaNumero(ByVal dblNumber As Double) As Long
            If IsNumeric(dblNumber) Then
                If dblNumber > 0 Then
                    ApprossimaNumero = System.Math.Ceiling(dblNumber)
                End If
            End If
        End Function
    End Class




End Namespace