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
    /// Classe per la gestione tariffe
    /// </summary>
	public class MetodiTariffe
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Tariffe));
		#region "Public methods"
        public static string[] GetAnniTariffe(string IdEnte, string IdTributo, bool DefaultValue)
        {
            try
            {
                DataTable dt = null;
                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                dt = DAO.GetAnniTariffe(IdEnte,IdTributo);
                return FillAnniTariffeFromDataTable(dt, DefaultValue);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetAnniTariffe.errore: ", Err);
                throw Err;
            }
        }

		public static Tariffe[] GetTariffe (int IdTariffa, string IdEnte, string IdTributo, int IdCategoria, int IdTipologiaOccupazione, int IdDurata, int Anno)
		{
			try
			{
				DataTable dt = GetAllTariffeFromDB (IdEnte, IdTributo, IdCategoria, IdTipologiaOccupazione, IdDurata, Anno);
				return FillTariffeFromDataTable(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetTariffe.errore: ", Err);
                throw Err;
            }
        }

		public static Tariffe[] GetTariffa (int IdTariffa)
		{
			try 
			{
				DataTable dt = GetTariffaFromDB (IdTariffa);;
				return FillTariffeFromDataTable(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetTariffa.errore: ", Err);
                throw Err;
            }
        }

		public static int DeleteTariffa(int IdTariffa)
		{
            try {
                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                return DAO.DeleteTariffa(IdTariffa);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.DeleteTariffa.errore: ", Err);
                throw Err;
            }
        }

        public static int InsertTariffa(string ConnectionString, string IdEnte, string IdTributo, int IdCategoria, int IdTipologiaOccupazione, int IdDurata, int Anno, decimal Valore, decimal MinimoApplicabile)
        {
            try {
                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                return DAO.InsertTariffa(ConnectionString, IdEnte, IdTributo, IdCategoria, IdTipologiaOccupazione, IdDurata, Anno, Valore, MinimoApplicabile);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.InsertTariffa.errore: ", Err);
                throw Err;
            }
        }


		public static int UpdateTariffa(int IdTariffa, decimal Valore, decimal MinimoApplicabile)
		{

            try {
                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                return DAO.UpdateTariffa(IdTariffa, Valore, MinimoApplicabile);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.UpdateTariffa.errore: ", Err);
                throw Err;
            }
        }

		public static Tariffe[] GetTariffe (int Anno)
		{
			try 
			{
                DataTable dt = GetTariffeFromDB(Anno, OPENgovTOCO.DichiarazioneSession.CodTributo("")); ;
				return FillTariffeFromDataTable(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetTariffe.errore: ", Err);
                throw Err;
            }
        }

        public static Tariffe[] GetTariffeForMotore(int Anno, string IdTributo)
		{
			try 
			{
				DataTable dt = GetTariffeFromDB (Anno,IdTributo);
				return FillTariffeFromDataTableForMotore(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetTariffeForMotore.errore: ", Err);
                throw Err;
            }
        }

        public static Tariffe GetTariffa(string ConnectionString,string IdEnte, int Anno, int IdCategoria, int IdTipologiaOccupazione, string IdTributo)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetTariffa( ConnectionString,IdEnte, Anno, IdCategoria, IdTipologiaOccupazione, IdTributo);
				Tariffe[] tar = FillTariffeFromDataTable(dt);
				return tar[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetTariffa.errore: ", Err);
                throw Err;
            }
        }

        public static int SetRibalta(int RibaltaDa, int RibaltaA, string IdEnte)
        {
            try {
                int esito = 0;
                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                esito = DAO.InsertRibalta(RibaltaDa, RibaltaA, IdEnte);
                return esito;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.SetRibalta.errore: ", Err);
                throw Err;
            }
        }
        #endregion

        #region "Private Method"

        private static DataTable GetTariffeFromDB(int Anno, string IdTributo)
        {
            try {
                DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();
                return DAO.GetTariffa(OPENgovTOCO.DichiarazioneSession.StringConnection, OPENgovTOCO.DichiarazioneSession.IdEnte, Anno, -1, -1, IdTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetAllTariffeFromDB.errore: ", Err);
                throw Err;
            }
        }

		private static DataTable GetAllTariffeFromDB(string IdEnte, string IdTributo, int IdCategoria, int IdTipologiaOccupazione, int IdDurata, int Anno)
		{
            try {
                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                return DAO.GetAllTariffe(IdEnte, IdTributo, IdCategoria, IdTipologiaOccupazione, IdDurata, Anno);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetAllTariffeFromDB.errore: ", Err);
                throw Err;
            }
        }

		private static DataTable GetTariffaFromDB(int IdTariffa)
		{
            try
            {


                DAO.TariffeDAO DAO = new DAO.TariffeDAO();
                return DAO.GetTariffa(IdTariffa);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.GetTariffaFromDB.errore: ", Err);
                throw Err;
            }
        }

        private static string[] FillAnniTariffeFromDataTable(DataTable dt, bool DefaultValue)
        {
            try {
                ArrayList MyArray = new ArrayList();

                foreach (DataRow myRow in dt.Rows)
                {
                    MyArray.Add(myRow["ANNO"].ToString());
                }

                if (DefaultValue)
                {
                    //'Add default value
                    MyArray.Insert(0, "...");
                }

                return (string[])MyArray.ToArray(typeof(string));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.FillAnniTariffeFromDataTable.errore: ", Err);
                throw Err;
            }
        }

		private static Tariffe[] FillTariffe(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Tariffe CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Tariffe();
                    CurrentItem.IdTariffa = (int)myRow["IDTARIFFA"];
                    CurrentItem.IdCategoria = (int)myRow["IDCATEGORIA"];
                    CurrentItem.Categoria = myRow["Categoria"].ToString();
                    CurrentItem.IdTipologiaOccupazione = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.TipologiaOccupazione = myRow["TipologiaOccupazione"].ToString();
                    CurrentItem.IdDurata = (int)myRow["IdDurata"];
                    CurrentItem.Durata = myRow["Durata"].ToString();
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.Valore = double.Parse(myRow["VALORE"].ToString());
                    CurrentItem.MinimoApplicabile = double.Parse(myRow["MINIMOAPPLICABILE"].ToString());

                    MyArray.Add(CurrentItem);
                }

                return (Tariffe[])MyArray.ToArray(typeof(Tariffe));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.FillTariffe.errore: ", Err);
                throw Err;
            }
        }

		private static Tariffe[] FillTariffeFromDataTable(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Tariffe CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Tariffe();

                    CurrentItem.IdTariffa = (int)myRow["IDTARIFFA"];
                    CurrentItem.IdCategoria = (int)myRow["IDCATEGORIA"];
                    CurrentItem.Categoria = myRow["Categoria"].ToString();
                    CurrentItem.IdTipologiaOccupazione = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.TipologiaOccupazione = myRow["TipologiaOccupazione"].ToString();
                    CurrentItem.IdDurata = (int)myRow["IdDurata"];
                    CurrentItem.Durata = myRow["Durata"].ToString();
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.Valore = double.Parse(myRow["VALORE"].ToString());
                    CurrentItem.MinimoApplicabile = double.Parse(myRow["MINIMOAPPLICABILE"].ToString());
                    CurrentItem.DescrTributo = myRow["descrtributo"].ToString();

                    MyArray.Add(CurrentItem);
                }

                return (Tariffe[])MyArray.ToArray(typeof(Tariffe));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.FillTariffeFromDataTable.errore: ", Err);
                throw Err;
            }
        }

		private static Tariffe[] FillTariffeFromDataTableForMotore(DataTable dt)
		{
            try
            {
                ArrayList MyArray = new ArrayList();
                Tariffe CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Tariffe();
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    CurrentItem.IdTariffa = (int)myRow["IDTARIFFA"];
                    CurrentItem.IdCategoria = (int)myRow["IDCATEGORIA"];
                    CurrentItem.IdTipologiaOccupazione = (int)myRow["IDTIPOLOGIAOCCUPAZIONE"];
                    CurrentItem.Anno = (int)myRow["ANNO"];
                    CurrentItem.Valore = double.Parse(myRow["VALORE"].ToString());
                    CurrentItem.MinimoApplicabile = double.Parse(myRow["MINIMOAPPLICABILE"].ToString());
                    //*** 20130412 - la tariffa è legata a tipo occupazione+categoria+durata ***
                    CurrentItem.IdDurata = (int)myRow["IdDurata"];

                    MyArray.Add(CurrentItem);
                }

                return (Tariffe[])MyArray.ToArray(typeof(Tariffe));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiTariffe.FillTariffeFromDataTableForMotore.errore: ", Err);
                throw Err;
            }
        }

		#endregion
	}

}