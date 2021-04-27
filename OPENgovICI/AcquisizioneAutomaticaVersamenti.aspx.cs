using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Business;
using DichiarazioniICI.Database;
using System.IO;
using FileImporterInterface;
using System.Globalization;
using log4net;
namespace DichiarazioniICI
{
    /// <summary>
    /// Pagina per l'importazione dei versamenti.
    /// Contiene i parametri di importazione, le funzioni della comandiera e la visualizzazione del risultato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class AcquisizioneAutomaticaVersamenti : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AcquisizioneAutomaticaVersamenti));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string tipo = ddlFormatoImport.SelectedValue;
            string operatore = ConstWrapper.sUsername.ToString();
            string codEnte = ConstWrapper.CodiceEnte.ToString();
            try
            {
                if (((Session["dirittioperatore"]== null)?"": Session["dirittioperatore"].ToString()) == "SUPERUSER")
                {
                    RegisterScript("document.getElementById('TRImportFolder').style.display='';", this.GetType());
                }
                else {
                    RegisterScript("document.getElementById('TRImportFolder').style.display='none';", this.GetType());
                }
                switch (tipo)
                {
                    case "f24":
                        //importazione F24
                        pnlFiltriRicerca.Visible = true;
                        lblImportazioneInCorso.Visible = false;

                        DataTable dtF24 = new Database.ImportazioneTable(operatore).GetRowImportazioni("versamenti", codEnte, "F24");

                        if (dtF24.Rows.Count > 0)
                        {
                            lblUltimaImportazione.Text = "Ultima importazione: " + dtF24.Rows[0]["data"].ToString();
                            lblOperatore.Text = "Operatore: " + dtF24.Rows[0]["Operatore"].ToString();
                            lblFileName.Text = "File importato: " + dtF24.Rows[0]["FileName"].ToString();
                            lblEsito.Text = "Esito: " + (dtF24.Rows[0]["Importato"].ToString() == "True" ? "eseguito" : "fallito");
                            lblImpoTotImport.Text = dtF24.Rows[0]["importoTotImportato"].ToString();
                            lblRecImport.Text = dtF24.Rows[0]["toTRecordImportati"].ToString();
                        }
                        break;

                    case "unirisc":
                        //importazione uniriscossioni
                        pnlFiltriRicerca.Visible = true;
                        lblImportazioneInCorso.Visible = false;

                        //DataTable dt = new Database.ImportazioneTable(operatore).GetRowUni("versamenti",codEnte);
                        DataTable dt = new Database.ImportazioneTable(operatore).GetRowImportazioni("versamenti", codEnte, "UNIRISCOSSIONE");

                        if (dt.Rows.Count > 0)
                        {
                            lblUltimaImportazione.Text = "Ultima importazione: " + dt.Rows[0]["data"].ToString();
                            lblOperatore.Text = "Operatore: " + dt.Rows[0]["Operatore"].ToString();
                            lblFileName.Text = "File importato: " + dt.Rows[0]["FileName"].ToString();
                            lblEsito.Text = "Esito: " + (dt.Rows[0]["Importato"].ToString() == "True" ? "eseguito" : "fallito");
                            lblImpoTotImport.Text = dt.Rows[0]["importoTotImportato"].ToString();
                            lblRecImport.Text = dt.Rows[0]["toTRecordImportati"].ToString();
                        }
                        break;

                    default:
                        // altre importazioni
                        if (new ImportazioneTable(ConstWrapper.sUsername).GetRow("Versamenti").Run)
                        {
                            pnlFiltriRicerca.Visible = false;
                            lblImportazioneInCorso.Visible = true;
                        }
                        else
                        {
                            pnlFiltriRicerca.Visible = true;
                            lblImportazioneInCorso.Visible = false;

                            ImportazioniRow riga = new ImportazioniTable().GetRow(TipologieProvenienza.versamenti);
                            if (riga.ID > 0)
                            {
                                lblUltimaImportazione.Text += " " + riga.Data.ToShortDateString();
                                lblOperatore.Text += " " + riga.Operatore;
                                lblFileName.Text += " " + riga.FileName;
                                lblEsito.Text += " " + (riga.Importato == true ? "eseguito" : "fallito");
                            }
                        }
                        break;

                }
                if (!Page.IsPostBack)
                {
                    new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Pagamento, "Importazione", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte,-1);
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AcquisizioneAutomaticaVersamenti.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
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
        protected void lbtnImporta_Click(object sender, System.EventArgs e)
        {
            try
            {
                            string strscript = "";
        string fileName;
                string nomeFile = fileUpload.Value.ToString();
                string[] path = fileUpload.Value.Split(new char[] { ('\\') });
                int numeroElementi = path.Length;
                fileName = path[numeroElementi - 1];

                //string operatore =ConstWrapper.sUsername.ToString();
                //string codEnte= ConstWrapper.CodiceEnte.ToString();

                if (fileName != "")
                {
                    string tipoImp = ddlFormatoImport.SelectedValue;
                    log.Debug("devo acquisire tipo::" + tipoImp);
                    switch (tipoImp)
                    {
                        case "f24":
                            //Importazione F24
                            string strstringa = "";
                            string sMyErr = "";
                            string Belfiore = "";

                            if (Session["COD_BELFIORE"] != null)
                                Belfiore = Session["COD_BELFIORE"].ToString();

                                                        //preleva la connessione al db ici ed il path per l'acquisizione
                            string percorsoF24 = ConfigurationManager.AppSettings["PATH_F24"];
                            string percorsoDestF24 = ConfigurationManager.AppSettings["PATH_F24_ACQUISITI"];
                            log.Debug("salvo il file al percorso::" + percorsoF24 + fileName);
                            //salvo il file sul server
                            fileUpload.PostedFile.SaveAs(percorsoF24 + fileName);

                            //metodo che effettua i controlli sul file e salva i dati nella tabella TAB_ACQUISIZIONE_F24
                            string messaggio = new ImportazioneF24.ImporterF24().AvviaImportazione(percorsoF24, fileName, Business.ConstWrapper.StringConnection, Business.ConstWrapper.sUsername, ref sMyErr);
                              log.Debug("AvviaImportazione ha dato esito::" + messaggio);
                            if (messaggio == "ok")
                            {
                                bool RetVal = new ImportazioneF24.ImporterF24().ImportSuTributo(ConstWrapper.CodiceEnte.ToString(), Business.ConstWrapper.CodiceTributo, Business.ConstWrapper.StringConnection, percorsoF24, fileName, percorsoDestF24, Belfiore, ConstWrapper.sUsername, -1, ref sMyErr);
                                log.Debug("ImportSuTributo ha dato esito::" + RetVal.ToString());
                                if (RetVal == false)
                                {
                                    strstringa = "GestAlert('a', 'danger', '', '', 'Elaborazione terminata con errori!";
                                    if (sMyErr != "")
                                    {
                                        strstringa += sMyErr;
                                    }
                                    strstringa += "');";
                                    RegisterScript(strstringa,this.GetType() );
                                }
                                else
                                {
                                    strstringa = "GestAlert('a', 'success', '', '', 'Elaborazione terminata!";
                                    if (sMyErr != "")
                                    {
                                        strstringa += sMyErr;
                                    }
                                    strstringa += "');";
                                    RegisterScript(strstringa,this.GetType());

                                    DataTable dtF24 = new Database.ImportazioneTable(ConstWrapper.sUsername.ToString()).GetRowImportazioni("versamenti", ConstWrapper.CodiceEnte.ToString(), "F24");
                                    if (dtF24.Rows.Count > 0)
                                    {
                                        lblUltimaImportazione.Text = "Ultima importazione: " + dtF24.Rows[0]["data"].ToString();
                                        lblOperatore.Text = "Operatore: " + dtF24.Rows[0]["Operatore"].ToString();
                                        lblFileName.Text = "File importato: " + dtF24.Rows[0]["FileName"].ToString();
                                        lblEsito.Text = "Esito: " + (dtF24.Rows[0]["Importato"].ToString() == "True" ? "eseguito" : "fallito");
                                        lblImpoTotImport.Text = dtF24.Rows[0]["importoTotImportato"].ToString();
                                        lblRecImport.Text = dtF24.Rows[0]["toTRecordImportati"].ToString();
                                    }
                                }
                            }

                            if (messaggio == "s")
                            {
                                //il file non è corretto
                                strstringa = "GestAlert('a', 'warning', '', '', 'Il file che si vuole acquisire non è un tracciato F24 corretto!');";
                                RegisterScript(strstringa,this.GetType());
                            }

                            if (messaggio == "err")
                            {
                                //errori in elaborazione
                                strstringa = "GestAlert('a', 'danger', '', '', 'Si sono verificati errori in fase di acquisizione" + sMyErr + "');";
                                RegisterScript(strstringa,this.GetType());
                            }

                            break;

                        case "unirisc":
                            //Importazione uniriscossione
                            string percorso = ConfigurationManager.AppSettings["PATH_UNIRISCOSSIONE"];
                            fileUpload.PostedFile.SaveAs(percorso + fileName);
                            string Nome_File_Trasf = percorso + fileName;

                            Type typeofRI = typeof(IRemotingInterfaceImporter);
                            IRemotingInterfaceImporter remObject = (IRemotingInterfaceImporter)Activator.GetObject(typeofRI,
                                "tcp://localhost:50610/FileImporterService.rem");
                            remObject.gestisciImportazione(ConstWrapper.CodiceEnte.ToString(), Nome_File_Trasf, ConstWrapper.sUsername.ToString());

                            string percorsoDest = ConfigurationManager.AppSettings["PATH_SPOSTA_UNIRISCOSSIONE"];
                            SpostaFile(Nome_File_Trasf, percorsoDest);

                            DataTable datiImportazioneUni = new Database.ImportazioneTable(ConstWrapper.sUsername.ToString()).GetRowImportazioni("versamenti", ConstWrapper.CodiceEnte.ToString(), "UNIRISCOSSIONE");

                            if (datiImportazioneUni.Rows.Count > 0)
                            {
                                lblUltimaImportazione.Text = "Ultima importazione: " + datiImportazioneUni.Rows[0]["data"].ToString();
                                lblOperatore.Text = "Operatore: " + datiImportazioneUni.Rows[0]["Operatore"].ToString();
                                lblFileName.Text = "File importato: " + datiImportazioneUni.Rows[0]["FileName"].ToString();
                                lblEsito.Text = "Esito: " + (datiImportazioneUni.Rows[0]["Importato"].ToString() == "True" ? "eseguito" : "fallito");
                                lblImpoTotImport.Text = datiImportazioneUni.Rows[0]["importoTotImportato"].ToString();
                                lblRecImport.Text = datiImportazioneUni.Rows[0]["toTRecordImportati"].ToString();
                            }

                            break;
                        case "uvi":
                            //Importazione Versamenti UVI
                            //Importazione uniriscossione
                            string percorsoUVI = ConfigurationManager.AppSettings["PATH_UNIRISCOSSIONE"];
                            fileUpload.PostedFile.SaveAs(percorsoUVI + fileName);
                            string Nome_File_TrasfUVI = percorsoUVI + fileName;

                            Type typeofRIUVI = typeof(IRemotingInterfaceImporter);
                            IRemotingInterfaceImporter remObjectUVI = (IRemotingInterfaceImporter)Activator.GetObject(typeofRIUVI,
                                "tcp://localhost:50610/FileImporterService.rem");
                            remObjectUVI.gestisciImportazioneVersamentiUVI(ConstWrapper.CodiceEnte.ToString(), Nome_File_TrasfUVI, ConstWrapper.sUsername.ToString());

                            string percorsoDestUVI = ConfigurationManager.AppSettings["PATH_SPOSTA_UNIRISCOSSIONE"];
                            SpostaFile(Nome_File_TrasfUVI, percorsoDestUVI);

                            DataTable datiImportazioneUvi = new Database.ImportazioneTable(ConstWrapper.sUsername.ToString()).GetRowImportazioni("versamenti", ConstWrapper.CodiceEnte.ToString(), "UVI");

                            if (datiImportazioneUvi.Rows.Count > 0)
                            {
                                lblUltimaImportazione.Text = "Ultima importazione: " + datiImportazioneUvi.Rows[0]["data"].ToString();
                                lblOperatore.Text = "Operatore: " + datiImportazioneUvi.Rows[0]["Operatore"].ToString();
                                lblFileName.Text = "File importato: " + datiImportazioneUvi.Rows[0]["FileName"].ToString();
                                lblEsito.Text = "Esito: " + (datiImportazioneUvi.Rows[0]["Importato"].ToString() == "True" ? "eseguito" : "fallito");
                                lblImpoTotImport.Text = datiImportazioneUvi.Rows[0]["importoTotImportato"].ToString();
                                lblRecImport.Text = datiImportazioneUvi.Rows[0]["toTRecordImportati"].ToString();
                            }

                            break;

                        default:
                            //altre importazioni
                            string filePath = ConfigurationManager.AppSettings["RootDocumenti"] + fileName;
                            string[] fileSplittato = this.LeggiFile(nomeFile);

                            if (new FileUploader().Upload(fileUpload.PostedFile, nomeFile))
                            {
                                ImportHelper.Import(ddlFormatoImport.SelectedValue, fileSplittato, fileName);
                                lblImportazioneInCorso.Visible = true;
                                pnlFiltriRicerca.Visible = false;
                            }
                            else {
                                strscript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Upload del file non riuscito.');";
                                RegisterScript(strscript, this.GetType());
                            }
                            break;
                    }
                }
                else
                {
                    strscript = "GestAlert('a', 'warning', '', '', 'Selezionare il file!');";
                    RegisterScript(strscript,this.GetType());
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AcquisizioneAutomaticaVersamenti.IbtnImporta_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtImportFolder_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtPathImport.Text != "")
                {
                    string tipoImp = ddlFormatoImport.SelectedValue;
                    log.Debug("devo acquisire tipo::" + tipoImp);
                    switch (tipoImp)
                    {
                        case "f24":
                            //Importazione F24
                            string sMyErr = "";
                            string Belfiore = "";
                            if (Session["COD_BELFIORE"] != null)
                                Belfiore = Session["COD_BELFIORE"].ToString();
                            string sDatiToWrite = "";
                            string percorsoF24 = ConfigurationManager.AppSettings["PATH_F24"];
                            string percorsoDestF24 = ConfigurationManager.AppSettings["PATH_F24_ACQUISITI"];
                            string NameFileLog = Belfiore + "_F24RiepilogoImport_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt";

                            string[] ListFiles = Directory.GetFiles(txtPathImport.Text, "*.RUN", SearchOption.TopDirectoryOnly);
                            foreach (string myItem in ListFiles)
                            {
                                string myFile = myItem.Replace(txtPathImport.Text + "\\", "");
                                sDatiToWrite = myFile; sMyErr = "";
                                //preleva la connessione al db ici ed il path per l'acquisizione
                                log.Debug("salvo il file al percorso::" + percorsoF24 + myFile);
                                //salvo il file sul server
                                File.Copy(txtPathImport.Text + "\\" + myFile, percorsoF24 + myFile);

                                //metodo che effettua i controlli sul file e salva i dati nella tabella TAB_ACQUISIZIONE_F24
                                string messaggio = new ImportazioneF24.ImporterF24().AvviaImportazione(percorsoF24, myFile, Business.ConstWrapper.StringConnection, Business.ConstWrapper.sUsername, ref sMyErr);
                                log.Debug("AvviaImportazione ha dato esito::" + messaggio);
                                if (messaggio == "ok")
                                {
                                    bool RetVal = new ImportazioneF24.ImporterF24().ImportSuTributo(ConstWrapper.CodiceEnte.ToString(), Business.ConstWrapper.CodiceTributo, Business.ConstWrapper.StringConnection, percorsoF24, myFile, percorsoDestF24, Belfiore, ConstWrapper.sUsername, -1, ref sMyErr);
                                    //scrivo il file
                                    log.Debug("ImportSuTributo ha dato esito::" + RetVal.ToString());
                                    if (RetVal == false)
                                    {
                                        sDatiToWrite += ";Elaborazione terminata con errori;" + sMyErr;
                                    }
                                    else
                                    {
                                        sDatiToWrite += ";Elaborazione terminata!;" + sMyErr;
                                        DataTable dtF24 = new Database.ImportazioneTable(ConstWrapper.sUsername.ToString()).GetRowImportazioni("versamenti", ConstWrapper.CodiceEnte.ToString(), "F24");
                                        if (dtF24.Rows.Count > 0)
                                        {
                                            sDatiToWrite += ";Ultima importazione: " + dtF24.Rows[0]["data"].ToString();
                                            sDatiToWrite += ";Operatore: " + dtF24.Rows[0]["Operatore"].ToString();
                                            sDatiToWrite += ";File importato: " + dtF24.Rows[0]["FileName"].ToString();
                                            sDatiToWrite += ";Esito: " + (dtF24.Rows[0]["Importato"].ToString() == "True" ? "eseguito" : "fallito");
                                            sDatiToWrite += ";TotImport " + dtF24.Rows[0]["importoTotImportato"].ToString();
                                            sDatiToWrite += ";RecImport " + dtF24.Rows[0]["toTRecordImportati"].ToString();
                                        }
                                    }
                                }
                                if (messaggio == "s")
                                {
                                    //il file non è corretto
                                    sDatiToWrite += ";Il file che si vuole acquisire non è un tracciato F24 corretto!";
                                }
                                if (messaggio == "err")
                                {
                                    //errori in elaborazione
                                    sDatiToWrite += ";Si sono verificati errori in fase di acquisizione" + sMyErr;
                                }
                                if (new CoreUtility().WriteFile(percorsoF24 + "\\" + NameFileLog, sDatiToWrite) == false)
                                {
                                    break;
                                }
                            }
                            RegisterScript("GestAlert('a', 'success', '', '', 'Importazione terminata!')", this.GetType());
                            break;
                        default:
                            //altre importazioni
                            break;
                    }
                }
                else
                {
                    RegisterScript("GestAlert('a', 'warning', '', '', 'Selezionare il percorso!');", this.GetType());
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AcquisizioneAutomaticaVersamenti.IbtnImporta_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Metodo per la stampa Excel delle dichiarazioni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
            //DataSet ds = new DataSet();
            //DataTable dtRiepilogoICI = new DataTable();
            //string sPathProspetti = string.Empty;
            //string NameXLS = string.Empty;
            //string sDatiStampa = string.Empty;
            //int nCampi;
            //ArrayList arratlistNomiColonne;
            //string[] arraystr = null;

            //arratlistNomiColonne = new ArrayList();
            //nCampi = 5;
            //try
            //{
            //    checked
            //    {
            //        for (int x = 0; x <= nCampi; x++) 
            //        {
            //            arratlistNomiColonne.Add("");
            //        }
            //    }
            //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

            //    NameXLS = ConstWrapper.CodiceEnte + "_CONTROLLOIMPORTAZIONI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

            //    ds.Tables.Add("RESULT");
            //    checked
            //    {
            //        for (int x = 0; x <= nCampi; x++)
            //        {
            //            ds.Tables["RESULT"].Columns.Add("").DataType = typeof(string);
            //        }
            //    }
            //    dtRiepilogoICI = ds.Tables["RESULT"];

            //    sDatiStampa = "Controllo Importazioni";
            //    sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
            //    Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

            //    //inserisco una riga vuota
            //    sDatiStampa = "";
            //    sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
            //    Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

            //    //inserisco intestazione di colonna
            //    sDatiStampa = "Ente|File|Da Importare|Importato|Differenza";
            //    Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

            //    TpSituazioneFinaleIci clTpSituazioneFinaleIci = new TpSituazioneFinaleIci();
            //    DataTable dtRiepilogoCalcoloICI = clTpSituazioneFinaleIci.GetStampaMinuta(ViewState["COD_CONTRIB"].ToString(), sAnno, ViewState["TRIBUTO"].ToString(), ConstWrapper.CodiceEnte);
            //    //ciclo sui dati da stampare
            //    foreach (DataRowView myRow in DvDati)
            //    {
            //        sDatiStampa = "";
            //        sDatiStampa += Utility.StringOperation.FormatString(myRow["IDENTE"]);
            //        sDatiStampa += Utility.StringOperation.FormatString(myRow["FILENAME"]);
            //        sDatiStampa += Utility.StringOperation.FormatString(myRow["DAIMPORTARE"]);
            //        sDatiStampa += Utility.StringOperation.FormatString(myRow["IMPORTATO"]);
            //        sDatiStampa += Utility.StringOperation.FormatString(myRow["DIF"]);
            //        Business.CoreUtility.AddRowStampa(DtStampa, sDatiStampa)
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICI.btnStampExcel_Click.errore: ", Ex);
            //    Response.Redirect("../../PaginaErrore.aspx");
            //}
            ////definisco l'insieme delle colonne da esportare
            //int[] iColumns = { 0, 1, 2, 3, 4};
            ////esporto i dati in excel
            //RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
            //objExport.ExportDetails(dtRiepilogoICI, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti + NameXLS);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string[] LeggiFile(String fileName)
        {
            log.Debug("Funzione LeggiFile Inizia");
            FileStream filestr = null;
            string s;
            ArrayList listaRiga = new ArrayList();
            log.Debug("Apro il file " + fileName);
            try
            {
                filestr = new FileStream(fileName, FileMode.Open);
            }
            catch (FileNotFoundException exc)
            {
                log.Error(exc.Message + " Impossibile aprire il file");
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AcquisizioneAutomaticaVersamenti.IbtnImporta_Click.errore: ", exc);
                Response.Redirect("../PaginaErrore.aspx");
                //Console.Out.Write(exc.Message);

            }
            StreamReader strRead = new StreamReader(filestr);
            log.Debug("Leggo le righe del file");
            while ((s = strRead.ReadLine()) != null && s.Length != 0)
            {
                listaRiga.Add(s);
            }
            string[] righeFile = new string[listaRiga.Count];
           int i = 0;
            foreach (string myString in listaRiga){
                righeFile[i] = myString;
                i++;
            }
            strRead.Close();
            filestr.Close();
            return righeFile;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="percorsodestinazione"></param>
        /// <returns></returns>
        public string SpostaFile(string fileName, string percorsodestinazione)
        {

            log.Debug("Funzione SpostaFile di Utility");
            string nomefilespostato = String.Empty;
            try
            {
                string data = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                string oraminuti = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                FileInfo infoFile = new FileInfo(fileName);
                string nomeFile = infoFile.Name;
                string estensione = infoFile.Extension;
                string nomeFileSenzaEstensione = nomeFile.Substring(0, (nomeFile.Length - estensione.Length));
                nomefilespostato = (percorsodestinazione + nomeFileSenzaEstensione + data + "-" + oraminuti + estensione);
                File.Move(fileName, nomefilespostato);
                log.Debug("File spostato: " + (percorsodestinazione + nomeFile + oraminuti + estensione));
            }
            catch (Exception e)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.AcquisizioneAutomaticaVersamenti.SpostaFile.errore: ", e);
                log.Error("On SpostaFile", e);
                Response.Redirect("../PaginaErrore.aspx");

            }
            return nomefilespostato;
        }
    }
}

