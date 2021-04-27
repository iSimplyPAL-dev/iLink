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

namespace DichiarazioniICI.CalcoloICI
{
	/// <summary>
	/// Pagina contenitore per la visualizzazione dell'elaborazione in corso.
	/// </summary>
	public partial class StatoElaborazioneICImassivo :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(StatoElaborazioneICImassivo));
        protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            try
            {

                System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
                string strTipoElaborazione;
                string strUTENTE;
                string strPARAMETRI;

                strTipoElaborazione = Request["TIPO_ELABORAZIONE"].ToString();
                strUTENTE = Request["UTENTE"].ToString();
                strPARAMETRI = "?TIPO_ELABORAZIONE=" + strTipoElaborazione + "&UTENTE=" + strUTENTE;

                strBuilder.Append("parent.parent.nascosto.location.href='./ViewElaborazioneICImassivo.aspx" + strPARAMETRI + "';");
                RegisterScript(strBuilder.ToString(),this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StatoElaborazioneICImassivo.Page_Load.errore: ", ex);
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
