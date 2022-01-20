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
using System.Configuration ;
using Business;
using Anagrafica.DLL;
using log4net;
using DichiarazioniICI.Database;
//using Estrattore.Oggetti;
//using RemotingInterfaceEstrattore;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la visualizzazione del riepilogo calcolato per categoria e classe.
    /// Contiene la funzione di stampa della comandiera.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GetCalcoloICICategoriaClasse :BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GetCalcoloICICategoriaClasse));
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
            try
            {
                string COD_CONTRIB = "-1";
                string Tributo;
                string ANNO = string.Empty;
                string ID_PROGRESSIVO_ELABORAZIONE = string.Empty;

                if (Request["COD_CONTRIB"] != null)
                    COD_CONTRIB = Request["COD_CONTRIB"].ToString();
                ANNO = Request["ANNO"].ToString();
                ID_PROGRESSIVO_ELABORAZIONE = Request["ID_PROG_ELAB"].ToString();
                Tributo = Request["COD_TRIBUTO"].ToString();
                ViewState.Add("ANNOICI", ANNO);
                ViewState.Add("COD_CONTRIB", COD_CONTRIB);

                if (!Page.IsPostBack)
                {
                    // Put user code to initialize the page here
                    TpSituazioneFinaleIci clTpSituazioneFinaleIci = new TpSituazioneFinaleIci();
                    //*** 20140509 - TASI ***
                    //DataTable dtGetImmobiliCatClasse = clTpSituazioneFinaleIci.GetImmobiliCategoriaClasse(COD_CONTRIB, ANNO, ConstWrapper.CodiceEnte, long.Parse(ID_PROGRESSIVO_ELABORAZIONE.ToString()));
                    DataTable dtGetImmobiliCatClasse = clTpSituazioneFinaleIci.GetImmobiliCategoriaClasse(COD_CONTRIB, ANNO, ConstWrapper.CodiceEnte, Tributo, long.Parse(ID_PROGRESSIVO_ELABORAZIONE.ToString()));
                    //*** ***
                    if (dtGetImmobiliCatClasse.Rows.Count > 0)
                    {
                        Session["TABELLA_RICERCA_CAT_CL"] = dtGetImmobiliCatClasse;
                        //Session["TblCalcoloICI"] = dtGetImmobiliCatClasse;
                        GrdCalcoloICI.DataSource = dtGetImmobiliCatClasse;
                        GrdCalcoloICI.DataBind();
                        lblMessage.Visible = false;
                    }
                    else
                    {
                        //*** 20120704 - IMU ***
                        lblMessage.Text = "Il Calcolo per l'anno " + ANNO + " non è stato ancora effettuato per il contribuente/i selezionato/i.";
                        lblMessage.Visible = true;
                        GrdCalcoloICI.Visible = false;
                        Session.Remove("TblCalcoloICI");
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICICategoriaClasse.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
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
            LoadSearch(e.NewPageIndex);
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
                GrdCalcoloICI.DataSource = (DataTable)Session["TABELLA_RICERCA_CAT_CL"];
                if (page.HasValue)
                    GrdCalcoloICI.PageIndex = page.Value;
                GrdCalcoloICI.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICICategoriaClasse.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }

        //*** 20140509 - TASI ***
        //private DataSet CreateDataSetCalcoloICI()
        //{
        //    DataSet dsTmp = new DataSet();

        //    dsTmp.Tables.Add("ELENCO_IMMOBILI_CATEGORIA_CALSSE");
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    //*** 20120828 - IMU adeguamento per importi statali *** 
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    //*** ***
        //    //*** 20130422 - aggiornamento IMU ***
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"].Columns.Add("").DataType = typeof(string);
        //    //*** ***

        //    return dsTmp;
        //}

        //protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        //{
        //    //			try
        //    //			{

        //    DataRow dr;
        //    DataSet ds;
        //    DataTable dtRiepilogoICI;
        //    string sPathProspetti =string.Empty;
        //    string NameXLS =string.Empty;

        //    ArrayList arratlistNomiColonne;
        //    string[] arraystr;

        //    arratlistNomiColonne = new ArrayList();

        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    //*** 20120828 - IMU adeguamento per importi statali *** 
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    //*** ***
        //    //*** 20130422 - aggiornamento IMU ***
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    //*** ***

        //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

        //    sPathProspetti = System.Configuration.ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"].ToString();
        //    NameXLS = "ELENCO_IMMOBILI_CATEGORIA_CALSSE" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

        //    ds = CreateDataSetCalcoloICI();

        //    dtRiepilogoICI = ds.Tables["ELENCO_IMMOBILI_CATEGORIA_CALSSE"];

        //    //inserisco intestazione - titolo prospetto + data stampa
        //    dr = dtRiepilogoICI.NewRow();
        //    //*** 20120704 - IMU ***
        //    dr[0] = "Prospetto Riepilogo Calcolo ICI/IMU Massivo Anno " + ViewState["ANNOICI"].ToString() + " - Importi e Totali Immobili per Categoria e Classe ";
        //    dr[2] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
        //    dtRiepilogoICI.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtRiepilogoICI.NewRow();
        //    dtRiepilogoICI.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtRiepilogoICI.NewRow();
        //    dtRiepilogoICI.Rows.Add(dr);

        //    //inserisco intestazione di colonna
        //    dr = dtRiepilogoICI.NewRow();
        //    dr[0] = "Categoria";
        //    dr[1] = "Classe";
        //    dr[2] = "Numero Immobili";
        //    dr[3] = "Importo Abitazione Principale";
        //    //*** 20120828 - IMU adeguamento per importi statali *** 
        //    dr[4] = "Importo Altri Fabbricati";
        //    dr[5] = "Importo Altri Fabbricati Stato";
        //    dr[6] =  "Importo Aree Fabbricabili";
        //    dr[7] = "Importo Aree Fabbricabili Stato";
        //    dr[8] = "Importo Terreni";
        //    dr[9] = "Importo Terreni Stato"; 
        //    dr[10]= "Importo Fabbricati Rurali Uso Strumentale";
        //    //*** ***
        //    //*** 20130422 - aggiornamento IMU ***
        //    dr[11]= "Importo Fabbricati Rurali Uso Strumentale Stato";
        //    dr[12]= "Importo Uso Prod.Cat.D";
        //    dr[13]= "Importo Uso Prod.Cat.D Stato";
        //    //*** ***
        //    dr[14]= "Importo Detrazione";
        //    dr[15]="Importo Detrazione Statale Applicata";
        //    dr[16]="Importo Totale";

        //    dtRiepilogoICI.Rows.Add(dr);

        //    DataTable dtImmobiliCatClasse=new DataTable();
        //    dtImmobiliCatClasse=(DataTable)Session["TABELLA_RICERCA_CAT_CL"];

        //    //TpSituazioneFinaleIci clTpSituazioneFinaleIci =new TpSituazioneFinaleIci();
        //    //DataTable dtRiepilogoCalcoloICI = clTpSituazioneFinaleIci.GetRiepilogoMassivo(ViewState["COD_CONTRIB"].ToString(), ViewState["ANNOICI"].ToString(), ConstWrapper.CodiceEnte);

        //    for (int i = 0; i < dtImmobiliCatClasse.Rows.Count; i++)
        //    {
        //        dr = dtRiepilogoICI.NewRow();
        //        dr[0] = dtImmobiliCatClasse.Rows[i]["CAT"].ToString();
        //        dr[1] = dtImmobiliCatClasse.Rows[i]["CL"].ToString();
        //        dr[2] = dtImmobiliCatClasse.Rows[i]["num_fabbricati"].ToString();
        //        dr[3] = dtImmobiliCatClasse.Rows[i]["Imp_Abi_Princ"].ToString();
        //        //*** 20120828 - IMU adeguamento per importi statali *** 
        //        dr[4] = dtImmobiliCatClasse.Rows[i]["Imp_Altri_Fab"].ToString();
        //        dr[5] = dtImmobiliCatClasse.Rows[i]["Imp_Altri_Fab_Stato"].ToString();
        //        dr[6] = dtImmobiliCatClasse.Rows[i]["Imp_Aree_Fab"].ToString();
        //        dr[7] = dtImmobiliCatClasse.Rows[i]["Imp_Aree_Fab_Stato"].ToString();
        //        dr[8] = dtImmobiliCatClasse.Rows[i]["Imp_Terreni"].ToString() ;
        //        dr[9] =dtImmobiliCatClasse.Rows[i]["Imp_Terreni_Stato"].ToString() ;
        //        dr[10]=dtImmobiliCatClasse.Rows[i]["Imp_FabRurUsoStrum"].ToString() ;
        //        //*** ***
        //        //*** 20130422 - aggiornamento IMU ***
        //        dr[11]=dtImmobiliCatClasse.Rows[i]["Imp_FabRurUsoStrum_stato"].ToString() ;
        //        dr[12]=dtImmobiliCatClasse.Rows[i]["Imp_UsoProdCatD"].ToString() ;
        //        dr[13]=dtImmobiliCatClasse.Rows[i]["Imp_UsoProdCatD_stato"].ToString() ;
        //        //*** ***
        //        dr[14]= dtImmobiliCatClasse.Rows[i]["Detrazione"].ToString() ;
        //        dr[15]= dtImmobiliCatClasse.Rows[i]["DSA"].ToString() ;
        //        dr[16]= dtImmobiliCatClasse.Rows[i]["Totale"].ToString() ;

        //        dtRiepilogoICI.Rows.Add(dr);
        //    }
        //    //inserisco riga vuota
        //    dr = dtRiepilogoICI.NewRow();
        //    dtRiepilogoICI.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtRiepilogoICI.NewRow();
        //    dtRiepilogoICI.Rows.Add(dr);

        //    //inserisco numero totali di contribuenti
        //    dr = dtRiepilogoICI.NewRow();
        //    //*** 20120704 - IMU ***
        //    dr[0] = "Totale Posizioni Calcolo Massivo ICI/IMU Anno " + ViewState["ANNOICI"].ToString() + " per Categoria e Classe:" + (dtImmobiliCatClasse.Rows.Count) ;
        //    dtRiepilogoICI.Rows.Add(dr);

        //    //log.Debug("Stampa Dichiarazione da Bonificare");

        //    //definisco l'insieme delle colonne da esportare
        //    int[] iColumns ={ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16};
        //    //esporto i dati in excel
        //    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
        //    objExport.ExportDetails(dtRiepilogoICI, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti + NameXLS);
        //    //			}
        //    //			catch (Exception er)
        //    //			{
        //    //				string str;
        //    //				str = "<script language='javascript'>alert('" + er.ToString() +"');</script>";
        //    //				RegisterScript(this.GetType(),"uscita", str);
        //log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICICategoriaClasse.btnStampaExcel_Click.errore: ", er);
          //      Response.Redirect("../../PaginaErrore.aspx");
        //    //				
        //    //			}

        //}
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
        protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
            DataRow dr;
            DataSet ds = new DataSet();
            DataTable dtRiepilogoICI = null;
            string NameXLS = string.Empty;
            ArrayList arratlistNomiColonne;
            string[] arraystr = null;
            int nCol = 18;
            int myCol = 0;

            try
            {
                arratlistNomiColonne = new ArrayList();
                checked
                {
                    for (int x = 0; x < nCol; x++)
                    {
                        arratlistNomiColonne.Add("");
                    }
                }
                arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

                NameXLS = ConstWrapper.CodiceEnte+"_ELENCO_IMMOBILI_CATEGORIA_CLASSE_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                ds.Tables.Add("ELENCO_IMMOBILI_CATEGORIA_CLASSE");
                checked
                {
                    for (int x = 0; x < nCol; x++)
                    {
                        ds.Tables["ELENCO_IMMOBILI_CATEGORIA_CLASSE"].Columns.Add("").DataType = typeof(string);
                    }
                }
                dtRiepilogoICI = ds.Tables["ELENCO_IMMOBILI_CATEGORIA_CLASSE"];

                //inserisco intestazione - titolo prospetto + data stampa
                dr = dtRiepilogoICI.NewRow();
                //*** 20120704 - IMU ***
                dr[0] = "Prospetto Riepilogo Calcolo Massivo Anno " + ViewState["ANNOICI"].ToString() + " - Importi e Totali Immobili per Categoria e Classe ";
                dr[2] = "Data Stampa:" + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
                dtRiepilogoICI.Rows.Add(dr);

                //inserisco riga vuota
                dr = dtRiepilogoICI.NewRow();
                dtRiepilogoICI.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtRiepilogoICI.NewRow();
                dtRiepilogoICI.Rows.Add(dr);

                //inserisco intestazione di colonna
                dr = dtRiepilogoICI.NewRow();
                myCol = 0; dr[myCol] = "Tributo";
                myCol++; dr[myCol] = "Categoria";
                myCol++; dr[myCol] = "Classe";
                myCol++; dr[myCol] = "Numero Immobili";
                myCol++; dr[myCol] = "Importo Abitazione Principale";
                //*** 20120828 - IMU adeguamento per importi statali *** 
                myCol++; dr[myCol] = "Importo Altri Fabbricati";
                myCol++; dr[myCol] = "Importo Altri Fabbricati Stato";
                myCol++; dr[myCol] = "Importo Aree Fabbricabili";
                myCol++; dr[myCol] = "Importo Aree Fabbricabili Stato";
                myCol++; dr[myCol] = "Importo Terreni";
                myCol++; dr[myCol] = "Importo Terreni Stato";
                myCol++; dr[myCol] = "Importo Fabbricati Rurali Uso Strumentale";
                //*** ***
                //*** 20130422 - aggiornamento IMU ***
                myCol++; dr[myCol] = "Importo Fabbricati Rurali Uso Strumentale Stato";
                myCol++; dr[myCol] = "Importo Uso Prod.Cat.D";
                myCol++; dr[myCol] = "Importo Uso Prod.Cat.D Stato";
                //*** ***
                myCol++; dr[myCol] = "Importo Detrazione";
                myCol++; dr[myCol] = "Importo Detrazione Statale Applicata";
                myCol++; dr[myCol] = "Importo Totale";

                dtRiepilogoICI.Rows.Add(dr);

                DataTable dtImmobiliCatClasse = new DataTable();
                dtImmobiliCatClasse = (DataTable)Session["TABELLA_RICERCA_CAT_CL"];
                foreach (DataRow myRow in dtImmobiliCatClasse.Rows)
                {
                    dr = dtRiepilogoICI.NewRow();
                    myCol = 0; dr[myCol] = myRow["DESCRTRIBUTO"].ToString();
                    myCol++; dr[myCol] = myRow["CAT"].ToString();
                    myCol++; dr[myCol] = myRow["CL"].ToString();
                    myCol++; dr[myCol] = myRow["num_fabbricati"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_Abi_Princ"].ToString();
                    //*** 20120828 - IMU adeguamento per importi statali *** 
                    myCol++; dr[myCol] = myRow["Imp_Altri_Fab"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_Altri_Fab_Stato"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_Aree_Fab"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_Aree_Fab_Stato"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_Terreni"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_Terreni_Stato"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_FabRurUsoStrum"].ToString();
                    //*** ***
                    //*** 20130422 - aggiornamento IMU ***
                    myCol++; dr[myCol] = myRow["Imp_FabRurUsoStrum_stato"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_UsoProdCatD"].ToString();
                    myCol++; dr[myCol] = myRow["Imp_UsoProdCatD_stato"].ToString();
                    //*** ***
                    myCol++; dr[myCol] = myRow["Detrazione"].ToString();
                    myCol++; dr[myCol] = myRow["DSA"].ToString();
                    myCol++; dr[myCol] = myRow["Totale"].ToString();

                    dtRiepilogoICI.Rows.Add(dr);
                }
                //inserisco riga vuota
                dr = dtRiepilogoICI.NewRow();
                dtRiepilogoICI.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtRiepilogoICI.NewRow();
                dtRiepilogoICI.Rows.Add(dr);

                //inserisco numero totali di contribuenti
                dr = dtRiepilogoICI.NewRow();
                //*** 20120704 - IMU ***
                dr[0] = "Totale Posizioni Calcolo Massivo Anno " + ViewState["ANNOICI"].ToString() + " per Categoria e Classe:" + (dtImmobiliCatClasse.Rows.Count);
                dtRiepilogoICI.Rows.Add(dr);
            }
            catch (Exception er)
            {
                string str;
                str = "GestAlert('a', 'danger', '', '', '" + er.ToString() + "');";
                RegisterScript(str,this.GetType());
                dtRiepilogoICI = null;
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICICategoriaClasse.btnStampaExcel_Click.errore: ",er);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            if (dtRiepilogoICI != null)
            {
                //definisco l'insieme delle colonne da esportare
                int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
                //esporto i dati in excel
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                objExport.ExportDetails(dtRiepilogoICI, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }

        }
        //*** ***

        //protected string EuroForGridView(object iInput)
        //{
        //    string ret = string.Empty;
        //try{
        //    if (iInput != DBNull.Value)
        //    {

        //        if ((iInput.ToString() == "-1")||(iInput.ToString() == "-1,00"))
        //        {
        //            ret = Convert.ToDecimal("0").ToString("N");	//string.Empty;	
        //        }
        //        else
        //        {
        //            ret = Convert.ToDecimal(iInput).ToString("N");
        //        }
        //    }
        //    else
        //    {
        //        ret = Convert.ToDecimal("0").ToString("N");	
        //    }
        //    return ret;
        //}
        //Catch(Exception Err){
       // log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICICategoriaClasse.EuroForGridView.errore: ", Err);
           //     Response.Redirect("../../PaginaErrore.aspx");
        //}
        //}
    }
}
