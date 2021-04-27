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

namespace DichiarazioniICI//.Analisi.FatturatoIncassato
{
    /// <summary>
    /// Pagina per la consultazione del raffronto fra fatturato ed incassato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class RicercaAnalisiEconomiche :BasePage
	{
		//protected System.Web.UI.WebControls.Label Label2;
		private static readonly ILog log = LogManager.GetLogger(typeof(RicercaAnalisiEconomiche));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			Business.CoreUtility oLoadCombo = new Business.CoreUtility();

			try{
				if(!IsPostBack)
				{
                     //carico gli anni a ruolo
                   DataView Vista = new Database.Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
                    Vista.Sort = "ANNO DESC";
                    oLoadCombo.loadCombo(DdlAnno, Vista);
				}
			}
			catch (Exception Err )
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaAnalisiEconomiche.Page_Load.errore: ", Err);
                Response.Redirect("../../../PaginaErrore.aspx");
			}		
	}
    
		#region Web Form Designer generated code
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
