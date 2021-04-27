using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione della vista viewVersamentiUtil.
	/// </summary>
	public class VersamentiUtilView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(VersamentiUtilView));
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public VersamentiUtilView()
		{
			this.TableName = "viewVersamentiUtil";
		}

		/// <summary>
		/// Torna una DataView valorizzata col elenco dei versamenti filtrati per ente, anno di
		/// riferimento, cognome, nome, codice fiscale e partita IVA.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <param name="cognome"></param>
		/// <param name="nome"></param>
		/// <param name="codiceFiscale"></param>
		/// <param name="partitaIva"></param>
		/// <returns></returns>
		public DataView List(string ente, string annoRiferimento, string cognome, string
			nome, string codiceFiscale, string partitaIva)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE Ente=@ente AND (Cognome LIKE @cognome) AND (Nome LIKE @nome)" +
				" AND (CodiceFiscale LIKE @codiceFiscale) AND (PartitaIva LIKE @partitaIva)";

			if(annoRiferimento != String.Empty)
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";

			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = cognome + "%";
			SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome + "%";
			SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale + "%";
			SelectCommand.Parameters.Add("@partitaIva", SqlDbType.VarChar).Value = partitaIva + "%";
			SelectCommand.Parameters.Add("@annoRiferimento", SqlDbType.VarChar).Value = annoRiferimento;

			DataView dv=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TpSituazioneFinaleIci.List.errore: ", Err);
                throw Err;
            }

        }
    }
}
