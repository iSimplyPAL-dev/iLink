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
using IRemInterfaceOSAP;
using DTO;

namespace OPENgovTOCO.Dichiarazioni
{
    /// <summary>
    /// Pagina per la visualizzazione articolo in dichiarazione
    /// </summary>
    public partial class DichiarazioniView : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioniView));

        private Wuc.WucDichiarazione wuc;

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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Put user code to initialize the page here
            lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
            if (hfCalcoloAvviso.Value == "0")
                RegisterScript("$('#DivCalcoloAvviso').hide();", this.GetType());
            try
            {
                if (!Page.IsPostBack)
                {
                    int IdDichiarazione = int.Parse(Request["IdDichiarazione"].ToString());
                    Log.Debug("DichiarazioniView::Page_Load::IdDichiarazione" + IdDichiarazione.ToString());
                    DichiarazioneSession.SessionDichiarazioneTosapCosap = DTO.MetodiDichiarazioneTosapCosap.GetDichiarazione(DichiarazioneSession.StringConnection, IdDichiarazione, DichiarazioneSession.IdEnte, -1, DichiarazioneSession.CodTributo(""));
                    // SharedFunction.EnableDisableButton(ref Edit, !DTO.MetodiDichiarazioneTosapCosap.IsCartellaCreata(IdDichiarazione)); 
                    // S.T. Abilitata sempre la modifica 
                    SharedFunction.EnableDisableButton(ref Edit, true);
                    /**** 201810 - Calcolo puntuale ****/
                    string[] lista = MetodiTariffe.GetAnniTariffe(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                    if (lista != null && lista.Length > 0)
                    {
                        ddlAnno.DataSource = lista;
                        ddlAnno.DataBind();
                    }
                    /**** ****/
                }
                wuc = (Wuc.WucDichiarazione)(this.FindControl("wucDichiarazione"));
                wuc.OpType = OSAPConst.OperationType.VIEW;

                Delete.Attributes.Add("onclick", "javascript:return confirm('Eliminare la Dichiarazione?');");
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");

                string sScript = "GestAlert('a', 'danger', '', '', '" + ex.Message + "')";
                RegisterScript(sScript,this.GetType());
            }
        }

        protected void Delete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            wuc.EliminaDichiarazione();
            Response.Redirect(OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo(""));
        }

        protected void Cancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string sScript = "";
            try
            {
                if (Request.Params["Provenienza"] != null)
                {
                    //*** 20150703 - INTERROGAZIONE GENERALE ***
                    if (Request.Params["Provenienza"] == "INTERGEN")
                    {
                        sScript = "parent.Visualizza.location.href='../../Interrogazioni/DichEmesso.aspx?Ente=" + DichiarazioneSession.IdEnte + "';";
                        sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';";
                        sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                        sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';";
                    }
                    else
                        sScript = "location.href='" + OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo("") + "'";
                }
                else
                    sScript = "location.href='" + OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo=" + DichiarazioneSession.CodTributo("") + "'";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.Cancel_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        protected void Edit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect(OSAPPages.DichiarazioniEdit);
        }
        /**** 201810 - Calcolo puntuale ****/
        #region "Calcolo Puntuale"
            /// <summary>
            /// Funzione per il calcolo puntuale.
            /// <strong><em>NON RILASCIATO</em></strong>
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        protected void Calcola_Click(object sender, ImageClickEventArgs e)
        {
            string msg = string.Empty;
            Categorie[] categ = null;
            TipologieOccupazioni[] tipiOcc = null;
            Agevolazione[] agevolazioni = null;
            Tariffe[] tarif = null;
            int Anno = -1;
            string myTemplate = string.Empty;

            try
            {
                if (ddlAnno.SelectedValue.CompareTo(string.Empty) != 0)
                {
                    Anno = int.Parse(ddlAnno.SelectedValue);
                    categ = MetodiCategorie.GetCategorieForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));

                    tipiOcc = MetodiTipologieOccupazioni.GetTipologieOccupazioniForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                    agevolazioni = MetodiAgevolazione.GetAgevolazioniForMotore(DichiarazioneSession.StringConnection, Anno.ToString(), DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                    tarif = MetodiTariffe.GetTariffeForMotore(Anno, DichiarazioneSession.CodTributo(""));

                    if (categ == null || categ.Length == 0)
                        msg = "Non è presente nessuna categoria per l'anno selezionato, " +
                            "impossibile procedere al calcolo";
                    else if (tipiOcc == null || tipiOcc.Length == 0)
                        msg = "Non è presente nessuna tipologia di occupazione per l'anno selezionato, " +
                            "impossibile procedere al calcolo";
                    else if (tarif == null || tarif.Length == 0)
                        msg = "Non è presente nessuna tariffa per l'anno selezionato, " +
                            "impossibile procedere al calcolo";
                }
                else
                    msg = "Anno selezionato per il calcolo non valido!";
                if (TxtDataScadenza.Text == string.Empty)
                    msg = "Data scadenza non valida!";
                if (msg.CompareTo(string.Empty) == 0)
                {
                    Cartella[] ListAvvisi = (Cartella[])(new ArrayList()).ToArray(typeof(Cartella));
                    ElaborazioneEffettuata myRuolo = new ElaborazioneEffettuata();

                    myRuolo.IdEnte = DichiarazioneSession.IdEnte;
                    myRuolo.IdTributo = DichiarazioneSession.CodTributo("");
                    myRuolo.Anno = Anno;
                    myRuolo.TipoRuolo = (int)Ruolo.E_TIPO.SUPPLETIVO;
                    if (!CalcoloAvviso(optSimulazione.Checked, ref myRuolo, ref ListAvvisi, categ, tipiOcc, agevolazioni, tarif, int.Parse(Request["IdDichiarazione"].ToString())))
                    {
                        RegisterScript("GestAlert('a', 'danger', '', '', 'Errore in calcolo Avviso!')", this.GetType());
                    }
                    else {
                        myRuolo.DataOraFineElaborazione = DateTime.Now;
                        if (ListAvvisi.GetLength(0) > 0)
                        {
                            if (!UploadTemplate(optSimulazione.Checked, Request.Files, myRuolo.IdFlusso, ref myTemplate))
                            {
                                RegisterScript("GestAlert('a', 'danger', '', '', 'Errore in caricamento template!')", this.GetType());
                            }
                            else {
                                myRuolo.DataOraMinutaStampata = DateTime.Now;
                                myRuolo.DataOraMinutaApprovata = DateTime.Now;
                                ArrayList myList = new ArrayList();
                                myList.Add(new Rate { IdEnte = DichiarazioneSession.IdEnte, IdFlusso = myRuolo.IdFlusso, Anno = myRuolo.Anno, DataScadenza = DateTime.Parse(TxtDataScadenza.Text), Descrizione = "RATA UNICA", NRata = "U" });
                                Rate[] ListRate = (Rate[])myList.ToArray(typeof(Rate));
                                myRuolo.DataOraCalcoloRate = DateTime.Now;
                                if (!CalcoloRate(optSimulazione.Checked, myRuolo, ListRate))
                                {
                                    RegisterScript("GestAlert('a', 'danger', '', '', 'Errore in calcolo rate Avviso!')", this.GetType());
                                }
                                else {
                                    myRuolo.DataOraDocumentiStampati = DateTime.Now;
                                    if (!ElabDocumento(optSimulazione.Checked, ListAvvisi, myRuolo.IdFlusso,myTemplate))
                                    {
                                        RegisterScript("GestAlert('a', 'danger', '', '', 'Errore in elaborazione documento Avviso!')", this.GetType());
                                    }
                                    else {
                                        myRuolo.DataOraDocumentiApprovati = DateTime.Now;
                                        System.Data.SqlClient.SqlCommand cmdMyCommand = null;
                                        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand,DichiarazioneSession.sOperatore);
                                        hfCalcoloAvviso.Value = "0";
                                    }
                                }
                            }
                        }
                        else {
                            RegisterScript("GestAlert('a', 'danger', '', '', 'Errore in calcolo Avviso!')", this.GetType());
                        }
                    }
                }
                else
                {
                    string sScript = "GestAlert('a', 'warning', '', '', '" + msg + "')";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.Calcola_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                RegisterScript("$('#divElaborazioneInCorso').hide();", this.GetType());
            }
        }
        private bool CalcoloAvviso(bool Simula, ref ElaborazioneEffettuata myRuolo, ref Cartella[] ListAvvisi, Categorie[] categ, TipologieOccupazioni[] tipiOcc, Agevolazione[] agevolazioni, Tariffe[] tarif, int IdDichiarazione)
        {
            bool myRet = false;
            try
            {
                //controllo se mancano delle tariffe
                string sTariffaMancante = MetodiElaborazioneEffettuata.CheckTariffe(myRuolo.Anno, DichiarazioneSession.CodTributo(""));
                if (sTariffaMancante != string.Empty)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', '" + sTariffaMancante + "')";
                    RegisterScript(sScript, this.GetType());
                    myRet = false;
                }
                else
                {
                    // Posso fare partire il calcolo
                    lblElaborazioneAnno.Text = myRuolo.Anno.ToString();
                    CacheManager.SetAvanzamentoElaborazione("");
                    Session["OSAPAnnoCalcoloInCorso"] = ddlAnno.SelectedValue;
                    Session["OSAPTipoRuoloCalcoloInCorso"] = "1";

                    Log.Debug("ElaborazioneAvvisi::btnCalcola_Click::idruolo::vuoto=0");
                    new CalcoloOSAP().CalcoloSync(Simula, Ruolo.E_TIPO.SUPPLETIVO, myRuolo.Anno, categ, tipiOcc, agevolazioni, tarif, DichiarazioneSession.IdEnte, DichiarazioneSession.sOperatore, DichiarazioneSession.CodTributo(""), myRuolo.SogliaMinimaRate, -1, string.Empty, IdDichiarazione);
                    myRuolo = CacheManager.GetRiepilogoCalcoloMassivo();
                    ListAvvisi = CacheManager.GetAvvisiCalcoloMassivo();
                    myRet = true;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.CalcoloAvviso.errore: ", ex);
                myRet = false;
            }
            return myRet;
        }
        private bool CalcoloRate(bool Simula, ElaborazioneEffettuata myRuolo, Rate[] config)
        {
            bool myRet = false;
            try
            {
                OPENUtility.ClsContiCorrenti oClsContiCorrente = new OPENUtility.ClsContiCorrenti();
                OPENUtility.objContoCorrente contoCorrente = oClsContiCorrente.GetContoCorrente(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), DichiarazioneSession.sOperatore, DichiarazioneSession.StringConnectionOPENgov);
                if (contoCorrente == null)
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile effettuare il calcolo rate per l\'anno " + myRuolo.Anno + "! Manca il Conto Corrente.')";
                    RegisterScript(sScript, this.GetType());
                    myRet = false;
                }
                else
                {
                    if (!Simula)
                        MetodiConfigurazioneRate.InsertConfigurazioneRate(config);
                    CacheManager.SetAvanzamentoElaborazione("");
                    new CalcoloRateOSAP().CalcoloSync(Simula, myRuolo.Anno, DichiarazioneSession.IdEnte, myRuolo, config, DichiarazioneSession.sOperatore, contoCorrente, DichiarazioneSession.CodTributo(""));
                }
                myRet = true;
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.CalcoloRate.errore: ", ex);
                myRet = false;
            }
            return myRet;
        }
        private bool ElabDocumento(bool Simula, Cartella[] ListAvvisi, int IdFlusso,string TemplateFile)
        {
            bool myRet = false;
            try
            {
                string sTipoElab = "PROVA";

                if (optEffettivo.Checked)
                    sTipoElab = "EFFETTIVO";

                if (new Generali.classi.Documenti().ElaboraDocumenti(DichiarazioneSession.DBType, DichiarazioneSession.CodTributo(""), ListAvvisi, ListAvvisi[0].Anno, DichiarazioneSession.IdEnte, IdFlusso, DichiarazioneSession.StringConnection, DichiarazioneSession.StringConnectionOPENgov, DichiarazioneSession.StringConnectionAnagrafica, DichiarazioneSession.PathStampe, DichiarazioneSession.PathVirtualStampe, 1, sTipoElab, "BOLLETTINISTANDARD", "451", true, false, false, false,TemplateFile) == 1)
                {
                        myRet = true;
                    RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] ListDoc = new Generali.classi.Documenti().GetDocumentiElaborati(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo("").ToString(), IdFlusso);
                    if (ListDoc != null)
                    {
                        ArrayList ListPathDoc = new ArrayList();
                        foreach (RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL myDoc in ListDoc)
                        {
                            ListPathDoc.Add(myDoc.URLComplessivo);
                        }

                        GrdDocumenti.DataSource = (RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL[])ListPathDoc.ToArray(typeof(RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL));
                        GrdDocumenti.DataBind();
                        GrdDocumenti.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.ElabDocumento.errore: ", ex);
                myRet = false;
            }
            return myRet;
        }
        private bool UploadTemplate(bool Simula,HttpFileCollection ListFiles, int IdFlusso, ref string TemplateFile)
        {
            bool myRet = false;
            TemplateFile = string.Empty;
            try
            {
                if (ListFiles != null)
                {
                    foreach (HttpPostedFile PostedFile in ListFiles)
                    {
                        if (PostedFile.ContentLength > 0)
                        {
                            ElaborazioneDatiStampeInterface.ObjTemplateDoc myTemplateDoc = new ElaborazioneDatiStampeInterface.ObjTemplateDoc();
                            myTemplateDoc.myStringConnection = DichiarazioneSession.StringConnectionOPENgov;
                            myTemplateDoc.IdEnte = DichiarazioneSession.IdEnte;
                            myTemplateDoc.IdTributo = DichiarazioneSession.CodTributo("");
                            myTemplateDoc.IdRuolo = IdFlusso;
                            myTemplateDoc.FileMIMEType = fileUploadDOT.PostedFile.ContentType;
                            myTemplateDoc.PostedFile = fileUploadDOT.FileBytes;
                            myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUploadDOT.PostedFile.FileName);
                            if (!Simula)
                            {
                                myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save();
                                if (myTemplateDoc.IdTemplateDoc > 0)
                                    myRet = true;
                            }
                            else
                            {
                             TemplateFile = fileUploadDOT.PostedFile.FileName;
                               myRet = true;
                            }
                            fileUploadDOT = new FileUpload();
                        }
                        else
                        {
                            myRet = true;
                        }
                    }
                }
                else
                {
                    myRet = true;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.UploadTemplate.errore: ", ex);
                myRet = false;
            }
            return myRet;
        }
        #region "Griglie"
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    foreach (GridViewRow myRow in GrdDocumenti.Rows)
                    {
                        if (IDRow == ((HiddenField)myRow.FindControl("hfid")).Value)
                        {
                            string PathFileToOpen = GrdDocumenti.Rows[myRow.RowIndex].Cells[2].Text;
                            string NomeFile = GrdDocumenti.Rows[myRow.RowIndex].Cells[0].Text;
                            string Url = GrdDocumenti.Rows[myRow.RowIndex].Cells[3].Text;
                            Log.Debug("URL:" + Url);
                            byte[] FileToDownLoad = new Elaborazioni.Documenti.ViewDocElaborati().GetAttachmentFile(PathFileToOpen);

                            if (FileToDownLoad.Length > 0)
                            {
                                //imposta le headers 
                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + NomeFile + "\"");
                                Response.AddHeader("Content-Length", FileToDownLoad.Length.ToString());
                                Response.ContentType = "application/octet-stream";

                                //leggo dal file e scrivo nello stream di risposta 
                                Response.BinaryWrite(FileToDownLoad);
                            }
                            else if (Url != "")
                            {
                                Log.Debug("chiamata a downloadwebfile");
                                new Elaborazioni.Documenti.ViewDocElaborati().downloadwebfile(Url, NomeFile, PathFileToOpen);
                            }
                            else
                            {
                                string sScript = "GestAlert('a', 'warning', '', '', 'Il documento non è stato trovato!');";
                                RegisterScript(sScript,this.GetType());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniView.GrdRowCommand.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #endregion
        #endregion
        /**** ****/
    }
}
