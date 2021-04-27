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

namespace DichiarazioniICI.ElaborazioneDocumenti
{
	/// <summary>
	/// Pagina per la selezione delle posizioni da elaborare.
    /// Contiene i filtri di ricerca.
	/// </summary>
	public partial class AvanzamentoElaborazione :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(AvanzamentoElaborazione));

		private DataTable TblCalcolo
		{
			get{
                try { 
				if (Session["TblDaStampare"] != null)
				{
					return (DataTable)Session["TblDaStampare"];
				}
				else
				{
					return null;
				}
                }
                catch (Exception Err)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AvanzamentoElaborazione.TblCalcolo.errore: ", Err);
                    Response.Redirect("../../PaginaErrore.aspx");
                    throw Err;
                }

            }
            set
            {
				Session["TblDaStampare"] = value;
			}
		} 

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            try{
			    if (!IsPostBack)
			    {
                    LoadSearch();
                }
			    else
			    {
				    // devo aggiornare i flag all'interno del datatable. 
				    ControllaCheckbox();
			    }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AvanzamentoElaborazione.Page_Load.errore: ", Err);
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
            LoadSearch( e.NewPageIndex);
        }
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdDaElaborare.DataSource = TblCalcolo;
                    if (page.HasValue)
                    GrdDaElaborare.PageIndex = page.Value;
                GrdDaElaborare.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.LoadSearch.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }

        private void ControllaCheckbox() 
		{ 
			DataTable TblDaStampare = TblCalcolo; 
			try 
			{ 
				foreach (GridViewRow itemGrid in GrdDaElaborare.Rows) 
				{
                    // prendo l'ID da aggiornare 
                    //int IdCalcoloICI = int.Parse(GrdDaElaborare.DataKeys[itemGrid.RowIndex].ToString());
                    int IdCalcoloICI = 0;
                    IdCalcoloICI= int.Parse(((HiddenField)itemGrid.FindControl("hfIdContribuente")).Value);
                    if (((CheckBox)itemGrid.FindControl("chkIncludi")).Checked == true) 
					{ 
                        //*** 20140509 - TASI ***
						//foreach (DataRow row in TblDaStampare.Select("ID_CALCOLO_FINALE_ICI = '" + IdCalcoloICI.ToString() + "'")) 
                        foreach (DataRow row in TblDaStampare.Select("CODCONTRIBUENTE=" + IdCalcoloICI.ToString())) 
						{ 
							row["INCLUDI"] = true; 
						} 
                        //*** ***
					} 
					else 
					{ 
                        //*** 20140509 - TASI ***
						//foreach (DataRow row in TblDaStampare.Select("ID_CALCOLO_FINALE_ICI = '" + IdCalcoloICI.ToString() + "'")) 
                        foreach (DataRow row in TblDaStampare.Select("CODCONTRIBUENTE=" + IdCalcoloICI.ToString())) 
						{ 
							row["INCLUDI"] = false; 
						} 
                        //*** ***
					} 
				}         
				// Memorizzo la datatable modificata 
				TblCalcolo = TblDaStampare; 
			}     
			catch (Exception ex) 
			{ 
				string StrErrore; 
				StrErrore = ex.Message;
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AvanzamentoElaborazione.ControllaChecbox.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            } 
		}

        //public bool ValorizzaCheck(object oCheck)
        //{
        //try{
        //    if (int.Parse(oCheck.ToString()) == 0)
        //    {
        //        return false;
        //    }
        //    else{
        //        return true;
        //    }
   // }
         //   catch (Exception Err)
          //  {
           //     log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AvanzamentoElaborazione.ValorizzaCheck.errore: ", Err);
            //    Response.Redirect("../../PaginaErrore.aspx");
         //   }
//}

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
