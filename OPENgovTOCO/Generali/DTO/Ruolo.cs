using System;
using System.Data;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione ruolo
    /// </summary>
	public class MetodiRuolo
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiRuolo));
		#region "Public methods"

        public static Ruolo[] GetRuoliCartella(string ConnectionString, int IdCartella, string IdEnte, Cartella objCartella)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetRuoliCartella (IdCartella, IdEnte);
				Ruolo[] ruoli = FillRuoliFromDataTable(dt);

				if (objCartella != null)
				{
					foreach (Ruolo r in ruoli)
						r.CartellaTOCO = objCartella;
				}

					foreach (Ruolo r in ruoli)
					{
                        r.ArticoloTOCO = MetodiArticolo.GetArticolo(ConnectionString,r.IdRuolo,IdEnte,r.CartellaTOCO.IdTributo);
//						if(r.ArticoloTOCO.Categoria!=null && r.ArticoloTOCO.TipologiaOccupazione!=null)
//                            r.Tariffa = MetodiTariffe.GetTariffa (r.CartellaTOCO.Anno,
//								r.ArticoloTOCO.Categoria.IdCategoria,
//								r.ArticoloTOCO.TipologiaOccupazione.IdTipologiaOccupazione);
				}

				return ruoli;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiRuolo.GetRuoliCartella.errore: ", Err);
                throw Err;
            }
        }

		#endregion

		#region "Private Method"

		private static Ruolo[] FillRuoliFromDataTable(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Ruolo CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Ruolo();

                    CurrentItem.IdRuolo = (int)myRow["IDRUOLO"];
                    CurrentItem.CartellaTOCO = new Cartella();
                    CurrentItem.CartellaTOCO.IdCartella = (int)myRow["IDCARTELLA"];
                    CurrentItem.ArticoloTOCO = new Articolo();
                    CurrentItem.ArticoloTOCO.IdArticolo = (int)myRow["IDARTICOLO"];
                    CurrentItem.Importo = double.Parse(myRow["IMPORTO"].ToString());
                    CurrentItem.ImportoLordo = double.Parse(myRow["IMPORTO_LORDO"].ToString());
                    if (myRow["DATA_VARIAZIONE"] != DBNull.Value)
                        CurrentItem.DataVariazione = (DateTime)myRow["DATA_VARIAZIONE"];
                    else
                        CurrentItem.DataVariazione = DateTime.MaxValue;
                    CurrentItem.Tariffa = new Tariffe();
                    CurrentItem.Tariffa.Valore = double.Parse(myRow["TARIFFA_APPLICATA"].ToString());

                    MyArray.Add(CurrentItem);
                }

                return (Ruolo[])MyArray.ToArray(typeof(Ruolo));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiRuolo.FillRuoliFromDataTable.errore: ", Err);
                throw Err;
            }
        }

		#endregion

	}

}