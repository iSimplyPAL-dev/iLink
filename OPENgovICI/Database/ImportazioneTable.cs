using System;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// 
    /// </summary>
    public struct ImportazioneRow
    {
        /// 
		public string Entita;
        /// 
		public bool Run;
        /// 
		public int Elemento;
        /// 
		public int MaxElemento;
        /// 
		public DateTime UltimoImport;
    }

    /// <summary>
    /// Classe di gestione della tabella tblImportazione.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ImportazioneTable : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportazioneTable));
        private string _username;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserName"></param>
		public ImportazioneTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "tblImportazione";
		}

		/// <summary>
		/// Inserisce un nuovo record a partire dai singoli campi.
		/// </summary>
		/// <param name="entita"></param>
		/// <param name="run"></param>
		/// <param name="elemento"></param>
		/// <param name="maxElemento"></param>
		/// <returns></returns>
		public bool Insert(string entita, bool run, int elemento, int maxElemento)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try { 
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Entita, Run, Elemento, MaxElemento) " +
				"VALUES (@entita, @run, @elemento, @maxElemento)";

			InsertCommand.Parameters.Add("@entita",SqlDbType.VarChar).Value = entita;
			InsertCommand.Parameters.Add("@run", SqlDbType.Bit).Value = run;
			InsertCommand.Parameters.Add("@elemento", SqlDbType.Int).Value = elemento;
			InsertCommand.Parameters.Add("@maxElemento", SqlDbType.Int).Value = maxElemento;

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.Insert.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Inserisce un nuovo record a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Insert(ImportazioneRow Item)
		{
			return Insert(Item.Entita, Item.Run, Item.Elemento, Item.MaxElemento);
		}

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire dai singoli campi.
		/// </summary>
		/// <param name="entita"></param>
		/// <param name="run"></param>
		/// <param name="elemento"></param>
		/// <param name="maxElemento"></param>
		/// <returns></returns>
		public bool Modify(string entita, bool run, int elemento, int maxElemento)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Run=@run, " +
				"Elemento=@elemento, MaxElemento=@maxElemento, UltimoImport=@ultimoImport " +
				"WHERE Entita=@entita";

			ModifyCommand.Parameters.Add("@entita",SqlDbType.VarChar).Value = entita;
			ModifyCommand.Parameters.Add("@run",SqlDbType.Bit).Value = run;
			ModifyCommand.Parameters.Add("@elemento",SqlDbType.Int).Value = elemento;
			ModifyCommand.Parameters.Add("@maxElemento",SqlDbType.Int).Value = maxElemento;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Aggiorna un record individuato dall'identity a partire da una struttura row.
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		public bool Modify(ImportazioneRow Item)
		{
			return Modify(Item.Entita, Item.Run, Item.Elemento, Item.MaxElemento);
		}

		/// <summary>
		/// Ritorna una struttura row che rappresenta un record individuato dall'entità.
		/// </summary>
		/// <param name="entita"></param>
		/// <returns></returns>
		public ImportazioneRow GetRow(string entita)
		{
			ImportazioneRow rigaImport = new ImportazioneRow();
			try
			{
				SqlCommand SelectCommand = new SqlCommand();
				SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
					" WHERE Entita=@entita";

				SelectCommand.Parameters.Add("@entita", SqlDbType.VarChar).Value = entita;
                //*** 20140630 - TASI ***
				//DataTable Import = Query(SelectCommand);
                DataTable Import = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if(Import.Rows.Count > 0)
				{
					rigaImport.Entita = Import.Rows[0]["Entita"].ToString();
					rigaImport.Run = (bool)Import.Rows[0]["Run"];
					rigaImport.Elemento = (int)Import.Rows[0]["Elemento"];
					rigaImport.MaxElemento = (int)Import.Rows[0]["MaxElemento"];
					rigaImport.UltimoImport = Import.Rows[0]["UltimoImport"] == DBNull.Value ? DateTime.MinValue : (DateTime)Import.Rows[0]["UltimoImport"];
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.GetRow.errore: ", Err);
                kill();
				rigaImport = new ImportazioneRow();
			}
			finally{
				kill();
			}
			return rigaImport;
		}

		/// <summary>
		/// Modifica la data dell'ultimo import delle dichiarazioni e se in corso o no.
		/// </summary>
		/// <param name="entita"></param>
		/// <param name="ultimoImport"></param>
		/// <param name="run"></param>
		/// <returns></returns>
		public bool Modify(string entita, DateTime ultimoImport, bool run)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET " +
				"UltimoImport=@ultimoImport, Run=@run WHERE Entita=@entita";

			ModifyCommand.Parameters.Add("@ultimoImport",SqlDbType.DateTime).Value = ultimoImport == DateTime.MinValue ? DBNull.Value : (object)ultimoImport;
			ModifyCommand.Parameters.Add("@entita",SqlDbType.VarChar).Value = entita;
			ModifyCommand.Parameters.Add("@run",SqlDbType.Bit).Value = run;

            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Scrive nel database le informazioni relative allo stato d'importazione.
		/// </summary>
		/// <param name="entita"></param>
		/// <param name="run"></param>
		/// <param name="elemento"></param>
		/// <param name="ultimoImport"></param>
		/// <returns></returns>
		public bool WriteInfoImport(string entita, bool run, int elemento, DateTime ultimoImport)
		{
			bool retval = false;
            try { 
			if(GetRow(entita).Entita == null)
			{
				retval = Insert(entita, run, elemento, 0);
			}
			else
			{
				if(ultimoImport == DateTime.MinValue)
					retval = Modify(entita, run, elemento, 0);
				else
					retval = Modify(entita, ultimoImport, run);
			}

			return retval;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.WriteInfoImport.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Costruisce un datatable con gli ultimi dati di importazione partirtendo 
		/// dalla tipologia di provenienza e dal codice ente.
		/// </summary>
		/// <param name="tipologiaProvenienza"></param>
		/// <param name="codiceEnte"></param>
		/// <returns></returns>
		public DataTable GetRowUni(string tipologiaProvenienza, string codiceEnte)
		{
//			try
//			{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "Select top 1 * From tblImportazioni ";

			switch(tipologiaProvenienza)
			{
				case "dichiarazioni":
					SelectCommand.CommandText += " Where Tipologia='D'";
					break;

				case "versamenti":
					SelectCommand.CommandText += " Where Tipologia='V'";
					break;
			}

			SelectCommand.CommandText += " and codice=" + codiceEnte;
			SelectCommand.CommandText += " order by ID desc";


            //			}
            //			catch  
            //			{
            //log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.GetRowUni.errore: ", Err);
           //    throw Err;
            //				RigaImportazione = InizializeTestataRow();
            //			}
            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
		}

		/// <summary>
		/// Costruisce un datatable con i dati dell'ultima importazione a partire dalla tipologia di provenienza,
		/// dal codice ente e dal tipo importazione
		/// </summary>
		/// <param name="tipologiaProvenienza"></param>
		/// <param name="codiceEnte"></param>
		/// <param name="tipoImportazione"></param>
		/// <returns>Datatable del record ricercato</returns>
		public DataTable GetRowImportazioni(string tipologiaProvenienza, string codiceEnte, string tipoImportazione)
		{
            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "SELECT TOP 1 * FROM TBLIMPORTAZIONI WHERE TRIBUTO='8852'";
                switch (tipologiaProvenienza)
                {
                    case "dichiarazioni":
                        SelectCommand.CommandText += " AND TIPOLOGIA='D'";
                        break;

                    case "versamenti":
                        SelectCommand.CommandText += " AND TIPOLOGIA='V'";
                        break;
                }
                SelectCommand.CommandText += " AND CODICE=" + codiceEnte;
                SelectCommand.CommandText += " AND TIPOIMPORTAZIONE = '" + tipoImportazione + "'";
                SelectCommand.CommandText += " ORDER BY ID DESC";
                //*** 20140630 - TASI ***
                //DataTable dt = Query(SelectCommand);
                DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
                //*** ***
			    return dt;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.GetRowImportazioni.errore: ", ex);
                return null;
            }
            finally
            {
                kill();
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="codEnte"></param>
		/// <param name="tipologia"></param>
		/// <param name="operatore"></param>
		/// <param name="fileName"></param>
		/// <param name="totrecord"></param>
		/// <param name="totRecordImp"></param>
		/// <param name="ImportoTotFile"></param>
		/// <param name="importoTotImport"></param>
		/// <returns></returns>
		public bool InsertTblImportazioni(string codEnte,string tipologia,string operatore, string fileName, int totrecord, int totRecordImp, string ImportoTotFile, string importoTotImport)
		{
			SqlCommand InsertCommand = new SqlCommand();
			int importato;
			string data;
			string giorno,mese;
			int giornoI,meseI;
			int ngiorno,nmese;
			
			giornoI = DateTime.Now.Day;
			giorno = Convert.ToString(giornoI);
			ngiorno=giorno.Length;
			if (ngiorno ==1)
				giorno = "0" + giorno;
            //else
            //    giorno=giorno;

			meseI = DateTime.Now.Month;
			mese= Convert.ToString(meseI);
			nmese=mese.Length;
            try { 

            if (nmese == 1)
                mese = "0" + mese;
            //else
            //    mese=mese;

            data = DateTime.Now.Year + "-" + mese + "-" + giorno;
             InsertCommand.CommandText = "INSERT INTO tblImportazioni " +
                "(Codice, Tributo, Tipologia, Data, Operatore,FileName, Importato, toTRecord,toTRecordImportati," +
                " importTotFile, importoTotImportato) " +
                "VALUES (" +
                "'" + codEnte + "','8852','" + tipologia + "','" + data + "','" + operatore + "','" + fileName + "',";

            if (totrecord == totRecordImp)
                importato = 1;
            else
                importato = 0;

            InsertCommand.CommandText += importato + ",'" + totrecord + "','" + totRecordImp + "'," +
                "'" + ImportoTotFile + "','" + importoTotImport + "')";

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.InsertTblImportazioni.errore: ", ex);
                throw ex;
            }

            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objImportazioni"></param>
        /// <returns></returns>
        public bool InsertTblImportazioni(TblImportazioni objImportazioni)
		{
			SqlCommand InsertCommand = new SqlCommand();
			int importato;
			string data;
			string giorno,mese;
			int giornoI,meseI;
			int ngiorno,nmese;
			
			giornoI = DateTime.Now.Day;
			giorno = Convert.ToString(giornoI);
			ngiorno=giorno.Length;
            try { 
			if (ngiorno ==1)
				giorno = "0" + giorno;
            //else
            //    giorno=giorno;

			meseI = DateTime.Now.Month;
			mese= Convert.ToString(meseI);
			nmese=mese.Length;
			if(nmese ==1)
				mese= "0" + mese;
            //else
            //    mese=mese;

			data = DateTime.Now.Year + "-" + mese + "-" + giorno;
			
			InsertCommand.CommandText = "INSERT INTO tblImportazioni " +
				"(Codice, Tributo, Tipologia, Data, Operatore,[FileName], Importato, toTRecord,toTRecordImportati," + 
				" importTotFile, importoTotImportato, tipoImportazione) " +
				"VALUES (" +
				"'" + objImportazioni.Codice + "','" + objImportazioni.Tributo + "','" + objImportazioni.Tipologia + "','" + data + "','" + 
				objImportazioni.Operatore + "','" + objImportazioni.FileName + "'," ;
			
			if(objImportazioni.toTRecord == objImportazioni.toTRecordImportati)
				importato = 1;
			else
				importato = 0;

			InsertCommand.CommandText += importato + ",'" + objImportazioni.toTRecord + "','" + objImportazioni.toTRecordImportati + "'," +
				"'" + objImportazioni.importTotFile + "','" + objImportazioni.importoTotImportato + "','" + objImportazioni.tipoImportazione + "')";

            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneTable.InsertTblImportazioni.errore: ", ex);
                throw ex;
            }
        }
	}

	/// <summary>
	/// 
	/// </summary>
	public class TblImportazioni
	{
		private string _Codice;
		private string _Tributo;
		private string _Tipologia;
		private string _Data	;
		private string _Operatore;
		private string _FileName	;
		private int _Importato;
		private int _toTRecord;
		private int _toTRecordImportati;
		private string _importTotFile;
		private string _importoTotImportato;
		private string _tipoImportazione;

		/// <summary>
		/// 
		/// </summary>
		public string Codice
		{
			get
			{
				return this._Codice;
			}
			set
			{
				this._Codice = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Tributo
		{
			get
			{
				return this._Tributo;
			}
			set
			{
				this._Tributo = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Tipologia
		{
			get
			{
				return this._Tipologia;
			}
			set
			{
				this._Tipologia = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Operatore
		{
			get
			{
				return this._Operatore;
			}
			set
			{
				this._Operatore = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				this._FileName = value;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int Importato
		{
			get
			{
				return this._Importato;
			}
			set
			{
				this._Importato = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int toTRecord
		{
			get
			{
				return this._toTRecord;
			}
			set
			{
				this._toTRecord = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int toTRecordImportati
		{
			get
			{
				return this._toTRecordImportati;
			}
			set
			{
				this._toTRecordImportati = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string importTotFile
		{
			get
			{
				return this._importTotFile;
			}
			set
			{
				this._importTotFile = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string importoTotImportato
		{
			get
			{
				return this._importoTotImportato;
			}
			set
			{
				this._importoTotImportato = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string tipoImportazione
		{
			get
			{
				return this._tipoImportazione;
			}
			set
			{
				this._tipoImportazione = value;
			}
		}

	}
}

