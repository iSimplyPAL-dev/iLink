using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto dei toponimi
    /// </summary>
    public class Toponimi : DbObject<Toponimi>
    {
        #region Variables and constructor

        public Toponimi()
        {
            Reset();
        }

        public Toponimi(int idToponimo)
        {
            Reset();
            IdToponimo = idToponimo;
        }
        #endregion

        #region Public properties
        public int IdToponimo { get; set; }
        public string Toponimo { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is Toponimi) &&
                ((obj as Toponimi).IdToponimo == this.IdToponimo);
        }

        public override int GetHashCode()
        {
            return base.GenerateHashCode(IdToponimo);
        }

        public override void Reset()
        {
            IdToponimo = default(int);
            Toponimo = default(string);
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
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
                sqlCmd.CommandText = "TOPONIMI_S";
                sqlCmd.Parameters.AddWithValue("@IDTOPONIMO", DbParam.Get(IdToponimo));
                sqlCmd.Parameters.AddWithValue("@TOPONIMO", DbParam.Get(Toponimo));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    Toponimo = DbValue<string>.Get(sqlRead["TOPONIMO"]);
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

        public override bool Delete()
        {
            throw new NotImplementedException();
        }

        public override Toponimi[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TOPONIMI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<Toponimi> list = new List<Toponimi>();
                while (sqlRead.Read())
                {
                    Toponimi item = new Toponimi();
                    item.IdToponimo = DbValue<int>.Get(sqlRead["IDTOPONIMO"]);
                    item.Toponimo = DbValue<string>.Get(sqlRead["TOPONIMO"]);
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
        #endregion
    }
}
