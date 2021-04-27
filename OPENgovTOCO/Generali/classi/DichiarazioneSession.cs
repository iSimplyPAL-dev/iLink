using Microsoft.VisualBasic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;
using OPENUtility;
using log4net;
using DTO;
using IRemInterfaceOSAP;

namespace OPENgovTOCO
{
    /// <summary>
    /// Classe per la gestione delle variabili da sessione e da config
    /// </summary>
    public class DichiarazioneSession
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioneSession));
        public static int CountScript = 0;
        //public static string TRIBUTOOccupazionePermanente
        //{
        //    get { return "3931"; }
        //}
        //public static string TRIBUTOOccupazioneTemporanea
        //{
        //    get { return "3932"; }
        //}
        //public static string TRIBUTOOSAP
        //{
        //    get { return "0453"; }
        //}
        //public static string TRIBUTOScuole
        //{
        //    get { return "9253"; }
        //}
        //ARTCAVA: Controlla il valore risultante qui sotto deve valere "1900-01-01"
        public static DateTime MyDateMinValue
        {
            get
            {
                DateTime _defaultDate = DateTime.MinValue.AddYears(1899);
                return _defaultDate;
            }
        }
        //*** ***

        public static DichiarazioneTosapCosap SessionDichiarazioneTosapCosap
        {
            get { return (DichiarazioneTosapCosap)HttpContext.Current.Session["objDichiarazioneTosapCosap"]; }
            set { HttpContext.Current.Session["objDichiarazioneTosapCosap"] = value; }
        }
        public static string DBType
        {
            get
            {
                return "SQL";
            }
        }
        public static string StringConnection
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["StringConnection"] == null)
                    {
                        //						CreateSessione WFSession;
                        //						string WfErrore = "";
                        //						WFSession = new CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(), HttpContext.Current.Session["username"].ToString(), HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"].ToString());
                        //						if (! WFSession.CreaSessione(HttpContext.Current.Session["username"].ToString() , ref WfErrore))
                        //						{
                        //							Log.Debug("DichiarazioneSession::StringConnection::si è verificato il seguente errore::Errore durante l'apertura della sessione di WorkFlow");
                        //							throw (new Exception("Errore durante l\'apertura della sessione di WorkFlow"));
                        //						}
                        //						HttpContext.Current.Session["StringConnection"] = WFSession.oSession.oAppDB.GetConnection().ConnectionString;// + ";Connection Timeout=600";
                        return ConfigurationManager.AppSettings["connectionStringOpenGovOSAP"].ToString();
                    }
                    else
                    {
                        HttpContext.Current.Session["StringConnection"] = ConfigurationManager.AppSettings["connectionStringOpenGovOSAP"].ToString();
                        return HttpContext.Current.Session["StringConnection"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.StringConnection.errore: ", Err);
                    return ConfigurationManager.AppSettings["connectionStringOpenGovOSAP"].ToString();
                }
            }
        }

        public static string StringConnectionAnagrafica
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["StringConnectionAnagrafica"] == null)
                    {
                        return ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"].ToString();
                    }
                    else
                    {
                        HttpContext.Current.Session["StringConnectionAnagrafica"] = ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"].ToString();
                        return HttpContext.Current.Session["StringConnectionAnagrafica"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.StringConnectionAnagrafica.errore: ", Err);

                    return ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"].ToString();
                }
            }
        }
        public static string StringConnectionOPENgov
        {
            
            get
            {
                try {
                    if (ConfigurationManager.AppSettings["connectionStringSQLOPENgov"] != null)
                    {
                        return ConfigurationManager.AppSettings["connectionStringSQLOPENgov"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.StringConnectionOPENgov.errore: ", Err);
                    throw Err;
                    
                }
            }
        }

        public static string IdEnte
        {
            get
            {
                try {
                    if (HttpContext.Current.Session["COD_ENTE"].ToString() != null)
                        return HttpContext.Current.Session["COD_ENTE"].ToString();
                    else
                        return "050027";
                }
                catch (Exception Err)
                {
                    Log.Debug( " - OPENgovOSAP.DichiarazioneSession.IdEnte.errore: ", Err);

                    return ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"].ToString();
                }
            }
        }

        public static string DescrizioneEnte
        {
            get
            {
                try {
                    if (HttpContext.Current.Session["DESCRIZIONE_ENTE"].ToString() != null)
                        return HttpContext.Current.Session["DESCRIZIONE_ENTE"].ToString();
                    else
                        return "Comune di Pomarance";
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.DescrizioneEnte.errore: ", Err);

                    throw Err;
                }
            }

        }

        public static string CodTributo(string myTributo)
        {
            try {
                if (myTributo == string.Empty && (HttpContext.Current.Session["COD_TRIBUTO"] == null || HttpContext.Current.Session["COD_TRIBUTO"].ToString() ==string.Empty))
                    HttpContext.Current.Session["COD_TRIBUTO"] = Utility.Costanti.TRIBUTO_OSAP;
                else if (myTributo != string.Empty)
                    HttpContext.Current.Session["COD_TRIBUTO"] = myTributo;
                return HttpContext.Current.Session["COD_TRIBUTO"].ToString();
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgovOSAP.DichiarazioneSession.CodTributo.errore: ", Err);
                throw Err;
            }
        }

        public static string StileStradario
        {
            get { return HttpContext.Current.Session["StileStradario"].ToString(); }
        }
        public static string UrlStradario
        {
            get { return ConfigurationManager.AppSettings["UrlPopUpStradario"]!=null? ConfigurationManager.AppSettings["UrlPopUpStradario"].ToString():string.Empty; }
        }

        public static string sOperatore
        {
            get
            {
                try {
                    if (HttpContext.Current.Session["username"] != null)
                        return HttpContext.Current.Session["username"].ToString();
                    else
                        return "USER";
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.sOperatore.errore: ", Err);
                    throw Err;
                    
                }
            }
        }

        //public static string ApplAnagrafica
        //{
        //    get
        //    {
        //        try {
        //            if (HttpContext.Current.Session["Anagrafica"] != null)
        //                return HttpContext.Current.Session["Anagrafica"].ToString();
        //            else
        //                return "OPENGOVA";
        //        }
        //        catch (Exception Err)
        //        {
        //            Log.Debug("OPENgovOSAP.DichiarazioneSession.ApplAnagrafica.errore: ", Err);

        //            throw Err;
        //        }
        //    }
        //}
        public static string UrlServizioStampeICI
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["URLElaborazioneDatiStampeICI"] == null)
                        return "tcp://192.168.14.187:33446/ElaborazioneStampeICI.rem";
                    else
                        return ConfigurationManager.AppSettings["URLElaborazioneDatiStampeICI"].ToString();
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.UrlServizioStampeICI.errore: ", Err);
                    return "tcp://192.168.14.187:33446/ElaborazioneStampeICI.rem";
                }
            }
        }
        public static string UrlMotoreOSAP
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["UrlMotoreTOCO"] == null)
                        return "";
                    else
                        return ConfigurationManager.AppSettings["UrlMotoreTOCO"].ToString();
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.UrlMotoreOSAP.errore: ", Err);
                    return "";
                }
            }
        }

        public static string PathImport
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["PATH_ACQUISIZIONE"] == null)
                        return "";
                    else
                        return ConfigurationManager.AppSettings["PATH_ACQUISIZIONE"].ToString();
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.PathImport.errore: ", Err);
                    return "";
                }
            }
        }
        public static string PathStampe
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["DIRTEMPLATE"] == null)
                        return "";
                    else
                        return ConfigurationManager.AppSettings["DIRTEMPLATE"].ToString();
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.PathStampe.errore: ", Err);

                    return "";
                }
            }
        }
        public static string PathVirtualStampe
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["DIRTEMPLATEVIRTUAL"] == null)
                        return "";
                    else
                        return ConfigurationManager.AppSettings["DIRTEMPLATEVIRTUAL"].ToString();
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.PathVirtualStampe.errore: ", Err);

                    return "";
                }
            }
        }
        public static int nMaxDocPerFile
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["NDocPerFile"] != null)
                        return int.Parse(ConfigurationManager.AppSettings["NDocPerFile"].ToString());
                    else
                        return 1;
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.NDocPerFile.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// se sono a true vuol dire che ho:
        /// - la gestione delle dichiarazioni fittizie
        /// - i parametri di ricerca dichiarazioni completi
        /// - il calcolo della parte variabile solo se NCPV>0 anziché FLAG_FORZA_CALCOLO_PV
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean HasDummyDich
        {
            get
            {
                try {
                    if (ConfigurationManager.AppSettings["HasDummyDich"] != null)
                    {
                        return bool.Parse(ConfigurationManager.AppSettings["HasDummyDich"].ToString());
                    }
                    else { return false; }
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.HasDummyDich.errore: ", Err);

                    throw Err;
                }
            }
        }
        /// <summary>
        /// se sono a true vuol dire che ho un unico pannello di visualizzazione anagrafica incluso nelle varie pagine
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean HasPlainAnag
        {
            get
            {
                try {
                    if (ConfigurationManager.AppSettings["HasPlainAnag"] != null)
                        return bool.Parse(ConfigurationManager.AppSettings["HasPlainAnag"].ToString());
                    else
                        return false;
                }
                catch (Exception Err)
                {
                    Log.Debug("OPENgovOSAP.DichiarazioneSession.HasPlainAnag.errore: ", Err);

                    throw Err;
                }
            }
        }
    }
}
