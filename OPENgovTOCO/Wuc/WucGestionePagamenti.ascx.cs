using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using log4net;
using AnagInterface; 
using IRemInterfaceOSAP;
using DTO;
using OPENgovTOCO;
using DAO;
using System.Data.SqlClient;
namespace OPENGovTOCO.Wuc
{
    /// <summary>
    ///	Usercontrol per la gestione Pagamenti.
    /// </summary>
    public partial class WucGestionePagamenti : System.Web.UI.UserControl
	{
        SqlCommand cmdMyCommand;
		#region Web Form definition
		private static readonly ILog Log = LogManager.GetLogger(typeof(WucGestionePagamenti));
		//public OSAPConst.OperationType OpType;
		private int _IdPagamento;
		private int _IdContribuente;

		public OSAPConst.OperationType OpType;
		protected System.Web.UI.WebControls.Button btnRibalta;
		protected System.Web.UI.WebControls.Button btnRibaltaAnagAnater;

		//private OPENUtility.CreateSessione WFSessione;
		protected System.Web.UI.WebControls.TextBox c;
		
		protected string displayCartellazione = "";
		protected string displayDataEntry= "none";
		#endregion

		#region Property
				
		/// <summary>
		/// 1 = Visualizzazione
		/// 2 = Modifica
		/// </summary>
		public int IdContribuente
		{
			get
			{
				return _IdContribuente;
			}
			set
			{
				_IdContribuente = value;
			}
		}
		
