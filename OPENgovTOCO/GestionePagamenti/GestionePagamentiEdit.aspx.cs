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
	/// Pagina per la modifica pagamento
	/// </summary>
	public partial class GestionePagamentiEdit :BasePage
	{
		protected System.Web.UI.WebControls.Button btnRibalta;
		private Wuc.WucGestionePagamenti wuc;
        private static readonly ILog Log = LogManager.GetLogger(typeof(GestionePagamentiEdit));

        protected void Page_Load(object sender, System.EventArgs e)
		{
			//Put user code to initialize the page here
			lblTitolo.Text = DichiarazioneSession.DescrizioneEnte;

            try {
                if (DichiarazioneSession.CodTributo("").ToString() == Utility.Costanti.TRIBUTO_SCUOLE.ToString())
                    info.InnerText = "SCUOLA";
                else
                    info.InnerText = "TOSAP/COSAP";
                info.InnerText += " - Pagamenti - Gestione";

                wuc = (Wuc.WucGestionePagamenti)(this.FindControl("wucGestionePagamenti"));

                             if (Page.IsPostBack)
                {
                    btnSalva.Enabled = true;
                    wuc.IdPagamento = -1;
                    wuc.IdContribuente = int.Parse(hdIdContribuente.Value);
                }
                else
                {
                    string sScript = "";
                    Type csType = this.GetType();
                    sScript = "<script language='javascript'>";
                    sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';";
                    sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';";
                    sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';";
                    sScript += "</script>";
                    ClientScript.RegisterClientScriptBlock(csType, "idcontrpar", sScript);
                    if (Request["IdPagamento"] != null && Request["CodContribuente"] != null)
                    {
                        int IdPagamento = int.Parse(Request["IdPagamento"].ToString());
                        int CodContribuente = int.Parse(Request["CodContribuente"].ToString());

                        wuc.IdContribuente = CodContribuente;
                        wuc.IdPagamento = IdPagamento;
                        wuc.OpType = OSAPConst.OperationType.VIEW;
                    }

                    // Debug 
                    //				wuc.IdContribuente = 23536;
                    //				wuc.IdPagamento = 66;
                    wuc.OpType = OSAPConst.OperationType.VIEW;

                    DichiarazioneTosapCosap objDichiarazione = new DichiarazioneTosapCosap();
                    objDichiarazione.IdEnte = DichiarazioneSession.IdEnte;
                    objDichiarazione.CodTributo = DichiarazioneSession.CodTributo("");
                    DichiarazioneSession.SessionDichiarazioneTosapCosap = objDichiarazione;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiEdit.Page_Load.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }


		protected void btnSalva_OnClick(System.Object sender, System.EventArgs e)
		{
			int tmpIdPagam = -1;
			int tmpImpCC = 0;

			GetIds(out tmpIdPagam, out tmpImpCC);

			wuc.IdContribuente = tmpImpCC;
			wuc.IdPagamento = tmpIdPagam;
			wuc.OpType = OSAPConst.OperationType.EDIT;

            try {
                if (tmpIdPagam > 0 && tmpImpCC > 0)
                {
                    if (wuc.UpdatePagamento() < 0)
                    {
                        wuc.LoadPagamentoForEdit(tmpImpCC, tmpIdPagam);
                        wuc.SetPanelVisibility();
                    }
                }
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiEdit.btnSalva_OnClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		protected void btnCancella_OnClick(System.Object sender, System.EventArgs e)
		{
            try {
                int esitoIns = -1;
                string sScript = string.Empty;

                int tmpIdPagam = -1;
                int tmpImpCC = 0;

                GetIds(out tmpIdPagam, out tmpImpCC);

                if (tmpIdPagam > 0)
                {
                    // Elimino
                    esitoIns = MetodiPagamento.DeletePagamento(tmpIdPagam);

                    if (esitoIns < 0)
                        sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un errore.');";
                    else
                        sScript = "GestAlert('a', 'success', '', '', 'Pagamento eliminato correttamente');";

                    // Redirect su pagica ricerca
                    if (esitoIns >= 0)
                        sScript = sScript + "location.href = '" + OSAPPages.GestionePagamentiSearch + "?NewSearch=false'";

                }
                else
                {
                    sScript = "Impossibile recuperare il pagamento da cancellare";
                }

                // Pubblic gli script
                if (sScript != "")
                    RegisterScript(sScript,this.GetType());

                if (esitoIns < 0)
                {
                    wuc.LoadPagamentoForEdit(tmpImpCC, tmpIdPagam);
                    wuc.SetPanelVisibility();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GestionePagamentiEdit.btnCancella_OnClick.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

		private void GetIds(out int idPagamento, out int idContribuente)
		{
			int tmpIdPagam = -1;
			int tmpImpCC = 0;

			TextBox TxtIdPagamento = (TextBox)wuc.FindControl("TxtIdPagamento");
			if (TxtIdPagamento != null)
			{				
				try
				{
					tmpIdPagam = int.Parse(TxtIdPagamento.Text);
					wuc.OpType = OSAPConst.OperationType.EDIT;
				} 
				catch { tmpIdPagam = -1; }			
			}

            //TextBox TxtCodContribuente = (TextBox)wuc.FindControl("TxtCodContribuente");
            //if (!SharedFunction.IsNullOrEmpty(TxtCodContribuente.Text) && 
            //    ((TxtCodContribuente.Text != "-1") && (TxtCodContribuente.Text != "0")))
            //{				
            //    try
            //    {
            //        tmpImpCC = int.Parse(TxtCodContribuente.Text);;							
            //    } 
            //    catch { tmpImpCC = 0; }
            //}
            if (!SharedFunction.IsNullOrEmpty(hdIdContribuente.Value) &&
                ((hdIdContribuente.Value != "-1") && (hdIdContribuente.Value != "0")))
            {
                try
                {
                    tmpImpCC = int.Parse(hdIdContribuente.Value); ;
                }
                catch { tmpImpCC = 0; }
            }

			idPagamento = tmpIdPagam;
			idContribuente = tmpImpCC ;

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
