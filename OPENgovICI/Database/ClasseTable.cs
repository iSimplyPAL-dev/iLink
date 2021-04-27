using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblClasse.
	/// </summary>
	public struct ClasseRow
	{
		/// <summary>
		/// Parametro di tipo <c>int</c> che identifica l'Identity
		/// </summary>
		public int ID;
		/// <summary>
		/// Parametro di tipo <c>String</c> che identifica l'Ente
		/// </summary>
		public string Ente;
		/// <summary>
		/// Parametro di tipo <c>String</c> che identifica la Classe
		/// </summary>
		public string Classe;
		/// <summary>
		/// Parametro di tipo <c>String</c> che identifica la Descrizione
		/// </summary>
		public string Descrizione;
	}
    /// <summary>
    /// Classe di gestione della tabella TblClasse.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ClasseTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ClasseTable));
        private string _username;

		/// <summary>
		/// Costruttore della classe
		/// </summary>
		/// <param name="UserName">Parametro di tipo <c>String</c> che identifica l'utente</param>
		public ClasseTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblClasse";
            this.ProcedureName = "prc_TBLCLASSE_S";
        }

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="ente">Parametro di tipo <c>String</c> che identifica l'Ente</param>
		/// <param name="classe">Parametro di tipo <c>String</c> che identifica la Classe</param>
		/// <param name="descrizione">Parametro di tipo <c>String</c> che identifica la Descrizione</param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Insert(string ente, string classe, string descrizione)
		{
			SqlCommand InsertCommand = new SqlCommand();
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, Classe, " +
				"Descrizione) VALUES (@ente, @classe, @descrizione)";

			InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			InsertCommand.Parameters.Add("@classe",SqlDbType.VarChar).Value = classe;
			InsertCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item">Parametro di tipo ClasseRow</param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Insert(ClasseRow Item)
		{
			return Insert(Item.Ente, Item.Classe, Item.Descrizione);
		}

		/// <summary>
		/// Effettua la modifica di un record identificato dall'identity
		/// </summary>
		/// <param name="id">Parametro di tipo <c>int</c> che rappresenta l'Identity</param>
		/// <param name="ente">Parametro di tipo <c>String</c> che identifica l'Ente</param>
		/// <param name="classe">Parametro di tipo <c>String</c> che identifica la Classe</param>
		/// <param name="descrizione">Parametro di tipo <c>String</c> che identifica la Descrizione</param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Modify(int id, string ente, string classe, string descrizione)
		{
			SqlCommand ModifyCommand = new SqlCommand();
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, " +
				"Classe=@classe, Descrizione=@descrizione WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			ModifyCommand.Parameters.Add("@classe",SqlDbType.VarChar).Value = classe;
			ModifyCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="Item">Parametro di tipo ClasseRow</param>
		/// <returns>La funzione restituisce un valore <c>bool</c> 
		/// <list type="bullet">
		/// <item ><c>true</c> se l'operazione è andata a buon fine</item>
		/// <item ><c>false</c> se si sono veriricati degli errori</item>
		/// </list>
		/// </returns>
		public bool Modify(ClasseRow Item)
		{
			return Modify(Item.ID, Item.Ente, Item.Classe, Item.Descrizione);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id">Parametro di tipo <c>int</c> che rappresenta l'Identity</param>
		/// <returns>Restituisce un oggetto di tipo ClasseRow</returns>
		public ClasseRow GetRow(int id)
		{
			ClasseRow Classe = new ClasseRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Classi = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Classi.Rows.Count > 0)
				{
					Classe.ID = (int)Classi.Rows[0]["ID"];
					Classe.Ente = (string)Classi.Rows[0]["Ente"];
					Classe.Classe = (string)Classi.Rows[0]["Classe"];
					Classe.Descrizione = (string)Classi.Rows[0]["Descrizione"];
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClasseTable.GetRow.errore: ", Err);
                kill();
				Classe = new ClasseRow();
			}
			finally{
				kill();
			}
			return Classe;
		}
	}
}
