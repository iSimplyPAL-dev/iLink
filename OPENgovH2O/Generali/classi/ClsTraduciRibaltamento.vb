Imports AnagInterface
Imports Anater.Oggetti
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Public Class ClsTraduciRibaltamento
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsTraduciRibaltamento))
    Private iDB As New DBAccess.getDBobject

    Public Function TraduciAnagraficaAnater(ByVal oAnagrafica As DettaglioAnagrafica) As Anater.Oggetti.AnagraficaH2O
        Dim oAnagraficaH2O As New Anater.Oggetti.AnagraficaH2O

        Try
            oAnagraficaH2O.CapResidenza = oAnagrafica.CapResidenza
            oAnagraficaH2O.CivicoResidenza = oAnagrafica.CivicoResidenza
            oAnagraficaH2O.COD_CONTRIBUENTE = oAnagrafica.COD_CONTRIBUENTE
            oAnagraficaH2O.CodContribuenteRappLegale = oAnagrafica.CodContribuenteRappLegale
            oAnagraficaH2O.CodEnte = oAnagrafica.CodEnte
            oAnagraficaH2O.CodFamiglia = oAnagrafica.CodFamiglia
            oAnagraficaH2O.CodiceComuneNascita = oAnagrafica.CodiceComuneNascita
            oAnagraficaH2O.CodiceComuneResidenza = oAnagrafica.CodiceComuneResidenza
            oAnagraficaH2O.CodiceFiscale = oAnagrafica.CodiceFiscale
            oAnagraficaH2O.CodIndividuale = oAnagrafica.CodIndividuale
            oAnagraficaH2O.CodViaResidenza = oAnagrafica.CodViaResidenza
            oAnagraficaH2O.Cognome = oAnagrafica.Cognome
            oAnagraficaH2O.ComuneNascita = oAnagrafica.ComuneNascita
            oAnagraficaH2O.ComuneResidenza = oAnagrafica.ComuneResidenza
            oAnagraficaH2O.Concurrency = oAnagrafica.Concurrency
            oAnagraficaH2O.DaRicontrollare = oAnagrafica.DaRicontrollare
            oAnagraficaH2O.DataFineValidita = oAnagrafica.DataFineValidita
            oAnagraficaH2O.DataInizioValidita = oAnagrafica.DataInizioValidita
            oAnagraficaH2O.DataMorte = oAnagrafica.DataMorte
            oAnagraficaH2O.DataNascita = oAnagrafica.DataNascita
            oAnagraficaH2O.DataUltimaModifica = oAnagrafica.DataUltimaModifica
            oAnagraficaH2O.DataUltimoAggAnagrafe = oAnagrafica.DataUltimoAggAnagrafe
            oAnagraficaH2O.DatiRiferimento = oAnagrafica.DatiRiferimento
            oAnagraficaH2O.dsContatti = Nothing
            oAnagraficaH2O.dsTipiContatti = Nothing
            oAnagraficaH2O.EsponenteCivicoResidenza = oAnagrafica.EsponenteCivicoResidenza
            oAnagraficaH2O.FrazioneResidenza = oAnagrafica.FrazioneResidenza
            oAnagraficaH2O.ID = oAnagrafica.ID
            oAnagraficaH2O.ID_DATA_ANAGRAFICA = oAnagrafica.ID_DATA_ANAGRAFICA
            oAnagraficaH2O.InternoCivicoResidenza = oAnagrafica.InternoCivicoResidenza
            oAnagraficaH2O.NazionalitaNascita = oAnagrafica.NazionalitaNascita
            oAnagraficaH2O.NazionalitaResidenza = oAnagrafica.NazionalitaResidenza
            oAnagraficaH2O.NCAnagraficaRes = oAnagrafica.NCAnagraficaRes
            oAnagraficaH2O.Nome = oAnagrafica.Nome
            oAnagraficaH2O.Note = oAnagrafica.Note
            oAnagraficaH2O.NucleoFamiliare = oAnagrafica.NucleoFamiliare
            oAnagraficaH2O.Operatore = oAnagrafica.Operatore
            oAnagraficaH2O.PartitaIva = oAnagrafica.PartitaIva
            oAnagraficaH2O.PosizioneCivicoResidenza = oAnagrafica.PosizioneCivicoResidenza
            oAnagraficaH2O.Professione = oAnagrafica.Professione
            oAnagraficaH2O.ProvinciaNascita = oAnagrafica.ProvinciaNascita
            oAnagraficaH2O.ProvinciaResidenza = oAnagrafica.ProvinciaResidenza
            oAnagraficaH2O.RappresentanteLegale = oAnagrafica.RappresentanteLegale
            oAnagraficaH2O.ScalaCivicoResidenza = oAnagrafica.ScalaCivicoResidenza
            oAnagraficaH2O.Sesso = oAnagrafica.Sesso
            oAnagraficaH2O.TipoRiferimento = oAnagrafica.TipoRiferimento
            oAnagraficaH2O.ViaResidenza = oAnagrafica.ViaResidenza
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            'oAnagraficaH2O.CapRCP = oAnagrafica.CapRCP
            'oAnagraficaH2O.CivicoRCP = oAnagrafica.CivicoRCP
            'oAnagraficaH2O.CodComuneRCP = oAnagrafica.CodComuneRCP
            'oAnagraficaH2O.CodTributo = oAnagrafica.CodTributo
            'oAnagraficaH2O.CodViaRCP = oAnagrafica.CodViaRCP
            'oAnagraficaH2O.CognomeInvio = oAnagrafica.CognomeInvio
            'oAnagraficaH2O.ComuneRCP = oAnagrafica.ComuneRCP
            'oAnagraficaH2O.DataFineValiditaSpedizione = oAnagrafica.DataFineValiditaSpedizione
            'oAnagraficaH2O.DataInizioValiditaSpedizione = oAnagrafica.DataInizioValiditaSpedizione
            'oAnagraficaH2O.DataUltimaModificaSpedizione = oAnagrafica.DataUltimaModificaSpedizione
            'oAnagraficaH2O.EsponenteCivicoRCP = oAnagrafica.EsponenteCivicoRCP
            'oAnagraficaH2O.FrazioneRCP = oAnagrafica.FrazioneRCP
            'oAnagraficaH2O.ID_DATA_SPEDIZIONE = oAnagrafica.ID_DATA_SPEDIZIONE
            'oAnagraficaH2O.InternoCivicoRCP = oAnagrafica.InternoCivicoRCP
            'oAnagraficaH2O.LocRCP = oAnagrafica.LocRCP
            'oAnagraficaH2O.NomeInvio = oAnagrafica.NomeInvio
            'oAnagraficaH2O.OperatoreSpedizione = oAnagrafica.OperatoreSpedizione
            'oAnagraficaH2O.PosizioneCivicoRCP = oAnagrafica.PosizioneCivicoRCP
            'oAnagraficaH2O.ProvinciaRCP = oAnagrafica.ProvinciaRCP
            'oAnagraficaH2O.ScalaCivicoRCP = oAnagrafica.ScalaCivicoRCP
            'oAnagraficaH2O.ViaRCP = oAnagrafica.ViaRCP
            For Each mySped As ObjIndirizziSpedizione In oAnagrafica.ListSpedizioni
                If mySped.CodTributo = "9000" Then
                    oAnagraficaH2O.CapRCP = mySped.CapRCP
                    oAnagraficaH2O.CivicoRCP = mySped.CivicoRCP
                    oAnagraficaH2O.CodComuneRCP = mySped.CodComuneRCP
                    oAnagraficaH2O.CodTributo = mySped.CodTributo
                    oAnagraficaH2O.CodViaRCP = mySped.CodViaRCP
                    oAnagraficaH2O.CognomeInvio = mySped.CognomeInvio
                    oAnagraficaH2O.ComuneRCP = mySped.ComuneRCP
                    oAnagraficaH2O.DataFineValiditaSpedizione = mySped.DataFineValiditaSpedizione
                    oAnagraficaH2O.DataInizioValiditaSpedizione = mySped.DataInizioValiditaSpedizione
                    oAnagraficaH2O.DataUltimaModificaSpedizione = mySped.DataUltimaModificaSpedizione
                    oAnagraficaH2O.EsponenteCivicoRCP = mySped.EsponenteCivicoRCP
                    oAnagraficaH2O.FrazioneRCP = mySped.FrazioneRCP
                    oAnagraficaH2O.ID_DATA_SPEDIZIONE = mySped.ID_DATA_SPEDIZIONE
                    oAnagraficaH2O.InternoCivicoRCP = mySped.InternoCivicoRCP
                    oAnagraficaH2O.LocRCP = mySped.LocRCP
                    oAnagraficaH2O.NomeInvio = mySped.NomeInvio
                    oAnagraficaH2O.OperatoreSpedizione = mySped.OperatoreSpedizione
                    oAnagraficaH2O.PosizioneCivicoRCP = mySped.PosizioneCivicoRCP
                    oAnagraficaH2O.ProvinciaRCP = mySped.ProvinciaRCP
                    oAnagraficaH2O.ScalaCivicoRCP = mySped.ScalaCivicoRCP
                    oAnagraficaH2O.ViaRCP = mySped.ViaRCP
                End If
            Next
            '*** ***
            Return oAnagraficaH2O
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTraduciRibaltamento.TraduciAnagraficaAnater.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function TraduciContatore(ByVal objContatore As objContatore, ByVal dsCatasto As DataSet, ByVal codiceFiscale As String) As Anater.Oggetti.DatiContatore

        Dim objDichContatoreAnater As New Anater.Oggetti.DatiContatore
        Dim objDatiCatastaliList() As Anater.Oggetti.DatiCatastali = Nothing
        Dim objDatiCatastaliSingolo As Anater.Oggetti.DatiCatastali
        Dim i As Integer

        objDichContatoreAnater.bEsenteAcqua = objContatore.bEsenteAcqua
        objDichContatoreAnater.bEsenteDepurazione = objContatore.bEsenteDepurazione
        objDichContatoreAnater.bEsenteFognatura = objContatore.bEsenteFognatura
        objDichContatoreAnater.bIgniraMora = objContatore.bIgnoraMora
        objDichContatoreAnater.bUtenteSospeso = objContatore.bUtenteSospeso
        objDichContatoreAnater.nCodIntestatario = objContatore.nIdIntestatario
        objDichContatoreAnater.nCodIva = objContatore.nCodIva
        objDichContatoreAnater.nCodUtente = objContatore.nIdUtente
        objDichContatoreAnater.nConEnte = objContatore.sIdEnte
        objDichContatoreAnater.nDepurazione = objContatore.nCodDepurazione
        objDichContatoreAnater.nDiametroCont = objContatore.nDiametroContatore
        objDichContatoreAnater.nDiametroPresa = objContatore.nDiametroPresa
        objDichContatoreAnater.nFognatura = objContatore.nCodFognatura
        objDichContatoreAnater.nGiro = objContatore.nGiro
        objDichContatoreAnater.nIdAttivita = objContatore.nIdAttivita
        objDichContatoreAnater.nidCodContratto = objContatore.nIdContratto
        objDichContatoreAnater.nIdImpianto = objContatore.nIdImpianto
        objDichContatoreAnater.nIdMinimo = objContatore.nIdMinimo
        objDichContatoreAnater.nIdVia = objContatore.nIdVia
        objDichContatoreAnater.nPosizione = objContatore.nPosizione
        objDichContatoreAnater.nTipoContatore = objContatore.nTipoContatore
        objDichContatoreAnater.nTipoUtenza = objContatore.nTipoUtenza
        objDichContatoreAnater.proprietario = objContatore.nProprietario
        objDichContatoreAnater.sCivico = objContatore.sCivico
        objDichContatoreAnater.sCodEnteAppartenenza = objContatore.sIdEnteAppartenenza
        objDichContatoreAnater.sCodiceFabbricante = objContatore.sCodiceFabbricante
        objDichContatoreAnater.sCodiceISTAT = objContatore.sCodiceISTAT
        objDichContatoreAnater.sConsumoMinimo = objContatore.nConsumoMinimo
        objDichContatoreAnater.sContatorePrec = objContatore.nIdContatorePrec
        objDichContatoreAnater.sContatoreSucc = objContatore.nIdContatoreSucc
        objDichContatoreAnater.sDataAttivazione = objContatore.sDataAttivazione
        objDichContatoreAnater.sDataCessazione = objContatore.sDataCessazione
        objDichContatoreAnater.sDataRimpTemp = objContatore.sDataRimTemp
        objDichContatoreAnater.sDataSospsensioneUtenza = objContatore.sDataSospensioneUtenza
        objDichContatoreAnater.sDataSostituzione = objContatore.sDataSostituzione
        objDichContatoreAnater.sDiritti = objContatore.nDiritti
        objDichContatoreAnater.sEsponente = objContatore.sEsponenteCivico
        objDichContatoreAnater.sLatoStrada = objContatore.sLatoStrada
        objDichContatoreAnater.sMatricola = objContatore.sMatricola
        objDichContatoreAnater.sNewCodContatore = objContatore.nIdContatore
        objDichContatoreAnater.sNote = objContatore.sNote
        objDichContatoreAnater.sNumeroCifreContatore = objContatore.sCifreContatore
        objDichContatoreAnater.sNumeroUtente = objContatore.sNumeroUtente
        objDichContatoreAnater.sNumeroUtenze = objContatore.nNumeroUtenze
        objDichContatoreAnater.sPenalita = objContatore.sPenalita
        objDichContatoreAnater.spendente = objContatore.bIsPendente
        objDichContatoreAnater.sProgressivo = objContatore.sProgressivo
        objDichContatoreAnater.sQuoteAgevolate = objContatore.sQuoteAgevolate
        objDichContatoreAnater.sSequenza = objContatore.sSequenza
        objDichContatoreAnater.sSpesa = objContatore.nSpesa
        objDichContatoreAnater.sStatoContatore = objContatore.sStatoContatore
        'objDichContatoreAnater.subAssociato = objContatore.nIDSubAssociato
        objDichContatoreAnater.sUbicazione = objContatore.sUbicazione

        objDichContatoreAnater.bLasciatoAvviso = 0
        objDichContatoreAnater.bLetto = 0
        objDichContatoreAnater.bSubContatore = 0
        objDichContatoreAnater.nConsumoStimato = 0
        objDichContatoreAnater.nCodLetturista = 0
        objDichContatoreAnater.nIpublicpianto1 = 0
        objDichContatoreAnater.bAcquisito = 0
        objDichContatoreAnater.bAnomalia = 0
        objDichContatoreAnater.provenienza = ""
        objDichContatoreAnater.sCodicePuntoPresa = ""
        objDichContatoreAnater.sCodiceUtenteEsterno = ""
        objDichContatoreAnater.sCognomePropFabbricato = ""
        objDichContatoreAnater.sDataControllo = ""
        objDichContatoreAnater.sFrazioneUbicazione = ""
        objDichContatoreAnater.sDataPassaggio = ""
        objDichContatoreAnater.sMatricolaNumerica = ""
        objDichContatoreAnater.sNomePropFabbricato = ""
        Try

            Dim x As Integer = 0

            If dsCatasto.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsCatasto.Tables(0).Rows.Count - 1
                    objDatiCatastaliSingolo = New Anater.Oggetti.DatiCatastali
                    objDatiCatastaliSingolo = TraduciDatiCatastali(dsCatasto.Tables(0).Rows(i)("IDCONT_CATAS"),
                    dsCatasto.Tables(0).Rows(i)("CODCONTATORE"), dsCatasto.Tables(0).Rows(i)("INTERNO"),
                    dsCatasto.Tables(0).Rows(i)("PIANO"), dsCatasto.Tables(0).Rows(i)("FOGLIO"),
                    dsCatasto.Tables(0).Rows(i)("NUMERO"), dsCatasto.Tables(0).Rows(i)("SUBALTERNO"), codiceFiscale)

                    ReDim Preserve objDatiCatastaliList(x)
                    objDatiCatastaliList(x) = objDatiCatastaliSingolo

                    x += 1
                Next
            End If

            objDichContatoreAnater.oDatiCatastali = objDatiCatastaliList

            'objDichContatoreAnater.dMinimoFatturabile = 0
            'objDichContatoreAnater.bDaNonConsiderare = 0
            'objDichContatoreAnater.bDaRicontrollare = 0
            'objDichContatoreAnater.bEstratto = 0
            'objDichContatoreAnater.bModuloAutoLettura = 0
            'objDichContatoreAnater.bScaricatoSuPDA = 0
            'objDichContatoreAnater.bSmat = 0

            Return objDichContatoreAnater
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTraduciRibaltamento.TraduciContatore.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function TraduciContatoreAnater(ByVal dsContatore As DataSet, ByVal dsCatasto As DataSet, ByVal codiceFiscale As String) As Anater.Oggetti.DatiContatore

        Dim objDichContatoreAnater As New Anater.Oggetti.DatiContatore
        Dim objDatiCatastaliList() As Anater.Oggetti.DatiCatastali = Nothing
        Dim objDatiCatastaliSingolo As Anater.Oggetti.DatiCatastali
        Dim i As Integer
        Dim ClsData As New ClsGenerale.Generale

        Try
            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("ESENTEACQUA")) Then
                If dsContatore.Tables(0).Rows(0)("ESENTEACQUA").ToString() = False Then
                    objDichContatoreAnater.bEsenteAcqua = 0
                Else
                    objDichContatoreAnater.bEsenteAcqua = 1
                End If
            Else
                objDichContatoreAnater.bEsenteAcqua = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("ESENTEDEPURAZIONE")) Then
                If dsContatore.Tables(0).Rows(0)("ESENTEDEPURAZIONE").ToString() = False Then
                    objDichContatoreAnater.bEsenteDepurazione = 0
                Else
                    objDichContatoreAnater.bEsenteDepurazione = 1
                End If
            Else
                objDichContatoreAnater.bEsenteDepurazione = 0
            End If

            If dsContatore.Tables(0).Rows(0)("ESENTEFOGNATURA").ToString() = False Then
                objDichContatoreAnater.bEsenteFognatura = 0
            Else
                objDichContatoreAnater.bEsenteFognatura = 1
            End If

            If dsContatore.Tables(0).Rows(0)("IGNORAMORA").ToString() = False Then
                objDichContatoreAnater.bIgniraMora = 0
            Else
                objDichContatoreAnater.bEsenteFognatura = 1
            End If

            If dsContatore.Tables(0).Rows(0)("UTENTESOSPESO").ToString() = False Then
                objDichContatoreAnater.bUtenteSospeso = 0
            Else
                objDichContatoreAnater.bUtenteSospeso = 1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODIVA")) Then
                If dsContatore.Tables(0).Rows(0)("CODIVA").ToString() = False Then
                    objDichContatoreAnater.nCodIva = 0
                Else
                    objDichContatoreAnater.nCodIva = 1
                End If
            Else
                objDichContatoreAnater.nCodIva = -1
            End If

            objDichContatoreAnater.nCodUtente = 0

            objDichContatoreAnater.nConEnte = CInt(dsContatore.Tables(0).Rows(0)("CODENTE").ToString())

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODDEPURAZIONE")) Then
                objDichContatoreAnater.nDepurazione = CInt(dsContatore.Tables(0).Rows(0)("CODDEPURAZIONE").ToString())
            Else
                objDichContatoreAnater.nDepurazione = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODDIAMETROCONTATORE")) Then
                objDichContatoreAnater.nDiametroCont = CInt(dsContatore.Tables(0).Rows(0)("CODDIAMETROCONTATORE").ToString())
            Else
                objDichContatoreAnater.nDiametroCont = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODDIAMETROPRESA")) Then
                objDichContatoreAnater.nDiametroPresa = CInt(dsContatore.Tables(0).Rows(0)("CODDIAMETROPRESA").ToString())
            Else
                objDichContatoreAnater.nDiametroPresa = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODFOGNATURA")) Then
                objDichContatoreAnater.nFognatura = CInt(dsContatore.Tables(0).Rows(0)("CODFOGNATURA").ToString())
            Else
                objDichContatoreAnater.nFognatura = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("IDGIRO")) Then
                objDichContatoreAnater.nGiro = CInt(dsContatore.Tables(0).Rows(0)("IDGIRO").ToString())
            Else
                objDichContatoreAnater.nGiro = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("IDTIPOATTIVITA")) Then
                objDichContatoreAnater.nIdAttivita = CInt(dsContatore.Tables(0).Rows(0)("IDTIPOATTIVITA").ToString())
            Else
                objDichContatoreAnater.nIdAttivita = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODCONTRATTO")) Then
                If dsContatore.Tables(0).Rows(0)("CODCONTRATTO").ToString() = "" Then
                    objDichContatoreAnater.nidCodContratto = -1
                Else
                    objDichContatoreAnater.nidCodContratto = dsContatore.Tables(0).Rows(0)("CODCONTRATTO").ToString()
                End If
            Else
                objDichContatoreAnater.nidCodContratto = "-1"
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODIMPIANTO")) Then
                objDichContatoreAnater.nIdImpianto = CInt(dsContatore.Tables(0).Rows(0)("CODIMPIANTO").ToString())
            Else
                objDichContatoreAnater.nIdImpianto = -1
            End If


            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODIMPIANTO")) Then
                objDichContatoreAnater.nIdImpianto = CInt(dsContatore.Tables(0).Rows(0)("CODIMPIANTO").ToString())
            Else
                objDichContatoreAnater.nIdImpianto = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("IDMINIMO")) Then
                objDichContatoreAnater.nIdMinimo = CInt(dsContatore.Tables(0).Rows(0)("IDMINIMO").ToString())
            Else
                objDichContatoreAnater.nIdMinimo = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("COD_STRADA")) Then
                objDichContatoreAnater.nIdVia = CInt(dsContatore.Tables(0).Rows(0)("COD_STRADA").ToString())
            Else
                objDichContatoreAnater.nIdVia = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODPOSIZIONE")) Then
                objDichContatoreAnater.nPosizione = CInt(dsContatore.Tables(0).Rows(0)("CODPOSIZIONE").ToString())
            Else
                objDichContatoreAnater.nPosizione = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("IDTIPOCONTATORE")) Then
                objDichContatoreAnater.nTipoContatore = CInt(dsContatore.Tables(0).Rows(0)("IDTIPOCONTATORE").ToString())
            Else
                objDichContatoreAnater.nTipoContatore = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("IDTIPOUTENZA")) Then
                objDichContatoreAnater.nTipoUtenza = CInt(dsContatore.Tables(0).Rows(0)("IDTIPOUTENZA").ToString())
            Else
                objDichContatoreAnater.nTipoUtenza = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("PROPRIETARIO")) Then
                objDichContatoreAnater.proprietario = CInt(dsContatore.Tables(0).Rows(0)("PROPRIETARIO").ToString())
            Else
                objDichContatoreAnater.proprietario = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CIVICO_UBICAZIONE")) Then
                If dsContatore.Tables(0).Rows(0)("CIVICO_UBICAZIONE").ToString() <> "" Then
                    objDichContatoreAnater.sCivico = dsContatore.Tables(0).Rows(0)("CIVICO_UBICAZIONE").ToString()
                Else
                    objDichContatoreAnater.sCivico = "-1"
                End If

            Else
                objDichContatoreAnater.sCivico = "-1"
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODENTEAPPARTENENZA")) Then
                objDichContatoreAnater.sCodEnteAppartenenza = dsContatore.Tables(0).Rows(0)("CODENTEAPPARTENENZA").ToString()
            Else
                objDichContatoreAnater.sCodEnteAppartenenza = "-1"
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODICEFABBRICANTE")) Then
                objDichContatoreAnater.sCodiceFabbricante = dsContatore.Tables(0).Rows(0)("CODICEFABBRICANTE").ToString()
            Else
                objDichContatoreAnater.sCodiceFabbricante = ""
            End If

            objDichContatoreAnater.sCodiceISTAT = dsContatore.Tables(0).Rows(0)("CODICE_ISTAT").ToString()

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CONSUMOMINIMOFATTURABILE")) Then
                objDichContatoreAnater.sConsumoMinimo = dsContatore.Tables(0).Rows(0)("CONSUMOMINIMOFATTURABILE").ToString()
            Else
                objDichContatoreAnater.sConsumoMinimo = "0"
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODCONTATOREPRECEDENTE")) Then
                objDichContatoreAnater.sContatorePrec = dsContatore.Tables(0).Rows(0)("CODCONTATOREPRECEDENTE").ToString()
            Else
                objDichContatoreAnater.sContatorePrec = "-1"
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODCONTATORESUCCESSIVO")) Then
                objDichContatoreAnater.sContatoreSucc = dsContatore.Tables(0).Rows(0)("CODCONTATORESUCCESSIVO").ToString()
            Else
                objDichContatoreAnater.sContatoreSucc = "-1"
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATAATTIVAZIONE")) Then
                objDichContatoreAnater.sDataAttivazione = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATAATTIVAZIONE").ToString())
            Else
                objDichContatoreAnater.sDataAttivazione = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATACESSAZIONE")) Then
                objDichContatoreAnater.sDataCessazione = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATACESSAZIONE").ToString())
            Else
                objDichContatoreAnater.sDataCessazione = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATARIMOZIONETEMPORANEA")) Then
                objDichContatoreAnater.sDataRimpTemp = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATARIMOZIONETEMPORANEA").ToString())
            Else
                objDichContatoreAnater.sDataRimpTemp = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATASOSPENSIONEUTENZA")) Then
                objDichContatoreAnater.sDataSospsensioneUtenza = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATASOSPENSIONEUTENZA").ToString())
            Else
                objDichContatoreAnater.sDataSospsensioneUtenza = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATASOSTITUZIONE")) Then
                objDichContatoreAnater.sDataSostituzione = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATASOSTITUZIONE").ToString())
            Else
                objDichContatoreAnater.sDataSostituzione = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DIRITTI")) Then
                objDichContatoreAnater.sDiritti = CDbl(dsContatore.Tables(0).Rows(0)("DIRITTI").ToString())
            Else
                objDichContatoreAnater.sDiritti = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("ESPONENTE_CIVICO")) Then
                objDichContatoreAnater.sEsponente = dsContatore.Tables(0).Rows(0)("ESPONENTE_CIVICO").ToString()
            Else
                objDichContatoreAnater.sEsponente = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("FOGLIO")) Then
                objDichContatoreAnater.sFoglio = dsContatore.Tables(0).Rows(0)("FOGLIO").ToString()
            Else
                objDichContatoreAnater.sFoglio = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("LATOSTRADA")) Then
                objDichContatoreAnater.sLatoStrada = dsContatore.Tables(0).Rows(0)("LATOSTRADA").ToString()
            Else
                objDichContatoreAnater.sLatoStrada = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("MATRICOLA")) Then
                objDichContatoreAnater.sMatricola = dsContatore.Tables(0).Rows(0)("MATRICOLA").ToString()
            Else
                objDichContatoreAnater.sMatricola = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("NOTE")) Then
                objDichContatoreAnater.sNote = dsContatore.Tables(0).Rows(0)("NOTE").ToString()
            Else
                objDichContatoreAnater.sNote = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("NUMERO")) Then
                objDichContatoreAnater.sNumero = dsContatore.Tables(0).Rows(0)("NUMERO").ToString()
            Else
                objDichContatoreAnater.sNumero = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CIFRECONTATORE")) Then
                objDichContatoreAnater.sNumeroCifreContatore = dsContatore.Tables(0).Rows(0)("CIFRECONTATORE").ToString()
            Else
                objDichContatoreAnater.sNumeroCifreContatore = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("NUMEROUTENTE")) Then
                objDichContatoreAnater.sNumeroUtente = dsContatore.Tables(0).Rows(0)("NUMEROUTENTE").ToString()
            Else
                objDichContatoreAnater.sNumeroUtente = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("NUMEROUTENZE")) Then
                objDichContatoreAnater.sNumeroUtenze = CInt(dsContatore.Tables(0).Rows(0)("NUMEROUTENZE").ToString())
            Else
                objDichContatoreAnater.sNumeroUtenze = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("PENALITA")) Then
                objDichContatoreAnater.sPenalita = dsContatore.Tables(0).Rows(0)("PENALITA").ToString()
            Else
                objDichContatoreAnater.sPenalita = -1
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("PENDENTE")) Then
                If dsContatore.Tables(0).Rows(0)("PENDENTE").ToString() = False Then
                    objDichContatoreAnater.spendente = 0
                Else
                    objDichContatoreAnater.spendente = 1
                End If
            Else
                objDichContatoreAnater.spendente = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("PIANO")) Then
                objDichContatoreAnater.sPiano = dsContatore.Tables(0).Rows(0)("PIANO").ToString()
            Else
                objDichContatoreAnater.sPiano = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("POSIZIONEPROGRESSIVA")) Then
                objDichContatoreAnater.sProgressivo = dsContatore.Tables(0).Rows(0)("POSIZIONEPROGRESSIVA").ToString()
            Else
                objDichContatoreAnater.sProgressivo = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("QUOTEAGEVOLATE")) Then
                objDichContatoreAnater.sQuoteAgevolate = dsContatore.Tables(0).Rows(0)("QUOTEAGEVOLATE").ToString()
            Else
                objDichContatoreAnater.sQuoteAgevolate = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("SEQUENZA")) Then
                objDichContatoreAnater.sSequenza = dsContatore.Tables(0).Rows(0)("SEQUENZA").ToString()
            Else
                objDichContatoreAnater.sSequenza = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("SPESA")) Then
                objDichContatoreAnater.sSpesa = CDbl(dsContatore.Tables(0).Rows(0)("SPESA").ToString())
            Else
                objDichContatoreAnater.sSpesa = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("STATOCONTATORE")) Then
                objDichContatoreAnater.sStatoContatore = dsContatore.Tables(0).Rows(0)("STATOCONTATORE").ToString()
            Else
                objDichContatoreAnater.sStatoContatore = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("SUBALTERNO")) Then
                objDichContatoreAnater.sSubalterno = CInt(dsContatore.Tables(0).Rows(0)("SUBALTERNO").ToString())
            Else
                objDichContatoreAnater.sSubalterno = -1
            End If

            'If Not IsDBNull(dsContatore.Tables(0).Rows(0)("SUBASSOCIATO")) Then
            '    objDichContatoreAnater.subAssociato = dsContatore.Tables(0).Rows(0)("SUBASSOCIATO").ToString()
            'Else
            '    objDichContatoreAnater.subAssociato = "-1"
            'End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("VIA_UBICAZIONE")) Then
                objDichContatoreAnater.sUbicazione = dsContatore.Tables(0).Rows(0)("VIA_UBICAZIONE").ToString()
            Else
                objDichContatoreAnater.sUbicazione = ""
            End If

            objDichContatoreAnater.sUserName = HttpContext.Current.Session("OPERATORE")

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("LASCIATOAVVISO")) Then
                If dsContatore.Tables(0).Rows(0)("LASCIATOAVVISO").ToString() = False Then
                    objDichContatoreAnater.bLasciatoAvviso = 0
                Else
                    objDichContatoreAnater.bLasciatoAvviso = 1
                End If
            Else
                objDichContatoreAnater.bLasciatoAvviso = 0
            End If


            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("LETTO")) Then
                If dsContatore.Tables(0).Rows(0)("LETTO").ToString() = False Then
                    objDichContatoreAnater.bLetto = 0
                Else
                    objDichContatoreAnater.bLetto = 1
                End If
            Else
                objDichContatoreAnater.bLetto = 0
            End If


            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("SUBCONTATORE")) Then
                If dsContatore.Tables(0).Rows(0)("SUBCONTATORE").ToString() = False Then
                    objDichContatoreAnater.bSubContatore = 0
                Else
                    objDichContatoreAnater.bSubContatore = 1
                End If
            Else
                objDichContatoreAnater.bSubContatore = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CONSUMOSTIMATO")) Then
                objDichContatoreAnater.nConsumoStimato = CInt(dsContatore.Tables(0).Rows(0)("CONSUMOSTIMATO").ToString())
            Else
                objDichContatoreAnater.nConsumoStimato = 0
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODLETTURISTA")) Then
                objDichContatoreAnater.nCodLetturista = CInt(dsContatore.Tables(0).Rows(0)("CODLETTURISTA").ToString())
            Else
                objDichContatoreAnater.nCodLetturista = 0
            End If

            objDichContatoreAnater.nIpublicpianto1 = 0

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("ACQUISITO")) Then
                If dsContatore.Tables(0).Rows(0)("ACQUISITO").ToString() = False Then
                    objDichContatoreAnater.bAcquisito = 0
                Else
                    objDichContatoreAnater.bAcquisito = 1
                End If
            Else
                objDichContatoreAnater.bAcquisito = 0
            End If


            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("ANOMALIA")) Then
                If dsContatore.Tables(0).Rows(0)("ANOMALIA").ToString() = False Then
                    objDichContatoreAnater.bAnomalia = 0
                Else
                    objDichContatoreAnater.bAnomalia = 1
                End If
            Else
                objDichContatoreAnater.bAnomalia = 0
            End If


            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("PROVENIENZA")) Then
                objDichContatoreAnater.provenienza = dsContatore.Tables(0).Rows(0)("PROVENIENZA").ToString()
            Else
                objDichContatoreAnater.provenienza = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODICE_PUNTO_PRESA")) Then
                objDichContatoreAnater.sCodicePuntoPresa = dsContatore.Tables(0).Rows(0)("CODICE_PUNTO_PRESA").ToString()
            Else
                objDichContatoreAnater.sCodicePuntoPresa = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("CODICE_UTENTE_ESTERNO")) Then
                objDichContatoreAnater.sCodiceUtenteEsterno = dsContatore.Tables(0).Rows(0)("CODICE_UTENTE_ESTERNO").ToString()
            Else
                objDichContatoreAnater.sCodiceUtenteEsterno = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("COGNOMEPROPRIETARIOFABBRICATO")) Then
                objDichContatoreAnater.sCognomePropFabbricato = dsContatore.Tables(0).Rows(0)("COGNOMEPROPRIETARIOFABBRICATO").ToString()
            Else
                objDichContatoreAnater.sCognomePropFabbricato = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATACONTROLLO")) Then
                objDichContatoreAnater.sDataControllo = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATACONTROLLO").ToString())
            Else
                objDichContatoreAnater.sDataControllo = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("FRAZIONE_UBICAZIONE")) Then
                objDichContatoreAnater.sFrazioneUbicazione = dsContatore.Tables(0).Rows(0)("FRAZIONE_UBICAZIONE").ToString()
            Else
                objDichContatoreAnater.sFrazioneUbicazione = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("DATADIPASSAGGIO")) Then
                objDichContatoreAnater.sDataPassaggio = ClsData.GiraDataFromDB(dsContatore.Tables(0).Rows(0)("DATADIPASSAGGIO").ToString())
            Else
                objDichContatoreAnater.sDataPassaggio = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("MATRICOLANUMERICA")) Then
                objDichContatoreAnater.sMatricolaNumerica = dsContatore.Tables(0).Rows(0)("MATRICOLANUMERICA").ToString()
            Else
                objDichContatoreAnater.sMatricolaNumerica = ""
            End If

            If Not IsDBNull(dsContatore.Tables(0).Rows(0)("NOMEPROPRIETARIOFABBRICATO")) Then
                objDichContatoreAnater.sNomePropFabbricato = dsContatore.Tables(0).Rows(0)("NOMEPROPRIETARIOFABBRICATO").ToString()
            Else
                objDichContatoreAnater.sNomePropFabbricato = ""
            End If

            objDichContatoreAnater.Id = CInt(dsContatore.Tables(0).Rows(0)("CODCONTATORE").ToString())


            objDichContatoreAnater.nIdDiametroContatoreContratto = 0
            objDichContatoreAnater.nIdDiametroPresaContratto = 0
            objDichContatoreAnater.nIdTipoUtenzaContratto = 0
            objDichContatoreAnater.nVirtualIDContratto = 0
            objDichContatoreAnater.sDataSottoscrizione = ""
            objDichContatoreAnater.sNewCodContatore = ""
            objDichContatoreAnater.sNumeroUtenzeContratto = 0
            objDichContatoreAnater.sCodiceContratto = ""
            objDichContatoreAnater.nCodAnagContatore = 0
            objDichContatoreAnater.nCodIntestatario = 0


            Dim x As Integer = 0

            If dsCatasto.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsCatasto.Tables(0).Rows.Count - 1
                    objDatiCatastaliSingolo = New Anater.Oggetti.DatiCatastali
                    objDatiCatastaliSingolo = TraduciDatiCatastali(dsCatasto.Tables(0).Rows(i)("IDCONT_CATAS"),
                    dsCatasto.Tables(0).Rows(i)("CODCONTATORE"), dsCatasto.Tables(0).Rows(i)("INTERNO"),
                    dsCatasto.Tables(0).Rows(i)("PIANO"), dsCatasto.Tables(0).Rows(i)("FOGLIO"),
                    dsCatasto.Tables(0).Rows(i)("NUMERO"), dsCatasto.Tables(0).Rows(i)("SUBALTERNO"), codiceFiscale)

                    ReDim Preserve objDatiCatastaliList(x)
                    objDatiCatastaliList(x) = objDatiCatastaliSingolo

                    x += 1
                Next
            Else
                objDatiCatastaliSingolo.Id = -1
                objDatiCatastaliSingolo.CodContatore = CInt(dsContatore.Tables(0).Rows(0)("CODCONTATORE").ToString())
                objDatiCatastaliSingolo.CodFiscalePiva = ""
                objDatiCatastaliSingolo.Foglio = ""
                objDatiCatastaliSingolo.Interno = ""
                objDatiCatastaliSingolo.Numero = ""
                objDatiCatastaliSingolo.Piano = ""
                objDatiCatastaliSingolo.Subalterno = -1

                ReDim Preserve objDatiCatastaliList(0)
                objDatiCatastaliList(0) = objDatiCatastaliSingolo

            End If

            objDichContatoreAnater.oDatiCatastali = objDatiCatastaliList

            'CODPDA
            'DATA_SCARICO_PDA
            'CONSUMOMINIMOFATTURABILERIMTEMP
            'objDichContatoreAnater.dMinimoFatturabile = dsContatore.Tables(0).Rows(0)("MINIMOFATTURABILE")
            'objDichContatoreAnater.bDaNonConsiderare = DANONCONSIDERARE
            'objDichContatoreAnater.bDaRicontrollare = DARICONTROLLARE
            'objDichContatoreAnater.bEstratto = ESTRATTO
            'objDichContatoreAnater.bModuloAutoLettura = MODULOAUTOLETTURA
            'objDichContatoreAnater.bScaricatoSuPDA = SCARICATOSUPDA
            'objDichContatoreAnater.bSmat = SMAT
            'NOTELETTURISTA
            'DIAMETROCONTATORE

            Return objDichContatoreAnater
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTraduciRibaltamento.TraduciContatoreAnater.errore: ", Err)
            Return Nothing
        End Try



    End Function


    Public Function TraduciDatiCatastali(ByVal idCodntCatas As Integer, ByVal codContatore As Integer, ByVal interno As String,
    ByVal piano As String, ByVal foglio As String, ByVal numero As String, ByVal subalterno As Integer, ByVal codiceFiscale As String) As Anater.Oggetti.DatiCatastali
        Dim objDatiCatastali As New Anater.Oggetti.DatiCatastali

        Try
            objDatiCatastali.CodContatore = codContatore
            objDatiCatastali.CodFiscalePiva = codiceFiscale
            objDatiCatastali.Foglio = foglio
            objDatiCatastali.Id = idCodntCatas
            objDatiCatastali.Interno = interno
            objDatiCatastali.Numero = numero
            objDatiCatastali.Piano = piano
            objDatiCatastali.Subalterno = subalterno

            Return objDatiCatastali

        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTraduciRibaltamento.TraduciDatiCatastali.errore: ", Err)
            Return Nothing
        End Try

    End Function

    'Public Function TrovaAnagrafica(ByVal codContatore As Integer) As DettaglioAnagrafica
    '    Dim ssqlAnag = ""
    '    Dim codUtente As Integer = 0
    '    Dim DBAccess As New DBAccess.getDBobject
    '    Dim dsAnag As New DataSet
    '    Dim oSession As RIBESFrameWork.Session
    '    Dim oSM As New RIBESFrameWork.SessionManager(HttpContext.Current.Session("PARAMETROENV"))

    '    Try

    '        If oSM.Initialize(ConstSession.UserName, HttpContext.Current.Session("PARAMETROENV")) Then
    '            oSession = oSM.CreateSession(ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString())
    '            If oSession Is Nothing Then
    '                'Errore creazione Session
    '            Else
    '                If oSession.oErr.Number <> 0 Then
    '                    'Errore
    '                End If
    '            End If
    '        End If

    '        ssqlAnag = "SELECT COD_CONTRIBUENTE FROM TR_CONTATORI_UTENTE "
    '        ssqlAnag += " WHERE(CODCONTATORE = " & codContatore & ")"

    '        dsAnag = DBAccess.RunSQLReturnDataSet(ssqlAnag)

    '        codUtente = CInt(dsAnag.Tables(0).Rows(0)("COD_CONTRIBUENTE").ToString())

    '        Dim COD_TRIBUTO As Int32 = Int32.Parse(HttpContext.Current.Session("COD_TRIBUTO"))
    '        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica(oSession, ConfigurationManager.AppSettings("PARAMETRO_ANAGRAFICA"))
    '        Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica

    '        oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(CLng(codUtente), COD_TRIBUTO, -1)


    '        Return oDettaglioAnagraficaUtente

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTraduciRibaltamento.TrovaAnagrafica.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function TrovaAnagrafica(ByVal codContatore As Integer) As DettaglioAnagrafica
        Dim ssqlAnag As String = ""
        Dim codUtente As Integer = 0
        Dim dsAnag As New DataSet

        Try
            ssqlAnag = "SELECT COD_CONTRIBUENTE FROM TR_CONTATORI_UTENTE "
            ssqlAnag += " WHERE(CODCONTATORE = " & codContatore & ")"
            dsAnag = iDB.RunSQLReturnDataSet(ssqlAnag)
            codUtente = CInt(dsAnag.Tables(0).Rows(0)("COD_CONTRIBUENTE").ToString())

            Dim COD_TRIBUTO As Int32 = Int32.Parse(ConstSession.CodTributo)
            Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
            Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica

            oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(CLng(codUtente), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'oAnagrafica.GetAnagrafica(CLng(codUtente), COD_TRIBUTO, -1)
            Return oDettaglioAnagraficaUtente

        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTraduciRibaltamento.TrovaAnagrafica.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class
