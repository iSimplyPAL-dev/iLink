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
using DichiarazioniICI.Database;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la consultazione delle aliquote per il calcolo.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ElencoAliquote :BasePage
	{
		string strANNO=string.Empty;
        private static readonly ILog log = LogManager.GetLogger(typeof(ElencoAliquote));				

		//protected System.Web.UI.WebControls.Label Label5;
		//protected System.Web.UI.WebControls.Label Label4;
		//protected System.Web.UI.WebControls.Label Label3;
		//protected System.Web.UI.WebControls.Label Label1;
		//protected System.Web.UI.WebControls.Label Label2;
		//protected System.Web.UI.WebControls.Label lblSaldo;
		//protected System.Web.UI.WebControls.Label Label6;
		//protected System.Web.UI.WebControls.Label lblContribuente;
		//protected System.Web.UI.WebControls.Label lblAnnoRiferimento;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            try
            {
                strANNO = Request["ANNO"].ToString();

                if (!(Page.IsPostBack))
                {
                    if (strANNO != "")
                    {
                        Aliquote objAliquote = new Aliquote();
                        DataTable dtAliquote;
                        //*** 20140509 - TASI ***
                        //*** 20130422 - aggiornamento IMU ***
                        int COD_CONTRIB = -1;
                        if (Request["COD_CONTRIB"] != null)
                            COD_CONTRIB = int.Parse(Request["COD_CONTRIB"].ToString());
                        //dtAliquote=objAliquote.ListaAliquote(-1,strANNO,"-1",-1,ConstWrapper.CodiceEnte,-1,COD_CONTRIB);
                        dtAliquote = objAliquote.ListaAliquote(Request["COD_TRIBUTO"].ToString(), -1, strANNO, "-1", -1, Business.ConstWrapper.CodiceEnte, -1, COD_CONTRIB);
                        //*** ***
                        //*** ***
                        if (dtAliquote.Rows.Count > 0)
                        {
                            ViewState.Add("dtAliquote", dtAliquote);
                            GrdCategorie.DataSource = dtAliquote;
                            GrdCategorie.DataBind();

                            lblMessage.Visible = false;
                            lblMessage.Text = "";
                            GrdCategorie.Visible = true;
                        }
                        else
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Non sono state trovate aliquote configurate.";
                            GrdCategorie.Visible = false;
                        }
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Non sono state trovate aliquote configurate per l'anno selezionato.";
                        GrdCategorie.Visible = false;
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElencoAliquote.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #region "Griglie"
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        #endregion
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdCategorie.DataSource = (DataTable)ViewState["dtAliquote"];
                if (page.HasValue)
                    GrdCategorie.PageIndex = page.Value;
                GrdCategorie.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElencoAliquote.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
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
