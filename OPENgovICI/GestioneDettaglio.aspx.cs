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
using AnagInterface;
using System.Configuration;
using log4net;
using log4net.Config;
using Anater.Oggetti;

namespace DichiarazioniICI
{
    /// <summary>
    /// Classe di gestione della pagina GestioneDettaglio.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GestioneDettaglio :BasePage   
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(GestioneDettaglio));

		#region Property
		/// <summary>
		/// Ritorna o assegna l'id del contribuente
		/// </summary>
		protected int IDContribuente
		{
			get{return ViewState["IDContribuente"] != null ? (int)ViewState["IDContribuente"] : 0;}	
			set{ViewState["IDContribuente"] = value;}
		}
		//*** 20131003 - gestione atti compravendita ***
        /// <summary>
        /// 
        /// </summary>
		protected int CompraVenditaId
		{
			get{return ViewState["CompraVenditaId"] != null ? int.Parse(ViewState["CompraVenditaId"].ToString()) : 0;}	
			set{ViewState["CompraVenditaId"] = value;}
		}
        /// <summary>
        /// 
        /// </summary>
		protected DateTime CompraVenditaDataValidita
		{
			get{return ViewState["CompraVenditaDataValidita"] != null ? Convert.ToDateTime(ViewState["CompraVenditaDataValidita"].ToString()) : DateTime.Now;}	
			set{ViewState["CompraVenditaDataValidita"] = value;}
        }
        //*** ***
        #endregion
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <revisionHistory>
            /// <revision date="03/10/2013">
            /// gestione atti compravendita
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
                Session["ListUIPassProp"] = null;
                if (!IsPostBack)
                {
                    ViewState.Add("SortKey", "");
                    ViewState.Add("OrderBy", "ASC");

                    if (Request.QueryString["IdAttoCompraVendita"] != null)
                    {
                        this.CompraVenditaId = int.Parse(Request.QueryString["IdAttoCompraVendita"]);
                    }
                    this.IDContribuente = int.Parse(Request.QueryString["IDContribuente"]);
                    if (Request.QueryString["IdEnte"] != null)
                    {
                        Session["COD_ENTE"] = Request.QueryString["IdEnte"];
                    }
                    hdIdContribuente.Value = this.IDContribuente.ToString();
                    int Bonificato = int.Parse(Request.QueryString["Bonificato"]);
                    ControlsBind(this.IDContribuente, Bonificato);

                    string sScript = "";
                    sScript += "parent.Basso.location.href='../aspVuota.aspx';";
                    sScript += "parent.Nascosto.location.href='../aspVuota.aspx';";
                    if (this.CompraVenditaId > 0)
                    {
                        sScript += "$('.divricerca').hide();";
                        sScript += "parent.Comandi.location.href='./CGestioneDettaglio.aspx?COMPRAVENDITA=COMPRAVENDITA';";
                        log.Debug("GestioneDettaglio::devo caricare compravendita::" + this.CompraVenditaId.ToString() + "::IDContribuente::" + this.IDContribuente.ToString());
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
                        myData = new DichiarazioniView().GetCompraVenditaSoggetto(int.Parse(Request.QueryString["IdAttoCompraVendita"]), this.IDContribuente, -1);
                        foreach (DataRow myRow in myData.Rows)
                        {
                            lblSoggettoNota.Text = myRow["NotaTitolarita"].ToString();
                        }
                        AttoCompraVendita.Style.Add("display", "");
                        VisCompraVendita.Style.Add("display", "");
                    }
                    else
                    {
                        DataTable myCompraVendite = new DichiarazioniView().GetCompraVenditeFromSoggetto(this.IDContribuente);
                        GrdCompraVendite.DataSource = myCompraVendite.DefaultView;
                        GrdCompraVendite.DataBind();

                        sScript += "parent.Comandi.location.href='./CGestioneDettaglio.aspx';";
                        AttoCompraVendita.Style.Add("display", "none");
                        if (myCompraVendite.DefaultView.Count > 0)
                            VisCompraVendita.Style.Add("display", "");
                        else
                            VisCompraVendita.Style.Add("display", "none");
                    }
                    RegisterScript(sScript, this.GetType());
                    //*** 20170510 - carico i dati da catasto e degli altri proprietari ***
                    LoadDatiCatasto(ConstWrapper.CodBelfiore, this.IDContribuente);
                }
                else
                {
                    SetGrdCheck();
                    GrdDichiarazioni.DataSource = Session["Dichiarazioni"];
                }
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                string Str = "";
                if (ConstWrapper.HasPlainAnag)
                    Str += "document.getElementById('TRSpecAnag').style.display='none';";
                else
                    Str += "document.getElementById('TRPlainAnag').style.display='none';";
                Str += "";
                RegisterScript(Str, this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
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
			this.btnIndietro.Click += new System.EventHandler(this.btnIndietro_Click);
			this.btnSelCella.Click += new System.EventHandler(this.btnSelCella_Click);
			//this.GrdDichiarazioni.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdDichiarazioni_ItemCommand);
			//this.GrdDichiarazioni.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdDichiarazioni_SortCommand);
			//this.GrdDichiarazioni.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdDichiarazioni_ItemDataBound);
			////*** 20131018 - DOCFA ***
			//this.GrdImmobili.ItemDataBound+= new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdImmobili_ItemDataBound);
			////*** ***
			//this.GrdImmobili.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdImmobili_ItemCommand);
			//this.GrdImmobili.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdImmobili_SortCommand);
			this.cmdNewImmobile.Click += new System.EventHandler(this.cmdNewImmobile_Click);
			this.Load += new System.EventHandler(this.Page_Load);
            // *** 20140923 - GIS ***
            this.CmdGIS.Click += new System.EventHandler(this.CmdGIS_Click);
            //*** ***
        }
        #endregion

		/// <summary>
		/// Esegue il bind dei controlli della pagina.
		/// </summary>
		/// <param name="idContribuente"></param>
		/// <param name="bonificato"></param>
        private void ControlsBind(int idContribuente, int bonificato)
        {
            try {
                if (idContribuente > 0)
                {
                    //*** 201504 - Nuova Gestione anagrafica con form unico ***                
                    if (ConstWrapper.HasPlainAnag)
                        ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + idContribuente.ToString() + "&Azione=" + Utility.Costanti.AZIONE_LETTURA);
                    else
                    {
                        //*** ***
                        // visualizza i dati del contribuente selezionato
                        DettaglioAnagrafica DatiContribuente = HelperAnagrafica.GetDatiPersona(long.Parse(idContribuente.ToString()));
                        lblCognome.Text = DatiContribuente.Cognome;
                        lblNome.Text = DatiContribuente.Nome;
                        if (DatiContribuente.DataNascita != "00/00/1900")
                        {
                            lblDataNascita.Text = DatiContribuente.DataNascita;
                        }
                        else
                        {
                            lblDataNascita.Text = string.Empty;
                        }
                        lblIndirizzo.Text = DatiContribuente.ViaResidenza + ", " + DatiContribuente.CivicoResidenza;
                        lblComune.Text = DatiContribuente.ComuneResidenza + " (" + DatiContribuente.ProvinciaResidenza + ")";
                    }
                    GrdDichiarazioniBind(idContribuente, bonificato);
                    GrdImmobiliBind(idContribuente, ConstWrapper.CodiceEnte, ConstWrapper.StringConnection);
                } }
                        catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.ControlsBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
  
            }
        }

		/// <summary>
		/// Esegue il bind della datagrid delle dichiarazioni.
		/// </summary>
		/// <param name="idContribuente"></param>
		/// <param name="bonificato"></param>
		private void GrdDichiarazioniBind(int idContribuente, int bonificato)
		{
			try { DataView VistaDichiarazioni = new TestataTable(ConstWrapper.sUsername).ListCont(idContribuente, (Bonificato)bonificato);
			Session["Dichiarazioni"] = VistaDichiarazioni;
            if (VistaDichiarazioni.Count>0)
            {
			    VistaDichiarazioni.Sort = "NumeroDichiarazione";
			    GrdDichiarazioni.DataSource = VistaDichiarazioni;
			    GrdDichiarazioni.DataBind();
            }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdDichiarazioniBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
            }
        }

        /// <summary>
        /// Esegue il bind della datagrid degli immobili.
        /// </summary>
        /// <param name="idContribuente"></param>
        /// <param name="IdEmte"></param>
        /// <param name="myConn"></param>
        private void GrdImmobiliBind(int idContribuente, string IdEmte, string myConn)
        {
            try
            {
                DataView VistaImmobili = new DichiarazoniOggettiView().ListCont(idContribuente, IdEmte, myConn).DefaultView;
                Session["Immobili"] = VistaImmobili;
                //VistaImmobili.Sort = " foglio,numero,subalterno,datainizio,datafine ";
                GrdImmobili.DataSource = VistaImmobili;
                GrdImmobili.DataBind();
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdImmondiBind.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
                throw Err;
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
                    if (Business.ConstWrapper.VisualDOCFA)
                    {
                        if (ConstWrapper.PathPDF_DOCFA != null && ConstWrapper.PathPDF_DOCFA != string.Empty)
                        {
                            string sIndirizzoWeb = ConstWrapper.PathPDF_DOCFA.ToString();
                            if (!sIndirizzoWeb.EndsWith("/")) sIndirizzoWeb += "/";
                            sIndirizzoWeb += "DOCFA/" + ConstWrapper.CodiceEnte + "/";

                            if (((HiddenField)e.Row.FindControl("hfDOCFAPDF")).Value != string.Empty && ((HiddenField)e.Row.FindControl("hfDOCFAPDF")).Value != "&nbsp;")
                            {
                                ImageButton DOCFAPDF = (ImageButton)e.Row.FindControl("imgDOCFAPDF");
                                DOCFAPDF.Attributes.Add("onclick", "ApriDocumentoPDF('" + sIndirizzoWeb + ((HiddenField)e.Row.FindControl("hfDOCFAPDF")).Value + "')");
                            }
                            else
                            {
                                ImageButton DOCFAPDF = (ImageButton)e.Row.FindControl("imgDOCFAPDF");
                                DOCFAPDF.Attributes.Add("onclick", "GestAlert('a', 'warning', '', '', 'DOCFA non associato')");
                            }
                            if (((HiddenField)e.Row.FindControl("hfDOCFAPlanimetria")).Value != string.Empty && ((HiddenField)e.Row.FindControl("hfDOCFAPlanimetria")).Value != "&nbsp;")
                            {
                                ImageButton DOCFAPlan = (ImageButton)e.Row.FindControl("imgDOCFAPlan");
                                DOCFAPlan.Attributes.Add("onclick", "ApriDocumentoPDF('" + sIndirizzoWeb + ((HiddenField)e.Row.FindControl("hfDOCFAPlanimetria")).Value + ".tiff')");
                            }
                            else
                            {
                                ImageButton DOCFAPlan = (ImageButton)e.Row.FindControl("imgDOCFAPlan");
                                DOCFAPlan.Attributes.Add("onclick", "GestAlert('a', 'warning', '', '', 'Planimetria non associata')");
                            }
                        }
                        else
                        {
                            GrdImmobili.Columns[12].Visible = false;
                            GrdImmobili.Columns[13].Visible = false;
                        }
                    }
                    else
                    {
                        GrdImmobili.Columns[12].Visible = false;
                        GrdImmobili.Columns[13].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdRowDataBound.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
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
                    if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdDichiarazioni")
                    {
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + IDRow + "&TYPEOPERATION=DETTAGLIO"), this.GetType());
                     }
                    else
                    {
                        foreach (GridViewRow myRow in GrdImmobili.Rows)
                        {
                            if (IDRow == ((HiddenField)myRow.FindControl("hfidoggetto")).Value)
                            {
                                RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + ((HiddenField)myRow.FindControl("hfidtestata")).Value + "&IDImmobile=" + IDRow + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "&IdDOCFA=" + ((HiddenField)myRow.FindControl("hfIdDOCFA")).Value + "&TYPEOPERATION=DETTAGLIO"+"&titolo="+Request.QueryString["titolo"]), this.GetType());
                            }
                        }
                    }
                }
                else if (e.CommandName == "RowSelect")
                {
                    if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdDichiarazioni")
                    {
                        foreach (GridViewRow myRow in GrdImmobili.Rows)
                        {
                            if (IDRow == ((HiddenField)myRow.FindControl("hfidtestata")).Value)
                            {
                                myRow.BackColor = Color.FromArgb(189, 203, 214);
                                myRow.ForeColor = Color.Black;
                            }
                            else
                            {
                                myRow.BackColor = Color.FromArgb(247, 247, 247);
                                myRow.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                else if (e.CommandName == "RowCopy")
                {
                    if (((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID == "GrdDichiarazioni")
                    {
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + IDRow + "&PassProp=1&TYPEOPERATION=DETTAGLIO"), this.GetType());
                    }
                    else
                    {
                        foreach (GridViewRow myRow in GrdImmobili.Rows)
                        {
                            if (IDRow == ((HiddenField)myRow.FindControl("hfidoggetto")).Value)
                            {
                                RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + ((HiddenField)myRow.FindControl("hfidtestata")).Value + "&IDImmobile=" + IDRow + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "&IdDOCFA=" + ((HiddenField)myRow.FindControl("hfIdDOCFA")).Value + "&TYPEOPERATION=DETTAGLIO"), this.GetType());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdRowCommand.errore: ", ex);
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
            LoadSearch(((Ribes.OPENgov.WebControls.RibesGridView)sender).UniqueID, e.NewPageIndex);
        }
        //      private void GrdDichiarazioni_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
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

        //	DataView DichiarazioniSort;
        //	DichiarazioniSort = (DataView) Session["Dichiarazioni"];
        //	//((DataTable)(Session["Immobili"])).DefaultView.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];
        //	DichiarazioniSort.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];

        //	Session["Dichiarazioni"] = DichiarazioniSort;

        //	//DataTable TabSource = (DataTable)Session["Immobili"];	
        //	GrdDichiarazioni.DataSource = DichiarazioniSort;
        //	GrdDichiarazioni.DataBind();
        // }
        //     catch (Exception ex)
        //   {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdDichiarazioni_SortCommand.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        //  }
        //}

        //private void GrdDichiarazioni_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //{
        //          //*** 20141110 - passaggio di proprietà ***
        //try{
        //	if(e.CommandName == "Select")
        //          {
        //              string IdTestata = GrdDichiarazioni.DataKeys[e.Item.ItemIndex].ToString();

        //              string idTest;
        //              foreach (GridViewRow Elem in GrdImmobili.Items)
        //              {
        //                  idTest = Elem.Cells[0].Text;//((Label)(Elem.FindControl("lblIDTestata"))).Text;
        //                  if (idTest.CompareTo(IdTestata) == 0)
        //                  {
        //                      Elem.BackColor = Color.FromArgb(189, 203, 214);
        //                      Elem.ForeColor = Color.Black;
        //                  }
        //                  else
        //                  {
        //                      Elem.BackColor = Color.FromArgb(247, 247, 247);
        //                      Elem.ForeColor = Color.Black;
        //                  }
        //              }
        //          }
        //          else if (e.CommandName=="Edit")
        //	{// va alla pagina della dichiarazione selezionata
        //		ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + GrdDichiarazioni.DataKeys[e.Item.ItemIndex] + "&TYPEOPERATION=DETTAGLIO");
        //	}
        //          else if (e.CommandName=="Update")
        //          {
        //              ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + GrdDichiarazioni.DataKeys[e.Item.ItemIndex] + "&PassProp=1&TYPEOPERATION=DETTAGLIO");
        //          }
        // }
        //     catch (Exception ex)
        //   {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdDichiarazioni_ItemCommand.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        //  }
        //      }



        //      //*** 20141110 - passaggio di proprietà ***
        //      private void GrdDichiarazioni_ItemDataBound(object source, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //      {
        //try{
        //          //if ((e.Item.ItemType.ToString().CompareTo("Header") != 0) && (e.Item.ItemType.ToString().CompareTo("Footer") != 0))
        //          //{
        //          //    //e.Item.Attributes.Add("onmouseup", "SelCelle('"+ GrdDichiarazioni.DataKeys[e.Item.ItemIndex] +"')");
        //          //    e.Item.Attributes.Add("onmouseup", "handleWisely(event.type, '" + GrdDichiarazioni.DataKeys[e.Item.ItemIndex] + "','" + e.Item.UniqueID + "')");
        //          //    e.Item.Attributes.Add("onclick", "handleWisely(event.type, '" + GrdDichiarazioni.DataKeys[e.Item.ItemIndex] + "','" + e.Item.UniqueID + "')");
        //          //    e.Item.Attributes.Add("onmousedown", "handleWisely(event.type, '" + GrdDichiarazioni.DataKeys[e.Item.ItemIndex] + "','" + e.Item.UniqueID + "')");
        //          //    e.Item.Attributes.Add("ondblclick", "handleWisely(event.type, '" + GrdDichiarazioni.DataKeys[e.Item.ItemIndex] + "','" + e.Item.UniqueID + "')");
        //          //}
        // }
        //     catch (Exception ex)
        //   {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdDichiarazioni_ItemDataBound.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        //  }
        //      }




        //private void GrdImmobili_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
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

        //	DataView TabellaSort;
        //	TabellaSort = (DataView) Session["Immobili"];
        //	//((DataTable)(Session["Immobili"])).DefaultView.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];
        //	TabellaSort.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"];

        //	Session["Immobili"] = TabellaSort;

        //	//DataTable TabSource = (DataTable)Session["Immobili"];	
        //	GrdImmobili.DataSource = TabellaSort;
        //	GrdImmobili.DataBind();
        // }
        //     catch (Exception ex)
        //   {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdImmobili_SortCommand.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        //  }
        //}

        //      private void GrdImmobili_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        //      {
        //          // va alla pagina di dettaglio dell'immobile selezionato
        //          //*** 20140923 - GIS ***
        //try{
        //          if (e.CommandName == "Update") //if (e.CommandName == "Select")
        //          {
        //              //*** 20131003 - gestione atti compravendita *** //*** 20131018 - DOCFA ***
        //              //ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + ((Label)e.Item.FindControl("lblIDTestata")).Text + "&IDImmobile=" + GrdImmobili.DataKeys[e.Item.ItemIndex] + "&TYPEOPERATION=DETTAGLIO");
        //              ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + e.Item.Cells[0].Text + "&IDImmobile=" + GrdImmobili.DataKeys[e.Item.ItemIndex] + "&IdAttoCompraVendita=" + this.CompraVenditaId.ToString() + "&IdDOCFA=" + e.Item.Cells[19].Text + "&TYPEOPERATION=DETTAGLIO");
        //              //*** ***
        //          }
        //          //*** ***
        // }
        //     catch (Exception ex)
        //   {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdImmobili_ItemCommand.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        //  }
        //      }



        ////*** 20131018 - DOCFA ***
        //protected void GrdImmobili_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //try{
        //	if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //	{
        //              if (Business.ConstWrapper.VisualDOCFA)
        //              {
        //                  if (ConstWrapper.PathPDF_DOCFA != null && ConstWrapper.PathPDF_DOCFA != string.Empty)
        //                  {
        //                      string sIndirizzoWeb = ConstWrapper.PathPDF_DOCFA.ToString();
        //                      if (!sIndirizzoWeb.EndsWith("/")) sIndirizzoWeb += "/";
        //                      sIndirizzoWeb += "DOCFA/" + ConstWrapper.CodiceEnte + "/";

        //                      if (e.Item.Cells[17].Text != string.Empty && e.Item.Cells[17].Text != "&nbsp;")
        //                      {
        //                          ImageButton DOCFAPDF = (ImageButton)e.Item.FindControl("imgDOCFAPDF");
        //                          DOCFAPDF.Attributes.Add("onclick", "ApriDocumentoPDF('" + sIndirizzoWeb + e.Item.Cells[17].Text + "')");
        //                      }
        //                      else
        //                      {
        //                          ImageButton DOCFAPDF = (ImageButton)e.Item.FindControl("imgDOCFAPDF");
        //                          DOCFAPDF.Attributes.Add("onclick", "alert('DOCFA non associato')");
        //                      }
        //                      if (e.Item.Cells[18].Text != string.Empty && e.Item.Cells[18].Text != "&nbsp;")
        //                      {
        //                          ImageButton DOCFAPlan = (ImageButton)e.Item.FindControl("imgDOCFAPlan");
        //                          DOCFAPlan.Attributes.Add("onclick", "ApriDocumentoPDF('" + sIndirizzoWeb + e.Item.Cells[18].Text + ".tiff')");
        //                      }
        //                      else
        //                      {
        //                          ImageButton DOCFAPlan = (ImageButton)e.Item.FindControl("imgDOCFAPlan");
        //                          DOCFAPlan.Attributes.Add("onclick", "alert('Planimetria non associata')");
        //                      }
        //                  }
        //                  else
        //                  {
        //                      GrdImmobili.Columns[15].Visible = false;
        //                      GrdImmobili.Columns[16].Visible = false;
        //                  }
        //              }
        //              else
        //              {
        //                  GrdImmobili.Columns[15].Visible = false;
        //                  GrdImmobili.Columns[16].Visible = false;
        //              }
        //          }
        // }
        //     catch (Exception ex)
        //   {
        //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GrdImmobili_ItemDataBound.errore: ", ex);
        //    Response.Redirect("../PaginaErrore.aspx");
        //  }
        //}
        ////*** ***
        #endregion
        private void LoadSearch(string myTableName, int? page = 0)
        {
            try
            {
                GrdImmobili.DataSource = (DataView)Session["Immobili"];
                if (page.HasValue)
                    GrdImmobili.PageIndex = page.Value;
                GrdImmobili.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.LoadSearch.errore: ", ex);
                 Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnIndietro_Click(object sender, System.EventArgs e)
		{
            try { 															
			//*** 20131003 - gestione atti compravendita ***
			if (this.CompraVenditaId>0)
			{
				string sScript= "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';";
				sScript+= "document.location.href='../20/AttiCompraVendita/CompraVenditaRicerca.aspx?ENTE="+ConstWrapper.CodiceEnte+"&IdAtto="+ this.CompraVenditaId.ToString() +"';";
				RegisterScript(sScript,this.GetType());
			}
			else
			{
				if (Request.Params["Provenienza"]=="CHECKRIFCAT")
				{
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_CONFRONTO_CATASTO", ""), this.GetType());//?TypeCheck=" + Request.Params["TypeCheck"]);
				}
				else
				{
                        RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE", String.Empty), this.GetType());
				}
			}
                //*** ***
                //			}
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.btnIndietro_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProvenienza"></param>
        /// <returns></returns>
        public string GetProvenienza(object idProvenienza)
		{
			string DescProv=String.Empty ; 
			ProvenienzeRow myvalue=new ProvenienzeRow();
			myvalue.Codice =idProvenienza.ToString ();
			myvalue.Tributo = ConstWrapper.CodiceTributo;//ApplicationHelper.Tributo;
			myvalue.Descrizione ="";
			myvalue.Tipologia ="D" ;

            DataTable Ricerca = new ProvenienzeTable(ConstWrapper.sUsername).Ricerca(myvalue, ConstWrapper.StringConnection);
            try {  
			if (Ricerca.Rows.Count>0){
				DescProv=(System.String)Ricerca.Rows[0].ItemArray[4];
			}

                //DataView Vista = new ProvenienzeTable(ConstWrapper.sUsername).List(TipologieProvenienza.dichiarazioni, ApplicationHelper.Tributo);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GetProvenienza.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
            return DescProv.ToString() ;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Contitolare"></param>
        /// <returns></returns>
		public string GetContribuente(object Contitolare)
		{
			string DescContr=String.Empty ;
            try { 
            if ( Contitolare==DBNull.Value )
			{
				DescContr="";
			}
			else
			{
				if (bool.Parse (Contitolare.ToString() ))
				{
					DescContr="Contitolare";
				}
				else
				{
					DescContr="Proprietario";
				}
			}
			
		
			return DescContr.ToString() ;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.GetContribuente.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelCella_Click(object sender, System.EventArgs e)
		{
			string IdTestata = txtIdTestata.Text.ToString();

			string idTest;
            try { 
			foreach (GridViewRow Elem in GrdImmobili.Rows){
                idTest = Elem.Cells[0].Text;//((Label)(Elem.FindControl("lblIDTestata"))).Text;
				if (idTest.CompareTo(IdTestata) == 0)
				{
					Elem.BackColor = Color.FromArgb(189, 203, 214);
					Elem.ForeColor = Color.Black;
				}
				else
				{
					Elem.BackColor = Color.FromArgb(247, 247, 247);
					Elem.ForeColor = Color.Black;
				}
			}
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.btnSelCella_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }

		//*** 20131003 - gestione atti compravendita ***
		private void cmdNewImmobile_Click(object sender, System.EventArgs e)
		{
			bool retval = true;

			int CodiceContribuenteContr = this.IDContribuente;

			int CodiceContribuenteDen = this.IDContribuente;

			TestataTable Testata = new TestataTable(ConstWrapper.sUsername);
            Utility.DichManagerICI.TestataRow RigaTestata = new Utility.DichManagerICI.TestataRow();

			RigaTestata.ID=-1;
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

			// inserisce una nuova testata
			int idTestata;
            //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
			//retval = Testata.Insert(RigaTestata, out idTestata);
            retval = new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetTestata(Utility.Costanti.AZIONE_NEW, RigaTestata, out idTestata);
            //*** ***
			
			string strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Salvataggio dichiarazione" + (retval == true ? "" : " non") + " effettuato.');";
            RegisterScript(strscript,this.GetType());

            RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + idTestata.ToString() + "&TYPEOPERATION=GESTIONE&IdAttoCompraVendita=" + this.CompraVenditaId.ToString()), this.GetType());

			Testata.kill ();
		}
        // *** 20140923 - GIS ***
        private void CmdGIS_Click(object sender, System.EventArgs e)
        {
            string CodeGIS, sScript, sRifPrec="";
            RemotingInterfaceAnater.GIS fncGIS = new RemotingInterfaceAnater.GIS();
            System.Collections.Generic.List<RicercaUnitaImmobiliareAnater> listRifCat = new System.Collections.Generic.List<RicercaUnitaImmobiliareAnater>();
            RicercaUnitaImmobiliareAnater myRifCat = new RicercaUnitaImmobiliareAnater();
            try
            {
                foreach (DataRowView myRow in (DataView)Session["Immobili"])
                {
                    if (Business.CoreUtility.FormattaGrdCheck(myRow["bSel"]) == true && myRow["foglio"].ToString() != "")
                    {
                        if (sRifPrec != myRow["foglio"].ToString() + "|" + myRow["numero"].ToString() + "|" + myRow["subalterno"].ToString())
                        {
                            myRifCat = new RicercaUnitaImmobiliareAnater();
                            myRifCat.Foglio = myRow["foglio"].ToString();
                            myRifCat.Mappale = myRow["numero"].ToString();
                            myRifCat.Subalterno = myRow["subalterno"].ToString();
                            myRifCat.CodiceRicerca = ConstWrapper.CodBelfiore;
                            listRifCat.Add(myRifCat);
                        }
                    }
                    sRifPrec = myRow["foglio"].ToString() + "|" + myRow["numero"].ToString() + "|" + myRow["subalterno"].ToString();
                }
                if ((listRifCat.ToArray().Length > 0))
                {
                    CodeGIS = fncGIS.getGIS(ConstWrapper.UrlWSGIS, listRifCat.ToArray());
                    if (!(CodeGIS == null))
                    {
                        sScript = "window.open(\'" + ConstWrapper.UrlWebGIS + CodeGIS + "\','wdwGIS')";
                        RegisterScript(sScript,this.GetType());
                    }
                    else
                    {
                        sScript = "GestAlert('a', 'warning', '', '', 'Errore in interrogazione Cartografia!');";
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
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.CmdGIS_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        private void SetGrdCheck()
        {
            Ribes.OPENgov.WebControls.RibesGridView myGrd = null;
          
            try
            {
                DataView myDvResult = null;
                myDvResult = (DataView)Session["Immobili"];
                myGrd = GrdImmobili;
                if (myDvResult != null)
                {
                    foreach (GridViewRow itemGrid in myGrd.Rows)
                    {
                        foreach (DataRowView myItem in myDvResult)
                        {
                            if (myItem["IDImmobile"].ToString() == ((HiddenField)itemGrid.FindControl("hfidoggetto")).Value.ToString())
                            {
                                myItem["bSel"] = ((CheckBox)itemGrid.FindControl("chkSel")).Checked;
                                break;
                            }
                        }
                    }
                    Session["Immobili"] = myDvResult;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.SetGrdCheck.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
                throw ex;
            }
        }
        //*** ***
        private void LoadDatiCatasto(string sIdEnte, int IdContribuente)
        {
            string sMyErr = string.Empty;
            string CFPIVA = string.Empty;
            DataTable dtMyDati = new DataTable();

            try
            {
                if (IdContribuente > 0)
                {
                    DettaglioAnagrafica DatiContribuente = HelperAnagrafica.GetDatiPersona(long.Parse(IdContribuente.ToString()));
                    if (DatiContribuente.PartitaIva != string.Empty)
                        CFPIVA = DatiContribuente.PartitaIva;
                    else
                        CFPIVA = DatiContribuente.CodiceFiscale;

                    if (new Catasto().SearchAnomalie(sIdEnte, Catasto.Anomalia.PerSoggetto, false, CFPIVA, string.Empty, string.Empty, out dtMyDati, out sMyErr))
                    {
                        if (!(dtMyDati == null))
                        {
                            while (GrdCatasto.Columns.Count > 0)
                            {
                                DropColGrd();
                            }
                            foreach (DataColumn myCol in dtMyDati.Columns)
                            {
                                BoundField myGrdCol = new BoundField();
                                myGrdCol.HeaderText = myCol.ColumnName;
                                myGrdCol.DataField = myCol.ColumnName;
                                GrdCatasto.Columns.Add(myGrdCol);
                            }
                            GrdCatasto.DataSource = dtMyDati;
                            GrdCatasto.DataBind();
                            if (GrdCatasto.Rows.Count > 0)
                            {
                                GrdCatasto.Visible = true;
                            }
                            else {
                                GrdCatasto.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.LoadDatiCatasto.errore: ", ex);
            }
        }
        private void DropColGrd()
        {
            try
            {
                checked
                {
                    for (int x = 0; x < GrdCatasto.Columns.Count; x++)
                    {
                        GrdCatasto.Columns.RemoveAt(x);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GestioneDettaglio.DropColGrd.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
    }
}
