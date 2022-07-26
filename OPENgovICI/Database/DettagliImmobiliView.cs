using Business;
using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struttura Dettaglio Immobili
	/// </summary>
	public struct DettagliImmobiliRow
    {
        /// 
		public int IDDettaglio;
        /// 
		public int IdTestata;
        /// 
		public int IDSoggetto;
        /// 
		public decimal PercPossesso;
        /// 
		public int MesiPossesso;
        /// 
		public int MesiEsclusioneEsenzione;
        /// 
		public int MesiRiduzione;
        /// 
		public decimal ImpDetrazAbitazPrincipale;
        /// 
		public bool Contitolare;
        /// 
		public bool AbitazionePrincipale;
        /// 
		public int IDOggetto;
        /// 
		public string NumeroOrdine;
        /// 
		public string NumeroModello;
        /// 
		public int CodUI;
        /// 
		public int TipoImmobile;
        /// 
		public int PartitaCatastale;
        /// 
		public string Foglio;
        /// 
		public string Numero;
        /// 
		public int Subalterno;
        /// 
		public int Caratteristica;
        /// 
		public string Sezione;
        /// 
		public string NumeroProtCatastale;
        /// 
		public string AnnoDenunciaCatastale;
        /// 
		public string CodCategoriaCatastale;
        /// 
		public string CodClasse;
        /// 
		public string CodRendita;
        /// 
		public bool Storico;
        /// 
		public decimal ValoreImmobile;
        /// 
		public int CodComune;
        /// 
		public int CodVia;
        /// 
		public int NumeroCivico;
        /// 
		public string EspCivico;
        /// 
		public string Scala;
        /// 
		public string Interno;
        /// 
		public string Piano;
        /// 
		public string Barrato;
        /// 
		public int NumeroEcografico;
        /// 
		public bool TitoloAcquisto;
        /// 
		public bool TitoloCessione;
        /// 
		public string DescrUffRegistro;
        /// 
		public DateTime DataUltimaModifica;
        /// 
		public bool AnnullatoDettaglio;
        /// 
		public bool AnnullatoImmobile;
        /// 
		public bool FlagValoreProvv;
	}

    /// <summary>
    /// Classe di gestione della vista viewDettagliImmobili.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class DettagliImmobiliView : Database
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DettagliImmobiliView));
		/// <summary>
		/// Costruttore della classe
		/// </summary>
		public DettagliImmobiliView()
		{
			this.TableName = "viewDettagliImmobili";
		}

		/// <summary>
		/// Torna una DataView valorizzata col elenco degli immobili e dei dettagli
		/// filtrato per l'id della dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
        /// <param name="myConn"></param>
		/// <returns></returns>
		public DataView ListByIDTestata(int idTestata,int IsPassaggio)
		{
			DataView myDataView = new DataView(); 
			try
			{
				string sSQL = string.Empty;
				using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
				{
					sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetListImmobili", "IDTESTATA", "PASSAGGIO");
					myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTESTATA", idTestata), ctx.GetParam("PASSAGGIO", IsPassaggio));
					ctx.Dispose();
				}
			}
			catch (Exception ex)
			{
				log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaTipoAliquote.errore: ", ex);
				myDataView = null;
			}
			return myDataView;
		}

		/// <summary>
		/// Creazione di una riga della tabella vuota.
		/// </summary>
		/// <param name="vista"></param>
		/// <param name="idTestata"></param>
		public static void AddEmptyRow(ref DataView vista, int idTestata)
		{
			DettagliImmobiliRow Item = new DettagliImmobiliRow();
			Item.IdTestata = idTestata;
			Item.IDSoggetto = 0;
			Item.PercPossesso = 0;
			Item.MesiPossesso = 0;
			Item.MesiEsclusioneEsenzione = 0;
			Item.MesiRiduzione = 0;
			Item.ImpDetrazAbitazPrincipale = 0;
			Item.Contitolare = false;
			Item.AbitazionePrincipale = false;
			Item.NumeroOrdine = "";
			Item.NumeroModello = "";
			Item.CodUI = 0;
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
			Item.CodVia = 0;
			Item.NumeroCivico = 0;
			Item.EspCivico = "";
			Item.Scala = "";
			Item.Interno = "";
			Item.Piano = "";
			Item.Barrato = "";
			Item.NumeroEcografico = 0;
			Item.TitoloAcquisto = false;
			Item.TitoloCessione = false;
			Item.DescrUffRegistro = "";		
			Item.DataUltimaModifica = Convert.ToDateTime(DateTime.Now.ToShortDateString());
			Item.AnnullatoDettaglio = false;
			Item.AnnullatoImmobile = false;
			Item.FlagValoreProvv = false;
		
			DataRow Riga = vista.Table.NewRow();

			Riga["IdTestata"] = Item.IdTestata;
			Riga["IDSoggetto"] = Item.IDSoggetto;
			Riga["PercPossesso"] = Item.PercPossesso;
			Riga["MesiPossesso"] = Item.MesiPossesso;
			Riga["MesiEsclusioneEsenzione"] = Item.MesiEsclusioneEsenzione;
			Riga["MesiRiduzione"] = Item.MesiRiduzione;
			Riga["ImpDetrazAbitazPrincipale"] = Item.ImpDetrazAbitazPrincipale;
			Riga["Contitolare"] = Item.Contitolare;
			Riga["AbitazionePrincipale"] = Item.AbitazionePrincipale;
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
			Riga["CodVia"] = Item.CodVia;
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
			Riga["DataUltimaModifica"] = Item.DataUltimaModifica;
			Riga["AnnullatoDettaglio"] = Item.AnnullatoDettaglio;
			Riga["AnnullatoImmobile"] = Item.AnnullatoImmobile;
			Riga["FlagValoreProvv"] = Item.FlagValoreProvv;

			vista.Table.Rows.Add(Riga);
			return;
		}

		/// <summary>
		/// Torna una DataView valorizzata col elenco degli immobili e dei dettagli
		/// filtrato per l'id della dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
        /// <param name="myConn"></param>
		/// <returns></returns>
		public DataView ListAbitazionePrincipale(int idTestata, string myConn)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Contitolare=0" +
				" AND AnnullatoDettaglio <> 1 AND AnnullatoImmobile <> 1 and AbitazioneprincipaleAttuale = 1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
            DataView dv = Query(SelectCommand, new SqlConnection(myConn)).DefaultView; ;
			kill();
			return dv;
		}
		/*dipe 12/03/2008*/
		/// <summary>
		/// Torna una DataView valorizzata col elenco degli immobili e dei dettagli
		/// filtrato per l'id del contribuente.
		/// </summary>
		/// <param name="idContribuente"></param>
        /// <param name="myConn"></param>
		/// <returns></returns>
		public DataView ListAbitazionePrincipaleTestata(int idContribuente, string myConn)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM viewDettagliImmobiliTestata" +
				" WHERE idcontribuente=@idContribuente AND Contitolare=0" +
				" AND AnnullatoDettaglio <> 1 AND AnnullatoImmobile <> 1 and AbitazioneprincipaleAttuale = 1";

			SelectCommand.Parameters.Add("@idContribuente",SqlDbType.Int).Value = idContribuente;
            DataView dv = Query(SelectCommand, new SqlConnection(myConn)).DefaultView; ;
			kill();
			return dv;
		}

	}
}
