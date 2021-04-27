using System;
using System.Data;
using System.Data.SqlClient;
using Business;
using log4net;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione per la vista viewVersamentiErrati.
	/// </summary>
	public class VersamentiErratiView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(VersamentiErratiView));
        public VersamentiErratiView()
		{
			this.TableName = "viewVersamentiErrati";
		}

		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <returns>Restituisce un DataView</returns>
		public DataView AnniCaricati()
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "Select AnnoRiferimento From " + this.TableName + " GROUP BY AnnoRiferimento, ente having ente = '"+ ConstWrapper.CodiceEnte +"' order by AnnoRiferimento DESC";
			DataView dv=Query(SelectCommand,new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VersamentiErratiView.AnniCaricati.errore: ", Err);
                throw Err;

            }


        }

        /// <summary>
        /// Torna una DataView con l'elenco dei versamenti errati filtrati per ente, anno di riferimento e errore.
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="annoRiferimento"></param>
        /// <param name="idErrore"></param>
        /// <returns>Restituisce un DataView</returns>
        public DataView List(string ente, string annoRiferimento, int idErrore)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE Ente=@ente"; //AND Annullato<>1";

			//if(annoRiferimento != String.Empty)
			if(annoRiferimento != "0")
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
			if(idErrore > 0)
				SelectCommand.CommandText += " AND IDErrore=@idErrore";

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			SelectCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.VersamentiErratiView.List.errore: ", Err);
                throw Err;

            }
        }
	}
}
