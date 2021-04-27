using System;
using System.Data;
using System.Data.SqlClient;
using Business;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione dei versamenti non abbinati ad anagrafiche.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class VersamentiNoAnagView:Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(VersamentiNoAnagView));
        public VersamentiNoAnagView()
		{
			this.TableName = "viewVersamentiNoAnag";
		}


		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns>Restituisce un DataView</returns>
		public DataView AnniCaricati()
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "Select AnnoRiferimento From " + this.TableName + " GROUP BY AnnoRiferimento, ente having ente = '"+ ConstWrapper.CodiceEnte +"' order by AnnoRiferimento DESC";
            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
		}


		/// <summary>
		/// Torna una DataView con l'elenco dei versamenti errati filtrati per ente, anno di riferimento e errore.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <returns>Restituisce un DataView</returns>
		public DataView List(string ente, string annoRiferimento)
		{
            try {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
                    " WHERE Ente=@ente"; //AND Annullato<>1";

                //if(annoRiferimento != String.Empty)
                if (annoRiferimento != "")
                    SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";

                SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
                SelectCommand.Parameters.Add("@annoRiferimento", SqlDbType.VarChar).Value = annoRiferimento;

                DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
                kill();
                return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiNoAnag.List.errore: ", Err);
                throw Err;
            }
        }
	}
}
