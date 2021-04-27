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
using Business;
using ElaborazioneDatiStampeInterface;
using System.Text;
using log4net;
using System.Data.SqlClient; 
using System.Configuration;

namespace DichiarazioniICI.ElaborazioneDocumenti
{
    /// <summary>
    /// Pagina per l'elaborazione dei documenti.
    /// Contiene il riepilogo elaborazione, i parametri di produzione documenti, le funzioni della comandiera e il frame per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ElaborazioneDocumenti :BasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ElaborazioneDocumenti));
		
		//protected RIDataGrid.RibesGrid grdDaElaborare;
		//protected System.Web.UI.WebControls.Label lblNoRicerca;
		//protected System.Web.UI.WebControls.Label lblErrore;
		//*** 201511 - template documenti per ruolo ***
		//private delegate void StampaMassivaICIAsync(string Tributo, int[,] CodContribuente, int AnnoRiferimento, string CodiceEnte, string IdFlussoRuolo, string[] TipologieEsclusione, string Connessione, string ConnessioneRepository, string ConnessioneAnagrafica, int ContribuentiPerGruppo, string TipoElaborazione, string ImpostazioniBollettini, string TipoCalcolo, string TipoBollettino, bool bIsStampaBollettino, bool bCreaPDF, bool nettoVersato, int nDecimal, bool bSendByMail);
        private delegate void StampaMassivaICIAsync(string DBType, string Tributo, int[,] CodContribuente, int AnnoRiferimento, string CodiceEnte, string IdFlussoRuolo, string[] TipologieEsclusione, string Connessione, string ConnessioneRepository, string ConnessioneAnagrafica, string PathTemplate, string PathVirtualTemplate, int ContribuentiPerGruppo, string TipoElaborazione, string ImpostazioniBollettini, string TipoCalcolo, string TipoBollettino, bool bIsStampaBollettino, bool bCreaPDF, bool nettoVersato, int nDecimal, bool bSendByMail);
        private int IdFlussoRuolo;
		/// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		    /// <revisionHistory>
    /// <revision date="04/07/2012">
    /// <strong>IMU</strong>
    /// passaggio tributo da ICI a IMU
    /// </revision>
    /// </revisionHistory>
protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            try{
			    string NOTE="";
			    string connessioneRepository=ConstWrapper.StringConnectionOPENgov;
                string sAnno = "";
                //*** 20140509 - TASI ***
                if (Request["AnnoRiferimento"] != null)
                {
                    sAnno = Request["AnnoRiferimento"].ToString();
                }
                txtAnno.Text = sAnno;
                if (Request["COD_TRIBUTO"] != null)
                {
                    if (Request["COD_TRIBUTO"].ToString() == ConstWrapper.CodiceTributo)
                    {
                        chkTASI.Checked = false; chkInquilino.Checked = false; chkInquilino.Enabled = false; chkProp.Enabled = false;
                    }
                    else if (Request["COD_TRIBUTO"].ToString() == Utility.Costanti.TRIBUTO_TASI)
                    {
                        chkICI.Checked = false;
                    }
                }
                if (Request["bNettoVersato"] != null)
                {
                    rdbCalcoloNetto.Checked = bool.Parse(Request["bNettoVersato"].ToString());
                }
                //*** ***
                int iInElab = InElaborazione(connessioneRepository, sAnno, ConstWrapper.CodiceEnte, ConstWrapper.CodiceTributo, ref NOTE, ref IdFlussoRuolo);
                if (Request["IdFlussoRuolo"] != null)
                {
                    txtIdFlussoRuolo.Value = Request["IdFlussoRuolo"].ToString();
                }
                else
                {
                    txtIdFlussoRuolo.Value = IdFlussoRuolo.ToString();
                }
                switch (iInElab)
                {
				    case 0:
					    //Elaborazione terminata con successo
					    lblElaborazioniEffettuate.Text="Elaborazione terminata con successo";
                        lnkScaricaDocEffettiviElab.Attributes.Add("style", "");
					    break;
				    case 1: 
					    //Elaborazione in corso
                        //Response.Redirect ("Elaborazioneincorso.aspx");
                        lblElaborazioniEffettuate.Text = "Elaborazione documenti in corso....Attendere prego...";
                        lnkScaricaDocEffettiviElab.Visible=false;
                        break;
				    case 2: 
					    //Elaborazione terminata con errori
					    lblElaborazioniEffettuate.Text="Elaborazione terminata con errore";
                        lnkScaricaDocEffettiviElab.Visible = false;
                        //*** 20120704 - IMU ***
					    log.Error ("Elaborazione Stampe terminata con errore. ENTE:"+ConstWrapper.CodiceEnte+", ANNO:"+Request["AnnoRiferimento"].ToString()+", TRIBUTO:8852, ERRORE: "+NOTE);
					    break;
			    } 
    						
			    if (!IsPostBack)
			    {
                    txtNumContrib.Text = ConstWrapper.nMaxDocPerFile.ToString();
                    DataTable TpRendita = new Database.Tipo_RenditaTable(ConstWrapper.sUsername).List();

				    if (TpRendita.Rows.Count > 0 )
				    {
					    GrdTipoRendita.DataSource = TpRendita;
					    GrdTipoRendita.DataBind();
				    }

				    // prendo tutti i contribuenti che devono ancora essere stampati...
				    // flag --> Elaborato = 0
                    //*** 20150430 - TASI Inquilino ***
                    string Tributo = "";
                    if (chkICI.Checked == true && chkTASI.Checked == false)
                        Tributo = ConstWrapper.CodiceTributo;
                    else if (chkICI.Checked == false && chkTASI.Checked == true)
                        Tributo = Utility.Costanti.TRIBUTO_TASI;
                    string sTipoTasiXStampa = "";
                    if (chkProp.Checked == false && chkInquilino.Checked == true)
                        sTipoTasiXStampa = "I";
                    if (chkInquilino.Checked == false && chkProp.Checked == true)
                        sTipoTasiXStampa = "P";
                    DataTable TblDaStampare = new Database.TpSituazioneFinaleIci().ListContribuentiPerElaborazioneICI("", "", "", int.Parse(Request["AnnoRiferimento"].ToString()), ConstWrapper.CodiceEnte, Database.TpSituazioneFinaleIci.TipoOrdinamento.CognomeNome,Tributo, sTipoTasiXStampa);
                    //*** ***
                    
				    // variabile di sessione che viene utilizzata dall'iframe avanzamentoelaborazione
				    Session["TblDaStampare"] = TblDaStampare;
			    }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.Page_File.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
               
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

		}
		#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void radioEffettivo_CheckedChanged(object sender, System.EventArgs e)
		{
            // se clicco sul radio button effettivo
            // abilito il check box per la selezione di tutte le posizioni da stampare.
            try { 
			if (radioEffettivo.Checked.CompareTo(true) == 0)
			{
				chkSelTutti.Visible = true;
                chkSendMail.Visible = true; ; chkSendMail.Checked = true;
			}
			else
			{
				chkSelTutti.Checked = false;
				chkSelTutti.Visible = false;
                chkSendMail.Visible = false; chkSendMail.Checked = false;
			}
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.radioEffettivo_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="senter"></param>
        /// <param name="e"></param>
		protected void chkSelTutti_CheckedChanged(object senter, System.EventArgs e)
		{
            try{
			    // controllo se è stato chekkato
			    if (chkSelTutti.Checked.CompareTo(true) == 0)
			    {
				    // imposto sulla variabile di sessione dei risultati
				    if (Session["TblDaStampare"] != null)
				    {
					    DataTable TblCheckTutti = (DataTable)Session["TblDaStampare"];	
    				
					    foreach (DataRow oDr in TblCheckTutti.Rows)
					    {
						    oDr["INCLUDI"] = 1;
					    }				
				    }
			    }
			    else
			    {
				    // imposto sulla variabile di sessione dei risultati
				    if (Session["TblDaStampare"] != null)
				    {
					    DataTable TblCheckTutti = (DataTable)Session["TblDaStampare"];	
    				
					    foreach (DataRow oDr in TblCheckTutti.Rows)
					    {
						    oDr["INCLUDI"] = 0;
					    }
				    }				
			    }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.chkSelTutti_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void radioProva_CheckedChanged(object sender, System.EventArgs e)
		{
            try { 
			if (radioProva.Checked == true)
			{
                chkSelTutti.Checked = false;
                chkSelTutti.Visible = false;
                chkSendMail.Visible = false; chkSendMail.Checked = false;
            }
            else
            {
                chkSelTutti.Visible = true;
                chkSendMail.Visible = true; ; chkSendMail.Checked = true;
            }

			if (Session["TblDaStampare"] != null)
			{
				DataTable TblCheckTutti = (DataTable)Session["TblDaStampare"];	
				
				foreach (DataRow oDr in TblCheckTutti.Rows)
				{
					oDr["INCLUDI"] = 0;
				}
			}
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.radioProva_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnRicerca_Click(object sender, System.EventArgs e)
		{
            Search();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="05/11/2020">
        /// devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
        /// </revision>
        /// </revisionHistory>
        protected void btnElabora_Click(object sender, System.EventArgs e)
        {
            try
            {
                RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] oGruppoRet;
                bool nettoVersato = true;
                string TipoElaborazione = "PROVA";
                string TipoCalcolo = "IMU";
                string TipoBollettino = "F24";
                int nDecimal = 2;
                bool bIsStampaBollettino = true;
                bool bCreaPDF = false;
                int[,] ContribuentiDaElaborare = GetArrayContribuenti();
                string[] TipologieEsclusione = GetTipologieDaEscludere();

                log.Debug("inizio stampa");
                IdFlussoRuolo = int.Parse(txtIdFlussoRuolo.Value);
                //*** 20130422 - aggiornamento IMU ***
                if (Request["bNettoVersato"] != null)
                {
                    if (Request["bNettoVersato"].ToString() == string.Empty)
                    {
                        if (int.Parse(Request["AnnoRiferimento"].ToString()) < 2012)
                        {
                            nettoVersato = false;
                            TipoCalcolo = "ICI";
                            TipoBollettino = "123";
                        }
                    }
                    else
                    {
                        nettoVersato = bool.Parse(Request["bNettoVersato"].ToString());
                    }
                }
                log.Debug("controllo contribuenti da elaborare");
                if (ContribuentiDaElaborare.Length > 0)
                {
                    //*** 201511 - template documenti per ruolo ***
                    try//scarico template dalla tabella dei ruoli elaborati
                    {
                        ObjTemplateDoc myTemplateDoc = new ObjTemplateDoc();
                        myTemplateDoc.myStringConnection = ConstWrapper.StringConnectionOPENgov;
                        myTemplateDoc.IdEnte = ConstWrapper.CodiceEnte;
                        myTemplateDoc.IdTributo = ConstWrapper.CodiceTributo;
                        myTemplateDoc.IdRuolo = int.Parse(txtIdFlussoRuolo.Value);
                        myTemplateDoc.Load();
                        string PathFileTemplate = ConstWrapper.PathStampe;
                        PathFileTemplate += ObjTemplateDoc.ATTOTemplate + "\\";
                        PathFileTemplate += ObjTemplateDoc.Dominio_ICI + "\\";
                        PathFileTemplate += ConstWrapper.DescrizioneEnte + "\\";
                        PathFileTemplate += myTemplateDoc.FileName;
                        System.IO.File.WriteAllBytes(PathFileTemplate, myTemplateDoc.PostedFile);
                    }
                    catch (Exception Err)
                    {
                         log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.btnElaboraClick.errore: ", Err);
                    }
                    //*** ***
                    // devo creare l'array di codici contribuente per chiamare il servizio di elaborazione massiva
                    if (radioEffettivo.Checked == true)
                    {
                        log.Debug("devo richiamare effettivo remObject.ElaborazioneMassivaDocumenti");
                        
                        lblElaborazioniEffettuate.Text = "Elaborazione documenti in corso....Attendere prego...";
                        lnkScaricaDocEffettiviElab.Visible = false;
                        TipoElaborazione = "EFFETTIVO";
                        StampaMassivaICIAsync del = new StampaMassivaICIAsync(ChiamaElaborazioneAsincrona);
                        del.BeginInvoke(ConstWrapper.DBType, ConstWrapper.CodiceTributo, ContribuentiDaElaborare, int.Parse(Request["AnnoRiferimento"].ToString()), ConstWrapper.CodiceEnte, IdFlussoRuolo.ToString(), TipologieEsclusione, ConstWrapper.StringConnection, ConstWrapper.StringConnectionOPENgov, ConstWrapper.StringConnectionAnagrafica,ConstWrapper.PathStampe, ConstWrapper.PathVirtualStampe, int.Parse(txtNumContrib.Text.ToString()), TipoElaborazione, GetImpostazioniBollettino(), TipoCalcolo, TipoBollettino, bIsStampaBollettino, bCreaPDF, nettoVersato, nDecimal, chkSendMail.Checked, null, null);
                    }
                    else
                    {
                        log.Debug("devo richiamare  remObject.ElaborazioneMassivaDocumenti");
                        // faccio partire l'elaborazione chiamo il servizio di elaborazione delle stampe massive.
                        Type typeofRI = typeof(IElaborazioneStampeICI);
                        IElaborazioneStampeICI remObject = (IElaborazioneStampeICI)Activator.GetObject(typeofRI, ConstWrapper.UrlServizioElaborazioneDocumentiICI.ToString());
                        /**** 201810 - Calcolo puntuale ****///*** 201511 - template documenti per ruolo ***
                        oGruppoRet = remObject.ElaborazioneMassivaDocumenti(ConstWrapper.DBType, ConstWrapper.CodiceTributo, ContribuentiDaElaborare, int.Parse(Request["AnnoRiferimento"].ToString()), ConstWrapper.CodiceEnte, IdFlussoRuolo.ToString(), TipologieEsclusione, ConstWrapper.StringConnection, ConstWrapper.StringConnectionOPENgov, ConstWrapper.StringConnectionAnagrafica, ConstWrapper.PathStampe, ConstWrapper.PathVirtualStampe, int.Parse(txtNumContrib.Text.ToString()), TipoElaborazione, GetImpostazioniBollettino(), TipoCalcolo, TipoBollettino, bIsStampaBollettino, bCreaPDF, nettoVersato, nDecimal, chkSendMail.Checked, false, string.Empty,ConstWrapper.CodiceTributo);
                        //*** ***
                        if (oGruppoRet != null)
                        {
                            // se viene restituito il gruppo documenti apro il popup
                            Session["StampaPuntuale"] = oGruppoRet[0];
                            ApriPopUpDownloadDocumenti();
                        }
                        else
                        {
                            System.Text.StringBuilder oStrBuild = new System.Text.StringBuilder();
                            oStrBuild.Append("GestAlert('a', 'warning', '', '', 'Non sono stati elaborati correttamente i documenti selezionati.')");
                            RegisterScript(oStrBuild.ToString(),this.GetType());
                        }
                    }
                }
                else
                {
                    // se non sono stati selezionati contribuenti per l'elaborazione do il messaggio javascript che l'elaborazione non è possibile.
                    System.Text.StringBuilder oStrBuild = new System.Text.StringBuilder();
                    oStrBuild.Append("GestAlert('a', 'warning', '', '', 'Per effetture elaborazioni di documenti è necessario selezionare le posizioni da elaborare!')");
                    RegisterScript(oStrBuild.ToString(),this.GetType());
                }
                //*** ***
                DivAttesa.Style.Add("display", "none");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.btnElaboraClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[,] GetArrayContribuenti()
        {
            try
            {
                // recupero il datatable che mi permette di selezionare i contribuenti per l'elaborazione
                DataTable DtElaborazione = (DataTable)Session["TblDaStampare"];
                DataRow[] myRows = DtElaborazione.Select("INCLUDI=1");

                int[,] IdDocToElab = new int[2, myRows.Length];
                int nList=-1;
                // ciclo il datatable e mi prendo i contribuenti selezionati
                foreach (DataRow oDr in myRows)
                {
                    nList++;
                    IdDocToElab[0, nList] = int.Parse(oDr["IDDOC"].ToString());
                    IdDocToElab[1, nList] = int.Parse(oDr["CODCONTRIBUENTE"].ToString());
                }
                return IdDocToElab;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.GetArrayContribuenti.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                string sScript = "";
		        sScript += "parent.Comandi.location.href='../CalcoloICI/CCalcoloICIMassivo.aspx';";
		        sScript += "parent.Visualizza.location.href = '../CalcoloICI/CalcoloICIMassivo.aspx';";
                sScript += "";
                RegisterScript(sScript,this.GetType());
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.btnClose_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
              
            }
        }
        //*** ***

		private string[] GetTipologieDaEscludere()
		{
			try{
			    ArrayList ArrListTipEsclusione = new ArrayList();

			    foreach (GridViewRow oItem in GrdTipoRendita.Rows)
			    {		
				    if (oItem.FindControl("chkEsclusione") != null)
				    {
					    CheckBox oCheck = (CheckBox)oItem.FindControl("chkEsclusione");

					    // se trovo dei checkbox selezionati aggiungo la tipologia all'array list.
					    if (oCheck.Checked)
					    {
						    // Aggiungo
						    ArrListTipEsclusione.Add(oItem.Cells[1].Text.ToString());
					    }
				    }				
			    }

			    return (string[])ArrListTipEsclusione.ToArray(typeof(string));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.GetTipologieDaEscludere.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;
            }
		}


        //*** 20140509 - TASI ***
        //private void ChiamaElaborazioneAsincrona(int[] CodContribuente, int AnnoRiferimento, string CodiceEnte, string[] TipologieEsclusione, string ConnessioneICI, string ConnessioneRepository, string ConnessioneAnagrafica, int ContribuentiPerGruppo)
        //{
        //    try
        //    {
        //        // faccio partire l'elaborazione
        //        // chiamo il servizio di elaborazione delle stampe massive.
        //        Type typeofRI = typeof(IElaborazioneStampeICI);
        //        bool nettoVersato = true;

        //        //*** 20130422 - aggiornamento IMU ***
        //        if (Request["bNettoVersato"].ToString()==string.Empty)
        //        {
        //            if (AnnoRiferimento < 2012)
        //                nettoVersato = false;
        //        }
        //        else
        //        {
        //            nettoVersato=bool.Parse(Request["bNettoVersato"].ToString());
        //        }
        //        //*** ***

        //        IElaborazioneStampeICI remObject = (IElaborazioneStampeICI)Activator.GetObject(typeofRI, ConstWrapper.UrlServizioElaborazioneDocumentiICI.ToString());
        //        remObject.ElaborazioneMassivaDocumentiICI(CodContribuente, AnnoRiferimento,CodiceEnte, TipologieEsclusione, ConnessioneICI, ConnessioneRepository, ConnessioneAnagrafica, ContribuentiPerGruppo, "EFFETTIVO", GetImpostazioniBollettino(),true,false, nettoVersato);
        //    }
        //    catch (Exception ex)
        //    {
        //         log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.ChiamaElaborazioneAsincrona.errore: ", Err);
        //       Response.Redirect("../../PaginaErrore.aspx");
        //        throw ex;
        //    }
        //}
        //*** 201511 - template documenti per ruolo ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBType"></param>
        /// <param name="Tributo"></param>
        /// <param name="CodContribuente"></param>
        /// <param name="AnnoRiferimento"></param>
        /// <param name="CodiceEnte"></param>
        /// <param name="IdFlussoRuolo"></param>
        /// <param name="TipologieEsclusione"></param>
        /// <param name="Connessione"></param>
        /// <param name="ConnessioneRepository"></param>
        /// <param name="ConnessioneAnagrafica"></param>
        /// <param name="PathTemplate"></param>
        /// <param name="PathVirtualTemplate"></param>
        /// <param name="ContribuentiPerGruppo"></param>
        /// <param name="TipoElaborazione"></param>
        /// <param name="ImpostazioniBollettini"></param>
        /// <param name="TipoCalcolo"></param>
        /// <param name="TipoBollettino"></param>
        /// <param name="bIsStampaBollettino"></param>
        /// <param name="bCreaPDF"></param>
        /// <param name="nettoVersato"></param>
        /// <param name="nDecimal"></param>
        /// <param name="bSendByMail"></param>
        /// <revisionHistory>
        /// <revision date="05/11/2020">
        /// devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
        /// </revision>
        /// </revisionHistory>
        private void ChiamaElaborazioneAsincrona(string DBType, string Tributo, int[,] CodContribuente, int AnnoRiferimento, string CodiceEnte, string IdFlussoRuolo, string[] TipologieEsclusione, string Connessione, string ConnessioneRepository, string ConnessioneAnagrafica, string PathTemplate, string PathVirtualTemplate, int ContribuentiPerGruppo, string TipoElaborazione, string ImpostazioniBollettini, string TipoCalcolo, string TipoBollettino, bool bIsStampaBollettino, bool bCreaPDF, bool nettoVersato, int nDecimal, bool bSendByMail)
        {
            try
            {
                // faccio partire l'elaborazione
                // chiamo il servizio di elaborazione delle stampe massive.
                Type typeofRI = typeof(IElaborazioneStampeICI);
                IElaborazioneStampeICI remObject = (IElaborazioneStampeICI)Activator.GetObject(typeofRI, ConstWrapper.UrlServizioElaborazioneDocumentiICI.ToString());
                /**** 201810 - Calcolo puntuale ****///*** 201511 - template documenti per ruolo ***
                remObject.ElaborazioneMassivaDocumenti(ConstWrapper.DBType, Tributo, CodContribuente, AnnoRiferimento, CodiceEnte, IdFlussoRuolo, TipologieEsclusione, Connessione, ConnessioneRepository, ConnessioneAnagrafica, PathTemplate, PathVirtualTemplate, ContribuentiPerGruppo, TipoElaborazione, ImpostazioniBollettini, TipoCalcolo, TipoBollettino, bIsStampaBollettino, bCreaPDF, nettoVersato, nDecimal, bSendByMail, false, string.Empty,Tributo);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.btnClose_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }
        //*** ***

		private void ApriPopUpDownloadDocumenti()
		{
			StringBuilder strBuilder = new StringBuilder();

			strBuilder.Append("");
			strBuilder.Append("ApriPopUpStampaDocumenti();");
			strBuilder.Append("");
			
			RegisterScript(strBuilder.ToString(),this.GetType());
		}

        //*** 20140509 - TASI ***
        //private int InElaborazione(string ConnessioneRepository, string ANNO, string COD_ENTE, string COD_TRIBUTO, ref string sNOTE)
        //{
        //    Utility.DBManager _oDbManagerRepository = null;
        //    SqlCommand cmdMyCommand = new SqlCommand();
        //    SqlDataReader objDR;
        //    bool bELABORAZIONE=false, bERRORI=false;

        //    try
        //    {
        //        _oDbManagerRepository = new Utility.DBManager(ConnessioneRepository);
        //        cmdMyCommand.CommandText="select * from TP_TASK_REPOSITORY";
        //        cmdMyCommand.CommandText += " where COD_ENTE=@COD_ENTE";
        //        cmdMyCommand.CommandText += " and COD_TRIBUTO=@COD_TRIBUTO";
        //        cmdMyCommand.CommandText += " and ANNO=@ANNO";
        //        cmdMyCommand.CommandText += " order by id_task_repository desc ,DATA_ELABORAZIONE desc";

        //        cmdMyCommand.Parameters.Add(new SqlParameter("@COD_ENTE", SqlDbType.NVarChar)).Value = COD_ENTE;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@COD_TRIBUTO", SqlDbType.NVarChar)).Value = COD_TRIBUTO;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = ANNO;


        //        objDR=_oDbManagerRepository.GetDataReader (cmdMyCommand);
        //        if (objDR.HasRows)
        //        {
        //            objDR.Read(); 
        //            bELABORAZIONE=Boolean.Parse(objDR["ELABORAZIONE"].ToString());
        //            bERRORI=Boolean.Parse (objDR["ERRORI"].ToString() );
        //            sNOTE=objDR["NOTE"] == DBNull.Value? "" : objDR["NOTE"].ToString();

        //        }
        //        objDR.Close();
        //        if (!bELABORAZIONE)
        //        {
        //            if (bERRORI)
        //                return 2;
        //            else
        //                return 0;
        //        }
        //        else
        //            return 1;

        //    }
        //    catch(Exception Ex)
        //    { log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.InElaborazione.errore: ", Err);
        //Response.Redirect("../../PaginaErrore.aspx");
        //        sNOTE=Ex.Message;
        //        return -1;
				
        //    }
        //}
        private int InElaborazione(string ConnessioneRepository, string ANNO, string COD_ENTE, string COD_TRIBUTO, ref string sNOTE, ref int IdFlussoRuolo)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            SqlDataReader objDR;
            bool bELABORAZIONE = false, bERRORI = false; ; IdFlussoRuolo = -1;

            try
            {
                cmdMyCommand.Connection = new SqlConnection(ConnessioneRepository);
                cmdMyCommand.Connection.Open();
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = "prc_TP_TASK_REPOSITORY_S";
                cmdMyCommand.Parameters.Add(new SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = COD_ENTE;
                cmdMyCommand.Parameters.Add(new SqlParameter("@TRIBUTO", SqlDbType.NVarChar)).Value = COD_TRIBUTO;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = ANNO;
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                objDR = cmdMyCommand.ExecuteReader();
                if (objDR.HasRows)
                {
                    objDR.Read();
                    bELABORAZIONE = Boolean.Parse(objDR["ELABORAZIONE"].ToString());
                    bERRORI = Boolean.Parse(objDR["ERRORI"].ToString());
                    sNOTE = objDR["NOTE"] == DBNull.Value ? "" : objDR["NOTE"].ToString();
                    IdFlussoRuolo = objDR["IDFLUSSORUOLOTARSU"] == DBNull.Value ? -1 : int.Parse(objDR["IDFLUSSORUOLOTARSU"].ToString());
                    if (!bELABORAZIONE)
                    {
                        if (bERRORI)
                            return 2;
                        else
                            return 0;
                    }
                    else
                        return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception Ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.InElaborazione.errore: ", Ex);
                Response.Redirect("../../PaginaErrore.aspx");
                sNOTE = Ex.Message;
                return -1;
            }
            finally
            {
          cmdMyCommand.Connection.Close();
                cmdMyCommand.Dispose();
            }
        }

        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnElimina_Click(object sender, System.EventArgs e)
		{
		    try{
			    IElaborazioneStampeICI remObject = (IElaborazioneStampeICI)Activator.GetObject(typeof(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstWrapper.UrlServizioElaborazioneDocumentiICI);

                bool RisultatoElaborazioni = remObject.EliminaElaborazioni(ConstWrapper.DBType, ConstWrapper.CodiceEnte, Utility.Costanti.TRIBUTO_ICI, int.Parse(Request["AnnoRiferimento"].ToString()), ConstWrapper.StringConnectionOPENgov.ToString());
			    if (RisultatoElaborazioni != false)
			    {
				    // messaggio di avvenuta cancellazione
				    StringBuilder strOk = new StringBuilder();
				    strOk.Append("");
				    strOk.Append("GestAlert('a', 'success', '', '', 'Eliminazione dell'elaborazione effettuata correttamente!')");
				    strOk.Append("");
				    RegisterScript(strOk.ToString(),this.GetType());
			    }
			    else
			    {
				    // messaggio di errore cancellazione
				    StringBuilder strOk = new StringBuilder();
				    strOk.Append("");
				    strOk.Append("GestAlert('a', 'warning', '', '', 'Eliminazione dell'elaborazione non effettuata!')");
				    strOk.Append("");
				    RegisterScript(strOk.ToString(),this.GetType());
			    }
            }
            catch (Exception Ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.btnElimina_Click.errore: ", Ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private string GetImpostazioniBollettino()
		{
            try { 
			if (radioBollettiniSenzaImporti.Checked) return "BOLLETTINISENZAIMPORTI";
			if (radioNoBollettini.Checked) return "NOBOLLETTINI";
            //*** 20140509 - TASI ***
            if (radioSoloAcconto.Checked) return "SOLOACCONTO";
            if (radioSoloSaldo.Checked) return "SOLOSALDO";
            //*** ***
			return "BOLLETTINISTANDARD";
           
            }
            catch (Exception Ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.GetImpostazioniBollettino.errore: ", Ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Ex;
            }
}
        //*** 20150430 - TASI Inquilino ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Tributo_CheckedChanged(object sender, System.EventArgs e)
        {
            string Tributo = "";
            try { 
            if (chkICI.Checked == true && chkTASI.Checked == false){
                Tributo = ConstWrapper.CodiceTributo;
                chkTASI.Checked = false; chkInquilino.Checked = false; chkInquilino.Enabled = false; chkProp.Enabled = false;
            }
            else if (chkICI.Checked == false && chkTASI.Checked == true)
            {
                Tributo = Utility.Costanti.TRIBUTO_TASI;
                chkICI.Checked = false;
            }
            
            Search();

            }
            catch (Exception Ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.Tributo_CheckedChanged.errore: ", Ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Ex;
            }
        }

        private void Search()
        {
            try
            {
                // recupero i parametri di ricerca inseriti ed effettuo la ricerca vera e propria
                string NominativoDa = txtNominativoDa.Text.Trim().CompareTo(string.Empty) == 0 ? null : txtNominativoDa.Text.Trim();
                string NominativoA = txtNominativoA.Text.Trim().CompareTo(string.Empty) == 0 ? null : txtNominativoA.Text.Trim();

                Database.TpSituazioneFinaleIci.TipoOrdinamento TipoOrdinamento;
                if (radioNominativo.Checked)
                {
                    TipoOrdinamento = Database.TpSituazioneFinaleIci.TipoOrdinamento.CognomeNome;
                }
                else
                {
                    TipoOrdinamento = Database.TpSituazioneFinaleIci.TipoOrdinamento.Indirizzo;
                }
                //*** 20150430 - TASI Inquilino ***
                string Tributo = "";
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;
                string sTipoTasiXStampa = "";
                if (chkProp.Checked == false && chkInquilino.Checked == true)
                    sTipoTasiXStampa = "I";
                if (chkInquilino.Checked == false && chkProp.Checked == true)
                    sTipoTasiXStampa = "P";
                DataTable TblDaStampare = new Database.TpSituazioneFinaleIci().ListContribuentiPerElaborazioneICI(null, NominativoDa, NominativoA, int.Parse(Request["AnnoRiferimento"].ToString()), ConstWrapper.CodiceEnte, TipoOrdinamento, Tributo, sTipoTasiXStampa);
                //*** ***

                // variabile di sessione che viene utilizzata dall'iframe avanzamentoelaborazione
                Session["TblDaStampare"] = TblDaStampare;
                DivAttesa.Style.Add("display", "none");
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ElaborazioneDocumenti.Search.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** ***
    }
}
