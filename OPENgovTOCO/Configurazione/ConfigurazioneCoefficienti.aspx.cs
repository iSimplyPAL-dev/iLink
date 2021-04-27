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
/// Pagina la consultazione/gestione dei coefficienti.
/// Le possibili opzioni sono:
/// - Ribalta
/// - Nuovo inserimento
/// - Salva
/// - Elimina
/// </summary>
    public partial class ConfigurazioneCoefficienti : BasePage
    {
        #region definizioni
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblTitolo;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Panel pnlCategorie;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Panel pnlAgevolazioni;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Panel pnlTipoOccupazioni;
        /// <summary>
        /// 
        /// </summary>
         protected System.Web.UI.WebControls.Label lblDescrizioneOperazione;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblTabSel;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txbDescrizione;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox tbxIdRecordToMod;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button btCanc;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txbAnno;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblAnnoMod;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblReq1;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurazioneCoefficienti));
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
                if (!Page.IsPostBack)
                {
                    DichiarazioneSession.CodTributo((Request.Params["CodTributo"] == null) ? "" : Request.Params["CodTributo"].ToString());
                    btDel.Attributes.Add("click", "confermaEliminazione(); return false;");

                    LoadCoefficienti(!IsPostBack);
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coeff"></param>
        /// <param name="bRebind"></param>
        /// <param name="page"></param>
        private void DataBindGridCoefficienti(Coefficienti[] coeff,  bool bRebind, int? page = 0)
        {
            try {
                //			GrdCoefficienti.SelectedIndex = -1;
                if (coeff != null && coeff.Length > 0)
                {
                    GrdCoefficienti.DataSource = coeff;
                    if (bRebind)
                    {
                        GrdCoefficienti.DataBind();
                    }
                    if (page.HasValue)
                        GrdCoefficienti.PageIndex = page.Value;
                    GrdCoefficienti.DataBind();
                    GrdCoefficienti.Visible = true;
                    lblResultList.Visible = false;
                }
                else
                {
                    lblResultList.Visible = true;
                    lblResultList.Text = "Nessun risultato per questa ricerca.";
                    GrdCoefficienti.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.DataBindGridCoefficienti.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

        #region "Griglie"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e) {
            try {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen") {
                    foreach (GridViewRow myRow in GrdCoefficienti.Rows)
                        if (IDRow == ((HiddenField)myRow.FindControl("hfIdTabella")).Value)
                        {
                            //TableCell itemCell = SharedFunction.CellByName(myRow, "IdTabella");
                            TableCell item1Cell = SharedFunction.CellByName(myRow, "Descrizione");
                            TableCell item2Cell = SharedFunction.CellByName(myRow, "Anno");
                            TableCell item3Cell = SharedFunction.CellByName(myRow, "Coefficiente");

                            bool fail = false;

                            if (((HiddenField)myRow.FindControl("hfIdTabella")) == null || item2Cell == null || item3Cell == null)
                            {
                                Log.Error("Impossibile recuperare l'id, l'Anno o il coefficiente da modificare.");
                                fail = true;
                            }

                            if (fail == true)
                            {
                                string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile completare l\'operazione.');" + "\r\n";
                                ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
                            }
                            else
                            {
                                pnlModifica.Visible = true;
                                pnlInserisci.Visible = false;

                                txbIdRecordToMod.Text = ((HiddenField)myRow.FindControl("hfIdTabella")).Value;
                                lblElmSelModifica.Text = item1Cell.Text;
                                lblAnnoSelModifica.Text = item2Cell.Text;
                                txbCoefficiente.Text = item3Cell.Text;
                            }
                        }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.GdrRowCommand.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }

        }
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e) {
            LoadCoefficienti(!IsPostBack,e.NewPageIndex);
        }
     #endregion

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
    protected void btNew_Click(object sender, System.EventArgs e)
		{
            
                pnlModifica.Visible = false;
                pnlInserisci.Visible = true;
                txbIdRecordToMod.Text = "0";
                txbAnnoInsert.Text = null;
                txbCoefficienteInsert.Text = null;
                LoadDescrizioni();
            
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btSalva_Click(object sender, System.EventArgs e)
		{
			string sScript = null;
			if (SharedFunction.IsNullOrEmpty(txbIdRecordToMod.Text) == true)
			{
				sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');" + "\r\n";
				ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
				return;
			}
			//Check Insert or Update
			int IdToMod = 0;
			int idTabella = 0;
			int esito = 0;
			int Anno = 0;
			decimal Coefficiente = 0;
			try
			{
				IdToMod = int.Parse(txbIdRecordToMod.Text);
			} 
			catch {
              
                IdToMod = -1;
            }

			switch(IdToMod)
			{
				case -1: //Errore
					sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');" + "\r\n";
					ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
					return;
				case 0: //Inserimento
					if (SharedFunction.IsNullOrEmpty(txbAnnoInsert.Text))//Check Anno
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci l\'Anno.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					if (SharedFunction.IsNullOrEmpty(txbCoefficienteInsert.Text))//Check Coefficiente
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Coefficiente.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					try{idTabella = int.Parse(ddlDescrizioniIns.SelectedValue);}
					catch{idTabella = -1;}
					if(idTabella == -1)
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Seleziona La Descrizione.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}

					try{Anno = int.Parse(txbAnnoInsert.Text);}
					catch{Anno = -1;}
					if(Anno == -1)
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci l\'Anno.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}

					try{Coefficiente = decimal.Parse(txbCoefficienteInsert.Text);}
					catch{Coefficiente = -1;}
					if(Coefficiente == -1)
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Coefficiente.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					if (EsisteCoeff(idTabella, Anno))
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Il Coefficiente esiste già.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					break;
				default: //Modifica
					if (SharedFunction.IsNullOrEmpty(lblAnnoSelModifica.Text))//Check Anno
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci l\'Anno.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					if (SharedFunction.IsNullOrEmpty(txbCoefficiente.Text))//Check Coefficiente
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Coefficiente.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					try{Anno = int.Parse(lblAnnoSelModifica.Text);}
					catch{Anno = -1;}
					if(Anno == -1)
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci l\'Anno.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}

					try{Coefficiente = decimal.Parse(txbCoefficiente.Text);}
					catch{Coefficiente = -1;}
					if(Coefficiente == -1)
					{
						sScript = "GestAlert('a', 'warning', '', '', 'Inserisci il Coefficiente.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					idTabella = IdToMod;
					break;
			}
			if (RBCategoria.Checked == true)
			{
				esito = MetodiCategorie.InsertOrUpdateCoefficiente(idTabella, Anno, Coefficiente);
                if (esito == 1)
                {
                    esito = 0;
                }
                else
                { esito = -1; }
			}		
			else if (RBAgevolazione.Checked == true)
			{
				esito = MetodiAgevolazione.InsertOrUpdateCoefficiente(idTabella, Anno, Coefficiente);
                if (esito == 1)
                {
                    esito = 0;
                }
                else
                { esito = -1; }
			}
			else if (RBTipologiaOccupazioni.Checked == true)
			{
				esito = MetodiTipologieOccupazioni.InsertOrUpdateCoefficiente(idTabella, Anno, Coefficiente);
                if (esito == 1)
                {
                    esito = 0;
                }
                else
                { esito = -1; }
			}
			/*
				* CONTROLLI ESITI
				* 
				* */
			if (esito >= 0)
			{
				if (IdToMod > 0)
					sScript = "GestAlert('a', 'success', '', '', 'Coefficiente modificato correttamente.');" + "\r\n";
				else
				{
					sScript = "GestAlert('a', 'success', '', '', 'Inserimento avvenuto correttamente.');" + "\r\n";
				}
				ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
				LoadCoefficienti(true);
			}
			else
			{
				sScript = "GestAlert('a', 'warning', '', '', 'Impossibile salvare il Coefficiente.');" + "\r\n";
				ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btDel_Click(object sender, System.EventArgs e)
		{
			if (SharedFunction.IsNullOrEmpty(txbIdRecordToMod.Text) == true || txbIdRecordToMod.Text == "0")
			{
				string sScript = "GestAlert('a', 'warning', '', '', 'Devi prima selezionare un record.');" + "\r\n";
				ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
			}
			else
			{
				int esito = 0;
				int IdToMod = 0;
				int Anno = 0;

				try
				{
					IdToMod = int.Parse(txbIdRecordToMod.Text);
				} 
				catch { IdToMod = -1; }

				// Inserimento o Update
				if (IdToMod > 0)
				{
					try{Anno = int.Parse(lblAnnoSelModifica.Text);}
					catch{Anno = -1;}
					if (Anno == -1)
					{
						string sScript = "GestAlert('a', 'warning', '', '', 'Anno non specificato.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
						return;
					}
					if (RBCategoria.Checked == true)
					{
						esito= MetodiCategorie.DeleteCoefficiente(IdToMod, Anno);
                        if (esito == 1)
                        {
                            esito = 0;
                        }
                        else
                        { esito = -1; }
					}		
					else if (RBAgevolazione.Checked == true)
					{
						esito = MetodiAgevolazione.DeleteCoefficiente(IdToMod, Anno);
                        if (esito == 1)
                        {
                            esito = 0;
                        }
                        else
                        { esito = -1; }
					}
					else if (RBTipologiaOccupazioni.Checked == true)
					{
						esito = MetodiTipologieOccupazioni.DeleteCoefficiente(IdToMod, Anno);
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
					string sScript = "GestAlert('a', 'danger', '', '', 'Errore del sistema');" + "\r\n";
					ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
				}		
		
				/*
				* CONTROLLI ESITI
				* 
				* */
				if (esito == 0)
				{
					string sScript = "GestAlert('a', 'success', '', '', 'Cancellazione avvenuta con successo.');" + "\r\n";
					ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");

					LoadCoefficienti(true);

					txbIdRecordToMod.Text = null;
					pnlModifica.Visible = false;
					pnlInserisci.Visible = false;
				}
				else
				{
					if (esito == -1)
					{
						string sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! Non è possibile cancellare questo elemento poiché vi sono degli articoli associati.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
					} 
					else
					{
						string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile salvare la descrizione.');" + "\r\n";
						ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
					}
				}	

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
                    string sScript = " GestAlert('a', 'warning', '', '', 'Compilare i Campi Da: e A:') ";
                    RegisterScript("<script language=\'javascript\'>" + sScript + "</script>", this.GetType());
                }
                else
                {
                    int esito = 0;
                    int RibaltaDa = int.Parse(txtRibaltaDa.Text);
                    int RibaltaA = int.Parse(txtRibaltaA.Text);

                    if (RBCategoria.Checked == true)
                    {
                        esito = MetodiCategorie.SetRibalta(RibaltaDa, RibaltaA, (string)DichiarazioneSession.IdEnte);
                        if (esito == 1)
                        {
                            esito = 0;
                        }
                        else
                        { esito = -1; }
                    }
                    else if (RBAgevolazione.Checked == true)
                    {
                        esito = MetodiAgevolazione.SetRibalta(RibaltaDa, RibaltaA, (string)DichiarazioneSession.IdEnte);
                        if (esito == 1)
                        {
                            esito = 0;
                        }
                        else
                        { esito = -1; }
                    }
                    else if (RBTipologiaOccupazioni.Checked == true)
                    {
                        esito = MetodiTipologieOccupazioni.SetRibalta(RibaltaDa, RibaltaA, (string)DichiarazioneSession.IdEnte);
                        if (esito == 1)
                        {
                            esito = 0;
                        }
                        else
                        { esito = -1; }
                    }

                    if (esito == 0)
                    {
                        string sScript = "GestAlert('a', 'success', '', '', 'Ribaltamento avvenuto con successo.') ";
                        RegisterScript("<script language=\'javascript\'>" + sScript + "</script>", this.GetType());
                    }
                    else
                    {
                        if (esito == -1)
                        {
                            string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile Ribaltare alla data inserita. \nData già presente.') ";
                            RegisterScript("<script language=\'javascript\'>" + sScript + "</script>", this.GetType());
                        }
                        else
                        {
                            string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile Ribaltare alla data inserita.')";
                            RegisterScript("<script language=\'javascript\'>" + sScript + "</script>", this.GetType());
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.btRibalta_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="page"></param>
		private void LoadCoefficienti(bool binding, int? page = 0)
		{
            try {
                Coefficienti[] coefficienti = null;

                if (RBCategoria.Checked == true)
                { coefficienti = MetodiCategorie.GetAllCoefficienti((string)DichiarazioneSession.IdEnte); }
                else if (RBAgevolazione.Checked == true)
                { coefficienti = MetodiAgevolazione.GetAllCoefficienti((string)DichiarazioneSession.IdEnte); }
                else if (RBTipologiaOccupazioni.Checked == true)
                { coefficienti = MetodiTipologieOccupazioni.GetAllCoefficienti((string)DichiarazioneSession.IdEnte); }
                DataBindGridCoefficienti(coefficienti, binding, page);

                pnlInserisci.Visible = pnlModifica.Visible = false;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.LoadCoefficienti.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
		private void LoadDescrizioni()
		{
            try {
                if (RBAgevolazione.Checked)
                {
                    Agevolazione[] lista = MetodiAgevolazione.GetAllAgevolazioni((string)DichiarazioneSession.IdEnte);
                    if (lista != null && lista.Length > 0)
                    {
                        ddlDescrizioniIns.DataSource = lista;
                        ddlDescrizioniIns.DataTextField = "Descrizione";
                        ddlDescrizioniIns.DataValueField = "IdAgevolazione";
                    }
                }
                if (RBCategoria.Checked)
                {
                    Categorie[] lista = MetodiCategorie.GetAllCategorie((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                    if (lista != null && lista.Length > 0)
                    {
                        ddlDescrizioniIns.DataSource = lista;
                        ddlDescrizioniIns.DataTextField = "Descrizione";
                        ddlDescrizioniIns.DataValueField = "IdCategoria";
                    }
                }
                if (RBTipologiaOccupazioni.Checked)
                {
                    TipologieOccupazioni[] lista = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni((string)DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                    if (lista != null && lista.Length > 0)
                    {
                        ddlDescrizioniIns.DataSource = lista;
                        ddlDescrizioniIns.DataTextField = "Descrizione";
                        ddlDescrizioniIns.DataValueField = "IdTipologiaOccupazione";
                    }
                }
                ddlDescrizioniIns.DataBind();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.LoadDescrizioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTabella"></param>
        /// <param name="Anno"></param>
        /// <returns></returns>
		private bool EsisteCoeff(int IdTabella, int Anno)
		{
            try {
                Coefficienti coeff = null;
                if (RBCategoria.Checked == true)
                {
                    coeff = MetodiCategorie.GetCoefficiente(IdTabella, Anno);
                }
                else if (RBAgevolazione.Checked == true)
                {
                    coeff = MetodiAgevolazione.GetCoefficiente(IdTabella, Anno);
                }
                else if (RBTipologiaOccupazioni.Checked == true)
                {
                    coeff = MetodiTipologieOccupazioni.GetCoefficiente(IdTabella, Anno);
                }
                return (coeff != null);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ConfigurazioneCoefficienti.EsisteCoeff.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void RBCategoria_CheckedChanged(object sender, System.EventArgs e)
		{
			LoadCoefficienti(true);

			lblIdTabIns.Text=lblTabSelModifica.Text="Categorie";
			txbIdRecordToMod.Text = null;

			txbAnnoInsert.Text = null;
			txbCoefficiente.Text = null;
			pnlModifica.Visible = false;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void RBAgevolazione_CheckedChanged(object sender, System.EventArgs e)
		{
			LoadCoefficienti(true);

			lblIdTabIns.Text=lblTabSelModifica.Text="Agevolazioni";
			txbIdRecordToMod.Text = null;

			txbAnnoInsert.Text = null;
			txbCoefficiente.Text = null;
			pnlModifica.Visible = false;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void RBTipologiaOccupazioni_CheckedChanged(object sender, System.EventArgs e)
		{
			LoadCoefficienti(true);

			lblIdTabIns.Text=lblTabSelModifica.Text="TipologiaOccupazioni";
			txbIdRecordToMod.Text = null;

			txbAnnoInsert.Text = null;
			txbCoefficiente.Text = null;
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
			//this.GrdCoefficienti.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdCoefficienti_ItemCommand);

		}
		#endregion

	}
}
