using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using System.Data;
using log4net;

namespace OPENgov.Acquisizioni.TARES.AggMassivo
{/// <summary>
/// Pagina per la gestione degli aggiornamenti massivi sulle dichiarazioni.
/// Le possibili opzioni sono:
/// - Stampa
/// - Salva
/// - Cerca
/// </summary>
    public partial class AggMassivo : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AggMassivo));
        protected FunctionGrd FncGrd = new FunctionGrd();
        int idSimula = 0;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "TARES/TARI - Aggiornamento Massivo";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();");
                LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" + Ente + "','"+ UrlPopStradario +"','"+ StileStradario +"')");
                LoadCombo();
                LoadRidDet();
                LoadParamSearch();
                HideDiv("divRidDet");
                if (Request.QueryString["IdSimulazione"] != null)
                {
                    int.TryParse(Request.QueryString["IdSimulazione"].ToString(), out idSimula);
                    if (idSimula > 0)
                    {
                        LoadSearch();
                        HideDiv("ParamSearch"); HideDiv("ParamAgg");
                    }
                }
            }
            ManageBottoniera();
            SelRow();
        }
        #region "Bottoni"
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            Search();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
        /// <revisionHistory><revision date = "26/05/2020">Per le riduzioni/detassazioni la regola è aggiungo la riduzione selezionata tra i parametri di aggiornamento, tranne nel caso in cui abbia selezionato <em>Elimina Riduzione</em>; in quel caso elimino la riduzione selezionata tra i parametri di ricerca</revision></revisionHistory>

        protected void CmdSaveClick(object sender, EventArgs e)
        {
            string sScript = "<script language='javascript'>";
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myRidEse = null;

            try
            {
                if (txtAggDal.Text == "")
                    sScript += "alert('Inserire la data di aggiornamento!')";
                else
                {
                    //ciclo per salvare
                    foreach (Articolo myItem in ((Articolo[])Session["SimulaArticoliAggMassivo"]))
                    {
                        if (myItem.IsSel)
                        {
                            MyArrayRid.Clear(); MyArrayDet.Clear();
                            Articolo myArticolo = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, IdSimulazione = idSimula, Id = myItem.Id };
                            myArticolo = myItem;
                            myArticolo.FromVariabile = FromVariabile;
                            //memorizzo i dati pre aggiornamento
                            myArticolo.Partita.tDataInizio = DateTime.Parse(txtAggDal.Text);
                            myArticolo.IdCatAtecoOld = myArticolo.Partita.IdCatAteco;
                            myArticolo.nComponentiPFOld = myArticolo.Partita.nNComponenti;
                            myArticolo.nComponentiPVOld = myArticolo.Partita.nComponentiPV;
                            if (rddlAggCat.SelectedValue != "")
                                myArticolo.Partita.IdCatAteco = int.Parse(rddlAggCat.SelectedValue);
                            if (txtAggNCFissa.Text != "")
                                myArticolo.Partita.nNComponenti = int.Parse(txtAggNCFissa.Text);
                            if (txtAggNCVariabile.Text != "")
                                myArticolo.Partita.nComponentiPV = int.Parse(txtAggNCVariabile.Text);
                            if (rddlAggStatoOccupazione.SelectedValue != "" && rddlAggStatoOccupazione.SelectedValue != "-1")
                                myArticolo.Partita.sIdStatoOccupazione = rddlAggStatoOccupazione.SelectedValue;
                            if (chkAggEsente.Checked)
                                myArticolo.Partita.nMQTassabili = 0;
                            if (rddlAggRid.SelectedValue != "")
                            {
                                myRidEse = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                                myRidEse.sCodice = rddlAggRid.SelectedValue;
                                if (myRidEse.sCodice != string.Empty && myRidEse.sCodice != RidEseEnte.DELETERidEse)
                                {
                                    if (myArticolo.Partita.oRiduzioni != null)
                                    {
                                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myItemRidEse in myArticolo.Partita.oRiduzioni)
                                        {
                                            MyArrayRid.Add(myItemRidEse);
                                        }
                                    }
                                    MyArrayRid.Add(myRidEse);
                                }
                                else
                                {
                                    if (myArticolo.Partita.oRiduzioni != null)
                                    {
                                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myItemRidEse in myArticolo.Partita.oRiduzioni)
                                        {
                                            if (myRidEse.sCodice != myItemRidEse.sCodice)
                                                MyArrayRid.Add(myItemRidEse);
                                        }
                                    }
                                }
                                //myArticolo.Partita.oRiduzioni = MyArrayRid.ToArray();
                            }
                            if (rddlAggDet.SelectedValue != "")
                            {
                                myRidEse = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                                myRidEse.sCodice = rddlAggDet.SelectedValue;
                                if (myRidEse.sCodice != string.Empty)
                                {
                                    MyArrayDet.Add(myRidEse);
                                    //myArticolo.Partita.oDetassazioni = MyArrayDet.ToArray();
                                }
                            }
                            else
                                myArticolo.Partita.oDetassazioni = null;
                            //salvo le modifiche apportate
                            if (!myArticolo.Save(MyArrayRid.ToArray(), MyArrayDet.ToArray()))
                            {
                                sScript += "alert('Errore in salvataggio!');";
                                break;
                            }
                        }
                    }
                }
                if (sScript.IndexOf("alert") <= 0)
                {
                    //nascondo la griglia
                    Session["SimulaArticoliAggMassivo"] = null;
                    HideDiv("SearchResult");
                    sScript += "alert('Salvataggio terminato con successo!');";
                }
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.CmdSaveClick.errore::", ex); ;
                throw ex;
            }
            finally
            {
                HideDiv("divRidDet");
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
                ManageBottoniera();
            }
        }
        //protected void CmdSaveClick(object sender, EventArgs e)
        //{
        //    string sScript = "<script language='javascript'>";
        //    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
        //    List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
        //    List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
        //    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myRidEse = null;

        //    try
        //    {
        //        if (txtAggDal.Text == "")
        //            sScript += "alert('Inserire la data di aggiornamento!')";
        //        else
        //        {
        //            //ciclo per salvare
        //            foreach (Articolo myItem in ((Articolo[])Session["SimulaArticoliAggMassivo"]))
        //            {
        //                if (myItem.IsSel)
        //                {
        //                    MyArrayRid.Clear(); MyArrayDet.Clear();
        //                    Articolo myArticolo = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, IdSimulazione = idSimula, Id = myItem.Id };
        //                    myArticolo = myItem;
        //                    myArticolo.FromVariabile = FromVariabile;
        //                    //memorizzo i dati pre aggiornamento
        //                    myArticolo.Partita.tDataInizio = DateTime.Parse(txtAggDal.Text);
        //                    myArticolo.IdCatAtecoOld = myArticolo.Partita.IdCatAteco;
        //                    myArticolo.nComponentiPFOld = myArticolo.Partita.nNComponenti;
        //                    myArticolo.nComponentiPVOld = myArticolo.Partita.nComponentiPV;
        //                    if (rddlAggCat.SelectedValue != "")
        //                        myArticolo.Partita.IdCatAteco = int.Parse(rddlAggCat.SelectedValue);
        //                    if (txtAggNCFissa.Text != "")
        //                        myArticolo.Partita.nNComponenti = int.Parse(txtAggNCFissa.Text);
        //                    if (txtAggNCVariabile.Text != "")
        //                        myArticolo.Partita.nComponentiPV = int.Parse(txtAggNCVariabile.Text);
        //                    if (rddlAggStatoOccupazione.SelectedValue != "" && rddlAggStatoOccupazione.SelectedValue != "-1")
        //                        myArticolo.Partita.sIdStatoOccupazione = rddlAggStatoOccupazione.SelectedValue;
        //                    if (chkAggEsente.Checked)
        //                        myArticolo.Partita.nMQTassabili = 0;
        //                    if (rddlAggRid.SelectedValue != "")
        //                    {
        //                        myRidEse = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
        //                        myRidEse.sCodice = rddlAggRid.SelectedValue;
        //                        if (myRidEse.sCodice != string.Empty)
        //                        {
        //                            MyArrayRid.Add(myRidEse);
        //                            myArticolo.Partita.oRiduzioni = MyArrayRid.ToArray();
        //                        }
        //                    }
        //                    else
        //                        myArticolo.Partita.oRiduzioni = null;
        //                    if (rddlAggDet.SelectedValue != "")
        //                    {
        //                        myRidEse = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
        //                        myRidEse.sCodice = rddlAggDet.SelectedValue;
        //                        if (myRidEse.sCodice != string.Empty)
        //                        {
        //                            MyArrayDet.Add(myRidEse);
        //                            myArticolo.Partita.oDetassazioni = MyArrayDet.ToArray();
        //                        }
        //                    }
        //                    else
        //                        myArticolo.Partita.oDetassazioni = null;
        //                    //salvo le modifiche apportate
        //                    if (!myArticolo.Save())
        //                    {
        //                        sScript += "alert('Errore in salvataggio!');";
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        if (sScript.IndexOf("alert") <= 0)
        //        {
        //            //nascondo la griglia
        //            Session["SimulaArticoliAggMassivo"] = null;
        //            HideDiv("SearchResult");
        //            sScript += "alert('Salvataggio terminato con successo!');";
        //        }
        //        sScript += "</script>";
        //        sBuilder.Append(sScript);
        //        ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("OPENgov.20.AggMassivo.CmdSaveClick.errore::", ex);;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        HideDiv("divRidDet");
        //        if (idSimula > 0)
        //            HideDiv("ParamSearch"); HideDiv("ParamAgg");
        //        ManageBottoniera();
        //    }
        //}

        protected void CmdPrintClick(object sender, EventArgs e)
        {
            //string filepath = string.Empty;

            //DataSet ds = null;
            //int MaxCol = 12;
            //ExportToExcel myxls = new ExportToExcel();
            //List<List<string>> rows = new List<List<string>>();
            //List<string> cols = new List<string>();
            //try
            //{
            //    if (Session["SimulaDefineTariffe"] != null)
            //    {
            //        //popolo le variabili con i dati della videata
            //        ParametriCalcoloEnte paramcalc = new ParametriCalcoloEnte
            //        {
            //            Ente = Ente,
            //            FromVariabile = FromVariabile,
            //            Anno = txtYear.Text,
            //            TypeCalcolo = rddlTipoCalcolo.SelectedValue,
            //            TypeMQ = "Dichiarate"
            //        };
            //        DatiPEF parampef = new DatiPEF
            //        {
            //            FromVariabile = FromVariabile,
            //            NUtenze = int.Parse(TxtNUtenzeTot.Text),
            //            NUtenzeDOM = int.Parse(TxtNUtenzeDom.Text),
            //            NUtenzeNONDOM = int.Parse(TxtNUtenzeNonDom.Text),
            //            PercUtenze = double.Parse(TxtPercentUtenzeTot.Text),
            //            PercUtenzeDOM = double.Parse(TxtPercentUtenzeDom.Text),
            //            PercUtenzeNONDOM = double.Parse(TxtPercentUtenzeNonDom.Text),
            //            KGRifiuti = double.Parse(TxtKgTot.Text),
            //            KGRifiutiDOM = double.Parse(TxtKgDom.Text),
            //            KGRifiutiNONDOM = double.Parse(TxtKgNonDom.Text),
            //            CostiPF = double.Parse(TxtCostiPFTot.Text),
            //            CostiPFDOM = double.Parse(TxtCostiPFDom.Text),
            //            CostiPFNONDOM = double.Parse(TxtCostiPFNonDom.Text),
            //            CostiPV = double.Parse(TxtCostiPVTot.Text),
            //            CostiPVDOM = double.Parse(TxtCostiPVDom.Text),
            //            CostiPVNONDOM = double.Parse(TxtCostiPVNonDom.Text),
            //            IsKCMax = (optKCMax.Checked) ? true : false,
            //            IsKDMax = (optKDMax.Checked) ? true : false
            //        };
            //        List<ToDefineTariffe> listPFDom = new List<ToDefineTariffe>();
            //        List<ToDefineTariffe> listPFNonDom = new List<ToDefineTariffe>();
            //        List<ToDefineTariffe> listPVDom = new List<ToDefineTariffe>();
            //        List<ToDefineTariffe> listPVNonDom = new List<ToDefineTariffe>();
            //        List<ToDefineTariffe> listDefTariffe = new List<ToDefineTariffe>((ToDefineTariffe[])Session["SimulaDefineTariffe"]);
            //        List<TotaliSimulazione> listTotali = new List<TotaliSimulazione>((TotaliSimulazione[])Session["SimulaTotali"]);
            //        foreach (ToDefineTariffe item in listDefTariffe)
            //        {
            //            switch (item.Tipo)
            //            {
            //                case "PF_DOM":
            //                    listPFDom.Add(item);
            //                    break;
            //                case "PF_NONDOM":
            //                    listPFNonDom.Add(item);
            //                    break;
            //                case "PV_DOM":
            //                    listPVDom.Add(item);
            //                    break;
            //                case "PV_NONDOM":
            //                    listPVNonDom.Add(item);
            //                    break;
            //            }
            //        }
            //        //creo l'array di righe e colonne da stampare
            //        cols.Add("Anno");
            //        cols.Add(paramcalc.Anno);
            //        cols.Add("");
            //        cols.Add("Tipologia Superfici");
            //        cols.Add(paramcalc.TypeMQ);
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Dati PEF");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("");
            //        cols.Add("N.Utenze");
            //        cols.Add("% Utenze");
            //        cols.Add("Rifiuti prodotti (Kg)");
            //        cols.Add("Costi Parte Fissa");
            //        cols.Add("Costi Parte Variabile");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Domestiche");
            //        cols.Add(parampef.NUtenzeDOM.ToString());
            //        cols.Add(parampef.PercUtenzeDOM.ToString());
            //        cols.Add(parampef.KGRifiutiDOM.ToString());
            //        cols.Add(parampef.CostiPFDOM.ToString());
            //        cols.Add(parampef.CostiPVDOM.ToString());
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Non Domestiche");
            //        cols.Add(parampef.NUtenzeNONDOM.ToString());
            //        cols.Add(parampef.PercUtenzeNONDOM.ToString());
            //        cols.Add(parampef.KGRifiutiNONDOM.ToString());
            //        cols.Add(parampef.CostiPFNONDOM.ToString());
            //        cols.Add(parampef.CostiPVNONDOM.ToString());
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Parte Fissa");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("Parte Variabile");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Categoria (NC)");
            //        cols.Add("MQ");
            //        cols.Add("Coefficiente");
            //        cols.Add("MQ Normalizzati");
            //        cols.Add("Tariffa");
            //        cols.Add("");
            //        cols.Add("Categoria (NC)");
            //        cols.Add("MQ");
            //        cols.Add("Coefficiente");
            //        cols.Add("MQ Normalizzati");
            //        cols.Add("Tariffa");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        foreach (ToDefineTariffe pf in listPFDom)
            //        {
            //            cols = new List<string>();
            //            cols.Add(pf.Descrizione);
            //            cols.Add(pf.MQ.ToString());
            //            cols.Add(pf.CoeffK.ToString());
            //            cols.Add(pf.MQNormalizzati.ToString());
            //            cols.Add(pf.Tariffa.ToString());
            //            cols.Add("");
            //            foreach (ToDefineTariffe pv in listPVDom)
            //            {
            //                if (pf.NC == pv.NC)
            //                {
            //                    cols.Add(pv.Descrizione);
            //                    cols.Add(pv.MQ.ToString());
            //                    cols.Add(pv.CoeffK.ToString());
            //                    cols.Add(pv.MQNormalizzati.ToString());
            //                    cols.Add(pv.Tariffa.ToString());
            //                    break;
            //                }
            //            }
            //            for (int x = cols.Count; x < MaxCol; x++)
            //                cols.Add("");
            //            rows.Add(cols);
            //        }
            //        //
            //        cols = new List<string>();
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Parte Variabile");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("");
            //        cols.Add("Parte Variabile");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Categoria");
            //        cols.Add("MQ");
            //        cols.Add("Coefficiente");
            //        cols.Add("MQ Normalizzati");
            //        cols.Add("Tariffa");
            //        cols.Add("");
            //        cols.Add("Categoria");
            //        cols.Add("MQ");
            //        cols.Add("Coefficiente");
            //        cols.Add("MQ Normalizzati");
            //        cols.Add("Tariffa");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        foreach (ToDefineTariffe pf in listPFNonDom)
            //        {
            //            cols = new List<string>();
            //            cols.Add(pf.Descrizione);
            //            cols.Add(pf.MQ.ToString());
            //            cols.Add(pf.CoeffK.ToString());
            //            cols.Add(pf.MQNormalizzati.ToString());
            //            cols.Add(pf.Tariffa.ToString());
            //            cols.Add("");
            //            foreach (ToDefineTariffe pv in listPVNonDom)
            //            {
            //                if (pf.IdCategoria == pv.IdCategoria)
            //                {
            //                    cols.Add(pv.Descrizione);
            //                    cols.Add(pv.MQ.ToString());
            //                    cols.Add(pv.CoeffK.ToString());
            //                    cols.Add(pv.MQNormalizzati.ToString());
            //                    cols.Add(pv.Tariffa.ToString());
            //                    break;
            //                }
            //            }
            //            for (int x = cols.Count; x < MaxCol; x++)
            //                cols.Add("");
            //            rows.Add(cols);
            //        }
            //        //
            //        cols = new List<string>();
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Totalizzatori");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        cols = new List<string>();
            //        cols.Add("Categoria");
            //        cols.Add("N.Componenti Fissa");
            //        cols.Add("N.Componenti Variabile");
            //        cols.Add("Riduzione");
            //        cols.Add("Detassazione");
            //        cols.Add("N.Utenti");
            //        cols.Add("MQ");
            //        cols.Add("N.Utenti Netti");
            //        cols.Add("MQ Netti");
            //        for (int x = cols.Count; x < MaxCol; x++)
            //            cols.Add("");
            //        rows.Add(cols);
            //        //
            //        foreach (TotaliSimulazione tot in listTotali)
            //        {
            //            cols = new List<string>();
            //            cols.Add(tot.DescrCategoria);
            //            cols.Add(tot.nComponentiPF.ToString());
            //            cols.Add(tot.nComponentiPV.ToString());
            //            cols.Add(tot.DescrRiduzione);
            //            cols.Add(tot.DescrDetassazione);
            //            cols.Add(tot.nUtenze.ToString());
            //            cols.Add(tot.nMQ.ToString());
            //            cols.Add(tot.UtenzeUtili.ToString());
            //            cols.Add(tot.MQUtili.ToString());
            //            for (int x = cols.Count; x < MaxCol; x++)
            //                cols.Add("");
            //            rows.Add(cols);
            //        }

            //        ds = myxls.CreateDataSet(rows, MaxCol);
            //        if (ds != null)
            //        {
            //            filepath = Server.MapPath(@".\ExcelTemplate.xls");
            //            myxls.ExportDataSetToExcel(ds, filepath);
            //        }
            //    }
            //}
            //catch (System.Threading.ThreadAbortException er)
            //{
            //    Global.Log.Write2(LogSeverity.Information, "End ExportDataSetToExcel");
            //}
            //catch (Exception er)
            //{
            //    Global.Log.Write2(LogSeverity.Critical, er);
            //}
        }
        protected void CmdResetClick(object sender, EventArgs e)
        {
        }
        protected void CmdSaveRidDetClick(object sender, EventArgs e)
        {
            string sScript = "<script language='javascript'>";
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            int idUI = 0;
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();

            try
            {
                int.TryParse(hfIdUI.Value, out idUI);
                //elimino dalla tabella di appoggio tutte le riduzioni e le detassazioni associate
                Articolo myArticolo = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, Id = idUI };
                if (myArticolo.Load())
                {
                    if (myArticolo.DeleteRidEse())
                    {
                        //ciclo sulla griglia delle riduzioni per salvare le riduzioni selezionate
                        foreach (GridViewRow myRow in rgvRid.Rows)
                        {
                            if (((CheckBox)myRow.Cells[2].FindControl("chkSel")).Checked == true)
                            {
                                RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myItem = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                                myItem.sCodice = myRow.Cells[0].Text;
                                myItem.sDescrizione = myRow.Cells[1].Text;
                                if (myItem.sCodice != string.Empty)
                                {
                                    MyArrayRid.Add(myItem);
                                    //myArticolo.Partita.oRiduzioni = MyArrayRid.ToArray();
                                }
                            }
                        }
                        //ciclo sulla griglia delle detassazioni per salvare le riduzioni selezionate
                        foreach (GridViewRow myRow in rgvDet.Rows)
                        {
                            if (((CheckBox)myRow.Cells[2].FindControl("chkSel")).Checked == true)
                            {
                                RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myItem = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                                myItem.sCodice = myRow.Cells[0].Text;
                                myItem.sDescrizione = myRow.Cells[1].Text;
                                if (myItem.sCodice != string.Empty)
                                {
                                    MyArrayDet.Add(myItem);
                                    //myArticolo.Partita.oDetassazioni = MyArrayRid.ToArray();
                                }
                            }
                        }
                        //salvo le modifiche apportate
                        if (!myArticolo.Save(MyArrayRid.ToArray(), MyArrayDet.ToArray()))
                        {
                            sScript += "alert('Errore in salvataggio!');";
                        }
                        else
                        {
                            //ricarico la griglia
                            Articolo[] ListArticoli = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, IdSimulazione = idSimula }.LoadAll();
                            Session["SimulaArticoliAggMassivo"] = ListArticoli;
                            LoadSearch(); HideDiv("divRidDet");
                            sScript += "alert('Salvataggio terminato con successo!');";
                        }
                    }
                    else
                    {
                        sScript += "alert('Errore in salvataggio!');";
                    }
                }
                else
                {
                    sScript += "alert('Errore in salvataggio!');";
                }
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.CmdSaveRidDetClick.errore::", ex);;
                throw ex;
            }
        }
        protected void CmdExitRidEseClick(object sender, EventArgs e)
        {
            HideDiv("divRidDet");
            if (idSimula > 0)
                HideDiv("ParamSearch"); HideDiv("ParamAgg");
            ManageBottoniera();
        }
        #endregion
        #region "Griglia"
        protected void rgvUIPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                LoadSearch(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.rgvUIPageIndexChanging.errore::", ex);;
                throw ex;
            }
            finally
            {
                HideDiv("divRidDet");
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
            }
        }
        protected void rgvRidDetPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                LoadRidDet(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.rgvRidDetPageIndexChanging.errore::", ex);;
                throw ex;
            }
            finally
            {
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
            }
        }

        protected void rgvUIRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idUI;
                int.TryParse(e.CommandArgument.ToString(), out idUI);
                if (idUI > 0)
                {
                    hfIdUI.Value = idUI.ToString();
                    switch (e.CommandName)
                    {
                        case "EditRidEse":
                            ShowDiv("divRidDet");
                            break;
                        case "SaveUI":
                            SaveUI(idUI);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.rgvUIRowCommand.errore::", ex);;
                throw ex;
            }
            finally
            {
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
                ManageBottoniera();
            }
        }
        protected void rgvRidDetRowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        #endregion
        private void ManageBottoniera()
        {
            if (idSimula > 0)
            {
                ShowDiv("divSimula"); HideDiv("divCmdSearch"); HideDiv("divHeading");
            }
            else
            {
                ShowDiv("divCmdSearch"); HideDiv("divSimula"); HideDiv("ParamAgg");
                string sScript = "<script language='javascript'>";
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript += "document.getElementById('aVisParamSearch').style.display='none';";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "visps", sBuilder.ToString());
            }
        }
        private void LoadCombo()
        {
            try
            {
                CategorieCatastali myCatCat = new CategorieCatastali { };
                rddlCatCatastale.DataSource = myCatCat.LoadAll();
                rddlCatCatastale.DataValueField = "CodiceCategoria";
                rddlCatCatastale.DataTextField = "Definizione";
                rddlCatCatastale.DataBind();
                CategorieAtecoEnte myCat = new CategorieAtecoEnte { Ente = Ente };
                CategorieAtecoEnte[] listCat = myCat.LoadAll();
                rddlCat.DataSource = listCat;
                rddlCat.DataValueField = "IdCategoriaAteco";
                rddlCat.DataTextField = "Descrizione";
                rddlCat.DataBind();
                rddlAggCat.DataSource = listCat;
                rddlAggCat.DataValueField = "IdCategoriaAteco";
                rddlAggCat.DataTextField = "Descrizione";
                rddlAggCat.DataBind();
                RidEseEnte myRid = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = "R" };
                RidEseEnte[] listRidEse = myRid.LoadAll();
                rddlRid.DataSource = listRidEse;
                rddlRid.DataValueField = "Codice";
                rddlRid.DataTextField = "Definizione";
                rddlRid.DataBind();
                myRid = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = "R", hasOptDel = 1 };
                listRidEse = myRid.LoadAll();
                rddlAggRid.DataSource = listRidEse;
                rddlAggRid.DataValueField = "Codice";
                rddlAggRid.DataTextField = "Definizione";
                rddlAggRid.DataBind();
                myRid = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = "D" };
                listRidEse = myRid.LoadAll();
                rddlDet.DataSource = listRidEse;
                rddlDet.DataValueField = "Codice";
                rddlDet.DataTextField = "Definizione";
                rddlDet.DataBind();
                myRid = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = "D", hasOptDel = 1 };
                listRidEse = myRid.LoadAll();
                rddlAggDet.DataSource = listRidEse;
                rddlAggDet.DataValueField = "Codice";
                rddlAggDet.DataTextField = "Definizione";
                rddlAggDet.DataBind();
                StatoOccupazione myStatoOccupazione = new StatoOccupazione { };
                StatoOccupazione[] listStatoOccup = myStatoOccupazione.LoadAll();
                rddlStatoOccupazione.DataSource = listStatoOccup;
                rddlStatoOccupazione.DataValueField = "Codice";
                rddlStatoOccupazione.DataTextField = "Definizione";
                rddlStatoOccupazione.DataBind();
                rddlAggStatoOccupazione.DataSource = listStatoOccup;
                rddlAggStatoOccupazione.DataValueField = "Codice";
                rddlAggStatoOccupazione.DataTextField = "Definizione";
                rddlAggStatoOccupazione.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.LoadCombo.errore::", ex);;
                throw;
            }
        }
        protected CategorieAtecoEnte[] LoadCat()
        {
            try
            {
                CategorieAtecoEnte myCat = new CategorieAtecoEnte { Ente = Ente };
                CategorieAtecoEnte[] listCat = myCat.LoadAll();
                return listCat;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.LoadCat.errore::", ex);;
                throw ex;
            }
        }
        private void LoadRidDet(int? page = 0)
        {
            try
            {
                RidEseEnte RidEse = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = "R" };
                rgvRid.DataSource = RidEse.LoadAll();
                if (page.HasValue)
                    rgvRid.PageIndex = page.Value;
                rgvRid.DataBind();

                RidEse = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = "D" };
                rgvDet.DataSource = RidEse.LoadAll();
                if (page.HasValue)
                    rgvDet.PageIndex = page.Value;
                rgvDet.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.LoadRidDet.errore::", ex);;
                throw;
            }
        }
        private void LoadParamSearch()
        {
            try
            {
                if (Session["ParamSearchAggMassivo"] != null)
                {
                    Global.Log.Write2(LogSeverity.Debug,"ho Session[ParamSearchAggMassivo]");
                    List<string> listParam = new List<string>((List<string>)Session["ParamSearchAggMassivo"]);
                    int x = 0;
                    TxtVia.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    TxtCivico.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    TxtFoglio.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    TxtNumero.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    TxtSubalterno.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    rddlCatCatastale.SelectedValue = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    txtDal.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    txtAl.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    rddlCat.SelectedValue = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    txtNC.Text = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    chkPF.Checked = bool.Parse(listParam[x].ToString());
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    chkPV.Checked = bool.Parse(listParam[x].ToString());
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    rddlRid.SelectedValue = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    rddlDet.SelectedValue = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    rddlStatoOccupazione.SelectedValue = listParam[x].ToString();
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    switch (listParam[x].ToString())
                    {
                        case "0":
                            optNoRes.Checked = true;
                            break;
                        case "1":
                            optRes.Checked = true;
                            break;
                        default:
                            optResNoRes.Checked = true;
                            break;
                    }
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    chkEsente.Checked = bool.Parse(listParam[x].ToString());
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    x++;
                    chkMoreUI.Checked = bool.Parse(listParam[x].ToString());
                    Global.Log.Write2(LogSeverity.Debug, "ho " + listParam[x].ToString());
                    string hasParam = "";
                    foreach (string myItem in listParam)
                        hasParam += myItem.Replace("false", "").Replace("-1", "");
                    if (hasParam != "")
                        Search();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.LoadParamSearch.errore::", ex);;
                throw;
            }
        }
        private void Search()
        {
            Articolo[] listArticoli = null;
            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata ParamSearchUI = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata();
            try
            {
                ParamSearchUI.sVia = TxtVia.Text;
                ParamSearchUI.sCivico = TxtCivico.Text;
                ParamSearchUI.sFoglio = TxtFoglio.Text;
                ParamSearchUI.sNumero = TxtNumero.Text;
                ParamSearchUI.sSubalterno = TxtSubalterno.Text;
                ParamSearchUI.IdCatCatastale = rddlCatCatastale.SelectedValue;
                ParamSearchUI.Dal = txtDal.Text;
                ParamSearchUI.Al = txtAl.Text;
                if (rddlCat.SelectedValue != "")
                    ParamSearchUI.IdCatTARES = rddlCat.SelectedValue;
                if (txtNC.Text != "")
                    ParamSearchUI.nComponenti = int.Parse(txtNC.Text);
                ParamSearchUI.IdStatoOccupazione = rddlStatoOccupazione.SelectedValue;

                if (optRes.Checked)
                    ParamSearchUI.TypeSogRes = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata.Sog_Res;
                else if (optNoRes.Checked)
                    ParamSearchUI.TypeSogRes = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata.Sog_NoRes;
                else
                    ParamSearchUI.TypeSogRes = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata.Sog_ALL;

                ParamSearchUI.IsPF = chkPF.Checked;
                ParamSearchUI.IsPV = chkPV.Checked;
                ParamSearchUI.IsEsente = chkEsente.Checked;
                ParamSearchUI.HasMoreUI = chkMoreUI.Checked;
                ParamSearchUI.IdRiduzione = rddlRid.SelectedValue;
                ParamSearchUI.IdDetassazione = rddlDet.SelectedValue;

                listArticoli = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, IdSimulazione = idSimula, ParamSearch = ParamSearchUI }.LoadAll();
                Session["SimulaArticoliAggMassivo"] = listArticoli;
                LoadSearch();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.Search.errore::", ex);;
                throw ex;
            }
            finally
            {
                HideDiv("divRidDet");
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
                ManageBottoniera();
            }
        }
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                if (Session["SimulaArticoliAggMassivo"] != null)
                {
                    List<Articolo> listArticoli = new List<Articolo>((Articolo[])Session["SimulaArticoliAggMassivo"]);
                    rgvUI.DataSource = listArticoli;
                    if (page.HasValue)
                        rgvUI.PageIndex = page.Value;
                    rgvUI.DataBind();
                    rgvUI.Visible = true;
                    if (idSimula > 0)
                        rgvUI.Columns[13].Visible = true;
                    else
                        rgvUI.Columns[13].Visible = false;
                    ShowDiv("SearchResult");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.LoadSearch.errore::", ex);;
                rgvUI.Visible = false;
                HideDiv("SearchResult");
                //throw;
            }
        }
        private void SaveUI(int idUI)
        {
            string sScript = "<script language='javascript'>";
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

            try
            {
                Articolo myArticolo = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, Id = idUI };
                if (myArticolo.Load())
                {
                    //ciclo sulla griglia per salvare
                    foreach (GridViewRow myRow in rgvUI.Rows)
                    {
                        int IdRow = 0;
                        int.TryParse(((HiddenField)myRow.Cells[0].FindControl("hfIdArticolo")).Value.ToString(), out IdRow);
                        if (IdRow == idUI)
                        {
                            myArticolo.Partita.tDataInizio = DateTime.Parse(((TextBox)myRow.Cells[3].FindControl("txtInizio")).Text);
                            myArticolo.Partita.tDataFine = DateTime.Parse(((TextBox)myRow.Cells[4].FindControl("txtFine")).Text);
                            myArticolo.Partita.IdCatAteco = int.Parse(((DropDownList)myRow.Cells[7].FindControl("ddlCat")).SelectedValue);
                            myArticolo.Partita.nNComponenti = int.Parse(((TextBox)myRow.Cells[8].FindControl("txtNCFissa")).Text);
                            myArticolo.Partita.nComponentiPV = int.Parse(((TextBox)myRow.Cells[9].FindControl("txtNCVariabile")).Text);
                            myArticolo.Partita.nMQTassabili = double.Parse(((TextBox)myRow.Cells[10].FindControl("txtMQ")).Text);
                        }
                    }
                    //salvo le modifiche apportate
                    if (!myArticolo.Save(myArticolo.Partita.oRiduzioni,myArticolo.Partita.oDetassazioni))
                    {
                        sScript += "alert('Errore in salvataggio!');";
                    }
                    else
                    {
                        //ricarico la griglia
                        Articolo[] ListArticoli = new Articolo { FromVariabile = FromVariabile, IdEnte = Ente, IdSimulazione = myArticolo.IdSimulazione }.LoadAll();
                        Session["SimulaArticoliAggMassivo"] = ListArticoli;
                        sScript += "alert('Salvataggio terminato con successo!');";
                    }
                }
                else
                {
                    sScript += "alert('Errore in salvataggio!');";
                }
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.SaveUI.errore::", ex);;
                throw ex;
            }
        }
        protected void SelAllRow(object sender, System.EventArgs e)
        {
            try
            {
                GridViewRow myHead = rgvUI.HeaderRow;
                CheckBox chkRow = ((CheckBox)myHead.Cells[12].FindControl("chkAll"));

                //ciclo per salvare
                Articolo[] ListArticoli = (Articolo[])Session["SimulaArticoliAggMassivo"];
                foreach (Articolo myItem in (ListArticoli))
                    myItem.IsSel = chkRow.Checked;
                Session["SimulaArticoliAggMassivo"] = ListArticoli;
                LoadSearch();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.SelAllRow.errore::", ex);;
                throw ex;
            }
            finally
            {
                HideDiv("divRidDet");
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
                ManageBottoniera();
            }
        }
        protected void SelRow()
        {
            try
            {
                if (Session["SimulaArticoliAggMassivo"] != null)
                {
                    Articolo[] ListArticoli = (Articolo[])Session["SimulaArticoliAggMassivo"];
                    foreach (GridViewRow itemGrid in rgvUI.Rows)
                    {
                        foreach (Articolo myItem in ListArticoli)
                        {
                            if (myItem.Id.ToString() == ((HiddenField)itemGrid.Cells[0].FindControl("hfIdArticolo")).Value)
                            {
                                myItem.IsSel = ((CheckBox)itemGrid.FindControl("chkSel")).Checked;
                                break;
                            }
                        }
                    }
                    Session["SimulaArticoliAggMassivo"] = ListArticoli;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.AggMassivo.SelRow.errore::", ex);;
                throw ex;
            }
            finally
            {
                HideDiv("divRidDet");
                if (idSimula > 0)
                    HideDiv("ParamSearch"); HideDiv("ParamAgg");
                ManageBottoniera();
            }
        }
    }
}