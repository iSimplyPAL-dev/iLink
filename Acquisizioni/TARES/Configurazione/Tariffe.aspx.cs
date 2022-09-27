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

namespace OPENgov.Acquisizioni.TARES.Configurazione
{
    public partial class Tariffe : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Tariffe));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "TARES/TARI - Configurazione - Tariffe";
            if (FromVariabile == "1")
                optCatConferimenti.Visible = true;
            else
                optCatConferimenti.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (hfIsNewRid.Value=="1")
                {
                    //ShowDiv("AddRidEse");
                    //HideDiv("ParamSearch"); HideDiv("ParamCopyYear");
                    //HideDiv("SearchTariffe"); HideDiv("SearchRidEse"); HideDiv("SearchMaggiorazioni");
                }
                return;
            }
            ManageBottoniera(false);
            LoadCombo("");
            LoadGridView(0,false);
        }

        #region "Bottoni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            LoadGridView(0,false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdInsertClick(object sender, EventArgs e)
        {
            try
            {
                if (rddlYear.SelectedValue != string.Empty)
                {
                    if (optCategorie.Checked == true || optMaggiorazioni.Checked == true || optCatConferimenti.Checked == true)
                    {
                        hfIsNewRid.Value = "0";
                        try
                        {
                            LoadGridView(0, true);
                        }
                        catch (Exception)
                        {
                            ManageBottoniera(false);
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('Tariffe già presenti impossibile inserire come nuovo');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "errCmdInsertClick", sBuilder.ToString());
                        }
                    }
                    else
                    {
                        hfIsNewRid.Value = "1";
                        if (hfTypeCalcolo.Value != string.Empty)
                        {
                            CategorieAtecoEnte Cat = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, TypeCalcolo = hfTypeCalcolo.Value };
                            rgvCat.DataSource = Cat.LoadAll();
                            rgvCat.PageIndex = 0;
                            rgvCat.DataBind();
                        }
                        ShowDiv("AddRidEse");
                        HideDiv("ParamSearch"); HideDiv("ParamCopyYear");
                        HideDiv("SearchTariffe"); HideDiv("SearchRidEse"); HideDiv("SearchMaggiorazioni");
                    }
                }
                else {
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Seleziona anno');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "errCmdInsertClick", sBuilder.ToString());
                    ManageBottoniera(false);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.CmdInsertClick.errore::", ex);;
                throw;
            }
        }

        protected void CmdSaveClick(object sender, EventArgs e)
        {
            if (optCategorie.Checked == true)
            {
                SaveTariffe();
            }
            else if (optMaggiorazioni.Checked == true || optCatConferimenti.Checked==true)
            {
                SaveMaggiorazioni();
            }
            else
            {
                hfIsNewRid.Value = "0";
                SaveRidEse();
            }
        }

        protected void CmdBackClick(object sender, EventArgs e)
        {
            ManageBottoniera(false);
        }

        protected void CmdCopyYearClick(object sender, EventArgs e)
        {
            try
            {
                ShowDiv("ParamCopyYear");
                HideDiv("divEdit");
                HideDiv("AddRidEse"); 
                HideDiv("ParamSearch"); HideDiv("SearchTariffe"); HideDiv("SearchRidEse"); ; HideDiv("SearchMaggiorazioni");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.CmdCopyYearClick.errore::", ex);;
                throw;
            }
        }

        protected void CmdSaveCopyClick(object sender, EventArgs e)
        {
            try
            {
                int typeTassazione = TariffeEnte.TypeTassazione_TariffeTributo;
                string typeRidEse = "R";
                //valorizzo che tipo di tariffe sto inserendo
                if (optCatConferimenti.Checked)
                    typeTassazione = TariffeEnte.TypeTassazione_TariffeConferimenti;
                else if (optMaggiorazioni.Checked)
                    typeTassazione = TariffeEnte.TypeTassazione_TariffeMaggiorazioni;
                //valorizzo che tipo di rid/ese sto inserendo
                if (optEsenzioni.Checked)
                    typeRidEse = "D";
                //se ho anno ribalto
                if (rddlYearFrom.SelectedValue != string.Empty && rddlYearTo.SelectedValue != string.Empty)
                {
                    if (optCategorie.Checked || optCatConferimenti.Checked || optMaggiorazioni.Checked)
                    {
                        TariffeEnte myTariffa = new TariffeEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = typeTassazione };
                        if (!myTariffa.CopyYear(rddlYearFrom.SelectedValue, rddlYearTo.SelectedValue))
                        {
                            ManageBottoniera(false);
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('Impossibile ribaltare le tariffe. Controlla che gli anni abbiano la stessa tipologia di calcolo!');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "CmdSaveCopyClick", sBuilder.ToString());
                        }
                        else
                        {
                            rddlYear.SelectedValue = rddlYearTo.SelectedValue;
                            ParametriCalcoloEnte myParamCalcolo = new ParametriCalcoloEnte { Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue };
                            if (myParamCalcolo.Load())
                            {
                                hfTypeCalcolo.Value = myParamCalcolo.TypeCalcolo;
                                ManageParam(myParamCalcolo);
                                LoadCombo(myParamCalcolo.TypeCalcolo);
                            }
                            LoadGridView(0, false);
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('Salvataggio terminato con successo!');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "CmdSaveCopyClick", sBuilder.ToString());
                        }
                    }
                    else {
                        RidEseTariffeEnte myRidEse = new RidEseTariffeEnte { Ente = Ente, FromVariabile = FromVariabile, Tipo = typeRidEse };
                        if (!myRidEse.CopyYear(rddlYearFrom.SelectedValue, rddlYearTo.SelectedValue))
                        {
                            ManageBottoniera(false);
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('Impossibile ribaltare le tariffe. Controlla che gli anni abbiano la stessa tipologia di calcolo!');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "CmdSaveCopyClick", sBuilder.ToString());
                        }
                        else
                        {
                            rddlYear.SelectedValue = rddlYearTo.SelectedValue;
                            ParametriCalcoloEnte myParamCalcolo = new ParametriCalcoloEnte { Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue };
                            if (myParamCalcolo.Load())
                            {
                                hfTypeCalcolo.Value = myParamCalcolo.TypeCalcolo;
                                ManageParam(myParamCalcolo);
                                LoadCombo(myParamCalcolo.TypeCalcolo);
                            }
                            LoadGridView(0, false);
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('Salvataggio terminato con successo!');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "CmdSaveCopyClick", sBuilder.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.CmdSaveCopyClick.errore::", ex);;
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdPrintClick(object sender, EventArgs e)
        {
            string filepath = string.Empty;

            DataSet ds = null;
            try
            {
                if (optCategorie.Checked == true)
                {
                    if (hfTypeCalcolo.Value != string.Empty)
                    {
                        int idTipoCat = -1;
                        int.TryParse(rddlTipoCat.SelectedValue, out idTipoCat);
                        TariffeEnte tariffe = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, TypeUtenze = idTipoCat, IsNewAnno = false };
                        ExportToExcel myxls = new ExportToExcel();
                        List<TariffeEnte> o = new List<TariffeEnte>(tariffe.LoadAll());
                        ds =myxls.CreateDataSet(o);
                        if (ds != null)
                        {
                            filepath = Server.MapPath(@".\ExcelTemplate.xls");
                            myxls.ExportDataSetToExcel(ds, filepath);
                        }
                    }
                }
                else if (optCatConferimenti.Checked == true)
                {
                    if (hfTypeCalcolo.Value != string.Empty)
                    {
                        int idTipoCat = -1;
                        int.TryParse(rddlTipoCat.SelectedValue, out idTipoCat);
                        TariffeEnte tariffe = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue, TypeTassazione = TariffeEnte.TypeTassazione_TariffeConferimenti, TypeUtenze = idTipoCat, IsNewAnno = false };
                        ExportToExcel myxls = new ExportToExcel();
                        List<TariffeEnte> o = new List<TariffeEnte>(tariffe.LoadAll());
                        ds = myxls.CreateDataSet(o);
                        if (ds != null)
                        {
                            filepath = Server.MapPath(@".\ExcelTemplate.xls");
                            myxls.ExportDataSetToExcel(ds, filepath);
                        }
                    }
                }
                else {
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Funzionalità al momento non disponibile');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "errCmdPrintClick", sBuilder.ToString());
                }
                ManageBottoniera(false);
            }
            catch (System.Threading.ThreadAbortException er)
            {
                Global.Log.Write2(LogSeverity.Information, "End ExportDataSetToExcel" + er.Message);
            }
            catch (Exception er)
            {
                Global.Log.Write2(LogSeverity.Critical, er);
            }
        }
        #endregion

        #region "DropDownList"
        protected void rddlYearSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ManageBottoniera(false);
                if (rddlYear.SelectedValue!=string.Empty)
                {
                    ParametriCalcoloEnte paramCalcolo = new ParametriCalcoloEnte { Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue };
                    if (paramCalcolo.Load())
                    {
                        hfTypeCalcolo.Value= paramCalcolo.TypeCalcolo;
                        ManageParam(paramCalcolo);
                        LoadCombo(paramCalcolo.TypeCalcolo);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.rddlYearSelectedIndexChanged.errore::", ex);;
                throw;
            }
        }
        #endregion
        #region "Griglia Tariffe"
        protected void RgvTariffePageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex, false);
        }

        protected void RgvTariffeRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
        }

        protected void RgvTariffeRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idTariffa;
            int.TryParse(e.CommandArgument.ToString(), out idTariffa);
            TariffeEnte tariffa = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, IdTariffa = idTariffa, FromVariabile = FromVariabile };
            if (tariffa.Load())
            {
                switch (e.CommandName)
                {
                    case "DeleteTariffe":
                        if (tariffa.Delete())
                            LoadGridView(0, false);
                        else
                        {
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('" + string.Format("Impossibile eliminare la tariffa: {0}",
                                                                                       tariffa.Descrizione) + "');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "errIdCategoriaAteco", sBuilder.ToString());
                            ManageBottoniera(false);
                        }
                        break;
                    default:
                        ManageBottoniera(false);
                        break;
                }
            }
        }
        #endregion
        #region "Griglia RidEse"
        protected void RgvRidEsePageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex, false);
        }

        protected void RgvRidEseRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
        }

        protected void RgvRidEseRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string mytype = "R";
            if (optEsenzioni.Checked)
                mytype = "D";
            int idTariffa;
            int.TryParse(e.CommandArgument.ToString(), out idTariffa);
            RidEseTariffeEnte tariffa = new RidEseTariffeEnte { myType = mytype, Ente = Ente, FromVariabile = FromVariabile, IdTariffa = idTariffa };
            if (tariffa.Load())
            {
                switch (e.CommandName)
                {
                    case "EditRidEse":
                        rddlYearRidEse.SelectedValue = tariffa.Anno;
                        rddlRidEse.SelectedValue = tariffa.Codice;
                        rddlTipoRidEse.SelectedValue = tariffa.Tipo;
                        txtValoreRidEse.Text = tariffa.Valore;
                        hfIdRidEse.Value = tariffa.IdTariffa.ToString();
                        chkPF.Checked= tariffa.hasPF; chkPV.Checked= tariffa.hasPV; chkPC.Checked= tariffa.hasPC; chkPM.Checked= tariffa.hasPM;
                        if (hfTypeCalcolo.Value != string.Empty)
                        {
                            CategorieAtecoEnte Cat = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, TypeCalcolo = hfTypeCalcolo.Value };//, IdRid=tariffa.IdTariffa
                            rgvCat.DataSource = Cat.LoadAll();
                            rgvCat.PageIndex = 0;
                            rgvCat.DataBind();
                        }

                        ManageBottoniera(true);
                        break;
                    case "DeleteRidEse":
                        if (tariffa.Delete())
                            LoadGridView(0,false);
                        else
                        {
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('" + string.Format("Impossibile eliminare la tariffa: {0}",
                                                                                       tariffa.IdTariffa) + "');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "errIdCategoriaAteco", sBuilder.ToString());
                            ManageBottoniera(false);
                        }
                        break;
                    default:
                        ManageBottoniera(false);
                        break;
                }
            }
        }
        #endregion
        #region "Griglia Maggiorazioni"
        protected void RgvMaggiorazioniPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex, false);
        }

        protected void RgvMaggiorazioniRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
        }

        protected void RgvMaggiorazioniRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idTariffa;
            int.TryParse(e.CommandArgument.ToString(), out idTariffa);
            TariffeEnte tariffa = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, IdTariffa = idTariffa, FromVariabile = FromVariabile };
            if (tariffa.Load())
            {
                switch (e.CommandName)
                {
                    case "DeleteMaggiorazioni":
                        if (tariffa.Delete())
                            LoadGridView(0, false);
                        else
                        {
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('" + string.Format("Impossibile eliminare la tariffa: {0}",
                                                                                       tariffa.IdTariffa) + "');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "errIdCategoriaAteco", sBuilder.ToString());
                            ManageBottoniera(false);
                        }
                        break;
                    default:
                        ManageBottoniera(false);
                        break;
                }
            }
        }
        #endregion
        #region "Griglia Cat"
        protected void RgvCatPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex, false);
        }

        protected void RgvCatRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
        }

        protected void RgvCatRowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        #endregion
        //*** 201712 - gestione tipo conferimento ***
        protected void ChangeTypeCodDescr(object sender, System.EventArgs e)
        {
            if (optCategorie.Checked == true)
            {
                HideDiv("SearchRidEse");HideDiv("SearchMaggiorazioni") ; ShowDiv("SearchTariffe");
            }
            else if (optMaggiorazioni.Checked == true || optCatConferimenti.Checked==true)
            {
                if (optCatConferimenti.Checked)
                {
                    rgvMaggiorazioni.Columns[2].Visible = true;
                    rgvMaggiorazioni.Columns[4].Visible=true;
                    rgvMaggiorazioni.Columns[5].Visible=true;
                }
                else{
                    rgvMaggiorazioni.Columns[2].Visible = false;
                    rgvMaggiorazioni.Columns[4].Visible=false;
                    rgvMaggiorazioni.Columns[5].Visible=false;
                }
                HideDiv("SearchTariffe"); HideDiv("SearchRidEse"); ShowDiv("SearchMaggiorazioni");
            }
            else
            {
                HideDiv("SearchTariffe"); HideDiv("SearchMaggiorazioni"); ShowDiv("SearchRidEse");

                string MyType = "";
                if (optRiduzioni.Checked == true)
                    MyType = "R";
                else
                    MyType = "D";
                RidEseEnte RidEse = new RidEseEnte { myType = MyType, Ente = Ente, FromVariabile = FromVariabile };
                rddlRidEse.DataSource = RidEse.LoadAll();
                rddlRidEse.DataValueField = "Codice";
                rddlRidEse.DataTextField = "Definizione";
                rddlRidEse.DataBind();
            }
            LoadCombo(hfTypeCalcolo.Value);
            LoadGridView(0,false);
        }
        //*** ***
        protected void ChangeTypeAppl(object sender, System.EventArgs e)
        {
            if (optSingleP.Checked == true)
            {
                chkPF.Enabled = true; chkPV.Enabled = true; chkPC.Enabled = true; chkPM.Enabled = true;
            }
            else
            {
                chkPF.Enabled = false; chkPV.Enabled = false; chkPC.Enabled = false; chkPM.Enabled = false;
            }
        }

        private void LoadCombo(string typeCalcolo)
        {
            try
            {
                //se ho il tipo calcolo vuol dire che ho già selezionato l'anno, quindi devo ricaricare solo le categorie
                if (typeCalcolo != string.Empty)
                {
                    CategorieAtecoEnte Categorie = null;
                    if (optCatConferimenti.Checked)
                        Categorie = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeConferimenti, TypeCalcolo = typeCalcolo };
                    else
                        Categorie = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, TypeCalcolo = typeCalcolo };
                    rddlCategorie.DataSource = Categorie.LoadAll();
                    rddlCategorie.DataValueField = "IdCategoriaAteco";
                    rddlCategorie.DataTextField = "Descrizione";
                    rddlCategorie.DataBind();
                }
                else
                {
                    ParametriCalcoloEnte paramCalcolo = new ParametriCalcoloEnte { Ente = Ente, FromVariabile = FromVariabile };
                    ParametriCalcoloEnte[] myParam = paramCalcolo.LoadAll();
                    rddlYear.DataSource = myParam;
                    rddlYear.DataValueField = "Anno";
                    rddlYear.DataTextField = "Anno";
                    rddlYear.DataBind();

                    rddlYearFrom.DataSource = myParam;
                    rddlYearFrom.DataValueField = "Anno";
                    rddlYearFrom.DataTextField = "Anno";
                    rddlYearFrom.DataBind();

                    rddlYearTo.DataSource = myParam;
                    rddlYearTo.DataValueField = "Anno";
                    rddlYearTo.DataTextField = "Anno";
                    rddlYearTo.DataBind();

                    rddlYearRidEse.DataSource = myParam;
                    rddlYearRidEse.DataValueField = "Anno";
                    rddlYearRidEse.DataTextField = "Anno";
                    rddlYearRidEse.DataBind();

                    CategorieAtecoEnte categorie = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile };
                    rddlTipoCat.DataSource = categorie.LoadType();
                    rddlTipoCat.DataValueField = "IdCategoriaAteco";
                    rddlTipoCat.DataTextField = "Definizione";
                    rddlTipoCat.DataBind();

                    RidEseEnte RidEse = new RidEseEnte { };
                    rddlTipoRidEse.DataSource = RidEse.LoadType();
                    rddlTipoRidEse.DataValueField = "Codice";
                    rddlTipoRidEse.DataTextField = "Definizione";
                    rddlTipoRidEse.DataBind();

                    string MyType = "";
                    if (optRiduzioni.Checked == true)
                        MyType = "R";
                    else
                        MyType = "D";
                    RidEse = new RidEseEnte { myType = MyType, Ente = Ente, FromVariabile = FromVariabile };
                    rddlRidEse.DataSource = RidEse.LoadAll();
                    rddlRidEse.DataValueField = "Codice";
                    rddlRidEse.DataTextField = "Definizione";
                    rddlRidEse.DataBind();

                    //ManageParam(myParamCalcolo);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.LoadCombo.errore::", ex);;
                throw;
            }
        }

        private void LoadGridView(int page, bool IsNewAnno)
        {
            try
            {
               if (optCategorie.Checked == true)
                {
                    if (hfTypeCalcolo.Value!=string.Empty) 
                    { 
                        int idTipoCat= -1;
                        int.TryParse(rddlTipoCat.SelectedValue, out idTipoCat);
                        TariffeEnte tariffe = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue, IdCategoria = rddlCategorie.SelectedValue, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, TypeUtenze = idTipoCat, IsNewAnno = IsNewAnno };
                        rgvTariffe.DataSource = tariffe.LoadAll();
                        rgvTariffe.PageIndex = page;
                        rgvTariffe.DataBind();
                    }
                }
               else if (optCatConferimenti.Checked == true)
               {
                   if (hfTypeCalcolo.Value != string.Empty)
                   {
                       TariffeEnte tariffe = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue, TypeTassazione = TariffeEnte.TypeTassazione_TariffeConferimenti, IsNewAnno = IsNewAnno };
                       rgvMaggiorazioni.DataSource = tariffe.LoadAll();
                       rgvMaggiorazioni.PageIndex = page;
                       rgvMaggiorazioni.DataBind();
                   }
               }
               else if (optMaggiorazioni.Checked == true)
                {
                    if (hfTypeCalcolo.Value != string.Empty)
                    {
                        TariffeEnte tariffe = new TariffeEnte { TypeCalcolo = hfTypeCalcolo.Value, Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue, TypeTassazione = TariffeEnte.TypeTassazione_TariffeMaggiorazioni, IsNewAnno = IsNewAnno };
                        rgvMaggiorazioni.DataSource = tariffe.LoadAll();
                        rgvMaggiorazioni.PageIndex = page;
                        rgvMaggiorazioni.DataBind();
                    }
                }
                else
                {
                    string MyType = "";
                    if (optRiduzioni.Checked == true)
                        MyType = "R";
                    else
                        MyType = "D";
                    RidEseTariffeEnte RidEse = new RidEseTariffeEnte { myType = MyType, Ente = Ente, FromVariabile = FromVariabile, Anno = rddlYear.SelectedValue, IsNewAnno = IsNewAnno };
                    rgvRidEse.DataSource = RidEse.LoadAll();
                    rgvRidEse.PageIndex = page;
                    rgvRidEse.DataBind();

                    if (hfTypeCalcolo.Value != string.Empty)
                    {
                        CategorieAtecoEnte Cat = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, TypeCalcolo = hfTypeCalcolo.Value };
                        rgvCat.DataSource = Cat.LoadAll();
                        rgvCat.PageIndex = page;
                        rgvCat.DataBind();
                    }
                }
                ManageBottoniera(false);
                //ManageParam(myParamCalcolo);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.LoadGridView.errore::", ex);;
                throw;
            }
        }

        private void ManageBottoniera(bool showEdit)
        {
            if (showEdit)
            {
                ShowDiv("divEdit");
                ShowDiv("AddRidEse"); HideDiv("ParamCopyYear");
                HideDiv("divSearch");
                HideDiv("ParamSearch"); HideDiv("SearchTariffe"); HideDiv("SearchRidEse"); ; HideDiv("SearchMaggiorazioni");
            }
            else
            {
                ShowDiv("divSearch"); HideDiv("divEdit");
                HideDiv("ParamCopyYear"); HideDiv("AddRidEse");
                if (optCategorie.Checked == true)
                {
                    ShowDiv("SearchTariffe"); HideDiv("SearchRidEse"); HideDiv("SearchMaggiorazioni");
                }
                else if (optMaggiorazioni.Checked == true || optCatConferimenti.Checked==true)
                {
                    ShowDiv("SearchMaggiorazioni"); HideDiv("SearchTariffe"); HideDiv("SearchRidEse");
                }
                else
                {
                    ShowDiv("SearchRidEse"); HideDiv("SearchTariffe"); HideDiv("SearchMaggiorazioni");
                }
            }
        }

        private void ManageParam(ParametriCalcoloEnte myCalcolo)
        {
            if (myCalcolo != null)
            {
                if (myCalcolo.TypeCalcolo == ParametriCalcoloEnte.Calcolo_PF)
                {
                    rgvTariffe.Columns[3].Visible = false;
                    chkPV.Visible = false;
                    rgvRidEse.Columns[3].Visible = false;
                }
                else
                {
                    rgvTariffe.Columns[3].Visible = true;
                    chkPV.Visible = true;
                    rgvRidEse.Columns[3].Visible = true;
                }

                if (myCalcolo.HasConferimenti == false)
                {
                    optCatConferimenti.Visible = false;
                    chkPC.Visible = false;
                    rgvRidEse.Columns[4].Visible = false;
                }
                else
                {
                    optCatConferimenti.Visible = true;
                    chkPC.Visible = true;
                    rgvRidEse.Columns[4].Visible = true;
                }

                if (myCalcolo.HasMaggiorazione == false)
                {
                    optMaggiorazioni.Visible = false;
                    chkPM.Visible = false;
                    rgvRidEse.Columns[5].Visible = false;
                }
                else
                {
                    optMaggiorazioni.Visible = true;
                    chkPM.Visible = true;
                    rgvRidEse.Columns[5].Visible = true;
                }
            }
        }
        /// <summary>
        /// Funzione per il salvataggio delle tariffe configurate a video
        /// </summary>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        private void SaveTariffe()
        {
            try
            {
                int idTariffa = 0;
                foreach (GridViewRow myRow in rgvTariffe.Rows)
                {
                    TariffeEnte myTariffa = new TariffeEnte();
                    myTariffa.FromVariabile = FromVariabile;
                    myTariffa.Anno = myRow.Cells[0].Text;
                    myTariffa.Ente = Ente;
                    myTariffa.Descrizione = myRow.Cells[1].Text;
                    myTariffa.IdCategoria = ((HiddenField)myRow.Cells[4].FindControl("hfIdCategoria")).Value.ToString();
                    myTariffa.IdTariffa = int.Parse(((HiddenField)myRow.Cells[4].FindControl("hfIdTariffa")).Value.ToString());
                    myTariffa.impPF = double.Parse(((TextBox)myRow.Cells[2].FindControl("txtPF")).Text);
                    myTariffa.impPV = double.Parse(((TextBox)myRow.Cells[3].FindControl("txtPV")).Text);
                    myTariffa.nComponenti = ((HiddenField)myRow.Cells[4].FindControl("hfNC")).Value.ToString();
                    myTariffa.TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo;
                    if (!myTariffa.Save())
                    {
                        ManageBottoniera(false);
                        string sScript;
                        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                        sScript = "<script language='javascript'>";
                        sScript += "alert('" + string.Format("Impossibile salvare la tariffa: {0}. Controlla che non sia già usata", myTariffa.Descrizione) + "');";
                        sScript += "</script>";
                        sBuilder.Append(sScript);
                        ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
                        idTariffa = 0;
                        break;
                    }
                    idTariffa = myTariffa.IdTariffa;
                }
                if (idTariffa > 0)
                {
                    LoadGridView(0, false);
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Salvataggio terminato con successo!');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.SaveTariffe.errore::", ex); ;
                throw ex;
            }
        }
        //private void SaveTariffe()
        //{
        //    try
        //    {
        //        int idTariffa = 0;
        //        foreach (GridViewRow myRow in rgvTariffe.Rows)
        //        {
        //            TariffeEnte myTariffa = new TariffeEnte();
        //            myTariffa.FromVariabile = FromVariabile;
        //            myTariffa.Anno = myRow.Cells[0].Text;
        //            myTariffa.Ente = Ente;
        //            myTariffa.Descrizione = myRow.Cells[1].Text;
        //            myTariffa.IdCategoria = ((HiddenField)myRow.Cells[4].FindControl("hfIdCategoria")).Value.ToString();
        //            myTariffa.IdTariffa = int.Parse(((HiddenField)myRow.Cells[4].FindControl("hfIdTariffa")).Value.ToString());
        //            myTariffa.impPF = double.Parse(((TextBox)myRow.Cells[2].FindControl("txtPF")).Text);
        //            if (hfTypeCalcolo.Value.Contains(ParametriCalcoloEnte.Calcolo_PFPV))//(hfTypeCalcolo.Value == ParametriCalcoloEnte.Calcolo_PFPV)
        //            {
        //                myTariffa.impPV = double.Parse(((TextBox)myRow.Cells[3].FindControl("txtPV")).Text);
        //                myTariffa.nComponenti = ((HiddenField)myRow.Cells[4].FindControl("hfNC")).Value.ToString();
        //            }
        //            myTariffa.TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo;
        //            if (!myTariffa.Save())
        //            {
        //                ManageBottoniera(false);
        //                string sScript;
        //                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
        //                sScript = "<script language='javascript'>";
        //                sScript += "alert('" + string.Format("Impossibile salvare la tariffa: {0}. Controlla che non sia già usata", myTariffa.Descrizione) + "');";
        //                sScript += "</script>";
        //                sBuilder.Append(sScript);
        //                ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
        //                idTariffa = 0;
        //                break;
        //            }
        //            idTariffa = myTariffa.IdTariffa;
        //        }
        //        if (idTariffa > 0)
        //        {
        //            LoadGridView(0, false);
        //            string sScript;
        //            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
        //            sScript = "<script language='javascript'>";
        //            sScript += "alert('Salvataggio terminato con successo!');";
        //            sScript += "</script>";
        //            sBuilder.Append(sScript);
        //            ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("OPENgov.20.Tariffe.SaveTariffe.errore::", ex);;
        //        throw ex;
        //    }
        //}

        private void SaveMaggiorazioni()
        {
            //*** 201712 - gestione tipo conferimento ***
            try
            {
                int idTariffa = 0;
                foreach (GridViewRow myRow in rgvMaggiorazioni.Rows)
                {
                    TariffeEnte myTariffa = new TariffeEnte();
                    myTariffa.FromVariabile = FromVariabile;
                    myTariffa.Anno = myRow.Cells[0].Text;
                    myTariffa.Ente = Ente;
                    myTariffa.Descrizione = myRow.Cells[1].Text;
                    myTariffa.IdCategoria = ((HiddenField)myRow.FindControl("hfIdCategoria")).Value.ToString();
                    myTariffa.IdTariffa = int.Parse(((HiddenField)myRow.FindControl("hfIdTariffa")).Value.ToString());
                    myTariffa.impPF = double.Parse(((TextBox)myRow.FindControl("txtPM")).Text);
                    if (optCatConferimenti.Checked)
                    {
                        myTariffa.TipoConferimento= ((HiddenField)myRow.FindControl("hfIdTipoConferimento")).Value.ToString();
                        myTariffa.MoltiplicatoreMinimo = double.Parse(((TextBox)myRow.FindControl("txtMoltiplicatoreMinimo")).Text);
                        myTariffa.impMinimo = double.Parse(((TextBox)myRow.FindControl("txtimpMinimo")).Text);
                    }
                    if (optCatConferimenti.Checked==true)
                        myTariffa.TypeTassazione = TariffeEnte.TypeTassazione_TariffeConferimenti;
                    else
                        myTariffa.TypeTassazione = TariffeEnte.TypeTassazione_TariffeMaggiorazioni;
                    if (!myTariffa.Save())
                    {
                        ManageBottoniera(false);
                        string sScript;
                        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                        sScript = "<script language='javascript'>";
                        sScript += "alert('" + string.Format("Impossibile salvare la tariffa: {0}. Controlla che non sia già usata", myTariffa.Descrizione) + "');";
                        sScript += "</script>";
                        sBuilder.Append(sScript);
                        ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
                        idTariffa = 0;
                        break;
                    }

                    idTariffa = myTariffa.IdTariffa;
                }
                if (idTariffa > 0)
                {
                    LoadGridView(0, false);
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Salvataggio terminato con successo!');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "tartrib", sBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.SaveMaggiorazioni.errore::", ex);;
                throw ex;
            }
        }

        private void SaveRidEse()
        {
            try
            {
                int idTariffa = 0;
                string MyType = "";
                if (optRiduzioni.Checked == true)
                    MyType = "R";
                else
                    MyType = "D";
                if (rddlYearRidEse.SelectedValue != string.Empty && rddlRidEse.SelectedValue != string.Empty && rddlTipoRidEse.SelectedValue != string.Empty && txtValoreRidEse.Text != string.Empty && (optImponibile.Checked == true || (optSingleP.Checked == true && (chkPF.Checked == true || chkPV.Checked == true || chkPC.Checked == true || chkPM.Checked == true))))
                {
                    RidEseTariffeEnte myTariffa = new RidEseTariffeEnte();
                    myTariffa.FromVariabile = FromVariabile;
                    myTariffa.Anno = rddlYearRidEse.SelectedValue;
                    myTariffa.Ente = Ente;
                    myTariffa.Codice = rddlRidEse.SelectedValue;
                    myTariffa.Descrizione = rddlRidEse.SelectedItem.Text;
                    myTariffa.hasImponibile = optImponibile.Checked;
                    myTariffa.hasPC = chkPC.Checked;
                    myTariffa.hasPF = chkPF.Checked;
                    myTariffa.hasPM = chkPM.Checked;
                    myTariffa.hasPV = chkPV.Checked;
                    myTariffa.myType = MyType;
                    myTariffa.IdTariffa = int.Parse(hfIdRidEse.Value);
                    myTariffa.Tipo = rddlTipoRidEse.SelectedValue;
                    myTariffa.Valore = txtValoreRidEse.Text;
                    if (!myTariffa.Save())
                    {
                        ManageBottoniera(false);
                        string sScript;
                        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                        sScript = "<script language='javascript'>";
                        sScript += "alert('" + string.Format("Impossibile salvare la tariffa: {0}. Controlla che non sia già usata", myTariffa.Descrizione) + "');";
                        sScript += "</script>";
                        sBuilder.Append(sScript);
                        ClientScript.RegisterStartupScript(this.GetType(), "tarridese", sBuilder.ToString());
                        idTariffa = 0;
                    }
                    idTariffa = myTariffa.IdTariffa;
                    if (idTariffa > 0)
                    {
                        LoadGridView(0, false);
                        string sScript;
                        System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                        sScript = "<script language='javascript'>";
                        sScript += "alert('Salvataggio terminato con successo!');";
                        sScript += "</script>";
                        sBuilder.Append(sScript);
                        ClientScript.RegisterStartupScript(this.GetType(), "tarridese", sBuilder.ToString());
                    }
                }
                else
                {
                    string sScript;
                    System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                    sScript = "<script language='javascript'>";
                    sScript += "alert('Inserire tutti i parametri di configurazione!');";
                    sScript += "</script>";
                    sBuilder.Append(sScript);
                    ClientScript.RegisterStartupScript(this.GetType(), "tarridese", sBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.Tariffe.SaveRidEse.errore::", ex); ;
                throw ex;
            }
        }
    }
}