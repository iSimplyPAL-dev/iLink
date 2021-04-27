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
using IRemInterfaceOSAP;
using log4net;
namespace OPENgovTOCO
{
	/// <summary>
	/// Pagina per la visualizzazione/gestione delle rate.
/// Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
	/// </summary>
	public partial class ConfigurazioneRate :BasePage
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurazioneRate));

        protected void Page_Load(object sender, System.EventArgs e)
		{
            try {
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerHtml += " - Elaborazione - Configurazione Rate";

                if (!Page.IsPostBack)
                {
                    ControlsDataBind();
                    SharedFunction.EnableDisableButton(ref btnAggiungiRata, false);
                    SharedFunction.EnableDisableButton(ref btnSalvaRate, false);
                    Session.Remove("ObjConfigurazioneRate");
                    divRateConfig.Style.Add("display", "none");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.Page_load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

		#region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            try {
                InitializeComponent();
                base.OnInit(e);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.OnInit.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent()
        {
            try {

                this.btnAggiungiRata.Click += new System.Web.UI.ImageClickEventHandler(this.btnAggiungiRata_Click);
                this.btnSalvaRate.Click += new System.Web.UI.ImageClickEventHandler(this.btnSalvaRate_Click);
                this.ddlAnno.SelectedIndexChanged += new System.EventHandler(this.ddlAnno_SelectedIndexChanged);
                //this.GrdRate.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdRate_ItemCommand);
                //this.GrdRate.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdRate_ItemDataBound);
                this.Load += new System.EventHandler(this.Page_Load);
                //*** 20130610 - ruolo supplettivo ***
                this.OptOrdinario.CheckedChanged += new System.EventHandler(this.ddlAnno_SelectedIndexChanged);
                this.OptSuppletivo.CheckedChanged += new System.EventHandler(this.ddlAnno_SelectedIndexChanged);
                //*** ***
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.InitializeComponent.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
		#endregion

		#region Events

		protected void ddlAnno_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try {
                //*** 20130610 - ruolo supplettivo ***
                Ruolo.E_TIPO TipoRuolo = Ruolo.E_TIPO.ORDINARIO;

                if (OptOrdinario.Checked == false && OptSuppletivo.Checked == false)
                {
                    //string msg="Selezionare il tipo di ruolo da elaborare.";
                    //string sScript = "GestAlert('a', 'warning', '', '', '"+msg+"');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "ErrSelTipo", "<script language=\'javascript\'>" + sScript + "</script>");
                }
                else
                {
                    if (OptSuppletivo.Checked == true)
                        TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO;
                    //*** ***
                    if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                    {
                        divRateConfig.Style.Add("display", "");

                        int Anno = int.Parse(ddlAnno.SelectedValue);
                        ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(Anno, TipoRuolo, DichiarazioneSession.CodTributo(""));
                        if (myRuolo != null)
                        {
                            Rate[] config = MetodiConfigurazioneRate.GetConfigurazioneRate(myRuolo.IdFlusso);

                            // Non configurato nulla per l'anno scelto
                            if (config.Length == 0)
                            {
                                config = new Rate[1];
                                config[0] = CreaRata(Anno, 0, myRuolo.IdFlusso);
                            }

                            GrdRate.DataSource = config;
                            GrdRate.DataBind();

                            SharedFunction.EnableDisableButton(ref btnAggiungiRata, true);
                            SharedFunction.EnableDisableButton(ref btnSalvaRate, true);

                            Session["ObjConfigurazioneRate"] = config;
                        }
                        else
                        {
                            string msg = "Ruolo non ancora elaborato.\nImpossibile configurare le rate!";
                            string sScript = "GestAlert('a', 'warning', '', '', '"+msg+"');";
                            RegisterScript(sScript,this.GetType());
                        }
                    }
                    else
                    {
                        SharedFunction.EnableDisableButton(ref btnAggiungiRata, false);
                        SharedFunction.EnableDisableButton(ref btnSalvaRate, false);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.ddlAnno_SelectedIndexChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

		protected void btnAggiungiRata_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
            try {
                UpdateObjects();

                Rate[] config = (Rate[])Session["ObjConfigurazioneRate"];
                int Anno = int.Parse(ddlAnno.SelectedValue);

                ArrayList configList = new ArrayList();
                configList.AddRange(config);

                // C'è solo soluzione unica, mi imposto per inserire sol. unica e almeno 2 rate
                if (config.Length == 1)
                {
                    configList.Add(CreaRata(Anno, 1, config[0].IdFlusso));
                    configList.Add(CreaRata(Anno, 2, config[0].IdFlusso));
                }
                else
                {
                    configList.Add(CreaRata(Anno, configList.Count, config[0].IdFlusso));
                }

                config = (Rate[])configList.ToArray(typeof(Rate));

                GrdRate.DataSource = config;
                GrdRate.DataBind();

                Session["ObjConfigurazioneRate"] = config;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.btnAggiungiRata_click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

        #region "Griglie"
        protected void GrdRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                TableCell itemCell = SharedFunction.CellByName(e.Row, "Numero Rata");
                if (itemCell != null && itemCell.FindControl("txtNRata") != null)
                {
                    string strNRata = ((TextBox)itemCell.FindControl("txtNRata")).Text;
                    TableCell deleteCell = SharedFunction.CellByName(e.Row, " ");
                    ImageButton imgDelete = (ImageButton)deleteCell.FindControl("imgDelete");
                    if (strNRata.CompareTo("U") == 0)
                    {
                        imgDelete.Visible = false;
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.GrdRowDataBound.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UpdateObjects();

                if (e.CommandSource is ImageButton)
                {
                    switch (((ImageButton)e.CommandSource).CommandName)
                    {
                        case "Delete": DeleteItem(e); break;
                        default: break;

                    }
                }

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.GrdRowCommand.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
  //      private void GrdRate_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		//{
        //try{

  //          UpdateObjects ();

		//	if (e.CommandSource is ImageButton)
		//	{
		//		switch(((ImageButton)e.CommandSource).CommandName)
		//		{
		//			case "Delete": DeleteItem(e); break;
		//			default: break;

		//		}
		//	}
		//}
  //      private void GrdRate_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
  //      {
  //          TableCell itemCell = SharedFunction.CellByName(e.Item, "Numero Rata");
  //          if (itemCell != null && itemCell.FindControl("txtNRata") != null)
  //          {
  //              string strNRata = ((TextBox)itemCell.FindControl("txtNRata")).Text;
  //              TableCell deleteCell = SharedFunction.CellByName(e.Item, " ");
  //              ImageButton imgDelete = (ImageButton)deleteCell.FindControl("imgDelete");
  //              if (strNRata.CompareTo("U") == 0)
  //              {
  //                  imgDelete.Visible = false;
  //              }
  //          }
  //        }
  // catch (Exception Err)
  //   {
  //      Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.btnSalvaRate_Click.errore: ", Err);
  //    Response.Redirect("../../PaginaErrore.aspx");
  // }
   //      }
    #endregion

    protected void btnSalvaRate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
            try {
                Rate[] config = (Rate[])Session["ObjConfigurazioneRate"];

                UpdateObjects();


                int Anno = int.Parse(ddlAnno.SelectedValue);

                string sScript = string.Empty;

                // Per la rata unica controllo soltanto che ci sia una data di scadenza inserita e valida
                if (config[0].DataScadenza.CompareTo(DateTime.MinValue) == 0)
                    sScript += "- Data di scadenza per la soluzione unica non valida\\n";

                // Per le rate controllo sia data scadenza valida che data scad rata i > data scad rata (i - 1)
                checked
                {
                    for (int i = 1; i < config.Length; i++)
                    {
                        Rate r = config[i];
                        Rate prec = config[i - 1];
                        if (r.DataScadenza.CompareTo(DateTime.MinValue) == 0)
                            sScript += "- Data di scadenza per la rata " + i + " non valida\\n";
                        // Se i == 1 è la prima rata, non ne ha di precedenti, quindi niente controllo
                        else if (i > 1 && r.DataScadenza.CompareTo(prec.DataScadenza) <= 0)
                            sScript += "- La rata " + i + " deve avere una scadenza successiva alla rata " + (i - 1) + "\\n";
                    }
                }
                if (sScript.CompareTo(string.Empty) == 0)
                {
                    MetodiConfigurazioneRate.InsertConfigurazioneRate(config);
                    sScript = "GestAlert('a', 'success', '', '', 'Configurazione rate memorizzata correttamente');";
                }
                else
                sScript = "GestAlert('a', 'warning', '', '', '" + sScript + "');";
                RegisterScript(sScript,this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.btnSalvaRate_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

		#endregion Events

		#region Private Methods

		private void ControlsDataBind ()
		{
			LoadDdlAnno ();
		}

		private void LoadDdlAnno ()
		{
            //ddlAnno.Items.Clear();
            //int EndYear = DateTime.Now.Year + 2;
            //int StartYear = DateTime.Now.Year - 5;
            //ddlAnno.Items.Add (string.Empty);
            //for (int i = EndYear; i >= StartYear; i--)
            //    ddlAnno.Items.Add (i.ToString ());
            //ddlAnno.SelectedValue = string.Empty;
            try {
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.LoadDdlAnno.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

		private Rate CreaRata (int Anno, int NRata, int IdFlusso)
		{
            try {
                Rate c = new Rate();
                //*** 20130610 - ruolo supplettivo ***
                c.IdFlusso = IdFlusso;
                //*** ***
                c.IdEnte = DichiarazioneSession.IdEnte;
                c.Anno = Anno;
                if (NRata == 0)
                    c.NRata = "U";
                else
                    c.NRata = NRata.ToString();
                c.Descrizione = string.Empty;
                c.DataScadenza = DichiarazioneSession.MyDateMinValue;
                return c;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.CreaRata.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

		private void DeleteItem(GridViewCommandEventArgs e)
		{
			string strNRata = e.CommandArgument.ToString();
			int NRata = int.Parse (strNRata);

			Rate[] config = (Rate[])Session["ObjConfigurazioneRate"];

            try
            {
                ArrayList configList = new ArrayList();
                configList.AddRange(config);

                // Nessun controllo sulla soluzione unica perchè è una rata che non si può cancellare
                configList.RemoveAt(NRata);

                // E' rimasta solo più una rata, elimino anche quella perchè
                // ho solo più la soluzione unica
                if (configList.Count == 2)
                    configList.RemoveAt(1);

                // Se elimino una rata centrale devo ricompattarle tutte
                checked
                {
                    for (int i = 1; i < configList.Count; i++)
                        ((Rate)configList[i]).NRata = i.ToString();
                }
                config = (Rate[])configList.ToArray(typeof(Rate));

                GrdRate.DataSource = config;
                GrdRate.DataBind();

                Session["ObjConfigurazioneRate"] = config;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.DeleteItem.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

		private void UpdateObjects ()
		{
            Rate[] config = (Rate[])Session["ObjConfigurazioneRate"];

            try
            {
                foreach (GridViewRow item in GrdRate.Rows)
                {
                    TableCell nRataCell = SharedFunction.CellByName(item, "Numero Rata");
                    TableCell descrizioneCell = SharedFunction.CellByName(item, "Descrizione Rata");
                    TableCell scadenzaCell = SharedFunction.CellByName(item, "Data Scadenza");
                    string strNRata = ((TextBox)nRataCell.FindControl("txtNRata")).Text;
                    string descRata = ((TextBox)descrizioneCell.FindControl("txtDescRata")).Text;
                    string scadRata = ((TextBox)scadenzaCell.FindControl("txtDataScadenza")).Text;

                    foreach (Rate myRow in config)
                    {
                        if (myRow.NRata.CompareTo(strNRata) == 0)
                        {
                            myRow.Descrizione = descRata.Trim().ToUpper();
                            try
                            {
                                myRow.DataScadenza = DateTime.ParseExact(scadRata, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                            }
                            catch(Exception Err)
                            {
                                myRow.DataScadenza = DichiarazioneSession.MyDateMinValue;
                                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.UpdateObjects.errore: ", Err);
                                Response.Redirect("../../PaginaErrore.aspx");
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception Err)
            {
               
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneRate.UpdateObjects.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		#endregion Private Methods

	}
}
