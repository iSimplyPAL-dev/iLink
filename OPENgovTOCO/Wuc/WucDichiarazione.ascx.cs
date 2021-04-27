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
using System.Data.SqlClient;

namespace OPENgovTOCO.Wuc
{
    /// <summary>
    ///	Usercontrol per la gestione Dichiarazioni.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Analisi eventi</em>
    /// </revision>
    /// </revisionHistory>
    public partial class WucDichiarazione : System.Web.UI.UserControl
	{
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
            //this.GrdArticoli.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdArticoli_ItemCommand);
            //this.GrdArticoli.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdArticoli_ItemDataBound);

		}
		#endregion

		private static readonly ILog Log = LogManager.GetLogger(typeof(WucDichiarazione));
		public OSAPConst.OperationType OpType;
		private int _idDichiarazione;
		private int _idContribuente;
		//private DichiarazioneTosapCosap objDichiarazione;
        //private OPENUtility.CreateSessione WFSessione;
        //private string WFErrore;
        SqlCommand cmdMyCommand;

		#region Property
		
		public int IdContribuente
		{
			get
			{
				return _idContribuente;
			}
			set
			{
				_idContribuente = value;
			}
		}
		
		public int IdDichiarazione
		{
			get
			{
				return _idDichiarazione;
			}
			set
			{
				_idDichiarazione = value;
			}
		}
		
		#endregion
		
