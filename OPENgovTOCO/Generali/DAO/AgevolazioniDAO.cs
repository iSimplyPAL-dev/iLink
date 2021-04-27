using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
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

namespace DAO
{
	/// <summary>
	/// Classe interfacciamento DB per la gestione Agevolazioni.
	/// </summary>
	public class AgevolazioniDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(AgevolazioniDAO));
        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		public AgevolazioniDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int DeleteAgevolazione(int IdAgevolazione)
		{

            //*** 20140410
            connessioneDB();
            
            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", IdAgevolazione);
                //*** 20140415 - modificato il valore da ritornare da -1 a 1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("AgevolazioniDAO::DeleteAgevolazione::esecuzione sp_DeleteAgevolazione");
                cmdMyCommand.CommandText = "sp_DeleteAgevolazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.DeleteAgevolazione.errore: ", Err);
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

				string commandText_ = "sp_DeleteAgevolazione";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdAgevolazione", IdAgevolazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;
			}
			catch (Exception Err)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.DeleteAgevolazione.errore: ", Err);
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

        public int InsertOrUpdateDescrizione(string IdEnte, int IdAgevolazione, string Descrizione)
		{
            //*** 20140410
            connessioneDB();

            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", IdAgevolazione);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                //*** 20140415 - modificato il valore da ritornare da -1 a 1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("AgevolazioniDAO::InsertOrUpdateDescrizione::esecuzione sp_InsertUpdateDescrizioneAgevolazione");
                cmdMyCommand.CommandText = "sp_InsertUpdateDescrizioneAgevolazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.InsertOrUpdateDescrizione.errore: ", Err);
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

				string commandText_ = "sp_InsertUpdateDescrizioneAgevolazione";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdAgevolazione", IdAgevolazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@Descrizione", Descrizione, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;*/
            /*
            string commandText_ = "UPDATE [dbo].[AGEVOLAZIONI] SET [DESCRIZIONE] = '" + Descrizione + "' WHERE IDAGEVOLAZIONE = " + IdAgevolazione + " AND IDENTE = '" + IdEnte + "'";
            dbEngine_.ExecuteNonQuery(commandText_, CommandType.Text);				
            errore = 0;
            */
            /*
			}
			catch (Exception Err)
			{
			Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.InsertOrUpdateDescrizione.errore: ", Err);
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

        public DataTable GetAllAgevolazioni(string IdEnte)
		{
            //*** 20140410
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                Log.Debug("AgevolazioniDAO::GetAllAgevolazioni::esecuzione sp_GetAllAgevolazioni");
                cmdMyCommand.CommandText = "sp_GetAllAgevolazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.GetAllAgevolazioni.errore: ", Err);
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
				string commandText_ = "sp_GetAllAgevolazioni";
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			catch (Exception Err)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.GetAllAgevolazioni.errore: ", Err);
				throw (Err);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
			*/
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

                Log.Debug("AgevolazioniDAO::InsertRibalta::esecuzione prc_RibaltaAgevolazioni");
                cmdMyCommand.CommandText = "prc_RibaltaAgevolazioni";
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.InsertRibalta.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return errore;
        }



        public void connessioneDB()
        {
            //*** 20140409
            try {
                Log.Debug("AgevolazioniDAO::connessioneDB::apertura della connessione al DB");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AgevolazioniDAO.connessioneDB.errore: ", Err);
                
            }

        }
	}
}
