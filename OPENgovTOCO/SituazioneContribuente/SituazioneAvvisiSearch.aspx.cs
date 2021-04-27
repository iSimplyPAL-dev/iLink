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
using Anagrafica.DLL;
using IRemInterfaceOSAP;
using DTO;
using System.Data.SqlClient;
using log4net;

namespace OPENgovTOCO.SituazioneContribuente
{
	/// <summary>
	/// Pagina per la ricerca avvisi.
	/// </summary>
	public partial class SituazioneAvvisiSearch :BasePage
	{
		protected System.Web.UI.WebControls.ImageButton ImageCompleta;
		protected System.Web.UI.WebControls.ImageButton Imagebutton3;
		protected System.Web.UI.WebControls.Label Label35;
		protected System.Web.UI.WebControls.Label Label27;
		protected System.Web.UI.WebControls.Panel PanelImmobile;

        protected SqlCommand cmdMyCommand;
        protected SqlDataReader myDataReader;
        protected DataTable dtMyDati;

        protected readonly ILog Log = LogManager.GetLogger(typeof(SituazioneAvvisiSearch));
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
			//this.GrdAvvisi.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdAvvisi_ItemCommand);

		}
		#endregion

		#region Events

		protected void Page_Load(object sender, System.EventArgs e)
		{
            try {
                DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerText += " - Elaborazione - Avvisi";

                if (!Page.IsPostBack)
                {
                    ControlsDataBind();
                    if (Request.QueryString["IdEnte"] != null)
                    {
                        Session["COD_ENTE"] = Request.QueryString["IdEnte"];
                        DTO.CartellaSearch SearchParams = new DTO.CartellaSearch();
                        SearchParams.IdTributo = DichiarazioneSession.CodTributo(Request.QueryString["CodTributo"]);
                        SearchParams.CognomeContribuente =(Request.QueryString["Cognome"] == null ? string.Empty : Request.QueryString["Cognome"]);
                        SearchParams.NomeContribuente = (Request.QueryString["Nome"] == null ? string.Empty : Request.QueryString["Nome"]);
                        SearchParams.CodFiscaleContribuente = (Request.QueryString["CodiceFiscale"] == null ? string.Empty : Request.QueryString["CodiceFiscale"]);
                        SearchParams.PIVAContribuente =(Request.QueryString["PartitaIva"] == null ? string.Empty : Request.QueryString["PartitaIva"]);
                        SearchParams.NumeroAvviso = string.Empty;
                        Session["objRicercaAvvisiTOCO"] = SearchParams;
                    }
                    RestoreSearch();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearc.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        #region "Griglie"
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    Response.Redirect(OSAPPages.SituazioneAvvisi + "?IdCartella=" + IDRow);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(".GrdRowCommand::errore::", ex);
                //Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            DataBindGridCartelle((Cartella[])Session["objRicercaAvvisiTOCOCartelle"], 0, false, e.NewPageIndex);
        }
  //      protected void GrdAvvisi_ItemCommand(Object sender, DataGridCommandEventArgs e)
		//{
		//	if (e.CommandSource is ImageButton)
		//	{
		//		switch(((ImageButton)e.CommandSource).CommandName)
		//		{
		//			case "Visualizza":
		//				VisualizzaAvviso(e);
		//				break;
		//			default:
		//				break;
		//		}
		//	}
		//}
        #endregion
		protected void Search_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
            try
            {
                connessioneDB();
                Session["objRicercaAvvisiTOCO"] = null;
                Session["objRicercaAvvisiTOCOCartelle"] = null;

                txtCognome.Text = txtCognome.Text.Trim();
                txtNome.Text = txtNome.Text.Trim();
                txtCodFiscale.Text = txtCodFiscale.Text.Trim();
                txtPIva.Text = txtPIva.Text.Trim();
                txtNumeroAvviso.Text = txtNumeroAvviso.Text.Trim();
                int iAnno = -1;
                if (ddlAnnoAvviso.SelectedIndex > 0)
                    iAnno = int.Parse(ddlAnnoAvviso.SelectedValue);

                DTO.CartellaSearch SearchParams = new DTO.CartellaSearch();
                SearchParams.IdTributo=DichiarazioneSession.CodTributo("");
                SearchParams.CognomeContribuente = txtCognome.Text;
                SearchParams.NomeContribuente = txtNome.Text;
                SearchParams.CodFiscaleContribuente = txtCodFiscale.Text;
                SearchParams.PIVAContribuente = txtPIva.Text;
                SearchParams.IsSgravate = ChkSgravate.Checked;

                SearchParams.NumeroAvviso = txtNumeroAvviso.Text;
                SearchParams.Anno = iAnno;

                Cartella[] cartelle = MetodiCartella.CartelleSearch(SearchParams, null);

                DataBindGridCartelle(cartelle, 0, true);

                Session["objRicercaAvvisiTOCO"] = SearchParams;
                Session["objRicercaAvvisiTOCOCartelle"] = cartelle;
            }
            catch (Exception  Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearch.Search_Click.errore: ", Err);
               Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }
		}
        protected void Stampa_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DataTable dtStampa = null;
            string NameXLS = "";
            int[] arrColumnsToExport = null;
            string[] arrHeaders = null;
            Cartella[] cartelle = null;
            try
            {
                NameXLS = DichiarazioneSession.IdEnte +"_ELENCOAVVISI_" +   DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                if (Session["objRicercaAvvisiTOCOCartelle"] != null)
                {
                    cartelle = (Cartella[])Session["objRicercaAvvisiTOCOCartelle"];
                    dtStampa = MetodiCartella.PrintAvvisi(cartelle);

                    arrHeaders = new string[dtStampa.Columns.Count];
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
                }
                else
                {
                    RegisterScript("GestAlert('a', 'warning', '', '', 'Effettuare prima la ricerca.');", this.GetType());
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearch.Stampa_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            if (dtStampa != null)
            {
                RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
                objStampa.ExportDetails(dtStampa, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }
        }

        public void connessioneDB()
        {
            //*** 20140409
            try {
                Log.Debug("SituazioneContribuente::SituazioneAvvisiSearch::connessioneDB::apertura della connessione al DB");
                cmdMyCommand = new SqlCommand();
                myDataReader = null;
                dtMyDati = new DataTable();

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearc.connessioneDB.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
		
/*
		private sub GrdAvvisi_SortCommand(Object sender, DataGridSortCommandEventArgs e)
Dim strSortKey As String
		Dim dt As DataView

					Try
					If e.SortExpression.ToString() = viewstate("SortKey").ToString() Then
																						Select Case viewstate("OrderBy").ToString()
																																Case "ASC"
		viewstate("OrderBy") = "DESC"

		Case "DESC"
		viewstate("OrderBy") = "ASC"
		End Select
			Else
				viewstate("SortKey") = e.SortExpression
										   viewstate("OrderBy") = "ASC"
		End If

			dt = objDS.Tables(0).DefaultView
									 dt.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")

		GrdAnagrafica.start_index = 0
		GrdAnagrafica.DataSource = dt
									   GrdAnagrafica.DataBind()
									   Catch Err As Exception
													 Log.Debug("Si è verificato un errore in ResultSitContrib::GrdAnagrafica_SortCommand::" & Err.Message)
		Log.Warn("Si è verificato un errore in ResultSitContrib::GrdAnagrafica_SortCommand::" & Err.Message)
		Response.Redirect("../PaginaErrore.aspx")
		End Try
			End Sub
*/
		#endregion Events

		#region Private methods
		private void ControlsDataBind ()
		{
			// Popolo ddl anno
            //int AnnoAttuale = DateTime.Now.Year;
            //ddlAnnoAvviso.Items.Add("");
            //for (int i = (AnnoAttuale + 1); i >= (AnnoAttuale - 20); i--)
            //    ddlAnnoAvviso.Items.Add (i.ToString ());
            string[] lista = MetodiTariffe.GetAnniTariffe(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
            try {
                if (lista != null && lista.Length > 0)
                {
                    ddlAnnoAvviso.DataSource = lista;
                    ddlAnnoAvviso.DataBind();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearc.ControlsDataBind.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private void RestoreSearch ()
		{
			string nsParam = Request["NewSearch"];
			bool newSearch = true;
            try {
                if (nsParam != null &&
                    nsParam.CompareTo("") != 0 &&
                    bool.Parse(nsParam) == false)
                {
                    newSearch = false;
                }

                // Sto tornando alla pagina da quella di visualizzazione dettaglio
                // per cui ripopolo la ricerca
                if (!newSearch)
                {
                    DTO.CartellaSearch objSearch = (DTO.CartellaSearch)Session["objRicercaAvvisiTOCO"];
                    if (objSearch != null)
                    {
                        txtCognome.Text = objSearch.CognomeContribuente;
                        txtNome.Text = objSearch.NomeContribuente;
                        txtCodFiscale.Text = objSearch.CodFiscaleContribuente;
                        txtPIva.Text = objSearch.PIVAContribuente;
                        txtNumeroAvviso.Text = objSearch.NumeroAvviso;
                        if (objSearch.Anno != -1)
                            ddlAnnoAvviso.SelectedValue = objSearch.Anno.ToString();
                        else
                            ddlAnnoAvviso.SelectedValue = string.Empty;
                        ChkSgravate.Checked = objSearch.IsSgravate;

                        //Cartella[] cartelle = (Cartella[])Session["objRicercaAvvisiTOCOCartelle"];
                        Cartella[] cartelle = MetodiCartella.CartelleSearch(objSearch, null);
                        DataBindGridCartelle(cartelle, 0, true);
                    }
                }
                else
                {
                    Session["objRicercaAvvisiTOCO"] = null;
                    Session["objRicercaAvvisiTOCOCartelle"] = null;
                }
            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearc.RestoreSearch.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private void DataBindGridCartelle (Cartella[] cartelle, int StartIndex, bool bRebind, int? page = 0)
		{
            try {
                if (cartelle != null && cartelle.Length > 0)
                {
                    GrdAvvisi.DataSource = cartelle;
                    if (bRebind)
                    {
                        GrdAvvisi.DataBind();
                    }
                    if (page.HasValue)
                        GrdAvvisi.PageIndex = page.Value;
                    GrdAvvisi.DataBind(); GrdAvvisi.Visible = true;
                    LblResultAvvisi.Visible = false;
                }
                else
                {
                    LblResultAvvisi.Visible = true;
                    GrdAvvisi.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearc.DataBindGridCartelle.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private void VisualizzaAvviso (DataGridCommandEventArgs e)
		{
            try {
                TableCell itemCell = SharedFunction.CellByName(e.Item, "Id Cartella");
                Response.Redirect(OSAPPages.SituazioneAvvisi + "?IdCartella=" + itemCell.Text);
            }
            catch (Exception Err)
            {

                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisiSearc.VisualizzaAvviso.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
		#endregion Private methods
	}
}
