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
/// Pagina per la consultazione/gestione delle tariffe.
/// Le possibili opzioni sono:
/// - Ribalta
/// - Nuovo inserimento
/// - Salva
/// - Elimina
/// - Ricerca
/// </summary>
	public partial class ConfigurazioneTariffe : BasePage
	{
		#region definizioni
		private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurazioneTariffe));
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
                //btDel.Attributes.Add("onclick", "confermaEliminazione(); return false;");
                btDel.Attributes.Add("click", "confermaEliminazione(); return false;");
                if (!IsPostBack)
                {
                    LoadCategorie(ddlCategorie, true);
                    LoadTipologieOccupazioni(ddlTipologieOccupazione, true);
                    LoadDurata(ddlDurata, true);
                    LoadTributi(ddlTributo, true);
                }
                //DataBindGridTariffe(LoadTariffe(), 0, !IsPostBack);
                pnlInserisci.Visible = pnlModifica.Visible = false;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="WithDefault"></param>
		protected void LoadCategorie(DropDownList ddl, bool WithDefault)
		{
            try {
                Categorie[] lista = MetodiCategorie.GetCategorie(DichiarazioneSession.StringConnection, DateTime.Now.Year, WithDefault, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                if (lista != null && lista.Length > 0)
                {
                    ddl.DataSource = lista;
                    ddl.DataTextField = "Descrizione";
                    ddl.DataValueField = "IdCategoria";
                    ddl.DataBind();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.LoadCategorie.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="WithDefault"></param>
		protected void LoadTipologieOccupazioni(DropDownList ddl, bool WithDefault)
		{
            try {
                TipologieOccupazioni[] lista = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), WithDefault);
                if (lista != null && lista.Length > 0)
                {
                    ddl.DataSource = lista;
                    ddl.DataTextField = "Descrizione";
                    ddl.DataValueField = "IdTipologiaOccupazione";
                    ddl.DataBind();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.LoadTipollgieOccupazioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="WithDefault"></param>
		protected void LoadDurata(DropDownList ddl, bool WithDefault)
		{
            try {
                Durata[] lista = MetodiDurata.GetDurate(DichiarazioneSession.StringConnection, WithDefault);
                if (lista != null && lista.Length > 0)
                {
                    ddl.DataSource = lista;
                    ddl.DataTextField = "Descrizione";
                    ddl.DataValueField = "IdDurata";
                    ddl.DataBind();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.LoadDurata.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="WithDefault"></param>
		protected void LoadTributi(DropDownList ddl, bool WithDefault)
		{
            try {
                if (DichiarazioneSession.CodTributo("") == Utility.Costanti.TRIBUTO_SCUOLE)
                {
                    ddl.Items.Add(new ListItem("SCUOLE", Utility.Costanti.TRIBUTO_SCUOLE));
                    ddl.SelectedValue = Utility.Costanti.TRIBUTO_SCUOLE;
                }
                else
                {
                    if (WithDefault)
                    {
                        ListItem item = new ListItem("...", "");
                        ddl.Items.Add(item);
                    }
                    ddl.Items.Add(new ListItem("OCCUPAZIONE PERMANENTE", Utility.Costanti.TRIBUTO_OccupazionePermanente));
                    ddl.Items.Add(new ListItem("OCCUPAZIONE TEMPORANEA", Utility.Costanti.TRIBUTO_OccupazioneTemporanea));
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.LoadTributi.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected Tariffe[] LoadTariffe()
		{
			return MetodiTariffe.GetTariffe(0,  (string)DichiarazioneSession.IdEnte, ddlTributo.SelectedValue, SharedFunction.IntTryParse(ddlCategorie.SelectedValue,0), SharedFunction.IntTryParse(ddlTipologieOccupazione.SelectedValue,0), SharedFunction.IntTryParse(ddlDurata.SelectedValue,0), SharedFunction.IntTryParse(txbAnno.Text, 0));
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tariff"></param>
        /// <param name="bRebind"></param>
        /// <param name="page"></param>
		private void DataBindGridTariffe (Tariffe[] tariff, bool bRebind, int? page = 0)
		{
            try {
                //			GrdTariffe.SelectedIndex = -1;
                if (tariff != null && tariff.Length > 0)
                {
                    GrdTariffe.DataSource = tariff;

                    if (bRebind)
                    {
                        GrdTariffe.DataBind();
                    }
                    if (page.HasValue)
                        GrdTariffe.PageIndex = page.Value;
                    GrdTariffe.DataBind();
                    GrdTariffe.Visible = true;
                    lblResultList.Visible = false;
                }
                else
                {
                    lblResultList.Visible = true;
                    lblResultList.Text = "Nessun risultato per questa ricerca.";
                    GrdTariffe.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.DataBindGridTariffe.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

        #region "Griglie"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.GrdRowDataBound.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
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
                    foreach (GridViewRow myRow in GrdTariffe.Rows)
                        
                        if (IDRow == ((HiddenField)myRow.FindControl("hfIdTariffa")).Value)
                        {
                            //TableCell itemCell = SharedFunction.CellByName(myRow, "IdTariffa");
                            string item1Cell = SharedFunction.CellByName(myRow, "Anno").Text;
                            string item2Cell = SharedFunction.CellByName(myRow, "Valore").Text;
                            string item3Cell = SharedFunction.CellByName(myRow, "MinimoApplicabile").Text;
                            string categoriaCell = SharedFunction.CellByName(myRow, "Categoria").Text;
                            string occupazioneCell = SharedFunction.CellByName(myRow, "Tipologia Occupazione").Text;
                            string durataCell = SharedFunction.CellByName(myRow, "Durata").Text;
                            string descrTributoCell = SharedFunction.CellByName(myRow, "Tributo").Text;

                            bool fail = false;

                            if (item2Cell == null || item3Cell == null)
                            {
                                Log.Error("Impossibile recuperare l'id, l'Anno il Valore o il MinimoApplicabile da modificare.");
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
                                pnlInserisci.Visible = false;

                                txbIdRecordToMod.Text = IDRow;
                                lblCategoriaEdit.Text = categoriaCell;
                                lblTipologiaOccupazioneEdit.Text = occupazioneCell;
                                lblDurataEdit.Text = durataCell;
                                lblAnnoEdit.Text = item1Cell;
                                txbValoreEdit.Text = item2Cell;
                                txbMinimoApplicabileEdit.Text = item3Cell;
                                lblTributoEdit.Text = descrTributoCell;
                            }
                        }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.GrdRowCommand.errore: ", Err);
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
            DataBindGridTariffe(LoadTariffe(), true,e.NewPageIndex);
        }
     #endregion

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
			//this.GrdTariffe.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdTariffe_ItemCommand);

		}
		#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			DataBindGridTariffe(LoadTariffe(),  true);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btNew_Click(object sender, System.EventArgs e)
		{
			pnlModifica.Visible = false;
			pnlInserisci.Visible = true;
			LoadCategorie(ddlCategoriaIns, false);
			LoadTipologieOccupazioni(ddlTipologiaOccupazioneIns, false);
			LoadDurata(ddlDurataIns, false);
			LoadTributi(ddlTributoIns, false);
			txbIdRecordToMod.Text = "0";
			txbAnnoIns.Text = null;
			txbValoreIns.Text = null;
			txbMinimoApplicabileIns.Text = null;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btSalva_Click(object sender, System.EventArgs e)
		{
            try {
                string sScript = null;
                if (SharedFunction.IsNullOrEmpty(txbIdRecordToMod.Text) == true)
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');";
                    RegisterScript(sScript,this.GetType());
                    return;
                }
                //Check Insert or Update
                int IdToMod = 0;
                int esito = 0;
                int Categoria = 0;
                int TipologiaOccupazione = 0;
                int Durata = 0;
                int Anno = 0;
                decimal Valore = 0;
                decimal MinimoApplicabile = 0;
                IdToMod = SharedFunction.IntTryParse(txbIdRecordToMod.Text, -1);

                switch (IdToMod)
                {
                    case -1: //Errore
                        sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');";
                        RegisterScript(sScript,this.GetType());
                        return;
                    case 0: //Inserimento
                        if (SharedFunction.IsNullOrEmpty(ddlTributoIns.SelectedValue))//Check Tributo
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Tributo.');";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        if (SharedFunction.IsNullOrEmpty(txbAnnoIns.Text))//Check Anno
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci l\'Anno.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        if (SharedFunction.IsNullOrEmpty(txbValoreIns.Text))//Check Valore
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Valore.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        if (SharedFunction.IsNullOrEmpty(txbMinimoApplicabileIns.Text))//Check Minimo Applicabile
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Minimo Applicabile.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        Categoria = SharedFunction.IntTryParse(ddlCategoriaIns.SelectedValue, -1);
                        if (Categoria == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Scegli la Categoria.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        TipologiaOccupazione = SharedFunction.IntTryParse(ddlTipologiaOccupazioneIns.SelectedValue, -1);
                        if (TipologiaOccupazione == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Scegli la Tipologia Occupazione.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        Durata = SharedFunction.IntTryParse(ddlDurataIns.SelectedValue, -1);
                        if (Durata == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Scegli la Durata.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        Anno = SharedFunction.IntTryParse(txbAnnoIns.Text, -1);
                        if (Anno == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci l\'Anno.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }

                        Valore = SharedFunction.DecimaltryParse(txbValoreIns.Text, -1);
                        if (Valore == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Valore.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        MinimoApplicabile = SharedFunction.DecimaltryParse(txbMinimoApplicabileIns.Text, -1);
                        if (MinimoApplicabile == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Minimo Applicabile.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }

                        esito = MetodiTariffe.InsertTariffa(DichiarazioneSession.StringConnection, (string)DichiarazioneSession.IdEnte, ddlTributoIns.SelectedValue, Categoria, TipologiaOccupazione, Durata, Anno, Valore, MinimoApplicabile);
                        /*if (esito == 1)
                        {
                            esito = 0;
                        }
                        else { esito = -1; }*/
                        break;
                    default: //Modifica
                        Valore = SharedFunction.DecimaltryParse(txbValoreEdit.Text, -1);
                        if (Valore == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Valore.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        MinimoApplicabile = SharedFunction.DecimaltryParse(txbMinimoApplicabileEdit.Text, -1);
                        if (MinimoApplicabile == -1)
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Minimo Applicabile.')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }
                        Anno = SharedFunction.IntTryParse(lblAnnoEdit.Text, -1);
                        //*** 20130610 - ruolo supplettivo ***
                        if (ElaborazioniEffettuate(Anno, Ruolo.E_TIPO.ORDINARIO))
                        {
                            sScript = "GestAlert('a', 'warning', '', '', 'Esistono già delle elaborazioni per l\'anno " + Anno + ". Non puoi modificare la Tariffa selezionata!')";
                            RegisterScript(sScript,this.GetType());
                            return;
                        }

                        //*** ***
                        esito = MetodiTariffe.UpdateTariffa(IdToMod, Valore, MinimoApplicabile);
                        //*** 20140416 - aggiunto controllo su esito
                        //senza controllo e senza modifica in UpdateTariffa
                        //veniva sempre ritornato -1, quindi restituiva sempre lo stesso messaggio
                        if (esito == -1)
                        {
                            esito = 0;
                        }
                        else { esito = -1; }
                        break;
                }
                /*
                    * CONTROLLI ESITI
                    * 
                    * */
                if (esito >= 0)
                {
                    if (IdToMod > 0)
                    {
                        sScript = "GestAlert('a', 'success', '', '', 'Tariffa modificata correttamente.')";
                    }
                    //*** 20140416 - aggiunto l' else if
                    //che controlla che IdToMod sia uguale a zero. Se lo è allora vuol dire
                    //che esiste già un record inserito con quei campi e perciò manda messaggio di errore giusto
                    else
                    {
                        sScript = "GestAlert('a', 'success', '', '', 'Inserimento avvenuto correttamente.')";
                    }


                    RegisterScript(sScript,this.GetType());
                    DataBindGridTariffe(LoadTariffe(), true);
                }
                else if (esito == -2)
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile inserire la Tariffa. Record duplicato.')";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile salvare la Tariffa.')";
                    RegisterScript(sScript,this.GetType());
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.btSalva_Click.errore: ", Err);
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
            try {
                string sScript = null;
                if (SharedFunction.IsNullOrEmpty(txbIdRecordToMod.Text) || txbIdRecordToMod.Text == "0")
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.')";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    int esito = 0;
                    int IdToMod = 0;
                    int Anno = 0;

                    Anno = SharedFunction.IntTryParse(lblAnnoEdit.Text, -1);
                    //*** 20130610 - ruolo supplettivo ***
                    if (ElaborazioniEffettuate(Anno, Ruolo.E_TIPO.ORDINARIO))
                    {
                        sScript = "GestAlert('a', 'warning', '', '', 'Impossibile eliminare l\'elemento poiché esiste un eleaborazione per l\'anno di riferimento.')";
                        RegisterScript(sScript,this.GetType());
                        return;
                    }
                    //*** ***

                    IdToMod = SharedFunction.IntTryParse(txbIdRecordToMod.Text, 0);

                    if (IdToMod > 0)
                    {
                        esito = MetodiTariffe.DeleteTariffa(IdToMod);
                        //*** 20140416 - aggiunto controllo sul valore di esito
                        if (esito == 1)
                        {
                            esito = 0;
                        }
                        else { esito = -1; }
                    }
                    else
                    {
                        Log.Error("Id record non valido");
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore del sistema')";
                        RegisterScript(sScript,this.GetType());
                    }

                    /*
                    * CONTROLLI ESITI
                    * 
                    * */
                    if (esito == 0)
                    {
                        sScript = "GestAlert('a', 'success', '', '', 'Cancellazione avvenuta con successo.')";
                        RegisterScript(sScript,this.GetType());

                        DataBindGridTariffe(LoadTariffe(),  true);

                        txbIdRecordToMod.Text = null;
                        pnlModifica.Visible = false;
                        pnlInserisci.Visible = false;
                    }
                    else
                    {
                        sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! Non è possibile cancellare questo elemento')";
                        RegisterScript(sScript,this.GetType());
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.btDel_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btRibalta_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtRibaltaDa.Text == "" || txtRibaltaA.Text == "")
                {
                    string sScript = " GestAlert('a', 'warning', '', '', 'Compilare i Campi \"Da:\" e \"A:\".')";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    int esito = 0;
                    int ribaltaDa = int.Parse(txtRibaltaDa.Text);
                    int ribaltaA = int.Parse(txtRibaltaA.Text);

                    esito = MetodiTariffe.SetRibalta(ribaltaDa, ribaltaA, (string)DichiarazioneSession.IdEnte);

                    if (esito == 0)
                    {
                        string sScript = " GestAlert('a', 'success', '', '', 'Ribaltamento avvenuto con successo.')";
                        RegisterScript(sScript,this.GetType());
                    }
                    else
                    {
                        if (esito == -1)
                        {
                            string sScript = " GestAlert('a', 'warning', '', '', 'Impossibile Ribaltare alla data inserita. \nData già presente.') ";
                            RegisterScript(sScript,this.GetType());
                        }
                        else
                        {
                            string sScript = " GestAlert('a', 'warning', '', '', 'Impossibile Ribaltare alla data inserita.')";
                            RegisterScript(sScript,this.GetType());
                        }
                    }
                }                                                              
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneTariffe.btRibalta_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anno"></param>
        /// <param name="TipoRuolo"></param>
        /// <returns></returns>
        private bool ElaborazioniEffettuate(int anno, Ruolo.E_TIPO TipoRuolo)
		{
            ElaborazioneEffettuata elab = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
			return (elab != null);
		}
	}
}
