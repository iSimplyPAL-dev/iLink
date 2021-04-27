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
using OPENgovTOCO;
using log4net;

namespace OPENGovTOCO.GestionePagamenti
{
    /// <summary>
    /// Pagina per l'inserimento pagamento
    /// </summary>
    public partial class GestionePagamentiAdd :BasePage
	{
		protected System.Web.UI.WebControls.Button btnCancella;
		protected System.Web.UI.WebControls.Button btnModifica;
		private Wuc.WucGestionePagamenti wuc;
        private static readonly ILog Log = LogManager.GetLogger(typeof(GestionePagamentiAdd));

		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                Log.Debug("entro pag add::" + hdIdContribuente.Value);
                //Put user code to initialize the page here
                lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerText += " - Pagamenti - Gestione";

                wuc = (Wuc.WucGestionePagamenti)(this.FindControl("wucGestionePagamenti"));
                //wuc.OpType = OSAPConst.OperationType.ADD;

                string[] clientArrayControl = wuc.GetMandatoryFields();
                this.btnSalva.Attributes.Add("onClick", "return ValidatePagamentiForm('" + clientArrayControl[0] + "','" + clientArrayControl[1] + "');");
                
                if (Page.IsPostBack)
                {
                    btnSalva.Enabled = true;
                    wuc.IdPagamento = -1;
                    wuc.IdContribuente =int.Parse(hdIdContribuente.Value);
                }
                else
                {
                    if (Request["IdPagamento"] != null && Request["CodContribuente"] != null)
                    {
                        int IdPagamento = int.Parse(Request["IdPagamento"].ToString());
                        int CodContribuente = int.Parse(Request["CodContribuente"].ToString());

                        wuc.IdContribuente = CodContribuente;
                        wuc.IdPagamento = IdPagamento;
                        wuc.OpType = OSAPConst.OperationType.VIEW;
                    }

                    DichiarazioneTosapCosap objDichiarazione = new DichiarazioneTosapCosap();
                    objDichiarazione.IdEnte = DichiarazioneSession.IdEnte;
                    objDichiarazione.CodTributo = DichiarazioneSession.CodTributo("");
                    DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiAdd.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		protected void btnSalva_OnClick(System.Object sender, System.EventArgs e)
		{
			wuc.SavePagamento();
		}

		protected void btnRibalta_OnClick(System.Object sender, System.EventArgs e)
		{
            Log.Debug("devo inizio ribalta");
			wuc.RibaltaAnagrafica();
		}

		protected void btnRicerca_OnClick(System.Object sender, System.EventArgs e)
		{
			wuc.GetRateData();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
