using Microsoft.VisualBasic;
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
    /// Classe interfacciamento DB per la gestione Categorie.
    /// </summary>
    public class CategorieDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CategorieDAO));

        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		public CategorieDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int DeleteCategoria(int IdCategoria)
		{
            //*** 20140410
            connessioneDB();

            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("CategorieDAO::DeleteCategoria::esecuzione sp_DeleteCategoria");
                cmdMyCommand.CommandText = "sp_DeleteCategoria";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
            

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.DeleteCategoria.errore: ", Err);
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

				string commandText_ = "sp_DeleteCategoria";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;
			}
			catch (Exception Err)
			{
				  Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.DeleteCategoria.errore: ", Err);
				errore = -1;
				throw (Err);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return errore;
			*/
        }

        public int InsertOrUpdateDescrizione(string ConnectionString, string IdEnte, string IdTributo, int IdCategoria, string Descrizione)
		{
            //*** 20140410
            connessioneDB( ConnectionString );

			int errore = 0;
			try
			{
                cmdMyCommand.Parameters.Clear();
				cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.CommandText = "sp_InsertUpdateDescrizioneCategoria";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
			}
			catch (Exception Err)
			{
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.InsertOrUpdateDescrizione.errore: ", Err);
                errore = -1;
				throw (Err);
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

				string commandText_ = "sp_InsertUpdateDescrizioneCategoria";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
				dbEngine_.AddParameter("@Descrizione", Descrizione, ParameterDirection.Input);
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
            Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.InsertOrUpdateDescrizione.errore: ", Err);
            errore = -1;
            throw (Err);
        }
        finally
        {
            dbEngine_.CloseConnection();
        }

        return errore;
        */
        }

        public DataTable GetAllCategorie(string IdEnte,string IdTributo)
		{
            //*** 20140410
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                Log.Debug("CategorieDAO::GetAllCategorie::esecuzione sp_GetAllCategorie");
                cmdMyCommand.CommandText = "sp_GetAllCategorie";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.GetAllCategorie.errore: ", Err);
                
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;

            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetAllCategorie";
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.GetAllCategorie.errore: ", Err);
                
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
                Log.Debug("CategorieDAO::connessioneDB::apertura della connessione al DB");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.connessioneDB.errore: ", Err);

                throw (Err);
            }

        }

        public int InsertRibalta(int ribaltaDa, int ribaltaA, string IdEnte)
        {
            //*** 20170328
            connessioneDB();

            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();                
                cmdMyCommand.Parameters.AddWithValue("@DA", ribaltaDa);
                cmdMyCommand.Parameters.AddWithValue("@A", ribaltaA);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);

                Log.Debug("CategorieDAO::InsertRibalta::esecuzione prc_RibaltaCategorie");
                cmdMyCommand.CommandText = "prc_RibaltaCategorie";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
                foreach (DataRow myRow in dtMyDati.Rows)
                {
                    if (myRow[0] != DBNull.Value)
                    {
                        errore = int.Parse(myRow[0].ToString());
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.InsertRibalta.errore: ", Err);

                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return errore;
        }


        public void connessioneDB(string ConnectionString)
        {
            //*** 20140409
            try {
                Log.Debug("AnagraficheDAO::connessioneDB::apertura della connessione al DB su " + ConnectionString);

                cmdMyCommand = new SqlCommand();
                myDataReader = null;
                dtMyDati = new DataTable();

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(ConnectionString);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CategorieDAO.connessioneDB.errore: ", Err);

                throw (Err);
            }

        }
	}
}
