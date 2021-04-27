using System;
using System.Collections;
using System.Data;
using log4net;
using IRemInterfaceOSAP;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione Lotto.
    /// </summary>
    public class MetodiLotto
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiLotto));
		#region "Public Method"

		public static Lotto GetLotto(string IdEnte,int Anno, int NLotto, ref SqlCommand cmdMyCommand)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetLotto(IdEnte,Anno, NLotto,ref cmdMyCommand);
				Lotto[] lotti = FillLottoFromDataTable(dt);
				if (lotti.Length > 0)
					return lotti[0];
				else
					return null; // Non è ancora stata fatta alcuna elaborazione
								// per l'anno considerato
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiLotto.GetLotto.errore: ", Err);
                throw (Err);
            }
        }

        //public static void InsertLotto(Lotto l, ref DBEngine dbEngine)
        //{
        //    DAO.CartelleDAO DAO = new DAO.CartelleDAO();

        //    try 
        //    {
        //        DAO.InsertLotto(l, ref dbEngine);
        //    } 
        //    catch (Exception Err) 
        //    {
        //         Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiLotto.InsertLotto.errore: ", Err);
        //        throw Err;
        //    }
        //}
        public static void InsertLotto(Lotto l, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DAO.InsertLotto(l, ref cmdMyCommand);
            }
            catch (Exception Err)
            {
                Log.Debug(l.IdEnte + " - OPENgovOSAP.MetodiLotto.InsertLotto.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

		#region "Private Method"

		private static Lotto[] FillLottoFromDataTable(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Lotto CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Lotto();

                    CurrentItem.IdEnte = (string)myRow["IDENTE"];
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.NLotto = (int)myRow["NLOTTO"];
                    CurrentItem.PrimaCartella = (int)myRow["PRIMACARTELLA"];
                    CurrentItem.UltimaCartella = (int)myRow["ULTIMACARTELLA"];
                    CurrentItem.CodiceConcessione = (string)myRow["CODICECONCESSIONE"];
                    CurrentItem.DataEmissione = (DateTime)myRow["DATAEMISSIONE"];

                    MyArray.Add(CurrentItem);
                }

                return (Lotto[])MyArray.ToArray(typeof(Lotto));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiLotto.FillLottoFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

	}
}
