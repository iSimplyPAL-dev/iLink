using System;
using System.Web.UI.WebControls;
using System.Configuration;
using FutureFog.U4N.General;
using OPENgovDL;
using Ribes.OPENgov.Utilities;
using log4net;

namespace OPENgov.Acquisizioni.GestMail
{/// <summary>
/// Pagina per la consultazione dell'invio delle mail.
/// Le possibili opzioni sono:
/// - Ricerca
/// </summary>
    public partial class GestEsitoInvio : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GestEsitoInvio));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BreadCrumb = "Invio Mail - Esiti di invio";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack) return;
                HideDiv("DivDettaglioLotto");
                LoadCombos();
                LoadSearch();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestEsitoInvio.Page_Load.errore::", ex); ;
                throw;
            }
        }
        #region "Bottoni"
        protected void CmdSearchClick(object sender, EventArgs e)
        {
            LoadSearch();
            HideDiv("DivDettaglioLotto");
        }
        #endregion
        #region Griglie
        protected void rgvLottiPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// Gestione degli eventi sulla griglia. 
        /// Con il comando ViewLotto viene visualizzato il dettaglio degli invii.
        /// Con il comando Resend vengono reinviate tutte le mail del lotto che avevano dato errore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        protected void rgvLottiRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idLotto;
            int.TryParse(e.CommandArgument.ToString(), out idLotto);
            hfIDLotto.Value = idLotto.ToString();
            switch (e.CommandName)
            {
                case "ViewLotto":
                    ShowDiv("DivDettaglioLotto");
                    LoadDettaglioEsiti(0);
                    break;
                case "Resend":

                    break;
                default:
                    break;
            }
        }
        //protected void rgvLottiRowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int idLotto;
        //    int.TryParse(e.CommandArgument.ToString(), out idLotto);
        //    hfIDLotto.Value = idLotto.ToString();
        //    switch (e.CommandName)
        //    {
        //        case "ViewLotto":
        //            ShowDiv("DivDettaglioLotto");
        //            LoadDettaglioEsiti(0);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        protected void rgvEsitiInvioPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadDettaglioEsiti(e.NewPageIndex);
        }
        #endregion

        private void LoadCombos()
        {
            try
            {
                InvioMail myGestMail = new InvioMail { Ente = Ente };
                rddlTributo.DataSource = myGestMail.LoadTributi(true);
                rddlTributo.DataValueField = "tributo";
                rddlTributo.DataTextField = "DescrTributo";
                rddlTributo.DataBind();

                rddlAnno.DataSource = myGestMail.LoadAnni(true);
                rddlAnno.DataValueField = "Anno";
                rddlAnno.DataTextField = "Anno";
                rddlAnno.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestEsitoInvio.LoadCombos.errore::", ex); ;
                throw;
            }
        }

        /// <summary>
        /// Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearch(int? page = 0)
        {
            string tributo = string.Empty;
            string anno = string.Empty;
            try
            {
                tributo = rddlTributo.SelectedValue;
                anno = rddlAnno.SelectedValue;
                InvioMail LottiToSend = new InvioMail { Ente = Ente, Tributo = tributo, Anno = anno };
                rgvLotti.DataSource = LottiToSend.LoadEsitiInvio();
                if (page.HasValue)
                    rgvLotti.PageIndex = page.Value;
                rgvLotti.DataBind();
                if (rgvLotti.Rows.Count > 0)
                    lblResultLotti.Visible = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestEsitoInvio.LoadSearch.errore::", ex); ;
                throw;
            }
        }

        private void LoadDettaglioEsiti(int? page = 0)
        {
            string tributo = string.Empty;
            string anno = string.Empty;
            int IdLotto = 0;
            int.TryParse(hfIDLotto.Value.ToString(), out IdLotto);
            try
            {
                InvioMailDestinatari LottiToSend = new InvioMailDestinatari { IdLotto = IdLotto };
                rgvEsitiInvio.DataSource = LottiToSend.LoadEsitiInvio();
                if (page.HasValue)
                    rgvEsitiInvio.PageIndex = page.Value;
                rgvEsitiInvio.DataBind();
                if (rgvEsitiInvio.Rows.Count > 0)
                    lblResultEsitiInvio.Visible = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestEsitoInvio.LoadDettaglioEsiti.errore::", ex); ;
                throw;
            }
        }
        /// <summary>
        /// Funzione per il reinvio delle mail di uno specifico lotto che avevano dato errore.
        /// </summary>
        /// <param name="IdLotto"></param>
        private void ResendMail(int IdLotto)
        {
            string sScript = string.Empty;
            string tributo = string.Empty;
            string anno = string.Empty;
            int.TryParse(hfIDLotto.Value.ToString(), out IdLotto);
            try
            {
                InvioMailDestinatari LottiToSend = new InvioMailDestinatari { IdLotto = IdLotto };
                if (LottiToSend.ResendMail())
                    sScript = "GestAlert('a', 'success', '', '', 'Invio mail rischedulato con successo!');";
                else
                    sScript = "GestAlert('a', 'warning', '', '', 'Errore nello rischedulamento delle mail!');";

                sScript = "<script language='javascript'>";
                sScript += "alert('Tariffe già presenti impossibile inserire come nuovo');";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ResendMailClick" + DateTime.Now.ToString(), sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgov.20.GestEsitoInvio.LoadDettaglioEsiti.errore::", ex); ;
                throw;
            }
        }
    }
}