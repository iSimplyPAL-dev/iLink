using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using OPENUtility;
using log4net;

namespace Business
{
    /// <summary>
    /// Classe con le funzioni per le interrogazioni di confronto con il catasto
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ClsAnalisi
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ClsAnalisi));
		private SqlCommand cmdMyCommand = new SqlCommand();
		private DataView DvDati;
        /// <summary>
        /// 
        /// </summary>
        public enum TypeCheckRifCatastali
        {///Riferimenti chiusi e non riaperti
			RifChiusi = 0,
            ///Riferimenti mancanti
            RifMancanti = 1,
            ///Riferimenti doppi per lo stesso periodo
            RifDoppi = 2,
            ///Riferimenti accertati 
            RifAccertati = 3,
            ///Rif.Catastali non in Dichiarazioni
            CatNoDic = 4,
            ///Rif.Dichiarati non a castato
            DicNoCat = 5,
            ///Posizione Dichiarata uguale a Catastale
            CatEqualDic = 6,
            ///Cat. e/o Classe Dichiarata diversa da Catastale
            CatDifferentDic = 7,
            ///Rendita e/o Consistenza Catastale diversa da Dichiarata
            RenCatDifferentDic = 8,
            ///Proprietario Catastale diverso da Dichiarato
            PropCatDifferentDic = 9,
            ///Errata percentuale di possesso (minore o maggiore di 100)
            RifErrataCopertura = 10,
            ///Rif.Dichiarati in ICI ma non in TARSU
            ICInoTARSU = 11,
            ///Rif.Dichiarati in TARSU ma non in ICI
            TARSUnoICI = 12
        }

        #region "Analisi Economiche"
        //*** 20140630 - TASI ***
        /// <summary>
        /// funzione che preleva il numero di utenti e la somma degli importi per il dovuto e il versato in base ai parametri in ingresso
        /// </summary>
        /// <param name="sMyIdEnte"></param>
        /// <param name="sMyTributo"></param>
        /// <param name="sAnno"></param>
        /// <param name="sAccreditoDal"></param>
        /// <param name="sAccreditoAl"></param>
        /// <returns></returns>
        public DataView GetRiepilogoEmesso(string sMyIdEnte, string sMyTributo, string sAnno, string sAccreditoDal, string sAccreditoAl)
        {
            try
            {
                //Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                //valorizzo il commandText
                cmdMyCommand.CommandText = "prc_FatInc_RiepilogoEmesso";
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODTRIBUTO", SqlDbType.VarChar)).Value = sMyTributo;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
                if (sAccreditoDal != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
                }
                if (sAccreditoAl != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
                }
                //eseguo la query
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmdMyCommand);
                DataSet dsMyDati=new DataSet();
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dsMyDati);
                DvDati = dsMyDati.Tables[0].DefaultView;
                return DvDati;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetRiepilogoEmesso.errore: ", Err);
                return null;
            }
        }

        /// <summary>
        /// funzione che preleva il numero e l'importo dei versamenti in unica soluzione piuttosto che a rate
        /// </summary>
        /// <param name="sMyIdEnte"></param>
        /// <param name="sMyTributo"></param>
        /// <param name="sAnno"></param>
        /// <param name="sAccreditoDal"></param>
        /// <param name="sAccreditoAl"></param>
        /// <param name="IsEvaseTotalmente"></param>
        /// <returns></returns>
        public DataView GetRiepilogoEmessoEvaso(string sMyIdEnte, string sMyTributo, string sAnno, string sAccreditoDal, string sAccreditoAl, int IsEvaseTotalmente)
        {
            try
            {
                //Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                //valorizzo il commandText
                cmdMyCommand.CommandText = "prc_FatInc_RiepilogoEmessoEvaso";
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODTRIBUTO", SqlDbType.VarChar)).Value = sMyTributo;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ISEVASOTOT", SqlDbType.Int)).Value = IsEvaseTotalmente;
                if (sAccreditoDal != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
                }
                if (sAccreditoAl != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
                }
                //eseguo la query
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmdMyCommand);
                DataSet dsMyDati = new DataSet();
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dsMyDati);
                DvDati = dsMyDati.Tables[0].DefaultView;
                return DvDati;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetRiepilogoEmessoEvaso.errore: ", Err);
                return null;
            }
        }
        //public DataView GetRiepilogoEmessoEvaso(string sMyIdEnte, string sAnno, string sAccreditoDal, string sAccreditoAl, int IsEvaseTotalmente, OPENUtility.CreateSessione WFSession)
        //{
        //    try 
        //    {
        //        //Valorizzo la connessione
        //        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection();
        //        //valorizzo il commandText
        //        cmdMyCommand.CommandText = "SELECT SUM(TMPVERS.NRC) AS NPAG, SUM(TMPVERS.IMP) AS IMPPAG";
        //        cmdMyCommand.CommandText += " FROM (";
        //        cmdMyCommand.CommandText += " SELECT TBLVERSAMENTI.ENTE AS IDENTE, TBLVERSAMENTI.ANNORIFERIMENTO AS ANNO, COUNT(TBLVERSAMENTI.ID) AS NRC, SUM(TBLVERSAMENTI.IMPORTOPAGATO) AS IMP, (CAST(TBLVERSAMENTI.ACCONTO AS NUMERIC)+CAST(TBLVERSAMENTI.SALDO AS NUMERIC)) AS TIPOVERSAMENTO";
        //        cmdMyCommand.CommandText += " FROM TBLVERSAMENTI";
        //        cmdMyCommand.CommandText += " WHERE (1=1)";
        //        if (sAccreditoDal!="")
        //        {
        //            cmdMyCommand.CommandText += " AND (DATAPAGAMENTO>=@VALUTADAL)";
        //        }
        //        if (sAccreditoAl!="")
        //        {
        //            cmdMyCommand.CommandText += " AND (DATAPAGAMENTO<=@VALUTAAL)";
        //        }
        //        cmdMyCommand.CommandText += " GROUP BY TBLVERSAMENTI.ENTE, TBLVERSAMENTI.ANNORIFERIMENTO, (CAST(TBLVERSAMENTI.ACCONTO AS NUMERIC)+CAST(TBLVERSAMENTI.SALDO AS NUMERIC))";
        //        cmdMyCommand.CommandText += " ) TMPVERS";
        //        cmdMyCommand.CommandText += " WHERE (TMPVERS.IDENTE=@CODISTAT)";
        //        if (sAnno!="") 
        //        {
        //            cmdMyCommand.CommandText += " AND (TMPVERS.ANNO=@ANNO)";
        //        }
        //        if (IsEvaseTotalmente == 1)
        //        {
        //            cmdMyCommand.CommandText += " AND (TMPVERS.TIPOVERSAMENTO=2)";
        //        }
        //        else
        //        {
        //            cmdMyCommand.CommandText += " AND (TMPVERS.TIPOVERSAMENTO=0 OR TMPVERS.TIPOVERSAMENTO=1)";
        //        }

        //        //valorizzo i parameters
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
        //        if (sAccreditoDal!="") 
        //        {
        //            cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
        //        }
        //        if (sAccreditoAl!="") 
        //        {
        //            cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
        //        }
        //        //eseguo la query
        //        DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand);
        //        return DvDati;
        //    }
        //    catch (Exception Err) 
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetRiepilogoEmessoEvaso.errore: ", Err);
        //        return null;
        //    }
        //}

        /// <summary>
        /// funzione che preleva il dettaglio degli importi del dovuto
        /// </summary>
        /// <param name="sMyIdEnte"></param>
        /// <param name="sMyTributo"></param>
        /// <param name="sAnno"></param>
        /// <param name="sAccreditoDal"></param>
        /// <param name="sAccreditoAl"></param>
        /// <returns></returns>
        //public DataView GetDettaglioEmesso(string sMyIdEnte, string sAnno, string sAccreditoDal, string sAccreditoAl, OPENUtility.CreateSessione WFSession)
        //{
        //    try 
        //    {
        //        //Valorizzo la connessione
        //        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection();
        //valorizzo il commandText
        //				cmdMyCommand.CommandText = "SELECT SUM(ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE) AS ABIPRIN"+
        //				", SUM(ICI_DOVUTA_TERRENI_TOTALE) AS TERAGR, SUM(ICI_DOVUTA_AREE_FABBRICABILI_TOTALE) AS TERFAB, SUM(ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE) AS ALTRIFAB"+
        //				/**** 20120828 - IMU adeguamento per importi statali ****/
        //				", SUM(IMP_TERRENI_TOT_STATALE) AS TERAGRSTATO, SUM(IMP_AREE_FAB_TOT_STATALE) AS TERFABSTATO, SUM(IMP_ALTRI_FAB_TOT_STATALE) AS ALTRIFABSTATO"+
        //				", SUM(IMP_FABRURUSOSTRUM_TOT) AS FABRURUSOSTRUM"+
        //				/**** ****/
        //				/**** 20130422 - aggiornamento IMU ****/
        //				", SUM(IMP_FABRURUSOSTRUM_TOT_STATALE) AS FABRURUSOSTRUMSTATO"+
        //				", SUM(IMP_USOPRODCATD_TOT) AS USOPRODCATD"+
        //				", SUM(IMP_USOPRODCATD_TOT_STATALE) AS USOPRODCATDSTATO"+
        //				/**** ****/
        //				", SUM(ICI_DOVUTA_DETRAZIONE_TOTALE) AS DETRAZIONE, SUM(ICI_DOVUTA_TOTALE) AS IMPORTO";
        //				cmdMyCommand.CommandText += " FROM TP_CALCOLO_FINALE_ICI";

        //        cmdMyCommand.CommandText = "SELECT SUM(IMP_ABI_PRINC) AS ABIPRIN";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_ALTRI_FAB) AS ALTRIFAB";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_AREE_FAB) AS TERFAB";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_TERRENI) AS TERAGR";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_ALTRI_FAB_STATO) AS ALTRIFABSTATO";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_AREE_FAB_STATO) AS TERFABSTATO";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_TERRENI_STATO) AS TERAGRSTATO";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_FABRURUSOSTRUM) AS FABRURUSOSTRUM";
        //        //*** 20130422 - aggiornamento IMU ***
        //        cmdMyCommand.CommandText += " ,SUM(IMP_FABRURUSOSTRUM_STATO) AS FABRURUSOSTRUMSTATO";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_USOPRODCATD) AS USOPRODCATD";
        //        cmdMyCommand.CommandText += " ,SUM(IMP_USOPRODCATD_STATO) AS USOPRODCATDSTATO";
        //        //*** ***
        //        cmdMyCommand.CommandText += " ,SUM(DETRAZIONE) AS DETRAZIONE";
        //        cmdMyCommand.CommandText += " ,SUM(TOTALE) AS IMPORTO";
        //        cmdMyCommand.CommandText += " ,SUM(NUMFABB) AS NUMFABB";
        //        cmdMyCommand.CommandText += " FROM V_GETCALCOLOIMUTOTALE ";
        //        cmdMyCommand.CommandText += " WHERE 1=1";
        //        cmdMyCommand.CommandText += " AND (COD_ENTE=@CODISTAT)";
        //        if (sAnno!="") 
        //        {
        //            cmdMyCommand.CommandText += " AND (ANNO=@ANNO)";
        //        }
        //        if (sAccreditoDal!="" || sAccreditoAl!="") 
        //        {
        //            cmdMyCommand.CommandText += " AND COD_CONTRIBUENTE IN (";
        //            cmdMyCommand.CommandText += " SELECT DISTINCT IDANAGRAFICO";
        //            cmdMyCommand.CommandText += " FROM TBLVERSAMENTI";
        //            cmdMyCommand.CommandText += " WHERE (1=1)";
        //            if (sAccreditoDal!="")
        //            {
        //                cmdMyCommand.CommandText += " AND (DATAPAGAMENTO>=@VALUTADAL)";
        //            }
        //            if (sAccreditoAl!="")
        //            {
        //                cmdMyCommand.CommandText += " AND (DATAPAGAMENTO<=@VALUTAAL)";
        //            }
        //            cmdMyCommand.CommandText += " )";
        //        }
        //        //valorizzo i parameters
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
        //        if (sAccreditoDal!="") 
        //        {
        //            cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
        //        }
        //        if (sAccreditoAl!="") 
        //        {
        //            cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
        //        }
        //        log.Debug("GetDettaglioEmesso::query::"+ cmdMyCommand.CommandText +"::sMyIdEnte::"+ sMyIdEnte+"::sAnno::"+sAnno+"::sAccreditoDal::"+sAccreditoDal+"::sAccreditoAl::"+sAccreditoAl);
        //        //eseguo la query
        //        DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand);
        //        return DvDati;
        //    }
        //    catch (Exception Err) 
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetDettaglioEmesso.errore: ", Err);
        //        return null;
        //    }
        //}
        public DataView GetDettaglioEmesso(string sMyIdEnte, string sMyTributo, string sAnno, string sAccreditoDal, string sAccreditoAl)
        {
            try
            {
                //Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                //valorizzo il commandText
                cmdMyCommand.CommandText = "prc_FatInc_DettaglioEmesso";
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODTRIBUTO", SqlDbType.VarChar)).Value = sMyTributo;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
                if (sAccreditoDal != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
                }
                if (sAccreditoAl != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
                }
                //eseguo la query
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmdMyCommand);
                DataSet dsMyDati = new DataSet();
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dsMyDati);
                DvDati = dsMyDati.Tables[0].DefaultView;
                return DvDati;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetDettaglioEmesso.errore: ", Err);
                return null;
            }
        }
        /// <summary>
        /// funzione che preleva il dettaglio degli importi del versato
        /// </summary>
        /// <param name="sMyIdEnte"></param>
        /// <param name="sMyTributo"></param>
        /// <param name="sAnno"></param>
        /// <param name="sAccreditoDal"></param>
        /// <param name="sAccreditoAl"></param>
        /// <returns></returns>
        public DataView GetDettaglioVersato(string sMyIdEnte, string sMyTributo, string sAnno, string sAccreditoDal, string sAccreditoAl)
        {
            try
            {
                //Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                //valorizzo il commandText
                cmdMyCommand.CommandText = "prc_FatInc_DettaglioVersato";
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
                cmdMyCommand.Parameters.Add(new SqlParameter("@CODTRIBUTO", SqlDbType.VarChar)).Value = sMyTributo;
                cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
                if (sAccreditoDal != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
                }
                if (sAccreditoAl != "")
                {
                    cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
                }
                //eseguo la query
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmdMyCommand);
                DataSet dsMyDati = new DataSet();
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myAdapter.Fill(dsMyDati);
                DvDati = dsMyDati.Tables[0].DefaultView;
                return DvDati;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetDettaglioVersato.errore: ", Err);
                return null;
            }
        }
        //public DataView GetDettaglioVersato(string sMyIdEnte, string sAnno, string sAccreditoDal, string sAccreditoAl, OPENUtility.CreateSessione WFSession)
        //{
        //    try 
        //    {
        //        //Valorizzo la connessione
        //        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection();
        //        //valorizzo il commandText
        //        cmdMyCommand.CommandText = "SELECT SUM(IMPORTOABITAZPRINCIPALE) AS ABIPRIN"+
        //        ", SUM(IMPOTERRENI) AS TERAGR, SUM(IMPORTOAREEFABBRIC) AS TERFAB, SUM(IMPORTOALTRIFABBRIC) AS ALTRIFAB"+
        //        /**** 20120828 - IMU adeguamento per importi statali ****/
        //        ", SUM(IMPORTOTERRENISTATALE) AS TERAGRSTATO, SUM(IMPORTOAREEFABBRICSTATALE) AS TERFABSTATO, SUM(IMPORTOALTRIFABBRICSTATALE) AS ALTRIFABSTATO"+
        //        ", SUM(IMPORTOFABRURUSOSTRUM) AS FABRURUSOSTRUM"+
        //        /**** ****/
        //        /**** 20130422 - aggiornamento IMU ****/
        //        ", SUM(IMPORTOFABRURUSOSTRUMSTATALE) AS FABRURUSOSTRUMSTATO"+
        //        ", SUM(IMPORTOUSOPRODCATD) AS USOPRODCATD"+
        //        ", SUM(IMPORTOUSOPRODCATDSTATALE) AS USOPRODCATDSTATO"+
        //        /**** ****/
        //        ", SUM(DETRAZIONEABITAZPRINCIPALE) AS DETRAZIONE, SUM(IMPORTOPAGATO) AS IMPORTO";
        //        cmdMyCommand.CommandText += " FROM TBLVERSAMENTI";
        //        cmdMyCommand.CommandText += " WHERE (TBLVERSAMENTI.ENTE=@CODISTAT)";
        //        if (sAnno != "") 
        //        {
        //            cmdMyCommand.CommandText += " AND (TBLVERSAMENTI.ANNORIFERIMENTO=@ANNO)";
        //        }
        //        if (sAccreditoDal!="")
        //        {
        //            cmdMyCommand.CommandText += " AND (DATAPAGAMENTO>=@VALUTADAL)";
        //        }
        //        if (sAccreditoAl!="")
        //        {
        //            cmdMyCommand.CommandText += " AND (DATAPAGAMENTO<=@VALUTAAL)";
        //        }
        //        //valorizzo i parameters
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte;
        //        cmdMyCommand.Parameters.Add(new SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno;
        //        if (sAccreditoDal!="") 
        //        {
        //            cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal;
        //        }
        //        if (sAccreditoAl!="") 
        //        {
        //            cmdMyCommand.Parameters.Add(new SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl;
        //        }
        //        //eseguo la query
        //        DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand);
        //        return DvDati;
        //    }
        //    catch (Exception Err) 
        //    {
        //       log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetDettaglioVersato.errore: ", Err);
        //        return null;
        //    }
        //}
        //*** ***


        #endregion

        #region "Confronto con Catasto"
                /// <summary>
        /// 
        /// </summary>
        /// <param name="myConn"></param>
        /// <param name="sMyIdEnte"></param>
        /// <param name="Anno"></param>
        /// <param name="nTypeCheck"></param>
        /// <param name="IsStampa"></param>
        /// <returns></returns>
        public DataSet GetCheckRifCatastali(string myConn, string sMyIdEnte, int Anno, int nTypeCheck, bool IsStampa)
        {
            DataSet myResult = new DataSet();
            SqlDataAdapter myDaDati = null;
            string SuffissoStampa = "";

            try
            {
                log.Debug("ClsAnalisi::GetCheckRifCatastali::devo caricare per tipo ricerca::" + nTypeCheck.ToString());
                if (IsStampa == true)
                {
                    SuffissoStampa = "_XSTAMPA";
                }
                // Valorizzo la connessione
                cmdMyCommand.Connection = new SqlConnection(myConn);
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                //valorizzo i parameters
                cmdMyCommand.Parameters.Clear();
                switch (nTypeCheck)
                {
                    case (int)TypeCheckRifCatastali.CatNoDic://Rif.Catastali non in Dichiarazioni
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Rif.Catastali non in Dichiarazioni");
                        // valorizzo il commandText
                        cmdMyCommand.CommandText = "prc_RIFCATnoRIFDICH" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    case (int)TypeCheckRifCatastali.RifMancanti://Rif.Catastali mancanti/non valorizzati
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Rif.Catastali mancanti/non valorizzati");
                        // valorizzo il commandText
                        cmdMyCommand.CommandText = "prc_RIFCATMANCANTI" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    case (int)TypeCheckRifCatastali.RifErrataCopertura://Rif.Catastali con errata percentuale di possesso (< o > di 100)
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Rif.Catastali con errata percentuale di possesso (< o > di 100)");
                        // valorizzo il commandText
                        cmdMyCommand.CommandText = "prc_RIFCATERRATACOPERTURA" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    case (int)TypeCheckRifCatastali.DicNoCat://Rif.Dichiarati non a castato
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Rif.Dichiarati non a castato");
                        // valorizzo il commandText
                        cmdMyCommand.CommandText = "prc_RIFDICHnoRIFCAT" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    case (int)TypeCheckRifCatastali.CatEqualDic://Posizione Dichiarata uguale a Catastale
                    case (int)TypeCheckRifCatastali.CatDifferentDic://Cat. e/o Classe Dichiarata diversa da Catastale
                    case (int)TypeCheckRifCatastali.RenCatDifferentDic://Rendita e/o Consistenza Catastale diversa da Dichiarata
                    case (int)TypeCheckRifCatastali.PropCatDifferentDic://Proprietario Catastale diverso da Dichiarato
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Posizione Dichiarata uguale a Catastale:::+::Cat. e/o Classe Dichiarata diversa da Catastale::+::Rendita e/o Consistenza Catastale diversa da Dichiarata::+::Proprietario Catastale diverso da Dichiarato");
                        // valorizzo il commandText
                        cmdMyCommand.CommandText = "prc_RIFDICHvsRIFCAT" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@TypeEstraz", SqlDbType.Int)).Value = nTypeCheck;
                        break;
                    case (int)TypeCheckRifCatastali.RifAccertati://Riferimenti accertati
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Riferimenti accertati");
                        cmdMyCommand.CommandText = "prc_RIFCATACCERTATI" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    case (int)TypeCheckRifCatastali.ICInoTARSU://Rif.Dichiarati in ICI ma non in TARSU
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Rif.Dichiarati in ICI ma non in TARSU");
                        cmdMyCommand.CommandText = "prc_RIFICInoTARSU" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    case (int)TypeCheckRifCatastali.TARSUnoICI://Rif.Dichiarati in TARSU ma non in ICI
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::Rif.Dichiarati in TARSU ma non in ICI");
                        cmdMyCommand.CommandText = "prc_RIFTARSUnoICI" + SuffissoStampa;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte;
                        cmdMyCommand.Parameters.Add(new SqlParameter("@Anno", SqlDbType.Int)).Value = Anno;
                        break;
                    default://Riferimenti chiusi e non riaperti//Riferimenti doppi per lo stesso periodo
                        log.Debug("ClsAnalisi::GetCheckRifCatastali::nessuna query");
                        // valorizzo il commandText
                        cmdMyCommand.CommandText = "";
                        break;
                }
                myDaDati = new SqlDataAdapter(cmdMyCommand);
                log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDaDati.Fill(myResult);
                cmdMyCommand.Dispose();
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ClsAnalisi.GetCheckRifCatastali.errore: ", Err);
                myResult = new DataSet();
            }
            return myResult;
        }
        //*** ***
		#endregion
	}
}
