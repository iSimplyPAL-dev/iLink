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
using Anagrafica.DLL;
using log4net;

namespace OPENgovTOCO.Dichiarazioni
{
    /// <summary>
    /// Pagina per la ricerca dichiarazioni.
    /// Le possibili opzioni sono:
/// - Elenco Dichiarazioni
/// - Inserimento nuova denuncia
/// - Ricerca
    /// </summary>
    public partial class DichiarazioniSearch : BaseEnte
    {
        protected System.Web.UI.WebControls.ImageButton ImageCompleta;
        protected System.Web.UI.WebControls.ImageButton Imagebutton3;
        protected System.Web.UI.WebControls.Label Label35;
        protected System.Web.UI.WebControls.Label Label27;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioniSearch));

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
            //this.GrdDichiarazioni.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdDichiarazioni_ItemCommand);
            //this.GrdDichiarazioni.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdDichiarazioni_SortCommand);

        }
        #endregion

        #region Events
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                LblResultDichiarazioni.Text = "";
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["IdEnte"] != null)
                    {
                        DTO.DichiarazioneSearch SearchParams = new DTO.DichiarazioneSearch();
                        Session["COD_ENTE"] = Request.QueryString["IdEnte"];
                        SearchParams.IdEnte = DichiarazioneSession.IdEnte;
                        SearchParams.CognomeContribuente = (Request.QueryString["Cognome"] == null ? string.Empty : Request.QueryString["Cognome"]);
                        SearchParams.NomeContribuente = (Request.QueryString["Nome"] == null ? string.Empty : Request.QueryString["Nome"]);
                        SearchParams.CodFiscaleContribuente = (Request.QueryString["CodiceFiscale"] == null ? string.Empty : Request.QueryString["CodiceFiscale"]);
                        SearchParams.PIVAContribuente = (Request.QueryString["PartitaIva"] == null ? string.Empty : Request.QueryString["PartitaIva"]);
                        Session["objRicercaDichiarazioniTOCO"] = SearchParams;
                    }
                    RestoreSearch();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        protected void Search_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                Session["objRicercaDichiarazioniTOCO"] = null;
                Session["objRicercaDichiarazioniTOCODataTable"] = null;

                DTO.DichiarazioneSearch SearchParams = new DTO.DichiarazioneSearch();
                SearchParams.IdEnte = DichiarazioneSession.IdEnte;
                SearchParams.CognomeContribuente = TxtCognome.Text.Trim().Replace("*", "");
                SearchParams.NomeContribuente = TxtNome.Text.Trim().Replace("*", "");
                SearchParams.CodFiscaleContribuente = TxtCodFiscale.Text.Trim().Replace("*", "");
                SearchParams.PIVAContribuente = TxtPIva.Text.Trim().Replace("*", "");
                SearchParams.NDichiarazione = TxtNDich.Text;

                DataTable dt = DTO.MetodiDichiarazioneTosapCosap.SearchDichiarazione(SearchParams);

                DataBindGridDichiarazioni(dt, true, 0);

                Session["objRicercaDichiarazioniTOCO"] = SearchParams;
                Session["objRicercaDichiarazioniTOCODataTable"] = dt;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.Search_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        protected void NuovaDichiarazione_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Session["oAnagrafe"] = null;
            Response.Redirect(OSAPPages.DichiarazioniAdd);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        protected void Stampa_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DataTable dtStampa = null;
            string NameXLS = "";
            int[] arrColumnsToExport = null;
            string[] arrHeaders = null;
            try
            {
                NameXLS = DichiarazioneSession.IdEnte + "_ELENCOAUTORIZZAZIONI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                DTO.DichiarazioneSearch SearchParams = new DTO.DichiarazioneSearch();
                SearchParams.IdEnte = DichiarazioneSession.IdEnte;
                SearchParams.CognomeContribuente = TxtCognome.Text.Trim().Replace("*", "");
                SearchParams.NomeContribuente = TxtNome.Text.Trim().Replace("*", "");
                SearchParams.CodFiscaleContribuente = TxtCodFiscale.Text.Trim().Replace("*", "");
                SearchParams.PIVAContribuente = TxtPIva.Text.Trim().Replace("*", "");
                SearchParams.NDichiarazione = TxtNDich.Text;
                dtStampa = DTO.MetodiDichiarazioneTosapCosap.PrintDichiarazioni(DichiarazioneSession.StringConnection,SearchParams);

                arrHeaders = new string[dtStampa.Columns.Count];
                checked
                {
                    for (int i = 0; i < arrHeaders.Length; i++)
                        arrHeaders[i] = string.Empty;
                }
                arrColumnsToExport = new int[arrHeaders.Length];
                checked
                {
                    for (int i = 0; i < arrHeaders.Length; i++)
                        arrColumnsToExport[i] = i;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.Stampa_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            if (dtStampa != null)
            {
                RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
                objStampa.ExportDetails(dtStampa, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }
        }
        //protected void Stampa_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    DataTable dtStampa = null;
        //    string NameXLS = "";
        //    int[] arrColumnsToExport = null;
        //    string[] arrHeaders = null;
        //    try
        //    {
        //        NameXLS = DichiarazioneSession.IdEnte + "_ELENCOAUTORIZZAZIONI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

        //        DTO.DichiarazioneSearch SearchParams = new DTO.DichiarazioneSearch();
        //        SearchParams.IdEnte = DichiarazioneSession.IdEnte;
        //        SearchParams.CognomeContribuente = TxtCognome.Text.Trim().Replace("*", "");
        //        SearchParams.NomeContribuente = TxtNome.Text.Trim().Replace("*", "");
        //        SearchParams.CodFiscaleContribuente = TxtCodFiscale.Text.Trim().Replace("*", "");
        //        SearchParams.PIVAContribuente = TxtPIva.Text.Trim().Replace("*", "");
        //        SearchParams.NDichiarazione = TxtNDich.Text;
        //        dtStampa = DTO.MetodiDichiarazioneTosapCosap.PrintDichiarazioni(SearchParams);

        //        arrHeaders = new string[dtStampa.Columns.Count];
        //        checked
        //        {
        //            for (int i = 0; i < arrHeaders.Length; i++)
        //                arrHeaders[i] = string.Empty;
        //        }
        //        arrColumnsToExport = new int[arrHeaders.Length];
        //        checked
        //        {
        //            for (int i = 0; i < arrHeaders.Length; i++)
        //                arrColumnsToExport[i] = i;
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.Stampa_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //    if (dtStampa != null)
        //    {
        //        RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
        //        objStampa.ExportDetails(dtStampa, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        //    }
        //}

        #region "Griglie"
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    foreach (GridViewRow myRow in GrdDichiarazioni.Rows)
                        if (IDRow == ((HiddenField)myRow.FindControl("hfIdDichiarazione")).Value)
                        {
                            Session["oAnagrafe"] = null;
                            VisualizzaDichiarazione(e);
                        }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.GrdRowCommand.errore: ", Err);
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
            DataBindGridDichiarazioni((DataTable)Session["objRicercaDichiarazioniTOCODataTable"], false, e.NewPageIndex);
        }

        //      private void GrdDichiarazioni_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        // {
        //	if(e.CommandName == "Select")
        //	{
        //		VisualizzaDichiarazione(e);
        //	}
        //}
        //private void GrdDichiarazioni_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        //{

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

        //	DataTable TabellaSort;
        //	TabellaSort = (DataTable) Session["TABELLA_RICERCA"];
        //	TabellaSort.DefaultView.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];

        //	Session["TABELLA_RICERCA"] = TabellaSort;

        //	GrdDichiarazioni.DataSource = TabellaSort.DefaultView;
        //	GrdDichiarazioni.DataBind();
        // }
        //       catch (Exception Err)
        //   {
        //       Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.GrdDichiarazioni_ItemCommand.errore: ", Err);
        //       Response.Redirect("../../PaginaErrore.aspx");
        //     }
        //}
        #endregion

        #endregion Events

        #region Private methods
        private void RestoreSearch()
        {
            try
            {
                string nsParam = Request["NewSearch"];
                bool newSearch = true;
                if (nsParam != null &&
                    nsParam.CompareTo("") != 0 &&
                    bool.Parse(nsParam) == false)
                {
                    newSearch = false;
                }

                // Sto tornando alla pagina da quella di visualizzazione dettaglio
                // per cui ripopolo la ricerca
                if (!newSearch)
                {
                    DTO.DichiarazioneSearch objSearch = (DTO.DichiarazioneSearch)Session["objRicercaDichiarazioniTOCO"];
                    if (objSearch != null)
                    {
                        TxtCognome.Text = objSearch.CognomeContribuente;
                        TxtNome.Text = objSearch.NomeContribuente;
                        TxtCodFiscale.Text = objSearch.CodFiscaleContribuente;
                        TxtPIva.Text = objSearch.PIVAContribuente;
                        TxtNDich.Text = objSearch.NDichiarazione;

                        //DataTable dt = (DataTable)Session["objRicercaDichiarazioniTOCODataTable"];
                        DataTable dt = DTO.MetodiDichiarazioneTosapCosap.SearchDichiarazione(objSearch);
                        DataBindGridDichiarazioni(dt, true, 0);
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.RestoreSearch.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        private void DataBindGridDichiarazioni(DataTable dt, bool bRebind, int? page = 0)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    GrdDichiarazioni.DataSource = dt;
                    if (bRebind)
                    {
                        GrdDichiarazioni.DataBind();
                    }
                    if (page.HasValue)
                        GrdDichiarazioni.PageIndex = page.Value;
                    GrdDichiarazioni.DataBind();
                    GrdDichiarazioni.Visible = true;
                    LblResultDichiarazioni.Text = "";
                }
                else
                {
                    LblResultDichiarazioni.Text = "Non sono presenti dichiarazioni";
                    GrdDichiarazioni.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.DataBindGridDichiarazioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        private void VisualizzaDichiarazione(GridViewCommandEventArgs e)
        {
            try
            {
                //Response.Redirect(OSAPPages.DichiarazioniView + "?IdDichiarazione=" + e.CommandArgument.ToString());
                string sScript = "location.href='" + OSAPPages.DichiarazioniView + "?IdDichiarazione=" + e.CommandArgument.ToString()+"';";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniSearch.VisualizzaDichiarazione.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #endregion Private methods
    }
}
