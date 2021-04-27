using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using log4net;
namespace DichiarazioniICI
{
/// <summary>
/// Pagina dei comandi per la consultazione/gestione degli immobili.
/// Le possibili opzioni sono:
/// - Visualizza GIS
/// - Stampa
/// - Inserisci nuovo
/// - Ricerca
/// </summary>
		public partial class CGestione :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(CGestione));
        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            

            lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
            string strScript = "";
            //*** 20140923 - GIS ***
            try { 
            if (Business.ConstWrapper.VisualGIS == false)
            {
                strScript = "document.getElementById('GIS').style.display='none';";
                RegisterScript(strScript,this.GetType());
            }
            //*** ***
            //*** 201511 - Funzioni Sovracomunali ***
            if (Business.ConstWrapper.CodiceEnte == "")
            {
                strScript += "document.getElementById('GIS').style.display='none';";
                strScript += "document.getElementById('New').style.display='none';";
                RegisterScript(strScript,this.GetType());
            }
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CGestione.Page_Load.errore: ", Err);
                Response.Redirect("../../../PaginaErrore.aspx");
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
