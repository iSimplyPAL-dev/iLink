using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{    
	/// <summary>
	/// Rappresenta una riga della tabella TblProvenienze.
	/// </summary>
	public struct ProvenienzeRow
    {
        /// 
        public int ID;
        ///    
        public string Codice;
        ///        
        public string Tributo;
        ///       
        public string Tipologia;
        ///       
        public string Descrizione;
	}
    /// <summary>
    /// 
    /// </summary>
	public enum TipologieProvenienza
    {
        /// 
        dichiarazioni,
        /// 
             versamenti
    }

    /// <summary>
    /// Classe di gestione della tabella TblProvenienze.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ProvenienzeTable : Database 
	{     
		private string _username;
        /// 
        public const int Dichiarazione_FITTIZIA = 1;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProvenienzeTable));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        public ProvenienzeTable(string UserName) 
		{
			this._username = UserName;
			this.TableName= "TblProvenienze";
		}
        
		/// <summary>
		/// Ritorna un valore che indica se è avvenuto l'inserimento di un nuovo elemento.
		/// </summary>
		/// <param name="codice"></param>
		/// <param name="tributo"></param>
		/// <param name="tipologia"></param>
		/// <param name="descrizione"></param>
		/// <returns></returns>
		public bool Insert(string codice, string tributo, string tipologia, string descrizione) 
		{
			SqlCommand InsertCommand = new SqlCommand();
            try { 
			InsertCommand.CommandText = "Insert Into TblProvenienze (Codice, Tributo, Tipologia, Descrizione) Values (@codice, @tributo, @tipologia, @descrizione)";
			InsertCommand.Parameters.Add("@codice", SqlDbType.VarChar).Value = codice;
			InsertCommand.Parameters.Add("@tributo", SqlDbType.VarChar).Value = tributo;
			InsertCommand.Parameters.Add("@tipologia", SqlDbType.VarChar).Value = tipologia;
			InsertCommand.Parameters.Add("@descrizione", SqlDbType.VarChar).Value = descrizione;
            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.Insert.errore: ", Err);
                throw Err;
            }

        }

        /// <summary>
        /// Ritorna un valore che indica se è avvenuto l'inserimento di un nuovo elemento.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Insert(ProvenienzeRow item) 
		{
			return Insert(item.Codice, item.Tributo, item.Tipologia, item.Descrizione);
		}
        
		/// <summary>
		/// Ritorna un valore che indica se è avvenuto la modifica di un elemento.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="codice"></param>
		/// <param name="tributo"></param>
		/// <param name="tipologia"></param>
		/// <param name="descrizione"></param>
		/// <returns></returns>
		public bool Modify(int id, string codice, string tributo, string tipologia, string descrizione) 
		{
			SqlCommand UpdateCommand = new SqlCommand();
            try { 
			UpdateCommand.CommandText = "Update TblProvenienze Set Codice=@codice, Tributo=@tributo, Tipologia=@tipologia, Descrizione=@descrizione  Where ID=@id ";
			UpdateCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
			UpdateCommand.Parameters.Add("@codice", SqlDbType.VarChar).Value = codice;
			UpdateCommand.Parameters.Add("@tributo", SqlDbType.VarChar).Value = tributo;
			UpdateCommand.Parameters.Add("@tipologia", SqlDbType.VarChar).Value = tipologia;
			UpdateCommand.Parameters.Add("@descrizione", SqlDbType.VarChar).Value = descrizione;
            return Execute(UpdateCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.Modify.errore: ", Err);
                throw Err;
            }
        }
        
		/// <summary>
		/// Ritorna un valore che indica se è avvenuta la modifica di un elemento.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Modify(ProvenienzeRow item) 
		{
			return Modify(item.ID, item.Codice, item.Tributo, item.Tipologia, item.Descrizione);
		}
        
		/// <summary>
		/// Ritorna una riga identificata dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ProvenienzeRow GetRow(int id) 
		{
			ProvenienzeRow riga = new ProvenienzeRow();
			SqlCommand SelectCommand = PrepareGetRow(id);
            DataTable tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
            try { 
			if (tabella.Rows.Count > 0) 
			{
				riga.ID = (System.Int32)tabella.Rows[0]["ID"];
				riga.Codice = (System.String)tabella.Rows[0]["Codice"];
				riga.Tributo = (System.String)tabella.Rows[0]["Tributo"];
				riga.Tipologia = (System.String)tabella.Rows[0]["Tipologia"];
				riga.Descrizione = (System.String)tabella.Rows[0]["Descrizione"];
			}
			return riga;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.GetRow.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna una dataview con la lista delle provenienze filtrate per tipologia.
		/// </summary>
		/// <param name="tipologia"></param>
		/// <param name="tributo"></param>
        /// <param name="IsFromRicerca"></param>
		/// <returns></returns>
        public DataView List(TipologieProvenienza tipologia, string tributo, int IsFromRicerca)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            string Tipo = "";
            try { 
            //cmdMyCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE TRIBUTO=@tributo";
            //switch(tipologia)
            //{
            //    case TipologieProvenienza.dichiarazioni:
            //        cmdMyCommand.CommandText += " AND TIPOLOGIA <> 'V'";
            //        break;
            //    case TipologieProvenienza.versamenti:
            //        cmdMyCommand.CommandText += " AND TIPOLOGIA <> 'D'";
            //        break;
            //}
            //cmdMyCommand.CommandText += "ORDER BY DESCRIZIONE";
            cmdMyCommand.CommandType = CommandType.StoredProcedure;
            cmdMyCommand.CommandText = "prc_GetProvenienze";
            switch (tipologia)
            {
                case TipologieProvenienza.dichiarazioni:
                    Tipo = "D";
                    break;
                case TipologieProvenienza.versamenti:
                    Tipo = "V";
                    break;
            }
            cmdMyCommand.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = Tipo;
            cmdMyCommand.Parameters.Add("@tributo", SqlDbType.VarChar).Value = tributo;
            cmdMyCommand.Parameters.Add("@FromRicerca", SqlDbType.Int).Value = IsFromRicerca;
            //*** 20140630 - TASI ***
            //DataView dv=Query(cmdMyCommand).DefaultView;
            DataView dv = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
            kill();
            return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.List.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Torna una datatable con la lista delle provenienze filtrate per codice, tributo, tipologia e/o descrizione.
		/// </summary>
		/// <param name="objProvenienze"></param>
        /// <param name="myConn"></param>
		/// <returns></returns>
		public DataTable Ricerca(ProvenienzeRow objProvenienze, string myConn)
		{
			SqlCommand SelectCommand = new SqlCommand();

			SelectCommand.CommandText = "Select * from " + this.TableName + " where 1 = 1 ";
            try { 
			if (objProvenienze.Codice.ToString() != "")
			{
				SelectCommand.CommandText += " AND Codice = @Codice";
				SelectCommand.Parameters.Add("@Codice", SqlDbType.Int).Value = objProvenienze.Codice;
			}
			if (objProvenienze.Descrizione.ToString() != "")
			{
				SelectCommand.CommandText += " AND Descrizione like @Descrizione";
				SelectCommand.Parameters.Add("@Descrizione", SqlDbType.VarChar).Value = objProvenienze.Descrizione + "%";
			}

			if (objProvenienze.Tipologia.ToString() != "-1")
			{
				SelectCommand.CommandText += " AND Tipologia = @Tipologia";
				SelectCommand.Parameters.Add("@Tipologia", SqlDbType.VarChar).Value = objProvenienze.Tipologia;
			}
			if (objProvenienze.Tributo.ToString() !="-1")
			{
				SelectCommand.CommandText += " AND Tributo = @Tributo";
				SelectCommand.Parameters.Add("@Tributo", SqlDbType.VarChar).Value = objProvenienze.Tributo;
			}

			SelectCommand.CommandText += " order by codice ";

			DataTable dt=Query(SelectCommand,new SqlConnection(myConn));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.Ricerca.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna una dataview con la lista di tutte le provenienze presenti in tabella
		/// </summary>
		/// <returns></returns>
		public DataTable RicercaNoParametri()
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "Select * from " + this.TableName ;

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.RicercaNoParametri.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna una dataview con la lista di tutte le provenienze filtrate per codice
		/// </summary>
		/// <param name="Codice"></param>
		/// <returns></returns>
		public DataTable RicercaPercodice(string Codice)
		{
			SqlCommand SelectCommand = new SqlCommand();

			SelectCommand.CommandText = "Select * from " + this.TableName + " where 1=1";
            try { 
			if (Codice.ToString() != "")
			{
				SelectCommand.CommandText += " AND Codice = @Codice";
				SelectCommand.Parameters.Add("@Codice", SqlDbType.Int).Value = Codice;
			}

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ProvenienzeTable.RicercaPercodice.errore: ", Err);
                throw Err;
            }
        }
	}
    //*** 201511 - Funzioni Sovracomunali ***
    /// <summary>
    /// 
    /// </summary>
    public class Enti : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Enti));
        /// <summary>
        /// 
        /// </summary>
        public Enti()
        {
        }

        /// <summary>
        /// Torna una dataview con la lista degli Enti.
        /// </summary>
        /// <returns></returns>
        public new DataView List()
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            try { 
            cmdMyCommand.CommandType = CommandType.StoredProcedure;
            cmdMyCommand.CommandText = "ENTI_S";
            cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.NVarChar).Value = Business.ConstWrapper.Ambiente;
            DataView dv = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnectionOPENgov)).DefaultView;
            kill();
            return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Enti.List.errore: ", Err);
                throw Err;
            }
        }
    }
    //*** ***
}