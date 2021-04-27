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

using ComPlusInterface;
using Ribes;
using System.Configuration;
using Business;
using System.Globalization;
using log4net;

namespace DichiarazioniICI.CalcoloICI.GestioneElaborazione
{
	/// <summary>
	/// Pagina con la visualizzazione delle elaborazioni massive prodotte.
	/// </summary>
	public partial class Repository :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Repository));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    /// <revisionHistory>
    /// <revision date="04/07/2012">
    /// <strong>IMU</strong>
    /// passaggio tributo da ICI a IMU
    /// </revision>
    /// </revisionHistory>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			try 
			{ 
				Hashtable objHashTable = new Hashtable(); 
				string strConnectionStringOPENgovICI; 
				string strConnectionStringOPENgovProvvedimenti; 
				string strConnectionStringAnagrafica; 
				string strConnectionStringOPENgovTerritorio; 
				string strConnectionStringOPENgovCatasto; 


                //*** 20140509 - TASI ***
                strConnectionStringOPENgovICI = ConstWrapper.StringConnection;
                strConnectionStringAnagrafica = ConstWrapper.StringConnectionAnagrafica;
                //*** ***
				strConnectionStringOPENgovProvvedimenti = "";
				strConnectionStringOPENgovTerritorio = "";
				strConnectionStringOPENgovCatasto = "";
				objHashTable.Add("CodENTE", Utility.StringOperation.FormatString(Session["COD_ENTE"]) ); 
                				
				objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", strConnectionStringOPENgovICI); 
				objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", strConnectionStringOPENgovProvvedimenti); 
				objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica); 
				objHashTable.Add("CONNECTIONSTRINGOPENGOVTERRITORIO", strConnectionStringOPENgovTerritorio); 
				objHashTable.Add("CONNECTIONSTRINGOPENGOVCATASTO", strConnectionStringOPENgovCatasto); 
				objHashTable.Add("USER", Utility.StringOperation.FormatString(Session["username"])); 
				objHashTable.Add("COD_TRIBUTO", Utility.StringOperation.FormatString(Session["COD_TRIBUTO"])); 					
				
				string strTipoElaborazione = Request["TIPO_ELABORAZIONE"].ToString();
				objHashTable.Add("TIPO_ELABORAZIONE", strTipoElaborazione );

				if(!IsPostBack)
				{
					DataSet dsElaborazioni;
					IFreezer remObject =(IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI); 

					dsElaborazioni=remObject.getDATI_TASK_REPOSITORY_CALCOLO_ICI(strConnectionStringOPENgovICI,Utility.StringOperation.FormatString( Session["COD_ENTE"]));
					if (dsElaborazioni.Tables[0].Rows.Count>0)
					{
                        log.Debug("Repository::page_load::sono state trovate elaborazioni");
						ViewState.Add("dsElaborazioni", dsElaborazioni);
						GrdResult.DataSource = dsElaborazioni;
						GrdResult.DataBind();

						lblMessage.Visible=false;
						lblMessage.Text="";
						GrdResult.Visible=true;
					}
					else
					{
                        log.Debug("Repository::page_load::non sono state trovate elaborazioni");
                        lblMessage.Visible = true;
						//*** 20120704 - IMU ***
						lblMessage.Text="Non sono state trovate Elaborazioni Massive.";
						GrdResult.Visible=false;
					}
				}
			} 
			catch (Exception ex) 
			{
                //*** 20120704 - IMU ***
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Repository.Page_Load.errore: ", ex);
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
			//this.GrdResult.ItemDataBound+= new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdResult_ItemDataBound);
		}
        #endregion

        #region "Griglie"
        //protected void GrdRowDataBound(Object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            //prelevo il progressivo elaborazione
        //            e.Row.Attributes.Add("OnClick", "GoToRiepilogoICI(" + e.Row.Cells[4].Text + ")");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Repository.GrdRowDataBound.errore: ", ex);
        //        Response.Redirect("../../../PaginaErrore.aspx");
        //    }
        //}
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        //      private void GrdResult_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //	long idProgrElab=0;
        //try{
        //	if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //	{

        //		//prelevo il progressivo elaborazione
        //		idProgrElab=int.Parse(e.Item.Cells[4].Text);
        //		e.Item.Attributes.Add("OnClick","GoToRiepilogoICI(" + idProgrElab + ")");

        //	}
        // }
        //   catch (Exception ex)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Repository.GrdPageIndexChanging.errore: ", ex);
        //  Response.Redirect("../../../PaginaErrore.aspx");
        // }
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
                GrdResult.DataSource = (DataSet)ViewState["dsElaborazioni"];
                if (page.HasValue)
                    GrdResult.PageIndex = page.Value;
                GrdResult.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Repository.LoadSearch.errore: ", ex);
                Response.Redirect("../../../PaginaErrore.aspx");
                throw ex;
            }
        }


        //protected string FormatDate(object iInput)
        //{
        //    string ret = string.Empty;

        //    IFormatProvider culture = new CultureInfo("it-IT", true);
        //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");
        //try {

        //    if ((iInput.ToString().Length == 0))
        //    {

        //        ret = string.Empty;	
        //    }
        //    else
        //    {
        //        ret = DateTime.Parse(iInput.ToString(),culture).ToString("dd/MM/yyyy HH:mm:ss");
        //    }
        //    return ret;
        // }
        //   catch (Exception ex)
        //  {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Repository.FormatDate.errore: ", ex);
        //  Response.Redirect("../../../PaginaErrore.aspx");
        // }
        //}
    }
}
