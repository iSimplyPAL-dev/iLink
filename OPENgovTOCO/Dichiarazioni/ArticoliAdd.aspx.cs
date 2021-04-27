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
/// Pagina per l'inserimento posizione in dichiarazione
/// </summary>
public partial class ArticoliAdd :BasePage
	{
		#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{

		}
        /// <summary>
        /// 
        /// </summary>
		protected System.Web.UI.WebControls.ImageButton Delete;

		/// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Init(System.Object sender, System.EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
		}
		
		#endregion
		
		private Wuc.WucArticolo wuc;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ArticoliAdd));
        /// <summary>
        /// caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(System.Object sender, System.EventArgs e)
		{
            try {
                //Put user code to initialize the page here
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                wuc = (Wuc.WucArticolo)(this.FindControl("wucArticolo"));
                //Add nuovo articolo da edit dichiarazione
                if (Request["AddFromEdit"] != null && Request["AddFromEdit"].CompareTo("true") == 0)
                {
                    wuc.OpType = OSAPConst.OperationType.ADDFROMEDIT;
                    Salva.Visible = false;
                }
                else
                {
                    wuc.OpType = OSAPConst.OperationType.ADD;
                    SalvaFromEdit.Visible = false;
                }

                if (!Page.IsPostBack)
                {
                    string[] clientArrayControl = wuc.GetMandatoryFields();

                    this.Salva.Attributes.Add("onClick", "return ValidateForm('" + clientArrayControl[0] + "','" + clientArrayControl[1] + "')");
                    this.SalvaFromEdit.Attributes.Add("onClick", "return ValidateForm('" + clientArrayControl[0] + "','" + clientArrayControl[1] + "')");

                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ArticoliAdd.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }		
		
		#region Event Handler
		/// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Salva_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
		{
			wuc.SalvaArticolo();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Cancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect (OSAPPages.DichiarazioniAdd + "?FromArticoli=true");
		}
		
		#endregion
        /// <summary>
        /// pulsante di salvataggio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void SalvaFromEdit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			wuc.OpType = OSAPConst.OperationType.ADDFROMEDIT;
			wuc.SalvaArticolo();
		}

	}
	
}
