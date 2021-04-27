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

namespace DichiarazioniICI//.Analisi.FatturatoIncassato
{
    /// <summary>
    /// Pagina del risultato in base ai parametri impostati per la consultazione del raffronto fra fatturato ed incassato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ResultAnalisiEconomiche :BasePage
	{
		Business.ClsAnalisi FncAnalisi=new Business.ClsAnalisi();
		private static readonly ILog log = LogManager.GetLogger(typeof(ResultAnalisiEconomiche));
	
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
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
         
			try 
			{
				if (!Page.IsPostBack) 
				{
					string Anno = Request["DdlAnno"];
					string DataAccreditoDal = Request["AccreditoDal"];
					string DataAccreditoAl = Request["AccreditoAl"];
                    //*** 20140630 - TASI ***
                    string Tributo = Request["Tributo"];//"8852";
					   if (LoadRiepilogoEmesso(Business.ConstWrapper.CodiceEnte, Tributo, Anno, DataAccreditoDal, DataAccreditoAl) == 0)
                    {
                        Response.End();
                    }
                    if (LoadDettaglioEmesso(Business.ConstWrapper.CodiceEnte, Tributo, Anno, DataAccreditoDal, DataAccreditoAl) == 0)
                    {
                        Response.End();
                    }
                    if (LoadDettaglioIncassato(Business.ConstWrapper.CodiceEnte, Tributo, Anno, DataAccreditoDal, DataAccreditoAl) == 0)
                    {
                        Response.End();
                    }
                    //*** ***
           
					string sScript = "";
					sScript = "parent.parent.Comandi.Grafico.disabled=false;";
					RegisterScript(sScript,this.GetType());
				}
			}
			catch (Exception Err) 
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.Page_Load.errore: ", Err);
                Response.Redirect("../../../PaginaErrore.aspx");
            }
            finally
            {
                //if ((WFSessione != null))
                //{
                //    WFSessione.Kill();
                //    WFSessione = null;
                //}
                string sScript = "";
                sScript = "parent.parent.Visualizza.document.getElementById('DivAttesa').style.display = 'none';";
                RegisterScript(sScript,this.GetType());
            }		
		}
        //*** 20140630 - TASI ***
        //private int LoadRiepilogoEmesso(OPENUtility.CreateSessione WFSessione, string sMyEnte, string Anno, string AccreditoDal, string AccreditoAl)
        //{
        //    DataView dvMyDati;
        //    int x = 0;

        //    try 
        //    {
        //        //azzero le text
        //        LblNUtentiDovuto.Text = "0";LblNUtentiVersato.Text = "0";LblNUtenti.Text = "0";
        //        LblTotImpDovuto.Text = "0,00";LblTotImpVersato.Text = "0,00";
        //        LblNPagUS.Text = "0";LblNPagRate.Text = "0";
        //        LblImpPagUS.Text = "0,00";LblImpPagRate.Text = "0,00";LblTotVersato.Text = "0,00";
        //        LblDovuto.Text = "0,00";LblVersato.Text = "0,00";LblInsoluto.Text = "0,00";LblPercentualeInsoluto.Text = "0,00";
        //        LblDovutoAbiPrin.Text = "0,00";LblVersatoAbiPrin.Text = "0,00";
        //        LblDovutoTerAgr.Text = "0,00";LblVersatoTerAgr.Text = "0,00";
        //        LblDovutoTerFab.Text = "0,00";LblVersatoTerFab.Text = "0,00";
        //        LblDovutoAltriFab.Text = "0,00";LblVersatoAltriFab.Text = "0,00";
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        LblDovutoStatoTerAgr.Text = "0,00";LblVersatoStatoTerAgr.Text = "0,00";
        //        LblDovutoStatoTerFab.Text = "0,00";LblVersatoStatoTerFab.Text = "0,00";
        //        LblDovutoStatoAltriFab.Text = "0,00";LblVersatoStatoAltriFab.Text = "0,00";
        //        LblDovutoFabRurUsoStrum.Text = "0,00";LblVersatoFabRurUsoStrum.Text = "0,00";
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        LblDovutoStatoFabRurUsoStrum.Text = "0,00";LblVersatoStatoFabRurUsoStrum.Text = "0,00";

        //        LblDovutoUsoProdCatD.Text = "0,00";LblVersatoUsoProdCatD.Text = "0,00";
        //        LblDovutoStatoUsoProdCatD.Text = "0,00";LblVersatoStatoUsoProdCatD.Text = "0,00";
        //        /**** ****/
        //        LblDovutoDetrazione.Text = "0,00";LblVersatoDetrazione.Text = "0,00";
        //        LblDovutoTot.Text = "0,00";LblVersatoTot.Text = "0,00";
        //        //prelevo i dati dal db del dovuto/versato
        //        dvMyDati = FncAnalisi.GetRiepilogoEmesso(sMyEnte, Anno, AccreditoDal, AccreditoAl, WFSessione);
        //        if ((dvMyDati != null)) 
        //        {
        //            for (x = 0; x <= dvMyDati.Count - 1; x++) 
        //            {
        //                switch (dvMyDati[x].Row["tipo"].ToString())
        //                {
        //                    case "DOVUTO":
        //                        if (dvMyDati[x].Row["nutenti"] != null)
        //                        {
        //                            if (dvMyDati[x].Row["nutenti"].ToString()!="")
        //                            {
        //                                LblNUtentiDovuto.Text = dvMyDati[x].Row["nutenti"].ToString();
        //                            }
        //                        }
        //                        if (dvMyDati[x].Row["importo"] != null)
        //                        {
        //                            if (dvMyDati[x].Row["importo"].ToString()!="")
        //                            {
        //                                LblTotImpDovuto.Text = double.Parse(dvMyDati[x].Row["importo"].ToString()).ToString("#,##0.00");
        //                            }
        //                        }
        //                        break;
        //                    case "VERSATO":
        //                        if (dvMyDati[x].Row["nutenti"] != null)
        //                        {
        //                            if (dvMyDati[x].Row["nutenti"].ToString()!="")
        //                            {
        //                                LblNUtentiVersato.Text = dvMyDati[x].Row["nutenti"].ToString();
        //                            }
        //                        }
        //                        if (dvMyDati[x].Row["importo"] != null)
        //                        {
        //                            if (dvMyDati[x].Row["importo"].ToString()!="")
        //                            {
        //                                LblTotImpVersato.Text = double.Parse(dvMyDati[x].Row["importo"].ToString()).ToString("#,##0.00");
        //                            }
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //        //prelevo i dati dal db dei versamenti US
        //        dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte, Anno, AccreditoDal, AccreditoAl, 1, WFSessione);
        //        if ((dvMyDati != null)) 
        //        {
        //            for (x = 0; x <= dvMyDati.Count - 1; x++) 
        //            {
        //        if (dvMyDati[x].Row["npag"] != null)
        //        {
        //            if (dvMyDati[x].Row["npag"].ToString()!="")
        //            {
        //                LblNPagUS.Text = dvMyDati[x].Row["npag"].ToString();
        //            }
        //        }
        //                if (dvMyDati[x].Row["imppag"] != null)
        //                {
        //                    if (dvMyDati[x].Row["imppag"].ToString()!="")
        //                    {
        //                        LblImpPagUS.Text = double.Parse(dvMyDati[x].Row["imppag"].ToString()).ToString("#,##0.00");
        //                    }
        //                }
        //                    }
        //        }
        //        //prelevo i dati dal db dei versamenti A/S/non specificato
        //        dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte, Anno, AccreditoDal, AccreditoAl, 0, WFSessione);
        //        if ((dvMyDati != null)) 
        //        {
        //            for (x = 0; x <= dvMyDati.Count - 1; x++) 
        //            {
        //                if (dvMyDati[x].Row["npag"] != null)
        //                {
        //                    if (dvMyDati[x].Row["npag"].ToString()!="")
        //                    {
        //                        LblNPagRate.Text = dvMyDati[x].Row["npag"].ToString();
        //                    }
        //                }
        //                if (dvMyDati[x].Row["imppag"] != null)
        //                {
        //                    if (dvMyDati[x].Row["imppag"].ToString()!="")
        //                    {
        //                        LblImpPagRate.Text = double.Parse(dvMyDati[x].Row["imppag"].ToString()).ToString("#,##0.00");
        //                    }
        //                }
        //                    }
        //        }
        //        LblTotVersato.Text = (double.Parse(LblImpPagRate.Text) + double.Parse(LblImpPagUS.Text)).ToString("#,##0.00");
        //        LblNUtenti.Text=LblNUtentiVersato.Text ;
        //        LblDovuto.Text = LblTotImpDovuto.Text;
        //        LblVersato.Text = LblTotImpVersato.Text;
        //        LblInsoluto.Text = (double.Parse(LblTotImpDovuto.Text) - double.Parse(LblTotImpVersato.Text)).ToString("#,##0.00");
        //        LblPercentualeInsoluto.Text = ((double.Parse(LblInsoluto.Text) * 100) / double.Parse(LblTotImpDovuto.Text)).ToString("#,##0.00");
        //        return 1;
        //    }
        //    catch (Exception err) 
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.LoadRiepilogoEmesso.errore: ", err);
       //         Response.Redirect("../../../PaginaErrore.aspx");
        //        return 0;
        //    }
        //}
        private int LoadRiepilogoEmesso(string sMyEnte, string sMyTributo, string Anno, string AccreditoDal, string AccreditoAl)
        {
            DataView dvMyDati;
            int x = 0;

            try
            {
                //azzero le text
                LblNUtentiDovuto.Text = "0"; LblNUtentiVersato.Text = "0"; LblNUtenti.Text = "0";
                LblTotImpDovuto.Text = "0,00"; LblTotImpVersato.Text = "0,00";
                LblNPagUS.Text = "0"; LblNPagRate.Text = "0";
                LblImpPagUS.Text = "0,00"; LblImpPagRate.Text = "0,00"; LblTotVersato.Text = "0,00";
                LblDovuto.Text = "0,00"; LblVersato.Text = "0,00"; LblInsoluto.Text = "0,00"; LblPercentualeInsoluto.Text = "0,00";
                LblDovutoAbiPrin.Text = "0,00"; LblVersatoAbiPrin.Text = "0,00";
                LblDovutoTerAgr.Text = "0,00"; LblVersatoTerAgr.Text = "0,00";
                LblDovutoTerFab.Text = "0,00"; LblVersatoTerFab.Text = "0,00";
                LblDovutoAltriFab.Text = "0,00"; LblVersatoAltriFab.Text = "0,00";
                /**** 20120828 - IMU adeguamento per importi statali ****/
                LblDovutoStatoTerAgr.Text = "0,00"; LblVersatoStatoTerAgr.Text = "0,00";
                LblDovutoStatoTerFab.Text = "0,00"; LblVersatoStatoTerFab.Text = "0,00";
                LblDovutoStatoAltriFab.Text = "0,00"; LblVersatoStatoAltriFab.Text = "0,00";
                LblDovutoFabRurUsoStrum.Text = "0,00"; LblVersatoFabRurUsoStrum.Text = "0,00";
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                LblDovutoStatoFabRurUsoStrum.Text = "0,00"; LblVersatoStatoFabRurUsoStrum.Text = "0,00";

                LblDovutoUsoProdCatD.Text = "0,00"; LblVersatoUsoProdCatD.Text = "0,00";
                LblDovutoStatoUsoProdCatD.Text = "0,00"; LblVersatoStatoUsoProdCatD.Text = "0,00";
                /**** ****/
                LblDovutoDetrazione.Text = "0,00"; LblVersatoDetrazione.Text = "0,00";
                LblDovutoTot.Text = "0,00"; LblVersatoTot.Text = "0,00";
                //prelevo i dati dal db del dovuto/versato
                dvMyDati = FncAnalisi.GetRiepilogoEmesso(sMyEnte, sMyTributo, Anno, AccreditoDal, AccreditoAl);
                if ((dvMyDati != null))
                {
                    for (x = 0; x <= dvMyDati.Count - 1; x++)
                    {
                        switch (dvMyDati[x].Row["tipo"].ToString())
                        {
                            case "DOVUTO":
                                if (dvMyDati[x].Row["nutenti"] != null)
                                {
                                    if (dvMyDati[x].Row["nutenti"].ToString() != "")
                                    {
                                        LblNUtentiDovuto.Text = dvMyDati[x].Row["nutenti"].ToString();
                                    }
                                }
                                if (dvMyDati[x].Row["importo"] != null)
                                {
                                    if (dvMyDati[x].Row["importo"].ToString() != "")
                                    {
                                        LblTotImpDovuto.Text = double.Parse(dvMyDati[x].Row["importo"].ToString()).ToString("#,##0.00");
                                    }
                                }
                                break;
                            case "VERSATO":
                                if (dvMyDati[x].Row["nutenti"] != null)
                                {
                                    if (dvMyDati[x].Row["nutenti"].ToString() != "")
                                    {
                                        LblNUtentiVersato.Text = dvMyDati[x].Row["nutenti"].ToString();
                                    }
                                }
                                if (dvMyDati[x].Row["importo"] != null)
                                {
                                    if (dvMyDati[x].Row["importo"].ToString() != "")
                                    {
                                        LblTotImpVersato.Text = double.Parse(dvMyDati[x].Row["importo"].ToString()).ToString("#,##0.00");
                                    }
                                }
                                break;
                        }
                    }
                }
                //prelevo i dati dal db dei versamenti US
                dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte,sMyTributo, Anno, AccreditoDal, AccreditoAl, 1);
                if ((dvMyDati != null))
                {
                    for (x = 0; x <= dvMyDati.Count - 1; x++)
                    {
                        if (dvMyDati[x].Row["npag"] != null)
                        {
                            if (dvMyDati[x].Row["npag"].ToString() != "")
                            {
                                LblNPagUS.Text = dvMyDati[x].Row["npag"].ToString();
                            }
                        }
                        if (dvMyDati[x].Row["imppag"] != null)
                        {
                            if (dvMyDati[x].Row["imppag"].ToString() != "")
                            {
                                LblImpPagUS.Text = double.Parse(dvMyDati[x].Row["imppag"].ToString()).ToString("#,##0.00");
                            }
                        }
                    }
                }
                //prelevo i dati dal db dei versamenti A/S/non specificato
                dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte,sMyTributo, Anno, AccreditoDal, AccreditoAl, 0);
                if ((dvMyDati != null))
                {
                    for (x = 0; x <= dvMyDati.Count - 1; x++)
                    {
                        if (dvMyDati[x].Row["npag"] != null)
                        {
                            if (dvMyDati[x].Row["npag"].ToString() != "")
                            {
                                LblNPagRate.Text = dvMyDati[x].Row["npag"].ToString();
                            }
                        }
                        if (dvMyDati[x].Row["imppag"] != null)
                        {
                            if (dvMyDati[x].Row["imppag"].ToString() != "")
                            {
                                LblImpPagRate.Text = double.Parse(dvMyDati[x].Row["imppag"].ToString()).ToString("#,##0.00");
                            }
                        }
                    }
                }
                LblTotVersato.Text = (double.Parse(LblImpPagRate.Text) + double.Parse(LblImpPagUS.Text)).ToString("#,##0.00");
                LblNUtenti.Text = LblNUtentiDovuto.Text;
                LblDovuto.Text = LblTotImpDovuto.Text;
                LblVersato.Text = LblTotImpVersato.Text;
                LblInsoluto.Text = (double.Parse(LblTotImpDovuto.Text) - double.Parse(LblTotImpVersato.Text)).ToString("#,##0.00");
                LblPercentualeInsoluto.Text = ((double.Parse(LblInsoluto.Text) * 100) / double.Parse(LblTotImpDovuto.Text)).ToString("#,##0.00");
                return 1;
            }
            catch (Exception err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.LoadRiepilogoEmesso.errore: ", err);
                 Response.Redirect("../../../PaginaErrore.aspx");
                return 0;
            }
        }
        //private int LoadDettaglioEmesso(OPENUtility.CreateSessione WFSessione, string sMyEnte, string Anno, string AccreditoDal, string AccreditoAl)
        //{
        //    DataView dvMyDati=null;
        //    int x = 0;
        //    double AbiPrin = 0;
        //    double TerAgr = 0;
        //    double TerFab = 0;
        //    double AltriFab = 0;
        //    /**** 20120828 - IMU adeguamento per importi statali ****/
        //    double TerAgrStato = 0;
        //    double TerFabStato = 0;
        //    double AltriFabStato = 0;
        //    double FabRurUsoStrum = 0;
        //    /**** ****/
        //    /**** 20130422 - aggiornamento IMU ****/
        //    double FabRurUsoStrumStato = 0;
        //    double UsoProdCatD=0;
        //    double UsoProdCatDStato=0;
        //    /**** ****/
        //    double Detrazione = 0;
        //    double Tot = 0;

        //    try 
        //    {
        //        //azzero le text
        //        LblDovutoUtenti.Text = "0";
        //        LblDovutoAbiPrin.Text = "0,00";TxtAbiPrinDovuto.Text = "0,00";
        //        LblDovutoTerAgr.Text = "0,00";TxtTerAgrDovuto.Text = "0,00";
        //        LblDovutoTerFab.Text = "0,00";TxtTerFabDovuto.Text = "0,00";
        //        LblDovutoAltriFab.Text = "0,00";TxtAltriFabDovuto.Text = "0,00";
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        LblDovutoStatoTerAgr.Text = "0,00";LblVersatoStatoTerAgr.Text = "0,00";
        //        LblDovutoStatoTerFab.Text = "0,00";LblVersatoStatoTerFab.Text = "0,00";
        //        LblDovutoStatoAltriFab.Text = "0,00";LblVersatoStatoAltriFab.Text = "0,00";
        //        LblDovutoFabRurUsoStrum.Text = "0,00";LblVersatoFabRurUsoStrum.Text = "0,00";
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        LblDovutoStatoFabRurUsoStrum.Text = "0,00";LblVersatoStatoFabRurUsoStrum.Text = "0,00";

        //        LblDovutoUsoProdCatD.Text = "0,00";LblVersatoUsoProdCatD.Text = "0,00";
        //        LblDovutoStatoUsoProdCatD.Text = "0,00";LblVersatoStatoUsoProdCatD.Text = "0,00";
        //        /**** ****/
        //        LblDovutoDetrazione.Text = "0,00";TxtDetrazDovuto.Text = "0,00";
        //        LblDovutoTot.Text = "0,00";TxtTotaleDovuto.Text = "0,00";
        //        //prelevo i dati dal db dell'emesso
        //        dvMyDati = FncAnalisi.GetDettaglioEmesso(sMyEnte, Anno, AccreditoDal, AccreditoAl, WFSessione);
        //        if ((dvMyDati != null)) 
        //        {
        //            for (x = 0; x <= dvMyDati.Count - 1; x++) 
        //            {
        //                if (dvMyDati[x].Row["abiprin"].ToString()!="")
        //                {
        //                    AbiPrin += double.Parse(dvMyDati[x].Row["abiprin"].ToString());
        //                }
        //                if (dvMyDati[x].Row["teragr"].ToString()!="")
        //                {
        //                    TerAgr += double.Parse(dvMyDati[x].Row["teragr"].ToString());
        //                }
        //                if (dvMyDati[x].Row["terfab"].ToString()!="")
        //                {
        //                    TerFab += double.Parse(dvMyDati[x].Row["terfab"].ToString());
        //                }
        //                if (dvMyDati[x].Row["altrifab"].ToString()!="")
        //                {
        //                    AltriFab += double.Parse(dvMyDati[x].Row["altrifab"].ToString());
        //                }
        //                /**** 20120828 - IMU adeguamento per importi statali ****/
        //                if (dvMyDati[x].Row["teragrStato"].ToString()!="")
        //                {
        //                    TerAgrStato += double.Parse(dvMyDati[x].Row["teragrStato"].ToString());
        //                }
        //                if (dvMyDati[x].Row["terfabStato"].ToString()!="")
        //                {
        //                    TerFabStato += double.Parse(dvMyDati[x].Row["terfabStato"].ToString());
        //                }
        //                if (dvMyDati[x].Row["altrifabStato"].ToString()!="")
        //                {
        //                    AltriFabStato += double.Parse(dvMyDati[x].Row["altrifabStato"].ToString());
        //                }
        //                if (dvMyDati[x].Row["FabRurUsoStrum"].ToString()!="")
        //                {
        //                    FabRurUsoStrum += double.Parse(dvMyDati[x].Row["FabRurUsoStrum"].ToString());
        //                }
        //                /**** ****/
        //                /**** 20130422 - aggiornamento IMU ****/
        //                if (dvMyDati[x].Row["FabRurUsoStrumStato"].ToString()!="")
        //                {
        //                    FabRurUsoStrumStato += double.Parse(dvMyDati[x].Row["FabRurUsoStrumStato"].ToString());
        //                }

        //                if (dvMyDati[x].Row["UsoProdCatD"].ToString()!="")
        //                {
        //                    UsoProdCatD += double.Parse(dvMyDati[x].Row["UsoProdCatD"].ToString());
        //                }
        //                if (dvMyDati[x].Row["UsoProdCatDStato"].ToString()!="")
        //                {
        //                    UsoProdCatDStato += double.Parse(dvMyDati[x].Row["UsoProdCatDStato"].ToString());
        //                }
        //                /**** ****/
        //                if (dvMyDati[x].Row["detrazione"].ToString()!="")
        //                {
        //                    Detrazione += double.Parse(dvMyDati[x].Row["detrazione"].ToString());
        //                }
        //                if (dvMyDati[x].Row["importo"].ToString()!="")
        //                {
        //                    Tot += double.Parse(dvMyDati[x].Row["importo"].ToString());
        //                }
        //            }
        //        }

        //        LblDovutoUtenti.Text=LblNUtentiDovuto.Text;
        //        LblDovutoAbiPrin.Text = AbiPrin.ToString("#,##0.00");TxtAbiPrinDovuto.Text = AbiPrin.ToString("#,##0.00");
        //        LblDovutoTerAgr.Text = TerAgr.ToString("#,##0.00");TxtTerAgrDovuto.Text= TerAgr.ToString("#,##0.00");
        //        LblDovutoTerFab.Text = TerFab.ToString("#,##0.00");TxtTerFabDovuto.Text =TerFab.ToString("#,##0.00");
        //        LblDovutoAltriFab.Text = AltriFab.ToString("#,##0.00");TxtAltriFabDovuto.Text  = AltriFab.ToString("#,##0.00");
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        LblDovutoStatoTerAgr.Text = TerAgrStato.ToString("#,##0.00");TxtTerAgrDovutoStato.Text= TerAgrStato.ToString("#,##0.00");
        //        LblDovutoStatoTerFab.Text = TerFabStato.ToString("#,##0.00");TxtTerFabDovutoStato.Text =TerFabStato.ToString("#,##0.00");
        //        LblDovutoStatoAltriFab.Text = AltriFabStato.ToString("#,##0.00");TxtAltriFabDovutoStato.Text  = AltriFabStato.ToString("#,##0.00");
        //        LblDovutoFabRurUsoStrum.Text = FabRurUsoStrum.ToString("#,##0.00");TxtFabRurUsoStrumDovuto.Text = FabRurUsoStrum.ToString("#,##0.00");
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        LblDovutoStatoFabRurUsoStrum.Text = FabRurUsoStrumStato.ToString("#,##0.00");TxtFabRurUsoStrumDovutoStato.Text = FabRurUsoStrumStato.ToString("#,##0.00");

        //        LblDovutoUsoProdCatD.Text = UsoProdCatD.ToString("#,##0.00");TxtUsoProdCatDDovuto.Text = UsoProdCatD.ToString("#,##0.00");
        //        LblDovutoStatoUsoProdCatD.Text = UsoProdCatDStato.ToString("#,##0.00");TxtUsoProdCatDDovutoStato.Text = UsoProdCatDStato.ToString("#,##0.00");
        //        /**** ****/
        //        LblDovutoDetrazione.Text =Detrazione.ToString("#,##0.00");TxtDetrazDovuto.Text =Detrazione.ToString("#,##0.00");
        //        LblDovutoTot.Text = Tot.ToString("#,##0.00");TxtTotaleDovuto.Text = Tot.ToString("#,##0.00");

        //        return 1;
        //    }
        //    catch (Exception err) 
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.LoadDettaglioEmesso.errore: ", err);
        //         Response.Redirect("../../../PaginaErrore.aspx");
        //        return 0;
        //    }
        //    finally 
        //    {
        //        dvMyDati.Dispose();
        //    }
        //}
        private int LoadDettaglioEmesso(string sMyEnte, string sMyTributo, string Anno, string AccreditoDal, string AccreditoAl)
        {
            DataView dvMyDati = null;
            int x = 0;
            double AbiPrin = 0;
            double TerAgr = 0;
            double TerFab = 0;
            double AltriFab = 0;
            /**** 20120828 - IMU adeguamento per importi statali ****/
            double TerAgrStato = 0;
            double TerFabStato = 0;
            double AltriFabStato = 0;
            double FabRurUsoStrum = 0;
            /**** ****/
            /**** 20130422 - aggiornamento IMU ****/
            double FabRurUsoStrumStato = 0;
            double UsoProdCatD = 0;
            double UsoProdCatDStato = 0;
            /**** ****/
            double Detrazione = 0;
            double Tot = 0;

            try
            {
                //azzero le text
                LblDovutoUtenti.Text = "0";
                LblDovutoAbiPrin.Text = "0,00"; TxtAbiPrinDovuto.Text = "0,00";
                LblDovutoTerAgr.Text = "0,00"; TxtTerAgrDovuto.Text = "0,00";
                LblDovutoTerFab.Text = "0,00"; TxtTerFabDovuto.Text = "0,00";
                LblDovutoAltriFab.Text = "0,00"; TxtAltriFabDovuto.Text = "0,00";
                /**** 20120828 - IMU adeguamento per importi statali ****/
                LblDovutoStatoTerAgr.Text = "0,00"; 
                LblDovutoStatoTerFab.Text = "0,00"; 
                LblDovutoStatoAltriFab.Text = "0,00"; 
                LblDovutoFabRurUsoStrum.Text = "0,00"; 
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                LblDovutoStatoFabRurUsoStrum.Text = "0,00"; 
                LblDovutoUsoProdCatD.Text = "0,00"; 
                LblDovutoStatoUsoProdCatD.Text = "0,00"; 
                /**** ****/
                LblDovutoDetrazione.Text = "0,00"; TxtDetrazDovuto.Text = "0,00";
                LblDovutoTot.Text = "0,00"; TxtTotaleDovuto.Text = "0,00";
                //prelevo i dati dal db dell'emesso
                dvMyDati = FncAnalisi.GetDettaglioEmesso(sMyEnte,sMyTributo, Anno, AccreditoDal, AccreditoAl);
                if ((dvMyDati != null))
                {
                    for (x = 0; x <= dvMyDati.Count - 1; x++)
                    {
                        if (dvMyDati[x].Row["abiprin"].ToString() != "")
                        {
                            AbiPrin += double.Parse(dvMyDati[x].Row["abiprin"].ToString());
                        }
                        if (dvMyDati[x].Row["teragr"].ToString() != "")
                        {
                            TerAgr += double.Parse(dvMyDati[x].Row["teragr"].ToString());
                        }
                        if (dvMyDati[x].Row["terfab"].ToString() != "")
                        {
                            TerFab += double.Parse(dvMyDati[x].Row["terfab"].ToString());
                        }
                        if (dvMyDati[x].Row["altrifab"].ToString() != "")
                        {
                            AltriFab += double.Parse(dvMyDati[x].Row["altrifab"].ToString());
                        }
                        /**** 20120828 - IMU adeguamento per importi statali ****/
                        if (dvMyDati[x].Row["teragrStato"].ToString() != "")
                        {
                            TerAgrStato += double.Parse(dvMyDati[x].Row["teragrStato"].ToString());
                        }
                        if (dvMyDati[x].Row["terfabStato"].ToString() != "")
                        {
                            TerFabStato += double.Parse(dvMyDati[x].Row["terfabStato"].ToString());
                        }
                        if (dvMyDati[x].Row["altrifabStato"].ToString() != "")
                        {
                            AltriFabStato += double.Parse(dvMyDati[x].Row["altrifabStato"].ToString());
                        }
                        if (dvMyDati[x].Row["FabRurUsoStrum"].ToString() != "")
                        {
                            FabRurUsoStrum += double.Parse(dvMyDati[x].Row["FabRurUsoStrum"].ToString());
                        }
                        /**** ****/
                        /**** 20130422 - aggiornamento IMU ****/
                        if (dvMyDati[x].Row["FabRurUsoStrumStato"].ToString() != "")
                        {
                            FabRurUsoStrumStato += double.Parse(dvMyDati[x].Row["FabRurUsoStrumStato"].ToString());
                        }

                        if (dvMyDati[x].Row["UsoProdCatD"].ToString() != "")
                        {
                            UsoProdCatD += double.Parse(dvMyDati[x].Row["UsoProdCatD"].ToString());
                        }
                        if (dvMyDati[x].Row["UsoProdCatDStato"].ToString() != "")
                        {
                            UsoProdCatDStato += double.Parse(dvMyDati[x].Row["UsoProdCatDStato"].ToString());
                        }
                        /**** ****/
                        if (dvMyDati[x].Row["detrazione"].ToString() != "")
                        {
                            Detrazione += double.Parse(dvMyDati[x].Row["detrazione"].ToString());
                        }
                        if (dvMyDati[x].Row["importo"].ToString() != "")
                        {
                            Tot += double.Parse(dvMyDati[x].Row["importo"].ToString());
                        }
                    }
                }

                LblDovutoUtenti.Text = LblNUtentiDovuto.Text;
                LblDovutoAbiPrin.Text = AbiPrin.ToString("#,##0.00"); TxtAbiPrinDovuto.Text = AbiPrin.ToString("#,##0.00");
                LblDovutoTerAgr.Text = TerAgr.ToString("#,##0.00"); TxtTerAgrDovuto.Text = TerAgr.ToString("#,##0.00");
                LblDovutoTerFab.Text = TerFab.ToString("#,##0.00"); TxtTerFabDovuto.Text = TerFab.ToString("#,##0.00");
                LblDovutoAltriFab.Text = AltriFab.ToString("#,##0.00"); TxtAltriFabDovuto.Text = AltriFab.ToString("#,##0.00");
                /**** 20120828 - IMU adeguamento per importi statali ****/
                LblDovutoStatoTerAgr.Text = TerAgrStato.ToString("#,##0.00"); TxtTerAgrDovutoStato.Text = TerAgrStato.ToString("#,##0.00");
                LblDovutoStatoTerFab.Text = TerFabStato.ToString("#,##0.00"); TxtTerFabDovutoStato.Text = TerFabStato.ToString("#,##0.00");
                LblDovutoStatoAltriFab.Text = AltriFabStato.ToString("#,##0.00"); TxtAltriFabDovutoStato.Text = AltriFabStato.ToString("#,##0.00");
                LblDovutoFabRurUsoStrum.Text = FabRurUsoStrum.ToString("#,##0.00"); TxtFabRurUsoStrumDovuto.Text = FabRurUsoStrum.ToString("#,##0.00");
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                LblDovutoStatoFabRurUsoStrum.Text = FabRurUsoStrumStato.ToString("#,##0.00"); TxtFabRurUsoStrumDovutoStato.Text = FabRurUsoStrumStato.ToString("#,##0.00");
                LblDovutoUsoProdCatD.Text = UsoProdCatD.ToString("#,##0.00"); TxtUsoProdCatDDovuto.Text = UsoProdCatD.ToString("#,##0.00");
                LblDovutoStatoUsoProdCatD.Text = UsoProdCatDStato.ToString("#,##0.00"); TxtUsoProdCatDDovutoStato.Text = UsoProdCatDStato.ToString("#,##0.00");
                /**** ****/
                LblDovutoDetrazione.Text = Detrazione.ToString("#,##0.00"); TxtDetrazDovuto.Text = Detrazione.ToString("#,##0.00");
                LblDovutoTot.Text = Tot.ToString("#,##0.00"); TxtTotaleDovuto.Text = Tot.ToString("#,##0.00");

                return 1;
            }
            catch (Exception err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.LoadDettaglioEmesso.errore: ", err);
                   Response.Redirect("../../../PaginaErrore.aspx");
                return 0;
            }
            finally
            {
                dvMyDati.Dispose();
            }
        }
        //private int LoadDettaglioIncassato(OPENUtility.CreateSessione WFSessione, string sMyEnte, string Anno, string AccreditoDal, string AccreditoAl)
        //{
        //    DataView dvMyDati=null;
        //    int x = 0;
        //    double AbiPrin = 0;
        //    double TerAgr = 0;
        //    double TerFab = 0;
        //    double AltriFab = 0;
        //    /**** 20120828 - IMU adeguamento per importi statali ****/
        //    double TerAgrStato = 0;
        //    double TerFabStato = 0;
        //    double AltriFabStato = 0;
        //    double FabRurUsoStrum = 0;
        //    /**** ****/
        //    /**** 20130422 - aggiornamento IMU ****/
        //    double FabRurUsoStrumStato = 0;
        //    double UsoProdCatD=0;
        //    double UsoProdCatDStato=0;
        //    /**** ****/
        //    double Detrazione = 0;
        //    double Tot = 0;

        //    try 
        //    {
        //        //azzero le text
        //        LblVersatoUtenti.Text = "0";
        //        LblVersatoAbiPrin.Text = "0,00";TxtAbiPrinVersato.Text = "0,00";
        //        LblVersatoTerAgr.Text = "0,00";TxtTerAgrVersato.Text = "0,00";
        //        LblVersatoTerFab.Text = "0,00";TxtTerFabVersato.Text = "0,00";
        //        LblVersatoAltriFab.Text = "0,00";TxtAltriFabVersato.Text = "0,00";
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        LblDovutoStatoTerAgr.Text = "0,00";LblVersatoStatoTerAgr.Text = "0,00";
        //        LblDovutoStatoTerFab.Text = "0,00";LblVersatoStatoTerFab.Text = "0,00";
        //        LblDovutoStatoAltriFab.Text = "0,00";LblVersatoStatoAltriFab.Text = "0,00";
        //        LblDovutoFabRurUsoStrum.Text = "0,00";LblVersatoFabRurUsoStrum.Text = "0,00";
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        LblDovutoStatoFabRurUsoStrum.Text = "0,00";LblVersatoStatoFabRurUsoStrum.Text = "0,00";

        //        LblDovutoUsoProdCatD.Text = "0,00";LblVersatoUsoProdCatD.Text = "0,00";
        //        LblDovutoStatoUsoProdCatD.Text = "0,00";LblVersatoStatoUsoProdCatD.Text = "0,00";
        //        /**** ****/
        //        LblVersatoDetrazione.Text = "0,00";TxtDetrazVersato.Text = "0,00";
        //        LblVersatoTot.Text = "0,00";TxtTotaleVersato.Text = "0,00";
        //        //prelevo i dati dal db dell'emesso
        //        dvMyDati = FncAnalisi.GetDettaglioVersato(sMyEnte, Anno, AccreditoDal, AccreditoAl, WFSessione);
        //        if ((dvMyDati != null)) 
        //        {
        //            for (x = 0; x <= dvMyDati.Count - 1; x++) 
        //            {
        //                if (dvMyDati[x].Row["abiprin"].ToString()!="")
        //                {
        //                    AbiPrin += double.Parse(dvMyDati[x].Row["abiprin"].ToString());
        //                }
        //                if (dvMyDati[x].Row["teragr"].ToString()!="")
        //                {
        //                    TerAgr += double.Parse(dvMyDati[x].Row["teragr"].ToString());
        //                }
        //                if (dvMyDati[x].Row["terfab"].ToString()!="")
        //                {
        //                    TerFab += double.Parse(dvMyDati[x].Row["terfab"].ToString());
        //                }
        //                if (dvMyDati[x].Row["altrifab"].ToString()!="")
        //                {
        //                    AltriFab += double.Parse(dvMyDati[x].Row["altrifab"].ToString());
        //                }
        //                /**** 20120828 - IMU adeguamento per importi statali ****/
        //                if (dvMyDati[x].Row["teragrStato"].ToString()!="")
        //                {
        //                    TerAgrStato += double.Parse(dvMyDati[x].Row["teragrStato"].ToString());
        //                }
        //                if (dvMyDati[x].Row["terfabStato"].ToString()!="")
        //                {
        //                    TerFabStato += double.Parse(dvMyDati[x].Row["terfabStato"].ToString());
        //                }
        //                if (dvMyDati[x].Row["altrifabStato"].ToString()!="")
        //                {
        //                    AltriFabStato += double.Parse(dvMyDati[x].Row["altrifabStato"].ToString());
        //                }
        //                if (dvMyDati[x].Row["FabRurUsoStrum"].ToString()!="")
        //                {
        //                    FabRurUsoStrum += double.Parse(dvMyDati[x].Row["FabRurUsoStrum"].ToString());
        //                }
        //                /**** ****/
        //                /**** 20130422 - aggiornamento IMU ****/
        //                if (dvMyDati[x].Row["FabRurUsoStrumStato"].ToString()!="")
        //                {
        //                    FabRurUsoStrumStato += double.Parse(dvMyDati[x].Row["FabRurUsoStrumStato"].ToString());
        //                }

        //                if (dvMyDati[x].Row["UsoProdCatD"].ToString()!="")
        //                {
        //                    UsoProdCatD += double.Parse(dvMyDati[x].Row["UsoProdCatD"].ToString());
        //                }
        //                if (dvMyDati[x].Row["UsoProdCatDStato"].ToString()!="")
        //                {
        //                    UsoProdCatDStato += double.Parse(dvMyDati[x].Row["UsoProdCatDStato"].ToString());
        //                }
        //                /*** ****/
        //                if (dvMyDati[x].Row["detrazione"].ToString()!="")
        //                {
        //                    Detrazione += double.Parse(dvMyDati[x].Row["detrazione"].ToString());
        //                }
        //                if (dvMyDati[x].Row["importo"].ToString()!="")
        //                {
        //                    Tot += double.Parse(dvMyDati[x].Row["importo"].ToString());
        //                }
        //            }
        //        }

        //        LblVersatoUtenti.Text=LblNUtentiVersato.Text;
        //        LblVersatoAbiPrin.Text = AbiPrin.ToString("#,##0.00");TxtAbiPrinVersato.Text = AbiPrin.ToString("#,##0.00");
        //        LblVersatoTerAgr.Text = TerAgr.ToString("#,##0.00");TxtTerAgrVersato.Text= TerAgr.ToString("#,##0.00");
        //        LblVersatoTerFab.Text = TerFab.ToString("#,##0.00");TxtTerFabVersato.Text =TerFab.ToString("#,##0.00");
        //        LblVersatoAltriFab.Text = AltriFab.ToString("#,##0.00");TxtAltriFabVersato.Text  = AltriFab.ToString("#,##0.00");
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        LblVersatoStatoTerAgr.Text = TerAgrStato.ToString("#,##0.00");TxtTerAgrVersatoStato.Text= TerAgrStato.ToString("#,##0.00");
        //        LblVersatoStatoTerFab.Text = TerFabStato.ToString("#,##0.00");TxtTerFabVersatoStato.Text =TerFabStato.ToString("#,##0.00");
        //        LblVersatoStatoAltriFab.Text = AltriFabStato.ToString("#,##0.00");TxtAltriFabVersatoStato.Text  = AltriFabStato.ToString("#,##0.00");
        //        LblVersatoFabRurUsoStrum.Text = FabRurUsoStrum.ToString("#,##0.00");TxtFabRurUsoStrumVersato.Text = FabRurUsoStrum.ToString("#,##0.00");
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        LblVersatoStatoFabRurUsoStrum.Text = FabRurUsoStrumStato.ToString("#,##0.00");TxtFabRurUsoStrumVersatoStato.Text = FabRurUsoStrumStato.ToString("#,##0.00");

        //        LblVersatoUsoProdCatD.Text = UsoProdCatD.ToString("#,##0.00");TxtUsoProdCatDVersato.Text = UsoProdCatD.ToString("#,##0.00");
        //        LblVersatoStatoUsoProdCatD.Text = UsoProdCatDStato.ToString("#,##0.00");TxtUsoProdCatDVersatoStato.Text = UsoProdCatDStato.ToString("#,##0.00");
        //        /**** ****/
        //        LblVersatoDetrazione.Text =Detrazione.ToString("#,##0.00");TxtDetrazVersato.Text =Detrazione.ToString("#,##0.00");
        //        LblVersatoTot.Text = Tot.ToString("#,##0.00");TxtTotaleVersato.Text = Tot.ToString("#,##0.00");

        //        return 1;
        //    }
        //    catch (Exception err) 
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.LoadDettaglioIncassato.errore: ", err);
        //Response.Redirect("../../../PaginaErrore.aspx");
        //        return 0;
        //    }
        //    finally 
        //    {
        //        dvMyDati.Dispose();
        //    }
        //}
        private int LoadDettaglioIncassato(string sMyEnte, string sMyTributo, string Anno, string AccreditoDal, string AccreditoAl)
        {
            DataView dvMyDati = null;
            int x = 0;
            double AbiPrin = 0;
            double TerAgr = 0;
            double TerFab = 0;
            double AltriFab = 0;
            /**** 20120828 - IMU adeguamento per importi statali ****/
            double TerAgrStato = 0;
            double TerFabStato = 0;
            double AltriFabStato = 0;
            double FabRurUsoStrum = 0;
            /**** ****/
            /**** 20130422 - aggiornamento IMU ****/
            double FabRurUsoStrumStato = 0;
            double UsoProdCatD = 0;
            double UsoProdCatDStato = 0;
            /**** ****/
            double Detrazione = 0;
            double Tot = 0;

            try
            {
                //azzero le text
                LblVersatoUtenti.Text = "0";
                LblVersatoAbiPrin.Text = "0,00"; TxtAbiPrinVersato.Text = "0,00";
                LblVersatoTerAgr.Text = "0,00"; TxtTerAgrVersato.Text = "0,00";
                LblVersatoTerFab.Text = "0,00"; TxtTerFabVersato.Text = "0,00";
                LblVersatoAltriFab.Text = "0,00"; TxtAltriFabVersato.Text = "0,00";
                /**** 20120828 - IMU adeguamento per importi statali ****/
                LblVersatoStatoTerAgr.Text = "0,00";
                LblVersatoStatoTerFab.Text = "0,00";
                 LblVersatoStatoAltriFab.Text = "0,00";
                 LblVersatoFabRurUsoStrum.Text = "0,00";
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                 LblVersatoStatoFabRurUsoStrum.Text = "0,00";

                 LblVersatoUsoProdCatD.Text = "0,00";
                LblVersatoStatoUsoProdCatD.Text = "0,00";
                /**** ****/
                LblVersatoDetrazione.Text = "0,00"; TxtDetrazVersato.Text = "0,00";
                LblVersatoTot.Text = "0,00"; TxtTotaleVersato.Text = "0,00";
                //prelevo i dati dal db dell'emesso
                dvMyDati = FncAnalisi.GetDettaglioVersato(sMyEnte,sMyTributo, Anno, AccreditoDal, AccreditoAl);
                if ((dvMyDati != null))
                {
                    for (x = 0; x <= dvMyDati.Count - 1; x++)
                    {
                        if (dvMyDati[x].Row["abiprin"].ToString() != "")
                        {
                            AbiPrin += double.Parse(dvMyDati[x].Row["abiprin"].ToString());
                        }
                        if (dvMyDati[x].Row["teragr"].ToString() != "")
                        {
                            TerAgr += double.Parse(dvMyDati[x].Row["teragr"].ToString());
                        }
                        if (dvMyDati[x].Row["terfab"].ToString() != "")
                        {
                            TerFab += double.Parse(dvMyDati[x].Row["terfab"].ToString());
                        }
                        if (dvMyDati[x].Row["altrifab"].ToString() != "")
                        {
                            AltriFab += double.Parse(dvMyDati[x].Row["altrifab"].ToString());
                        }
                        /**** 20120828 - IMU adeguamento per importi statali ****/
                        if (dvMyDati[x].Row["teragrStato"].ToString() != "")
                        {
                            TerAgrStato += double.Parse(dvMyDati[x].Row["teragrStato"].ToString());
                        }
                        if (dvMyDati[x].Row["terfabStato"].ToString() != "")
                        {
                            TerFabStato += double.Parse(dvMyDati[x].Row["terfabStato"].ToString());
                        }
                        if (dvMyDati[x].Row["altrifabStato"].ToString() != "")
                        {
                            AltriFabStato += double.Parse(dvMyDati[x].Row["altrifabStato"].ToString());
                        }
                        if (dvMyDati[x].Row["FabRurUsoStrum"].ToString() != "")
                        {
                            FabRurUsoStrum += double.Parse(dvMyDati[x].Row["FabRurUsoStrum"].ToString());
                        }
                        /**** ****/
                        /**** 20130422 - aggiornamento IMU ****/
                        if (dvMyDati[x].Row["FabRurUsoStrumStato"].ToString() != "")
                        {
                            FabRurUsoStrumStato += double.Parse(dvMyDati[x].Row["FabRurUsoStrumStato"].ToString());
                        }

                        if (dvMyDati[x].Row["UsoProdCatD"].ToString() != "")
                        {
                            UsoProdCatD += double.Parse(dvMyDati[x].Row["UsoProdCatD"].ToString());
                        }
                        if (dvMyDati[x].Row["UsoProdCatDStato"].ToString() != "")
                        {
                            UsoProdCatDStato += double.Parse(dvMyDati[x].Row["UsoProdCatDStato"].ToString());
                        }
                        /*** ****/
                        if (dvMyDati[x].Row["detrazione"].ToString() != "")
                        {
                            Detrazione += double.Parse(dvMyDati[x].Row["detrazione"].ToString());
                        }
                        if (dvMyDati[x].Row["importo"].ToString() != "")
                        {
                            Tot += double.Parse(dvMyDati[x].Row["importo"].ToString());
                        }
                    }
                }

                LblVersatoUtenti.Text = LblNUtentiVersato.Text;
                LblVersatoAbiPrin.Text = AbiPrin.ToString("#,##0.00"); TxtAbiPrinVersato.Text = AbiPrin.ToString("#,##0.00");
                LblVersatoTerAgr.Text = TerAgr.ToString("#,##0.00"); TxtTerAgrVersato.Text = TerAgr.ToString("#,##0.00");
                LblVersatoTerFab.Text = TerFab.ToString("#,##0.00"); TxtTerFabVersato.Text = TerFab.ToString("#,##0.00");
                LblVersatoAltriFab.Text = AltriFab.ToString("#,##0.00"); TxtAltriFabVersato.Text = AltriFab.ToString("#,##0.00");
                /**** 20120828 - IMU adeguamento per importi statali ****/
                LblVersatoStatoTerAgr.Text = TerAgrStato.ToString("#,##0.00"); TxtTerAgrVersatoStato.Text = TerAgrStato.ToString("#,##0.00");
                LblVersatoStatoTerFab.Text = TerFabStato.ToString("#,##0.00"); TxtTerFabVersatoStato.Text = TerFabStato.ToString("#,##0.00");
                LblVersatoStatoAltriFab.Text = AltriFabStato.ToString("#,##0.00"); TxtAltriFabVersatoStato.Text = AltriFabStato.ToString("#,##0.00");
                LblVersatoFabRurUsoStrum.Text = FabRurUsoStrum.ToString("#,##0.00"); TxtFabRurUsoStrumVersato.Text = FabRurUsoStrum.ToString("#,##0.00");
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                LblVersatoStatoFabRurUsoStrum.Text = FabRurUsoStrumStato.ToString("#,##0.00"); TxtFabRurUsoStrumVersatoStato.Text = FabRurUsoStrumStato.ToString("#,##0.00");

                LblVersatoUsoProdCatD.Text = UsoProdCatD.ToString("#,##0.00"); TxtUsoProdCatDVersato.Text = UsoProdCatD.ToString("#,##0.00");
                LblVersatoStatoUsoProdCatD.Text = UsoProdCatDStato.ToString("#,##0.00"); TxtUsoProdCatDVersatoStato.Text = UsoProdCatDStato.ToString("#,##0.00");
                /**** ****/
                LblVersatoDetrazione.Text = Detrazione.ToString("#,##0.00"); TxtDetrazVersato.Text = Detrazione.ToString("#,##0.00");
                LblVersatoTot.Text = Tot.ToString("#,##0.00"); TxtTotaleVersato.Text = Tot.ToString("#,##0.00");

                return 1;
            }
            catch (Exception err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ResultAnalisiEconomiche.LoadDettaglioIncassato.errore: ", err);
                Response.Redirect("../../../PaginaErrore.aspx");
                return 0;
            }
            finally
            {
                dvMyDati.Dispose();
            }
        }
        //*** ***
    }
}
