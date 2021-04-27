using System;
using System.Data;
using DTO;
using log4net;
using IRemInterfaceOSAP;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione Pagamenti.
    /// </summary>
    public class PagamentiDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PagamentiDAO));

        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		/// <summary>
		/// 
		/// </summary>
		public PagamentiDAO()
		{
			
		}


		/// <summary>
        /// 
        /// </summary>
        /// <param name="idEnte"></param>
        /// <param name="idContribuente"></param>
        /// <param name="codiceCartella"></param>
        /// <returns></returns>
		public double GetTotalePagatoPerCatella(string idEnte, int idContribuente, string codiceCartella)
		{
            //*** 20140409 
            connessioneDB();

            double ret = 0;
            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", idEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdContribuente", idContribuente);
                cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", codiceCartella);
                Log.Debug("PagamentiDAO::GetTotalePagatoPerCatella::esecuzione sp_GetTotalePagatoPerCartella ");
                cmdMyCommand.CommandText = "sp_GetTotalePagatoPerCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

                if (dtMyDati != null)
                {
                    try
                    {
                        ret = double.Parse(dtMyDati.Rows[0]["IMPORTO_PAGATO"].ToString());
                    }
                    catch(Exception Err) {
                        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetTotalePagatoPerCartella.errore: ", Err);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetTotalePagatoPerCartella.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return ret;

            /*
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			double ret = 0;
			try
			{
				string commandText_ = "sp_GetTotalePagatoPerCartella";
				dbEngine_.AddParameter("@IdEnte", idEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdContribuente", idContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceCartella", codiceCartella, ParameterDirection.Input);

				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);

				if (dt != null)
				{
					try
					{
						ret = double.Parse(dt.Rows[0]["IMPORTO_PAGATO"].ToString());
					} 
					catch {
                    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetTotalePagatoPerCartella.errore: ", Err);
                      }
				}
			}
			catch (Exception ex)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetTotalePagatoPerCartella.errore: ", Err);
				throw (ex);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return ret;
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPagamento"></param>
        public DataTable GetPagamentoById(int idPagamento)
		{
            //*** 20140409 
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdPagamento", idPagamento);
                Log.Debug("PagamentiDAO::GetPagamentoById::esecuzione sp_GetPagamentoById");
                cmdMyCommand.CommandText = "sp_GetPagamentoById";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetPagamentoById.errore: ", Err);

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
				string commandText_ = "sp_GetPagamentoById";
				dbEngine_.AddParameter("@IdPagamento", idPagamento, ParameterDirection.Input);

				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetPagamentoById.errore: ", Err);

                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameter"></param>
        public DataTable RicercaPagamenti(PagamentiSearch searchParameter)
		{
			return RicercaPagamenti(searchParameter, null);
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="sufxxExp"></param>
        /// <returns></returns>
		public DataTable RicercaPagamenti(PagamentiSearch searchParameter, string sufxxExp)
		{
            //*** 20140409 
            connessioneDB();

            object dataDal = null;
            object dataAl = null;

            if ((searchParameter.DataAccreditoDal > DateTime.MinValue && searchParameter.DataAccreditoDal < DateTime.MaxValue))
                dataDal = searchParameter.DataAccreditoDal;

            if ((searchParameter.DataAccreditoAl > DateTime.MinValue && searchParameter.DataAccreditoAl < DateTime.MaxValue))
                dataAl = searchParameter.DataAccreditoAl;

            try
            {
                string commandText_ = "sp_GetPagamenti" + sufxxExp;
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", searchParameter.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", searchParameter.IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@Anno", searchParameter.AnnoRif);
                cmdMyCommand.Parameters.AddWithValue("@Cognome", searchParameter.Cognome);
                cmdMyCommand.Parameters.AddWithValue("@Nome", searchParameter.Nome);
                cmdMyCommand.Parameters.AddWithValue("@Cf", searchParameter.CF);
                cmdMyCommand.Parameters.AddWithValue("@Piva", searchParameter.PIVA);
                cmdMyCommand.Parameters.AddWithValue("@NAvviso", searchParameter.NAvviso);
                cmdMyCommand.Parameters.AddWithValue("@DataAccreditoDal", dataDal);
                cmdMyCommand.Parameters.AddWithValue("@DataAccreditoAl", dataAl);
                cmdMyCommand.Parameters.AddWithValue("@IdContribuente", searchParameter.IdContribuente);
                //cmdMyCommand.CommandText = "sp_GetPagamenti" + sufxxExp;
                cmdMyCommand.CommandText = commandText_;

                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.RicercaPagamenti.errore: ", Err);

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
			

			object dataDal = null;
			object dataAl = null;

			if ((searchParameter.DataAccreditoDal > DateTime.MinValue && searchParameter.DataAccreditoDal < DateTime.MaxValue))
				dataDal = searchParameter.DataAccreditoDal;

			if ((searchParameter.DataAccreditoAl > DateTime.MinValue && searchParameter.DataAccreditoAl < DateTime.MaxValue))
				dataAl = searchParameter.DataAccreditoAl;

			try
			{
				string commandText_ = "sp_GetPagamenti" + sufxxExp;
				dbEngine_.AddParameter("@IdEnte", searchParameter.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", searchParameter.AnnoRif, ParameterDirection.Input);
				dbEngine_.AddParameter("@Cognome", searchParameter.Cognome, ParameterDirection.Input);
				dbEngine_.AddParameter("@Nome", searchParameter.Nome, ParameterDirection.Input);
				dbEngine_.AddParameter("@Cf", searchParameter.CF, ParameterDirection.Input);
				dbEngine_.AddParameter("@Piva", searchParameter.PIVA, ParameterDirection.Input);
				dbEngine_.AddParameter("@NAvviso", searchParameter.NAvviso, ParameterDirection.Input);
				dbEngine_.AddParameter("@DataAccreditoDal", dataDal, ParameterDirection.Input);
				dbEngine_.AddParameter("@DataAccreditoAl", dataAl, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdContribuente", searchParameter.IdContribuente, ParameterDirection.Input);
                Log.Debug("sp_GetPagamenti @IdEnte:" + searchParameter.IdEnte + "@Anno:" + searchParameter.AnnoRif + "@Cognome:" + searchParameter.Cognome + "@Nome:" + searchParameter.Nome + "@Cf:" + searchParameter.CF + "@Piva:" + searchParameter.PIVA + "@NAvviso:" + searchParameter.NAvviso + "@DataAccreditoDal:" + searchParameter.DataAccreditoDal.ToShortDateString() + "@DataAccreditoAl:" + searchParameter.DataAccreditoDal.ToShortDateString() + "@IdContribuente:" + searchParameter.IdContribuente.ToString());
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.RicercaPagamenti.errore: ", Err);

                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable RicercaRate(RataSearch searchParameter)
		{
            //*** 20140409 
            connessioneDB();

            try
            {
                object tmpAnno = null;
                if (searchParameter.Anno > 0)
                    tmpAnno = searchParameter.Anno;

                cmdMyCommand.Parameters.AddWithValue("@IdEnte", searchParameter.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", searchParameter.IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@Anno", tmpAnno);
                cmdMyCommand.Parameters.AddWithValue("@IdContribuente", searchParameter.IdContribuente);
                cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", searchParameter.CodiceCartella);
                cmdMyCommand.Parameters.AddWithValue("@IdCartella", searchParameter.IdCartella);
                cmdMyCommand.CommandText = "sp_GetRate";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.RicercaRate.errore: ", Err);

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
				object tmpAnno = null;
				if (searchParameter.Anno > 0)
					tmpAnno = searchParameter.Anno;

				string commandText_ = "sp_GetRate";
				dbEngine_.AddParameter("@IdEnte", searchParameter.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", tmpAnno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdContribuente", searchParameter.IdContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceCartella", searchParameter.CodiceCartella, ParameterDirection.Input);
				
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception ex)
			{
				 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.RicercaRate.errore: ", Err);

                throw (Err);
            }
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
             */
        }

        /// <summary>
        /// Inserisce o modifica un pagamento
        /// </summary>
        /// <param name="objToInsertOrUpdate"></param>
        /// <param name="operatore"></param>
        /// <returns></returns>
        public int InsertUpdatePagamento(PagamentoExt objToInsertOrUpdate, string operatore)
		{
            //*** 20140409 
            connessioneDB();

            int esito = 0;
            try
            {
                object tmpDataAccredito = null;
                if (objToInsertOrUpdate.DataAccredito > DateTime.MinValue.AddYears(1755))
                    tmpDataAccredito = objToInsertOrUpdate.DataAccredito;

                string commandText_ = "sp_InsertUpdatePagamento";
                // ID
                cmdMyCommand.Parameters.AddWithValue("@IdPagamento", objToInsertOrUpdate.IdPagamento);

                // Obbligatori
                cmdMyCommand.Parameters.AddWithValue("@cod_contribuente", objToInsertOrUpdate.CodContribuente);
                cmdMyCommand.Parameters.AddWithValue("@iddataanagrafica", objToInsertOrUpdate.IdDataAnagrafica);
                cmdMyCommand.Parameters.AddWithValue("@idente", objToInsertOrUpdate.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", objToInsertOrUpdate.CodiceCartella);
                cmdMyCommand.Parameters.AddWithValue("@importo_pagato", objToInsertOrUpdate.ImportoPagato);
                cmdMyCommand.Parameters.AddWithValue("@data_pagamento", objToInsertOrUpdate.DataPagamento);

                // Opzionali
                cmdMyCommand.Parameters.AddWithValue("@numero_rata", objToInsertOrUpdate.NumeroRataString);
                cmdMyCommand.Parameters.AddWithValue("@data_accredito", tmpDataAccredito);
                cmdMyCommand.Parameters.AddWithValue("@provenienza", objToInsertOrUpdate.Provenienza);
                cmdMyCommand.Parameters.AddWithValue("@codice_bollettino", objToInsertOrUpdate.CodiceBollettino);
                cmdMyCommand.Parameters.AddWithValue("@anno", objToInsertOrUpdate.Anno);
                cmdMyCommand.Parameters.AddWithValue("@operatore", operatore);
                cmdMyCommand.Parameters.AddWithValue("@idcartella", null);
                //cmdMyCommand.CommandText = "sp_InsertUpdatePagamento";
                Log.Debug("PagamentiDAO::InsertUpdatePagamento::esecuzione sp_InsertUpdatePagamento"); 
                cmdMyCommand.CommandText = commandText_;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                esito = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetPagamentoById.errore: ", Err);

                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return esito;
            /*
			int esito = 0;
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				object tmpDataAccredito = null;
				if (objToInsertOrUpdate.DataAccredito > DateTime.MinValue.AddYears(1755))
					tmpDataAccredito = objToInsertOrUpdate.DataAccredito;

				string commandText_ = "sp_InsertUpdatePagamento";
				// ID
				dbEngine_.AddParameter("@IdPagamento", objToInsertOrUpdate.IdPagamento, ParameterDirection.Input);
				
				// Obbligatori
				dbEngine_.AddParameter("@cod_contribuente", objToInsertOrUpdate.CodContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@iddataanagrafica", objToInsertOrUpdate.IdDataAnagrafica, ParameterDirection.Input);
				dbEngine_.AddParameter("@idente", objToInsertOrUpdate.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodiceCartella", objToInsertOrUpdate.CodiceCartella, ParameterDirection.Input);				
				dbEngine_.AddParameter("@importo_pagato", objToInsertOrUpdate.ImportoPagato, ParameterDirection.Input);
				dbEngine_.AddParameter("@data_pagamento", objToInsertOrUpdate.DataPagamento, ParameterDirection.Input);
				
				// Opzionali
				dbEngine_.AddParameter("@numero_rata", objToInsertOrUpdate.NumeroRataString, ParameterDirection.Input);
				dbEngine_.AddParameter("@data_accredito", tmpDataAccredito, ParameterDirection.Input);
				dbEngine_.AddParameter("@provenienza", objToInsertOrUpdate.Provenienza, ParameterDirection.Input);
				dbEngine_.AddParameter("@codice_bollettino", objToInsertOrUpdate.CodiceBollettino, ParameterDirection.Input);
				dbEngine_.AddParameter("@anno", objToInsertOrUpdate.Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@operatore", operatore, ParameterDirection.Input);
				dbEngine_.AddParameter("@idcartella", null, ParameterDirection.Input);
				
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                esito = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.GetPagamentoById.errore: ", Err);

                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return esito;
            */
        }

        /// <summary>
        /// Cancella un pagamento
        /// </summary>
        /// <param name="idPagamento"></param>
        /// <returns></returns>
        public int DeletePagamento(int idPagamento)
		{
            //*** 20140409 
            connessioneDB();
			int esito = 0;

            try
            {
                // ID
                cmdMyCommand.Parameters.AddWithValue("@IdPagamento", idPagamento);
                Log.Debug("PagamentiDAO::DeletePagamento::esecuzione sp_DeletePagamento");
                cmdMyCommand.CommandText = "sp_DeletePagamento";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                esito = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.DeletePagamento.errore: ", Err);

                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return esito;

            /*
			DBEngine dbEngine_;
		
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_DeletePagamento";

				// ID
				dbEngine_.AddParameter("@IdPagamento", idPagamento, ParameterDirection.Input);							
				dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
			}
		catch (Exception Err)
            {
                esito = -1;
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.DeletePagamento.errore: ", Err);

                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return esito;
            */
        }


        public void connessioneDB()
        {
            //*** 20140409
            try
            {
                Log.Debug("PagamentiDAO::connessioneDB::apertura della connessione al DB");
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
                
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.PagamentiDAO.connessioneDB.errore: ", Err);

                
            }


        }
	}
}
