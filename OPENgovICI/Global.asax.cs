using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.IO;
using log4net;
using log4net.Config;


namespace DichiarazioniICI 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// 
        /// </summary>
		public Global()
		{
			InitializeComponent();
		}	
		/// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Application_Start(Object sender, EventArgs e)
		{
            string pathfileinfo;
            pathfileinfo = System.Configuration.ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
            FileInfo fileconfiglog4net = new FileInfo(pathfileinfo);
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);
        }
 /// <summary>
 /// 
 /// </summary>
 /// <param name="sender"></param>
 /// <param name="e"></param>
		protected void Session_Start(Object sender, EventArgs e)
		{
            try { 
            HttpContext.Current.Session["path"] = System.Configuration.ConfigurationManager.AppSettings["NOME_SITO"] + System.Configuration.ConfigurationManager.AppSettings["PATH_OPENGOVI"].ToString();//Application["nome_sito"]            
                            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Global.Session_Start.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Application_Error(Object sender, EventArgs e)
		{

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Session_End(Object sender, EventArgs e)
		{

		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

