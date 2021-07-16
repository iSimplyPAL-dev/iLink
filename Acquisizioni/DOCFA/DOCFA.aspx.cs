using System;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using log4net;

namespace OPENgov.Acquisizioni.DOCFAGestione
{/// <summary>
/// Pagina per la ricerca e gestione dei DOCFA.
/// Le possibili opzioni sono:
/// - Ricerca
/// - Dettaglio
/// - Gestione
/// - Torna alla videata precedente
/// </summary>
    public partial class DOCFAGest : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DOCFAGest));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "DOCFA - Gestione";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                if (IsPostBack) return;
                int idDOCFA = 0;
                if (Request.QueryString["IdDOCFA"] != null)
                    int.TryParse(Request.QueryString["IdDOCFA"], out idDOCFA);

                if (Request.QueryString["IdTestata"] != null)
                    hfOrigineChiamata.Value = "Dich";

                if (hfOrigineChiamata.Value.ToString() == string.Empty)
                {
                    HideDiv("ComandiDettaglio");
                    HideDiv("RicercaDich");
                    HideDiv("DettaglioDOCFA");
                    ShowDiv("RicercaDOCFA"); ShowDiv("ComandiRicerca");
                }
                else if (hfOrigineChiamata.Value.ToString() == "RicercaDich")
                {
                    HideDiv("ComandiDettaglio");
                    HideDiv("RicercaDOCFA");
                    ShowDiv("RicercaDich"); ShowDiv("ComandiRicerca");
                    sScript += "$('#DettaglioDOCFA').removeClass();$('#DettaglioDOCFA').addClass('active');";
                    RegisterScript(sScript);
                    LoadFilesDOCFA(idDOCFA, false);
                }
                else if (hfOrigineChiamata.Value.ToString() == "Dettaglio" || hfOrigineChiamata.Value.ToString() == "Dich")
                {
                    ShowDiv("ComandiDettaglio");
                    HideDiv("RicercaDOCFA");
                    HideDiv("RicercaDich");
                    HideDiv("ComandiRicerca");
                    sScript += "$('#DettaglioDOCFA').removeClass();$('#DettaglioDOCFA').addClass('active');";
                    RegisterScript(sScript);
                    LoadFilesDOCFA(idDOCFA, true);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.DOCFAGestione.Page_Load.errore::", ex); ;
                throw;
            }
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (IsPostBack) return;
        //        int idDOCFA = 0;
        //        if (Request.QueryString["IdDOCFA"] != null)
        //            int.TryParse(Request.QueryString["IdDOCFA"], out idDOCFA);

        //        if (Request.QueryString["IdTestata"] != null)
        //            hfOrigineChiamata.Value = "Dich";

        //        if (hfOrigineChiamata.Value.ToString() == string.Empty)
        //        {
        //            HideDiv("ComandiDettaglio");
        //            HideDiv("RicercaDich");
        //            HideDiv("DettaglioDOCFA");
        //            ShowDiv("RicercaDOCFA"); ShowDiv("ComandiRicerca");
        //        }
        //        else if (hfOrigineChiamata.Value.ToString() == "RicercaDich")
        //        {
        //            HideDiv("ComandiDettaglio");
        //            HideDiv("RicercaDOCFA");
        //            ShowDiv("RicercaDich"); ShowDiv("ComandiRicerca");
        //            ShowDiv("DettaglioDOCFA");
        //            LoadFilesDOCFA(idDOCFA, false);
        //        }
        //        else if (hfOrigineChiamata.Value.ToString() == "Dettaglio" || hfOrigineChiamata.Value.ToString() == "Dich")
        //        {
        //            ShowDiv("ComandiDettaglio");
        //            HideDiv("RicercaDOCFA");
        //            HideDiv("RicercaDich");
        //            ShowDiv("DettaglioDOCFA"); HideDiv("ComandiRicerca");
        //            LoadFilesDOCFA(idDOCFA, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("OPENgov.20.DOCFAGestione.Page_Load.errore::", ex); ;
        //        throw;
        //    }
        //}

        #region Bottoniera
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            LoadSearch();
        }

        protected void CmdBackClick(object sender, EventArgs e)
        {
            try
            {
                if (hfOrigineChiamata.Value.ToString() == "RicercaDich" || hfOrigineChiamata.Value.ToString() == "Dettaglio")
                {
                    hfOrigineChiamata.Value = string.Empty;
                    HideDiv("ComandiDettaglio");
                    HideDiv("RicercaDich");
                    HideDiv("DettaglioDOCFA");
                    ShowDiv("RicercaDOCFA"); ShowDiv("ComandiRicerca");
                }
                else if (hfOrigineChiamata.Value.ToString() == "Dich")
                {
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "parent('nascosto').location.href='../../aspVuota.aspx';";
                    sScript += "parent('comandi').location.href='../../" + SiteICI + "/CImmobileDettaglioMod.aspx?Operation=DETTAGLIO';";
                    sScript += "location.href ='../../" + SiteICI + "/ImmobileDettaglio.aspx?IDTestata=" + Request.QueryString["IdTestata"].ToString() + "&IDImmobile=" + Request.QueryString["IdImmobile"].ToString() + "&IdAttoCompraVendita=0&TYPEOPERATION=DETTAGLIO';";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "loadnewpage", sBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.DOCFAGestione.CmdBackClick.errore::", ex); ;
                throw;
            }
        }
        #endregion

        #region Griglia Ricerca DOCFA
        protected void rgvDOCFAPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// Funzione di gestione eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void rgvDOCFARowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sScript = string.Empty;
            int idDOCFA;
            int.TryParse(e.CommandArgument.ToString(), out idDOCFA);
            hfIdDOCFA.Value = idDOCFA.ToString();
            switch (e.CommandName)
            {
                case "EditDOCFA":
                    hfOrigineChiamata.Value = "RicercaDich";
                    HideDiv("ComandiDettaglio");
                    HideDiv("RicercaDOCFA");
                    ShowDiv("RicercaDich"); ShowDiv("ComandiRicerca");
                    sScript += "$('#DettaglioDOCFA').removeClass();$('#DettaglioDOCFA').addClass('active');";
                    RegisterScript(sScript);
                    LoadFilesDOCFA(idDOCFA, false);
                    break;
                case "ViewDOCFA":
                    hfOrigineChiamata.Value = "Dettaglio";
                    LoadFilesDOCFA(idDOCFA, true);
                    sScript += "$('#DettaglioDOCFA').removeClass();$('#DettaglioDOCFA').addClass('active');";
                    RegisterScript(sScript);
                    break;
                default:
                    break;
            }
        }
        //protected void rgvDOCFARowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int idDOCFA;
        //    int.TryParse(e.CommandArgument.ToString(), out idDOCFA);
        //    hfIdDOCFA.Value = idDOCFA.ToString();
        //    switch (e.CommandName)
        //    {
        //        case "EditDOCFA":
        //            hfOrigineChiamata.Value = "RicercaDich";
        //            HideDiv("ComandiDettaglio");
        //            HideDiv("RicercaDOCFA");
        //            ShowDiv("RicercaDich"); ShowDiv("ComandiRicerca");
        //            ShowDiv("DettaglioDOCFA");
        //            LoadFilesDOCFA(idDOCFA, false);
        //            break;
        //        case "ViewDOCFA":
        //            hfOrigineChiamata.Value = "Dettaglio";
        //            ShowDiv("ComandiDettaglio");
        //            HideDiv("RicercaDOCFA");
        //            HideDiv("RicercaDich");
        //            ShowDiv("DettaglioDOCFA"); HideDiv("ComandiRicerca");
        //            LoadFilesDOCFA(idDOCFA,true);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region Griglia Ricerca Dichiarazioni
        protected void rgvDichPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// Funzione di gestione eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void rgvDichRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string tributo = string.Empty;
            int idDOCFA;
            int.TryParse(e.CommandArgument.ToString(), out idDOCFA);
            DOCFADichiarazioni DOCFADich = new DOCFADichiarazioni { FKIdDOCFA = int.Parse(hfIdDOCFA.Value.ToString()), FKIdDich = idDOCFA, CodTributo = tributo };
            switch (e.CommandName)
            {
                case "EditDich":
                    if (rbICI.Checked == true)
                        tributo = "8852";
                    if (rbTARSU.Checked == true)
                        tributo = "0434";
                    if (!DOCFADich.Save()) return;
                    break;
                case "EditDichAll":
                    if (rbICI.Checked == true)
                        tributo = "8852";
                    if (rbTARSU.Checked == true)
                        tributo = "0434";
                    foreach (GridViewRow myRow in rgvDich.Rows)
                    {
                        DOCFADich = new DOCFADichiarazioni { FKIdDOCFA = int.Parse(hfIdDOCFA.Value.ToString()), FKIdDich = int.Parse(((HiddenField)myRow.FindControl("hfFKIdDich")).Value), CodTributo = tributo };
                        if (!DOCFADich.Save()) return;
                    }
                    break;
                default:
                    break;
            }
            hfOrigineChiamata.Value = "";
            LoadSearch();
        }
        //protected void rgvDichRowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string tributo = string.Empty;
        //    int idDOCFA;
        //    int.TryParse(e.CommandArgument.ToString(), out idDOCFA);
        //    switch (e.CommandName)
        //    {
        //        case "EditDich":
        //            if (rbICI.Checked == true)
        //                tributo = "8852";
        //            if (rbTARSU.Checked == true)
        //                tributo = "0434";
        //            DOCFADichiarazioni DOCFADich = new DOCFADichiarazioni { FKIdDOCFA = int.Parse(hfIdDOCFA.Value.ToString()), FKIdDich = idDOCFA, CodTributo = tributo };
        //            if (!DOCFADich.Save()) return;
        //            hfOrigineChiamata.Value = "";
        //            LoadSearch();
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region void/function
        private void LoadFilesDOCFA(int idDOCFA, bool HasListFile)
        {
            try
            {
                DOCFADocumento DOCFA = new DOCFADocumento { IdDOCFA = idDOCFA };
                if (!DOCFA.Load()) return;
                lblDocumento.Text = DOCFA.DatiDocumento;
                lblRifCat.Text = DOCFA.RifCatastali;
                lblIndirizzo.Text = DOCFA.Indirizzo;
                lblClassamento.Text = DOCFA.Classamento;
                if (HasListFile == true)
                {
                    dlFiles.Visible = true;
                    dlFiles.DataSource = DOCFA.Files;
                    dlFiles.DataBind();
                }
                else
                    dlFiles.Visible = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.DOCFAGestione.LoadFilesDOCFA.errore::", ex); ;
                throw;
            }
        }

        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// Modifiche da revisione manuale
    /// </revision>
    /// </revisionHistory>
        private void LoadSearch(int? page = 0)
        {
            string sScript = string.Empty;
            string tributo = string.Empty;
            try
            {
                if (hfOrigineChiamata.Value.ToString() == string.Empty)
                {
                    rgvDOCFA.DataSource = new DOCFA().LoadNonAbbinati(Ente, txtProtocollo.Text, txtDataRegistrazione.Text, txtFoglioDOCFA.Text, txtNumeroDOCFA.Text, txtSubDOCFA.Text, txtUbicazioneDOCFA.Text);
                    if (page.HasValue)
                        rgvDOCFA.PageIndex = page.Value;
                    rgvDOCFA.DataBind();
                    if (rgvDOCFA.Rows.Count > 0)
                        lblResultDOCFA.Visible = false;
                    HideDiv("ComandiDettaglio");
                    HideDiv("RicercaDich");
                    HideDiv("DettaglioDOCFA");
                    ShowDiv("RicercaDOCFA"); ShowDiv("ComandiRicerca");
                }
                else
                {
                    if (rbICI.Checked == true)
                        tributo = "8852";
                    if (rbTARSU.Checked == true)
                        tributo = "0434";
                    rgvDich.DataSource = new DOCFADichiarazioni().LoadDichiarazioni(Ente, hfIdDOCFA.Value.ToString(), tributo, txtFoglio.Text, txtNumero.Text, txtSub.Text, txtUbicazione.Text);
                    if (page.HasValue)
                        rgvDich.PageIndex = page.Value;
                    rgvDich.DataBind();
                    if (rgvDich.Rows.Count > 0)
                        lblResultDich.Visible = false;
                    HideDiv("ComandiDettaglio");
                    HideDiv("RicercaDOCFA");
                    ShowDiv("RicercaDich"); ShowDiv("ComandiRicerca");
                    sScript += "$('#DettaglioDOCFA').removeClass();$('#DettaglioDOCFA').addClass('active');";
                    RegisterScript(sScript);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.DOCFAGestione.LoadSearch.errore::", ex); ;
                throw;
            }
        }
        //private void LoadSearch(int? page = 0)
        //{
        //    string tributo = string.Empty;
        //    try
        //    {
        //        if (hfOrigineChiamata.Value.ToString() == string.Empty)
        //        {
        //            rgvDOCFA.DataSource = new DOCFA().LoadNonAbbinati(Ente, txtProtocollo.Text, txtDataRegistrazione.Text, txtFoglioDOCFA.Text, txtNumeroDOCFA.Text, txtSubDOCFA.Text, txtUbicazioneDOCFA.Text);
        //            if (page.HasValue)
        //                rgvDOCFA.PageIndex = page.Value;
        //            rgvDOCFA.DataBind();
        //            if (rgvDOCFA.Rows.Count > 0)
        //                lblResultDOCFA.Visible = false;
        //            HideDiv("ComandiDettaglio");
        //            HideDiv("RicercaDich");
        //            HideDiv("DettaglioDOCFA");
        //            ShowDiv("RicercaDOCFA"); ShowDiv("ComandiRicerca");
        //        }
        //        else
        //        {
        //            if (rbICI.Checked == true)
        //                tributo = "8852";
        //            if (rbTARSU.Checked == true)
        //                tributo = "0434";
        //            rgvDich.DataSource = new DOCFADichiarazioni().LoadDichiarazioni(Ente, hfIdDOCFA.Value.ToString(), tributo, txtFoglio.Text, txtNumero.Text, txtSub.Text, txtUbicazione.Text);
        //            if (page.HasValue)
        //                rgvDich.PageIndex = page.Value;
        //            rgvDich.DataBind();
        //            if (rgvDich.Rows.Count > 0)
        //                lblResultDich.Visible = false;
        //            HideDiv("ComandiDettaglio");
        //            HideDiv("RicercaDOCFA");
        //            ShowDiv("RicercaDich"); ShowDiv("ComandiRicerca");
        //            ShowDiv("DettaglioDOCFA");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("OPENgov.20.DOCFAGestione.LoadSearch.errore::", ex); ;
        //        throw;
        //    }
        //}

        protected string Path_IMAGE(object objFileName)
        {
            string PathImages = "";
            if (ConfigurationManager.AppSettings["PathImages"] != null)
            {
                PathImages = ConfigurationManager.AppSettings["PathImages"].ToString();
                if (!PathImages.EndsWith("/"))
                    PathImages += "/";
                PathImages += "DOCFA/" + Ente + "/";
            }
            else
            {
                PathImages = "http://serviziostampe.isimply.it/RibesServizioStampe/Documenti/";
                if (!PathImages.EndsWith("/"))
                    PathImages += "/";
                PathImages += "DOCFA/" + Ente + "/";
            }
            if (objFileName != null)
                if (objFileName.ToString().EndsWith("pdf"))
                    return PathImages + "pdf.png";
                else
                    return PathImages + objFileName + ".tiff";
            else
                return "";
        }

        protected string Path_DOC(object objFileName)
        {
            string PathImages = "";
            if (ConfigurationManager.AppSettings["PathImages"] != null)
            {
                PathImages = ConfigurationManager.AppSettings["PathImages"].ToString();
                if (!PathImages.EndsWith("/"))
                    PathImages += "/";
                PathImages += "DOCFA/" + Ente + "/";
            }
            else
            {
                PathImages = "http://serviziostampe.isimply.it/RibesServizioStampe/Documenti/";
                if (!PathImages.EndsWith("/"))
                    PathImages += "/";
                PathImages += "DOCFA/" + Ente + "/";
            }
            if (objFileName != null)
                if (!objFileName.ToString().EndsWith("pdf"))
                    return PathImages + objFileName + ".tiff";
                else
                    return PathImages + objFileName;
            else
                return "";
        }
        #endregion
    }
}