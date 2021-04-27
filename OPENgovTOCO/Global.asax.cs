using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using log4net;
using log4net.Config;
using System.IO;

namespace OPENgovTOCO 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));

        public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
            try {
                string pathfileinfo;
                pathfileinfo = System.Configuration.ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
                FileInfo fileconfiglog4net = new FileInfo(pathfileinfo);
                XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioneSession.PathVirtualStampe.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
 
		protected void Session_Start(Object sender, EventArgs e)
		{
            /* S.T. DEBUG 
            HttpContext.Current.Session["PARAMETROENV"] = "OG_TRIBUTI_PRO_RIBES";
            HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"] = "OPENGOVTOCO";
            HttpContext.Current.Session["Anagrafica"] = "OPENGOVA";
            HttpContext.Current.Session["username"] = "RIADM";
            HttpContext.Current.Session["COD_ENTE"] = "050027";//"050019";//
            HttpContext.Current.Session["DESCRIZIONE_ENTE"] = "POMARANCE";//"MONTECATINI"; //
            HttpContext.Current.Session["COD_TRIBUTO"] = "0453";*/
        }

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

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

