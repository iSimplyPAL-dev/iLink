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
namespace DichiarazioniICI.CalcoloICI
{
/// <summary>
/// Pagina dei comandi per la configurazione delle aliquote.
/// Le possibili opzioni sono:
/// - Elimina
/// - Salva
/// - Torna alla videata precedente
/// </summary>
	public partial class CConfigAliquote :BasePage
	{
        /// <summary>
        /// 
        /// </summary>
		protected System.Web.UI.WebControls.Label infoEnte;
	/// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
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
