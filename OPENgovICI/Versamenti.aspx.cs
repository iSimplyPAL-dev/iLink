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
using System.Data.SqlTypes;
using DichiarazioniICI.Database;
using Business;
using AnagInterface;
using Ribes;
using System.Text;
using log4net;

namespace DichiarazioniICI
{
    /// <summary>
    /// Classe di gestione per la pagina Versamenti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
 /// <revisionHistory>
/// <revision date="12/04/2019">
/// <strong>Qualificazione AgID-analisi_rel01</strong>
/// <em>Analisi eventi</em>
/// </revision>
/// </revisionHistory>
public partial class Versamenti :BaseEnte
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Versamenti));

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

		}
		#endregion
		/**** 20120828 - IMU adeguamento per importi statali ****/
		/**** ****/
		#region Property
		/// <summary>
		/// Ritorna o assegna l'id del versamento.
		/// </summary>
		protected int IDVersamento
		{
			get{return ViewState["IDVersamento"] != null ? (int)ViewState["IDVersamento"] : 0;}	
			set{ViewState["IDVersamento"] = value;}
		}
		#endregion

		#region Metodi
		/// <summary>
		/// Torna una Dataview con l'elenco delle provenienze.
		/// </summary>
		/// <returns></returns>
		protected DataView ListProvenienze()
		{
            //*** 20140630 - TASI ***
			//DataView Vista = new ProvenienzeTable(ConstWrapper.sUsername).List(TipologieProvenienza.versamenti, ApplicationHelper.Tributo);
            DataView Vista = new ProvenienzeTable(ConstWrapper.sUsername).List(TipologieProvenienza.versamenti, ConstWrapper.CodiceTributo,0);
			return Vista;
		}
		#endregion
        /// <summary>
        /// caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string strscript = "";
            try
            {
                if (!IsPostBack)
                {
                    // Binding dei parametri statici letti da config
                    txtContoCorrente.DataBind();
                    txtComuneIntestatrio.DataBind();
                    txtComuneUbicazImmob.DataBind();
                    ddlProvenienze.DataBind();
                    //salvo il typeoperation, che mi gestisce il pulsante di Back
                    txtTypeOperation.Text = Request["TYPEOPERATION"].ToString();

                    AbilitaViolazione(false);

                    lnkPulisciContr.Attributes.Add("onclick", "return PulisciContr();");

                    if (Request.QueryString["IDVersamento"] != null)
                    {
                        strscript = "parent.Comandi.location.href='CVersamenti.aspx';";
                        strscript += "$('#Unlock').show();";
                        RegisterScript(strscript,this.GetType());

                        //prendo i dati del versamento
                        this.IDVersamento = int.Parse(Request.QueryString["IDVersamento"]);
                        //btnBonifica.Attributes.Add("onclick", "javascript:window.showModalDialog('PopUpErroriVersamenti.aspx?IDVersamento=" + this.IDVersamento + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');return false;");
                        LoadVersamento();
                    }
                    else
                    {
                        //*** 201504 - Nuova Gestione anagrafica con form unico ***
                        if (ConstWrapper.HasPlainAnag)
                            ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString());
                        Abilita(true);
                        AbilitaViolazione(chkViolazione.Checked);

                        strscript = "parent.Comandi.location.href='CVersamenti.aspx';";
                        strscript += "$('#Unlock').hide();";
                        RegisterScript(strscript,this.GetType());
                    }
                    btnSalva.Attributes.Add("onclick", "return ControllaCampi();");
                }
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                if (ConstWrapper.HasPlainAnag)
                    strscript = "document.getElementById('TRSpecAnag').style.display='none';";
                else
                    strscript = "document.getElementById('TRPlainAnag').style.display='none';";
                RegisterScript(strscript,this.GetType());
                new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Pagamento, "Gestione", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDVersamento);
            }
            catch (Exception Err) 
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
		}

        /// <summary>
        /// Esegue il salvataggio del versamento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="28/08/2012">
        /// <strong>IMU adeguamento per importi statali</strong>
        /// </revision>
        /// <revision date="22/04/2013">
        /// <strong>aggiornamento IMU</strong>
        /// </revision>
        /// <revision date="30/06/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnSalva_Click(object sender, System.EventArgs e)
		{
			bool retval = true;
            try
            {
                // effettuo il salvataggio
                Utility.DichManagerICI.VersamentiRow RigaVersamento = new Utility.DichManagerICI.VersamentiRow();
                RigaVersamento.Ente = ConstWrapper.CodiceEnte;
                RigaVersamento.IdAnagrafico =Utility.StringOperation.FormatInt( hdIdContribuente.Value );
                RigaVersamento.AnnoRiferimento = txtAnnoRiferimento.Text;
                if (optTASI.Checked)
                    RigaVersamento.CodTributo = Utility.Costanti.TRIBUTO_TASI;
                else
                    RigaVersamento.CodTributo = Utility.Costanti.TRIBUTO_ICI;
                RigaVersamento.CodiceFiscale = txtCodFiscale.Text;
                RigaVersamento.PartitaIva = txtPIVA.Text;
                RigaVersamento.ImportoPagato = txtImportoPagamento.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoPagamento.Text);
                RigaVersamento.DataPagamento = Utility.StringOperation.FormatDateTime(txtDataPagamento.Text);
                RigaVersamento.NumeroBollettino = txtNumBolletino.Text;
                RigaVersamento.NumeroFabbricatiPosseduti =Utility.StringOperation.FormatInt(txtNumFabbricatiPosseduti.Text);
                RigaVersamento.Acconto = chkAcconto.Checked;
                RigaVersamento.Saldo = chkSaldo.Checked;
                RigaVersamento.RavvedimentoOperoso = chkRavvedimentoOperoso.Checked;
                RigaVersamento.ImportoTerreni = txtImportoTerreniAgricoli.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoTerreniAgricoli.Text);
                RigaVersamento.ImportoAreeFabbric = txtImportoAreeFabbricabili.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAreeFabbricabili.Text);
                RigaVersamento.ImportoAltrifabbric = txtImportoAltriFabbricati.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAltriFabbricati.Text);
                RigaVersamento.ImportoAbitazPrincipale = txtImportoAbitazPrincipale.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAbitazPrincipale.Text);
                RigaVersamento.DetrazioneAbitazPrincipale = txtDetrazioniAbitazPrincipale.Text == String.Empty ? 0 : Convert.ToDecimal(txtDetrazioniAbitazPrincipale.Text);
                RigaVersamento.ImportoTerreniStatale = txtImportoTerreniAgricoliStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoTerreniAgricoliStato.Text);
                RigaVersamento.ImportoAreeFabbricStatale = txtImportoAreeFabbricabiliStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAreeFabbricabiliStato.Text);
                RigaVersamento.ImportoAltrifabbricStatale = txtImportoAltriFabbricatiStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAltriFabbricatiStato.Text);
                RigaVersamento.ImportoFabRurUsoStrum = txtImpFabRurUsoStrum.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpFabRurUsoStrum.Text);
                RigaVersamento.ImportoFabRurUsoStrumStatale = txtImpFabRurUsoStrumStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpFabRurUsoStrumStato.Text);
                RigaVersamento.ImportoUsoProdCatD = txtImpUsoProdCatD.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpUsoProdCatD.Text);
                RigaVersamento.ImportoUsoProdCatDStatale = txtImpUsoProdCatDStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpUsoProdCatDStato.Text);
                RigaVersamento.ContoCorrente = txtContoCorrente.Text;
                RigaVersamento.ComuneUbicazioneImmobile = txtComuneUbicazImmob.Text;
                RigaVersamento.ComuneIntestatario = txtComuneIntestatrio.Text;
                RigaVersamento.Bonificato = false;
                RigaVersamento.DataInizioValidità = DateTime.Now;
                RigaVersamento.DataFineValidità = DateTime.MaxValue;
                RigaVersamento.Operatore = ConstWrapper.sUsername;
                RigaVersamento.Annullato = false;
                RigaVersamento.ImportoSoprattassa = txtImportoSoprattassa.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoSoprattassa.Text);
                RigaVersamento.ImportoPenaPecuniaria = txtPenaPecuniaria.Text == String.Empty ? 0 : Convert.ToDecimal(txtPenaPecuniaria.Text);
                RigaVersamento.DataProvvedimentoViolazione = Utility.StringOperation.FormatDateTime(txtDataAtto.Text);
                RigaVersamento.NumeroAttoAccertamento = txtNAtto.Text.ToString();
                RigaVersamento.Interessi = txtInteressi.Text == String.Empty ? 0 : Convert.ToDecimal(txtInteressi.Text);
                RigaVersamento.Violazione = chkViolazione.Checked;
                RigaVersamento.IDProvenienza = Utility.StringOperation.FormatInt(ddlProvenienze.SelectedValue);
                RigaVersamento.Provenienza = ddlProvenienze.SelectedItem.Text;
                RigaVersamento.DataRiversamento = Utility.StringOperation.FormatDateTime(TxtDataRiversamento.Text);
                RigaVersamento.NumeroProvvedimentoViolazione = ""; RigaVersamento.ImportoImposta = 0; RigaVersamento.Note = ""; RigaVersamento.DetrazioneStatale = 0;
                if (this.IDVersamento == 0)
                {
                    int IdVersamento;
                    retval = new VersamentiTable(ConstWrapper.sUsername).Insert(Utility.Costanti.AZIONE_NEW,RigaVersamento, out IdVersamento);
                    this.IDVersamento = IdVersamento;
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new  Utility.Costanti.LogEventArgument().Pagamento, "btnSalva", Utility.Costanti.AZIONE_NEW.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDVersamento);
                }
                else
                {
                    RigaVersamento.ID = this.IDVersamento;
                    int myID = this.IDVersamento;
                    retval = new VersamentiTable(ConstWrapper.sUsername).Insert(Utility.Costanti.AZIONE_UPDATE, RigaVersamento, out myID);
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new  Utility.Costanti.LogEventArgument().Pagamento, "btnSalva", Utility.Costanti.AZIONE_UPDATE.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDVersamento);
                }
                string   strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Salvataggio" + (retval == true ? "" : " non") + " effettuato.');";
                RegisterScript(strscript,this.GetType());
                if (retval)
                    btnElimina.Enabled = true;

                GoToIndietro();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.btnSalva_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //protected void btnSalva_Click(object sender, System.EventArgs e)
        //{
        //    bool retval = true;
        //    try
        //    {
        //        // effettuo il salvataggio
        //        VersamentiRow RigaVersamento = new VersamentiRow();
        //        RigaVersamento.Ente = ConstWrapper.CodiceEnte;
        //        RigaVersamento.IdAnagrafico = hdIdContribuente.Value == String.Empty ? 0 : int.Parse(hdIdContribuente.Value);
        //        RigaVersamento.AnnoRiferimento = txtAnnoRiferimento.Text;
        //        //*** 20140630 - TASI ***
        //        if (optTASI.Checked)
        //            RigaVersamento.CodTributo = Utility.Costanti.TRIBUTO_TASI;
        //        else
        //            RigaVersamento.CodTributo = ConstWrapper.CodiceTributo;
        //        //*** ***
        //        RigaVersamento.CodiceFiscale = txtCodFiscale.Text;
        //        RigaVersamento.PartitaIva = txtPIVA.Text;
        //        RigaVersamento.ImportoPagato = txtImportoPagamento.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoPagamento.Text);
        //        RigaVersamento.DataPagamento = Convert.ToDateTime(txtDataPagamento.Text);
        //        RigaVersamento.NumeroBollettino = txtNumBolletino.Text;
        //        if (txtNumFabbricatiPosseduti.Text.CompareTo("") == 0)
        //        {
        //            RigaVersamento.NumeroFabbricatiPosseduti = 0;
        //        }
        //        else
        //        {
        //            RigaVersamento.NumeroFabbricatiPosseduti = int.Parse(txtNumFabbricatiPosseduti.Text);
        //        }
        //        RigaVersamento.Acconto = chkAcconto.Checked;
        //        RigaVersamento.Saldo = chkSaldo.Checked;
        //        RigaVersamento.RavvedimentoOperoso = chkRavvedimentoOperoso.Checked;
        //        RigaVersamento.ImportoTerreni = txtImportoTerreniAgricoli.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoTerreniAgricoli.Text);
        //        RigaVersamento.ImportoAreeFabbric = txtImportoAreeFabbricabili.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAreeFabbricabili.Text);
        //        RigaVersamento.ImportoAltriFabbric = txtImportoAltriFabbricati.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAltriFabbricati.Text);
        //        RigaVersamento.ImportoAbitazPrincipale = txtImportoAbitazPrincipale.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAbitazPrincipale.Text);
        //        RigaVersamento.DetrazioneAbitazPrincipale = txtDetrazioniAbitazPrincipale.Text == String.Empty ? 0 : Convert.ToDecimal(txtDetrazioniAbitazPrincipale.Text);
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        RigaVersamento.ImportoTerreniStatale = txtImportoTerreniAgricoliStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoTerreniAgricoliStato.Text);
        //        RigaVersamento.ImportoAreeFabbricStatale = txtImportoAreeFabbricabiliStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAreeFabbricabiliStato.Text);
        //        RigaVersamento.ImportoAltriFabbricStatale = txtImportoAltriFabbricatiStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoAltriFabbricatiStato.Text);
        //        RigaVersamento.ImportoFabRurUsoStrum = txtImpFabRurUsoStrum.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpFabRurUsoStrum.Text);
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        RigaVersamento.ImportoFabRurUsoStrumStatale = txtImpFabRurUsoStrumStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpFabRurUsoStrumStato.Text);
        //        RigaVersamento.ImportoUsoProdCatD = txtImpUsoProdCatD.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpUsoProdCatD.Text);
        //        RigaVersamento.ImportoUsoProdCatDStatale = txtImpUsoProdCatDStato.Text == String.Empty ? 0 : Convert.ToDecimal(txtImpUsoProdCatDStato.Text);
        //        /**** ****/
        //        RigaVersamento.ContoCorrente = txtContoCorrente.Text;
        //        RigaVersamento.ComuneUbicazioneImmobile = txtComuneUbicazImmob.Text;
        //        RigaVersamento.ComuneIntestatario = txtComuneIntestatrio.Text;
        //        RigaVersamento.Bonificato = false;
        //        RigaVersamento.DataInizioValidità = DateTime.Now;
        //        RigaVersamento.DataFineValidità = (DateTime)SqlDateTime.MinValue;
        //        RigaVersamento.Operatore = ConstWrapper.sUsername;
        //        RigaVersamento.Annullato = false;
        //        RigaVersamento.ImportoSoprattassa = txtImportoSoprattassa.Text == String.Empty ? 0 : Convert.ToDecimal(txtImportoSoprattassa.Text);
        //        RigaVersamento.ImportoPenaPecuniaria = txtPenaPecuniaria.Text == String.Empty ? 0 : Convert.ToDecimal(txtPenaPecuniaria.Text);
        //        RigaVersamento.DataProvvedimentoViolazione = txtDataAtto.Text == String.Empty ? DateTime.MinValue : DateTime.Parse(txtDataAtto.Text);
        //        RigaVersamento.NumeroAttoAccertamento = txtNAtto.Text.ToString();
        //        RigaVersamento.Interessi = txtInteressi.Text == String.Empty ? 0 : Convert.ToDecimal(txtInteressi.Text);
        //        RigaVersamento.Violazione = chkViolazione.Checked;
        //        RigaVersamento.IDProvenienza = int.Parse(ddlProvenienze.SelectedValue);
        //        RigaVersamento.Provenienza = ddlProvenienze.SelectedItem.Text;
        //        RigaVersamento.DataRiversamento = TxtDataRiversamento.Text == String.Empty ? DateTime.MinValue : DateTime.Parse(TxtDataRiversamento.Text);
        //        RigaVersamento.NumeroProvvedimentoViolazione = ""; RigaVersamento.ImportoImposta = 0; RigaVersamento.Note = ""; RigaVersamento.DetrazioneStatale = 0;
        //        if (this.IDVersamento == 0)
        //        {
        //            int IdVersamento;
        //            retval = new VersamentiTable(ConstWrapper.sUsername).Insert(RigaVersamento, out IdVersamento);
        //            this.IDVersamento = IdVersamento;
        //        }
        //        else
        //        {
        //            RigaVersamento.ID = this.IDVersamento;
        //            retval = new VersamentiTable(ConstWrapper.sUsername).Modify(RigaVersamento);
        //        }
        //        string strscript = "GestAlert('a', '" + (retval == true ? "success" : "warning") + "', '', '', 'Salvataggio" + (retval == true ? "" : " non") + " effettuato.');";
        //        RegisterScript(strscript, this.GetType());
        //        if (retval)
        //            btnElimina.Enabled = true;

        //        GoToIndietro();

        //        //alep 11/02/2008 aggiunto if
        //        /*** 201511 -  la bonifica non serve più ***
        //        if (chkViolazione.Checked == false)
        //        {
        //            string strScript = string.Empty;
        //            //btnBonifica.Attributes.Add("onclick", "javascript:window.showModalDialog('PopUpErroriVersamenti.aspx?IDVersamento=" + this.IDVersamento + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');return false;");			
        //            strScript = "javascript:window.showModalDialog('PopUpErroriVersamenti.aspx?IDVersamento=" + this.IDVersamento + "', window, 'dialogHeight: 400px; dialogWidth: 400px; status: no');";
        //            RegisterScript(sScript,this.GetType());,"", "" + strScript + "");
        //        }*/
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.btnSalva_Click.errore: ", ex);
        //        Response.Redirect("../PaginaErrore.aspx");
        //    }
        //}
        /// <summary>
        /// Esegue l'eliminazione del versamento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnElimina_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ConstWrapper.CancellazioneFisicaVersamenti)
                {
                    int x = 0;
                    new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetVersamenti(Utility.Costanti.AZIONE_DELETE, new Utility.DichManagerICI.VersamentiRow() { ID = this.IDVersamento }, out x);
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new  Utility.Costanti.LogEventArgument().Pagamento, "btnElimina", Utility.Costanti.AZIONE_DELETE.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDVersamento);

                    btnIndietro_Click(sender, e);
                }
                else
                {
                    new VersamentiTable(ConstWrapper.sUsername).CancellazioneLogica(this.IDVersamento);
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new  Utility.Costanti.LogEventArgument().Pagamento, "btnElimina", Utility.Costanti.AZIONE_DELETE.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, this.IDVersamento);

                    this.IDVersamento = 0;

                    txtCodFiscale.Text = "";
                    txtPIVA.Text = "";
                    txtCognome.Text = "";
                    txtNome.Text = "";
                    hdIdContribuente.Value = "";
                    rdbMaschio.Checked = false; rdbFemmina.Checked = false; rdbGiuridica.Checked = false;
                    txtDataNasc.Text = "";
                    txtComNasc.Text = "";
                    txtProvNasc.Text = "";
                    txtViaRes.Text = "";
                    txtNumCivRes.Text = "";
                    txtIntRes.Text = "";
                    txtScalaRes.Text = "";
                    txtComuneRes.Text = "";
                    txtProvRes.Text = "";
                    txtCapRes.Text = "";
                    txtEsponenteCivico.Text = "";

                    //*** 20140630 - TASI ***
                    optICI.Checked = true; optTASI.Checked = false;
                    //*** ***
                    txtAnnoRiferimento.Text = "";
                    txtImportoPagamento.Text = "0,0";
                    txtDataPagamento.Text = "";
                    txtContoCorrente.Text = ConstWrapper.ContoCorrente;
                    txtNumBolletino.Text = "";
                    txtComuneIntestatrio.Text = ConstWrapper.IntestatarioBollettino;
                    txtComuneUbicazImmob.Text = ConstWrapper.ComuneUbicazioneImmobile;
                    txtNumFabbricatiPosseduti.Text = "0";
                    txtImportoTerreniAgricoli.Text = "0,0";
                    txtImportoAreeFabbricabili.Text = "0,0";
                    txtImportoAltriFabbricati.Text = "0,0";
                    txtImportoAbitazPrincipale.Text = "0,0";
                    txtDetrazioniAbitazPrincipale.Text = "0,0";
                    /**** 20120828 - IMU adeguamento per importi statali ****/
                    txtImportoTerreniAgricoliStato.Text = "0,0";
                    txtImportoAreeFabbricabiliStato.Text = "0,0";
                    txtImportoAltriFabbricatiStato.Text = "0,0";
                    txtImpFabRurUsoStrum.Text = "0,0";
                    /**** ****/
                    /**** 20130422 - aggiornamento IMU ****/
                    txtImpFabRurUsoStrumStato.Text = "0,0";
                    txtImpUsoProdCatD.Text = "0,0";
                    txtImpUsoProdCatDStato.Text = "0,0";
                    /**** ****/
                    chkRavvedimentoOperoso.Checked = false;
                    chkAcconto.Checked = false;
                    chkSaldo.Checked = false;
                    chkViolazione.Checked = false;
                    txtImportoSoprattassa.Text = "0,0";
                    txtPenaPecuniaria.Text = "0,0";
                    txtInteressi.Text = "0,0";
                    ddlProvenienze.SelectedValue = ddlProvenienze.Items[0].Value;
                    btnElimina.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.btnElimina_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        //protected void btnElimina_Click(object sender, System.EventArgs e)
        //{

        //    if (ConstWrapper.CancellazioneFisicaVersamenti)
        //    {
        //        //new VersamentiTable(ConstWrapper.sUsername).DeleteItem(this.IDVersamento);	
        //        int x = 0;
        //        int myID = this.IDVersamento;
        //        new Utility.DichManagerICI(ConstWrapper.DBType, ConstWrapper.StringConnection).SetVersamenti(Utility.Costanti.AZIONE_DELETE, new Utility.DichManagerICI.VersamentiRow() { ID = this.IDVersamento }, out x);

        //        btnIndietro_Click(sender, e);
        //    }
        //    else
        //    {
        //        new VersamentiTable(ConstWrapper.sUsername).CancellazioneLogica(this.IDVersamento);

        //        this.IDVersamento = 0;

        //        txtCodFiscale.Text = "";
        //        txtPIVA.Text = "";
        //        txtCognome.Text = "";
        //        txtNome.Text = "";
        //        hdIdContribuente.Value = "";
        //        rdbMaschio.Checked = false; rdbFemmina.Checked = false; rdbGiuridica.Checked = false;
        //        txtDataNasc.Text = "";
        //        txtComNasc.Text = "";
        //        txtProvNasc.Text = "";
        //        txtViaRes.Text = "";
        //        txtNumCivRes.Text = "";
        //        txtIntRes.Text = "";
        //        txtScalaRes.Text = "";
        //        txtComuneRes.Text = "";
        //        txtProvRes.Text = "";
        //        txtCapRes.Text = "";
        //        txtEsponenteCivico.Text = "";

        //        //*** 20140630 - TASI ***
        //        optICI.Checked = true; optTASI.Checked = false;
        //        //*** ***
        //        txtAnnoRiferimento.Text = "";
        //        txtImportoPagamento.Text = "0,0";
        //        txtDataPagamento.Text = "";
        //        txtContoCorrente.Text = ConstWrapper.ContoCorrente;
        //        txtNumBolletino.Text = "";
        //        txtComuneIntestatrio.Text = ConstWrapper.IntestatarioBollettino;
        //        txtComuneUbicazImmob.Text = ConstWrapper.ComuneUbicazioneImmobile;
        //        txtNumFabbricatiPosseduti.Text = "0";
        //        txtImportoTerreniAgricoli.Text = "0,0";
        //        txtImportoAreeFabbricabili.Text = "0,0";
        //        txtImportoAltriFabbricati.Text = "0,0";
        //        txtImportoAbitazPrincipale.Text = "0,0";
        //        txtDetrazioniAbitazPrincipale.Text = "0,0";
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        txtImportoTerreniAgricoliStato.Text = "0,0";
        //        txtImportoAreeFabbricabiliStato.Text = "0,0";
        //        txtImportoAltriFabbricatiStato.Text = "0,0";
        //        txtImpFabRurUsoStrum.Text = "0,0";
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        txtImpFabRurUsoStrumStato.Text = "0,0";
        //        txtImpUsoProdCatD.Text = "0,0";
        //        txtImpUsoProdCatDStato.Text = "0,0";
        //        /**** ****/
        //        chkRavvedimentoOperoso.Checked = false;
        //        chkAcconto.Checked = false;
        //        chkSaldo.Checked = false;
        //        chkViolazione.Checked = false;
        //        txtImportoSoprattassa.Text = "0,0";
        //        txtPenaPecuniaria.Text = "0,0";
        //        txtInteressi.Text = "0,0";
        //        ddlProvenienze.SelectedValue = ddlProvenienze.Items[0].Value;
        //        btnElimina.Enabled = false;
        //    }
        //}

        protected void lnkIntestatoA_Click(object sender, System.EventArgs e)
		{
            txtComuneIntestatrio.DataBind();
			txtComuneIntestatrio.Enabled = !txtComuneIntestatrio.Enabled;
		}

		protected void lnkUbicazione_Click(object sender, System.EventArgs e)
		{
            txtComuneUbicazImmob.DataBind();
			txtComuneUbicazImmob.Enabled = !txtComuneUbicazImmob.Enabled;
		}

		protected void lnkContoCorrente_Click(object sender, System.EventArgs e)
		{
            txtContoCorrente.DataBind();
            txtContoCorrente.Enabled = !txtContoCorrente.Enabled;
		}

		#region Abilita
		/// <summary>
		/// Abilita o disabilita i comandi della pagina.
		/// </summary>
		/// <param name="abilita"></param>
		private void Abilita(bool abilita)
		{
            try { 
	//			cvalImpAbitazPrincip.Enabled = abilita;
	//			cvalImpAltriFabbr.Enabled = abilita;
	//			cvalImportoPagamento.Enabled = abilita;
	//			cvalImpTerreni.Enabled = abilita;
	//			cvalAreeFabbric.Enabled = abilita;
	//			cvalDetrazAbitazPrincip.Enabled = abilita;
            //dati anagrafici solo in consultazione
            txtCodFiscale.Enabled = false;
            txtPIVA.Enabled = false;
            txtCognome.Enabled = false;
            txtNome.Enabled = false;
            rdbMaschio.Enabled = false;
            rdbFemmina.Enabled = false;
            rdbGiuridica.Enabled = false;
            txtDataNasc.Enabled = false;
            txtComNasc.Enabled = false;
            txtProvNasc.Enabled = false;
            txtViaRes.Enabled = false;
            txtNumCivRes.Enabled = false;
            txtIntRes.Enabled = false;
            txtScalaRes.Enabled = false;
            txtComuneRes.Enabled = false;
            txtProvRes.Enabled = false;
            txtCapRes.Enabled = false;
            txtEsponenteCivico.Enabled = false; 
            //*** ***
            lnkPulisciContr.Enabled = abilita;
			lblContrAnno.Enabled = abilita;
            //lblContrContrib.Enabled = abilita;
			lblContrDataPag.Enabled= abilita;
			lblContrModoPag.Enabled = abilita;
			lblContrNFab.Enabled = abilita;
            //*** 20140630 - TASI ***
            optICI.Enabled = abilita; optTASI.Enabled = abilita;
            //*** ***
            txtContoCorrente.Enabled = abilita;
            txtComuneIntestatrio.Enabled = abilita;
            txtComuneUbicazImmob.Enabled = abilita;
			lblAnnoRiferimento.Enabled = abilita;
			txtAnnoRiferimento.Enabled = abilita;
			lblImportoPagamento.Enabled = abilita;
			txtImportoPagamento.Enabled = abilita;
			lblDataPagamento.Enabled = abilita;
			txtDataPagamento.Enabled = abilita;
			Label1.Enabled=abilita;
			TxtDataRiversamento.Enabled=abilita;
			lnkContoCorrente.Enabled = abilita;
			lblNumBollettino.Enabled = abilita;
			txtNumBolletino.Enabled = abilita;
			lnkUbicazione.Enabled = abilita;
			lnkIntestatoA.Enabled = abilita;
			lblNumeroFabbricati.Enabled = abilita;
			txtNumFabbricatiPosseduti.Enabled = abilita;
			lblImportoAltriFabbricati.Enabled = abilita;
			txtImportoAltriFabbricati.Enabled = abilita;
			lblImportoTerreniAgricoli.Enabled = abilita;
			txtImportoTerreniAgricoli.Enabled = abilita;
			lblImportoAreeFabbricabili.Enabled = abilita;
			txtImportoAreeFabbricabili.Enabled = abilita;
			lblImportoAbitazPrincipale.Enabled = abilita;
			txtImportoAbitazPrincipale.Enabled = abilita;
			lblDetrazioniAbitazPrincipale.Enabled = abilita;
			txtDetrazioniAbitazPrincipale.Enabled = abilita;
			/**** 20120828 - IMU adeguamento per importi statali ****/
			lblImportoAltriFabbricatiStatale.Enabled = abilita;
			txtImportoAltriFabbricatiStato.Enabled = abilita;
			lblImportoTerreniAgricoliStatale.Enabled = abilita;
			txtImportoTerreniAgricoliStato.Enabled = abilita;
			lblImportoAreeFabbricabiliStatale.Enabled = abilita;
			txtImportoAreeFabbricabiliStato.Enabled = abilita;
			lblImpFabRurUsoStrum.Enabled = abilita;
			txtImpFabRurUsoStrum.Enabled = abilita;
			/**** ****/
			/**** 20130422 - aggiornamento IMU ****/
			lblImpFabRurUsoStrumStatale.Enabled = abilita;
			txtImpFabRurUsoStrumStato.Enabled = abilita;
			lblImpUsoProdCatD.Enabled = abilita;
			txtImpUsoProdCatD.Enabled = abilita;
			lblImpUsoProdCatDStatale.Enabled = abilita;
			txtImpUsoProdCatDStato.Enabled = abilita;
			/**** ****/
			lblModoPagamento.Enabled = abilita;
			chkAcconto.Enabled = abilita;
			chkSaldo.Enabled = abilita;
			lblAvviso.Enabled = abilita;
			chkRavvedimentoOperoso.Enabled = abilita;
			chkViolazione.Enabled = abilita;
			lblProvenienza.Enabled = abilita;
			ddlProvenienze.Enabled = abilita;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.Abilita.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

		/// <summary>
		/// Abilita i controlli di violazione.
		/// </summary>
		/// <param name="abilita"></param>
		private void AbilitaViolazione(bool abilita)
		{
			lblImportoSoprattassa.Enabled = abilita;
			txtImportoSoprattassa.Enabled = abilita;
		//	cvalImportoSoprattassa.Enabled = abilita;
		//	rvalImportoSoprattassa.Enabled = abilita;
			lblPenaPecuniaria.Enabled = abilita;
			txtPenaPecuniaria.Enabled = abilita;
		//	rvalPenaPecuniaria.Enabled = abilita;
		//	cvalPenaPecuniaria.Enabled = abilita;
			lblInteressi.Enabled = abilita;
			txtInteressi.Enabled = abilita;

			lblNAtto.Enabled = abilita;
			lblDataAtto.Enabled = abilita;
			txtNAtto.Enabled = abilita ;
			txtDataAtto.Enabled = abilita;
            try { 
			if (abilita==true)
			{
				lblContrModoPag.Visible=false;
				lblContrNFab.Visible=false;
			}
			else
			{
				lblContrModoPag.Visible=true;
				lblContrNFab.Visible=true;
			}

                //	cvalInteressi.Enabled = abilita;
                //	rvalInteressi.Enabled = abilita;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.AbilitaViolazione.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
		#endregion

		/// <summary>
		/// Abilita o disabilita i controlli.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAbilita_Click(object sender, System.EventArgs e)
		{
            if (!txtDataPagamento.Enabled)//if(!lblCodiceFiscale.Enabled)
			{
				Abilita(true);
				AbilitaViolazione(chkViolazione.Checked);
			}
			else
			{
				Abilita(false);
				AbilitaViolazione(false);
			}
		}

		protected void lnkVerificaContribuente_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			HelperAnagrafica.PopUpAnagrafica(Page, "contribuente", hdIdContribuente.Value, txtIdDataAnagrafica.Value);
		}

		/// <summary>
		/// Ribalta i dati del versamento nel database di Anater.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnRibalta_Click(object sender, System.EventArgs e)
		{
			this.Ribalta();
		}

		private void lbtnUpdate_Click(object sender, System.EventArgs e)
		{
            Utility.DichManagerICI.VersamentiRow RigaVersamento = new VersamentiTable(ConstWrapper.sUsername).GetRow(this.IDVersamento);

			if(!RigaVersamento.Bonificato)
				lblBonificato.Visible = true;
			else
				lblBonificato.Visible = false;
		}

		protected void chkViolazione_CheckedChanged(object sender, System.EventArgs e)
		{
			AbilitaViolazione(chkViolazione.Checked);
		}

		protected void btnIndietro_Click(object sender, System.EventArgs e)
		{
            GoToIndietro();
		}

        private void GoToIndietro()
        {
            if (txtTypeOperation.Text.ToUpper() == "GESTIONE")
            {
                RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_RICERCA_VERSAMENTI", String.Empty), this.GetType());
            }
            else if (txtTypeOperation.Text.ToUpper() == "GESTIONEBONIFICA")
            {
                RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_VERSAMENTI_BONIFICARE", String.Empty), this.GetType());
            }
            else if (txtTypeOperation.Text.ToUpper() == "BONIFICA")
            {
                RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_BONIFICA_VERSAMENTO", String.Empty),this.GetType());
            }
        }
        private void LoadVersamento()
        {
            try
            {
                Utility.DichManagerICI.VersamentiRow RigaVersamento = new VersamentiTable(ConstWrapper.sUsername).GetRow(this.IDVersamento);
                hdIdContribuente.Value = RigaVersamento.IdAnagrafico.ToString();
                txtAnnoRiferimento.Text = RigaVersamento.AnnoRiferimento;
                //*** 20140630 - TASI ***
                if (RigaVersamento.CodTributo == Utility.Costanti.TRIBUTO_TASI)
                    optTASI.Checked = true;
                else
                    optICI.Checked = true;
                //*** ***
                if (RigaVersamento.ImportoPagato == -1)
                    txtImportoPagamento.Text = string.Empty;
                else
                    txtImportoPagamento.Text = RigaVersamento.ImportoPagato.ToString("N").Replace(".", "");

                txtDataPagamento.Text = RigaVersamento.DataPagamento.ToShortDateString();
                txtContoCorrente.Text = RigaVersamento.ContoCorrente;
                txtNumBolletino.Text = RigaVersamento.NumeroBollettino;
                txtComuneIntestatrio.Text = RigaVersamento.ComuneIntestatario;
                txtComuneUbicazImmob.Text = RigaVersamento.ComuneUbicazioneImmobile;
                txtNumFabbricatiPosseduti.Text = RigaVersamento.NumeroFabbricatiPosseduti.ToString();
                if (RigaVersamento.ImportoTerreni == -1)
                    txtImportoTerreniAgricoli.Text = string.Empty;
                else
                    txtImportoTerreniAgricoli.Text = RigaVersamento.ImportoTerreni.ToString("N");

                if (RigaVersamento.ImportoAreeFabbric == -1)
                    txtImportoAreeFabbricabili.Text = string.Empty;
                else
                    txtImportoAreeFabbricabili.Text = RigaVersamento.ImportoAreeFabbric.ToString("N");

                if (RigaVersamento.ImportoAltrifabbric == -1)
                    txtImportoAltriFabbricati.Text = string.Empty;
                else
                    txtImportoAltriFabbricati.Text = RigaVersamento.ImportoAltrifabbric.ToString("N");

                if (RigaVersamento.ImportoAbitazPrincipale == -1)
                    txtImportoAbitazPrincipale.Text = string.Empty;
                else
                    txtImportoAbitazPrincipale.Text = RigaVersamento.ImportoAbitazPrincipale.ToString("N").Replace(".", "");

                if (RigaVersamento.DetrazioneAbitazPrincipale == -1)
                    txtDetrazioniAbitazPrincipale.Text = string.Empty;
                else
                    txtDetrazioniAbitazPrincipale.Text = RigaVersamento.DetrazioneAbitazPrincipale.ToString("N");
                /**** 20120828 - IMU adeguamento per importi statali ****/
                if (RigaVersamento.ImportoTerreniStatale == -1)
                    txtImportoTerreniAgricoliStato.Text = string.Empty;
                else
                    txtImportoTerreniAgricoliStato.Text = RigaVersamento.ImportoTerreniStatale.ToString("N");

                if (RigaVersamento.ImportoAreeFabbricStatale == -1)
                    txtImportoAreeFabbricabiliStato.Text = string.Empty;
                else
                    txtImportoAreeFabbricabiliStato.Text = RigaVersamento.ImportoAreeFabbricStatale.ToString("N");

                if (RigaVersamento.ImportoAltrifabbricStatale == -1)
                    txtImportoAltriFabbricatiStato.Text = string.Empty;
                else
                    txtImportoAltriFabbricatiStato.Text = RigaVersamento.ImportoAltrifabbricStatale.ToString("N");
                if (RigaVersamento.ImportoFabRurUsoStrum == -1)
                    txtImpFabRurUsoStrum.Text = string.Empty;
                else
                    txtImpFabRurUsoStrum.Text = RigaVersamento.ImportoFabRurUsoStrum.ToString("N");
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                if (RigaVersamento.ImportoFabRurUsoStrumStatale == -1)
                    txtImpFabRurUsoStrumStato.Text = string.Empty;
                else
                    txtImpFabRurUsoStrumStato.Text = RigaVersamento.ImportoFabRurUsoStrumStatale.ToString("N");
                if (RigaVersamento.ImportoUsoProdCatD == -1)
                    txtImpUsoProdCatD.Text = string.Empty;
                else
                    txtImpUsoProdCatD.Text = RigaVersamento.ImportoUsoProdCatD.ToString("N");
                if (RigaVersamento.ImportoUsoProdCatDStatale == -1)
                    txtImpUsoProdCatDStato.Text = string.Empty;
                else
                    txtImpUsoProdCatDStato.Text = RigaVersamento.ImportoUsoProdCatDStatale.ToString("N");
                /**** ****/

                chkAcconto.Checked = RigaVersamento.Acconto;
                chkSaldo.Checked = RigaVersamento.Saldo;
                chkRavvedimentoOperoso.Checked = RigaVersamento.RavvedimentoOperoso;
                chkViolazione.Checked = RigaVersamento.Violazione;
                if (RigaVersamento.ImportoSoprattassa == -1)
                    txtImportoSoprattassa.Text = string.Empty;
                else
                    txtImportoSoprattassa.Text = RigaVersamento.ImportoSoprattassa.ToString("N");

                if (RigaVersamento.ImportoPenaPecuniaria == -1)
                    txtPenaPecuniaria.Text = string.Empty;
                else
                    txtPenaPecuniaria.Text = RigaVersamento.ImportoPenaPecuniaria.ToString("N");

                txtNAtto.Text = RigaVersamento.NumeroAttoAccertamento;
                if (RigaVersamento.DataProvvedimentoViolazione.CompareTo(DateTime.MinValue) != 0)
                {
                    txtDataAtto.Text = RigaVersamento.DataProvvedimentoViolazione.ToShortDateString();
                }
                else
                {
                    txtDataAtto.Text = String.Empty;
                }

                if (RigaVersamento.Interessi == -1)
                    txtInteressi.Text = string.Empty;
                else
                    txtInteressi.Text = RigaVersamento.Interessi.ToString("N").Replace(".", "");

                foreach (ListItem myRow in ddlProvenienze.Items)
                {
                    if (myRow.Value.CompareTo(RigaVersamento.IDProvenienza.ToString()) == 0)
                    {
                        myRow.Selected = true;
                        break;
                    }
                }

                if (!RigaVersamento.Bonificato)
                    lblBonificato.Visible = true;

                if (RigaVersamento.DataRiversamento.CompareTo(DateTime.MinValue) != 0)
                {
                    TxtDataRiversamento.Text = RigaVersamento.DataRiversamento.ToShortDateString();
                }
                else
                {
                    TxtDataRiversamento.Text = String.Empty;
                }

                // prendo i dati del contribuente di quel versamento
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                if (ConstWrapper.HasPlainAnag)
                    ifrmAnag.Attributes.Add("src", "../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                else
                {
                    DettaglioAnagrafica oDettAnag = HelperAnagrafica.GetDatiPersona(long.Parse(hdIdContribuente.Value));
                    Session["contribuente"] = oDettAnag;
                    this.Ribalta();
                }
                Abilita(false);
                btnElimina.Enabled = true;
                btnElimina.Attributes.Add("onclick", "return confirm('Sei sicuro di voler eliminare il versamento?')");

                if (!RigaVersamento.Bonificato)
                    lblBonificato.Visible = true;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.LoadVersamento.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        private void Ribalta()
        {
            try
            {
                DettaglioAnagrafica oDettaglioAnagrafica;
                if (Session["contribuente"] != null)
                {
                    oDettaglioAnagrafica = (DettaglioAnagrafica)(Session["contribuente"]);
                    txtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale;
                    txtPIVA.Text = oDettaglioAnagrafica.PartitaIva;
                    txtCognome.Text = oDettaglioAnagrafica.Cognome;
                    txtNome.Text = oDettaglioAnagrafica.Nome;
                    hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                    txtIdDataAnagrafica.Value = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA.ToString();
                    if (oDettaglioAnagrafica.Sesso == "")
                    {
                        rdbMaschio.Checked = false;
                        rdbFemmina.Checked = false;
                        rdbGiuridica.Checked = false;
                    }
                    if (oDettaglioAnagrafica.Sesso == "M")
                        rdbMaschio.Checked = true;
                    if (oDettaglioAnagrafica.Sesso == "F")
                        rdbFemmina.Checked = true;
                    if (oDettaglioAnagrafica.Sesso == "G")
                        rdbGiuridica.Checked = true;

                    if (oDettaglioAnagrafica.DataNascita != "00/00/1900")
                    {
                        txtDataNasc.Text = oDettaglioAnagrafica.DataNascita;
                    }
                    else
                    {
                        txtDataNasc.Text = string.Empty;
                    }
                    txtComNasc.Text = oDettaglioAnagrafica.ComuneNascita;
                    txtProvNasc.Text = oDettaglioAnagrafica.ProvinciaNascita;
                    txtViaRes.Text = oDettaglioAnagrafica.ViaResidenza;
                    txtNumCivRes.Text = oDettaglioAnagrafica.CivicoResidenza;
                    txtIntRes.Text = oDettaglioAnagrafica.InternoCivicoResidenza;
                    txtScalaRes.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;
                    txtComuneRes.Text = oDettaglioAnagrafica.ComuneResidenza;
                    txtProvRes.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                    txtCapRes.Text = oDettaglioAnagrafica.CapResidenza;
                    txtEsponenteCivico.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Versamenti.AbilitaViolazione.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
    }
}