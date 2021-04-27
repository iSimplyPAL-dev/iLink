using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblTipoImmobile.
	/// </summary>
	public struct TipoImmobileRow
    {
        /// 
		public int ID;
        /// 
		public string Ente;
        /// 
		public int TipoImmobile;
        /// 
		public string Descrizione;
	}

    /// <summary>
    /// Classe di gestione della tabella TblTipoImmobile.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class TipoImmobileTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoImmobileTable));

        private string _username;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserName"></param>
		public TipoImmobileTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblTipoImmobile";
		}

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="tipoImmobile"></param>
		/// <param name="descrizione"></param>
		/// <returns></returns>
		public bool Insert(string ente, int tipoImmobile, string descrizione)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try { 
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, TipoImmobile, " +
				"Descrizione) VALUES (@ente, @tipoImmobile, @descrizione)";

			InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			InsertCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = tipoImmobile;
			InsertCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoImmobileTable.Insert.errore: ", Err);
                throw Err;
            }

        }

        /// <summary>
        /// Inserisce un nuovo record a partire da una struttura row.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Insert(TipoImmobileRow Item)
		{
			return Insert(Item.Ente, Item.TipoImmobile, Item.Descrizione);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="ente"></param>
		/// <param name="tipoImmobile"></param>
		/// <param name="descrizione"></param>
		/// <returns></returns>
		public bool Modify(int id, string ente, int tipoImmobile, string descrizione)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, " +
				"TipoImmobile=@tipoImmobile, Descrizione=@descrizione WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			ModifyCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = tipoImmobile;
			ModifyCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoImmobileTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Modify(TipoImmobileRow Item)
		{
			return Modify(Item.ID, Item.Ente, Item.TipoImmobile, Item.Descrizione);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TipoImmobileRow GetRow(int id)
		{
			TipoImmobileRow Immobile = new TipoImmobileRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Immobili = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Immobili.Rows.Count > 0)
				{
					Immobile.ID = (int)Immobili.Rows[0]["ID"];
					Immobile.Ente = (string)Immobili.Rows[0]["Ente"];
					Immobile.TipoImmobile = (int)Immobili.Rows[0]["TipoImmobile"];
					Immobile.Descrizione = (string)Immobili.Rows[0]["Descrizione"];
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoImmobileTable.GetRow.errore: ", Err);
                Immobile = new TipoImmobileRow();
			}
			return Immobile;
		}
	}
}

