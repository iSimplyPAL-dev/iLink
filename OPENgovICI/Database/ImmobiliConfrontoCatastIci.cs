using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Business;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione delle posizioni a castasto.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ImmobiliConfrontoCatastoIci : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ImmobiliConfrontoCatastoIci));

        public ImmobiliConfrontoCatastoIci()
		{
			
		}

		/// <summary>
		/// Torna una DataTable valorizzata con l'elenco degli immobili e dei contribuenti filtrati per anno ed ente.
		/// </summary>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns></returns>
		public DataTable ListImmobiliContribuenti(string anno, string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();

            //			SelectCommand.CommandText += "SELECT Cognome, NOME, CodiceFiscale, PartitaIva, DataNascita, Foglio, Numero, CAST(Subalterno AS varchar) as Subalterno, Classe, CategoriaCatastale, ValoreImmobile, DataInizio, CodContribuente "; 
            //			SelectCommand.CommandText += " FROM viewContribImmobiliCatasto ";
            //			SelectCommand.CommandText += " WHERE (Ente = '"+ ente +"') AND (YEAR(DataInizio) <= " + anno +") AND (YEAR(DataFine) >= " + anno +") AND Caratteristica=3 ";
            try { 
			SelectCommand.CommandText = "SELECT Cognome, NOME, CodiceFiscale, PartitaIva, DataNascita, Foglio, Numero,  subalternoStringa, ";
			SelectCommand.CommandText += " Classe, CategoriaCatastale, ValoreImmobile, DataInizio, CodContribuente, lenfoglio , lennumero,lensubalterno, ";
			SelectCommand.CommandText += " FoglioFormattato=case lenfoglio  when '1' then '000'+foglio ";
			SelectCommand.CommandText += " when '2' then '00'+foglio ";
			SelectCommand.CommandText += " when '3' then '0'+foglio ";
			SelectCommand.CommandText += " when '4' then foglio end, ";
			SelectCommand.CommandText += " NumeroFormattato=case lennumero  when '1' then '0000'+numero "; 
			SelectCommand.CommandText += " when '2' then '000'+numero ";
			SelectCommand.CommandText += " when '3' then '00'+numero ";
			SelectCommand.CommandText += " when '4' then '0'+numero ";
			SelectCommand.CommandText += " when '5' then numero end, ";
			SelectCommand.CommandText += " SubalternoFormattato=case lensubalterno  when '1' then '000'+subalternoStringa ";
			SelectCommand.CommandText += " when '2' then '00'+subalternoStringa ";
			SelectCommand.CommandText += " when '3' then '0'+subalternoStringa "; 
			SelectCommand.CommandText += " when '4' then subalternoStringa end, idimmobile, SubalternoOriginale, ";
			SelectCommand.CommandText += " numeroOrdinato=case isnumeric(numero) when '1' then ''+numero when '0' then '0' end , ";
			SelectCommand.CommandText += " FoglioOrdinato=case isnumeric(foglio) when '1' then ''+foglio when '0' then '0' end ";
			SelectCommand.CommandText += " FROM viewContribImmobiliCatasto ";
			SelectCommand.CommandText += " WHERE (Ente = '"+ ente +"') AND (YEAR(DataInizio) <= " + anno +") AND (YEAR(DataFine) >= " + anno +") AND Caratteristica=3 ";
			SelectCommand.CommandText += " order by cast((case isnumeric(foglio) when '1' then ''+foglio when '0' then '0' end )as int), ";
			SelectCommand.CommandText += " cast((case isnumeric(numero) when '1' then ''+numero when '0' then '0' end ) as int), cast(SubalternoOriginale as int)";
			//SelectCommand.CommandText += " order by cast(foglio as int),cast(numero as int), cast(SubalternoOriginale as int)";

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImmobiliConfrontoCatastoIci.ListImmobiliContribuenti.errore: ", Err);
                throw Err;
            }

        }

    }
}

