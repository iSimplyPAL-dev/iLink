using System;
using System.Data;
using System.Data.SqlClient;
using Business;
using log4net;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Struct contenente ogni campo, appositamente tipato, 
	/// della tabella TblTestata del DB.
	/// </summary>
	public struct ImportazioniRow
    {
        /// 
		public int ID;
        /// 
        public string Codice;
        /// 
		public string Tributo;
        /// 
		public string Tipologia;
        /// 
		public DateTime Data;
        /// 
		public string Operatore;
        /// 
		public string FileName;
        /// 
		public bool Importato;
	}



    /// <summary>
    /// Classe che interfaccia la tabella TblTestata del DB. 
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ImportazioniTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportazioniTable));
        /// <summary>
        /// Inizializza la tabella gestita
        /// </summary>
        public ImportazioniTable()
		{
			this.TableName = "TblImportazioni";
		}

		#region Insert
		/// <summary>
		/// Inserisce un nuovo elemento nella tabella.
		/// </summary>
		/// <param name="item"> Elemento TestataRow da inserire </param>
		/// <returns> Esito dell'operazione </returns>
		public bool Insert(ImportazioniRow item)
		{
			return Insert(item.Codice, item.Tributo, item.Tipologia, item.Operatore, item.FileName, item.Importato);
		}

		/// <summary>
		/// Inserisce un nuovo elemento nella tabella. 
		/// </summary>
		/// <param name="item">Elemento TestataRow da inserire</param>
		/// <param name="id">Reference all'identificativo del nuovo elemento inserito nel DB</param>
		/// <returns>Esito dell'operazione</returns>
		public bool Insert(ImportazioniRow item, out int id)
		{

			return Insert(item.Codice, item.Tributo, item.Tipologia, item.Operatore, item.FileName, item.Importato, out id);
		}



		/// <summary>
		/// Inserisce un nuovo elemento nella tabella,
		/// ricevendo in input separatamente tutti i campi della tabella. 
		/// </summary>
		/// <param name="codice"></param>
		/// <param name="tributo"></param>
		/// <param name="tipologia"></param>
		/// <param name="operatore"></param>
		/// <param name="fileName"></param>
		/// <param name="importato"></param>
		/// <returns> Esito dell'operazione </returns>
		public bool Insert(string codice, string tributo, string tipologia, string operatore, string fileName, bool importato) 
		{
			SqlCommand insertCommand = ConstructInsertCommand(codice, tributo, tipologia, operatore, fileName, importato);
			return Execute(insertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
		}
        /// <summary>
        /// Inserisce un nuovo elemento nella tabella.
        /// ricevendo in input separatamente tutti i campi della tabella, 
        /// </summary>
        /// <param name="codice"></param>
        /// <param name="tributo"></param>
        /// <param name="tipologia"></param>
        /// <param name="operatore"></param>
        /// <param name="fileName"></param>
        /// <param name="importato"></param>
        /// <param name="id">Reference all'identificativo del nuovo elemento inserito nel DB</param>
        /// <returns> Esito dell'operazione </returns>
        public bool Insert(string codice, string tributo, string tipologia, string operatore, string fileName, bool importato, out int id) 
		{
			SqlCommand insertCommand = ConstructInsertCommand(codice, tributo, tipologia, operatore, fileName, importato);
			return Execute(insertCommand, new SqlConnection(Business.ConstWrapper.StringConnection), out id);
		}
		#endregion

		#region ConstructCommand
		/// <summary>
		/// Costruisce un SqlCommand di tipo INSERT con tutti i campi.
		/// </summary>
		/// <param name="codice"></param>
		/// <param name="tributo"></param>
		/// <param name="tipologia"></param>
		/// <param name="operatore"></param>
		/// <param name="fileName"></param>
		/// <param name="importato"></param>
		/// <returns> SqlCommand costruito.  </returns>
		protected SqlCommand ConstructInsertCommand(string codice, string tributo, string tipologia, string operatore,
			string fileName, bool importato)
		{
			SqlCommand insertCommand = new SqlCommand();
            try { 
			insertCommand.CommandText = "INSERT INTO " + this.TableName +
				"(Codice, Tributo, Tipologia, Data, Operatore, FileName, Importato)"+
				" VALUES (@codice, @tributo, @tipologia, @data, @operatore, @fileName, @importato)";
			insertCommand.Parameters.Add(new SqlParameter("@codice", SqlDbType.VarChar)).Value = codice;
			insertCommand.Parameters.Add(new SqlParameter("@tributo", SqlDbType.VarChar)).Value = tributo;
			insertCommand.Parameters.Add(new SqlParameter("@tipologia", SqlDbType.VarChar)).Value = tipologia;
			insertCommand.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime)).Value = DateTime.Now;
			insertCommand.Parameters.Add(new SqlParameter("@operatore", SqlDbType.VarChar)).Value = operatore;
			insertCommand.Parameters.Add(new SqlParameter("@fileName", SqlDbType.VarChar)).Value = fileName;
			insertCommand.Parameters.Add(new SqlParameter("@importato", SqlDbType.Bit)).Value = importato;

			return insertCommand;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioniTable.ConstructInsertCommand.errore: ", Err);
                throw Err;
            }

        }

		#endregion


		/// <summary>
		/// Costruisce un TestataRow a partire dal record della
		/// tabella con l'identificativo passato. 
		/// </summary>
		/// <param name="id">Identificativo del record ricercato</param>
		/// <returns>TestataRow  del record ricercato se esistente,
		/// altrimenti un TestataRow  allocato</returns>
		public ImportazioniRow GetRow(int id)
		{
			ImportazioniRow RigaImportazione = new ImportazioniRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);

				DataTable tblImportazioni = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (tblImportazioni.Rows.Count > 0)
				{
					RigaImportazione = ReadRow(tblImportazioni.Rows[0]); 
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioniTable.GetRow.errore: ", Err);
                kill();
				RigaImportazione = InizializeTestataRow();
			}
			finally{
				kill();
			}
			return RigaImportazione;
		}

		/// <summary>
		/// Costruisce un TestataRow a partire dal record della
		/// tabella con l'identificativo passato. 
		/// </summary>
		/// <param name="tipologiaProvenienza"></param>
		/// <returns>TestataRow  del record ricercato se esistente,
		/// altrimenti un TestataRow  allocato</returns>
		public ImportazioniRow GetRow(TipologieProvenienza tipologiaProvenienza)
		{
			ImportazioniRow RigaImportazione = new ImportazioniRow();
			try
			{
				SqlCommand SelectCommand = new SqlCommand();
				SelectCommand.CommandText = "Select * From " + this.TableName;
				switch(tipologiaProvenienza)
				{
					case TipologieProvenienza.dichiarazioni:
						SelectCommand.CommandText += " Where Tipologia='D'";
						break;

					case TipologieProvenienza.versamenti:
						SelectCommand.CommandText += " Where Tipologia='V'";
						break;
				}
                //*** 20140630 - TASI ***
                //DataTable tblImportazioni = Query(SelectCommand);
				DataTable tblImportazioni = Query(SelectCommand,new SqlConnection(Business.ConstWrapper.StringConnection));
				if (tblImportazioni.Rows.Count > 0)
				{
					RigaImportazione = ReadRow(tblImportazioni.Rows[tblImportazioni.Rows.Count - 1]); 
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioniTable.GetRow.errore: ", Err);
                kill();
				RigaImportazione = InizializeTestataRow();
			}
			finally
			{
				kill();
			}
			return RigaImportazione;
		}


		/// <summary>
		/// Legge una riga della tabella e ne costruisce un TestataRow. 
		/// </summary>
		/// <param name="row">Riga della tabella.</param>
		/// <returns> TestataRow  letta.</returns>
		public ImportazioniRow ReadRow(DataRow row)
		{
			ImportazioniRow rowImportazioni = new ImportazioniRow();
            try { 
			rowImportazioni.ID = (int) row["ID"];
			rowImportazioni.Codice = row["Codice"].ToString();
			rowImportazioni.Tributo = row["Tributo"].ToString();
			rowImportazioni.Tipologia = row["Tipologia"].ToString();
			rowImportazioni.Data = (DateTime)row["Data"];
			rowImportazioni.Operatore = row["Operatore"].ToString();
			rowImportazioni.FileName = row["FileName"].ToString();
			rowImportazioni.Importato = (bool)row["Importato"];
	
			return rowImportazioni;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioniTable.ReadRow.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Costruisce una TestataRow  inizializzata ai valori di default,
		/// ossia 0 per gli interi ed stringa vuota per le stringhe.
		/// </summary>
		/// <returns>TestataRow di default.</returns>
		public ImportazioniRow InizializeTestataRow()
		{
			ImportazioniRow rowImportazioni = new ImportazioniRow();
            try { 
			rowImportazioni.ID = 0;
			rowImportazioni.Codice = "";
			rowImportazioni.Tributo = "";
			rowImportazioni.Tipologia = "";
			rowImportazioni.Data = DateTime.MinValue;
			rowImportazioni.Operatore = "";
			rowImportazioni.FileName = "";
			rowImportazioni.Importato = false;
			
			return rowImportazioni;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioniTable.InizializeTestataRow.errore: ", Err);
                throw Err;
            }
        }	
	}
}
