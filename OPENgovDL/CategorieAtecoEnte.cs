using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione delle categorie TARI dell'ente
    /// </summary>
    public class CategorieAtecoEnte : DbObject<CategorieAtecoEnte>
    {
        #region Variables and constructor
        /// <summary>
        /// 
        /// </summary>
        public CategorieAtecoEnte()
        {
            Reset();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCategoriaAteco"></param>
        public CategorieAtecoEnte(int idCategoriaAteco)
        {
            Reset();
            IdCategoriaAteco = idCategoriaAteco;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// 
        /// </summary>
        public string FromVariabile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IdCategoriaAteco { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CodiceCategoria { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ente { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FKIdTypeAteco { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Definizione { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Descrizione { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Domestica { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeCalcolo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TypeTassazione { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IdRid { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is CategorieAtecoEnte) &&
                ((obj as CategorieAtecoEnte).IdCategoriaAteco == IdCategoriaAteco);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdCategoriaAteco);
        }

        public override sealed void Reset()
        {
            FromVariabile = default(string);
            IdCategoriaAteco = default(int);
            CodiceCategoria = default(string);
            Ente = default(string);
            FKIdTypeAteco = default(int);
            Definizione = default(string);
            Descrizione = default(string);
            Domestica = default(bool);
            TypeCalcolo = ParametriCalcoloEnte.Calcolo_PFPV;
            TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo;
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
                sqlCmd.CommandText = "CATEGORIE_ATECO_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TypeTassazione", DbParam.Get(TypeTassazione));
                if (IdCategoriaAteco > 0)
                    sqlCmd.Parameters.AddWithValue("@IdCategoriaAteco", DbParam.Get(IdCategoriaAteco));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@CodiceCategoria", DbParam.Get(CodiceCategoria));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    CodiceCategoria = DbValue<string>.Get(sqlRead["CodiceCategoria"]);
                    Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    FKIdTypeAteco = DbValue<int>.Get(sqlRead["fk_IdTypeAteco"]);
                    Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
                    Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    Domestica = DbValue<bool>.Get(sqlRead["Domestica"]);
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
        /// <revisionHistory><revision date = "20210113"></revision></revisionHistory>
        public override CategorieAtecoEnte[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_ATECO_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TypeTassazione", DbParam.Get(TypeTassazione));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@TypeCalcolo", DbParam.Get(TypeCalcolo));
                sqlCmd.Parameters.AddWithValue("@CodiceCategoria", DbParam.Get(CodiceCategoria));
                sqlCmd.Parameters.AddWithValue("@IDRID", DbParam.Get(IdRid));
                sqlRead = sqlCmd.ExecuteReader();

                List<CategorieAtecoEnte> list = new List<CategorieAtecoEnte>();
                while (sqlRead.Read())
                {
                    CategorieAtecoEnte item = new CategorieAtecoEnte();
                    item.IdCategoriaAteco = DbValue<int>.Get(sqlRead["IdCategoriaAteco"]);
                    item.CodiceCategoria = DbValue<string>.Get(sqlRead["CodiceCategoria"]);
                    item.Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    item.FKIdTypeAteco = DbValue<int>.Get(sqlRead["fk_IdTypeAteco"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
                    item.Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    item.Domestica = DbValue<bool>.Get(sqlRead["Domestica"]);
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

        public CategorieAtecoEnte[] LoadType()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_CATEGORIE_S";
                if (!string.IsNullOrEmpty(Ente))
                    sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlRead = sqlCmd.ExecuteReader();

                List<CategorieAtecoEnte> list = new List<CategorieAtecoEnte>();
                while (sqlRead.Read())
                {
                    CategorieAtecoEnte item = new CategorieAtecoEnte();
                    item.IdCategoriaAteco = DbValue<int>.Get(sqlRead["IdCategoriaAteco"]);
                    item.Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
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
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_ATECO_ENTE_IU";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdCategoriaAteco", DbParam.Get(IdCategoriaAteco));
                sqlCmd.Parameters.AddWithValue("@CodiceCategoria", DbParam.Get(CodiceCategoria));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@fk_IdTypeAteco", DbParam.Get(FKIdTypeAteco));
                sqlCmd.Parameters.AddWithValue("@Definizione", DbParam.Get(Definizione));
                sqlCmd.Parameters.AddWithValue("@Domestica", DbParam.Get(Domestica));

                sqlCmd.Parameters["@IdCategoriaAteco"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdCategoriaAteco = (int)sqlCmd.Parameters["@IdCategoriaAteco"].Value;
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
                sqlCmd.CommandText = "CATEGORIE_ATECO_ENTE_D";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdCategoriaAteco", DbParam.Get(IdCategoriaAteco));
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
    /// <summary>
    /// Classe per la gestione delle riduzioni/esenzioni dell'ente
    /// </summary>
    public class RidEseEnte : DbObject<RidEseEnte>
    {
        public const string DELETERidEse = "ELIMINA";
        #region Variables and constructor

        public RidEseEnte()
        {
            Reset();
        }

        public RidEseEnte(string codice)
        {
            Reset();
            Codice = codice;
        }
        #endregion

        #region Public properties
        public string FromVariabile { get; set; }
        public string myType { get; set; }
        public string Codice { get; set; }
        public string Ente { get; set; }
        public string Definizione { get; set; }
        public int hasOptDel { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is RidEseEnte) &&
                ((obj as RidEseEnte).Codice == Codice);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(Codice);
        }

        public override sealed void Reset()
        {
            FromVariabile = default(string);
            myType = "R";
            Codice = default(string);
            Ente = default(string);
            Definizione = default(string);
            hasOptDel = 0;
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
                sqlCmd.CommandText = "RID_ESE_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@myType", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Codice", DbParam.Get(Codice));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    myType = DbValue<string>.Get(sqlRead["myType"]);
                    Codice = DbValue<string>.Get(sqlRead["Codice"]);
                    Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
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

        public override RidEseEnte[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "RID_ESE_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@myType", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@HASOPTDEL", DbParam.Get(hasOptDel));
                if (!string.IsNullOrEmpty(Ente))
                    sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlRead = sqlCmd.ExecuteReader();
                List<RidEseEnte> list = new List<RidEseEnte>();
                while (sqlRead.Read())
                {
                    RidEseEnte item = new RidEseEnte();
                    item.myType = DbValue<string>.Get(sqlRead["myType"]);
                    item.Codice = DbValue<string>.Get(sqlRead["Codice"]);
                    item.Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
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
            int IdRidEse = -1;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "RID_ESE_ENTE_IU";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdRidEse", DbParam.Get(IdRidEse));
                sqlCmd.Parameters.AddWithValue("@Codice", DbParam.Get(Codice));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@myType", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@Definizione", DbParam.Get(Definizione));

                sqlCmd.Parameters["@IdRidEse"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdRidEse = (int)sqlCmd.Parameters["@IdRidEse"].Value;
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
                sqlCmd.CommandText = "RID_ESE_ENTE_D";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@myType", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Codice", DbParam.Get(Codice));
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
        public RidEseEnte[] LoadType()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TYPES_RIDESE_S";
                sqlRead = sqlCmd.ExecuteReader();
                List<RidEseEnte> list = new List<RidEseEnte>();
                while (sqlRead.Read())
                {
                    RidEseEnte item = new RidEseEnte();
                    item.Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    item.Codice = DbValue<string>.Get(sqlRead["Codice"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
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
}
