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
    /// Pagina per la consultazione degli utenti che hanno pagato in modo diverso rispetto al dovuto.
    /// Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class StampaContribPagatoDiverso : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StampaContribPagatoDiverso));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataView GetAnniDichiarazione()
        {
            DataView VistaAnni = new ContribPagatoDiverso().AnniCaricati();
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
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribPagatoDiverso.Page_Load.errore: ", Err);
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
            this.btnStampaExcel.Click += new EventHandler(this.btnStampaExcel_Click);
            //this.GrdRisultati.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdRisultati_SortCommand);
        }
        #endregion

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
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribPagatoDiverso.LoadSearch.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                DataView Vista = new ContribPagatoDiverso().PagatoDiverso(Ente, Tributo, anno);
                if (Vista != null)
                {
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
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribPagatoDiverso.btnTrova_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
/// <summary>
/// 
/// </summary>
/// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
             int anno;

            anno = int.Parse(ddlAnnoRiferimento.SelectedValue);

                       DataRow dr;
            DataSet ds;
            DataTable dtVersamenti = new DataTable();
            string NameXLS = string.Empty;

            ArrayList arratlistNomiColonne;
            string[] arraystr = null;

            arratlistNomiColonne = new ArrayList();
            try
            {
                //*** 20140630 - TASI ***
                arratlistNomiColonne.Add("");
                //*** ***
                arratlistNomiColonne.Add("");
                arratlistNomiColonne.Add("");
                arratlistNomiColonne.Add("");
                arratlistNomiColonne.Add("");
                arratlistNomiColonne.Add("");
                arratlistNomiColonne.Add("");
                arratlistNomiColonne.Add("");

                arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

                NameXLS = ConstWrapper.CodiceEnte + "_PAGATODIVERSO_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                ds = CreateDataSetDovuto();

                dtVersamenti = ds.Tables["VERSAMENTI"];

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
                {
                    dr[0] = "Elenco contribuenti con versato diverso da importo dovuto - Tutti gli anni ";
                }
                else
                {
                    dr[0] = "Elenco contribuenti con versato diverso da importo dovuto - Anno " + anno;
                }
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
                dr[x] = "Importo tot. versato Euro";
                x++;
                dr[x] = "Importo Dovuto Euro";
                x++;
                dr[x] = "Differenza (Dovuto - Versato)";
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
                    dr[x] = " " + Business.CoreUtility.FormattaGrdEuro(myRow["ImportoPagato"].ToString());
                    x++;
                    dr[x] = " " + Business.CoreUtility.FormattaGrdEuro(myRow["ImportoDovuto"].ToString());
                    x++;
                    dr[x] = " " + Business.CoreUtility.FormattaGrdEuro(myRow["diff"].ToString());
                    dtVersamenti.Rows.Add(dr);
                    //*** ***
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
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribPagatoDiverso.btnStampaExcel_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }

            //definisco l'insieme delle colonne da esportare
            int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7 };
            //esporto i dati in excel
            RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
            objExport.ExportDetails(dtVersamenti, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        }
    }
}
