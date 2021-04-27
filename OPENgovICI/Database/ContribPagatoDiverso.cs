using System;
using System.Data;
using System.Data.SqlClient;
using Business;
using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione dei Contribuenti che hanno Importo Dovuto diverso dall'Importo Pagato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ContribPagatoDiverso : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ContribPagatoDiverso));
        /// <summary>
		/// Costruttore della classe
		/// </summary>
		public ContribPagatoDiverso()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	
		/// <summary>
		/// Carica l'elenco degli anni presenti nella tabella.
		/// </summary>
		/// <returns></returns>
		public DataView AnniCaricati()
		{
            DataView Vista = new Aliquote().ListaAnni(Business.ConstWrapper.CodiceEnte);
            Vista.Sort = "ANNO DESC";
            return Vista;
		}

        //*** 20140630 - TASI ***
        /// <summary>
        /// Torna una DataView con l'elenco dei versamenti con Importo del dovuto ed Importo Pagato filtrati
        /// per ente ed anno di riferimento
        /// </summary>
        /// <param name="sMyIdEnte">Parametro di tipo <c>String</c> che identifica l'ente</param>
        /// <param name="sMyTributo"></param>
        /// <param name="sAnno">Parametro di tipo <c>String</c> che identifica l'anno</param>
        /// <returns></returns>
        public DataView PagatoDiverso(string sMyIdEnte, string sMyTributo, string sAnno)
        {
		    SqlCommand cmdMyCommand = new SqlCommand();
            DataView DvDati;
            try
            {
                //Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandTimeout = 3000;
                //valorizzo il commandText
                cmdMyCommand.CommandText = "prc_AnalisiPagatoDiverso";
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@TRIBUTO", SqlDbType.VarChar)).Value = sMyTributo;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
                //eseguo la query
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmdMyCommand);
                DataSet dsMyDati = new DataSet();
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dsMyDati);
                DvDati = dsMyDati.Tables[0].DefaultView;
                return DvDati;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribPagatoDiverso.PagatoDiverso.errore: ", Err);
                return null;
            }
        }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="sMyIdEnte"></param>
     /// <param name="sMyTributo"></param>
     /// <param name="sAnno"></param>
     /// <returns></returns>              
        public DataView NonPagato(string sMyIdEnte, string sMyTributo, string sAnno)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            DataView DvDati;
            try
            {
                //Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                cmdMyCommand.CommandTimeout = 0;                
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                //valorizzo il commandText
                cmdMyCommand.CommandText = "prc_AnalisiNonPagato";
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@TRIBUTO", SqlDbType.VarChar)).Value = sMyTributo;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
                //eseguo la query
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmdMyCommand);
                DataSet dsMyDati = new DataSet();
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dsMyDati);
                DvDati = dsMyDati.Tables[0].DefaultView;
                return DvDati;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribPagatoDiverso.NonPagato.errore: ", Err);
                return null;
            }
        }
        //*** ***
	}
}
