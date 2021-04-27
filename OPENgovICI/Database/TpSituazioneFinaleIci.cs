using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Business;
using System.Configuration;
using log4net;
using Utility;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione della tabella TpSituazioneFinaleIci.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class TpSituazioneFinaleIci : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TpSituazioneFinaleIci));
        /// <summary>
        /// 
        /// </summary>
        public enum TipoOrdinamento
        {
            /// 
            CognomeNome = 0,
            /// 
            Indirizzo = 1
        }
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public TpSituazioneFinaleIci()
        {
            //
            // TODO: Add constructor logic here
            //

            this.TableName = "TP_SITUAZIONE_FINALE_ICI";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNomeTabella"></param>
        /// <returns></returns>
        public long getNewID(string strNomeTabella)
        {
            try
            {


                string sSQL;
                long lngMaxId = 0;

                bool blnVal = true;
                SqlCommand selCommand = new SqlCommand();
                sSQL = "SELECT MAXID FROM CONTATORI WHERE NOME_TABELLA =" + this.CStrToDB(strNomeTabella, ref blnVal, false);
                selCommand.CommandText = sSQL;
                DataTable dt = Query(selCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

                if (dt.Rows.Count == 1)
                {
                    lngMaxId = long.Parse(dt.Rows[0]["MAXID"].ToString());
                    lngMaxId = lngMaxId + 1;
                }
                dt.Dispose();

                sSQL = "UPDATE CONTATORI SET MAXID=" + lngMaxId + " WHERE NOME_TABELLA ='" + strNomeTabella + "'";
                SqlCommand UpdateCommand = new SqlCommand();
                UpdateCommand.CommandText = sSQL;
                Execute(UpdateCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

                return lngMaxId;
            }
            catch (Exception ex)
            {
                kill();
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.getNewID.errore: ", ex);
                throw ex;
            }
            finally
            {
                kill();
            }
        }

        //*** 20140509 - TASI ***
        /// <summary>
        /// Inserisce un nuovo record a partire dai singoli campi.
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="CodEnte"></param>
        /// <param name="Tributo"></param>
        /// <param name="idProgrElab"></param>
        /// <returns></returns>
        public DataTable GetImmoCalcoloICItotale(string CodContribuente, string Anno, string CodEnte, string Tributo, long idProgrElab)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetImmoCalcoloICITotale";
                SelectCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
                if (CodContribuente.CompareTo("") != 0)
                {
                    SelectCommand.Parameters.Add("@codcontribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
                }
                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@anno", SqlDbType.NVarChar).Value = Anno;
                }
                if (CodEnte.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@codente", SqlDbType.NVarChar).Value = CodEnte;
                }
                if (idProgrElab != -1)
                {
                    SelectCommand.Parameters.Add("@idProgrElab", SqlDbType.BigInt).Value = idProgrElab;
                }

                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetImmoCalcoloICItotale.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        //*** ***

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="CodEnte"></param>
        /// <param name="Tributo"></param>
        /// <param name="idProgrElab"></param>
        /// <param name="nettoVersato"></param>
        /// <returns></returns>
        //*** 20140509 - TASI ***
        /**** 20120828 - IMU adeguamento per importi statali ****/
        public DataTable GetCalcoloICItotale(string CodContribuente, string Anno, string CodEnte, string Tributo, long idProgrElab, bool nettoVersato)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            DataTable Tabella;
            string sProcedureName = "prc_GETCALCOLOICITOTALE";

            try
            {
                if ((Anno != string.Empty) && (int.Parse(Anno) < 2012))
                {
                    sProcedureName = "prc_GETCALCOLOICITOTALE";
                }
                else
                {
                    if (nettoVersato)
                        sProcedureName = "prc_GETCALCOLOIMUNETTOVERSATO";
                    else
                        sProcedureName = "prc_GETCALCOLOIMUTOTALE";
                }
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = sProcedureName;
                cmdMyCommand.Parameters.Add("@codente", SqlDbType.NVarChar).Value = CodEnte;
                cmdMyCommand.Parameters.Add("@codcontribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
                cmdMyCommand.Parameters.Add("@anno", SqlDbType.Int).Value = int.Parse(Anno);
                cmdMyCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
                cmdMyCommand.Parameters.Add("@idProgrElab", SqlDbType.Int).Value = idProgrElab;

                log.Debug("GetCalcoloICItotale::query::" + cmdMyCommand.CommandText + "::parametri::@codente" + CodEnte + "::Anno::" + Anno + "::Tributo::" + Tributo + "::CodContribuente::" + CodContribuente.ToString() + "::idProgrElab::" + idProgrElab.ToString());
                Tabella = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICItotale.errore: ", Err);

                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }

        //        public DataTable GetCalcoloICItotale(string CodContribuente, string Anno, string CodEnte, long idProgrElab, bool nettoVersato)
        //        {
        //            DataTable Tabella;

        //            try
        //            {
        //                SqlCommand SelectCommand = new SqlCommand();
        //                //*** 20121010 - IMU l'aliquota contiene somma di comunale+statale quindi in visualizzazione aree fab/terreni/altri fab devo fare la sottrazione ***
        //                //SelectCommand.CommandText = "SELECT ANNO, SUM(CASE WHEN (FLAG_PRINCIPALE = 1 OR COD_IMMOBILE_PERTINENZA>0) THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS IMP_ABI_PRINC";
        ////				SelectCommand.CommandText += ", SUM(CASE WHEN (TIPO_RENDITA <> 'AF') AND (TIPO_RENDITA <> 'TA') AND (FLAG_PRINCIPALE<>1 AND COD_IMMOBILE_PERTINENZA=-1) THEN ICI_TOTALE_DOVUTA-ICI_TOTALE_DOVUTA_STATALE ELSE 0 END) AS IMP_ALTRI_FAB";
        ////				SelectCommand.CommandText += ", SUM(CASE WHEN (TIPO_RENDITA = 'AF')THEN ICI_TOTALE_DOVUTA-ICI_TOTALE_DOVUTA_STATALE ELSE 0 END) AS IMP_AREE_FAB ";
        ////				SelectCommand.CommandText += ", SUM(CASE WHEN (TIPO_RENDITA = 'TA') THEN ICI_TOTALE_DOVUTA-ICI_TOTALE_DOVUTA_STATALE ELSE 0 END) AS IMP_TERRENI ";
        ////				SelectCommand.CommandText += ", SUM(CASE WHEN (TIPO_RENDITA <> 'AF') AND (TIPO_RENDITA <> 'TA') AND (FLAG_PRINCIPALE<>1 AND COD_IMMOBILE_PERTINENZA=-1) THEN ICI_TOTALE_DOVUTA_STATALE ELSE 0 END) AS IMP_ALTRI_FAB_STATO";
        ////				SelectCommand.CommandText += ", SUM(CASE WHEN (TIPO_RENDITA = 'AF')THEN ICI_TOTALE_DOVUTA_STATALE ELSE 0 END) AS IMP_AREE_FAB_STATO ";
        ////				SelectCommand.CommandText += ", SUM(CASE WHEN (TIPO_RENDITA = 'TA') THEN ICI_TOTALE_DOVUTA_STATALE ELSE 0 END) AS IMP_TERRENI_STATO ";
        ////				SelectCommand.CommandText += ", 0 AS IMP_FABRURUSOSTRUM";
        ////				SelectCommand.CommandText += ", SUM(ICI_TOTALE_DETRAZIONE_APPLICATA) AS DETRAZIONE ";
        ////				SelectCommand.CommandText += ", SUM(ICI_TOTALE_DETRAZIONE_STATALE_APPLICATA) AS DSA ";
        ////				SelectCommand.CommandText += ", SUM(ICI_TOTALE_DOVUTA) AS TOTALE" ;
        ////				SelectCommand.CommandText += ", COUNT(*) AS NUMFABB " ;
        ////				SelectCommand.CommandText += " FROM TP_SITUAZIONE_FINALE_ICI";

        //                SelectCommand.CommandText = "SELECT ANNO,COD_BELFIORE";
        //                SelectCommand.CommandText += " ,SUM(IMP_ABI_PRINC) AS IMP_ABI_PRINC";
        //                SelectCommand.CommandText += " ,SUM(IMP_ALTRI_FAB) AS IMP_ALTRI_FAB";
        //                SelectCommand.CommandText += " ,SUM(IMP_AREE_FAB) AS IMP_AREE_FAB";
        //                SelectCommand.CommandText += " ,SUM(IMP_TERRENI) AS IMP_TERRENI";
        //                SelectCommand.CommandText += " ,SUM(IMP_ALTRI_FAB_STATO) AS IMP_ALTRI_FAB_STATO";
        //                SelectCommand.CommandText += " ,SUM(IMP_AREE_FAB_STATO) AS IMP_AREE_FAB_STATO";
        //                SelectCommand.CommandText += " ,SUM(IMP_TERRENI_STATO) AS IMP_TERRENI_STATO";
        //                SelectCommand.CommandText += " ,SUM(IMP_FABRURUSOSTRUM) AS IMP_FABRURUSOSTRUM";
        //                //*** 20130422 - aggiornamento IMU ***
        //                SelectCommand.CommandText += " ,SUM(IMP_FABRURUSOSTRUM_STATO) AS IMP_FABRURUSOSTRUM_STATO";
        //                SelectCommand.CommandText += " ,SUM(IMP_USOPRODCATD) AS IMP_USOPRODCATD";
        //                SelectCommand.CommandText += " ,SUM(IMP_USOPRODCATD_STATO) AS IMP_USOPRODCATD_STATO";
        //                //*** ***
        //                SelectCommand.CommandText += " ,SUM(DETRAZIONE) AS DETRAZIONE";
        //                SelectCommand.CommandText += " ,SUM(DSA) AS DSA";
        //                SelectCommand.CommandText += " ,SUM(TOTALE) AS TOTALE";
        //                SelectCommand.CommandText += " ,SUM(NUMFABB) AS NUMFABB";
        //                if ((Anno != string.Empty) && (int.Parse(Anno) < 2012))
        //                {
        //                    SelectCommand.CommandText += " FROM V_GETCALCOLOICITOTALE ";
        //                }
        //                else
        //                {
        //                    if(nettoVersato)
        //                        SelectCommand.CommandText += " FROM V_GETCALCOLOIMUNETTOVERSATO ";
        //                    else 
        //                        SelectCommand.CommandText += " FROM V_GETCALCOLOIMUTOTALE ";
        //                }
        //                SelectCommand.CommandText += " WHERE 1=1";
        //                if (CodContribuente.CompareTo("")!=0)
        //                {
        //                    SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
        //                    SelectCommand.Parameters.Add("@codcontribuente",SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
        //                }
        //                if (Anno.ToString().CompareTo("-1")!=0)
        //                {
        //                    SelectCommand.CommandText += " AND ANNO=@anno";
        //                    SelectCommand.Parameters.Add("@anno",SqlDbType.NVarChar).Value = Anno;
        //                }
        //                if (CodEnte.ToString().CompareTo("-1")!=0)
        //                {
        //                    SelectCommand.CommandText += " AND COD_ENTE=@codente";
        //                    SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = CodEnte;
        //                }
        //                if (idProgrElab!=-1)
        //                {
        //                    SelectCommand.CommandText += " AND PROGRESSIVO_ELABORAZIONE=@idProgrElab";
        //                    SelectCommand.Parameters.Add("@idProgrElab",SqlDbType.BigInt).Value = idProgrElab;
        //                }
        //                SelectCommand.CommandText += " GROUP BY ANNO,COD_BELFIORE";

        //                log.Debug("GetCalcoloICItotale::query::"+ SelectCommand.CommandText +"::parametri::@codente"+CodEnte+"::Anno::"+Anno+"::CodContribuente::"+CodContribuente.ToString()+"::idProgrElab::"+idProgrElab.ToString());
        //                Tabella = Query(SelectCommand);
        //            }
        //            catch(Exception Err)
        //            { log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICItotale.errore: ", Err);

        //                kill();
        //                Tabella = new DataTable();
        //            }
        //            finally
        //            {
        //                kill();
        //            }

        //            return Tabella;
        //        }

        //		public DataTable GetCalcoloICItotale(string CodContribuente, string Anno, string CodEnte, long idProgrElab)
        //		{
        //			DataTable Tabella;
        ////			DataSet dsRet=new DataSet();
        ////			dsRet=CreateDSperRiepilogoICI();
        ////			dsRet.Tables["TP_SITUAZIONE_FINALE_ICI"].Rows[0]["ANNO"]=Anno;
        //
        //			try
        //			{
        //
        //				SqlCommand SelectCommand = new SqlCommand();
        //				/*	SelectCommand.CommandText = " select ANNO, sum(case when (FLAG_PRINCIPALE = 1) then ICI_TOTALE_DOVUTA else 0 end) as ABITPRINC, ";
        //					SelectCommand.CommandText += " sum(case when (TIPO_RENDITA <> N'AF') AND (TIPO_RENDITA <> N'TA') then ICI_TOTALE_DOVUTA else 0 end) as ALTRIFABB, ";
        //					SelectCommand.CommandText += " sum(case when (TIPO_RENDITA = N'AF')then ICI_TOTALE_DOVUTA else 0 end) as AREEDIF, ";
        //					SelectCommand.CommandText += " sum(case when (TIPO_RENDITA <> N'TA') OR (TIPO_RENDITA <> N'TA') then ICI_TOTALE_DOVUTA else 0 end) as TERRAGR, ";
        //					SelectCommand.CommandText += " sum(ICI_TOTALE_DETRAZIONE_APPLICATA) AS DETRAZ, sum(ICI_TOTALE_SENZA_DETRAZIONE) as IMPNETTO" ;
        //				*/
        //				SelectCommand.CommandText = "SELECT ANNO, SUM(CASE WHEN (FLAG_PRINCIPALE = 1) THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS IMP_ABI_PRINC, ";
        //				SelectCommand.CommandText += " SUM(CASE WHEN (TIPO_RENDITA <> N'AF') AND (TIPO_RENDITA <> N'TA') AND (FLAG_PRINCIPALE<>1) THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS IMP_ALTRI_FAB, ";
        //				SelectCommand.CommandText += " SUM(CASE WHEN (TIPO_RENDITA = N'AF')THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS IMP_AREE_FAB, ";
        //				SelectCommand.CommandText += " SUM(CASE WHEN (TIPO_RENDITA = N'TA') THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS IMP_TERRENI, ";
        //				SelectCommand.CommandText += " SUM(ICI_TOTALE_DETRAZIONE_APPLICATA) AS DETRAZIONE, ";
        //				SelectCommand.CommandText += " SUM(ICI_TOTALE_DETRAZIONE_STATALE_APPLICATA) AS DSA, ";
        //				SelectCommand.CommandText += " SUM(ICI_TOTALE_DOVUTA) AS TOTALE" ;
        //				SelectCommand.CommandText += " ,COUNT(*) AS NUMFABB " ;
        //				SelectCommand.CommandText += " FROM TP_SITUAZIONE_FINALE_ICI";
        //				SelectCommand.CommandText += " WHERE 1=1";
        //
        //				if (CodContribuente.CompareTo("")!=0)
        //				{
        //					SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
        //					SelectCommand.Parameters.Add("@codcontribuente",SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
        //				}
        //			
        //				if (Anno.ToString().CompareTo("-1")!=0)
        //				{
        //					SelectCommand.CommandText += " AND ANNO=@anno";
        //					SelectCommand.Parameters.Add("@anno",SqlDbType.NVarChar).Value = Anno;
        //				}
        //
        //				if (CodEnte.ToString().CompareTo("-1")!=0)
        //				{
        //					SelectCommand.CommandText += " AND COD_ENTE=@codente";
        //					SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = CodEnte;
        //				}
        //
        //				if (idProgrElab!=-1)
        //				{
        //					SelectCommand.CommandText += " AND PROGRESSIVO_ELABORAZIONE=@idProgrElab";
        //					SelectCommand.Parameters.Add("@idProgrElab",SqlDbType.BigInt).Value = idProgrElab;
        //				}
        //
        //				SelectCommand.CommandText += " GROUP BY ANNO";
        //
        //				Tabella = Query(SelectCommand);
        //
        //				//
        //				//
        //				//ABITAZIONE PRINCIPALE
        //				//				SqlCommand SelectCommand = new SqlCommand();
        //				//				SelectCommand.CommandText =" SELECT SUM(ICI_TOTALE_DOVUTA) AS IMPORTO FROM " + this.TableName + " where 1=1";
        //				//				SelectCommand.CommandText += " GROUP BY FLAG_PRINCIPALE HAVING (FLAG_PRINCIPALE = 1)";
        //				//
        //				//				if (CodContribuente.CompareTo(-1)!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
        //				//					SelectCommand.Parameters.Add("@codcontribuente",SqlDbType.Int).Value = CodContribuente;
        //				//				}
        //				//			
        //				//				if (Anno.ToString().CompareTo("-1")!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND ANNO=@anno";
        //				//					SelectCommand.Parameters.Add("@anno",SqlDbType.NVarChar).Value = Anno;
        //				//				}
        //				//
        //				//				if (CodEnte.ToString().CompareTo("-1")!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND COD_ENTE=@codente";
        //				//					SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = CodEnte;
        //				//				}
        //				//
        //				//				Tabella = Query(SelectCommand);
        //				//				dsRet.Tables["TP_SITUAZIONE_FINALE_ICI"].Rows[0]["ABITAZIONE_PRINCIPALE"]=Tabella.Rows[0]["IMPORTO"].ToString();
        //				//
        //				//
        //				//ALTRI FABBRICATI
        //				//				
        //				//				SelectCommand.CommandText =" SELECT SUM(ICI_TOTALE_DOVUTA) AS IMPORTO FROM " + this.TableName + " where 1=1";
        //				//				SelectCommand.CommandText += " GROUP BY TIPO_RENDITA HAVING (TIPO_RENDITA <> 'AF') AND (TIPO_RENDITA <> 'TA')";
        //				//
        //				//				if (CodContribuente.CompareTo(-1)!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
        //				//					SelectCommand.Parameters.Add("@codcontribuente",SqlDbType.Int).Value = CodContribuente;
        //				//				}
        //				//			
        //				//				if (Anno.ToString().CompareTo("-1")!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND ANNO=@anno";
        //				//					SelectCommand.Parameters.Add("@anno",SqlDbType.NVarChar).Value = Anno;
        //				//				}
        //				//
        //				//				if (CodEnte.ToString().CompareTo("-1")!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND COD_ENTE=@codente";
        //				//					SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = CodEnte;
        //				//				}
        //				//
        //				//				Tabella = Query(SelectCommand);
        //				//				dsRet.Tables["TP_SITUAZIONE_FINALE_ICI"].Rows[0]["ALTRI_FABBRICATI"]=Tabella.Rows[0]["IMPORTO"].ToString();
        //				//
        //				//
        //				//AREE EDIFICABILI
        //				//				
        //				//				SelectCommand.CommandText =" SELECT SUM(ICI_TOTALE_DOVUTA) AS IMPORTO FROM " + this.TableName + " where 1=1";
        //				//				SelectCommand.CommandText += " GROUP BY FLAG_PRINCIPALE HAVING (FLAG_PRINCIPALE = 1)";
        //				//
        //				//				if (CodContribuente.CompareTo(-1)!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
        //				//					SelectCommand.Parameters.Add("@codcontribuente",SqlDbType.Int).Value = CodContribuente;
        //				//				}
        //				//			
        //				//				if (Anno.ToString().CompareTo("-1")!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND ANNO=@anno";
        //				//					SelectCommand.Parameters.Add("@anno",SqlDbType.NVarChar).Value = Anno;
        //				//				}
        //				//
        //				//				if (CodEnte.ToString().CompareTo("-1")!=0)
        //				//				{
        //				//					SelectCommand.CommandText += " AND COD_ENTE=@codente";
        //				//					SelectCommand.Parameters.Add("@codente",SqlDbType.NVarChar).Value = CodEnte;
        //				//				}
        //				//
        //				//				Tabella = Query(SelectCommand);
        //				//				dsRet.Tables["TP_SITUAZIONE_FINALE_ICI"].Rows[0]["ABITAZIONE_PRINCIPALE"]=Tabella.Rows[0]["IMPORTO"].ToString();
        //
        //			}
        //			catch(Exception Err)
        //			{log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICItotale.errore: ", Err);
        //				kill();
        //				Tabella = new DataTable();
        //			}
        //			finally{
        //				kill();
        //			}
        //
        //			return Tabella;
        //		}
        //
        /**** ****/
        //*** ***

        //*** 20150430 - TASI Inquilino ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodEnte"></param>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="Tributo"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataTable GetCalcoloICISoggetti(string CodEnte, string CodContribuente, string Anno, string Tributo)
        {
            DataTable Tabella;
            DataSet myData = new DataSet();

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    ConstWrapper.nTry = 0;
                    ReDo:
                    try
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GETCALCOLOCONTRIB", "codente"
                            , "codcontribuente"
                            , "anno"
                            , "Tributo"
                        );
                        myData = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("codente", CodEnte)
                                , ctx.GetParam("codcontribuente", CodContribuente)
                                , ctx.GetParam("anno", Anno)
                                , ctx.GetParam("Tributo", Tributo)
                            );
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") && ConstWrapper.nTry <= 3)
                        {
                            ConstWrapper.nTry += 1;
                            goto ReDo;
                        }
                        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICISoggett.errore: ", ex);
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                    Tabella = myData.Tables[0];
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICISoggetti.errore: ", Err);
                Tabella = new DataTable();
            }
            return Tabella;
        }
        //public DataTable GetCalcoloICISoggetti(string CodEnte, string CodContribuente, string Anno, string Tributo)
        //{
        //    SqlCommand cmdMyCommand = new SqlCommand();
        //    DataTable Tabella;
        //    string sProcedureName = "prc_GETCALCOLOCONTRIB";

        //    try
        //    {
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.CommandText = sProcedureName;
        //        cmdMyCommand.Parameters.Add("@codente", SqlDbType.NVarChar).Value = CodEnte;
        //        cmdMyCommand.Parameters.Add("@codcontribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
        //        cmdMyCommand.Parameters.Add("@anno", SqlDbType.Int).Value = int.Parse(Anno);
        //        cmdMyCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;

        //        log.Debug("GetCalcoloICISoggetti::query::" + cmdMyCommand.CommandText + "::parametri::@codente" + CodEnte + "::Anno::" + Anno + "::Tributo::" + Tributo + "::CodContribuente::" + CodContribuente.ToString());
        //        Tabella = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //    }
        //    catch(Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICISoggetti.errore: ", Err);
        //        kill();
        //        Tabella = new DataTable();
        //    }
        //    finally
        //    {
        //        kill();
        //    }

        //    return Tabella;
        //}
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="CodEnte"></param>
        /// <param name="Tributo"></param>
        /// <param name="idProgrElab"></param>
        /// <returns></returns>
        //*** 20140509 - TASI ***
        public DataTable GetImmobiliCategoriaClasse(string CodContribuente, string Anno, string CodEnte, string Tributo, long idProgrElab)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetSituazioneFinaleCategoriaClasse";
                SelectCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
                if (CodContribuente.CompareTo("") != 0)
                {
                    SelectCommand.Parameters.Add("@IdContribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
                }
                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
                }
                if (CodEnte.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@IdEnte", SqlDbType.NVarChar).Value = CodEnte;
                }
                if (idProgrElab != -1)
                {
                    SelectCommand.Parameters.Add("@IdElaborazione", SqlDbType.BigInt).Value = idProgrElab;
                }
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetImmobiliCategoriaClasse.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        //*** ***

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="Tributo"></param>
        /// <param name="CodEnte"></param>
        /// <returns></returns>
        //*** 20140509 - TASI ***
        public DataTable GetRiepilogoMassivo(string CodContribuente, string Anno, string Tributo, string CodEnte)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetSituazioneFinaleRiepMassivo";
                SelectCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
                if (CodContribuente.CompareTo("") != 0)
                {
                    SelectCommand.Parameters.Add("@IdContribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
                }
                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
                }
                if (CodEnte.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@IdEnte", SqlDbType.NVarChar).Value = CodEnte;
                }
                //*** ***
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetRiepilogoMassivo.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        //*** ***

        //		public DataSet CreateDSperRiepilogoICI() 
        //		{ 
        //			DataSet objDS = new DataSet(); 
        //			DataTable newTable; 
        //  try{
        //			newTable = new DataTable("TP_SITUAZIONE_FINALE_ICI"); 
        //
        //			DataColumn NewColumn = new DataColumn(); 
        //			NewColumn.ColumnName = "ANNO"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = ""; 
        //			newTable.Columns.Add(NewColumn); 
        //
        //			NewColumn = new DataColumn(); 
        //			NewColumn.ColumnName = "ABITAZIONE_PRINCIPALE"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = ""; 
        //			newTable.Columns.Add(NewColumn); 
        //
        //			NewColumn = new DataColumn(); 
        //			NewColumn.ColumnName = "ALTRI_FABBRICATI"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = ""; 
        //			newTable.Columns.Add(NewColumn);
        // 
        //			NewColumn = new DataColumn(); 
        //			NewColumn.ColumnName = "AREE_EDIFICABILI"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = ""; 
        //			newTable.Columns.Add(NewColumn); 
        //
        //			NewColumn = new DataColumn(); 
        //			NewColumn.ColumnName = "TERRENI_AGRICOLI"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = "0"; 
        //			newTable.Columns.Add(NewColumn); 
        //
        //			NewColumn.ColumnName = "DETRAZIONE"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = "0"; 
        //			newTable.Columns.Add(NewColumn); 
        //
        //			NewColumn = new DataColumn(); 
        //			NewColumn.ColumnName = "TOTALE"; 
        //			NewColumn.DataType = System.Type.GetType("System.String"); 
        //			NewColumn.DefaultValue = ""; 
        //			newTable.Columns.Add(NewColumn); 
        //
        //			objDS.Tables.Add(newTable); 
        //
        //			return objDS; 
        // }
        //   catch(Exception Err)
        //  {
        // log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.CreateDSperRiepilogoICI.errore: ", Err);
        //}
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="cod_ente"></param>
        /// <returns></returns>
        public DataTable GetImportoDovuto(int CodContribuente, string Anno, string cod_ente)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "Select sum(ICI_DOVUTA_ACCONTO) as ACCONTO ,sum(ICI_DOVUTA_SALDO) as SALDO";
                SelectCommand.CommandText += " From " + this.TableName + " where 1=1";

                if (CodContribuente.CompareTo(-1) != 0)
                {
                    SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
                    SelectCommand.Parameters.Add("@codcontribuente", SqlDbType.Int).Value = CodContribuente;
                }

                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.CommandText += " AND ANNO=@anno";
                    SelectCommand.Parameters.Add("@anno", SqlDbType.NVarChar).Value = Anno;
                }
                if (cod_ente.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.CommandText += " AND cod_ente=@cod_ente";
                    SelectCommand.Parameters.Add("@cod_ente", SqlDbType.NVarChar).Value = cod_ente;
                }

                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetImportoDovuto.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <returns></returns>
        public DataTable GetCalcoloICI(int CodContribuente, string Anno)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "Select * From " + this.TableName + " where 1=1";

                if (CodContribuente.CompareTo(-1) != 0)
                {
                    SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
                    SelectCommand.Parameters.Add("@codcontribuente", SqlDbType.Int).Value = CodContribuente;
                }

                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.CommandText += " AND ANNO=@anno";
                    SelectCommand.Parameters.Add("@anno", SqlDbType.NVarChar).Value = Anno;
                }

                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetCalcoloICI.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        /// <summary>
        /// Funzione per l'estrazione dei dati di minuta
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="Tributo"></param>
        /// <param name="CodEnte"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        public DataTable GetStampaMinuta(string CodContribuente, string Anno, string Tributo, string CodEnte)
        {
            DataTable dtDati;

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetMinuta", "Tributo", "IdContribuente", "Anno", "IdEnte");
                    DataSet myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("Tributo", Tributo)
                            , ctx.GetParam("IdContribuente", CodContribuente)
                            , ctx.GetParam("Anno", Anno)
                            , ctx.GetParam("IdEnte", CodEnte)
                        );
                    dtDati = myDataSet.Tables[0];
                    ctx.Dispose();
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetStampaMinuta.errore: ", Err);
                kill();
                dtDati = new DataTable();
            }
            finally
            {
                kill();
            }
            return dtDati;
        }
        //public DataTable GetStampaMinuta(string CodContribuente, string Anno, string Tributo, string CodEnte)
        //{
        //    DataTable Tabella;

        //    try
        //    {
        //        SqlCommand SelectCommand = new SqlCommand();
        //        SelectCommand.CommandType = CommandType.StoredProcedure;
        //        SelectCommand.CommandText = "prc_GetMinuta";
        //        SelectCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
        //        if (CodContribuente.CompareTo("") != 0)
        //        {
        //            SelectCommand.Parameters.Add("@IdContribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
        //        }
        //        if (Anno.ToString().CompareTo("-1") != 0)
        //        {
        //            SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
        //        }
        //        if (CodEnte.ToString().CompareTo("-1") != 0)
        //        {
        //            SelectCommand.Parameters.Add("@IdEnte", SqlDbType.NVarChar).Value = CodEnte;
        //        }
        //        //*** ***
        //        Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //    }
        //    catch(Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetStampaMinuta.errore: ", Err);
        //        kill();
        //        Tabella = new DataTable();
        //    }
        //    finally
        //    {
        //        kill();
        //    }

        //    return Tabella;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objICI"></param>
        /// <returns></returns>
        public bool InsertCALCOLOICI(DataSet objICI)
        {
            bool InsertRetVal;
            string SQL;
            long lngID;
            bool blnVal = true;
            try {
                foreach (DataRow myRow in objICI.Tables["TP_SITUAZIONE_FINALE_ICI"].Rows)
                {
                    SQL = "DELETE FROM TP_SITUAZIONE_FINALE_ICI WHERE ANNO =" + this.CStrToDB(myRow["ANNO"], ref blnVal, false) + " AND COD_ENTE=" + this.CStrToDB(myRow["COD_ENTE"], ref blnVal, false) + " AND COD_CONTRIBUENTE=" + this.cToInt(myRow["COD_CONTRIBUENTE"]);

                    SqlCommand deleteCommand = new SqlCommand();
                    deleteCommand.CommandText = SQL;

                    InsertRetVal = Execute(deleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));

                    deleteCommand.Dispose();
                }
                foreach (DataRow myRow in objICI.Tables["TP_SITUAZIONE_FINALE_ICI"].Rows)
                {
                    lngID = this.getNewID(this.TableName);

                    SQL = "INSERT INTO TP_SITUAZIONE_FINALE_ICI (ID_SITUAZIONE_FINALE,ANNO,COD_ENTE,PROVENIENZA,CARATTERISTICA,INDIRIZZO,SEZIONE,FOGLIO,NUMERO,SUBALTERNO";
                    SQL = SQL + ",CATEGORIA,CLASSE,PROTOCOLLO,FLAG_STORICO,VALORE,FLAG_PROVVISORIO,PERC_POSSESSO,MESI_POSSESSO,MESI_ESCL_ESENZIONE,MESI_RIDUZIONE,IMPORTO_DETRAZIONE";
                    SQL = SQL + ",FLAG_POSSEDUTO,FLAG_ESENTE,FLAG_RIDUZIONE,FLAG_PRINCIPALE,COD_CONTRIBUENTE,COD_IMMOBILE_PERTINENZA,COD_IMMOBILE,DAL,AL,NUMERO_MESI_ACCONTO,NUMERO_MESI_TOTALI";
                    SQL = SQL + ",NUMERO_UTILIZZATORI,TIPO_RENDITA,ICI_ACCONTO_SENZA_DETRAZIONE,ICI_ACCONTO_DETRAZIONE_APPLICATA,ICI_DOVUTA_ACCONTO,ICI_ACCONTO_DETRAZIONE_RESIDUA,ICI_TOTALE_SENZA_DETRAZIONE";
                    SQL = SQL + ",ICI_TOTALE_DETRAZIONE_APPLICATA,ICI_TOTALE_DOVUTA,ICI_TOTALE_DETRAZIONE_RESIDUA,ICI_DOVUTA_SALDO,ICI_DOVUTA_DETRAZIONE_SALDO,ICI_DOVUTA_SENZA_DETRAZIONE,ICI_DOVUTA_DETRAZIONE_RESIDUA,RIDUZIONE,MESE_INIZIO,DATA_SCADENZA,RITORNATA,DATA_ELABORAZIONE)";

                    SQL = SQL + " VALUES(" + this.CIdToDB(lngID);
                    SQL = SQL + "," + this.CStrToDB(myRow["ANNO"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["COD_ENTE"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["PROVENIENZA"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["CARATTERISTICA"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["INDIRIZZO"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["SEZIONE"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["FOGLIO"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["NUMERO"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["SUBALTERNO"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["CATEGORIA"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["CLASSE"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["PROTOCOLLO"], ref blnVal, false);
                    SQL = SQL + "," + this.CToBit(myRow["FLAG_STORICO"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["VALORE"]);
                    SQL = SQL + "," + this.CToBit(myRow["FLAG_PROVVISORIO"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["PERC_POSSESSO"]);
                    SQL = SQL + "," + this.CStrToDB(myRow["MESI_POSSESSO"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["MESI_ESCL_ESENZIONE"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["MESI_RIDUZIONE"], ref blnVal, false);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["IMPORTO_DETRAZIONE"]);
                    SQL = SQL + "," + this.CToBit(myRow["FLAG_POSSEDUTO"]);
                    SQL = SQL + "," + this.CToBit(myRow["FLAG_ESENTE"]);
                    SQL = SQL + "," + this.CToBit(myRow["FLAG_RIDUZIONE"]);
                    SQL = SQL + "," + this.cToInt(myRow["FLAG_PRINCIPALE"]);
                    SQL = SQL + "," + this.cToInt(myRow["COD_CONTRIBUENTE"]);
                    SQL = SQL + "," + this.cToInt(myRow["COD_IMMOBILE_PERTINENZA"]);
                    SQL = SQL + "," + this.cToInt(myRow["COD_IMMOBILE"]);
                    SQL = SQL + "," + this.CStrToDB(myRow["DAL"], ref blnVal, false);
                    SQL = SQL + "," + this.CStrToDB(myRow["AL"], ref blnVal, false);
                    SQL = SQL + "," + this.cToInt(myRow["NUMERO_MESI_ACCONTO"]);
                    SQL = SQL + "," + this.cToInt(myRow["NUMERO_MESI_TOTALI"]);
                    SQL = SQL + "," + this.cToInt(myRow["NUMERO_UTILIZZATORI"]);
                    SQL = SQL + "," + this.CStrToDB(myRow["TIPO_RENDITA"], ref blnVal, false);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_ACCONTO_SENZA_DETRAZIONE"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_ACCONTO_DETRAZIONE_APPLICATA"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_DOVUTA_ACCONTO"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_ACCONTO_DETRAZIONE_RESIDUA"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_TOTALE_SENZA_DETRAZIONE"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_TOTALE_DETRAZIONE_APPLICATA"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_TOTALE_DOVUTA"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_TOTALE_DETRAZIONE_RESIDUA"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_DOVUTA_SALDO"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_DOVUTA_DETRAZIONE_SALDO"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_DOVUTA_SENZA_DETRAZIONE"]);
                    SQL = SQL + "," + this.CDoubleToDB(myRow["ICI_DOVUTA_DETRAZIONE_RESIDUA"]);
                    SQL = SQL + "," + this.CToBit(myRow["RIDUZIONE"]);
                    SQL = SQL + "," + this.cToInt(myRow["MESE_INIZIO"]);
                    SQL = SQL + "," + this.CStrToDB(myRow["DATA_SCADENZA"], ref blnVal, false);
                    SQL = SQL + ",0";
                    SQL = SQL + "," + System.DateTime.Now.ToString("yyyyMMdd") + ")";

                    SqlCommand insertCommand = new SqlCommand();
                    insertCommand.CommandText = SQL;

                    InsertRetVal = Execute(insertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
                    if (InsertRetVal == false)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.InsertCALCOLOICI.errore: ", Err);
                throw Err;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vInput"></param>
        /// <returns></returns>
        public string CIdToDB(object vInput)
        {
            string stringa = string.Empty;
            stringa = "Null";
            try {
                if (vInput.ToString() != DBNull.Value.ToString())
                {

                    double result;
                    if (double.TryParse(vInput.ToString(), NumberStyles.Integer, null, out result) == true)
                    //if (IsNumeric(vInput)) 
                    {
                        if (System.Convert.ToDouble(vInput) > 0)
                        {
                            stringa = System.Convert.ToString(System.Convert.ToDouble(vInput));
                        }
                    }
                }
                return stringa;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.CldToDB.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vInput"></param>
        /// <param name="blnClearSpace"></param>
        /// <param name="blnUseNull"></param>
        /// <returns></returns>
        public string CStrToDB(object vInput, ref bool blnClearSpace, bool blnUseNull)
        {
            string sTesto;
            string stringa = string.Empty;
            try {
                if (blnUseNull)
                {
                    stringa = "Null";
                }
                else
                {
                    stringa = "''";
                }
                if (vInput.ToString() != DBNull.Value.ToString())
                {
                    sTesto = System.Convert.ToString(vInput);
                    if (blnClearSpace)
                    {
                        sTesto = sTesto.Trim();
                    }
                    if (sTesto.Trim() != "")
                    {
                        stringa = "'" + sTesto.Replace("'", "''") + "'";
                    }
                }

                return stringa;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.CStrToDB.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vInput"></param>
        /// <returns></returns>
        public short CToBit(object vInput)
        {
            short myBool;
            myBool = 0;
            try {
                if (vInput.ToString() != DBNull.Value.ToString())
                {
                    if (System.Convert.ToBoolean(vInput))
                    {
                        myBool = 1;
                    }
                    else
                    {
                        myBool = 0;
                    }
                }

                return myBool;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.CToBit.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vInput"></param>
        /// <returns></returns>
        public string CDoubleToDB(object vInput)
        {
            string strToDbl = "Null";
            try {
                if (vInput.ToString() != DBNull.Value.ToString())
                {
                    strToDbl = System.Convert.ToString(vInput);
                    strToDbl = strToDbl.Replace(",", ".");
                }
                return strToDbl;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.CDoubleToDB.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public int cToInt(object objInput)
        {
            int intero = 0;
            try {
                if (objInput.ToString() != DBNull.Value.ToString())
                {
                    double result;
                    if (double.TryParse(objInput.ToString(), NumberStyles.Integer, null, out result) == true)
                    //if (IsNumeric(objInput)) 
                    {
                        intero = Convert.ToInt32(objInput);
                    }
                }
                return intero;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.cToInt.errore: ", Err);
                throw Err;
            }
        }

        /*SELECT     OPEN_ANAGRAFICA.DBO.ANAGRAFICA.COGNOME_DENOMINAZIONE, OPEN_ANAGRAFICA.DBO.ANAGRAFICA.NOME, 
        OPEN_ANAGRAFICA.DBO.ANAGRAFICA.COD_FISCALE, OPEN_ANAGRAFICA.DBO.ANAGRAFICA.PARTITA_IVA, 
        TP_SITUAZIONE_FINALE_ICI.COD_CONTRIBUENTE, ANNO, SUM(CASE WHEN (FLAG_PRINCIPALE = 1) THEN ICI_TOTALE_DOVUTA ELSE 0 END) 
        AS Imp_Abi_Princ, SUM(CASE WHEN (TIPO_RENDITA <> N'AF') AND (TIPO_RENDITA <> N'TA') AND (flag_principale <>1) 
        THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS Imp_Altri_Fab, SUM(CASE WHEN (TIPO_RENDITA = N'AF') THEN ICI_TOTALE_DOVUTA ELSE 0 END) 
        AS Imp_Aree_Fab, SUM(CASE WHEN (TIPO_RENDITA = N'TA') THEN ICI_TOTALE_DOVUTA ELSE 0 END) AS Imp_Terreni, 
        SUM(ICI_TOTALE_DETRAZIONE_APPLICATA) AS Detrazione, SUM(ICI_TOTALE_DOVUTA) AS Totale
        FROM         TP_SITUAZIONE_FINALE_ICI INNER JOIN
        OPEN_ANAGRAFICA.DBO.ANAGRAFICA ON 
        OPEN_ANAGRAFICA.DBO.ANAGRAFICA.COD_CONTRIBUENTE = TP_SITUAZIONE_FINALE_ICI.COD_CONTRIBUENTE
        GROUP BY ANNO, TP_SITUAZIONE_FINALE_ICI.COD_CONTRIBUENTE, OPEN_ANAGRAFICA.DBO.ANAGRAFICA.COGNOME_DENOMINAZIONE, 
        OPEN_ANAGRAFICA.DBO.ANAGRAFICA.NOME, OPEN_ANAGRAFICA.DBO.ANAGRAFICA.COD_FISCALE, 
        OPEN_ANAGRAFICA.DBO.ANAGRAFICA.PARTITA_IVA*/

        //*** 20140509 - TASI ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="CodEnte"></param>
        /// <param name="Tributo"></param>
        /// <param name="nettoVersato"></param>
        /// <returns></returns>
        public DataTable GetBollettinoICI(string CodContribuente, string Anno, string CodEnte, string Tributo, bool nettoVersato)
        {
            DataTable Tabella;
            SqlCommand SelectCommand = new SqlCommand();

            try
            {
                SelectCommand.CommandText = "SELECT *";
                if (nettoVersato)
                {
                    SelectCommand.CommandText += " FROM V_GETBOLLETTINONETTOVERSATO ";
                }
                else
                {
                    SelectCommand.CommandText += " FROM V_GETBOLLETTINOTOTALE ";
                }

                SelectCommand.CommandText += " WHERE 1=1";
                if (CodContribuente.CompareTo("") != 0)
                {
                    SelectCommand.CommandText += " AND COD_CONTRIBUENTE=@codcontribuente";
                    SelectCommand.Parameters.Add("@codcontribuente", SqlDbType.Int).Value = int.Parse(CodContribuente.ToString());
                }
                if (Tributo.ToString().CompareTo("") != 0)
                {
                    SelectCommand.CommandText += " AND (CODTRIBUTO=@Tributo)";
                    SelectCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
                }
                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.CommandText += " AND ANNO=@anno";
                    SelectCommand.Parameters.Add("@anno", SqlDbType.NVarChar).Value = Anno;
                }

                if (CodEnte.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.CommandText += " AND CODICE_ENTE=@codente";
                    SelectCommand.Parameters.Add("@codente", SqlDbType.NVarChar).Value = CodEnte;
                }
                log.Debug("ddl" + SelectCommand.CommandText + "::parametri::@codente" + CodEnte + "::Anno::" + Anno + "::CodContribuente::" + CodContribuente.ToString());
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetBollettinoICI.errore: ", Err);
                Tabella = new DataTable();
            }
            finally
            {
                SelectCommand.Connection.Close();
                SelectCommand.Dispose();
            }

            return Tabella;
        }
        //*** ***

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodContribuente"></param>
        /// <param name="Anno"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public bool ConfermaCalcoloICI(string CodContribuente, string Anno)
        {
            bool RetVal = false;
            DataView myDataView = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    ConstWrapper.nTry = 0;
                    ReDo:
                    try
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_SetConfermaCalcolo", "idente"
                            , "anno"
                            , "Codcontribuente"
                            , "utenteConferma"
                        );
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("idente", ConstWrapper.CodiceEnte)
                               , ctx.GetParam("anno", Anno)
                               , ctx.GetParam("Codcontribuente", CodContribuente)
                               , ctx.GetParam("utenteConferma", ConstWrapper.sUsername.ToString())
                           );
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") && ConstWrapper.nTry <= 3)
                        {
                            ConstWrapper.nTry += 1;
                            goto ReDo;
                        }
                        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.ConfermaCalcoloICI.errore: ", ex);
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                    foreach (DataRowView myRow in myDataView)
                    {
                        if (StringOperation.FormatInt(myRow["id"]) <= 0)
                            RetVal = false;
                        else
                            RetVal = true;
                    }
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.ConfermaCalcoloICI.errore: ", Err);
                RetVal = false;
            }
            return RetVal;
        }
        //public bool ConfermaCalcoloICI(string CodContribuente, string Anno)
        //{
        //    string SQL;
        //    /*long lngID;
        //    bool blnVal=true; */

        //    SQL = "UPDATE TP_CALCOLO_FINALE_ICI SET ";
        //    SQL += "CONFERMATO_ICI=@confermatoICI, ";
        //    SQL += "DATA_CONFERMA =@dataConferma, ";
        //    SQL += "UTENTE_CONFERMA=@utenteConferma ";
        //    SQL += "WHERE COD_CONTRIBUENTE =@Codcontribuente";
        //    SQL += " AND ANNO =@anno";
        //    SQL += " AND COD_ENTE =@codEnte";



        //    SqlCommand updateCommand = new SqlCommand();
        //    try { 
        //    updateCommand.CommandText = SQL;
        //    updateCommand.Parameters.Add("@Codcontribuente", SqlDbType.Int).Value = int.Parse(CodContribuente);
        //    updateCommand.Parameters.Add("@anno", SqlDbType.NVarChar).Value = Anno;
        //    updateCommand.Parameters.Add("@codEnte", SqlDbType.NVarChar).Value = ConstWrapper.CodiceEnte;

        //    updateCommand.Parameters.Add("@confermatoICI", SqlDbType.Bit).Value = 0;
        //    updateCommand.Parameters.Add("@dataConferma", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString();
        //    updateCommand.Parameters.Add("@utenteConferma", SqlDbType.NVarChar).Value = ConstWrapper.sUsername.ToString();

        //    return Execute(updateCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.ConfermaCalcoloICI.errore: ", Err);
        //        throw Err;
        //    }
        //}

        //*** 20140509 - TASI ***//*** 20150430 - TASI Inquilino ***
        /// <summary>
        /// ListContribuentiPerElaborazioneICI
        /// </summary>
        /// <param name="Nominativo"></param>
        /// <param name="NominativoDa"></param>
        /// <param name="NominativoA"></param>
        /// <param name="AnnoRiferimento"></param>
        /// <param name="CodiceEnte"></param>
        /// <param name="OrderBy"></param>
        /// <param name="Tributo"></param>
        /// <param name="sTipoTASI"></param>
        /// <returns></returns>
        public DataTable ListContribuentiPerElaborazioneICI(string Nominativo, string NominativoDa, string NominativoA, int AnnoRiferimento, string CodiceEnte, TipoOrdinamento OrderBy, string Tributo, string sTipoTASI)
        {
            SqlCommand cmdMyCommand = new SqlCommand();

            try
            {
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandTimeout = 0;
                cmdMyCommand.CommandText = "prc_ContribuentiPerElabDoc";
                cmdMyCommand.Parameters.Add("@CodEnte", SqlDbType.VarChar).Value = CodiceEnte;
                cmdMyCommand.Parameters.Add("@Anno", SqlDbType.VarChar).Value = AnnoRiferimento.ToString();
                cmdMyCommand.Parameters.Add("@Nominativo", SqlDbType.VarChar).Value = Nominativo;
                cmdMyCommand.Parameters.Add("@NominativoDa", SqlDbType.VarChar).Value = NominativoDa;
                cmdMyCommand.Parameters.Add("@NominativoA", SqlDbType.VarChar).Value = NominativoA;
                cmdMyCommand.Parameters.Add("@Ordinamento", SqlDbType.VarChar).Value = OrderBy.ToString();
                cmdMyCommand.Parameters.Add("@Tributo", SqlDbType.VarChar).Value = Tributo;
                cmdMyCommand.Parameters.Add("@TipoTasi", SqlDbType.VarChar).Value = sTipoTASI;
                DataTable dt = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
                return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.ListContribuentiPerElaborazioneICI.errore: ", Err);
                return null;
            }
            finally
            { kill(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodEnte"></param>
        /// <param name="Tributo"></param>
        /// <param name="idFlussoRuolo"></param>
        /// <returns></returns>
        public RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] GetDocElaboratiEffettivi(string CodEnte, string Tributo, int idFlussoRuolo)
        {
            RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL oOggettoGruppoURL;
            RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] ListGruppoURL = null;
            RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL oOggettoURL;
            SqlCommand cmdMyCommand = new SqlCommand();
            DataTable Tabella;
            ArrayList ListDocElabEff = new ArrayList();

            try
            {
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = "prc_GetDocEffettiviElaborati";
                cmdMyCommand.Parameters.Add("@IDENTE", SqlDbType.NVarChar).Value = CodEnte;
                cmdMyCommand.Parameters.Add("@TRIBUTO", SqlDbType.NVarChar).Value = Tributo;
                cmdMyCommand.Parameters.Add("@IDFLUSSORUOLO", SqlDbType.Int).Value = idFlussoRuolo;

                log.Debug("GetDocElaboratiEffettivi::query::" + cmdMyCommand.CommandText + "::parametri::@codente" + CodEnte + "::Tributo::" + Tributo + "::idFlussoRuolo::" + idFlussoRuolo.ToString());
                Tabella = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnectionOPENgov));
                if (Tabella.Rows.Count > 0)
                {
                    foreach (DataRow myRow in Tabella.Rows)
                    {
                        oOggettoURL = new RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL();

                        oOggettoURL.Name = myRow["NOME_FILE"].ToString();
                        oOggettoURL.Path = myRow["PATH"].ToString();
                        oOggettoURL.Url = myRow["PATH_WEB"].ToString();

                        oOggettoGruppoURL = new RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL();
                        oOggettoGruppoURL.URLComplessivo = oOggettoURL;

                        ListDocElabEff.Add(oOggettoGruppoURL);
                    }
                    ListGruppoURL = (RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[])ListDocElabEff.ToArray(typeof(RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL));
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.TpSituazioneFinaleIci.GetDocElaboratiEffettivi.errore: ", Err);
                kill();
                ListGruppoURL = null;
            }
            finally
            {
                kill();
            }

            return ListGruppoURL;
        }
        //*** ***
        //*** 201511 - template documenti per ruolo ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="sAnno"></param>
        /// <param name="sIdTributo"></param>
        /// <returns></returns>
        public int getIdRuolo(string IdEnte, string sAnno, string sIdTributo)
        {
            DataView myDataView = new DataView();
            int myRet = -1;
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    ConstWrapper.nTry = 0;
                    ReDo:
                    try
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetIdRuolo", "IdEnte", "Anno", "IdTributo");
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", IdEnte)
                            , ctx.GetParam("Anno", sAnno)
                            , ctx.GetParam("IdTributo", sIdTributo)
                        );
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") && ConstWrapper.nTry <= 3)
                        {
                            ConstWrapper.nTry += 1;
                            goto ReDo;
                        }
                        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaAnni.errore: ", ex);
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                    foreach (DataRowView myRow in myDataView)
                    {
                        int.TryParse(myRow["IdRuolo"].ToString(), out myRet);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaAnni.errore: ", ex);
                myDataView = null;
            }
            return myRet;
        }
        /**** ****/
    }
}
