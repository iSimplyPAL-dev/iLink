using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using log4net;
using Utility;

namespace DichiarazioniICI.Database
{
    /*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
    /// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblOggetti.
	/// </summary>
	public struct OggettiRow
	{
		public int ID;
		public string Ente;
		public int IdTestata;
		public string NumeroOrdine;
		public string NumeroModello;
		public int CodUI;
		public int TipoImmobile;
		public int PartitaCatastale;
		public string Foglio;
		public string Numero;
		public int Subalterno;
		public int Caratteristica;
		public string Sezione;
		public string NumeroProtCatastale;
		public string AnnoDenunciaCatastale;
		public string CodCategoriaCatastale;
		public string CodClasse;
		public string CodRendita;
		public bool Storico;
		//public float ValoreImmobile;
		public decimal ValoreImmobile;		
		public int IDValuta;
		public bool FlagValoreProvv;
		public int CodComune;
		public string Comune;
		public string CodVia;
		public string Via;
		public int NumeroCivico;
		public string EspCivico;
		public string Scala;
		public string Interno;
		public string Piano;
		public string Barrato;
		public int NumeroEcografico;
		//public bool TitoloAcquisto;
		//public bool TitoloCessione;
		public int TitoloAcquisto;
		public int TitoloCessione;
		public string DescrUffRegistro;
		public DateTime DataInizioValidità;
		public DateTime DataFineValidità;
		public bool Bonificato;
		public bool Annullato;
		public DateTime DataUltimaModifica;
		public string Operatore;
		public DateTime DataInizio;
		public DateTime	DataFine;
		public int IDImmobilePertinente;
		public string NoteIci;
		public string Zona;
		public decimal Rendita;
		public decimal Consistenza;
		public bool ExRurale;
	}
    */
    /// <summary>
    /// Classe di gestione della tabella TblOggetti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class OggettiTable : Database
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(OggettiTable));
		private string _username;

		public OggettiTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblOggetti";
		}

        /*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="idTestata"></param>
		/// <param name="numeroOrdine"></param>
		/// <param name="numeroModello"></param>
		/// <param name="codUI"></param>
		/// <param name="tipoImmobile"></param>
		/// <param name="partitaCatastale"></param>
		/// <param name="foglio"></param>
		/// <param name="numero"></param>
		/// <param name="subalterno"></param>
		/// <param name="caratteristica"></param>
		/// <param name="sezione"></param>
		/// <param name="numeroProtCatastale"></param>
		/// <param name="annoDenunciaCatastale"></param>
		/// <param name="codCategoriaCatastale"></param>
		/// <param name="codClasse"></param>
		/// <param name="codRendita"></param>
		/// <param name="storico"></param>
		/// <param name="valoreImmobile"></param>
		/// <param name="idValuta"></param>
		/// <param name="flagValoreProvv"></param>
		/// <param name="codComune"></param>
		/// <param name="comune"></param>
		/// <param name="codVia"></param>
		/// <param name="via"></param>
		/// <param name="numeroCivico"></param>
		/// <param name="espCivico"></param>
		/// <param name="scala"></param>
		/// <param name="interno"></param>
		/// <param name="piano"></param>
		/// <param name="barrato"></param>
		/// <param name="numeroEcografico"></param>
		/// <param name="titoloAcquisto"></param>
		/// <param name="titoloCessione"></param>
		/// <param name="descrUffRegistro"></param>
		/// <param name="dataInizioValidità"></param>
		/// <param name="dataFineValidità"></param>
		/// <param name="DataInizio"></param>
		/// <param name="DataFine"></param>
		/// <param name="IDImmobilePertinente"></param>
		/// <param name="bonificato"></param>
		/// <param name="annullato"></param>
		/// <param name="dataUltimaModifica"></param>
		/// <param name="operatore"></param>
		/// <param name="NoteIci"></param>
		/// <param name="zona"></param>
		/// <param name="rendita"></param>
		/// <param name="consistenza"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public bool Insert(string ente, int idTestata,
			string numeroOrdine, string numeroModello, int codUI, int tipoImmobile, int partitaCatastale,
			string foglio, string numero, int subalterno, int caratteristica, string sezione, string numeroProtCatastale,
			string annoDenunciaCatastale, string codCategoriaCatastale, string codClasse, string codRendita, bool storico,
			//float valoreImmobile, int idValuta, bool flagValoreProvv, int codComune, string comune, string codVia, string via, int numeroCivico,
			decimal valoreImmobile, int idValuta, bool flagValoreProvv, int codComune, string comune, string codVia, string via, int numeroCivico,
			string espCivico, string scala, string interno, string piano, string barrato, int numeroEcografico,
			//bool titoloAcquisto, bool titoloCessione, string descrUffRegistro, DateTime dataInizioValidità, DateTime dataFineValidità,
			int titoloAcquisto, int titoloCessione, string descrUffRegistro, DateTime dataInizioValidità, DateTime dataFineValidità, DateTime DataInizio, DateTime DataFine, int IDImmobilePertinente,
			bool bonificato, bool annullato, DateTime dataUltimaModifica, string operatore, string NoteIci, string zona, decimal rendita, decimal consistenza, bool ExRurale,out int idOggetto)
		{	
			string test;
			
			SqlCommand InsertCommand = new SqlCommand();
            try{
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, IdTestata, " +
				"NumeroOrdine, NumeroModello, CodUI, TipoImmobile, PartitaCatastale, Foglio, Numero, Subalterno, Caratteristica, " +
				"Sezione, NumeroProtCatastale, AnnoDenunciaCatastale, CodCategoriaCatastale, CodClasse, CodRendita, " +
				"Storico, ValoreImmobile, IDValuta, FlagValoreProvv, CodComune, Comune, CodVia, Via, NumeroCivico, EspCivico, Scala, Interno, " +
				"Piano, Barrato, NumeroEcografico, TitoloAcquisto, TitoloCessione, DescrUffRegistro, DataInizioValidità, " +
				"DataFineValidità, DataInizio, DataFine, IDImmobilePertinente,Bonificato, Annullato, DataUltimaModifica, Operatore, NoteIci, Zona, Rendita, Consistenza, ExRurale) VALUES (@ente, @idTestata, " +
				"@numeroOrdine, @numeroModello, @codUI, @tipoImmobile, " +
				"@partitaCatastale, @foglio, @numero, @subalterno, @caratteristica, @sezione, @numeroProtCatastale, " +
				"@annoDenunciaCatastale, @codCategoriaCatastale, @codClasse, @codRendita, @storico, @valoreImmobile, " +
				"@idValuta, @flagValoreProvv, @codComune, @comune, @codVia, @via, @numeroCivico, @espCivico, @scala, @interno, @piano, @barrato, " +
				"@numeroEcografico, @titoloAcquisto, @titoloCessione, @descrUffRegistro, @dataInizioValidità, " +
				"@dataFineValidità, @dataInizio, @dataFine, @idImmobilePertinente, @bonificato, @annullato, @dataUltimaModifica, @operatore, @noteIci, @zona, @rendita, @consistenza, @ExRurale)";
		
			test=InsertCommand.CommandText+ "::";
			InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			test+="'"+ente.ToString()+"'";
 
			InsertCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			test=test + "," + idTestata.ToString();
			InsertCommand.Parameters.Add("@numeroOrdine",SqlDbType.VarChar).Value = numeroOrdine;
			test=test + ",'" + numeroOrdine.ToString() + "'";
			InsertCommand.Parameters.Add("@numeroModello",SqlDbType.VarChar).Value = numeroModello;
			test=test + ",'" + numeroModello.ToString() + "'";
			InsertCommand.Parameters.Add("@codUI",SqlDbType.Int).Value = codUI;
			test=test + "," + codUI.ToString();
			if (tipoImmobile == 0)
			{
				InsertCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = DBNull.Value;
			}
			else
			{
				InsertCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = (object)tipoImmobile;
			}
			//InsertCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = tipoImmobile == 0 ? DBNull.Value : (object)tipoImmobile;
			test=test + "," + tipoImmobile.ToString();
			InsertCommand.Parameters.Add("@partitaCatastale",SqlDbType.Int).Value = partitaCatastale;
			test=test + "," + partitaCatastale.ToString();
			InsertCommand.Parameters.Add("@foglio",SqlDbType.VarChar).Value = foglio;
			test=test + ",'" + foglio.ToString()+"'";
			InsertCommand.Parameters.Add("@numero",SqlDbType.VarChar).Value = numero;
			test=test + ",'" + numero.ToString()+"'";
			InsertCommand.Parameters.Add("@subalterno",SqlDbType.Int).Value = subalterno;
			test=test + "," + subalterno.ToString();
			InsertCommand.Parameters.Add("@caratteristica",SqlDbType.Int).Value = caratteristica;
			test=test + "," + caratteristica.ToString();
			InsertCommand.Parameters.Add("@sezione",SqlDbType.VarChar).Value = sezione;
			test=test + ",'" + sezione.ToString()+"'";
			InsertCommand.Parameters.Add("@numeroProtCatastale",SqlDbType.VarChar).Value = numeroProtCatastale;
			test=test + ",'" + numeroProtCatastale.ToString()+"'";
			InsertCommand.Parameters.Add("@annoDenunciaCatastale",SqlDbType.VarChar).Value = annoDenunciaCatastale;
			test=test + ",'" + annoDenunciaCatastale.ToString()+"'";
			if (codCategoriaCatastale == "0")
			{
				InsertCommand.Parameters.Add("@codCategoriaCatastale",SqlDbType.VarChar).Value = DBNull.Value;	
			}
			else
			{
				InsertCommand.Parameters.Add("@codCategoriaCatastale",SqlDbType.VarChar).Value = (object)codCategoriaCatastale;
			}
			//InsertCommand.Parameters.Add("@codCategoriaCatastale",SqlDbType.VarChar).Value = codCategoriaCatastale == "0" ? DBNull.Value : (object)codCategoriaCatastale;
			test=test + ",'" + codCategoriaCatastale+"'";
			if (codClasse == "0")
			{
				InsertCommand.Parameters.Add("@codClasse",SqlDbType.VarChar).Value = DBNull.Value;
			}
			else
			{
				InsertCommand.Parameters.Add("@codClasse",SqlDbType.VarChar).Value = (object)codClasse;
			}

			//InsertCommand.Parameters.Add("@codClasse",SqlDbType.VarChar).Value = codClasse == "0" ? DBNull.Value : (object)codClasse;
			test=test + ",'" +  codClasse + "'";
			InsertCommand.Parameters.Add("@codRendita",SqlDbType.VarChar).Value = codRendita;

			test=test + ",'" + codRendita.ToString()+"'";
			if (storico == true){
				InsertCommand.Parameters.Add("@storico",SqlDbType.Bit).Value = 1;
			}else{
				InsertCommand.Parameters.Add("@storico",SqlDbType.Bit).Value = 0;
			}
			test=test + "," + storico.ToString();
			InsertCommand.Parameters.Add("@valoreImmobile",SqlDbType.Decimal).Value = valoreImmobile;
			test=test + "," + valoreImmobile.ToString();
			//InsertCommand.Parameters.Add("@valoreImmobile",SqlDbType.Float).Value = valoreImmobile;
			InsertCommand.Parameters.Add("@idValuta",SqlDbType.Int).Value = idValuta;
			test=test + "," + idValuta.ToString();
			if (flagValoreProvv == true){
				InsertCommand.Parameters.Add("@flagValoreProvv",SqlDbType.Bit).Value = 1;
			}else{
				InsertCommand.Parameters.Add("@flagValoreProvv",SqlDbType.Bit).Value = 0;
			}
			test=test + "," + flagValoreProvv.ToString();
			InsertCommand.Parameters.Add("@codComune",SqlDbType.Int).Value = codComune;
			test=test + "," + codComune.ToString();
			InsertCommand.Parameters.Add("@comune",SqlDbType.VarChar).Value = comune;
			test=test + ",'" + comune.ToString()+"'";
			InsertCommand.Parameters.Add("@codVia",SqlDbType.VarChar).Value = codVia;
			test=test + ",'" + codVia.ToString()+"'";
			InsertCommand.Parameters.Add("@via",SqlDbType.VarChar).Value = via;
			test=test + ",'" + via.ToString()+"'";
			InsertCommand.Parameters.Add("@numeroCivico",SqlDbType.Int).Value = numeroCivico;
			test=test + "," + numeroCivico.ToString();
			InsertCommand.Parameters.Add("@espCivico",SqlDbType.VarChar).Value = espCivico;
			test=test + ",'" + espCivico.ToString()+"'";
			InsertCommand.Parameters.Add("@scala",SqlDbType.VarChar).Value = scala;
			test=test + ",'" + scala.ToString()+"'";
			InsertCommand.Parameters.Add("@interno",SqlDbType.VarChar).Value = interno;
			test=test + ",'" + interno.ToString()+"'";
			InsertCommand.Parameters.Add("@piano",SqlDbType.VarChar).Value = piano;
			test=test + ",'" + piano.ToString()+"'";
			InsertCommand.Parameters.Add("@barrato",SqlDbType.VarChar).Value = barrato;
			test=test + ",'" + barrato.ToString()+"'";
			InsertCommand.Parameters.Add("@numeroEcografico",SqlDbType.Int).Value = numeroEcografico;
			test=test + "," + numeroEcografico.ToString();
			
			//if (titoloAcquisto == true){
				InsertCommand.Parameters.Add("@titoloAcquisto",SqlDbType.Int).Value = titoloAcquisto;
			//}else{
				//InsertCommand.Parameters.Add("@titoloAcquisto",SqlDbType.Bit).Value = 0;
			//}

			test=test + "," + titoloAcquisto.ToString();
			
			//if (titoloCessione == true){
				InsertCommand.Parameters.Add("@titoloCessione",SqlDbType.Int).Value = titoloCessione;
			//}else{
			//	InsertCommand.Parameters.Add("@titoloCessione",SqlDbType.Bit).Value = 0;
			//}

			test=test + "," + titoloCessione.ToString();
			InsertCommand.Parameters.Add("@descrUffRegistro",SqlDbType.VarChar).Value = descrUffRegistro;
			test=test + ",'" + descrUffRegistro.ToString()+"'";
			InsertCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
			test=test + ",#" + dataInizioValidità.ToString()+"#";
			InsertCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
			if (dataFineValidità == DateTime.MinValue)
			{
				test=test + ",#Null#";
			}
			else
			{
				test=test + ",#" + dataFineValidità.ToString()+"#";
			}

			InsertCommand.Parameters.Add("@dataInizio", SqlDbType.DateTime).Value = DataInizio;
			test=test + ",#" + DataInizio.ToString()+"#";

			InsertCommand.Parameters.Add("@dataFine", SqlDbType.DateTime).Value = DataFine;
			test=test + ",#" + DataFine.ToString()+"#";

			InsertCommand.Parameters.Add("@idImmobilePertinente", SqlDbType.Int).Value = IDImmobilePertinente;
			test=test + "," + IDImmobilePertinente.ToString();

			if (bonificato == true)
			{
				InsertCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = 1;
			}else{
				InsertCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = 0;
			}

			test=test + "," + bonificato.ToString();
			
			if (annullato == true){
				InsertCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = 1;
			}else{
				InsertCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = 0;
			}
			
			test=test + "," + annullato.ToString();
			InsertCommand.Parameters.Add("@dataUltimaModifica",SqlDbType.DateTime).Value = dataUltimaModifica == DateTime.MinValue ? DBNull.Value : (object)dataUltimaModifica;
			test=test + ",#" + dataUltimaModifica.ToString()+"#";
			InsertCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
			
			test=test + ",'" + operatore.ToString()+"'";

			InsertCommand.Parameters.Add("@noteIci", SqlDbType.NVarChar).Value = NoteIci;
			test=test + ",'" + NoteIci.ToString()+"'";

			InsertCommand.Parameters.Add("@zona", SqlDbType.NVarChar).Value = zona;
			test=test + ",'" + zona.ToString()+"'";
			InsertCommand.Parameters.Add("@rendita", SqlDbType.Float).Value = float.Parse(rendita.ToString());
			test=test + ",'" + rendita.ToString()+"'";
			InsertCommand.Parameters.Add("@consistenza", SqlDbType.Float).Value = float.Parse(consistenza.ToString());
			test=test + ",'" + consistenza.ToString()+"'";
			if (ExRurale  == true)
			{
				InsertCommand.Parameters.Add("@ExRurale",SqlDbType.Bit).Value = 1;
			}
			else
			{
				InsertCommand.Parameters.Add("@ExRurale",SqlDbType.Bit).Value = 0;
			}
			test=test + "," + ExRurale.ToString();
			
			log.Debug("OggettiTable::Insert::SQL::" + test);
			bool ciccio;
            ciccio = Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection), out idOggetto);
			log.Debug("esito query::" + ciccio.ToString());
			return ciccio;
             }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.Insert.errore: ", Err);
            }

		}

		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="idOgetto"></param>
		/// <returns></returns>
		public bool Insert(OggettiRow Item, out int idOgetto)
		{
			return Insert(Item.Ente, Item.IdTestata,
				Item.NumeroOrdine, Item.NumeroModello, Item.CodUI, Item.TipoImmobile, Item.PartitaCatastale,
				Item.Foglio, Item.Numero, Item.Subalterno, Item.Caratteristica, Item.Sezione,
				Item.NumeroProtCatastale, Item.AnnoDenunciaCatastale, Item.CodCategoriaCatastale, Item.CodClasse,
				Item.CodRendita, Item.Storico, Item.ValoreImmobile , Item.IDValuta, Item.FlagValoreProvv, Item.CodComune,
				Item.Comune, Item.CodVia, Item.Via, Item.NumeroCivico, Item.EspCivico, Item.Scala, Item.Interno, Item.Piano,
				Item.Barrato, Item.NumeroEcografico, Item.TitoloAcquisto, Item.TitoloCessione, Item.DescrUffRegistro,
				Item.DataInizioValidità, Item.DataFineValidità, Item.DataInizio, Item.DataFine, Item.IDImmobilePertinente, Item.Bonificato, Item.Annullato,
				Item.DataUltimaModifica, Item.Operatore, Item.NoteIci, Item.Zona, Item.Rendita, Item.Consistenza, Item.ExRurale , out idOgetto);
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="ente"></param>
		/// <param name="idTestata"></param>
		/// <param name="numeroOrdine"></param>
		/// <param name="numeroModello"></param>
		/// <param name="codUI"></param>
		/// <param name="tipoImmobile"></param>
		/// <param name="partitaCatastale"></param>
		/// <param name="foglio"></param>
		/// <param name="numero"></param>
		/// <param name="subalterno"></param>
		/// <param name="caratteristica"></param>
		/// <param name="sezione"></param>
		/// <param name="numeroProtCatastale"></param>
		/// <param name="annoDenunciaCatastale"></param>
		/// <param name="codCategoriaCatastale"></param>
		/// <param name="codClasse"></param>
		/// <param name="codRendita"></param>
		/// <param name="storico"></param>
		/// <param name="valoreImmobile"></param>
		/// <param name="idValuta"></param>
		/// <param name="flagValoreProvv"></param>
		/// <param name="codComune"></param>
		/// <param name="comune"></param>
		/// <param name="codVia"></param>
		/// <param name="via"></param>
		/// <param name="numeroCivico"></param>
		/// <param name="espCivico"></param>
		/// <param name="scala"></param>
		/// <param name="interno"></param>
		/// <param name="piano"></param>
		/// <param name="barrato"></param>
		/// <param name="numeroEcografico"></param>
		/// <param name="titoloAcquisto"></param>
		/// <param name="titoloCessione"></param>
		/// <param name="descrUffRegistro"></param>
		/// <param name="dataInizioValidità"></param>
		/// <param name="dataFineValidità"></param>
		/// <param name="dataInizio"></param>
		/// <param name="dataFine"></param>
		/// <param name="IDImmobilePertinente"></param>
		/// <param name="bonificato"></param>
		/// <param name="annullato"></param>
		/// <param name="dataUltimaModifica"></param>
		/// <param name="operatore"></param>
		/// <param name="NoteIci"></param>
		/// <param name="Zona"></param>
		/// <param name="Rendita"></param>
		/// <param name="Consistenza"></param>
		/// <returns></returns>
		public bool Modify(int id, string ente, int idTestata,
			string numeroOrdine, string numeroModello, int codUI, int tipoImmobile, int partitaCatastale,
			string foglio, string numero, int subalterno, int caratteristica, string sezione, string numeroProtCatastale,
			string annoDenunciaCatastale, string codCategoriaCatastale, string codClasse, string codRendita, bool storico,
			//float valoreImmobile, int idValuta, bool flagValoreProvv, int codComune, string comune, string codVia, string via, int numeroCivico,
			decimal valoreImmobile, int idValuta, bool flagValoreProvv, int codComune, string comune, string codVia, string via, int numeroCivico,
			string espCivico, string scala, string interno, string piano, string barrato, int numeroEcografico,
			int titoloAcquisto, int titoloCessione, string descrUffRegistro, DateTime dataInizioValidità, DateTime dataFineValidità, DateTime dataInizio, DateTime dataFine, int IDImmobilePertinente,
			//bool titoloAcquisto, bool titoloCessione, string descrUffRegistro, DateTime dataInizioValidità, DateTime dataFineValidità,
			bool bonificato, bool annullato, DateTime dataUltimaModifica, string operatore, string NoteIci, string Zona, decimal Rendita, decimal Consistenza,bool ExRurale)
		{
			SqlCommand ModifyCommand = new SqlCommand ();
            try{
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, IdTestata=@idTestata, " +
				"NumeroOrdine=@numeroOrdine, NumeroModello=@numeroModello, CodUI=@codUI, TipoImmobile=@tipoImmobile, " +
				"PartitaCatastale=@partitaCatastale, Foglio=@foglio, Numero=@numero, Subalterno=@subalterno, " +
				"Caratteristica=@caratteristica, Sezione=@sezione, NumeroProtCatastale=@numeroProtCatastale, " +
				"AnnoDenunciaCatastale=@annoDenunciaCatastale, CodCategoriaCatastale = @codCategoriaCatastale, " +
				"CodClasse=@codClasse, CodRendita=@codRendita, Storico=@storico, ValoreImmobile=@valoreImmobile, IDValuta=@idValuta, " +
				"FlagValoreProvv=@flagValoreProvv, CodComune=@codComune, Comune=@comune, CodVia=@codVia, Via=@via, NumeroCivico=@numeroCivico, " +
				"EspCivico=@espCivico, Scala=@scala, Interno=@interno, Piano=@piano, Barrato=@barrato, " +
				"NumeroEcografico=@numeroEcografico, TitoloAcquisto=@titoloAcquisto, TitoloCessione=@titoloCessione, " +
				"DescrUffRegistro=@descrUffRegistro, DataInizioValidità = @dataInizioValidità, " +
				"DataFineValidità=@dataFineValidità, DataInizio = @dataInizio, DataFine = @dataFine, IDImmobilePertinente = @idImmobilePertinente, Bonificato=@bonificato, Annullato=@annullato, " +
				"DataUltimaModifica=@dataUltimaModifica, Operatore=@operatore, NoteIci=@noteIci, Zona=@zona, Rendita=@rendita, Consistenza=@consistenza, ExRurale=@ExRurale WHERE ID=@id";
		
			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			ModifyCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			ModifyCommand.Parameters.Add("@numeroOrdine",SqlDbType.VarChar).Value = numeroOrdine;
			ModifyCommand.Parameters.Add("@numeroModello",SqlDbType.VarChar).Value = numeroModello;
			ModifyCommand.Parameters.Add("@codUI",SqlDbType.Int).Value = codUI;
			if (tipoImmobile == 0)
			{
				ModifyCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = DBNull.Value;
			}
			else
			{
				ModifyCommand.Parameters.Add("@tipoImmobile",SqlDbType.Int).Value = (object)tipoImmobile;
			}
			
			ModifyCommand.Parameters.Add("@partitaCatastale",SqlDbType.Int).Value = partitaCatastale;
			ModifyCommand.Parameters.Add("@foglio",SqlDbType.VarChar).Value = foglio;
			ModifyCommand.Parameters.Add("@numero",SqlDbType.VarChar).Value = numero;
			ModifyCommand.Parameters.Add("@subalterno",SqlDbType.Int).Value = subalterno;
			ModifyCommand.Parameters.Add("@caratteristica",SqlDbType.Int).Value = caratteristica;
			ModifyCommand.Parameters.Add("@sezione",SqlDbType.VarChar).Value = sezione;
			ModifyCommand.Parameters.Add("@numeroProtCatastale",SqlDbType.VarChar).Value = numeroProtCatastale;
			ModifyCommand.Parameters.Add("@annoDenunciaCatastale",SqlDbType.VarChar).Value = annoDenunciaCatastale;
			if (codCategoriaCatastale == "0")
			{
				ModifyCommand.Parameters.Add("@codCategoriaCatastale",SqlDbType.VarChar).Value = DBNull.Value;
			}
			else
			{
				ModifyCommand.Parameters.Add("@codCategoriaCatastale",SqlDbType.VarChar).Value = (object)codCategoriaCatastale;
			}
			if (codClasse == "0")
			{
				ModifyCommand.Parameters.Add("@codClasse",SqlDbType.VarChar).Value = DBNull.Value;			
			}
			else
			{
				ModifyCommand.Parameters.Add("@codClasse",SqlDbType.VarChar).Value = (object)codClasse;
			}
			
			ModifyCommand.Parameters.Add("@codRendita",SqlDbType.VarChar).Value = codRendita;
			ModifyCommand.Parameters.Add("@storico",SqlDbType.Bit).Value = storico;
			ModifyCommand.Parameters.Add("@valoreImmobile",SqlDbType.Decimal).Value = valoreImmobile;
			//ModifyCommand.Parameters.Add("@valoreImmobile",SqlDbType.Float).Value = valoreImmobile;
			ModifyCommand.Parameters.Add("@idValuta",SqlDbType.Int).Value = idValuta;
			ModifyCommand.Parameters.Add("@flagValoreProvv",SqlDbType.Bit).Value = flagValoreProvv;
			ModifyCommand.Parameters.Add("@codComune",SqlDbType.Int).Value = codComune;
			ModifyCommand.Parameters.Add("@comune",SqlDbType.VarChar).Value = comune;
			ModifyCommand.Parameters.Add("@codVia",SqlDbType.VarChar).Value = codVia;
			ModifyCommand.Parameters.Add("@via",SqlDbType.VarChar).Value = via;
			ModifyCommand.Parameters.Add("@numeroCivico",SqlDbType.Int).Value = numeroCivico;
			ModifyCommand.Parameters.Add("@espCivico",SqlDbType.VarChar).Value = espCivico;
			ModifyCommand.Parameters.Add("@scala",SqlDbType.VarChar).Value = scala;
			ModifyCommand.Parameters.Add("@interno",SqlDbType.VarChar).Value = interno;
			ModifyCommand.Parameters.Add("@piano",SqlDbType.VarChar).Value = piano;
			ModifyCommand.Parameters.Add("@barrato",SqlDbType.VarChar).Value = barrato;
			ModifyCommand.Parameters.Add("@numeroEcografico",SqlDbType.Int).Value = numeroEcografico;			
			ModifyCommand.Parameters.Add("@titoloAcquisto",SqlDbType.Int).Value = titoloAcquisto;
			ModifyCommand.Parameters.Add("@titoloCessione",SqlDbType.Int).Value = titoloCessione;
		//	ModifyCommand.Parameters.Add("@titoloAcquisto",SqlDbType.Bit).Value = titoloAcquisto;
		//	ModifyCommand.Parameters.Add("@titoloCessione",SqlDbType.Bit).Value = titoloCessione;
			ModifyCommand.Parameters.Add("@descrUffRegistro",SqlDbType.VarChar).Value = descrUffRegistro;
			ModifyCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
			ModifyCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
			ModifyCommand.Parameters.Add("@dataInizio", SqlDbType.DateTime).Value = dataInizio;
			ModifyCommand.Parameters.Add("@dataFine", SqlDbType.DateTime).Value = dataFine;
			ModifyCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
			ModifyCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
			ModifyCommand.Parameters.Add("@dataUltimaModifica",SqlDbType.DateTime).Value = dataUltimaModifica == DateTime.MinValue ? DBNull.Value : (object)dataUltimaModifica;
			ModifyCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
			ModifyCommand.Parameters.Add("@idImmobilePertinente", SqlDbType.Int).Value = IDImmobilePertinente;
			ModifyCommand.Parameters.Add("@noteIci", SqlDbType.NVarChar).Value = NoteIci;
			ModifyCommand.Parameters.Add("@zona", SqlDbType.NVarChar).Value=Zona;
			ModifyCommand.Parameters.Add("@rendita", SqlDbType.Decimal).Value = Rendita;
			ModifyCommand.Parameters.Add("@consistenza", SqlDbType.Decimal).Value = Consistenza;
			ModifyCommand.Parameters.Add("@ExRurale",SqlDbType.Bit).Value = ExRurale;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
             }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.Modify.errore: ", Err);
            }
		}
		
		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Modify(OggettiRow Item)
		{
			return Modify(Item.ID, Item.Ente, Item.IdTestata,
				Item.NumeroOrdine, Item.NumeroModello, Item.CodUI, Item.TipoImmobile, Item.PartitaCatastale,
				Item.Foglio, Item.Numero, Item.Subalterno, Item.Caratteristica, Item.Sezione, Item.NumeroProtCatastale,
				Item.AnnoDenunciaCatastale, Item.CodCategoriaCatastale, Item.CodClasse, Item.CodRendita,
				Item.Storico, Item.ValoreImmobile, Item.IDValuta, Item.FlagValoreProvv, Item.CodComune, Item.Comune, Item.CodVia,
				Item.Via, Item.NumeroCivico, Item.EspCivico, Item.Scala, Item.Interno, Item.Piano, Item.Barrato,
				Item.NumeroEcografico, Item.TitoloAcquisto, Item.TitoloCessione, Item.DescrUffRegistro,
				Item.DataInizioValidità, Item.DataFineValidità, Item.DataInizio, Item.DataFine, Item.IDImmobilePertinente,Item.Bonificato, Item.Annullato,
				Item.DataUltimaModifica, Item.Operatore, Item.NoteIci, Item.Zona, Item.Rendita, Item.Consistenza, Item.ExRurale );
		}
		*/
        /// <summary>
        /// Ritorna una struttura row che rappresenta un record individuato dall'identity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="myConn"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        public DichManagerICI.OggettiRow GetRow(int id, string myConn)
        {
            DichManagerICI.OggettiRow Oggetto = new DichManagerICI.OggettiRow();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, myConn))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TBLOGGETTI_S", "ID");

                    DataView dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", id));
                    foreach (DataRowView myRow in dvMyView)
                    {
                        Oggetto.ID = StringOperation.FormatInt(myRow["ID"]);
                        Oggetto.Ente = StringOperation.FormatString(myRow["Ente"]);
                        Oggetto.IdTestata = StringOperation.FormatInt(myRow["IdTestata"]);
                        Oggetto.NumeroOrdine = StringOperation.FormatString(myRow["NumeroOrdine"]);
                        Oggetto.NumeroModello = StringOperation.FormatString(myRow["NumeroModello"]);
                        Oggetto.CodUI = StringOperation.FormatString(myRow["CodUI"]);
                        Oggetto.TipoImmobile = StringOperation.FormatInt(myRow["TipoImmobile"]);
                        Oggetto.PartitaCatastale = StringOperation.FormatInt(myRow["PartitaCatastale"]);
                        Oggetto.Foglio = StringOperation.FormatString(myRow["Foglio"]);
                        Oggetto.Numero = StringOperation.FormatString(myRow["Numero"]);
                        Oggetto.Subalterno = StringOperation.FormatInt(myRow["Subalterno"]);
                        Oggetto.Caratteristica = StringOperation.FormatInt(myRow["Caratteristica"]);
                        Oggetto.Sezione = StringOperation.FormatString(myRow["Sezione"]);
                        Oggetto.NumeroProtCatastale = StringOperation.FormatString(myRow["NumeroProtCatastale"]);
                        Oggetto.AnnoDenunciaCatastale = StringOperation.FormatString(myRow["AnnoDenunciaCatastale"]);
                        Oggetto.CodCategoriaCatastale = StringOperation.FormatString(myRow["CodCategoriaCatastale"]);
                        Oggetto.CodClasse = StringOperation.FormatString(myRow["CodClasse"]);
                        Oggetto.CodRendita = StringOperation.FormatString(myRow["CodRendita"]);
                        Oggetto.Storico = StringOperation.FormatBool(myRow["Storico"]);
                        Oggetto.ValoreImmobile = Convert.ToDecimal(myRow["ValoreImmobile"].ToString());
                        Oggetto.IDValuta = StringOperation.FormatInt(myRow["IDValuta"]);
                        Oggetto.FlagValoreProvv = StringOperation.FormatBool(myRow["FlagValoreProvv"]);
                        Oggetto.CodComune = StringOperation.FormatInt(myRow["CodComune"]);
                        Oggetto.Comune = StringOperation.FormatString(myRow["Comune"]);
                        Oggetto.CodVia = StringOperation.FormatString(myRow["CodVia"]);
                        Oggetto.Via = StringOperation.FormatString(myRow["Via"]);
                        Oggetto.NumeroCivico = StringOperation.FormatInt(myRow["NumeroCivico"]);
                        Oggetto.EspCivico = StringOperation.FormatString(myRow["EspCivico"]);
                        Oggetto.Scala = StringOperation.FormatString(myRow["Scala"]);
                        Oggetto.Interno = StringOperation.FormatString(myRow["Interno"]);
                        Oggetto.Piano = StringOperation.FormatString(myRow["Piano"]);
                        Oggetto.Barrato = StringOperation.FormatString(myRow["Barrato"]);
                        Oggetto.NumeroEcografico = StringOperation.FormatInt(myRow["NumeroEcografico"]);
                        Oggetto.TitoloAcquisto = StringOperation.FormatInt(myRow["TitoloAcquisto"]);
                        Oggetto.TitoloCessione = StringOperation.FormatInt(myRow["TitoloCessione"]);
                        Oggetto.DescrUffRegistro = StringOperation.FormatString(myRow["DescrUffRegistro"]);
                        Oggetto.DataInizioValidità = StringOperation.FormatDateTime(myRow["DataInizioValidità"]);
                        Oggetto.DataFineValidità = StringOperation.FormatDateTime(myRow["DataFineValidità"]);
                        Oggetto.DataInizio = StringOperation.FormatDateTime(myRow["DataInizio"]);
                        Oggetto.DataFine = StringOperation.FormatDateTime(myRow["DataFine"]);
                        Oggetto.Bonificato = StringOperation.FormatBool(myRow["Bonificato"]);
                        Oggetto.Annullato = StringOperation.FormatBool(myRow["Annullato"]);
                        Oggetto.DataUltimaModifica = StringOperation.FormatDateTime(myRow["DataUltimaModifica"]);
                        Oggetto.Operatore = StringOperation.FormatString(myRow["Operatore"]);
                        Oggetto.IDImmobilePertinente = myRow["IdImmobilePertinente"] == DBNull.Value ? -1 : StringOperation.FormatInt(myRow["IdImmobilePertinente"]);
                        Oggetto.NoteIci = StringOperation.FormatString(myRow["NoteIci"]);
                        Oggetto.Zona = StringOperation.FormatString(myRow["Zona"]);
                        Oggetto.Rendita = myRow["Rendita"] == DBNull.Value ? -1 : decimal.Parse(myRow["Rendita"].ToString());
                        Oggetto.Consistenza = myRow["Consistenza"] == DBNull.Value ? -1 : decimal.Parse(myRow["Consistenza"].ToString());
                        Oggetto.ExRurale = StringOperation.FormatBool(myRow["ExRurale"]);
                    }
                    ctx.Dispose();
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.GetRow.errore: ", Err);

                kill();
                Oggetto = new Utility.DichManagerICI.OggettiRow();
            }
            finally
            {
                kill();
            }
            return Oggetto;
        }
        //      public Utility.DichManagerICI.OggettiRow GetRow(int id, string myConn)
        //{
        //          Utility.DichManagerICI.OggettiRow Oggetto = new Utility.DichManagerICI.OggettiRow();
        //	try
        //	{
        //		SqlCommand SelectCommand = PrepareGetRow(id);
        //		DataTable Oggetti = Query(SelectCommand,new SqlConnection(myConn));
        //		if(Oggetti.Rows.Count > 0)
        //		{
        //			Oggetto.ID = (int)Oggetti.Rows[0]["ID"];
        //			Oggetto.Ente = (string)Oggetti.Rows[0]["Ente"];
        //			Oggetto.IdTestata = (int)Oggetti.Rows[0]["IdTestata"];
        //			Oggetto.NumeroOrdine = (string)Oggetti.Rows[0]["NumeroOrdine"];
        //			Oggetto.NumeroModello = (string)Oggetti.Rows[0]["NumeroModello"];
        //			Oggetto.CodUI = Oggetti.Rows[0]["CodUI"] == DBNull.Value ? string.Empty : Oggetti.Rows[0]["CodUI"].ToString();
        //			Oggetto.TipoImmobile = Oggetti.Rows[0]["TipoImmobile"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["TipoImmobile"];
        //			Oggetto.PartitaCatastale = Oggetti.Rows[0]["PartitaCatastale"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["PartitaCatastale"];
        //			Oggetto.Foglio = Oggetti.Rows[0]["Foglio"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Foglio"];
        //			Oggetto.Numero = Oggetti.Rows[0]["Numero"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Numero"];
        //			Oggetto.Subalterno = Oggetti.Rows[0]["Subalterno"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["Subalterno"];
        //			Oggetto.Caratteristica = Oggetti.Rows[0]["Caratteristica"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["Caratteristica"];
        //			Oggetto.Sezione = Oggetti.Rows[0]["Sezione"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Sezione"];
        //			Oggetto.NumeroProtCatastale = Oggetti.Rows[0]["NumeroProtCatastale"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["NumeroProtCatastale"];
        //			Oggetto.AnnoDenunciaCatastale = Oggetti.Rows[0]["AnnoDenunciaCatastale"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["AnnoDenunciaCatastale"];
        //			Oggetto.CodCategoriaCatastale = Oggetti.Rows[0]["CodCategoriaCatastale"] == DBNull.Value ? "0" : (string)Oggetti.Rows[0]["CodCategoriaCatastale"];
        //			Oggetto.CodClasse = Oggetti.Rows[0]["CodClasse"] == DBNull.Value ? "0" : (string)Oggetti.Rows[0]["CodClasse"];
        //			Oggetto.CodRendita = (string)Oggetti.Rows[0]["CodRendita"];
        //			Oggetto.Storico = (bool)Oggetti.Rows[0]["Storico"];
        //			Oggetto.ValoreImmobile =Convert.ToDecimal(Oggetti.Rows[0]["ValoreImmobile"].ToString()); //float.Parse(Oggetti.Rows[0]["ValoreImmobile"].ToString());
        //			Oggetto.IDValuta = (int)Oggetti.Rows[0]["IDValuta"];
        //			Oggetto.FlagValoreProvv = (bool)Oggetti.Rows[0]["FlagValoreProvv"];
        //			Oggetto.CodComune = Oggetti.Rows[0]["CodComune"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["CodComune"];
        //			Oggetto.Comune = Oggetti.Rows[0]["Comune"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Comune"];
        //			Oggetto.CodVia = Oggetti.Rows[0]["CodVia"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["CodVia"];
        //			Oggetto.Via = Oggetti.Rows[0]["Via"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Via"];
        //			Oggetto.NumeroCivico = Oggetti.Rows[0]["NumeroCivico"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["NumeroCivico"];
        //			Oggetto.EspCivico = Oggetti.Rows[0]["EspCivico"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["EspCivico"];
        //			Oggetto.Scala = Oggetti.Rows[0]["Scala"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Scala"];
        //			Oggetto.Interno = Oggetti.Rows[0]["Interno"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Interno"];
        //			Oggetto.Piano = Oggetti.Rows[0]["Piano"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Piano"];
        //			Oggetto.Barrato = Oggetti.Rows[0]["Barrato"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["Barrato"];
        //			Oggetto.NumeroEcografico = Oggetti.Rows[0]["NumeroEcografico"] == DBNull.Value ? 0 : (int)Oggetti.Rows[0]["NumeroEcografico"];
        //			//Oggetto.TitoloAcquisto = (bool)Oggetti.Rows[0]["TitoloAcquisto"];
        //			//Oggetto.TitoloCessione = (bool)Oggetti.Rows[0]["TitoloCessione"];
        //			Oggetto.TitoloAcquisto = (int)Oggetti.Rows[0]["TitoloAcquisto"];
        //			Oggetto.TitoloCessione = (int)Oggetti.Rows[0]["TitoloCessione"];
        //			Oggetto.DescrUffRegistro = Oggetti.Rows[0]["DescrUffRegistro"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["DescrUffRegistro"];
        //			Oggetto.DataInizioValidità = (DateTime)Oggetti.Rows[0]["DataInizioValidità"];
        //			Oggetto.DataFineValidità = Oggetti.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : (DateTime)Oggetti.Rows[0]["DataFineValidità"];
        //			Oggetto.DataInizio = Oggetti.Rows[0]["DataInizio"] == DBNull.Value ? DateTime.MinValue : (DateTime)Oggetti.Rows[0]["DataInizio"];
        //			Oggetto.DataFine = Oggetti.Rows[0]["DataFine"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Oggetti.Rows[0]["DataFine"];
        //			Oggetto.Bonificato = (bool)Oggetti.Rows[0]["Bonificato"];
        //			Oggetto.Annullato = (bool)Oggetti.Rows[0]["Annullato"];
        //			Oggetto.DataUltimaModifica = Oggetti.Rows[0]["DataUltimaModifica"] == DBNull.Value? DateTime.MinValue : (DateTime)Oggetti.Rows[0]["DataUltimaModifica"];
        //			Oggetto.Operatore = (string)Oggetti.Rows[0]["Operatore"];
        //			Oggetto.IDImmobilePertinente = Oggetti.Rows[0]["IdImmobilePertinente"] == DBNull.Value ? -1 : (int)Oggetti.Rows[0]["IdImmobilePertinente"];
        //			Oggetto.NoteIci = Oggetti.Rows[0]["NoteIci"] == DBNull.Value ? String.Empty : (string)Oggetti.Rows[0]["NoteIci"];
        //			Oggetto.Zona = Oggetti.Rows[0]["Zona"] == DBNull.Value ? string.Empty : Oggetti.Rows[0]["Zona"].ToString();
        //			Oggetto.Rendita = Oggetti.Rows[0]["Rendita"] == DBNull.Value ? -1 : decimal.Parse(Oggetti.Rows[0]["Rendita"].ToString());
        //			Oggetto.Consistenza = Oggetti.Rows[0]["Consistenza"] == DBNull.Value ? -1 : decimal.Parse(Oggetti.Rows[0]["Consistenza"].ToString());
        //			Oggetto.ExRurale = (bool)Oggetti.Rows[0]["ExRurale"];
        //		}
        //	}
        //	catch(Exception Err)
        //	{
        //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.GetRow.errore: ", Err);

        //              kill();
        //              Oggetto = new Utility.DichManagerICI.OggettiRow();
        //	}
        //	finally{
        //		kill();
        //	}
        //	return Oggetto;	
        //}

        /// <summary>
        /// Prende la lista degli immobili appartenenti alla testata.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        public DataTable GetImmobileByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = " SELECT * FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Storico=0 AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;
			DataTable dt=Query(SelectCommand,new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.GetImmobileByIDTestata.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Prende la lista dell'immobile appartenente alla testata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="IdOggetto"></param>
		/// <returns></returns>
		public DataTable GetImmobileByIDTestata(int idTestata, int IdOggetto)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = " SELECT * FROM " + this.TableName +
				" WHERE ID=@IdOggetto AND IDTestata=@idTestata AND Storico=0 AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;
			SelectCommand.Parameters.Add("@IdOggetto", SqlDbType.Int).Value = IdOggetto;
            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.GetImmobileByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Calcola il numero degli immobili appartenenti a una testata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public object CountImmobiliByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Storico=0 AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;
            return QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.CountImmobiliByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Modifica soltanto l'id della testata (Per esempio dopo la storicizzazione).
		/// </summary>
		/// <param name="idTestataNuova"></param>
		/// <param name="idTestataVecchia"></param>
		/// <returns></returns>
		public bool Modify(int idTestataNuova, int idTestataVecchia)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET IDTestata=@idTestataNuova " +
				"WHERE IDTestata=@idTestataVecchia";
 
			ModifyCommand.Parameters.Add("@idTestataNuova", SqlDbType.Int).Value = idTestataNuova;
			ModifyCommand.Parameters.Add("@idTestataVecchia", SqlDbType.Int).Value = idTestataVecchia;
            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Storicizza i dati: modifica i dati vecchi.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Storicizzazione(int id)
		{
			SqlCommand StoricizzaCommand = new SqlCommand();
            try { 
			StoricizzaCommand.CommandText = "UPDATE " + this.TableName + " SET " +
				"DataFineValidità=@dataFineValidità WHERE ID=@id";

			StoricizzaCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			StoricizzaCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = DateTime.Now;

            return Execute(StoricizzaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.Storicizzazione.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Esegue la cancellazione logica dei dati.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool CancellazioneLogica(int id)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "UPDATE " + this.TableName + " SET " +
				"Annullato=1 WHERE ID=@id";

			DeleteCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.CancellazioneLogica.errore: ", Err);
                throw Err;
            }
        }
           
        /// <summary>
        /// Torna la quantità degli immobili già bonificati che appartengono a una certa dichiarazione.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        public object CountImmobiliBonificatiByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Bonificato=1 AND Storico=0 AND Annullato<>1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

            return QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.CountImmobiliBonificaByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna la quantità degli immobili non bonificati che appartengono a una certa dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public int CountImmobiliNonBonificatiByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Bonificato=0 AND Storico=0 AND Annullato<>1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

            return Convert.ToInt32(QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.CountImmobiliNonBonificaByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Imposta il flag bonificato a true.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public bool Bonifica(int idTestata)
		{
			SqlCommand BonificaCommand = new SqlCommand();
            try { 
			BonificaCommand.CommandText = "UPDATE " + this.TableName +
				" SET Bonificato=1 WHERE IDTestata=@idTestata";

			BonificaCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
            return Execute(BonificaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.Bonifica.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Imposta il flag bonificato a true
		/// </summary>
		/// <param name="idTestata">Id Testata</param>
		/// <param name="idOggetto">Id Oggetto</param>
		/// <returns></returns>
		public  bool Bonifica(int idTestata, int idOggetto)
		{
			SqlCommand BonificaCommand = new SqlCommand();
            try { 
			BonificaCommand.CommandText = "UPDATE " + this.TableName +
				" SET Bonificato=1 where IDTestata=@idTestata and ID=@idOggetto";

			BonificaCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;
			BonificaCommand.Parameters.Add("@idOggetto", SqlDbType.Int).Value = idOggetto;

            return Execute(BonificaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.Bonifica.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Imposta il flag bonificato a false
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool BonificaFalse(int id)
		{
			SqlCommand BonificaCommand = new SqlCommand();
            try { 
			BonificaCommand.CommandText = "UPDATE " + this.TableName +
				" SET Bonificato=0 WHERE ID=@id";

			BonificaCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
            return Execute(BonificaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.BonificaFalse.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Esegue la cancellazione degli immobili identificati dall'idTestata.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public bool DeleteItemByIDTestata(int idTestata)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IDTestata=@idTestata";

			DeleteCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;

            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.DeleteItemByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Creazione di una riga della tabella vuota.
		/// </summary>
		/// <param name="tabella"></param>
		/// <param name="codiceEnte"></param>
		/// <param name="idTestata"></param>
		/// <param name="operatore"></param>
		public static void AddEmptyRow(ref DataTable tabella, string codiceEnte, int idTestata, string operatore)
		{
            Utility.DichManagerICI.OggettiRow Item = new Utility.DichManagerICI.OggettiRow();
			Item.Ente = codiceEnte;
			Item.IdTestata = idTestata;
			Item.NumeroOrdine = "";
			Item.NumeroModello = "";
			Item.CodUI = "0";
			Item.TipoImmobile = 0;
			Item.PartitaCatastale = 0;
			Item.Foglio = "";
			Item.Numero = "";
			Item.Subalterno = 0;
			Item.Caratteristica = 1;
			Item.Sezione = "";
			Item.NumeroProtCatastale = "";
			Item.AnnoDenunciaCatastale = "";
			Item.CodCategoriaCatastale = "";
			Item.CodClasse = "";
			Item.CodRendita = "";
			Item.Storico = false;
			Item.ValoreImmobile = 0;
			Item.FlagValoreProvv = false;
			Item.CodComune = 0;
			Item.Comune = "";
			Item.CodVia = "";
			Item.Via = "";
			Item.NumeroCivico = 0;
			Item.EspCivico = "";
			Item.Scala = "";
			Item.Interno = "";
			Item.Piano = "";
			Item.Barrato = "";
			Item.NumeroEcografico = 0;
			//Item.TitoloAcquisto = false;
			//Item.TitoloCessione = false;
			Item.TitoloAcquisto = 0;
			Item.TitoloCessione = 0;
			Item.DescrUffRegistro = "";
			Item.DataInizioValidità = DateTime.Now;
			Item.DataFineValidità = DateTime.Now;
			Item.DataInizio = DateTime.Now;
			Item.DataFine = DateTime.MaxValue;
			Item.Bonificato = false;
			Item.Annullato = false;
			Item.DataUltimaModifica = DateTime.Now;
			Item.Operatore = operatore;
			Item.NoteIci = "";
			Item.ExRurale = false;

			DataRow Riga = tabella.NewRow();

			Riga["Ente"] = Item.Ente;
			Riga["IdTestata"] = Item.IdTestata;
			Riga["NumeroOrdine"] = Item.NumeroOrdine;
			Riga["NumeroModello"] = Item.NumeroModello;
			Riga["CodUI"] = Item.CodUI;
			Riga["TipoImmobile"] = Item.TipoImmobile;
			Riga["PartitaCatastale"] = Item.PartitaCatastale;
			Riga["Foglio"] = Item.Foglio;
			Riga["Numero"] = Item.Numero;
			Riga["Subalterno"] = Item.Subalterno;
			Riga["Caratteristica"] = Item.Caratteristica;
			Riga["Sezione"] = Item.Sezione;
			Riga["NumeroProtCatastale"] = Item.NumeroProtCatastale;
			Riga["AnnoDenunciaCatastale"] = Item.AnnoDenunciaCatastale;
			Riga["CodCategoriaCatastale"] = Item.CodCategoriaCatastale;
			Riga["CodClasse"] = Item.CodClasse;
			Riga["CodRendita"] = Item.CodRendita;
			Riga["Storico"] = Item.Storico;
			Riga["ValoreImmobile"] = Item.ValoreImmobile;
			Riga["FlagValoreProvv"] = Item.FlagValoreProvv;
			Riga["CodComune"] = Item.CodComune;
			Riga["Comune"] = Item.Comune;
			Riga["CodVia"] = Item.CodVia;
			Riga["Via"] = Item.Via;
			Riga["NumeroCivico"] = Item.NumeroCivico;
			Riga["EspCivico"] = Item.EspCivico;
			Riga["Scala"] = Item.Scala;
			Riga["Interno"] = Item.Interno;
			Riga["Piano"] = Item.Piano;
			Riga["Barrato"] = Item.Barrato;
			Riga["NumeroEcografico"] = Item.NumeroEcografico;
			Riga["TitoloAcquisto"] = Item.TitoloAcquisto;
			Riga["TitoloCessione"] = Item.TitoloCessione;
			Riga["DescrUffRegistro"] = Item.DescrUffRegistro;
			Riga["DataInizioValidità"] = Item.DataInizioValidità;
			Riga["DataFineValidità"] = Item.DataFineValidità;
			Riga["DataInizio"] = Item.DataInizio;
			Riga["DataFine"] = Item.DataFine;
			Riga["Bonificato"] = Item.Bonificato;
			Riga["Annullato"] = Item.Annullato;
			Riga["DataUltimaModifica"] = Item.DataUltimaModifica;
			Riga["Operatore"] = Item.Operatore;
			Riga["NoteIci"] = Item.NoteIci;
			Riga["ExRurale"] = Item.ExRurale;

			tabella.Rows.Add(Riga);
			return;
		}

        public void LoadDatiTARSU(string myStringConnection, string sIdEnte, string sFoglio, string sNumero, string sSubalterno, DateTime tDataInizio, DateTime tDataFine, Ribes.OPENgov.WebControls.RibesGridView grdTARSU, global::System.Web.UI.WebControls.ImageButton cmdNew)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            DataTable dtMyDati = new DataTable();
            DataView dvMyDati = new DataView();

            try
            {
                cmdMyCommand.Connection = new SqlConnection(myStringConnection);
                cmdMyCommand.Connection.Open();
                cmdMyCommand.CommandTimeout = 0;
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = "prc_GetUIFromTARSU";
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@IDENTE", SqlDbType.VarChar)).Value = sIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@FOGLIO", SqlDbType.VarChar)).Value = sFoglio;
                cmdMyCommand.Parameters.Add(new SqlParameter("@NUMERO", SqlDbType.VarChar)).Value = sNumero;
                cmdMyCommand.Parameters.Add(new SqlParameter("@SUB", SqlDbType.VarChar)).Value = sSubalterno;
                cmdMyCommand.Parameters.Add(new SqlParameter("@DAL", SqlDbType.DateTime)).Value = tDataInizio;
                cmdMyCommand.Parameters.Add(new SqlParameter("@AL", SqlDbType.DateTime)).Value = tDataFine;
                myAdapter.SelectCommand = cmdMyCommand;
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dtMyDati);
                dvMyDati = dtMyDati.DefaultView;
                myAdapter.Dispose();
                if (dvMyDati != null)
                {
                    if (dvMyDati.Count > 0)
                    {
                        grdTARSU.Style.Add("display", "");
                        cmdNew.Style.Add("display", "none");
                        grdTARSU.SelectedIndex = -1;
                        grdTARSU.DataSource = dvMyDati;
                        grdTARSU.DataBind();
                    }
                    else
                    {
                        grdTARSU.Style.Add("display", "none");
                        cmdNew.Style.Add("display", "");
                    }
                }
                else
                {
                    grdTARSU.Style.Add("display", "none");
                    cmdNew.Style.Add("display", "");
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.LoadDatiTARSU.errore: ", ex);
            
            }
            finally
            {
                dtMyDati.Dispose();
                cmdMyCommand.Dispose();
                cmdMyCommand.Connection.Close();
            }
        }

        //		public double CalcoloValore(string sUserName, string sIdEnte, int Anno, string TipoRendita, string Categoria, double Rendita, string Zona, double Consistenza, DateTime Dal) 
        //		{
        //			double RenditaRivalutata;
        //			double Valore=0;
        //			double nMoltiplicatore;
        //			DataTable dtTariffa;
        //			try 
        //			{
        //				if ((Anno >= 1997)) 
        //				{
        //					// La rivalutazione non deve avvenire per le aree fabbricabili e le categorie D da libri contabili
        //					switch (TipoRendita) 
        //					{
        //						case "AF":
        //						case "LC":
        //							RenditaRivalutata = Rendita;
        //							break;
        //						case "TA":
        //							RenditaRivalutata = (Rendita * 1.25);
        //							break;
        //						default:
        //							RenditaRivalutata = (Rendita * 1.05);
        //							break;
        //					}
        //				}
        //				else 
        //				{
        //					RenditaRivalutata = Rendita;
        //				}
        //				if (((TipoRendita == "AF") || (TipoRendita == "LC"))) 
        //				{
        //					dtTariffa = new DatabaseOpengov.TariffeEstimoAFTable(sUserName).SelectTariffa(sIdEnte, Zona, DateTime.Parse("01/01"+Anno));
        //					if ((dtTariffa.Rows.Count > 0)) 
        //					{
        //						Valore = (Consistenza * double.Parse(dtTariffa.Rows[0]["TARIFFA_EURO"].ToString()));
        //					}
        //					else 
        //					{
        //						Valore = RenditaRivalutata;
        //					}
        //				}
        //				else 
        //				{
        //					nMoltiplicatore = GetMoltiplicatoreRendita(TipoRendita, Categoria, Anno);
        //					if (Anno==2006)
        //					{
        //						if (Dal.Month>= 10)
        //						{
        //							nMoltiplicatore = 140;
        //						}
        //						else
        //						{
        //							nMoltiplicatore = 100;
        //						}
        //				}
        //					else
        //					{
        //						Valore = (RenditaRivalutata * nMoltiplicatore);
        //					}
        //				}
        //				return Valore;
        //			}
        //			catch (Exception ex) 
        //			{
        //				log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.CalcoloValore.errore: ", ex);
        //			}
        //		}
        //    
        //		private double GetMoltiplicatoreRendita(string TipoRendita, string Categoria, int Anno) 
        //		{
        //			SqlCommand SelectCommand = new SqlCommand();
        //			double nMoltiplicatore=0;
        //			DataTable dtMyDati;
        //			try 
        //			{
        //				//cerco moltiplicatore per tipo rendita e categoria se non lo trovo prendo moltiplicatore di default per anno(ovvero in tabella senza tipo rendita e categoria)
        //				SelectCommand.Parameters.Clear();
        //				SelectCommand.CommandText = "SELECT TOP 1 *";
        //				SelectCommand.CommandText += " FROM MOLTIPLICATORI_PER_CALCOLO_VALORE";
        //				SelectCommand.CommandText += " WHERE (1=1)";
        //				if (TipoRendita=="AF")
        //				{
        //					SelectCommand.CommandText += " AND (TIPO_RENDITA=@TipoRendita)";
        //					SelectCommand.Parameters.Add("@TipoRendita", SqlDbType.VarChar).Value = TipoRendita;
        //				}
        //				SelectCommand.CommandText += " AND (CATEGORIA=@Categoria)";
        //				SelectCommand.CommandText += " AND (ANNO<=@Anno)";
        //				SelectCommand.CommandText += " ORDER BY ANNO";
        //				SelectCommand.Parameters.Add("@Categoria", SqlDbType.DateTime).Value = Categoria;
        //				SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
        //				dtMyDati = Query(SelectCommand);
        //				if ((dtMyDati.Rows.Count > 0)) 
        //				{
        //					nMoltiplicatore = double.Parse(dtMyDati.Rows[0]["moltiplicatore"].ToString());
        //				}
        //				else
        //				{
        //					dtMyDati.Dispose();
        //					SelectCommand.Parameters.Clear();
        //					SelectCommand.CommandText = "SELECT TOP 1 *";
        //					SelectCommand.CommandText += " FROM MOLTIPLICATORI_PER_CALCOLO_VALORE";
        //					SelectCommand.CommandText += " WHERE (1=1)";
        //					SelectCommand.CommandText += " AND (ANNO<=@Anno)";
        //					SelectCommand.CommandText += " ORDER BY ANNO";
        //					SelectCommand.Parameters.Add("@Categoria", SqlDbType.DateTime).Value = Categoria;
        //					SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
        //					dtMyDati = Query(SelectCommand);
        //					if ((dtMyDati.Rows.Count > 0)) 
        //					{
        //						nMoltiplicatore = double.Parse(dtMyDati.Rows[0]["moltiplicatore"].ToString());
        //					}
        //				}
        //				dtMyDati.Dispose();
        //				return nMoltiplicatore;
        //			}
        //			catch (Exception ex) 
        //			{
        //				log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.GetMoltiplicatoreRendita.errore: ", ex);
        //			}
        //		}	
    }

    //*** 20130304 - gestione dati da territorio ***
    public class UITerritorio :Database
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(UITerritorio));

		public UITerritorio()
		{
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'identity.
		/// </summary>
		/// <param name="myConn"></param>
        /// <param name="sMyIdEnte"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="IdVia"></param>
        /// <param name="Foglio"></param>
        /// <param name="Numero"></param>
        /// <param name="Sub"></param>
        /// <param name="IdUI"></param>
        /// <param name="IdProprieta"></param>
        /// <param name="IdProprietario"></param>
		/// <returns></returns>
        public DataSet GetRow(string myConn, string sMyIdEnte, int IdContribuente, int IdVia, string Foglio, string Numero, string Sub, int IdUI, int IdProprieta, int IdProprietario)
        {
            DataSet myResult = new DataSet();
            SqlCommand cmdMyCommand = new SqlCommand();
            try
            {
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.CommandText = "prc_GetUI_ICI";
                cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@IdContribuente", SqlDbType.Int)).Value = IdContribuente;
                cmdMyCommand.Parameters.Add(new SqlParameter("@IdVia", SqlDbType.Int)).Value = IdVia;
                cmdMyCommand.Parameters.Add(new SqlParameter("@Foglio", SqlDbType.VarChar)).Value = Foglio;
                cmdMyCommand.Parameters.Add(new SqlParameter("@Numero", SqlDbType.VarChar)).Value = Numero;
                cmdMyCommand.Parameters.Add(new SqlParameter("@Sub", SqlDbType.VarChar)).Value = Sub;
                cmdMyCommand.Parameters.Add(new SqlParameter("@IdUI", SqlDbType.Int)).Value = IdUI;
                cmdMyCommand.Parameters.Add(new SqlParameter("@IdProprieta", SqlDbType.Int)).Value = IdProprieta;
                cmdMyCommand.Parameters.Add(new SqlParameter("@IdProprietario", SqlDbType.Int)).Value = IdProprietario;
                DataTable Oggetti = Query(cmdMyCommand, new SqlConnection(myConn));
                myResult = Oggetti.DataSet;
            }
            catch (Exception err)
            {
                kill();
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.UITerritorio.GetRow.errore: ", err);
                myResult = null;
            }
            finally
            {
                kill();
            }
            return myResult;
        }
        //public DataSet GetRow(OPENUtility.CreateSessione WFSession,string sMyIdEnte, int IdContribuente, int IdVia, string Foglio, string Numero, string Sub,int IdUI, int IdProprieta, int IdProprietario)
        //{
        //    DataSet myResult=new DataSet();
        //    SqlCommand cmdMyCommand= new SqlCommand();
        //    SqlDataAdapter myDaDati=new SqlDataAdapter();
        //    try
        //    {
        //        // Valorizzo la connessione
        //        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection();
        //        cmdMyCommand.CommandType=CommandType.StoredProcedure;
        //        //valorizzo i parameters
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.CommandText = "prc_GetUI_ICI";
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sMyIdEnte;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IdContribuente", SqlDbType.Int)).Value = IdContribuente;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IdVia", SqlDbType.Int)).Value = IdVia;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@Foglio", SqlDbType.VarChar)).Value = Foglio;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@Numero", SqlDbType.VarChar)).Value = Numero;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@Sub", SqlDbType.VarChar)).Value = Sub;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IdUI", SqlDbType.Int)).Value = IdUI;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IdProprieta", SqlDbType.Int)).Value = IdProprieta;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IdProprietario", SqlDbType.Int)).Value = IdProprietario;
        //        myDaDati = WFSession.oSession.oAppDB.GetPrivateDataAdapter(cmdMyCommand);
        //        myDaDati.Fill(myResult);
        //        cmdMyCommand.Dispose();

        //    }
        //    catch(Exception err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.OggettiTable.GetRow.errore: ", ex);
        //        myResult = null;
        //    }
        //    return myResult;	
        //}
    }
    //*** ***
}
							

