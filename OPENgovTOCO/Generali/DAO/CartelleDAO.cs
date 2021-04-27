using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using log4net;
using DTO;
using IRemInterfaceOSAP;
using OPENgovTOCO;
using System.Web.UI;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione Cartelle
    /// </summary>
    public class CartelleDAO
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CartelleDAO));
        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;
        private SqlTransaction iniziaTransazione;
        /// <summary>
        /// 
        /// </summary>
        public CartelleDAO()
        {
        }


        /// <summary>
        /// Recupera la cartella dal suo codice
        /// </summary>
        /// <param name="codiceCartella"></param>
        /// <param name="codContribuente"></param>
        /// <param name="idEnte"></param>
        public DataTable GetCartellaByCodiceCartella(string codiceCartella, int codContribuente, string idEnte)
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", codiceCartella);
                cmdMyCommand.Parameters.AddWithValue("@CodContribuente", codContribuente);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", idEnte);
                Log.Debug("CartelleDAO::GetCartellaByCodiceCartella::esecuzione sp_GetCartellaByCodiceCartella @CodiceCartella='" + codiceCartella + "', @CodContribuente=" + codContribuente + ", @IdEnte='" + idEnte + "'");
                cmdMyCommand.CommandText = "sp_GetCartellaByCodiceCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetCartellaByCodiceCartella.errore: ", Err);
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
				string commandText_ = "sp_GetCartellaByCodiceCartella";
				dbEngine_.AddParameter("@CodiceCartella", codiceCartella, ParameterDirection.Input);
				dbEngine_.AddParameter("@CodContribuente", codContribuente, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", idEnte, ParameterDirection.Input);

				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetCartellaByCodiceCartella.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        //public DataTable CartelleSearch (CartellaSearch objSearch, DBEngine _dbEngineOut)
        //{
        //    DBEngine dbEngine_;
        //    DataTable dt = new DataTable();

        //    if (_dbEngineOut == null)
        //    {
        //        dbEngine_ = DBEngineFactory.GetDBEngine();
        //        dbEngine_.OpenConnection();
        //    }
        //    else
        //        dbEngine_ = _dbEngineOut;

        //    try
        //    {
        //        string tableName = String.Empty;

        //        if (objSearch.CodContribuenti!=null)
        //        {
        //            DAO.AnagraficheDAO daoAnag= new AnagraficheDAO(); 
        //            tableName = daoAnag.SetCodContribuentiTempTable(objSearch.CodContribuenti, dbEngine_);
        //        }

        //        string commandText_ = "sp_SearchCartelle";
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@CodiceCartella", objSearch.NumeroAvviso, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdEnte", objSearch.IdEnte, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@TempTableName", tableName, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Anno", objSearch.Anno, ParameterDirection.Input);
        //        //*** 20130610 - ruolo supplettivo ***
        //        dbEngine_.AddParameter("@IdFlusso", objSearch.IdFlusso, ParameterDirection.Input);
        //        //*** ***
        //        dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.CartellaSearch.errore: ", Err);
        //        throw (Err);
        //    }
        //    finally
        //    {
        //        if (_dbEngineOut == null)
        //            dbEngine_.CloseConnection();
        //    }
        //    return dt;
        //}
        public DataTable CartelleSearch(CartellaSearch objSearch, SqlCommand cmdMyCommandOut)
        {
            //*** 20140409
            dtMyDati = new DataTable();
            myDataReader = null;
            if (cmdMyCommandOut == null)
            {
                connessioneDB();
            }
            else
                cmdMyCommand = cmdMyCommandOut;

            try
            {
                string tableName = String.Empty;

                //if (objSearch.CodContribuenti != null)
                //{
                //    DAO.AnagraficheDAO daoAnag = new AnagraficheDAO();
                //    tableName = daoAnag.SetCodContribuentiTempTable(objSearch.CodContribuenti, cmdMyCommand);
                //}

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@Cognome", objSearch.CognomeContribuente);
                cmdMyCommand.Parameters.AddWithValue("@Nome", objSearch.NomeContribuente);
                cmdMyCommand.Parameters.AddWithValue("@CodFiscale", objSearch.CodFiscaleContribuente);
                cmdMyCommand.Parameters.AddWithValue("@PartitaIva", objSearch.PIVAContribuente);
                cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", objSearch.NumeroAvviso);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", objSearch.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", objSearch.IdTributo);
                //cmdMyCommand.Parameters.AddWithValue("@TempTableName", tableName);
                cmdMyCommand.Parameters.AddWithValue("@Anno", objSearch.Anno);
                //*** 20130610 - ruolo supplettivo ***
                cmdMyCommand.Parameters.AddWithValue("@IdFlusso", objSearch.IdFlusso);
                //*** ***
                cmdMyCommand.Parameters.AddWithValue("@IsSgravate", objSearch.IsSgravate);
                Log.Debug("CartelleDAO::CartelleSearch::esecuzione sp_SearchCartelle");
                cmdMyCommand.CommandText = "sp_SearchCartelle";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.CartellaSearch.errore: ", Err);
                throw (Err);
            }
            finally
            {
                if (cmdMyCommandOut == null)
                    cmdMyCommand.Connection.Close();
            }
            return dtMyDati;
        }


        public DataTable GetCartella(int IdCartella, string IdEnte)
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdCartella", IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                Log.Debug("CartelleDAO::GetCartella::esecuzione sp_GetCartella");
                cmdMyCommand.CommandText = "sp_GetCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetCartella.errore: ", Err);

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
				string commandText_ = "sp_GetCartella";
				dbEngine_.AddParameter("@IdCartella", IdCartella, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception ex)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetCartella.errore: ", Err);
				throw (ex);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        public DataTable GetPagamentiCartella(int IdCartella, string IdEnte)
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdCartella", IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                Log.Debug("CartelleDAO::GetPagamentiCartella::esecuzione sp_GetPagamentiCartella");
                cmdMyCommand.CommandText = "sp_GetPagamentiCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetPagamentiCartella.errore: ", Err);
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
				string commandText_ = "sp_GetPagamentiCartella";
				dbEngine_.AddParameter("@IdCartella", IdCartella, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetPagamentiCartella.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        public DataTable GetRuoliCartella(int IdCartella, string IdEnte)
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdCartella", IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                Log.Debug("CartelleDAO::GetRuoliCartella::esecuzione sp_GetRuoliCartella");
                cmdMyCommand.CommandText = "sp_GetRuoliCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetRuoliCartella.errore: ", Err);
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
				string commandText_ = "sp_GetRuoliCartella";
				dbEngine_.AddParameter("@IdCartella", IdCartella, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetRuoliCartella.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
             */
        }


        #region Calcolo Massivo

        public DataTable GetElaborazioneEffettuata(int Anno, Ruolo.E_TIPO TipoRuolo, string IdTributo)
        {
            //*** 20140409 
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                //*** 20130610 - ruolo supplettivo ***
                cmdMyCommand.Parameters.AddWithValue("@TipoRuolo", TipoRuolo);
                //*** ***
                cmdMyCommand.CommandText = "sp_GetElaborazioneEffettuata";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetElaborazioneEffettuata.errore: ", Err);
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
				string commandText_ = "sp_GetElaborazioneEffettuata";
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				//*** 20130610 - ruolo supplettivo ***
				dbEngine_.AddParameter("@TipoRuolo", TipoRuolo, ParameterDirection.Input);
				Log.Debug("CartelleDAO::GetElaborazioneEffettuata::query::sp_GetElaborazioneEffettuata::parametri::Anno::"+Anno.ToString()+"::IdEnte::"+DichiarazioneSession.IdEnte+"::TipoRuolo::"+TipoRuolo.ToString());
				//*** ***
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetElaborazioneEffettuata.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
             */
        }

        public DataTable GetStatistichePreElaborazione(int Anno, Ruolo.E_TIPO TipoRuolo, string IdTributo)
        {
            //*** 20140409 
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                //*** 20130610 - ruolo supplettivo ***
                cmdMyCommand.Parameters.AddWithValue("@TipoRuolo", TipoRuolo);
                //*** ***
                Log.Debug("CartelleDAO::GetStatistichePreElaborazione::esecuzione sp_GetStatistichePreElaborazione");
                cmdMyCommand.CommandText = "sp_GetStatistichePreElaborazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetStatistichePreElaborazione.errore: ", Err);
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
				string commandText_ = "sp_GetStatistichePreElaborazione";
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				//*** 20130610 - ruolo supplettivo ***
				dbEngine_.AddParameter("@TipoRuolo", TipoRuolo, ParameterDirection.Input);
				//*** ***
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetStatistichePreElaborazione.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        //*** 20130610 - ruolo supplettivo ***
        public DataTable CheckTariffa(int Anno, string IdTributo)
        {
            //*** 20140409 
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@TariffeMancanti", "");
                Log.Debug("CartelleDAO::CheckTariffa::esecuzione sp_CheckTariffeElaborazione");
                cmdMyCommand.CommandText = "sp_CheckTariffeElaborazione";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.CheckTariffa.errore: ", Err);
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
				string commandText_ = "sp_CheckTariffeElaborazione";
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@TariffeMancanti", "", ParameterDirection.Output);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.CheckTariffa.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}			
			return dt;
             */
        }

        //public void SetElaborazioneEffettuata (ElaborazioneEffettuata elab, ref DBEngine dbEngineOut)
        //{
        //    DBEngine dbEngine_ = null;
        //    try
        //    {
        //        // Questa funzione può essere richiamata sia in transazione
        //        // (calcolo massivo) sia indipendentemente (es: stampo minuta e
        //        // devo solo aggiornare lo stato), per cui gestisco il doppio caso
        //        // per la connessione al DB
        //        if (dbEngineOut != null)
        //            dbEngine_ = dbEngineOut;
        //        else
        //        {
        //            dbEngine_ = DBEngineFactory.GetDBEngine();
        //            dbEngine_.OpenConnection();
        //            dbEngine_.BeginTransaction();
        //        }

        //        string commandText_ = "sp_SetElaborazioneEffettuata";
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdEnte", elab.IdEnte, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Anno", elab.Anno, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraFineElaborazione", elab.DataOraFineElaborazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraInizioElaborazione", elab.DataOraInizioElaborazione, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraDocumentiStampati", elab.DataOraDocumentiStampati, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraDocumentiApprovati", elab.DataOraDocumentiApprovati, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@ImportoTotale", elab.ImportoTotale, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraMinutaApprovata", elab.DataOraMinutaApprovata, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraMinutaStampata", elab.DataOraMinutaStampata, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@NArticoli", elab.NArticoli, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@NDichiarazioni", elab.NDichiarazioni, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Note", elab.Note, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@NUtenti", elab.NUtenti, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataOraRateCalcolate", elab.DataOraCalcoloRate, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@SogliaMinimaRate", elab.SogliaMinimaRate, ParameterDirection.Input);
        //        //*** 20130610 - ruolo supplettivo ***
        //        dbEngine_.AddParameter("@TipoRuolo", elab.TipoRuolo, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdOrg", elab.IdFlusso, ParameterDirection.Input);
        //        Log.Debug("CartelleDAO::SetElaborazioneEffettuata::query::sp_SetElaborazioneEffettuata::parametri::IdEnte::"+ elab.IdEnte.ToString()+"::Anno::"+ elab.Anno.ToString()+"::DataOraFineElaborazione::"+ elab.DataOraFineElaborazione.ToString()+"::DataOraInizioElaborazione::"+ elab.DataOraInizioElaborazione.ToString()+"::DataOraDocumentiStampati::"+ elab.DataOraDocumentiStampati.ToString()+"::DataOraDocumentiApprovati::"+ elab.DataOraDocumentiApprovati.ToString()+"::ImportoTotale::"+ elab.ImportoTotale.ToString()+"::DataOraMinutaApprovata::"+ elab.DataOraMinutaApprovata.ToString()+"::DataOraMinutaStampata::"+ elab.DataOraMinutaStampata.ToString()+"::NArticoli::"+ elab.NArticoli.ToString()+"::NDichiarazioni::"+ elab.NDichiarazioni.ToString()+"::Note::"+ elab.Note.ToString()+"::NUtenti::"+ elab.NUtenti.ToString()+"::DataOraRateCalcolate::"+ elab.DataOraCalcoloRate.ToString()+"::SogliaMinimaRate::"+ elab.SogliaMinimaRate.ToString()+"::TipoRuolo::"+ elab.TipoRuolo.ToString()+"::IdOrg::"+ elab.IdFlusso.ToString());
        //        //*** ***
        //        dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

        //        if (dbEngineOut == null)
        //            dbEngine_.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (dbEngineOut == null)
        //            dbEngine_.RollbackTransaction(); 
        //        Log.Debug("CartelleDAO::SetElaborazioneEffettuata::si è verificato il seguente errore::" + ex.Message +"::query::sp_SetElaborazioneEffettuata::parametri::IdEnte::"+ elab.IdEnte.ToString()+"::Anno::"+ elab.Anno.ToString()+"::DataOraFineElaborazione::"+ elab.DataOraFineElaborazione.ToString()+"::DataOraInizioElaborazione::"+ elab.DataOraInizioElaborazione.ToString()+"::DataOraDocumentiStampati::"+ elab.DataOraDocumentiStampati.ToString()+"::DataOraDocumentiApprovati::"+ elab.DataOraDocumentiApprovati.ToString()+"::ImportoTotale::"+ elab.ImportoTotale.ToString()+"::DataOraMinutaApprovata::"+ elab.DataOraMinutaApprovata.ToString()+"::DataOraMinutaStampata::"+ elab.DataOraMinutaStampata.ToString()+"::NArticoli::"+ elab.NArticoli.ToString()+"::NDichiarazioni::"+ elab.NDichiarazioni.ToString()+"::Note::"+ elab.Note.ToString()+"::NUtenti::"+ elab.NUtenti.ToString()+"::DataOraRateCalcolate::"+ elab.DataOraCalcoloRate.ToString()+"::SogliaMinimaRate::"+ elab.SogliaMinimaRate.ToString()+"::TipoRuolo::"+ elab.TipoRuolo.ToString()+"::IdOrg::"+ elab.IdFlusso.ToString());
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (dbEngineOut == null)
        //            dbEngine_.CloseConnection();
        //    }
        //}

        /// <summary>
        /// Questa funzione può essere richiamata sia in transazione (calcolo massivo) sia indipendentemente (es: stampo minuta e devo solo aggiornare lo stato), per cui gestisco il doppio caso per la connessione al DB
        /// </summary>
        /// <param name="myItem">ref ElaborazioneEffettuata oggetto da gestire</param>
        /// <param name="cmdMyCommandOut">ref SqlCommand</param>
        /// <param name="Operatore">string utente</param>
        /// <revisionHistory>
        /// <revision date="10/60/2013">
        /// <strong>ruolo supplettivo</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="30/04/2014">
        /// <strong>modificata la connessione della transazione</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="06/2018">
        /// <strong>Possibilità di calcolare ruolo solo per alcune tipologie di occupazioni</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public void SetElaborazioneEffettuata(ref ElaborazioneEffettuata myItem, ref SqlCommand cmdMyCommandOut,string Operatore)
        {
            connessioneDB();
            iniziaTransazione = cmdMyCommand.Connection.BeginTransaction();
            try
            {
                if (cmdMyCommandOut != null)
                    cmdMyCommand = cmdMyCommandOut;
                else
                {
                    cmdMyCommand.Transaction = iniziaTransazione;
                }

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", myItem.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", myItem.IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@Anno", myItem.Anno);
                cmdMyCommand.Parameters.AddWithValue("@DataOraFineElaborazione", myItem.DataOraFineElaborazione);
                cmdMyCommand.Parameters.AddWithValue("@DataOraInizioElaborazione", myItem.DataOraInizioElaborazione);
                cmdMyCommand.Parameters.AddWithValue("@DataOraDocumentiStampati", myItem.DataOraDocumentiStampati);
                cmdMyCommand.Parameters.AddWithValue("@DataOraDocumentiApprovati", myItem.DataOraDocumentiApprovati);
                cmdMyCommand.Parameters.AddWithValue("@ImportoTotale", myItem.ImportoTotale);
                cmdMyCommand.Parameters.AddWithValue("@DataOraMinutaApprovata", myItem.DataOraMinutaApprovata);
                cmdMyCommand.Parameters.AddWithValue("@DataOraMinutaStampata", myItem.DataOraMinutaStampata);
                cmdMyCommand.Parameters.AddWithValue("@NArticoli", myItem.NArticoli);
                cmdMyCommand.Parameters.AddWithValue("@NDichiarazioni", myItem.NDichiarazioni);
                cmdMyCommand.Parameters.AddWithValue("@Note", myItem.Note);
                cmdMyCommand.Parameters.AddWithValue("@NUtenti", myItem.NUtenti);
                cmdMyCommand.Parameters.AddWithValue("@DataOraRateCalcolate", myItem.DataOraCalcoloRate);
                cmdMyCommand.Parameters.AddWithValue("@SogliaMinimaRate", myItem.SogliaMinimaRate);
                cmdMyCommand.Parameters.AddWithValue("@TipoRuolo", myItem.TipoRuolo);
                cmdMyCommand.Parameters.AddWithValue("@IdOrg", myItem.IdFlusso);
                cmdMyCommand.Parameters.AddWithValue("@ListOccupazioni", myItem.ListOccupazioni);
                cmdMyCommand.Parameters.AddWithValue("@Id", myItem.IdFlusso);
                cmdMyCommand.Parameters.AddWithValue("@Operatore", Operatore);
                cmdMyCommand.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                cmdMyCommand.CommandText = "sp_SetElaborazioneEffettuata";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                int parameter_ = (int)cmdMyCommand.Parameters["@Id"].Value;
                myItem.IdFlusso = parameter_;

                if (cmdMyCommandOut == null)
                    iniziaTransazione.Commit();
            }
            catch (Exception ex)
            {
                if (cmdMyCommandOut == null)
                    iniziaTransazione.Rollback();
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
             Log.Debug(myItem.IdEnte + " - OPENgovOSAP.CartellaDAO.SetElaborazioneEffettutata.errore: ", ex);
                throw (ex);
            }
            finally
            {
                if (cmdMyCommandOut == null)
                    cmdMyCommand.Connection.Close();
            }
        }
        //public void SetElaborazioneEffettuata(ref ElaborazioneEffettuata myItem, ref SqlCommand cmdMyCommandOut)
        //{
        //    connessioneDB();
        //    iniziaTransazione = cmdMyCommand.Connection.BeginTransaction();
        //    try
        //    {
        //        if (cmdMyCommandOut != null)
        //            cmdMyCommand = cmdMyCommandOut;
        //        else
        //        {
        //            cmdMyCommand.Transaction = iniziaTransazione;
        //        }

        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.AddWithValue("@IdEnte", myItem.IdEnte);
        //        cmdMyCommand.Parameters.AddWithValue("@IdTributo", myItem.IdTributo);
        //        cmdMyCommand.Parameters.AddWithValue("@Anno", myItem.Anno);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraFineElaborazione", myItem.DataOraFineElaborazione);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraInizioElaborazione", myItem.DataOraInizioElaborazione);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraDocumentiStampati", myItem.DataOraDocumentiStampati);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraDocumentiApprovati", myItem.DataOraDocumentiApprovati);
        //        cmdMyCommand.Parameters.AddWithValue("@ImportoTotale", myItem.ImportoTotale);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraMinutaApprovata", myItem.DataOraMinutaApprovata);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraMinutaStampata", myItem.DataOraMinutaStampata);
        //        cmdMyCommand.Parameters.AddWithValue("@NArticoli", myItem.NArticoli);
        //        cmdMyCommand.Parameters.AddWithValue("@NDichiarazioni", myItem.NDichiarazioni);
        //        cmdMyCommand.Parameters.AddWithValue("@Note", myItem.Note);
        //        cmdMyCommand.Parameters.AddWithValue("@NUtenti", myItem.NUtenti);
        //        cmdMyCommand.Parameters.AddWithValue("@DataOraRateCalcolate", myItem.DataOraCalcoloRate);
        //        cmdMyCommand.Parameters.AddWithValue("@SogliaMinimaRate", myItem.SogliaMinimaRate);
        //        cmdMyCommand.Parameters.AddWithValue("@TipoRuolo", myItem.TipoRuolo);
        //        cmdMyCommand.Parameters.AddWithValue("@IdOrg", myItem.IdFlusso);
        //        cmdMyCommand.Parameters.AddWithValue("@ListOccupazioni", myItem.ListOccupazioni);
        //        cmdMyCommand.Parameters.AddWithValue("@Id", myItem.IdFlusso);
        //        cmdMyCommand.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
        //        cmdMyCommand.CommandText = "sp_SetElaborazioneEffettuata";
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //        cmdMyCommand.ExecuteNonQuery();
        //        int parameter_ = (int)cmdMyCommand.Parameters["@Id"].Value;
        //        myItem.IdFlusso = parameter_;

        //        if (cmdMyCommandOut == null)
        //            iniziaTransazione.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (cmdMyCommandOut == null)
        //            iniziaTransazione.Rollback();
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //        Log.Debug(myItem.IdEnte + " - OPENgovOSAP.CartellaDAO.SetElaborazioneEffettutata.errore: ", ex);
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (cmdMyCommandOut == null)
        //            cmdMyCommand.Connection.Close();
        //    }
        //}

        /// <summary>
        /// Funzione per l'inserimento dell'avviso
        /// </summary>
        /// <param name="myItem">ref Cartella oggetto da gestire</param>
        /// <param name="cmdMyCommand">ref SqlCommand</param>
        /// <param name="HasArrotondamento">bool</param>
        /// <param name="IsSgravio">int</param>
        /// <param name="sOperatore">string</param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public void InsertCartella(ref Cartella myItem, ref SqlCommand cmdMyCommand, bool HasArrotondamento, int IsSgravio, string sOperatore)
        {
            cmdMyCommand.CommandTimeout = 0;
            string commandText_ = string.Empty;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", myItem.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Anno", myItem.Anno);
                cmdMyCommand.Parameters.AddWithValue("@CodContribuente", myItem.CodContribuente);
                cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", myItem.Dichiarazione.IdDichiarazione);
                cmdMyCommand.Parameters.AddWithValue("@NAvviso", myItem.CodiceCartella);
                cmdMyCommand.Parameters.AddWithValue("@DataEmissione", myItem.DataEmissione);
                cmdMyCommand.Parameters.AddWithValue("@ImportoTotale", myItem.ImportoTotale);
                //*** 20130123 - devo inserire anche idflusso_ruolo ***
                cmdMyCommand.Parameters.AddWithValue("@IdFlussoRuolo", myItem.IdFlussoRuolo);
                //*** ***
                cmdMyCommand.Parameters.AddWithValue("@HasArrotondamento", HasArrotondamento);
                cmdMyCommand.Parameters.AddWithValue("@ISSGRAVIO", IsSgravio);
                cmdMyCommand.Parameters.AddWithValue("@IdCartella", myItem.IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", sOperatore);
                cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", DateTime.Now);
                cmdMyCommand.Parameters["@IdCartella"].Direction = ParameterDirection.InputOutput;

                cmdMyCommand.CommandText = "sp_InsertCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                int parameter_ = (int)cmdMyCommand.Parameters["@IdCartella"].Value;
                myItem.IdCartella = parameter_;

                cmdMyCommand.CommandTimeout = 0;
                Log.Debug("CartelleDAO::InsertCartella::per la cartella::" + myItem.IdCartella.ToString() + " ho n." + myItem.Ruoli.Length.ToString() + " articoli");

                commandText_ = "sp_InsertRuolo";
                foreach (Ruolo myRuolo in  myItem.Ruoli)
                {
                    Log.Debug("controllo se ho articolo");
                    if (myRuolo != null)
                    {
                        Log.Debug("nuova cartella");
                        myRuolo.CartellaTOCO = new Cartella();
                        myRuolo.CartellaTOCO.IdCartella = myItem.IdCartella;
                        Log.Debug("assegnato id cartella istanzio parametri");
                        cmdMyCommand.Parameters.Clear();
                        cmdMyCommand.Parameters.AddWithValue("@IdCartella", myRuolo.CartellaTOCO.IdCartella);
                        cmdMyCommand.Parameters.AddWithValue("@IdArticolo", myRuolo.ArticoloTOCO.IdArticolo);
                        cmdMyCommand.Parameters.AddWithValue("@TariffaApplicata", myRuolo.Tariffa.Valore);
                        cmdMyCommand.Parameters.AddWithValue("@ImportoLordo", myRuolo.ImportoLordo);
                        cmdMyCommand.Parameters.AddWithValue("@Importo", myRuolo.Importo);
                        cmdMyCommand.Parameters.AddWithValue("@CODVIA", myRuolo.ArticoloTOCO.CodVia);
                        cmdMyCommand.Parameters.AddWithValue("@VIA", myRuolo.ArticoloTOCO.SVia);
                        cmdMyCommand.Parameters.AddWithValue("@CIVICO", myRuolo.ArticoloTOCO.Civico);
                        cmdMyCommand.Parameters.AddWithValue("@IDCATEGORIA", myRuolo.ArticoloTOCO.Categoria.IdCategoria);
                        cmdMyCommand.Parameters.AddWithValue("@IDTIPOLOGIAOCCUPAZIONE", myRuolo.ArticoloTOCO.TipologiaOccupazione.IdTipologiaOccupazione);
                        cmdMyCommand.Parameters.AddWithValue("@CONSISTENZA", myRuolo.ArticoloTOCO.Consistenza);
                        cmdMyCommand.Parameters.AddWithValue("@IDTIPOCONSISTENZA", myRuolo.ArticoloTOCO.TipoConsistenzaTOCO.IdTipoConsistenza);
                        cmdMyCommand.Parameters.AddWithValue("@IDDURATA", myRuolo.ArticoloTOCO.TipoDurata.IdDurata);
                        cmdMyCommand.Parameters.AddWithValue("@DURATAOCCUPAZIONE", myRuolo.ArticoloTOCO.DurataOccupazione);
                        cmdMyCommand.Parameters.AddWithValue("@MAGGIORAZIONE_IMPORTO", myRuolo.ArticoloTOCO.MaggiorazioneImporto);
                        cmdMyCommand.Parameters.AddWithValue("@MAGGIORAZIONE_PERC", myRuolo.ArticoloTOCO.MaggiorazionePerc);
                        cmdMyCommand.Parameters.AddWithValue("@DETRAZIONE_IMPORTO", myRuolo.ArticoloTOCO.DetrazioneImporto);
                        cmdMyCommand.Parameters.AddWithValue("@ATTRAZIONE", myRuolo.ArticoloTOCO.Attrazione);
                        cmdMyCommand.Parameters.AddWithValue("@OPERATORE", sOperatore);
                        cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", DateTime.Now);
                        //Log.Debug("CartelleDAO::InsertRuolo::IdCartella::" + myRuolo.CartellaTOCO.IdCartella.ToString());
                        //Log.Debug("CartelleDAO::InsertRuolo::IdArticolo::" + myRuolo.ArticoloTOCO.IdArticolo.ToString());
                        //Log.Debug("CartelleDAO::InsertRuolo::ImportoLordo::" + myRuolo.ImportoLordo.ToString());
                        //Log.Debug("CartelleDAO::InsertRuolo::Tariffa::" + myRuolo.Tariffa.Valore.ToString());
                        //Log.Debug("CartelleDAO::InsertRuolo::Importo::" + myRuolo.Importo.ToString());
                        cmdMyCommand.Parameters.AddWithValue("@IdRuolo", -1);
                        cmdMyCommand.Parameters["@IdRuolo"].Direction = ParameterDirection.Output;
                        Log.Debug("CartelleDAO::InsertCartella::esecuzione sp_InsertRuolo");
                        cmdMyCommand.CommandText = commandText_;
                        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                        cmdMyCommand.ExecuteNonQuery();
                        int parameter2_ = (int)cmdMyCommand.Parameters["@IdRuolo"].Value;
                        myRuolo.IdRuolo = parameter2_;
                        foreach(Agevolazione myAgev in myRuolo.ArticoloTOCO.ListAgevolazioni)
                        {
                            cmdMyCommand.Parameters.Clear();
                            cmdMyCommand.Parameters.AddWithValue("@IdArticolo", -1);
                            cmdMyCommand.Parameters.AddWithValue("@IdRuolo", myRuolo.IdRuolo);
                            cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", myAgev.IdAgevolazione);
                            cmdMyCommand.CommandText = "sp_InsertArticoloVSAgevolazione";
                            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                            cmdMyCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("CartelleDAO::InsertCartella::si è verificato il seguente errore::" + ex.Message);
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                Log.Debug(myItem.IdEnte + " - OPENgovOSAP.CartellaDAO.InsertCartella.errore: ", ex);
                throw (ex);
            }
        }
        //public void InsertCartella(ref Cartella myItem, ref SqlCommand cmdMyCommand, bool HasArrotondamento, int IsSgravio, string sOperatore)
        //{
        //    cmdMyCommand.CommandTimeout = 0;
        //    string commandText_ = string.Empty;
        //    try
        //    {
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.AddWithValue("@IdEnte", myItem.IdEnte);
        //        cmdMyCommand.Parameters.AddWithValue("@Anno", myItem.Anno);
        //        cmdMyCommand.Parameters.AddWithValue("@CodContribuente", myItem.CodContribuente);
        //        cmdMyCommand.Parameters.AddWithValue("@IdDichiarazione", myItem.Dichiarazione.IdDichiarazione);
        //        cmdMyCommand.Parameters.AddWithValue("@NAvviso", myItem.CodiceCartella);
        //        cmdMyCommand.Parameters.AddWithValue("@DataEmissione", myItem.DataEmissione);
        //        cmdMyCommand.Parameters.AddWithValue("@ImportoTotale", myItem.ImportoTotale);
        //        //*** 20130123 - devo inserire anche idflusso_ruolo ***
        //        cmdMyCommand.Parameters.AddWithValue("@IdFlussoRuolo", myItem.IdFlussoRuolo);
        //        //*** ***
        //        cmdMyCommand.Parameters.AddWithValue("@HasArrotondamento", HasArrotondamento);
        //        cmdMyCommand.Parameters.AddWithValue("@ISSGRAVIO", IsSgravio);
        //        cmdMyCommand.Parameters.AddWithValue("@IdCartella", myItem.IdCartella);
        //        cmdMyCommand.Parameters["@IdCartella"].Direction = ParameterDirection.InputOutput;

        //        cmdMyCommand.CommandText = "sp_InsertCartella";
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //        cmdMyCommand.ExecuteNonQuery();
        //        int parameter_ = (int)cmdMyCommand.Parameters["@IdCartella"].Value;
        //        myItem.IdCartella = parameter_;

        //        cmdMyCommand.CommandTimeout = 0;
        //        Log.Debug("CartelleDAO::InsertCartella::per la cartella::" + myItem.IdCartella.ToString() + " ho n." + myItem.Ruoli.Length.ToString() + " articoli");

        //        commandText_ = "sp_InsertRuolo";
        //        foreach (Ruolo myRuolo in myItem.Ruoli)
        //        {
        //            Log.Debug("controllo se ho articolo");
        //            if (myRuolo != null)
        //            {
        //                Log.Debug("nuova cartella");
        //                myRuolo.CartellaTOCO = new Cartella();
        //                myRuolo.CartellaTOCO.IdCartella = myItem.IdCartella;
        //                Log.Debug("assegnato id cartella istanzio parametri");
        //                cmdMyCommand.Parameters.Clear();
        //                cmdMyCommand.Parameters.AddWithValue("@IdCartella", myRuolo.CartellaTOCO.IdCartella);
        //                cmdMyCommand.Parameters.AddWithValue("@IdArticolo", myRuolo.ArticoloTOCO.IdArticolo);
        //                cmdMyCommand.Parameters.AddWithValue("@TariffaApplicata", myRuolo.Tariffa.Valore);
        //                cmdMyCommand.Parameters.AddWithValue("@ImportoLordo", myRuolo.ImportoLordo);
        //                cmdMyCommand.Parameters.AddWithValue("@Importo", myRuolo.Importo);
        //                cmdMyCommand.Parameters.AddWithValue("@CODVIA", myRuolo.ArticoloTOCO.CodVia);
        //                cmdMyCommand.Parameters.AddWithValue("@VIA", myRuolo.ArticoloTOCO.SVia);
        //                cmdMyCommand.Parameters.AddWithValue("@CIVICO", myRuolo.ArticoloTOCO.Civico);
        //                cmdMyCommand.Parameters.AddWithValue("@IDCATEGORIA", myRuolo.ArticoloTOCO.Categoria.IdCategoria);
        //                cmdMyCommand.Parameters.AddWithValue("@IDTIPOLOGIAOCCUPAZIONE", myRuolo.ArticoloTOCO.TipologiaOccupazione.IdTipologiaOccupazione);
        //                cmdMyCommand.Parameters.AddWithValue("@CONSISTENZA", myRuolo.ArticoloTOCO.Consistenza);
        //                cmdMyCommand.Parameters.AddWithValue("@IDTIPOCONSISTENZA", myRuolo.ArticoloTOCO.TipoConsistenzaTOCO.IdTipoConsistenza);
        //                cmdMyCommand.Parameters.AddWithValue("@IDDURATA", myRuolo.ArticoloTOCO.TipoDurata.IdDurata);
        //                cmdMyCommand.Parameters.AddWithValue("@DURATAOCCUPAZIONE", myRuolo.ArticoloTOCO.DurataOccupazione);
        //                cmdMyCommand.Parameters.AddWithValue("@MAGGIORAZIONE_IMPORTO", myRuolo.ArticoloTOCO.MaggiorazioneImporto);
        //                cmdMyCommand.Parameters.AddWithValue("@MAGGIORAZIONE_PERC", myRuolo.ArticoloTOCO.MaggiorazionePerc);
        //                cmdMyCommand.Parameters.AddWithValue("@DETRAZIONE_IMPORTO", myRuolo.ArticoloTOCO.DetrazioneImporto);
        //                cmdMyCommand.Parameters.AddWithValue("@ATTRAZIONE", myRuolo.ArticoloTOCO.Attrazione);
        //                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", sOperatore);
        //                cmdMyCommand.Parameters.AddWithValue("@DATA_INSERIMENTO", DateTime.Now);
        //                //Log.Debug("CartelleDAO::InsertRuolo::IdCartella::" + myRuolo.CartellaTOCO.IdCartella.ToString());
        //                //Log.Debug("CartelleDAO::InsertRuolo::IdArticolo::" + myRuolo.ArticoloTOCO.IdArticolo.ToString());
        //                //Log.Debug("CartelleDAO::InsertRuolo::ImportoLordo::" + myRuolo.ImportoLordo.ToString());
        //                //Log.Debug("CartelleDAO::InsertRuolo::Tariffa::" + myRuolo.Tariffa.Valore.ToString());
        //                //Log.Debug("CartelleDAO::InsertRuolo::Importo::" + myRuolo.Importo.ToString());
        //                cmdMyCommand.Parameters.AddWithValue("@IdRuolo", -1);
        //                cmdMyCommand.Parameters["@IdRuolo"].Direction = ParameterDirection.Output;
        //                Log.Debug("CartelleDAO::InsertCartella::esecuzione sp_InsertRuolo");
        //                cmdMyCommand.CommandText = commandText_;
        //                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //                cmdMyCommand.ExecuteNonQuery();
        //                int parameter2_ = (int)cmdMyCommand.Parameters["@IdRuolo"].Value;
        //                myRuolo.IdRuolo = parameter2_;
        //                foreach (Agevolazione myAgev in myRuolo.ArticoloTOCO.ListAgevolazioni)
        //                {
        //                    cmdMyCommand.Parameters.Clear();
        //                    cmdMyCommand.Parameters.AddWithValue("@IdArticolo", -1);
        //                    cmdMyCommand.Parameters.AddWithValue("@IdRuolo", myRuolo.IdRuolo);
        //                    cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", myAgev.IdAgevolazione);
        //                    cmdMyCommand.CommandText = "sp_InsertArticoloVSAgevolazione";
        //                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //                    cmdMyCommand.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("CartelleDAO::InsertCartella::si è verificato il seguente errore::" + ex.Message);
        //        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //        Log.Debug(myItem.IdEnte + " - OPENgovOSAP.CartellaDAO.InsertCartella.errore: ", ex);
        //        throw (ex);
        //    }
        //}
        public string GetNAvvisoAutomatico(string ConnectionString, string IdEnte, string CodTributo, string Anno)
        {
            string sRet = "";
            connessioneDB(ConnectionString);
            try
            {
                DataTable dtDichiarazione = new DataTable();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@CodTributo", CodTributo);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.CommandText = "prc_GetNAvvisoAutomatico";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
                foreach (DataRow myRow in dtMyDati.Rows)
                {
                    sRet = myRow["navviso"].ToString();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetNAvvisoAutomatico.errore: ", Err);
                throw (Err);
            }
            finally
            {
                myDataReader.Close();
                cmdMyCommand.Connection.Close();
            }
            return sRet;
        }
        public void UndoSgravio(ref Cartella myItem, ref SqlCommand cmdMyCommand, string sOperatore)
        {
            cmdMyCommand.CommandTimeout = 0;
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IDCARTELLA", myItem.IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@CODICE_CARTELLA", myItem.CodiceCartella);
                cmdMyCommand.Parameters["@IdCartella"].Direction = ParameterDirection.InputOutput;
                cmdMyCommand.CommandText = "prc_UndoSgravio";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                int ValRet = (int)cmdMyCommand.Parameters["@IdCartella"].Value;
                if (ValRet < 0)
                    throw new Exception("Errore in annullamento sgravio.");
                myItem.IdCartella = ValRet;
            }
            catch (Exception ex)
            {
                Log.Debug("CartelleDAO::UndoSgravio::si è verificato il seguente errore::" + ex.Message + "::query::prc_UndoSgravio::parametri::::IdCartella=" + myItem.IdCartella.ToString());
                Log.Debug(myItem.IdEnte + " - OPENgovOSAP.CartellaDAO.UndoSgravio.errore: ", ex);
                throw (ex);
            }
        }
        //public void SetCodiceCartella (Cartella cart, ref DBEngine dbEngine_)
        //{
        //    try
        //    {
        //        string commandText_ = "sp_SetCodiceCartella";
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdCartella", cart.IdCartella, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@CodiceCartella", cart.CodiceCartella, ParameterDirection.Input);

        //        dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.SetCodiceCartella.errore: ", Err);
        //        throw (Err);
        //    }
        //}

        public void SetCodiceCartella(Cartella cart, ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdCartella", cart.IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", cart.CodiceCartella);
                Log.Debug("CartelleDAO::SetCodiceCartella::esecuzione sp_SetCodiceCartella");
                cmdMyCommand.CommandText = "sp_SetCodiceCartella";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.SetCodiceCartella.errore: ", Err);
                throw (Err);
            }
        }

        //public DBEngine StartCalcoloMassivoTransaction ()
        //{
        //    DBEngine dbEngine_;
        //    dbEngine_ = DBEngineFactory.GetDBEngine();
        //    dbEngine_.OpenConnection();
        //    dbEngine_.BeginTransaction ();
        //    return dbEngine_;
        //}

        public SqlCommand StartCalcoloMassivoTransaction()
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                connessioneDB();
                conn = cmdMyCommand.Connection;
                SqlTransaction iniziaTransazione = conn.BeginTransaction();
                cmdMyCommand.Transaction = iniziaTransazione;
                return cmdMyCommand;
            }
            catch (Exception Err)
            {
                Log.Debug( " - OPENgovOSAP.CartellaDAO.StartCalcoloMassivoTransaction.errore: ", Err);
                throw (Err);
            }
        }

        //public DBEngine OpenCalcoloMassivoConnection ()
        //{
        //    DBEngine dbEngine_;
        //    dbEngine_ = DBEngineFactory.GetDBEngine();
        //    dbEngine_.OpenConnection();
        //    return dbEngine_;
        //}

        public SqlCommand OpenCalcoloMassivoConnection()
        {
            try
            {
                connessioneDB();
                return cmdMyCommand;
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgovOSAP.CartellaDAO.OpenCalcoloMassivoConncetion.errore: ", Err);
                throw (Err);
            }
        }

        //public void CommitCalcoloMassivoTransaction (ref DBEngine dbEngine_)
        //{
        //    dbEngine_.CommitTransaction ();
        //    dbEngine_.CloseConnection();
        //}

        public void CommitCalcoloMassivoTransaction(ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Transaction.Commit();
                cmdMyCommand.Connection.Close();
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgovOSAP.CartellaDAO.CommitCalcoloMassivoTransaction.errore: ", Err);
                throw (Err);
            }
        }

        //public void RollbackCalcoloMassivoTransaction (ref DBEngine dbEngine_)
        //{
        //    dbEngine_.RollbackTransaction ();
        //    dbEngine_.CloseConnection();
        //}

        public void RollbackCalcoloMassivoTransaction(ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Transaction.Rollback();
                cmdMyCommand.Connection.Close();
            }
            catch (Exception Err)
            {
                Log.Debug( " - OPENgovOSAP.CartellaDAO.RollbackCalcoloMassivoTransaction.errore: ", Err);
                throw (Err);
            }

        }

        //*** 20130610 - ruolo supplettivo ***
        // Elimina completamente un calcolo massivo effettuato
        //public void DeleteCalcoloMassivo (int Anno, string IdEnte,Ruolo.E_TIPO TipoRuolo)
        public void DeleteCalcoloMassivo(int IdFlusso)
        {
            //*** 20140409
            try
            {
                connessioneDB();
                //conn = new SqlConnection();
                //SqlTransaction iniziaTransazione = conn.BeginTransaction();
                //iniziaTransazione = conn.BeginTransaction();
                //cmdMyCommand.Transaction = iniziaTransazione;

                iniziaTransazione = cmdMyCommand.Connection.BeginTransaction();
                cmdMyCommand.Transaction = iniziaTransazione;

                cmdMyCommand.Parameters.Clear();
                //*** 20130610 - ruolo supplettivo ***
                //dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
                //dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
                cmdMyCommand.Parameters.AddWithValue("@IdFlusso", IdFlusso);
                //*** ***
                Log.Debug("CartelleDAO::DeleteCalcoloMassivo::esecuzione sp_EliminaCalcoloMassivo");
                cmdMyCommand.CommandText = "sp_EliminaCalcoloMassivo";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();

                //cmdMyCommand.Transaction.Commit();
                iniziaTransazione.Commit();
            }
            catch (Exception Err)
            {
                //cmdMyCommand.Transaction.Rollback();
                iniziaTransazione.Rollback();
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.DeleteCalcoloMassivo.errore: ", Err);
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
				dbEngine_ = DBEngineFactory.GetDBEngine();
				dbEngine_.OpenConnection();
				dbEngine_.BeginTransaction();

				string commandText_ = "sp_EliminaCalcoloMassivo";
				dbEngine_.ClearParameters();
				//*** 20130610 - ruolo supplettivo ***
				//dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				//dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdFlusso", IdFlusso, ParameterDirection.Input);
				//*** ***
				dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
				
				dbEngine_.CommitTransaction();
			}
			catch (Exception ex)
			{
				dbEngine_.RollbackTransaction(); 
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.DeleteCalcoloMassivo.errore: ", Err);
				throw (ex);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
            */
        }

        //public void InsertRata (ref Rata r, ref DBEngine dbEngine_)
        //{
        //    try
        //    {
        //        IDataParameterCollection parameterCollection_;
        //        IDataParameter parameter_;

        //        string commandText_ = "sp_InsertRata";
        //        dbEngine_.ClearParameters();

        //        dbEngine_.AddParameter("@IdCartella", r.IdCartella, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@NumeroRata", r.NumeroRata, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DescrizioneRata", r.DescrizioneRata, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@ImportoRata", r.ImportoRata, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@DataScadenza", r.DataScadenza, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@CodiceBollettino", r.CodiceBollettino, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Codeline", r.Codeline, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@NumeroContoCorrente", r.NumeroContoCorrente, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Barcode", r.Barcode, ParameterDirection.Input);

        //        dbEngine_.AddParameter("@IdRata", -1, ParameterDirection.Output, DbType.Int32,  Int32.MaxValue);

        //        parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
        //        parameter_ = (IDataParameter)parameterCollection_["@IdRata"];
        //        r.IdRata = (int)parameter_.Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("CartelleDAO::InsertRata::si è verificato il seguente errore::" + ex.Message);
        //        throw (ex);
        //    }
        //}

        public void InsertRata(ref Rata r, ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Parameters.Clear();

                cmdMyCommand.Parameters.AddWithValue("@IdCartella", r.IdCartella);
                cmdMyCommand.Parameters.AddWithValue("@NumeroRata", r.NumeroRata);
                cmdMyCommand.Parameters.AddWithValue("@DescrizioneRata", r.DescrizioneRata);
                cmdMyCommand.Parameters.AddWithValue("@ImportoRata", r.ImportoRata);
                cmdMyCommand.Parameters.AddWithValue("@DataScadenza", r.DataScadenza);
                cmdMyCommand.Parameters.AddWithValue("@CodiceBollettino", r.CodiceBollettino);
                cmdMyCommand.Parameters.AddWithValue("@Codeline", r.Codeline);
                cmdMyCommand.Parameters.AddWithValue("@NumeroContoCorrente", r.NumeroContoCorrente);
                cmdMyCommand.Parameters.AddWithValue("@Barcode", r.Barcode);

                cmdMyCommand.Parameters.AddWithValue("@IdRata", -1);
                cmdMyCommand.Parameters["@IdRata"].Direction = ParameterDirection.Output;
                Log.Debug("CartelleDAO::InsertRata::esecuzione sp_InsertRata");
                cmdMyCommand.CommandText = "sp_InsertRata";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                int idRata = (int)cmdMyCommand.Parameters["@IdRata"].Value;

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.InsertRata.errore: ", Err);
                throw (Err);
            }
        }
        public int CalcolaRate(int DBOperation, int IdFlusso, int IdAvviso, double SogliaRata, double SogliaBollettino, ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Parameters.Clear();

                cmdMyCommand.Parameters.AddWithValue("@DBOperation", DBOperation);
                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", IdFlusso);
                cmdMyCommand.Parameters.AddWithValue("@IDAVVISO", IdAvviso);
                cmdMyCommand.Parameters.AddWithValue("@SOGLIARATA", SogliaRata);
                cmdMyCommand.Parameters.AddWithValue("@SOGLIABOLLETTINO", SogliaBollettino);


                cmdMyCommand.Parameters.AddWithValue("@Id", -1);
                cmdMyCommand.Parameters["@Id"].Direction = ParameterDirection.Output;
                cmdMyCommand.CommandText = "prc_SetRate";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                return (int)cmdMyCommand.Parameters["@Id"].Value;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.CalcoloRate.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myStringConnection"></param>
        /// <param name="IdEnte"></param>
        /// <param name="IdTributo"></param>
        /// <param name="IdFlusso"></param>
        /// <param name="IsRate"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="10/06/2013">
        /// <strong>ruolo supplettivo</strong>
        /// </revision>
        /// </revisionHistory>
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
        public DataTable GetMinuta(string myStringConnection, string IdEnte,string IdTributo,int IdFlusso, bool IsRate)
        {
            connessioneDB(myStringConnection);

            try
            {
                 cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdFlusso", IdFlusso);

                if (IsRate)
                    cmdMyCommand.CommandText = "prc_GetMinutaXStampatoreRate";
                else
                    cmdMyCommand.CommandText = "prc_GetMinutaXStampatoreDoc";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetMinuta.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            return dtMyDati;
        }
        //     public DataTable GetMinuta(int IdFlusso, bool IsRate)
          //     //public DataTable GetMinuta (int Anno)
          //     {
          //         //*** 20140409 
          //         connessioneDB();

        //         try
        //         {
        //             cmdMyCommand.Parameters.AddWithValue("@IdFlusso", IdFlusso);
        //             //dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
        //             //cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
        //             if (IsRate)
        //                 cmdMyCommand.CommandText = "prc_GetMinutaXStampatoreRate";
        //             else
        //                 cmdMyCommand.CommandText = "prc_GetMinutaXStampatoreDoc";
        //             Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
        //             myDataReader = cmdMyCommand.ExecuteReader();
        //             dtMyDati.Load(myDataReader);
        //         }
        //         catch (Exception Err)
        //         {
        //             Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetMinuta.errore: ", Err);
        //             throw (Err);
        //         }
        //         finally
        //         {
        //             cmdMyCommand.Connection.Close();
        //         }
        //         return dtMyDati;
        //         /*
        //DBEngine dbEngine_;
        //DataTable dt = new DataTable();

        //dbEngine_ = DBEngineFactory.GetDBEngine();
        //dbEngine_.OpenConnection();

        //try
        //{
        //	string commandText_ = "sp_GetMinuta";
        //	dbEngine_.AddParameter("@IdFlusso", IdFlusso, ParameterDirection.Input);
        //	//dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
        //	dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
        //	Log.Debug("CartelleDAO::GetMinuta::query::sp_GetMinuta::parametri::IdFlusso::"+IdFlusso.ToString()+"::IdEnte::"+DichiarazioneSession.IdEnte);
        //	dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
        //}
        //catch (Exception Err)
        //         {
        //             Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetMinuta.errore: ", Err);
        //             throw (Err);
        //         }
        //finally
        //{
        //	dbEngine_.CloseConnection();
        //}
        //return dt;
        //         */
        //     }
        //*** ***

        #region Gestione lotti

        //public void InsertLotto (Lotto l, ref DBEngine dbEngine_)
        //{
        //    try
        //    {
        //        IDataParameterCollection parameterCollection_;
        //        //IDataParameter parameter_;

        //        string commandText_ = "sp_InsertLotto";
        //        dbEngine_.ClearParameters();

        //        dbEngine_.AddParameter("@IdEnte", l.IdEnte, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@Anno", l.Anno, ParameterDirection.Input);
        //        parameterCollection_ = dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex)
        //    {
        //      Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.InsertLotto.errore: ", Err);
        //        throw (ex);
        //    }
        //}

        public void InsertLotto(Lotto l, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            try
            {
                //IDataParameter parameter_;

                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                    cmdMyCommand.Connection.Open();
                cmdMyCommand.Parameters.Clear();

                cmdMyCommand.Parameters.AddWithValue("@IdEnte", l.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Anno", l.Anno);
                Log.Debug("CartelleDAO::InsertLotto::esecuzione sp_InsertLotto");
                cmdMyCommand.CommandText = "sp_InsertLotto";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                Log.Debug(l.IdEnte + " - OPENgovOSAP.CartellaDAO.InsertLotto.errore: ", Err);
                throw (Err);
            }
        }

        public DataTable GetLotto(string IdEnte,int Anno, int NLotto, ref SqlCommand cmdMyCommand)
        {
            bool bCloseCon = false;
            try
            {
                dtMyDati = new DataTable();
                //*** 20140409 
                if (cmdMyCommand == null)
                {
                    connessioneDB();
                    bCloseCon = true;
                }
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                    cmdMyCommand.Connection.Open();
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@NLotto", NLotto);
                cmdMyCommand.CommandText = "sp_GetLotto";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetLotto.errore: ", Err);
                throw (Err);
            }
            finally
            {
                if (bCloseCon)
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
				string commandText_ = "sp_GetLotto";
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@NLotto", NLotto, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
				catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetLotto.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        #endregion Gestione lotti

        #region Gestione configurazione rate

        //*** 20130610 - ruolo supplettivo ***
        //public void InsertConfigurazioneRate (int Anno, Rate[] c)
        public void InsertConfigurazioneRate(Rate[] c)
        {
            //*** 20140409               
            try
            {
                connessioneDB();
                //SqlTransaction iniziaTransazione = conn.BeginTransaction();
                iniziaTransazione = cmdMyCommand.Connection.BeginTransaction();
                cmdMyCommand.Transaction = iniziaTransazione;

                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdFlusso", c[0].IdFlusso);
                //dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
                //dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
                Log.Debug("CartelleDAO::InsertConfigurazioneRate::esecuzione sp_DeleteConfigurazioneRate");
                cmdMyCommand.CommandText = "sp_DeleteConfigurazioneRate";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.ExecuteNonQuery();
                checked
                {
                    for (int i = 0; i < c.Length; i++)
                    {
                        cmdMyCommand.Parameters.Clear();
                        cmdMyCommand.Parameters.AddWithValue("@IdFlusso", c[i].IdFlusso);
                        cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                        cmdMyCommand.Parameters.AddWithValue("@Anno", c[i].Anno);
                        cmdMyCommand.Parameters.AddWithValue("@NRata", c[i].NRata);
                        cmdMyCommand.Parameters.AddWithValue("@Descrizione", c[i].Descrizione);
                        cmdMyCommand.Parameters.AddWithValue("@DataScadenza", c[i].DataScadenza);
                        Log.Debug("CartelleDAO::InsertConfigurazioneRate::esecuzione sp_InsertConfigurazioneRate");
                        cmdMyCommand.CommandText = "sp_InsertConfigurazioneRate";
                        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                        myDataReader = cmdMyCommand.ExecuteReader();
                        dtMyDati.Load(myDataReader);
                    }
                }
                //cmdMyCommand.Transaction.Commit();
                iniziaTransazione.Commit();
            }
            catch (Exception Err)
            {
                //cmdMyCommand.Transaction.Rollback();
                iniziaTransazione.Rollback();
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.InsertConfigurazioneRate.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
            /*
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				dbEngine_.BeginTransaction ();

				string commandText_ = "sp_DeleteConfigurazioneRate";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdFlusso", c[0].IdFlusso, ParameterDirection.Input);
				//dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				//dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);

				commandText_ = "sp_InsertConfigurazioneRate";
				
				for (int i = 0; i < c.Length; i++)
				{
					dbEngine_.ClearParameters();
					dbEngine_.AddParameter("@IdFlusso", c[i].IdFlusso, ParameterDirection.Input);
					dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
					dbEngine_.AddParameter("@Anno", c[i].Anno, ParameterDirection.Input);
					dbEngine_.AddParameter("@NRata", c [i].NRata, ParameterDirection.Input);
					dbEngine_.AddParameter("@Descrizione", c [i].Descrizione, ParameterDirection.Input);
					dbEngine_.AddParameter("@DataScadenza", c [i].DataScadenza, ParameterDirection.Input);
					dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
				}

				dbEngine_.CommitTransaction ();
			}
			catch (Exception Err)
			{
				dbEngine_.RollbackTransaction ();
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.InsertConfigurazioneRate.errore: ", Err);
				throw (Err);
			}
            */
        }
        //*** ***

        //*** 20130610 - ruolo supplettivo ***
        //public DataTable GetConfigurazioneRate (int Anno)
        public DataTable GetConfigurazioneRate(int IdFlusso)
        {
            //*** 20140409 
            connessioneDB();
            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdFlusso", IdFlusso);
                Log.Debug("CartelleDAO::GetConfigurazioneRate::esecuzione sp_GetConfigurazioneRate");
                cmdMyCommand.CommandText = "sp_GetConfigurazioneRate";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetConfigurazioneRate.errore: ", Err);
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
				string commandText_ = "sp_GetConfigurazioneRate";
				//dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				//dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdFlusso", IdFlusso, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
				catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.GetConfigurazioneRate.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			return dt;
            */
        }
        //*** ***
        #endregion Gestione configurazione rate

        #endregion Calcolo Massivo


        public void connessioneDB()
        {
            //*** 20140409
            try {
                Log.Debug("CartelleDAO::connessioneDB::apro la connessione al DB");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.connessioneDB.errore: ", Err);
                throw (Err);
            }


        }
        public void connessioneDB(string ConnectionString)
        {
            //*** 20140409
            try
            {
                Log.Debug("CartelleDAO::connessioneDB::apertura della connessione al DB su " + ConnectionString);
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.connessioneDB.errore: ", Err);
                throw (Err);
            }

        }
        #region ImportFlussi
        public SqlCommand StartImportTransaction()
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                connessioneDB();
                conn = cmdMyCommand.Connection;
                SqlTransaction iniziaTransazione = conn.BeginTransaction();
                cmdMyCommand.Transaction = iniziaTransazione;
                return cmdMyCommand;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.StartImportTransaction.errore: ", Err);
                throw (Err);
            }
        }
        public SqlCommand OpenImportConnection()
        {
            try
            {
                connessioneDB();
                return cmdMyCommand;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.OpenImportConnection.errore: ", Err);
                throw (Err);
            }
        }
        public void CommitImportTransaction(ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Transaction.Commit();
                cmdMyCommand.Connection.Close();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.CommitImportTransaction.errore: ", Err);
                throw (Err);
            }
        }
        public void RollbackImportTransaction(ref SqlCommand cmdMyCommand)
        {
            try
            {
                cmdMyCommand.Transaction.Rollback();
                cmdMyCommand.Connection.Close();
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CartellaDAO.RollbackImportTranscation.errore: ", Err);
                throw (Err);
            }
        }
        #endregion
    }
}
