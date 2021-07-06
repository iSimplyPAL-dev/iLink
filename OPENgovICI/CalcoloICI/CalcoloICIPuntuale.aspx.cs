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
using AnagInterface; 
using System.Configuration;
using DichiarazioniICI.Database;
using Business;
using Ribes;
using ComPlusInterface;
using log4net;
using ElaborazioneDatiStampeInterface;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per l'elaborazione del calcolo puntuale IMU/TASI.
    /// Contiene i parametri di calcolo, le funzioni della comandiera e i frame per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Analisi eventi</em>
    /// </revision>
    /// </revisionHistory>
    public partial class CalcoloICIPuntuale : BaseEnte
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CalcoloICIPuntuale));
        HtmlControl myFrame;
        /// <summary>
        /// 
        /// </summary>
        protected string Tributo;

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

        #region METODI
        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco degli anni di riferimento.
        /// </summary>
        protected DataView GetAnniAliquote()
        {
            DataView Vista = new Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
            Vista.Sort = "ANNO DESC";
            return Vista;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            try
            {
                if (!IsPostBack)
                {
                    string strAnno = String.Empty;
                    ddlAnnoRiferimento.DataBind();

                    ListItem myitem;

                    myitem = new ListItem();
                    myitem.Value = "";
                    myitem.Text = "...";
                    ddlAnnoRiferimento.Items.Add(myitem);
                    //*** 20150703 - INTERROGAZIONE GENERALE ***
                    if (Request["anno"] != null)
                    {
                        ddlAnnoRiferimento.SelectedValue = Request["anno"];
                        log.Debug(Request["anno"]);
                    }
                    if (Request["Provenienza"] == null || Request["Provenienza"] != "INTERGEN")
                    {
                        string sScript = "";
                        Type csType = this.GetType();
                        sScript = "";
                        sScript += "parent.Comandi.document.GetElementbyId('Cancel').style.display='none';";
                        sScript += "";
                        RegisterScript(sScript, this.GetType());
                    }
                    //*** ***

                    strAnno = ddlAnnoRiferimento.SelectedItem.Value;

                    lblLinkCalcolo.Attributes.Add("onMouseOver", "this.style.cursor='hand';");
                    lblLinkCalcolo.Attributes.Add("OnCLick", "return LinkCalcoloICI(" + strAnno + ")");

                    //*** 20140509 - TASI ***
                    int COD_CONTRIB = 0;
                    if (Request["COD_CONTRIB"] != null)
                    {
                        COD_CONTRIB = int.Parse(Request["COD_CONTRIB"].ToString());
                        string strscript = "";
                        strscript = strscript + "parent.Comandi.location.href='CCalcoloICIPuntuale.aspx';";
                        RegisterScript(strscript, this.GetType());
                    }
                    LoadFrame(COD_CONTRIB);
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerificaContribuente_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                //Anagrafica.DLL.GestioneAnagrafica oAnagrafica; 
                DettaglioAnagrafica oDettaglioAnagrafica;

                // Valorizzazione delle variabili di sessione necessaria all'anagrafica
                HttpContext.Current.Session["modificaDiretta"] = false;
                HttpContext.Current.Session["PARAMETROENV"] = ConstWrapper.ParametroENV;
                HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"] = ConstWrapper.ParametroAPPLICATION;
                HttpContext.Current.Session["username"] = ConstWrapper.UserNameFramework;
                HttpContext.Current.Session["COD_TRIBUTO"] = ConstWrapper.CodiceTributoAnag;
                HttpContext.Current.Session["CODENTE"] = ConstWrapper.CodiceEnte;
                HttpContext.Current.Session["Anagrafica"] = ConstWrapper.ParametroAnagrafica;

                oDettaglioAnagrafica = new DettaglioAnagrafica();
                oDettaglioAnagrafica.COD_CONTRIBUENTE = -1;
                oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = -1;

                HttpContext.Current.Session["contribuente"] = oDettaglioAnagrafica;

                RegisterScript("ApriRicercaAnagrafeCalcoloIci('contribuente');", this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.InkVerificaContribuente_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// Ribaltamento dei dati anagrafici nella maschera video
        /// </summary>
        private void Ribalta()
        {
            try
            {
                DettaglioAnagrafica oDettaglioAnagrafica;
                if (Session["contribuente"] != null)
                {
                    oDettaglioAnagrafica = (DettaglioAnagrafica)(Session["contribuente"]);
                    txtCodFiscaleContr.Text = oDettaglioAnagrafica.CodiceFiscale;
                    txtPIVAContr.Text = oDettaglioAnagrafica.PartitaIva;
                    txtCognomeContr.Text = oDettaglioAnagrafica.Cognome;
                    txtNomeContr.Text = oDettaglioAnagrafica.Nome;
                    txtCodContribuenteCon.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                    txtCodContribuenteCon1.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                    if (oDettaglioAnagrafica.Sesso == "")
                    {
                        rdbMaschioContr.Checked = false;
                        rdbFemminaContr.Checked = false;
                        rdbGiuridicaContr.Checked = false;
                    }
                    if (oDettaglioAnagrafica.Sesso == "M")
                        rdbMaschioContr.Checked = true;
                    if (oDettaglioAnagrafica.Sesso == "F")
                        rdbFemminaContr.Checked = true;
                    if (oDettaglioAnagrafica.Sesso == "G")
                        rdbGiuridicaContr.Checked = true;

                    if (oDettaglioAnagrafica.DataNascita != "00/00/1900")
                    {
                        txtDataNascContr.Text = oDettaglioAnagrafica.DataNascita;
                    }
                    else
                    {
                        txtDataNascContr.Text = string.Empty;
                    }
                    txtComNascContr.Text = oDettaglioAnagrafica.ComuneNascita;
                    txtProvNascContr.Text = oDettaglioAnagrafica.ProvinciaNascita;
                    txtViaResContr.Text = oDettaglioAnagrafica.ViaResidenza;
                    txtNumCivResContr.Text = oDettaglioAnagrafica.CivicoResidenza;
                    txtIntResContr.Text = oDettaglioAnagrafica.InternoCivicoResidenza;
                    txtScalaResContr.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;
                    txtComuneResContr.Text = oDettaglioAnagrafica.ComuneResidenza;
                    txtProvResContr.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                    txtCapResContr.Text = oDettaglioAnagrafica.CapResidenza;
                    txtEsponenteCivico.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;

                    //*** 20140509 - TASI ***
                    ////*** 20130422 - aggiornamento IMU ***
                    LoadFrame(int.Parse(txtCodContribuenteCon.Value.ToString()));
                    //*** ***
                    VisLabelConfermata();
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.Ribalta.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnRibalta_Click(object sender, System.EventArgs e)
        {
            this.Ribalta();
        }

        /// <summary>
        /// Pulsante per il calcolo dell'IMU/TASI; blocco calcolo se mancano i dati obbligatori; il calcolo viene fatto richiamando il servizio esterno in modalità sincrona.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="09/05/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnCalcoloICI_Click(object sender, System.EventArgs e)
        {
            string strscript = "";
            try
            {
                Hashtable objHashTable = new Hashtable();
                string strConnectionStringOPENgovICI;
                string strConnectionStringOPENgovProvvedimenti;
                string strConnectionStringAnagrafica;
                string strConnectionStringOPENgovTerritorio;
                string strConnectionStringOPENgovCatasto;
                bool bVersatoNelDovuto = chkVersatoNelDovuto.Checked;
                bool bCalcolaArrotondamento = chkArrotondamento.Checked;

                log.Debug("CalcoloICIPuntuale::btnCalcoloICI::inizio");
                objHashTable.Add("CodENTE", ConstWrapper.CodiceEnte);
                strConnectionStringOPENgovICI = ConstWrapper.StringConnection;
                strConnectionStringAnagrafica = ConstWrapper.StringConnectionAnagrafica;
                strConnectionStringOPENgovProvvedimenti = "";
                strConnectionStringOPENgovTerritorio = "";
                strConnectionStringOPENgovCatasto = "";

                objHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstWrapper.StringConnectionOPENgov);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", strConnectionStringOPENgovICI);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", strConnectionStringOPENgovProvvedimenti);
                objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVTERRITORIO", strConnectionStringOPENgovTerritorio);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVCATASTO", strConnectionStringOPENgovCatasto);
                objHashTable.Add("USER", ConstWrapper.sUsername);
                objHashTable.Add("COD_TRIBUTO", ConstWrapper.CodiceTributo);

                Tributo = string.Empty;
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;
                objHashTable.Add("TRIBUTOCALCOLO", Tributo);

                objHashTable.Add("ANNODA", ddlAnnoRiferimento.SelectedItem.Text);
                objHashTable.Add("ANNOA", "-1");

                objHashTable.Add("CODCONTRIBUENTE", txtCodContribuenteCon1.Text);
                //*** 20150430 - TASI Inquilino ***
                if (optTASIProp.Checked == true)
                    objHashTable.Add("TASIAPROPRIETARIO", 1);
                else
                    objHashTable.Add("TASIAPROPRIETARIO", 0);
                //*** ***

                int TipoCalcolo = CalcoloICI.TIPOCalcolo_STANDARD;
                if (rdbCalcoloNetto.Checked == true)
                {
                    TipoCalcolo = CalcoloICI.TIPOCalcolo_NETTOVERSATO;
                }
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                //blocco calcolo se manca rendita e tipo utilizzo di RE
                DataTable dtBloccoCalcolo = null;
                int isTASISuProp = 0;
                if (optTASIProp.Checked == true)
                    isTASISuProp = 1;
                if (!new CalcoloICI().isBloccaCalcolo(ConstWrapper.CodiceEnte, int.Parse(ddlAnnoRiferimento.SelectedItem.Text), int.Parse(txtCodContribuenteCon1.Text), isTASISuProp, out dtBloccoCalcolo))
                {
                    log.Debug("CalcoloICIPuntuale::btnCalcoloICI::attivo il servizio al percorso::" + ConstWrapper.UrlServizioCalcoloICI);
                    bool iRetValCalcoloICI;
                    IFreezer remObjectFreezer = (IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI);

                    bool ConfigDichiarazione = ConstWrapper.ConfigurazioneDichiarazione;
                    objSituazioneFinale[] ListSituazioneFinale = null;
                    iRetValCalcoloICI = remObjectFreezer.CalcoloFromSoggetto(ConstWrapper.StringConnectionOPENgov, strConnectionStringOPENgovICI,  ConstWrapper.CodiceEnte,int.Parse( txtCodContribuenteCon1.Text),Tributo, ConstWrapper.CodiceTributo, ddlAnnoRiferimento.SelectedItem.Text,"-1", false,  ConfigDichiarazione, bVersatoNelDovuto, bCalcolaArrotondamento, TipoCalcolo,"", isTASISuProp.ToString(), string.Empty, ConstWrapper.sUsername, ref ListSituazioneFinale); // bool ribaltamento versato nel dovuto :: bVersatoNelDovuto
                    log.Debug("CalcoloICIPuntuale::btnCalcoloICI::fatto");
                    DivAttesa.Style.Add("display", "none");
                    if (iRetValCalcoloICI)
                    {
                        new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Elaborazioni, "Puntuale.btnCalcoloICI-" + ddlAnnoRiferimento.SelectedItem.Text, Utility.Costanti.AZIONE_NEW.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, int.Parse(txtCodContribuenteCon1.Text));
                    }
                    strscript = "GestAlert('a','" + (iRetValCalcoloICI == true ? "success" : "warning") + "', '', '', 'Calcolo puntuale" + (iRetValCalcoloICI == true ? "" : " non") + " effettuato con successo.');";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    strscript = "GestAlert('a', 'warning', '', '', 'Impossibile effettuare il Calcolo puntuale per mancanza di rendita e/o tipo utilizzo!');";
                    RegisterScript(strscript, this.GetType());
                }
                //*** ***
            }
            catch (Exception ex)
            {
                //*** 20120704 - IMU ***
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.btnCalcoloICI_Click.errore: ", ex);
                if (ex.Message == "00000")
                {
                    strscript = "GestAlert('a', 'warning', '', '', 'Non sono state individuate dichiarazioni per il contribuente selezionato. Impossibile effettuare il calcolo.');";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    strscript = "GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante il calcolo puntuale');";
                    RegisterScript(strscript, this.GetType());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        protected void btnConfermaCalcolo_Click(object sender, System.EventArgs e)
        {
            string strscript = "";
            try
            {
                TpSituazioneFinaleIci tpCalcolo = new TpSituazioneFinaleIci();
                bool ResultUpdate = tpCalcolo.ConfermaCalcoloICI(txtCodContribuenteCon1.Text, ddlAnnoRiferimento.SelectedItem.Text);

                if (ResultUpdate.CompareTo(true) == 0)
                {
                    //*** 20120704 - IMU ***
                    string strMsg = "Calcolo escluso in data " + DateTime.Now.ToShortDateString() + " dall'utente \"" + ConstWrapper.sUsername.ToString() + "\"";
                    lblConfermato.Text = strMsg;
                    lblConfermato.Visible = true;
                    strscript = strscript + "UpdateMsgEsclusione();";
                    RegisterScript(strscript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                //*** 20120704 - IMU ***
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.btnConfermaCalcolo_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** ***

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAnnoRiferimento_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                //*** 20140509 - TASI ***
                ////*** 20130422 - aggiornamento IMU ***
                int COD_CONTRIB = 0;
                if (txtCodContribuenteCon.Value.CompareTo("") != 0)
                    COD_CONTRIB = int.Parse(txtCodContribuenteCon.Value.ToString());
                LoadFrame(COD_CONTRIB);
                //*** ***
                VisLabelConfermata();
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.ddlAnnoRiferimento_SelectedIndexChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        private void VisLabelConfermata()
        {
            TpSituazioneFinaleIci TpCalcolo = new TpSituazioneFinaleIci();
            string Codcontribuente = string.Empty;
            string Anno = ddlAnnoRiferimento.SelectedItem.Text.ToString();
            bool nettoVersato = true;

            //*** 20130422 - aggiornamento IMU ***
            nettoVersato = rdbCalcoloNetto.Checked;
            //*** ***
            try
            {
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;

                DataTable tblBollettinoICI = TpCalcolo.GetBollettinoICI(txtCodContribuenteCon.Value, ddlAnnoRiferimento.SelectedItem.Text, ConstWrapper.CodiceEnte, Tributo, nettoVersato);

                if (tblBollettinoICI.Rows.Count > 0)
                {
                    // controllo che valore ha il campo bloccato, data_blocco e utente_blocco
                    // e visualizzo la label che indica se la situazione è già stata bloccata
                    bool VersInDovuto = bool.Parse(tblBollettinoICI.Rows[0]["DOVUTO_FORZATO"].ToString());
                    bool Confermato = bool.Parse(tblBollettinoICI.Rows[0]["CONFERMATO_ICI"].ToString());
                    string UtenteConferma = tblBollettinoICI.Rows[0]["UTENTE_CONFERMA"].ToString();
                    DateTime DataConferma = tblBollettinoICI.Rows[0]["DATA_CONFERMA"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(tblBollettinoICI.Rows[0]["DATA_CONFERMA"].ToString());

                    if (Confermato.CompareTo(false) == 0)
                    {
                        //*** 20120704 - IMU ***
                        lblConfermato.Text = "Calcolo escluso in data " + DataConferma.ToShortDateString() + " dall'utente \"" + UtenteConferma + "\"";
                        lblConfermato.Visible = true;
                    }
                    else
                    {
                        lblConfermato.Text = "";
                        lblConfermato.Visible = false;
                    }

                    if (VersInDovuto.CompareTo(true) == 0)
                    {
                        chkVersatoNelDovuto.Checked = true;
                    }
                    else
                    {
                        chkVersatoNelDovuto.Checked = false;
                    }
                }
                else
                {
                    lblConfermato.Text = "";
                    lblConfermato.Visible = false;
                    chkVersatoNelDovuto.Checked = false;
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.VisLabelConfermata.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        //*** 20140509 - TASI ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Tributo_CheckedChanged(object sender, System.EventArgs e)
        {
            int COD_CONTRIB = 0;
            try
            {
                if (txtCodContribuenteCon.Value.CompareTo("") != 0)
                    COD_CONTRIB = int.Parse(txtCodContribuenteCon.Value.ToString());
                LoadFrame(COD_CONTRIB);
                //*** 20150430 - TASI Inquilino ***
                if (chkTASI.Checked == true)
                { optTASINo.Enabled = true; optTASIProp.Enabled = true; }
                else
                { optTASINo.Enabled = false; optTASIProp.Enabled = false; }

                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.Tributo_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        private void LoadFrame(int COD_CONTRIB)
        {
            try
            {
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;

                if ((COD_CONTRIB > 0) && ((txtCodContribuenteCon.Value == string.Empty) || txtCodContribuenteCon.Value != "-1"))
                {
                    DettaglioAnagrafica oDettAnag = HelperAnagrafica.GetDatiPersona(COD_CONTRIB);
                    Session["contribuente"] = oDettAnag;
                    if (txtCodContribuenteCon1.Text != txtCodContribuenteCon.Value.ToString() || (txtCodContribuenteCon.Value == string.Empty || txtCodContribuenteCon.Value == "-1"))
                        this.Ribalta();

                    myFrame = (HtmlControl)(this.FindControl("loadGridRiepilogoCalcoloICI"));
                    myFrame.Attributes.Add("src", "./GetCalcoloICI.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&ID_PROG_ELAB=-1&bNettoVersato=" + rdbCalcoloNetto.Checked.ToString() + "&COD_TRIBUTO=" + Tributo + "&COD_CONTRIB=" + COD_CONTRIB);

                    myFrame = (HtmlControl)(this.FindControl("loadCalcoloCatVSCl"));
                    myFrame.Attributes.Add("src", "./GetImmobiliICI.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&ID_PROG_ELAB=-1&COD_TRIBUTO=" + Tributo + "&COD_CONTRIB=" + COD_CONTRIB);

                    myFrame = (HtmlControl)(this.FindControl("loadBollettino"));
                    myFrame.Attributes.Add("src", "./GetBollettinoIci.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&bNettoVersato=" + rdbCalcoloNetto.Checked.ToString() + "&COD_TRIBUTO=" + Tributo + "&COD_CONTRIB=" + COD_CONTRIB);

                    myFrame = (HtmlControl)(this.FindControl("loadVersamenti"));
                    myFrame.Attributes.Add("src", "./GetVersamenti.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&Cognome=" + txtCognomeContr.Text + "&Nome=" + txtNomeContr.Text + "&CodiceFiscale=" + txtCodFiscaleContr.Text + "&PartitaIVA=" + txtPIVAContr.Text + "&COD_TRIBUTO=" + Tributo + "&COD_CONTRIB=" + COD_CONTRIB);
                }
                else {
                    string sScript = "document.getElementById('DivLoading').style.display='none';";
                    RegisterScript(sScript, this.GetType());
                }
                myFrame = (HtmlControl)(this.FindControl("loadGridAliquote"));
                myFrame.Attributes.Add("src", "./ElencoAliquote.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&COD_TRIBUTO=" + Tributo + "&COD_CONTRIB=" + COD_CONTRIB);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.LoadFrame.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="05/11/2020">
        /// devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
        /// </revision>
        /// </revisionHistory>
        protected void btnElaboraDoc_Click(object sender, System.EventArgs e)
        {
            try
            {
                string strscript = "";
                RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] oGruppoRet;
                bool nettoVersato = rdbCalcoloNetto.Checked;
                string TipoElaborazione = "PROVA";
                string TipoCalcolo = "IMU";
                string TipoBollettino = "F24";
                int nDecimal = 2;
                bool bIsStampaBollettino = true;
                bool bCreaPDF = false;
                string[] TipologieEsclusione = GetTipologieDaEscludere();
                string IdFlussoRuolo = "-1";

                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;
                DataTable DtElaborazione = new TpSituazioneFinaleIci().GetCalcoloICISoggetti(ConstWrapper.CodiceEnte, txtCodContribuenteCon.Value.ToString(), ddlAnnoRiferimento.SelectedItem.Text, Tributo);
                Session["TblDaStampare"] = DtElaborazione;
                int[,] ContribuentiDaElaborare = new ElaborazioneDocumenti.ElaborazioneDocumenti().GetArrayContribuenti();

                //*** 20130422 - aggiornamento IMU ***
                if (int.Parse(ddlAnnoRiferimento.SelectedItem.Text) < 2012)
                {
                    nettoVersato = false;
                    TipoCalcolo = "ICI";
                    TipoBollettino = "123";
                }

                if (ContribuentiDaElaborare.Length > 0)
                {
                    //faccio partire l'elaborazione chiamo il servizio di elaborazione delle stampe massive.
                    Type typeofRI = typeof(IElaborazioneStampeICI);
                    IElaborazioneStampeICI remObject = (IElaborazioneStampeICI)Activator.GetObject(typeofRI, ConstWrapper.UrlServizioElaborazioneDocumentiICI.ToString());
                    /**** 201810 - Calcolo puntuale ****///*** 201511 - template documenti per ruolo ***
                    oGruppoRet = remObject.ElaborazioneMassivaDocumenti(ConstWrapper.DBType, ConstWrapper.CodiceTributo, ContribuentiDaElaborare, int.Parse(ddlAnnoRiferimento.SelectedItem.Text), ConstWrapper.CodiceEnte, IdFlussoRuolo, TipologieEsclusione, ConstWrapper.StringConnection, ConstWrapper.StringConnectionOPENgov, ConstWrapper.StringConnectionAnagrafica, ConstWrapper.PathStampe, ConstWrapper.PathVirtualStampe, ConstWrapper.nMaxDocPerFile, TipoElaborazione, GetImpostazioniBollettino(), TipoCalcolo, TipoBollettino, bIsStampaBollettino, bCreaPDF, nettoVersato, nDecimal, false, false, string.Empty,ConstWrapper.CodiceTributo);
                    //*** ***
                    if (oGruppoRet != null)
                    {
                        // se viene restituito il gruppo documenti apro il popup
                        Session["StampaPuntuale"] = oGruppoRet[0];
                        ApriPopUpDownloadDocumenti();
                    }
                    else
                    {
                        strscript = "GestAlert('a', 'danger', '', '', 'Non sono stati elaborati correttamente i documenti selezionati.');";
                        RegisterScript(strscript, this.GetType());
                    }
                }
                else
                {
                    // se non sono stati selezionati contribuenti per l'elaborazione do il messaggio javascript che l'elaborazione non è possibile.
                    strscript = "GestAlert('a', 'warning', '', '', 'Per effetture elaborazioni di documenti è necessario selezionare le posizioni da elaborare!');";
                    RegisterScript(strscript, this.GetType());
                }
                //*** ***
                DivAttesa.Style.Add("display", "none");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.btnElaboraDoc_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
            DataRow dr;
            DataSet ds = new DataSet();
            DataTable dtRiepilogoICI = null;
            string NameXLS = string.Empty;
            int x;
            string[] arraystr = null;
            string strscript = "";

            try
            {
                int COD_CONTRIB = 0;
                if (txtCodContribuenteCon.Value.CompareTo("") != 0)
                    COD_CONTRIB = int.Parse(txtCodContribuenteCon.Value.ToString());
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;

                NameXLS = ConstWrapper.CodiceEnte + "_CALCOLO_ICI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                //DATI CONTRIBUENTE
                if ((Session["contribuente"] != null) && (Session["TblCalcoloICI"] != null) && (Session["tblRiepilogoImmobili"] != null) && (Session["TabBollettino"] != null))
                {
                    ArrayList ArrListHeaders = new ArrayList();
                    //*** 20140509 - TASI ***//*** 20120629 - IMU ***
                    checked
                    {
                        for (int i = 0; i <= 18; i++)//(int i = 0; i < 18; i++)
                        {
                            ArrListHeaders.Add("");
                        }
                    }
                    //*** ***
                    arraystr = (string[])ArrListHeaders.ToArray(typeof(string));
                    DettaglioAnagrafica objDettaglioAnag = (DettaglioAnagrafica)Session["contribuente"];
                    DataTable CalcoloIci = (DataTable)Session["TblCalcoloICI"];
                    DataTable ImmobiliIci = (DataTable)Session["tblRiepilogoImmobili"];
                    DataTable BollettinoIci = (DataTable)Session["TabBollettino"];

                    ds.Tables.Add("RESULT_CALCOLO_ICI");
                    //*** 20140509 - TASI ***//*** 20120629 - IMU ***
                    for (x = 0; x <= 18; x++)//(int i = 0; i < 18; i++)
                    {
                        ds.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
                    }
                    dtRiepilogoICI = ds.Tables["RESULT_CALCOLO_ICI"];
                    //*** ***

                    //inserisco intestazione di colonna
                    // COMUNE DI DESCRIZIONE ENTE
                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = ConstWrapper.DescrizioneEnte;
                    x++; dr[x] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year; ;
                    dtRiepilogoICI.Rows.Add(dr);

                    //aggiungo uno spazio
                    dr = dtRiepilogoICI.NewRow();
                    dtRiepilogoICI.Rows.Add(dr);

                    //*** 20120704 - IMU ***
                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Calcolo Puntuale - Riepilogo Immobili e Importo Calcolo".ToUpper();
                    dtRiepilogoICI.Rows.Add(dr);

                    // riga vuota
                    dr = dtRiepilogoICI.NewRow();
                    dtRiepilogoICI.Rows.Add(dr);

                    // riga DATI CONTRIBUENTE
                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "DATI CONTRIBUENTE";
                    dtRiepilogoICI.Rows.Add(dr);

                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Cognome/Ragione Sociale : " + objDettaglioAnag.Cognome.ToUpper();
                    x++; dr[x] = "Nome : " + objDettaglioAnag.Nome.ToUpper();
                    dtRiepilogoICI.Rows.Add(dr);
                    //				
                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Data Nascita : " + objDettaglioAnag.DataNascita;
                    dtRiepilogoICI.Rows.Add(dr);

                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Residente in : " + objDettaglioAnag.ViaResidenza.ToUpper() + " ," + objDettaglioAnag.CivicoResidenza.ToUpper();
                    dtRiepilogoICI.Rows.Add(dr);

                    // riga vuota
                    dr = dtRiepilogoICI.NewRow();
                    dtRiepilogoICI.Rows.Add(dr);

                    // riga Riepilogo importo Calcolo ICI
                    //*** 20120704 - IMU ***
                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Riepilogo Importi Calcolo".ToUpper();
                    dtRiepilogoICI.Rows.Add(dr);

                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Anno";
                    //*** 20140509 - TASI ***
                    x++; dr[x] = "Tributo";
                    //*** ***
                    x++; dr[x] = "Imp. Abi. Princ. Euro";
                    x++; dr[x] = "Imp.Altri Fab. Euro";
                    x++; dr[x] = "Imp.Aree Fab. Euro";
                    x++; dr[x] = "Imp.Terreni Euro";
                    x++; dr[x] = "Detrazione Euro";
                    x++; dr[x] = "Detrazione Statale Applicata Euro";
                    x++; dr[x] = "Totale Euro";
                    dtRiepilogoICI.Rows.Add(dr);

                    foreach (DataRow drImm in CalcoloIci.Rows)
                    {
                        dr = dtRiepilogoICI.NewRow();
                        x = 0; dr[x] = drImm["ANNO"].ToString();
                        //*** 20140509 - TASI ***
                        x++; dr[x] = drImm["DESCRTRIBUTO"].ToString();
                        //*** ***
                        x++; dr[x] = drImm["Imp_Abi_Princ"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["Imp_Altri_Fab"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["Imp_Aree_Fab"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["Imp_Terreni"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["Detrazione"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["DSA"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["Totale"].ToString().Replace(".", "");
                        dtRiepilogoICI.Rows.Add(dr);
                    }

                    // riga vuota
                    dr = dtRiepilogoICI.NewRow();
                    dtRiepilogoICI.Rows.Add(dr);
                    //RIEPILOGO IMMOBILI
                    //*** 20120704 - IMU ***
                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "RIEPILOGO IMMOBILI CALCOLO";
                    dtRiepilogoICI.Rows.Add(dr);

                    dr = dtRiepilogoICI.NewRow();
                    //*** 20140509 - TASI ***
                    x = 0; dr[x] = "Tributo";
                    //*** ***
                    x++; dr[x] = "Foglio";
                    x++; dr[x] = "Numero";
                    x++; dr[x] = "Sub";
                    x++; dr[x] = "Categoria";
                    x++; dr[x] = "Classe";
                    x++; dr[x] = "Valore";
                    x++; dr[x] = "Indirizzo";
                    x++; dr[x] = "Tipo Rendita";
                    x++; dr[x] = "% Possesso";
                    x++; dr[x] = "Mesi Possesso";
                    x++; dr[x] = "Abit. Princ.";
                    //*** 20120629 - IMU ***
                    x++; dr[x] = "N.Utilizzatori";
                    x++; dr[x] = "Ridotto";
                    x++; dr[x] = "Esente";
                    x++; dr[x] = "Coltivatore Diretto";
                    x++; dr[x] = "N.Figli";
                    x++; dr[x] = "Aliquota";
                    x++; dr[x] = "Dovuto Euro";
                    dtRiepilogoICI.Rows.Add(dr);

                    foreach (DataRow drImm in ImmobiliIci.Rows)
                    {
                        dr = dtRiepilogoICI.NewRow();
                        //*** 20140509 - TASI ***
                        x = 0; dr[x] = drImm["DESCRTRIBUTO"].ToString();
                        //*** ***
                        x++; dr[x] = drImm["FOGLIO"].ToString();
                        x++; dr[x] = drImm["NUMERO"].ToString();
                        x++; dr[x] = drImm["SUBALTERNO"].ToString();
                        x++; dr[x] = drImm["CATEGORIA"].ToString();
                        x++; dr[x] = drImm["CLASSE"].ToString();
                        x++; dr[x] = drImm["Valore"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["INDIRIZZONEW"].ToString();
                        x++; dr[x] = drImm["TIPO_RENDITA"].ToString();
                        x++; dr[x] = drImm["PERC_POSSESSO"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["MESI_POSSESSO"].ToString();
                        x++; dr[x] = Business.CoreUtility.FormattaGrdBoolToString(0, drImm["FLAG_PRINCIPALE"].ToString());
                        x++; dr[x] = drImm["numero_utilizzatori"].ToString();
                        x++; dr[x] = Business.CoreUtility.FormattaGrdBoolToString(1, drImm["FLAG_RIDUZIONE"].ToString());
                        x++; dr[x] = Business.CoreUtility.FormattaGrdBoolToString(1, drImm["FLAG_ESENTE"].ToString());
                        if ((drImm["COLTIVATOREDIRETTO"].ToString() == "1") || (drImm["COLTIVATOREDIRETTO"].ToString().ToUpper() == "TRUE"))
                        {
                            x++; dr[x] = "SI";
                        }
                        else
                        {
                            x++; dr[x] = "NO";
                        }
                        x++; dr[x] = drImm["NUMEROFIGLI"].ToString();
                        if (drImm["PERCENTCARICOFIGLI"] != DBNull.Value)
                        {
                            if (Convert.ToDecimal(drImm["PERCENTCARICOFIGLI"].ToString()) > 0)
                            {
                                dr[x] += " rid. al " + Convert.ToDecimal(drImm["PERCENTCARICOFIGLI"].ToString()).ToString() + "%";
                            }
                        }
                        x++; dr[x] = drImm["ICI_VALORE_ALIQUOTA"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["ICI_TOTALE_DOVUTA"].ToString().Replace(".", "");
                        dtRiepilogoICI.Rows.Add(dr);
                    }
                    //*** ***
                    dr = dtRiepilogoICI.NewRow();
                    dtRiepilogoICI.Rows.Add(dr);

                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "RIEPILOGO BOLLETTINO";
                    dtRiepilogoICI.Rows.Add(dr);

                    dr = dtRiepilogoICI.NewRow();
                    x = 0; dr[x] = "Descrizione";
                    //*** 20140509 - TASI ***
                    x++; dr[x] = "Tributo";
                    //*** ***
                    x++; dr[x] = "Imp. Abi. Princ. Euro";
                    x++; dr[x] = "Imp. Terr. Agr. Euro";
                    x++; dr[x] = "Imp. Altri Fab.Euro";
                    x++; dr[x] = "Imp. Aree Fab. Euro";
                    x++; dr[x] = "Imp. Detrazione Euro";
                    x++; dr[x] = "Imp. Detrazione Statale Applicata Euro";
                    x++; dr[x] = "Imp. Senza Arr. Euro";
                    x++; dr[x] = "Imp. Arr. Euro";
                    x++; dr[x] = "Imp. Totale Euro";
                    dtRiepilogoICI.Rows.Add(dr);

                    foreach (DataRow drImm in BollettinoIci.Rows)
                    {
                        dr = dtRiepilogoICI.NewRow();
                        x = 0; dr[x] = drImm["DESCRIZIONE"].ToString().ToLower();
                        //*** 20140509 - TASI ***
                        x++; dr[x] = drImm["DESCRTRIBUTO"].ToString();
                        //*** ***
                        x++; dr[x] = drImm["IMP_ABI_PRINC"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_TERRENI"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_ALRI_FAB"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_AREE_FAB"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_DET"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_DET_DSA"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_S_ARR"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_ARROT"].ToString().Replace(".", "");
                        x++; dr[x] = drImm["IMP_TOTALE"].ToString().Replace(".", "");
                        dtRiepilogoICI.Rows.Add(dr);
                    }
                }
                else
                {
                    strscript = "GestAlert('a', 'warning', '', '', 'Non sono presenti tutti i dati per effettuare la stampa del report in excel!');";
                    RegisterScript(strscript, this.GetType());
                }
            }
            catch (Exception er)
            {
                strscript = "GestAlert('a', 'danger', '', '', '" + er.ToString() + "');";
                RegisterScript(strscript, this.GetType());

                dtRiepilogoICI = null;
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.btnStampaExcel_Click.errore: ", er);
            }
            if (dtRiepilogoICI != null)
            {
                //definisco l'insieme delle colonne da esportare
                int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
                //esporto i dati in excel
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                objExport.ExportDetails(dtRiepilogoICI, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel,  NameXLS);
            }
        }

        private string[] GetTipologieDaEscludere()
        {
            try
            {
                ArrayList ArrListTipEsclusione = new ArrayList();

                foreach (GridViewRow oItem in GrdTipoRendita.Rows)
                {
                    if (oItem.FindControl("chkEsclusione") != null)
                    {
                        CheckBox oCheck = (CheckBox)oItem.FindControl("chkEsclusione");

                        // se trovo dei checkbox selezionati aggiungo la tipologia all'array list.
                        if (oCheck.Checked)
                        {
                            // Aggiungo
                            ArrListTipEsclusione.Add(oItem.Cells[1].Text.ToString());
                        }
                    }
                }
                return (string[])ArrListTipEsclusione.ToArray(typeof(string));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.GetTipologieDaEscludere.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;
            }
        }

        private string GetImpostazioniBollettino()
        {
            if (radioBollettiniSenzaImporti.Checked) return "BOLLETTINISENZAIMPORTI";
            if (radioNoBollettini.Checked) return "NOBOLLETTINI";
            //*** 20140509 - TASI ***
            if (radioSoloAcconto.Checked) return "SOLOACCONTO";
            if (radioSoloSaldo.Checked) return "SOLOSALDO";
            //*** ***
            return "BOLLETTINISTANDARD";
        }

        private void ApriPopUpDownloadDocumenti()
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

            strBuilder.Append("ApriPopUpStampaDocumenti();");
            RegisterScript(strBuilder.ToString(), this.GetType());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIndietro_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Request.Params["Provenienza"] != null)
                {
                    //*** 20150703 - INTERROGAZIONE GENERALE ***
                    if (Request.Params["Provenienza"] == "INTERGEN")
                    {
                        string sScript = "";
                        Type csType = this.GetType();
                        sScript = "";
                        sScript += "parent.Visualizza.location.href='../../Interrogazioni/DichEmesso.aspx?Ente=" + ConstWrapper.CodiceEnte + "';";
                        sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';";
                        sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                        sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIPuntuale.btnIndietro_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
    }
}
