using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione Agevolazione
    /// </summary>
	public class MetodiAgevolazione
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Agevolazione));

        #region "Public Method"

        public static int DeleteAgevolazione(int IdAgevolazione)
        {
            try
            {
                DAO.AgevolazioniDAO DAO = new DAO.AgevolazioniDAO();
                return DAO.DeleteAgevolazione(IdAgevolazione);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.DeleteAgevolazione.errore: ", Err);
                throw (Err);
            }
        }


        public static int DeleteCoefficiente(int IdAgevolazione, int Anno)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.DeleteCoefficiente(IdAgevolazione, Anno, "Agevolazioni");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.DeleteCoefficiente.errore: ", Err);
                throw (Err);


            }
            
		}

		public static int InsertOrUpdateDescrizione(string IdEnte, int IdAgevolazione, string Descrizione)
        {
            try
            {
                DAO.AgevolazioniDAO DAO = new DAO.AgevolazioniDAO();
                return DAO.InsertOrUpdateDescrizione(IdEnte, IdAgevolazione, Descrizione);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.InsertOrUpdateDescrizione.errore: ", Err);
                throw (Err);


            }
           
		}

		public static int InsertOrUpdateCoefficiente(int IdAgevolazioni, int Anno, decimal Coefficiente)
        {
            try
            {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.InsertOrUpdateCoefficiente(IdAgevolazioni, Anno, Coefficiente, "Agevolazioni");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.InsertOrUpdateCoefficiente.errore: ", Err);
                throw (Err);


            }
         
		}

		public static Agevolazione[] GetAllAgevolazioni(string IdEnte)
		{
			try 
			{
				DataTable dt = GetAllAgevolazioniFromDB (IdEnte);
				return FillAllAgevolazioniFromDataTable(dt, false);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAllAgevolazioni.errore: ", Err);
                throw (Err);


            }
        }

		public static Coefficienti[] GetAllCoefficienti(string IdEnte)
		{
			try 
			{
				DataTable dt = GetAllCaefficientiFromDB(IdEnte, "Agevolazioni");
				return FillAllCoefficienti(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAllCoefficienti.errore: ", Err);
                throw (Err);


            }
        }

		public static Agevolazione[] GetAgevolazioni(string Anno, int nIdArticolo, string idEnte)
		{
			try 
			{
                DataTable dt = GetAgevolazioniFromDB(OPENgovTOCO.DichiarazioneSession.StringConnection,Anno, nIdArticolo, idEnte, OPENgovTOCO.DichiarazioneSession.CodTributo(""));
				return FillAgevolazioniFromDataTable(dt, false);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAgevolazioni.errore: ", Err);
                throw (Err);


            }
        }

		public static Coefficienti GetCoefficiente(int IdAgevolazioni, int Anno)
		{
			try 
			{
				DataTable dt = GetCoefficienteFromDB (IdAgevolazioni, Anno);
				Coefficienti[] lista = FillAllCoefficiente(dt);
				if((lista != null) && (lista.Length>0))
					return lista[0];
				return null;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetCoefficiente.errore: ", Err);
                throw (Err);


            }
        }

        public static Agevolazione[] GetAgevolazioniForMotore(string ConnectionString, string Anno, string idEnte, string IdTributo)
		{
			try 
			{
                DataTable dt = GetAgevolazioniFromDB(ConnectionString,Anno, -1, idEnte, IdTributo);
				return FillAgevolazioniFromDataTableForMotore(dt);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAgevolazioniForMotore.errore: ", Err);
                throw (Err);


            }
        }

        public static Agevolazione[] GetAgevolazioniVSArticolo(string ConnectionString, int IdArticolo, int IdRuolo, string idEnte, string CodTributo)
		{
			DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

			try 
			{
				DataTable dt = null;
                dt = DAO.GetAgevolazioni(ConnectionString ,- 1, "", IdArticolo,IdRuolo, true, idEnte, CodTributo);
				Agevolazione[] tipi = FillAgevolazioniFromDataTable(dt, false);
				return tipi;
			}
            catch (Exception Err)
            {
            Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAgevolazioniVSArticolo.errore: ", Err);
            throw (Err);
            }

        }

		//*** 20130610 - ruolo supplettivo ***
		public static string GetElencoPercAgevolazioni(Agevolazione[] myList)
		{
			string myReturn=string.Empty;
			try 
			{
				foreach (Agevolazione a in myList )
				{
					if (a.Selezionato==true)
					{
						if (myReturn!=string.Empty)
							myReturn+="-";
						myReturn+=a.AgevolazionePerc.ToString()+"%";
					}
				}
				return myReturn;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetElencoPercAgevolazioni.errore: ", Err);
                throw (Err);
            }

        }


        //public static Agevolazione[] GetAgevolazioniVSArticolo(string sIdEnte,int IdArticolo, ref DAL.DBEngine dbEngine_)
        //{
        //    DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

        //    try 
        //    {
        //        DataTable dt = null;
        //        dt = DAO.GetAgevolazioni(sIdEnte,-1, "",IdArticolo,true, ref dbEngine_);
        //        Agevolazione[] tipi = FillAgevolazioniFromDataTable(dt, false);
        //        return tipi;
        //    } 
        //    catch (Exception ex) 
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAgevolazioniVSArticolo.errore: ", Err);
        //        throw ex;
        //    }

        //}

        public static Agevolazione[] GetAgevolazioniVSArticolo(string ConnectionString,string sIdEnte, int IdArticolo, int IdRuolo)
        {
            //*** 20140409
            DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetAgevolazioni(ConnectionString,-1,"", IdArticolo,IdRuolo, true,sIdEnte, "");
                Agevolazione[] tipi = FillAgevolazioniFromDataTable(dt, false);
                return tipi;
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAgevolazioniVSArticolo.errore: ", Err);
                throw (Err);
            }
        }

        public static int SetRibalta(int RibaltaDa, int RibaltaA, string IdEnte)
        {
            try { 
                int esito = 0;
                DAO.AgevolazioniDAO DAO = new DAO.AgevolazioniDAO();
                esito = DAO.InsertRibalta(RibaltaDa, RibaltaA, IdEnte);
                return esito;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.SetRibalta.errore: ", Err);
                throw (Err);
            }
        }
        #endregion

        #region "Private Method"

        private static DataTable GetAllAgevolazioniFromDB (string IdEnte)
		{
            try {
                DAO.AgevolazioniDAO DAO = new DAO.AgevolazioniDAO();
                return DAO.GetAllAgevolazioni(IdEnte);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAllAgevolazioniFromDB.errore: ", Err);
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAllCoefficientiFromDB.errore: ", Err);
                throw (Err);
            }
            
		}

		private static DataTable GetCoefficienteFromDB(int IdAgevolazione, int Anno)
		{
            try {
                DAO.CoefficientiDAO DAO = new DAO.CoefficientiDAO();
                return DAO.GetCoefficiente(IdAgevolazione, Anno, "Agevolazioni");
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetCoefficientiFromDB.errore: ", Err);
                throw (Err);
            }
        }

        private static DataTable GetAgevolazioniFromDB(string ConnectionString, string Anno, int nIdArticolo, string idEnte, string IdTributo)
		{
            try
            {
                DAO.AnagraficheDAO DAO = new DAO.AnagraficheDAO();
                return DAO.GetAgevolazioni(ConnectionString, -1, Anno, nIdArticolo,-1, false, idEnte, IdTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.GetAgevolazioniFromDB.errore: ", Err);
                throw (Err);
            }

        }

		private static Agevolazione[] FillAllAgevolazioniFromDataTable(DataTable dt, bool defaultValue)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Agevolazione CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Agevolazione();

                    //FillTipologieOccupazioni
                    CurrentItem.IdAgevolazione = (int)myRow["IDAGEVOLAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];

                    MyArray.Add(CurrentItem);
                }

                return (Agevolazione[])MyArray.ToArray(typeof(Agevolazione));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.FillAllAgevolazioniFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

		private static Agevolazione[] FillAgevolazioniFromDataTable(DataTable dt, bool defaultValue)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Agevolazione CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Agevolazione();

                    //FillTipologieOccupazioni
                    CurrentItem.IdAgevolazione = (int)myRow["IDAGEVOLAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.AgevolazionePerc = double.Parse(myRow["AGEVOLAZIONE_PERC"].ToString());
                    CurrentItem.Selezionato = (bool)myRow["selezionato"];

                    MyArray.Add(CurrentItem);
                }

                return (Agevolazione[])MyArray.ToArray(typeof(Agevolazione));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.FillAgevolazioniFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

		private static Agevolazione[] FillAgevolazioniFromDataTableForMotore(DataTable dt)
		{
            try {
                ArrayList MyArray = new ArrayList();
                Agevolazione CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Agevolazione();

                    //FillTipologieOccupazioni
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    CurrentItem.IdAgevolazione = (int)myRow["IDAGEVOLAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.AgevolazionePerc = double.Parse(myRow["AGEVOLAZIONE_PERC"].ToString());

                    MyArray.Add(CurrentItem);
                }

                return (Agevolazione[])MyArray.ToArray(typeof(Agevolazione));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.FillAgevolazioniFromDataTableForMotore.errore: ", Err);
                throw (Err);
            }
        }

        private static Coefficienti[] FillAllCoefficienti(DataTable dt)
        {
            try
            {
                ArrayList MyArray = new ArrayList();
                Coefficienti CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Coefficienti();
                    //FillCategorie
                    CurrentItem.TipoTabella = "Agevolazioni";
                    CurrentItem.IdTabella = (int)myRow["IDAGEVOLAZIONE"];
                    CurrentItem.Descrizione = (string)myRow["DESCRIZIONE"];
                    CurrentItem.Anno = (int)myRow["Anno"];
                    CurrentItem.Coefficiente = (decimal)myRow["AGEVOLAZIONE_PERC"];

                    MyArray.Add(CurrentItem);
                }

                return (Coefficienti[])MyArray.ToArray(typeof(Coefficienti));

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.FillAllCoefficienti ", Err);
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
                    CurrentItem.TipoTabella = "Agevolazioni";
                    CurrentItem.IdTabella = (int)myRow["IDAGEVOLAZIONE"];
                    CurrentItem.Anno = (int)myRow["Anno"];
                    CurrentItem.Coefficiente = (decimal)myRow["AGEVOLAZIONE_PERC"];

                    MyArray.Add(CurrentItem);
                }

                return (Coefficienti[])MyArray.ToArray(typeof(Coefficienti));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiAgevolazione.FillAllCoefficiente.errore: ", Err);
                throw (Err);
            }
        }

		#endregion

	}
}
