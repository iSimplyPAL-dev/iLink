using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe di gestione della vista viewErroriDichiarazioni.
	/// </summary>
	public class ErroriDichiarazioniView : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ErroriDichiarazioniView));
        /// <summary>
        /// 
        /// </summary>
        public ErroriDichiarazioniView()
		{
			this.TableName = "viewErroriDichiarazioni";
		}

		/// <summary>
		/// Torna una DataView valorizzata col elenco degli errori filtrati per l'id della
		/// dichiarazione.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <returns></returns>
		public DataView ListByIDTestata(int idDichiarazione)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDDichiarazione=@idDichiarazione";

			SelectCommand.Parameters.Add("@idDichiarazione",SqlDbType.Int).Value = idDichiarazione;
			DataView dv=Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.ErroriDichiarazioniView.ListByIDTestata.errore: ", Err);
                throw Err;
            }
        }

	}
}
