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

using ComPlusInterface;
using Ribes;
using System.Configuration;
using Business;
using log4net;

namespace DichiarazioniICI.CalcoloICI.GestioneElaborazione
{
    /// <summary>
    /// Pagina per la visualizzazione dell'elaborazione in corso.
    /// </summary>
    public partial class ViewElaborazioneICImassivo :BasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ViewElaborazioneICImassivo));
        /// <summary>
        /// 
        /// </summary>
        public const string Elaborazione_in_Corso = "ELABORAZIONE IN CORSO";
        /// <summary>
        /// 
        /// </summary>
		public const string Elaborazione_Terminata_con_successo = "ELABORAZIONE TERMINATA CON SUCCESSO";
        /// <summary>
        /// 
        /// </summary>
		public const string Elaborazione_Terminata_con_errori = "ELABORAZIONE TERMINATA CON ERRORI";
        /// <summary>
        /// 
        /// </summary>
		public const string Non_ci_sono_elaborazioni_in_corso = "NON CI SONO ELABORAZIONI IN CORSO";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			Hashtable objHashTable = new Hashtable(); 
			string strConnectionStringOPENgovICI; 
			string strConnectionStringOPENgovProvvedimenti; 
			string strConnectionStringAnagrafica; 
			string strConnectionStringOPENgovTerritorio; 
			string strConnectionStringOPENgovCatasto;

            try{
            //*** 20140509 - TASI ***
            strConnectionStringOPENgovICI = ConstWrapper.StringConnection;
            strConnectionStringAnagrafica = ConstWrapper.StringConnectionAnagrafica;
            //*** ***
			strConnectionStringOPENgovProvvedimenti = "";
			strConnectionStringOPENgovTerritorio = "";
			strConnectionStringOPENgovCatasto = "";
			objHashTable.Add("CodENTE", ConstWrapper.CodiceEnte ); 

			objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", strConnectionStringOPENgovICI); 
			objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", strConnectionStringOPENgovProvvedimenti); 
			objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", strConnectionStringAnagrafica); 
			objHashTable.Add("CONNECTIONSTRINGOPENGOVTERRITORIO", strConnectionStringOPENgovTerritorio); 
			objHashTable.Add("CONNECTIONSTRINGOPENGOVCATASTO", strConnectionStringOPENgovCatasto); 
			objHashTable.Add("USER", Session["username"]); 
			objHashTable.Add("COD_TRIBUTO", Session["COD_TRIBUTO"]); 				

			string strTipoElaborazione = Request["TIPO_ELABORAZIONE"].ToString();
			objHashTable.Add("TIPO_ELABORAZIONE", strTipoElaborazione );

			IFreezer remObject =(IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI); 
			string strViewCodaCalcoloICIMassivo;
			strViewCodaCalcoloICIMassivo=remObject.ViewCodaCalcoloICIMassivo(strConnectionStringOPENgovICI, ConstWrapper.CodiceEnte);
            log.Debug("ViewElaborazioneICImassivo::page_load::stato prelevato::"+strViewCodaCalcoloICIMassivo);
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder ();
			switch (strViewCodaCalcoloICIMassivo.ToUpper())
			{
				case Elaborazione_in_Corso:
				    strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneIncorso.style.display='';");
				    strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneTerminata.style.display='none';");
				    strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneErrore.style.display='none';");
				    strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').NessunaElaborazione.style.display='none';");
					break;

				case Elaborazione_Terminata_con_successo:
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneTerminata.style.display='';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneIncorso.style.display='none';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneErrore.style.display='none';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').NessunaElaborazione.style.display='none';");
					//strBuilder.Append("parent.Visualizza.frames.item('Repository').location.href='../GestioneStatoElaborazione/Repository.aspx" + strPARAMETRI + "';");
					strBuilder.Append("location.href='../aspVuota.aspx';");
					break;

                case Elaborazione_Terminata_con_errori:
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneErrore.style.display='';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneTerminata.style.display='none';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').NessunaElaborazione.style.display='none';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneIncorso.style.display='none';");
					strBuilder.Append("location.href='../aspVuota.aspx';");
					break;

				case Non_ci_sono_elaborazioni_in_corso:
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').NessunaElaborazione.style.display='';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneErrore.style.display='none';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneIncorso.style.display='none';");
					strBuilder.Append("parent.Visualizza.frames.item('ProgressTask').ElaborazioneTerminata.style.display='none';");
					strBuilder.Append("location.href='../aspVuota.aspx';");
					break;

			  }
			RegisterScript(strBuilder.ToString(),this.GetType() );
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ViewElaborazioneICImassivo.Page_Load.errore: ", Err);
                Response.Redirect("../../../PaginaErrore.aspx");
            }
        }

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
	}
}
