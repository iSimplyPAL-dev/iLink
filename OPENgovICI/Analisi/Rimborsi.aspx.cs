using Business;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
namespace DichiarazioniICI.Analisi
{
    /// <summary>
    /// Pagina dei comandi per la consultazione dei contribuenti che hanno diritto ad un rimborso.
    /// Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
    /// Le possibili opzioni sono:
    /// - Stampa
    /// - Ricerca
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class Rimborsi : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Rimborsi));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                sScript = "$('#LblResult').hide();$('#GrdRimborsi').hide();$('#DivRiepTotali').hide();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Rimborsi.Page_Init.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblTitolo.Text = ConstWrapper.DescrizioneEnte;
                if (!IsPostBack)
                {
                    DataView Vista = new Database.Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
                    Vista.Sort = "ANNO DESC";
                    new Business.CoreUtility().loadCombo(ddlAnno, Vista);
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Rimborsi.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #region"Bottoni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                DataView dvMyDati = new Database.DovutoPerContribuente().GetRimborsi(ConstWrapper.CodiceEnte, ddlAnno.SelectedValue);
                Session["TABELLA_RIMBORSI"] = dvMyDati;
                if (dvMyDati.Count > 0)
                {
                    LoadSearch(0);
                    sScript = "$('#LblResult').hide();$('#GrdRimborsi').show();";
                }
                else
                {
                    sScript = "$('#LblResult').show();$('#GrdRimborsi').hide();";
                }
                sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Rimborsi.CmdSearch_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdPrint_Click(object sender, EventArgs e)
        {
            string NameXLS, sPathProspetti,  sScript;
            int x, nCol;
            DataTable DtDatiStampa = new DataTable();
            ArrayList aListColonne;
            string[] aMyHeaders;

            NameXLS = ""; sPathProspetti = "";  sScript = "";
            x = 0; nCol = 0;
            if (Session["TABELLA_RIMBORSI"] == null)
            {
                sScript = "GestAlert('a', 'warning', '', '', 'Effettuare la ricerca!')";
                RegisterScript(sScript, this.GetType());
            }
            else {
                try
                {
                    nCol = 9;
                    NameXLS = ConstWrapper.CodiceEnte + "_RIMBORSI_" +  DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    if (((DataView)Session["TABELLA_RIMBORSI"]).Count > 0)
                    {
                        DtDatiStampa = new Database.StampaXLS().PrintRimborsi((DataView)Session["TABELLA_RIMBORSI"], ConstWrapper.CodiceEnte + "-" + ConstWrapper.DescrizioneEnte, nCol);
                    }
                }
                catch (Exception Err)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Rimborsi.CmdPrint_Click.errore: ", Err);
                    Response.Redirect("../../PaginaErrore.aspx");
                }
                finally
                {
                    sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';";
                    RegisterScript(sScript, this.GetType());
                }
                if (DtDatiStampa != null)
                {
                    aListColonne = new ArrayList();
                    for (x = 0; x < nCol; x++)
                    {
                        aListColonne.Add("");
                    }
                    aMyHeaders = ((string[])(aListColonne.ToArray(typeof(string))));
                    int[] MyCol = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                    new RKLib.ExportData.Export("Web").ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti + NameXLS);
                }
            }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdSorting(Object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (e.SortExpression.ToString() == ViewState["SortKey"].ToString())
                {
                    switch (bool.Parse(ViewState["OrderBy"].ToString()))
                    {
                        case TipoOrdinamento.Crescente:
                            ViewState["OrderBy"] = TipoOrdinamento.Decrescente;
                            break;
                        case TipoOrdinamento.Decrescente:
                            ViewState["OrderBy"] = TipoOrdinamento.Crescente;
                            break;
                    }
                }
                else {
                    ViewState["SortKey"] = e.SortExpression;
                    ViewState["OrderBy"] = TipoOrdinamento.Crescente;
                }

                DataView myDvResult = new DataView(((DataView)Session["TABELLA_RIMBORSI"]).ToTable().Copy(), "", (!(bool.Parse(ViewState["OrderBy"].ToString())) ? "DESC" : "ASC"), DataViewRowState.CurrentRows);
                GrdRimborsi.DataSource = myDvResult;
                GrdRimborsi.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Rimborsi.GrdSorting.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                string sScript = "$('#LblResult').hide();$('#GrdRimborsi').show();";
                RegisterScript(sScript, this.GetType());
            }
        }
        #endregion
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int page)
        {
            try
            {
                GrdRimborsi.DataSource = (DataView)Session["TABELLA_RIMBORSI"];
                if (page > 0)
                    GrdRimborsi.PageIndex = page;
                GrdRimborsi.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Rimborsi.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
            finally
            {
                string sScript = "$('#LblResult').hide();$('#GrdRimborsi').show();";
                RegisterScript(sScript, this.GetType());
            }
        }
    }
}