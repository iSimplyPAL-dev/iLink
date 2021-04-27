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
using log4net;

namespace DichiarazioniICI//.Analisi
{
    /// <summary>
    /// Pagina per la consultazione degli utenti che non hanno provveduto al pagamento.
    /// Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class StampaContribNonPagato :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(StampaContribNonPagato));

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
			//this.GrdRisultati.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(GrdRisultati_SortCommand);
            this.btnStampaExcel.Click += new EventHandler(this.btnStampaExcel_Click);
        }
		#endregion

		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns>DataView</returns>
		protected DataView GetAnniDichiarazione()
		{
			DataView VistaAnni = new Database.ContribPagatoDiverso().AnniCaricati();
			//VistaAnni.Sort = "Anno";
			return VistaAnni;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (!IsPostBack)
                {
                    ViewState.Add("SortKey", "USERNAME");
                    ViewState.Add("OrderBy", "ASC");

                    ListItem myListItem = new ListItem();
                    myListItem.Text = "...";
                    myListItem.Value = "0";
                    ddlAnnoRiferimento.Items.Add(myListItem);

                    DataView myDataview = GetAnniDichiarazione();

                    //	if(myDataview.Count !=0)
                    //	{
                    foreach (DataRow myRow in myDataview.Table.Rows)
                    {
                        if (myRow["ANNO"].ToString().CompareTo("0") != 0)
                        {
                            ListItem myListItem1 = new ListItem();
                            myListItem1.Text = myRow["ANNO"].ToString();
                            myListItem1.Value = myRow["ANNO"].ToString();
                            ddlAnnoRiferimento.Items.Add(myListItem1);
                        }
                    }
                    /*	}
                        else
                        {
                            ListItem myListItem = new ListItem();
                            myListItem.Text = "Nessun Anno Presente";
                            myListItem.Value = "0";
                            ddlAnnoRiferimento.Items.Add(myListItem);
                        }*/
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.Page_Load.errore: ", Err);
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
                GrdRisultati.DataSource = (DataView)Session["VistaVersamenti"];
                if (page.HasValue)
                    GrdRisultati.PageIndex = page.Value;
                GrdRisultati.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }
        //*** 20140630 - TASI ***
        /// <summary>
        /// Permette di effettuare una ricerca
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">System.EventArgs</param>
        //protected void btnTrova_Click(object sender, System.EventArgs e){

        //    int Nrecord, NVersamenti, presente;
        //    string ente, anno;
        //try{
        //    anno =  ddlAnnoRiferimento.SelectedValue;
        //    if (anno=="0" || anno==""){
        //        RegisterScript(this.GetType(),"","<script>controlloanno();</script>");
        //        //anno="";
        //        //for (indice=1; indice<ddlAnnoRiferimento.Items.Count; indice++)
        //        //{
        //        //	anno+=ddlAnnoRiferimento.Items[indice].Text+",";
        //        //}
        //        //anno= anno.Substring(0,(anno.Length)-1);
        //        return;
        //    }

        //    ente = ConstWrapper.CodiceEnte;

        //    // Tutti i dovuti calcolati per l'anno selezionato
        //    DataView tabella = new Database.StampaContribNonPagato().DovutoPerAnno(anno,ente);
        //    Nrecord = tabella.Count;

        //    string codContribuente,contribuente, contribuenteVers, codContribuenteVers, idPersona, idPersonatemp;
        //    string idNonTrovato, idZero;
        //    contribuente = string.Empty;
        //    codContribuente= string.Empty;
        //    idZero = string.Empty;

        //    codContribuenteVers=string.Empty;
        //    contribuenteVers= string.Empty;
        //    idNonTrovato=string.Empty;

        //    if(Nrecord >0)
        //    {
        //        for(int i=0; i<tabella.Count;i++)
        //        {
        //            codContribuente = codContribuente + tabella[i].Row["COD_CONTRIBUENTE"].ToString() + ",";
        //        }
        //        if (codContribuente !="")
        //            contribuente = codContribuente.Substring(0,(codContribuente.Length)-1);
        //        //}

        //        // Tutti i contribuenti che hanno effettuato versamenti nell'anno selezionato
        //        DataView elencoVersamenti = new Database.StampaContribNonPagato().IdAnagraficiVersamenti(ente, anno);
        //        NVersamenti= elencoVersamenti.Count;

        //        if(NVersamenti >0)
        //        {
        //            for(int j=0; j<elencoVersamenti.Count; j++)
        //            {
        //                codContribuenteVers = codContribuenteVers + elencoVersamenti[j].Row["IdAnagrafico"].ToString() + ",";
        //            }
        //            //if (codContribuenteVers !="")
        //            //	contribuenteVers = codContribuenteVers.Substring(0,(codContribuenteVers.Length)-1);
        //            contribuenteVers=","+codContribuenteVers;
        //        }

        //        if(Nrecord >0)
        //        {
        //            // Tutti i contribuenti che non hanno fatto versamenti
        //            for(int k=0; k<tabella.Count;k++)
        //            {
        //                idPersona = tabella[k].Row["COD_CONTRIBUENTE"].ToString();
        //                idPersonatemp = ","+tabella[k].Row["COD_CONTRIBUENTE"].ToString()+",";
        //                presente = contribuenteVers.IndexOf(idPersonatemp, 0);

        //                if (presente ==-1)
        //                {
        //                    idNonTrovato= idNonTrovato + idPersona + ",";
        //                }					
        //            }
        //            if (idNonTrovato !="")
        //                idNonTrovato = idNonTrovato.Substring(0,(idNonTrovato.Length)-1);
        //        }

        //        // Tutti i contribuenti con versamenti con importoPagato =0
        //        DataView VersamentiZero = new Database.StampaContribNonPagato().VersamentiImportoZero(ente, anno);

        //        if(VersamentiZero.Count >0)
        //        {
        //            for(int c=0; c < VersamentiZero.Count; c++)
        //            {
        //                idZero = idZero + VersamentiZero[c].Row["IdAnagrafico"].ToString() + ",";
        //            }
        //            if (idZero != "")
        //                idZero = idZero.Substring(0,(idZero.Length)-1);
        //        }

        //        // IdContribuente non presente nella tabella dei versamenti e quelli presenti ma con importo=0
        //        if(idZero !="")
        //        {
        //            if(idNonTrovato!="")
        //                idZero = idZero + "," + idNonTrovato;
        //            //else
        //            //    idZero= idZero;
        //        }
        //        else
        //        {
        //            if(idNonTrovato!="")
        //                idZero = idNonTrovato;
        //            else
        //                idZero= "";
        //        }


        //        // Contribuenti che non hanno pagato
        //        DataView ContribuentiNonPag = new Database.StampaContribNonPagato().ContribuentiNonPagato(idZero, anno, ente);		

        //        Session["VistaDovuto"] = ContribuentiNonPag;
        //        if(ContribuentiNonPag.Count > 0)
        //            ContribuentiNonPag.Sort = "Cognome";
        //        GrdRisultati.AllowCustomPaging = false;
        //        GrdRisultati.start_index = (0).ToString();
        //        GrdRisultati.DataSource = ContribuentiNonPag;
        //        GrdRisultati.Rows.Count = ContribuentiNonPag.Count;
        //        GrdRisultati.CurrentPageIndex = 0;

        //        if (ContribuentiNonPag.Count != 0)
        //        {
        //            lblRisultati.Text = "Risultati Ricerca";
        //            GrdRisultati.DataBind();
        //            GrdRisultati.Visible = true;
        //        }
        //        else
        //        {
        //            lblRisultati.Text = "La ricerca non ha prodotto risultati.";
        //            GrdRisultati.Visible = false;
        //        }
        //    }
        //    else{
        //        lblRisultati.Text = "La ricerca non ha prodotto risultati.";
        //        GrdRisultati.Visible = false;
        //    }
        // }
        //   catch (Exception Err)
        //  {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.btnTrova_Click.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //  }
        //}
        protected void btnTrova_Click(object sender, System.EventArgs e)
        {
            try
            {
                string anno = string.Empty;
                string Ente = ConstWrapper.CodiceEnte;
                string Tributo = string.Empty;
                if (ddlAnnoRiferimento.SelectedValue != "0")
                    anno = ddlAnnoRiferimento.SelectedValue.ToString();
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;

                DataView Vista = new ContribPagatoDiverso().NonPagato(Ente, Tributo, anno);
                Session["VistaVersamenti"] = Vista;
                if (Vista.Count > 0)
                    Vista.Sort = "Cognome";
                GrdRisultati.DataSource = Vista;
                if (Vista.Count != 0)
                {
                    lblRisultati.Text = "Risultati Ricerca";
                    GrdRisultati.DataBind();
                    GrdRisultati.Visible = true;
                }
                else
                {
                    lblRisultati.Text = "La ricerca non ha prodotto risultati.";
                    GrdRisultati.Visible = false;
                }
                DivAttesa.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.btnTrova_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /*
		/// <summary>
		/// Permette di ordinare un datagrid
		/// </summary>
		/// <param name="source">object</param>
		/// <param name="e">System.Web.UI.WebControls.DataGridSortCommandEventArgs</param>
		private void GrdRisultati_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
        try{
			if (e.SortExpression.ToString().CompareTo(ViewState["SortKey"].ToString()) == 0)
			{
				switch (ViewState["OrderBy"].ToString())
				{
					case "ASC":
						ViewState["OrderBy"] = "DESC";
						break;
					case "DESC":
						ViewState["OrderBy"] = "ASC";
						break;
				}
			}
			else
			{
				ViewState["SortKey"] = e.SortExpression.ToString();
				ViewState["OrderBy"] = "ASC";
			}
			DataView VistaOrdinata;
			VistaOrdinata = (DataView)Session["VistaDovuto"];
			VistaOrdinata.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];
			Session["VistaDovuto"] = VistaOrdinata;

			//DataTable TabSource = (DataTable)Session["VistaDovuto"];	
			GrdRisultati.DataSource = VistaOrdinata;
			GrdRisultati.DataBind();
              }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.grdRisultati_SortCommand.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
		}
        */
        private DataSet CreateDataSetDovuto()
        {
            DataSet dsTmp = new DataSet();

            dsTmp.Tables.Add("VERSAMENTI");
            //*** 20140630 - TASI ***
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            //*** ***
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            return dsTmp;
        }

        //*** 20140630 - TASI ***
        /// <summary>
        /// Permette di esportare in Excel la ricerca effettuata
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">System.EventArgs</param>
        //protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        //{

        //    string ente, codFisc_Piva, annopag;
        //    int anno;

        //    anno = int.Parse(ddlAnnoRiferimento.SelectedValue);

        //    ente = ConstWrapper.CodiceEnte;

        //    DataRow dr;
        //    DataSet ds;
        //    DataTable dtVersamenti;
        //    string sPathProspetti =string.Empty;
        //    string NameXLS =string.Empty;

        //    ArrayList arratlistNomiColonne;
        //    string[] arraystr;

        //    arratlistNomiColonne = new ArrayList();
        //try{
        //    IFormatProvider culture = new CultureInfo("it-IT", true);
        //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");


        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");

        //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

        //    sPathProspetti = System.Configuration.ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"].ToString();
        //    NameXLS = "ContribuentiNonPagato" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";


        //    ds = CreateDataSetDovuto();

        //    dtVersamenti = ds.Tables["VERSAMENTI"];

        //    //inserisco intestazione di colonna
        //    // COMUNE DI DESCRIZIONE ENTE
        //    dr = dtVersamenti.NewRow();
        //    dr[0] = ConstWrapper.DescrizioneEnte;
        //    dr[3]= "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;;
        //    dtVersamenti.Rows.Add(dr);

        //    //aggiungo uno spazio
        //    dr = dtVersamenti.NewRow();					
        //    dtVersamenti.Rows.Add(dr);

        //    //inserisco intestazione - titolo prospetto + data stampa
        //    dr = dtVersamenti.NewRow();
        //    if (anno==0)
        //        dr[0] = "Elenco contribuenti con versato uguale a zero e con dovuto maggiore di zero - Tutti gli anni";
        //    else
        //        dr[0] = "Elenco contribuenti con versato uguale a zero e con dovuto maggiore di zero - Anno " + anno;

        //    dtVersamenti.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtVersamenti.NewRow();
        //    dtVersamenti.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtVersamenti.NewRow();
        //    dtVersamenti.Rows.Add(dr);

        //    //inserisco intestazione di colonna
        //    dr = dtVersamenti.NewRow();
        //    dr[0] = "Cognome";
        //    dr[1] = "Nome";
        //    dr[2] = "Codice Fiscale/Partita IVA";
        //    dr[3] = "Importo Dovuto Euro";
        //    dr[4] = "Anno";
        //    dtVersamenti.Rows.Add(dr);

        //    DataView  dvVers=new DataView();
        //    dvVers=(DataView)Session["VistaDovuto"];


        //    for (int i = 0; i < dvVers.Count; i++)
        //    {

        //        if(dvVers[i].Row["CodiceFiscale"].ToString() !="")
        //            codFisc_Piva= dvVers[i].Row["CodiceFiscale"].ToString();
        //        else
        //        {
        //            if(dvVers[i].Row["PartitaIva"].ToString() !="")
        //                codFisc_Piva = dvVers[i].Row["PartitaIva"].ToString();
        //            else
        //                codFisc_Piva="";
        //        }

        //        /** 06092007 se la data di morte è valorizzata ed è diversa da minvalue cognome=EREDE DI cognome **/

        //        string cognome="";
        //        if((dvVers[i].Row["DATA_MORTE"] != DBNull.Value)&&(dvVers[i].Row["DATA_MORTE"].ToString() != null)&&(dvVers[i].Row["DATA_MORTE"].ToString() != ""))
        //        {
        //            string dataMorte, giornoM, meseM, annoM;
        //            dataMorte=dvVers[i].Row["DATA_MORTE"].ToString();
        //            giornoM = dataMorte.Substring(6,2);
        //            meseM = dataMorte.Substring(4,2);
        //            annoM = dataMorte.Substring(0,4);

        //            DateTime dataM;
        //            dataM = new DateTime(Convert.ToInt16(annoM),Convert.ToInt16(meseM),Convert.ToInt16(giornoM));

        //            if (dataM != DateTime.MinValue)
        //                cognome = "EREDE DI " + dvVers[i].Row["COGNOME"].ToString();
        //            else
        //                cognome = dvVers[i].Row["COGNOME"].ToString();
        //        }
        //        else
        //        {
        //            cognome = dvVers[i].Row["COGNOME"].ToString();
        //        }
        //        annopag=dvVers[i].Row["AnnoRiferimento"].ToString();

        //        dr = dtVersamenti.NewRow();
        //        dr[0] = " " + cognome;
        //        dr[1] = " " + dvVers[i].Row["Nome"].ToString();
        //        dr[2] = " " + codFisc_Piva;
        //        dr[3] = " " + EuroForGridView(dvVers[i].Row["ImportoDovuto"].ToString());
        //        dr[4] = " " + annopag;

        //        dtVersamenti.Rows.Add(dr);

        //    }


        //    //inserisco riga vuota
        //    dr = dtVersamenti.NewRow();
        //    dtVersamenti.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtVersamenti.NewRow();
        //    dtVersamenti.Rows.Add(dr);

        //    //inserisco numero totali di contribuenti
        //    dr = dtVersamenti.NewRow();
        //    dr[0] = "Totale contribuenti: " + dvVers.Count;
        //    dtVersamenti.Rows.Add(dr);

        //    //log.Debug("Stampa Dichiarazione da Bonificare");

        //    //definisco l'insieme delle colonne da esportare
        //    int[] iColumns ={ 0, 1, 2, 3, 4};
        //    //esporto i dati in excel
        //    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
        //    objExport.ExportDetails(dtVersamenti, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti + NameXLS);
   // }
         //   catch (Exception ex)
         //   {
             //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.btnStampaExcel_Click.errore: ", ex);
              //  Response.Redirect("../../PaginaErrore.aspx");
           // }
//}
private void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
            int anno;

            anno = int.Parse(ddlAnnoRiferimento.SelectedValue);

            DataRow dr;
            DataSet ds;
            DataTable dtVersamenti;
            string NameXLS = string.Empty;

            ArrayList arratlistNomiColonne;
            string[] arraystr;

            arratlistNomiColonne = new ArrayList();

            //*** 20140630 - TASI ***
            arratlistNomiColonne.Add("");
            //*** ***
            arratlistNomiColonne.Add("");
            arratlistNomiColonne.Add("");
            arratlistNomiColonne.Add("");
            arratlistNomiColonne.Add("");
            arratlistNomiColonne.Add("");

            arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

            NameXLS =ConstWrapper.CodiceEnte+ "_NONPAGATO_" + DateTime.Now.ToString( "yyyyMMdd_hhmmss") + ".xls";

            ds = CreateDataSetDovuto();

            dtVersamenti = ds.Tables["VERSAMENTI"];
            try { 
            //inserisco intestazione di colonna
            // COMUNE DI DESCRIZIONE ENTE
            dr = dtVersamenti.NewRow();
            dr[0] = ConstWrapper.DescrizioneEnte;
            dr[3] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year; ;
            dtVersamenti.Rows.Add(dr);

            //aggiungo uno spazio
            dr = dtVersamenti.NewRow();
            dtVersamenti.Rows.Add(dr);

            //inserisco intestazione - titolo prospetto + data stampa
            dr = dtVersamenti.NewRow();
            if (anno == 0)
                dr[0] = "Elenco contribuenti con versato uguale a zero e con dovuto maggiore di zero - Tutti gli anni";
            else
                dr[0] = "Elenco contribuenti con versato uguale a zero e con dovuto maggiore di zero - Anno " + anno;
            dtVersamenti.Rows.Add(dr);

            //inserisco riga vuota
            dr = dtVersamenti.NewRow();
            dtVersamenti.Rows.Add(dr);
            //inserisco riga vuota
            dr = dtVersamenti.NewRow();
            dtVersamenti.Rows.Add(dr);

            //inserisco intestazione di colonna
            dr = dtVersamenti.NewRow();
            int x = 0;
            dr[x] = "Cognome";
            x++;
            dr[x] = "Nome";
            x++;
            dr[x] = "Codice Fiscale/Partita IVA";
            x++;
            dr[x] = "Anno";
            //*** 20140630 - TASI ***
            x++;
            dr[x] = "Tributo";
            //*** ***
            x++;
            dr[x] = "Importo Dovuto Euro";
            dtVersamenti.Rows.Add(dr);

            DataView dvVers = new DataView();
            dvVers = (DataView)Session["VistaVersamenti"];
                foreach (DataRow myRow in dvVers.Table.Rows)
                {
                    dr = dtVersamenti.NewRow();
                x = 0;
                dr[x] = " " + myRow["cognome"].ToString();
                x++;
                dr[x] = " " + myRow["nome"].ToString();
                x++;
                dr[x] = " " + myRow["cfpiva"].ToString();
                x++;
                dr[x] = " " + myRow["annoriferimento"].ToString();
                x++;
                dr[x] = " " + myRow["descrtributo"].ToString();
                x++;
                dr[x] = " " + Business.CoreUtility.FormattaGrdEuro(myRow["ImportoDovuto"].ToString());
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
            dr[0] = "Totale contribuenti: " + dvVers.Count;
            dtVersamenti.Rows.Add(dr);

            //log.Debug("Stampa Dichiarazione da Bonificare");

            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.btnStampaExcel_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            //definisco l'insieme delle colonne da esportare
            int[] iColumns = { 0, 1, 2, 3, 4, 5};
            //esporto i dati in excel
            RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
            objExport.ExportDetails(dtVersamenti, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel,  NameXLS);
        }
        //*** ***
	}
}
