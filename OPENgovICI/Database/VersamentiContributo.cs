using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Business;
using log4net;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione della tabella TblVersamenti.
	/// </summary>
	public class VeramantiContributo : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(VeramantiContributo));
        public VeramantiContributo()
		{
			
		}

		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns>Restituisce un dataView</returns>
		public DataView AnniCaricati()
		{
			SqlCommand SelectCommand = new SqlCommand();
			//SelectCommand.CommandText = "Select AnnoRiferimento From tblversamenti GROUP BY AnnoRiferimento, ente having ente = '"+ ConstWrapper.CodiceEnte +"' order by AnnoRiferimento DESC";
			
			SelectCommand.CommandText = "SELECT YEAR(DataPagamento) AS annoPagamento "+
				"from tblversamenti where ente = '"+ ConstWrapper.CodiceEnte +"' group by YEAR(DataPagamento) ORDER BY annopagamento DESC";

			DataView dv=Query(SelectCommand,new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
		}

        public double GetContributo(string ente)
        {
            double Contributo = 0;
            SqlCommand SelectCommand = new SqlCommand();
            try { 
            SelectCommand.CommandText = "SELECT PERCCONTRIBUTOANCICNC FROM ENTI WHERE COD_ENTE='" + ente + "'";

            DataView dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov)).DefaultView;
            foreach (DataRowView myRow in dv)
                double.TryParse(myRow["PERCCONTRIBUTOANCICNC"].ToString(), out Contributo);
            return Contributo;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VeramantiContributo.GetContributo.errore: ", Err);
                throw Err;
            }

        }

        /// <summary>
        /// Torna l'importo totale dei versamenti ordinari
        /// </summary>
        /// <param name="anno"></param>
        /// <param name="ente"></param>
        /// <returns>Restituisce un dataView</returns>
        public DataView TotaleImportoVersOrdinari(string anno, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText ="select sum(importoPagato) as Totale " +
				"from tblversamenti where ravvedimentoOperoso=0 and violazione=0 and Ente='" + ente + "'";

			if(anno != "0")
				SelectCommand.CommandText += " and AnnoRiferimento='" + anno + "'";

            DataView dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VeramantiContributo.TotaleImportoVersOrdinari.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna l'importo totale dei versamenti da violazione
		/// </summary>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns>Restituisce un dataView</returns>
		public DataView TotaleImportoVersViolazione(string anno, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText ="select sum(importoPagato) as Totale " +
				"from tblversamenti " +
				"where ravvedimentoOperoso=0 and violazione=1 and Ente='" + ente + "'";

			if(anno != "0")
				SelectCommand.CommandText += " and AnnoRiferimento='" + anno + "'";

            DataView dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VeramantiContributo.TotaleImportoVersViolazione.errore: ", Err);
                throw Err;
            }

        }

		/// <summary>
		/// Torna l'importo totale dei versamenti da ravvedimento operoso
		/// </summary>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns>Restituisce un dataView</returns>
		public DataView TotaleImportoVersRavOperoso(string anno, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText ="select sum(importoPagato) as Totale " +
				"from tblversamenti " +
				"where ravvedimentoOperoso=1 and violazione=0 and Ente='" + ente + "'";

			if(anno != "0")
				SelectCommand.CommandText += " and AnnoRiferimento='" + anno + "'";

            DataView dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;

            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VeramantiContributo.TotaleImportoVersRavOperoso.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna l'importo totale di tutti i versamenti
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <returns>Restituisce un dataView</returns>
		public DataView TotaleImportoVersamenti(string ente, string annoRiferimento)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "select sum(importoPagato) as Totale" +
				" from tblversamenti WHERE Ente= '" + ente + "'";

			if(annoRiferimento != "0")
				SelectCommand.CommandText += " AND AnnoRiferimento='" + annoRiferimento + "'";

            DataView dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VeramantiContributo.TotaleImportoVersamenti.errore: ", Err);
                throw Err;
            }
        }

	}
}