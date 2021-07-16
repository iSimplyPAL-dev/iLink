using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;
using log4net;
using Utility;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni dell'ente
    /// </summary>
    public class Enti : DbObject<Enti>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Enti));
        private string TypeDB { get; set; }
        private string ConnectionString { get; set; }
        #region Variables and constructor

        /*public Enti()
        {
            Reset();

        }*/
        public Enti(string typeDB, string connectionString)
        {
            Reset();
            TypeDB = typeDB;
            ConnectionString = connectionString;
        }
        public Enti(string codEnte)
        {
            Reset();
            CodEnte = codEnte;
        }
        #endregion

        #region Public properties
        public string CodEnte { get; set; }
        public string DescrizioneEnte { get; set; }
        public string Denominazione { get; set; }
        public string IndirizzoCivico { get; set; }
        public string CAP { get; set; }
        public string Localita { get; set; }
        public string ProvinciaSigla { get; set; }
        public string ProvinciaEstesa { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public int? NumAbitanti { get; set; }
        public int? NumNucleiFam { get; set; }
        public string CodCatasto { get; set; }
        public string CodBelfiore { get; set; }
        public string CodIstat { get; set; }
        public string CodProvincia { get; set; }
        public string NoteEnte { get; set; }
        public int? IDEnte { get; set; }
        public string CodTributo { get; set; }
        public string ContoCorrente { get; set; }
        public string IntestazioneBollettino { get; set; }
        public string CodEnteCnc { get; set; }
        public string IdenteCredben { get; set; }
        public string CfPiva { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Sesso { get; set; }
        public string DataNascita { get; set; }
        public string ComuneNascitasede { get; set; }
        public string PvNascitasede { get; set; }
        public string CodUbicazcat { get; set; }
        public string Comunecat { get; set; }
        public int fk_IdTypeAteco { get; set; }
        public string PosizioneGeografica { get; set; }
        public string Ambiente { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is Enti) &&
                (String.Compare((obj as Enti).CodEnte, CodEnte, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode((CodEnte == null) ? null : CodEnte.ToUpper());
        }

        public override sealed void Reset()
        {
            CodEnte = default(string);
            DescrizioneEnte = default(string);
            Denominazione = default(string);
            IndirizzoCivico = default(string);
            CAP = default(string);
            Localita = default(string);
            ProvinciaSigla = default(string);
            ProvinciaEstesa = default(string);
            Telefono = default(string);
            Fax = default(string);
            EMail = default(string);
            NumAbitanti = default(int?);
            NumNucleiFam = default(int?);
            CodCatasto = default(string);
            CodBelfiore = default(string);
            CodIstat = default(string);
            CodProvincia = default(string);
            NoteEnte = default(string);
            IDEnte = default(int?);
            CodTributo = default(string);
            ContoCorrente = default(string);
            IntestazioneBollettino = default(string);
            CodEnteCnc = default(string);
            IdenteCredben = default(string);
            CfPiva = default(string);
            Cognome = default(string);
            Nome = default(string);
            Sesso = default(string);
            DataNascita = default(string);
            ComuneNascitasede = default(string);
            PvNascitasede = default(string);
            CodUbicazcat = default(string);
            Comunecat = default(string);
            fk_IdTypeAteco = 2;
            PosizioneGeografica = default(string);
            Ambiente = default(string);
        }

        public override bool Load()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            try
            {
                /*Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ENTI_S";
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(CodEnte));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    DescrizioneEnte = DbValue<string>.Get(sqlRead["DESCRIZIONE_ENTE"]);
                    Denominazione = DbValue<string>.Get(sqlRead["DENOMINAZIONE"]);
                    IndirizzoCivico = DbValue<string>.Get(sqlRead["INDIRIZZO_CIVICO"]);
                    CAP = DbValue<string>.Get(sqlRead["CAP"]);
                    Localita = DbValue<string>.Get(sqlRead["LOCALITA"]);
                    ProvinciaSigla = DbValue<string>.Get(sqlRead["PROVINCIA_SIGLA"]);
                    ProvinciaEstesa = DbValue<string>.Get(sqlRead["PROVINCIA_ESTESA"]);
                    Telefono = DbValue<string>.Get(sqlRead["TELEFONO"]);
                    Fax = DbValue<string>.Get(sqlRead["FAX"]);
                    EMail = DbValue<string>.Get(sqlRead["E_MAIL"]);
                    NumAbitanti = DbValue<int?>.Get(sqlRead["NUM_ABITANTI"]);
                    NumNucleiFam = DbValue<int?>.Get(sqlRead["NUM_NUCLEI_FAM"]);
                    CodCatasto = DbValue<string>.Get(sqlRead["COD_CATASTO"]);
                    CodBelfiore = DbValue<string>.Get(sqlRead["COD_BELFIORE"]);
                    CodIstat = DbValue<string>.Get(sqlRead["COD_ISTAT"]);
                    CodProvincia = DbValue<string>.Get(sqlRead["COD_PROVINCIA"]);
                    NoteEnte = DbValue<string>.Get(sqlRead["NOTE_ENTE"]);
                    IDEnte = DbValue<int?>.Get(sqlRead["ID_ENTE"]);
                    CodTributo = DbValue<string>.Get(sqlRead["COD_TRIBUTO"]);
                    ContoCorrente = DbValue<string>.Get(sqlRead["CONTO_CORRENTE"]);
                    IntestazioneBollettino = DbValue<string>.Get(sqlRead["INTESTAZIONE_BOLLETTINO"]);
                    CodEnteCnc = DbValue<string>.Get(sqlRead["COD_ENTE_CNC"]);
                    IdenteCredben = DbValue<string>.Get(sqlRead["IDENTE_CREDBEN"]);
                    CfPiva = DbValue<string>.Get(sqlRead["CF_PIVA"]);
                    Cognome = DbValue<string>.Get(sqlRead["COGNOME"]);
                    Nome = DbValue<string>.Get(sqlRead["NOME"]);
                    Sesso = DbValue<string>.Get(sqlRead["SESSO"]);
                    DataNascita = DbValue<string>.Get(sqlRead["DATA_NASCITA"]);
                    ComuneNascitasede = DbValue<string>.Get(sqlRead["COMUNE_NASCITASEDE"]);
                    PvNascitasede = DbValue<string>.Get(sqlRead["PV_NASCITASEDE"]);
                    CodUbicazcat = DbValue<string>.Get(sqlRead["COD_UBICAZCAT"]);
                    Comunecat = DbValue<string>.Get(sqlRead["COMUNECAT"]);
                    fk_IdTypeAteco = DbValue<int>.Get(sqlRead["fk_IdTypeAteco"]);
                    PosizioneGeografica = DbValue<string>.Get(sqlRead["PosizioneGeografica"]);
                    Ambiente = DbValue<string>.Get(sqlRead["AMBIENTE"]);
                }
                else
                {
                    Reset();
                }*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ENTI_S", "IDENTE", "AMBIENTE", "BELFIORE");
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", CodEnte)
                            , ctx.GetParam("AMBIENTE", string.Empty)
                            , ctx.GetParam("BELFIORE", string.Empty)
                        );
                    if (dvMyView.Count > 0)
                    {
                        foreach (DataRowView myRow in dvMyView)
                        {
                            DescrizioneEnte = DbValue<string>.Get(myRow["DESCRIZIONE_ENTE"]);
                            Denominazione = DbValue<string>.Get(myRow["DENOMINAZIONE"]);
                            IndirizzoCivico = DbValue<string>.Get(myRow["INDIRIZZO_CIVICO"]);
                            CAP = DbValue<string>.Get(myRow["CAP"]);
                            Localita = DbValue<string>.Get(myRow["LOCALITA"]);
                            ProvinciaSigla = DbValue<string>.Get(myRow["PROVINCIA_SIGLA"]);
                            ProvinciaEstesa = DbValue<string>.Get(myRow["PROVINCIA_ESTESA"]);
                            Telefono = DbValue<string>.Get(myRow["TELEFONO"]);
                            Fax = DbValue<string>.Get(myRow["FAX"]);
                            EMail = DbValue<string>.Get(myRow["E_MAIL"]);
                            NumAbitanti = DbValue<int?>.Get(myRow["NUM_ABITANTI"]);
                            NumNucleiFam = DbValue<int?>.Get(myRow["NUM_NUCLEI_FAM"]);
                            CodCatasto = DbValue<string>.Get(myRow["COD_CATASTO"]);
                            CodBelfiore = DbValue<string>.Get(myRow["COD_BELFIORE"]);
                            CodIstat = DbValue<string>.Get(myRow["COD_ISTAT"]);
                            CodProvincia = DbValue<string>.Get(myRow["COD_PROVINCIA"]);
                            NoteEnte = DbValue<string>.Get(myRow["NOTE_ENTE"]);
                            IDEnte = DbValue<int?>.Get(myRow["ID_ENTE"]);
                            CodTributo = DbValue<string>.Get(myRow["COD_TRIBUTO"]);
                            ContoCorrente = DbValue<string>.Get(myRow["CONTO_CORRENTE"]);
                            IntestazioneBollettino = DbValue<string>.Get(myRow["INTESTAZIONE_BOLLETTINO"]);
                            CodEnteCnc = DbValue<string>.Get(myRow["COD_ENTE_CNC"]);
                            IdenteCredben = DbValue<string>.Get(myRow["IDENTE_CREDBEN"]);
                            CfPiva = DbValue<string>.Get(myRow["CF_PIVA"]);
                            Cognome = DbValue<string>.Get(myRow["COGNOME"]);
                            Nome = DbValue<string>.Get(myRow["NOME"]);
                            Sesso = DbValue<string>.Get(myRow["SESSO"]);
                            DataNascita = DbValue<string>.Get(myRow["DATA_NASCITA"]);
                            ComuneNascitasede = DbValue<string>.Get(myRow["COMUNE_NASCITASEDE"]);
                            PvNascitasede = DbValue<string>.Get(myRow["PV_NASCITASEDE"]);
                            CodUbicazcat = DbValue<string>.Get(myRow["COD_UBICAZCAT"]);
                            Comunecat = DbValue<string>.Get(myRow["COMUNECAT"]);
                            fk_IdTypeAteco = DbValue<int>.Get(myRow["fk_IdTypeAteco"]);
                            PosizioneGeografica = DbValue<string>.Get(myRow["PosizioneGeografica"]);
                            Ambiente = DbValue<string>.Get(myRow["AMBIENTE"]);
                        }
                    }
                    else
                        Reset();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in Enti.Load::", ex);
                return false;
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }

        public override Enti[] LoadAll()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            List<Enti> list = new List<Enti>();
                try
            {
                if (CodEnte == null)
                    CodEnte = string.Empty;
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ENTI_S","IDENTE","AMBIENTE","BELFIORE");
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", CodEnte)
                            , ctx.GetParam("AMBIENTE", string.Empty)
                            , ctx.GetParam("BELFIORE", string.Empty)
                        );
                    foreach (DataRowView myRow in dvMyView)
                    {
                        Enti item = new Enti(TypeDB, ConnectionString)
                        {
                            CodEnte = DbValue<string>.Get(myRow["COD_ENTE"]),
                            DescrizioneEnte = DbValue<string>.Get(myRow["DESCRIZIONE_ENTE"]),
                            Denominazione = DbValue<string>.Get(myRow["DENOMINAZIONE"]),
                            IndirizzoCivico = DbValue<string>.Get(myRow["INDIRIZZO_CIVICO"]),
                            CAP = DbValue<string>.Get(myRow["CAP"]),
                            Localita = DbValue<string>.Get(myRow["LOCALITA"]),
                            ProvinciaSigla = DbValue<string>.Get(myRow["PROVINCIA_SIGLA"]),
                            ProvinciaEstesa = DbValue<string>.Get(myRow["PROVINCIA_ESTESA"]),
                            Telefono = DbValue<string>.Get(myRow["TELEFONO"]),
                            Fax = DbValue<string>.Get(myRow["FAX"]),
                            EMail = DbValue<string>.Get(myRow["E_MAIL"]),
                            NumAbitanti = DbValue<int?>.Get(myRow["NUM_ABITANTI"]),
                            NumNucleiFam = DbValue<int?>.Get(myRow["NUM_NUCLEI_FAM"]),
                            CodCatasto = DbValue<string>.Get(myRow["COD_CATASTO"]),
                            CodBelfiore = DbValue<string>.Get(myRow["COD_BELFIORE"]),
                            CodIstat = DbValue<string>.Get(myRow["COD_ISTAT"]),
                            CodProvincia = DbValue<string>.Get(myRow["COD_PROVINCIA"]),
                            NoteEnte = DbValue<string>.Get(myRow["NOTE_ENTE"]),
                            IDEnte = DbValue<int?>.Get(myRow["ID_ENTE"]),
                            CodTributo = DbValue<string>.Get(myRow["COD_TRIBUTO"]),
                            ContoCorrente = DbValue<string>.Get(myRow["CONTO_CORRENTE"]),
                            IntestazioneBollettino = DbValue<string>.Get(myRow["INTESTAZIONE_BOLLETTINO"]),
                            CodEnteCnc = DbValue<string>.Get(myRow["COD_ENTE_CNC"]),
                            IdenteCredben = DbValue<string>.Get(myRow["IDENTE_CREDBEN"]),
                            CfPiva = DbValue<string>.Get(myRow["CF_PIVA"]),
                            Cognome = DbValue<string>.Get(myRow["COGNOME"]),
                            Nome = DbValue<string>.Get(myRow["NOME"]),
                            Sesso = DbValue<string>.Get(myRow["SESSO"]),
                            DataNascita = DbValue<string>.Get(myRow["DATA_NASCITA"]),
                            ComuneNascitasede = DbValue<string>.Get(myRow["COMUNE_NASCITASEDE"]),
                            PvNascitasede = DbValue<string>.Get(myRow["PV_NASCITASEDE"]),
                            CodUbicazcat = DbValue<string>.Get(myRow["COD_UBICAZCAT"]),
                            Comunecat = DbValue<string>.Get(myRow["COMUNECAT"]),
                            fk_IdTypeAteco = DbValue<int>.Get(myRow["fk_IdTypeAteco"]),
                            PosizioneGeografica = DbValue<string>.Get(myRow["PosizioneGeografica"]),
                            Ambiente = DbValue<string>.Get(myRow["AMBIENTE"])
                        };
                        list.Add(item);
                    }
                    ctx.Dispose();
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in Enti.LoadAll::", ex);
                return null;
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }

        public override bool Save()
        {
            throw new NotImplementedException();
            /*SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ENTI_IU";

                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@DESCRIZIONE_ENTE", DbParam.Get(DescrizioneEnte));
                sqlCmd.Parameters.AddWithValue("@DENOMINAZIONE", DbParam.Get(Denominazione));
                sqlCmd.Parameters.AddWithValue("@INDIRIZZO_CIVICO", DbParam.Get(IndirizzoCivico));
                sqlCmd.Parameters.AddWithValue("@CAP", DbParam.Get(CAP));
                sqlCmd.Parameters.AddWithValue("@LOCALITA", DbParam.Get(Localita));
                sqlCmd.Parameters.AddWithValue("@PROVINCIA_SIGLA", DbParam.Get(ProvinciaSigla));
                sqlCmd.Parameters.AddWithValue("@PROVINCIA_ESTESA", DbParam.Get(ProvinciaEstesa));
                sqlCmd.Parameters.AddWithValue("@TELEFONO", DbParam.Get(Telefono));
                sqlCmd.Parameters.AddWithValue("@FAX", DbParam.Get(Fax));
                sqlCmd.Parameters.AddWithValue("@E_MAIL", DbParam.Get(EMail));
                sqlCmd.Parameters.AddWithValue("@NUM_ABITANTI", DbParam.Get(NumAbitanti));
                sqlCmd.Parameters.AddWithValue("@NUM_NUCLEI_FAM", DbParam.Get(NumNucleiFam));
                sqlCmd.Parameters.AddWithValue("@COD_CATASTO", DbParam.Get(CodCatasto));
                sqlCmd.Parameters.AddWithValue("@COD_BELFIORE", DbParam.Get(CodBelfiore));
                sqlCmd.Parameters.AddWithValue("@COD_ISTAT", DbParam.Get(CodIstat));
                sqlCmd.Parameters.AddWithValue("@COD_PROVINCIA", DbParam.Get(CodProvincia));
                sqlCmd.Parameters.AddWithValue("@NOTE_ENTE", DbParam.Get(NoteEnte));
                sqlCmd.Parameters.AddWithValue("@ID_ENTE", DbParam.Get(IDEnte));
                sqlCmd.Parameters.AddWithValue("@COD_TRIBUTO", DbParam.Get(CodTributo));
                sqlCmd.Parameters.AddWithValue("@CONTO_CORRENTE", DbParam.Get(ContoCorrente));
                sqlCmd.Parameters.AddWithValue("@INTESTAZIONE_BOLLETTINO", DbParam.Get(IntestazioneBollettino));
                sqlCmd.Parameters.AddWithValue("@COD_ENTE_CNC", DbParam.Get(CodEnteCnc));
                sqlCmd.Parameters.AddWithValue("@IDENTE_CREDBEN", DbParam.Get(IdenteCredben));
                sqlCmd.Parameters.AddWithValue("@CF_PIVA", DbParam.Get(CfPiva));
                sqlCmd.Parameters.AddWithValue("@COGNOME", DbParam.Get(Cognome));
                sqlCmd.Parameters.AddWithValue("@NOME", DbParam.Get(Nome));
                sqlCmd.Parameters.AddWithValue("@SESSO", DbParam.Get(Sesso));
                sqlCmd.Parameters.AddWithValue("@DATA_NASCITA", DbParam.Get(DataNascita));
                sqlCmd.Parameters.AddWithValue("@COMUNE_NASCITASEDE", DbParam.Get(ComuneNascitasede));
                sqlCmd.Parameters.AddWithValue("@PV_NASCITASEDE", DbParam.Get(PvNascitasede));
                sqlCmd.Parameters.AddWithValue("@COD_UBICAZCAT", DbParam.Get(CodUbicazcat));
                sqlCmd.Parameters.AddWithValue("@COMUNECAT", DbParam.Get(Comunecat));
                sqlCmd.Parameters.AddWithValue("@fk_IdTypeAteco", DbParam.Get(fk_IdTypeAteco));
                sqlCmd.Parameters.AddWithValue("@POSIZIONEGEOGRAFICA", DbParam.Get(PosizioneGeografica));

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
            }*/
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
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ENTI_D";
                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
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

        public string GetBelfioreByEnte(string codEnte)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ENTI_S_BELFIORE";
                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(codEnte));
                sqlRead = sqlCmd.ExecuteReader();

                if (!sqlRead.Read())
                    throw new Exception(string.Format(
                        "Impossibile trovare il Codice Belfiore a partire dall'Ente <{0}>!", codEnte));

                return DbValue<string>.Get(sqlRead["COD_BELFIORE"]);
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
}
