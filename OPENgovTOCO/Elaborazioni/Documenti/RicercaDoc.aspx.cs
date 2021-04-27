using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Configuration;
using IRemInterfaceOSAP;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using DTO;
using log4net;
using AnagInterface;
using RIBESElaborazioneDocumentiInterface.Stampa.oggetti;
using System.Data.SqlClient;

namespace OPENgovTOCO.Elaborazioni.Documenti
{
/// <summary>
/// Pagina per la selezione dei parametri di ricerca dei documenti da stampare e per la gestione dell'iter di stampa.
/// Contiene i parametri di ricerca, le funzioni della comandiera e griglia per la visualizzazione del risultato.
/// </summary>
    public partial class RicercaDoc : BaseEnte
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RicercaDoc));

        protected System.Web.UI.WebControls.TextBox TextBox1;

        //private int _total;
        //private int _elaborati;
        //private int _daelaborare;

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
            int _total;
            int _elaborati;
            int _daelaborare;
            try
            {
                //*** 20140512 commentata la riga successiva che causava la non chiamata al metodo btnElaborazioneDocumenti_Click()
                //dopo il caricamento della pagina   
                //btnElaborazioneDocumenti.Attributes.Add("onclick", "ElaborazioniDocumenti(); return false;");
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerText += " - Elaborazione - Documenti";

                if (!Page.IsPostBack)
                {
                    txtNumDoc.Text=DichiarazioneSession.nMaxDocPerFile.ToString();
                    if (SharedFunction.IsNullOrEmpty(Request["Anno"]))
                    {
                        string sScript = "GestAlert('a', 'warning', '', '', 'Impossibile recuperare le informazioni per l\'elaborazione.')";
                        RegisterScript(sScript,this.GetType());
                    }
                    hfAnno.Value = Request["Anno"].ToString();
                    //*** 20130610 - ruolo supplettivo ***
                    if (int.Parse(Request["TipoRuolo"].ToString()) == 1)
                        hfTipoRuolo.Value = ((int)Ruolo.E_TIPO.SUPPLETIVO).ToString();
                    else
                        hfTipoRuolo.Value = ((int)Ruolo.E_TIPO.ORDINARIO).ToString();

                    ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(hfAnno.Value), (Ruolo.E_TIPO)int.Parse(hfTipoRuolo.Value), DichiarazioneSession.CodTributo(""));
                    hfIdFlusso.Value = myRuolo.IdFlusso.ToString();
                    //*** ***
                    lblAnnoRuolo.Text = hfAnno.Value;
                    if (myRuolo.IdTributo == Utility.Costanti.TRIBUTO_SCUOLE)
                        lblTipoRuolo.Text = "SCUOLE";
                    else
                        lblTipoRuolo.Text = "TRIBUTI PER OCCUPAZIONE SUOLO PUBBLICO";
                    lblDataCartellazione.Text = myRuolo.DataOraCalcoloRate.ToString("dd/MM/yyyy");
                    DAO.ElaborazioniDAO elab = new DAO.ElaborazioniDAO();
                    elab.GetElaborazioniStatistics(DichiarazioneSession.IdEnte, myRuolo.IdFlusso, int.Parse(hfAnno.Value), out _total, out _elaborati, out _daelaborare);
                    lblNumeroDocElaborati.Text = _elaborati.ToString();
                    lblNumeroDocDaElaborare.Text = _daelaborare.ToString();

                    hfDocTot.Value = _total.ToString();
                    hfDocElab.Value = _elaborati.ToString();

                    if (int.Parse(_daelaborare.ToString()) == 0)
                    {
                        Generali.classi.Documenti FncDoc = new Generali.classi.Documenti();
                        ArrayList urls = new ArrayList();
                        RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] docs = FncDoc.GetDocumentiElaborati(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo("").ToString(), myRuolo.IdFlusso);
                        Log.Debug("docs: " + docs.ToString());
                        if (docs.Length > 0)
                        {
                            foreach (GruppoURL url in docs)
                                urls.Add(url.URLComplessivo);

                            DataBindGrdElenco(urls, 0, true);
                        }
                    }
                    DataBindGridElaborazioni(TryGetFromSession(),  true);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                string sScript = "GestAlert('a', 'danger', '', '', '" + ex.Message + "')";
                RegisterScript(sScript,this.GetType());
            }
        }
        #region "Griglie"
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            DataBindGridElaborazioni(TryGetFromSession(), true, e.NewPageIndex);
        }
        #endregion

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("../ElaborazioneAvvisi.aspx");
        }
        protected void btnSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                DataBindGridElaborazioni(null, true);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnSearch_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        protected void btnElaborazioneDocumenti_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string sTipoElab = "PROVA";
            int nReturn;
            int nMaxDocPerFile;
            bool bElaboraBollettini = false;
            bool bCreaPDF = false;
            string TipoBollettino = "896";
            string ImpostazioneBollettini = string.Empty;

            try
            {
             
                if (optEffettivo.Checked)
                    sTipoElab = "EFFETTIVO";
                if (chkSoloBollettino.Checked)
                {
                    chkElaboraBollettini.Checked = true;
                }
                if (chkElaboraBollettini.Checked)
                {
                    bElaboraBollettini = true;
                    ImpostazioneBollettini = "BOLLETTINISTANDARD";
                }
                else
                {
                    bElaboraBollettini = false;
                    ImpostazioneBollettini = "NOBOLLETTINI";
                }
                if (optTipoBollettino451.Checked)
                {
                    TipoBollettino = "451";
                }

                nMaxDocPerFile = SharedFunction.IntTryParse(txtNumDoc.Text, int.Parse(ConfigurationManager.AppSettings["NDocPerFile"]));
                try
                {
                    Generali.classi.Documenti FncDoc = new Generali.classi.Documenti();
                    Log.Debug("RicercaDoc::btnElaborazioneDocumenti_Click::devo richiamare ElaboraDocumenti per idruolo=" + hfIdFlusso.Value);
                    //nReturn = FncDoc.ElaboraDocumenti(nTipoElab, nMaxDocPerFile, sTypeOrd, _IdFlusso, PrelevaCartelle(), bElaboraBollettini, bCreaPDF,TipoBollettino);
                    /**** 201810 - Calcolo puntuale ****/
                    nReturn = FncDoc.ElaboraDocumenti(DichiarazioneSession.DBType, DichiarazioneSession.CodTributo(""), PrelevaCartelle(), int.Parse(hfAnno.Value), DichiarazioneSession.IdEnte, int.Parse(hfIdFlusso.Value), DichiarazioneSession.StringConnection, DichiarazioneSession.StringConnectionOPENgov, DichiarazioneSession.StringConnectionAnagrafica, DichiarazioneSession.PathStampe, DichiarazioneSession.PathVirtualStampe, nMaxDocPerFile, sTipoElab, ImpostazioneBollettini, TipoBollettino, bElaboraBollettini, bCreaPDF, chkSendMail.Checked, chkSoloBollettino.Checked,string.Empty);
                    if (nReturn == 1)
                    {
                        RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] docs = FncDoc.GetDocumentiElaborati(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo("").ToString(), int.Parse(hfIdFlusso.Value));
                        Session["StampaPuntuale"] = docs;
                        string sScript = "document.getElementById('DivAttesa').style.display = 'none';";
                        sScript += "document.getElementById('divVisual').style.display='none';";
                        sScript += "document.getElementById('divStampa').style.display='';";
                        sScript += "document.getElementById('loadStampa').src='ViewDocElaborati.aspx?idFlussoRuolo="+hfIdFlusso.Value+"';";
                        RegisterScript(sScript, this.GetType());
                        /*Log.Debug("docs: " + docs.ToString());
                        if (docs.Length > 0)
                        {
                            foreach (GruppoURL url in docs)
                                urls.Add(url.URLComplessivo);

                            DataBindGrdElenco(urls, 0, true);
                        }*/
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.btnElaborazioneDocumenti_Click.errore: ", Err);
                    Response.Redirect("../../PaginaErrore.aspx");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnElaborazioneDoucmenti_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnStampaRate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(hfAnno.Value), (Ruolo.E_TIPO)int.Parse(hfTipoRuolo.Value), DichiarazioneSession.CodTributo(""));
            hfIdFlusso.Value = myRuolo.IdFlusso.ToString();

            string NameXLS = DichiarazioneSession.IdEnte + "_MINUTARATE_" + myRuolo.IdTributo + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

            DataTable dtMinuta = MetodiMinuta.GetMinutaRate(DichiarazioneSession.StringConnection, DichiarazioneSession.IdEnte,myRuolo.IdFlusso);
            string[] arrHeaders = null;
            int[] arrColumnsToExport = null;
            try
            {
                arrHeaders = new string[dtMinuta.Columns.Count];
                checked
                {
                    for (int i = 0; i < arrHeaders.Length; i++)
                        arrHeaders[i] = string.Empty;
                }
                arrColumnsToExport = new int[arrHeaders.Length];
                checked
                {
                    for (int i = 0; i < arrHeaders.Length; i++)
                        arrColumnsToExport[i] = i;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnStampaRate_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }

            RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
            objStampa.ExportDetails(dtMinuta, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        }
        //protected void btnStampaRate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(hfAnno.Value), (Ruolo.E_TIPO)int.Parse(hfTipoRuolo.Value), DichiarazioneSession.CodTributo(""));
        //    hfIdFlusso.Value = myRuolo.IdFlusso.ToString();

        //    string NameXLS = DichiarazioneSession.IdEnte + "_MINUTARATE_" + myRuolo.IdTributo + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

        //    DataTable dtMinuta = MetodiMinuta.GetMinutaRate(myRuolo.IdFlusso);
        //    string[] arrHeaders = null;
        //    int[] arrColumnsToExport = null;
        //    try
        //    {
        //         arrHeaders = new string[dtMinuta.Columns.Count];
        //        checked
        //        {
        //            for (int i = 0; i < arrHeaders.Length; i++)
        //                arrHeaders[i] = string.Empty;
        //        }
        //        arrColumnsToExport = new int[arrHeaders.Length];
        //        checked
        //        {
        //            for (int i = 0; i < arrHeaders.Length; i++)
        //                arrColumnsToExport[i] = i;
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnStampaRate_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }

        //        RKLib.ExportData.Export objStampa = new RKLib.ExportData.Export("Web");
        //        objStampa.ExportDetails(dtMinuta, arrColumnsToExport, arrHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        //}
        /// <summary>
        /// Pulsante per l'approvazione del ruolo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnApprovaDocumenti_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(hfAnno.Value), (Ruolo.E_TIPO)int.Parse(hfTipoRuolo.Value), DichiarazioneSession.CodTributo(""));
            try
            {
                if ((myRuolo.IdTributo == Utility.Costanti.TRIBUTO_OSAP && int.Parse(hfDocTot.Value) > 0 && hfDocTot.Value == hfDocElab.Value) || myRuolo.IdTributo == Utility.Costanti.TRIBUTO_SCUOLE)
                {
                    myRuolo.DataOraDocumentiApprovati = DateTime.Now;
                    SqlCommand cmdMyCommand = null;
                    MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand,DichiarazioneSession.sOperatore);
                    string sScript;
                    sScript = "GestAlert('a', 'success', '', '', 'Elaborazio approvata con successo!');";
                    sScript+="parent.Visualizza.location.href='../ElaborazioneAvvisi.aspx';";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    string sScript = "GestAlert('a', 'warning', '', '', 'Elabora tutti i documenti prima di approvare!')";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnApprovaDocumenti_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void btnApprovaDocumenti_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(hfAnno.Value), (Ruolo.E_TIPO)int.Parse(hfTipoRuolo.Value), DichiarazioneSession.CodTributo(""));
        //    try
        //    {
        //        if ((myRuolo.IdTributo == Utility.Costanti.TRIBUTO_OSAP && int.Parse(hfDocTot.Value) > 0 && hfDocTot.Value == hfDocElab.Value) || myRuolo.IdTributo == Utility.Costanti.TRIBUTO_SCUOLE)
        //        {
        //            myRuolo.DataOraDocumentiApprovati = DateTime.Now;
        //            //DAL.DBEngine dbEngine = null;
        //            //MetodiElaborazioneEffettuata.SetElaborazioneEffettuata (statistiche, ref dbEngine);
        //            SqlCommand cmdMyCommand = null;
        //            MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand);
        //            Response.Redirect("../ElaborazioneAvvisi.aspx");
        //        }
        //        else
        //        {
        //            string sScript = "GestAlert('a', 'warning', '', '', 'Elabora tutti i documenti prima di approvare!')";
        //            RegisterScript(sScript,this.GetType());
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnApprovaDocumenti_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");

        //    }

        //}
        /// <summary>
        /// Funzione che elimina l'elaborazione dei documenti del ruolo in elaborazione.
        /// un'elaborazione si può eliminare solo se non è stata data l'approvazione 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdEliminaDoc_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try {
                SqlCommand cmdMyCommand = null;

                ElaborazioneEffettuata myRuolo = MetodiElaborazioneEffettuata.GetElaborazioneEffettuata(int.Parse(hfAnno.Value), (Ruolo.E_TIPO)int.Parse(hfTipoRuolo.Value), DichiarazioneSession.CodTributo(""));
                if (new OPENgovTIA.ClsGestDocumenti().DeleteTabGuidaComunico(DichiarazioneSession.IdEnte, "TBLGUIDA_COMUNICO", myRuolo.IdFlusso, DichiarazioneSession.CodTributo("")) == 0) {
                    throw new Exception();
                }
                if (new OPENgovTIA.ClsGestDocumenti().DeleteTabGuidaComunico(DichiarazioneSession.IdEnte, "TBLDOCUMENTI_ELABORATI", myRuolo.IdFlusso, DichiarazioneSession.CodTributo("")) == 0)
                {
                    throw new Exception();
                }

                myRuolo.DataOraDocumentiStampati = DateTime.MaxValue;
                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref myRuolo, ref cmdMyCommand, DichiarazioneSession.sOperatore);
                Page_Load(sender, e);
            }
            catch (Exception Err) {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.CmdEliminaDoc_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
         protected void btnDownloadAll_Click(object sender, System.EventArgs e)
        {
            WebClient WC = new WebClient();
            string FileName, sScript, path_web, NOME_FILE, sData, txtZipFileName, strPathZip;
            ArrayList objDocumentiStampate = new ArrayList();
            ArrayList oArrayOggettoUrl = new ArrayList();
            Uri cUri;
            DirectoryInfo di;
            ZipEntry entry;
            string strPath = string.Empty;

            try
            {
                strPathZip = System.Configuration.ConfigurationManager.AppSettings["PATH_ZIP"];
                if (strPathZip.LastIndexOf("\\") == strPathZip.Length - 1)
                    strPathZip += "\\";
                sData = DateTime.Now.ToString("ddMMyyyyHHmmss");

                WC.Credentials = CredentialCache.DefaultCredentials;

                strPath = strPathZip + sData + "\\";
                di = new DirectoryInfo(strPath);
                if (!di.Exists) di.Create();

                objDocumentiStampate = (ArrayList)Session["ELENCO_DOCUMENTI_STAMPATI"];
                foreach (oggettoURL ourl in objDocumentiStampate)
                {
                    oArrayOggettoUrl.Add(ourl);
                    path_web = ourl.Url;
                    FileName = ourl.Name;
                    NOME_FILE = strPath + ourl.Name;

                    cUri = new Uri(path_web);
                    WC.BaseAddress = path_web;
                    WC.DownloadFile(cUri.ToString(), NOME_FILE);
                    Log.Info("Salvataggio del file '" + path_web + "' in '" + FileName + "' effettuato");
                }

                FileStream fs;
                di = new DirectoryInfo(strPath);

                FileName = Session["COD_ENTE"] + "_Lettere_" + sData + ".zip";
                txtZipFileName = strPathZip + FileName;
                if (File.Exists(txtZipFileName)) File.Delete(txtZipFileName);
                ZipOutputStream strmZipOutputStream = new ZipOutputStream(File.Create(txtZipFileName));
                strmZipOutputStream.SetLevel(6);


                foreach (FileInfo myfile in di.GetFiles())
                {
                    fs = File.OpenRead(myfile.FullName);

                    Byte[] buffer = new Byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry("" + myfile.Name);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    strmZipOutputStream.PutNextEntry(entry);
                    strmZipOutputStream.Write(buffer, 0, buffer.Length);
                    strmZipOutputStream.CloseEntry();
                    fs.Close();
                }

                strmZipOutputStream.Finish();
                strmZipOutputStream.Close();

                download(txtZipFileName, FileName);
                Log.Info("Cancellazione della cartella " + di.FullName);
                if (di.Exists) di.Delete(true);
                if (File.Exists(txtZipFileName)) File.Delete(txtZipFileName);
                Log.Info("Cancellazione del file " + txtZipFileName);
                Thread.Sleep(1000);
            }
            catch (DirectoryNotFoundException dex)
            {
                Log.Error("Non è stato trovato il percorso " + strPath);
                Log.Debug("Non è stato trovato il percorso " + strPath + "::" + dex.StackTrace);
                sScript = "GestAlert('a', 'danger', '', '', 'Non è stato trovato il percorso '+ " + strPath + "');";
                RegisterScript(sScript,this.GetType());
            }
            catch (UnauthorizedAccessException Uex)
            {
                Log.Error("Non si dispone delle credenziali per accedere al percorso " + strPath);
                Log.Debug("Non si dispone delle credenziali per accedere al percorso " + strPath + "::" + Uex.StackTrace);
                sScript = "GestAlert('a', 'danger', '', '', 'Non si dispone delle credenziali per accedere al percorso '+ " + strPath + "');";
                RegisterScript(sScript,this.GetType());
            }
            catch (SecurityException sex)
            {
                Log.Error("SecurityException: " + sex.Message);
                Log.Debug("SecurityException: " + sex.Message);
                sScript = "GestAlert('a', 'danger', '', '', '" + sex.Message + "');";
                RegisterScript(sScript,this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.btnDownloadAll_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                sScript = "GestAlert('a', 'danger', '', '', '" + ex.Message + "');";
                RegisterScript(sScript,this.GetType());
            }
            finally
            {
                WC.Dispose();
                sScript = "document.getElementById('download').className = 'downloadh';";
                RegisterScript(sScript,this.GetType());
            }
        }

        protected void chkElaboraTutti_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                Cartella[] lista = (Cartella[])Session["ListaCartelle"];

                foreach (Cartella myRow in lista)
                {
                    myRow.Selezionato = chkElaboraTutti.Checked;
                }
                foreach (GridViewRow itemGrid in GrdDocumenti.Rows)
                    (itemGrid.FindControl("ChkSelezionato") as CheckBox).Checked = chkElaboraTutti.Checked;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.chkElaboraTutti_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }
        protected void ChkSelezionato_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cartella[] lista = (Cartella[])Session["ListaCartelle"];

                foreach (GridViewRow MyItemGrd in GrdDocumenti.Rows)
                {
                    if (((CheckBox)(MyItemGrd.FindControl("ChkSelezionato"))).Checked == true)
                    {
                        foreach (Cartella myAvv in lista)
                        {
                            if (myAvv.CodContribuente.ToString() == ((HiddenField)(MyItemGrd.FindControl("hfCodContribuente"))).Value)
                                myAvv.Selezionato = true;
                        }
                    }
                }
                Session["ListaCartelle"] = lista;
                DataBindGridElaborazioni(lista, true, GrdDocumenti.PageIndex);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.ChkSelezionato_CheckedChanged.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");

            }
        }

        private void DataBindGridElaborazioni(Cartella[] elenco, bool bRebind, int? page = 0)
        {
            try
            {
                if (elenco == null)
                {
                    DTO.ElaborazioniSearch SearchParams = new DTO.ElaborazioniSearch();
                    if (!SharedFunction.IsNullOrEmpty(txtNominativoDa.Text))
                        SearchParams.NomeDA = txtNominativoDa.Text.Trim().Replace("*", "");
                    if (!SharedFunction.IsNullOrEmpty(txtNominativoA.Text))
                        SearchParams.NomeA = txtNominativoA.Text.Trim().Replace("*", "");
                    if (!SharedFunction.IsNullOrEmpty(txtCodiceCartella.Text))
                        SearchParams.CodiceCartella = txtCodiceCartella.Text.Trim().Replace("*", "");
                    SearchParams.Anno = int.Parse(hfAnno.Value);
                    //*** 20130610 - ruolo supplettivo ***
                    SearchParams.IdFlusso = int.Parse(hfIdFlusso.Value);
                    //*** ***

                    elenco = DTO.MetodiElaborazioniTosapCosap.SearchElaborazione(SearchParams);
                }
                if (elenco != null && elenco.Length > 0)
                {
                    GrdDocumenti.DataSource = elenco;
                    if (bRebind)
                    {
                        GrdDocumenti.DataBind();
                    }
                    if (page.HasValue)
                        GrdDocumenti.PageIndex = page.Value;
                    GrdDocumenti.DataBind();
                    GrdDocumenti.Visible = true;
                    Session["ListaCartelle"] = elenco;
                    ClientScript.RegisterStartupScript(this.GetType(), "viewSearch", "<script language='javascript'>ViewSearch();</script>");
                }
                else
                {
                    GrdDocumenti.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.DataBindGridElaborazioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        private void DataBindGrdElenco(ArrayList elenco, int StartIndex, bool bRebind, int? page = 0)
        {
            try
            {
                if (elenco == null)
                    return;
                if (elenco.Count > 0)
                {
                    GrdElenco.DataSource = (oggettoURL[])elenco.ToArray(typeof(oggettoURL));
                    if (bRebind)
                    {
                        GrdElenco.DataBind();
                    }
                    GrdElenco.Visible = true;
                    if (page.HasValue)
                        GrdElenco.PageIndex = page.Value;
                    GrdElenco.DataBind();
                    Session["ELENCO_DOCUMENTI_STAMPATI"] = elenco;
                    RegisterScript("ViewList();$('#lblDownloadAll').hide();", this.GetType());
                }
                else
                {
                    GrdElenco.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.DataBindGrdElenco.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        private Cartella[] TryGetFromSession()
        {
            try
            {
                if (Session["ListaCartelle"] == null) return null;
                Cartella[] lista = (Cartella[])Session["ListaCartelle"];
                if (GrdDocumenti.Rows.Count > 0)
                {
                    foreach (GridViewRow item in GrdDocumenti.Rows)
                    {
                        CheckBox sel = (CheckBox)item.Cells[6].FindControl("ChkSelezionato");
                        foreach (Cartella myRow in lista)
                        {
                            if (myRow.CodiceCartella == item.Cells[2].Text)
                            {
                                myRow.Selezionato = sel.Checked;
                                break;
                            }
                        }
                    }
                }
                return lista;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.TryGetFromSession.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;
            }
        }
        //private string[] PrelevaCartelle()
        //{
        //    try
        //    {
        //        ArrayList aCartelle = new ArrayList();
        //        Cartella[] lista = (Cartella[])Session["ListaCartelle"];
        //        for(int i = 0; i < lista.Length; i++)
        //        {
        //            if(lista[i].Selezionato)
        //                aCartelle.Add(lista[i].CodiceCartella);
        //        }
        //        return (string[])aCartelle.ToArray(typeof(string));
        //    }
        //    catch (Exception ex) 
        //    {
        //         Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.PrelevaCartelle.errore: ", Err);
        //          Response.Redirect("../../PaginaErrore.aspx");
        //        return null;
        //    }
        //}
        private Cartella[] PrelevaCartelle()
        {
            try
            {
                ArrayList aCartelle = new ArrayList();
                Cartella[] lista = (Cartella[])Session["ListaCartelle"];
                foreach (Cartella myRow in lista)
                {
                    if (myRow.Selezionato)
                        aCartelle.Add(myRow);
                }
                Log.Debug("ho selezionato " + aCartelle.Count.ToString() + " avvisi");
                return (Cartella[])aCartelle.ToArray(typeof(Cartella));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.PrelevaCartelle.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                return null;

            }
        }
        protected string GetContribuente(object anagrafica, string field)
        {
            try
            {
                if ((anagrafica == null) || !(anagrafica is DettaglioAnagrafica)) return string.Empty;
                switch (field)
                {
                    case "Cognome":
                        return ((DettaglioAnagrafica)anagrafica).Cognome;
                    case "Nome":
                        return ((DettaglioAnagrafica)anagrafica).Nome;
                    default:
                        return string.Empty;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.GetContribuente.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }
        private void download(string txtZipFileName, string nomeFileZip)
        {
            Response.ContentType = "application/zip";
            Response.AppendHeader("content-disposition", "attachment; filename=" + nomeFileZip);
            Response.WriteFile(txtZipFileName);
            Response.Flush();
        }
    }
}
