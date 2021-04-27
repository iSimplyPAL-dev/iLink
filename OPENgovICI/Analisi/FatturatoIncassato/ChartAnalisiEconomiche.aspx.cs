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
using WebChart;
namespace DichiarazioniICI.Analisi.FatturatoIncassato
{
/// <summary>
/// Pagina per la visualizzazione in formato grafico dell'analisi fatturato/incassato.
/// </summary>
/// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
	public partial class ChartAnalisiEconomiche :BasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ChartAnalisiEconomiche));
	
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
			// Put user code to initialize the page here
			try
			{
                log.Debug("ChartAnalisiEconomiche::Page_Load::inizio");
				DataSet ds = CreateDataSet();

				ColumnChart ChartDovuto = new ColumnChart(); //StackedColumnChart ChartDovuto = new StackedColumnChart();
				ChartDovuto.Fill.Color = Color.FromArgb(50, Color.CadetBlue);
				ChartDovuto.MaxColumnWidth=30;
				ChartDovuto.Shadow.Visible = false;
				ChartDovuto.Legend = "Dovuto";
				 
				ColumnChart ChartVersato = new ColumnChart();//StackedColumnChart ChartVersato = new StackedColumnChart();
				ChartVersato.Fill.Color = Color.FromArgb(50, Color.MediumVioletRed);
				ChartVersato.MaxColumnWidth=30;
				ChartVersato.Shadow.Visible = false;
				ChartVersato.Legend = "Versato";
								 
				foreach(DataRow row in ds.Tables[0].Rows) 
				{
					ChartDovuto.Data.Add(new ChartPoint(row["tipovoce"].ToString(), (int)row["dovuto"])); 
					ChartVersato.Data.Add(new ChartPoint(row["tipovoce"].ToString(), (int)row["versato"]));
				}
 
				MyChartAnalisi.Charts.Add(ChartDovuto);
				MyChartAnalisi.Charts.Add(ChartVersato);
				MyChartAnalisi.YValuesFormat= "{0:N}";
				MyChartAnalisi.Legend.Position= LegendPosition.Bottom;
				MyChartAnalisi.LeftChartPadding=50;
				MyChartAnalisi.RedrawChart();
                log.Debug("ChartAnalisiEconomiche::Page_Load::file name::" + MyChartAnalisi.FileName);
			}
			catch (Exception err) 
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ChartAnalisiEconomiche.Page_Load.errore: ", err);
                Response.Redirect("../../../PaginaErrore.aspx");
            }
		}

		/// <summary>
		/// Just generate some random data
		/// </summary>
		DataSet CreateDataSet() 
		{
            DataSet ds = new DataSet();
            try
            {
                double impDovuto = 0;
                double impVersato = 0;

                DataTable table = ds.Tables.Add("Table");
                table.Columns.Add("tipovoce");
                table.Columns.Add("dovuto", typeof(int));
                table.Columns.Add("versato", typeof(int));

                DataRow row = table.NewRow();
                row["tipovoce"] = "Abitazione Princ.";
                impDovuto = double.Parse(Request["AbiPrinDovuto"].ToString());
                impVersato = double.Parse(Request["AbiPrinVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add abiprinc");

                /**** 20120828 - IMU adeguamento per importi statali ****/
                row = table.NewRow();
                row["tipovoce"] = "Ter.Agr. Comune";
                impDovuto = double.Parse(Request["TerAgrDovuto"].ToString());
                impVersato = double.Parse(Request["TerAgrVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add teragr");

                row = table.NewRow();
                row["tipovoce"] = "Ter.Agr. Stato";
                impDovuto = double.Parse(Request["TerAgrDovutoStato"].ToString());
                impVersato = double.Parse(Request["TerAgrVersatoStato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add teragrstato");

                row = table.NewRow();
                row["tipovoce"] = "Ter.Fab. Comune";
                impDovuto = double.Parse(Request["TerFabDovuto"].ToString());
                impVersato = double.Parse(Request["TerFabVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add areefab");

                row = table.NewRow();
                row["tipovoce"] = "Ter.Fab. Stato";
                impDovuto = double.Parse(Request["TerFabDovutoStato"].ToString());
                impVersato = double.Parse(Request["TerFabVersatoStato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add areefabrstato");
                
                row = table.NewRow();
                row["tipovoce"] = "Altri Fab. Comune";
                impDovuto = double.Parse(Request["AltriFabDovuto"].ToString());
                impVersato = double.Parse(Request["AltriFabVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add altrifab");

                row = table.NewRow();
                row["tipovoce"] = "Altri Fab. Stato";
                impDovuto = double.Parse(Request["AltriFabDovutoStato"].ToString());
                impVersato = double.Parse(Request["AltriFabVersatoStato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add altrifabstato");

                row = table.NewRow();
                row["tipovoce"] = "Fab.Rur. Uso Strum.";
                impDovuto = double.Parse(Request["FabRurUsoStrumDovuto"].ToString());
                impVersato = double.Parse(Request["FabRurUsoStrumVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add farrur");
                /**** ****/
                /**** 20130422 - aggiornamento IMU ****/
                row = table.NewRow();
                row["tipovoce"] = "Fab.Rur. Uso Strum. Stato";
                impDovuto = double.Parse(Request["FabRurUsoStrumDovutoStato"].ToString());
                impVersato = double.Parse(Request["FabRurUsoStrumVersatoStato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add farrurstato");

                row = table.NewRow();
                row["tipovoce"] = "Uso Prod.Cat. D";
                impDovuto = double.Parse(Request["UsoProdCatDDovuto"].ToString());
                impVersato = double.Parse(Request["UsoProdCatDVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add catd");

                row = table.NewRow();
                row["tipovoce"] = "Uso Prod.Cat.D Stato";
                impDovuto = double.Parse(Request["UsoProdCatDDovutoStato"].ToString());
                impVersato = double.Parse(Request["UsoProdCatDVersatoStato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add catdstato");
                /**** ****/

                row = table.NewRow();
                row["tipovoce"] = "Detrazione";
                impDovuto = double.Parse(Request["DetrazDovuto"].ToString());
                impVersato = double.Parse(Request["DetrazVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add detraz");

                row = table.NewRow();
                row["tipovoce"] = "Totale";
                impDovuto = double.Parse(Request["TotaleDovuto"].ToString());
                impVersato = double.Parse(Request["TotaleVersato"].ToString());
                row["dovuto"] = impDovuto;
                row["versato"] = impVersato;
                table.Rows.Add(row);
                log.Debug("Si è verificato un errore in ChartAnalisiEconomiche::CreateDataSet::add tot");
            }
            catch (Exception err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ChartAnalisiEconomiche.CreateDataSet.errore: ", err);
                Response.Redirect("../../../PaginaErrore.aspx");
            }
            return ds;
		}	
	}
}
