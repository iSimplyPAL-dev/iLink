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

using System.Configuration;
using DichiarazioniICI.Database;
using Business;
using ComPlusInterface;
using Ribes;
using System.Globalization;
using System.Threading;

using Microsoft.VisualBasic;
using AnagInterface;
using log4net;
using System.Collections.Generic;
using ElaborazioneDatiStampeInterface;
using Utility;

namespace DichiarazioniICI//.RavvedimentoOperoso
{
    /// <summary>
    /// Pagina per il calcolo del ravvedimento operoso.
    /// Contiene i parametri di calcolo e le funzioni della comandiera.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class GestioneRavvedimento : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GestioneRavvedimento));
        int myNFab;
        double myAbiPrincAcc, myAbiPrincSal, myAbiPrincTot, myAltriFabAcc, myAltriFabSal, myAltriFabTot, myAltriFabStatoAcc, myAltriFabStatoSal, myAltriFabSatoTot, myAreeFabAcc, myAreeFabSal, myAreeFabTot, myAreeFabStatoAcc, myAreeFabStatoSal, myAreeFabStatoTot, myTerreniAcc, myTerreniSal, myTerreniTot, myTerreniStatoAcc, myTerreniStatoSal, myTerreniStatoTot, myFabRurAcc, myFabRurSal, myFabRurTot, myFabRurStatoAcc, myFabRurStatoSal, myFabRurStatoTot, myUsoProdAcc, myUsoProdSal, myUsoProdTot, myUsoProdStatoAcc, myUsoProdStatoSal, myUsoProdStatoTot, myDetrazAcc, myDetrazSal, myDetrazTot, myDovutoAcc, myDovutoSal, myDovutoTot;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="14/804/2020">Sono cambiate le regole di applicazione sanzione</revision></revisionHistory>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string sScript = "";
            // Put user code to initialize the page here
            try
            {
                if (ConstWrapper.HasPlainAnag)
                    sScript += "document.getElementById('TRSpecAnag').style.display='none';";
                else
                    sScript += "document.getElementById('TRPlainAnag').style.display='none';";
                RegisterScript(sScript, this.GetType());

                if (ConstWrapper.HasPlainAnag)
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Costanti.AZIONE_NEW.ToString());

