using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;
using Anagrafica.DLL;
using log4net;
using OPENgovTOCO;
using System.Data.SqlClient;
namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione Coefficienti.
    /// </summary>
    public class CoefficientiDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CoefficientiDAO));

        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		public CoefficientiDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int DeleteCoefficiente(int IdTabella, int Anno, string Tabella)
		{
            //*** 20140410
            connessioneDB();

			Int32 errore = 0;
			try
			{
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdTabella", IdTabella);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                //*** 20140415 - modificato il valore da ritornare da -1 a 1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("CoefficientiDAO::DeleteCoefficiente::esecuzione sp_DeleteCoefficiente");
                cmdMyCommand.CommandText = "sp_DeleteCoefficiente" + Tabella;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.DeleteCoefficiente.errore: ", Err);

                errore=-1;
            }
            finally
			{
                cmdMyCommand.Connection.Close();
			}
			
			return errore;

            /*
			DBEngine dbEngine_;		
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			Int32 errore = 0;
			try
			{
				IDataParameterCollection parameterCollection_;
				IDataParameter parameter_;

				string commandText_ = "sp_DeleteCoefficiente" + Tabella;
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdTabella", IdTabella, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (Int32)parameter_.Value;
            */
            /*
            string commandText_ = "UPDATE [dbo].[CATEGORIE] SET [DESCRIZIONE] = '" + Descrizione + "' WHERE IDCATEGORIA = " + IdCategoria + " AND IDENTE = '" + IdEnte + "'";
            dbEngine_.ExecuteNonQuery(commandText_, CommandType.Text);				
            errore = 0;
            */
            /*
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.DeleteCoefficiente.errore: ", Err);

                errore=-1;
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return errore;
			*/
        }

        public int InsertOrUpdateCoefficiente(int IdTabella, int Anno, decimal Coefficiente, string Tabella)
		{
            //*** 20140410
            connessioneDB();

            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdTabella", IdTabella);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@Coefficiente", Coefficiente);
                //*** 20140415 - modificato il valore da ritornare da -1 a 1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("CoefficientiDAO::InsertOrUpdateCoefficiente::esecuzione sp_InsertUpdateCoefficiente");
                cmdMyCommand.CommandText = "sp_InsertUpdateCoefficiente" + Tabella;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.InsertOrUpdateCoefficiente.errore: ", Err);

                errore = -1;
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return errore;

            /*
			DBEngine dbEngine_;		
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			int errore = 0;
			try
			{
				IDataParameterCollection parameterCollection_;
				IDataParameter parameter_;

				string commandText_ = "sp_InsertUpdateCoefficiente" + Tabella;
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdTabella", IdTabella, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@Coefficiente", Coefficiente, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;
            */
            /*
            string commandText_ = "UPDATE [dbo].[CATEGORIE] SET [DESCRIZIONE] = '" + Descrizione + "' WHERE IDCATEGORIA = " + IdCategoria + " AND IDENTE = '" + IdEnte + "'";
            dbEngine_.ExecuteNonQuery(commandText_, CommandType.Text);				
            errore = 0;
            */
            /*
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.InsertOrUpdateCoefficiente.errore: ", Err);

                errore=-1;
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return errore;
			*/
        }

        public DataTable GetAllCoefficienti(string IdEnte, string TipoTabella)
		{
            //*** 20140410
            connessioneDB();

            try
            {
                string commandText_ = "sp_GetAllCoefficienti" + TipoTabella;
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                Log.Debug("CoefficientiDAO::GetAllCoefficienti::esecuzione sp_GetAllCoefficienti");
                cmdMyCommand.CommandText = "sp_GetAllCoefficienti" + TipoTabella;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.GetAllCoefficienti.errore: ", Err);

                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            return dtMyDati;

            /*
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetAllCoefficienti" + TipoTabella;
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			   catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.GetAllCoefficienti.errore: ", Err);

                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			return dt;
            */
        }

        public DataTable GetCoefficiente(int IdTabella, int Anno, string TipoTabella)
		{
            //*** 20140410
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdTabella", IdTabella);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                Log.Debug("CoefficientiDAO::GetCoefficiente::esecuzione sp_GetCoefficiente");
                cmdMyCommand.CommandText = "sp_GetCoefficiente" + TipoTabella;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.GetCoefficienti.errore: ", Err);

                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            return dtMyDati;

            /*
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetCoefficiente" + TipoTabella;
				dbEngine_.AddParameter("@IdTabella", IdTabella, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			   catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.GetCoefficienti.errore: ", Err);

                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			return dt;
            */
        }

        public void connessioneDB()
        {
            //*** 20140409
            try {
                Log.Debug("CoefficientiDAO::connessioneDB::apertura della connessione al DB");
                cmdMyCommand = new SqlCommand();
                myDataReader = null;
                dtMyDati = new DataTable();

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CoefficientiDAO.connessioneDB.errore: ", Err);

                throw (Err);
            }
            
        }
	}
}
