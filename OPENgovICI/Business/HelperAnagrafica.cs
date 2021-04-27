using System;
using System.Data;
using AnagInterface;
using Anagrafica.DLL;
using Ribes;
using log4net;
using log4net.Config;

namespace Business
{
    /// <summary>
    /// Classe per l'interfacciamento con le funzioni anagrafiche
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class HelperAnagrafica
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(HelperAnagrafica));

		/// <summary>
		/// TEST: Inizializzatore degli oggetti per l'anagrafica
		/// </summary>
		/// <returns></returns>
		private static GestioneAnagrafica InitModuloAnagrafica()
		{
            try { 
            //Ribes.CreateSessione RibesSession = new Ribes.CreateSessione(ConstWrapper.ParametroENV, ConstWrapper.UserNameFramework, ConstWrapper.ParametroAPPLICATION);
			
            //Anagrafica.DLL.GestioneAnagrafica retVal = new Anagrafica.DLL.GestioneAnagrafica(RibesSession.oSession, "0");

            ////CreateSessione RibesSession = new CreateSessione(ConstWrapper.ParametroENV, ConstWrapper.UserNameFramework, ConstWrapper.ParametroAPPLICATION);
            //string error = "";
            //if(RibesSession.CreaSessione(ConstWrapper.UserNameFramework, ref error))
            //{
            //    retVal = new GestioneAnagrafica(RibesSession.oSession, ConstWrapper.ParametroAnagrafica);
            //}
			//***non posso chiudere quì la connessione altrimenti AggiungiInAnagrafica non funziona*** 
			//RibesSession.Kill ();
			//****************************************************************************************
            Anagrafica.DLL.GestioneAnagrafica retVal;
            retVal = new GestioneAnagrafica();
			return retVal;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.InitModuloAnagrafica.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// Metodo helper per l'interfacciamento alla ricerca in Anagrafica
        /// </summary>
        /// <param name="cognome">Cognome</param>
        /// <param name="nome">Nome</param>
        /// <param name="codiceFiscale">Codice Fiscale</param>
        /// <param name="partitaIva">Partita IVA</param>
        /// <returns>DataTable</returns>
        public static DataTable RicercaPersona(string cognome, string nome, string codiceFiscale, string partitaIva)
        {
            DataTable retVal = new DataTable();
            try
            {
                //GestioneAnagrafica objAnagrafica = new GestioneAnagrafica(RibesSession.oSession, ConstWrapper.ParametroAnagrafica);
                //DataSet Result = objAnagrafica.GetListaPersone(cognome, nome, codiceFiscale, partitaIva);
                GestioneAnagrafica objAnagrafica = new GestioneAnagrafica();
                DataSet Result = objAnagrafica.GetListaPersone(-1, cognome, nome, codiceFiscale, partitaIva, ConstWrapper.CodiceEnte, ConstWrapper.DBType, ConstWrapper.StringConnectionAnagrafica);
                if (Result != null)
                    retVal = Result.Tables[0];
                return retVal;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.RicercaPersona.errore: ", Err);
                throw Err;

            }
        }


        /// <summary>
        /// Metodo helper per l'interfacciamento alla ricerca in Anagrafica
        /// </summary>
        /// <param name="cognome">Cognome</param>
        /// <param name="nome">Nome</param>
        /// <param name="codiceFiscale">Codice Fiscale</param>
        /// <param name="partitaIva">Partita IVA</param>
        /// <param name="codEnte">Codice Ente</param>
        /// <returns>DataTable</returns>
        public static DataTable RicercaPersona(string cognome, string nome, string codiceFiscale, string partitaIva, string codEnte)
		{
			DataTable retVal = new DataTable();
            try { 
                //GestioneAnagrafica objAnagrafica = new GestioneAnagrafica(RibesSession.oSession, ConstWrapper.ParametroAnagrafica);
                //DataSet Result = objAnagrafica.GetListaPersone(cognome, nome, codiceFiscale, partitaIva, codEnte);
                GestioneAnagrafica objAnagrafica = new GestioneAnagrafica();
                DataSet Result = objAnagrafica.GetListaPersone(-1,cognome, nome, codiceFiscale, partitaIva, codEnte, ConstWrapper.DBType, ConstWrapper.StringConnectionAnagrafica);
                if (Result != null)
					retVal = Result.Tables[0];
			return retVal;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.RicercaPersona.errore: ", Err);
                throw Err;

            }
        }

        /// <summary>
        /// Metodo per estrarre i dati di un singolo individuo identificato dal codice contribuente
        /// </summary>
        /// <param name="codiceContribuente">CodiceContribuente</param>
        /// <returns>DettaglioAnagrafica</returns>
        public static DettaglioAnagrafica GetDatiPersona(long codiceContribuente)
        {
            DettaglioAnagrafica RigaAnagrafica = new DettaglioAnagrafica();

            try
            {
                GestioneAnagrafica objAnagrafica = new GestioneAnagrafica();
                RigaAnagrafica = objAnagrafica.GetAnagrafica((long)codiceContribuente, -1, string.Empty, ConstWrapper.DBType, ConstWrapper.StringConnectionAnagrafica, false);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.RicercaPersona.errore: ", Err);
                throw Err;
            }
            return RigaAnagrafica;
        }
		/// <summary>
		/// Aggiunge un record all'anagrafica
		/// </summary>
		/// <param name="datiPersona">datiPersona</param>
		/// <returns>int</returns>
		public static int AggiungiInAnagrafica(DettaglioAnagrafica datiPersona)
		{
			GestioneAnagrafica objAnagrafica = InitModuloAnagrafica();
			datiPersona.COD_CONTRIBUENTE = -1;
			//return (int)objAnagrafica.SetAnagrafica(datiPersona);
            return (int)objAnagrafica.SetAnagrafica(datiPersona, ConstWrapper.DBType, ConstWrapper.StringConnectionAnagrafica);
		}


		/// <summary>
		/// Apertura del popup Anagrafica pewr la ricerca
		/// </summary>
		/// <param name="page"></param>
		/// <param name="chiaveSessione"></param>
		public static void PopUpAnagrafica(System.Web.UI.Page page, string chiaveSessione)
		{
            //Anagrafica.DLL.GestioneAnagrafica oAnagrafica; 
			DettaglioAnagrafica oDettaglioAnagrafica;
            try { 
			// Valorizzazione delle variabili di sessione necessaria all'anagrafica
			System.Web.HttpContext.Current.Session["modificaDiretta"] = false;
			System.Web.HttpContext.Current.Session["PARAMETROENV"] = ConstWrapper.ParametroENV;
			System.Web.HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"] = ConstWrapper.ParametroAPPLICATION;
            //System.Web.HttpContext.Current.Session["username"] = ConstWrapper.UserNameFramework;
			System.Web.HttpContext.Current.Session["COD_TRIBUTO"] = ConstWrapper.CodiceTributoAnag;
			System.Web.HttpContext.Current.Session["CODENTE"] = ConstWrapper.CodiceEnte;
			System.Web.HttpContext.Current.Session["Anagrafica"] = ConstWrapper.ParametroAnagrafica;

			//oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConstWrapper.ParametroAnagrafica); 
            //oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(); 
			oDettaglioAnagrafica = new DettaglioAnagrafica(); 
			oDettaglioAnagrafica.COD_CONTRIBUENTE = -1; 
			oDettaglioAnagrafica.ID_DATA_ANAGRAFICA =-1; 
			 
			System.Web.HttpContext.Current.Session[chiaveSessione] = oDettaglioAnagrafica; 
			writeJavascript(page, chiaveSessione);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.PopUpAnagrafica.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="page"></param>
		/// <param name="chiaveSessione"></param>
		/// <param name="COD_CONTRIBUENTE"></param>
		/// <param name="ID_DATA_ANAGRAFICA"></param>
		public static void PopUpAnagrafica(System.Web.UI.Page page, string chiaveSessione, string COD_CONTRIBUENTE, string ID_DATA_ANAGRAFICA)
		{
            //Anagrafica.DLL.GestioneAnagrafica oAnagrafica; 
			DettaglioAnagrafica oDettaglioAnagrafica;

			// Valorizzazione delle variabili di sessione necessaria all'anagrafica
			System.Web.HttpContext.Current.Session["modificaDiretta"] = false;
			System.Web.HttpContext.Current.Session["PARAMETROENV"] = ConstWrapper.ParametroENV;
			System.Web.HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"] = ConstWrapper.ParametroAPPLICATION;
			System.Web.HttpContext.Current.Session["username"] = ConstWrapper.UserNameFramework;
			System.Web.HttpContext.Current.Session["COD_TRIBUTO"] = ConstWrapper.CodiceTributoAnag;
			System.Web.HttpContext.Current.Session["CODENTE"] = ConstWrapper.CodiceEnte;
			System.Web.HttpContext.Current.Session["Anagrafica"] = ConstWrapper.ParametroAnagrafica;

			//oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConstWrapper.ParametroAnagrafica); 
			oDettaglioAnagrafica = new DettaglioAnagrafica();
            try { 
			if (COD_CONTRIBUENTE != "" && COD_CONTRIBUENTE != "0")
				oDettaglioAnagrafica.COD_CONTRIBUENTE = int.Parse(COD_CONTRIBUENTE);
			else
				oDettaglioAnagrafica.COD_CONTRIBUENTE = -1;
			
			if (ID_DATA_ANAGRAFICA != "")
				oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = int.Parse(ID_DATA_ANAGRAFICA);
			else
				oDettaglioAnagrafica.ID_DATA_ANAGRAFICA =-1; 
			 
			System.Web.HttpContext.Current.Session[chiaveSessione] = oDettaglioAnagrafica; 
			
			writeJavascript(page, chiaveSessione);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.PopUpAnagrafica.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Metodo di supporto all'interfacciamento con l'anagrafica ribes
		/// Scrive il Javascript necessario alla pagina epr aprire il popup
		/// </summary>
		/// <param name="page"></param>
		/// <param name="nomeSessione">nomeSessione</param>
		private static void writeJavascript(System.Web.UI.Page page,  string nomeSessione) 
		{ 
			string strscript; 
			strscript = ""; 
			strscript = "ApriRicercaAnagrafe('" + nomeSessione + "');"; 
			//strscript = "ApriRicercaAnagrafeCalcoloIci('" + nomeSessione + "');"; 
			page.RegisterStartupScript("", "<script language='javascript'>" + strscript + "</script>"); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="page"></param>
		/// <param name="chiaveSessione"></param>
		public static void PopUpAnagraficaCalcoloIci(System.Web.UI.Page page, string chiaveSessione)
		{
            //Anagrafica.DLL.GestioneAnagrafica oAnagrafica; 
			DettaglioAnagrafica oDettaglioAnagrafica;
            try { 
			// Valorizzazione delle variabili di sessione necessaria all'anagrafica
			System.Web.HttpContext.Current.Session["modificaDiretta"] = false;
			System.Web.HttpContext.Current.Session["PARAMETROENV"] = ConstWrapper.ParametroENV;
			System.Web.HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"] = ConstWrapper.ParametroAPPLICATION;
			System.Web.HttpContext.Current.Session["username"] = ConstWrapper.UserNameFramework;
			System.Web.HttpContext.Current.Session["COD_TRIBUTO"] = ConstWrapper.CodiceTributoAnag;
			System.Web.HttpContext.Current.Session["CODENTE"] = ConstWrapper.CodiceEnte;
			System.Web.HttpContext.Current.Session["Anagrafica"] = ConstWrapper.ParametroAnagrafica;

            //oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConstWrapper.ParametroAnagrafica); 
			oDettaglioAnagrafica = new DettaglioAnagrafica(); 
			oDettaglioAnagrafica.COD_CONTRIBUENTE = -1; 
			oDettaglioAnagrafica.ID_DATA_ANAGRAFICA =-1; 
			 
			System.Web.HttpContext.Current.Session[chiaveSessione] = oDettaglioAnagrafica; 
			
			writeJavascriptCalcoloIci(page, chiaveSessione);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.PopUpAnagraficaCalcoloIci.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Metodo di supporto all'interfacciamento con l'anagrafica ribes
		/// Scrive il Javascript necessario alla pagina per aprire il popup
		/// </summary>
		/// <param name="page"></param>
		/// <param name="nomeSessione">nomeSessione</param>
		private static void writeJavascriptCalcoloIci(System.Web.UI.Page page,  string nomeSessione) 
		{ 
			string strscript; 
			strscript = ""; 
			strscript = "ApriRicercaAnagrafeCalcoloIci('" + nomeSessione + "');"; 
			page.RegisterStartupScript("", "<script language='javascript'>" + strscript + "</script>"); 
		}

		/// <summary>
		/// Apertura del popup Anagrafica pewr la ricerca anagrafica nelle compensazioni
		/// </summary>
		/// <param name="page">page</param>
		/// <param name="chiaveSessione"></param>
		public static void PopUpAnagraficaCompensazione(System.Web.UI.Page page, string chiaveSessione)
		{
            //Anagrafica.DLL.GestioneAnagrafica oAnagrafica; 
			DettaglioAnagrafica oDettaglioAnagrafica;
            try { 
			// Valorizzazione delle variabili di sessione necessaria all'anagrafica
			System.Web.HttpContext.Current.Session["modificaDiretta"] = false;
			System.Web.HttpContext.Current.Session["PARAMETROENV"] = ConstWrapper.ParametroENV;
			System.Web.HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"] = ConstWrapper.ParametroAPPLICATION;
			System.Web.HttpContext.Current.Session["username"] = ConstWrapper.UserNameFramework;
			System.Web.HttpContext.Current.Session["COD_TRIBUTO"] = ConstWrapper.CodiceTributoAnag;
			System.Web.HttpContext.Current.Session["CODENTE"] = ConstWrapper.CodiceEnte;
			System.Web.HttpContext.Current.Session["Anagrafica"] = ConstWrapper.ParametroAnagrafica;

            //oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConstWrapper.ParametroAnagrafica); 
			oDettaglioAnagrafica = new DettaglioAnagrafica(); 
			oDettaglioAnagrafica.COD_CONTRIBUENTE = -1; 
			oDettaglioAnagrafica.ID_DATA_ANAGRAFICA =-1; 
			 
			System.Web.HttpContext.Current.Session[chiaveSessione] = oDettaglioAnagrafica; 
			
			writeJavascriptCompensazioni(page, chiaveSessione);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.HelperAnagrafica.PopUpAnagraficaCompensazione.errore: ", Err);
                throw Err;
            }
        }


		private static void writeJavascriptCompensazioni(System.Web.UI.Page page, string nomeSessione) 
		{ 
			string strscript; 
			strscript = ""; 
			strscript = "ApriRicercaAnagrafeCompensazioni('" + nomeSessione + "');"; 
			page.RegisterStartupScript("", "<script language='javascript'>" + strscript + "</script>"); 
		}

	}
}
