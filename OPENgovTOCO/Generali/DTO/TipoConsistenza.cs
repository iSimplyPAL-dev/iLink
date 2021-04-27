using System;
using System.Data;
using System.Collections;
using System.Web.Services;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione tipo consistenza
    /// </summary>
	public class MetodiTipoConsistenza
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TipoConsistenza));
		#region "Public Method"

        public static TipoConsistenza[] GetTipiConsistenza(string ConnectionString, bool DefaultValue)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetTipiConsistenza(ConnectionString ,- 1);
				return FillTipiConsistenzaFromDataTable(dt, DefaultValue);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipoConsistenza.GetTipiConsistenza.errore: ", Err);
                throw Err;
            }

        }

        public static TipoConsistenza GetTipoConsistenza(string ConnectionString, int IdTipoConsistenza)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetTipiConsistenza(ConnectionString,IdTipoConsistenza);
				TipoConsistenza[] tipi = FillTipiConsistenzaFromDataTable(dt, false);
				return tipi[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipoConsistenza.GetTipoConsistenza.errore: ", Err);
                throw Err;
            }

        }
		#endregion

		#region "Private Method"

		private static TipoConsistenza[] FillTipiConsistenzaFromDataTable(DataTable dt, bool defaultValue)
		{
            try {
                ArrayList MyArray = new ArrayList();
                TipoConsistenza CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new TipoConsistenza();
                    //FillCategorie
                    CurrentItem.IdTipoConsistenza = (int)myRow["IdTipoConsistenza"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];

                    MyArray.Add(CurrentItem);
                }

                if (defaultValue)
                {
                    //'Add default value
                    CurrentItem = new TipoConsistenza();

                    //FillCategorie
                    CurrentItem.IdTipoConsistenza = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }

                return (TipoConsistenza[])MyArray.ToArray(typeof(TipoConsistenza));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipoConsistenza.FillTipiConsistenzaFromDataTable.errore: ", Err);
                throw Err;
            }
        }


		#endregion
	
	}
}
