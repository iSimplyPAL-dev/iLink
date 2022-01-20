using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Business;
//using CreaTracciatoPOSTEL;
using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione degli F24
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ImportazioneF24 : Database
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ImportazioneF24));
		public ImportazioneF24()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oFlusso"></param>
		/// <returns></returns>
		//*** 20120828 - IMU adeguamento per importi statali *** 
		public DataTable DatiPerVersamenti(ObjFlussi oFlusso)
		{
			SqlCommand selectCommand = new SqlCommand();
            try { 
			selectCommand.CommandText = "SELECT COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, CF_PIVA, COGNOME, NOME, ANNO, FLAG_RAVVEDIMENTO_OPEROSO, FLAG_ACCONTO, FLAG_SALDO, ";
			selectCommand.CommandText += "FLAG_ACCONTO_SALDO, SUM(CASE WHEN COD_TRIBUTO='3901' OR COD_TRIBUTO='3904' OR COD_TRIBUTO='3912' OR COD_TRIBUTO='3918' THEN N_FAB ELSE 0 END) AS NFAB, ";
			selectCommand.CommandText += "DATA_ACCREDITO, DATA_VERSAMENTO, FLAG_ANNO_ERRATO = CASE WHEN SUM(FLAG_ANNO_ERRATO) > 0 THEN  1 ELSE 0 END, ";
			selectCommand.CommandText += "FLAG_DATI_ERRATI = CASE WHEN SUM(FLAG_DATI_ERRATI) > 0 THEN  1 ELSE 0 END, ";
			//***20081216 - aggiunto la detrazione statale per il codice tributo 3900***
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3900' THEN IMPORTO ELSE 0 END) AS DETRAZIONE_STATALE, ";
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3901' OR COD_TRIBUTO='3940' OR COD_TRIBUTO='3912' THEN IMPORTO ELSE 0 END) AS ABIPRIN, ";
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3902' OR COD_TRIBUTO='3941' OR COD_TRIBUTO='3914' THEN IMPORTO ELSE 0 END) AS TERAGR, ";
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3903' OR COD_TRIBUTO='3942' OR COD_TRIBUTO='3916' THEN IMPORTO ELSE 0 END) AS AREEFAB, ";
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3904' OR COD_TRIBUTO='3943' OR COD_TRIBUTO='3918' THEN IMPORTO ELSE 0 END) AS ALTRIFAB, ";
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3906' OR COD_TRIBUTO='3923' THEN IMPORTO ELSE 0 END) AS INTERESSI, ";
			selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3907' OR COD_TRIBUTO='3924' THEN IMPORTO ELSE 0 END) AS SANZIONI";

			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='3913' THEN IMPORTO ELSE 0 END) AS FAB_RUR_USO_STRUM";
			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='3915' THEN IMPORTO ELSE 0 END) AS TERAGR_STATALE";
			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='3917' THEN IMPORTO ELSE 0 END) AS AREEFAB_STATALE";
			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='3919' THEN IMPORTO ELSE 0 END) AS ALTRIFAB_STATALE";
			//*** 20130422 - aggiornamento IMU ***
			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='xxxx' THEN IMPORTO ELSE 0 END) AS FAB_RUR_USO_STRUM_STATALE";//ha lo stesso codice di ALTRIFAB_STATALE quindi va tutto là
			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='3930' THEN IMPORTO ELSE 0 END) AS USOPRODCATD";
			selectCommand.CommandText += ",SUM(CASE WHEN COD_TRIBUTO='3925' THEN IMPORTO ELSE 0 END) AS USOPRODCATD_STATALE";
			//*** ***
			selectCommand.CommandText += ", SUM(IMPORTO) AS TOTVERSATO, SUM(DETRAZIONE) AS DETRAZIONE ";
			selectCommand.CommandText += "FROM TAB_ACQUISIZIONE_F24 ";
			selectCommand.CommandText += "WHERE COD_BELFIORE='" + oFlusso.CodBelfiore + "' AND PROVENIENZA='" + oFlusso.Provenienza + "' AND DATA_CREAZIONE='" + oFlusso.DataCreazione + "'  ";
			selectCommand.CommandText += "AND IDENTIFICATIVO='" + oFlusso.Identificativo + " ' ";
			selectCommand.CommandText += "GROUP BY COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, CF_PIVA, COGNOME, NOME, ANNO, FLAG_RAVVEDIMENTO_OPEROSO, FLAG_ACCONTO, FLAG_SALDO, ";
			selectCommand.CommandText += "FLAG_ACCONTO_SALDO, DATA_ACCREDITO, DATA_VERSAMENTO ";
			selectCommand.CommandText += "ORDER BY COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, DATA_ACCREDITO, DATA_VERSAMENTO, CF_PIVA, ANNO";

            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.DatiPerVersamenti.errore: ", Err);
                throw Err;
            }

        }

        /*
        public DataTable DatiPerVersamenti(ObjFlussi oFlusso)
       {
           SqlCommand selectCommand = new SqlCommand();
           try{
           selectCommand.CommandText = "SELECT COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, CF_PIVA, COGNOME, NOME, ANNO, FLAG_RAVVEDIMENTO_OPEROSO, FLAG_ACCONTO, FLAG_SALDO, ";
           selectCommand.CommandText += "FLAG_ACCONTO_SALDO, SUM(CASE WHEN COD_TRIBUTO='3901' OR COD_TRIBUTO='3904' THEN N_FAB ELSE 0 END) AS NFAB, ";
           selectCommand.CommandText += "DATA_ACCREDITO, DATA_VERSAMENTO, FLAG_ANNO_ERRATO = CASE WHEN SUM(FLAG_ANNO_ERRATO) > 0 THEN  1 ELSE 0 END, ";
           selectCommand.CommandText += "FLAG_DATI_ERRATI = CASE WHEN SUM(FLAG_DATI_ERRATI) > 0 THEN  1 ELSE 0 END, ";
           //***20081216 - aggiunto la detrazione statale per il codice tributo 3900***
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3900' THEN IMPORTO ELSE 0 END) AS DETRAZIONE_STATALE, ";
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3901' THEN IMPORTO ELSE 0 END) AS ABIPRIN, ";
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3902' THEN IMPORTO ELSE 0 END) AS TERAGR, ";
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3903' THEN IMPORTO ELSE 0 END) AS AREEFAB, ";
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3904' THEN IMPORTO ELSE 0 END) AS ALTRIFAB, ";
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3906' THEN IMPORTO ELSE 0 END) AS INTERESSI, ";
           selectCommand.CommandText += "SUM(CASE WHEN COD_TRIBUTO='3907' THEN IMPORTO ELSE 0 END) AS SANZIONI, SUM(IMPORTO) AS TOTVERSATO, SUM(DETRAZIONE) AS DETRAZIONE ";
           selectCommand.CommandText += "FROM TAB_ACQUISIZIONE_F24 ";
           selectCommand.CommandText += "WHERE COD_BELFIORE='" + oFlusso.CodBelfiore + "' AND PROVENIENZA='" + oFlusso.Provenienza + "' AND DATA_CREAZIONE='" + oFlusso.DataCreazione + "'  ";
           selectCommand.CommandText += "AND IDENTIFICATIVO='" + oFlusso.Identificativo + " ' ";
           selectCommand.CommandText += "GROUP BY COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, CF_PIVA, COGNOME, NOME, ANNO, FLAG_RAVVEDIMENTO_OPEROSO, FLAG_ACCONTO, FLAG_SALDO, ";
           selectCommand.CommandText += "FLAG_ACCONTO_SALDO, DATA_ACCREDITO, DATA_VERSAMENTO ";
           selectCommand.CommandText += "ORDER BY COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, DATA_ACCREDITO, DATA_VERSAMENTO, CF_PIVA, ANNO";

           DataTable dt=Query(selectCommand);
           kill();
           return dt;
                       }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.DatiPerVersamenti.errore: ", Err);
                throw Err;
            }
       }*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable DatiFileImportati()
		{
			SqlCommand selectCommand = new SqlCommand();
            try { 
			selectCommand.CommandText = "SELECT COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, IDENTIFICATIVO";
            selectCommand.CommandText += " FROM TAB_ACQUISIZIONE_F24";
            selectCommand.CommandText += " GROUP BY COD_BELFIORE, DATA_CREAZIONE, PROVENIENZA, IDENTIFICATIVO";

            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.DatiFileImportanti.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object NImportazioni()
		{
			SqlCommand selectCommand = new SqlCommand();
            try { 
			selectCommand.CommandText = "SELECT count(*) as totF24";
			selectCommand.CommandText += " FROM TAB_ACQUISIZIONE_F24";

            return QueryScalar(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.NImportazioni.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CancellaDatiTabF24()
		{
			SqlCommand deleteCommand = new SqlCommand();
            try { 
			deleteCommand.CommandText ="DELETE FROM TAB_ACQUISIZIONE_F24";

            return Execute(deleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.CancellaDatiTabF24.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataTable ProvenienzaF24(){
			SqlCommand selectCommand = new SqlCommand();
            try { 
			selectCommand.CommandText="SELECT * FROM TblProvenienze WHERE (Descrizione LIKE 'F24')";
            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.ProvenienzaF24.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rigaVersamenti"></param>
		/// <returns></returns>
		public string insertVersamenti(VersamentiImportazione[] rigaVersamenti)
		{
			SqlCommand InsertCommand = new SqlCommand();
			int nSalvati=0;
			double ImportoTotImp =0;
			double importoPagato = 0;
			string lingua_date= ConfigurationManager.AppSettings["lingua_date"];
			
			SqlConnection oConn = new SqlConnection() ;
			string connessione = ConfigurationManager.AppSettings["connectionStringOpenGovICI"];
			oConn.ConnectionString = connessione;
			oConn.Open();
			
			try
			{
				InsertCommand.Connection=oConn;
                foreach (VersamentiImportazione myRow in rigaVersamenti)
                {
					InsertCommand.CommandText = "INSERT INTO TblVersamenti (Ente, IdAnagrafico, AnnoRiferimento, " +
						"CodiceFiscale, PartitaIva, ImportoPagato," +
						"DataPagamento, NumeroBollettino, NumeroFabbricatiPosseduti," +
						" Acconto, Saldo, RavvedimentoOperoso, " +
						"ImpoTerreni, ImportoAreeFabbric, ImportoAbitazPrincipale, " +
						"ImportoAltriFabbric, DetrazioneAbitazPrincipale, ContoCorrente, " +
						//*** 20120828 - IMU adeguamento per importi statali ***
						"IMPORTOTERRENISTATALE, IMPORTOAREEFABBRICSTATALE, IMPORTOALTRIFABBRICSTATALE, IMPORTOFABRURUSOSTRUM, "+
						//*** 20130422 - aggiornamento IMU ***
						"IMPORTOFABRURUSOSTRUMSTATALE, "+
						"IMPORTOUSOPRODCATD, "+
						"IMPORTOUSOPRODCATDSTATALE, "+
						//***20081216 - aggiunto la detrazione statale***
						"DetrazioneStatale," +
						"ComuneUbicazioneImmobile, ComuneIntestatario, Bonificato, " +
						"DataInizioValidità, DataFineValidità, Operatore, " +
						"Annullato, ImportoSoprattassa, ImportoPenaPecuniaria, " +
						"Interessi, Violazione, IDProvenienza, " +
						"NumeroAttoAccertamento, DataProvvedimentoViolazione, " +
						"ImportoPagatoArrotondamento, DataRiversamento) " +
						"VALUES (" +
						"'" + myRow.Ente + "'," + myRow.IdAnagrafico + ",'" + myRow.AnnoRiferimento + "'," +
						"'" + myRow.CodiceFiscale + "','" + myRow.PartitaIva + "'," + myRow.ImportoPagato + "," +
						"'" + DateTime.Parse(myRow.DataPagamento).ToString(lingua_date).Replace(".", ":") +
						"','" + myRow.NumeroBollettino + "'," + myRow.NumeroFabbricatiPosseduti + "," ;
					//					"null,'" + myRow.NumeroBollettino + "'," + myRow.NumeroFabbricatiPosseduti + "," ;
					if(myRow.Acconto == true)
						InsertCommand.CommandText += "1,";
					else
						InsertCommand.CommandText += "0,";

					if(myRow.Saldo == true)
						InsertCommand.CommandText += "1,";
					else
						InsertCommand.CommandText += "0,";

					if(myRow.RavvedimentoOperoso == true)
						InsertCommand.CommandText += "1,";
					else
						InsertCommand.CommandText += "0,";

					InsertCommand.CommandText += myRow.ImpoTerreni + "," + myRow.ImportoAreeFabbric +"," + myRow.ImportoAbitazPrincipale + ",";
					InsertCommand.CommandText +=myRow.ImportoAltriFabbric + "," + myRow.DetrazioneAbitazPrincipale + ",'" + myRow.ContoCorrente +"'," ;
					//*** 20120828 - IMU adeguamento per importi statali ***
					InsertCommand.CommandText += myRow.ImpoTerreniStatale + "," + myRow.ImportoAreeFabbricStatale +"," + myRow.ImportoAltriFabbricStatale + "," + myRow.ImportoFabRurUsoStrum + ",";
					//*** 20130422 - aggiornamento IMU ***
					InsertCommand.CommandText += myRow.ImportoFabRurUsoStrumStatale + ",";
					InsertCommand.CommandText += myRow.ImportoUsoProdCatD + ",";
					InsertCommand.CommandText += myRow.ImportoUsoProdCatDStatale + ",";
					//*** ***
					InsertCommand.CommandText +=myRow.DetrazioneStatale + "," ;
					InsertCommand.CommandText +="'" + myRow.ComuneUbicazioneImmobile + "','" +myRow.ComuneIntestatario + "',";

					if(myRow.Bonificato == true)
						InsertCommand.CommandText += "1,";
					else
						InsertCommand.CommandText += "0,";

					InsertCommand.CommandText +="'" + DateTime.Parse(myRow.DataInizioValidità).ToString(lingua_date).Replace(".", ":") + "',";
					InsertCommand.CommandText +="'" + DateTime.Parse(myRow.DataFineValidità).ToString(lingua_date).Replace(".", ":") + "',";
					InsertCommand.CommandText +="'" + myRow.Operatore + "'," ;
					//InsertCommand.CommandText +="null,null,'" + myRow.Operatore + "'," ;

					if(myRow.Annullato == true)
						InsertCommand.CommandText += "1,";
					else
						InsertCommand.CommandText += "0,";

					InsertCommand.CommandText += myRow.ImportoSoprattassa + "," + myRow.ImportoPenaPecuniaria + "," ;
					InsertCommand.CommandText +=myRow.Interessi + ",";
				
					if(myRow.Violazione == true)
						InsertCommand.CommandText += "1,";
					else
						InsertCommand.CommandText += "0,";

					InsertCommand.CommandText += myRow.IDProvenienza + "," ;
					InsertCommand.CommandText += "'" + myRow.NumeroAttoAccertamento + "',";

					if (myRow.DataProvvedimentoViolazione != "")
						InsertCommand.CommandText +="'" + DateTime.Parse(myRow.DataProvvedimentoViolazione).ToString(lingua_date).Replace(".", ":") + "',";
					else 
						InsertCommand.CommandText +="null,";
				
					InsertCommand.CommandText +=myRow.ImportoPagatoArrotondamento + "," ;
					InsertCommand.CommandText +=  "@DataRiversamento)";

					DateTime oDataRiv = DateTime.MinValue;
					if (myRow.DataRiversamento.Length > 1)
					{
						oDataRiv = new DateTime(int.Parse(myRow.DataRiversamento.Substring(0,4).ToString()), int.Parse(myRow.DataRiversamento.Substring(5,2).ToString()), int.Parse(myRow.DataRiversamento.Substring(8,2).ToString()));
					}
				
					InsertCommand.Parameters.Clear();
					InsertCommand.Parameters.Add("@DataRiversamento",oDataRiv.ToString(lingua_date).Replace(".", ":"));

                    log.Debug(Utility.Costanti.LogQuery(InsertCommand));
                    InsertCommand.ExecuteNonQuery();

					importoPagato = Convert.ToDouble(myRow.ImportoPagatoDouble);
					ImportoTotImp = ImportoTotImp + importoPagato;

					nSalvati++;
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ImportazioneF24.insertVersamenti.errore: ", Err);
           	}
			oConn.Close();
			return nSalvati + "-" + ImportoTotImp;
		}


	}

	/// <summary>
	/// 
	/// </summary>
	public class ObjFlussi
	{
		private string sCOD_BELFIORE;
		private string sDATA_CREAZIONE;
		private string sPROVENIENZA;
		private string sIDENTIFICATIVO;

		/// <summary>
		/// 
		/// </summary>
		public string CodBelfiore
		{
			get
			{
				return this.sCOD_BELFIORE;
			}
			set
			{
				this.sCOD_BELFIORE = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DataCreazione
		{
			get
			{
				return this.sDATA_CREAZIONE;
			}
			set
			{
				this.sDATA_CREAZIONE = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Provenienza
		{
			get
			{
				return this.sPROVENIENZA;
			}
			set
			{
				this.sPROVENIENZA = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Identificativo
		{
			get
			{
				return this.sIDENTIFICATIVO;
			}
			set
			{
				this.sIDENTIFICATIVO = value;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public struct VersamentiImportazione
    {
        /// 
		public int ID;
        /// 
		public string Ente;
        /// 
		public int IdAnagrafico;
        /// 
		public string AnnoRiferimento;
        /// 
		public string CodiceFiscale;
        /// 
		public string PartitaIva;
        /// 
		public string ImportoPagato;
        /// 
		public string DataPagamento;
        /// 
		public string NumeroBollettino;
        /// 
		public int NumeroFabbricatiPosseduti;
        /// 
		public bool Acconto;
        /// 
		public bool Saldo;
        /// 
		public bool RavvedimentoOperoso;
        /// 
		public string ImpoTerreni;
        /// 
		public string ImportoAreeFabbric;
        /// 
		public string ImportoAbitazPrincipale;
        /// 
		public string ImportoAltriFabbric;
        /// 
		public string DetrazioneAbitazPrincipale;
        /// 
        //*** 20120828 - IMU adeguamento per importi statali *** 
        public string ImportoFabRurUsoStrum;
        /// 
		public string ImpoTerreniStatale;
        /// 
		public string ImportoAreeFabbricStatale;
        /// 
		public string ImportoAltriFabbricStatale;
        /// 
        //*** 20130422 - aggiornamento IMU ***
        public string ImportoFabRurUsoStrumStatale;
        /// 
		public string ImportoUsoProdCatD;
        /// 
		public string ImportoUsoProdCatDStatale;
        /// 
        //***20081216 - aggiunto la detrazione statale***
        public string DetrazioneStatale;
        /// 
		public string ContoCorrente;
        /// 
		public string ComuneUbicazioneImmobile;
        /// 
		public string ComuneIntestatario;
        /// 
		public bool Bonificato;
        /// 
		public string DataInizioValidità;
        /// 
		public string DataFineValidità;
        /// 
		public string DataRiversamento;
        /// 
		public string Operatore;
        /// 
		public bool Annullato;
        /// 
		public decimal ImportoSoprattassa;
        /// 
		public decimal ImportoPenaPecuniaria;
        /// 
		public string Interessi;
        /// 
		public bool Violazione;
        /// 
		public int IDProvenienza;
        /// 
		public string NumeroAttoAccertamento;
        /// 
		public string DataProvvedimentoViolazione;
        /// 
		public string ImportoPagatoArrotondamento;
        /// 
		public double ImportoPagatoDouble;

	}
	

}