using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione della vista viewContribuenti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ContribuentiView : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContribuentiView));
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public ContribuentiView()
		{
			this.TableName = "viewContribuenti";
		}

		/// <summary>
		/// Restituisce una DataView con la lista dei contribuenti filtrati per cognome, nome,
		/// codice fiscale e/o partita Iva.
		/// </summary>
		/// <param name="cognome">Parametro di tipo <c>String</c> che identifica il Cognome del Contribuente</param>
		/// <param name="nome">Parametro di tipo <c>String</c> che identifica il Nome del Contribuente</param>
		/// <param name="codiceFiscale">Parametro di tipo <c>String</c> che identifica il Codice Fiscle del Contribuente</param>
		/// <param name="partitaIVA">Parametro di tipo <c>String</c> che identifica la Partita IVA del Contribuente</param>
		/// <param name="bonificato">Parametro di tipo <c>Bonificato</c> che identifica se il Contribuente è stato bonificato</param>
		/// <returns>Restituisce una DataView con la lista dei contribuenti filtrati</returns>
		public DataView ListContribuenti(string cognome, string nome, string codiceFiscale,			string partitaIVA, Bonificato bonificato)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE (Cognome LIKE @cognome) AND (Nome LIKE @nome) AND (CodiceFiscale" +
				" LIKE @codiceFiscale) AND (PartitaIva LIKE @partitaIVA)";

			switch(bonificato)
			{
				case Bonificato.Bonificate:
					SelectCommand.CommandText += " AND Bonificato=1";
					break;

				case Bonificato.DaBonificare:
					SelectCommand.CommandText += " AND Bonificato=0";
					break;
			}

			SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = cognome + "%";
			SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome + "%";
			SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale + "%";
			SelectCommand.Parameters.Add("@partitaIVA", SqlDbType.VarChar).Value = partitaIVA + "%";

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView; ;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribuentiView.ListContribuenti.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Restituisce una DataView con la lista dei contribuenti filtrati  in base al flag bonificato.
		/// </summary>
		/// <param name="bonificato">Parametro di tipo <c>Bonificato</c> che identifica se il Contribuente è stato bonificato</param>
		/// <returns>Restituisce una DataView con la lista dei contribuenti filtrati</returns>
		public DataView List(Bonificato bonificato)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName;
            try { 
			switch(bonificato)
			{
				case Bonificato.Bonificate:
					SelectCommand.CommandText += " WHERE Bonificato=1";
					break;

				case Bonificato.DaBonificare:
					SelectCommand.CommandText += " WHERE Bonificato=0";
					break;
			}

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView; ;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribuentiView.List.errore: ", Err);
                throw Err;
            }
        }
	}
}
