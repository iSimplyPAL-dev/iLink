using System;
using System.Data;
using System.Data.SqlClient;
using Business;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione dei versamenti non abbinati a dichiarazioni.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class VersamentiNoDichView:Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(VersamentiNoDichView));
        public VersamentiNoDichView()
		{
			this.TableName = "viewVersamentiNoDich";
		}


		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns>Restituisce un DataView</returns>
		public DataView AnniCaricati()
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "Select AnnoRiferimento From " + this.TableName + " GROUP BY AnnoRiferimento, ente having ente = '"+ ConstWrapper.CodiceEnte +"' order by AnnoRiferimento DESC";
            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;

            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDichView.AnniCaricati.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Torna una DataView con l'elenco dei versamenti errati filtrati per ente, anno di riferimento e errore.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <returns>Restituisce un DataView</returns>
		public DataView List(string ente, string annoRiferimento)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE Ente=@ente"; //AND Annullato<>1";
            try { 
			//if(annoRiferimento != String.Empty)
			if(annoRiferimento != "")
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
	
			SelectCommand.CommandText += " ORDER BY COGNOME, NOME";
			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoDichView.List.errore: ", Err);
                throw Err;
            }
        }
	}
}
