using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da anagrafe residenti.
    /// </summary>
    public class AnagrafeResidenti : DbObject<AnagrafeResidenti>
    {
        #region Variables and constructor

        public AnagrafeResidenti()
        {
            Reset();
        }

        public AnagrafeResidenti(int iD)
        {
            Reset();
            ID = iD;
        }
        #endregion

        #region Public properties
            public int ID { get; set; }
            public string CodEnte { get; set; }
            public int CodIndividuale { get; set; }
            public string Cognome { get; set; }
            public string Nome { get; set; }
            public string Sesso { get; set; }
            public DateTime DataNascita { get; set; }
            public DateTime DataMorte { get; set; }
            public string LuogoNascita { get; set; }
            public string CodFiscale { get; set; }
            public int CodVia { get; set; }
            public string Numero { get; set; }
            public string Lettera { get; set; }
            public string Interno { get; set; }
            public int NumeroFamiglia { get; set; }
            public int CodicePosizioneFamiglia { get; set; }
            public bool IsActive { get; set; }
            public int CodiceAzione { get; set; }
            public DateTime DataAzione { get; set; }
            public int Idflusso { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeResidenti) &&
                ((obj as AnagrafeResidenti).ID == ID);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(ID);
        }

        public override void Reset()
        {
            ID = CodVia = CodIndividuale = NumeroFamiglia = CodicePosizioneFamiglia = default(int);
            CodEnte = Cognome = Nome = Sesso = LuogoNascita = CodFiscale =  Numero = Lettera = Interno = default(string);
            DataNascita = DataMorte = DateTime.MinValue;
            IsActive = true;
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_RESIDENTI_NEW_S";
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    CodEnte = DbValue<string>.Get(sqlRead["COD_ENTE"]);
                    CodIndividuale = DbValue<int>.Get(sqlRead["COD_INDIVIDUALE"]);
                    Cognome = DbValue<string>.Get(sqlRead["COGNOME"]);
                    Nome = DbValue<string>.Get(sqlRead["NOME"]);
                    Sesso = DbValue<string>.Get(sqlRead["SESSO"]);
                    DataNascita = DbValue<DateTime>.Get(sqlRead["DATA_NASCITA"]);
                    DataMorte = DbValue<DateTime>.Get(sqlRead["DATA_MORTE"]);
                    LuogoNascita = DbValue<string>.Get(sqlRead["LUOGO_NASCITA"]);
                    CodFiscale = DbValue<string>.Get(sqlRead["COD_FISCALE"]);
                    CodVia = DbValue<int>.Get(sqlRead["COD_VIA"]);
                    Numero = DbValue<string>.Get(sqlRead["NUMERO"]);
                    Lettera = DbValue<string>.Get(sqlRead["LETTERA"]);
                    Interno = DbValue<string>.Get(sqlRead["INTERNO"]);
                    NumeroFamiglia = DbValue<int>.Get(sqlRead["NUMERO_FAMIGLIA"]);
                    CodicePosizioneFamiglia = DbValue<int>.Get(sqlRead["CODICE_POSIZIONE_FAMIGLIA"]);
                    IsActive = DbValue<bool>.Get(sqlRead["IsActive"]);
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

        public override AnagrafeResidenti[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_RESIDENTI_NEW_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeResidenti> list = new List<AnagrafeResidenti>();
                while (sqlRead.Read())
                {
                    AnagrafeResidenti item = new AnagrafeResidenti
                        {
                            ID = DbValue<int>.Get(sqlRead["ID"]),
                            CodEnte = DbValue<string>.Get(sqlRead["COD_ENTE"]),
                            CodIndividuale = DbValue<int>.Get(sqlRead["COD_INDIVIDUALE"]),
                            Cognome = DbValue<string>.Get(sqlRead["COGNOME"]),
                            Nome = DbValue<string>.Get(sqlRead["NOME"]),
                            Sesso = DbValue<string>.Get(sqlRead["SESSO"]),
                            DataNascita = DbValue<DateTime>.Get(sqlRead["DATA_NASCITA"]),
                            DataMorte = DbValue<DateTime>.Get(sqlRead["DATA_MORTE"]),
                            LuogoNascita = DbValue<string>.Get(sqlRead["LUOGO_NASCITA"]),
                            CodFiscale = DbValue<string>.Get(sqlRead["COD_FISCALE"]),
                            CodVia = DbValue<int>.Get(sqlRead["COD_VIA"]),
                            Numero = DbValue<string>.Get(sqlRead["NUMERO"]),
                            Lettera = DbValue<string>.Get(sqlRead["LETTERA"]),
                            Interno = DbValue<string>.Get(sqlRead["INTERNO"]),
                            NumeroFamiglia = DbValue<int>.Get(sqlRead["NUMERO_FAMIGLIA"]),
                            CodicePosizioneFamiglia = DbValue<int>.Get(sqlRead["CODICE_POSIZIONE_FAMIGLIA"]),
                            IsActive = DbValue<bool>.Get(sqlRead["IsActive"])
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
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_RESIDENTI_NEW_IU";

                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(default(int)));
                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@COD_INDIVIDUALE", DbParam.Get(CodIndividuale));
                sqlCmd.Parameters.AddWithValue("@COGNOME", DbParam.Get(Cognome));
                sqlCmd.Parameters.AddWithValue("@NOME", DbParam.Get(Nome));
                sqlCmd.Parameters.AddWithValue("@SESSO", DbParam.Get(Sesso));
                sqlCmd.Parameters.AddWithValue("@DATA_NASCITA", DbParam.Get(DataNascita));
                sqlCmd.Parameters.AddWithValue("@DATA_MORTE", DbParam.Get(DataMorte));
                sqlCmd.Parameters.AddWithValue("@LUOGO_NASCITA", DbParam.Get(LuogoNascita));
                sqlCmd.Parameters.AddWithValue("@COD_FISCALE", DbParam.Get(CodFiscale));
                sqlCmd.Parameters.AddWithValue("@COD_VIA", DbParam.Get(CodVia));
                sqlCmd.Parameters.AddWithValue("@NUMERO", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@LETTERA", DbParam.Get(Lettera));
                sqlCmd.Parameters.AddWithValue("@INTERNO", DbParam.Get(Interno));
                sqlCmd.Parameters.AddWithValue("@NUMERO_FAMIGLIA", DbParam.Get(NumeroFamiglia));
                sqlCmd.Parameters.AddWithValue("@CODICE_POSIZIONE_FAMIGLIA", DbParam.Get(CodicePosizioneFamiglia));
                sqlCmd.Parameters.AddWithValue("@CODICE_AZIONE", DbParam.Get(CodiceAzione));
                sqlCmd.Parameters.AddWithValue("@DATA_AZIONE", DbParam.Get(DataAzione));
                sqlCmd.Parameters.AddWithValue("@IDFLUSSO", DbParam.Get(Idflusso));

                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.Output;
                sqlCmd.ExecuteNonQuery();
                ID = (int)sqlCmd.Parameters["@ID"].Value;
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
        public bool SaveFromTributi(ref string errore)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_ANAGRAFE_RESIDENTI_FROMTRIBUTI";

                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@DATA_AZIONE", DbParam.Get(DataAzione));
                sqlCmd.Parameters.AddWithValue("@IDFLUSSO", DbParam.Get(Idflusso));

                sqlCmd.ExecuteNonQuery();
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
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_RESIDENTI_NEW_D";
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
    }
}
