using Business;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina contenitore per la consultazione incrociata tra tributi dell'immobile.
    /// </summary>
    public partial class FrmUI :BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FrmUI));
        /// <summary>
        /// Valorizzo la sessione per l'ingresso da sportello
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            string[] ListParam = StringOperation.FormatString(Request.QueryString["ParamUIBody"]).Split(new char[] { ('$') });
            foreach (string Param in ListParam)
            {
                if (Param.Contains("Sportello"))
                    Session["Sportello"] = StringOperation.FormatString(Param.Replace("Sportello=", String.Empty));
                else if (Param.Contains("IDImmobile"))
                    Session["COD_ENTE"] = getEnteByUI(StringOperation.FormatInt(Param.Replace("IDImmobile=", String.Empty)));
            }
            //set the forms auth cookie
            string IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //when user is behind proxy server
            if (IP == null)
                IP = Request.ServerVariables["REMOTE_ADDR"];
            //FormsAuthentication.SetAuthCookie(Usr, True) ***works only in SSL
            FormsAuthenticationTicket myTicket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout), true, ConstWrapper.SportelloUser + "|" + IP);
            //Create an HttpOnly cookie.
            HttpCookie myHttpOnlyCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(myTicket));
            //Setting the HttpOnly value to true, makes this cookie accessible only to ASP.NET.
            myHttpOnlyCookie.HttpOnly = true;
            Response.AppendCookie(myHttpOnlyCookie);
            //create a cookie
            string myCookieName = "aplckute";
            myHttpOnlyCookie = new HttpCookie(myCookieName, FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, myCookieName, DateTime.Now, DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout), true, ConstWrapper.SportelloUser + "|" + IP)));
            //Setting the HttpOnly value to true, makes this cookie accessible only to ASP.NET.
            myHttpOnlyCookie.HttpOnly = true;
            Response.AppendCookie(myHttpOnlyCookie);
            authCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            base.OnInit(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        private string getEnteByUI(int id)
        {
            string myVal = string.Empty;
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetEnteByUI", "ID");

                    DataView dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", id));
                    foreach (DataRowView myRow in dvMyView)
                    {
                        myVal = StringOperation.FormatString(myRow["Ente"]);
                        Session["DESCRIZIONE_ENTE"] = StringOperation.FormatString(myRow["DESCRIZIONE_ENTE"]);
                    }
                    ctx.Dispose();
                }
            }
            catch (Exception Err)
            {
                log.Debug("." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.FrmUI.getEnteByUI.errore: ", Err);                
            }
            return myVal;
        }
    }
}