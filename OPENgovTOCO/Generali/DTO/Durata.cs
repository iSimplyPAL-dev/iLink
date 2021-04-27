using System;
using System.Data;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione durata
    /// </summary>
	public class MetodiDurata
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Durata));

		#region "Public methods"

        public static Durata[] GetDurate(string ConnectionString, bool DefaultValue)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetDurata(ConnectionString ,- 1);
				return FillDurateFromDataTable(dt, DefaultValue);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDurata.GetDurate.errore: ", Err);
                throw (Err);
            }
        }


        public static Durata GetDurata(string ConnectionString, int IdDurata)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetDurata(ConnectionString,IdDurata);
				Durata[] dur = FillDurateFromDataTable(dt, false);
				return dur[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDurata.GetDurata.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

		#region "Private Method"

		private static Durata[] FillDurateFromDataTable(DataTable dt, bool DefaultValue)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Durata CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Durata();

                    CurrentItem.IdDurata = (int)myRow["IDDURATA"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];

                    MyArray.Add(CurrentItem);
                }

                if (DefaultValue)
                {
                    //'Add default value
                    CurrentItem = new Durata();

                    //FillCategorie
                    CurrentItem.IdDurata = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }


                return (Durata[])MyArray.ToArray(typeof(Durata));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDurata.FillDurateFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

	}
}
