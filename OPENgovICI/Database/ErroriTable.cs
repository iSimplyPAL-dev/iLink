using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblErrori.
	/// </summary>
	public struct ErroriRow
	{
		public int ID;
		public string Descrizione;
		public bool Warning;
	}

	/// <summary>
	/// Classe di gestione della tabella TblErrori.
	/// </summary>
	public class ErroriTable : Database
	{
		private string _username;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ErroriTable));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        public ErroriTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblErrori";
		}

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="descrizione"></param>
		/// <param name="warning"></param>
		/// <returns></returns>
		public bool Insert(string descrizione, bool warning)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try { 
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Descrizione, Warning) " +
				"VALUES (@descrizione, @warning)";

			InsertCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;
			InsertCommand.Parameters.Add("@warning",SqlDbType.Bit).Value = warning;

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ErroriTable.Insert.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Insert(ErroriRow Item)
		{
			return Insert(Item.Descrizione, Item.Warning);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="descrizione"></param>
		/// <param name="warning"></param>
		/// <returns></returns>
		public bool Modify(int id, string descrizione, bool warning)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Descrizione=@descrizione," +
				" Warning=@warning WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;
			ModifyCommand.Parameters.Add("@warning",SqlDbType.Bit).Value = warning;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ErroriTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Modify(ErroriRow Item)
		{
			return Modify(Item.ID, Item.Descrizione, Item.Warning);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ErroriRow GetRow(int id)
		{
			ErroriRow Errore = new ErroriRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Errori = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Errori.Rows.Count > 0)
				{
					Errore.ID = (int)Errori.Rows[0]["ID"];
					Errore.Descrizione = (string)Errori.Rows[0]["Descrizione"];
					Errore.Warning = (bool)Errori.Rows[0]["Warning"];
				}
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ErroriTable.GetRow.errore: ", Err);
                kill();
				Errore = new ErroriRow();
			}
			finally{
				kill();
			}
			return Errore;
		}
	}
}
