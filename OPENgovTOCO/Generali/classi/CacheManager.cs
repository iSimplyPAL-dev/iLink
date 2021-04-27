using System;
using System.Web.Caching;
using System.Web;
using log4net;

namespace OPENgovTOCO
{
    /// <summary>
    /// Classe per la gestione cache dell'elaborazione asincrona
    /// </summary>
    public class CacheManager
    {
        private static System.Web.Caching.Cache IISCache = HttpRuntime.Cache;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CacheManager));

        private CacheManager()
        {
        }

        #region Calcolo massivo
        private static string CalcoloMassivoInCorsoKey = "OSAPAnnoCalcoloMassivoInCorso";
        public static int GetCalcoloMassivoInCorso()
        {
            try
            {
                if (IISCache[CalcoloMassivoInCorsoKey] != null)
                    return (int)IISCache[CalcoloMassivoInCorsoKey];
                else
                    return (-1);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CacheManager.GetCalcoloMassivoInCorso.errore: ", Err);
                throw Err;
            }
        }
        public static void SetCalcoloMassivoInCorso(int Anno)
        {
            try
            {
                IISCache.Insert(CalcoloMassivoInCorsoKey, Anno, null,
                    Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, null);
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgovOSAP.CacheManager.SetCalcoloMassivoInCorso.errore: ", Err);

            }
        }
        public static void RemoveCalcoloMassivoInCorso()
        {
            IISCache.Remove(CalcoloMassivoInCorsoKey);
        }
        /**** 201810 - Calcolo Puntuale ****/
        private static string RiepilogoCalcoloMassivoKey = "OSAPRiepilogoCalcoloMassivo";
        public static IRemInterfaceOSAP.ElaborazioneEffettuata GetRiepilogoCalcoloMassivo()
        {
            if (IISCache[RiepilogoCalcoloMassivoKey] != null)
            {
                return (IRemInterfaceOSAP.ElaborazioneEffettuata)(IISCache[RiepilogoCalcoloMassivoKey]);
            }
            else {
                return null;
            }
        }
        public static void SetRiepilogoCalcoloMassivo(IRemInterfaceOSAP.ElaborazioneEffettuata myRuolo)
        {
            IISCache.Insert(RiepilogoCalcoloMassivoKey, myRuolo, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }
        public static void RemoveRiepilogoCalcoloMassivo()
        {
            IISCache.Remove(RiepilogoCalcoloMassivoKey);
        }
        private static string AvvisiCalcoloMassivoKey = "OSAPAvvisiCalcoloMassivo";
        public static IRemInterfaceOSAP.Cartella[] GetAvvisiCalcoloMassivo()
        {
            if (IISCache[AvvisiCalcoloMassivoKey] != null)
            {
                return (IRemInterfaceOSAP.Cartella[])(IISCache[AvvisiCalcoloMassivoKey]);
            }
            else {
                return null;
            }
        }
        public static void SetAvvisiCalcoloMassivo(IRemInterfaceOSAP.Cartella[] myRuolo)
        {
            IISCache.Insert(AvvisiCalcoloMassivoKey, myRuolo, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }
        public static void RemoveAvvisiCalcoloMassivo()
        {
            IISCache.Remove(AvvisiCalcoloMassivoKey);
        }
        /**** ****/
        #endregion Calcolo massivo
        #region Avanzamento Elaborazione
        private static string AvanzamentoElaborazioneKey = "OSAPAvanzamentoElaborazione";
        public static string GetAvanzamentoElaborazione()
        {
            try
            {
                if (IISCache[AvanzamentoElaborazioneKey] != null)
                    return IISCache[AvanzamentoElaborazioneKey].ToString();
                else
                    return null;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CacheManager.GetAvanzamentoElaborazione.errore: ", Err);
                throw Err;
            }
        }
        public static void SetAvanzamentoElaborazione(string sMyDati)
        {
            try
            {
                IISCache.Insert(AvanzamentoElaborazioneKey, sMyDati, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            catch (Exception Err)
            {
                Log.Debug("OPENgovOSAP.CacheManager.SetAvanzamentoElaborazione.errore: ", Err);

            }
        }
        public static void RemoveAvanzamentoElaborazione()
        {
            IISCache.Remove(AvanzamentoElaborazioneKey);
        }
        #endregion
        #region Import flussi
        private static string ImportInCorsoKey = "OSAPImportInCorso";

        public static string GetImportInCorso()
        {
            try
            {
                if (IISCache[ImportInCorsoKey] != null)
                    return (string)IISCache[ImportInCorsoKey];
                else
                    return ("");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CacheManager.GetImportInCorso.errore: ", Err);
                throw Err;

            }
        }
        public static void SetImportInCorso(string myNomeFile)
        {
            try
            {
                IISCache.Insert(ImportInCorsoKey, myNomeFile, null,
                    Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, null);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CacheManager.SetImportoInCorso.errore: ", Err);

            }
        }
        public static void RemoveImportInCorso()
        {
            IISCache.Remove(ImportInCorsoKey);
        }
        #endregion
    }
}
