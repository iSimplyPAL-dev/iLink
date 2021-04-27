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
using Anater.Oggetti;
using RemotingInterfaceAnater;

namespace DichiarazioniICI.Anater
{
    /// <summary>
    /// Pagina per l'inserimento degli immobili da ANATER.
    /// Effettuando una ricerca e selezionando l'immobile interessato il sistema precompila il più possibile con i dati di ANATER.
    /// Le possibili opzioni sono:
    /// - Associa
    /// - Ricerca
    /// - Torna alla videata precedente
    /// </summary>
    public partial class RicercaImmobile :BasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(RicercaImmobile));
        /// <summary>
        /// 
        /// </summary>
        protected string UrlStradario = Business.ConstWrapper.UrlStradario;

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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try { 
			// Put user code to initialize the page here
			if (!IsPostBack) 
			{
				LnkApriStradario.Attributes.Add("onclick", "ApriStradario();");
				LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();");
				if ((Request.Params["IDContribuente"] != "")) 
				{
					txtIdContribuente.Value=Request.Params["IDContribuente"];
				}
				else
				{
					txtIdContribuente.Value="-1";
				}
				if ((Request.Params["lblCognomeContr"] != "")) 
				{
					lblCognomeContr.Text = Request.Params["lblCognomeContr"];
				}
				if ((Request.Params["lblNomeContr"] != "")) 
				{
					lblNomeContr.Text = Request.Params["lblNomeContr"];
				}
				if ((Request.Params["lblDataNascContr"] != "")) 
				{
					lblDataNascContr.Text = Request.Params["lblDataNascContr"];
				}
				if ((Request.Params["lblResidContr"] != "")) 
				{
					lblResidContr.Text = Request.Params["lblResidContr"];
				}
				if ((Request.Params["lblComuneContr"] != "")) 
				{
					lblComuneContr.Text = Request.Params["lblComuneContr"];
				}
			}
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaImmobile.Page_Load.errore: ", Err);
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
			try 
			{
                String sScript = "GestAlert('a', 'info', '', '', 'Funzionalita\' non attiva.');";
                RegisterScript(sScript, this.GetType());
             }
            catch (Exception Err) 
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaImmobile.CmdRicerca_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
			}		
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void CmdRibaltaAnater_Click(object sender, System.EventArgs e)
		{
			string sScript;

			try {
				foreach (GridViewRow MyItem in GrdRicerca.Rows) 
				{
					if (((CheckBox)(MyItem.FindControl("ChkSelezionato"))).Checked==true)
					{
						sScript = "opener.document.getElementById(\'txtIdTerUI\').value="+ MyItem.Cells[11].Text.ToString() +";";
						sScript += "opener.document.getElementById(\'txtIdTerProprieta\').value="+ MyItem.Cells[12].Text.ToString() +";";
						sScript += "opener.document.getElementById(\'txtIdTerProprietario\').value="+ MyItem.Cells[13].Text.ToString() +";";
						sScript += "window.close();";
						sScript += "window.opener.document.forms(0).submit();";
						RegisterScript(sScript,this.GetType());
					}
				}
			}
			catch (Exception Err) {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.RicercaImmobile.CmdRibaltaAnater_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
			}

		}
	}
}
