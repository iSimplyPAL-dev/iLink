using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto degli indirizzi immobile
    /// </summary>
    public class AnagrafeUiIndirizzi : DbObject<AnagrafeUiIndirizzi>
    {
        #region Variables and constructor

        public AnagrafeUiIndirizzi()
        {
            Reset();
        }

        public AnagrafeUiIndirizzi(int idUIIndirizzi)
        {
            Reset();
            IdUIIndirizzi = idUIIndirizzi;
        }
        #endregion

        #region Public properties
        public int IdUIIndirizzi { get; set; }
        public int FkidUIMovimenti { get; set; }
        public int Toponimo { get; set; }
        public string Indirizzo { get; set; }
        public string Civico1 { get; set; }
        public string Civico2 { get; set; }
        public string Civico3 { get; set; }
        public int CodiceStrada { get; set; }
        public int Km { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiIndirizzi) &&
                ((obj as AnagrafeUiIndirizzi).IdUIIndirizzi == this.IdUIIndirizzi);
        }

        public override int GetHashCode()
        {
            return base.GenerateHashCode(IdUIIndirizzi);
        }

        public override void Reset()
        {
            IdUIIndirizzi = default(int);
            FkidUIMovimenti = default(int);
            Toponimo = default(int);
            Indirizzo = default(string);
            Civico1 = default(string);
            Civico2 = default(string);
            Civico3 = default(string);
            CodiceStrada = default(int);
            Km = default(int);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_INDIRIZZI_S";
                sqlCmd.Parameters.AddWithValue("@IdUIIndirizzi", DbParam.Get(IdUIIndirizzi));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FkidUIMovimenti = DbValue<int>.Get(sqlRead["fk_IdUIMovimenti"]);
                    Toponimo = DbValue<int>.Get(sqlRead["Toponimo"]);
                    Indirizzo = DbValue<string>.Get(sqlRead["Indirizzo"]);
                    Civico1 = DbValue<string>.Get(sqlRead["Civico1"]);
                    Civico2 = DbValue<string>.Get(sqlRead["Civico2"]);
                    Civico3 = DbValue<string>.Get(sqlRead["Civico3"]);
                    CodiceStrada = DbValue<int>.Get(sqlRead["Codice_Strada"]);
                    Km = DbValue<int>.Get(sqlRead["Km"]);
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
                base.Disconnect(sqlCmd, sqlRead);
            }
        }

        public override AnagrafeUiIndirizzi[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_INDIRIZZI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiIndirizzi> list = new List<AnagrafeUiIndirizzi>();
                while (sqlRead.Read())
                {
                    AnagrafeUiIndirizzi item = new AnagrafeUiIndirizzi
                        {
                            IdUIIndirizzi = DbValue<int>.Get(sqlRead["IdUIIndirizzi"]),
                            FkidUIMovimenti = DbValue<int>.Get(sqlRead["fk_IdUIMovimenti"]),
                            Toponimo = DbValue<int>.Get(sqlRead["Toponimo"]),
                            Indirizzo = DbValue<string>.Get(sqlRead["Indirizzo"]),
                            Civico1 = DbValue<string>.Get(sqlRead["Civico1"]),
                            Civico2 = DbValue<string>.Get(sqlRead["Civico2"]),
                            Civico3 = DbValue<string>.Get(sqlRead["Civico3"]),
                            CodiceStrada = DbValue<int>.Get(sqlRead["Codice_Strada"]),
                            Km = DbValue<int>.Get(sqlRead["Km"])
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
                sqlCmd.CommandText = "ANAGRAFE_UI_INDIRIZZI_IU";

                sqlCmd.Parameters.AddWithValue("@IdUIIndirizzi", DbParam.Get(IdUIIndirizzi));
                sqlCmd.Parameters.AddWithValue("@fk_IdUIMovimenti", DbParam.Get(FkidUIMovimenti));
                sqlCmd.Parameters.AddWithValue("@Toponimo", DbParam.Get(Toponimo));
                sqlCmd.Parameters.AddWithValue("@Indirizzo", DbParam.Get(Indirizzo));
                sqlCmd.Parameters.AddWithValue("@Civico1", DbParam.Get(Civico1));
                sqlCmd.Parameters.AddWithValue("@Civico2", DbParam.Get(Civico2));
                sqlCmd.Parameters.AddWithValue("@Civico3", DbParam.Get(Civico3));
                sqlCmd.Parameters.AddWithValue("@Codice_Strada", DbParam.Get(CodiceStrada));
                sqlCmd.Parameters.AddWithValue("@Km", DbParam.Get(Km));

                sqlCmd.Parameters["@IdUIIndirizzi"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUIIndirizzi = (int)sqlCmd.Parameters["@IdUIIndirizzi"].Value;
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
                sqlCmd.CommandText = "ANAGRAFE_UI_INDIRIZZI_D";
                sqlCmd.Parameters.AddWithValue("@IdUIIndirizzi", DbParam.Get(IdUIIndirizzi));
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