		#region Event
		/// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(System.Object sender, System.EventArgs e)
		{
			//Put user code to initialize the page here
			
			//objDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap;
            try
            {
                if (!Page.IsPostBack)
                {
                    //objDichiarazione.TestataDichiarazione.IdDichiarazione = - 1;
                    BindData();
                    if (this.OpType == OSAPConst.OperationType.VIEW)
                    {
                        Wuc.WucDichiarazione wuc = (Wuc.WucDichiarazione)Page.FindControl("wucDichiarazione");
                        SharedFunction.ChangeControlStatus(false, wuc);
                    }
                }
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

        }
			/// <summary>
            /// Gestione inserimento nuovo o modifica di una posizione
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
		protected void lnkGestImmobili_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
		{
            try {
                UpdateSessionObject();
                if (this.OpType == OSAPConst.OperationType.ADD)
                {
                    string sScript;
                    sScript = "parent.Visualizza.location.href='ArticoliAdd.aspx';";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "NewUI", "<script language=\'javascript\'>" + sScript + "</script>");
                    //Response.Redirect(OSAPPages.ArticoliAdd);
                }
                else if (this.OpType == OSAPConst.OperationType.EDIT)
                {
                    string sScript;
                    sScript = "parent.Visualizza.location.href='ArticoliAdd.aspx?AddFromEdit=true';";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AddFromEdit", "<script language=\'javascript\'>" + sScript + "</script>");
                    //Response.Redirect(OSAPPages.ArticoliAdd + "?AddFromEdit=true");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.InkGestImmobili_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
		/// <summary>
        /// Gestione anagrafica
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
				
                //Anagrafica.DLL.GestioneAnagrafica oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, DichiarazioneSession.ApplAnagrafica );
                //Anagrafica.DLL.GestioneAnagrafica oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica();
				DettaglioAnagrafica oDettaglioAnagrafica = new DettaglioAnagrafica();
                oDettaglioAnagrafica.COD_CONTRIBUENTE = int.Parse(hdIdContribuente.Value);
				oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = int.Parse(TxtIdDataAnagrafica.Text);
				ViewState["sessionName"] = "codContribuente";
				Session[ViewState["sessionName"].ToString ()] = oDettaglioAnagrafica;
				writeJavascriptAnagrafica(ViewState["sessionName"].ToString());
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.LnkAnagTributi_Click.errore: ", Err);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkPulisciContr_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect(OSAPPages.DichiarazioniAdd);
        }

        #region "Griglie"
        protected void GrdRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (this.OpType == OSAPConst.OperationType.VIEW && e.Row.RowType == DataControlRowType.Header)
                {
                    checked { for (int i = 0; i < this.GrdArticoli.Columns.Count; i++)
                    {
                        if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("D") == 0)
                        {
                            this.GrdArticoli.Columns[i].Visible = false;
                                                        }
                            if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("E") == 0)
                            {
                                this.GrdArticoli.Columns[i].Visible = false;
                            }
                        }
                    }
                }
                else if (this.OpType == OSAPConst.OperationType.EDIT && e.Row.RowType == DataControlRowType.Header)
                {
                    checked { for (int i = 0; i < this.GrdArticoli.Columns.Count; i++)
                        {
                            if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("V") == 0)
                            {
                                this.GrdArticoli.Columns[i].Visible = false;
                            }
                            if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("E") == 0)
                            {
                                this.GrdArticoli.Columns[i].Visible = true;
                            }
                        }
                    }
                }
                if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
                {
                    ImageButton deleteButton = (ImageButton)e.Row.FindControl("imgDelete");
                    deleteButton.Attributes.Add("onclick", "javascript:return confirm('Eliminare Articolo dalla dichiarazione?');");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.GrdRowDataBound.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                        string sScript;
                string IDRow = e.CommandArgument.ToString();
                switch (((ImageButton)e.CommandSource).CommandName)
                {
                    case "Canc":
                        Articolo[] arrayArticoli = MetodiArticolo.RemoveArticolo(int.Parse(IDRow), DichiarazioneSession.IdEnte);
                        DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione = arrayArticoli;
                        UpdateSessionObject();
                        BindData();
                        break;
                    case "Open":
                        sScript = "parent.Visualizza.location.href='"+ OSAPPages.ArticoliView + "?IdArticolo=" + IDRow + "';";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddFromEdit", "<script language=\'javascript\'>" + sScript + "</script>");
                        //Response.Redirect(OSAPPages.ArticoliView + "?IdArticolo=" + IDRow);
                        break;
                    case "Mod":// "Edit":
                        sScript = "parent.Visualizza.location.href='" + OSAPPages.ArticoliEdit + "?IdArticolo=" + IDRow + "';";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AddFromEdit", "<script language=\'javascript\'>" + sScript + "</script>");
                        //Response.Redirect(OSAPPages.ArticoliEdit + "?IdArticolo=" + IDRow);
                        break;
                    default:
                        // Do nothing.
                        break;
                }
                                    }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.GrdRowCommand.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //      protected void GrdArticoli_ItemCommand(Object sender, DataGridCommandEventArgs e)
        //{
        //try
        //	switch(((ImageButton)e.CommandSource).CommandName)
        //	{

        //		case "Delete":
        //			DeleteItem(e);
        //			break;

        //			// Add other cases here, if there are multiple ButtonColumns in 
        //			// the DataGrid control.

        //		case "View":
        //			ViewItem(e);
        //			break;

        //		case "Edit":
        //			EditItem(e);
        //			break;

        //		default:
        //			// Do nothing.
        //			break;

        //	}
        //}
        //     catch (Exception Err)
        //   {
        //     Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.GrdArticoli_ItemCommand.errore: ", Err);
        //      Response.Redirect("../../PaginaErrore.aspx");
        //  }
        //}


        //private void GrdArticoli_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
        //  try{
        //	if (this.OpType == OSAPConst.OperationType.VIEW && 	e.Item.ItemType == ListItemType.Header)

        //		for (int i = 0; i < this.GrdArticoli.Columns.Count; i++)
        //		{
        //			if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("D")==0)
        //			{
        //				this.GrdArticoli.Columns[i].Visible = false;

        //			}
        //			if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("E")==0)
        //			{
        //				this.GrdArticoli.Columns[i].Visible = false;

        //			}
        //		}
        //	else if (this.OpType == OSAPConst.OperationType.EDIT && e.Item.ItemType == ListItemType.Header)

        //		for (int i = 0; i < this.GrdArticoli.Columns.Count; i++)
        //		{
        //			if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("V")==0)
        //			{
        //				this.GrdArticoli.Columns[i].Visible = false;
        //			}
        //			if (this.GrdArticoli.Columns[i].HeaderText.CompareTo("E")==0)
        //			{
        //				this.GrdArticoli.Columns[i].Visible = true;
        //			}

        //		}


        //	if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //	{
        //		ImageButton deleteButton = (ImageButton)e.Item.FindControl("imgDelete");
        //		deleteButton.Attributes.Add("onclick", "javascript:return confirm('Eliminare Articolo dalla dichiarazione?');");
        //	}
        //   }
        // catch (Exception Err)
        //     {
        //       Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.GrdArticoli_ItemDataBound.errore: ", Err);
        //      Response.Redirect("../../PaginaErrore.aspx");
        //   }

        //}
        #endregion

        #endregion

        #region Public Method
        /// <summary>
        /// Salvataggio delle modifiche apportate alla dichiarazione
        /// </summary>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public void SalvaDichiarazione()
		{
            string sScript = string.Empty;

            try
            {
                UpdateSessionObject();

                if (OpType == OSAPConst.OperationType.ADD)
                {
                    MetodiDichiarazioneTosapCosap.SetDichiarazione(DichiarazioneSession.StringConnection,DichiarazioneSession.SessionDichiarazioneTosapCosap);
                    sScript = "GestAlert('a', 'success', '', '', 'Dichiarazione memorizzata correttamente');";
                }
                else if (OpType == OSAPConst.OperationType.EDIT)
                {
                    DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now;
                    MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap);

                    DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.MinValue;
                    MetodiDichiarazioneTosapCosap.SetDichiarazione(DichiarazioneSession.StringConnection,DichiarazioneSession.SessionDichiarazioneTosapCosap);
                    sScript = "GestAlert('a', 'success', '', '', 'Dichiarazione modificata correttamente');";
                }
                sScript += "location.href = '" + OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo("") + "';";
                //Response.Redirect(OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo(""), false);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.SalvaDichiarazione.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            if (sScript!=string.Empty)
                Page.ClientScript.RegisterStartupScript(this.GetType(),"", "<script language=\'javascript\'>" + sScript + "</script>");
		}
        //public void SalvaDichiarazione()
        //{
        //    string sScript = string.Empty;

        //    try
        //    {
        //        UpdateSessionObject();

        //        if (OpType == OSAPConst.OperationType.ADD)
        //        {
        //            MetodiDichiarazioneTosapCosap.SetDichiarazione(DichiarazioneSession.StringConnection, DichiarazioneSession.SessionDichiarazioneTosapCosap);
        //            sScript = "GestAlert('a', 'success', '', '', 'Dichiarazione memorizzata correttamente');";
        //        }
        //        else if (OpType == OSAPConst.OperationType.EDIT)
        //        {
        //            DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now;
        //            MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap);

        //            DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.MinValue;
        //            MetodiDichiarazioneTosapCosap.SetDichiarazione(DichiarazioneSession.StringConnection, DichiarazioneSession.SessionDichiarazioneTosapCosap);
        //            sScript = "GestAlert('a', 'success', '', '', 'Dichiarazione modificata correttamente');";
        //        }
        //        sScript += "location.href = '" + OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo("") + "';";
        //        //Response.Redirect(OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo(""), false);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.SalvaDichiarazione.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //    if (sScript != string.Empty)
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
        //}
        /// <summary>
        /// Cancellazione dichiarazione
        /// </summary>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public void EliminaDichiarazione()
		{
            try
            {
                string sScript = string.Empty;
                Log.Debug("WucDichiarazione::EliminaDichiarazione::prelevo i dati dalla memoria");
                UpdateSessionObject();
                Log.Debug("WucDichiarazione::EliminaDichiarazione::setto data di variazione e cessazione");
                DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now;
                DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataCessazione = DateTime.Now;
                MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap);
                new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, DichiarazioneSession.sOperatore, new  Utility.Costanti.LogEventArgument().Immobile, "EliminaDichiarazione", Utility.Costanti.AZIONE_DELETE.ToString(), DichiarazioneSession.CodTributo(""), DichiarazioneSession.IdEnte, DichiarazioneSession.SessionDichiarazioneTosapCosap.IdDichiarazione);
                sScript = "GestAlert('a', 'success', '', '', 'Dichiarazione eliminata correttamente')";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.EliminaDichiarazione.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //public void EliminaDichiarazione()
        //{
        //    try
        //    {
        //        string sScript = string.Empty;
        //        Log.Debug("WucDichiarazione::EliminaDichiarazione::prelevo i dati dalla memoria");
        //        UpdateSessionObject();
        //        Log.Debug("WucDichiarazione::EliminaDichiarazione::setto data di variazione e cessazione");
        //        DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now;
        //        DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataCessazione = DateTime.Now;
        //        MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap);
        //        sScript = "GestAlert('a', 'success', '', '', 'Dichiarazione eliminata correttamente')";
        //        //sScript+=";location.href='DichiarazioniSearch.aspx?NewSearch=true'";
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.EliminaDichiarazione.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //}
        /// <summary>
        /// funzione per il popolamento dell'anagrafica selezionata
        /// </summary>
        public void RibaltaAnagrafica()
		{
			DettaglioAnagrafica oDettaglioAnagrafica = new DettaglioAnagrafica();
			try
			{
                if (ViewState["Contribuente"] == null)
                {
                    Log.Debug("Session contribuente è nulla");
                }
                if (ViewState["sessionName"] != null || DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente != null)
                {
                    if (ViewState["sessionName"] != null)
                    {
                        oDettaglioAnagrafica = (DettaglioAnagrafica)Session[ViewState["sessionName"].ToString()];
                        Log.Debug("Valorizzo oDettaglioAnagrafica quando sessione non è nulla");
                    }
                    else
                    {
                        oDettaglioAnagrafica = DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente;
                        Log.Debug("Valorizzo oDettaglioAnagrafica quando sessione è nulla");
                    }
                    hdIdContribuente.Value= oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                    TxtIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA.ToString();
                    //*** 201504 - Nuova Gestione anagrafica con form unico ***
                    if (DichiarazioneSession.HasPlainAnag)
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                    else
                    {
                        //Session["oAnagrafe"] = oDettaglioAnagrafica;
                    Log.Debug(oDettaglioAnagrafica.CodiceFiscale.ToString());
                    TxtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale;

                    Log.Debug(oDettaglioAnagrafica.PartitaIva.ToString());
                    TxtPIva.Text = oDettaglioAnagrafica.PartitaIva;

                    Log.Debug(oDettaglioAnagrafica.Cognome.ToString());
                    TxtCognome.Text = oDettaglioAnagrafica.Cognome;

                    Log.Debug(oDettaglioAnagrafica.Nome.ToString());
                    TxtNome.Text = oDettaglioAnagrafica.Nome;

                    Log.Debug(oDettaglioAnagrafica.Sesso.ToString());
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

                    Log.Debug(oDettaglioAnagrafica.DataNascita.ToString());
                    TxtDataNascita.Text = oDettaglioAnagrafica.DataNascita;

                    Log.Debug(oDettaglioAnagrafica.ComuneNascita.ToString());
                    TxtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita;

                    Log.Debug(oDettaglioAnagrafica.ViaResidenza.ToString());
                    TxtResVia.Text = oDettaglioAnagrafica.ViaResidenza;

                    Log.Debug(oDettaglioAnagrafica.CivicoResidenza.ToString());
                    TxtResCivico.Text = oDettaglioAnagrafica.CivicoResidenza;

                    Log.Debug(oDettaglioAnagrafica.EsponenteCivicoResidenza.ToString());
                    TxtResEsponente.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;

                    Log.Debug(oDettaglioAnagrafica.InternoCivicoResidenza.ToString());
                    TxtResInterno.Text = oDettaglioAnagrafica.InternoCivicoResidenza;

                    Log.Debug(oDettaglioAnagrafica.ScalaCivicoResidenza.ToString());
                    TxtResScala.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;

                    Log.Debug(oDettaglioAnagrafica.CapResidenza.ToString());
                    TxtResCAP.Text = oDettaglioAnagrafica.CapResidenza;

                    Log.Debug(oDettaglioAnagrafica.ComuneResidenza.ToString());
                    TxtResComune.Text = oDettaglioAnagrafica.ComuneResidenza;

                    Log.Debug(oDettaglioAnagrafica.ProvinciaResidenza.ToString());
                    TxtResPv.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                }
                    if (ViewState["sessionName"] != null)
                        Session.Remove(ViewState["sessionName"].ToString());

                    DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente = oDettaglioAnagrafica;
                    //objDichiarazione.TestataDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione; 
                    //objDichiarazione.ArticoliDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione; 
                    //DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
                }
                else
                {
                    if(ViewState["sessionName"]==null)
                    {
                        Log.Debug("Sessione è nulla");
                    }
                    if (DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente == null)
                    {
                        Log.Debug("AnagraficaContribuente è nulla");
                    }

                }
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.RibaltaAnagrafica.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		/// <summary>
        /// Set mandatory form fields
        /// </summary>
        
		public string[] GetMandatoryFields()
		{
			string[] returnValue = new string[2];

			string clientControlsID = "";
			string clientMandatoryMessagge = "";

			clientControlsID = this.TxtDataDichiarazione.ClientID + ";";  
			clientControlsID += this.TxtNDichiarazione.ClientID + ";";  
			clientControlsID += this.cmbTipoAtto.ClientID + ";"; 
			clientControlsID += this.cmbTitoloRichiedente.ClientID + ";";
			clientControlsID += this.cmbUffici.ClientID + ";";
			clientControlsID += this.hdIdContribuente.ClientID+ ";";
			clientControlsID += this.txtNumeroArticoli.ClientID;
			
			clientMandatoryMessagge = " - Data Dichiarazione" + ";";  
			clientMandatoryMessagge += " - Numero Dichiarazione" + ";";  
			clientMandatoryMessagge += " - Tipo Atto" + ";"; 
			clientMandatoryMessagge += " - Titolo Richiedente" + ";";
			clientMandatoryMessagge += " - Ufficio" + ";";
			clientMandatoryMessagge += " - Contribuente" + ";";
			clientMandatoryMessagge += " - Articoli";
		
			returnValue[0] = clientControlsID;
			returnValue[1] = clientMandatoryMessagge;

			return returnValue;
		}

        public void connessioneDB()
        {
            //*** 20140409
            try {
                cmdMyCommand = new SqlCommand();
                Log.Debug("WucDichiarazione::connessioneDB::apertura della connessione al DB");

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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.connessioneDB.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		
		#endregion
		
		#region Private Method
		/// <summary>
        /// popolamento della videata con i dati dell'oggetto
        /// </summary>
		private void BindData()
		{
            try {
                //Bind DropDown Uffici
                cmbUffici.DataSource = MetodiUffici.GetUffici();
                cmbUffici.DataValueField = "IdUfficio";
                cmbUffici.DataTextField = "Descrizione";
                cmbUffici.DataBind();
                cmbUffici.SelectedValue = "-1";

                //Bind DropDown Tipologie Occupazioni
                cmbTitoloRichiedente.DataSource = MetodiTitoloRichiedente.GetTitoloRichiedente();
                cmbTitoloRichiedente.DataValueField = "IdTitoloRichiedente";
                cmbTitoloRichiedente.DataTextField = "Descrizione";
                cmbTitoloRichiedente.DataBind();
                cmbTitoloRichiedente.SelectedValue = "-1";

                if (DichiarazioneSession.SessionDichiarazioneTosapCosap != null)
                {
                    GrdArticoli.DataSource = DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione;
                    GrdArticoli.DataBind();

                    if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione != null &&
                        DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione.Length > 0)
                    {
                        txtNumeroArticoli.Text = DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione.Length.ToString();
                        LblResultImmobili.Visible = false;
                    }
                    else
                    {
                        GrdArticoli.Visible = false;
                        LblResultImmobili.Visible = true;
                    }

                    if (DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione != null)
                    {
                        if (DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataDichiarazione != DateTime.MinValue)
                            TxtDataDichiarazione.Text = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataDichiarazione.ToString("dd/MM/yyyy");
                        TxtNDichiarazione.Text = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.NDichiarazione;
                        TxtNoteDich.Text = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.NoteDichiarazione;
                        cmbTipoAtto.SelectedValue = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.IdTipoAtto.ToString();
                        cmbTitoloRichiedente.SelectedValue = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.TitoloRichiedente.IdTitoloRichiedente.ToString();
                        cmbUffici.SelectedValue = DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.Ufficio.IdUfficio.ToString();
                        RibaltaAnagrafica();
                    }
                    else
                        //*** 201504 - Nuova Gestione anagrafica con form unico ***
                        if (DichiarazioneSession.HasPlainAnag)
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString());
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.BindData.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

        }
        /// <summary>
        /// aggiornamento dell'oggetto in sessione
        /// </summary>
		private void UpdateSessionObject()
		{
            try
            {
                //Testata Dichiarazione
                DichiarazioneTosapCosap objDichiarazione = new DichiarazioneTosapCosap();

                objDichiarazione.IdEnte = DichiarazioneSession.IdEnte;

                Log.Debug("Wuc::WucDichiarazione::UpdateSessionObject:titolo richiedente");
                TitoloRichiedente objTitoloRichiedente = new TitoloRichiedente();
                objTitoloRichiedente.IdTitoloRichiedente = int.Parse(cmbTitoloRichiedente.SelectedValue);
                objTitoloRichiedente.Descrizione = cmbTitoloRichiedente.SelectedItem.Text;

                Log.Debug("Wuc::WucDichiarazione::UpdateSessionObject:ufficio");
                Uffici objUfficio = new Uffici();
                objUfficio.IdUfficio = int.Parse(cmbUffici.SelectedValue);
                objUfficio.Descrizione = cmbUffici.SelectedItem.Text;

                Log.Debug("Wuc::WucDichiarazione::UpdateSessionObject:dati testata");
                DichiarazioneTosapCosapTestata objDichiarazioneTestata = new DichiarazioneTosapCosapTestata();
                if (TxtDataDichiarazione.Text != "")
                    objDichiarazioneTestata.DataDichiarazione = DateTime.Parse(TxtDataDichiarazione.Text);
                objDichiarazioneTestata.NDichiarazione = TxtNDichiarazione.Text;
                objDichiarazioneTestata.NoteDichiarazione = TxtNoteDich.Text;
                objDichiarazioneTestata.TitoloRichiedente = objTitoloRichiedente;
                objDichiarazioneTestata.IdTipoAtto = int.Parse(cmbTipoAtto.SelectedValue);
                objDichiarazioneTestata.Ufficio = objUfficio;
                objDichiarazioneTestata.Operatore = DichiarazioneSession.sOperatore;
                objDichiarazioneTestata.DataInserimento = DateTime.Now;
                objDichiarazione.TestataDichiarazione = objDichiarazioneTestata;

                //Salvo anagrafica dichiarazione
                Log.Debug("Wuc::WucDichiarazione::UpdateSessionObject::salvo anagrafica dichiarazione");
                if (DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente != null)
                    objDichiarazione.AnagraficaContribuente = DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente;

                //Salvo articoli dichiarazione
                Log.Debug("Wuc::WucDichiarazione::UpdateSessionObject::salvo articoli dichiarazione");
                if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione != null)
                    objDichiarazione.ArticoliDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione;

                if (DichiarazioneSession.SessionDichiarazioneTosapCosap.IdDichiarazione > 0)
                    objDichiarazione.IdDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.IdDichiarazione;
                DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.UpdateSessionObject.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// eliminazione di un immobile dalla dichiarazione
        /// </summary>
        /// <param name="e"></param>
		private void DeleteItem(DataGridCommandEventArgs e)
		{
            try {
                TableCell itemCell = SharedFunction.CellByName(e.Item, "IdArticolo");
                int itemValue = int.Parse(itemCell.Text);

                // e.Item is the table row where the command is raised. For bound
                // columns, the value is stored in the Text property of a TableCell.

                Articolo[] arrayArticoli = MetodiArticolo.RemoveArticolo(itemValue, DichiarazioneSession.IdEnte);
                DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione = arrayArticoli;
                UpdateSessionObject();
                BindData();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.DeleteItem.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private void ViewItem(DataGridCommandEventArgs e)
		{
            try {

                TableCell itemCell = SharedFunction.CellByName(e.Item, "IdArticolo");
                int itemValue = int.Parse(itemCell.Text);

                Response.Redirect(OSAPPages.ArticoliView + "?IdArticolo=" + itemValue);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.ViewItem.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

        }

		private void EditItem(DataGridCommandEventArgs e)
		{
            try {
                TableCell itemCell = SharedFunction.CellByName(e.Item, "IdArticolo");
                int itemValue = int.Parse(itemCell.Text);

                Response.Redirect(OSAPPages.ArticoliEdit + "?IdArticolo=" + itemValue);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.EditItem.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

        }
		

		private void writeJavascriptAnagrafica(string nomeSessione)
		{
            try {
                string sScript;

                sScript = "ApriRicercaAnagrafe(\'" + nomeSessione + "\');" + "\r\n";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucDichiarazione.writeJavascriptAnagrafica.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
				

		#endregion


	}
	
}


		
