using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto dei soggetti degli immobili
    /// </summary>
    public class AnagrafeUiSoggetti : DbObject<AnagrafeUiSoggetti>
    {
        #region Variables and constructor

        public AnagrafeUiSoggetti()
        {
            Reset();
        }

        public AnagrafeUiSoggetti(int idUISoggetto)
        {
            Reset();
            IdUISoggetto = idUISoggetto;
        }
        #endregion

        #region Public properties
        public int IdUISoggetto { get; set; }
        public int IdentificativoSoggetto { get; set; }
        public int FKIdTypeSoggetto { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Luogo { get; set; }
        public DateTime DataNascita { get; set; }
        public string CF { get; set; }
        public string PI { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagrafeUiSoggetti) &&
                ((obj as AnagrafeUiSoggetti).IdUISoggetto == IdUISoggetto);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUISoggetto);
        }

        public override sealed void Reset()
        {
            IdUISoggetto = default(int);
            IdentificativoSoggetto = default(int);
            FKIdTypeSoggetto = default(int);
            Nome = string.Empty;
            Cognome = string.Empty;
            Luogo = string.Empty;
            DataNascita = Global.MinDateTime();
            CF = string.Empty;
            PI = string.Empty;
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
                sqlCmd.CommandText = "ANAGRAFE_UI_SOGGETTI_S";
                if (IdUISoggetto != default(int))
                    sqlCmd.Parameters.AddWithValue("@IdUISoggetto", DbParam.Get(IdUISoggetto));
                if (IdentificativoSoggetto != default(int))
                    sqlCmd.Parameters.AddWithValue("@Identificativo_Soggetto", DbParam.Get(IdentificativoSoggetto));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    IdUISoggetto = DbValue<int>.Get(sqlRead["IdUISoggetto"]);
                    IdentificativoSoggetto = DbValue<int>.Get(sqlRead["Identificativo_Soggetto"]);
                    FKIdTypeSoggetto = DbValue<int>.Get(sqlRead["fk_IdTypeSoggetto"]);
                    Nome = DbValue<string>.Get(sqlRead["Nome"]);
                    Cognome = DbValue<string>.Get(sqlRead["Cognome"]);
                    Luogo = DbValue<string>.Get(sqlRead["Luogo"]);
                    DataNascita = DbValue<DateTime>.Get(sqlRead["DataNascita"]);
                    CF = DbValue<string>.Get(sqlRead["CF"]);
                    PI = DbValue<string>.Get(sqlRead["PI"]);
                }
                else
                {
                    IdUISoggetto = 0;
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

        public override AnagrafeUiSoggetti[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_SOGGETTI_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUiSoggetti> list = new List<AnagrafeUiSoggetti>();
                while (sqlRead.Read())
                {
                    AnagrafeUiSoggetti item = new AnagrafeUiSoggetti
                        {
                            IdUISoggetto = DbValue<int>.Get(sqlRead["IdUISoggetto"]),
                            IdentificativoSoggetto = DbValue<int>.Get(sqlRead["Identificativo_Soggetto"]),
                            FKIdTypeSoggetto = DbValue<int>.Get(sqlRead["fk_IdTypeSoggetto"]),
                            Nome = DbValue<string>.Get(sqlRead["Nome"]),
                            Cognome = DbValue<string>.Get(sqlRead["Cognome"]),
                            Luogo = DbValue<string>.Get(sqlRead["Luogo"]),
                            DataNascita = DbValue<DateTime>.Get(sqlRead["DataNascita"]),
                            CF = DbValue<string>.Get(sqlRead["CF"]),
                            PI = DbValue<string>.Get(sqlRead["PI"])
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
                sqlCmd.CommandText = "ANAGRAFE_UI_SOGGETTI_IU";

                sqlCmd.Parameters.AddWithValue("@IdUISoggetto", DbParam.Get(IdUISoggetto));
                sqlCmd.Parameters.AddWithValue("@Identificativo_Soggetto", DbParam.Get(IdentificativoSoggetto));
                sqlCmd.Parameters.AddWithValue("@fk_IdTypeSoggetto", DbParam.Get(FKIdTypeSoggetto));
                sqlCmd.Parameters.AddWithValue("@Nome", DbParam.Get(Nome));
                sqlCmd.Parameters.AddWithValue("@Cognome", DbParam.Get(Cognome));
                sqlCmd.Parameters.AddWithValue("@Luogo", DbParam.Get(Luogo));
                sqlCmd.Parameters.AddWithValue("@DataNascita", DbParam.Get(DataNascita));
                sqlCmd.Parameters.AddWithValue("@CF", DbParam.Get(CF));
                sqlCmd.Parameters.AddWithValue("@PI", DbParam.Get(PI));

                sqlCmd.Parameters["@IdUISoggetto"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdUISoggetto = (int)sqlCmd.Parameters["@IdUISoggetto"].Value;
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
                sqlCmd.CommandText = "ANAGRAFE_UI_SOGGETTI_D";
                sqlCmd.Parameters.AddWithValue("@IdUISoggetto", DbParam.Get(IdUISoggetto));
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

        #region Public Methods

        public bool Exists()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_SOGGETTI_S";
                if (IdentificativoSoggetto != default(int))
                    sqlCmd.Parameters.AddWithValue("@Identificativo_Soggetto", DbParam.Get(IdentificativoSoggetto));
                sqlRead = sqlCmd.ExecuteReader();

                return (sqlRead.Read());
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

        public int Assign(int idUIMovimenti)
        {
            SqlCommand sqlCmd = null;
            int idMovimentiSoggetti = -1;
            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_MOVIMENTI_SOGGETTI_IU";

                sqlCmd.Parameters.AddWithValue("@IdMovimentiSoggetti", DbParam.Get(idMovimentiSoggetti));
                sqlCmd.Parameters.AddWithValue("@fK_IdUIMovimenti", DbParam.Get(idUIMovimenti));
                sqlCmd.Parameters.AddWithValue("@fk_IdUISoggetto", DbParam.Get(IdUISoggetto));

                sqlCmd.Parameters["@IdMovimentiSoggetti"].Direction = ParameterDirection.Output;
                sqlCmd.ExecuteNonQuery();
                idMovimentiSoggetti = (int)sqlCmd.Parameters["@IdMovimentiSoggetti"].Value;
                return idMovimentiSoggetti;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return idMovimentiSoggetti;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
        #endregion
    }
}
