using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblDichiarazioniDaBonificare.
	/// </summary>
	public struct DichiarazioniDaBonificareRow
    {
        /// 
		public int ID;
        /// 
		public int IdDichiarazione;
        /// 
		public int IdErrore;
        /// 
		public int IDTestata;
        /// 
		public int IDDettaglioTestata;
        /// 
		public int IDOggetto;
	}

    /// <summary>
    /// Classe di gestione della tabella TblDichiarazioniDaBonificare.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class DichiarazioniDaBonificareTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(DichiarazioniDaBonificareTable));
        private string _username;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserName"></param>
		public DichiarazioniDaBonificareTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblDichiarazioniDaBonificare";
		}

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <param name="idErrore"></param>
		/// <param name="idTestata"></param>
		/// <param name="idDettaglioTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public bool Insert(int idDichiarazione, int idErrore, int idTestata, int idDettaglioTestata, int idOggetto)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try { 
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (IdDichiarazione, " +
				"IdErrore, IDTestata, IDDettaglioTestata, IDOggetto) VALUES " +
				"(@idDichiarazione, @idErrore, @idTestata, @idDettaglioTestata, @idOggetto)";

			InsertCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
			InsertCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;
			InsertCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata == 0 ? DBNull.Value : (object)idTestata;
			InsertCommand.Parameters.Add("@idDettaglioTestata",SqlDbType.Int).Value = idDettaglioTestata == 0 ? DBNull.Value : (object)idDettaglioTestata;
			InsertCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto == 0 ? DBNull.Value : (object)idOggetto;

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniDaBonificareTable.Insert.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Insert(DichiarazioniDaBonificareRow Item)
		{
			return Insert(Item.IdDichiarazione, Item.IdErrore, Item.IDTestata, Item.IDDettaglioTestata, Item.IDOggetto);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="idDichiarazione"></param>
		/// <param name="idErrore"></param>
		/// <param name="idTestata"></param>
		/// <param name="idDettaglioTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public bool Modify(int id, int idDichiarazione, int idErrore, int idTestata, int idDettaglioTestata, int idOggetto)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try {
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET IdDichiarazione=@idDichiarazione, " +
				"IdErrore=@idErrore, IDTestata=@idTestata, IDDettaglioTestata=@idDettaglioTestata, " +
				"IDOggetto=@idOggetto WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
			ModifyCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;
			ModifyCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata == 0 ? DBNull.Value : (object)idTestata;
			ModifyCommand.Parameters.Add("@idDettaglioTestata",SqlDbType.Int).Value = idDettaglioTestata == 0 ? DBNull.Value : (object)idDettaglioTestata;
			ModifyCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto == 0 ? DBNull.Value : (object)idOggetto;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniDaBonificareTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Modify(DichiarazioniDaBonificareRow Item)
		{
			return Modify(Item.ID, Item.IdDichiarazione, Item.IdErrore, Item.IDTestata, Item.IDDettaglioTestata, Item.IDOggetto);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DichiarazioniDaBonificareRow GetRow(int id)
		{
			DichiarazioniDaBonificareRow Dichiarazione = new DichiarazioniDaBonificareRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Dichiarazioni = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Dichiarazioni.Rows.Count > 0)
				{
					Dichiarazione.ID = (int)Dichiarazioni.Rows[0]["ID"];
					Dichiarazione.IdDichiarazione = (int)Dichiarazioni.Rows[0]["IdDichiarazione"];
					Dichiarazione.IdErrore = (int)Dichiarazioni.Rows[0]["IdErrore"];
					Dichiarazione.IDTestata = Dichiarazioni.Rows[0]["IDTestata"] == DBNull.Value ? 0 : (int)Dichiarazioni.Rows[0]["IDTestata"];
					Dichiarazione.IDDettaglioTestata = Dichiarazioni.Rows[0]["IDDettaglioTestata"] == DBNull.Value ? 0 : (int)Dichiarazioni.Rows[0]["IDDettaglioTestata"];
					Dichiarazione.IDOggetto = Dichiarazioni.Rows[0]["IDOggetto"] == DBNull.Value ? 0 : (int)Dichiarazioni.Rows[0]["IDOggetto"];
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniDaBonificareTable.GetRow.errore: ", Err);
                kill();
				Dichiarazione = new DichiarazioniDaBonificareRow();
			}
			finally{
				kill();
			}
			return Dichiarazione;
		}

		/// <summary>
		/// Esegue l'eliminazione dei record identificati dall' id della dichiarazione.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <returns></returns>
		public bool DeleteByIDDichiarazione(int idDichiarazione)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IdDichiarazione=@idDichiarazione";

			DeleteCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniDaBonificareTable.DeleteByIDDichiarazione.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Esegue l'eliminazione dei record identificati dall' id della dichiarazione e dall'id Immobile.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <param name="IdOggetto"></param>
		/// <returns></returns>
		public bool DeleteByIDDichiarazione(int idDichiarazione, int IdOggetto)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IdDichiarazione=@idDichiarazione and IDOggetto=@IdOggetto";

			DeleteCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
			DeleteCommand.Parameters.Add("@IdOggetto",SqlDbType.Int).Value = IdOggetto;
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniDaBonificareTable.DeleteByIDDichiarazione.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Esegue l'eliminazione dei record identificati dall' id della dichiarazione e dall'IDDettaglioTestata.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <param name="IdOggetto"></param>
		/// <param name="IDDettaglioTestata"></param>
		/// <returns></returns>
		public bool DeleteByIDDichiarazione(int idDichiarazione,int IdOggetto,  int IDDettaglioTestata)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IdDichiarazione=@idDichiarazione and IDDettaglioTestata=@IDDettaglioTestata";

			DeleteCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
			DeleteCommand.Parameters.Add("@IDDettaglioTestata",SqlDbType.Int).Value = IDDettaglioTestata;
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniDaBonificareTable.DeleteByIDDichiarazione.errore: ", Err);
                throw Err;
            }
        }

	}
}
