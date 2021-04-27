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
using DichiarazioniICI.Database;
using Business;
using log4net;

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina di gestione della pertinenza.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ImmobilePertinenza : BaseEnte
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImmobilePertinenza));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
		{
            lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
			// Put user code to initialize the page here
			this.IDTestata = int.Parse(Request["idTestata"]);
			this.IDContribuente = int.Parse(Request["IDContribuente"]);
            try { 
			if (Request["IdOggetto"].CompareTo("") == 0)
			{
				this.IDPertinenza = 0;
			}
			else{
				this.IDPertinenza = int.Parse(Request["IdOggetto"]);
			}			
			
			if (!IsPostBack){
				//GrdImmobili.DataSource = viewDettagliImmobili;
				//dipe 13/03/2008
				GrdImmobili.DataSource = viewDettagliImmobiliTestata;
				GrdImmobili.DataBind();
			}
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobilePertinenza.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
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
			//this.GrdImmobili.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdImmobili_ItemDataBound);
		}
		#endregion

		#region Property
		/// <summary>
		/// Ritorna o assegna l'id della testata/dichiarazione.
		/// </summary>
		protected int IDTestata
		{
			get{return ViewState["IDTestata"] != null ? (int)ViewState["IDTestata"] : 0;}	
			set{ViewState["IDTestata"] = value;}
		}
		/// <summary>
		/// Ritorna o assegna l'id del contribuente.
		/// </summary>
		protected int IDContribuente
		{
			get{return ViewState["IDContribuente"] != null ? (int)ViewState["IDContribuente"] : 0;}	
			set{ViewState["IDContribuente"] = value;}
		}
        /// <summary>
        /// 
        /// </summary>
		protected int IDPertinenza
		{
			get{return ViewState["IDPertinenza"] != null ? (int)ViewState["IDPertinenza"] : 0;}	
			set{ViewState["IDPertinenza"] = value;}
		}
        /// <summary>
        /// 
        /// </summary>
		protected DataView viewDettagliImmobili
		{
            get { return (new DettagliImmobiliView().ListAbitazionePrincipale(this.IDTestata, ConstWrapper.StringConnection)); }
			//get {return (new DettagliImmobiliView().ListByIDTestata(this.IDTestata));}
			set{viewDettagliImmobili = value;}
		}
        /// <summary>
        /// 
        /// </summary>
		protected DataView viewDettagliImmobiliTestata
		{
            get { return (new DettagliImmobiliView().ListAbitazionePrincipaleTestata(this.IDContribuente, ConstWrapper.StringConnection)); }
			set{viewDettagliImmobiliTestata = value;}
		}
		
		/// <summary>
		/// Ritorna o assegna l'id dell'immobile.
		/// </summary>
		protected int IDOggetto
		{
			get{return ViewState["IDOggetto"] != null ? (int)ViewState["IDOggetto"] : 0;}	
			set{ViewState["IDOggetto"] = value;}
        }
		#endregion

		#region Metodi

        //public string IntForGridView(object iInput)
        //{
        //    string ret = string.Empty;
		//try{	
        //    if ((iInput.ToString() == "-1")||(iInput.ToString() == "-1,00"))
        //    {
        //        ret = string.Empty;	
        //    }
        //    else
        //    {
        //        ret = Convert.ToString(iInput);
        //    }
        //    return ret;
       //  catch (Exception Err)
         //   {
            //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobilePertinenza.IntForGridView.errore: ", Err);
             //   Response.Redirect("../PaginaErrore.aspx");
          //  }
    //}

    //protected string FormattaData(object DataDaFormattareParam)
    //{
    //    string result=string.Empty;

    //    result = ((DateTime)DataDaFormattareParam).ToShortDateString();
    //    return result;
    //}

    /// <summary>
    /// Torna l'indirizzo dell'immobile.
    /// </summary>
    /// <param name="idImmobile"></param>
    /// <returns></returns>
    protected string GetIndirizzo(int idImmobile)
		{
			string indirizzo;
            Utility.DichManagerICI.OggettiRow riga = new OggettiTable(ConstWrapper.sUsername).GetRow(idImmobile, ConstWrapper.StringConnection);
			indirizzo = riga.Via;
            try { 

            if (riga.NumeroCivico != -1)
                indirizzo += " " + riga.NumeroCivico.ToString();
            if (riga.Barrato != String.Empty)
                indirizzo += "/" + riga.Barrato;
            if (riga.Interno != String.Empty)
                indirizzo += ", Interno " + riga.Interno;
            if (riga.Scala != String.Empty)
                indirizzo += ", Scala " + riga.Scala;
            if (riga.Piano != String.Empty)
                indirizzo += ", Piano " + riga.Piano;

            return indirizzo;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobilePertinenza.GetIndirizzo.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }

        }

		#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnAssocia_Click(object sender, System.EventArgs e)
		{
			
			int codImmobilePertinenza = -1;
            try { 
			foreach (GridViewRow myRow in GrdImmobili.Rows)
			{
				CheckBox chkSel = (CheckBox)myRow.FindControl("chkPertinenza");
				if (chkSel.Checked)
				{
					codImmobilePertinenza = int.Parse(((HiddenField)myRow.FindControl("hfIDOggetto")).Value);
				}
			}
				string strScript = string.Empty;
				strScript += "PopolaPertinenza("+ codImmobilePertinenza +")";
				RegisterScript( strScript, this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobilePertinenza.btnAssocia_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        #region "Griglie"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((HiddenField)e.Row.FindControl("hfIDOggetto")).Value==this.IDPertinenza.ToString())
                    {
                        CheckBox chkSel = (CheckBox)e.Row.FindControl("chkPertinenza");
                        chkSel.Checked = true;
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobilePertinenza.GrdRowDataBound.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
  //      private void GrdImmobili_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		//{
		//	if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer){
		//		if (GrdImmobili.DataKeys[e.Item.ItemIndex].ToString().CompareTo(this.IDPertinenza.ToString()) == 0)
		//		{
		//			CheckBox chkSel = (CheckBox)e.Item.FindControl("chkPertinenza");
		//			chkSel.Checked = true;
		//		}
		//	}
		//}
        #endregion
    }
}
