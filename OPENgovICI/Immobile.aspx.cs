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
using log4net;

namespace DichiarazioniICI
{

	/// <summary>
	/// Classe di gestione della pagina Immobile.
	/// </summary>
	public partial class Immobile : BaseEnte
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Immobile));

		#region Property
		/// <summary>
		/// Ritorna o assegna l'id della testata/dichiarazione.
		/// </summary>
		protected int IDTestata
		{
			get { return ViewState["IDTestata"] != null ? (int)ViewState["IDTestata"] : 0; }
			set { ViewState["IDTestata"] = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		protected DataView viewDettagliImmobili
		{
			get { return (new DettagliImmobiliView().ListByIDTestata(this.IDTestata, 0)); }
			set { viewDettagliImmobili = value; }
		}

		/// <summary>
		/// Ritorna o assegna l'id dell'immobile.
		/// </summary>
		protected int IDOggetto
		{
			get { return ViewState["IDOggetto"] != null ? (int)ViewState["IDOggetto"] : 0; }
			set { ViewState["IDOggetto"] = value; }
		}
		#endregion

		#region Metodi
		/// <summary>
		/// Ritorna una DataView valorizzata con l'elenco delle categorie catastali.
		/// </summary>
		/// <returns></returns>
		protected DataView GetListCategorieCatastali()
		{
			DataView Vista = new CategoriaCatastaleTable(ConstWrapper.sUsername).List().DefaultView;
			Vista.Sort = "Descrizione";
			return Vista;
		}

		/// <summary>
		/// Ritorna una DataView valorizzata con l'elenco delle classi.
		/// </summary>
		/// <returns></returns>
		protected DataView GetListClassi()
		{
			DataView Vista = new ClasseTable(ConstWrapper.sUsername).List().DefaultView;
			Vista.Sort = "Descrizione";
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
		/// Ritorna una DataView valorizzata con l'elenco delle valute.
		/// </summary>
		/// <returns></returns>
		protected DataView GetListValute()
		{
			DataView Vista = new ValutaTable(ConstWrapper.sUsername).List().DefaultView;
			Vista.Sort = "Descrizione";
			return Vista;
		}

		/// <summary>
		/// Torna l'indirizzo dell'immobile.
		/// </summary>
		/// <returns></returns>
		protected string GetIndirizzo(int idImmobile)
		{
			string indirizzo;
			Utility.DichManagerICI.OggettiRow riga = new OggettiTable(ConstWrapper.sUsername).GetRow(idImmobile, ConstWrapper.StringConnection);
			indirizzo = riga.Via;
			try {
				if (riga.NumeroCivico != -1)
					indirizzo += " " + riga.NumeroCivico.ToString();
				if (riga.Barrato != String.Empty)
					indirizzo += "/" + riga.Barrato;
				if (riga.Interno != String.Empty)
					indirizzo += ", Interno " + riga.Interno;
				if (riga.Scala != String.Empty)
					indirizzo += ", Scala " + riga.Scala;
				if (riga.Piano != String.Empty)
					indirizzo += ", Piano " + riga.Piano;

				return indirizzo;
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.GetIndirizzo.errore: ", Err);
				Response.Redirect("../../PaginaErrore.aspx");
				throw Err;
			}

		}
		#endregion
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (!IsPostBack)
				{
					ViewState.Add("SortKey", "Foglio");
					ViewState.Add("OrderBy", "ASC");

					this.IDTestata = int.Parse(Request.QueryString["IDTestata"]);

					GrdImmobiliBind(this.IDTestata);

					if (Request.QueryString["IDOggetto"] != null)
					{
						this.IDOggetto = int.Parse(Request.QueryString["IDOggetto"]);
						GrdImmobiliBind(this.IDTestata);
					}

					// rendo visibile la label se la dichiarazione non è bonificata
					//           if (new TestataTable(ConstWrapper.sUsername).GetRow(this.IDTestata, this.IDOggetto, ConstWrapper.StringConnection).Bonificato == false)
					//lblBonificata.Visible = true;
				}
				else
				{
					// faccio il bind della ribesdatagrid per gestire la paginazione
					//GrdImmobili.DataSource = Session["VistaImmobili"];
				}
				SetGrdCheck();
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.Page_Load.errore: ", Err);
				Response.Redirect("../PaginaErrore.aspx");
				throw Err;
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
			//this.GrdImmobili.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.rGrdImmobili_ItemCommand);
			//this.GrdImmobili.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.GrdImmobili_SortCommand);
		}
		#endregion

		/// <summary>
		/// Prende la quantità degli immobili quali appartengono alla testata selezionata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		private int CountImmobili(int idTestata)
		{
			int Immobili = (int)new OggettiTable(ConstWrapper.sUsername).CountImmobiliByIDTestata(idTestata);
			return Immobili;
		}

		/// <summary>
		/// Prende la quantità degli immobili già bonificati che appartengono alla testata selezionata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		private int CountImmobiliBonificati(int idTestata)
		{
			int ImmobiliBonificati = (int)new OggettiTable(ConstWrapper.sUsername).CountImmobiliBonificatiByIDTestata(idTestata);
			return ImmobiliBonificati;
		}

		/// <summary>
		/// Prende la quantità dei contitolari quali appartengono alla testata selezionata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		private int CountContitolari(int idTestata)
		{
			int Contitolari = (int)new DettaglioTestataTable(ConstWrapper.sUsername).CountContitolariByIDTestata(idTestata);
			return Contitolari;
		}

		/// <summary>
		/// Prende la quantità dei contitolari già bonificati che appartengono alla testata selezionata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		private int CountContitoariBonificati(int idTestata)
		{
			int ContitolariBonificati = (int)new DettaglioTestataTable(ConstWrapper.sUsername).CountContitolariBonificatiByIDTestata(idTestata);
			return ContitolariBonificati;
		}

		/// <summary>
		/// Esegue il bind della DataGrid.
		/// </summary>
		/// <param name="idTestata"></param>
		private void GrdImmobiliBind(int idTestata)
		{
			//*** 20141110 - passaggio di proprietà ***
			DataView Vista = new DataView();
			try
			{
				if (Session["ListUIPassProp"]!=null)
					Vista = (DataView)Session["ListUIPassProp"];
				else
				{
					if (idTestata > 0)
					{
						int PassProp = 0;
						int.TryParse(Request.QueryString["PassProp"], out PassProp);
						Vista = new DettagliImmobiliView().ListByIDTestata(idTestata, PassProp);
					}
				}
				if (Vista.Count == 0)
				{
					lblMessage.Visible = true;
				}
				else
				{
					Session["VistaImmobili"] = Vista;
					Vista.Sort = "Foglio";
					GrdImmobili.DataSource = Vista;
					GrdImmobili.DataBind();
					lblMessage.Visible = false;

					Session["ListUIPassProp"] = Vista;
				}
				//*** ***
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.GrdImmobiliBind.errore: ", Err);
				Response.Redirect("../PaginaErrore.aspx");
				throw Err;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnbAddImmobile_Click(object sender, System.EventArgs e)
		{
			//Response.Redirect("ImmobileDettaglio.aspx?IDTestata=" + this.IDTestata);
			ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE");
		}

		/// <summary>
		/// Aggiunge un modulo vuoto per inserire un nuovo immobile.
		/// </summary>
		private void AggiungiImmobile()
		{
			DataView vista = viewDettagliImmobili;
			DettagliImmobiliView.AddEmptyRow(ref vista, this.IDTestata);
			GrdImmobili.DataSource = vista;
		}

		/// <summary>
		/// Esegue il salvataggio dei dati dell'immobile.
		/// </summary>
		/// <param name="DataGrid"></param>
		private void SalvaImmobile(System.Web.UI.WebControls.DataGridCommandEventArgs DataGrid)
		{
			int idOggetto;
			//*** 20130923 - gestione modifiche tributarie ***
			try {
				Utility.ModificheTributarie FncModificheTributarie = new Utility.ModificheTributarie();
				//*** ***
				Utility.DichManagerICI.OggettiRow Riga = new Utility.DichManagerICI.OggettiRow();
				Riga.Ente = ConstWrapper.CodiceEnte;
				Riga.IdTestata = this.IDTestata;
				Riga.NumeroOrdine = ((TextBox)DataGrid.Item.FindControl("txtNumOrdine")).Text;
				Riga.NumeroModello = ((TextBox)DataGrid.Item.FindControl("txtNumModello")).Text;
				Riga.CodUI = ((TextBox)DataGrid.Item.FindControl("txtCodiceUI")).Text;
				Riga.TipoImmobile = int.Parse(((DropDownList)DataGrid.Item.FindControl("ddlTipoImmobile")).SelectedItem.Value);
				Riga.PartitaCatastale = int.Parse(((TextBox)DataGrid.Item.FindControl("txtPartitaCatastale")).Text);
				Riga.Foglio = ((TextBox)DataGrid.Item.FindControl("txtFoglio")).Text;
				Riga.Numero = ((TextBox)DataGrid.Item.FindControl("txtNumero")).Text;
				Riga.Subalterno = int.Parse(((TextBox)DataGrid.Item.FindControl("txtSubalterno")).Text);
				Riga.Caratteristica = int.Parse(((DropDownList)DataGrid.Item.FindControl("ddlCaratteristica")).SelectedItem.Value);
				Riga.Sezione = ((TextBox)DataGrid.Item.FindControl("txtSezione")).Text;
				Riga.NumeroProtCatastale = ((TextBox)DataGrid.Item.FindControl("txtNumeroProtocollo")).Text;
				Riga.AnnoDenunciaCatastale = ((TextBox)DataGrid.Item.FindControl("txtAnnoDenunciaCatastale")).Text;
				Riga.CodCategoriaCatastale = ((DropDownList)DataGrid.Item.FindControl("ddlCategoriaCatastale")).SelectedItem.Value;
				Riga.CodClasse = ((DropDownList)DataGrid.Item.FindControl("ddlClasse")).SelectedItem.Value;
				Riga.CodRendita = ((DropDownList)DataGrid.Item.FindControl("ddlCodiceRendita")).SelectedItem.Value;
				Riga.Storico = ((CheckBox)DataGrid.Item.FindControl("chkImmobileStorico")).Checked;
				Riga.ValoreImmobile = Convert.ToDecimal(((TextBox)DataGrid.Item.FindControl("txtValoreImmobile")).Text);// float.Parse(((TextBox)DataGrid.Item.FindControl("txtValoreImmobile")).Text);
				Riga.IDValuta = int.Parse(((DropDownList)DataGrid.Item.FindControl("ddlValuta")).SelectedItem.Value);
				Riga.FlagValoreProvv = ((CheckBox)DataGrid.Item.FindControl("chkValoreProvvisorio")).Checked;
				Riga.CodComune = int.Parse(((TextBox)DataGrid.Item.FindControl("txtCodiceComune")).Text);
				Riga.CodVia = ((TextBox)DataGrid.Item.FindControl("txtVia")).Text;
				Riga.NumeroCivico = int.Parse(((TextBox)DataGrid.Item.FindControl("txtNumCivico")).Text);
				Riga.EspCivico = ((TextBox)DataGrid.Item.FindControl("txtEspCivico")).Text;
				Riga.Scala = ((TextBox)DataGrid.Item.FindControl("txtScala")).Text;
				Riga.Interno = ((TextBox)DataGrid.Item.FindControl("txtInterno")).Text;
				Riga.Piano = ((TextBox)DataGrid.Item.FindControl("txtPiano")).Text;
				Riga.Piano = ((TextBox)DataGrid.Item.FindControl("txtPiano")).Text;
				Riga.Barrato = ((TextBox)DataGrid.Item.FindControl("txtBarrato")).Text;
				Riga.NumeroEcografico = int.Parse(((TextBox)DataGrid.Item.FindControl("txtNumeroEcografico")).Text);
				Riga.TitoloAcquisto = int.Parse(((DropDownList)DataGrid.Item.FindControl("ddlAcquisto")).SelectedValue);
				Riga.TitoloCessione = int.Parse(((DropDownList)DataGrid.Item.FindControl("ddlCessione")).SelectedValue);
				//Riga.TitoloAcquisto = ((CheckBox)DataGrid.Item.FindControl("chkAcquisto")).Checked;
				//Riga.TitoloCessione = ((CheckBox)DataGrid.Item.FindControl("chkCessione")).Checked;
				Riga.DescrUffRegistro = ((TextBox)DataGrid.Item.FindControl("txtDescrizioneUffRegistro")).Text;
				Riga.DataInizioValidità = DateTime.Now;
				Riga.DataFineValidità = DateTime.MinValue;
				Riga.Bonificato = false;
				Riga.Annullato = false;
				string Giorno = ((TextBox)DataGrid.Item.FindControl("txtDataUltimaModifica")).Text.Substring(0, 2);
				string Mese = ((TextBox)DataGrid.Item.FindControl("txtDataUltimaModifica")).Text.Substring(2, 2);
				string Anno = ((TextBox)DataGrid.Item.FindControl("txtDataUltimaModifica")).Text.Substring(4, 4);
				DateTime DataUltimaModifica = Convert.ToDateTime(Giorno + "/" + Mese + "/" + Anno);
				Riga.DataUltimaModifica = DataUltimaModifica;
				Riga.Operatore = ConstWrapper.sUsername;

				if (((Label)DataGrid.Item.FindControl("lblIDOggetto")).Text == String.Empty)
				{
					// inserisce un nuovo immobile
					//*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
					//new OggettiTable(ConstWrapper.sUsername).Insert(Riga, out idOggetto);
					new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_NEW, Riga, this.IDTestata, out idOggetto);
					//*** ***
					this.IDOggetto = idOggetto;
					//*** 20130923 - gestione modifiche tributarie ***
					if (FncModificheTributarie.SetModificheTributarie(ConstWrapper.StringConnectionOPENgov.ToString(), (int)Utility.ModificheTributarie.DBOperation.Insert, 0, ConstWrapper.CodiceEnte, Session["COD_TRIBUTO"].ToString(), (int)Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, Riga.Foglio, Riga.Numero, Riga.Subalterno.ToString(), DateTime.Now, Session["username"].ToString(), Riga.ID, DateTime.MaxValue) == false)
					{
						log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" + Session["COD_ENTE"] + "::@TRIBUTO=" + Session["COD_TRIBUTO"] + "::@IDCAUSALE=" + Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali + "::@FOGLIO=" + Riga.Foglio + "::@NUMERO=" + Riga.Numero + "::@SUBALTERNO=" + Riga.Subalterno + "::@DATAVARIAZIONE=" + DateTime.Now.ToString() + "::@OPERATORE=" + Session["username"] + "::@IDOGGETTOTRIBUTI=" + Riga.ID + "::@DATATRATTATO=" + DateTime.MaxValue.ToString());
					}
					//*** ***
				}
				else
				{
					Riga.ID = int.Parse(((Label)DataGrid.Item.FindControl("lblIDOggetto")).Text);
					// modifico i dati vecchi
					//*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
					//new OggettiTable(ConstWrapper.sUsername).Modify(Riga);
					new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetOggetti(Utility.Costanti.AZIONE_UPDATE, Riga, this.IDTestata, out idOggetto);
					//*** ***
				}
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.SalvaImmobile.errore: ", Err);
				Response.Redirect("../PaginaErrore.aspx");
				throw Err;
			}
		}

		/// <summary>
		/// Esegue il salvataggio del dettaglio nella TblDettaglioTestata.
		/// </summary>
		/// <param name="DataGrid"></param>
		/// <revisionHistory>
		/// <revision date="09/05/2014">
		/// <strong>TASI</strong>
		/// </revision>
		/// </revisionHistory>
		/// <revisionHistory>
		/// <revision date="07/2015">
		/// <strong>GESTIONE INCROCIATA RIFIUTI/ICI</strong>
		/// </revision>
		/// </revisionHistory>
		/// <revisionHistory>
		/// <revision date="12/04/2019">
		/// <strong>Qualificazione AgID-analisi_rel01</strong>
		/// <em>Analisi eventi</em>
		/// </revision>
		/// </revisionHistory>
		private void SalvaDettaglio(System.Web.UI.WebControls.DataGridCommandEventArgs DataGrid)
		{
			int idDettaglio;
			try
			{
				Utility.DichManagerICI.DettaglioTestataRow RigaDettaglio = new Utility.DichManagerICI.DettaglioTestataRow();
				RigaDettaglio.Ente = ConstWrapper.CodiceEnte;
				RigaDettaglio.IdTestata = this.IDTestata;
				RigaDettaglio.NumeroOrdine = ((TextBox)DataGrid.Item.FindControl("txtNumOrdine")).Text;
				RigaDettaglio.NumeroModello = ((TextBox)DataGrid.Item.FindControl("txtNumModello")).Text;
				RigaDettaglio.IdSoggetto = 0;
				RigaDettaglio.TipoUtilizzo = 0;
				RigaDettaglio.TipoPossesso = 0;
				RigaDettaglio.PercPossesso = Convert.ToDecimal(((TextBox)DataGrid.Item.FindControl("txtPercentualePossesso")).Text);
				RigaDettaglio.MesiPossesso = int.Parse(((TextBox)DataGrid.Item.FindControl("txtMesiPossesso")).Text);
				RigaDettaglio.MesiEsclusioneEsenzione = int.Parse(((TextBox)DataGrid.Item.FindControl("txtMesiEsclusioneEsenzione")).Text);
				RigaDettaglio.MesiRiduzione = int.Parse(((TextBox)DataGrid.Item.FindControl("txtMesiRiduzione")).Text);
				RigaDettaglio.ImpDetrazAbitazPrincipale = Convert.ToDecimal(((TextBox)DataGrid.Item.FindControl("txtImpDetrazione")).Text);
				RigaDettaglio.Contitolare = false;
				RigaDettaglio.Bonificato = false;
				RigaDettaglio.Annullato = false;
				RigaDettaglio.DataInizioValidità = DateTime.Now;
				RigaDettaglio.DataFineValidità = DateTime.MinValue;
				RigaDettaglio.Operatore = ConstWrapper.sUsername;

				if (((Label)DataGrid.Item.FindControl("lblIDDettaglio")).Text == String.Empty)
				{
					RigaDettaglio.DataFineValidità = DateTime.MaxValue;
					// inserisce un nuovo immobile
					RigaDettaglio.IdOggetto = this.IDOggetto;
					new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestataCompleta(RigaDettaglio, out idDettaglio);
					((Label)DataGrid.Item.FindControl("lblIDDettaglio")).Text = idDettaglio.ToString();
				}
				else
				{

					RigaDettaglio.DataFineValidità = DateTime.Now;
					// modifico il dettaglio della testata
					RigaDettaglio.ID = int.Parse(((Label)DataGrid.Item.FindControl("lblIDDettaglio")).Text);
					RigaDettaglio.IdOggetto = int.Parse(((Label)DataGrid.Item.FindControl("lblIDOggetto")).Text);
					new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetDettaglioTestataCompleta(RigaDettaglio, out idDettaglio);
				}
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.SalvaDettaglio.errore: ", Err);
				Response.Redirect("../PaginaErrore.aspx");
				throw Err;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnbTornaTestata_Click(object sender, System.EventArgs e)
		{
			ApplicationHelper.LoadFrameworkPage("SR_INSERISCI_DICH", "?IDTestata=" + this.IDTestata.ToString() + "&TYPEOPERATION=GESTIONE");
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
					foreach (GridViewRow myRow in GrdImmobili.Rows)
					{
						if (IDRow == ((HiddenField)myRow.FindControl("hfIDOGGETTO")).Value)
						{
							RegisterScript(ApplicationHelper.LoadFrameworkPageFromIframe("SR_IMMOBILE_DETTAGLIO", "?IDTestata=" + this.IDTestata + "&IDImmobile=" + IDRow + "&TYPEOPERATION=GESTIONE"), this.GetType());
						}
					}
				}
			}
			catch (Exception ex)
			{

				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.GrdRowCommand.errore: ", ex);
				Response.Redirect("../PaginaErrore.aspx");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
		{
			LoadSearch(e.NewPageIndex);
		}
		#endregion
		private void LoadSearch(int? page = 0)
		{
			try
			{
				GrdImmobili.DataSource = (DataView)Session["VistaImmobili"];
				if (page.HasValue)
					GrdImmobili.PageIndex = page.Value;
				GrdImmobili.DataBind();
			}
			catch (Exception ex)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.LoadSearch.errore: ", ex);
				Response.Redirect("../PaginaErrore.aspx");
				throw ex;
			}
		}
		private void SetGrdCheck()
		{
			Ribes.OPENgov.WebControls.RibesGridView myGrd = null;
			string ListUIPassaggio = "";

			try
			{
				myGrd = GrdImmobili;
				foreach (GridViewRow itemGrid in myGrd.Rows)
				{
					if (((CheckBox)itemGrid.FindControl("chkSel")).Checked)
					{
						if (ListUIPassaggio != string.Empty)
							ListUIPassaggio += "|";
						ListUIPassaggio += ((HiddenField)itemGrid.FindControl("hfIDOGGETTO")).Value;
					}
				}
				Session["ListUIPassaggio"] = ListUIPassaggio;
			}
			catch (Exception ex)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Immobile.SetGrdCheck.errore: ", ex);
				Response.Redirect("../PaginaErrore.aspx");
			}
		}
	}
}
