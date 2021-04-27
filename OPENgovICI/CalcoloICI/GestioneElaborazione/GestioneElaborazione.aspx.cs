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

namespace DichiarazioniICI.CalcoloICI.GestioneElaborazione
{
    /// <summary>
    /// Pagina contenitore per il controllo delle elaborazioni massive.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GestioneElaborazione :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(GestioneElaborazione));

        HtmlControl frameRepository;
		HtmlControl frameProgressTask;
        /// <summary>
        /// Permette di monitorare l’avanzamento del calcolo massivo e della storia delle elaborazioni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            try
            {
                string strTipoElaborazione;
                string strUTENTE;

                //strTipoElaborazione = Request["TIPO_ELABORAZIONE"].ToString();
                //strUTENTE = Request["UTENTE"].ToString();

                strTipoElaborazione = "C";
                strUTENTE = Session["username"].ToString();

                string strPARAMETRI;
                strPARAMETRI = "?TIPO_ELABORAZIONE=" + strTipoElaborazione + "&UTENTE=" + strUTENTE;

                frameRepository = (HtmlControl)(this.FindControl("Repository"));
                frameRepository.Attributes.Add("src", "./Repository.aspx" + strPARAMETRI + ";");
                log.Debug("GestioneElaborazione::page_load::devo caricare repository::./Repository.aspx" + strPARAMETRI);

                frameProgressTask = (HtmlControl)(this.FindControl("ProgressTask"));
                frameProgressTask.Attributes.Add("src", "./StatoElaborazioneICImassivo.aspx" + strPARAMETRI + ";");
                log.Debug("GestioneElaborazione::page_load::devo caricare stato elaborazione::./StatoElaborazioneICImassivo.aspx" + strPARAMETRI);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneElaborazione.Page_Load.errore: ", ex);
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
