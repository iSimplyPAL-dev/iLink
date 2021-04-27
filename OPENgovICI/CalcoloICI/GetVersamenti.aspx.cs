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
using System.Globalization;
using System.Text;
using log4net;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la visualizzazione dei versamenti già presenti per il calcolo.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GetVersamenti : BaseEnte
    {
		protected System.Web.UI.WebControls.Button btnStampaExcel;
        private static readonly ILog log = LogManager.GetLogger(typeof(GetVersamenti));

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="30/06/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                string Ente = ConstWrapper.CodiceEnte;
                string AnnoRiferimento = (Request["ANNO"] == null ? "" : Request["ANNO"].ToString());
                string Cognome = (Request["Cognome"] == null ? "" : Request["Cognome"].ToString());
                string Nome = (Request["Nome"] == null ? "" : Request["Nome"].ToString());
                string CodiceFiscale = (Request["CodiceFiscale"] == null ? "" : Request["CodiceFiscale"].ToString());
                string PartitaIva = (Request["PartitaIVA"] == null ? "" : Request["PartitaIVA"].ToString());
                int CodContrib = -1;
                int IdTipoPagamento = -1;
                int IdTipologia = -1;
                int IdConfrontoImporto = -1;
                double ImportoPagato = 0;
                string DataRiversDa = String.Empty;
                string DataRiversA = String.Empty;
                string Tributo = String.Empty;
                if (Request["COD_TRIBUTO"] != null)
                    Tributo = Request["COD_TRIBUTO"].ToString();
                int.TryParse(Request["COD_CONTRIB"].ToString(), out CodContrib);
                DataView Vista = new VersamentiView().List(ConstWrapper.StringConnection,Tributo, CodContrib, Cognome, Nome, CodiceFiscale, PartitaIva, AnnoRiferimento, Ente, IdTipoPagamento, IdTipologia, IdConfrontoImporto, ImportoPagato, DataRiversDa, DataRiversA,string.Empty);
                Session["VistaVersamenti"] = Vista;
                if (Vista.Count > 0)
                    Vista.Sort = "AnnoRiferimento";
                GrdRisultati.DataSource = Vista;

                if (Vista.Count != 0)
                {
                    GrdRisultati.DataBind();
                    lblRisultati.Text = "Elenco versamenti per l'anno " + AnnoRiferimento;
                }
                else
                {
                    lblRisultati.Text = "Nessun versamento effettuato per l'anno " + AnnoRiferimento;
                }
            }
            catch (Exception Ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetVersamenti.Page_Load.errore: ", Ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void Page_Load(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        string Ente = ConstWrapper.CodiceEnte;
        //        string AnnoRiferimento = (Request["ANNO"] == null ? "" : Request["ANNO"].ToString());
        //        string Cognome = (Request["Cognome"] == null ? "" : Request["Cognome"].ToString());
        //        string Nome = (Request["Nome"] == null ? "" : Request["Nome"].ToString());
        //        string CodiceFiscale = (Request["CodiceFiscale"] == null ? "" : Request["CodiceFiscale"].ToString());
        //        string PartitaIva = (Request["PartitaIVA"] == null ? "" : Request["PartitaIVA"].ToString());
        //        int CodContrib = -1;
        //        int IdTipoPagamento = -1;
        //        int IdTipologia = -1;
        //        int IdConfrontoImporto = -1;
        //        string Importo = String.Empty;
        //        double ImportoPagato = 0;
        //        string DataRiversDa = String.Empty;
        //        string DataRiversA = String.Empty;
        //        //*** 20140630 - TASI ***
        //        string Tributo = String.Empty;
        //        if (Request["COD_TRIBUTO"] != null)
        //            Tributo = Request["COD_TRIBUTO"].ToString();
        //        int.TryParse(Request["COD_CONTRIB"].ToString(), out CodContrib);
        //        //DataView Vista = new VersamentiView().List(Cognome, Nome, CodiceFiscale, PartitaIva, AnnoRiferimento, Ente, IdTipoPagamento, IdTipologia, IdConfrontoImporto, ImportoPagato, DataRiversDa, DataRiversA);
        //        DataView Vista = new VersamentiView().List(Tributo, CodContrib, Cognome, Nome, CodiceFiscale, PartitaIva, AnnoRiferimento, Ente, IdTipoPagamento, IdTipologia, IdConfrontoImporto, ImportoPagato, DataRiversDa, DataRiversA);
        //        Session["VistaVersamenti"] = Vista;
        //        if (Vista.Count > 0)
        //            Vista.Sort = "AnnoRiferimento";
        //        GrdRisultati.DataSource = Vista;

        //        if (Vista.Count != 0)
        //        {
        //            GrdRisultati.DataBind();
        //            lblRisultati.Text = "Elenco versamenti per l'anno " + AnnoRiferimento;
        //        }
        //        else
        //        {
        //            lblRisultati.Text = "Nessun versamento effettuato per l'anno " + AnnoRiferimento;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetVersamenti.Page_Load.errore: ", Ex);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //}

        private void GrdRisultati_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			/*switch(e.CommandName)
			{
				case "Select":
					//ApplicationHelper.LoadFrameworkPage("*", "?IDVersamento=" + GrdRisultati.DataKeys[e.Item.ItemIndex] + "&TYPEOPERATION=GESTIONE");
					ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_VERSAMENTI", "?IDVersamento=" + GrdRisultati.DataKeys[e.Item.ItemIndex] + "&TYPEOPERATION=GESTIONE");
					break;
			}*/
		}



        //public string IntForGridView(object iInput)
        //{
        //    string ret = string.Empty;
        //try{
        //    if (iInput != DBNull.Value)
        //    {
        //        if (Convert.ToDecimal(iInput) == -1)
        //        {
        //            ret = string.Empty;	
        //        }
        //        else
        //        {
        //            ret = Convert.ToString(iInput);
        //        }
        //    }
        //    else
        //    {
        //        ret = string.Empty;
        //    }
        //    return ret;
        //  }
        //    catch (Exception Ex)
        //  {
        //  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetVersamenti.IntForGridView.errore: ", Ex);
        // Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}


        //public string EuroForGridView(object iInput)
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
        //    catch (Exception Ex)
        //  {
        //  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetVersamenti.EuroForGridView.errore: ", Ex);
        // Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}

        //public string AccontoSaldo(object saldo, object acconto)
        //{
        //    string valore = string.Empty;
        //try{
        //    if (Convert.ToBoolean(acconto)==true)
        //        valore = "Acconto";
        //    if (Convert.ToBoolean(saldo)==true)
        //        valore = "Saldo";
        //    if ((Convert.ToBoolean(acconto)==true) && (Convert.ToBoolean(saldo)==true))
        //        valore = "Unica Soluzione";	
        //    return valore;
        //  }
        //    catch (Exception Ex)
        //  {
        //  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetVersamenti.AccontoSaldo.errore: ", Ex);
        // Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}


        //protected string FormattaData(object DataDaFormattareParam)
        //{
        //try{
        //    IFormatProvider culture = new CultureInfo("it-IT", true);
        //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");

        //    string result=string.Empty ;

        //    if (DataDaFormattareParam!=DBNull.Value)
        //    {
        //        result = DateTime.Parse(DataDaFormattareParam.ToString(),culture).ToString("dd/MM/yyyy");			
        //    }

        //    return result ;
        //  }
        //    catch (Exception Ex)
        //  {
        //  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetVersamenti.FormattaData.errore: ", Ex);
        // Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}

        //public string getViolazione(object Violazione)
        //{
        //    string valore = string.Empty;

        //    if (Convert.ToBoolean(Violazione)==true)
        //        valore = "SI";
        //    else
        //        valore="NO";
        //    return valore;
        //}



    }
}
