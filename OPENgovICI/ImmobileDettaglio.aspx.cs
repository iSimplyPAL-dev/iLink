using System;
using System.Collections;
using System.Text;
using System.Configuration;
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
using AnagInterface;
using OggettiComuniStrade;
using RemotingInterfaceAnater;
using log4net;
using log4net.Config;
using System.IO;
using Anater.Oggetti;
using Utility;
using System.Data.SqlClient;

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina per la gestione dell'immobile.
    /// Le possibili opzioni sono:
    /// - Visualizza GIS
    /// - DOCFA
    /// - Precarica
    /// - Visualizza situazione Territorio
    /// - Duplica l'immobile
    /// - Abilita
    /// - Salva
    /// - Elimina
    /// - Torna alla videata precedente
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Analisi eventi</em>
    /// </revision>
    /// </revisionHistory>
    public partial class ImmobileDettaglio : BaseEnte
    {
        #region Variabili
        private static readonly ILog log = LogManager.GetLogger(typeof(ImmobileDettaglio));
        //public string ApplicazioneTerritorio = System.Configuration.ConfigurationManager.AppSettings["OPENGOVT"].ToString();
        #endregion
        #region Web Form Designer generated code
        /// 
        protected System.Web.UI.WebControls.RequiredFieldValidator rvalDataUltimaModifica;
        /// 
        protected System.Web.UI.WebControls.ValidationSummary valErrorSummary;
        /// 
        protected System.Web.UI.WebControls.CustomValidator cstValidaImpDetrazione;

        // aggiunti per il ribaltamento in anater
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
            GestComandi();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // *** 20140923 - GIS ***
            this.CmdGIS.Click += new System.EventHandler(this.CmdGIS_Click);
            //this.GrdTARSU.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdTARSU_ItemCommand);
            //*** ***
            this.ibNewTARSU.Click += new System.Web.UI.ImageClickEventHandler(this.ibNewTARSU_Click);
        }
        #endregion

        #region Property
        /// <summary>
        /// Ritorna o assegna l'id della testata/dichiarazione.
        /// </summary>
        public int IDTestata
        {
            get { return ViewState["IDTestata"] != null ? (int)ViewState["IDTestata"] : 0; }
            set { ViewState["IDTestata"] = value; }
        }

        /// <summary>
        /// Ritorna o assegna l'id dell'immobile.
        /// </summary>
        protected int IDImmobile
        {
            get { return ViewState["IDImmobile"] != null ? (int)ViewState["IDImmobile"] : 0; }
            set { ViewState["IDImmobile"] = value; }
        }

        /// <summary>
        /// Ritorna o assegna l'id del dettaglio testata.
        /// </summary>
        protected int IDDettaglioTestata
        {
            get { return ViewState["IDDettaglioTestata"] != null ? (int)ViewState["IDDettaglioTestata"] : 0; }
            set { ViewState["IDDettaglioTestata"] = value; }
        }
        //*** 20131003 - gestione atti compravendita ***
        /// <summary>
        /// 
        /// </summary>
        protected int CompraVenditaId
        {
            get { return ViewState["CompraVenditaId"] != null ? int.Parse(ViewState["CompraVenditaId"].ToString()) : 0; }
            set { ViewState["CompraVenditaId"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected DateTime CompraVenditaDataValidita
        {
            get { return ViewState["CompraVenditaDataValidita"] != null ? Convert.ToDateTime(ViewState["CompraVenditaDataValidita"].ToString()) : DateTime.Now; }
            set { ViewState["CompraVenditaDataValidita"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string TipoSoggettoInCompraVendita
        {
            get { return ViewState["TipoSoggettoInCompraVendita"] != null ? ViewState["TipoSoggettoInCompraVendita"].ToString() : ""; }
            set { ViewState["TipoSoggettoInCompraVendita"] = value; }
        }
        //*** ***
        //*** 20131018 - DOCFA ***
        /// <summary>
        /// 
        /// </summary>
        protected int DOCFAId
        {
            get { return ViewState["DOCFAId"] != null ? int.Parse(ViewState["DOCFAId"].ToString()) : 0; }
            set { ViewState["DOCFAId"] = value; }
        }
        //*** ***
        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
        /// <summary>
        /// 
        /// </summary>
        protected int iHasDummyDich
        {
            get
            {
                int nMyVal = 0;
                if (ConstWrapper.HasDummyDich)
                    nMyVal = 1;
                return nMyVal;
            }
        }
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        protected string UrlPopUpTerritorio
        {
            get
            {
                return (ConfigurationManager.AppSettings["UrlPopUpTerritorio"] != null ? ConfigurationManager.AppSettings["UrlPopUpTerritorio"].ToString() : string.Empty);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string UrlStradario
        {
            get { return ConstWrapper.UrlStradario; }
        }
        #endregion
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/03/2013">
        /// gestione dati da territorio
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="23/09/2013">
        /// gestione modifiche tributarie
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="03/10/2013">
        /// gestione atti compravendita
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="18/10/2013">
        /// DOCFA
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="07/2015">
        /// GESTIONE INCROCIATA RIFIUTI/ICI
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="10/05/2017">
        /// carico i dati da catasto e degli altri proprietari
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                log.Debug("ImmobileDettaglio::Page_Load::entro");
                if (TxtViaRibaltata.Text != "")
                {
                    txtViaDummy.Text = TxtViaRibaltata.Text;
                    txtViaNoDummy.Text = TxtViaRibaltata.Text;
                }

                string scriptComandi = string.Empty;
                if (!IsPostBack)
                {
                    this.IDTestata = Request.QueryString["IDTestata"] == null ? 0 : int.Parse(Request.QueryString["IDTestata"]);
                    this.IDImmobile = Request.QueryString["IDImmobile"] == null ? 0 : int.Parse(Request.QueryString["IDImmobile"]);
                    //salvo il typeoperation, che mi gestisce il pulsante di Back
                    hdTypeOperation.Value = Request["TYPEOPERATION"].ToString();
                    if (Request.QueryString["IdAttoCompraVendita"] != null)
                    {
                        this.CompraVenditaId = int.Parse(Request.QueryString["IdAttoCompraVendita"]);
                    }
                    else
                    {
                        AttoCompraVendita.Style.Add("display", "none");
                    }
                    if (Request.QueryString["IdDOCFA"] != null)
                    {
                        this.DOCFAId = int.Parse(Request.QueryString["IdDOCFA"]);
                    }
                    btnElimina.Attributes.Add("onclick", "javascript:return confirm('Confermi la cancellazione?');");
                    btnSalva.Attributes.Add("onclick", "return ControllaValore();");
                    btnDuplica.Attributes.Add("onclick", "return ControllaValore();");
                    lnkRenditaDummy.Attributes.Add("onclick", "return CalcolaTariffa();");
                    lnkRenditaNoDummy.Attributes.Add("onclick", "return CalcolaTariffa();");
                    lnkValoreDummy.Attributes.Add("onclick", "return CalcolaValoreImmobile();");
                    lnkValoreNoDummy.Attributes.Add("onclick", "return CalcolaValoreImmobile();");

                    LoadCombos();
                    txtCodiceComune.Text = ConstWrapper.CodiceEnte;

                    Session["TABELLA_ESTIMO_CATASTALE_FAB"] = new TariffeEstimoAFTable(ConstWrapper.sUsername).ListDistinct(ConstWrapper.CodiceEnte);
                    Session["TABELLA_ESTIMO_CATASTALE"] = new TariffeEstimoTable(ConstWrapper.sUsername).ListDistinct(ConstWrapper.CodiceEnte);

                    if (this.IDImmobile > 0 || this.CompraVenditaId > 0)
                    {
                        ControlsBind(this.IDImmobile, this.CompraVenditaId, false);
                        log.Debug("ImmobileDettaglio::page_load:: carico pagina comandi");
                        btnAggiungiContitolare.Enabled = true;
                        Abilita(false);
                    }
                    else
                    {
                        txtDataUltimaModifica.Text = DateTime.Now.ToShortDateString();
                        if (ConstWrapper.HasDummyDich)
                        {
                            txtDataInizioDummy.Text = DateTime.Now.ToShortDateString();
                        }
                        else
                        {
                            txtDataInizioNoDummy.Text = DateTime.Now.ToShortDateString();
                        }
                        txtCodPertinenza.Text = "-1";
                        Abilita(true);
                        // anche se non ho i dettagli dell'immobile carico ugualmente i dati del contribuente che sono associati alla testata
                        ContribuenteBind(-1, this.IDTestata);
                    }
                    log.Debug("registro le info x gestione modifiche tributarie");
                    SaveInitValues();
                    if (ConstWrapper.HasDummyDich)
                    {
                        if (txtFoglioDummy.Text != "")
                        {
                            DateTime tInizio, tFine;
                            tInizio = DateTime.MaxValue; tFine = DateTime.MaxValue;
                            if (txtDataInizioDummy.Text != "")
                                tInizio = DateTime.Parse(txtDataInizioDummy.Text);
                            if (txtDataFineDummy.Text != "")
                                tFine = DateTime.Parse(txtDataFineDummy.Text);
                            OggettiTable fncDatiTARSU = new OggettiTable(ConstWrapper.sUsername);
                            fncDatiTARSU.LoadDatiTARSU(ConstWrapper.StringConnection, ConstWrapper.CodiceEnte, txtFoglioDummy.Text, txtNumeroDummy.Text, txtSubalternoDummy.Text, tInizio, tFine, GrdTARSU, ibNewTARSU);
                            LoadAltriProprietari(ConstWrapper.CodiceEnte, txtFoglioDummy.Text, txtNumeroDummy.Text, txtSubalternoDummy.Text);
                            LoadDatiCatasto(ConstWrapper.CodBelfiore, txtFoglioDummy.Text, txtNumeroDummy.Text, txtSubalternoDummy.Text);
                        }
                    }
                }
                else if (txtIdTerUI.Value != "")
                {
                    log.Debug("carico da territorio");
                    ControlsBindFromTer(int.Parse(txtIdTerUI.Value), int.Parse(txtIdTerProprieta.Value), int.Parse(txtIdTerProprietario.Value), this.IDTestata);
                }
                if (Request.QueryString["IdAttoCompraVendita"] != null)
                {
                    this.CompraVenditaId = int.Parse(Request.QueryString["IdAttoCompraVendita"]);
                }
                else
                {
                    AttoCompraVendita.Style.Add("display", "none");
                }
                if (this.CompraVenditaId > 0)
                {
                    log.Debug("carico atto compravendita");
                    DataTable myData = new DichiarazioniView().GetCompraVendita(int.Parse(Request.QueryString["IdAttoCompraVendita"]));
                    foreach (DataRow myRow in myData.Rows)
                    {
                        lblNotaTrascrizione.Text = myRow["NotaTrascrizione"].ToString();
                        lblRifNota.Text = myRow["RifNota"].ToString();
                        lblCatNota.Text = myRow["CatNota"].ToString();
                        lblUbicazioneNota.Text = myRow["UbicazioneNota"].ToString();
                        lblUbicazioneCatasto.Text = myRow["UbicazioneCatasto"].ToString();
                        this.CompraVenditaDataValidita = DateTime.Parse(myRow["datavalidita"].ToString());
                    }
                    myData = new DichiarazioniView().GetCompraVenditaSoggetto(int.Parse(Request.QueryString["IdAttoCompraVendita"]), int.Parse(hdIdContribuente.Value), -1);
                    foreach (DataRow myRow in myData.Rows)
                    {
                        lblSoggettoNota.Text = myRow["NotaTitolarita"].ToString();
                        this.TipoSoggettoInCompraVendita = myRow["Tipo"].ToString();
                    }
                    AttoCompraVendita.Style.Add("display", "");
                }
                if (ConstWrapper.HasDummyDich)
                    scriptComandi = "document.getElementById('DummyDich').style.display = '';document.getElementById('NoDummyDich').style.display = 'none';";
                else
                    scriptComandi = "document.getElementById('DummyDich').style.display = 'none';document.getElementById('NoDummyDich').style.display = '';";
                if (ConstWrapper.HasPlainAnag)
                    scriptComandi += "document.getElementById('TRSpecAnag').style.display='none';";
                else
                    scriptComandi += "document.getElementById('TRPlainAnag').style.display='none';";
                RegisterScript(scriptComandi, this.GetType());
                log.Debug("ImmobileDettaglio::Page_Load::esco");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #region "Bottoni"
        //*** 20131018 - DOCFA ***
        /// <summary>
        /// Pulsante per la visualizzazione del DOCFA associato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdDOCFADet_Click(object sender, System.EventArgs e)
        {
            try
            {
                string sScript = "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';";
                sScript += "document.location.href='../20/DOCFA/DOCFA.aspx?ENTE=" + ConstWrapper.CodiceEnte + "&IdDOCFA=" + this.DOCFAId.ToString() + "&IdTestata=" + this.IDTestata.ToString() + "&IdImmobile=" + this.IDImmobile.ToString() + "';";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.cmdDOCFADet_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** ***
        /// <summary>
        /// Torna indietro alla pagina della testata.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIndietro_Click(object sender, System.EventArgs e)
        {
            try
            {
                log.Debug("ImmobileDettaglio::btnIndietro_Click::entro");
                //ApplicationHelper.LoadFrameworkPage("SR_IMMOBILI", "?IDTestata=" + this.IDTestata.ToString());
                //*** 20131003 - gestione atti compravendita ***
                if (this.CompraVenditaId > 0)
                {
                    string sScript = "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';";
                    sScript += "document.location.href='../20/AttiCompraVendita/CompraVenditaRicerca.aspx?ENTE=" + ConstWrapper.CodiceEnte + "&IdAtto=" + this.CompraVenditaId.ToString() + "';";
                    RegisterScript(sScript, this.GetType());
                }
                else if (StringOperation.FormatString(Request.Params["Sportello"]) == "1")
                {
                    string sScript = "parent.window.close();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    LoadBack();
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnIndietro_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// Elimina l'immobile dalla dichiarazione eliminando il dettaglio della testata.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnElimina_Click(object sender, System.EventArgs e)
        {
            try
            {
                int x = 0;
                new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_DELETE, new Utility.DichManagerICI.OggettiRow() { ID = this.IDImmobile }, -1, out x);
                new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestata(Utility.Costanti.AZIONE_DELETE, new Utility.DichManagerICI.DettaglioTestataRow() { ID = -1, IdOggetto = this.IDImmobile }, out x);
                new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Immobile, "btnElimina", Utility.Costanti.AZIONE_DELETE.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDImmobile);
                RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnElimina_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //protected void btnElimina_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        int x = 0;
        //        new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_DELETE, new Utility.DichManagerICI.OggettiRow() { ID = this.IDImmobile }, -1, out x);
        //        new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestata(Utility.Costanti.AZIONE_DELETE, new Utility.DichManagerICI.DettaglioTestataRow() { ID = -1, IdOggetto = this.IDImmobile }, out x);
        //        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnElimina_Click.errore: ", Err);
        //        Response.Redirect("../PaginaErrore.aspx");
        //    }
        //    /*if (new OggettiTable(ConstWrapper.sUsername).CancellazioneImmobile(this.IDImmobile))
        //    {
        //        if (new DettaglioTestataTable(ConstWrapper.sUsername).DeleteItem(this.IDImmobile, this.IDTestata))
        //        {
        //            //strscript = "alert('');";RegisterScript(sScript,this.GetType());, "err", strscript);this, "L'immobile è stato cancellato dalla dichiarazione.");
        //            btnIndietro_Click(sender, e);
        //        }
        //        else
        //        {
        //            strscript = "alert('');";RegisterScript(sScript,this.GetType());, "err", strscript);this, "Cancellazione fallita.");
        //        }
        //    }
        //    else
        //    {
        //        strscript = "alert('');";RegisterScript(sScript,this.GetType());, "err", strscript);this, "Cancellazione fallita.");
        //    }*/
        //}

        /// <summary>
        /// Esegue il salvataggio o la modifica dei dati dell'immobile e del dettaglio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnSalva_Click(object sender, System.EventArgs e)
        {
            bool retval = true;
            string strscript = "";
            int myIdUI = 0;
            try
            {
                myIdUI = this.IDImmobile;
                log.Debug("ImmobileDettaglio::btnSalva_Click::si inizia a salvare in tbloggetti");
                retval = SalvaModificaImmobile();
                if (!retval)
                {
                    log.Debug("ImmobileDettaglio::btnSalva_Click::ho dato errore");
                    strscript = "GestAlert('a', 'danger', '', '', 'Salvataggio non effettuato.');";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    log.Debug("ImmobileDettaglio::btnSalva_Click::vado a salvare in tbldettaglio");
                    retval = SalvaModificaDettaglio();
                    log.Debug("ImmobileDettaglio::btnSalva_Click::sono andato " + retval.ToString());
                    if (retval)
                    {
                        new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Immobile, "btnSalva", (myIdUI > 0 ? Utility.Costanti.AZIONE_UPDATE : Utility.Costanti.AZIONE_NEW).ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDImmobile);
                    }
                    strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Salvataggio" + (retval == true ? "" : " non") + " effettuato.');";
                    RegisterScript(strscript, this.GetType());
                    Abilita(!retval);
                    //*** 20131003 - gestione atti compravendita ***
                    if (this.CompraVenditaId > 0)
                    {
                        string sScript = "if (confirm('Desideri aggiornare lo stato del Soggetto?')){document.getElementById('cmdTrattatoSoggettoCompraVendita').click();}";
                        RegisterScript(sScript, this.GetType());
                    }
                    else if (StringOperation.FormatString(Request.Params["Sportello"]) == "1")
                    {
                        string sScript = "parent.window.close();";
                        RegisterScript(sScript, this.GetType());
                    }
                    //*** ***
                    else {
                        LoadBack();
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnSalva_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //protected void btnSalva_Click(object sender, System.EventArgs e)
        //{
        //    bool retval = true;
        //    string strscript = "";
        //    try
        //    {
        //        log.Debug("ImmobileDettaglio::btnSalva_Click::si inizia a salvare in tbloggetti");
        //        retval = SalvaModificaImmobile();
        //        if (!retval)
        //        {
        //            log.Debug("ImmobileDettaglio::btnSalva_Click::ho dato errore");
        //            strscript = "GestAlert('a', 'danger', '', '', 'Salvataggio non effettuato.');";
        //            RegisterScript(strscript, this.GetType());
        //        }
        //        else
        //        {
        //            log.Debug("ImmobileDettaglio::btnSalva_Click::vado a salvare in tbldettaglio");
        //            retval = SalvaModificaDettaglio();
        //            log.Debug("ImmobileDettaglio::btnSalva_Click::sono andato " + retval.ToString());
        //            strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Salvataggio" + (retval == true ? "" : " non") + " effettuato.');";
        //            RegisterScript(strscript, this.GetType());
        //            // Faccio una bonifica dell'immobile inserito per marcare gli errori
        //            bool RetValBonifica = false;

        //            //RetValBonifica = new ModuloIci(String.Empty).BonificaDichiarazione(this.IDTestata);
        //            try
        //            {
        //                RetValBonifica = new ModuloIci(String.Empty).BonificaImmobile(this.IDTestata, this.IDImmobile);
        //            }
        //            catch (Exception ErrBon)
        //            {
        //                log.Debug("Bonifica::errore::", ErrBon);
        //            }
        //            // RIBALTO LA DICHIARAZIONE IN ANATER
        //            if (ConstWrapper.UsoAnater == "true")
        //            {
        //                try
        //                {
        //                    Type typeofRI = typeof(IRemotingInterfaceICI);
        //                    IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

        //                    // DEVO RECUPERARE I DATI DEL CONTRIBUENTE
        //                    Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, this.IDImmobile, ConstWrapper.StringConnection);

        //                    DettaglioAnagrafica oDettAnagContrib = new DettaglioAnagrafica();

        //                    if (RigaTestata.IDContribuente != 0)
        //                    {
        //                        oDettAnagContrib = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDContribuente.ToString()));
        //                    }

        //                    int iRetValControlloAnagraficaAnater = 3;
        //                    //					iRetValControlloAnagraficaAnater= remObject.ControlloAnagraficaAnater (oDettAnagContrib,null,ConstWrapper.CodiceEnte);
        //                    //					//valori ritornati:
        //                    //					//0- contribuente trovato --> residente
        //                    //					//1- contribuente trovato --> non  residente --> dati non variati
        //                    //					//2- contribuente trovato --> non  residente --> dati variati
        //                    //					//3- contribuente non trovato --> nuovo inserimento
        //                    //
        //                    //					// controllo l'immobile
        //                    //					OggettiRow ImmobileAnater = new OggettiTable(ConstWrapper.sUsername).GetRow(this.IDImmobile);
        //                    //
        //                    //					TrasformatoreAnater TrasformaOggettiAnater = new TrasformatoreAnater();
        //                    //								
        //                    //					OggettiRowICI oImmobile = TrasformaOggettiAnater.TrasformaRigaOggetto(ImmobileAnater);
        //                    //
        //                    //					string ControlloDatiPreRibaltamento = remObject.ControlliCollegamentoImmobilePreRibaltamento(oImmobile, Business.ConstWrapper.CodiceEnte);
        //                    //
        //                    bool bAbbinamentoManuale = false;
        //                    bool bDatiCambiati = false;
        //                    //
        //                    //					if (ControlloDatiPreRibaltamento != string.Empty)
        //                    //					{
        //                    //						bAbbinamentoManuale = bool.Parse((ControlloDatiPreRibaltamento.Split('|'))[0].ToString());
        //                    //						bDatiCambiati = bool.Parse((ControlloDatiPreRibaltamento.Split('|'))[1].ToString());
        //                    //					}

        //                    // apertura del popup che richiede le opzioni per il ribaltamento di anater. 

        //                    strscript = "OpenPopUpRibaltamento(" + iRetValControlloAnagraficaAnater + ", " + bDatiCambiati.ToString().ToLower() + "," + bAbbinamentoManuale.ToString().ToLower() + " );";
        //                    RegisterScript(strscript, this.GetType());
        //                }
        //                catch (Exception Ex)
        //                {
        //                    log.Debug("Salvataggio in ANATER::errore::", Ex);
        //                }
        //            }
        //            Abilita(!retval);
        //            //*** 20131003 - gestione atti compravendita ***
        //            if (this.CompraVenditaId > 0)
        //            {
        //                string sScript = "if (confirm('Desideri aggiornare lo stato del Soggetto?')){document.Form1.cmdTrattatoSoggettoCompraVendita.click();}";
        //                RegisterScript(sScript, this.GetType());
        //            }
        //            //*** ***
        //            else {
        //                //if (!RetValBonifica)
        //                //{
        //                //    string strScript = "javascript:window.showModalDialog('PopUpErroriImmobile.aspx?IDTestata=" + this.IDTestata + "&IDOggetto=" + this.IDImmobile + "&IDDettaglioTestata=" + this.IDDettaglioTestata + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');";
        //                //    string aggiornacomandi = "parent.Comandi.location.href='cimmobiledettagliomod.aspx';";
        //                //    RegisterScript(sScript,this.GetType());, "", "" + strScript + aggiornacomandi + "");
        //                //}
        //                //else
        //                //{
        //                //    // VECCHIO RIBALTAMENTO IN ANATER, SI RIBALTAVA SOLO QUANDO L'IMMOBILE ERA BONIFICATO
        //                //}
        //                LoadBack();
        //            }


        //        }

        //        if (retval)
        //        {
        //            //				btnAggiungiContitolare.Enabled = true;
        //            //
        //            //				//string stringa ="";
        //            //				string stringa;
        //            //				stringa= "parent.Comandi.document.getElementById('Unlock').style.display = '';";
        //            //				stringa = stringa + "parent.Comandi.document.getElementById('Contitolari').style.display = ''";
        //            //				//RegisterScript(sScript,this.GetType());,"mes",stringa);
        //            //				RegisterScript(sScript,this.GetType());,"stri", "" + stringa + "");
        //        }

        //        //Abilita(!retval);
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnSalva_Click.errore: ", Err);
        //        Response.Redirect("../PaginaErrore.aspx");
        //    }
        //}

        /// <summary>
        /// Duplica dell'immobile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDuplica_Click(object sender, System.EventArgs e)
        {
            bool retval = true;
            bool RetValBonifica = false;
            string strscript = "";
            try
            {
                retval = DuplicaImmobile("");
                if (!retval)
                {
                    strscript = "GestAlert('a', 'warning', '', '', 'Non è stato possibile duplicare l'immobile.');";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    retval = SalvaModificaDettaglio();
                    strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Duplica Immobile" + (retval == true ? "" : " non") + " effettuata.');";
                    RegisterScript(strscript, this.GetType());

                    // Faccio una bonifica dell'immobile inserito per marcare gli errori
                    //RetValBonifica = new ModuloIci(String.Empty).BonificaDichiarazione(this.IDTestata);
                    RetValBonifica = new ModuloIci(String.Empty).BonificaImmobile(this.IDTestata, this.IDImmobile);
                    if (!RetValBonifica)
                    {
                        string strScript = "javascript:window.showModalDialog('PopUpErroriImmobile.aspx?IDTestata=" + this.IDTestata + "&IDOggetto=" + this.IDImmobile + "&IDDettaglioTestata=" + this.IDDettaglioTestata + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');";
                        strscript += "parent.Comandi.location.href='cimmobiledettagliomod.aspx';";
                        RegisterScript(strScript, this.GetType());
                    }
                    else
                    {
                        // VECCHIO RIBALTAMENTO IN ANATER, SI RIBALTAVA SOLO QUANDO L'IMMOBILE ERA BONIFICATO
                    }

                    // RIBALTO LA DICHIARAZIONE IN ANATER
                    if (ConstWrapper.UsoAnater == "true")
                    {
                        Type typeofRI = typeof(IRemotingInterfaceICI);
                        IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

                        // DEVO RECUPERARE I DATI DEL CONTRIBUENTE
                        Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, this.IDImmobile, ConstWrapper.StringConnection);
                        DettaglioAnagrafica oDettAnagContrib = new DettaglioAnagrafica();

                        if (RigaTestata.IDContribuente != 0)
                        {
                            oDettAnagContrib = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDContribuente.ToString()));
                        }

                        int iRetValControlloAnagraficaAnater;
                        iRetValControlloAnagraficaAnater = remObject.ControlloAnagraficaAnater(oDettAnagContrib, null, ConstWrapper.CodiceEnte);
                        //valori ritornati:
                        //0- contribuente trovato --> residente
                        //1- contribuente trovato --> non  residente --> dati non variati
                        //2- contribuente trovato --> non  residente --> dati variati
                        //3- contribuente non trovato --> nuovo inserimento				
                        strscript = "RibaltaInAnater(" + iRetValControlloAnagraficaAnater + ");";
                        RegisterScript(strscript, this.GetType());
                    }
                }

                if (retval)
                {
                    btnAggiungiContitolare.Enabled = true;

                    //string stringa ="";
                    string stringa;
                    stringa = "parent.Comandi.document.getElementById('Unlock').style.display = '';";
                    stringa = stringa + "parent.Comandi.document.getElementById('Contitolari').style.display = '';";
                    //RegisterScript(sScript,this.GetType());,"mes",stringa);
                    RegisterScript(stringa, this.GetType());
                }

                Abilita(!retval);

                if (this.IDImmobile > 0)
                {
                    strscript = "";

                    //				ControlsBind(this.IDImmobile, this.IDTestata);
                    //				btnAggiungiContitolare.Enabled = true;

                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + this.IDTestata.ToString() + "&IDImmobile=" + this.IDImmobile.ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());

                    strscript = "parent.Comandi.location.href='CImmobileDettaglioMod.aspx'";
                    RegisterScript(strscript, this.GetType());
                    //Abilita(true);
                }
                else
                {
                    strscript = "";
                    TextBox txtDataInizio = null;
                    if (ConstWrapper.HasDummyDich)
                        txtDataInizio = txtDataInizioDummy;
                    else
                        txtDataInizio = txtDataInizioNoDummy;

                    txtDataUltimaModifica.Text = DateTime.Now.ToShortDateString();
                    txtDataInizio.Text = DateTime.Now.ToShortDateString();
                    Abilita(true);

                    txtCodPertinenza.Text = "-1";

                    strscript = "parent.Comandi.location.href='CImmobileDettaglio.aspx';";
                    strscript = strscript + "parent.Comandi.document.getElementById('Contitolari').style.display='none';";
                    RegisterScript(strscript, this.GetType());

                    //string stringa= "parent.Comandi.document.getElementById('Contitolari').style.display = 'none';";
                    //RegisterScript(sScript,this.GetType());,"str", "" + stringa + "");

                    // anche se non ho i dettagli dell'immobile carico ugualmente i dati del contribuente che sono associati alla testata
                    ContribuenteBind(int.Parse(hdIdContribuente.Value), this.IDTestata);
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnDuplica_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVisualizzaDettagli_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.IDImmobile.CompareTo(0) != 0)
                {
                    string strScript = string.Empty;
                    strScript = "";
                    strScript += "ApriDatiAggiuntivi(" + this.IDImmobile + ")";
                    strScript += "";
                    RegisterScript(strScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnVisualizzaDettagli_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAggiungiContitolare_Click(object sender, System.EventArgs e)
        {
            StringBuilder strBuild = new StringBuilder();

            strBuild.Append("");
            strBuild.Append("parent.Visualizza.location.href='Contitolari.aspx?IDTestata=" + this.IDTestata.ToString() + "&IDOggetto=" + this.IDImmobile.ToString() + "';");
            strBuild.Append("parent.Comandi.location.href='cContitolari.aspx';");
            strBuild.Append("");
            RegisterScript(strBuild.ToString(), this.GetType());

            //ApplicationHelper.LoadFrameworkPage("SR_CONTITOLARI", "?IDTestata=" + this.IDTestata.ToString() + "&IDOggetto=" + this.IDImmobile.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRibaltaInAnater_Click(object sender, System.EventArgs e)
        {
            string strscript = "";
            try
            {

                string str;
                bool UpdateAnagAnater = false;
                bool UpdateAnagVerticale = false;
                str = txtUpdateAnagraficaValue.Text;

                if (str.ToUpper() == "FALSE")
                {
                    UpdateAnagAnater = false;
                }
                else if (str.ToUpper() == "TRUE")
                {
                    UpdateAnagAnater = true;
                }
                else if (str.ToUpper() == "TRUE VERTICALE")
                {
                    UpdateAnagVerticale = true;
                }

                TrasformatoreAnater TrasformaOggettiAnater = new TrasformatoreAnater();

                log.Debug("Carico i dati della testata con id= " + this.IDTestata);
                log.Error("Carico i dati della testata con id= " + this.IDTestata);
                Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, this.IDImmobile, ConstWrapper.StringConnection);

                 TestataRowICI oTestataANATER = new TestataRowICI();
                   TrasformatoreAnater TrasformaOggetti = new TrasformatoreAnater();

                oTestataANATER = TrasformaOggetti.TrasformaRigaTestata(RigaTestata);
    // SE LA DICHIARAZIONE RISULTA BONIFICATA RIBALTO IN ANATER
                // preparo l'oggetto testata
            
                DettaglioAnagrafica DatiContribuenteAnater;
                DettaglioAnagrafica DatiDenuncianteAnater;

                log.Debug("preparo l'oggetto DettaglioAnagrafica col contribuente");
                log.Error("preparo l'oggetto DettaglioAnagrafica col contribuente");

                // preparo l'oggetto DettaglioAnagrafica col contribuente
                if (RigaTestata.IDContribuente != 0)
                    DatiContribuenteAnater = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDContribuente.ToString()));
                else
                    DatiContribuenteAnater = new DettaglioAnagrafica();

                // preparo l'oggetto denunciante
                if (RigaTestata.IDDenunciante != 0)
                    DatiDenuncianteAnater = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDDenunciante.ToString()));
                else
                    DatiDenuncianteAnater = new DettaglioAnagrafica();

                ArrayList arrayListImmobileCompleto = new ArrayList();

                log.Debug("partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione");
                log.Error("partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione");

                // partendo dall'id testata prendo l'elenco di tutti gli immobili presenti nella dichiarazione
                // DataTable ImmobiliDichiarazione = new OggettiTable(ConstWrapper.sUsername).GetImmobileByIDTestata(this.IDTestata);

                // procedura nuova per il ribaltamento del singolo immobile.


                // foreach(DataRow RigaImmobile in ImmobiliDichiarazione.Rows)
                // {

                //	int IdImmobileAnater = (int)RigaImmobile["ID"];
                // prendo la riga dell'immobile
                Utility.DichManagerICI.OggettiRow ImmobileAnater = new OggettiTable(ConstWrapper.sUsername).GetRow(this.IDImmobile, ConstWrapper.StringConnection);

                Utility.DichManagerICI.DettaglioTestataRow DettaglioAnater = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(this.IDImmobile, RigaTestata.ID, false, ConstWrapper.StringConnection);

                OggettoImmobileCompleto OggettoImmobileAnater = new OggettoImmobileCompleto();

                OggettoImmobileAnater.oImmobile = TrasformaOggettiAnater.TrasformaRigaOggetto(ImmobileAnater);

                OggettoImmobileAnater.oImmobile.CodRicercaAnater = txtCodRicercaAnater.Text.ToString();

                OggettoImmobileAnater.oDettaglioTestata = TrasformaOggettiAnater.TrasformaRigaDettaglio(DettaglioAnater);

                // preparo l'oggetto contitolari

                log.Debug("preparo l'oggetto contitolari");

                DataTable DettaglioContitolari = new DettaglioTestataTable(ConstWrapper.sUsername).List(RigaTestata.ID, this.IDImmobile);

                int idDettaglioContitolare;
                int idSoggettoContitolare;

                ArrayList arrayListContitolari = new ArrayList();

                foreach (DataRow RigaContitolare in DettaglioContitolari.Rows)
                //for (int i=0; i < DettaglioContitolari.Rows.Count; i++)
                {
                    //idDettaglioContitolare = (int)DettaglioContitolari.Rows[i]["ID"];
                    //idSoggettoContitolare = (int)DettaglioContitolari.Rows[i]["idSoggetto"];
                    idDettaglioContitolare = (int)RigaContitolare["ID"];
                    idSoggettoContitolare = (int)RigaContitolare["idSoggetto"];

                    // prendo il record dettagliorow
                    Utility.DichManagerICI.DettaglioTestataRow oTestataContitolare = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(idDettaglioContitolare);
                    DettaglioAnagrafica oAnagrafeContitolare = HelperAnagrafica.GetDatiPersona(long.Parse(idSoggettoContitolare.ToString()));

                    OggettoContitolare oContitolare = new OggettoContitolare();

                    oContitolare.objDettaglioTestataContitolare = TrasformaOggettiAnater.TrasformaRigaDettaglio(oTestataContitolare);
                    oContitolare.objDettaglioAnagraficaContitolare = oAnagrafeContitolare;

                    arrayListContitolari.Add(oContitolare);
                }

                OggettoContitolare[] oContitolareAnater = (OggettoContitolare[])arrayListContitolari.ToArray(typeof(OggettoContitolare));

                OggettoImmobileAnater.ArrayObjContitolare = oContitolareAnater;

                arrayListImmobileCompleto.Add(OggettoImmobileAnater);
                // }

                log.Debug("carico l'oggetto OggettoImmobileCompleto");

                OggettoImmobileCompleto[] ArrayImmobiliCompleto = (OggettoImmobileCompleto[])arrayListImmobileCompleto.ToArray(typeof(OggettoImmobileCompleto));

                try
                {
                    bool iRetValRibaltaAnater;
                    Type typeofRI = typeof(IRemotingInterfaceICI);
                    IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

                    log.Debug("chiamo il metodo RibaltaInAnater");

                    iRetValRibaltaAnater = remObject.RibaltaInAnater(DatiContribuenteAnater, DatiDenuncianteAnater, TrasformaOggettiAnater.TrasformaRigaTestata(RigaTestata), ArrayImmobiliCompleto, ConstWrapper.CodiceEnte, UpdateAnagAnater, UpdateAnagVerticale, int.Parse(Session["COD_OPERATORE"].ToString()));// TrasformaRigaDettaglio(DettaglioAnater), TrasformaRigaOggetto(ImmobileAnater), oContitolareAnater , ConstWrapper.CodiceEnte);	

                    strscript = "GestAlert('a', '" + (iRetValRibaltaAnater == true ? "success" : "warning") + "', '', '', 'Ribaltamento in Anater" + (iRetValRibaltaAnater == true ? "" : " non") + " effettuato.');";
                    RegisterScript(strscript, this.GetType());

                    // prova collegamento tarsu
                    /*Type typeofRI1 = typeof(IRemotingInterfaceTARSU);
                    IRemotingInterfaceTARSU remObject1 = (IRemotingInterfaceTARSU)Activator.GetObject(typeofRI1, System.Configuration.ConfigurationManager.AppSettings["URLanaterTARSU"].ToString());

                    DettaglioAnagrafica objDetta = new DettaglioAnagrafica();

                    remObject1.RibaltaInAnater(objDetta, null, null, "", false, false, 0);
*/
                }
                catch (Exception ex)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnVisualizzaDettagli_Click.errore: ", ex);
                    log.Debug("Si sono verificati dei problemi durante il Ribaltamento in Anater." + ex.StackTrace);
                    strscript = "GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante il Ribaltamento in Anater.');";
                    RegisterScript(strscript, this.GetType());
                }
            }
            catch (Exception exception)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnVisualizzaDettagli_Click.errore: ", exception);
                log.Debug("Si sono verificati dei problemi durante l'elaborazione dati per il Ribaltamento in Anater." + exception.Message);
                strscript = "GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante l'elaborazione dati per il Ribaltamento in Anater.');";
                RegisterScript(strscript, this.GetType());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoogleMaps_Click(object sender, System.EventArgs e)
        {
            string sVia, sCivico,  sEnte, sAddress;
            StringBuilder strBuild = new StringBuilder();
            try
            {
                strBuild.Append("");
                if (ConstWrapper.HasDummyDich)
                {
                    sVia = txtViaDummy.Text;
                    sCivico = txtNumCivDummy.Text;
                }
                else
                {
                    sVia = txtViaNoDummy.Text;
                    sCivico = txtNumCivNoDummy.Text;
                }
                if (sVia != "")
                {
                    if (sCivico == "0") sCivico = "";

                    sVia = sVia.Replace("FRAZ.", "Frazione");
                    sVia = sVia.Replace("Fraz.", "Frazione");
                    sVia = sVia.Replace("fraz.", "Frazione");

                    sVia = sVia.Replace("LOC.", "Località");
                    sVia = sVia.Replace("Loc.", "Località");
                    sVia = sVia.Replace("loc.", "Località");

                    sEnte = ConstWrapper.DescrizioneEnte;
                    sEnte = sEnte.Replace("comune di ", "");
                    sEnte = sEnte.Replace("Comune di ", "");

                    sAddress = sVia + " " + sCivico + ", " + sEnte;

                    strBuild.Append("visualizzaImmobile('" + sAddress + "')");
                }
                else
                {
                    strBuild.Append("GestAlert('a', 'warning', '', '', 'Selezionare una via dallo stradario')");
                }
                RegisterScript(strBuild.ToString(), this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnGoogleMaps_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        //*** 20131003 - gestione atti compravendita ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdPrecarica_Click(object sender, System.EventArgs e)
        {
            string strscript = "";
            bool retval = true;
            TextBox txtDataInizio = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    txtDataInizio = txtDataInizioDummy;
                }
                else
                {
                    txtDataInizio = txtDataInizioNoDummy;
                }

                if (Request.QueryString["titolo"] != "C")
                {
                    retval = DuplicaImmobile(this.CompraVenditaDataValidita.ToString());
                    if (!retval)
                    {
                        strscript = "GestAlert('a', 'danger', '', '', 'Non è stato possibile duplicare l'immobile.');";
                        RegisterScript(strscript, this.GetType());
                    }
                    else
                    {
                        retval = SalvaModificaDettaglio();
                        strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Duplica Immobile" + (retval == true ? "" : " non") + " effettuata.');";
                        RegisterScript(strscript, this.GetType());

                        // RIBALTO LA DICHIARAZIONE IN ANATER
                        if (ConstWrapper.UsoAnater == "true")
                        {
                            Type typeofRI = typeof(IRemotingInterfaceICI);
                            IRemotingInterfaceICI remObject = (IRemotingInterfaceICI)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLanaterICI"].ToString());

                            // DEVO RECUPERARE I DATI DEL CONTRIBUENTE
                            Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, this.IDImmobile, ConstWrapper.StringConnection);

                            DettaglioAnagrafica oDettAnagContrib = new DettaglioAnagrafica();

                            if (RigaTestata.IDContribuente != 0)
                            {
                                oDettAnagContrib = HelperAnagrafica.GetDatiPersona(long.Parse(RigaTestata.IDContribuente.ToString()));
                            }

                            int iRetValControlloAnagraficaAnater;
                            iRetValControlloAnagraficaAnater = remObject.ControlloAnagraficaAnater(oDettAnagContrib, null, ConstWrapper.CodiceEnte);
                            //valori ritornati:
                            //0- contribuente trovato --> residente
                            //1- contribuente trovato --> non  residente --> dati non variati
                            //2- contribuente trovato --> non  residente --> dati variati
                            //3- contribuente non trovato --> nuovo inserimento
                            strscript = "RibaltaInAnater(" + iRetValControlloAnagraficaAnater + ");";
                            RegisterScript(strscript, this.GetType());
                        }

                        txtDataUltimaModifica.Text = DateTime.Now.ToShortDateString();
                        txtDataInizio.Text = DateTime.Now.ToShortDateString();
                        Abilita(true);

                        txtCodPertinenza.Text = "-1";

                        //carico i dati dall'atto
                        AttoCompraVenditaBind(this.CompraVenditaId, true);
                    }
                }
                else
                {
                    txtDataUltimaModifica.Text = DateTime.Now.ToShortDateString();
                    Abilita(true);

                    txtCodPertinenza.Text = "-1";

                    //carico i dati dall'atto
                    AttoCompraVenditaBind(this.CompraVenditaId, true);
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.cmdPrecarica_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdTrattatoSoggettoCompraVendita_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool retval = true;
                log.Debug("cmdTrattatoSoggettoCompraVendita::devo settare trattato::CompraVenditaId::" + this.CompraVenditaId.ToString() + "::IDContrib::" + hdIdContribuente.Value + "::TipoSoggettoInCompraVendita::" + this.TipoSoggettoInCompraVendita);
                retval = new DichiarazioniView().SetCompraVenditaSoggetto(this.CompraVenditaId, int.Parse(hdIdContribuente.Value), this.TipoSoggettoInCompraVendita);
                if (!retval)
                {
                    string strscript = "GestAlert('a', 'danger', '', '', 'Errore in settaggio soggetto di compra vendita trattato.');";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    string sScript = "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';";
                    sScript += "document.location.href='../20/AttiCompraVendita/CompraVenditaRicerca.aspx?ENTE=" + ConstWrapper.CodiceEnte + "&IdAtto=" + this.CompraVenditaId.ToString() + "';";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.cmdTrattatoSoggettoCompraVendita_Click.errore: ", err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** ***
        // *** 20140923 - GIS ***
        private void CmdGIS_Click(object sender, System.EventArgs e)
        {
            string CodeGIS;
            string sScript;
            RemotingInterfaceAnater.GIS fncGIS = new RemotingInterfaceAnater.GIS();
            System.Collections.Generic.List<RicercaUnitaImmobiliareAnater> listRifCat = new System.Collections.Generic.List<RicercaUnitaImmobiliareAnater>();
            RicercaUnitaImmobiliareAnater myRifCat = new RicercaUnitaImmobiliareAnater();
            TextBox txtFoglio = null;
            TextBox txtNumero = null;
            TextBox txtSubalterno = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    txtFoglio = txtFoglioDummy;
                    txtNumero = txtNumeroDummy; txtSubalterno = txtSubalternoDummy;
                }
                else
                {
                    txtFoglio = txtFoglioNoDummy; txtNumero = txtNumeroNoDummy;
                    txtSubalterno = txtSubalternoNoDummy;
                }
                if (txtFoglio.Text != "")
                {
                    myRifCat = new RicercaUnitaImmobiliareAnater();
                    myRifCat.Foglio = txtFoglio.Text;
                    myRifCat.Mappale = txtNumero.Text;
                    myRifCat.Subalterno = txtSubalterno.Text;
                    myRifCat.CodiceRicerca = ConstWrapper.CodBelfiore;
                    listRifCat.Add(myRifCat);
                }
                if ((listRifCat.ToArray().Length > 0))
                {
                    CodeGIS = fncGIS.getGIS(ConstWrapper.UrlWSGIS, listRifCat.ToArray());
                    if (!(CodeGIS == null))
                    {
                        sScript = "window.open(\'" + ConstWrapper.UrlWebGIS + CodeGIS + "\')";
                        RegisterScript(sScript, this.GetType());
                    }
                    else
                    {
                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in interrogazione Cartografia!');";
                        RegisterScript(sScript, this.GetType());
                    }
                }
                else
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.CmdGIS_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** ***
        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
        private void ibNewTARSU_Click(object sender, System.EventArgs e)
        {
            try
            {
                RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata oMyTestata = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata();
                RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTessera oMyTessera = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTessera();
                RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata oMyDettaglioTestata = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata();
                RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti oMyVano = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti();
                ArrayList myList = new ArrayList();
                ArrayList myListTessere = new ArrayList();
                TextBox TxtVia = null;
                TextBox TxtCivico = null;
                TextBox TxtEsponente = null;
                TextBox TxtInterno = null;
                TextBox TxtScala = null;
                TextBox TxtDataInizio = null;
                TextBox TxtDataFine = null;
                TextBox TxtFoglio = null;
                TextBox TxtNumero = null;
                TextBox TxtSubalterno = null;
                Label lblViaOld = null;

                if (ConstWrapper.HasDummyDich)
                {
                    TxtVia = txtViaDummy;
                    TxtCivico = txtNumCivDummy;
                    TxtEsponente = txtEspCivicoDummy;
                    TxtInterno = txtInternoDummy;
                    TxtScala = txtScalaDummy;
                    TxtDataInizio = txtDataInizioDummy;
                    TxtDataFine = txtDataFineDummy;
                    TxtFoglio = txtFoglioDummy;
                    TxtNumero = txtNumeroDummy;
                    TxtSubalterno = txtSubalternoDummy;
                    lblViaOld = lblViaOldDummy;
                }
                else
                {
                    TxtVia = txtViaNoDummy;
                    TxtCivico = txtNumCivNoDummy;
                    TxtEsponente = txtEspCivicoNoDummy;
                    TxtInterno = txtInternoNoDummy;
                    TxtScala = txtScalaNoDummy;
                    TxtDataInizio = txtDataInizioNoDummy;
                    TxtDataFine = txtDataFineNoDummy;
                    TxtFoglio = txtFoglioNoDummy;
                    TxtNumero = txtNumeroNoDummy;
                    TxtSubalterno = txtSubalternoNoDummy;
                    lblViaOld = lblViaOldNoDummy;
                }

                //carico i dati
                oMyVano.IdCategoria = oMyVano.sCategoria = oMyVano.IdTipoVano = "";
                oMyVano.IdCatTARES = oMyVano.nNC = oMyVano.nNCPV = 0;
                oMyVano.nMq = 0;
                oMyVano.bForzaCalcolaPV = oMyVano.bIsEsente = false;
                oMyVano.nVani = 1;
                oMyVano.IdOggetto = oMyVano.IdDettaglioTestata = -1;
                oMyVano.sProvenienza = "FITTIZIA";
                oMyVano.tDataInserimento = DateTime.Now;
                oMyVano.tDataVariazione = oMyVano.tDataCessazione = DateTime.MinValue;
                oMyVano.sOperatore = ConstWrapper.sUsername;
                myList = new ArrayList();
                myList.Add(oMyVano);

                oMyDettaglioTestata.Id = oMyDettaglioTestata.IdDettaglioTestata = oMyDettaglioTestata.IdTessera = oMyDettaglioTestata.IdTestata = oMyDettaglioTestata.IdPadre = -1;
                oMyDettaglioTestata.sCodVia = txtCodVia.Text;
                if (TxtVia.Text != "")
                    oMyDettaglioTestata.sVia = TxtVia.Text;
                else
                    oMyDettaglioTestata.sVia = lblViaOld.Text.Replace("(", "").Replace(")", "");
                oMyDettaglioTestata.sCivico = TxtCivico.Text;
                oMyDettaglioTestata.sEsponente = TxtEsponente.Text;
                oMyDettaglioTestata.sInterno = TxtInterno.Text;
                oMyDettaglioTestata.sScala = TxtScala.Text;
                oMyDettaglioTestata.tDataInizio = Convert.ToDateTime(TxtDataInizio.Text);
                if (TxtDataFine.Text.CompareTo("") != 0)
                    if (Convert.ToDateTime(TxtDataFine.Text) == DateTime.MaxValue)
                        oMyDettaglioTestata.tDataFine = Convert.ToDateTime(TxtDataFine.Text);
                oMyDettaglioTestata.nMQ = oMyDettaglioTestata.nVani = 1;
                oMyDettaglioTestata.sFoglio = TxtFoglio.Text;
                oMyDettaglioTestata.sNumero = TxtNumero.Text;
                oMyDettaglioTestata.sSubalterno = TxtSubalterno.Text;
                oMyDettaglioTestata.tDataInserimento = DateTime.Now;
                oMyDettaglioTestata.tDataCessazione = DateTime.MinValue;
                oMyDettaglioTestata.sOperatore = ConstWrapper.sUsername;
                oMyDettaglioTestata.oOggetti = (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti[])myList.ToArray(typeof(RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti));
                myList = new ArrayList();
                myList.Add(oMyDettaglioTestata);

                oMyTestata.Id = oMyTestata.IdTestata = -1;
                oMyTestata.sEnte = ConstWrapper.CodiceEnte;
                oMyTestata.IdContribuente = int.Parse(hdIdContribuente.Value);
                oMyTestata.tDataDichiarazione = DateTime.Now;
                oMyTestata.sNDichiarazione = "";
                oMyTestata.sIdProvenienza = Utility.DichManagerICI.Dichiarazione_FITTIZIA.ToString();
                oMyTestata.tDataInserimento = DateTime.Now;
                oMyTestata.tDataVariazione = oMyTestata.tDataCessazione = DateTime.MinValue;
                oMyTestata.oFamiglia = null;
                oMyTestata.oAnagrafe = null;
                oMyTestata.sOperatore = ConstWrapper.sUsername;
                oMyTestata.oImmobili = (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata[])myList.ToArray(typeof(RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata));

                if (ConstWrapper.IsFromVariabile == "1")
                {
                    oMyTessera.Id = oMyTessera.IdTessera = oMyTessera.IdTestata = -1;
                    oMyTessera.IdEnte = ConstWrapper.CodiceEnte;
                    oMyTessera.sCodUtente = oMyTessera.sCodInterno = oMyTessera.sNote = "";
                    oMyTessera.sNumeroTessera = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTessera.TESSERA_BIDONE;
                    oMyTessera.tDataRilascio = oMyTessera.tDataInserimento = DateTime.Now;
                    oMyTessera.tDataCessazione = DateTime.MinValue;
                    oMyTessera.oRiduzioni = oMyTessera.oDetassazioni = null;
                    oMyTessera.sOperatore = ConstWrapper.sUsername;
                    oMyTessera.oImmobili = (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata[])myList.ToArray(typeof(RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata));
                    myListTessere.Add(oMyTessera);
                    oMyTestata.oTessere = (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTessera[])myListTessere.ToArray(typeof(RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTessera));
                }
                log.Debug("string connection=" + ConstWrapper.StringConnectionTARSU);
                if (new DichManagerTARSU(ConstWrapper.DBType, ConstWrapper.StringConnectionTARSU, ConstWrapper.StringConnectionOPENgov, ConstWrapper.CodiceEnte).SetDichiarazione(oMyTestata, ConstWrapper.IsFromVariabile) <= 0)
                    log.Debug("ImmobileDettaglio::ibNewTARSU_Click:: errore in inserimento dichiarazione");
                else
                {
                    //*** 20130923 - gestione modifiche tributarie ***
                    foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata item in oMyTestata.oImmobili)
                    {
                        if (new Utility.ModificheTributarie().SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.NuovaDichiarazione, item.sFoglio, item.sNumero, item.sSubalterno, DateTime.Now, ConstWrapper.sUsername.ToString(), item.Id, DateTime.MaxValue) == false)
                            log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.NuovaDichiarazione + "::@FOGLIO=" + item.sFoglio + "::@NUMERO=" + item.sNumero + "::@SUBALTERNO=" + item.sSubalterno + "::@DATAVARIAZIONE=" + DateTime.Now + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + item.Id + "::@DATATRATTATO=" + DateTime.MaxValue);
                    }
                    //*** ***
                    if (txtFoglioDummy.Text != "")
                    {
                        DateTime tInizio, tFine;
                        tInizio = DateTime.MaxValue; tFine = DateTime.MaxValue;
                        if (txtDataInizioDummy.Text != "")
                            tInizio = DateTime.Parse(txtDataInizioDummy.Text);
                        if (txtDataFineDummy.Text != "")
                            tFine = DateTime.Parse(txtDataFineDummy.Text);
                        OggettiTable fncDatiTARSU = new OggettiTable(ConstWrapper.sUsername);
                        fncDatiTARSU.LoadDatiTARSU(ConstWrapper.StringConnection, ConstWrapper.CodiceEnte, txtFoglioDummy.Text, txtNumeroDummy.Text, txtSubalternoDummy.Text, tInizio, tFine, GrdTARSU, ibNewTARSU);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ibNewTARSU_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** ***
        /// <summary>
        /// Abilita/disabilita i controlli.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAbilita_Click(object sender, System.EventArgs e)
        {
            bool IsEnabled = false;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    if (!txtDataInizioDummy.Enabled)
                    {
                        IsEnabled = true;
                    }
                    else
                    {
                        IsEnabled = false;
                    }
                }
                else
                {
                    if (!txtDataInizioNoDummy.Enabled)
                    {
                        IsEnabled = true;
                    }
                    else
                    {
                        IsEnabled = false;
                    }
                }
                Abilita(IsEnabled);
                string sScript = string.Empty;
                System.Collections.Generic.List<string> oListCmd = new System.Collections.Generic.List<string>();
                oListCmd.Add("Unlock");
                foreach (string myItem in oListCmd)
                {
                    sScript += "$('#" + myItem + "').addClass('hidden');";
                }
                oListCmd = new System.Collections.Generic.List<string>();
                oListCmd.Add("Insert");
                oListCmd.Add("Delete");
                foreach (string myItem in oListCmd)
                {
                    sScript += "$('#" + myItem + "').removeClass('hidden');";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.btnAbilita_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #endregion

        private void lbtnCodiceComune_Click(object sender, System.EventArgs e)
        {
            txtCodiceComune.Enabled = !txtCodiceComune.Enabled;
        }

        /// <summary>
        /// Quando seleziono la caratteristica.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCaratteristica_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DropDownList myDdlRendita = null;
            DropDownList myDdlCarat = null;
            DropDownList ddlEstimo = null;
            CheckBox chkValoreProvvisorio = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    myDdlRendita = ddlCodiceRenditaDummy;
                    myDdlCarat = ddlCaratteristicaDummy;
                    ddlEstimo = ddlEstimoDummy;
                    chkValoreProvvisorio = chkValoreProvvisorioDummy;
                }
                else
                {
                    myDdlRendita = ddlCodiceRenditaNoDummy;
                    myDdlCarat = ddlCaratteristicaNoDummy;
                    ddlEstimo = ddlEstimoNoDummy;
                    chkValoreProvvisorio = chkValoreProvvisorioNoDummy;
                }
                // azzero i valori selezionati dalla ddlCodiceRendita
                log.Debug("ddlCaratteristica_SelectedIndexChanged::devo caricare ddlCodiceRendita::");
                myDdlRendita.SelectedItem.Selected = false;
                PopolaDDlEstimo(ddlEstimo, "");
                ListItem SelezionaItem;
                switch (myDdlCarat.SelectedValue)
                {
                    case "1":
                        SelezionaItem = myDdlRendita.Items.FindByText("TA");
                        myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                        break;
                    case "2":
                        SelezionaItem = myDdlRendita.Items.FindByText("AF");
                        myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                        PopolaDDlEstimo(ddlEstimo, "AF");
                        break;
                    case "3":
                        if (chkValoreProvvisorio.Checked)
                        {
                            SelezionaItem = myDdlRendita.Items.FindByText("RP");
                            myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                        }
                        else
                        {
                            SelezionaItem = myDdlRendita.Items.FindByText("RE");
                            myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                        }
                        break;
                    case "4":
                        SelezionaItem = myDdlRendita.Items.FindByText("LC");
                        myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ddlCaratteristica_SelectedIndexChanged.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCodiceRendita_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                DropDownList myDDL = ddlEstimoDummy;
                if (ConstWrapper.HasDummyDich)
                    myDDL = ddlEstimoDummy;
                else
                    myDDL = ddlEstimoNoDummy;
                if (myDDL.SelectedItem.Text.CompareTo("AF") == 0)
                {
                    // PASSO IL PARAMETRO AF
                    PopolaDDlEstimo(myDDL, "AF");
                }
                else
                {
                    //TUTTI GLI ALTRI CASI
                    PopolaDDlEstimo(myDDL, "");
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ddlCodiceRendita_SelectedIndexChanged.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkValoreProvvisorio_CheckedChanged(object sender, System.EventArgs e)
        {
            // azzero i valori selezionati dalla ddlCodiceRendita
            ListItem SelezionaItem;
            DropDownList myDdlCarat = ddlCaratteristicaDummy;
            DropDownList myDdlRendita = ddlCodiceRenditaDummy;
            CheckBox myChkProv = chkValoreProvvisorioDummy;

            log.Debug("chkValoreProvvisorio_CheckedChanged::devo caricare ddlCodiceRendita::");
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    myDdlCarat = ddlCaratteristicaDummy;
                    myDdlRendita = ddlCodiceRenditaDummy;
                    myChkProv = chkValoreProvvisorioDummy;
                }
                else
                {
                    myDdlCarat = ddlCaratteristicaNoDummy;
                    myDdlRendita = ddlCodiceRenditaNoDummy;
                    myChkProv = chkValoreProvvisorioNoDummy;
                }
                if (myDdlCarat.SelectedValue == "3")
                {
                    if (myChkProv.Checked)
                    {
                        SelezionaItem = myDdlRendita.Items.FindByText("RP");
                        myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                    }
                    else
                    {
                        SelezionaItem = myDdlRendita.Items.FindByText("RE");
                        myDdlRendita.SelectedIndex = myDdlRendita.Items.IndexOf(SelezionaItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.chkValoreProvvisorio_Checked.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAbitprincipale_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox myChkAbiPrinc = null;
            CheckBox myChkPertinenza = null;
            LinkButton myLnkPertinenza = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    myChkAbiPrinc = chkAbitPrincipaleDummy;
                    myChkPertinenza = chkPertinenzaDummy;
                    myLnkPertinenza = lnkApriPertinenzaDummy;
                }
                else
                {
                    myChkAbiPrinc = chkAbitprincipaleNoDummy;
                    myChkPertinenza = chkPertinenzaNoDummy;
                    myLnkPertinenza = lnkApriPertinenzaNoDummy;
                }

                if (myChkAbiPrinc.Checked)
                {
                    myChkPertinenza.Checked = false;
                    myChkPertinenza.Enabled = false;
                    myLnkPertinenza.Enabled = false;
                }
                else
                {
                    myLnkPertinenza.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.chkAbitprincipale_CheckedChanged.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkApriPertinenza_Click(object sender, System.EventArgs e)
        {
            RegisterScript("ApriPertinenza();", this.GetType());
        }
        /// <summary>
        /// Serve per il calcolo della rendita immobile. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkRendita_Click(object sender, System.EventArgs e)
        {
            DropDownList myDDL = null;
            DropDownList ddlCategoriaCatastale = null;
            DropDownList ddlClasse = null;
            TextBox myTxt = null;
            DropDownList ddlEstimo = null;
            DataTable TabTariffa = null;
            DateTime myInizio = DateTime.MaxValue;
            decimal Tariffa = 0;
            decimal Rendita = 0;
            decimal Consistenza = 0;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    myDDL = ddlCodiceRenditaDummy;
                    ddlCategoriaCatastale = ddlCategoriaCatastaleDummy;
                    ddlClasse = ddlClasseDummy;
                    myTxt = txtRenditaDummy;
                    ddlEstimo = ddlEstimoDummy;
                    myInizio = DateTime.Parse(txtDataInizioDummy.Text);
                    Consistenza = decimal.Parse(txtConsistenzaDummy.Text);
                }
                else
                {
                    myDDL = ddlCodiceRenditaNoDummy;
                    ddlCategoriaCatastale = ddlCategoriaCatastaleNoDummy;
                    ddlClasse = ddlClasseNoDummy;
                    myTxt = txtRenditaNoDummy;
                    ddlEstimo = ddlEstimoNoDummy;
                    myInizio = DateTime.Parse(txtDataInizioNoDummy.Text);
                    Consistenza = decimal.Parse(txtConsistenzaNoDummy.Text);
                }
                // controllo il tipo di immobile
                if (myDDL.SelectedItem.Text.CompareTo("AF") == 0)
                    // devo vedere se c'è qualche tariffa configurata per la zona selezionata
                    TabTariffa = new TariffeEstimoAFTable(ConstWrapper.sUsername).SelectTariffa(ConstWrapper.CodiceEnte, ddlEstimo.SelectedValue, myInizio);
                else
                    // devo vedere se c'è qualche tariffa configurata per la zona, la classe e la categoria selezionata
                    TabTariffa = new TariffeEstimoTable(ConstWrapper.sUsername).SelectTariffa(ConstWrapper.CodiceEnte, ddlEstimo.SelectedValue, ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue, myInizio);
                if (TabTariffa != null)
                {
                    if (TabTariffa.Rows.Count < 1)
                    {
                        // se nn ci sono tariffe configurate
                        StringBuilder strBuild = new StringBuilder();
                        strBuild.Append("");
                        strBuild.Append("GestAlert('a', 'warning', '', '', 'Attenzione, non ci sono tariffe configurate per la zona selezionata!');");
                        strBuild.Append("");
                        RegisterScript(strBuild.ToString(), this.GetType());
                    }
                    else
                    {
                        Tariffa = decimal.Parse(TabTariffa.Rows[0]["TARIFFA_EURO"].ToString());
                        // calcolo la rendita con i dati che possiedo
                        Rendita = Consistenza * Tariffa;
                        myTxt.Text = Rendita.ToString("N");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.InkRendita_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkValore_Click(object sender, System.EventArgs e)
        {
            try
            {                
                //*** 20120530 - IMU ***
                ComPlusInterface.FncICI FncValore = new ComPlusInterface.FncICI();
                int nAnno = DateTime.Now.Year;
                DateTime dDal = DateTime.Now;
                string sTipoRendita = "";
                string sCategoria = "";
                string sClasse = "";
                string sZona = "";
                double nRendita = 0;
                double nConsistenza = 0;
                double nValoreDich = 0;
                bool IsColtivatore = false;
                TextBox myTxtCons = null;
                TextBox myTxtVal = null;

                if (ConstWrapper.HasDummyDich)
                {
                    myTxtCons = txtConsistenzaDummy;
                    myTxtVal = txtValoreDummy;
                    IsColtivatore = chkColtivatoriDummy.Checked;
                    nAnno = DateTime.Parse(txtDataInizioDummy.Text).Year;
                    dDal = DateTime.Parse(txtDataInizioDummy.Text);
                    sTipoRendita = ddlCodiceRenditaDummy.SelectedItem.Text;
                    sCategoria = ddlCategoriaCatastaleDummy.SelectedItem.Text;
                    sClasse = ddlClasseDummy.SelectedValue;
                    sZona = ddlEstimoDummy.SelectedValue;
                    nRendita = double.Parse(txtRenditaDummy.Text);
                }
                else
                {
                    myTxtCons = txtConsistenzaNoDummy;
                    myTxtVal = txtValoreNoDummy;
                    IsColtivatore = chkcoltivatoriNoDummy.Checked;
                    nAnno = DateTime.Parse(txtDataInizioNoDummy.Text).Year;
                    dDal = DateTime.Parse(txtDataInizioNoDummy.Text);
                    sTipoRendita = ddlCodiceRenditaNoDummy.SelectedItem.Text;
                    sCategoria = ddlCategoriaCatastaleNoDummy.SelectedItem.Text;
                    sClasse = ddlClasseNoDummy.SelectedValue;
                    sZona = ddlEstimoNoDummy.SelectedValue;
                    nRendita = double.Parse(txtRenditaNoDummy.Text);
                }

                if (myTxtCons.Text != string.Empty)
                    nConsistenza = double.Parse(myTxtCons.Text);

                //*** 20120709 - IMU per AF e LC devo usare il campo valore ***
                if (myTxtVal.Text != string.Empty)
                {
                    nValoreDich = double.Parse(myTxtVal.Text);
                }
                myTxtVal.Text = FncValore.CalcoloValore(ConstWrapper.DBType,ConstWrapper.StringConnectionOPENgov, ConstWrapper.StringConnection, ConstWrapper.CodiceEnte, nAnno, sTipoRendita, sCategoria, sClasse, sZona, nRendita, nValoreDich, nConsistenza, dDal, IsColtivatore).ToString();
                //*** ***
                //*** ***
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.InkValore_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        
        //*** 20120629 - IMU ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtnumfigli_TextChanged(object sender, System.EventArgs e)
        {
            int nFigli = 0;
            Ribes.OPENgov.WebControls.RibesGridView myGrdFigli = null;
            Label lblCaricoFigli = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    myGrdFigli = GrdCaricoFigliDummy;
                    lblCaricoFigli = lblCaricoFigliDummy;
                    nFigli = Business.CoreUtility.ConvertiNumero(txtNumFigliDummy.Text);
                }
                else
                {
                    myGrdFigli = GrdCaricoFigliNoDummy;
                    lblCaricoFigli = lblCaricoFigliNoDummy;
                    nFigli = Business.CoreUtility.ConvertiNumero(txtnumfigliNoDummy.Text);
                }

                if (nFigli > 6)
                    nFigli = 6;

                if (nFigli > 0)
                {
                    DataTable dtCaricoFigli = new DataTable("CaricoFigli");
                    dtCaricoFigli.Columns.Add("nFiglio");
                    dtCaricoFigli.Columns.Add("percentuale");
                    object[] ListPercCarico = new object[2];
                    ListPercCarico.Initialize();
                    checked {
                        for (int x = 1; x <= nFigli; x++)
                        {
                            ListPercCarico[0] = "Figlio n." + x.ToString();
                            ListPercCarico[1] = "";
                            dtCaricoFigli.Rows.Add(ListPercCarico);
                        }
                    }
                    myGrdFigli.DataSource = dtCaricoFigli;
                    myGrdFigli.DataBind();
                    lblCaricoFigli.Style.Add("display", "");
                }
                else
                {
                    myGrdFigli.Visible = false;
                    lblCaricoFigli.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.txtnumerifigli_TextChanged.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** ***
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
                    foreach (GridViewRow myRow in GrdTARSU.Rows)
                    {
                        if (IDRow == ((HiddenField)myRow.FindControl("hfid")).Value)
                        {
                            string sScript, sParam = "";
                             sParam += "IdContribuente=" + ((HiddenField)myRow.FindControl("hfIdContribuente")).Value + "&IdTessera=" + ((HiddenField)myRow.FindControl("hfIdTessera")).Value + "&IdUniqueUI=" + IDRow + "&IdTestata=" + ((HiddenField)myRow.FindControl("hfIdTestata")).Value + "&AzioneProv=1&Provenienza=5&IdList=-1";
                            sParam += "&IsFromVariabile=" + ConstWrapper.IsFromVariabile;
                            sParam += "&ParamRitornoICI=\"IDTestata=" + this.IDTestata.ToString() + "$IDImmobile=" + this.IDImmobile.ToString() + "$IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "$IdDOCFA=" + this.DOCFAId + "$TYPEOPERATION=DETTAGLIO\"";
                            sScript = "";
                            sScript += "parent.Visualizza.location.href = '.." + ConstWrapper.Path_TARSU + "/Dichiarazioni/GestImmobili.aspx?" + sParam + "';";
                            sScript += "parent.Comandi.location.href = '../aspVuotaRemoveComandi';";
                            sScript += "parent.Basso.location.href = '../aspVuotaRemoveComandi';";
                            sScript += "parent.Nascosto.location.href = '../aspVuotaRemoveComandi';";
                            log.Debug("devo aprire TARSU in::" + sScript);
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.GrdRowCommand.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
        //private void GrdTARSU_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        //try{
        //    if (e.CommandName == "Update")
        //    {
        //        string sScript, sParam = "";
        //        Type csType = this.GetType();
        //        sScript = "";
        //        sScript += "parent.Comandi.location.href = '.." + ConstWrapper.Path_TARSU + "/Dichiarazioni/ComandiGestImmobili.aspx?Provenienza=5&AzioneProv=1';";
        //        sParam += "IdContribuente=" + e.Item.Cells[9].Text + "&IdTessera=" + e.Item.Cells[10].Text + "&IdUniqueUI=" + e.Item.Cells[11].Text + "&IdTestata=" + e.Item.Cells[12].Text + "&AzioneProv=1&Provenienza=5&IdList=-1";
        //        sParam += "&IsFromVariabile=" + ConstWrapper.IsFromVariabile;
        //        sParam += "&ParamRitornoICI=\"IDTestata=" + this.IDTestata.ToString() + "$IDImmobile=" + this.IDImmobile.ToString() + "$IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "$IdDOCFA=" + this.DOCFAId + "$TYPEOPERATION=DETTAGLIO\"";
        //        sScript += "parent.Visualizza.location.href = '.." + ConstWrapper.Path_TARSU + "/Dichiarazioni/GestImmobili.aspx?" + sParam + "';";
        //        sScript += "";
        //        log.Debug("devo aprire TARSU in::" + sScript);
        //        RegisterScript(sScript,this.GetType());csType, "idcontrpar", sScript);
        //    }
        // }
        //      catch (Exception ex)
        //    {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.GrdTARSU_ItemCommand.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        // }
        //}
        //*** ***
        #endregion
        //*** ***
        #region Metodi
        //*** 20140509 - TASI ***
        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco dei tipi di possesso.
        /// </summary>
        /// <returns></returns>
        protected DataView GetListTipiUtilizzi()
        {
            DataView Vista = new UtilizzoTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
            Vista.Sort = "Descrizione";
            return Vista;
        }
        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco dei tipi di possesso.
        /// </summary>
        /// <returns></returns>
        protected DataView GetListTipiPossesso()
        {
            DataView Vista = new PossessoTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
            Vista.Sort = "Descrizione";
            return Vista;
        }
        //*** ***

        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco delle categorie catastali.
        /// </summary>
        /// <returns></returns>
        protected DataView GetListCategorieCatastali()
        {
            DataView Vista = new CategoriaCatastaleTable(ConstWrapper.sUsername).List().DefaultView;
            Vista.Sort = "CategoriaCatastale";
            return Vista;
        }

        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco delle classi.
        /// </summary>
        /// <returns></returns>
        protected DataView GetListClassi()
        {
            DataView Vista = new ClasseTable(ConstWrapper.sUsername).List().DefaultView;
            Vista.Sort = "Classe";
            return Vista;
        }

        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco dei tipi degli immobili.
        /// </summary>
        /// <returns></returns>
        protected DataView GetListTipiImmobili()
        {
            DataView Vista = new TipoImmobileTable(ConstWrapper.sUsername).List().DefaultView;
            Vista.Sort = "Descrizione";
            return Vista;
        }

        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco dei codici delle rendite.
        /// </summary>
        /// <returns></returns>
        protected DataView GetListCodiciRendite()
        {
            DataView Vista = new Tipo_RenditaTable(ConstWrapper.sUsername).List().DefaultView;
            Vista.Sort = "COD_RENDITA";
            return Vista;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataView GetListCaratteristica()
        {
            DataView Vista = new Database.CaratteristicaTable(ConstWrapper.sUsername).List(ConstWrapper.StringConnectionOPENgov).DefaultView; //new DatabaseOpengov.CaratteristicaTable(ConstWrapper.sUsername).List().DefaultView;
            Vista.Sort = "Descrizione_Breve";
            return Vista;
        }
        #region "Combos"
        private void LoadCombos()
        {
            DataView dvDati = new DataView();

            try
            {
                ddlCaratteristicaDummy.Items.Clear();
                ddlCaratteristicaNoDummy.Items.Clear();
                dvDati = new DataView();
                ListItem myNoSelCar = new ListItem();
                myNoSelCar.Text = "...";
                myNoSelCar.Value = "0";
                ddlCaratteristicaDummy.Items.Add(myNoSelCar);
                ddlCaratteristicaNoDummy.Items.Add(myNoSelCar);
                dvDati = GetListCaratteristica();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["COD_CARATTERISTICA"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelCar1 = new ListItem();
                        myNoSelCar1.Text = myRow["COD_CARATTERISTICA"].ToString() + " - " + (string)myRow["Descrizione_Breve"];
                        myNoSelCar1.Value = myRow["COD_CARATTERISTICA"].ToString();
                        ddlCaratteristicaDummy.Items.Add(myNoSelCar1);
                        ddlCaratteristicaNoDummy.Items.Add(myNoSelCar1);
                    }
                }

                ddlCategoriaCatastaleDummy.Items.Clear();
                ddlCategoriaCatastaleNoDummy.Items.Clear();
                dvDati = new DataView();
                ListItem myNoSelCat = new ListItem("...", "0");
                ddlCategoriaCatastaleDummy.Items.Add(myNoSelCat);
                ddlCategoriaCatastaleNoDummy.Items.Add(myNoSelCat);
                dvDati = GetListCategorieCatastali();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["CategoriaCatastale"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelCat1 = new ListItem();
                        myNoSelCat1.Text = (string)myRow["CategoriaCatastale"];
                        myNoSelCat1.Value = (string)myRow["CategoriaCatastale"];
                        ddlCategoriaCatastaleDummy.Items.Add(myNoSelCat1);
                        ddlCategoriaCatastaleNoDummy.Items.Add(myNoSelCat1);
                    }
                }

                ddlClasseDummy.Items.Clear();
                ddlClasseNoDummy.Items.Clear();
                dvDati = new DataView();
                ListItem myNoSelCl = new ListItem("...", "0");
                ddlClasseDummy.Items.Add(myNoSelCl);
                ddlClasseNoDummy.Items.Add(myNoSelCl);
                dvDati = GetListClassi();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["Classe"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelCl1 = new ListItem();
                        myNoSelCl1.Text = (string)myRow["Classe"];
                        myNoSelCl1.Value = (string)myRow["Classe"];
                        ddlClasseDummy.Items.Add(myNoSelCl1);
                        ddlClasseNoDummy.Items.Add(myNoSelCl1);
                    }
                }

                ddlCodiceRenditaDummy.Items.Clear();
                ddlCodiceRenditaNoDummy.Items.Clear();
                dvDati = new DataView();
                ListItem myNoSelRen = new ListItem("...", "0");
                ddlCodiceRenditaDummy.Items.Add(myNoSelRen);
                ddlCodiceRenditaNoDummy.Items.Add(myNoSelRen);
                dvDati = GetListCodiciRendite();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["COD_RENDITA"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelRen1 = new ListItem();
                        myNoSelRen1.Text = (string)myRow["Sigla"];
                        myNoSelRen1.Value = (string)myRow["COD_RENDITA"];
                        ddlCodiceRenditaDummy.Items.Add(myNoSelRen1);
                        ddlCodiceRenditaNoDummy.Items.Add(myNoSelRen1);
                    }
                }

                ddlTipoImmobileDummy.Items.Clear();
                ddlTipoImmobileNoDummy.Items.Clear();
                dvDati = new DataView();
                ListItem myNoSelTipoImm = new ListItem("...", "0");
                ddlTipoImmobileDummy.Items.Add(myNoSelTipoImm);
                ddlTipoImmobileNoDummy.Items.Add(myNoSelTipoImm);
                dvDati = GetListTipiImmobili();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["TipoImmobile"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelTipoImm1 = new ListItem();
                        myNoSelTipoImm1.Text = myRow["TipoImmobile"].ToString();
                        myNoSelTipoImm1.Value = myRow["TipoImmobile"].ToString();
                        ddlTipoImmobileDummy.Items.Add(myNoSelTipoImm1);
                        ddlTipoImmobileNoDummy.Items.Add(myNoSelTipoImm1);
                    }
                }

                //*** 20140509 - TASI ***
                ddlTipoUtilizzoDummy.Items.Clear();
                ddlTipoUtilizzoNoDummy.Items.Clear();
                dvDati = new DataView();
                ListItem myNoSelTipoPos = new ListItem("...", "0");
                ddlTipoUtilizzoDummy.Items.Add(myNoSelTipoPos);
                ddlTipoUtilizzoNoDummy.Items.Add(myNoSelTipoPos);
                dvDati = GetListTipiUtilizzi();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["IdTipoUtilizzo"].ToString().CompareTo("0") != 0)
                    {
                        myNoSelTipoPos = new ListItem();
                        myNoSelTipoPos.Text = myRow["Descrizione"].ToString();
                        myNoSelTipoPos.Value = myRow["IdTipoUtilizzo"].ToString();
                        ddlTipoUtilizzoDummy.Items.Add(myNoSelTipoPos);
                        ddlTipoUtilizzoNoDummy.Items.Add(myNoSelTipoPos);
                    }
                }

                ddlTipoPossessoDummy.Items.Clear();
                ddlTipoPossessoNoDummy.Items.Clear();
                dvDati = new DataView();
                myNoSelTipoPos = new ListItem("...", "0");
                ddlTipoPossessoDummy.Items.Add(myNoSelTipoPos);
                ddlTipoPossessoNoDummy.Items.Add(myNoSelTipoPos);
                dvDati = GetListTipiPossesso();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["IdTipoPossesso"].ToString().CompareTo("0") != 0)
                    {
                        myNoSelTipoPos = new ListItem();
                        myNoSelTipoPos.Text = myRow["Descrizione"].ToString();
                        myNoSelTipoPos.Value = myRow["IdTipoPossesso"].ToString();
                        ddlTipoPossessoDummy.Items.Add(myNoSelTipoPos);
                        ddlTipoPossessoNoDummy.Items.Add(myNoSelTipoPos);
                    }
                }
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.LoadCombos.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #endregion
        #region "ControlsBind"
        /// <summary>
        /// Esegue il bind dei controlli della pagina.
        /// </summary>
        /// <param name="idImmobile"></param>
        /// <param name="idAttoCompraVendita"></param>
        /// <param name="fromPrecarica"></param>
        private void ControlsBind(int idImmobile, int idAttoCompraVendita, bool fromPrecarica)
        {
            try
            {
                log.Debug("ControlsBind");
                Utility.DichManagerICI.TestataRow myTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, idImmobile, ConstWrapper.StringConnection);
                if (ConstWrapper.HasDummyDich)
                    lblAnnoDichiarazioneDummy.Text = "Situazione al 31 dicembre " + myTestata.AnnoDichiarazione;
                else
                    lblAnnoDichiarazioneNoDummy.Text = "Situazione al 31 dicembre " + myTestata.AnnoDichiarazione;

                ContribuenteBind(myTestata.IDContribuente, this.IDTestata);
                ImmobileBind(this.IDImmobile, idAttoCompraVendita, int.Parse(hdIdContribuente.Value), fromPrecarica);
                DettaglioBind(this.IDTestata, this.IDImmobile, idAttoCompraVendita, fromPrecarica);
                log.Debug("esco da ControlsBind");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ControlIsBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //*** 20130304 - gestione dati da territorio ***
        //        private void ControlsBindFromTer(int IdUI, int IdProprieta, int IdProprietario, int IdTestata)
        //        {
        //            OPENUtility.CreateSessione WFSessione = new OPENUtility.CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(), HttpContext.Current.Session["username"].ToString(), System.Configuration.ConfigurationManager.AppSettings["OPENGOVT"]);
        //            try 
        //            {
        //                string WFErrore="";
        //                ContribuenteBind(this.IDTestata);
        //                //inizializzo la connessione
        //                if (!(WFSessione.CreaSessione(HttpContext.Current.Session["username"].ToString(), ref WFErrore ))) 
        //                { 
        //                    throw new Exception("Errore durante l'apertura della sessione di WorkFlow"); 
        //                } 
        //                DataSet myDsResult= new DataSet();
        //                myDsResult = new Database.UITerritorio().GetRow(WFSessione, ConstWrapper.CodiceEnte, int.Parse(hdIdContribuente.Value), -1, "", "", "", IdUI, IdProprieta, IdProprietario);
        //                if (myDsResult != null)
        //                {
        //                    if (myDsResult.Tables[0].Rows.Count > 0)
        //                    {
        //                        for (int x=0;x<myDsResult.Tables[0].Rows.Count;x++)
        //                        {																		   
        //                            if (myDsResult.Tables[0].Rows[x]["ValoreCatastale"].ToString() != "-1,00")
        //                                txtValore.Text = myDsResult.Tables[0].Rows[x]["ValoreCatastale"].ToString();
        //                            else
        //                                txtValore.Text = string.Empty;

        //                            lblViaOld.Text = "(" + myDsResult.Tables[0].Rows[x]["indirizzo"].ToString() + ")";
        //                            // cerco la via nello stradario
        //                            if ((myDsResult.Tables[0].Rows[x]["id_Via"].ToString().CompareTo("") == 0 ) || (myDsResult.Tables[0].Rows[x]["id_Via"].ToString().CompareTo("-1") == 0))
        //                            {
        //                                txtCodVia.Text = "-1";
        //                                txtVia.Text = "";
        //                            }
        //                            else
        //                            {		
        //                                try
        //                                {
        //                                    OggettoStrada[] ArrStrade;
        //                                    OggettoStrada Strada = new OggettoStrada();

        //                                    Strada.CodiceStrada = int.Parse(myDsResult.Tables[0].Rows[x]["id_Via"].ToString());
        //                                    Strada.CodiceEnte = ConstWrapper.CodiceEnte;

        //                                    //*** richiamando il ws ****
        //                                    if (System.Configuration.ConfigurationManager.AppSettings["TipoStradario"].ToString()== "WS")
        //                                    {
        //                                        WsStradario.Stradario objStradario = new WsStradario.Stradario();
        //                                        ArrStrade = objStradario.GetStrade(Strada);	
        //                                    }
        //                                    else
        //                                    {
        //                                        //*** richiamando direttamente il servizio ***
        //                                        Type typeofRI = typeof(RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario);
        //                                        RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario RemStradario = (RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLServizioStradario"].ToString());
        //                                        ArrStrade = RemStradario.GetArrayOggettoStrade(System.Configuration.ConfigurationManager.AppSettings["ConnessioneDBComuniStrade"], Strada);
        //                                    }

        //                                    if (ArrStrade.Length.CompareTo(1) == 0)
        //                                    {
        //                                        txtCodVia.Text = ArrStrade[0].CodiceStrada.ToString();
        //                                        txtVia.Text = ArrStrade[0].DenominazioneStrada;
        //                                    }
        //                                }
        //                                catch(Exception err)
        //                                {
        //                                    log.Warn ("Errore connesione stradario",err);
        //                                    StringBuilder strBuild = new StringBuilder();
        //                                    strBuild.Append("");
        //                                    strBuild.Append("alert('Si sono verificati dei problemi nello stradario. Contattare il servizio di assistenza!')");
        //                                    strBuild.Append("");
        //                                    RegisterScript(sScript,this.GetType());,"scriptalert", strBuild.ToString());
        //                                }
        //                            }

        //                            if (int.Parse(myDsResult.Tables[0].Rows[x]["Civico"].ToString())==-1)
        //                                txtNumCiv.Text = string.Empty;
        //                            else
        //                                txtNumCiv.Text = myDsResult.Tables[0].Rows[x]["Civico"].ToString();
        //                            txtEspCivico.Text = myDsResult.Tables[0].Rows[x]["Esponente"].ToString();

        //                            txtInterno.Text = myDsResult.Tables[0].Rows[x]["Interno"].ToString();
        //                            txtScala.Text = myDsResult.Tables[0].Rows[x]["Scala"].ToString();
        //                            txtPiano.Text = myDsResult.Tables[0].Rows[x]["Piano"].ToString();
        //                            txtFoglio.Text = myDsResult.Tables[0].Rows[x]["Foglio"].ToString();
        //                            txtNumero.Text = myDsResult.Tables[0].Rows[x]["Numero"].ToString();
        //                            //if (int.Parse(myDsResult.Tables[0].Rows[x]["Subalterno"].ToString())==-1) 
        //                            //    txtSubalterno.Text = string.Empty;	
        //                            //else
        //                                txtSubalterno.Text = myDsResult.Tables[0].Rows[x]["Subalterno"].ToString();
        //                            log.Debug("ControlBindFromTer:.devo caricare ddlCategoriaCatastale::"+myDsResult.Tables[0].Rows[x]["CatCatastale"].ToString());
        //                            ddlCategoriaCatastale.SelectedItem.Selected = false;
        //                            ddlCategoriaCatastale.Items.FindByValue(myDsResult.Tables[0].Rows[x]["CatCatastale"].ToString()).Selected = true;
        //                            log.Debug("ControlBindFromTer:.devo caricare ddlClasse::"+myDsResult.Tables[0].Rows[x]["ClasseCatastale"].ToString());
        //                            ddlClasse.SelectedItem.Selected = false;
        //                            ddlClasse.Items.FindByValue(myDsResult.Tables[0].Rows[x]["ClasseCatastale"].ToString()).Selected = true;
        //                            log.Debug("ControlBindFromTer:.devo caricare ddlCodiceRendita::"+myDsResult.Tables[0].Rows[x]["Cod_tipo_Rendita"].ToString());
        //                            ddlCodiceRendita.SelectedItem.Selected = false;
        //                            ddlCodiceRendita.Items.FindByValue(myDsResult.Tables[0].Rows[x]["Cod_tipo_Rendita"].ToString()).Selected = true;
        //                            // popolo la combo estimo
        //                            if (ddlCodiceRendita.SelectedItem.Text.CompareTo("AF") == 0)
        //                            {
        //                                PopolaDDlEstimo("AF");
        //                            }
        //                            else
        //                            {
        //                                PopolaDDlEstimo("");
        //                            }
        //                            log.Debug("ControlBindFromTer:.devo caricare ddlCaratteristica::"+ddlCodiceRendita.SelectedValue.ToString());							
        //                            ddlCaratteristica.SelectedItem.Selected = false;
        //                            switch(ddlCodiceRendita.SelectedValue)
        //                            {
        //                                case "TA":
        //                                    ddlCaratteristica.Items.FindByValue("1").Selected = true;
        //                                    break;
        //                                case "AF":
        //                                    ddlCaratteristica.Items.FindByValue("2").Selected = true;
        //                                    PopolaDDlEstimo("AF");
        //                                    break;
        //                                case "RE":
        //                                    ddlCaratteristica.Items.FindByValue("3").Selected = true;
        //                                    break;
        //                                case "LC":
        //                                    ddlCaratteristica.Items.FindByValue("4").Selected = true;
        //                                    break;
        //                            }
        //                            txtDataInizio.Text = Business.CoreUtility.FormattaDataGrd(myDsResult.Tables[0].Rows[x]["Data_Inizio"]);
        //                            txtDataFine.Text = Business.CoreUtility.FormattaDataGrd(myDsResult.Tables[0].Rows[x]["Data_Fine"]);
        //                            if (myDsResult.Tables[0].Rows[x]["Consistenza"].ToString().CompareTo("-1,00") == 0 || myDsResult.Tables[0].Rows[x]["Consistenza"].ToString().CompareTo("-1") == 0)
        //                            {
        //                                txtConsistenza.Text = string.Empty;
        //                            }
        //                            else
        //                            {
        //                                txtConsistenza.Text = myDsResult.Tables[0].Rows[x]["Consistenza"].ToString();
        //                            }
        //                            if (myDsResult.Tables[0].Rows[x]["Percentuale_proprieta"].ToString() == "-1")
        //                                txtPercPossesso.Text = string.Empty;
        //                            else
        //                                txtPercPossesso.Text = myDsResult.Tables[0].Rows[x]["Percentuale_proprieta"].ToString();
        //                            if (myDsResult.Tables[0].Rows[x]["MesiPossesso"].ToString() == "-1")
        //                                txtMesiPossesso.Text = string.Empty;
        //                            else
        //                                txtMesiPossesso.Text = myDsResult.Tables[0].Rows[x]["MesiPossesso"].ToString();
        //                            //*** 20140509 - TASI ***
        //                            log.Debug("ControlBindFromTer:.devo caricare ddlTipoUtilizzo::" + myDsResult.Tables[0].Rows[x]["cod_tipo_utilizzo"].ToString());
        //                            ddlTipoUtilizzo.SelectedItem.Selected = false;
        //                            ddlTipoUtilizzo.Items.FindByValue(myDsResult.Tables[0].Rows[x]["cod_tipo_proprieta"].ToString()).Selected = true;
        //                            log.Debug("ControlBindFromTer:.devo caricare ddlTipoPossesso::" + myDsResult.Tables[0].Rows[x]["cod_tipo_proprieta"].ToString());
        //                            ddlTipoPossesso.SelectedItem.Selected = false;
        //                            ddlTipoPossesso.Items.FindByValue(myDsResult.Tables[0].Rows[x]["cod_tipo_proprieta"].ToString()).Selected = true;
        //                            //*** ***
        ////							ddlPossesso.SelectedItem.Selected = false;
        ////							ddlPossesso.Items.FindByValue(myDsResult.Tables[0].Rows[x]["Possesso"].ToString()).Selected = true;
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception Err) 
        //            {
        //                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ControlsBindFromTer.errore: ", Err);
        //                     Response.Redirect("../PaginaErrore.aspx");
        //            }		
        //            finally
        //            {
        //                txtIdTerUI.Value="";
        //                WFSessione.Kill();
        //            }
        //        }
        private void ControlsBindFromTer(int IdUI, int IdProprieta, int IdProprietario, int IdTestata)
        {
                   try
            {
                String sScript = "GestAlert('a', 'info', '', '', 'Funzionalita\' non attiva.');";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ControlIsBindFromTer.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
            finally
            {
                txtIdTerUI.Value = "";
            }
        }
        //*** ***
        #region ImmobileBind
        /// <summary>
        /// Esegue il bind dei controlli dei dati dell'immobile.
        /// </summary>
        /// <param name="idImmobile"></param>
        /// <param name="idAttoCompraVendita"></param>
        /// <param name="idContribuente"></param>
        /// <param name="fromPrecarica"></param>
        private void ImmobileBind(int idImmobile, int idAttoCompraVendita, int idContribuente, bool fromPrecarica)
        {
            try
            {
                //log.Debug ("INIZIO ImmobileBind");
                Utility.DichManagerICI.OggettiRow Riga = new Utility.DichManagerICI.OggettiRow();
                //*** 20131003 - gestione atti compravendita ***
                if ((idImmobile <= 0 && idAttoCompraVendita > 0) || (idAttoCompraVendita > 0 && fromPrecarica == true))
                {
                    log.Debug("ImmobileBind::carico da oggetti compravendita::idAttoCompraVendita::" + idAttoCompraVendita.ToString() + "::idContribuente::" + idContribuente.ToString());
                    Riga = new DichiarazioniView().GetCompraVenditaOggetti(idAttoCompraVendita, this.IDTestata, idContribuente);
                }
                else
                {
                    log.Debug("ImmobileBind::carico da oggetti ici::idImmobile::" + idImmobile.ToString());
                    Riga = new OggettiTable(ConstWrapper.sUsername).GetRow(idImmobile, ConstWrapper.StringConnection);
                }
                //*** ***
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                ImmobileBind(Riga, fromPrecarica);
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ImmobileBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        private void ImmobileBind(Utility.DichManagerICI.OggettiRow Riga, bool fromPrecarica)
        {
            TextBox txtNumOrdine = null;
            TextBox txtNumModello = null;
            TextBox txtValore = null;
            TextBox txtCodUI = null;
            TextBox txtVia = null;
            TextBox txtNumCiv = null;
            TextBox txtEspCivico = null;
            TextBox txtBarrato = null;
            TextBox txtInterno = null;
            TextBox txtScala = null;
            TextBox txtPiano = null;
            TextBox txtNumeroEcografico = null;
            TextBox txtDescrizioneUffRegistro = null;
            TextBox txtSezione = null;
            TextBox txtPartitaCatastale = null;
            TextBox txtFoglio = null;
            TextBox txtNumero = null;
            TextBox txtSubalterno = null;
            TextBox txtNumProtocollo = null;
            TextBox txtAnnoDenunciaCatastale = null;
            TextBox txtDataInizio = null;
            TextBox txtDataFine = null;
            TextBox txtNoteIci = null;
            TextBox txtConsistenza = null;
            TextBox txtRendita = null;

            DropDownList ddlCaratteristica = null;
            DropDownList ddlTipoImmobile = null;
            DropDownList ddlCategoriaCatastale = null;
            DropDownList ddlClasse = null;
            DropDownList ddlCodiceRendita = null;
            DropDownList ddlAcquisto = null;
            DropDownList ddlCessione = null;
            DropDownList ddlEstimo = null;

            CheckBox chkStorico = null;
            CheckBox chkExRurale = null;
            CheckBox chkValoreProvvisorio = null;
            CheckBox chkPertinenza = null;

            Label lblViaOld = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    txtNumOrdine = txtNumOrdineDummy;
                    txtNumModello = txtNumModelloDummy;
                    txtValore = txtValoreDummy;
                    txtCodUI = txtCodUIDummy;
                    txtVia = txtViaDummy;
                    txtNumCiv = txtNumCivDummy;
                    txtEspCivico = txtEspCivicoDummy;
                    txtBarrato = txtBarratoDummy;
                    txtInterno = txtInternoDummy;
                    txtScala = txtScalaDummy;
                    txtPiano = txtPianoDummy;
                    txtNumeroEcografico = txtNumeroEcograficoDummy;
                    txtDescrizioneUffRegistro = txtDescrizioneUffRegistroDummy;
                    txtSezione = txtSezioneDummy;
                    txtPartitaCatastale = txtPartitaCatastaleDummy;
                    txtFoglio = txtFoglioDummy;
                    txtNumero = txtNumeroDummy;
                    txtSubalterno = txtSubalternoDummy;
                    txtNumProtocollo = txtNumProtocolloDummy;
                    txtAnnoDenunciaCatastale = txtAnnoDenunciaCatastaleDummy;
                    txtDataInizio = txtDataInizioDummy;
                    txtDataFine = txtDataFineDummy;
                    txtNoteIci = txtNoteIciDummy;
                    txtConsistenza = txtConsistenzaDummy;
                    txtRendita = txtRenditaDummy;

                    ddlCaratteristica = ddlCaratteristicaDummy;
                    ddlTipoImmobile = ddlTipoImmobileDummy;
                    ddlCategoriaCatastale = ddlCategoriaCatastaleDummy;
                    ddlClasse = ddlClasseDummy;
                    ddlCodiceRendita = ddlCodiceRenditaDummy;
                    ddlAcquisto = ddlAcquistoDummy;
                    ddlCessione = ddlCessioneDummy;
                    ddlEstimo = ddlEstimoDummy;

                    chkStorico = chkStoricoDummy;
                    chkExRurale = chkExRuraleDummy;
                    chkValoreProvvisorio = chkValoreProvvisorioDummy;
                    chkPertinenza = chkPertinenzaDummy;

                    lblViaOld = lblViaOldDummy;
                }
                else
                {
                    txtNumOrdine = txtNumOrdineNoDummy;
                    txtNumModello = txtNumModelloNoDummy;
                    txtValore = txtValoreNoDummy;
                    txtCodUI = txtCodUINoDummy;
                    txtVia = txtViaNoDummy;
                    txtNumCiv = txtNumCivNoDummy;
                    txtEspCivico = txtEspCivicoNoDummy;
                    txtBarrato = txtBarratoNoDummy;
                    txtInterno = txtInternoNoDummy;
                    txtScala = txtScalaNoDummy;
                    txtPiano = txtPianoNoDummy;
                    txtNumeroEcografico = txtNumeroEcograficoNoDummy;
                    txtDescrizioneUffRegistro = txtDescrizioneUffRegistroNoDummy;
                    txtSezione = txtSezioneNoDummy;
                    txtPartitaCatastale = txtPartitaCatastaleNoDummy;
                    txtFoglio = txtFoglioNoDummy;
                    txtNumero = txtNumeroNoDummy;
                    txtSubalterno = txtSubalternoNoDummy;
                    txtNumProtocollo = txtNumProtocolloNoDummy;
                    txtAnnoDenunciaCatastale = txtAnnoDenunciaCatastaleNoDummy;
                    txtDataInizio = txtDataInizioNoDummy;
                    txtDataFine = txtDataFineNoDummy;
                    txtNoteIci = txtNoteIciNoDummy;
                    txtConsistenza = txtConsistenzaNoDummy;
                    txtRendita = txtRenditaNoDummy;

                    ddlCaratteristica = ddlCaratteristicaNoDummy;
                    ddlTipoImmobile = ddlTipoImmobileNoDummy;
                    ddlCategoriaCatastale = ddlCategoriaCatastaleNoDummy;
                    ddlClasse = ddlClasseNoDummy;
                    ddlCodiceRendita = ddlCodiceRenditaNoDummy;
                    ddlAcquisto = ddlAcquistoNoDummy;
                    ddlCessione = ddlCessioneNoDummy;
                    ddlEstimo = ddlEstimoNoDummy;

                    chkStorico = chkStoricoNoDummy;
                    chkExRurale = chkExruraleNoDummy;
                    chkValoreProvvisorio = chkValoreProvvisorioNoDummy;
                    chkPertinenza = chkPertinenzaNoDummy;

                    lblViaOld = lblViaOldNoDummy;
                }
                txtNumOrdine.Text = Riga.NumeroOrdine;
                txtNumModello.Text = Riga.NumeroModello;
                txtDataUltimaModifica.Text = Riga.DataUltimaModifica == DateTime.MinValue ? String.Empty : Riga.DataUltimaModifica.ToShortDateString();

                chkStorico.Checked = Riga.Storico;
                chkExRurale.Checked = Riga.ExRurale;
                //txtValore.Text = Riga.ValoreImmobile.ToString("N");			 

                if (Riga.ValoreImmobile.ToString() != "-1,00")
                    txtValore.Text = Riga.ValoreImmobile.ToString("N");
                //txtValore.Text = Riga.ValoreImmobile.ToString("N").Replace(".","");
                else
                    txtValore.Text = string.Empty;

                chkValoreProvvisorio.Checked = Riga.FlagValoreProvv;

                //log.Debug("devo caricare txtCodUI::" + Riga.CodUI.ToString());
                if (Riga.CodUI == "-1" || Riga.CodUI == string.Empty || Riga.CodUI == null)
                    txtCodUI.Text = string.Empty;
                else
                    txtCodUI.Text = Riga.CodUI.ToString();
                //log.Debug("devo caricare comune::" + Riga.Comune.ToString());
                txtCodiceComune.Text = Riga.Comune;
                //log.Debug("devo caricare via::" + Riga.Via.ToString());

                lblViaOld.Text = "(" + Riga.Via + ")";

                //log.Debug("devo caricare cod via::" + Riga.CodVia.ToString());
                // cerco la via nello stradario
                if ((Riga.CodVia == null || Riga.CodVia.CompareTo("") == 0) || (Riga.CodVia.CompareTo("-1") == 0))
                {
                    txtCodVia.Text = "-1";
                    txtVia.Text = "";
                }
                else
                {
                    try
                    {
                        //log.Debug ("Richiamo stradario");
                        OggettoStrada[] ArrStrade;
                        OggettoStrada Strada = new OggettoStrada();

                        Strada.CodiceStrada = int.Parse(Riga.CodVia);
                        Strada.CodiceEnte = ConstWrapper.CodiceEnte;

                        //*** richiamando il ws ****
                        if (System.Configuration.ConfigurationManager.AppSettings["TipoStradario"].ToString() == "WS")
                        {
                            WsStradario.Stradario objStradario = new WsStradario.Stradario();
                            ArrStrade = objStradario.GetStrade(Strada);
                        }
                        else
                        {
                            //*** richiamando direttamente il servizio ***
                            Type typeofRI = typeof(RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario);
                            RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario RemStradario = (RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLServizioStradario"].ToString());
                            ArrStrade = RemStradario.GetArrayOggettoStrade(System.Configuration.ConfigurationManager.AppSettings["DBType"].ToString(), System.Configuration.ConfigurationManager.AppSettings["ConnessioneDBComuniStrade"], Strada);
                        }

                        if (ArrStrade.Length.CompareTo(1) == 0)
                        {
                            txtCodVia.Text = ArrStrade[0].CodiceStrada.ToString();
                            txtVia.Text = ArrStrade[0].DenominazioneStrada;
                        }
                        //log.Debug ("stradario OK");
                    }
                    catch (Exception err)
                    {
                        log.Warn("Errore connesione stradario", err);
                        /*StringBuilder strBuild = new StringBuilder();
                        strBuild.Append("");
                        strBuild.Append("alert('Si sono verificati dei problemi nello stradario. Contattare il servizio di assistenza!')");
                        strBuild.Append("");
                        RegisterScript(sScript,this.GetType());,"scriptalert", strBuild.ToString());*/
                    }
                }

                //log.Debug("devo caricare civico::" + Riga.NumeroCivico.ToString());
                //txtVia.Text = Riga.Via;
                if (Riga.NumeroCivico == -1)
                    txtNumCiv.Text = string.Empty;
                else
                    txtNumCiv.Text = Riga.NumeroCivico.ToString();
                //log.Debug("devo caricare esponennte::" + Riga.EspCivico.ToString());
                txtEspCivico.Text = Riga.EspCivico;
                //log.Debug("devo caricare barrato::" + Riga.Barrato.ToString());
                txtBarrato.Text = Riga.Barrato;
                //log.Debug("devo caricare interno::" + Riga.Interno.ToString());
                txtInterno.Text = Riga.Interno;
                //log.Debug("devo caricare scala::" + Riga.Scala.ToString());
                txtScala.Text = Riga.Scala;
                //log.Debug("devo caricare piano::" + Riga.Piano.ToString());
                txtPiano.Text = Riga.Piano;
                //log.Debug("devo caricare numeco::" + Riga.NumeroEcografico.ToString());
                if (Riga.NumeroEcografico == -1)
                    txtNumeroEcografico.Text = string.Empty;
                else
                    txtNumeroEcografico.Text = Riga.NumeroEcografico.ToString();
                //log.Debug("devo caricare uffreg::" + Riga.DescrUffRegistro.ToString());
                txtDescrizioneUffRegistro.Text = Riga.DescrUffRegistro;
                //log.Debug("devo caricare sezione::" + Riga.Sezione.ToString());
                txtSezione.Text = Riga.Sezione;
                //log.Debug("devo caricare aprtitacat::" + Riga.PartitaCatastale.ToString());
                if (Riga.PartitaCatastale == -1)
                    txtPartitaCatastale.Text = string.Empty;
                else
                    txtPartitaCatastale.Text = Riga.PartitaCatastale.ToString();
                //log.Debug("devo caricare foglio::" + Riga.Foglio.ToString());
                txtFoglio.Text = Riga.Foglio;
                //log.Debug("devo caricare num::" + Riga.Numero.ToString());
                txtNumero.Text = Riga.Numero;
                //log.Debug("devo caricare sub::" + Riga.Subalterno.ToString());
                //if (Riga.Subalterno == -1) 
                //    txtSubalterno.Text = string.Empty;	
                //else
                txtSubalterno.Text = Riga.Subalterno.ToString();

                //log.Debug("devo caricare ddlCategoriaCatastale::" + Riga.CodCategoriaCatastale.ToString());
                ddlCategoriaCatastale.SelectedItem.Selected = false;
                if (Riga.CodCategoriaCatastale != null)
                {
                    try
                    {
                        ddlCategoriaCatastale.Items.FindByValue(Riga.CodCategoriaCatastale.ToString()).Selected = true;
                    }
                    catch
                    {
                        log.Debug("categoria non trovata->" + Riga.CodCategoriaCatastale.ToString());
                    }
                }
                //log.Debug("devo caricare ddlClasse::" + Riga.CodClasse.ToString());
                ddlClasse.SelectedItem.Selected = false;
                if (Riga.CodClasse != null)
                {
                    try
                    {
                        if (Riga.CodClasse.StartsWith("0"))
                            Riga.CodClasse = Riga.CodClasse.Substring(1, Riga.CodClasse.Length - 1);
                        ddlClasse.Items.FindByValue(Riga.CodClasse.ToString()).Selected = true;
                    }
                    catch
                    {
                        log.Debug("classe non trovata->" + Riga.CodClasse.ToString());
                    }
                }
                //log.Debug("devo caricare ddlCodiceRendita::" + Riga.CodRendita.ToString());
                ddlCodiceRendita.SelectedItem.Selected = false;
                if (Riga.CodRendita != null)
                {
                    try
                    {
                        ddlCodiceRendita.Items.FindByValue(Riga.CodRendita.ToString()).Selected = true;
                    }
                    catch
                    {
                        log.Debug("rendita non trovata->" + Riga.CodRendita.ToString());
                    }
                }
                //log.Debug("devo caricare ddlTipoImmobile::" + Riga.TipoImmobile.ToString());
                ddlTipoImmobile.SelectedItem.Selected = false;
                ddlTipoImmobile.Items.FindByValue(Riga.TipoImmobile.ToString()).Selected = true;
                //log.Debug("devo caricare ddlCaratteristica::" + Riga.Caratteristica.ToString());
                ddlCaratteristica.SelectedItem.Selected = false;
                ddlCaratteristica.Items.FindByValue(Riga.Caratteristica.ToString()).Selected = true;

                // popolo la combo estimo
                if (ddlCodiceRendita.SelectedItem.Text.CompareTo("AF") == 0)
                {
                    PopolaDDlEstimo(ddlEstimo, "AF");
                }
                else
                {
                    PopolaDDlEstimo(ddlEstimo, "");
                }

                txtNumProtocollo.Text = Riga.NumeroProtCatastale;
                txtAnnoDenunciaCatastale.Text = Riga.AnnoDenunciaCatastale;

                //chkAcquisto.Checked = Riga.TitoloAcquisto;
                //chkCessione.Checked = Riga.TitoloCessione;
                //log.Debug("devo caricare ddlAcquisto::"+Riga.TitoloAcquisto.ToString());
                ddlAcquisto.SelectedValue = Riga.TitoloAcquisto.ToString();
                //log.Debug("devo caricare ddlCessione::"+Riga.TitoloCessione.ToString());
                ddlCessione.SelectedValue = Riga.TitoloCessione.ToString();

                //txtDataInizio.Text = Riga.DataInizio.ToString();
                //txtDataFine.Text = Riga.DataFine.ToString();
                if (Request.QueryString["titolo"] != "C" || fromPrecarica == false)
                {
                    txtDataInizio.Text = Riga.DataInizio.ToShortDateString();
                }

                txtDataFine.Text = Business.CoreUtility.FormattaDataGrd(Riga.DataFine);
                //if (Riga.DataFine.CompareTo(DateTime.MaxValue) != 0)
                //{
                //    txtDataFine.Text = Riga.DataFine.ToShortDateString();
                //}
                //else
                //{
                //    txtDataFine.Text = "";
                //}

                txtCodPertinenza.Text = Riga.IDImmobilePertinente.ToString();
                if ((Riga.IDImmobilePertinente.CompareTo(0) != 0) && (Riga.IDImmobilePertinente.CompareTo(-1) != 0) && (Riga.IDImmobilePertinente.CompareTo(this.IDImmobile) != 0))
                {
                    chkPertinenza.Checked = true;
                }
                else
                {
                    chkPertinenza.Checked = false;
                }

                txtNoteIci.Text = Riga.NoteIci;

                if (Riga.Consistenza.ToString().CompareTo("-1,00") == 0 || Riga.Consistenza.ToString().CompareTo("-1") == 0)
                {
                    txtConsistenza.Text = string.Empty;
                }
                else
                {
                    txtConsistenza.Text = Riga.Consistenza.ToString();
                }

                if (Riga.Rendita.ToString().CompareTo("-1,00") == 0 || Riga.Rendita.ToString().CompareTo("-1") == 0)
                {
                    txtRendita.Text = string.Empty;
                }
                else
                {
                    txtRendita.Text = Riga.Rendita.ToString();
                }

                if (Riga.Zona != "0" && Riga.Zona != null)
                {
                    //log.Debug("devo caricare ddlEstimo::" + Riga.Zona.ToString());
                    ListItem SelezionaItem;
                    ddlEstimo.SelectedItem.Selected = false;
                    SelezionaItem = ddlEstimo.Items.FindByText(Riga.Zona.ToString());
                    ddlEstimo.SelectedIndex = ddlEstimo.Items.IndexOf(SelezionaItem);
                }
                //log.Debug ("FINE ImmobileBind");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ImmobileBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #endregion
        #region DettaglioBind
        /// <summary>
        ///  Esegue il bind dei controlli dei dati del dettaglio.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <param name="idImmobile"></param>
        /// <param name="idAttoCompraVendita"></param>
        /// <param name="fromPrecarica"></param>
        private void DettaglioBind(int idTestata, int idImmobile, int idAttoCompraVendita, bool fromPrecarica)
        {
            try
            {
                //log.Debug ("INIZIO DettaglioBind");
                Utility.DichManagerICI.DettaglioTestataRow Riga = new Utility.DichManagerICI.DettaglioTestataRow();
                //*** 20131003 - gestione atti compravendita ***
                if ((idImmobile <= 0 && idAttoCompraVendita > 0) || (idAttoCompraVendita > 0 && fromPrecarica == true))
                {
                    Riga = new DichiarazioniView().GetCompraVenditaDettaglioTestata(idAttoCompraVendita, this.IDTestata, int.Parse(hdIdContribuente.Value));
                }
                else
                {
                    Riga = new DettaglioTestataTable(ConstWrapper.sUsername).GetRow(idImmobile, idTestata, false, ConstWrapper.StringConnection);
                }
                //*** ***
                DettaglioBind(Riga);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.DettaglioBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        private void DettaglioBind(Utility.DichManagerICI.DettaglioTestataRow Riga)
        {
            TextBox txtPercPossesso = null;
            TextBox txtMesiPossesso = null;
            TextBox txtMesiEsclusione = null;
            TextBox txtMesiRiduzione = null;
            TextBox txtImpDetrazione = null;
            TextBox txtNumeroUtilizzatori = null;
            TextBox txtNumFigli = null;

            DropDownList ddlTipoUtilizzo = null;
            DropDownList ddlTipoPossesso = null;
            DropDownList ddlAbitazionePrincipale = null;
            DropDownList ddlPossesso = null;
            DropDownList ddlRiduzione = null;
            DropDownList ddlEsclusoEsente = null;

            CheckBox chkAbitPrincipale = null;
            CheckBox chkPertinenza = null;
            CheckBox chkColtivatori = null;

            Ribes.OPENgov.WebControls.RibesGridView GrdCaricoFigli = null;

            Label LblCaricoFigli = null;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    txtPercPossesso = txtPercPossessoDummy;
                    txtMesiPossesso = txtMesiPossessoDummy;
                    txtMesiEsclusione = txtMesiEsclusioneDummy;
                    txtMesiRiduzione = txtMesiRiduzioneDummy;
                    txtImpDetrazione = txtImpDetrazioneDummy;
                    txtNumeroUtilizzatori = txtNumeroUtilizzatoriDummy;
                    txtNumFigli = txtNumFigliDummy;

                    ddlTipoUtilizzo = ddlTipoUtilizzoDummy;
                    ddlTipoPossesso = ddlTipoPossessoDummy;
                    ddlAbitazionePrincipale = ddlAbitazionePrincipaleDummy;
                    ddlPossesso = ddlPossessoDummy;
                    ddlRiduzione = ddlRiduzioneDummy;
                    ddlEsclusoEsente = ddlEsclusoEsenteDummy;

                    chkAbitPrincipale = chkAbitPrincipaleDummy;
                    chkPertinenza = chkPertinenzaDummy;
                    chkColtivatori = chkColtivatoriDummy;

                    GrdCaricoFigli = GrdCaricoFigliDummy;

                    LblCaricoFigli = lblCaricoFigliDummy;
                }
                else
                {
                    txtPercPossesso = txtPercPossessoNoDummy;
                    txtMesiPossesso = txtMesiPossessoNoDummy;
                    txtMesiEsclusione = txtMesiEsclusioneNoDummy;
                    txtMesiRiduzione = txtMesiRiduzioneNoDummy;
                    txtImpDetrazione = txtImpDetrazioneNoDummy;
                    txtNumeroUtilizzatori = txtNumeroUtilizzatoriNoDummy;
                    txtNumFigli = txtnumfigliNoDummy;

                    ddlTipoUtilizzo = ddlTipoUtilizzoNoDummy;
                    ddlTipoPossesso = ddlTipoPossessoNoDummy;
                    ddlAbitazionePrincipale = ddlAbitazionePrincipaleNoDummy;
                    ddlPossesso = ddlPossessoNoDummy;
                    ddlRiduzione = ddlRiduzioneNoDummy;
                    ddlEsclusoEsente = ddlEsclusoEsenteNoDummy;

                    chkAbitPrincipale = chkAbitprincipaleNoDummy;
                    chkPertinenza = chkPertinenzaNoDummy;
                    chkColtivatori = chkcoltivatoriNoDummy;

                    GrdCaricoFigli = GrdCaricoFigliNoDummy;

                    LblCaricoFigli = lblCaricoFigliNoDummy;
                }
                if (Riga.PercPossesso == -1)
                    txtPercPossesso.Text = string.Empty;
                else
                    txtPercPossesso.Text = Riga.PercPossesso.ToString("n2");
                if (Riga.MesiPossesso == -1)
                    txtMesiPossesso.Text = string.Empty;
                else
                    txtMesiPossesso.Text = Riga.MesiPossesso.ToString();
                //*** 20140509 - TASI ***
                //log.Debug("devo caricare ddlTipoUtilizzo::" + Riga.TipoUtilizzo.ToString());
                ddlTipoUtilizzo.SelectedItem.Selected = false;
                ddlTipoUtilizzo.Items.FindByValue(Riga.TipoUtilizzo.ToString()).Selected = true;
                //log.Debug("devo caricare ddlTipoPossesso::" + Riga.TipoPossesso.ToString());
                ddlTipoPossesso.SelectedItem.Selected = false;
                ddlTipoPossesso.Items.FindByValue(Riga.TipoPossesso.ToString()).Selected = true;
                //*** ***
                if (Riga.MesiEsclusioneEsenzione == -1)
                    txtMesiEsclusione.Text = string.Empty;
                else
                    txtMesiEsclusione.Text = Riga.MesiEsclusioneEsenzione.ToString();
                if (Riga.MesiRiduzione == -1)
                    txtMesiRiduzione.Text = string.Empty;
                else
                    txtMesiRiduzione.Text = Riga.MesiRiduzione.ToString();

                //log.Debug("devo caricare ddlAbitazionePrincipale::" + Riga.AbitazionePrincipale.ToString());
                ddlAbitazionePrincipale.SelectedItem.Selected = false;
                ddlAbitazionePrincipale.Items.FindByValue(Riga.AbitazionePrincipale.ToString()).Selected = true;
                if (Riga.ImpDetrazAbitazPrincipale == -1)
                    txtImpDetrazione.Text = String.Empty;
                else
                    txtImpDetrazione.Text = Riga.ImpDetrazAbitazPrincipale.ToString();

                //log.Debug("devo caricare ddlPossesso::" + Riga.Possesso.ToString());
                ddlPossesso.SelectedItem.Selected = false;
                ddlPossesso.Items.FindByValue(Riga.Possesso.ToString()).Selected = true;
                //log.Debug("devo caricare ddlRiduzione::" + Riga.Riduzione.ToString());
                ddlRiduzione.SelectedItem.Selected = false;
                ddlRiduzione.Items.FindByValue(Riga.Riduzione.ToString()).Selected = true;
                //log.Debug("devo caricare ddlEsclusoEsente::" + Riga.EsclusioneEsenzione.ToString());
                ddlEsclusoEsente.SelectedItem.Selected = false;
                ddlEsclusoEsente.Items.FindByValue(Riga.EsclusioneEsenzione.ToString()).Selected = true;

                if (Riga.AbitazionePrincipaleAttuale.CompareTo(0) == 0)
                {
                    chkAbitPrincipale.Checked = false;
                }
                else
                {
                    chkAbitPrincipale.Checked = true;
                    chkPertinenza.Enabled = false;
                    //txtCodPertinenza.Text = "0";
                }

                if (Riga.NumeroUtilizzatori.CompareTo(0) != 0)
                {
                    txtNumeroUtilizzatori.Text = Riga.NumeroUtilizzatori.ToString();
                }
                //*** 20120629 - IMU ***
                chkColtivatori.Checked = Riga.ColtivatoreDiretto;
                if (Riga.NumeroFigli != 0)
                    txtNumFigli.Text = Riga.NumeroFigli.ToString();
                if (Riga.ListCaricoFigli != null)
                {
                    DataTable dtCaricoFigli = new DataTable("CaricoFigli");
                    dtCaricoFigli.Columns.Add("nFiglio");
                    dtCaricoFigli.Columns.Add("percentuale");
                    dtCaricoFigli.Columns.Add("id");
                    object[] ListPercCarico = new object[3];
                    ListPercCarico.Initialize();
                    foreach (DichManagerICI.CaricoFigliRow myRow in Riga.ListCaricoFigli)
                    {
                        ListPercCarico[0] = "Figlio n." + myRow.nFiglio.ToString();
                        ListPercCarico[1] = myRow.Percentuale;
                        ListPercCarico[2] = myRow.nFiglio.ToString();
                        dtCaricoFigli.Rows.Add(ListPercCarico);
                    }
                    GrdCaricoFigli.DataSource = dtCaricoFigli;
                    GrdCaricoFigli.DataBind();
                    LblCaricoFigli.Style.Add("display", "");
                }
                else
                    LblCaricoFigli.Style.Add("display", "none");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.DettaglioBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
            //*** ***
            //log.Debug ("FINE DettaglioBindDummyDich");
        }
        #endregion
        #region ContribuenteBind
        //*** 201504 - Nuova Gestione anagrafica con form unico ***
        /// <summary>
        /// Esegue il Bind dei Controlli del dettaglio Contribuente
        /// </summary>
        /// <param name="idContrib"></param>
        /// <param name="idTestata"></param>
        private void ContribuenteBind(int idContrib, int idTestata)
        {
            //log.Debug("INIZIO ContribuenteBind");
            try
            {
                if (idContrib <= 0)
                {//log.Debug ("idTestata=" + idTestata);
                    Utility.DichManagerICI.TestataRow TestContrib = new TestataTable(ConstWrapper.sUsername).GetRow(idTestata, -1, ConstWrapper.StringConnection);
                    //log.Debug ("Get Dati Testata OK");
                    idContrib = TestContrib.IDContribuente;
                    /*string annodich = TestContrib.AnnoDichiarazione;
                    lblAnnDichiaraz.Text = annodich;*/
                }
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                hdIdContribuente.Value = idContrib.ToString();
                if (ConstWrapper.HasPlainAnag)
                    ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + idContrib.ToString() + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                else
                {
                    //log.Debug("GetDatiPersona");
                    DettaglioAnagrafica oDettAnagContrib = HelperAnagrafica.GetDatiPersona(long.Parse(idContrib.ToString()));
                    //log.Debug("GetDatiPersona OK");
                    if (oDettAnagContrib != null)
                    {
                        lblCognomeContr.Text = oDettAnagContrib.Cognome;
                        lblNomeContr.Text = oDettAnagContrib.Nome;
                        if (oDettAnagContrib.DataNascita == "00/00/1900")
                        {
                            lblDataNascContr.Text = "";
                        }
                        else
                        {
                            lblDataNascContr.Text = oDettAnagContrib.DataNascita;
                        }
                        lblResidContr.Text = oDettAnagContrib.ViaResidenza + ", " + oDettAnagContrib.CivicoResidenza;
                        lblComuneContr.Text = oDettAnagContrib.ComuneResidenza + " (" + oDettAnagContrib.ProvinciaResidenza + ")";
                    }
                }
                //*** ***
                //log.Debug("devo caricare IDCONTRIB::" + TestContrib.IDContribuente.ToString());
                //log.Debug("devo caricare TXTCONTRIB::" + txtIDContrib.Text);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ContribuenteBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //private void ContribuenteBind(int idTestata)
        //{
        //    //log.Debug("INIZIO ContribuenteBind");
        //    try
        //    {
        //        //log.Debug ("idTestata=" + idTestata);
        //        TestataRow TestContrib = new TestataTable(ConstWrapper.sUsername).GetRow(idTestata, ConstWrapper.StringConnection);
        //        //log.Debug ("Get Dati Testata OK");
        //        hdIdContribuente.Value =  TestContrib.IDContribuente;
        //        //log.Debug("devo caricare IDCONTRIB::" + TestContrib.IDContribuente.ToString());
        //        string annodich = TestContrib.AnnoDichiarazione;
        //        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value.ToString() + "&Azione=" + ConstWrapper.AZIONE_LETTURA);
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ContribuenteBind.errore: ", Err);
        //  Response.Redirect("../PaginaErrore.aspx");
        //    }
        //}
        //*** ***
        #endregion
        #endregion
        #region Abilita
        /// <summary>
        /// Abilita o disabilita i controlli della pagina.
        /// </summary>
        /// <param name="abilita"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        private void Abilita(bool abilita)
        {
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    AbilitaDummy(abilita);
                    string sScript = "";
                    if (abilita)
                    {
                        sScript = "$('#DummyDich').removeAttr('disabled');";
                        RegisterScript(sScript, this.GetType());
                    }
                    else
                    {
                        sScript = "$('#DummyDich').prop('disabled',true);";
                        RegisterScript(sScript, this.GetType());
                    }
                }
                else
                    AbilitaNoDummy(abilita);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.Abilita.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //private void Abilita(bool abilita)
        //{
        //    try
        //    {
        //        if (ConstWrapper.HasDummyDich)
        //            AbilitaDummy(abilita);
        //        else
        //            AbilitaNoDummy(abilita);
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.Abilita.errore: ", Err);
        //        Response.Redirect("../PaginaErrore.aspx");
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abilita"></param>
        /// <revisionhistory></revisionhistory>
        /// <revisionHistory>
        /// <revision date="25/02/2018">
        /// Per allineare la videata al resto dell'applicativo è stato tolto l'inserimento da Territorio (non usato) e messo al suo posto lo stradario.
        /// </revision>
        /// </revisionHistory>
        private void AbilitaDummy(bool abilita)
        {
            try
            {
                //lblNumOrdineDummy.Enabled = abilita;
                //lblNumModelloDummy.Enabled = abilita;
                //lblDataUltimaModificaDummy.Enabled = abilita;
                //lblCaratteristicaDummy.Enabled = abilita;
                //lblStoricoDummy.Enabled = abilita;
                //lblValoreImmobileDummy.Enabled = abilita;
                //lblValoreProvvisorioDummy.Enabled = abilita;
                //lblTipoImmobileDummy.Enabled = abilita;
                //lblCodUIDummy.Enabled = abilita;
                //lbtnCodiceComuneDummy.Enabled = abilita;
                //lblNumeroCivicoDummy.Enabled = abilita;
                //lblEspCivicoDummy.Enabled = abilita;
                //lblBarraDummy.Enabled = abilita;
                //lblInternoDummy.Enabled = abilita;
                //lblScalaDummy.Enabled = abilita;
                //lblPianoDummy.Enabled = abilita;
                //lblNumEcograficoDummy.Enabled = abilita;
                //lblDescrUffRegistroDummy.Enabled = abilita;
                //lblSezioneDummy.Enabled = abilita;
                //lblPartitaCatastaleDummy.Enabled = abilita;
                //lblFoglioDummy.Enabled = abilita;
                //lblNumeroDummy.Enabled = abilita;
                //lblSubalternoDummy.Enabled = abilita;
                //lblCategoriaCatastaleDummy.Enabled = abilita;
                //lblClasseDummy.Enabled = abilita;
                //lblCodRenditaDummy.Enabled = abilita;
                //lblNumProtocolloDummy.Enabled = abilita;
                //lblAnnoDenunciaCatastaleDummy.Enabled = abilita;
                //lblAcquistoDummy.Enabled	= abilita;
                //lblCessioneDummy.Enabled = abilita;
                //lblPercPossessoDummy.Enabled = abilita;
                //lblMesiPossessoDummy.Enabled = abilita;
                //lblAstTipoUtilizzoDummy.Enabled = abilita;
                //lblTipoUtilizzoDummy.Enabled = abilita;
                //lblTipoPossessoDummy.Enabled = abilita;
                //lblMesiEsclusEsenzDummy.Enabled = abilita;
                //lblMesiRiduzioneDummy.Enabled = abilita;
                //lblAbitazPrincipaleDummy.Enabled = abilita;
                //lblImportoDetrazAbitazPrincipDummy.Enabled = abilita;
                //lblPossessoDummy.Enabled = abilita;
                //lblEsclusoEsenteDummy.Enabled = abilita;
                //lblRiduzioneDummy.Enabled = abilita;
                //lblAstCaratteristicaDummy.Enabled = abilita;
                //lblAstFoglioDummy.Enabled = abilita;
                //lblAstNumeroDummy.Enabled = abilita;
                //lblDataFineDummy.Enabled = abilita;
                //lblDataInizioDummy.Enabled = abilita;
                //lblNumUtilizzatoriDummy.Enabled = abilita;
                //lblViaOldDummy.Enabled = abilita;
                //lblComboEstimoDummy.Enabled = abilita;
                //lblRenditaDummy.Enabled = abilita; 
                //lblConsistenzaDummy.Enabled = abilita;
                //lblExruraleDummy.Enabled = abilita;
                //lblnumfigliDummy.Enabled = abilita;
                lblCodViaDummy.Enabled = abilita;

                //*** 20130304 - gestione dati da territorio ***
                LnkNewUIAnaterDummy.Enabled = abilita;
                //*** ***
                lnkApriPertinenzaDummy.Enabled = abilita;
                lnkRenditaDummy.Enabled = abilita;
                lnkValoreDummy.Enabled = abilita;

                txtNumOrdineDummy.Enabled = abilita;
                txtNumModelloDummy.Enabled = abilita;
                txtDataUltimaModifica.Enabled = abilita;
                txtValoreDummy.Enabled = abilita;
                txtCodUIDummy.Enabled = abilita;
                txtViaDummy.Enabled = abilita;
                txtNumCivDummy.Enabled = abilita;
                txtEspCivicoDummy.Enabled = abilita;
                txtBarratoDummy.Enabled = abilita;
                txtInternoDummy.Enabled = abilita;
                txtScalaDummy.Enabled = abilita;
                txtPianoDummy.Enabled = abilita;
                txtNumeroEcograficoDummy.Enabled = abilita;
                txtDescrizioneUffRegistroDummy.Enabled = abilita;
                txtSezioneDummy.Enabled = abilita;
                txtPartitaCatastaleDummy.Enabled = abilita;
                txtFoglioDummy.Enabled = abilita;
                txtNumeroDummy.Enabled = abilita;
                txtSubalternoDummy.Enabled = abilita;
                txtNumProtocolloDummy.Enabled = abilita;
                txtAnnoDenunciaCatastaleDummy.Enabled = abilita;
                txtPercPossessoDummy.Enabled = abilita;
                txtMesiPossessoDummy.Enabled = abilita;
                txtMesiEsclusioneDummy.Enabled = abilita;
                txtMesiRiduzioneDummy.Enabled = abilita;
                txtImpDetrazioneDummy.Enabled = abilita;
                txtDataFineDummy.Enabled = abilita;
                txtDataInizioDummy.Enabled = abilita;
                txtNumeroUtilizzatoriDummy.Enabled = abilita;
                txtNoteIciDummy.Enabled = abilita;
                txtRenditaDummy.Enabled = abilita;
                txtConsistenzaDummy.Enabled = abilita;

                chkStoricoDummy.Enabled = abilita;
                chkValoreProvvisorioDummy.Enabled = abilita;
                chkAbitPrincipaleDummy.Enabled = abilita;
                chkExRuraleDummy.Enabled = abilita;

                ddlCaratteristicaDummy.Enabled = abilita;
                ddlValuta.Enabled = abilita;
                ddlTipoImmobileDummy.Enabled = abilita;
                ddlCategoriaCatastaleDummy.Enabled = abilita;
                ddlClasseDummy.Enabled = abilita;
                ddlAcquistoDummy.Enabled = abilita;
                ddlCessioneDummy.Enabled = abilita;
                //*** 20140509 - TASI ***
                ddlTipoUtilizzoDummy.Enabled = abilita;
                ddlTipoPossessoDummy.Enabled = abilita;
                //*** ***
                ddlAbitazionePrincipaleDummy.Enabled = abilita;
                ddlPossessoDummy.Enabled = abilita;
                ddlEsclusoEsenteDummy.Enabled = abilita;
                ddlRiduzioneDummy.Enabled = abilita;
                ddlCodiceRenditaDummy.Enabled = abilita;
                ddlEstimoDummy.Enabled = abilita;

                //*** 20120629 - IMU ***
                chkColtivatoriDummy.Enabled = abilita;
                txtNumFigliDummy.Enabled = abilita;
                GrdCaricoFigliDummy.Enabled = abilita;
                //*** ***
                //ddlCodiceRenditaDummy.Enabled = abilita;
                //lblAstNumOrdineDummy.Enabled = abilita;
                //chkPertinenzaDummy.Enabled = abilita;
                //cvalValoreImmobileDummy.Enabled = abilita;

                if (abilita.CompareTo(true) == 0)
                {
                    //lblCodViaDummy.Attributes.Add("onclick", "ApriStradario();");
                    //lblCodViaDummy.Attributes.Add("style", "text-decoration:underline;cursor:hand;");
                    //*** 20130304 - gestione dati da territorio ***
                    //LnkNewUIAnaterDummy.Attributes.Add("onclick", "ShowRicUIAnater()");
                    //LnkNewUIAnaterDummy.Attributes.Add("style", "text-decoration:underline;cursor:hand;");
                    //LnkNewUIAnaterDummy.ToolTip = "Nuovo Immobile da " + ConfigurationManager.AppSettings["NameSistemaTerritorio"].ToString();
                    LnkNewUIAnaterDummy.Attributes.Add("onclick", "ApriStradario()");
                    LnkNewUIAnaterDummy.ToolTip = "Apri stradario";
                    //*** ***
                }

                // se è un'abitazione principale non devo abilitare la pertinenza
                if (chkAbitPrincipaleDummy.Checked)
                {
                    chkPertinenzaDummy.Enabled = false;
                    lnkApriPertinenzaDummy.Enabled = false;
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.AbilitaDummy.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        private void AbilitaNoDummy(bool abilita)
        {
            try
            {
                lblCodViaNoDummy.Enabled = abilita;

                //*** 20130304 - gestione dati da territorio ***
                LnkNewUIAnaterNoDummy.Enabled = abilita;
                //*** ***
                lnkApriPertinenzaNoDummy.Enabled = abilita;
                lnkRenditaNoDummy.Enabled = abilita;
                lnkValoreNoDummy.Enabled = abilita;

                txtNumOrdineNoDummy.Enabled = abilita;
                txtNumModelloNoDummy.Enabled = abilita;
                txtDataUltimaModifica.Enabled = abilita;
                txtValoreNoDummy.Enabled = abilita;
                txtCodUINoDummy.Enabled = abilita;
                txtViaNoDummy.Enabled = abilita;
                txtNumCivNoDummy.Enabled = abilita;
                txtEspCivicoNoDummy.Enabled = abilita;
                txtBarratoNoDummy.Enabled = abilita;
                txtInternoNoDummy.Enabled = abilita;
                txtScalaNoDummy.Enabled = abilita;
                txtPianoNoDummy.Enabled = abilita;
                txtNumeroEcograficoNoDummy.Enabled = abilita;
                txtDescrizioneUffRegistroNoDummy.Enabled = abilita;
                txtSezioneNoDummy.Enabled = abilita;
                txtPartitaCatastaleNoDummy.Enabled = abilita;
                txtFoglioNoDummy.Enabled = abilita;
                txtNumeroNoDummy.Enabled = abilita;
                txtSubalternoNoDummy.Enabled = abilita;
                txtNumProtocolloNoDummy.Enabled = abilita;
                txtAnnoDenunciaCatastaleNoDummy.Enabled = abilita;
                txtPercPossessoNoDummy.Enabled = abilita;
                txtMesiPossessoNoDummy.Enabled = abilita;
                txtMesiEsclusioneNoDummy.Enabled = abilita;
                txtMesiRiduzioneNoDummy.Enabled = abilita;
                txtImpDetrazioneNoDummy.Enabled = abilita;
                txtDataFineNoDummy.Enabled = abilita;
                txtDataInizioNoDummy.Enabled = abilita;
                txtNumeroUtilizzatoriNoDummy.Enabled = abilita;
                txtNoteIciNoDummy.Enabled = abilita;
                txtRenditaNoDummy.Enabled = abilita;
                txtConsistenzaNoDummy.Enabled = abilita;

                chkStoricoNoDummy.Enabled = abilita;
                chkValoreProvvisorioNoDummy.Enabled = abilita;
                chkAbitprincipaleNoDummy.Enabled = abilita;
                chkExruraleNoDummy.Enabled = abilita;

                ddlCaratteristicaNoDummy.Enabled = abilita;
                ddlValuta.Enabled = abilita;
                ddlTipoImmobileNoDummy.Enabled = abilita;
                ddlCategoriaCatastaleNoDummy.Enabled = abilita;
                ddlClasseNoDummy.Enabled = abilita;
                ddlAcquistoNoDummy.Enabled = abilita;
                ddlCessioneNoDummy.Enabled = abilita;
                //*** 20140509 - TASI ***
                ddlTipoUtilizzoNoDummy.Enabled = abilita;
                ddlTipoPossessoNoDummy.Enabled = abilita;
                //*** ***
                ddlAbitazionePrincipaleNoDummy.Enabled = abilita;
                ddlPossessoNoDummy.Enabled = abilita;
                ddlEsclusoEsenteNoDummy.Enabled = abilita;
                ddlRiduzioneNoDummy.Enabled = abilita;
                ddlCodiceRenditaNoDummy.Enabled = abilita;
                ddlEstimoNoDummy.Enabled = abilita;

                //*** 20120629 - IMU ***
                chkcoltivatoriNoDummy.Enabled = abilita;
                txtnumfigliNoDummy.Enabled = abilita;
                GrdCaricoFigliNoDummy.Enabled = abilita;
                //*** ***

                if (abilita.CompareTo(true) == 0)
                {
                    lblCodViaNoDummy.Attributes.Add("onclick", "ApriStradario();");
                    lblCodViaNoDummy.Attributes.Add("style", "text-decoration:underline;cursor:hand;");
                    //*** 20130304 - gestione dati da territorio ***
                    LnkNewUIAnaterNoDummy.Attributes.Add("onclick", "ShowRicUIAnater()");
                    LnkNewUIAnaterNoDummy.Attributes.Add("style", "text-decoration:underline;cursor:hand;");
                    LnkNewUIAnaterNoDummy.ToolTip = "Nuovo Immobile da " + ConstWrapper.NameSistemaTerritorio;
                    //*** ***
                }

                // se è un'abitazione principale non devo abilitare la pertinenza
                if (chkAbitprincipaleNoDummy.Checked)
                {
                    chkPertinenzaNoDummy.Enabled = false;
                    lnkApriPertinenzaNoDummy.Enabled = false;
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.AbilitaNoDummy.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #endregion
        #region DuplicaImmobile
        /// <summary>
        /// Salva o modifica i dati dell'immobile.
        /// </summary>
        /// <returns></returns>
        /*private bool DuplicaImmobile(string DataValiditaAtto)
        {
            bool retval = true;
            Utility.DichManagerICI.OggettiRow riga = new Utility.DichManagerICI.OggettiRow();
            //*** 20130923 - gestione modifiche tributarie ***
            Utility.ModificheTributarie FncModificheTributarie=new Utility.ModificheTributarie();
                int idUI = 0;
            //*** ***

            IDImmobile=0;
            //try{
            riga.AnnoDenunciaCatastale = txtAnnoDenunciaCatastale.Text;
            riga.Annullato = false;
            riga.Barrato = txtBarrato.Text;
            riga.Bonificato = false;
            riga.Caratteristica = int.Parse(ddlCaratteristica.SelectedValue);
            riga.CodCategoriaCatastale = ddlCategoriaCatastale.SelectedValue;
            riga.CodClasse = ddlClasse.SelectedValue;
            riga.CodComune = Convert.ToInt32(ConstWrapper.CodiceEnte);
            riga.Comune = ConstWrapper.DescrizioneEnte;
            //	riga.CodComune = 0;
            //	riga.Comune = txtCodiceComune.Text;
            riga.CodRendita = ddlCodiceRendita.SelectedValue;
            if (txtCodUI.Text != "")
            {
                riga.CodUI = int.Parse(txtCodUI.Text);
            }
            else
            {
                riga.CodUI = -1;
            }
            riga.CodVia = txtCodVia.Text;
			
            if (txtVia.Text.CompareTo("") != 0)
            {
                //	se via è popolato
                riga.Via = txtVia.Text;
            }
            else
            {
                // prendo la via che non ha codifica
                riga.Via = lblViaOld.Text.Replace("(","").Replace(")","");
            }
            //riga.Via = txtVia.Text;
            riga.DataFineValidità = DateTime.MinValue;
            riga.DataInizioValidità = DateTime.Now;
            riga.DescrUffRegistro = txtDescrizioneUffRegistro.Text;
            riga.Ente = ConstWrapper.CodiceEnte;
            riga.EspCivico = txtEspCivico.Text;
            riga.FlagValoreProvv = chkValoreProvvisorio.Checked;
            riga.Foglio = txtFoglio.Text;
            log.Debug("txtDataUltimaModifica.Text::"+txtDataUltimaModifica.Text);
            try{riga.DataUltimaModifica = Convert.ToDateTime(txtDataUltimaModifica.Text);}
            catch {riga.DataUltimaModifica=DateTime.Now;}
            riga.IdTestata = this.IDTestata;
            riga.IDValuta = 1;//int.Parse(ddlValuta.SelectedValue);
            riga.Interno = txtInterno.Text;
            riga.Numero = txtNumero.Text;
            if (txtNumCiv.Text != "")
            {
                riga.NumeroCivico = int.Parse(txtNumCiv.Text);
            }
            else
            {
                riga.NumeroCivico = -1;
            }

            if (txtNumeroEcografico.Text != "")
            {
                riga.NumeroEcografico = int.Parse(txtNumeroEcografico.Text);
            }
            else
            {
                riga.NumeroEcografico = -1;
            }
            riga.NumeroModello = txtNumModello.Text;
            riga.NumeroOrdine = txtNumOrdine.Text;
            riga.NumeroProtCatastale = txtNumProtocollo.Text;
            riga.Operatore = ConstWrapper.sUsername;
            if (txtPartitaCatastale.Text != "")
            {
                riga.PartitaCatastale = int.Parse(txtPartitaCatastale.Text);
            }
            else
            {
                riga.PartitaCatastale = -1;
            }
			
            riga.Piano = txtPiano.Text;
            riga.Scala = txtScala.Text;
            riga.Sezione = txtSezione.Text;
            riga.Storico = chkStorico.Checked;
            if (txtSubalterno.Text != "")
            {
                riga.Subalterno = int.Parse(txtSubalterno.Text);
            }
            else
            {
                riga.Subalterno = -1;
            }
            // ora che ho nascosto prendo tipoImmobile = CodCaratteristica
            riga.TipoImmobile = int.Parse(ddlCodiceRendita.SelectedValue);
			
            riga.TitoloAcquisto = int.Parse(ddlAcquisto.SelectedValue);
            riga.TitoloCessione = int.Parse(ddlCessione.SelectedValue);
			
            if (txtValore.Text != "")
            {
                riga.ValoreImmobile = Convert.ToDecimal(txtValore.Text); //float.Parse(txtValore.Text);
            }
            else
            {
                riga.ValoreImmobile = -1;
            }

            //*** 20131003 - gestione atti compravendita ***
            if (DataValiditaAtto!=string.Empty)
            {
                log.Debug("Convert.ToDateTime(DataValiditaAtto).AddDays(-1)::"+Convert.ToDateTime(DataValiditaAtto).AddDays(-1).ToString());
                riga.DataFine= Convert.ToDateTime(DataValiditaAtto).AddDays(-1);
            }
            else
            {
                if (txtDataFine.Text.CompareTo("") != 0)
                {
                    log.Debug("Convert.ToDateTime(txtDataFine.Text)::"+Convert.ToDateTime(txtDataFine.Text).ToString());
                    riga.DataFine= Convert.ToDateTime(txtDataFine.Text);
                }
                else
                {
                    riga.DataFine = DateTime.MaxValue;
                }
            }
            //*** ***
            log.Debug("Convert.ToDateTime(txtDataInizio.Text)::"+Convert.ToDateTime(txtDataInizio.Text).ToString());
            riga.DataInizio = Convert.ToDateTime(txtDataInizio.Text);

            if ((txtCodPertinenza.Text.CompareTo("") != 0) && (txtCodPertinenza.Text.CompareTo("-1")!=0) && (this.IDImmobile.CompareTo(int.Parse(txtCodPertinenza.Text)) != 0))
            {
                riga.IDImmobilePertinente = int.Parse(txtCodPertinenza.Text);
                Utility.DichManagerICI.OggettiRow DettaglioAbitPrinc = new OggettiTable(ConstWrapper.sUsername).GetRow(riga.IDImmobilePertinente, ConstWrapper.StringConnection);
                DettaglioAbitPrinc.IDImmobilePertinente = riga.IDImmobilePertinente;
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                //retval = new OggettiTable(ConstWrapper.sUsername).Modify(DettaglioAbitPrinc);
                retval = new Utility.DichManagerICI().SetOggetti(ConstWrapper.StringConnection, DettaglioAbitPrinc, out idUI);
                //*** ***
            }
            else
            {
                if (this.IDImmobile.CompareTo(int.Parse(txtCodPertinenza.Text)) != 0)
                {
                    riga.IDImmobilePertinente = -1;
                }
                else
                {
                    riga.IDImmobilePertinente = int.Parse(txtCodPertinenza.Text);
                }
            }

            riga.NoteIci = txtNoteIci.Text;

            if (txtConsistenza.Text == "")
            {
                riga.Consistenza = decimal.Parse("-1");
            }
            else
            {
                riga.Consistenza = decimal.Parse(txtConsistenza.Text);
            }

            if (txtRendita.Text == "")
            {
                riga.Rendita = decimal.Parse("-1");
            }
            else
            {
                riga.Rendita = decimal.Parse(txtRendita.Text);
            }

            riga.Zona = ddlEstimo.SelectedValue.ToString();

            // ORA EFFETTUO IL SALVATAGGIO DELL'IMMOBILE
            // SE HO L'ID MODIFICO ALTRIMENTI INSERISCO
            if(this.IDImmobile > 0)
            {
                riga.ID = this.IDImmobile;
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                //retval = new OggettiTable(ConstWrapper.sUsername).Modify(riga);
                retval = new Utility.DichManagerICI().SetOggetti(ConstWrapper.StringConnection, riga, out idUI);
                //*** ***
                //*** 20130923 - gestione modifiche tributarie ***
                if (riga.Foglio!=Session["Foglio"].ToString() || riga.Numero!=Session["Numero"].ToString() || riga.Subalterno.ToString()!=Session["Subalterno"].ToString())
                {
                    log.Debug("VariazioneRiferimentiCatastali");
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                else if (riga.DataInizio.ToShortDateString()!=Session["DataInizio"].ToString() || riga.DataFine.ToShortDateString()!=Session["DataFine"].ToString())
                {
                    log.Debug("VariazionePeriodo::" + riga.DataInizio.ToString() +"::"+ Session["DataInizio"].ToString() +"::"+ riga.DataFine.ToString() +"::"+Session["DataFine"].ToString());
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                else if (riga.CodRendita!=Session["CodRendita"].ToString())
                {
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCodiceRendita, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCodiceRendita + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                else if (riga.CodCategoriaCatastale!=Session["CodCategoriaCatastale"].ToString())
                {
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCategoriaCatastale, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCategoriaCatastale + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                else if (riga.CodClasse!=Session["CodClasse"].ToString())
                {
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneClasse, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneClasse + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                else if (riga.Consistenza.ToString()!=Session["Consistenza"].ToString())
                {
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneConsistenza, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneConsistenza + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                else if (riga.Rendita.ToString()!=Session["Rendita"].ToString() || riga.ValoreImmobile.ToString()!=Session["ValoreImmobile"].ToString())
                {
                    if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRenditaValore, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                    {
                        log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRenditaValore + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                    }
                }
                //*** ***
            }
            else
            {
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                //int idImmobile = 0;
                //retval = new OggettiTable(ConstWrapper.sUsername).Insert(riga, out idImmobile);
                //this.IDImmobile = idImmobile;
                retval = new Utility.DichManagerICI().SetOggetti(ConstWrapper.StringConnection, riga, out idUI);
                this.IDImmobile = idUI;
                //*** ***
                //*** 20130923 - gestione modifiche tributarie ***
                if (FncModificheTributarie.SetModificheTributarie(ConfigurationManager.AppSettings["connectionStringSQLOPENgov"], (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false )
                {
                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                }
                //*** ***
            }
             }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.DuplicaImmobile.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }

            return retval;
        }*/
        private bool DuplicaImmobile(string DataValiditaAtto)
        {
            bool retval = true;
            Utility.DichManagerICI.OggettiRow riga = new Utility.DichManagerICI.OggettiRow();
            //*** 20130923 - gestione modifiche tributarie ***
            Utility.ModificheTributarie FncModificheTributarie = new Utility.ModificheTributarie();
            int idUI = 0;
            //*** ***
            bool HasPertinenza = false;

            IDImmobile = 0;

            retval = ImmobileLoad(DataValiditaAtto, out riga, out HasPertinenza);
            try
            {
                if (retval)
                {
                    if (HasPertinenza)
                    {
                        riga.IDImmobilePertinente = int.Parse(txtCodPertinenza.Text);
                        Utility.DichManagerICI.OggettiRow DettaglioAbitPrinc = new OggettiTable(ConstWrapper.sUsername).GetRow(riga.IDImmobilePertinente, ConstWrapper.StringConnection);
                        DettaglioAbitPrinc.IDImmobilePertinente = riga.IDImmobilePertinente;
                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //retval = new OggettiTable(ConstWrapper.sUsername).Modify(DettaglioAbitPrinc);
                        // non aggiorno altrimenti cancello abitazione principale
                        //retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_UPDATE, DettaglioAbitPrinc, DettaglioAbitPrinc.IdTestata, out idUI);
                        //*** ***
                    }

                    // ORA EFFETTUO IL SALVATAGGIO DELL'IMMOBILE
                    // SE HO L'ID MODIFICO ALTRIMENTI INSERISCO
                    if (this.IDImmobile > 0)
                    {
                        riga.ID = this.IDImmobile;
                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //retval = new OggettiTable(ConstWrapper.sUsername).Modify(riga);
                        retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_UPDATE, riga, riga.IdTestata, out idUI);
                        //*** ***
                        //*** 20130923 - gestione modifiche tributarie ***
                        if (riga.Foglio != Session["Foglio"].ToString() || riga.Numero != Session["Numero"].ToString() || riga.Subalterno.ToString() != Session["Subalterno"].ToString())
                        {
                            log.Debug("VariazioneRiferimentiCatastali");
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.DataInizio.ToShortDateString() != Session["DataInizio"].ToString() || riga.DataFine.ToShortDateString() != Session["DataFine"].ToString())
                        {
                            log.Debug("VariazionePeriodo::" + riga.DataInizio.ToString() + "::" + Session["DataInizio"].ToString() + "::" + riga.DataFine.ToString() + "::" + Session["DataFine"].ToString());
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.CodRendita != Session["CodRendita"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCodiceRendita, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCodiceRendita + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.CodCategoriaCatastale != Session["CodCategoriaCatastale"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCategoriaCatastale, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCategoriaCatastale + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.CodClasse != Session["CodClasse"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneClasse, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneClasse + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.Consistenza.ToString() != Session["Consistenza"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneConsistenza, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneConsistenza + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.Rendita.ToString() != Session["Rendita"].ToString() || riga.ValoreImmobile.ToString() != Session["ValoreImmobile"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRenditaValore, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRenditaValore + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        //*** ***
                    }
                    else
                    {
                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //int idImmobile = 0;
                        //retval = new OggettiTable(ConstWrapper.sUsername).Insert(riga, out idImmobile);
                        //this.IDImmobile = idImmobile;
                        retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_NEW, riga, riga.IdTestata, out idUI);
                        this.IDImmobile = idUI;
                        //*** ***
                        //*** 20130923 - gestione modifiche tributarie ***
                        if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                        {
                            log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                        }
                        //*** ***
                    }
                }
                return retval;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.DuplicaImmobile.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }
        #endregion
        #region SalvaModificaImmobile
        /// <summary>
        /// Salva o modifica i dati dell'immobile.
        /// </summary>
        /// <returns></returns>
        private bool SalvaModificaImmobile()
        {
            bool retval = true;
            int idImmobile = 0;
            bool HasPertinenza = false;
            try
            {
                Utility.DichManagerICI.OggettiRow riga = new Utility.DichManagerICI.OggettiRow();
                //*** 20130923 - gestione modifiche tributarie ***
                Utility.ModificheTributarie FncModificheTributarie = new Utility.ModificheTributarie();
                //*** ***
                retval = ImmobileLoad("", out riga, out HasPertinenza);
                if (retval)
                {
                    log.Debug("ho caricato i dati dal form");
                    //if (HasPertinenza)
                    //{
                    //    log.Debug("ho pertinenza");
                    //    Utility.DichManagerICI.OggettiRow DettaglioAbitPrinc = new OggettiTable(ConstWrapper.sUsername).GetRow(riga.IDImmobilePertinente, ConstWrapper.StringConnection);
                    //    DettaglioAbitPrinc.IDImmobilePertinente = riga.IDImmobilePertinente;
                    //    //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                    //    //retval = new OggettiTable(ConstWrapper.sUsername).Modify(DettaglioAbitPrinc);
                    //    retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_UPDATE, DettaglioAbitPrinc, DettaglioAbitPrinc.IdTestata, out idImmobile);
                    //    //*** ***
                    //}
                    // ORA EFFETTUO IL SALVATAGGIO DELL'IMMOBILE SE HO L'ID MODIFICO ALTRIMENTI INSERISCO
                    if (this.IDImmobile > 0)
                    {
                        log.Debug("modifico");
                        riga.ID = this.IDImmobile;
                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //retval = new OggettiTable(ConstWrapper.sUsername).Modify(riga);
                        retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_UPDATE, riga, riga.IdTestata, out idImmobile);
                        this.IDImmobile = idImmobile;
                        //*** ***
                        //*** 20130923 - gestione modifiche tributarie ***
                        try
                        {
                            log.Debug("riga.Foglio::" + riga.Foglio); log.Debug("Session[Foglio]:." + Session["Foglio"].ToString());
                            log.Debug("riga.Numero::" + riga.Numero); log.Debug("Session[Numero]::" + Session["Numero"].ToString());
                            log.Debug("riga.Subalterno::" + riga.Subalterno.ToString()); log.Debug("Session[Subalterno]::" + Session["Subalterno"].ToString());
                            log.Debug("riga.DataInizio::" + riga.DataInizio.ToString()); log.Debug("Session[DataInizio]::" + Session["DataInizio"].ToString());
                            log.Debug("riga.DataFine::" + riga.DataFine.ToString()); log.Debug("Session[DataFine]::" + Session["DataFine"].ToString());
                            log.Debug("riga.CodRendita::" + riga.CodRendita.ToString()); log.Debug("Session[CodRendita]::" + Session["CodRendita"].ToString());
                            log.Debug("riga.CodCategoriaCatastale::" + riga.CodCategoriaCatastale.ToString()); log.Debug("Session[CodCategoriaCatastale]::" + Session["CodCategoriaCatastale"].ToString());
                            log.Debug("riga.CodClasse::" + riga.CodClasse.ToString()); log.Debug("Session[CodClasse]::" + Session["CodClasse"].ToString());
                            log.Debug("riga.Consistenza::" + riga.Consistenza.ToString()); log.Debug("Session[Consistenza]::" + Session["Consistenza"].ToString());
                            log.Debug("riga.Rendita::" + riga.Rendita.ToString()); log.Debug("Session[Rendita]::" + Session["Rendita"].ToString());
                            log.Debug("riga.ValoreImmobile::" + riga.ValoreImmobile.ToString()); log.Debug("Session[ValoreImmobile]::" + Session["ValoreImmobile"].ToString());

                            log.Debug("controllo FG+NUM+SUB");
                            if (riga.Foglio != Session["Foglio"].ToString() || riga.Numero != Session["Numero"].ToString() || riga.Subalterno.ToString() != Session["Subalterno"].ToString())
                            {
                                log.Debug("VariazioneRiferimentiCatastali");
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                            log.Debug("controllo INZIO+FINE");
                            if (riga.DataInizio.ToShortDateString() != DateTime.Parse(Session["DataInizio"].ToString()).ToShortDateString() || riga.DataFine.ToShortDateString() != DateTime.Parse(Session["DataFine"].ToString()).ToShortDateString())
                            {
                                log.Debug("VariazionePeriodo::" + riga.DataInizio.ToString() + "::" + Session["DataInizio"].ToString() + "::" + riga.DataFine.ToString() + "::" + Session["DataFine"].ToString());
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                            log.Debug("controllo CODRENDITA");
                            if (riga.CodRendita != Session["CodRendita"].ToString())
                            {
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCodiceRendita, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCodiceRendita + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                            log.Debug("controllo CATEGORIA");
                            if (riga.CodCategoriaCatastale != Session["CodCategoriaCatastale"].ToString())
                            {
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCategoriaCatastale, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneCategoriaCatastale + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                            log.Debug("controllo CLASSE");
                            if (riga.CodClasse != Session["CodClasse"].ToString())
                            {
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneClasse, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneClasse + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                            log.Debug("controllo CONSISTENZA");
                            if (riga.Consistenza.ToString() != Session["Consistenza"].ToString())
                            {
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneConsistenza, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneConsistenza + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                            log.Debug("controllo RENDITA+VALORE");
                            if (riga.Rendita.ToString() != Session["Rendita"].ToString() || riga.ValoreImmobile.ToString() != Session["ValoreImmobile"].ToString())
                            {
                                if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRenditaValore, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, riga.ID, DateTime.MaxValue) == false)
                                {
                                    log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRenditaValore + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.SalvaModificaImmobile.errore: ", ex);
                            Response.Redirect("../PaginaErrore.aspx");
                        }
                        //*** ***
                    }
                    else
                    {
                        log.Debug("inserisco nuovo");
                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //retval = new OggettiTable(ConstWrapper.sUsername).Insert(riga, out idImmobile);
                        retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_NEW, riga, riga.IdTestata, out idImmobile);
                        //*** ***
                        this.IDImmobile = idImmobile;
                        //*** 20130923 - gestione modifiche tributarie ***
                        int idModTrib = 0;
                        if (hdTypeOperation.Value.ToUpper() == "GESTIONE")
                            idModTrib = (int)Utility.ModificheTributarie.ModificheTributarieCausali.NuovaDichiarazione;
                        else
                            idModTrib = (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali;
                        if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, idModTrib, riga.Foglio, riga.Numero, riga.Subalterno.ToString(), DateTime.Now, ConstWrapper.sUsername, idImmobile, DateTime.MaxValue) == false)
                        {
                            log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + idModTrib.ToString() + "::@FOGLIO=" + riga.Foglio + "::@NUMERO=" + riga.Numero + "::@SUBALTERNO=" + riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + ConstWrapper.sUsername + "::@IDOGGETTOTRIBUTI=" + idImmobile + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                        }
                        //*** ***
                    }
                    //controllo di congruenza ente
                    if (!new TestataTable(riga.Operatore).CongruenzaEnte(riga.IdTestata))
                    {
                        string sScript = "GestAlert('a', 'Problemi in inserimento Oggetti contattare l\'assistenza tecnica.');";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.SalvaModificaImmobile.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
            return retval;
        }
        private bool ImmobileLoad(string DataValiditaAtto, out Utility.DichManagerICI.OggettiRow riga, out bool HasPertinenza)
        {
            riga = new Utility.DichManagerICI.OggettiRow();
            HasPertinenza = false;
            TextBox txtAnnoDenunciaCatastale = null;
            TextBox txtBarrato = null;
            TextBox txtCodUI = null;
            TextBox txtVia = null;
            TextBox txtDescrUffRegistro = null;
            TextBox txtEspCivico = null;
            TextBox txtFoglio = null;
            TextBox txtInterno = null;
            TextBox txtNumero = null;
            TextBox txtNumeroCivico = null;
            TextBox txtNumeroEcografico = null;
            TextBox txtNumeroModello = null;
            TextBox txtNumeroOrdine = null;
            TextBox txtNumeroProtCatastale = null;
            TextBox txtPartitaCatastale = null;
            TextBox txtPiano = null;
            TextBox txtScala = null;
            TextBox txtSezione = null;
            TextBox txtSubalterno = null;
            TextBox txtValoreImmobile = null;
            TextBox txtDataFine = null;
            TextBox txtDataInizio = null;
            TextBox txtNoteIci = null;
            TextBox txtConsistenza = null;
            TextBox txtRendita = null;

            DropDownList ddlCaratteristica = null;
            DropDownList ddlCodCategoriaCatastale = null;
            DropDownList ddlCodClasse = null;
            DropDownList ddlCodRendita = null;
            DropDownList ddlZona = null;
            DropDownList ddlAcquisto = null;
            DropDownList ddlCessione = null;

            Label lblVia = null;

            CheckBox chkFlagValoreProvv = null;
            CheckBox chkStorico = null;
            CheckBox chkExRurale = null;

            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    txtAnnoDenunciaCatastale = txtAnnoDenunciaCatastaleDummy;
                    txtBarrato = txtBarratoDummy;
                    txtCodUI = txtCodUIDummy;
                    txtVia = txtViaDummy;
                    txtDescrUffRegistro = txtDescrizioneUffRegistroDummy;
                    txtEspCivico = txtEspCivicoDummy;
                    txtFoglio = txtFoglioDummy;
                    txtInterno = txtInternoDummy;
                    txtNumero = txtNumeroDummy;
                    txtNumeroCivico = txtNumCivDummy;
                    txtNumeroEcografico = txtNumeroEcograficoDummy;
                    txtNumeroModello = txtNumModelloDummy;
                    txtNumeroOrdine = txtNumOrdineDummy;
                    txtNumeroProtCatastale = txtNumProtocolloDummy;
                    txtPartitaCatastale = txtPartitaCatastaleDummy;
                    txtPiano = txtPianoDummy;
                    txtScala = txtScalaDummy;
                    txtSezione = txtSezioneDummy;
                    txtSubalterno = txtSubalternoDummy;
                    txtValoreImmobile = txtValoreDummy;
                    txtDataFine = txtDataFineDummy;
                    txtDataInizio = txtDataInizioDummy;
                    txtNoteIci = txtNoteIciDummy;
                    txtConsistenza = txtConsistenzaDummy;
                    txtRendita = txtRenditaDummy;

                    ddlCaratteristica = ddlCaratteristicaDummy;
                    ddlCodCategoriaCatastale = ddlCategoriaCatastaleDummy;
                    ddlCodClasse = ddlClasseDummy;
                    ddlCodRendita = ddlCodiceRenditaDummy;
                     ddlZona = ddlEstimoDummy;
                    ddlAcquisto = ddlAcquistoDummy;
                    ddlCessione = ddlCessioneDummy;

                    lblVia = lblViaOldDummy;

                    chkFlagValoreProvv = chkValoreProvvisorioDummy;
                    chkStorico = chkStoricoDummy;
                    chkExRurale = chkExRuraleDummy;
                }
                else
                {
                    txtAnnoDenunciaCatastale = txtAnnoDenunciaCatastaleNoDummy;
                    txtBarrato = txtBarratoNoDummy;
                    txtCodUI = txtCodUINoDummy;
                    txtVia = txtViaNoDummy;
                    txtDescrUffRegistro = txtDescrizioneUffRegistroNoDummy;
                    txtEspCivico = txtEspCivicoNoDummy;
                    txtFoglio = txtFoglioNoDummy;
                    txtInterno = txtInternoNoDummy;
                    txtNumero = txtNumeroNoDummy;
                    txtNumeroCivico = txtNumCivNoDummy;
                    txtNumeroEcografico = txtNumeroEcograficoNoDummy;
                    txtNumeroModello = txtNumModelloNoDummy;
                    txtNumeroOrdine = txtNumOrdineNoDummy;
                    txtNumeroProtCatastale = txtNumProtocolloNoDummy;
                    txtPartitaCatastale = txtPartitaCatastaleNoDummy;
                    txtPiano = txtPianoNoDummy;
                    txtScala = txtScalaNoDummy;
                    txtSezione = txtSezioneNoDummy;
                    txtSubalterno = txtSubalternoNoDummy;
                    txtValoreImmobile = txtValoreNoDummy;
                    txtDataFine = txtDataFineNoDummy;
                    txtDataInizio = txtDataInizioNoDummy;
                    txtNoteIci = txtNoteIciNoDummy;
                    txtConsistenza = txtConsistenzaNoDummy;
                    txtRendita = txtRenditaNoDummy;

                    ddlCaratteristica = ddlCaratteristicaNoDummy;
                    ddlCodCategoriaCatastale = ddlCategoriaCatastaleNoDummy;
                    ddlCodClasse = ddlClasseNoDummy;
                    ddlCodRendita = ddlCodiceRenditaNoDummy;
                      ddlZona = ddlEstimoNoDummy;
                    ddlAcquisto = ddlAcquistoNoDummy;
                    ddlCessione = ddlCessioneNoDummy;

                    lblVia = lblViaOldNoDummy;

                    chkFlagValoreProvv = chkValoreProvvisorioNoDummy;
                    chkStorico = chkStoricoNoDummy;
                    chkExRurale = chkExruraleNoDummy;
                }
                riga.AnnoDenunciaCatastale = txtAnnoDenunciaCatastale.Text;
                riga.Annullato = false;
                riga.Barrato = txtBarrato.Text;
                riga.Bonificato = false;
                riga.Caratteristica = int.Parse(ddlCaratteristica.SelectedValue);
                riga.CodCategoriaCatastale = ddlCodCategoriaCatastale.SelectedValue;
                riga.CodClasse = ddlCodClasse.SelectedValue;
                riga.CodComune = Convert.ToInt32(ConstWrapper.CodiceEnte);
                riga.Comune = ConstWrapper.DescrizioneEnte;
                //	riga.CodComune = 0;
                //	riga.Comune = txtCodiceComune.Text;
                riga.CodRendita = ddlCodRendita.SelectedValue;
                if (txtCodUI.Text != "")
                {
                    riga.CodUI = txtCodUI.Text;
                }
                else
                {
                    riga.CodUI = "-1";
                }
                riga.CodVia = txtCodVia.Text;

                if (txtVia.Text.CompareTo("") != 0)
                {
                    //	se via è popolato
                    riga.Via = txtVia.Text;
                }
                else
                {
                    // prendo la via che non ha codifica
                    riga.Via = lblVia.Text.Replace("(", "").Replace(")", "");
                }
                //riga.Via = txtVia.Text;
                riga.DataFineValidità = DateTime.MinValue;
                riga.DataInizioValidità = DateTime.Now;
                riga.DescrUffRegistro = txtDescrUffRegistro.Text;
                riga.Ente = ConstWrapper.CodiceEnte;
                riga.EspCivico = txtEspCivico.Text;
                riga.FlagValoreProvv = chkFlagValoreProvv.Checked;
                riga.Foglio = txtFoglio.Text;
                if (txtDataUltimaModifica.Text != string.Empty)
                    try { riga.DataUltimaModifica = Convert.ToDateTime(txtDataUltimaModifica.Text); }
                    catch { riga.DataUltimaModifica = DateTime.Now; }
                else
                    riga.DataUltimaModifica = DateTime.Now;
                riga.IdTestata = this.IDTestata;
                riga.IDValuta = 1;//int.Parse(ddlValuta.SelectedValue);
                riga.Interno = txtInterno.Text;
                riga.Numero = txtNumero.Text;
                if (txtNumeroCivico.Text != "")
                {
                    riga.NumeroCivico = int.Parse(txtNumeroCivico.Text);
                }
                else
                {
                    riga.NumeroCivico = -1;
                }

                if (txtNumeroEcografico.Text != "")
                {
                    riga.NumeroEcografico = int.Parse(txtNumeroEcografico.Text);
                }
                else
                {
                    riga.NumeroEcografico = -1;
                }
                riga.NumeroModello = txtNumeroModello.Text;
                riga.NumeroOrdine = txtNumeroOrdine.Text;
                riga.NumeroProtCatastale = txtNumeroProtCatastale.Text;
                riga.Operatore = ConstWrapper.sUsername;
                if (txtPartitaCatastale.Text != "")
                {
                    riga.PartitaCatastale = int.Parse(txtPartitaCatastale.Text);
                }
                else
                {
                    riga.PartitaCatastale = -1;
                }
                riga.Piano = txtPiano.Text;
                riga.Scala = txtScala.Text;
                riga.Sezione = txtSezione.Text;
                riga.Storico = chkStorico.Checked;
                riga.ExRurale = chkExRurale.Checked;
                if (txtSubalterno.Text != "")
                {
                    riga.Subalterno = int.Parse(txtSubalterno.Text);
                }
                else
                {
                    riga.Subalterno = -1;
                }
                //ora che ho nascosto prendo tipoImmobile = CodCaratteristica
                riga.TipoImmobile = int.Parse(ddlCodRendita.SelectedValue);

                riga.TitoloAcquisto = int.Parse(ddlAcquisto.SelectedValue);
                riga.TitoloCessione = int.Parse(ddlCessione.SelectedValue);

                if (txtValoreImmobile.Text != "")
                {
                    riga.ValoreImmobile = Convert.ToDecimal(txtValoreImmobile.Text); //float.Parse(txtValore.Text);
                }
                else
                {
                    riga.ValoreImmobile = -1;
                }
                //*** 20131003 - gestione atti compravendita ***
                if (DataValiditaAtto != string.Empty)
                {
                    log.Debug("Convert.ToDateTime(DataValiditaAtto).AddDays(-1)::" + Convert.ToDateTime(DataValiditaAtto).AddDays(-1).ToString());
                    riga.DataFine = Convert.ToDateTime(DataValiditaAtto).AddDays(-1);
                }
                else
                {
                    if (txtDataFine.Text.CompareTo("") != 0)
                    {
                        log.Debug("Convert.ToDateTime(txtDataFine.Text)::" + Convert.ToDateTime(txtDataFine.Text).ToString());
                        riga.DataFine = Convert.ToDateTime(txtDataFine.Text);
                    }
                    else
                    {
                        riga.DataFine = DateTime.MaxValue;
                    }
                }
                //*** ***
                log.Debug("Convert.ToDateTime(txtDataInizio.Text)::" + Convert.ToDateTime(txtDataInizio.Text).ToString());
                riga.DataInizio = Convert.ToDateTime(txtDataInizio.Text);

                if ((txtCodPertinenza.Text.CompareTo("") != 0) && (txtCodPertinenza.Text.CompareTo("-1") != 0) && (this.IDImmobile.CompareTo(int.Parse(txtCodPertinenza.Text)) != 0))
                {
                    riga.IDImmobilePertinente = int.Parse(txtCodPertinenza.Text);
                    HasPertinenza = true;
                }
                else
                {
                    if (this.IDImmobile.CompareTo(int.Parse(txtCodPertinenza.Text)) != 0)
                    {
                        riga.IDImmobilePertinente = -1;
                    }
                    else
                    {
                        riga.IDImmobilePertinente = int.Parse(txtCodPertinenza.Text);
                    }
                }

                riga.NoteIci = txtNoteIci.Text;

                if (txtConsistenza.Text == "")
                {
                    riga.Consistenza = decimal.Parse("0");
                }
                else
                {
                    riga.Consistenza = decimal.Parse(txtConsistenza.Text);
                }

                if (txtRendita.Text == "")
                {
                    riga.Rendita = decimal.Parse("0");
                }
                else
                {
                    riga.Rendita = decimal.Parse(txtRendita.Text);
                }

                riga.Zona = ddlZona.SelectedValue.ToString();
                return true;
            }
            catch (Exception Err)
            {
                riga = new Utility.DichManagerICI.OggettiRow();
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.ImmobileLoad.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                return false;
            }
        }
        #endregion
        #region SalvaModificaDettaglio
        /// <summary>
        /// Salva o modifica i dati del dettaglio.
        /// </summary>
        /// <returns></returns>
        private bool SalvaModificaDettaglio()
        {
            bool retval = true;
            Utility.DichManagerICI.DettaglioTestataRow riga = new Utility.DichManagerICI.DettaglioTestataRow();
            //*** 20130923 - gestione modifiche tributarie ***
            Utility.ModificheTributarie FncModificheTributarie = new Utility.ModificheTributarie();
            //*** ***
            int idDettaglio;

            DettaglioTestataTable tblDettaglio = new DettaglioTestataTable(ConstWrapper.sUsername);
            riga.ID = tblDettaglio.GetRow(this.IDImmobile, this.IDTestata, false, ConstWrapper.StringConnection).ID;
            log.Debug("Sono in SalvaModificaDettaglio, riga.ID=" + riga.ID.ToString());
            retval = DettaglioLoad(out riga, riga.ID);
            try
            {
                if (retval)
                {
                    log.Debug("Sono TRUE");
                    if (riga.ID > 0)
                    {
                        log.Debug("riga.ID è > 0");

                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //retval = tblDettaglio.Modify(riga);
                        retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestataCompleta(riga, out idDettaglio);
                        //*** ***
                        this.IDDettaglioTestata = tblDettaglio.GetRow(this.IDImmobile, this.IDTestata, false, ConstWrapper.StringConnection).ID;
                        //*** 20130923 - gestione modifiche tributarie ***
                        TextBox txtFoglio = null;
                        TextBox txtNumero = null;
                        TextBox txtSubalterno = null;
                        if (ConstWrapper.HasDummyDich)
                        {
                            txtFoglio = txtFoglioDummy;
                            txtNumero = txtNumeroDummy;
                            txtSubalterno = txtSubalternoDummy;
                        }
                        else
                        {
                            txtFoglio = txtFoglioNoDummy;
                            txtNumero = txtNumeroNoDummy;
                            txtSubalterno = txtSubalternoNoDummy;
                        }
                        if (riga.TipoUtilizzo.ToString() != Session["TipoUtilizzo"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneTipoPossesso, txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneTipoPossesso + "::@FOGLIO=" + txtFoglio.Text + "::@NUMERO=" + txtNumero.Text + "::@SUBALTERNO=" + txtSubalterno.Text + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.PercPossesso.ToString() != Session["PercPossesso"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePercentualePossesso, txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePercentualePossesso + "::@FOGLIO=" + txtFoglio.Text + "::@NUMERO=" + txtNumero.Text + "::@SUBALTERNO=" + txtSubalterno.Text + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        else if (riga.AbitazionePrincipaleAttuale.ToString() != Session["AbitazionePrincipaleAttuale"].ToString())
                        {
                            if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov, (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte.ToString(), ConstWrapper.CodiceTributo.ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneFlagAbitazionePrincipale, txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, DateTime.Now, Session["username"].ToString(), riga.ID, DateTime.MaxValue) == false)
                            {
                                log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + ConstWrapper.CodiceEnte + "::@TRIBUTO=" + ConstWrapper.CodiceTributo + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneFlagAbitazionePrincipale + "::@FOGLIO=" + txtFoglio.Text + "::@NUMERO=" + txtNumero.Text + "::@SUBALTERNO=" + txtSubalterno.Text + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
                            }
                        }
                        //*** ***
                    }
                    else
                    {
                        log.Debug("riga.ID è < 0");
                        //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                        //retval = tblDettaglio.Insert(riga, out idDettaglio);
                        retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestataCompleta(riga, out idDettaglio);
                        //controllo di congruenza ente
                        if (!new TestataTable(riga.Operatore).CongruenzaEnte(riga.IdTestata)){
                            string sScript = "GestAlert('a', 'Problemi in inserimento Dettaglio Testata contattare l\'assistenza tecnica.');";
                            RegisterScript(sScript, this.GetType());
                        }
                        //*** ***
                        this.IDDettaglioTestata = idDettaglio;
                    }
                }
                return retval;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.SalvaModificaDettaglio.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                return false;
            }
        }
        private bool DettaglioLoad(out Utility.DichManagerICI.DettaglioTestataRow riga, int myID)
        {
             TextBox txtMesiPossesso = null;
            TextBox txtMesiRiduzione = null;
            TextBox txtNumeroModello = null;
            TextBox txtNumeroOrdine = null;
            TextBox txtPercPossesso = null;
            TextBox txtNumeroFigli = null;
            TextBox txtNumeroUtilizzatori = null;
            TextBox txtImpDetrazione = null;
            TextBox txtMesiEsclusione = null;
            DropDownList ddlAbitazionePrincipale = null;
            DropDownList ddlTipoUtilizzo = null;
            DropDownList ddlTipoPossesso = null;
            DropDownList ddlRiduzione = null;
            DropDownList ddlPossesso = null;
            DropDownList ddlEsclusioneEsenzione = null;
            CheckBox chkAbiPrinc = null;
            CheckBox chkColtivatoreDiretto = null;
            Ribes.OPENgov.WebControls.RibesGridView GrdFigli = null;
            try
            {
                riga = new Utility.DichManagerICI.DettaglioTestataRow();
                if (ConstWrapper.HasDummyDich)
                {
                     txtMesiPossesso = txtMesiPossessoDummy;
                    txtMesiRiduzione = txtMesiRiduzioneDummy;
                    txtNumeroModello = txtNumModelloDummy;
                    txtNumeroOrdine = txtNumOrdineDummy;
                    txtPercPossesso = txtPercPossessoDummy;
                    txtNumeroFigli = txtNumFigliDummy;
                    txtNumeroUtilizzatori = txtNumeroUtilizzatoriDummy;
                    txtImpDetrazione = txtImpDetrazioneDummy;
                    txtMesiEsclusione = txtMesiEsclusioneDummy;
                    ddlAbitazionePrincipale = ddlAbitazionePrincipaleDummy;
                    ddlTipoUtilizzo = ddlTipoUtilizzoDummy;
                    ddlTipoPossesso = ddlTipoPossessoDummy;
                    ddlRiduzione = ddlRiduzioneDummy;
                    ddlPossesso = ddlPossessoDummy;
                    ddlEsclusioneEsenzione = ddlEsclusoEsenteDummy;
                    chkAbiPrinc = chkAbitPrincipaleDummy;
                    chkColtivatoreDiretto = chkColtivatoriDummy;
                    GrdFigli = GrdCaricoFigliDummy;
                }
                else
                {
                    txtMesiPossesso = txtMesiPossessoNoDummy;
                    txtMesiRiduzione = txtMesiRiduzioneNoDummy;
                    txtNumeroModello = txtNumModelloNoDummy;
                    txtNumeroOrdine = txtNumOrdineNoDummy;
                    txtPercPossesso = txtPercPossessoNoDummy;
                    txtNumeroFigli = txtnumfigliNoDummy;
                    txtNumeroUtilizzatori = txtNumeroUtilizzatoriNoDummy;
                    txtImpDetrazione = txtImpDetrazioneNoDummy;
                    txtMesiEsclusione = txtMesiEsclusioneNoDummy;
                    ddlAbitazionePrincipale = ddlAbitazionePrincipaleNoDummy;
                    ddlTipoUtilizzo = ddlTipoUtilizzoNoDummy;
                    ddlTipoPossesso = ddlTipoPossessoNoDummy;
                    ddlRiduzione = ddlRiduzioneNoDummy;
                    ddlPossesso = ddlPossessoNoDummy;
                    ddlEsclusioneEsenzione = ddlEsclusoEsenteNoDummy;
                    chkAbiPrinc = chkAbitprincipaleNoDummy;
                    chkColtivatoreDiretto = chkcoltivatoriNoDummy;
                    GrdFigli = GrdCaricoFigliNoDummy;
                }
                riga.ID = myID;
                riga.AbitazionePrincipale = int.Parse(ddlAbitazionePrincipale.SelectedValue);
                riga.Annullato = false;
                riga.Bonificato = false;
                riga.Contitolare = false;
                riga.DataFineValidità = DateTime.MinValue;
                riga.DataInizioValidità = DateTime.Now;
                riga.Ente = ConstWrapper.CodiceEnte;
                riga.IdOggetto = this.IDImmobile;
                riga.IdSoggetto = hdIdContribuente.Value == String.Empty ? 0 : int.Parse(hdIdContribuente.Value);
                riga.IdTestata = this.IDTestata;
                if (txtImpDetrazione.Text != "")
                {
                    riga.ImpDetrazAbitazPrincipale = decimal.Parse(txtImpDetrazione.Text);
                }
                else
                {
                    riga.ImpDetrazAbitazPrincipale = -1;
                }

                if (txtMesiEsclusione.Text != "")
                {
                    riga.MesiEsclusioneEsenzione = int.Parse(txtMesiEsclusione.Text);
                }
                else
                {
                    riga.MesiEsclusioneEsenzione = -1;
                }

                if (txtMesiPossesso.Text != "")
                {
                    riga.MesiPossesso = int.Parse(txtMesiPossesso.Text);
                }
                else
                {
                    riga.MesiPossesso = -1;
                }
                if (txtMesiRiduzione.Text != "")
                {
                    riga.MesiRiduzione = int.Parse(txtMesiRiduzione.Text);
                }
                else
                {
                    riga.MesiRiduzione = -1;
                }

                riga.NumeroModello = txtNumeroModello.Text;
                riga.NumeroOrdine = txtNumeroOrdine.Text;
                riga.Operatore = ConstWrapper.sUsername;

                if (txtPercPossesso.Text != "")
                {
                    riga.PercPossesso = decimal.Parse(txtPercPossesso.Text);
                }
                else
                {
                    riga.PercPossesso = -1;
                }
                //*** 20140509 - TASI ***
                if (int.Parse(ddlTipoUtilizzo.SelectedValue) == 0)
                {
                    riga.TipoUtilizzo = 99;
                }
                else
                {
                    riga.TipoUtilizzo = int.Parse(ddlTipoUtilizzo.SelectedValue);
                }
                if (int.Parse(ddlTipoPossesso.SelectedValue) == 0)
                {
                    riga.TipoPossesso = 99;
                }
                else
                {
                    riga.TipoPossesso = int.Parse(ddlTipoPossesso.SelectedValue);
                }
                //*** ***
                riga.Riduzione = int.Parse(ddlRiduzione.SelectedValue);
                riga.Possesso = int.Parse(ddlPossesso.SelectedValue);
                riga.EsclusioneEsenzione = int.Parse(ddlEsclusioneEsenzione.SelectedValue);

                if (chkAbiPrinc.Checked)
                {
                    riga.AbitazionePrincipaleAttuale = 1;
                }
                else
                {
                    riga.AbitazionePrincipaleAttuale = 0;
                }

                if (txtNumeroUtilizzatori.Text.CompareTo("") == 0)
                {
                    riga.NumeroUtilizzatori = 0;
                }
                else
                {
                    riga.NumeroUtilizzatori = int.Parse(txtNumeroUtilizzatori.Text);
                }
                //*** 20120629 - IMU ***
                riga.ColtivatoreDiretto = chkColtivatoreDiretto.Checked;
                try
                {
                    log.Debug("testo se ho txt val");
                    if (txtNumeroFigli.Text != string.Empty)
                    {
                        riga.NumeroFigli = int.Parse(txtNumeroFigli.Text);
                        if (riga.NumeroFigli > 0)
                        {
                            Utility.DichManagerICI.CaricoFigliRow[] ListCaricoFigli = new Utility.DichManagerICI.CaricoFigliRow[riga.NumeroFigli];
                            int x = 0;
                            foreach (GridViewRow oItemGrid in GrdFigli.Rows)
                            {
                                Utility.DichManagerICI.CaricoFigliRow oCaricoFiglio = new Utility.DichManagerICI.CaricoFigliRow();
                                oCaricoFiglio.IdDettaglioTestata = riga.ID;
                                oCaricoFiglio.nFiglio = x + 1;
                                //oCaricoFiglio.Percentuale=int.Parse(((DropDownList)oItemGrid.FindControl("DdlCaricoFigli")).SelectedValue.ToString());
                                oCaricoFiglio.Percentuale = int.Parse(((TextBox)oItemGrid.FindControl("TxtPercentCarico")).Text.ToString());
                                log.Debug("valorizzo list");
                                ListCaricoFigli[x] = oCaricoFiglio;
                                log.Debug("valorizzo x++");
                                x += 1;
                            }
                            log.Debug("associo lista");
                            riga.ListCaricoFigli = ListCaricoFigli;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("ImmobileDettaglio::DettaglioLoadDummy::carico figli::errore::", ex);
                }
                return true;
            }
            catch (Exception Err)
            {
                riga = new Utility.DichManagerICI.DettaglioTestataRow();
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.DettaglioLoad.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                return false;
            }
        }
        #endregion
        private void PopolaDDlEstimo(DropDownList myDDL, string TipoRendita)
        {
            DataTable TabEstimo = new DataTable();
            myDDL.Items.Clear();
            try
            {
                if (TipoRendita.CompareTo("AF") == 0)
                {
                    TabEstimo = (DataTable)Session["TABELLA_ESTIMO_CATASTALE_FAB"];
                }
                else
                {
                    TabEstimo = (DataTable)Session["TABELLA_ESTIMO_CATASTALE"];
                }

                ListItem myListItem = new ListItem("...", "0");
                myDDL.Items.Add(myListItem);
                foreach (DataRow DR in TabEstimo.Rows)
                {
                    ListItem myListItem1 = new ListItem(DR["ZONA"].ToString(), DR["ZONA"].ToString());
                    myDDL.Items.Add(myListItem1);
                }
            }
            catch (Exception Err)
            {

                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.PopolaDDIEstimo.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");

            }
        }
        //*** 20131003 - gestione atti compravendita ***
        private void AttoCompraVenditaBind(int IdAttoCompraVendita, bool fromPrecarica)
        {
            try
            {
                log.Debug("AttoCompraVenditaBind::inizio");
             
                int CodiceContribuenteContr;
                CodiceContribuenteContr = hdIdContribuente.Value == String.Empty ? 0 : int.Parse(hdIdContribuente.Value);

                int CodiceContribuenteDen;
                CodiceContribuenteDen = hdIdContribuente.Value == String.Empty ? 0 : int.Parse(hdIdContribuente.Value);

                TestataTable Testata = new TestataTable(ConstWrapper.sUsername);
                Utility.DichManagerICI.TestataRow RigaTestata = new Utility.DichManagerICI.TestataRow();
                this.IDTestata = 0;
                RigaTestata.ID = this.IDTestata;
                RigaTestata.Ente = ConstWrapper.CodiceEnte;
                RigaTestata.NumeroDichiarazione = -1;

                RigaTestata.AnnoDichiarazione = this.CompraVenditaDataValidita.Year.ToString();
                RigaTestata.DataInizio = this.CompraVenditaDataValidita;
                RigaTestata.DataFine = DateTime.MinValue;
                RigaTestata.Bonificato = false;
                RigaTestata.Annullato = false;
                RigaTestata.DataInizioValidità = DateTime.Now;
                RigaTestata.DataFineValidità = DateTime.MinValue;
                RigaTestata.Operatore = ConstWrapper.sUsername;
                RigaTestata.IDContribuente = CodiceContribuenteContr;
                RigaTestata.IDDenunciante = CodiceContribuenteDen;
                RigaTestata.IDProvenienza = DichiarazioniView.ProvenienzaDich_Conservatoria;
                log.Debug("AttoCompraVenditaBind::devo inserire nuova testata::IDContribuente::" + RigaTestata.IDContribuente.ToString() + "::DataInizio::" + RigaTestata.DataInizio.ToString());
                // inserisce una nuova testata
                int idTestata;
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                 new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetTestata(Costanti.AZIONE_NEW, RigaTestata, out idTestata);
                //*** ***
                this.IDTestata = idTestata;
                log.Debug("AttoCompraVenditaBind::nuovo idtestata::" + this.IDTestata.ToString());
                Testata.kill();
                log.Debug("devo caricare da attocompravendita");
                if (Request.QueryString["titolo"] != "C")
                {
                    this.IDImmobile = -1;
                }
                ControlsBind(-1, IdAttoCompraVendita, fromPrecarica);
                log.Debug("AttoCompraVenditaBind::caricato controlli");
                //strscript = "alert('');";RegisterScript(sScript,this.GetType());, "err", strscript);this, "Salvataggio" + (retval == true ? "" : " non") + " effettuato.");
            }
            catch (Exception err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.AttoCompraVenditaBind.errore: ", err);
                Response.Redirect("../PaginaErrore.aspx");

            }
            //log.Debug("FINE ContribuenteBind");
        }
        //***	 ***
        private void SaveInitValues()
        {
            Session["Consistenza"] = txtCodPertinenza.Text;
            try
            {
                if (ConstWrapper.HasDummyDich)
                {
                    Session["Foglio"] = txtFoglioDummy.Text;
                    Session["Numero"] = txtNumeroDummy.Text;
                    Session["Subalterno"] = txtSubalternoDummy.Text;
                    Session["DataInizio"] = txtDataInizioDummy.Text;
                    Session["DataFine"] = txtDataFineDummy.Text;
                    Session["Consistenza"] = txtConsistenzaDummy.Text;
                    Session["CodRendita"] = ddlCodiceRenditaDummy.SelectedValue;
                    Session["CodCategoriaCatastale"] = ddlCategoriaCatastaleDummy.SelectedValue;
                    Session["CodClasse"] = ddlClasseDummy.SelectedValue;
                    Session["Rendita"] = txtRenditaDummy.Text;
                    Session["ValoreImmobile"] = txtValoreDummy.Text;
                    //*** 20140509 - TASI ***
                    Session["TipoUtilizzo"] = ddlTipoUtilizzoDummy.SelectedValue;
                    Session["TipoPossesso"] = ddlTipoPossessoDummy.SelectedValue;
                    //*** ***
                    Session["PercPossesso"] = txtPercPossessoDummy.Text;
                    if (chkAbitPrincipaleDummy.Checked)
                    {
                        Session["AbitazionePrincipaleAttuale"] = 1;
                    }
                    else
                    {
                        Session["AbitazionePrincipaleAttuale"] = 0;
                    }
                }
                else
                {
                    Session["Foglio"] = txtFoglioNoDummy.Text;
                    Session["Numero"] = txtNumeroNoDummy.Text;
                    Session["Subalterno"] = txtSubalternoNoDummy.Text;
                    Session["DataInizio"] = txtDataInizioNoDummy.Text;
                    Session["DataFine"] = txtDataFineNoDummy.Text;
                    Session["Consistenza"] = txtConsistenzaDummy.Text;
                    Session["CodRendita"] = ddlCodiceRenditaNoDummy.SelectedValue;
                    Session["CodCategoriaCatastale"] = ddlCategoriaCatastaleNoDummy.SelectedValue;
                    Session["CodClasse"] = ddlClasseNoDummy.SelectedValue;
                    Session["Rendita"] = txtRenditaNoDummy.Text;
                    Session["ValoreImmobile"] = txtValoreNoDummy.Text;
                    //*** 20140509 - TASI ***
                    Session["TipoUtilizzo"] = ddlTipoUtilizzoNoDummy.SelectedValue;
                    Session["TipoPossesso"] = ddlTipoPossessoNoDummy.SelectedValue;
                    //*** ***
                    Session["PercPossesso"] = txtPercPossessoNoDummy.Text;
                    if (chkAbitprincipaleNoDummy.Checked)
                    {
                        Session["AbitazionePrincipaleAttuale"] = 1;
                    }
                    else
                    {
                        Session["AbitazionePrincipaleAttuale"] = 0;
                    }
                }

                if (Session["DataFine"].ToString() == "")
                {
                    Session["DataFine"] = "31/12/9999";
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.SaveInitValues.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        private void LoadBack()
        {
            try
            {
                if (Session["BLN_BONIFICATOVALUE"] == null)
                { Session["BLN_BONIFICATOVALUE"] = "1"; }
                if (hdTypeOperation.Value.ToUpper() == "GESTIONE")
                {
                    log.Debug("torno su gestione");
                       // 21022007 Fabi - ritorna a dichiarazione.aspx
                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());
                }

                if (hdTypeOperation.Value.ToUpper() == "DETTAGLIO")
                {
                    log.Debug("torno su dettaglio");
                    Utility.DichManagerICI.TestataRow RigaTestata = new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, this.IDImmobile, ConstWrapper.StringConnection);
                    if (Request.Params["Provenienza"] != null)
                    {
                        log.Debug("con provenienza");
                        if (Request.Params["Provenienza"] == "CHECKRIFCAT")
                        {
                            log.Debug("checkrifcat");
                            RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_CONFRONTO_CATASTO", ""), this.GetType());//?TypeCheck=" + Request.Params["TypeCheck"]);
                        }
                        else if (Request.Params["ProvenienzaTARSU"] == "TARSU")
                        {
                            log.Debug("tarsu");
                            string sScript = "";
                             sScript = "";
                             sScript += "parent.Visualizza.location.href = '.." + ConstWrapper.Path_TARSU + "/Dichiarazioni/GestImmobili.aspx?";// +Request.Params["ParamRitorno"].ToString() + "'";
                            sScript += "IdContribuente=" + Request.Params["IdContribuente"].ToString() + "&IdTessera=" + Request.Params["IdTessera"].ToString() + "&IdUniqueUI=" + Request.Params["IdUniqueUI"].ToString();
                            sScript += "&IsFromVariabile=" + Request.Params["IsFromVariabile"].ToString();
                            sScript += "&AzioneProv=" + Request.Params["AzioneProv"].ToString() + "&Provenienza=" + Request.Params["Provenienza"].ToString() + "&IdList=" + Request.Params["IdList"].ToString() + "'";
                            sScript += "parent.Comandi.location.href = '../aspVuotaRemoveComandi';";
                            sScript += "parent.Basso.location.href = '../aspVuotaRemoveComandi';";
                            sScript += "parent.Nascosto.location.href = '../aspVuotaRemoveComandi';";
                            log.Debug("registro script::" + sScript);
                            RegisterScript(sScript, this.GetType());
                        }
                        //*** 20150703 - INTERROGAZIONE GENERALE ***
                        else if (Request.Params["Provenienza"] == "INTERGEN")
                        {
                            log.Debug("intergen");
                            string sScript = "";
                            sScript = "";
                            sScript += "parent.Visualizza.location.href='../Interrogazioni/DichEmesso.aspx?Ente=" + ConstWrapper.CodiceEnte + "';";
                            sScript += "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';";
                            sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';";
                            sScript += "parent.Nascosto.location.href='../aspVuotaRemoveComandi.aspx';";
                            sScript += "";
                            RegisterScript(sScript, this.GetType());
                        }
                        //*** ***
                        else
                        {
                            log.Debug("nessuna");
                            RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + (int)RigaTestata.IDContribuente + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());
                        }
                    }
                    else
                    {
                        log.Debug("senza provenienza");
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + (int)RigaTestata.IDContribuente + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString() + "&TYPEOPERATION=GESTIONE"), this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.LoadBack.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw new Exception("LoadBack::" + ex.Message);
            }
        }
        private void GestComandi()
        {
            try
            {
                System.Collections.Generic.List<string> oListCmd = new System.Collections.Generic.List<string>();
                string sScript = "";
                lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
                sScript = "parent.Comandi.location.href = '../aspVuotaRemoveComandi.aspx';";
                sScript += "parent.Basso.location.href = '../aspVuotaRemoveComandi.aspx';";
                sScript += "parent.Nascosto.location.href = '../aspVuotaRemoveComandi.aspx';";
                //Dipe 25/10/2007 
                //Attivazione del tasto per ritornare alla videta dei dati attuali
                //Il valore della session è ti tipo booleano
                if (Session["DATI_ATTUALI"] == null)
                {
                    Session["DATI_ATTUALI"] = false;
                }
                if (Session["SOLA_LETTURA"] == null)
                    Session["SOLA_LETTURA"] = "0";
                if (Session["SOLA_LETTURA"].ToString() == "0")
                {
                    oListCmd = new System.Collections.Generic.List<string>();
                    if ((bool)Session["DATI_ATTUALI"])
                    {
                        oListCmd.Add("Cancel");
                        oListCmd.Add("Unlock");
                        oListCmd.Add("Insert");
                        oListCmd.Add("Delete");
                        oListCmd.Add("Duplica");
                    }
                    else
                    {
                        oListCmd.Add("backAttuali");
                    }
                    foreach (string myItem in oListCmd)
                    {
                        sScript += "$('#" + myItem + "').addClass('hidden');";
                    }
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    if (!(bool)Session["DATI_ATTUALI"])
                    {
                        oListCmd = new System.Collections.Generic.List<string>();
                        oListCmd.Add("backAttuali");
                        foreach (string myItem in oListCmd)
                        {
                            sScript += "$('#" + myItem + "').addClass('hidden');";
                        }
                        RegisterScript(sScript, this.GetType());
                    }
                }
                sScript = "";
                oListCmd = new System.Collections.Generic.List<string>();
                //*** 20131003 - gestione atti compravendita ***
                if (Request["IdAttoCompraVendita"] != null && int.Parse(Request["IdAttoCompraVendita"]) > 0)
                {
                    oListCmd.Add("Territorio");
                    oListCmd.Add("Duplica");
                    oListCmd.Add("DatiAggiuntivi");
                    oListCmd.Add("Contitolari");
                }
                else
                {
                    oListCmd.Add("Precarica");
                }
                //*** ***
                if (Request["OPERATION"] != null)
                {
                    if (Request["OPERATION"].CompareTo("DETTAGLIOSITUAZIONE") == 0)
                    {
                        oListCmd.Add("backAttuali");
                    }
                    else
                    {
                        oListCmd.Add("backSitContrib");
                    }
                }
                if (Business.ConstWrapper.VisualDOCFA == false || this.DOCFAId <= 0)
                {
                    oListCmd.Add("DOCFADet");
                }
                    if (Business.ConstWrapper.NameSistemaTerritorio== "ANATER")
                        oListCmd.Add("Territorio");
                //*** 20140923 - GIS ***
                if (Business.ConstWrapper.VisualGIS == false)
                {
                    oListCmd.Add("GIS");
                }
                //*** ***
                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
                if (Request.Params["ProvenienzaTARSU"] == "TARSU")
                {
                    oListCmd.Add("GIS");
                    oListCmd.Add("DOCFADet");
                    oListCmd.Add("Territorio");
                    oListCmd.Add("Duplica");
                    oListCmd.Add("DatiAggiuntivi");
                    oListCmd.Add("backAttuali");
                    oListCmd.Add("backSitContrib");
                    oListCmd.Add("Contitolari");
                    oListCmd.Add("Precarica");
                    oListCmd.Add("Unlock");
                    oListCmd.Add("Insert");
                    oListCmd.Add("Delete");
                }
                //*** ***
                //if (this.IDImmobile <= 0 || this.CompraVenditaId <= 0)
                //{
                //    oListCmd.Add("Contitolari");
                //    oListCmd.Add("Precarica");
                //}
                foreach (string myItem in oListCmd)
                {
                    sScript += "$('#" + myItem + "').addClass('hidden');";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.GestComandi.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        private void LoadAltriProprietari(string sIdEnte, string sFoglio, string sNumero, string sSubalterno)
        {
            DataTable dtMyDati = new DataTable();

            try
            {
                if (new DichiarazioniView().SearchAltriProprietari(sIdEnte, sFoglio, sNumero, sSubalterno, out dtMyDati))
                {
                    if (!(dtMyDati == null))
                    {
                        foreach (DataColumn myCol in dtMyDati.Columns)
                        {
                            BoundField myGrdCol = new BoundField();
                            myGrdCol.HeaderText = myCol.ColumnName;
                            myGrdCol.DataField = myCol.ColumnName;
                            GrdAltriProprietari.Columns.Add(myGrdCol);
                        }
                        GrdAltriProprietari.DataSource = dtMyDati;
                        GrdAltriProprietari.DataBind();
                        if (GrdAltriProprietari.Rows.Count > 0)
                        {
                            GrdAltriProprietari.Visible = true;
                        }
                        else {
                            GrdAltriProprietari.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.LoadAltriProprietari.errore: ", ex);
            }
        }
        /// <summary>
        /// Metodo per l'estrazione dei dati di catasto
        /// </summary>
        /// <param name="sIdEnte"></param>
        /// <param name="sFoglio"></param>
        /// <param name="sNumero"></param>
        /// <param name="sSubalterno"></param>
        /// <revisionHistory><revision date="02/04/2020">Si separano i dati catasto tra dati ui e dati titolarità per avere entrambe le date di inizio/fine</revision></revisionHistory>
        private void LoadDatiCatasto(string sIdEnte, string sFoglio, string sNumero, string sSubalterno)
        {
            string sMyErr = string.Empty;
            DataTable dtMyDati = new DataTable();

            try
            {
                if (new Catasto().SearchAnomalie(sIdEnte, Catasto.Anomalia.CatastoUI, false, sFoglio, sNumero, sSubalterno, out dtMyDati, out sMyErr))
                {
                    if (!(dtMyDati == null))
                    {
                        foreach (DataColumn myCol in dtMyDati.Columns)
                        {
                            BoundField myGrdCol = new BoundField();
                            myGrdCol.HeaderText = myCol.ColumnName;
                            myGrdCol.DataField = myCol.ColumnName;
                            GrdCatastoUI.Columns.Add(myGrdCol);
                        }
                        GrdCatastoUI.DataSource = dtMyDati;
                        GrdCatastoUI.DataBind();
                        if (GrdCatastoUI.Rows.Count > 0)
                        {
                            GrdCatastoUI.Visible = true;
                        }
                        else {
                            GrdCatastoUI.Visible = false;
                        }
                    }
                }

                if (new Catasto().SearchAnomalie(sIdEnte, Catasto.Anomalia.CatastoTit, false, sFoglio, sNumero, sSubalterno, out dtMyDati, out sMyErr))
                {
                    if (!(dtMyDati == null))
                    {
                        foreach (DataColumn myCol in dtMyDati.Columns)
                        {
                            BoundField myGrdCol = new BoundField();
                            myGrdCol.HeaderText = myCol.ColumnName;
                            myGrdCol.DataField = myCol.ColumnName;
                            GrdCatastoTit.Columns.Add(myGrdCol);
                        }
                        GrdCatastoTit.DataSource = dtMyDati;
                        GrdCatastoTit.DataBind();
                        if (GrdCatastoTit.Rows.Count > 0)
                        {
                            GrdCatastoTit.Visible = true;
                        }
                        else
                        {
                            GrdCatastoTit.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.LoadDatiCatasto.errore: ", ex);
            }
        }
        //private void LoadDatiCatasto(string sIdEnte, string sFoglio, string sNumero, string sSubalterno)
        //{
        //    string sMyErr = string.Empty;
        //    DataTable dtMyDati = new DataTable();

        //    try
        //    {
        //        if (new Catasto().SearchAnomalie(sIdEnte, Catasto.Anomalia.Catasto, false, sFoglio, sNumero, sSubalterno, out dtMyDati, out sMyErr))
        //        {
        //            if (!(dtMyDati == null))
        //            {
        //                foreach (DataColumn myCol in dtMyDati.Columns)
        //                {
        //                    BoundField myGrdCol = new BoundField();
        //                    myGrdCol.HeaderText = myCol.ColumnName;
        //                    myGrdCol.DataField = myCol.ColumnName;
        //                    GrdCatasto.Columns.Add(myGrdCol);
        //                }
        //                GrdCatasto.DataSource = dtMyDati;
        //                GrdCatasto.DataBind();
        //                if (GrdCatasto.Rows.Count > 0)
        //                {
        //                    GrdCatasto.Visible = true;
        //                }
        //                else {
        //                    GrdCatasto.Visible = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobileDettaglio.LoadDatiCatasto.errore: ", ex);
        //    }
        //}
        #endregion
    }
}
