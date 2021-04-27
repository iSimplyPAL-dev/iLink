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
    /// Classe per la gestione tipo occupazioni
    /// </summary>
	public class MetodiTipologieOccupazioni
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TipologieOccupazioni));

		#region "Public Method"

		public static int DeleteTipologiaOccupazione(int IdTipologiaOccupazione)
		{
            try {
                DAO.TipoOccupazDAO DAO = new DAO.TipoOccupazDAO();
                return DAO.DeleteTipologiaOccupazione(IdTipologiaOccupazione);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.DeleteTipologiaOccupazione.errore: ", Err);
                throw Err;
            }
        }

		public static int DeleteCoefficiente(int IdTipologiaOccupazione, int Anno)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.DeleteCoefficiente(IdTipologiaOccupazione, Anno, "TipologieOccupazioni");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.DeleteCoefficiente .errore: ", Err);
                throw Err;
            }
        }

        public static int InsertOrUpdateDescrizione(string ConnectionString, string IdEnte, string IdTributo, int IdTipologiaOccupazione, string Descrizione)
		{
            try {
                DAO.TipoOccupazDAO DAO = new DAO.TipoOccupazDAO();
                return DAO.InsertOrUpdateDescrizione(ConnectionString, IdEnte, IdTributo, IdTipologiaOccupazione, Descrizione);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.InsertOrUpdateDescrizione.errore: ", Err);
                throw Err;
            }
        }

		public static int InsertOrUpdateCoefficiente(int IdTipologiaOccupazione, int Anno, decimal Coefficiente)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.InsertOrUpdateCoefficiente(IdTipologiaOccupazione, Anno, Coefficiente, "TipologieOccupazioni");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.InsertOrUpdateCoefficiente.errore: ", Err);
                throw Err;
            }
        }

		public static TipologieOccupazioni[] GetAllTipologieOccupazioni(string IdEnte, string IdTributo, bool DefaultValue)
		{
			try 
			{
                DataTable dt = GetAllTipologieOccupazioniFromDB(IdEnte, IdTributo);
				return FillAllTipologiaOccupazioniFromDataTable(dt, DefaultValue);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetAllTipologieOccupazione.errore: ", Err);
                throw Err;
            }

        }

		public static Coefficienti[] GetAllCoefficienti(string IdEnte)
		{
			try 
			{
				DataTable dt = GetAllCaefficientiFromDB(IdEnte, "TipologieOccupazioni");
				return FillAllCoefficienti(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetAllCoefficientiFromDB.errore: ", Err);
                throw Err;
            }
        }

		public static Coefficienti GetCoefficiente(int IdTipologiaOccupazione, int Anno)
		{
			try 
			{
				DataTable dt = GetCoefficienteFromDB (IdTipologiaOccupazione, Anno);
				Coefficienti[] lista = FillAllCoefficiente(dt);
				if((lista != null) && (lista.Length>0))
					return lista[0];
				return null;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetCoefficiente.errore: ", Err);
                throw Err;
            }
        }

		public static TipologieOccupazioni[] GetTipologieOccupazioni(int Anno, bool DefaultValue, string idEnte)
		{
			try 
			{
                DataTable dt = GetTipologieOccupazioniFromDB(OPENgovTOCO.DichiarazioneSession.StringConnection,Anno, idEnte, OPENgovTOCO.DichiarazioneSession.CodTributo(""));
				return FillTipologiaOccupazioniFromDataTable(dt, DefaultValue);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetTipologieOccupazioni.errore: ", Err);
                throw Err;
            }

        }

        public static TipologieOccupazioni[] GetTipologieOccupazioniForMotore(string ConnectionString, int Anno, string idEnte, string IdTributo)
		{
			try 
			{
                DataTable dt = GetTipologieOccupazioniFromDB(ConnectionString,Anno, idEnte, IdTributo);
				return FillTipologiaOccupazioniFromDataTableForMotore(dt);
			}

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetTipologieOccupazioniFromDB.errore: ", Err);
                throw Err;
            }
        }

        public static TipologieOccupazioni GetTipologiaOccupazione(string ConnectionString, int IdTipologiaOccupazione,string Descrizione, string idEnte, string CodTributo)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetTipologieOccupazioni(ConnectionString,IdTipologiaOccupazione,Descrizione, -1, idEnte, CodTributo);
				TipologieOccupazioni[] tipi = FillTipologiaOccupazioniFromDataTable(dt, false);
				return tipi[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetTipologiaOccupazione.errore: ", Err);
                throw Err;
            }

        }

        public static int SetRibalta(int RibaltaDa, int RibaltaA, string IdEnte)
        {
            try { 
                int esito = 0;
                DAO.TipoOccupazDAO DAO = new DAO.TipoOccupazDAO();
                esito = DAO.InsertRibalta(RibaltaDa, RibaltaA, IdEnte);
                return esito;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.SetRibalta.errore: ", Err);
                throw Err;
            }
        }

        #endregion

        #region "Private Method"

        private static DataTable GetAllTipologieOccupazioniFromDB (string IdEnte, string IdTributo)
		{
            try {
                DAO.TipoOccupazDAO DAO = new DAO.TipoOccupazDAO();
                return DAO.GetAllTipoOccupazioni(IdEnte, IdTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetAllTipologieOccupazioniFromDB.errore: ", Err);
                throw Err;
            }
        }

		private static DataTable GetAllCaefficientiFromDB (string IdEnte, string TipoTabella)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.GetAllCoefficienti(IdEnte, TipoTabella);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetAllCoefficientiFromDB.errore: ", Err);
                throw Err;
            }
        }

        private static DataTable GetTipologieOccupazioniFromDB(string ConnectionString, int Anno, string idEnte, string IdTributo)
		{
            try {
                DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();
                return DAO.GetTipologieOccupazioni(ConnectionString, -1, "", Anno, idEnte, IdTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetTipologieOccupazioniFromDB.errore: ", Err);
                throw Err;
            }
        }

        private static TipologieOccupazioni[] FillAllTipologiaOccupazioniFromDataTable(DataTable dt, bool defaultValue)
        {
            try {

                ArrayList MyArray = new ArrayList();
                TipologieOccupazioni CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new TipologieOccupazioni();

                    //FillTipologieOccupazioni
                    CurrentItem.IdTipologiaOccupazione = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];

                    MyArray.Add(CurrentItem);
                }

                if (defaultValue)
                {
                    //'Add default value
                    CurrentItem = new TipologieOccupazioni();

                    //FillCategorie
                    CurrentItem.IdTipologiaOccupazione = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }


                return (TipologieOccupazioni[])MyArray.ToArray(typeof(TipologieOccupazioni));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.FillTipologiaOccupazioniFromDataTable.errore: ", Err);
                throw Err;
            }
        }
		private static TipologieOccupazioni[] FillTipologiaOccupazioniFromDataTable(DataTable dt, bool defaultValue)
		{
            try {
                ArrayList MyArray = new ArrayList();
                TipologieOccupazioni CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new TipologieOccupazioni();

                    //FillTipologieOccupazioni
                    CurrentItem.IdTipologiaOccupazione = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.CoefficienteMoltiplicativo = double.Parse(myRow["COEFF_MOLTIPLICATIVO"].ToString());

                    MyArray.Add(CurrentItem);
                }

                if (defaultValue)
                {
                    //'Add default value
                    CurrentItem = new TipologieOccupazioni();

                    //FillCategorie
                    CurrentItem.IdTipologiaOccupazione = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }


                return (TipologieOccupazioni[])MyArray.ToArray(typeof(TipologieOccupazioni));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.FillTipologaeOccupazioniFromDataTable.errore: ", Err);
                throw Err;
            }
        }

		private static TipologieOccupazioni[] FillTipologiaOccupazioniFromDataTableForMotore(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                TipologieOccupazioni CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new TipologieOccupazioni();

                    //FillTipologieOccupazioni
                    CurrentItem.IdTributo = (string)myRow["idtributo"];
                    CurrentItem.IdTipologiaOccupazione = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.CoefficienteMoltiplicativo = double.Parse(myRow["COEFF_MOLTIPLICATIVO"].ToString());

                    MyArray.Add(CurrentItem);
                }


                return (TipologieOccupazioni[])MyArray.ToArray(typeof(TipologieOccupazioni));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.FillTipologiaOccupazioniFromDataTableForMotore.errore: ", Err);
                throw Err;
            }
        }

		private static Coefficienti[] FillAllCoefficienti(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Coefficienti CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Coefficienti();
                    //FillCategorie
                    CurrentItem.TipoTabella = "TipologieOccupazioni";
                    CurrentItem.IdTabella = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.Coefficiente = (decimal)myRow["COEFF_MOLTIPLICATIVO"];

                    MyArray.Add(CurrentItem);
                }

                return (Coefficienti[])MyArray.ToArray(typeof(Coefficienti));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.FillAllCoefficienti.errore: ", Err);
                throw Err;
            }
        }
		private static Coefficienti[] FillAllCoefficiente(DataTable dt)
		{
            try {

                ArrayList MyArray = new ArrayList();
                Coefficienti CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Coefficienti();
                    //FillCategorie
                    CurrentItem.TipoTabella = "TipologieOccupazioni";
                    CurrentItem.IdTabella = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.Coefficiente = (decimal)myRow["COEFF_MOLTIPLICATIVO"];

                    MyArray.Add(CurrentItem);
                }

                return (Coefficienti[])MyArray.ToArray(typeof(Coefficienti));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.FillAllCoefficiente.errore: ", Err);
                throw Err;
            }
        }

		private static DataTable GetCoefficienteFromDB(int IdTipologiaOccupazione, int Anno)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.GetCoefficiente(IdTipologiaOccupazione, Anno, "TipologieOccupazioni");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTipologieOccupazioni.GetCoefficienteFromDB.errore: ", Err);
                throw Err;
            }
        }

		#endregion
	}

}