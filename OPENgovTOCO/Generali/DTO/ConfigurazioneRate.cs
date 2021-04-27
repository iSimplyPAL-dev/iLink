using System;
using System.Collections;
using System.Data;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la configurazione Rate
    /// </summary>
	public class MetodiConfigurazioneRate
		{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiConfigurazioneRate));
			#region "Public Method"

		//*** 20130610 - ruolo supplettivo ***
		//public static Rate[] GetConfigurazioneRate(int Anno)
		public static Rate[] GetConfigurazioneRate(int IdFlusso)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();
			try 
			{
				DataTable dt = null;
				dt = DAO.GetConfigurazioneRate(IdFlusso);
				return FillConfigurazioneRateFromDataTable(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiConfigurazioneRate.GetConfigurazioneRate.errore: ", Err);
                throw (Err);
            }
        }
		//*** ***

		public static void InsertConfigurazioneRate(Rate[] c)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();

			try 
			{
				DAO.InsertConfigurazioneRate(c);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiConfigurazioneRate.InsertConfigurazioneRate.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

		#region "Private Method"

		private static Rate[] FillConfigurazioneRateFromDataTable(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Rate CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Rate();
                    //*** 20130610 - ruolo supplettivo ***
                    CurrentItem.IdFlusso = (int)myRow["IDFLUSSO_RUOLO"];
                    //*** ***
                    CurrentItem.IdEnte = (string)myRow["IDENTE"];
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.NRata = (string)myRow["NRATA"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.DataScadenza = (DateTime)myRow["DATASCADENZA"];

                    MyArray.Add(CurrentItem);
                }

                return (Rate[])MyArray.ToArray(typeof(Rate));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiConfigurazioneRate.FillConfigurazioneRateFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

	}
}
