using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using System.Data;
using log4net;

namespace OPENgov.Acquisizioni.TARES.Simulazione
{/// <summary>
/// Pagina per la consultazione/elaborazione della simulazione tariffaria.
/// 
/// Il Calcolo della parte Fissa della Tariffa e' dato da									
/// 			(superficie * correttivo dato da n.componenti nucleo)							
/// definizioni:
/// 			TFd(n,S)=Tariffa fissa utenze domestiche							
/// 			n = n.componenti nucleo familiare							
/// 			S = superficie abitazione							
/// 			TFd(n,S)=Quf * S * Ka(n)							
/// 			Quf = quota unitaria €/m2 determ. Tra costi fissi attrib.a utenze domestiche e sup.totale corretta da coefficiente di adattamento ( Ka)							
/// 										
/// 			Quf=Ctudf/Sommatoria S(n) * Ka(n)							
/// 			Ctuf = costi fissi attribuili alle utenze domestiche							
/// 			Ka = coefficiente di adattamento in base alla reale distrib.di superfici e n. componenti 							
/// 										
/// Per il Calcolo del Quf si devono determinare le superfici adattate al coefficiente e quindi il Quf (quota unitaria €/m2) risulta essere di :									
/// 				Quf = Ctuf / Sommatoria S (n) * Ka(n)		
/// 				
/// Il Calcolo della parte Variabile Utenze Domestiche si ottiene come prodotto della quota unitaria (qta rifiuti rapportata ad ogni singola utenza in funzione del numero di componenti del nucleo corretto da un coefficiente di proporzionalità per un coefficiente di adattamento per il costo unitario (€/Kg)								
/// definizioni:						
/// 			TVd(n,S)=Quv* Kb* Cu							
/// 			n= n.componenti nucleo familiare								
/// 			Cu = costo unitario  €/Kg. Rapporto tra costi variabili attrib.ut.domest. e Q.tot.rif. Prodotti da n. utenze domestiche								
/// 											
/// 			Kb= Coefficiente proporzionale di produttività per utenza domestica in funzione del numero dei componenti del nucelo familiare costituente la singola utenza.								
/// 											
/// 			Quv = quota unitaria: rapporto tra qta tot.rifiuti dom.e n.tot.utenze dom.in funzione del n. componenti nucleo familiare corrette da un coefficiente proporz. di produttività								
/// 											
/// 			N= n.totale delle Utenze domestiche in funzione del n. di comp.del nucleo familiare								
/// 			Qtot = quantita' totale rifiuti								
/// 			Quv = Qtot / Sommatoria di ( N(n) * Kb(n))								
/// 									
/// Per il Calcolo del Quv Tabella 2 - coeff. Per l'attribuzione della parte variabile della tariffa ut.domestiche e quindi il Quv risulta essere di :								
/// 	Q.Tot.Rfiuti/somm.N.ut*Kb						    Quv		
/// quindi il Cu (costo unitario €/Kg) risulta essere di :								
/// 	costi variab.ut.dom./qta rifiuti ut.dom.						Cu	
/// 	
/// Il Calcolo della parte Fissa della Tariffa per  NON domestiche si ottiene come prodotto dalla quota unitaria (€/m2) per al superficie dell'utenza per il coefficiente potenziale di produzione per tipologia di attività (Kc)									
/// definizioni:
/// 			TFnd(ap, Sap) = Qapf * Sap (ap) * Kc(ap)								
/// 			Tfnd  = quota fissa della tariffa per ut non domestica di tipologia ap e superficie Sap								
/// 			Sap= superficie locali attività produttiva								
/// 			Qapf = quota unitaria £/m2 determ.da rapporto tra costi fissi attrib.a utenze non domest.e sup.tot.Ut.not Dom. corretta da coeffic.potenz.produzione (Kc)								
/// 											
/// 			Ctapf = costi fissi attribuili alle utenze NON domestiche								
/// 			Kc = coefficiente potenziale di produzione di rifiuto connesso al tipo di attiv. per aree geografiche e grandezza comuni (5000)								
/// 											
/// 			Qapf= Ctapf/SommatoriaSap*Kcap								
/// quindi il Qapf ( quota unitaria €/m2) risulta essere di:									
/// 	Qapf=Ctfund/Sommatoria Stot*Kc							Qapf	
/// 	
/// Il Calcolo  parte Variabile della Tariffa per  NON domestiche si ottiene come prodotto del costo unitario €/Kg per la superficie dell'utenza per il coefficiente di produzione per tipologia di attività (Kd)										
/// definizioni:									
/// 			TVnd(ap, Sap) = Cu * Sap (ap) * Kd(ap)									
/// 			TVnd = quota variabile della tariffa per un'utenza non domestica con tipologia di attività produttiva ap									
/// 												
/// 			Sap= superfice locali dove si svolge l'attivita' produttiva									
/// 			Cu = costo unitario (€/Kg). E' determinato dal rapporto tra costi variabili utenze non domestiche e quantità totale rifiuti non domestici									
/// 												
/// 			Kd = coefficiente potenziale di produzione in Kg /m2 anno che tiene conto della quantità  di rifiuti minima e massima per aree geografiche e grandezza comuni ( 5000)									
/// 											
/// quindi il Cu (costo unitario €/Kg) risulta essere di:										
/// 	Costi variabili ut.non dom./ qta rifiuti ut.non dom.					Cu					
/// 
/// Le possibili opzioni sono:
/// - Stampa
/// - Elimina
/// - Salva
/// - Calcola
/// - Estrai elenco immobili
/// - Cerca
/// </summary>
    public partial class Simulazione : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Simulazione));
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "TARES/TARI - Simulazione";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                LoadCombo();
                LoadK();
            } HideDiv("KA"); HideDiv("KB"); HideDiv("KC"); HideDiv("KD");
            string ElabInCorso = CacheManager.GetElaborazioneInCorso();
            if (ElabInCorso != "")
            {
                if (ElabInCorso.StartsWith("Elaborazione Finita"))
                {
                    string[] ListParam=ElabInCorso.Split(char.Parse("|"));
                    txtYear.Text=ListParam[1];
                    rddlTipoCalcolo.SelectedValue = ListParam[2];
                    CacheManager.RemoveElaborazioneInCorso();
                    LoadDatiSimulazione();
                }
                else
                {
                    ShowDiv("ElabProgress");
                    LblAvanzamento.Text = CacheManager.GetAvanzamentoElaborazione();
                }
            }
            else
            {
                HideDiv("ElabProgress");
                LblAvanzamento.Text = "";
            }
        }
        #region "Bottoni"
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            try
            {
                HideDiv("ElabProgress");
                //HideDiv("AggMassivo"); HideDiv("EsitoSimula");
                LoadDatiSimulazione();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdSearchClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdAnalyzeClick(object sender, EventArgs e)
        {
            try
            {
                AnalyzeData();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdAnalyzeClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdCalcoloClick(object sender, EventArgs e)
        {
            try
            {
                HideDiv("ElabProgress");
                CalcoloTariffe();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdCalcoloClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdSaveClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            string sScript = "<script language='javascript'>";
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            try
            {
                ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
                {
                    Ente = Ente,
                    FromVariabile = FromVariabile,
                    Anno = txtYear.Text,
                    TypeCalcolo = rddlTipoCalcolo.SelectedValue,
                    TypeMQ = "D"
                };
                DatiPEF parampef = LoadDatiPEF();
                Simula mySimula = new Simula { IdSimulazione = int.Parse(hfIdSimulazione.Value), ParamCalcolo = paramcalc, ParamPEF = parampef, DataConferma = DateTime.Now };
                if (mySimula.Save())
                    sScript += "alert('Salvataggio Tariffe effettuato con successo!');";
                else
                    sScript += "alert('Errore in salvataggio!');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "savetar", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdSaveClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdDeleteClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            string sScript = "<script language='javascript'>";
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            try
            {
                ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
                {
                    Ente = Ente,
                    FromVariabile = FromVariabile,
                    Anno = txtYear.Text,
                    TypeCalcolo = rddlTipoCalcolo.SelectedValue,
                    TypeMQ = "D"
                };
                DatiPEF parampef = LoadDatiPEF();
                Simula mySimula = new Simula { IdSimulazione = int.Parse(hfIdSimulazione.Value), ParamCalcolo = paramcalc, ParamPEF = parampef };
                if (mySimula.Delete())
                {
                    Session["SimulaArticoliAggMassivo"] = Session["SimulaTotali"] = Session["SimulaTariffe"] = Session["SimulaDefineTariffe"] = null;
                    sScript += "alert('Eliminazione Tariffe effettuato con successo!');";
                    LoadDatiSimulazione();
                }
                else
                    sScript += "alert('Errore in eliminazione!');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "deltar", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdDeleteClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdPrintClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            string filepath = string.Empty;

            DataSet ds = null;
            int MaxCol = 12;
            ExportToExcel myxls = new ExportToExcel();
            List<List<string>> rows = new List<List<string>>();
            List<string> cols = new List<string>();
            try
            {
                if (Session["SimulaDefineTariffe"] != null)
                {
                    //popolo le variabili con i dati della videata
                    ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
                    {
                        Ente = Ente,
                        FromVariabile = FromVariabile,
                        Anno = txtYear.Text,
                        TypeCalcolo = rddlTipoCalcolo.SelectedValue,
                        TypeMQ = "Dichiarate"
                    };
                    DatiPEF parampef = LoadDatiPEF();
                    List<ToDefineTariffe> listPFDom = new List<ToDefineTariffe>();
                    List<ToDefineTariffe> listPFNonDom = new List<ToDefineTariffe>();
                    List<ToDefineTariffe> listPVDom = new List<ToDefineTariffe>();
                    List<ToDefineTariffe> listPVNonDom = new List<ToDefineTariffe>();
                    List<ToDefineTariffe> listDefTariffe = new List<ToDefineTariffe>((ToDefineTariffe[])Session["SimulaDefineTariffe"]);
                    List<TotaliSimulazione> listTotali = new List<TotaliSimulazione>((TotaliSimulazione[])Session["SimulaTotali"]);
                    foreach (ToDefineTariffe item in listDefTariffe)
                    {
                        switch (item.Tipo)
                        {
                            case "PF_DOM":
                                listPFDom.Add(item);
                                break;
                            case "PF_NONDOM":
                                listPFNonDom.Add(item);
                                break;
                            case "PV_DOM":
                                listPVDom.Add(item);
                                break;
                            case "PV_NONDOM":
                                listPVNonDom.Add(item);
                                break;
                        }
                    }
                    //creo l'array di righe e colonne da stampare
                    cols.Add("Anno");
                    cols.Add(paramcalc.Anno);
                    cols.Add("");
                    cols.Add("Tipologia Superfici");
                    cols.Add(paramcalc.TypeMQ);
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Dati PEF");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("");
                    cols.Add("N.Utenze");
                    cols.Add("% Utenze");
                    cols.Add("Rifiuti prodotti (Kg)");
                    cols.Add("Costi Parte Fissa");
                    cols.Add("Costi Parte Variabile");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Domestiche");
                    cols.Add(parampef.NUtenzeDOM.ToString());
                    cols.Add(parampef.PercUtenzeDOM.ToString());
                    cols.Add(parampef.KGRifiutiDOM.ToString());
                    cols.Add(parampef.CostiPFDOM.ToString());
                    cols.Add(parampef.CostiPVDOM.ToString());
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Non Domestiche");
                    cols.Add(parampef.NUtenzeNONDOM.ToString());
                    cols.Add(parampef.PercUtenzeNONDOM.ToString());
                    cols.Add(parampef.KGRifiutiNONDOM.ToString());
                    cols.Add(parampef.CostiPFNONDOM.ToString());
                    cols.Add(parampef.CostiPVNONDOM.ToString());
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Parte Fissa");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("Parte Variabile");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Categoria (NC)");
                    cols.Add("MQ");
                    cols.Add("Coefficiente");
                    cols.Add("MQ Normalizzati");
                    cols.Add("Tariffa");
                    cols.Add("");
                    cols.Add("Categoria (NC)");
                    cols.Add("Utenti");
                    cols.Add("Coefficiente");
                    cols.Add("Utenti Normalizzati");
                    cols.Add("Tariffa");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    foreach (ToDefineTariffe pf in listPFDom)
                    {
                        cols = new List<string>();
                        cols.Add(pf.Descrizione);
                        cols.Add(pf.MQ.ToString());
                        cols.Add(pf.CoeffK.ToString());
                        cols.Add(pf.MQNormalizzati.ToString());
                        cols.Add(pf.Tariffa.ToString());
                        cols.Add("");
                        foreach (ToDefineTariffe pv in listPVDom)
                        {
                            if (pf.NC == pv.NC)
                            {
                                cols.Add(pv.Descrizione);
                                cols.Add(pv.MQ.ToString());
                                cols.Add(pv.CoeffK.ToString());
                                cols.Add(pv.MQNormalizzati.ToString());
                                cols.Add(pv.Tariffa.ToString());
                                break;
                            }
                        }
                        for (int x = cols.Count; x < MaxCol; x++)
                            cols.Add("");
                        rows.Add(cols);
                    }
                    //
                    cols = new List<string>();
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Parte Variabile");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("");
                    cols.Add("Parte Variabile");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Categoria");
                    cols.Add("MQ");
                    cols.Add("Coefficiente");
                    cols.Add("MQ Normalizzati");
                    cols.Add("Tariffa");
                    cols.Add("");
                    cols.Add("Categoria");
                    cols.Add("MQ");
                    cols.Add("Coefficiente");
                    cols.Add("MQ Normalizzati");
                    cols.Add("Tariffa");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    foreach (ToDefineTariffe pf in listPFNonDom)
                    {
                        cols = new List<string>();
                        cols.Add(pf.Descrizione);
                        cols.Add(pf.MQ.ToString());
                        cols.Add(pf.CoeffK.ToString());
                        cols.Add(pf.MQNormalizzati.ToString());
                        cols.Add(pf.Tariffa.ToString());
                        cols.Add("");
                        foreach (ToDefineTariffe pv in listPVNonDom)
                        {
                            if (pf.IdCategoria == pv.IdCategoria)
                            {
                                cols.Add(pv.Descrizione);
                                cols.Add(pv.MQ.ToString());
                                cols.Add(pv.CoeffK.ToString());
                                cols.Add(pv.MQNormalizzati.ToString());
                                cols.Add(pv.Tariffa.ToString());
                                break;
                            }
                        }
                        for (int x = cols.Count; x < MaxCol; x++)
                            cols.Add("");
                        rows.Add(cols);
                    }
                    //
                    cols = new List<string>();
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Totalizzatori");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    cols = new List<string>();
                    cols.Add("Categoria");
                    cols.Add("N.Componenti Fissa");
                    cols.Add("N.Componenti Variabile");
                    cols.Add("Riduzione");
                    cols.Add("Detassazione");
                    cols.Add("N.Utenti");
                    cols.Add("MQ");
                    cols.Add("N.Utenti Netti");
                    cols.Add("MQ Netti Fissa");
                    cols.Add("MQ Netti Variabile");
                    for (int x = cols.Count; x < MaxCol; x++)
                        cols.Add("");
                    rows.Add(cols);
                    //
                    foreach (TotaliSimulazione tot in listTotali)
                    {
                        cols = new List<string>();
                        cols.Add(tot.DescrCategoria);
                        cols.Add(tot.nComponentiPF.ToString());
                        cols.Add(tot.nComponentiPV.ToString());
                        cols.Add(tot.DescrRiduzione);
                        cols.Add(tot.DescrDetassazione);
                        cols.Add(tot.nUtenze.ToString());
                        cols.Add(tot.nMQ.ToString());
                        cols.Add(tot.UtenzeUtili.ToString());
                        cols.Add(tot.MQUtiliPF.ToString());
                        cols.Add(tot.MQUtiliPV.ToString());
                        for (int x = cols.Count; x < MaxCol; x++)
                            cols.Add("");
                        rows.Add(cols);
                    }

                    ds = myxls.CreateDataSet(rows, MaxCol);
                    if (ds != null)
                    {
                        filepath = Server.MapPath(@".\ExcelTemplate.xls");
                        myxls.ExportDataSetToExcel(ds, filepath);
                    }
                }
            }
            catch (System.Threading.ThreadAbortException er)
            {
                Global.Log.Write2(LogSeverity.Information, "End ExportDataSetToExcel "+er.Message);
            }
            catch (Exception er)
            {
                Global.Log.Write2(LogSeverity.Critical, er);
            }
        }
        protected void CmdSaveKAClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            try
            {
                string sScript = "<script language='javascript'>"; ;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                bool HasErr = false;
                foreach (GridViewRow myRow in rgvKA.Rows)
                {
                    CoefficienteKA myK = new CoefficienteKA();
                    myK.FromVariabile = FromVariabile;
                    myK.Ente = Ente;
                    myK.IdUsato = int.Parse(((HiddenField)myRow.Cells[5].FindControl("hfId")).Value.ToString());
                    myK.CoefficienteUsato = double.Parse(((TextBox)myRow.Cells[5].FindControl("txtValore")).Text);
                    if (!myK.Save())
                        HasErr = true;
                }
                int idSimula = 0;
                int.TryParse(hfIdSimulazione.Value, out idSimula);
                CoefficienteKA myKA = new CoefficienteKA { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KA"] = myKA.LoadAll();
                LoadSearch("KA");
                HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                HideDiv("KA"); HideDiv("KB"); HideDiv("KC"); HideDiv("KD");
                if (!HasErr)
                    sScript += "alert('Salvataggio effettuato con successo!');";
                else
                    sScript += "alert('Errore in salvataggio!');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "saveka", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdSaveKAClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdSaveKBClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            try
            {
                string sScript = "<script language='javascript'>"; ;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                bool HasErr = false;
                foreach (GridViewRow myRow in rgvKB.Rows)
                {
                    CoefficienteKB myK = new CoefficienteKB();
                    myK.FromVariabile = FromVariabile;
                    myK.Ente = Ente;
                    myK.IdUsato = int.Parse(((HiddenField)myRow.Cells[5].FindControl("hfId")).Value.ToString());
                    myK.CoefficienteUsato = double.Parse(((TextBox)myRow.Cells[5].FindControl("txtValore")).Text);
                    if (!myK.Save())
                        HasErr = true;
                }
                int idSimula = 0;
                int.TryParse(hfIdSimulazione.Value, out idSimula);
                CoefficienteKB myKB = new CoefficienteKB { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KB"] = myKB.LoadAll();
                LoadSearch("KB");
                HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                HideDiv("KA"); HideDiv("KB"); HideDiv("KC"); HideDiv("KD");
                if (!HasErr)
                    sScript += "alert('Salvataggio effettuato con successo!');";
                else
                    sScript += "alert('Errore in salvataggio!');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "saveKB", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdSaveKBClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdSaveKCClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            try
            {
                string sScript = "<script language='javascript'>"; ;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                bool HasErr = false;
                foreach (GridViewRow myRow in rgvKC.Rows)
                {
                    CoefficienteKC myK = new CoefficienteKC();
                    myK.FromVariabile = FromVariabile;
                    myK.Ente = Ente;
                    myK.IdUsato = int.Parse(((HiddenField)myRow.Cells[7].FindControl("hfId")).Value.ToString());
                    myK.CoefficienteUsato = double.Parse(((TextBox)myRow.Cells[7].FindControl("txtValore")).Text);
                    if (!myK.Save())
                        HasErr = true;
                }
                int idSimula = 0;
                int.TryParse(hfIdSimulazione.Value, out idSimula);
                CoefficienteKC myKC = new CoefficienteKC { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KC"] = myKC.LoadAll();
                LoadSearch("KC");
                HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                HideDiv("KA"); HideDiv("KB"); HideDiv("KC"); HideDiv("KD");
                if (!HasErr)
                    sScript += "alert('Salvataggio effettuato con successo!');";
                else
                    sScript += "alert('Errore in salvataggio!');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "saveKC", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdSaveKCClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdSaveKDClick(object sender, EventArgs e)
        {
            HideDiv("ElabProgress");
            try
            {
                string sScript = "<script language='javascript'>"; ;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                bool HasErr = false;
                foreach (GridViewRow myRow in rgvKD.Rows)
                {
                    CoefficienteKD myK = new CoefficienteKD();
                    myK.FromVariabile = FromVariabile;
                    myK.Ente = Ente;
                    myK.IdUsato = int.Parse(((HiddenField)myRow.Cells[7].FindControl("hfId")).Value.ToString());
                    myK.CoefficienteUsato = double.Parse(((TextBox)myRow.Cells[7].FindControl("txtValore")).Text);
                    if (!myK.Save())
                        HasErr = true;
                }
                int idSimula = 0;
                int.TryParse(hfIdSimulazione.Value, out idSimula);
                CoefficienteKD myKD = new CoefficienteKD { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KD"] = myKD.LoadAll();
                LoadSearch("KD");
                HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                HideDiv("KA"); HideDiv("KB"); HideDiv("KC"); HideDiv("KD");
                if (!HasErr)
                    sScript += "alert('Salvataggio effettuato con successo!');";
                else
                    sScript += "alert('Errore in salvataggio!');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "saveKD", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CmdSaveKDClick.errore::", ex);;
                throw ex;
            }
        }
        #endregion
        #region "DropDownList"
        private void LoadCombo()
        {
            try
            {
                ParametriCalcoloEnte myType = new ParametriCalcoloEnte { Ente = Ente };
                rddlTipoCalcolo.DataSource = myType.LoadTypeCalcolo();
                rddlTipoCalcolo.DataValueField = "TypeCalcolo";
                rddlTipoCalcolo.DataTextField = "TypeCalcolo";
                rddlTipoCalcolo.DataBind();
                rddlTipoCalcolo.SelectedValue = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRuolo.TipoCalcolo.TARES;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadCombo.errore::", ex);;
                throw;
            }
        }
        #endregion
        #region "Griglie"
        protected void rgvKAPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearch("KA", e.NewPageIndex);
        }
        protected void rgvKBPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearch("KB", e.NewPageIndex);
        }
        protected void rgvKCPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearch("KC", e.NewPageIndex);
        }
        protected void rgvKDPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearch("KD", e.NewPageIndex);
        }
        protected void rgvTotaliPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearch("Totali", e.NewPageIndex);
        }
        protected void rgvTariffePageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearch("Tariffe", e.NewPageIndex);
        }
        private void LoadSearch(string GrdOrigin, int? page = 0)
        {
            try
            {
                switch (GrdOrigin)
                {
                    case "Totali":
                        if (Session["SimulaTotali"] != null)
                        {
                            List<TotaliSimulazione> listTotali = new List<TotaliSimulazione>((TotaliSimulazione[])Session["SimulaTotali"]);
                            rgvTotali.DataSource = listTotali;
                            if (page.HasValue)
                                rgvTotali.PageIndex = page.Value;
                            rgvTotali.DataBind();
                        }
                        break;
                    case "Tariffe":
                        if (Session["SimulaTariffe"] != null)
                        {
                            List<TariffeEnte> listTariffe = new List<TariffeEnte>((TariffeEnte[])Session["SimulaTariffe"]);
                            rgvTariffe.DataSource = listTariffe;
                            if (page.HasValue)
                                rgvTariffe.PageIndex = page.Value;
                            rgvTariffe.DataBind();
                        }
                        break;
                    case "DefineTariffe":
                        LoadDefineTariffe("");
                        break;
                    case "KA":
                        List<CoefficienteKA> listKA = new List<CoefficienteKA>((CoefficienteKA[])Session["KA"]);
                        rgvKA.DataSource = listKA;
                        if (page.HasValue)
                            rgvKA.PageIndex = page.Value;
                        rgvKA.DataBind();
                        break;
                    case "KB":
                        List<CoefficienteKB> listKB = new List<CoefficienteKB>((CoefficienteKB[])Session["KB"]);
                        rgvKB.DataSource = listKB;
                        if (page.HasValue)
                            rgvKB.PageIndex = page.Value;
                        rgvKB.DataBind();
                        break;
                    case "KC":
                        List<CoefficienteKC> listKC = new List<CoefficienteKC>((CoefficienteKC[])Session["KC"]);
                        rgvKC.DataSource = listKC;
                        if (page.HasValue)
                            rgvKC.PageIndex = page.Value;
                        rgvKC.DataBind();
                        break;
                    case "KD":
                        List<CoefficienteKD> listKD = new List<CoefficienteKD>((CoefficienteKD[])Session["KD"]);
                        rgvKD.DataSource = listKD;
                        if (page.HasValue)
                            rgvKD.PageIndex = page.Value;
                        rgvKD.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadSearch.errore::", ex);;
                throw;
            }
        }
        #endregion
        #region "Load Dati"
        private void LoadDatiSimulazione(int? page = 0)
        {
            string sScript = "<script language='javascript'>";
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            try
            {
                ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
                {
                    Ente = Ente,
                    FromVariabile = FromVariabile,
                    Anno = txtYear.Text,
                    TypeCalcolo = rddlTipoCalcolo.SelectedValue,
                    TypeMQ = "D"
                };
                Simula mySimula = new Simula { ParamCalcolo = paramcalc };
                if (mySimula.Load())
                {
                    hfIdSimulazione.Value = mySimula.IdSimulazione.ToString();
                    //non ho ancora mai lavorato quindi ricarico da dati form
                    if (mySimula.ListArticoli == null)
                    {
                        mySimula.ParamCalcolo = paramcalc;
                    }
                    else
                    {
                        frAggMassivo.Attributes.Add("src", "../AggMassivo/AggMassivo.aspx?IdSimulazione=" + hfIdSimulazione.Value);
                    }
                    LoadParamCalcolo(mySimula.ParamCalcolo);
                    LoadPEF(mySimula.ParamPEF);

                    Session["SimulaArticoliAggMassivo"] = mySimula.ListArticoli;
                    Session["SimulaTotali"] = mySimula.ListTotali;
                    Session["SimulaTariffe"] = mySimula.ListTariffe;
                    Session["SimulaDefineTariffe"] = mySimula.ListDefineTariffe;

                    if (mySimula.ListTotali != null)
                    {
                        ShowDiv("SimulaTariffe"); HideDiv("SimulaTotali"); HideDiv("AggMassivo");
                        sScript += "document.getElementById('aVisTariffe').title = 'Nascondi Tariffe';";
                        sScript += "document.getElementById('aVisTariffe').innerText = 'Nascondi Tariffe';";
                        sScript += "</script>";
                        sBuilder.Append(sScript);
                        ClientScript.RegisterStartupScript(this.GetType(), "vistar", sBuilder.ToString());

                        LoadSearch("Totali", 0);
                        LoadSearch("Tariffe", 0);
                        LoadSearch("DefineTariffe", 0);
                    }
                    else
                    {
                        HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                    }
                    HideDiv("ElabProgress");
                    if (mySimula.DataConferma.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                    {
                        cmdAnalyze.Enabled = false;
                        cmdCalc.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadDatiSimulazione.errore::", ex);;
                throw;
            }
        }
        private void LoadParamCalcolo(ParametriCalcoloEnte myItem)
        {
            try
            {
                txtYear.Text = myItem.Anno;
                try
                {
                    rddlTipoCalcolo.SelectedValue = myItem.TypeCalcolo;
                }
                catch (Exception ex)
                {
                    Log.Debug("OPENgov.20.Simulazione.LoadParamCalcolo.errore::", ex);;
                }
                if (myItem.TypeMQ == "D")
                {
                    optDic.Checked = true; optCat.Checked = false;
                }
                else
                {
                    optCat.Checked = true; optDic.Checked = false;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadParamCalcolo.errore::", ex);;
                throw;
            }
        }
        private void LoadPEF(DatiPEF myItem)
        {
            try
            {
                TxtNUtenzeTot.Text = myItem.NUtenze.ToString();
                TxtNUtenzeDom.Text = myItem.NUtenzeDOM.ToString();
                TxtNUtenzeNonDom.Text = myItem.NUtenzeNONDOM.ToString();
                TxtPercentUtenzeTot.Text = myItem.PercUtenze.ToString();
                TxtPercentUtenzeDom.Text = myItem.PercUtenzeDOM.ToString();
                TxtPercentUtenzeNonDom.Text = myItem.PercUtenzeNONDOM.ToString();

                TxtMQTot.Text = myItem.MQ.ToString();
                TxtMQDom.Text = myItem.MQDOM.ToString();
                TxtMQNonDom.Text = myItem.MQNONDOM.ToString();
                TxtPercentMQTot.Text = myItem.PercMQ.ToString();
                TxtPercentMQDom.Text = myItem.PercMQDOM.ToString();
                TxtPercentMQNonDom.Text = myItem.PercMQNONDOM.ToString();

                TxtKgTot.Text = myItem.KGRifiuti.ToString();
                TxtKgDom.Text = myItem.KGRifiutiDOM.ToString();
                TxtKgNonDom.Text = myItem.KGRifiutiNONDOM.ToString();
                TxtPercentKGTot.Text = myItem.PercKG.ToString();
                TxtPercentKGDom.Text = myItem.PercKGDOM.ToString();
                TxtPercentKGNonDom.Text = myItem.PercKGNONDOM.ToString();

                TxtCostiPFTot.Text = myItem.CostiPF.ToString();
                TxtCostiPFDom.Text = myItem.CostiPFDOM.ToString();
                TxtCostiPFNonDom.Text = myItem.CostiPFNONDOM.ToString();

                TxtCostiPVTot.Text = myItem.CostiPV.ToString();
                TxtCostiPVDom.Text = myItem.CostiPVDOM.ToString();
                TxtCostiPVNonDom.Text = myItem.CostiPVNONDOM.ToString();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadPEF.errore::", ex);;
                throw;
            }
        }
        private void LoadDefineTariffe(string table, int? page = 0)
        {
            List<ToDefineTariffe> listPFDom = new List<ToDefineTariffe>();
            List<ToDefineTariffe> listPFNonDom = new List<ToDefineTariffe>();
            List<ToDefineTariffe> listPVDom = new List<ToDefineTariffe>();
            List<ToDefineTariffe> listPVNonDom = new List<ToDefineTariffe>();
            try
            {
                if (Session["SimulaDefineTariffe"] != null)
                {
                    List<ToDefineTariffe> listDefTariffe = new List<ToDefineTariffe>((ToDefineTariffe[])Session["SimulaDefineTariffe"]);
                    foreach (ToDefineTariffe item in listDefTariffe)
                    {
                        if (item.Tipo == table.ToString() || table.ToString() == "")
                        {
                            switch (item.Tipo)
                            {
                                case "PF_DOM":
                                    listPFDom.Add(item);
                                    break;
                                case "PF_NONDOM":
                                    listPFNonDom.Add(item);
                                    break;
                                case "PV_DOM":
                                    listPVDom.Add(item);
                                    break;
                                case "PV_NONDOM":
                                    listPVNonDom.Add(item);
                                    break;
                            }
                        }
                    }
                    rgvPFDOM.DataSource = listPFDom;
                    if (page.HasValue)
                        rgvPFDOM.PageIndex = page.Value;
                    rgvPFDOM.DataBind();
                    rgvPFNONDOM.DataSource = listPFNonDom;
                    if (page.HasValue)
                        rgvPFNONDOM.PageIndex = page.Value;
                    rgvPFNONDOM.DataBind();
                    rgvPVDOM.DataSource = listPVDom;
                    if (page.HasValue)
                        rgvPVDOM.PageIndex = page.Value;
                    rgvPVDOM.DataBind();
                    rgvPVNONDOM.DataSource = listPVNonDom;
                    if (page.HasValue)
                        rgvPVNONDOM.PageIndex = page.Value;
                    rgvPVNONDOM.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadDefineTariffe.errore::", ex);;
                throw;
            }
        }
        private void LoadK()
        {
            try
            {
                int idSimula = 0;
                int.TryParse(hfIdSimulazione.Value, out idSimula);
                CoefficienteKA myKA = new CoefficienteKA { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KA"] = myKA.LoadAll();
                LoadSearch("KA");
                CoefficienteKB myKB = new CoefficienteKB { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KB"] = myKB.LoadAll();
                LoadSearch("KB");
                CoefficienteKC myKC = new CoefficienteKC { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KC"] = myKC.LoadAll();
                LoadSearch("KC");
                CoefficienteKD myKD = new CoefficienteKD { FromVariabile = FromVariabile, Ente = Ente, IdSimulazione = idSimula };
                Session["KD"] = myKD.LoadAll();
                LoadSearch("KD");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadK.errore::", ex);;
                throw;
            }
        }
        #endregion
        #region "Calcoli"
        private void AnalyzeData()
        {
            try
            {
                Global.Log.Write2(LogSeverity.Debug, "entro analyze");
                string ElabInCorso = CacheManager.GetElaborazioneInCorso();
                if (ElabInCorso == "")
                {
                    ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
                    {
                        Ente = Ente,
                        FromVariabile = FromVariabile,
                        Anno = txtYear.Text,
                        TypeCalcolo = rddlTipoCalcolo.SelectedValue,
                        TypeMQ = "D"
                    };
                    DatiPEF parampef = LoadDatiPEF();
                    Simula mySimula = new Simula { IdSimulazione = int.Parse(hfIdSimulazione.Value), ParamCalcolo = paramcalc, ParamPEF = parampef };
                    mySimula.StartAnalyze();
                    HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                    /*if (mySimula.Analyze())
                    {
                        LoadDatiSimulazione();
                    }
                    else
                    {
                        mySimula.Delete();
                        HideDiv("AggMassivo"); HideDiv("SimulaTotali"); HideDiv("SimulaTariffe");
                    }*/
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.AnalyzeData.errore::", ex);;
                throw;
            }
        }
        private void CalcoloTariffe()
        {
            try
            {
                ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
                {
                    Ente = Ente,
                    FromVariabile = FromVariabile,
                    Anno = txtYear.Text,
                    TypeCalcolo = rddlTipoCalcolo.SelectedValue,
                    TypeMQ = "D"
                };
                DatiPEF parampef = LoadDatiPEF();
                Simula mySimula = new Simula { IdSimulazione = int.Parse(hfIdSimulazione.Value), ParamCalcolo = paramcalc, ParamPEF = parampef };
                if (mySimula.DefineTariffe())
                {
                    LoadDatiSimulazione();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.CalcoloTariffe.errore::", ex);;
                throw;
            }
        }
        #endregion
        protected void txtTotCalc(object sender, EventArgs e)
        {
            double myPercToCalc = 0;
            double myPercDifToCalc = 0;
            double myValTot = 0;
            //double myValPerc = 0;
            //double myValDif = 0;
            double ParteTot = 0;
            double ParteDom = 0;
            double ParteNonDom = 0;

            try
            {
                switch (((TextBox)sender).ID)
                {
                    case "TxtNUtenzeDom":
                    case "TxtNUtenzeNonDom":
                        if (TxtNUtenzeDom.Text != "")
                            double.TryParse(TxtNUtenzeDom.Text, out ParteDom);
                        if (TxtNUtenzeNonDom.Text != "")
                            double.TryParse(TxtNUtenzeNonDom.Text, out ParteNonDom);
                        ParteTot = (ParteDom + ParteNonDom);
                        TxtNUtenzeTot.Text = ParteTot.ToString();
                        TxtPercentUtenzeDom.Text = string.Format("{0:0.000000}", ((ParteDom / ParteTot) * 100));
                        TxtPercentUtenzeNonDom.Text = string.Format("{0:0.000000}", ((ParteNonDom / ParteTot) * 100));
                        break;
                    case "TxtMQDom":
                    case "TxtMQNonDom":
                        if (TxtMQDom.Text != "")
                            double.TryParse(TxtMQDom.Text, out ParteDom);
                        if (TxtMQNonDom.Text != "")
                            double.TryParse(TxtMQNonDom.Text, out ParteNonDom);
                        ParteTot = (ParteDom + ParteNonDom);
                        TxtMQTot.Text = ParteTot.ToString();
                        TxtPercentMQDom.Text = string.Format("{0:0.000000}", ((ParteDom / ParteTot) * 100));
                        TxtPercentMQNonDom.Text = string.Format("{0:0.000000}", ((ParteNonDom / ParteTot) * 100));
                        break;
                    case "TxtPercentUtenzeDom":
                        if (TxtPercentUtenzeDom.Text != "")
                            double.TryParse(TxtPercentUtenzeDom.Text, out myPercToCalc);
                        TxtPercentUtenzeNonDom.Text = string.Format("{0:0.000000}", 100 - myPercToCalc);
                        break;
                    case "TxtPercentUtenzeNonDom":
                        if (TxtPercentUtenzeNonDom.Text != "")
                            double.TryParse(TxtPercentUtenzeNonDom.Text, out myPercToCalc);
                        TxtPercentUtenzeDom.Text = string.Format("{0:0.000000}", 100 - myPercToCalc);
                        break;
                    case "TxtPercentKGDom":
                        if (TxtPercentKGDom.Text != "")
                            double.TryParse(TxtPercentKGDom.Text, out myPercToCalc);
                        TxtPercentKGNonDom.Text = string.Format("{0:0.000000}", 100 - myPercToCalc);
                        break;
                    case "TxtPercentKGNonDom":
                        if (TxtPercentKGNonDom.Text != "")
                            double.TryParse(TxtPercentKGNonDom.Text, out myPercToCalc);
                        TxtPercentKGDom.Text = string.Format("{0:0.000000}", 100 - myPercToCalc);
                        break;
                    default:
                        break;
                }

                //*** KG ***
                myPercToCalc = myPercDifToCalc = 0;
                if (TxtPercentKGDom.Text != "")
                    double.TryParse(TxtPercentKGDom.Text, out myPercToCalc);

                if (TxtPercentKGNonDom.Text != "")
                    double.TryParse(TxtPercentKGNonDom.Text, out myPercDifToCalc);
                else
                    myPercDifToCalc = 100 - myPercToCalc;
                if (TxtKgTot.Text != "")
                    double.TryParse(TxtKgTot.Text, out myValTot);
                //if (TxtKgDom.Text != "")
                //    double.TryParse(TxtKgDom.Text, out myValPerc);
                //else if (TxtKgNonDom.Text != "")
                //    double.TryParse(TxtKgNonDom.Text, out myValDif);

                if (myValTot > 0)
                {
                    TxtKgDom.Text = ((myValTot * myPercToCalc) / 100).ToString();
                    TxtKgNonDom.Text = (myValTot - ((myValTot * myPercToCalc) / 100)).ToString();
                }
                //*** ***

                //*** COSTI FISSI si divido sulle % delle Utenze***
                myPercToCalc = myPercDifToCalc = 0;
                    if (TxtPercentUtenzeDom.Text != "")
                        double.TryParse(TxtPercentUtenzeDom.Text, out myPercToCalc);

                    if (TxtPercentUtenzeNonDom.Text != "")
                        double.TryParse(TxtPercentUtenzeNonDom.Text, out myPercDifToCalc);
                    else
                        myPercDifToCalc = 100 - myPercToCalc;

                if (TxtCostiPFTot.Text != "")
                    double.TryParse(TxtCostiPFTot.Text, out myValTot);
                //if (TxtCostiPFDom.Text != "")
                //    double.TryParse(TxtCostiPFDom.Text, out myValPerc);
                //else if (TxtCostiPFNonDom.Text != "")
                //    double.TryParse(TxtCostiPFNonDom.Text, out myValDif);

                if (myValTot > 0)
                {
                    TxtCostiPFDom.Text = ((myValTot * myPercToCalc) / 100).ToString();
                    TxtCostiPFNonDom.Text = (myValTot - ((myValTot * myPercToCalc) / 100)).ToString();
                }
                //*** ***
                //*** COSTI VARIABILI si dividono sulle % dei KG***
                myPercToCalc = myPercDifToCalc = 0;
                if (TxtPercentKGDom.Text != "")
                    double.TryParse(TxtPercentKGDom.Text, out myPercToCalc);
                else
                    if (TxtPercentUtenzeDom.Text != "")
                    double.TryParse(TxtPercentUtenzeDom.Text, out myPercToCalc);

                if (TxtPercentKGNonDom.Text != "")
                    double.TryParse(TxtPercentKGNonDom.Text, out myPercDifToCalc);
                else
                    if (TxtPercentUtenzeNonDom.Text != "")
                    double.TryParse(TxtPercentUtenzeNonDom.Text, out myPercDifToCalc);
                else
                    myPercDifToCalc = 100 - myPercToCalc;

                if (TxtCostiPVTot.Text != "")
                    double.TryParse(TxtCostiPVTot.Text, out myValTot);
                //if (TxtCostiPVDom.Text != "")
                //    double.TryParse(TxtCostiPVDom.Text, out myValPerc);
                //else if (TxtCostiPVNonDom.Text != "")
                //    double.TryParse(TxtCostiPVNonDom.Text, out myValDif);

                if (myValTot > 0)
                {
                    TxtCostiPVDom.Text = ((myValTot * myPercToCalc) / 100).ToString();
                    TxtCostiPVNonDom.Text = (myValTot - ((myValTot * myPercToCalc) / 100)).ToString();
                }
                //*** ***                
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.txtTotCalc.errore::", ex);;
                throw ex;
            }
            finally
            {
                if (Session["SimulaTotali"] != null)
                    ShowDiv("SimulaTariffe");
                HideDiv("SimulaTotali"); HideDiv("AggMassivo");
                string sScript = "<script language='javascript'>";
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript += "document.getElementById('aVisTariffe').title = 'Nascondi Tariffe';";
                sScript += "document.getElementById('aVisTariffe').innerText = 'Nascondi Tariffe';";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "vistar", sBuilder.ToString());
            }
        }

        private DatiPEF LoadDatiPEF()
        {
            try
            {
                DatiPEF paramPEF = new DatiPEF
                {
                    FromVariabile = FromVariabile,

                    NUtenze = int.Parse(TxtNUtenzeTot.Text),
                    NUtenzeDOM = int.Parse(TxtNUtenzeDom.Text),
                    NUtenzeNONDOM = int.Parse(TxtNUtenzeNonDom.Text),
                    PercUtenze = double.Parse(TxtPercentUtenzeTot.Text),
                    PercUtenzeDOM = double.Parse(TxtPercentUtenzeDom.Text),
                    PercUtenzeNONDOM = double.Parse(TxtPercentUtenzeNonDom.Text),

                    MQ = int.Parse(TxtMQTot.Text),
                    MQDOM = int.Parse(TxtMQDom.Text),
                    MQNONDOM = int.Parse(TxtMQNonDom.Text),
                    PercMQ = double.Parse(TxtPercentMQTot.Text),
                    PercMQDOM = double.Parse(TxtPercentMQDom.Text),
                    PercMQNONDOM = double.Parse(TxtPercentMQNonDom.Text),

                    KGRifiuti = double.Parse(TxtKgTot.Text),
                    KGRifiutiDOM = double.Parse(TxtKgDom.Text),
                    KGRifiutiNONDOM = double.Parse(TxtKgNonDom.Text),
                    PercKG = double.Parse(TxtPercentKGTot.Text),
                    PercKGDOM = double.Parse(TxtPercentKGDom.Text),
                    PercKGNONDOM = double.Parse(TxtPercentKGNonDom.Text),

                    CostiPF = double.Parse(TxtCostiPFTot.Text),
                    CostiPFDOM = double.Parse(TxtCostiPFDom.Text),
                    CostiPFNONDOM = double.Parse(TxtCostiPFNonDom.Text),

                    CostiPV = double.Parse(TxtCostiPVTot.Text),
                    CostiPVDOM = double.Parse(TxtCostiPVDom.Text),
                    CostiPVNONDOM = double.Parse(TxtCostiPVNonDom.Text)
                };

                return paramPEF;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Simulazione.LoadDatiPEF.errore::", ex);;
                throw;
            }
        }
    }
}