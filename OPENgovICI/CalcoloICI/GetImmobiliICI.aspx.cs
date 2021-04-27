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

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina per la visualizzazione del calcolo per singolo immobile.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GetImmobiliICI : BaseEnte
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GetImmobiliICI));

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

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			string COD_CONTRIB="-1";
            string Tributo = "";
			string ANNO=string.Empty;
			string ID_PROGRESSIVO_ELABORAZIONE=string.Empty;

            try
            {
                if (Request["COD_CONTRIB"] != null)
                    COD_CONTRIB = Request["COD_CONTRIB"].ToString();
                Tributo = Request["COD_TRIBUTO"].ToString();
                ANNO = Request["ANNO"].ToString();
                ID_PROGRESSIVO_ELABORAZIONE = Request["ID_PROG_ELAB"].ToString();
                if (!Page.IsPostBack)
                {
                    // Put user code to initialize the page here
                    TpSituazioneFinaleIci clTpSituazioneFinaleIci = new TpSituazioneFinaleIci();
                    //*** 20140509 - TASI ***
                    //DataTable dtGetImmobiliICI = clTpSituazioneFinaleIci.GetImmoCalcoloICItotale(COD_CONTRIB, ANNO, ConstWrapper.CodiceEnte, long.Parse(ID_PROGRESSIVO_ELABORAZIONE.ToString()));
                    DataTable dtGetImmobiliICI = clTpSituazioneFinaleIci.GetImmoCalcoloICItotale(COD_CONTRIB, ANNO, Business.ConstWrapper.CodiceEnte, Tributo, long.Parse(ID_PROGRESSIVO_ELABORAZIONE.ToString()));
                    //*** ***
                    if (dtGetImmobiliICI.Rows.Count > 0)
                    {
                        Session["TABELLA_RICERCA"] = dtGetImmobiliICI;
                        // variabile che serve per la stampa excel del riepilogo ICI
                        Session["tblRiepilogoImmobili"] = dtGetImmobiliICI;
                        GrdImmobiliICI.DataSource = dtGetImmobiliICI;
                        GrdImmobiliICI.DataBind();
                        lbltitolo1.Visible = true;
                        GrdImmobiliICI.Visible = true;
                    }
                    else
                    {
                        lbltitolo1.Visible = false;
                        GrdImmobiliICI.Visible = false;
                        Session.Remove("tblRiepilogoImmobili");
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.Page_Load.errore: ", Err);
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
                GrdImmobiliICI.DataSource = (DataTable)Session["tblRiepilogoImmobili"];
                if (page.HasValue)
                    GrdImmobiliICI.PageIndex = page.Value;
                GrdImmobiliICI.DataBind();
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.LoadSearch.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected string BoolToStringForGridView(object iInput)
        //{
        //    string ret = string.Empty;
        //try{
        //    if ((iInput.ToString() == "1")||(iInput.ToString().ToUpper() == "TRUE"))
        //    {
        //        ret = "SI";
        //    }
        //    else
        //    {
        //        ret = "NO";
        //    }
        //    return ret;
        //  }
        //     catch (Exception Err)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.BoolToStringForGridView.errore: ", Err);
        //   Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}

        //protected string BoolToStringForGridView_alt(object iInput)
        //{
        //    string ret = string.Empty;
        //try{
        //    if ((iInput.ToString() == "1")||(iInput.ToString().ToUpper() == "TRUE"))
        //    {
        //        ret = "NO";
        //    }
        //    else
        //    {
        //        ret = "SI";
        //    }
        //    return ret;
        //  }
        //     catch (Exception Err)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.BoolToStringForGridView_alt.errore: ", Err);
        //   Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}

        //protected string EuroForGridView(object iInput)
        //{
        //    string ret = string.Empty;
        //try{
        //    if (iInput != DBNull.Value)
        //    {

        //        if ((iInput.ToString() == "-1")||(iInput.ToString() == "-1,00"))
        //        {
        //            ret = Convert.ToDecimal("0").ToString("N");	//string.Empty;	
        //        }
        //        else
        //        {
        //            ret = Convert.ToDecimal(iInput).ToString("N");
        //        }
        //    }
        //    else
        //    {
        //        ret = Convert.ToDecimal("0").ToString("N");	
        //    }
        //    return ret;
        //  }
        //     catch (Exception Err)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.EuroForGridView.errore: ", Err);
        //   Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}
        //*** 20120629 - IMU ***
        //protected string CaricoFigli(object nFigli, object PercentCarico)
        //{
        //    string ret = string.Empty;
        //try{
        //    if (nFigli != DBNull.Value)
        //    {
        //        if (int.Parse(nFigli.ToString())>0)
        //        {
        //            ret = nFigli.ToString();
        //        }
        //    }
        //    if (PercentCarico != DBNull.Value)
        //    {
        //        if (Convert.ToDecimal(PercentCarico)>0)
        //        {
        //            ret +=" rid. al " + Convert.ToDecimal(PercentCarico).ToString() +"%";
        //        }
        //    }
        //    return ret;
        //  }
        //     catch (Exception Err)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.CaricoFigli.errore: ", Err);
        //   Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}
        //*** ***

        //protected string IntForGridView(object iInput)
        //{
        //    string ret = string.Empty;
        //try{
        //    if ((iInput.ToString() == "-1")||(iInput.ToString() == "-1"))
        //    {
        //        ret = string.Empty;	
        //    }
        //    else
        //    {
        //        ret = iInput.ToString();
        //    }
        //    return ret;
        //  }
        //     catch (Exception Err)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetImmobiliICI.IntForGridView.errore: ", Err);
        //   Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}
    }
}
