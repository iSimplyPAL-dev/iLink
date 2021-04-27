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
using DichiarazioniICI.Database;
using Business;
using System.Globalization;
using System.Text;
using log4net;

namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina per la ricerca dei versamenti.
    /// Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class RicercaVersamenti :BaseEnte
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(RicercaVersamenti));
        protected System.Web.UI.WebControls.Label lblCodiceFiscale1;
		protected System.Web.UI.WebControls.Label lblNome1;
		protected System.Web.UI.WebControls.Label lblCognome1;

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
            //this.GrdRisultati.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdRisultati_ItemCommand);
            //this.GrdRisultati.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdRisultati_SortCommand);
        }
        #endregion
        /// <summary>
        /// Caricamento pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Session["DATI_ATTUALI"] = false;
                if (!IsPostBack)
                {
                    ViewState.Add("SortKey", "USERNAME");
                    ViewState.Add("OrderBy", "ASC");

                    if (ConstWrapper.Parametri != null)
                    {
                        txtAnnoRiferimento.Text =Utility.StringOperation.FormatString( ConstWrapper.Parametri["AnnoRiferimento"]);
                        txtCognome.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["Cognome"]);
                        txtNome.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["Nome"]);
                        txtCodiceFiscale.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["CodiceFiscale"]);
                        txtPartitaIVA.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["PartitaIVA"]);
                        ddlTipoPagamento.SelectedValue = Utility.StringOperation.FormatString(ConstWrapper.Parametri["TipoPagamento"] );
                        ddlTipologiaV.SelectedValue = Utility.StringOperation.FormatString(ConstWrapper.Parametri["Tipologia"] );
                        ddlImportoPagato.SelectedValue = Utility.StringOperation.FormatString(ConstWrapper.Parametri["ConfrontoImporto"]);
                        txtImportoPagato.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["Importo"]);
                        TxtDataRiversamentoDa.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["DataRiversDa"] );
                        TxtDataRiversamentoA.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["DataRiversA"] );
                        txtFlusso.Text = Utility.StringOperation.FormatString(ConstWrapper.Parametri["Flusso"]);

                        if (ConstWrapper.Parametri["CODTRIBUTO"] != null)
                        {
                            if (ConstWrapper.Parametri["CODTRIBUTO"].ToString() == Utility.Costanti.TRIBUTO_TASI)
                            {
                                chkICI.Checked = false; chkTASI.Checked = true;
                            }
                            else if (ConstWrapper.Parametri["CODTRIBUTO"].ToString() == "")
                            {
                                chkICI.Checked = true; chkTASI.Checked = true;
                            }
                        }
                        GrdRisultatiBind();
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void Page_Load(object sender, System.EventArgs e)
        //{
        //          try
        //          {
        //              Session["DATI_ATTUALI"] = false;
        //              if (!IsPostBack)
        //              {
        //                  ViewState.Add("SortKey", "USERNAME");
        //                  ViewState.Add("OrderBy", "ASC");

        //                  //int VersamentiDaBonificare = new ICIWeb.Database.VersamentiTable().GetVersamentiDaImportare();
        //                  //if (VersamentiDaBonificare > 0 && new DichiarazioniICI.Database.ImportazioneTable(ConstWrapper.sUsername).GetRow("BonVersamenti").Run == false)
        //                  //{
        //                  //    if (!Page.IsStartupScriptRegistered("popupdabonificare"))
        //                  //        RegisterScript(this.GetType(),"popupdabonificare", "<Script language=Javascript>window.showModalDialog('PopUpBonifica.aspx?TipoBonifica=bonversamenti&Messaggio=Sono presenti " + VersamentiDaBonificare.ToString() + " versamenti da bonificare!', window, 'dialogHeight: 200px; dialogWidth: 300px; status: no');</Script>");
        //                  //}

        //                  if (ConstWrapper.Parametri != null)
        //                  {
        //                      txtAnnoRiferimento.Text = ConstWrapper.Parametri["AnnoRiferimento"] == null ? String.Empty : ConstWrapper.Parametri["AnnoRiferimento"].ToString();
        //                      txtCognome.Text = ConstWrapper.Parametri["Cognome"] == null ? String.Empty : ConstWrapper.Parametri["Cognome"].ToString();
        //                      txtNome.Text = ConstWrapper.Parametri["Nome"] == null ? String.Empty : ConstWrapper.Parametri["Nome"].ToString();
        //                      txtCodiceFiscale.Text = ConstWrapper.Parametri["CodiceFiscale"] == null ? String.Empty : ConstWrapper.Parametri["CodiceFiscale"].ToString();
        //                      txtPartitaIVA.Text = ConstWrapper.Parametri["PartitaIVA"] == null ? String.Empty : ConstWrapper.Parametri["PartitaIVA"].ToString();
        //                      ddlTipoPagamento.SelectedValue = ConstWrapper.Parametri["TipoPagamento"] == null ? String.Empty : ConstWrapper.Parametri["TipoPagamento"].ToString();
        //                      ddlTipologiaV.SelectedValue = ConstWrapper.Parametri["Tipologia"] == null ? string.Empty : ConstWrapper.Parametri["Tipologia"].ToString();
        //                      ddlImportoPagato.SelectedValue = ConstWrapper.Parametri["ConfrontoImporto"] == null ? string.Empty : ConstWrapper.Parametri["ConfrontoImporto"].ToString();
        //                      txtImportoPagato.Text = ConstWrapper.Parametri["Importo"] == null ? string.Empty : ConstWrapper.Parametri["Importo"].ToString();
        //                      TxtDataRiversamentoDa.Text = ConstWrapper.Parametri["DataRiversDa"] == null ? string.Empty : ConstWrapper.Parametri["DataRiversDa"].ToString();
        //                      TxtDataRiversamentoA.Text = ConstWrapper.Parametri["DataRiversA"] == null ? string.Empty : ConstWrapper.Parametri["DataRiversA"].ToString();

        //                      //*** 20140630 - TASI ***
        //                      if (ConstWrapper.Parametri["CODTRIBUTO"] != null)
        //                      {
        //                          if (ConstWrapper.Parametri["CODTRIBUTO"].ToString() == Utility.Costanti.TRIBUTO_TASI)
        //                          {
        //                              chkICI.Checked = false; chkTASI.Checked = true;
        //                          }
        //                          else if (ConstWrapper.Parametri["CODTRIBUTO"].ToString() == "")
        //                          {
        //                              chkICI.Checked = true; chkTASI.Checked = true;
        //                          }
        //                      }
        //                      GrdRisultatiBind();
        //                      //*** ***
        //                  }
        //              }
        //          }
        //          catch (Exception Err)
        //          {
        //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.Page_Load.errore: ", Err);
        //              Response.Redirect("../../PaginaErrore.aspx");
        //          }
        //      }

        /// <summary>
        /// Esegue il bind della datagrid.
        /// </summary>
        /// <revisionHistory>
        /// <revision date="30/06/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        private void GrdRisultatiBind()
        {
            try
            {
                string Ente = ConstWrapper.CodiceEnte;
                string Tributo = string.Empty;
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;
                string AnnoRiferimento = txtAnnoRiferimento.Text;
                string Cognome = txtCognome.Text;
                string Nome = txtNome.Text;
                string CodiceFiscale = txtCodiceFiscale.Text;
                string PartitaIva = txtPartitaIVA.Text;
                int IdTipoPagamento = int.Parse(ddlTipoPagamento.SelectedValue);
                int IdTipologia = int.Parse(ddlTipologiaV.SelectedValue);
                int IdConfrontoImporto = int.Parse(ddlImportoPagato.SelectedValue);
                string Importo = txtImportoPagato.Text;
                double ImportoPagato = 0;
                string DataRiversamentoDa = TxtDataRiversamentoDa.Text;
                string DataRiversamentoA = TxtDataRiversamentoA.Text;

                ConstWrapper.Parametri = new System.Collections.Hashtable();
                ConstWrapper.Parametri["CODTRIBUTO"] = Tributo;
                ConstWrapper.Parametri["AnnoRiferimento"] = AnnoRiferimento;
                ConstWrapper.Parametri["Cognome"] = Cognome;
                ConstWrapper.Parametri["Nome"] = Nome;
                ConstWrapper.Parametri["CodiceFiscale"] = CodiceFiscale;
                ConstWrapper.Parametri["PartitaIVA"] = PartitaIva;
                ConstWrapper.Parametri["TipoPagamento"] = IdTipoPagamento;
                ConstWrapper.Parametri["Tipologia"] = IdTipologia;
                ConstWrapper.Parametri["ConfrontoImporto"] = IdConfrontoImporto;
                ConstWrapper.Parametri["Importo"] = Importo;
                ConstWrapper.Parametri["DataRiversDa"] = DataRiversamentoDa;
                ConstWrapper.Parametri["DataRiversA"] = DataRiversamentoA;
                ConstWrapper.Parametri["Flusso"] = Utility.StringOperation.FormatString(txtFlusso.Text);

                if (Importo == "")
                    Importo = "0";

                ImportoPagato = double.Parse(Importo);

                DataView Vista = new VersamentiView().List(ConstWrapper.StringConnection,Tributo, 0, Cognome, Nome, CodiceFiscale, PartitaIva, AnnoRiferimento, Ente, IdTipoPagamento, IdTipologia, IdConfrontoImporto, ImportoPagato, DataRiversamentoDa, DataRiversamentoA, Utility.StringOperation.FormatString(txtFlusso.Text));
                Session["VistaVersamenti"] = Vista;
                GrdRisultati.DataSource = Vista;

                if (Vista.Count != 0)
                {
                    lblRisultati.Text = "Risultati Ricerca";
                    int nPag = 0;
                    double impPag = 0;
                    foreach (DataRowView myRow in Vista)
                    {
                        nPag += 1;
                        impPag += Utility.StringOperation.FormatDouble(myRow["ImportoPagato"]);
                    }
                    LblNPag.Text = "N.Pagamenti:&emsp;" + nPag.ToString();
                    LblImpPag.Text = "per un Totale di €&emsp;" + impPag.ToString("#,##0.00");
                    GrdRisultati.DataBind();
                    GrdRisultati.Visible = true;
                }
                else
                {
                    lblRisultati.Text = "La ricerca non ha prodotto risultati.";
                    LblNPag.Text =string.Empty;
                    LblImpPag.Text = string.Empty;
                    GrdRisultati.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.GrdRisultatiBind.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                DivAttesa.Style.Add("display", "none");
            }
        }
        //private void GrdRisultatiBind()
        //{
        //          try
        //          {
        //              string Ente = ConstWrapper.CodiceEnte;
        //              //*** 20140630 - TASI ***
        //              string Tributo = string.Empty;
        //              if (chkICI.Checked == true && chkTASI.Checked == false)
        //                  Tributo = ConstWrapper.CodiceTributo;
        //              else if (chkICI.Checked == false && chkTASI.Checked == true)
        //                  Tributo = Utility.Costanti.TRIBUTO_TASI;
        //              //*** ***
        //              string AnnoRiferimento = txtAnnoRiferimento.Text;
        //              string Cognome = txtCognome.Text;
        //              string Nome = txtNome.Text;
        //              string CodiceFiscale = txtCodiceFiscale.Text;
        //              string PartitaIva = txtPartitaIVA.Text;
        //              int IdTipoPagamento = int.Parse(ddlTipoPagamento.SelectedValue);
        //              int IdTipologia = int.Parse(ddlTipologiaV.SelectedValue);
        //              int IdConfrontoImporto = int.Parse(ddlImportoPagato.SelectedValue);
        //              string Importo = txtImportoPagato.Text;
        //              double ImportoPagato = 0;
        //              string DataRiversamentoDa = TxtDataRiversamentoDa.Text;
        //              string DataRiversamentoA = TxtDataRiversamentoA.Text;

        //              ConstWrapper.Parametri = new System.Collections.Hashtable();
        //              //*** 20140630 - TASI ***
        //              ConstWrapper.Parametri["CODTRIBUTO"] = Tributo;
        //              //*** ***
        //              ConstWrapper.Parametri["AnnoRiferimento"] = AnnoRiferimento;
        //              ConstWrapper.Parametri["Cognome"] = Cognome;
        //              ConstWrapper.Parametri["Nome"] = Nome;
        //              ConstWrapper.Parametri["CodiceFiscale"] = CodiceFiscale;
        //              ConstWrapper.Parametri["PartitaIVA"] = PartitaIva;
        //              ConstWrapper.Parametri["TipoPagamento"] = IdTipoPagamento;
        //              ConstWrapper.Parametri["Tipologia"] = IdTipologia;
        //              ConstWrapper.Parametri["ConfrontoImporto"] = IdConfrontoImporto;
        //              ConstWrapper.Parametri["Importo"] = Importo;
        //              ConstWrapper.Parametri["DataRiversDa"] = DataRiversamentoDa;
        //              ConstWrapper.Parametri["DataRiversA"] = DataRiversamentoA;

        //              // se arrivo dall'accertamento nn pulisco la variabile di sessione
        //              //if (Session["VersFromAccertamenti"] == null)
        //              //{
        //              //    Session["SEARCHPARAMETRES"] = Session["SEARCHPARAMETRES1"];
        //              //}

        //              if (Importo == "")
        //                  Importo = "0";

        //              ImportoPagato = double.Parse(Importo);

        //              //*** 20140630 - TASI ***
        //              //DataView Vista = new VersamentiView().List(Cognome, Nome, CodiceFiscale, PartitaIva, AnnoRiferimento, Ente, IdTipoPagamento, IdTipologia, IdConfrontoImporto, ImportoPagato, DataRiversamentoDa, DataRiversamentoA);
        //              DataView Vista = new VersamentiView().List(Tributo,0, Cognome, Nome, CodiceFiscale, PartitaIva, AnnoRiferimento, Ente, IdTipoPagamento, IdTipologia, IdConfrontoImporto, ImportoPagato, DataRiversamentoDa, DataRiversamentoA);
        //              Session["VistaVersamenti"] = Vista;
        //              //if (Vista.Count > 0)
        //              //    Vista.Sort = "AnnoRiferimento";
        //              GrdRisultati.DataSource = Vista;

        //              if (Vista.Count != 0)
        //              {
        //                  lblRisultati.Text = "Risultati Ricerca";
        //                  GrdRisultati.DataBind();
        //                  GrdRisultati.Visible = true;
        //              }
        //              else
        //              {
        //                  lblRisultati.Text = "La ricerca non ha prodotto risultati.";
        //                  GrdRisultati.Visible = false;
        //              }
        //          }
        //          catch (Exception ex)
        //          {
        //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.GrdRisultatiBind.errore: ", ex);
        //              Response.Redirect("../../PaginaErrore.aspx");
        //          }
        //          finally
        //          {
        //              DivAttesa.Style.Add("display", "none");
        //          }
        //}

        /// <summary>
        /// Esegue la ricerca.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTrova_Click(object sender, System.EventArgs e)
		{
			GrdRisultatiBind();
		}

        #region "Griglie"
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_VERSAMENTI", "?IDVersamento=" + IDRow + "&TYPEOPERATION=GESTIONE"), this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.GrdRowCommand.errore: ", ex);
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
            LoadSearch(e.NewPageIndex);
        }
        //      private void GrdRisultati_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        //	switch(e.CommandName)
        //	{
        //		case "Select":
        //			//ApplicationHelper.LoadFrameworkPage("*", "?IDVersamento=" + GrdRisultati.DataKeys[e.Item.ItemIndex] + "&TYPEOPERATION=GESTIONE");
        //			ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_VERSAMENTI", "?IDVersamento=" + GrdRisultati.DataKeys[e.Item.ItemIndex] + "&TYPEOPERATION=GESTIONE");
        //			break;
        //	}
        //}
        //private void GrdRisultati_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        //{
        //try{
        //	if (e.SortExpression.ToString().CompareTo(ViewState["SortKey"].ToString()) == 0)
        //	{
        //		switch (ViewState["OrderBy"].ToString())
        //		{
        //			case "ASC":
        //				ViewState["OrderBy"] = "DESC";
        //				break;
        //			case "DESC":
        //				ViewState["OrderBy"] = "ASC";
        //				break;
        //		}
        //	}
        //	else
        //	{
        //		ViewState["SortKey"] = e.SortExpression.ToString();
        //		ViewState["OrderBy"] = "ASC";
        //	}
        //	DataView VistaOrdinata;
        //	VistaOrdinata = (DataView)Session["VistaVersamenti"];
        //	VistaOrdinata.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];
        //	Session["VistaVersamenti"] = VistaOrdinata;

        //	//DataTable TabSource = (DataTable)Session["TABELLA_RICERCA"];	
        //	GrdRisultati.DataSource = VistaOrdinata;
        //	GrdRisultati.DataBind();
        //  }
        //    catch (Exception ex)
        //   {
        //     log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.GrdRisultati_SortCommand.errore: ", ex);
        //      Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}
        #endregion
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdRisultati.DataSource = (DataView)(Session["VistaVersamenti"]);
                if (page.HasValue)
                    GrdRisultati.PageIndex = page.Value;
                GrdRisultati.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }

		protected void btnNuovoVersamento_Click(object sender, System.EventArgs e)
		{
            RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_VERSAMENTI", "?TYPEOPERATION=GESTIONE"), this.GetType());
		}

 		private DataSet CreateDataSetVersamenti()
		{
			DataSet dsTmp = new DataSet();
            try { 
			dsTmp.Tables.Add("VERSAMENTI");
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            //*** 20140630 - TASI ***
            dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
            //*** ***
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			/**** 20120828 - IMU adeguamento per importi statali ****/
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			/**** ****/
			/**** 20130422 - aggiornamento IMU ****/
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			dsTmp.Tables["VERSAMENTI"].Columns.Add("").DataType = typeof(string);
			/**** ****/

			return dsTmp;

            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.CreaDataSetVersamenti.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }

		/// <summary>
		/// Genera la stampa Excel dei versamenti in base ai parametri di ricerca passati.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnStampaExcel_Click(object sender, System.EventArgs e)
		{
			DataRow dr;
			DataSet ds;
			DataTable dtVersamenti;
			string NameXLS =string.Empty;

			ArrayList arratlistNomiColonne;
			string[] arraystr;
			arratlistNomiColonne = new ArrayList();
			int x;

			IFormatProvider culture = new CultureInfo("it-IT", true);
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");
            try { 
			for (x=0;x<=32;x++)
			{
				arratlistNomiColonne.Add("");
			}
			
			arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

            NameXLS =ConstWrapper.CodiceEnte+ "_VERSAMENTI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
			                
			ds = CreateDataSetVersamenti();

			dtVersamenti = ds.Tables["VERSAMENTI"];

			//inserisco intestazione di colonna
			// COMUNE DI DESCRIZIONE ENTE
			dr = dtVersamenti.NewRow();
			dr[0] = ConstWrapper.DescrizioneEnte;
			dr[3]= "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;;
			dtVersamenti.Rows.Add(dr);

			//aggiungo uno spazio
			dr = dtVersamenti.NewRow();					
			dtVersamenti.Rows.Add(dr);

			//inserisco intestazione - titolo prospetto + data stampa
			dr = dtVersamenti.NewRow();
			dr[0] = "Prospetto Versamenti".ToUpper();
			//dr[2] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
			dtVersamenti.Rows.Add(dr);

			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);
			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);

			//inserisco intestazione di colonna
			dr = dtVersamenti.NewRow();
			x=0;
			dr[x] = "Anno Riferimento";
			x++;
			dr[x] = "Codice Fiscale";
			x++;
			dr[x] = "Partita Iva";
			x++;
			dr[x] = "Cognome";
			x++;
			dr[x] = "Nome";
            //*** 20140630 - TASI ***
            x++;
            dr[x] = "Tributo";
            //*** ***
			x++;
			dr[x] = "Data Pagamento";
			x++;
			dr[x] = "Data Riversamento";
			x++;
			dr[x] = "Tipo Pagamento";					
			x++;
			dr[x] = "N° Fabb.";
			x++;
			dr[x] = "Importo Abitaz. Principale";
			/**** 20120828 - IMU adeguamento per importi statali ****/
			x++;
			dr[x] = "Importo Altri Fabbricati Comune";
			x++;
			dr[x] = "Importo Altri Fabbricati Stato";
			x++;
			dr[x] = "Importo Aree Fabbricabili Comune";
			x++;
			dr[x] = "Importo Aree Fabbricabili Stato";
			x++;
			dr[x] = "Importo Terreni Agr. Comune";
			x++;
			dr[x] = "Importo Terreni Agr. Stato";
			x++;
			dr[x] = "Importo Fabbricati Rurali Uso Strumentale";
			/**** ****/
			/**** 20130422 - aggiornamento IMU ****/
			x++;
			dr[x] = "Importo Fabbricati Rurali Uso Strumentale Stato";
			x++;
			dr[x] = "Importo Uso Prod.Cat.D";
			x++;
			dr[x] = "Importo Uso Prod.Cat.D Stato";
			/**** ****/
			x++;
			dr[x] = "Det. Abitaz. Principale";
			x++;
			dr[x] = "Importo Pagato";
			x++;
			dr[x] = "Importo Imposta";
			x++;
			dr[x] = "Importo Soprattassa";
			x++;
			dr[x] = "Importo PenaPecuniaria";
			x++;
			dr[x] = "Interessi";
			x++;
			dr[x] = "Data Provvedimento";
			x++;
			dr[x] = "Numero Provvedimento";
			x++;
			dr[x] = "Arrotondamento";
			x++;
			dr[x] = "Provenienza";
			//13012009 Fabi
			x++;
			dr[x] = "Tipo versamento";
			x++;
			dr[x] = "Note";			
			dtVersamenti.Rows.Add(dr);
			DataView  dvVers=new DataView();
			dvVers=(DataView)Session["VistaVersamenti"];
                foreach (DataRow myRow in dvVers.Table.Rows)
                {
                    dr = dtVersamenti.NewRow();
				x=0;
				dr[x] = " " + myRow["ANNORIFERIMENTO"].ToString();
				x++;
				dr[x] = myRow["CodiceFiscale"].ToString();
				x++;
				dr[x] = " " + myRow["PartitaIVA"].ToString();

				string cognome="";

				/* 05092007 se la data di morte è valorizzata ed è diversa da minvalue cognome=EREDE DI cognome **/
				if((myRow["DATA_MORTE"] != DBNull.Value)&&(myRow["DATA_MORTE"].ToString() != null))
				{
					DateTime dataM;
					string dataMorte, giornoM, meseM, annoM;
					dataMorte=myRow["DATA_MORTE"].ToString();
					if (dataMorte!="")
					{
						giornoM = dataMorte.Substring(6,2);
						meseM = dataMorte.Substring(4,2);
						annoM = dataMorte.Substring(0,4);
						dataM = new DateTime(Convert.ToInt16(annoM),Convert.ToInt16(meseM),Convert.ToInt16(giornoM));
					}
					else{dataM = DateTime.MinValue;}
					if (dataM != DateTime.MinValue)
						cognome = "EREDE DI " + myRow["COGNOME"].ToString();
					else
						cognome = myRow["COGNOME"].ToString();
				}
				else
				{
					cognome = myRow["COGNOME"].ToString();
				}

				x++;
				dr[x] = cognome;
				x++;
				dr[x] = myRow["NOME"].ToString();
                //*** 20140630 - TASI ***
				x++;
                dr[x] = myRow["DESCRTRIBUTO"].ToString();
                //*** ***
                x++;
				dr[x] = " " + DateTime.Parse(myRow["Datapagamento"].ToString(),culture).ToString("dd/MM/yyyy");
				x++;
				if (myRow["DataRiversamento"] !=DBNull.Value && myRow["DataRiversamento"].ToString () !=""  ) 
					dr[x] =  " " + DateTime.Parse(myRow["DataRiversamento"].ToString(),culture).ToString("dd/MM/yyyy");
				else
					dr[x] =  " ";

				x++;
                dr[x] = Business.CoreUtility.FormattaGrdAccontoSaldo(myRow["Saldo"].ToString(), myRow["Acconto"].ToString());				
				x++;
				dr[x] = myRow["NumeroFabbricatiPosseduti"].ToString();
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoAbitazPrincipale"]);
				/**** 20120828 - IMU adeguamento per importi statali ****/
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoAltriFabbric"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoAltriFabbricStatale"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoAreeFabbric"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoAreeFabbricStatale"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImpoTerreni"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoTerreniStatale"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoFabRurUsoStrum"]);
				/**** ****/
				/**** 20130422 - aggiornamento IMU ****/
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoFabRurUsoStrumStatale"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoUsoProdCatD"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoUsoProdCatDStatale"]);
				/**** ****/
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["DetrazioneAbitazPrincipale"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoPagato"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoImposta"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoSoprattassa"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoPenaPecuniaria"]);
				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["Interessi"]);
				x++;
				if (myRow["DataProvvedimentoViolazione"] !=DBNull.Value && myRow["DataProvvedimentoViolazione"].ToString () !="" && Convert.ToDateTime(myRow["DataProvvedimentoViolazione"].ToString()) != DateTime.MinValue ) 
					dr[x] =  " " + DateTime.Parse(myRow["DataProvvedimentoViolazione"].ToString(),culture).ToString("dd/MM/yyyy");
				else
					dr[x] =  " ";

				x++;
				if (myRow["NumeroAttoAccertamento"]!=DBNull.Value && myRow["NumeroAttoAccertamento"].ToString()!="")
					dr[x] = myRow["NumeroAttoAccertamento"].ToString();
				else
					dr[x] = "";

				x++;
				dr[x] = Business.CoreUtility.FormattaGrdEuro(myRow["ImportoPagatoArrotondamento"]);
				x++;
				dr[x] = myRow["DescProvenienza"].ToString();
				
				//Fabi 13012009
				x++;
				if(Convert.ToBoolean(myRow["violazione"].ToString()) == false  && Convert.ToBoolean(myRow["ravvedimentoOperoso"].ToString()) ==false)
					dr[x] = "Ordinario";
				
				if(Convert.ToBoolean(myRow["violazione"].ToString()) == true && Convert.ToBoolean(myRow["ravvedimentoOperoso"].ToString())==false)
					dr[x] = "Violazione";

				if(Convert.ToBoolean(myRow["violazione"].ToString())== false && Convert.ToBoolean(myRow["ravvedimentoOperoso"].ToString())==true)
					dr[x] = "Ravvedimento operoso";

				x++;
				dr[x] = myRow["note"].ToString();
                    
				dtVersamenti.Rows.Add(dr);
			}
			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);
			//inserisco riga vuota
			dr = dtVersamenti.NewRow();
			dtVersamenti.Rows.Add(dr);

			//inserisco numero totali di contribuenti
			dr = dtVersamenti.NewRow();
			dr[0] = "Totale Versamenti: " + (dvVers.Count);
			dtVersamenti.Rows.Add(dr);

            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.btnStampaExcel_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
			//definisco l'insieme delle colonne da esportare
			int[] iColumns ={ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23,24,25,26,27,28,29,30,31,32};
			//esporto i dati in excel
			RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
			objExport.ExportDetails(dtVersamenti, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        }
		protected void btnIndietro_Click(object sender, System.EventArgs e)
		{
            // VersFromAccertamenti --> Variabile di sessione valorizzata 
            // in gestioneAccertamenti.aspx
            try { 
			if (Session["VersFromAccertamenti"] != null)
			{
				if (Session["VersFromAccertamenti"].ToString().ToLower() == "true"){
					// devo ritornare alla pagina di gestione dell'accertamento.
					StringBuilder strBuild = new StringBuilder();
					strBuild.Append("parent.Comandi.location.href='../Provvedimenti/GestioneAccertamenti/ComandiGestioneAccertamenti.aspx';");
					strBuild.Append("parent.Visualizza.location.href='../Provvedimenti/GestioneAccertamenti/GestioneAccertamenti.aspx?DaVersamenti=true';");
					//*strBuild.Append("parent.nascosto.location.href='../Provvedimenti/';");
					RegisterScript(strBuild.ToString(),this.GetType() );
					Session.Remove("VersFromAccertamenti");
				}
			}
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaVersamenti.btnIndietro_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }
	}
}
