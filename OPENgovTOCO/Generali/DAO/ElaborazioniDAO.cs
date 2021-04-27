using System;
using System.Data;
using System.Collections;
using log4net;
using OPENUtility;
using IRemInterfaceOSAP;
using AnagInterface;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione Elaborazioni.
    /// </summary>
    public class ElaborazioniDAO
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ElaborazioniDAO));
        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

		public ElaborazioniDAO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Cartella[] SearchElaborazioni(DTO.ElaborazioniSearch SearchParams)
		{
            //*** 20140409 
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", SearchParams.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", SearchParams.IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@DBName", SearchParams.DBName);
                cmdMyCommand.Connection = new SqlConnection(DichiarazioneSession.StringConnection);
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                    cmdMyCommand.Connection.Open();
                if (SearchParams.CodiceCartella != null)
                    cmdMyCommand.Parameters.AddWithValue("@CodiceCartella", SearchParams.CodiceCartella);
                if (SearchParams.Nome != null)
                    cmdMyCommand.Parameters.AddWithValue("@Nome", SearchParams.Nome);
                else
                {
                    if (SearchParams.NomeDA != null)
                        cmdMyCommand.Parameters.AddWithValue("@NomeDA", SearchParams.NomeDA);
                    if (SearchParams.NomeA != null)
                        cmdMyCommand.Parameters.AddWithValue("@NomeA", SearchParams.NomeA);
                }
                cmdMyCommand.Parameters.AddWithValue("@Anno", SearchParams.Anno);
                //*** 20130610 - ruolo supplettivo ***
                cmdMyCommand.Parameters.AddWithValue("@IdFlusso", SearchParams.IdFlusso);
                //*** ***
                Log.Debug("ElaborazioniDAO::SearchElaborazioni::esecuzione sp_SearchDocumentiElaborazioni");
                cmdMyCommand.CommandText = "sp_SearchDocumentiElaborazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
                return FillCartellaFromDataTable(dtMyDati);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioniDAO.SearchElaborazioni.errore: ", Err);
                return null;
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
				string tableName = String.Empty;

				string commandText_ = "sp_SearchDocumentiElaborazioni";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", SearchParams.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@DBName", SearchParams.DBName, ParameterDirection.Input);
				
				if(SearchParams.CodiceCartella != null)
					dbEngine_.AddParameter("@CodiceCartella", SearchParams.CodiceCartella, ParameterDirection.Input);
				if(SearchParams.Nome!=null)
					dbEngine_.AddParameter("@Nome", SearchParams.Nome, ParameterDirection.Input);
				else
				{
					if(SearchParams.NomeDA != null)
						dbEngine_.AddParameter("@NomeDA", SearchParams.NomeDA, ParameterDirection.Input);
					if(SearchParams.NomeA != null)
						dbEngine_.AddParameter("@NomeA", SearchParams.NomeA, ParameterDirection.Input);
				}
				dbEngine_.AddParameter("@Anno", SearchParams.Anno, ParameterDirection.Input);
				//*** 20130610 - ruolo supplettivo ***
				dbEngine_.AddParameter("@IdFlusso", SearchParams.IdFlusso, ParameterDirection.Input);
				//*** ***

				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				return FillCartellaFromDataTable(dt);
			}
			catch (Exception ex)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioniDAO.SearchElaborazioni.errore: ", Err);
				return null;
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
            */
        }

        public int GetElaborazioniStatistics(string IdEnte, int IdFlussoRuolo, int Anno, out int Total, out int Elaborati, out int DaElaborare)
		{
            //*** 20140409 
            connessioneDB();
            Total = 0;
            Elaborati = 0;
            DaElaborare = 0;
            try
            {
                             cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdFlussoRuolo", IdFlussoRuolo);
                Log.Debug("ElaborazioniDAO::GetElaborazioniStatistics::esecuzione sp_GetElaborazioniStatistics");
                cmdMyCommand.CommandText = "sp_GetElaborazioniStatistics";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);

                if (dtMyDati.Rows.Count == 1)
                {
                    Total = (int)dtMyDati.Rows[0]["Total"];
                    Elaborati = (int)dtMyDati.Rows[0]["Elaborati"];
                    DaElaborare = Total - Elaborati;
                }

                return 0;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioniDAO.GetElaborazioniStatistics.errore: ", Err);
                return -1;
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            /*
			Total=Elaborati=DaElaborare=0;
			DBEngine dbEngine_;
			DataTable dt = new DataTable();

			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
	
			try
			{
				string tableName = String.Empty;

				string commandText_ = "sp_GetElaborazioniStatistics";
				dbEngine_.ClearParameters();
				dbEngine_.AddParameter("@IdEnte", IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdFlussoRuolo", IdFlussoRuolo, ParameterDirection.Input);

				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				if(dt.Rows.Count==1)
				{
					Total = (int)dt.Rows[0]["Total"];
					Elaborati = (int)dt.Rows[0]["Elaborati"];
					DaElaborare=Total-Elaborati;
				}

				return 0;
			}
			catch (Exception ex)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioniDAO.GetElaborazioniStatistics.errore: ", Err);
				return -1;
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
            */
        }

        public Cartella[] FillCartellaFromDataTable(DataTable dt)
		{
            ArrayList MyArray = new ArrayList();
            Cartella CurrentItem = null;

            try
            {
                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Cartella();
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    CurrentItem.IdCartella = (int)myRow["IDCARTELLA"];
                    if (myRow["CODICE_CARTELLA"] != DBNull.Value)
                        CurrentItem.CodiceCartella = (string)myRow["CODICE_CARTELLA"];
                    else
                        CurrentItem.CodiceCartella = string.Empty;
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.DataEmissione = (DateTime)myRow["DATA_EMISSIONE"];
                    CurrentItem.CodContribuente = (int)myRow["COD_CONTRIBUENTE"];
                    CurrentItem.IdEnte = (string)myRow["IDENTE"];

                    CurrentItem.Dichiarazione = new DichiarazioneTosapCosap();
                    CurrentItem.Dichiarazione.IdDichiarazione = (int)myRow["IDDICHIARAZIONE"];

                    CurrentItem.ImportoTotale = double.Parse(myRow["IMPORTO_TOTALE"].ToString());
                    CurrentItem.ImportoArrotondamento = double.Parse(myRow["IMPORTO_ARROTONDAMENTO"].ToString());
                    CurrentItem.ImportoCarico = double.Parse(myRow["IMPORTO_CARICO"].ToString());
                    if (myRow["IDFLUSSO_RUOLO"] != DBNull.Value)
                        CurrentItem.IdFlussoRuolo = (int)myRow["IDFLUSSO_RUOLO"];
                    else
                        CurrentItem.IdFlussoRuolo = -1;
                    if (myRow["DATA_VARIAZIONE"] != DBNull.Value)
                        CurrentItem.DataVariazione = (DateTime)myRow["DATA_VARIAZIONE"];
                    else
                        CurrentItem.DataVariazione = DateTime.MaxValue;

                    CurrentItem.DettaglioContribuente = new DettaglioAnagrafica();
                    CurrentItem.DettaglioContribuente.Cognome = (string)myRow["COGNOME_DENOMINAZIONE"];
                    CurrentItem.DettaglioContribuente.Nome = (string)myRow["NOME"];
                    CurrentItem.IdTipoDoc = (myRow["TIPODOC"] == DBNull.Value) ? 0 : int.Parse(myRow["TIPODOC"].ToString());
                    CurrentItem.IsSgravio = (myRow["ISSGRAVIO"] == DBNull.Value) ? false : ((int.Parse(myRow["ISSGRAVIO"].ToString())==0? false :true));
                    CurrentItem.ImpPreSgravio = (myRow["IMPPRESGRAVIO"] == DBNull.Value) ? 0 : decimal.Parse(myRow["IMPPRESGRAVIO"].ToString());
                    CurrentItem.ImpPagato = (myRow["IMPPAGATO"] == DBNull.Value) ? 0 : decimal.Parse(myRow["IMPPAGATO"].ToString());

                    MyArray.Add(CurrentItem);

                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioniDAO.FillCartellaFromDataTable.errore: ", Err);
                throw Err;
            }

			return (Cartella[])MyArray.ToArray(typeof(Cartella));
		}

        public void connessioneDB()
        {
            //*** 20140409
            try {
                Log.Debug("ElaborazioniDAO::connessioneDB::apertura della connessione al DB");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioniDAO.connessioneDB.errore: ", Err);
                
            }

        }
	}
}
