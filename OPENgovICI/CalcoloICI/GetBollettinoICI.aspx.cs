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
using Business;
using log4net;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la visualizzazione dei dati del bollettino calcolato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GetBollettinoICI :BasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(GetBollettinoICI));

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
			// Put user code to initialize the page here
			try 
			{
                string COD_CONTRIB = "-1";
                string Tributo;
                string ANNO = string.Empty;
				bool nettoVersato = true;

                if (Request["COD_CONTRIB"].ToString() != string.Empty)
                    COD_CONTRIB = Request["COD_CONTRIB"].ToString();
                Tributo = Request["COD_TRIBUTO"].ToString();
                ANNO = Request["ANNO"].ToString();
				/*ID_PROGRESSIVO_ELABORAZIONE=Request["ID_PROG_ELAB"].ToString();*/
				//*** 20121210 - occorre vedere: Riepilogo Bollettino ICI/IMU= in base alla data in cui mi trovo, dovrà riportare il dettaglio dei pagamenti che l'utente dovrà fare (se non ci sono pagamenti riporterà acconto/saldo, se ci sono pagamenti riporterà il saldo al netto del versato)
				if (Request["bNettoVersato"].ToString()==string.Empty)
				{
					if ((ANNO != string.Empty ) && (int.Parse(ANNO) < 2012))
						nettoVersato = false;
				}
				else
				{
					nettoVersato=bool.Parse(Request["bNettoVersato"].ToString());
				}
				DataTable TabellaTotali = new Database.TpSituazioneFinaleIci().GetBollettinoICI(COD_CONTRIB, ANNO, ConstWrapper.CodiceEnte, Tributo, nettoVersato);

				if (TabellaTotali.Rows.Count > 0)
				{
					// creo il datatable per visualizzare i dati incolonnati dei totali
					/*
					ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO
					ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO 
					ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE
					ICI_DOVUTA_ALTRI_FABBRICATI_ACCONTO
					ICI_DOVUTA_ALTRI_FABBRICATI_SALDO 
					ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE 
					ICI_DOVUTA_AREE_FABBRICABILI_ACCONTO 
					ICI_DOVUTA_AREE_FABBRICABILI_SALDO 
					ICI_DOVUTA_AREE_FABBRICABILI_TOTALE 
					ICI_DOVUTA_TERRENI_ACCONTO 
					ICI_DOVUTA_TERRENI_SALDO 
					ICI_DOVUTA_TERRENI_TOTALE 
					ICI_DOVUTA_DETRAZIONE_ACCONTO 
					ICI_DOVUTA_DETRAZIONE_SALDO 
					ICI_DOVUTA_DETRAZIONE_TOTALE
					ICI_DOVUTA_ACCONTO_SENZA_ARROTONDAMENTO
					ICI_DOVUTA_SALDO_SENZA_ARROTONDAMENTO 
					ICI_DOVUTA_TOTALE_SENZA_ARROTONDAMENTO 
					
					ICI_DOVUTA_DETRAZIONE_STATALE_ACCONTO
					ICI_DOVUTA_DETRAZIONE_STATALE_SALDO
					ICI_DOVUTA_DETRAZIONE_STATALE_TOTALE
					
					ARROTONDAMENTO_ICI_DOVUTA_ACCONTO 
					ARROTONDAMENTO_ICI_DOVUTA_SALDO 
					ARROTONDAMENTO_ICI_DOVUTA_TOTALE 
					ICI_DOVUTA_ACCONTO   
					ICI_DOVUTA_SALDO     
					ICI_DOVUTA_TOTALE  
					*/

                    DataTable TabellaRiepilogo = new DataTable("RiepilogoBollettino");
                    //*** 20140509 - TASI ***
                    TabellaRiepilogo.Columns.Add("DESCRTRIBUTO");
                    //*** ***
                    TabellaRiepilogo.Columns.Add("DESCRIZIONE");
                    TabellaRiepilogo.Columns.Add("IMP_ABI_PRINC");
                    /**** 20120828 - IMU adeguamento per importi statali ****/
                    TabellaRiepilogo.Columns.Add("IMP_TERRENI");
                    TabellaRiepilogo.Columns.Add("IMP_TERRENISTATO");
                    TabellaRiepilogo.Columns.Add("IMP_ALRI_FAB");
                    TabellaRiepilogo.Columns.Add("IMP_ALTRI_FABSTATO");
                    TabellaRiepilogo.Columns.Add("IMP_AREE_FAB");
                    TabellaRiepilogo.Columns.Add("IMP_AREE_FABSTATO");
                    TabellaRiepilogo.Columns.Add("IMP_FABRURUSOSTRUM");
                    /**** ****/
                    /**** 20130422 - aggiornamento IMU ****/
                    TabellaRiepilogo.Columns.Add("IMP_FABRURUSOSTRUMSTATO");
                    TabellaRiepilogo.Columns.Add("IMP_USOPRODCATD");
                    TabellaRiepilogo.Columns.Add("IMP_USOPRODCATDSTATO");
                    /**** ****/
                    TabellaRiepilogo.Columns.Add("IMP_DET");
                    TabellaRiepilogo.Columns.Add("IMP_DET_DSA");
                    TabellaRiepilogo.Columns.Add("IMP_S_ARR");
                    TabellaRiepilogo.Columns.Add("IMP_ARROT");
                    TabellaRiepilogo.Columns.Add("IMP_TOTALE");

                    object[] ArrCampi = new object[18];
                    int nCol = 0;
                    ArrCampi.Initialize();

                    foreach (DataRow myRow in TabellaTotali.Rows)
                    {
                        string DovutoForzato = myRow["DOVUTO_FORZATO"].ToString();
                        if (DovutoForzato.CompareTo("True") == 0)
                        {
                            lblMessage.Visible = true;
                        }
                        else
                        {
                            lblMessage.Visible = false;
                        }
                        if (!nettoVersato)
                        {
                            //*** 20140509 - TASI ***
                            nCol = 0; ArrCampi[nCol] = myRow["DESCRTRIBUTO"];
                            //*** ***
                            nCol++; ArrCampi[nCol] = "ACCONTO";
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO"];
                            /**** 20120828 - IMU adeguamento per importi statali ****/
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_TERRENI_ACCONTO"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_TERRENI_ACC_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ALTRI_FABBRICATI_ACCONTO"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_ALTRI_FAB_ACC_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_AREE_FABBRICABILI_ACCONTO"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_AREE_FAB_ACC_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_FABRURUSOSTRUM_ACC"];
                            /**** ****/
                            /**** 20130422 - aggiornamento IMU ****/
                            nCol++; ArrCampi[nCol] = myRow["IMP_FABRURUSOSTRUM_ACC_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_USOPRODCATD_ACC"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_USOPRODCATD_ACC_STATALE"];
                            /**** ****/
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_DETRAZIONE_ACCONTO"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_DETRAZIONE_STATALE_ACCONTO"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ACCONTO_SENZA_ARROTONDAMENTO"];
                            nCol++; ArrCampi[nCol] = myRow["ARROTONDAMENTO_ICI_DOVUTA_ACCONTO"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ACCONTO"];

                            TabellaRiepilogo.Rows.Add(ArrCampi);
                        }

                        //*** 20140509 - TASI ***
                        nCol = 0; ArrCampi[nCol] = myRow["DESCRTRIBUTO"];
                        //*** ***
                        nCol++; ArrCampi[nCol] = "SALDO";
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO"];
                        /**** 20120828 - IMU adeguamento per importi statali ****/
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_TERRENI_SALDO"];
                        nCol++; ArrCampi[nCol] = myRow["IMP_TERRENI_SAL_STATALE"];
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ALTRI_FABBRICATI_SALDO"];
                        nCol++; ArrCampi[nCol] = myRow["IMP_ALTRI_FAB_SAL_STATALE"];
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_AREE_FABBRICABILI_SALDO"];
                        nCol++; ArrCampi[nCol] = myRow["IMP_AREE_FAB_SAL_STATALE"];
                        nCol++; ArrCampi[nCol] = myRow["IMP_FABRURUSOSTRUM_SAL"];
                        /**** ****/
                        /**** 20130422 - aggiornamento IMU ****/
                        nCol++; ArrCampi[nCol] = myRow["IMP_FABRURUSOSTRUM_SAL_STATALE"];
                        nCol++; ArrCampi[nCol] = myRow["IMP_USOPRODCATD_SAL"];
                        nCol++; ArrCampi[nCol] = myRow["IMP_USOPRODCATD_SAL_STATALE"];
                        /**** ****/
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_DETRAZIONE_SALDO"];
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_DETRAZIONE_STATALE_SALDO"];
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_SALDO_SENZA_ARROTONDAMENTO"];
                        nCol++; ArrCampi[nCol] = myRow["ARROTONDAMENTO_ICI_DOVUTA_SALDO"];
                        nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_SALDO"];

                        TabellaRiepilogo.Rows.Add(ArrCampi);

                        if (!nettoVersato)
                        {
                            //*** 20140509 - TASI ***
                            nCol = 0; ArrCampi[nCol] = myRow["DESCRTRIBUTO"];
                            //*** ***
                            nCol++; ArrCampi[nCol] = "UNICA SOLUZIONE";
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE"];
                            /**** 20120828 - IMU adeguamento per importi statali ****/
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_TERRENI_TOTALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_TERRENI_TOT_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_ALTRI_FAB_TOT_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_AREE_FABBRICABILI_TOTALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_AREE_FAB_TOT_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_FABRURUSOSTRUM_TOT"];
                            /**** ****/
                            /**** 20130422 - aggiornamento IMU ****/
                            nCol++; ArrCampi[nCol] = myRow["IMP_FABRURUSOSTRUM_TOT_STATALE"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_USOPRODCATD_TOT"];
                            nCol++; ArrCampi[nCol] = myRow["IMP_USOPRODCATD_TOT_STATALE"];
                            /**** ****/
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_DETRAZIONE_TOTALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_DETRAZIONE_STATALE_TOTALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_TOTALE_SENZA_ARROTONDAMENTO"];
                            nCol++; ArrCampi[nCol] = myRow["ARROTONDAMENTO_ICI_DOVUTA_TOTALE"];
                            nCol++; ArrCampi[nCol] = myRow["ICI_DOVUTA_TOTALE"];

                            TabellaRiepilogo.Rows.Add(ArrCampi);
                        }
                    }					
				    Session["TabBollettino"] = TabellaRiepilogo;
				    //Session["TabellaTotaliBollettino"] = TabellaTotali;
				    Session.Add("TabellaTotaliBollettino", TabellaTotali);
					
				    GrdBollettinoICI.DataSource = TabellaRiepilogo;
				    GrdBollettinoICI.DataBind();
				}
				else{
					Session.Remove("TabBollettino");
					Session.Remove("TabellaTotaliBollettino");
				}
			}
			catch (Exception Ex)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetBollettinoICI.Page_Load.errore: ",Ex);
                Response.Redirect("../../PaginaErrore.aspx");
			}
        }

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
   // }
		//	catch (Exception Ex)
		//	{
             //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.GetBollettinoICI.EuroGridView.errore: ",Ex);
              //  Response.Redirect("../../PaginaErrore.aspx");
			//}
        //}
	}
}
