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
using System.Threading;
using RIBESElaborazioneDocumentiInterface.Stampa.oggetti;
using log4net;
using System.IO;

namespace OPENgovTOCO.Elaborazioni.Documenti
{
    /// <summary>
    /// Pagina per il download dei documenti elaborati
    /// </summary>
    public partial class ViewDocElaborati :BasePage
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ViewDocElaborati));
        protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                int indice = 0;
                oggettoURL[] oArrayOggettoUrl;
                GruppoURL[] objDocumentiStampate = null;
                int indiceoArrayOggettoUrl;

                if (!IsPostBack)
                {
                    //*** 20140509 - TASI ***
                    // Put user code to initialize the page here
                    if (Session["StampaPuntuale"] != null)
                    {
                        try {
                        objDocumentiStampate = (GruppoURL[])Session["StampaPuntuale"]; }
                        catch 
                        {
                            GruppoURL myGruppo = new GruppoURL();
                            ArrayList myList = new ArrayList();
                            myGruppo=(GruppoURL)Session["StampaPuntuale"];
                            myList.Add(myGruppo);
                            objDocumentiStampate= (GruppoURL[])myList.ToArray(typeof(GruppoURL));
                        }
                    }
                    else {
                        if (Request["idFlussoRuolo"] != null)
                        {
                            int idRuolo = int.Parse(Request["idFlussoRuolo"]);
                            objDocumentiStampate = new Generali.classi.Documenti().GetDocumentiElaborati(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), idRuolo);
                        }
                    }
                    indiceoArrayOggettoUrl = 0;
                    if (objDocumentiStampate != null)
                    {
                        oArrayOggettoUrl = new oggettoURL[objDocumentiStampate.Length];

                        for (indice = 0; indice < objDocumentiStampate.Length; indice++)
                        {
                            oArrayOggettoUrl[indiceoArrayOggettoUrl] = objDocumentiStampate[indice].URLComplessivo;
                            indiceoArrayOggettoUrl++;
                        }
                        Session["StampaDocOSAP"] = oArrayOggettoUrl;
                        DataBindGrid((oggettoURL[])Session["StampaDocOSAP"], 0);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DownloadDocumenti.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
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
                            byte[] FileToDownLoad = GetAttachmentFile(PathFileToOpen);

                            if (FileToDownLoad.Length > 0)
                            {
                                //imposta le headers 
                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + NomeFile + "\"");
                                Response.AddHeader("Content-Length", FileToDownLoad.Length.ToString());
                                Response.ContentType = "application/octet-stream";

                                //leggo dal file e scrivo nello stream di risposta 
                                //Response.WriteFile(fileName)
                                Response.BinaryWrite(FileToDownLoad);
                            }
                            else if (Url != "")
                            {
                                Log.Debug("chiamata a downloadwebfile");
                                downloadwebfile(Url, NomeFile, PathFileToOpen);
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
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DownloadDocumenti.GrdRowCommand.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            DataBindGrid((oggettoURL[])Session["StampaDocOSAP"],  e.NewPageIndex);
        }
        #endregion
        private void DataBindGrid(oggettoURL[] oArrayOggettoUrl, int? page = 0)
        {
            try
            {
                    GrdDocumenti.DataSource = oArrayOggettoUrl;
                    if (page.HasValue)
                        GrdDocumenti.PageIndex = page.Value;
                    GrdDocumenti.DataBind();
                    GrdDocumenti.Visible = true;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.RicercaDoc.DataBindGridElaborazioni.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        public byte[] GetAttachmentFile(string FileToOpen)
        {
            //Public Function GetAttachmentFile(ByVal FileName As String, ByVal AttachmentType As TQS.Commons.SharedObjects.TQSEnum.eAttachmentType) As Byte()

            byte[] ArrByte = new byte[0];

            System.IO.FileInfo FI = new System.IO.FileInfo(FileToOpen);
            System.IO.FileStream oFileStream;
            try
            {
                if (FI.Exists)
                {

                    oFileStream = FI.OpenRead();
                    long lBytes = oFileStream.Length;

                    // ERROR: Not supported in C#: ReDimStatement
                    ArrByte = new byte[lBytes];

                    oFileStream.Read(ArrByte, 0, int.Parse(lBytes.ToString()));

                }

                return ArrByte;
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DownloadDocumenti.GetAttachmentFile.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }

        public void downloadwebfile(string path_web, string NOME_FILE, string Path)
        {
            System.Net.WebClient WC = new System.Net.WebClient();
            string FileName, sScript;
            string strPath = string.Empty;
            Uri cUri;
            DirectoryInfo di;
            string sData;
            string strPathZip;

            try
            {

                strPathZip = System.Configuration.ConfigurationManager.AppSettings["PATH_ZIP"].ToString();
                if (strPathZip.Substring(strPathZip.Length - 1, 1) != "\\")
                    strPathZip += "\\";


                sData = DateTime.Now.ToString();
                sData = sData.Replace("/", "").Replace(".", "").Replace(" ", "");

                WC.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Log.Debug("Creazione cartella " + strPath);
                strPath = strPathZip + sData + "\\";
                di = new DirectoryInfo(strPath);
                if (!di.Exists)
                {

                    di.Create();
                    Log.Debug("Creazione cartella effettuata");
                }


                FileName = strPath + NOME_FILE;
                cUri = new Uri(path_web);
                WC.BaseAddress = path_web;
                WC.DownloadFile(cUri.ToString(), FileName);
                Log.Info("Salvataggio del file '" + path_web + "' in '" + FileName + "' effettuato");

                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment; filename=" + NOME_FILE);
                Response.WriteFile(FileName);
                Response.Flush();

                Log.Info("Cancellazione della cartella " + di.FullName);
                if (di.Exists) { di.Delete(true); }

                if (File.Exists(FileName)) { File.Delete(FileName); }
                Log.Info("Cancellazione del file " + FileName);

                Thread.Sleep(1000);
            }
            catch (UnauthorizedAccessException Uex)
            {
                Log.Error("Non si dispone delle credenziali per accedere al percorso " + strPath + " :: " + Uex.Message);
                Log.Debug("Non si dispone delle credenziali per accedere al percorso " + strPath);
                sScript = "GestAlert('a', 'danger', '', '', 'Non si dispone delle credenziali per accedere al percorso '+ " + strPath + "');";
                RegisterScript(sScript,this.GetType());
            }
            catch (System.Security.SecurityException sex)
            {
                Log.Error("SecurityException: " + sex.Message);
                Log.Debug("SecurityException: " + sex.Message);
                sScript = "GestAlert('a', 'danger', '', '', '" + sex.Message + "');";
                RegisterScript(sScript,this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DownloadDocumenti.downloadwebfile.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                WC.Dispose();
                sScript = "document.getElementById('download').className = 'downloadh';";
                RegisterScript(sScript,this.GetType());
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
	}
}
