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
/// Pagina dei comandi per la consultazione/gestione delle dichiarazioni e degli immobili del soggetto.
/// Le possibili opzioni sono:
/// - Visualizza GIS
/// - Calcolo puntuale
/// - Torna alla videata precedente
/// </summary>
	public partial class CGestioneDettaglio :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(CGestioneDettaglio));
        protected void Page_Load(object sender, System.EventArgs e)
		{
			            lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
			//*** 20131003 - gestione atti compravendita ***
			string strScript="";
            try { 
			if (Request["COMPRAVENDITA"]!=null)
			{
				strScript= "document.getElementById('NewImmobile').style.display='';";
				strScript+= "document.getElementById('CalcoloICIpuntuale').style.display='none';";
				strScript+= "document.getElementById('ControlloPreAccertamento').style.display='none';";
			}
			else
			{
				strScript+= "document.getElementById('NewImmobile').style.display='none';";
			}
            //*** 20140923 - GIS ***
            if (Business.ConstWrapper.VisualGIS == false)
			{
                strScript += "document.getElementById('GIS').style.display='none';";
			}
            //*** ***
            RegisterScript(strScript,this.GetType());
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CGestioneDettaglio.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
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
