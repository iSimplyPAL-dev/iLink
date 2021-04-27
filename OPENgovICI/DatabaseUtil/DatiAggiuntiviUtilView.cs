using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione della vista viewVersamentiUtil.
	/// </summary>
	public class DatiAggiuntiviUtilview : Database
	{
		/// <summary>
		/// Costruttore della classe
		/// </summary>
		public DatiAggiuntiviUtilview()
		{
			this.TableName = "Dati_Aggiuntivi";
		}

		/// <summary>
		/// Torna una DataView valorizzata col elenco dei versamenti filtrati per ente, anno di
		/// riferimento, cognome, nome, codice fiscale e partita IVA.
		/// </summary>
		/// <param name="IDTestata"></param>
		/// <param name="codente"></param>
		/// <returns></returns>
		public DataTable ListDatiAggiuntivi(int IDTestata, string codente)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM DATI_AGGIUNTIVI WHERE ENTE=@codente AND IdTestata=@idtestata";

			SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = codente;
			SelectCommand.Parameters.Add("@idtestata",SqlDbType.Int).Value = IDTestata;
			DataTable dt=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
		}
	}
}