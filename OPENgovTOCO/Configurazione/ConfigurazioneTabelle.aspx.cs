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

namespace OPENGovTOCO.Configurazione
{
    /// <summary>
/// Pagina per la consultazione/gestione dei codici/descrizione.
/// Le possibili opzioni sono:
/// - nuovo inserimento
/// - Salva
/// - Elimina
/// </summary>
    public partial class ConfigurazioneTabelle : BasePage
    {
        #region definizioni
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblTitolo;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox tbxIdRecordToMod;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button btCanc;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurazioneTabelle));
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try {
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                Log.Debug("ConfigurazioneTabelle::Page_Load::caricamento pagina");
                btDel.Attributes.Add("click", "confermaEliminazione(); return false;");
                if (!IsPostBack)
                {
                    GetDati();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        private void GetDati(int? page = 0)
        {
            Log.Debug("ConfigurazioneTabelle::GetDati::caricamento dati");
            try
            {

                pnlCategorie.Visible = false;
                pnlAgevolazioni.Visible = false;
                pnlTipoOccupazioni.Visible = false;

                if (RBCategoria.Checked == true)
                {
                    pnlCategorie.Visible = true;
                    Categorie[] categorie = MetodiCategorie.GetAllCategorie((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));

                    if (IsPostBack == false)
                    {
                        DataBindGridCategorie(categorie,  true);
                        lblTabSel.Text = RBCategoria.Text;
                    }
                    else
                    {
                        DataBindGridCategorie(categorie, false,page);
                    }
                }
                else if (RBAgevolazione.Checked == true)
                {
                    pnlAgevolazioni.Visible = true;
                    Agevolazione[] agevolazioni = MetodiAgevolazione.GetAllAgevolazioni((string)DichiarazioneSession.IdEnte);

                    if (IsPostBack == false)
                    {
                        DataBindGridAgevolazioni(agevolazioni,  true);
                        lblTabSel.Text = RBAgevolazione.Text;
                    }
                    else
                    {
                        DataBindGridAgevolazioni(agevolazioni, false,page);
                    }
                }
                else if (RBTipologiaOccupazioni.Checked == true)
                {
                    pnlTipoOccupazioni.Visible = true;
                    TipologieOccupazioni[] tipOccupaz = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), false);

                    if (IsPostBack == false)
                    {
                        DataBindGridTipoOccupazioni(tipOccupaz, true);
                        lblTabSel.Text = RBTipologiaOccupazioni.Text;
                    }
                    else
                    {
                        DataBindGridTipoOccupazioni(tipOccupaz, false,page);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.GetDati.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agevolazioni"></param>
        /// <param name="bRebind"></param>
        /// <param name="page"></param>
        private void DataBindGridAgevolazioni(Agevolazione[] agevolazioni, bool bRebind, int? page = 0)
        {
            try {
                GrdAgevolazioni.SelectedIndex = -1;
                if (agevolazioni != null && agevolazioni.Length > 0)
                {
                    GrdAgevolazioni.DataSource = agevolazioni;
                    if (bRebind)
                    {
                        //GrdAgevolazioni.SelectedIndex = -1;
                        GrdAgevolazioni.DataBind();
                    }
                    if (page.HasValue)
                        GrdAgevolazioni.PageIndex = page.Value;
                    GrdAgevolazioni.DataBind();
                    GrdAgevolazioni.Visible = true;
                    lblResultList.Visible = false;
                }
                else
                {
                    lblResultList.Visible = true;
                    lblResultList.Text = "Nessun risultato per questa ricerca.";
                    GrdAgevolazioni.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.DataBindGridAgevolazioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipOccupaz"></param>
        /// <param name="bRebind"></param>
        /// <param name="page"></param>
        private void DataBindGridTipoOccupazioni(TipologieOccupazioni[] tipOccupaz,  bool bRebind, int? page = 0)
        {
            try {
                GrdTipOccupazioni.SelectedIndex = -1;
                if (tipOccupaz != null && tipOccupaz.Length > 0)
                {
                    GrdTipOccupazioni.DataSource = tipOccupaz;
                    if (bRebind)
                    {
                        //GrdTipOccupazioni.SelectedIndex = -1;
                        GrdTipOccupazioni.DataBind();
                    }
                    if (page.HasValue)
                        GrdTipOccupazioni.PageIndex = page.Value;
                    GrdTipOccupazioni.DataBind();
                    GrdTipOccupazioni.Visible = true;
                    lblResultList.Visible = false;
                }
                else
                {
                    lblResultList.Visible = true;
                    lblResultList.Text = "Nessun risultato per questa ricerca.";
                    GrdTipOccupazioni.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.DataBindGripTipoOccupazioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagamenti"></param>
        /// <param name="bRebind"></param>
        /// <param name="page"></param>
        private void DataBindGridCategorie(Categorie[] pagamenti,  bool bRebind, int? page = 0)
        {
            try {
                GrdCategorie.SelectedIndex = -1;
                if (pagamenti != null && pagamenti.Length > 0)
                {
                    GrdCategorie.DataSource = pagamenti;
                    if (bRebind)
                    {
                        //GrdCategorie.SelectedIndex = -1;
                        GrdCategorie.DataBind();
                    }
                    if (page.HasValue)
                        GrdCategorie.PageIndex = page.Value;
                    GrdCategorie.DataBind();
                    GrdCategorie.Visible = true;
                    lblResultList.Visible = false;
                }
                else
                {
                    lblResultList.Visible = true;
                    lblResultList.Text = "Nessun risultato per questa ricerca.";
                    GrdCategorie.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.DataBindGridCategorie.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

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
                    if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdCategorie")
                    {
                        foreach (GridViewRow myRow in GrdCategorie.Rows)
                            if (IDRow == ((HiddenField)myRow.FindControl("hfIdCategoria")).Value)
                            {
                                TableCell item2Cell = SharedFunction.CellByName(myRow, "Descrizione");

                                bool fail = false;

                                if ((HiddenField)myRow.FindControl("hfIdCategoria") == null || item2Cell == null)
                                {
                                    Log.Error("Impossibile recuperare l'id o la descrizione da modificare.");
                                    fail = true;
                                }

                                if (fail == true)
                                {
                                    string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile completare l\'operazione.');";
                                    RegisterScript(sScript,this.GetType());
                                }
                                else
                                {
                                    pnlModifica.Visible = true;

                                    txbIdRecordToMod.Text = ((HiddenField)myRow.FindControl("hfIdCategoria")).Value;
                                    txbDescrizione.Text = item2Cell.Text;
                                }
                            }
                    }
                    else if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdAgevolazioni")
                    {
                        foreach (GridViewRow myRow in GrdAgevolazioni.Rows)
                            if (IDRow == ((HiddenField)myRow.FindControl("hfIdAgevolazione")).Value)
                            {
                                TableCell item2Cell = SharedFunction.CellByName(myRow, "Descrizione");

                                bool fail = false;

                                if ((HiddenField)myRow.FindControl("hfIdAgevolazione") == null || item2Cell == null)
                                {
                                    Log.Error("Impossibile recuperare l'id o la descrizione da modificare.");
                                    fail = true;
                                }

                                if (fail == true)
                                {
                                    string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile completare l\'operazione.');" ;
                                    RegisterScript(sScript,this.GetType());
                                }
                                else
                                {
                                    pnlModifica.Visible = true;

                                    txbIdRecordToMod.Text = ((HiddenField)myRow.FindControl("hfIdAgevolazione")).Value;
                                    txbDescrizione.Text = item2Cell.Text;
                                }
                            }
                    }
                    else if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdTipOccupazioni")
                    {
                        foreach (GridViewRow myRow in GrdTipOccupazioni.Rows)
                            if (IDRow == ((HiddenField)myRow.FindControl("hfIdTipologiaOccupazione")).Value)
                            {
                                TableCell item2Cell = SharedFunction.CellByName(myRow, "Descrizione");

                                bool fail = false;

                                if ((HiddenField)myRow.FindControl("hfIdTipologiaOccupazione") == null || item2Cell == null)
                                {
                                    Log.Error("Impossibile recuperare l'id o la descrizione da modificare.");
                                    fail = true;
                                }

                                if (fail == true)
                                {
                                    string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile completare l\'operazione.');" ;
                                    RegisterScript(sScript,this.GetType());
                                }
                                else
                                {
                                    pnlModifica.Visible = true;

                                    txbIdRecordToMod.Text = ((HiddenField)myRow.FindControl("hfIdTipologiaOccupazione")).Value;
                                    txbDescrizione.Text = item2Cell.Text;
                                }
                            }
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.GrdRowCommand.errore: ", Err);
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
            GetDati(e.NewPageIndex);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btNew_Click(object sender, System.EventArgs e)
        {
            Log.Debug("ConfigurazioneTabelle::btNew_Click::click sul pulsante nuovo");
            pnlModifica.Visible = true;
            txbIdRecordToMod.Text = "0";
            txbDescrizione.Text = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSalva_Click(object sender, System.EventArgs e)
        {
            try {
                if (SharedFunction.IsNullOrEmpty(txbIdRecordToMod.Text) == true)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    // Descrizione vuota
                    if (SharedFunction.IsNullOrEmpty(txbDescrizione.Text) == true)
                    {
                        string sScript = "GestAlert('a', 'warning', '', '', 'Inserisci una descrizione.');";
                        RegisterScript(sScript,this.GetType());
                    }
                    else
                    {
                        int esito = 0;
                        int IdToMod = 0;

                        // Se non è in modalità INSERIMENTO
                        if (txbIdRecordToMod.Text != "0")
                        {
                            try
                            {
                                IdToMod = int.Parse(txbIdRecordToMod.Text);
                            }
                            catch { IdToMod = -1; }
                        }

                        // Inserimento o Update
                        if (IdToMod >= 0)
                        {
                            if (RBCategoria.Checked == true)
                            {
                                esito = MetodiCategorie.InsertOrUpdateDescrizione(DichiarazioneSession.StringConnection, (string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""),
                                    IdToMod, txbDescrizione.Text.Trim());
                                if (esito == 1)
                                {
                                    esito = 0;
                                }
                                else
                                { esito = -1; }
                            }
                            else if (RBAgevolazione.Checked == true)
                            {
                                esito = MetodiAgevolazione.InsertOrUpdateDescrizione((string)DichiarazioneSession.IdEnte,
                                    IdToMod, txbDescrizione.Text.Trim());
                                if (esito == 1)
                                {
                                    esito = 0;
                                }
                                else
                                { esito = -1; }
                            }
                            else if (RBTipologiaOccupazioni.Checked == true)
                            {
                                esito = MetodiTipologieOccupazioni.InsertOrUpdateDescrizione(DichiarazioneSession.StringConnection, (string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""),
                                    IdToMod, txbDescrizione.Text.Trim());
                                if (esito == 1)
                                {
                                    esito = 0;
                                }
                                else
                                { esito = -1; }
                            }

                            txbIdRecordToMod.Text = null;
                            txbDescrizione.Text = null;
                            pnlModifica.Visible = false;
                        }
                        else
                        {
                            Log.Error("Id record non valido");
                            string sScript = "GestAlert('a', 'danger', '', '', 'Errore del sistema');";
                            RegisterScript(sScript,this.GetType());
                        }

                        /*
                         * CONTROLLI ESITI
                         * 
                         * */
                        if (esito >= 0)
                        {
                            string sScript = "";
                            if (IdToMod > 0)
                                sScript = "GestAlert('a', 'success', '', '', 'Descrizione modificata correttamente.');";
                            else
                            {
                                txbIdRecordToMod.Text = esito.ToString();
                                sScript = "GestAlert('a', 'success', '', '', 'Inserimento avvenuto correttamente.');";
                            }
                            RegisterScript(sScript,this.GetType());

                            if (RBCategoria.Checked == true)
                            {
                                Categorie[] categorie = MetodiCategorie.GetAllCategorie((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                                DataBindGridCategorie(categorie,  true);
                            }
                            else if (RBAgevolazione.Checked == true)
                            {
                                Agevolazione[] agevolazioni = MetodiAgevolazione.GetAllAgevolazioni((string)DichiarazioneSession.IdEnte);
                                DataBindGridAgevolazioni(agevolazioni,  true);
                            }
                            else if (RBTipologiaOccupazioni.Checked == true)
                            {
                                TipologieOccupazioni[] tipOccupaz = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                                DataBindGridTipoOccupazioni(tipOccupaz,  true);
                            }
                        }
                        else
                        {
                            if (esito == -1)
                            {
                                string sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! Descrizione già presente.');";
                                RegisterScript(sScript,this.GetType());
                            }
                            else
                            {
                                string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile salvare la descrizione.');";
                                RegisterScript(sScript,this.GetType());
                            }
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.btSalva_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btDel_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (SharedFunction.IsNullOrEmpty(txbIdRecordToMod.Text) == true || txbIdRecordToMod.Text == "0")
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    Log.Debug("btDel_Click::inizio dei controlli");
                    int esito = 0;
                    int IdToMod = 0;

                    try
                    {
                        IdToMod = int.Parse(txbIdRecordToMod.Text);
                    }
                    catch
                    {
                        IdToMod = -1;
                    }

                    // Inserimento o Update
                    if (IdToMod > 0)
                    {
                        if (RBCategoria.Checked == true)
                        {
                            esito = MetodiCategorie.DeleteCategoria(IdToMod);
                            if (esito == 1)
                            {
                                esito = 0;
                            }
                            else
                            { esito = -1; }
                        }
                        else if (RBAgevolazione.Checked == true)
                        {
                            esito = MetodiAgevolazione.DeleteAgevolazione(IdToMod);
                            if (esito == 1)
                            {
                                esito = 0;
                            }
                            else
                            { esito = -1; }
                        }
                        else if (RBTipologiaOccupazioni.Checked == true)
                        {
                            esito = MetodiTipologieOccupazioni.DeleteTipologiaOccupazione(IdToMod);
                            if (esito == 1)
                            {
                                esito = 0;
                            }
                            else
                            { esito = -1; }
                        }
                    }
                    else
                    {
                        Log.Error("Id record non valido");
                        string sScript = "GestAlert('a', 'danger', '', '', 'Errore del sistema');";
                        RegisterScript(sScript,this.GetType());
                    }

                    /*
                    * CONTROLLI ESITI
                    * 
                    * */
                    if (esito == 0)
                    {
                        string sScript = "GestAlert('a', 'success', '', '', 'Cancellazione avvenuta con successo.');";
                        RegisterScript(sScript,this.GetType());

                        if (RBCategoria.Checked == true)
                        {
                            Categorie[] categorie = MetodiCategorie.GetAllCategorie((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                            DataBindGridCategorie(categorie,  true);
                        }
                        else if (RBAgevolazione.Checked == true)
                        {
                            Agevolazione[] agevolazioni = MetodiAgevolazione.GetAllAgevolazioni((string)DichiarazioneSession.IdEnte);
                            DataBindGridAgevolazioni(agevolazioni,  true);
                        }
                        else if (RBTipologiaOccupazioni.Checked == true)
                        {
                            TipologieOccupazioni[] tipOccupaz = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                            DataBindGridTipoOccupazioni(tipOccupaz,  true);
                        }

                        txbIdRecordToMod.Text = null;
                        txbDescrizione.Text = null;
                        pnlModifica.Visible = false;
                    }
                    else
                    {
                        if (esito == -1)
                        {
                            string sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! Non è possibile cancellare questo elemento poiché vi sono degli articoli associati.');";
                            RegisterScript(sScript,this.GetType());
                        }
                        else
                        {
                            string sScript = "GestAlert('a', 'danger', '', '', 'Impossibile salvare la descrizione.');";
                            RegisterScript(sScript,this.GetType());
                        }
                    }

                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTabelle.btDel_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RBCategoria_CheckedChanged(object sender, System.EventArgs e)
        {
            Categorie[] categorie = MetodiCategorie.GetAllCategorie((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
            DataBindGridCategorie(categorie,  true);

            txbIdRecordToMod.Text = null;
            txbDescrizione.Text = null;
            pnlModifica.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RBAgevolazione_CheckedChanged(object sender, System.EventArgs e)
        {
            Agevolazione[] agevolazioni = MetodiAgevolazione.GetAllAgevolazioni((string)DichiarazioneSession.IdEnte);
            DataBindGridAgevolazioni(agevolazioni,  true);

            txbIdRecordToMod.Text = null;
            txbDescrizione.Text = null;
            pnlModifica.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RBTipologiaOccupazioni_CheckedChanged(object sender, System.EventArgs e)
        {
            TipologieOccupazioni[] tipOccupaz = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
            DataBindGridTipoOccupazioni(tipOccupaz,  true);

            txbIdRecordToMod.Text = null;
            txbDescrizione.Text = null;
            pnlModifica.Visible = false;
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

            //this.GrdCategorie.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdCategorie_ItemCommand);
            //this.GrdAgevolazioni.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdAgevolazioni_ItemCommand);
            //this.GrdTipOccupazioni.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdTipOccupazioni_ItemCommand);

        }
        #endregion

    }
}
