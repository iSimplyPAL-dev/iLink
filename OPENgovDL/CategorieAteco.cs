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
    /// Classe per la gestione delle Categorie TARI nazionali
    /// </summary>
    public class CategorieAteco : DbObject<CategorieAteco>
    {
        #region Variables and constructor

        public CategorieAteco()
        {
            Reset();
        }

        public CategorieAteco(int idCategoriaAteco)
        {
            Reset();
            IdCategoriaAteco = idCategoriaAteco;
        }
        #endregion

        #region Public properties
        public int IdCategoriaAteco { get; set; }
        public string CodiceCategoria { get; set; }
        public string Ente { get; set; }
        public int FKIdTypeAteco { get; set; }
        public string Definizione { get; set; }
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
            IdCategoriaAteco = default(int);
            CodiceCategoria = default(string);
            Ente = default(string);
            FKIdTypeAteco = default(int);
            Definizione = default(string);
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
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }

        public override CategorieAteco[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "CATEGORIE_ATECO_L";
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlRead = sqlCmd.ExecuteReader();

                List<CategorieAteco> list = new List<CategorieAteco>();
                while (sqlRead.Read())
                {
                    CategorieAteco item = new CategorieAteco();
                    item.IdCategoriaAteco = DbValue<int>.Get(sqlRead["IdCategoriaAteco"]);
                    item.CodiceCategoria = DbValue<string>.Get(sqlRead["CodiceCategoria"]);
                    item.Ente = Ente;
                    item.FKIdTypeAteco = DbValue<int>.Get(sqlRead["fk_IdTypeAteco"]);
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
        #endregion
    }
}
