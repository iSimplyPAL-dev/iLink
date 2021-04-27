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
using RIBESElaborazioneDocumentiInterface.Stampa.oggetti;

using System.IO;
using System.Net;
using System.Security;
using System.Threading ;
using System.Configuration; 
using log4net;
using log4net.Config;

namespace DichiarazioniICI.ElaborazioneDocumenti
{
    /// <summary>
    /// Pagina per il download dei documenti.
    /// </summary>	
    public partial class DownloadDocumenti : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DownloadDocumenti));

        protected void Page_Load(object sender, System.EventArgs e)
        {
            log.Debug("DichiarazioniICI.ElaborazioneDocumenti.DownloadDocumenti.Page_Load");
            int indice = 0;
            oggettoURL[] oArrayOggettoUrl;
            GruppoURL[] objDocumentiStampate = null;
            int indiceoArrayOggettoUrl;
            try
            {
                if (!IsPostBack)
                {
                    //*** 20140509 - TASI ***
                    // Put user code to initialize the page here
                    if (Session["StampaPuntuale"] != null)
                    {
                        log.Debug("session Stampapuntuale");
                        //oGruppoURL= (GruppoURL)Session["StampaPuntuale"];
                        // oggetto che definisce i documenti dei singoli contribuenti
                        //GrdDaElaborare.DataSource = oGruppoURL.URLGruppi;					
                        objDocumentiStampate = new GruppoURL[1];
                        objDocumentiStampate[0] = (GruppoURL)Session["StampaPuntuale"];
                    }
                    else {
                        if (Request["idFlussoRuolo"] != null)
                        {
                        log.Debug("request idflussoruolo");
                            int idRuolo = int.Parse(Request["idFlussoRuolo"]);
                            objDocumentiStampate = new Database.TpSituazioneFinaleIci().GetDocElaboratiEffettivi(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodiceTributo, idRuolo);
                        }
                    }
                    indiceoArrayOggettoUrl = 0;
                    if (objDocumentiStampate != null)
                    {
                        log.Debug("ho oggetto stampato");
                        oArrayOggettoUrl = new oggettoURL[objDocumentiStampate.Length];

                        for (indice = 0; indice < objDocumentiStampate.Length; indice++)
                        {
                            oArrayOggettoUrl[indiceoArrayOggettoUrl] = objDocumentiStampate[indice].URLComplessivo;
                            indiceoArrayOggettoUrl++;
                        }
                        GrdDaElaborare.DataSource = oArrayOggettoUrl;
                        GrdDaElaborare.DataBind();
                    }
                    /*
                                Dim objDocumentiStampate() As GruppoURL
                                Dim oArrayOggettoUrl() As oggettoURL

                                objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
                                indiceoArrayOggettoUrl = 0
                                For indice = 0 To objDocumentiStampate.Length - 1
                                    If Not IsNothing(objDocumentiStampate(indice)) Then
                                        ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
                                        oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
                                        indiceoArrayOggettoUrl += 1
                                    End If
                                Next
                            */
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DownloadDocumenti.Page_Load.errore: ", Err);
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
            //this.GrdDaElaborare.ItemCommand += new DataGridCommandEventHandler(GrdDaElaborare_ItemCommand);
        }
        #endregion

        #region "Griglie"
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    foreach (GridViewRow myRow in GrdDaElaborare.Rows)
                    {
                        if (IDRow == ((HiddenField)myRow.FindControl("hfid")).Value)
                        {
                            string PathFileToOpen = GrdDaElaborare.Rows[myRow.RowIndex].Cells[2].Text;
                            string NomeFile = GrdDaElaborare.Rows[myRow.RowIndex].Cells[0].Text;
                            string Url = GrdDaElaborare.Rows[myRow.RowIndex].Cells[3].Text;
                            log.Debug("URL:" + Url);
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
                                log.Debug("chiamata a downloadwebfile");
                                downloadwebfile(Url, NomeFile, PathFileToOpen);
                            }
                            else
                            {
                                string strScript = "";
                                strScript = strScript + "GestAlert('a', 'warning', '', '', 'Il documento non è stato trovato!');";
                                strScript = strScript + "";
                                RegisterScript(strScript, this.GetType());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DownloadDocumenti.GrdRowCommand.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #endregion

        //      private void GrdDaElaborare_ItemCommand(object sender, DataGridCommandEventArgs e)
        //{
        //try{
        //	if (e.CommandName == "S")
        //	{
        //		string PathFileToOpen = GrdDaElaborare.Items[e.Item.ItemIndex].Cells[2].Text;
        //		string NomeFile = GrdDaElaborare.Items[e.Item.ItemIndex].Cells[0].Text;
        //		string Url = GrdDaElaborare.Items[e.Item.ItemIndex].Cells[3].Text;
        //		log.Debug ("URL:"+Url);
        //		byte[] FileToDownLoad = GetAttachmentFile(PathFileToOpen);		

        //		if (FileToDownLoad.Length > 0)
        //		{
        //			//imposta le headers 
        //			Response.Clear();
        //			Response.AddHeader("Content-Disposition", "attachment; filename=\"" + NomeFile + "\"");
        //			Response.AddHeader("Content-Length", FileToDownLoad.Length.ToString());
        //			Response.ContentType = "application/octet-stream";

        //			//leggo dal file e scrivo nello stream di risposta 
        //			//Response.WriteFile(fileName)
        //			Response.BinaryWrite(FileToDownLoad);
        //		}
        //		else if (Url!=""){
        //			log.Debug ("chiamata a downloadwebfile");
        //			downloadwebfile(Url,NomeFile,PathFileToOpen);
        //		}
        //		else
        //		{
        //			string strScript = "";
        //			strScript = strScript + "alert('Il documento non è stato trovato!');";
        //			strScript = strScript + "";

        //			RegisterScript(sScript,this.GetType());,"strMod", strScript);
        //		}
        //	}
        //}
        //    catch (Exception ex)
        //  {
        //     log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DownloadDocumenti.GrdDaElaborare_ItemCommand.errore: ", ex);
        //  Response.Redirect("../../PaginaErrore.aspx");
        // }
        //}

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
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DownloadDocumenti.GetAttachmentFile.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }

        void downloadwebfile(string path_web, string NOME_FILE, string Path)
        {
            WebClient WC = new WebClient();
            string FileName, sScript;
            string strPath = string.Empty;
            Uri cUri;
            DirectoryInfo di;
            string sData;
            string strPathZip;

            try
            {

                strPathZip = ConfigurationManager.AppSettings["PATH_ZIP"].ToString();
                if (strPathZip.Substring(strPathZip.Length - 1, 1) != "\\")
                    strPathZip += "\\";


                sData = DateTime.Now.ToString();
                sData = sData.Replace("/", "").Replace(".", "").Replace(" ", "");

                WC.Credentials = CredentialCache.DefaultCredentials;

                log.Debug("Creazione cartella " + strPath);
                strPath = strPathZip + sData + "\\";
                di = new DirectoryInfo(strPath);
                if (!di.Exists)
                {

                    di.Create();
                    log.Debug("Creazione cartella effettuata");
                }


                FileName = strPath + NOME_FILE;
                cUri = new Uri(path_web);
                WC.BaseAddress = path_web;
                WC.DownloadFile(cUri.ToString(), FileName);
                log.Info("Salvataggio del file '" + path_web + "' in '" + FileName + "' effettuato");

                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment; filename=" + NOME_FILE);
                Response.WriteFile(FileName);
                Response.Flush();

                log.Info("Cancellazione della cartella " + di.FullName);
                if (di.Exists) { di.Delete(true); }

                if (File.Exists(FileName)) { File.Delete(FileName); }
                log.Info("Cancellazione del file " + FileName);

                Thread.Sleep(1000);
            }
            //		 catch (IO.DirectoryNotFoundException dex)
            //		 {
            //			 //log.Error("Non è stato trovato il percorso " + strPath);
            //			 //log.Debug("Non è stato trovato il percorso " + strPath);
            //			 sScript = "alert('Non è stato trovato il percorso '+ " + strPath + " )";
            //			 RegisterScript(sScript,this.GetType());,"savefile", "" + sScript + "");
            //		 }
            catch (UnauthorizedAccessException Uex)
            {
                log.Error("Non si dispone delle credenziali per accedere al percorso " + strPath + " :: " + Uex.Message);
                log.Debug("Non si dispone delle credenziali per accedere al percorso " + strPath);
                sScript = "GestAlert('a', 'warning', '', '', 'Non si dispone delle credenziali per accedere al percorso '+ " + strPath + " )";
                RegisterScript(sScript, this.GetType());
            }
            catch (SecurityException sex)
            {
                log.Error("SecurityException: " + sex.Message);
                log.Debug("SecurityException: " + sex.Message);
                sScript = "GestAlert('a', 'danger', '', '', '" + sex.Message + "')";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DownloadDocumenti.downloadwebfile.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                sScript = "GestAlert('a', 'danger', '', '', '" + ex.Message + "')";
                RegisterScript(sScript, this.GetType());
            }
            finally
            {
                WC.Dispose();
                RegisterScript("document.getElementById('download').className = 'downloadh';", this.GetType());
            }
        }
    }
}