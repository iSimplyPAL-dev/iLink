using System;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione del Dovuto per Contribuente.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class DovutoPerContribuente : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(DovutoPerContribuente));
        /// <summary>
        /// 
        /// </summary>
        public DovutoPerContribuente()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Torna una DataTable con la lista dei contribuenti filtrati per cognome, nome,
		/// codice fiscale e/o partita Iva.
		/// </summary>
		/// <param name="cognome"></param>
		/// <param name="nome"></param>
		/// <param name="codiceFiscale"></param>
		/// <param name="partitaIVA"></param>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataView ListContribuenti(string cognome, string nome, string codiceFiscale,string partitaIVA, string ente, string anno)
		{
			SqlCommand SelectCommand = new SqlCommand();

			string NomeDbAnag= ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();
            try { 
            //SelectCommand.CommandText = "SELECT " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE AS Cognome, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME AS Nome, "+                      
            //    NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE AS CodiceFiscale, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA AS PartitaIva, "+
            //    NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AS CodContribuente, TP_CALCOLO_FINALE_ICI.* "+
            //    "FROM TP_CALCOLO_FINALE_ICI INNER JOIN "+
            //    NomeDbAnag + ".dbo.ANAGRAFICA ON "+
            //    "TP_CALCOLO_FINALE_ICI.COD_CONTRIBUENTE = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE "+

            //    " WHERE (" + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE LIKE @cognome) AND (" + NomeDbAnag + ".dbo.ANAGRAFICA.NOME LIKE @nome) AND (" + NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE" +
            //    " LIKE @codiceFiscale) AND (" + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA LIKE @partitaIVA) AND (TP_CALCOLO_FINALE_ICI.COD_ENTE LIKE @ente) and (TP_CALCOLO_FINALE_ICI.ANNO LIKE @anno) and ("+ NomeDbAnag +".dbo.ANAGRAFICA.DATA_FINE_VALIDITA is NULL)";
            SelectCommand.CommandText = "SELECT *";
            SelectCommand.CommandText += " FROM V_GETDOVUTO";
            SelectCommand.CommandText += " WHERE 1=1";
            SelectCommand.CommandText += " AND (COD_ENTE LIKE @ente)";
            SelectCommand.CommandText += " AND (ANNO LIKE @anno)";
            SelectCommand.CommandText += " AND (COGNOME LIKE @cognome)";
            SelectCommand.CommandText += " AND (NOME LIKE @nome)";
            SelectCommand.CommandText += " AND (CODICEFISCALE LIKE @codiceFiscale)";
            SelectCommand.CommandText += " AND (PARTITAIVA LIKE @partitaIVA)";
			SelectCommand.CommandText += " ORDER BY COGNOME, NOME ";

			SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = cognome + "%";
			SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome + "%";
			SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale + "%";
			SelectCommand.Parameters.Add("@partitaIVA", SqlDbType.VarChar).Value = partitaIVA + "%";
			SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";
			SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";
            
			//DataView dv=Query(SelectCommand).DefaultView;
            DataView dv = Query(SelectCommand,new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DovutoPerContribuente.ListContribuenti.errore: ", Err);
                throw Err;
            }

        }
        public DataView GetRimborsi(string IdEnte, string Anno)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            DataView myView;

            try
            {
               cmdMyCommand.Parameters.Clear();
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = "prc_GetRimborsi";
                cmdMyCommand.Parameters.Add("@idente", SqlDbType.NVarChar).Value = IdEnte;
                cmdMyCommand.Parameters.Add("@anno", SqlDbType.Int).Value = int.Parse(Anno);
                myView = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
            }
            catch(Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DovutoPerContribuente.GetRimborsi.errore: ", Err);
                myView =null;
            }
            finally
            {
                kill();
            }
            return myView;
        }
    }
}
