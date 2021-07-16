using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione della transcodifica delle categorie TARI
    /// </summary>
    public class Categorie : DbObject<Categorie>
    {
        #region Variables and constructor

        public Categorie()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public string CodEnte { get; set; }
        public string CategoriaLegacy { get; set; }
        public int CategoriaAteco { get; set; }
        public int FKIdCategoriaAteco { get; set; }
        public string DescrizioneCategoriaLegacy { get; set; }
        public string DescrizioneCategoriaAteco { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is Categorie) &&
                (String.Compare((obj as Categorie).CodEnte, CodEnte, StringComparison.OrdinalIgnoreCase) == 0) &&
                (String.Compare((obj as Categorie).CategoriaLegacy, CategoriaLegacy, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(
                (CodEnte == null) ? null : CodEnte.ToUpper(),
                (CategoriaLegacy == null) ? null : CategoriaLegacy.ToUpper());
        }

        public override sealed void Reset()
        {
            CodEnte = default(string);
            CategoriaLegacy = default(string);
            FKIdCategoriaAteco = default(int);
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_TRANSCOD_S";
                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@CATEGORIA_LEGACY", DbParam.Get(CategoriaLegacy));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    FKIdCategoriaAteco = DbValue<int>.Get(sqlRead["IdCategoriaAteco"]);
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

        public override Categorie[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_TRANSCOD_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<Categorie> list = new List<Categorie>();
                while (sqlRead.Read())
                {
                    Categorie item = new Categorie();
                    item.CodEnte = DbValue<string>.Get(sqlRead["COD_ENTE"]);
                    item.CategoriaLegacy = DbValue<string>.Get(sqlRead["CATEGORIA_LEGACY"]);
                    item.FKIdCategoriaAteco = DbValue<int>.Get(sqlRead["fk_IdCategoriaAteco"]);
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
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_TRANSCOD_IU";

                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@CATEGORIA_LEGACY", DbParam.Get(CategoriaLegacy));
                sqlCmd.Parameters.AddWithValue("@fk_IdCategoriaAteco", DbParam.Get(FKIdCategoriaAteco));

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

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_TRANSCOD_D";
                sqlCmd.Parameters.AddWithValue("@COD_ENTE", DbParam.Get(CodEnte));
                sqlCmd.Parameters.AddWithValue("@CATEGORIA_LEGACY", DbParam.Get(CategoriaLegacy));
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

        public Categorie[] LoadLastYear()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_CATEGORIE_VALIDE_ANNO";
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(CodEnte));
                sqlRead = sqlCmd.ExecuteReader();

                List<Categorie> list = new List<Categorie>();
                while (sqlRead.Read())
                {
                    Categorie item = new Categorie();
                    item.CodEnte = CodEnte;
                    item.CategoriaLegacy = DbValue<string>.Get(sqlRead["CODICE"]);
                    item.DescrizioneCategoriaLegacy = DbValue<string>.Get(sqlRead["DESCRIZIONE"]);
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