                if (!Page.IsPostBack)
                {
                    PanelSanzInt.Visible = false;
                    PanelRO.Visible = false;
                    txtAnno.Text = (DateTime.Now.Year - 1).ToString();

                    sScript = "parent.Comandi.location.href='./CGestioneRavvedimento.aspx';";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception Err)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.Page_Load.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <revisionHistory><revision date="14/804/2020">Sono cambiate le regole di applicazione sanzione</revision></revisionHistory>
        private DataSet PopolaGridSanzioni()
        {
            DataSet objDSSanzioni;
            string sSQL;
            try
            {
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionProvvedimenti))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetSanzioniRavvedimento");
                    objDSSanzioni = ctx.GetDataSet(sSQL, "TBL");
                    ctx.Dispose();
                }
                return objDSSanzioni;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.PopolaGridSanzioni.errore: ", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataSet PopolaComboInteresse()
        {
            DataSet objDSInteressi;
            string sSQL;
            try
            {
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionProvvedimenti))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetInteressiRavvedimento","IDENTE","ANNO","TRIBUTO");
                    objDSInteressi = ctx.GetDataSet(sSQL, "TBL",ctx.GetParam("IDENTE",ConstWrapper.CodiceEnte) 
                            , ctx.GetParam("ANNO", txtAnno.Text)
                            , ctx.GetParam("TRIBUTO", Costanti.TRIBUTO_ICI)
                        );
                    ctx.Dispose();
                }
                return objDSInteressi;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.PopolaComboInteresse.errore: ", ex);
                return null;
            }
        }
        //private DataSet PopolaComboInteresse()
        //{
        //    DataSet myDataset;
        //    Hashtable objHashTable = new Hashtable();

        //    try
        //    {
        //        //carico la hash table
        //        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstWrapper.StringConnectionProvvedimenti);//objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", strConnectionStringOPENgovProvvedimenti);
        //        objHashTable.Add("USER", Session["username"]);
        //        objHashTable.Add("CODENTE", Session["CODENTE"]);

        //        objHashTable.Add("CODTIPOINTERESSE", "-1");
        //        objHashTable.Add("DAL", "");
        //        objHashTable.Add("AL", "");
        //        objHashTable.Add("TASSO", "");
        //        //aggiunto da ema 20071206
        //        objHashTable.Add("CODTRIBUTO", Costanti.TRIBUTO_ICI);

        //        IGestioneConfigurazione objCOMTipoVoci = (IGestioneConfigurazione)Activator.GetObject(typeof(IGestioneConfigurazione), ConfigurationManager.AppSettings["URLGestioneConfigurazione"].ToString());
        //        myDataset = objCOMTipoVoci.GetTipoInteresse(objHashTable);
        //        return myDataset;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.PopolaComboIntese.errore: ", ex);
        //        return null;
        //    }
        //}

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
            //this.GrdRavvedimentoOperoso.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrdRavvedimentoOperoso_ItemDataBound);

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRibalta_Click(object sender, System.EventArgs e)
        {
            this.Ribalta();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Ribalta()
        {
            DettaglioAnagrafica oDettaglioAnagrafica;
            try
            {
                if (Session["contribuente"] != null)
                {
                    oDettaglioAnagrafica = (DettaglioAnagrafica)(Session["contribuente"]);
                    txtCodFiscaleContr.Text = oDettaglioAnagrafica.CodiceFiscale;
                    txtPIVAContr.Text = oDettaglioAnagrafica.PartitaIva;
                    txtCognomeContr.Text = oDettaglioAnagrafica.Cognome;
                    txtNomeContr.Text = oDettaglioAnagrafica.Nome;
                    hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString();
                    if (oDettaglioAnagrafica.Sesso == "")
                    {
                        rdbMaschioContr.Checked = false;
                        rdbFemminaContr.Checked = false;
                        rdbGiuridicaContr.Checked = false;
                    }
                    if (oDettaglioAnagrafica.Sesso == "M")
                        rdbMaschioContr.Checked = true;
                    if (oDettaglioAnagrafica.Sesso == "F")
                        rdbFemminaContr.Checked = true;
                    if (oDettaglioAnagrafica.Sesso == "G")
                        rdbGiuridicaContr.Checked = true;

                    if (oDettaglioAnagrafica.DataNascita != "00/00/1900")
                    {
                        txtDataNascContr.Text = oDettaglioAnagrafica.DataNascita;
                    }
                    else
                    {
                        txtDataNascContr.Text = string.Empty;
                    }
                    txtComNascContr.Text = oDettaglioAnagrafica.ComuneNascita;
                    txtProvNascContr.Text = oDettaglioAnagrafica.ProvinciaNascita;
                    txtViaResContr.Text = oDettaglioAnagrafica.ViaResidenza;
                    txtNumCivResContr.Text = oDettaglioAnagrafica.CivicoResidenza;
                    txtIntResContr.Text = oDettaglioAnagrafica.InternoCivicoResidenza;
                    txtScalaResContr.Text = oDettaglioAnagrafica.ScalaCivicoResidenza;
                    txtComuneResContr.Text = oDettaglioAnagrafica.ComuneResidenza;
                    txtProvResContr.Text = oDettaglioAnagrafica.ProvinciaResidenza;
                    txtCapResContr.Text = oDettaglioAnagrafica.CapResidenza;
                    txtEsponenteCivico.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza;
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.Ribalta.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerificaContribuente_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            RegisterScript("ApriRicercaAnagrafeCalcoloIci('contribuente');", this.GetType());

            PanelSanzInt.Visible = false;
            PanelRO.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalva_Click(object sender, System.EventArgs e)
        {
            string strscript = "";
            try
            {
                DataTable TabellaRO = new DataTable();

                TabellaRO.Columns.Add("ENTE");
                TabellaRO.Columns.Add("ANNO");
                TabellaRO.Columns.Add("COD_CONTRIBUENTE");
                TabellaRO.Columns.Add("TIPOLOGIA_VERSAMENTO");
                TabellaRO.Columns.Add("TIPO_VERSAMENTO");
                TabellaRO.Columns.Add("DATA_SCADENZA");
                TabellaRO.Columns.Add("GIORNI_RITARDO");
                TabellaRO.Columns.Add("IMPORTO_TOTALE");
                TabellaRO.Columns.Add("IMPORTO_NON_VERSATO");
                TabellaRO.Columns.Add("IMPORTO_SANZIONI");
                TabellaRO.Columns.Add("IMPORTO_INTERESSI");
                TabellaRO.Columns.Add("COD_VOCE");
                TabellaRO.Columns.Add("ID_VALORE_VOCE");
                TabellaRO.Columns.Add("VALORE_VOCE");
                TabellaRO.Columns.Add("TIPO_MISURA_VOCE");
                TabellaRO.Columns.Add("COD_TIPO_INTERESSE");
                TabellaRO.Columns.Add("VALORE_INTERESSE");
                TabellaRO.Columns.Add("NOTE");

                object[] ArrCampi;
                ArrCampi = new object[18];
                foreach (GridViewRow oItemGrid in GrdRavvedimentoOperoso.Rows)
                {
                    if (((CheckBox)oItemGrid.FindControl("chkSelect")).Checked == true)
                    {
                        ArrCampi[0] = oItemGrid.Cells[15].Text; //ENTE
                        ArrCampi[1] = oItemGrid.Cells[16].Text;//ANNO
                        ArrCampi[2] = oItemGrid.Cells[17].Text;//COD_CONTRIBUENTE
                        ArrCampi[3] = oItemGrid.Cells[1].Text;//TIPOLOGIA_VERSAMENTO
                        ArrCampi[4] = oItemGrid.Cells[18].Text;//TIPO_VERSAMENTO
                        ArrCampi[5] = ((TextBox)(oItemGrid.FindControl("txtDataScadenza"))).Text;//DATA_SCADENZA		
                        ArrCampi[6] = ((TextBox)(oItemGrid.FindControl("txtGGritardo"))).Text;//GIORNI_RITARDO
                        ArrCampi[7] = ((TextBox)(oItemGrid.FindControl("txtTotale"))).Text.Replace(".", "");//IMPORTO_TOTALE
                        ArrCampi[8] = ((TextBox)(oItemGrid.FindControl("txtTotaleNonVersato"))).Text.Replace(".", "");//IMPORTO_NON_VERSATO
                        ArrCampi[9] = ((TextBox)(oItemGrid.FindControl("txtTotaleSanzioni"))).Text.Replace(".", "");//IMPORTO_SANZIONI
                        ArrCampi[10] = ((TextBox)(oItemGrid.FindControl("txtTotaleInteressi"))).Text.Replace(".", "");//IMPORTO_INTERESSI
                        ArrCampi[11] = ViewState["CODICEVOCE"].ToString();//COD_VOCE
                        ArrCampi[12] = ViewState["IDVALOREVOCE"].ToString();//ID_VALORE_VOCE
                        ArrCampi[13] = ViewState["VALOREVOCE"];//VALORE_VOCE
                        ArrCampi[14] = ViewState["CODMISURA"].ToString();//TIPO_MISURA_VOCE
                        ArrCampi[15] = ViewState["COD_TIPO_INTERESSE"].ToString();//COD_TIPO_INTERESSE
                        ArrCampi[16] = ViewState["VALORE_INTERESSE"].ToString();//VALORE_INTERESSE
                        ArrCampi[17] = ((TextBox)(oItemGrid.FindControl("txtNote"))).Text;//NOTE

                        TabellaRO.Rows.Add(ArrCampi);
                    }
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(TabellaRO);

                bool bRetVal = false;
                TblRavvedimentoOperoso objTblRavvedimentoOperoso = new TblRavvedimentoOperoso();
                bRetVal = objTblRavvedimentoOperoso.InsertRavvedimentoOperoso(ds);

                if (bRetVal)
                {
                    strscript = "GestAlert('a', 'success', '', '', 'Salvataggio del Ravvedimento Operoso effettuato con successo!');";
                    RegisterScript(strscript, this.GetType());
                }
                else
                {
                    strscript = "GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante il salvataggio del Ravvedimento Operoso!');";
                    RegisterScript(strscript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.btnSalva_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");

            }
        }
        /// <summary>
        /// Pulsante per la pulizia del contribuente e relativo calcolo per procedere ad un nuovo calcolo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                hdIdContribuente.Value = "-1";
                if (ConstWrapper.HasPlainAnag)
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Costanti.AZIONE_NEW.ToString());

                    PanelSanzInt.Visible = false;
                    PanelRO.Visible = false;
                    txtAnno.Text = (DateTime.Now.Year - 1).ToString();
            }
            catch (Exception Err)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.btnClear_Click.errore: ", Err);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Si applica una sanzione diversa in base ai giorni di ritardo come da prospetto:
        /// Momento del ravvedimento	Sanzione ridotta da ravvedimento
        /// entro i primi 14 giorni	    0,1% per ogni giorno di ritardo
        /// dal 15° al 30° giorno	    1,50%
        /// dal 31° al 90° giorno	    1,67%
        /// dal 91° giorno 31/12/AA+1	3,75%
        /// entro 31/12/AA+2	        4,29%
        /// oltre 31/12/AA+2	        5%
        /// la griglia sopra è intabellata per consentirne una variazione di aliquote più agevole.
        /// in bae ai giorni di ritardo totali costruisco array delle singole sanzioni da applicare;ciclando sull'array eseguo il calcolo.
        /// </summary>
        /// <param name="dsSanzioni"></param>
        /// <param name="iGGritardo"></param>
        /// <param name="dImportoNonPagato"></param>
        /// <returns></returns>
        /// <revisionHistory><revision date="14/04/2020">Sono cambiate le regole di applicazione sanzione</revision></revisionHistory>
        private double CalcolaSanzione(DataSet dsSanzioni, int iGGritardo, double dImportoNonPagato)
        {
            objRavvedimento myItem = new objRavvedimento();
            List<objRavvedimento> ListItem = new List<objRavvedimento>();
            double myTot = 0;
            try
            {
                foreach (DataRow myRow in dsSanzioni.Tables[0].Rows)
                {
                    if (iGGritardo > StringOperation.FormatInt(myRow["da"]) && iGGritardo <= StringOperation.FormatInt(myRow["a"]))
                    {
                        myItem = new objRavvedimento();
                        myItem.da = StringOperation.FormatInt(myRow["da"]);
                        myItem.a = StringOperation.FormatInt(myRow["a"]);
                        myItem.giorni = ((StringOperation.FormatString(myRow["tipocalcolo"]) == "G") ? (iGGritardo - StringOperation.FormatInt(myRow["da"])) : 1);
                        myItem.aliquota = StringOperation.FormatDouble(myRow["aliquota"]);
                        ListItem.Add(myItem);
                    }
                }
                foreach (objRavvedimento myRav in ListItem)
                {
                    myRav.importo += ((dImportoNonPagato * myRav.aliquota / 100) * myRav.giorni);
                    log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.CalcolaSanzione.((dImportoNonPagato * myRav.aliquota / 100) * myRav.giorni)->(("+dImportoNonPagato.ToString()+" * "+ myRav.aliquota.ToString()+" / 100) * "+myRav.giorni.ToString()+")");
                }
                foreach (objRavvedimento myRav in ListItem)
                {
                    myTot += myRav.importo;
                }
                return myTot;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.CalcolaSanzione.errore: ", ex);
                return -1;
            }
        }
        private double CalcolaInteressi(DataSet dsInteressi, double dImportoNonPagato,string sRata)
        {
            objRavvedimento myItem = new objRavvedimento();
            List<objRavvedimento> ListItem = new List<objRavvedimento>();
            double myTot = 0;
            try
            {
                foreach (DataRow myRow in dsInteressi.Tables[0].Rows)
                {
                    myItem = new objRavvedimento();
                    myItem.da = StringOperation.FormatInt(myRow["dal"]);
                    myItem.a = StringOperation.FormatInt(myRow["al"]);
                    if (sRata == "S")
                    {
                        myItem.giorni = StringOperation.FormatInt(myRow["ggsal"]);
                    }
                    else {
                        myItem.giorni = StringOperation.FormatInt(myRow["ggacc"]);
                    }
                    myItem.aliquota = StringOperation.FormatDouble(myRow["tasso_annuale"]);
                    ListItem.Add(myItem);
                }
                foreach (objRavvedimento myRav in ListItem)
                {
                    myRav.importo += (((dImportoNonPagato * myRav.aliquota / 100) * myRav.giorni) / 365);
                    log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.CalcolaInteressi.((dImportoNonPagato * myRav.aliquota / 100) * myRav.giorni)->((" + dImportoNonPagato.ToString() + " * " + myRav.aliquota.ToString() + " / 100) * " + myRav.giorni.ToString() + ")");
                }
                foreach (objRavvedimento myRav in ListItem)
                {
                    myTot += myRav.importo;
                }
                return myTot;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.CalcolaInteressi.errore: ", ex);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcolaRO_Click(object sender, System.EventArgs e)
        {
            DataSet dsSanzioni = new DataSet();
            DataSet dsInteressi = new DataSet();
            string scadenza = string.Empty;
            double dImportoNonPagato;
            double dTotSanzione;
            double dTotInteresse;
            int GG;
            double dTotale;
            string sRata = "A";

            try
            {
                if (!this.Controlli(out dsSanzioni, out dsInteressi))
                {
                    string strscript = "GestAlert('a', 'warning', '', '', 'Sanzioni non presenti!');";
                    RegisterScript(strscript, this.GetType());
                    return;
                }
                if (DelCalcoloFinale() < 0)
                {
                    string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Sanzioni!');";
                    RegisterScript(strscript, this.GetType());
                    return;
                }
                foreach (GridViewRow oItemGrid in GrdRavvedimentoOperoso.Rows)
                {
                    if (((CheckBox)oItemGrid.FindControl("chkSelect")).Checked == true)
                    {
                        dImportoNonPagato = StringOperation.FormatDouble(((TextBox)(oItemGrid.FindControl("txtTotaleNonVersato"))).Text);
                        scadenza = ((TextBox)(oItemGrid.FindControl("txtDataScadenza"))).Text;
                        if (DateTime.Parse(scadenza).Month == 12)
                            sRata = "S";

                        Calcolo_Giorni(scadenza, DateTime.Now.ToString(), out GG);

                        //CALCOLO SANZIONE
                        dTotSanzione = CalcolaSanzione(dsSanzioni, GG, dImportoNonPagato);
                        if (dTotSanzione < 0)
                        {
                            string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Sanzioni!');";
                            RegisterScript(strscript, this.GetType());
                            return;
                        }
                        //CALCOLO INTERESSE
                        //Modifica calcolo interessi inserendo calcolo per giorni e semestri
                        //Dipe 11/02/2008
                        dTotInteresse =CalcolaInteressi(dsInteressi,dImportoNonPagato,sRata);
                        if (dTotInteresse < 0)
                        {
                            string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Interessi!');";
                            RegisterScript(strscript, this.GetType());
                            return;
                        }
                        //TOTALE
                        dTotale = dImportoNonPagato + dTotSanzione + dTotInteresse;
                        //VALORIZZAZIONE DELLA GRIGLIA
                        ((TextBox)(oItemGrid.FindControl("txtGGritardo"))).Text = GG.ToString();
                        ((TextBox)(oItemGrid.FindControl("txtTotaleNonVersato"))).Text = CoreUtility.FormattaGrdEuro(dImportoNonPagato);
                        ((TextBox)(oItemGrid.FindControl("txtTotaleSanzioni"))).Text = CoreUtility.FormattaGrdEuro(dTotSanzione);
                        ((TextBox)(oItemGrid.FindControl("txtTotaleInteressi"))).Text = CoreUtility.FormattaGrdEuro(dTotInteresse);
                        ((TextBox)(oItemGrid.FindControl("txtTotale"))).Text = CoreUtility.FormattaGrdEuro(dTotale);
                        switch (oItemGrid.Cells[1].Text)
                        {
                            case "ACCONTO":
                                myAbiPrincAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAbiPrinc"))).Value);
                                myAltriFabAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFab"))).Value);
                                myAltriFabStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFabStato"))).Value);
                                myAreeFabAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFab"))).Value);
                                myAreeFabStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFabStato"))).Value);
                                myTerreniAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgr"))).Value);
                                myTerreniStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgrStato"))));
                                myFabRurAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRur"))).Value);
                                myFabRurStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRurStato"))).Value);
                                myUsoProdAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatD"))).Value);
                                myUsoProdStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatDStato"))).Value);
                                myDetrazAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfDetraz"))).Value);
                                myDovutoAcc = dTotale;
                                if (myAltriFabAcc != 0)
                                {
                                    myAltriFabAcc += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myAreeFabAcc != 0)
                                {
                                    myAreeFabAcc += dTotSanzione + dTotInteresse;
                                break;
                        }
                                else if (myTerreniAcc != 0)
                        {
                            myTerreniAcc += dTotSanzione + dTotInteresse;
                        break;
                    }
                    else if (myFabRurAcc != 0)
                    {
                        myFabRurAcc += dTotSanzione + dTotInteresse;
                    break;
                }
                                else if (myUsoProdAcc != 0)
                {
                    myUsoProdAcc += dTotSanzione + dTotInteresse;
                break;
            }
                                else if (myUsoProdStatoAcc != 0)
            {
                myUsoProdStatoAcc += dTotSanzione + dTotInteresse;
            break;
        }
                                else
                                    myAbiPrincAcc += dTotSanzione + dTotInteresse;
                                break;
                            case "SALDO":
                                myAbiPrincSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAbiPrinc"))).Value);
                                myAltriFabSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFab"))).Value);
                                myAltriFabStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFabStato"))).Value);
                                myAreeFabSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFab"))).Value);
                                myAreeFabStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFabStato"))).Value);
                                myTerreniSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgr"))).Value);
                                myTerreniStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgrStato"))).Value);
                                myFabRurSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRur"))).Value);
                                myFabRurStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRurStato"))).Value);
                                myUsoProdSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatD"))).Value);
                                myUsoProdStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatDStato"))).Value);
                                myDetrazSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfDetraz"))).Value);
                                myDovutoSal = dTotale;
                                if (myAltriFabSal != 0)
                                {
                                    myAltriFabSal += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myAreeFabSal != 0)
                                {
                                    myAreeFabSal += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myTerreniSal != 0)
                                {
                                    myTerreniSal += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myFabRurSal != 0)
                                {
                                    myFabRurSal += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myUsoProdSal != 0)
                                {
                                    myUsoProdSal += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myUsoProdStatoSal != 0)
                                {
                                    myUsoProdStatoSal += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else
                                    myAbiPrincSal += dTotSanzione + dTotInteresse;
                                break;
                            default:
                                myNFab = StringOperation.FormatInt(((HiddenField)(oItemGrid.FindControl("hfNFab"))).Value);
                                myAbiPrincTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAbiPrinc"))).Value);
                                myAltriFabTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFab"))).Value);
                                myAltriFabSatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFabStato"))).Value);
                                myAreeFabTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFab"))).Value);
                                myAreeFabStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFabStato"))).Value);
                                myTerreniTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgr"))).Value);
                                myTerreniStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgrStato"))).Value);
                                myFabRurTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRur"))).Value);
                                myFabRurStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRurStato"))).Value);
                                myUsoProdTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatD"))).Value);
                                myUsoProdStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatDStato"))).Value);
                                myDetrazTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfDetraz"))).Value);
                                myDovutoTot = dTotale;
                                if (myAltriFabTot != 0)
                                {
                                    myAltriFabTot += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myAreeFabTot != 0)
                                {
                                    myAreeFabTot += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myTerreniTot != 0)
                                {
                                    myTerreniTot += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myFabRurTot != 0)
                                {
                                    myFabRurTot += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myUsoProdTot != 0)
                                {
                                    myUsoProdTot += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                else if (myUsoProdStatoTot != 0)
                                {
                                    myUsoProdStatoTot += dTotSanzione + dTotInteresse;
                                    break;
                                }
                                myAbiPrincTot += dTotSanzione + dTotInteresse;
                                break;
                        }
                        if (SetCalcoloFinale() < 0)
                        {
                            string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Sanzioni!');";
                            RegisterScript(strscript, this.GetType());
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.btnCalcolaRO_Click.errore: ", ex);
                Response.Redirect("txt../PaginaErrore.aspx");
            }
        }
        //protected void btnCalcolaRO_Click(object sender, System.EventArgs e)
        //{
        //    DataSet dsSanzioni = new DataSet();
        //    DataSet dsInteressi = new DataSet();
        //    string scadenza = string.Empty;
        //    double dImportoNonPagato;
        //    double dTotSanzione;
        //    double dTotInteresse;
        //    int GG;
        //    double dTotale;
        //    double valoreInteresse = 0;

        //    try
        //    {
        //        if (!this.Controlli(out dsSanzioni, out dsInteressi))
        //        {
        //            string strscript = "GestAlert('a', 'warning', '', '', 'Sanzioni non presenti!');";
        //            RegisterScript(strscript, this.GetType());
        //            return;
        //        }
        //        if (DelCalcoloFinale() < 0)
        //        {
        //            string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Sanzioni!');";
        //            RegisterScript(strscript, this.GetType());
        //            return;
        //        }
        //        foreach (DataRow myRow in dsInteressi.Tables[0].Rows)
        //        {
        //            valoreInteresse = StringOperation.FormatDouble(myRow["TASSO_ANNUALE"]);
        //        }
        //        foreach (GridViewRow oItemGrid in GrdRavvedimentoOperoso.Rows)
        //        {
        //            if (((CheckBox)oItemGrid.FindControl("chkSelect")).Checked == true)
        //            {
        //                dImportoNonPagato = StringOperation.FormatDouble(((TextBox)(oItemGrid.FindControl("txtTotaleNonVersato"))).Text);
        //                scadenza = ((TextBox)(oItemGrid.FindControl("txtDataScadenza"))).Text;
        //                Calcolo_Giorni(scadenza, DateTime.Now.ToString(), out GG);

        //                //CALCOLO SANZIONE
        //                dTotSanzione = CalcolaSanzione(dsSanzioni, GG, dImportoNonPagato);
        //                if (dTotSanzione < 0)
        //                {
        //                    string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Sanzioni!');";
        //                    RegisterScript(strscript, this.GetType());
        //                    return;
        //                }
        //                //CALCOLO INTERESSE
        //                //Modifica calcolo interessi inserendo calcolo per giorni e semestri
        //                //Dipe 11/02/2008
        //                dTotInteresse = (((dImportoNonPagato * (valoreInteresse / 100)) * GG) / 365);
        //                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.CalcolaInteressi.(((dImportoNonPagato * (valoreInteresse / 100)) * GG) / 365)->((" + dImportoNonPagato.ToString() + " * (" + valoreInteresse.ToString() + " / 100)) * " + GG.ToString() + ")");
        //                //TOTALE
        //                dTotale = dImportoNonPagato + dTotSanzione + dTotInteresse;
        //                //VALORIZZAZIONE DELLA GRIGLIA
        //                ((TextBox)(oItemGrid.FindControl("txtGGritardo"))).Text = GG.ToString();
        //                ((TextBox)(oItemGrid.FindControl("txtTotaleNonVersato"))).Text = CoreFormattaGrdEuro(dImportoNonPagato);
        //                ((TextBox)(oItemGrid.FindControl("txtTotaleSanzioni"))).Text = CoreFormattaGrdEuro(dTotSanzione);
        //                ((TextBox)(oItemGrid.FindControl("txtTotaleInteressi"))).Text = CoreFormattaGrdEuro(dTotInteresse);
        //                ((TextBox)(oItemGrid.FindControl("txtTotale"))).Text = CoreFormattaGrdEuro(dTotale);
        //                switch (oItemGrid.Cells[1].Text)
        //                {
        //                    case "ACCONTO":
        //                        myAbiPrincAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAbiPrinc"))).Value);
        //                        myAltriFabAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFab"))).Value);
        //                        myAltriFabStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFabStato"))).Value);
        //                        myAreeFabAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFab"))).Value);
        //                        myAreeFabStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFabStato"))).Value);
        //                        myTerreniAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgr"))).Value);
        //                        myTerreniStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgrStato"))));
        //                        myFabRurAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRur"))).Value);
        //                        myFabRurStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRurStato"))).Value);
        //                        myUsoProdAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatD"))).Value);
        //                        myUsoProdStatoAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatDStato"))).Value);
        //                        myDetrazAcc = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfDetraz"))).Value);
        //                        myDovutoAcc = dTotale;
        //                        if (myAltriFabAcc != 0)
        //                            myAltriFabAcc += dTotSanzione + dTotInteresse;
        //                        else if (myAreeFabAcc != 0)
        //                            myAreeFabAcc += dTotSanzione + dTotInteresse;
        //                        else if (myTerreniAcc != 0)
        //                            myTerreniAcc += dTotSanzione + dTotInteresse;
        //                        else if (myFabRurAcc != 0)
        //                            myFabRurAcc += dTotSanzione + dTotInteresse;
        //                        else if (myUsoProdAcc != 0)
        //                            myUsoProdAcc += dTotSanzione + dTotInteresse;
        //                        else
        //                            myAbiPrincAcc += dTotSanzione + dTotInteresse;
        //                        break;
        //                    case "SALDO":
        //                         myAbiPrincSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAbiPrinc"))).Value);
        //                        myAltriFabSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFab"))).Value);
        //                        myAltriFabStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFabStato"))).Value);
        //                        myAreeFabSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFab"))).Value);
        //                        myAreeFabStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFabStato"))).Value);
        //                        myTerreniSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgr"))).Value);
        //                        myTerreniStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgrStato"))).Value);
        //                        myFabRurSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRur"))).Value);
        //                        myFabRurStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRurStato"))).Value);
        //                        myUsoProdSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatD"))).Value);
        //                        myUsoProdStatoSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatDStato"))).Value);
        //                        myDetrazSal = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfDetraz"))).Value);
        //                        myDovutoSal = dTotale;
        //                        if (myAltriFabSal != 0)
        //                            myAltriFabSal += dTotSanzione + dTotInteresse;
        //                        else if (myAreeFabSal != 0)
        //                            myAreeFabSal += dTotSanzione + dTotInteresse;
        //                        else if (myTerreniSal != 0)
        //                            myTerreniSal += dTotSanzione + dTotInteresse;
        //                        else if (myFabRurSal != 0)
        //                            myFabRurSal += dTotSanzione + dTotInteresse;
        //                        else if (myUsoProdSal != 0)
        //                            myUsoProdSal += dTotSanzione + dTotInteresse;
        //                        else
        //                            myAbiPrincSal += dTotSanzione + dTotInteresse;
        //                        break;
        //                    default: 
        //                        myNFab = StringOperation.FormatInt(((HiddenField)(oItemGrid.FindControl("hfNFab"))).Value);
        //                        myAbiPrincTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAbiPrinc"))).Value);
        //                        myAltriFabTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFab"))).Value);
        //                        myAltriFabSatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAltriFabStato"))).Value);
        //                        myAreeFabTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFab"))).Value);
        //                        myAreeFabStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfAreeFabStato"))).Value);
        //                        myTerreniTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgr"))).Value);
        //                        myTerreniStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfTerrAgrStato"))).Value);
        //                        myFabRurTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRur"))).Value);
        //                        myFabRurStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfFabRurStato"))).Value);
        //                        myUsoProdTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatD"))).Value);
        //                        myUsoProdStatoTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfUsoProdCatDStato"))).Value);
        //                        myDetrazTot = StringOperation.FormatDouble(((HiddenField)(oItemGrid.FindControl("hfDetraz"))).Value);
        //                        myDovutoTot = dTotale;
        //                        if (myAltriFabTot != 0)
        //                            myAltriFabTot += dTotSanzione + dTotInteresse;
        //                        else if (myAreeFabTot != 0)
        //                            myAreeFabTot += dTotSanzione + dTotInteresse;
        //                        else if (myTerreniTot != 0)
        //                            myTerreniTot += dTotSanzione + dTotInteresse;
        //                        else if (myFabRurTot != 0)
        //                            myFabRurTot += dTotSanzione + dTotInteresse;
        //                        else if (myUsoProdTot != 0)
        //                            myUsoProdTot += dTotSanzione + dTotInteresse;
        //                        else
        //                            myAbiPrincTot += dTotSanzione + dTotInteresse;
        //                        break;
        //                }
        //        if (SetCalcoloFinale() < 0)
        //        {
        //            string strscript = "GestAlert('a', 'warning', '', '', 'Errore in calcolo Sanzioni!');";
        //            RegisterScript(strscript, this.GetType());
        //            return;
        //        }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.btnCalcolaRO_Click.errore: ", ex);
        //        Response.Redirect("txt../PaginaErrore.aspx");
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Controlli(out DataSet dsSanz, out DataSet dsInt)
        {
            dsSanz = new DataSet();
            dsInt = new DataSet();
            try
            {
                dsSanz = PopolaGridSanzioni();
                dsInt = PopolaComboInteresse();
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.Controlli.errore: ", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfiguraRO_Click(object sender, System.EventArgs e)
        {
            double DovutoAcc, ImpAbiPrincAcc, ImpTerrAgrAcc, ImpTerrAgrStatoAcc, ImpAltriFabAcc, ImpAltriFabStatoAcc, ImpAreeFabAcc, ImpAreeFabStatoAcc, ImpFabRurAcc, ImpFabRurStatoAcc, ImpUsoProdCatDAcc, ImpUsoProdCatDStatoAcc, ImpDetrazAcc;
            double DovutoSal, ImpAbiPrincSal, ImpTerrAgrSal, ImpTerrAgrStatoSal, ImpAltriFabSal, ImpAltriFabStatoSal, ImpAreeFabSal, ImpAreeFabStatoSal, ImpFabRurSal, ImpFabRurStatoSal, ImpUsoProdCatDSal, ImpUsoProdCatDStatoSal, ImpDetrazSal;
            double DovutoTot, ImpAbiPrincTot, ImpTerrAgrTot, ImpTerrAgrStatoTot, ImpAltriFabTot, ImpAltriFabStatoTot, ImpAreeFabTot, ImpAreeFabStatoTot, ImpFabRurTot, ImpFabRurStatoTot, ImpUsoProdCatDTot, ImpUsoProdCatDStatoTot, ImpDetrazTot;
            double PagAcconto, PagAbiPrincAcc, PagTerrAgrAcc, PagTerrAgrStatoAcc, PagAltriFabAcc, PagAltriFabStatoAcc, PagAreeFabAcc, PagAreeFabStatoAcc, PagFabRurAcc, PagFabRurStatoAcc, PagUsoProdCatDAcc, PagUsoProdCatDStatoAcc, PagDetrazAcc;
            double PagSaldo, PagAbiPrincSal, PagTerrAgrSal, PagTerrAgrStatoSal, PagAltriFabSal, PagAltriFabStatoSal, PagAreeFabSal, PagAreeFabStatoSal, PagFabRurSal, PagFabRurStatoSal, PagUsoProdCatDSal, PagUsoProdCatDStatoSal, PagDetrazSal;
            double PagUS, PagAbiPrincUS, PagTerrAgrUS, PagTerrAgrStatoUS, PagAltriFabUS, PagAltriFabStatoUS, PagAreeFabUS, PagAreeFabStatoUS, PagFabRurUS, PagFabRurStatoUS, PagUsoProdCatDUS, PagUsoProdCatDStatoUS, PagDetrazUS;
            int NumFab;
            string sScript = string.Empty;
            string sTributo = Costanti.TRIBUTO_ICI;
            try
            {
                DovutoAcc = ImpAbiPrincAcc = ImpTerrAgrAcc = ImpTerrAgrStatoAcc = ImpAltriFabAcc = ImpAltriFabStatoAcc = ImpAreeFabAcc = ImpAreeFabStatoAcc = ImpFabRurAcc = ImpFabRurStatoAcc = ImpUsoProdCatDAcc = ImpUsoProdCatDStatoAcc = ImpDetrazAcc = 0;
                DovutoSal = ImpAbiPrincSal = ImpTerrAgrSal = ImpTerrAgrStatoSal = ImpAltriFabSal = ImpAltriFabStatoSal = ImpAreeFabSal = ImpAreeFabStatoSal = ImpFabRurSal = ImpFabRurStatoSal = ImpUsoProdCatDSal = ImpUsoProdCatDStatoSal = ImpDetrazSal = 0;
                DovutoTot = ImpAbiPrincTot = ImpTerrAgrTot = ImpTerrAgrStatoTot = ImpAltriFabTot = ImpAltriFabStatoTot = ImpAreeFabTot = ImpAreeFabStatoTot = ImpFabRurTot = ImpFabRurStatoTot = ImpUsoProdCatDTot = ImpUsoProdCatDStatoTot = ImpDetrazTot = 0;
                PagAcconto = PagAbiPrincAcc = PagTerrAgrAcc = PagTerrAgrStatoAcc = PagAltriFabAcc = PagAltriFabStatoAcc = PagAreeFabAcc = PagAreeFabStatoAcc = PagFabRurAcc = PagFabRurStatoAcc = PagUsoProdCatDAcc = PagUsoProdCatDStatoAcc = PagDetrazAcc = 0;
                PagSaldo = PagAbiPrincSal = PagTerrAgrSal = PagTerrAgrStatoSal = PagAltriFabSal = PagAltriFabStatoSal = PagAreeFabSal = PagAreeFabStatoSal = PagFabRurSal = PagFabRurStatoSal = PagUsoProdCatDSal = PagUsoProdCatDStatoSal = PagDetrazSal = 0;
                PagUS = PagAbiPrincUS = PagTerrAgrUS = PagTerrAgrStatoUS = PagAltriFabUS = PagAltriFabStatoUS = PagAreeFabUS = PagAreeFabStatoUS = PagFabRurUS = PagFabRurStatoUS = PagUsoProdCatDUS = PagUsoProdCatDStatoUS = PagDetrazUS = 0;
                NumFab = 0;
                //prelevo il dovuto
                if (chkTASI.Checked)
                    sTributo = Costanti.TRIBUTO_TASI;
                DataTable dtDovuto = new Database.TpSituazioneFinaleIci().GetBollettinoICI(hdIdContribuente.Value, txtAnno.Text, ConstWrapper.CodiceEnte, sTributo, false);
                foreach (DataRow myRow in dtDovuto.Rows)
                {
                    NumFab = StringOperation.FormatInt(myRow["NUMFABB"]);
                    ImpAbiPrincAcc = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO"]);
                    ImpTerrAgrAcc = StringOperation.FormatDouble(myRow["ICI_DOVUTA_TERRENI_ACCONTO"]);
                    ImpTerrAgrStatoAcc = StringOperation.FormatDouble(myRow["IMP_TERRENI_ACC_STATALE"]);
                    ImpAltriFabAcc = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ALTRI_FABBRICATI_ACCONTO"]);
                    ImpAltriFabStatoAcc = StringOperation.FormatDouble(myRow["IMP_ALTRI_FAB_ACC_STATALE"]);
                    ImpAreeFabAcc = StringOperation.FormatDouble(myRow["ICI_DOVUTA_AREE_FABBRICABILI_ACCONTO"]);
                    ImpAreeFabStatoAcc = StringOperation.FormatDouble(myRow["IMP_AREE_FAB_ACC_STATALE"]);
                    ImpFabRurAcc = StringOperation.FormatDouble(myRow["IMP_FABRURUSOSTRUM_ACC"]);
                    ImpFabRurStatoAcc = StringOperation.FormatDouble(myRow["IMP_FABRURUSOSTRUM_ACC_STATALE"]);
                    ImpUsoProdCatDAcc = StringOperation.FormatDouble(myRow["IMP_USOPRODCATD_ACC"]);
                    ImpUsoProdCatDStatoAcc = StringOperation.FormatDouble(myRow["IMP_USOPRODCATD_ACC_STATALE"]);
                    ImpDetrazAcc = StringOperation.FormatDouble(myRow["ICI_DOVUTA_DETRAZIONE_ACCONTO"]);
                    ImpAbiPrincSal = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO"]);
                    ImpTerrAgrSal = StringOperation.FormatDouble(myRow["ICI_DOVUTA_TERRENI_SALDO"]);
                    ImpTerrAgrStatoSal = StringOperation.FormatDouble(myRow["IMP_TERRENI_SAL_STATALE"]);
                    ImpAltriFabSal = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ALTRI_FABBRICATI_SALDO"]);
                    ImpAltriFabStatoSal = StringOperation.FormatDouble(myRow["IMP_ALTRI_FAB_SAL_STATALE"]);
                    ImpAreeFabSal = StringOperation.FormatDouble(myRow["ICI_DOVUTA_AREE_FABBRICABILI_SALDO"]);
                    ImpAreeFabStatoSal = StringOperation.FormatDouble(myRow["IMP_AREE_FAB_SAL_STATALE"]);
                    ImpFabRurSal = StringOperation.FormatDouble(myRow["IMP_FABRURUSOSTRUM_SAL"]);
                    ImpFabRurStatoSal = StringOperation.FormatDouble(myRow["IMP_FABRURUSOSTRUM_SAL_STATALE"]);
                    ImpUsoProdCatDSal = StringOperation.FormatDouble(myRow["IMP_USOPRODCATD_SAL"]);
                    ImpUsoProdCatDStatoSal = StringOperation.FormatDouble(myRow["IMP_USOPRODCATD_SAL_STATALE"]);
                    ImpDetrazSal = StringOperation.FormatDouble(myRow["ICI_DOVUTA_DETRAZIONE_SALDO"]);
                    ImpAbiPrincTot = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE"]);
                    ImpTerrAgrTot = StringOperation.FormatDouble(myRow["ICI_DOVUTA_TERRENI_TOTALE"]);
                    ImpTerrAgrStatoTot = StringOperation.FormatDouble(myRow["IMP_TERRENI_TOT_STATALE"]);
                    ImpAltriFabTot = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE"]);
                    ImpAltriFabStatoTot = StringOperation.FormatDouble(myRow["IMP_ALTRI_FAB_TOT_STATALE"]);
                    ImpAreeFabTot = StringOperation.FormatDouble(myRow["ICI_DOVUTA_AREE_FABBRICABILI_TOTALE"]);
                    ImpAreeFabStatoTot = StringOperation.FormatDouble(myRow["IMP_AREE_FAB_TOT_STATALE"]);
                    ImpFabRurTot = StringOperation.FormatDouble(myRow["IMP_FABRURUSOSTRUM_TOT"]);
                    ImpFabRurStatoTot = StringOperation.FormatDouble(myRow["IMP_FABRURUSOSTRUM_TOT_STATALE"]);
                    ImpUsoProdCatDTot = StringOperation.FormatDouble(myRow["IMP_USOPRODCATD_TOT"]);
                    ImpUsoProdCatDStatoTot = StringOperation.FormatDouble(myRow["IMP_USOPRODCATD_TOT_STATALE"]);
                    ImpDetrazTot = StringOperation.FormatDouble(myRow["ICI_DOVUTA_DETRAZIONE_TOTALE"]);
                    DovutoTot = StringOperation.FormatDouble(myRow["ICI_DOVUTA_TOTALE"]);
                    DovutoAcc = StringOperation.FormatDouble(myRow["ICI_DOVUTA_ACCONTO"]);
                    DovutoSal = StringOperation.FormatDouble(myRow["ICI_DOVUTA_SALDO"]);
                }
                //prelevo i versamenti per l'anno
                DataView dvVersamenti = new VersamentiView().List(ConstWrapper.StringConnection, sTributo, int.Parse(hdIdContribuente.Value), string.Empty, string.Empty, string.Empty, string.Empty, txtAnno.Text, ConstWrapper.CodiceEnte, -1, -1, -1, 0, string.Empty, string.Empty,string.Empty);
                foreach (DataRowView myRow in dvVersamenti)
                {
                    switch (CoreUtility.FormattaGrdAccontoSaldo(myRow["Saldo"], myRow["Acconto"]))
                    {
                        case "Acconto":
                            PagAbiPrincAcc += StringOperation.FormatDouble(myRow["ImportoAbitazPrincipale"]);
                            PagTerrAgrAcc += StringOperation.FormatDouble(myRow["ImpoTerreni"]);
                            PagTerrAgrStatoAcc += StringOperation.FormatDouble(myRow["IMPORTOTERRENISTATALE"]);
                            PagAltriFabAcc += StringOperation.FormatDouble(myRow["ImportoAltriFabbric"]);
                            PagAltriFabStatoAcc += StringOperation.FormatDouble(myRow["IMPORTOALTRIFABBRICSTATALE"]);
                            PagAreeFabAcc += StringOperation.FormatDouble(myRow["ImportoAreeFabbric"]);
                            PagAreeFabStatoAcc += StringOperation.FormatDouble(myRow["IMPORTOAREEFABBRICSTATALE"]);
                            PagFabRurAcc += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUM"]);
                            PagFabRurStatoAcc += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUMSTATALE"]);
                            PagUsoProdCatDAcc += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATD"]);
                            PagUsoProdCatDStatoAcc += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATDSTATALE"]);
                            PagDetrazAcc += StringOperation.FormatDouble(myRow["DetrazioneAbitazPrincipale"]);
                            PagAcconto += StringOperation.FormatDouble(myRow["ImportoPagato"]);

                            PagAbiPrincUS += StringOperation.FormatDouble(myRow["ImportoAbitazPrincipale"]);
                            PagTerrAgrUS += StringOperation.FormatDouble(myRow["ImpoTerreni"]);
                            PagTerrAgrStatoUS += StringOperation.FormatDouble(myRow["IMPORTOTERRENISTATALE"]);
                            PagAltriFabUS += StringOperation.FormatDouble(myRow["ImportoAltriFabbric"]);
                            PagAltriFabStatoUS += StringOperation.FormatDouble(myRow["IMPORTOALTRIFABBRICSTATALE"]);
                            PagAreeFabUS += StringOperation.FormatDouble(myRow["ImportoAreeFabbric"]);
                            PagAreeFabStatoUS += StringOperation.FormatDouble(myRow["IMPORTOAREEFABBRICSTATALE"]);
                            PagFabRurUS += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUM"]);
                            PagFabRurStatoUS += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUMSTATALE"]);
                            PagUsoProdCatDUS += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATD"]);
                            PagUsoProdCatDStatoUS += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATDSTATALE"]);
                            PagDetrazUS += StringOperation.FormatDouble(myRow["DetrazioneAbitazPrincipale"]);
                            PagUS += StringOperation.FormatDouble(myRow["ImportoPagato"]);
                            break;
                        case "Saldo":
                            PagAbiPrincSal += StringOperation.FormatDouble(myRow["ImportoAbitazPrincipale"]);
                            PagTerrAgrSal += StringOperation.FormatDouble(myRow["ImpoTerreni"]);
                            PagTerrAgrStatoSal += StringOperation.FormatDouble(myRow["IMPORTOTERRENISTATALE"]);
                            PagAltriFabSal += StringOperation.FormatDouble(myRow["ImportoAltriFabbric"]);
                            PagAltriFabStatoSal += StringOperation.FormatDouble(myRow["IMPORTOALTRIFABBRICSTATALE"]);
                            PagAreeFabSal += StringOperation.FormatDouble(myRow["ImportoAreeFabbric"]);
                            PagAreeFabStatoSal += StringOperation.FormatDouble(myRow["IMPORTOAREEFABBRICSTATALE"]);
                            PagFabRurSal += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUM"]);
                            PagFabRurStatoSal += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUMSTATALE"]);
                            PagUsoProdCatDSal += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATD"]);
                            PagUsoProdCatDStatoSal += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATDSTATALE"]);
                            PagDetrazSal += StringOperation.FormatDouble(myRow["DetrazioneAbitazPrincipale"]);
                            PagSaldo += StringOperation.FormatDouble(myRow["ImportoPagato"]);

                            PagAbiPrincUS += StringOperation.FormatDouble(myRow["ImportoAbitazPrincipale"]);
                            PagTerrAgrUS += StringOperation.FormatDouble(myRow["ImpoTerreni"]);
                            PagTerrAgrStatoUS += StringOperation.FormatDouble(myRow["IMPORTOTERRENISTATALE"]);
                            PagAltriFabUS += StringOperation.FormatDouble(myRow["ImportoAltriFabbric"]);
                            PagAltriFabStatoUS += StringOperation.FormatDouble(myRow["IMPORTOALTRIFABBRICSTATALE"]);
                            PagAreeFabUS += StringOperation.FormatDouble(myRow["ImportoAreeFabbric"]);
                            PagAreeFabStatoUS += StringOperation.FormatDouble(myRow["IMPORTOAREEFABBRICSTATALE"]);
                            PagFabRurUS += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUM"]);
                            PagFabRurStatoUS += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUMSTATALE"]);
                            PagUsoProdCatDUS += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATD"]);
                            PagUsoProdCatDStatoUS += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATDSTATALE"]);
                            PagDetrazUS += StringOperation.FormatDouble(myRow["DetrazioneAbitazPrincipale"]);
                            PagUS += StringOperation.FormatDouble(myRow["ImportoPagato"]);
                            break;
                        default:
                            PagAbiPrincUS += StringOperation.FormatDouble(myRow["ImportoAbitazPrincipale"]);
                            PagTerrAgrUS += StringOperation.FormatDouble(myRow["ImpoTerreni"]);
                            PagTerrAgrStatoUS += StringOperation.FormatDouble(myRow["IMPORTOTERRENISTATALE"]);
                            PagAltriFabUS += StringOperation.FormatDouble(myRow["ImportoAltriFabbric"]);
                            PagAltriFabStatoUS += StringOperation.FormatDouble(myRow["IMPORTOALTRIFABBRICSTATALE"]);
                            PagAreeFabUS += StringOperation.FormatDouble(myRow["ImportoAreeFabbric"]);
                            PagAreeFabStatoUS += StringOperation.FormatDouble(myRow["IMPORTOAREEFABBRICSTATALE"]);
                            PagFabRurUS += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUM"]);
                            PagFabRurStatoUS += StringOperation.FormatDouble(myRow["IMPORTOFABRURUSOSTRUMSTATALE"]);
                            PagUsoProdCatDUS += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATD"]);
                            PagUsoProdCatDStatoUS += StringOperation.FormatDouble(myRow["IMPORTOUSOPRODCATDSTATALE"]);
                            PagDetrazUS += StringOperation.FormatDouble(myRow["DetrazioneAbitazPrincipale"]);
                            PagUS += StringOperation.FormatDouble(myRow["ImportoPagato"]);
                            break;
                    }
                }
                if ((DovutoTot -  PagUS) > 0)
                {
                    DataTable TabellaRO = new DataTable();

                    TabellaRO.Columns.Add("ENTE");
                    TabellaRO.Columns.Add("ANNO");
                    TabellaRO.Columns.Add("COD_CONTRIBUENTE");
                    TabellaRO.Columns.Add("TIPOLOGIA_VERSAMENTO");
                    TabellaRO.Columns.Add("TIPO_VERSAMENTO");
                    TabellaRO.Columns.Add("DATA_SCADENZA");
                    TabellaRO.Columns.Add("GIORNI_RITARDO");
                    TabellaRO.Columns.Add("IMPORTO_TOTALE");
                    TabellaRO.Columns.Add("IMPORTO_PAGATO");
                    TabellaRO.Columns.Add("NFab");
                    TabellaRO.Columns.Add("AbiPrinc");
                    TabellaRO.Columns.Add("TerrAgr");
                    TabellaRO.Columns.Add("TerrAgrStato");
                    TabellaRO.Columns.Add("AltriFab");
                    TabellaRO.Columns.Add("AltriFabStato");
                    TabellaRO.Columns.Add("AreeFab");
                    TabellaRO.Columns.Add("AreeFabStato");
                    TabellaRO.Columns.Add("FabRur");
                    TabellaRO.Columns.Add("FabRurStato");
                    TabellaRO.Columns.Add("UsoProdCatD");
                    TabellaRO.Columns.Add("UsoProdCatDStato");
                    TabellaRO.Columns.Add("Detraz");
                    TabellaRO.Columns.Add("IMPORTO_NON_VERSATO");
                    TabellaRO.Columns.Add("IMPORTO_SANZIONI");
                    TabellaRO.Columns.Add("IMPORTO_INTERESSI");

                    object[] ArrCampi;
                    //ACCONTO
                    ArrCampi = new object[25];
                    int x = 0;
                    ArrCampi[x] = ConstWrapper.CodiceEnte; //ENTE
                    x += 1; ArrCampi[x] = txtAnno.Text;//ANNO
                    x += 1; ArrCampi[x] = hdIdContribuente.Value;//COD_CONTRIBUENTE
                    x += 1; ArrCampi[x] = "ACCONTO";//TIPOLOGIA_VERSAMENTO
                    x += 1; ArrCampi[x] = "1";//TIPO_VERSAMENTO
                    x += 1; ArrCampi[x] = "16/06/" + txtAnno.Text;//DATA_SCADENZA		
                    x += 1; ArrCampi[x] = "";//GIORNI_RITARDO
                    x += 1; ArrCampi[x] = (DovutoAcc).ToString("#,##0.00");//IMPORTO_TOTALE
                    x += 1; ArrCampi[x] = (PagAcconto).ToString("#,##0.00");//IMPORTO_PAGATO		
                    x += 1; ArrCampi[x] = NumFab;//NFAB
                    x += 1; ArrCampi[x] = (ImpAbiPrincAcc - PagAbiPrincAcc).ToString("#,##0.00");//AbiPrinc
                    x += 1; ArrCampi[x] = (ImpTerrAgrAcc - PagTerrAgrAcc).ToString("#,##0.00");//TerrAgr
                    x += 1; ArrCampi[x] = (ImpTerrAgrStatoAcc - PagTerrAgrStatoAcc).ToString("#,##0.00");//TerrAgrStato
                    x += 1; ArrCampi[x] = (ImpAltriFabAcc - PagAltriFabAcc).ToString("#,##0.00");//AltriFab
                    x += 1; ArrCampi[x] = (ImpAltriFabStatoAcc - PagAltriFabStatoAcc).ToString("#,##0.00");//AltriFabStato
                    x += 1; ArrCampi[x] = (ImpAreeFabAcc - PagAreeFabAcc).ToString("#,##0.00");//AreeFab
                    x += 1; ArrCampi[x] = (ImpAreeFabStatoAcc - PagAreeFabStatoAcc).ToString("#,##0.00");//AreeFabStato
                    x += 1; ArrCampi[x] = (ImpFabRurAcc - PagFabRurAcc).ToString("#,##0.00");//FabRur
                    x += 1; ArrCampi[x] = (ImpFabRurStatoAcc - PagFabRurStatoAcc).ToString("#,##0.00");//FabRurStato
                    x += 1; ArrCampi[x] = (ImpUsoProdCatDAcc - PagUsoProdCatDAcc).ToString("#,##0.00");//UsoProdCatD
                    x += 1; ArrCampi[x] = (ImpUsoProdCatDStatoAcc - PagUsoProdCatDStatoAcc).ToString("#,##0.00");//UsoProdCatDStato
                    x += 1; ArrCampi[x] = (ImpDetrazAcc - PagDetrazAcc).ToString("#,##0.00");//Detraz");
                    x += 1; ArrCampi[x] = (DovutoAcc - PagAcconto).ToString("#,##0.00");//IMPORTO_NON_VERSATO
                    x += 1; ArrCampi[x] = "";//IMPORTO_SANZIONI
                    x += 1; ArrCampi[x] = "";//IMPORTO_INTERESSI
                    TabellaRO.Rows.Add(ArrCampi);

                    //SALDO
                    ArrCampi = new object[25];
                    x = 0; ArrCampi[x] = ConstWrapper.CodiceEnte; //ENTE
                    x += 1; ArrCampi[x] = txtAnno.Text;//ANNO
                    x += 1; ArrCampi[x] = hdIdContribuente.Value;//COD_CONTRIBUENTE
                    x += 1; ArrCampi[x] = "SALDO";//TIPOLOGIA_VERSAMENTO
                    x += 1; ArrCampi[x] = "2";//TIPO_VERSAMENTO
                    x += 1; ArrCampi[x] = "16/12/" + txtAnno.Text;//DATA_SCADENZA		
                    x += 1; ArrCampi[x] = "";//GIORNI_RITARDO	
                    x += 1; ArrCampi[x] = (DovutoSal).ToString("#,##0.00");//IMPORTO_TOTALE
                    x += 1; ArrCampi[x] = (PagSaldo).ToString("#,##0.00");//IMPORTO_PAGATO
                    x += 1; ArrCampi[x] = NumFab;//NFAB
                    x += 1; ArrCampi[x] = (ImpAbiPrincSal - PagAbiPrincSal).ToString("#,##0.00");//AbiPrinc
                    x += 1; ArrCampi[x] = (ImpTerrAgrSal - PagTerrAgrSal).ToString("#,##0.00");//TerrAgr
                    x += 1; ArrCampi[x] = (ImpTerrAgrStatoSal - PagTerrAgrStatoSal).ToString("#,##0.00");//TerrAgrStato
                    x += 1; ArrCampi[x] = (ImpAltriFabSal - PagAltriFabSal).ToString("#,##0.00");//AltriFab
                    x += 1; ArrCampi[x] = (ImpAltriFabStatoSal - PagAltriFabStatoSal).ToString("#,##0.00");//AltriFabStato
                    x += 1; ArrCampi[x] = (ImpAreeFabSal - PagAreeFabSal).ToString("#,##0.00");//AreeFab
                    x += 1; ArrCampi[x] = (ImpAreeFabStatoSal - PagAreeFabStatoSal).ToString("#,##0.00");//AreeFabStato
                    x += 1; ArrCampi[x] = (ImpFabRurSal - PagFabRurSal).ToString("#,##0.00");//FabRur
                    x += 1; ArrCampi[x] = (ImpFabRurStatoSal - PagFabRurStatoSal).ToString("#,##0.00");//FabRurStato
                    x += 1; ArrCampi[x] = (ImpUsoProdCatDSal - PagUsoProdCatDSal).ToString("#,##0.00");//UsoProdCatD
                    x += 1; ArrCampi[x] = (ImpUsoProdCatDStatoSal - PagUsoProdCatDStatoSal).ToString("#,##0.00");//UsoProdCatDStato
                    x += 1; ArrCampi[x] = (ImpDetrazSal - PagDetrazSal).ToString("#,##0.00");//Detraz");
                    x += 1; ArrCampi[x] = (DovutoSal - PagSaldo).ToString("#,##0.00");//IMPORTO_NON_VERSATO
                    x += 1; ArrCampi[x] = "";//IMPORTO_SANZIONI
                    x += 1; ArrCampi[x] = "";//IMPORTO_INTERESSI
                    TabellaRO.Rows.Add(ArrCampi);

                    //UNICA SOLUZIONE
                    ArrCampi = new object[25];
                    x = 0; ArrCampi[x] = ConstWrapper.CodiceEnte; //ENTE
                    x += 1; ArrCampi[x] = txtAnno.Text;//ANNO
                    x += 1; ArrCampi[x] = hdIdContribuente.Value;//COD_CONTRIBUENTE
                    x += 1; ArrCampi[x] = "UNICA SOLUZIONE";//TIPOLOGIA_VERSAMENTO
                    x += 1; ArrCampi[x] = "3";//TIPO_VERSAMENTO
                    x += 1; ArrCampi[x] = "16/06/" + txtAnno.Text;//DATA_SCADENZA		
                    x += 1; ArrCampi[x] = "";//GIORNI_RITARDO	
                    x += 1; ArrCampi[x] = (DovutoTot).ToString("#,##0.00");//IMPORTO_TOTALE
                    x += 1; ArrCampi[x] = (PagUS).ToString("#,##0.00");//IMPORTO_PAGATO
                    x += 1; ArrCampi[x] = NumFab;//NFAB
                    x += 1; ArrCampi[x] = (ImpAbiPrincTot - PagAbiPrincUS).ToString("#,##0.00");//AbiPrinc
                    x += 1; ArrCampi[x] = (ImpTerrAgrTot - PagTerrAgrUS).ToString("#,##0.00");//TerrAgr
                    x += 1; ArrCampi[x] = (ImpTerrAgrStatoTot - PagTerrAgrStatoUS).ToString("#,##0.00");//TerrAgrStato
                    x += 1; ArrCampi[x] = (ImpAltriFabTot - PagAltriFabUS).ToString("#,##0.00");//AltriFab
                    x += 1; ArrCampi[x] = (ImpAltriFabStatoTot - PagAltriFabStatoUS).ToString("#,##0.00");//AltriFabStato
                    x += 1; ArrCampi[x] = (ImpAreeFabTot - PagAreeFabUS).ToString("#,##0.00");//AreeFab
                    x += 1; ArrCampi[x] = (ImpAreeFabStatoTot - PagAreeFabStatoUS).ToString("#,##0.00");//AreeFabStato
                    x += 1; ArrCampi[x] = (ImpFabRurTot - PagFabRurUS).ToString("#,##0.00");//FabRur
                    x += 1; ArrCampi[x] = (ImpFabRurStatoTot - PagFabRurStatoUS).ToString("#,##0.00");//FabRurStato
                    x += 1; ArrCampi[x] = (ImpUsoProdCatDTot - PagUsoProdCatDUS).ToString("#,##0.00");//UsoProdCatD
                    x += 1; ArrCampi[x] = (ImpUsoProdCatDStatoTot - PagUsoProdCatDStatoUS).ToString("#,##0.00");//UsoProdCatDStato
                    x += 1; ArrCampi[x] = (ImpDetrazTot - PagDetrazUS).ToString("#,##0.00");//Detraz");
                    x += 1; ArrCampi[x] = (DovutoTot - PagUS).ToString("#,##0.00");//IMPORTO_NON_VERSATO
                    x += 1; ArrCampi[x] = "";//IMPORTO_SANZIONI
                    x += 1; ArrCampi[x] = "";//IMPORTO_INTERESSI
                    TabellaRO.Rows.Add(ArrCampi);

                    GrdRavvedimentoOperoso.DataSource = TabellaRO;
                    GrdRavvedimentoOperoso.DataBind();

                    PanelRO.Visible = true;
                }
                else
                {
                    sScript = "GestAlert('a', 'warning', '', '', 'Pagato maggiore di dovuto. Impossibile proseguire!');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.btnConfiguraRO_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #region "Griglie"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowDataBound(Object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((TextBox)(e.Row.FindControl("txtTotaleNonVersato"))).Enabled = false;
                    ((CheckBox)(e.Row.FindControl("chkSelect"))).Attributes.Add("OnClick", "AbilitaImporto('" + ((CheckBox)(e.Row.FindControl("chkSelect"))).ClientID + "','" + ((TextBox)(e.Row.FindControl("txtTotaleNonVersato"))).ClientID + "');");
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.GrdRowDataBound.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime startDate, DateTime endDate)
        {
            long dateDiffVal = 0;

            System.Globalization.Calendar cal = Thread.CurrentThread.CurrentCulture.Calendar;

            TimeSpan ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            try
            {
                switch (interval)
                {
                    case DateInterval.Day:
                        dateDiffVal = (long)ts.TotalDays;
                        break;
                    case DateInterval.Hour:
                        dateDiffVal = (long)ts.TotalHours;
                        break;
                    case DateInterval.Minute:
                        dateDiffVal = (long)ts.TotalMinutes;
                        break;
                    case DateInterval.Month:
                        dateDiffVal = (long)(((cal.GetYear(endDate)
                            - cal.GetYear(startDate)) * 12
                            + cal.GetMonth(endDate))
                            - cal.GetMonth(startDate));
                        break;
                    case DateInterval.Quarter:
                        dateDiffVal = (long)((((cal.GetYear(endDate)
                            - cal.GetYear(startDate)) * 4)
                            + ((cal.GetMonth(endDate) - 1) / 3))
                            - ((cal.GetMonth(startDate) - 1) / 3));
                        break;
                    case DateInterval.Second:
                        dateDiffVal = (long)ts.TotalSeconds;
                        break;
                    case DateInterval.Year:
                        dateDiffVal = (long)(cal.GetYear(endDate) - cal.GetYear(startDate));
                        break;
                }
                return dateDiffVal;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.GrdRowDataBound.errore: ", ex);
                throw ex;
            }
        }
        /// <summary>
        ///		*****************************************
        ///		Scopo :    Questa procedura permette di determinare il numero dei semestri
        ///		           in acconto e saldo compresi in un periodo
        ///		Input :    dataverifca = data sino alla quale si deve effettuare il conteggio
        ///		           annorif = anno di riferimento dal quale parte in controllo
        ///		           sal_acc_div = (se true) Indica se i semestri a saldo e acconto devono essere diversi
        ///		
        ///		Output :   semacconto = numero di semestri relativi all'acconto
        ///		semsaldo = numero di semestri relativi a saldo
        ///		
        ///		*****************************************
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="al"></param>
        /// <param name="sem"></param>
        public void Calcolo_Semestri(string dal, string al, out int sem)
        {
            //int semestri;
            DateTime strDAL, strAL;

            strDAL = DateTime.Parse(dal);
            strAL = DateTime.Parse(al);

            sem = (int)(DateDiff(DateInterval.Month, strDAL, strAL) / 6);
        }
        /// <summary>
        ///		*****************************************
        ///		Scopo :    Questa procedura permette di determinare il numero dei giorni
        ///		           in acconto e saldo compresi in un periodo
        ///		Input :    dataverifca = data sino alla quale si deve effettuare il conteggio
        ///		           annorif = anno di riferimento dal quale parte in controllo
        ///		           sal_acc_div = (se true) Indica se i giorni a saldo e acconto devono essere diversi
        ///		
        ///		Output :   ggcconto = numero di giorni relativi all'acconto
        ///		ggsaldo = numero di giorni relativi a saldo
        ///		
        ///		*****************************************
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="al"></param>
        /// <param name="GG"></param>
        public void Calcolo_Giorni(string dal, string al, out int GG)
        {
            GG = 1;
            try
            {
                DateTime strDAL, strAL;

                strDAL = DateTime.Parse(dal);
                strAL = DateTime.Parse(al);

                GG = (int)(DateDiff(DateInterval.Day, strDAL, strAL));
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.Calcolo_Giorni.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }

        // FUNZIONI PER LA STAMPA IN EXCEL
        private DataSet CreateDataSetRavvedimentoOperoso()
        {
            DataSet dsTmp = new DataSet();

            dsTmp.Tables.Add("RAVVEDIMENTOOPEROSO");
            dsTmp.Tables["RAVVEDIMENTOOPEROSO"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["RAVVEDIMENTOOPEROSO"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["RAVVEDIMENTOOPEROSO"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["RAVVEDIMENTOOPEROSO"].Columns.Add("").DataType = typeof(string);
            dsTmp.Tables["RAVVEDIMENTOOPEROSO"].Columns.Add("").DataType = typeof(string);
            return dsTmp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="05/11/2020">
        /// devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
        /// </revision>
        /// </revisionHistory>
        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] oGruppoRet;
            string myTributo=Costanti.TRIBUTO_ICI;
            try
            {
                if (chkTASI.Checked)
                    myTributo = Costanti.TRIBUTO_TASI;
                // faccio partire l'elaborazione chiamo il servizio di elaborazione delle stampe massive.
                Type typeofRI = typeof(IElaborazioneStampeICI);
                string[]TipologieEsclusione=null;
                DataTable DtElaborazione = new TpSituazioneFinaleIci().GetCalcoloICISoggetti(ConstWrapper.CodiceEnte, hdIdContribuente.Value, txtAnno.Text, ConstWrapper.CodiceTributo);
                Session["TblDaStampare"] = DtElaborazione;
                int[,] ContribuentiDaElaborare = new ElaborazioneDocumenti.ElaborazioneDocumenti().GetArrayContribuenti();
                IElaborazioneStampeICI remObject = (IElaborazioneStampeICI)Activator.GetObject(typeofRI, ConstWrapper.UrlServizioElaborazioneDocumentiICI.ToString());
                oGruppoRet = remObject.ElaborazioneMassivaDocumenti(ConstWrapper.DBType, "RVOP", ContribuentiDaElaborare, int.Parse(txtAnno.Text), ConstWrapper.CodiceEnte, "-1",TipologieEsclusione, ConstWrapper.StringConnection, ConstWrapper.StringConnectionOPENgov, ConstWrapper.StringConnectionAnagrafica, ConstWrapper.PathStampe, ConstWrapper.PathVirtualStampe, 1, "PROVA", "BOLLETTINISTANDARD", "RAVVEDIMENTO", "F24", true, true, false, 2,false, true, string.Empty,myTributo);
                //*** ***
                if (oGruppoRet != null)
                {
                    // se viene restituito il gruppo documenti apro il popup
                    Session["StampaPuntuale"] = oGruppoRet[0];
                    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
                    strBuilder.Append("ApriPopUpStampaDocumenti();");
                    RegisterScript(strBuilder.ToString(), this.GetType());
                }
                else
                {
                    System.Text.StringBuilder oStrBuild = new System.Text.StringBuilder();
                    oStrBuild.Append("GestAlert('a', 'warning', '', '', 'Non sono stati elaborati correttamente i documenti selezionati.')");
                    RegisterScript(oStrBuild.ToString(), this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.btnExcel_Click.errore: ", ex);
                Response.Redirect("../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int SetCalcoloFinale()
        {
            DataSet dsMyDati;
            string sSQL;
            int retVal=0;
            try
            {
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_CALCOLO_FINALE_ICI_INSERT", "ANNO"
                                , "COD_ENTE"
                                , "COD_CONTRIBUENTE"
                                , "ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO"
                                , "ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO"
                                , "ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE"
                                , "ICI_DOVUTA_ALTRI_FABBRICATI_ACCONTO"
                                , "ICI_DOVUTA_ALTRI_FABBRICATI_SALDO"
                                , "ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE"
                                , "ICI_DOVUTA_AREE_FABBRICABILI_ACCONTO"
                                , "ICI_DOVUTA_AREE_FABBRICABILI_SALDO"
                                , "ICI_DOVUTA_AREE_FABBRICABILI_TOTALE"
                                , "ICI_DOVUTA_TERRENI_ACCONTO"
                                , "ICI_DOVUTA_TERRENI_SALDO"
                                , "ICI_DOVUTA_TERRENI_TOTALE"
                                , "ICI_DOVUTA_DETRAZIONE_ACCONTO"
                                , "ICI_DOVUTA_DETRAZIONE_SALDO"
                                , "ICI_DOVUTA_DETRAZIONE_TOTALE"
                                , "ICI_DOVUTA_ACCONTO_SENZA_ARROTONDAMENTO"
                                , "ICI_DOVUTA_SALDO_SENZA_ARROTONDAMENTO"
                                , "ICI_DOVUTA_TOTALE_SENZA_ARROTONDAMENTO"
                                , "ARROTONDAMENTO_ICI_DOVUTA_ACCONTO"
                                , "ARROTONDAMENTO_ICI_DOVUTA_SALDO"
                                , "ARROTONDAMENTO_ICI_DOVUTA_TOTALE"
                                , "ICI_DOVUTA_ACCONTO"
                                , "ICI_DOVUTA_SALDO"
                                , "ICI_DOVUTA_TOTALE"
                                , "PROGRESSIVO_ELABORAZIONE"
                                , "CONFERMATO_ICI"
                                , "DOVUTO_FORZATO"
                                , "NUMERO_FABBRICATI"
                                , "DATA_CONFERMA"
                                , "UTENTE_CONFERMA"
                                , "DATA_INSERIMENTO"
                                , "CODELINE_ACCONTO"
                                , "CODELINE_SALDO"
                                , "CODELINE_UNICA_SOLUZIONE"
                                , "DOCUMENTO_ELABORATO"
                                , "ICI_DOVUTA_DETRAZIONE_STATALE_ACCONTO"
                                , "ICI_DOVUTA_DETRAZIONE_STATALE_SALDO"
                                , "ICI_DOVUTA_DETRAZIONE_STATALE_TOTALE"
                                , "IMP_ABI_PRINC_ACC_STATALE"
                                , "IMP_ABI_PRINC_SAL_STATALE"
                                , "IMP_ABI_PRINC_TOT_STATALE"
                                , "IMP_ALTRI_FAB_ACC_STATALE"
                                , "IMP_ALTRI_FAB_SAL_STATALE"
                                , "IMP_ALTRI_FAB_TOT_STATALE"
                                , "IMP_AREE_FAB_ACC_STATALE"
                                , "IMP_AREE_FAB_SAL_STATALE"
                                , "IMP_AREE_FAB_TOT_STATALE"
                                , "IMP_TERRENI_ACC_STATALE"
                                , "IMP_TERRENI_SAL_STATALE"
                                , "IMP_TERRENI_TOT_STATALE"
                                , "DETRAZIONE_ACC_STATALE"
                                , "DETRAZIONE_SAL_STATALE"
                                , "DETRAZIONE_TOT_STATALE"
                                , "TOTALE_ACC_ORIG_STATALE"
                                , "TOTALE_SAL_ORIG_STATALE"
                                , "TOTALE_ORIG_STATALE"
                                , "ARR_TOTALE_ACC_STATALE"
                                , "ARR_TOTALE_SAL_STATALE"
                                , "ARR_TOTALE_STATALE"
                                , "TOTALE_ACC_STATALE"
                                , "TOTALE_SAL_STATALE"
                                , "TOTALE_STATALE"
                                , "IMP_FABRURUSOSTRUM_ACC"
                                , "IMP_FABRURUSOSTRUM_SAL"
                                , "IMP_FABRURUSOSTRUM_TOT"
                                , "IMP_FABRURUSOSTRUM_ACC_STATALE"
                                , "IMP_FABRURUSOSTRUM_SAL_STATALE"
                                , "IMP_FABRURUSOSTRUM_TOT_STATALE"
                                , "IMP_USOPRODCATD_ACC"
                                , "IMP_USOPRODCATD_SAL"
                                , "IMP_USOPRODCATD_TOT"
                                , "IMP_USOPRODCATD_ACC_STATALE"
                                , "IMP_USOPRODCATD_SAL_STATALE"
                                , "IMP_USOPRODCATD_TOT_STATALE"
                                , "CODTRIBUTO"
                                , "TIPOTASI"
                                , "IDCONTRIBUENTECALCOLO"
                                , "OPERATORE"
                                , "DATA_VARIAZIONE");
                    dsMyDati = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("ANNO",txtAnno.Text)
                        , ctx.GetParam("COD_ENTE", ConstWrapper.CodiceEnte)
                        , ctx.GetParam("COD_CONTRIBUENTE", hdIdContribuente.Value)
                        , ctx.GetParam("ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO", myAbiPrincAcc)
                        , ctx.GetParam("ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO", myAbiPrincSal)
                        , ctx.GetParam("ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE", myAbiPrincTot)
                        , ctx.GetParam("ICI_DOVUTA_ALTRI_FABBRICATI_ACCONTO", myAltriFabAcc)
                        , ctx.GetParam("ICI_DOVUTA_ALTRI_FABBRICATI_SALDO", myAltriFabSal)
                        , ctx.GetParam("ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE", myAltriFabTot)
                        , ctx.GetParam("ICI_DOVUTA_AREE_FABBRICABILI_ACCONTO", myAreeFabAcc)
                        , ctx.GetParam("ICI_DOVUTA_AREE_FABBRICABILI_SALDO", myAreeFabSal)
                        , ctx.GetParam("ICI_DOVUTA_AREE_FABBRICABILI_TOTALE", myAreeFabTot)
                        , ctx.GetParam("ICI_DOVUTA_TERRENI_ACCONTO", myTerreniAcc)
                        , ctx.GetParam("ICI_DOVUTA_TERRENI_SALDO", myTerreniSal)
                        , ctx.GetParam("ICI_DOVUTA_TERRENI_TOTALE", myTerreniTot)
                        , ctx.GetParam("ICI_DOVUTA_DETRAZIONE_ACCONTO", myDetrazAcc)
                        , ctx.GetParam("ICI_DOVUTA_DETRAZIONE_SALDO", myDetrazSal)
                        , ctx.GetParam("ICI_DOVUTA_DETRAZIONE_TOTALE", myDetrazTot)
                        , ctx.GetParam("ICI_DOVUTA_ACCONTO_SENZA_ARROTONDAMENTO", myDovutoAcc)
                        , ctx.GetParam("ICI_DOVUTA_SALDO_SENZA_ARROTONDAMENTO", myDovutoSal)
                        , ctx.GetParam("ICI_DOVUTA_TOTALE_SENZA_ARROTONDAMENTO", myDovutoTot)
                        , ctx.GetParam("ARROTONDAMENTO_ICI_DOVUTA_ACCONTO", 0)
                        , ctx.GetParam("ARROTONDAMENTO_ICI_DOVUTA_SALDO", 0)
                        , ctx.GetParam("ARROTONDAMENTO_ICI_DOVUTA_TOTALE", 0)
                        , ctx.GetParam("ICI_DOVUTA_ACCONTO", myDovutoAcc)
                        , ctx.GetParam("ICI_DOVUTA_SALDO", myDovutoSal)
                        , ctx.GetParam("ICI_DOVUTA_TOTALE", myDovutoTot)
                        , ctx.GetParam("PROGRESSIVO_ELABORAZIONE", -1)
                        , ctx.GetParam("CONFERMATO_ICI", 1)
                        , ctx.GetParam("DOVUTO_FORZATO", 0)
                        , ctx.GetParam("NUMERO_FABBRICATI", myNFab)
                        , ctx.GetParam("DATA_CONFERMA", DateTime.Now)
                        , ctx.GetParam("UTENTE_CONFERMA", ConstWrapper.sUsername)
                        , ctx.GetParam("DATA_INSERIMENTO", DateTime.Now)
                        , ctx.GetParam("CODELINE_ACCONTO", "")
                        , ctx.GetParam("CODELINE_SALDO", "")
                        , ctx.GetParam("CODELINE_UNICA_SOLUZIONE", "")
                        , ctx.GetParam("DOCUMENTO_ELABORATO", -1)
                        , ctx.GetParam("ICI_DOVUTA_DETRAZIONE_STATALE_ACCONTO", 0)
                        , ctx.GetParam("ICI_DOVUTA_DETRAZIONE_STATALE_SALDO", 0)
                        , ctx.GetParam("ICI_DOVUTA_DETRAZIONE_STATALE_TOTALE", 0)
                        , ctx.GetParam("IMP_ABI_PRINC_ACC_STATALE", 0)
                        , ctx.GetParam("IMP_ABI_PRINC_SAL_STATALE", 0)
                        , ctx.GetParam("IMP_ABI_PRINC_TOT_STATALE", 0)
                        , ctx.GetParam("IMP_ALTRI_FAB_ACC_STATALE", myAltriFabStatoAcc)
                        , ctx.GetParam("IMP_ALTRI_FAB_SAL_STATALE", myAltriFabStatoSal)
                        , ctx.GetParam("IMP_ALTRI_FAB_TOT_STATALE", myAltriFabSatoTot)
                        , ctx.GetParam("IMP_AREE_FAB_ACC_STATALE", myAreeFabStatoAcc)
                        , ctx.GetParam("IMP_AREE_FAB_SAL_STATALE", myAreeFabStatoSal)
                        , ctx.GetParam("IMP_AREE_FAB_TOT_STATALE", myAreeFabStatoTot)
                        , ctx.GetParam("IMP_TERRENI_ACC_STATALE", myTerreniStatoAcc)
                        , ctx.GetParam("IMP_TERRENI_SAL_STATALE", myTerreniStatoSal)
                        , ctx.GetParam("IMP_TERRENI_TOT_STATALE", myTerreniStatoTot)
                        , ctx.GetParam("DETRAZIONE_ACC_STATALE", 0)
                        , ctx.GetParam("DETRAZIONE_SAL_STATALE", 0)
                        , ctx.GetParam("DETRAZIONE_TOT_STATALE", 0)
                        , ctx.GetParam("TOTALE_ACC_ORIG_STATALE", 0)
                        , ctx.GetParam("TOTALE_SAL_ORIG_STATALE", 0)
                        , ctx.GetParam("TOTALE_ORIG_STATALE", 0)
                        , ctx.GetParam("ARR_TOTALE_ACC_STATALE", 0)
                        , ctx.GetParam("ARR_TOTALE_SAL_STATALE", 0)
                        , ctx.GetParam("ARR_TOTALE_STATALE", 0)
                        , ctx.GetParam("TOTALE_ACC_STATALE", 0)
                        , ctx.GetParam("TOTALE_SAL_STATALE", 0)
                        , ctx.GetParam("TOTALE_STATALE", 0)
                        , ctx.GetParam("IMP_FABRURUSOSTRUM_ACC", myFabRurAcc)
                        , ctx.GetParam("IMP_FABRURUSOSTRUM_SAL", myFabRurSal)
                        , ctx.GetParam("IMP_FABRURUSOSTRUM_TOT", myFabRurTot)
                        , ctx.GetParam("IMP_FABRURUSOSTRUM_ACC_STATALE", myFabRurStatoAcc)
                        , ctx.GetParam("IMP_FABRURUSOSTRUM_SAL_STATALE", myFabRurStatoSal)
                        , ctx.GetParam("IMP_FABRURUSOSTRUM_TOT_STATALE", myFabRurStatoTot)
                        , ctx.GetParam("IMP_USOPRODCATD_ACC", myUsoProdAcc)
                        , ctx.GetParam("IMP_USOPRODCATD_SAL", myUsoProdSal)
                        , ctx.GetParam("IMP_USOPRODCATD_TOT", myUsoProdTot)
                        , ctx.GetParam("IMP_USOPRODCATD_ACC_STATALE", myUsoProdStatoAcc)
                        , ctx.GetParam("IMP_USOPRODCATD_SAL_STATALE", myUsoProdStatoSal)
                        , ctx.GetParam("IMP_USOPRODCATD_TOT_STATALE", myUsoProdStatoTot)
                        , ctx.GetParam("CODTRIBUTO","RVOP")
                        , ctx.GetParam("TIPOTASI","P")
                        , ctx.GetParam("IDCONTRIBUENTECALCOLO",hdIdContribuente.Value)
                        , ctx.GetParam("OPERATORE",ConstWrapper.sUsername)
                        , ctx.GetParam("DATA_VARIAZIONE",DateTime.MaxValue)
);
                    ctx.Dispose();
                }
                foreach(DataRow myRow in dsMyDati.Tables[0].Rows)
                {
                    retVal = StringOperation.FormatInt(myRow["id"]);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.SetCalcoloFinale.errore: ", ex);
                retVal=-1;
            }
                return retVal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int DelCalcoloFinale()
        {
            DataSet dsMyDati;
            string sSQL;
            int retVal = 0;
            try
            {
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_CALCOLO_FINALE_ICI_DELRAV", "COD_CONTRIBUENTE"
                                , "ANNO"
                                , "CODTRIBUTO");
                    dsMyDati = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("COD_CONTRIBUENTE", hdIdContribuente.Value)
                                , ctx.GetParam("ANNO", txtAnno.Text)
                                , ctx.GetParam("CODTRIBUTO", "RVOP")
                            );
                    ctx.Dispose();
                }
                foreach (DataRow myRow in dsMyDati.Tables[0].Rows)
                {
                    retVal = StringOperation.FormatInt(myRow["id"]);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.GestioneRavvedimento.DelCalcoloFinale.errore: ", ex);
                retVal = -1;
            }
            return retVal;
        }

    }
}
