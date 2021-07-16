using System;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using System.Data;
using System.Collections;
using log4net;

namespace OPENgov.Acquisizioni.AttiCompraVendita
{/// <summary>
/// Pagina per la ricerca e gestione degli atti di compravendita.
/// Le possibili opzioni sono:
/// - Stampa
/// - Ricerca
/// - Gestione
/// - Torna alla videata precedente
/// </summary>
    public partial class CompraVenditaRicerca : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CompraVenditaRicerca));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
                      
            BreadCrumb = "Atti di Compravendita - Ricerca";
            Session["oAnagrafe"] = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCombo();
            if (Request.QueryString["IdAtto"] != null)
            {
                int idAtto;
                int.TryParse(Request.QueryString["IdAtto"], out idAtto);
                ShowDiv("ComandiGestione"); ShowDiv("GestioneAtto");
                HideDiv("ComandiRicerca"); HideDiv("Ricerca");
                LoadDettaglioAtto(idAtto);
            }
            else
            {
                HideDiv("RicercaSoggetto");
                HideDiv("GestioneAtto");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangeParamSearch(object sender, System.EventArgs e)
        {
            if (rbImmobile.Checked == true)
            {
                HideDiv("RicercaSoggetto");
                ShowDiv("RicercaImmobile");
            }
            else if (rbSoggetto.Checked== true)
            {
                HideDiv("RicercaImmobile");
                ShowDiv("RicercaSoggetto");
            }
        }

        #region Griglia Ricerca Atti di Compravendita
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgvAttiPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgvAttiRowCommand(object sender, GridViewCommandEventArgs e)
        {
 
            int idAtto;
            int.TryParse(e.CommandArgument.ToString(), out idAtto);
                switch (e.CommandName)
                {
                    case "EditAtto":
                        ShowDiv("ComandiGestione"); ShowDiv("GestioneAtto");
                        HideDiv("ComandiRicerca");HideDiv("Ricerca");
                        LoadDettaglioAtto(idAtto);
                        break;
                    default:
                        break;
                }
        }
        #endregion

        #region Griglia Gestione Acquirenti
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgvAcquirentiPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgvAcquirentiRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string titolo;
            int idAcquirente;
            int.TryParse(e.CommandArgument.ToString(), out idAcquirente);
            switch (e.CommandName)
            {
                case "EditAcquirente":
                    titolo = "A";
                    LoadPageDichiarazioneICI(idAcquirente, int.Parse(hfidImmobile.Value), Ente, titolo);
                   break;
                default:
                    break;
            }
        }
        #endregion

        #region Griglia Gestione Cessionari
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgvCessionariPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgvCessionariRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string titolo;
            int idCessionario;
            int.TryParse(e.CommandArgument.ToString(), out idCessionario);
            switch (e.CommandName)
            {
                case "EditCessionario":
                    titolo = "C";
                    LoadPageDichiarazioneICI(idCessionario, int.Parse(hfidImmobile.Value), Ente, titolo);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Bottoniera
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            LoadSearch();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdStampaClick(object sender, System.EventArgs e)
        {
            fsRisultatiRicerca.Visible = false;
            DataSet myDsResult = new DataSet();
            int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17};
            string[] aMyHeaders = null;
            ArrayList oListNomiColonne = new ArrayList();
            string sNameXLS =  Ente + "_ATTICOMPRAVENDITA_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

            try
            {                
                Log.Debug("CompraVenditaRicerca::CmdStampa_Click::inizio::" + DateTime.Now.ToString());                
                Log.Debug("CompraVenditaRicerca::CmdStampa_Click::devo prelevare i dati da stampare::" + DateTime.Now.ToString());
                int idStato = 0;
                int.TryParse(rddlStato.SelectedValue, out idStato);
                myDsResult = new CompraVendita().LoadAttiStampa(Ente, txtFoglio.Text, txtNumero.Text, txtSub.Text, txtUbicazione.Text, txtNominativo.Text, rddlTipoImmobile.SelectedValue,idStato);                

                oListNomiColonne.Add("Anno");
                oListNomiColonne.Add("Foglio");
                oListNomiColonne.Add("Numero");
                oListNomiColonne.Add("Subalterno");
                oListNomiColonne.Add("Tipo Immobile");
                oListNomiColonne.Add("Ubicazione");
                oListNomiColonne.Add("Nominativo");
                oListNomiColonne.Add("Cod.Fiscale/P.IVA");
                oListNomiColonne.Add("Diritto");
                oListNomiColonne.Add("Acquisto");
                oListNomiColonne.Add("Cessione");
                oListNomiColonne.Add("Data Validita");
                oListNomiColonne.Add("Cat.");
                oListNomiColonne.Add("Classe");               
                oListNomiColonne.Add("Rendita");                                                              
                oListNomiColonne.Add("Zona");
                oListNomiColonne.Add("Possesso");
                oListNomiColonne.Add("Nota Trascrizione");
            }
            catch (Exception ex)
            {
                Log.Debug("CompraVenditaRicerca::CmdStampa_Click::si è verificato il seguente errore::" + ex.Message);
            }
            finally
            {
                //DivAttesa.Style.Add("display", "none");
            }
            if (myDsResult != null)
            {
                if (myDsResult.Tables.Count > 0)
                {
                    if (myDsResult.Tables[0].Rows.Count > 0)
                    {
                        RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                        aMyHeaders = ((string[])(oListNomiColonne.ToArray(typeof(string))));
                        Log.Debug("CompraVenditaRicerca::CmdStampa_Click::richiamo RKLIB::" + DateTime.Now.ToString());
                        objExport.ExportDetails(myDsResult.Tables[0], iColumns, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS);
                        Log.Debug("CompraVenditaRicerca::CmdStampa_Click::finito RKLIB::" + DateTime.Now.ToString());
                        //LblDownloadFile.Text = sNameXLS;
                    }
                }
            }
            //LblDownloadFile.Style.Add("display", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBackClick(object sender, EventArgs e)
        {
            ShowDiv("ComandiRicerca"); ShowDiv("Ricerca");
            HideDiv("ComandiGestione"); HideDiv("GestioneAtto");
            LoadSearch();
        }
        #endregion

        private void LoadCombo()
        {
            try
            {
                StatoLavoroAtti StatoLavoro = new StatoLavoroAtti { Ente = Ente };
                rddlStato.DataSource = StatoLavoro.LoadAll();
                rddlStato.DataValueField = "IdStato";
                rddlStato.DataTextField = "Definizione";
                rddlStato.DataBind();
                rddlStato.SelectedIndex = 0;

                rddlTipoImmobile.DataSource = StatoLavoro.LoadTipoImmobile();
                rddlTipoImmobile.DataValueField = "Codice";
                rddlTipoImmobile.DataTextField = "Definizione";
                rddlTipoImmobile.DataBind();                
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AttiCompravenditaRicerca.LoadCombo.errore::", ex);;
                throw;
            }
        }
        
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                int idStato = 0;
                int.TryParse(rddlStato.SelectedValue, out idStato);
                DataSet myResult = new DataSet();
                if (rbImmobile.Checked == true)
                {
                    rgvAtti.DataSource = new CompraVendita().LoadAtti(Ente, -1, -1, idStato, txtFoglio.Text, txtNumero.Text, txtSub.Text, txtUbicazione.Text, txtNominativo.Text, rddlTipoImmobile.SelectedValue, 0);
                    if (page.HasValue)
                        rgvAtti.PageIndex = page.Value;
                    rgvAtti.DataBind();
                    rgvAttiSoggetti.Visible = false;
                    HideDiv("RicercaSoggetto");
                    ShowDiv("RicercaImmobile");
                    if (rgvAtti.Rows.Count > 0)
                        lblResult.Visible = false;
                }
                else
                {
                    rgvAttiSoggetti.DataSource = new CompraVendita().LoadAtti(Ente, -1, -1, idStato, txtFoglio.Text, txtNumero.Text, txtSub.Text, txtUbicazione.Text, txtNominativo.Text, rddlTipoImmobile.SelectedValue, 1);
                    if (page.HasValue)
                        rgvAttiSoggetti.PageIndex = page.Value;
                    rgvAttiSoggetti.DataBind();
                    rgvAtti.Visible = false;
                    HideDiv("RicercaImmobile");
                    ShowDiv("RicercaSoggetto");
                    if (rgvAttiSoggetti.Rows.Count > 0)
                        lblResult.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AttiCompravenditaRicerca.LoadSearch.errore::", ex);;
                throw;
            }
        }

        private void LoadDettaglioAtto(int idImmobile)
        {
            try
            {
                CompraVendita Atto = new CompraVendita { IdImmobile = idImmobile };
                if (!Atto.Load()) return;
                hfidImmobile.Value = Atto.IdImmobile.ToString();
                lblNotaTrascrizione.Text = Atto.NotaTrascrizione;
                lblRifNota.Text = Atto.RifNota;
                lblCatNota.Text = Atto.CatNota;
                lblUbicazioneNota.Text = Atto.UbicazioneNota;
                lblUbicazioneCatasto.Text = Atto.UbicazioneCatasto;
                rgvAcquirenti.DataSource = Atto.ListAcquirenti;
                rgvAcquirenti.DataBind();
                rgvCessionari.DataSource = Atto.ListCessionari;
                rgvCessionari.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AttiCompravenditaRicerca.LoadDettaglioAtto.errore::", ex);;
                throw;
            }
        }

        private void LoadPageDichiarazioneICI(int idSoggetto, int idAtto, string IdEnte, string titolo)
        {
            try
            {
                string sScript;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript = "<script language='javascript'>";
                sScript += "parent.Nascosto.location.href='../../aspVuota.aspx';";
                if (idSoggetto <= 0)
                {
                    sScript += "parent.Comandi.location.href='../../" + SiteICI + "/CGestione.aspx?COMPRAVENDITA=COMPRAVENDITA';";
                    sScript += "location.href ='../../" + SiteICI + "/Gestione.aspx?IdAttoSoggetto=" + (idSoggetto * -1).ToString() + "&IdAttoCompravendita=" + idAtto.ToString() + "&IdEnte="+IdEnte+"&titolo="+titolo+"';";
                }
                else
                {
                    sScript += "parent.Comandi.location.href='../../" + SiteICI + "/CGestioneDettaglio.aspx?COMPRAVENDITA=COMPRAVENDITA';";
                    sScript += "location.href ='../../" + SiteICI + "/GestioneDettaglio.aspx?IDContribuente=" + idSoggetto.ToString() + "&Bonificato=-1&IdAttoCompravendita=" + idAtto.ToString() + "&IdEnte=" + IdEnte + "&titolo=" + titolo + "';";
                }
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "loadnewpage", sBuilder.ToString());
                

                Global.Log.Write2(LogSeverity.Critical, sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AttiCompravenditaRicerca.LoadPageDichiarazioneICI.errore::", ex);;
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LblDownloadFile_Click(object sender, System.EventArgs e)
        {
            string sFileExport = CacheManager.PathProspetti + LblDownloadFile.Text;
            Response.ContentType = "*/*";
            Response.AppendHeader("content-disposition", ("attachment; filename=" + LblDownloadFile.Text));
            Response.WriteFile(sFileExport);
            Response.End();
        }
    }
}