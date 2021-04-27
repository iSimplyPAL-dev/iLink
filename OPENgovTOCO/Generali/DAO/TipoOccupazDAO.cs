using Microsoft.VisualBasic;
using System.Data;
using System;
using System.Collections;
using System.Configuration;
using System.Drawing;
using Anagrafica.DLL;
using log4net;
using OPENgovTOCO;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI.WebControls;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione TipoOccupaz.
    /// </summary>
    public class TipoOccupazDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TipoOccupazDAO));

        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		public TipoOccupazDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int DeleteTipologiaOccupazione(int IdTipologiaOccupazione)
		{
            //*** 20140409 
            connessioneDB();

            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", IdTipologiaOccupazione);
                //*** 20140415 - modificato il valore da ritornare da -1 a 1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("TipoOccupazDAO::DeleteTipologiaOccupazione::esecuzione sp_DeleteTipologiaOccupazione");
                cmdMyCommand.CommandText = "sp_DeleteTipologiaOccupazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.DeleteTipologiaOccupazione.errore: ", Err);
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

				string commandText_ = "sp_DeleteTipologiaOccupazione";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;
			}
			catch (Exception Err)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.DeleteTipologiaOccupazione.errore: ", Err);
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

        public int InsertOrUpdateDescrizione(string ConnectionString, string IdEnte, string IdTributo, int IdTipologiaOccupazione, string Descrizione)
		{
            //*** 20140409 
            connessioneDB(ConnectionString);

			int errore = 0;
			try
			{
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", IdTipologiaOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                //*** 20140415 - modificato il valore da ritornare da -1 a 1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("TipoOccupazDAO::InsertOrUpdateDescrizione::esecuzione sp_InsertUpdateDescrizioneTipologiaOccupazione");
                cmdMyCommand.CommandText = "sp_InsertUpdateDescrizioneTipologiaOccupazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;

				/*
				string commandText_ = "UPDATE [dbo].[TIPOLOGIAOCCUPAZIONI] SET [DESCRIZIONE] = '" + Descrizione + "' WHERE IDTIPOLOGIAOCCUPAZIONE = " + IdTipologiaOccupazione + " AND IDENTE = '" + IdEnte + "'";
				dbEngine_.ExecuteNonQuery(commandText_, CommandType.Text);				
				errore = 0;
				*/
            
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.InsertOrUpdateDescrizione.errore: ", Err);
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

				string commandText_ = "sp_InsertUpdateDescrizioneTipologiaOccupazione";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@Descrizione", Descrizione, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;
				/*
				string commandText_ = "UPDATE [dbo].[TIPOLOGIAOCCUPAZIONI] SET [DESCRIZIONE] = '" + Descrizione + "' WHERE IDTIPOLOGIAOCCUPAZIONE = " + IdTipologiaOccupazione + " AND IDENTE = '" + IdEnte + "'";
				dbEngine_.ExecuteNonQuery(commandText_, CommandType.Text);				
				errore = 0;
				*/
            /*
			}
			catch (Exception Err)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.InsertOrUpdateDescrizione.errore: ", Err);
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

        public DataTable GetAllTipoOccupazioni(string IdEnte, string IdTributo)
		{
            //*** 20140409 
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                Log.Debug("TipoOccupazDAO::GetAllTipoOccupazioni::esecuzione sp_GetAllTipoOccupazioni");
                cmdMyCommand.CommandText = "sp_GetAllTipoOccupazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.GetAllTipoOccupazioni.errore: ", Err);
               
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
				string commandText_ = "sp_GetAllTipoOccupazioni";
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			catch (Exception Err)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.GetAllTipoOccupazioni.errore: ", Err);
				
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
            try
            {
                Log.Debug("TipoOccupazDAO::connessioneDB::apertura della connessione al DB");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.connessioneDB.errore: ", Err);
                
                
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

                Log.Debug("TipoOccupazDAO::InsertRibalta::esecuzione prc_RibaltaTipologieOccupazione");
                cmdMyCommand.CommandText = "prc_RibaltaTipologieOccupazioni";
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.InsertRibalta.errore: ", Err);

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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TipoOccupazDAO.connessioneDB.errore: ", Err);

                
            }
           
        }
	}
}
