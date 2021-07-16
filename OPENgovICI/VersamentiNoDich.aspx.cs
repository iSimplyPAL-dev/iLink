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
using RKLib;
using log4net;

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina per la consultazione dei versamenti non abbinati a dichiarazione.
    /// Contiene i parametri di calcolo, le funzioni della comandiera e i frame per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class VersamentiNoDich :BasePage
	{
		protected System.Web.UI.WebControls.Button btnNuovoVersamento;
		protected System.Web.UI.WebControls.Button btnRibalta;
		protected System.Web.UI.WebControls.Button Button1;
        private static readonly ILog log = LogManager.GetLogger(typeof(VersamentiNoDich));
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
			//this.GrdRisultati.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdRisultati_SortCommand);
		}
        #endregion
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (!Page.IsPostBack)
                {

                    Session["TabellaRisultati"] = null;
                    // memorizzo il modo di ordeinare le posizioni
                    ViewState.Add("SortKey", "ANNORIFERIMENTO");
                    ViewState.Add("OrderBy", "ASC");


                    // popolo la combo con gli anni di riferimento
                    ListItem Li = new ListItem("...", "");
                    ddlAnno.Items.Add(Li);
                    DataView dvAnni = new VersamentiNoDichView().AnniCaricati();
                    foreach (DataRow drAnni in dvAnni.Table.Rows)
                    {
                        Li = new ListItem(drAnni["AnnoRiferimento"].ToString(), drAnni["AnnoRiferimento"].ToString());
                        ddlAnno.Items.Add(Li);
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }

        }

        //public string IntForGridView(object iInput)
        //{
        //    string ret = string.Empty;
        //try
        //    if (iInput != DBNull.Value && iInput.ToString() !="")
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
        //}
        //  catch (Exception Err)
        // {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.IntForGridView.errore: ", Err);
        //   Response.Redirect("../PaginaErrore.aspx");
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
        //            ret = string.Empty;	
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
        //}
        //  catch (Exception Err)
        // {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.EuroForGridView.errore: ", Err);
        //   Response.Redirect("../PaginaErrore.aspx");
        // }
        //}
        ////*** 20120828 - IMU adeguamento per importi statali ***
        //public string SumEuroForGridView(object oImpComune, object oImpStato)
        //{
        //try{
        //    string ret = string.Empty;
        //    double myImp=0;

        //    if (oImpComune != DBNull.Value)
        //    {

        //        if ((oImpComune.ToString() != "-1") && (oImpComune.ToString() != "-1,00"))
        //        {
        //            myImp = Convert.ToDouble(oImpComune);
        //        }
        //    }
        //    if (oImpStato != DBNull.Value)
        //    {

        //        if ((oImpStato.ToString() != "-1") && (oImpStato.ToString() != "-1,00"))
        //        {
        //            myImp += Convert.ToDouble(oImpStato);
        //        }
        //    }
        //    ret = Convert.ToDecimal(myImp).ToString("N");	
        //    return ret;
        //}
        //  catch (Exception Err)
        // {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.SumEuroForGridView.errore: ", Err);
        //   Response.Redirect("../PaginaErrore.aspx");
        // }
        //}
        ////*** ***

        //public string AccontoSaldo(object saldo, object acconto)
        //{
        //try{
        //    string valore = string.Empty;

        //    if (Convert.ToBoolean(acconto)==true)
        //        valore = "Acconto";
        //    if (Convert.ToBoolean(saldo)==true)
        //        valore = "Saldo";
        //    if ((Convert.ToBoolean(acconto)==true) && (Convert.ToBoolean(saldo)==true))
        //        valore = "Unica Soluzione";	
        //    return valore;
        //}
        //  catch (Exception Err)
        // {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.AccontoSaldo.errore: ", Err);
        //   Response.Redirect("../PaginaErrore.aspx");
        // }
        //}

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
        //      private void GrdRisultati_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        //{
        //try{
        //	if (e.SortExpression.ToString().CompareTo(ViewState["SortKey"].ToString()) == 0)
        //	{
        //		switch (ViewState["OrderBy"].ToString())
        //		{
        //			case "ASC":
        //				ViewState["OrderBy"] = "DESC";
        //				break;
        //			case "DESC":
        //				ViewState["OrderBy"] = "ASC";
        //				break;
        //		}
        //	}
        //	else
        //	{
        //		ViewState["SortKey"] = e.SortExpression.ToString();
        //		ViewState["OrderBy"] = "ASC";
        //	}
        //	DataTable VistaOrdinata;
        //	VistaOrdinata = (DataTable)Session["TabellaRisultati"];
        //	VistaOrdinata.DefaultView.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];
        //	Session["TabellaRisultati"] = VistaOrdinata;

        //	//DataTable TabSource = (DataTable)Session["TABELLA_RICERCA"];	
        //	GrdRisultati.DataSource = VistaOrdinata;
        //	/*GrdRisultati.DataBind();*/
        //}
        //  catch (Exception Err)
        // {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.GrdRisultati_SortCommand.errore: ", Err);
        //   Response.Redirect("../PaginaErrore.aspx");
        // }
        //}

        // FUNZIONI PER LA STAMPA IN EXCEL
        #endregion
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdRisultati.DataSource = (DataTable)Session["TabellaRisultati"];
                if (page.HasValue)
                    GrdRisultati.PageIndex = page.Value;
                GrdRisultati.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.LoadSearch.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }

        protected void btnTrova_Click(object sender, System.EventArgs e)
		{
            try { 
			DataView DvGriglia = new VersamentiNoDichView().List(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value.ToString());
			if (DvGriglia.Count.CompareTo(0) != 0) 
			{				
				GrdRisultati.DataSource = DvGriglia.Table;
				GrdRisultati.DataBind();
				Session["TabellaRisultati"] = DvGriglia.Table;
			}
			else
			{
				lblRisultati.Text = "Non ci sono risultati per i parametri di Ricerca inseriti.";
				lblRisultati.Visible = true;
			}
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.btnTrova_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
            

		private DataSet CreateDataSetVersamenti()
		{
			DataSet dsTmp = new DataSet();

			dsTmp.Tables.Add("VERSAMENTI");
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			return dsTmp;
		}

		protected void btnStampaExcel_Click(object sender, System.EventArgs e)
		{
		
			//			try
			//			{

			DataRow dr;
			DataSet ds;
			DataTable dtVersamenti;
			string NameXLS =string.Empty;

			ArrayList arratlistNomiColonne;
			string[] arraystr;

			arratlistNomiColonne = new ArrayList();
            try { 
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");
			arratlistNomiColonne.Add("");

			arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

            NameXLS =ConstWrapper.CodiceEnte+ "_VERSAMENTINODICH_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
			                
			ds = CreateDataSetVersamenti();

			dtVersamenti = ds.Tables["VERSAMENTI"];

			//inserisco intestazione - titolo prospetto + data stampa
			dr = dtVersamenti.NewRow();
			dr[0] = "Prospetto Versamenti non abbinati a Dichiarazioni";
			dr[2] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
			dtVersamenti.Rows.Add(dr);

			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);
			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);

			//inserisco intestazione di colonna
			dr = dtVersamenti.NewRow();
			dr[0] = "Anno Riferimento";
			dr[1] = "Codice Fiscale";
			dr[2] = "Partita Iva";
			//dr[3] = "Nominativo";
			// 05092007 Cognome Nome separati
			dr[3] = "Cognome";
			dr[4] = "Nome";
			dr[5] = "Data Pagamento";
			dr[6] = "Tipo Pagamento";					
			//*** 20120828 - IMU adeguamento per importi statali ***
			dr[7] = "Importo Terreni";
			dr[8] = "Importo Terreni Stato";
			dr[9] = "Importo Aree Fabbricabili";
			dr[10] = "Importo Aree Fabbricabili Stato";
			dr[11] = "Importo Altri Fabbricati";
			dr[12] = "Importo Altri Fabbricati Stato";
			dr[13] = "Importo Fabbricati Rurali Uso Strumentale";
			//*** ***
			dr[14] = "Importo Abitaz. Principale";
			dr[15] = "Importo Pagato";

			dtVersamenti.Rows.Add(dr);
			DataView  dvVers=new DataView();
			DataTable dtVers=(DataTable)Session["TabellaRisultati"];
			dvVers = dtVers.DefaultView;

                foreach (DataRow myRow in dvVers.Table.Rows)
                {
                    dr = dtVersamenti.NewRow();

				string cognome="";

				//* 05092007 se la data di morte è valorizzata ed è diversa da minvalue cognome=EREDE DI cognome **/
				if((myRow["DATA_MORTE"] != DBNull.Value)&&(myRow["DATA_MORTE"].ToString() != null)&&(myRow["DATA_MORTE"].ToString() != ""))
				{
					string dataMorte, giornoM, meseM, annoM;
					dataMorte=myRow["DATA_MORTE"].ToString();
					giornoM = dataMorte.Substring(6,2);
					meseM = dataMorte.Substring(4,2);
					annoM = dataMorte.Substring(0,4);

					DateTime dataM;
					dataM = new DateTime(Convert.ToInt16(annoM),Convert.ToInt16(meseM),Convert.ToInt16(giornoM));

					if (dataM != DateTime.MinValue)
						cognome = "EREDE DI " + myRow["COGNOME"].ToString();
					else
						cognome = myRow["COGNOME"].ToString();
				}
				else
				{
					cognome = myRow["COGNOME"].ToString();
				}

				dr[0] = myRow["ANNORIFERIMENTO"].ToString();
				dr[1] = myRow["CodiceFiscale"].ToString().ToUpper ();
				if (myRow["PartitaIVA"]!=DBNull.Value && myRow["PartitaIVA"].ToString() !="" && myRow["PartitaIVA"].ToString() !=null)
					dr[2] = "."+myRow["PartitaIVA"].ToString();
				else
					dr[2] = "";
				//dr[3] = myRow["COGNOME"].ToString() + myRow["NOME"].ToString();
				dr[3] = cognome;
				dr[4] = myRow["NOME"].ToString();
				dr[5] = ((DateTime)myRow["Datapagamento"]).ToShortDateString();
                dr[6] = Business.CoreUtility.FormattaGrdAccontoSaldo(myRow["Saldo"].ToString(), myRow["Acconto"].ToString());				
				//*** 20120828 - IMU adeguamento per importi statali ***
                dr[7] = Business.CoreUtility.FormattaGrdInt(myRow["ImpoTerreni"].ToString());
                dr[8] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoTerreniStatale"].ToString());
                dr[9] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoAreeFabbric"].ToString());
                dr[10] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoAreeFabbricStatale"].ToString());
                dr[11] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoAltriFabbric"].ToString());
                dr[12] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoAltriFabbricStatale"].ToString());
                dr[13] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoFabRurUsoStrum"].ToString());
				//*** ***
                dr[14] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoAbitazPrincipale"].ToString());
                dr[15] = Business.CoreUtility.FormattaGrdInt(myRow["ImportoPagato"].ToString());
                    
				dtVersamenti.Rows.Add(dr);
			}
			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);
			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);

			//inserisco numero totali di contribuenti
			dr = dtVersamenti.NewRow();
			dr[0] = "Totale Versamenti non abbinati: " + (dvVers.Count);
			dtVersamenti.Rows.Add(dr);

			//log.Debug("Stampa Dichiarazione da Bonificare");


                //			}
                //			catch (Exception er)
                //			{
                //				string str;
                //				str = "<script language='javascript'>alert('" + er.ToString() +"');</script>";
                //				RegisterScript(this.GetType(),"uscita", str);
                //				
                //			}

            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDich.btnStampaExcel_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
			//definisco l'insieme delle colonne da esportare
			int[] iColumns ={ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,12,13,14,15};
			//esporto i dati in excel
			RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
			objExport.ExportDetails(dtVersamenti, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        }
    }
	}

