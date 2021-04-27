using System;
using System.Data;
using System.Configuration;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;
//using OPENgovOSAP.MotoreTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione uffici
    /// </summary>
	public class MetodiUffici
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Uffici));

		#region "Public Method"
		public static Uffici[] GetUffici()
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetUffici(-1);
				return FillUfficiFromDataTable(dt, true);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiUffici.GetUffici.errore: ", Err);
                throw Err;
            }

        }


		public static Uffici GetUfficio(int IdUfficio)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetUffici(IdUfficio);
				Uffici[] uff = FillUfficiFromDataTable(dt, true);
				return uff[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiUffici.GetUfficio.errore: ", Err);
                throw Err;
            }

        }
        public static Uffici GetUfficio(string ConnectionString, string IdEnte, string Descrizione, bool defaultValue)
        {
            DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetUffici(ConnectionString, IdEnte, Descrizione);
                Uffici[] uff = FillUfficiFromDataTable(dt, defaultValue);
                return uff[0];
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiUffici.GetUfficio.errore: ", Err);
                throw Err;
            }
        }
		#endregion

		#region "Private Method"
		private static Uffici[] FillUfficiFromDataTable(DataTable dt, bool defaultValue)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Uffici CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Uffici();

                    CurrentItem.IdUfficio = (int)myRow["IdUfficio"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];

                    MyArray.Add(CurrentItem);
                }

                if (defaultValue)
                {
                    //'Add default value
                    CurrentItem = new Uffici();
                    //FillCategorie
                    CurrentItem.IdUfficio = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }

                return (Uffici[])MyArray.ToArray(typeof(Uffici));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiUffici.FillUfficiFromDataTable.errore: ", Err);
                throw Err;
            }
        }
		#endregion
	}
}