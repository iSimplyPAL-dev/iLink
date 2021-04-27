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
    /// Classe per la gestione Categorie
    /// </summary>
	public class MetodiCategorie
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Categorie));
		#region "Public Method"

		public static int DeleteCategoria(int IdCategoria)
        {
            try {

                DAO.CategorieDAO DAO = new DAO.CategorieDAO();
                return DAO.DeleteCategoria(IdCategoria);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.DeleteCateogria.errore: ", Err);
                throw (Err);
            }
        }

		public static int DeleteCoefficiente(int IdCategoria, int Anno)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.DeleteCoefficiente(IdCategoria, Anno, "Categorie");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.DeleteCoefficiente.errore: ", Err);
                throw (Err);
            }

        }

        public static int InsertOrUpdateDescrizione(string ConnectionString, string IdEnte, string IdTributo, int IdCategoria, string Descrizione)
		{
            try {
                DAO.CategorieDAO DAO = new DAO.CategorieDAO();
                return DAO.InsertOrUpdateDescrizione(ConnectionString, IdEnte, IdTributo, IdCategoria, Descrizione);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.InsertOrUpdateDescrizione.errore: ", Err);
                throw (Err);
            }
        }

		public static int InsertOrUpdateCoefficiente(int IdCategoria, int Anno, decimal Coefficiente)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.InsertOrUpdateCoefficiente(IdCategoria, Anno, Coefficiente, "Categorie");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.InsertOrUpdateCoefficiente.errore: ", Err);
                throw (Err);
            }
        }

        public static Categorie[] GetCategorie(string ConnectionString, int Anno, bool DefaultValue, string idEnte, string IdTributo)
		{
			try 
			{
                DataTable dt = GetCategorieFromDB(ConnectionString,Anno, idEnte, IdTributo);
				return FillCategorieFromDataTable(dt, DefaultValue);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetCategorieFromDB.errore: ", Err);
                throw (Err);
            }
        }
		
		public static Categorie[] GetAllCategorie(string IdEnte, string IdTributo)
		{
			try 
			{
				DataTable dt = GetAllCategorieFromDB(IdEnte, IdTributo);
				return FillAllCategorie(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetAllCategorie.errore: ", Err);
                throw (Err);
            }
        }

		public static Coefficienti[] GetAllCoefficienti(string IdEnte)
		{
			try 
			{
				DataTable dt = GetAllCaefficientiFromDB(IdEnte, "Categorie");
				return FillAllCoefficienti(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetAllCoefficienti.errore: ", Err);
                throw (Err);
            }
        }

		public static Coefficienti GetCoefficiente(int IdCategoria, int Anno)
		{
			try 
			{
				DataTable dt = GetCoefficienteFromDB (IdCategoria, Anno);
				Coefficienti[] lista = FillAllCoefficiente(dt);
				if((lista != null) && (lista.Length>0))
					return lista[0];
				return null;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetCoefficiente.errore: ", Err);
                throw (Err);
            }
        }

        public static Categorie[] GetCategorieForMotore(string ConnectionString, int Anno, string idEnte, string IdTributo)
		{

			try 
			{
                DataTable dt = GetCategorieFromDB(ConnectionString,Anno, idEnte, IdTributo);
				return FillCategorieFromDataTableForMotore(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetCategorieForMotore.errore: ", Err);
                throw (Err);
            }
        }

        public static Categorie GetCategoria(string ConnectionString, int IdCategoria,string Descrizione, string idEnte, string IdTributo)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetCategorie(ConnectionString,IdCategoria, Descrizione, -1, idEnte, IdTributo);
				Categorie[] cat = FillCategorieFromDataTable(dt, false);
				return cat[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetCategoria.errore: ", Err);
                throw (Err);
            }

        }

		public static Categorie[] GetMotoreObjects (Categorie[] categ)
		{
            try {
                Categorie[] categMotore = new Categorie[categ.Length];
                int i = 0;
                foreach (Categorie myRow in categ)
                {
                    categMotore[i] = new Categorie();
                    categMotore[i].IdCategoria = myRow.IdCategoria;
                    categMotore[i].Descrizione = myRow.Descrizione;
                    categMotore[i].Coefficiente = myRow.Coefficiente;
                    i++;
                }

                return categMotore;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetMotoreObjects.errore: ", Err);
                throw (Err);
            }
        }

        public static int SetRibalta(int RibaltaDa, int RibaltaA, string IdEnte)
        {
            try
            {
                int esito = 0;
                DAO.CategorieDAO DAO = new DAO.CategorieDAO();
                esito = DAO.InsertRibalta(RibaltaDa, RibaltaA, IdEnte);
                return esito;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.SetRibalta.errore: ", Err);
                throw (Err);
            }
        }

        #endregion

        #region "Private Method"

        private static DataTable GetCategorieFromDB(string ConnectionString, int Anno, string idEnte, string IdTributo)
		{
            try {
                DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();
                return DAO.GetCategorie(ConnectionString, -1, "", Anno, idEnte, IdTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetCategorieFromDB.errore: ", Err);
                throw (Err);
            }
        }

		private static DataTable GetAllCategorieFromDB (string IdEnte, string IdTributo)
		{
            try {
                DAO.CategorieDAO DAO = new DAO.CategorieDAO();
                return DAO.GetAllCategorie(IdEnte, IdTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetAllCategorieFromDB.errore: ", Err);
                throw (Err);
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetAllcaefficientiFromDB.errore: ", Err);
                throw (Err);
            }
        }

		private static DataTable GetCoefficienteFromDB(int IdCategoria, int Anno)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.GetCoefficiente(IdCategoria, Anno, "Categorie");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.GetCoefficienteFromDB.errore: ", Err);
                throw (Err);
            }
        }

		private static Categorie[] FillCategorieFromDataTable(DataTable dt, bool defaultValue)
		{
            try
            {
                ArrayList MyArray = new ArrayList();
                Categorie CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Categorie();
                    //FillCategorie
                    CurrentItem.IdCategoria = (int)myRow["IdCategoria"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];
                    CurrentItem.Coefficiente = double.Parse(myRow["Coefficiente"].ToString());

                    MyArray.Add(CurrentItem);
                }

                if (defaultValue)
                {
                    //'Add default value
                    CurrentItem = new Categorie();

                    //FillCategorie
                    CurrentItem.IdCategoria = -1;
                    CurrentItem.Descrizione = "...";
                    MyArray.Insert(0, CurrentItem);
                }

                return (Categorie[])MyArray.ToArray(typeof(Categorie));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.FillCategorieFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

		private static Categorie[] FillCategorieFromDataTableForMotore(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Categorie CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Categorie();
                    //FillCategorie
                    CurrentItem.IdTributo = (string)myRow["idtributo"];
                    CurrentItem.IdCategoria = (int)myRow["IdCategoria"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];
                    CurrentItem.Coefficiente = double.Parse(myRow["Coefficiente"].ToString());

                    MyArray.Add(CurrentItem);
                }

                return (Categorie[])MyArray.ToArray(typeof(Categorie));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.FillCategorieFromDataTableForMotore.errore: ", Err);
                throw (Err);
            }
        }

		private static Categorie[] FillAllCategorie(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Categorie CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Categorie();
                    //FillCategorie
                    CurrentItem.IdCategoria = (int)myRow["IdCategoria"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];

                    MyArray.Add(CurrentItem);
                }

                return (Categorie[])MyArray.ToArray(typeof(Categorie));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.FillAllCategorie.errore: ", Err);
                throw (Err);
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
                    CurrentItem.TipoTabella = "Categorie";
                    CurrentItem.IdTabella = (int)myRow["IdCategoria"];
                    CurrentItem.Descrizione = (string)myRow["Descrizione"];
                    CurrentItem.Anno = (int)myRow["Anno"];
                    CurrentItem.Coefficiente = (decimal)myRow["Coefficiente"];

                    MyArray.Add(CurrentItem);
                }

                return (Coefficienti[])MyArray.ToArray(typeof(Coefficienti));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.FillAllCoefficienti.errore: ", Err);
                throw (Err);
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
                    CurrentItem.TipoTabella = "Categorie";
                    CurrentItem.IdTabella = (int)myRow["IdCategoria"];
                    CurrentItem.Anno = (int)myRow["Anno"];
                    CurrentItem.Coefficiente = (decimal)myRow["Coefficiente"];

                    MyArray.Add(CurrentItem);
                }

                return (Coefficienti[])MyArray.ToArray(typeof(Coefficienti));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCategorie.FillAllCoefficiente.errore: ", Err);
                throw (Err);
            }
        }

		#endregion
	}
}