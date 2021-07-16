using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei parametri di calcolo ruolo TARI
    /// </summary>
    public class ParametriCalcoloEnte : DbObject<ParametriCalcoloEnte>
    {
        public const string Calcolo_PF="TARSU";
        public const string Calcolo_PFPV="TARES";

        #region Variables and constructor
        public ParametriCalcoloEnte()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public string Anno { get; set; }
        public string TypeCalcolo { get; set; }
        public Boolean HasMaggiorazione { get; set; }
        public Boolean HasConferimenti { get; set; }
        public string TypeMQ { get; set; }
        public string TypeNCNonRes { get; set; }
        public int NCNonRes { get; set; }
        public string TypeValiditaUpdateRes { get; set; }
        public string DescrTypeMQ { get; set; }
        public string DescrTypeNCNonRes { get; set; }
        public string DescrTypeValiditaUpdateRes { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is ParametriCalcoloEnte) &&
                ((obj as ParametriCalcoloEnte).Ente == Ente) &&
                ((obj as ParametriCalcoloEnte).Anno == Anno);
        }

        public override int GetHashCode()
        {
            object[] fields=new object[2];
            fields[0]=Ente;
            fields[1]=Anno;
            return GenerateHashCode(fields);
        }

        public override sealed void Reset()
        {
            FromVariabile = default(string);
            Ente = default(string);
            Anno = default(string);
            TypeCalcolo = default(string);
            HasMaggiorazione = default(Boolean);
            HasConferimenti = default(Boolean);
            TypeMQ = default(string);
            TypeNCNonRes = default(string);
            NCNonRes = default(int);
            TypeValiditaUpdateRes = default(string);
            DescrTypeMQ = default(string);
            DescrTypeNCNonRes = default(string);
            DescrTypeValiditaUpdateRes = default(string);
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
                sqlCmd.CommandText = "PARAMETRI_CALCOLO_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    TypeCalcolo = DbValue<string>.Get(sqlRead["TipoCalcolo"]);
                    HasMaggiorazione = DbValue<Boolean>.Get(sqlRead["HasMaggiorazione"]);
                    HasConferimenti = DbValue<Boolean>.Get(sqlRead["HasConferimenti"]);
                    TypeMQ = DbValue<string>.Get(sqlRead["TipoMQ"]);
                    TypeNCNonRes = DbValue<string>.Get(sqlRead["TipoNCNonRes"]);
                    NCNonRes = DbValue<int>.Get(sqlRead["NCNonRes"]);
                    TypeValiditaUpdateRes = DbValue<string>.Get(sqlRead["TipoValiditaAggRes"]);
                    DescrTypeMQ = DbValue<string>.Get(sqlRead["DESCRTYPEMQ"]);
                    DescrTypeNCNonRes = DbValue<string>.Get(sqlRead["DESCRNCNONRES"]);
                    DescrTypeValiditaUpdateRes = DbValue<string>.Get(sqlRead["DESCRVALIDITAUPDATERES"]);
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

        public override ParametriCalcoloEnte[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "PARAMETRI_CALCOLO_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                Global.Log.Write2(LogSeverity.Debug, sqlCmd.Connection.ConnectionString + sqlCmd.CommandText + FromVariabile.ToString() + "," + Ente);
                sqlRead = sqlCmd.ExecuteReader();
                List<ParametriCalcoloEnte> list = new List<ParametriCalcoloEnte>();
                while (sqlRead.Read())
                {
                    ParametriCalcoloEnte item = new ParametriCalcoloEnte();
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    item.TypeCalcolo = DbValue<string>.Get(sqlRead["TipoCalcolo"]);
                    item.HasMaggiorazione = DbValue<Boolean>.Get(sqlRead["HasMaggiorazione"]);
                    item.HasConferimenti = DbValue<Boolean>.Get(sqlRead["HasConferimenti"]);
                    item.TypeMQ = DbValue<string>.Get(sqlRead["TipoMQ"]);
                    item.TypeNCNonRes = DbValue<string>.Get(sqlRead["TipoNCNonRes"]);
                    item.NCNonRes = DbValue<int>.Get(sqlRead["NCNonRes"]);
                    item.TypeValiditaUpdateRes = DbValue<string>.Get(sqlRead["TipoValiditaAggRes"]);
                    item.DescrTypeMQ = DbValue<string>.Get(sqlRead["DESCRTYPEMQ"]);
                    item.DescrTypeNCNonRes = DbValue<string>.Get(sqlRead["DESCRNCNONRES"]);
                    item.DescrTypeValiditaUpdateRes = DbValue<string>.Get(sqlRead["DESCRVALIDITAUPDATERES"]);
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
            int myId = 0;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "PARAMETRI_CALCOLO_IU";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@myId", DbParam.Get(myId));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@TipoCalcolo", DbParam.Get(TypeCalcolo));
                sqlCmd.Parameters.AddWithValue("@HasMaggiorazione", DbParam.Get(HasMaggiorazione));
                sqlCmd.Parameters.AddWithValue("@HasConferimenti", DbParam.Get(HasConferimenti));
                sqlCmd.Parameters.AddWithValue("@TipoMQ", DbParam.Get(TypeMQ));
                sqlCmd.Parameters.AddWithValue("@TipoNCNonRes", DbParam.Get(TypeNCNonRes));
                sqlCmd.Parameters.AddWithValue("@NCNonRes", DbParam.Get(NCNonRes));
                sqlCmd.Parameters.AddWithValue("@TipoValiditaAggRes", DbParam.Get(TypeValiditaUpdateRes));

                sqlCmd.Parameters["@myId"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                myId = (int)sqlCmd.Parameters["@myId"].Value;
                if (myId >= 0)
                    return true;
                else
                    return false;
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
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "PARAMETRI_CALCOLO_D";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
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

        public ParametriCalcoloEnte[] LoadTypeCalcolo()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_CALCOLO_S";
                sqlRead = sqlCmd.ExecuteReader();
                List<ParametriCalcoloEnte> list = new List<ParametriCalcoloEnte>();
                while (sqlRead.Read())
                {
                    ParametriCalcoloEnte item = new ParametriCalcoloEnte();
                    item.TypeCalcolo = DbValue<string>.Get(sqlRead["TypeCalcolo"]);
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
    /// <summary>
    /// Classe per la gestione del numero di componenti sulle superfici
    /// </summary>
    public class NCSup : DbObject<NCSup>
    {
        #region Variables and constructor
        public NCSup()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public string Anno { get; set; }
        public double Da { get; set; }
        public double A { get; set; }
        public int NC { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            object[] fields = new object[3];
            fields[0] = Ente;
            fields[1] = Anno;
            fields[2] = Da;
            return GenerateHashCode(fields);
        }

        public override sealed void Reset()
        {
            FromVariabile = default(string);
            Ente = default(string);
            Anno = default(string);
            Da = default(double);
            A = default(double);
            NC = default(int);
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
                sqlCmd.CommandText = "NC_PER_MQ_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@Da", DbParam.Get(Da));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    Da = doubleParser(DbValue<decimal>.Get(sqlRead["Da"]).ToString());
                    A = doubleParser(DbValue<decimal>.Get(sqlRead["A"]).ToString());
                    NC = DbValue<int>.Get(sqlRead["NC"]);
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

        public override NCSup[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "NC_PER_MQ_S";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlRead = sqlCmd.ExecuteReader();
                List<NCSup> list = new List<NCSup>();
                while (sqlRead.Read())
                {
                    NCSup item = new NCSup();
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    item.Da = doubleParser(DbValue<decimal>.Get(sqlRead["Da"]).ToString());
                    item.A = doubleParser(DbValue<decimal>.Get(sqlRead["A"]).ToString());
                    item.NC = DbValue<int>.Get(sqlRead["NC"]);
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
            int IdNCSup = 0;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "NC_PER_MQ_IU";

                sqlCmd.Parameters.AddWithValue("@IdNCSup", DbParam.Get(IdNCSup));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@Da", DbParam.Get(Da));
                sqlCmd.Parameters.AddWithValue("@A", DbParam.Get(A));
                sqlCmd.Parameters.AddWithValue("@NC", DbParam.Get(NC));
                sqlCmd.Parameters["@IdNCSup"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdNCSup = (int)sqlCmd.Parameters["@IdNCSup"].Value;
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
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "NC_PER_MQ_D";
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@Da", DbParam.Get(Da));
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
