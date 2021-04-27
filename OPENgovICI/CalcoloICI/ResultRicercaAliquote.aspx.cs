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
    /// Pagina per la visualizzazione del risultato della ricerca aliquote.
    /// Contiene la griglia dalla quale è possibile accedere al dettaglio.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ResultRicercaAliquote :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ResultRicercaAliquote));
        string strANNO = string.Empty;
		string strTIPO=string.Empty;
		double dblValore;

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
                strTIPO = Request["TIPO"].ToString();
                if (Request["VALORE"].ToString().CompareTo("") == 0)
                {
                    dblValore = -1;
                }
                else
                {
                    dblValore = double.Parse(Request["VALORE"].ToString());
                }

                if (!(Page.IsPostBack))
                {
                    Aliquote objAliquote = new Aliquote();
                    DataTable dtAliquote;
                    //*** 20130422 - aggiornamento IMU ***
                    dtAliquote = objAliquote.ListaAliquote(Request["COD_TRIBUTO"].ToString(), -1, strANNO, strTIPO, dblValore, Business.ConstWrapper.CodiceEnte, -1, -1);
                    //*** ***
                    if (dtAliquote.Rows.Count > 0)
                    {
                        //ViewState.Add("dtAliquote", dtAliquote);
                        Session.Add("dtAliquote", dtAliquote);
                        GrdResult.DataSource = dtAliquote;
                        GrdResult.DataBind();

                        lblMessage.Visible = false;
                        lblMessage.Text = "";
                        GrdResult.Visible = true;
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Non sono state trovate aliquote configurate.";
                        GrdResult.Visible = false;
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultRicercaAliquote.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region "Griglie"
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    string strscript = "";
                    strscript = strscript + "parent.location.href='ConfigAliquote.aspx?ID_ALIQUOTA=" + IDRow + "';";
                    RegisterScript(strscript,this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultRicercaAliquote.GrdRowCommand.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        //      public void GrdResult_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //	string ID_ALIQUOTA = GrdResult.SelectedItem.Cells[7].Text; 

        //	string strscript = ""; 
        //	strscript = strscript + "parent.location.href='ConfigAliquote.aspx?ID_ALIQUOTA=" + ID_ALIQUOTA + "';"; 
        //	RegisterScript(this.GetType(),"", "<script language='javascript'>" + strscript + "</script>"); 
        //}
        #endregion
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdResult.DataSource = (DataTable)Session["dtAliquote"];//(DataTable)ViewState["dtAliquote"];
                if (page.HasValue)
                    GrdResult.PageIndex = page.Value;
                GrdResult.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultRicercaAliquote.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }
	}
}
