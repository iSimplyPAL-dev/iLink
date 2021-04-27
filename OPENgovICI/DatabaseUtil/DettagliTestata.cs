using System;
using System.Data.SqlClient;
using System.Data;
using log4net;

namespace DichiarazioniICI.Database
{

	/// <summary>
	/// Struct contenente ogni campo, appositamente tipato, 
	/// della vista ViewDettagliTestata del DB.
	/// </summary>
	#region DettagliTestataRow
	public struct DettagliTestataRow
	{
		public int IdDettaglio;
		public int IdTestata;
		public int IdOggetto;
		public int IdSoggetto;
		public int NumeroDichiarazione;
		public string AnnoDichiarazione;
		public string Ente;
		public string NumeroOrdine;
		public string NumeroModello;
		public float PercPossesso;
		public int MesiPossesso;
		public int MesiEsclusioneEserc;
		public int MesiRiduzione;
		public float ImpDetrazAbitazPrincipale;
		public bool Contitolare;
		public int Possesso;
		public int AbitazionePrincipale;
		public int Riduzione;
		public int EsclusoEsercizio;
		// campi appartenenti alla tab. TblOggetti
		public int TipoOggetto;
		public string PartitaCatastale;		
		public string Foglio;
		public string Numero;
		public int Subalterno;
		public int Caratteristica;
		public string Sezione;
		public string NumeroProtocolloCat;
		public string AnnoDenunciaCat;
		public string Categoria;
		public string Classe;
		public bool Storico;
		public double ValoreImmobile;
		public string Indirizzo;
		public bool ValoreProvvisorio;
		public int CodComune;
		public int CodVia;
		public int NumeroCivico;
		public string EspCivico;
		public string Scala;
		public string Interno;
		public string Piano;
		public string Barrato;
		public int TitoloAcquisto;
		public int TitoloCessione;
		public string DescrUffRegistro;
		// dati soggetto
		public string Cognome; 
		public string Nome; 
		public string CodiceFiscale; 
		public string PartitaIva; 
		public string ComuneNascita; 
		public string ProvNascita; 
		public string DataNascita; 
		public string Sex; 
		public string ComuneResid; 
		public string ProvResid; 
		public int CAPResid; 
		public string IndirizzoSoggetto; 
		public int NumCivicoSoggetto; 
		public string EspCivicoSoggetto; 
		public string ScalaSoggetto; 
		public string IntSoggetto; 
		public string PianoSoggetto; 
		public string BarratoSoggetto;
	}

	#endregion

