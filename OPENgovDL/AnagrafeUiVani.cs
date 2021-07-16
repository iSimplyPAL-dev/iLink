using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto dei vani degli immobili
    /// </summary>
    public class AnagrafeUiVani : DbObject<AnagrafeUiVani>
    {
        #region Variables and constructor

        public AnagrafeUiVani()
        {
            Reset();
        }

        public AnagrafeUiVani(int idUIVani)
        {
            Reset();
            IdUIVani = idUIVani;
        }
        #endregion

        #region Public properties
        public int IdUIVani { get; set; }
        public int FKIdUIMovimenti { get; set; }
        public string FKTipoVano { get; set; }
        public int Superficie { get; set; }
        public int Altezza { get; set; }
        public int AltezzaMax { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiVani) &&
                ((obj as AnagrafeUiVani).IdUIVani == IdUIVani);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUIVani);
        }

        public override void Reset()
        {
            IdUIVani = default(int);
            FKIdUIMovimenti = default(int);
            FKTipoVano = default(string);
            Superficie = default(int);
            Altezza = default(int);
            AltezzaMax = default(int);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_VANI_S";
                sqlCmd.Parameters.AddWithValue("@IdUIVani", DbParam.Get(IdUIVani));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FKIdUIMovimenti = DbValue<int>.Get(sqlRead["fk_IdUIMovimenti"]);
                    FKTipoVano = DbValue<string>.Get(sqlRead["fk_TipoVano"]);
                    Superficie = DbValue<int>.Get(sqlRead["Superficie"]);
                    Altezza = DbValue<int>.Get(sqlRead["Altezza"]);
                    AltezzaMax = DbValue<int>.Get(sqlRead["AltezzaMax"]);
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

        public override AnagrafeUiVani[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_VANI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiVani> list = new List<AnagrafeUiVani>();
                while (sqlRead.Read())
                {
                    AnagrafeUiVani item = new AnagrafeUiVani
                        {
                            IdUIVani = DbValue<int>.Get(sqlRead["IdUIVani"]),
                            FKIdUIMovimenti = DbValue<int>.Get(sqlRead["fk_IdUIMovimenti"]),
                            FKTipoVano = DbValue<string>.Get(sqlRead["fk_TipoVano"]),
                            Superficie = DbValue<int>.Get(sqlRead["Superficie"]),
                            Altezza = DbValue<int>.Get(sqlRead["Altezza"]),
                            AltezzaMax = DbValue<int>.Get(sqlRead["AltezzaMax"])
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
                sqlCmd.CommandText = "ANAGRAFE_UI_VANI_IU";

                sqlCmd.Parameters.AddWithValue("@IdUIVani", DbParam.Get(IdUIVani));
                sqlCmd.Parameters.AddWithValue("@fk_IdUIMovimenti", DbParam.Get(FKIdUIMovimenti));
                sqlCmd.Parameters.AddWithValue("@fk_TipoVano", DbParam.Get(FKTipoVano));
                sqlCmd.Parameters.AddWithValue("@Superficie", DbParam.Get(Superficie));
                sqlCmd.Parameters.AddWithValue("@Altezza", DbParam.Get(Altezza));
                sqlCmd.Parameters.AddWithValue("@AltezzaMax", DbParam.Get(AltezzaMax));

                sqlCmd.Parameters["@IdUIVani"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUIVani = (int)sqlCmd.Parameters["@IdUIVani"].Value;
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
                sqlCmd.CommandText = "ANAGRAFE_UI_VANI_D";
                sqlCmd.Parameters.AddWithValue("@IdUIVani", DbParam.Get(IdUIVani));
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
