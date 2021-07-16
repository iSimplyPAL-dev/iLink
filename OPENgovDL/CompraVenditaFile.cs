using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
/// Classe per la gestione dei dati esterni da Compravendita dell'atto
/// </summary>
    public class Trascrizione : DbObject<Trascrizione>
    {
        #region Variables and constructor
        public Trascrizione()
        {
            Reset();
        }

        public Trascrizione(int IdTrascrizione)
        {
            Reset();
            idTrascrizione = IdTrascrizione;
        }
        #endregion

        #region Public properties
        public int idTrascrizione { get; set; }
        public string idEnte { get; set; }
        public int idFlusso { get; set; }
        public string tipoNota { get; set; }
        public string numeroNota { get; set; }
        public string progressivoNota { get; set; }
        public string anno { get; set; }
        public DateTime dataValiditaAtto { get; set; }
        public DateTime dataPresentazioneAtto { get; set; }
        public int esitoNota { get; set; }
        public string esitoNotaNonRegistrata { get; set; }
        public DateTime dataRegistrazioneInAtti { get; set; }
        public int numeroRepertorio { get; set; }
        public int codiceAtto { get; set; }
        public string cognomeNomeRogante { get; set; }
        public string codiceFiscaleRogante { get; set; }
        public string sedeRogante { get; set; }
        public bool registrazioneDifferita { get; set; }
        public DateTime dataInDifferita { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_ATTI_TRASCRIZIONE_IU";

                sqlCmd.Parameters.AddWithValue("@IdTrascrizione", DbParam.Get(idTrascrizione));
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                sqlCmd.Parameters.AddWithValue("@fkidFlusso", DbParam.Get(idFlusso));
                sqlCmd.Parameters.AddWithValue("@tipoNota", DbParam.Get(tipoNota));
                sqlCmd.Parameters.AddWithValue("@numeroNota", DbParam.Get(numeroNota));
                sqlCmd.Parameters.AddWithValue("@progressivoNota", DbParam.Get(progressivoNota));
                sqlCmd.Parameters.AddWithValue("@anno", DbParam.Get(anno));
                sqlCmd.Parameters.AddWithValue("@dataValiditaAtto", DbParam.Get(dataValiditaAtto));
                sqlCmd.Parameters.AddWithValue("@dataPresentazioneAtto", DbParam.Get(dataPresentazioneAtto));
                sqlCmd.Parameters.AddWithValue("@fkesitoNota", DbParam.Get(esitoNota));
                sqlCmd.Parameters.AddWithValue("@fkesitoNotaNonRegistrata", DbParam.Get(esitoNotaNonRegistrata));
                sqlCmd.Parameters.AddWithValue("@dataRegistrazioneInAtti", DbParam.Get(dataRegistrazioneInAtti));
                sqlCmd.Parameters.AddWithValue("@numeroRepertorio", DbParam.Get(numeroRepertorio));
                sqlCmd.Parameters.AddWithValue("@fkcodiceAtto", DbParam.Get(codiceAtto));
                sqlCmd.Parameters.AddWithValue("@cognomeNomeRogante", DbParam.Get(cognomeNomeRogante));
                sqlCmd.Parameters.AddWithValue("@codiceFiscaleRogante", DbParam.Get(codiceFiscaleRogante));
                sqlCmd.Parameters.AddWithValue("@sedeRogante", DbParam.Get(sedeRogante));
                sqlCmd.Parameters.AddWithValue("@registrazioneDifferita", DbParam.Get(registrazioneDifferita));
                sqlCmd.Parameters.AddWithValue("@dataInDifferita", DbParam.Get(dataInDifferita));

                sqlCmd.Parameters["@IdTrascrizione"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                idTrascrizione = (int)sqlCmd.Parameters["@IdTrascrizione"].Value;

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                errore = ex.Message;
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Equals(object obj)
        {
            return
                (obj is Trascrizione) &&
                ((obj as Trascrizione).idTrascrizione == idTrascrizione);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(idTrascrizione);
        }

        public override sealed void Reset()
        {
            idTrascrizione = default(int);
            idEnte = string.Empty;
            idFlusso = default(int);
            tipoNota = string.Empty;
            numeroNota = string.Empty;
            progressivoNota = string.Empty;
            anno = string.Empty;
            dataValiditaAtto = DateTime.MaxValue;
            dataPresentazioneAtto = DateTime.MaxValue;
            esitoNota = default(int);
            esitoNotaNonRegistrata = string.Empty;
            dataRegistrazioneInAtti = DateTime.MaxValue;
            numeroRepertorio = default(int);
            codiceAtto = default(int);
            cognomeNomeRogante = string.Empty;
            codiceFiscaleRogante = string.Empty;
            sedeRogante = string.Empty;
            registrazioneDifferita = default(bool);
            dataInDifferita = DateTime.MaxValue;
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override Trascrizione[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita dell'immobile
    /// </summary>
    public class Fabbricato : DbObject<Fabbricato>
    {
        #region Variables and constructor
        public Fabbricato()
        {
            Reset();
        }

        public Fabbricato(int IdFabbricato)
        {
            Reset();
            idFabbricato = IdFabbricato;
        }
        #endregion

        #region Public properties
        public int idFabbricato { get; set; }
        public int idTrascrizione { get; set; }
        public string tipologiaImmobile { get; set; }
        public int flagGraffato { get; set; }
        public int idCatastaleImmobile { get; set; }
        public string codiceEsito { get; set; }
        public string ref_Immobile { get; set; }
        public List<Identificativi> identificativi { get; set; }
        public string natura { get; set; }
        public string zona { get; set; }
        public string categoria { get; set; }
        public string classe { get; set; }
        public int MC { get; set; }
        public int MQ { get; set; }
        public double Vani { get; set; }
        public double superficie { get; set; }
        public double renditaEuro { get; set; }
        public string lottoNota { get; set; }
        public string edificioNota { get; set; }
        public string scalaNota { get; set; }
        public string interno1Nota { get; set; }
        public string interno2Nota { get; set; }
        public string piano1Nota { get; set; }
        public string piano2Nota { get; set; }
        public string piano3Nota { get; set; }
        public string piano4Nota { get; set; }
        public string toponimoNota { get; set; }
        public string indirizzoNota { get; set; }
        public string civico1Nota { get; set; }
        public string civico2Nota { get; set; }
        public string civico3Nota { get; set; }
        public string lottoCatasto { get; set; }
        public string edificioCatasto { get; set; }
        public string scalaCatasto { get; set; }
        public string interno1Catasto { get; set; }
        public string interno2Catasto { get; set; }
        public string piano1Catasto { get; set; }
        public string piano2Catasto { get; set; }
        public string piano3Catasto { get; set; }
        public string piano4Catasto { get; set; }
        public string toponimoCatasto { get; set; }
        public string indirizzoCatasto { get; set; }
        public string civico1Catasto { get; set; }
        public string civico2Catasto { get; set; }
        public string civico3Catasto { get; set; }
        public string annotazioniCatasto { get; set; }
        public string partita { get; set; }
        public int qualita { get; set; }
        public double ettari { get; set; }
        public double are { get; set; }
        public double centiare { get; set; }
        public bool redditoNonCalcolabile { get; set; }
        public double dominicaleEuro { get; set; }
        public double agrarioEuro { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_ATTI_FABBRICATO_IU";

                sqlCmd.Parameters.AddWithValue("@idFabbricato", DbParam.Get(idFabbricato));
                sqlCmd.Parameters.AddWithValue("@fkidTrascrizione", DbParam.Get(idTrascrizione));
                sqlCmd.Parameters.AddWithValue("@fktipologiaImmobile", DbParam.Get(tipologiaImmobile));
                sqlCmd.Parameters.AddWithValue("@flagGraffato", DbParam.Get(flagGraffato));
                sqlCmd.Parameters.AddWithValue("@idCatastaleImmobile", DbParam.Get(idCatastaleImmobile));
                sqlCmd.Parameters.AddWithValue("@fkesitoNotaNonRegistrata", DbParam.Get(codiceEsito));
                sqlCmd.Parameters.AddWithValue("@ref_Immobile", DbParam.Get(ref_Immobile));
                sqlCmd.Parameters.AddWithValue("@fknatura", DbParam.Get(natura));
                sqlCmd.Parameters.AddWithValue("@zona", DbParam.Get(zona));
                sqlCmd.Parameters.AddWithValue("@categoria", DbParam.Get(categoria));
                sqlCmd.Parameters.AddWithValue("@classe", DbParam.Get(classe));
                sqlCmd.Parameters.AddWithValue("@MC", DbParam.Get(MC));
                sqlCmd.Parameters.AddWithValue("@MQ", DbParam.Get(MQ));
                sqlCmd.Parameters.AddWithValue("@Vani", DbParam.Get(Vani));
                sqlCmd.Parameters.AddWithValue("@superficie", DbParam.Get(superficie));
                sqlCmd.Parameters.AddWithValue("@renditaEuro", DbParam.Get(renditaEuro));
                sqlCmd.Parameters.AddWithValue("@lottoNota", DbParam.Get(lottoNota));
                sqlCmd.Parameters.AddWithValue("@edificioNota", DbParam.Get(edificioNota));
                sqlCmd.Parameters.AddWithValue("@scalaNota", DbParam.Get(scalaNota));
                sqlCmd.Parameters.AddWithValue("@interno1Nota", DbParam.Get(interno1Nota));
                sqlCmd.Parameters.AddWithValue("@interno2Nota", DbParam.Get(interno2Nota));
                sqlCmd.Parameters.AddWithValue("@piano1Nota", DbParam.Get(piano1Nota));
                sqlCmd.Parameters.AddWithValue("@piano2Nota", DbParam.Get(piano2Nota));
                sqlCmd.Parameters.AddWithValue("@piano3Nota", DbParam.Get(piano3Nota));
                sqlCmd.Parameters.AddWithValue("@piano4Nota", DbParam.Get(piano4Nota));
                sqlCmd.Parameters.AddWithValue("@toponimoNota", DbParam.Get(toponimoNota));
                sqlCmd.Parameters.AddWithValue("@indirizzoNota", DbParam.Get(indirizzoNota));
                sqlCmd.Parameters.AddWithValue("@civico1Nota", DbParam.Get(civico1Nota));
                sqlCmd.Parameters.AddWithValue("@civico2Nota", DbParam.Get(civico2Nota));
                sqlCmd.Parameters.AddWithValue("@civico3Nota", DbParam.Get(civico3Nota));
                sqlCmd.Parameters.AddWithValue("@lottoCatasto", DbParam.Get(lottoCatasto));
                sqlCmd.Parameters.AddWithValue("@edificioCatasto", DbParam.Get(edificioCatasto));
                sqlCmd.Parameters.AddWithValue("@scalaCatasto", DbParam.Get(scalaCatasto));
                sqlCmd.Parameters.AddWithValue("@interno1Catasto", DbParam.Get(interno1Catasto));
                sqlCmd.Parameters.AddWithValue("@interno2Catasto", DbParam.Get(interno2Catasto));
                sqlCmd.Parameters.AddWithValue("@piano1Catasto", DbParam.Get(piano1Catasto));
                sqlCmd.Parameters.AddWithValue("@piano2Catasto", DbParam.Get(piano2Catasto));
                sqlCmd.Parameters.AddWithValue("@piano3Catasto", DbParam.Get(piano3Catasto));
                sqlCmd.Parameters.AddWithValue("@piano4Catasto", DbParam.Get(piano4Catasto));
                sqlCmd.Parameters.AddWithValue("@toponimoCatasto", DbParam.Get(toponimoCatasto));
                sqlCmd.Parameters.AddWithValue("@indirizzoCatasto", DbParam.Get(indirizzoCatasto));
                sqlCmd.Parameters.AddWithValue("@civico1Catasto", DbParam.Get(civico1Catasto));
                sqlCmd.Parameters.AddWithValue("@civico2Catasto", DbParam.Get(civico2Catasto));
                sqlCmd.Parameters.AddWithValue("@civico3Catasto", DbParam.Get(civico3Catasto));
                sqlCmd.Parameters.AddWithValue("@annotazioniCatasto", DbParam.Get(annotazioniCatasto));
                sqlCmd.Parameters.AddWithValue("@partita", DbParam.Get(partita));
                sqlCmd.Parameters.AddWithValue("@fkqualita", DbParam.Get(qualita));
                sqlCmd.Parameters.AddWithValue("@ettari", DbParam.Get(ettari));
                sqlCmd.Parameters.AddWithValue("@are", DbParam.Get(are));
                sqlCmd.Parameters.AddWithValue("@centiare", DbParam.Get(centiare));
                sqlCmd.Parameters.AddWithValue("@redditoNonCalcolabile", DbParam.Get(redditoNonCalcolabile));
                sqlCmd.Parameters.AddWithValue("@dominicaleEuro", DbParam.Get(dominicaleEuro));
                sqlCmd.Parameters.AddWithValue("@agrarioEuro", DbParam.Get(agrarioEuro));

                sqlCmd.Parameters["@idFabbricato"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                idFabbricato = (int)sqlCmd.Parameters["@idFabbricato"].Value;

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                errore = ex.Message;
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Equals(object obj)
        {
            return
                (obj is Fabbricato) &&
                ((obj as Fabbricato).idFabbricato == idFabbricato);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(idFabbricato);
        }

        public override sealed void Reset()
        {
			idFabbricato = default(int);
            idTrascrizione = default(int);
			tipologiaImmobile = string.Empty;
			flagGraffato = default(int);
            idCatastaleImmobile = default(int);
			codiceEsito = string.Empty;
			ref_Immobile = string.Empty;
			identificativi = null;
			natura = string.Empty;
			zona = string.Empty;
			categoria = string.Empty;
			classe = string.Empty;
			MC = default(int);
			MQ = default(int);
			Vani = default(double);
			superficie = default(double);
			renditaEuro = default(double);
			lottoNota = string.Empty;
			edificioNota = string.Empty;
			scalaNota = string.Empty;
			interno1Nota = string.Empty;
			interno2Nota = string.Empty;
			piano1Nota = string.Empty;
			piano2Nota = string.Empty;
			piano3Nota = string.Empty;
			piano4Nota = string.Empty;
			toponimoNota = string.Empty;
			indirizzoNota = string.Empty;
			civico1Nota = string.Empty;
			civico2Nota = string.Empty;
			civico3Nota = string.Empty;
			lottoCatasto = string.Empty;
			edificioCatasto = string.Empty;
			scalaCatasto = string.Empty;
			interno1Catasto = string.Empty;
			interno2Catasto = string.Empty;
			piano1Catasto = string.Empty;
			piano2Catasto = string.Empty;
			piano3Catasto = string.Empty;
			piano4Catasto = string.Empty;
			toponimoCatasto = string.Empty;
			indirizzoCatasto = string.Empty;
			civico1Catasto = string.Empty;
			civico2Catasto = string.Empty;
			civico3Catasto = string.Empty;
			annotazioniCatasto = string.Empty;
            partita = string.Empty;
            qualita = default(int);
            ettari = default(double);
            are = default(double);
            centiare = default(double);
            redditoNonCalcolabile = default(bool);
            dominicaleEuro = default(double);
            agrarioEuro = default(double);
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override Fabbricato[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita dei riferimenti catastali
    /// </summary>
    public class Identificativi : DbObject<Identificativi>
    {
        #region Variables and constructor
        public Identificativi()
        {
            Reset();
        }

        public Identificativi(int _idIdentificativo)
        {
            Reset();
            idIdentificativo = _idIdentificativo;
        }
        #endregion

        #region Public properties
        public int idIdentificativo { get; set; }
        public int idFabbricato { get; set; }
        public string sezioneCensuaria { get; set; }
        public string sezioneUrbana { get; set; }
        public string foglio { get; set; }
        public string numero { get; set; }
        public string denominatore { get; set; }
        public string subalterno { get; set; }
        public string edificialita { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_ATTI_IDENTIFICATIVI_IU";

                sqlCmd.Parameters.AddWithValue("@idIdentificativo", DbParam.Get(idIdentificativo));
                sqlCmd.Parameters.AddWithValue("@fkidFabbricato", DbParam.Get(idFabbricato));
                sqlCmd.Parameters.AddWithValue("@sezioneCensuaria", DbParam.Get(sezioneCensuaria));
                sqlCmd.Parameters.AddWithValue("@sezioneUrbana", DbParam.Get(sezioneUrbana));
                sqlCmd.Parameters.AddWithValue("@foglio", DbParam.Get(foglio));
                sqlCmd.Parameters.AddWithValue("@numero", DbParam.Get(numero));
                sqlCmd.Parameters.AddWithValue("@denominatore", DbParam.Get(denominatore));
                sqlCmd.Parameters.AddWithValue("@subalterno", DbParam.Get(subalterno));
                sqlCmd.Parameters.AddWithValue("@edificialita", DbParam.Get(edificialita));

                sqlCmd.Parameters["@idIdentificativo"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                idIdentificativo = (int)sqlCmd.Parameters["@idIdentificativo"].Value;

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                string error = string.Empty;
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Equals(object obj)
        {
            return
                (obj is Identificativi) &&
                ((obj as Identificativi).idIdentificativo == idIdentificativo);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(idIdentificativo);
        }

        public override sealed void Reset()
        {
            idIdentificativo = default(int);
            idFabbricato = default(int);
            sezioneCensuaria = string.Empty;
            sezioneUrbana = string.Empty;
            foglio = string.Empty;
            numero = string.Empty;
            denominatore = string.Empty;
            subalterno = string.Empty;
            edificialita = string.Empty;
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override Identificativi[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita dei soggetti
    /// </summary>
    public class Soggetto : DbObject<Soggetto>
    {
        #region Variables and constructor
        public Soggetto()
        {
            Reset();
        }

        public Soggetto(int IdSoggetto)
        {
            Reset();
            idSoggetto = IdSoggetto;
        }
        #endregion

        #region Public properties
        public int idSoggetto { get; set; }
        public int idTrascrizione { get; set; }
        public int idSoggettoNota { get; set; }
        public int idSoggettoCatastale { get; set; }
        public string cognome { get; set; }
        public string nome { get; set; }
        public string sesso { get; set; }
        public DateTime dataNascita { get; set; }
        public string luogoNascita { get; set; }
        public string codiceFiscale { get; set; }
        public int tipoIndirizzo { get; set; }
        public string comune { get; set; }
        public string provincia { get; set; }
        public string indirizzo { get; set; }
        public string CAP { get; set; }
        public List<Titolarita> datiTitolarita { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_ATTI_SOGGETTO_IU";

                sqlCmd.Parameters.AddWithValue("@IdSoggetto", DbParam.Get(idSoggetto));
                sqlCmd.Parameters.AddWithValue("@fkidTrascrizione", DbParam.Get(idTrascrizione));
                sqlCmd.Parameters.AddWithValue("@idSoggettoNota", DbParam.Get(idSoggettoNota));
                sqlCmd.Parameters.AddWithValue("@idSoggettoCatastale", DbParam.Get(idSoggettoCatastale));
                sqlCmd.Parameters.AddWithValue("@cognome", DbParam.Get(cognome));
                sqlCmd.Parameters.AddWithValue("@nome", DbParam.Get(nome));
                sqlCmd.Parameters.AddWithValue("@sesso", DbParam.Get(sesso));
                sqlCmd.Parameters.AddWithValue("@dataNascita", DbParam.Get(dataNascita));
                sqlCmd.Parameters.AddWithValue("@luogoNascita", DbParam.Get(luogoNascita));
                sqlCmd.Parameters.AddWithValue("@codiceFiscale", DbParam.Get(codiceFiscale));
                sqlCmd.Parameters.AddWithValue("@tipoIndirizzo", DbParam.Get(tipoIndirizzo));
                sqlCmd.Parameters.AddWithValue("@comune", DbParam.Get(comune));
                sqlCmd.Parameters.AddWithValue("@provincia", DbParam.Get(provincia));
                sqlCmd.Parameters.AddWithValue("@indirizzo", DbParam.Get(indirizzo));
                sqlCmd.Parameters.AddWithValue("@CAP", DbParam.Get(CAP));

                sqlCmd.Parameters["@IdSoggetto"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                idSoggetto = (int)sqlCmd.Parameters["@IdSoggetto"].Value;

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                errore = ex.Message;
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Equals(object obj)
        {
            return
                (obj is Soggetto) &&
                ((obj as Soggetto).idSoggetto == idSoggetto);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(idSoggetto);
        }

        public override sealed void Reset()
        {
            idSoggetto = default(int);
            idTrascrizione = default(int);
            idSoggettoNota = default(int);
            idSoggettoCatastale = default(int);
            cognome = string.Empty;
            nome = string.Empty;
            sesso = string.Empty;
            dataNascita = DateTime.MaxValue;
            luogoNascita = string.Empty;
            codiceFiscale = string.Empty;
            tipoIndirizzo = default(int);
            comune = string.Empty;
            provincia = string.Empty;
            indirizzo = string.Empty;
            CAP = string.Empty;
            datiTitolarita = null;
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override Soggetto[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita delle titolarità dei soggetti sugli immobili
    /// </summary>
    public class Titolarita : DbObject<Titolarita>
    {
        #region Variables and constructor
        public Titolarita()
        {
            Reset();
        }

        public Titolarita(int IdTitolarita)
        {
            Reset();
            idTitolarita = IdTitolarita;
        }
        #endregion

        #region Public properties
        public int idTitolarita { get; set; }
        public int idSoggetto { get; set; }
        public int idFabbricato { get; set; }
        public string tipologiaImmobile { get; set; }
        public string ref_Immobile { get; set; }
        public string tipoDiritto { get; set; }
        public string codiceDiritto { get; set; }
        public double quotaNumeratore { get; set; }
        public double quotaDenominatore { get; set; }
        public string regime { get; set; }
        public int soggettoRiferimento { get; set; }
        public string codiceDirittoDirittiCat { get; set; }
        public double quotaNumeratoreDirittiCat { get; set; }
        public double quotaDenominatoreDirittiCat { get; set; }
        public string regimeDirittiCat { get; set; }
        public int soggettoRiferimentoDirittiCat { get; set; }
        public int idTitolaritaDirittiCat { get; set; }
        public string titoloNonCodificatoDirittiCat { get; set; }
        public int idStato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_ATTI_TITOLARITA_IU";

                sqlCmd.Parameters.AddWithValue("@IdTitolarita", DbParam.Get(idTitolarita));
                sqlCmd.Parameters.AddWithValue("@fkidSoggetto", DbParam.Get(idSoggetto));
                sqlCmd.Parameters.AddWithValue("@fkidFabbricato", DbParam.Get(idFabbricato));
                sqlCmd.Parameters.AddWithValue("@fktipologiaImmobile", DbParam.Get(tipologiaImmobile));
                sqlCmd.Parameters.AddWithValue("@tipoDiritto", DbParam.Get(tipoDiritto));
                sqlCmd.Parameters.AddWithValue("@fkcodiceDiritto", DbParam.Get(codiceDiritto));
                sqlCmd.Parameters.AddWithValue("@quotaNumeratore", DbParam.Get(quotaNumeratore));
                sqlCmd.Parameters.AddWithValue("@quotaDenominatore", DbParam.Get(quotaDenominatore));
                sqlCmd.Parameters.AddWithValue("@fkregime", DbParam.Get(regime));
                sqlCmd.Parameters.AddWithValue("@soggettoRiferimento", DbParam.Get(soggettoRiferimento));
                sqlCmd.Parameters.AddWithValue("@fkcodiceDirittoDirittiCat", DbParam.Get(codiceDirittoDirittiCat));
                sqlCmd.Parameters.AddWithValue("@quotaNumeratoreDirittiCat", DbParam.Get(quotaNumeratoreDirittiCat));
                sqlCmd.Parameters.AddWithValue("@quotaDenominatoreDirittiCat", DbParam.Get(quotaDenominatoreDirittiCat));
                sqlCmd.Parameters.AddWithValue("@fkregimeDirittiCat", DbParam.Get(regimeDirittiCat));
                sqlCmd.Parameters.AddWithValue("@soggettoRiferimentoDirittiCat", DbParam.Get(soggettoRiferimentoDirittiCat));
                sqlCmd.Parameters.AddWithValue("@idTitolaritaDirittiCat", DbParam.Get(idTitolaritaDirittiCat));
                sqlCmd.Parameters.AddWithValue("@titoloNonCodificatoDirittiCat", DbParam.Get(titoloNonCodificatoDirittiCat));
                sqlCmd.Parameters.AddWithValue("@idStato", DbParam.Get(idStato));

                sqlCmd.Parameters["@IdTitolarita"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                idTitolarita = (int)sqlCmd.Parameters["@IdTitolarita"].Value;

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                errore = ex.Message;
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Equals(object obj)
        {
            return
                (obj is Titolarita) &&
                ((obj as Titolarita).idTitolarita == idTitolarita);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(idTitolarita);
        }

        public override sealed void Reset()
        {
            idTitolarita = default(int);
            idSoggetto = default(int);
            idFabbricato = default(int);
            tipologiaImmobile = string.Empty;
            ref_Immobile = string.Empty;
            tipoDiritto = string.Empty;
            codiceDiritto = string.Empty;
            quotaNumeratore = default(double);
            quotaDenominatore = default(double);
            regime = string.Empty;
            soggettoRiferimento = default(int);
            codiceDirittoDirittiCat = string.Empty;
            quotaNumeratoreDirittiCat = default(double);
            quotaDenominatoreDirittiCat = default(double);
            regimeDirittiCat = string.Empty;
            soggettoRiferimentoDirittiCat = default(int);
            idTitolaritaDirittiCat = default(int);
            titoloNonCodificatoDirittiCat = string.Empty;
            idStato = default(int);
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override Titolarita[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
