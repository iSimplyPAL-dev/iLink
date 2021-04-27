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
using System.Threading;
using DTO;
using DAO;
using IRemInterfaceOSAP;
using log4net;
using System.Data.SqlClient;

namespace OPENgovTOCO.Elaborazioni
{
    /// <summary>
    /// Pagina per la generazione del ruolo ordinario. La generazione di un ruolo passa attraverso un iter procedurale composto dai seguenti passaggi:
    /// - Creazione
    /// - Stampa Minuta
    /// - Approva Minuta
    /// - Configura Rate
    /// - Calcolo Rate
    /// - Elabora Documenti
    /// - Approva Documenti
    /// Per il tributo SCUOLE invece della creazione è disponibile l'importazione
    /// Ad ogni ruolo è possibile associare un template di stampa specifico; la gestione del template personalizzato è resa possibile dai pulsanti "Upload Template" e "Download Template.
    /// </summary>
    public partial class ElaborazioneAvvisi : BaseEnte
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ElaborazioneAvvisi));
        private static int nCallForm;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                Session["ListaCartelle"] = null;
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                {
                    info.InnerText = "SCUOLA";
                    btnImport.Style.Add("display", "");
                    btnCalcola.Style.Add("display", "none");
                    string sScript = "";
                    sScript += "document.getElementById('DownloadTemplate').style.display='none';";
                    sScript += "document.getElementById('UploadTemplate').style.display='none';";
                    sScript += "document.getElementById('trOccupToElab').style.display='none';";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    info.InnerText = "TOSAP/COSAP";
                    btnImport.Style.Add("display", "none");
                    btnCalcola.Style.Add("display", "");
                    RegisterScript("document.getElementById('trImport').style.display='none';", this.GetType());
                }
                info.InnerText += " - Elaborazione - Avvisi";
                if (!Page.IsPostBack)
                {
                    nCallForm = 0;
                    ControlsDataBind();

                    int AnnoCalcoloInCorso = CacheManager.GetCalcoloMassivoInCorso();

                    if (CacheManager.GetCalcoloMassivoInCorso() != -1 || CacheManager.GetImportInCorso() != "")
                    {
                        ManageButtons(null, null, false);
                        lblElaborazioneAnno.Text = AnnoCalcoloInCorso.ToString();

                        ShowCalcoloInCorso();
                    }
                    else if (Session["OSAPAnnoCalcoloInCorso"] != null)
                    {
                        int anno = 0;
                        int.TryParse((string)Session["OSAPAnnoCalcoloInCorso"], out anno);
                        ddlAnno.SelectedValue = anno.ToString();
                        if (Session["OSAPTipoRuoloCalcoloInCorso"].ToString() == "1")
                            OptSuppletivo.Checked = true;
                        else
                            OptOrdinario.Checked = true;
                        ddlAnno_SelectedIndexChanged(null, null);
                    }
                    else
                        ManageButtons(null, null, true);
                    new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, DichiarazioneSession.sOperatore, new Utility.Costanti.LogEventArgument().Elaborazioni, "Calcolo", Utility.Costanti.AZIONE_LETTURA.ToString(), DichiarazioneSession.CodTributo(""), DichiarazioneSession.IdEnte, int.Parse(hfIdElab.Value));
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.Page_Load.errore: ", Err);
                //Response.Redirect("../../PaginaErrore.aspx");
                RegisterScript("GestAlert('a', 'danger', '', '', 'Si è verificato un errore!');", this.GetType());
            }
        }

        #region Events
        protected void ddlAnno_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                //*** 20130610 - ruolo supplettivo ***
                Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
                if (OptOrdinario.Checked == true || OptSuppletivo.Checked == true)
                {
                    if (OptSuppletivo.Checked == true)
                        TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                    int AnnoSel = 0;
                    int.TryParse(ddlAnno.SelectedValue, out AnnoSel);
                    if (AnnoSel > 0)//if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        if (DichiarazioneSession.CodTributo("").ToString() != Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                        {
                            TipologieOccupazioni[] ListOccup = MetodiTipologieOccupazioni.GetTipologieOccupazioniForMotore(DichiarazioneSession.StringConnection, int.Parse(ddlAnno.SelectedValue), DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                            if (ListOccup != null)
                            {
                                GrdOccupaz.DataSource = ListOccup;
                                GrdOccupaz.DataBind();
                            }
                        }
                        else { }
                        ElaborazioneEffettuata preStatistiche = MetodiElaborazioneEffettuata.GetStatistichePreElaborazione(int.Parse(ddlAnno.SelectedValue), TipoRuolo, DichiarazioneSession.CodTributo(""));
                        ElaborazioneEffettuata postStatistiche = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(ddlAnno.SelectedValue), TipoRuolo, DichiarazioneSession.CodTributo(""));

                        lblPreNUtenti.Text = preStatistiche.NUtenti.ToString();
                        lblPreNDichiarazioni.Text = preStatistiche.NDichiarazioni.ToString();
                        lblPreNArticoli.Text = preStatistiche.NArticoli.ToString();

                        if (postStatistiche != null)
                        {
                            lblPostNUtenti.Text = postStatistiche.NUtenti.ToString();
                            lblPostNDichiarazioni.Text = postStatistiche.NDichiarazioni.ToString();
                            lblPostNArticoli.Text = postStatistiche.NArticoli.ToString();
                            lblPostImportoTotale.Text = string.Format("€ {0:0.00}", postStatistiche.ImportoTotale);
                            lblPostNote.Text = postStatistiche.Note;
                            // Esiste già un calcolo effettuato
                            hfIdElab.Value = postStatistiche.IdFlusso.ToString();
                        }
                        else
                        {
                            lblPostNUtenti.Text = string.Empty;
                            lblPostNDichiarazioni.Text = string.Empty;
                            lblPostNArticoli.Text = string.Empty;
                            lblPostImportoTotale.Text = string.Empty;
                            lblPostNote.Text = string.Empty;
                        }
                        ManageButtons(preStatistiche, postStatistiche, true);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.ddlAnno_SelectedIndexChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            //*** ***
        }
        protected void btnImport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                string sFileImport = "";
                sFileImport = fileUpload.PostedFile.FileName;
                if (sFileImport == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script language='javascript'>alert('Selezionare il file!');</script>");
                }
                else
                {
                    ShowCalcoloInCorso();
                    sFileImport = DichiarazioneSession.PathImport + sFileImport;
                    fileUpload.PostedFile.SaveAs(sFileImport);
                    clsImportFlussi FncImport = new clsImportFlussi();
                    FncImport.StartImport(DichiarazioneSession.DBType, DichiarazioneSession.StringConnection, DichiarazioneSession.StringConnectionAnagrafica, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), sFileImport, "UMAVC", "SCUOLA", DichiarazioneSession.sOperatore);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnlImport_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                string sScript = "GestAlert('a', 'danger', '', '', ''" + ex.Message + "');";
                RegisterScript(sScript, this.GetType());
            }
        }
        /// <summary>
        /// Pulsante per il calcolo asincrono del ruolo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcola_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string msg = string.Empty;
            Categorie[] categ = null;
            TipologieOccupazioni[] tipiOcc = null;
            Agevolazione[] agevolazioni = null;
            Tariffe[] tarif = null;
            int Anno = -1;
            double ImportoRate = 0.0;
            //*** 20130610 - ruolo supplettivo ***
            Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;

            try
            {
                if (nCallForm == 0)
                {
                    nCallForm = 1;
                    if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                    {
                        msg = "Selezionare il tipo di ruolo da elaborare.";
                        string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
                        RegisterScript(sScript, this.GetType());
                    }
                    else
                    {
                        if (OptSuppletivo.Checked == true)
                            TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                        //*** ***

                        if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                        {
                            Anno = int.Parse(ddlAnno.SelectedValue);
                            categ = MetodiCategorie.GetCategorieForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));

                            tipiOcc = MetodiTipologieOccupazioni.GetTipologieOccupazioniForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                            agevolazioni = MetodiAgevolazione.GetAgevolazioniForMotore(DichiarazioneSession.StringConnection, Anno.ToString(), DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                            tarif = MetodiTariffe.GetTariffeForMotore(Anno, DichiarazioneSession.CodTributo(""));

                            if (categ == null || categ.Length == 0)
                                msg = "Non è presente nessuna categoria per l'anno selezionato, " +
                                    "impossibile procedere al calcolo";
                            else if (tipiOcc == null || tipiOcc.Length == 0)
                                msg = "Non è presente nessuna tipologia di occupazione per l'anno selezionato, " +
                                    "impossibile procedere al calcolo";
                            else if (tarif == null || tarif.Length == 0)
                                msg = "Non è presente nessuna tariffa per l'anno selezionato, " +
                                    "impossibile procedere al calcolo";
                            else if (!double.TryParse(txtSogliaMinima.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out ImportoRate))
                                msg = "Inserire un importo valido per l'importo minimo di rateizzazione";
                        }
                        else
                            msg = "Anno selezionato per il calcolo non valido!";

                        if (msg.CompareTo(string.Empty) == 0)
                        {
                            //controllo se mancano delle tariffe
                            string sTariffaMancante = MetodiElaborazioneEffettuata.CheckTariffe(Anno, DichiarazioneSession.CodTributo(""));
                            if (sTariffaMancante != string.Empty)
                            {
                                //string sScript = "GestAlert('a', 'warning', '', '', '" + sTariffaMancante + "');";
                                //RegisterScript(sScript, this.GetType());
                                lblPostNote.Text = sTariffaMancante;
                            }
                            else
                            {
                                Log.Debug("ElaborazioneAvvisi.btnCalcola_Click.OccupazioniToElab=" + hfOccupazioniToElab.Value);
                                // Posso fare partire il calcolo
                                lblElaborazioneAnno.Text = Anno.ToString();
                                CacheManager.SetAvanzamentoElaborazione("");
                                ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
                                ShowCalcoloInCorso();

                                CalcoloOSAP calcolo = new CalcoloOSAP();
                                if (myRuolo != null)
                                {
                                    Log.Debug("ElaborazioneAvvisi::btnCalcola_Click::idruolo::" + myRuolo.IdFlusso.ToString());
                                    calcolo.StartCalcolo(TipoRuolo, Anno, categ, tipiOcc, agevolazioni, tarif, DichiarazioneSession.IdEnte, DichiarazioneSession.sOperatore, DichiarazioneSession.CodTributo(""), ImportoRate, myRuolo.IdFlusso, hfOccupazioniToElab.Value);
                                }
                                else
                                {
                                    Log.Debug("ElaborazioneAvvisi::btnCalcola_Click::idruolo::vuoto=0");
                                    calcolo.StartCalcolo(TipoRuolo, Anno, categ, tipiOcc, agevolazioni, tarif, DichiarazioneSession.IdEnte, DichiarazioneSession.sOperatore, DichiarazioneSession.CodTributo(""), ImportoRate, -1, hfOccupazioniToElab.Value);
                                }
                            }
                        }
                        else
                        {
                            string sScript = "GestAlert('a', 'danger', '', '', '" + msg + "');";
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnCalcola_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");

                string sScript = "GestAlert('a', 'danger', '', '', '" + ex.Message + "');";
                RegisterScript(sScript, this.GetType());
            }
        }
        //protected void btnCalcola_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    string msg = string.Empty;
        //    Categorie[] categ = null;
        //    TipologieOccupazioni[] tipiOcc = null;
        //    Agevolazione[] agevolazioni = null;
        //    Tariffe[] tarif = null;
        //    int Anno = -1;
        //    double ImportoRate = 0.0;
        //    //*** 20130610 - ruolo supplettivo ***
        //    Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
        //    //string OccupazioniToElab = "";

        //    try
        //    {
        //        if (nCallForm == 0)
        //        {
        //            nCallForm = 1;
        //            if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
        //            {
        //                msg = "Selezionare il tipo di ruolo da elaborare.";
        //                string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
        //                RegisterScript(sScript, this.GetType());
        //            }
        //            else
        //            {
        //                if (OptSuppletivo.Checked == true)
        //                    TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
        //                //*** ***

        //                if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
        //                {
        //                    Anno = int.Parse(ddlAnno.SelectedValue);
        //                    categ = MetodiCategorie.GetCategorieForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));

        //                    tipiOcc = MetodiTipologieOccupazioni.GetTipologieOccupazioniForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
        //                    agevolazioni = MetodiAgevolazione.GetAgevolazioniForMotore(DichiarazioneSession.StringConnection, Anno.ToString(), DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
        //                    tarif = MetodiTariffe.GetTariffeForMotore(Anno, DichiarazioneSession.CodTributo(""));

        //                    if (categ == null || categ.Length == 0)
        //                        msg = "Non è presente nessuna categoria per l'anno selezionato, " +
        //                            "impossibile procedere al calcolo";
        //                    else if (tipiOcc == null || tipiOcc.Length == 0)
        //                        msg = "Non è presente nessuna tipologia di occupazione per l'anno selezionato, " +
        //                            "impossibile procedere al calcolo";
        //                    else if (tarif == null || tarif.Length == 0)
        //                        msg = "Non è presente nessuna tariffa per l'anno selezionato, " +
        //                            "impossibile procedere al calcolo";
        //                    else if (!double.TryParse(txtSogliaMinima.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out ImportoRate))
        //                        msg = "Inserire un importo valido per l'importo minimo di rateizzazione";
        //                }
        //                else
        //                    msg = "Anno selezionato per il calcolo non valido!";

        //                if (msg.CompareTo(string.Empty) == 0)
        //                {
        //                    //controllo se mancano delle tariffe
        //                    string sTariffaMancante = MetodiElaborazioneEffettuata.CheckTariffe(Anno, DichiarazioneSession.CodTributo(""));
        //                    if (sTariffaMancante != string.Empty)
        //                    {
        //                        string sScript = "GestAlert('a', 'warning', '', '', '" + sTariffaMancante + "');";
        //                        RegisterScript(sScript, this.GetType());
        //                    }
        //                    else
        //                    {
        //                        /*//valorizzo la variabile delle occupazioni da calcolare
        //                        foreach (GridViewRow MyItemGrd in GrdOccupaz.Rows)
        //                        {
        //                            if (((CheckBox)(MyItemGrd.FindControl("ChkSelezionato"))).Checked == true)
        //                            {
        //                                if (OccupazioniToElab != "")
        //                                    OccupazioniToElab += ",";
        //                                OccupazioniToElab += MyItemGrd.Cells[0].Text;
        //                            }
        //                        }*/
        //                        Log.Debug("ElaborazioneAvvisi.btnCalcola_Click.OccupazioniToElab=" + hfOccupazioniToElab.Value);
        //                        // Posso fare partire il calcolo
        //                        lblElaborazioneAnno.Text = Anno.ToString();
        //                        CacheManager.SetAvanzamentoElaborazione("");
        //                        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
        //                        ShowCalcoloInCorso();

        //                        CalcoloOSAP calcolo = new CalcoloOSAP();
        //                        if (myRuolo != null)
        //                        {
        //                            Log.Debug("ElaborazioneAvvisi::btnCalcola_Click::idruolo::" + myRuolo.IdFlusso.ToString());
        //                            calcolo.StartCalcolo(TipoRuolo, Anno, categ, tipiOcc, agevolazioni, tarif, DichiarazioneSession.IdEnte, DichiarazioneSession.sOperatore, DichiarazioneSession.CodTributo(""), ImportoRate, myRuolo.IdFlusso, hfOccupazioniToElab.Value);
        //                        }
        //                        else
        //                        {
        //                            Log.Debug("ElaborazioneAvvisi::btnCalcola_Click::idruolo::vuoto=0");
        //                            calcolo.StartCalcolo(TipoRuolo, Anno, categ, tipiOcc, agevolazioni, tarif, DichiarazioneSession.IdEnte, DichiarazioneSession.sOperatore, DichiarazioneSession.CodTributo(""), ImportoRate, -1, hfOccupazioniToElab.Value);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    string sScript = "GestAlert('a', 'danger', '', '', '" + msg + "');";
        //                    RegisterScript(sScript, this.GetType());
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnCalcola_Click.errore: ", ex);
        //        Response.Redirect("../../PaginaErrore.aspx");

        //        string sScript = "GestAlert('a', 'danger', '', '', '" + ex.Message + "');";
        //        RegisterScript(sScript, this.GetType());
        //    }
        //}
        /// <summary>
        /// Pulsante per la stampa minuta del ruolo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="10/06/2013">
        /// <strong>ruolo supplettivo</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnStampaMinuta_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
            DataTable dtMinuta = null;
            string NameXLS = "";
            int[] arrColumnsToExport = null;
            string[] arrHeaders = null;
            try
            {
                if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                {
                    string msg = "Selezionare il tipo di ruolo da elaborare.";
                    string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    if (OptSuppletivo.Checked == true)
                        TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                    if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        int Anno = int.Parse(ddlAnno.SelectedValue);
                        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));

                        NameXLS = DichiarazioneSession.IdEnte + "_MINUTA_" + myRuolo.IdTributo + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                        dtMinuta = MetodiMinuta.GetMinuta(DichiarazioneSession.StringConnection, DichiarazioneSession.IdEnte, DichiarazioneSession.DescrizioneEnte, myRuolo.IdFlusso);

                        arrHeaders = new string[dtMinuta.Columns.Count];
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
                        myRuolo.DataOraMinutaStampata = DateTime.Now;
                        SqlCommand cmdMyCommand = null;
                        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand, DichiarazioneSession.sOperatore);

                        ManageButtons(myRuolo, myRuolo, true);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnStampaMinuta_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            if (dtMinuta != null)
            {
                RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
                objStampa.ExportDetails(dtMinuta, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }

            //*** ***
        }
        //protected void btnStampaMinuta_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    //*** 20130610 - ruolo supplettivo ***
        //    Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
        //    DataTable dtMinuta=null;
        //    string NameXLS="";
        //    int[] arrColumnsToExport=null;
        //    string[] arrHeaders = null;
        //    try {
        //        if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
        //        {
        //            string msg = "Selezionare il tipo di ruolo da elaborare.";
        //            string sScript = "GestAlert('a', 'warning', '', '', '"+msg+"');";
        //            RegisterScript(sScript,this.GetType());
        //        }
        //        else
        //        {
        //            if (OptSuppletivo.Checked == true)
        //                TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
        //            if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
        //            {
        //                int Anno = int.Parse(ddlAnno.SelectedValue);
        //                ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));

        //                NameXLS = DichiarazioneSession.IdEnte + "_MINUTA_" + myRuolo.IdTributo + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

        //                dtMinuta = MetodiMinuta.GetMinuta(myRuolo.IdFlusso);

        //                 arrHeaders = new string[dtMinuta.Columns.Count];
        //                checked
        //                {
        //                    for (int i = 0; i < arrHeaders.Length; i++)
        //                        arrHeaders[i] = string.Empty;
        //                }
        //                //int[] arrColumnsToExport = MetodiMinuta.GetColumnsToExport();
        //                arrColumnsToExport = new int[arrHeaders.Length];
        //                checked
        //                {
        //                    for (int i = 0; i < arrHeaders.Length; i++)
        //                        arrColumnsToExport[i] = i;
        //                }
        //                myRuolo.DataOraMinutaStampata = DateTime.Now;
        //                /*
        //                DAL.DBEngine dbEngine = null;
        //                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata (myRuolo, ref dbEngine);
        //                */
        //                //*** 20140410
        //                SqlCommand cmdMyCommand = null;
        //                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand);

        //                ManageButtons(myRuolo, myRuolo, true);
        //            }
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnStampaMinuta_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //    if (dtMinuta != null)
        //    {
        //        RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
        //        objStampa.ExportDetails(dtMinuta, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        //    }

        //    //*** ***
        //}
        /// <summary>
        /// Pulsante per l'approvazione minuta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="10/06/2013">
        /// <strong>ruolo supplettivo</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnApprovaMinuta_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
            try
            {
                if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                {
                    string msg = "Selezionare il tipo di ruolo da elaborare.";
                    string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    if (OptSuppletivo.Checked == true)
                        TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                    if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        int Anno = int.Parse(ddlAnno.SelectedValue);
                        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));

                        myRuolo.DataOraMinutaApprovata = DateTime.Now;
                        SqlCommand cmdMyCommand = null;
                        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand, DichiarazioneSession.sOperatore);

                        string sScript = "GestAlert('a', 'success', '', '', 'Minuta anno " + Anno + " approvata correttamente');";
                        RegisterScript(sScript, this.GetType());

                        ManageButtons(myRuolo, myRuolo, true);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnApprovaMinuta_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void btnApprovaMinuta_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    //*** 20130610 - ruolo supplettivo ***
        //    Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
        //    try {
        //        if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
        //        {
        //            string msg = "Selezionare il tipo di ruolo da elaborare.";
        //            string sScript = "GestAlert('a', 'warning', '', '', '"+msg+"');";
        //            RegisterScript(sScript,this.GetType());
        //        }
        //        else
        //        {
        //            if (OptSuppletivo.Checked == true)
        //                TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
        //            if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
        //            {
        //                int Anno = int.Parse(ddlAnno.SelectedValue);
        //                ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));

        //                myRuolo.DataOraMinutaApprovata = DateTime.Now;
        //                /*
        //                DAL.DBEngine dbEngine = null;
        //                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata (statistiche, ref dbEngine);
        //                */
        //                //*** 20140410
        //                SqlCommand cmdMyCommand = null;
        //                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand);

        //                string sScript = "GestAlert('a', 'success', '', '', 'Minuta anno " + Anno + " approvata correttamente');";
        //                RegisterScript(sScript,this.GetType());

        //                ManageButtons(myRuolo, myRuolo, true);
        //            }
        //        }
        //        //*** ***
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnApprovaMinuta_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //}

        protected void btnCalcolaRate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //*** 20130610 - ruolo supplettivo ***
            Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
            try
            {
                if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                {
                    string msg = "Selezionare il tipo di ruolo da elaborare.";
                    string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    if (OptSuppletivo.Checked == true)
                        TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                    if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        int Anno = int.Parse(ddlAnno.SelectedValue);
                        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
                        //*** 20130610 - ruolo supplettivo ***
                        //Rate[] config = MetodiConfigurazioneRate.GetConfigurazioneRate (Anno);
                        Rate[] config = MetodiConfigurazioneRate.GetConfigurazioneRate(myRuolo.IdFlusso);
                        if (config.Length == 0)
                        {
                            string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile effettuare il calcolo rate per l\'anno " + Anno + "! Manca la configurazione delle rate.');";
                            RegisterScript(sScript, this.GetType());
                        }
                        else
                        {
                            string userName = HttpContext.Current.Session["username"].ToString();
                            OPENUtility.ClsContiCorrenti oClsContiCorrente = new OPENUtility.ClsContiCorrenti();
                            OPENUtility.objContoCorrente contoCorrente = oClsContiCorrente.GetContoCorrente(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), userName, DichiarazioneSession.StringConnectionOPENgov);
                            if (contoCorrente == null)
                            {
                                string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile effettuare il calcolo rate per l\'anno " + Anno + "! Manca il Conto Corrente.');";
                                RegisterScript(sScript, this.GetType());
                            }
                            else
                            {
                                lblElaborazioneAnno.Text = Anno.ToString();
                                CacheManager.SetAvanzamentoElaborazione("");
                                ShowCalcoloInCorso();

                                CalcoloRateOSAP calcolo = new CalcoloRateOSAP();
                                calcolo.StartCalcolo(Anno, DichiarazioneSession.IdEnte, myRuolo, config, userName, contoCorrente, DichiarazioneSession.CodTributo(""));
                            }
                        }
                        ManageButtons(myRuolo, myRuolo, true);
                    }
                }
                //*** ***
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnCalcolaRate_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        protected void btnStampaDocumenti_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //*** 20130610 - ruolo supplettivo ***
            try
            {
                if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                {
                    string msg = "Selezionare il tipo di ruolo da elaborare.";
                    string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    int nTipoRuolo = 0;
                    if (OptSuppletivo.Checked == true)
                        nTipoRuolo = 1;
                    if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        int Anno = int.Parse(ddlAnno.SelectedValue);
                        string sScript;
                        sScript = "parent.Visualizza.location.href='Documenti/RicercaDoc.aspx?Anno=" + Anno.ToString() + "&TipoRuolo=" + nTipoRuolo.ToString() + "';";
                        RegisterScript(sScript, this.GetType());
                    }
                }
                //*** ***
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnStampaDocumenti_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        protected void btnEliminaRuolo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //*** 20130610 - ruolo supplettivo ***
            try
            {
                Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
                if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                {
                    string msg = "Selezionare il tipo di ruolo da elaborare.";
                    string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    if (OptSuppletivo.Checked == true)
                        TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                    if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        if (OptSuppletivo.Checked == true)
                            TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;

                        int Anno = int.Parse(ddlAnno.SelectedValue);
                        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));

                        //dao.DeleteCalcoloMassivo (Anno, DichiarazioneSession.IdEnte,TipoRuolo);
                        if (myRuolo != null)
                            new CartelleDAO().DeleteCalcoloMassivo(myRuolo.IdFlusso);
                        //*** ***

                        string sScript = "GestAlert('a', 'success', '', '', 'Dati elaborazione anno " + Anno + " eliminati correttamente');";
                        RegisterScript(sScript, this.GetType());

                        // Ripopola le tabelle di statistiche
                        ddlAnno_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnEliminaRuolo_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** 201511 - template documenti per ruolo ***
        protected void BtnUploadClick(object sender, EventArgs e)
        {
            try//carico template nella tabella dei ruoli elaborati
            {
                Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
                int Anno = 0;
                if (OptSuppletivo.Checked == true)
                    TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    Anno = int.Parse(ddlAnno.SelectedValue);
                ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
                if (myRuolo.IdFlusso <= 0)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    HttpFileCollection ListFiles = Request.Files;
                    if (ListFiles != null)
                    {
                        foreach (string myFile in ListFiles)
                        {
                            try
                            {
                                HttpPostedFile PostedFile = Request.Files[myFile] as HttpPostedFile;
                                if (PostedFile.ContentLength > 0)
                                {
                                    ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
                                    myTemplateDoc.myStringConnection = DichiarazioneSession.StringConnectionOPENgov;
                                    myTemplateDoc.IdEnte = DichiarazioneSession.IdEnte;
                                    myTemplateDoc.IdTributo = DichiarazioneSession.CodTributo("");
                                    myTemplateDoc.IdRuolo = myRuolo.IdFlusso;
                                    myTemplateDoc.FileMIMEType = fileUploadDOT.PostedFile.ContentType;
                                    myTemplateDoc.PostedFile = fileUploadDOT.FileBytes;
                                    myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUploadDOT.PostedFile.FileName);
                                    myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save();
                                    if (myTemplateDoc.IdTemplateDoc <= 0)
                                        lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
                                    else
                                    {
                                        lblMessage.Text = "File caricato con successo.";
                                        lblMessage.CssClass = "Input_Label_bold";
                                    }

                                    lblMessage.Visible = true;
                                    fileUploadDOT = new FileUpload();
                                }
                            }
                            catch (Exception Err)
                            {
                                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnUploadClick.PostedFile.errore: ", Err);
                                Response.Redirect("../../PaginaErrore.aspx");
                            }
                            finally
                            {
                                divElaborazioneInCorso.Style.Add("display", "none");
                            }
                        }
                    }


                    if (System.IO.Path.GetFileName(fileUpload.PostedFile.FileName) == "")
                    {
                        RegisterScript("GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un file!');", this.GetType());
                    }
                    else
                    {
                        ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
                        myTemplateDoc.myStringConnection = DichiarazioneSession.StringConnectionOPENgov;
                        myTemplateDoc.IdEnte = DichiarazioneSession.IdEnte;
                        myTemplateDoc.IdTributo = DichiarazioneSession.CodTributo("");
                        myTemplateDoc.IdRuolo = myRuolo.IdFlusso;
                        myTemplateDoc.FileMIMEType = fileUploadDOT.PostedFile.ContentType;
                        myTemplateDoc.PostedFile = fileUploadDOT.FileBytes;
                        myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUploadDOT.PostedFile.FileName);
                        myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save();
                        if (myTemplateDoc.IdTemplateDoc <= 0)
                            lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
                        else
                        {
                            lblMessage.Text = "File caricato con successo.";
                            lblMessage.CssClass = "Input_Label_bold";
                        }

                        lblMessage.Visible = true;
                        fileUploadDOT = new FileUpload();
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnUploadClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                divElaborazioneInCorso.Style.Add("display", "none");
            }
        }
        //protected void BtnUploadClick(object sender, EventArgs e)
        //{
        //    try//carico template nella tabella dei ruoli elaborati
        //    {
        //        Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
        //        int Anno = 0;
        //        if (OptSuppletivo.Checked == true)
        //            TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
        //        if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
        //            Anno = int.Parse(ddlAnno.SelectedValue);
        //        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
        //        if (myRuolo.IdFlusso <= 0)
        //        {
        //            string sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');";
        //            RegisterScript(sScript,this.GetType());
        //        }
        //        else
        //        {
        //            HttpFileCollection ListFiles = Request.Files;
        //            if (ListFiles != null)
        //            {
        //                foreach ( HttpPostedFile PostedFile in ListFiles)
        //                {
        //                    try
        //                    {
        //                        if (PostedFile.ContentLength > 0)
        //                        {
        //                            ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
        //                            myTemplateDoc.myStringConnection = DichiarazioneSession.StringConnectionOPENgov;
        //                            myTemplateDoc.IdEnte = DichiarazioneSession.IdEnte;
        //                            myTemplateDoc.IdTributo = DichiarazioneSession.CodTributo("");
        //                            myTemplateDoc.IdRuolo = myRuolo.IdFlusso;
        //                            myTemplateDoc.FileMIMEType = fileUploadDOT.PostedFile.ContentType;
        //                            myTemplateDoc.PostedFile = fileUploadDOT.FileBytes;
        //                            myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUploadDOT.PostedFile.FileName);
        //                            myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save();
        //                            if (myTemplateDoc.IdTemplateDoc <= 0)
        //                                lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
        //                            else
        //                            {
        //                                lblMessage.Text = "File caricato con successo.";
        //                                lblMessage.CssClass = "Input_Label_bold";
        //                            }

        //                            lblMessage.Visible = true;
        //                            fileUploadDOT = new FileUpload();
        //                        }
        //                    }
        //                    catch (Exception Err)
        //                    {
        //                        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnUploadClick.PostedFile.errore: ", Err);
        //                        Response.Redirect("../../PaginaErrore.aspx");
        //                    }
        //                    finally
        //                    {
        //                        divElaborazioneInCorso.Style.Add("display", "none");
        //                    }
        //                }
        //            }


        //            if (System.IO.Path.GetFileName(fileUpload.PostedFile.FileName) == "")
        //            {
        //                RegisterScript("GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un file!');", this.GetType());
        //            }
        //            else
        //            {
        //                ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
        //                myTemplateDoc.myStringConnection = DichiarazioneSession.StringConnectionOPENgov;
        //                myTemplateDoc.IdEnte = DichiarazioneSession.IdEnte;
        //                myTemplateDoc.IdTributo = DichiarazioneSession.CodTributo("");
        //                myTemplateDoc.IdRuolo = myRuolo.IdFlusso;
        //                myTemplateDoc.FileMIMEType = fileUploadDOT.PostedFile.ContentType;
        //                myTemplateDoc.PostedFile = fileUploadDOT.FileBytes;
        //                myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUploadDOT.PostedFile.FileName);
        //                myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save();
        //                if (myTemplateDoc.IdTemplateDoc <= 0)
        //                    lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
        //                else
        //                {
        //                    lblMessage.Text = "File caricato con successo.";
        //                    lblMessage.CssClass = "Input_Label_bold";
        //                }

        //                lblMessage.Visible = true;
        //                fileUploadDOT = new FileUpload();
        //            }
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnUploadClick.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //    finally
        //    {
        //        divElaborazioneInCorso.Style.Add("display", "none");
        //    }
        //}
        protected void BtnDownloadClick(object sender, EventArgs e)
        {
            ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
            try//scarico template dalla tabella dei ruoli elaborati
            {
                Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
                int Anno = 0;
                if (OptSuppletivo.Checked == true)
                    TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    Anno = int.Parse(ddlAnno.SelectedValue);
                ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
                if (myRuolo.IdFlusso <= 0)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    myTemplateDoc.myStringConnection = DichiarazioneSession.StringConnectionOPENgov;
                    myTemplateDoc.IdEnte = DichiarazioneSession.IdEnte;
                    myTemplateDoc.IdTributo = DichiarazioneSession.CodTributo("");
                    myTemplateDoc.IdRuolo = myRuolo.IdFlusso;
                    myTemplateDoc.Load();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.btnDownloadClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                divElaborazioneInCorso.Style.Add("display", "none");
            }
            if (myTemplateDoc.PostedFile != null)
            {
                Response.ContentType = myTemplateDoc.FileMIMEType;
                Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", myTemplateDoc.FileName));
                Response.BinaryWrite(myTemplateDoc.PostedFile);
                Response.End();
            }
        }
        #endregion Events

        #region Private Methods
        private void ControlsDataBind()
        {
            LoadDdlAnno();
        }
        private void LoadDdlAnno()
        {
            try
            {
                //ddlAnno.Items.Clear();
                //int EndYear = DateTime.Now.Year + 2;
                //int StartYear = DateTime.Now.Year - 5;
                //ddlAnno.Items.Add(string.Empty);
                //for (int i = EndYear; i >= StartYear; i--)
                //    ddlAnno.Items.Add(i.ToString());
                //ddlAnno.SelectedValue = string.Empty;
                string[] lista = MetodiTariffe.GetAnniTariffe(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                if (lista != null && lista.Length > 0)
                {
                    ddlAnno.DataSource = lista;
                    //ddlAnno.DataTextField = "Descrizione";
                    //ddlAnno.DataValueField = "Id";
                    ddlAnno.DataBind();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.LoadDdlAnno.errore: ", Err);
            }
        }

        private void ManageButtons(ElaborazioneEffettuata preStat, ElaborazioneEffettuata stat, bool EnableState)
        {
            SharedFunction.EnableDisableButton(ref btnCalcola, EnableState);
            SharedFunction.EnableDisableButton(ref btnImport, EnableState);
            SharedFunction.EnableDisableButton(ref btnStampaMinuta, EnableState);
            SharedFunction.EnableDisableButton(ref btnApprovaMinuta, EnableState);
            SharedFunction.EnableDisableButton(ref btnCalcolaRate, EnableState);
            SharedFunction.EnableDisableButton(ref btnStampaDocumenti, EnableState);
            SharedFunction.EnableDisableButton(ref btnEliminaRuolo, EnableState);

            try
            {
                if (stat != null)
                {
                    // gestione Bottone btnCalcola/btnImport a seconda del tributo
                    SharedFunction.EnableDisableButton(ref btnCalcola, stat.DataOraMinutaApprovata == DichiarazioneSession.MyDateMinValue);
                    SharedFunction.EnableDisableButton(ref btnImport, stat.DataOraMinutaApprovata == DichiarazioneSession.MyDateMinValue);
                    if (stat.DataOraMinutaApprovata != DichiarazioneSession.MyDateMinValue)
                    {
                        string sScript = "";
                        sScript += "document.getElementById('trOccupToElab').style.display='none';";
                        RegisterScript(sScript, this.GetType());
                    }
                    // gestione Bottone btnStampaMinuta
                    SharedFunction.EnableDisableButton(ref btnStampaMinuta, (stat.DataOraFineElaborazione > DichiarazioneSession.MyDateMinValue && stat.DataOraMinutaApprovata == DichiarazioneSession.MyDateMinValue));
                    // gestione Bottone btnApprovaMinuta
                    SharedFunction.EnableDisableButton(ref btnApprovaMinuta, (stat.DataOraMinutaStampata > DichiarazioneSession.MyDateMinValue && stat.DataOraMinutaApprovata == DichiarazioneSession.MyDateMinValue));
                    // gestione Bottone btnCalcolaRate
                    SharedFunction.EnableDisableButton(ref btnCalcolaRate, (stat.DataOraMinutaApprovata > DichiarazioneSession.MyDateMinValue && stat.DataOraDocumentiApprovati == DichiarazioneSession.MyDateMinValue));
                    // gestione Bottone btnStampaDocumenti
                    SharedFunction.EnableDisableButton(ref btnStampaDocumenti, (stat.DataOraCalcoloRate > DichiarazioneSession.MyDateMinValue && stat.DataOraDocumentiApprovati == DichiarazioneSession.MyDateMinValue));
                    // gestione Bottone btnEliminaRuolo
                    SharedFunction.EnableDisableButton(ref btnEliminaRuolo, (stat.DataOraFineElaborazione > DichiarazioneSession.MyDateMinValue));
                }
                // Anno senza alcun calcolo effettuato
                else
                {
                    if (preStat == null || preStat.NDichiarazioni == 0)
                    {
                        SharedFunction.EnableDisableButton(ref btnCalcola, false);
                        SharedFunction.EnableDisableButton(ref btnImport, false);
                        string sScript = "";
                        sScript += "document.getElementById('trOccupToElab').style.display='none';";
                        RegisterScript(sScript, this.GetType());
                    }
                    SharedFunction.EnableDisableButton(ref btnStampaMinuta, false);
                    SharedFunction.EnableDisableButton(ref btnApprovaMinuta, false);
                    SharedFunction.EnableDisableButton(ref btnCalcolaRate, false);
                    SharedFunction.EnableDisableButton(ref btnStampaDocumenti, false);
                    SharedFunction.EnableDisableButton(ref btnEliminaRuolo, false);
                }

                if (DichiarazioneSession.CodTributo("") == Utility.Costanti.TRIBUTO_SCUOLE && ddlAnno.SelectedValue == "...")
                    SharedFunction.EnableDisableButton(ref btnImport, true);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.ManageButtons.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        private void ShowCalcoloInCorso()
        {
            try
            {
                divElaborazioneInCorso.Style.Add("display", "");
                tblStatistiche.Style.Add("display", "none");
                ddlAnno.Enabled = false;
                txtSogliaMinima.Enabled = false;
                ManageButtons(null, null, false);
                Session["OSAPAnnoCalcoloInCorso"] = ddlAnno.SelectedValue;
                if (OptSuppletivo.Checked)
                    Session["OSAPTipoRuoloCalcoloInCorso"] = "1";
                else
                    Session["OSAPTipoRuoloCalcoloInCorso"] = "0";
                if (CacheManager.GetAvanzamentoElaborazione() != null)
                    LblAvanzamento.Text = CacheManager.GetAvanzamentoElaborazione();

                //Response.AppendHeader("refresh", "5");
                RegisterScript("setTimeout('window.location.href = window.location.href', 5000);", this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.ShowCalcoloInCorso.errore: ", ex);
            }
        }

        protected void SelAllRow(object sender, System.EventArgs e)
        {
            try
            {
                bool bCheck = true;
                bCheck = ((CheckBox)sender).Checked;
                foreach (GridViewRow MyItemGrd in GrdOccupaz.Rows)
                {
                    ((CheckBox)(MyItemGrd.FindControl("ChkSelezionato"))).Checked = bCheck;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.SelAllRow.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #endregion Private Methods
        #region Codice generato da Progettazione Web Form
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: questa chiamata   richiesta da Progettazione Web Form ASP.NET.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEliminaRuolo.Click += new System.Web.UI.ImageClickEventHandler(this.btnEliminaRuolo_Click);
            this.btnStampaDocumenti.Click += new System.Web.UI.ImageClickEventHandler(this.btnStampaDocumenti_Click);
            this.btnCalcolaRate.Click += new System.Web.UI.ImageClickEventHandler(this.btnCalcolaRate_Click);
            this.btnApprovaMinuta.Click += new System.Web.UI.ImageClickEventHandler(this.btnApprovaMinuta_Click);
            this.btnStampaMinuta.Click += new System.Web.UI.ImageClickEventHandler(this.btnStampaMinuta_Click);
            this.btnCalcola.Click += new System.Web.UI.ImageClickEventHandler(this.btnCalcola_Click);
            this.btnImport.Click += new System.Web.UI.ImageClickEventHandler(this.btnImport_Click);
            this.ddlAnno.SelectedIndexChanged += new System.EventHandler(this.ddlAnno_SelectedIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);
            //*** 20130610 - ruolo supplettivo ***
            this.OptOrdinario.CheckedChanged += new System.EventHandler(this.ddlAnno_SelectedIndexChanged);
            this.OptSuppletivo.CheckedChanged += new System.EventHandler(this.ddlAnno_SelectedIndexChanged);
            //*** ***
        }
        #endregion

        protected void ChkSelezionato_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow MyItemGrd in GrdOccupaz.Rows)
                {
                    if (((CheckBox)(MyItemGrd.FindControl("ChkSelezionato"))).Checked == true)
                    {
                        if (hfOccupazioniToElab.Value != "")
                            hfOccupazioniToElab.Value += ",";
                        hfOccupazioniToElab.Value += ((HiddenField)(MyItemGrd.FindControl("hfIdTipologiaOccupazione"))).Value;
                    }
                }
                Log.Debug("ElaborazioneAvvisi.ChkSelezionato_CheckedChanged.OccupazioniToElab=" + hfOccupazioniToElab.Value);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ElaborazioneAvvisi.ChkSelezionato_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
    }
}