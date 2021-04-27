using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblVersamentiDaBonificare.
	/// </summary>
	public struct VersamentiDaBonificareRow
	{
		public int ID;
		public int IdVersamenti;
		public int IdErrore;
	}
	/// <summary>
	/// Classe di gestione della tabella TblVersamentiDaBonificare.
	/// </summary>
	public class VersamentiDaBonificareTable : Database
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VersamentiDaBonificareTable));


        private string _username;

		public VersamentiDaBonificareTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblVersamentiDaBonificare";
		}

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="idVersamenti"></param>
		/// <param name="idErrore"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Insert(int idVersamenti, int idErrore)
		{
			SqlCommand cmdMyCommand = new SqlCommand();
            try { 
			cmdMyCommand.CommandText = "INSERT INTO " + this.TableName + " (IdVersamenti, " +
				"IdErrore) VALUES (@idVersamenti, @idErrore)";

			cmdMyCommand.Parameters.Add("@idVersamenti",SqlDbType.Int).Value = idVersamenti;
			cmdMyCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;
            //*** 20140630 - TASI ***
			//return Execute(InsertCommand);
            return Execute(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VersamentiDaBonificareTable.Insert.errore: ", Err);
                throw Err;
            }


        }


        /// <summary>
        /// Inserisce un nuovo record a partire da una struttura row.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns>
        /// Restituisce un valore booleano:
        /// true: se l'operazione è terminata correttamente
        /// false: se si sono verificati errori
        /// </returns>
        public bool Insert(VersamentiDaBonificareRow Item)
		{
			return Insert(Item.IdVersamenti, Item.IdErrore);
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="idVersamenti"></param>
		/// <param name="idErrore"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Modify(int id, int idVersamenti, int idErrore)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET IdVersamenti=@idVersamenti, " +
				"IdErrore=@idErrore WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@idVersamenti",SqlDbType.Int).Value = idVersamenti;
			ModifyCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VersamentiDaBonificareTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		
		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori</returns>
		public bool Modify(VersamentiDaBonificareRow Item)
		{
			return Modify(Item.ID, Item.IdVersamenti, Item.IdErrore);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Restituisce un oggetto di tipo VersamentiDaBonificareRow</returns>
		public VersamentiDaBonificareRow GetRow(int id)
		{
			VersamentiDaBonificareRow Versamento = new VersamentiDaBonificareRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Versamenti = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Versamenti.Rows.Count > 0)
				{
					Versamento.ID = (int)Versamenti.Rows[0]["ID"];
					Versamento.IdVersamenti = (int)Versamenti.Rows[0]["IdVersamenti"];
					Versamento.IdErrore = (int)Versamenti.Rows[0]["IdErrore"];
				}
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VersamentiDaBonificareTable.GetRow.errore: ", Err);
                Versamento = new VersamentiDaBonificareRow();
			}
			return Versamento;
		}


		/// <summary>
		/// Esegue l'eliminazione dei record identificati dall'id del versamento.
		/// </summary>
		/// <param name="idVersamento"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool DeleteByIDVersamento(int idVersamento)
		{
			SqlCommand cmdMyCommand = new SqlCommand();
            try { 
			cmdMyCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IDVersamenti=@idVersamento";

			cmdMyCommand.Parameters.Add("@idVersamento", SqlDbType.Int).Value = idVersamento;
            //*** 20140630 - TASI ***
			//return Execute(DeleteCommand);
            return Execute(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VersamentiDaBonificareTable.DeleteByIDVersamento.errore: ", Err);
                throw Err;
            }
        }
	}
}
