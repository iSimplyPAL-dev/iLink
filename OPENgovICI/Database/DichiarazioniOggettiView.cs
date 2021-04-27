using System;
using System.Data;
using System.Data.SqlClient;
using Business;
using log4net;

namespace DichiarazioniICI.Database {
    
    /// <summary>
    /// Rappresenta una riga della tabella viewDichiarazoniOggetti.
    /// </summary>
    public struct DichiarazoniOggettiViewRow
    {
        /// 
        public string Ente;
        /// 
        public int IdTestata;
        /// 
        public string NumeroOrdine;
        /// 
        public string NumeroModello;
        /// 
        public int IdSoggetto;
        /// 
        //*** 20140509 - TASI ***
        public int TipoUtilizzo;
        /// 
        public int TipoPossesso;
        /// 
        //*** ***
        public System.Double PercPossesso;
        /// 
        public int MesiPossesso;
        /// 
        public int MesiEsclusioneEsenzione;
        /// 
        public int MesiRiduzione;
        /// 
        public System.Decimal ImpDetrazAbitazPrincipale;
        /// 
        public bool Contitolare;
        /// 
        public bool AbitazionePrincipale;
        /// 
        public bool Bonificato;
        /// 
        public bool Annullato;
        /// 
		public System.DateTime DataUltimaModifica;
        /// 
        public System.DateTime DataInizioValidità;
        /// 
        public System.DateTime DataFineValidità;
        /// 
        public string Operatore;
        /// 
        public int NumeroDichiarazione;
        /// 
        public string AnnoDichiarazione;
        /// 
        public string NumeroProtocollo;
        /// 
        public System.DateTime DataProtocollo;
        /// 
        public string TotaleModelli;
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
        public System.Decimal ValoreImmobile;
        /// 
		public bool FlagValoreProvv;
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
        public string Barrato;
        /// 
		public string Piano;
        /// 
        public int NumeroEcografico;
        /// 
        public bool TitoloAcquisto;
        /// 
        public bool TitoloCessione;
        /// 
        public string DescrUffRegistro;
        /// 
		public int IDImmobile;
        /// 
		public bool StoricoDichiarazione;
    }
    
	/// <summary>
	/// 
	/// </summary>
	public enum BonificatoTestata
    {
        /// 
		Tutte = -1,
        /// 
		Bonificate,
        /// 
		DaBonificare
    }

    /// <summary>
    /// Classe di gestione della tabella viewDichiarazoniOggetti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class DichiarazoniOggettiView : Database {
        private static readonly ILog log = LogManager.GetLogger(typeof(DichiarazoniOggettiView));
        /// <summary>
        /// 
        /// </summary>
        public DichiarazoniOggettiView() {
            this.TableName = "viewDichiarazoniOggetti";
            this.ProcedureName = "prc_GetDichiarazioniOggetti";
        }
        

		/// <summary>
		/// Ritorna tutte le righe aventi come contribuente quello specificato
		/// </summary>
		/// <param name="codiceContribuente"></param>
		/// <param name="bonificato"></param>
		/// <returns> La DataTable valorizzata o null </returns>
		public DataTable List(int codiceContribuente, Bonificato bonificato)
		{
			DataTable Tabella;

			try
			{
				SqlCommand SelectCommand = new SqlCommand(); 
				//SelectCommand.CommandText = "Select * From " + this.TableName + " where idContribuente=@codiceContribuente" +
				//	" AND Contitolare=0 AND Annullato=0 and ente like @ente";
				//dipe 26/04/2010
				SelectCommand.CommandText = "Select * From " + this.TableName + " where idsoggetto=@codiceContribuente" +
					" AND Contitolare=0 AND Annullato=0 and ente like @ente";
				
				switch(bonificato)
				{
					case Bonificato.Bonificate:
						SelectCommand.CommandText += " AND Bonificato=1";
						break;

					case Bonificato.DaBonificare:
						SelectCommand.CommandText += " AND Bonificato=0";
						break;
				}

				SelectCommand.Parameters.Add("@codiceContribuente", SqlDbType.VarChar).Value = codiceContribuente;
				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniOggettiView.List.errore: ", Err);
                kill();
				Tabella = new DataTable();
			}
			finally{
				kill();
			}

			return Tabella;
		}

