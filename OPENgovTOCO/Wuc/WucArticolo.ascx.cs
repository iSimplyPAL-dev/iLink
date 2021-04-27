using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Configuration;
using DTO;
using log4net;
using IRemInterfaceOSAP;
using System.Data.SqlClient;

namespace OPENgovTOCO.Wuc
{
    /// <summary>
    ///	Usercontrol per la gestione Articolo.
    /// </summary>
    public partial class WucArticolo : System.Web.UI.UserControl
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
            this.Load += new System.EventHandler(this.Page_Load);
            //*** 20130801 - accertamento OSAP ***
            this.cmbTipoDurata.SelectedIndexChanged += new System.EventHandler(this.cmbTipoDurata_SelectedIndexChanged);
            //*** ***
        }
        #endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof(WucArticolo));
        public OSAPConst.OperationType OpType;
        public DichiarazioneTosapCosap objDichiarazione;

        private WucDatiContribuente wucContribuente;
        private int _idArticolo;

        //*** 20140410
        SqlCommand cmdMyCommand;
        DataTable dtMyDati;
        SqlTransaction startTransazione;
        SqlConnection conn;

        public int IdArticolo
        {
            get { return _idArticolo; }
            set { _idArticolo = value; }
        }
        //*** 20130610 - ruolo supplettivo ***
        //*** ***
        /// <summary>
        /// caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                //Put user code to initialize the page here
                SetEvent();
                if (TxtViaRibaltata.Text != "")
                    TxtVia.Text = TxtViaRibaltata.Text;

                objDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap;
                if (!(Page.IsPostBack))
                {
                    BindData();
                    if (this.OpType == OSAPConst.OperationType.VIEW)
                    {
                        Wuc.WucArticolo wuc = (Wuc.WucArticolo)Page.FindControl("wucArticolo");
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

        private void SetEvent()
        {
            //LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidEse()");
            //LnkDelRid.Attributes.Add("OnClick", "DeleteRidEse(\'R\')");
            //LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidEse('" & ObjCodDescr.TIPO_ESENZIONI & "')")
            //LnkDelDet.Attributes.Add("OnClick", "return DeleteMaggiorazioni('" & txtMaggiorazioni.ClientID & "')")
            string UrlStradario = DichiarazioneSession.UrlStradario;
            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario(\'" + UrlStradario + "\',\'RibaltaStrada\', \'" + DichiarazioneSession.IdEnte + "\')");
        }
        /// <summary>
        /// funzione di popolamento dati della videata da oggetto
        /// </summary>
        public void BindData()
        {
            try
            {
                Log.Debug("WUCArticolo::BindData::si entra");
                Log.Debug("WUCArticolo::BindData::inizio con hdIdContribuente=" + hdIdContribuente.Value);
                objDichiarazione = new DichiarazioneTosapCosap();

                wucContribuente = (WucDatiContribuente)(this.FindControl("wucContribuente"));
                if (wucContribuente != null)
                {
                    Log.Debug("WUCArticolo::BindData::ho wuccontribuente");
                    if (objDichiarazione.AnagraficaContribuente != null)
                    {
                        Log.Debug("WUCArticolo::BindData::ho anagrafica contribuente");
                        wucContribuente.oAnagrafica = objDichiarazione.AnagraficaContribuente;
                    }
                    else
                    {
                        if (DichiarazioneSession.SessionDichiarazioneTosapCosap != null)
                        {
                            Log.Debug("WUCArticolo::BindData::altro caso");
                            wucContribuente.oAnagrafica = DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente;
                        }
                        else {
                            return;
                        }
                    }
                }
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                hdIdContribuente.Value = wucContribuente.oAnagrafica.COD_CONTRIBUENTE.ToString();
                if (DichiarazioneSession.HasPlainAnag)
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                Log.Debug("WUCArticolo::BindData::hdIdContribuente=" + hdIdContribuente.Value);
                //Bind DropDown Categorie
                cmbCategoria.SelectedValue = "-1";
                cmbCategoria.DataSource = MetodiCategorie.GetCategorie(DichiarazioneSession.StringConnection, -1, true, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                cmbCategoria.DataValueField = "IdCategoria";
                cmbCategoria.DataTextField = "Descrizione";
                cmbCategoria.DataBind();

                //Bind DropDown Tipologie Occupazioni
                cmbTipologiaOccupazione.SelectedValue = "-1";
                cmbTipologiaOccupazione.DataSource = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                //cmbTipologiaOccupazione.DataSource = MetodiTipologieOccupazioni.GetTipologieOccupazioni(-1, true, DichiarazioneSession.IdEnte);
                cmbTipologiaOccupazione.DataValueField = "IdTipologiaOccupazione";
                cmbTipologiaOccupazione.DataTextField = "Descrizione";
                cmbTipologiaOccupazione.DataBind();

                //Bind DropDown Tipi consistenza
                cmbTipoConsistenza.SelectedValue = "-1";
                cmbTipoConsistenza.DataSource = MetodiTipoConsistenza.GetTipiConsistenza(DichiarazioneSession.StringConnection, true);
                cmbTipoConsistenza.DataValueField = "IdTipoConsistenza";
                cmbTipoConsistenza.DataTextField = "Descrizione";
                cmbTipoConsistenza.DataBind();

                //Bind DropDown Durata
                cmbTipoDurata.SelectedValue = "-1";
                cmbTipoDurata.DataSource = MetodiDurata.GetDurate(DichiarazioneSession.StringConnection, true);
                cmbTipoDurata.DataValueField = "IdDurata";
                cmbTipoDurata.DataTextField = "Descrizione";
                cmbTipoDurata.DataBind();

                if (this.OpType == OSAPConst.OperationType.VIEW || this.OpType == OSAPConst.OperationType.EDIT)
                {
                    foreach (Articolo o in DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione)
                    {
                        if (o.IdArticolo == this.IdArticolo)
                        {
                            Session["Agevolazioni"] = MetodiAgevolazione.GetAgevolazioni("", o.IdArticolo, DichiarazioneSession.IdEnte);
                            GrdAgevolazioni.DataSource = Session["Agevolazioni"];
                            GrdAgevolazioni.DataBind();

                            this.TxtCivico.Text = (o.Civico == -1) ? String.Empty : o.Civico.ToString();
                            this.TxtCodVia.Text = o.CodVia.ToString();
                            this.txtConsistenza.Text = o.Consistenza.ToString();
                            this.TxtDataInizio.Text = o.DataInizioOccupazione.ToString("dd/MM/yyyy");
                            this.txtDataFine.Text = o.DataFineOccupazione.ToString("dd/MM/yyyy");
                            this.txtDetrazioni.Text = o.DetrazioneImporto.ToString();
                            this.txtDurata.Text = o.DurataOccupazione.ToString();
                            this.TxtEsponente.Text = o.Esponente;
                            this.TxtInterno.Text = o.Interno;
                            this.txtMaggiorazioni.Text = o.MaggiorazioneImporto.ToString();
                            this.txtMaggiorazioniPerc.Text = o.MaggiorazionePerc.ToString();
                            this.TxtNoteUI.Text = o.Note;
                            this.TxtScala.Text = o.Scala;
                            this.TxtVia.Text = o.SVia;
                            this.cmbCategoria.SelectedValue = o.Categoria.IdCategoria.ToString();
                            this.cmbTipoConsistenza.SelectedValue = o.TipoConsistenzaTOCO.IdTipoConsistenza.ToString();
                            this.cmbTipoDurata.SelectedValue = o.TipoDurata.IdDurata.ToString();
                            this.cmbTipologiaOccupazione.SelectedValue = o.TipologiaOccupazione.IdTipologiaOccupazione.ToString();
                            this.chkAttrazione.Checked = bool.Parse(o.Attrazione.ToString());
                            //*** 20130610 - ruolo supplettivo ***
                            this.TxtIdArticoloPadre.Text = o.IdArticoloPadre.ToString();
                            //*** ***
                            //*** 20130801 - accertamento OSAP ***
                            if (o.IdTributo.ToString() == Utility.Costanti.TRIBUTO_OccupazionePermanente)
                            {
                                this.TxtTributo.Text = "OCCUP.PERMANENTE";
                            }
                            else
                            {
                                this.TxtTributo.Text = "OCCUP.TEMPORANEA";
                            }
                            //*** ***
                        }
                    }
                }
                else
                {
                    Session["Agevolazioni"] = MetodiAgevolazione.GetAgevolazioni("", -1, DichiarazioneSession.IdEnte);
                    GrdAgevolazioni.DataSource = Session["Agevolazioni"];
                    GrdAgevolazioni.DataBind();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.BindData.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }


        #region Event Handler
        //*** 20130801 - accertamento OSAP ***
        protected void cmbTipoDurata_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbTipoDurata.SelectedValue.ToString() == "1" || cmbTipoDurata.SelectedValue.ToString() == "2")
                {
                    this.TxtTributo.Text = "OCCUP.TEMPORANEA";
                }
                else
                {
                    this.TxtTributo.Text = "OCCUP.PERMANENTE";
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.cmbTipoDurata_SelectedIndexChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }
        //*** ***


        private void btnUpdate_Click(System.Object sender, System.EventArgs e)
        {

            /*
            try{
            if (!System.Convert.ToBoolean(Session["oDatiRid"] == null))
			{
				GrdRiduzioni.Style.Add("display", "");
				GrdRiduzioni.DataSource = Session["oDatiRid"];
				GrdRiduzioni.start_index = GrdRiduzioni.CurrentPageIndex.ToString();
				//GrdRiduzioni.SelectedIndex = -1
				GrdRiduzioni.DataBind();
				LblResultRid.Style.Add("display", "none");
			}
			else
			{
				GrdRiduzioni.Style.Add("display", "none");
				LblResultRid.Style.Add("display", "");
			}
            }
              catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.btnUpdate_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
            */

        }




        #endregion

        #region Private Method
        /// <summary>
        /// Funzione di salvataggio dati videata
        /// </summary>
        /// <revisionHistory>
        /// <revision date="29/04/2019">
        /// 22/PO/19 - Sistemazione per refresh dopo salvataggio
        /// </revision>
        /// </revisionHistory>
        public void SalvaArticolo()
        {
            int nRet = -1;
            string sScript = "";

            try
            {
                if (OpType == OSAPConst.OperationType.ADD || OpType == OSAPConst.OperationType.ADDFROMEDIT)
                {
                    if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione == null)
                    {
                        Articolo[] arrayArticolo = new Articolo[1];
                        arrayArticolo[0] = SetArticoliObject();
                        //Usato per eliminare gli articoli;
                        objDichiarazione.ArticoliDichiarazione = arrayArticolo;
                    }
                    else
                    {
                        objDichiarazione.ArticoliDichiarazione = GetMyArray();
                    }

                    DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;

                    if (OpType == OSAPConst.OperationType.ADD)
                    {
                        sScript = "location.href = '" + OSAPPages.DichiarazioniAdd + "?FromArticoli=true';";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
                    }
                    else if (OpType == OSAPConst.OperationType.ADDFROMEDIT)
                    {
                        DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now;
                        MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap);

                        DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.MinValue;

                        sScript = "GestAlert('a', 'success', '', '', 'Nuovo Articolo aggiunto correttamente alla dichiarazione');location.href = '" + OSAPPages.DichiarazioniEdit + "?FromArticoli=true';";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
                    }
                }
                else if (OpType == OSAPConst.OperationType.EDIT)
                {
                    DAO.ArticoliDAO FncArticolo = new DAO.ArticoliDAO();
                    Articolo[] arrayArticolo = new Articolo[1];
                    arrayArticolo[0] = SetArticoliObject();
                    //Usato per eliminare gli articoli;
                    arrayArticolo[0].IdArticolo = this.IdArticolo;
                    foreach (Articolo myRow in DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione)
                    {
                        if (myRow.IdArticolo == this.IdArticolo)
                        {
                            Articolo myItem = myRow;
                            conn = new SqlConnection(DichiarazioneSession.StringConnection);
                            conn.Open();
                            connessioneDB();
                            startTransazione = conn.BeginTransaction();

                            //Salva Dichiarazione e modifica articolo
                            myRow.DataVariazione = DateTime.Now;
                            nRet = FncArticolo.UpdateArticolo(ref cmdMyCommand, myRow);
                            if (nRet < 1)
                            {
                                startTransazione.Rollback();
                                cmdMyCommand.Connection.Close();
                                break;
                            }
                            myItem = arrayArticolo[0];
                            myRow.DataVariazione = DateTime.MinValue;
                            nRet = FncArticolo.SetArticolo(ref cmdMyCommand, myItem);
                            if (nRet < 1)
                            {
                                startTransazione.Rollback();
                                cmdMyCommand.Connection.Close();
                                break;
                            }
                            startTransazione.Commit();
                            cmdMyCommand.Connection.Close();
                            break;
                        }
                    }
                    if (nRet > 1)
                    {
                     objDichiarazione.ArticoliDichiarazione = GetMyArray();
                    DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
                       sScript = "GestAlert('a', 'success', '', '', 'Articolo modificato correttamente');";
                        sScript += "location.href = '" + OSAPPages.DichiarazioniEdit + "?FromArticoli=true';";
                    }
                    else
                    {
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore durante la modifica');";
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.SalvaArticolo.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }
       // public void SalvaArticolo()
       // {
       //     int nRet = -1;
       //     string sScript = "";

       //     try
       //     {
       //         if (OpType == OSAPConst.OperationType.ADD || OpType == OSAPConst.OperationType.ADDFROMEDIT)
       //         {
       //             if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione == null)
       //             {
       //                 Articolo[] arrayArticolo = new Articolo[1];
       //                 arrayArticolo[0] = SetArticoliObject();
       //                 //Usato per eliminare gli articoli;
       //                 objDichiarazione.ArticoliDichiarazione = arrayArticolo;
       //             }
       //             else
       //             {
       //                 objDichiarazione.ArticoliDichiarazione = GetMyArray();
       //             }

       //             DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;

       //             if (OpType == OSAPConst.OperationType.ADD)
       //             {
       //                 sScript = "location.href = '" + OSAPPages.DichiarazioniAdd + "?FromArticoli=true';";
       //                 Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
       //                 //Response.Redirect (OSAPPages.DichiarazioniAdd + "?FromArticoli=true");
       //             }
       //             else if (OpType == OSAPConst.OperationType.ADDFROMEDIT)
       //             {
       //                 DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.Now;
       //                 MetodiDichiarazioneTosapCosap.UpdateDichiarazione(DichiarazioneSession.SessionDichiarazioneTosapCosap);

       //                 DichiarazioneSession.SessionDichiarazioneTosapCosap.TestataDichiarazione.DataVariazione = DateTime.MinValue;
       //                 //Response.Redirect (OSAPPages.DichiarazioniAdd + "?FromArticoli=true");

       //                 sScript = "GestAlert('a', 'success', '', '', 'Nuovo Articolo aggiunto correttamente alla dichiarazione');location.href = '" + OSAPPages.DichiarazioniEdit + "?FromArticoli=true';";
       //                 Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");

       //                 //Response.Redirect (OSAPPages.DichiarazioniEdit + "?FromArticoli=true");
       //             }
       //         }
       //         else if (OpType == OSAPConst.OperationType.EDIT)
       //         {
       //             DAO.ArticoliDAO FncArticolo = new DAO.ArticoliDAO();
       //             Articolo[] arrayArticolo = new Articolo[1];
       //             arrayArticolo[0] = SetArticoliObject();
       //             //Usato per eliminare gli articoli;
       //             arrayArticolo[0].IdArticolo = this.IdArticolo;
       //             //Log.Debug("SalvaArticolo::IdArticoloPadre::"+arrayArticolo[0].IdArticoloPadre.ToString());
       //             foreach (Articolo myRow in DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione)
       //             {
       //                 if (myRow.IdArticolo == this.IdArticolo)
       //                 {
       //                     Articolo myItem = myRow;
       //                     //*** 20140410
       //                     conn = new SqlConnection(DichiarazioneSession.StringConnection);
       //                     conn.Open();
       //                     /*
							//DBEngine MyDBEngine=null;
							//MyDBEngine = DAO.DBEngineFactory.GetDBEngine();
							//MyDBEngine.OpenConnection();
							//MyDBEngine.BeginTransaction();
       //                     */
       //                     connessioneDB();
       //                     startTransazione = conn.BeginTransaction();
       //                     //startTransazione.Connection.BeginTransaction();

       //                     //Salva Dichiarazione e modifica articolo
       //                     myRow.DataVariazione = DateTime.Now;
       //                     //nRet=FncArticolo.UpdateArticolo(ref MyDBEngine, myRow);
       //                     nRet = FncArticolo.UpdateArticolo(ref cmdMyCommand, myRow);
       //                     if (nRet < 1)
       //                     {
       //                         startTransazione.Rollback();
       //                         cmdMyCommand.Connection.Close();
       //                         break;
       //                         /*
							//	MyDBEngine.RollbackTransaction();
							//	MyDBEngine.CloseConnection();
       //                         */
       //                     }
       //                     myItem = arrayArticolo[0];
       //                     myRow.DataVariazione = DateTime.MinValue;
       //                     //nRet=FncArticolo.SetArticolo(ref MyDBEngine, myRow);
       //                     nRet = FncArticolo.SetArticolo(ref cmdMyCommand, myItem);
       //                     if (nRet < 1)
       //                     {
       //                         startTransazione.Rollback();
       //                         cmdMyCommand.Connection.Close();
       //                         break;
       //                         /*
							//	MyDBEngine.RollbackTransaction();
							//	MyDBEngine.CloseConnection();								
       //                         */
       //                     }
       //                     startTransazione.Commit();
       //                     cmdMyCommand.Connection.Close();
       //                     //MyDBEngine.CommitTransaction();  
       //                     //MyDBEngine.CloseConnection();
       //                     break;
       //                 }
       //             }
       //             if (nRet > 1)
       //             {
       //                 sScript = "GestAlert('a', 'success', '', '', 'Articolo modificato correttamente');";
       //                 sScript += "location.href = '" + OSAPPages.DichiarazioniEdit + "?FromArticoli=true';";
       //                 //Response.Redirect(OSAPPages.DichiarazioniEdit + "?FromArticoli=true");
       //             }
       //             else
       //             {
       //                 sScript = "GestAlert('a', 'danger', '', '', 'Errore durante la modifica');";
       //             }
       //             Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script language=\'javascript\'>" + sScript + "</script>");
       //         }
       //     }
       //     catch (Exception Err)
       //     {
       //         Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.SalvaArticolo.errore: ", Err);
       //         Response.Redirect("../../PaginaErrore.aspx");
       //         throw Err;
       //     }
       // }


        public Articolo SetArticoliObject()
        {
            Articolo CurrentItem = new Articolo();

            try
            {
                //Immobile Dichiarazione
                CurrentItem.Operatore = DichiarazioneSession.sOperatore;
                CurrentItem.DataInserimento = DateTime.Now;

                if (TxtCivico.Text.CompareTo("") != 0)
                    CurrentItem.Civico = int.Parse(TxtCivico.Text);
                else
                    CurrentItem.Civico = -1;
                CurrentItem.CodVia = int.Parse(TxtCodVia.Text);
                CurrentItem.DataInizioOccupazione = DateTime.Parse(TxtDataInizio.Text);
                CurrentItem.Esponente = TxtEsponente.Text;
                CurrentItem.IdArticolo = -1;
                CurrentItem.IdDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap.IdDichiarazione;
                CurrentItem.Interno = TxtInterno.Text;

                CurrentItem.MaggiorazioneImporto = SharedFunction.FormatDoubleToDb(txtMaggiorazioni.Text);
                CurrentItem.MaggiorazionePerc = SharedFunction.FormatDoubleToDb(txtMaggiorazioniPerc.Text);

                CurrentItem.Consistenza = SharedFunction.FormatDoubleToDb(txtConsistenza.Text);

                TipoConsistenza objTipoConsistenza = new TipoConsistenza();
                objTipoConsistenza.IdTipoConsistenza = int.Parse(cmbTipoConsistenza.SelectedValue);
                objTipoConsistenza.Descrizione = cmbTipoConsistenza.SelectedItem.Text;
                CurrentItem.TipoConsistenzaTOCO = objTipoConsistenza;

                CurrentItem.Note = TxtNoteUI.Text;
                CurrentItem.Scala = TxtScala.Text;
                CurrentItem.SVia = TxtVia.Text;
                CurrentItem.DetrazioneImporto = SharedFunction.FormatDoubleToDb(txtDetrazioni.Text);

                Categorie objCategoria = new Categorie();
                objCategoria.IdCategoria = int.Parse(cmbCategoria.SelectedValue);
                objCategoria.Descrizione = cmbCategoria.SelectedItem.Text;
                CurrentItem.Categoria = objCategoria;

                TipologieOccupazioni objTipologiaOccupazione = new TipologieOccupazioni();
                objTipologiaOccupazione.IdTipologiaOccupazione = int.Parse(cmbTipologiaOccupazione.SelectedValue);
                objTipologiaOccupazione.Descrizione = cmbTipologiaOccupazione.SelectedItem.Text;
                CurrentItem.TipologiaOccupazione = objTipologiaOccupazione;


                ArrayList oListArtVSAgev = new ArrayList();
                
                foreach (GridViewRow MyItemGrd in GrdAgevolazioni.Rows)
                {
                    if (((CheckBox)(MyItemGrd.FindControl("ChkSelezionato"))).Checked == true)
                    {
                        Agevolazione oMyAgevolazione = new Agevolazione();
                        oMyAgevolazione.IdAgevolazione = int.Parse(((HiddenField)(MyItemGrd.FindControl("hfIdAgevolazione"))).Value);

                        oListArtVSAgev.Add(oMyAgevolazione);
                    }
                }
                CurrentItem.ListAgevolazioni = (Agevolazione[])oListArtVSAgev.ToArray(typeof(Agevolazione));

                CurrentItem.DurataOccupazione = int.Parse(txtDurata.Text);

                Durata objDurata = new Durata();
                objDurata.IdDurata = int.Parse(cmbTipoDurata.SelectedValue);
                objDurata.Descrizione = cmbTipoDurata.SelectedItem.Text;
                CurrentItem.TipoDurata = objDurata;
                //se la durata è annuale allora sono in occupazione permanente altrimenti è temporanea
                if (objDurata.Descrizione == "ORE" || objDurata.Descrizione == "GIORNI")
                {
                    CurrentItem.IdTributo = Utility.Costanti.TRIBUTO_OccupazioneTemporanea;
                }
                else
                {
                    CurrentItem.IdTributo = Utility.Costanti.TRIBUTO_OccupazionePermanente;
                }
                //ArtCava: aggiunta compilazione del campo
                if (SharedFunction.IsNullOrEmpty(txtDataFine))
                {
                    switch (objDurata.Descrizione)
                    {
                        case "ORE":
                            CurrentItem.DataFineOccupazione = CurrentItem.DataInizioOccupazione.AddHours(CurrentItem.DurataOccupazione);
                            break;
                        case "GIORNI":
                            CurrentItem.DataFineOccupazione = CurrentItem.DataInizioOccupazione.AddDays(CurrentItem.DurataOccupazione);
                            break;
                        default:
                            CurrentItem.DataFineOccupazione = CurrentItem.DataInizioOccupazione.AddYears(CurrentItem.DurataOccupazione);
                            break;
                    }
                }
                else
                    CurrentItem.DataFineOccupazione = DateTime.Parse(txtDataFine.Text);

                CurrentItem.Attrazione = chkAttrazione.Checked;
                //*** 20130610 - ruolo supplettivo ***
                if (TxtIdArticoloPadre.Text != string.Empty)
                    CurrentItem.IdArticoloPadre = int.Parse(TxtIdArticoloPadre.Text);
                //Log.Debug("WucArticolo::SetArticoliObject::IdArticoloPadre::"+CurrentItem.IdArticoloPadre.ToString());
                //*** ***
                //Setto un IdArticolo all'immobile per poterlo elimare
                if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione != null)
                    CurrentItem.IdArticolo = DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione.Length + 1000000001;
                else
                    CurrentItem.IdArticolo = 1000000001;

                return CurrentItem;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.SetArticoliObject.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }
        /// <summary>
        /// Get Articoli dichiarazione da Dichiarazione session object
        /// </summary>
        /// <returns>Articolo[]</returns>
        /// <revisionHistory>
        /// <revision date="29/04/2019">
        /// 22/PO/19 - Sistemazione per refresh dopo salvataggio
        /// </revision>
        /// </revisionHistory>
        private Articolo[] GetMyArray()
        {
            try
            {
                ArrayList MyArray = new ArrayList();


                if (OpType == OSAPConst.OperationType.ADD || OpType == OSAPConst.OperationType.ADDFROMEDIT)
                {
                    MyArray = MetodiArticolo.GetMyArray(DichiarazioneSession.IdEnte);
                    MyArray.Add(SetArticoliObject());
                }
                else
                {
                    foreach (Articolo myRow in DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione)
                    {
                        if (myRow.IdArticolo == this.IdArticolo)
                            MyArray.Add(SetArticoliObject());
                        else
                            MyArray.Add(myRow);
                    }
                }
                return (Articolo[])MyArray.ToArray(typeof(Articolo));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.GetMyArray.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }
        //private Articolo[] GetMyArray()
        //{
        //    try
        //    {
        //        ArrayList MyArray = new ArrayList();

        //        MyArray = MetodiArticolo.GetMyArray(DichiarazioneSession.IdEnte);
        //        if (OpType == OSAPConst.OperationType.ADD || OpType == OSAPConst.OperationType.ADDFROMEDIT)
        //        {
        //            MyArray.Add(SetArticoliObject());
        //        }
        //        return (Articolo[])MyArray.ToArray(typeof(Articolo));
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.GetMyArray.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //        throw Err;
        //    }
        //}

        public void connessioneDB()
        {
            //*** 20140409
            try
            {
                Log.Debug("WucArticolo::connessioneDB::apertura della connessione al DB");
                cmdMyCommand = new SqlCommand();
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.connessioneDB.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

        protected void ChkSelezionato_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ArrayList oListArtVSAgev = new ArrayList();
                foreach (GridViewRow MyItemGrd in GrdAgevolazioni.Rows)
                {
                    if (((CheckBox)(MyItemGrd.FindControl("ChkSelezionato"))).Checked == true)
                    {
                        Agevolazione oMyAgevolazione = new Agevolazione();
                        oMyAgevolazione.IdAgevolazione = int.Parse(((HiddenField)(MyItemGrd.FindControl("hfIdAgevolazione"))).Value);
                        oListArtVSAgev.Add(oMyAgevolazione);
                    }
                }
                GrdAgevolazioni.DataSource = oListArtVSAgev;
                GrdAgevolazioni.DataBind();
                Session["Agevolazioni"] = oListArtVSAgev;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.WucArticolo.ChkSelezionato_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

        #endregion

        #region Public Method

        //Set mandatory form fields
        public string[] GetMandatoryFields()
        {
            string[] returnValue = new string[2];

            string clientControlsID = "";
            string clientMandatoryMessagge = "";

            clientControlsID = this.TxtCodVia.ClientID + ";";
            clientControlsID += this.TxtDataInizio.ClientID + ";";
            clientControlsID += this.cmbCategoria.ClientID + ";";
            clientControlsID += this.cmbTipoDurata.ClientID + ";";
            clientControlsID += this.txtDurata.ClientID + ";";
            clientControlsID += this.cmbTipologiaOccupazione.ClientID + ";";
            clientControlsID += this.txtConsistenza.ClientID + ";";
            clientControlsID += this.cmbTipoConsistenza.ClientID + ";";
            //*** 20130325 - aggiunto controllo obligatorietà su data fine ***
            clientControlsID += this.txtDataFine.ClientID;
            //*** ***

            clientMandatoryMessagge = " - Via" + ";";
            clientMandatoryMessagge += " - Data Inizio" + ";";
            clientMandatoryMessagge += " - Categoria" + ";";
            clientMandatoryMessagge += " - Tipo Durata" + ";";
            clientMandatoryMessagge += " - Durata" + ";";
            clientMandatoryMessagge += " - Tipologia Occupazione" + ";";
            clientMandatoryMessagge += " - Consistenza" + ";";
            clientMandatoryMessagge += " - Tipo Consistenza" + ";";
            //*** 20130325 - aggiunto controllo obligatorietà su data fine ***
            clientMandatoryMessagge += " - Data Fine;";
            //*** ***

            returnValue[0] = clientControlsID;
            returnValue[1] = clientMandatoryMessagge;

            return returnValue;
        }

        public Articolo[] RemoveArticolo()
        {
            return GetMyArray();
        }

        #endregion

    }
}