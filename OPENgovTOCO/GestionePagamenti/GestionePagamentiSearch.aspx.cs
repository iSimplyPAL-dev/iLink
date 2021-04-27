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

using DTO;
using OPENgovTOCO;
using IRemInterfaceOSAP;
using log4net;

namespace OPENGovTOCO.GestionePagamenti
{
    /// <summary>
    /// Pagina per la ricerca pagamento
    /// </summary>
    public partial class GestionePagamentiSearch :BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GestionePagamentiSearch));
        protected System.Web.UI.WebControls.Label LblResult;
        protected System.Web.UI.WebControls.Button btSearchPag;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try {
                DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerText += " - Pagamenti - Gestione";

                // Recupero la ricerca
                if (!Page.IsPostBack)
                {
                    RestoreSearch();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagamenti"></param>
        /// <param name="StartIndex"></param>
        /// <param name="bRebind"></param>
        /// <param name="page"></param>
        private void DataBindGridPagamenti(PagamentoExt[] pagamenti, int StartIndex, bool bRebind, int? page = 0)
        {
            try {
                if (pagamenti != null && pagamenti.Length > 0)
                {
                    GrdPagamenti.DataSource = pagamenti;
                    if (bRebind)
                    {
                        //--- 20140429 se a CurrentPageIndex viene assegnato 0,
                        //andando a fare click sui pulsanti per cambiare pagine dà errore
                        //perciò l'ho sostituito con la variabile StartIndex che gli viene passata
                        //GrdPagamenti.CurrentPageIndex = 0;
                        GrdPagamenti.DataBind();
                    }
                    if (page.HasValue)
                        GrdPagamenti.PageIndex = page.Value;
                    GrdPagamenti.DataBind();
                    GrdPagamenti.Visible = true;
                    LblResultPagamenti.Visible = false;
                    pnlTotali.Visible = true;

                    CalcolaTotali(pagamenti);
                }
                else
                {
                    LblResultPagamenti.Visible = true;
                    LblResultPagamenti.Text = "Nessun risultato per questa ricerca.";
                    GrdPagamenti.Visible = false;
                    pnlTotali.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.DataBlindGridPagamenti.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagamenti"></param>
        private void CalcolaTotali(PagamentoExt[] pagamenti)
        {
            try {
                int tmpTot = 0;
                double tmpTotImp = 0;
                if (pagamenti != null)
                {
                    tmpTot = pagamenti.Length;

                    foreach (PagamentoExt myRow in pagamenti)
                    {
                        tmpTotImp = tmpTotImp + myRow.ImportoPagato;
                    }
                }

                lblTot.Text = tmpTot.ToString();
                lblTotImp.Text = string.Format("{0:0.00}", tmpTotImp);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.CalcolaTotali.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSearch_OnClick(object sender, System.EventArgs e)
        {
            bool okParam = VerificaParametri();

            try
            {
                if (okParam == true)
                {
                    Log.Debug("faccio ricerca");
                    Session["objGestionePagamentiTOCO_ParamRicerca"] = null;
                    //Session["objGestionePagamentiTOCO_ListaPagamenti"] = null;

                    PagamentiSearch SearchParams = GetSearchParams();
                    PagamentoExt[] pagamenti = MetodiPagamento.GetPagamenti(SearchParams);
                    DataBindGridPagamenti(pagamenti, 0, true);

                    Session["objGestionePagamentiTOCO_ParamRicerca"] = SearchParams;
                    Session["objGestionePagamentiTOCO_ListaPagamenti"] = pagamenti;
                }
                else
                    Log.Debug("no faccio ricerca");

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.btSearch_OnClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx",false);
            }
        }

        #region "Griglie"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    foreach (GridViewRow myRow in GrdPagamenti.Rows)
                        if (IDRow == ((HiddenField)myRow.FindControl("hfIdPagamento")).Value)
                        {
                            bool fail = false;

                            if (SharedFunction.IsNullOrEmpty(((HiddenField)myRow.FindControl("hfIdPagamento")).Value) && SharedFunction.IsNullOrEmpty(((HiddenField)myRow.FindControl("hfCod_Contribuente")).Value))
                                fail = true;


                            if (fail == true)
                            {
                                string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile completare l\'operazione.');";
                                RegisterScript(sScript,this.GetType());
                            }
                            else
                            {
                                Response.Redirect(OSAPPages.GestionePagamentiEdit + "?IdPagamento=" + IDRow + "&CodContribuente=" + ((HiddenField)myRow.FindControl("hfCod_Contribuente")).Value);
                            }
                        }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.GrdRowCommand.errore: ", Err);
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
            DataBindGridPagamenti((PagamentoExt[])Session["objGestionePagamentiTOCO_ListaPagamenti"], e.NewPageIndex, true);
        }
    #endregion
    /// <summary>
    /// Verifica i parametri per la query
    /// </summary>
    /// <returns></returns>
    private bool VerificaParametri()
        {
            if (SharedFunction.IsNullOrEmpty(DichiarazioneSession.IdEnte) == true)
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RestoreSearch()
        {

            try {
                string nsParam = Request["NewSearch"];
                bool newSearch = true;
                if (nsParam != null &&
                    nsParam.CompareTo("") != 0 && bool.Parse(nsParam) == false)
                {
                    newSearch = false;
                }

                // Sto tornando alla pagina da quella di visualizzazione dettaglio
                // per cui ripopolo la ricerca
                if (!newSearch)
                {
                    DTO.PagamentiSearch SearchParams = (DTO.PagamentiSearch)Session["objGestionePagamentiTOCO_ParamRicerca"];
                    if (SearchParams != null)
                    {
                        txtAnnoRif.Text = SearchParams.AnnoRif;
                        txtCognome.Text = SearchParams.Cognome;
                        txtNome.Text = SearchParams.Nome;
                        txtCodiceFiscale.Text = SearchParams.CF;
                        txtPartitaIva.Text = SearchParams.PIVA;
                        txtNAvviso.Text = SearchParams.NAvviso = txtNAvviso.Text;

                        if ((SearchParams.DataAccreditoDal > DateTime.MinValue && SearchParams.DataAccreditoDal < DateTime.MaxValue))
                            txtDataAccreditoDal.Text = SearchParams.DataAccreditoDal.ToString("dd/MM/yyyy");

                        if ((SearchParams.DataAccreditoAl > DateTime.MinValue && SearchParams.DataAccreditoAl < DateTime.MaxValue))
                            txtDataAccreditoAl.Text = SearchParams.DataAccreditoAl.ToString("dd/MM/yyyy");

                        //PagamentoExt[] pagamenti = (PagamentoExt[])Session["objGestionePagamentiTOCO_ListaPagamenti"];
                        PagamentoExt[] pagamenti = MetodiPagamento.GetPagamenti(SearchParams);
                        DataBindGridPagamenti(pagamenti, 0, true);
                    }
                }
                else
                {
                    Session["objRicercaDichiarazioniTOCO"] = null;
                    Session["objRicercaDichiarazioniTOCODataTable"] = null;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.RestoreSearch.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tDataGrd"></param>
        /// <returns></returns>
        public string FormattaDataGrd(DateTime tDataGrd)
        {
            try {
                if (tDataGrd == DateTime.MinValue || tDataGrd == DateTime.MaxValue)
                    return String.Empty;
                else
                    return tDataGrd.ToShortDateString();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.GestionePagamentiSearch.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private PagamentiSearch GetSearchParams()
        {
            try {
                PagamentiSearch SearchParams = new PagamentiSearch();
                SearchParams.IdEnte = DichiarazioneSession.IdEnte;
                SearchParams.IdTributo = DichiarazioneSession.CodTributo("");
                if (!SharedFunction.IsNullOrEmpty(txtAnnoRif.Text))
                    SearchParams.AnnoRif = txtAnnoRif.Text;

                if (!SharedFunction.IsNullOrEmpty(txtCognome.Text))
                    SearchParams.Cognome = txtCognome.Text;

                if (!SharedFunction.IsNullOrEmpty(txtNome.Text))
                    SearchParams.Nome = txtNome.Text;

                if (!SharedFunction.IsNullOrEmpty(txtCodiceFiscale.Text))
                    SearchParams.CF = txtCodiceFiscale.Text;

                if (!SharedFunction.IsNullOrEmpty(txtPartitaIva.Text))
                    SearchParams.PIVA = txtPartitaIva.Text;

                if (!SharedFunction.IsNullOrEmpty(txtNAvviso.Text))
                    SearchParams.NAvviso = txtNAvviso.Text;

                if (!SharedFunction.IsNullOrEmpty(txtDataAccreditoDal.Text))
                {
                    try
                    {
                        SearchParams.DataAccreditoDal = DateTime.ParseExact(txtDataAccreditoDal.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        SearchParams.DataAccreditoDal = DichiarazioneSession.MyDateMinValue;
                    }
                }

                if (!SharedFunction.IsNullOrEmpty(txtDataAccreditoAl.Text))
                {
                    try
                    {
                        SearchParams.DataAccreditoAl = DateTime.ParseExact(txtDataAccreditoAl.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        SearchParams.DataAccreditoAl = DichiarazioneSession.MyDateMinValue;
                    }
                }

                return SearchParams;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.GetSearchParams.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_OnClick(object sender, System.EventArgs e)
        {
            bool okParam = VerificaParametri();


            if (okParam == true)
            {
                DataTable dtRes = null;
              
                string NameXLS = null;
                string[] arrHeaders = null;
                int[] arrColumnsToExport = null;

                try
                {
                    PagamentiSearch SearchParams = GetSearchParams();
                    Session["objGestionePagamentiTOCO_ParamRicerca"] = SearchParams;

                    string commandName = ((Button)sender).CommandName;
                     MetodiPagamento.GetPagamenti(SearchParams, commandName, out dtRes);

                    NameXLS = DichiarazioneSession.IdEnte+"_ESPORTAZIONE_PAGAMENTI" + commandName.ToUpper()+"_"+ DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    arrColumnsToExport = MetodiPagamento.GetColumnsToExport(commandName);
                    switch (commandName)
                    {
                        case "_ExpImportoMaggiore":
                            arrHeaders = MetodiPagamento.COL_HEADERS_MagPag;
                            break;
                        case "_ExpNonPagati":
                        case "_ExpImportoMinore":
                            arrHeaders = MetodiPagamento.COL_HEADERS_MinPag;
                            break;
                        default:
                            arrHeaders = MetodiPagamento.COL_HEADERS_SinglePag;
                            break;
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiSearch.Export_OnClick.errore: ", Err);
                    Response.Redirect("../../PaginaErrore.aspx");
                }
                if (dtRes.Columns.Count > 0 && dtRes.Rows.Count > 0)
                {
                    RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
                    objStampa.ExportDetails(dtRes, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
                }
                else
                {
                    string sScript = "alert(\'Non sono presenti record per questa esportazione.');";
                    RegisterScript(sScript,this.GetType());
                }
                //DataBindGridPagamenti(pagamenti, 0, true);
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
            //this.GrdPagamenti.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdPagamenti_ItemCommand);

        }
        #endregion
    }
}
