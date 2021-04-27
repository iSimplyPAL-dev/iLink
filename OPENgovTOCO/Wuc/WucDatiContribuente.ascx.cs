using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AnagInterface;
using log4net;
//using OPENUtility;

namespace OPENgovTOCO.Wuc
{
    /// <summary>
    /// Usercontrol per la gestione Anagrafica
    /// </summary>
	public partial class WucDatiContribuente : System.Web.UI.UserControl
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(WucDatiContribuente));
        public DettaglioAnagrafica oAnagrafica= new DettaglioAnagrafica();

		protected void Page_Load(object sender, System.EventArgs e)
		{
            try {
                if (!Page.IsPostBack)
                {
                    BindData();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDatiContribuente.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private void BindData ()
		{
            try {
                if (oAnagrafica != null)
                {
                    lblCognome.Text = oAnagrafica.Cognome;
                    lblNome.Text = oAnagrafica.Nome;
                    if (oAnagrafica.DataNascita != null &&
                        oAnagrafica.DataNascita.CompareTo("00/00/1900") != 0)
                    {
                        lblDataNascita.Text = oAnagrafica.DataNascita;
                    }
                    else
                    {
                        lblDataNascita.Text = string.Empty;
                    }
                    lblIndirizzo.Text = oAnagrafica.ViaResidenza + ", " + oAnagrafica.CivicoResidenza;
                    lblComune.Text = oAnagrafica.ComuneResidenza + " (" + oAnagrafica.ProvinciaResidenza + ")";
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDatiContribuente.BlindData.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

        }

		public void LoadAnagrafica (int CodContribuente)
		{
			DAO.AnagraficheDAO objAnagrafica = new DAO.AnagraficheDAO();  

			this.oAnagrafica =(DettaglioAnagrafica) objAnagrafica.GetAnagraficaContribuente (CodContribuente);//,  DichiarazioneSession.CodTributo("")
		}

		#region Codice generato da Progettazione Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: questa chiamata è richiesta da Progettazione Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
