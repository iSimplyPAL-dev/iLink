using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione della vista viewContribuentiUtil.
	/// </summary>
	public class ContribuentiUtilView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContribuentiUtilView));

        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public ContribuentiUtilView()
		{
			this.TableName = "viewContribuentiUtil";
		}

		/// <summary>
		/// Torna una DataTable con la lista dei contribuenti filtrati per cognoem, nome,
		/// codice fiscale e/o partita Iva.
		/// </summary>
		/// <param name="cognome"></param>
		/// <param name="nome"></param>
		/// <param name="codiceFiscale"></param>
		/// <param name="partitaIVA"></param>
		/// <param name="ente"></param>
		/// <param name="effettivo"></param>
		/// <returns></returns>
		public DataTable ListContribuenti(string cognome, string nome, string codiceFiscale,
			string partitaIVA, string ente, Effettivo effettivo)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE (Cognome LIKE @cognome) AND (Nome LIKE @nome) AND (CodiceFiscale" +
				" LIKE @codiceFiscale) AND (PartitaIva LIKE @partitaIVA) AND (Ente LIKE @ente)";
            try { 
			switch(effettivo)
			{
				case Effettivo.Effettivi:
					SelectCommand.CommandText += " AND Effettivo=1";
					break;

				case Effettivo.NonEffettivi:
					SelectCommand.CommandText += " AND Effettivo=0";
					break;
			}

			SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = cognome + "%";
			SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome + "%";
			SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale + "%";
			SelectCommand.Parameters.Add("@partitaIVA", SqlDbType.VarChar).Value = partitaIVA + "%";
			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";

			DataTable dt=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ContribuentiUtilView.ListContribuenti.errore: ", Err);
                throw Err;
            }
        }
	}
}
