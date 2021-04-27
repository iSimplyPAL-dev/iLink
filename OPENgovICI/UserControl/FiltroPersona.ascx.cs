namespace DichiarazioniICI.UserControl
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using DichiarazioniICI.Database;
    using Business;
    using log4net;
    /// <summary>
    ///	UesrControl per la ricerca delle dichiarazioni per Soggetto.
    /// </summary>
    public class FiltroPersona : System.Web.UI.UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FiltroPersona));
        protected System.Web.UI.WebControls.TextBox txtPartitaIva;
        protected System.Web.UI.WebControls.TextBox txtCodiceFiscale;
        protected System.Web.UI.WebControls.TextBox txtNome;
        protected System.Web.UI.WebControls.Button btnTrova;
        protected System.Web.UI.WebControls.TextBox txtCognome;
        protected string IdEnte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Gestione PaginaPrecedente
        {
            get
            {
                return ((Gestione)Page);
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            //*** 201511 - Funzioni Sovracomunali ***
            IdEnte = PaginaPrecedente.ddlEnti.SelectedValue.ToString();
            //*** ***
            try
            {
                if (!IsPostBack)
                {
                    if (ConstWrapper.Parametri != null)
                    {
                        if (ConstWrapper.Parametri["TipoRicerca"] != null)
                        {
                            if (ConstWrapper.Parametri["TipoRicerca"].ToString() == "Persona")
                            {
                                PaginaPrecedente.GrdImmobili.Visible = false;
                                txtCognome.Text = ConstWrapper.Parametri["Cognome"] == null ? String.Empty : ConstWrapper.Parametri["Cognome"].ToString();
                                txtNome.Text = ConstWrapper.Parametri["Nome"] == null ? String.Empty : ConstWrapper.Parametri["Nome"].ToString();
                                txtCodiceFiscale.Text = ConstWrapper.Parametri["CodiceFiscale"] == null ? String.Empty : ConstWrapper.Parametri["CodiceFiscale"].ToString();
                                txtPartitaIva.Text = ConstWrapper.Parametri["PartitaIVA"] == null ? String.Empty : ConstWrapper.Parametri["PartitaIVA"].ToString();
                                this.btnTrovaContribuente_Click(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroPersona.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTrova.Click += new System.EventHandler(this.btnTrovaContribuente_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
        /// <summary>
        /// funzione che esegue la ricerca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        private void btnTrovaContribuente_Click(object sender, System.EventArgs e)
        {
            string Dal, Al;
            try
            {
                Dal = Al = DateTime.MaxValue.ToString();
                ConstWrapper.Parametri = new System.Collections.Hashtable();
                ConstWrapper.Parametri["Cognome"] = txtCognome.Text;
                ConstWrapper.Parametri["Nome"] = txtNome.Text;
                ConstWrapper.Parametri["CodiceFiscale"] = txtCodiceFiscale.Text;
                ConstWrapper.Parametri["PartitaIVA"] = txtPartitaIva.Text;
                ConstWrapper.Parametri["TipoRicerca"] = "Persona";

                if (PaginaPrecedente.TxtDal.Text != "")
                    Dal = PaginaPrecedente.TxtDal.Text;
                if (PaginaPrecedente.TxtAl.Text != "")
                    Al = PaginaPrecedente.TxtAl.Text;
                DataTable Tabella = new ContribuentiTuttiView().ListContribuenti(int.Parse(PaginaPrecedente.ddlProv.SelectedValue), Dal, Al, txtCognome.Text, txtNome.Text, txtCodiceFiscale.Text, txtPartitaIva.Text, IdEnte, int.Parse(PaginaPrecedente.ddlBonificato.SelectedValue));
                Session["TABELLA_RICERCA"] = Tabella;
                if (Tabella.Rows.Count > 0)
                {
                    Tabella.DefaultView.Sort = "Cognome";

                    if (Business.ConstWrapper.CodiceEnte == "")
                        PaginaPrecedente.GrdContribuenti.Columns[0].Visible = true;
                    else
                        PaginaPrecedente.GrdContribuenti.Columns[0].Visible = false;
                    PaginaPrecedente.GrdImmobili.Visible = false;
                    PaginaPrecedente.GrdContribuenti.DataSource = Session["TABELLA_RICERCA"];
                    PaginaPrecedente.GrdContribuenti.Visible = true;
                    PaginaPrecedente.GrdContribuenti.DataBind();
                    PaginaPrecedente.lblRisultati.Text = "Risultati della Ricerca";
                    PaginaPrecedente.lblNRecord.Text = "N. Posizioni " + Tabella.Rows.Count.ToString();
                    //per la stampa
                    Tabella = new ContribuentiTuttiView().ListContribuentiStampa(IdEnte, "", int.Parse(PaginaPrecedente.ddlProv.SelectedValue), Dal, Al
                        , txtCognome.Text, txtNome.Text, txtCodiceFiscale.Text, txtPartitaIva.Text
                        , "", "", "", "", "", -1, "", "", "", "-1", "-1", 0, 0, -1, 0, 0, 0, 0, 0, 0);
                    Session["TABELLA_STAMPA"] = Tabella;
                }
                else
                {
                    PaginaPrecedente.GrdImmobili.Visible = false;
                    PaginaPrecedente.lblRisultati.Text = "La ricerca non ha prodotto risultati";
                    PaginaPrecedente.lblNRecord.Text = "";
                    PaginaPrecedente.GrdContribuenti.Visible = false;
                }
           }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.FiltroPersona.btnTrovaContribuente_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
    }
}
