using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella Tipo_Rendita.
	/// </summary>
	public struct Tipo_RenditaRow
    {
        /// 
		public string COD_ENTE;
        /// 
		public string COD_RENDITA;
        /// 
		public string SIGLA;
        /// 
		public string DESCRIZIONE;
	}

    /// <summary>
    /// Classe di gestione della tabella Tipo_Rendita.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class Tipo_RenditaTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Tipo_RenditaTable));

        private string _username;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
		public Tipo_RenditaTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TIPO_RENDITA";
		}
	}
}