        /// <summary>
        /// Ritorna tutte le righe aventi come contribuente quello specificato
        /// verificando anche il contitolare
        /// </summary>
        /// <param name="codiceContribuente"></param>
        /// <param name="IdEnte"></param>
        /// <param name="myConn"></param>
        /// <returns> La DataTable valorizzata o null </returns>
        public DataTable ListCont(int codiceContribuente, string IdEnte, string myConn)
        {
            DataTable Tabella;
            SqlCommand cmdMyCommand = new SqlCommand();

            try
            {
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = this.ProcedureName;
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add("@ENTE", SqlDbType.VarChar).Value = IdEnte;
                cmdMyCommand.Parameters.Add("@IDCONTRIBUENTE", SqlDbType.VarChar).Value = codiceContribuente;
                Tabella = Query(cmdMyCommand, new SqlConnection(myConn));
            }
            catch(Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniOggettiView.ListCont.errore: ", Err);
                Tabella = new DataTable();
            }
            finally
            {
                cmdMyCommand.Dispose();
            }
            return Tabella;
        }
        //*** ***
		//public DataTable Search(string sezione, string foglio, string numero, string subalterno,
		//						string categoria, string protocollo, string classe,
		//						string anno, string via, string numeroCivico, 
		//						string interno, string scala, BonificatoTestata bonificatoTestata)
		//{
  //          try {
  //              string SqlString = "Select * from " + this.TableName + " where (Sezione like @sezione) and (foglio like @foglio) and " +
  //                                  " (numero like @numero) and (subalterno like @subalterno) and (categoriaCatastale like @categoria) and (NumeroProtocollo like @protocollo) and " +
  //                                  " (classe like @classe) and (AnnoDichiarazione like @anno)  and (numeroCivico like @numeroCivico) and " +
  //                                  " (interno like @interno) and (scala like @scala) And StoricoDichiarazione <> 1 and ente like @ente";

  //              SqlCommand SearchCommand = new SqlCommand(SqlString);

  //              switch (bonificatoTestata)
  //              {
  //                  case BonificatoTestata.Bonificate:
  //                      SearchCommand.CommandText += " AND BonificatoTestata=1";
  //                      break;

  //                  case BonificatoTestata.DaBonificare:
  //                      SearchCommand.CommandText += " AND BonificatoTestata=0";
  //                      break;
  //              }

  //              SearchCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione + "%";
  //              SearchCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio + "%";
  //              SearchCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero + "%";
  //              SearchCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno + "%";
  //              SearchCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria + "%";
  //              SearchCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo + "%";
  //              SearchCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe + "%";
  //              SearchCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";
  //              SearchCommand.Parameters.Add("@numeroCivico", SqlDbType.VarChar).Value = numeroCivico + "%";
  //              SearchCommand.Parameters.Add("@interno", SqlDbType.VarChar).Value = interno + "%";
  //              SearchCommand.Parameters.Add("@scala", SqlDbType.VarChar).Value = scala + "%";
  //              SearchCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ConstWrapper.CodiceEnte;
  //              DataTable dt = Query(SearchCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
  //              kill();
  //              return dt;
  //          }
  //          catch (Exception Err)
  //          {
  //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniOggettiView.Search.errore: ", Err);
  //              throw Err;
  //          }
  //      }
       
