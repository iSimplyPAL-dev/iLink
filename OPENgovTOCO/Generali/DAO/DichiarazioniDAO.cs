using System;
using System.Data;
using IRemInterfaceOSAP;
using OPENgovTOCO;
using System.Data.SqlClient;
using log4net;
using AnagInterface;
using Utility;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione dichiarazioni
    /// </summary>
	public class DichiarazioniDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioniDAO));
        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;


		public DichiarazioniDAO()
		{
		}

		public DataTable GetArticolo(int IdRuolo)
		{
            //*** 20140409  - cambiato modo di connesione
            cmdMyCommand = new SqlCommand();
            myDataReader = null;
            dtMyDati = new DataTable();
            
            try
            {
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdRuolo", IdRuolo);
                Log.Debug("DichiarazioniDAO::GetArticolo::esecuzione sp_GetArticolo");
                cmdMyCommand.CommandText = "sp_GetArticolo";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetArticolo.errore: ", Err);                
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
            /*
            //********
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetArticolo";
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdArticolo", IdArticolo, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetArticolo.errore: ", Err);
                
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
			*/
        }
        /**** 201810 - Calcolo puntuale ****/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="ListOccupazioni"></param>
        /// <param name="cmdMyCommand"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="10/2018">
        /// <strong>Calcolo Puntuale</strong>
        /// </revision>
        /// <revision date="24/03/2020">In caso di supplettivo devo considerare solo l'avviso precedente per le stesse tipologie di occupazioni</revision>
        /// </revisionHistory>
        public DataTable GetArticoloPrecedente(string IdEnte, int IdContribuente, int Anno, string ListOccupazioni, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            //connessioneDB();
            try
            {
                dtMyDati = new DataTable();

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdContribuente", IdContribuente);
                cmdMyCommand.Parameters.AddWithValue("@LISTOCCUPAZIONI", ListOccupazioni);
                cmdMyCommand.CommandText = "sp_GetArticoloPrecedente";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

                return dtMyDati;
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.DichiarazioniDAO.GetArticoloPrecedente.errore: ", Err);
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                throw (Err);
            }
            //finally
            //{
            //    cmdMyCommand.Connection.Close();
            //}
        }
        //public DataTable GetArticoloPrecedente(string IdEnte, int IdContribuente, int Anno,int IdDichiarazione, ref SqlCommand cmdMyCommand)
        //{
        //    //*** 20140409
        //    //connessioneDB();
        //    try
        //    {
        //        dtMyDati = new DataTable();

        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
        //        cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
        //        cmdMyCommand.Parameters.AddWithValue("@IdContribuente", IdContribuente);
        //        cmdMyCommand.Parameters.AddWithValue("@IDDICHIARAZIONE", IdDichiarazione);
        //        cmdMyCommand.CommandText = "sp_GetArticoloPrecedente";
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //        myDataReader = cmdMyCommand.ExecuteReader();
        //        dtMyDati.Load(myDataReader);

        //        return dtMyDati;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(IdEnte + " - OPENgovOSAP.DichiarazioniDAO.GetArticoloPrecedente.errore: ", Err);
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //        throw (Err);
        //    }
        //    //finally
        //    //{
        //    //    cmdMyCommand.Connection.Close();
        //    //}
        //}
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="dichiarazione"></param>
        public void SetDichiarazione(string ConnectionString, ref DichiarazioneTosapCosap dichiarazione)
		{
            //*** 20140409
            SqlTransaction startTransazione;
            int IdRif = dichiarazione.IdDichiarazione;
            try
            {
                connessioneDB(ConnectionString);
                startTransazione = cmdMyCommand.Connection.BeginTransaction();
                
                /**************************
                **** Dichiarazione Data ***
                **************************/

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@DataDichiarazione", dichiarazione.TestataDichiarazione.DataDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@NDichiarazione", dichiarazione.TestataDichiarazione.NDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@TipoAtto", dichiarazione.TestataDichiarazione.IdTipoAtto);
                cmdMyCommand.Parameters.AddWithValue("@IdUfficio", dichiarazione.TestataDichiarazione.Ufficio.IdUfficio);
                cmdMyCommand.Parameters.AddWithValue("@IdTitoloRichiedente", dichiarazione.TestataDichiarazione.TitoloRichiedente.IdTitoloRichiedente);
                cmdMyCommand.Parameters.AddWithValue("@CodContribuente", dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", dichiarazione.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Note", SharedFunction.StringToDbNull(dichiarazione.TestataDichiarazione.NoteDichiarazione));
                cmdMyCommand.Parameters.AddWithValue("@Operatore", dichiarazione.TestataDichiarazione.Operatore);
                cmdMyCommand.Parameters.AddWithValue("@DataInserimento", dichiarazione.TestataDichiarazione.DataInserimento);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo",dichiarazione.CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", dichiarazione.IdDichiarazione);
                cmdMyCommand.Parameters["@IdDichiarazione"].Direction = ParameterDirection.InputOutput;
                Log.Debug("DichiarazioniDAO::SetDichiarazione::esecuzione sp_InsertDichiarazione");
                cmdMyCommand.CommandText = "sp_InsertDichiarazione";
                cmdMyCommand.Transaction=startTransazione;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                dichiarazione.IdDichiarazione = StringOperation.FormatInt(cmdMyCommand.Parameters["@IdDichiarazione"].Value);

                /*********************
                **** Articoli Data ***
                **********************/
                DAO.ArticoliDAO FncArticolo = new DAO.ArticoliDAO();
                foreach(Articolo myItem in dichiarazione.ArticoliDichiarazione)//for (int i = 0; i < dichiarazione.ArticoliDichiarazione.Length; i++)
                {
                    myItem.IdDichiarazione = dichiarazione.IdDichiarazione;
                    myItem.IdArticolo=FncArticolo.SetArticolo(ref cmdMyCommand, myItem);
                    if (myItem.IdArticolo <= 0)
                        throw new Exception("Errore in inserimento articolo");
                }
                cmdMyCommand.Transaction.Commit();
                new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, DichiarazioneSession.sOperatore, new Utility.Costanti.LogEventArgument().Immobile, "SetDichiarazione", (IdRif > 0?Utility.Costanti.AZIONE_UPDATE.ToString(): Utility.Costanti.AZIONE_NEW.ToString()), DichiarazioneSession.CodTributo(""), DichiarazioneSession.IdEnte, dichiarazione.IdDichiarazione);
            }
            catch (Exception Err)
            {
                cmdMyCommand.Transaction.Rollback();
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.SetDichiarazione.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            /*
			DBEngine dbEngine_ = null;
			
			try
			{
				
				IDataParameterCollection parameterCollection_;
				IDataParameter parameter_;
						
				dbEngine_ = DBEngineFactory.GetDBEngine();
				dbEngine_.OpenConnection();
				dbEngine_.BeginTransaction();
                 */
            /**************************
            **** Dichiarazione Data ***
            **************************/

            /*
            dbEngine_.ClearParameters();
            dbEngine_.AddParameter("@DataDichiarazione", dichiarazione.TestataDichiarazione.DataDichiarazione, ParameterDirection.Input);
            dbEngine_.AddParameter("@NDichiarazione", dichiarazione.TestataDichiarazione.NDichiarazione, ParameterDirection.Input);
            dbEngine_.AddParameter("@TipoAtto", dichiarazione.TestataDichiarazione.IdTipoAtto, ParameterDirection.Input);
            dbEngine_.AddParameter("@IdUfficio", dichiarazione.TestataDichiarazione.Ufficio.IdUfficio, ParameterDirection.Input);
            dbEngine_.AddParameter("@IdTitoloRichiedente", dichiarazione.TestataDichiarazione.TitoloRichiedente.IdTitoloRichiedente, ParameterDirection.Input);
            dbEngine_.AddParameter("@CodContribuente", dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, ParameterDirection.Input);
            dbEngine_.AddParameter("@IdEnte", dichiarazione.IdEnte, ParameterDirection.Input);
            dbEngine_.AddParameter("@Note", SharedFunction.StringToDbNull(dichiarazione.TestataDichiarazione.NoteDichiarazione), ParameterDirection.Input);
            dbEngine_.AddParameter("@Operatore", dichiarazione.TestataDichiarazione.Operatore, ParameterDirection.Input);
            dbEngine_.AddParameter("@DataInserimento", dichiarazione.TestataDichiarazione.DataInserimento, ParameterDirection.Input);

            dbEngine_.AddParameter("@IdDichiarazione", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);

            parameterCollection_ = dbEngine_.ExecuteNonQuery("sp_InsertDichiarazione", CommandType.StoredProcedure);
            parameter_ = (IDataParameter)parameterCollection_["@IdDichiarazione"];
            int dichiarazioneId = StringOperation.FormatInt(parameter_.Value;
            dichiarazione.IdDichiarazione = dichiarazioneId;
            */
            /*********************
            **** Articoli Data ***
            **********************/
            /*
            DAO.ArticoliDAO FncArticolo = new DAO.ArticoliDAO();
            for (int i=0;i<dichiarazione.ArticoliDichiarazione.Length;i++)
            {
                dichiarazione.ArticoliDichiarazione[i].IdDichiarazione=dichiarazione.IdDichiarazione;
                FncArticolo.SetArticolo(ref dbEngine_, dichiarazione.ArticoliDichiarazione[i]);
            }

            dbEngine_.CommitTransaction();  

        }
        catch (Exception Err)
        {
            dbEngine_.RollbackTransaction(); 
        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.SetDichiarazione.errore: ", Err);
            throw (Err);
        }
        finally
        {
            dbEngine_.CloseConnection();
        }*/
        }

        public void UpdateDichiarazione(ref SqlCommand cmdMyCommand, DichiarazioneTosapCosap dichiarazione)
		{
            //*** 20140409
           // connessioneDB();
            try
            {
                /**************************
                **** Dichiarazione Data ***
                **************************/

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@DataDichiarazione", dichiarazione.TestataDichiarazione.DataDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@NDichiarazione", dichiarazione.TestataDichiarazione.NDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@TipoAtto", dichiarazione.TestataDichiarazione.IdTipoAtto);
                cmdMyCommand.Parameters.AddWithValue("@IdUfficio", dichiarazione.TestataDichiarazione.Ufficio.IdUfficio);
                cmdMyCommand.Parameters.AddWithValue("@IdTitoloRichiedente", dichiarazione.TestataDichiarazione.TitoloRichiedente.IdTitoloRichiedente);
                cmdMyCommand.Parameters.AddWithValue("@CodContribuente", dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", dichiarazione.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", dichiarazione.CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@Note", SharedFunction.StringToDbNull(dichiarazione.TestataDichiarazione.NoteDichiarazione));
                cmdMyCommand.Parameters.AddWithValue("@Operatore", dichiarazione.TestataDichiarazione.Operatore);
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", dichiarazione.IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@DataVariazione", dichiarazione.TestataDichiarazione.DataVariazione);

                if (dichiarazione.TestataDichiarazione.DataCessazione == DateTime.MinValue)
                {
                    cmdMyCommand.Parameters.AddWithValue("@DataCessazione", System.Convert.DBNull);
                }
                else
                {
                    cmdMyCommand.Parameters.AddWithValue("@DataCessazione", dichiarazione.TestataDichiarazione.DataCessazione);
                }
                Log.Debug("DichiarazioniDAO::UpdateDichiarazione::esecuzione sp_UpdateDichiarazione");
                cmdMyCommand.CommandText="sp_UpdateDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                /*********************
                **** Articoli Data ***
                **********************/

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", dichiarazione.IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@DataVariazione", dichiarazione.TestataDichiarazione.DataVariazione);
                if (dichiarazione.TestataDichiarazione.DataCessazione == DateTime.MinValue)
                {
                    cmdMyCommand.Parameters.AddWithValue("@DataCessazione", System.Convert.DBNull);
                }
                else
                {
                    cmdMyCommand.Parameters.AddWithValue("@DataCessazione", dichiarazione.TestataDichiarazione.DataCessazione);
                }
                Log.Debug("DichiarazioniDAO::UpdateDichiarazione::esecuzione sp_UpdateArticoloByDichiarazione");
                cmdMyCommand.CommandText = "sp_UpdateArticoloByDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.UpdateDichiarazione.errore: ", Err);
                throw (Err);
            }
            //finally
            //{
            //    cmdMyCommand.Connection.Close();
            //}
            /*
			try
			{
			
				IDataParameterCollection parameterCollection_;*/
            /**************************
            **** Dichiarazione Data ***
            **************************/
            /*
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@DataDichiarazione", dichiarazione.TestataDichiarazione.DataDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@NDichiarazione", dichiarazione.TestataDichiarazione.NDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@TipoAtto", dichiarazione.TestataDichiarazione.IdTipoAtto, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdUfficio", dichiarazione.TestataDichiarazione.Ufficio.IdUfficio, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTitoloRichiedente", dichiarazione.TestataDichiarazione.TitoloRichiedente.IdTitoloRichiedente, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodContribuente", dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", dichiarazione.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@Note", SharedFunction.StringToDbNull(dichiarazione.TestataDichiarazione.NoteDichiarazione), ParameterDirection.Input);
				dbEngine_.AddParameter("@Operatore", dichiarazione.TestataDichiarazione.Operatore, ParameterDirection.Input);
        		dbEngine_.AddParameter("@IdDichiarazione", dichiarazione.IdDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@DataVariazione", dichiarazione.TestataDichiarazione.DataVariazione, ParameterDirection.Input);
				if (dichiarazione.TestataDichiarazione.DataCessazione==DateTime.MinValue)
				{
					dbEngine_.AddParameter("@DataCessazione",  System.Convert.DBNull, ParameterDirection.Input);
				}
				else
				{
					dbEngine_.AddParameter("@DataCessazione", dichiarazione.TestataDichiarazione.DataCessazione, ParameterDirection.Input);
				}
               
				dbEngine_.ExecuteNonQuery("sp_UpdateDichiarazione", CommandType.StoredProcedure);
            */
            /*********************
            **** Articoli Data ***
            **********************/
            /*
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdDichiarazione", dichiarazione.IdDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@DataVariazione", dichiarazione.TestataDichiarazione.DataVariazione, ParameterDirection.Input);
				if (dichiarazione.TestataDichiarazione.DataCessazione==DateTime.MinValue)
				{
					dbEngine_.AddParameter("@DataCessazione",  System.Convert.DBNull, ParameterDirection.Input);
				}
				else
				{
					dbEngine_.AddParameter("@DataCessazione", dichiarazione.TestataDichiarazione.DataCessazione, ParameterDirection.Input);
				}

				parameterCollection_ = dbEngine_.ExecuteNonQuery("sp_UpdateArticoloByDichiarazione", CommandType.StoredProcedure);

			}
             catch (Exception Err)
            {
               Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.UpdateDichiarazione.errore: ", Err);

                throw (Err);
            }
			*/
        }

        //public void UpdateDichiarazione(ref DBEngine dbEngine_, DichiarazioneTosapCosap dichiarazione)
        //{
        //    try
        //    {

        //        IDataParameterCollection parameterCollection_;
        //        /**************************
        //        **** Dichiarazione Data ***
        //        **************************/

        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@DataDichiarazione", dichiarazione.TestataDichiarazione.DataDichiarazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@NDichiarazione", dichiarazione.TestataDichiarazione.NDichiarazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@TipoAtto", dichiarazione.TestataDichiarazione.IdTipoAtto, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdUfficio", dichiarazione.TestataDichiarazione.Ufficio.IdUfficio, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdTitoloRichiedente", dichiarazione.TestataDichiarazione.TitoloRichiedente.IdTitoloRichiedente, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@CodContribuente", dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdEnte", dichiarazione.IdEnte, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Note", SharedFunction.StringToDbNull(dichiarazione.TestataDichiarazione.NoteDichiarazione), ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Operatore", dichiarazione.TestataDichiarazione.Operatore, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdDichiarazione", dichiarazione.IdDichiarazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataVariazione", dichiarazione.TestataDichiarazione.DataVariazione, ParameterDirection.Input);
        //        if (dichiarazione.TestataDichiarazione.DataCessazione==DateTime.MinValue)
        //        {
        //            dbEngine_.AddParameter("@DataCessazione",  System.Convert.DBNull, ParameterDirection.Input);
        //        }
        //        else
        //        {
        //            dbEngine_.AddParameter("@DataCessazione", dichiarazione.TestataDichiarazione.DataCessazione, ParameterDirection.Input);
        //        }

        //        dbEngine_.ExecuteNonQuery("sp_UpdateDichiarazione", CommandType.StoredProcedure);

        //    /*********************
        //    **** Articoli Data ***
        //    **********************/

        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdDichiarazione", dichiarazione.IdDichiarazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataVariazione", dichiarazione.TestataDichiarazione.DataVariazione, ParameterDirection.Input);
        //        if (dichiarazione.TestataDichiarazione.DataCessazione==DateTime.MinValue)
        //        {
        //            dbEngine_.AddParameter("@DataCessazione",  System.Convert.DBNull, ParameterDirection.Input);
        //        }
        //        else
        //        {
        //            dbEngine_.AddParameter("@DataCessazione", dichiarazione.TestataDichiarazione.DataCessazione, ParameterDirection.Input);
        //        }

        //        parameterCollection_ = dbEngine_.ExecuteNonQuery("sp_UpdateArticoloByDichiarazione", CommandType.StoredProcedure);

        //    }
        //   catch (Exception Err)
        //{
        // Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.UpdateDichiarazione.errore: ", Err);

        //   throw (Err);
        // }
        //}

        public DichiarazioneTosapCosap GetDichiarazione(string ConnectionString, int IdDichiarazione, string IdEnte, int Anno, string CodTributo)
		{     
            //*** 20140409
            connessioneDB(ConnectionString);
            try
            {
                Log.Debug("DichiarazioniDAO::GetDichiarazione::entrata");
                
                Log.Debug("DichiarazioniDAO::GetDichiarazione::aperto connessione");

				/**************************
				**** Dichiarazione Data ***
				**************************/
                DataTable dtDichiarazione = new DataTable();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                Log.Debug("DichiarazioniDAO::GetDichiarazione::esecuzione sp_GetDichiarazione");
                cmdMyCommand.CommandText="sp_GetDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtDichiarazione.Load(myDataReader);
                

				/*********************
				**** Articoli Data ***
				**********************/
                DataTable dtArticoli = new DataTable();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                Log.Debug("DichiarazioniDAO::GetDichiarazione::esecuzione sp_GetArticoliByIdDichiarazione @IdDichiarazione=" + IdDichiarazione.ToString() + ", @Anno="+ Anno.ToString());
                cmdMyCommand.CommandText = "sp_GetArticoliByIdDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtArticoli.Load(myDataReader);

                return FillDichiarazione(ConnectionString, dtDichiarazione, dtArticoli, false);//, CodTributo
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetDichiarazione.errore: ", Err);

                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }


            /*
             DBEngine dbEngine_ = null;

             try
             {
                 Log.Debug("DichiarazioniDAO::GetDichiarazione::entrata");
                 dbEngine_ = DBEngineFactory.GetDBEngine();
                 dbEngine_.OpenConnection();
                 Log.Debug("DichiarazioniDAO::GetDichiarazione::aperto connessione");
                  */
            /**************************
            **** Dichiarazione Data ***
            **************************/
            /*
				DataTable dtDichiarazione = new DataTable();
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdDichiarazione", IdDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dtDichiarazione, "sp_GetDichiarazione", CommandType.StoredProcedure);
				Log.Debug("DichiarazioniDAO::GetDichiarazione::query sp_GetDichiarazione");
            */
            /*********************
            **** Articoli Data ***
            **********************/
            /*
				DataTable dtArticoli = new DataTable();
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdDichiarazione", IdDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dtArticoli, "sp_GetArticoliByIdDichiarazione", CommandType.StoredProcedure);
				Log.Debug("DichiarazioniDAO::GetDichiarazione::query sp_GetArticoliByIdDichiarazione");
				return FillDichiarazione(dtDichiarazione, dtArticoli, CodTributo, false);
			}
			catch (Exception Err)
			{
				dbEngine_.RollbackTransaction(); 
			  Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetDichiarazione.errore: ", Err);
				throw (Err);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
              */
        }

        public string GetNDichAutomatico(string ConnectionString,  string IdEnte, string CodTributo)
        {
            string sRet = "";
            connessioneDB(ConnectionString);
            try
            {
                DataTable dtDichiarazione = new DataTable();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@CodTributo", CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.CommandText = "prc_GetNDichAutomatico";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
                foreach (DataRow myRow in dtMyDati.Rows)
                {
                    sRet = myRow["ndich"].ToString();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetNDichAutomatico.errore: ", Err);


            }
            finally
            {
                myDataReader.Close();
                cmdMyCommand.Connection.Close();
            }
            return sRet;
        }

        //public DichiarazioneTosapCosap GetDichiarazioneForMotore(int IdDichiarazione,string IdEnte, string CodTributo,int Anno, ref DBEngine dbEngine_)
        //{
        //    try
        //    {			
        //        /**************************
        //        **** Dichiarazione Data ***
        //        **************************/
        //        DataTable dtDichiarazione = new DataTable();
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdDichiarazione", IdDichiarazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);

        //        dbEngine_.ExecuteQuery(ref dtDichiarazione, "sp_GetDichiarazione", CommandType.StoredProcedure);		

        //        /*********************
        //        **** Articoli Data ***
        //        **********************/
        //        DataTable dtArticoli = new DataTable();
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdDichiarazione", IdDichiarazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);

        //        dbEngine_.ExecuteQuery(ref dtArticoli, "sp_GetArticoliByIdDichiarazione", CommandType.StoredProcedure);

        //        return FillDichiarazione(dtDichiarazione, dtArticoli, CodTributo, true);
        //    }
        //    catch (Exception Err)
        //    {
        //         Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetDichiarazioneForMotore.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDichiarazione"></param>
        /// <param name="IdEnte"></param>
        /// <param name="CodTributo"></param>
        /// <param name="Anno"></param>
        /// <param name="ListOccupazioni"></param>
        /// <param name="cmdMyCommand"></param>
        /// <returns></returns>
        public DichiarazioneTosapCosap GetDichiarazioneForMotore(int IdDichiarazione, string IdEnte, string CodTributo, int Anno, string ListOccupazioni, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            DataTable dtMyArticoli = new DataTable();
            SqlDataReader myDataReaderArticoli;
            dtMyDati = new DataTable();
            try
            {
                /**************************
                **** Dichiarazione Data ***
                **************************/
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                    cmdMyCommand.Connection.Open();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Tributo", CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@LISTOCCUPAZIONI", ListOccupazioni);
                Log.Debug("DichiarazioniDAO::GetDichiarazioneForMotore::esecuzione sp_GetDichiarazione @IdDichiarazione=" + IdDichiarazione.ToString() + ", @IdEnte=" + IdEnte + ", @Tributo=" + CodTributo + ", @LISTOCCUPAZIONI=" + ListOccupazioni);
                cmdMyCommand.CommandText = "sp_GetDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

                /*********************
                **** Articoli Data ***
                **********************/

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@LISTOCCUPAZIONI", ListOccupazioni);
                Log.Debug("DichiarazioniDAO::GetDichiarazioneForMotore::esecuzione sp_GetArticoliByIdDichiarazione @IdDichiarazione=" + IdDichiarazione.ToString() + ", @Anno=" + Anno.ToString() + ", @LISTOCCUPAZIONI=" + ListOccupazioni);
                cmdMyCommand.CommandText = "sp_GetArticoliByIdDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReaderArticoli = cmdMyCommand.ExecuteReader();
                dtMyArticoli.Load(myDataReaderArticoli);

                return FillDichiarazione(cmdMyCommand.Connection.ConnectionString, dtMyDati, dtMyArticoli, true);//, CodTributo
            }
            catch (Exception Err)
            {
                 Log.Debug(IdEnte + " - OPENgovOSAP.DichiarazioniDAO.GetDichiarazioneForMotore.errore: ", Err);
                throw (Err);
            }
            }


        public DataTable SearchDichiarazioni(DTO.DichiarazioneSearch SearchParams)
		{
            //*** 20140409
            connessioneDB();

            try
            {
                string tableName = String.Empty;

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", SearchParams.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", SearchParams.CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@Cognome", SearchParams.CognomeContribuente);
                cmdMyCommand.Parameters.AddWithValue("@Nome", SearchParams.NomeContribuente);
                cmdMyCommand.Parameters.AddWithValue("@CodFiscale", SearchParams.CodFiscaleContribuente);
                cmdMyCommand.Parameters.AddWithValue("@PartitaIva", SearchParams.PIVAContribuente);
                cmdMyCommand.Parameters.AddWithValue("@NDichiarazione", SearchParams.NDichiarazione);
                cmdMyCommand.CommandText = "sp_SearchDichiarazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));

                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.SearchDichiarazioni.errore: ", Err);
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
				string tableName = String.Empty;

				string commandText_ = "sp_SearchDichiarazioni";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", SearchParams.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@Cognome", SearchParams.CognomeContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@Nome", SearchParams.NomeContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodFiscale", SearchParams.CodFiscaleContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@PartitaIva", SearchParams.PIVAContribuente, ParameterDirection.Input);

				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                 Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.SearchDichiarazioni.errore: ", Err);
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
        /// <param name="myStringConnection"></param>
        /// <param name="SearchParams"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="09/04/2014">
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public DataView PrintDichiarazioni(string myStringConnection,DTO.DichiarazioneSearch SearchParams)
        {
            connessioneDB(myStringConnection);

            try
            {
                string tableName = String.Empty;

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", SearchParams.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", SearchParams.CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@Cognome", SearchParams.CognomeContribuente);
                cmdMyCommand.Parameters.AddWithValue("@Nome", SearchParams.NomeContribuente);
                cmdMyCommand.Parameters.AddWithValue("@CodFiscale", SearchParams.CodFiscaleContribuente);
                cmdMyCommand.Parameters.AddWithValue("@PartitaIva", SearchParams.PIVAContribuente);
                cmdMyCommand.Parameters.AddWithValue("@NDichiarazione", SearchParams.NDichiarazione);
                cmdMyCommand.CommandText = "prc_StampaDichiarazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));

                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.PrintDichiarazioni.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            return dtMyDati.DefaultView;
        }
        //public DataView PrintDichiarazioni(DTO.DichiarazioneSearch SearchParams)
        //{
        //    //*** 20140409
        //    connessioneDB();

        //    try
        //    {
        //        string tableName = String.Empty;

        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.AddWithValue("@IdEnte", SearchParams.IdEnte);
        //        cmdMyCommand.Parameters.AddWithValue("@IdTributo", SearchParams.CodTributo);
        //        cmdMyCommand.Parameters.AddWithValue("@Cognome", SearchParams.CognomeContribuente);
        //        cmdMyCommand.Parameters.AddWithValue("@Nome", SearchParams.NomeContribuente);
        //        cmdMyCommand.Parameters.AddWithValue("@CodFiscale", SearchParams.CodFiscaleContribuente);
        //        cmdMyCommand.Parameters.AddWithValue("@PartitaIva", SearchParams.PIVAContribuente);
        //        cmdMyCommand.Parameters.AddWithValue("@NDichiarazione", SearchParams.NDichiarazione);
        //        cmdMyCommand.CommandText = "prc_StampaDichiarazioni";
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));

        //        myDataReader = cmdMyCommand.ExecuteReader();
        //        dtMyDati.Load(myDataReader);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.PrintDichiarazioni.errore: ", Err);
        //        throw (Err);
        //    }
        //    finally
        //    {
        //        cmdMyCommand.Connection.Close();
        //    }
        //    return dtMyDati.DefaultView;
        //}

        //public DataTable GetIdDichiarazioniAnno(int Anno, string IdEnte, int IdContribuente, ref DBEngine dbEngine__)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
        //        //*** 20130801 - accertamento OSAP ***
        //        dbEngine_.AddParameter("@IdContribuente", IdContribuente, ParameterDirection.Input);
        //        //*** ***

        //        dbEngine_.ExecuteQuery(ref dt, "sp_GetDichiarazioniFromArticoliAnno", CommandType.StoredProcedure);

        //        return dt;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.GetIdDichiarazioniAnno.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        /**** 201810 - Calcolo Puntuale ****/
        public DataTable GetIdDichiarazioniAnno(int Anno, string IdEnte, string IdTributo, int IdContribuente,int IdDichiarazione, string ListOccupazioni, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            dtMyDati = new DataTable();
            try
            {
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                    cmdMyCommand.Connection.Open();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                //*** 20130801 - accertamento OSAP ***
                cmdMyCommand.Parameters.AddWithValue("@IdContribuente", IdContribuente);
                //*** ***
                /**** 201810 - Calcolo Puntuale ****/
                cmdMyCommand.Parameters.AddWithValue("@IDDICHIARAZIONE", IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@LISTOCCUPAZIONI", ListOccupazioni);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = "sp_GetDichiarazioniFromArticoliAnno";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

                return dtMyDati;
            }
            catch (Exception Err)
            {
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
				Log.Debug(IdEnte + " - OPENgovOSAP.DichiarazioniDAO.GetIdDichiarazioniAnno.errore: ", Err);
                throw (Err);
            }
        }

		public bool IsCartellaCreata(int IdDichiarazione)
		{
            //*** 20140904 
            //richiamo il metodo per la connessione
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@RowCount", 0);
                cmdMyCommand.Parameters["@RowCount"].Direction = ParameterDirection.InputOutput;
                Log.Debug("DichiarazioniDAO::IsCartellaCreata::esecuzione sp_IsCartellaCreataByIdDichiarazione");
                cmdMyCommand.CommandText = "sp_IsCartellaCreataByIdDichiarazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                int rowcount = StringOperation.FormatInt(cmdMyCommand.Parameters["@RowCount"].Value);

                if (rowcount == 0) return false;
                else return true;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.IsCartellaCreata.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            /*
			DBEngine dbEngine_;

			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();

			try
			{

				IDataParameterCollection parameterCollection_;
				IDataParameter parameter_;

				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdDichiarazione", IdDichiarazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@RowCount", 0, ParameterDirection.Output);
               
				parameterCollection_ = dbEngine_.ExecuteNonQuery("sp_IsCartellaCreataByIdDichiarazione", CommandType.StoredProcedure);
				parameter_ = (IDataParameter)parameterCollection_["@RowCount"];
				int rowcount = StringOperation.FormatInt(parameter_.Value;
					
				//Esiste almeno una cartella creata per la dichiarazione
				if (rowcount == 0) return false;
				else return true;
			
			}
             catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.IsCartellaCreata.errore: ", Err);
                throw (Err);
            }

			
			finally
			{
				dbEngine_.CloseConnection();
			}
              */
        }

        public void connessioneDB()
        {
            //*** 20140409
            try
            {
                Log.Debug("DichiarazioniDAO::connessioneDB::apertura della connessione al DB");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.connessioneDB.errore: ", Err);
                throw (Err);
            }

        }

        public void connessioneDB(string ConnectionString)
        {
            //*** 20140409
            try
            {
                Log.Debug("DichiarazioniDAO::connessioneDB::apertura della connessione al DB su " + ConnectionString);
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.connessioneDB.errore: ", Err);
                throw (Err);
            }

        }

        #region Private Method

        private DichiarazioneTosapCosap FillDichiarazione(string ConnectionString, DataTable dtDichiarazione, DataTable dtArticoli, bool IsForMotore)//, string CodTributo
        {
            DichiarazioneTosapCosap objDichiarazione = new DichiarazioneTosapCosap();
            try
            {
                DichiarazioneTosapCosapTestata objTestataDichiarazione = new DichiarazioneTosapCosapTestata();
                Articolo[] objArticoli = new Articolo[dtArticoli.Rows.Count];

                //Fill Testata Dichiarazione
                objTestataDichiarazione.DataDichiarazione = StringOperation.FormatDateTime(dtDichiarazione.Rows[0]["DATADICHIARAZIONE"]);
                objTestataDichiarazione.IdTipoAtto = StringOperation.FormatInt(dtDichiarazione.Rows[0]["TIPOATTO"]);
                objTestataDichiarazione.NDichiarazione = dtDichiarazione.Rows[0]["NDICHIARAZIONE"].ToString();
                objTestataDichiarazione.NoteDichiarazione = StringOperation.FormatString(dtDichiarazione.Rows[0]["NOTE"]);

                TitoloRichiedente objTitoloRichiedente = new TitoloRichiedente();
                objTitoloRichiedente.IdTitoloRichiedente = StringOperation.FormatInt(dtDichiarazione.Rows[0]["IDTITOLORICHIEDENTE"]);
                objTestataDichiarazione.TitoloRichiedente = objTitoloRichiedente;

                Uffici objUfficio = new Uffici();
                objUfficio.IdUfficio = StringOperation.FormatInt(dtDichiarazione.Rows[0]["IDUFFICIO"]);
                objTestataDichiarazione.Ufficio = objUfficio;

                //Fill Dati Dichiarazione
                objDichiarazione.IdDichiarazione = StringOperation.FormatInt(dtDichiarazione.Rows[0]["IDDICHIARAZIONE"]);
                objDichiarazione.IdEnte = dtDichiarazione.Rows[0]["IDENTE"].ToString();
                objDichiarazione.CodTributo = dtDichiarazione.Rows[0]["IDTRIBUTO"].ToString();
                objDichiarazione.TestataDichiarazione = objTestataDichiarazione;
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                objDichiarazione.ArticoliDichiarazione = DTO.MetodiArticolo.FillArticoloFromDataTable(ConnectionString, dtArticoli, objDichiarazione.IdEnte, objDichiarazione.CodTributo, -1, IsForMotore);
                //*** 20130801 - accertamento OSAP ***
                if (!IsForMotore)
                {
                    //Dati Anagrafici
                    DAO.AnagraficheDAO angraficaDAO = new AnagraficheDAO();
                    objDichiarazione.AnagraficaContribuente = angraficaDAO.GetAnagraficaContribuente(int.Parse(dtDichiarazione.Rows[0]["COD_CONTRIBUENTE"].ToString()));//,CodTributo);
                }
                else
                {
                    //Dati Anagrafici
                    objDichiarazione.AnagraficaContribuente = new DettaglioAnagrafica();
                    objDichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE = int.Parse(dtDichiarazione.Rows[0]["COD_CONTRIBUENTE"].ToString());
                }
                //*** ***
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.FillDichiarazione.errore: ", Err);

            }
            return objDichiarazione;
        }
		//*** 20130801 - accertamento OSAP ***
//		private DichiarazioneTosapCosap FillDichiarazioneForMotore(DataTable dtDichiarazione, DataTable dtArticoli, string CodTributo, ref DBEngine dbEngine_)
//		{
//			DichiarazioneTosapCosap objDichiarazione = new DichiarazioneTosapCosap();
//          try{
//			DichiarazioneTosapCosapTestata objTestataDichiarazione = new DichiarazioneTosapCosapTestata();
//			Articolo[] objArticoli = new Articolo[dtArticoli.Rows.Count];  
//			
//			//Fill Testata Dichiarazione
//			objTestataDichiarazione.DataDichiarazione = DateTime.Parse ( dtDichiarazione.Rows[0]["DATADICHIARAZIONE"].ToString()  );
//			objTestataDichiarazione.IdTipoAtto = StringOperation.FormatInt(dtDichiarazione.Rows[0]["TIPOATTO"];
//			objTestataDichiarazione.NDichiarazione = dtDichiarazione.Rows[0]["NDICHIARAZIONE"].ToString() ;
//			objTestataDichiarazione.NoteDichiarazione = StringOperation.FormatString(dtDichiarazione.Rows[0]["NOTE"]);
//				
//			TitoloRichiedente objTitoloRichiedente = new TitoloRichiedente(); 
//			objTitoloRichiedente.IdTitoloRichiedente = StringOperation.FormatInt(dtDichiarazione.Rows[0]["IDTITOLORICHIEDENTE"];
//			objTestataDichiarazione.TitoloRichiedente = objTitoloRichiedente;
//
//			Uffici objUfficio = new Uffici(); 
//			objUfficio.IdUfficio = StringOperation.FormatInt(dtDichiarazione.Rows[0]["IDUFFICIO"];
//			objTestataDichiarazione.Ufficio = objUfficio;
//		
//			//Fill Dati Dichiarazione
//			objDichiarazione.IdDichiarazione = StringOperation.FormatInt(dtDichiarazione.Rows[0]["IDDICHIARAZIONE"];
//			objDichiarazione.IdEnte = dtDichiarazione.Rows[0]["IDENTE"].ToString() ;
//			objDichiarazione.TestataDichiarazione = objTestataDichiarazione;
//			objDichiarazione.ArticoliDichiarazione = DTO.MetodiArticolo.FillArticoloFromDataTableForMotore(dtArticoli,objDichiarazione.IdEnte, ref dbEngine_);
//
//			//Dati Anagrafici
//			objDichiarazione.AnagraficaContribuente = new DettaglioAnagrafica ();
//			objDichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE = dtDichiarazione.Rows[0]["COD_CONTRIBUENTE"].ToString ();
//}
//catch (Exception Err)
          //  {
             //   Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.FillDichiarazioneForMotore.errore: ", Err);
                
            //}
    //			return objDichiarazione;
    //		}
    //*** ***
    #endregion


}

	public class ArticoliDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ArticoliDAO));
		public ArticoliDAO()
		{
		}

        //public int SetArticolo(ref DBEngine dbEngine_, Articolo oMyArticolo)
        //{
        //    IDataParameterCollection parameterCollection_;
        //    IDataParameter parameter_;

        //    try
        //    {
        //        /*********************
        //        **** Articoli Data ***
        //        **********************/
        //        //log.Debug("DAO::SetArticolo::IdArticoloPadre::"+oMyArticolo.IdArticoloPadre.ToString());
        //        SetDBEngineArticoli(StringOperation.FormatInt(OSAPConst.OperationType.ADD,ref dbEngine_, oMyArticolo);
        //        parameterCollection_ = dbEngine_.ExecuteNonQuery("sp_InsertArticolo", CommandType.StoredProcedure);
        //        parameter_ = (IDataParameter)parameterCollection_["@IdArticolo"];
        //        int nIdArticolo = StringOperation.FormatInt(parameter_.Value;

        //        for (int x = 0; x <= oMyArticolo.ListAgevolazioni.GetUpperBound(0); x++)
        //        {
        //            SetDBEngineArticoliVSAgevolazione(ref dbEngine_, nIdArticolo, oMyArticolo.ListAgevolazioni[x].IdAgevolazione);
        //            dbEngine_.ExecuteNonQuery("sp_InsertArticoloVSAgevolazione", CommandType.StoredProcedure);
        //        }
        //        return nIdArticolo;
        //    }
        //    catch (Exception Err)
        //    {
        //         Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ArticoliDAO.SetArticolo.errore: ", Err);
        //        return -1;
        //    }
        //}

        public int SetArticolo(ref SqlCommand cmdMyCommand, Articolo oMyArticolo)
        {
            //*** 20140409
            try
            {
                /*********************
                **** Articoli Data ***
                **********************/
                //log.Debug("DAO::SetArticolo::IdArticoloPadre::"+oMyArticolo.IdArticoloPadre.ToString());
                SetDBEngineArticoli((int)OSAPConst.OperationType.ADD, ref cmdMyCommand, oMyArticolo);
                cmdMyCommand.Parameters["@IdArticolo"].Direction = ParameterDirection.InputOutput;
                Log.Debug("DAO.ArticoliDAO::SetArticolo::esecuzione sp_InsertArticolo");
                cmdMyCommand.CommandText="sp_InsertArticolo";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                int nIdArticolo = StringOperation.FormatInt(cmdMyCommand.Parameters["@IdArticolo"].Value);
                if (oMyArticolo.ListAgevolazioni != null)
                {
                    foreach (Agevolazione myRow in oMyArticolo.ListAgevolazioni)
                    {
                        SetDBEngineArticoliVSAgevolazione(ref cmdMyCommand, nIdArticolo, myRow.IdAgevolazione);
                        Log.Debug("DAO.ArticoliDAO::SetArticolo::esecuzione sp_InsertArticoloVSAgevolazione");
                        cmdMyCommand.CommandText = "sp_InsertArticoloVSAgevolazione";
                        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                        cmdMyCommand.ExecuteNonQuery();
                    }
                }
                return nIdArticolo;
            }
            catch (Exception Err)
           {
              Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ArticoliDAO.SetArticolo.errore: ", Err);
              return -1;
            }
            }

        //public int UpdateArticolo(ref DBEngine dbEngine_, Articolo oMyArticolo)
        //{
        //    try
        //    {
        //        /*********************
        //        **** Articoli Data ***
        //        **********************/
        //        //log.Debug("DAO::UpdateArticolo::IdArticoloPadre::"+oMyArticolo.IdArticoloPadre.ToString());
        //        SetDBEngineArticoli(StringOperation.FormatInt(OSAPConst.OperationType.EDIT,ref dbEngine_, oMyArticolo);
        //        dbEngine_.ExecuteNonQuery("sp_UpdateArticolo", CommandType.StoredProcedure);
        //        return oMyArticolo.IdArticolo;
        //    }
        //    catch (Exception Err)
        //    {
        //         Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ArticoliDAO.UpdateArticolo.errore: ", Err);
        //        return -1;
        //    }
        //}

        public int UpdateArticolo(ref SqlCommand cmdMyCommand, Articolo oMyArticolo)
        {
            try
            {
                /*********************
                **** Articoli Data ***
                **********************/
                //log.Debug("DAO::UpdateArticolo::IdArticoloPadre::"+oMyArticolo.IdArticoloPadre.ToString());
                SetDBEngineArticoli((int)OSAPConst.OperationType.EDIT, ref cmdMyCommand, oMyArticolo);
                cmdMyCommand.CommandText = "sp_UpdateArticolo";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                return oMyArticolo.IdArticolo;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ArticoliDAO.UpdateArticolo.errore: ", Err);
                return -1;
            }
        }

        //private DBEngine SetDBEngineArticoli(int nOperationType,ref DBEngine dbEngine_, Articolo articolo)
        //{
        //    dbEngine_.ClearParameters();
        //    dbEngine_.AddParameter("@IdDichiarazione", articolo.IdDichiarazione, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@IdTributo", articolo.IdTributo, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@CodVia", articolo.CodVia, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Civico", SharedFunction.IntegerToDbNull(articolo.Civico), ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Esponente", SharedFunction.StringToDbNull(articolo.Esponente), ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Interno", SharedFunction.StringToDbNull(articolo.Interno), ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Scala", SharedFunction.StringToDbNull(articolo.Scala), ParameterDirection.Input);
        //    dbEngine_.AddParameter("@IdCategoria", articolo.Categoria.IdCategoria, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@IdTipologiaOccupazione", articolo.TipologiaOccupazione.IdTipologiaOccupazione, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Consistenza", articolo.Consistenza, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@IdTipoConsistenza", articolo.TipoConsistenzaTOCO.IdTipoConsistenza, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@DataInizioOccupazione", articolo.DataInizioOccupazione, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@DataFineOccupazione", articolo.DataFineOccupazione, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@IdDurata", articolo.TipoDurata.IdDurata, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@DurataOccupazione", articolo.DurataOccupazione, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@MaggiorazioneImporto", articolo.MaggiorazioneImporto, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@MaggiorazionePerc", articolo.MaggiorazionePerc, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Note", SharedFunction.StringToDbNull(articolo.Note), ParameterDirection.Input);
        //    dbEngine_.AddParameter("@DetrazioneImporto", articolo.DetrazioneImporto, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Attrazione", articolo.Attrazione, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@Operatore", articolo.Operatore, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@DataInserimento", articolo.DataInserimento, ParameterDirection.Input);
        //    if (articolo.DataVariazione!= DateTime.MinValue)
        //    {
        //        dbEngine_.AddParameter("@DataVariazione", articolo.DataVariazione, ParameterDirection.Input);
        //    }
        //    if (nOperationType==StringOperation.FormatInt(OSAPConst.OperationType.ADD)
        //    {
        //        dbEngine_.AddParameter("@IdArticolo", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);
        //    }
        //    else
        //    {
        //        dbEngine_.AddParameter("@IdArticolo", articolo.IdArticolo,  ParameterDirection.Input);
        //    }
        //    //*** 20130610 - ruolo supplettivo ***
        //    dbEngine_.AddParameter("@IdArticoloPadre", articolo.IdArticoloPadre,  ParameterDirection.Input);
        //    //*** ***
           
        //    return dbEngine_;
             
        //}

        private SqlCommand SetDBEngineArticoli(int nOperationType, ref SqlCommand cmdMyCommand, Articolo articolo)
        {
            try
            {
                //*** 20140409
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", articolo.IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", articolo.IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@CodVia", articolo.CodVia);
                cmdMyCommand.Parameters.AddWithValue("@Via", articolo.SVia);
                cmdMyCommand.Parameters.AddWithValue("@Civico", SharedFunction.IntegerToDbNull(articolo.Civico));
                cmdMyCommand.Parameters.AddWithValue("@Esponente", SharedFunction.StringToDbNull(articolo.Esponente));
                cmdMyCommand.Parameters.AddWithValue("@Interno", SharedFunction.StringToDbNull(articolo.Interno));
                cmdMyCommand.Parameters.AddWithValue("@Scala", SharedFunction.StringToDbNull(articolo.Scala));
                cmdMyCommand.Parameters.AddWithValue("@IdCategoria", articolo.Categoria.IdCategoria);
                cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", articolo.TipologiaOccupazione.IdTipologiaOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@Consistenza", articolo.Consistenza);
                cmdMyCommand.Parameters.AddWithValue("@IdTipoConsistenza", articolo.TipoConsistenzaTOCO.IdTipoConsistenza);
                cmdMyCommand.Parameters.AddWithValue("@DataInizioOccupazione", articolo.DataInizioOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@DataFineOccupazione", articolo.DataFineOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@IdDurata", articolo.TipoDurata.IdDurata);
                cmdMyCommand.Parameters.AddWithValue("@DurataOccupazione", articolo.DurataOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@MaggiorazioneImporto", articolo.MaggiorazioneImporto);
                cmdMyCommand.Parameters.AddWithValue("@MaggiorazionePerc", articolo.MaggiorazionePerc);
                cmdMyCommand.Parameters.AddWithValue("@Note", SharedFunction.StringToDbNull(articolo.Note));
                cmdMyCommand.Parameters.AddWithValue("@DetrazioneImporto", articolo.DetrazioneImporto);
                cmdMyCommand.Parameters.AddWithValue("@Attrazione", articolo.Attrazione);
                cmdMyCommand.Parameters.AddWithValue("@Operatore", articolo.Operatore);
                cmdMyCommand.Parameters.AddWithValue("@DataInserimento", articolo.DataInserimento);

                if (articolo.DataVariazione != DateTime.MinValue)
                {
                    cmdMyCommand.Parameters.AddWithValue("@DataVariazione", articolo.DataVariazione);
                }
                if (nOperationType == (int)OSAPConst.OperationType.ADD)
                {
                    cmdMyCommand.Parameters.AddWithValue("@IdArticolo", -1);
                }
                else
                {
                    cmdMyCommand.Parameters.AddWithValue("@IdArticolo", articolo.IdArticolo);
                }

                //*** 20130610 - ruolo supplettivo ***
                cmdMyCommand.Parameters.AddWithValue("@IdArticoloPadre", articolo.IdArticoloPadre);
                //*** ***

                return cmdMyCommand;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.SetDBEngineArticoli.errore: ", Err);
                throw Err;
            }
        }

		private SqlCommand SetDBEngineArticoliVSAgevolazione(ref SqlCommand cmdMyCommand, int nIdArticolo, int nIdAgevolazione)
		{
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdArticolo", nIdArticolo);
                cmdMyCommand.Parameters.AddWithValue("@IdRuolo", -1);
                cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", nIdAgevolazione);

                return cmdMyCommand;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniDAO.SetDBEngineArticoliVSAgevolazione.errore: ", Err);
                throw Err;
            }
		}

        //private DBEngine SetDBEngineArticoliVSAgevolazione(ref DBEngine dbEngine_, int nIdArticolo, int nIdAgevolazione)
        //{
        //    dbEngine_.ClearParameters();
        //    dbEngine_.AddParameter("@IdArticolo", nIdArticolo, ParameterDirection.Input);
        //    dbEngine_.AddParameter("@IdAgevolazione", nIdAgevolazione, ParameterDirection.Input);

        //    return dbEngine_;

        //}
	}
}
