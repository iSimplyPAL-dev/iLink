using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{

	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblCategoriaCatastale.
	/// </summary>
	public struct CaratteristicaRow
	{
		public string CodCaratteristica;
		public string DescrizioneBreve;
		public string DescrizioneEstesa;
	}


    /// <summary>
    /// Classe di gestione della tabella TblCategoriaCatastale.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class CaratteristicaTable : Database
	{
		private string _username;

		/// <summary>
		/// Costruttore della classe
		/// </summary>
		/// <param name="UserName"></param>
		public CaratteristicaTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TAB_CARATTERISTICA";
		}
		
	}
}