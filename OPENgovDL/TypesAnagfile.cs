using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei flussi dei dati esterni
    /// </summary>
    public class TypesAnagfile : DbObject<TypesAnagfile>
    {
        #region Variables and constructor

        public TypesAnagfile()
        {
            Reset();
        }

        public TypesAnagfile(int idAnagFileType)
        {
            Reset();
            IdAnagFileType = idAnagFileType;
        }
        #endregion

        #region Public properties
        public int IdAnagFileType { get; set; }
        public string AnagFileName { get; set; }
        public string Description { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is TypesAnagfile) &&
                ((obj as TypesAnagfile).IdAnagFileType == IdAnagFileType);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdAnagFileType);
        }

        public override void Reset()
        {
            IdAnagFileType = default(int);
            AnagFileName = default(string);
            Description = default(string);
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
                sqlCmd.CommandText = "TYPES_ANAGFILE_S";
                sqlCmd.Parameters.AddWithValue("@IdAnagFileType", DbParam.Get(IdAnagFileType));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    AnagFileName = DbValue<string>.Get(sqlRead["AnagFileName"]);
                    Description = DbValue<string>.Get(sqlRead["Description"]);
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

        public override TypesAnagfile[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_ANAGFILE_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<TypesAnagfile> list = new List<TypesAnagfile>();
                while (sqlRead.Read())
                {
                    TypesAnagfile item = new TypesAnagfile
                        {
                            IdAnagFileType = DbValue<int>.Get(sqlRead["IdAnagFileType"]),
                            AnagFileName = DbValue<string>.Get(sqlRead["AnagFileName"]),
                            Description = DbValue<string>.Get(sqlRead["Description"])
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

        public override bool Save()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_ANAGFILE_IU";

                sqlCmd.Parameters.AddWithValue("@IdAnagFileType", DbParam.Get(IdAnagFileType));
                sqlCmd.Parameters.AddWithValue("@AnagFileName", DbParam.Get(AnagFileName));
                sqlCmd.Parameters.AddWithValue("@Description", DbParam.Get(Description));

                sqlCmd.Parameters["@IdAnagFileType"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdAnagFileType = (int)sqlCmd.Parameters["@IdAnagFileType"].Value;
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
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_ANAGFILE_D";
                sqlCmd.Parameters.AddWithValue("@IdAnagFileType", DbParam.Get(IdAnagFileType));
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
