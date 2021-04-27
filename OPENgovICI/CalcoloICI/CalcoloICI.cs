using System;
using System.Data;
using System.Data.SqlClient;
using ComPlusInterface;
using System.Collections;
using System.Configuration;
using log4net;
using Utility;
using Business;

namespace DichiarazioniICI.CalcoloICI
{
    /// <summary>
    /// Classe per il controllo se il calcolo deve essere bloccato
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class CalcoloICI : DichiarazioniICI.Database.Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(CalcoloICI));
		public const int TIPOCalcolo_STANDARD = 0;
		public const int TIPOCalcolo_NETTOVERSATO = 1;

		public CalcoloICI()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// blocco calcolo se manca rendita e tipo utilizzo di RE
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Anno"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="isTASISuProp"></param>
        /// <param name="dtMyDati"></param>
        /// <revisionHistory>
        /// <revision date="07/2015">
        /// GESTIONE INCROCIATA RIFIUTI/ICI e DIVERSA GESTIONE QUOTA VARIABILE
        /// </revision>
        /// </revisionHistory>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public bool isBloccaCalcolo(string IdEnte, int Anno, int IdContribuente, int isTASISuProp, out DataTable dtMyDati)
        {
            bool bRet = true;
            dtMyDati = null;
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    ConstWrapper.nTry = 0;
                    ReDo:
                    try
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_CheckBloccoCalcolo", "IDENTE"
                            , "ANNO"
                            , "IDCONTRIBUENTE"
                            , "TASIAPROPRIETARIO"
                        );
                    dtMyDati = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("ANNO", Anno)
                            , ctx.GetParam("IDCONTRIBUENTE", IdContribuente)
                            , ctx.GetParam("TASIAPROPRIETARIO", isTASISuProp)
                        ).Tables[0];
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") && ConstWrapper.nTry <= 3)
                        {
                            ConstWrapper.nTry += 1;
                            goto ReDo;
                        }
                        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICI.isBloccaCalcolo.errore: ", ex);
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                    if (dtMyDati.Rows.Count > 0)
                {
                    bRet = true;
                }
                else
                    bRet = false;
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICI.isBloccaCalcolo.errore: ", ex);
            }
            return bRet;
        }
        //public bool isBloccaCalcolo(string IdEnte, int Anno, int IdContribuente,int isTASISuProp, out DataTable dtMyDati)
        //{
        //    bool bRet = true;
        //    dtMyDati = null;
        //    try
        //    {
        //        SqlCommand cmdMyCommand = new SqlCommand();
        //        cmdMyCommand.Connection = new SqlConnection(ConstWrapper.StringConnection);
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.CommandText = "prc_CheckBloccoCalcolo";
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = IdEnte;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.Int)).Value = Anno;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = IdContribuente;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@TASIAPROPRIETARIO", SqlDbType.Int)).Value = isTASISuProp;
        //        dtMyDati = Query(cmdMyCommand, new SqlConnection(ConstWrapper.StringConnection));
        //        if (dtMyDati.Rows.Count > 0)
        //        {
        //            bRet = true;
        //        }
        //        else
        //            bRet = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICI.isBloccaCalcolo.errore: ", ex);
        //    }
        //    return bRet;
        //}
        //*** ***

        /*public objSituazioneFinale[] CalcolaICI(Hashtable objHashTable , string strCOD_CONTRIBUENTE , int TipoCalcolo )
		{

			IFreezer remObjectFreezer =(IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI); 
			
			DataSet objDSDichiaratoIci = null; 
//			DataSet objDSImmobiliIci = null; 
//			DataSet objDSContitolariIci = null; 
			DataSet objDSSituazioneFinaleIci;
			DataSet objDSAnagrafica=null;
			string strANNODA=string.Empty;
			string strANNOA=string.Empty;
			//string strANNOdaLIQUIDARE=string.Empty;
			int strANNOdaLIQUIDARE;
			DataRow[] objDSDichiarazioniTotalePerAnno;
			int intIncrtement;
            objSituazioneFinale[] DSICI =null;

			objDSDichiaratoIci = remObjectFreezer.GetSituazioneVirtualeDichiarazioni( objHashTable, strCOD_CONTRIBUENTE); 
			//objDSImmobiliIci = remObjectFreezer.GetSituazioneVirtualeImmobili(objDSDichiaratoIci, objHashTable, strCOD_CONTRIBUENTE); 
			//objDSContitolariIci = remObjectFreezer.GetSituazioneVirtualeContitolari(objDSDichiaratoIci, objHashTable, strCOD_CONTRIBUENTE); 
			//objDSSituazioneFinaleIci = objDSImmobiliIci.Copy;

			objDSSituazioneFinaleIci = remObjectFreezer.GetSituazioneVirtualeImmobili(objDSDichiaratoIci, objHashTable, strCOD_CONTRIBUENTE); 

			strANNODA = objHashTable["ANNODA"].ToString(); 
			strANNOA = objHashTable["ANNOA"].ToString();
            try { 
			if (strANNOA != "-1" & strANNOA != strANNODA) 
			{ 
				intIncrtement = int.Parse(strANNOA) - int.Parse(strANNODA); 
			} 
			else 
			{ 
				intIncrtement = 1; 
			} 

			for (int i = 0; i<intIncrtement ;i++)
			{

				strANNOdaLIQUIDARE = int.Parse(strANNODA);// + intIncrtement;

				objDSDichiarazioniTotalePerAnno = objDSSituazioneFinaleIci.Tables[0].Select("ANNO='" + strANNOdaLIQUIDARE + "'"); 
				if (objDSDichiarazioniTotalePerAnno.Length > 0) 
				{ 
					DSICI=addRowsCalcoloICI(objDSDichiarazioniTotalePerAnno, objHashTable,TipoCalcolo ); 
				}

			}

			return DSICI;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICI.CalcolaICI.errore: ", ex);
                throw ex;
            }

        }


        private objSituazioneFinale[] addRowsCalcoloICI(DataRow[] objDSDichiarazioniTotalePerAnno, Hashtable objHashTable, int TipoCalcolo)
        {
            try
            {
                objSituazioneFinale row;
                //DataTable newTable;
                //DataSet objDSImmobili; 
                ArrayList objDSImmobiliAppoggio=new ArrayList();

                IFreezer remObjectFreezer = (IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI);

                //newTable = objDSImmobiliAppoggio.Tables[0].Copy();
                //int i = 0; 
                for (int iCount = 0; iCount <= objDSDichiarazioniTotalePerAnno.Length - 1; iCount++)
                {
                    row = new objSituazioneFinale();

                    row.Id= iCount + 1;
                    row.IdContribuente= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["COD_CONTRIBUENTE"].ToString());
                    row.Anno= objDSDichiarazioniTotalePerAnno[iCount]["ANNO"].ToString();
                    row.AccMesi= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["NUMERO_MESI_ACCONTO"].ToString());
                    row.Mesi= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["NUMERO_MESI_TOTALI"].ToString());
                    row.NUtilizzatori= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["NUMERO_UTILIZZATORI"].ToString());

                    if (objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString().Length == 0 && bool.Parse(objDSDichiarazioniTotalePerAnno[iCount]["FLAG_PRINCIPALE"].ToString()) == true)
                    {
                        row.FlagPrincipale= 1;
                    }
                    else if (objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString().Length == 0 && bool.Parse(objDSDichiarazioniTotalePerAnno[iCount]["FLAG_PRINCIPALE"].ToString()) == false)
                    {
                        row.FlagPrincipale= 0;
                    }
                    else if (objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString().Length > 0)
                    {
                        row.FlagPrincipale= 2;
                    }

                    row.PercPossesso= double.Parse(objDSDichiarazioniTotalePerAnno[iCount]["PERC_POSSESSO"].ToString());
                    row.Valore= double.Parse(objDSDichiarazioniTotalePerAnno[iCount]["VALORE"].ToString());
                    row.FlagRiduzione= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["RIDUZIONE"].ToString());
                    row.IdEnte= objDSDichiarazioniTotalePerAnno[iCount]["ENTE"].ToString();
                    row.IdProcedimento= 0;
                    row.IdRiferimento= 0;
                    row.Provenienza= "";
                    row.Caratteristica= objDSDichiarazioniTotalePerAnno[iCount]["CARATTERISTICA"].ToString();
                    row.Via = objDSDichiarazioniTotalePerAnno[iCount]["VIA"].ToString();
                    row.NCivico = objDSDichiarazioniTotalePerAnno[iCount]["NUMEROCIVICO"].ToString();
                    row.Sezione= objDSDichiarazioniTotalePerAnno[iCount]["SEZIONE"].ToString();
                    row.Foglio= objDSDichiarazioniTotalePerAnno[iCount]["FOGLIO"].ToString();
                    row.Numero= objDSDichiarazioniTotalePerAnno[iCount]["NUMERO"].ToString();
                    row.Subalterno= objDSDichiarazioniTotalePerAnno[iCount]["SUBALTERNO"].ToString();
                    row.Categoria= objDSDichiarazioniTotalePerAnno[iCount]["CODCATEGORIACATASTALE"].ToString();
                    row.Classe= objDSDichiarazioniTotalePerAnno[iCount]["CODCLASSE"].ToString();
                    row.Protocollo= "0";
                    row.FlagStorico= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["STORICO"].ToString());
                    row.FlagProvvisorio= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["FLAGVALOREPROVV"].ToString());
                    row.MesiPossesso= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["MESIPOSSESSO"].ToString());
                    row.MesiEsenzione= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["MESIESCLUSIONEESENZIONE"].ToString());
                    row.MesiRiduzione= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["MESIRIDUZIONE"].ToString());
                    row.ImpDetrazione=double.Parse( objDSDichiarazioniTotalePerAnno[iCount]["IMPDETRAZABITAZPRINCIPALE"].ToString());
                    row.FlagPosseduto= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["POSSESSO"].ToString());
                    row.FlagEsente= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["ESENTE_ESCLUSO"].ToString());
                    row.FlagRiduzione= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["RIDUZIONE"].ToString());
                    row.IdImmobile= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["ID"].ToString());
                    row.IdImmobilePertinenza= int.Parse(objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString());
                    row.Dal= DateTime.MaxValue;
                    row.Al= DateTime.MaxValue;
                    row.TipoRendita= objDSDichiarazioniTotalePerAnno[iCount]["TIPO_RENDITA"].ToString();
                    row.MeseInizio= 0;
                    row.DataScadenza= "";

                    objDSImmobiliAppoggio.Add(row);
                }

                objSituazioneFinale[] objICICalcolato;
                //objICICalcolato = remObjectFreezer.CalcoloICI((objSituazioneFinale[])objDSImmobiliAppoggio.ToArray(typeof(objSituazioneFinale)), objHashTable, TipoCalcolo);// objCOMCalcoloICI.getCALCOLO_ICI(); 
                objICICalcolato = (objSituazioneFinale[])objDSImmobiliAppoggio.ToArray(typeof(objSituazioneFinale));
                remObjectFreezer.CalcoloICI(ref objICICalcolato, objHashTable, TipoCalcolo);// objCOMCalcoloICI.getCALCOLO_ICI(); 

                return objICICalcolato;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICI.addRowsCalcoloICI.errore: ", ex);
                throw ex;
            }
        }*/
        /*private DataSet addRowsCalcoloICI(DataRow[] objDSDichiarazioniTotalePerAnno, Hashtable objHashTable, int TipoCalcolo ) 
		{ 
			try 
			{ 
				DataRow row; 
				DataTable newTable; 
				//DataSet objDSImmobili; 
				DataSet objDSImmobiliAppoggio; 
				objDSImmobiliAppoggio = this.CreateDSperCalcoloICI(); 
				//DataSet objICI;

				IFreezer remObjectFreezer =(IFreezer)Activator.GetObject(typeof(IFreezer), ConstWrapper.UrlServizioCalcoloICI); 

				newTable = objDSImmobiliAppoggio.Tables[0].Copy(); 
				//int i = 0; 
				for (int iCount = 0; iCount <= objDSDichiarazioniTotalePerAnno.Length - 1; iCount++) 
				{ 
					row = newTable.NewRow(); 
					
					row["ID_SITUAZIONE_FINALE"]=iCount+1;
					row["COD_CONTRIBUENTE"] = objDSDichiarazioniTotalePerAnno[iCount]["COD_CONTRIBUENTE"]; 
					row["ANNO"] = objDSDichiarazioniTotalePerAnno[iCount]["ANNO"]; 
					row["NUMERO_MESI_ACCONTO"] = objDSDichiarazioniTotalePerAnno[iCount]["NUMERO_MESI_ACCONTO"]; 
					row["NUMERO_MESI_TOTALI"] = objDSDichiarazioniTotalePerAnno[iCount]["NUMERO_MESI_TOTALI"]; 
					row["NUMERO_UTILIZZATORI"] = objDSDichiarazioniTotalePerAnno[iCount]["NUMERO_UTILIZZATORI"]; 

					if (objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString().Length==0 && bool.Parse (objDSDichiarazioniTotalePerAnno[iCount]["FLAG_PRINCIPALE"].ToString())==true )
					{
						row["FLAG_PRINCIPALE"] = 1;
					}
					else if (objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString().Length==0 && bool.Parse (objDSDichiarazioniTotalePerAnno[iCount]["FLAG_PRINCIPALE"].ToString())==false)
					{
						row["FLAG_PRINCIPALE"] = 0;
					}
					else if (objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"].ToString().Length > 0)
					{
						row["FLAG_PRINCIPALE"] = 2;
					}

					row["PERC_POSSESSO"] = objDSDichiarazioniTotalePerAnno[iCount]["PERC_POSSESSO"]; 
					row["VALORE"] = objDSDichiarazioniTotalePerAnno[iCount]["VALORE"]; 
					row["RIDUZIONE"] = objDSDichiarazioniTotalePerAnno[iCount]["RIDUZIONE"]; 
					row["COD_ENTE"] = objDSDichiarazioniTotalePerAnno[iCount]["ENTE"]; 
					row["ID_PROCEDIMENTO"] = 0; 
					row["ID_RIFERIMENTO"] = 0; 
					row["PROVENIENZA"] = ""; 
					row["CARATTERISTICA"] = objDSDichiarazioniTotalePerAnno[iCount]["CARATTERISTICA"]; 
					row["INDIRIZZO"] = objDSDichiarazioniTotalePerAnno[iCount]["VIA"] + " " + objDSDichiarazioniTotalePerAnno[iCount]["NUMEROCIVICO"]; 
					row["SEZIONE"] = objDSDichiarazioniTotalePerAnno[iCount]["SEZIONE"]; 
					row["FOGLIO"] = objDSDichiarazioniTotalePerAnno[iCount]["FOGLIO"]; 
					row["NUMERO"] = objDSDichiarazioniTotalePerAnno[iCount]["NUMERO"]; 
					row["SUBALTERNO"] = objDSDichiarazioniTotalePerAnno[iCount]["SUBALTERNO"]; 
					row["CATEGORIA"] = objDSDichiarazioniTotalePerAnno[iCount]["CODCATEGORIACATASTALE"]; 
					row["CLASSE"] = objDSDichiarazioniTotalePerAnno[iCount]["CODCLASSE"]; 
					row["PROTOCOLLO"] = 0; 
					row["FLAG_STORICO"] = objDSDichiarazioniTotalePerAnno[iCount]["STORICO"]; 
					row["FLAG_PROVVISORIO"] = objDSDichiarazioniTotalePerAnno[iCount]["FLAGVALOREPROVV"]; 
					row["MESI_POSSESSO"] = objDSDichiarazioniTotalePerAnno[iCount]["MESIPOSSESSO"]; 
					row["MESI_ESCL_ESENZIONE"] = objDSDichiarazioniTotalePerAnno[iCount]["MESIESCLUSIONEESENZIONE"]; 
					row["MESI_RIDUZIONE"] = objDSDichiarazioniTotalePerAnno[iCount]["MESIRIDUZIONE"]; 
					row["IMPORTO_DETRAZIONE"] = objDSDichiarazioniTotalePerAnno[iCount]["IMPDETRAZABITAZPRINCIPALE"]; 
					row["FLAG_POSSEDUTO"] = objDSDichiarazioniTotalePerAnno[iCount]["POSSESSO"]; 
					row["FLAG_ESENTE"] = objDSDichiarazioniTotalePerAnno[iCount]["ESENTE_ESCLUSO"]; 
					row["FLAG_RIDUZIONE"] = objDSDichiarazioniTotalePerAnno[iCount]["RIDUZIONE"]; 
					row["COD_IMMOBILE"] = objDSDichiarazioniTotalePerAnno[iCount]["ID"]; 

					//PERTINENZE----------------------------------------------------------------------
//					if (row["FLAG_PRINCIPALE"].ToString()=="1")
//					{
//						row["COD_IMMOBILE_PERTINENZA"] = objDSDichiarazioniTotalePerAnno[iCount]["ID"]; 
//					}
//					else
//					{
						row["COD_IMMOBILE_PERTINENZA"] = objDSDichiarazioniTotalePerAnno[iCount]["IDIMMOBILEPERTINENTE"]; 
//					}
					//PERTINENZE----------------------------------------------------------------------

					row["DAL"] = ""; 
					row["AL"] = ""; 
					row["TIPO_RENDITA"] = objDSDichiarazioniTotalePerAnno[iCount]["TIPO_RENDITA"];
					row["MESE_INIZIO"] = 0; 
					row["DATA_SCADENZA"] = ""; 
					
					newTable.Rows.Add(row); 
					newTable.AcceptChanges(); 
					objDSImmobiliAppoggio.Tables[0].ImportRow(row); 
					objDSImmobiliAppoggio.Tables[0].AcceptChanges(); 
				} 

				DataSet objICICalcolato; 
				objICICalcolato =remObjectFreezer.CalcoloICI(objDSImmobiliAppoggio, objHashTable,TipoCalcolo );// objCOMCalcoloICI.getCALCOLO_ICI(); 

				return objICICalcolato;
			} 
			catch (Exception ex) 
			{
                log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.CalcoloICI.addRowsCalcoloICI.errore: ", ex);
                throw ex;
            } 
		}
		public DataSet CreateDSperCalcoloICI() 
		{ 
			DataSet objDS = new DataSet(); 
			DataTable newTable; 
			newTable = new DataTable("TP_SITUAZIONE_FINALE_ICI"); 
			DataColumn NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ID_SITUAZIONE_FINALE"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ANNO"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn);
 
			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "COD_ENTE"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ID_PROCEDIMENTO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ID_RIFERIMENTO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "PROVENIENZA"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "CARATTERISTICA"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "INDIRIZZO"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "SEZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FOGLIO"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "NUMERO"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "SUBALTERNO"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "CATEGORIA"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "CLASSE"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "PROTOCOLLO"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn);
 
			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FLAG_STORICO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "VALORE"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FLAG_PROVVISORIO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "PERC_POSSESSO"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "MESI_POSSESSO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "MESI_ESCL_ESENZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "MESI_RIDUZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "IMPORTO_DETRAZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FLAG_POSSEDUTO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn);
 
			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FLAG_ESENTE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FLAG_RIDUZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "FLAG_PRINCIPALE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "COD_CONTRIBUENTE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "COD_IMMOBILE_PERTINENZA"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "COD_IMMOBILE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "DAL"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "AL"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "NUMERO_MESI_ACCONTO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "NUMERO_MESI_TOTALI"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "NUMERO_UTILIZZATORI"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "TIPO_RENDITA"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_ACCONTO_SENZA_DETRAZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_APPLICATA"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_DOVUTA_ACCONTO"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_RESIDUA"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_TOTALE_SENZA_DETRAZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_APPLICATA"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_TOTALE_DOVUTA"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_RESIDUA"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_DOVUTA_SALDO"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_SALDO"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_DOVUTA_SENZA_DETRAZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_RESIDUA"; 
			NewColumn.DataType = System.Type.GetType("System.Double"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "RIDUZIONE"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "MESE_INIZIO"; 
			NewColumn.DataType = System.Type.GetType("System.Int64"); 
			NewColumn.DefaultValue = "0"; 
			newTable.Columns.Add(NewColumn); 

			NewColumn = new DataColumn(); 
			NewColumn.ColumnName = "DATA_SCADENZA"; 
			NewColumn.DataType = System.Type.GetType("System.String"); 
			NewColumn.DefaultValue = ""; 
			newTable.Columns.Add(NewColumn); 
			objDS.Tables.Add(newTable); 

            //--------------------------------------------------------------------------------------

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_CALCOLATA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_APPLICATA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_RESIDUA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_CALCOLATA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_APPLICATA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_RESIDUA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_CALCOLATA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_APPLICATA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            NewColumn = new DataColumn(); 
            NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_RESIDUA";
            NewColumn.DataType = System.Type.GetType("System.Double");
            NewColumn.DefaultValue = "0";
            newTable.Columns.Add(NewColumn);

            //--------------------------------------------------------------------------------------

			return objDS; 
		}*/

    }
}
