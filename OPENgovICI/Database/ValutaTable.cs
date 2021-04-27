using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblValuta.
	/// </summary>
	public struct ValutaRow
    {
        /// 
		public int ID;
        /// 
		public string Descrizione;
	}

	/// <summary>
	/// Classe di gestione della tabella TblValuta.
	/// </summary>
	public class ValutaTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ValutaTable));
        private string _username;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
		public ValutaTable(string userName)
		{
			this._username = userName;
			this.TableName = "TblValuta";
		}

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="descrizione"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Insert(string descrizione)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try { 
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Descrizione) " +
				"VALUES (@descrizione)";

			InsertCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;
            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.Insert.errore: ", Err);
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
        public bool Insert(ValutaRow Item)
		{
			return Insert(Item.Descrizione);
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="descrizione"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Modify(int id, string descrizione)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Descrizione=@descrizione " +
				"WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;
            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Modify(ValutaRow Item)
		{
			return Modify(Item.ID, Item.Descrizione);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Restituisce un oggetto di tipo ValutaRow</returns>
		public ValutaRow GetRow(int id)
		{
			ValutaRow Valuta = new ValutaRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Valute = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Valute.Rows.Count > 0)
				{
					Valuta.ID = (int)Valute.Rows[0]["ID"];
					Valuta.Descrizione = (string)Valute.Rows[0]["Descrizione"];
				}
			}
			catch(Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetRow.errore: ", Err);

                kill();
				Valuta = new ValutaRow();
			}
			finally{
				kill();
			}
			return Valuta;
		}
	}
}