	/// <summary>
	/// Classe che interfaccia la vista ViewDettagliTestata del DB. 
	/// Tale vista mette in correlazione un dettaglio, un immobile, un soggetto;
	/// L'IdSoggetto o è quello del contribuente o quello del contitolare.
	/// </summary>
	public class DettagliTestata : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(DettagliTestata));

        /// <summary>
        /// Inizializza la vista gestita
        /// </summary>
        public DettagliTestata()
		{
			this.TableName = "ViewDettagliTestate";
		}
		
		/// <summary>
		/// Seleziona tutti i record della vista con l'identificativo di testata e 
		/// l'identificativo di contribuente passati.
		/// In sostanza vengono recuperati tutti i dettagli di una determinata 
		/// testata il cui soggetto è il contribuente.
		/// </summary>
		/// <param name="idTestata">Identificativo della testata.</param>
		/// <param name="idSoggetto">Identificativo del soggetto presente nel dettaglio.</param>
		/// <param name="soloValidi">I soli record validi se true, tutti i record altrimenti.</param>
		/// <returns>DataTable con tutti i record trovati.</returns>
		public DataTable ListPerSoggetto(int idTestata, int idSoggetto, bool soloValidi)
		{
			DataTable Tabella;

			try
			{
				string selectStatem = "Select * From " + this.TableName + " where IdTestata=@IdTestata and IdSoggetto = @IdSoggetto";
				SqlCommand selectCommand = new SqlCommand();
				selectCommand.Connection=  this._DbConnection; 
				if (soloValidi)
				{
					selectStatem += " and DataFineDettaglio='' ";			
				}
				selectCommand.CommandText = selectStatem;
				selectCommand.Parameters.Add(new SqlParameter( "@IdTestata",SqlDbType.Int)).Value = idTestata;
				selectCommand.Parameters.Add(new SqlParameter( "@IdSoggetto",SqlDbType.Int)).Value = idSoggetto;
				Tabella = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliTestata.ListPerSoggetto.errore: ", Err);
                kill();
				Tabella = new DataTable();
			}
			finally{
				kill();
			}
			return Tabella;
		}


		/// <summary>
		/// Seleziona tutti i record della vista con l'identificativo di testata e
		/// quello di immobile passati.
		/// In particolare, seleziona tutti quei dettagli in cui il soggetto non è
		/// il contribuente ma un contitolare.
		/// </summary>
 		/// <param name="idTestata">Identificativo della testata.</param>
		/// <param name="idOggetto">Identificativo dell'immobile</param>
		/// <param name="idSoggetto">Identificativo del contribuente della testata.</param>
		/// <param name="soloValidi">I soli record validi se true, tutti i record altrimenti.</param>
		/// <returns>DataTable con tutti i record trovati.</returns>
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
					selectStatem += " and DataFineDettaglio='' ";			
					//selectCommand.Parameters.Add(new SqlParameter( "@DataFineValidita",SqlDbType.NVarChar)).Value =  "''";					
				}
				selectCommand.CommandText = selectStatem;
				selectCommand.Parameters.Add(new SqlParameter( "@IdTestata",SqlDbType.Int)).Value = idTestata;
				selectCommand.Parameters.Add(new SqlParameter( "@IdOggetto",SqlDbType.Int)).Value = idOggetto;
				selectCommand.Parameters.Add(new SqlParameter( "@IdSoggetto",SqlDbType.Int)).Value = idSoggetto;
				Tabella = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliTestata.ListPerOggetto.errore: ", Err);

                kill();
				Tabella = new DataTable();
			}
			finally
			{
				kill();
			}

			return Tabella;
		}


		#region Add EmptyRow
		/// <summary>
		/// Aggiunge alla tabella passata una nuova riga settata a valori di default.
		/// </summary>
		/// <param name="table">Reference alla tabella a cui viene 
		/// aggiunta la riga vuota di default.</param>
		public static void AddEmptyRow(ref DataTable table)
		{
			DettagliTestataRow item = new DettagliTestataRow();
			item.IdDettaglio = 0;
			item.IdTestata = 0;
			item.IdOggetto = 0;
			item.IdSoggetto = 0;
			item.NumeroDichiarazione = 0; 
			item.AnnoDichiarazione = ""; 
			item.Ente = ""; 
			item.NumeroOrdine = ""; 
			item.NumeroModello = ""; 
			item.PercPossesso = 0; 
			item.MesiPossesso = 0;
			item.MesiEsclusioneEserc = 0; 
			item.MesiRiduzione = 0; 
			item.ImpDetrazAbitazPrincipale = 0; 
			item.Contitolare = false; 
			item.Possesso = 0; 
			item.AbitazionePrincipale = 0;  
			item.Riduzione = 0; 
			item.EsclusoEsercizio = 0; 
			item.TipoOggetto = 0;
			item.PartitaCatastale = "";  
			item.Foglio = "";  
			item.Numero = ""; 
			item.Subalterno= 0;
			item.Caratteristica= 1;
			item.Sezione = "";  
			item.NumeroProtocolloCat = "";  
			item.AnnoDenunciaCat = "";  
			item.Categoria = "";  
			item.Classe = "";  
			item.Storico = false; 
			item.ValoreImmobile = 0;
			item.Indirizzo = "";  
			item.ValoreProvvisorio = false; 
			item.CodComune = 0; 
			item.CodVia = 0; 
			item.NumeroCivico = 0; 
			item.EspCivico = "";  
			item.Scala = "";  
			item.Interno = "";  
			item.Piano = "";  
			item.Barrato = "";  
			item.TitoloAcquisto = 0; 
			item.TitoloCessione = 0; 
			item.DescrUffRegistro = "";
 			item.Cognome = ""; 
			item.Nome = ""; 
			item.CodiceFiscale = ""; 
			item.PartitaIva = ""; 
			item.ComuneNascita = ""; 
			item.ProvNascita = ""; 
			item.DataNascita = ""; 
			item.Sex = "M"; 
			item.ComuneResid = ""; 
			item.ProvResid = ""; 
			item.CAPResid = 0; 
			item.IndirizzoSoggetto = ""; 
			item.NumCivicoSoggetto = 0; 
			item.EspCivicoSoggetto = ""; 
			item.ScalaSoggetto = ""; 
			item.IntSoggetto = ""; 
			item.PianoSoggetto = ""; 
			item.BarratoSoggetto = "";

            DataRow row = table.NewRow();

			row["IdDettaglio"] = item.IdDettaglio;
			row["IdTestata"] = item.IdTestata;
			row["IdOggetto"] = item.IdOggetto;
			row["IdSoggetto"] = item.IdSoggetto;
			row["NumeroDichiarazione"] = item.NumeroDichiarazione; 
			row["AnnoDichiarazione"] = item.AnnoDichiarazione; 
			row["Ente"] = item.Ente; 
			row["NumeroOrdine"] = item.NumeroOrdine; 
			row["NumeroModello"] = item.NumeroModello; 
			row["PercPossesso"] = item.PercPossesso; 
			row["MesiPossesso"] = item.MesiPossesso;
			row["MesiEsclusioneEserc"] = item.MesiEsclusioneEserc; 
			row["MesiRiduzione"] = item.MesiRiduzione; 
			row["ImpDetrazAbitazPrincipale"] = item.ImpDetrazAbitazPrincipale; 
			row["Contitolare"] = item.Contitolare; 
			row["Possesso"] = item.Possesso; 
			row["AbitazionePrincipale"] = item.AbitazionePrincipale;  
			row["Riduzione"] = item.Riduzione; 
			row["EsclusoEsercizio"] = item.EsclusoEsercizio; 
			row["TipoOggetto"] = item.TipoOggetto;
			row["PartitaCatastale"] = item.PartitaCatastale;  
			row["Foglio"] = item.Foglio;  
			row["Numero"] = item.Numero; 
			row["Subalterno"] = item.Subalterno;
			row["Caratteristica"] = item.Caratteristica;
			row["Sezione"] = item.Sezione;  
			row["NumeroProtocolloCat"] = item.NumeroProtocolloCat;  
			row["AnnoDenunciaCat"] = item.AnnoDenunciaCat;  
			row["Categoria"] = item.Categoria;  
			row["Classe"] = item.Classe;  
			row["Storico"] = item.Storico; 
			row["ValoreImmobile"] = item.ValoreImmobile;
			row["Indirizzo"] = item.Indirizzo;  
			row["ValoreProvvisorio"] = item.ValoreProvvisorio; 
			row["CodComune"] = item.CodComune; 
			row["CodVia"] = item.CodVia; 
			row["NumeroCivico"] = item.NumeroCivico; 
			row["EspCivico"] = item.EspCivico;  
			row["Scala"] = item.Scala;  
			row["Interno"] = item.Interno;  
			row["Piano"] = item.Piano;  
			row["Barrato"] = item.Barrato;  
			row["TitoloAcquisto"] = item.TitoloAcquisto; 
			row["TitoloCessione"] = item.TitoloCessione; 
			row["DescrUffRegistro"] = item.DescrUffRegistro;
			row["Cognome"] = item.Cognome; 
			row["Nome"] = item.Nome; 
			row["CodiceFiscale"] = item.CodiceFiscale; 
			row["PartitaIva"] = item.PartitaIva; 
			row["ComuneNascita"] = item.ComuneNascita; 
			row["ProvNascita"] = item.ProvNascita; 
			row["DataNascita"] = item.DataNascita; 
			row["Sex"] = item.Sex; 
			row["ComuneResid"] = item.ComuneResid; 
			row["ProvResid"] = item.ProvResid; 
			row["CAPResid"] = item.CAPResid; 
			row["IndirizzoSoggetto"] = item.IndirizzoSoggetto; 
			row["NumCivicoSoggetto"] = item.NumCivicoSoggetto; 
			row["EspCivicoSoggetto"] = item.EspCivicoSoggetto; 
			row["ScalaSoggetto"] = item.ScalaSoggetto; 
			row["IntSoggetto"] = item.IntSoggetto; 
			row["PianoSoggetto"] = item.PianoSoggetto; 
			row["BarratoSoggetto"] = item.BarratoSoggetto;

			table.Rows.Add(row);
			return ;
		}
  
		#endregion


		/// <summary>
		/// Legge una riga della tabella e ne costruisce un DettagliTestataRow
		/// </summary>
		/// <param name="row">Riga della tabella</param>
		/// <returns> DettagliTestataRow letta</returns>
		public DettagliTestataRow ReadRow(DataRow row)
		{
			DettagliTestataRow item = new DettagliTestataRow();
			item.IdDettaglio = (int) row["IdDettaglio"];
			item.IdTestata = (int) row["IdTestata"];
			item.IdOggetto =(int)  row["IdOggetto"];
			item.IdSoggetto =(int)  row["IdSoggetto"] ;
			item.NumeroDichiarazione = (int) row["NumeroDichiarazione"]; 
			item.AnnoDichiarazione  = row["AnnoDichiarazione"].ToString(); 
			item.Ente  = row["Ente"].ToString(); 
			item.NumeroOrdine  = row["NumeroOrdine"].ToString(); 
			item.NumeroModello  = row["NumeroModello"].ToString(); 
			item.PercPossesso  = (float) row["PercPossesso"]; 
			item.MesiPossesso  =(int) row["MesiPossesso"];
			item.MesiEsclusioneEserc = (int) row["MesiEsclusioneEserc"] ; 
			item.MesiRiduzione  = (int) row["MesiRiduzione"]; 
			item.ImpDetrazAbitazPrincipale = (float) row["ImpDetrazAbitazPrincipale"]; 
			item.Contitolare  = (bool) row["Contitolare"]; 
			item.Possesso  = (int) row["Possesso"]; 
			item.AbitazionePrincipale  = (int) row["AbitazionePrincipale"]  ;  
			item.Riduzione  = (int) row["Riduzione"]; 
			item.EsclusoEsercizio  =  (int)row["EsclusoEsercizio"]; 
			item.TipoOggetto  = (int) row["TipoOggetto"];
			item.PartitaCatastale  = row["PartitaCatastale"].ToString();  
			item.Foglio  = row["Foglio"].ToString();  
			item.Numero  = row["Numero"].ToString(); 
			item.Subalterno  = (int) row["Subalterno"];
			item.Caratteristica  =  (int)row["Caratteristica"];
			item.Sezione  = row["Sezione"].ToString();  
			item.NumeroProtocolloCat  =row["NumeroProtocolloCat"].ToString();  
			item.AnnoDenunciaCat  = row["AnnoDenunciaCat"].ToString();  
			item.Categoria  = row["Categoria"].ToString();  
			item.Classe  = row["Classe"].ToString();  
			item.Storico  = (bool) row["Storico"]; 
			item.ValoreImmobile  = (float)  row["ValoreImmobile"];
			item.Indirizzo  = row["Indirizzo"].ToString();  
			item.ValoreProvvisorio = (bool) row["ValoreProvvisorio"] ; 
			item.CodComune = (int) row["CodComune"]; 
			item.CodVia = (int) row["CodVia"]; 
			item.NumeroCivico = (int) row["NumeroCivico"]; 
			item.EspCivico = row["EspCivico"].ToString();  
			item.Scala = row["Scala"].ToString();  
			item.Interno = row["Interno"].ToString();  
			item.Piano = row["Piano"].ToString();  
			item.Barrato = row["Barrato"].ToString();  
			item.TitoloAcquisto = (int) row["TitoloAcquisto"]; 
			item.TitoloCessione = (int) row["TitoloCessione"]; 
			item.DescrUffRegistro = row["DescrUffRegistro"].ToString();
			item.Cognome = row["Cognome"].ToString(); 
			item.Nome = row["Nome"].ToString(); 
			item.CodiceFiscale = row["CodiceFiscale"].ToString(); 
			item.PartitaIva = row["PartitaIva"].ToString(); 
			item.ComuneNascita = row["ComuneNascita"].ToString(); 
			item.ProvNascita = row["ProvNascita"].ToString(); 
			item.DataNascita = row["DataNascita"].ToString(); 
			item.Sex = row["Sex"].ToString(); 
			item.ComuneResid = row["ComuneResid"].ToString(); 
			item.ProvResid = row["ProvResid"].ToString(); 
			item.CAPResid = (int) row["CAPResid"]; 
			item.IndirizzoSoggetto = row["IndirizzoSoggetto"].ToString(); 
			item.NumCivicoSoggetto = (int) row["NumCivicoSoggetto"]; 
			item.EspCivicoSoggetto = row["EspCivicoSoggetto"].ToString(); 
			item.ScalaSoggetto = row["ScalaSoggetto"].ToString(); 
			item.IntSoggetto = row["IntSoggetto"].ToString(); 
			item.PianoSoggetto = row["PianoSoggetto"].ToString(); 
			item.BarratoSoggetto = row["BarratoSoggetto"].ToString();
			return item; 
		}

   
		/// <summary>
		/// Prepara il command di selezione sull'identity IdDettaglio
		/// </summary>
		/// <param name="id"> id del record da cercare </param>
		/// <returns>Il SqlCommand costruito</returns>
		protected override SqlCommand PrepareGetRow(int id)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "Select * From " + this.TableName + " Where IdDettaglio=@id";
			SelectCommand.Parameters.Add("@id",SqlDbType.Int,4).Value = id;
			return SelectCommand;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliTestata.PrepareGetRow.errore: ", Err);
                throw Err;
            }
        }


        /// <summary>
        /// Duplica tutti i dettagli legati a un determinato immobile.
        /// I vecchi dettagli sono invalidati impostandone la DataFineValidita,
        /// mentre i nuovi, copia dei vecchi, vengono fatti riferire al nuovo immobile.
        /// </summary>
        /// <param name="idDettaglio">Identificativo del dettaglio principale, quello appartenente 
        /// al contribuente </param>
        /// <param name="newIdOggetto">Identificativo del nuovo immobile a cui si 
        /// riferiranno tutti i nuovi dettagli duplicati.</param>
        /// <param name="dataFine">Data di fine validità dei vecchi dettagli e 
        /// di inizio validità dei dettagli nuovi.</param>
        /// <returns>Esito dell'operazione</returns>
        //public bool SaveDettagliImmobili(int idDettaglio, int newIdOggetto, string dataFine)
        //{
        //	DettaglioTable dbDettaglio = new DettaglioTable();
        //	// si rende invalido il dettaglio principale
        //try{
        //	if (!dbDettaglio.Modify(idDettaglio, dataFine))
        //		return false;
        //	DettaglioRow itemDettaglio = dbDettaglio.GetRow(idDettaglio);
        //	if (itemDettaglio.ID == 0) 
        //		return false; 		
        //	int idTestata = itemDettaglio.IdTestata;
        //	int idContribuente = itemDettaglio.IdSoggetto;
        //	int idOggetto = itemDettaglio.IdOggetto;

        //	itemDettaglio.IdOggetto = newIdOggetto; 
        //	itemDettaglio.DataFineValidita = "";
        //	// duplico il dettaglio (opportunamente aggiornato) nel DB
        //	if (!dbDettaglio.Insert(itemDettaglio))
        //		return false; 

        //	// si recuperano tutti i dettagli dei contitolari 
        //	DataTable tblDettagliContitolari = ListPerOggetto(idTestata, idOggetto, idContribuente, true); 
        //	foreach (DataRow rowDettaglio in tblDettagliContitolari.Rows)
        //	{	
        //		int idDettaglioContitolare = (int) rowDettaglio["IdDettaglio"];
        //		// si invalida il dettaglio contitolare 
        //		if (!dbDettaglio.Modify(idDettaglioContitolare, dataFine))
        //			return false;
        //		// si duplica il dettaglio contitolare
        //		itemDettaglio = dbDettaglio.GetRow(idDettaglioContitolare);
        //		if (itemDettaglio.ID == 0)
        //			return false; 
        //		itemDettaglio.IdOggetto = newIdOggetto; 
        //		itemDettaglio.DataFineValidita = "";
        //		if ( !dbDettaglio.Insert(itemDettaglio))
        //			return false; 
        //	}
        //	return true; 
        //}
        //  catch (Exception Err)
        //  {
        //   Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliTestata.SaveDettagliImmobili.errore: ", Err);
        //   throw Err;
        //  }
        //}


        /// <summary>
        /// Imposta la data di fine validità del record dettaglio principale, 
        /// del record oggetto a cui il dettaglio principale si riferisce,
        /// dei record dettagli e anagrafiche associati ai contitolari su 
        /// quell'immobile. 
        /// </summary>
        /// <param name="idDettaglio">Identificativo del dettaglio principale, 
        /// quello appartenente  al contribuente </param>
        /// <param name="dataFine">Data di fine validità dei record correlati.</param>
        /// <returns>Esito dell'operazione</returns>
        //public bool DeleteDettagliImmobile(int idDettaglio, string dataFine)
        //{
        //	DataTable table = List(idDettaglio);
        //	DataRow row = null; 
        //try{
        //	if (table.Rows.Count > 0) 
        //	{
        //		row = table.Rows[0];
        //		int idTestata = (int) row["IdTestata"];
        //		int idOggetto = (int) row["IdOggetto"];
        //		int idContribuente = (int) row["IdSoggetto"];
        //		// si invalida il record oggetto (immobile) 
        //		bool success = (new OggettiTable()).Modify(idOggetto, dataFine);
        //		if (!success) 
        //			return false; 
        //		// si invalida il record dettaglio legato al contribuente 
        //		success = (new DettaglioTable()).Modify(idDettaglio, dataFine);
        //		if (!success)
        //			return false;
        //		// si invalidano i contitolari e i dettagli associati
        //		DataTable tableDettagliContitolari = ListPerOggetto(idTestata, idOggetto, idContribuente, true);
        //		foreach (DataRow rowDettaglio in tableDettagliContitolari.Rows)
        //		{					
        //			int idContitolare = (int) rowDettaglio["IdSoggetto"];
        //			success = (new AnagraficaTable()).Modify(idContitolare, dataFine);
        //			if (!success)
        //			{
        //				return false; 
        //			} 
        //			int idDettaglioContitolare = (int) rowDettaglio["IdDettaglio"];
        //			success = (new DettaglioTable()).Modify(idDettaglioContitolare, dataFine);
        //			if (!success)
        //			{
        //				return false;
        //			} 

        //		}
        //		return true;
        //	} 
        //	else 
        //		return false; 
        //}
        //  catch (Exception Err)
        //  {
        //   Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliTestata.DeleteDettagliImmobili.errore: ", Err);
        //   throw Err;
        //  }
        //}

        /// <summary>
        /// Torna una DataView valorizzata con l'elenco degli immobili e con i suoi dettagli
        /// che appartengono alla dichiarazione.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <param name="effettivo"></param>
        /// <returns></returns>
        public DataView List(int idTestata, bool effettivo)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IdTestata=@idTestata AND Contitolare=0" +
				" AND Annullato<>1";
            try { 
			if(effettivo)
				SelectCommand.CommandText += " AND Effettivo=0";
			
			SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;

			DataView dv=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
                }
                  catch (Exception Err)
                  {
                   Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DettagliTestata.List.errore: ", Err);
                   throw Err;
                 }
            }

    }        
}
        
		
