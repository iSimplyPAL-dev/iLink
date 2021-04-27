using CatastoInterface;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Utility;

namespace DichiarazioniICI
{
    /// <summary>
    /// Classe per l'aggiornamento della banca dati tributaria con i dati repertiti da flusso catastale.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class Catasto : DichiarazioniICI.Database.Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Catasto));
        /// <summary>
        /// 
        /// </summary>
        public class Fase
        {
            /// 
            public const int EstrazioneBancaDatiTrib = 1;
            /// 
            public const int UploadFile = 2;
            /// 
            public const int EstrazioneDichWork = 3;
            /// 
            public const int EstrazioneAnomalie = 4;
            /// 
            public const int Monitoring = 5;
            /// 
            public const int ImportDichCat = 6;
        }
        /// <summary>
        /// Variabili per le varie estrazioni da catasto
        /// </summary>
        /// <revisionHistory><revision date="02/04/2020">Si separano i dati catasto tra dati ui e dati titolarità per avere entrambe le date di inizio/fine</revision></revisionHistory>
        public class Anomalia
        {
            /// Titolari che non sono in soggetti
            public const int TitNoSog = 1;
            /// Soggetti che non sono in titolari
            public const int SogNoTit = 2;
            /// Titolari che non sono in UI
            public const int TitNoFab = 3;
            /// UI che non sono in titolari
            public const int FabNoTit = 4;
            /// Titolari senza possesso
            public const int NoPoss = 5;
            /// Soggetti che non sono hanno riferimenti
            public const int NoSogRif = 6;
            /// Titolari senza diritto
            public const int NoDiritto = 7;
            /// Flag Rurale
            public const int Rurale = 8;
            /// Catasto
            public const int Catasto = 9;
            /// Percentuale possesso
            public const int PerSoggetto = 10;
            /// Titotari che non sono in terreni
            public const int TitNoTer = 11;
            /// Terreni che non sono titolari
            public const int TerNoTit = 12;
            /// Solo dati dei fabbricati
            public const int CatastoUI = 13;
            /// Solo dati dei titolari
            public const int CatastoTit = 14;
        }
        //public class Anomalia
        //{
        //    /// Titolari che non sono in soggetti
        //    public const int TitNoSog = 1;
        //    /// Soggetti che non sono in titolari
        //    public const int SogNoTit = 2;
        //    /// Titolari che non sono in UI
        //    public const int TitNoFab = 3;
        //    /// UI che non sono in titolari
        //    public const int FabNoTit = 4;
        //    /// Titolari senza possesso
        //    public const int NoPoss = 5;
        //    /// Soggetti che non sono hanno riferimenti
        //    public const int NoSogRif = 6;
        //    /// Titolari senza diritto
        //    public const int NoDiritto = 7;
        //    /// Flag Rurale
        //    public const int Rurale = 8;
        //    /// Catasto
        //    public const int Catasto = 9;
        //    /// Percentuale possesso
        //    public const int PerSoggetto = 10;
        //    /// Titotari che non sono in terreni
        //    public const int TitNoTer = 11;
        //    /// Terreni che non sono titolari
        //    public const int TerNoTit = 12;
        //}
        #region "Banca Dati Tributaria"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="CodCatastale"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="myDati"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool SearchBancaDati(string IdEnte, string CodCatastale, DateTime Dal, DateTime Al, out DataTable myDati, out string sErr)
        {
            myDati = new DataTable();
            sErr = string.Empty;
            Elaborazione myElab = new Elaborazione();
            try
            {
                if (!CheckElabInCorso(CodCatastale, false, out myElab))
                {
                    myDati = GetBancaDatiTributaria(IdEnte, Dal, Al);
                }
                else
                {
                    sErr = "Elaborazione in corso. Impossibile proseguire.";
                    myDati = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.SearchBancaDati.errore: ", ex);
                return false;
            }
        }
        private DataTable GetBancaDatiTributaria(string IdEnte, DateTime Dal, DateTime Al)
        {
            DataTable dtDati = new DataTable();
            try
            {
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "prc_GetBancaDatiTributaria";
                myCommand.Parameters.Add("@IdEnte", SqlDbType.VarChar).Value = IdEnte;
                myCommand.Parameters.Add("@Dal", SqlDbType.DateTime).Value = Dal;
                myCommand.Parameters.Add("@Al", SqlDbType.DateTime).Value = Al;
                dtDati = Query(myCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.GetBancaDatiTributaria.errore: ", ex);
                dtDati = new DataTable();
            }
            return dtDati;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodCatastale"></param>
        /// <param name="ListFiles"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool UploadFiles(string CodCatastale, HttpFileCollection ListFiles, out string sErr)
        {
            sErr = string.Empty;
            int nUpload = 0;
            Elaborazione myElab = new Elaborazione();
            try
            {
                if (!CheckElabInCorso(CodCatastale, false, out myElab))
                {
                    if (ListFiles != null)
                    {
                        myElab.IDEnte = Business.ConstWrapper.CodiceEnte;
                        myElab.IDCatastale = CodCatastale;
                        if (myElab.InizioUpload.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                        {
                            myElab.InizioUpload = DateTime.Now;
                        }
                        new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO).SetElaborazione(myElab);
                        if (myElab.ID > 0)
                        {
                            foreach (string sFileName in ListFiles)//foreach (HttpPostedFile PostedFile in ListFiles)
                            {
                                try
                                {
                                    HttpPostedFile PostedFile = ListFiles[sFileName];
                                    if (PostedFile.ContentLength > 0)
                                    {
                                        if (PostedFile.FileName.ToUpper().Substring(0, 4) == CodCatastale &&
                                            (PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Fabbricati)
                                                || PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Storico)
                                                || PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Terreni)
                                                || PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Soggetti)
                                                || PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Titoli)
                                                || PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Dichiarazioni))
                                            )
                                        {
                                            PostedFile.SaveAs(Business.ConstWrapper.PathImportMotoreCatasto + PostedFile.FileName);
                                            nUpload += 1;
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.UploadFiles.errore: ", Ex);
                                    return false;
                                }
                            }
                            myElab.FineUpload = DateTime.Now;
                            if (nUpload <= 0)
                            {
                                sErr = "Nessun flusso caricato.";
                                myElab.EsitoUpload = Elaborazione.Esito.KO;
                            }
                            else
                            {
                                myElab.EsitoUpload = Elaborazione.Esito.OK;
                            }
                            new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO).SetElaborazione(myElab);
                        }
                        else
                        {
                            sErr = "Nessun flusso caricato.";
                        }
                    }
                    else
                    {
                        sErr = "Nessun flusso caricato.";
                    }
                }
                else
                {
                    sErr = "Elaborazione in corso. Impossibile proseguire.";
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.UploadFiles.errore: ", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodCatastale"></param>
        /// <param name="ListFiles"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool UploadRibaltaFiles(string CodCatastale, HttpFileCollection ListFiles, out string sErr)
        {
            sErr = string.Empty;
            int nUpload = 0;
            Elaborazione myElab = new Elaborazione();
            try
            {
                if (ListFiles != null)
                {
                    myElab.IDEnte = Business.ConstWrapper.CodiceEnte;
                    myElab.IDCatastale = CodCatastale;
                    if (myElab.InizioUpload.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                    {
                        myElab.InizioUpload = DateTime.Now;
                    }
                    new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection).SetElaborazione(myElab);
                    if (myElab.ID > 0)
                    {
                        foreach (HttpPostedFile PostedFile  in ListFiles)
                        {
                             try
                            {
                                if (PostedFile.ContentLength > 0)
                                {
                                    if (PostedFile.FileName.ToUpper().Substring(0, 4) == CodCatastale && PostedFile.FileName.ToUpper().EndsWith(CatastoInterface.ElaborazioneFile.Estensioni.Dichiarazioni))
                                    {
                                        PostedFile.SaveAs(Business.ConstWrapper.PathRibaltaMotoreCatasto + PostedFile.FileName);
                                        nUpload += 1;
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.UploadRibaltaFiles.errore: ", Ex);
                                return false;
                            }
                        }
                        myElab.FineUpload = DateTime.Now;
                        if (nUpload <= 0)
                        {
                            sErr = "Nessun flusso caricato.";
                            myElab.EsitoUpload = Elaborazione.Esito.KO;
                        }
                        else
                        {
                            myElab.EsitoUpload = Elaborazione.Esito.OK;
                        }
                        new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection).SetElaborazione(myElab);
                    }
                    else
                    {
                        sErr = "Nessun flusso caricato.";
                    }
                }
                else
                {
                    sErr = "Nessun flusso caricato.";
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.UploadRibaltaFiles.errore: ", ex);
                return false;
            }
        }
        #region "DichWork"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodCatastale"></param>
        /// <param name="IsEstraz"></param>
        /// <param name="myDati"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool SearchDichWork(string CodCatastale, bool IsEstraz, out DataTable myDati, out string sErr)
        {
            myDati = new DataTable();
            sErr = string.Empty;
            Elaborazione myElab = new Elaborazione();
            try
            {
                if (!CheckElabInCorso(CodCatastale, true, out myElab))
                {
                    myElab.InizioEstrazioneDichWork = DateTime.Now;
                    myDati = GetDichWork(myElab.ID);
                    myElab.FineEstrazioneDichWork = DateTime.Now;
                    if (myDati.Rows.Count <= 0)
                    {
                        sErr = "Nessun flusso estratto.";
                        myElab.EsitoEstrazioneDichWork = Elaborazione.Esito.KO;
                    }
                    else
                    {
                        myElab.EsitoEstrazioneDichWork = Elaborazione.Esito.OK;
                    }
                    if (IsEstraz)
                    {
                        new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO).SetElaborazione(myElab);
                    }
                }
                else
                {
                    sErr = "Elaborazione in corso. Impossibile proseguire.";
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.SearchDichWork.errore: ", ex);
                return false;
            }
        }
        private DataTable GetDichWork(int IdElaborazione)
        {
            DataTable dtDati = new DataTable();
            try
            {
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "prc_GetDichWork";
                myCommand.Parameters.Add("@IDELABORAZIONE", SqlDbType.Int).Value = IdElaborazione;
                dtDati = Query(myCommand, new SqlConnection(Business.ConstWrapper.StringConnectionCATASTO));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.GetDichWork.errore: ", ex);
                dtDati = new DataTable();
            }
            return dtDati;
        }
        #endregion
        #region "Anomalie"
        /// <summary>
        /// Funzione che estrae i dati dal catasto
        /// </summary>
        /// <param name="CodCatastale"></param>
        /// <param name="nType"></param>
        /// <param name="IsEstraz"></param>
        /// <param name="sFoglio"></param>
        /// <param name="sNumero"></param>
        /// <param name="sSubalterno"></param>
        /// <param name="myDati"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        /// <revisionHistory><revision date="02/04/2020">Si separano i dati catasto tra dati ui e dati titolarità per avere entrambe le date di inizio/fine</revision></revisionHistory>
        public bool SearchAnomalie(string CodCatastale, int nType, bool IsEstraz, string sFoglio, string sNumero, string sSubalterno, out DataTable myDati, out string sErr)
        {
            myDati = new DataTable();
            sErr = string.Empty;
            Elaborazione myElab = new Elaborazione();
            try
            {
                if (!CheckElabInCorso(CodCatastale, true, out myElab))
                {
                    switch (nType)
                    {
                        case Catasto.Anomalia.TitNoSog:
                            myElab.InizioEstrazioneTitVSSog = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneTitVSSog = DateTime.Now;
                            myElab.EsitoEstrazioneTitVSSog = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.SogNoTit:
                            myElab.InizioEstrazioneSogVSTit = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneSogVSTit = DateTime.Now;
                            myElab.EsitoEstrazioneSogVSTit = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.TitNoFab:
                            myElab.InizioEstrazioneTitVSFab = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneTitVSFab = DateTime.Now;
                            myElab.EsitoEstrazioneTitVSFab = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.FabNoTit:
                            myElab.InizioEstrazioneFabVSTit = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneFabVSTit = DateTime.Now;
                            myElab.EsitoEstrazioneFabVSTit = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.TitNoTer:
                            myElab.InizioEstrazioneTitVSTer = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneTitVSTer = DateTime.Now;
                            myElab.EsitoEstrazioneTitVSTer = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.TerNoTit:
                            myElab.InizioEstrazioneTerVSTit = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneTerVSTit = DateTime.Now;
                            myElab.EsitoEstrazioneTerVSTit = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.NoPoss:
                            myElab.InizioEstrazionePossMancante = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazionePossMancante = DateTime.Now;
                            myElab.EsitoEstrazionePossMancante = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.NoSogRif:
                            myElab.InizioEstrazioneComunioneMancante = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneComunioneMancante = DateTime.Now;
                            myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.NoDiritto:
                            myElab.InizioEstrazioneDirittoMancante = DateTime.Now;
                            myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
                            myElab.FineEstrazioneDirittoMancante = DateTime.Now;
                            myElab.EsitoEstrazioneDirittoMancante = Elaborazione.Esito.OK;
                            break;
                        case Catasto.Anomalia.Rurale:
                        case Catasto.Anomalia.Catasto:
                        case Catasto.Anomalia.PerSoggetto:
                        case Catasto.Anomalia.CatastoUI:
                        case Catasto.Anomalia.CatastoTit:
                            myDati = GetAnomalie(myElab.ID, CodCatastale, nType, sFoglio, sNumero, sSubalterno);
                            break;
                        default:
                            break;
                    }

                    if (IsEstraz)
                    {
                        new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO).SetElaborazione(myElab);
                    }
                }
                else
                {
                    sErr = "Elaborazione in corso. Impossibile proseguire.";
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.SearchAnomalie.errore: ", ex);
                return false;
            }
        }
        //public bool SearchAnomalie(string CodCatastale, int nType, bool IsEstraz, string sFoglio, string sNumero, string sSubalterno, out DataTable myDati, out string sErr)
        //{
        //    myDati = new DataTable();
        //    sErr = string.Empty;
        //    Elaborazione myElab = new Elaborazione();
        //    try
        //    {
        //        if (!CheckElabInCorso(CodCatastale, true, out myElab))
        //        {
        //            switch (nType)
        //            {
        //                case Catasto.Anomalia.TitNoSog:
        //                    myElab.InizioEstrazioneTitVSSog = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneTitVSSog = DateTime.Now;
        //                    myElab.EsitoEstrazioneTitVSSog = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.SogNoTit:
        //                    myElab.InizioEstrazioneSogVSTit = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneSogVSTit = DateTime.Now;
        //                    myElab.EsitoEstrazioneSogVSTit = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.TitNoFab:
        //                    myElab.InizioEstrazioneTitVSFab = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneTitVSFab = DateTime.Now;
        //                    myElab.EsitoEstrazioneTitVSFab = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.FabNoTit:
        //                    myElab.InizioEstrazioneFabVSTit = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneFabVSTit = DateTime.Now;
        //                    myElab.EsitoEstrazioneFabVSTit = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.TitNoTer:
        //                    myElab.InizioEstrazioneTitVSTer = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneTitVSTer = DateTime.Now;
        //                    myElab.EsitoEstrazioneTitVSTer = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.TerNoTit:
        //                    myElab.InizioEstrazioneTerVSTit = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneTerVSTit = DateTime.Now;
        //                    myElab.EsitoEstrazioneTerVSTit = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.NoPoss:
        //                    myElab.InizioEstrazionePossMancante = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazionePossMancante = DateTime.Now;
        //                    myElab.EsitoEstrazionePossMancante = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.NoSogRif:
        //                    myElab.InizioEstrazioneComunioneMancante = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneComunioneMancante = DateTime.Now;
        //                    myElab.EsitoEstrazioneComunioneMancante = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.NoDiritto:
        //                    myElab.InizioEstrazioneDirittoMancante = DateTime.Now;
        //                    myDati = GetAnomalie(myElab.ID, myElab.IDCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    myElab.FineEstrazioneDirittoMancante = DateTime.Now;
        //                    myElab.EsitoEstrazioneDirittoMancante = Elaborazione.Esito.OK;
        //                    break;
        //                case Catasto.Anomalia.Rurale:
        //                case Catasto.Anomalia.Catasto:
        //                case Catasto.Anomalia.PerSoggetto:
        //                    myDati = GetAnomalie(myElab.ID, CodCatastale, nType, sFoglio, sNumero, sSubalterno);
        //                    break;
        //                default:
        //                    break;
        //            }

        //            if (IsEstraz)
        //            {
        //                new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO).SetElaborazione(myElab);
        //            }
        //        }
        //        else
        //        {
        //            sErr = "Elaborazione in corso. Impossibile proseguire.";
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.SearchAnomalie.errore: ", ex);
        //        return false;
        //    }
        //}
        /// <summary>
        /// Funzione che preleva le anomalie per i riferimenti catastali dalle importazioni di catasto
        /// </summary>
        /// <param name="IdElaborazione"></param>
        /// <param name="IDCatastale"></param>
        /// <param name="nType"></param>
        /// <param name="sFoglio"></param>
        /// <param name="sNumero"></param>
        /// <param name="sSubalterno"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        private DataTable GetAnomalie(int IdElaborazione, string IDCatastale, int nType, string sFoglio, string sNumero, string sSubalterno)
        {
            DataTable dtDati = new DataTable();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetAnomalie", "IDELABORAZIONE", "IDCATASTALE", "Type", "FOGLIO", "NUMERO", "SUBALTERNO");
                    DataSet myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDELABORAZIONE", IdElaborazione)
                            , ctx.GetParam("IDCATASTALE", IDCatastale)
                            , ctx.GetParam("Type", nType)
                            , ctx.GetParam("FOGLIO", (nType == Anomalia.PerSoggetto ? sFoglio : sFoglio.PadLeft(4, char.Parse("0"))))
                            , ctx.GetParam("NUMERO", sNumero.PadLeft(5, char.Parse("0")))
                            , ctx.GetParam("SUBALTERNO", sSubalterno.PadLeft(4, char.Parse("0")))
                        );
                    dtDati = myDataSet.Tables[0];
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.GetAnomalie.errore: ", ex);
                dtDati = new DataTable();
            }
            return dtDati;
        }
        #endregion
        #region "Monitoraggio"
            /// <summary>
            /// 
            /// </summary>
            /// <param name="IdEnte"></param>
            /// <param name="IDElaborazione"></param>
            /// <param name="myDati"></param>
            /// <param name="sErr"></param>
            /// <returns></returns>
        public bool Monitoring(string IdEnte, int IDElaborazione, out DataTable myDati, out string sErr)
        {
            myDati = new DataTable();
            sErr = string.Empty;
            Elaborazione myElab = new Elaborazione();
            try
            {
                myDati = GetMonitoring(IdEnte,IDElaborazione);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.Monitoring.errore: ", ex);
                return false;
            }
        }
        private DataTable GetMonitoring(string IdEnte, int IdElaborazione)
        {
            DataTable dtDati = new DataTable();
            try
            {
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "prc_GetMonitoring";
                myCommand.Parameters.Add("@IdEnte", SqlDbType.VarChar).Value = IdEnte;
                myCommand.Parameters.Add("@IdElaborazione", SqlDbType.Int).Value = IdElaborazione;
                dtDati = Query(myCommand, new SqlConnection(Business.ConstWrapper.StringConnectionCATASTO));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.GetMonitoring.errore: ", ex);
                dtDati = new DataTable();
            }
            return dtDati;
        }
        #endregion
        /// <summary>
        /// se l'ultima fase è ok allora non ci sono elaborazioni in corso
        /// </summary>
        /// <param name="IdCatastale"></param>
        /// <param name="IsForExport"></param>
        /// <param name="myElab"></param>
        /// <returns></returns>
        public bool CheckElabInCorso(string IdCatastale, bool IsForExport, out Elaborazione myElab)
        {
            try
            {
                bool InCorso = true;
                myElab = new Elaborazione();
                List<Elaborazione> listElab = new Utility.DichManagerCatasto(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionCATASTO).LoadLastElaborazione(IdCatastale);
                if (listElab.Count > 0)
                {
                    foreach (Elaborazione myItem in listElab)
                    {
                        myElab = myItem;
                        if (!IsForExport)
                        {
                            if (myItem.InizioImport.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                            {
                                if (myItem.EsitoEstrazioneComunioneMancante == Elaborazione.Esito.OK)
                                {
                                    InCorso = false;
                                }
                            }
                            else
                            {
                                InCorso = false;
                            }
                        }
                        else {
                            if (myItem.InizioImport.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                            {
                                if (myItem.EsitoIncrocio.ToUpper() == CatastoInterface.Elaborazione.Esito.OK)
                                {
                                    InCorso = false;
                                }
                            }
                            else
                            {
                                InCorso = false;
                            }
                        }
                    }
                }
                else
                {
                    InCorso = false;
                }
                return InCorso;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Catasto.CheckElabInCorso.errore: ", ex);
                myElab = new Elaborazione();
                return true;
            }
        }
    }
}