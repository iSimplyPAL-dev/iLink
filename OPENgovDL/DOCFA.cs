using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;
using Utility;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da DOCFA
    /// </summary>
    public class DOCFA : DbObject<DOCFA>
    {
        #region Variables and constructor
        public DOCFA()
        {
            Reset();
        }

        public DOCFA(int idDOCFA)
        {
            Reset();
            IdDOCFA = idDOCFA;
        }
        #endregion

        #region Public properties
        public int IdDOCFA { get; set; }
        public string CodEnte { get; set; }
        public int FKIdFlusso { get; set; }
        public int IdentificativoImmobile { get; set; }
        public string Protocollo { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public string Zona { get; set; }
        public string Categoria { get; set; }
        public string Classe { get; set; }
        public double Consistenza { get; set; }
        public int Superficie { get; set; }
        public double RenditaEuro { get; set; }
        public string PartitaSpeciale { get; set; }
        public string Lotto { get; set; }
        public string Edificio { get; set; }
        public string Scala { get; set; }
        public string Interno1 { get; set; }
        public string Interno2 { get; set; }
        public string Piano1 { get; set; }
        public string Piano2 { get; set; }
        public string Piano3 { get; set; }
        public string Piano4 { get; set; }
        public string SezioneUrbana { get; set; }
        public int Denominatore { get; set; }
        public string Edificialita { get; set; }
        public string PresenzaGraffati { get; set; }
        public string Toponimo { get; set; }
        public string Indirizzo { get; set; }
        public string Civico1 { get; set; }
        public string Civico2 { get; set; }
        public string Civico3 { get; set; }
        public string PresenzaUlterioriIndirizzi { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is DOCFA) &&
                ((obj as DOCFA).IdDOCFA == IdDOCFA);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdDOCFA);
        }

        public override sealed void Reset()
        {
            IdDOCFA = default(int);
            CodEnte = string.Empty;
            FKIdFlusso = default(int);
            IdentificativoImmobile = default(int);
            Protocollo = string.Empty;
            DataRegistrazione = DateTime.MaxValue;
            Foglio = string.Empty;
            Numero = string.Empty;
            Subalterno = string.Empty;
            Zona = string.Empty;
            Categoria = string.Empty;
            Classe = string.Empty;
            Consistenza = default(double);
            Superficie = default(int);
            RenditaEuro = default(double);
            PartitaSpeciale = string.Empty;
            Lotto = string.Empty;
            Edificio = string.Empty;
            Scala = string.Empty;
            Interno1 = string.Empty;
            Interno2 = string.Empty;
            Piano1 = string.Empty;
            Piano2 = string.Empty;
            Piano3 = string.Empty;
            Piano4 = string.Empty;
            SezioneUrbana = string.Empty;
            Denominatore = default(int);
            Edificialita = string.Empty;
            PresenzaGraffati = string.Empty;
            Toponimo = string.Empty;
            Indirizzo = string.Empty;
            Civico1 = string.Empty;
            Civico2 = string.Empty;
            Civico3 = string.Empty;
            PresenzaUlterioriIndirizzi = string.Empty;
        }

        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_DOCFA_PROTOCOLLI_IU";

                sqlCmd.Parameters.AddWithValue("@IdDOCFA", DbParam.Get(IdDOCFA));
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@FKIdFlusso", DbParam.Get(FKIdFlusso));
                sqlCmd.Parameters.AddWithValue("@IdImmobile", DbParam.Get(IdentificativoImmobile));
                sqlCmd.Parameters.AddWithValue("@Protocollo", DbParam.Get(Protocollo));
                sqlCmd.Parameters.AddWithValue("@DataRegistrazione", DbParam.Get(DataRegistrazione));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@Zona", DbParam.Get(Zona));
                sqlCmd.Parameters.AddWithValue("@Categoria", DbParam.Get(Categoria));
                sqlCmd.Parameters.AddWithValue("@Classe", DbParam.Get(Classe));
                sqlCmd.Parameters.AddWithValue("@Consistenza", DbParam.Get(Consistenza));
                sqlCmd.Parameters.AddWithValue("@Superficie", DbParam.Get(Superficie));
                sqlCmd.Parameters.AddWithValue("@RenditaEuro", DbParam.Get(RenditaEuro));
                sqlCmd.Parameters.AddWithValue("@PartitaSpeciale", DbParam.Get(PartitaSpeciale));
                sqlCmd.Parameters.AddWithValue("@Lotto", DbParam.Get(Lotto));
                sqlCmd.Parameters.AddWithValue("@Edificio", DbParam.Get(Edificio));
                sqlCmd.Parameters.AddWithValue("@Scala", DbParam.Get(Scala));
                sqlCmd.Parameters.AddWithValue("@Interno1", DbParam.Get(Interno1));
                sqlCmd.Parameters.AddWithValue("@Interno2", DbParam.Get(Interno2));
                sqlCmd.Parameters.AddWithValue("@Piano1", DbParam.Get(Piano1));
                sqlCmd.Parameters.AddWithValue("@Piano2", DbParam.Get(Piano2));
                sqlCmd.Parameters.AddWithValue("@Piano3", DbParam.Get(Piano3));
                sqlCmd.Parameters.AddWithValue("@Piano4", DbParam.Get(Piano4));
                sqlCmd.Parameters.AddWithValue("@SezioneUrbana", DbParam.Get(SezioneUrbana));
                sqlCmd.Parameters.AddWithValue("@Denominatore", DbParam.Get(Denominatore));
                sqlCmd.Parameters.AddWithValue("@Edificialita", DbParam.Get(Edificialita));
                sqlCmd.Parameters.AddWithValue("@PresenzaGraffati", DbParam.Get(PresenzaGraffati));
                sqlCmd.Parameters.AddWithValue("@Toponimo", DbParam.Get(Toponimo));
                sqlCmd.Parameters.AddWithValue("@Indirizzo", DbParam.Get(Indirizzo));
                sqlCmd.Parameters.AddWithValue("@Civico1", DbParam.Get(Civico1));
                sqlCmd.Parameters.AddWithValue("@Civico2", DbParam.Get(Civico2));
                sqlCmd.Parameters.AddWithValue("@Civico3", DbParam.Get(Civico3));
                sqlCmd.Parameters.AddWithValue("@PresenzaUlterioriIndirizzi", DbParam.Get(PresenzaUlterioriIndirizzi));

                sqlCmd.Parameters["@IdDOCFA"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdDOCFA = (int)sqlCmd.Parameters["@IdDOCFA"].Value;

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

        public DOCFA[] LoadNonAbbinati(string idEnte, string Protocollo, string DataRegistrazione, string Foglio, string Numero, string Subalterno, string Ubicazione)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetDOCFANonAbbinati";
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                sqlCmd.Parameters.AddWithValue("@Protocollo", DbParam.Get(Protocollo));
                sqlCmd.Parameters.AddWithValue("@DataRegistrazione", DbParam.Get(DataRegistrazione));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@Ubicazione", DbParam.Get(Ubicazione));
                sqlRead = sqlCmd.ExecuteReader();

                List<DOCFA> list = new List<DOCFA>();
                while (sqlRead.Read())
                {
                    DOCFA item = new DOCFA();
                    item.IdDOCFA = DbValue<int>.Get(sqlRead["IdDOCFA"]);
                    item.Protocollo = DbValue<string>.Get(sqlRead["Protocollo"]);
                    item.DataRegistrazione = DbValue<DateTime>.Get(sqlRead["DataRegistrazione"]);
                    item.Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
                    item.Numero = DbValue<string>.Get(sqlRead["Numero"]);
                    item.Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
                    item.Indirizzo = DbValue<string>.Get(sqlRead["Ubicazione"]);
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
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override DOCFA[] LoadAll()
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
    /// Classe per la gestione dei dati esterni da DOCFA delle planimetrie
    /// </summary>
    public class Planimetrie : DbObject<Planimetrie>
    {
        #region Variables and constructor
        public Planimetrie()
        {
            Reset();
        }

        public Planimetrie(int idPlanimetrie)
        {
            Reset();
            IdPlanimetrie = idPlanimetrie;
        }
        #endregion

        #region Public properties
        public int IdPlanimetrie { get; set; }
        public int FKIdDOCFA { get; set; }
        public string IdEnte { get; set; }
        public int FKIdFlusso { get; set; }
        public int IdentificativoImmobile { get; set; }
        public string Protocollo { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public string Planimetria { get; set; }
        public string Formato { get; set; }
        public int Scala { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is Planimetrie) &&
                ((obj as Planimetrie).IdPlanimetrie == IdPlanimetrie);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdPlanimetrie);
        }

        public override sealed void Reset()
        {
            IdPlanimetrie = default(int);
            FKIdDOCFA = default(int);
            IdEnte = string.Empty;
            FKIdFlusso = default(int);
            IdentificativoImmobile = default(int);
            Protocollo = string.Empty;
            DataRegistrazione = DateTime.MaxValue;
            Planimetria = string.Empty;
            Formato = string.Empty;
            Scala = default(int);
        }

        public override bool Save(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_DOCFA_PLANIMETRIE_IU";

                sqlCmd.Parameters.AddWithValue("@IdPlanimetria", DbParam.Get(IdPlanimetrie));
                sqlCmd.Parameters.AddWithValue("@IdImmobile", DbParam.Get(IdentificativoImmobile));
                sqlCmd.Parameters.AddWithValue("@Protocollo", DbParam.Get(Protocollo));
                sqlCmd.Parameters.AddWithValue("@DataRegistrazione", DbParam.Get(DataRegistrazione));
                sqlCmd.Parameters.AddWithValue("@Planimetria", DbParam.Get(Planimetria));
                sqlCmd.Parameters.AddWithValue("@Formato", DbParam.Get(Formato));
                sqlCmd.Parameters.AddWithValue("@Scala", DbParam.Get(Scala));
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(IdEnte));
                sqlCmd.Parameters.AddWithValue("@FKIdFlusso", DbParam.Get(FKIdFlusso));

                sqlCmd.Parameters["@IdPlanimetria"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdPlanimetrie = (int)sqlCmd.Parameters["@IdPlanimetria"].Value;

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

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override Planimetrie[] LoadAll()
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
    /// Classe per la gestione dei dati esterni da DOCFA del documento
    /// </summary>
    public class DOCFADocumento : DbObject<DOCFADocumento>
    {
        #region Variables and constructor
        public DOCFADocumento()
        {
            Reset();
        }

        public DOCFADocumento(int idDOCFA)
        {
            Reset();
            IdDOCFA = idDOCFA;
        }
        #endregion

        #region Public properties
        public int IdDOCFA { get; set; }
        public string DatiDocumento { get; set; }
        public string RifCatastali { get; set; }
        public string Indirizzo { get; set; }
        public string Classamento { get; set; }
        public DOCFAFiles[] Files { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is DOCFADocumento) &&
                ((obj as DOCFADocumento).IdDOCFA == IdDOCFA);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdDOCFA);
        }

        public override sealed void Reset()
        {
            IdDOCFA = default(int);
            DatiDocumento = string.Empty;
            RifCatastali = string.Empty;
            Indirizzo = string.Empty;
            Classamento = string.Empty;
            Files = null;
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetDOCFADocumenti";
                sqlCmd.Parameters.AddWithValue("@IdDOCFA", DbParam.Get(IdDOCFA));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    IdDOCFA = DbValue<int>.Get(sqlRead["IdDOCFA"]);
                    DatiDocumento = DbValue<string>.Get(sqlRead["DatiDocumento"]);
                    RifCatastali = DbValue<string>.Get(sqlRead["RifCatastali"]);
                    Indirizzo = DbValue<string>.Get(sqlRead["Indirizzo"]);
                    Classamento = DbValue<string>.Get(sqlRead["Classamento"]);
                    Files = new DOCFAFiles { IdDOCFA = IdDOCFA }.LoadAll();
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

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override DOCFADocumento[] LoadAll()
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
    /// Classe per la gestione dei dati esterni da DOCFA dei files
    /// </summary>
    public class DOCFAFiles : DbObject<DOCFAFiles>
    {
        #region Public properties
        public int IdDOCFA { get; set; }
        public string NomeFile { get; set; }
        public string Scala { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdDOCFA);
        }

        public override sealed void Reset()
        {
            IdDOCFA = default(int);
            NomeFile = default(string);
            Scala = default(string);
        }

        public override DOCFAFiles[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetDOCFAFiles";
                sqlCmd.Parameters.AddWithValue("@IdDOCFA", DbParam.Get(IdDOCFA));
                sqlRead = sqlCmd.ExecuteReader();

                List<DOCFAFiles> list = new List<DOCFAFiles>();
                while (sqlRead.Read())
                {
                    DOCFAFiles item = new DOCFAFiles();
                    item.IdDOCFA = DbValue<int>.Get(sqlRead["IdDOCFA"]);
                    item.NomeFile = DbValue<string>.Get(sqlRead["NomeFile"]);
                    item.Scala = DbValue<string>.Get(sqlRead["Scala"]);
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

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
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
    /// Classe per la gestione dei dati esterni da DOCFA delle dichiarazioni
    /// </summary>
    public class DOCFADichiarazioni : DbObject<DOCFADichiarazioni>
    {
        private string TypeDB { get; set; }
        private string ConnectionString { get; set; }
        #region Variables and constructor
        public DOCFADichiarazioni()
        {
            Reset();
        }
        public DOCFADichiarazioni(string typeDB, string connectionString)
        {
            Reset();
            TypeDB = typeDB;
            ConnectionString = connectionString;
        }

        public DOCFADichiarazioni(int idDOCFADichiarazioni)
        {
            Reset();
            IdDOCFADichiarazioni = idDOCFADichiarazioni;
        }
        #endregion

        #region Public properties
        public int IdDOCFADichiarazioni { get; set; }
        public int FKIdDOCFA { get; set; }
        public int FKIdDich { get; set; }
        public string Protocollo { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public string Ubicazione { get; set; }
        public string CodTributo { get; set; }
        public string DescrTributo { get; set; }
        public string Nominativo { get; set; }
        public string CFPIVA { get; set; }
        public string DataInizio { get; set; }
        public string DataFine { get; set; }
        public string Classamento { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is DOCFADichiarazioni) &&
                ((obj as DOCFADichiarazioni).IdDOCFADichiarazioni == IdDOCFADichiarazioni);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdDOCFADichiarazioni);
        }

        public override sealed void Reset()
        {
            IdDOCFADichiarazioni = default(int);
            FKIdDOCFA = default(int);
            FKIdDich = default(int);
            Protocollo = string.Empty;
            DataRegistrazione = DateTime.MaxValue;
            Foglio = string.Empty;
            Numero = string.Empty;
            Subalterno = string.Empty;
            Ubicazione = string.Empty;
            CodTributo = string.Empty;
            DescrTributo = string.Empty;
            Nominativo = string.Empty;
            CFPIVA = string.Empty;
            DataInizio = string.Empty;
            DataFine = string.Empty;
            Classamento = string.Empty;
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_DOCFA_RIFERIMENTIDICHIARAZIONI_IU";

                sqlCmd.Parameters.AddWithValue("@IDRIFDICH", DbParam.Get(IdDOCFADichiarazioni));
                sqlCmd.Parameters.AddWithValue("@FKIdDOCFA", DbParam.Get(FKIdDOCFA));
                sqlCmd.Parameters.AddWithValue("@IDRIFCAT", DbParam.Get(FKIdDich));
                sqlCmd.Parameters.AddWithValue("@Tributo", DbParam.Get(CodTributo));

                sqlCmd.Parameters["@IDRIFDICH"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdDOCFADichiarazioni = (int)sqlCmd.Parameters["@IDRIFDICH"].Value;

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

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override DOCFADichiarazioni[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
        public DOCFADichiarazioni[] LoadDichiarazioni(string idEnte, string idDOCFA, string Tributo, string Foglio, string Numero, string Subalterno, string Ubicazione)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetDOCFADichiarazioniDaAbbinare";
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                sqlCmd.Parameters.AddWithValue("@Tributo", DbParam.Get(Tributo));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@Ubicazione", DbParam.Get(Ubicazione));
                sqlRead = sqlCmd.ExecuteReader();

                List<DOCFADichiarazioni> list = new List<DOCFADichiarazioni>();
                while (sqlRead.Read())
                {
                    DOCFADichiarazioni item = new DOCFADichiarazioni();
                    item.FKIdDOCFA = int.Parse(idDOCFA);
                    item.FKIdDich = DbValue<int>.Get(sqlRead["FKIdDich"]);
                    item.Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
                    item.Numero = DbValue<string>.Get(sqlRead["Numero"]);
                    item.Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
                    item.Ubicazione = DbValue<string>.Get(sqlRead["Ubicazione"]);
                    item.DescrTributo = DbValue<string>.Get(sqlRead["DescrTributo"]);
                    item.CodTributo = DbValue<string>.Get(sqlRead["CodTributo"]);
                    item.Nominativo = DbValue<string>.Get(sqlRead["Nominativo"]);
                    item.CFPIVA = DbValue<string>.Get(sqlRead["CFPIVA"]);
                    item.DataInizio = DbValue<string>.Get(sqlRead["DataInizio"]);
                    item.DataFine = DbValue<string>.Get(sqlRead["DataFine"]);
                    item.Classamento = DbValue<string>.Get(sqlRead["Classamento"]);
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
    }
}
