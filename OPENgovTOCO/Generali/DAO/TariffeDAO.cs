using System;
using System.Data;
using log4net;
using OPENGovTOCO;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione Tariffe.
    /// </summary>
    public class TariffeDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TariffeDAO));
        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		public TariffeDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int DeleteTariffa(int IdTariffa)
		{
            //*** 20140410
            connessioneDB();

            Int32 errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdTariffa", IdTariffa);
                //*** 20140416 - modificato un parametro da -1 a +1
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("TariffeDAO::DeleteTariffa::esecuzione sp_DeleteTariffa");
                cmdMyCommand.CommandText = "sp_DeleteTariffa";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
           catch (Exception Err)
            {
               errore = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.DeleteTariffa.errore: ", Err);

                
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

				string commandText_ = "sp_DeleteTariffa";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdTariffa", IdTariffa, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (Int32)parameter_.Value;
			}
			catch (Exception Err)
            {
               errore = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.DeleteTariffa.errore: ", Err);

                
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return errore;
			*/
        }

        public int InsertTariffa(string ConnectionString, string IdEnte, string IdTributo, int IdCategoria, int IdTipologiaOccupazione, int IdDurata, int Anno, decimal Valore, decimal MinimoApplicabile)
		{
            //*** 20140410
            connessioneDB(ConnectionString);
            
            int idTariffa = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", IdTipologiaOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@IdDurata", IdDurata);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@Valore", Valore);
                cmdMyCommand.Parameters.AddWithValue("@MinimoApplicabile", MinimoApplicabile);
                //*** 20140416 - cambiato il parametro da -1 a +1
                //cmdMyCommand.Parameters.AddWithValue("@IdTariffa", -1);
                cmdMyCommand.Parameters.AddWithValue("@IdTariffa", -1);
                cmdMyCommand.Parameters["@IdTariffa"].Direction = ParameterDirection.InputOutput;

                Log.Debug("TariffeDAO::InsertTariffa::esecuzione sp_InsertTariffe");
                cmdMyCommand.CommandText = "sp_InsertTariffe";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                idTariffa = (int)cmdMyCommand.Parameters["@IdTariffa"].Value;
            }
            catch (Exception Err)
            {
                idTariffa = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.InsertTariffa.errore: ", Err);


            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return idTariffa;

            /*
			DBEngine dbEngine_;		
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			int idTariffa = 0;
			try
			{
				IDataParameterCollection parameterCollection_;
				IDataParameter parameter_;

				string commandText_ = "sp_InsertTariffe";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTributo", IdTributo, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdDurata", IdDurata, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@Valore", Valore, ParameterDirection.Input);
				dbEngine_.AddParameter("@MinimoApplicabile", MinimoApplicabile, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTariffa", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@IdTariffa"];
				idTariffa = (int)parameter_.Value;
			}
			 catch (Exception Err)
            {
                idTariffa = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.InsertTariffa.errore: ", Err);


            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return idTariffa;
			*/
        }

        public int UpdateTariffa(int IdTariffa, decimal Valore, decimal MinimoApplicabile)
		{
            //*** 20140410
            connessioneDB();

            int errore = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdTariffa", IdTariffa);
                cmdMyCommand.Parameters.AddWithValue("@Valore", Valore);
                cmdMyCommand.Parameters.AddWithValue("@MinimoApplicabile", MinimoApplicabile);
                cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", -1);
                //cmdMyCommand.Parameters.AddWithValue("@CodiceErrore", 1);
                Log.Debug("TariffeDAO::UpdateTariffa::esecuzione sp_UpdateTariffa");
                cmdMyCommand.CommandText = "sp_UpdateTariffa";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = (int)cmdMyCommand.Parameters["@CodiceErrore"].Value;
            }
            catch (Exception Err)
            {
                errore = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.UpdateTariffa.errore: ", Err);


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

				string commandText_ = "sp_UpdateTariffa";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdTariffa", IdTariffa, ParameterDirection.Input);
				dbEngine_.AddParameter("@Valore", Valore, ParameterDirection.Input);
				dbEngine_.AddParameter("@MinimoApplicabile", MinimoApplicabile, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceErrore", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
				parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				parameter_ = (IDataParameter)parameterCollection_["@CodiceErrore"];
				errore = (int)parameter_.Value;
			}
			 catch (Exception Err)
            {
                errore = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.UpdateTariffa.errore: ", Err);


            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return errore;
			*/
        }
        public DataTable GetAnniTariffe(string IdEnte, string IdTributo)
        {
            //*** 20140410
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                Log.Debug("TariffeDAO::GetAllTariffe::esecuzione sp_GetAnniTariffe");
                cmdMyCommand.CommandText = "sp_GetAnniTariffe";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.GetAnniTariffe.errore: ", Err);
                throw (Err);

            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            return dtMyDati;
        }

		public DataTable GetAllTariffe(string IdEnte, string IdTributo, int IdCategoria, int IdTipologiaOccupazione, int IdDurata, int Anno)
		{
            //*** 20140410
            connessioneDB();

            try
            {

                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                if ((IdTributo != null) && (IdTributo.Trim() != ""))
                    cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                if (IdCategoria > 0)
                    cmdMyCommand.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                if (IdTipologiaOccupazione > 0)
                    cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", IdTipologiaOccupazione);
                if (IdDurata > 0)
                    cmdMyCommand.Parameters.AddWithValue("@IdDurata", IdDurata);
                if (Anno > 0)
                    cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                Log.Debug("TariffeDAO::GetAllTariffe::esecuzione sp_GetAllTariffe");
                cmdMyCommand.CommandText = "sp_GetAllTariffe";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.GetAllTariffe.errore: ", Err);
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
				string commandText_ = "sp_GetAllTariffe";
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				if((IdTributo != null) && (IdTributo.Trim() != ""))
					dbEngine_.AddParameter("@IdTributo", IdTributo, ParameterDirection.Input);
				if (IdCategoria > 0)
					dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
				if (IdTipologiaOccupazione > 0)
					dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
				if (IdDurata > 0)
					dbEngine_.AddParameter("@IdDurata", IdDurata, ParameterDirection.Input);
				if (Anno > 0)
					dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			 catch (Exception Err)
            {
                
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.GetAllTariffe.errore: ", Err);
                throw (Err);

            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			return dt;
            */
        }

        public DataTable GetTariffa(int IdTariffa)
		{
            //*** 20140410
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdTariffa", IdTariffa);
                Log.Debug("TariffeDAO::GetTariffa::esecuzione sp_GetAllTariffe");
                cmdMyCommand.CommandText = "sp_GetAllTariffe";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.GetTariffa.errore: ", Err);
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
				string commandText_ = "sp_GetAllTariffe";
				dbEngine_.AddParameter("@IdTariffa", IdTariffa, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			 catch (Exception Err)
            {
                
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.GetTariffa.errore: ", Err);
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
                Log.Debug("TariffeDAO::connessioneDB::apertura della connessione al DB");
                cmdMyCommand = new SqlCommand();
                myDataReader = null;
                dtMyDati = new DataTable();

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(OPENgovTOCO.DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.connessioneDB.errore: ", Err);
                

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

                Log.Debug("TariffeDAO::InsertRibalta::esecuzione prc_RibaltaTariffe");
                cmdMyCommand.CommandText = "prc_RibaltaTariffe";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                errore = 1;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.InsertRibalta.errore: ", Err);
                errore = -1;
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

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.TariffeDAO.connessioneDB.errore: ", Err);


            }

            
        }
	}
}