		public int IdPagamento
		{
			get
			{
				return _IdPagamento;
			}
			set
			{
				_IdPagamento = value;
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
            try {
                //Log.Debug("WUCPag::Load::hdIdContribuente::" + hdIdContribuente.Value);
                // Carico il pagamento
                if (this._IdContribuente > 0 && this._IdPagamento > 0)
                {
                    LoadPagamentoForEdit(IdContribuente, IdPagamento);
                    if (OpType == OSAPConst.OperationType.VIEW)
                    {
                        txtDataPag.Enabled = false;
                        txtDataAccreditoDE.Enabled = false;
                        txtNRata.Enabled = false;
                        txtImportoPag.Enabled = false;
                    }
                }
                else {
                    //*** 201504 - Nuova Gestione anagrafica con form unico ***
                    if (DichiarazioneSession.HasPlainAnag)
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + this._IdContribuente + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString());
                }

                // Put user code to initialize the page here
                if (IsPostBack == false)
                {
                    if (this._IdContribuente <= 0 && this._IdPagamento <= 0)
                        rdbDaCartellazione.Checked = true;
                }

                SetPanelVisibility();
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                string Str = "<script language='javascript'>";
                if (DichiarazioneSession.HasPlainAnag)
                    Str += "document.getElementById('TRSpecAnag').style.display='none';";
                else
                    Str += "document.getElementById('TRPlainAnag').style.display='none';";
                Str += "</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "shpa", Str);
            }
            catch (Exception Err)
            {
               Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.Page_Load.errore: ", Err);
               Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public void SetPanelVisibility()
		{
            try {
                // Modifica pagamento
                if (this._IdContribuente > 0 && this._IdPagamento > 0)
                {
                    rdbDataEntry.Checked = true;
                    rdbDataEntry.Enabled = false;
                    rdbDaCartellazione.Enabled = false;

                    displayCartellazione = "none";
                    displayDataEntry = "";

                }
                // Inserimento
                else
                {
                    if (IsPostBack == false)
                    {
                        rdbDaCartellazione.Checked = true;
                        displayDataEntry = "none";
                        displayCartellazione = "";
                    }
                    else
                    {
                        if (rdbDaCartellazione.Checked == true)
                        {
                            displayDataEntry = "none";
                            displayCartellazione = "";
                        }
                        else
                        {
                            displayDataEntry = "";
                            displayCartellazione = "none";
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.SetPanelVisibility.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

            //			if (IsPostBack == false)
            //			{
            //				if (this._IdContribuente > 0 && this._IdPagamento > 0)
            //				{
            //					rdbDataEntry.Checked = true;
            //
            //					rdbDataEntry.Enabled = false;
            //					rdbDaCartellazione.Enabled = false;
            //
            //					displayCartellazione = "none";
            //					displayDataEntry = "";
            //				}
            //				else
            //				{
            //					rdbDaCartellazione.Checked = true;
            //					displayDataEntry = "none";
            //					displayCartellazione = "";
            //				}
            //			}
            //			else
            //			{
            //				if (rdbDaCartellazione.Checked == true)
            //				{
            //					displayDataEntry = "none";
            //					displayCartellazione = "";
            //				}
            //				else
            //				{
            //					displayDataEntry = "";
            //					displayCartellazione = "none";
            //				}
            //				
            //			}
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="idContribuente"></param>
		/// <param name="idPagamento"></param>
		/// <returns></returns>
		public int LoadPagamentoForEdit(int idContribuente, int idPagamento)
		{
            try {
                if (idContribuente > 0 && idPagamento > 0)
                {

                    TxtAnno.Enabled = false;
                    txtNAvvisoDE.Enabled = false;

                    PagamentoExt item = MetodiPagamento.GetPagamentoById(IdPagamento);
                    if (item != null)
                    {
                        txtNAvvisoDE.Text = item.CodiceCartella;
                        TxtAnno.Text = item.Anno;

                        if (item.DataPagamento > DateTime.MinValue && item.DataPagamento < DateTime.MaxValue)
                            txtDataPag.Text = item.DataPagamento.ToShortDateString();

                        if (item.DataAccredito > DateTime.MinValue && item.DataAccredito < DateTime.MaxValue)
                            txtDataAccreditoDE.Text = item.DataAccredito.ToShortDateString();

                        if (item.NumeroRataString != "")
                            txtNRata.Text = item.NumeroRataString; // item.DataAccredito.ToShortDateString();

                        if (item.ImportoPagato > 0)
                            txtImportoPag.Text = string.Format("{0:0.00}", item.ImportoPagato);

                        TxtIdPagamento.Text = item.IdPagamento.ToString();
                        TxtImportoPagatoUpd.Text = string.Format("{0:0.00}", item.ImportoPagato);
                        TxtCodiceCartellaUpd.Text = item.CodiceCartella;

                        // Carico Anagrafica 
                        //Dati Anagrafici
                        DAO.AnagraficheDAO angraficaDAO = new AnagraficheDAO();
                        DettaglioAnagrafica anagraficaContribuente = angraficaDAO.GetAnagraficaContribuente(IdContribuente);//, DichiarazioneSession.CodTributo("")
                        RibaltaAnagrafica(anagraficaContribuente);
                    }
                    else
                    {
                        string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile recuperare il pagamento.');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
                    }
                }

                return 0;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.LoadPagamentoForEdit.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }


		/// <summary>
		/// Set mandatory form fields
		/// </summary>
		/// <returns></returns>
		public string[] GetMandatoryFields()
		{

			string[] returnValue = new string[2];

			string clientControlsID = "";
			string clientMandatoryMessagge = "";

			clientControlsID = this.TxtAnno.ClientID + ";";  
			clientControlsID += this.txtNAvvisoDE.ClientID + ";";  
			clientControlsID += this.txtDataPag.ClientID + ";";   
			clientControlsID += this.txtImportoPag.ClientID + ";";  
			//clientControlsID += this.hdIdContribuente.ClientID;  
			
			clientMandatoryMessagge = " - Anno" + ";";  
			clientMandatoryMessagge += " - N. Avviso" + ";";  
			clientMandatoryMessagge += " - Data Pagamento" + ";";
			clientMandatoryMessagge += " - Importo Pagato" + ";";
			//clientMandatoryMessagge += " - Contribuente";
		
			returnValue[0] = clientControlsID;
			returnValue[1] = clientMandatoryMessagge;

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void LnkAnagTributi_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
		{
			try
			{
                connessioneDB();
                //WFSessione = new OPENUtility.CreateSessione(System.Convert.ToString(Session["PARAMETROENV"]), System.Convert.ToString(Session["username"]), System.Convert.ToString(Session["IDENTIFICATIVOAPPLICAZIONE"]));
                //if (! WFSessione.CreaSessione((Session["username"].ToString()), ref WFErrore))
                //{
                //    throw (new Exception("Errore durante l\'apertura della sessione di WorkFlow"));
                //}
				
				if (ViewState["sessionName"] != null)
					Session.Remove(ViewState["sessionName"].ToString());
				
                //Anagrafica.DLL.GestioneAnagrafica oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, DichiarazioneSession.ApplAnagrafica);
                //Anagrafica.DLL.GestioneAnagrafica oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica();
				DettaglioAnagrafica oDettaglioAnagrafica = new DettaglioAnagrafica();
                oDettaglioAnagrafica.COD_CONTRIBUENTE = this._IdContribuente;//int.Parse(hdIdContribuente.Value);
				oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = int.Parse(TxtIdDataAnagrafica.Text);
				ViewState["sessionName"] = "codContribuente";
				Session[ViewState["sessionName"].ToString ()] = oDettaglioAnagrafica;
				writeJavascriptAnagrafica(ViewState["sessionName"].ToString());
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

            finally
			{
                //if (WFSessione != null)
                //{
                //    WFSessione.Kill();
                //    WFSessione = null;
                //}
                if (cmdMyCommand != null)
                {
                    cmdMyCommand.Connection.Close();
                    cmdMyCommand = null;
                }
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public void connessioneDB()
        {
            //*** 20140409
            try {
                cmdMyCommand = new SqlCommand();
                Log.Debug("WucGestionePagamenti::connessioneDB::apertura della connessione al DB");

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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.connessioneDB.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void LnkPulisciContr_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect (OSAPPages.GestionePagamentiAdd);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nomeSessione"></param>
		private void writeJavascriptAnagrafica(string nomeSessione)
		{
			string sScript;
			
			sScript = "ApriRicercaAnagrafe(\'" + nomeSessione + "\');";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
		}

		/// <summary>
		/// 
		/// </summary>
		public void RibaltaAnagrafica()
		{
			RibaltaAnagrafica(null);
		}

		/// <summary>
		/// Copia l'anagrafica sul form di ricarca
		/// </summary>
        public void RibaltaAnagrafica(DettaglioAnagrafica anagrInput)
        {
            DettaglioAnagrafica oDettaglioAnagrafica = null;
            try
            {
                Log.Debug("inizio ribalta");
                if (anagrInput == null)
                {
                    if (ViewState["sessionName"] != null || DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente != null)
                    {
                        if (ViewState["sessionName"] != null)
                            oDettaglioAnagrafica = (DettaglioAnagrafica)Session[ViewState["sessionName"].ToString()];
                        else
                            oDettaglioAnagrafica = DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente;
                    }
                }
                else
                    oDettaglioAnagrafica = anagrInput;

                if (oDettaglioAnagrafica != null)
                {
                    Log.Debug("ho anagrafica");
                    //hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                    this._IdContribuente = oDettaglioAnagrafica.COD_CONTRIBUENTE;
                    TxtIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA.ToString();
                    //*** 201504 - Nuova Gestione anagrafica con form unico ***
                    if (DichiarazioneSession.HasPlainAnag)
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + this._IdContribuente + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                    else
                    {
                        //Session["oAnagrafe"] = oDettaglioAnagrafica;
                        TxtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale;
                        TxtPIva.Text = oDettaglioAnagrafica.PartitaIva;
                        TxtCognome.Text = oDettaglioAnagrafica.Cognome;
                        TxtNome.Text = oDettaglioAnagrafica.Nome;
                        switch (oDettaglioAnagrafica.Sesso)
                        {
                            case "F":
                                F.Checked = true;
                                break;
                            case "G":
                                G.Checked = true;
                                break;
                            case "M":
                                M.Checked = true;
                                break;
                        }
                        TxtDataNascita.Text = oDettaglioAnagrafica.DataNascita;
                        TxtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita;
                        TxtResVia.Text = oDettaglioAnagrafica.ViaResidenza;
                        TxtResCivico.Text = oDettaglioAnagrafica.CivicoResidenza;
                        TxtResEsponente.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;
                        TxtResInterno.Text = oDettaglioAnagrafica.InternoCivicoResidenza;
                        TxtResScala.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;
                        TxtResCAP.Text = oDettaglioAnagrafica.CapResidenza;
                        TxtResComune.Text = oDettaglioAnagrafica.ComuneResidenza;
                        TxtResPv.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                    }
                    if (ViewState["sessionName"] != null)
                        Session.Remove(ViewState["sessionName"].ToString());
                    DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente = oDettaglioAnagrafica;
                    //objDichiarazione.TestataDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione; 
                    //objDichiarazione.ArticoliDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione; 
                    //DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
                    Log.Debug("prendo rate");
                    GetRateData();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.RibaltaAnagrafica.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		/// <summary>
		/// Cerca le rate
		/// </summary>
		public void GetRateData()
		{
            Log.Debug("WUCPag::GetRateData::hdIdContribuente::" + this._IdContribuente);
            if (this._IdPagamento <= 0)//if (this._IdContribuente <= 0 && this._IdPagamento <= 0)
            {
                Log.Debug("GetRateData:: no contrib no pag");
				try
				{
					if (
                        (!SharedFunction.IsNullOrEmpty(this._IdContribuente) && (this._IdContribuente>0)) 
						//&& (!SharedFunction.IsNullOrEmpty(TxtIdDataAnagrafica.Text) && ((TxtIdDataAnagrafica.Text != "-1") && (TxtIdDataAnagrafica.Text != "0")))
						)
					{
						rdbDataEntry.Checked = false;
						rdbDaCartellazione.Checked = true;	
						displayCartellazione = "";
						displayDataEntry = "none";

						RataExt[] rate = new RataExt[0];
						RataSearch SearchParams = new RataSearch();
						SearchParams.IdEnte = (string)DichiarazioneSession.IdEnte;
                        SearchParams.IdTributo = DichiarazioneSession.CodTributo("");
                        SearchParams.IdContribuente = this._IdContribuente;
		
						if (!SharedFunction.IsNullOrEmpty(txtAnnoVersamento.Text))
							SearchParams.Anno = int.Parse(txtAnnoVersamento.Text);

						if (!SharedFunction.IsNullOrEmpty(txtNAvviso.Text))
							SearchParams.CodiceCartella = txtNAvviso.Text;

						rate = MetodiRata.GetRate(SearchParams);	
					
						DataBindGridRate(rate, 0, true);
					}
					else
					{
						string sScript = "GestAlert('a', 'warning', '', '', Devi selezionare un contribuente');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
					}
				}
                catch (Exception Err)
                {
                    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.GetRateData.errore: ", Err);
                    Response.Redirect("../../PaginaErrore.aspx");
                }
            }
		}

		/// <summary>
		/// 
		/// </summary>
		public int UpdatePagamento()
		{
            try {
                int esitoIns = -1;
                string messaggioErr = "";
                string sScript = string.Empty;

                lblRisultato.Visible = false;

                // Dati da salvare
                PagamentoExt objToInsertOrUpdate = VerificaDatiUpdate(ref messaggioErr);


                if (objToInsertOrUpdate != null)
                {
                    // Modifico
                    esitoIns = MetodiPagamento.InsertUpdatePagamento(objToInsertOrUpdate, (string)HttpContext.Current.Session["username"]);

                    if (esitoIns < 0)
                        sScript = "GestAlert('a', 'danger', '', '', Si è verificato un errore.');";
                    else
                        sScript = "GestAlert('a', 'success', '', '', 'Pagamento modificato correttamente');";
                }
                else
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Dati non validi: " + messaggioErr + "');";
                }

                // Redirect su pagica ricerca
                if (esitoIns >= 0)
                    sScript = sScript + "location.href = '" + OSAPPages.GestionePagamentiSearch + "?NewSearch=false'";

                // Pubblic gli script
                if (sScript != "")
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");

                return esitoIns;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.UpdatePagamento.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }


		/// <summary>
		/// 
		/// </summary>
		public void SavePagamento()
		{
			int esitoIns = -1;
			string messaggioErr = "";
			string sScript = string.Empty; 

			try
			{
				if (rdbDataEntry.Checked == true)
				{
					lblRisultato.Visible = false;

					// Dati da salvare
					PagamentoExt objToInsertOrUpdate = VerificaDatiDataEntry(ref messaggioErr);

					if (objToInsertOrUpdate != null)
					{
						esitoIns = MetodiPagamento.InsertUpdatePagamento(objToInsertOrUpdate, (string)HttpContext.Current.Session["username"]);
						if (esitoIns < 0)
						{
							sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un errore.');";
						}
						else
						{
							sScript = "GestAlert('a', 'success', '', '', 'Pagamento memorizzato correttamente');";
						}
					}
					else
						sScript = "GestAlert('a', 'warning', '', '', 'Dati non validi: " + messaggioErr + "');";
				}

				if (rdbDaCartellazione.Checked == true)
				{
					PagamentoExt[] newList = VerificaDatiDaCartellazione(ref messaggioErr);
					if (newList != null && newList.Length > 0)
					{
						if (!SharedFunction.IsNullOrEmpty(messaggioErr))
							sScript = "GestAlert('a', 'warning', '', '', '" + messaggioErr + "');";
						else
						{
                            foreach (PagamentoExt myRow in newList)
                            {
                                esitoIns = MetodiPagamento.InsertUpdatePagamento(myRow, (string)HttpContext.Current.Session["username"]);
								if (esitoIns < 0)
								{
									sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un errore.');";
									Log.Warn("Si è verificato un errore in SavePagamento inserendo un pagamento con i seguenti parametri-> Data Pagamento: " + myRow.DataPagamento.ToString() +
										"; Data Acrredito: " + myRow.DataAccredito + "; " + " Importo Pagato: " + myRow.ImportoPagato);
									break;
								}
								else
								{
									sScript = "GestAlert('a', 'success', '', '', 'agamento memorizzato correttamente');";									
								}
							}
						}
					}
					else
						sScript = "GestAlert('a', 'warning', '', '', 'Devi selezionare un versamento');";
				}				
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.SavePagamento.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

            if (esitoIns >= 0)
				sScript = sScript + "location.href = '" + OSAPPages.GestionePagamentiSearch + "?NewSearch=false'";

			if (sScript != "")
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messaggioErr"></param>
		/// <returns></returns>
		private PagamentoExt[] VerificaDatiDaCartellazione(ref string messaggioErr)
		{
            try {
                // Dati					
                if (DichiarazioneSession.IdEnte == null)
                {
                    messaggioErr = "Sessione scaduta. Chiudere il browser e rieffetuare il login.";
                    return new PagamentoExt[0];
                }

                int counter = 0;
                PagamentoExt[] newList = new PagamentoExt[0];

                if (GrdRate.Rows != null)
                {
                    foreach (GridViewRow myRow in GrdRate.Rows)
                    {
                        CheckBox cb = (CheckBox)myRow.Cells[0].FindControl("ckbSelezione");

                        if (cb != null && cb.Checked == true)
                            counter++;
                    }

                    newList = new PagamentoExt[counter];

                    counter = 0;
                    foreach (GridViewRow myRow in GrdRate.Rows)
                    {
                        // Selezione
                        CheckBox cb = (CheckBox)myRow.Cells[0].FindControl("ckbSelezione");

                        TextBox txtDataPagamento = (TextBox)myRow.Cells[6].FindControl("txtDataPagamento");
                        TextBox txtDataAccredito = (TextBox)myRow.Cells[7].FindControl("txtDataAccredito");
                        TextBox txtTotalePagamento = (TextBox)myRow.Cells[8].FindControl("txtTotalePagamento");

                        // Input rata nasdcosti
                        TextBox txtCodiceCartellaRow = (TextBox)myRow.Cells[9].FindControl("txtCodiceCartellaRow");
                        TextBox txtNumRataRow = (TextBox)myRow.Cells[9].FindControl("txtNumRataRow");
                        TextBox txtAnnoRow = (TextBox)myRow.Cells[9].FindControl("txtAnnoRow");
                        
                        if (cb != null && txtDataPagamento != null &&
                            txtDataAccredito != null && txtTotalePagamento != null)
                        {
                        }
                        else
                        {
                            messaggioErr = "Si è verificato un errore. Dati si sistema mancanti.";
                            break;
                        }

                        if (cb.Checked == true)
                        {
                            bool esitoItem = true;
                            PagamentoExt objToInsertOrUpdate = new PagamentoExt();

                            // Ente
                            objToInsertOrUpdate.IdEnte = DichiarazioneSession.IdEnte.ToString();

                            // Anagrafica
                            if (!SharedFunction.IsNullOrEmpty(this._IdContribuente) &&
                                ((this._IdContribuente > 0)))
                            {
                                int tmpImpAn = 0;
                                try
                                {
                                    tmpImpAn = this._IdContribuente;
                                    objToInsertOrUpdate.CodContribuente = tmpImpAn;
                                }
                                catch
                                {
                                    messaggioErr = "Contribuente non valido.";
                                    break;
                                }
                            }
                            else
                            {
                                messaggioErr = "Seleziona un contribuente.";
                                break;
                            }

                            // Dati rata
                            objToInsertOrUpdate.CodiceCartella = txtCodiceCartellaRow.Text;
                            objToInsertOrUpdate.NumeroRataString = txtNumRataRow.Text;
                            objToInsertOrUpdate.Anno = txtAnnoRow.Text;

                            // Data Pagamento
                            DateTime tmpDataPagam = DateTime.MinValue;
                            if (!SharedFunction.IsNullOrEmpty(txtDataPagamento.Text))
                            {
                                try
                                {
                                    tmpDataPagam = DateTime.ParseExact(txtDataPagamento.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    messaggioErr = "Data Pagamento non valida.";
                                    break;
                                }
                                objToInsertOrUpdate.DataPagamento = tmpDataPagam;
                            }
                            else
                            {
                                messaggioErr = "Campo \\'Data Pagamento\\' obbligatorio";
                                break;
                            }


                            // Data Pagamento
                            DateTime tmpDataAccr = DateTime.MinValue;
                            if (!SharedFunction.IsNullOrEmpty(txtDataAccredito.Text))
                            {
                                try
                                {
                                    tmpDataAccr = DateTime.ParseExact(txtDataAccredito.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    messaggioErr = "Data Accredito non valida.";
                                    break;
                                }
                                objToInsertOrUpdate.DataAccredito = tmpDataAccr;
                            }

                            if (tmpDataAccr > DateTime.MinValue && tmpDataPagam > DateTime.MinValue &&
                                tmpDataAccr > DateTime.MaxValue && tmpDataPagam > DateTime.MaxValue)
                            {
                                if (tmpDataAccr < tmpDataPagam)
                                {
                                    messaggioErr = "La data di Accredito deve essere maggiore di quella di Pagamento.";
                                    break;
                                }
                            }

                            if (!SharedFunction.IsNullOrEmpty(txtTotalePagamento.Text))
                            {
                                double tmpImpPag = 0;
                                try
                                {
                                    Log.Debug("txtTotalePagamento::" + txtTotalePagamento.Text);
                                    tmpImpPag = double.Parse(txtTotalePagamento.Text);
                                    Log.Debug("convertito in::" + tmpImpPag.ToString());
                                    if (tmpImpPag <= 0)
                                    {
                                        messaggioErr = "L\\'Importo pagato\\' deve essere maggiore di zero.";
                                        break;
                                    }
                                    else
                                    {
                                        Log.Debug("prelevo cartella");
                                        Cartella cartellabyCod = MetodiCartella.GetCartellaByCodiceCartella(objToInsertOrUpdate.CodiceCartella,
                                            objToInsertOrUpdate.CodContribuente, objToInsertOrUpdate.IdEnte);
                                        Log.Debug("prelevato " + cartellabyCod.ToString());
                                        if (cartellabyCod != null)
                                        {
                                            Log.Debug("trovata");
                                            double giaPagato = DTO.MetodiPagamento.GetTotalePagatoPerCatella(cartellabyCod.IdEnte,
                                                objToInsertOrUpdate.CodContribuente, cartellabyCod.CodiceCartella);

                                            if (giaPagato > 0)
                                            {
                                                if ((cartellabyCod.ImportoCarico - giaPagato) < tmpImpPag)
                                                {
                                                    messaggioErr = "Sono già presenti delle rate per questa cartella. La somma delle " +
                                                        " rate più l\\'importo inserito supera l\\'importo della cartella. E\\' possibile " +
                                                        " inserire un importo non maggiore di " + string.Format("{0:0.00}", (cartellabyCod.ImportoCarico - giaPagato));
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (tmpImpPag > cartellabyCod.ImportoCarico)
                                                {
                                                    messaggioErr = "L'importo inserito non può essere maggiore di " + cartellabyCod.ImportoCarico.ToString();
                                                    return null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            messaggioErr = "Impossibile recuperare i dati della cartella.";
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    messaggioErr = "Importo pagato non valido.";
                                    break;
                                }

                                objToInsertOrUpdate.ImportoPagato = tmpImpPag;
                            }
                            else
                            {
                                messaggioErr = "Campo \\'Importo pagato\\' obbligatorio";
                                break;
                            }

                            if (esitoItem == true)
                            {
                                newList[counter] = objToInsertOrUpdate;
                                counter++;
                            }
                        }
                    }
                }

                //			if (esitoVerifCampi == false)
                //				newList = new PagamentoExt[0];

                return newList;
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.VerificaDatiDaCartellazione.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messaggioErr"></param>
		/// <returns></returns>
		private PagamentoExt VerificaDatiUpdate(ref string messaggioErr)
		{
            
			messaggioErr = "";
			PagamentoExt objToInsertOrUpdate = new PagamentoExt();
            try {

                try
                {
                    objToInsertOrUpdate.IdPagamento = int.Parse(TxtIdPagamento.Text);
                }
                catch
                {
                    messaggioErr = "Errore del sistema, IdPagamento mancante.";
                    return null;
                }

                // Dati finti non necessari al'update
                objToInsertOrUpdate.CodContribuente = int.Parse(hdIdContribuente.Value);
                objToInsertOrUpdate.IdDataAnagrafica = int.Parse(TxtIdDataAnagrafica.Text);
                objToInsertOrUpdate.IdEnte = (string)DichiarazioneSession.IdEnte;
                objToInsertOrUpdate.CodiceCartella = TxtCodiceCartellaUpd.Text;

                // NUMERO RATA
                if (!SharedFunction.IsNullOrEmpty(txtNRata.Text))
                    objToInsertOrUpdate.NumeroRataString = txtNRata.Text;

                // DATA VERSAMENTO
                DateTime tmpDataPagam = DateTime.MinValue;
                if (!SharedFunction.IsNullOrEmpty(txtDataPag.Text))
                {
                    try
                    {
                        tmpDataPagam = DateTime.ParseExact(txtDataPag.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        messaggioErr = "Data Pagamento non valida.";
                        return null;
                    }
                    objToInsertOrUpdate.DataPagamento = tmpDataPagam;
                }
                else
                {
                    messaggioErr = "Campo \\'Data Pagamento\\' obbligatorio";
                    return null;
                }

                // DATA ACCREDITO
                DateTime tmpDataAccr = DateTime.MinValue;
                if (!SharedFunction.IsNullOrEmpty(txtDataAccreditoDE.Text))
                {
                    try
                    {
                        tmpDataAccr = DateTime.ParseExact(txtDataAccreditoDE.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        messaggioErr = "Data Riversamento non valida.";
                        return null;
                    }
                    objToInsertOrUpdate.DataAccredito = tmpDataAccr;
                }

                // VERIFICA data accredito maggiore data versamento
                if (tmpDataAccr > DateTime.MinValue && tmpDataPagam > DateTime.MinValue &&
                    tmpDataAccr < DateTime.MaxValue && tmpDataPagam < DateTime.MaxValue)
                {
                    if (tmpDataAccr < tmpDataPagam)
                    {
                        messaggioErr = "La data di Accredito deve essere uguale o maggiore della data di Pagamento.";
                        return null;
                    }
                }

                // IMPORTO PAGATO
                if (!SharedFunction.IsNullOrEmpty(txtImportoPag.Text))
                {
                    double tmpImpPag = 0;
                    try
                    {
                        tmpImpPag = double.Parse(txtImportoPag.Text);

                        if (tmpImpPag <= 0)
                        {
                            messaggioErr = "L\\'Importo pagato\\' deve essere maggiore di zero.";
                            return null;
                        }
                        else
                        {
                            Cartella cartellabyCod = cartellabyCod = MetodiCartella.GetCartellaByCodiceCartella(txtNAvvisoDE.Text, objToInsertOrUpdate.CodContribuente, objToInsertOrUpdate.IdEnte);
                            if (cartellabyCod != null)
                            {
                                double giaPagato = DTO.MetodiPagamento.GetTotalePagatoPerCatella(cartellabyCod.IdEnte,
                                    objToInsertOrUpdate.CodContribuente, cartellabyCod.CodiceCartella);

                                if (giaPagato > 0)
                                {
                                    double oldPagamento = 0;
                                    try
                                    {
                                        oldPagamento = double.Parse(TxtImportoPagatoUpd.Text);
                                    }
                                    catch {
                                        messaggioErr = "Errore di sistema. Impossibile recuperare i dati del pagamento.";
                                        return null;
                                    }

                                    if (tmpImpPag > (cartellabyCod.ImportoCarico - (giaPagato - oldPagamento)))
                                    {
                                        messaggioErr = "Sono già presenti delle rate per questa cartella. La somma delle " +
                                            " rate più l\\'importo inserito supera l\\'importo della cartella. E\\' possibile " +
                                            " inserire un importo non maggiore di € " + string.Format("{0:0.00}", (cartellabyCod.ImportoCarico - (giaPagato - oldPagamento)));
                                        txtImportoPag.Text = string.Format("{0:0.00}", (cartellabyCod.ImportoCarico - (giaPagato - oldPagamento)));
                                        return null;
                                    }
                                }
                                // Caso che non dovrebbe verificarsi
                                else
                                {
                                    if (tmpImpPag > cartellabyCod.ImportoCarico)
                                    {
                                        messaggioErr = "L'importo inserito non può essere maggiore di " + cartellabyCod.ImportoCarico.ToString();
                                        return null;
                                    }
                                }
                            }
                            else
                            {
                                messaggioErr = "Impossibile recuperare i dati della cartella.";
                                return null;
                            }

                            objToInsertOrUpdate.ImportoPagato = tmpImpPag;
                        }
                    }
                    catch
                    {
                        messaggioErr = "Importo pagato non valido.";
                        return null;
                    }
                }
                else
                {
                    messaggioErr = "Campo \\'Importo pagato\\' è obbligatorio";
                    return null;
                }

                return objToInsertOrUpdate;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.VerificaDatiUpdate.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messaggioErr"></param>
		/// <returns></returns>
		private PagamentoExt VerificaDatiDataEntry(ref string messaggioErr)
		{
			messaggioErr = "";
			PagamentoExt objToInsertOrUpdate = new PagamentoExt();
            try {

                // Dati					
                if (DichiarazioneSession.IdEnte == null)
                {
                    messaggioErr = "Sessione scaduta. Chiudere il browser e rieffetuare il login.";
                    return null;
                }

                objToInsertOrUpdate.IdEnte = (string)DichiarazioneSession.IdEnte;

                // Anagrafica
                if (!SharedFunction.IsNullOrEmpty(this._IdContribuente) &&
                    ((this._IdContribuente > 0)))
                {
                    int tmpImpAn = 0;
                    try
                    {
                        tmpImpAn = this._IdContribuente;
                    }
                    catch { tmpImpAn = 0; }
                    objToInsertOrUpdate.CodContribuente = tmpImpAn;
                }
                else
                {
                    messaggioErr = "Seleziona un contribuente.";
                    return null;
                }
                //// CodControbuente
                //if (!SharedFunction.IsNullOrEmpty(hdIdContribuente.Value) && 
                //    ((hdIdContribuente.Value != "-1") && (hdIdContribuente.Value != "0")))
                //{
                //    int tmpImpCC = 0;
                //    try
                //    {
                //        tmpImpCC = int.Parse(hdIdContribuente.Value);;							
                //    } 
                //    catch { tmpImpCC = 0; }
                //    objToInsertOrUpdate.CodContribuente = tmpImpCC;
                //}
                //else
                //{
                //    messaggioErr = "Seleziona un contribuente.";
                //    return null;
                //}

                // In questo caso può essere valorizzato o no
                if (!SharedFunction.IsNullOrEmpty(TxtIdPagamento.Text))
                {
                    int tmpIdPagam = -1;
                    try
                    {
                        tmpIdPagam = int.Parse(TxtIdPagamento.Text); ;
                    }
                    catch
                    {
                        messaggioErr = "IdPagamento non valido.";
                        return null;
                    }
                    objToInsertOrUpdate.IdPagamento = tmpIdPagam;
                }
                else
                    objToInsertOrUpdate.IdPagamento = -1;

                // Anno
                if (!SharedFunction.IsNullOrEmpty(TxtAnno.Text))
                    objToInsertOrUpdate.Anno = TxtAnno.Text;
                else
                {
                    messaggioErr = "Campo \\'Anno\\' obbligatorio";
                    return null;
                }

                // Controlli sul codice cartella
                Cartella cartellabyCod = null;
                if (!SharedFunction.IsNullOrEmpty(txtNAvvisoDE.Text))
                {
                    // Recupero la cartella
                    cartellabyCod = MetodiCartella.GetCartellaByCodiceCartella(txtNAvvisoDE.Text, objToInsertOrUpdate.CodContribuente, objToInsertOrUpdate.IdEnte);
                    if (cartellabyCod == null)
                    {
                        messaggioErr = "Numero avviso errato, insesistente o associato ad un altro contribuente.";
                        return null;
                    }
                    else
                    {
                        if (!SharedFunction.IsNullOrEmpty(objToInsertOrUpdate.Anno))
                        {
                            // Controllo che l'anno inserito sia uguale a quello dul DB
                            if (objToInsertOrUpdate.Anno != cartellabyCod.Anno.ToString())
                            {
                                messaggioErr = "L'anno inserito è differente da quello già presente per la cartalla in questione. Verrà automaticamente sostiuito.";
                                objToInsertOrUpdate.Anno = cartellabyCod.Anno.ToString();
                                TxtAnno.Text = cartellabyCod.Anno.ToString();
                                return null;
                            }
                        }
                        objToInsertOrUpdate.CodiceCartella = txtNAvvisoDE.Text;
                    }
                }
                else
                {
                    messaggioErr = "Campo \\'Numero avviso\\' obbligatorio";
                    return null;
                }

                if (!SharedFunction.IsNullOrEmpty(txtNRata.Text))
                    objToInsertOrUpdate.NumeroRataString = txtNRata.Text;

                DateTime tmpDataPagam = DateTime.MinValue;
                if (!SharedFunction.IsNullOrEmpty(txtDataPag.Text))
                {
                    try
                    {
                        tmpDataPagam = DateTime.ParseExact(txtDataPag.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        messaggioErr = "Data Pagamento non valida.";
                        return null;
                    }
                    objToInsertOrUpdate.DataPagamento = tmpDataPagam;
                }
                else
                {
                    messaggioErr = "Campo \\'Data Pagamento\\' obbligatorio";
                    return null;
                }

                DateTime tmpDataAccr = DateTime.MinValue;
                if (!SharedFunction.IsNullOrEmpty(txtDataAccreditoDE.Text))
                {
                    try
                    {
                        tmpDataAccr = DateTime.ParseExact(txtDataAccreditoDE.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        messaggioErr = "Data Riversamento non valida.";
                        return null;
                    }
                    objToInsertOrUpdate.DataAccredito = tmpDataAccr;
                }

                if (tmpDataAccr > DateTime.MinValue && tmpDataPagam > DateTime.MinValue &&
                    tmpDataAccr < DateTime.MaxValue && tmpDataPagam < DateTime.MaxValue)
                {
                    if (tmpDataAccr < tmpDataPagam)
                    {
                        messaggioErr = "La data di Accredito deve essere uguale o maggiore della data di Pagamento.";
                        return null;
                    }
                }

                if (!SharedFunction.IsNullOrEmpty(txtImportoPag.Text))
                {
                    double tmpImpPag = 0;
                    try
                    {
                        tmpImpPag = double.Parse(txtImportoPag.Text);

                        if (tmpImpPag <= 0)
                        {
                            messaggioErr = "L\\'Importo pagato\\' deve essere maggiore di zero.";
                            return null;
                        }
                        else
                        {
                            if (cartellabyCod != null)
                            {
                                double giaPagato = DTO.MetodiPagamento.GetTotalePagatoPerCatella(cartellabyCod.IdEnte,
                                    objToInsertOrUpdate.CodContribuente, cartellabyCod.CodiceCartella);

                                if (giaPagato > 0)
                                {
                                    if ((cartellabyCod.ImportoCarico - giaPagato) < tmpImpPag)
                                    {
                                        messaggioErr = "Sono già presenti delle rate per questa cartella. La somma delle " +
                                            " rate più l\\'importo inserito supera l\\'importo della cartella. E\\' possibile " +
                                            " inserire un importo non maggiore di € " + string.Format("{0:0.00}", (cartellabyCod.ImportoCarico - giaPagato));
                                        txtImportoPag.Text = string.Format("{0:0.00}", (cartellabyCod.ImportoCarico - giaPagato));
                                        return null;
                                    }
                                }
                                else
                                {
                                    if (tmpImpPag > cartellabyCod.ImportoCarico)
                                    {
                                        messaggioErr = "L'importo inserito non può essere maggiore di " + string.Format("{0:0.00}", cartellabyCod.ImportoCarico);
                                        return null;
                                    }
                                }
                            }

                            objToInsertOrUpdate.ImportoPagato = tmpImpPag;
                        }
                    }
                    catch
                    {
                        messaggioErr = "Importo pagato non valido.";
                        return null;
                    }
                }
                else
                {
                    messaggioErr = "Campo \\'Importo pagato\\' è obbligatorio";
                    return null;
                }

                return objToInsertOrUpdate;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.VerificaDatiDataEntry.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

		/// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="StartIndex"></param>
        /// <param name="bRebind"></param>
		private void DataBindGridRate (RataExt[] rate, int StartIndex, bool bRebind)
		{
            try {
                if (rate != null && rate.Length > 0)
                {
                    GrdRate.DataSource = rate;
                    if (bRebind)
                    {
                        GrdRate.DataBind();
                    }
                    GrdRate.Visible = true;
                    //LblResultPagamenti.Visible = false;
                }
                else
                {
                    //LblResultPagamenti.Visible = true;
                    GrdRate.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.DataBlindGridRate.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tDataGrd"></param>
		/// <returns></returns>
		public string FormattaDataGrd(DateTime tDataGrd)
		{
            try {
                if (tDataGrd == DateTime.MinValue || tDataGrd == DateTime.MaxValue)
                    return String.Empty;
                else
                    return tDataGrd.ToShortDateString();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.FormattaDataGrd.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GrdRate_OnItemDataBound(object sender, DataGridItemEventArgs e)
		{
            try {
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    RataExt rata = e.Item.DataItem as RataExt;

                    if (rata != null)
                    {
                        CheckBox cbFinded = (CheckBox)e.Item.FindControl("ckbSelezione");
                        TextBox tbDPagam = (TextBox)e.Item.FindControl("txtDataPagamento");
                        TextBox tbDAccre = (TextBox)e.Item.FindControl("txtDataAccredito");
                        TextBox cbPagam = (TextBox)e.Item.FindControl("txtTotalePagamento");
                        TextBox txtTotalePagamento = (TextBox)e.Item.FindControl("txtTotalePagamento");

                        double giaPagato = DTO.MetodiPagamento.GetTotalePagatoPerCatella(rata.IdEnte, rata.IdContribuente, rata.CodiceCartella);
                        Cartella cartellabyCod = MetodiCartella.GetCartellaByCodiceCartella(rata.CodiceCartella, rata.IdContribuente, rata.IdEnte);

                        // PAgamento disabilitato perché già efettuato
                        if (rata.DataPagamento > DateTime.MinValue && rata.ImportoPagato > 0)
                        {
                            txtTotalePagamento.Text = string.Format("{0:0.00}", rata.ImportoPagato);

                            if (cbFinded != null)
                                cbFinded.Enabled = false;

                            if (tbDPagam != null)
                                tbDPagam.Enabled = false;

                            if (tbDAccre != null)
                                tbDAccre.Enabled = false;

                            if (cbPagam != null)
                                cbPagam.Enabled = false;
                        }
                        else
                        {
                            if (txtTotalePagamento != null)
                            {
                                // Rata che ha già superato o raggiunto il limite
                                if (cartellabyCod.ImportoTotale <= giaPagato)
                                {
                                    txtTotalePagamento.Text = string.Format("{0:0.00}", giaPagato);
                                    e.Item.CssClass = "impSuperato";

                                    if (cbFinded != null)
                                        cbFinded.Enabled = false;

                                    if (tbDPagam != null)
                                        tbDPagam.Enabled = false;

                                    if (tbDAccre != null)
                                        tbDAccre.Enabled = false;

                                    if (cbPagam != null)
                                        cbPagam.Enabled = false;
                                }
                                else //Rata pagabile
                                {
                                    if ((cartellabyCod.ImportoTotale - giaPagato) < rata.ImportoRata)
                                        txtTotalePagamento.Text = string.Format("{0:0.00}", (cartellabyCod.ImportoTotale - giaPagato));
                                    else
                                        txtTotalePagamento.Text = string.Format("{0:0.00}", (rata.ImportoRata - giaPagato));
                                    ////Segnalo che c'è stata una variazione
                                    //if (giaPagato > 0)
                                    //    e.Item.Cells[8].CssClass = "impSuperatoCellImporto";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucGestionePagamenti.GrdRate_OnItemDataBound.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
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

		}
		#endregion
	}
}
