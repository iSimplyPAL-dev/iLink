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
using IRemInterfaceOSAP;
using DTO;
using DAO;
using log4net;
using System.Collections.Generic;

namespace OPENgovTOCO.SituazioneContribuente
{
    /// <summary>
    /// Pagina per la consultazione Avvisi.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Analisi eventi</em>
    /// </revision>
    /// </revisionHistory>
    public partial class SituazioneAvvisi : BasePage
    {
        private Wuc.WucDatiContribuente wucContribuente;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SituazioneAvvisi));
        //*** 201802 - Sgravi ***
        private static Cartella oCartella;
        private static double Pagato;
        //*** ***
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerText += " - Avvisi - Consultazione";

                if (!Page.IsPostBack)
                {
                    //*** 201802 - Sgravi ***
                    hfVisualArticolo.Value = "0";
                    sScript = "document.getElementById('divVisual').style.display='';";
                    sScript += "document.getElementById('divEdit').style.display='none';";
                    sScript += "document.getElementById('btnSalva').style.display='none';";
                    RegisterScript(sScript, this.GetType());
                    //*** ***
                    BindData(int.Parse(Request["IdCartella"]));
                }
                //*** 201504 - Nuova Gestione anagrafica con form unico ***
                if (DichiarazioneSession.HasPlainAnag)
                    sScript = "document.getElementById('TRSpecAnag').style.display='none';";
                else
                    sScript = "document.getElementById('TRPlainAnag').style.display='none';";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                if (hfSgravioInCorso.Value == "1")
                    LblSgravioInCorso.Text = "* Sgravio in corso";
                else
                    LblSgravioInCorso.Text = "";
                if (hfSgravato.Value == "0")
                    sScript = "document.getElementById('btnDelSgravio').style.display='none';";
                else
                    sScript = "document.getElementById('btnDelSgravio').style.display='';";
                RegisterScript(sScript, this.GetType());
            }
        }

        private void BindData(int IdCartella)
        {
            double DaPagare = 0.0;
            double Residuo = 0.0;
            Pagato = default(double);

            oCartella = MetodiCartella.GetCartella(DichiarazioneSession.StringConnection, IdCartella, DichiarazioneSession.IdEnte, false);
            Ruolo[] listRuoli = oCartella.Ruoli;
            Pagamento[] listPagamenti = MetodiPagamento.GetPagamentiCartella(IdCartella, DichiarazioneSession.IdEnte);

            //*** 201504 - Nuova Gestione anagrafica con form unico ***
            hdIdContribuente.Value = oCartella.CodContribuente.ToString();
            hfSgravato.Value = (!oCartella.IsSgravio) ? "0" : "1";
            try
            {
                if (DichiarazioneSession.HasPlainAnag)
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + oCartella.CodContribuente.ToString() + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString());
                else
                {
                    wucContribuente = (Wuc.WucDatiContribuente)(this.FindControl("wucDatiContribuente"));
                    wucContribuente.LoadAnagrafica(oCartella.CodContribuente);
                }
                // Info cartella
                if (oCartella != null)
                {
                    lblAvvisoNumero.Text = oCartella.CodiceCartella;
                    lblAvvisoAnno.Text = oCartella.Anno.ToString();
                    lblAvvisoTotale.Text = string.Format("{0:0.00}", oCartella.ImportoTotale);
                    lblAvvisoArrotondamento.Text = string.Format("{0:0.00}", oCartella.ImportoArrotondamento);
                    lblAvvisoDovuto.Text = string.Format("{0:0.00}", oCartella.ImportoCarico);

                    DaPagare = oCartella.ImportoCarico;
                    hfIdFlusso.Value = oCartella.IdFlussoRuolo.ToString();
                }
                else
                {
                    hfIdFlusso.Value = "-1";
                }

                // Info pagamenti
                if (listPagamenti != null)
                {
                    Pagato = 0.0;
                    foreach (Pagamento p in listPagamenti)
                        Pagato += p.ImportoPagato;

                    GrdDettaglioPagamenti.DataSource = listPagamenti;
                    GrdDettaglioPagamenti.DataBind();
                }

                Residuo = (DaPagare - Pagato);
                lblTotaleDaPagare.Text = string.Format("€ {0:0.00}", DaPagare);
                lblTotalePagato.Text = string.Format("€ {0:0.00}", Pagato);
                lblTotaleResiduo.Text = string.Format("€ {0:0.00}", Residuo);

                if (Residuo <= 0.0)
                    lblTotaleResiduo.ForeColor = System.Drawing.Color.LightGreen;
                else
                    lblTotaleResiduo.ForeColor = System.Drawing.Color.Red;

                // Info ruoli
                if (listRuoli != null)
                {
                    GrdDettaglioRuoli.DataSource = listRuoli;
                    GrdDettaglioRuoli.DataBind();
                }

                //rate
                LoadRate();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.BindData.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        private void DataBindGridRate(RataExt[] rate, int StartIndex, bool bRebind)
        {
            try
            {
                if (rate != null && rate.Length > 0)
                {
                    GrdRate.DataSource = rate;
                    if (bRebind)
                    {
                        GrdRate.DataBind();
                    }
                    GrdRate.Visible = true;
                    //LblResultPagamenti.Visible = false;
                }
                else
                {
                    //LblResultPagamenti.Visible = true;
                    GrdRate.Visible = false;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.DataBindGridRate.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        #region Codice generato da Progettazione Web Form
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: questa chiamata è richiesta da Progettazione Web Form ASP.NET.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
        #region Bottoni
        protected void btnClose_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                //*** 201802 - Sgravi ***
                string sScript = "";
                int myInt = default(int);
                int.TryParse(hfVisualArticolo.Value, out myInt);
                sScript = "document.getElementById('divVisual').style.display='';";
                sScript += "document.getElementById('divEdit').style.display='none';";
                if (myInt == 1)
                {
                    sScript += "document.getElementById('btnSalva').style.display='none';";
                    RegisterScript(sScript, this.GetType());
                    hfVisualArticolo.Value = "0";
                }
                //*** ***
                else {
                    int.TryParse(hfSgravioInCorso.Value, out myInt);
                    if (myInt == 1)
                    {
                        sScript += "GestAlert('a', 'info', '', '', 'Terminare la procedura di sgravio prima di uscire dalla pagina!');";
                        sScript += "document.getElementById('btnSalva').style.display='none';";
                    }
                    else {
                        sScript += "document.getElementById('btnSalva').style.display='none';";
                        sScript += "document.getElementById('btnSgravio').style.display='none';";
                        sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                        sScript += "document.getElementById('btnStampaDocumenti').style.display='none';";
                        if (Request.Params["Provenienza"] != null)
                        {
                            //*** 20150703 - INTERROGAZIONE GENERALE ***
                            if (Request.Params["Provenienza"] == "INTERGEN")
                            {
                                sScript += "parent.Visualizza.location.href='../../Interrogazioni/DichEmesso.aspx?Ente=" + DichiarazioneSession.IdEnte + "';";
                                sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';";
                                sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                                sScript += "parent.Nascosto.location.href='../../aspSvuota.aspx';";
                            }
                            else
                            {
                                sScript += "parent.Visualizza.location.href='" + OSAPPages.SituazioneAvvisiSearch + "?NewSearch=false';";
                                sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';";
                                sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                                sScript += "parent.Nascosto.location.href='../../aspSvuota.aspx';";
                            }
                        }
                        else {
                            sScript += "parent.Visualizza.location.href='" + OSAPPages.SituazioneAvvisiSearch + "?NewSearch=false';";
                            sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';";
                            sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                            sScript += "parent.Nascosto.location.href='../../aspSvuota.aspx';";
                        }
                    }
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.bntClose_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** 201802 - Sgravi ***
        /// <summary>
        /// Pulsante per la gestione degli sgravi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnSgravio_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                int InCorso = default(int);
                int.TryParse(hfSgravioInCorso.Value, out InCorso);
                if (InCorso == 0)
                {
                    hfSgravioInCorso.Value = "1";
                    LblSgravioInCorso.Text = "* Sgravio in corso";
                    sScript = "GestAlert('a', 'info', '', '', 'Posizione del Contribuente Bloccata. Procedere con la procedura di Sgravio!');";
                    sScript += "document.getElementById('btnSalva').style.display='none';";
                    sScript += "document.getElementById('btnSgravio').style.display='';";
                    sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                    sScript += "document.getElementById('btnStampaDocumenti').style.display='none';";
                }
                else
                {
                    System.Data.SqlClient.SqlCommand cmdMyCommand = new System.Data.SqlClient.SqlCommand();
                    cmdMyCommand.CommandType = CommandType.StoredProcedure;
                    cmdMyCommand.Connection = new System.Data.SqlClient.SqlConnection(DichiarazioneSession.StringConnection);
                    cmdMyCommand.CommandTimeout = 0;
                    if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                    {
                        cmdMyCommand.Connection.Open();
                    }
                    MetodiCartella.InsertCartella(ref oCartella, ref cmdMyCommand, true, 1, DichiarazioneSession.sOperatore);
                    MetodiRata.CalcolaRate(3, oCartella.IdFlussoRuolo, oCartella.IdCartella, 0, 0, ref cmdMyCommand);
                    MetodiRata.CalcolaRate(4, oCartella.IdFlussoRuolo, oCartella.IdCartella, 0, 0, ref cmdMyCommand);
                    BindData(oCartella.IdCartella);
                    hfSgravato.Value = "1";
                    new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, DichiarazioneSession.sOperatore, new  Utility.Costanti.LogEventArgument().Sgravio, "btnSgravio", Utility.Costanti.AZIONE_NEW.ToString(), DichiarazioneSession.CodTributo(""), DichiarazioneSession.IdEnte, oCartella.IdCartella);
  sScript = "GestAlert('a', 'success', '', '', 'Procedura di Sgravio terminata correttamente!');";
                    sScript += "document.getElementById('btnSalva').style.display='none';";
                    sScript += "document.getElementById('btnSgravio').style.display='none';";
                    sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                    sScript += "document.getElementById('btnStampaDocumenti').style.display='';";
                    hfSgravioInCorso.Value = "0";
                    LblSgravioInCorso.Text = "";
                }
                sScript += "document.getElementById('divVisual').style.display='';";
                sScript += "document.getElementById('divEdit').style.display='none';";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnSgravio_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void btnSgravio_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        string sScript = string.Empty;
        //        int InCorso = default(int);
        //        int.TryParse(hfSgravioInCorso.Value, out InCorso);
        //        if (InCorso == 0)
        //        {
        //            hfSgravioInCorso.Value = "1";
        //            LblSgravioInCorso.Text = "* Sgravio in corso";
        //            sScript = "GestAlert('a', 'info', '', '', 'Posizione del Contribuente Bloccata. Procedere con la procedura di Sgravio!');";
        //            sScript += "document.getElementById('btnSalva').style.display='none';";
        //            sScript += "document.getElementById('btnSgravio').style.display='';";
        //            sScript += "document.getElementById('btnDelSgravio').style.display='none';";
        //            sScript += "document.getElementById('btnStampaDocumenti').style.display='none';";
        //        }
        //        else
        //        {
        //            System.Data.SqlClient.SqlCommand cmdMyCommand = new System.Data.SqlClient.SqlCommand();
        //            cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //            cmdMyCommand.Connection = new System.Data.SqlClient.SqlConnection(DichiarazioneSession.StringConnection);
        //            cmdMyCommand.CommandTimeout = 0;
        //            if (cmdMyCommand.Connection.State == ConnectionState.Closed)
        //            {
        //                cmdMyCommand.Connection.Open();
        //            }
        //            MetodiCartella.InsertCartella(ref oCartella, ref cmdMyCommand, true, 1, DichiarazioneSession.sOperatore);
        //            MetodiRata.CalcolaRate(3, oCartella.IdFlussoRuolo, oCartella.IdCartella, 0, 0, ref cmdMyCommand);
        //            MetodiRata.CalcolaRate(4, oCartella.IdFlussoRuolo, oCartella.IdCartella, 0, 0, ref cmdMyCommand);
        //            BindData(oCartella.IdCartella);
        //            hfSgravato.Value = "1";
        //            sScript = "GestAlert('a', 'success', '', '', 'Procedura di Sgravio terminata correttamente!');";
        //            sScript += "document.getElementById('btnSalva').style.display='none';";
        //            sScript += "document.getElementById('btnSgravio').style.display='none';";
        //            sScript += "document.getElementById('btnDelSgravio').style.display='none';";
        //            sScript += "document.getElementById('btnStampaDocumenti').style.display='';";
        //            hfSgravioInCorso.Value = "0";
        //            LblSgravioInCorso.Text = "";
        //        }
        //        sScript += "document.getElementById('divVisual').style.display='';";
        //        sScript += "document.getElementById('divEdit').style.display='none';";
        //        RegisterScript(sScript, this.GetType());
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnSgravio_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //}
        protected void btnSalva_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                Articolo myArt = new Articolo();
                string myErr = string.Empty;
                double TotAvviso = default(double);
                int Anno = 0;
                int.TryParse(lblAvvisoAnno.Text, out Anno);

                if (!CheckDatiArticolo(out myArt, out myErr))
                {
                    RegisterScript("GestAlert('a', 'danger', '', '', '" + myErr + "');", this.GetType());
                }
                else {
                    Categorie[] ListCategorie = MetodiCategorie.GetCategorieForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                    TipologieOccupazioni[] ListTipiOcc = MetodiTipologieOccupazioni.GetTipologieOccupazioniForMotore(DichiarazioneSession.StringConnection, Anno, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                    Agevolazione[] ListAgevolazioni = MetodiAgevolazione.GetAgevolazioniForMotore(DichiarazioneSession.StringConnection, Anno.ToString(), DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                    Tariffe[] ListTariffe = MetodiTariffe.GetTariffeForMotore(Anno, DichiarazioneSession.CodTributo(""));
                    IRemotingInterfaceOSAP motore = (IRemotingInterfaceOSAP)Activator.GetObject(typeof(IRemotingInterfaceOSAP), DichiarazioneSession.UrlMotoreOSAP);
                    CalcoloResult newCalcolato = motore.CalcolaOSAP(Ruolo.E_TIPO.ORDINARIO, myArt, ListCategorie, ListTipiOcc, ListAgevolazioni, ListTariffe, new CalcoloResult());
                    if (newCalcolato.Result == E_CALCOLORESULT.OK)
                    {
                        foreach (Ruolo myItem in oCartella.Ruoli)
                        {
                            if (myItem.ArticoloTOCO.IdArticolo.ToString() == hfIdSgravio.Value)
                            {
                                myItem.Importo = newCalcolato.ImportoCalcolato;
                                myItem.ImportoLordo = newCalcolato.ImportoLordo;
                                Tariffe myTariffa = new Tariffe();
                                myTariffa.Valore = newCalcolato.TariffaApplicata;
                                myItem.Tariffa = myTariffa;
                                myItem.ArticoloTOCO = myArt;
                            }
                            TotAvviso += myItem.Importo;
                        }
                        oCartella.ImportoTotale = TotAvviso;
                        oCartella.ImportoCarico = Math.Round(TotAvviso, 0);
                        oCartella.ImportoArrotondamento = Math.Round(TotAvviso, 0) - TotAvviso;
                    }
                    else
                    {
                        switch (newCalcolato.Result)
                        {
                            case (E_CALCOLORESULT.ERRORECALCOLO):
                                RegisterScript("GestAlert('a', 'danger', '', '', 'Si è verificato un errore di calcolo');", this.GetType());
                                break;
                            case (E_CALCOLORESULT.NOCATEGORIA):
                                RegisterScript("GestAlert('a', 'warning', '', '', 'Non è stata trovata la categoria corretta');", this.GetType());
                                break;
                            case (E_CALCOLORESULT.NOTARIFFA):
                                RegisterScript("GestAlert('a', 'warning', '', '', 'Non è stata trovata la tariffa corretta');", this.GetType());
                                break;
                            case (E_CALCOLORESULT.NOTIPOLOGIAOCCUPAZIONE):
                                RegisterScript("GestAlert('a', 'warning', '', '', 'Non è stata trovata la tipologia corretta');", this.GetType());
                                break;
                        }
                    }
                    LoadArticolo("-1");
                    lblAvvisoTotale.Text = string.Format("{0:0.00}", oCartella.ImportoTotale);
                    lblAvvisoArrotondamento.Text = string.Format("{0:0.00}", oCartella.ImportoArrotondamento);
                    lblAvvisoDovuto.Text = string.Format("{0:0.00}", oCartella.ImportoCarico);
                    lblTotaleDaPagare.Text = string.Format("€ {0:0.00}", oCartella.ImportoCarico);
                    lblTotalePagato.Text = string.Format("€ {0:0.00}", Pagato);
                    lblTotaleResiduo.Text = string.Format("€ {0:0.00}", (oCartella.ImportoCarico - Pagato));

                    if ((oCartella.ImportoCarico - Pagato) <= 0.0)
                        lblTotaleResiduo.ForeColor = System.Drawing.Color.LightGreen;
                    else
                        lblTotaleResiduo.ForeColor = System.Drawing.Color.Red;

                    GrdDettaglioRuoli.DataSource = oCartella.Ruoli;
                    GrdDettaglioRuoli.DataBind();
                    hfVisualArticolo.Value = "0";
                    string sScript = "document.getElementById('divVisual').style.display='';";
                    sScript += "document.getElementById('divEdit').style.display='none';";
                    sScript += "document.getElementById('btnSalva').style.display='none';";
                    sScript += "document.getElementById('btnSgravio').style.display='';";
                    sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                    sScript += "document.getElementById('btnStampaDocumenti').style.display='none';";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnSalva_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// Pulsante per la cancellazione di un avviso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        protected void btnDeleteSgravio_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                System.Data.SqlClient.SqlCommand cmdMyCommand = new System.Data.SqlClient.SqlCommand();
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new System.Data.SqlClient.SqlConnection(DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
                MetodiCartella.UndoSgravio(ref oCartella, ref cmdMyCommand, DichiarazioneSession.sOperatore);
                BindData(oCartella.IdCartella);
                new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, DichiarazioneSession.sOperatore, new  Utility.Costanti.LogEventArgument().Sgravio, "btnDeleteSgravio", Utility.Costanti.AZIONE_DELETE.ToString(), DichiarazioneSession.CodTributo(""), DichiarazioneSession.IdEnte, oCartella.IdCartella);
                sScript = "GestAlert('a', 'success', '', '', 'Annullo di Sgravio terminato correttamente!');";
                sScript += "document.getElementById('btnSalva').style.display='none';";
                sScript += "document.getElementById('btnSgravio').style.display='';";
                sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                sScript += "document.getElementById('btnStampaDocumenti').style.display='';";
                sScript += "document.getElementById('divVisual').style.display='';";
                sScript += "document.getElementById('divEdit').style.display='none';";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnDeleteSgravio_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //protected void btnDeleteSgravio_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        string sScript = string.Empty;
        //        System.Data.SqlClient.SqlCommand cmdMyCommand = new System.Data.SqlClient.SqlCommand();
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.Connection = new System.Data.SqlClient.SqlConnection(DichiarazioneSession.StringConnection);
        //        cmdMyCommand.CommandTimeout = 0;
        //        if (cmdMyCommand.Connection.State == ConnectionState.Closed)
        //        {
        //            cmdMyCommand.Connection.Open();
        //        }
        //        MetodiCartella.UndoSgravio(ref oCartella, ref cmdMyCommand, DichiarazioneSession.sOperatore);
        //        BindData(oCartella.IdCartella);
        //        sScript = "GestAlert('a', 'success', '', '', 'Annullo di Sgravio terminato correttamente!');";
        //        sScript += "document.getElementById('btnSalva').style.display='none';";
        //        sScript += "document.getElementById('btnSgravio').style.display='';";
        //        sScript += "document.getElementById('btnDelSgravio').style.display='none';";
        //        sScript += "document.getElementById('btnStampaDocumenti').style.display='';";
        //        sScript += "document.getElementById('divVisual').style.display='';";
        //        sScript += "document.getElementById('divEdit').style.display='none';";
        //        RegisterScript(sScript, this.GetType());
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnDeleteSgravio_Click.errore: ", Err);
        //        Response.Redirect("../../PaginaErrore.aspx");
        //    }
        //}
        //*** ***
        protected void btnStampaDocumenti_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
                string sScript = string.Empty;
            try
            {
                Generali.classi.Documenti FncDoc = new Generali.classi.Documenti();
                string sTipoElab = "PROVA";
                int nReturn;
                int nMaxDocPerFile;
                bool bElaboraBollettini = false;
                bool bCreaPDF = false;
                string TipoBollettino = "896";
                string ImpostazioneBollettini = string.Empty;

                bElaboraBollettini = true;
                ImpostazioneBollettini = "BOLLETTINISTANDARD";
                TipoBollettino = "451";
                nMaxDocPerFile = 1;
                try
                {
                    Cartella[] aCartelle = new Cartella[1];
                    aCartelle[0] = oCartella;
                    /**** 201810 - Calcolo puntuale ****/
                    nReturn = FncDoc.ElaboraDocumenti(DichiarazioneSession.DBType, DichiarazioneSession.CodTributo(""), aCartelle, int.Parse(lblAvvisoAnno.Text), DichiarazioneSession.IdEnte, int.Parse(hfIdFlusso.Value), DichiarazioneSession.StringConnection, DichiarazioneSession.StringConnectionOPENgov, DichiarazioneSession.StringConnectionAnagrafica, DichiarazioneSession.PathStampe, DichiarazioneSession.PathVirtualStampe, nMaxDocPerFile, sTipoElab, ImpostazioneBollettini, TipoBollettino, bElaboraBollettini, bCreaPDF, false, false,string.Empty);
                    if (nReturn == 1)
                    {
                        RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] docs = FncDoc.GetDocumentiElaborati(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo("").ToString(), int.Parse(hfIdFlusso.Value));
                        if (docs.Length > 0)
                        {
                        Session["StampaPuntuale"] = docs[0];
                            /*foreach (RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL url in docs)
                                sScript = "window.open('" + url.URLComplessivo.Url + "','_blank')";*/
                            sScript = "document.getElementById('loadStampa').src='../Elaborazioni/Documenti/ViewDocElaborati.aspx';";
                        }
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnStampaDocumenti_Click.errore: ", Err);
                    Response.Redirect("../../PaginaErrore.aspx");
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.btnStampaDocumenti_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            finally
            {
                sScript += "document.getElementById('divEdit').style.display='none';";
                sScript += "document.getElementById('btnSalva').style.display='none';";
                sScript += "document.getElementById('divVisual').style.display='none';";
                sScript += "document.getElementById('DivAttesa').style.display = 'none';";
                sScript += "document.getElementById('divStampa').style.display='';";
                RegisterScript(sScript, this.GetType());
            }
        }
        #endregion
        #region "Griglie"
        //*** 201802 - Sgravi ***
        protected void GrdRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                int InCorso = default(int);
                int.TryParse(hfSgravioInCorso.Value, out InCorso);
                if (e.CommandName == "RowOpen")
                {
                    LoadArticolo(IDRow);
                    hfVisualArticolo.Value = "1";
                    string sScript = "document.getElementById('divVisual').style.display='none';";
                    sScript += "document.getElementById('divEdit').style.display='';";
                    if (InCorso == 1)
                    {
                        EnabledEdit(true);
                        sScript += "document.getElementById('btnSalva').style.display='';";
                        sScript += "document.getElementById('btnSgravio').style.display='none';";
                        sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                        sScript += "document.getElementById('btnStampaDocumenti').style.display='none';";
                    }
                    else {
                        EnabledEdit(false);
                        sScript += "document.getElementById('btnSalva').style.display='none';";
                        sScript += "document.getElementById('btnSgravio').style.display='none';";
                        sScript += "document.getElementById('btnDelSgravio').style.display='none';";
                        sScript += "document.getElementById('btnStampaDocumenti').style.display='none';";
                    }
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.GrdRowCommand.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        //*** ***
        #endregion
        //*** 201802 - Sgravi ***
        private void LoadArticolo(string Id)
        {
            try
            {
                Articolo myItem = new Articolo();
                myItem = MetodiArticolo.GetArticolo(DichiarazioneSession.StringConnection, int.Parse(Id), DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                //carico le combo
                cmbCategoria.SelectedValue = "-1";
                cmbCategoria.DataSource = MetodiCategorie.GetCategorie(DichiarazioneSession.StringConnection, -1, true, DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""));
                cmbCategoria.DataValueField = "IdCategoria";
                cmbCategoria.DataTextField = "Descrizione";
                cmbCategoria.DataBind();
                cmbTipologiaOccupazione.SelectedValue = "-1";
                cmbTipologiaOccupazione.DataSource = MetodiTipologieOccupazioni.GetAllTipologieOccupazioni(DichiarazioneSession.IdEnte, DichiarazioneSession.CodTributo(""), true);
                cmbTipologiaOccupazione.DataValueField = "IdTipologiaOccupazione";
                cmbTipologiaOccupazione.DataTextField = "Descrizione";
                cmbTipologiaOccupazione.DataBind();
                cmbTipoConsistenza.SelectedValue = "-1";
                cmbTipoConsistenza.DataSource = MetodiTipoConsistenza.GetTipiConsistenza(DichiarazioneSession.StringConnection, true);
                cmbTipoConsistenza.DataValueField = "IdTipoConsistenza";
                cmbTipoConsistenza.DataTextField = "Descrizione";
                cmbTipoConsistenza.DataBind();
                cmbTipoDurata.SelectedValue = "-1";
                cmbTipoDurata.DataSource = MetodiDurata.GetDurate(DichiarazioneSession.StringConnection, true);
                cmbTipoDurata.DataValueField = "IdDurata";
                cmbTipoDurata.DataTextField = "Descrizione";
                cmbTipoDurata.DataBind();
                GrdAgevolazioni.DataSource = MetodiAgevolazione.GetAgevolazioni("", -1, DichiarazioneSession.IdEnte);
                GrdAgevolazioni.DataBind();
                //carico l'articolo
                hfIdSgravio.Value = myItem.IdArticolo.ToString();
                TxtVia.Text = myItem.SVia;
                if (myItem.Civico > 0)
                    TxtCivico.Text = myItem.Civico.ToString();
                txtConsistenza.Text = myItem.Consistenza.ToString();
                txtDurata.Text = myItem.DurataOccupazione.ToString();
                txtMaggiorazioni.Text = myItem.MaggiorazioneImporto.ToString();
                txtMaggiorazioniPerc.Text = myItem.MaggiorazionePerc.ToString();
                txtDetrazioni.Text = myItem.DetrazioneImporto.ToString();
                chkAttrazione.Checked = bool.Parse(myItem.Attrazione.ToString());
                if (myItem.TipologiaOccupazione != null)
                    cmbTipologiaOccupazione.SelectedValue = myItem.TipologiaOccupazione.IdTipologiaOccupazione.ToString();
                if (myItem.Categoria != null)
                    cmbCategoria.SelectedValue = myItem.Categoria.IdCategoria.ToString();
                if (myItem.TipoConsistenzaTOCO != null)
                    cmbTipoConsistenza.SelectedValue = myItem.TipoConsistenzaTOCO.IdTipoConsistenza.ToString();
                if (myItem.TipoDurata != null)
                    cmbTipoDurata.SelectedValue = myItem.TipoDurata.IdDurata.ToString();
                if (myItem.ListAgevolazioni != null)
                    foreach (Agevolazione myAg in myItem.ListAgevolazioni)
                    {
                        foreach (GridViewRow myRow in GrdAgevolazioni.Rows)
                        {
                            if (myAg.IdAgevolazione == int.Parse(((HiddenField)(myRow.FindControl("hfIdAgevolazione"))).Value))
                            {
                                ((CheckBox)(myRow.FindControl("ChkSelezionato"))).Checked = true;
                            }
                        }
                    }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.LoadArticolo.errore: ", Err);
                throw Err;
            }
        }
        private bool CheckDatiArticolo(out Articolo myItem, out string myErr)
        {
            myItem = new Articolo();
            myErr = string.Empty;
            try
            {
                List<Agevolazione> ListAgev = new List<Agevolazione>();
                int myInt = default(int);
                double myDouble = default(double);

                myItem.IdArticolo = int.Parse(hfIdSgravio.Value);
                if (TxtVia.Text == string.Empty)
                    myErr = "Inserire la Via";
                else
                    myItem.SVia = TxtVia.Text;
                int.TryParse(TxtCivico.Text, out myInt);
                myItem.Civico = myInt;
                if (cmbTipologiaOccupazione.SelectedValue == "-1")
                    myErr = "Inserire la tipologia occupazione";
                else
                {
                    int.TryParse(cmbTipologiaOccupazione.SelectedValue, out myInt);
                    TipologieOccupazioni myOcc = new TipologieOccupazioni();
                    myOcc.IdTipologiaOccupazione = myInt;
                    myItem.TipologiaOccupazione = myOcc;
                }
                if (cmbCategoria.SelectedValue == "-1")
                    myErr = "Inserire la categoria";
                else {
                    int.TryParse(cmbCategoria.SelectedValue, out myInt);
                    Categorie myCat = new Categorie();
                    myCat.IdCategoria = myInt;
                    myItem.Categoria = myCat;
                }
                if (txtConsistenza.Text == string.Empty)
                    myErr = "Inserire la consistenza";
                else {
                    double.TryParse(txtConsistenza.Text, out myDouble);
                    myItem.Consistenza = myDouble;
                }
                if (cmbTipoConsistenza.SelectedValue == "-1")
                    myErr = "Inserire la tipologia consistenza";
                else {
                    int.TryParse(cmbTipoConsistenza.SelectedValue, out myInt);
                    TipoConsistenza myTipiCons = new TipoConsistenza();
                    myTipiCons.IdTipoConsistenza = myInt;
                    myItem.TipoConsistenzaTOCO = myTipiCons;
                }
                if (txtDurata.Text == string.Empty)
                    myErr = "Inserire la durata";
                else {
                    int.TryParse(txtDurata.Text, out myInt);
                    myItem.DurataOccupazione = myInt;
                }
                if (cmbTipoDurata.SelectedValue == "-1")
                    myErr = "Inserire la tipologia durata";
                else {
                    int.TryParse(cmbTipoDurata.SelectedValue, out myInt);
                    Durata myDurata = new Durata();
                    myDurata.IdDurata = myInt;
                    myItem.TipoDurata = myDurata;
                    if (myInt == 3)
                        myItem.DurataOccupazione = 1;
                }
                double.TryParse(txtMaggiorazioni.Text, out myDouble);
                myItem.MaggiorazioneImporto = myDouble;
                double.TryParse(txtMaggiorazioniPerc.Text, out myDouble);
                myItem.MaggiorazionePerc = myDouble;
                double.TryParse(txtDetrazioni.Text, out myDouble);
                myItem.DetrazioneImporto = myDouble;
                myItem.Attrazione = bool.Parse(chkAttrazione.Checked.ToString());
                foreach (GridViewRow myRow in GrdAgevolazioni.Rows)
                {
                    if (((CheckBox)(myRow.FindControl("ChkSelezionato"))).Checked)
                    {
                        Agevolazione myAgevolazione = new Agevolazione();
                        myAgevolazione.IdAgevolazione = int.Parse(((HiddenField)(myRow.FindControl("hfIdAgevolazione"))).Value);
                        ListAgev.Add(myAgevolazione);
                    }
                }
                myItem.ListAgevolazioni = (Agevolazione[])ListAgev.ToArray();
                if (myErr != string.Empty)
                    return false;
                else
                    return true;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.LoadArticolo.errore: ", Err);
                return false;
            }
        }
        private void LoadRate()
        {
            try
            {
                RataExt[] rate = new RataExt[0];
                RataSearch SearchParams = new RataSearch();
                SearchParams.IdEnte = DichiarazioneSession.IdEnte;
                SearchParams.IdTributo = DichiarazioneSession.CodTributo("");
                SearchParams.IdContribuente = oCartella.CodContribuente;
                SearchParams.CodiceCartella = oCartella.CodiceCartella;
                SearchParams.IdCartella = oCartella.IdCartella;

                rate = MetodiRata.GetRate(SearchParams);

                DataBindGridRate(rate, 0, true);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.LoadRate.errore: ", Err);
                throw Err;
            }
        }
        private void EnabledEdit(bool IsEnabled)
        {
            try
            {
                TxtVia.Enabled = IsEnabled;
                TxtCivico.Enabled = IsEnabled;
                txtConsistenza.Enabled = IsEnabled;
                txtDurata.Enabled = IsEnabled;
                txtMaggiorazioni.Enabled = IsEnabled;
                txtMaggiorazioniPerc.Enabled = IsEnabled;
                txtDetrazioni.Enabled = IsEnabled;
                chkAttrazione.Enabled = IsEnabled;
                cmbTipologiaOccupazione.Enabled = IsEnabled;
                cmbCategoria.Enabled = IsEnabled;
                cmbTipoConsistenza.Enabled = IsEnabled;
                cmbTipoDurata.Enabled = IsEnabled;
                GrdAgevolazioni.Enabled = IsEnabled;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.SituazioneAvvisi.EnabledEdit.errore: ", Err);
                throw Err;
            }
        }
        //*** ***
    }
}
