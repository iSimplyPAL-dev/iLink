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
using log4net;
using DichiarazioniICI.Database;
using Business;
using System.Configuration;
using Ribes;
using ComPlusInterface;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per l'elaborazione del calcolo massivo IMU/TASI.
    /// Contiene i parametri di calcolo, le funzioni della comandiera e i frame per la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Analisi eventi</em>
    /// </revision>
    /// </revisionHistory>
    public partial class CalcoloICIMassivo : BaseEnte
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CalcoloICIMassivo));
		HtmlControl myFrame;
        /// <summary>
        /// 
        /// </summary>
        protected string Tributo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="08/10/2020">Inserito contatore a video di progressione elaborazione</revision></revisionHistory>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            try
            {
                lblTitolo.Text = Business.ConstWrapper.DescrizioneEnte;
                if (!IsPostBack)
                {
                    string strAnno = String.Empty;
                    ddlAnnoRiferimento.DataBind();

                    ListItem myitem;

                    myitem = new ListItem();
                    myitem.Value = "";
                    myitem.Text = "...";
                    ddlAnnoRiferimento.Items.Add(myitem);

                    strAnno = ddlAnnoRiferimento.SelectedItem.Value;
                   
                    lblLinkCalcolo.Attributes.Add("onMouseOver", "this.style.cursor='hand';");
                    lblLinkCalcolo.Attributes.Add("OnCLick", "return LinkCalcoloICI(''," + strAnno + ")");
                    if (chkICI.Checked == true && chkTASI.Checked == false)
                        Tributo = ConstWrapper.CodiceTributo;
                    else if (chkICI.Checked == false && chkTASI.Checked == true)
                        Tributo = Utility.Costanti.TRIBUTO_TASI;
                    //*** 201511 - template documenti per ruolo ***
                    hdIdRuolo.Value = new TpSituazioneFinaleIci().getIdRuolo(ConstWrapper.CodiceEnte,ddlAnnoRiferimento.SelectedItem.Text, Tributo).ToString();
                    //*** ***
                    //*** 20140509 - TASI ***
                    LoadFrame();
                    //*** ***
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Elaborazioni, "CalcoloMassivo", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte,-1);
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.Page_Load.errore: ", Err);
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		#region METODI
		/// <summary>
		/// Ritorna una DataView valorizzata con l'elenco degli anni di riferimento.
		/// </summary>
		protected DataView GetAnniAliquote()
		{
			DataView Vista = new Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
			Vista.Sort = "ANNO DESC";
			return Vista;
		}
        #endregion

        /// <summary>
        /// Pulsante per il calcolo massivo IMU/TASI; blocco calcolo se mancano i dati obbligatori; il calcolo viene fatto richiamando il servizio esterno in modalità asincrona.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="09/05/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnCalcoloICI_Click(object sender, System.EventArgs e)
        {
            try
            {
                Hashtable objHashTable = new Hashtable();
                string strConnectionStringOPENgovICI;
                string strConnectionStringOPENgovProvvedimenti;
                string strConnectionStringAnagrafica;
                string strConnectionStringOPENgovTerritorio;
                string strConnectionStringOPENgovCatasto;
                bool bVersatoNelDovuto = chkVersatoNelDovuto.Checked;
                bool bCalcolaArrotondamento = chkArrotondamento.Checked;
                objHashTable.Add("CodENTE", ConstWrapper.CodiceEnte);
                strConnectionStringOPENgovICI = ConstWrapper.StringConnection;
                strConnectionStringAnagrafica = ConstWrapper.StringConnectionAnagrafica;
                strConnectionStringOPENgovProvvedimenti = "";
                strConnectionStringOPENgovTerritorio = "";
                strConnectionStringOPENgovCatasto = "";

                //*** 20120629 - IMU ***	
                //if (System.Configuration.ConfigurationManager.AppSettings["connectionStringSQLOPENgov"]!=null)
                objHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstWrapper.StringConnectionOPENgov.ToString());
                //*** ***
                objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", strConnectionStringOPENgovICI);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", strConnectionStringOPENgovProvvedimenti);
                objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVTERRITORIO", strConnectionStringOPENgovTerritorio);
                objHashTable.Add("CONNECTIONSTRINGOPENGOVCATASTO", strConnectionStringOPENgovCatasto);
                objHashTable.Add("USER", ConstWrapper.sUsername);
                objHashTable.Add("COD_TRIBUTO",ConstWrapper.CodiceTributo);

                Tributo = string.Empty;
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;
                objHashTable.Add("TRIBUTOCALCOLO", Tributo);
                //*** 20150430 - TASI Inquilino ***
                if (optTASIProp.Checked == true)
                    objHashTable.Add("TASIAPROPRIETARIO", 1);
                else
                    objHashTable.Add("TASIAPROPRIETARIO", 0);
                //*** ***

                objHashTable.Add("ANNODA", ddlAnnoRiferimento.SelectedItem.Text);
                objHashTable.Add("ANNOA", "-1");

                objHashTable.Add("CODCONTRIBUENTE", "-1");

                int TipoCalcolo = CalcoloICI.TIPOCalcolo_STANDARD;
                if (rdbCalcoloNetto.Checked == true)
                {
                    TipoCalcolo = CalcoloICI.TIPOCalcolo_NETTOVERSATO;
                }

                //*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                //blocco calcolo se manca rendita e tipo utilizzo di RE
                DataTable dtBloccoCalcolo = null;
                int isTASISuProp = 0;
                if (optTASIProp.Checked == true)
                    isTASISuProp = 1;
                if (!new CalcoloICI().isBloccaCalcolo(ConstWrapper.CodiceEnte, int.Parse(ddlAnnoRiferimento.SelectedItem.Text), -1, isTASISuProp, out dtBloccoCalcolo))
                {
                    IFreezer remObjectFreezer = (IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI);
                    bool ConfigDichiarazione = ConstWrapper.ConfigurazioneDichiarazione;
                    objSituazioneFinale[] ListSituazioneFinale = null;
                    remObjectFreezer.CalcoloFromSoggetto(ConstWrapper.StringConnectionOPENgov, strConnectionStringOPENgovICI , ConstWrapper.CodiceEnte,-1,Tributo, ConstWrapper.CodiceTributo, ddlAnnoRiferimento.SelectedItem.Text,"-1", true,  ConfigDichiarazione, bVersatoNelDovuto, bCalcolaArrotondamento, TipoCalcolo,"",isTASISuProp.ToString(), string.Empty, ConstWrapper.sUsername, ref ListSituazioneFinale);
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new  Utility.Costanti.LogEventArgument().Elaborazioni, "Massivo.btnCalcoloICI", Utility.Costanti.AZIONE_NEW.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte,-1);
                }
                else
                { string strscript = "GestAlert('a', 'warning', '', '', 'Impossibile effettuare il Calcolo Massivo per mancanza di rendita e/o tipo utilizzo!');";
                    RegisterScript(strscript,this.GetType()); }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.btnCalcoloICI_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAnnoRiferimento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                //*** 20140509 - TASI ***
                 //*** 20130422 - aggiornamento IMU ***
                LoadFrame();
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.ddlAnnoRiferimento_SelectedIndexChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** 20140509 - TASI ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Tributo_CheckedChanged(object sender, System.EventArgs e)
        {
            //*** 20150430 - TASI Inquilino ***
            try { 
            if (chkTASI.Checked == true)
            { optTASINo.Enabled = true; optTASIProp.Enabled = true; }
            else
            { optTASINo.Enabled = false; optTASIProp.Enabled = false; }
            //*** ***
            LoadFrame();
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.Tributo_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** 201511 - template documenti per ruolo ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUploadClick(object sender, EventArgs e)
        {
            try//carico template nella tabella dei ruoli elaborati
            {
                if (int.Parse(hdIdRuolo.Value) <= 0)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!')";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    if (System.IO.Path.GetFileName(fileUpload.PostedFile.FileName) == "")
                    {
                        RegisterScript("GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un file!');",this.GetType());
                    }
                    else
                    {
                        ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
                        myTemplateDoc.myStringConnection = ConstWrapper.StringConnectionOPENgov;
                        myTemplateDoc.IdEnte = ConstWrapper.CodiceEnte;
                        myTemplateDoc.IdTributo = ConstWrapper.CodiceTributo;
                        myTemplateDoc.IdRuolo = int.Parse(hdIdRuolo.Value);
                        myTemplateDoc.FileMIMEType = fileUpload.PostedFile.ContentType;
                        myTemplateDoc.PostedFile = fileUpload.FileBytes;
                        myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                        myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save();
                        if (myTemplateDoc.IdTemplateDoc <= 0)
                            lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
                        else
                        {
                            lblMessage.Text = "File caricato con successo.";
                            lblMessage.CssClass = "Input_Label_bold";
                        }

                        lblMessage.Visible = true;
                        fileUpload = new FileUpload();
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.BtnUploadClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                DivAttesa.Style.Add("display", "none");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDownloadClick(object sender, EventArgs e)
        {
            ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
            try//scarico template dalla tabella dei ruoli elaborati
            {
                if (int.Parse(hdIdRuolo.Value) <= 0)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');";
                    RegisterScript(sScript,this.GetType());
                }
                else
                {
                    myTemplateDoc.myStringConnection = ConstWrapper.StringConnectionOPENgov;
                    myTemplateDoc.IdEnte = ConstWrapper.CodiceEnte;
                    myTemplateDoc.IdTributo = ConstWrapper.CodiceTributo;
                    myTemplateDoc.IdRuolo = int.Parse(hdIdRuolo.Value);
                    myTemplateDoc.Load();
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.BtnDownloadClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                DivAttesa.Style.Add("display", "none");
            }
            if (myTemplateDoc.PostedFile != null)
            {
                Response.ContentType = myTemplateDoc.FileMIMEType;
                Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", myTemplateDoc.FileName));
                Response.BinaryWrite(myTemplateDoc.PostedFile);
                Response.End();
            }
        }
        //*** ***

        private void LoadFrame()
        {
            try
            {
                if (chkICI.Checked == true && chkTASI.Checked == false)
                    Tributo = ConstWrapper.CodiceTributo;
                else if (chkICI.Checked == false && chkTASI.Checked == true)
                    Tributo = Utility.Costanti.TRIBUTO_TASI;

                myFrame = (HtmlControl)(this.FindControl("loadGridAliquote"));
                myFrame.Attributes.Add("src", "./ElencoAliquote.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&COD_TRIBUTO=" + Tributo);

                myFrame = (HtmlControl)(this.FindControl("loadGridRiepilogoCalcoloICI"));
                myFrame.Attributes.Add("src", "./GetCalcoloICI.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&ID_PROG_ELAB=-1&bNettoVersato=" + rdbCalcoloNetto.Checked.ToString() + "&COD_TRIBUTO=" + Tributo);

                myFrame = (HtmlControl)(this.FindControl("loadCalcoloCatVSCl"));
                myFrame.Attributes.Add("src", "./GetCalcoloICICategoriaClasse.aspx?ANNO=" + ddlAnnoRiferimento.SelectedItem.Text + "&ID_PROG_ELAB=-1&COD_TRIBUTO=" + Tributo);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICIMassivo.LoadFrame.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** ***
	}
}
