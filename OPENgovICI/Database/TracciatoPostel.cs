using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Business;
using CreaTracciatoPOSTEL;
using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// Classe per la gestione dei dati del tracciato Postel
	/// </summary>
	public class TracciatoPostel : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(TracciatoPostel));
        public TracciatoPostel(){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataTable TrovaLetteraPostel(string ente, string anno)
		{
			SqlCommand selectCommand =new SqlCommand();
            try { 
			selectCommand.CommandText = "SELECT * ";
			selectCommand.CommandText += "FROM TBLLETTERAICIPOSTEL ";
			selectCommand.CommandText += "WHERE(IDENTE = '"+ ente +"') AND (ANNO = '"+ anno +"')";

            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.TrovaLetteraPostel.errore: ", Err);
                throw Err;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operazione"></param>
        /// <param name="anno"></param>
        /// <param name="ente"></param>
        /// <param name="oRigaLettera"></param>
        /// <param name="oResponsabile"></param>
        /// <returns></returns>
        public bool SetLetteraPostel(int operazione,string anno, string ente, OggettoRigaLettera[] oRigaLettera, OggettoResponsabileComune oResponsabile)
		{

			SqlCommand selectCommand = new SqlCommand();
            try { 
			switch(operazione)
			{
				case 1:
				{
					selectCommand.CommandText = "UPDATE TBLLETTERAICIPOSTEL SET ANNO = '" + anno + "'";
					if (oResponsabile.Nominativo != "")
						selectCommand.CommandText += ",RESPONSABILE='" + oResponsabile.Nominativo.Replace("'","''") + "'";
					else
						selectCommand.CommandText += ",RESPONSABILE=''";
					
					if (oResponsabile.Tel != "")
						selectCommand.CommandText += ", TELEFONO='" + oResponsabile.Tel.Replace("'", "''") + "'";
					else
						selectCommand.CommandText += ", TELEFONO=''";

					if (oResponsabile.Fax !="")
						selectCommand.CommandText += ", FAX='" + oResponsabile.Fax.Replace("'", "''") + "'";
					else
						selectCommand.CommandText += ", FAX=''";

					for(int iCount=0; iCount<42; iCount++)
					{
						if (oRigaLettera[iCount].Riga != "")
							selectCommand.CommandText += ", " + "RIGA" + (iCount + 1) + "='" + oRigaLettera[iCount].Riga.Replace("'", "''") + "'";
						else
							selectCommand.CommandText += ", " + "RIGA" + (iCount + 1) + "=''";
					}

					selectCommand.CommandText += " WHERE IDENTE='" + ente + "'";
					break;
				}
				case 0:
				{
					string SqlInsert="";
					string SqlValues= "";
					string sSql ="";

					SqlInsert = "INSERT INTO TBLLETTERAICIPOSTEL (IDENTE, ANNO";
					SqlValues = " VALUES ('" + ente + "','" + anno + "'";
					if (oResponsabile.Nominativo != "")
					{
						SqlInsert += ",RESPONSABILE";
						SqlValues += ",'" + oResponsabile.Nominativo.Replace("'", "''") + "'";
					}
					else
					{
						SqlInsert += ",RESPONSABILE";
						SqlValues += ",''";
					}
					if (oResponsabile.Tel != "")
					{
						SqlInsert += ",TELEFONO";
						SqlValues += ",'" + oResponsabile.Tel.Replace("'", "''") + "'";
					}
					else
					{
						SqlInsert += ",TELEFONO";
						SqlValues += ",''";
					}
					if (oResponsabile.Fax != "")
					{
						SqlInsert += ",FAX";
						SqlValues += ",'" + oResponsabile.Fax.Replace("'", "''") + "'";
					}
					else
					{
						SqlInsert += ",FAX";
						SqlValues += ",''";
					}


					for (int iCount=0; iCount<42; iCount++)
					{
						SqlInsert += "," + ("RIGA" + (iCount + 1));
						if (oRigaLettera[iCount].Riga != "")
							SqlValues += ",'" + oRigaLettera[iCount].Riga.Replace("'", "''") + "'";
						else
							SqlValues += ",''";
					}

					SqlInsert += ")";
                    SqlValues += ")";
					sSql = SqlInsert + SqlValues;

					selectCommand.CommandText = sSql;
					break;
				}
			}

            return Execute(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.SetLetteraPostel.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns></returns>
		public DataTable datiPostel (string anno, string ente)
		{
			SqlCommand selectCommand = new SqlCommand();
            try {
		 	selectCommand.CommandText = "SELECT * ";
			selectCommand.CommandText = "FROM TP_CALCOLO_FINALE_ICI ";
			selectCommand.CommandText = "WHERE (COD_ENTE = '" + ente + "') AND (ANNO = '"+ anno +"')";

            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.datiPostel.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataTable datiAnagrafici (string ente, string anno)
		{
			string NomeDbAnag= ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();
						
			SqlCommand selectCommand = new SqlCommand();
            try { 
//			selectCommand.CommandText = "SELECT " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE, " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, ";
//			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.NOME, " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE, ";
//			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA, ";
//			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.VIA_RES, " + NomeDbAnag + ".dbo.ANAGRAFICA.CIVICO_RES, ";
//            selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.CAP_RES, "+ NomeDbAnag +".dbo.ANAGRAFICA.COMUNE_RES, ";
//            selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.PROVINCIA_RES, " + NomeDbAnag + ".dbo.ANAGRAFICA.FRAZIONE_RES, ";
//			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO, ";
//			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO, ";
//			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE ";
//			selectCommand.CommandText += "FROM TP_CALCOLO_FINALE_ICI INNER JOIN ";
//			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA ON ";
//			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.COD_CONTRIBUENTE = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AND ";
//			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.COD_ENTE = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE ";
//			selectCommand.CommandText += "WHERE (TP_CALCOLO_FINALE_ICI.COD_ENTE = '" + ente + "') AND (TP_CALCOLO_FINALE_ICI.ANNO = '" + anno + "') AND ";
//			selectCommand.CommandText += "(" + NomeDbAnag + ".dbo.ANAGRAFICA.DATA_FINE_VALIDITA IS NULL)";


			selectCommand.CommandText = "SELECT " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE, " + NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.COGNOME_INVIO, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.NOME_INVIO, " + NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.COMUNE_RCP, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.PROVINCIA_RCP, " + NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.CAP_RCP, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.VIA_RCP, " + NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.CIVICO_RCP, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.FRAZIONE_RCP, " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COMUNE_RES, " + NomeDbAnag + ".dbo.ANAGRAFICA.PROVINCIA_RES, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.CAP_RES, " + NomeDbAnag + ".dbo.ANAGRAFICA.VIA_RES, " + NomeDbAnag + ".dbo.ANAGRAFICA.FRAZIONE_RES, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.CIVICO_RES, " + NomeDbAnag + ".dbo.ANAGRAFICA.NAZIONALITA_RES, TP_CALCOLO_FINALE_ICI.ANNO, ";
			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_ABITAZIONE_PRINCIPALE_ACCONTO, ";
			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_ABITAZIONE_PRINCIPALE_SALDO, TP_CALCOLO_FINALE_ICI.ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE, ";
			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.CODELINE_ACCONTO, TP_CALCOLO_FINALE_ICI.CODELINE_SALDO, ";
			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.CODELINE_UNICA_SOLUZIONE ";
			selectCommand.CommandText += "FROM " + NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE RIGHT OUTER JOIN ";
			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI INNER JOIN ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA ON TP_CALCOLO_FINALE_ICI.COD_ENTE = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE AND ";
			selectCommand.CommandText += "TP_CALCOLO_FINALE_ICI.COD_CONTRIBUENTE = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE ON ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.COD_CONTRIBUENTE = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE ";
			selectCommand.CommandText += "WHERE (" + NomeDbAnag + ".dbo.ANAGRAFICA.DATA_FINE_VALIDITA IS NULL) AND ";
			selectCommand.CommandText += "(" + NomeDbAnag + ".dbo.INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA IS NULL) and (" + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE = '" + ente + "') ";
			selectCommand.CommandText += "and (TP_CALCOLO_FINALE_ICI.ANNO = '" + anno + "') ";
			selectCommand.CommandText += "order by " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME, ";
			selectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA";

            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.datiAnagrafici.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="anno"></param>
		/// <returns></returns>
		public DataTable datiEstrazione(string ente, string anno){
			SqlCommand selectCommand = new SqlCommand();
            try {
			selectCommand.CommandText = "SELECT * FROM TBLLOTTIPOSTEL";
            selectCommand.CommandText += " WHERE IDENTE='" + ente + "' AND TIPO_SERVIZIO='I' AND ANNO='" + anno + "'";
			selectCommand.CommandText += " order by numero_lotto desc";

            DataTable dt = Query(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.datiEstrazione.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns></returns>
		public bool CancellazioneDatiEstrazione(string anno, string ente)
		{
			SqlCommand selectCommand = new SqlCommand();
            try { 
			selectCommand.CommandText = "DELETE FROM TBLLOTTIPOSTEL WHERE ANNO='" + anno + "' AND IDENTE='" + ente + "'";

            return Execute(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.CancellazioneDatiEstrazione.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oggettoLottiIci"></param>
		/// <returns></returns>
		public bool InsertDatiEstrazione(oggettoTblLottiIci oggettoLottiIci)
		{
			SqlCommand selectCommand = new SqlCommand();
            try { 
			selectCommand.CommandText = "INSERT INTO TBLLOTTIPOSTEL ";
			selectCommand.CommandText += "(ANNO, NUMERO_LOTTO, IDENTE, DATA_CREAZIONE, TIPO_SERVIZIO, NOME_FILE, SCADENZA_ACCONTO, SCADENZA_SALDO, SCADENZA_UNICA_SOL)";
			selectCommand.CommandText += " VALUES (";
			selectCommand.CommandText += " '" + oggettoLottiIci.Anno + "'," + oggettoLottiIci.Numero_Lotto + ",'" + oggettoLottiIci.Id_Ente + "',";
			selectCommand.CommandText += " '" + oggettoLottiIci.Data_Creazione + "','" + oggettoLottiIci.Tipo_Servizio + "',";
			selectCommand.CommandText += " '" + oggettoLottiIci.Nome_File + "', ";
			
			if (oggettoLottiIci.Scadenza_Acconto == "")
				selectCommand.CommandText += DBNull.Value + ",";
			else
				selectCommand.CommandText += "'" + oggettoLottiIci.Scadenza_Acconto + "',";
			
			if (oggettoLottiIci.Scadenza_Acconto == "")
				selectCommand.CommandText += DBNull.Value + ",";
			else
				selectCommand.CommandText += "'" + oggettoLottiIci.Scadenza_Saldo + "',";
			
			if (oggettoLottiIci.Scadenza_Unica_Soluzione== "")
				selectCommand.CommandText += DBNull.Value;
			else
				selectCommand.CommandText += "'" + oggettoLottiIci.Scadenza_Unica_Soluzione + "'";
			selectCommand.CommandText += ")";

            return Execute(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.InsertDatiEstrazione.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objDatiPostel"></param>
		/// <returns></returns>
		public int SalvataggioCodeLine(OggettoDatiPostelIci[] objDatiPostel)
		{
			SqlCommand InsertCommand = new SqlCommand();
			int nSalvati=0;
			SqlConnection oConn = new SqlConnection() ;
			string connessione = ConfigurationManager.AppSettings["connectionStringOpenGovICI"];
			oConn.ConnectionString = connessione;
			oConn.Open();
			
			InsertCommand.Connection=oConn;
            try { 
			for(int i=0; i<objDatiPostel.Length;i++)
			{
				InsertCommand.CommandText = "UPDATE TP_CALCOLO_FINALE_ICI ";
				InsertCommand.CommandText += "SET CODELINE_ACCONTO = '" + objDatiPostel[i].CodeLineAcc + "',";
				InsertCommand.CommandText += " CODELINE_SALDO = '" + objDatiPostel[i].CodeLineSal + "',";
				InsertCommand.CommandText += " CODELINE_UNICA_SOLUZIONE = '" + objDatiPostel[i].CodeLineUnicaSol + "' ";
				InsertCommand.CommandText += " WHERE (COD_CONTRIBUENTE = " + objDatiPostel[i].IdContribuente + ") AND ";
				InsertCommand.CommandText += " (ANNO = '" + objDatiPostel[i].AnnoRiferimento + "') AND (COD_ENTE = '" + objDatiPostel[i].CodiceEnte + "')";

				InsertCommand.ExecuteNonQuery();
				nSalvati++;

			}
			oConn.Close();
			return nSalvati;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.SalvataggioCodeLine.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oOggettoDatiPostelIci"></param>
		/// <returns></returns>
		public bool UpdateDatiIci(OggettoDatiPostelIci oOggettoDatiPostelIci){
			SqlCommand selectCommand = new SqlCommand();
            try { 
				selectCommand.CommandText = "UPDATE TP_CALCOLO_FINALE_ICI ";
				selectCommand.CommandText += "SET CODELINE_ACCONTO = '" + oOggettoDatiPostelIci.CodeLineAcc + "',";
				selectCommand.CommandText += " CODELINE_SALDO = '" + oOggettoDatiPostelIci.CodeLineAcc + "',";
				selectCommand.CommandText += " CODELINE_UNICA_SOLUZIONE = '" + oOggettoDatiPostelIci.CodeLineUnicaSol + "' ";
				selectCommand.CommandText += " WHERE (COD_CONTRIBUENTE = " + oOggettoDatiPostelIci.IdContribuente + ") AND ";
				selectCommand.CommandText += " (ANNO = '" + oOggettoDatiPostelIci.AnnoRiferimento + "') AND (COD_ENTE = '" + oOggettoDatiPostelIci.CodiceEnte + "')";

                return Execute(selectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.TracciatoPostel.UpdateDatiIci.errore: ", Err);
                throw Err;
            }
        }
	}
}