        /// <summary>
        /// Ritorna una riga identificata dall'identity.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <param name="idImmobile"></param>
        /// <returns></returns>
        public DichiarazoniOggettiViewRow GetRow(int idTestata, int idImmobile)
		{
            DichiarazoniOggettiViewRow riga = new DichiarazoniOggettiViewRow();

            SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IdTestata=@idTestata AND IDImmobile=@idImmobile";

			SelectCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;
			SelectCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idImmobile;

            DataTable tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
            try { 
            if (tabella.Rows.Count > 0)
			{
                riga.Ente = (System.String)tabella.Rows[0]["Ente"];
                riga.IdTestata = (System.Int32)tabella.Rows[0]["IdTestata"];
                riga.NumeroOrdine = (System.String)tabella.Rows[0]["NumeroOrdine"];
                riga.NumeroModello = (System.String)tabella.Rows[0]["NumeroModello"];
                riga.IdSoggetto = (System.Int32)tabella.Rows[0]["IdSoggetto"];
                //*** 20140509 - TASI ***
                riga.TipoUtilizzo = tabella.Rows[0]["IdTipoUtilizzo"] == DBNull.Value ? 0 : (System.Int32)tabella.Rows[0]["IdTipoUtilizzo"];
                riga.TipoPossesso = tabella.Rows[0]["IdTipoPossesso"] == DBNull.Value ? 0 : (System.Int32)tabella.Rows[0]["IdTipoPossesso"];
                //*** ***
                riga.PercPossesso = (System.Double)tabella.Rows[0]["PercPossesso"];
                riga.MesiPossesso = (System.Int32)tabella.Rows[0]["MesiPossesso"];
                riga.MesiEsclusioneEsenzione = (System.Int32)tabella.Rows[0]["MesiEsclusioneEsenzione"];
                riga.MesiRiduzione = (System.Int32)tabella.Rows[0]["MesiRiduzione"];
                riga.ImpDetrazAbitazPrincipale = (System.Decimal)tabella.Rows[0]["ImpDetrazAbitazPrincipale"];
                riga.Contitolare = (System.Boolean)tabella.Rows[0]["Contitolare"];
                riga.AbitazionePrincipale = (System.Boolean)tabella.Rows[0]["AbitazionePrincipale"];
                riga.Bonificato = (System.Boolean)tabella.Rows[0]["Bonificato"];
                riga.Annullato = (System.Boolean)tabella.Rows[0]["Annullato"];
				riga.DataUltimaModifica = (System.DateTime)tabella.Rows[0]["DataUltimaModifica"];
                riga.DataInizioValidità = (System.DateTime)tabella.Rows[0]["DataInizioValidità"];
                riga.DataFineValidità = tabella.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : (System.DateTime)tabella.Rows[0]["DataFineValidità"];
                riga.Operatore = (System.String)tabella.Rows[0]["Operatore"];
                riga.NumeroDichiarazione = (System.Int32)tabella.Rows[0]["NumeroDichiarazione"];
                riga.AnnoDichiarazione = (System.String)tabella.Rows[0]["AnnoDichiarazione"];
                riga.NumeroProtocollo = (System.String)tabella.Rows[0]["NumeroProtocollo"];
                riga.DataProtocollo = (System.DateTime)tabella.Rows[0]["DataProtocollo"];
                riga.TotaleModelli = (System.String)tabella.Rows[0]["TotaleModelli"];
                riga.CodUI = (System.Int32)tabella.Rows[0]["CodUI"];
                riga.TipoImmobile = (System.Int32)tabella.Rows[0]["TipoImmobile"];
                riga.PartitaCatastale = (System.Int32)tabella.Rows[0]["PartitaCatastale"];
                riga.Foglio = (System.String)tabella.Rows[0]["Foglio"];
                riga.Numero = (System.String)tabella.Rows[0]["Numero"];
                riga.Subalterno = (System.Int32)tabella.Rows[0]["Subalterno"];
                riga.Caratteristica = (System.Int32)tabella.Rows[0]["Caratteristica"];
                riga.Sezione = (System.String)tabella.Rows[0]["Sezione"];
                riga.NumeroProtCatastale = (System.String)tabella.Rows[0]["NumeroProtCatastale"];
                riga.AnnoDenunciaCatastale = (System.String)tabella.Rows[0]["AnnoDenunciaCatastale"];
                riga.CodCategoriaCatastale = (System.String)tabella.Rows[0]["CodCategoriaCatastale"];
                riga.CodClasse = (System.String)tabella.Rows[0]["CodClasse"];
                riga.CodRendita = (System.String)tabella.Rows[0]["CodRendita"];
                riga.ValoreImmobile = (System.Decimal)tabella.Rows[0]["ValoreImmobile"];
				riga.FlagValoreProvv = (System.Boolean)tabella.Rows[0]["FlagValoreProvv"];
                riga.CodComune = (System.Int32)tabella.Rows[0]["CodComune"];
                riga.CodVia = (System.Int32)tabella.Rows[0]["CodVia"];
                riga.NumeroCivico = (System.Int32)tabella.Rows[0]["NumeroCivico"];
                riga.EspCivico = (System.String)tabella.Rows[0]["EspCivico"];
                riga.Scala = (System.String)tabella.Rows[0]["Scala"];
                riga.Interno = (System.String)tabella.Rows[0]["Interno"];
                riga.Barrato = (System.String)tabella.Rows[0]["Barrato"];
				riga.Piano = (System.String)tabella.Rows[0]["Piano"];
                riga.NumeroEcografico = (System.Int32)tabella.Rows[0]["NumeroEcografico"];
                riga.TitoloAcquisto = (System.Boolean)tabella.Rows[0]["TitoloAcquisto"];
                riga.TitoloCessione = (System.Boolean)tabella.Rows[0]["TitoloCessione"];
                riga.DescrUffRegistro = (System.String)tabella.Rows[0]["DescrUffRegistro"];
				riga.IDImmobile = (System.Int32)tabella.Rows[0]["IDImmobile"];
				riga.StoricoDichiarazione = (System.Boolean)tabella.Rows[0]["StoricoDichiarazione"];
            }
            return riga;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniOggettiView.GetRow.errore: ", Err);
                throw Err;
            }
        }
    }
}
