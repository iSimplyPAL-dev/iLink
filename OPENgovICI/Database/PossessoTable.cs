using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
    //*** 20140509 - TASI ***
    /// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblTipoUtilizzo.
	/// </summary>
	public struct UtilizzoRow
	{
        /// 
        public int IdTipo;
        /// 
		public string Descrizione;
	}
    /// <summary>
    /// Struttura che rappresenta il singolo record della tabella TblTipoPossesso.
    /// </summary>
    public struct PossessoRow
    {
        /// 
        public int IdTipo;
        /// 
        public string Descrizione;
    }

    /// <summary>
    /// Classe di gestione della table TblTipoUtilizzo.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class UtilizzoTable : Database
	{
		private string _username;
        private static readonly ILog log = LogManager.GetLogger(typeof(UtilizzoTable));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        public UtilizzoTable(string UserName)
		{
			this._username = UserName;
            //this.TableName = "TblPossesso";
            //this.ProcedureName = "prc_TBLPOSSESSO_S";
            this.TableName = "TBLTIPOUTILIZZO";
            this.ProcedureName = "prc_TBLTIPOUTILIZZO_S";
        }

        ///// <summary>
        ///// Inserisce un nuovo record a partire dai singoli campi.
        ///// </summary>
        ///// <param name="ente"></param>
        ///// <param name="tipoPossesso"></param>
        ///// <param name="descrizione"></param>
        ///// <returns></returns>
        //public bool Insert(string ente, int tipoPossesso, string descrizione)
        //{
        //    SqlCommand InsertCommand = new SqlCommand();
        // try{
        //    InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, TipoPossesso, " +
        //        "Descrizione) VALUES (@ente, @tipoPossesso, @descrizione)";

        //    InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
        //    InsertCommand.Parameters.Add("@tipoPossesso",SqlDbType.Int).Value = tipoPossesso;
        //    InsertCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

        //    return Execute(InsertCommand);
        // }
        //     catch (Exception Err)
        //   {
        //  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.UtilizzoTable.Insert.errore: ", Err);
        //   }

        //}

        ///// <summary>
        ///// Inserisce un nuovo record a partire da una struttura row.
        ///// </summary>
        ///// <param name="Item"></param>
        ///// <returns></returns>
        //public bool Insert(UtilizzoRow Item)
        //{
        //    return Insert(Item.Ente, Item.IdTipo, Item.Descrizione);
        //}

        ///// <summary>
        ///// Aggiorna un record individuato dall'identity a partire dai singoli campi.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="ente"></param>
        ///// <param name="tipoPossesso"></param>
        ///// <param name="descrizione"></param>
        ///// <returns></returns>
        //public bool Modify(int id, string ente, int tipoPossesso, string descrizione)
        //{
        //    SqlCommand ModifyCommand = new SqlCommand();
        //try{
        //    ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, " +
        //        "TipoPossesso=@tipoPossesso, Descrizione=@descrizione WHERE ID=@id";

        //    ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
        //    ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
        //    ModifyCommand.Parameters.Add("@tipoPossesso",SqlDbType.Int).Value = tipoPossesso;
        //    ModifyCommand.Parameters.Add("@descrizione",SqlDbType.VarChar).Value = descrizione;

        //    return Execute(ModifyCommand);
        //  }
        //     catch (Exception Err)
        //   {
        //  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.UtilizzoTable.Modify.errore: ", Err);
        //   }
        //}

        ///// <summary>
        ///// Aggiorna un record individuato dall'identity a partire da una struttura row.
        ///// </summary>
        ///// <param name="Item"></param>
        ///// <returns></returns>
        //public bool Modify(UtilizzoRow Item)
        //{
        //    return Modify(Item.ID, Item.Ente, Item.IdTipo, Item.Descrizione);
        //}

        ///// <summary>
        ///// Ritorna una struttura row che rappresenta un record individuato dall'identity.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public UtilizzoRow GetRow(int id)
        //{
        //    UtilizzoRow myRow = new UtilizzoRow();
        //    try
        //    {
        //        SqlCommand SelectCommand = PrepareGetRow(id);
        //        DataTable Possession = Query(SelectCommand);
        //        if(Possession.Rows.Count > 0)
        //        {
        //            myRow.ID = (int)Possession.Rows[0]["ID"];
        //            myRow.Ente = (string)Possession.Rows[0]["Ente"];
        //            //*** 20140509 - TASI ***
        //            //Possesso.TipoPossesso = (int)Possession.Rows[0]["TipoPossesso"];
        //            myRow.IdTipo = (int)Possession.Rows[0]["IdTipoUtilizzo"];
        //            //*** ***
        //            myRow.Descrizione = (string)Possession.Rows[0]["Descrizione"];
        //        }
        //    }
        //    catch(Exception Err)
        //    {
        ////  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.UtilizzoTable.GetRow.errore: ", Err);
        //        kill();
        //        myRow = new UtilizzoRow();
        //    }
        //    finally{
        //        kill();
        //    }
        //    return myRow;
        //    }
    }

    /// <summary>
    /// Classe di gestione della table TblPossesso.
    /// </summary>
    public class PossessoTable : Database
    {
        private string _username;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        public PossessoTable(string UserName)
        {
            this._username = UserName;
            this.TableName = "TBLTIPOPOSSESSO";
            this.ProcedureName = "prc_TBLTIPOPOSSESSO_S";
        }
    }
    //*** ***

	}
