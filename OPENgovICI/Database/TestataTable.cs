using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Business;
using System.Collections;
using System.Configuration;
using System.IO;
using log4net;
using log4net.Config;
using Utility;

namespace DichiarazioniICI.Database
{
	/**** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblTestata.
	/// </summary>
	public struct TestataRow
	{
		public int ID;
		public string Ente;
		public int NumeroDichiarazione;
		public string AnnoDichiarazione;
		public string NumeroProtocollo;
		public DateTime DataProtocollo;
		public string TotaleModelli;
		public DateTime DataInizio;
		public DateTime DataFine;
		public int IDContribuente;
		public int IDDenunciante;
		public bool Bonificato;
		public bool Annullato;
		public DateTime DataInizioValidità;
		public DateTime DataFineValidità;
		public string Operatore;
		public bool Storico;
		public int IDQuestionario;
		public int IDProvenienza;
	}
	*/
	/// <summary>
	/// Classe di gestione della tabella TblaTestata.
	/// </summary>
	/// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
	public class TestataTable : Database
	{
		private string _username;
		private static readonly ILog log = LogManager.GetLogger(typeof(ImmobileDettaglio));


		public TestataTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblTestata";
		}
		/**** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="idTestata"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Insert(TestataRow Item, out int idTestata)
		{
			return Insert(Item.Ente, Item.NumeroDichiarazione, Item.AnnoDichiarazione, Item.NumeroProtocollo, 
				Item.DataProtocollo, Item.TotaleModelli, Item.DataInizio, Item.DataFine, Item.IDContribuente, Item.IDDenunciante, 
				Item.Bonificato, Item.Annullato, Item.DataInizioValidità, Item.DataFineValidità, Item.Operatore,
				Item.Storico, Item.IDProvenienza, out idTestata);
		}
		
		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="numeroDichiarazione"></param>
		/// <param name="annoDichiarazione"></param>
		/// <param name="numeroProtocollo"></param>
		/// <param name="dataProtocollo"></param>
		/// <param name="totaleModelli"></param>
		/// <param name="dataInizio"></param>
		/// <param name="dataFine"></param>
		/// <param name="idContribuente"></param>
		/// <param name="idDenunciante"></param>
		/// <param name="bonificato"></param>
		/// <param name="annullato"></param>
		/// <param name="dataInizioValidità"></param>
		/// <param name="dataFineValidità"></param>
		/// <param name="operatore"></param>
		/// <param name="storico"></param>
		/// <param name="idProvenienza"></param>
		/// <param name="idTestata"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>


		public bool Insert( string ente, int numeroDichiarazione, string annoDichiarazione, string numeroProtocollo, 
			DateTime dataProtocollo, string totaleModelli, DateTime dataInizio, DateTime dataFine, int idContribuente, int idDenunciante,
			bool bonificato, bool annullato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore,
			bool storico, int idProvenienza, out int idTestata)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try{
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, NumeroDichiarazione, AnnoDichiarazione, " +
				"NumeroProtocollo, DataProtocollo, TotaleModelli, DataInizio, DataFine, IdContribuente, IdDenunciante, " + 
				"Bonificato, Annullato, DataInizioValidità, DataFineValidità, Operatore, Storico, IDProvenienza) VALUES (@ente, @numeroDichiarazione, " +
				"@annoDichiarazione, @numeroProtocollo, @dataProtocollo, @totaleModelli, @dataInizio, " +
				"@dataFine, @idContribuente, @idDenunciante, @bonificato, @annullato, @dataInizioValidità, @dataFineValidità, " +
				"@operatore, @storico, @idProvenienza)";

			InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			InsertCommand.Parameters.Add("@numeroDichiarazione",SqlDbType.Int).Value = numeroDichiarazione;
			InsertCommand.Parameters.Add("@annoDichiarazione",SqlDbType.VarChar).Value = annoDichiarazione;
			InsertCommand.Parameters.Add("@numeroProtocollo",SqlDbType.VarChar).Value = numeroProtocollo == null ? DBNull.Value : (object)numeroProtocollo;
			InsertCommand.Parameters.Add("@dataProtocollo",SqlDbType.DateTime).Value = dataProtocollo == DateTime.MinValue ? SqlDateTime.Null : (object)dataProtocollo;
			InsertCommand.Parameters.Add("@totaleModelli",SqlDbType.VarChar).Value = totaleModelli == null ? DBNull.Value : (object)totaleModelli;
			InsertCommand.Parameters.Add("@dataInizio",SqlDbType.DateTime).Value = dataInizio == DateTime.MinValue ? DBNull.Value : (object)dataInizio;
			InsertCommand.Parameters.Add("@dataFine",SqlDbType.DateTime).Value = dataFine == DateTime.MinValue ? DBNull.Value : (object)dataFine;
			InsertCommand.Parameters.Add("@idContribuente",SqlDbType.Int).Value = idContribuente == 0 ? DBNull.Value : (object)idContribuente;
			InsertCommand.Parameters.Add("@idDenunciante",SqlDbType.Int).Value = idDenunciante == 0 ? DBNull.Value : (object)idDenunciante;
			InsertCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
			InsertCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
			InsertCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
			InsertCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value :(object)dataFineValidità;
			InsertCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
			InsertCommand.Parameters.Add("@storico",SqlDbType.Bit).Value = storico;
			InsertCommand.Parameters.Add("@idProvenienza", SqlDbType.Int).Value = idProvenienza;
			
			return Execute(InsertCommand,new SqlConnection(ConstWrapper.StringConnection), out idTestata);
               }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.Insert.errore: ", Err);
                throw Err;
            }
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Modify(TestataRow Item)
		{
			return Modify(Item.ID, Item.Ente, Item.NumeroDichiarazione, Item.AnnoDichiarazione, Item.NumeroProtocollo, 
				Item.DataProtocollo, Item.TotaleModelli, Item.DataInizio, Item.DataFine, Item.IDContribuente, Item.IDDenunciante, 
				Item.Bonificato, Item.Annullato, Item.DataInizioValidità, Item.DataFineValidità, Item.Operatore, Item.Storico, Item.IDProvenienza);
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="ente"></param>
		/// <param name="numeroDichiarazione"></param>
		/// <param name="annoDichiarazione"></param>
		/// <param name="numeroProtocollo"></param>
		/// <param name="dataProtocollo"></param>
		/// <param name="totaleModelli"></param>
		/// <param name="dataInizio"></param>
		/// <param name="dataFine"></param>
		/// <param name="idContribuente"></param>
		/// <param name="idDenunciante"></param>
		/// <param name="bonificato"></param>
		/// <param name="annullato"></param>
		/// <param name="dataInizioValidità"></param>
		/// <param name="dataFineValidità"></param>
		/// <param name="operatore"></param>
		/// <param name="storico"></param>
		/// <param name="idProvenienza"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>


		public bool Modify(int id, string ente, int numeroDichiarazione, string annoDichiarazione, string numeroProtocollo, 
			DateTime dataProtocollo, string totaleModelli, DateTime dataInizio, DateTime dataFine, int idContribuente, int idDenunciante, 
			bool bonificato, bool annullato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore, bool storico, int idProvenienza)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try{
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, NumeroDichiarazione=@numeroDichiarazione, " +
				"AnnoDichiarazione=@annoDichiarazione,  NumeroProtocollo=@numeroProtocollo, DataProtocollo=@dataProtocollo, " +
				"TotaleModelli=@totaleModelli, DataInizio=@dataInizio, DataFine=@dataFine, IdContribuente=@idContribuente, IdDenunciante=@iddenunciante, " +
				"Bonificato=@bonificato, Annullato=@annullato, DataInizioValidità=@dataInizioValidità, DataFineValidità=@dataFineValidità, " +
				"Operatore=@operatore, Storico=@stroico, IDProvenienza=@idProvenienza WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			ModifyCommand.Parameters.Add("@numeroDichiarazione",SqlDbType.Int).Value = numeroDichiarazione;
			ModifyCommand.Parameters.Add("@annoDichiarazione",SqlDbType.VarChar).Value = annoDichiarazione;
			ModifyCommand.Parameters.Add("@numeroProtocollo",SqlDbType.VarChar).Value = numeroProtocollo == null ? DBNull.Value : (object)numeroProtocollo;
			ModifyCommand.Parameters.Add("@dataProtocollo",SqlDbType.DateTime).Value = dataProtocollo == DateTime.MinValue ? SqlDateTime.Null : (object)dataProtocollo;
			ModifyCommand.Parameters.Add("@totaleModelli",SqlDbType.VarChar).Value = totaleModelli == null ? DBNull.Value : (object)totaleModelli;
			ModifyCommand.Parameters.Add("@dataInizio",SqlDbType.DateTime).Value = dataInizio == DateTime.MinValue ? DBNull.Value : (object)dataInizio;
			ModifyCommand.Parameters.Add("@dataFine",SqlDbType.DateTime).Value = dataFine == DateTime.MinValue ? DBNull.Value : (object)dataFine;
			ModifyCommand.Parameters.Add("@idContribuente",SqlDbType.Int).Value = idContribuente == 0 ? DBNull.Value : (object)idContribuente;
			ModifyCommand.Parameters.Add("@iddenunciante",SqlDbType.Int).Value = idDenunciante == 0 ? DBNull.Value : (object)idDenunciante;
			ModifyCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
			ModifyCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
			ModifyCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
			ModifyCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
			ModifyCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
			ModifyCommand.Parameters.Add("@stroico",SqlDbType.Bit).Value = storico;
			ModifyCommand.Parameters.Add("@idProvenienza", SqlDbType.Int).Value = idProvenienza;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

		}   }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.Modify.errore: ", Err);
                throw Err;
            }
        */
		/// <summary>
		/// 20141110 - passaggio di proprietà
		/// </summary>
		/// <param name="myItem"></param>
		/// <param name="dvListUI"></param>
		/// <returns></returns>
		public bool PassaggioProp(Utility.DichManagerICI.TestataRow myItem, string ListUIPassaggio, out DataView dvListUI)
		{
			SqlCommand InsertCommand = new SqlCommand();
			dvListUI = null;
			try
			{
				InsertCommand.CommandType = CommandType.StoredProcedure;
				InsertCommand.CommandText = "prc_SetPassaggioProprieta";
				InsertCommand.Parameters.Add("@IDTESTATA", SqlDbType.Int).Value = myItem.ID;
				InsertCommand.Parameters.Add("@DATAPASSAGGIO", SqlDbType.DateTime).Value = myItem.DataFine;
				InsertCommand.Parameters.Add("@IDCONTRIBUENTE", SqlDbType.Int).Value = myItem.IDContribuente;
				InsertCommand.Parameters.Add("@OPERATORE", SqlDbType.VarChar).Value = myItem.Operatore;
				InsertCommand.Parameters.Add("@ListUIPassaggio", SqlDbType.VarChar).Value = ListUIPassaggio;
				dvListUI = Query(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
				if (dvListUI != null)
					return true;
				else
					return false;
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.PassaggiProp.errore: ", Err);
				throw Err;
			}
		}
		//*** ***
		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="IdOggetto"></param>
		/// <param name="myConn"></param>
		/// <returns>
		/// Restituisce un oggetto di tipo TestataRow
		/// </returns>
		public Utility.DichManagerICI.TestataRow GetRow(int id, int IdOggetto, string myConn)
		{
			//log.Debug ("INIZIO GetRow");
			Utility.DichManagerICI.TestataRow Testata = new Utility.DichManagerICI.TestataRow();
			try
			{
				SqlCommand SelectCommand = new SqlCommand();/*= PrepareGetRow(id);*/
				SelectCommand.CommandType = CommandType.StoredProcedure;
				SelectCommand.CommandText = "prc_GetTestata";
				SelectCommand.Parameters.Clear();
				SelectCommand.Parameters.Add("@IDCONTRIBUENTE", SqlDbType.Int).Value = -1;
				SelectCommand.Parameters.Add("@IDTESTATA", SqlDbType.Int).Value = (IdOggetto > 0 ? -1 : id);
				SelectCommand.Parameters.Add("@IDOGGETTO", SqlDbType.Int).Value = IdOggetto;
				SelectCommand.Parameters.Add("@ENTE", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
				DataTable Tabella = Query(SelectCommand, new SqlConnection(myConn));
				if (Tabella.Rows.Count > 0)
				{
					Testata.ID = (int)Tabella.Rows[0]["ID"];
					Testata.Ente = (string)Tabella.Rows[0]["Ente"];
					Testata.NumeroDichiarazione = (int)Tabella.Rows[0]["NumeroDichiarazione"];
					Testata.AnnoDichiarazione = (string)Tabella.Rows[0]["AnnoDichiarazione"];
					//Testata.NumeroProtocollo = (string)Tabella.Rows[0]["NumeroProtocollo"];
					Testata.NumeroProtocollo = Tabella.Rows[0]["NumeroProtocollo"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["NumeroProtocollo"];
					Testata.DataProtocollo = Tabella.Rows[0]["DataProtocollo"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[0]["DataProtocollo"];
					Testata.TotaleModelli = Tabella.Rows[0]["TotaleModelli"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["TotaleModelli"];
					Testata.DataInizio = Tabella.Rows[0]["DataInizio"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Tabella.Rows[0]["DataInizio"]);
					Testata.DataFine = Tabella.Rows[0]["DataFine"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Tabella.Rows[0]["DataFine"]);
					Testata.IDContribuente = Tabella.Rows[0]["IDContribuente"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["IDContribuente"];
					Testata.IDDenunciante = Tabella.Rows[0]["IDDenunciante"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["IDDenunciante"];
					Testata.Bonificato = (bool)Tabella.Rows[0]["Bonificato"];
					Testata.Annullato = (bool)Tabella.Rows[0]["Annullato"];
					Testata.DataInizioValidità = (DateTime)Tabella.Rows[0]["DataInizioValidità"];
					Testata.DataFineValidità = Tabella.Rows[0]["DataFineValidità"] == DBNull.Value ? (DateTime)SqlDateTime.MinValue : (DateTime)Tabella.Rows[0]["DataFineValidità"];
					Testata.Operatore = (string)Tabella.Rows[0]["Operatore"];
					Testata.Storico = (bool)Tabella.Rows[0]["Storico"];
					Testata.IDQuestionario = (int)Tabella.Rows[0]["IDQuestionario"];
					Testata.IDProvenienza = (int)Tabella.Rows[0]["IDProvenienza"];
				}
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.GetRow.errore: ", Err);
				kill();
				log.Warn("Errore ");
				Testata = new Utility.DichManagerICI.TestataRow();
			}
			finally
			{
				kill();
			}
			//log.Debug ("FINE GetRow");
			return Testata;
		}

		private SqlCommand PrepareGetTestata(int idContribuente, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();
			try
			{
				SelectCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
				SelectCommand.CommandText = "Select * From " + this.TableName + " Where idContribuente=@idContribuente and ente=@ente";
				SelectCommand.Parameters.Add("@idContribuente", SqlDbType.Int, 4).Value = idContribuente;
				SelectCommand.Parameters.Add("@ente", SqlDbType.NVarChar).Value = ente;
				return SelectCommand;
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.PrepareGetTestata.errore: ", Err);
				throw Err;
			}
		}

		/// <summary>
		/// Ritorna un'array di struttura che rappresenta l'elenco delle testate di un contribuente.
		/// <param name="idContribuente"> idContribuente del record da cercare </param>
		/// <param name="ente"> ente del record da cercare </param>
		/// </summary>
		/// <returns>
		/// Restituisce un array di oggetti di tipo TestaRow
		/// </returns>
		public Utility.DichManagerICI.TestataRow[] GetTestataContribuente(int idContribuente, string ente)
		{

			string pathfileinfo;
			pathfileinfo = ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
			FileInfo fileconfiglog4net = new FileInfo(pathfileinfo);
			XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);
			ILog log = LogManager.GetLogger(typeof(TestataTable));

			int i = 0;
			try
			{
				SqlCommand SelectCommand = PrepareGetTestata(idContribuente, ente);
				DataTable Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				//TestataRow[] Testata = new TestataRow[Tabella.Rows.Count];
				ArrayList arrTestata = new ArrayList();
				Utility.DichManagerICI.TestataRow Testata;
				for (i = 0; i < Tabella.Rows.Count; i++)
				{
					Testata = new Utility.DichManagerICI.TestataRow();
					Testata.ID = (int)Tabella.Rows[i]["ID"];
					Testata.Ente = (string)Tabella.Rows[i]["Ente"];
					Testata.NumeroDichiarazione = (int)Tabella.Rows[i]["NumeroDichiarazione"];
					Testata.AnnoDichiarazione = (string)Tabella.Rows[i]["AnnoDichiarazione"];
					Testata.NumeroProtocollo = (string)Tabella.Rows[i]["NumeroProtocollo"];
					Testata.DataProtocollo = Tabella.Rows[i]["DataProtocollo"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[i]["DataProtocollo"];
					Testata.TotaleModelli = (string)Tabella.Rows[i]["TotaleModelli"];
					Testata.DataInizio = Tabella.Rows[i]["DataInizio"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Tabella.Rows[i]["DataInizio"]);
					Testata.DataFine = Tabella.Rows[i]["DataFine"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Tabella.Rows[i]["DataFine"]);
					Testata.IDContribuente = Tabella.Rows[i]["IDContribuente"] == DBNull.Value ? 0 : (int)Tabella.Rows[i]["IDContribuente"];
					Testata.IDDenunciante = Tabella.Rows[i]["IDDenunciante"] == DBNull.Value ? 0 : (int)Tabella.Rows[i]["IDDenunciante"];
					Testata.Bonificato = (bool)Tabella.Rows[i]["Bonificato"];
					Testata.Annullato = (bool)Tabella.Rows[i]["Annullato"];
					Testata.DataInizioValidità = (DateTime)Tabella.Rows[i]["DataInizioValidità"];
					Testata.DataFineValidità = Tabella.Rows[i]["DataFineValidità"] == DBNull.Value ? (DateTime)SqlDateTime.MinValue : (DateTime)Tabella.Rows[i]["DataFineValidità"];
					Testata.Operatore = (string)Tabella.Rows[i]["Operatore"];
					Testata.Storico = (bool)Tabella.Rows[i]["Storico"];
					Testata.IDQuestionario = (int)Tabella.Rows[i]["IDQuestionario"];
					Testata.IDProvenienza = (int)Tabella.Rows[i]["IDProvenienza"];

					arrTestata.Add(Testata);
				}
				return (Utility.DichManagerICI.TestataRow[])arrTestata.ToArray(typeof(Utility.DichManagerICI.TestataRow));
			}
			catch (Exception ex)
			{
				kill();
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.GetTestataContribuente.errore: ", ex);
				Utility.DichManagerICI.TestataRow[] vuoto = null;
				return vuoto;
			}
			finally
			{
				kill();
			}
		}


		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dal numero protocollo.
		/// </summary>
		/// <param name="numeroProtocollo"></param>
		/// <returns>
		/// restituisce un oggetto di tipo TestataRow
		/// </returns>
		public Utility.DichManagerICI.TestataRow GetRow(string numeroProtocollo)
		{
			Utility.DichManagerICI.TestataRow Testata = new Utility.DichManagerICI.TestataRow();
			try
			{
				SqlCommand SelectCommand = new SqlCommand();
				SelectCommand.CommandType = CommandType.Text;
				SelectCommand.CommandText = "Select * from " + this.TableName + " where NumeroProtocollo=@numeroProtocollo";
				SelectCommand.Parameters.Add("@numeroProtocollo", SqlDbType.VarChar).Value = numeroProtocollo;
				DataTable Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (Tabella.Rows.Count > 0)
				{
					Testata.ID = (int)Tabella.Rows[0]["ID"];
					Testata.Ente = (string)Tabella.Rows[0]["Ente"];
					Testata.NumeroDichiarazione = (int)Tabella.Rows[0]["NumeroDichiarazione"];
					Testata.AnnoDichiarazione = (string)Tabella.Rows[0]["AnnoDichiarazione"];
					Testata.NumeroProtocollo = (string)Tabella.Rows[0]["NumeroProtocollo"];
					Testata.DataProtocollo = Tabella.Rows[0]["DataProtocollo"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[0]["DataProtocollo"];
					Testata.TotaleModelli = (string)Tabella.Rows[0]["TotaleModelli"];
					Testata.DataInizio = Convert.ToDateTime(Tabella.Rows[0]["DataInizio"]);
					Testata.DataFine = Convert.ToDateTime(Tabella.Rows[0]["DataFine"]);
					Testata.IDContribuente = (int)Tabella.Rows[0]["IDContribuente"];
					Testata.IDDenunciante = Tabella.Rows[0]["IDDenunciante"] == System.DBNull.Value ? 0 : (int)Tabella.Rows[0]["IDDenunciante"];
					Testata.Bonificato = (bool)Tabella.Rows[0]["Bonificato"];
					Testata.Annullato = (bool)Tabella.Rows[0]["Annullato"];
					Testata.DataInizioValidità = (DateTime)Tabella.Rows[0]["DataInizioValidità"];
					Testata.DataFineValidità = Tabella.Rows[0]["DataFineValidità"] == DBNull.Value ? (DateTime)SqlDateTime.MinValue : (DateTime)Tabella.Rows[0]["DataFineValidità"];
					Testata.Operatore = (string)Tabella.Rows[0]["Operatore"];
					Testata.Storico = (bool)Tabella.Rows[0]["Storico"];
					Testata.IDQuestionario = (int)Tabella.Rows[0]["IDQuestionario"];
					Testata.IDProvenienza = (int)Tabella.Rows[0]["IDProvenienza"];
				}
			}
			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.GetTestataContribuente.errore: ", Err);

				kill();
				Testata = new Utility.DichManagerICI.TestataRow();
			}
			finally
			{
				kill();
			}
			return Testata;
		}


		/// <summary>
		/// Prende le dichiarazioni non cancellate.
		/// </summary>
		/// <returns>
		/// Restituisce un dataView contenente tutte le dichiarazioni non cancellate
		/// </returns>
		public DataView GetListTestateNonAnnullate()
		{
			DataView dv;
			SqlCommand SelectCommand = new SqlCommand();
			try
			{
				SelectCommand.CommandType = CommandType.Text;
				SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
					" WHERE Annullato <> 1 and ente like @ente";

				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
				dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
				kill();
				return dv;
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.GetListTestateNonAnnullate.errore: ", Err);
				throw Err;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Anno"></param>
		/// <returns></returns>
		public DataView GetListTestatePerAnater(string Anno)
		{
			DataView dv;
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM viewDichiarazioniDaRibaltareInAnater";
			SelectCommand.CommandText += " WHERE Ente like @ente and RibaltatoInAnater=0 and Annullato <> 1";

			// RIMUOVO IL FILTRO SU BONIFICATO = 1
			// SelectCommand.CommandText += " WHERE Ente like @ente and RibaltatoInAnater=0 and bonificato=1 and Annullato <> 1";
			try
			{
				if (Anno.ToString().CompareTo("-1") != 0)
				{
					SelectCommand.CommandText += " AND AnnoDichiarazione = @Anno";
				}
				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
				SelectCommand.Parameters.Add("@Anno", SqlDbType.VarChar).Value = Anno.ToString();
				SelectCommand.CommandText += " ORDER BY COGNOME_DENOMINAZIONE, NOME";

				dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
				kill();
				return dv;
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.GetListTestatePerAnater.errore: ", Err);
				throw Err;
			}
		}


		/// <summary>
		/// Storicizza i dati: modifica i dati vecchi.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Storicizzazione(int id)
		{
			SqlCommand StoricizzaCommand = new SqlCommand();
			try
			{
				StoricizzaCommand.CommandText = "UPDATE " + this.TableName + " SET " +
					"Storico=@storico, DataFineValidità=@dataFineValidità WHERE ID=@id";

				StoricizzaCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
				StoricizzaCommand.Parameters.Add("@storico", SqlDbType.Bit).Value = true;
				StoricizzaCommand.Parameters.Add("@dataFineValidità", SqlDbType.DateTime).Value = DateTime.Now;

				return Execute(StoricizzaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.Storicizzazione.errore: ", Err);
				throw Err;
			}
		}


		/// <summary>
		/// Esegue la cancellazione logica dei dati.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool CancellazioneLogica(int id)
		{
			SqlCommand DeleteCommand = new SqlCommand();
			try
			{
				DeleteCommand.CommandText = "UPDATE " + this.TableName + " SET " +
					"Annullato=1 WHERE ID=@id";

				DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
				return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.CancellazzioneLogica.errore: ", Err);
				throw Err;
			}
		}


		/// <summary>
		/// Elimina definitivamente una dichiarazione
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		///  Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool CancellazioneDichiarazione(int id) // eliminazione definitiva di una dichiarazione Ale 12/07/2006
		{
			SqlCommand DeleteCommand = new SqlCommand();
			try
			{
				DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
					" WHERE ID=@id";

				DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
				return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.Bonifica.errore: ", Err);
				throw Err;
			}
		}

		/// <summary>
		/// Imposta il flag bonificato a true.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		///  Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool Bonifica(int id)
		{
			SqlCommand BonificaCommand = new SqlCommand();
			try
			{
				BonificaCommand.CommandText = "UPDATE " + this.TableName +
					" SET Bonificato=1 WHERE ID=@id";

				BonificaCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
				return Execute(BonificaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.Bonifica.errore: ", Err);
				throw Err;
			}
		}


		/// <summary>
		/// Imposta il flag bonificato a false
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		///  Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool BonificaFalse(int id)
		{
			SqlCommand BonificaCommand = new SqlCommand();
			try
			{
				BonificaCommand.CommandText = "UPDATE " + this.TableName +
					" SET Bonificato=0 WHERE ID=@id";

				BonificaCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
				return Execute(BonificaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.BonificaFalse.errore: ", Err);
				throw Err;
			}
		}

		/// <summary>
		/// Torna una DataView con l'elenco delle dichiarazioni filtrate per contribuente.
		/// </summary>
		/// <param name="idContribuente"></param>
		/// <param name="bonificato"></param>
		/// <returns>
		/// Restituisce un dataView
		/// </returns>
		public DataView List(int idContribuente, Bonificato bonificato)
		{
			DataView dv;
			SqlCommand SelectCommand = new SqlCommand();
			try
			{
				SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
					" WHERE IDContribuente=@idContribuente AND Annullato=0 and Ente like @ente";

				switch (bonificato)
				{
					case Bonificato.Bonificate:
						SelectCommand.CommandText += " AND Bonificato=1";
						break;

					case Bonificato.DaBonificare:
						SelectCommand.CommandText += " AND Bonificato=0";
						break;
				}

				SelectCommand.Parameters.Add("@idContribuente", SqlDbType.Int).Value = idContribuente;
				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
				dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
				kill();
				return dv;
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.List.errore: ", Err);
				throw Err;
			}
		}

		/// <summary>
		/// Torna una DataView con l'elenco delle dichiarazioni filtrate per contribuente.
		/// verificando anche il contitolare
		/// </summary>
		/// <param name="idContribuente"></param>
		/// <param name="bonificato"></param>
		/// <returns>
		/// Restituisce un dataView
		/// </returns>
		public DataView ListCont(int idContribuente, Bonificato bonificato)
		{
			DataView dv;
			SqlCommand SelectCommand = new SqlCommand();
			try
			{
				//dipe 26/04/2010
				//			SelectCommand.CommandText = "SELECT distinct *, tbldettagliotestata.contitolare FROM " + this.TableName +
				//				" left outer join tbldettagliotestata on " +
				//				" tbldettagliotestata.idtestata = " + this.TableName +".id" +
				//				" and tbldettagliotestata.Ente = " + this.TableName +".Ente" +
				//				" WHERE (idcontribuente=@idContribuente or idsoggetto=@idContribuente) "+
				//				" AND " + this.TableName +".Annullato=0 "+
				//				" and " + this.TableName +".Ente like @ente";

				//SelectCommand.CommandText = "select distinct tbltestata.*, contitolare "+
				//	" FROM tbltestata "+
				//	" inner join tbldettagliotestata on "+
				//	" tbldettagliotestata.idtestata = TblTestata.id"+
				//	" and tbldettagliotestata.Ente = TblTestata.Ente"+
				//	" where tbldettagliotestata.id in ("+
				//	"   select distinct tbldettagliotestata.id"+
				//	"   FROM TblTestata"+
				//	"   left outer join tbldettagliotestata on "+
				//	"   tbldettagliotestata.idtestata = TblTestata.id"+
				//	"   and tbldettagliotestata.Ente = TblTestata.Ente"+
				//	"   WHERE (TblTestata.idcontribuente=@idContribuente)and (tbldettagliotestata.contitolare=0) "+
				//	"   AND TblTestata.Annullato=0 "+
				//	"   and TblTestata.Ente like @ente"+
				//	" union"+
				//	"   select distinct tbldettagliotestata.id"+
				//	"   FROM TblTestata"+
				//	"   left outer join tbldettagliotestata on "+
				//	"   tbldettagliotestata.idtestata = TblTestata.id"+
				//	"   and tbldettagliotestata.Ente = TblTestata.Ente"+
				//	"   WHERE (tbldettagliotestata.idsoggetto=@idContribuente)and (tbldettagliotestata.contitolare=1) "+
				//	"   AND TblTestata.Annullato=0 "+
				//	"   and TblTestata.Ente like @ente"+
				//	" ) "+
				//	" AND TblTestata.Annullato=0 "+
				//	" and TblTestata.Ente like @ente";

				SelectCommand.CommandType = CommandType.StoredProcedure;
				SelectCommand.CommandText = "prc_GetTestata";
				SelectCommand.Parameters.Clear();
				SelectCommand.Parameters.Add("@IDCONTRIBUENTE", SqlDbType.Int).Value = idContribuente;
				SelectCommand.Parameters.Add("@IDTESTATA", SqlDbType.Int).Value = -1;
				SelectCommand.Parameters.Add("@IDOGGETTO", SqlDbType.Int).Value = -1;
				SelectCommand.Parameters.Add("@ENTE", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
				//dv=Query(SelectCommand).DefaultView;
				dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
				SelectCommand.Dispose();


				/*

							SelectCommand.CommandText = "SELECT distinct TblTestata.*, "+
								" (select distinct contitolare from tbldettagliotestata where idsoggetto=@idContribuente and idtestata=TblTestata.id) as contitolare"+
								" from TblTestata "+
								" WHERE idcontribuente=@idContribuente "+
								" AND TblTestata.Annullato=0 "+
								" and TblTestata.Ente like @ente";


							switch(bonificato)
							{
								case Bonificato.Bonificate:
									SelectCommand.CommandText += " AND Bonificato=1";
									break;

								case Bonificato.DaBonificare:
									SelectCommand.CommandText += " AND Bonificato=0";
									break;
							}

							SelectCommand.Parameters.Add("@idContribuente", SqlDbType.Int).Value = idContribuente;
							SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
							dv=Query(SelectCommand).DefaultView;
							SelectCommand.Dispose();

							if (dv.Table.Rows.Count    ==0 ){

								SqlCommand SelectCommand1 = new SqlCommand();
								SelectCommand1.CommandText = "SELECT distinct *, tbldettagliotestata.contitolare FROM " + this.TableName +
									" left outer join tbldettagliotestata on " +
									" tbldettagliotestata.idtestata = " + this.TableName +".id" +
									" and tbldettagliotestata.Ente = " + this.TableName +".Ente" +
									" WHERE (idcontribuente=@idContribuente or idsoggetto=@idContribuente) "+
									" AND " + this.TableName +".Annullato=0 "+
									" and " + this.TableName +".Ente like @ente";
								switch(bonificato)
								{
									case Bonificato.Bonificate:
										SelectCommand1.CommandText += " AND Bonificato=1";
										break;

									case Bonificato.DaBonificare:
										SelectCommand1.CommandText += " AND Bonificato=0";
										break;
								}
								SelectCommand1.Parameters.Add("@idContribuente", SqlDbType.Int).Value = idContribuente;
								SelectCommand1.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
								dv=Query(SelectCommand1).DefaultView;
								SelectCommand1.Dispose() ;
							}
				*/
				kill();
				return dv;
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.ListCont.errore: ", Err);
				throw Err;
			}
		}


		/// <summary>
		/// Modifica l'id questionario della dichiarazione passata.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <param name="idQuestione"></param>
		/// <returns>
		///  Restituisce un valore booleano:
		/// true : se l'operazione è terminata correttamente
		/// false: se si sono verificati errori
		/// </returns>
		public bool SetQuestionario(int idDichiarazione, int idQuestione)
		{
			SqlCommand ModifyCommand = new SqlCommand();
			try
			{
				ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET" +
					" IDQuestionario=@idQuestionario WHERE ID=@idDichiarazione";

				ModifyCommand.Parameters.Add("@idQuestionario", SqlDbType.Int).Value = idQuestione;
				ModifyCommand.Parameters.Add("@idDichiarazione", SqlDbType.Int).Value = idDichiarazione;

				return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}

			catch (Exception Err)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.SetQuestionario.errore: ", Err);
				throw Err;
			}
		}


		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns>
		/// Restituisce un datview
		/// </returns>
		public DataView AnniCaricati()
		{
			DataView dv;
			try
			{
				SqlCommand SelectCommand = new SqlCommand();
				SelectCommand.CommandText = "Select AnnoDichiarazione From " + this.TableName + " WHERE ente like @ente GROUP BY AnnoDichiarazione";

				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
				dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
				return dv;
			}
			catch (Exception ex)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.AnniCaricati.errore: ", ex);
				return new DataView();
			}
			finally
			{
				kill();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataView GetListTestateAnagNoRibaltate()
		{
			DataView dv;
			SqlCommand SelectCommand = new SqlCommand();
			try
			{
				SelectCommand.CommandText = "SELECT * FROM viewDichiarazioniDaRibaltareInAnater" +
					" WHERE Ente like @ente and RibaltatoInAnater=1 and Annullato <> 1 and AnagrafeRibaltata = 0";
				//" WHERE Ente like @ente and RibaltatoInAnater=1 and bonificato=1 and Annullato <> 1 and AnagrafeRibaltata = 0";


				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;

				SelectCommand.CommandText += " ORDER BY COGNOME_DENOMINAZIONE, NOME";


				dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
				kill();
				return dv;
			}
			catch (Exception ex)
			{
				log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TestataTable.GetListTestateAnagNoRibaltate.errore: ", ex);
				return new DataView();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="IdTestata"></param>
		/// <returns></returns>
		public bool CongruenzaEnte(int IdTestata)
		{
			DataView myDataView = new DataView();
			bool myRet = false;
			try
			{
				string sSQL = string.Empty;
				using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
				{
					sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_CongruenzaEnte", "IDTESTATA");
					myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESTATA", IdTestata));
					ctx.Dispose();
				}
				foreach (DataRowView myRow in myDataView)
				{
					if (StringOperation.FormatInt(myRow["nrc"]) > 1)
						myRet = false;
					else
						myRet = true;
				}
			}
			catch (Exception ex)
			{
				log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Database.TestataTable.CongruenzaEnte.errore: ", ex);
				myRet = false;
			}
			return myRet;
		}
	}
}
