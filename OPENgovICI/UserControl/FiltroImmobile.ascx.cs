using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DichiarazioniICI.Database;
using Business;
using log4net;

namespace DichiarazioniICI.UserControl
{
	/// <summary>
	///	UserControl per la ricerca dichiarazioni per Immobile.
	/// </summary>
	public class FiltroImmobile : System.Web.UI.UserControl
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(FiltroImmobile));
        protected System.Web.UI.WebControls.DropDownList ddlCategoriaCatastale;
		protected System.Web.UI.WebControls.TextBox txtVia;
		protected System.Web.UI.WebControls.TextBox txtAnno;
		protected System.Web.UI.WebControls.TextBox txtProtocollo;
		protected System.Web.UI.WebControls.TextBox txtSubalterno;
		protected System.Web.UI.WebControls.TextBox txtNumero;
		protected System.Web.UI.WebControls.TextBox txtFoglio;
		protected System.Web.UI.WebControls.Button btnTrova;
		protected System.Web.UI.WebControls.TextBox txtPartitaCatastale;
		protected System.Web.UI.WebControls.DropDownList ddlClasse;
		protected System.Web.UI.WebControls.DropDownList ddlTipoValore;
		protected System.Web.UI.WebControls.TextBox txtValore;
		protected System.Web.UI.WebControls.TextBox txtPercentualePos;
		protected System.Web.UI.WebControls.CheckBox ckbImmNoAgganciati;
		protected System.Web.UI.WebControls.TextBox txtSezione;
        protected System.Web.UI.WebControls.DropDownList ddlCaratteristica;
        protected System.Web.UI.WebControls.DropDownList ddlTipoUtilizzo;
        protected System.Web.UI.WebControls.DropDownList ddlTipoPossesso;
        protected System.Web.UI.WebControls.CheckBox chkAbiPrinc;
        protected System.Web.UI.WebControls.CheckBox chkPertinenza;
        protected System.Web.UI.WebControls.CheckBox chkColtivatori;
        protected string IdEnte { get; set; }

		#region Metodi per il binding

		/// <summary>
		/// Ritorna una DataView valorizzata con l'elenco delle classi.
		/// </summary>
		/// <returns></returns>
        protected DataView GetListCaratteristica()
        {
            //DataView Vista = new DatabaseOpengov.CaratteristicaTable(ConstWrapper.sUsername).List().DefaultView;
            DataView Vista = new Database.CaratteristicaTable(ConstWrapper.sUsername).List(Business.ConstWrapper.StringConnectionOPENgov).DefaultView;
            Vista.Sort = "Descrizione_Breve";
            return Vista;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataView GetListClassi()
		{
            DataView Vista = new ClasseTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
			Vista.Sort = "Classe";
			return Vista;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		protected DataView GetListCategorieCatastali()
		{
            DataView Vista = new CategoriaCatastaleTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
			Vista.Sort = "CategoriaCatastale";
			return Vista;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataView GetListTipiUtilizzi()
        {
            DataView Vista = new UtilizzoTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
            Vista.Sort = "Descrizione";
            return Vista;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DataView GetListTipiPossesso()
        {
            DataView Vista = new PossessoTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
            Vista.Sort = "Descrizione";
            return Vista;
        }

		#endregion
        /// <summary>
        /// 
        /// </summary>
		public Gestione PaginaPrecedente
		{
			get
			{
				return ((Gestione)Page);
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                //*** 201511 - Funzioni Sovracomunali ***
                IdEnte = PaginaPrecedente.ddlEnti.SelectedValue.ToString();
                //*** ***
                if (!IsPostBack)
			    {
                    LoadCombos();
				    if(ConstWrapper.Parametri != null)
				    {
					    if (ConstWrapper.Parametri["TipoRicerca"] != null)
					    {
						    if (ConstWrapper.Parametri["TipoRicerca"].ToString()=="Immobile")
						    {
							    PaginaPrecedente.GrdContribuenti.Visible= false;
                                txtVia.Text = ConstWrapper.Parametri["Via"] == null ? String.Empty : ConstWrapper.Parametri["Via"].ToString();
                                txtFoglio.Text = ConstWrapper.Parametri["Foglio"] == null ? String.Empty : ConstWrapper.Parametri["Foglio"].ToString();
                                txtNumero.Text = ConstWrapper.Parametri["Numero"] == null ? String.Empty : ConstWrapper.Parametri["Numero"].ToString();
                                txtSubalterno.Text = ConstWrapper.Parametri["Subalterno"] == null ? String.Empty : ConstWrapper.Parametri["Subalterno"].ToString();
                                if (ConstWrapper.Parametri["Caratteristica"] != null)
                                {
                                    try
                                    {
                                        ddlCaratteristica.ClearSelection();
                                        ddlCaratteristica.Items.FindByValue(ConstWrapper.Parametri["Caratteristica"].ToString()).Selected = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.Page_Load.errore: ", ex);
                                        log.Debug("FiltroImmobile::PageLoad::loadCaratteristica::" + ConstWrapper.Parametri["Caratteristica"].ToString() + "::errore::", ex);
                                    }
                                }
                                if (ConstWrapper.Parametri["CategoriaCatastale"] != null)
                                {
                                    try{
                                    ddlCategoriaCatastale.ClearSelection();
                                    ddlCategoriaCatastale.Items.FindByValue(ConstWrapper.Parametri["CategoriaCatastale"].ToString()).Selected = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.Page_Load.errore: ", ex);
                                        log.Debug("FiltroImmobile::PageLoad::loadCategoria::" + ConstWrapper.Parametri["CategoriaCatastale"].ToString() + "::errore::", ex);
                                    }
                                }
                                if (ConstWrapper.Parametri["Classe"] != null)
                                {
                                    try{
                                    ddlClasse.ClearSelection();
                                    ddlClasse.Items.FindByValue(ConstWrapper.Parametri["Classe"].ToString()).Selected = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.Page_Load.errore: ", ex);
                                        log.Debug("FiltroImmobile::PageLoad::loadClasse::" + ConstWrapper.Parametri["Classe"].ToString() + "::errore::", ex);
                                    }
                                }
                               if (ConstWrapper.Parametri["TipoUtilizzo"] != null)
                                {
                                    ddlTipoUtilizzo.ClearSelection();
                                    ddlTipoUtilizzo.Items.FindByValue(ConstWrapper.Parametri["TipoUtilizzo"].ToString()).Selected = true;
                                }
                                if (ConstWrapper.Parametri["TipoPossesso"] != null)
                                {
                                    ddlTipoPossesso.ClearSelection();
                                    ddlTipoPossesso.Items.FindByValue(ConstWrapper.Parametri["TipoPossesso"].ToString()).Selected = true;
                                }
                                txtPercentualePos.Text = ConstWrapper.Parametri["PercPos"] == null ? String.Empty : ConstWrapper.Parametri["PercPos"].ToString();
                                ddlTipoValore.SelectedValue = ConstWrapper.Parametri["ConfrontoImporto"] == null ? String.Empty : ConstWrapper.Parametri["ConfrontoImporto"].ToString();
                                txtValore.Text = ConstWrapper.Parametri["Importo"] == null ? String.Empty : ConstWrapper.Parametri["Importo"].ToString();
                                chkColtivatori.Checked = ConstWrapper.Parametri["Coltivatori"] == null ? false : Business.CoreUtility.FormattaGrdCheck(ConstWrapper.Parametri["Coltivatori"]);
                                ckbImmNoAgganciati.Checked = ConstWrapper.Parametri["ImmobiliNoAgg"] == null ? false : Business.CoreUtility.FormattaGrdCheck(ConstWrapper.Parametri["ImmobiliNoAgg"]);
                                txtPartitaCatastale.Text = ConstWrapper.Parametri["PartitaCatastale"] == null ? String.Empty : ConstWrapper.Parametri["PartitaCatastale"].ToString();
                                txtSezione.Text = ConstWrapper.Parametri["Sezione"] == null ? String.Empty : ConstWrapper.Parametri["Sezione"].ToString();
                                txtProtocollo.Text = ConstWrapper.Parametri["Protocollo"] == null ? String.Empty : ConstWrapper.Parametri["Protocollo"].ToString();
                                txtAnno.Text = ConstWrapper.Parametri["Anno"] == null ? String.Empty : ConstWrapper.Parametri["Anno"].ToString();
                                chkAbiPrinc.Checked = ConstWrapper.Parametri["AbiPrinc"] == null ? false : Business.CoreUtility.FormattaGrdCheck(ConstWrapper.Parametri["AbiPrinc"]);
                                chkPertinenza.Checked = ConstWrapper.Parametri["IsPertinenza"] == null ? false : Business.CoreUtility.FormattaGrdCheck(ConstWrapper.Parametri["IsPertinenza"]);
                                this.btnTrova_Click(null,null);
						    }
					    }
				    }
			    }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.Page_Load.errore: ",ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
		}

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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnTrova.Click += new System.EventHandler(this.btnTrova_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="11/2015">
        /// <strong>Funzioni Sovracomunali</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        private void btnTrova_Click(object sender, System.EventArgs e)
		{
            string Dal, Al;
            try
            {
                Dal = Al = DateTime.MaxValue.ToString();
                if (PaginaPrecedente.TxtDal.Text != "")
                    Dal = PaginaPrecedente.TxtDal.Text;
                if (PaginaPrecedente.TxtAl.Text != "")
                    Al = PaginaPrecedente.TxtAl.Text;
                ConstWrapper.Parametri = new System.Collections.Hashtable();
                ConstWrapper.Parametri["Via"] = txtVia.Text;
                ConstWrapper.Parametri["Foglio"] = txtFoglio.Text;
                ConstWrapper.Parametri["Numero"] = txtNumero.Text;
                ConstWrapper.Parametri["Subalterno"] = txtSubalterno.Text;
                ConstWrapper.Parametri["Caratteristica"] = ddlCaratteristica.SelectedValue;
                ConstWrapper.Parametri["CategoriaCatastale"] = ddlCategoriaCatastale.SelectedValue == "" ? "-1" : ddlCategoriaCatastale.SelectedValue;
                ConstWrapper.Parametri["Classe"] = ddlClasse.SelectedValue == "" ? "-1" : ddlClasse.SelectedValue;
                ConstWrapper.Parametri["AbiPrinc"] = chkAbiPrinc.Checked == true ? "1" : "0";
                ConstWrapper.Parametri["IsPertinenza"] = chkPertinenza.Checked == true ? "1" : "0";
                int n = 0;
                int.TryParse(ddlTipoUtilizzo.SelectedValue, out n);
                ConstWrapper.Parametri["TipoUtilizzo"] = n;
                int.TryParse(ddlTipoPossesso.SelectedValue, out n);
                ConstWrapper.Parametri["TipoPossesso"] = n;
                ConstWrapper.Parametri["PercPos"] = txtPercentualePos.Text == string.Empty ? 0 : double.Parse(txtPercentualePos.Text);
                ConstWrapper.Parametri["ConfrontoImporto"] = ddlTipoValore.SelectedValue;
                ConstWrapper.Parametri["Importo"] = txtValore.Text == string.Empty ? 0 : double.Parse(txtValore.Text);
                ConstWrapper.Parametri["Coltivatori"] = chkColtivatori.Checked == true ? "1" : "0";
                ConstWrapper.Parametri["ImmobiliNoAgg"] = ckbImmNoAgganciati.Checked == true ? "1" : "0";
                ConstWrapper.Parametri["PartitaCatastale"] = txtPartitaCatastale.Text;
                ConstWrapper.Parametri["Sezione"] = txtSezione.Text;
                ConstWrapper.Parametri["Protocollo"] = txtProtocollo.Text;
                ConstWrapper.Parametri["Anno"] = txtAnno.Text;
                ConstWrapper.Parametri["TipoRicerca"] = "Immobile";
                PaginaPrecedente.GrdContribuenti.Visible = false;

                DataTable Tabella = new ContribuentiImmobileView().ListContribuenti(ConstWrapper.StringConnection, ConstWrapper.Ambiente, "", int.Parse(PaginaPrecedente.ddlProv.SelectedValue), Dal, Al, txtPartitaCatastale.Text,
                txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, txtSezione.Text, int.Parse(ConstWrapper.Parametri["Caratteristica"].ToString()), txtAnno.Text,
                txtProtocollo.Text, txtVia.Text, ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue,
                IdEnte, (Bonificato)int.Parse(PaginaPrecedente.ddlBonificato.SelectedValue),
                double.Parse(ConstWrapper.Parametri["PercPos"].ToString()), double.Parse(ConstWrapper.Parametri["Importo"].ToString()), int.Parse(ddlTipoValore.SelectedValue), int.Parse(ConstWrapper.Parametri["ImmobiliNoAgg"].ToString()),
                int.Parse(ConstWrapper.Parametri["AbiPrinc"].ToString()), int.Parse(ConstWrapper.Parametri["IsPertinenza"].ToString()), int.Parse(ConstWrapper.Parametri["TipoUtilizzo"].ToString()), int.Parse(ConstWrapper.Parametri["TipoPossesso"].ToString()), int.Parse(ConstWrapper.Parametri["Coltivatori"].ToString()));
                Session["TABELLA_RICERCA_IM"] = Tabella;
                if (Tabella.Rows.Count > 0)
                {
                    //*** 201511 - Funzioni Sovracomunali ***
                    if (Business.ConstWrapper.CodiceEnte == "")
                        PaginaPrecedente.GrdImmobili.Columns[0].Visible = true;
                    else
                        PaginaPrecedente.GrdImmobili.Columns[0].Visible = false;
                    //*** ***
                    PaginaPrecedente.GrdContribuenti.Visible = false;
                    Tabella.DefaultView.Sort = "Cognome";
                    PaginaPrecedente.GrdImmobili.DataSource = Tabella;
                    PaginaPrecedente.GrdImmobili.DataBind();
                    PaginaPrecedente.GrdImmobili.Visible = true;
                    PaginaPrecedente.lblRisultati.Text = "Risultati della Ricerca";
                    PaginaPrecedente.lblNRecord.Text = "N. Posizioni " + Tabella.Rows.Count.ToString();
                    //per la stampa
                    Tabella = new ContribuentiTuttiView().ListContribuentiStampa(IdEnte, "", int.Parse(PaginaPrecedente.ddlProv.SelectedValue), Dal, Al
                        , "", "", "", ""
                        , txtPartitaCatastale.Text, txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, txtSezione.Text
                        , int.Parse(ConstWrapper.Parametri["Caratteristica"].ToString()), txtAnno.Text, txtProtocollo.Text, txtVia.Text
                        , ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue
                        , double.Parse(ConstWrapper.Parametri["PercPos"].ToString()), double.Parse(ConstWrapper.Parametri["Importo"].ToString()), int.Parse(ddlTipoValore.SelectedValue)
                        , int.Parse(ConstWrapper.Parametri["ImmobiliNoAgg"].ToString()), int.Parse(ConstWrapper.Parametri["AbiPrinc"].ToString()), int.Parse(ConstWrapper.Parametri["IsPertinenza"].ToString())
                        , int.Parse(ConstWrapper.Parametri["TipoUtilizzo"].ToString()), int.Parse(ConstWrapper.Parametri["TipoPossesso"].ToString()), int.Parse(ConstWrapper.Parametri["Coltivatori"].ToString()));
                    // Variabile per la stampa in Excel
                    Session["TABELLA_STAMPA"] = Tabella;
                }
                else
                {
                    PaginaPrecedente.lblRisultati.Text = "La ricerca non ha prodotto risultati";
                    PaginaPrecedente.lblNRecord.Text = "";
                    PaginaPrecedente.GrdImmobili.Visible = false;
                    PaginaPrecedente.GrdContribuenti.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.btnTrova_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
		}
        //private void btnTrova_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        //double Importo=0;
        //        //float Possesso;
        //        //string valoreImmobile, percPos;

        //        ConstWrapper.Parametri = new System.Collections.Hashtable();
        //        ConstWrapper.Parametri["Via"] = txtVia.Text;
        //        ConstWrapper.Parametri["Foglio"] = txtFoglio.Text;
        //        ConstWrapper.Parametri["Numero"] = txtNumero.Text;
        //        ConstWrapper.Parametri["Subalterno"] = txtSubalterno.Text;
        //        ConstWrapper.Parametri["Caratteristica"] = ddlCaratteristica.SelectedValue;
        //        ConstWrapper.Parametri["CategoriaCatastale"] = ddlCategoriaCatastale.SelectedValue == "" ? "-1" : ddlCategoriaCatastale.SelectedValue;
        //        ConstWrapper.Parametri["Classe"] = ddlClasse.SelectedValue == "" ? "-1" : ddlClasse.SelectedValue;
        //        ConstWrapper.Parametri["AbiPrinc"] = chkAbiPrinc.Checked == true ? "1" : "0";
        //        ConstWrapper.Parametri["IsPertinenza"] = chkPertinenza.Checked == true ? "1" : "0";
        //        int n = 0;
        //        int.TryParse(ddlTipoUtilizzo.SelectedValue, out n);
        //        ConstWrapper.Parametri["TipoUtilizzo"] = n;
        //        int.TryParse(ddlTipoPossesso.SelectedValue, out n);
        //        ConstWrapper.Parametri["TipoPossesso"] = n;
        //        ConstWrapper.Parametri["PercPos"] = txtPercentualePos.Text == string.Empty ? 0 : double.Parse(txtPercentualePos.Text);
        //        ConstWrapper.Parametri["ConfrontoImporto"] = ddlTipoValore.SelectedValue;
        //        ConstWrapper.Parametri["Importo"] = txtValore.Text == string.Empty ? 0 : double.Parse(txtValore.Text);
        //        ConstWrapper.Parametri["Coltivatori"] = chkColtivatori.Checked == true ? "1" : "0";
        //        ConstWrapper.Parametri["ImmobiliNoAgg"] = ckbImmNoAgganciati.Checked == true ? "1" : "0";
        //        ConstWrapper.Parametri["PartitaCatastale"] = txtPartitaCatastale.Text;
        //        ConstWrapper.Parametri["Sezione"] = txtSezione.Text;
        //        ConstWrapper.Parametri["Protocollo"] = txtProtocollo.Text;
        //        ConstWrapper.Parametri["Anno"] = txtAnno.Text;
        //        ConstWrapper.Parametri["TipoRicerca"] = "Immobile";

        //        //valoreImmobile= txtValore.Text;

        //        //if (valoreImmobile=="")
        //        //    valoreImmobile="0";
        //        //Importo= double.Parse(valoreImmobile);

        //        //percPos= txtPercentualePos.Text;

        //        //if (percPos=="")
        //        //    percPos="0";
        //        //Possesso= float.Parse(percPos);

        //        PaginaPrecedente.GrdContribuenti.Visible = false;

        //        //*** 201511 - Funzioni Sovracomunali ***
        //        /*DataTable Tabella = new ContribuentiImmobileView().ListContribuenti("", int.Parse(PaginaPrecedente.ddlProv.SelectedValue), PaginaPrecedente.TxtDal.Text, PaginaPrecedente.TxtAl.Text, txtPartitaCatastale.Text,
        //        txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, txtSezione.Text, int.Parse(ConstWrapper.Parametri["Caratteristica"].ToString()), txtAnno.Text,
        //        txtProtocollo.Text, txtVia.Text, ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue,
        //        ConstWrapper.CodiceEnte, (Bonificato)int.Parse(PaginaPrecedente.ddlBonificato.SelectedValue),
        //        double.Parse(ConstWrapper.Parametri["PercPos"].ToString()), double.Parse(ConstWrapper.Parametri["Importo"].ToString()), int.Parse(ddlTipoValore.SelectedValue), int.Parse(ConstWrapper.Parametri["ImmobiliNoAgg"].ToString()),
        //        int.Parse(ConstWrapper.Parametri["AbiPrinc"].ToString()), int.Parse(ConstWrapper.Parametri["TipoUtilizzo"].ToString()), int.Parse(ConstWrapper.Parametri["TipoPossesso"].ToString()), int.Parse(ConstWrapper.Parametri["Coltivatori"].ToString()));*/
        //        DataTable Tabella = new ContribuentiImmobileView().ListContribuenti("", int.Parse(PaginaPrecedente.ddlProv.SelectedValue), PaginaPrecedente.TxtDal.Text, PaginaPrecedente.TxtAl.Text, txtPartitaCatastale.Text,
        //        txtFoglio.Text, txtNumero.Text, txtSubalterno.Text, txtSezione.Text, int.Parse(ConstWrapper.Parametri["Caratteristica"].ToString()), txtAnno.Text,
        //        txtProtocollo.Text, txtVia.Text, ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue,
        //        IdEnte, (Bonificato)int.Parse(PaginaPrecedente.ddlBonificato.SelectedValue),
        //        double.Parse(ConstWrapper.Parametri["PercPos"].ToString()), double.Parse(ConstWrapper.Parametri["Importo"].ToString()), int.Parse(ddlTipoValore.SelectedValue), int.Parse(ConstWrapper.Parametri["ImmobiliNoAgg"].ToString()),
        //        int.Parse(ConstWrapper.Parametri["AbiPrinc"].ToString()), int.Parse(ConstWrapper.Parametri["IsPertinenza"].ToString()), int.Parse(ConstWrapper.Parametri["TipoUtilizzo"].ToString()), int.Parse(ConstWrapper.Parametri["TipoPossesso"].ToString()), int.Parse(ConstWrapper.Parametri["Coltivatori"].ToString()));
        //        //*** ***
        //        //DataTable Tabella = new ContribuentiImmobileView().ListContribuenti("",PaginaPrecedente.TxtDal.Text,PaginaPrecedente.TxtAl.Text,txtPartitaCatastale.Text, txtFoglio.Text,
        //        //            txtNumero.Text, txtSubalterno.Text, txtSezione.Text, txtCaratteristica.Text, txtAnno.Text,
        //        //            txtProtocollo.Text, txtVia.Text, ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue,
        //        //            ConstWrapper.CodiceEnte, (Bonificato)int.Parse(PaginaPrecedente.ddlBonificato.SelectedValue),
        //        //            Possesso, Importo, int.Parse(ddlTipoValore.SelectedValue),ConstWrapper.Parametri["ImmobiliNoAgg"].ToString());
        //        //			DataTable TabellaStampa = new ContribuentiImmobiliTutti().ListContribuentiStampa(PaginaPrecedente.TxtDal.Text,PaginaPrecedente.TxtAl.Text,txtPartitaCatastale.Text, txtFoglio.Text,
        //        //						txtNumero.Text, txtSubalterno.Text, txtSezione.Text, txtCaratteristica.Text, txtAnno.Text,
        //        //						txtProtocollo.Text, txtVia.Text, ddlCategoriaCatastale.SelectedValue, ddlClasse.SelectedValue,
        //        //						ConstWrapper.CodiceEnte, (Bonificato)int.Parse(PaginaPrecedente.ddlBonificato.SelectedValue),
        //        //						Possesso, Importo, int.Parse(ddlImportoPagato.SelectedValue), ConstWrapper.Parametri["ImmobiliNoAgg"].ToString());
        //        DataTable TabellaStampa = Tabella;

        //        /*int CodContribuente = 0;
        //        foreach(DataRow riga in Tabella.Rows)
        //        {
        //            if((int)riga["CodContribuente"] != CodContribuente)
        //                CodContribuente = (int)riga["CodContribuente"];
        //            else
        //                riga.Delete();
        //        }*/

        //        Session["TABELLA_RICERCA_IM"] = Tabella;

        //        // Variabile per la stampa in Excel
        //        Session["TABELLA_STAMPA"] = TabellaStampa;

        //        if (Tabella.Rows.Count > 0)
        //        {
        //            //*** 201511 - Funzioni Sovracomunali ***
        //            if (Business.ConstWrapper.CodiceEnte == "")
        //                PaginaPrecedente.GrdImmobili.Columns[0].Visible = true;
        //            else
        //                PaginaPrecedente.GrdImmobili.Columns[0].Visible = false;
        //            //*** ***
        //            PaginaPrecedente.GrdContribuenti.Visible = false;
        //            Tabella.DefaultView.Sort = "Cognome";
        //            //PaginaPrecedente.GrdContribuenti.start_index = "0";
        //            //PaginaPrecedente.SettaPageIndex0();
        //            PaginaPrecedente.GrdImmobili.DataSource = Tabella;
        //            PaginaPrecedente.GrdImmobili.DataBind();
        //            PaginaPrecedente.GrdImmobili.Visible = true;
        //            PaginaPrecedente.lblRisultati.Text = "Risultati della Ricerca";
        //            PaginaPrecedente.lblNRecord.Text = "N. Posizioni " + Tabella.Rows.Count.ToString();
        //        }
        //        else
        //        {
        //            PaginaPrecedente.lblRisultati.Text = "La ricerca non ha prodotto risultati";
        //            PaginaPrecedente.lblNRecord.Text = "";
        //            PaginaPrecedente.GrdImmobili.Visible = false;
        //            PaginaPrecedente.GrdContribuenti.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.btnTrova_Click.errore: ", ex);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //}
        private void LoadCombos()
        {
            try
            {
                ddlCaratteristica.Items.Clear();
                DataView dvCar = new DataView();
                ListItem myNoSelCar = new ListItem();
                myNoSelCar.Text = "...";
                myNoSelCar.Value = "0";
                ddlCaratteristica.Items.Add(myNoSelCar);
                dvCar = GetListCaratteristica();
                foreach (DataRow myRow in dvCar.Table.Rows)
                {
                    if (myRow["COD_CARATTERISTICA"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelCar1 = new ListItem();
                        myNoSelCar1.Text = myRow["COD_CARATTERISTICA"].ToString() + " - " + (string)myRow["Descrizione_Breve"];
                        myNoSelCar1.Value = myRow["COD_CARATTERISTICA"].ToString();
                        ddlCaratteristica.Items.Add(myNoSelCar1);
                    }
                }

                ddlCategoriaCatastale.Items.Clear();
                DataView dvCat = new DataView();
                ListItem myNoSelCat = new ListItem("...", "0");
                ddlCategoriaCatastale.Items.Add(myNoSelCat);
                dvCat = GetListCategorieCatastali();
                foreach (DataRow myRow in dvCat.Table.Rows)
                {
                    if (myRow["CategoriaCatastale"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelCat1 = new ListItem();
                        myNoSelCat1.Text = (string)myRow["CategoriaCatastale"];
                        myNoSelCat1.Value = (string)myRow["CategoriaCatastale"];
                        ddlCategoriaCatastale.Items.Add(myNoSelCat1);
                    }
                }

                ddlClasse.Items.Clear();
                DataView dvCl = new DataView();
                ListItem myNoSelCl = new ListItem("...", "0");
                ddlClasse.Items.Add(myNoSelCl);
                dvCl = GetListClassi();
                foreach (DataRow myRow in dvCl.Table.Rows)
                {
                    if (myRow["Classe"].ToString().CompareTo("0") != 0)
                    {
                        ListItem myNoSelCl1 = new ListItem();
                        myNoSelCl1.Text = (string)myRow["Classe"];
                        myNoSelCl1.Value = (string)myRow["Classe"];
                        ddlClasse.Items.Add(myNoSelCl1);
                    }
                }

                //*** 20140509 - TASI ***
                ddlTipoUtilizzo.Items.Clear();
                DataView dvDati = new DataView();
                ListItem myNoSelTipoPos = new ListItem("...", "0");
                ddlTipoUtilizzo.Items.Add(myNoSelTipoPos);
                dvDati = GetListTipiUtilizzi();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["IdTipoUtilizzo"].ToString().CompareTo("0") != 0)
                    {
                        myNoSelTipoPos = new ListItem();
                        myNoSelTipoPos.Text = myRow["Descrizione"].ToString();
                        myNoSelTipoPos.Value = myRow["IdTipoUtilizzo"].ToString();
                        ddlTipoUtilizzo.Items.Add(myNoSelTipoPos);
                    }
                }

                ddlTipoPossesso.Items.Clear();
                dvDati = new DataView();
                myNoSelTipoPos = new ListItem("...", "0");
                ddlTipoPossesso.Items.Add(myNoSelTipoPos);
                dvDati = GetListTipiPossesso();
                foreach (DataRow myRow in dvDati.Table.Rows)
                {
                    if (myRow["IdTipoPossesso"].ToString().CompareTo("0") != 0)
                    {
                        myNoSelTipoPos = new ListItem();
                        myNoSelTipoPos.Text = myRow["Descrizione"].ToString();
                        myNoSelTipoPos.Value = myRow["IdTipoPossesso"].ToString();
                        ddlTipoPossesso.Items.Add(myNoSelTipoPos);
                    }
                }
                //*** ***

            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroImmobile.LoadCombos.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
    }
}
