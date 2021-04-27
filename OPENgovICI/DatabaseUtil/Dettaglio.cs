using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struct contenente ogni campo, appositamente tipato, della TblDettaglio del DB.
	/// </summary>
	#region DettaglioRow
		public struct DettaglioRow
		{
			public int ID;
			public int IdTestata;
			public int NumeroDichiarazione;
			public string AnnoDichiarazione;
			public string Ente;
			public int IdOggetto;
			public int IdSoggetto;
			public string NumeroOrdine;
			public string NumeroModello;
			public float PercPossesso;
			public int MesiPossesso;
			public int MesiEsclusioneEserc;
			public int MesiRiduzione;
			public float ImpDetrazAbitazPrincipale;
			public bool Contitolare;
			public int Possesso;
			public int EsclusoEsercizio;
			public int Riduzione;
			public int AbitazionePrincipale;
			public string DataInizioValidita;
			public string DataFineValidita;
			public bool Effettivo;
			public bool Annullato;
			public string Operatore;
		}
	#endregion
	

	/// <summary>
	/// Classe che interfaccia la tabella TblDettaglio del DB.
	/// </summary>
	public class DettaglioTable : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(DettaglioTable));
        /// <summary>
        /// Inizializza la tabella gestita
        /// </summary>
        public DettaglioTable()
		{
			this.TableName = "TblDettaglio";
		}


		#region List
		/// <summary>
		/// In particolare seleziona tutti i dettagli di un oggetto di una determinata
		/// testata legati ai contitolari 
		/// (il soggetto del dettaglio non è il contribuente ma un contitolare).
		/// </summary>
		/// <param name="idTestata">Id della Testata cui appartiene l'oggetto</param>
		/// <param name="idOggetto">Id dell'oggetto di cui si cercano i dettagli legati ai contitolari</param>
		/// <param name="idSoggetto">Id del contribuente</param>
		/// <param name="soloValidi">I soli record validi se true, tutti se false</param>
		/// <returns>DataTable valorizzata</returns>
		public DataTable ListPerOggetto(int idTestata, int idOggetto, int idSoggetto, bool soloValidi)
		{
			DataTable Tabella;
			try
			{
				string selectStatem = "Select * From " + this.TableName + " where IdTestata=@IdTestata and IdOggetto=@IdOggetto and IdSoggetto<>@IdSoggetto";
				SqlCommand selectCommand = new SqlCommand();
				selectCommand.Connection=  this._DbConnection; 
				if (soloValidi)
				{
					selectStatem += " and DataFineValidita='' ";			
				}
				selectCommand.CommandText = selectStatem;
				selectCommand.Parameters.Add(new SqlParameter( "@IdTestata",SqlDbType.Int)).Value = idTestata;
				selectCommand.Parameters.Add(new SqlParameter( "@IdOggetto",SqlDbType.Int)).Value = idOggetto;
				selectCommand.Parameters.Add(new SqlParameter( "@IdSoggetto",SqlDbType.Int)).Value = idSoggetto;
				Tabella = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.ListPerOggetto.errore: ", Err);
                kill();
				Tabella = new DataTable();
			}
			finally{
				kill();
			}

			return Tabella;
		}


		/// <summary>
		/// Seleziona tutti i dettagli che si riferiscono all'immobile
		/// di cui viene passato l'identificativo.
		/// </summary>
		/// <param name="idOggetto">Identificativo dell'immobile.</param>
		/// <param name="soloValidi">I soli record validi se true, tutti se false</param>
		/// <returns>DataTable di tutte le righe trovate</returns>
		public DataTable ListPerOggetto(int idOggetto, bool soloValidi)
		{
			DataTable Tabella;
			try
			{
				string selectStatem = "Select * From " + this.TableName + " where IdOggetto=@IdOggetto ";
				SqlCommand selectCommand = new SqlCommand();
				selectCommand.Connection=  this._DbConnection; 
				if (soloValidi)
				{
					selectStatem += " and DataFineValidita='' ";			
				}
				selectCommand.CommandText = selectStatem;
				selectCommand.Parameters.Add(new SqlParameter( "@IdOggetto",SqlDbType.Int)).Value = idOggetto;
				Tabella = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.ListPerOggetto.errore: ", Err);
                kill();
				Tabella = new DataTable();
			}
			finally
			{
				kill();
			}

			return Tabella;
		}

	
		/// <summary>
		/// Recupera tutti i dettagli associati a una data testata valida.
		/// </summary>
		/// <param name="idTestata">Identificativo della testata di appartenenza.</param>
		/// <returns>DataTable di tutte le righe trovate</returns>
		public DataTable ListPerTestata(int idTestata)
		{
			DataTable Tabella;
			try
			{
				string selectStatem = "Select * From " + this.TableName + 
									" where IdTestata=@IdTestata and DataFineValidita='' ";
				SqlCommand selectCommand = new SqlCommand();
				selectCommand.CommandText = selectStatem;
				selectCommand.Parameters.Add(new SqlParameter( "@IdTestata",SqlDbType.Int)).Value = idTestata;
				Tabella = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.ListPerTestata.errore: ", Err);
                kill();
				Tabella = new DataTable();
			}
			finally
			{
				kill();
			}

			return Tabella;
		}
		#endregion


		#region Insert
		/// <summary>
		/// Inserisce un nuovo elemento nella tabella
		/// </summary>
		/// <param name="Item"> Elemento DettaglioRow da inserire </param>
		/// <returns> Esito dell'operazione </returns>
		public bool Insert(DettaglioRow item)
		{

			return Insert(item.IdTestata, item.NumeroDichiarazione, item.AnnoDichiarazione,
				item.Ente, item.IdOggetto, item.IdSoggetto, item.NumeroOrdine,
				item.NumeroModello, item.PercPossesso, item.MesiPossesso, 
				item.MesiEsclusioneEserc, item.MesiRiduzione, item.ImpDetrazAbitazPrincipale,
				item.Contitolare, item.Possesso, item.EsclusoEsercizio, item.Riduzione,
				item.AbitazionePrincipale, item.DataInizioValidita, item.DataFineValidita,
				item.Effettivo, item.Annullato, item.Operatore);
		}


		/// <summary>
		/// Inserisce un nuovo elemento nella tabella e ne restituisce l'id
		/// </summary>
		/// <param name="Item"> Elemento DettaglioRow  da inserire </param>
		/// <param name="id">Reference all'identificativo del nuovo elemento inserito nel DB</param>
		/// <returns> Esito dell'operazione </returns>
		public bool Insert(DettaglioRow item, out int id)
		{

			return Insert(item.IdTestata, item.NumeroDichiarazione, item.AnnoDichiarazione,
				item.Ente, item.IdOggetto, item.IdSoggetto, item.NumeroOrdine,
				item.NumeroModello, item.PercPossesso, item.MesiPossesso, 
				item.MesiEsclusioneEserc, item.MesiRiduzione, item.ImpDetrazAbitazPrincipale,
				item.Contitolare, item.Possesso, item.EsclusoEsercizio, item.Riduzione,
				item.AbitazionePrincipale, item.DataInizioValidita, item.DataFineValidita,
				item.Effettivo, item.Annullato, item.Operatore, out id);
		}


		/// <summary>
		/// Inserisce un nuovo elemento nella tabella, 
		/// ricevendo in input separatamente tutti i campi della tabella.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="numeroDichiarazione"></param>
		/// <param name="annoDichiarazione"></param>
		/// <param name="ente"></param>
		/// <param name="idOggetto"></param>
		/// <param name="idSoggetto"></param>
		/// <param name="numeroOrdine"></param>
		/// <param name="numeroModello"></param>
		/// <param name="percPossesso"></param>
		/// <param name="mesiPossesso"></param>
		/// <param name="mesiEsclusioneEserc"></param>
		/// <param name="mesiRiduzione"></param>
		/// <param name="impDetrazAbitazPrincipale"></param>
		/// <param name="contitolare"></param>
		/// <param name="possesso"></param>
		/// <param name="esclusoEsercizio"></param>
		/// <param name="riduzione"></param>
		/// <param name="abitazionePrincipale"></param>
		/// <param name="dataInizioValidita"></param>
		/// <param name="dataFineValidita"></param>
		/// <param name="effettivo"></param>
		/// <param name="annullato"></param>
		/// <param name="operatore"></param>
		/// <returns> Esito dell'operazione </returns>
		public bool Insert(	int idTestata, int numeroDichiarazione, string annoDichiarazione,
						 string ente, int idOggetto, int idSoggetto, string numeroOrdine,
						 string numeroModello, float percPossesso, int mesiPossesso,
						 int mesiEsclusioneEserc, int mesiRiduzione, float impDetrazAbitazPrincipale,
						 bool contitolare, int possesso, int esclusoEsercizio, int riduzione,
						 int abitazionePrincipale, string dataInizioValidita, string dataFineValidita,
						 bool effettivo, bool annullato, string operatore) 
		{
			SqlCommand insertCommand = ConstructInsertCommand(idTestata, numeroDichiarazione,
					annoDichiarazione, ente, idOggetto, idSoggetto, numeroOrdine, numeroModello,
					percPossesso, mesiPossesso, mesiEsclusioneEserc, mesiRiduzione, 
					impDetrazAbitazPrincipale, contitolare, possesso, esclusoEsercizio, 
					riduzione, abitazionePrincipale, dataInizioValidita, dataFineValidita, effettivo, 
					annullato, operatore);		
			return Execute(insertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Inserisce un nuovo elemento nella tabella
		/// ricevendo in input separatamente tutti i campi della tabella.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="numeroDichiarazione"></param>
		/// <param name="annoDichiarazione"></param>
		/// <param name="ente"></param>
		/// <param name="idOggetto"></param>
		/// <param name="idSoggetto"></param>
		/// <param name="numeroOrdine"></param>
		/// <param name="numeroModello"></param>
		/// <param name="percPossesso"></param>
		/// <param name="mesiPossesso"></param>
		/// <param name="mesiEsclusioneEserc"></param>
		/// <param name="mesiRiduzione"></param>
		/// <param name="impDetrazAbitazPrincipale"></param>
		/// <param name="contitolare"></param>
		/// <param name="possesso"></param>
		/// <param name="esclusoEsercizio"></param>
		/// <param name="riduzione"></param>
		/// <param name="abitazionePrincipale"></param>
		/// <param name="dataInizioValidita"></param>
		/// <param name="dataFineValidita"></param>
		/// <param name="effettivo"></param>
		/// <param name="annullato"></param>
		/// <param name="operatore"></param>
		/// <param name="id">Reference all'identificativo del nuovo elemento inserito nel DB</param>
		/// <returns> Esito dell'operazione </returns>
		public bool Insert(	int idTestata, int numeroDichiarazione, string annoDichiarazione,
			string ente, int idOggetto, int idSoggetto, string numeroOrdine,
			string numeroModello, float percPossesso, int mesiPossesso,
			int mesiEsclusioneEserc, int mesiRiduzione, float impDetrazAbitazPrincipale,
			bool contitolare, int possesso, int esclusoEsercizio, int riduzione,
			int abitazionePrincipale, string dataInizioValidita, string dataFineValidita,
			bool effettivo, bool annullato, string operatore, out int id) 
		{
			SqlCommand insertCommand = ConstructInsertCommand(idTestata, numeroDichiarazione,
				annoDichiarazione, ente, idOggetto, idSoggetto, numeroOrdine, numeroModello,
				percPossesso, mesiPossesso, mesiEsclusioneEserc, mesiRiduzione, 
				impDetrazAbitazPrincipale, contitolare, possesso, esclusoEsercizio, 
				riduzione, abitazionePrincipale,  dataInizioValidita, dataFineValidita, effettivo, 
				annullato, operatore);
			return Execute(insertCommand, new SqlConnection(Business.ConstWrapper.StringConnection), out id);
		}
	#endregion


		#region ConstructCommand
		/// <summary>
		/// Costruisce un SqlCommand di tipo INSERT con tutti i campi.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="numeroDichiarazione"></param>
		/// <param name="annoDichiarazione"></param>
		/// <param name="ente"></param>
		/// <param name="idOggetto"></param>
		/// <param name="idSoggetto"></param>
		/// <param name="numeroOrdine"></param>
		/// <param name="numeroModello"></param>
		/// <param name="percPossesso"></param>
		/// <param name="mesiPossesso"></param>
		/// <param name="mesiEsclusioneEserc"></param>
		/// <param name="mesiRiduzione"></param>
		/// <param name="impDetrazAbitazPrincipale"></param>
		/// <param name="contitolare"></param>
		/// <param name="possesso"></param>
		/// <param name="esclusoEsercizio"></param>
		/// <param name="riduzione"></param>
		/// <param name="abitazionePrincipale"></param>
		/// <param name="dataInizioValidita"></param>
		/// <param name="dataFineValidita"></param>
		/// <param name="effettivo"></param>
		/// <param name="annullato"></param>
		/// <param name="operatore"></param>
		/// <returns> SqlCommand costruito </returns>
		private SqlCommand  ConstructInsertCommand(	int idTestata, 
				int numeroDichiarazione, string annoDichiarazione,
				string ente, int idOggetto, int idSoggetto, string numeroOrdine,
				string numeroModello, float percPossesso, int mesiPossesso,
				int mesiEsclusioneEserc, int mesiRiduzione, float impDetrazAbitazPrincipale,
				bool contitolare, int possesso, int esclusoEsercizio, int riduzione,
				int abitazionePrincipale, string dataInizioValidita, string dataFineValidita,
				bool effettivo, bool annullato, string operatore) 
		{
			SqlCommand insertCommand = new SqlCommand();
            try { 
			insertCommand.CommandText = "INSERT INTO " + this.TableName +
				"(IdTestata, NumeroDichiarazione, AnnoDichiarazione, Ente, IdOggetto,"+
				" IdSoggetto, NumeroOrdine, NumeroModello, PercPossesso, MesiPossesso,"+
				" MesiEsclusioneEserc, MesiRiduzione, ImpDetrazAbitazPrincipale, Contitolare,"+
				" Possesso, EsclusoEsercizio, Riduzione, AbitazionePrincipale, Effettivo, "+
				" Annullato, DataFineValidita, DataInizioValidita, Operatore)"+
				" VALUES (@IdTestata, @NumeroDichiarazione, @AnnoDichiarazione,"+
				" @Ente, @IdOggetto, @IdSoggetto, @NumeroOrdine, @NumeroModello,"+ 
				" @PercPossesso, @MesiPossesso, @MesiEsclusioneEserc, @MesiRiduzione,"+
				" @ImpDetrazAbitazPrincipale, @Contitolare, @Possesso, @EsclusoEsercizio,"+
				" @Riduzione, @AbitazionePrincipale, @Effettivo, @Annullato, "+
				" @DataFineValidita, @DataInizioValidita, @Operatore)";

			insertCommand.Parameters.Add(new SqlParameter("@IdTestata", System.Data.SqlDbType.Int)).Value = idTestata;
			insertCommand.Parameters.Add(new SqlParameter("@NumeroDichiarazione", System.Data.SqlDbType.Int)).Value = numeroDichiarazione;
			insertCommand.Parameters.Add(new SqlParameter("@AnnoDichiarazione", System.Data.SqlDbType.NVarChar)).Value = annoDichiarazione;
			insertCommand.Parameters.Add(new SqlParameter("@Ente", System.Data.SqlDbType.NVarChar)).Value = ente;
			insertCommand.Parameters.Add(new SqlParameter("@IdOggetto", System.Data.SqlDbType.Int)).Value = idOggetto;
			insertCommand.Parameters.Add(new SqlParameter("@IdSoggetto", System.Data.SqlDbType.Int)).Value = idSoggetto == 0 ? DBNull.Value : (object)idSoggetto;
			insertCommand.Parameters.Add(new SqlParameter("@NumeroOrdine", System.Data.SqlDbType.NVarChar)).Value = numeroOrdine;
			insertCommand.Parameters.Add(new SqlParameter("@NumeroModello", System.Data.SqlDbType.NVarChar)).Value = numeroModello;
			insertCommand.Parameters.Add(new SqlParameter("@PercPossesso", System.Data.SqlDbType.Decimal)).Value =  percPossesso;
			insertCommand.Parameters.Add(new SqlParameter("@MesiPossesso", System.Data.SqlDbType.Int)).Value = mesiPossesso;
			insertCommand.Parameters.Add(new SqlParameter("@MesiEsclusioneEserc", System.Data.SqlDbType.Int)).Value = mesiEsclusioneEserc;
			insertCommand.Parameters.Add(new SqlParameter("@MesiRiduzione", System.Data.SqlDbType.Decimal)).Value = mesiRiduzione;
			insertCommand.Parameters.Add(new SqlParameter("@ImpDetrazAbitazPrincipale", System.Data.SqlDbType.Decimal)).Value = impDetrazAbitazPrincipale;
			insertCommand.Parameters.Add(new SqlParameter("@Contitolare", System.Data.SqlDbType.Int)).Value = contitolare;
			insertCommand.Parameters.Add(new SqlParameter("@Possesso", System.Data.SqlDbType.Int)).Value = possesso;
			insertCommand.Parameters.Add(new SqlParameter("@EsclusoEsercizio", System.Data.SqlDbType.Int)).Value = esclusoEsercizio;
			insertCommand.Parameters.Add(new SqlParameter("@Riduzione", System.Data.SqlDbType.Int)).Value = riduzione;
			insertCommand.Parameters.Add(new SqlParameter("@AbitazionePrincipale", System.Data.SqlDbType.Int)).Value = abitazionePrincipale;
			insertCommand.Parameters.Add(new SqlParameter("@Effettivo", System.Data.SqlDbType.Bit)).Value = effettivo;
			insertCommand.Parameters.Add(new SqlParameter("@Annullato", System.Data.SqlDbType.Bit)).Value = annullato;
			insertCommand.Parameters.Add(new SqlParameter("@DataFineValidita", System.Data.SqlDbType.NVarChar)).Value = dataFineValidita;
			insertCommand.Parameters.Add(new SqlParameter("@DataInizioValidita", System.Data.SqlDbType.NVarChar)).Value = dataInizioValidita;
			insertCommand.Parameters.Add(new SqlParameter("@Operatore", System.Data.SqlDbType.NVarChar)).Value = operatore;
			
			return insertCommand;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.ConstructInsertCommand.errore: ", Err);
                throw Err;
            }
        }
		#endregion


		#region Modify
		/// <summary>
		/// Modifica il record della tabella con identificativo corrispondente
		/// all'ID della DettaglioRow passata in input.
		/// </summary>
		/// <param name="Item"> Elemento DettaglioRow con i nuovi valori</param>
		/// <returns> Esito dell'operazione </returns>
		public bool Modify(DettaglioRow item)
		{

			return Modify(item.ID, item.IdTestata, item.NumeroDichiarazione, item.AnnoDichiarazione,
				item.Ente, item.IdOggetto, item.IdSoggetto, item.NumeroOrdine,
				item.NumeroModello, item.PercPossesso, item.MesiPossesso, 
				item.MesiEsclusioneEserc, item.MesiRiduzione, item.ImpDetrazAbitazPrincipale,
				item.Contitolare, item.Possesso, item.EsclusoEsercizio, item.Riduzione,
				item.AbitazionePrincipale, item.DataInizioValidita, item.DataFineValidita,
				item.Effettivo, item.Annullato, item.Operatore);
		}

		/// <summary>
		/// Modifica del record della tabella con identificativo passato come primo argomento.
		/// In input sono passati i nuovi valori per ogni campo della tabella. 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="idTestata"></param>
		/// <param name="numeroDichiarazione"></param>
		/// <param name="annoDichiarazione"></param>
		/// <param name="ente"></param>
		/// <param name="idOggetto"></param>
		/// <param name="idSoggetto"></param>
		/// <param name="numeroOrdine"></param>
		/// <param name="numeroModello"></param>
		/// <param name="percPossesso"></param>
		/// <param name="mesiPossesso"></param>
		/// <param name="mesiEsclusioneEserc"></param>
		/// <param name="mesiRiduzione"></param>
		/// <param name="impDetrazAbitazPrincipale"></param>
		/// <param name="contitolare"></param>
		/// <param name="possesso"></param>
		/// <param name="esclusoEsercizio"></param>
		/// <param name="riduzione"></param>
		/// <param name="abitazionePrincipale"></param>
		/// <param name="dataInizioValidita"></param>
		/// <param name="dataFineValidita"></param>
		/// <param name="effettivo"></param>
		/// <param name="annullato"></param>
		/// <param name="operatore"></param>
		/// <returns> Esito dell'operazione </returns>
		public bool Modify(int id, int idTestata, int numeroDichiarazione, string annoDichiarazione,
			string ente, int idOggetto, int idSoggetto, string numeroOrdine,
			string numeroModello, float percPossesso, int mesiPossesso,
			int mesiEsclusioneEserc, int mesiRiduzione, float impDetrazAbitazPrincipale,
			bool contitolare, int possesso, int esclusoEsercizio, int riduzione,
			int abitazionePrincipale, string dataInizioValidita, string dataFineValidita,
			bool effettivo, bool annullato, string operatore)
		{
			SqlCommand modifyCommand = new SqlCommand();
            try { 
			modifyCommand.CommandText = "UPDATE " + this.TableName +
				" SET IdTestata = @IdTestata, NumeroDichiarazione = @NumeroDichiarazione," +
				" AnnoDichiarazione = @AnnoDichiarazione, Ente = @Ente, IdOggetto = @IdOggetto, " +
				"IdSoggetto = @IdSoggetto, NumeroOrdine = @NumeroOrdine, NumeroModello = @NumeroModello," +
				" PercPossesso = @PercPossesso, MesiPossesso = @MesiPossesso," +
				" MesiEsclusioneEserc = @MesiEsclusioneEserc, MesiRiduzione = @MesiRiduzione, " +
				" ImpDetrazAbitazPrincipale = @ImpDetrazAbitazPrincipale, Contitolare = @Contitolare," +
				" Possesso = @Possesso, EsclusoEsercizio = @EsclusoEsercizio, Riduzione = @Riduzione, " +
				" AbitazionePrincipale = @AbitazionePrincipale, Effettivo = @Effettivo," +
				" Annullato = @Annullato, DataFineValidita = @DataFineValidita, DataInizioValidita = @DataInizioValidita," +
				" Operatore = @Operatore WHERE (ID = @ID)";
			
			modifyCommand.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.Int)).Value = id;
			modifyCommand.Parameters.Add(new SqlParameter("@IdTestata", System.Data.SqlDbType.Int)).Value = idTestata;
			modifyCommand.Parameters.Add(new SqlParameter("@NumeroDichiarazione", System.Data.SqlDbType.Int)).Value = numeroDichiarazione;
			modifyCommand.Parameters.Add(new SqlParameter("@AnnoDichiarazione", System.Data.SqlDbType.NVarChar)).Value = annoDichiarazione;
			modifyCommand.Parameters.Add(new SqlParameter("@Ente", System.Data.SqlDbType.NVarChar)).Value = ente;
			modifyCommand.Parameters.Add(new SqlParameter("@IdOggetto", System.Data.SqlDbType.Int)).Value = idOggetto;
			modifyCommand.Parameters.Add(new SqlParameter("@IdSoggetto", System.Data.SqlDbType.Int)).Value = idSoggetto == 0 ? DBNull.Value : (object)idSoggetto;
			modifyCommand.Parameters.Add(new SqlParameter("@NumeroOrdine", System.Data.SqlDbType.NVarChar)).Value = numeroOrdine;
			modifyCommand.Parameters.Add(new SqlParameter("@NumeroModello", System.Data.SqlDbType.NVarChar)).Value = numeroModello;
			modifyCommand.Parameters.Add(new SqlParameter("@PercPossesso", System.Data.SqlDbType.Decimal)).Value =  percPossesso;
			modifyCommand.Parameters.Add(new SqlParameter("@MesiPossesso", System.Data.SqlDbType.Int)).Value = mesiPossesso;
			modifyCommand.Parameters.Add(new SqlParameter("@MesiEsclusioneEserc", System.Data.SqlDbType.Int)).Value = mesiEsclusioneEserc;
			modifyCommand.Parameters.Add(new SqlParameter("@MesiRiduzione", System.Data.SqlDbType.Int)).Value = mesiRiduzione;
			modifyCommand.Parameters.Add(new SqlParameter("@ImpDetrazAbitazPrincipale", System.Data.SqlDbType.Decimal)).Value = impDetrazAbitazPrincipale;
			modifyCommand.Parameters.Add(new SqlParameter("@Contitolare", System.Data.SqlDbType.Bit)).Value = contitolare;
			modifyCommand.Parameters.Add(new SqlParameter("@Possesso", System.Data.SqlDbType.Int)).Value = possesso;
			modifyCommand.Parameters.Add(new SqlParameter("@EsclusoEsercizio", System.Data.SqlDbType.Int)).Value = esclusoEsercizio;
			modifyCommand.Parameters.Add(new SqlParameter("@Riduzione", System.Data.SqlDbType.Int)).Value = riduzione;
			modifyCommand.Parameters.Add(new SqlParameter("@AbitazionePrincipale", System.Data.SqlDbType.Int)).Value = abitazionePrincipale;
			modifyCommand.Parameters.Add(new SqlParameter("@Effettivo", System.Data.SqlDbType.Bit)).Value = effettivo;
			modifyCommand.Parameters.Add(new SqlParameter("@Annullato", System.Data.SqlDbType.Bit)).Value = annullato;
			modifyCommand.Parameters.Add(new SqlParameter("@DataFineValidita", System.Data.SqlDbType.NVarChar)).Value = dataFineValidita;
			modifyCommand.Parameters.Add(new SqlParameter("@DataInizioValidita", System.Data.SqlDbType.NVarChar)).Value = dataInizioValidita;
			modifyCommand.Parameters.Add(new SqlParameter("@Operatore", System.Data.SqlDbType.NVarChar)).Value = operatore;
			
			return Execute(modifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.Modify.errore: ", Err);
                throw Err;
            }
        }

        #endregion


        /// <summary>
        /// Duplica tutti i record dettagli-contitolari di una data 
        /// testata e immobile invalidando quelli vecchi.  Viene invocato in 
        /// caso di ri-salvamento di un immobile.
        /// </summary>
        /// <param name="oldIdDettaglio">Identificativo del vecchio dettaglio principale, quello appartenente al contribuente. </param>
        /// <param name="newIdOggetto">Identificativo del nuovo oggetto</param>
        /// <param name="operatore">Operatore che effettua la modifica</param>
        /// <param name="dataFine">Data di fine validità dei vecchi record e inizio validità dei nuovi.</param>
        /// <returns>Esito dell'operazione.</returns>
        //public bool DuplicateAndUpdateDettagliContitolari(int oldIdDettaglio, int newIdOggetto, string operatore, string dataFine)
        //{
        //try{
        //	DettaglioRow itemDettaglio = GetRow(oldIdDettaglio);
        //	if (itemDettaglio.ID == 0) 
        //		return false; 		
        //	int idTestata = itemDettaglio.IdTestata;
        //	int idContribuente = itemDettaglio.IdSoggetto;
        //	int idOggetto = itemDettaglio.IdOggetto;

        //	// si recuperano tutti i dettagli dei contitolari 
        //	DataTable tblDettagliContitolari = ListPerOggetto(idTestata, idOggetto, idContribuente, true); 
        //	foreach (DataRow rowDettaglio in tblDettagliContitolari.Rows)
        //	{	
        //		int idDettaglioContitolare = (int) rowDettaglio["ID"];
        //		// si invalida il dettaglio contitolare 
        //		if (!Modify(idDettaglioContitolare, dataFine))
        //			return false;
        //		// si duplica il dettaglio contitolare
        //		itemDettaglio = GetRow(idDettaglioContitolare);
        //		if (itemDettaglio.ID == 0)
        //			return false; 
        //		itemDettaglio.IdOggetto = newIdOggetto; 
        //		itemDettaglio.Operatore = operatore;
        //		itemDettaglio.DataFineValidita = "";
        //		itemDettaglio.DataInizioValidita = dataFine; 
        //		if ( !Insert(itemDettaglio))
        //			return false; 
        //	}
        //	return true; 
   // }
		//	catch(Exception Err)
			//{
              //  Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.DuplicateAndUpdatrDettagliContitolari.errore: ", Err);
           //}
//}


/// <summary>
/// Costruisce un DettaglioRow a partire dal record della
/// tabella con l'identificativo passato. 
/// </summary>
/// <param name="id">Identificativo del record ricercato</param>
/// <returns>DettaglioRow del record ricercato se esistente o solo allocato
/// altrimenti </returns>
public DettaglioRow GetRow(int id)
		{
			DettaglioRow rowDettaglio = new DettaglioRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);

				DataTable tblDettaglio = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (tblDettaglio.Rows.Count > 0)
				{
					rowDettaglio = ReadRow(tblDettaglio.Rows[0]);
				}
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.GetRow.errore: ", Err);
                kill();
				rowDettaglio = new DettaglioRow();
			}
			finally{
				kill();
			}

			return rowDettaglio;
		}

		/// <summary>
		/// Prende il dettaglio a partire dall'id dell'immobile e dall'id della testata.
		/// </summary>
		/// <param name="idImmobile"></param>
		/// <param name="idTestata"></param>
		/// <param name="contitolare"></param>
		/// <returns></returns>
		public DettaglioRow GetRow(int idImmobile, int idTestata, bool contitolare)
		{
			DettaglioRow rowDettaglio = new DettaglioRow();
			try
			{
				SqlCommand SelectCommand = new SqlCommand();
				SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
					" WHERE IDOggetto=@idImmobile AND IDTestata=@idTestata";

				if(contitolare)
					SelectCommand.CommandText += " AND Contitolare = 1";
				else
					SelectCommand.CommandText += " AND Contitolare = 0";

				SelectCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idImmobile;
				SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;

				DataTable tblDettaglio = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (tblDettaglio.Rows.Count > 0)
				{
					rowDettaglio = ReadRow(tblDettaglio.Rows[0]);
				}
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.GetRow.errore: ", Err);
                kill();
				rowDettaglio = new DettaglioRow();
			}
			finally
			{
				kill();
			}

			return rowDettaglio;
		}


		/// <summary>
		/// Legge una riga della tabella e ne costruisce un DettaglioRow.
		/// </summary>
		/// <param name="row">Riga della tabella.</param>
		/// <returns>La DettaglioRow letta.</returns>
		public DettaglioRow ReadRow(DataRow row)
		{
			DettaglioRow rowDettaglio = new DettaglioRow();
			rowDettaglio.ID  = (int)row["ID"];
			rowDettaglio.IdTestata  =  (int) row["IdTestata"];
			rowDettaglio.NumeroDichiarazione  =  (int) row["NumeroDichiarazione"];
			rowDettaglio.AnnoDichiarazione  =  row["AnnoDichiarazione"].ToString();
			rowDettaglio.Ente   =  row["Ente"].ToString();
			rowDettaglio.IdOggetto  = (int)  row["IdOggetto"];
			rowDettaglio.IdSoggetto  = (row["IdSoggetto"] != DBNull.Value) ? (int)  row["IdSoggetto"] : 0;
			rowDettaglio.NumeroOrdine  =  row["NumeroOrdine"].ToString();
			rowDettaglio.NumeroModello  =  row["NumeroModello"].ToString();
			rowDettaglio.PercPossesso =   (float)  row["PercPossesso"];
			rowDettaglio.MesiPossesso   = (int)  row["MesiPossesso"];
			rowDettaglio.MesiEsclusioneEserc  = (int)  row["MesiEsclusioneEserc"];
			rowDettaglio.MesiRiduzione  = (int)  row["MesiRiduzione"];
			rowDettaglio.ImpDetrazAbitazPrincipale  = (float) row["ImpDetrazAbitazPrincipale"];
			rowDettaglio.Contitolare = (bool) row["Contitolare"];  
			rowDettaglio.Possesso = (int) row["Possesso"];
			rowDettaglio.EsclusoEsercizio =(int) row["EsclusoEsercizio"];
			rowDettaglio.Riduzione = (int) row["Riduzione"];
			rowDettaglio.AbitazionePrincipale = (int) row["AbitazionePrincipale"];
			rowDettaglio.DataInizioValidita  =  row["DataInizioValidita"].ToString();
			rowDettaglio.DataFineValidita  =  row["DataFineValidita"].ToString();
			rowDettaglio.Effettivo = (bool) row["Effettivo"];
			rowDettaglio.Annullato = (bool) row["Annullato"];
			rowDettaglio.Operatore  =  row["Operatore"].ToString();
			return rowDettaglio; 
		}

		/// <summary>
		/// Imposta il flag Effettivo a True.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public bool ModifyEffettivo(int idTestata)
		{
			SqlCommand ModifyCommad = new SqlCommand();
			ModifyCommad.CommandText = "UPDATE " + this.TableName +
				" SET Effettivo=1 WHERE IDTestata=@idTestata";

			ModifyCommad.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

			return Execute(ModifyCommad, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Cancella la riga identificata dall'id dell'immobile e dall'id della testata.
		/// Così viene cancellata l'associazione tra un immobile e una dichiarazione.
		/// </summary>
		/// <param name="idImmobile"></param>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public bool DeleteItem(int idImmobile, int idTestata)
		{
			SqlCommand DeleteCommand = new SqlCommand();
			DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IDOggetto=@idImmobile AND IDTestata=@idTestata";

			DeleteCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idImmobile;
			DeleteCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;

			return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}

		/// <summary>
		/// Torna l'id dell'immobile del dettaglio.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int GetIDOggetto(int id)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT IdOggetto FROM " + this.TableName +
				" WHERE ID=@id";

			SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
			return Convert.ToInt32(QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)));
		}

		/// <summary>
		/// Torna una dataview valorizzata col elenco dei contitolari.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="idImmobile"></param>
		/// <param name="effettivo"></param>
		/// <returns></returns>
		public DataView ListContitolari(int idTestata, int idImmobile, bool effettivo)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IdTestata=@idTestata AND IdOggetto=@idImmobile AND Contitolare=1";
            try {
                if (effettivo)
                    SelectCommand.CommandText += " AND Effettivo=0";

                SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;
                SelectCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idImmobile;

                DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
                kill();
                return dv;
            }
            catch(Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettaglioTable.ListContitolari.errore: ", Err);
                throw Err;
            }
		}
	}
}

	