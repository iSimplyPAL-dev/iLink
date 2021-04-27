using System;
using System.Data;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione rate
    /// </summary>
	public class MetodiRata
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiRata));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="cmdMyCommand"></param>
         public static void InsertRata(Rata r, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DAO.InsertRata(ref r, ref cmdMyCommand);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiRata.InsertRata.errore: ", Err);
                throw Err;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBOperation"></param>
        /// <param name="IdFlusso"></param>
        /// <param name="IdAvviso"></param>
        /// <param name="SogliaRata"></param>
        /// <param name="SogliaBollettino"></param>
        /// <param name="cmdMyCommand"></param>
        public static void CalcolaRate(int DBOperation, int IdFlusso, int IdAvviso, double SogliaRata, double SogliaBollettino, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DAO.CalcolaRate(DBOperation, IdFlusso, IdAvviso, SogliaRata, SogliaBollettino, ref cmdMyCommand);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiRata.CalcolaRate.errore: ", Err);
                throw Err;
            }
        }
		/// <summary>
		/// Metodo per recuperare le rate 
		/// </summary>
		/// <param name="searchParameter"></param>
		/// <returns></returns>
		public static RataExt[] GetRate(RataSearch searchParameter)
		{
			if (searchParameter == null)
				throw new Exception("L'oggetto 'searchParameter' non può essere nullo");

			DAO.PagamentiDAO DAO = new DAO.PagamentiDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.RicercaRate(searchParameter);
				if (dt != null)
					return FillRateExtFromDataTable(dt);
				else
					return new RataExt[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiRata.GetRate.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// S.T. Riscrittura del metodo
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		private static RataExt[] FillRateExtFromDataTable(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                RataExt CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new RataExt();
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    if (myRow["ANNO"] != DBNull.Value)
                        CurrentItem.Anno = (int)myRow["ANNO"];

                    if (myRow["COD_CONTRIBUENTE"] != DBNull.Value)
                        CurrentItem.IdContribuente = (int)myRow["COD_CONTRIBUENTE"];

                    if (myRow["CODICE_CARTELLA"] != DBNull.Value)
                        CurrentItem.CodiceCartella = (string)myRow["CODICE_CARTELLA"];

                    if (myRow["IDRATA"] != DBNull.Value)
                        CurrentItem.IdRata = (int)myRow["IDRATA"];

                    if (myRow["IDENTE"] != DBNull.Value)
                        CurrentItem.IdEnte = (string)myRow["IDENTE"];

                    if (myRow["COD_CONTRIBUENTE"] != DBNull.Value)
                        CurrentItem.IdContribuente = (int)myRow["COD_CONTRIBUENTE"];

                    if (myRow["NUMERO_RATA"] != DBNull.Value)
                        CurrentItem.NumeroRata = (string)myRow["NUMERO_RATA"];

                    if (myRow["IMPORTO_RATA"] != DBNull.Value)
                        CurrentItem.ImportoRata = double.Parse(myRow["IMPORTO_RATA"].ToString());

                    if (myRow["IMPORTO_PAGATO"] != DBNull.Value)
                        CurrentItem.ImportoPagato = double.Parse(myRow["IMPORTO_PAGATO"].ToString());
                    //else
                    //	CurrentItem.ImportoPagato = double.Parse(myRow["IMPORTO_RATA"].ToString());

                    if (myRow["DATA_SCADENZA"] != DBNull.Value)
                        CurrentItem.DataScadenza = (DateTime)myRow["DATA_SCADENZA"];

                    if (myRow["DATA_PAGAMENTO"] != DBNull.Value)
                        CurrentItem.DataPagamento = (DateTime)myRow["DATA_PAGAMENTO"];

                    if (myRow["DATA_ACCREDITO"] != DBNull.Value)
                        CurrentItem.DataAccredito = (DateTime)myRow["DATA_ACCREDITO"];

                    CurrentItem.IdRata = (int)myRow["IDRATA"];

                    MyArray.Add(CurrentItem);
                }

                return (RataExt[])MyArray.ToArray(typeof(RataExt));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiRata.FillRateExtFromDataTable.errore: ", Err);
                throw Err;
            }
        }

	}
}
