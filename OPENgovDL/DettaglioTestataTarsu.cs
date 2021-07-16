using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione della transcodifica da TARSU a TARI del dettaglio della testata
    /// </summary>
    public class DettaglioTestataTarsu : DbObject<DettaglioTestataTarsu>
    {
        #region Variables and constructor

/*
        private DateTime? _DATA_INIZIO_BCK_20110928;
*/

        public DettaglioTestataTarsu()
        {
            Reset();
        }

        public DettaglioTestataTarsu(int iD)
        {
            Reset();
            ID = iD;
        }
        #endregion

        #region Public properties
        public int ID { get; set; }
        public int IdDettaglioTestata { get; set; }
        public int IdTestata { get; set; }
        public int? IdPadre { get; set; }
        public int? CodVia { get; set; }
        public string Via { get; set; }
        public int? Civico { get; set; }
        public string Esponente { get; set; }
        public string Interno { get; set; }
        public string Scala { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public int? GGTarsu { get; set; }
        public string IdStatoOccupazione { get; set; }
        public string IdTipoConduttore { get; set; }
        public string NominativoProprietario { get; set; }
        public string NominativoOccupantePrec { get; set; }
        public string NoteDettaglioTestata { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime? DataVariazione { get; set; }
        public DateTime? DataCessazione { get; set; }
        public string Operatore { get; set; }
        public string EstensioneParticella { get; set; }
        public int? IDAssenzaDatiCatastali { get; set; }
        public int? IDDestinazioneUso { get; set; }
        public int? IDNaturaOccupante { get; set; }
        public string IDTipoParticella { get; set; }
        public string IDTipoUnita { get; set; }
        public int? IDTitoloOccupazione { get; set; }
        public string Sezione { get; set; }
        public string AzioneASeguitoDiRettifica { get; set; }
        public int NComponenti { get; set; }
        public double MQCatasto { get; set; }
        public int FKIdCategoriaAteco { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is DettaglioTestataTarsu) &&
                ((obj as DettaglioTestataTarsu).ID == ID);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(ID);
        }

        public override sealed void Reset()
        {
            ID = default(int);
            IdDettaglioTestata = default(int);
            IdTestata = default(int);
            IdPadre = default(int?);
            CodVia = default(int?);
            Via = default(string);
            Civico = default(int?);
            Esponente = default(string);
            Interno = default(string);
            Scala = default(string);
            Foglio = default(string);
            Numero = default(string);
            Subalterno = default(string);
            DataInizio = Global.MinDateTime();
            DataFine = default(DateTime?);
            GGTarsu = default(int?);
            IdStatoOccupazione = default(string);
            IdTipoConduttore = default(string);
            NominativoProprietario = default(string);
            NominativoOccupantePrec = default(string);
            NoteDettaglioTestata = default(string);
            DataInserimento = Global.MinDateTime();
            DataVariazione = default(DateTime?);
            DataCessazione = default(DateTime?);
            Operatore = default(string);
            EstensioneParticella = default(string);
            IDAssenzaDatiCatastali = default(int?);
            IDDestinazioneUso = default(int?);
            IDNaturaOccupante = default(int?);
            IDTipoParticella = default(string);
            IDTipoUnita = default(string);
            IDTitoloOccupazione = default(int?);
            Sezione = default(string);
            AzioneASeguitoDiRettifica = default(string);
            NComponenti = default(int);
            MQCatasto = default(double);
            FKIdCategoriaAteco = default(int);
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLDETTAGLIOTESTATATARSU_S";
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    IdDettaglioTestata = DbValue<int>.Get(sqlRead["IDDETTAGLIOTESTATA"]);
                    IdTestata = DbValue<int>.Get(sqlRead["IDTESTATA"]);
                    IdPadre = DbValue<int?>.Get(sqlRead["IDPADRE"]);
                    CodVia = DbValue<int?>.Get(sqlRead["CODVIA"]);
                    Via = DbValue<string>.Get(sqlRead["VIA"]);
                    Civico = DbValue<int?>.Get(sqlRead["CIVICO"]);
                    Esponente = DbValue<string>.Get(sqlRead["ESPONENTE"]);
                    Interno = DbValue<string>.Get(sqlRead["INTERNO"]);
                    Scala = DbValue<string>.Get(sqlRead["SCALA"]);
                    Foglio = DbValue<string>.Get(sqlRead["FOGLIO"]);
                    Numero = DbValue<string>.Get(sqlRead["NUMERO"]);
                    Subalterno = DbValue<string>.Get(sqlRead["SUBALTERNO"]);
                    DataInizio = DbValue<DateTime>.Get(sqlRead["DATA_INIZIO"]);
                    DataFine = DbValue<DateTime?>.Get(sqlRead["DATA_FINE"]);
                    GGTarsu = DbValue<int?>.Get(sqlRead["GGTARSU"]);
                    IdStatoOccupazione = DbValue<string>.Get(sqlRead["IDSTATOOCCUPAZIONE"]);
                    IdTipoConduttore = DbValue<string>.Get(sqlRead["IDTIPOCONDUTTORE"]);
                    NominativoProprietario = DbValue<string>.Get(sqlRead["NOMINATIVO_PROPRIETARIO"]);
                    NominativoOccupantePrec = DbValue<string>.Get(sqlRead["NOMINATIVO_OCCUPANTE_PREC"]);
                    NoteDettaglioTestata = DbValue<string>.Get(sqlRead["NOTEDETTAGLIOTESTATA"]);
                    DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]);
                    DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]);
                    DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]);
                    Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]);
                    EstensioneParticella = DbValue<string>.Get(sqlRead["ESTENSIONE_PARTICELLA"]);
                    IDAssenzaDatiCatastali = DbValue<int?>.Get(sqlRead["ID_ASSENZA_DATI_CATASTALI"]);
                    IDDestinazioneUso = DbValue<int?>.Get(sqlRead["ID_DESTINAZIONE_USO"]);
                    IDNaturaOccupante = DbValue<int?>.Get(sqlRead["ID_NATURA_OCCUPANTE"]);
                    IDTipoParticella = DbValue<string>.Get(sqlRead["ID_TIPO_PARTICELLA"]);
                    IDTipoUnita = DbValue<string>.Get(sqlRead["ID_TIPO_UNITA"]);
                    IDTitoloOccupazione = DbValue<int?>.Get(sqlRead["ID_TITOLO_OCCUPAZIONE"]);
                    Sezione = DbValue<string>.Get(sqlRead["SEZIONE"]);
                    AzioneASeguitoDiRettifica = DbValue<string>.Get(sqlRead["AZIONEASEGUITODIRETTIFICA"]);
                    NComponenti = DbValue<int>.Get(sqlRead["NCOMPONENTI"]);
                    MQCatasto = DbValue<double>.Get(sqlRead["MQCATASTO"]);
                    FKIdCategoriaAteco = DbValue<int>.Get(sqlRead["fk_IdCategoriaAteco"]);
                }
                else
                {
                    Reset();
                }

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override DettaglioTestataTarsu[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLDETTAGLIOTESTATATARSU_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<DettaglioTestataTarsu> list = new List<DettaglioTestataTarsu>();
                while (sqlRead.Read())
                {
                    DettaglioTestataTarsu item = new DettaglioTestataTarsu
                        {
                            ID = DbValue<int>.Get(sqlRead["ID"]),
                            IdDettaglioTestata = DbValue<int>.Get(sqlRead["IDDETTAGLIOTESTATA"]),
                            IdTestata = DbValue<int>.Get(sqlRead["IDTESTATA"]),
                            IdPadre = DbValue<int?>.Get(sqlRead["IDPADRE"]),
                            CodVia = DbValue<int?>.Get(sqlRead["CODVIA"]),
                            Via = DbValue<string>.Get(sqlRead["VIA"]),
                            Civico = DbValue<int?>.Get(sqlRead["CIVICO"]),
                            Esponente = DbValue<string>.Get(sqlRead["ESPONENTE"]),
                            Interno = DbValue<string>.Get(sqlRead["INTERNO"]),
                            Scala = DbValue<string>.Get(sqlRead["SCALA"]),
                            Foglio = DbValue<string>.Get(sqlRead["FOGLIO"]),
                            Numero = DbValue<string>.Get(sqlRead["NUMERO"]),
                            Subalterno = DbValue<string>.Get(sqlRead["SUBALTERNO"]),
                            DataInizio = DbValue<DateTime>.Get(sqlRead["DATA_INIZIO"]),
                            DataFine = DbValue<DateTime?>.Get(sqlRead["DATA_FINE"]),
                            GGTarsu = DbValue<int?>.Get(sqlRead["GGTARSU"]),
                            IdStatoOccupazione = DbValue<string>.Get(sqlRead["IDSTATOOCCUPAZIONE"]),
                            IdTipoConduttore = DbValue<string>.Get(sqlRead["IDTIPOCONDUTTORE"]),
                            NominativoProprietario = DbValue<string>.Get(sqlRead["NOMINATIVO_PROPRIETARIO"]),
                            NominativoOccupantePrec = DbValue<string>.Get(sqlRead["NOMINATIVO_OCCUPANTE_PREC"]),
                            NoteDettaglioTestata = DbValue<string>.Get(sqlRead["NOTEDETTAGLIOTESTATA"]),
                            DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]),
                            DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]),
                            DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]),
                            Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]),
                            EstensioneParticella = DbValue<string>.Get(sqlRead["ESTENSIONE_PARTICELLA"]),
                            IDAssenzaDatiCatastali = DbValue<int?>.Get(sqlRead["ID_ASSENZA_DATI_CATASTALI"]),
                            IDDestinazioneUso = DbValue<int?>.Get(sqlRead["ID_DESTINAZIONE_USO"]),
                            IDNaturaOccupante = DbValue<int?>.Get(sqlRead["ID_NATURA_OCCUPANTE"]),
                            IDTipoParticella = DbValue<string>.Get(sqlRead["ID_TIPO_PARTICELLA"]),
                            IDTipoUnita = DbValue<string>.Get(sqlRead["ID_TIPO_UNITA"]),
                            IDTitoloOccupazione = DbValue<int?>.Get(sqlRead["ID_TITOLO_OCCUPAZIONE"]),
                            Sezione = DbValue<string>.Get(sqlRead["SEZIONE"]),
                            AzioneASeguitoDiRettifica = DbValue<string>.Get(sqlRead["AZIONEASEGUITODIRETTIFICA"]),
                            NComponenti = DbValue<int>.Get(sqlRead["NCOMPONENTI"]),
                            MQCatasto = DbValue<double>.Get(sqlRead["MQCATASTO"]),
                            FKIdCategoriaAteco = DbValue<int>.Get(sqlRead["fk_IdCategoriaAteco"])
                        };
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLDETTAGLIOTESTATATARSU_IU";

                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlCmd.Parameters.AddWithValue("@IDDETTAGLIOTESTATA", DbParam.Get(IdDettaglioTestata));
                sqlCmd.Parameters.AddWithValue("@IDTESTATA", DbParam.Get(IdTestata));
                sqlCmd.Parameters.AddWithValue("@IDPADRE", DbParam.Get(IdPadre));
                sqlCmd.Parameters.AddWithValue("@CODVIA", DbParam.Get(CodVia));
                sqlCmd.Parameters.AddWithValue("@VIA", DbParam.Get(Via));
                sqlCmd.Parameters.AddWithValue("@CIVICO", DbParam.Get(Civico));
                sqlCmd.Parameters.AddWithValue("@ESPONENTE", DbParam.Get(Esponente));
                sqlCmd.Parameters.AddWithValue("@INTERNO", DbParam.Get(Interno));
                sqlCmd.Parameters.AddWithValue("@SCALA", DbParam.Get(Scala));
                sqlCmd.Parameters.AddWithValue("@FOGLIO", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@NUMERO", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@SUBALTERNO", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@DATA_INIZIO", DbParam.Get(DataInizio));
                sqlCmd.Parameters.AddWithValue("@DATA_FINE", DbParam.Get(DataFine));
                sqlCmd.Parameters.AddWithValue("@GGTARSU", DbParam.Get(GGTarsu));
                sqlCmd.Parameters.AddWithValue("@IDSTATOOCCUPAZIONE", DbParam.Get(IdStatoOccupazione));
                sqlCmd.Parameters.AddWithValue("@IDTIPOCONDUTTORE", DbParam.Get(IdTipoConduttore));
                sqlCmd.Parameters.AddWithValue("@NOMINATIVO_PROPRIETARIO", DbParam.Get(NominativoProprietario));
                sqlCmd.Parameters.AddWithValue("@NOMINATIVO_OCCUPANTE_PREC", DbParam.Get(NominativoOccupantePrec));
                sqlCmd.Parameters.AddWithValue("@NOTEDETTAGLIOTESTATA", DbParam.Get(NoteDettaglioTestata));
                sqlCmd.Parameters.AddWithValue("@DATA_INSERIMENTO", DbParam.Get(DataInserimento));
                sqlCmd.Parameters.AddWithValue("@DATA_VARIAZIONE", DbParam.Get(DataVariazione));
                sqlCmd.Parameters.AddWithValue("@DATA_CESSAZIONE", DbParam.Get(DataCessazione));
                sqlCmd.Parameters.AddWithValue("@OPERATORE", DbParam.Get(Operatore));
                sqlCmd.Parameters.AddWithValue("@ESTENSIONE_PARTICELLA", DbParam.Get(EstensioneParticella));
                sqlCmd.Parameters.AddWithValue("@ID_ASSENZA_DATI_CATASTALI", DbParam.Get(IDAssenzaDatiCatastali));
                sqlCmd.Parameters.AddWithValue("@ID_DESTINAZIONE_USO", DbParam.Get(IDDestinazioneUso));
                sqlCmd.Parameters.AddWithValue("@ID_NATURA_OCCUPANTE", DbParam.Get(IDNaturaOccupante));
                sqlCmd.Parameters.AddWithValue("@ID_TIPO_PARTICELLA", DbParam.Get(IDTipoParticella));
                sqlCmd.Parameters.AddWithValue("@ID_TIPO_UNITA", DbParam.Get(IDTipoUnita));
                sqlCmd.Parameters.AddWithValue("@ID_TITOLO_OCCUPAZIONE", DbParam.Get(IDTitoloOccupazione));
                sqlCmd.Parameters.AddWithValue("@SEZIONE", DbParam.Get(Sezione));
                sqlCmd.Parameters.AddWithValue("@AZIONEASEGUITODIRETTIFICA", DbParam.Get(AzioneASeguitoDiRettifica));
                sqlCmd.Parameters.AddWithValue("@NCOMPONENTI", DbParam.Get(NComponenti));
                sqlCmd.Parameters.AddWithValue("@MQCATASTO", DbParam.Get(MQCatasto));
                sqlCmd.Parameters.AddWithValue("@fk_IdCategoriaAteco", DbParam.Get(FKIdCategoriaAteco));

                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                ID = (int)sqlCmd.Parameters["@ID"].Value;
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLDETTAGLIOTESTATATARSU_D";
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
        #endregion

        #region Public Methods

        public DettaglioTestataTarsu[] LoadFiltered(string idEnte, DateTime dataFine)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLDETTAGLIOTESTATATARSU_F";
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                sqlCmd.Parameters.AddWithValue("@DataFine", DbParam.Get(dataFine));
                sqlRead = sqlCmd.ExecuteReader();

                List<DettaglioTestataTarsu> list = new List<DettaglioTestataTarsu>();
                while (sqlRead.Read())
                {
                    DettaglioTestataTarsu item = new DettaglioTestataTarsu
                    {
                        ID = DbValue<int>.Get(sqlRead["ID"]),
                        IdDettaglioTestata = DbValue<int>.Get(sqlRead["IDDETTAGLIOTESTATA"]),
                        IdTestata = DbValue<int>.Get(sqlRead["IDTESTATA"]),
                        IdPadre = DbValue<int?>.Get(sqlRead["IDPADRE"]),
                        CodVia = DbValue<int?>.Get(sqlRead["CODVIA"]),
                        Via = DbValue<string>.Get(sqlRead["VIA"]),
                        Civico = DbValue<int?>.Get(sqlRead["CIVICO"]),
                        Esponente = DbValue<string>.Get(sqlRead["ESPONENTE"]),
                        Interno = DbValue<string>.Get(sqlRead["INTERNO"]),
                        Scala = DbValue<string>.Get(sqlRead["SCALA"]),
                        Foglio = DbValue<string>.Get(sqlRead["FOGLIO"]),
                        Numero = DbValue<string>.Get(sqlRead["NUMERO"]),
                        Subalterno = DbValue<string>.Get(sqlRead["SUBALTERNO"]),
                        DataInizio = DbValue<DateTime>.Get(sqlRead["DATA_INIZIO"]),
                        DataFine = DbValue<DateTime?>.Get(sqlRead["DATA_FINE"]),
                        GGTarsu = DbValue<int?>.Get(sqlRead["GGTARSU"]),
                        IdStatoOccupazione = DbValue<string>.Get(sqlRead["IDSTATOOCCUPAZIONE"]),
                        IdTipoConduttore = DbValue<string>.Get(sqlRead["IDTIPOCONDUTTORE"]),
                        NominativoProprietario = DbValue<string>.Get(sqlRead["NOMINATIVO_PROPRIETARIO"]),
                        NominativoOccupantePrec = DbValue<string>.Get(sqlRead["NOMINATIVO_OCCUPANTE_PREC"]),
                        NoteDettaglioTestata = DbValue<string>.Get(sqlRead["NOTEDETTAGLIOTESTATA"]),
                        DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]),
                        DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]),
                        DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]),
                        Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]),
                        EstensioneParticella = DbValue<string>.Get(sqlRead["ESTENSIONE_PARTICELLA"]),
                        IDAssenzaDatiCatastali = DbValue<int?>.Get(sqlRead["ID_ASSENZA_DATI_CATASTALI"]),
                        IDDestinazioneUso = DbValue<int?>.Get(sqlRead["ID_DESTINAZIONE_USO"]),
                        IDNaturaOccupante = DbValue<int?>.Get(sqlRead["ID_NATURA_OCCUPANTE"]),
                        IDTipoParticella = DbValue<string>.Get(sqlRead["ID_TIPO_PARTICELLA"]),
                        IDTipoUnita = DbValue<string>.Get(sqlRead["ID_TIPO_UNITA"]),
                        IDTitoloOccupazione = DbValue<int?>.Get(sqlRead["ID_TITOLO_OCCUPAZIONE"]),
                        Sezione = DbValue<string>.Get(sqlRead["SEZIONE"]),
                        AzioneASeguitoDiRettifica = DbValue<string>.Get(sqlRead["AZIONEASEGUITODIRETTIFICA"]),
                        NComponenti = DbValue<int>.Get(sqlRead["NCOMPONENTI"]),
                        MQCatasto = DbValue<double>.Get(sqlRead["MQCATASTO"]),
                        FKIdCategoriaAteco = DbValue<int>.Get(sqlRead["fk_IdCategoriaAteco"])
                    };
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public void SetAtecoFromCategoria(string categoria, string idEnte)
        {
            try
            {
                Categorie category = new Categorie {CategoriaLegacy = categoria, CodEnte = idEnte};
                if (!category.Load()) return;
                FKIdCategoriaAteco = category.FKIdCategoriaAteco;
                Save();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
            }
        }

        public DettaglioImmobile[] LoadDettagli(string idEnte, string[] categorie)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GET_DETTAGLIO_IMMOBILI_BY_CATEGORIE";
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                for (int i = 0; i <= 4 && categorie.Length >= (i + 1); i++)
                    sqlCmd.Parameters.AddWithValue("@CAT" + (i + 1), DbParam.Get(categorie[i]));
                
                sqlRead = sqlCmd.ExecuteReader();

                List<DettaglioImmobile> list = new List<DettaglioImmobile>();
                while (sqlRead.Read())
                {
                    DettaglioImmobile item = new DettaglioImmobile
                    {
                        IdDettaglio = DbValue<int>.Get(sqlRead["ID"]),
                        CognomeDenominazione = DbValue<string>.Get(sqlRead["COGNOME_DENOMINAZIONE"]),
                        Nome = DbValue<string>.Get(sqlRead["NOME"]),
                        PartitaIVA = DbValue<string>.Get(sqlRead["PARTITA_IVA"]),
                        CodiceFiscale = DbValue<string>.Get(sqlRead["COD_FISCALE"]),
                        Via = DbValue<string>.Get(sqlRead["VIA"]),
                        Civico = DbValue<int>.Get(sqlRead["CIVICO"]),
                        Foglio = DbValue<string>.Get(sqlRead["FOGLIO"]),
                        Numero = DbValue<string>.Get(sqlRead["NUMERO"]),
                        Subalterno = DbValue<string>.Get(sqlRead["SUBALTERNO"]),
                        MQ = DbValue<double>.Get(sqlRead["MQ"])
                    };
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        #endregion
    }
    /// <summary>
    /// Classe per la gestione della transcodifica da TARSU a TARI dell'immobile
    /// </summary>
    public class DettaglioImmobile
    {
        public int IdDettaglio { get; set; }
        public string CognomeDenominazione { get; set; }
        public string Nome { get; set; }
        public string PartitaIVA { get; set; }
        public string CodiceFiscale { get; set; }
        public string Via { get; set; }
        public int Civico { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public double MQ { get; set; }
    }
}
