using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto delle variazioni immobile
    /// </summary>
    public class AnagrafeUiMovimenti : DbObject<AnagrafeUiMovimenti>
    {
        #region Variables and constructor

        public AnagrafeUiMovimenti()
        {
            Reset();
        }

        public AnagrafeUiMovimenti(int idUIMovimenti)
        {
            Reset();
            IdUIMovimenti = idUIMovimenti;
        }
        #endregion

        #region Public properties

        public int IdUIMovimenti { get; set; }
        public int FKIdAnagrafeUI { get; set; }
        public int FKIdFlusso { get; set; }
        public string CodAmministrativo { get; set; }
        public string Sezione { get; set; }
        public int IdentificativoImmobile { get; set; }
        public string TipoImmobile { get; set; }
        public int Progressivo { get; set; }
        public string Zona { get; set; }
        public string Categoria { get; set; }
        public string Classe { get; set; }
        public double? Consistenza { get; set; }
        public double? Superficie { get; set; }
        public int? RenditaLire { get; set; }
        public double? RenditaEuro { get; set; }
        public string Lotto { get; set; }
        public string Edificio { get; set; }
        public string Scala { get; set; }
        public string Interno1 { get; set; }
        public string Interno2 { get; set; }
        public string Piano1 { get; set; }
        public string Piano2 { get; set; }
        public string Piano3 { get; set; }
        public string Piano4 { get; set; }
        public DateTime? DataEfficaciaGenerazione { get; set; }
        public DateTime? DataRegistazioneGenerazione { get; set; }
        public string TipoNotaGenerazione { get; set; }
        public string NumeroNotaGenerazione { get; set; }
        public string ProgNotaGenerazione { get; set; }
        public int? AnnoNotaGenerazione { get; set; }
        public DateTime? DataEfficaciaConclusione { get; set; }
        public DateTime? DataRegistazioneConclusione { get; set; }
        public string TipoNotaConclusione { get; set; }
        public string NumeroNotaConclusione { get; set; }
        public string ProgNotaConclusione { get; set; }
        public int? AnnoNotaConclusione { get; set; }
        public string Partita { get; set; }
        public string Annotazione { get; set; }
        public int? IdentificativoMutazioneIniziale { get; set; }
        public int? IdentificativoMutazioneFinale { get; set; }
        public string ProtocolloNotifica { get; set; }
        public DateTime? DataNotifica { get; set; }
        public string CodiceCausaleAttoGenerante { get; set; }
        public string DescrizioneAttoGenerante { get; set; }
        public string CodiceCausaleAttoConclusivo { get; set; }
        public string DescrizioneAttoConclusivo { get; set; }
        public int? FlagClassamento { get; set; }
        public string CodiceRiserva { get; set; }
        public string PartitaRiserva { get; set; }

        public List<AnagrafeUiGraffati> Graffati { get; set; }
        public List<AnagrafeUiIndirizzi> Indirizzi { get; set; }
        public List<AnagrafeUiRiserve> Riserve { get; set; }
        public List<AnagrafeUiVani> Vani { get; set; }
        public List<AnagrafeUiSoggetti> Soggetti { get; set; } 

        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiMovimenti) &&
                ((obj as AnagrafeUiMovimenti).IdUIMovimenti == IdUIMovimenti);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUIMovimenti);
        }

        public override sealed void Reset()
        {
            IdUIMovimenti = default(int);
            FKIdAnagrafeUI = default(int);
            FKIdFlusso = default(int);
            CodAmministrativo = default(string);
            Sezione = default(string);
            IdentificativoImmobile = default(int);
            TipoImmobile = default(string);
            Progressivo = default(int);
            Zona = default(string);
            Categoria = default(string);
            Classe = default(string);
            Consistenza = default(double?);
            Superficie = default(double?);
            RenditaLire = default(int?);
            RenditaEuro = default(double?);
            Lotto = default(string);
            Edificio = default(string);
            Scala = default(string);
            Interno1 = default(string);
            Interno2 = default(string);
            Piano1 = default(string);
            Piano2 = default(string);
            Piano3 = default(string);
            Piano4 = default(string);
            DataEfficaciaGenerazione = default(DateTime?);
            DataRegistazioneGenerazione = default(DateTime?);
            TipoNotaGenerazione = default(string);
            NumeroNotaGenerazione = default(string);
            ProgNotaGenerazione = default(string);
            AnnoNotaGenerazione = default(int?);
            DataEfficaciaConclusione = default(DateTime?);
            DataRegistazioneConclusione = default(DateTime?);
            TipoNotaConclusione = default(string);
            NumeroNotaConclusione = default(string);
            ProgNotaConclusione = default(string);
            AnnoNotaConclusione = default(int?);
            Partita = default(string);
            Annotazione = default(string);
            IdentificativoMutazioneIniziale = default(int?);
            IdentificativoMutazioneFinale = default(int?);
            ProtocolloNotifica = default(string);
            DataNotifica = default(DateTime?);
            CodiceCausaleAttoGenerante = default(string);
            DescrizioneAttoGenerante = default(string);
            CodiceCausaleAttoConclusivo = default(string);
            DescrizioneAttoConclusivo = default(string);
            FlagClassamento = default(int?);
            CodiceRiserva = PartitaRiserva = string.Empty;

            Graffati = new List<AnagrafeUiGraffati>();
            Indirizzi = new List<AnagrafeUiIndirizzi>();
            Riserve = new List<AnagrafeUiRiserve>();
            Vani = new List<AnagrafeUiVani>();
            Soggetti = new List<AnagrafeUiSoggetti>();
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
                sqlCmd.CommandText = "ANAGRAFE_UI_MOVIMENTI_S";
                sqlCmd.Parameters.AddWithValue("@IdUIMovimenti", DbParam.Get(IdUIMovimenti));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FKIdAnagrafeUI = DbValue<int>.Get(sqlRead["fK_IdAnagrafeUI"]);
                    FKIdFlusso = DbValue<int>.Get(sqlRead["fk_IdFlusso"]);
                    CodAmministrativo = DbValue<string>.Get(sqlRead["Cod_Amministrativo"]);
                    Sezione = DbValue<string>.Get(sqlRead["Sezione"]);
                    IdentificativoImmobile = DbValue<int>.Get(sqlRead["Identificativo_Immobile"]);
                    TipoImmobile = DbValue<string>.Get(sqlRead["Tipo_Immobile"]);
                    Progressivo = DbValue<int>.Get(sqlRead["Progressivo"]);
                    Zona = DbValue<string>.Get(sqlRead["Zona"]);
                    Categoria = DbValue<string>.Get(sqlRead["Categoria"]);
                    Classe = DbValue<string>.Get(sqlRead["Classe"]);
                    Consistenza = DbValue<double?>.Get(sqlRead["Consistenza"]);
                    Superficie = DbValue<double?>.Get(sqlRead["Superficie"]);
                    RenditaLire = DbValue<int?>.Get(sqlRead["Rendita_Lire"]);
                    RenditaEuro = DbValue<double?>.Get(sqlRead["Rendita_Euro"]);
                    Lotto = DbValue<string>.Get(sqlRead["Lotto"]);
                    Edificio = DbValue<string>.Get(sqlRead["Edificio"]);
                    Scala = DbValue<string>.Get(sqlRead["Scala"]);
                    Interno1 = DbValue<string>.Get(sqlRead["Interno1"]);
                    Interno2 = DbValue<string>.Get(sqlRead["Interno2"]);
                    Piano1 = DbValue<string>.Get(sqlRead["Piano1"]);
                    Piano2 = DbValue<string>.Get(sqlRead["Piano2"]);
                    Piano3 = DbValue<string>.Get(sqlRead["Piano3"]);
                    Piano4 = DbValue<string>.Get(sqlRead["Piano4"]);
                    DataEfficaciaGenerazione = DbValue<DateTime?>.Get(sqlRead["Data_Efficacia_Generazione"]);
                    DataRegistazioneGenerazione = DbValue<DateTime?>.Get(sqlRead["Data_Registazione_Generazione"]);
                    TipoNotaGenerazione = DbValue<string>.Get(sqlRead["Tipo_Nota_Generazione"]);
                    NumeroNotaGenerazione = DbValue<string>.Get(sqlRead["Numero_Nota_Generazione"]);
                    ProgNotaGenerazione = DbValue<string>.Get(sqlRead["Prog_Nota_Generazione"]);
                    AnnoNotaGenerazione = DbValue<int?>.Get(sqlRead["Anno_Nota_Generazione"]);
                    DataEfficaciaConclusione = DbValue<DateTime?>.Get(sqlRead["Data_Efficacia_Conclusione"]);
                    DataRegistazioneConclusione = DbValue<DateTime?>.Get(sqlRead["Data_Registazione_Conclusione"]);
                    TipoNotaConclusione = DbValue<string>.Get(sqlRead["Tipo_Nota_Conclusione"]);
                    NumeroNotaConclusione = DbValue<string>.Get(sqlRead["Numero_Nota_Conclusione"]);
                    ProgNotaConclusione = DbValue<string>.Get(sqlRead["Prog_Nota_Conclusione"]);
                    AnnoNotaConclusione = DbValue<int?>.Get(sqlRead["Anno_Nota_Conclusione"]);
                    Partita = DbValue<string>.Get(sqlRead["Partita"]);
                    Annotazione = DbValue<string>.Get(sqlRead["Annotazione"]);
                    IdentificativoMutazioneIniziale = DbValue<int?>.Get(sqlRead["Identificativo_Mutazione_Iniziale"]);
                    IdentificativoMutazioneFinale = DbValue<int?>.Get(sqlRead["Identificativo_Mutazione_Finale"]);
                    ProtocolloNotifica = DbValue<string>.Get(sqlRead["Protocollo_Notifica"]);
                    DataNotifica = DbValue<DateTime?>.Get(sqlRead["Data_Notifica"]);
                    CodiceCausaleAttoGenerante = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Generante"]);
                    DescrizioneAttoGenerante = DbValue<string>.Get(sqlRead["Descrizione_Atto_Generante"]);
                    CodiceCausaleAttoConclusivo = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Conclusivo"]);
                    DescrizioneAttoConclusivo = DbValue<string>.Get(sqlRead["Descrizione_Atto_Conclusivo"]);
                    FlagClassamento = DbValue<int?>.Get(sqlRead["Flag_Classamento"]);
                    CodiceRiserva = DbValue<string>.Get(sqlRead["Codice_Riserva"]);
                    PartitaRiserva = DbValue<string>.Get(sqlRead["Partita_Riserva"]);
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

        public override AnagrafeUiMovimenti[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_MOVIMENTI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiMovimenti> list = new List<AnagrafeUiMovimenti>();
                while (sqlRead.Read())
                {
                    AnagrafeUiMovimenti item = new AnagrafeUiMovimenti();
                    item.IdUIMovimenti = DbValue<int>.Get(sqlRead["IdUIMovimenti"]);
                    item.FKIdAnagrafeUI = DbValue<int>.Get(sqlRead["fK_IdAnagrafeUI"]);
                    item.FKIdFlusso = DbValue<int>.Get(sqlRead["fk_IdFlusso"]);
                    item.CodAmministrativo = DbValue<string>.Get(sqlRead["Cod_Amministrativo"]);
                    item.Sezione = DbValue<string>.Get(sqlRead["Sezione"]);
                    item.IdentificativoImmobile = DbValue<int>.Get(sqlRead["Identificativo_Immobile"]);
                    item.TipoImmobile = DbValue<string>.Get(sqlRead["Tipo_Immobile"]);
                    item.Progressivo = DbValue<int>.Get(sqlRead["Progressivo"]);
                    item.Zona = DbValue<string>.Get(sqlRead["Zona"]);
                    item.Categoria = DbValue<string>.Get(sqlRead["Categoria"]);
                    item.Classe = DbValue<string>.Get(sqlRead["Classe"]);
                    item.Consistenza = DbValue<double?>.Get(sqlRead["Consistenza"]);
                    item.Superficie = DbValue<double?>.Get(sqlRead["Superficie"]);
                    item.RenditaLire = DbValue<int?>.Get(sqlRead["Rendita_Lire"]);
                    item.RenditaEuro = DbValue<double?>.Get(sqlRead["Rendita_Euro"]);
                    item.Lotto = DbValue<string>.Get(sqlRead["Lotto"]);
                    item.Edificio = DbValue<string>.Get(sqlRead["Edificio"]);
                    item.Scala = DbValue<string>.Get(sqlRead["Scala"]);
                    item.Interno1 = DbValue<string>.Get(sqlRead["Interno1"]);
                    item.Interno2 = DbValue<string>.Get(sqlRead["Interno2"]);
                    item.Piano1 = DbValue<string>.Get(sqlRead["Piano1"]);
                    item.Piano2 = DbValue<string>.Get(sqlRead["Piano2"]);
                    item.Piano3 = DbValue<string>.Get(sqlRead["Piano3"]);
                    item.Piano4 = DbValue<string>.Get(sqlRead["Piano4"]);
                    item.DataEfficaciaGenerazione = DbValue<DateTime?>.Get(sqlRead["Data_Efficacia_Generazione"]);
                    item.DataRegistazioneGenerazione = DbValue<DateTime?>.Get(sqlRead["Data_Registazione_Generazione"]);
                    item.TipoNotaGenerazione = DbValue<string>.Get(sqlRead["Tipo_Nota_Generazione"]);
                    item.NumeroNotaGenerazione = DbValue<string>.Get(sqlRead["Numero_Nota_Generazione"]);
                    item.ProgNotaGenerazione = DbValue<string>.Get(sqlRead["Prog_Nota_Generazione"]);
                    item.AnnoNotaGenerazione = DbValue<int?>.Get(sqlRead["Anno_Nota_Generazione"]);
                    item.DataEfficaciaConclusione = DbValue<DateTime?>.Get(sqlRead["Data_Efficacia_Conclusione"]);
                    item.DataRegistazioneConclusione = DbValue<DateTime?>.Get(sqlRead["Data_Registazione_Conclusione"]);
                    item.TipoNotaConclusione = DbValue<string>.Get(sqlRead["Tipo_Nota_Conclusione"]);
                    item.NumeroNotaConclusione = DbValue<string>.Get(sqlRead["Numero_Nota_Conclusione"]);
                    item.ProgNotaConclusione = DbValue<string>.Get(sqlRead["Prog_Nota_Conclusione"]);
                    item.AnnoNotaConclusione = DbValue<int?>.Get(sqlRead["Anno_Nota_Conclusione"]);
                    item.Partita = DbValue<string>.Get(sqlRead["Partita"]);
                    item.Annotazione = DbValue<string>.Get(sqlRead["Annotazione"]);
                    item.IdentificativoMutazioneIniziale = DbValue<int?>.Get(sqlRead["Identificativo_Mutazione_Iniziale"]);
                    item.IdentificativoMutazioneFinale = DbValue<int?>.Get(sqlRead["Identificativo_Mutazione_Finale"]);
                    item.ProtocolloNotifica = DbValue<string>.Get(sqlRead["Protocollo_Notifica"]);
                    item.DataNotifica = DbValue<DateTime?>.Get(sqlRead["Data_Notifica"]);
                    item.CodiceCausaleAttoGenerante = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Generante"]);
                    item.DescrizioneAttoGenerante = DbValue<string>.Get(sqlRead["Descrizione_Atto_Generante"]);
                    item.CodiceCausaleAttoConclusivo = DbValue<string>.Get(sqlRead["Codice_Causale_Atto_Conclusivo"]);
                    item.DescrizioneAttoConclusivo = DbValue<string>.Get(sqlRead["Descrizione_Atto_Conclusivo"]);
                    item.FlagClassamento = DbValue<int?>.Get(sqlRead["Flag_Classamento"]);
                    item.CodiceRiserva = DbValue<string>.Get(sqlRead["Codice_Riserva"]);
                    item.PartitaRiserva = DbValue<string>.Get(sqlRead["Partita_Riserva"]);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_MOVIMENTI_IU";

                sqlCmd.Parameters.AddWithValue("@IdUIMovimenti", DbParam.Get(IdUIMovimenti));
                sqlCmd.Parameters.AddWithValue("@fK_IdAnagrafeUI", DbParam.Get(FKIdAnagrafeUI));
                sqlCmd.Parameters.AddWithValue("@fk_IdFlusso", DbParam.Get(FKIdFlusso));
                sqlCmd.Parameters.AddWithValue("@Cod_Amministrativo", DbParam.Get(CodAmministrativo));
                sqlCmd.Parameters.AddWithValue("@Sezione", DbParam.Get(Sezione));
                sqlCmd.Parameters.AddWithValue("@Identificativo_Immobile", DbParam.Get(IdentificativoImmobile));
                sqlCmd.Parameters.AddWithValue("@Tipo_Immobile", DbParam.Get(TipoImmobile));
                sqlCmd.Parameters.AddWithValue("@Progressivo", DbParam.Get(Progressivo));
                sqlCmd.Parameters.AddWithValue("@Zona", DbParam.Get(Zona));
                sqlCmd.Parameters.AddWithValue("@Categoria", DbParam.Get(Categoria));
                sqlCmd.Parameters.AddWithValue("@Classe", DbParam.Get(Classe));
                sqlCmd.Parameters.AddWithValue("@Consistenza", DbParam.Get(Consistenza));
                sqlCmd.Parameters.AddWithValue("@Superficie", DbParam.Get(Superficie));
                sqlCmd.Parameters.AddWithValue("@Rendita_Lire", DbParam.Get(RenditaLire));
                sqlCmd.Parameters.AddWithValue("@Rendita_Euro", DbParam.Get(RenditaEuro));
                sqlCmd.Parameters.AddWithValue("@Lotto", DbParam.Get(Lotto));
                sqlCmd.Parameters.AddWithValue("@Edificio", DbParam.Get(Edificio));
                sqlCmd.Parameters.AddWithValue("@Scala", DbParam.Get(Scala));
                sqlCmd.Parameters.AddWithValue("@Interno1", DbParam.Get(Interno1));
                sqlCmd.Parameters.AddWithValue("@Interno2", DbParam.Get(Interno2));
                sqlCmd.Parameters.AddWithValue("@Piano1", DbParam.Get(Piano1));
                sqlCmd.Parameters.AddWithValue("@Piano2", DbParam.Get(Piano2));
                sqlCmd.Parameters.AddWithValue("@Piano3", DbParam.Get(Piano3));
                sqlCmd.Parameters.AddWithValue("@Piano4", DbParam.Get(Piano4));
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
                sqlCmd.Parameters.AddWithValue("@Annotazione", DbParam.Get(Annotazione));
                sqlCmd.Parameters.AddWithValue("@Identificativo_Mutazione_Iniziale", DbParam.Get(IdentificativoMutazioneIniziale));
                sqlCmd.Parameters.AddWithValue("@Identificativo_Mutazione_Finale", DbParam.Get(IdentificativoMutazioneFinale));
                sqlCmd.Parameters.AddWithValue("@Protocollo_Notifica", DbParam.Get(ProtocolloNotifica));
                sqlCmd.Parameters.AddWithValue("@Data_Notifica", DbParam.Get(DataNotifica));
                sqlCmd.Parameters.AddWithValue("@Codice_Causale_Atto_Generante", DbParam.Get(CodiceCausaleAttoGenerante));
                sqlCmd.Parameters.AddWithValue("@Descrizione_Atto_Generante", DbParam.Get(DescrizioneAttoGenerante));
                sqlCmd.Parameters.AddWithValue("@Codice_Causale_Atto_Conclusivo", DbParam.Get(CodiceCausaleAttoConclusivo));
                sqlCmd.Parameters.AddWithValue("@Descrizione_Atto_Conclusivo", DbParam.Get(DescrizioneAttoConclusivo));
                sqlCmd.Parameters.AddWithValue("@Flag_Classamento", DbParam.Get(FlagClassamento));
                sqlCmd.Parameters.AddWithValue("@Codice_Riserva", DbParam.Get(CodiceRiserva));
                sqlCmd.Parameters.AddWithValue("@Partita_Riserva", DbParam.Get(PartitaRiserva));

                sqlCmd.Parameters["@IdUIMovimenti"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUIMovimenti = (int)sqlCmd.Parameters["@IdUIMovimenti"].Value;

                if ((Graffati != null) && (Graffati.Count > 0))
                {
                    foreach (AnagrafeUiGraffati graffato in Graffati)
                    {
                        graffato.FKIdUIMovimenti = IdUIMovimenti;
                        graffato.Save(ref errore);
                    }
                }

                if ((Indirizzi != null) && (Indirizzi.Count > 0))
                {
                    foreach (AnagrafeUiIndirizzi indirizzo in Indirizzi)
                    {
                        indirizzo.FkidUIMovimenti = IdUIMovimenti;
                        indirizzo.Save(ref errore);
                    }
                }

                if ((Riserve != null) && (Riserve.Count > 0))
                {
                    foreach (AnagrafeUiRiserve riserva in Riserve)
                    {
                        riserva.FKIdUIMovimenti = IdUIMovimenti;
                        riserva.Save(ref errore);
                    }
                }

                if ((Vani != null) && (Vani.Count > 0))
                {
                    foreach (AnagrafeUiVani vano in Vani)
                    {
                        vano.FKIdUIMovimenti = IdUIMovimenti;
                        vano.Save(ref errore);
                    }
                }

                if ((Soggetti != null) && (Soggetti.Count > 0))
                {
                    foreach (AnagrafeUiSoggetti soggetto in Soggetti)
                    {
                        if (!soggetto.Exists())
                            soggetto.Save(ref errore);
                        soggetto.Assign(IdUIMovimenti);
                    }
                }
                
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
                sqlCmd.CommandText = "ANAGRAFE_UI_MOVIMENTI_D";
                sqlCmd.Parameters.AddWithValue("@IdUIMovimenti", DbParam.Get(IdUIMovimenti));
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
    }
}
