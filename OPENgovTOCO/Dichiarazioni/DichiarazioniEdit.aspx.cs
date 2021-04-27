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
    /// Pagina per la modifica dichiarazione
    /// </summary>
	public partial class DichiarazioniEdit :BasePage
	{
		
		private Wuc.WucDichiarazione wuc;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioniEdit));

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
		
		protected void Page_Load(System.Object sender, System.EventArgs e)
		{
            try {
                //Put user code to initialize the page here
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;

                wuc = (Wuc.WucDichiarazione)(this.FindControl("wucDichiarazione"));
                wuc.OpType = OSAPConst.OperationType.EDIT;

                string[] clientArrayControl = wuc.GetMandatoryFields();
                this.Salva.Attributes.Add("onClick", "return ValidateForm('" + clientArrayControl[0] + "','" + clientArrayControl[1] + "');DichiarazioneValidate()");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniEdit.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		protected void Salva_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			wuc.SalvaDichiarazione();
		}
		
		protected void Cancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect (OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo="+DichiarazioneSession.CodTributo(""));
		}
	}
	
}
