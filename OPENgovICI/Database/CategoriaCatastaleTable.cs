using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblCategoriaCatastale.
	/// Contiene quattro elementi
	/// <list type="bullet">
	/// <item>ID di tipo <c>int</c></item> 
	/// <item>Ente di tipo <c>string</c></item> 
	/// <item>CategoriaCatastale di tipo <c>string</c></item> 
	/// <item>Descrizione di tipo <c>string</c></item> 
	/// </list>
	/// </summary>
	public struct CategoriaCatastaleRow
	{
		public int ID;
		public string Ente;
		public string CategoriaCatastale;
		public string Descrizione;
	}
    /// <summary>
    /// Classe di gestione della tabella TblCategoriaCatastale.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class CategoriaCatastaleTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(CategoriaCatastaleTable));
        private string _username;

		/// <summary>
		/// Costruttore della classe
		/// </summary>
		/// <param name="UserName"></param>
		public CategoriaCatastaleTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblCategoriaCatastale";
            this.ProcedureName = "prc_TBLCATEGORIACATASTALE_S";
		}
        /// <summary>
        /// Elenco categorie
        /// </summary>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataView ListaCategorie(string Gruppo)
        {
            DataView dvDati = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TBLCATEGORIACATASTALE_S","GRUPPO");
                    dvDati =ctx.GetDataView(sSQL, "TBL", ctx.GetParam("GRUPPO", Gruppo));
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategoriaCatastaleTable.ListaCategorie.errore: ", ex);
                dvDati = null;
            }
            return dvDati;
        }

        /*/// <summary>
        /// Inserisce un nuovo record a partire dai singoli campi.
        /// </summary>
        /// <param name="ente">Paramentro di tipo <c>string</c> che determina l'ente</param>
        /// <param name="categoriaCatastale">Paramentro di tipo <c>string</c> che determina la categoria catastale</param>
        /// <param name="descrizione">Paramentro di tipo <c>string</c> che determina la descrizione della categoria catastale</param>
        /// <returns>La funzione restituisce un valore <c>bool</c> 
        /// <list type="bullet">
        /// <item ><c>true</c> se l'operazione è andata a buon fine</item>
        /// <item ><c>false</c> se si sono veriricati degli errori</item>
        /// </list>
        /// </returns>
        public bool Insert(string ente, string categoriaCatastale, string descrizione)
		{
			SqlCommand InsertCommand = new SqlCommand();
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, CategoriaCatastale, " +
				"Descrizione) VALUES (@ente, @categoriaCatastale, @descrizione)";

			InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			InsertCommand.Parameters.Add("@categoriaCatastale",SqlDbType.VarChar).Value = categoriaCatastale;
			InsertCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item">Parametro di tipo <see cref="CategoriaCatastaleRow"/></param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Insert(CategoriaCatastaleRow Item)
		{
			return Insert(Item.Ente, Item.CategoriaCatastale, Item.Descrizione);
		}

		/// <summary>
		/// Modifica un record esistente nella tabella "TblCategoriaCatastale"
		/// </summary>
		/// <param name="id">Parametro di tipo <c>int</c>. Identity della Categoria Catastale</param>
		/// <param name="ente">Parametro di tipo <c>string</c>. Codice dell'ente al quale collegare la Categoria Catastale</param>
		/// <param name="categoriaCatastale">Parametro di tipo <c>string</c>. Categoria Catastale</param>
		/// <param name="descrizione">Parametro di tipo <c>string</c>. Descrizione estesa della Categoria Catastale</param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Modify(int id, string ente, string categoriaCatastale, string descrizione)
		{
			SqlCommand ModifyCommand = new SqlCommand();
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, " +
				"CategoriaCatastale=@categoriaCatastale, Descrizione=@descrizione WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			ModifyCommand.Parameters.Add("@categoriaCatastale",SqlDbType.VarChar).Value = categoriaCatastale;
			ModifyCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="Item">Parametro di tipo <see cref="CategoriaCatastaleRow"/></param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Modify(CategoriaCatastaleRow Item)
		{
			return Modify(Item.ID, Item.Ente, Item.CategoriaCatastale, Item.Descrizione);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id">Parametro di tipo <c>int</c>. Identity della Categoria Catastale</param>
		/// <returns>La funzione ritorna una struttura di tipo <see cref="CategoriaCatastaleRow"/> valorizzata nei suoi elementi</returns>
		public CategoriaCatastaleRow GetRow(int id)
		{
			CategoriaCatastaleRow Categoria = new CategoriaCatastaleRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Categorie = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Categorie.Rows.Count > 0)
				{
					Categoria.ID = (int)Categorie.Rows[0]["ID"];
					Categoria.Ente = (string)Categorie.Rows[0]["Ente"];
					Categoria.CategoriaCatastale = (string)Categorie.Rows[0]["CategoriaCatastale"];
					Categoria.Descrizione = (string)Categorie.Rows[0]["Descrizione"];
				}
			}
			catch(Exception Err)
			{

                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategoriaCatastaleTable.GetRow.errore: ", Err);
                kill();
				Categoria = new CategoriaCatastaleRow();
			}
			finally{
				kill();
			}
			return Categoria;
		}*/
	}
}
