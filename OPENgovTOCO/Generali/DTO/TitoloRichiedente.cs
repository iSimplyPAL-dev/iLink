using System;
using System.Data;
using System.Configuration;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione richiedente
    /// </summary>
	public class MetodiTitoloRichiedente
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TitoloRichiedente));

		#region "Public Method"

		public static TitoloRichiedente[] GetTitoloRichiedente()
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetTitoloRichiedente(-1);
				return FillTitoloRichiedenteFromDataTable(dt, true);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTitoloRichiedente.GetTitoloRichiedente.errore: ", Err);
                throw Err;
            }

        }

		public static TitoloRichiedente GetTitoloRichiedente(int IdTitoloRichiedente)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetTitoloRichiedente(IdTitoloRichiedente);
				TitoloRichiedente[] titoli = FillTitoloRichiedenteFromDataTable(dt, true);
				return titoli[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTitoloRichiedente.GetTitoloRichiedente.errore: ", Err);
                throw Err;
            }

        }
        public static TitoloRichiedente GetTitoloRichiedente(string ConnectionString, string IdEnte, string Descrizione, bool defaultValue)
        {
            DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetTitoloRichiedente(ConnectionString, IdEnte, Descrizione);
                TitoloRichiedente[] titoli = FillTitoloRichiedenteFromDataTable(dt,  defaultValue);
                return titoli[0];
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTitoloRichiedente.GetTitoloRichiedente.errore: ", Err);
                throw Err;
            }

        }
		#endregion

		#region "Private Method"

		private static TitoloRichiedente[] FillTitoloRichiedenteFromDataTable(DataTable dt, bool defaultValue)
		{
            try {

                ArrayList MyArray = new ArrayList();
                TitoloRichiedente CurrentItem = null;



                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new TitoloRichiedente();
                    //FillTipologieOccupazioni
                    CurrentItem.IdTitoloRichiedente = (int)myRow["IdTitoloRichiedente"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];

                    MyArray.Add(CurrentItem);
                }

                if (defaultValue)
                {
                    //'Add default value
                    CurrentItem = new TitoloRichiedente();

                    CurrentItem.IdTitoloRichiedente = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }

                return (TitoloRichiedente[])MyArray.ToArray(typeof(TitoloRichiedente));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTitoloRichiedente.FillTitoloRichiedenteFromDataTable.errore: ", Err);
                throw Err;
            }
        }


		#endregion

	}

}