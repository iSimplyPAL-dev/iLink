using Microsoft.VisualBasic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;
using log4net;

namespace OPENgovTOCO.Dichiarazioni
{
    /// <summary>
    /// Pagina per la modifica posizione in dichiarazione
    /// </summary>
	public partial class ArticoliEdit :BasePage
	{
		#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{

		}

		
		protected void Page_Init(System.Object sender, System.EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
		}
		
		#endregion

		
		private Wuc.WucArticolo wuc;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ArticoliEdit));

        protected void Page_Load(System.Object sender, System.EventArgs e)
		{
            try {
                //Put user code to initialize the page here
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;

                int IdArticolo = int.Parse(Request["IdArticolo"].ToString());

                wuc = (Wuc.WucArticolo)(this.FindControl("wucArticolo"));
                wuc.IdArticolo = IdArticolo;
                wuc.OpType = OSAPConst.OperationType.EDIT;
                //Put user code to initialize the page here
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ArticoliEdit.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		protected void Salva_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			wuc.SalvaArticolo();
		}

		protected void Cancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect (OSAPPages.DichiarazioniEdit);
		}
		
	}
	
}
