using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto dei diritti sugli immobili
    /// </summary>
    public class TypesDiritto : DbObject<TypesDiritto>
    {
        #region Variables and constructor

        public TypesDiritto()
        {
            Reset();
        }

        public TypesDiritto(string codiceDiritto)
        {
            Reset();
            CodiceDiritto = codiceDiritto;
        }
        #endregion

        #region Public properties
        public string CodiceDiritto { get; set; }
        public string Descrizione { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is TypesDiritto) &&
                (String.Compare((obj as TypesDiritto).CodiceDiritto, CodiceDiritto, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return base.GenerateHashCode((CodiceDiritto == null) ? null : CodiceDiritto.ToUpper());
        }

        public override sealed void Reset()
        {
            CodiceDiritto = default(string);
            Descrizione = default(string);
        }

        public override bool Save()
        {
            return false;
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
                sqlCmd.CommandText = "TYPES_DIRITTO_S";
                sqlCmd.Parameters.AddWithValue("@CodiceDiritto", DbParam.Get(CodiceDiritto));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
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

        public override bool Delete()
        {
            return false;
        }

        public override TypesDiritto[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_DIRITTO_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<TypesDiritto> list = new List<TypesDiritto>();
                while (sqlRead.Read())
                {
                    TypesDiritto item = new TypesDiritto();
                    item.CodiceDiritto = DbValue<string>.Get(sqlRead["CodiceDiritto"]);
                    item.Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
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
