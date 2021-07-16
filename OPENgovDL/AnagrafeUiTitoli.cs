using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto delle titolarità dei soggetti sugli immobili
    /// </summary>
    public class AnagrafeUiTitoli : DbObject<AnagrafeUiTitoli>
    {
        #region Variables and constructor

        public AnagrafeUiTitoli()
        {
            Reset();
        }

        public AnagrafeUiTitoli(int idUITitoli)
        {
            Reset();
            IdUITitoli = idUITitoli;
        }
        #endregion

        #region Public properties
        public int IdUITitoli { get; set; }
        public int FKIdAnagrafeUI { get; set; }
        public int FKIdFlusso { get; set; }
        public int FKIdSoggetto { get; set; }
        public string FKCodiceDiritto { get; set; }
        public int IdentificativoTitolo { get; set; }
        public string CodAmministrativo { get; set; }
        public string Sezione { get; set; }
        public string DescrizioneTitolo { get; set; }
        public int QuotaNumeratore { get; set; }
        public int QuotaDenominatore { get; set; }
        public string Regime { get; set; }
        public int SoggettoRif { get; set; }
        public DateTime DataEfficaciaGenerazione { get; set; }
        public DateTime DataRegistazioneGenerazione { get; set; }
        public string TipoNotaGenerazione { get; set; }
        public string NumeroNotaGenerazione { get; set; }
        public string ProgNotaGenerazione { get; set; }
        public int AnnoNotaGenerazione { get; set; }
        public DateTime DataEfficaciaConclusione { get; set; }
        public DateTime DataRegistazioneConclusione { get; set; }
        public string TipoNotaConclusione { get; set; }
        public string NumeroNotaConclusione { get; set; }
        public string ProgNotaConclusione { get; set; }
        public int AnnoNotaConclusione { get; set; }
        public string Partita { get; set; }
        public int IdentificativoMutazioneIniziale { get; set; }
        public int IdentificativoMutazioneFinale { get; set; }
        public string CodiceCausaleAttoGenerante { get; set; }
        public string DescrizioneAttoGenerante { get; set; }
        public string CodiceCausaleAttoConclusivo { get; set; }
        public string DescrizioneAttoConclusivo { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiTitoli) &&
                ((obj as AnagrafeUiTitoli).IdUITitoli == IdUITitoli);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUITitoli);
        }

        public override sealed void Reset()
        {
            IdUITitoli = default(int);
            FKIdAnagrafeUI = default(int);
            FKIdFlusso = default(int);
            FKIdSoggetto = default(int);
            FKCodiceDiritto = string.Empty;
            IdentificativoTitolo = default(int);
            CodAmministrativo = string.Empty;
            Sezione = string.Empty;
            DescrizioneTitolo = string.Empty;
            QuotaNumeratore = default(int);
            QuotaDenominatore = default(int);
            Regime = string.Empty;
            SoggettoRif = default(int);
            DataEfficaciaGenerazione = default(DateTime);
            DataRegistazioneGenerazione = default(DateTime);
            TipoNotaGenerazione = string.Empty;
            NumeroNotaGenerazione = string.Empty;
            ProgNotaGenerazione = string.Empty;
            AnnoNotaGenerazione = default(int);
            DataEfficaciaConclusione = default(DateTime);
            DataRegistazioneConclusione = default(DateTime);
            TipoNotaConclusione = string.Empty;
            NumeroNotaConclusione = string.Empty;
            ProgNotaConclusione = string.Empty;
            AnnoNotaConclusione = default(int);
            Partita = string.Empty;
            IdentificativoMutazioneIniziale = default(int);
            IdentificativoMutazioneFinale = default(int);
            CodiceCausaleAttoGenerante = string.Empty;
            DescrizioneAttoGenerante = string.Empty;
            CodiceCausaleAttoConclusivo = string.Empty;
            DescrizioneAttoConclusivo = string.Empty;
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_TITOLI_S";
                sqlCmd.Parameters.AddWithValue("@IdUITitoli", DbParam.Get(IdUITitoli));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FKIdAnagrafeUI = DbValue<int>.Get(sqlRead["fK_IdAnagrafeUI"]);
                    FKIdFlusso = DbValue<int>.Get(sqlRead["fk_IdFlusso"]);
                    FKIdSoggetto = DbValue<int>.Get(sqlRead["fk_IdUISoggetto"]);
                    FKCodiceDiritto = DbValue<string>.Get(sqlRead["fk_CodiceDiritto"]);
                    IdentificativoTitolo = DbValue<int>.Get(sqlRead["IdentificativoTitolo"]);
                    CodAmministrativo = DbValue<string>.Get(sqlRead["Cod_Amministrativo"]);
                    Sezione = DbValue<string>.Get(sqlRead["Sezione"]);
                    DescrizioneTitolo = DbValue<string>.Get(sqlRead["DescrizioneTitolo"]);
                    QuotaNumeratore = DbValue<int>.Get(sqlRead["QuotaNumeratore"]);
                    QuotaDenominatore = DbValue<int>.Get(sqlRead["QuotaDenominatore"]);
                    Regime = DbValue<string>.Get(sqlRead["Regime"]);
                    SoggettoRif = DbValue<int>.Get(sqlRead["SoggettoRif"]);
                    DataEfficaciaGenerazione = DbValue<DateTime>.Get(sqlRead["Data_Efficacia_Generazione"]);
                    DataRegistazioneGenerazione = DbValue<DateTime>.Get(sqlRead["Data_Registazione_Generazione"]);
                    TipoNotaGenerazione = DbValue<string>.Get(sqlRead["Tipo_Nota_Generazione"]);
                    NumeroNotaGenerazione = DbValue<string>.Get(sqlRead["Numero_Nota_Generazione"]);
                    ProgNotaGenerazione = DbValue<string>.Get(sqlRead["Prog_Nota_Generazione"]);
                    AnnoNotaGenerazione = DbValue<int>.Get(sqlRead["Anno_Nota_Generazione"]);
                    DataEfficaciaConclusione = DbValue<DateTime>.Get(sqlRead["Data_Efficacia_Conclusione"]);
                    DataRegistazioneConclusione = DbValue<DateTime>.Get(sqlRead["Data_Registazione_Conclusione"]);
                    TipoNotaConclusione = DbValue<string>.Get(sqlRead["Tipo_Nota_Conclusione"]);
                    NumeroNotaConclusione = DbValue<string>.Get(sqlRead["Numero_Nota_Conclusione"]);
                    ProgNotaConclusione = DbValue<string>.Get(sqlRead["Prog_Nota_Conclusione"]);
                    AnnoNotaConclusione = DbValue<int>.Get(sqlRead["Anno_Nota_Conclusione"]);
                    Partita = DbValue<string>.Get(sqlRead["Partita"]);
                    IdentificativoMutazioneIniziale = DbValue<int>.Get(sqlRead["Identificativo_Mutazione_Iniziale"]);
                    IdentificativoMutazioneFinale = DbValue<int>.Get(sqlRead["Identificativo_Mutazione_Finale"]);
                    CodiceCausaleAttoGenerante = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Generante"]);
                    DescrizioneAttoGenerante = DbValue<string>.Get(sqlRead["Descrizione_Atto_Generante"]);
                    CodiceCausaleAttoConclusivo = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Conclusivo"]);
                    DescrizioneAttoConclusivo = DbValue<string>.Get(sqlRead["Descrizione_Atto_Conclusivo"]);
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

        public override AnagrafeUiTitoli[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_TITOLI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiTitoli> list = new List<AnagrafeUiTitoli>();
                while (sqlRead.Read())
                {
                    AnagrafeUiTitoli item = new AnagrafeUiTitoli
                        {
                            IdUITitoli = DbValue<int>.Get(sqlRead["IdUITitoli"]),
                            FKIdAnagrafeUI = DbValue<int>.Get(sqlRead["fK_IdAnagrafeUI"]),
                            FKIdFlusso = DbValue<int>.Get(sqlRead["fk_IdFlusso"]),
                            FKIdSoggetto = DbValue<int>.Get(sqlRead["fk_IdUISoggetto"]),
                            FKCodiceDiritto = DbValue<string>.Get(sqlRead["fk_CodiceDiritto"]),
                            IdentificativoTitolo = DbValue<int>.Get(sqlRead["IdentificativoTitolo"]),
                            CodAmministrativo = DbValue<string>.Get(sqlRead["Cod_Amministrativo"]),
                            Sezione = DbValue<string>.Get(sqlRead["Sezione"]),
                            DescrizioneTitolo = DbValue<string>.Get(sqlRead["DescrizioneTitolo"]),
                            QuotaNumeratore = DbValue<int>.Get(sqlRead["QuotaNumeratore"]),
                            QuotaDenominatore = DbValue<int>.Get(sqlRead["QuotaDenominatore"]),
                            Regime = DbValue<string>.Get(sqlRead["Regime"]),
                            SoggettoRif = DbValue<int>.Get(sqlRead["SoggettoRif"]),
                            DataEfficaciaGenerazione = DbValue<DateTime>.Get(sqlRead["Data_Efficacia_Generazione"]),
                            DataRegistazioneGenerazione = DbValue<DateTime>.Get(sqlRead["Data_Registazione_Generazione"]),
                            TipoNotaGenerazione = DbValue<string>.Get(sqlRead["Tipo_Nota_Generazione"]),
                            NumeroNotaGenerazione = DbValue<string>.Get(sqlRead["Numero_Nota_Generazione"]),
                            ProgNotaGenerazione = DbValue<string>.Get(sqlRead["Prog_Nota_Generazione"]),
                            AnnoNotaGenerazione = DbValue<int>.Get(sqlRead["Anno_Nota_Generazione"]),
                            DataEfficaciaConclusione = DbValue<DateTime>.Get(sqlRead["Data_Efficacia_Conclusione"]),
                            DataRegistazioneConclusione = DbValue<DateTime>.Get(sqlRead["Data_Registazione_Conclusione"]),
                            TipoNotaConclusione = DbValue<string>.Get(sqlRead["Tipo_Nota_Conclusione"]),
                            NumeroNotaConclusione = DbValue<string>.Get(sqlRead["Numero_Nota_Conclusione"]),
                            ProgNotaConclusione = DbValue<string>.Get(sqlRead["Prog_Nota_Conclusione"]),
                            AnnoNotaConclusione = DbValue<int>.Get(sqlRead["Anno_Nota_Conclusione"]),
                            Partita = DbValue<string>.Get(sqlRead["Partita"]),
                            IdentificativoMutazioneIniziale =
                                DbValue<int>.Get(sqlRead["Identificativo_Mutazione_Iniziale"]),
                            IdentificativoMutazioneFinale = DbValue<int>.Get(sqlRead["Identificativo_Mutazione_Finale"]),
                            CodiceCausaleAttoGenerante = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Generante"]),
                            DescrizioneAttoGenerante = DbValue<string>.Get(sqlRead["Descrizione_Atto_Generante"]),
                            CodiceCausaleAttoConclusivo = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Conclusivo"]),
                            DescrizioneAttoConclusivo = DbValue<string>.Get(sqlRead["Descrizione_Atto_Conclusivo"])
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

        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_TITOLI_IU";

                sqlCmd.Parameters.AddWithValue("@IdUITitoli", DbParam.Get(IdUITitoli));
                sqlCmd.Parameters.AddWithValue("@fK_IdAnagrafeUI", DbParam.Get(FKIdAnagrafeUI));
                sqlCmd.Parameters.AddWithValue("@fk_IdFlusso", DbParam.Get(FKIdFlusso));
                sqlCmd.Parameters.AddWithValue("@fk_IdUISoggetto", DbParam.Get(FKIdSoggetto));
                sqlCmd.Parameters.AddWithValue("@fk_CodiceDiritto", DbParam.Get(FKCodiceDiritto));
                sqlCmd.Parameters.AddWithValue("@IdentificativoTitolo", DbParam.Get(IdentificativoTitolo));
                sqlCmd.Parameters.AddWithValue("@Cod_Amministrativo", DbParam.Get(CodAmministrativo));
                sqlCmd.Parameters.AddWithValue("@Sezione", DbParam.Get(Sezione));
                sqlCmd.Parameters.AddWithValue("@DescrizioneTitolo", DbParam.Get(DescrizioneTitolo));
                sqlCmd.Parameters.AddWithValue("@QuotaNumeratore", DbParam.Get(QuotaNumeratore));
                sqlCmd.Parameters.AddWithValue("@QuotaDenominatore", DbParam.Get(QuotaDenominatore));
                sqlCmd.Parameters.AddWithValue("@Regime", DbParam.Get(Regime));
                sqlCmd.Parameters.AddWithValue("@SoggettoRif", DbParam.Get(SoggettoRif));
                sqlCmd.Parameters.AddWithValue("@Data_Efficacia_Generazione", DbParam.Get(DataEfficaciaGenerazione));
                sqlCmd.Parameters.AddWithValue("@Data_Registazione_Generazione", DbParam.Get(DataRegistazioneGenerazione));
                sqlCmd.Parameters.AddWithValue("@Tipo_Nota_Generazione", DbParam.Get(TipoNotaGenerazione));
                sqlCmd.Parameters.AddWithValue("@Numero_Nota_Generazione", DbParam.Get(NumeroNotaGenerazione));
                sqlCmd.Parameters.AddWithValue("@Prog_Nota_Generazione", DbParam.Get(ProgNotaGenerazione));
                sqlCmd.Parameters.AddWithValue("@Anno_Nota_Generazione", DbParam.Get(AnnoNotaGenerazione));
                sqlCmd.Parameters.AddWithValue("@Data_Efficacia_Conclusione", DbParam.Get(DataEfficaciaConclusione));
                sqlCmd.Parameters.AddWithValue("@Data_Registazione_Conclusione", DbParam.Get(DataRegistazioneConclusione));
                sqlCmd.Parameters.AddWithValue("@Tipo_Nota_Conclusione", DbParam.Get(TipoNotaConclusione));
                sqlCmd.Parameters.AddWithValue("@Numero_Nota_Conclusione", DbParam.Get(NumeroNotaConclusione));
                sqlCmd.Parameters.AddWithValue("@Prog_Nota_Conclusione", DbParam.Get(ProgNotaConclusione));
                sqlCmd.Parameters.AddWithValue("@Anno_Nota_Conclusione", DbParam.Get(AnnoNotaConclusione));
                sqlCmd.Parameters.AddWithValue("@Partita", DbParam.Get(Partita));
                sqlCmd.Parameters.AddWithValue("@Identificativo_Mutazione_Iniziale", DbParam.Get(IdentificativoMutazioneIniziale));
                sqlCmd.Parameters.AddWithValue("@Identificativo_Mutazione_Finale", DbParam.Get(IdentificativoMutazioneFinale));
                sqlCmd.Parameters.AddWithValue("@Codice_Causale_Atto_Generante", DbParam.Get(CodiceCausaleAttoGenerante));
                sqlCmd.Parameters.AddWithValue("@Descrizione_Atto_Generante", DbParam.Get(DescrizioneAttoGenerante));
                sqlCmd.Parameters.AddWithValue("@Codice_Causale_Atto_Conclusivo", DbParam.Get(CodiceCausaleAttoConclusivo));
                sqlCmd.Parameters.AddWithValue("@Descrizione_Atto_Conclusivo", DbParam.Get(DescrizioneAttoConclusivo));

                sqlCmd.Parameters["@IdUITitoli"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUITitoli = (int)sqlCmd.Parameters["@IdUITitoli"].Value;
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

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_TITOLI_D";
                sqlCmd.Parameters.AddWithValue("@IdUITitoli", DbParam.Get(IdUITitoli));
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

        public bool Exists()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_TITOLI_S";
                if (IdentificativoTitolo != default(int))
                    sqlCmd.Parameters.AddWithValue("@IdentificativoTitolo", DbParam.Get(IdentificativoTitolo));
                sqlRead = sqlCmd.ExecuteReader();

                return (sqlRead.Read());
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
        #endregion
    }
}
