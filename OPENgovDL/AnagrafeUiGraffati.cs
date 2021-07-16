using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;


namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto degli immobili graffati
    /// </summary>
    public class AnagrafeUiGraffati : DbObject<AnagrafeUiGraffati>
    {
        #region Variables and constructor

        public AnagrafeUiGraffati()
        {
            Reset();
        }

        public AnagrafeUiGraffati(int idUIGraffati)
        {
            Reset();
            IdUIGraffati = idUIGraffati;
        }
        #endregion

        #region Public properties
        public int IdUIGraffati { get; set; }
        public int FKIdUIMovimenti { get; set; }
        public string Sezioneurbana { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public int Denominatore { get; set; }
        public string Subalterno { get; set; }
        public string Edificialita { get; set; }
        public bool Comune { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiGraffati) &&
                ((obj as AnagrafeUiGraffati).IdUIGraffati == this.IdUIGraffati);
        }

        public override int GetHashCode()
        {
            return base.GenerateHashCode(IdUIGraffati);
        }

        public override void Reset()
        {
            IdUIGraffati = default(int);
            FKIdUIMovimenti = default(int);
            Sezioneurbana = default(string);
            Foglio = default(string);
            Numero = default(string);
            Denominatore = default(int);
            Subalterno = default(string);
            Edificialita = default(string);
            Comune = default(bool);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_GRAFFATI_S";
                sqlCmd.Parameters.AddWithValue("@IdUIGraffati", DbParam.Get(IdUIGraffati));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FKIdUIMovimenti = DbValue<int>.Get(sqlRead["fk_IdUIMovimenti"]);
                    Sezioneurbana = DbValue<string>.Get(sqlRead["Sezioneurbana"]);
                    Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
                    Numero = DbValue<string>.Get(sqlRead["Numero"]);
                    Denominatore = DbValue<int>.Get(sqlRead["Denominatore"]);
                    Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
                    Edificialita = DbValue<string>.Get(sqlRead["Edificialita"]);
                    Comune = DbValue<bool>.Get(sqlRead["Comune"]);
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

        public override AnagrafeUiGraffati[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_GRAFFATI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiGraffati> list = new List<AnagrafeUiGraffati>();
                while (sqlRead.Read())
                {
                    AnagrafeUiGraffati item = new AnagrafeUiGraffati();
                    item.IdUIGraffati = DbValue<int>.Get(sqlRead["IdUIGraffati"]);
                    item.FKIdUIMovimenti = DbValue<int>.Get(sqlRead["fk_IdUIMovimenti"]);
                    item.Sezioneurbana = DbValue<string>.Get(sqlRead["Sezioneurbana"]);
                    item.Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
                    item.Numero = DbValue<string>.Get(sqlRead["Numero"]);
                    item.Denominatore = DbValue<int>.Get(sqlRead["Denominatore"]);
                    item.Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
                    item.Edificialita = DbValue<string>.Get(sqlRead["Edificialita"]);
                    item.Comune = DbValue<bool>.Get(sqlRead["Comune"]);
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
                sqlCmd.CommandText = "ANAGRAFE_UI_GRAFFATI_IU";

                sqlCmd.Parameters.AddWithValue("@IdUIGraffati", DbParam.Get(IdUIGraffati));
                sqlCmd.Parameters.AddWithValue("@fk_IdUIMovimenti", DbParam.Get(FKIdUIMovimenti));
                sqlCmd.Parameters.AddWithValue("@Sezioneurbana", DbParam.Get(Sezioneurbana));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@Denominatore", DbParam.Get(Denominatore));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@Edificialita", DbParam.Get(Edificialita));
                sqlCmd.Parameters.AddWithValue("@Comune", DbParam.Get(Comune));

                sqlCmd.Parameters["@IdUIGraffati"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUIGraffati = (int)sqlCmd.Parameters["@IdUIGraffati"].Value;
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
                sqlCmd.CommandText = "ANAGRAFE_UI_GRAFFATI_D";
                sqlCmd.Parameters.AddWithValue("@IdUIGraffati", DbParam.Get(IdUIGraffati));
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
