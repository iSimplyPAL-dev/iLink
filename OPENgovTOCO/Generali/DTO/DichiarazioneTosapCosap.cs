using System;
using System.Data;
using System.Configuration;
using log4net;
using Anagrafica.DLL;
using IRemInterfaceOSAP;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione Dichiarazione
    /// </summary>
	public class MetodiDichiarazioneTosapCosap
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioneTosapCosap));
        private static SqlTransaction startTransazione;
        private static SqlCommand cmdMyCommand;

//		private DichiarazioneTosapCosapTestata _testataDichiarazione;
//		private DettaglioAnagrafica _anagraficaContribuente;
//		private Articolo[] _articoliDichiarazione;
//		private string _idEnte;
//		private string _codTributo;
//		private int _idDichiarazione;
//
//		public DichiarazioneTosapCosapTestata TestataDichiarazione
//		{
//			get { return _testataDichiarazione; }
//			set { _testataDichiarazione = value; }
//		}
//
//		public DettaglioAnagrafica AnagraficaContribuente
//		{
//			get { return _anagraficaContribuente; }
//			set { _anagraficaContribuente = value; }
//		}
//
//		public Articolo[] ArticoliDichiarazione
//		{
//			get { return _articoliDichiarazione; }
//			set { _articoliDichiarazione = value; }
//		}
//
//		public string IdEnte
//		{
//			get { return _idEnte; }
//			set { _idEnte = value; }
//		}
//
//		public string CodTributo
//		{
//			get { return _codTributo; }
//			set { _codTributo = value; }
//		}
//
//
//		public int IdDichiarazione
//		{
//			get { return _idDichiarazione; }
//			set { _idDichiarazione = value; }
//		}

		#region "Public Method"
        public static int SetDichiarazione(string ConnectionString, DichiarazioneTosapCosap dichiarazione)
		{
			DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

			try 
			{
				objDAO.SetDichiarazione( ConnectionString, ref dichiarazione);
                return dichiarazione.IdDichiarazione;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.SetDichiarazione.errore: ", Err);
                throw (Err);
            }
        }
        //public static void UpdateDichiarazione(DichiarazioneTosapCosap dichiarazione)
        //{
        //    DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

        //    DBEngine dbEngine_ = null;

        //    try
        //    {
        //        dbEngine_ = DAO.DBEngineFactory.GetDBEngine();
        //        dbEngine_.OpenConnection();
        //        dbEngine_.BeginTransaction();
        //        objDAO.UpdateDichiarazione(ref dbEngine_, dichiarazione);
        //        dbEngine_.CommitTransaction();  
        //    }
        //    catch (Exception ex)
        //    {
        //        dbEngine_.RollbackTransaction(); 
        //       Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.UpdateDichiarazione.errore: ", Err);
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        dbEngine_.CloseConnection();
        //    }
        //}
        public static void UpdateDichiarazione(DichiarazioneTosapCosap dichiarazione)
        {
            //*** 20140410
            DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();
            connessioneDB();
            //conn = new SqlConnection();
            startTransazione = cmdMyCommand.Connection.BeginTransaction();
            try
            {
                //conn.BeginTransaction();
                //startTransazione.Connection.BeginTransaction();
                cmdMyCommand.Transaction = startTransazione;
                objDAO.UpdateDichiarazione(ref cmdMyCommand, dichiarazione);

                startTransazione.Commit();
            }
            catch (Exception Err)
            {
                startTransazione.Rollback();
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.UpdateDichiarazione.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
        }
        public static DichiarazioneTosapCosap GetDichiarazione(string ConnectionString, int IdDichiarazione, string IdEnte, int Anno, string CodTributo)
		{
			DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

			try 
			{
                return objDAO.GetDichiarazione(ConnectionString,IdDichiarazione, IdEnte, Anno, CodTributo);
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.GetDichiarazione.errore: ", Err);
                throw (Err);
            }
        }
        //public static DichiarazioneTosapCosap GetDichiarazioneForMotore(int IdDichiarazione, string IdEnte, string CodTributo,int Anno, ref DBEngine dbEngine)
        //{
        //    DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

        //    try 
        //    {
        //        return objDAO.GetDichiarazioneForMotore(IdDichiarazione, IdEnte, CodTributo,Anno, ref dbEngine);
        //    } 
        //    catch (Exception ex) 
        //    {
        //      Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore.errore: ", Err); 
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDichiarazione"></param>
        /// <param name="IdEnte"></param>
        /// <param name="CodTributo"></param>
        /// <param name="Anno"></param>
        /// <param name="ListOccupazioni"></param>
        /// <param name="cmdMyCommand"></param>
        /// <returns></returns>
        public static DichiarazioneTosapCosap GetDichiarazioneForMotore(int IdDichiarazione, string IdEnte, string CodTributo, int Anno, string ListOccupazioni, ref SqlCommand cmdMyCommand)
        {
            DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

            try
            {
                return objDAO.GetDichiarazioneForMotore(IdDichiarazione, IdEnte, CodTributo, Anno, ListOccupazioni, ref cmdMyCommand);
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IdEnte"></param>
        /// <param name="CodTributo"></param>
        /// <returns></returns>
        public static string GetNDichAutomatico(string ConnectionString, string IdEnte, string CodTributo)
        {
            DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

            try
            {
                return objDAO.GetNDichAutomatico(ConnectionString,  IdEnte,  CodTributo);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.GetNDichAutomatico.errore: ", Err);
                throw (Err);
            }
        }
		public static DataTable SearchDichiarazione(DichiarazioneSearch SearchParams)
		{
			DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

			try 
			{
				DataTable dt = objDAO.SearchDichiarazioni(SearchParams);
				return dt;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.SearchDichiarazione.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myStringConnection">string</param>
        /// <param name="SearchParams"></param>
        /// <returns></returns>
            /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Esportazione completa dati</em>
    /// </revision>
    /// </revisionHistory>
public static DataTable PrintDichiarazioni(string myStringConnection,DichiarazioneSearch SearchParams)
        {
            try
            {
                DataTable dtStampa = new DataTable();
                DataSet dsStampa = new DataSet();
                DataView dvDati = new DAO.DichiarazioniDAO().PrintDichiarazioni(myStringConnection,SearchParams);
                int nCampi = 0;
                string sDatiStampa = "";

                nCampi = 21;
                dsStampa.Tables.Add("STAMPA");
                //carico le colonne nel dataset
                checked
                {
                    for (int x = 0; x <= nCampi + 1; x++)
                    {
                        dsStampa.Tables["STAMPA"].Columns.Add("Col" + x.ToString().PadLeft(3, char.Parse("0")));
                    }
                }
                //carico il datatable
                dtStampa = dsStampa.Tables["STAMPA"];
                //inserisco l//intestazione dell//ente
                sDatiStampa = DichiarazioneSession.IdEnte + " - " + DichiarazioneSession.DescrizioneEnte; ;
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco l//intestazione del report
                sDatiStampa = "Elenco Autorizzazioni/Concessioni";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco le intestazioni di colonna
                sDatiStampa = "";
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA";
                sDatiStampa += "|Via Res.|Civico Res.|Cap Res.|Comune Res.|Provincia Res.";
                sDatiStampa += "|N.Autoriz./Conces.|Data Autoriz./Conces.";
                sDatiStampa += "|Ubicazione|Tipologia Occupazione|Categoria|Consistenza|Data Inizio|Data Fine|Durata";
                sDatiStampa += "|Attrazione|Agevolazioni|Imp.Magg.|% Magg.|Imp.Detraz.";
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                if (dvDati != null)
                {
                    //ciclo sui dati da stampare
                    foreach (DataRowView myRow in dvDati)
                    {
                        sDatiStampa = PrintDichiarazioniRowDati(DichiarazioneSession.IdEnte, myRow);
                        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                            return null;
                    }
                }
                else
                {
                    Log.Debug("PrintDichiarazioni::dataset vuoto causa errore non posso stampare");
                }
                return dtStampa;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.PrintDichiarazioni.errore: ", Err);
                throw (Err);
            }
        }
        //public static DataTable PrintDichiarazioni(DichiarazioneSearch SearchParams)
        //{
        //    try
        //    {
        //        DataTable dtStampa = new DataTable();
        //        DataSet dsStampa = new DataSet();
        //        DataView dvDati = new DAO.DichiarazioniDAO().PrintDichiarazioni(SearchParams);
        //        int nCampi = 0;
        //        string sDatiStampa = "";

        //        nCampi = 21;
        //        dsStampa.Tables.Add("STAMPA");
        //        //carico le colonne nel dataset
        //        checked
        //        {
        //            for (int x = 0; x <= nCampi + 1; x++)
        //            {
        //                dsStampa.Tables["STAMPA"].Columns.Add("Col" + x.ToString().PadLeft(3, char.Parse("0")));
        //            }
        //        }
        //        //carico il datatable
        //        dtStampa = dsStampa.Tables["STAMPA"];
        //        //inserisco l//intestazione dell//ente
        //        sDatiStampa = DichiarazioneSession.IdEnte + " - " + DichiarazioneSession.DescrizioneEnte; ;
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco una riga vuota
        //        sDatiStampa = "";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco l//intestazione del report
        //        sDatiStampa = "Elenco Autorizzazioni/Concessioni";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco una riga vuota
        //        sDatiStampa = "";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco le intestazioni di colonna
        //        sDatiStampa = "";
        //        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA";
        //        sDatiStampa += "|Via Res.|Civico Res.|Cap Res.|Comune Res.|Provincia Res.";
        //        sDatiStampa += "|N.Autoriz./Conces.|Data Autoriz./Conces.";
        //        sDatiStampa += "|Ubicazione|Tipologia Occupazione|Categoria|Consistenza|Data Inizio|Data Fine|Durata";
        //        sDatiStampa += "|Attrazione|Agevolazioni|Imp.Magg.|% Magg.|Imp.Detraz.";
        //        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        if (dvDati != null)
        //        {
        //            //ciclo sui dati da stampare
        //            foreach (DataRowView myRow in dvDati)
        //            {
        //                sDatiStampa = "";
        //                if (myRow["COGNOME"] != null)
        //                    sDatiStampa += myRow["COGNOME"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["NOME"] != null)
        //                    sDatiStampa += "|" + myRow["NOME"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["cfpiva"] != null)
        //                    sDatiStampa += "|'" + myRow["cfpiva"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["VIA_RES"] != null)
        //                    sDatiStampa += "|" + myRow["VIA_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CIVICO_RES"] != null)
        //                    sDatiStampa += "|" + myRow["CIVICO_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CAP_RES"] != null)
        //                    sDatiStampa += "|" + myRow["CAP_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";

        //                if (myRow["COMUNE_RES"] != null)
        //                    sDatiStampa += "|" + myRow["COMUNE_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["PROVINCIA_RES"] != null)
        //                    sDatiStampa += "|" + myRow["PROVINCIA_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["NDICHIARAZIONE"] != null)
        //                    sDatiStampa += "|'" + myRow["NDICHIARAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DATADICHIARAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["DATADICHIARAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["UBICAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["UBICAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["TIPOLOGIAOCCUPAZIONE_DESC"] != null)
        //                    sDatiStampa += "|" + myRow["TIPOLOGIAOCCUPAZIONE_DESC"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CATEGORIA_DESC"] != null)
        //                    sDatiStampa += "|" + myRow["CATEGORIA_DESC"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CONSISTENZA"] != null)
        //                    sDatiStampa += "|" + myRow["CONSISTENZA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DATAINIZIOOCCUPAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["DATAINIZIOOCCUPAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DATAFINEOCCUPAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["DATAFINEOCCUPAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DURATA"] != null)
        //                    sDatiStampa += "|" + myRow["DURATA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["ATTRAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["ATTRAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["AGEVOLAZIONI"] != null)
        //                    sDatiStampa += "|" + myRow["AGEVOLAZIONI"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["MAGGIORAZIONE_IMPORTO"] != null)
        //                    sDatiStampa += "|" + myRow["MAGGIORAZIONE_IMPORTO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["MAGGIORAZIONE_PERC"] != null)
        //                    sDatiStampa += "|" + myRow["MAGGIORAZIONE_PERC"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DETRAZIONE_IMPORTO"] != null)
        //                    sDatiStampa += "|" + myRow["DETRAZIONE_IMPORTO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //                    return null;
        //            }
        //        }
        //        else
        //        {
        //            Log.Debug("PrintDichiarazioni::dataset vuoto causa errore non posso stampare");
        //        }
        //        return dtStampa;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.PrintDichiarazioni.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        public static string PrintDichiarazioniRowDati(string IdEnte,DataRowView myRow)
        {
                string sDatiStampa = "";
                
            try
            {
                 if (myRow["COGNOME"] != null)
                            sDatiStampa += myRow["COGNOME"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["NOME"] != null)
                            sDatiStampa += "|" + myRow["NOME"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["cfpiva"] != null)
                            sDatiStampa += "|'" + myRow["cfpiva"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["VIA_RES"] != null)
                            sDatiStampa += "|" + myRow["VIA_RES"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["CIVICO_RES"] != null)
                            sDatiStampa += "|" + myRow["CIVICO_RES"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["CAP_RES"] != null)
                            sDatiStampa += "|" + myRow["CAP_RES"].ToString();
                        else
                            sDatiStampa += "|";

                        if (myRow["COMUNE_RES"] != null)
                            sDatiStampa += "|" + myRow["COMUNE_RES"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["PROVINCIA_RES"] != null)
                            sDatiStampa += "|" + myRow["PROVINCIA_RES"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["NDICHIARAZIONE"] != null)
                            sDatiStampa += "|'" + myRow["NDICHIARAZIONE"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["DATADICHIARAZIONE"] != null)
                            sDatiStampa += "|" + myRow["DATADICHIARAZIONE"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["UBICAZIONE"] != null)
                            sDatiStampa += "|" + myRow["UBICAZIONE"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["TIPOLOGIAOCCUPAZIONE_DESC"] != null)
                            sDatiStampa += "|" + myRow["TIPOLOGIAOCCUPAZIONE_DESC"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["CATEGORIA_DESC"] != null)
                            sDatiStampa += "|" + myRow["CATEGORIA_DESC"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["CONSISTENZA"] != null)
                            sDatiStampa += "|" + myRow["CONSISTENZA"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["DATAINIZIOOCCUPAZIONE"] != null)
                            sDatiStampa += "|" + myRow["DATAINIZIOOCCUPAZIONE"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["DATAFINEOCCUPAZIONE"] != null)
                            sDatiStampa += "|" + myRow["DATAFINEOCCUPAZIONE"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["DURATA"] != null)
                            sDatiStampa += "|" + myRow["DURATA"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["ATTRAZIONE"] != null)
                            sDatiStampa += "|" + myRow["ATTRAZIONE"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["AGEVOLAZIONI"] != null)
                            sDatiStampa += "|" + myRow["AGEVOLAZIONI"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["MAGGIORAZIONE_IMPORTO"] != null)
                            sDatiStampa += "|" + myRow["MAGGIORAZIONE_IMPORTO"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["MAGGIORAZIONE_PERC"] != null)
                            sDatiStampa += "|" + myRow["MAGGIORAZIONE_PERC"].ToString();
                        else
                            sDatiStampa += "|";
                        if (myRow["DETRAZIONE_IMPORTO"] != null)
                            sDatiStampa += "|" + myRow["DETRAZIONE_IMPORTO"].ToString();
                        else
                            sDatiStampa += "|";
                return sDatiStampa;
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.PrintDichiarazioniRowDati.errore: ", Err);
                throw (Err);
            }
        }
        //public static int[] GetIdDichiarazioniAnno (int Anno, string IdEnte,int IdContribuente, ref DBEngine dbEngine)
        //{
        //    DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

        //    try 
        //    {
        //        DataTable dt = objDAO.GetIdDichiarazioniAnno(Anno, IdEnte,IdContribuente, ref dbEngine);
        //        int[] IdDichiarazioni = new int[dt.Rows.Count];
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //            IdDichiarazioni[i] = (int)dt.Rows[i]["IDDICHIARAZIONE"];
        //        return IdDichiarazioni;
        //    } 
        //    catch (Exception ex) 
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno.errore: ", Err);
        //        throw ex;
        //    }
        //}
        /**** 201810 - Calcolo Puntuale ****/
        public static int[] GetIdDichiarazioniAnno(int Anno, string IdEnte, string IdTributo, int IdContribuente,int IdDichiarazione,string ListOccupazioni, ref SqlCommand cmdMyCommand)
        {
            DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

            try
            {
                DataTable dt = objDAO.GetIdDichiarazioniAnno(Anno, IdEnte,IdTributo, IdContribuente,IdDichiarazione,ListOccupazioni, ref cmdMyCommand);
                int[] IdDichiarazioni = new int[dt.Rows.Count];
                int i = 0;
                foreach (DataRow myRow in dt.Rows)
                {
                    IdDichiarazioni[i] = (int)myRow["IDDICHIARAZIONE"];
                    i++;
                }
                return IdDichiarazioni;
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno.errore: ", Err);
                throw (Err);
            }
        }
		public static bool IsCartellaCreata(int IdDichiarazione)
		{
			DAO.DichiarazioniDAO objDAO = new DAO.DichiarazioniDAO();

			try 
			{
				return objDAO.IsCartellaCreata(IdDichiarazione);
			
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.IsCartellaCreata.errore: ", Err);
                throw (Err);
            }
        }
        public static void connessioneDB()
        {
            //*** 20140409
            try {
                cmdMyCommand = new SqlCommand();
                Log.Debug("DichiarazioneTosapCosap::connessioneDB::apertura della connessione al DB");

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(OPENgovTOCO.DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiDichiarazioneTosapCosap.connessioneDB.errore: ", Err);
                throw (Err);
            }
        }
		#endregion

		#region "Private Method"
	


		#endregion
	}
}