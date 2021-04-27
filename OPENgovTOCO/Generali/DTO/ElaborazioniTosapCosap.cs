using System;
using System.Data;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
	/// <summary>
	/// Classe per la ricerca Elaborazioni.
	/// </summary>
	public class MetodiElaborazioniTosapCosap
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiElaborazioniTosapCosap));
		
		public static Cartella[] SearchElaborazione(ElaborazioniSearch SearchParams)
		{
			DAO.ElaborazioniDAO objDAO = new DAO.ElaborazioniDAO();

			try 
			{
				Cartella[] lista = objDAO.SearchElaborazioni(SearchParams);
				return lista;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiElaborazioniTosapCosap.SearchElaborazione.errore: ", Err);
                throw (Err);
            }
        }
	}
}
