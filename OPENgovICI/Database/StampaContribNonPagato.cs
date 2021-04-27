using System;
using System.Data;
using System.Data.SqlClient;
using Business;

using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione degli utenti che non hanno provveduto al pagamento.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class StampaContribNonPagato : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(StampaContribNonPagato));
        public StampaContribNonPagato()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns></returns>
		public DataView AnniCaricati()
		{
            try { 
            DataView Vista = new Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
            Vista.Sort = "ANNO DESC";
            return Vista;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.AnniCaricati.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna un DataView con la lista dei dovuti filtrati per anno.
		/// </summary>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns></returns>
		public DataView DovutoPerAnno(string anno, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText="select * from TP_CALCOLO_FINALE_ICI ";
			SelectCommand.CommandText+=" where COD_ENTE LIKE @ente";
			if (anno!="0" && anno!="")
				SelectCommand.CommandText+=" and ANNO in ("+anno+")";
			SelectCommand.CommandText+=" and ICI_DOVUTA_TOTALE > 0";

			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.DovutoPerAnno.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna un DataView con la lista degli IdAnagrafici dei contribuenti che hanno effettuato versamenti per l'anno e l'ente selezionato.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataView IdAnagraficiVersamenti (string ente, string anno)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM TblVersamenti " +
				"where (Ente like @ente) ";
			if (anno!="0" && anno!="")
				SelectCommand.CommandText +="and (AnnoRiferimento in ("+anno+")) ";
			SelectCommand.CommandText +="and (violazione = 0) and (ravvedimentoOperoso = 0) ";
				//"and (ImportoPagato = 0)";

			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";
			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.IdAnagraficiVersamenti.errore: ", Err);
                throw Err;
            }

        }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="codContribuente"></param>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataView ControlloVersamenti(string codContribuente, string ente, string anno)
		{
			SqlCommand SelectCommand = new SqlCommand();

			string NomeDbAnag= ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();

            /*SelectCommand.CommandText = "SELECT * FROM TblVersamenti " +
				"where (IdAnagrafico IN " + codContribuente + ") and (Ente like @ente) "+
				"and (AnnoRiferimento like @anno) and (violazione = 0) and (ravvedimentoOperoso = 0) "+
				"and (ImportoPagato = 0)";
			*/
            try { 
			SelectCommand.CommandText ="SELECT TblVersamenti.*," + NomeDbAnag +".dbo.ANAGRAFICA.COD_CONTRIBUENTE AS CodContribuente, "+ 
				NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE AS Cognome, " + NomeDbAnag +".dbo.ANAGRAFICA.NOME AS Nome, "+
				NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE AS CodiceFiscale, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA AS PartitaIva "+
				"FROM TblVersamenti INNER JOIN "+
				NomeDbAnag + ".dbo.ANAGRAFICA ON TblVersamenti.IdAnagrafico = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE "+
				"WHERE (TblVersamenti.Ente LIKE @ente) ";
			if (anno!="0" && anno!="")
				SelectCommand.CommandText +=" AND (TblVersamenti.AnnoRiferimento in ("+anno+"))";
			SelectCommand.CommandText +=" AND (TblVersamenti.Violazione = 0) AND "+
				"(TblVersamenti.RavvedimentoOperoso = 0) AND (TblVersamenti.ImportoPagato = 0) "+
				" and (IdAnagrafico IN " + codContribuente + ")";

		//	SelectCommand.Parameters.Add("@codContribuente", SqlDbType.VarChar).Value = codContribuente;
			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";
			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.ControlloVersamenti.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Torna un DataView con la lista dei versamenti ordinari con Importo Pgato=0, filtrati per ente e anno selezionato.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataView VersamentiImportoZero(string ente, string anno)
		{
			
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM TblVersamenti " +
				"where (Ente like @ente) ";
			if (anno!="0" && anno!="")
				SelectCommand.CommandText +=" and (AnnoRiferimento in ("+anno+"))";
			SelectCommand.CommandText +=" and (violazione = 0)"+
				"and (ravvedimentoOperoso = 0) "+
				"and (ImportoPagato = 0)";

			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";
			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.VersamentiImportoZero.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna un DataView con la lista dei contribuenti che non hanno effettuato 
		/// versamenti, filtrati per anno ed ente selezionato e per elenco dei codContribuente.
		/// </summary>
		/// <param name="codContribuente"></param>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns></returns>
		public DataView ContribuentiNonPagato(string codContribuente, string anno, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();

			string NomeDbAnag= ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();
            try { 
			SelectCommand.CommandText = "SELECT distinct "+ NomeDbAnag +".dbo.ANAGRAFICA.COD_CONTRIBUENTE AS CodContribuente, "+
				NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE AS Cognome, "+ NomeDbAnag +".dbo.ANAGRAFICA.NOME AS Nome, "+
				NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE AS CodiceFiscale, "+ NomeDbAnag +".dbo.ANAGRAFICA.PARTITA_IVA AS PartitaIva, " + NomeDbAnag +".dbo.ANAGRAFICA.DATA_MORTE, "+
				"TP_CALCOLO_FINALE_ICI.ANNO as AnnoRiferimento, TP_CALCOLO_FINALE_ICI.COD_ENTE, TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_TOTALE as ImportoDovuto "+
				"FROM TP_CALCOLO_FINALE_ICI INNER JOIN "+
				NomeDbAnag + ".dbo.ANAGRAFICA ON  "+
				"TP_CALCOLO_FINALE_ICI.COD_CONTRIBUENTE = "+ NomeDbAnag +".dbo.ANAGRAFICA.COD_CONTRIBUENTE "+
				"WHERE (TP_CALCOLO_FINALE_ICI.COD_ENTE = "+ ente +")";
            SelectCommand.CommandText += " AND TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_TOTALE>0)";
			if (anno!="0" && anno!="")
				SelectCommand.CommandText+=" AND (TP_CALCOLO_FINALE_ICI.ANNO in (" + anno +"))";
			if(codContribuente !="")
		 		SelectCommand.CommandText +=" AND ("+ NomeDbAnag +".dbo.ANAGRAFICA.COD_CONTRIBUENTE IN ("+ codContribuente +"))";
			else
				SelectCommand.CommandText +=" AND ("+ NomeDbAnag +".dbo.ANAGRAFICA.COD_CONTRIBUENTE IN ('"+ codContribuente +"'))";

			//aggiunto controllo data fine validità
			SelectCommand.CommandText +=" and "+ NomeDbAnag +".dbo.ANAGRAFICA.data_fine_validita is null";
			SelectCommand.CommandText += " Order by "+ NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, "+ NomeDbAnag +".dbo.ANAGRAFICA.NOME ";
			
			//SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";
			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaContribNonPagato.ContribuentiNonPagato.errore: ", Err);
                throw Err;
            }
        }
	}
}
