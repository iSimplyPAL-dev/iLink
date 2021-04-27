using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// classe DettagliStoricoView
	/// </summary>
	public class DettagliStoricoView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(DettagliStoricoView));
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public DettagliStoricoView()
		{
			//this.TableName = "viewStoricoParini";
		}

		/// <summary>
		/// Torna una DataTable valorizzato con l'elenco degli immobili e dei dettagli
		/// filtrato per codice ente.
		/// </summary>
		/// <param name="codente"></param>
		/// <returns>
		/// Restituisce un dataTable.
		/// </returns>
		public DataTable ListPariniByCodEnte(string codente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM viewStoricoParini WHERE CODENTE=@codente";

			SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = codente;
            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliStoricoView.ListPariniByCodEnte.errore: ", Err);
                throw Err;
            }
        }

		
		/// <summary>
		/// Torna una DataTable valorizzato con l'elenco degli immobili e dei dettagli
		/// filtrato per codice ente.
		/// </summary>
		/// <param name="codente"></param>
		/// <returns>
		/// Restituisce un DataTable
		/// </returns>
		public DataTable ListSaecByCodEnte(string codente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM viewStoricoSaec WHERE COD_ENTE=@codente";

			SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = codente;
            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliStoricoView.ListSaecByCodEnte.errore: ", Err);
                throw Err;
            }
        }

		
		/// <summary>
		/// Cerca l'elenco delle dichiarazioni originali in base ai parametri di ricerca passati.
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="Ente"></param>
		/// <param name="Cognome"></param>
		/// <param name="Nome"></param>
		/// <param name="CF"></param>
		/// <param name="PI"></param>
		/// <param name="Foglio"></param>
		/// <param name="Numero"></param>
		/// <param name="Sub"></param>
		/// <param name="Progressivo"></param>
		/// <returns>
		/// Restituisce un dataTable delle dichiarazioni.
		/// </returns>
		public DataTable RicercaDichiarazioniOriginali(long ID, string Ente, string Cognome, string Nome, string CF, string PI,string Foglio,string Numero,string Sub, string Progressivo )
		{
			SqlCommand SelectCommand = new SqlCommand();
			string sSelect=string.Empty;
            try { 
			if(ID==-1)
			{
				SelectCommand.CommandText = "SELECT ID, Cognome_contribuente, Nome_contribuente, Codice_fiscale_contribuente, PartitaIva_contribuente, Foglio, Numero, Subalterno, N_PROGRESSIVO FROM DICHIARAZIONI_ORIGINALI" +
				" WHERE (Cognome_contribuente LIKE @cognome) AND (Nome_contribuente LIKE @nome) AND " +
					" (Codice_fiscale_contribuente LIKE @codiceFiscale) AND (PartitaIva_contribuente LIKE @partitaIVA) AND (ENTE LIKE @ente)" +
					" AND (FOGLIO LIKE @Foglio) AND (NUMERO LIKE @Numero) AND (SUBALTERNO LIKE @Sub) AND (N_PROGRESSIVO LIKE @Progressivo)" + 
					" ORDER BY Cognome_contribuente, Nome_contribuente, FOGLIO,NUMERO,SUBALTERNO, N_PROGRESSIVO";


				SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = Cognome + "%";
				SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = Nome + "%";
				SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = CF + "%";
				SelectCommand.Parameters.Add("@partitaIVA", SqlDbType.VarChar).Value = PI + "%";
				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = Ente;
				SelectCommand.Parameters.Add("@Foglio", SqlDbType.VarChar).Value = Foglio + "%";
				SelectCommand.Parameters.Add("@Numero", SqlDbType.VarChar).Value = Numero + "%";
				SelectCommand.Parameters.Add("@Sub", SqlDbType.VarChar).Value = Sub + "%";
				SelectCommand.Parameters.Add("@Progressivo", SqlDbType.VarChar).Value = Progressivo + "%";
			}

			else
			{
				SelectCommand.CommandText = "SELECT * FROM DICHIARAZIONI_ORIGINALI " +
					" WHERE (ID=@id) ";
				SelectCommand.Parameters.Add("@id",SqlDbType.NVarChar).Value = ID;
			}

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliStoricoView.RicercaDichiarazioniOriginali.errore: ", Err);
                throw Err;
            }
        }
	}
}