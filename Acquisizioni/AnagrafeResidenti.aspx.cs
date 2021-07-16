using System;
using System.Globalization;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using log4net;

namespace OPENgov.Acquisizioni
{/// <summary>
/// Pagina per la consultazione/importazione dei flussi di carico.
/// Le possibili opzioni sono:
/// - Importa
/// - Carica residenti da tributi
/// </summary>
    public partial class AnagrafeResidenti : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AnagrafeResidenti));
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "Dati Esterni - Acquisizione";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGridView(0);

                TypesAnagfile[] anagfiles = new TypesAnagfile().LoadAll();
                if (anagfiles == null)
                {
                    fileUpload.Enabled = cmdImports.Enabled = false;
                    lblMessage.Text = "Errore nel caricamento delle tipologie di file dal DB.";
                    lblMessage.Visible = true;
                }
                else if (anagfiles.Length == 0)
                {
                    fileUpload.Enabled = cmdImports.Enabled = false;
                    lblMessage.Text = "Nessun tipo di file presente nel DB.";
                    lblMessage.Visible = true;
                }
                else
                {
                    cmbFileType.DataSource = anagfiles;
                    cmbFileType.DataTextField = "AnagFileName";
                    cmbFileType.DataValueField = "IdAnagFileType";
                    cmbFileType.DataBind();
                }
            }
        }

        private void LoadGridView(int page)
        {
            try
            {
                AnagFile anag = new AnagFile(new DBConfig().DBType, new DBConfig().ConnectionStringANAGRAFICA) { Ente = Ente };
                AnagFile[] anagfiles = anag.LoadFiltered();
                gvAnagFiles.DataSource = anagfiles;
                gvAnagFiles.PageIndex = page;
                gvAnagFiles.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AnagrafeResidenti.LoadGridView.errore::", ex);
                throw;
            }
        }
        /// <summary>
        /// Funzione per il caricamento dei flussi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUpdateClick(object sender, EventArgs e)
        {
            int idanagfiletype;
            AnagFile anagfile = new AnagFile(new DBConfig().DBType, new DBConfig().ConnectionStringANAGRAFICA);
            if (!int.TryParse(cmbFileType.SelectedValue, out idanagfiletype))
                lblMessage.Text = "Selezionare una tipologia valida.";
            else
            {
                anagfile.FileMIMEType = fileUpload.PostedFile.ContentType;
                anagfile.Ente = Ente;
                anagfile.PostedFile = fileUpload.FileBytes;
                anagfile.FileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                anagfile.IdAnagFileType = idanagfiletype;
                if (!anagfile.Save())
                    lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
                else
                {
                    lblMessage.Text = "File caricato con successo.";
                    lblMessage.CssClass = "";

                    Module.SetNextRun(GetModuleToRun(idanagfiletype), DateTime.Now);
                }
            }
            lblMessage.Visible = true;
            fileUpload = new FileUpload();
            LoadGridView(0);
        }
        protected void cmdResFromTributiClick(object sender, EventArgs e)
        {
            try
            {
                int idanagfiletype;
                AnagFile anagfile = new AnagFile(new DBConfig().DBType, new DBConfig().ConnectionStringANAGRAFICA);
                if (!int.TryParse(cmbFileType.SelectedValue, out idanagfiletype))
                    lblMessage.Text = "Selezionare una tipologia valida.";
                else
                {
                    if (idanagfiletype != 1)
                    {
                        lblMessage.Text = "Tipo di file non valido.";
                        string sScript = "<script language='javascript'>";
                        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                        sScript += "alert('Tipo di file non valido!');";
                        sScript += "</script>";
                        sBuilder.Append(sScript);
                        ClientScript.RegisterStartupScript(this.GetType(), "fnv", sBuilder.ToString());
                    }
                    else
                    {
                        anagfile.Ente = Ente;
                        anagfile.PostedFile = null;
                        anagfile.FileMIMEType = "query";
                        anagfile.FileName = "Residenti da Tributi";
                        anagfile.IdAnagFileType = idanagfiletype;
                        if (!anagfile.Save())
                            lblMessage.Text = "Si sono verificati errori durante il salvataggio del file.";
                        else
                        {
                            lblMessage.Text = "File caricato con successo.";
                            lblMessage.CssClass = "";

                            Module.SetNextRun(GetModuleToRun(idanagfiletype), DateTime.Now);
                        }
                    }
                }
                lblMessage.Visible = true;
                fileUpload = new FileUpload();
                LoadGridView(0);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AnagrafeResidenti.cmdResFromTributiClick.errore::", ex); ;
                throw;
            }
        }

        private string GetModuleToRun(int fileType)
        {
            switch (fileType)
            {
                case 1:
                    return "AcquireResidenti";
                case 2:
                case 3:
                    return "AcquireCatasto";
                case 4:
                    return "AcquireSoggetti";
                case 5:
                    return "AcquireTitoli";
                case 6:
                    return "AcquireCompraVendita";
                case 7:
                case 8:
                    return "AcquireDOCFA";
            }
            return null;
        }

        protected void GvAnagFilesPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex);
        }
        /// <summary>
        /// Funzione per la gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void GvAnagFilesRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idanagfile;
            if (int.TryParse(e.CommandArgument.ToString(), out idanagfile))
            {
                AnagFile anagfile = new AnagFile(new DBConfig().DBType, new DBConfig().ConnectionStringANAGRAFICA) { IdAnagFile = idanagfile };
                if (anagfile.Load())
                {
                    switch (e.CommandName.ToLower())
                    {
                        case "download":
                            Response.ContentType = anagfile.FileMIMEType;
                            Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", anagfile.FileName));
                            Response.BinaryWrite(anagfile.PostedFile);
                            Response.End();
                            break;
                        case "viewlog":
                            LoadLog(anagfile);
                            break;
                    }
                }
            }
        }
        //protected void GvAnagFilesRowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int idanagfile;
        //    if (int.TryParse(e.CommandArgument.ToString(), out idanagfile))
        //    {
        //        AnagFile anagfile = new AnagFile(idanagfile);
        //        if (anagfile.Load())
        //        {
        //            switch (e.CommandName.ToLower())
        //            {
        //                case "download":
        //                    Response.ContentType = anagfile.FileMIMEType;
        //                    Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", anagfile.FileName));
        //                    Response.BinaryWrite(anagfile.PostedFile);
        //                    Response.End();
        //                    break;
        //                case "viewlog":
        //                    LoadLog(anagfile);
        //                    break;
        //            }
        //        }
        //    }
        //}

        private void LoadLog(AnagFile anagfile)
        {
            try
            {
                RowLog[] logs = anagfile.LoadAllLogs();
                dlLog.DataSource = logs;
                dlLog.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AnagrafeResidenti.LoadLog.errore::", ex); ;
                throw;
            }
        }
        /// <summary>
        /// Funzione per il caricamento dinamico dell'immagine in griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void DlLogItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                RowLog row = (RowLog)e.Item.DataItem;
                var image = e.Item.FindControl("imgSeverity") as Image;
                if (image != null)
                    image.CssClass = string.Format("BottoneDialog{0}_grd.png", (row.Severity == LogSeverity.Critical) ? "Error" : "Warning");
                var rowNumber = e.Item.FindControl("lblRow") as Label;
                if (rowNumber != null)
                    rowNumber.Text = row.Row.ToString(CultureInfo.InvariantCulture);
                var label = e.Item.FindControl("lblError") as Label;
                if (label != null)
                    label.Text = row.Error;
            }
        }

        //protected void DlLogItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    if (e.Item.DataItem != null)
        //    {
        //        RowLog row = (RowLog)e.Item.DataItem;
        //        var image = e.Item.FindControl("imgSeverity") as Image;
        //        if (image != null)
        //            image.ImageUrl = string.Format("~/Images/icon_error_small_{0}.png", (row.Severity == LogSeverity.Critical) ? "critical" : "warning");
        //        var rowNumber = e.Item.FindControl("lblRow") as Label;
        //        if (rowNumber != null)
        //            rowNumber.Text = row.Row.ToString(CultureInfo.InvariantCulture);
        //        var label = e.Item.FindControl("lblError") as Label;
        //        if (label != null)
        //            label.Text = row.Error;
        //    }
        //}
    }
}