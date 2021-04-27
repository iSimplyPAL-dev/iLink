using Microsoft.VisualBasic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;
using IRemInterfaceOSAP;
using DTO;
using log4net;
using AnagInterface;

namespace OPENgovTOCO.Dichiarazioni
{
    /// <summary>
    /// Pagina per l'inserimento dichiarazione
    /// </summary>
    public partial class DichiarazioniAdd :BaseEnte
	{
		
		#region  Web Form Designer Generated Code
		
		//This call is required by the Web Form Designer.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{

		}
		
	
		
		protected void Page_Init(System.Object sender, System.EventArgs e)
		{
			//CODEGEN: This method call is required by the Web Form Designer
			//Do not modify it using the code editor.
			InitializeComponent();
		}
		
		#endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioniAdd));
        //--- 20140428 aggiunto il new a wuc perchè dava errore
		private Wuc.WucDichiarazione wuc;//= new Wuc.WucDichiarazione();
		/// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(System.Object sender, System.EventArgs e)
		{
			//Put user code to initialize the page here
			lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;

            wuc = (Wuc.WucDichiarazione)(this.FindControl("wucDichiarazione"));
            wuc.OpType = OSAPConst.OperationType.ADD;

            //string[] clientArrayControl = wuc.GetMandatoryFields();
            //this.Salva.Attributes.Add("onClick","return ValidateForm('" + clientArrayControl[0] + "','" + clientArrayControl[1] + "');DichiarazioneValidate()");  
			if (Page.IsPostBack)
			{
				wuc.IdDichiarazione = - 1;		
			}
			else
			{
				if (Request["FromArticoli"] == null)
				{
					DichiarazioneTosapCosap objDichiarazione = new DichiarazioneTosapCosap();
					objDichiarazione.IdEnte = DichiarazioneSession.IdEnte;
                    objDichiarazione.CodTributo = DichiarazioneSession.CodTributo("");
					DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
				}
			}
            //*** 201504 - Nuova Gestione anagrafica con form unico ***
            DettaglioAnagrafica oDettaglioAnagrafica = new DettaglioAnagrafica();
            try
            {
                if (hdIdContribuente.Value != "-1")
                {
                    if (DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente == null)
                    {
                        oDettaglioAnagrafica.COD_CONTRIBUENTE = int.Parse(hdIdContribuente.Value);
                        DichiarazioneSession.SessionDichiarazioneTosapCosap.AnagraficaContribuente = oDettaglioAnagrafica;
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniAdd.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Salva_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string sScript = string.Empty;
            sScript = "GestAlert('a', 'warning', '', '', 'Bisogna inserire gli articoli prima di salvare!');" + "\r\n";
            //controllo che ci siano articoli prima di inserire altrimenti errore
            if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione == null)
            {
                RegisterScript(sScript, this.GetType());
            }
            else
            {
                if (DichiarazioneSession.SessionDichiarazioneTosapCosap.ArticoliDichiarazione.GetLength(0) >= 1)
                {
                    wuc.SalvaDichiarazione();
                }
                else
                {
                    RegisterScript(sScript, this.GetType());
                }
            }
        }
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void btnRibalta_Click(System.Object sender, System.EventArgs e)
		{
            ILog Log = LogManager.GetLogger(typeof(DichiarazioniAdd));
            try {
                Log.Debug("Dichiarazioni::DichiarazioniAdd::btnRibalta_Click::ribalto l'anagrafica");
                if (ViewState["sessionName"] == null)
                {
                    Log.Debug("UFFI :( la Sessione è nulla");
                }
                wuc.RibaltaAnagrafica();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.DichiarazioniAdd.btnRibatla_Click.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Cancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect (OSAPPages.DichiarazioniSearch + "?NewSearch=false&CodTributo="+DichiarazioneSession.CodTributo(""), false);
		}
	}
}
