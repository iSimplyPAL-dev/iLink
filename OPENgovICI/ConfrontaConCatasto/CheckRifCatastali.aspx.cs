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
using Business;

namespace DichiarazioniICI//.ConfrontaConCatasto
{
/// <summary>
/// Pagina per le estrazioni di confronto dei riferimenti catastali fra banche dati.
/// Indicando l'anno le possibili opzioni sono:
/// - Riferimenti mancanti in IMU
/// - Riferimenti con errata copertura di possesso
/// - Riferimenti chiusi e non riaperti
/// - Riferimenti doppi per lo stesso periodo
/// - Riferimenti IMU accertati
/// - Rif.Dichiarati in ICI/IMU e non in TARI
/// - Rif.Dichiarati in TARI e non in ICI/IMU
/// - Rif.Catastali non in Dichiarazioni
/// - Rif.Dichiarati non a catasto
/// - Posizione Dichiarata uguale a Catastale
/// - Cat. e/o Classe Dichiarata diversa da Catastale
/// - Rendita e/o Consistenza Catastale diversa da Dichiarata
/// - Proprietario e/o Copertura Catastale diverso da Dichiarato
/// </summary>
/// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class CheckRifCatastali :BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckRifCatastali));
        private int nTypeCheck = 0;
        private ClsAnalisi FncCheck = new ClsAnalisi();

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
            //this.GrdDichRifCatastali.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdDichRifCatastali_ItemCommand);
            //this.GrdDichCatRifCatastali.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdDichRifCatastali_ItemCommand);
            //this.GrdDichRifErrataCopertura.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdDichRifCatastali_ItemCommand);
            //         this.GrdDichTARSUnoICI.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrdDichRifCatastali_ItemCommand);
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Put user code to initialize the page here
                lblTitolo.Text = ConstWrapper.DescrizioneEnte;
                //if (Request.Params["TypeCheck"] != null)
                //    nTypeCheck = int.Parse(Request.Params["TypeCheck"]);
                if (!ConstWrapper.HasDummyDich)
                {
                    OptICInoTARSU.Style.Add("display", "none");
                    OptTARSUnoICI.Style.Add("display", "none");
                }
                if (nTypeCheck <= 0)
                {
                    if (OptRifChiusi.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.RifChiusi;
                    }
                    if (OptCatNoDic.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.CatNoDic;
                    }
                    if (OptCatEqualDic.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.CatEqualDic;
                    }
                    if (OptRifMancanti.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.RifMancanti;
                    }
                    if (OptDicNoCat.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.DicNoCat;
                    }
                    if (OptCatDifferentDic.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.CatDifferentDic;
                    }
                    if (OptRifDoppi.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.RifDoppi;
                    }
                    if (OptRenCatDifferentDic.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.RenCatDifferentDic;
                    }
                    if (OptRifAccertati.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.RifAccertati;
                    }
                    if (OptPropCatDifferentDic.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.PropCatDifferentDic;
                    }
                    if (OptRifErrataCopertura.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.RifErrataCopertura;
                    }
                    if (OptICInoTARSU.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.ICInoTARSU;
                    }
                    if (OptTARSUnoICI.Checked == true)
                    {
                        nTypeCheck = (int)ClsAnalisi.TypeCheckRifCatastali.TARSUnoICI;
                    }
                }
                if (!Page.IsPostBack)
                {
                    ListItem myListItem1;
                    int x;
                    DdlAnno.Items.Clear();
                    for (x = DateTime.Now.Year; (x >= (DateTime.Now.Year - 6)); x = (x + -1))
                    {
                        myListItem1 = new ListItem();
                        myListItem1.Text = x.ToString();
                        myListItem1.Value = x.ToString();
                        DdlAnno.Items.Add(myListItem1);
                    }
                    if (Session["AnnoCheckRifCatastali"] != null)
                    {
                        DdlAnno.SelectedValue = Session["AnnoCheckRifCatastali"].ToString();
                    }
                    if (Session["TypeCheckRifCat"] != null && (int)Session["TypeCheckRifCat"] != 0)
                    {
                        log.Debug("ho session'TypeCheckRifCat' -- check=" + nTypeCheck.ToString() + "  sessione=" + Session["TypeCheckRifCat"].ToString());
                        nTypeCheck = (int)Session["TypeCheckRifCat"];
                    }
                    try
                    {
                        if (nTypeCheck > 0)
                        {
                            log.Debug("CheckRifCatastali::Page_Load::sto tornando da un dettaglio quindi devo ricaricare la ricerca precedente::");
                            if (!LoadGrd(FncCheck.GetCheckRifCatastali(ConstWrapper.StringConnection, ConstWrapper.CodiceEnte, int.Parse(DdlAnno.SelectedValue), nTypeCheck, false), nTypeCheck))
                            {
                                Response.Redirect("../../PaginaErrore.aspx");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali.Page_Load.errore: ", ex);
                        Response.Redirect("../../PaginaErrore.aspx");
                    }
                }

                Session["TypeCheckRifCat"] = nTypeCheck;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdRicerca_Click(object sender, System.EventArgs e)
        {            
            DataSet myDsResult;
            Session["dsResultCheckRifCatastali"] = null;
            try
            {
                log.Debug("CheckRifCatastali::CmdRicerca_Click::inizio ricerca");
                LblDownloadFile.Text = "";
                LblDownloadFile.Style.Add("display", "none");
                Session["AnnoCheckRifCatastali"] = int.Parse(DdlAnno.SelectedValue);
                myDsResult = FncCheck.GetCheckRifCatastali(ConstWrapper.StringConnection, ConstWrapper.CodiceEnte, int.Parse(DdlAnno.SelectedValue), nTypeCheck, false);
                Session["dsResultCheckRifCatastali"] = myDsResult;
                if (!LoadGrd(myDsResult, nTypeCheck))
                {
                    throw new Exception("Errore in caricamento griglia");
                }
                DivAttesa.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali.CmdRicerca_Click.errore: ", ex);
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
                string IDRow = e.CommandArgument.ToString();
                if (e.CommandName == "RowOpen")
                {
                    switch (nTypeCheck)
                    {
                        case (int)ClsAnalisi.TypeCheckRifCatastali.RifMancanti://Rif.Catastali mancanti
                        case (int)ClsAnalisi.TypeCheckRifCatastali.DicNoCat://Rif.Dichiarati non a castato
                        case (int)ClsAnalisi.TypeCheckRifCatastali.CatEqualDic://Posizione Dichiarata uguale a Catastale
                        case (int)ClsAnalisi.TypeCheckRifCatastali.ICInoTARSU://Rif.Dichiarati in ICI ma non in TARSU
                                                                              //va alla pagina di dettaglio dell'immobile selezionato
                            foreach (GridViewRow myRow in GrdDichRifCatastali.Rows)
                            {
                                if (IDRow == ((HiddenField)myRow.FindControl("hfidoggetto")).Value)
                                {
                                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?Provenienza=CHECKRIFCAT&IDTestata=" + ((HiddenField)myRow.FindControl("hfidtestata")).Value + "&IDImmobile=" + IDRow + "&TYPEOPERATION=DETTAGLIO"), this.GetType());//&TypeCheck=" + Session["TypeCheckRifCat"]);
                                    break;
                                }
                            }
                            break;
                        case (int)ClsAnalisi.TypeCheckRifCatastali.CatDifferentDic://Cat. e/o Classe Dichiarata diversa da Catastale
                        case (int)ClsAnalisi.TypeCheckRifCatastali.RenCatDifferentDic://Rendita e/o Consistenza Catastale diversa da Dichiarata
                        case (int)ClsAnalisi.TypeCheckRifCatastali.PropCatDifferentDic://Proprietario Catastale diverso da Dichiarato
                                                                                       //va alla pagina di dettaglio dell'immobile selezionato
                            foreach (GridViewRow myRow in GrdDichCatRifCatastali.Rows)
                            {
                                if (IDRow == ((HiddenField)myRow.FindControl("hfidoggetto")).Value)
                                {
                                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_IMMOBILE_DETTAGLIO", "?Provenienza=CHECKRIFCAT&IDTestata=" + ((HiddenField)myRow.FindControl("hfidtestata")).Value + "&IDImmobile=" + IDRow + "&TYPEOPERATION=DETTAGLIO"), this.GetType());//&TypeCheck=" + Session["TypeCheckRifCat"]);
                                    break;
                                }
                            }
                            break;
                        case (int)ClsAnalisi.TypeCheckRifCatastali.TARSUnoICI://Rif.Dichiarati in TARSU ma non in ICI
                            foreach (GridViewRow myRow in GrdDichTARSUnoICI.Rows)
                            {
                                if (IDRow == ((HiddenField)myRow.FindControl("hfiddettagliotestata")).Value)
                                {
                                    string sScript, sParam = "";
                                    Type csType = this.GetType();
                                     sParam = "IdContribuente=" + ((HiddenField)myRow.FindControl("hfidcontribuente")).Value + "&IdTessera=" + ((HiddenField)myRow.FindControl("hfIDTESSERA")).Value + "&IdUniqueUI=" + ((HiddenField)myRow.FindControl("hfiddettagliotestata")).Value + "&AzioneProv=1&Provenienza=4&IdList=-1";//&TypeCheck=" + Session["TypeCheckRifCat"];
                                    sScript = "parent.Comandi.location.href = '../../aspVuotaRemoveComandi';";//"parent.Comandi.location.href = '../.." + ConstWrapper.Path_TARSU + "/Dichiarazioni/ComandiGestImmobili.aspx?Provenienza=4&AzioneProv=1';";
                                    sScript += "parent.Visualizza.location.href = '../.." + ConstWrapper.Path_TARSU + "/Dichiarazioni/GestImmobili.aspx?" + sParam + "';";
                                    sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                                    sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';";
                                    sScript += "";
                                    RegisterScript(sScript,this.GetType());
                                    break;
                                }
                            }
                            break;
                        case (int)ClsAnalisi.TypeCheckRifCatastali.RifErrataCopertura://Rif.Catastali con errata percentuale di possesso (< o > di 100)
                            foreach (GridViewRow myRow in GrdDichRifErrataCopertura.Rows)
                            {
                                if (IDRow == ((HiddenField)myRow.FindControl("hfid")).Value)
                                {
                                    ConstWrapper.Parametri = new System.Collections.Hashtable();
                                    ConstWrapper.Parametri["TipoRicerca"] = "Immobile";
                                    ConstWrapper.Parametri["Foglio"] = myRow.Cells[0].Text;
                                    ConstWrapper.Parametri["Numero"] = myRow.Cells[1].Text;
                                    ConstWrapper.Parametri["Subalterno"] = myRow.Cells[2].Text;
                                    ConstWrapper.Parametri["CategoriaCatastale"] = "-1";
                                    ConstWrapper.Parametri["Classe"] = "-1";
                                    ConstWrapper.Parametri["ConfrontoImporto"] = "-1";
                                    ConstWrapper.Parametri["Importo"] = "";
                                    ConstWrapper.Parametri["PercPos"] = "";
                                    ConstWrapper.Parametri["ImmobiliNoAgg"] = "";
                                    RegisterScript(ApplicationHelper.LoadFrameworkPage("SR_GESTIONE", ""), this.GetType());//?&TypeCheck=" + Session["TypeCheckRifCat"]);
                                    break;
                                }
                            }
                            break;
                        default://Rif.Catastali non in Dichiarazioni//Riferimenti accertati//Riferimenti chiusi e non riaperti//Riferimenti doppi per lo stesso periodo
                                //non va in nessuna pagina
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali.GrdRowCommand.errore: ", ex);
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
            LoadGrd((DataSet)Session["dsResultCheckRifCatastali"], nTypeCheck, e.NewPageIndex);
        }
        #endregion
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        protected void CmdStampa_Click(object sender, System.EventArgs e)
        {
            DataSet myDsResult = null;
            int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            ArrayList oListNomiColonne = new ArrayList();
            string[] aMyHeaders = null;
            string NameXLS = "";

            try
            {
                log.Debug("CheckRifCatastali::CmdStampa_Click::inizio::" + DateTime.Now.ToString());
                GrdDichRifCatastali.Style.Add("display", "none");
                GrdDichRifErrataCopertura.Style.Add("display", "none");
                GrdDichCatRifCatastali.Style.Add("display", "none");
                GrdDichTARSUnoICI.Style.Add("display", "none");
                log.Debug("CheckRifCatastali::CmdStampa_Click::devo prelevare i dati da stampare::" + DateTime.Now.ToString());
                myDsResult = FncCheck.GetCheckRifCatastali(ConstWrapper.StringConnection, ConstWrapper.CodiceEnte, int.Parse(DdlAnno.SelectedValue), nTypeCheck, true);
                log.Debug("CheckRifCatastali::CmdStampa_Click::dati prelevati::" + DateTime.Now.ToString());
                if (!(myDsResult == null))
                {
                    if (myDsResult.Tables[0].Rows.Count > 0)
                    {
                        //definisco l'insieme delle colonne da esportare
                        NameXLS = ConstWrapper.CodiceEnte + "_CONTROLLORIFERIMENTICATASTALI_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                        if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.DicNoCat || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatNoDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifMancanti || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifAccertati || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.ICInoTARSU)
                        {
                            oListNomiColonne.Add("Nominativo");
                            oListNomiColonne.Add("Cod.Fiscale/P.IVA");
                            oListNomiColonne.Add("Ubicazione");
                            oListNomiColonne.Add("Foglio");
                            oListNomiColonne.Add("Numero");
                            oListNomiColonne.Add("Subalterno");
                            oListNomiColonne.Add("Data Inizio");
                            oListNomiColonne.Add("Data Fine");
                            oListNomiColonne.Add("Cat.");
                            oListNomiColonne.Add("Classe");
                            oListNomiColonne.Add("Cons.");
                            oListNomiColonne.Add("Rendita");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                        }
                        else if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatEqualDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatDifferentDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RenCatDifferentDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.PropCatDifferentDic)
                        {
                            oListNomiColonne.Add("Nominativo");
                            oListNomiColonne.Add("Cod.Fiscale/P.IVA");
                            oListNomiColonne.Add("Ubicazione");
                            oListNomiColonne.Add("Foglio");
                            oListNomiColonne.Add("Numero");
                            oListNomiColonne.Add("Subalterno");
                            oListNomiColonne.Add("Data Inizio");
                            oListNomiColonne.Add("Data Fine");
                            oListNomiColonne.Add("Cat.");
                            oListNomiColonne.Add("Classe");
                            oListNomiColonne.Add("Cons.");
                            oListNomiColonne.Add("Rendita");
                            oListNomiColonne.Add("Perc.Possesso");
                            oListNomiColonne.Add("Nominativo Catastale");
                            oListNomiColonne.Add("Cod.Fiscale/P.IVA Catastale");
                            oListNomiColonne.Add("Cat. Catastale");
                            oListNomiColonne.Add("Classe Catastale");
                            oListNomiColonne.Add("Cons. Catastale");
                            oListNomiColonne.Add("Rendita Catastale");
                            oListNomiColonne.Add("Perc.Possesso Catastale");
                        }
                        else if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifErrataCopertura)
                        {
                            oListNomiColonne.Add("Cognome");
                            oListNomiColonne.Add("Nome");
                            oListNomiColonne.Add("Cod.Fiscale/P.IVA");
                            oListNomiColonne.Add("Foglio");
                            oListNomiColonne.Add("Numero");
                            oListNomiColonne.Add("Subalterno");
                            oListNomiColonne.Add("Data Inizio");
                            oListNomiColonne.Add("Data Fine");
                            oListNomiColonne.Add("Categoria");
                            oListNomiColonne.Add("Perc.Possesso");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                        }
                        else if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.TARSUnoICI)
                        {
                            oListNomiColonne.Add("Nominativo");
                            oListNomiColonne.Add("Cod.Fiscale/P.IVA");
                            oListNomiColonne.Add("Ubicazione");
                            oListNomiColonne.Add("Foglio");
                            oListNomiColonne.Add("Numero");
                            oListNomiColonne.Add("Subalterno");
                            oListNomiColonne.Add("Data Inizio");
                            oListNomiColonne.Add("Data Fine");
                            oListNomiColonne.Add("Cat.");
                            oListNomiColonne.Add("Mq");
                            oListNomiColonne.Add("Mq Tassabili");
                            oListNomiColonne.Add("N.Vani");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                            oListNomiColonne.Add("");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali. CmdStampa_Click.errore: ", ex);
                 Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                DivAttesa.Style.Add("display", "none");
            }
            if (myDsResult != null)
            {
                if (myDsResult.Tables.Count > 0)
                {
                    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Win");
                    aMyHeaders = ((string[])(oListNomiColonne.ToArray(typeof(string))));
                    log.Debug("CheckRifCatastali::CmdStampa_Click::richiamo RKLIB::" + DateTime.Now.ToString());
                    objExport.ExportDetails(myDsResult.Tables[0], iColumns, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, ConstWrapper.PathProspetti + NameXLS);
                    log.Debug("CheckRifCatastali::CmdStampa_Click::finito RKLIB::" + DateTime.Now.ToString());
                    LblDownloadFile.Text = NameXLS;
                }
            }
            LblDownloadFile.Style.Add("display", "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LblDownloadFile_Click(object sender, System.EventArgs e)
        {
            string sFileExport = ConstWrapper.PathProspetti + LblDownloadFile.Text;
            Response.ContentType = "*/*";
            Response.AppendHeader("content-disposition", ("attachment; filename=" + LblDownloadFile.Text));
            Response.WriteFile(sFileExport);
            Response.End();
        }

        private bool LoadGrd(DataSet myDSDatiGrd, int nType, int? page = 0)
        {
            try
            {
                //se necessario riassegno il checked all'opzione selezionata
                switch (nTypeCheck)
                {
                    case (int)ClsAnalisi.TypeCheckRifCatastali.RifChiusi:
                        OptRifChiusi.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.CatNoDic:
                        OptCatNoDic.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.CatEqualDic:
                        OptCatEqualDic.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.RifMancanti:
                        OptRifMancanti.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.DicNoCat:
                        OptDicNoCat.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.CatDifferentDic:
                        OptCatDifferentDic.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.RifDoppi:
                        OptRifDoppi.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.RenCatDifferentDic:
                        OptRenCatDifferentDic.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.RifAccertati:
                        OptRifAccertati.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.PropCatDifferentDic:
                        OptPropCatDifferentDic.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.RifErrataCopertura:
                        OptRifErrataCopertura.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.ICInoTARSU:
                        OptICInoTARSU.Checked = true;
                        break;
                    case (int)ClsAnalisi.TypeCheckRifCatastali.TARSUnoICI:
                        OptTARSUnoICI.Checked = true;
                        break;
                }
                log.Debug("CheckRifCatastali::LoadGrd::nTypeCheck::" + nTypeCheck.ToString());
                //popolo la griglia
                if (myDSDatiGrd != null)
                {
                    if (myDSDatiGrd.Tables.Count > 0)
                    {
                        if (myDSDatiGrd.Tables[0].Rows.Count > 0)
                        {
                            if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatEqualDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.DicNoCat || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatNoDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifMancanti || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifAccertati || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.ICInoTARSU)
                            {
                                GrdDichRifCatastali.DataSource = myDSDatiGrd.Tables[0];
                                if (page.HasValue)
                                    GrdDichRifCatastali.PageIndex = page.Value;
                                GrdDichRifCatastali.DataBind();
                                GrdDichRifCatastali.SelectedIndex = -1;
                                GrdDichRifCatastali.Style.Add("display", "");
                                if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifAccertati || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatNoDic)
                                    GrdDichRifCatastali.Columns[12].Visible = false;
                                else
                                    GrdDichRifCatastali.Columns[12].Visible = true;
                                GrdDichCatRifCatastali.Style.Add("display", "none");
                                GrdDichRifErrataCopertura.Style.Add("display", "none");
                                GrdDichTARSUnoICI.Style.Add("display", "none");
                                LblResult.Style.Add("display", "none");
                            }
                            else if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.CatDifferentDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RenCatDifferentDic || nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.PropCatDifferentDic)
                            {
                                GrdDichCatRifCatastali.DataSource = myDSDatiGrd.Tables[0];
                                if (page.HasValue)
                                    GrdDichCatRifCatastali.PageIndex = page.Value;
                                GrdDichCatRifCatastali.DataBind();
                                GrdDichCatRifCatastali.SelectedIndex = -1;
                                GrdDichCatRifCatastali.Style.Add("display", "");
                                if (nTypeCheck != (int)ClsAnalisi.TypeCheckRifCatastali.PropCatDifferentDic)
                                    GrdDichCatRifCatastali.Columns[18].Visible = false;
                                else
                                    GrdDichCatRifCatastali.Columns[18].Visible = true;
                                GrdDichRifErrataCopertura.Style.Add("display", "none");
                                GrdDichRifCatastali.Style.Add("display", "none");
                                GrdDichTARSUnoICI.Style.Add("display", "none");
                                LblResult.Style.Add("display", "none");
                            }
                            else if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.RifErrataCopertura)
                            {
                                GrdDichRifErrataCopertura.DataSource = myDSDatiGrd.Tables[0];
                                if (page.HasValue)
                                    GrdDichRifErrataCopertura.PageIndex = page.Value;
                                GrdDichRifErrataCopertura.DataBind();
                                GrdDichRifErrataCopertura.SelectedIndex = -1;
                                GrdDichRifErrataCopertura.Style.Add("display", "");
                                GrdDichCatRifCatastali.Style.Add("display", "none");
                                GrdDichRifCatastali.Style.Add("display", "none");
                                GrdDichTARSUnoICI.Style.Add("display", "none");
                                LblResult.Style.Add("display", "none");
                            }
                            else if (nTypeCheck == (int)ClsAnalisi.TypeCheckRifCatastali.TARSUnoICI)
                            {
                                GrdDichTARSUnoICI.DataSource = myDSDatiGrd.Tables[0];
                                if (page.HasValue)
                                    GrdDichTARSUnoICI.PageIndex = page.Value;
                                GrdDichTARSUnoICI.DataBind();
                                GrdDichTARSUnoICI.SelectedIndex = -1;
                                GrdDichTARSUnoICI.Style.Add("display", "");
                                GrdDichCatRifCatastali.Style.Add("display", "none");
                                GrdDichRifCatastali.Style.Add("display", "none");
                                GrdDichRifErrataCopertura.Style.Add("display", "none");
                                LblResult.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            LblResult.Text = "La ricerca non ha prodotto risultati.";
                            GrdDichCatRifCatastali.Style.Add("display", "none");
                            GrdDichRifCatastali.Style.Add("display", "none");
                            LblResult.Style.Add("display", "");
                        }
                    }
                    else
                    {
                        LblResult.Text = "La ricerca non ha prodotto risultati.";
                        GrdDichCatRifCatastali.Style.Add("display", "none");
                        GrdDichRifCatastali.Style.Add("display", "none");
                        LblResult.Style.Add("display", "");
                    }
                }
                Session["paginaSelezionata"] = null;
                log.Debug("svuoto session'TypeCheckRifCat'");
                Session["TypeCheckRifCat"] = null;
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali.LoadGrd.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ClearTypeCheck(object sender, System.EventArgs e)
        {
            try
            {
                GrdDichRifCatastali.Style.Add("display", "none");
                GrdDichCatRifCatastali.Style.Add("display", "none");
                GrdDichRifErrataCopertura.Style.Add("display", "none");
                GrdDichTARSUnoICI.Style.Add("display", "none");
                LblResult.Style.Add("display", "none");
                Session["TypeCheckRifCat"] = 0;
                Session["dsResultCheckRifCatastali"] = null;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CheckRifCatastali.ClearTypeCheck.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
    }
}
