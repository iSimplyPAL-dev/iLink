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
using DichiarazioniICI.Database;
using log4net; 
using Business;


namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Pagina per la configurazione delle aliqutoe.
    /// Contiene i parametri di configurazione e le funzioni della comandiera.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ConfigAliquote : BaseEnte
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfigAliquote));

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
        /// Caricamento pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
            string sCodAliquota = String.Empty;
            try
            {
                if (!(Page.IsPostBack))
                {
                    int ID_ALIQUOTA;
                    this.LoadDDL();

                    if (Request["ID_ALIQUOTA"] == null)
                    {
                        ID_ALIQUOTA = -1;
                    }
                    else
                    {
                        ID_ALIQUOTA = int.Parse(Request["ID_ALIQUOTA"].ToString());
                        Aliquote objAliquote = new Aliquote();

                        DataTable dtAliquote;
                        dtAliquote = objAliquote.ListaAliquote(ddlTributo.SelectedValue, ID_ALIQUOTA, "-1", "-1", -1, ConstWrapper.CodiceEnte, -1, -1);
                        sCodAliquota = dtAliquote.Rows[0]["TIPO"].ToString();
                        LoadAliquota(dtAliquote);
                    }

                    ViewState.Add("ID_ALIQUOTA", ID_ALIQUOTA);
                    ViewState.Add("OLD_COD_ALIQUOTA", sCodAliquota);

                    string strscript = "";
                    switch (sCodAliquota)
                    {
                        case "AAP":
                            //VISUALIZZO configurazione AAP
                            strscript = strscript + "NascondiVisualizzaAAP('');";
                            break;
                        case "AUG1":
                        case "AUG2":
                        case "AUG3":
                            //VISUALIZZO configurazione AUG
                            strscript = strscript + "NascondiVisualizzaAUG('');";
                            break;
                        case "APEX":
                            //VISUALIZZO configurazione AAP
                            strscript = strscript + "NascondiVisualizzaAPEX('');";
                            break;
                        default:
                            //nascondo configurazione DSAAP
                            strscript = strscript + "NascondiVisualizzaAAP('none');";
                            strscript = strscript + "NascondiVisualizzaAUG('none');";
                            strscript = strscript + "NascondiVisualizzaAPEX('none');";
                            break;
                    }
                    //Caricamento griglie
                    DataView VistaCat = new CategoriaCatastaleTable(ConstWrapper.sUsername).ListaCategorie("A");//L'esclusione si applica solo all'abitazione principale quindi filtro solo il gruppo A
                    VistaCat.Sort = "CategoriaCatastale";
                    GrdCategorie.DataSource = VistaCat;
                    GrdCategorie.DataBind();
                    Session["CatCat"] = VistaCat;

                    // COMMENTO PARTE RELATIVA ALLA DETRAZIONE STATALE
                    if (sCodAliquota == "AAP" || sCodAliquota == "AUG1" || sCodAliquota == "AUG2" || sCodAliquota == "AUG3")
                    {
                        //Controllo quali categorie sono state flaggate come da escludere
                        DataView VistaCatEscl = new CategorieEscluse(ConstWrapper.sUsername).ListaCategorieEscluse(ConstWrapper.CodiceEnte, txtAnno.Text, sCodAliquota, ddlTributo.SelectedValue);
                        if (VistaCatEscl.Count > 0)
                        {
                            foreach (DataRow myRow in VistaCatEscl.Table.Rows)
                            {
                                foreach (GridViewRow oRow in GrdCategorie.Rows)
                                {
                                    string catescl = myRow["COD_CAT"].ToString();
                                    string cat = oRow.Cells[1].Text.ToString();
                                    if (catescl == cat)
                                    {
                                        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                                        chkSel.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!optSogliaRenditaFinoA.Checked && !optSogliaRenditaPartireDa.Checked)
                        optSogliaRenditaPartireDa.Checked = true;
                    strscript = strscript + "parent.Comandi.location.href='./CConfigAliquote.aspx';";
                    RegisterScript(strscript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void Page_Load(object sender, System.EventArgs e)
        //{
        //	// Put user code to initialize the page here
        //	string sCodAliquota=String.Empty; 
        //	try{
        //              if (!(Page.IsPostBack))
        //              {
        //                  int ID_ALIQUOTA;
        //                  this.LoadDDL();

        //                  if (Request["ID_ALIQUOTA"] == null)
        //                  {
        //                      ID_ALIQUOTA = -1;
        //                  }
        //                  else
        //                  {
        //                      ID_ALIQUOTA = int.Parse(Request["ID_ALIQUOTA"].ToString());
        //                      Aliquote objAliquote = new Aliquote();

        //                      DataTable dtAliquote;
        //                      //***20140509 - TASI ***
        //                      //*** 20130422 - aggiornamento IMU ***
        //                      //dtAliquote = objAliquote.ListaAliquote(ID_ALIQUOTA, "-1", "-1", -1, ConstWrapper.CodiceEnte, -1, -1);
        //                      dtAliquote = objAliquote.ListaAliquote(ddlTributo.SelectedValue, ID_ALIQUOTA, "-1", "-1", -1, ConstWrapper.CodiceEnte, -1, -1);
        //                      //*** ***
        //                      //*** ***
        //                      sCodAliquota = dtAliquote.Rows[0]["TIPO"].ToString();
        //                      LoadAliquota(dtAliquote);
        //                  }

        //                  ViewState.Add("ID_ALIQUOTA", ID_ALIQUOTA);
        //                  ViewState.Add("OLD_COD_ALIQUOTA", sCodAliquota);

        //                  string strscript = "";

        //                  // COMMENTO SU CODICE DETRAZIONE STATALE
        //                  /*if (sCodAliquota=="DSAAP")
        //                  {
        //                      //Visualizzazione configurazione DSAAP
        //                      strscript = strscript + "NascondiVisualizzaDSAAP('');";
        //                  }
        //                  else */

        //                  switch (sCodAliquota)
        //                  {
        //                      case "AAP":
        //                          //VISUALIZZO configurazione AAP
        //                          strscript = strscript + "NascondiVisualizzaAAP('');";
        //                          break;
        //                      case "AUG1":
        //                      case "AUG2":
        //                      case "AUG3":
        //                          //VISUALIZZO configurazione AUG
        //                          strscript = strscript + "NascondiVisualizzaAUG('');";
        //                          break;
        //                      case "APEX":
        //                          //VISUALIZZO configurazione AAP
        //                          strscript = strscript + "NascondiVisualizzaAPEX('');";
        //                          break;
        //                      default:
        //                          //nascondo configurazione DSAAP
        //                          strscript = strscript + "NascondiVisualizzaAAP('none');";
        //                          strscript = strscript + "NascondiVisualizzaAUG('none');";
        //                          strscript = strscript + "NascondiVisualizzaAPEX('none');";
        //                          break;
        //                  }

        //                  //				if (sCodAliquota == "AAP")
        //                  //				{
        //                  //					//VISUALIZZO configurazione AAP
        //                  //					strscript = strscript + "NascondiVisualizzaAAP('');";
        //                  //				}
        //                  //				else
        //                  //				{
        //                  //					if (sCodAliquota == "AUG1" || sCodAliquota == "AUG2" || sCodAliquota == "AUG3")
        //                  //					{
        //                  //						//VISUALIZZO configurazione AUG
        //                  //						strscript = strscript + "NascondiVisualizzaAUG('');";
        //                  //					}
        //                  //					else
        //                  //					{
        //                  //						//nascondo configurazione DSAAP
        //                  //						strscript = strscript + "NascondiVisualizzaAUG('none');";
        //                  //					}
        //                  //					//nascondo configurazione DSAAP
        //                  //					//strscript = strscript + "NascondiVisualizzaAAP('none');";
        //                  //				}

        //                  //Caricamento griglie
        //                  DataView VistaCat = new CategoriaCatastaleTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
        //                  VistaCat.Sort = "CategoriaCatastale";
        //                  GrdCategorie.DataSource = VistaCat;
        //                  GrdCategorie.DataBind();
        //                  Session["CatCat"] = VistaCat;

        //                  //DataView VistaTitoli = new UtilizzoTable(ConstWrapper.sUsername).ListFromSP().DefaultView;
        //                  //VistaTitoli.Sort = "Descrizione";
        //                  //grdTitoli.DataSource = VistaTitoli;
        //                  //grdTitoli.DataBind();

        //                  // COMMENTO PARTE RELATIVA ALLA DETRAZIONE STATALE
        //                  // if ((sCodAliquota=="DSAAP")||(sCodAliquota=="AAP")){ 
        //                  if (sCodAliquota == "AAP" || sCodAliquota == "AUG1" || sCodAliquota == "AUG2" || sCodAliquota == "AUG3")
        //                  {
        //                      //Controllo quali categorie sono state flaggate come da escludere
        //                      DataView VistaCatEscl = new CategorieEscluse(ConstWrapper.sUsername).ListaCategorieEscluse(ConstWrapper.CodiceEnte, txtAnno.Text, sCodAliquota, ddlTributo.SelectedValue);
        //                      if (VistaCatEscl.Count > 0)
        //                      {
        //                          foreach (DataRow myRow in VistaCatEscl.Table.Rows)
        //                          {
        //                              foreach (GridViewRow oRow in GrdCategorie.Rows)
        //                              {
        //                                  string catescl = myRow["COD_CAT"].ToString();
        //                                  string cat = oRow.Cells[1].Text.ToString();
        //                                  if (catescl == cat)
        //                                  {
        //                                      CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
        //                                      chkSel.Checked = true;
        //                                  }
        //                              }
        //                          }
        //                      }

        //                      //Controllo quali Tipi posesso sono stati flaggati come da escludere
        //                      /* COMMENTO DETRAZIONE STATALE
        //                      DataView VistaTipoPossEscl= new TipoPossessoEscluso(ConstWrapper.sUsername).ListaTipoPossessoEscluse(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, sCodAliquota);  
        //                      if (VistaTipoPossEscl.Count>0)
        //                      {
        //                          for (int i=0;i<VistaTipoPossEscl.Count;i++)
        //                          {
        //                              foreach (GridViewRow oRow in grdTitoli.Items) 
        //                              {
        //                                  string catescl=VistaTipoPossEscl.Table.Rows[i]["ID_POSS"].ToString();
        //                                  string cat=oRow.Cells[1].Text.ToString();
        //                                  if (catescl==cat)
        //                                  {
        //                                      CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
        //                                      chkSel.Checked=true; 
        //                                  }
        //                              }
        //                          }  
        //                      }*/

        //                  }
        //                  if (!optSogliaRenditaFinoA.Checked && !optSogliaRenditaPartireDa.Checked)
        //                      optSogliaRenditaPartireDa.Checked = true;
        //                  strscript = strscript + "parent.Comandi.location.href='./CConfigAliquote.aspx';";
        //                  RegisterScript(strscript,this.GetType());
        //	    }
        //          }
        //          catch (Exception ex)
        //          {
        //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.Page_Load.errore: ", ex);
        //              Response.Redirect("../../PaginaErrore.aspx");
        //          }
        //}

        /// <summary>
        /// Gestione del cambio pagina della griglia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdPageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            try
            {
                GrdCategorie.DataSource = Session["CatCat"];
                if (page.HasValue)
                    GrdCategorie.PageIndex = page.Value;
                GrdCategorie.DataBind();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.LoadSearch.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                throw ex;
            }
        }

        /// <summary>
        /// Funzione per la valorizzazione della combo delle aliquote
        /// </summary>
        /// <param name="dtAliquote">DataTable contenente la lista delle aliquote</param>
        private void LoadAliquota(DataTable dtAliquote)
        {
            try
            {
                /*
                if (dtAliquote.Rows[0]["TIPO"].ToString()=="DSAAP") 
                {
                    txtMagDetraz.Text =dtAliquote.Rows[0]["VALORE"].ToString();
                    txtTettoMassimo.Text =dtAliquote.Rows[0]["TETTO_MASSIMO"].ToString();
                }
                else
                {
                */
                txtValore.Text = dtAliquote.Rows[0]["VALORE"].ToString();
                /*}*/
                //*** 20120530 - IMU ***
                TxtAliquotaStatale.Text = dtAliquote.Rows[0]["ALIQUOTA_STATALE"].ToString();
                //*** ***
                txtAnno.Text = dtAliquote.Rows[0]["ANNO"].ToString();
                int iCount = 0;
                for (iCount = 0; iCount <= ddlTipo.Items.Count - 1; iCount++)
                {
                    if (ddlTipo.Items[iCount].Value == dtAliquote.Rows[0]["TIPO"].ToString())
                    {
                        ddlTipo.SelectedIndex = iCount;
                        break;
                    }
                }
                //*** 20140509 - TASI ***
                for (iCount = 0; iCount <= ddlTributo.Items.Count - 1; iCount++)
                {
                    if (ddlTributo.Items[iCount].Value == dtAliquote.Rows[0]["CODTRIBUTO"].ToString())
                    {
                        ddlTributo.SelectedIndex = iCount;
                        break;
                    }
                }
                txtSogliaRendita.Text = dtAliquote.Rows[0]["SOGLIARENDITA"].ToString();
                if (dtAliquote.Rows[0]["TIPOSOGLIA"].ToString() == ">")
                {
                    optSogliaRenditaPartireDa.Checked = true;
                }
                else
                {
                    optSogliaRenditaFinoA.Checked = true;
                }
                //*** ***
                //*** 20150430 - TASI Inquilino ***
                if (dtAliquote.Rows[0]["percinquilino"] != null)
                    TxtPercInquilino.Text = dtAliquote.Rows[0]["percinquilino"].ToString();
                //*** ***
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.LoadAliquota.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// Valorizzazione delle Combo presenti nella videata
        /// </summary>
        private void LoadDDL()
        {
            try
            {
                //anno
                //*** 20150430 - TASI Inquilino ***
                //for (int i = DateTime.Today.Year + 1; i > 1989; i--)
                //{
                //    myitem = new ListItem();
                //    myitem.Value = i.ToString();
                //    myitem.Text = i.ToString();
                //    ddlAnno.Items.Add(myitem);
                //}
                //*** ***
                //tipo aliquota
                ddlTipo.DataBind();

                //*** 20140509 - TASI ***
                ListItem myListItem = new ListItem();
                myListItem.Text = "...";
                myListItem.Value = "";
                ddlTributo.Items.Add(myListItem);

                DataView myDataview = new Aliquote().ListaTributo();
                foreach (DataRow myRow in myDataview.Table.Rows)
                {
                    ListItem myListItem1 = new ListItem();
                    myListItem1.Text = (string)myRow["DESCR"];
                    myListItem1.Value = (string)myRow["TIPO"];
                    ddlTributo.Items.Add(myListItem1);
                }
                //*** ***
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.LoadDDL.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnSalva_Click(object sender, System.EventArgs e)
        {
            Aliquote objAliquote = new Aliquote();
            int ID_ALIQUOTA_OLD;
            bool iretval = false;
            string Valore = "0";
            string TettoMax = "";
            DataTable dtAliquote;
            string sTipoSoglia = ">";

            try
            {
                if (Request["ID_ALIQUOTA"] == null)
                {
                    ID_ALIQUOTA_OLD = -1;
                }
                else
                {
                    ID_ALIQUOTA_OLD = int.Parse(Request["ID_ALIQUOTA"].ToString());
                }

                if (ddlTipo.SelectedItem.Value == "DSAAP")
                {
                    Valore = txtMagDetraz.Text;
                    TettoMax = txtTettoMassimo.Text;
                }
                else
                {
                    Valore = txtValore.Text;
                }

                //*** 20120530 - IMU ***
                double AliquotaStatale = 0;
                if (TxtAliquotaStatale.Text != string.Empty)
                {
                    AliquotaStatale = double.Parse(TxtAliquotaStatale.Text);
                }
                //*** ***
                //*** 20140509 - TASI ***
                if (optSogliaRenditaFinoA.Checked)
                {
                    sTipoSoglia = "<";
                }
                //*** 20130422 - aggiornamento IMU ***
                //dtAliquote = objAliquote.ListaAliquote(-1, ddlAnno.SelectedItem.Value, ddlTipo.SelectedItem.Value, -1, ConstWrapper.CodiceEnte, ID_ALIQUOTA_OLD, -1);
                dtAliquote = objAliquote.ListaAliquote(ddlTributo.SelectedValue, -1, txtAnno.Text, ddlTipo.SelectedItem.Value, -1, ConstWrapper.CodiceEnte, ID_ALIQUOTA_OLD, -1);
                //*** ***
                //*** ***
                if (dtAliquote.Rows.Count > 0)
                {
                    string strscript1 = "";
                    /*
                     * if (ddlTipo.SelectedItem.Value=="DSAAP")
                    {
                        //Visualizzazione configurazione DSAAP
                        strscript1 += "NascondiVisualizzaDSAAP('');";
                    }
                    else
                    {
                        //nascondo configurazione DSAAP
                        strscript1 += "NascondiVisualizzaDSAAP('none');";
                    }
                    */
                    if (ddlTipo.SelectedItem.Value == "AAP")
                    {
                        //Visualizzazione configurazione AAP
                        strscript1 += "NascondiVisualizzaAAP('');";
                    }
                    else
                    {
                        //nascondo configurazione AAP
                        strscript1 += "NascondiVisualizzaAAP('none');";
                    }
                    if (ddlTipo.SelectedItem.Value == "AUG1" || ddlTipo.SelectedItem.Value == "AUG2" || ddlTipo.SelectedItem.Value == "AUG3")
                    {
                        //Visualizzazione configurazione AUG
                        strscript1 += "NascondiVisualizzaAUG('');";
                    }
                    else
                    {
                        //nascondo configurazione AUG
                        strscript1 += "NascondiVisualizzaAUG('none');";
                    }
                    if (ddlTipo.SelectedItem.Value == "APEX")
                    {
                        //Visualizzazione configurazione APEX
                        strscript1 += "NascondiVisualizzaAPEX('');";
                    }
                    else
                    {
                        //nascondo configurazione AAP
                        strscript1 += "NascondiVisualizzaAPEX('none');";
                    }
                    strscript1 += "GestAlert('a', 'warning', '', '', 'Aliquota/detrazione già configurata a sistema per Anno, Tipo e Valore!');";
                    RegisterScript(strscript1, this.GetType());
                    return;
                }

                //*** 20140509 - TASI ***
                //// SE LA ALIQUOTA SELEZIONATA E DI TIPO AAP
                //if (ddlTipo.SelectedItem.Value == "AAP")
                //{
                //    CategorieEscluse objCatEscluse = new CategorieEscluse(ConstWrapper.sUsername);
                //    TipoPossessoEscluso objTipoPossEscluso = new TipoPossessoEscluso(ConstWrapper.sUsername);

                //    //Elimino i dati relativi all'ente per l'anno selezionato
                //    iretval = objCatEscluse.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, "AAP");
                //    iretval = objTipoPossEscluso.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value);

                //    string cat = String.Empty;
                //    foreach (GridViewRow oRow in GrdCategorie.Items)
                //    {
                //        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //        if (chkSel.Checked)
                //        {
                //            cat = oRow.Cells[1].Text.ToString();
                //            iretval = objCatEscluse.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, cat, "AAP");
                //        }
                //    }

                //    string ID_POSS = String.Empty;
                //    foreach (GridViewRow oRow in grdTitoli.Items)
                //    {
                //        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //        if (chkSel.Checked)
                //        {
                //            ID_POSS = oRow.Cells[1].Text.ToString();
                //            iretval = objTipoPossEscluso.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, ID_POSS);
                //        }
                //    }
                //}

                //// SE LA ALIQUOTA SELEZIONATA E DI TIPO AUG 1-2-3
                //if (ddlTipo.SelectedItem.Value == "AUG1" || ddlTipo.SelectedItem.Value == "AUG2" || ddlTipo.SelectedItem.Value == "AUG3")
                //{
                //    CategorieEscluse objCatEscluse = new CategorieEscluse(ConstWrapper.sUsername);
                //    TipoPossessoEscluso objTipoPossEscluso = new TipoPossessoEscluso(ConstWrapper.sUsername);

                //    //Elimino i dati relativi all'ente per l'anno selezionato
                //    iretval = objCatEscluse.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, ddlTipo.SelectedItem.Value);
                //    iretval = objTipoPossEscluso.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value);

                //    string cat = String.Empty;
                //    foreach (GridViewRow oRow in GrdCategorie.Items)
                //    {
                //        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //        if (chkSel.Checked)
                //        {
                //            cat = oRow.Cells[1].Text.ToString();
                //            iretval = objCatEscluse.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, cat, ddlTipo.SelectedItem.Value);
                //        }
                //    }

                //    string ID_POSS = String.Empty;
                //    foreach (GridViewRow oRow in grdTitoli.Items)
                //    {
                //        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //        if (chkSel.Checked)
                //        {
                //            ID_POSS = oRow.Cells[1].Text.ToString();
                //            iretval = objTipoPossEscluso.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, ID_POSS);
                //        }
                //    }
                //}

                //// SE LA ALIQUOTA SELEZIONATA E DI TIPO APEX
                //if (ddlTipo.SelectedItem.Value == "APEX")
                //{
                //    CategorieEscluse objCatEscluse = new CategorieEscluse(ConstWrapper.sUsername);
                //    TipoPossessoEscluso objTipoPossEscluso = new TipoPossessoEscluso(ConstWrapper.sUsername);

                //    //Elimino i dati relativi all'ente per l'anno selezionato
                //    iretval = objCatEscluse.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, "APEX");
                //    iretval = objTipoPossEscluso.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value);

                //    string cat = String.Empty;
                //    foreach (GridViewRow oRow in GrdCategorie.Items)
                //    {
                //        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //        if (chkSel.Checked)
                //        {
                //            cat = oRow.Cells[1].Text.ToString();
                //            iretval = objCatEscluse.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, cat, "APEX");
                //        }
                //    }

                //    string ID_POSS = String.Empty;
                //    foreach (GridViewRow oRow in grdTitoli.Items)
                //    {
                //        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //        if (chkSel.Checked)
                //        {
                //            ID_POSS = oRow.Cells[1].Text.ToString();
                //            iretval = objTipoPossEscluso.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, ID_POSS);
                //        }
                //    }
                //}

                // SE LA ALIQUOTA SELEZIONATA E DI TIPO AAP/AUG/APEX
                if (ddlTipo.SelectedItem.Value == "AAP"
                    || ddlTipo.SelectedItem.Value == "AUG1" || ddlTipo.SelectedItem.Value == "AUG2" || ddlTipo.SelectedItem.Value == "AUG3"
                    || ddlTipo.SelectedItem.Value == "APEX")
                {
                    iretval = SetEsclusioneCategorie(ddlTipo.SelectedItem.Value);
                    iretval = SetEsclusioneTipoPossesso();
                }

                //if (ViewState["ID_ALIQUOTA"].ToString() == "-1")
                //{
                //    // inserimento nuova aliquota per anno selezionato
                //    iretval = objAliquote.InsertAliquota(-1, ddlAnno.SelectedItem.Value, ddlTipo.SelectedItem.Value, double.Parse(Valore), AliquotaStatale, ConstWrapper.CodiceEnte, 0, TettoMax);
                //}
                //else
                //{
                //    // modifica aliquota esistente
                //    iretval = objAliquote.InsertAliquota(int.Parse(ViewState["ID_ALIQUOTA"].ToString()), ddlAnno.SelectedItem.Value, ddlTipo.SelectedItem.Value, double.Parse(Valore), AliquotaStatale, ConstWrapper.CodiceEnte, 1, TettoMax);
                //}
                //*** 20150430 - TASI Inquilino ***
                //iretval = objAliquote.InsertAliquota(ddlTributo.SelectedValue, ID_ALIQUOTA_OLD, ddlAnno.SelectedItem.Value, ddlTipo.SelectedItem.Value, double.Parse(Valore), AliquotaStatale, double.Parse(txtSogliaRendita.Text),sTipoSoglia, ConstWrapper.CodiceEnte, TettoMax);
                iretval = objAliquote.InsertAliquota(ddlTributo.SelectedValue, ID_ALIQUOTA_OLD, txtAnno.Text, ddlTipo.SelectedItem.Value, double.Parse(Valore), AliquotaStatale, double.Parse(TxtPercInquilino.Text), double.Parse(txtSogliaRendita.Text), sTipoSoglia, ConstWrapper.CodiceEnte, TettoMax);
                //*** ***
                //*** ***
                /*
                 * if (ddlTipo.SelectedItem.Value=="DSAAP")
                {//Salvataggio dati per DSAAP
                    CategorieEscluse objCatEscluse=new CategorieEscluse (ConstWrapper.sUsername);
                    TipoPossessoEscluso objTipoPossEscluso=new TipoPossessoEscluso(ConstWrapper.sUsername);
				
                    //Elimino i dati relativi all'ente per l'anno selezionato
                    iretval=objCatEscluse.delete (ConstWrapper.CodiceEnte,ddlAnno.SelectedItem.Value, "DSAAP");
                    iretval=objTipoPossEscluso.delete (ConstWrapper.CodiceEnte,ddlAnno.SelectedItem.Value);
                    //Ciclo sulla griglia delle categorie e inserisco quelle selezionate

                    string cat=String.Empty;
                    foreach (GridViewRow oRow in GrdCategorie.Items) 
                    {
                        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                        if (chkSel.Checked){
                            cat=oRow.Cells[1].Text.ToString();
                            iretval=objCatEscluse.Insert(ConstWrapper.CodiceEnte,ddlAnno.SelectedItem.Value,cat, "DSAAP");
                        }
                    }

                    string ID_POSS=String.Empty;
                    foreach (GridViewRow oRow in grdTitoli.Items) 
                    {
                        CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                        if (chkSel.Checked)
                        {
                            ID_POSS=oRow.Cells[1].Text.ToString();
                            iretval=objTipoPossEscluso.Insert(ConstWrapper.CodiceEnte,ddlAnno.SelectedItem.Value,ID_POSS);
                        }
                    }
                }
                */

                string strscript = "";
                if (ddlTipo.SelectedItem.Value == "AAP")
                {
                    //Visualizzazione configurazione AAP
                    strscript += "NascondiVisualizzaAAP('');";
                }
                else
                {
                    if (ddlTipo.SelectedItem.Value == "AUG1" || ddlTipo.SelectedItem.Value == "AUG2" || ddlTipo.SelectedItem.Value == "AUG3")
                    {
                        //Visualizzazione configurazione AAP
                        strscript += "NascondiVisualizzaAUG('');";
                    }
                    else
                    {
                        if (ddlTipo.SelectedItem.Value == "APEX")
                        {
                            //Visualizzazione configurazione APEX
                            strscript += "NascondiVisualizzaAPEX('');";
                        }
                        else
                        {
                            //nascondo configurazione DSAAP
                            strscript += "NascondiVisualizzaAUG('none');";
                            strscript += "NascondiVisualizzaAAP('none');";
                            strscript += "NascondiVisualizzaAPEX('none');";
                        }
                    }
                }
                strscript += "GestAlert('a', '" + (iretval == true ? "success" : "warning") + "', '', '', 'Salvataggio" + (iretval == true ? "" : " non") + " effettuato.');";
                RegisterScript(strscript, this.GetType());

                if (iretval == true)
                    //RegisterScript(sScript,this.GetType());,"back", "parent.Visualizza.location.href='RicercaAliquote.aspx?Anno=" + ddlAnno.SelectedItem.Value.ToString() + "';");
                    RegisterScript("location.href='RicercaAliquote.aspx?Anno=" + txtAnno.Text + "';", this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.btnSalva_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnDelete_Click(object sender, System.EventArgs e)
        {

            Aliquote objAliquote = new Aliquote();
            bool iretval;
            try
            {
                CategorieEscluse objCatEscluse = new CategorieEscluse(ConstWrapper.sUsername);
                //TipoPossessoEscluso objTipoPossEscluso=new TipoPossessoEscluso(ConstWrapper.sUsername);
                //Elimino i dati relativi all'ente per l'anno selezionato
                iretval = objCatEscluse.Delete(ConstWrapper.CodiceEnte, txtAnno.Text, ddlTipo.SelectedItem.Value, ddlTributo.SelectedItem.Value);
                //iretval = objTipoPossEscluso.delete(ConstWrapper.CodiceEnte, txtAnno.Text);
                //*** 20140509 - TASI ***
                //iretval=objAliquote.InsertAliquota(int.Parse(ViewState["ID_ALIQUOTA"].ToString()),"","",0,-1,ConstWrapper.CodiceEnte, 2,"");
                iretval = objAliquote.DeleteAliquota(int.Parse(ViewState["ID_ALIQUOTA"].ToString()));
                //*** ***				
                //RegisterScript(sScript,this.GetType());,"", "<script language='javascript'>" + strscript + ""); 

                string strscript = "";
                strscript = "GestAlert('a', '" + (iretval == true ? "success" : "warning") + "', '', '', 'Cancellazione" + (iretval == true ? "" : " non") + " effettuata.');";
                strscript = strscript + "location.href='RicercaAliquote.aspx?Anno=" + txtAnno.Text + "';";
                RegisterScript(strscript, this.GetType());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.btnDelete_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        #region METODI
        /// <summary>
        /// Ritorna una DataView valorizzata con l'elenco degli anni di riferimento.
        /// </summary>
        /// <returns></returns>
        protected DataView GetTipoAliquote()
        {
            DataView Vista = new Aliquote().ListaTipoAliquote("-1");
            Vista.Sort = "";
            return Vista;
        }
        #endregion

        //*** 20140509 - TASI ***
        private bool SetEsclusioneTipoPossesso()
        {
            //bool iretval = false;

            try
            {
                //TipoPossessoEscluso objTipoPossEscluso = new TipoPossessoEscluso(ConstWrapper.sUsername);
                ////cancello
                //iretval = objTipoPossEscluso.delete(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value);
                ////e reinserisco
                //foreach (GridViewRow oRow in grdTitoli.Items)
                //{
                //    CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                //    if (chkSel.Checked)
                //    {
                //        iretval = objTipoPossEscluso.Insert(ConstWrapper.CodiceEnte, ddlAnno.SelectedItem.Value, oRow.Cells[1].Text.ToString());
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.SetEsclusioneTipoPossesso.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                return false;
            }
        }

        private bool SetEsclusioneCategorie(string sTipoAliquota)
        {
            bool iretval = false;

            try
            {
                CategorieEscluse objCatEscluse = new CategorieEscluse(ConstWrapper.sUsername);
                //cancello
                iretval = objCatEscluse.Delete(ConstWrapper.CodiceEnte, txtAnno.Text, sTipoAliquota, ddlTributo.SelectedItem.Value);
                //e reinserisco

                foreach (GridViewRow oRow in GrdCategorie.Rows)
                {
                    CheckBox chkSel = (CheckBox)oRow.FindControl("chkEsclusione");
                    if (chkSel.Checked)
                    {
                        iretval = objCatEscluse.Insert(ConstWrapper.CodiceEnte, txtAnno.Text, oRow.Cells[1].Text.ToString(), sTipoAliquota, ddlTributo.SelectedItem.Value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfigAliquote.SetEsclusioneCategorie.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                return false;
            }
        }
        //*** ***
    }
}
