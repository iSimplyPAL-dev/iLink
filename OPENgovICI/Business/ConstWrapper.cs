using System;
using System.Configuration;
using System.Web;
using log4net;

namespace Business
{
    /// <summary>
    /// Classe per la gestione delle variabili da sessione e da config
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ConstWrapper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConstWrapper));
        /// <summary>
        /// 
        /// </summary>
        public static int CountScript = 0;
        /// <summary>
        /// 
        /// </summary>
        public static int nTry = 0;
        /// <summary>
        /// Codice tributo per il modulo Anagrafica
        /// </summary>
        public static int CodiceTributoAnag
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["CodiceTributo"]);
             }
        }
        /// <summary>
        /// Codice tributo per il modulo
        /// </summary>
        public static string CodiceTributo
        {
            get
            {
                try
                {
                    return Utility.Costanti.TRIBUTO_ICI;// "8852";
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.CodiceTributo.errore: ", Err);
                    throw Err;
                }
            }


        }
        /// <summary>
        /// 
        /// </summary>
        public static string Ambiente
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["Ambiente"] == null)
                        return "";
                    else
                        return HttpContext.Current.Session["Ambiente"].ToString();
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.Ambiente.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Codice Ente per il modulo Anagrafica
        /// </summary>
        public static string CodiceEnte
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["COD_ENTE"] != null)
                        return (HttpContext.Current.Session["COD_ENTE"]).ToString();
                    else
                        return "";
                    //return (ConfigurationManager.AppSettings["CodiceEnte"]).ToString();
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.CodiceEnte.errore: ", Err);
                    throw Err;
                }
            }
        }

        /// <summary>
        /// Descrizione Ente per il modulo Anagrafica
        /// </summary>
        public static string DescrizioneEnte
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["DESCRIZIONE_ENTE"] != null)
                        return (HttpContext.Current.Session["DESCRIZIONE_ENTE"]).ToString();
                    else
                        return "";
                    //return (ConfigurationManager.AppSettings["CodiceEnte"]).ToString();
                }

                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.DescrizioneEnte.errore: ", Err);
                    throw Err;
                }
            }
        }

        /// <summary>
        /// Ritorna il nome utente che ha effettuato il login. 
        /// </summary>
        public static string sUsername
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["username"] != null)
                    {
                        return HttpContext.Current.Session["username"].ToString();
                    }
                    else { return ConfigurationManager.AppSettings["UserFramework"].ToString(); }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.sUsername.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Indica se si arriva dalle funzioni sovracomunali
        /// </summary>
        public static string FromSovracomunali
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["FromSovracomunali"] != null)
                        return (HttpContext.Current.Session["FromSovracomunali"]).ToString();
                    else
                        return "";
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.FromSovracomunali.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Ritorna true o false a seconda che l'applicativo deve usare anater o meno. 
        /// </summary>
        public static string UsoAnater
        {
            get
            {
                return ConfigurationManager.AppSettings["USO_ANATER"].ToString();
            }
        }
        /// <summary>
        /// Ritorna il codice belfiore dell'ente in questione. 
        /// </summary>
        public static string CodBelfiore
        {
            get
            {
                return Utility.StringOperation.FormatString( HttpContext.Current.Session["COD_BELFIORE"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static int ProvenienzaVersamentoRibaltato
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PROVENIENZA_VERS_RIBALTATO"].ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool CancellazioneFisicaVersamenti
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["CancellazioneFisicaVersamenti"] != null)
                    {
                        return bool.Parse(ConfigurationManager.AppSettings["CancellazioneFisicaVersamenti"].ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.CancellazioneFisicaVersamenti.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static Boolean VisualDOCFA
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["VisualDOCFA"] != null)
                    {
                        return bool.Parse(ConfigurationManager.AppSettings["VisualDOCFA"].ToString());
                    }
                    else { return false; }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.VisualDOCFA.errore: ", Err);
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
                try
                {
                    if (ConfigurationManager.AppSettings["HasDummyDich"] != null)
                    {
                        return bool.Parse(ConfigurationManager.AppSettings["HasDummyDich"].ToString());
                    }
                    else { return false; }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.HasDummyDich.errore: ", Err);
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
                try
                {
                    if (ConfigurationManager.AppSettings["HasPlainAnag"] != null)
                        return bool.Parse(ConfigurationManager.AppSettings["HasPlainAnag"].ToString());
                    else
                        return false;
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.HasPlainAnag.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string IsFromVariabile
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["IsFromVariabile"] == null)
                    {
                        HttpContext.Current.Session["IsFromVariabile"] = "0";
                        if (Ambiente == "CMGC")
                            HttpContext.Current.Session["IsFromVariabile"] = "1";

                    }
                    else
                    {
                        if (HttpContext.Current.Session["IsFromVariabile"].ToString() == "")
                            HttpContext.Current.Session["IsFromVariabile"] = "0";
                        if (Ambiente == "CMGC")
                            HttpContext.Current.Session["IsFromVariabile"] = "1";
                    }
                    log.Debug("IsFromVariabile=" + HttpContext.Current.Session["IsFromVariabile"].ToString());
                }
                catch (Exception ex)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.IsFromVariabile.errore: ", ex);
                    HttpContext.Current.Session["IsFromVariabile"] = "0";
                    if (Ambiente == "CMGC")
                        HttpContext.Current.Session["IsFromVariabile"] = "1";
                }
                return HttpContext.Current.Session["IsFromVariabile"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
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
                    log.Debug("DichiarazioniICI.ConstWrapper.NDocPerFile.errore: ", Err);
                    throw Err;
                }
            }
        }
        #region "Parametri WorkFlow"
        /// <summary>
        /// Property per ricavare il parametro env per il framework ribes
        /// </summary>
        public static string ParametroENV
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["PARAMETROENV"] != null)
                        return HttpContext.Current.Session["PARAMETROENV"].ToString();
                    else
                        return ConfigurationManager.AppSettings["ParametroENV"];
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.ParametroENV.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Codice tributo per il modulo Anagrafica
        /// </summary>
        public static string UserNameFramework
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["username"] != null)
                        return HttpContext.Current.Session["username"].ToString();
                    else
                        return ConfigurationManager.AppSettings["UserFramework"].ToString();
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.UserNameFramework.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Property per ricavare l'identificativo dell'applicazione
        /// </summary>
        public static string ParametroAPPLICATION
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["OPENGOVI"] != null)
                        return ConfigurationManager.AppSettings["OPENGOVI"].ToString();
                    else
                        return "OPENGOVI";
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.ParametroAPPLICATION.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Property per ricavare l'identificativo dell'applicazione UTILITA
        /// </summary>
        public static string ParametroUTILITA
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ParametroUTILITA"]; }
        }
        /// <summary>
        /// Codice della sottoaplicazione Anagrafica
        /// </summary>
        public static string ParametroAnagrafica
        {
            get
            {
                return ConfigurationManager.AppSettings["ParametroAnagrafica"];
            }
        }
        #endregion
        #region "Url Servizi"
        /// <summary>
        /// 
        /// </summary>
        public static string UrlStradario
        {
            get { return ConfigurationManager.AppSettings["UrlPopUpStradario"] != null ? ConfigurationManager.AppSettings["UrlPopUpStradario"].ToString() : string.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string UrlServizioCalcoloICI
        {
            get
            {
                return ConfigurationManager.AppSettings["URLServiziFreezer"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string UrlServizioElaborazioneDocumentiICI
        {
            get
            {
                return ConfigurationManager.AppSettings["URLElaborazioneDatiStampeICI"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string URLServizioStampe
        {
            get
            {
                return ConfigurationManager.AppSettings["ServizioStampe"].ToString();
            }
        }
        /// <summary>
        /// Parametro con l'url di accesso al servizio ImexService
        /// </summary>
        public static string ImexServiceAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["ImexServiceAddress"];
            }
        }
        /// <summary>
        /// Parametro con l'url di accesso al servizio ImexService per la Bonifica
        /// </summary>
        public static string BonificaServiceAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["BonificaServiceAddress"];
            }
        }
        #endregion
        #region "Path"
        /// <summary>
        /// Ritorna il nome del file compreso di path dell'eseguibile Imex per le importazioni
        /// </summary>
        public static string ImexPath
        {
            get
            {
                return ConfigurationManager.AppSettings["Imex"].ToString();
            }
        }
        /// <summary>
        /// Ritorna il nome del file compreso di path dell'eseguibile bonifica per la bonifica automatica
        /// </summary>
        public static string BonificaPath
        {
            get
            {
                return ConfigurationManager.AppSettings["Bonifica"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string Path
        {
            get
            {
                try
                {
                    //if (HttpContext.Current.Session["path"]!=null)
                    //{
                    //    return HttpContext.Current.Session["path"].ToString();
                    //}
                    //else {
                    return ConfigurationManager.AppSettings["NOME_SITO"] + ConfigurationManager.AppSettings["PATH_OPENGOVI"].ToString();
                    //}

                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.Path.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathPDF_DOCFA
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["HTTP_PDF_TARSU"] != null)
                    {
                        return ConfigurationManager.AppSettings["HTTP_PDF_TARSU"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathPDF_DOCFA.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathImportMotoreCatasto
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["PathImportMotoreCatasto"] != null)
                    {
                        return ConfigurationManager.AppSettings["PathImportMotoreCatasto"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathImportMotoreCatasto.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathRibaltaMotoreCatasto
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["PathRibaltaMotoreCatasto"] != null)
                    {
                        return ConfigurationManager.AppSettings["PathRibaltaMotoreCatasto"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathRibaltaMotoreCatasto.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Percorso del file batch che sposta i file dal server del sito al server del motore catasto
        /// </summary>
        public static string PathCatastoBatchFile
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["PathCatastoBatchFile"] != null)
                    {
                        return ConfigurationManager.AppSettings["PathCatastoBatchFile"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathCatastoBatchFile.errore: ", Err);
                    throw Err;
                }
            }
        }
        #endregion
        #region "Stringhe di connessione"
        /// <summary>
        /// 
        /// </summary>
        public static string DBType
        {
            get
            {
                return "SQL";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnection
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["connectionStringOpenGovICI"] != null)
                    {
                        return ConfigurationManager.AppSettings["connectionStringOpenGovICI"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.StringConnection.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionOPENgov
        {
            get
            {
                try
                {
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
                    log.Debug("DichiarazioniICI.ConstWrapper.StringConnectionOPENgov.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionAnagrafica
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"] != null)
                    {
                        return ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.StringConnectionAnagrafica.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionProvvedimenti
        {

            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["connectionStringOPENgovPROVVEDIMENTI"] != null)
                    {
                        return ConfigurationManager.AppSettings["connectionStringOPENgovPROVVEDIMENTI"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.StringConnectionProvvedimenti.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionTARSU
        {
            get
            {
                try
                {
                    if (IsFromVariabile == "1")
                        return ConfigurationManager.AppSettings["connectionStringOpenGovTIA"].ToString();
                    else
                        return ConfigurationManager.AppSettings["connectionStringOpenGovTARSU"].ToString();
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.StringConnectionTARSU.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionCATASTO
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["connectionStringCATASTO"] != null)
                    {
                        return ConfigurationManager.AppSettings["connectionStringCATASTO"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.StringConnectionCATASTO.errore: ", Err);
                    throw Err;
                }
            }
        }
        #endregion
        #region "GIS"
        /// <summary>
        /// 
        /// </summary>
        public static Boolean VisualGIS
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["VisualGIS"] != null)
                    {
                        log.Debug("Session(VisualGIS)::" + HttpContext.Current.Session["VisualGIS"].ToString());
                        return bool.Parse(HttpContext.Current.Session["VisualGIS"].ToString());
                    }
                    else { return false; }


                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.VisualGIS.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string UrlWSGIS
        {
            get
            {
                try
                {
                    if ((ConfigurationManager.AppSettings["UrlWSGIS"] == null))
                    {
                        return "http://gis.oikosweb.com/CATWS/Gismappale.asmx";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["UrlWSGIS"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.UrlWSGIS.errore: ", Err);

                    return "http://gis.oikosweb.com/CATWS/Gismappale.asmx";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string UrlWebGIS
        {
            get
            {
                try
                {
                    if ((ConfigurationManager.AppSettings["UrlWebGIS"] == null))
                    {
                        return "http://map.portalecomuni.net/mapguide/wgis/ddd.html?&GisGuidMap=";
                    }
                    else
                    {
                        if (!ConfigurationManager.AppSettings["UrlWebGIS"].ToString().EndsWith("&GisGuidMap="))
                            return ConfigurationManager.AppSettings["UrlWebGIS"].ToString() + "&GisGuidMap=";
                        else
                            return ConfigurationManager.AppSettings["UrlWebGIS"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.UrlWebGIS.errore: ", Err);
                    return "http://map.portalecomuni.net/mapguide/wgis/ddd.html?&GisGuidMap=";
                }
            }
        }
        #endregion
        #region "Cartelle altri moduli"
        /// <summary>
        /// 
        /// </summary>
        public static string Path_TARSU
        {
            get
            {
                try
                {
                    if ((ConfigurationManager.AppSettings["PATH_OPENGOVTIA"] == null))
                    {
                        return "/OPENgovTIA";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["PATH_OPENGOVTIA"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.Path_TARSU.errore: ", Err);
                    return "/OPENgovTIA";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathProspetti
        {
            get
            {
                try
                {
                    if ((ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"] == null))
                    {
                        return "";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathProspetti.errore: ", Err);
                    return "";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathStampe
        {
            get
            {
                try
                {
                    if ((ConfigurationManager.AppSettings["DIRTEMPLATE"] == null))
                    {
                        return "";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["DIRTEMPLATE"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathStampe.errore: ", Err);
                    return "";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathVirtualStampe
        {
            get
            {
                try
                {
                    if ((ConfigurationManager.AppSettings["DIRTEMPLATEVIRTUAL"] == null))
                    {
                        return "";
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["DIRTEMPLATEVIRTUAL"].ToString();
                    }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.PathVirtualStampe.errore: ", Err);
                    return "";
                }
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public static string NameSistemaTerritorio
        {
            get
            {
                return Utility.StringOperation.FormatString(ConfigurationManager.AppSettings["NameSistemaTerritorio"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool ConfigurazioneDichiarazione
        {
            get
            {
                try
                {
                    return bool.Parse(ConfigurationManager.AppSettings["CONFIGURAZIONE_DICHIARAZIONE"].ToString());
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Interfaccia verso il parametro di configuraziome omonimo
        /// </summary>
        public static string ContoCorrente
        {
            get
            {
                try
                {
                    OPENUtility.ClsContiCorrenti FncCC = new OPENUtility.ClsContiCorrenti();
                    //return ConfigurationManager.AppSettings["ContoCorrente"];
                    if (HttpContext.Current.Session["CONTO_CORRENTE"] != null)
                        return HttpContext.Current.Session["CONTO_CORRENTE"].ToString();
                    else
                        return FncCC.GetContoCorrente(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodiceTributo, Business.ConstWrapper.sUsername, Business.ConstWrapper.StringConnectionOPENgov).ContoCorrente;
                }
                catch (Exception Err)
                {
                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.ContoCorrente.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Interfaccia verso il parametro di configuraziome omonimo
        /// </summary>
        public static string IntestatarioBollettino
        {
            get
            {
                try
                {
                    OPENUtility.ClsContiCorrenti FncCC = new OPENUtility.ClsContiCorrenti();
                    //return ConfigurationManager.AppSettings["IntestatarioBollettino"];
                    if (HttpContext.Current.Session["INTESTAZIONE_BOLLETTINO"] != null)
                        return HttpContext.Current.Session["INTESTAZIONE_BOLLETTINO"].ToString();
                    else
                        return FncCC.GetContoCorrente(Business.ConstWrapper.CodiceEnte, Business.ConstWrapper.CodiceTributo, Business.ConstWrapper.sUsername, Business.ConstWrapper.StringConnectionOPENgov).Intestazione_1;
                }
                catch (Exception Err)
                {
                   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.IntestatarioBollettino.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Interfaccia verso il parametro di configuraziome omonimo
        /// </summary>
        public static string ComuneUbicazioneImmobile
        {
            get
            {
                try
                {
                    //return ConfigurationManager.AppSettings["ComuneUbicazioneImmobile"];
                    if (HttpContext.Current.Session["COMUNE_UBICAZIONE_IMMOBILE"] != null)
                        return HttpContext.Current.Session["COMUNE_UBICAZIONE_IMMOBILE"].ToString();
                    else
                        return ConstWrapper.DescrizioneEnte;
                }
                catch (Exception Err)
                {
                  log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ApplicationHelper.ComuneUbicazioneImmobile.errore: ", Err);
                    throw Err;
                }
            }
        }
        /// <summary>
        /// Wrapping della session per la gestione dei tipi e della validità.
        /// </summary>
        public static System.Collections.Hashtable Parametri
        {
            /*DIPE 29/11/2007
			Modificato il nome della session da SEARCHPARAMETRES a SEARCHPARAMETRES1
			in quanto andava in conflitto con il nome della sessione usata in anagrafica
			la quale modificando i valori all'interno, generava un errore quando
			si caricavano i dati dalla session nell'applicazione che l'aveva creata
			in precedenza
			*/
            get
            {
                return (System.Web.HttpContext.Current.Session["SEARCHPARAMETRES1"] != null ? (System.Collections.Hashtable)System.Web.HttpContext.Current.Session["SEARCHPARAMETRES1"] : null);
            }

            set
            {
                System.Web.HttpContext.Current.Session["SEARCHPARAMETRES1"] = value;
            }
        }
        /// <summary>
        /// Ritorna se si è stati chiamati da sportello. 
        /// </summary>
        public static string Sportello
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["Sportello"] != null)
                    {
                        return HttpContext.Current.Session["Sportello"].ToString();
                    }
                    else { return string.Empty; }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.Sportello.errore: ", Err);
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Ritorna l'utente di sportello. 
        /// </summary>
        public static string SportelloUser
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["SportelloUser"] != null)
                    {
                        return ConfigurationManager.AppSettings["SportelloUser"].ToString();
                    }
                    else { return "USportello"; }
                }
                catch (Exception Err)
                {
                    log.Debug("DichiarazioniICI.ConstWrapper.SportelloUsername.errore: ", Err);
                    return "USportello";
                }
            }
        }
    }
}