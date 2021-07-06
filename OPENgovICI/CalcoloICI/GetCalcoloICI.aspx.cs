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

using DichiarazioniICI.Database;
//using Estrattore.Oggetti;
using RemotingInterfaceEstrattore;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;
using log4net;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la visualizzazione del risultato calcolo elaborato.
    /// Contiene la funzione di stampa della comandiera.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GetCalcoloICI : BaseEnte
    {
		private static readonly ILog log = LogManager.GetLogger(typeof(GetCalcoloICI));

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
                string COD_CONTRIB="-1";
                string ANNO = string.Empty;
                string ID_PROGRESSIVO_ELABORAZIONE = string.Empty;
                bool nettoVersato = true;

                if (Request["COD_CONTRIB"] != null)
                    COD_CONTRIB = Request["COD_CONTRIB"].ToString();
                ANNO = Request["ANNO"].ToString();
                ID_PROGRESSIVO_ELABORAZIONE = Request["ID_PROG_ELAB"].ToString();

                ViewState.Add("ANNOICI", ANNO);
                ViewState.Add("COD_CONTRIB", COD_CONTRIB);
                //*** 20140509 - TASI ***
                ViewState.Add("TRIBUTO", Request["COD_TRIBUTO"].ToString());
                //*** ***

                if (!Page.IsPostBack)
                {
                    // Put user code to initialize the page here
                    TpSituazioneFinaleIci clTpSituazioneFinaleIci = new TpSituazioneFinaleIci();

                    //*** 20121210 - occorre vedere: Riepilogo Importo Calcolo ICI/IMU= totale dovuto annuo (no raffronto con il pagato)
                    //				if((ANNO != string.Empty ) && (int.Parse(ANNO) < 2012))
                    //*** 20130422 - aggiornamento IMU ***
                    if (Request["bNettoVersato"].ToString() == string.Empty)
                    {
                        if ((ANNO != string.Empty) && (int.Parse(ANNO) < 2012))
                            nettoVersato = false;
                    }
                    else
                    {
                        nettoVersato = bool.Parse(Request["bNettoVersato"].ToString());
                    }
                    //*** ***
                    //*** 20140509 - TASI ***
                    //DataTable dtGetCalcoloICI = clTpSituazioneFinaleIci.GetCalcoloICItotale(COD_CONTRIB, ANNO, ConstWrapper.CodiceEnte,long.Parse(ID_PROGRESSIVO_ELABORAZIONE.ToString()), nettoVersato);
                    DataTable dtGetCalcoloICI = clTpSituazioneFinaleIci.GetCalcoloICItotale(COD_CONTRIB, ANNO, ConstWrapper.CodiceEnte, Request["COD_TRIBUTO"].ToString(), long.Parse(ID_PROGRESSIVO_ELABORAZIONE.ToString()), nettoVersato);
                    string VisualParamElabDoc = "none";
                    if (dtGetCalcoloICI.Rows.Count > 0)
                    {
                        Session["TABELLA_RICERCA"] = dtGetCalcoloICI;
                        Session["TblCalcoloICI"] = dtGetCalcoloICI;
                        GrdCalcoloICI.DataSource = dtGetCalcoloICI;
                        GrdCalcoloICI.DataBind();
                        lblMessage.Visible = false;
                        VisualParamElabDoc = "";
                        new Utility.DBUtility(ConstWrapper.DBType, ConstWrapper.StringConnectionOPENgov).LogActionEvent(DateTime.Now, ConstWrapper.sUsername, new Utility.Costanti.LogEventArgument().Elaborazioni, "CalcoloPuntuale", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstWrapper.CodiceTributo, ConstWrapper.CodiceEnte, int.Parse(dtGetCalcoloICI.Rows[0]["ID_SITUAZIONE_FINALE"].ToString()));
                    }
                    else
                    {
                        //*** 20120704 - IMU ***
                        lblMessage.Text = "Il Calcolo per l'anno " + ANNO + " non è stato ancora effettuato per il contribuente/i selezionato/i.";
                        lblMessage.Visible = true;
                        GrdCalcoloICI.Visible = false;
                        Session.Remove("TblCalcoloICI");
                        VisualParamElabDoc = "none";
                    }
                    string sScript = "parent.document.getElementById('ParamElabDoc').style.display='" + VisualParamElabDoc + "';";
                    sScript += "parent.document.getElementById('DivLoading').style.display='none';";
                    RegisterScript(sScript,this.GetType());
                    //*** ***
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICI.Page_Load.errore: ", Err);
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
                GrdCalcoloICI.DataSource = (DataTable)Session["TABELLA_RICERCA"];
                if (page.HasValue)
                    GrdCalcoloICI.PageIndex = page.Value;
                GrdCalcoloICI.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICI.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");

                throw ex;
            }
        }
        //*** 20150430 - TASI Inquilino ***
        //protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        //{
        //    //			try
        //    //			{

        //    DataRow dr;
        //    DataSet ds;
        //    DataTable dtRiepilogoICI;
        //    string sPathProspetti =string.Empty;
        //    string NameXLS =string.Empty;
        //    int x;
        //    ArrayList arratlistNomiColonne;
        //    string[] arraystr;

        //    arratlistNomiColonne = new ArrayList();
        //    //*** 20140509 - TASI ***
        //    //*** 20130422 - aggiornamento IMU ***
        //    //*** 20120828 - IMU adeguamento per importi statali *** 
        //    for (x = 0; x <= 18; x++) //for (x = 0; x <= 17; x++)
        //    {
        //        arratlistNomiColonne.Add("");
        //    }
        //    //*** ***
        //    //*** ***
        //    //*** ***
        //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

        //    sPathProspetti = ConstWrapper.PathProspetti;
        //    NameXLS = "RESULT_CALCOLO_ICI" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

        //    ds = CreateDataSetCalcoloICI();

        //    dtRiepilogoICI = ds.Tables["RESULT_CALCOLO_ICI"];

        //    //inserisco intestazione - titolo prospetto + data stampa
        //    dr = dtRiepilogoICI.NewRow();
        //    //*** 20120704 - IMU ***
        //    dr[0] = "Prospetto Riepilogo Calcolo Massivo Anno " + ViewState["ANNOICI"].ToString();
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
        //    // 05092007 Cognome Nome separati
        //    //dr[0] = "Nominativo";
        //    x = 0;
        //    dr[x] = "Cognome";
        //    x++; dr[x] = "Nome";
        //    x++; dr[x] = "Codice Fiscale";
        //    x++; dr[x] = "Partita Iva";
        //    //*** 20140509 - TASI ***
        //    x++; dr[x] = "Tributo";
        //    //*** ***
        //    x++; dr[x] = "Importo Abitazione Principale";
        //    //*** 20120828 - IMU adeguamento per importi statali *** 
        //    x++; dr[x] = "Importo Altri Fabbricati";
        //    x++; dr[x] = "Importo Altri Fabbricati Stato";
        //    x++; dr[x] = "Importo Aree Fabbricabili";
        //    x++; dr[x] = "Importo Aree Fabbricabili Stato";
        //    x++; dr[x] = "Importo Terreni";
        //    x++; dr[x] = "Importo Terreni Stato";
        //    x++; dr[x] = "Importo Fabbricati Rurali Uso Strumentale";
        //    //*** ***
        //    //*** 20130422 - aggiornamento IMU ***
        //    x++; dr[x] = "Importo Fabbricati Rurali Uso Strumentale Stato";
        //    x++; dr[x] = "Importo Uso Prod.Cat.D";
        //    x++; dr[x] = "Importo Uso Prod.Cat.D Stato";
        //    //*** ***
        //    x++; dr[x] = "Importo Detrazione";
        //    x++; dr[x] = "Importo Detrazione Statale Applicata";
        //    x++; dr[x] = "Importo Totale";			
        //    dtRiepilogoICI.Rows.Add(dr);

        //    TpSituazioneFinaleIci clTpSituazioneFinaleIci =new TpSituazioneFinaleIci();
        //    //*** 20140509 - TASI ***
        //    //DataTable dtRiepilogoCalcoloICI = clTpSituazioneFinaleIci.GetRiepilogoMassivo(ViewState["COD_CONTRIB"].ToString(), ViewState["ANNOICI"].ToString(), ConstWrapper.CodiceEnte);
        //    DataTable dtRiepilogoCalcoloICI = clTpSituazioneFinaleIci.GetRiepilogoMassivo(ViewState["COD_CONTRIB"].ToString(), ViewState["ANNOICI"].ToString(),ViewState["TRIBUTO"].ToString(), ConstWrapper.CodiceEnte);
        //    //*** ***
        //    for (int i = 0; i < dtRiepilogoCalcoloICI.Rows.Count; i++)
        //    {
        //        dr = dtRiepilogoICI.NewRow();

        //        string cognome="";

        //        /** 05092007 se la data di morte è valorizzata ed è diversa da minvalue cognome=EREDE DI cognome **/
        //        if((dtRiepilogoCalcoloICI.Rows[i]["DATA_MORTE"] != DBNull.Value)&&(dtRiepilogoCalcoloICI.Rows[i]["DATA_MORTE"].ToString() != null) &&(dtRiepilogoCalcoloICI.Rows[i]["DATA_MORTE"].ToString().CompareTo("")!=0))
        //        {
        //            string dataMorte, giornoM, meseM, annoM;
        //            dataMorte=dtRiepilogoCalcoloICI.Rows[i]["DATA_MORTE"].ToString();
        //            giornoM = dataMorte.Substring(6,2);
        //            meseM = dataMorte.Substring(4,2);
        //            annoM = dataMorte.Substring(0,4);

        //            DateTime dataM;
        //            dataM = new DateTime(Convert.ToInt16(annoM),Convert.ToInt16(meseM),Convert.ToInt16(giornoM));

        //            if (dataM != DateTime.MinValue)
        //                cognome = "EREDE DI " + dtRiepilogoCalcoloICI.Rows[i]["COGNOME_DENOMINAZIONE"].ToString();
        //            else
        //                cognome = dtRiepilogoCalcoloICI.Rows[i]["COGNOME_DENOMINAZIONE"].ToString();
        //        }
        //        else
        //        {
        //            cognome = dtRiepilogoCalcoloICI.Rows[i]["COGNOME_DENOMINAZIONE"].ToString();
        //        }

        //        //dr[0] = dtRiepilogoCalcoloICI.Rows[i]["COGNOME_DENOMINAZIONE"].ToString() + dtRiepilogoCalcoloICI.Rows[i]["NOME"].ToString();
        //        x=0; dr[x] = cognome;
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["NOME"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["COD_FISCALE"].ToString();
        //        x++; dr[x] = "'" + dtRiepilogoCalcoloICI.Rows[i]["PARTITA_IVA"].ToString();
        //        //*** 20140509 - TASI ***
        //        x++; dr[x] = "'" + dtRiepilogoCalcoloICI.Rows[i]["DESCRTRIBUTO"].ToString();
        //        //*** ***
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Abi_Princ"].ToString();
        //        //*** 20120828 - IMU adeguamento per importi statali *** 
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Altri_Fab"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Altri_Fab_Stato"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Aree_Fab"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Aree_Fab_Stato"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Terreni"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_Terreni_Stato"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_FabRurUsoStrum"].ToString();
        //        //*** ***
        //        //*** 20130422 - aggiornamento IMU ***
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_FabRurUsoStrum_stato"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_UsoProdCatD"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Imp_UsoProdCatD_stato"].ToString();
        //        //*** ***
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Detrazione"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["DSA"].ToString();
        //        x++; dr[x] = dtRiepilogoCalcoloICI.Rows[i]["Totale"].ToString();

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
        //    dr[0] = "Totale Contribuenti Calcolo Massivo Anno " + ViewState["ANNOICI"].ToString() + ":" + (dtRiepilogoCalcoloICI.Rows.Count);
        //    dtRiepilogoICI.Rows.Add(dr);

        //    //log.Debug("Stampa Dichiarazione da Bonificare");

        //    //definisco l'insieme delle colonne da esportare
        //    int[] iColumns ={ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14,15,16,17,18};
        //    //esporto i dati in excel
        //    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
        //    objExport.ExportDetails(dtRiepilogoICI, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti + NameXLS);
        //    //			}
        //    //			catch (Exception er)
        //    //			{
        //    //				string str;
        //    //				str = "<script language='javascript'>alert('" + er.ToString() +"');</script>";
        //    //				RegisterScript(this.GetType(),"uscita", str);
        // log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICI.btnStampaExcel_Click.errore: ", Ex);
        //     Response.Redirect("../../PaginaErrore.aspx");
        //    //				
        //    //			}

        //}
        //private DataSet CreateDataSetCalcoloICI()
        //{
        //    DataSet dsTmp = new DataSet();
        //    int x;
        //    dsTmp.Tables.Add("RESULT_CALCOLO_ICI");
        //    //*** 20140509 - TASI ***
        //    for (x = 0; x <= 18; x++)
        //    {
        //        dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    }
        //    //*** ***
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    ////*** 20120828 - IMU adeguamento per importi statali *** 
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    ////*** ***
        //    ////*** 20130422 - aggiornamento IMU ***
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
        //    ////*** ***
        //    return dsTmp;
        //}
  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void btnStampaExcel_Click(object sender, System.EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dtRiepilogoICI = new DataTable();
            string NameXLS = string.Empty;
            string sDatiStampa = string.Empty;
            int nCampi;
            double impTotale = 0;
            ArrayList arratlistNomiColonne;
            string[] arraystr = null;
            string sAnno = string.Empty;

            arratlistNomiColonne = new ArrayList();
            nCampi = 50;
            //*** 20150430 - TASI Inquilino ***//*** 20140509 - TASI ***

            //*** 20130422 - aggiornamento IMU ***//*** 20120828 - IMU adeguamento per importi statali ***//*** 20120704 - IMU ***
            try
            {
                sAnno = ViewState["ANNOICI"].ToString();
                checked
                {
                    for (int x = 0; x <= nCampi; x++) //for (x = 0; x <= 17; x++)
                    {
                        arratlistNomiColonne.Add("");
                    }
                }
                arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

                //sPathProspetti = ConstWrapper.PathProspetti;
                NameXLS =ConstWrapper.CodiceEnte+ "_MINUTA_"+ DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                ds.Tables.Add("RESULT_CALCOLO_ICI");
                //*** 20140509 - TASI ***
                checked
                {
                    for (int x = 0; x <= nCampi; x++)
                    {
                        ds.Tables["RESULT_CALCOLO_ICI"].Columns.Add("").DataType = typeof(string);
                    }
                }
                dtRiepilogoICI = ds.Tables["RESULT_CALCOLO_ICI"];

                sDatiStampa = "Prospetto Riepilogo Calcolo Massivo Anno " + sAnno;
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

                //inserisco intestazione di colonna
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva";
                sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza";
                sDatiStampa += "|Nominativo Invio|Indirizzo Invio|Civico Invio|CAP Invio|Comune Invio|Provincia Invio";
                sDatiStampa += "|Tributo";
                sDatiStampa += "|Foglio|Numero|Sub|Cat.|Classe|Zona|Consistenza|Valore|Ubicazione|Tipo Rendita|% Pos.|Mesi Pos.|Abit. Princ.|N.Utiliz.|Ridotto|Esente|Colt.Dir.|Figli|Aliquota|Aliquota Stat.";
                sDatiStampa += "|Dovuto UI €|Dovuto Stat. UI €";
                sDatiStampa += "|Abitazione Principale (3912-3958) €";
                sDatiStampa += "|Altri Fabbricati (3918-3961) €";
                if (sAnno == "2012")
                    sDatiStampa += "|Altri Fabbricati Stato (3919) €";
                sDatiStampa += "|Aree Fabbricabili (3916-3960) €";
                if (sAnno == "2012")
                    sDatiStampa += "|Aree Fabbricabili Stato (3917) €";
                sDatiStampa += "|Terreni (3914) €";
                if (sAnno == "2012")
                    sDatiStampa += "|Terreni Stato (3915) €";
                sDatiStampa += "|Fabbricati Rurali Uso Strumentale (3913-3959) €";
                if (sAnno == "2012")
                    sDatiStampa += "|Fabbricati Rurali Uso Strumentale Stato (3919) €";
                sDatiStampa += "|Uso Prod.Cat.D (3930) €";
                sDatiStampa += "|Uso Prod.Cat.D Stato (3925) €";
                sDatiStampa += "|Detrazione €";
                if (sAnno == "2012")
                    sDatiStampa += "|Detrazione Statale Applicata €";
                sDatiStampa += "|Totale €";
                Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

                TpSituazioneFinaleIci clTpSituazioneFinaleIci = new TpSituazioneFinaleIci();
                DataTable dtRiepilogoCalcoloICI = clTpSituazioneFinaleIci.GetStampaMinuta(ViewState["COD_CONTRIB"].ToString(), sAnno, ViewState["TRIBUTO"].ToString(), ConstWrapper.CodiceEnte);
                foreach (DataRow myRow in dtRiepilogoCalcoloICI.Rows)// (int i = 0; i < dtRiepilogoCalcoloICI.Rows.Count; i++)
                {
                    string cognome = "";
                    //** 05092007 se la data di morte è valorizzata ed è diversa da minvalue cognome=EREDE DI cognome **
                    if ((myRow["DATA_MORTE"] != DBNull.Value) && (myRow["DATA_MORTE"].ToString() != null) && (myRow["DATA_MORTE"].ToString().CompareTo("") != 0))
                    {
                        string dataMorte, giornoM, meseM, annoM;
                        dataMorte = myRow["DATA_MORTE"].ToString();
                        giornoM = dataMorte.Substring(6, 2);
                        meseM = dataMorte.Substring(4, 2);
                        annoM = dataMorte.Substring(0, 4);

                        DateTime dataM;
                        dataM = new DateTime(Convert.ToInt16(annoM), Convert.ToInt16(meseM), Convert.ToInt16(giornoM));

                        if (dataM != DateTime.MinValue)
                            cognome = "EREDE DI " + myRow["COGNOME_DENOMINAZIONE"].ToString();
                        else
                            cognome = myRow["COGNOME_DENOMINAZIONE"].ToString();
                    }
                    else
                    {
                        cognome = myRow["COGNOME_DENOMINAZIONE"].ToString();
                    }

                    sDatiStampa = cognome;
                    if (myRow["NOME"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["NOME"].ToString();
                    else
                        sDatiStampa += "|";
                    string sCFPIVA = string.Empty;
                    if (myRow["PARTITA_IVA"] != DBNull.Value)
                    {
                        if (myRow["PARTITA_IVA"].ToString() != string.Empty)
                        {
                            sCFPIVA = myRow["PARTITA_IVA"].ToString();
                        }
                        else {
                            if (myRow["COD_FISCALE"] != DBNull.Value)
                            {
                                sCFPIVA = myRow["COD_FISCALE"].ToString();
                            }
                        }
                    }
                    else {
                        if (myRow["COD_FISCALE"] != DBNull.Value)
                        {
                            sCFPIVA = myRow["COD_FISCALE"].ToString();
                        }
                    }
                    sDatiStampa += "|'" + sCFPIVA;
                    if (myRow["VIA_RES"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["VIA_RES"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Civico_Res"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Civico_Res"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["CAP_Res"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["CAP_Res"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Comune_Res"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Comune_Res"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["PROVINCIA_RES"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["PROVINCIA_RES"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["NominativoCO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["NominativoCO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["INDIRIZZOCO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["IndirizzoCO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["CivicoCO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["CivicoCO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["CAPCO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["CAPCO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["COMUNECO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["ComuneCO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["PVCO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["PVCO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["DESCRTRIBUTO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["DESCRTRIBUTO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Foglio"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Foglio"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Numero"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Numero"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["SUBALTERNO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["SUBALTERNO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["CATEGORIA"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["CATEGORIA"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Classe"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Classe"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Zona"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Zona"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["consistenza"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["consistenza"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Valore"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Valore"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["INDIRIZZONEW"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["INDIRIZZONEW"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["DESCR_RENDITA"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["DESCR_RENDITA"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["PERC_POSSESSO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["PERC_POSSESSO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["MESI_POSSESSO"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["MESI_POSSESSO"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["FLAG_PRINCIPALE"] != DBNull.Value)
                        sDatiStampa += "|" + Business.CoreUtility.FormattaGrdAbiPrinc(myRow["FLAG_PRINCIPALE"].ToString());
                    else
                        sDatiStampa += "|";
                    if (myRow["NUMERO_UTILIZZATORI"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["NUMERO_UTILIZZATORI"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["FLAG_RIDUZIONE"] != DBNull.Value)
                        sDatiStampa += "|" + Business.CoreUtility.FormattaGrdBoolToString(1, myRow["FLAG_RIDUZIONE"].ToString());
                    else
                        sDatiStampa += "|";
                    if (myRow["FLAG_ESENTE"] != DBNull.Value)
                        sDatiStampa += "|" + Business.CoreUtility.FormattaGrdBoolToString(1, myRow["FLAG_ESENTE"].ToString());
                    else
                        sDatiStampa += "|";
                    if (myRow["COLTIVATOREDIRETTO"] != DBNull.Value)
                        sDatiStampa += "|" + Business.CoreUtility.FormattaGrdBoolToString(0, myRow["COLTIVATOREDIRETTO"].ToString());
                    else
                        sDatiStampa += "|";
                    if (myRow["PERCENTCARICOFIGLI"] != DBNull.Value)
                        sDatiStampa += "|" + Business.CoreUtility.FormattaGrdCaricoFigli(myRow["NUMEROFIGLI"].ToString(), myRow["PERCENTCARICOFIGLI"].ToString());
                    else
                        sDatiStampa += "|";
                    if (myRow["ICI_VALORE_ALIQUOTA"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["ICI_VALORE_ALIQUOTA"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["ICI_VALORE_ALIQUOTA_STATALE"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["ICI_VALORE_ALIQUOTA_STATALE"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["ICI_TOTALE_DOVUTA"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["ICI_TOTALE_DOVUTA"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["ICI_TOTALE_DOVUTA_STATALE"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["ICI_TOTALE_DOVUTA_STATALE"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Imp_Abi_Princ"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_Abi_Princ"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Imp_Altri_Fab"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_Altri_Fab"].ToString();
                    else
                        sDatiStampa += "|";
                    if (sAnno == "2012")
                    {
                        if (myRow["Imp_Altri_Fab_Stato"] != DBNull.Value)
                            sDatiStampa += "|" + myRow["Imp_Altri_Fab_Stato"].ToString();
                        else
                            sDatiStampa += "|";
                    }
                    if (myRow["Imp_Aree_Fab"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_Aree_Fab"].ToString();
                    else
                        sDatiStampa += "|";
                    if (sAnno == "2012")
                    {
                        if (myRow["Imp_Aree_Fab_Stato"] != DBNull.Value)
                            sDatiStampa += "|" + myRow["Imp_Aree_Fab_Stato"].ToString();
                        else
                            sDatiStampa += "|";
                    }
                    if (myRow["Imp_Terreni"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_Terreni"].ToString();
                    else
                        sDatiStampa += "|";
                    if (sAnno == "2012")
                    {
                        if (myRow["Imp_Terreni_Stato"] != DBNull.Value)
                            sDatiStampa += "|" + myRow["Imp_Terreni_Stato"].ToString();
                        else
                            sDatiStampa += "|";
                    }
                    if (myRow["Imp_FabRurUsoStrum"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_FabRurUsoStrum"].ToString();
                    else
                        sDatiStampa += "|";
                    if (sAnno == "2012")
                    {
                        if (myRow["Imp_FabRurUsoStrum_stato"] != DBNull.Value)
                            sDatiStampa += "|" + myRow["Imp_FabRurUsoStrum_stato"].ToString();
                        else
                            sDatiStampa += "|";
                    }
                    if (myRow["Imp_UsoProdCatD"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_UsoProdCatD"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Imp_UsoProdCatD_stato"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Imp_UsoProdCatD_stato"].ToString();
                    else
                        sDatiStampa += "|";
                    if (myRow["Detrazione"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Detrazione"].ToString();
                    else
                        sDatiStampa += "|";
                    if (sAnno == "2012")
                    {
                        if (myRow["DSA"] != DBNull.Value)
                            sDatiStampa += "|" + myRow["DSA"].ToString();
                        else
                            sDatiStampa += "|";
                    }
                    if (myRow["Totale"] != DBNull.Value)
                        sDatiStampa += "|" + myRow["Totale"].ToString();
                    else
                        sDatiStampa += "|";
                    Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);
                    impTotale += double.Parse(myRow["ICI_TOTALE_DOVUTA"].ToString()) + double.Parse(myRow["ICI_TOTALE_DOVUTA_STATALE"].ToString());
                }
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(nCampi, char.Parse("|"));
                Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);

                //inserisco numero totali di contribuenti
                sDatiStampa = "Totale Contribuenti Anno " + sAnno + ": " + (dtRiepilogoCalcoloICI.Rows.Count);
                sDatiStampa = sDatiStampa.PadRight(nCampi, char.Parse("|"));
                Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);
                sDatiStampa = "Totale €: " + impTotale.ToString();
                sDatiStampa = sDatiStampa.PadRight(nCampi, char.Parse("|"));
                Business.CoreUtility.AddRowStampa(ref dtRiepilogoICI, sDatiStampa);
            }
            catch (Exception Ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICI.btnStampExcel_Click.errore: ", Ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            //definisco l'insieme delle colonne da esportare
            int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };
            //esporto i dati in excel
            RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
            objExport.ExportDetails(dtRiepilogoICI, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        }

        //        protected string EuroForGridView(object iInput)
        //        {
        ////			string ret = string.Empty;
        ////			try{
        ////			if ((iInput.ToString() == "-1")||(iInput.ToString() == "-1,00"))
        ////			{
        ////				ret = string.Empty;	
        ////			}
        ////			else
        ////			{
        ////				ret = Convert.ToDecimal(iInput).ToString("N");
        ////			}
        ////			return ret;
        //            string ret = string.Empty;

        //            if (iInput != DBNull.Value)
        //            {

        //                if ((iInput.ToString() == "-1")||(iInput.ToString() == "-1,00"))
        //                {
        //                    ret = Convert.ToDecimal("0").ToString("N");	//string.Empty;	
        //                }
        //                else
        //                {
        //                    ret = Convert.ToDecimal(iInput).ToString("N");
        //                }
        //            }
        //            else
        //            {
        //                ret = Convert.ToDecimal("0").ToString("N");	
        //            }
        //            return ret;
   // }
        //    catch (Exception Ex)
          //  {
            //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetCalcoloICI.EuroForGridView.errore: ", Ex);
             //   Response.Redirect("../../PaginaErrore.aspx");
           // }
//        }
}
}
