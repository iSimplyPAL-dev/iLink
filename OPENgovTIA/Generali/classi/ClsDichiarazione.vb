Imports log4net
Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports AnagInterface
Imports Utility
''' <summary>
''' Classe per la gestione delle dichiarazioni
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsDichiarazione
    Private oReplace As New generalClass.generalFunction
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsDichiarazione))
    'Private cmdMyCommand As New SqlClient.SqlCommand
    'Private DrReturn As SqlClient.SqlDataReader
    ''' <summary>
    ''' Costanti per definire il tipo di stampa dichiarazioni che si intende eseguire
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
#Region "Tipo Stampa Dichiarazioni"
    Public Const TipoStampaAnalitica As String = "A"
    Public Const TipoStampaSintetica As String = "S"
    Public Const TipoStampaEsportazione As String = "E"
#End Region

    'Public Function DeleteDichiarazione(ByVal WFSessione As OPENUtility.CreateSessione, ByVal oTestata As ObjTestata) As Integer
    '    Dim sMyErr As String
    '    Dim cancellaT As New GestTestata
    '    Dim FncTessera As New GestTessera
    '    Dim cancellaDetT As New GestDettaglioTestata
    '    Dim cancellaVano As New GestOggetti
    '    Dim delTestata As Integer = 0
    '    Dim i, k, w As Integer

    '    Try
    '        If cancellaT.DeleteTestataDichiarazione(oTestata.IdTestata, Now, WFSessione, sMyErr) = 0 Then
    '            Return 0
    '        End If

    '        For i = 0 To oTestata.oTessere.Length - 1
    '            If FncTessera.SetTessera(oTestata.oTessere(i), 2, WFSessione) = 0 Then
    '                Return 0
    '            Else
    '                For w = 0 To oTestata.oTessere(i).oImmobili.Length - 1
    '                    If cancellaDetT.DeleteDettaglioTestataDich(WFSessione, oTestata.oTessere(i).oImmobili(w).Id, Now, 0) = 0 Then
    '                        Return 0
    '                    Else
    '                        For k = 0 To oTestata.oTessere(i).oImmobili(w).oOggetti.Length - 1
    '                            If cancellaVano.DeleteOggetti(WFSessione, oTestata.oTessere(i).oImmobili(w).oOggetti(k).IdOggetto, Now, oTestata.oTessere(i).oImmobili(w).Id, 0) = 0 Then
    '                                Return 0
    '                            End If
    '                        Next
    '                    End If
    '                Next
    '            End If
    '        Next
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.DeleteDichiarazione.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function GetDichiarazione(ByVal WFSessione As OPENUtility.CreateSessione, ByVal nIDDichiarazione As Integer, ByRef nIdContribuente As Integer, ByVal bIsRicerca As Boolean, Optional ByVal sAnno As String = "") As ObjTestata()
    '    Dim oMyTestata() As ObjTestata
    '    Dim i, x, y, nList As Integer

    '    Try
    '        'prelevo i dati della testata
    '        Dim FunctionTestata As New GestTestata
    '        oMyTestata = FunctionTestata.GetTestata(WFSessione, nIDDichiarazione, nIdContribuente, sAnno)

    '        If Not oMyTestata Is Nothing Then
    '            For i = 0 To oMyTestata.Length - 1
    '                nIdContribuente = oMyTestata(i).IdContribuente
    '                'prelevo i dati di dettaglio testata
    '                Dim FncTessere As New GestTessera
    '                oMyTestata(i).oTessere = FncTessere.GetTessera(WFSessione, oMyTestata(i).sEnte, -1, oMyTestata(i).Id, "", -1, "", -1, True, False)

    '                'se arrivo dalla ricerca devo popolare un oggetto che è l'unione di tessera e immobile
    '                If bIsRicerca = True Then
    '                    Dim oListTesUI() As ObjTesseraUI
    '                    oListTesUI = FncTessere.GetTesVSUI(oMyTestata(i))
    '                    oMyTestata(i).oTesUI = oListTesUI
    '                End If

    '                'prelevo i dati della famiglia
    '                Dim FunctionFamiglia As New GestFamiglia
    '                oMyTestata(i).oFamiglia = FunctionFamiglia.GetFamiglia(WFSessione, oMyTestata(i).Id)
    '            Next
    '        End If
    '        Return oMyTestata
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichiarazione.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function SetDichiarazione(ByVal oNewTestataDichiarazione As ObjTestata, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Try
    '        Dim IdNewTestata As Integer
    '        'inserisco i dati della testata
    '        Dim FncTestata As New GestTestata
    '        IdNewTestata = FncTestata.SetTestata(oNewTestataDichiarazione, WFSessione)
    '        If IdNewTestata <= 0 Then
    '            Return 0
    '        End If
    '        oNewTestataDichiarazione.Id = IdNewTestata

    '        'inserisco i dati di tessera
    '        Dim FncTessera As New GestTessera
    '        IdNewTestata = FncTessera.SetTesseraCompleta(oNewTestataDichiarazione.oTessere, IdNewTestata, WFSessione)
    '        If IdNewTestata <= 0 Then
    '            'cancello la testata inserita incompleta
    '            FncTestata.DeleteTestata(IdNewTestata, Now, WFSessione, 1)
    '            Return 0
    '        End If
    '        'inserisco i dati dei componenti famiglia
    '        If Not oNewTestataDichiarazione.oFamiglia Is Nothing Then
    '            Dim FncFamiglia As New GestFamiglia
    '            If FncFamiglia.SetFamiglia(WFSessione, oNewTestataDichiarazione.oFamiglia, oNewTestataDichiarazione.IdTestata) = 0 Then
    '                Return 0
    '            End If
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.SetDichiarazione.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function GetNDichAutomatico(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sIdEnte As String) As String
    '    Dim nDichAutomatico As String = ""

    '    Try
    '        'prelevo i dati
    '        cmdMyCommand.CommandText = "SELECT NDICH= CASE WHEN ISNUMERIC(NUMERO_DICHIARAZIONE)=1 THEN NUMERO_DICHIARAZIONE+1 ELSE NULL END"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " ORDER BY IDTESTATA DESC"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        'eseguo la query
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        If DrReturn.Read Then
    '            If Not IsDBNull(DrReturn("ndich")) Then
    '                nDichAutomatico = StringOperation.FormatString(DrReturn("ndich"))
    '            End If
    '        End If
    '        Return nDichAutomatico
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetNDichAutomatico.errore: ", Err)
    '        Return ""
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function IsUniqueDich(ByVal sIdEnte As String, ByVal tDataDich As Date, ByVal sNDich As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Try
    '        'prelevo i dati
    '        cmdMyCommand.CommandText = "SELECT IDTESTATA"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (DATA_DICHIARAZIONE=@DATADICH) "
    '        cmdMyCommand.CommandText += " AND (NUMERO_DICHIARAZIONE=@NDICH)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADICH", SqlDbType.DateTime)).Value = tDataDich
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NDICH", SqlDbType.NVarChar)).Value = sNDich
    '        'eseguo la query
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        If DrReturn.Read Then
    '            If Not IsDBNull(DrReturn("idtestata")) Then
    '                Return 0
    '            End If
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.IsUniqueDich.errore: ", ex)
    '        Return 0
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function GetDichPerCalcolo(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sIdEnte As String, ByVal sAnno As String, ByVal nIdTestata As Integer, ByVal nIdContribuente As Integer) As ObjAvviso()
    '    Dim oMyAvviso As ObjAvviso
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim oMyUI As ObjUnitaImmobiliare
    '    Dim oListUI() As ObjUnitaImmobiliare
    '    Dim nListAvvisi, nListUI, nIdContribPrec As Integer
    '    Dim FncTessere As New GestTessera
    '    Dim FncRidEse As New GestRidEse
    '    Dim oRicRidEse As New ObjRidEse

    '    Try
    '        nListAvvisi = -1 : nListUI = -1
    '        'prelevo i dati per il calcolo
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_GETDICHCALCOLO"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (YEAR(DATA_INIZIO)<=@ANNO)"
    '        cmdMyCommand.CommandText += " AND ((YEAR(DATA_FINE) IS NULL) OR (YEAR(DATA_FINE)>=@ANNO))"
    '        cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '        If nIdTestata <> -1 Then
    '            cmdMyCommand.CommandText += " AND IDTESTATA=@IDTESTATA"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        End If
    '        If nIdContribuente <> -1 Then
    '            cmdMyCommand.CommandText += " AND IDCONTRIBUENTE=@IDCONTRIBUENTE"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY IDCONTRIBUENTE, IDDETTAGLIOTESTATA, IDCATEGORIA, DATA_INIZIO, DATA_FINE"
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Log.Debug("GetDichPerCalcolo::SQL::" & cmdMyCommand.CommandText)
    '        Do While DrReturn.Read
    '            If StringOperation.Formatint(DrReturn("IDCONTRIBUENTE")) <> nIdContribPrec And Not IsNothing(oMyUI) Then
    '                oMyAvviso.oTessere = FncTessere.GetTessera(WFSessione, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, oMyAvviso.sAnnoRiferimento, -1, False, False)
    '                oMyAvviso.oUI = oListUI
    '                nListAvvisi += 1
    '                ReDim Preserve oListAvvisi(nListAvvisi)
    '                oListAvvisi(nListAvvisi) = oMyAvviso
    '                nListUI = -1
    '            End If
    '            Log.Debug("GetDichPerCalcolo::SQL::nListAvvisi::" & nListAvvisi)

    '            oMyAvviso = New ObjAvviso
    '            oMyUI = New ObjUnitaImmobiliare

    '            oMyAvviso.IdEnte = sIdEnte
    '            oMyAvviso.IdContribuente = DrReturn("IDCONTRIBUENTE")
    '            oMyAvviso.sAnnoRiferimento = sAnno
    '            'oMyAvviso.IdTestata = DrReturn("IDTESTATA")

    '            If Not IsDBNull(DrReturn("cognome")) Then
    '                oMyAvviso.sCognome = StringOperation.FormatString(DrReturn("cognome"))
    '            End If
    '            If Not IsDBNull(DrReturn("nome")) Then
    '                oMyAvviso.sNome = StringOperation.FormatString(DrReturn("nome"))
    '            End If
    '            If Not IsDBNull(DrReturn("cod_fiscale")) Then
    '                oMyAvviso.sCodFiscale = StringOperation.FormatString(DrReturn("cod_fiscale"))
    '            End If
    '            If Not IsDBNull(DrReturn("partita_iva")) Then
    '                oMyAvviso.sPIVA = StringOperation.FormatString(DrReturn("partita_iva"))
    '            End If
    '            If Not IsDBNull(DrReturn("via_res")) Then
    '                oMyAvviso.sIndirizzoRes = StringOperation.FormatString(DrReturn("via_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("civico_res")) Then
    '                oMyAvviso.sCivicoRes = StringOperation.FormatString(DrReturn("civico_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("cap_res")) Then
    '                oMyAvviso.sCAPRes = StringOperation.FormatString(DrReturn("cap_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("comune_res")) Then
    '                oMyAvviso.sComuneRes = StringOperation.FormatString(DrReturn("comune_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("pv_res")) Then
    '                oMyAvviso.sProvRes = StringOperation.FormatString(DrReturn("pv_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("frazione_res")) Then
    '                oMyAvviso.sFrazRes = StringOperation.FormatString(DrReturn("frazione_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("nominativo_co")) Then
    '                oMyAvviso.sNominativoCO = StringOperation.FormatString(DrReturn("nominativo_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("via_co")) Then
    '                oMyAvviso.sIndirizzoCO = StringOperation.FormatString(DrReturn("via_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("cap_co")) Then
    '                oMyAvviso.sCAPCO = StringOperation.FormatString(DrReturn("cap_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("comune_co")) Then
    '                oMyAvviso.sComuneCO = StringOperation.FormatString(DrReturn("comune_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("pv_co")) Then
    '                oMyAvviso.sProvCO = StringOperation.FormatString(DrReturn("pv_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("frazione_co")) Then
    '                oMyAvviso.sFrazCO = StringOperation.FormatString(DrReturn("frazione_co"))
    '            End If

    '            oMyUI.Id = DrReturn("id")
    '            oMyUI.IdDettaglioTestata = DrReturn("IDDETTAGLIOTESTATA")
    '            oMyUI.tDataInizio = DrReturn("DATA_INIZIO")
    '            If Not IsDBNull(DrReturn("DATA_FINE")) Then
    '                oMyUI.tDataFine = DrReturn("DATA_FINE")
    '            End If
    '            If Not IsDBNull(DrReturn("via")) Then
    '                oMyUI.sVia = StringOperation.FormatString(DrReturn("VIA"))
    '            End If
    '            If Not IsDBNull(DrReturn("civico")) Then
    '                oMyUI.sCivico = StringOperation.FormatString(DrReturn("CIVICO"))
    '            End If
    '            If Not IsDBNull(DrReturn("esponente")) Then
    '                oMyUI.sEsponente = StringOperation.FormatString(DrReturn("ESPONENTE"))
    '            End If
    '            If Not IsDBNull(DrReturn("interno")) Then
    '                oMyUI.sInterno = StringOperation.FormatString(DrReturn("INTERNO"))
    '            End If
    '            If Not IsDBNull(DrReturn("scala")) Then
    '                oMyUI.sScala = StringOperation.FormatString(DrReturn("SCALA"))
    '            End If
    '            If Not IsDBNull(DrReturn("foglio")) Then
    '                oMyUI.sFoglio = StringOperation.FormatString(DrReturn("foglio"))
    '            End If
    '            If Not IsDBNull(DrReturn("numero")) Then
    '                oMyUI.sNumero = StringOperation.FormatString(DrReturn("numero"))
    '            End If
    '            If Not IsDBNull(DrReturn("subalterno")) Then
    '                oMyUI.sSubalterno = StringOperation.FormatString(DrReturn("subalterno"))
    '            End If
    '            oMyUI.nMQ = stringoperation.formatdouble(DrReturn("MQ"))
    '            If Not IsDBNull(DrReturn("GGTARSU")) Then
    '                oMyUI.nGGTarsu = DrReturn("GGTARSU")
    '            End If
    '            oMyUI.IdCategoria = StringOperation.FormatString(DrReturn("IDCATEGORIA"))
    '            oMyUI.IdTariffa = StringOperation.Formatint(DrReturn("IDTARIFFA"))
    '            oMyUI.impTariffa = stringoperation.formatdouble(DrReturn("IMPORTO_TARIFFA"))

    '            '***Agenzia Entrate***
    '            If Not IsDBNull(DrReturn("sezione")) Then
    '                oMyUI.sSezione = StringOperation.FormatString(DrReturn("sezione"))
    '            End If
    '            If Not IsDBNull(DrReturn("estensione_particella")) Then
    '                oMyUI.sEstensioneParticella = StringOperation.FormatString(DrReturn("estensione_particella"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_tipo_particella")) Then
    '                oMyUI.sIdTipoParticella = StringOperation.FormatString(DrReturn("id_tipo_particella"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_titolo_occupazione")) Then
    '                oMyUI.nIdTitoloOccupaz = StringOperation.Formatint(DrReturn("id_titolo_occupazione"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_natura_occupante")) Then
    '                oMyUI.nIdNaturaOccupaz = StringOperation.Formatint(DrReturn("id_natura_occupante"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_destinazione_uso")) Then
    '                oMyUI.nIdDestUso = StringOperation.Formatint(DrReturn("id_destinazione_uso"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_tipo_unita")) Then
    '                oMyUI.sIdTipoUnita = StringOperation.FormatString(DrReturn("id_tipo_unita"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_assenza_dati_catastali")) Then
    '                oMyUI.nIdAssenzaDatiCatastali = StringOperation.Formatint(DrReturn("id_assenza_dati_catastali"))
    '            End If
    '            '*********************
    '            oRicRidEse.IdEnte = sIdEnte
    '            oMyUI.oRiduzioni = FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_UI, oMyUI.Id)
    '            oMyUI.oDetassazioni = FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_UI, oMyUI.Id)

    '            nListUI += 1
    '            ReDim Preserve oListUI(nListUI)
    '            oListUI(nListUI) = oMyUI
    '            nIdContribPrec = StringOperation.Formatint(DrReturn("IDCONTRIBUENTE"))
    '        Loop
    '        Log.Debug("GetDichPerCalcolo::devo prelevare tessera per oMyAvviso.IdContribuente::" & oMyAvviso.IdContribuente)
    '        oMyAvviso.oTessere = FncTessere.GetTessera(WFSessione, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, oMyAvviso.sAnnoRiferimento, -1, False, False)
    '        Log.Debug("GetDichPerCalcolo::prelevato tessera")
    '        oMyAvviso.oUI = oListUI
    '        nListAvvisi += 1
    '        ReDim Preserve oListAvvisi(nListAvvisi)
    '        oListAvvisi(nListAvvisi) = oMyAvviso
    '        nListUI = -1

    '        oMyAvviso = New ObjAvviso
    '        oMyUI = New ObjUnitaImmobiliare

    '        Log.Debug("GetDichPerCalcolo::esco")
    '        Return oListAvvisi
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function GetSoggettiFromDich(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sIdEnte As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCodFiscale As String, ByVal sPIva As String, ByVal sNTessera As String, ByVal FlagAperta As Integer, ByRef sErrDichiarazione As String) As ObjTestataSearch()
    '    Dim oDich As ObjTestataSearch
    '    Dim nListDich As Integer = -1
    '    Dim oListToReturned() As ObjTestataSearch

    '    Try
    '        'prelevo i dati della testata
    '        cmdMyCommand.CommandText = "SELECT DISTINCT IDENTE"
    '        cmdMyCommand.CommandText += ", COGNOME_DENOMINAZIONE, NOME"
    '        cmdMyCommand.CommandText += ", COD_FISCALE, PARTITA_IVA, CF_PIVA"
    '        cmdMyCommand.CommandText += ", DATA_CESSAZIONE,CHIUSA"
    '        cmdMyCommand.CommandText += ", COD_CONTRIBUENTE"
    '        cmdMyCommand.CommandText += ", ID, IDTESTATA"
    '        cmdMyCommand.CommandText += ", DATA_DICHIARAZIONE, NUMERO_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " FROM V_GETDICHIARAZIONI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        If FlagAperta = 0 Then
    '            cmdMyCommand.CommandText += " AND (DATA_CESSAZIONE IS NULL)"
    '        ElseIf FlagAperta = 1 Then
    '            cmdMyCommand.CommandText += " AND (NOT DATA_CESSAZIONE IS NULL)"
    '        End If
    '        If sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_DENOMINAZIONE LIKE @COGNOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sCognome) & "%"
    '        End If
    '        If sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sNome) & "%"
    '        End If
    '        If sCodFiscale <> "" Then
    '            cmdMyCommand.CommandText += " AND (COD_FISCALE LIKE @CF)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CF", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sCodFiscale) & ""
    '        End If
    '        If sPIva <> "" Then
    '            cmdMyCommand.CommandText += " AND (PARTITA_IVA LIKE @PIVA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIVA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sPIva) & "%"
    '        End If
    '        If sNTessera <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO_TESSERA=@NTESSERA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NTESSERA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sNTessera)
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME_DENOMINAZIONE, NOME, COD_FISCALE, PARTITA_IVA"
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            nListDich += 1
    '            oDich = New ObjTestataSearch
    '            oDich.sCognome = StringOperation.FormatString(DrReturn("COGNOME_DENOMINAZIONE"))
    '            oDich.IdContribuente = StringOperation.Formatint(DrReturn("COD_CONTRIBUENTE"))
    '            oDich.sNome = StringOperation.FormatString(DrReturn("NOME"))
    '            oDich.sCfPiva = StringOperation.FormatString(DrReturn("CF_PIVA"))
    '            oDich.Id = StringOperation.Formatint(DrReturn("ID"))
    '            oDich.IdTestata = StringOperation.Formatint(DrReturn("IDTESTATA"))
    '            oDich.tDataDichiarazione = StringOperation.Formatdatetime(DrReturn("DATA_DICHIARAZIONE"))
    '            oDich.sNDichiarazione = StringOperation.FormatString(DrReturn("NUMERO_DICHIARAZIONE"))
    '            oDich.Chiusa = StringOperation.Formatint(DrReturn("CHIUSA"))
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturned(nListDich)
    '            'memorizzo i dati nell'array
    '            oListToReturned(nListDich) = oDich
    '        Loop

    '        HttpContext.Current.Session("myDvResult") = oListToReturned
    '        Return oListToReturned
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetSoggettiFromDich.errore: ",Err)
    '        sErrDichiarazione = "GetSoggettiFromDich::" & "Si è verificato il seguente errore: " & vbCrLf & Err.Message()
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function GetSoggettiFromImmobili(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sIdEnte As String, ByVal sVia As String, ByVal sCivico As String, ByVal sInterno As String, ByVal FlagAperta As Integer, ByRef sErrDichiarazione As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal sSubalterno As String) As ObjTestataSearch()
    '    Dim oDich As ObjTestataSearch
    '    Dim nListDich As Integer = -1
    '    Dim oListToReturned() As ObjTestataSearch

    '    Try
    '        'prelevo i dati della testata
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_GETDICHIARAZIONI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        '*** 20130109 - in questo caso devo filtrare sulle sole tessere che hanno un immobile associato
    '        cmdMyCommand.CommandText += " AND (NOT VIA IS NULL)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        If FlagAperta = 0 Then
    '            cmdMyCommand.CommandText += " AND (DATA_CESSAZIONE IS NULL)"
    '        ElseIf FlagAperta = 1 Then
    '            cmdMyCommand.CommandText += " AND (NOT DATA_CESSAZIONE IS NULL)"
    '        End If
    '        If sVia <> "" Then
    '            cmdMyCommand.CommandText += " AND (VIA LIKE @VIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sVia) & "%"
    '        End If
    '        If sCivico <> "" Then
    '            cmdMyCommand.CommandText += " AND (CIVICO LIKE @CIVICO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sCivico) & "%"
    '        End If
    '        If sInterno <> "" Then
    '            cmdMyCommand.CommandText += " AND (INTERNO LIKE @INTERNO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sInterno) & "%"
    '        End If
    '        If sFoglio <> "" Then
    '            cmdMyCommand.CommandText += " AND (FOGLIO=@FOGLIO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = sFoglio
    '        End If
    '        If sNumero <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO=@NUMERO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = sNumero
    '        End If
    '        If sSubalterno <> "" Then
    '            cmdMyCommand.CommandText += " AND (SUBALTERNO=@SUBALTERNO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.NVarChar)).Value = sSubalterno
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME_DENOMINAZIONE, NOME, COD_FISCALE, PARTITA_IVA"
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            nListDich += 1
    '            oDich = New ObjTestataSearch
    '            oDich.IdContribuente = StringOperation.Formatint(DrReturn("COD_CONTRIBUENTE"))
    '            oDich.Id = StringOperation.Formatint(DrReturn("ID"))
    '            oDich.IdTestata = StringOperation.Formatint(DrReturn("IDTESTATA"))
    '            If Not IsDBNull(DrReturn("COGNOME_DENOMINAZIONE")) Then
    '                oDich.sCognome = StringOperation.FormatString(DrReturn("COGNOME_DENOMINAZIONE"))
    '            End If
    '            If Not IsDBNull(DrReturn("NOME")) Then
    '                oDich.sNome = StringOperation.FormatString(DrReturn("NOME"))
    '            End If
    '            If Not IsDBNull(DrReturn("CF_PIVA")) Then
    '                oDich.sCfPiva = StringOperation.FormatString(DrReturn("CF_PIVA"))
    '            End If
    '            If Not IsDBNull(DrReturn("DATA_DICHIARAZIONE")) Then
    '                oDich.tDataDichiarazione = StringOperation.Formatdatetime(DrReturn("DATA_DICHIARAZIONE"))
    '            End If
    '            If Not IsDBNull(DrReturn("NUMERO_DICHIARAZIONE")) Then
    '                oDich.sNDichiarazione = StringOperation.FormatString(DrReturn("NUMERO_DICHIARAZIONE"))
    '            End If
    '            If Not IsDBNull(DrReturn("via")) Then
    '                oDich.sVia = StringOperation.FormatString(DrReturn("via"))
    '            End If
    '            If Not IsDBNull(DrReturn("civico")) Then
    '                oDich.sCivico = StringOperation.FormatString(DrReturn("civico"))
    '            End If
    '            If Not IsDBNull(DrReturn("interno")) Then
    '                oDich.sInterno = StringOperation.FormatString(DrReturn("interno"))
    '            End If
    '            If Not IsDBNull(DrReturn("CHIUSA")) Then
    '                oDich.Chiusa = StringOperation.Formatint(DrReturn("CHIUSA"))
    '            End If
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturned(nListDich)
    '            'memorizzo i dati nell'array
    '            oListToReturned(nListDich) = oDich
    '        Loop

    '        HttpContext.Current.Session("myDvResult") = oListToReturned
    '        Return oListToReturned
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetSoggettiFromImmobili.errore: ",Err)
    '        sErrDichiarazione = "GetSoggettiFromImmobili::" & "Si è verificato il seguente errore: " & vbCrLf & Err.Message()
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="nIDDichiarazione"></param>
    ''' <param name="nIDContribuente"></param>
    ''' <param name="nIDDettaglioTestata"></param>
    ''' <param name="bIsRicerca"></param>
    ''' <param name="sAnno"></param>
    ''' <returns></returns>2
    Public Function GetDichiarazione(ByVal myConnectionString As String, ByVal nIDDichiarazione As Integer, ByRef nIDContribuente As Integer, nIDDettaglioTestata As Integer, ByVal bIsRicerca As Boolean, ByVal sAnno As String) As ObjTestata()
        Dim ListTestata() As ObjTestata

        Try
            'prelevo i dati della testata
            Dim FunctionTestata As New GestTestata
            ListTestata = FunctionTestata.GetTestata(myConnectionString, nIDDichiarazione, nIDContribuente, nIDDettaglioTestata, sAnno)
            If Not ListTestata Is Nothing Then
                For Each myItem As ObjTestata In ListTestata
                    nIDContribuente = myItem.IdContribuente
                    'prelevo i dati di dettaglio testata
                    Dim FncTessere As New GestTessera
                    myItem.oTessere = FncTessere.GetTessera(myConnectionString, myItem.sEnte, -1, myItem.Id, "", -1, "", -1, True, False)
                    '*** X UNIONE CON BANCADATI CMGC ***
                    'prelevo i dati di dettaglio testata
                    Dim FncDettaglio As New GestDettaglioTestata
                    myItem.oImmobili = FncDettaglio.GetDettaglioTestata(myConnectionString, -1, -1, myItem.Id, myItem.sEnte, bIsRicerca)
                    '*** ***
                    'se arrivo dalla ricerca devo popolare un oggetto che è l'unione di tessera e immobile
                    If bIsRicerca = True Then
                        Dim oListTesUI() As ObjTesseraUI
                        oListTesUI = FncTessere.GetTesVSUI(myItem)
                        myItem.oTesUI = oListTesUI
                    End If

                    'prelevo i dati della famiglia
                    Dim FncFamiglia As New GestFamiglia
                    myItem.oFamiglia = FncFamiglia.GetFamiglia(myConnectionString, myItem.Id)
                    myItem.dvFamigliaResidenti = FncFamiglia.GetFamigliaResidenti(ConstSession.StringConnectionAnagrafica, myItem.IdContribuente)
                Next
            End If
            Return ListTestata
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichiarazione.errore: ", Err)
            Return Nothing
        End Try
    End Function

    '*** 201511 - Funzioni Sovracomunali ***'*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="myParamSearch"></param>
    ''' <param name="sErrDichiarazione"></param>
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
    Public Function GetSoggettiFromDich(ByVal myStringConnection As String, ByVal myParamSearch As ObjSearchTestata, ByRef sErrDichiarazione As String) As ObjTestataSearch()
        Dim oDich As ObjTestataSearch
        Dim nListDich As Integer = -1
        Dim oListToReturned() As ObjTestataSearch = Nothing
        Dim dvMyDati As New DataView

        Try
            'prelevo i dati
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, myStringConnection)
            Dim sSQL As String = ""
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDichiarazioni", "TYPERICERCA", "AMBIENTE", "IDENTE", "COGNOME", "NOME", "CF", "PIVA", "NTESSERA", "CHIUSE", "VIA", "CIVICO", "INTERNO", "FOGLIO", "NUMERO", "SUBALTERNO", "DAL", "AL", "LISTCONTRIB", "IDPROVENIENZA", "TYPESOGRES", "CATCATASTALE", "IDCATEGORIA", "NC", "ISPF", "ISPV", "ISESENTE", "HASMOREUI", "IDRIDUZIONE", "IDDETASSAZIONE", "IDSTATOOCCUPAZIONE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TYPERICERCA", "S") _
                        , ctx.GetParam("AMBIENTE", ConstSession.Ambiente) _
                        , ctx.GetParam("IdEnte", myParamSearch.IdEnte) _
                        , ctx.GetParam("COGNOME", oReplace.ReplaceCharsForSearch(myParamSearch.sCognome)) _
                        , ctx.GetParam("NOME", oReplace.ReplaceCharsForSearch(myParamSearch.sNome)) _
                        , ctx.GetParam("CF", oReplace.ReplaceCharsForSearch(myParamSearch.sCF)) _
                        , ctx.GetParam("PIVA", oReplace.ReplaceCharsForSearch(myParamSearch.sPIVA)) _
                        , ctx.GetParam("NTESSERA", oReplace.ReplaceCharsForSearch(myParamSearch.sNTessera)) _
                        , ctx.GetParam("CHIUSE", myParamSearch.Chiusa) _
                        , ctx.GetParam("VIA", "") _
                        , ctx.GetParam("CIVICO", "") _
                        , ctx.GetParam("INTERNO", "") _
                        , ctx.GetParam("FOGLIO", "") _
                        , ctx.GetParam("NUMERO", "") _
                        , ctx.GetParam("SUBALTERNO", "") _
                        , ctx.GetParam("Dal", oReplace.FormattaData(myParamSearch.Dal, "A")) _
                        , ctx.GetParam("Al", oReplace.FormattaData(myParamSearch.Al, "A")) _
                        , ctx.GetParam("LISTCONTRIB", "") _
                        , ctx.GetParam("IDPROVENIENZA", myParamSearch.sProvenienza) _
                        , ctx.GetParam("TYPESOGRES", myParamSearch.TypeSogRes) _
                        , ctx.GetParam("CATCATASTALE", "") _
                        , ctx.GetParam("IDCATEGORIA", -1) _
                        , ctx.GetParam("NC", -1) _
                        , ctx.GetParam("ISPF", 0) _
                        , ctx.GetParam("ISPV", 0) _
                        , ctx.GetParam("ISESENTE", 0) _
                        , ctx.GetParam("HASMOREUI", 0) _
                        , ctx.GetParam("IDRIDUZIONE", "") _
                        , ctx.GetParam("IDDETASSAZIONE", "") _
                        , ctx.GetParam("IDSTATOOCCUPAZIONE", "")
                    )
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                nListDich += 1
                oDich = New ObjTestataSearch
                oDich.DescrizioneEnte = StringOperation.FormatString(dtMyRow("DESCRIZIONE_ENTE"))
                oDich.sCognome = StringOperation.FormatString(dtMyRow("COGNOME_DENOMINAZIONE"))
                oDich.IdContribuente = StringOperation.FormatInt(dtMyRow("COD_CONTRIBUENTE"))
                oDich.sNome = StringOperation.FormatString(dtMyRow("NOME"))
                oDich.sCfPiva = StringOperation.FormatString(dtMyRow("CF_PIVA"))
                oDich.Id = StringOperation.FormatInt(dtMyRow("ID"))
                oDich.IdTestata = StringOperation.FormatInt(dtMyRow("IDTESTATA"))
                oDich.tDataDichiarazione = StringOperation.FormatDateTime(dtMyRow("DATA_DICHIARAZIONE"))
                oDich.sNDichiarazione = StringOperation.FormatString(dtMyRow("NUMERO_DICHIARAZIONE"))
                oDich.Chiusa = StringOperation.FormatInt(dtMyRow("CHIUSA"))
                oDich.tDataInizio = StringOperation.FormatDateTime(dtMyRow("DATA_INIZIO"))
                oDich.tDataFine = StringOperation.FormatDateTime(dtMyRow("DATA_FINE"))
                'dimensiono l'array
                ReDim Preserve oListToReturned(nListDich)
                'memorizzo i dati nell'array
                oListToReturned(nListDich) = oDich
            Next

            HttpContext.Current.Session("myDvResult") = oListToReturned
            Return oListToReturned
        Catch Err As Exception
            Log.Debug(myParamSearch.IdEnte + " - OPENgovTIA.ClsDichiarazione.GetSoggettiFromDich.errore: ", Err)
            sErrDichiarazione = "GetSoggettiFromDich::Si è verificato il seguente errore: " & Err.Message()
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetSoggettiFromDich(ByVal myStringConnection As String, ByVal myParamSearch As ObjSearchTestata, ByRef sErrDichiarazione As String) As ObjTestataSearch()
    '    'Public Function GetSoggettiFromDich(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal myParamSearch As ObjSearchTestata, ByRef sErrDichiarazione As String) As ObjTestataSearch()
    '    'Public Function GetSoggettiFromDich(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCodFiscale As String, ByVal sPIva As String, ByVal sNTessera As String, ByVal FlagChiuse As Integer, ByRef sErrDichiarazione As String) As ObjTestataSearch()
    '    Dim oDich As ObjTestataSearch
    '    Dim nListDich As Integer = -1
    '    Dim oListToReturned() As ObjTestataSearch = Nothing
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        'prelevo i dati della testata

    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetDichiarazioni"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPERICERCA", SqlDbType.VarChar)).Value = "S"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AMBIENTE", SqlDbType.VarChar)).Value = ConstSession.Ambiente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = myParamSearch.IdEnte 'sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVENIENZA", SqlDbType.VarChar)).Value = myParamSearch.sProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(myParamSearch.Dal, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(myParamSearch.Al, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CHIUSE", SqlDbType.Int)).Value = myParamSearch.Chiusa
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sCognome)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sNome)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CF", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sCF)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIVA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sPIVA)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPESOGRES", SqlDbType.Int)).Value = myParamSearch.TypeSogRes
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NTESSERA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sNTessera)
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            nListDich += 1
    '            oDich = New ObjTestataSearch
    '            '*** 201511 - Funzioni Sovracomunali ***
    '            oDich.DescrizioneEnte = StringOperation.FormatString(dtMyRow("DESCRIZIONE_ENTE"))
    '            '*** ***
    '            oDich.sCognome = StringOperation.FormatString(dtMyRow("COGNOME_DENOMINAZIONE"))
    '            oDich.IdContribuente = StringOperation.Formatint(dtMyRow("COD_CONTRIBUENTE"))
    '            oDich.sNome = StringOperation.FormatString(dtMyRow("NOME"))
    '            oDich.sCfPiva = StringOperation.FormatString(dtMyRow("CF_PIVA"))
    '            oDich.Id = StringOperation.Formatint(dtMyRow("ID"))
    '            oDich.IdTestata = StringOperation.Formatint(dtMyRow("IDTESTATA"))
    '            oDich.tDataDichiarazione = StringOperation.Formatdatetime(dtMyRow("DATA_DICHIARAZIONE"))
    '            oDich.sNDichiarazione = StringOperation.FormatString(dtMyRow("NUMERO_DICHIARAZIONE"))
    '            oDich.Chiusa = StringOperation.Formatint(dtMyRow("CHIUSA"))
    '            If Not IsDBNull(dtMyRow("DATA_INIZIO")) Then
    '                oDich.tDataInizio = StringOperation.Formatdatetime(dtMyRow("DATA_INIZIO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_FINE")) Then
    '                oDich.tDataFine = StringOperation.Formatdatetime(dtMyRow("DATA_FINE"))
    '            End If
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturned(nListDich)
    '            'memorizzo i dati nell'array
    '            oListToReturned(nListDich) = oDich
    '        Next

    '        HttpContext.Current.Session("myDvResult") = oListToReturned
    '        Return oListToReturned
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetSoggettiFromDich.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        sErrDichiarazione = "GetSoggettiFromDich::Si è verificato il seguente errore: " & Err.Message()
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '*** 201511 - Funzioni Sovracomunali ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="myParamSearch"></param>
    ''' <param name="sErrDichiarazione"></param>
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
    Public Function GetSoggettiFromImmobili(ByVal myStringConnection As String, ByVal myParamSearch As ObjSearchTestata, ByRef sErrDichiarazione As String) As ObjTestataSearch()
        Dim oDich As ObjTestataSearch
        Dim nListDich As Integer = -1
        Dim oListToReturned() As ObjTestataSearch = Nothing
        Dim dvMyDati As New DataView

        Try
            'prelevo i dati
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, myStringConnection)
            Dim sSQL As String = ""
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDichiarazioni", "TYPERICERCA", "AMBIENTE", "IDENTE", "COGNOME", "NOME", "CF", "PIVA", "NTESSERA", "CHIUSE", "VIA", "CIVICO", "INTERNO", "FOGLIO", "NUMERO", "SUBALTERNO", "DAL", "AL", "LISTCONTRIB", "IDPROVENIENZA", "TYPESOGRES", "CATCATASTALE", "IDCATEGORIA", "NC", "ISPF", "ISPV", "ISESENTE", "HASMOREUI", "IDRIDUZIONE", "IDDETASSAZIONE", "IDSTATOOCCUPAZIONE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TYPERICERCA", "I") _
                        , ctx.GetParam("AMBIENTE", ConstSession.Ambiente) _
                        , ctx.GetParam("IdEnte", myParamSearch.IdEnte) _
                        , ctx.GetParam("COGNOME", "") _
                        , ctx.GetParam("NOME", "") _
                        , ctx.GetParam("CF", "") _
                        , ctx.GetParam("PIVA", "") _
                        , ctx.GetParam("NTESSERA", "") _
                        , ctx.GetParam("CHIUSE", myParamSearch.Chiusa) _
                        , ctx.GetParam("VIA", oReplace.ReplaceCharsForSearch(myParamSearch.sVia)) _
                        , ctx.GetParam("CIVICO", oReplace.ReplaceCharsForSearch(myParamSearch.sCivico)) _
                        , ctx.GetParam("INTERNO", oReplace.ReplaceCharsForSearch(myParamSearch.sInterno)) _
                        , ctx.GetParam("FOGLIO", myParamSearch.sFoglio) _
                        , ctx.GetParam("NUMERO", myParamSearch.sNumero) _
                        , ctx.GetParam("SUBALTERNO", myParamSearch.sSubalterno) _
                        , ctx.GetParam("Dal", oReplace.FormattaData(myParamSearch.Dal, "A")) _
                        , ctx.GetParam("Al", oReplace.FormattaData(myParamSearch.Al, "A")) _
                        , ctx.GetParam("LISTCONTRIB", myParamSearch.sListContrib) _
                        , ctx.GetParam("IDPROVENIENZA", myParamSearch.sProvenienza) _
                        , ctx.GetParam("TYPESOGRES", -1) _
                        , ctx.GetParam("CATCATASTALE", myParamSearch.IdCatCatastale) _
                        , ctx.GetParam("IDCATEGORIA", myParamSearch.IdCatTARES) _
                        , ctx.GetParam("NC", myParamSearch.nComponenti) _
                        , ctx.GetParam("ISPF", myParamSearch.IsPF) _
                        , ctx.GetParam("ISPV", myParamSearch.IsPV) _
                        , ctx.GetParam("ISESENTE", myParamSearch.IsEsente) _
                        , ctx.GetParam("HASMOREUI", myParamSearch.HasMoreUI) _
                        , ctx.GetParam("IDRIDUZIONE", myParamSearch.IdRiduzione) _
                        , ctx.GetParam("IDDETASSAZIONE", myParamSearch.IdDetassazione) _
                        , ctx.GetParam("IDSTATOOCCUPAZIONE", myParamSearch.IdStatoOccupazione)
                    )
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                nListDich += 1
                oDich = New ObjTestataSearch
                oDich.DescrizioneEnte = StringOperation.FormatString(dtMyRow("DESCRIZIONE_ENTE"))
                oDich.IdContribuente = StringOperation.FormatInt(dtMyRow("COD_CONTRIBUENTE"))
                oDich.Id = StringOperation.FormatInt(dtMyRow("ID"))
                oDich.IdTestata = StringOperation.FormatInt(dtMyRow("IDTESTATA"))
                oDich.sCognome = StringOperation.FormatString(dtMyRow("COGNOME_DENOMINAZIONE"))
                oDich.sNome = StringOperation.FormatString(dtMyRow("NOME"))
                oDich.sCfPiva = StringOperation.FormatString(dtMyRow("CF_PIVA"))
                oDich.tDataDichiarazione = StringOperation.FormatDateTime(dtMyRow("DATA_DICHIARAZIONE"))
                oDich.sNDichiarazione = StringOperation.FormatString(dtMyRow("NUMERO_DICHIARAZIONE"))
                oDich.sVia = StringOperation.FormatString(dtMyRow("via"))
                oDich.sCivico = StringOperation.FormatString(dtMyRow("civico"))
                oDich.sEsponente = StringOperation.FormatString(dtMyRow("esponente"))
                oDich.sScala = StringOperation.FormatString(dtMyRow("scala"))
                oDich.sInterno = StringOperation.FormatString(dtMyRow("interno"))
                oDich.sFoglio = StringOperation.FormatString(dtMyRow("foglio"))
                oDich.sNumero = StringOperation.FormatString(dtMyRow("numero"))
                oDich.sSubalterno = StringOperation.FormatString(dtMyRow("subalterno"))
                oDich.Chiusa = StringOperation.FormatInt(dtMyRow("CHIUSA"))
                oDich.tDataInizio = StringOperation.FormatDateTime(dtMyRow("DATA_INIZIO"))
                oDich.tDataFine = StringOperation.FormatDateTime(dtMyRow("DATA_FINE"))
                'dimensiono l'array
                ReDim Preserve oListToReturned(nListDich)
                'memorizzo i dati nell'array
                oListToReturned(nListDich) = oDich
            Next

            HttpContext.Current.Session("myDvResult") = oListToReturned
            Return oListToReturned
        Catch Err As Exception
            Log.Debug(myParamSearch.IdEnte + " - OPENgovTIA.ClsDichiarazione.GetSoggettiFromImmobili.errore: ", Err)
            sErrDichiarazione = "GetSoggettiFromImmobili::Si è verificato il seguente errore: " & Err.Message()
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetSoggettiFromImmobili(ByVal myStringConnection As String, ByVal myParamSearch As ObjSearchTestata, ByRef sErrDichiarazione As String) As ObjTestataSearch()
    '    'Public Function GetSoggettiFromImmobili(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal myParamSearch As ObjSearchTestata, ByRef sErrDichiarazione As String) As ObjTestataSearch()
    '    'Public Function GetSoggettiFromImmobili(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sVia As String, ByVal sCivico As String, ByVal sInterno As String, ByVal FlagChiuse As Integer, ByRef sErrDichiarazione As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal sSubalterno As String, ByVal sDal As String, ByVal sAl As String, ByVal sListContrib As String) As ObjTestataSearch()
    '    Dim oDich As ObjTestataSearch
    '    Dim nListDich As Integer = -1
    '    Dim oListToReturned() As ObjTestataSearch = Nothing
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetDichiarazioni"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPERICERCA", SqlDbType.VarChar)).Value = "I"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = myParamSearch.IdEnte 'sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVENIENZA", SqlDbType.VarChar)).Value = myParamSearch.sProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CHIUSE", SqlDbType.Int)).Value = myParamSearch.Chiusa
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sVia)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sCivico)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sInterno)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.VarChar)).Value = myParamSearch.sFoglio
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.VarChar)).Value = myParamSearch.sNumero
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.VarChar)).Value = myParamSearch.sSubalterno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATCATASTALE", SqlDbType.VarChar)).Value = myParamSearch.IdCatCatastale
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.Int)).Value = myParamSearch.IdCatTARES
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NC", SqlDbType.Int)).Value = myParamSearch.nComponenti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISPF", SqlDbType.Bit)).Value = myParamSearch.IsPF
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISPV", SqlDbType.Bit)).Value = myParamSearch.IsPV
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISESENTE", SqlDbType.Bit)).Value = myParamSearch.IsEsente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@HASMOREUI", SqlDbType.Bit)).Value = myParamSearch.HasMoreUI
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRIDUZIONE", SqlDbType.VarChar)).Value = myParamSearch.IdRiduzione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETASSAZIONE", SqlDbType.VarChar)).Value = myParamSearch.IdDetassazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDSTATOOCCUPAZIONE", SqlDbType.VarChar)).Value = myParamSearch.IdStatoOccupazione
    '        '*** 20140923 - GIS ***
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(myParamSearch.Dal, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(myParamSearch.Al, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LISTCONTRIB", SqlDbType.NVarChar)).Value = myParamSearch.sListContrib
    '        '*** ***
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            nListDich += 1
    '            oDich = New ObjTestataSearch
    '            '*** 201511 - Funzioni Sovracomunali ***
    '            oDich.DescrizioneEnte = StringOperation.FormatString(dtMyRow("DESCRIZIONE_ENTE"))
    '            '*** ***
    '            oDich.IdContribuente = StringOperation.Formatint(dtMyRow("COD_CONTRIBUENTE"))
    '            oDich.Id = StringOperation.Formatint(dtMyRow("ID"))
    '            oDich.IdTestata = StringOperation.Formatint(dtMyRow("IDTESTATA"))
    '            If Not IsDBNull(dtMyRow("COGNOME_DENOMINAZIONE")) Then
    '                oDich.sCognome = StringOperation.FormatString(dtMyRow("COGNOME_DENOMINAZIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("NOME")) Then
    '                oDich.sNome = StringOperation.FormatString(dtMyRow("NOME"))
    '            End If
    '            If Not IsDBNull(dtMyRow("CF_PIVA")) Then
    '                oDich.sCfPiva = StringOperation.FormatString(dtMyRow("CF_PIVA"))
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_DICHIARAZIONE")) Then
    '                oDich.tDataDichiarazione = StringOperation.Formatdatetime(dtMyRow("DATA_DICHIARAZIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("NUMERO_DICHIARAZIONE")) Then
    '                oDich.sNDichiarazione = StringOperation.FormatString(dtMyRow("NUMERO_DICHIARAZIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("via")) Then
    '                oDich.sVia = StringOperation.FormatString(dtMyRow("via"))
    '            End If
    '            If Not IsDBNull(dtMyRow("civico")) Then
    '                oDich.sCivico = StringOperation.FormatString(dtMyRow("civico"))
    '            End If
    '            If Not IsDBNull(dtMyRow("esponente")) Then
    '                oDich.sEsponente = StringOperation.FormatString(dtMyRow("esponente"))
    '            End If
    '            If Not IsDBNull(dtMyRow("scala")) Then
    '                oDich.sScala = StringOperation.FormatString(dtMyRow("scala"))
    '            End If
    '            If Not IsDBNull(dtMyRow("interno")) Then
    '                oDich.sInterno = StringOperation.FormatString(dtMyRow("interno"))
    '            End If
    '            If Not IsDBNull(dtMyRow("foglio")) Then
    '                oDich.sFoglio = StringOperation.FormatString(dtMyRow("foglio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("numero")) Then
    '                oDich.sNumero = StringOperation.FormatString(dtMyRow("numero"))
    '            End If
    '            If Not IsDBNull(dtMyRow("subalterno")) Then
    '                oDich.sSubalterno = StringOperation.FormatString(dtMyRow("subalterno"))
    '            End If
    '            If Not IsDBNull(dtMyRow("CHIUSA")) Then
    '                oDich.Chiusa = StringOperation.Formatint(dtMyRow("CHIUSA"))
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_INIZIO")) Then
    '                oDich.tDataInizio = StringOperation.Formatdatetime(dtMyRow("DATA_INIZIO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_FINE")) Then
    '                oDich.tDataFine = StringOperation.Formatdatetime(dtMyRow("DATA_FINE"))
    '            End If
    '            'dimensiono l'array
    '            ReDim Preserve oListToReturned(nListDich)
    '            'memorizzo i dati nell'array
    '            oListToReturned(nListDich) = oDich
    '        Next

    '        HttpContext.Current.Session("myDvResult") = oListToReturned
    '        Return oListToReturned
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetSoggettiFromImmobili.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        sErrDichiarazione = "GetSoggettiFromImmobili::Si è verificato il seguente errore: " & Err.Message()
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '*** 201511 - Funzioni Sovracomunali ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sTipoStampa"></param>
    ''' <param name="myParamSearch"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetStampaDichiarazioni(ByVal myStringConnection As String, ByVal sTipoStampa As String, ByVal myParamSearch As ObjSearchTestata) As DataView
        Dim dvMyDati As New DataView

        Try
            'prelevo i dati
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, myStringConnection)
            Dim sSQL As String = ""
            Dim sTipoRicerca As String
            If myParamSearch.rbSoggetto Then
                sTipoRicerca = "S"
            Else
                sTipoRicerca = "I"
            End If
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetStampaDichiarazioni", "TYPESTAMPA", "TYPERICERCA", "IDENTE", "IDPROVENIENZA", "DAL", "AL", "CHIUSE", "COGNOME", "NOME", "CF", "PIVA", "TYPESOGRES", "NTESSERA", "VIA", "CIVICO", "INTERNO", "FOGLIO", "NUMERO", "SUBALTERNO", "CATCATASTALE", "IDCATEGORIA", "NC", "ISPF", "ISPV", "ISESENTE", "HASMOREUI", "IDRIDUZIONE", "IDDETASSAZIONE", "IDSTATOOCCUPAZIONE", "LISTCONTRIB")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TYPESTAMPA", sTipoStampa) _
                        , ctx.GetParam("TYPERICERCA", sTipoRicerca) _
                        , ctx.GetParam("IdEnte", myParamSearch.IdEnte) _
                        , ctx.GetParam("IDPROVENIENZA", myParamSearch.sProvenienza) _
                        , ctx.GetParam("Dal", oReplace.FormattaData(myParamSearch.Dal, "A")) _
                        , ctx.GetParam("Al", oReplace.FormattaData(myParamSearch.Al, "A")) _
                        , ctx.GetParam("CHIUSE", myParamSearch.Chiusa) _
                        , ctx.GetParam("COGNOME", oReplace.ReplaceCharsForSearch(myParamSearch.sCognome)) _
                        , ctx.GetParam("NOME", oReplace.ReplaceCharsForSearch(myParamSearch.sNome)) _
                        , ctx.GetParam("CF", oReplace.ReplaceCharsForSearch(myParamSearch.sCF)) _
                        , ctx.GetParam("PIVA", oReplace.ReplaceCharsForSearch(myParamSearch.sPIVA)) _
                        , ctx.GetParam("TYPESOGRES", myParamSearch.TypeSogRes) _
                        , ctx.GetParam("NTESSERA", oReplace.ReplaceCharsForSearch(myParamSearch.sNTessera)) _
                        , ctx.GetParam("VIA", oReplace.ReplaceCharsForSearch(myParamSearch.sVia)) _
                        , ctx.GetParam("CIVICO", oReplace.ReplaceCharsForSearch(myParamSearch.sCivico)) _
                        , ctx.GetParam("INTERNO", oReplace.ReplaceCharsForSearch(myParamSearch.sInterno)) _
                        , ctx.GetParam("FOGLIO", myParamSearch.sFoglio) _
                        , ctx.GetParam("NUMERO", myParamSearch.sNumero) _
                        , ctx.GetParam("SUBALTERNO", myParamSearch.sSubalterno) _
                        , ctx.GetParam("CATCATASTALE", myParamSearch.IdCatCatastale) _
                        , ctx.GetParam("IDCATEGORIA", myParamSearch.IdCatTARES) _
                        , ctx.GetParam("NC", myParamSearch.nComponenti) _
                        , ctx.GetParam("ISPF", myParamSearch.IsPF) _
                        , ctx.GetParam("ISPV", myParamSearch.IsPV) _
                        , ctx.GetParam("ISESENTE", myParamSearch.IsEsente) _
                        , ctx.GetParam("HASMOREUI", myParamSearch.HasMoreUI) _
                        , ctx.GetParam("IDRIDUZIONE", myParamSearch.IdRiduzione) _
                        , ctx.GetParam("IDDETASSAZIONE", myParamSearch.IdDetassazione) _
                        , ctx.GetParam("IDSTATOOCCUPAZIONE", myParamSearch.IdStatoOccupazione) _
                        , ctx.GetParam("LISTCONTRIB", myParamSearch.sListContrib)
                )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(myParamSearch.IdEnte + " - OPENgovTIA.ClsDichiarazione.GetStampaDichiarazioni.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetStampaDichiarazioni(ByVal myStringConnection As String, ByVal sTipoStampa As String, ByVal myParamSearch As ObjSearchTestata) As DataView
    '    'Public Function GetStampaDichiarazioni(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sTipoStampa As String, ByVal myParamSearch As ObjSearchTestata) As DataView
    '    'Public Function GetStampaDichiarazioni(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sTipoStampa As String, ByVal sTipoRicerca As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCodFiscale As String, ByVal sPIva As String, ByVal sNTessera As String, ByVal sVia As String, ByVal sCivico As String, ByVal sInterno As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal sSubalterno As String, ByVal FlagAperta As Integer) As DataView
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()

    '    Try
    '        'prelevo i dati della testata
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetStampaDichiarazioni"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPESTAMPA", SqlDbType.VarChar)).Value = sTipoStampa
    '        If myParamSearch.rbSoggetto Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPERICERCA", SqlDbType.VarChar)).Value = "S"
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPERICERCA", SqlDbType.VarChar)).Value = "I"
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = myParamSearch.IdEnte 'sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVENIENZA", SqlDbType.VarChar)).Value = myParamSearch.sProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(myParamSearch.Dal, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(myParamSearch.Al, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CHIUSE", SqlDbType.Int)).Value = myParamSearch.Chiusa
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sCognome)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sNome)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CF", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sCF)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIVA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sPIVA)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPESOGRES", SqlDbType.Int)).Value = myParamSearch.TypeSogRes
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NTESSERA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sNTessera)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sVia)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sCivico)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(myParamSearch.sInterno)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.VarChar)).Value = myParamSearch.sFoglio
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.VarChar)).Value = myParamSearch.sNumero
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.VarChar)).Value = myParamSearch.sSubalterno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATCATASTALE", SqlDbType.VarChar)).Value = myParamSearch.IdCatCatastale
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.Int)).Value = myParamSearch.IdCatTARES
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NC", SqlDbType.Int)).Value = myParamSearch.nComponenti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISPF", SqlDbType.Bit)).Value = myParamSearch.IsPF
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISPV", SqlDbType.Bit)).Value = myParamSearch.IsPV
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISESENTE", SqlDbType.Bit)).Value = myParamSearch.IsEsente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@HASMOREUI", SqlDbType.Bit)).Value = myParamSearch.HasMoreUI
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRIDUZIONE", SqlDbType.VarChar)).Value = myParamSearch.IdRiduzione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETASSAZIONE", SqlDbType.VarChar)).Value = myParamSearch.IdDetassazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDSTATOOCCUPAZIONE", SqlDbType.VarChar)).Value = myParamSearch.IdStatoOccupazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LISTCONTRIB", SqlDbType.NVarChar)).Value = myParamSearch.sListContrib
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        Return dtMyDati.DefaultView
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetStampaDichiarazioni.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="TipoTassazione"></param>
    ''' <param name="TipoCalcolo"></param>
    ''' <param name="PercentTariffe"></param>
    ''' <param name="HasMaggiorazione"></param>
    ''' <param name="HasConferimenti"></param>
    ''' <param name="TipoMQ"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nIdTestata"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <param name="tDataInizioConf"></param>
    ''' <param name="tDataFineConf"></param>
    ''' <param name="cmdMyCommandOut"></param>
    ''' <returns></returns>
    Public Function GetDichPerCalcolo(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMQ As String, ByVal sAnno As String, ByVal nIdTestata As Integer, ByVal nIdContribuente As Integer, tDataInizioConf As DateTime, tDataFineConf As DateTime, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjAvviso()
        Dim CurrentItem As ObjAvviso
        Dim MyArray As New ArrayList
        Dim MyArrayUI As New ArrayList
        Dim nListAvvisi, nListUI As Integer
        Dim FncTessere As New GestTessera
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0
        Dim nPartite As Integer = 0
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            nListAvvisi = -1 : nListUI = -1
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetAvvisi", "IDENTE", "ANNO", "IDTESTATA", "IDCONTRIBUENTE", "TIPOMQ", "INIZIOCONF", "FINECONF")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("ANNO", sAnno) _
                            , ctx.GetParam("IDTESTATA", nIdTestata) _
                            , ctx.GetParam("IDCONTRIBUENTE", nIdContribuente) _
                            , ctx.GetParam("TIPOMQ", "D") _
                            , ctx.GetParam("INIZIOCONF", oReplace.FormattaData(tDataInizioConf.ToString(), "A")) _
                            , ctx.GetParam("FINECONF", oReplace.FormattaData(tDataFineConf.ToString(), "A"))
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    Log.Debug("GetDichPerCalcolo::contrib::" & StringOperation.FormatInt(myRow("IDCONTRIBUENTE").ToString))
                    nAvanzamento += 1
                    sAvanzamento = "Analisi posizione " & nAvanzamento & " su " & myDataView.Count
                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                    CurrentItem = New ObjAvviso
                    nPartite = 0
                    MyArrayUI.Clear()
                    'carico l'avviso
                    CurrentItem = LoadAvviso(myRow)
                    If CurrentItem Is Nothing Then
                        Return Nothing
                    End If
                    'carico la parte fissa
                    MyArrayUI = LoadPartite(myStringConnection, myRow, ObjArticolo.PARTEFISSA & TipoTassazione, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
                    If MyArrayUI Is Nothing Then
                        Return Nothing
                    End If
                    nPartite += MyArrayUI.Count
                    Log.Debug("GetDichPerCalcolo::caricato PF " & MyArrayUI.Count)
                    'controllo se devo calcolare TARES (PF+PV)
                    If TipoTassazione = ObjRuolo.TipoCalcolo.TARES Then
                        'carico la parte variabile
                        MyArrayUI = LoadPartite(myStringConnection, myRow, ObjArticolo.PARTEVARIABILE, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
                        If MyArrayUI Is Nothing Then
                            Return Nothing
                        End If
                        nPartite += MyArrayUI.Count
                        Log.Debug("GetDichPerCalcolo::caricato PV " & MyArrayUI.Count)
                    End If
                    'controllo se devo calcolare MAGGIORAZIONE
                    If HasMaggiorazione = True Then
                        MyArrayUI = LoadPartite(myStringConnection, myRow, ObjArticolo.PARTEMAGGIORAZIONE, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
                        If MyArrayUI Is Nothing Then
                            Return Nothing
                        End If
                        nPartite += MyArrayUI.Count
                        Log.Debug("GetDichPerCalcolo::caricato PM " & MyArrayUI.Count)
                    End If
                    'controllo se devo calcolare CONFERIMENTI
                    If HasConferimenti = True Then
                        MyArrayUI = LoadPartite(myStringConnection, myRow, ObjArticolo.PARTECONFERIMENTI, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
                        If MyArrayUI Is Nothing Then
                            Return Nothing
                        End If
                        nPartite += MyArrayUI.Count
                        Log.Debug("GetDichPerCalcolo::caricato PC " & MyArrayUI.Count)
                    End If
                    CurrentItem.oUI = CType(MyArrayUI.ToArray(GetType(ObjUnitaImmobiliare)), ObjUnitaImmobiliare())
                    MyArrayUI.Clear()
                    '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
                    If TipoCalcolo = ObjRuolo.Ruolo.AConguaglio Or TipoCalcolo = ObjRuolo.Ruolo.Singolo Then
                        CurrentItem.oArticoliPrec = CType(LoadPartitePrec(myStringConnection, CurrentItem.IdEnte, CurrentItem.IdContribuente, CurrentItem.sAnnoRiferimento).ToArray(GetType(ObjArticolo)), ObjArticolo())
                    End If
                    '*** ***
                    'carico le tessere
                    CurrentItem.oTessere = FncTessere.GetTessera(myStringConnection, CurrentItem.IdEnte, CurrentItem.IdContribuente, -1, "", -1, CurrentItem.sAnnoRiferimento, -1, False, False)
                    If CurrentItem Is Nothing Then
                        Return Nothing
                    End If
                    If nPartite > 0 Then
                        MyArray.Add(CurrentItem)
                    Else
                        Log.Debug("scartato contribuente perchè non ha partite su cui calcolare")
                    End If
                Next
            End Using

            Return CType(MyArray.ToArray(GetType(ObjAvviso)), ObjAvviso())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", Err)
            Return Nothing
        Finally
        myDataView.Dispose()
        End Try
    End Function
    'Public Function GetDichPerCalcolo(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMQ As String, ByVal sAnno As String, ByVal nIdTestata As Integer, ByVal nIdContribuente As Integer, tDataInizioConf As DateTime, tDataFineConf As DateTime, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjAvviso()
    '    Dim CurrentItem As ObjAvviso
    '    Dim MyArray As New ArrayList
    '    Dim MyArrayUI As New ArrayList
    '    Dim nListAvvisi, nListUI As Integer
    '    Dim FncTessere As New GestTessera
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0
    '    Dim nPartite As Integer = 0

    '    Try
    '        nListAvvisi = -1 : nListUI = -1
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetAvvisi"
    '        Log.Debug("GetDichPerCalcolo::devo eseguire prc_GetAvvisi")

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INIZIOCONF", SqlDbType.VarChar)).Value = oReplace.FormattaData(tDataInizioConf.ToString(), "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FINECONF", SqlDbType.VarChar)).Value = oReplace.FormattaData(tDataFineConf.ToString(), "A")
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            Log.Debug("GetDichPerCalcolo::contrib::" & StringOperation.FormatInt(dtMyRow("IDCONTRIBUENTE").ToString))
    '            nAvanzamento += 1
    '            sAvanzamento = "Analisi posizione " & nAvanzamento & " su " & dtMyDati.Rows.Count
    '            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '            CurrentItem = New ObjAvviso
    '            nPartite = 0
    '            MyArrayUI.Clear()
    '            'carico l'avviso
    '            CurrentItem = LoadAvviso(dtMyRow)
    '            If CurrentItem Is Nothing Then
    '                Return Nothing
    '            End If
    '            'carico la parte fissa
    '            MyArrayUI = LoadPartite(myStringConnection, dtMyRow, ObjArticolo.PARTEFISSA & TipoTassazione, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
    '            If MyArrayUI Is Nothing Then
    '                Return Nothing
    '            End If
    '            nPartite += MyArrayUI.Count
    '            Log.Debug("GetDichPerCalcolo::caricato PF " & MyArrayUI.Count)
    '            'controllo se devo calcolare TARES (PF+PV)
    '            If TipoTassazione = ObjRuolo.TipoCalcolo.TARES Then
    '                'carico la parte variabile
    '                MyArrayUI = LoadPartite(myStringConnection, dtMyRow, ObjArticolo.PARTEVARIABILE, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
    '                If MyArrayUI Is Nothing Then
    '                    Return Nothing
    '                End If
    '                nPartite += MyArrayUI.Count
    '                Log.Debug("GetDichPerCalcolo::caricato PV " & MyArrayUI.Count)
    '            End If
    '            'controllo se devo calcolare MAGGIORAZIONE
    '            If HasMaggiorazione = True Then
    '                MyArrayUI = LoadPartite(myStringConnection, dtMyRow, ObjArticolo.PARTEMAGGIORAZIONE, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
    '                If MyArrayUI Is Nothing Then
    '                    Return Nothing
    '                End If
    '                nPartite += MyArrayUI.Count
    '                Log.Debug("GetDichPerCalcolo::caricato PM " & MyArrayUI.Count)
    '            End If
    '            'controllo se devo calcolare CONFERIMENTI
    '            If HasConferimenti = True Then
    '                MyArrayUI = LoadPartite(myStringConnection, dtMyRow, ObjArticolo.PARTECONFERIMENTI, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI)
    '                If MyArrayUI Is Nothing Then
    '                    Return Nothing
    '                End If
    '                nPartite += MyArrayUI.Count
    '                Log.Debug("GetDichPerCalcolo::caricato PC " & MyArrayUI.Count)
    '            End If
    '            CurrentItem.oUI = CType(MyArrayUI.ToArray(GetType(ObjUnitaImmobiliare)), ObjUnitaImmobiliare())
    '            MyArrayUI.Clear()
    '            '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    '            If TipoCalcolo = ObjRuolo.Ruolo.AConguaglio Or TipoCalcolo = ObjRuolo.Ruolo.Supplettivo Then
    '                CurrentItem.oArticoliPrec = CType(LoadPartitePrec(myStringConnection, CurrentItem.IdEnte, CurrentItem.IdContribuente, CurrentItem.sAnnoRiferimento).ToArray(GetType(ObjArticolo)), ObjArticolo())
    '            End If
    '            '*** ***
    '            'carico le tessere
    '            CurrentItem.oTessere = FncTessere.GetTessera(myStringConnection, CurrentItem.IdEnte, CurrentItem.IdContribuente, -1, "", -1, CurrentItem.sAnnoRiferimento, -1, False, False)
    '            If CurrentItem Is Nothing Then
    '                Return Nothing
    '            End If
    '            Log.Debug("GetDichPerCalcolo::caricato TESSERE " & CurrentItem.oTessere.GetUpperBound(0).ToString())
    '            If nPartite > 0 Then
    '                MyArray.Add(CurrentItem)
    '            Else
    '                Log.Debug("scartato contribuente perchè non ha partite su cui calcolare")
    '            End If
    '        Next
    '        Log.Debug("GetDichPerCalcolo::esco")

    '        Return CType(MyArray.ToArray(GetType(ObjAvviso)), ObjAvviso())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '    End Try
    'End Function
    'Public Function GetDichPerCalcolo(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMQ As String, ByVal sAnno As String, ByVal nIdTestata As Integer, ByVal nIdContribuente As Integer, tDataInizioConf As DateTime, tDataFineConf As DateTime, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjAvviso()
    '    Dim CurrentItem As ObjAvviso
    '    Dim MyArray As New ArrayList
    '    Dim MyArrayUI As New ArrayList
    '    Dim nListAvvisi, nListUI As Integer
    '    Dim FncTessere As New GestTessera
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0
    '    Dim nPartite As Integer = 0

    '    Try
    '        nListAvvisi = -1 : nListUI = -1
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetAvvisi"
    '        Log.Debug("GetDichPerCalcolo::devo eseguire prc_GetAvvisi")

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INIZIOCONF", SqlDbType.VarChar)).Value = oReplace.FormattaData(tDataInizioConf.ToString(), "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FINECONF", SqlDbType.VarChar)).Value = oReplace.FormattaData(tDataFineConf.ToString(), "A")
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            Log.Debug("GetDichPerCalcolo::contrib::" & StringOperation.FormatInt(dtMyRow("IDCONTRIBUENTE").ToString))
    '            nAvanzamento += 1
    '            sAvanzamento = "Analisi posizione " & nAvanzamento & " su " & dtMyDati.Rows.Count
    '            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '            CurrentItem = New ObjAvviso
    '            nPartite = 0
    '            MyArrayUI.Clear()
    '            'carico l'avviso
    '            CurrentItem = LoadAvviso(dtMyRow)
    '            If CurrentItem Is Nothing Then
    '                Return Nothing
    '            End If
    '            'carico la parte fissa
    '            MyArrayUI = LoadPartite(dtMyRow, ObjArticolo.PARTEFISSA & TipoTassazione, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI, cmdMyCommand)
    '            If MyArrayUI Is Nothing Then
    '                Return Nothing
    '            End If
    '            nPartite += MyArrayUI.Count
    '            Log.Debug("GetDichPerCalcolo::caricato PF " & MyArrayUI.Count)
    '            'controllo se devo calcolare TARES (PF+PV)
    '            If TipoTassazione = ObjRuolo.TipoCalcolo.TARES Then
    '                'carico la parte variabile
    '                MyArrayUI = LoadPartite(dtMyRow, ObjArticolo.PARTEVARIABILE, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI, cmdMyCommand)
    '                If MyArrayUI Is Nothing Then
    '                    Return Nothing
    '                End If
    '                nPartite += MyArrayUI.Count
    '                Log.Debug("GetDichPerCalcolo::caricato PV " & MyArrayUI.Count)
    '            End If
    '            'controllo se devo calcolare MAGGIORAZIONE
    '            If HasMaggiorazione = True Then
    '                MyArrayUI = LoadPartite(dtMyRow, ObjArticolo.PARTEMAGGIORAZIONE, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI, cmdMyCommand)
    '                If MyArrayUI Is Nothing Then
    '                    Return Nothing
    '                End If
    '                nPartite += MyArrayUI.Count
    '                Log.Debug("GetDichPerCalcolo::caricato PM " & MyArrayUI.Count)
    '            End If
    '            'controllo se devo calcolare CONFERIMENTI
    '            If HasConferimenti = True Then
    '                MyArrayUI = LoadPartite(dtMyRow, ObjArticolo.PARTECONFERIMENTI, PercentTariffe, TipoMQ, tDataInizioConf, tDataFineConf, MyArrayUI, cmdMyCommand)
    '                If MyArrayUI Is Nothing Then
    '                    Return Nothing
    '                End If
    '                nPartite += MyArrayUI.Count
    '                Log.Debug("GetDichPerCalcolo::caricato PC " & MyArrayUI.Count)
    '            End If
    '            CurrentItem.oUI = CType(MyArrayUI.ToArray(GetType(ObjUnitaImmobiliare)), ObjUnitaImmobiliare())
    '            MyArrayUI.Clear()
    '            '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    '            If TipoCalcolo = ObjRuolo.Ruolo.AConguaglio Or TipoCalcolo = ObjRuolo.Ruolo.Supplettivo Then
    '                CurrentItem.oArticoliPrec = CType(LoadPartitePrec(CurrentItem.IdEnte, CurrentItem.IdContribuente, CurrentItem.sAnnoRiferimento, cmdMyCommand).ToArray(GetType(ObjArticolo)), ObjArticolo())
    '            End If
    '            '*** ***
    '            'carico le tessere
    '            CurrentItem.oTessere = FncTessere.GetTessera(CurrentItem.IdEnte, CurrentItem.IdContribuente, -1, "", -1, CurrentItem.sAnnoRiferimento, -1, False, False, cmdMyCommand)
    '            If CurrentItem Is Nothing Then
    '                Return Nothing
    '            End If
    '            Log.Debug("GetDichPerCalcolo::caricato TESSERE " & CurrentItem.oTessere.GetUpperBound(0).ToString())
    '            If nPartite > 0 Then
    '                MyArray.Add(CurrentItem)
    '            Else
    '                Log.Debug("scartato contribuente perchè non ha partite su cui calcolare")
    '            End If
    '        Next
    '        Log.Debug("GetDichPerCalcolo::esco")

    '        Return CType(MyArray.ToArray(GetType(ObjAvviso)), ObjAvviso())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dtMyRow"></param>
    ''' <returns></returns>
    Private Function LoadAvviso(ByVal dtMyRow As DataRowView) As ObjAvviso
        Dim CurrentItem As New ObjAvviso

        Try
            CurrentItem.IdEnte = StringOperation.FormatString(dtMyRow("idente"))
            CurrentItem.IdContribuente = StringOperation.FormatInt(dtMyRow("IDCONTRIBUENTE").ToString)
            CurrentItem.sAnnoRiferimento = StringOperation.FormatString(dtMyRow("anno"))
            CurrentItem.sCognome = StringOperation.FormatString(dtMyRow("cognome"))
            CurrentItem.sNome = StringOperation.FormatString(dtMyRow("nome"))
            CurrentItem.sCodFiscale = StringOperation.FormatString(dtMyRow("cod_fiscale"))
            CurrentItem.sPIVA = StringOperation.FormatString(dtMyRow("partita_iva"))
            CurrentItem.sIndirizzoRes = StringOperation.FormatString(dtMyRow("via_res"))
            CurrentItem.sCivicoRes = StringOperation.FormatString(dtMyRow("civico_res"))
            CurrentItem.sCAPRes = StringOperation.FormatString(dtMyRow("cap_res"))
            CurrentItem.sComuneRes = StringOperation.FormatString(dtMyRow("comune_res"))
            CurrentItem.sProvRes = StringOperation.FormatString(dtMyRow("pv_res"))
            CurrentItem.sFrazRes = StringOperation.FormatString(dtMyRow("frazione_res"))
            CurrentItem.sNominativoCO = StringOperation.FormatString(dtMyRow("nominativo_co"))
            CurrentItem.sIndirizzoCO = StringOperation.FormatString(dtMyRow("via_co"))
            CurrentItem.sCivicoCO = StringOperation.FormatString(dtMyRow("civico_co"))
            CurrentItem.sCAPCO = StringOperation.FormatString(dtMyRow("cap_co"))
            CurrentItem.sComuneCO = StringOperation.FormatString(dtMyRow("comune_co"))
            CurrentItem.sProvCO = StringOperation.FormatString(dtMyRow("pv_co"))
            CurrentItem.sFrazCO = StringOperation.FormatString(dtMyRow("frazione_co"))
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadAvviso.errore: ", Err)
            CurrentItem = Nothing
        End Try
        Return CurrentItem
    End Function
    'Private Function LoadAvviso(ByVal dtMyRow As DataRow) As ObjAvviso
    '    Dim CurrentItem As New ObjAvviso

    '    Try
    '        CurrentItem.IdEnte = dtMyRow("idente")
    '        CurrentItem.IdContribuente = StringOperation.FormatInt(dtMyRow("IDCONTRIBUENTE").ToString)
    '        CurrentItem.sAnnoRiferimento = dtMyRow("anno")
    '        'CurrentItem.IdTestata = dtmyrow("IDTESTATA")

    '        If Not IsDBNull(dtMyRow("cognome")) Then
    '            CurrentItem.sCognome = StringOperation.FormatString(dtMyRow("cognome"))
    '        End If
    '        If Not IsDBNull(dtMyRow("nome")) Then
    '            CurrentItem.sNome = StringOperation.FormatString(dtMyRow("nome"))
    '        End If
    '        If Not IsDBNull(dtMyRow("cod_fiscale")) Then
    '            CurrentItem.sCodFiscale = StringOperation.FormatString(dtMyRow("cod_fiscale"))
    '        End If
    '        If Not IsDBNull(dtMyRow("partita_iva")) Then
    '            CurrentItem.sPIVA = StringOperation.FormatString(dtMyRow("partita_iva"))
    '        End If
    '        If Not IsDBNull(dtMyRow("via_res")) Then
    '            CurrentItem.sIndirizzoRes = StringOperation.FormatString(dtMyRow("via_res"))
    '        End If
    '        If Not IsDBNull(dtMyRow("civico_res")) Then
    '            CurrentItem.sCivicoRes = StringOperation.FormatString(dtMyRow("civico_res"))
    '        End If
    '        If Not IsDBNull(dtMyRow("cap_res")) Then
    '            CurrentItem.sCAPRes = StringOperation.FormatString(dtMyRow("cap_res"))
    '        End If
    '        If Not IsDBNull(dtMyRow("comune_res")) Then
    '            CurrentItem.sComuneRes = StringOperation.FormatString(dtMyRow("comune_res"))
    '        End If
    '        If Not IsDBNull(dtMyRow("pv_res")) Then
    '            CurrentItem.sProvRes = StringOperation.FormatString(dtMyRow("pv_res"))
    '        End If
    '        If Not IsDBNull(dtMyRow("frazione_res")) Then
    '            CurrentItem.sFrazRes = StringOperation.FormatString(dtMyRow("frazione_res"))
    '        End If
    '        If Not IsDBNull(dtMyRow("nominativo_co")) Then
    '            CurrentItem.sNominativoCO = StringOperation.FormatString(dtMyRow("nominativo_co"))
    '        End If
    '        If Not IsDBNull(dtMyRow("via_co")) Then
    '            CurrentItem.sIndirizzoCO = StringOperation.FormatString(dtMyRow("via_co"))
    '        End If
    '        If Not IsDBNull(dtMyRow("civico_co")) Then
    '            CurrentItem.sCivicoCO = StringOperation.FormatString(dtMyRow("civico_co"))
    '        End If
    '        If Not IsDBNull(dtMyRow("cap_co")) Then
    '            CurrentItem.sCAPCO = StringOperation.FormatString(dtMyRow("cap_co"))
    '        End If
    '        If Not IsDBNull(dtMyRow("comune_co")) Then
    '            CurrentItem.sComuneCO = StringOperation.FormatString(dtMyRow("comune_co"))
    '        End If
    '        If Not IsDBNull(dtMyRow("pv_co")) Then
    '            CurrentItem.sProvCO = StringOperation.FormatString(dtMyRow("pv_co"))
    '        End If
    '        If Not IsDBNull(dtMyRow("frazione_co")) Then
    '            CurrentItem.sFrazCO = StringOperation.FormatString(dtMyRow("frazione_co"))
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadAvviso.errore: ", Err)
    '        CurrentItem = Nothing
    '    End Try
    '    Return CurrentItem
    'End Function
    '*** 20181011 Dal/Al Conferimenti ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="dtRow"></param>
    ''' <param name="TipoPartita"></param>
    ''' <param name="PercentTariffe"></param>
    ''' <param name="TipoMQ"></param>
    ''' <param name="tDataInizioConf"></param>
    ''' <param name="tDataFineConf"></param>
    ''' <param name="MyArrayUI"></param>
    ''' <returns></returns>
    Private Function LoadPartite(myConnectionString As String, ByVal dtRow As DataRowView, ByVal TipoPartita As String, ByVal PercentTariffe As Double, ByVal TipoMQ As String, tDataInizioConf As DateTime, tDataFineConf As DateTime, ByRef MyArrayUI As ArrayList) As ArrayList
        Dim CurrentItem As New ObjUnitaImmobiliare
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Dim MyProcedure As String = "prc_GetPF_TARES"
        Dim FncRidEse As New GestRidEse
        Dim oRicRidEse As New ObjRidEse
        Dim MyArrayRid As New ArrayList
        Dim MyArrayDet As New ArrayList
        Dim IdPrec As Integer

        Try
            If MyArrayUI Is Nothing Then
                MyArrayUI = New ArrayList
            End If
            Select Case TipoPartita
                Case ObjArticolo.PARTEFISSA & ObjRuolo.TipoCalcolo.TARSU
                    MyProcedure = "prc_GetPF_TARSU"
                Case ObjArticolo.PARTEVARIABILE
                    MyProcedure = "prc_GetPV_TARES"
                Case ObjArticolo.PARTEMAGGIORAZIONE
                    MyProcedure = "prc_GetPM"
                Case ObjArticolo.PARTECONFERIMENTI
                    MyProcedure = "prc_GetPC"
            End Select
            If TipoPartita.StartsWith(ObjArticolo.PARTEFISSA) Then
                TipoPartita = ObjArticolo.PARTEFISSA
            End If
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, MyProcedure, "IDTESTATA", "ANNO", "PERCENTTARIFFE", "TIPOMQ", "INIZIOCONF", "FINECONF")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESTATA", StringOperation.FormatInt(dtRow("idtestata"))) _
                            , ctx.GetParam("ANNO", StringOperation.FormatInt(dtRow("Anno"))) _
                            , ctx.GetParam("PERCENTTARIFFE", PercentTariffe) _
                            , ctx.GetParam("TIPOMQ", TipoMQ) _
                            , ctx.GetParam("INIZIOCONF", oReplace.FormattaData(tDataInizioConf, "A")) _
                            , ctx.GetParam("FINECONF", oReplace.FormattaData(tDataFineConf, "A"))
                        )
                    For Each myRow As DataRowView In dvMyDati
                        If StringOperation.FormatInt(myRow("id")) <> IdPrec Then
                            If CurrentItem.Id > 0 Then
                                MyArrayUI.Add(CurrentItem)
                            End If
                            CurrentItem = New ObjUnitaImmobiliare
                            MyArrayRid = New ArrayList
                            MyArrayDet = New ArrayList
                            CurrentItem.TipoPartita = TipoPartita
                            CurrentItem.Id = myRow("id")
                            CurrentItem.IdDettaglioTestata = StringOperation.FormatInt(myRow("IDDETTAGLIOTESTATA"))
                            CurrentItem.tDataInizio = StringOperation.FormatDateTime(myRow("DATA_INIZIO"))
                            CurrentItem.tDataFine = StringOperation.FormatDateTime(myRow("DATA_FINE"))
                            CurrentItem.sVia = StringOperation.FormatString(myRow("VIA"))
                            CurrentItem.sCivico = StringOperation.FormatString(myRow("CIVICO"))
                            CurrentItem.sEsponente = StringOperation.FormatString(myRow("ESPONENTE"))
                            CurrentItem.sInterno = StringOperation.FormatString(myRow("INTERNO"))
                            CurrentItem.sScala = StringOperation.FormatString(myRow("SCALA"))
                            CurrentItem.sFoglio = StringOperation.FormatString(myRow("foglio"))
                            CurrentItem.sNumero = StringOperation.FormatString(myRow("numero"))
                            CurrentItem.sSubalterno = StringOperation.FormatString(myRow("subalterno"))
                            CurrentItem.nMQ = StringOperation.FormatDouble(myRow("MQ"))
                            CurrentItem.nGGTarsu = StringOperation.FormatInt(myRow("GGTARSU"))
                            CurrentItem.nNComponenti = StringOperation.FormatInt(myRow("nc"))
                            '*** 20140701 - IMU/TARES ***
                            CurrentItem.nComponentiPV = StringOperation.FormatInt(myRow("NC_PV"))
                            CurrentItem.bForzaPV = StringOperation.FormatBool(myRow("FORZA_CALCOLAPV"))
                            '*** ***
                            CurrentItem.IdCategoria = StringOperation.FormatString(myRow("IDCATEGORIA"))
                            CurrentItem.sCatAteco = StringOperation.FormatString(myRow("descrcat"))
                            CurrentItem.IdTariffa = StringOperation.FormatInt(myRow("IDTARIFFA"))
                            CurrentItem.impTariffa = StringOperation.FormatDouble(myRow("IMPORTO_TARIFFA"))
                            '***Agenzia Entrate***
                            CurrentItem.sSezione = StringOperation.FormatString(myRow("sezione"))
                            CurrentItem.sEstensioneParticella = StringOperation.FormatString(myRow("estensione_particella"))
                            CurrentItem.sIdTipoParticella = StringOperation.FormatString(myRow("id_tipo_particella"))
                            CurrentItem.nIdTitoloOccupaz = StringOperation.FormatInt(myRow("id_titolo_occupazione"))
                            CurrentItem.nIdNaturaOccupaz = StringOperation.FormatInt(myRow("id_natura_occupante"))
                            CurrentItem.nIdDestUso = StringOperation.FormatInt(myRow("id_destinazione_uso"))
                            CurrentItem.sIdTipoUnita = StringOperation.FormatString(myRow("id_tipo_unita"))
                            CurrentItem.nIdAssenzaDatiCatastali = StringOperation.FormatInt(myRow("id_assenza_dati_catastali"))
                            '*********************
                            '*** 20141211 - legami PF-PV ***
                            'i legami sono presenti solo sugli oggetti di tipo partevariabile
                            If TipoPartita = ObjArticolo.PARTEVARIABILE Then
                                'carico i legami tra pf e pv
                                CurrentItem.ListPFvsPV = LoadLegamiPFPV(myConnectionString, CurrentItem.Id, StringOperation.FormatInt(dtRow("idtestata")), StringOperation.FormatInt(dtRow("anno")), PercentTariffe, TipoMQ)
                            End If
                            '*** ***
                            ' BD 09/07/2021
                            'If TipoPartita = ObjArticolo.PARTEVARIABILE Then
                            CurrentItem.ImportoFissoRid = StringOperation.FormatDouble(myRow("importo_fissorid"))
                            'End If
                            ' BD 09/07/2021
                        End If
                        Dim CurrentRid As New ObjRidEseApplicati
                        CurrentRid.sTipoValore = StringOperation.FormatString(myRow("tiporid"))
                        CurrentRid.sValore = StringOperation.FormatString(myRow("valorerid"))
                        CurrentRid.ID = StringOperation.FormatInt(myRow("IDRID"))
                        CurrentRid.IdRiferimento = StringOperation.FormatInt(myRow("IDRID"))
                        CurrentRid.sCodice = StringOperation.FormatString(myRow("CODRID"))
                        CurrentRid.sDescrizione = StringOperation.FormatString(myRow("DESCRRID"))
                        If CurrentRid.sValore <> "" Then
                            MyArrayRid.Add(CurrentRid)
                            CurrentItem.oRiduzioni = CType(MyArrayRid.ToArray(GetType(ObjRidEseApplicati)), ObjRidEseApplicati())
                        End If

                        Dim CurrentDet As New ObjRidEseApplicati
                        CurrentDet.sTipoValore = StringOperation.FormatString(myRow("tipoDet"))
                        CurrentDet.sValore = StringOperation.FormatString(myRow("valoreDet"))
                        CurrentDet.ID = StringOperation.FormatInt(myRow("IDDET"))
                        CurrentDet.IdRiferimento = StringOperation.FormatInt(myRow("IDDET"))
                        CurrentDet.sCodice = StringOperation.FormatString(myRow("CODDET"))
                        CurrentDet.sDescrizione = StringOperation.FormatString(myRow("DESCRDET"))
                        If CurrentDet.sValore <> "" Then
                            MyArrayDet.Add(CurrentDet)
                            CurrentItem.oDetassazioni = CType(MyArrayDet.ToArray(GetType(ObjRidEseApplicati)), ObjRidEseApplicati())
                        End If

                        IdPrec = StringOperation.FormatInt(myRow("id"))
                    Next
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPartite.erroreQuery: ", ex)
                    MyArrayUI = Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
            If CurrentItem.Id > 0 Then
                MyArrayUI.Add(CurrentItem)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPartite.errore: ", Err)
            MyArrayUI = Nothing
        Finally
            dvMyDati.Dispose()
        End Try
        Return MyArrayUI
    End Function
    'Private Function LoadPartite(ByVal dtMyRow As DataRow, ByVal TipoPartita As String, ByVal PercentTariffe As Double, ByVal TipoMQ As String, tDataInizioConf As DateTime, tDataFineConf As DateTime, ByRef MyArrayUI As ArrayList, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ArrayList
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim CurrentItem As New ObjUnitaImmobiliare
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtRow As DataRow
    '    Dim MyProcedure As String = "prc_GetPF_TARES"
    '    Dim FncRidEse As New GestRidEse
    '    Dim oRicRidEse As New ObjRidEse
    '    Dim MyArrayRid As New ArrayList
    '    Dim MyArrayDet As New ArrayList
    '    Dim IdPrec As Integer

    '    Try
    '        If MyArrayUI Is Nothing Then
    '            MyArrayUI = New ArrayList
    '        End If
    '        Select Case TipoPartita
    '            Case ObjArticolo.PARTEFISSA & ObjRuolo.TipoCalcolo.TARSU
    '                MyProcedure = "prc_GetPF_TARSU"
    '            Case ObjArticolo.PARTEVARIABILE
    '                MyProcedure = "prc_GetPV_TARES"
    '            Case ObjArticolo.PARTEMAGGIORAZIONE
    '                MyProcedure = "prc_GetPM"
    '            Case ObjArticolo.PARTECONFERIMENTI
    '                MyProcedure = "prc_GetPC"
    '        End Select
    '        If TipoPartita.StartsWith(ObjArticolo.PARTEFISSA) Then
    '            TipoPartita = ObjArticolo.PARTEFISSA
    '        End If
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If

    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        'Log.Debug("CalcolaRuoloFromDich::devo eseguire " & MyProcedure)
    '        cmdMyCommand.CommandText = MyProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = dtMyRow("idtestata")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = dtMyRow("Anno")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PercentTariffe", SqlDbType.Float)).Value = PercentTariffe
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TipoMQ", SqlDbType.VarChar)).Value = TipoMQ
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INIZIOCONF", SqlDbType.VarChar)).Value = oReplace.FormattaData(tDataInizioConf, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FINECONF", SqlDbType.VarChar)).Value = oReplace.FormattaData(tDataFineConf, "A")
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtRow In dtMyDati.Rows
    '            If StringOperation.FormatInt(dtRow("id")) <> IdPrec Then
    '                If CurrentItem.Id > 0 Then
    '                    MyArrayUI.Add(CurrentItem)
    '                End If
    '                CurrentItem = New ObjUnitaImmobiliare
    '                MyArrayRid = New ArrayList
    '                MyArrayDet = New ArrayList
    '                CurrentItem.TipoPartita = TipoPartita
    '                CurrentItem.Id = dtRow("id")
    '                CurrentItem.IdDettaglioTestata = dtRow("IDDETTAGLIOTESTATA")
    '                CurrentItem.tDataInizio = dtRow("DATA_INIZIO")
    '                If Not IsDBNull(dtRow("DATA_FINE")) Then
    '                    CurrentItem.tDataFine = dtRow("DATA_FINE")
    '                End If
    '                If Not IsDBNull(dtRow("via")) Then
    '                    CurrentItem.sVia = StringOperation.FormatString(dtRow("VIA"))
    '                End If
    '                If Not IsDBNull(dtRow("civico")) Then
    '                    CurrentItem.sCivico = StringOperation.FormatString(dtRow("CIVICO"))
    '                End If
    '                If Not IsDBNull(dtRow("esponente")) Then
    '                    CurrentItem.sEsponente = StringOperation.FormatString(dtRow("ESPONENTE"))
    '                End If
    '                If Not IsDBNull(dtRow("interno")) Then
    '                    CurrentItem.sInterno = StringOperation.FormatString(dtRow("INTERNO"))
    '                End If
    '                If Not IsDBNull(dtRow("scala")) Then
    '                    CurrentItem.sScala = StringOperation.FormatString(dtRow("SCALA"))
    '                End If
    '                If Not IsDBNull(dtRow("foglio")) Then
    '                    CurrentItem.sFoglio = StringOperation.FormatString(dtRow("foglio"))
    '                End If
    '                If Not IsDBNull(dtRow("numero")) Then
    '                    CurrentItem.sNumero = StringOperation.FormatString(dtRow("numero"))
    '                End If
    '                If Not IsDBNull(dtRow("subalterno")) Then
    '                    CurrentItem.sSubalterno = StringOperation.FormatString(dtRow("subalterno"))
    '                End If
    '                CurrentItem.nMQ = stringoperation.formatdouble(dtRow("MQ"))
    '                If Not IsDBNull(dtRow("GGTARSU")) Then
    '                    CurrentItem.nGGTarsu = dtRow("GGTARSU")
    '                End If
    '                If Not IsDBNull(dtRow("nc")) Then
    '                    CurrentItem.nNComponenti = StringOperation.FormatString(dtRow("nc"))
    '                End If
    '                '*** 20140701 - IMU/TARES ***
    '                If Not IsDBNull(dtRow("NC_PV")) Then
    '                    CurrentItem.nComponentiPV = StringOperation.FormatString(dtRow("NC_PV"))
    '                End If
    '                If Not IsDBNull(dtRow("FORZA_CALCOLAPV")) Then
    '                    CurrentItem.bForzaPV = StringOperation.FormatString(dtRow("FORZA_CALCOLAPV"))
    '                End If
    '                '*** ***
    '                CurrentItem.IdCategoria = StringOperation.FormatString(dtRow("IDCATEGORIA"))
    '                CurrentItem.sCatAteco = StringOperation.FormatString(dtRow("descrcat"))
    '                CurrentItem.IdTariffa = StringOperation.FormatInt(dtRow("IDTARIFFA"))
    '                CurrentItem.impTariffa = stringoperation.formatdouble(dtRow("IMPORTO_TARIFFA"))

    '                '***Agenzia Entrate***
    '                If Not IsDBNull(dtRow("sezione")) Then
    '                    CurrentItem.sSezione = StringOperation.FormatString(dtRow("sezione"))
    '                End If
    '                If Not IsDBNull(dtRow("estensione_particella")) Then
    '                    CurrentItem.sEstensioneParticella = StringOperation.FormatString(dtRow("estensione_particella"))
    '                End If
    '                If Not IsDBNull(dtRow("id_tipo_particella")) Then
    '                    CurrentItem.sIdTipoParticella = StringOperation.FormatString(dtRow("id_tipo_particella"))
    '                End If
    '                If Not IsDBNull(dtRow("id_titolo_occupazione")) Then
    '                    CurrentItem.nIdTitoloOccupaz = StringOperation.FormatInt(dtRow("id_titolo_occupazione"))
    '                End If
    '                If Not IsDBNull(dtRow("id_natura_occupante")) Then
    '                    CurrentItem.nIdNaturaOccupaz = StringOperation.FormatInt(dtRow("id_natura_occupante"))
    '                End If
    '                If Not IsDBNull(dtRow("id_destinazione_uso")) Then
    '                    CurrentItem.nIdDestUso = StringOperation.FormatInt(dtRow("id_destinazione_uso"))
    '                End If
    '                If Not IsDBNull(dtRow("id_tipo_unita")) Then
    '                    CurrentItem.sIdTipoUnita = StringOperation.FormatString(dtRow("id_tipo_unita"))
    '                End If
    '                If Not IsDBNull(dtRow("id_assenza_dati_catastali")) Then
    '                    CurrentItem.nIdAssenzaDatiCatastali = StringOperation.FormatInt(dtRow("id_assenza_dati_catastali"))
    '                End If
    '                '*********************
    '                '*** 20141211 - legami PF-PV ***
    '                'i legami sono presenti solo sugli oggetti di tipo partevariabile
    '                If TipoPartita = ObjArticolo.PARTEVARIABILE Then
    '                    'carico i legami tra pf e pv
    '                    CurrentItem.ListPFvsPV = LoadLegamiPFPV(CurrentItem.Id, StringOperation.FormatInt(dtRow("idtestata")), StringOperation.FormatInt(dtRow("anno")), PercentTariffe, TipoMQ, cmdMyCommand)
    '                End If
    '                '*** ***
    '            End If
    '            Dim CurrentRid As New ObjRidEseApplicati
    '            If Not IsDBNull(dtRow("tiporid")) Then
    '                CurrentRid.sTipoValore = StringOperation.FormatString(dtRow("tiporid"))
    '            End If
    '            If Not IsDBNull(dtRow("valorerid")) Then
    '                CurrentRid.sValore = StringOperation.FormatString(dtRow("valorerid"))
    '            End If
    '            If Not IsDBNull(dtRow("IDRID")) Then
    '                CurrentRid.ID = StringOperation.FormatString(dtRow("IDRID"))
    '                CurrentRid.IdRiferimento = StringOperation.FormatString(dtRow("IDRID"))
    '            End If
    '            If Not IsDBNull(dtRow("CODRID")) Then
    '                CurrentRid.sCodice = StringOperation.FormatString(dtRow("CODRID"))
    '            End If
    '            If Not IsDBNull(dtRow("DESCRRID")) Then
    '                CurrentRid.sDescrizione = StringOperation.FormatString(dtRow("DESCRRID"))
    '            End If
    '            If CurrentRid.sValore <> "" Then
    '                MyArrayRid.Add(CurrentRid)
    '                CurrentItem.oRiduzioni = CType(MyArrayRid.ToArray(GetType(ObjRidEseApplicati)), ObjRidEseApplicati())
    '            End If

    '            Dim CurrentDet As New ObjRidEseApplicati
    '            If Not IsDBNull(dtRow("tipoDet")) Then
    '                CurrentDet.sTipoValore = StringOperation.FormatString(dtRow("tipoDet"))
    '            End If
    '            If Not IsDBNull(dtRow("valoreDet")) Then
    '                CurrentDet.sValore = StringOperation.FormatString(dtRow("valoreDet"))
    '            End If
    '            If Not IsDBNull(dtRow("IDDET")) Then
    '                CurrentDet.ID = StringOperation.FormatString(dtRow("IDDET"))
    '                CurrentDet.IdRiferimento = StringOperation.FormatString(dtRow("IDDET"))
    '            End If
    '            If Not IsDBNull(dtRow("CODDET")) Then
    '                CurrentDet.sCodice = StringOperation.FormatString(dtRow("CODDET"))
    '            End If
    '            If Not IsDBNull(dtRow("DESCRDET")) Then
    '                CurrentDet.sDescrizione = StringOperation.FormatString(dtRow("DESCRDET"))
    '            End If
    '            If CurrentDet.sValore <> "" Then
    '                MyArrayDet.Add(CurrentDet)
    '                CurrentItem.oDetassazioni = CType(MyArrayDet.ToArray(GetType(ObjRidEseApplicati)), ObjRidEseApplicati())
    '            End If

    '            IdPrec = StringOperation.FormatInt(dtRow("id"))
    '        Next
    '        If CurrentItem.Id > 0 Then
    '            MyArrayUI.Add(CurrentItem)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPartite.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        MyArrayUI = Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    '    Return MyArrayUI
    'End Function
    '*** ***
    '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <param name="sAnno"></param>
    ''' <returns></returns>
    Private Function LoadPartitePrec(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal sAnno As String) As ArrayList
        Dim CurrentItem As New ObjArticolo
        Dim MyArrayPrec As New ArrayList
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPartitePrec", "IDENTE", "IDCONTRIBUENTE", "ANNO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDCONTRIBUENTE", nIdContribuente) _
                            , ctx.GetParam("ANNO", sAnno)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPartitePrec.erroreQuery: ", ex)
                    MyArrayPrec = Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    CurrentItem = New ObjArticolo
                    CurrentItem.IdEnte = sIdEnte
                    CurrentItem.IdContribuente = nIdContribuente
                    CurrentItem.sAnno = sAnno
                    CurrentItem.TipoPartita = StringOperation.FormatString(myRow("tipopartita"))
                    CurrentItem.sVia = StringOperation.FormatString(myRow("via"))
                    CurrentItem.impNetto = StringOperation.FormatDouble(myRow("importo_netto"))
                    MyArrayPrec.Add(CurrentItem)
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPartitePrec.errore: ", Err)
            MyArrayPrec = Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return MyArrayPrec
    End Function
    'Private Function LoadPartitePrec(ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal sAnno As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ArrayList
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim CurrentItem As New ObjArticolo
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtRow As DataRow
    '    Dim MyArrayPrec As New ArrayList

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        'Log.Debug("CalcolaRuoloFromDich::devo eseguire " & MyProcedure)
    '        cmdMyCommand.CommandText = "prc_GetPartitePrec"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.VarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.VarChar)).Value = sAnno
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtRow In dtMyDati.Rows
    '            CurrentItem = New ObjArticolo
    '            CurrentItem.IdEnte = sIdEnte
    '            CurrentItem.IdContribuente = nIdContribuente
    '            CurrentItem.sAnno = sAnno
    '            If Not IsDBNull(dtRow("tipopartita")) Then
    '                CurrentItem.TipoPartita = dtRow("tipopartita")
    '            End If
    '            If Not IsDBNull(dtRow("via")) Then
    '                CurrentItem.sVia = StringOperation.FormatString(dtRow("via"))
    '            End If
    '            If Not IsDBNull(dtRow("importo_netto")) Then
    '                CurrentItem.impNetto = stringoperation.formatdouble(dtRow("importo_netto"))
    '            End If
    '            MyArrayPrec.Add(CurrentItem)
    '        Next
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPartitePrec.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        MyArrayPrec = Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    '    Return MyArrayPrec
    'End Function
    '*** ***
    '*** 20141211 - legami PF-PV ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdOggetto"></param>
    ''' <param name="IdTestata"></param>
    ''' <param name="Anno"></param>
    ''' <param name="PercentTariffe"></param>
    ''' <param name="TipoMQ"></param>
    ''' <returns></returns>
    Public Function LoadLegamiPFPV(myStringConnection As String, ByVal IdOggetto As Integer, ByVal IdTestata As Integer, ByVal Anno As Integer, ByVal PercentTariffe As Double, ByVal TipoMQ As String) As ObjLegamePFPV()
        Dim CurrentItem As New ObjLegamePFPV
        Dim MyArray As New ArrayList
        Dim sSQL As String = ""
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLegamePFPV", "IDTESTATA", "ANNO", "PERCENTTARIFFE", "TIPOMQ", "IDPV")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESTATA", IdTestata) _
                            , ctx.GetParam("ANNO", Anno) _
                            , ctx.GetParam("PERCENTTARIFFE", PercentTariffe) _
                            , ctx.GetParam("TIPOMQ", TipoMQ) _
                            , ctx.GetParam("IDPV", IdOggetto)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadLegamiPFVP.erroreQuery: ", ex)
                    MyArray = Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    CurrentItem = New ObjLegamePFPV
                    CurrentItem.IdPF = StringOperation.FormatInt(myRow("idpf"))
                    CurrentItem.IdPV = StringOperation.FormatInt(myRow("idpv"))
                    MyArray.Add(CurrentItem)
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadLegamiPFVP.errore: ", Err)
            MyArray = Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return CType(MyArray.ToArray(GetType(ObjLegamePFPV)), ObjLegamePFPV())
    End Function
    'Private Function LoadLegamiPFPV(ByVal IdOggetto As Integer, ByVal IdTestata As Integer, ByVal Anno As String, ByVal PercentTariffe As Double, ByVal TipoMQ As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjLegamePFPV()
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim CurrentItem As New ObjLegamePFPV
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtRow As DataRow
    '    Dim MyProcedure As String = "prc_GetLegamePFPV"
    '    Dim MyArray As New ArrayList

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        'Log.Debug("CalcolaRuoloFromDich::devo eseguire " & MyProcedure)
    '        cmdMyCommand.CommandText = MyProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = IdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = Anno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PercentTariffe", SqlDbType.Float)).Value = PercentTariffe
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TipoMQ", SqlDbType.VarChar)).Value = TipoMQ
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPV", SqlDbType.Int)).Value = IdOggetto
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtRow In dtMyDati.Rows
    '            CurrentItem = New ObjLegamePFPV
    '            CurrentItem.IdPF = StringOperation.FormatInt(dtRow("idpf"))
    '            CurrentItem.IdPV = StringOperation.FormatInt(dtRow("idpv"))
    '            MyArray.Add(CurrentItem)
    '        Next
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadLegamiPFVP.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        MyArray = Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    '    Return CType(MyArray.ToArray(GetType(ObjLegamePFPV)), ObjLegamePFPV())
    'End Function
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nIdTestata"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <returns></returns>
    Public Function GetDichPerCalcolo(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal sAnno As String, ByVal nIdTestata As Integer, ByVal nIdContribuente As Integer) As ObjAvviso()
        Dim oMyAvviso As New ObjAvviso
        Dim oListAvvisi() As ObjAvviso
        Dim oMyUI As New ObjUnitaImmobiliare
        Dim oListUI() As ObjUnitaImmobiliare = Nothing
        Dim nListAvvisi, nListUI, nIdContribPrec As Integer
        Dim FncTessere As New GestTessera
        Dim FncRidEse As New GestRidEse
        Dim oRicRidEse As New ObjRidEse
        Dim sSQL As String = ""
        Dim myDataView As New DataView

        Try
            nListAvvisi = -1 : nListUI = -1
            'prelevo i dati per il calcolo
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = "SELECT *"
                    sSQL += " FROM V_GETDICHCALCOLO"
                    sSQL += " WHERE (1=1)"
                    sSQL += " AND (IDENTE=@IDENTE)"
                    sSQL += " AND (YEAR(DATA_INIZIO)<=@ANNO)"
                    sSQL += " AND ((YEAR(DATA_FINE) IS NULL) OR (YEAR(DATA_FINE)>=@ANNO))"
                    sSQL += " AND (ANNO=@ANNO)"
                    sSQL += " AND (@IDTESTATA<=0 OR IDTESTATA=@IDTESTATA)"
                    sSQL += " AND (@IDCONTRIBUENTE<=0 OR IDCONTRIBUENTE=@IDCONTRIBUENTE)"
                    sSQL += " ORDER BY IDCONTRIBUENTE, IDDETTAGLIOTESTATA, IDCATEGORIA, DATA_INIZIO, DATA_FINE"
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL, "IDENTE", "ANNO", "IDTESTATA", "IDCONTRIBUENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("ANNO", sAnno) _
                        , ctx.GetParam("IDTESTATA", nIdTestata) _
                        , ctx.GetParam("IDCONTRIBUENTE", nIdContribuente)
                    )
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    If StringOperation.FormatInt(myRow("IDCONTRIBUENTE")) <> nIdContribPrec And Not IsNothing(oMyUI) Then
                        oMyAvviso.oTessere = FncTessere.GetTessera(myConnectionString, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, oMyAvviso.sAnnoRiferimento, -1, False, False)
                        oMyAvviso.oUI = oListUI
                        nListAvvisi += 1
                        ReDim Preserve oListAvvisi(nListAvvisi)
                        oListAvvisi(nListAvvisi) = oMyAvviso
                        nListUI = -1
                    End If
                    Log.Debug("GetDichPerCalcolo::SQL::nListAvvisi::" & nListAvvisi)

                    oMyAvviso = New ObjAvviso
                    oMyUI = New ObjUnitaImmobiliare

                    oMyAvviso.IdEnte = sIdEnte
                    oMyAvviso.IdContribuente = StringOperation.FormatInt(myRow("IDCONTRIBUENTE"))
                    oMyAvviso.sAnnoRiferimento = sAnno

                    oMyAvviso.sCognome = StringOperation.FormatString(myRow("cognome"))
                    oMyAvviso.sNome = StringOperation.FormatString(myRow("nome"))
                    oMyAvviso.sCodFiscale = StringOperation.FormatString(myRow("cod_fiscale"))
                    oMyAvviso.sPIVA = StringOperation.FormatString(myRow("partita_iva"))
                    oMyAvviso.sIndirizzoRes = StringOperation.FormatString(myRow("via_res"))
                    oMyAvviso.sCivicoRes = StringOperation.FormatString(myRow("civico_res"))
                    oMyAvviso.sCAPRes = StringOperation.FormatString(myRow("cap_res"))
                    oMyAvviso.sComuneRes = StringOperation.FormatString(myRow("comune_res"))
                    oMyAvviso.sProvRes = StringOperation.FormatString(myRow("pv_res"))
                    oMyAvviso.sFrazRes = StringOperation.FormatString(myRow("frazione_res"))
                    oMyAvviso.sNominativoCO = StringOperation.FormatString(myRow("nominativo_co"))
                    oMyAvviso.sIndirizzoCO = StringOperation.FormatString(myRow("via_co"))
                    oMyAvviso.sCAPCO = StringOperation.FormatString(myRow("cap_co"))
                    oMyAvviso.sComuneCO = StringOperation.FormatString(myRow("comune_co"))
                    oMyAvviso.sProvCO = StringOperation.FormatString(myRow("pv_co"))
                    oMyAvviso.sFrazCO = StringOperation.FormatString(myRow("frazione_co"))

                    oMyUI.Id = StringOperation.FormatInt(myRow("id"))
                    oMyUI.IdDettaglioTestata = StringOperation.FormatInt(myRow("IDDETTAGLIOTESTATA"))
                    oMyUI.tDataInizio = StringOperation.FormatDateTime(myRow("DATA_INIZIO"))
                    oMyUI.tDataFine = StringOperation.FormatDateTime(myRow("DATA_FINE"))
                    oMyUI.sVia = StringOperation.FormatString(myRow("VIA"))
                    oMyUI.sCivico = StringOperation.FormatString(myRow("CIVICO"))
                    oMyUI.sEsponente = StringOperation.FormatString(myRow("ESPONENTE"))
                    oMyUI.sInterno = StringOperation.FormatString(myRow("INTERNO"))
                    oMyUI.sScala = StringOperation.FormatString(myRow("SCALA"))
                    oMyUI.sFoglio = StringOperation.FormatString(myRow("foglio"))
                    oMyUI.sNumero = StringOperation.FormatString(myRow("numero"))
                    oMyUI.sSubalterno = StringOperation.FormatString(myRow("subalterno"))
                    oMyUI.nMQ = StringOperation.FormatDouble(myRow("MQ"))
                    oMyUI.nGGTarsu = StringOperation.FormatInt(myRow("GGTARSU"))
                    oMyUI.IdCategoria = StringOperation.FormatString(myRow("IDCATEGORIA"))
                    oMyUI.IdTariffa = StringOperation.FormatInt(myRow("IDTARIFFA"))
                    oMyUI.impTariffa = StringOperation.FormatDouble(myRow("IMPORTO_TARIFFA"))

                    '***Agenzia Entrate***
                    oMyUI.sSezione = StringOperation.FormatString(myRow("sezione"))
                    oMyUI.sEstensioneParticella = StringOperation.FormatString(myRow("estensione_particella"))
                    oMyUI.sIdTipoParticella = StringOperation.FormatString(myRow("id_tipo_particella"))
                    oMyUI.nIdTitoloOccupaz = StringOperation.FormatInt(myRow("id_titolo_occupazione"))
                    oMyUI.nIdNaturaOccupaz = StringOperation.FormatInt(myRow("id_natura_occupante"))
                    oMyUI.nIdDestUso = StringOperation.FormatInt(myRow("id_destinazione_uso"))
                    oMyUI.sIdTipoUnita = StringOperation.FormatString(myRow("id_tipo_unita"))
                    oMyUI.nIdAssenzaDatiCatastali = StringOperation.FormatInt(myRow("id_assenza_dati_catastali"))
                    '*********************
                    oRicRidEse.IdEnte = sIdEnte
                    oMyUI.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_UI, oMyUI.Id, "")
                    oMyUI.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_UI, oMyUI.Id, "")

                    nListUI += 1
                    ReDim Preserve oListUI(nListUI)
                    oListUI(nListUI) = oMyUI
                    nIdContribPrec = StringOperation.FormatInt(myRow("IDCONTRIBUENTE"))
                Next
                Log.Debug("GetDichPerCalcolo::devo prelevare tessera per oMyAvviso.IdContribuente::" & oMyAvviso.IdContribuente)
                oMyAvviso.oTessere = FncTessere.GetTessera(myConnectionString, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, oMyAvviso.sAnnoRiferimento, -1, False, False)
                Log.Debug("GetDichPerCalcolo::prelevato tessera")
                oMyAvviso.oUI = oListUI
                nListAvvisi += 1
                ReDim Preserve oListAvvisi(nListAvvisi)
                oListAvvisi(nListAvvisi) = oMyAvviso
                nListUI = -1

                oMyAvviso = New ObjAvviso
                oMyUI = New ObjUnitaImmobiliare

                Log.Debug("GetDichPerCalcolo::esco")
                Return oListAvvisi
            End Using
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", Err)
            oListAvvisi = Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function GetDichPerCalcolo(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal sAnno As String, ByVal nIdTestata As Integer, ByVal nIdContribuente As Integer) As ObjAvviso()
    '    Dim oMyAvviso As New ObjAvviso
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim oMyUI As New ObjUnitaImmobiliare
    '    Dim oListUI() As ObjUnitaImmobiliare = Nothing
    '    Dim nListAvvisi, nListUI, nIdContribPrec As Integer
    '    Dim FncTessere As New GestTessera
    '    Dim FncRidEse As New GestRidEse
    '    Dim oRicRidEse As New ObjRidEse

    '    Try
    '        nListAvvisi = -1 : nListUI = -1
    '        'prelevo i dati per il calcolo
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_GETDICHCALCOLO"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (YEAR(DATA_INIZIO)<=@ANNO)"
    '        cmdMyCommand.CommandText += " AND ((YEAR(DATA_FINE) IS NULL) OR (YEAR(DATA_FINE)>=@ANNO))"
    '        cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '        If nIdTestata <> -1 Then
    '            cmdMyCommand.CommandText += " AND IDTESTATA=@IDTESTATA"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        End If
    '        If nIdContribuente <> -1 Then
    '            cmdMyCommand.CommandText += " AND IDCONTRIBUENTE=@IDCONTRIBUENTE"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY IDCONTRIBUENTE, IDDETTAGLIOTESTATA, IDCATEGORIA, DATA_INIZIO, DATA_FINE"
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        Do While DrReturn.Read
    '            If StringOperation.FormatInt(DrReturn("IDCONTRIBUENTE")) <> nIdContribPrec And Not IsNothing(oMyUI) Then
    '                oMyAvviso.oTessere = FncTessere.GetTessera(myConnectionString, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, oMyAvviso.sAnnoRiferimento, -1, False, False)
    '                oMyAvviso.oUI = oListUI
    '                nListAvvisi += 1
    '                ReDim Preserve oListAvvisi(nListAvvisi)
    '                oListAvvisi(nListAvvisi) = oMyAvviso
    '                nListUI = -1
    '            End If
    '            Log.Debug("GetDichPerCalcolo::SQL::nListAvvisi::" & nListAvvisi)

    '            oMyAvviso = New ObjAvviso
    '            oMyUI = New ObjUnitaImmobiliare

    '            oMyAvviso.IdEnte = sIdEnte
    '            oMyAvviso.IdContribuente = DrReturn("IDCONTRIBUENTE")
    '            oMyAvviso.sAnnoRiferimento = sAnno
    '            'oMyAvviso.IdTestata = DrReturn("IDTESTATA")

    '            If Not IsDBNull(DrReturn("cognome")) Then
    '                oMyAvviso.sCognome = StringOperation.FormatString(DrReturn("cognome"))
    '            End If
    '            If Not IsDBNull(DrReturn("nome")) Then
    '                oMyAvviso.sNome = StringOperation.FormatString(DrReturn("nome"))
    '            End If
    '            If Not IsDBNull(DrReturn("cod_fiscale")) Then
    '                oMyAvviso.sCodFiscale = StringOperation.FormatString(DrReturn("cod_fiscale"))
    '            End If
    '            If Not IsDBNull(DrReturn("partita_iva")) Then
    '                oMyAvviso.sPIVA = StringOperation.FormatString(DrReturn("partita_iva"))
    '            End If
    '            If Not IsDBNull(DrReturn("via_res")) Then
    '                oMyAvviso.sIndirizzoRes = StringOperation.FormatString(DrReturn("via_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("civico_res")) Then
    '                oMyAvviso.sCivicoRes = StringOperation.FormatString(DrReturn("civico_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("cap_res")) Then
    '                oMyAvviso.sCAPRes = StringOperation.FormatString(DrReturn("cap_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("comune_res")) Then
    '                oMyAvviso.sComuneRes = StringOperation.FormatString(DrReturn("comune_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("pv_res")) Then
    '                oMyAvviso.sProvRes = StringOperation.FormatString(DrReturn("pv_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("frazione_res")) Then
    '                oMyAvviso.sFrazRes = StringOperation.FormatString(DrReturn("frazione_res"))
    '            End If
    '            If Not IsDBNull(DrReturn("nominativo_co")) Then
    '                oMyAvviso.sNominativoCO = StringOperation.FormatString(DrReturn("nominativo_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("via_co")) Then
    '                oMyAvviso.sIndirizzoCO = StringOperation.FormatString(DrReturn("via_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("cap_co")) Then
    '                oMyAvviso.sCAPCO = StringOperation.FormatString(DrReturn("cap_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("comune_co")) Then
    '                oMyAvviso.sComuneCO = StringOperation.FormatString(DrReturn("comune_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("pv_co")) Then
    '                oMyAvviso.sProvCO = StringOperation.FormatString(DrReturn("pv_co"))
    '            End If
    '            If Not IsDBNull(DrReturn("frazione_co")) Then
    '                oMyAvviso.sFrazCO = StringOperation.FormatString(DrReturn("frazione_co"))
    '            End If

    '            oMyUI.Id = DrReturn("id")
    '            oMyUI.IdDettaglioTestata = DrReturn("IDDETTAGLIOTESTATA")
    '            oMyUI.tDataInizio = DrReturn("DATA_INIZIO")
    '            If Not IsDBNull(DrReturn("DATA_FINE")) Then
    '                oMyUI.tDataFine = DrReturn("DATA_FINE")
    '            End If
    '            If Not IsDBNull(DrReturn("via")) Then
    '                oMyUI.sVia = StringOperation.FormatString(DrReturn("VIA"))
    '            End If
    '            If Not IsDBNull(DrReturn("civico")) Then
    '                oMyUI.sCivico = StringOperation.FormatString(DrReturn("CIVICO"))
    '            End If
    '            If Not IsDBNull(DrReturn("esponente")) Then
    '                oMyUI.sEsponente = StringOperation.FormatString(DrReturn("ESPONENTE"))
    '            End If
    '            If Not IsDBNull(DrReturn("interno")) Then
    '                oMyUI.sInterno = StringOperation.FormatString(DrReturn("INTERNO"))
    '            End If
    '            If Not IsDBNull(DrReturn("scala")) Then
    '                oMyUI.sScala = StringOperation.FormatString(DrReturn("SCALA"))
    '            End If
    '            If Not IsDBNull(DrReturn("foglio")) Then
    '                oMyUI.sFoglio = StringOperation.FormatString(DrReturn("foglio"))
    '            End If
    '            If Not IsDBNull(DrReturn("numero")) Then
    '                oMyUI.sNumero = StringOperation.FormatString(DrReturn("numero"))
    '            End If
    '            If Not IsDBNull(DrReturn("subalterno")) Then
    '                oMyUI.sSubalterno = StringOperation.FormatString(DrReturn("subalterno"))
    '            End If
    '            oMyUI.nMQ = StringOperation.FormatDouble(DrReturn("MQ"))
    '            If Not IsDBNull(DrReturn("GGTARSU")) Then
    '                oMyUI.nGGTarsu = DrReturn("GGTARSU")
    '            End If
    '            oMyUI.IdCategoria = StringOperation.FormatString(DrReturn("IDCATEGORIA"))
    '            oMyUI.IdTariffa = StringOperation.FormatInt(DrReturn("IDTARIFFA"))
    '            oMyUI.impTariffa = StringOperation.FormatDouble(DrReturn("IMPORTO_TARIFFA"))

    '            '***Agenzia Entrate***
    '            If Not IsDBNull(DrReturn("sezione")) Then
    '                oMyUI.sSezione = StringOperation.FormatString(DrReturn("sezione"))
    '            End If
    '            If Not IsDBNull(DrReturn("estensione_particella")) Then
    '                oMyUI.sEstensioneParticella = StringOperation.FormatString(DrReturn("estensione_particella"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_tipo_particella")) Then
    '                oMyUI.sIdTipoParticella = StringOperation.FormatString(DrReturn("id_tipo_particella"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_titolo_occupazione")) Then
    '                oMyUI.nIdTitoloOccupaz = StringOperation.FormatInt(DrReturn("id_titolo_occupazione"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_natura_occupante")) Then
    '                oMyUI.nIdNaturaOccupaz = StringOperation.FormatInt(DrReturn("id_natura_occupante"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_destinazione_uso")) Then
    '                oMyUI.nIdDestUso = StringOperation.FormatInt(DrReturn("id_destinazione_uso"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_tipo_unita")) Then
    '                oMyUI.sIdTipoUnita = StringOperation.FormatString(DrReturn("id_tipo_unita"))
    '            End If
    '            If Not IsDBNull(DrReturn("id_assenza_dati_catastali")) Then
    '                oMyUI.nIdAssenzaDatiCatastali = StringOperation.FormatInt(DrReturn("id_assenza_dati_catastali"))
    '            End If
    '            '*********************
    '            oRicRidEse.IdEnte = sIdEnte
    '            oMyUI.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_UI, oMyUI.Id, "")
    '            oMyUI.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_UI, oMyUI.Id, "")

    '            nListUI += 1
    '            ReDim Preserve oListUI(nListUI)
    '            oListUI(nListUI) = oMyUI
    '            nIdContribPrec = StringOperation.FormatInt(DrReturn("IDCONTRIBUENTE"))
    '        Loop
    '        Log.Debug("GetDichPerCalcolo::devo prelevare tessera per oMyAvviso.IdContribuente::" & oMyAvviso.IdContribuente)
    '        oMyAvviso.oTessere = FncTessere.GetTessera(myConnectionString, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, oMyAvviso.sAnnoRiferimento, -1, False, False)
    '        Log.Debug("GetDichPerCalcolo::prelevato tessera")
    '        oMyAvviso.oUI = oListUI
    '        nListAvvisi += 1
    '        ReDim Preserve oListAvvisi(nListAvvisi)
    '        oListAvvisi(nListAvvisi) = oMyAvviso
    '        nListUI = -1

    '        oMyAvviso = New ObjAvviso
    '        oMyUI = New ObjUnitaImmobiliare

    '        Log.Debug("GetDichPerCalcolo::esco")
    '        Return oListAvvisi
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetDichPerCalcolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    'Public Function SetDichiarazione(ByVal myConnectionString As String, ByVal oNewTestata As ObjTestata) As Integer
    '    Try
    '        Dim IdNewTestata As Integer
    '        'inserisco i dati della testata
    '        Dim FncTestata As New GestTestata
    '        IdNewTestata = FncTestata.SetTestata(myConnectionString, oNewTestata)
    '        If IdNewTestata <= 0 Then
    '            Return 0
    '        End If
    '        oNewTestata.Id = IdNewTestata

    '        '*** X UNIONE CON BANCADATI CMGC ***
    '        If ConstSession.IsFromVariabile = "1" Then
    '            'inserisco i dati di tessera
    '            Dim FncTessera As New GestTessera
    '            IdNewTestata = FncTessera.SetTesseraCompleta(myConnectionString, oNewTestata.oTessere, IdNewTestata)
    '            If IdNewTestata <= 0 Then
    '                'cancello la testata inserita incompleta
    '                FncTestata.DeleteTestata(myConnectionString, IdNewTestata, Now, 1)
    '                Return 0
    '            End If
    '        Else
    '            'inserisco i dati di immobile
    '            Dim FncDettaglioTestata As New GestDettaglioTestata
    '            IdNewTestata = FncDettaglioTestata.SetDettaglioTestataCompleta(myConnectionString, oNewTestata.oImmobili, IdNewTestata, -1)
    '            If IdNewTestata <= 0 Then
    '                'cancello la testata inserita incompleta
    '                FncTestata.DeleteTestata(myConnectionString, IdNewTestata, Now, 1)
    '                Return 0
    '            End If
    '        End If
    '        'inserisco i dati dei componenti famiglia
    '        If Not oNewTestata.oFamiglia Is Nothing Then
    '            Dim FncFamiglia As New GestFamiglia
    '            If FncFamiglia.SetFamiglia(myConnectionString, oNewTestata.oFamiglia, oNewTestata.IdTestata) = 0 Then
    '                Return 0
    '            End If
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.SetDichiarazione.errore: ",Err)
    '        Return 0
    '    End Try
    'End Function
    '*** ***

    'Public Function GetNDichAutomatico(ByVal myConnectionString As String, ByVal sIdEnte As String) As String
    '    Dim nDichAutomatico As String = ""
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'prelevo i dati
    '        cmdMyCommand.CommandText = "SELECT NDICH= CASE WHEN ISNUMERIC(NUMERO_DICHIARAZIONE)=1 THEN NUMERO_DICHIARAZIONE+1 ELSE NULL END"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " ORDER BY IDTESTATA DESC"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        'eseguo la query
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        If DrReturn.Read Then
    '            If Not IsDBNull(DrReturn("ndich")) Then
    '                nDichAutomatico = StringOperation.FormatString(DrReturn("ndich"))
    '            End If
    '        End If
    '        Return nDichAutomatico
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetNDichAutomatico.errore: ",Err)
    '        Return ""
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sIdEnte"></param>
    ''' <returns></returns>
    Public Function GetNDichAutomatico(ByVal myStringConnection As String, ByVal sIdEnte As String) As String
        Dim nDichAutomatico As String = ""
        Dim sSQL As String = ""
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetNDichAutomatico", "IDENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte))
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.ClsDichiarazione.GetNDichAutomatico.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nDichAutomatico = StringOperation.FormatString(myRow("ndich"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovTIA.ClsDichiarazione.GetNDichAutomatico.errore: ", Err)
        Finally
            myDataView.Dispose()
        End Try
        Return nDichAutomatico
    End Function
    'Public Function GetNDichAutomatico(ByVal myStringConnection As String, ByVal sIdEnte As String) As String
    '    Dim nDichAutomatico As String = ""
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetNDichAutomatico"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sIdEnte
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            If Not IsDBNull(dtMyRow("ndich")) Then
    '                nDichAutomatico = StringOperation.FormatString(dtMyRow("ndich"))
    '            End If
    '        Next
    '        Return nDichAutomatico
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.GetNDichAutomatico.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return ""
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="tDataDich"></param>
    ''' <param name="sNDich"></param>
    ''' <returns></returns>
    Public Function IsUniqueDich(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal tDataDich As Date, ByVal sNDich As String) As Integer
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = 0

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_ISUNIQUEDICH", "IDENTE", "DATADICH", "NDICH")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("DATADICH", tDataDich) _
                            , ctx.GetParam("NDICH", sNDich)
                        )
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.ClsDichiarazione.IsUniqueDich.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nMyReturn = StringOperation.FormatInt(myRow("idtestata"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovTIA.ClsDichiarazione.IsUniqueDich.errore: ", Err)
            nMyReturn = 0
        Finally
            myDataView.Dispose()
        End Try
        Return nMyReturn
    End Function
    'Public Function IsUniqueDich(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal tDataDich As Date, ByVal sNDich As String) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_ISUNIQUEDICH"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADICH", SqlDbType.DateTime)).Value = tDataDich
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NDICH", SqlDbType.NVarChar)).Value = sNDich
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrReturn = cmdMyCommand.ExecuteReader
    '        If DrReturn.Read Then
    '            If Not IsDBNull(DrReturn("idtestata")) Then
    '                Return 0
    '            End If
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.IsUniqueDich.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 0
    '    Finally
    '        DrReturn.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBType"></param>
    ''' <param name="myConnectionString"></param>
    ''' <param name="oTestata"></param>
    ''' <returns></returns>
    Public Function DeleteDichiarazione(DBType As String, ByVal myConnectionString As String, ByVal myConnectionStringGOV As String, ByVal oTestata As ObjTestata) As Integer
        'Dim cancellaT As New GestTestata
        Dim FncTessera As New Utility.DichManagerTARSU(DBType, myConnectionString, myConnectionStringGOV, oTestata.sEnte) 'As New GestTessera
        'Dim cancellaDetT As New GestDettaglioTestata
        Dim cancellaVano As New GestOggetti
        Dim delTestata As Integer = 0
        Dim i, k As Integer

        Try
            If FncTessera.SetTestata(Utility.Costanti.AZIONE_DELETE, oTestata) = 0 Then
                Return 0
            End If

            '*** X UNIONE CON BANCADATI CMGC ***
            Dim ListUI() As ObjDettaglioTestata
            If ConstSession.IsFromVariabile = "1" Then
                For i = 0 To oTestata.oTessere.Length - 1
                    If FncTessera.SetTessera(Utility.Costanti.AZIONE_DELETE, oTestata.oTessere(i), oTestata.oTessere(i).IdContribuente) = 0 Then
                        Return 0
                    Else
                    End If
                Next
                ListUI = oTestata.oTessere(i).oImmobili
            Else
                ListUI = oTestata.oImmobili
            End If
            For Each myItem As ObjDettaglioTestata In ListUI
                If FncTessera.SetDettaglioTestata(Utility.Costanti.AZIONE_DELETE, myItem) = 0 Then
                    Return 0
                Else
                    For k = 0 To myItem.oOggetti.Length - 1
                        If cancellaVano.DeleteOggetti(myConnectionString, myItem.oOggetti(k).IdOggetto, Now, myItem.Id, 0) = 0 Then
                            Return 0
                        End If
                    Next
                End If
            Next
            '*** ***
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.DeleteDichiarazione.errore: ", Err)
            Return 0
        End Try
    End Function

    '*** 20140805 - Gestione Categorie Vani ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdContribuente"></param>
    ''' <param name="sTributo"></param>
    ''' <param name="MyConnectionString"></param>
    ''' <param name="LblNominativo"></param>
    ''' <param name="LblCFPIVA"></param>
    ''' <param name="LblDatiNascita"></param>
    ''' <param name="LblResidenza"></param>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Public Sub LoadPannelAnagrafica(ByVal IdContribuente As Integer, ByVal sTributo As String, ByVal MyConnectionString As String, ByRef LblNominativo As Label, ByRef LblCFPIVA As Label, ByRef LblDatiNascita As Label, ByRef LblResidenza As Label)
        Dim FncAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oMyAnagrafica As New DettaglioAnagrafica

        Try
            LblNominativo.Text = "" : LblCFPIVA.Text = "" : LblDatiNascita.Text = "" : LblResidenza.Text = ""

            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            'oMyAnagrafica = FncAnagrafica.GetAnagrafica(IdContribuente, ConstSession.CodTributo, Costanti.INIT_VALUE_NUMBER, MyConnectionString)
            oMyAnagrafica = FncAnagrafica.GetAnagrafica(IdContribuente, Utility.Costanti.INIT_VALUE_NUMBER, String.Empty, ConstSession.DBType, MyConnectionString, False)
            '*** ***
            LblNominativo.Text = "Nominativo/Ragione Sociale " + oMyAnagrafica.Cognome + " " + oMyAnagrafica.Nome
            LblCFPIVA.Text = "Cod.Fiscale/P.IVA" & Space(5)
            If oMyAnagrafica.PartitaIva <> "" Then
                LblCFPIVA.Text += oMyAnagrafica.PartitaIva
            Else
                LblCFPIVA.Text += oMyAnagrafica.CodiceFiscale
            End If
            LblDatiNascita.Text = "Nato "
            If oMyAnagrafica.DataNascita <> "" And oMyAnagrafica.DataNascita <> "00/00/0000" And oMyAnagrafica.DataNascita <> "00/00/1900" Then
                LblDatiNascita.Text += "il " + oMyAnagrafica.DataNascita
            End If
            If oMyAnagrafica.ComuneNascita <> "" Then
                LblDatiNascita.Text += Space(5) + " a " + oMyAnagrafica.ComuneNascita
            End If
            LblResidenza.Text = "Residente a" + Space(5) + oMyAnagrafica.ViaResidenza + ", " + oMyAnagrafica.CivicoResidenza
            LblResidenza.Text += Space(3) + oMyAnagrafica.CapResidenza + " " + oMyAnagrafica.ComuneResidenza + " (" + oMyAnagrafica.ProvinciaResidenza + ")"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPannelAnagrafica.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTessera"></param>
    ''' <param name="LblCodUtente"></param>
    ''' <param name="LblCodInterno"></param>
    ''' <param name="LblNTessera"></param>
    ''' <param name="LblDataRilascio"></param>
    ''' <param name="LblDataCessazione"></param>
    Public Sub LoadPannelTessera(ByVal oTessera As Object, ByRef LblCodUtente As Label, ByRef LblCodInterno As Label, ByRef LblNTessera As Label, ByRef LblDataRilascio As Label, ByRef LblDataCessazione As Label)
        Dim oMyTessera As New ObjTessera

        Try
            LblCodUtente.Text = "" : LblCodInterno.Text = "" : LblNTessera.Text = "" : LblDataRilascio.Text = "" : LblDataCessazione.Text = ""
            If Not IsNothing(oTessera) Then
                oMyTessera = oTessera
                LblCodUtente.Text = "Cod.Utente " & oMyTessera.sCodUtente
                LblCodInterno.Text = "Cod.Interno " & oMyTessera.sCodInterno
                LblNTessera.Text = "N.Tessera " & oMyTessera.sNumeroTessera
                If oMyTessera.tDataRilascio <> Date.MinValue Then
                    LblDataRilascio.Text = "Rilasciata il " & oMyTessera.tDataRilascio
                Else
                    LblDataRilascio.Text = ""
                End If
                If oMyTessera.tDataCessazione <> Date.MinValue Then
                    LblDataCessazione.Text = "Cessata il " & oMyTessera.tDataCessazione
                Else
                    LblDataCessazione.Text = ""
                End If
            Else
                LblNTessera.Text = "N.Tessera " & ObjTessera.TESSERA_BIDONE
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPannelTessera.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oImmobile"></param>
    ''' <param name="LblUbicazione"></param>
    ''' <param name="LblRifCat"></param>
    ''' <param name="LblDataInizio"></param>
    ''' <param name="LblDataFine"></param>
    ''' <param name="LblMQCatasto"></param>
    ''' <param name="LblMQTassabili"></param>
    ''' <param name="LblCat"></param>
    ''' <param name="LblComponentiPF"></param>
    ''' <param name="LblComponentiPV"></param>
    ''' <param name="LblForzaCalcoloPV"></param>
    Public Sub LoadPannelImmobile(ByVal oImmobile As ObjDettaglioTestata, ByRef LblUbicazione As Label, ByRef LblRifCat As Label, ByRef LblDataInizio As Label, ByRef LblDataFine As Label, ByRef LblMQCatasto As Label, ByRef LblMQTassabili As Label, ByRef LblCat As Label, ByRef LblComponentiPF As Label, ByRef LblComponentiPV As Label, ByRef LblForzaCalcoloPV As Label)
        Dim oMyUI As New ObjDettaglioTestata

        Try
            LblUbicazione.Text = "" : LblRifCat.Text = ""
            LblDataInizio.Text = "" : LblDataFine.Text = "" : LblMQCatasto.Text = "" : LblMQTassabili.Text = ""
            LblCat.Text = "" : LblComponentiPF.Text = "" : LblComponentiPV.Text = "" : LblForzaCalcoloPV.Text = ""
            If Not IsNothing(oImmobile) Then
                oMyUI = oImmobile
                LblUbicazione.Text = oMyUI.sVia + " " + oMyUI.sCivico
                If oMyUI.sEsponente <> "" Then
                    LblUbicazione.Text += " Esp. " + oMyUI.sEsponente
                End If
                If oMyUI.sScala <> "" Then
                    LblUbicazione.Text += " Scala " + oMyUI.sScala
                End If
                If oMyUI.sInterno <> "" Then
                    LblUbicazione.Text += " Int. " + oMyUI.sInterno
                End If
                If oMyUI.sFoglio <> "" Then
                    LblRifCat.Text = "Fg. " + oMyUI.sFoglio
                End If
                If oMyUI.sNumero <> "" Then
                    LblRifCat.Text += " Map. " + oMyUI.sNumero
                End If
                If oMyUI.sSubalterno <> "" Then
                    LblRifCat.Text += " Sub. " + oMyUI.sSubalterno
                End If
                If oMyUI.tDataInizio <> Date.MinValue And oMyUI.tDataInizio <> Date.MaxValue Then
                    LblDataInizio.Text = "Dal " + oMyUI.tDataInizio.ToShortDateString
                End If
                If oMyUI.tDataFine <> Date.MinValue And oMyUI.tDataFine <> Date.MaxValue Then
                    LblDataFine.Text = "Al " + oMyUI.tDataFine.ToShortDateString
                End If
                LblMQCatasto.Text = "MQ Catasto " + oMyUI.nMQCatasto.ToString
                LblMQTassabili.Text = "MQ Tassabili " + oMyUI.nMQTassabili.ToString
                LblCat.Text = "Cat. " + oMyUI.sCatAteco
                If oMyUI.sCatAteco.ToUpper.StartsWith("DOM") Then
                    LblComponentiPF.Text = "Componenti PF " + oMyUI.nNComponenti.ToString
                    LblComponentiPV.Text = "Componenti PV " + oMyUI.nComponentiPV.ToString
                End If
                '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                If Not ConstSession.HasDummyDich Then
                    If oMyUI.bForzaPV = True Then
                        LblForzaCalcoloPV.Text = "Forza Calcolo PV"
                    End If
                End If
                '*** ***
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadPannelImmobile.errore: ", ex)
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sFoglio"></param>
    ''' <param name="sNumero"></param>
    ''' <param name="sSubalterno"></param>
    ''' <param name="tDataInizio"></param>
    ''' <param name="tDataFine"></param>
    ''' <param name="grdICI"></param>
    ''' <param name="cmdNew"></param>
    Public Sub LoadDatiICI(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal sSubalterno As String, ByVal tDataInizio As DateTime, ByVal tDataFine As DateTime, ByVal grdICI As Ribes.OPENgov.WebControls.RibesGridView, ByVal cmdNew As Global.System.Web.UI.WebControls.ImageButton)
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvMyDati As New DataView

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetUIFromICI"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.VarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.VarChar)).Value = sFoglio
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.VarChar)).Value = sNumero
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUB", SqlDbType.VarChar)).Value = sSubalterno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DAL", SqlDbType.DateTime)).Value = tDataInizio
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AL", SqlDbType.DateTime)).Value = tDataFine
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            dvMyDati = dtMyDati.DefaultView
            myAdapter.Dispose()
            If Not dvMyDati Is Nothing Then
                If dvMyDati.Count > 0 Then
                    grdICI.Style.Add("display", "")
                    cmdNew.Style.Add("display", "none")
                    grdICI.SelectedIndex = -1
                    grdICI.DataSource = dvMyDati
                    grdICI.DataBind()
                Else
                    grdICI.Style.Add("display", "none")
                    cmdNew.Style.Add("display", "")
                End If
            Else
                grdICI.Style.Add("display", "none")
                cmdNew.Style.Add("display", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsDichiarazione.LoadDataICI.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
End Class
''' <summary>
''' Classe per la gestione degli avvisi
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestAvviso
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestAvviso))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    '*** 20120917 - sgravi ***
    'Public Function GetAvvisoSgravato(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sCodCartella As String) As Integer
    '    Dim DsDati As DataSet

    '    Try
    '        'PRELEVO I DATI DELLA TESTATA
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLCARTELLE"
    '        cmdMyCommand.CommandText += " WHERE (NOT DATA_VARIAZIONE IS NULL)"
    '        If sCodCartella <> "" Then
    '            cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = sCodCartella
    '        End If
    '        DsDati = WFSessione.oSession.oAppDB.GetPrivateDataSet(cmdMyCommand)
    '        If DsDati.Tables(0).Rows.Count > 0 Then
    '            Return 1
    '        Else
    '            Return 0
    '        End If

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.GetAvvisoSgravato.errore: ", ex)
    '        Log.Debug("Si è verificato un errore in GetAvvisoSgravato::" & ex.Message & "::SQL::" & cmdMyCommand.CommandText & "::param::" & sCodCartella)
    '        Return 0
    '    End Try
    'End Function

    'Public Function GetLockSgravio(ByVal sCodCartella As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    '{-1: non presente procedura sgravio; 0: presente procedura sgravio; 1: presente procedura sgravio altro utente; 2: errore}
    '    Dim DvDati As New DataView
    '    Dim nLock As Integer

    '    Try
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLPROCEDURASGRAVIO"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        If sCodCartella <> "" Then
    '            cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = sCodCartella
    '        End If
    '        'eseguo la query
    '        DvDati = WFSessione.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        If Not DvDati Is Nothing Then
    '            If DvDati.Count > 0 Then
    '                If StringOperation.FormatString(DvDati(0)("OPERATORE")) <> StringOperation.FormatString(ConstSession.UserName) Then
    '                    nLock = 1
    '                Else
    '                    nLock = 0
    '                End If
    '            End If
    '        Else
    '            nLock = -1
    '        End If
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.GetLockSgravio.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestAvviso::GetLockSgravio::" & Err.Message & "::SQL::" & cmdMyCommand.CommandText & "::param::" & sCodCartella)
    '        nLock = 2
    '    End Try
    '    Return nLock
    'End Function

    'Public Function SetLockSgravio(ByVal nDBOperation As Integer, ByVal sCodiceCartella As String, ByVal nIdContribuente As Integer, ByVal nIdFlussoRuolo As Integer, ByVal sOperatore As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Try
    '        Select Case nDBOperation
    '            Case Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLPROCEDURASGRAVIO (CODICE_CARTELLA, IDCONTRIBUENTE, IDFLUSSO_RUOLO, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@CODICE_CARTELLA,@COD_CONTRIBUENTE,@IDFLUSSO_RUOLO,@OPERATORE)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = sCodiceCartella
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_CONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = sOperatore
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLPROCEDURASGRAVIO"
    '                cmdMyCommand.CommandText += " WHERE (CODICE_CARTELLA=@CODICE_CARTELLA)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = sCodiceCartella
    '        End Select
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetLockSgravio.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestAvviso::LockSgravio::" & Err.Message + "::SQL::" + cmdMyCommand.CommandText & "::param::" & sCodiceCartella)
    '        Return -1
    '    End Try
    'End Function

    'Public Function CalcoloAvvisoSgravio(ByRef oMyAvviso As ObjAvviso, ByVal sUsername As String, ByRef sMyErr As String, ByVal WFSessione As OPENUtility.CreateSessione) As Boolean
    '    Dim x As Integer
    '    Dim FncCC As New OPENUtility.ClsContiCorrenti
    '    Dim oContoCorrente As OPENUtility.objContoCorrente
    '    Dim FncRate As New GestRata
    '    Dim FncAddiz As New GestAddizionali
    '    Dim FncLotto As New GestLottoCartellazione
    '    Dim oRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim oAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '    Dim oLottoCartellazione As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '    Dim impPF As Double = 0
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim oAvvisi(0) As ObjAvviso
    '    Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
    '    Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile

    '    Try
    '        'verifico la presenza del conto corrente
    '        oContoCorrente = FncCC.GetContoCorrente(oMyAvviso.IdEnte, "0434", sUsername)
    '        If oContoCorrente Is Nothing Then
    '            sMyErr = "alert('Non e\' presente il Conto Corrente.\nImpossibile proseguire!');"
    '            Return False
    '        End If
    '        'verifico la presenza delle rate
    '        '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '        oRate = FncRate.GetRateConfigurate(oMyAvviso.IdFlussoRuolo, oContoCorrente, WFSessione)
    '        '*** ***
    '        If oRate Is Nothing Then
    '            sMyErr = "alert('Non sono state configurate le rate!\nImpossibile proseguire!');"
    '            Return False
    '        Else
    '            'valorizzo il conto sulle rate
    '            For x = 0 To oRate.GetUpperBound(0)
    '                oRate(x).NumeroContoCorrente = oContoCorrente.ContoCorrente
    '            Next
    '        End If
    '        'verifico la presenza delle addizionali
    '        oAddizionali = FncAddiz.GetAddizionale(oMyAvviso.IdEnte, oMyAvviso.sAnnoRiferimento, "", WFSessione)
    '        If oAddizionali Is Nothing Then
    '            sMyErr = "alert('Non sono state configurate le addizionali!Impossibile proseguire!');"
    '            Return False
    '        End If
    '        'annullo il vecchio avviso
    '        If SetAvviso(oMyAvviso, -1, Costanti.AZIONE_DELETE, WFSessione) <= 0 Then
    '            sMyErr = "alert('Procedura di Sgravio terminata a causa di un errore!')"
    '            Return False
    '        End If
    '        'ricalcolo l'importo carico
    '        impPF = 0
    '        For x = 0 To oMyAvviso.oArticoli.GetUpperBound(0)
    '            impPF += oMyAvviso.oArticoli(x).impNetto
    '        Next
    '        oMyAvviso.impPF = impPF
    '        If FormatNumber(oMyAvviso.impPF, 2) = 0 Then
    '            oMyAvviso.impArrotondamento = 0
    '            oMyAvviso.impCarico = 0
    '            oMyAvviso.impTotale = 0
    '        End If
    '        oAvvisi(0) = oMyAvviso
    '        'attivo il servizio
    '        RemoRuolo = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloTARSU"))
    '        'eseguo la creazione degli articoli
    '        oListAvvisi = RemoRuolo.CartellaAvvisi(oAvvisi, oRate, oAddizionali, Nothing)
    '        'se ha cartellato aggiorno il record per il ruolo
    '        If Not oListAvvisi Is Nothing Then
    '            'ciclo sugli avvisi cartellati
    '            For x = 0 To oListAvvisi.GetUpperBound(0)
    '                'registro l'avviso nella tabella
    '                If SetAvvisoCompleto(oListAvvisi(x), WFSessione) = 0 Then
    '                    sMyErr = "Errore in inserimento Cartellazione"
    '                    'devo eliminare le operazioni fatte finora
    '                    If UndoSgravio(oListAvvisi(x), WFSessione) = 0 Then
    '                        sMyErr += vbCrLf & "Errore in annullo sgravio"
    '                    End If
    '                    Return False
    '                End If
    '                'aggiorno la variabile totale
    '                oMyAvviso = oListAvvisi(x)
    '            Next
    '        Else
    '            sMyErr = "Errore in calcolo Cartellazione"
    '            Return False
    '        End If

    '        Return True
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.CalcoloAvvisoSgravio.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestAvviso::CalcoloAvvisoSgravio::" & Err.Message & "::param::" & oMyAvviso.sCodiceCartella)
    '        Return False
    '    End Try
    'End Function

    'Public Function UndoSgravio(ByVal oMyAvviso As ObjAvviso, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        'cancello tutti i record associati alla riga dell'avviso senza data variazione
    '        'tolgo data variazione alla riga per idavviso
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "sp_UndoSgravio"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = oMyAvviso.sCodiceCartella
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyAvviso.ID
    '        Log.Debug("GestAvviso::UndoSgravio::SQL::" & cmdMyCommand.CommandText & "::param::" & oMyAvviso.sCodiceCartella & "::param::" & oMyAvviso.ID)
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) > 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.UndoSgravio.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestAvviso::UndoSgravio::" & Err.Message & "::SQL::" & cmdMyCommand.CommandText & "::param::" & oMyAvviso.sCodiceCartella & "::param::" & oMyAvviso.ID)
    '        Return 0
    '    End Try
    'End Function
    '*** ***

    'Public Function SetAvvisoCompleto(ByVal oMyAvviso As ObjAvviso, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity, x As Integer
    '    Dim FncTessera As New GestTessera

    '    Try
    '        myIdentity = SetAvviso(oMyAvviso, -1, Costanti.AZIONE_NEW, WFSessione)
    '        If myIdentity <= 0 Then
    '            Return 0
    '        End If
    '        oMyAvviso.ID = myIdentity

    '        'controllo se inserire i dati di tessera
    '        If Not IsNothing(oMyAvviso.oTessere) Then
    '            For x = 0 To oMyAvviso.oTessere.GetUpperBound(0)
    '                If oMyAvviso.oTessere(x).Id = -1 Then
    '                    Dim oListTessere(0) As ObjTessera
    '                    oListTessere(0) = oMyAvviso.oTessere(x)
    '                    myIdentity = FncTessera.SetTesseraCompleta(oListTessere, -1, WFSessione)
    '                    If myIdentity <= 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '                'inserisco il legame tra tessera e avviso
    '                myIdentity = FncTessera.SetTesseraAvviso(oMyAvviso.ID, oMyAvviso.oTessere(x).Id, -1, Costanti.AZIONE_NEW, WFSessione)
    '                If myIdentity <= 0 Then
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        'inserisco gli scaglioni
    '        If Not oMyAvviso.oScaglioni Is Nothing Then
    '            Dim FncScaglioni As New GestScaglione
    '            For x = 0 To oMyAvviso.oScaglioni.GetUpperBound(0)
    '                oMyAvviso.oScaglioni(x).IdAvviso = oMyAvviso.ID
    '                If FncScaglioni.SetScaglione(oMyAvviso.oScaglioni(x), -1, Costanti.AZIONE_NEW, WFSessione) = 0 Then
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        'inserisco gli articoli
    '        If Not oMyAvviso.oArticoli Is Nothing Then
    '            Dim FncArticoli As New GestArticolo
    '            For x = 0 To oMyAvviso.oArticoli.GetUpperBound(0)
    '                oMyAvviso.oArticoli(x).IdAvviso = oMyAvviso.ID
    '                If FncArticoli.SetArticoloCompleto(oMyAvviso.oArticoli(x), WFSessione) = 0 Then
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        'inserisco il dettaglio voci
    '        If Not oMyAvviso.oDetVoci Is Nothing Then
    '            Dim FncDetVoci As New GestDetVoci
    '            For x = 0 To oMyAvviso.oDetVoci.GetUpperBound(0)
    '                oMyAvviso.oDetVoci(x).IdAvviso = oMyAvviso.ID
    '                If FncDetVoci.SetDetVoci(oMyAvviso.oDetVoci(x), -1, Costanti.AZIONE_NEW, WFSessione) = 0 Then
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        'inserisco le rate
    '        If Not oMyAvviso.oRate Is Nothing Then
    '            Dim FncRate As New GestRata
    '            For x = 0 To oMyAvviso.oRate.GetUpperBound(0)
    '                oMyAvviso.oRate(x).IdAvviso = oMyAvviso.ID
    '                If oMyAvviso.oRate(x).impRata > 0 Then
    '                    If FncRate.SetRata(oMyAvviso.oRate(x), -1, Costanti.AZIONE_NEW, WFSessione) = 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '            Next
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetAvvisoCompleto.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetAvviso(ByVal oMyAvviso As ObjAvviso, ByVal nIdFlusso As Integer, ByVal nDBOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        Select Case nDBOperation
    '            Case Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLE(IDENTE, CODICE_CARTELLA, ANNO"
    '                cmdMyCommand.CommandText += ", DATA_EMISSIONE, COD_CONTRIBUENTE, LOTTO_CARTELLAZIONE"
    '                cmdMyCommand.CommandText += ", ANNI_PRESENZA_RUOLO, COGNOME_DENOMINAZIONE, NOME, COD_FISCALE"
    '                cmdMyCommand.CommandText += ", PARTITA_IVA, VIA_RES, CIVICO_RES, CAP_RES, COMUNE_RES, PROVINCIA_RES"
    '                cmdMyCommand.CommandText += ", FRAZIONE_RES, NOMINATIVO_INVIO, VIA_RCP, CIVICO_RCP, CAP_RCP"
    '                cmdMyCommand.CommandText += ", COMUNE_RCP, PROVINCIA_RCP, FRAZIONE_RCP, IMPORTO_TOTALE"
    '                cmdMyCommand.CommandText += ", IMPORTO_ARROTONDAMENTO, IMPORTO_CARICO,  IDFLUSSO_RUOLO,NOTE"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDENTE,@CODICE_CARTELLA,@ANNO "
    '                cmdMyCommand.CommandText += ",@DATA_EMISSIONE,@COD_CONTRIBUENTE,@LOTTO_CARTELLAZIONE "
    '                cmdMyCommand.CommandText += ",@ANNI_PRESENZA_RUOLO,@COGNOME_DENOMINAZIONE,@NOME,@COD_FISCALE "
    '                cmdMyCommand.CommandText += ",@PARTITA_IVA,@VIA_RES,@CIVICO_RES,@CAP_RES,@COMUNE_RES,@PROVINCIA_RES "
    '                cmdMyCommand.CommandText += ",@FRAZIONE_RES,@NOMINATIVO_INVIO,@VIA_RCP,@CIVICO_RCP,@CAP_RCP "
    '                cmdMyCommand.CommandText += ",@COMUNE_RCP,@PROVINCIA_RCP,@FRAZIONE_RCP,@IMPORTO_TOTALE "
    '                cmdMyCommand.CommandText += ",@IMPORTO_ARROTONDAMENTO,@IMPORTO_CARICO,@IDFLUSSO_RUOLO,@NOTE"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyAvviso.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = oMyAvviso.sCodiceCartella
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyAvviso.sAnnoRiferimento
    '                If oMyAvviso.tDataEmissione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_EMISSIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_EMISSIONE", SqlDbType.DateTime)).Value = oMyAvviso.tDataEmissione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_CONTRIBUENTE", SqlDbType.Int)).Value = oMyAvviso.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LOTTO_CARTELLAZIONE", SqlDbType.Int)).Value = oMyAvviso.nLottoCartellazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNI_PRESENZA_RUOLO", SqlDbType.NVarChar)).Value = oMyAvviso.sAnniPresenzaRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME_DENOMINAZIONE", SqlDbType.NVarChar)).Value = oMyAvviso.sCognome
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyAvviso.sNome
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_FISCALE", SqlDbType.NVarChar)).Value = oMyAvviso.sCodFiscale
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PARTITA_IVA", SqlDbType.NVarChar)).Value = oMyAvviso.sPIVA
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA_RES", SqlDbType.NVarChar)).Value = oMyAvviso.sIndirizzoRes
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO_RES", SqlDbType.NVarChar)).Value = oMyAvviso.sCivicoRes
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CAP_RES", SqlDbType.NVarChar)).Value = oMyAvviso.sCAPRes
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COMUNE_RES", SqlDbType.NVarChar)).Value = oMyAvviso.sComuneRes
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVINCIA_RES", SqlDbType.NVarChar)).Value = oMyAvviso.sProvRes
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FRAZIONE_RES", SqlDbType.NVarChar)).Value = oMyAvviso.sFrazRes
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMINATIVO_INVIO", SqlDbType.NVarChar)).Value = oMyAvviso.sNominativoCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA_RCP", SqlDbType.NVarChar)).Value = oMyAvviso.sIndirizzoCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO_RCP", SqlDbType.NVarChar)).Value = oMyAvviso.sCivicoCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CAP_RCP", SqlDbType.NVarChar)).Value = oMyAvviso.sCAPCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COMUNE_RCP", SqlDbType.NVarChar)).Value = oMyAvviso.sComuneCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVINCIA_RCP", SqlDbType.NVarChar)).Value = oMyAvviso.sProvCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FRAZIONE_RCP", SqlDbType.NVarChar)).Value = oMyAvviso.sFrazCO
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_TOTALE", SqlDbType.Float)).Value = oMyAvviso.impTotale
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_ARROTONDAMENTO", SqlDbType.Float)).Value = oMyAvviso.impArrotondamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_CARICO", SqlDbType.Float)).Value = oMyAvviso.impCarico
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = oMyAvviso.IdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyAvviso.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                If oMyAvviso.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyAvviso.tDataVariazione
    '                End If
    '                If oMyAvviso.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyAvviso.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyAvviso.sOperatore
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case Costanti.AZIONE_UPDATE
    '                cmdMyCommand.CommandText = "UPDATE TBLCARTELLE SET"
    '                cmdMyCommand.CommandText += " CODICE_CARTELLA=@CODICE_CARTELLA"
    '                cmdMyCommand.CommandText += ", DATA_EMISSIONE=@DATA_EMISSIONE"
    '                cmdMyCommand.CommandText += ", LOTTO_CARTELLAZIONE=@LOTTO_CARTELLAZIONE"
    '                cmdMyCommand.CommandText += ", IMPORTO_TOTALE=@IMPORTO_TOTALE"
    '                cmdMyCommand.CommandText += ", IMPORTO_ARROTONDAMENTO=@IMPORTO_ARROTONDAMENTO"
    '                cmdMyCommand.CommandText += ", IMPORTO_CARICO=@IMPORTO_CARICO"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDAVVISO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyAvviso.ID
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = oMyAvviso.sCodiceCartella
    '                If oMyAvviso.tDataEmissione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_EMISSIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_EMISSIONE", SqlDbType.DateTime)).Value = oMyAvviso.tDataEmissione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LOTTO_CARTELLAZIONE", SqlDbType.Int)).Value = oMyAvviso.nLottoCartellazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_TOTALE", SqlDbType.Float)).Value = oMyAvviso.impTotale
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_ARROTONDAMENTO", SqlDbType.Float)).Value = oMyAvviso.impArrotondamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_CARICO", SqlDbType.Float)).Value = oMyAvviso.impCarico
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oMyAvviso.ID
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "UPDATE TBLCARTELLE"
    '                cmdMyCommand.CommandText += " SET DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = Now
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                If nIdFlusso > 0 Then
    '                    cmdMyCommand.CommandText += " AND (IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                If Not IsNothing(oMyAvviso) Then
    '                    cmdMyCommand.CommandText += " AND (ID=@IDAVVISO)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyAvviso.ID
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetAvviso.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetCartella(ByVal oMyAvviso As ObjAvviso, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity, x As Integer
    '    Dim FncTessera As New GestTessera

    '    Try
    '        'aggiorno il codice cartella calcolato e la data di emissione
    '        myIdentity = SetAvviso(oMyAvviso, -1, Costanti.AZIONE_UPDATE, WFSessione)
    '        If myIdentity <= 0 Then
    '            Return 0
    '        End If

    '        'inserisco le rate
    '        If Not oMyAvviso.oRate Is Nothing Then
    '            For x = 0 To oMyAvviso.oRate.GetUpperBound(0)
    '                Dim FncRate As New GestRata
    '                If FncRate.SetRata(oMyAvviso.oRate(x), -1, Costanti.AZIONE_NEW, WFSessione) <= 0 Then
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        'inserisco il dettaglio voci
    '        If Not oMyAvviso.oDetVoci Is Nothing Then
    '            Dim FncDetVoci As New GestDetVoci
    '            For x = 0 To oMyAvviso.oDetVoci.GetUpperBound(0)
    '                If FncDetVoci.SetDetVoci(oMyAvviso.oDetVoci(x), -1, Costanti.AZIONE_NEW, WFSessione) <= 0 Then
    '                    Return 0
    '                End If
    '            Next
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetCartella.errore: ",Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function DeleteAvvisoCompleto(ByVal nIdFlusso As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity, x As Integer
    '    Dim FncTessera As New GestTessera
    '    Dim FncScaglioni As New GestScaglione
    '    Dim FncArticoli As New GestArticolo
    '    Dim FncDetVoci As New GestDetVoci
    '    Dim FncRate As New GestRata

    '    Try
    '        'elimino il legame tra tessera e avviso
    '        If FncTessera.SetTesseraAvviso(-1, -1, nIdFlusso, Costanti.AZIONE_DELETE, WFSessione) < 0 Then
    '            Return 0
    '        End If
    '        'elimino gli scaglioni
    '        If FncScaglioni.SetScaglione(Nothing, nIdFlusso, Costanti.AZIONE_DELETE, WFSessione) < 0 Then
    '            Return 0
    '        End If
    '        'elimino gli articoli
    '        If FncArticoli.SetArticolo(Nothing, nIdFlusso, Costanti.AZIONE_DELETE, WFSessione) < 0 Then
    '            Return 0
    '        End If
    '        'elimino il dettaglio voci
    '        If FncDetVoci.SetDetVoci(Nothing, nIdFlusso, Costanti.AZIONE_DELETE, WFSessione) < 0 Then
    '            Return 0
    '        End If
    '        'elimino le rate
    '        If FncRate.SetRata(Nothing, nIdFlusso, Costanti.AZIONE_DELETE, WFSessione) < 0 Then
    '            Return 0
    '        End If
    '        'elimino l'avviso
    '        If SetAvviso(Nothing, nIdFlusso, Costanti.AZIONE_DELETE, WFSessione) <= 0 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.DeleteAvvisoCompleto.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    Public Function DeleteAvvisoCompleto(DBType As String, ByVal myStringConnection As String, ByVal myStringConnectionGOV As String, IsFromVariabile As String, IdEnte As String, ByVal nIdFlusso As Integer) As Integer
        Dim FncDB As New Utility.DichManagerTARSU(DBType, myStringConnection, myStringConnectionGOV, IdEnte)

        Try
            'elimino il legame tra tessera e avviso
            If FncDB.SetTesseraAvviso(-1, -1, nIdFlusso, Utility.Costanti.AZIONE_DELETE) < 0 Then
                Return 0
            End If
            'elimino gli scaglioni
            If FncDB.SetScaglione(Nothing, nIdFlusso, Utility.Costanti.AZIONE_DELETE) < 0 Then
                Return 0
            End If
            'elimino gli articoli
            If FncDB.SetArticoloCompleto(Nothing, IsFromVariabile, nIdFlusso, Utility.Costanti.AZIONE_DELETE) < 0 Then
                Return 0
            End If
            'elimino il dettaglio voci
            If FncDB.SetDetVoci(Nothing, -1, nIdFlusso, Utility.Costanti.AZIONE_DELETE) < 0 Then
                Return 0
            End If
            'elimino le rate
            If FncDB.SetRata(Nothing, nIdFlusso, Utility.Costanti.AZIONE_DELETE) < 0 Then
                Return 0
            End If
            'elimino l'avviso
            If FncDB.SetAvviso(Nothing, nIdFlusso, Utility.Costanti.AZIONE_DELETE) < 0 Then
                Return 0
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.GestAvviso.DeleteAvvisoCompleto.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="nIdAvviso"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="IdFlussoRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sProgRuolo"></param>
    ''' <param name="sCognome"></param>
    ''' <param name="sNome"></param>
    ''' <param name="sCFPIVA"></param>
    ''' <param name="sCodCartella"></param>
    ''' <param name="sNomeDal"></param>
    ''' <param name="sNomeAl"></param>
    ''' <param name="bGetDettaglio"></param>
    ''' <param name="bIsSgravio"></param>
    ''' <param name="bIsSgravate"></param>
    ''' <param name="bIsDocToElab"></param>
    ''' <param name="DataEmissione"></param>
    ''' <param name="IdContribuente"></param>
    ''' <param name="cmdMyCommandOut"></param>
    ''' <returns></returns>
    ''' <revisionHistory><revision date="01/11/2013">TARES</revision></revisionHistory>
    ''' <revisionHistory><revision date="09/2018">Cartelle Insoluti</revision></revisionHistory>
    ''' <revisionHistory><revision date="10/2018">Generazione Massiva Atti</revision></revisionHistory>
    Public Function GetAvviso(ByVal myStringConnection As String, ByVal nIdAvviso As Integer, ByVal sIdEnte As String, ByVal IdFlussoRuolo As Integer, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal sProgRuolo As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCFPIVA As String, ByVal sCodCartella As String, ByVal sNomeDal As String, ByVal sNomeAl As String, ByVal bGetDettaglio As Boolean, ByVal bIsSgravio As Boolean, ByVal bIsSgravate As Boolean, ByVal bIsDocToElab As Boolean, DataEmissione As String, IdContribuente As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjAvviso()
        Dim oMyAvviso As ObjAvviso
        Dim oListAvvisi As New ArrayList
        Dim FncArticoli As New GestArticolo
        Dim FncDetVoci As New GestDetVoci
        Dim FncRate As New GestRata
        Dim FncTessere As New GestTessera
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim FncGestPag As New ClsGestPag
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1
        Dim DocToElab As Integer = 0
        Dim Sgravio As Integer = 0
        Dim Sgravate As Integer = 0

        Try
            If bIsDocToElab = True Then
                DocToElab = 1
            End If
            If bIsSgravio = True Then
                Sgravio = 1
            End If
            If bIsSgravate = True Then
                Sgravate = 1
            End If
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetAvviso", "IDENTE", "IDFLUSSO", "ANNO", "TIPO_RUOLO", "IDAVVISO", "COGNOME", "NOME", "CFPIVA", "CODCARTELLA", "ISDOCTOELAB", "SGRAVIO", "ISSGRAVIO", "NOMEDA", "NOMEA", "DATAEMISSIONE", "IDCONTRIBUENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDFLUSSO", IdFlussoRuolo) _
                            , ctx.GetParam("ANNO", sAnno) _
                            , ctx.GetParam("TIPO_RUOLO", sTipoRuolo) _
                            , ctx.GetParam("IDAVVISO", nIdAvviso) _
                            , ctx.GetParam("COGNOME", oReplace.ReplaceCharsForSearch(sCognome) & "%") _
                            , ctx.GetParam("NOME", oReplace.ReplaceCharsForSearch(sNome) & "%") _
                            , ctx.GetParam("CFPIVA", oReplace.ReplaceCharsForSearch(sCFPIVA) & "%") _
                            , ctx.GetParam("CODCARTELLA", sCodCartella) _
                            , ctx.GetParam("ISDOCTOELAB", DocToElab) _
                            , ctx.GetParam("SGRAVIO", Sgravio) _
                            , ctx.GetParam("ISSGRAVIO", Sgravate) _
                            , ctx.GetParam("NOMEDA", oReplace.ReplaceCharsForSearch(sNomeDal) & "%") _
                            , ctx.GetParam("NOMEA", oReplace.ReplaceCharsForSearch(sNomeAl) & "%") _
                            , ctx.GetParam("DATAEMISSIONE", oReplace.FormattaData(DataEmissione, "A")) _
                            , ctx.GetParam("IDCONTRIBUENTE", IdContribuente)
                        )
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetAvviso.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nAvanzamento += 1
                    sAvanzamento = "Lettura posizione " & nAvanzamento & " su " & myDataView.Count
                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)

                    oMyAvviso = New ObjAvviso
                    oMyAvviso.ID = StringOperation.FormatInt(myRow("id"))
                    oMyAvviso.IdEnte = StringOperation.FormatString(myRow("idente"))
                    oMyAvviso.IdContribuente = StringOperation.FormatInt(myRow("COD_CONTRIBUENTE"))
                    oMyAvviso.sCodFiscale = StringOperation.FormatString(myRow("cfpiva"))

                    oMyAvviso.tDataNascita = StringOperation.FormatDateTime(myRow("data_nascita"))
                    oMyAvviso.sComuneNascita = StringOperation.FormatString(myRow("comune_nascita"))
                    oMyAvviso.sPVNascita = StringOperation.FormatString(myRow("pv_nascita"))
                    oMyAvviso.sSesso = StringOperation.FormatString(myRow("sesso"))

                    oMyAvviso.sIndirizzoRes = StringOperation.FormatString(myRow("via_res"))
                    oMyAvviso.sCivicoRes = StringOperation.FormatString(myRow("civico_res"))
                    oMyAvviso.sCAPRes = StringOperation.FormatString(myRow("cap_res"))
                    oMyAvviso.sComuneRes = StringOperation.FormatString(myRow("comune_res"))
                    oMyAvviso.sProvRes = StringOperation.FormatString(myRow("provincia_res"))

                    oMyAvviso.sNominativoCO = StringOperation.FormatString(myRow("nominativoco"))
                    oMyAvviso.sIndirizzoCO = StringOperation.FormatString(myRow("indirizzoco"))
                    oMyAvviso.sCivicoCO = StringOperation.FormatString(myRow("civicoco"))
                    oMyAvviso.sCAPCO = StringOperation.FormatString(myRow("capco"))
                    oMyAvviso.sComuneCO = StringOperation.FormatString(myRow("comuneco"))
                    oMyAvviso.sProvCO = StringOperation.FormatString(myRow("pvco"))

                    oMyAvviso.sAnnoRiferimento = StringOperation.FormatString(myRow("ANNO"))
                    oMyAvviso.sCodiceCartella = StringOperation.FormatString(myRow("CODICE_CARTELLA"))
                    oMyAvviso.IdFlussoRuolo = StringOperation.FormatInt(myRow("IDFLUSSO_RUOLO"))
                    oMyAvviso.tDataEmissione = StringOperation.FormatDateTime(myRow("DATA_EMISSIONE"))
                    oMyAvviso.impPF = StringOperation.FormatDouble(myRow("IMPORTO_PF"))
                    oMyAvviso.impPV = StringOperation.FormatDouble(myRow("IMPORTO_PV"))
                    oMyAvviso.impPC = StringOperation.FormatDouble(myRow("IMPORTO_PC"))
                    oMyAvviso.impPM = StringOperation.FormatDouble(myRow("IMPORTO_PM"))
                    oMyAvviso.impTotale = StringOperation.FormatDouble(myRow("IMPORTO_TOTALE"))
                    oMyAvviso.impArrotondamento = StringOperation.FormatDouble(myRow("IMPORTO_ARROTONDAMENTO"))
                    oMyAvviso.impCarico = StringOperation.FormatDouble(myRow("IMPORTO_CARICO"))
                    oMyAvviso.sCognome = StringOperation.FormatString(myRow("COGNOME"))
                    oMyAvviso.sNome = StringOperation.FormatString(myRow("NOME"))
                    oMyAvviso.nLottoCartellazione = StringOperation.FormatInt(myRow("LOTTO_CARTELLAZIONE"))
                    oMyAvviso.bIsSgravio = StringOperation.FormatInt(myRow("ISGRAVIO"))
                    oMyAvviso.impPRESgravio = StringOperation.FormatDouble(myRow("IMPORTO_PRE_SGRAVIO"))
                    oMyAvviso.impPagato = StringOperation.FormatDouble(myRow("PAGATO"))
                    oMyAvviso.tDataNotifica = StringOperation.FormatDateTime(myRow("data_notifica"))
                    oMyAvviso.tDataAccertamento = StringOperation.FormatDateTime(myRow("data_accertamento"))
                    If bGetDettaglio = True Then
                        'prelevo gli articoli
                        oMyAvviso.oArticoli = FncArticoli.GetArticoli(myStringConnection, -1, oMyAvviso.ID, -1, True)
                        If oMyAvviso.oArticoli Is Nothing Then
                            Log.Debug("Mancano gli articoli::oMyAvviso.ID::" & oMyAvviso.ID & "::oMyAvviso.sCodiceCartella::" & oMyAvviso.sCodiceCartella)
                            Return Nothing
                        End If
                        'prelevo le tessere
                        oMyAvviso.oTessere = FncTessere.GetTessera(myStringConnection, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, "", oMyAvviso.ID, False, True)
                        'prelevo il dettaglio voci
                        oMyAvviso.oDetVoci = FncDetVoci.GetDetVoci(myStringConnection, oMyAvviso.ID, -1)
                        'prelevo le rate
                        oMyAvviso.oRate = FncRate.GetRate(myStringConnection, oMyAvviso.ID, -1)
                    End If
                    If nIdAvviso > 0 Then
                        oMySearchPag.sEnte = sIdEnte
                        oMySearchPag.bRicPag = False
                        oMySearchPag.IdContribuente = oMyAvviso.IdContribuente
                        oMySearchPag.sNAvviso = oMyAvviso.sCodiceCartella
                        oMySearchPag.tDataAccreditoDal = DateTime.MaxValue
                        oMySearchPag.tDataAccreditoAl = DateTime.MaxValue
                        oMyAvviso.oPagamenti = FncGestPag.GetListPagamenti(oMySearchPag, myStringConnection)
                        If Not oMyAvviso.oPagamenti Is Nothing Then
                            Dim myPag As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti
                            For Each myPag In oMyAvviso.oPagamenti
                                oMyAvviso.impPagato += myPag.dImportoPagamento
                            Next
                        End If
                    End If
                    oMyAvviso.impSaldo = oMyAvviso.impCarico - oMyAvviso.impPagato
                    oListAvvisi.Add(oMyAvviso)
                Next
            End Using
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetAvviso.errore: ", Err)
            oListAvvisi = Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return CType(oListAvvisi.ToArray(GetType(ObjAvviso)), ObjAvviso())
    End Function
    'Public Function GetAvviso(ByVal myStringConnection As String, ByVal nIdAvviso As Integer, ByVal sIdEnte As String, ByVal IdFlussoRuolo As Integer, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal sProgRuolo As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCFPIVA As String, ByVal sCodCartella As String, ByVal sNomeDal As String, ByVal sNomeAl As String, ByVal bGetDettaglio As Boolean, ByVal bIsSgravio As Boolean, ByVal bIsSgravate As Boolean, ByVal bIsDocToElab As Boolean, DataEmissione As String, IdContribuente As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjAvviso()
    '    Dim oMyAvviso As ObjAvviso
    '    Dim oListAvvisi As New ArrayList
    '    Dim FncArticoli As New GestArticolo
    '    Dim FncDetVoci As New GestDetVoci
    '    Dim FncRate As New GestRata
    '    Dim FncTessere As New GestTessera
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim oMySearchPag As New ObjSearchPagamenti
    '    Dim FncGestPag As New ClsGestPag
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0
    '    Dim myAdapter As New SqlClient.SqlDataAdapter

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetAvviso"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = IdFlussoRuolo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.VarChar)).Value = sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUOLO", SqlDbType.VarChar)).Value = sTipoRuolo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sCognome) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sNome) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sCFPIVA) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.VarChar)).Value = sCodCartella
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISDOCTOELAB", SqlDbType.Int)).Value = StringOperation.FormatInt(bIsDocToElab)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SGRAVIO", SqlDbType.Int)).Value = StringOperation.FormatInt(bIsSgravio)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISSGRAVIO", SqlDbType.Int)).Value = StringOperation.FormatInt(bIsSgravate)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEDA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sNomeDal) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sNomeAl) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataEmissione", SqlDbType.NVarChar)).Value = oReplace.FormattaData(DataEmissione, "A")
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = IdContribuente
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            nAvanzamento += 1
    '            sAvanzamento = "Lettura posizione " & nAvanzamento & " su " & dtMyDati.Rows.Count
    '            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)

    '            oMyAvviso = New ObjAvviso
    '            oMyAvviso.ID = StringOperation.FormatInt(dtMyRow("id"))
    '            oMyAvviso.IdEnte = StringOperation.FormatString(dtMyRow("idente"))
    '            oMyAvviso.IdContribuente = StringOperation.FormatInt(dtMyRow("COD_CONTRIBUENTE"))
    '            oMyAvviso.sCodFiscale = StringOperation.FormatString(dtMyRow("cfpiva"))

    '            If Not IsDBNull(dtMyRow("data_nascita")) Then
    '                oMyAvviso.tDataNascita = StringOperation.FormatDateTime(dtMyRow("data_nascita"))
    '            End If
    '            If Not IsDBNull(dtMyRow("comune_nascita")) Then
    '                oMyAvviso.sComuneNascita = StringOperation.FormatString(dtMyRow("comune_nascita"))
    '            End If
    '            If Not IsDBNull(dtMyRow("pv_nascita")) Then
    '                oMyAvviso.sPVNascita = StringOperation.FormatString(dtMyRow("pv_nascita"))
    '            End If
    '            If Not IsDBNull(dtMyRow("sesso")) Then
    '                oMyAvviso.sSesso = StringOperation.FormatString(dtMyRow("sesso"))
    '            End If

    '            oMyAvviso.sIndirizzoRes = StringOperation.FormatString(dtMyRow("via_res"))
    '            oMyAvviso.sCivicoRes = StringOperation.FormatString(dtMyRow("civico_res"))
    '            oMyAvviso.sCAPRes = StringOperation.FormatString(dtMyRow("cap_res"))
    '            oMyAvviso.sComuneRes = StringOperation.FormatString(dtMyRow("comune_res"))
    '            oMyAvviso.sProvRes = StringOperation.FormatString(dtMyRow("provincia_res"))

    '            oMyAvviso.sNominativoCO = StringOperation.FormatString(dtMyRow("nominativoco"))
    '            oMyAvviso.sIndirizzoCO = StringOperation.FormatString(dtMyRow("indirizzoco"))
    '            oMyAvviso.sCivicoCO = StringOperation.FormatString(dtMyRow("civicoco"))
    '            oMyAvviso.sCAPCO = StringOperation.FormatString(dtMyRow("capco"))
    '            oMyAvviso.sComuneCO = StringOperation.FormatString(dtMyRow("comuneco"))
    '            oMyAvviso.sProvCO = StringOperation.FormatString(dtMyRow("pvco"))

    '            oMyAvviso.sAnnoRiferimento = StringOperation.FormatString(dtMyRow("ANNO"))
    '            oMyAvviso.sCodiceCartella = StringOperation.FormatString(dtMyRow("CODICE_CARTELLA"))
    '            oMyAvviso.IdFlussoRuolo = StringOperation.FormatInt(dtMyRow("IDFLUSSO_RUOLO"))
    '            If Not IsDBNull(dtMyRow("DATA_EMISSIONE")) Then
    '                oMyAvviso.tDataEmissione = StringOperation.FormatDateTime(dtMyRow("DATA_EMISSIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PF")) Then
    '                oMyAvviso.impPF = StringOperation.FormatDouble(dtMyRow("IMPORTO_PF"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PV")) Then
    '                oMyAvviso.impPV = StringOperation.FormatDouble(dtMyRow("IMPORTO_PV"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PC")) Then
    '                oMyAvviso.impPC = StringOperation.FormatDouble(dtMyRow("IMPORTO_PC"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PM")) Then
    '                oMyAvviso.impPM = StringOperation.FormatDouble(dtMyRow("IMPORTO_PM"))
    '            End If
    '            oMyAvviso.impTotale = StringOperation.FormatDouble(dtMyRow("IMPORTO_TOTALE"))
    '            oMyAvviso.impArrotondamento = StringOperation.FormatDouble(dtMyRow("IMPORTO_ARROTONDAMENTO"))
    '            oMyAvviso.impCarico = StringOperation.FormatDouble(dtMyRow("IMPORTO_CARICO"))
    '            If Not IsDBNull(dtMyRow("COGNOME")) Then
    '                oMyAvviso.sCognome = StringOperation.FormatString(dtMyRow("COGNOME"))
    '            End If
    '            If Not IsDBNull(dtMyRow("NOME")) Then
    '                oMyAvviso.sNome = StringOperation.FormatString(dtMyRow("NOME"))
    '            End If
    '            If Not IsDBNull(dtMyRow("LOTTO_CARTELLAZIONE")) Then
    '                oMyAvviso.nLottoCartellazione = StringOperation.FormatInt(dtMyRow("LOTTO_CARTELLAZIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("ISGRAVIO")) Then
    '                oMyAvviso.bIsSgravio = StringOperation.FormatInt(dtMyRow("ISGRAVIO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PRE_SGRAVIO")) Then
    '                oMyAvviso.impPRESgravio = StringOperation.FormatDouble(dtMyRow("IMPORTO_PRE_SGRAVIO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("PAGATO")) Then
    '                oMyAvviso.impPagato = StringOperation.FormatDouble(dtMyRow("PAGATO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_notifica")) Then
    '                oMyAvviso.tDataNotifica = StringOperation.FormatDateTime(dtMyRow("data_notifica"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_accertamento")) Then
    '                oMyAvviso.tDataAccertamento = StringOperation.FormatDateTime(dtMyRow("data_accertamento"))
    '            End If
    '            If bGetDettaglio = True Then
    '                'prelevo gli articoli
    '                oMyAvviso.oArticoli = FncArticoli.GetArticoli(myStringConnection, -1, oMyAvviso.ID, -1, True, cmdMyCommand)
    '                If oMyAvviso.oArticoli Is Nothing Then
    '                    Log.Debug("Mancano gli articoli::oMyAvviso.ID::" & oMyAvviso.ID & "::oMyAvviso.sCodiceCartella::" & oMyAvviso.sCodiceCartella)
    '                    Return Nothing
    '                End If
    '                'prelevo gli scaglioni
    '                'oMyAvviso.oScaglioni = FncScaglioni.GetScaglioni(oMyAvviso.ID, MyDBEngine)
    '                'prelevo le tessere
    '                oMyAvviso.oTessere = FncTessere.GetTessera(myStringConnection, oMyAvviso.IdEnte, oMyAvviso.IdContribuente, -1, "", -1, "", oMyAvviso.ID, False, True)
    '                'prelevo il dettaglio voci
    '                oMyAvviso.oDetVoci = FncDetVoci.GetDetVoci(oMyAvviso.ID, -1, cmdMyCommand)
    '                'prelevo le rate
    '                oMyAvviso.oRate = FncRate.GetRate(oMyAvviso.ID, -1, cmdMyCommand)
    '            End If
    '            If nIdAvviso > 0 Then
    '                oMySearchPag.sEnte = sIdEnte
    '                oMySearchPag.bRicPag = False
    '                oMySearchPag.IdContribuente = oMyAvviso.IdContribuente
    '                oMySearchPag.sNAvviso = oMyAvviso.sCodiceCartella
    '                oMySearchPag.tDataAccreditoDal = DateTime.MaxValue
    '                oMySearchPag.tDataAccreditoAl = DateTime.MaxValue
    '                oMyAvviso.oPagamenti = FncGestPag.GetListPagamenti(oMySearchPag, myStringConnection)
    '                If Not oMyAvviso.oPagamenti Is Nothing Then
    '                    Dim myPag As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti
    '                    For Each myPag In oMyAvviso.oPagamenti
    '                        oMyAvviso.impPagato += myPag.dImportoPagamento
    '                    Next
    '                End If
    '            End If
    '            oMyAvviso.impSaldo = oMyAvviso.impCarico - oMyAvviso.impPagato
    '            oListAvvisi.Add(oMyAvviso)
    '        Next

    '        Return CType(oListAvvisi.ToArray(GetType(ObjAvviso)), ObjAvviso())
    '    Catch Err As Exception
    '        Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetAvviso.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '**** 201809 - Cartelle Insoluti ***
    Public Function GetStampaAvvisi(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCFPIVA As String, ByVal sCodCartella As String, ByVal bIsSgravate As Boolean, DataEmissione As String) As DataView
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim Sgravate As Integer = 0

        Try
            If bIsSgravate = True Then
                Sgravate = 1
            End If
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetAvviso", "IDENTE", "IDFLUSSO", "ANNO", "TIPO_RUOLO", "IDAVVISO", "COGNOME", "NOME", "CFPIVA", "CODCARTELLA", "ISDOCTOELAB", "SGRAVIO", "ISSGRAVIO", "NOMEDA", "NOMEA", "DATAEMISSIONE", "IDCONTRIBUENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDFLUSSO", -1) _
                            , ctx.GetParam("ANNO", sAnno) _
                            , ctx.GetParam("TIPO_RUOLO", sTipoRuolo) _
                            , ctx.GetParam("IDAVVISO", -1) _
                            , ctx.GetParam("COGNOME", oReplace.ReplaceCharsForSearch(sCognome) & "%") _
                            , ctx.GetParam("NOME", oReplace.ReplaceCharsForSearch(sNome) & "%") _
                            , ctx.GetParam("CFPIVA", oReplace.ReplaceCharsForSearch(sCFPIVA) & "%") _
                            , ctx.GetParam("CODCARTELLA", sCodCartella) _
                            , ctx.GetParam("ISDOCTOELAB", 0) _
                            , ctx.GetParam("SGRAVIO", 0) _
                            , ctx.GetParam("ISSGRAVIO", Sgravate) _
                            , ctx.GetParam("NOMEDA", "%") _
                            , ctx.GetParam("NOMEA", "%") _
                            , ctx.GetParam("DATAEMISSIONE", oReplace.FormattaData(DataEmissione, "A")) _
                            , ctx.GetParam("IDCONTRIBUENTE", -1)
                        )
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetStampaAvvisi.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetStampaAvvisi.errore: ", Err)
            Return Nothing
        End Try
        Return myDataView
    End Function
    'Public Function GetStampaAvvisi(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCFPIVA As String, ByVal sCodCartella As String, ByVal bIsSgravate As Boolean, DataEmissione As String) As DataView
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetAvviso"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = -1
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.VarChar)).Value = sAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUOLO", SqlDbType.VarChar)).Value = sTipoRuolo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = -1
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sCognome) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sNome) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.VarChar)).Value = oReplace.ReplaceCharsForSearch(sCFPIVA) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.VarChar)).Value = sCodCartella
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISDOCTOELAB", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SGRAVIO", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISSGRAVIO", SqlDbType.Int)).Value = StringOperation.FormatInt(bIsSgravate)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEDA", SqlDbType.VarChar)).Value = "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEA", SqlDbType.VarChar)).Value = "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataEmissione", SqlDbType.NVarChar)).Value = oReplace.FormattaData(DataEmissione, "A")
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        Return dtMyDati.DefaultView
    '    Catch Err As Exception
    '        Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetStampaAvvisi.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    'Public Function SetAvvisoCompleto(ByVal oMyAvviso As ObjAvviso, ByVal bIsFromVariabile As Boolean, ByRef DBEngineOut As DBEngine) As Integer
    '    Dim x As Integer
    '    Dim FncTessera As New GestTessera
    '    Dim MyDBEngine As DBEngine = Nothing

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '            MyDBEngine.BeginTransaction()
    '        End If

    '        oMyAvviso.ID = SetAvviso(oMyAvviso, -1, Costanti.AZIONE_NEW, MyDBEngine)
    '        If oMyAvviso.ID <= 0 Then
    '            If (DBEngineOut Is Nothing) Then
    '                MyDBEngine.RollbackTransaction()
    '            End If
    '            Return 0
    '        End If
    '        'inserisco gli articoli
    '        If Not oMyAvviso.oArticoli Is Nothing Then
    '            Dim FncArticoli As New GestArticolo
    '            For x = 0 To oMyAvviso.oArticoli.GetUpperBound(0)
    '                oMyAvviso.oArticoli(x).IdAvviso = oMyAvviso.ID
    '                oMyAvviso.oArticoli(x).Id = -1
    '                If FncArticoli.SetArticoloCompleto(oMyAvviso.oArticoli(x), bIsFromVariabile, MyDBEngine) = 0 Then
    '                    If (DBEngineOut Is Nothing) Then
    '                        MyDBEngine.RollbackTransaction()
    '                    End If
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        'controllo se inserire i dati di tessera
    '        If Not IsNothing(oMyAvviso.oTessere) Then
    '            For x = 0 To oMyAvviso.oTessere.GetUpperBound(0)
    '                'inserisco il legame tra tessera e avviso
    '                If FncTessera.SetTesseraAvviso(oMyAvviso.ID, oMyAvviso.oTessere(x).Id, -1, Costanti.AZIONE_NEW, MyDBEngine) <= 0 Then
    '                    If (DBEngineOut Is Nothing) Then
    '                        MyDBEngine.RollbackTransaction()
    '                    End If
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CommitTransaction()
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetAvvisoCompleto.errore: ", Err)
    '        Return 0
    '    Finally
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CloseConnection()
    '        End If
    '    End Try
    'End Function

    'Public Function SetAvvisoCompleto(ByVal oMyAvviso As ObjAvviso, ByVal bIsFromVariabile As Boolean, ByVal bIsFromSgravio As Boolean, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim FncTessera As New GestTessera
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myTrans As SqlClient.SqlTransaction
    '    Dim nIdRif As Integer = 0

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If

    '        oMyAvviso.ID = SetAvviso(oMyAvviso, -1, Utility.Costanti.AZIONE_NEW, cmdMyCommand)
    '        If oMyAvviso.ID <= 0 Then
    '            If (cmdMyCommandOut Is Nothing) Then
    '                myTrans.Rollback()
    '            End If
    '            Return 0
    '        End If
    '        'inserisco gli articoli
    '        '*** 20141211 - legami PF-PV ***
    '        If Not oMyAvviso.oArticoli Is Nothing Then
    '            Dim FncArticoli As New GestArticolo
    '            Dim nIdRuolo As Integer
    '            For Each myArt As ObjArticolo In oMyAvviso.oArticoli
    '                myArt.IdAvviso = oMyAvviso.ID
    '                'memorizzo l'id che avrò tra i legami PF-PV, se arrivo da sgravio=id se arrivo da calcolo massivo=idoggetto
    '                If bisfromsgravio Then
    '                    nIdRif = myArt.Id
    '                Else
    '                    nIdRif = myArt.IdOggetto
    '                End If
    '                myArt.Id = -1
    '                nIdRuolo = FncArticoli.SetArticoloCompleto(myArt, bIsFromVariabile, cmdMyCommand)
    '                If nIdRuolo = 0 Then
    '                    If (cmdMyCommandOut Is Nothing) Then
    '                        myTrans.Rollback()
    '                    End If
    '                    Return 0
    '                End If
    '                'per l'id PF appena inserito vado a registrare nei legami il corrispettivo idruolo generato sostituendolo all'idoggetto
    '                If myArt.TipoPartita = ObjArticolo.PARTEFISSA Then
    '                    For Each myPF As ObjArticolo In oMyAvviso.oArticoli
    '                        If myPF.TipoPartita = ObjArticolo.PARTEVARIABILE And myPF.sVia <> ObjArticolo.PARTEPRECEMESSO_DESCR Then 'i legami sono presenti solo sugli oggetti di tipo partevariabile che non sono GIÀ EMESSO
    '                            For Each myLeg As ObjLegamePFPV In myPF.ListPFvsPV
    '                                If myLeg.IdPF = nIdRif Then
    '                                    myLeg.IdPF = nIdRuolo
    '                                    Exit For
    '                                End If
    '                            Next
    '                        End If
    '                    Next
    '                ElseIf myArt.TipoPartita = ObjArticolo.PARTEVARIABILE And myArt.sVia <> ObjArticolo.PARTEPRECEMESSO_DESCR Then
    '                    For Each myLeg As ObjLegamePFPV In myArt.ListPFvsPV
    '                        myLeg.IdPV = nIdRuolo
    '                    Next
    '                End If
    '            Next
    '            'dopo aver inserito tutti gli articoli inserisco i legami PF-PV
    '            For Each myArt As ObjArticolo In oMyAvviso.oArticoli
    '                If myArt.TipoPartita = ObjArticolo.PARTEVARIABILE And myArt.sVia <> ObjArticolo.PARTEPRECEMESSO_DESCR Then
    '                    For Each myLeg As ObjLegamePFPV In myArt.ListPFvsPV
    '                        If FncArticoli.SetLegamePFPV(myLeg, cmdMyCommand) <= 0 Then
    '                            If (cmdMyCommandOut Is Nothing) Then
    '                                myTrans.Rollback()
    '                            End If
    '                            Return 0
    '                        End If
    '                    Next
    '                End If
    '            Next
    '            '*** ***
    '        End If
    '        'controllo se inserire i dati di tessera
    '        If Not IsNothing(oMyAvviso.oTessere) Then
    '            For Each myTes As ObjTessera In oMyAvviso.oTessere
    '                'inserisco il legame tra tessera e avviso
    '                If FncTessera.SetTesseraAvviso(oMyAvviso.ID, myTes.Id, -1, Utility.Costanti.AZIONE_NEW, cmdMyCommand) <= 0 Then
    '                    If (cmdMyCommandOut Is Nothing) Then
    '                        myTrans.Rollback()
    '                    End If
    '                    Return 0
    '                End If
    '            Next
    '        End If
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetAvvisoCompleto.errore: ", Err)
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    'Public Function SetCartella(ByRef oMyAvviso As ObjAvviso, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim myIdentity, x As Integer
    '    Dim FncTessera As New GestTessera
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myTrans As SqlClient.SqlTransaction

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If
    '        'aggiorno il codice cartella calcolato e la data di emissione
    '        myIdentity = SetAvviso(oMyAvviso, -1, Utility.Costanti.AZIONE_UPDATE, cmdMyCommand)
    '        If myIdentity <= 0 Then
    '            Return 0
    '        End If
    '        oMyAvviso.ID = myIdentity
    '        'inserisco il dettaglio voci
    '        If Not oMyAvviso.oDetVoci Is Nothing Then
    '            Dim FncDetVoci As New GestDetVoci
    '            For x = 0 To oMyAvviso.oDetVoci.GetUpperBound(0)
    '                oMyAvviso.oDetVoci(x).IdAvviso = myIdentity
    '                If FncDetVoci.SetDetVoci(oMyAvviso.oDetVoci(x), -1, Utility.Costanti.AZIONE_NEW, cmdMyCommand) <= 0 Then
    '                    If (cmdMyCommandOut Is Nothing) Then
    '                        myTrans.Rollback()
    '                    End If
    '                    Return 0
    '                End If
    '            Next
    '        End If

    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetCartella.errore: ", Err)
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Rollback()
    '        End If
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    'Public Function SetAvviso(ByVal oMyAvviso As ObjAvviso, ByVal nIdFlusso As Integer, ByVal nDBOperation As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim MyProcedure As String = "prc_TBLCARTELLE_IU"
    '    Dim myTrans As SqlClient.SqlTransaction

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        If oMyAvviso Is Nothing Then
    '            oMyAvviso = New ObjAvviso
    '            oMyAvviso.IdFlussoRuolo = nIdFlusso
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@ID", oMyAvviso.ID)
    '        Select Case nDBOperation
    '            Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
    '                cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyAvviso.IdEnte)
    '                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", oMyAvviso.IdFlussoRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@CODICE_CARTELLA", oMyAvviso.sCodiceCartella)
    '                cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyAvviso.sAnnoRiferimento)
    '                If oMyAvviso.tDataEmissione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_EMISSIONE", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_EMISSIONE", oMyAvviso.tDataEmissione)
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@COD_CONTRIBUENTE", oMyAvviso.IdContribuente)
    '                cmdMyCommand.Parameters.AddWithValue("@LOTTO_CARTELLAZIONE", oMyAvviso.nLottoCartellazione)
    '                cmdMyCommand.Parameters.AddWithValue("@ANNI_PRESENZA_RUOLO", oMyAvviso.sAnniPresenzaRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@COGNOME_DENOMINAZIONE", oMyAvviso.sCognome)
    '                cmdMyCommand.Parameters.AddWithValue("@NOME", oMyAvviso.sNome)
    '                cmdMyCommand.Parameters.AddWithValue("@COD_FISCALE", oMyAvviso.sCodFiscale)
    '                cmdMyCommand.Parameters.AddWithValue("@PARTITA_IVA", oMyAvviso.sPIVA)
    '                cmdMyCommand.Parameters.AddWithValue("@VIA_RES", oMyAvviso.sIndirizzoRes)
    '                cmdMyCommand.Parameters.AddWithValue("@CIVICO_RES", oMyAvviso.sCivicoRes)
    '                cmdMyCommand.Parameters.AddWithValue("@CAP_RES", oMyAvviso.sCAPRes)
    '                cmdMyCommand.Parameters.AddWithValue("@COMUNE_RES", oMyAvviso.sComuneRes)
    '                cmdMyCommand.Parameters.AddWithValue("@PROVINCIA_RES", oMyAvviso.sProvRes)
    '                cmdMyCommand.Parameters.AddWithValue("@FRAZIONE_RES", oMyAvviso.sFrazRes)
    '                cmdMyCommand.Parameters.AddWithValue("@NOMINATIVO_INVIO", oMyAvviso.sNominativoCO)
    '                cmdMyCommand.Parameters.AddWithValue("@VIA_RCP", oMyAvviso.sIndirizzoCO)
    '                cmdMyCommand.Parameters.AddWithValue("@CIVICO_RCP", oMyAvviso.sCivicoCO)
    '                cmdMyCommand.Parameters.AddWithValue("@CAP_RCP", oMyAvviso.sCAPCO)
    '                cmdMyCommand.Parameters.AddWithValue("@COMUNE_RCP", oMyAvviso.sComuneCO)
    '                cmdMyCommand.Parameters.AddWithValue("@PROVINCIA_RCP", oMyAvviso.sProvCO)
    '                cmdMyCommand.Parameters.AddWithValue("@FRAZIONE_RCP", oMyAvviso.sFrazCO)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_TOTALE", Math.Round(oMyAvviso.impTotale, 2))
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_ARROTONDAMENTO", oMyAvviso.impArrotondamento)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_CARICO", oMyAvviso.impCarico)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_CREDITODEBITO_PREC", oMyAvviso.impCreditoDebitoPrec)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_DOVUTO", Math.Round(oMyAvviso.impDovuto, 2))
    '                cmdMyCommand.Parameters.AddWithValue("@NOTE", oMyAvviso.sNote)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", Now)
    '                If oMyAvviso.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", oMyAvviso.tDataVariazione)
    '                End If
    '                If oMyAvviso.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_CESSAZIONE", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_CESSAZIONE", oMyAvviso.tDataCessazione)
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", oMyAvviso.sOperatore)
    '                MyProcedure = "prc_TBLCARTELLE_IU"
    '            Case Utility.Costanti.AZIONE_DELETE
    '                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", oMyAvviso.IdFlussoRuolo)
    '                MyProcedure = "prc_TBLCARTELLE_D"
    '        End Select
    '        'eseguo la query
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = MyProcedure
    '        Log.Debug("SetAvviso::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        oMyAvviso.ID = cmdMyCommand.Parameters("@ID").Value

    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return oMyAvviso.ID
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetAvviso.errore: ", Err)
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Rollback()
    '        End If
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    Public Function GetAvvisoSgravato(ByVal sCodCartella As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim nRet As Integer = 0

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
            cmdMyCommand.Parameters.AddWithValue("@CODCARTELLA", sCodCartella)
            cmdMyCommand.CommandText = "prc_GetAvvisoSgravato"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                nRet = StringOperation.FormatInt(dtMyRow("issgravato"))
            Next

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.GetAvvisoSgravato.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            nRet = 0
        Finally
            dtMyDati.Dispose()
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
        Return nRet
    End Function

    Public Function SetLockSgravio(ByVal nDBOperation As Integer, ByVal sCodiceCartella As String, ByVal nIdContribuente As Integer, ByVal nIdFlussoRuolo As Integer, ByVal sOperatore As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim MyProcedure As String = "prc_TBLPROCEDURASGRAVIO_IU"
        Dim MyRet As Integer = 0

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
                cmdMyCommand.CommandTimeout = 0
            End If
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ID", MyRet)
            Select Case nDBOperation
                Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
                    cmdMyCommand.Parameters.AddWithValue("@COD_CONTRIBUENTE", nIdContribuente)
                    cmdMyCommand.Parameters.AddWithValue("@CODICE_CARTELLA", sCodiceCartella)
                    cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdFlussoRuolo)
                    cmdMyCommand.Parameters.AddWithValue("@OPERATORE", sOperatore)
                    MyProcedure = "prc_TBLPROCEDURASGRAVIO_IU"
                Case Utility.Costanti.AZIONE_DELETE
                    cmdMyCommand.Parameters.AddWithValue("@CODICE_CARTELLA", sCodiceCartella)
                    MyProcedure = "prc_TBLPROCEDURASGRAVIO_D"
            End Select
            'eseguo la query
            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = MyProcedure
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            MyRet = cmdMyCommand.Parameters("@ID").Value

            Return MyRet
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetLockSgravio.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function

    Public Function CalcoloAvvisoSgravio(DBType As String, myConnectionString As String, ByRef oMyAvviso As ObjAvviso, ByVal sUsername As String, ByVal bIsFromVariabile As Boolean, ByVal bIsFromSgravio As Boolean, ByRef sMyErr As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Boolean
        Dim FncDB As New Utility.DichManagerTARSU(DBType, myConnectionString, "", oMyAvviso.IdEnte)

        Try
            'annullo il vecchio avviso
            oMyAvviso.tDataVariazione = DateTime.Now
            If FncDB.SetAvviso(oMyAvviso, -1, Utility.Costanti.AZIONE_DELETE) <= 0 Then
                sMyErr = "GestAlert('a', 'danger', '', '', 'Procedura di Sgravio terminata a causa di un errore!');"
                Return False
            End If
            oMyAvviso.tDataVariazione = Date.MinValue
            'se ha cartellato aggiorno il record per il ruolo
            If Not oMyAvviso Is Nothing Then
                'registro l'avviso nella tabella
                oMyAvviso.ID = -1
                oMyAvviso.sOperatore = sUsername
                oMyAvviso.tDataInserimento = Now
                oMyAvviso.tDataVariazione = DateTime.MaxValue
                oMyAvviso.tDataCessazione = DateTime.MaxValue
                If FncDB.SetAvvisoCompleto(oMyAvviso, bIsFromVariabile, bIsFromSgravio) = 0 Then
                    sMyErr = "Errore in inserimento Cartellazione"
                    'devo eliminare le operazioni fatte finora
                    If UndoSgravio(oMyAvviso, cmdMyCommand) = 0 Then
                        sMyErr += vbCrLf & "Errore in annullo sgravio"
                    End If
                    Return False
                End If
                If FncDB.SetCartella(oMyAvviso) = 0 Then
                    sMyErr = "Errore in inserimento Cartellazione"
                    'devo eliminare le operazioni fatte finora
                    If UndoSgravio(oMyAvviso, cmdMyCommand) = 0 Then
                        sMyErr += vbCrLf & "Errore in annullo sgravio"
                    End If
                    Return False
                End If
            Else
                sMyErr = "Errore in calcolo Cartellazione"
                Return False
            End If
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.CalcoloAvvisoSgravio.errore: ", Err)
            Log.Debug("Si è verificato un errore in GestAvviso::CalcoloAvvisoSgravio::" & Err.Message & "::param::" & oMyAvviso.sCodiceCartella)
            Return False
        Finally
            'If (cmdMyCommandOut Is Nothing) Then
            '    cmdMyCommand.Dispose()
            '    cmdMyCommand.Connection.Close()
            'End If
        End Try
    End Function

    Public Function UndoSgravio(ByVal oMyAvviso As ObjAvviso, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim MyRet As Integer = 0

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
                cmdMyCommand.CommandTimeout = 0
            End If
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ID", MyRet)
            cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", oMyAvviso.ID)
            cmdMyCommand.Parameters.AddWithValue("@CODICE_CARTELLA", oMyAvviso.sCodiceCartella)
            'eseguo la query
            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_UndoSgravio"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            MyRet = cmdMyCommand.Parameters("@ID").Value
            HttpContext.Current.Session("MyAvvisoRicalcolato") = Nothing : HttpContext.Current.Session("oListArticoliSgravi") = Nothing

            Return MyRet
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.UndoSgravio.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function

    Public Function GetLockSgravio(ByVal sCodCartella As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        '{-1: non presente procedura sgravio; 0: presente procedura sgravio; 1: presente procedura sgravio altro utente; 2: errore}
        Dim nLock As Integer = -1
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

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
            cmdMyCommand.Parameters.AddWithValue("@CODCARTELLA", sCodCartella)
            cmdMyCommand.Parameters.AddWithValue("@OPERATORE", ConstSession.UserName)
            cmdMyCommand.CommandText = "prc_GetLockSgravio"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                nLock = StringOperation.FormatInt(dtMyRow("nret"))
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.GetLockSgravio.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            nLock = 2
        Finally
            dtMyDati.Dispose()
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
        Return nLock
    End Function
    '*** ***
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' Inserisce la data di notifica sia in ordinario che sull'eventuale ingiunzione e calcola la data di scadenza
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="myItem"></param>
    ''' <returns></returns>
    Public Function SetNotifica(ByVal myStringConnection As String, ByVal myItem As ObjAvviso) As Boolean
        Dim sSQL As String
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetNotifica", "ID", "DATANOTIFICA")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", myItem.ID) _
                            , ctx.GetParam("DATANOTIFICA", oReplace.FormattaData(myItem.tDataNotifica, "A"))
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetNotifica.erroreQuery: ", ex)
                    Return False
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    If StringOperation.FormatInt(myRow("id")) <= 0 Then
                        Return False
                    End If
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestAvviso.SetNotifica.errore: ", Err)
            Return False
        Finally
            myDataView.Dispose()
        End Try
        Return True
    End Function
End Class
''' <summary>
''' Classe per la gestione della testata
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestTestata
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestTestata))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    'Public Function GetTestata(ByVal WFSessione As OPENUtility.CreateSessione, ByVal IdDichiarazione As Integer, Optional ByVal IdContribuente As Integer = -1, Optional ByVal sAnno As String = "") As ObjTestata()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oTestataDichiarazione As ObjTestata
    '    Dim oTestata() As ObjTestata
    '    Dim intTestata As Integer = -1

    '    Try
    '        'prelevo i dati della testata
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL) "
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        If IdDichiarazione > -1 Then
    '            cmdMyCommand.CommandText += " AND (ID=@IDTESTATA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = IdDichiarazione
    '        End If
    '        If IdContribuente > -1 Then
    '            cmdMyCommand.CommandText += " AND (IDCONTRIBUENTE=@IDCONTRIBUENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = IdContribuente
    '        End If
    '        If sAnno <> "" Then
    '            cmdMyCommand.CommandText += " AND (year(DATA_DICHIARAZIONE)=@ANNO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '        End If
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            oTestataDichiarazione = New ObjTestata

    '            oTestataDichiarazione.Id = StringOperation.Formatint(DrDati("id"))
    '            oTestataDichiarazione.IdContribuente = StringOperation.Formatint(DrDati("idcontribuente"))
    '            oTestataDichiarazione.IdTestata = StringOperation.Formatint(DrDati("idtestata"))
    '            oTestataDichiarazione.sEnte = StringOperation.FormatString(DrDati("idente"))
    '            oTestataDichiarazione.tDataDichiarazione = StringOperation.Formatdatetime(DrDati("data_dichiarazione"))
    '            oTestataDichiarazione.sNDichiarazione = StringOperation.FormatString(DrDati("numero_dichiarazione"))
    '            If Not IsDBNull(DrDati("data_protocollo")) Then
    '                oTestataDichiarazione.tDataProtocollo = StringOperation.Formatdatetime(DrDati("data_protocollo"))
    '            End If
    '            If Not IsDBNull(DrDati("numero_protocollo")) Then
    '                oTestataDichiarazione.sNProtocollo = StringOperation.FormatString(DrDati("numero_protocollo"))
    '            End If
    '            oTestataDichiarazione.sIdProvenienza = StringOperation.FormatString(DrDati("idprovenienza"))
    '            If Not IsDBNull(DrDati("note_dichiarazione")) Then
    '                oTestataDichiarazione.sNoteDichiarazione = StringOperation.FormatString(DrDati("note_dichiarazione"))
    '            End If
    '            oTestataDichiarazione.tDataInserimento = StringOperation.Formatdatetime(DrDati("data_inserimento"))
    '            If Not IsDBNull(DrDati("data_variazione")) Then
    '                oTestataDichiarazione.tDataVariazione = StringOperation.Formatdatetime(DrDati("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrDati("data_cessazione")) Then
    '                oTestataDichiarazione.tDataCessazione = StringOperation.Formatdatetime(DrDati("data_cessazione"))
    '            End If
    '            oTestataDichiarazione.sOperatore = ConstSession.UserName
    '            intTestata += 1
    '            ReDim Preserve oTestata(intTestata)
    '            oTestata(intTestata) = oTestataDichiarazione
    '        Loop

    '        Return oTestata
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.GetTestata.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function
    Public Function GetTestata(ByVal myStringConnection As String, ByVal IdDichiarazione As Integer, ByVal IdContribuente As Integer, IdDettaglioTestata As Integer, ByVal sAnno As String) As ObjTestata()
        Dim oTestataDichiarazione As ObjTestata
        Dim oTestata() As ObjTestata = Nothing
        Dim intTestata As Integer = -1
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_TBLTESTATA_S"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = IdDichiarazione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = IdDettaglioTestata
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = IdContribuente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                oTestataDichiarazione = New ObjTestata

                oTestataDichiarazione.Id = StringOperation.FormatInt(dtMyRow("id"))
                oTestataDichiarazione.IdContribuente = StringOperation.FormatInt(dtMyRow("idcontribuente"))
                oTestataDichiarazione.IdTestata = StringOperation.FormatInt(dtMyRow("idtestata"))
                oTestataDichiarazione.sEnte = StringOperation.FormatString(dtMyRow("idente"))
                oTestataDichiarazione.tDataDichiarazione = StringOperation.FormatDateTime(dtMyRow("data_dichiarazione"))
                oTestataDichiarazione.sNDichiarazione = StringOperation.FormatString(dtMyRow("numero_dichiarazione"))
                If Not IsDBNull(dtMyRow("data_protocollo")) Then
                    oTestataDichiarazione.tDataProtocollo = StringOperation.FormatDateTime(dtMyRow("data_protocollo"))
                End If
                If Not IsDBNull(dtMyRow("numero_protocollo")) Then
                    oTestataDichiarazione.sNProtocollo = StringOperation.FormatString(dtMyRow("numero_protocollo"))
                End If
                oTestataDichiarazione.sIdProvenienza = StringOperation.FormatString(dtMyRow("idprovenienza"))
                If Not IsDBNull(dtMyRow("note_dichiarazione")) Then
                    oTestataDichiarazione.sNoteDichiarazione = StringOperation.FormatString(dtMyRow("note_dichiarazione"))
                End If
                oTestataDichiarazione.tDataInserimento = StringOperation.FormatDateTime(dtMyRow("data_inserimento"))
                If Not IsDBNull(dtMyRow("data_variazione")) Then
                    oTestataDichiarazione.tDataVariazione = StringOperation.FormatDateTime(dtMyRow("data_variazione"))
                End If
                If Not IsDBNull(dtMyRow("data_cessazione")) Then
                    oTestataDichiarazione.tDataCessazione = StringOperation.FormatDateTime(dtMyRow("data_cessazione"))
                End If
                oTestataDichiarazione.sOperatore = ConstSession.UserName
                intTestata += 1
                ReDim Preserve oTestata(intTestata)
                oTestata(intTestata) = oTestataDichiarazione
            Next

            Return oTestata
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestTestata.GetTestata.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    'Public Function SetTestata(ByVal oMyNewTestata As ObjTestata, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        'costruisco la query
    '        cmdMyCommand.CommandText = "INSERT INTO TBLTESTATA (IDTESTATA,IDENTE,IDCONTRIBUENTE"
    '        cmdMyCommand.CommandText += " ,DATA_DICHIARAZIONE,NUMERO_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,DATA_PROTOCOLLO,NUMERO_PROTOCOLLO"
    '        cmdMyCommand.CommandText += " ,IDPROVENIENZA,NOTE_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,DATA_INSERIMENTO,DATA_VARIAZIONE,DATA_CESSAZIONE"
    '        cmdMyCommand.CommandText += " ,OPERATORE)"
    '        cmdMyCommand.CommandText += " VALUES(@IDTESTATA,@IDENTE,@IDCONTRIBUENTE"
    '        cmdMyCommand.CommandText += " ,@DATA_DICHIARAZIONE,@NUMERO_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,@DATA_PROTOCOLLO,@NUMERO_PROTOCOLLO"
    '        cmdMyCommand.CommandText += " ,@IDPROVENIENZA,@NOTE_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE"
    '        cmdMyCommand.CommandText += " ,@OPERATORE)"
    '        cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = oMyNewTestata.IdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyNewTestata.sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyNewTestata.IdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_DICHIARAZIONE", SqlDbType.DateTime)).Value = oMyNewTestata.tDataDichiarazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_DICHIARAZIONE", SqlDbType.NVarChar)).Value = oMyNewTestata.sNDichiarazione
    '        If oMyNewTestata.tDataProtocollo = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PROTOCOLLO", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PROTOCOLLO", SqlDbType.DateTime)).Value = oMyNewTestata.tDataProtocollo
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_PROTOCOLLO", SqlDbType.NVarChar)).Value = oMyNewTestata.sNProtocollo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVENIENZA", SqlDbType.NVarChar)).Value = oMyNewTestata.sIdProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE_DICHIARAZIONE", SqlDbType.NVarChar)).Value = oMyNewTestata.sNoteDichiarazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyNewTestata.tDataInserimento
    '        If oMyNewTestata.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyNewTestata.tDataVariazione
    '        End If
    '        If oMyNewTestata.tDataCessazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyNewTestata.tDataCessazione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyNewTestata.sOperatore
    '        'eseguo la query
    '        Dim DrReturn As SqlClient.SqlDataReader
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            myIdentity = DrReturn(0)
    '        Loop
    '        DrReturn.Close()
    '        'controllo se devo aggiornare l'IDTESTATA
    '        If oMyNewTestata.Id = -1 Then
    '            cmdMyCommand.CommandText = "UPDATE TBLTESTATA SET IDTESTATA=ID"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = myIdentity
    '            If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 0 Then
    '                Return 0
    '            End If
    '            oMyNewTestata.IdTestata = myIdentity
    '            oMyNewTestata.Id = myIdentity
    '        End If

    '        Return myIdentity
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.SetTestata.errore: ", Err)

    '        Return 0
    '    End Try
    'End Function

    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    'Public Function SetTestata(ByVal myConnectionString As String, ByVal oMyNewTestata As ObjTestata) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        'costruisco la query
    '        cmdMyCommand.CommandText = "prc_TBLTESTATA_IU"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyNewTestata.Id
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = oMyNewTestata.IdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyNewTestata.sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyNewTestata.IdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_DICHIARAZIONE", SqlDbType.DateTime)).Value = oMyNewTestata.tDataDichiarazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_DICHIARAZIONE", SqlDbType.NVarChar)).Value = oMyNewTestata.sNDichiarazione
    '        If oMyNewTestata.tDataProtocollo = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PROTOCOLLO", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PROTOCOLLO", SqlDbType.DateTime)).Value = oMyNewTestata.tDataProtocollo
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_PROTOCOLLO", SqlDbType.NVarChar)).Value = oMyNewTestata.sNProtocollo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVENIENZA", SqlDbType.NVarChar)).Value = oMyNewTestata.sIdProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE_DICHIARAZIONE", SqlDbType.NVarChar)).Value = oMyNewTestata.sNoteDichiarazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyNewTestata.tDataInserimento
    '        If oMyNewTestata.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyNewTestata.tDataVariazione
    '        End If
    '        If oMyNewTestata.tDataCessazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyNewTestata.tDataCessazione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyNewTestata.sOperatore
    '        'eseguo la query
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        'eseguo la query
    '        cmdMyCommand.ExecuteNonQuery()
    '        oMyNewTestata.Id = cmdMyCommand.Parameters("@ID").Value
    '        oMyNewTestata.IdTestata = oMyNewTestata.Id

    '        Return oMyNewTestata.Id
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.SetTestata.errore: ", Err)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '*** ***
    'Public Function UpdateTestata(ByVal WFSessione As OPENUtility.CreateSessione, ByVal oMyTestata As ObjTestata) As Integer
    '    Try
    '        'costruisco la query
    '        cmdMyCommand.CommandText = "UPDATE TBLTESTATA SET"
    '        cmdMyCommand.CommandText += " IDTESTATA=@IDTESTATA"
    '        cmdMyCommand.CommandText += " ,IDENTE=@IDENTE"
    '        cmdMyCommand.CommandText += " ,IDCONTRIBUENTE=@IDCONTRIBUENTE"
    '        cmdMyCommand.CommandText += " ,DATA_DICHIARAZIONE=@DATA_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,NUMERO_DICHIARAZIONE=@NUMERO_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,DATA_PROTOCOLLO=@DATA_PROTOCOLLO"
    '        cmdMyCommand.CommandText += " ,NUMERO_PROTOCOLLO=@NUMERO_PROTOCOLLO"
    '        cmdMyCommand.CommandText += " ,IDPROVENIENZA=@IDPROVENIENZA"
    '        cmdMyCommand.CommandText += " ,NOTE_DICHIARAZIONE=@NOTE_DICHIARAZIONE"
    '        cmdMyCommand.CommandText += " ,DATA_INSERIMENTO=@DATA_INSERIMENTO"
    '        cmdMyCommand.CommandText += " ,DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '        cmdMyCommand.CommandText += " ,DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '        cmdMyCommand.CommandText += " ,OPERATORE=@OPERATORE"
    '        cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyTestata.Id
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = oMyTestata.IdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyTestata.sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyTestata.IdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_DICHIARAZIONE", SqlDbType.DateTime)).Value = oMyTestata.tDataDichiarazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_DICHIARAZIONE", SqlDbType.NVarChar)).Value = oMyTestata.sNDichiarazione
    '        If oMyTestata.tDataProtocollo = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PROTOCOLLO", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PROTOCOLLO", SqlDbType.DateTime)).Value = oMyTestata.tDataProtocollo
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_PROTOCOLLO", SqlDbType.NVarChar)).Value = oMyTestata.sNProtocollo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROVENIENZA", SqlDbType.NVarChar)).Value = oMyTestata.sIdProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE_DICHIARAZIONE", SqlDbType.NVarChar)).Value = oMyTestata.sNoteDichiarazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyTestata.tDataInserimento
    '        If oMyTestata.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyTestata.tDataVariazione
    '        End If
    '        If oMyTestata.tDataCessazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyTestata.tDataCessazione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyTestata.sOperatore
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.UpdateTestata.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function DeleteTestata(ByVal nIdDeleteTestata As Integer, ByVal tDataDelete As Date, ByVal WFSessione As OPENUtility.CreateSessione, Optional ByVal IsErrorInsert As Integer = 0) As Integer
    '    Try
    '        'costruisco la query
    '        If IsErrorInsert = 1 Then
    '            cmdMyCommand.CommandText = "DELETE"
    '            cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '        Else
    '            cmdMyCommand.CommandText = "UPDATE TBLTESTATA SET"
    '            cmdMyCommand.CommandText += " DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '            cmdMyCommand.CommandText += " ,DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '        End If
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.DeleteTestata.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    'Public Function DeleteTestata(ByVal myConnectionString As String, ByVal nIdDeleteTestata As Integer, ByVal tDataDelete As Date, Optional ByVal IsErrorInsert As Integer = 0) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_TBLTESTATA_D"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISERROR", SqlDbType.Int)).Value = IsErrorInsert
    '        'eseguo la query
    '        If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.DeleteTestata.errore: ", Err)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '*** ***
    'Public Function DeleteTestataDichiarazione(ByVal nIdDeleteTestata As Integer, ByVal tDataDelete As Date, ByVal WFSessione As OPENUtility.CreateSessione, Optional ByVal IsErrorInsert As Integer = 0) As Integer
    '    Try
    '        'costruisco la query
    '        If IsErrorInsert = 1 Then
    '            cmdMyCommand.CommandText = "DELETE"
    '            cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '        Else
    '            cmdMyCommand.CommandText = "UPDATE TBLTESTATA SET"
    '            cmdMyCommand.CommandText += " DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '            cmdMyCommand.CommandText += " ,DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '            cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '            cmdMyCommand.CommandText += " AND (IDTESTATA=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = tDataDelete

    '        End If
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.DeleteTestataDichiarazione.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function DeleteTestataDichiarazione(ByVal myConnectionString As String, ByVal nIdDeleteTestata As Integer, ByVal tDataDelete As Date, Optional ByVal IsErrorInsert As Integer = 0) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'costruisco la query
    '        If IsErrorInsert = 1 Then
    '            cmdMyCommand.CommandText = "DELETE"
    '            cmdMyCommand.CommandText += " FROM TBLTESTATA"
    '            cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '        Else
    '            cmdMyCommand.CommandText = "UPDATE TBLTESTATA SET"
    '            cmdMyCommand.CommandText += " DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '            cmdMyCommand.CommandText += " ,DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '            cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '            cmdMyCommand.CommandText += " AND (IDTESTATA=@ID)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteTestata
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = tDataDelete
    '        End If
    '        'eseguo la query
    '        If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTestata.DeleteTestataDichiarazione.errore: ", Err)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
End Class
''' <summary>
''' Classe per la gestione del dettaglio testata
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestDettaglioTestata
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestDettaglioTestata))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    ''' <summary>
    ''' prelevo i dati di dettaglio testata
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="nId"></param>
    ''' <param name="nIdTessera"></param>
    ''' <param name="nIdTestata"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="bIsRicerca"></param>
    ''' <returns></returns>
    Public Function GetDettaglioTestata(ByVal myConnectionString As String, ByVal nId As Integer, ByVal nIdTessera As Integer, ByVal nIdTestata As Integer, ByVal sIdEnte As String, ByVal bIsRicerca As Boolean) As ObjDettaglioTestata()
        Dim myDataReader As SqlClient.SqlDataReader = Nothing
        Dim oDettaglioTestata As ObjDettaglioTestata
        Dim oListDettTestata() As ObjDettaglioTestata = Nothing
        Dim nDettaglioTestata As Integer = -1
        Dim FunctionOggetti As New GestOggetti
        Dim FncRidEse As New GestRidEse
        Dim nMQ As Double
        Dim nVani As Integer
        Dim oRicRidEse As New ObjRidEse

        Dim sSQL As String = ""

        Try
            'Valorizzo la connessione
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GETDETTAGLIOTESTATA", "IDTESTATA", "ID", "IDTESSERA", "NUMERO_TESSERA")
                    myDataReader = ctx.GetDataReader(sSQL, ctx.GetParam("IDTESTATA", nIdTestata) _
                            , ctx.GetParam("ID", nId) _
                            , ctx.GetParam("IDTESSERA", nIdTessera) _
                            , ctx.GetParam("NUMERO_TESSERA", ObjTessera.TESSERA_BIDONE)
                        )
                    Do While myDataReader.Read
                        'incremento l'indice dell'array
                        oDettaglioTestata = New ObjDettaglioTestata
                        nDettaglioTestata += 1
                        oDettaglioTestata.Id = StringOperation.FormatInt(myDataReader("id"))
                        oDettaglioTestata.IdDettaglioTestata = StringOperation.FormatInt(myDataReader("iddettagliotestata"))
                        oDettaglioTestata.IdPadre = StringOperation.FormatInt(myDataReader("idpadre"))
                        '*** X UNIONE CON BANCADATI CMGC ***
                        oDettaglioTestata.IdTestata = StringOperation.FormatInt(myDataReader("idtestata"))
                        '*** ***
                        oDettaglioTestata.IdTessera = StringOperation.FormatInt(myDataReader("idtessera"))
                        oDettaglioTestata.nGGTarsu = StringOperation.FormatInt(myDataReader("ggtarsu"))
                        oDettaglioTestata.nNComponenti = StringOperation.FormatInt(myDataReader("ncomponenti"))
                        Log.Debug("GetDettaglioTestata::devo caricare oggetti")
                        oDettaglioTestata.oOggetti = FunctionOggetti.GetOggetti(myConnectionString, sIdEnte, oDettaglioTestata.Id)
                        oRicRidEse.IdEnte = sIdEnte
                        oDettaglioTestata.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_UI, oDettaglioTestata.Id, "")
                        oDettaglioTestata.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_UI, oDettaglioTestata.Id, "")
                        'calcolo il totale di vani e mq per l'immobile
                        If FunctionOggetti.GetTotOggetti(oDettaglioTestata.oOggetti, nVani, nMQ) <> 1 Then
                            Return Nothing
                        End If
                        oDettaglioTestata.nMQ = nMQ
                        oDettaglioTestata.nVani = nVani
                        oDettaglioTestata.sCodVia = StringOperation.FormatString(myDataReader("codvia"))
                        oDettaglioTestata.sVia = StringOperation.FormatString(myDataReader("via"))
                        If StringOperation.FormatString(myDataReader("civico")) <> "0" And StringOperation.FormatString(myDataReader("civico")) <> "-1" Then
                            oDettaglioTestata.sCivico = StringOperation.FormatString(myDataReader("civico"))
                        End If
                        oDettaglioTestata.sEsponente = StringOperation.FormatString(myDataReader("esponente"))
                        oDettaglioTestata.sInterno = StringOperation.FormatString(myDataReader("interno"))
                        oDettaglioTestata.sScala = StringOperation.FormatString(myDataReader("scala"))
                        oDettaglioTestata.sFoglio = StringOperation.FormatString(myDataReader("foglio"))
                        oDettaglioTestata.sNumero = StringOperation.FormatString(myDataReader("numero"))
                        oDettaglioTestata.sSubalterno = StringOperation.FormatString(myDataReader("subalterno"))
                        '*** 20130201 - gestione mq da catasto per TARES ***
                        oDettaglioTestata.nMQCatasto = StringOperation.FormatDouble(myDataReader("MQCatasto"))
                        '*** ***
                        '*** 20130325 - gestione mq tassabili per TARES ***
                        oDettaglioTestata.nMQTassabili = StringOperation.FormatDouble(myDataReader("MQtassabili"))
                        '*** ***
                        '*** 20130228 - gestione categoria Ateco per TARES ***
                        oDettaglioTestata.IdCatAteco = StringOperation.FormatInt(myDataReader("fk_IdCategoriaAteco"))
                        oDettaglioTestata.sCatAteco = StringOperation.FormatString(myDataReader("DescrCatAteco"))
                        oDettaglioTestata.bForzaPV = StringOperation.FormatBool(myDataReader("forza_calcolapv"))
                        oDettaglioTestata.nComponentiPV = StringOperation.FormatInt(myDataReader("ncomponenti_pv"))
                        '*** ***
                        '***Agenzia Entrate***
                        oDettaglioTestata.sSezione = StringOperation.FormatString(myDataReader("sezione"))
                        oDettaglioTestata.sEstensioneParticella = StringOperation.FormatString(myDataReader("estensione_particella"))
                        oDettaglioTestata.sIdTipoParticella = StringOperation.FormatString(myDataReader("id_tipo_particella"))
                        oDettaglioTestata.nIdTitoloOccupaz = StringOperation.FormatInt(myDataReader("id_titolo_occupazione"))
                        oDettaglioTestata.nIdNaturaOccupaz = StringOperation.FormatInt(myDataReader("id_natura_occupante"))
                        oDettaglioTestata.nIdDestUso = StringOperation.FormatInt(myDataReader("id_destinazione_uso"))
                        oDettaglioTestata.sIdTipoUnita = StringOperation.FormatString(myDataReader("id_tipo_unita"))
                        oDettaglioTestata.nIdAssenzaDatiCatastali = StringOperation.FormatString(myDataReader("id_assenza_dati_catastali"))
                        '*********************
                        oDettaglioTestata.tDataInizio = StringOperation.FormatDateTime(myDataReader("data_inizio"))
                        oDettaglioTestata.tDataFine = StringOperation.FormatDateTime(myDataReader("data_fine"))
                        oDettaglioTestata.sIdStatoOccupazione = StringOperation.FormatString(myDataReader("idstatooccupazione"))
                        oDettaglioTestata.sDescrOccupazione = StringOperation.FormatString(myDataReader("descrizione"))
                        oDettaglioTestata.sNomeOccupantePrec = StringOperation.FormatString(myDataReader("nominativo_occupante_prec"))
                        oDettaglioTestata.sNomeProprietario = StringOperation.FormatString(myDataReader("nominativo_proprietario"))
                        oDettaglioTestata.sNoteUI = StringOperation.FormatString(myDataReader("notedettagliotestata"))
                        oDettaglioTestata.tDataInserimento = StringOperation.FormatDateTime(myDataReader("data_inserimento"))
                        oDettaglioTestata.tDataVariazione = StringOperation.FormatDateTime(myDataReader("data_variazione"))
                        oDettaglioTestata.tDataCessazione = StringOperation.FormatDateTime(myDataReader("data_cessazione"))
                        If bIsRicerca = True Then
                            'prelevo i metri da anater
                            oDettaglioTestata.nMQAnater = GetMQTerritorio(oDettaglioTestata)
                            'prelevo la categoria catastale da ICI
                            oDettaglioTestata.sCatCatastale = GetCatCatastale(myConnectionString, sIdEnte, oDettaglioTestata.sFoglio, oDettaglioTestata.sNumero, oDettaglioTestata.sSubalterno)
                        End If
                        'BD 9/7/2021 Modificata anche la prc_GETDETTAGLIOTESTATA con l'introduzione del campo importofissorid
                        oDettaglioTestata.ImportoFissoRid = StringOperation.FormatDouble(myDataReader("importo_fissorid"))
                        'BD 9/7/2021
                        'dimensiono l'array
                        ReDim Preserve oListDettTestata(nDettaglioTestata)
                        'memorizzo i dati nell'array
                        oListDettTestata(nDettaglioTestata) = oDettaglioTestata
                    Loop
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetDettaglioTestata.errorequery: ", ex)
                    oListDettTestata = Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetDettaglioTestata.errore: ", ex)
            oListDettTestata = Nothing
        Finally
            myDataReader.Close()
        End Try
        Return oListDettTestata
    End Function
    Public Function DeleteDettaglioTestataVani(DBType As String, ByVal myConnectionString As String, ByVal oDettaglio As ObjDettaglioTestata) As Integer
        Dim i As Integer
        Dim cancellaVani As New GestOggetti
        Dim FncGest As New Utility.DichManagerTARSU(DBType, myConnectionString, "", "")

        Try
            If FncGest.SetDettaglioTestata(Utility.Costanti.AZIONE_DELETE, oDettaglio) = 0 Then
                Return 0
            Else
                For i = 0 To oDettaglio.oOggetti.Length - 1
                    If cancellaVani.DeleteOggetti(myConnectionString, oDettaglio.oOggetti(i).IdOggetto, Now, oDettaglio.Id, 0) = 0 Then
                        Return 0
                    End If
                Next
            End If

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.DeleteDettaglioTestataVani.errore: ", Err)
            Return 0
        End Try
    End Function

    Public Function CheckPeriodi(ByVal oListDettagli() As ObjDettaglioTestata, ByVal oSingleDettaglio As ObjDettaglioTestata) As Integer
        Dim x, IsMyFineForzata, IsFineDichForzata As Integer

        Try
            If Not oListDettagli Is Nothing Then
                For x = 0 To oListDettagli.GetUpperBound(0)
                    If oListDettagli(x).tDataFine = Date.MinValue Then
                        oListDettagli(x).tDataFine = Now
                        IsFineDichForzata = 1
                    Else
                        IsFineDichForzata = 0
                    End If
                    If oSingleDettaglio.tDataFine = Date.MinValue Then
                        oSingleDettaglio.tDataFine = Now
                        IsMyFineForzata = 1
                    Else
                        IsMyFineForzata = 0
                    End If
                    If oListDettagli(x).IdDettaglioTestata <> oSingleDettaglio.IdDettaglioTestata And oListDettagli(x).sVia = oSingleDettaglio.sVia And oListDettagli(x).sCivico = oSingleDettaglio.sCivico And oListDettagli(x).sInterno = oSingleDettaglio.sInterno And oListDettagli(x).sEsponente = oSingleDettaglio.sEsponente And oListDettagli(x).sScala = oSingleDettaglio.sScala And oListDettagli(x).sFoglio = oSingleDettaglio.sFoglio And oListDettagli(x).sNumero = oSingleDettaglio.sNumero And oListDettagli(x).sSubalterno = oSingleDettaglio.sSubalterno Then
                        If oSingleDettaglio.tDataInizio >= oListDettagli(x).tDataInizio And oSingleDettaglio.tDataInizio <= oListDettagli(x).tDataFine Then
                            If IsFineDichForzata = 1 Then
                                oListDettagli(x).tDataFine = Date.MinValue
                            End If
                            If IsMyFineForzata = 1 Then
                                oSingleDettaglio.tDataFine = Date.MinValue
                            End If
                            Return 0
                        End If
                        If oSingleDettaglio.tDataFine >= oListDettagli(x).tDataInizio And oSingleDettaglio.tDataFine <= oListDettagli(x).tDataFine Then
                            If IsFineDichForzata = 1 Then
                                oListDettagli(x).tDataFine = Date.MinValue
                            End If
                            If IsMyFineForzata = 1 Then
                                oSingleDettaglio.tDataFine = Date.MinValue
                            End If
                            Return 0
                        End If
                    End If
                    If IsFineDichForzata = 1 Then
                        oListDettagli(x).tDataFine = Date.MinValue
                    End If
                    If IsMyFineForzata = 1 Then
                        oSingleDettaglio.tDataFine = Date.MinValue
                    End If
                Next
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.ChecPeriodi.errore: ", Err)
            Return -1
        End Try
    End Function

    Public Function GetMQTerritorio(ByVal oMyDettaglio As ObjDettaglioTestata) As Double
        Dim nMyMQ As Double = 0
        Dim oRicercaAnater As New RicercaTarsuAnater
        Dim TypeOfRI As Type = GetType(IRemotingInterfaceTARSU)
        Dim RemobjTARSU As IRemotingInterfaceTARSU
        Dim oVanoAnater() As OggettoVanoAnater
        Dim x As Integer

        Try
            If ConstSession.UrlServizioAnater <> "" Then
                'carico i parametri di ricerca
                oRicercaAnater.CodiceComune = ConstSession.IdEnte
                oRicercaAnater.Via = oMyDettaglio.sVia
                If oMyDettaglio.sCivico <> "" Then
                    oRicercaAnater.Civico = StringOperation.FormatInt(oMyDettaglio.sCivico)
                End If
                oRicercaAnater.Interno = oMyDettaglio.sInterno
                oRicercaAnater.Foglio = oMyDettaglio.sFoglio
                oRicercaAnater.Numero = oMyDettaglio.sNumero
                oRicercaAnater.Subalterno = oMyDettaglio.sSubalterno
                'attivo il servizio
                RemobjTARSU = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioAnater)
                'eseguo la ricerca su ANATER
                oVanoAnater = RemobjTARSU.GetVaniAnater(oRicercaAnater)
                For x = 0 To oVanoAnater.GetUpperBound(0)
                    nMyMQ += oVanoAnater(x).CUMSuperficie
                Next
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetMQTerritorio.errore: ", Err)
        End Try
        Return nMyMQ
    End Function
    Public Function GetCatCatastale(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal sSubalterno As String) As String
        Dim sCatCatastale As String = ""
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = "SELECT *"
                    sSQL += " FROM V_ICI_GETCATEGORIACATASTALE"
                    sSQL += " WHERE (IDENTE=@IDENTE)"
                    sSQL += " AND (FOGLIO=@FOGLIO)"
                    sSQL += " AND (NUMERO=@NUMERO)"
                    sSQL += " AND (SUBALTERNO=@SUBALTERNO)"
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("FOGLIO", sFoglio) _
                            , ctx.GetParam("NUMERO", sNumero) _
                            , ctx.GetParam("SUBALTERNO", sSubalterno)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetCatCatastale.erroreQuery: ", ex)
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    sCatCatastale += " " & StringOperation.FormatString(myRow("codcategoriacatastale"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetCatCatastale.errore: ", Err)
        Finally
            myDataView.Dispose()
        End Try
        Return sCatCatastale.Trim
    End Function
    'Public Function GetCatCatastale(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal sFoglio As String, ByVal sNumero As String, ByVal sSubalterno As String) As String
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim sCatCatastale As String = ""
    '    Dim DrDati As SqlClient.SqlDataReader = Nothing

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'prelevo i dati di dettaglio testata
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_ICI_GETCATEGORIACATASTALE"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (FOGLIO=@FOGLIO)"
    '        cmdMyCommand.CommandText += " AND (NUMERO=@NUMERO)"
    '        cmdMyCommand.CommandText += " AND (SUBALTERNO=@SUBALTERNO)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = sFoglio
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = sNumero
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.NVarChar)).Value = sSubalterno
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrDati = cmdMyCommand.ExecuteReader
    '        Do While DrDati.Read
    '            If Not IsDBNull(DrDati("codcategoriacatastale")) Then
    '                sCatCatastale += " " & StringOperation.FormatString(DrDati("codcategoriacatastale"))
    '            End If
    '        Loop
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetCatCatastale.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '    Finally
    '        DrDati.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    '    Return sCatCatastale.Trim
    'End Function

    ''*** 20131104 - TARES ***
    'Public Function GetDettaglioTestata(ByVal nIdTessera As Integer, ByVal nIdTestata As Integer, ByVal sIdEnte As String, ByVal bIsRicerca As Boolean, ByRef DBEngineOut As DBEngine) As ObjDettaglioTestata()
    '    Dim oDettaglioTestata As ObjDettaglioTestata
    '    Dim oListDettTestata As New ArrayList
    '    Dim nDettaglioTestata As Integer = -1
    '    Dim FunctionOggetti As New GestOggetti
    '    Dim FncRidEse As New GestRidEse
    '    Dim nMQ As Double
    '    Dim nVani As Integer
    '    Dim oRicRidEse As New ObjRidEse
    '    Dim MyDBEngine As DBEngine = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '        End If

    '        MyDBEngine.ClearParameters()
    '        MyDBEngine.AddParameter("@IDTESSERA", nIdTessera, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@NUMERO_TESSERA", ObjTessera.TESSERA_BIDONE, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IDTESTATA", nIdTestata, ParameterDirection.Input)
    '        MyDBEngine.ExecuteQuery(dtMyDati, "prc_GetRiepilogoDaElaborare", CommandType.StoredProcedure)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oDettaglioTestata = New ObjDettaglioTestata
    '            nDettaglioTestata += 1
    '            oDettaglioTestata.Id = StringOperation.Formatint(dtMyRow("id"))
    '            oDettaglioTestata.IdDettaglioTestata = StringOperation.Formatint(dtMyRow("iddettagliotestata"))
    '            If Not IsDBNull(dtMyRow("idpadre")) Then
    '                oDettaglioTestata.IdPadre = StringOperation.Formatint(dtMyRow("idpadre"))
    '            End If
    '            oDettaglioTestata.IdTessera = StringOperation.Formatint(dtMyRow("idtessera"))
    '            If Not IsDBNull(dtMyRow("ggtarsu")) Then
    '                oDettaglioTestata.nGGTarsu = StringOperation.Formatint(dtMyRow("ggtarsu"))
    '            End If
    '            If Not IsDBNull(dtMyRow("ncomponenti")) Then
    '                oDettaglioTestata.nNComponenti = StringOperation.Formatint(dtMyRow("ncomponenti"))
    '            End If
    '            oDettaglioTestata.oOggetti = FunctionOggetti.GetOggetti(sIdEnte, oDettaglioTestata.Id, WFSessione)
    '            oRicRidEse.IdEnte = sIdEnte
    '            oDettaglioTestata.oRiduzioni = FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_UI, oDettaglioTestata.Id)
    '            oDettaglioTestata.oDetassazioni = FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_UI, oDettaglioTestata.Id)
    '            'calcolo il totale di vani e mq per l'immobile
    '            If FunctionOggetti.GetTotOggetti(oDettaglioTestata.oOggetti, nVani, nMQ) <> 1 Then
    '                Exit Function
    '            End If
    '            oDettaglioTestata.nMQ = nMQ
    '            oDettaglioTestata.nVani = nVani
    '            If Not IsDBNull(dtMyRow("codvia")) Then
    '                oDettaglioTestata.sCodVia = StringOperation.FormatString(dtMyRow("codvia"))
    '            End If
    '            If Not IsDBNull(dtMyRow("via")) Then
    '                oDettaglioTestata.sVia = StringOperation.FormatString(dtMyRow("via"))
    '            End If
    '            If Not IsDBNull(dtMyRow("civico")) Then
    '                If StringOperation.FormatString(dtMyRow("civico")) <> "0" And StringOperation.FormatString(dtMyRow("civico")) <> "-1" Then
    '                    oDettaglioTestata.sCivico = StringOperation.FormatString(dtMyRow("civico"))
    '                End If
    '            End If
    '            If Not IsDBNull(dtMyRow("esponente")) Then
    '                oDettaglioTestata.sEsponente = StringOperation.FormatString(dtMyRow("esponente"))
    '            End If
    '            If Not IsDBNull(dtMyRow("interno")) Then
    '                oDettaglioTestata.sInterno = StringOperation.FormatString(dtMyRow("interno"))
    '            End If
    '            If Not IsDBNull(dtMyRow("scala")) Then
    '                oDettaglioTestata.sScala = StringOperation.FormatString(dtMyRow("scala"))
    '            End If
    '            If Not IsDBNull(dtMyRow("foglio")) Then
    '                oDettaglioTestata.sFoglio = StringOperation.FormatString(dtMyRow("foglio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("numero")) Then
    '                oDettaglioTestata.sNumero = StringOperation.FormatString(dtMyRow("numero"))
    '            End If
    '            If Not IsDBNull(dtMyRow("subalterno")) Then
    '                oDettaglioTestata.sSubalterno = StringOperation.FormatString(dtMyRow("subalterno"))
    '            End If
    '            '***Agenzia Entrate***
    '            If Not IsDBNull(dtMyRow("sezione")) Then
    '                oDettaglioTestata.sSezione = StringOperation.FormatString(dtMyRow("sezione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("estensione_particella")) Then
    '                oDettaglioTestata.sEstensioneParticella = StringOperation.FormatString(dtMyRow("estensione_particella"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_tipo_particella")) Then
    '                oDettaglioTestata.sIdTipoParticella = StringOperation.FormatString(dtMyRow("id_tipo_particella"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_titolo_occupazione")) Then
    '                oDettaglioTestata.nIdTitoloOccupaz = StringOperation.Formatint(dtMyRow("id_titolo_occupazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_natura_occupante")) Then
    '                oDettaglioTestata.nIdNaturaOccupaz = StringOperation.Formatint(dtMyRow("id_natura_occupante"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_destinazione_uso")) Then
    '                oDettaglioTestata.nIdDestUso = StringOperation.Formatint(dtMyRow("id_destinazione_uso"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_tipo_unita")) Then
    '                oDettaglioTestata.sIdTipoUnita = StringOperation.FormatString(dtMyRow("id_tipo_unita"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_assenza_dati_catastali")) Then
    '                oDettaglioTestata.nIdAssenzaDatiCatastali = StringOperation.FormatString(dtMyRow("id_assenza_dati_catastali"))
    '            End If
    '            '*********************
    '            oDettaglioTestata.tDataInizio = StringOperation.Formatdatetime(dtMyRow("data_inizio"))
    '            If Not IsDBNull(dtMyRow("data_fine")) Then
    '                oDettaglioTestata.tDataFine = StringOperation.Formatdatetime(dtMyRow("data_fine"))
    '            End If
    '            If Not IsDBNull(dtMyRow("idstatooccupazione")) Then
    '                oDettaglioTestata.sIdStatoOccupazione = StringOperation.FormatString(dtMyRow("idstatooccupazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("descrizione")) Then
    '                oDettaglioTestata.sDescrOccupazione = StringOperation.FormatString(dtMyRow("descrizione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("nominativo_occupante_prec")) Then
    '                oDettaglioTestata.sNomeOccupantePrec = StringOperation.FormatString(dtMyRow("nominativo_occupante_prec"))
    '            End If
    '            If Not IsDBNull(dtMyRow("nominativo_proprietario")) Then
    '                oDettaglioTestata.sNomeProprietario = StringOperation.FormatString(dtMyRow("nominativo_proprietario"))
    '            End If
    '            If Not IsDBNull(dtMyRow("notedettagliotestata")) Then
    '                oDettaglioTestata.sNoteUI = StringOperation.FormatString(dtMyRow("notedettagliotestata"))
    '            End If
    '            oDettaglioTestata.tDataInserimento = StringOperation.Formatdatetime(dtMyRow("data_inserimento"))
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oDettaglioTestata.tDataVariazione = StringOperation.Formatdatetime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oDettaglioTestata.tDataCessazione = StringOperation.Formatdatetime(dtMyRow("data_cessazione"))
    '            End If
    '            'If bIsRicerca = True Then
    '            '    'prelevo i metri da anater
    '            '    oDettaglioTestata.nMQAnater = GetMQTerritorio(oDettaglioTestata)
    '            '    'prelevo la categoria catastale da ICI
    '            '    oDettaglioTestata.sCatCatastale = GetCatCatastale(sIdEnte, oDettaglioTestata.sFoglio, oDettaglioTestata.sNumero, oDettaglioTestata.sSubalterno, WFSessione)
    '            'End If

    '            oListDettTestata.Add(oDettaglioTestata)
    '        Next

    '        Return CType(oListDettTestata.ToArray(GetType(ObjDettaglioTestata)), ObjDettaglioTestata())
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestDettaglioTestata.GetDettaglioTestata.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CloseConnection()
    '        End If
    '    End Try
    'End Function
    ''*** ***
End Class
''' <summary>
''' Classe per la gestione degli oggetti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestOggetti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjOggetti))
    Private oReplace As New generalClass.generalFunction

    '*** 20140805 - Gestione Categorie Vani ***
    Public Function GetOggetti(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal nIdGetDettaglioTestata As Integer) As ObjOggetti()
        Dim oOggettiDich As ObjOggetti
        Dim oListOggettiDich() As ObjOggetti = Nothing
        Dim nListOggetti As Integer = -1
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLOGGETTI_S", "IDENTE", "IDDETTAGLIOTESTATA")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDDETTAGLIOTESTATA", nIdGetDettaglioTestata)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestOggetti.GetOggetti.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    'incremento l'indice dell'array
                    nListOggetti += 1
                    oOggettiDich = New ObjOggetti
                    oOggettiDich.IdOggetto = StringOperation.FormatInt(myRow("id"))
                    oOggettiDich.IdDettaglioTestata = nIdGetDettaglioTestata
                    oOggettiDich.IdCategoria = StringOperation.FormatString(myRow("idcategoria"))
                    If StringOperation.FormatString(myRow("idcategoria")) <> "-1" Then
                        oOggettiDich.sCategoria = StringOperation.FormatString(myRow("idcategoria")) & " - " & StringOperation.FormatString(myRow("descrcat"))
                    End If
                    oOggettiDich.IdTipoVano = StringOperation.FormatString(myRow("idtipovano"))
                    oOggettiDich.sTipoVano = StringOperation.FormatString(myRow("descrvano"))
                    oOggettiDich.nVani = StringOperation.FormatInt(myRow("nvani"))
                    oOggettiDich.nMq = StringOperation.FormatDouble(myRow("mq"))
                    '*** 20130325 - gestione mq tassabili per TARES ***
                    oOggettiDich.bIsEsente = StringOperation.FormatBool(myRow("esente"))
                    '*** ***
                    '*** 20140805 - Gestione Categorie Vani ***
                    oOggettiDich.IdCatTARES = StringOperation.FormatInt(myRow("fk_idcategoriaateco"))
                    oOggettiDich.sDescrCatTARES = StringOperation.FormatString(myRow("DESCRCATTARES"))
                    oOggettiDich.nNC = StringOperation.FormatInt(myRow("ncomponenti"))
                    oOggettiDich.nNCPV = StringOperation.FormatInt(myRow("ncomponenti_pv"))
                    oOggettiDich.bForzaCalcolaPV = StringOperation.FormatBool(myRow("FORZA_CALCOLAPV"))
                    '*** ***
                    oOggettiDich.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                    oOggettiDich.sProvenienza = StringOperation.FormatString(myRow("provenienza"))
                    oOggettiDich.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                    oOggettiDich.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))

                    'dimensiono l'array
                    ReDim Preserve oListOggettiDich(nListOggetti)
                    'memorizzo i dati nell'array
                    oListOggettiDich(nListOggetti) = oOggettiDich
                Next
            End Using
            Return oListOggettiDich
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestOggetti.GetOggetti.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function GetOggetti(ByVal myConnectionString As String, ByVal sIdEnte As String, ByVal nIdGetDettaglioTestata As Integer) As ObjOggetti()
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim DrDati As SqlClient.SqlDataReader = Nothing
    '    Dim oOggettiDich As ObjOggetti
    '    Dim oListOggettiDich() As ObjOggetti = Nothing
    '    Dim nListOggetti As Integer = -1

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_TBLOGGETTI_S"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = nIdGetDettaglioTestata
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrDati = cmdMyCommand.ExecuteReader
    '        Do While DrDati.Read
    '            'incremento l'indice dell'array
    '            nListOggetti += 1
    '            oOggettiDich = New ObjOggetti
    '            oOggettiDich.IdOggetto = StringOperation.FormatInt(DrDati("id"))
    '            oOggettiDich.IdDettaglioTestata = nIdGetDettaglioTestata
    '            oOggettiDich.IdCategoria = StringOperation.FormatString(DrDati("idcategoria"))
    '            If StringOperation.FormatString(DrDati("idcategoria")) <> "-1" Then
    '                oOggettiDich.sCategoria = StringOperation.FormatString(DrDati("idcategoria")) & " - " & StringOperation.FormatString(DrDati("descrcat"))
    '            End If
    '            oOggettiDich.IdTipoVano = StringOperation.FormatString(DrDati("idtipovano"))
    '            oOggettiDich.sTipoVano = StringOperation.FormatString(DrDati("descrvano"))
    '            oOggettiDich.nVani = StringOperation.FormatInt(DrDati("nvani"))
    '            oOggettiDich.nMq = StringOperation.FormatDouble(DrDati("mq"))
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            oOggettiDich.bIsEsente = stringoperation.formatbool(DrDati("esente"))
    '            '*** ***
    '            '*** 20140805 - Gestione Categorie Vani ***
    '            If Not IsDBNull(DrDati("fk_idcategoriaateco")) Then
    '                oOggettiDich.IdCatTARES = StringOperation.FormatInt(DrDati("fk_idcategoriaateco"))
    '            End If
    '            If Not IsDBNull(DrDati("DESCRCATTARES")) Then
    '                oOggettiDich.sDescrCatTARES = StringOperation.FormatString(DrDati("DESCRCATTARES"))
    '            End If
    '            If Not IsDBNull(DrDati("ncomponenti")) Then
    '                oOggettiDich.nNC = StringOperation.FormatInt(DrDati("ncomponenti"))
    '            End If
    '            If Not IsDBNull(DrDati("ncomponenti_pv")) Then
    '                oOggettiDich.nNCPV = StringOperation.FormatInt(DrDati("ncomponenti_pv"))
    '            End If
    '            If Not IsDBNull(DrDati("FORZA_CALCOLAPV")) Then
    '                oOggettiDich.bForzaCalcolaPV = stringoperation.formatbool(DrDati("FORZA_CALCOLAPV"))
    '            End If
    '            '*** ***
    '            oOggettiDich.tDataInserimento = StringOperation.FormatDateTime(DrDati("data_inserimento"))
    '            If Not IsDBNull(DrDati("provenienza")) Then
    '                oOggettiDich.sProvenienza = StringOperation.FormatString(DrDati("provenienza"))
    '            Else
    '                oOggettiDich.sProvenienza = ""
    '            End If
    '            If Not IsDBNull(DrDati("data_variazione")) Then
    '                oOggettiDich.tDataVariazione = StringOperation.FormatDateTime(DrDati("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrDati("data_cessazione")) Then
    '                oOggettiDich.tDataCessazione = StringOperation.FormatDateTime(DrDati("data_cessazione"))
    '            End If

    '            'dimensiono l'array
    '            ReDim Preserve oListOggettiDich(nListOggetti)
    '            'memorizzo i dati nell'array
    '            oListOggettiDich(nListOggetti) = oOggettiDich
    '        Loop

    '        Return oListOggettiDich
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestOggetti.GetOggetti.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '*** ***
    Public Function DeleteOggetti(ByVal myConnectionString As String, ByVal nIdDeleteOggetti As Integer, ByVal tDataDelete As Date, ByVal IdDettaglioTestata As Integer, ByRef nMQTassabili As Double) As Integer
        Dim DrReturn As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'costruisco la query
            cmdMyCommand.CommandText = "prc_TBLOGGETTI_D"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA", SqlDbType.DateTime)).Value = tDataDelete
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteOggetti
            'eseguo la query
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            If cmdMyCommand.ExecuteNonQuery <> 1 Then
                Return 0
            End If
            '*** 20130325 - gestione mq tassabili per TARES ***
            'devo aggiornare i metri tassabili sul dettagliotestata
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_SETMQTASSABILI"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdDettaglioTestata", SqlDbType.Int)).Value = IdDettaglioTestata
            'eseguo la query()
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                nMQTassabili = DrReturn(0)
            Loop
            DrReturn.Close()
            '*** ***
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestOggetti.DeleteOggetti.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    Public Function GetTotOggetti(ByVal oMyListOggetti() As ObjOggetti, ByRef nTotVani As Integer, ByRef nTotMq As Double) As Integer
        Dim x As Integer
        Try
            'ciclo su tutti i vani e totalizzo
            nTotVani = 0 : nTotMq = 0
            If Not IsNothing(oMyListOggetti) Then
                For x = 0 To oMyListOggetti.GetUpperBound(0)
                    nTotVani += oMyListOggetti(x).nVani
                    nTotMq += oMyListOggetti(x).nMq
                Next
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestOggetti.GetTotOggetti.errore: ", Err)
            Return -1
        End Try
    End Function

    'Public Function GetOggetti(ByVal sIdEnte As String, ByVal nIdGetDettaglioTestata As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjOggetti()
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oOggettiDich As ObjOggetti
    '    Dim oListOggettiDich() As ObjOggetti
    '    Dim nListOggetti As Integer = -1

    '    Try
    '        'prelevo i dati di oggetti
    '        cmdMyCommand.CommandText = "SELECT TBLOGGETTI.*, TBLCATEGORIE.DESCRIZIONE AS DESCRCAT, TBLTIPOVANI.DESCRIZIONE AS DESCRVANO"
    '        cmdMyCommand.CommandText += " FROM TBLOGGETTI INNER JOIN TBLTIPOVANI ON TBLOGGETTI.IDTIPOVANO=TBLTIPOVANI.IDTIPOVANO"
    '        cmdMyCommand.CommandText += " INNER JOIN TBLCATEGORIE ON TBLOGGETTI.IDCATEGORIA=TBLCATEGORIE.CODICE"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        cmdMyCommand.CommandText += " AND (TBLCATEGORIE.IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (TBLTIPOVANI.IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandText += " AND (IDDETTAGLIOTESTATA=@IDDETTAGLIOTESTATA)"
    '        cmdMyCommand.CommandText += " ORDER BY TBLOGGETTI.IDCATEGORIA,TBLOGGETTI.IDTIPOVANO"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = nIdGetDettaglioTestata
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            'incremento l'indice dell'array
    '            nListOggetti += 1
    '            oOggettiDich = New ObjOggetti
    '            oOggettiDich.IdOggetto = StringOperation.Formatint(DrDati("id"))
    '            oOggettiDich.IdDettaglioTestata = nIdGetDettaglioTestata
    '            oOggettiDich.IdCategoria = StringOperation.FormatString(DrDati("idcategoria"))
    '            If StringOperation.FormatString(DrDati("idcategoria")) <> "-1" Then
    '                oOggettiDich.sCategoria = StringOperation.FormatString(DrDati("idcategoria")) & " - " & StringOperation.FormatString(DrDati("descrcat"))
    '            End If
    '            oOggettiDich.IdTipoVano = StringOperation.FormatString(DrDati("idtipovano"))
    '            oOggettiDich.sTipoVano = StringOperation.FormatString(DrDati("descrvano"))
    '            oOggettiDich.nVani = StringOperation.Formatint(DrDati("nvani"))
    '            oOggettiDich.nMq = stringoperation.formatdouble(DrDati("mq"))
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            oOggettiDich.bIsEsente = stringoperation.formatbool(DrDati("esente"))
    '            '*** ***
    '            '*** 20140805 - Gestione Categorie Vani ***
    '            If Not IsDBNull(DrDati("fk_idcategoriaateco")) Then
    '                oOggettiDich.IdCatTARES = StringOperation.Formatint(DrDati("fk_idcategoriaateco"))
    '            End If
    '            If Not IsDBNull(DrDati("ncomponenti")) Then
    '                oOggettiDich.nNC = StringOperation.Formatint(DrDati("ncomponenti"))
    '            End If
    '            If Not IsDBNull(DrDati("ncomponenti_pv")) Then
    '                oOggettiDich.nNCPV = StringOperation.Formatint(DrDati("ncomponenti_pv"))
    '            End If
    '            If Not IsDBNull(DrDati("FORZA_CALCOLAPV")) Then
    '                oOggettiDich.bForzaCalcolaPV = stringoperation.formatbool(DrDati("FORZA_CALCOLAPV"))
    '            End If
    '            '*** ***
    '            oOggettiDich.tDataInserimento = StringOperation.Formatdatetime(DrDati("data_inserimento"))
    '            If Not IsDBNull(DrDati("provenienza")) Then
    '                oOggettiDich.sProvenienza = StringOperation.FormatString(DrDati("provenienza"))
    '            Else
    '                oOggettiDich.sProvenienza = ""
    '            End If
    '            If Not IsDBNull(DrDati("data_variazione")) Then
    '                oOggettiDich.tDataVariazione = StringOperation.Formatdatetime(DrDati("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrDati("data_cessazione")) Then
    '                oOggettiDich.tDataCessazione = StringOperation.Formatdatetime(DrDati("data_cessazione"))
    '            End If

    '            'dimensiono l'array
    '            ReDim Preserve oListOggettiDich(nListOggetti)
    '            'memorizzo i dati nell'array
    '            oListOggettiDich(nListOggetti) = oOggettiDich
    '        Loop
    '        DrDati.Close()

    '        Return oListOggettiDich
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestOggetti.GetOggetti.errore: ", Err)
    '    End Try
    'End Function

    'Public Function DeleteOggetti(ByVal WFSessione As OPENUtility.CreateSessione, ByVal nIdDeleteOggetti As Integer, ByVal tDataDelete As Date, ByVal IdDettaglioTestata As Integer, ByRef nMQTassabili As Double) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Try
    '        'costruisco la query
    '        cmdMyCommand.CommandText = "UPDATE TBLOGGETTI SET"
    '        cmdMyCommand.CommandText += " DATA_VARIAZIONE=@DATA"
    '        cmdMyCommand.CommandText += ",DATA_CESSAZIONE=@DATA"
    '        cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA", SqlDbType.DateTime)).Value = tDataDelete
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = nIdDeleteOggetti
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return 0
    '        End If
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        'devo aggiornare i metri tassabili sul dettagliotestata
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_SETMQTASSABILI"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdDettaglioTestata", SqlDbType.Int)).Value = IdDettaglioTestata
    '        'eseguo la query()
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            nMQTassabili = DrReturn(0)
    '        Loop
    '        DrReturn.Close()
    '        '*** ***
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestOggetti.DeleteOggetti.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    '*** 20140805 - Gestione Categorie Vani ***
    'Public Function UpdateOggetti(ByVal WFSessione As OPENUtility.CreateSessione, ByVal oMyOggetti As ObjOggetti) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Try
    '        'costruisco la query
    '        cmdMyCommand.CommandText = "UPDATE TBLOGGETTI SET"
    '        cmdMyCommand.CommandText += " IDDETTAGLIOTESTATA=@IDDETTAGLIOTESTATA"
    '        cmdMyCommand.CommandText += ",IDCATEGORIA=@IDCATEGORIA"
    '        cmdMyCommand.CommandText += ",IDTIPOVANO=@IDTIPOVANO"
    '        cmdMyCommand.CommandText += ",NVANI=@NVANI"
    '        cmdMyCommand.CommandText += ",MQ=@MQ"
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        cmdMyCommand.CommandText += ", ESENTE=@ESENTE"
    '        '*** ***
    '        cmdMyCommand.CommandText += ",DATA_INSERIMENTO=@DATA_INSERIMENTO"
    '        cmdMyCommand.CommandText += ",DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '        cmdMyCommand.CommandText += ",DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '        cmdMyCommand.CommandText += ",OPERATORE=@OPERATORE"
    '        cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyOggetti.IdOggetto
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = oMyOggetti.IdDettaglioTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyOggetti.IdCategoria
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOVANO", SqlDbType.NVarChar)).Value = oMyOggetti.IdTipoVano
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NVANI", SqlDbType.Int)).Value = oMyOggetti.nVani
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyOggetti.nMq
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTE", SqlDbType.Bit)).Value = StringOperation.Formatint(oMyOggetti.bIsEsente)
    '        '*** ***
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyOggetti.tDataInserimento
    '        If oMyOggetti.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyOggetti.tDataVariazione
    '        End If
    '        If oMyOggetti.tDataCessazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyOggetti.tDataCessazione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyOggetti.sOperatore
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestOggetti.UpdateOggetti.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function
    ''*** 20130325 - gestione mq tassabili per TARES ***

    'Public Function SetOggetti(ByVal oMyNewOggetti() As ObjOggetti, ByVal IdNewDettTestata As Integer, ByVal WFSessione As OPENUtility.CreateSessione, ByRef nMQTassabili As Double) As Integer
    '    Dim x As Integer
    '    Dim myIdentity As Integer
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        'costruisco la query
    '        For x = 0 To oMyNewOggetti.GetUpperBound(0)
    '            cmdMyCommand.CommandText = "INSERT INTO TBLOGGETTI (IDDETTAGLIOTESTATA, IDCATEGORIA, IDTIPOVANO, NVANI, MQ"
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            cmdMyCommand.CommandText += ", ESENTE"
    '            '*** ***
    '            cmdMyCommand.CommandText += " ,PROVENIENZA, DATA_INSERIMENTO,DATA_VARIAZIONE,DATA_CESSAZIONE,OPERATORE)"
    '            cmdMyCommand.CommandText += " VALUES(@IDDETTAGLIOTESTATA,@IDCATEGORIA,@IDTIPOVANO,@NVANI,@MQ"
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            cmdMyCommand.CommandText += ", @ESENTE"
    '            '*** ***
    '            cmdMyCommand.CommandText += " ,@PROVENIENZA,@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '            cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = IdNewDettTestata
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyNewOggetti(x).IdCategoria
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOVANO", SqlDbType.NVarChar)).Value = oMyNewOggetti(x).IdTipoVano
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NVANI", SqlDbType.Int)).Value = oMyNewOggetti(x).nVani
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyNewOggetti(x).nMq
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTE", SqlDbType.Bit)).Value = oMyNewOggetti(x).bIsEsente
    '            '*** ***
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyNewOggetti(x).sProvenienza
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyNewOggetti(x).tDataInserimento
    '            If oMyNewOggetti(x).tDataVariazione = Date.MinValue Then
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '            Else
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyNewOggetti(x).tDataVariazione
    '            End If
    '            If oMyNewOggetti(x).tDataCessazione = Date.MinValue Then
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '            Else
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyNewOggetti(x).tDataCessazione
    '            End If
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyNewOggetti(x).sOperatore

    '            '*** IdOggetto = -1  -> nuovo vano (aggiornamento dell'idOggetto)
    '            '*** IdNewDettTestata <> -1  -> dichiarazione ed immobile già presenti nel db
    '            'eseguo la query
    '            DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '            Do While DrReturn.Read
    '                myIdentity = DrReturn(0)
    '                'aggiungere campo per vecchio idOggetto = oMyNewOggetti(x).IdOggetto
    '                oMyNewOggetti(x).IdOggettoOld = oMyNewOggetti(x).IdOggetto
    '                oMyNewOggetti(x).IdOggetto = myIdentity
    '            Loop
    '            oMyNewOggetti(x).IdDettaglioTestata = IdNewDettTestata
    '        Next
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        'devo aggiornare i metri tassabili sul dettagliotestata
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_SETMQTASSABILI"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdDettaglioTestata", SqlDbType.Int)).Value = oMyNewOggetti(0).IdDettaglioTestata
    '        'eseguo la query()
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            nMQTassabili = DrReturn(0)
    '        Loop
    '        DrReturn.Close()
    '        '*** ***
    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestOggetti.SetOggetti.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    'Public Function SetOggetti(ByVal myConnectionString As String, ByVal oMyNewOggetti() As ObjOggetti, ByVal nIdDettTestata As Integer, ByRef nMQTassabili As Double) As Integer
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        'costruisco la query
    '        For Each oMyOggetto As ObjOggetti In oMyNewOggetti
    '            oMyOggetto.IdDettaglioTestata = nIdDettTestata
    '            oMyOggetto.IdOggetto = -1
    '            oMyOggetto.tDataInserimento = Now
    '            If SetOggetto(myConnectionString, oMyOggetto) <= 0 Then
    '                Return 0
    '            End If
    '        Next
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        Try
    '            'devo aggiornare i metri tassabili sul dettagliotestata
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.CommandType = CommandType.StoredProcedure
    '            cmdMyCommand.CommandText = "prc_SETMQTASSABILI"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdDettaglioTestata", SqlDbType.Int)).Value = oMyNewOggetti(0).IdDettaglioTestata
    '            'eseguo la query()
    '            DrReturn = cmdMyCommand.ExecuteReader
    '            Do While DrReturn.Read
    '                nMQTassabili = DrReturn(0)
    '            Loop
    '        Catch ex As Exception
    '            Log.Debug("Si è verificato un errore in ObjOggetti::SetOggetti::", ex)
    '            Return 0
    '        Finally
    '            DrReturn.Close()
    '            cmdMyCommand.Connection.Close()
    '            cmdMyCommand.Dispose()
    '        End Try
    '        '*** ***
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestOggetti.SetOggetti.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetOggetto(ByVal myConnectionString As String, ByVal oNewOggetto As ObjOggetti) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        'costruisco la query
    '        cmdMyCommand.CommandText = "prc_TBLOGGETTI_IU"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oNewOggetto.IdOggetto
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = oNewOggetto.IdDettaglioTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oNewOggetto.IdCategoria
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPOVANO", SqlDbType.NVarChar)).Value = oNewOggetto.IdTipoVano
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NVANI", SqlDbType.Int)).Value = oNewOggetto.nVani
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oNewOggetto.nMq
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESENTE", SqlDbType.Bit)).Value = oNewOggetto.bIsEsente
    '        '*** ***
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oNewOggetto.sProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oNewOggetto.tDataInserimento
    '        If oNewOggetto.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oNewOggetto.tDataVariazione
    '        End If
    '        If oNewOggetto.tDataCessazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oNewOggetto.tDataCessazione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oNewOggetto.sOperatore
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@fk_IdCategoriaAteco", SqlDbType.Int)).Value = oNewOggetto.IdCatTARES
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = oNewOggetto.nNC
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI_PV", SqlDbType.Int)).Value = oNewOggetto.nNCPV
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FORZA_CALCOLAPV", SqlDbType.Bit)).Value = oNewOggetto.bForzaCalcolaPV
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        '*** IdOggetto = -1  -> nuovo vano (aggiornamento dell'idOggetto)
    '        '*** IdNewDettTestata <> -1  -> dichiarazione ed immobile già presenti nel db
    '        'aggiungere campo per vecchio idOggetto = oNewOggetto.IdOggetto
    '        oNewOggetto.IdOggettoOld = oNewOggetto.IdOggetto
    '        'eseguo la query
    '        cmdMyCommand.ExecuteNonQuery()
    '        oNewOggetto.IdOggetto = cmdMyCommand.Parameters("@ID").Value
    '        Return oNewOggetto.IdOggetto
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestOggetti.SetOggetto.errore: ", Err)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '*** ***'*** ***
End Class
''' <summary>
''' Classe per la gestione dei dati della famiglia
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestFamiglia
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjFamiglia))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    'Public Function GetFamiglia(ByVal WFSessione As OPENUtility.CreateSessione, ByVal nIdGetTestata As Integer) As ObjFamiglia()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oFamigliaDich As ObjFamiglia
    '    Dim oListFamigliaDich() As ObjFamiglia
    '    Dim nListFamiglia As Integer = -1

    '    Try
    '        'prelevo i dati di Famiglia
    '        cmdMyCommand.CommandText = "SELECT TBLTESTATAFAMIGLIA.*, DESCRIZIONE"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATAFAMIGLIA"
    '        cmdMyCommand.CommandText += " LEFT JOIN TBLPARENTELE ON TBLTESTATAFAMIGLIA.IDPARENTELA=TBLPARENTELE.IDPARENTELA"
    '        cmdMyCommand.CommandText += " WHERE (IDTESTATA=@IDTESTATA)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdGetTestata
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            'incremento l'indice dell'array
    '            nListFamiglia += 1
    '            oFamigliaDich = New ObjFamiglia
    '            oFamigliaDich.IdFamiglia = StringOperation.Formatint(DrDati("id"))
    '            oFamigliaDich.IdTestata = nIdGetTestata
    '            If Not IsDBNull(DrDati("cognome")) Then
    '                oFamigliaDich.sCognome = StringOperation.FormatString(DrDati("cognome"))
    '            End If
    '            If Not IsDBNull(DrDati("nome")) Then
    '                oFamigliaDich.sNome = StringOperation.FormatString(DrDati("nome"))
    '            End If
    '            If Not IsDBNull(DrDati("luogo_nascita")) Then
    '                oFamigliaDich.sLuogoNascita = StringOperation.FormatString(DrDati("luogo_nascita"))
    '            End If
    '            If Not IsDBNull(DrDati("data_nascita")) Then
    '                oFamigliaDich.tDataNascita = StringOperation.Formatdatetime(DrDati("data_nascita"))
    '            End If
    '            If Not IsDBNull(DrDati("idparentela")) Then
    '                oFamigliaDich.sParentela = StringOperation.FormatString(DrDati("idparentela"))
    '            End If
    '            If Not IsDBNull(DrDati("descrizione")) Then
    '                oFamigliaDich.sDescrParentela = StringOperation.FormatString(DrDati("descrizione"))
    '            End If

    '            'dimensiono l'array
    '            ReDim Preserve oListFamigliaDich(nListFamiglia)
    '            'memorizzo i dati nell'array
    '            oListFamigliaDich(nListFamiglia) = oFamigliaDich
    '        Loop

    '        Return oListFamigliaDich
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestFamiglia.GetFamiglia.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function SetFamiglia(ByVal WFSessione As OPENUtility.CreateSessione, ByVal oMyNewFamiglia() As ObjFamiglia, ByVal IdNewDettTestata As Integer) As Integer
    '    Dim x As Integer

    '    Try
    '        'costruisco la query
    '        For x = 0 To oMyNewFamiglia.GetUpperBound(0)
    '            cmdMyCommand.CommandText = "INSERT INTO TBLTESTATAFAMIGLIA (IDTESTATA,"
    '            cmdMyCommand.CommandText += " COGNOME, NOME, DATA_NASCITA, LUOGO_NASCITA, IDPARENTELA)"
    '            cmdMyCommand.CommandText += " VALUES (@IDTESTATA"
    '            cmdMyCommand.CommandText += ",@COGNOME,@NOME,@DATA_NASCITA,@LUOGO_NASCITA,@IDPARENTELA)"
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = IdNewDettTestata
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sCognome
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sNome
    '            If oMyNewFamiglia(x).tDataNascita <> Date.MinValue Then
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_NASCITA", SqlDbType.DateTime)).Value = oMyNewFamiglia(x).tDataNascita
    '            Else
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_NASCITA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '            End If
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LUOGO_NASCITA", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sLuogoNascita
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPARENTELA", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sParentela
    '            'eseguo la query
    '            If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '                Return 0
    '            End If
    '        Next
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestFamiglia.SetFamiglia.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function UpdateFamiglia(ByVal WFSessione As OPENUtility.CreateSessione, ByVal oMyFamiglia As ObjFamiglia) As Integer
    '    Dim x As Integer = 0

    '    Try
    '        'costruisco la query
    '        cmdMyCommand.CommandText = "UPDATE TBLTESTATAFAMIGLIA SET"
    '        cmdMyCommand.CommandText += " IDTESTATA=@IDTESTATA"
    '        cmdMyCommand.CommandText += ",COGNOME=@COGNOME"
    '        cmdMyCommand.CommandText += ",NOME=@NOME"
    '        cmdMyCommand.CommandText += ",DATA_NASCITA=@DATA_NASCITA"
    '        cmdMyCommand.CommandText += ",LUOGO_NASCITA=@LUOGO_NASCITA"
    '        cmdMyCommand.CommandText += ",IDPARENTELA=@IDPARENTELA"
    '        cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyFamiglia.IdFamiglia
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = oMyFamiglia.IdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyFamiglia.sCognome
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyFamiglia.sNome
    '        If oMyFamiglia.tDataNascita <> Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_NASCITA", SqlDbType.DateTime)).Value = oMyFamiglia.tDataNascita
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_NASCITA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LUOGO_NASCITA", SqlDbType.NVarChar)).Value = oMyFamiglia.sLuogoNascita
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPARENTELA", SqlDbType.NVarChar)).Value = oMyFamiglia.sParentela
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestFamiglia.UpdateFamiglia.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function DeleteFamiglia(ByVal WFSessione As OPENUtility.CreateSessione, ByVal nIdDeleteFamiglia As Integer) As Integer
    '    Try
    '        'costruisco la query
    '        cmdMyCommand.CommandText = "DELETE"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATAFAMIGLIA"
    '        cmdMyCommand.CommandText += " WHERE (IDFAMIGLIA=@IDFAMIGLIA)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFAMIGLIA", SqlDbType.Int)).Value = nIdDeleteFamiglia
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <= 0 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestFamiglia.DeleteFamiglia.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    Public Function GetFamigliaResidenti(ByVal myConnectionString As String, ByVal nIdContribuente As Integer) As DataView
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetResidenti"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            Return dtMyDati.DefaultView
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestFamiglia.GetFamigliaResidenti.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    Public Function GetFamiglia(ByVal myConnectionString As String, ByVal nIdGetTestata As Integer) As ObjFamiglia()
        Dim oFamigliaDich As ObjFamiglia
        Dim oListFamigliaDich() As ObjFamiglia = Nothing
        Dim nListFamiglia As Integer = -1
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                sSQL = "SELECT TBLTESTATAFAMIGLIA.*, DESCRIZIONE"
                sSQL += " FROM TBLTESTATAFAMIGLIA"
                sSQL += " LEFT JOIN TBLPARENTELE ON TBLTESTATAFAMIGLIA.IDPARENTELA=TBLPARENTELE.IDPARENTELA"
                sSQL += " WHERE (IDTESTATA=@IDTESTATA)"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESTATA", nIdGetTestata))
                ctx.Dispose()
            End Using
            For Each dtMyRow As DataRowView In dvMyDati
                'incremento l'indice dell'array
                nListFamiglia += 1
                oFamigliaDich = New ObjFamiglia
                oFamigliaDich.IdFamiglia = StringOperation.FormatInt(dtMyRow("id"))
                oFamigliaDich.IdTestata = nIdGetTestata
                oFamigliaDich.sCognome = StringOperation.FormatString(dtMyRow("cognome"))
                oFamigliaDich.sNome = StringOperation.FormatString(dtMyRow("nome"))
                oFamigliaDich.sLuogoNascita = StringOperation.FormatString(dtMyRow("luogo_nascita"))
                oFamigliaDich.tDataNascita = StringOperation.FormatDateTime(dtMyRow("data_nascita"))
                oFamigliaDich.sParentela = StringOperation.FormatString(dtMyRow("idparentela"))
                oFamigliaDich.sDescrParentela = StringOperation.FormatString(dtMyRow("descrizione"))

                'dimensiono l'array
                ReDim Preserve oListFamigliaDich(nListFamiglia)
                'memorizzo i dati nell'array
                oListFamigliaDich(nListFamiglia) = oFamigliaDich
            Next
            Return oListFamigliaDich
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestFamiglia.GetFamiglia.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetFamiglia(ByVal myConnectionString As String, ByVal nIdGetTestata As Integer) As ObjFamiglia()
    '    Dim DrDati As SqlClient.SqlDataReader = Nothing
    '    Dim oFamigliaDich As ObjFamiglia
    '    Dim oListFamigliaDich() As ObjFamiglia = Nothing
    '    Dim nListFamiglia As Integer = -1
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'prelevo i dati di Famiglia
    '        cmdMyCommand.CommandText = "SELECT TBLTESTATAFAMIGLIA.*, DESCRIZIONE"
    '        cmdMyCommand.CommandText += " FROM TBLTESTATAFAMIGLIA"
    '        cmdMyCommand.CommandText += " LEFT JOIN TBLPARENTELE ON TBLTESTATAFAMIGLIA.IDPARENTELA=TBLPARENTELE.IDPARENTELA"
    '        cmdMyCommand.CommandText += " WHERE (IDTESTATA=@IDTESTATA)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdGetTestata
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        DrDati = cmdMyCommand.ExecuteReader
    '        Do While DrDati.Read
    '            'incremento l'indice dell'array
    '            nListFamiglia += 1
    '            oFamigliaDich = New ObjFamiglia
    '            oFamigliaDich.IdFamiglia = StringOperation.Formatint(DrDati("id"))
    '            oFamigliaDich.IdTestata = nIdGetTestata
    '            If Not IsDBNull(DrDati("cognome")) Then
    '                oFamigliaDich.sCognome = StringOperation.FormatString(DrDati("cognome"))
    '            End If
    '            If Not IsDBNull(DrDati("nome")) Then
    '                oFamigliaDich.sNome = StringOperation.FormatString(DrDati("nome"))
    '            End If
    '            If Not IsDBNull(DrDati("luogo_nascita")) Then
    '                oFamigliaDich.sLuogoNascita = StringOperation.FormatString(DrDati("luogo_nascita"))
    '            End If
    '            If Not IsDBNull(DrDati("data_nascita")) Then
    '                oFamigliaDich.tDataNascita = StringOperation.Formatdatetime(DrDati("data_nascita"))
    '            End If
    '            If Not IsDBNull(DrDati("idparentela")) Then
    '                oFamigliaDich.sParentela = StringOperation.FormatString(DrDati("idparentela"))
    '            End If
    '            If Not IsDBNull(DrDati("descrizione")) Then
    '                oFamigliaDich.sDescrParentela = StringOperation.FormatString(DrDati("descrizione"))
    '            End If

    '            'dimensiono l'array
    '            ReDim Preserve oListFamigliaDich(nListFamiglia)
    '            'memorizzo i dati nell'array
    '            oListFamigliaDich(nListFamiglia) = oFamigliaDich
    '        Loop

    '        Return oListFamigliaDich
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestFamiglia.GetFamiglia.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    Public Function SetFamiglia(ByVal myConnectionString As String, ByVal oMyNewFamiglia() As ObjFamiglia, ByVal IdNewDettTestata As Integer) As Integer
        Dim x As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            'costruisco la query
            For x = 0 To oMyNewFamiglia.GetUpperBound(0)
                cmdMyCommand.CommandText = "INSERT INTO TBLTESTATAFAMIGLIA (IDTESTATA,"
                cmdMyCommand.CommandText += " COGNOME, NOME, DATA_NASCITA, LUOGO_NASCITA, IDPARENTELA)"
                cmdMyCommand.CommandText += " VALUES (@IDTESTATA"
                cmdMyCommand.CommandText += ",@COGNOME,@NOME,@DATA_NASCITA,@LUOGO_NASCITA,@IDPARENTELA)"
                'valorizzo i parameters:
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = IdNewDettTestata
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sCognome
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sNome
                If oMyNewFamiglia(x).tDataNascita <> Date.MinValue Then
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_NASCITA", SqlDbType.DateTime)).Value = oMyNewFamiglia(x).tDataNascita
                Else
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_NASCITA", SqlDbType.DateTime)).Value = System.DBNull.Value
                End If
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LUOGO_NASCITA", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sLuogoNascita
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPARENTELA", SqlDbType.NVarChar)).Value = oMyNewFamiglia(x).sParentela
                'eseguo la query
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                If cmdMyCommand.ExecuteNonQuery <> 1 Then
                    Return 0
                End If
            Next
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestFamiglia.SetFamiglia.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
End Class
''' <summary>
''' Definizione oggetto per la ricerca
''' </summary>
Public Class ObjTestataSearch
    Implements IComparable
    Private _Id As Integer = -1
    Private _IdTestata As Integer = -1
    Private _sEnte As String = ""
    Private _IdContribuente As Integer = -1
    Private _tDataDichiarazione As Date = Nothing
    Private _sNDichiarazione As String = ""
    Private _tDataProtocollo As Date = Nothing
    Private _sNProtocollo As String = ""
    Private _nComponenti As Integer = 0
    Private _sIdProvenienza As String = ""
    Private _sNoteDichiarazione As String = ""
    Private _tDataInserimento As Date = Nothing
    Private _tDataVariazione As Date = Nothing
    Private _tDataCessazione As Date = Nothing
    Private _sOperatore As String = ""
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCfPiva As String = ""
    Private _sVia As String = ""
    Private _sCivico As String = ""
    Private _sEsponente As String = ""
    Private _sScala As String = ""
    Private _sInterno As String = ""
    Private _sFoglio As String = ""
    Private _sNumero As String = ""
    Private _sSubalterno As String = ""
    Private _Chiusa As Integer = 0
    '*** 20140923 - GIS ***
    Private _bSel As Boolean = True
    '***  ***
    Private _tDataInizio As Date = Date.MaxValue
    Private _tDataFine As Date = Date.MaxValue
    '*** 201511 - Funzioni Sovracomunali ***
    Private _DescrizioneEnte As String = ""
    '*** ***

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal Value As Integer)
            _Id = Value
        End Set
    End Property
    Public Property IdContribuente() As Integer
        Get
            Return _IdContribuente
        End Get
        Set(ByVal Value As Integer)
            _IdContribuente = Value
        End Set
    End Property
    Public Property IdTestata() As Integer
        Get
            Return _IdTestata
        End Get
        Set(ByVal Value As Integer)
            _IdTestata = Value
        End Set
    End Property
    Public Property sEnte() As String
        Get
            Return _sEnte
        End Get
        Set(ByVal Value As String)
            _sEnte = Value
        End Set
    End Property
    Public Property tDataDichiarazione() As Date
        Get
            Return _tDataDichiarazione
        End Get
        Set(ByVal Value As Date)
            _tDataDichiarazione = Value
        End Set
    End Property
    Public Property sNDichiarazione() As String
        Get
            Return _sNDichiarazione
        End Get
        Set(ByVal Value As String)
            _sNDichiarazione = Value
        End Set
    End Property
    Public Property tDataProtocollo() As Date
        Get
            Return _tDataProtocollo
        End Get
        Set(ByVal Value As Date)
            _tDataProtocollo = Value
        End Set
    End Property
    Public Property sNProtocollo() As String
        Get
            Return _sNProtocollo
        End Get
        Set(ByVal Value As String)
            _sNProtocollo = Value
        End Set
    End Property
    Public Property nComponenti() As Integer
        Get
            Return _nComponenti
        End Get
        Set(ByVal Value As Integer)
            _nComponenti = Value
        End Set
    End Property
    Public Property sIdProvenienza() As String
        Get
            Return _sIdProvenienza
        End Get
        Set(ByVal Value As String)
            _sIdProvenienza = Value
        End Set
    End Property
    Public Property sNoteDichiarazione() As String
        Get
            Return _sNoteDichiarazione
        End Get
        Set(ByVal Value As String)
            _sNoteDichiarazione = Value
        End Set
    End Property
    Public Property tDataInserimento() As Date
        Get
            Return _tDataInserimento
        End Get
        Set(ByVal Value As Date)
            _tDataInserimento = Value
        End Set
    End Property
    Public Property tDataVariazione() As Date
        Get
            Return _tDataVariazione
        End Get
        Set(ByVal Value As Date)
            _tDataVariazione = Value
        End Set
    End Property
    Public Property tDataCessazione() As Date
        Get
            Return _tDataCessazione
        End Get
        Set(ByVal Value As Date)
            _tDataCessazione = Value
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
    Public Property sCognome() As String
        Get
            Return _sCognome
        End Get
        Set(ByVal Value As String)
            _sCognome = Value
        End Set
    End Property
    Public Property sNome() As String
        Get
            Return _sNome
        End Get
        Set(ByVal Value As String)
            _sNome = Value
        End Set
    End Property
    Public Property sCfPiva() As String
        Get
            Return _sCfPiva
        End Get
        Set(ByVal Value As String)
            _sCfPiva = Value
        End Set
    End Property
    Public Property sVia() As String
        Get
            Return _sVia
        End Get
        Set(ByVal Value As String)
            _sVia = Value
        End Set
    End Property
    Public Property sCivico() As String
        Get
            Return _sCivico
        End Get
        Set(ByVal Value As String)
            _sCivico = Value
        End Set
    End Property
    Public Property sEsponente() As String
        Get
            Return _sEsponente
        End Get
        Set(ByVal Value As String)
            _sEsponente = Value
        End Set
    End Property
    Public Property sScala() As String
        Get
            Return _sScala
        End Get
        Set(ByVal Value As String)
            _sScala = Value
        End Set
    End Property
    Public Property sInterno() As String
        Get
            Return _sInterno
        End Get
        Set(ByVal Value As String)
            _sInterno = Value
        End Set
    End Property
    Public Property sFoglio() As String
        Get
            Return _sFoglio
        End Get
        Set(ByVal Value As String)
            _sFoglio = Value
        End Set
    End Property
    Public Property sNumero() As String
        Get
            Return _sNumero
        End Get
        Set(ByVal Value As String)
            _sNumero = Value
        End Set
    End Property
    Public Property sSubalterno() As String
        Get
            Return _sSubalterno
        End Get
        Set(ByVal Value As String)
            _sSubalterno = Value
        End Set
    End Property
    Public Property Chiusa() As Integer
        Get
            Return _Chiusa
        End Get
        Set(ByVal Value As Integer)
            _Chiusa = Value
        End Set
    End Property
    '*** 20140923 - GIS ***
    Public Property bSel() As Boolean
        Get
            Return _bSel
        End Get
        Set(ByVal Value As Boolean)
            _bSel = Value
        End Set
    End Property
    '***  ***
    Public Property tDataInizio() As Date
        Get
            Return _tDataInizio
        End Get
        Set(ByVal Value As Date)
            _tDataInizio = Value
        End Set
    End Property
    Public Property tDataFine() As Date
        Get
            Return _tDataFine
        End Get
        Set(ByVal Value As Date)
            _tDataFine = Value
        End Set
    End Property
    '*** 201511 - Funzioni Sovracomunali ***
    Public Property DescrizioneEnte() As String
        Get
            Return _DescrizioneEnte
        End Get
        Set(ByVal Value As String)
            _DescrizioneEnte = Value
        End Set
    End Property
    '*** ***
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return 0
    End Function
End Class

'Public Class GestScaglione
'    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjScaglione))
'    Private oReplace As New generalClass.generalFunction
'    Private cmdMyCommand As New SqlClient.SqlCommand
'    'Public Function GetScaglioni(ByVal nIdAvviso As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjScaglione()

'    'End Function

'    'Public Function SetScaglione(ByVal oScaglione As ObjScaglione, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
'    '    Dim myIdentity As Integer

'    '    Try
'    '        Select Case DbOperation
'    '            Case Costanti.AZIONE_NEW
'    '                cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLESCAGLIONI (IDPESATURA, IDAVVISO"
'    '                cmdMyCommand.CommandText += " , ANNO, ID_SCAGLIONE, ALIQUOTA, QUANTITA, IMPORTO"
'    '                cmdMyCommand.CommandText += " , DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
'    '                cmdMyCommand.CommandText += " VALUES(@IDPESATURA, @IDAVVISO"
'    '                cmdMyCommand.CommandText += " ,@ANNO,@ID_SCAGLIONE,@ALIQUOTA,@QUANTITA,@IMPORTO"
'    '                cmdMyCommand.CommandText += " ,@DATA_INSERIMENTO, @DATA_VARIAZIONE, @DATA_CESSAZIONE, @OPERATORE)"
'    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
'    '                'valorizzo i parameters:
'    '                cmdMyCommand.Parameters.Clear()
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPESATURA", SqlDbType.Int)).Value = oScaglione.nIdConferimento
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oScaglione.IdAvviso
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oScaglione.sAnno
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_SCAGLIONE", SqlDbType.Int)).Value = oScaglione.nIdScaglione
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ALIQUOTA", SqlDbType.Float)).Value = oScaglione.impTariffa
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@QUANTITA", SqlDbType.Int)).Value = oScaglione.nQuantita
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oScaglione.impScaglione
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oScaglione.tDataInserimento
'    '                If oScaglione.tDataVariazione = Date.MinValue Then
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
'    '                Else
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oScaglione.tDataVariazione
'    '                End If
'    '                If oScaglione.tDataCessazione = Date.MinValue Then
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
'    '                Else
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oScaglione.tDataCessazione
'    '                End If
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oScaglione.sOperatore
'    '                'eseguo la query
'    '                Dim DrReturn As SqlClient.SqlDataReader
'    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
'    '                Do While DrReturn.Read
'    '                    myIdentity = DrReturn(0)
'    '                Loop
'    '                DrReturn.Close()
'    '            Case Costanti.AZIONE_DELETE
'    '                cmdMyCommand.CommandText = "DELETE"
'    '                cmdMyCommand.CommandText += " FROM TBLCARTELLESCAGLIONI"
'    '                cmdMyCommand.CommandText += " WHERE (1=1)"
'    '                'valorizzo i parameters:
'    '                cmdMyCommand.Parameters.Clear()
'    '                If Not oScaglione Is Nothing Then
'    '                    If oScaglione.IdAvviso <> -1 Then
'    '                        cmdMyCommand.CommandText += " AND (IDAVVISO=@IDAVVISO)"
'    '                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oScaglione.IdAvviso
'    '                    Else
'    '                        cmdMyCommand.CommandText += " AND (ID=@ID)"
'    '                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oScaglione.Id
'    '                    End If
'    '                Else
'    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
'    '                    cmdMyCommand.CommandText += "  SELECT ID"
'    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
'    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
'    '                End If
'    '                'eseguo la query
'    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
'    '                    Return 0
'    '                End If
'    '                myIdentity = 1
'    '        End Select
'    '        Return myIdentity
'    '    Catch Err As Exception
'    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestScaglione.SetScaglione.errore: ", Err)
'    '        Log.Debug("Si è verificato un errore in ObjScaglione::SetScaglione::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
'    '        Return 0
'    '    End Try
'    'End Function

'    'Public Function SetScaglione(ByVal myStringConnection As String, ByVal oScaglione As ObjScaglione, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer) As Integer
'    '    Dim myIdentity As Integer

'    '    Try
'    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
'    '        cmdMyCommand.Connection.Open()
'    '        cmdMyCommand.CommandTimeout = 0
'    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
'    '        Select Case DbOperation
'    '            Case Utility.Costanti.AZIONE_NEW
'    '                'cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLESCAGLIONI (IDPESATURA, IDAVVISO"
'    '                'cmdMyCommand.CommandText += " , ANNO, ID_SCAGLIONE, ALIQUOTA, QUANTITA, IMPORTO"
'    '                'cmdMyCommand.CommandText += " , DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
'    '                'cmdMyCommand.CommandText += " VALUES(@IDPESATURA, @IDAVVISO"
'    '                'cmdMyCommand.CommandText += " ,@ANNO,@ID_SCAGLIONE,@ALIQUOTA,@QUANTITA,@IMPORTO"
'    '                'cmdMyCommand.CommandText += " ,@DATA_INSERIMENTO, @DATA_VARIAZIONE, @DATA_CESSAZIONE, @OPERATORE)"
'    '                'cmdMyCommand.CommandText += " SELECT @@IDENTITY"
'    '                cmdMyCommand.CommandText = "prc_TBLCARTELLESCAGLIONI_IU"
'    '                'valorizzo i parameters:
'    '                cmdMyCommand.Parameters.Clear()
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPESATURA", SqlDbType.Int)).Value = oScaglione.nIdConferimento
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oScaglione.IdAvviso
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oScaglione.sAnno
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_SCAGLIONE", SqlDbType.Int)).Value = oScaglione.nIdScaglione
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ALIQUOTA", SqlDbType.Float)).Value = oScaglione.impTariffa
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@QUANTITA", SqlDbType.Int)).Value = oScaglione.nQuantita
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oScaglione.impScaglione
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oScaglione.tDataInserimento
'    '                If oScaglione.tDataVariazione = Date.MinValue Then
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
'    '                Else
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oScaglione.tDataVariazione
'    '                End If
'    '                If oScaglione.tDataCessazione = Date.MinValue Then
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
'    '                Else
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oScaglione.tDataCessazione
'    '                End If
'    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oScaglione.sOperatore
'    '                'eseguo la query
'    '                Log.Debug("SetScaglione::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
'    '                Dim DrReturn As SqlClient.SqlDataReader
'    '                DrReturn = cmdMyCommand.ExecuteReader
'    '                Do While DrReturn.Read
'    '                    myIdentity = DrReturn(0)
'    '                Loop
'    '                DrReturn.Close()
'    '            Case Utility.Costanti.AZIONE_DELETE
'    '                'cmdMyCommand.CommandText = "DELETE"
'    '                'cmdMyCommand.CommandText += " FROM TBLCARTELLESCAGLIONI"
'    '                'cmdMyCommand.CommandText += " WHERE (1=1)"
'    '                If Not oScaglione Is Nothing Then
'    '                    cmdMyCommand.CommandText = "prc_TBLCARTELLESCAGLIONI_D"
'    '                    'valorizzo i parameters:
'    '                    cmdMyCommand.Parameters.Clear()
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oScaglione.IdAvviso
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oScaglione.Id
'    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
'    '                    'If Not oScaglione Is Nothing Then
'    '                    '    If oScaglione.IdAvviso <> -1 Then
'    '                    '        cmdMyCommand.CommandText += " AND (IDAVVISO=@IDAVVISO)"
'    '                    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oScaglione.IdAvviso
'    '                    '    Else
'    '                    '        cmdMyCommand.CommandText += " AND (ID=@ID)"
'    '                    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oScaglione.Id
'    '                    '    End If
'    '                    'Else
'    '                    '    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
'    '                    '    cmdMyCommand.CommandText += "  SELECT ID"
'    '                    '    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
'    '                    '    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
'    '                    '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
'    '                    'End If
'    '                    'eseguo la query
'    '                    Log.Debug("SetScaglione::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
'    '                    If cmdMyCommand.ExecuteNonQuery <> 1 Then
'    '                        Return 0
'    '                    End If
'    '                End If
'    '                myIdentity = 1
'    '        End Select
'    '        Return myIdentity
'    '    Catch Err As Exception
'    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestScaglione.SetScaglione.errore: ", Err)
'    '        Log.Debug("Si è verificato un errore in ObjScaglione::SetScaglione::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
'    '        Return 0
'    '    Finally
'    '        cmdMyCommand.Dispose()
'    '        cmdMyCommand.Connection.Close()
'    '    End Try
'    'End Function
'End Class
''' <summary>
''' Classe per la gestione dell'articolo
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestArticolo
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestArticolo))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    'Public Function GetArticoli(ByVal nIdArticolo As Integer, ByVal nIdAvviso As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjArticolo()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oListArticoli() As ObjArticolo
    '    Dim oMyArticolo As ObjArticolo
    '    Dim nList As Integer = -1
    '    Dim FncRidEse As New GestRidEse
    '    Dim oRicRidEse As New ObjRidEse

    '    Try
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLARTICOLI"
    '        cmdMyCommand.CommandText += " LEFT JOIN TBLCATEGORIE ON TBLARTICOLI.IDCATEGORIA=TBLCATEGORIE.CODICE AND TBLARTICOLI.IDENTE=TBLCATEGORIE.IDENTE"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        If nIdArticolo > 0 Then
    '            cmdMyCommand.CommandText += " AND (ID=@IDARTICOLO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDARTICOLO", SqlDbType.Int)).Value = nIdArticolo
    '        End If
    '        If nIdAvviso > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDAVVISO=@IDAVVISO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '        End If
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            'incremento l'indice
    '            oMyArticolo = New ObjArticolo
    '            oMyArticolo.Id = StringOperation.Formatint(DrDati("id"))
    '            oMyArticolo.IdArticolo = StringOperation.Formatint(DrDati("idruolo"))
    '            oMyArticolo.IdContribuente = StringOperation.Formatint(DrDati("idcontribuente"))
    '            If Not IsDBNull(DrDati("iddettagliotestata")) Then
    '                oMyArticolo.IdDettaglioTestata = StringOperation.Formatint(DrDati("iddettagliotestata"))
    '            End If
    '            oMyArticolo.IdEnte = StringOperation.FormatString(DrDati("idente"))
    '            oMyArticolo.sAnno = StringOperation.FormatString(DrDati("anno"))
    '            oMyArticolo.sVia = StringOperation.FormatString(DrDati("via"))
    '            If Not IsDBNull(DrDati("civico")) Then
    '                oMyArticolo.sCivico = StringOperation.FormatString(DrDati("civico"))
    '            End If
    '            oMyArticolo.sEsponente = StringOperation.FormatString(DrDati("esponente"))
    '            oMyArticolo.sInterno = StringOperation.FormatString(DrDati("interno"))
    '            oMyArticolo.sScala = StringOperation.FormatString(DrDati("scala"))
    '            If Not IsDBNull(DrDati("foglio")) Then
    '                oMyArticolo.sFoglio = StringOperation.FormatString(DrDati("foglio"))
    '            End If
    '            If Not IsDBNull(DrDati("numero")) Then
    '                oMyArticolo.sNumero = StringOperation.FormatString(DrDati("numero"))
    '            End If
    '            If Not IsDBNull(DrDati("subalterno")) Then
    '                oMyArticolo.sSubalterno = StringOperation.FormatString(DrDati("subalterno"))
    '            End If
    '            '***Agenzia Entrate***
    '            If Not IsDBNull(DrDati("sezione")) Then
    '                oMyArticolo.sSezione = StringOperation.FormatString(DrDati("sezione"))
    '            End If
    '            If Not IsDBNull(DrDati("estensione_particella")) Then
    '                oMyArticolo.sEstensioneParticella = StringOperation.FormatString(DrDati("estensione_particella"))
    '            End If
    '            If Not IsDBNull(DrDati("id_tipo_particella")) Then
    '                oMyArticolo.sIdTipoParticella = StringOperation.FormatString(DrDati("id_tipo_particella"))
    '            End If
    '            If Not IsDBNull(DrDati("id_titolo_occupazione")) Then
    '                oMyArticolo.nIdTitoloOccupaz = StringOperation.Formatint(DrDati("id_titolo_occupazione"))
    '            End If
    '            If Not IsDBNull(DrDati("id_natura_occupante")) Then
    '                oMyArticolo.nIdNaturaOccupaz = StringOperation.Formatint(DrDati("id_natura_occupante"))
    '            End If
    '            If Not IsDBNull(DrDati("id_destinazione_uso")) Then
    '                oMyArticolo.nIdDestUso = StringOperation.Formatint(DrDati("id_destinazione_uso"))
    '            End If
    '            If Not IsDBNull(DrDati("id_tipo_unita")) Then
    '                oMyArticolo.sIdTipoUnita = StringOperation.FormatString(DrDati("id_tipo_unita"))
    '            End If
    '            If Not IsDBNull(DrDati("id_assenza_dati_catastali")) Then
    '                oMyArticolo.nIdAssenzaDatiCatastali = StringOperation.Formatint(DrDati("id_assenza_dati_catastali"))
    '            End If
    '            '*********************
    '            If Not IsDBNull(DrDati("idcategoria")) Then
    '                oMyArticolo.sCategoria = StringOperation.FormatString(DrDati("idcategoria"))
    '            End If
    '            If Not IsDBNull(DrDati("descrizione")) Then
    '                oMyArticolo.sDescrCategoria = StringOperation.FormatString(DrDati("descrizione"))
    '            End If

    '            If Not IsDBNull(DrDati("idtariffa")) Then
    '                oMyArticolo.nIdTariffa = StringOperation.Formatint(DrDati("idtariffa"))
    '            End If
    '            If Not IsDBNull(DrDati("importo_tariffa")) Then
    '                oMyArticolo.impTariffa = stringoperation.formatdouble(DrDati("importo_tariffa"))
    '            End If
    '            If Not IsDBNull(DrDati("ncomponenti")) Then
    '                oMyArticolo.nComponenti = StringOperation.Formatint(DrDati("ncomponenti"))
    '            End If
    '            oMyArticolo.nMQ = stringoperation.formatdouble(DrDati("mq"))
    '            oMyArticolo.nBimestri = StringOperation.Formatint(DrDati("bimestri"))
    '            If Not IsDBNull(DrDati("data_inizio")) Then
    '                oMyArticolo.tDataInizio = StringOperation.Formatdatetime(DrDati("data_inizio"))
    '            End If
    '            If Not IsDBNull(DrDati("data_fine")) Then
    '                oMyArticolo.tDataFine = StringOperation.Formatdatetime(DrDati("data_fine"))
    '            End If
    '            If Not IsDBNull(DrDati("istarsugiornaliera")) Then
    '                oMyArticolo.bIsTarsuGiornaliera = stringoperation.formatbool(DrDati("istarsugiornaliera"))
    '            End If
    '            If Not IsDBNull(DrDati("importo")) Then
    '                oMyArticolo.impRuolo = stringoperation.formatdouble(DrDati("importo"))
    '            End If
    '            If Not IsDBNull(DrDati("importo_netto")) Then
    '                oMyArticolo.impNetto = StringOperation.FormatString(DrDati("importo_netto"))
    '            End If
    '            If Not IsDBNull(DrDati("importo_riduzioni")) Then
    '                oMyArticolo.impRiduzione = stringoperation.formatdouble(DrDati("importo_riduzioni"))
    '            End If
    '            If Not IsDBNull(DrDati("importo_detassazioni")) Then
    '                oMyArticolo.impDetassazione = stringoperation.formatdouble(DrDati("importo_detassazioni"))
    '            End If
    '            If Not IsDBNull(DrDati("importo_forzato")) Then
    '                oMyArticolo.bIsImportoForzato = stringoperation.formatbool(DrDati("importo_forzato"))
    '            End If
    '            If Not IsDBNull(DrDati("idflusso_ruolo")) Then
    '                oMyArticolo.nIdFlussoRuolo = StringOperation.Formatint(DrDati("idflusso_ruolo"))
    '            End If
    '            If Not IsDBNull(DrDati("tipo_ruolo")) Then
    '                oMyArticolo.sTipoRuolo = StringOperation.FormatString(DrDati("tipo_ruolo"))
    '            End If
    '            If Not IsDBNull(DrDati("idavviso")) Then
    '                oMyArticolo.IdAvviso = StringOperation.Formatint(DrDati("idavviso"))
    '            End If
    '            If Not IsDBNull(DrDati("informazioni")) Then
    '                oMyArticolo.sNote = StringOperation.FormatString(DrDati("informazioni"))
    '            End If
    '            If Not IsDBNull(DrDati("operatore")) Then
    '                oMyArticolo.sOperatore = StringOperation.FormatString(DrDati("operatore"))
    '            End If
    '            If Not IsDBNull(DrDati("data_inserimento")) Then
    '                oMyArticolo.tDataInserimento = StringOperation.Formatdatetime(DrDati("data_inserimento"))
    '            End If
    '            If Not IsDBNull(DrDati("data_variazione")) Then
    '                oMyArticolo.tDataVariazione = StringOperation.Formatdatetime(DrDati("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrDati("data_cessazione")) Then
    '                oMyArticolo.tDataCessazione = StringOperation.Formatdatetime(DrDati("data_cessazione"))
    '            End If
    '            'prelevo le riduzioni
    '            oRicRidEse.IdEnte = oMyArticolo.IdEnte
    '            oRicRidEse.sAnno = oMyArticolo.sAnno
    '            omyarticolo.oRiduzioni= FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id) 
    '            'prelevo le detassazioni
    '            omyarticolo.oDetassazioni= FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id) 

    '            nList += 1
    '            ReDim Preserve oListArticoli(nList)
    '            oListArticoli(nList) = oMyArticolo
    '        Loop

    '        Return oListArticoli
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetArticoli.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function SetArticoloCompleto(ByVal oMyArticolo As ObjArticolo, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity, x As Integer
    '    Dim FncRidEse As New GestRidEse

    '    Try
    '        'inserisco i dati dell'articolo
    '        myIdentity = SetArticolo(oMyArticolo, -1, Costanti.AZIONE_NEW, WFSessione)
    '        If myIdentity <= 0 Then
    '            Return 0
    '        End If
    '        'memorizzo l'id inserito
    '        oMyArticolo.Id = myIdentity
    '        If Not oMyArticolo.oRiduzioni Is Nothing Then
    '            'inserisco i dati di riduzione
    '            For x = 0 To oMyArticolo.oRiduzioni.GetUpperBound(0)
    '                'devo forzare il riferimento riduzione ad articolo altrimenti inserisce sbagliato
    '                oMyArticolo.oRiduzioni(x).Riferimento = ObjRidEseApplicati.RIF_ARTICOLO
    '            Next
    '            If FncRidEse.SetRidEseApplicate(Costanti.AZIONE_NEW, WFSessione, oMyArticolo.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, oMyArticolo.Id, -1) = 0 Then
    '                Return 0
    '            End If
    '        End If
    '        If Not oMyArticolo.oDetassazioni Is Nothing Then
    '            'inserisco i dati di detassazione
    '            If FncRidEse.SetRidEseApplicate(Costanti.AZIONE_NEW, WFSessione, oMyArticolo.oDetassazioni, ObjRidEse.TIPO_ESENZIONI, oMyArticolo.Id, -1) = 0 Then
    '                Return 0
    '            End If
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestArticolo.SetArticoloCompleto.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetArticolo(ByVal oMyArticolo As ObjArticolo, ByVal nIdFlusso As Integer, ByVal nDBOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        'costruisco la query
    '        Select Case nDBOperation
    '            Case Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLARTICOLI (IDRUOLO, IDCONTRIBUENTE, IDENTE,IDDETTAGLIOTESTATA,IDAVVISO"
    '                cmdMyCommand.CommandText += ", CODVIA, VIA, CIVICO, ESPONENTE, INTERNO, SCALA"
    '                cmdMyCommand.CommandText += ", FOGLIO, NUMERO, SUBALTERNO"
    '                cmdMyCommand.CommandText += ", ANNO, IDCATEGORIA, NCOMPONENTI, MQ"
    '                cmdMyCommand.CommandText += ", BIMESTRI, DATA_INIZIO, DATA_FINE, ISTARSUGIORNALIERA"
    '                cmdMyCommand.CommandText += ", IDTARIFFA, IMPORTO_TARIFFA"
    '                cmdMyCommand.CommandText += ", SEZIONE,ESTENSIONE_PARTICELLA,ID_TIPO_PARTICELLA,ID_TITOLO_OCCUPAZIONE,ID_NATURA_OCCUPANTE,ID_DESTINAZIONE_USO,ID_TIPO_UNITA,ID_ASSENZA_DATI_CATASTALI"
    '                cmdMyCommand.CommandText += ", IMPORTO, IMPORTO_NETTO, IMPORTO_RIDUZIONI, IMPORTO_DETASSAZIONI, IMPORTO_FORZATO"
    '                cmdMyCommand.CommandText += ", INFORMAZIONI, IDFLUSSO_RUOLO, TIPO_RUOLO"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDRUOLO,@IDCONTRIBUENTE,@IDENTE,@IDDETTAGLIOTESTATA,@IDAVVISO"
    '                cmdMyCommand.CommandText += ",@CODVIA,@VIA,@CIVICO,@ESPONENTE,@INTERNO,@SCALA"
    '                cmdMyCommand.CommandText += ",@FOGLIO,@NUMERO,@SUBALTERNO"
    '                cmdMyCommand.CommandText += ",@ANNO,@IDCATEGORIA,@NCOMPONENTI,@MQ"
    '                cmdMyCommand.CommandText += ",@BIMESTRI,@DATA_INIZIO,@DATA_FINE,@ISTARSUGIORNALIERA"
    '                cmdMyCommand.CommandText += ",@IDTARIFFA,@IMPORTO_TARIFFA"
    '                cmdMyCommand.CommandText += ",@SEZIONE,@ESTENSIONE_PARTICELLA,@ID_TIPO_PARTICELLA,@ID_TITOLO_OCCUPAZIONE,@ID_NATURA_OCCUPANTE,@ID_DESTINAZIONE_USO,@ID_TIPO_UNITA,@ID_ASSENZA_DATI_CATASTALI"
    '                cmdMyCommand.CommandText += ",@IMPORTO,@IMPORTO_NETTO,@IMPORTO_RIDUZIONI,@IMPORTO_DETASSAZIONI,@IMPORTO_FORZATO"
    '                cmdMyCommand.CommandText += ",@INFORMAZIONI,@IDFLUSSO_RUOLO,@TIPO_RUOLO"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"

    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRUOLO", SqlDbType.Int)).Value = oMyArticolo.IdArticolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyArticolo.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyArticolo.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = oMyArticolo.IdDettaglioTestata
    '                If oMyArticolo.nCodVia <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = oMyArticolo.nCodVia
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = oMyArticolo.sVia
    '                If oMyArticolo.sCivico <> "-1" Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.NVarChar)).Value = oMyArticolo.sCivico
    '                Else
    '                    cmdMyCommand.CommandText += "'',"
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = oMyArticolo.sEsponente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sInterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SCALA", SqlDbType.NVarChar)).Value = oMyArticolo.sScala
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = oMyArticolo.sFoglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = oMyArticolo.sNumero
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sSubalterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyArticolo.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyArticolo.sCategoria
    '                If oMyArticolo.nComponenti > 0 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = oMyArticolo.nComponenti
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyArticolo.nMQ
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@BIMESTRI", SqlDbType.Int)).Value = oMyArticolo.nBimestri
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO", SqlDbType.DateTime)).Value = oMyArticolo.tDataInizio
    '                If oMyArticolo.tDataFine = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = oMyArticolo.tDataFine
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISTARSUGIORNALIERA", SqlDbType.Bit)).Value = oMyArticolo.bIsTarsuGiornaliera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTARIFFA", SqlDbType.Int)).Value = oMyArticolo.nIdTariffa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_TARIFFA", SqlDbType.Float)).Value = oMyArticolo.impTariffa
    '                '***Agenzia Entrate***
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEZIONE", SqlDbType.NVarChar)).Value = oMyArticolo.sSezione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESTENSIONE_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sEstensioneParticella
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoParticella
    '                If oMyArticolo.nIdTitoloOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = oMyArticolo.nIdTitoloOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdNaturaOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = oMyArticolo.nIdNaturaOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdDestUso <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = oMyArticolo.nIdDestUso
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UNITA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoUnita
    '                If oMyArticolo.nIdAssenzaDatiCatastali <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = oMyArticolo.nIdAssenzaDatiCatastali
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                '*********************
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyArticolo.impRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_NETTO", SqlDbType.Float)).Value = oMyArticolo.impNetto
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_RIDUZIONI", SqlDbType.Float)).Value = oMyArticolo.impRiduzione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_DETASSAZIONI", SqlDbType.Float)).Value = oMyArticolo.impDetassazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_FORZATO", SqlDbType.Bit)).Value = oMyArticolo.bIsImportoForzato
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFORMAZIONI", SqlDbType.NVarChar)).Value = oMyArticolo.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = oMyArticolo.nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUOLO", SqlDbType.NVarChar)).Value = oMyArticolo.sTipoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyArticolo.IdAvviso

    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                If oMyArticolo.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataVariazione
    '                End If
    '                If oMyArticolo.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyArticolo.sOperatore

    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '                'controllo se devo aggiornare l'IDRUOLO
    '                If oMyArticolo.Id = -1 Then
    '                    cmdMyCommand.CommandText = "UPDATE TBLARTICOLI SET IDRUOLO=ID"
    '                    cmdMyCommand.CommandText += " WHERE (ID=" & myIdentity & ")"
    '                    If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '            Case Costanti.AZIONE_UPDATE
    '                cmdMyCommand.CommandText = "UPDATE TBLARTICOLI SET"

    '                cmdMyCommand.CommandText += "IDRUOLO=@IDRUOLO"
    '                cmdMyCommand.CommandText += ", IDCONTRIBUENTE=@IDCONTRIBUENTE"
    '                cmdMyCommand.CommandText += ", IDENTE=@IDENTE"
    '                cmdMyCommand.CommandText += ", IDDETTAGLIOTESTATA=@IDDETTAGLIOTESTATA"
    '                cmdMyCommand.CommandText += ", IDAVVISO=@IDAVVISO"
    '                cmdMyCommand.CommandText += ", CODVIA=@CODVIA"
    '                cmdMyCommand.CommandText += ", VIA=@VIA"
    '                cmdMyCommand.CommandText += ", CIVICO=@CIVICO"
    '                cmdMyCommand.CommandText += ", ESPONENTE=@ESPONENTE"
    '                cmdMyCommand.CommandText += ", INTERNO=@INTERNO"
    '                cmdMyCommand.CommandText += ", SCALA=@SCALA"
    '                cmdMyCommand.CommandText += ", FOGLIO=@FOGLIO"
    '                cmdMyCommand.CommandText += ", NUMERO=@NUMERO"
    '                cmdMyCommand.CommandText += ", SUBALTERNO=@SUBALTERNO"
    '                cmdMyCommand.CommandText += ", ANNO=@ANNO"
    '                cmdMyCommand.CommandText += ", IDCATEGORIA=@IDCATEGORIA"
    '                cmdMyCommand.CommandText += ", NCOMPONENTI=@NCOMPONENTI"
    '                cmdMyCommand.CommandText += ", MQ=@MQ"
    '                cmdMyCommand.CommandText += ", BIMESTRI=@BIMESTRI"
    '                cmdMyCommand.CommandText += ", DATA_INIZIO=@DATA_INIZIO"
    '                cmdMyCommand.CommandText += ", DATA_FINE=@DATA_FINE"
    '                cmdMyCommand.CommandText += ", ISTARSUGIORNALIERA=@ISTARSUGIORNALIERA"
    '                cmdMyCommand.CommandText += ", IDTARIFFA=@IDTARIFFA"
    '                cmdMyCommand.CommandText += ", IMPORTO_TARIFFA=@IMPORTO_TARIFFA"
    '                cmdMyCommand.CommandText += ", SEZIONE=@SEZIONE"
    '                cmdMyCommand.CommandText += ", ESTENSIONE_PARTICELLA=@ESTENSIONE_PARTICELLA"
    '                cmdMyCommand.CommandText += ", ID_TIPO_PARTICELLA=@ID_TIPO_PARTICELLA"
    '                cmdMyCommand.CommandText += ", ID_TITOLO_OCCUPAZIONE=@ID_TITOLO_OCCUPAZIONE"
    '                cmdMyCommand.CommandText += ", ID_NATURA_OCCUPANTE=@ID_NATURA_OCCUPANTE"
    '                cmdMyCommand.CommandText += ", ID_DESTINAZIONE_USO=@ID_DESTINAZIONE_USO"
    '                cmdMyCommand.CommandText += ", ID_TIPO_UNITA=@ID_TIPO_UNITA"
    '                cmdMyCommand.CommandText += ", ID_ASSENZA_DATI_CATASTALI=@ID_ASSENZA_DATI_CATASTALI"
    '                cmdMyCommand.CommandText += ", IMPORTO=@IMPORTO"
    '                cmdMyCommand.CommandText += ", IMPORTO_NETTO=@IMPORTO_NETTO"
    '                cmdMyCommand.CommandText += ", IMPORTO_RIDUZIONI=@IMPORTO_RIDUZIONI"
    '                cmdMyCommand.CommandText += ", IMPORTO_DETASSAZIONI=@IMPORTO_DETASSAZIONI"
    '                cmdMyCommand.CommandText += ", IMPORTO_FORZATO=@IMPORTO_FORZATO"
    '                cmdMyCommand.CommandText += ", INFORMAZIONI=@INFORMAZIONI"
    '                cmdMyCommand.CommandText += ", IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO"
    '                cmdMyCommand.CommandText += ", TIPO_RUOLO=@TIPO_RUOLO"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO=@DATA_INSERIMENTO"
    '                cmdMyCommand.CommandText += ", DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += ", DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '                cmdMyCommand.CommandText += ", OPERATORE=@OPERATORE"
    '                cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyArticolo.nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRUOLO", SqlDbType.Int)).Value = oMyArticolo.IdArticolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyArticolo.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyArticolo.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = oMyArticolo.IdDettaglioTestata
    '                If oMyArticolo.nCodVia > 0 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = oMyArticolo.nCodVia
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = oMyArticolo.sVia
    '                If oMyArticolo.sCivico <> "-1" Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.NVarChar)).Value = oMyArticolo.sCivico
    '                Else
    '                    cmdMyCommand.CommandText += "'',"
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = oMyArticolo.sEsponente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sInterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SCALA", SqlDbType.NVarChar)).Value = oMyArticolo.sScala
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = oMyArticolo.sFoglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = oMyArticolo.sNumero
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sSubalterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyArticolo.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyArticolo.sCategoria
    '                If oMyArticolo.nComponenti > 0 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = oMyArticolo.nComponenti
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyArticolo.nMQ
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@BIMESTRI", SqlDbType.Int)).Value = oMyArticolo.nBimestri
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO", SqlDbType.DateTime)).Value = oMyArticolo.tDataInizio
    '                If oMyArticolo.tDataFine = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = oMyArticolo.tDataFine
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISTARSUGIORNALIERA", SqlDbType.Bit)).Value = oMyArticolo.bIsTarsuGiornaliera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTARIFFA", SqlDbType.Int)).Value = oMyArticolo.nIdTariffa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_TARIFFA", SqlDbType.Float)).Value = oMyArticolo.impTariffa
    '                '***Agenzia Entrate***
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEZIONE", SqlDbType.NVarChar)).Value = oMyArticolo.sSezione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESTENSIONE_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sEstensioneParticella
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoParticella
    '                If oMyArticolo.nIdTitoloOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = oMyArticolo.nIdTitoloOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdNaturaOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = oMyArticolo.nIdNaturaOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdDestUso <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = oMyArticolo.nIdDestUso
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UNITA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoUnita
    '                If oMyArticolo.nIdAssenzaDatiCatastali <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = oMyArticolo.nIdAssenzaDatiCatastali
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                '*********************
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyArticolo.impRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_NETTO", SqlDbType.Float)).Value = oMyArticolo.impNetto
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_RIDUZIONI", SqlDbType.Float)).Value = oMyArticolo.impRiduzione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_DETASSAZIONI", SqlDbType.Float)).Value = oMyArticolo.impDetassazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_FORZATO", SqlDbType.Bit)).Value = oMyArticolo.bIsImportoForzato
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFORMAZIONI", SqlDbType.NVarChar)).Value = oMyArticolo.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = oMyArticolo.nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUOLO", SqlDbType.NVarChar)).Value = oMyArticolo.sTipoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyArticolo.IdAvviso

    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyArticolo.tDataInserimento
    '                If oMyArticolo.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataVariazione
    '                End If
    '                If oMyArticolo.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyArticolo.sOperatore
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oMyArticolo.Id
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "UPDATE TBLARTICOLI"
    '                cmdMyCommand.CommandText += " SET DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = Now
    '                If Not oMyArticolo Is Nothing Then
    '                    cmdMyCommand.CommandText += " AND (ID=@ID)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyArticolo.Id
    '                End If
    '                If nIdFlusso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
    '                    cmdMyCommand.CommandText += "  SELECT ID "
    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestArticolo.SetArticolo.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetArticolo(ByVal myStringConnection As String, ByVal oMyArticolo As ObjArticolo, ByVal nIdFlusso As Integer, ByVal nDBOperation As Integer) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'costruisco la query
    '        Select Case nDBOperation
    '            Case Utility.Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLARTICOLI (IDRUOLO, IDCONTRIBUENTE, IDENTE,IDDETTAGLIOTESTATA,IDAVVISO"
    '                cmdMyCommand.CommandText += ", CODVIA, VIA, CIVICO, ESPONENTE, INTERNO, SCALA"
    '                cmdMyCommand.CommandText += ", FOGLIO, NUMERO, SUBALTERNO"
    '                cmdMyCommand.CommandText += ", ANNO, IDCATEGORIA, NCOMPONENTI, MQ"
    '                cmdMyCommand.CommandText += ", BIMESTRI, DATA_INIZIO, DATA_FINE, ISTARSUGIORNALIERA"
    '                cmdMyCommand.CommandText += ", IDTARIFFA, IMPORTO_TARIFFA"
    '                cmdMyCommand.CommandText += ", SEZIONE,ESTENSIONE_PARTICELLA,ID_TIPO_PARTICELLA,ID_TITOLO_OCCUPAZIONE,ID_NATURA_OCCUPANTE,ID_DESTINAZIONE_USO,ID_TIPO_UNITA,ID_ASSENZA_DATI_CATASTALI"
    '                cmdMyCommand.CommandText += ", IMPORTO, IMPORTO_NETTO, IMPORTO_RIDUZIONI, IMPORTO_DETASSAZIONI, IMPORTO_FORZATO"
    '                cmdMyCommand.CommandText += ", INFORMAZIONI, IDFLUSSO_RUOLO, TIPO_RUOLO"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDRUOLO,@IDCONTRIBUENTE,@IDENTE,@IDDETTAGLIOTESTATA,@IDAVVISO"
    '                cmdMyCommand.CommandText += ",@CODVIA,@VIA,@CIVICO,@ESPONENTE,@INTERNO,@SCALA"
    '                cmdMyCommand.CommandText += ",@FOGLIO,@NUMERO,@SUBALTERNO"
    '                cmdMyCommand.CommandText += ",@ANNO,@IDCATEGORIA,@NCOMPONENTI,@MQ"
    '                cmdMyCommand.CommandText += ",@BIMESTRI,@DATA_INIZIO,@DATA_FINE,@ISTARSUGIORNALIERA"
    '                cmdMyCommand.CommandText += ",@IDTARIFFA,@IMPORTO_TARIFFA"
    '                cmdMyCommand.CommandText += ",@SEZIONE,@ESTENSIONE_PARTICELLA,@ID_TIPO_PARTICELLA,@ID_TITOLO_OCCUPAZIONE,@ID_NATURA_OCCUPANTE,@ID_DESTINAZIONE_USO,@ID_TIPO_UNITA,@ID_ASSENZA_DATI_CATASTALI"
    '                cmdMyCommand.CommandText += ",@IMPORTO,@IMPORTO_NETTO,@IMPORTO_RIDUZIONI,@IMPORTO_DETASSAZIONI,@IMPORTO_FORZATO"
    '                cmdMyCommand.CommandText += ",@INFORMAZIONI,@IDFLUSSO_RUOLO,@TIPO_RUOLO"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"

    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRUOLO", SqlDbType.Int)).Value = oMyArticolo.IdArticolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyArticolo.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyArticolo.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = oMyArticolo.IdDettaglioTestata
    '                If oMyArticolo.nCodVia <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = oMyArticolo.nCodVia
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = oMyArticolo.sVia
    '                If oMyArticolo.sCivico <> "-1" Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.NVarChar)).Value = oMyArticolo.sCivico
    '                Else
    '                    cmdMyCommand.CommandText += "'',"
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = oMyArticolo.sEsponente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sInterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SCALA", SqlDbType.NVarChar)).Value = oMyArticolo.sScala
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = oMyArticolo.sFoglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = oMyArticolo.sNumero
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sSubalterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyArticolo.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyArticolo.sCategoria
    '                If oMyArticolo.nComponenti > 0 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = oMyArticolo.nComponenti
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyArticolo.nMQ
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@BIMESTRI", SqlDbType.Int)).Value = oMyArticolo.nBimestri
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO", SqlDbType.DateTime)).Value = oMyArticolo.tDataInizio
    '                If oMyArticolo.tDataFine = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = oMyArticolo.tDataFine
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISTARSUGIORNALIERA", SqlDbType.Bit)).Value = oMyArticolo.bIsTarsuGiornaliera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTARIFFA", SqlDbType.Int)).Value = oMyArticolo.nIdTariffa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_TARIFFA", SqlDbType.Float)).Value = oMyArticolo.impTariffa
    '                '***Agenzia Entrate***
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEZIONE", SqlDbType.NVarChar)).Value = oMyArticolo.sSezione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESTENSIONE_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sEstensioneParticella
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoParticella
    '                If oMyArticolo.nIdTitoloOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = oMyArticolo.nIdTitoloOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdNaturaOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = oMyArticolo.nIdNaturaOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdDestUso <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = oMyArticolo.nIdDestUso
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UNITA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoUnita
    '                If oMyArticolo.nIdAssenzaDatiCatastali <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = oMyArticolo.nIdAssenzaDatiCatastali
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                '*********************
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyArticolo.impRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_NETTO", SqlDbType.Float)).Value = oMyArticolo.impNetto
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_RIDUZIONI", SqlDbType.Float)).Value = oMyArticolo.impRiduzione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_DETASSAZIONI", SqlDbType.Float)).Value = oMyArticolo.impDetassazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_FORZATO", SqlDbType.Bit)).Value = oMyArticolo.bIsImportoForzato
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFORMAZIONI", SqlDbType.NVarChar)).Value = oMyArticolo.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = oMyArticolo.nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUOLO", SqlDbType.NVarChar)).Value = oMyArticolo.sTipoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyArticolo.IdAvviso

    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                If oMyArticolo.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataVariazione
    '                End If
    '                If oMyArticolo.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyArticolo.sOperatore
    '                Log.Debug("SetArticolo::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = cmdMyCommand.ExecuteReader
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '                'controllo se devo aggiornare l'IDRUOLO
    '                If oMyArticolo.Id = -1 Then
    '                    cmdMyCommand.CommandText = "UPDATE TBLARTICOLI SET IDRUOLO=ID"
    '                    cmdMyCommand.CommandText += " WHERE (ID=" & myIdentity & ")"
    '                    If cmdMyCommand.ExecuteNonQuery < 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '            Case Utility.Costanti.AZIONE_UPDATE
    '                cmdMyCommand.CommandText = "UPDATE TBLARTICOLI SET"

    '                cmdMyCommand.CommandText += "IDRUOLO=@IDRUOLO"
    '                cmdMyCommand.CommandText += ", IDCONTRIBUENTE=@IDCONTRIBUENTE"
    '                cmdMyCommand.CommandText += ", IDENTE=@IDENTE"
    '                cmdMyCommand.CommandText += ", IDDETTAGLIOTESTATA=@IDDETTAGLIOTESTATA"
    '                cmdMyCommand.CommandText += ", IDAVVISO=@IDAVVISO"
    '                cmdMyCommand.CommandText += ", CODVIA=@CODVIA"
    '                cmdMyCommand.CommandText += ", VIA=@VIA"
    '                cmdMyCommand.CommandText += ", CIVICO=@CIVICO"
    '                cmdMyCommand.CommandText += ", ESPONENTE=@ESPONENTE"
    '                cmdMyCommand.CommandText += ", INTERNO=@INTERNO"
    '                cmdMyCommand.CommandText += ", SCALA=@SCALA"
    '                cmdMyCommand.CommandText += ", FOGLIO=@FOGLIO"
    '                cmdMyCommand.CommandText += ", NUMERO=@NUMERO"
    '                cmdMyCommand.CommandText += ", SUBALTERNO=@SUBALTERNO"
    '                cmdMyCommand.CommandText += ", ANNO=@ANNO"
    '                cmdMyCommand.CommandText += ", IDCATEGORIA=@IDCATEGORIA"
    '                cmdMyCommand.CommandText += ", NCOMPONENTI=@NCOMPONENTI"
    '                cmdMyCommand.CommandText += ", MQ=@MQ"
    '                cmdMyCommand.CommandText += ", BIMESTRI=@BIMESTRI"
    '                cmdMyCommand.CommandText += ", DATA_INIZIO=@DATA_INIZIO"
    '                cmdMyCommand.CommandText += ", DATA_FINE=@DATA_FINE"
    '                cmdMyCommand.CommandText += ", ISTARSUGIORNALIERA=@ISTARSUGIORNALIERA"
    '                cmdMyCommand.CommandText += ", IDTARIFFA=@IDTARIFFA"
    '                cmdMyCommand.CommandText += ", IMPORTO_TARIFFA=@IMPORTO_TARIFFA"
    '                cmdMyCommand.CommandText += ", SEZIONE=@SEZIONE"
    '                cmdMyCommand.CommandText += ", ESTENSIONE_PARTICELLA=@ESTENSIONE_PARTICELLA"
    '                cmdMyCommand.CommandText += ", ID_TIPO_PARTICELLA=@ID_TIPO_PARTICELLA"
    '                cmdMyCommand.CommandText += ", ID_TITOLO_OCCUPAZIONE=@ID_TITOLO_OCCUPAZIONE"
    '                cmdMyCommand.CommandText += ", ID_NATURA_OCCUPANTE=@ID_NATURA_OCCUPANTE"
    '                cmdMyCommand.CommandText += ", ID_DESTINAZIONE_USO=@ID_DESTINAZIONE_USO"
    '                cmdMyCommand.CommandText += ", ID_TIPO_UNITA=@ID_TIPO_UNITA"
    '                cmdMyCommand.CommandText += ", ID_ASSENZA_DATI_CATASTALI=@ID_ASSENZA_DATI_CATASTALI"
    '                cmdMyCommand.CommandText += ", IMPORTO=@IMPORTO"
    '                cmdMyCommand.CommandText += ", IMPORTO_NETTO=@IMPORTO_NETTO"
    '                cmdMyCommand.CommandText += ", IMPORTO_RIDUZIONI=@IMPORTO_RIDUZIONI"
    '                cmdMyCommand.CommandText += ", IMPORTO_DETASSAZIONI=@IMPORTO_DETASSAZIONI"
    '                cmdMyCommand.CommandText += ", IMPORTO_FORZATO=@IMPORTO_FORZATO"
    '                cmdMyCommand.CommandText += ", INFORMAZIONI=@INFORMAZIONI"
    '                cmdMyCommand.CommandText += ", IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO"
    '                cmdMyCommand.CommandText += ", TIPO_RUOLO=@TIPO_RUOLO"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO=@DATA_INSERIMENTO"
    '                cmdMyCommand.CommandText += ", DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += ", DATA_CESSAZIONE=@DATA_CESSAZIONE"
    '                cmdMyCommand.CommandText += ", OPERATORE=@OPERATORE"
    '                cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyArticolo.nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRUOLO", SqlDbType.Int)).Value = oMyArticolo.IdArticolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyArticolo.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyArticolo.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOTESTATA", SqlDbType.Int)).Value = oMyArticolo.IdDettaglioTestata
    '                If oMyArticolo.nCodVia > 0 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = oMyArticolo.nCodVia
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVIA", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = oMyArticolo.sVia
    '                If oMyArticolo.sCivico <> "-1" Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.NVarChar)).Value = oMyArticolo.sCivico
    '                Else
    '                    cmdMyCommand.CommandText += "'',"
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = oMyArticolo.sEsponente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sInterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SCALA", SqlDbType.NVarChar)).Value = oMyArticolo.sScala
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.NVarChar)).Value = oMyArticolo.sFoglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = oMyArticolo.sNumero
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUBALTERNO", SqlDbType.NVarChar)).Value = oMyArticolo.sSubalterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyArticolo.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyArticolo.sCategoria
    '                If oMyArticolo.nComponenti > 0 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = oMyArticolo.nComponenti
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyArticolo.nMQ
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@BIMESTRI", SqlDbType.Int)).Value = oMyArticolo.nBimestri
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INIZIO", SqlDbType.DateTime)).Value = oMyArticolo.tDataInizio
    '                If oMyArticolo.tDataFine = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_FINE", SqlDbType.DateTime)).Value = oMyArticolo.tDataFine
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISTARSUGIORNALIERA", SqlDbType.Bit)).Value = oMyArticolo.bIsTarsuGiornaliera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTARIFFA", SqlDbType.Int)).Value = oMyArticolo.nIdTariffa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_TARIFFA", SqlDbType.Float)).Value = oMyArticolo.impTariffa
    '                '***Agenzia Entrate***
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEZIONE", SqlDbType.NVarChar)).Value = oMyArticolo.sSezione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESTENSIONE_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sEstensioneParticella
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_PARTICELLA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoParticella
    '                If oMyArticolo.nIdTitoloOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = oMyArticolo.nIdTitoloOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TITOLO_OCCUPAZIONE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdNaturaOccupaz <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = oMyArticolo.nIdNaturaOccupaz
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_NATURA_OCCUPANTE", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                If oMyArticolo.nIdDestUso <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = oMyArticolo.nIdDestUso
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_DESTINAZIONE_USO", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_UNITA", SqlDbType.NVarChar)).Value = oMyArticolo.sIdTipoUnita
    '                If oMyArticolo.nIdAssenzaDatiCatastali <> -1 Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = oMyArticolo.nIdAssenzaDatiCatastali
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_ASSENZA_DATI_CATASTALI", SqlDbType.Int)).Value = System.DBNull.Value
    '                End If
    '                '*********************
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyArticolo.impRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_NETTO", SqlDbType.Float)).Value = oMyArticolo.impNetto
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_RIDUZIONI", SqlDbType.Float)).Value = oMyArticolo.impRiduzione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_DETASSAZIONI", SqlDbType.Float)).Value = oMyArticolo.impDetassazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_FORZATO", SqlDbType.Bit)).Value = oMyArticolo.bIsImportoForzato
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFORMAZIONI", SqlDbType.NVarChar)).Value = oMyArticolo.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = oMyArticolo.nIdFlussoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUOLO", SqlDbType.NVarChar)).Value = oMyArticolo.sTipoRuolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyArticolo.IdAvviso

    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyArticolo.tDataInserimento
    '                If oMyArticolo.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataVariazione
    '                End If
    '                If oMyArticolo.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyArticolo.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyArticolo.sOperatore
    '                'eseguo la query
    '                Log.Debug("SetArticolo::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oMyArticolo.Id
    '            Case Utility.Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "UPDATE TBLARTICOLI"
    '                cmdMyCommand.CommandText += " SET DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = Now
    '                If Not oMyArticolo Is Nothing Then
    '                    cmdMyCommand.CommandText += " AND (ID=@ID)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyArticolo.Id
    '                End If
    '                If nIdFlusso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
    '                    cmdMyCommand.CommandText += "  SELECT ID "
    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                'eseguo la query
    '                Log.Debug("SetArticolo::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                If cmdMyCommand.ExecuteNonQuery < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestArticolo.SetArticolo.errore: ", Err)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="nIdArticolo"></param>
    ''' <param name="nIdAvviso"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <param name="IsFromAvviso"></param>
    ''' <returns></returns>
    Public Function GetArticoli(myConnectionString As String, ByVal nIdArticolo As Integer, ByVal nIdAvviso As Integer, nIdContribuente As Integer, ByVal IsFromAvviso As Boolean) As ObjArticolo()
        Dim oListArticoli As New ArrayList
        Dim oMyArticolo As ObjArticolo
        Dim FncRidEse As New GestRidEse
        Dim oRicRidEse As New ObjRidEse
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetArticolo", "IDARTICOLO", "IDAVVISO", "IDCONTRIBUENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDARTICOLO", nIdArticolo) _
                            , ctx.GetParam("IDAVVISO", nIdAvviso) _
                            , ctx.GetParam("IDCONTRIBUENTE", nIdContribuente)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetArticoli.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyArticolo = New ObjArticolo
                    oMyArticolo.Id = StringOperation.FormatInt(myRow("id"))
                    oMyArticolo.IdArticolo = StringOperation.FormatInt(myRow("idruolo"))
                    oMyArticolo.IdContribuente = StringOperation.FormatInt(myRow("idcontribuente"))
                    oMyArticolo.IdDettaglioTestata = StringOperation.FormatInt(myRow("iddettagliotestata"))
                    oMyArticolo.TipoPartita = StringOperation.FormatString(myRow("tipopartita"))
                    oMyArticolo.IdEnte = StringOperation.FormatString(myRow("idente"))
                    oMyArticolo.sAnno = StringOperation.FormatString(myRow("anno"))
                    oMyArticolo.sVia = StringOperation.FormatString(myRow("via"))
                    oMyArticolo.sCivico = StringOperation.FormatString(myRow("civico"))
                    oMyArticolo.sEsponente = StringOperation.FormatString(myRow("esponente"))
                    oMyArticolo.sInterno = StringOperation.FormatString(myRow("interno"))
                    oMyArticolo.sScala = StringOperation.FormatString(myRow("scala"))
                    oMyArticolo.sFoglio = StringOperation.FormatString(myRow("foglio"))
                    oMyArticolo.sNumero = StringOperation.FormatString(myRow("numero"))
                    oMyArticolo.sSubalterno = StringOperation.FormatString(myRow("subalterno"))
                    '***Agenzia Entrate***
                    oMyArticolo.sSezione = StringOperation.FormatString(myRow("sezione"))
                    oMyArticolo.sEstensioneParticella = StringOperation.FormatString(myRow("estensione_particella"))
                    oMyArticolo.sIdTipoParticella = StringOperation.FormatString(myRow("id_tipo_particella"))
                    oMyArticolo.nIdTitoloOccupaz = StringOperation.FormatInt(myRow("id_titolo_occupazione"))
                    oMyArticolo.nIdNaturaOccupaz = StringOperation.FormatInt(myRow("id_natura_occupante"))
                    oMyArticolo.nIdDestUso = StringOperation.FormatInt(myRow("id_destinazione_uso"))
                    oMyArticolo.sIdTipoUnita = StringOperation.FormatString(myRow("id_tipo_unita"))
                    oMyArticolo.nIdAssenzaDatiCatastali = StringOperation.FormatInt(myRow("id_assenza_dati_catastali"))
                    '*********************
                    oMyArticolo.sCategoria = StringOperation.FormatString(myRow("idcategoria"))
                    oMyArticolo.sDescrCategoria = StringOperation.FormatString(myRow("descrizione"))
                    oMyArticolo.nIdTariffa = StringOperation.FormatInt(myRow("idtariffa"))
                    oMyArticolo.impTariffa = StringOperation.FormatDouble(myRow("importo_tariffa"))
                    oMyArticolo.nComponenti = StringOperation.FormatInt(myRow("ncomponenti"))
                    oMyArticolo.nComponentiPV = StringOperation.FormatInt(myRow("ncomponenti_pv"))
                    oMyArticolo.nMQ = StringOperation.FormatDouble(myRow("mq"))
                    oMyArticolo.nBimestri = StringOperation.FormatInt(myRow("bimestri"))
                    oMyArticolo.tDataInizio = StringOperation.FormatDateTime(myRow("data_inizio"))
                    oMyArticolo.tDataFine = StringOperation.FormatDateTime(myRow("data_fine"))
                    oMyArticolo.bIsTarsuGiornaliera = StringOperation.FormatBool(myRow("istarsugiornaliera"))
                    oMyArticolo.bForzaPV = StringOperation.FormatBool(myRow("forza_calcolapv"))
                    oMyArticolo.impRuolo = StringOperation.FormatDouble(myRow("importo"))
                    oMyArticolo.impNetto = StringOperation.FormatString(myRow("importo_netto"))
                    oMyArticolo.impRiduzione = StringOperation.FormatDouble(myRow("importo_riduzioni"))
                    oMyArticolo.impDetassazione = StringOperation.FormatDouble(myRow("importo_detassazioni"))
                    oMyArticolo.bIsImportoForzato = StringOperation.FormatBool(myRow("importo_forzato"))
                    oMyArticolo.nIdFlussoRuolo = StringOperation.FormatInt(myRow("idflusso_ruolo"))
                    oMyArticolo.sTipoRuolo = StringOperation.FormatString(myRow("tipo_ruolo"))
                    oMyArticolo.IdAvviso = StringOperation.FormatInt(myRow("idavviso"))
                    oMyArticolo.sNote = StringOperation.FormatString(myRow("informazioni"))
                    oMyArticolo.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    oMyArticolo.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                    oMyArticolo.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                    oMyArticolo.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))
                    'BD 09/07/2021
                    oMyArticolo.ImportoFissoRid = StringOperation.FormatDouble(myRow("importo_fissorid"))
                    'BD 09/07/2021
                    'prelevo le riduzioni
                    oRicRidEse.IdEnte = oMyArticolo.IdEnte
                    oRicRidEse.sAnno = oMyArticolo.sAnno
                    If IsFromAvviso = True Then
                        oMyArticolo.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, "")
                        'prelevo le detassazioni
                        oMyArticolo.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, "")
                    Else
                        oMyArticolo.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, oMyArticolo.TipoPartita)
                        'prelevo le detassazioni
                        oMyArticolo.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, oMyArticolo.TipoPartita)
                    End If
                    '*** 20141211 - legami PF-PV ***
                    If Not IsDBNull(myRow("idoggetto")) Then
                        oMyArticolo.IdOggetto = StringOperation.FormatInt(myRow("idoggetto"))
                    End If
                    If oMyArticolo.TipoPartita = ObjArticolo.PARTEVARIABILE Then
                        oMyArticolo.ListPFvsPV = New ClsDichiarazione().LoadLegamiPFPV(myConnectionString, oMyArticolo.Id, -1, oMyArticolo.sAnno, 100, "D")
                    End If
                    '*** ***


                    oListArticoli.Add(oMyArticolo)
                Next
            End Using

            Return CType(oListArticoli.ToArray(GetType(ObjArticolo)), ObjArticolo())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetArticoli.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function GetArticoli(myConnectionString As String, ByVal nIdArticolo As Integer, ByVal nIdAvviso As Integer, nIdContribuente As Integer, ByVal IsFromAvviso As Boolean, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjArticolo()
    '    Dim oListArticoli As New ArrayList
    '    Dim oMyArticolo As ObjArticolo
    '    Dim FncRidEse As New GestRidEse
    '    Dim oRicRidEse As New ObjRidEse
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IDARTICOLO", nIdArticolo)
    '        cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", nIdAvviso)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", nIdContribuente)
    '        cmdMyCommand.CommandText = "prc_GetArticolo"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyArticolo = New ObjArticolo
    '            oMyArticolo.Id = StringOperation.FormatInt(dtMyRow("id"))
    '            oMyArticolo.IdArticolo = StringOperation.FormatInt(dtMyRow("idruolo"))
    '            oMyArticolo.IdContribuente = StringOperation.FormatInt(dtMyRow("idcontribuente"))
    '            If Not IsDBNull(dtMyRow("iddettagliotestata")) Then
    '                oMyArticolo.IdDettaglioTestata = StringOperation.FormatInt(dtMyRow("iddettagliotestata"))
    '            End If
    '            oMyArticolo.TipoPartita = StringOperation.FormatString(dtMyRow("tipopartita"))
    '            oMyArticolo.IdEnte = StringOperation.FormatString(dtMyRow("idente"))
    '            oMyArticolo.sAnno = StringOperation.FormatString(dtMyRow("anno"))
    '            oMyArticolo.sVia = StringOperation.FormatString(dtMyRow("via"))
    '            If Not IsDBNull(dtMyRow("civico")) Then
    '                oMyArticolo.sCivico = StringOperation.FormatString(dtMyRow("civico"))
    '            End If
    '            If Not IsDBNull(dtMyRow("esponente")) Then
    '                oMyArticolo.sEsponente = StringOperation.FormatString(dtMyRow("esponente"))
    '            End If
    '            If Not IsDBNull(dtMyRow("interno")) Then
    '                oMyArticolo.sInterno = StringOperation.FormatString(dtMyRow("interno"))
    '            End If
    '            If Not IsDBNull(dtMyRow("scala")) Then
    '                oMyArticolo.sScala = StringOperation.FormatString(dtMyRow("scala"))
    '            End If
    '            If Not IsDBNull(dtMyRow("foglio")) Then
    '                oMyArticolo.sFoglio = StringOperation.FormatString(dtMyRow("foglio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("numero")) Then
    '                oMyArticolo.sNumero = StringOperation.FormatString(dtMyRow("numero"))
    '            End If
    '            If Not IsDBNull(dtMyRow("subalterno")) Then
    '                oMyArticolo.sSubalterno = StringOperation.FormatString(dtMyRow("subalterno"))
    '            End If
    '            '***Agenzia Entrate***
    '            If Not IsDBNull(dtMyRow("sezione")) Then
    '                oMyArticolo.sSezione = StringOperation.FormatString(dtMyRow("sezione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("estensione_particella")) Then
    '                oMyArticolo.sEstensioneParticella = StringOperation.FormatString(dtMyRow("estensione_particella"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_tipo_particella")) Then
    '                oMyArticolo.sIdTipoParticella = StringOperation.FormatString(dtMyRow("id_tipo_particella"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_titolo_occupazione")) Then
    '                oMyArticolo.nIdTitoloOccupaz = StringOperation.FormatInt(dtMyRow("id_titolo_occupazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_natura_occupante")) Then
    '                oMyArticolo.nIdNaturaOccupaz = StringOperation.FormatInt(dtMyRow("id_natura_occupante"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_destinazione_uso")) Then
    '                oMyArticolo.nIdDestUso = StringOperation.FormatInt(dtMyRow("id_destinazione_uso"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_tipo_unita")) Then
    '                oMyArticolo.sIdTipoUnita = StringOperation.FormatString(dtMyRow("id_tipo_unita"))
    '            End If
    '            If Not IsDBNull(dtMyRow("id_assenza_dati_catastali")) Then
    '                oMyArticolo.nIdAssenzaDatiCatastali = StringOperation.FormatInt(dtMyRow("id_assenza_dati_catastali"))
    '            End If
    '            '*********************
    '            If Not IsDBNull(dtMyRow("idcategoria")) Then
    '                oMyArticolo.sCategoria = StringOperation.FormatString(dtMyRow("idcategoria"))
    '            End If
    '            If Not IsDBNull(dtMyRow("descrizione")) Then
    '                oMyArticolo.sDescrCategoria = StringOperation.FormatString(dtMyRow("descrizione"))
    '            End If

    '            If Not IsDBNull(dtMyRow("idtariffa")) Then
    '                oMyArticolo.nIdTariffa = StringOperation.FormatInt(dtMyRow("idtariffa"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo_tariffa")) Then
    '                oMyArticolo.impTariffa = StringOperation.FormatDouble(dtMyRow("importo_tariffa"))
    '            End If
    '            If Not IsDBNull(dtMyRow("ncomponenti")) Then
    '                oMyArticolo.nComponenti = StringOperation.FormatInt(dtMyRow("ncomponenti"))
    '            End If
    '            If Not IsDBNull(dtMyRow("ncomponenti_pv")) Then
    '                oMyArticolo.nComponentiPV = StringOperation.FormatInt(dtMyRow("ncomponenti_pv"))
    '            End If
    '            oMyArticolo.nMQ = StringOperation.FormatDouble(dtMyRow("mq"))
    '            oMyArticolo.nBimestri = StringOperation.FormatInt(dtMyRow("bimestri"))
    '            If Not IsDBNull(dtMyRow("data_inizio")) Then
    '                oMyArticolo.tDataInizio = StringOperation.FormatDateTime(dtMyRow("data_inizio"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_fine")) Then
    '                oMyArticolo.tDataFine = StringOperation.FormatDateTime(dtMyRow("data_fine"))
    '            End If
    '            If Not IsDBNull(dtMyRow("istarsugiornaliera")) Then
    '                oMyArticolo.bIsTarsuGiornaliera = StringOperation.FormatBool(dtMyRow("istarsugiornaliera"))
    '            End If
    '            If Not IsDBNull(dtMyRow("forza_calcolapv")) Then
    '                oMyArticolo.bForzaPV = StringOperation.FormatBool(dtMyRow("forza_calcolapv"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo")) Then
    '                oMyArticolo.impRuolo = StringOperation.FormatDouble(dtMyRow("importo"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo_netto")) Then
    '                oMyArticolo.impNetto = StringOperation.FormatString(dtMyRow("importo_netto"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo_riduzioni")) Then
    '                oMyArticolo.impRiduzione = StringOperation.FormatDouble(dtMyRow("importo_riduzioni"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo_detassazioni")) Then
    '                oMyArticolo.impDetassazione = StringOperation.FormatDouble(dtMyRow("importo_detassazioni"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo_forzato")) Then
    '                oMyArticolo.bIsImportoForzato = StringOperation.FormatBool(dtMyRow("importo_forzato"))
    '            End If
    '            If Not IsDBNull(dtMyRow("idflusso_ruolo")) Then
    '                oMyArticolo.nIdFlussoRuolo = StringOperation.FormatInt(dtMyRow("idflusso_ruolo"))
    '            End If
    '            If Not IsDBNull(dtMyRow("tipo_ruolo")) Then
    '                oMyArticolo.sTipoRuolo = StringOperation.FormatString(dtMyRow("tipo_ruolo"))
    '            End If
    '            If Not IsDBNull(dtMyRow("idavviso")) Then
    '                oMyArticolo.IdAvviso = StringOperation.FormatInt(dtMyRow("idavviso"))
    '            End If
    '            If Not IsDBNull(dtMyRow("informazioni")) Then
    '                oMyArticolo.sNote = StringOperation.FormatString(dtMyRow("informazioni"))
    '            End If
    '            If Not IsDBNull(dtMyRow("operatore")) Then
    '                oMyArticolo.sOperatore = StringOperation.FormatString(dtMyRow("operatore"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_inserimento")) Then
    '                oMyArticolo.tDataInserimento = StringOperation.FormatDateTime(dtMyRow("data_inserimento"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyArticolo.tDataVariazione = StringOperation.FormatDateTime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyArticolo.tDataCessazione = StringOperation.FormatDateTime(dtMyRow("data_cessazione"))
    '            End If
    '            'prelevo le riduzioni
    '            oRicRidEse.IdEnte = oMyArticolo.IdEnte
    '            oRicRidEse.sAnno = oMyArticolo.sAnno
    '            If IsFromAvviso = True Then
    '                oMyArticolo.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, "")
    '                'prelevo le detassazioni
    '                oMyArticolo.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, "")
    '            Else
    '                oMyArticolo.oRiduzioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, oMyArticolo.TipoPartita)
    '                'prelevo le detassazioni
    '                oMyArticolo.oDetassazioni = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, ObjRidEseApplicati.RIF_ARTICOLO, oMyArticolo.Id, oMyArticolo.TipoPartita)
    '            End If
    '            '*** 20141211 - legami PF-PV ***
    '            If Not IsDBNull(dtMyRow("idoggetto")) Then
    '                oMyArticolo.IdOggetto = StringOperation.FormatInt(dtMyRow("idoggetto"))
    '            End If
    '            If oMyArticolo.TipoPartita = ObjArticolo.PARTEVARIABILE Then
    '                oMyArticolo.ListPFvsPV = GetLegamePFPV(oMyArticolo.Id, cmdMyCommand)
    '            End If
    '            '*** ***
    '            oListArticoli.Add(oMyArticolo)
    '        Next

    '        Return CType(oListArticoli.ToArray(GetType(ObjArticolo)), ObjArticolo())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetArticoli.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    'Public Function SetArticoloCompleto(ByVal oMyArticolo As ObjArticolo, ByVal bIsFromVariabile As Boolean, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim FncRidEse As New GestRidEse
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim MyProcedure As String = "prc_TBLARTICOLI_IU"
    '    Dim oRidEse As ObjRidEseApplicati
    '    Dim myTrans As SqlClient.SqlTransaction

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@ID", oMyArticolo.Id)
    '        cmdMyCommand.Parameters.AddWithValue("@IDRUOLO", oMyArticolo.IdArticolo)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", oMyArticolo.IdContribuente)
    '        cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyArticolo.IdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@IDDETTAGLIOTESTATA", oMyArticolo.IdDettaglioTestata)
    '        '*** 20141211 - legami PF-PV ***
    '        cmdMyCommand.Parameters.AddWithValue("@IDOGGETTO", oMyArticolo.IdOggetto)
    '        '*** ***
    '        If oMyArticolo.nCodVia <> -1 Then
    '            cmdMyCommand.Parameters.AddWithValue("@CODVIA", oMyArticolo.nCodVia)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@CODVIA", System.DBNull.Value)
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@VIA", oMyArticolo.sVia)
    '        If oMyArticolo.sCivico <> "-1" Then
    '            cmdMyCommand.Parameters.AddWithValue("@CIVICO", oMyArticolo.sCivico)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@CIVICO", "")
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@ESPONENTE", oMyArticolo.sEsponente)
    '        cmdMyCommand.Parameters.AddWithValue("@INTERNO", oMyArticolo.sInterno)
    '        cmdMyCommand.Parameters.AddWithValue("@SCALA", oMyArticolo.sScala)
    '        cmdMyCommand.Parameters.AddWithValue("@FOGLIO", oMyArticolo.sFoglio)
    '        cmdMyCommand.Parameters.AddWithValue("@NUMERO", oMyArticolo.sNumero)
    '        cmdMyCommand.Parameters.AddWithValue("@SUBALTERNO", oMyArticolo.sSubalterno)
    '        cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyArticolo.sAnno)
    '        cmdMyCommand.Parameters.AddWithValue("@IDCATEGORIA", oMyArticolo.sCategoria)
    '        If oMyArticolo.nComponenti > 0 Then
    '            cmdMyCommand.Parameters.AddWithValue("@NCOMPONENTI", oMyArticolo.nComponenti)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@NCOMPONENTI", System.DBNull.Value)
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@MQ", oMyArticolo.nMQ)
    '        cmdMyCommand.Parameters.AddWithValue("@BIMESTRI", oMyArticolo.nBimestri)
    '        If oMyArticolo.tDataInizio = Date.MinValue Then
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_INIZIO", System.DBNull.Value)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_INIZIO", oMyArticolo.tDataInizio)
    '        End If
    '        If oMyArticolo.tDataFine = Date.MinValue Then
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_FINE", System.DBNull.Value)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_FINE", oMyArticolo.tDataFine)
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@ISTARSUGIORNALIERA", oMyArticolo.bIsTarsuGiornaliera)
    '        cmdMyCommand.Parameters.AddWithValue("@IDTARIFFA", oMyArticolo.nIdTariffa)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_TARIFFA", oMyArticolo.impTariffa)
    '        '***Agenzia Entrate***                                                                                                   
    '        cmdMyCommand.Parameters.AddWithValue("@SEZIONE", oMyArticolo.sSezione)
    '        cmdMyCommand.Parameters.AddWithValue("@ESTENSIONE_PARTICELLA", oMyArticolo.sEstensioneParticella)
    '        cmdMyCommand.Parameters.AddWithValue("@ID_TIPO_PARTICELLA", oMyArticolo.sIdTipoParticella)
    '        If oMyArticolo.nIdTitoloOccupaz <> -1 Then
    '            cmdMyCommand.Parameters.AddWithValue("@ID_TITOLO_OCCUPAZIONE", oMyArticolo.nIdTitoloOccupaz)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@ID_TITOLO_OCCUPAZIONE", System.DBNull.Value)
    '        End If
    '        If oMyArticolo.nIdNaturaOccupaz <> -1 Then
    '            cmdMyCommand.Parameters.AddWithValue("@ID_NATURA_OCCUPANTE", oMyArticolo.nIdNaturaOccupaz)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@ID_NATURA_OCCUPANTE", System.DBNull.Value)
    '        End If
    '        If oMyArticolo.nIdDestUso <> -1 Then
    '            cmdMyCommand.Parameters.AddWithValue("@ID_DESTINAZIONE_USO", oMyArticolo.nIdDestUso)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@ID_DESTINAZIONE_USO", System.DBNull.Value)
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@ID_TIPO_UNITA", oMyArticolo.sIdTipoUnita)
    '        If oMyArticolo.nIdAssenzaDatiCatastali <> -1 Then
    '            cmdMyCommand.Parameters.AddWithValue("@ID_ASSENZA_DATI_CATASTALI", oMyArticolo.nIdAssenzaDatiCatastali)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@ID_ASSENZA_DATI_CATASTALI", System.DBNull.Value)
    '        End If
    '        '*********************                                                                                                   
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO", oMyArticolo.impRuolo)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_NETTO", oMyArticolo.impNetto)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_RIDUZIONI", oMyArticolo.impRiduzione)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_DETASSAZIONI", oMyArticolo.impDetassazione)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_FORZATO", oMyArticolo.bIsImportoForzato)
    '        cmdMyCommand.Parameters.AddWithValue("@INFORMAZIONI", oMyArticolo.sNote)
    '        cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", oMyArticolo.nIdFlussoRuolo)
    '        cmdMyCommand.Parameters.AddWithValue("@TIPO_RUOLO", oMyArticolo.sTipoRuolo)
    '        cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", oMyArticolo.IdAvviso)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_SANZIONI", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_INTERESSI", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@IMPORTO_SPESE_NOTIFICA", 0)
    '        cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE_DIFFERENZAIMPOSTA", String.Empty)
    '        cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE_SANZIONI", String.Empty)
    '        cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE_INTERESSI", String.Empty)
    '        cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE_SPESENOTIFICA", String.Empty)
    '        cmdMyCommand.Parameters.AddWithValue("@DA_ACCERTAMENTO", False)
    '        cmdMyCommand.Parameters.AddWithValue("@TIPOPARTITA", oMyArticolo.TipoPartita)

    '        cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", Now)
    '        If oMyArticolo.tDataVariazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", System.DBNull.Value)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", oMyArticolo.tDataVariazione)
    '        End If
    '        If oMyArticolo.tDataCessazione = Date.MinValue Then
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_CESSAZIONE", System.DBNull.Value)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_CESSAZIONE", oMyArticolo.tDataCessazione)
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@OPERATORE", oMyArticolo.sOperatore)

    '        'eseguo la query
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = MyProcedure
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '        Log.Debug("SetArticoloCompleto::SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        cmdMyCommand.ExecuteNonQuery()
    '        oMyArticolo.Id = cmdMyCommand.Parameters("@ID").Value
    '        Log.Debug("SetArticoloCompleto::IDRuolo::" & oMyArticolo.Id)
    '        If oMyArticolo.Id > 0 Then
    '            If Not oMyArticolo.oRiduzioni Is Nothing Then
    '                For Each oRidEse In oMyArticolo.oRiduzioni
    '                    'devo forzare il riferimento riduzione ad articolo altrimenti inserisce sbagliato
    '                    oRidEse.Riferimento = ObjRidEseApplicati.RIF_ARTICOLO
    '                    cmdMyCommand.Parameters.Clear()
    '                    cmdMyCommand.Parameters.AddWithValue("@IDARTICOLO", oMyArticolo.Id)
    '                    cmdMyCommand.Parameters.AddWithValue("@IDENTE", ConstSession.IdEnte)
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", System.DBNull.Value)
    '                    If bIsFromVariabile = "1" Then
    '                        cmdMyCommand.Parameters.AddWithValue("@IDCODICE", oRidEse.sCodice) '*** x CMGC ***
    '                    Else
    '                        cmdMyCommand.Parameters.AddWithValue("@IDCODICE", oRidEse.ID) '*** x RIBES ***
    '                    End If
    '                    MyProcedure = "prc_TBLARTICOLI" & ObjRidEse.TIPO_RIDUZIONI & "_IU"
    '                    'eseguo la query
    '                    cmdMyCommand.Parameters("@IDARTICOLO").Direction = ParameterDirection.InputOutput
    '                    cmdMyCommand.CommandText = MyProcedure
    '                    sValParametri = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '                    Log.Debug("SetArticoloCompleto::RID::SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '                    cmdMyCommand.ExecuteNonQuery()
    '                    oRidEse.ID = cmdMyCommand.Parameters("@IDARTICOLO").Value
    '                    If oRidEse.ID <= 0 Then
    '                        Log.Debug("Si è verificato un errore in SetArticoloCompleto::SetRidEse::oMyArticolo.IdContribuente::" & oMyArticolo.IdContribuente)
    '                        If (cmdMyCommandOut Is Nothing) Then
    '                            myTrans.Rollback()
    '                        End If
    '                        Return 0
    '                    End If
    '                Next
    '            End If
    '            If Not oMyArticolo.oDetassazioni Is Nothing Then
    '                For Each oRidEse In oMyArticolo.oDetassazioni
    '                    'devo forzare il riferimento riduzione ad articolo altrimenti inserisce sbagliato
    '                    oRidEse.Riferimento = ObjRidEseApplicati.RIF_ARTICOLO
    '                    cmdMyCommand.Parameters.Clear()
    '                    cmdMyCommand.Parameters.AddWithValue("@IDARTICOLO", oMyArticolo.Id)
    '                    cmdMyCommand.Parameters.AddWithValue("@IDENTE", ConstSession.IdEnte)
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", System.DBNull.Value)
    '                    If bIsFromVariabile = "1" Then
    '                        cmdMyCommand.Parameters.AddWithValue("@IDCODICE", oRidEse.sCodice) '*** x CMGC ***
    '                    Else
    '                        cmdMyCommand.Parameters.AddWithValue("@IDCODICE", oRidEse.ID) '*** x RIBES ***
    '                    End If
    '                    MyProcedure = "prc_TBLARTICOLI" & ObjRidEse.TIPO_ESENZIONI & "_IU"
    '                    'eseguo la query
    '                    cmdMyCommand.Parameters("@IDARTICOLO").Direction = ParameterDirection.InputOutput
    '                    cmdMyCommand.CommandText = MyProcedure
    '                    sValParametri = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '                    Log.Debug("SetArticoloCompleto::DET::SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '                    cmdMyCommand.ExecuteNonQuery()
    '                    oRidEse.ID = cmdMyCommand.Parameters("@IDARTICOLO").Value
    '                    If oRidEse.ID <= 0 Then
    '                        Log.Debug("Si è verificato un errore in SetArticoloCompleto::SetRidEse::oMyArticolo.IdContribuente::" & oMyArticolo.IdContribuente)
    '                        If (cmdMyCommandOut Is Nothing) Then
    '                            myTrans.Rollback()
    '                        End If
    '                        Return 0
    '                    End If
    '                Next
    '            End If
    '        End If

    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return oMyArticolo.Id
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestArticolo.SetArticoloCompleto.errore: ", Err)
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Rollback()
    '        End If
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
    '*** 20141211 - legami PF-PV ***
    'Public Function GetLegamePFPV(myConnectionString As String, ByVal nIdArticolo As Integer) As ObjLegamePFPV()
    '    Dim myList As New ArrayList
    '    Dim oMyLegame As ObjLegamePFPV
    '    Dim sSQL As String = ""
    '    Dim myDataView As New DataView
    '    Dim nMyReturn As Integer = -1

    '    Try
    '        Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
    '            Try
    '                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLegamePFPV", "IDTESTATA", "Anno", "IDPV")
    '                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESTATA", -1) _
    '                        , ctx.GetParam("Anno", -1) _
    '                        , ctx.GetParam("IDPV", nIdArticolo)
    '                    )
    '            Catch ex As Exception
    '                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetLegamePFPV.erroreQuery: ", ex)
    '                Return Nothing
    '            Finally
    '                ctx.Dispose()
    '            End Try
    '            For Each myRow As DataRowView In myDataView
    '                oMyLegame = New ObjLegamePFPV
    '                oMyLegame.IdPF = StringOperation.FormatInt(myRow("idpf"))
    '                oMyLegame.IdPV = StringOperation.FormatInt(myRow("idpv"))
    '                myList.Add(oMyLegame)
    '            Next
    '        End Using
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetLegamePFPV.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        myDataView.Dispose()
    '    End Try
    '    Return CType(myList.ToArray(GetType(ObjLegamePFPV)), ObjLegamePFPV())
    'End Function
    'Public Function GetLegamePFPV(ByVal nIdArticolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjLegamePFPV()
    '    Dim myList As New ArrayList
    '    Dim oMyLegame As ObjLegamePFPV
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = -1
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = -1
    '        cmdMyCommand.Parameters.AddWithValue("@IDPV", nIdArticolo)
    '        cmdMyCommand.CommandText = "prc_GetLegamePFPV"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyLegame = New ObjLegamePFPV
    '            oMyLegame.IdPF = StringOperation.FormatInt(dtMyRow("idpf"))
    '            oMyLegame.IdPV = StringOperation.FormatInt(dtMyRow("idpv"))
    '            myList.Add(oMyLegame)
    '        Next

    '        Return CType(myList.ToArray(GetType(ObjLegamePFPV)), ObjLegamePFPV())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestArticolo.GetLegamePFPV.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    'Public Function SetLegamePFPV(ByVal myObj As OBJLEGAMEPFPV, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim FncRidEse As New GestRidEse
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim MyProcedure As String = "prc_TBLARTICOLI_PFVSPV_IU"
    '    Dim oRidEse As ObjRidEseApplicati
    '    Dim myTrans As SqlClient.SqlTransaction

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IDPF", myObj.IdPF)
    '        cmdMyCommand.Parameters.AddWithValue("@IDPV", myObj.IdPV)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = MyProcedure
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '        Log.Debug("SetLegamePFVP::SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        cmdMyCommand.ExecuteNonQuery()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestArticolo.SetLegamePFPV.errore: ", Err)
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Rollback()
    '        End If
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
End Class
''' <summary>
''' Classe per la gestione delle voci di dettaglio avviso
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestDetVoci
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestDetVoci))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    'Public Function GetDetVoci(ByVal nIdAvviso As Integer, ByVal nIdDetVoce As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjDetVoci()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oListDetVoci() As ObjDetVoci
    '    Dim oMyDetVoci As ObjDetVoci
    '    Dim nList As Integer = -1

    '    Try
    '        cmdMyCommand.CommandText = "SELECT IDDETTAGLIO, IDAVVISO, CODICE_CAPITOLO, ANNO, CODICE_VOCE, IMPORTO"
    '        cmdMyCommand.CommandText += ", CASE WHEN CODICE_CAPITOLO='0000' THEN 'IMPOSTA' ELSE TBLADDIZIONALI.DESCRIZIONE END AS DESCRCAPITOLO"
    '        cmdMyCommand.CommandText += ", OPERATORE, DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE"
    '        cmdMyCommand.CommandText += " FROM TBLCARTELLEDETTAGLIOVOCI"
    '        cmdMyCommand.CommandText += " LEFT JOIN TBLADDIZIONALI ON TBLCARTELLEDETTAGLIOVOCI.CODICE_CAPITOLO=TBLADDIZIONALI.CODICE"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        If nIdDetVoce > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDDETTAGLIO=@IDDETTAGLIO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDetVoci", SqlDbType.Int)).Value = nIdDetVoce
    '        End If
    '        If nIdAvviso > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDAVVISO=@IDAVVISO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY CODICE_CAPITOLO"
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            'incremento l'indice
    '            oMyDetVoci = New ObjDetVoci
    '            oMyDetVoci.IdDettaglio = StringOperation.Formatint(DrDati("iddettaglio"))
    '            oMyDetVoci.IdAvviso = StringOperation.Formatint(DrDati("idavviso"))
    '            oMyDetVoci.sAnno = StringOperation.FormatString(DrDati("anno"))
    '            oMyDetVoci.sCapitolo = StringOperation.Formatint(DrDati("codice_capitolo"))
    '            If Not IsDBNull(DrDati("descrcapitolo")) Then
    '                oMyDetVoci.sDescrizione = StringOperation.FormatString(DrDati("descrcapitolo"))
    '            End If
    '            If Not IsDBNull(DrDati("importo")) Then
    '                oMyDetVoci.impDettaglio = stringoperation.formatdouble(DrDati("importo"))
    '            End If
    '            If Not IsDBNull(DrDati("operatore")) Then
    '                oMyDetVoci.sOperatore = StringOperation.FormatString(DrDati("operatore"))
    '            End If
    '            If Not IsDBNull(DrDati("data_inserimento")) Then
    '                oMyDetVoci.tDataInserimento = StringOperation.Formatdatetime(DrDati("data_inserimento"))
    '            End If
    '            If Not IsDBNull(DrDati("data_variazione")) Then
    '                oMyDetVoci.tDataVariazione = StringOperation.Formatdatetime(DrDati("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrDati("data_cessazione")) Then
    '                oMyDetVoci.tDataCessazione = StringOperation.Formatdatetime(DrDati("data_cessazione"))
    '            End If

    '            nList += 1
    '            ReDim Preserve oListDetVoci(nList)
    '            oListDetVoci(nList) = oMyDetVoci
    '        Loop

    '        Return oListDetVoci
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestDetVoci.GetDetVoci.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function SetDetVoci(ByVal oMyDettaglio As ObjDetVoci, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        Select Case DbOperation
    '            Case Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLEDETTAGLIOVOCI(IDAVVISO"
    '                cmdMyCommand.CommandText += ", ANNO, CODICE_CAPITOLO, CODICE_VOCE, IMPORTO"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES(@IDAVVISO"
    '                cmdMyCommand.CommandText += ",@ANNO,@CODICE_CAPITOLO,@CODICE_VOCE,@IMPORTO"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyDettaglio.IdAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyDettaglio.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CAPITOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sCapitolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_VOCE", SqlDbType.NVarChar)).Value = -1
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyDettaglio.impDettaglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                If oMyDettaglio.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyDettaglio.tDataVariazione
    '                End If
    '                If oMyDettaglio.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyDettaglio.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyDettaglio.sOperatore
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "UPDATE TBLCARTELLEDETTAGLIOVOCI"
    '                cmdMyCommand.CommandText += " SET DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = Now
    '                If nIdFlusso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
    '                    cmdMyCommand.CommandText += "  SELECT ID "
    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestDetVoci.SetDetVoci.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="nIdAvviso"></param>
    ''' <param name="nIdDetVoce"></param>
    ''' <returns></returns>
    Public Function GetDetVoci(myStringConnection As String, ByVal nIdAvviso As Integer, ByVal nIdDetVoce As Integer) As ObjDetVoci()
        Dim oListDetVoci As New ArrayList
        Dim oMyDetVoci As ObjDetVoci
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDettaglioVoci", "IDDettaglio", "IDAVVISO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDDettaglio", nIdDetVoce) _
                            , ctx.GetParam("IDAVVISO", nIdAvviso)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDetVoci.GetDetVoci.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyDetVoci = New ObjDetVoci
                    oMyDetVoci.IdDettaglio = StringOperation.FormatInt(myRow("iddettaglio"))
                    oMyDetVoci.IdAvviso = StringOperation.FormatInt(myRow("idavviso"))
                    oMyDetVoci.sAnno = StringOperation.FormatString(myRow("anno"))
                    oMyDetVoci.sCapitolo = StringOperation.FormatString(myRow("codice_capitolo"))
                    oMyDetVoci.sDescrizione = StringOperation.FormatString(myRow("descrcapitolo"))
                    oMyDetVoci.CodVoce = StringOperation.FormatInt(myRow("codice_voce"))
                    oMyDetVoci.impDettaglio = StringOperation.FormatDouble(myRow("importo"))
                    oMyDetVoci.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    oMyDetVoci.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                    oMyDetVoci.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                    oMyDetVoci.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))
                    oListDetVoci.Add(oMyDetVoci)
                Next
            End Using

            Return CType(oListDetVoci.ToArray(GetType(ObjDetVoci)), ObjDetVoci())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDetVoci.GetDetVoci.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function GetDetVoci(ByVal nIdAvviso As Integer, ByVal nIdDetVoce As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjDetVoci()
    '    Dim oListDetVoci As New ArrayList
    '    Dim oMyDetVoci As ObjDetVoci
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IDDettaglio", nIdDetVoce)
    '        cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", nIdAvviso)
    '        cmdMyCommand.CommandText = "prc_GetDettaglioVoci"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyDetVoci = New ObjDetVoci
    '            oMyDetVoci.IdDettaglio = StringOperation.FormatInt(dtMyRow("iddettaglio"))
    '            oMyDetVoci.IdAvviso = StringOperation.FormatInt(dtMyRow("idavviso"))
    '            oMyDetVoci.sAnno = StringOperation.FormatString(dtMyRow("anno"))
    '            oMyDetVoci.sCapitolo = StringOperation.FormatString(dtMyRow("codice_capitolo"))
    '            If Not IsDBNull(dtMyRow("descrcapitolo")) Then
    '                oMyDetVoci.sDescrizione = StringOperation.FormatString(dtMyRow("descrcapitolo"))
    '            End If
    '            If Not IsDBNull(dtMyRow("codice_voce")) Then
    '                oMyDetVoci.CodVoce = StringOperation.FormatInt(dtMyRow("codice_voce"))
    '            End If
    '            If Not IsDBNull(dtMyRow("importo")) Then
    '                oMyDetVoci.impDettaglio = StringOperation.FormatDouble(dtMyRow("importo"))
    '            End If
    '            If Not IsDBNull(dtMyRow("operatore")) Then
    '                oMyDetVoci.sOperatore = StringOperation.FormatString(dtMyRow("operatore"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_inserimento")) Then
    '                oMyDetVoci.tDataInserimento = StringOperation.FormatDateTime(dtMyRow("data_inserimento"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyDetVoci.tDataVariazione = StringOperation.FormatDateTime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyDetVoci.tDataCessazione = StringOperation.FormatDateTime(dtMyRow("data_cessazione"))
    '            End If
    '            oListDetVoci.Add(oMyDetVoci)
    '        Next

    '        Return CType(oListDetVoci.ToArray(GetType(ObjDetVoci)), ObjDetVoci())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDetVoci.GetDetVoci.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    'Public Function SetDetVoci(ByVal oMyDettaglio As ObjDetVoci, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim MyProcedure As String = "prc_TBLCARTELLEDETTAGLIOVOCI_IU"
    '    Dim myTrans As SqlClient.SqlTransaction

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If

    '        cmdMyCommand.Parameters.Clear()
    '        If oMyDettaglio Is Nothing Then
    '            oMyDettaglio = New ObjDetVoci
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@ID", oMyDettaglio.IdDettaglio)
    '        Select Case DbOperation
    '            Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
    '                cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", oMyDettaglio.IdAvviso)
    '                cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyDettaglio.sAnno)
    '                cmdMyCommand.Parameters.AddWithValue("@CODICE_CAPITOLO", oMyDettaglio.sCapitolo)
    '                cmdMyCommand.Parameters.AddWithValue("@CODICE_VOCE", oMyDettaglio.CodVoce)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO", oMyDettaglio.impDettaglio)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", Now)
    '                If oMyDettaglio.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", oMyDettaglio.tDataVariazione)
    '                End If
    '                If oMyDettaglio.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_CESSAZIONE", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_CESSAZIONE", oMyDettaglio.tDataCessazione)
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", oMyDettaglio.sOperatore)
    '                MyProcedure = "prc_TBLCARTELLEDETTAGLIOVOCI_IU"
    '            Case Utility.Costanti.AZIONE_DELETE
    '                If oMyDettaglio.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATA_VARIAZIONE", oMyDettaglio.tDataVariazione)
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdFlusso)
    '                MyProcedure = "prc_TBLCARTELLEDETTAGLIOVOCI_D"
    '        End Select
    '        'eseguo la query
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = MyProcedure
    '        Log.Debug("SetDetVoci::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        oMyDettaglio.IdDettaglio = cmdMyCommand.Parameters("@ID").Value
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return oMyDettaglio.IdDettaglio
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestDetVoci.SetDetVoci.errore: ", Err)
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Rollback()
    '        End If
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
End Class
''' <summary>
''' Classe per la gestione della rata
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestRata
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestRata))
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand

    'Public Function GetRateConfigurate(ByVal nIdFlusso As Integer, ByVal oMyConto As OPENUtility.objContoCorrente, ByVal WFSessione As OPENUtility.CreateSessione) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata()
    '    Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim oMyRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim nList As Integer = -1

    '    Try
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLRATEENTE"
    '        cmdMyCommand.CommandText += " WHERE (IDRUOLO=@IDFLUSSORUOLO)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSORUOLO", SqlDbType.Int)).Value = nIdFlusso
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            oMyRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata

    '            oMyRata.NumeroRata = DrDati("NRATA")
    '            oMyRata.DescrizioneRata = DrDati("DESCRIZIONE")
    '            oMyRata.DataScadenza = DrDati("DATASCADENZA")
    '            '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '            If Not IsNothing(oMyConto) Then
    '                oMyRata.NumeroContoCorrente = oMyConto.ContoCorrente
    '            End If
    '            '*** ***
    '            nList += 1
    '            ReDim Preserve oListRate(nList)
    '            oListRate(nList) = oMyRata
    '        Loop

    '        Return oListRate
    '    Catch err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRata.GetRateConfigurate.errore: ", err)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function SetRataConfigurata(ByVal oMyRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByVal sOperatore As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        Select Case DbOperation
    '            Case Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLRATEENTE(IDRUOLO"
    '                cmdMyCommand.CommandText += ", NRATA, DESCRIZIONE, DATASCADENZA, DATA_INSERIMENTO,OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDFLUSSORUOLO"
    '                cmdMyCommand.CommandText += ",@NRATA,@DESCRIZIONE,@DATASCADENZA,@DATAINSERIMENTO,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSORUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NRATA", SqlDbType.NVarChar)).Value = oMyRata.NumeroRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DESCRIZIONE", SqlDbType.NVarChar)).Value = oMyRata.DescrizioneRata
    '                If oMyRata.DataScadenza = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASCADENZA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASCADENZA", SqlDbType.DateTime)).Value = oMyRata.DataScadenza
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = sOperatore

    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case Costanti.AZIONE_UPDATE
    '                cmdMyCommand.CommandText = "UPDATE TBLRATEENTE SET"
    '                cmdMyCommand.CommandText += " IDRUOLO=@IDFLUSSORUOLO"
    '                cmdMyCommand.CommandText += ",NRATA=@NRATA"
    '                cmdMyCommand.CommandText += ",DESCRIZIONE=@DESCRIZIONE"
    '                cmdMyCommand.CommandText += ",DATASCADENZA=@DATASCADENZA"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                cmdMyCommand.CommandText += " AND (IDRUOLO=@IDFLUSSORUOLO)"
    '                cmdMyCommand.CommandText += " AND (NRATA=@NRATA)"
    '                cmdMyCommand.CommandText += " AND (DESCRIZIONE=@DESCRIZIONE)"
    '                cmdMyCommand.CommandText += " AND (DATASCADENZA=@DATASCADENZA)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSORUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NRATA", SqlDbType.NVarChar)).Value = oMyRata.NumeroRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DESCRIZIONE", SqlDbType.NVarChar)).Value = oMyRata.DescrizioneRata
    '                If oMyRata.DataScadenza = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASCADENZA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASCADENZA", SqlDbType.DateTime)).Value = oMyRata.DataScadenza
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLRATEENTE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                cmdMyCommand.CommandText += " AND (IDRUOLO=@IDFLUSSORUOLO)"
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSORUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                If Not IsNothing(oMyRata) Then
    '                    cmdMyCommand.CommandText += " AND (NRATA=@NRATA)"
    '                    cmdMyCommand.CommandText += " AND (DESCRIZIONE=@DESCRIZIONE)"
    '                    cmdMyCommand.CommandText += " AND (DATASCADENZA=@DATASCADENZA)"
    '                    'valorizzo i parameters:
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NRATA", SqlDbType.NVarChar)).Value = oMyRata.NumeroRata
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DESCRIZIONE", SqlDbType.NVarChar)).Value = oMyRata.DescrizioneRata
    '                    If oMyRata.DataScadenza = Date.MinValue Then
    '                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASCADENZA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                    Else
    '                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATASCADENZA", SqlDbType.DateTime)).Value = oMyRata.DataScadenza
    '                    End If
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 0 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRata.SetRataConfigurata.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function GetRate(ByVal nIdAvviso As Integer, ByVal nIdRata As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjRata()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oListRate() As ObjRata
    '    Dim oMyRata As ObjRata
    '    Dim nList As Integer = -1

    '    Try
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLCARTELLERATE"
    '        cmdMyCommand.CommandText += " WHERE (DATA_VARIAZIONE IS NULL)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        If nIdRata > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDRATA=@IDRATA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDRATA", SqlDbType.Int)).Value = nIdRata
    '        End If
    '        If nIdAvviso > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDAVVISO=@IDAVVISO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = nIdAvviso
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY NUMERO_RATA"
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            'incremento l'indice
    '            oMyRata = New ObjRata
    '            oMyRata.Id = StringOperation.Formatint(DrDati("idrata"))
    '            oMyRata.IdAvviso = StringOperation.Formatint(DrDati("idavviso"))
    '            oMyRata.sNRata = StringOperation.FormatString(DrDati("numero_rata"))
    '            oMyRata.sDescrRata = StringOperation.FormatString(DrDati("descrizione_rata"))
    '            If Not IsDBNull(DrDati("importo_rata")) Then
    '                oMyRata.impRata = stringoperation.formatdouble(DrDati("importo_rata"))
    '            End If
    '            If Not IsDBNull(DrDati("codice_bollettino")) Then
    '                oMyRata.sCodBollettino = StringOperation.FormatString(DrDati("codice_bollettino"))
    '            End If
    '            If Not IsDBNull(DrDati("codeline")) Then
    '                oMyRata.sCodeline = StringOperation.FormatString(DrDati("codeline"))
    '            End If
    '            If Not IsDBNull(DrDati("codice_barcode")) Then
    '                oMyRata.sCodiceBarcode = StringOperation.FormatString(DrDati("codice_barcode"))
    '            End If
    '            If Not IsDBNull(DrDati("numero_conto_corrente")) Then
    '                oMyRata.sContoCorrente = StringOperation.FormatString(DrDati("numero_conto_corrente"))
    '            End If
    '            If Not IsDBNull(DrDati("data_scadenza")) Then
    '                oMyRata.tDataScadenza = StringOperation.Formatdatetime(DrDati("data_scadenza"))
    '            End If
    '            If Not IsDBNull(DrDati("data_inserimento")) Then
    '                oMyRata.tDataInserimento = StringOperation.Formatdatetime(DrDati("data_inserimento"))
    '            End If
    '            If Not IsDBNull(DrDati("data_variazione")) Then
    '                oMyRata.tDataVariazione = StringOperation.Formatdatetime(DrDati("data_variazione"))
    '            End If
    '            If Not IsDBNull(DrDati("data_cessazione")) Then
    '                oMyRata.tDataCessazione = StringOperation.Formatdatetime(DrDati("data_cessazione"))
    '            End If

    '            nList += 1
    '            ReDim Preserve oListRate(nList)
    '            oListRate(nList) = oMyRata
    '        Loop

    '        Return oListRate
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRata.GetRate.errore: ", err)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function SetRata(ByVal oMyRata As ObjRata, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        Select Case DbOperation
    '            Case Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLERATE (IDAVVISO"
    '                cmdMyCommand.CommandText += ", NUMERO_RATA, DESCRIZIONE_RATA, IMPORTO_RATA, DATA_SCADENZA"
    '                cmdMyCommand.CommandText += ", CODICE_BOLLETTINO, CODELINE, NUMERO_CONTO_CORRENTE, CODICE_BARCODE"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDAVVISO"
    '                cmdMyCommand.CommandText += ",@NUMERO_RATA,@DESCRIZIONE_RATA,@IMPORTO_RATA,@DATA_SCADENZA"
    '                cmdMyCommand.CommandText += ",@CODICE_BOLLETTINO,@CODELINE,@NUMERO_CONTO_CORRENTE,@CODICE_BARCODE"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyRata.IdAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyRata.sNRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DESCRIZIONE_RATA", SqlDbType.NVarChar)).Value = oMyRata.sDescrRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_RATA", SqlDbType.Float)).Value = oMyRata.impRata
    '                If oMyRata.tDataScadenza = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_SCADENZA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_SCADENZA", SqlDbType.DateTime)).Value = oMyRata.tDataScadenza
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyRata.sCodBollettino
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODELINE", SqlDbType.NVarChar)).Value = oMyRata.sCodeline
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_CONTO_CORRENTE", SqlDbType.NVarChar)).Value = oMyRata.sContoCorrente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BARCODE", SqlDbType.NVarChar)).Value = oMyRata.sCodiceBarcode
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                If oMyRata.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyRata.tDataVariazione
    '                End If
    '                If oMyRata.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyRata.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyRata.sOperatore

    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "UPDATE TBLCARTELLERATE"
    '                cmdMyCommand.CommandText += " SET DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = Now
    '                If nIdFlusso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
    '                    cmdMyCommand.CommandText += "  SELECT ID "
    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRata.SetRata.errore: ", err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetRata(ByVal myStringConnection As String, ByVal oMyRata As ObjRata, ByVal nIdFlusso As Integer, ByVal DbOperation As Integer) As Integer
    '    Dim myIdentity As Integer

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        Select Case DbOperation
    '            Case Utility.Costanti.AZIONE_NEW
    '                cmdMyCommand.CommandText = "INSERT INTO TBLCARTELLERATE (IDAVVISO"
    '                cmdMyCommand.CommandText += ", NUMERO_RATA, DESCRIZIONE_RATA, IMPORTO_RATA, DATA_SCADENZA"
    '                cmdMyCommand.CommandText += ", CODICE_BOLLETTINO, CODELINE, NUMERO_CONTO_CORRENTE, CODICE_BARCODE"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDAVVISO"
    '                cmdMyCommand.CommandText += ",@NUMERO_RATA,@DESCRIZIONE_RATA,@IMPORTO_RATA,@DATA_SCADENZA"
    '                cmdMyCommand.CommandText += ",@CODICE_BOLLETTINO,@CODELINE,@NUMERO_CONTO_CORRENTE,@CODICE_BARCODE"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@DATA_VARIAZIONE,@DATA_CESSAZIONE,@OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = oMyRata.IdAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyRata.sNRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DESCRIZIONE_RATA", SqlDbType.NVarChar)).Value = oMyRata.sDescrRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_RATA", SqlDbType.Float)).Value = oMyRata.impRata
    '                If oMyRata.tDataScadenza = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_SCADENZA", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_SCADENZA", SqlDbType.DateTime)).Value = oMyRata.tDataScadenza
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyRata.sCodBollettino
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODELINE", SqlDbType.NVarChar)).Value = oMyRata.sCodeline
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_CONTO_CORRENTE", SqlDbType.NVarChar)).Value = oMyRata.sContoCorrente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BARCODE", SqlDbType.NVarChar)).Value = oMyRata.sCodiceBarcode
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                If oMyRata.tDataVariazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oMyRata.tDataVariazione
    '                End If
    '                If oMyRata.tDataCessazione = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oMyRata.tDataCessazione
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyRata.sOperatore
    '                Log.Debug("SetRata::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = cmdMyCommand.ExecuteReader
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case Utility.Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "UPDATE TBLCARTELLERATE"
    '                cmdMyCommand.CommandText += " SET DATA_VARIAZIONE=@DATA_VARIAZIONE"
    '                cmdMyCommand.CommandText += " WHERE (1=1)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = Now
    '                If nIdFlusso <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDAVVISO IN ("
    '                    cmdMyCommand.CommandText += "  SELECT ID "
    '                    cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '                    cmdMyCommand.CommandText += "  WHERE IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO))"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '                End If
    '                'eseguo la query
    '                Log.Debug("SetRata::" & cmdMyCommand.CommandText & " " & Utility.Costanti.GetValParamCmd(cmdMyCommand))
    '                If cmdMyCommand.ExecuteNonQuery < 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRata.SetRata.errore: ", err)
    '        Return 0
    '    Finally
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="nIdFlusso"></param>
    ''' <param name="oMyConto"></param>
    ''' <returns></returns>
    Public Function GetRateConfigurate(myStringConnection As String, ByVal nIdFlusso As Integer, ByVal oMyConto As OPENUtility.objContoCorrente) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata()
        Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata = Nothing
        Dim oMyRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
        Dim nList As Integer = -1
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRate", "IdFlusso")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdFlusso", nIdFlusso))
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.GetRateConfigurate.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                    oMyRata.NumeroRata = StringOperation.FormatString(myRow("NRATA"))
                    oMyRata.DescrizioneRata = StringOperation.FormatString(myRow("DESCRIZIONE"))
                    oMyRata.DataScadenza = StringOperation.FormatString(myRow("DATASCADENZA"))
                    oMyRata.Percentuale = StringOperation.FormatDouble(myRow("percentuale"))
                    oMyRata.HasImposta = StringOperation.FormatBool(myRow("hasimposta"))
                    oMyRata.HasMaggiorazione = StringOperation.FormatBool(myRow("hasmaggiorazione"))
                    oMyRata.sTipoBollettino = StringOperation.FormatString(myRow("TipoBollettino"))
                    oMyRata.impSoglia = StringOperation.FormatDouble(myRow("SOGLIARATE"))
                    '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                    If Not IsNothing(oMyConto) Then
                        oMyRata.NumeroContoCorrente = oMyConto.ContoCorrente
                    End If
                    '*** ***
                    nList += 1
                    ReDim Preserve oListRate(nList)
                    oListRate(nList) = oMyRata
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.GetRateConfigurate.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return oListRate
    End Function
    'Public Function GetRateConfigurate(ByVal nIdFlusso As Integer, ByVal oMyConto As OPENUtility.objContoCorrente, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata()
    '    Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata = Nothing
    '    Dim oMyRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim nList As Integer = -1
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdFlusso", nIdFlusso)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetRate"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata

    '            oMyRata.NumeroRata = dtMyRow("NRATA")
    '            oMyRata.DescrizioneRata = dtMyRow("DESCRIZIONE")
    '            oMyRata.DataScadenza = dtMyRow("DATASCADENZA")
    '            oMyRata.Percentuale = dtMyRow("percentuale")
    '            oMyRata.HasImposta = dtMyRow("hasimposta")
    '            oMyRata.HasMaggiorazione = dtMyRow("hasmaggiorazione")
    '            oMyRata.sTipoBollettino = dtMyRow("TipoBollettino")
    '            oMyRata.impSoglia = dtMyRow("SOGLIARATE")
    '            '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '            If Not IsNothing(oMyConto) Then
    '                oMyRata.NumeroContoCorrente = oMyConto.ContoCorrente
    '            End If
    '            '*** ***
    '            nList += 1
    '            ReDim Preserve oListRate(nList)
    '            oListRate(nList) = oMyRata
    '        Next
    '        Return oListRate
    '    Catch err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.GetRateConfigurate.errore: ", err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        If ((cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="oMyRata"></param>
    ''' <param name="nIdFlusso"></param>
    ''' <param name="nDbOperation"></param>
    ''' <param name="sOperatore"></param>
    ''' <returns></returns>
    Public Function SetRataConfigurata(myStringConnection As String, ByVal oMyRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata, ByVal nIdFlusso As Integer, ByVal nDbOperation As Integer, ByVal sOperatore As String) As Integer
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    Select Case nDbOperation
                        Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLRATEENTE_IU", "ID", "IDRUOLO", "NRATA", "DESCRIZIONE", "PERCENTUALE", "HASIMPOSTA", "HASMAGGIORAZIONE", "TIPOBOLLETTINO", "SOGLIARATE", "DATASCADENZA", "DATA_INSERIMENTO", "OPERATORE") '
                            myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", -1) _
                            , ctx.GetParam("IDRUOLO", nIdFlusso) _
                            , ctx.GetParam("NRATA", oMyRata.NumeroRata) _
                            , ctx.GetParam("DESCRIZIONE", oMyRata.DescrizioneRata) _
                            , ctx.GetParam("PERCENTUALE", oMyRata.Percentuale) _
                            , ctx.GetParam("HASIMPOSTA", oMyRata.HasImposta) _
                            , ctx.GetParam("HASMAGGIORAZIONE", oMyRata.HasMaggiorazione) _
                            , ctx.GetParam("TIPOBOLLETTINO", oMyRata.sTipoBollettino) _
                            , ctx.GetParam("SOGLIARATE", oMyRata.impSoglia) _
                            , ctx.GetParam("DATASCADENZA", StringOperation.FormatDateTime(oMyRata.DataScadenza)) _
                            , ctx.GetParam("DATA_INSERIMENTO", Now) _
                            , ctx.GetParam("OPERATORE", sOperatore)
                        )
                        Case Utility.Costanti.AZIONE_DELETE
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLRATEENTE_D", "ID", "IDRUOLO", "NRATA")
                            myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", -1) _
                            , ctx.GetParam("IDRUOLO", nIdFlusso) _
                            , ctx.GetParam("NRATA", String.Empty)
                        )
                    End Select
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.SetRataConfigurata.errorequery: ", ex)
                    nMyReturn = -1
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nMyReturn = Utility.StringOperation.FormatInt(myRow("id"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.SetRataConfigurata.errore: ", Err)
            nMyReturn = 0
        Finally
            myDataView.Dispose()
        End Try
        Return nMyReturn
    End Function
    'Public Function SetRataConfigurata(ByVal oMyRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata, ByVal nIdFlusso As Integer, ByVal nDbOperation As Integer, ByVal sOperatore As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim MyProcedure As String = "prc_TBLRATEENTE_IU"
    '    Dim MyIdentity As Integer = -1

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@ID", -1)
    '        Select Case nDbOperation
    '            Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
    '                cmdMyCommand.Parameters.AddWithValue("@IDRUOLO", nIdFlusso)
    '                cmdMyCommand.Parameters.AddWithValue("@NRATA", oMyRata.NumeroRata)
    '                cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE", oMyRata.DescrizioneRata)
    '                cmdMyCommand.Parameters.AddWithValue("@PERCENTUALE", oMyRata.Percentuale)
    '                cmdMyCommand.Parameters.AddWithValue("@HASIMPOSTA", oMyRata.HasImposta)
    '                cmdMyCommand.Parameters.AddWithValue("@HASMAGGIORAZIONE", oMyRata.HasMaggiorazione)
    '                cmdMyCommand.Parameters.AddWithValue("@TIPOBOLLETTINO", oMyRata.sTipoBollettino)
    '                cmdMyCommand.Parameters.AddWithValue("@SOGLIARATE", oMyRata.impSoglia)
    '                If oMyRata.DataScadenza = Date.MinValue Then
    '                    cmdMyCommand.Parameters.AddWithValue("@DATASCADENZA", System.DBNull.Value)
    '                Else
    '                    cmdMyCommand.Parameters.AddWithValue("@DATASCADENZA", StringOperation.FormatDateTime(oMyRata.DataScadenza))
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", Now)
    '                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", sOperatore)

    '                MyProcedure = "prc_TBLRATEENTE_IU"
    '            Case Utility.Costanti.AZIONE_DELETE
    '                cmdMyCommand.Parameters.AddWithValue("@IDRUOLO", nIdFlusso)
    '                If Not IsNothing(oMyRata) Then
    '                    cmdMyCommand.Parameters.AddWithValue("@NRATA", oMyRata.NumeroRata)
    '                End If
    '                MyProcedure = "prc_TBLRATEENTE_D"
    '        End Select
    '        'eseguo la query
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.CommandText = MyProcedure
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        MyIdentity = cmdMyCommand.Parameters("@ID").Value

    '        Return MyIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.SetRataConfigurata.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="nIdAvviso"></param>
    ''' <param name="nIdRata"></param>
    ''' <returns></returns>
    Public Function GetRate(myStringConnection As string,ByVal nIdAvviso As Integer, ByVal nIdRata As Integer) As ObjRata()
        Dim oListRate As New ArrayList
        Dim oMyRata As ObjRata
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRateAvviso", "IDRATA", "IDAVVISO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDRATA", nIdRata) _
                            , ctx.GetParam("IDAVVISO", nIdAvviso)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.GetRate.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyRata = New ObjRata
                    oMyRata.Id = StringOperation.FormatInt(myRow("idrata"))
                    oMyRata.IdAvviso = StringOperation.FormatInt(myRow("idavviso"))
                    oMyRata.sNRata = StringOperation.FormatString(myRow("numero_rata"))
                    oMyRata.sDescrRata = StringOperation.FormatString(myRow("descrizione_rata"))
                    oMyRata.impRata = StringOperation.FormatDouble(myRow("importo_rata"))
                    oMyRata.sCodBollettino = StringOperation.FormatString(myRow("codice_bollettino"))
                    oMyRata.sCodeline = StringOperation.FormatString(myRow("codeline"))
                    oMyRata.sCodiceBarcode = StringOperation.FormatString(myRow("codice_barcode"))
                    oMyRata.sContoCorrente = StringOperation.FormatString(myRow("numero_conto_corrente"))
                    oMyRata.tDataScadenza = StringOperation.FormatDateTime(myRow("data_scadenza"))
                    oMyRata.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                    oMyRata.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                    oMyRata.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))
                    oListRate.Add(oMyRata)
                Next
            End Using

            Return CType(oListRate.ToArray(GetType(ObjRata)), ObjRata())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.GetRate.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function GetRate(ByVal nIdAvviso As Integer, ByVal nIdRata As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjRata()
    '    Dim oListRate As New ArrayList
    '    Dim oMyRata As ObjRata
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IDRATA", nIdRata)
    '        cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", nIdAvviso)
    '        cmdMyCommand.CommandText = "prc_GetRateAvviso"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyRata = New ObjRata
    '            oMyRata.Id = StringOperation.FormatInt(dtMyRow("idrata"))
    '            oMyRata.IdAvviso = StringOperation.FormatInt(dtMyRow("idavviso"))
    '            oMyRata.sNRata = StringOperation.FormatString(dtMyRow("numero_rata"))
    '            oMyRata.sDescrRata = StringOperation.FormatString(dtMyRow("descrizione_rata"))
    '            If Not IsDBNull(dtMyRow("importo_rata")) Then
    '                oMyRata.impRata = StringOperation.FormatDouble(dtMyRow("importo_rata"))
    '            End If
    '            If Not IsDBNull(dtMyRow("codice_bollettino")) Then
    '                oMyRata.sCodBollettino = StringOperation.FormatString(dtMyRow("codice_bollettino"))
    '            End If
    '            If Not IsDBNull(dtMyRow("codeline")) Then
    '                oMyRata.sCodeline = StringOperation.FormatString(dtMyRow("codeline"))
    '            End If
    '            If Not IsDBNull(dtMyRow("codice_barcode")) Then
    '                oMyRata.sCodiceBarcode = StringOperation.FormatString(dtMyRow("codice_barcode"))
    '            End If
    '            If Not IsDBNull(dtMyRow("numero_conto_corrente")) Then
    '                oMyRata.sContoCorrente = StringOperation.FormatString(dtMyRow("numero_conto_corrente"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_scadenza")) Then
    '                oMyRata.tDataScadenza = StringOperation.FormatDateTime(dtMyRow("data_scadenza"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_inserimento")) Then
    '                oMyRata.tDataInserimento = StringOperation.FormatDateTime(dtMyRow("data_inserimento"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyRata.tDataVariazione = StringOperation.FormatDateTime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyRata.tDataCessazione = StringOperation.FormatDateTime(dtMyRow("data_cessazione"))
    '            End If
    '            oListRate.Add(oMyRata)
    '        Next

    '        Return CType(oListRate.ToArray(GetType(ObjRata)), ObjRata())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRata.GetRate.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
End Class
''' <summary>
''' Classe per la gestione del lotto di cartellazione
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestLottoCartellazione
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestLottoCartellazione))
    Private oReplace As New generalClass.generalFunction

    'Public Function GetLottoCartellazione(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sAnno As String, ByVal sConcessione As String, ByVal sCodiceEnte As String, ByVal NumeroCartelle As Integer) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '    Dim oMyLotto As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '    Dim DrDati As SqlClient.SqlDataReader

    '    Try
    '        'valorizzo il lotto
    '        cmdMyCommand.CommandText = "SELECT TOP 1 *"
    '        cmdMyCommand.CommandText += " FROM TBLLOTTICARTELLAZIONE"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        cmdMyCommand.CommandText += " AND (CODICE_CONCESSIONE=@CODCONCESSIONE)"
    '        cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
    '        cmdMyCommand.CommandText += " ORDER BY NUMERO_LOTTO DESC"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONCESSIONE", SqlDbType.NVarChar)).Value = sConcessione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        oMyLotto = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '        oMyLotto.Anno = sAnno
    '        oMyLotto.CodiceConcessione = sConcessione
    '        oMyLotto.DataEmissione = DateTime.Now
    '        oMyLotto.IdEnte = sCodiceEnte
    '        Do While DrDati.Read
    '            oMyLotto.NumeroLotto = StringOperation.Formatint(DrDati("numero_lotto")) + 1
    '            oMyLotto.Primacartella = StringOperation.Formatint(DrDati("ultima_cartella")) + 1
    '        Loop
    '        DrDati.Close()
    '        oMyLotto.UltimaCartella = oMyLotto.Primacartella + NumeroCartelle
    '        oMyLotto.StatoElaborazione = -1
    '        Return oMyLotto
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.GetLottoCartellazione.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function SetLottoCartellazione(ByVal WFSessione As OPENUtility.CreateSessione, ByVal oMyLotto As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione) As Integer
    '    Try
    '        'inserisco il record nel db
    '        cmdMyCommand.CommandText = "INSERT INTO TBLLOTTICARTELLAZIONE (IDENTE, ANNO, CODICE_CONCESSIONE"
    '        cmdMyCommand.CommandText += ", NUMERO_LOTTO, PRIMA_CARTELLA, ULTIMA_CARTELLA"
    '        cmdMyCommand.CommandText += ", DATA_EMISSIONE, STATO_ELABORAZIONE)"
    '        cmdMyCommand.CommandText += " VALUES (@IDENTE,@ANNO,@CODCONCESSIONE"
    '        cmdMyCommand.CommandText += ",@NUMERO_LOTTO,@PRIMA_CARTELLA,@ULTIMA_CARTELLA"
    '        cmdMyCommand.CommandText += ",@DATA_EMISSIONE,@STATO_ELABORAZIONE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyLotto.IdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyLotto.Anno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONCESSIONE", SqlDbType.NVarChar)).Value = oMyLotto.CodiceConcessione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_LOTTO", SqlDbType.Int)).Value = oMyLotto.NumeroLotto
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PRIMA_CARTELLA", SqlDbType.Int)).Value = oMyLotto.Primacartella
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ULTIMA_CARTELLA", SqlDbType.NVarChar)).Value = oMyLotto.UltimaCartella
    '        If oMyLotto.DataEmissione = Date.MinValue Then
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_EMISSIONE", SqlDbType.DateTime)).Value = System.DBNull.Value
    '        Else
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_EMISSIONE", SqlDbType.DateTime)).Value = oMyLotto.DataEmissione
    '        End If
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_ELABORAZIONE", SqlDbType.Int)).Value = oMyLotto.StatoElaborazione
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) <> 1 Then
    '            Return -1
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.SetLottoCartellazione.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Public Function DeleteLottoCartellazione(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sIdEnte As String, ByVal nIdFlusso As Integer) As Integer
    '    Try
    '        'cancello il record nel db
    '        cmdMyCommand.CommandText = "DELETE"
    '        cmdMyCommand.CommandText += " FROM TBLLOTTICARTELLAZIONE"
    '        cmdMyCommand.CommandText += " WHERE (NUMERO_LOTTO IN("
    '        cmdMyCommand.CommandText += "  SELECT LOTTO_CARTELLAZIONE"
    '        cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '        cmdMyCommand.CommandText += "  WHERE (IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO)))"
    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) > 1 Then
    '            Return -1
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.DeleteLottoCartellazione.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sConcessione"></param>
    ''' <param name="sCodiceEnte"></param>
    ''' <param name="NumeroCartelle"></param>
    ''' <returns></returns>
    Public Function GetLottoCartellazione(myStringConnection As String, ByVal sAnno As String, ByVal sConcessione As String, ByVal sCodiceEnte As String, ByVal NumeroCartelle As Integer) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
        Dim oMyLotto As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            'valorizzo il lotto
            oMyLotto = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
            oMyLotto.Anno = sAnno
            oMyLotto.CodiceConcessione = sConcessione
            oMyLotto.DataEmissione = DateTime.Now
            oMyLotto.IdEnte = sCodiceEnte
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLotto", "CODCONCESSIONE", "ANNO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONCESSIONE", sConcessione) _
                            , ctx.GetParam("ANNO", sAnno)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.GetLottoCartellazione.errorQuerye: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyLotto.NumeroLotto = StringOperation.FormatInt(myRow("numero_lotto")) + 1
                    oMyLotto.Primacartella = StringOperation.FormatInt(myRow("ultima_cartella")) + 1
                Next
                oMyLotto.UltimaCartella = oMyLotto.Primacartella + NumeroCartelle
                oMyLotto.StatoElaborazione = -1
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.GetLottoCartellazione.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return oMyLotto
    End Function
    'Public Function GetLottoCartellazione(ByVal sAnno As String, ByVal sConcessione As String, ByVal sCodiceEnte As String, ByVal NumeroCartelle As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '    Dim oMyLotto As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '        End If
    '        'valorizzo il lotto
    '        oMyLotto = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '        oMyLotto.Anno = sAnno
    '        oMyLotto.CodiceConcessione = sConcessione
    '        oMyLotto.DataEmissione = DateTime.Now
    '        oMyLotto.IdEnte = sCodiceEnte
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONCESSIONE", sConcessione)
    '        cmdMyCommand.Parameters.AddWithValue("@Anno", sAnno)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetLotto"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyLotto.NumeroLotto = StringOperation.FormatInt(dtMyRow("numero_lotto")) + 1
    '            oMyLotto.Primacartella = StringOperation.FormatInt(dtMyRow("ultima_cartella")) + 1
    '        Next
    '        oMyLotto.UltimaCartella = oMyLotto.Primacartella + NumeroCartelle
    '        oMyLotto.StatoElaborazione = -1
    '        Return oMyLotto
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.GetLottoCartellazione.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        If ((cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    Public Function SetLottoCartellazione(myStringConnection As String, ByVal oMyLotto As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione) As Integer
        Dim MyIdentity As Integer = -1
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            If oMyLotto.DataEmissione = Date.MinValue Then
                oMyLotto.DataEmissione = Date.MaxValue
            End If
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLLOTTICARTELLAZIONE_IU", "ID", "IDENTE", "ANNO", "CODCONCESSIONE", "NUMERO_LOTTO", "PRIMA_CARTELLA", "ULTIMA_CARTELLA", "DATA_EMISSIONE", "STATO_ELABORAZIONE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", -1) _
                            , ctx.GetParam("IDENTE", oMyLotto.IdEnte) _
                            , ctx.GetParam("ANNO", oMyLotto.Anno) _
                            , ctx.GetParam("CODCONCESSIONE", oMyLotto.CodiceConcessione) _
                            , ctx.GetParam("NUMERO_LOTTO", oMyLotto.NumeroLotto) _
                            , ctx.GetParam("PRIMA_CARTELLA", oMyLotto.Primacartella) _
                            , ctx.GetParam("ULTIMA_CARTELLA", oMyLotto.UltimaCartella) _
                            , ctx.GetParam("DATA_EMISSIONE", oMyLotto.DataEmissione) _
                            , ctx.GetParam("STATO_ELABORAZIONE", oMyLotto.StatoElaborazione)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.SetLottoCartellazione.erroreQuery: ", ex)
                    Return 0
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    MyIdentity = StringOperation.FormatInt(myRow("ID"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.SetLottoCartellazione.errore: ", Err)
            Return 0
        Finally
            myDataView.Dispose()
        End Try
        Return MyIdentity
    End Function
    'Public Function SetLottoCartellazione(ByVal oMyLotto As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim MyProcedure As String = "prc_TBLLOTTICARTELLAZIONE_IU"
    '    Dim MyIdentity As Integer = -1
    '    Dim myTrans As SqlClient.SqlTransaction = Nothing

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            myConnection.Open()
    '            myTrans = myConnection.BeginTransaction
    '            cmdMyCommand.Connection = myConnection
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.Transaction = myTrans
    '        End If
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@ID", -1)
    '        cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyLotto.IdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyLotto.Anno)
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONCESSIONE", oMyLotto.CodiceConcessione)
    '        cmdMyCommand.Parameters.AddWithValue("@NUMERO_LOTTO", oMyLotto.NumeroLotto)
    '        cmdMyCommand.Parameters.AddWithValue("@PRIMA_CARTELLA", oMyLotto.Primacartella)
    '        cmdMyCommand.Parameters.AddWithValue("@ULTIMA_CARTELLA", oMyLotto.UltimaCartella)
    '        If oMyLotto.DataEmissione = Date.MinValue Then
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_EMISSIONE", System.DBNull.Value)
    '        Else
    '            cmdMyCommand.Parameters.AddWithValue("@DATA_EMISSIONE", oMyLotto.DataEmissione)
    '        End If
    '        cmdMyCommand.Parameters.AddWithValue("@STATO_ELABORAZIONE", oMyLotto.StatoElaborazione)
    '        'eseguo la query
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = MyProcedure
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        MyIdentity = cmdMyCommand.Parameters("@ID").Value

    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Commit()
    '        End If
    '        Return MyIdentity
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.SetLottoCartellazione.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        If (cmdMyCommandOut Is Nothing) Then
    '            myTrans.Rollback()
    '        End If
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    Public Function DeleteLottoCartellazione(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal nIdFlusso As Integer) As Integer
        Dim sSQL As String = ""

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = "DELETE"
                    sSQL += " FROM TBLLOTTICARTELLAZIONE"
                    sSQL += " WHERE (NUMERO_LOTTO IN("
                    sSQL += "  SELECT LOTTO_CARTELLAZIONE"
                    sSQL += "  FROM TBLCARTELLE"
                    sSQL += "  WHERE (IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO)))"
                    sSQL += " AND (IDENTE=@IDENTE)"
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                    Return ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDFLUSSO_RUOLO", nIdFlusso)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.DeleteLottoCartellazione.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.DeleteLottoCartellazione.errore: ", Err)
            Return -1
        End Try
    End Function
    'Public Function DeleteLottoCartellazione(ByVal myStringConnection As String, ByVal sIdEnte As String, ByVal nIdFlusso As Integer) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        'cancello il record nel db
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = "DELETE"
    '        cmdMyCommand.CommandText += " FROM TBLLOTTICARTELLAZIONE"
    '        cmdMyCommand.CommandText += " WHERE (NUMERO_LOTTO IN("
    '        cmdMyCommand.CommandText += "  SELECT LOTTO_CARTELLAZIONE"
    '        cmdMyCommand.CommandText += "  FROM TBLCARTELLE"
    '        cmdMyCommand.CommandText += "  WHERE (IDFLUSSO_RUOLO=@IDFLUSSO_RUOLO)))"
    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO_RUOLO", SqlDbType.Int)).Value = nIdFlusso
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        If cmdMyCommand.ExecuteNonQuery > 1 Then
    '            Return -1
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestLottoCartellazione.DeleteLottoCartellazione.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return -1
    '    Finally
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    '*** ***
End Class
''' <summary>
''' Classe per la gestione delle addizionali
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsAddizionali
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsAddizionali))
    Public Function GetAddizionali(ByVal IdAddizionale As String, ByVal DescrAddizionale As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            Dim sSQL As String
            sSQL = "SELECT IDCAPITOLO, DESCRIZIONE FROM TBLADDIZIONALI"
            If IdAddizionale.CompareTo("") <> 0 And IdAddizionale.CompareTo("").ToString() <> "..." Then
                sSQL += " AND IDCATEGORIA='" & IdAddizionale.Replace("'", "''") & "'"
            End If
            If DescrAddizionale.CompareTo("") <> 0 And DescrAddizionale.CompareTo("").ToString() <> "..." Then
                sSQL += " AND DESCRIZIONE='" & DescrAddizionale.Replace("'", "''") & "'"
            End If
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.GetAddizionali.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Throw New Exception("Problemi nell'esecuzione di GetAddizionali " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    Public Function GetAddizionaliEnte(myConnectionString As String, ByVal obj As ObjAddizionali) As DataSet
        Dim sSQL As String = ""
        Dim myDataSet As New DataSet
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetAddizionali", "IDENTE", "ANNO", "CODICE", "TIPORUOLO")
                    myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                            , ctx.GetParam("ANNO", obj.sAnno) _
                            , ctx.GetParam("CODICE", obj.sIDcapitolo) _
                            , ctx.GetParam("TIPORUOLO", "")
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.GetAddizionaliEnte.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                HttpContext.Current.Session("dsAddizionaliEnte") = myDataSet
                Return myDataSet
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.GetAddizionaliEnte.errore: ", Err)
            Return Nothing
        Finally
            myDataSet.Dispose()
        End Try
    End Function
    'Public Function GetAddizionaliEnte(ByVal obj As ObjAddizionali) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet

    '    Try
    '        'eseguo la query
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetAddizionali"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = ConstSession.IdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Codice", SqlDbType.VarChar)).Value = obj.sIDcapitolo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.VarChar)).Value = obj.sAnno
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        HttpContext.Current.Session("dsAddizionaliEnte") = myDataSet
    '        Return myDataSet
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.GetAddizionaliEnte.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Throw New Exception("Problemi nell'esecuzione di GetAddizionaliEnte " + ex.Message)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    '**** 201809 - Cartelle Insoluti ***
    'Public Function GetAddizionaliAssociati(ByVal obj As ObjAddizionali) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet

    '    Try
    '        Dim sSQL As String
    '        sSQL = "SELECT *"
    '        sSQL += " FROM TBLCARTELLEDETTAGLIOVOCI"
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND ANNO=" & obj.sAnno
    '        sSQL += " AND CODICE_CAPITOLO='" & obj.sIDcapitolo & "'"
    '        'eseguo la query
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        Return myDataSet
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.GetAddizionaliAssociati.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Throw New Exception("Problemi nell'esecuzione di GetAddizionaliAssociati " + ex.Message)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    'Public Function SetAddizionali(ByVal obj As ObjAddizionali, ByRef strError As String) As Boolean
    '    Dim myIdentity As Integer
    '    Dim lingua_date As String
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        lingua_date = ConstSession.LinguaDate
    '        SetAddizionali = False
    '        obj.DataInizioValidita = StringOperation.Formatdatetime("01/01/" & obj.sAnno)
    '        obj.DataFineValidita = StringOperation.Formatdatetime("31/12/" & obj.sAnno)

    '        Dim ds As DataSet
    '        ds = GetAddizionaliEnte(obj)
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
    '            SetAddizionali = False
    '            Exit Function
    '        End If

    '        Dim sSQL As String

    '        sSQL = "INSERT INTO TBLADDIZIONALIENTE"
    '        sSQL += " (IDENTE, IDCAPITOLO, ANNO, VALORE, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA)"
    '        sSQL += " VALUES('" & ConstSession.IdEnte & "','" & obj.sIDcapitolo.Replace("'", "''") & "','" & obj.sAnno.Replace("'", "''") & "','" & obj.sValore & "','" & obj.DataInizioValidita.ToString(lingua_date).Replace(".", ":") & "','" & obj.DataFineValidita.ToString(lingua_date).Replace(".", ":") & "')"
    '        'eseguo la query
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myIdentity = cmdMyCommand.ExecuteNonQuery
    '        SetAddizionali = True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.SetAddizionali.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Throw New Exception("Problemi nell'esecuzione di SetAddizionali " + ex.Message)
    '        SetAddizionali = False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    'Public Function UpdateAddizionali(ByVal objOld As ObjAddizionali, ByVal objNew As ObjAddizionali, ByRef strError As String) As Boolean
    '    Dim myIdentity As Integer
    '    Dim lingua_date As String
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        lingua_date = ConstSession.LinguaDate

    '        UpdateAddizionali = False
    '        Dim sSQL As String

    '        'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA TARIFFA NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
    '        Dim CartellazioneGettata As DataSet
    '        CartellazioneGettata = GetAddizionaliAssociati(objOld)
    '        If CartellazioneGettata.Tables(0).Rows.Count > 0 Then
    '            'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
    '            strError = "Impossibile MODIFICARE la posizione selezionata!\nLa cartellazione per l\'Anno della posizione è già stata effettuata!"
    '            UpdateAddizionali = False
    '            Exit Function
    '        End If

    '        objNew.DataInizioValidita = StringOperation.Formatdatetime("01/01/" & objNew.sAnno)
    '        objNew.DataFineValidita = StringOperation.Formatdatetime("31/12/" & objNew.sAnno)

    '        'PRELEVO I DATI DELLA TARIFFA DA MODIFICARE
    '        Dim DST As New DataSet

    '        DST = GetAddizionaliEnte(objOld)
    '        If DST.Tables(0).Rows.Count > 0 Then
    '            objOld.ID = StringOperation.FormatString(DST.Tables(0).Rows(0)("IDADDIZIONALE"))
    '            objOld.sValore = StringOperation.FormatString(DST.Tables(0).Rows(0)("percentuale"))
    '            objOld.sAnno = StringOperation.FormatString(DST.Tables(0).Rows(0)("anno"))
    '            objOld.sIDcapitolo = StringOperation.FormatString(DST.Tables(0).Rows(0)("IDCAPITOLO"))
    '        Else
    '            strError = "Non sono presenti dati per l\'Addizionale Selezionato."
    '            UpdateAddizionali = False
    '            Exit Function
    '        End If
    '        DST.Dispose()
    '        'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
    '        If objOld.sAnno <> objNew.sAnno Then 'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then
    '            'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
    '            'se è presente blocco l'operazione
    '            Dim ds As DataSet
    '            ds = GetAddizionaliEnte(objNew)
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
    '                UpdateAddizionali = False
    '                Exit Function
    '            End If
    '            ds.Dispose()

    '            'verifico se è già presente una tariffa con gli stessi dati storicizzata in data odierna
    '            'se è presente do il messaggio e blocco l'operazione
    '            'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
    '            sSQL = "UPDATE TBLADDIZIONALIENTE SET ANNO='" & objNew.sAnno & "', DATA_INIZIO_VALIDITA='" & objNew.DataInizioValidita.ToString(lingua_date).Replace(".", ":") & "', DATA_FINE_VALIDITA='" & objNew.DataFineValidita.ToString(lingua_date).Replace(".", ":") & "',"
    '            sSQL += " VALORE='" & objNew.sValore & "'"
    '            sSQL += " WHERE TBLADDIZIONALIENTE.ID=" & objOld.ID & ""
    '            sSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.Text
    '            cmdMyCommand.CommandText = sSQL
    '            myIdentity = cmdMyCommand.ExecuteNonQuery
    '        ElseIf objNew.sValore <> objOld.sValore And objOld.sAnno = objNew.sAnno Then
    '            'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
    '            'SQL = "UPDATE TBLADDIZIONALIENTE SET VALORE='" & objNew.sValore.Replace(",", ".") & "'"
    '            sSQL = "UPDATE TBLADDIZIONALIENTE SET VALORE='" & objNew.sValore & "'"
    '            sSQL += " WHERE ID=" & objNew.ID & ""
    '            sSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
    '            'eseguo la query
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.Text
    '            cmdMyCommand.CommandText = sSQL
    '            myIdentity = cmdMyCommand.ExecuteNonQuery
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.UpdateAddizionali.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Throw New Exception("Problemi nell'esecuzione di UpdateAddizionali " + ex.Message)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    Public Function GetAddizionaliAssociati(ByVal obj As ObjAddizionali) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            Dim sSQL As String
            sSQL = "SELECT C.ID"
            sSQL += " FROM TBLCARTELLE C"
            sSQL += " INNER JOIN TBLRUOLI_GENERATI R ON C.IDFLUSSO_RUOLO=R.IDFLUSSO AND C.IDENTE=R.IDENTE"
            sSQL += " INNER JOIN TBLCARTELLEDETTAGLIOVOCI CD ON C.ID=CD.IDAVVISO"
            sSQL += " WHERE 1=1"
            sSQL += " AND YEAR(CASE WHEN C.DATA_VARIAZIONE IS NULL THEN '99991231' ELSE C.DATA_VARIAZIONE END)=9999"
            sSQL += " AND C.ANNO=" & obj.sAnno
            sSQL += " AND CD.CODICE_CAPITOLO='" & obj.sIDcapitolo & "'"
            'eseguo la query
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.GetAddizionaliAssociati.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Throw New Exception("Problemi nell'esecuzione di GetAddizionaliAssociati " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    Public Function SetAddizionali(ByVal obj As ObjAddizionali, TipoCalcolo As Integer, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Dim lingua_date As String
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            lingua_date = ConstSession.LinguaDate
            SetAddizionali = False
            obj.DataInizioValidita = StringOperation.FormatDateTime("01/01/" & obj.sAnno)
            obj.DataFineValidita = StringOperation.FormatDateTime("31/12/" & obj.sAnno)

            Dim ds As DataSet
            ds = GetAddizionaliEnte(ConstSession.StringConnection, obj)
            If ds.Tables(0).Rows.Count > 0 Then
                strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
                SetAddizionali = False
                Exit Function
            End If

            Dim sSQL As String

            sSQL = "INSERT INTO TBLADDIZIONALIENTE"
            sSQL += " (IDENTE, IDCAPITOLO, ANNO, VALORE, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA,TIPOCALCOLO)"
            sSQL += " VALUES('" & ConstSession.IdEnte & "','" & obj.sIDcapitolo.Replace("'", "''") & "','" & obj.sAnno.Replace("'", "''") & "','" & obj.sValore & "','" & obj.DataInizioValidita.ToString(lingua_date).Replace(".", ":") & "','" & obj.DataFineValidita.ToString(lingua_date).Replace(".", ":") & "'," & TipoCalcolo & ")"
            'eseguo la query
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myIdentity = cmdMyCommand.ExecuteNonQuery
            SetAddizionali = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.SetAddizionali.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Throw New Exception("Problemi nell'esecuzione di SetAddizionali " + ex.Message)
            SetAddizionali = False
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    'Public Function SetAddizionali(ByVal obj As ObjAddizionali, TipoCalcolo As Integer, ByRef strError As String) As Boolean
    '    Dim myIdentity As Integer
    '    Dim lingua_date As String
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        lingua_date = ConstSession.LinguaDate
    '        SetAddizionali = False
    '        obj.DataInizioValidita = StringOperation.FormatDateTime("01/01/" & obj.sAnno)
    '        obj.DataFineValidita = StringOperation.FormatDateTime("31/12/" & obj.sAnno)

    '        Dim ds As DataSet
    '        ds = GetAddizionaliEnte(obj)
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la Posizione presente."
    '            SetAddizionali = False
    '            Exit Function
    '        End If

    '        Dim sSQL As String

    '        sSQL = "INSERT INTO TBLADDIZIONALIENTE"
    '        sSQL += " (IDENTE, IDCAPITOLO, ANNO, VALORE, DATA_INIZIO_VALIDITA, DATA_FINE_VALIDITA,TIPOCALCOLO)"
    '        sSQL += " VALUES('" & ConstSession.IdEnte & "','" & obj.sIDcapitolo.Replace("'", "''") & "','" & obj.sAnno.Replace("'", "''") & "','" & obj.sValore & "','" & obj.DataInizioValidita.ToString(lingua_date).Replace(".", ":") & "','" & obj.DataFineValidita.ToString(lingua_date).Replace(".", ":") & "'," & TipoCalcolo & ")"
    '        'eseguo la query
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myIdentity = cmdMyCommand.ExecuteNonQuery
    '        SetAddizionali = True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.SetAddizionali.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Throw New Exception("Problemi nell'esecuzione di SetAddizionali " + ex.Message)
    '        SetAddizionali = False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    Public Function UpdateAddizionali(ByVal objOld As ObjAddizionali, ByVal objNew As ObjAddizionali, TipoCalcolo As Integer, ByRef strError As String) As Boolean
        Dim myIdentity As Integer
        Dim lingua_date As String
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            lingua_date = ConstSession.LinguaDate

            UpdateAddizionali = False
            Dim sSQL As String

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA TARIFFA NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
            Dim CartellazioneGettata As DataSet
            CartellazioneGettata = GetAddizionaliAssociati(objOld)
            If CartellazioneGettata.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile MODIFICARE la posizione selezionata!\nLa cartellazione per l\'Anno della posizione è già stata effettuata!"
                UpdateAddizionali = False
                Exit Function
            End If

            objNew.DataInizioValidita = StringOperation.FormatDateTime("01/01/" & objNew.sAnno)
            objNew.DataFineValidita = StringOperation.FormatDateTime("31/12/" & objNew.sAnno)

            'PRELEVO I DATI DELLA TARIFFA DA MODIFICARE
            Dim DST As New DataSet

            DST = GetAddizionaliEnte(ConstSession.StringConnection, objOld)
            If DST.Tables(0).Rows.Count > 0 Then
                objOld.ID = StringOperation.FormatString(DST.Tables(0).Rows(0)("ID"))
                objOld.sValore = StringOperation.FormatString(DST.Tables(0).Rows(0)("valore"))
                objOld.sIDcapitolo = StringOperation.FormatString(DST.Tables(0).Rows(0)("codice"))
                objOld.sAnno = StringOperation.FormatString(DST.Tables(0).Rows(0)("anno"))
            Else
                strError = "Non sono presenti dati per l\'Addizionale Selezionato."
                UpdateAddizionali = False
                Exit Function
            End If
            DST.Dispose()
            'SE L'IDCATEGORIA è CAMBIATO o è CAMBIATA LA DATA_INIZIO_VALIDITA
            Try
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
                cmdMyCommand.CommandTimeout = 0
                cmdMyCommand.CommandType = CommandType.Text
                If objOld.sAnno <> objNew.sAnno Then 'And DescrizioneOld <> DescrizioneNew And DataInizioOld <> DataInizioNew Then
                    'verifico se è già presente una tariffa con data fine validita valorizzata per i dati inseriti
                    'se è presente blocco l'operazione
                    Dim ds As DataSet
                    ds = GetAddizionaliEnte(ConstSession.StringConnection, objNew)
                    If ds.Tables(0).Rows.Count > 0 Then
                        strError = "E\' già presente una posizione per i dati Inseriti. Aggiornare la posizione presente."
                        UpdateAddizionali = False
                        Exit Function
                    End If
                    ds.Dispose()

                    'verifico se è già presente una tariffa con gli stessi dati storicizzata in data odierna
                    'se è presente do il messaggio e blocco l'operazione
                    'METTO DATA_FINE_VALIDITA AL RECORD VECCHHIO
                    sSQL = "UPDATE TBLADDIZIONALIENTE SET ANNO='" & objNew.sAnno & "', DATA_INIZIO_VALIDITA='" & objNew.DataInizioValidita.ToString(lingua_date).Replace(".", ":") & "', DATA_FINE_VALIDITA='" & objNew.DataFineValidita.ToString(lingua_date).Replace(".", ":") & "',"
                    sSQL += " CODICE='" & objNew.sIDcapitolo & "', VALORE='" & objNew.sValore & "',TIPOCALCOLO=" & TipoCalcolo
                    sSQL += " WHERE TBLADDIZIONALIENTE.ID=" & objOld.ID & ""
                    sSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
                Else
                    'ALTRIMENTI SE CAMBIA SOLO LA DESCRIZIONE AGGIORNO LA DESCRIZIONE
                    'SQL = "UPDATE TBLADDIZIONALIENTE SET VALORE='" & objNew.sValore.Replace(",", ".") & "'"
                    sSQL = "UPDATE TBLADDIZIONALIENTE SET CODICE='" & objNew.sIDcapitolo & "', VALORE='" & objNew.sValore & "',TIPOCALCOLO=" & TipoCalcolo
                    sSQL += " WHERE ID=" & objNew.ID & ""
                    sSQL += " AND IDENTE='" & ConstSession.IdEnte & "'"
                End If
                'eseguo la query
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myIdentity = cmdMyCommand.ExecuteNonQuery
                Return True
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.UpdateAddizionali.errore: ", ex)
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                Throw New Exception("Problemi nell'esecuzione di UpdateAddizionali " + ex.Message)
            Finally
                cmdMyCommand.Connection.Close()
                cmdMyCommand.Dispose()
            End Try
        Catch err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.UpdateAddizionali.errore: ", err)
            Throw New Exception("Problemi nell'esecuzione di UpdateAddizionali " + err.Message)
        End Try
    End Function
    Public Function DeleteAddizionale(ByVal OBJ As ObjAddizionali, ByRef strError As String) As Boolean
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            DeleteAddizionale = False
            Dim sSQL As String

            'PRIMA DI ELIMINARE DEVO VERIFICARE CHE LA TARIFFA NON SIA STATA ASSOCIATA AD UN ARTICOLO A RUOLO
            Dim CartellazioneGettata As New DataSet
            CartellazioneGettata = GetAddizionaliAssociati(OBJ)
            If CartellazioneGettata.Tables(0).Rows.Count > 0 Then
                'SE è STATA ASSOCIATA BLOCCO L'OPERAZIONE
                strError = "Impossibile ELIMINARE la posizione selezionata! La cartellazione per l\'Anno della posizione è già stata effettuata!"
                Return False
            End If

            'SE NON è STATA ASSOCIATA LA CANCELLO
            sSQL = "DELETE FROM TBLADDIZIONALIENTE"
            sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                ctx.ExecuteNonQuery(sSQL)
                ctx.Dispose()
            End Using

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.DeleteAddizionale.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeleteAddizionale " + ex.Message)
        End Try
    End Function
    Public Sub LoadComboAddizionali(ByVal ddlAddizionali As DropDownList)
        Try
            Dim SQL As String

            SQL = "SELECT IDCAPITOLO + ' - ' + DESCRIZIONE AS DESCRIZIONE, IDCAPITOLO FROM TBLADDIZIONALI ORDER BY DESCRIZIONE"
            Dim oLoadCombo As New generalClass.generalFunction
            oLoadCombo.LoadComboGenerale(ddlAddizionali, SQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAddizionali.LoadComboAddizionali.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di LoadComboAddizionali " + ex.Message)
        End Try
    End Sub
End Class
''' <summary>
''' Definizione oggetto addizionali
''' </summary>
Public Class ObjAddizionali
    Dim _id As Integer = 0
    Dim _idCapitolo As String = ""
    Dim _anno As String = ""
    Dim _datainiziovalidita As Date = Date.MinValue
    Dim _datafinevalidita As Date = Date.MinValue
    Dim _valore As String = ""

    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal Value As Integer)
            _id = Value
        End Set
    End Property
    Public Property sIDcapitolo() As String
        Get
            Return _idCapitolo
        End Get
        Set(ByVal Value As String)
            _idCapitolo = Value
        End Set
    End Property

    Public Property sAnno() As String
        Get
            Return _anno
        End Get
        Set(ByVal Value As String)
            _anno = Value
        End Set
    End Property

    Public Property sValore() As String
        Get
            Return _valore
        End Get
        Set(ByVal Value As String)
            _valore = Value
        End Set
    End Property

    Public Property DataInizioValidita() As Date
        Get
            Return _datainiziovalidita
        End Get
        Set(ByVal Value As Date)
            _datainiziovalidita = Value
        End Set
    End Property

    Public Property DataFineValidita() As Date
        Get
            Return _datafinevalidita
        End Get
        Set(ByVal Value As Date)
            _datafinevalidita = Value
        End Set
    End Property
End Class
''' <summary>
''' Classe per la gestione dei vani
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsVani
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsVani))
    Public Function GetVani(ByVal IdVano As String) As DataSet
        Dim sSQL As String = ""
        Dim myDataSet As New DataSet

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = "SELECT IDTIPOVANO, DESCRIZIONE"
                    sSQL += " FROM TBLTIPOVANI"
                    sSQL += " WHERE IDENTE=@IDENTE"
                    sSQL += " ORDER BY DESCRIZIONE"
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.View, "prc_GetAvviso", "IDENTE")
                    myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte))
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + " - OPENgovTIA.ClsVani.GetVani.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + " - OPENgovTIA.ClsVani.GetVani.errore: ", Err)
            Throw New Exception("Problemi nell'esecuzione di GetVani " + Err.Message)
        End Try
        Return myDataSet
    End Function
    'Public Function GetVani(ByVal IdVano As String) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet

    '    Try
    '        'eseguo la query
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = "SELECT IDTIPOVANO, DESCRIZIONE"
    '        cmdMyCommand.CommandText += " FROM TBLTIPOVANI"
    '        cmdMyCommand.CommandText += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
    '        cmdMyCommand.CommandText += " ORDER BY DESCRIZIONE"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        HttpContext.Current.Session("dsAddizionaliEnte") = myDataSet
    '        Return myDataSet
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsVani.GetVani.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Throw New Exception("Problemi nell'esecuzione di GetVani " + ex.Message)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
End Class
