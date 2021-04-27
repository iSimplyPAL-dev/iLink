using System;
using System.Diagnostics;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Runtime.InteropServices;
using System.Text;
using log4net;

namespace Business
{
   /* /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }
   
    /// <summary>
    /// L'enumerator delle tipologie delle provenienze.
    /// </summary>
    public enum TipologieProvenienze{VersamentiUVI=3, VersamentiPOSTEL=10, VersamentiANCICNC=11, VersamentiRISCONET=12, DichiarazioneCNC=14, AnagrafeUfficiale=6, ProvenienzaSconosciuta=18, Liquidazione=19, Accertamento=20}
	
	/// <summary>
	/// L'enumarotor delle rendite.
	/// </summary>
	public enum Rendite{RE=1, RP=2, RPM=3, LC=4, AF=5, TA=6} */
    /// <summary>
    /// Classe per le direttive delle pagine
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ApplicationHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ApplicationHelper));
        /// <summary>
        /// Interfaccia verso il parametro di configuraziome omonimo
        /// </summary>
        public static string ContoCorrente
        {
            get
            {
                try
                {
                    OPENUtility.ClsContiCorrenti FncCC = new OPENUtility.ClsContiCorrenti();
                    //return ConfigurationManager.AppSettings["ContoCorrente"];
                    if (HttpContext.Current.Session["CONTO_CORRENTE"] != null)
                        return HttpContext.Current.Session["CONTO_CORRENTE"].ToString();
                    else
                        return FncCC.GetContoCorrente(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodiceTributo, Business.ConstWrapper.sUsername, Business.ConstWrapper.StringConnectionOPENgov).ContoCorrente;
                }
                catch (Exception Err)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.ContoCorrente.errore: ", Err);
                    throw Err;
                }
            }

        }
        /// <summary>
        /// Interfaccia verso il parametro di configuraziome omonimo
        /// </summary>
        public static string IntestatarioBollettino
        {
            get
            {
                try
                {
                    OPENUtility.ClsContiCorrenti FncCC = new OPENUtility.ClsContiCorrenti();
                    //return ConfigurationManager.AppSettings["IntestatarioBollettino"];
                    if (HttpContext.Current.Session["INTESTAZIONE_BOLLETTINO"] != null)
                        return HttpContext.Current.Session["INTESTAZIONE_BOLLETTINO"].ToString();
                    else
                        return FncCC.GetContoCorrente(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodiceTributo, Business.ConstWrapper.sUsername, Business.ConstWrapper.StringConnectionOPENgov).Intestazione_1;
                }
                catch (Exception Err)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.IntestatarioBollettino.errore: ", Err);
                    throw Err;
                }
            }
        }

        /// <summary>
        /// Interfaccia verso il parametro di configuraziome omonimo
        /// </summary>
        public static string ComuneUbicazioneImmobile
        {
            get
            {
                try
                {
                    //return ConfigurationManager.AppSettings["ComuneUbicazioneImmobile"];
                    if (HttpContext.Current.Session["COMUNE_UBICAZIONE_IMMOBILE"] != null)
                        return HttpContext.Current.Session["COMUNE_UBICAZIONE_IMMOBILE"].ToString();
                    else
                        return ConstWrapper.DescrizioneEnte;
                }
                catch (Exception Err)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.ComuneUbicazioneImmobile.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Carica una pagina secondo le direttive 
        /// </summary>
        /// <param name="service">nome del servizio definito  </param>
        /// <param name="parametri"> parametri da passare all'url </param>
        /// <returns></returns>
        public static string LoadFrameworkPage(string service, string parametri)
        {
            string Visualizza = ConstWrapper.Path + "../../aspVuota.aspx";
            string Comandi = ConstWrapper.Path + "../../aspVuota.aspx";
            string Basso = ConstWrapper.Path + "../../aspVuota.aspx";
            string sHTML = "";
            try { 
            switch (service)
            {
                case "SR_GESTIONE":
                    Visualizza = ConstWrapper.Path + "/Gestione.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CGestione.aspx" + parametri;
                    break;
                case "SR_GESTIONE_DETTAGLIO":
                    Visualizza = ConstWrapper.Path + "/GestioneDettaglio.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CGestioneDettaglio.aspx" + parametri;
                    break;
                case "SR_IMMOBILE_DETTAGLIO":
                    Visualizza = ConstWrapper.Path + "/ImmobileDettaglio.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CImmobileDettaglio.aspx" + parametri;
                    break;
                case "SR_INSERISCI_DICH":
                    Visualizza = ConstWrapper.Path + "/Dichiarazione.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CDichiarazioneMod.aspx" + parametri;
                    break;
                case "SR_RICERCA_VERSAMENTI":
                    Visualizza = ConstWrapper.Path + "/RicercaVersamenti.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CRicercaVersamenti.aspx";
                    break;
                case "SR_INSERISCI_VERSAMENTI":
                    Visualizza = ConstWrapper.Path + "/Versamenti.aspx" + parametri;
                    break;
                case "SR_CONFRONTO_CATASTO":
                    Visualizza = ConstWrapper.Path + "/ConfrontaConCatasto/CheckRifCatastali.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "../../aspVuotaRemoveComandi.aspx";
                    Basso = ConstWrapper.Path + "../../aspVuotaRemoveComandi.aspx";
                    break;
                case "SR_BONIFICA_VERSAMENTO":
                    Visualizza = ConstWrapper.Path + "/RicercaVersamentiDaBonificare.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CRicercaVersamentiDaBonificare.aspx";
                    break;
                case "SR_VERSAMENTI_BONIFICARE":
                    Visualizza = ConstWrapper.Path + "/RicercaVersamentiDaBonificare.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CRicercaVersamentiDaBonificare.aspx";
                    break;
                default:
                    log.Debug("LoadFrameworkPage: servizio richiesto->" + service);
                    break;
            }

             sHTML = "parent.Comandi.location.href='" + Comandi + "'";
            sHTML = sHTML + System.Environment.NewLine + "parent.Visualizza.location.href='" + Visualizza + "'";
            sHTML = sHTML + System.Environment.NewLine + "parent.Basso.location.href='" + Basso + "'";
                sHTML = sHTML + System.Environment.NewLine + "parent.Nascosto.location.href='" + Basso + "'";
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.LoadFrameworkPage.errore: ", Err);
                throw Err;
            }
            return sHTML;
        }
        /// <summary>
        /// Carica una pagina da iFrame secondo le direttive 
        /// </summary>
        /// <param name="service">nome del servizio definito </param>
        /// <param name="parametri"> parametri da passare all'url </param>
        /// <returns></returns>
        public static string LoadFrameworkPageFromIframe(string service, string parametri)
        {
            string Visualizza = ConstWrapper.Path + "/generali/asp/aspVuota.aspx";
            string Comandi = ConstWrapper.Path + "/generali/asp/aspVuota.aspx";
            string Basso = ConstWrapper.Path + "/generali/asp/aspVuota.aspx";
            string sHTML="";
            try {         
            switch (service)
            {
                case "SR_GESTIONE_DETTAGLIO":
                    Visualizza = ConstWrapper.Path + "/GestioneDettaglio.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CGestioneDettaglio.aspx" + parametri;
                    break;
                case "SR_IMMOBILE_DETTAGLIO":
                    Visualizza = ConstWrapper.Path + "/ImmobileDettaglio.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CImmobileDettaglio.aspx" + parametri;
                    break;
                case "SR_INSERISCI_DICH":
                    Visualizza = ConstWrapper.Path + "/Dichiarazione.aspx" + parametri;
                    Comandi = ConstWrapper.Path + "/CDichiarazioneMod.aspx" + parametri;
                    break;
                default:
                    log.Debug("LoadFrameworkPageFromIframe: servizio richiesto->" + service);
                    break;
            }

           sHTML = "parent.parent.Comandi.location.href='" + Comandi + "';";
            sHTML += "parent.parent.Visualizza.location.href='" + Visualizza + "';";
            sHTML +=  "parent.parent.Basso.location.href='" + Basso + "';";
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.LoadFrameworkPageIframe.errore: ", Err);
                throw Err;
            }
            return sHTML;
        }

     }
}