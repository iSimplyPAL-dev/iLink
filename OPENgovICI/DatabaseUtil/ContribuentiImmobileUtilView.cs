using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// 
	/// </summary>
	public enum Effettivo
	{
		Tutti = -1,
		Effettivi,
		NonEffettivi
	}

	/// <summary>
	/// Classei di gestione della vista viewContribuentiImmobileUtil.
	/// </summary>
	public class ContribuentiImmobileUtilView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContribuentiImmobileUtilView));

        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public ContribuentiImmobileUtilView()
		{
			this.TableName = "viewContribuentiImmobileUtil";
		}

		/// <summary>
		/// Torna una DataTable valorizzata con l'elenco dei contribuenti filtrati per immobile.
		/// </summary>
		/// <param name="partitaCatastale"></param>
		/// <param name="foglio"></param>
		/// <param name="numero"></param>
		/// <param name="subalterno"></param>
		/// <param name="sezione"></param>
		/// <param name="caratteristica"></param>
		/// <param name="anno"></param>
		/// <param name="protocollo"></param>
		/// <param name="codVia"></param>
		/// <param name="categoria"></param>
		/// <param name="classe"></param>
		/// <param name="ente"></param>
		/// <param name="effettivo"></param>
		/// <returns></returns>
		public DataTable ListContribuenti(string partitaCatastale, string foglio, string numero, string subalterno,
			string sezione, string caratteristica, string anno, string protocollo, string codVia,
			string categoria, string classe, string ente, Effettivo effettivo)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE (PartitaCatastale LIKE @partitaCatastale) AND (Foglio LIKE @foglio)" +
				" AND (Numero LIKE @numero) AND (Subalterno LIKE @subalterno) AND (Sezione LIKE @sezione)" +
				" AND (Caratteristica LIKE @caratteristica) AND (AnnoDenunciaCat LIKE @anno)" +
				" AND (NumeroProtocolloCat LIKE @protocollo) AND (CodVia LIKE @codVia) AND" +
				" (Categoria LIKE @categoria) AND (Classe LIKE @classe) AND (Ente LIKE @ente)";

			switch(effettivo)
			{
				case Effettivo.Effettivi:
					SelectCommand.CommandText += " AND Effettivo=1";
					break;

				case Effettivo.NonEffettivi:
					SelectCommand.CommandText += " AND Effettivo=0";
					break;
			}

			SelectCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale + "%";
			SelectCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio + "%";
			SelectCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero + "%";
			SelectCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno + "%";
			SelectCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione + "%";
			SelectCommand.Parameters.Add("@caratteristica", SqlDbType.VarChar).Value = caratteristica + "%";
			SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";
			SelectCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo + "%";
			SelectCommand.Parameters.Add("@codVia", SqlDbType.VarChar).Value = codVia + "%";
			SelectCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria + "%";
			SelectCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe + "%";
			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";

			DataTable dt=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ContribuentiImmoibleUtilView.ListContribuenti.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna una DataView valorizzata col elenco degli immobili filtrati per
		/// contribuente.
		/// </summary>
		/// <param name="idContribuente"></param>
		/// <param name="effettivo"></param>
		/// <returns></returns>
		public  DataView List(int idContribuente, Effettivo effettivo)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDContribuente=@idContribuente";
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

			SelectCommand.Parameters.Add("@idContribuente", SqlDbType.Int).Value = idContribuente;
			DataView dv=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView ;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ContribuentiImmoibleUtilView.List.errore: ", Err);
                throw Err;
            }
        }
	}
}
