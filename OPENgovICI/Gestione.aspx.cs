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
using DichiarazioniICI.UserControl;
using Business;
using System.Globalization;
using DichiarazioniICI.Database;
using log4net;
using Anater.Oggetti;

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina per la ricerca degli immobili.
    /// Contiene i parametri di ricerca, le funzioni della comandiera e gli usercontrol per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class Gestione : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Gestione));
        /// 
        public System.Web.UI.WebControls.DropDownList ddlProv;
        /// 
        public System.Web.UI.WebControls.DropDownList ddlBonificato;
        /// 
        protected System.Web.UI.WebControls.RadioButton rdbSoggetto;
        /// 
        protected System.Web.UI.WebControls.RadioButton rdbImmobile;
        /// 
        protected System.Web.UI.WebControls.Panel Panel1;
        /// 
        protected FiltroPersona FiltroPersona1;
        /// 
        public Ribes.OPENgov.WebControls.RibesGridView GrdContribuenti;
        /// 
        public Ribes.OPENgov.WebControls.RibesGridView GrdImmobili;
        /// 
        protected System.Web.UI.WebControls.Button btnInserimentoDichiarazione;
        /// 
        public System.Web.UI.WebControls.Label lblRisultati;
        /// 
        public System.Web.UI.WebControls.Label lblNRecord;
        /// 
        protected FiltroImmobile FiltroImmobile1;
        /// 
        protected System.Web.UI.WebControls.Label lblNotaTrascrizione;
        /// 
        protected System.Web.UI.WebControls.Label lblRifNota;
        /// 
        protected System.Web.UI.WebControls.Label lblCatNota;
        /// 
        protected System.Web.UI.WebControls.Label lblUbicazioneNota;
        /// 
        protected System.Web.UI.WebControls.Label lblUbicazioneCatasto;
        /// 
        protected System.Web.UI.WebControls.Label lblSoggettoNota;
        /// 
        protected System.Web.UI.HtmlControls.HtmlGenericControl AttoCompraVendita;
        /// 
        protected System.Web.UI.WebControls.Label Label13;
        /// 
        protected System.Web.UI.WebControls.Label Label14;
        /// 
        public System.Web.UI.WebControls.TextBox TxtDal;
        /// 
        protected System.Web.UI.WebControls.Label Label15;
        /// 
        public System.Web.UI.WebControls.TextBox TxtAl;
        /// 
        protected System.Web.UI.WebControls.Button btnStampaExcel;
        /// 
        protected System.Web.UI.WebControls.Button CmdGIS;
        //*** 201511 - Funzioni Sovracomunali ***
        /// 
        public System.Web.UI.WebControls.DropDownList ddlEnti;
        //*** ***

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
        protected int CompraVenditaIdSoggetto
        {
            get { return ViewState["CompraVenditaIdSoggetto"] != null ? int.Parse(ViewState["CompraVenditaIdSoggetto"].ToString()) : 0; }
            set { ViewState["CompraVenditaIdSoggetto"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected DateTime CompraVenditaDataValidita
        {
            get { return ViewState["CompraVenditaDataValidita"] != null ? Convert.ToDateTime(ViewState["CompraVenditaDataValidita"].ToString()) : DateTime.Now; }
            set { ViewState["CompraVenditaDataValidita"] = value; }
        }

        //*** ***
        //*** 20140923 - GIS ***
        /// <summary>
        /// 
        /// </summary>
        protected string OrigineRichiamo, RifCat;
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataView ListProvenienze()
        {
            DataView Vista = new ProvenienzeTable(ConstWrapper.sUsername).List(TipologieProvenienza.dichiarazioni, ConstWrapper.CodiceTributo, 1);
            return Vista;
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
            this.rdbSoggetto.CheckedChanged += new System.EventHandler(this.rdbSoggetto_CheckedChanged);
            this.rdbImmobile.CheckedChanged += new System.EventHandler(this.rdbImmobile_CheckedChanged);
            this.btnInserimentoDichiarazione.Click += new System.EventHandler(this.btnInserimentoDichiarazione_Click);
            this.btnStampaExcel.Click += new System.EventHandler(this.btnStampaExcel_Click);
            //this.GrdContribuenti.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdContribuenti_ItemDataBound);
            //this.GrdContribuenti.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdContribuenti_ItemCommand);
            //this.GrdContribuenti.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdContribuenti_SortCommand);
            //this.GrdImmobili.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdImmobili_ItemCommand);
            //this.GrdImmobili.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdImmobili_SortCommand);
            this.Load += new System.EventHandler(this.Page_Load);
            // *** 20140923 - GIS ***
            this.CmdGIS.Click += new System.EventHandler(this.CmdGIS_Click);
            //*** ***
        }
        #endregion

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
               Session["DATI_ATTUALI"] = false;

                //M.B.
               Session.Remove("oAnagrafe");

                if (!IsPostBack)
                {
                    ddlProv.Items.Clear();
                    DataView dvProv = new ProvenienzeTable(ConstWrapper.sUsername).List(TipologieProvenienza.dichiarazioni, ConstWrapper.CodiceTributo, 1);
                    ddlProv.DataSource = dvProv;
                    ddlProv.DataBind();

                    //*** 201511 - Funzioni Sovracomunali ***
                    try
                    {
                        ddlEnti.Items.Clear();
                        DataView dvEnti = new DataView();
                        ListItem myNoSel = new ListItem();
                        myNoSel.Text = "...";
                        myNoSel.Value = "";
                        ddlEnti.Items.Add(myNoSel);
                        dvEnti = new Enti().List();
                        foreach (DataRow myRow in dvEnti.Table.Rows)
                        {
                            myNoSel = new ListItem();
                            myNoSel.Text = myRow[1].ToString();
                            myNoSel.Value = myRow[0].ToString();
                            ddlEnti.Items.Add(myNoSel);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.Page_Load.errore: ", ex);
                        Response.Redirect("../PaginaErrore.aspx");
                    }
                    //*** ***

                    //*** 20131003 - gestione atti compravendita ***
                    if (Request.QueryString["IdAttoCompraVendita"] != null)
                    {
                        this.CompraVenditaId = int.Parse(Request.QueryString["IdAttoCompraVendita"]);
                    }
                    if (Request.QueryString["IdAttoSoggetto"] != null)
                    {
                        this.CompraVenditaIdSoggetto = int.Parse(Request.QueryString["IdAttoSoggetto"]);
                    }
                    if (Request.QueryString["IdEnte"] != null)
                    {
                        Session["COD_ENTE"] = Request.QueryString["IdEnte"];
                    }
                    //*** *** 
                    //*** 20140923 - GIS ***
                    if (Request.QueryString["Org"] != null)
                    {
                        this.OrigineRichiamo = Request.QueryString["Org"].ToString();
                        this.RifCat = Request.QueryString["RifCat"].ToString();
                    }
                    //*** ***

                    ViewState.Add("SortKey", "Cognome");
                    ViewState.Add("OrderBy", "ASC");
                    //dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy");

                    if (this.OrigineRichiamo == "GIS")
                    {
                        string[] sRif = this.RifCat.Split('-');
                        ConstWrapper.Parametri = new System.Collections.Hashtable();
                        ConstWrapper.Parametri["TipoRicerca"] = "Immobile";
                        ConstWrapper.Parametri["CategoriaCatastale"] = "-1";
                        ConstWrapper.Parametri["Classe"] = "-1";
                        ConstWrapper.Parametri["Foglio"] = sRif[0];
                        ConstWrapper.Parametri["Numero"] = sRif[1];
                        rdbImmobile.Checked = true;
                        rdbSoggetto.Checked = false;
                        FiltroImmobile1.DataBind();
                    }
                    else
                    {
                        int DichiarazioniDaBonificare = 0;
                        if (DichiarazioniDaBonificare > 0 && new DichiarazioniICI.Database.ImportazioneTable(ConstWrapper.sUsername).GetRow("BonDichiarazioni").Run == false)
                        {
                            if (!Page.IsStartupScriptRegistered("popupdabonificare"))
                            {
                             string   sScript = "window.showModalDialog('PopUpBonifica.aspx?TipoBonifica=bondichiarazioni&Messaggio=Sono presenti " + DichiarazioniDaBonificare.ToString() + " dichiarazioni da bonificare!', window, 'dialogHeight: 200px; dialogWidth: 300px; status: no');";
                                RegisterScript(sScript, this.GetType());
                            }
                        }
                        if (Request.QueryString["TipoRicerca"] != null)
                        {
                            Hashtable myHashTable = new Hashtable();
                            myHashTable.Add("TipoRicerca", Request.QueryString["TipoRicerca"]);
                            myHashTable.Add("Cognome", (Request.QueryString["Cognome"]==null?string.Empty: Request.QueryString["Cognome"]));
                            myHashTable.Add("Nome", (Request.QueryString["Nome"] ==null?string.Empty: Request.QueryString["Nome"]));
                            myHashTable.Add("CodiceFiscale", (Request.QueryString["CodiceFiscale"] ==null?string.Empty: Request.QueryString["CodiceFiscale"]));
                            myHashTable.Add("PartitaIVA", (Request.QueryString["PartitaIva"] ==null?string.Empty: Request.QueryString["PartitaIva"]));
                            Session["SEARCHPARAMETRES1"] = myHashTable;
                        }

                        if (ConstWrapper.Parametri != null)
                        {
                            if (ConstWrapper.Parametri["TipoRicerca"] != null)
                            {
                                if (ConstWrapper.Parametri["TipoRicerca"].ToString() == "Persona")
                                {
                                    rdbSoggetto.Checked = true;
                                    rdbImmobile.Checked = false;
                                }
                                else
                                {
                                    rdbImmobile.Checked = true;
                                    rdbSoggetto.Checked = false;
                                }
                            }
                        }

                        FiltroPersona1.DataBind();
                        FiltroImmobile1.DataBind();
                        GrdContribuenti.Visible = false;

                        //*** 20131003 - gestione atti compravendita ***
                        if (this.CompraVenditaId > 0)
                        {
                            log.Debug("GestioneDettaglio::devo caricare compravendita::" + this.CompraVenditaId.ToString() + "::IDContribuente::" + this.CompraVenditaIdSoggetto.ToString());
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
                            myData = new DichiarazioniView().GetCompraVenditaSoggetto(this.CompraVenditaId, -1, this.CompraVenditaIdSoggetto);
                            foreach (DataRow myRow in myData.Rows)
                            {
                                lblSoggettoNota.Text = myRow["NotaTitolarita"].ToString();
                            }
                            AttoCompraVendita.Style.Add("display", "");
                        }
                        else
                        {
                            AttoCompraVendita.Style.Add("display", "none");
                        }
                    }
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Immobile, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte,-1);
                }
                else
                {
                    //*** 20140923 - GIS ***
                    string sTipoRicerca = "S";
                    if (rdbImmobile.Checked == true)
                        sTipoRicerca = "I";
                    SetGrdCheck(sTipoRicerca);
                    //***  ***
                    if (rdbSoggetto.Checked == true)
                    {
                        GrdImmobili.Visible = false;
                    }
                    if (rdbImmobile.Checked == true)
                    {
                        GrdContribuenti.Visible = false;
                    }
                }
                //*** 201511 - Funzioni Sovracomunali ***
                if (ConstWrapper.CodiceEnte != "")
                {
                    ddlEnti.SelectedValue = ConstWrapper.CodiceEnte;
                    string strscript = "";
                    strscript += "document.getElementById('lblEnti').style.display='none';";
                    strscript += "document.getElementById('ddlEnti').style.display='none';";
                    strscript += "";
                    RegisterScript(strscript,this.GetType());
                }
                //*** ***
                if (Business.ConstWrapper.VisualGIS == false)
                {
                    GrdContribuenti.Columns[7].Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.Page_Load.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
       
        private void rdbSoggetto_CheckedChanged(object sender, System.EventArgs e)
        {
            rdbImmobile.Checked = !rdbSoggetto.Checked;
            FiltroPersona1.DataBind();
            FiltroImmobile1.DataBind();
            GrdContribuenti.Visible = false;
            this.lblRisultati.Text = "Risultati della Ricerca";
        }
                private void rdbImmobile_CheckedChanged(object sender, System.EventArgs e)
        {
            rdbSoggetto.Checked = !rdbImmobile.Checked;
            FiltroPersona1.DataBind();
            FiltroImmobile1.DataBind();
            GrdImmobili.Visible = false;
            this.lblRisultati.Text = "Risultati della Ricerca";
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
                    if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdContribuenti")
                    {
                    foreach (GridViewRow myRow in GrdContribuenti.Rows)
                        if (IDRow == ((HiddenField)myRow.FindControl("hfcodcontribuente")).Value)
                        {
                                //*** 201511 - Funzioni Sovracomunali ***
                                if (ConstWrapper.CodiceEnte != "")
                                {
                                    Session.Add("BLN_BONIFICATOVALUE", ddlBonificato.SelectedValue);
                                    //*** 20131003 - gestione atti compravendita ***
                                    if (this.CompraVenditaId > 0)
                                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + int.Parse(IDRow )+ "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString() + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString()), this.GetType());
                                    else
                                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + int.Parse(IDRow) + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString()), this.GetType());
                                    //*** ***
                                }
                                else
                                {
                                    string strscript = "";
                                    strscript += "GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla funzione sovracomunale');";
                                    strscript += "";
                                    RegisterScript(strscript,this.GetType());
                                }
                                //*** ***
                            }
                    }
                    else {
                        foreach (GridViewRow myRow in GrdImmobili.Rows)
                            if (IDRow == ((HiddenField)myRow.FindControl("hfcodcontribuente")).Value)
                            {
                                //*** 201511 - Funzioni Sovracomunali ***
                                if (ConstWrapper.CodiceEnte != "")
                                {
                                    Session.Add("BLN_BONIFICATOVALUE", ddlBonificato.SelectedValue);
                                    //*** 20131003 - gestione atti compravendita ***
                                    if (this.CompraVenditaId > 0)
                                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + int.Parse(IDRow) + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString() + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString()), this.GetType());
                                    else
                                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE_DETTAGLIO", "?IDContribuente=" + int.Parse(IDRow) + "&Bonificato=" + Session["BLN_BONIFICATOVALUE"].ToString()), this.GetType());
                                    //*** ***
                                }
                                else
                                {
                                    string strscript = "";
                                    strscript += "GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla funzione sovracomunale');";
                                    strscript += "";
                                    RegisterScript(strscript,this.GetType());                                }
                                //*** ***
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.GrdRowCommand.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            LoadSearch(((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID,e.NewPageIndex);
        }
        #endregion
        private void LoadSearch(string myTableName,int? page = 0)
        {
            try
            {
                if (myTableName == "GrdContribuenti")
                {
                    GrdContribuenti.DataSource = (DataTable)Session["TABELLA_RICERCA"];
                    if (page.HasValue)
                        GrdContribuenti.PageIndex = page.Value;
                    GrdContribuenti.DataBind();
                }
                else {
                    GrdImmobili.DataSource = (DataTable)Session["TABELLA_RICERCA_IM"];
                    if (page.HasValue)
                        GrdImmobili.PageIndex = page.Value;
                    GrdImmobili.DataBind();
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.LoadSearch.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }

        private void btnInserimentoDichiarazione_Click(object sender, System.EventArgs e)
        {
            //*** 20131003 - gestione atti compravendita ***
            try { 
            if (this.CompraVenditaId > 0)
                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?TYPEOPERATION=GESTIONE" + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "&IdAttoSoggetto=" + this.CompraVenditaIdSoggetto.ToString()), this.GetType());
            else
                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?TYPEOPERATION=GESTIONE"), this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.btnInserimentoDichiarazione_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
            //*** ***
        }
/// <summary>
/// 
/// </summary>
/// <param name="iInput"></param>
/// <returns></returns>
protected string DecimaliForStampa(object iInput)
        {
            string ret = string.Empty;
            try { 
            if ((iInput.ToString() == "-1") || (iInput.ToString() == "-1,00"))
            {
                ret = string.Empty;
            }
            else
            {
                ret = Convert.ToDecimal(iInput).ToString("N");
            }
            return ret;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.btnInserimentoDichiarazione_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        /// <summary>
        /// Metodo per la stampa Excel delle dichiarazioni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
            DataTable dtImmobili = null;
            string NameXLS = string.Empty;
            string[] arraystr = null;
            DataTable dvVers = new DataTable();

            try
            {
                if (Session["TABELLA_STAMPA"] != null)
                {
                    dvVers = (DataTable)Session["TABELLA_STAMPA"];
                    DataRow dr;
                    DataSet ds = new DataSet();

                    ArrayList listNomiColonne = new ArrayList();
                    int x = 0;

                    IFormatProvider culture = new CultureInfo("it-IT", true);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");
                    //*** 201511 - Funzioni Sovracomunali ***/*** 20140306 ****//**** 20130422 - aggiornamento IMU ****/
                    for (x = 0; x <= 29; x++)//26//25//23
                    {
                        listNomiColonne.Add("");
                    }
                    arraystr = (string[])listNomiColonne.ToArray(typeof(string));

                    NameXLS = ConstWrapper.CodiceEnte + "_IMMOBILI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                    ds.Tables.Add("IMMOBILI");
                    for (x = 0; x <= 29; x++)//26//25//23
                    {
                        ds.Tables["IMMOBILI"].Columns.Add("").DataType = typeof(string);
                    }

                    dtImmobili = ds.Tables["IMMOBILI"];

                    //inserisco intestazione di colonna
                    // COMUNE DI DESCRIZIONE ENTE
                    dr = dtImmobili.NewRow();
                    dr[0] = ConstWrapper.DescrizioneEnte;
                    dr[3] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year; ;
                    dtImmobili.Rows.Add(dr);

                    //aggiungo uno spazio
                    dr = dtImmobili.NewRow();
                    dtImmobili.Rows.Add(dr);

                    //inserisco intestazione - titolo prospetto + data stampa
                    dr = dtImmobili.NewRow();
                    dr[0] = "Prospetto Immobili".ToUpper();
                    dtImmobili.Rows.Add(dr);

                    //inserisco riga vuota
                    dr = dtImmobili.NewRow();
                    dtImmobili.Rows.Add(dr);
                    //inserisco riga vuota
                    dr = dtImmobili.NewRow();
                    dtImmobili.Rows.Add(dr);

                    //inserisco intestazione di colonna
                    dr = dtImmobili.NewRow();
                    x = 0;
                    if (Business.ConstWrapper.CodiceEnte == "")
                    { dr[x] = "Ente"; x += 1; }
                    dr[x] = "Cognome"; x += 1;
                    dr[x] = "Nome"; x += 1;
                    dr[x] = "Codice Fiscale"; x += 1;
                    dr[x] = "Partita IVA"; x += 1;
                    dr[x] = "Data Inizio"; x += 1;
                    dr[x] = "Data Fine"; x += 1;
                    dr[x] = "Caratteristica"; x += 1;
                    dr[x] = "Indirizzo"; x += 1;
                    dr[x] = "Partita Cat."; x += 1;
                    dr[x] = "Sezione"; x += 1;
                    dr[x] = "Foglio"; x += 1;
                    dr[x] = "Numero"; x += 1;
                    dr[x] = "Subalterno"; x += 1;
                    dr[x] = "Cat. Cat."; x += 1;
                    dr[x] = "Classe"; x += 1;
                    dr[x] = "Consistenza"; x += 1;
                    dr[x] = "Valore Euro"; x += 1;
                    /*** 20140306 ****/
                    dr[x] = "Rendita Euro"; x += 1;
                    /**** ***/
                    dr[x] = "% Poss."; x += 1;
                    dr[x] = "Mesi Poss."; x += 1;
                    //*** 20140509 - TASI ***
                    dr[x] = "Tipo Utilizzo"; x += 1;
                    dr[x] = "Tipo Possesso"; x += 1;
                    //*** ***
                    dr[x] = "Mesi Rid."; x += 1;
                    dr[x] = "Mesi Esc. Esen."; x += 1;
                    dr[x] = "Abit. Principale"; x += 1;
                    dr[x] = "N. Utilizzatori"; x += 1;
                    dr[x] = "Pertinenza"; x += 1;
                    /**** 20130422 - aggiornamento IMU ****/
                    dr[x] = "N.Figli Minori"; x += 1;
                    dr[x] = "Note";
                    /**** ****/
                    dtImmobili.Rows.Add(dr);

                    foreach (DataRow rStampa in dvVers.Rows)
                    {
                        string nCivico, esponente, piano, interno, scala, subalterno, partitaCat, dataMorte, giornoM, meseM, annoM;
                        string abitazPrinc, TipoUtilizzo, Pertinenza, nUtilizzatori, mesiRid, mesiEsclEsen, cognome;

                        if ((rStampa["NumeroCivico"].ToString() != "0") && (rStampa["NumeroCivico"].ToString() != "-1"))
                            nCivico = " " + rStampa["NumeroCivico"].ToString();
                        else
                            nCivico = "";

                        if (rStampa["EspCivico"].ToString() != "")
                            esponente = " " + rStampa["EspCivico"].ToString();
                        else
                            esponente = "";

                        if (rStampa["Scala"].ToString() != "")
                            scala = " " + rStampa["Scala"].ToString();
                        else
                            scala = "";

                        if (rStampa["Interno"].ToString() != "")
                            interno = " " + rStampa["Interno"].ToString();
                        else
                            interno = "";

                        if (rStampa["Piano"].ToString() != "")
                            piano = " " + rStampa["Piano"].ToString();
                        else
                            piano = "";

                        if (rStampa["Subalterno"].ToString() != "-1")
                            subalterno = rStampa["Subalterno"].ToString();
                        else
                            subalterno = "";

                        if (rStampa["PartitaCatastale"].ToString() != "-1")
                            partitaCat = rStampa["PartitaCatastale"].ToString();
                        else
                            partitaCat = "";

                        if (rStampa["AbitazionePrincipaleAttuale"].ToString() == "1")
                            abitazPrinc = "Si";
                        else
                            abitazPrinc = "No";

                        //*** 20140509 - TASI ***
                        if (rStampa["IdTipoUtilizzo"].ToString() != "99")
                        {
                            TipoUtilizzo = rStampa["DescTipoUtilizzo"].ToString();
                            TipoUtilizzo = TipoUtilizzo.Replace("°", "");
                            TipoUtilizzo = TipoUtilizzo.Replace("\n", "");
                            TipoUtilizzo = TipoUtilizzo.Replace("\r", "");
                        }
                        else
                            TipoUtilizzo = "";
                        //*** ***

                        if (rStampa["NumeroUtilizzatori"].ToString() == "-1")
                            nUtilizzatori = "";
                        else
                            nUtilizzatori = rStampa["NumeroUtilizzatori"].ToString();

                        if (rStampa["IDIMMOBILEPERTINENTE"].ToString() != "" && rStampa["IDIMMOBILEPERTINENTE"].ToString() != "-1")
                            Pertinenza = "Si";
                        else
                            Pertinenza = "No";

                        if (rStampa["MesiRiduzione"].ToString() != "-1")
                            mesiRid = rStampa["MesiRiduzione"].ToString();
                        else
                            mesiRid = "";
                        if (rStampa["MesiEsclusioneEsenzione"].ToString() != "-1")
                            mesiEsclEsen = rStampa["MesiEsclusioneEsenzione"].ToString();
                        else
                            mesiEsclEsen = "";

                        /* 05092007 se la data di morte è valorizzata ed è diversa da minvalue cognome=EREDE DI cognome */
                        if ((rStampa["DATA_MORTE"] != DBNull.Value) && (rStampa["DATA_MORTE"].ToString() != null))
                        {
                            dataMorte = rStampa["DATA_MORTE"].ToString();
                            DateTime dataM;
                            if (dataMorte != "")
                            {
                                giornoM = dataMorte.Substring(6, 2);
                                meseM = dataMorte.Substring(4, 2);
                                annoM = dataMorte.Substring(0, 4);
                                dataM = new DateTime(Convert.ToInt16(annoM), Convert.ToInt16(meseM), Convert.ToInt16(giornoM));
                            }
                            else { dataM = DateTime.MinValue; }
                            if (dataM != DateTime.MinValue)
                                cognome = "EREDE DI " + rStampa["Cognome"].ToString();
                            else
                                cognome = rStampa["Cognome"].ToString();
                        }
                        else
                        {
                            cognome = rStampa["Cognome"].ToString();
                        }

                        dr = dtImmobili.NewRow();
                        //*** 201511 - Funzioni Sovracomunali ***
                        x = 0;
                        if (Business.ConstWrapper.CodiceEnte == "")
                        { dr[x] = rStampa["DESCRIZIONE_ENTE"].ToString(); x += 1; }
                        dr[x] = " " + cognome; x += 1;
                        dr[x] = " " + rStampa["Nome"].ToString(); x += 1;
                        dr[x] = " " + rStampa["codiceFiscale"].ToString(); x += 1;
                        dr[x] = "'" + rStampa["partitaIva"].ToString(); x += 1;
                        dr[x] = DateTime.Parse(rStampa["DataInizio"].ToString(), culture).ToString("dd/MM/yyyy"); x += 1;
                        dr[x] = DateTime.Parse(rStampa["DataFine"].ToString(), culture).ToString("dd/MM/yyyy"); x += 1;
                        dr[x] = " " + (rStampa["Caratteristica"].ToString()).Trim(); x += 1;
                        dr[x] = rStampa["Via"].ToString() + nCivico + esponente + scala + interno + piano; x += 1;
                        dr[x] = partitaCat + " "; x += 1;
                        dr[x] = " " + rStampa["Sezione"].ToString(); x += 1;
                        dr[x] = rStampa["Foglio"].ToString() + "  "; x += 1;
                        dr[x] = rStampa["Numero"].ToString() + "  "; x += 1;
                        dr[x] = subalterno + "  "; x += 1;
                        dr[x] = " " + rStampa["Categoria"].ToString(); x += 1;
                        dr[x] = rStampa["Classe"].ToString(); x += 1;
                        dr[x] = DecimaliForStampa(rStampa["Consistenza"].ToString()); x += 1;
                        dr[x] = DecimaliForStampa(rStampa["ValoreImmobile"].ToString()); x += 1;
                        /*** 20140306 ****/
                        dr[x] = DecimaliForStampa(rStampa["rendita"].ToString()); x += 1;
                        /*****/
                        dr[x] = DecimaliForStampa(rStampa["PercPossesso"].ToString()) + "  "; x += 1;
                        dr[x] = rStampa["MesiPossesso"].ToString() + "  "; x += 1;
                        dr[x] = TipoUtilizzo; x += 1;
                        dr[x] = rStampa["DescTipoPossesso"].ToString(); x += 1;
                        dr[x] = mesiRid; x += 1;
                        dr[x] = mesiEsclEsen; x += 1;
                        dr[x] = " " + abitazPrinc; x += 1;
                        dr[x] = nUtilizzatori; x += 1;
                        dr[x] = Pertinenza; x += 1;
                        /**** 20130422 - aggiornamento IMU ****/
                        if (rStampa["NUMEROFIGLI"] != DBNull.Value)
                        {
                            dr[x] = rStampa["NUMEROFIGLI"].ToString(); x += 1;
                        }
                        else
                        {
                            dr[x] = ""; x += 1;
                        }
                        if (rStampa["NOTEICI"] != DBNull.Value)
                        {
                            dr[x] = rStampa["NOTEICI"].ToString(); x += 1;
                        }
                        else
                        {
                            dr[x] = ""; x += 1;
                        }
                        /**** ****/
                        dtImmobili.Rows.Add(dr);
                    }

                    //inserisco riga vuota
                    dr = dtImmobili.NewRow();
                    dtImmobili.Rows.Add(dr);
                    //inserisco riga vuota
                    dr = dtImmobili.NewRow();
                    dtImmobili.Rows.Add(dr);

                    //inserisco numero totali di contribuenti
                    dr = dtImmobili.NewRow();
                    dr[0] = "Totale Immobili: " + (dvVers.Rows.Count);
                    dtImmobili.Rows.Add(dr);
                }
                else
                {
                    string stringa;
                    stringa = "GestAlert('a', 'warning', '', '', 'Per la stampa effettuare prima la ricerca!');";
                    RegisterScript(stringa, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.btnStampaExcel_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                dtImmobili = null;
            }
            if (dtImmobili != null)
            {
                //definisco l'insieme delle colonne da esportare
                int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
                //esporto i dati in excel
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                objExport.ExportDetails(dtImmobili, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }
        }
        //*** 20140923 - GIS ***
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
        private void CmdGIS_Click(object sender, System.EventArgs e)
        {
            string CodeGIS, sScript, sRifPrec, sListContrib;
            CodeGIS = sScript = sRifPrec = sListContrib = "";
            RemotingInterfaceAnater.GIS fncGIS = new RemotingInterfaceAnater.GIS();
            System.Collections.Generic.List<RicercaUnitaImmobiliareAnater> listRifCat = new System.Collections.Generic.List<RicercaUnitaImmobiliareAnater>();
            RicercaUnitaImmobiliareAnater myRifCat = new RicercaUnitaImmobiliareAnater();
            DataTable listUI = null;
            try
            {
                if (rdbSoggetto.Checked)
                {
                    if (Session["TABELLA_RICERCA"] != null)
                    {
                        listUI = (DataTable)Session["TABELLA_RICERCA"];
                        foreach (DataRow myUI in listUI.Rows)
                        {
                            if (Business.CoreUtility.FormattaGrdCheck(myUI["bSel"]))
                                sListContrib += myUI["codcontribuente"].ToString() + ",";
                        }
                        sListContrib = sListContrib.Substring(0, sListContrib.Length - 1);
                        listUI = new ContribuentiImmobileView().ListContribuenti(ConstWrapper.StringConnection,ConstWrapper.Ambiente,sListContrib, -1, "", "", "", "", "", "", "", 0, "", "", "", "-1", "-1", ConstWrapper.CodiceEnte, Bonificato.Tutte, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    }
                }
                else
                    listUI = (DataTable)Session["TABELLA_STAMPA"];

                if (listUI != null)
                {
                    foreach (DataRow myUI in listUI.Rows)
                    {
                        if (Business.CoreUtility.FormattaGrdCheck(myUI["bSel"]) == true && myUI["foglio"].ToString() != string.Empty)
                        {
                            if (sRifPrec != myUI["foglio"].ToString() + "|" + myUI["numero"].ToString() + "|" + myUI["subalterno"].ToString())
                            {
                                myRifCat = new RicercaUnitaImmobiliareAnater();
                                myRifCat.Foglio = myUI["foglio"].ToString();
                                myRifCat.Mappale = myUI["numero"].ToString();
                                myRifCat.Subalterno = myUI["subalterno"].ToString();
                                myRifCat.CodiceRicerca = ConstWrapper.CodBelfiore;
                                listRifCat.Add(myRifCat);
                            }
                        }
                        sRifPrec = myUI["foglio"].ToString() + "|" + myUI["numero"].ToString() + "|" + myUI["subalterno"].ToString();
                    }
                }
                if ((listRifCat.ToArray().Length > 0))
                {
                    CodeGIS = fncGIS.getGIS(ConstWrapper.UrlWSGIS, listRifCat.ToArray());
                    if (!(CodeGIS == null))
                    {
                        sScript = "window.open(\'" + ConstWrapper.UrlWebGIS + CodeGIS + "\','wdwGIS');";
                        RegisterScript(sScript,this.GetType());
                    }
                    else
                    {
                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in interrogazione Cartografia!';)";
                        RegisterScript(sScript,this.GetType());
                    }
                }
                else
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');";
                    RegisterScript(sScript,this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.CmdGIS_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //private void CmdGIS_Click(object sender, System.EventArgs e)
        //{
        //    string CodeGIS, sScript, sRifPrec, sListContrib;
        //    CodeGIS = sScript = sRifPrec = sListContrib = "";
        //    RemotingInterfaceAnater.GIS fncGIS = new RemotingInterfaceAnater.GIS();
        //    System.Collections.Generic.List<RicercaUnitaImmobiliareAnater> listRifCat = new System.Collections.Generic.List<RicercaUnitaImmobiliareAnater>();
        //    RicercaUnitaImmobiliareAnater myRifCat = new RicercaUnitaImmobiliareAnater();
        //    DataTable listUI = null;
        //    try
        //    {
        //        if (rdbSoggetto.Checked)
        //        {
        //            if (Session["TABELLA_RICERCA"] != null)
        //            {
        //                listUI = (DataTable)Session["TABELLA_RICERCA"];
        //                foreach (DataRow myUI in listUI.Rows)
        //                {
        //                    if (Business.CoreUtility.FormattaGrdCheck(myUI["bSel"]))
        //                        sListContrib += myUI["codcontribuente"].ToString() + ",";
        //                }
        //                sListContrib = sListContrib.Substring(0, sListContrib.Length - 1);
        //                listUI = new ContribuentiImmobileView().ListContribuenti(sListContrib, -1, "", "", "", "", "", "", "", 0, "", "", "", "-1", "-1", ConstWrapper.CodiceEnte, Bonificato.Tutte, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        //            }
        //        }
        //        else
        //            listUI = (DataTable)Session["TABELLA_STAMPA"];

        //        if (listUI != null)
        //        {
        //            foreach (DataRow myUI in listUI.Rows)
        //            {
        //                if (Business.CoreUtility.FormattaGrdCheck(myUI["bSel"]) == true && myUI["foglio"].ToString() != string.Empty)
        //                {
        //                    if (sRifPrec != myUI["foglio"].ToString() + "|" + myUI["numero"].ToString() + "|" + myUI["subalterno"].ToString())
        //                    {
        //                        myRifCat = new RicercaUnitaImmobiliareAnater();
        //                        myRifCat.Foglio = myUI["foglio"].ToString();
        //                        myRifCat.Mappale = myUI["numero"].ToString();
        //                        myRifCat.Subalterno = myUI["subalterno"].ToString();
        //                        myRifCat.CodiceRicerca = ConstWrapper.CodBelfiore;
        //                        listRifCat.Add(myRifCat);
        //                    }
        //                }
        //                sRifPrec = myUI["foglio"].ToString() + "|" + myUI["numero"].ToString() + "|" + myUI["subalterno"].ToString();
        //            }
        //        }
        //        if ((listRifCat.ToArray().Length > 0))
        //        {
        //            CodeGIS = fncGIS.getGIS(ConstWrapper.UrlWSGIS, listRifCat.ToArray());
        //            if (!(CodeGIS == null))
        //            {
        //                sScript = "window.open(\'" + ConstWrapper.UrlWebGIS + CodeGIS + "\','wdwGIS');";
        //                RegisterScript(sScript, this.GetType());
        //            }
        //            else
        //            {
        //                sScript = "GestAlert('a', 'warning', '', '', 'Errore in interrogazione Cartografia!';)";
        //                RegisterScript(sScript, this.GetType());
        //            }
        //        }
        //        else
        //        {
        //            sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');";
        //            RegisterScript(sScript, this.GetType());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.CmdGIS_Click.errore: ", ex);
        //        Response.Redirect("../PaginaErrore.aspx");
        //    }
        //}
        private void SetGrdCheck(string sTipoRicerca)
        {
            Ribes.OPENgov.WebControls.RibesGridView myGrd = null;
            int nCol = 0;

            try
            {
                DataTable myDvResult = null;
                if (sTipoRicerca == "S")
                {
                    myDvResult = (DataTable)Session["TABELLA_RICERCA"];
                    myGrd = GrdContribuenti;
                    nCol = 7;
                }
                else
                {
                    myDvResult = (DataTable)Session["TABELLA_RICERCA_IM"];
                    myGrd = GrdImmobili;
                    nCol = 12;
                }
                if (myDvResult != null)
                {
                    foreach (GridViewRow itemGrid in myGrd.Rows)
                    {
                        foreach (DataRow myItem in myDvResult.Rows)
                        {
                            if (myItem["codcontribuente"].ToString() == itemGrid.Cells[nCol].Text)
                            {
                                myItem["bSel"] = ((CheckBox)itemGrid.FindControl("chkSel")).Checked;
                                break;
                            }
                        }
                    }
                    if (sTipoRicerca == "S")
                        Session["TABELLA_RICERCA"] = myDvResult;
                    else
                        Session["TABELLA_RICERCA_IM"] = myDvResult;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Gestione.SetGrdCheck.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //***  ***
    }
}