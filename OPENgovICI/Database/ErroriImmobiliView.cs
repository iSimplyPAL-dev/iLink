using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione della vista viewErroriDichiarazioni.
	/// </summary>
	public class ErroriImmobiliView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ErroriImmobiliView));
        /// <summary>
        /// 
        /// </summary>
        public ErroriImmobiliView()
		{
			this.TableName = "viewErroriImmobili";
		}

		/// <summary>
		/// Torna una DataView valorizzata col elenco degli errori filtrati per l'id della
		/// dichiarazione, id immobile e id dettaglio testata.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <param name="idOggetto"></param>
		/// <param name="idDettaglioTestata"></param>
		/// <returns></returns>
		public DataView ListByIDOggetto(int idDichiarazione, int idOggetto, int idDettaglioTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDDichiarazione=@idDichiarazione AND (idOggetto=@idOggetto OR IDDettaglioTestata=@IDDettaglioTestata )";

			SelectCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
			SelectCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;
			SelectCommand.Parameters.Add("@idDettaglioTestata",SqlDbType.Int).Value = idDettaglioTestata;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ErroriImmobiliView.ListByIDOggetto.errore: ", Err);
                throw Err;
            }

        }
    }
}
