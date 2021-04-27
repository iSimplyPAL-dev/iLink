using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DichiarazioniICI
{
 /// <summary>
/// Pagina per l'aggiornamento della banca dati tributaria con i dati repertiti da flusso catastale.
/// La procedura parte in automatico ogni 30minuti, se si ha la gestione del verticale bisogna prima estrarre la banca dati. Procedere quindi nell'ordine con:
/// - carico dei flussi (selezionare i flussi di catasto e di banca dati tributaria)
/// - estrazione incrocio (estrarre la nuova situazione dichiarativa a seguito di incrocio con catasto)
/// - estrazione anomalie (estrarre i prospetti di anomalie)
/// - se si ha la gestione del verticale (importazione incrocio)
/// si può controllare lo stato di avanzamento dalla voce monitoraggio
/// Le possibili opzioni sono:
/// - Upload flussi
/// - Stampa
/// - Ricerca
/// </summary>
/// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
   public partial class ExportCatasto : BaseEnte
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExportCatasto));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            try
            {
                string sScript = string.Empty;
                sScript = "$('#divExpImpBancaDati').hide();$('#divExpImpPeriodo').hide();$('#divImpDich').hide();$('#divUpload').hide();$('#divAnomalie').hide();$('#divResult').hide();$('#divMonitoring').hide();";
                sScript += "$('#lblError').hide();";
                sScript += "$('#Print').hide();$('#Search').hide();$('#Upload').hide();";
                sScript += "$('#lblBelfiore').html('<p>Codice Catastale <strong>"+ Business.ConstWrapper.CodBelfiore+"</strong></p>');";
                RegisterScript(sScript, this.GetType());
                lblEnte.Text = Business.ConstWrapper.DescrizioneEnte;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.OnInti.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            base.OnInit(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            //in base al tipo di chiamata visualizzo il solo div che interessa
            try
            {
                if (Request.QueryString["Type"].ToString() == Catasto.Fase.EstrazioneBancaDatiTrib.ToString() || Request.QueryString["Type"].ToString() == Catasto.Fase.EstrazioneDichWork.ToString() || Request.QueryString["Type"].ToString() == Catasto.Fase.ImportDichCat.ToString())
                {
                    sScript = "$('#divExpImpBancaDati').show();$('#Print').show();$('#Search').show();";
                }
                else if (Request.QueryString["Type"].ToString() == Catasto.Fase.UploadFile.ToString())
                {
                    sScript = "$('#divUpload').show();$('#Upload').show();";
                    sScript += GetFilesUploaded();
                }
                else if (Request.QueryString["Type"].ToString() == Catasto.Fase.EstrazioneAnomalie.ToString())
                {
                    sScript = "$('#divAnomalie').show();$('#Print').show();$('#Search').show();";
                }
                else if (Request.QueryString["Type"].ToString() == Catasto.Fase.Monitoring.ToString())
                {
                    sScript = "$('#divMonitoring').show();";
                    if (!Page.IsPostBack)
                    {
                        DataTable dtMyDati = new DataTable();
                        string sMyErr;
                        sScript = sMyErr = string.Empty;
                        if (!new Catasto().Monitoring(Business.ConstWrapper.CodiceEnte, -1, out dtMyDati, out sMyErr))
                        {
                            sScript = "$('#lblError').text('Errore in estrazione Monitoraggio!');$('#lblError').show();";
                        }
                        if (!(dtMyDati == null))
                        {
                            GrdMonitorGen.DataSource = dtMyDati;
                            GrdMonitorGen.DataBind();
                            if (GrdMonitorGen.Rows.Count > 0)
                            {
                                GrdMonitorGen.Visible = true;
                            }
                            else {
                                GrdMonitorGen.Visible = false;
                                sScript = "$('#lblError').text('Nessuna posizione trovata.');$('#lblError').show();";
                            }
                            sScript += "$('#divMonitoring').show();";
                        }
                        else
                        {
                            if (sMyErr != string.Empty)
                            {
                                sScript = "$('#lblError').text('" + sMyErr + "');$('#lblError').show();";
                            }
                        }
                    }
                    RegisterScript(sScript, this.GetType());
                }
                sScript += "$('#lblError').hide();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        #region "Griglie"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string sScript, sMyErr;
                sScript = sMyErr = string.Empty;
                DataTable dtMyDati = new DataTable();
                int IDRow = 0;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                if (e.CommandName == "RowOpen")
                {
                    if (!new Catasto().Monitoring(Business.ConstWrapper.CodiceEnte, IDRow, out dtMyDati, out sMyErr))
                    {
                        sScript = "$('#lblError').text('Errore in estrazione Monitoraggio!');$('#lblError').show();";
                    }
                    else {
                        GrdMonitorDet.DataSource = dtMyDati;
                        GrdMonitorDet.DataBind();
                        if (GrdMonitorDet.Rows.Count > 0)
                        {
                            GrdMonitorDet.Visible = true;
                            LoadColumnChart(dtMyDati);
                        }
                        else {
                            GrdMonitorDet.Visible = false;
                            sScript = "$('#lblError').text('Nessuna posizione trovata.');$('#lblError').show();";
                        }
                    }
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.GrdRowCommand.errore: ", ex);
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
            LoadSearch(e.NewPageIndex);
        }
        #endregion
        #region "Bottoni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdSearch_Click(object sender, System.EventArgs e)
        {
            string sScript, sMyErr;
            sScript = sMyErr = string.Empty;
            try
            {
                LblDownloadFile.Text = "";
                DataTable dtMyDati = new DataTable();
                int nType = 0;
                int.TryParse(Request.QueryString["Type"].ToString(), out nType);
                if (nType == Catasto.Fase.EstrazioneBancaDatiTrib)
                {
                    if (optExpDich.Checked)
                    {
                        if (!new Catasto().SearchBancaDati(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodBelfiore, DateTime.Parse(txtDal.Text), DateTime.Parse(txtAl.Text), out dtMyDati, out sMyErr))
                        {
                            sScript = "$('#lblError').text('Errore in estrazione Banca Dati!');$('#lblError').show();";
                        }
                    }
                    else if (optExpDichWork.Checked)
                    {
                        if (!new Catasto().SearchDichWork(Business.ConstWrapper.CodBelfiore, false, out dtMyDati, out sMyErr))
                        {
                            sScript = "$('#lblError').text('Errore in estrazione Dichiarazioni da Catasto!');$('#lblError').show();";
                        }
                    }
                }
                else if (nType == Catasto.Fase.EstrazioneAnomalie)
                {
                    int TypeAnomalia = 0;
                    if (optTitNoSog.Checked)
                        TypeAnomalia = Catasto.Anomalia.TitNoSog;
                    else if (optSogNoTit.Checked)
                        TypeAnomalia = Catasto.Anomalia.SogNoTit;
                    else if (optTitNoFab.Checked)
                        TypeAnomalia = Catasto.Anomalia.TitNoFab;
                    else if (optFabNoTit.Checked)
                        TypeAnomalia = Catasto.Anomalia.FabNoTit;
                    else if (optNoPoss.Checked)
                        TypeAnomalia = Catasto.Anomalia.NoPoss;
                    else if (optNoSogRif.Checked)
                        TypeAnomalia = Catasto.Anomalia.NoSogRif;
                    else if (optNoDiritto.Checked)
                        TypeAnomalia = Catasto.Anomalia.NoDiritto;
                    else if (optRurale.Checked)
                        TypeAnomalia = Catasto.Anomalia.Rurale;
                    else if (optCatasto.Checked)
                        TypeAnomalia = Catasto.Anomalia.Catasto;

                    if (!new Catasto().SearchAnomalie(Business.ConstWrapper.CodBelfiore, TypeAnomalia, false,string.Empty, string.Empty, string.Empty, out dtMyDati, out sMyErr))
                    {
                        sScript = "$('#lblError').text('Errore in estrazione Anomalie!');$('#lblError').show();";
                    }
                }

                if (!(dtMyDati == null))
                {
                    Session["dtSearchCatasto"] = dtMyDati;
                    while (GrdResult.Columns.Count > 0)
                    {
                        DropColGrd();
                    }
                    foreach (DataColumn myCol in dtMyDati.Columns)
                    {
                        BoundField myGrdCol = new BoundField();
                        myGrdCol.HeaderText = myCol.ColumnName;
                        myGrdCol.DataField = myCol.ColumnName;
                        GrdResult.Columns.Add(myGrdCol);
                    }
                    GrdResult.DataSource = dtMyDati;
                    GrdResult.DataBind();
                    if (GrdResult.Rows.Count > 0)
                    {
                        GrdResult.Visible = true;
                    }
                    else {
                        GrdResult.Visible = false;
                        sScript = "$('#lblError').text('Nessuna posizione trovata.');$('#lblError').show();";
                    }
                    sScript += "$('#divResult').show();";
                }
                else
                {
                    if (sMyErr != string.Empty)
                    {
                        sScript = "$('#lblError').text('" + sMyErr + "');$('#lblError').show();";
                    }
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.CmdSearch_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdPrint_Click(object sender, System.EventArgs e)
        {
            string sPathProspetti = string.Empty;
            string NameXLS = string.Empty;
            DataTable dtMyDati = new DataTable();
            string sScript, sMyErr;
            sScript = sMyErr = string.Empty;
            int nType = 0;
            try
            {
                int.TryParse(Request.QueryString["Type"].ToString(), out nType);
                if (nType == Catasto.Fase.EstrazioneBancaDatiTrib)
                {
                    if (optExpDich.Checked)
                    {
                        NameXLS = Business.ConstWrapper.CodiceEnte+ "_DICH_BANCADATITRIBUTARIA_" + Business.ConstWrapper.CodBelfiore  + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
                        if (!new Catasto().SearchBancaDati(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodBelfiore, DateTime.Parse(txtDal.Text), DateTime.Parse(txtAl.Text), out dtMyDati, out sMyErr))
                        {
                            sScript = "$('#lblError').text('Errore in estrazione Banca Dati!');$('#lblError').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                    }
                    else if (optExpDichWork.Checked)
                    {
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_DICHDACATASTO_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
                        if (!new Catasto().SearchDichWork(Business.ConstWrapper.CodBelfiore, true, out dtMyDati, out sMyErr))
                        {
                            sScript = "$('#lblError').text('Errore in estrazione Dichiarazioni da Catasto!');$('#lblError').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                    }
                }
                else if (nType == Catasto.Fase.EstrazioneAnomalie)
                {
                    int TypeAnomalia = 0;
                    if (optTitNoSog.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.TitNoSog;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_TITNOSOG_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optSogNoTit.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.SogNoTit;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_SOGNOTIT_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optTitNoFab.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.TitNoFab;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_TITNOFAB_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optFabNoTit.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.FabNoTit;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_FABNOTIT_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optNoPoss.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.NoPoss;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_NOPOSS_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optNoSogRif.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.NoSogRif;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_NOSOGRIF_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optNoDiritto.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.NoDiritto;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_NODIRITTO_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optRurale.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.Rurale;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_RURALE_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    else if (optCatasto.Checked)
                    {
                        TypeAnomalia = Catasto.Anomalia.Catasto;
                        NameXLS = Business.ConstWrapper.CodBelfiore + "_CATASTO_" + Business.ConstWrapper.CodiceEnte + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
                    }
                    if (!new Catasto().SearchAnomalie(Business.ConstWrapper.CodBelfiore, TypeAnomalia, true, string.Empty, string.Empty, string.Empty, out dtMyDati, out sMyErr))
                    {
                        sScript = "$('#lblError').text('Errore in estrazione Anomalie!');$('#lblError').show();";
                        RegisterScript(sScript, this.GetType());
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.CmdPrint_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            sPathProspetti = Business.ConstWrapper.PathProspetti;
            if (dtMyDati != null)
            {
                if (sMyErr != string.Empty)
                {
                    sScript = "$('#lblError').text('" + sMyErr + "');$('#lblError').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    if (nType == Catasto.Fase.EstrazioneBancaDatiTrib)
                    {
                        var writer = new StreamWriter(sPathProspetti + NameXLS);
                        try
                        {
                            string NewLine = string.Empty;
                            foreach (DataColumn myCol in dtMyDati.Columns)
                            {
                                NewLine += myCol.ColumnName + ";";
                            }
                            writer.WriteLine(NewLine);
                            foreach (DataRow myRow in dtMyDati.Rows)
                            {
                                NewLine = string.Empty;
                                foreach (DataColumn myCol in dtMyDati.Columns)
                                {
                                    NewLine += myRow[myCol.ColumnName].ToString() + ";";
                                }
                                writer.WriteLine(NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.CmdPrint_Click.errore: ", ex);
                            Response.Redirect("../../PaginaErrore.aspx");
                            sScript = "$('#lblError').text('Errore in estrazione!');$('#lblError').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                        finally
                        {
                            writer.Flush();
                            writer.Close();
                        }
                    }
                    else
                    {
                        if (dtMyDati.Rows.Count > 0)
                        {
                            //definisco le colonne
                            List<string> MyHeaders = new List<string>();
                            foreach (DataColumn myCol in dtMyDati.Columns)
                            {
                                MyHeaders.Add(myCol.ColumnName);
                            }
                            //definisco l'insieme delle colonne da esportare
                            List<int> MyCol = new List<int>();
                            checked
                            {
                                for (int x = 0; x < dtMyDati.Columns.Count; x++)
                                {
                                    MyCol.Add(x);
                                }
                            }
                            //esporto i dati in excel
                            RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Win");
                            objExport.ExportDetails(dtMyDati, MyCol.ToArray(), MyHeaders.ToArray(), RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti + NameXLS);
                        }
                        else
                        {
                            sScript = "$('#lblError').text('Nessuna posizione trovata.');$('#lblError').show();";
                            NameXLS = string.Empty;
                        }
                    }
                    if (NameXLS != string.Empty)
                        LblDownloadFile.Text = NameXLS;
                    sScript += "$('#divResult').show();";
                    RegisterScript(sScript, this.GetType());
                }
            }
            else
            {
                sScript = "$('#lblError').text('" + sMyErr + "');$('#lblError').show();";
                RegisterScript(sScript, this.GetType());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdUpload_Click(object sender, System.EventArgs e)
        {
            string sScript, sMyErr;
            sScript = sMyErr = string.Empty;
            try
            {
                LblDownloadFile.Text = "";
                if (optImpDich.Checked)
                {
                    CatastoInterface.Elaborazione myElab = new CatastoInterface.Elaborazione();
                    //se l'elaborazione da catasto non è finita non posso importare in verticale
                    if (!new Catasto().CheckElabInCorso(Business.ConstWrapper.CodBelfiore, false, out myElab))
                    {
                        if (!new Catasto().UploadRibaltaFiles(Business.ConstWrapper.CodBelfiore, Request.Files, out sMyErr))
                        {
                            sScript = "$('#lblError').text('Errore in caricamento flusso!');$('#lblError').show();";
                        }
                        else
                        {
                            if (sMyErr != string.Empty)
                            {
                                sScript = "$('#lblError').text('" + sMyErr + "');$('#lblError').show();";
                            }
                            else
                            {
                                sScript = "$('#lblError').text('Upload flusso effettuato con successo!');$('#lblError').show();";
                                if (Business.ConstWrapper.PathCatastoBatchFile != string.Empty)
                                {
                                    System.Diagnostics.Process.Start(Business.ConstWrapper.PathCatastoBatchFile);
                                }
                            }
                        }
                    }
                    else
                    {
                        sScript = "$('#lblError').text('Elaborazione in corso. Impossibile proseguire.');$('#lblError').show();";
                    }
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    if (!new Catasto().UploadFiles(Business.ConstWrapper.CodBelfiore, Request.Files, out sMyErr))
                    {
                        sScript = "$('#lblError').text('Errore in caricamento flussi!');$('#lblError').show();";
                    }
                    else
                    {
                        if (sMyErr != string.Empty)
                        {
                            sScript = "$('#lblError').text('" + sMyErr + "');$('#lblError').show();";
                        }
                        else
                        {
                            sScript = "$('#lblError').text('Upload flussi effettuato con successo!');$('#lblError').show();";
                            sScript += GetFilesUploaded();
                        }
                    }
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.CmdUpload_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LblDownloadFile_Click(object sender, System.EventArgs e)
        {
            string sFileExport = Business.ConstWrapper.PathProspetti + LblDownloadFile.Text;
            Response.ContentType = "*/*";
            Response.AppendHeader("content-disposition", ("attachment; filename=" + LblDownloadFile.Text));
            Response.WriteFile(sFileExport);
            Response.End();
        }
        #endregion
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdResult.DataSource = (DataTable)Session["dtSearchCatasto"];
                if (page.HasValue)
                    GrdResult.PageIndex = page.Value;
                GrdResult.DataBind();
                GrdResult.Visible = true;
                RegisterScript("$('#divResult').show();", this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DropColGrd()
        {
            try
            {
                for (int x = 0; x < GrdResult.Columns.Count; x++)
                {
                    GrdResult.Columns.RemoveAt(x);
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.DropColGrd.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtMyDati"></param>
        private void LoadColumnChart(DataTable dtMyDati)
        {
            try
            {
                string sScript = "";

                // Load the Visualization API and the corechart package.
                sScript += "google.charts.load('current', {'packages':['bar']});";

                // Set a callback to run when the Google Visualization API is loaded.
                sScript += "google.charts.setOnLoadCallback(drawChart);";

                // Callback that creates and populates a data table,
                // instantiates the pie chart, passes in the data and
                // draws it.
                sScript += "function drawChart()";
                sScript += "{";
                // Create the data table.
                sScript += "var data = new google.visualization.arrayToDataTable([";
                sScript += "[\"Periodo\", \"N.Istanze\"]";
                foreach (DataRow myRow in dtMyDati.Rows)
                {
                    string tipo = myRow["FASE"].ToString();
                    int numero = int.Parse(myRow["SECONDI"].ToString());
                    sScript += ", ['" + tipo + "', " + numero + "]";
                }
                sScript += "]);";
                // Set chart options
                sScript += "var options = {";
                sScript += "title: ''";
                sScript += ",backgroundColor: 'transparent'";
                //sScript += ",width: 900";
                sScript += ",legend: { position: 'none' }";
                sScript += ",chart: { subtitle: '' }";
                sScript += ",axes:{x:{0: { side: 'bttom', label: ''} }}";// Top x-axis.
                sScript += ",bar: { groupWidth: \"90 %\" }";
                sScript += "};";

                // Instantiate and draw our chart, passing in some options.
                sScript += "var chart = new google.charts.Bar(document.getElementById('chart_div'));";
                sScript += "chart.draw(data, google.charts.Bar.convertOptions(options));";
                sScript += "}";
                RegisterScript(sScript,this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.LoadColumnChart.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetFilesUploaded()
        {
            string GetList = string.Empty;
            try
            {
                string[] ListFiles = Directory.GetFiles(Business.ConstWrapper.PathImportMotoreCatasto);
                string sListFiles = string.Empty;
                foreach (string myItem in ListFiles)
                {
                    sListFiles += "<li>" + myItem.Replace(Business.ConstWrapper.PathImportMotoreCatasto, string.Empty) + "</li>";
                }
                if (sListFiles != string.Empty)
                {
                    GetList += "$('#divFilesUploaded').html('<span>Flussi già caricati:</span><ul style=\"list - style - type:square\">" + sListFiles + "</ul>');";
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ExportCatasto.CmdUpload_Click.errore: ", ex);
                GetList = string.Empty;
            }
            return GetList;
        }
    }
}