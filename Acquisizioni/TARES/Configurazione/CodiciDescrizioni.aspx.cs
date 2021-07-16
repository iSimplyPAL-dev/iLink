using System;
using System.Globalization;
using System.Web.UI.WebControls;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using log4net;

namespace OPENgov.Acquisizioni.TARES.Configurazione
{/// <summary>
/// Pagina per la consultazione/gestione della configurazione dei codici/descrizioni per:
/// - Categorie
/// - Tipo Conferimenti
/// - Tariffe Conferimenti
/// - Riduzioni
/// - Esenzioni
/// - Maggiorazioni.
/// Le possibili opzioni sono:
/// - Importa le Categorie TARES Ministeriali
/// - Inserisci nuovo
/// - Cerca
/// - Salva
/// - Torna alla videata precedente
/// </summary>
    public partial class CodiciDescrizioni : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CodiciDescrizioni));
        //*** 201712 - gestione tipo conferimento***
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "TARES/TARI - Configurazione - Codici descrizioni";
            Enti ente = new Enti(new DBConfig().DBType, new DBConfig().ConnectionStringGENERAL) { CodEnte = Ente };
            if (ente.Load())
            {
                lblDatiComune.Text = "Risultati della ricerca come Comune del " + ente.PosizioneGeografica;
                if (ente.fk_IdTypeAteco == 1)
                    lblDatiComune.Text += " > 5.000 abitanti";
                else
                    lblDatiComune.Text += " < 5.000 abitanti";
            }
            if (FromVariabile == "1")
                optTipoConferimenti.Visible = true;
            else
                optTipoConferimenti.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            //ManageBottoniera(false);
            LoadCombo();
            LoadGridView(0);
        }

        protected void  ChangeTypeCodDescr(object sender, System.EventArgs e)
        {
            ShowDiv("divSelect"); HideDiv("divEdit"); HideDiv("editCodDescr"); 
            if (optCategorie.Checked == true)
            {
                HideDiv("searchRidEse"); ShowDiv("searchCategoria");
                cmdCopyCategories.Visible = true;
                LoadCombo();
            }
            else 
            {
                HideDiv("searchCategoria"); ShowDiv("searchRidEse");
                cmdCopyCategories.Visible = false;
            }
            LoadGridView(0);
        }

        protected void CmdCopyCategoriesClick(object sender, EventArgs e)
        {
            try
            {
                CategorieAteco ateco = new CategorieAteco {Ente = Ente};
                foreach (var item in ateco.LoadAll())
                {
                    CategorieAtecoEnte atecoEnte = new CategorieAtecoEnte
                        {
                            FromVariabile=FromVariabile,
                            CodiceCategoria = item.CodiceCategoria,
                            Definizione = item.Definizione,
                            Ente = item.Ente,
                            FKIdTypeAteco = item.FKIdTypeAteco,
                            Domestica = item.CodiceCategoria == "DOM"
                        };
                    atecoEnte.Save();
                }
                string sScript;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript = "<script language='javascript'>";
                sScript += "alert('Categorie importate.');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "newMagg", sBuilder.ToString());
                LoadCombo();
                LoadGridView(0);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.CodiciDescrizioni.CmdCopyCategoriesClick.errore::", ex);;
                throw;
            }
        }

        private void LoadCombo()
        {
            try
            {
                CategorieAtecoEnte categorie = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo };
                rddlCategorie.DataSource = categorie.LoadAll();
                rddlCategorie.DataValueField = "CodiceCategoria";
                rddlCategorie.DataTextField = "Definizione";
                rddlCategorie.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.CodiciDescrizioni.LoadCombo.errore::", ex);;
                throw;
            }
        }

        private void LoadGridView(int page)
        {
            try
            {
                if (optCategorie.Checked == true)
                {
                    CategorieAtecoEnte categorie = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, CodiceCategoria = rddlCategorie.SelectedValue };
                    rgvCategorie.DataSource = categorie.LoadAll();
                    rgvCategorie.PageIndex = page;
                    rgvCategorie.DataBind();
                }
                else { 
                    string MyType="";
                    if (optMaggiorazioni.Checked == true)
                        MyType = "M";
                    else if (optTipoConferimenti.Checked == true)
                        MyType = "C";
                    else if (optRiduzioni.Checked == true)
                        MyType = "R";
                    else
                        MyType = "D";
                    RidEseEnte RidEse = new RidEseEnte { Ente = Ente, FromVariabile = FromVariabile, myType = MyType };
                    rgvRidEse.DataSource = RidEse.LoadAll();
                    rgvRidEse.PageIndex = page;
                    rgvRidEse.DataBind();
                }
                ManageBottoniera(false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.CodiciDescrizioni.LoadGridView.errore::", ex);;
                throw;
            }
        }

        private void ManageBottoniera(bool showEdit)
        {
            if(showEdit)
            { ShowDiv("divEdit"); HideDiv("divSelect"); ShowDiv("editCodDescr"); HideDiv("searchCategoria"); HideDiv("searchRidEse"); }
            else
            { 
                ShowDiv("divSelect"); HideDiv("divEdit"); HideDiv("editCodDescr");
                if (optCategorie.Checked == true)
                {
                    ShowDiv("searchCategoria");HideDiv("searchRidEse");
                }
                else 
                {
                    ShowDiv("searchRidEse"); HideDiv("searchCategoria");
                }
            }
            lblMessage.Visible = lblEditMessage.Visible = false;
        }

        protected void CmdSearchClick(object sender, EventArgs e)
        {
            LoadGridView(0);
        }

        protected void CmdInsertClick(object sender, EventArgs e)
        {
            if (optMaggiorazioni.Checked == false)
            {
                hfIdCategoriaAteco.Value = "0";
                txtCodice.Text = txtDescrizione.Text = null;
                txtCodice.Enabled = true;
                chkDomestica.Checked = false;
                if (optCategorie.Checked == false)
                {
                    lblDomestica.Visible = false; chkDomestica.Visible = false;
                }
                ManageBottoniera(true);
            }
            else {
                ManageBottoniera(false);
                string sScript;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript = "<script language='javascript'>";
                sScript += "alert('Funzionalità al momento non disponibile');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "newMagg", sBuilder.ToString());
            }
        }

        protected void CmdBackClick(object sender, EventArgs e)
        {
            ManageBottoniera(false);
        }

        protected void CmdSaveClick(object sender, EventArgs e)
        {
            if (optCategorie.Checked == true)
            {
                CategorieAtecoEnte ateco;
                int idCategoriaAteco = 0;
                int.TryParse(hfIdCategoriaAteco.Value, out idCategoriaAteco);
                if (idCategoriaAteco > 0)
                {
                    ateco = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, IdCategoriaAteco = idCategoriaAteco };
                    ateco.Load();
                }
                else
                    ateco = new CategorieAtecoEnte { FKIdTypeAteco = 3, Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo };

                ateco.CodiceCategoria = txtCodice.Text;
                ateco.Definizione = txtDescrizione.Text;
                ateco.Domestica = chkDomestica.Checked;
                if (ateco.Save())
                {
                    //ManageBottoniera(false);
                    LoadCombo();
                    LoadGridView(rgvCategorie.PageIndex);
                }
                else
                {
                    ManageBottoniera(true);
                    lblEditMessage.Visible = true;
                    lblEditMessage.Text = string.Format("Impossibile salvare la Categoria: {0}.<br/>Controlla che non sia già presente", txtCodice.Text);
                }
            }
            else
            {
                RidEseEnte ridese;
                string CodiceRidEse = txtCodice.Text;
                string MyType, MyTypeDescr;

                if (optTipoConferimenti.Checked == true)
                {
                    MyType = "C";
                    MyTypeDescr = "Tipo Conferimento";
                    if (txtCodice.Text.Length > 5)
                    {
                        ManageBottoniera(true);
                        lblEditMessage.Visible = true;
                        lblEditMessage.Text = string.Format("Impossibile salvare " + MyTypeDescr + ": {0}.<br/>Il Codice deve essere al massimo lungo 5chr.", txtCodice.Text);
                        return;
                    }
                }
                else if (optRiduzioni.Checked == true)
                {
                    MyType = "R";
                    MyTypeDescr = "Riduzione";
                }
                else {
                    MyType = "D";
                    MyTypeDescr = "Esenzione";
                }

                ridese = new RidEseEnte { myType = MyType, Ente = Ente, FromVariabile = FromVariabile };
                ridese.Codice = txtCodice.Text;
                ridese.Definizione = txtDescrizione.Text;
                if (ridese.Save())
                {
                    LoadGridView(rgvRidEse.PageIndex);
                }
                else
                {
                    ManageBottoniera(true);
                    lblEditMessage.Visible = true;
                    lblEditMessage.Text = string.Format("Impossibile salvare " + MyTypeDescr + ": {0}.<br/>Controlla che non sia già presente.", txtCodice.Text);
                }
            }
        }
        #region "Griglia Categorie"
        protected void RgvCategoriePageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex);
        }

        protected void RgvCategorieRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
            CategorieAtecoEnte ateco = new CategorieAtecoEnte { CodiceCategoria = args.Row.Cells[0].Text, Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo };
            if (!ateco.Load()) return;
            hfIdCategoriaAteco.Value = ateco.IdCategoriaAteco.ToString(CultureInfo.InvariantCulture);
            txtCodice.Text = ateco.CodiceCategoria;
            txtDescrizione.Text = ateco.Definizione;
            chkDomestica.Checked = ateco.Domestica;
            ManageBottoniera(true);
        }

        protected void RgvCategorieRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idAteco;
            int.TryParse(e.CommandArgument.ToString(), out idAteco);
            CategorieAtecoEnte categoriaAteco = new CategorieAtecoEnte { Ente = Ente, FromVariabile = FromVariabile, TypeTassazione = TariffeEnte.TypeTassazione_TariffeTributo, IdCategoriaAteco = idAteco };
            if (categoriaAteco.Load())
            {
                switch (e.CommandName)
                {
                    case "DeleteATECO":
                        if (categoriaAteco.Delete())
                            LoadGridView(0);
                        else
                        {
                            string sScript;
                            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                            sScript = "<script language='javascript'>";
                            sScript += "alert('" + string.Format("Impossibile eliminare la Categoria: {0}",
                                                                                        categoriaAteco.CodiceCategoria) + "');";
                            sScript += "</script>";
                            sBuilder.Append(sScript);
                            ClientScript.RegisterStartupScript(this.GetType(), "errIdCategoriaAteco", sBuilder.ToString());
                            ManageBottoniera(false);
                        }
                        break;
                    case "EditATECO":
                        hfIdCategoriaAteco.Value = categoriaAteco.IdCategoriaAteco.ToString(CultureInfo.InvariantCulture);
                        txtCodice.Text = categoriaAteco.CodiceCategoria;
                        txtDescrizione.Text = categoriaAteco.Definizione;
                        chkDomestica.Checked = categoriaAteco.Domestica;
                        ManageBottoniera(true);
                        break;
                    default:
                        ManageBottoniera(false);
                        break;
                }
            }
        }
        #endregion
        #region "Griglia Riduzioni/Esenzioni/Maggiorazioni"
        protected void RgvRidEseRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string MyType, MyTypeDescr;
            if (optMaggiorazioni.Checked == true)
            {
                ManageBottoniera(false);
                string sScript;
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                sScript = "<script language='javascript'>";
                sScript += "alert('Funzionalità al momento non disponibile');";
                sScript += "</script>";
                sBuilder.Append(sScript);
                ClientScript.RegisterStartupScript(this.GetType(), "grdMagg", sBuilder.ToString());
            }
            else
            {
                if (optTipoConferimenti.Checked == true)
                {
                    MyType = "C";
                    MyTypeDescr = "Tipo Conferimento";
                }
                else if (optRiduzioni.Checked == true)
                {
                    MyType = "R";
                    MyTypeDescr = "Riduzione";
                }
                else
                {
                    MyType = "D";
                    MyTypeDescr = "Esenzione";
                }

                string CodiceRidEse = e.CommandArgument.ToString();
                RidEseEnte myRidEse = new RidEseEnte { myType = MyType, Ente = Ente, FromVariabile = FromVariabile, Codice = CodiceRidEse };
                if (myRidEse.Load())
                {
                    switch (e.CommandName)
                    {
                        case "DeleteRidEse":
                            if (myRidEse.Delete())
                                LoadGridView(0);
                            else
                            {
                                ManageBottoniera(false);
                                string sScript;
                                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                                sScript = "<script language='javascript'>";
                                sScript += "alert('" + string.Format("Impossibile eliminare la " + MyTypeDescr + " RidEse: {0}",
                                                                                            myRidEse.Codice) + "');";
                                sScript += "</script>";
                                sBuilder.Append(sScript);
                                ClientScript.RegisterStartupScript(this.GetType(), "errRidEse", sBuilder.ToString());
                            }
                            break;
                        case "EditRidEse":
                            txtCodice.Text = myRidEse.Codice;
                            txtCodice.Enabled = false;
                            txtDescrizione.Text = myRidEse.Definizione;
                            lblDomestica.Visible = false; chkDomestica.Visible = false;
                            ManageBottoniera(true);
                            break;
                        default:
                            ManageBottoniera(false);
                            break;
                    }
                }
            }
        }

        protected void RgvRidEsePageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadGridView(e.NewPageIndex);
        }

        protected void RgvRidEseRowClicked(object sender, Ribes.OPENgov.WebControls.GridViewRowClickedEventArgs args)
        {
            string MyType;
            if (optMaggiorazioni.Checked == false)
            {
                if (optTipoConferimenti.Checked == true)
                                    MyType = "C";
                else if (optRiduzioni.Checked == true)
                    MyType = "R";
                else
                    MyType = "D";

                RidEseEnte myRidEse = new RidEseEnte { myType = MyType, Ente = Ente, FromVariabile = FromVariabile, Codice = args.Row.Cells[0].Text };
                if (!myRidEse.Load()) return;
                txtCodice.Text = myRidEse.Codice;
                txtDescrizione.Text = myRidEse.Definizione;
                ManageBottoniera(true);
            }
        }
        #endregion
    }
}