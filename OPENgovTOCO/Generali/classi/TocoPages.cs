using log4net;
using System;
using System.Web;
using System.Web.Security;

namespace OPENgovTOCO
{
    /// <summary>
    /// Classe generale che eredita BasePage.
    /// Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
    /// </summary>
    public class BaseEnte : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BaseEnte));

        override protected void OnInit(EventArgs e)
        {
            if (DichiarazioneSession.IdEnte == "")
            {
                RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", this.GetType());
            }
        }
    }
    /// <summary>
    /// Classe generale che eredita Page.
    /// Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BasePage));
        public HttpCookie authCookie = null;
        override protected void OnInit(EventArgs e)
        {
            try
            {
                bool isExpired = false;
                //get cookie
                authCookie = HttpContext.Current.Request.Cookies.Get("aplckute");
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null)
                    {
                        if (!authTicket.Expired)
                        {
                            string[] myData = authTicket.UserData.Split(char.Parse("|"));
                            if (myData != null)
                            {
                                if (myData.GetUpperBound(0) != 1)
                                {
                                    Log.Debug("sessione scaduta perchè non ho userdata nel cookie");
                                    isExpired = true;
                                }
                                else {
                                    //if (myData[0] != DichiarazioneSession.sOperatore)
                                    //{
                                    Log.Debug("utente cookie->" + myData[0] + "  utente sessione->" + DichiarazioneSession.sOperatore);
                                if (DichiarazioneSession.sOperatore != string.Empty)
                                        {
                                            //Log.Debug("sessione scaduta perchè utente cookie diverso da utente sessione");
                                            //isExpired = true;
                                        }
                                        else {
                                            Session["username"] = myData[0];
                                        }
                                    //}
                                }
                            }
                            else {
                                Log.Debug("sessione scaduta perchè userdata cookie null");
                                isExpired = true;
                            }
                        }
                        else {
                            Log.Debug("sessione scaduta perchè cookie scaduto");
                            isExpired = true;
                        }
                    }
                    else {
                        Log.Debug("sessione scaduta perchè non ticket cookie");
                        isExpired = true;
                    }
                }
                else {
                    Log.Debug("sessione scaduta perchè non ho cookie");
                    isExpired = true;
                }
                if (DichiarazioneSession.sOperatore == "")
                {
                    Log.Debug("sessione scaduta perchè sessione operatore vuoto");
                    isExpired = true;
                }
                if (isExpired)
                {
                    HttpContext.Current.Session["username"] = "";
                    RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", this.GetType());
                }
                if (Session["COD_TRIBUTO"] == null || (Session["COD_TRIBUTO"].ToString() != Utility.Costanti.TRIBUTO_SCUOLE && Session["COD_TRIBUTO"].ToString() != Utility.Costanti.TRIBUTO_OSAP))
                    Session["COD_TRIBUTO"] = Utility.Costanti.TRIBUTO_OSAP;
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovTOCO.Utility.BasePage_Init.errore: ", ex);
            }
        }

        protected void RegisterScript(string myScript, Type type)
        {
            try
            {
                DichiarazioneSession.CountScript = (DichiarazioneSession.CountScript + 1);
                string uniqueId = "spc_" + DichiarazioneSession.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += myScript;
                sScript += "</script>";
                ClientScript.RegisterStartupScript(type, uniqueId, sScript);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.BasePage.RegisterScript.errore: ", Err);
            }
        }
    }
    /// <summary>
    /// Classe per il reindirizzamento delle pagine.
    /// </summary>
    public class OSAPPages
    {

        public static string DichiarazioniAdd = "DichiarazioniAdd.aspx";
        public static string DichiarazioniEdit = "DichiarazioniEdit.aspx";
        public static string DichiarazioniView = "DichiarazioniView.aspx";
        public static string ArticoliView = "ArticoliView.aspx";
        public static string ArticoliAdd = "ArticoliAdd.aspx";
        public static string ArticoliEdit = "ArticoliEdit.aspx";
        public static string DichiarazioniSearch = "DichiarazioniSearch.aspx";

        public static string SituazioneAvvisiSearch = "SituazioneAvvisiSearch.aspx";
        public static string SituazioneAvvisi = "SituazioneAvvisi.aspx";
        public static string ElaborazioneAvvisi = "ElaborazioneAvvisi.aspx";

        public static string GestionePagamentiSearch = "GestionePagamentiSearch.aspx";
        public static string GestionePagamentiAdd = "GestionePagamentiAdd.aspx";
        public static string GestionePagamentiEdit = "GestionePagamentiEdit.aspx";

        public OSAPPages()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}