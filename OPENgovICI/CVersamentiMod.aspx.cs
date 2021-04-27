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
	/// Summary description for CVersamenti.
	/// </summary>
	public partial class CVersamentiMod :BasePage
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(CVersamentiMod));
        protected void Page_Load(object sender, System.EventArgs e)
		{
			string strScript="";
			            lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
            //Dipe 25/10/2007 
            //Attivazione del tasto per ritornare alla videta dei dati attuali
            //Il valore della session è ti tipo booleano
            try { 
			if (Session["DATI_ATTUALI"]==null)
			{
				Session["DATI_ATTUALI"]=false;
			}

			if ( (bool)Session["DATI_ATTUALI"] )
			{
				strScript="document.getElementById ('backAttuali').style.display='inline';";
				strScript=strScript+"document.getElementById ('Delete').style.display='none';";
			}
			else
			{
				strScript="document.getElementById ('backAttuali').style.display='none';";
				strScript=strScript+"document.getElementById ('Delete').style.display='inline';";
			}
			ClientScript.RegisterStartupScript(this.GetType(),"", "<script language='javascript'>" + strScript +"</script>");
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.CVersamentiMod.Page_Load.errore: ", Err);
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
