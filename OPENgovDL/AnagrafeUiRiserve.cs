using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto delle riverse dell'immobile
    /// </summary>
    public class AnagrafeUiRiserve : DbObject<AnagrafeUiRiserve>
    {
        #region Variables and constructor

        public AnagrafeUiRiserve()
        {
            Reset();
        }

        public AnagrafeUiRiserve(int idUIRiserve)
        {
            Reset();
            IdUIRiserve = idUIRiserve;
        }
        #endregion

        #region Public properties
        public int IdUIRiserve { get; set; }
        public int FKIdUIMovimenti { get; set; }
        public string CodiceRiserva { get; set; }
        public string PartitaRiserva { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiRiserve) &&
                ((obj as AnagrafeUiRiserve).IdUIRiserve == IdUIRiserve);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUIRiserve);
        }

        public override sealed void Reset()
        {
            IdUIRiserve = default(int);
            FKIdUIMovimenti = default(int);
            CodiceRiserva = default(string);
            PartitaRiserva = default(string);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_RISERVE_S";
                sqlCmd.Parameters.AddWithValue("@IdUIRiserve", DbParam.Get(IdUIRiserve));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FKIdUIMovimenti = DbValue<int>.Get(sqlRead["fK_IdUIMovimenti"]);
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

        public override AnagrafeUiRiserve[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_RISERVE_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiRiserve> list = new List<AnagrafeUiRiserve>();
                while (sqlRead.Read())
                {
                    AnagrafeUiRiserve item = new AnagrafeUiRiserve();
                    item.IdUIRiserve = DbValue<int>.Get(sqlRead["IdUIRiserve"]);
                    item.FKIdUIMovimenti = DbValue<int>.Get(sqlRead["fK_IdUIMovimenti"]);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_RISERVE_IU";

                sqlCmd.Parameters.AddWithValue("@IdUIRiserve", DbParam.Get(IdUIRiserve));
                sqlCmd.Parameters.AddWithValue("@fK_IdUIMovimenti", DbParam.Get(FKIdUIMovimenti));
                sqlCmd.Parameters.AddWithValue("@Codice_Riserva", DbParam.Get(CodiceRiserva));
                sqlCmd.Parameters.AddWithValue("@Partita_Riserva", DbParam.Get(PartitaRiserva));

                sqlCmd.Parameters["@IdUIRiserve"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUIRiserve = (int)sqlCmd.Parameters["@IdUIRiserve"].Value;
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
                sqlCmd.CommandText = "ANAGRAFE_UI_RISERVE_D";
                sqlCmd.Parameters.AddWithValue("@IdUIRiserve", DbParam.Get(IdUIRiserve));
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
