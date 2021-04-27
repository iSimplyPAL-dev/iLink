using System;
using System.Web.UI;
using log4net;
using System.Web.Security;
using System.Web;

namespace DichiarazioniICI
{
    /// <summary>
    /// Classe generale che eredita BasePage.
    /// Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class BaseEnte : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BaseEnte));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            try
            {
                if (Business.ConstWrapper.CodiceEnte == "" && Business.ConstWrapper.FromSovracomunali!="S" && Business.ConstWrapper.Sportello!="1")
                {
                    RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", this.GetType());
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.BaseEnte.OnInit.errore: ", Err);
            }
        }
    }
    /// <summary>
    /// Classe generale che eredita Page.
    /// Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class BasePage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BasePage));
        /// <summary>
        /// 
        /// </summary>
        public HttpCookie authCookie = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            Session["COD_TRIBUTO"] = Utility.Costanti.TRIBUTO_ICI;
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
                                    log.Debug("sessione scaduta perchè non ho userdata nel cookie");
                                    isExpired = true;
                                }
                                else {
                                    //if (myData[0] != Business.ConstWrapper.sUsername)
                                    //{
                                    log.Debug("utente cookie->" + myData[0] + "  utente sessione->" + Business.ConstWrapper.sUsername);
                                  if (Business.ConstWrapper.sUsername != string.Empty)
                                        {
                                            //log.Debug("sessione scaduta perchè utente cookie diverso da utente sessione");
                                            //isExpired = true;
                                        }
                                        else {
                                            Session["username"] = myData[0];
                                        }
                                    //}
                                }
                            }
                            else {
                                log.Debug("sessione scaduta perchè userdata cookie null");
                                isExpired = true;
                            }
                        }
                        else {
                            log.Debug("sessione scaduta perchè cookie scaduto");
                            isExpired = true;
                        }
                    }
                    else {
                        log.Debug("sessione scaduta perchè non ticket cookie");
                        isExpired = true;
                    }
                }
                else {
                    if (Utility.StringOperation.FormatString(Business.ConstWrapper.Sportello) != "1")
                    {
                        log.Debug("sessione scaduta perchè non ho cookie");
                        isExpired = true;
                    }
                    else
                    {
                        Session["username"] = Business.ConstWrapper.SportelloUser;
                    }
                }
                if (Business.ConstWrapper.sUsername == "")
                {
                    log.Debug("sessione scaduta perchè sessione operatore vuoto");
                    isExpired = true;
                }
                if (isExpired)
                {
                    HttpContext.Current.Session["username"] = "";
                    RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.BasePage_Init.errore: ", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <param name="type"></param>
        protected void RegisterScript(string script, Type type)
        {
            Business.ConstWrapper.CountScript = (Business.ConstWrapper.CountScript + 1);
            string uniqueId = "spc_" + Business.ConstWrapper.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
            string sScript = "<script language='javascript'>";
            sScript += script;
            sScript += "</script>";
            ClientScript.RegisterStartupScript(type, uniqueId, sScript);
        }
    }
}