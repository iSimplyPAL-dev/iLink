using System;
using System.Threading;
using System.Collections;
using System.Configuration;
using DAO;
using DTO;
using IRemInterfaceOSAP;
using log4net;
using System.Data.SqlClient;
using AnagInterface;

namespace OPENgovTOCO
{
    /// <summary>
    /// classe per la gestione del calcolo del ruolo.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Analisi eventi</em>
    /// </revision>
    /// </revisionHistory>
    public class CalcoloOSAP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CalcoloOSAP));
        private int _Anno;
        private Categorie[] _Categ;
        private TipologieOccupazioni[] _TipiOcc;
        private Agevolazione[] _Agev;
        private Tariffe[] _Tarif;
        private string _IdEnte;
        private string _Operatore;
        private string _CodTributo;
        private double _ImportoRate;
        private string _ListOccupazioni = "";
        /*
		private DBEngine _dbEngine;
		private DBEngine _dbEngineNoTrans;
        */

        //private SqlCommand connTrans;
        private SqlCommand connCmd;

        //*** 20130610 - ruolo supplettivo ***
        private Ruolo.E_TIPO _TipoRuolo = Ruolo.E_TIPO.ORDINARIO;
        private int _IdFlusso;
        //*** ***
        /**** 201810 - Calcolo Puntuale ****/
        private bool _IsSimula = false;
        private int _IDDichiarazione;

        public CalcoloOSAP()
        {
        }

        //*** 20130610 - ruolo supplettivo ***
        public void StartCalcolo(Ruolo.E_TIPO TipoRuolo, int Anno, Categorie[] categ, TipologieOccupazioni[] tipiOcc, Agevolazione[] agev, Tariffe[] tarif, string IdEnte, string Operatore, string CodTributo, double ImportoRate, int IdFlussoPrec, string ListOccupazioni)
        //public void StartCalcolo (int Anno, Categorie[] categ, TipologieOccupazioni[] tipiOcc, Agevolazione[] agev, Tariffe[] tarif, string IdEnte, string CodTributo, double ImportoRate)
        {
            try
            {
                ThreadStart threadDelegate = new ThreadStart(this.StartCalcoloThreadEntryPoint);
                Thread t = new Thread(threadDelegate);

                _Anno = Anno;
                _Categ = categ;
                _TipiOcc = tipiOcc;
                _Agev = agev;
                _Tarif = tarif;
                _IdEnte = IdEnte;
                _Operatore = Operatore;
                _CodTributo = CodTributo;
                _ImportoRate = ImportoRate;
                _TipoRuolo = TipoRuolo;
                _IdFlusso = IdFlussoPrec;
                _ListOccupazioni = ListOccupazioni;

                // Inizio la transazione e recupero l'istanza della connessione al db
                // Devo farlo qui visto che sto eseguendo codice ancora nel thread
                // della PostBack da FE, per cui ho il HttpContext disponibile con
                // tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre
                // nel thread non ho queste informazioni
                CartelleDAO dao = new CartelleDAO();

                // Elimino calcoli vecchi effettuati
                //dao.DeleteCalcoloMassivo (_Anno, _IdEnte, _TipoRuolo);
                dao.DeleteCalcoloMassivo(_IdFlusso);

                //_dbEngine = dao.StartCalcoloMassivoTransaction ();
                //connTrans = dao.StartCalcoloMassivoTransaction();

                //_dbEngineNoTrans = dao.OpenCalcoloMassivoConnection ();
                connCmd = dao.OpenCalcoloMassivoConnection();

                t.Start();
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.CalcoloOSAP.StartCalcolo.errore: ", Err);

            }
        }
        /**** 201810 - Calcolo Puntuale ****/
        public void CalcoloSync(bool Simula, Ruolo.E_TIPO TipoRuolo, int Anno, Categorie[] categ, TipologieOccupazioni[] tipiOcc, Agevolazione[] agev, Tariffe[] tarif, string IdEnte, string Operatore, string CodTributo, double ImportoRate, int IdFlussoPrec, string ListOccupazioni, int IDDichiarazione)
        {
            try
            {
                _Anno = Anno;
                _Categ = categ;
                _TipiOcc = tipiOcc;
                _Agev = agev;
                _Tarif = tarif;
                _IdEnte = IdEnte;
                _Operatore = Operatore;
                _CodTributo = CodTributo;
                _ImportoRate = ImportoRate;
                _TipoRuolo = TipoRuolo;
                _IdFlusso = IdFlussoPrec;
                _ListOccupazioni = ListOccupazioni;
                _IsSimula = Simula;
                _IDDichiarazione = IDDichiarazione;

                CartelleDAO dao = new CartelleDAO();
                // Elimino calcoli vecchi effettuati
                dao.DeleteCalcoloMassivo(_IdFlusso);
                //connTrans = dao.StartCalcoloMassivoTransaction();
                connCmd = dao.OpenCalcoloMassivoConnection();
                StartCalcoloThreadEntryPoint();
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.CalcoloOSAP.CalcoloSync.errore: ", Err);
            }
        }
        /**** ****/
        /// <summary>
        /// Funzione asincrona per il calcolo del ruolo.
        /// Recupero gli ID di tutte le dichiarazioni da elaborare; ottengo la dichiarazione con tutti gli articoli; scorro tutti gli articoli della dichiarazione e memorizzo i ruoli, se sono in supplettivo prelevo l'eventuale articolo già generato per il dettagliotestata e anno; richiamo il motore di calcolo; creo la cartella con i ruoli; salvo in una hashtable tutti gli utenti per memorizzare le statistiche di elaborazione alla fine.
        /// In caso di eccezione memorizzo comunque l'elaborazione effettuata, segnando l'eccezione verificatasi
        /// </summary>
        /// <revisionHistory>
        /// <revision date="10/06/2013">
        /// <strong>ruolo supplettivo</strong>
        /// </revision>
        /// <revision date="10/2018">
        /// <strong>Calcolo Puntuale</strong>
        /// </revision>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// <revision date="24/03/2020">In caso di supplettivo devo considerare solo l'avviso precedente per le stesse tipologie di occupazioni</revision>
        /// </revisionHistory>
        private void StartCalcoloThreadEntryPoint()
        {
            CacheManager.SetCalcoloMassivoInCorso(_Anno);

            ElaborazioneEffettuata elab = new ElaborazioneEffettuata();
            CartelleDAO dao = new CartelleDAO();
            ArrayList ListAvvisi = new ArrayList();

            elab.IdEnte = _IdEnte;
            elab.IdTributo = _CodTributo;
            elab.Anno = _Anno;
            elab.TipoRuolo = (int)_TipoRuolo;
            elab.DataOraDocumentiStampati = DichiarazioneSession.MyDateMinValue;
            elab.DataOraDocumentiApprovati = DichiarazioneSession.MyDateMinValue;
            elab.DataOraMinutaApprovata = DichiarazioneSession.MyDateMinValue;
            elab.DataOraMinutaStampata = DichiarazioneSession.MyDateMinValue;
            elab.DataOraCalcoloRate = DichiarazioneSession.MyDateMinValue;
            elab.ImportoTotale = 0.0;
            elab.NArticoli = 0;
            elab.DataOraInizioElaborazione = DateTime.Now;
            elab.SogliaMinimaRate = _ImportoRate;
            elab.ListOccupazioni = _ListOccupazioni;

            try
            {
                bool Error = false;
                string ErrorMessage = string.Empty;
                Hashtable utenti = new Hashtable();
                Hashtable dichiarazioniCalcolo = new Hashtable();

                int[] ListaDichiarazioni = DTO.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno(_Anno, _IdEnte, _CodTributo, -1, _IDDichiarazione, _ListOccupazioni, ref connCmd);
                IRemotingInterfaceOSAP motore = (IRemotingInterfaceOSAP)Activator.GetObject(typeof(IRemotingInterfaceOSAP), DichiarazioneSession.UrlMotoreOSAP);
                checked
                {
                    for (int i = 0; i < ListaDichiarazioni.Length && !Error; i++)
                    {
                        CacheManager.SetAvanzamentoElaborazione("Posizione " + i.ToString() + " di " + ListaDichiarazioni.Length.ToString());
                        int IdDichiarazione = ListaDichiarazioni[i];
                        Log.Debug("Calcolo IdDichiarazione=" + IdDichiarazione.ToString());
                        DichiarazioneTosapCosap Dichiarazione = MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore(IdDichiarazione, _IdEnte, _CodTributo, _Anno, _ListOccupazioni, ref connCmd);
                        Cartella cart = CreateCartella(_Anno, Dichiarazione, Dichiarazione.ArticoliDichiarazione.Length);
                        checked
                        {
                            for (int j = 0; j < Dichiarazione.ArticoliDichiarazione.Length && !Error; j++)
                            {
                                Articolo a = Dichiarazione.ArticoliDichiarazione[j];
                                CalcoloResult ArticoloOrdinario = null;
                                if (_TipoRuolo == Ruolo.E_TIPO.SUPPLETIVO)
                                {
                                    ArticoloOrdinario = MetodiArticolo.GetArticoloPrecedente(_IdEnte, Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, _Anno, _ListOccupazioni, ref connCmd);
                                }

                                CalcoloResult result = motore.CalcolaOSAP(_TipoRuolo, a, _Categ, _TipiOcc, _Agev, _Tarif, ArticoloOrdinario);
                                Log.Debug("result.Result=" + result.Result.ToString());
                                Log.Debug("result.ImportoCalcolato=" + result.ImportoCalcolato.ToString());
                                //*** ***
                                if (result.Result == E_CALCOLORESULT.OK)
                                {
                                    if (double.Parse(string.Format("{0:0.00}", result.ImportoCalcolato)) > 0)
                                    {
                                        elab.NArticoli++;
                                        double ImportoDueDecimali = double.Parse(string.Format("{0:0.00}", result.ImportoCalcolato));
                                        elab.ImportoTotale += ImportoDueDecimali;
                                        cart.ImportoTotale += ImportoDueDecimali;
                                        cart.Ruoli[j] = CreateRuolo(result.ImportoCalcolato, a.IdArticolo);
                                        cart.Ruoli[j].ImportoLordo = double.Parse(string.Format("{0:0.00}", result.ImportoLordo));
                                        cart.Ruoli[j].Tariffa = new Tariffe();
                                        cart.Ruoli[j].Tariffa.Valore = result.TariffaApplicata;
                                        cart.Ruoli[j].ArticoloTOCO = a;
                                        cart.IdEnte = elab.IdEnte;
                                        cart.IdTributo = elab.IdTributo;
                                    }
                                }
                                else
                                {
                                    Error = true;
                                    switch (result.Result)
                                    {
                                        case (E_CALCOLORESULT.ERRORECALCOLO):
                                            ErrorMessage = "Si è verificato un errore di calcolo durante ";
                                            break;
                                        case (E_CALCOLORESULT.NOCATEGORIA):
                                            ErrorMessage = "Non è stata trovata la categoria corretta per ";
                                            break;
                                        case (E_CALCOLORESULT.NOTARIFFA):
                                            ErrorMessage = "Non è stata trovata la tariffa corretta per ";
                                            break;
                                        case (E_CALCOLORESULT.NOTIPOLOGIAOCCUPAZIONE):
                                            ErrorMessage = "Non è stata trovata la tipologia corretta per ";
                                            break;
                                    }
                                    ErrorMessage += " l'elaborazione dell'articolo n. " + (j + 1) + ", tipologia \"" + a.TipologiaOccupazione.Descrizione
                                        + "\" categoria " + a.Categoria.Descrizione
                                        + "\" durata " + a.TipoDurata.Descrizione
                                        + "\" della dichiarazione n. " + Dichiarazione.TestataDichiarazione.NDichiarazione;

                                    break;
                                }
                            }
                        }
                        if (ErrorMessage == string.Empty && cart.ImportoTotale > 0)
                        {
                            //connTrans.CommandTimeout = 0;
                            cart.IdCartella = -1;
                            cart.DataEmissione = DateTime.Now;
                            if (!_IsSimula)
                            {
                                MetodiCartella.InsertCartella(ref cart, ref connCmd, true, 0, _Operatore);//MetodiCartella.InsertCartella(ref cart, ref connTrans, true, 0, _Operatore);
                            }
                            if (!utenti.ContainsKey(Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE))
                                utenti.Add(Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE);
                            if (!dichiarazioniCalcolo.ContainsKey(Dichiarazione.IdDichiarazione))
                                dichiarazioniCalcolo.Add(Dichiarazione.IdDichiarazione, Dichiarazione.IdDichiarazione);
                            ListAvvisi.Add(cart);
                        }
                    }
                }
                elab.DataOraFineElaborazione = DateTime.Now;
                elab.Note = ErrorMessage;
                elab.NUtenti = utenti.Keys.Count;
                elab.NDichiarazioni = dichiarazioniCalcolo.Keys.Count;
                Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.elab.NUtenti=" + elab.NUtenti.ToString());
                Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.elab.NDichiarazioni=" + elab.NDichiarazioni.ToString());
                Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.elab.Note=" + elab.Note);
                new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, _Operatore, new Utility.Costanti.LogEventArgument().Elaborazioni, "StartCalcoloThreadEntryPoint", Utility.Costanti.AZIONE_NEW.ToString(), _CodTributo, _IdEnte, elab.IdFlusso);
                if (!_IsSimula)
                {
                    MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref connCmd, _Operatore);//MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref connTrans,_Operatore);
                    if (!Error)
                    {
                        //dao.CommitCalcoloMassivoTransaction(ref connTrans);
                        try
                        {
                            int i = 0;
                            Lotto myLotto = new Lotto();
                            myLotto.IdEnte = _IdEnte;
                            myLotto.Anno = _Anno;

                            MetodiLotto.InsertLotto(myLotto, ref connCmd);
                            myLotto = MetodiLotto.GetLotto(_IdEnte, _Anno, -1, ref connCmd);
                            foreach (Cartella myAvviso in ListAvvisi)
                            {
                                myAvviso.CodiceCartella = new CalcoloRateOSAP().GetCodiceCartella(myLotto.CodiceConcessione, _Anno.ToString(), myLotto.PrimaCartella + i);
                                MetodiCartella.SetCodiceCartella(myAvviso, ref connCmd);
                                i++;
                            }
                        }
                        catch (Exception myEx)
                        {
                            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.CalcoloCodiceCartella.errore: ", myEx);
                        }
                    }
                    else
                    {
                        //dao.RollbackCalcoloMassivoTransaction(ref connTrans);
                    }
                }
            }
            catch (Exception ex)
            {
                //dao.RollbackCalcoloMassivoTransaction(ref connTrans);
                elab.DataOraFineElaborazione = DateTime.Now;
                elab.Note = "Eccezione durante il calcolo massivo: " + ex.Message;
                try
                {
                    if (!_IsSimula)
                        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref connCmd, _Operatore);
                }
                catch (Exception Err)
                {
                    Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.errore: ", Err);
                }
            }
            finally
            {
                connCmd.Connection.Close();
                CacheManager.SetRiepilogoCalcoloMassivo(elab);
                CacheManager.SetAvvisiCalcoloMassivo((Cartella[])ListAvvisi.ToArray(typeof(Cartella)));
                CacheManager.RemoveCalcoloMassivoInCorso();
                CacheManager.RemoveAvanzamentoElaborazione();
            }
        }
        //private void StartCalcoloThreadEntryPoint()
        //{
        //    CacheManager.SetCalcoloMassivoInCorso(_Anno);

        //    ElaborazioneEffettuata elab = new ElaborazioneEffettuata();
        //    CartelleDAO dao = new CartelleDAO();
        //    ArrayList ListAvvisi = new ArrayList();

        //    elab.IdEnte = _IdEnte;
        //    elab.IdTributo = _CodTributo;
        //    elab.Anno = _Anno;
        //    elab.TipoRuolo = (int)_TipoRuolo;
        //    elab.DataOraDocumentiStampati = DichiarazioneSession.MyDateMinValue;
        //    elab.DataOraDocumentiApprovati = DichiarazioneSession.MyDateMinValue;
        //    elab.DataOraMinutaApprovata = DichiarazioneSession.MyDateMinValue;
        //    elab.DataOraMinutaStampata = DichiarazioneSession.MyDateMinValue;
        //    elab.DataOraCalcoloRate = DichiarazioneSession.MyDateMinValue;
        //    elab.ImportoTotale = 0.0;
        //    elab.NArticoli = 0;
        //    elab.DataOraInizioElaborazione = DateTime.Now;
        //    elab.SogliaMinimaRate = _ImportoRate;
        //    elab.ListOccupazioni = _ListOccupazioni;

        //    try
        //    {
        //        bool Error = false;
        //        string ErrorMessage = string.Empty;
        //        Hashtable utenti = new Hashtable();
        //        Hashtable dichiarazioniCalcolo = new Hashtable();

        //        int[] ListaDichiarazioni = DTO.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno(_Anno, _IdEnte, _CodTributo, -1, _IDDichiarazione, _ListOccupazioni, ref connCmd);
        //        IRemotingInterfaceOSAP motore = (IRemotingInterfaceOSAP)Activator.GetObject(typeof(IRemotingInterfaceOSAP), DichiarazioneSession.UrlMotoreOSAP);
        //        checked
        //        {
        //            for (int i = 0; i < ListaDichiarazioni.Length && !Error; i++)
        //            {
        //                CacheManager.SetAvanzamentoElaborazione("Posizione " + i.ToString() + " di " + ListaDichiarazioni.Length.ToString());
        //                int IdDichiarazione = ListaDichiarazioni[i];
        //                Log.Debug("Calcolo IdDichiarazione=" + IdDichiarazione.ToString());
        //                DichiarazioneTosapCosap Dichiarazione = MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore(IdDichiarazione, _IdEnte, _CodTributo, _Anno, _ListOccupazioni, ref connCmd);
        //                Cartella cart = CreateCartella(_Anno, Dichiarazione, Dichiarazione.ArticoliDichiarazione.Length);
        //                checked
        //                {
        //                    for (int j = 0; j < Dichiarazione.ArticoliDichiarazione.Length && !Error; j++)
        //                    {
        //                        Articolo a = Dichiarazione.ArticoliDichiarazione[j];
        //                        CalcoloResult ArticoloOrdinario = null;
        //                        if (_TipoRuolo == Ruolo.E_TIPO.SUPPLETIVO)
        //                        {
        //                            ArticoloOrdinario = MetodiArticolo.GetArticoloPrecedente(_IdEnte, Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, _Anno, IdDichiarazione, ref connCmd);
        //                        }

        //                        CalcoloResult result = motore.CalcolaOSAP(_TipoRuolo, a, _Categ, _TipiOcc, _Agev, _Tarif, ArticoloOrdinario);
        //                        Log.Debug("result.Result=" + result.Result.ToString());
        //                        Log.Debug("result.ImportoCalcolato=" + result.ImportoCalcolato.ToString());
        //                        //*** ***
        //                        if (result.Result == E_CALCOLORESULT.OK)
        //                        {
        //                            if (double.Parse(string.Format("{0:0.00}", result.ImportoCalcolato)) > 0)
        //                            {
        //                                elab.NArticoli++;
        //                                double ImportoDueDecimali = double.Parse(string.Format("{0:0.00}", result.ImportoCalcolato));
        //                                elab.ImportoTotale += ImportoDueDecimali;
        //                                cart.ImportoTotale += ImportoDueDecimali;
        //                                cart.Ruoli[j] = CreateRuolo(result.ImportoCalcolato, a.IdArticolo);
        //                                cart.Ruoli[j].ImportoLordo = double.Parse(string.Format("{0:0.00}", result.ImportoLordo));
        //                                cart.Ruoli[j].Tariffa = new Tariffe();
        //                                cart.Ruoli[j].Tariffa.Valore = result.TariffaApplicata;
        //                                cart.Ruoli[j].ArticoloTOCO = a;
        //                                cart.IdEnte = elab.IdEnte;
        //                                cart.IdTributo = elab.IdTributo;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Error = true;
        //                            switch (result.Result)
        //                            {
        //                                case (E_CALCOLORESULT.ERRORECALCOLO):
        //                                    ErrorMessage = "Si è verificato un errore di calcolo durante ";
        //                                    break;
        //                                case (E_CALCOLORESULT.NOCATEGORIA):
        //                                    ErrorMessage = "Non è stata trovata la categoria corretta per ";
        //                                    break;
        //                                case (E_CALCOLORESULT.NOTARIFFA):
        //                                    ErrorMessage = "Non è stata trovata la tariffa corretta per ";
        //                                    break;
        //                                case (E_CALCOLORESULT.NOTIPOLOGIAOCCUPAZIONE):
        //                                    ErrorMessage = "Non è stata trovata la tipologia corretta per ";
        //                                    break;
        //                            }
        //                            ErrorMessage += " l'elaborazione dell'articolo n. " + (j + 1) + ", tipologia \"" + a.TipologiaOccupazione.Descrizione
        //                                + "\" categoria " + a.Categoria.Descrizione
        //                                + "\" durata " + a.TipoDurata.Descrizione
        //                                + "\" della dichiarazione n. " + Dichiarazione.TestataDichiarazione.NDichiarazione;

        //                            break;
        //                        }
        //                    }
        //                }
        //                if (ErrorMessage == string.Empty && cart.ImportoTotale > 0)
        //                {
        //                    //connTrans.CommandTimeout = 0;
        //                    cart.IdCartella = -1;
        //                    cart.DataEmissione = DateTime.Now;
        //                    if (!_IsSimula)
        //                    {
        //                        MetodiCartella.InsertCartella(ref cart, ref connCmd, true, 0, _Operatore);//MetodiCartella.InsertCartella(ref cart, ref connTrans, true, 0, _Operatore);
        //                    }
        //                    if (!utenti.ContainsKey(Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE))
        //                        utenti.Add(Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE);
        //                    if (!dichiarazioniCalcolo.ContainsKey(Dichiarazione.IdDichiarazione))
        //                        dichiarazioniCalcolo.Add(Dichiarazione.IdDichiarazione, Dichiarazione.IdDichiarazione);
        //                    ListAvvisi.Add(cart);
        //                }
        //            }
        //        }
        //        elab.DataOraFineElaborazione = DateTime.Now;
        //        elab.Note = ErrorMessage;
        //        elab.NUtenti = utenti.Keys.Count;
        //        elab.NDichiarazioni = dichiarazioniCalcolo.Keys.Count;
        //        Log.Debug(_IdEnte  +"."+ _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.elab.NUtenti=" + elab.NUtenti.ToString());
        //        Log.Debug(_IdEnte  +"."+ _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.elab.NDichiarazioni=" + elab.NDichiarazioni.ToString());
        //        Log.Debug(_IdEnte  +"."+ _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.elab.Note=" + elab.Note);
        //        new Utility.DBUtility(DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionOPENgov).LogActionEvent(DateTime.Now, _Operatore, new Utility.Costanti.LogEventArgument().Elaborazioni, "StartCalcoloThreadEntryPoint", Utility.Costanti.AZIONE_NEW.ToString(), _CodTributo, _IdEnte, elab.IdFlusso);
        //        if (!_IsSimula)
        //        {
        //            MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref connCmd, _Operatore);//MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref connTrans,_Operatore);
        //            if (!Error)
        //            {
        //                //dao.CommitCalcoloMassivoTransaction(ref connTrans);
        //                try
        //                {
        //                    int i = 0;
        //                    Lotto myLotto = new Lotto();
        //                    myLotto.IdEnte = _IdEnte;
        //                    myLotto.Anno = _Anno;

        //                    MetodiLotto.InsertLotto(myLotto, ref connCmd);
        //                    myLotto = MetodiLotto.GetLotto(_IdEnte, _Anno, -1, ref connCmd);
        //                    foreach (Cartella myAvviso in ListAvvisi)
        //                    {
        //                        myAvviso.CodiceCartella = new CalcoloRateOSAP().GetCodiceCartella(myLotto.CodiceConcessione, _Anno.ToString(), myLotto.PrimaCartella + i);
        //                        MetodiCartella.SetCodiceCartella(myAvviso, ref connCmd);
        //                        i++;
        //                    }
        //                }
        //                catch (Exception myEx)
        //                {
        //                    Log.Debug(_IdEnte  +"."+ _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.CalcoloCodiceCartella.errore: ", myEx);
        //                }
        //            }
        //            else
        //            {
        //                //dao.RollbackCalcoloMassivoTransaction(ref connTrans);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //dao.RollbackCalcoloMassivoTransaction(ref connTrans);
        //        elab.DataOraFineElaborazione = DateTime.Now;
        //        elab.Note = "Eccezione durante il calcolo massivo: " + ex.Message;
        //        try
        //        {
        //            if (!_IsSimula)
        //                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref connCmd, _Operatore);
        //        }
        //        catch (Exception Err)
        //        {
        //            Log.Debug(_IdEnte  +"."+ _Operatore + " - OPENgovOSAP.CalcoloOSAP.StartCalcoloThreadEntryPoint.errore: ", Err);
        //        }
        //    }
        //    finally
        //    {
        //        connCmd.Connection.Close();
        //        CacheManager.SetRiepilogoCalcoloMassivo(elab);
        //        CacheManager.SetAvvisiCalcoloMassivo((Cartella[])ListAvvisi.ToArray(typeof(Cartella)));
        //        CacheManager.RemoveCalcoloMassivoInCorso();
        //        CacheManager.RemoveAvanzamentoElaborazione();
        //    }
        //}

        private Cartella CreateCartella(int Anno, DichiarazioneTosapCosap Dichiarazione, int nArticoli)
        {
            try
            {
                Cartella cart = new Cartella();
                cart.IdTributo = _CodTributo;
                cart.IdEnte = _IdEnte;
                cart.Anno = Anno;
                cart.CodContribuente = Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE;
                cart.ImportoTotale = 0.0;
                cart.Dichiarazione = new DichiarazioneTosapCosap();
                cart.Dichiarazione.IdDichiarazione = Dichiarazione.IdDichiarazione;
                cart.Ruoli = new Ruolo[nArticoli];
                return cart;
            }
            catch (Exception Err)
            {
                Log.Debug(_IdEnte  +"."+ _Operatore + " - OPENgovOSAP.CalcoloOSAP.CreateCartella.errore: ", Err);
                throw Err;
            }
        }

        private Ruolo CreateRuolo(double Importo, int IdArticolo)
        {
            try
            {
                Ruolo ruolo = new Ruolo();
                ruolo.Importo = Importo;
                ruolo.ArticoloTOCO = new Articolo();
                ruolo.ArticoloTOCO.IdArticolo = IdArticolo;
                return ruolo;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloOSAP.CreateRuolo.errore: ", Err);
                throw Err;
            }
        }
    }
    /// <summary>
    /// Definizione oggetto flusso SCUOLE
    /// </summary>
    public class ObjFlussoScuole : IComparable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ObjFlussoScuole));
        public string CodFiscale = "";
        public string LineFlusso = "";

        public enum PosizioniAvvisi
        {
            ANNO = 2
            ,
            COGNOME = 6
                ,
            NOME = 7
                ,
            CF = 8
                ,
            DATANASCITA = 9
                ,
            COMUNENASCITA = 10
                ,
            VIA_RES = 12
                ,
            FRAZIONE_RES = 11
                ,
            CAP_RES = 13
                ,
            COMUNE_RES = 14
                ,
            PROV_RES = 15
                ,
            VIA_CO = 16
                ,
            CAP_CO = 17
                ,
            COMUNE_CO = 18
                ,
            PROV_CO = 19
                ,
            COGNOME_ALUNNO = 21
                ,
            NOME_ALUNNO = 22
                ,
            DAL = 23
                ,
            AL = 24
                ,
            SERVIZIO = 25
                ,
            SCUOLA = 27
                ,
            SEZIONE = 28
                ,
            OCCORRENZE = 29
                ,
            COSTO_UNITARIO = 30
                , COSTO_TOTALE = 32
        }

        // implement IComparable interface
        public int CompareTo(object obj)
        {
            try
            {
                if (obj is ObjFlussoScuole)
                {
                    return this.CodFiscale.CompareTo((obj as ObjFlussoScuole).CodFiscale);
                }
                throw new ArgumentException("Object is not a ObjFlussoScuole");
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.ObjFlussoScuole.CompareTo.errore: ", Err);
                throw Err;
            }
        }
    }
    /// <summary>
    /// Classe importazione flussi
    /// </summary>
    public class clsImportFlussi
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(clsImportFlussi));
        private string _DBType;
        private string _ConnString;
        private string _ConnStringAnag;
        private string _Operatore;
        private string _IdEnte;
        private string _CodTributo;
        private string _NomeFile;
        private string _TipoUfficio;
        private string _Richiedente;
        private SqlCommand conn;
        private SqlCommand connCmd;
        public void StartImport(string TypeDB, string ConnString, string ConnStringAnag, string IdEnte, string CodTributo, string NomeFile, string TipoUfficio, string Richiedente, string Operatore)
        {
            try
            {
                ThreadStart threadDelegate = new ThreadStart(this.StartImportThreadEntryPoint);
                Thread t = new Thread(threadDelegate);

                _DBType = TypeDB;
                _ConnString = ConnString;
                _ConnStringAnag = ConnStringAnag;
                _IdEnte = IdEnte;
                _CodTributo = CodTributo;
                _NomeFile = NomeFile;
                _TipoUfficio = TipoUfficio;
                _Richiedente = Richiedente;
                _Operatore = Operatore;

                // Inizio la transazione e recupero l'istanza della connessione al db
                // Devo farlo qui visto che sto eseguendo codice ancora nel thread
                // della PostBack da FE, per cui ho il HttpContext disponibile con
                // tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre
                // nel thread non ho queste informazioni
                CartelleDAO dao = new CartelleDAO();

                conn = dao.StartImportTransaction();

                connCmd = dao.OpenImportConnection();

                t.Start();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImport.errore: ", Err);
                throw Err;
            }
        }
        /// <summary>
        /// Metodo per l'importazione del ruolo.
        /// carico il file in arraylist; ordino i dati del file; carico il file in apposito oggetto che lega dichiarazioni e cartelle; valorizzo gli oggetti della dichiarazione; aggiorno l'array delle dichiarazioni; valorizzo gli oggetti dell'avviso; valorizzo l'array degli avvisi; azzero per ripartire da capo; 
        /// popolo testata; popolo anagrafe; popolo articoli; popolo ruoli; memorizzo il soggetto trattato; 
        /// valorizzo gli oggetti della dichiarazione; aggiorno l'array delle dichiarazioni; valorizzo gli oggetti dell'avviso; valorizzo l'array degli avvisi; azzero per ripartire da capo; inserisco la dichiarazione; inserisco l'avviso; valorizzo il flusso ruolo elaborato; Inserimento lotto
        /// </summary>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        private void StartImportThreadEntryPoint()
        {
            CacheManager.SetImportInCorso(_NomeFile);
            CartelleDAO dao = new CartelleDAO();
            try
            {
                string sLineFile, sCFPrec, sAnnoAvviso;
                sLineFile = sCFPrec = sAnnoAvviso = "";
                double impAvviso, impRuolo;
                impAvviso = impRuolo = 0;
                int nArticoliRuolo = 0;
                System.IO.StreamReader oMyFile = new System.IO.StreamReader(_NomeFile);
                ArrayList listDatiFile = new ArrayList();
                ArrayList listDichiarazioni = new ArrayList();
                ArrayList listArticoli = new ArrayList();
                ArrayList listRuoli = new ArrayList();
                ArrayList listAvvisi = new ArrayList();
                DichiarazioneTosapCosap myDich = new DichiarazioneTosapCosap();
                DichiarazioneTosapCosapTestata myTestata = new DichiarazioneTosapCosapTestata();
                DettaglioAnagrafica myAnag = new DettaglioAnagrafica();
                Cartella myAvviso = new Cartella();
                Hashtable utenti = new Hashtable();

                IFormatProvider myCultureInfo = new System.Globalization.CultureInfo("it-IT", true);

                try
                {
                    while ((sLineFile = oMyFile.ReadLine()) != null)
                    {
                        ObjFlussoScuole myRC = new ObjFlussoScuole();
                        myRC.CodFiscale = sLineFile.Split(char.Parse(";"))[(int)ObjFlussoScuole.PosizioniAvvisi.CF];
                        myRC.LineFlusso = sLineFile;
                        listDatiFile.Add(myRC);
                    }
                }
                catch (Exception Err)
                {
                    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Err);

                }
                finally
                {
                    oMyFile.Close();
                }
                listDatiFile.Sort();

                foreach (ObjFlussoScuole oLine in listDatiFile)
                {
                    string[] myLine = oLine.LineFlusso.Split(char.Parse(";"));
                    if (oLine.CodFiscale == "")
                        oLine.CodFiscale = myLine[3].ToString();
                    Log.Debug("tratto:" + oLine.CodFiscale);
                    if (oLine.CodFiscale != sCFPrec)
                    {
                        if (sCFPrec != "")
                        {
                            Log.Debug("valorizzo gli oggetti della dichiarazione/avviso");
                            //valorizzo gli oggetti della dichiarazione
                            myDich.IdDichiarazione = Utility.Costanti.INIT_VALUE_NUMBER;
                            myDich.IdEnte = _IdEnte;
                            myDich.CodTributo = _CodTributo;
                            myDich.TestataDichiarazione = myTestata;
                            myDich.AnagraficaContribuente = myAnag;
                            myDich.ArticoliDichiarazione = (Articolo[])listArticoli.ToArray(typeof(Articolo));
                            //aggiorno l'array delle dichiarazioni
                            listDichiarazioni.Add(myDich);
                            //valorizzo gli oggetti dell'avviso
                            myAvviso.Dichiarazione = myDich;
                            myAvviso.IdTributo = _CodTributo;
                            myAvviso.IdEnte = _IdEnte;
                            myAvviso.Anno = int.Parse(sAnnoAvviso);
                            myAvviso.CodContribuente = myDich.AnagraficaContribuente.COD_CONTRIBUENTE;
                            myAvviso.ImportoTotale = impAvviso;
                            myAvviso.Ruoli = (Ruolo[])listRuoli.ToArray(typeof(Ruolo));
                            //valorizzo l'array degli avvisi
                            listAvvisi.Add(myAvviso);
                            if (!utenti.ContainsKey(myDich.AnagraficaContribuente.COD_CONTRIBUENTE))
                                utenti.Add(myDich.AnagraficaContribuente.COD_CONTRIBUENTE, myDich.AnagraficaContribuente.COD_CONTRIBUENTE);
                            //azzero per ripartire da capo
                            myDich = new DichiarazioneTosapCosap();
                            listArticoli = new ArrayList();
                            myTestata = new DichiarazioneTosapCosapTestata();
                            myAnag = new DettaglioAnagrafica();
                            myAvviso = new Cartella();
                            listRuoli = new ArrayList();
                            impAvviso = 0;
                        }

                        sAnnoAvviso = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.ANNO];
                        Log.Debug("popolo testata");
                        //popolo testata
                        myTestata.DataDichiarazione = DateTime.Now;
                        myTestata.IdTipoAtto = 1;
                        myTestata.TitoloRichiedente = MetodiTitoloRichiedente.GetTitoloRichiedente(_ConnString, _IdEnte, _Richiedente, false);
                        myTestata.Ufficio = MetodiUffici.GetUfficio(_ConnString, _IdEnte, _TipoUfficio, false);
                        myTestata.Operatore = _Operatore;
                        myTestata.DataInserimento = DateTime.Now;
                        Log.Debug("popolo anagrafe");
                        //popolo anagrafe
                        myAnag.CodEnte = _IdEnte;
                        myAnag.Cognome = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COGNOME];
                        myAnag.Nome = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.NOME];
                        myAnag.CodiceFiscale = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.CF];
                        myAnag.DataNascita = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.DATANASCITA];
                        myAnag.ComuneNascita = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COMUNENASCITA];
                        myAnag.ViaResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.VIA_RES];
                        myAnag.FrazioneResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.FRAZIONE_RES];
                        myAnag.CapResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.CAP_RES];
                        myAnag.ComuneResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COMUNE_RES];
                        myAnag.ProvinciaResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.PROV_RES];
                        ObjIndirizziSpedizione myIndSped = new ObjIndirizziSpedizione();
                        myIndSped.CodTributo = _CodTributo;
                        myIndSped.NomeInvio = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COGNOME] + " " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.NOME];
                        myIndSped.ViaRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.VIA_CO];
                        myIndSped.CapRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.CAP_CO];
                        myIndSped.ComuneRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COMUNE_CO];
                        myIndSped.ProvinciaRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.PROV_CO];
                        myAnag.ListSpedizioni.Add(myIndSped);
                        myAnag.dsContatti = new Anagrafica.DLL.dsContatti();
                        DettaglioAnagraficaReturn oAnagRet = new Anagrafica.DLL.GestioneAnagrafica().GestisciAnagrafica(myAnag, _DBType, _ConnStringAnag, true, false);
                        myAnag.COD_CONTRIBUENTE = int.Parse(oAnagRet.COD_CONTRIBUENTE);
                    }
                    //popolo articoli
                    Log.Debug("popolo articoli");
                    Articolo myArticolo = new Articolo();
                    try
                    {
                        myArticolo.Categoria = MetodiCategorie.GetCategoria(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SCUOLA] + " - " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SEZIONE], _IdEnte, _CodTributo);
                    }
                    catch
                    {
                        MetodiCategorie.InsertOrUpdateDescrizione(_ConnString, _IdEnte, _CodTributo, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SCUOLA] + " - " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SEZIONE]);
                        myArticolo.Categoria = MetodiCategorie.GetCategoria(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SCUOLA] + " - " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SEZIONE], _IdEnte, _CodTributo);
                    }
                    myArticolo.Consistenza = double.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.OCCORRENZE], myCultureInfo);
                    try
                    {
                        myArticolo.DataFineOccupazione = DateTime.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.AL], myCultureInfo);
                    }
                    catch (Exception Ex)
                    {
                        Log.Debug("Errore in lettura DataFine::" + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.AL], Ex);
                        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Ex);
                        throw new Exception(Ex.Message);
                    }
                    try
                    {
                        myArticolo.DataInizioOccupazione = DateTime.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.DAL], myCultureInfo);
                    }
                    catch (Exception Ex)
                    {
                        Log.Debug("Errore in lettura DataInizio::" + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.DAL], Ex);
                        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Ex);
                        throw new Exception(Ex.Message);
                    }
                    myArticolo.DataInserimento = DateTime.Now;
                    myArticolo.DurataOccupazione = int.Parse((myArticolo.DataFineOccupazione - myArticolo.DataInizioOccupazione).TotalDays.ToString()) + 1;
                    myArticolo.IdArticolo = listArticoli.Count + 1;
                    myArticolo.IdTributo = _CodTributo;
                    myArticolo.Operatore = _Operatore;
                    myArticolo.SVia = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COGNOME_ALUNNO] + " " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.NOME_ALUNNO];
                    TipoConsistenza myConsistenza = new TipoConsistenza();
                    myConsistenza.IdTipoConsistenza = (int)TipoConsistenza.E_CONSISTENZA.NUTENTI;
                    myArticolo.TipoConsistenzaTOCO = myConsistenza;
                    Durata myDurata = new Durata();
                    myDurata.IdDurata = 2;
                    myArticolo.TipoDurata = myDurata;
                    try
                    {
                        myArticolo.TipologiaOccupazione = MetodiTipologieOccupazioni.GetTipologiaOccupazione(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SERVIZIO], _IdEnte, _CodTributo);
                    }
                    catch
                    {
                        MetodiTipologieOccupazioni.InsertOrUpdateDescrizione(_ConnString, _IdEnte, _CodTributo, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SERVIZIO]);
                        myArticolo.TipologiaOccupazione = MetodiTipologieOccupazioni.GetTipologiaOccupazione(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SERVIZIO], _IdEnte, _CodTributo);
                    }
                    listArticoli.Add(myArticolo);
                    //popolo ruoli
                    Log.Debug("popolo ruoli");
                    Ruolo myRuolo = new Ruolo();
                    myRuolo.ArticoloTOCO = myArticolo;
                    myRuolo.ImportoLordo = myRuolo.Importo = double.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COSTO_TOTALE], myCultureInfo);
                    impAvviso += myRuolo.ImportoLordo;
                    impRuolo += myRuolo.ImportoLordo;
                    nArticoliRuolo += 1;
                    try
                    {
                        myRuolo.Tariffa = MetodiTariffe.GetTariffa(_ConnString, _IdEnte, int.Parse(sAnnoAvviso), myArticolo.Categoria.IdCategoria, myArticolo.TipologiaOccupazione.IdTipologiaOccupazione, _CodTributo);
                    }
                    catch
                    {
                        MetodiTariffe.InsertTariffa(_ConnString, _IdEnte, _CodTributo, myArticolo.Categoria.IdCategoria, myArticolo.TipologiaOccupazione.IdTipologiaOccupazione, myArticolo.TipoDurata.IdDurata, int.Parse(sAnnoAvviso), decimal.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COSTO_UNITARIO], myCultureInfo), 0);
                        myRuolo.Tariffa = MetodiTariffe.GetTariffa(_ConnString, _IdEnte, int.Parse(sAnnoAvviso), myArticolo.Categoria.IdCategoria, myArticolo.TipologiaOccupazione.IdTipologiaOccupazione, _CodTributo);
                    }
                    listRuoli.Add(myRuolo);
                    //memorizzo il soggetto trattato
                    sCFPrec = oLine.CodFiscale;
                }
                Log.Debug("valorizzo gli oggetti della dichiarazione/avviso");
                //valorizzo gli oggetti della dichiarazione
                myDich.IdDichiarazione = Utility.Costanti.INIT_VALUE_NUMBER;
                myDich.IdEnte = _IdEnte;
                myDich.CodTributo = _CodTributo;
                myDich.TestataDichiarazione = myTestata;
                myDich.AnagraficaContribuente = myAnag;
                myDich.ArticoliDichiarazione = (Articolo[])listArticoli.ToArray(typeof(Articolo));
                //aggiorno l'array delle dichiarazioni
                listDichiarazioni.Add(myDich);
                //valorizzo gli oggetti dell'avviso
                myAvviso.Dichiarazione = myDich;
                myAvviso.IdTributo = _CodTributo;
                myAvviso.IdEnte = _IdEnte;
                myAvviso.Anno = int.Parse(sAnnoAvviso);
                myAvviso.CodContribuente = myDich.AnagraficaContribuente.COD_CONTRIBUENTE;
                myAvviso.ImportoTotale = impAvviso;
                myAvviso.Ruoli = (Ruolo[])listRuoli.ToArray(typeof(Ruolo));
                //valorizzo l'array degli avvisi
                listAvvisi.Add(myAvviso);
                if (!utenti.ContainsKey(myDich.AnagraficaContribuente.COD_CONTRIBUENTE))
                    utenti.Add(myDich.AnagraficaContribuente.COD_CONTRIBUENTE, myDich.AnagraficaContribuente.COD_CONTRIBUENTE);
                //azzero per ripartire da capo
                myDich = new DichiarazioneTosapCosap();
                listArticoli = new ArrayList();
                myTestata = new DichiarazioneTosapCosapTestata();
                myAnag = new DettaglioAnagrafica();
                myAvviso = new Cartella();
                listRuoli = new ArrayList();
                impAvviso = 0;
                //inserisco la dichiarazione
                Log.Debug("inserisco la dichiarazione");
                foreach (DichiarazioneTosapCosap myItem in listDichiarazioni)
                {
                    myItem.TestataDichiarazione.NDichiarazione = MetodiDichiarazioneTosapCosap.GetNDichAutomatico(_ConnString, _IdEnte, _CodTributo);
                    myItem.IdDichiarazione = MetodiDichiarazioneTosapCosap.SetDichiarazione(_ConnString, myItem);
                }
                Log.Debug("inserisco l'avviso");
                //inserisco l'avviso
                foreach (Cartella myItem in listAvvisi)
                {
                    Cartella oSingleAvviso = myItem;
                    oSingleAvviso.CodiceCartella = MetodiCartella.GetNAvvisoAutomatico(_ConnString, _IdEnte, _CodTributo, sAnnoAvviso);
                    oSingleAvviso.IdCartella = -1;
                    oSingleAvviso.DataEmissione = DateTime.Now;
                    MetodiCartella.InsertCartella(ref oSingleAvviso, ref conn, false, 0, _Operatore);
                }
                Log.Debug("valorizzo il flusso ruolo elaborato");
                //valorizzo il flusso ruolo elaborato
                ElaborazioneEffettuata elab = new ElaborazioneEffettuata();
                elab.IdEnte = _IdEnte;
                elab.IdTributo = _CodTributo;
                elab.Anno = int.Parse(sAnnoAvviso);
                elab.TipoRuolo = (int)Ruolo.E_TIPO.ORDINARIO;
                elab.DataOraDocumentiStampati = DichiarazioneSession.MyDateMinValue;
                elab.DataOraDocumentiApprovati = DichiarazioneSession.MyDateMinValue;
                elab.DataOraMinutaApprovata = DichiarazioneSession.MyDateMinValue;
                elab.DataOraMinutaStampata = DichiarazioneSession.MyDateMinValue;
                elab.DataOraCalcoloRate = DichiarazioneSession.MyDateMinValue;
                elab.ImportoTotale = impRuolo;
                elab.NArticoli = nArticoliRuolo;
                elab.DataOraInizioElaborazione = DateTime.Now;
                elab.SogliaMinimaRate = 0;
                elab.DataOraFineElaborazione = DateTime.Now;
                elab.Note = "";
                elab.NUtenti = utenti.Keys.Count;
                elab.NDichiarazioni = utenti.Keys.Count;
                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref conn, _Operatore);

                // Inserimento lotto
                Lotto l = new Lotto();
                l.IdEnte = _IdEnte;
                l.Anno = int.Parse(sAnnoAvviso);
                MetodiLotto.InsertLotto(l, ref conn);

                dao.CommitImportTransaction(ref conn);
            }
            catch (Exception Err)
            {
                dao.RollbackImportTransaction(ref conn);
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Err);
            }
            finally
            {
                connCmd.Connection.Close();
                CacheManager.RemoveImportInCorso();
            }
        }
        //private void StartImportThreadEntryPoint()
        //{
        //    CacheManager.SetImportInCorso(_NomeFile);
        //    CartelleDAO dao = new CartelleDAO();
        //    try
        //    {
        //        string sLineFile, sCFPrec, sAnnoAvviso;
        //        sLineFile = sCFPrec = sAnnoAvviso = "";
        //        double impAvviso, impRuolo;
        //        impAvviso = impRuolo = 0;
        //        int nArticoliRuolo = 0;
        //        System.IO.StreamReader oMyFile = new System.IO.StreamReader(_NomeFile);
        //        ArrayList listDatiFile = new ArrayList();
        //        ArrayList listDichiarazioni = new ArrayList();
        //        ArrayList listArticoli = new ArrayList();
        //        ArrayList listRuoli = new ArrayList();
        //        ArrayList listAvvisi = new ArrayList();
        //        DichiarazioneTosapCosap myDich = new DichiarazioneTosapCosap();
        //        DichiarazioneTosapCosapTestata myTestata = new DichiarazioneTosapCosapTestata();
        //        DettaglioAnagrafica myAnag = new DettaglioAnagrafica();
        //        Cartella myAvviso = new Cartella();
        //        Hashtable utenti = new Hashtable();

        //        IFormatProvider myCultureInfo = new System.Globalization.CultureInfo("it-IT", true);
        //        //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");

        //        //carico il file in arraylist
        //        try
        //        {
        //            while ((sLineFile = oMyFile.ReadLine()) != null)
        //            {
        //                ObjFlussoScuole myRC = new ObjFlussoScuole();
        //                myRC.CodFiscale = sLineFile.Split(char.Parse(";"))[(int)ObjFlussoScuole.PosizioniAvvisi.CF];
        //                myRC.LineFlusso = sLineFile;
        //                listDatiFile.Add(myRC);
        //            }
        //        }
        //        catch (Exception Err)
        //        {
        //            Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Err);

        //        }
        //        finally
        //        {
        //            oMyFile.Close();
        //        }
        //        //ordino i dati del file
        //        //Utility.Comparatore mySort = new Utility.Comparatore(new string[] { "CodFiscale" }, new bool[] { Utility.TipoOrdinamento.Crescente });
        //        //Array.Sort(listDatiFile.ToArray(), mySort);
        //        listDatiFile.Sort();

        //        //carico il file in apposito oggetto che lega dichiarazioni e cartelle
        //        foreach (ObjFlussoScuole oLine in listDatiFile)
        //        {
        //            string[] myLine = oLine.LineFlusso.Split(char.Parse(";"));
        //            if (oLine.CodFiscale == "")
        //                oLine.CodFiscale = myLine[3].ToString();
        //            Log.Debug("tratto:" + oLine.CodFiscale);
        //            if (oLine.CodFiscale != sCFPrec)
        //            {
        //                if (sCFPrec != "")
        //                {
        //                    Log.Debug("valorizzo gli oggetti della dichiarazione/avviso");
        //                    //valorizzo gli oggetti della dichiarazione
        //                    myDich.IdDichiarazione = Utility.Costanti.INIT_VALUE_NUMBER;
        //                    myDich.IdEnte = _IdEnte;
        //                    myDich.CodTributo = _CodTributo;
        //                    myDich.TestataDichiarazione = myTestata;
        //                    myDich.AnagraficaContribuente = myAnag;
        //                    myDich.ArticoliDichiarazione = (Articolo[])listArticoli.ToArray(typeof(Articolo));
        //                    //aggiorno l'array delle dichiarazioni
        //                    listDichiarazioni.Add(myDich);
        //                    //valorizzo gli oggetti dell'avviso
        //                    myAvviso.Dichiarazione = myDich;
        //                    myAvviso.IdTributo = _CodTributo;
        //                    myAvviso.IdEnte = _IdEnte;
        //                    myAvviso.Anno = int.Parse(sAnnoAvviso);
        //                    myAvviso.CodContribuente = myDich.AnagraficaContribuente.COD_CONTRIBUENTE;
        //                    myAvviso.ImportoTotale = impAvviso;
        //                    myAvviso.Ruoli = (Ruolo[])listRuoli.ToArray(typeof(Ruolo));
        //                    //valorizzo l'array degli avvisi
        //                    listAvvisi.Add(myAvviso);
        //                    if (!utenti.ContainsKey(myDich.AnagraficaContribuente.COD_CONTRIBUENTE))
        //                        utenti.Add(myDich.AnagraficaContribuente.COD_CONTRIBUENTE, myDich.AnagraficaContribuente.COD_CONTRIBUENTE);
        //                    //azzero per ripartire da capo
        //                    myDich = new DichiarazioneTosapCosap();
        //                    listArticoli = new ArrayList();
        //                    myTestata = new DichiarazioneTosapCosapTestata();
        //                    myAnag = new DettaglioAnagrafica();
        //                    myAvviso = new Cartella();
        //                    listRuoli = new ArrayList();
        //                    impAvviso = 0;
        //                }

        //                sAnnoAvviso = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.ANNO];
        //                Log.Debug("popolo testata");
        //                //popolo testata
        //                myTestata.DataDichiarazione = DateTime.Now;
        //                myTestata.IdTipoAtto = 1;
        //                myTestata.TitoloRichiedente = MetodiTitoloRichiedente.GetTitoloRichiedente(_ConnString, _IdEnte, _Richiedente, false);
        //                myTestata.Ufficio = MetodiUffici.GetUfficio(_ConnString, _IdEnte, _TipoUfficio, false);
        //                myTestata.Operatore = _Operatore;
        //                myTestata.DataInserimento = DateTime.Now;
        //                Log.Debug("popolo anagrafe");
        //                //popolo anagrafe
        //                myAnag.CodEnte = _IdEnte;
        //                myAnag.Cognome = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COGNOME];
        //                myAnag.Nome = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.NOME];
        //                myAnag.CodiceFiscale = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.CF];
        //                myAnag.DataNascita = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.DATANASCITA];
        //                myAnag.ComuneNascita = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COMUNENASCITA];
        //                myAnag.ViaResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.VIA_RES];
        //                myAnag.FrazioneResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.FRAZIONE_RES];
        //                myAnag.CapResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.CAP_RES];
        //                myAnag.ComuneResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COMUNE_RES];
        //                myAnag.ProvinciaResidenza = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.PROV_RES];
        //                ObjIndirizziSpedizione myIndSped = new ObjIndirizziSpedizione();
        //                myIndSped.CodTributo = _CodTributo;
        //                myIndSped.NomeInvio = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COGNOME] + " " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.NOME];
        //                myIndSped.ViaRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.VIA_CO];
        //                myIndSped.CapRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.CAP_CO];
        //                myIndSped.ComuneRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COMUNE_CO];
        //                myIndSped.ProvinciaRCP = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.PROV_CO];
        //                myAnag.ListSpedizioni.Add(myIndSped);
        //                myAnag.dsContatti = new Anagrafica.DLL.dsContatti();
        //                DettaglioAnagraficaReturn oAnagRet = new Anagrafica.DLL.GestioneAnagrafica().GestisciAnagrafica(myAnag, _DBType, _ConnStringAnag, true, false);
        //                myAnag.COD_CONTRIBUENTE = int.Parse(oAnagRet.COD_CONTRIBUENTE);
        //            }
        //            //popolo articoli
        //            Log.Debug("popolo articoli");
        //            Articolo myArticolo = new Articolo();
        //            try
        //            {
        //                myArticolo.Categoria = MetodiCategorie.GetCategoria(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SCUOLA] + " - " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SEZIONE], _IdEnte, _CodTributo);
        //            }
        //            catch
        //            {
        //                MetodiCategorie.InsertOrUpdateDescrizione(_ConnString, _IdEnte, _CodTributo, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SCUOLA] + " - " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SEZIONE]);
        //                myArticolo.Categoria = MetodiCategorie.GetCategoria(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SCUOLA] + " - " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SEZIONE], _IdEnte, _CodTributo);
        //            }
        //            myArticolo.Consistenza = double.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.OCCORRENZE], myCultureInfo);
        //            try
        //            {
        //                myArticolo.DataFineOccupazione = DateTime.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.AL], myCultureInfo);
        //            }
        //            catch (Exception Ex)
        //            {
        //                Log.Debug("Errore in lettura DataFine::" + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.AL], Ex);
        //                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Ex);
        //                throw new Exception(Ex.Message);
        //            }
        //            try
        //            {
        //                myArticolo.DataInizioOccupazione = DateTime.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.DAL], myCultureInfo);
        //            }
        //            catch (Exception Ex)
        //            {
        //                Log.Debug("Errore in lettura DataInizio::" + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.DAL], Ex);
        //                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Ex);
        //                throw new Exception(Ex.Message);
        //            }
        //            myArticolo.DataInserimento = DateTime.Now;
        //            myArticolo.DurataOccupazione = int.Parse((myArticolo.DataFineOccupazione - myArticolo.DataInizioOccupazione).TotalDays.ToString()) + 1;
        //            myArticolo.IdArticolo = listArticoli.Count + 1;
        //            myArticolo.IdTributo = _CodTributo;
        //            myArticolo.Operatore = _Operatore;
        //            myArticolo.SVia = myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COGNOME_ALUNNO] + " " + myLine[(int)ObjFlussoScuole.PosizioniAvvisi.NOME_ALUNNO];
        //            TipoConsistenza myConsistenza = new TipoConsistenza();
        //            myConsistenza.IdTipoConsistenza = (int)TipoConsistenza.E_CONSISTENZA.NUTENTI;
        //            myArticolo.TipoConsistenzaTOCO = myConsistenza;
        //            Durata myDurata = new Durata();
        //            myDurata.IdDurata = 2;
        //            myArticolo.TipoDurata = myDurata;
        //            try
        //            {
        //                myArticolo.TipologiaOccupazione = MetodiTipologieOccupazioni.GetTipologiaOccupazione(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SERVIZIO], _IdEnte, _CodTributo);
        //            }
        //            catch
        //            {
        //                MetodiTipologieOccupazioni.InsertOrUpdateDescrizione(_ConnString, _IdEnte, _CodTributo, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SERVIZIO]);
        //                myArticolo.TipologiaOccupazione = MetodiTipologieOccupazioni.GetTipologiaOccupazione(_ConnString, -1, myLine[(int)ObjFlussoScuole.PosizioniAvvisi.SERVIZIO], _IdEnte, _CodTributo);
        //            }
        //            listArticoli.Add(myArticolo);
        //            //popolo ruoli
        //            Log.Debug("popolo ruoli");
        //            Ruolo myRuolo = new Ruolo();
        //            myRuolo.ArticoloTOCO = myArticolo;
        //            myRuolo.ImportoLordo = myRuolo.Importo = double.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COSTO_TOTALE], myCultureInfo);
        //            impAvviso += myRuolo.ImportoLordo;
        //            impRuolo += myRuolo.ImportoLordo;
        //            nArticoliRuolo += 1;
        //            try
        //            {
        //                myRuolo.Tariffa = MetodiTariffe.GetTariffa(_ConnString, _IdEnte, int.Parse(sAnnoAvviso), myArticolo.Categoria.IdCategoria, myArticolo.TipologiaOccupazione.IdTipologiaOccupazione, _CodTributo);
        //            }
        //            catch
        //            {
        //                MetodiTariffe.InsertTariffa(_ConnString, _IdEnte, _CodTributo, myArticolo.Categoria.IdCategoria, myArticolo.TipologiaOccupazione.IdTipologiaOccupazione, myArticolo.TipoDurata.IdDurata, int.Parse(sAnnoAvviso), decimal.Parse(myLine[(int)ObjFlussoScuole.PosizioniAvvisi.COSTO_UNITARIO], myCultureInfo), 0);
        //                myRuolo.Tariffa = MetodiTariffe.GetTariffa(_ConnString, _IdEnte, int.Parse(sAnnoAvviso), myArticolo.Categoria.IdCategoria, myArticolo.TipologiaOccupazione.IdTipologiaOccupazione, _CodTributo);
        //            }
        //            listRuoli.Add(myRuolo);
        //            //memorizzo il soggetto trattato
        //            sCFPrec = oLine.CodFiscale;
        //        }
        //        Log.Debug("valorizzo gli oggetti della dichiarazione/avviso");
        //        //valorizzo gli oggetti della dichiarazione
        //        myDich.IdDichiarazione = Utility.Costanti.INIT_VALUE_NUMBER;
        //        myDich.IdEnte = _IdEnte;
        //        myDich.CodTributo = _CodTributo;
        //        myDich.TestataDichiarazione = myTestata;
        //        myDich.AnagraficaContribuente = myAnag;
        //        myDich.ArticoliDichiarazione = (Articolo[])listArticoli.ToArray(typeof(Articolo));
        //        //aggiorno l'array delle dichiarazioni
        //        listDichiarazioni.Add(myDich);
        //        //valorizzo gli oggetti dell'avviso
        //        myAvviso.Dichiarazione = myDich;
        //        myAvviso.IdTributo = _CodTributo;
        //        myAvviso.IdEnte = _IdEnte;
        //        myAvviso.Anno = int.Parse(sAnnoAvviso);
        //        myAvviso.CodContribuente = myDich.AnagraficaContribuente.COD_CONTRIBUENTE;
        //        myAvviso.ImportoTotale = impAvviso;
        //        myAvviso.Ruoli = (Ruolo[])listRuoli.ToArray(typeof(Ruolo));
        //        //valorizzo l'array degli avvisi
        //        listAvvisi.Add(myAvviso);
        //        if (!utenti.ContainsKey(myDich.AnagraficaContribuente.COD_CONTRIBUENTE))
        //            utenti.Add(myDich.AnagraficaContribuente.COD_CONTRIBUENTE, myDich.AnagraficaContribuente.COD_CONTRIBUENTE);
        //        //azzero per ripartire da capo
        //        myDich = new DichiarazioneTosapCosap();
        //        listArticoli = new ArrayList();
        //        myTestata = new DichiarazioneTosapCosapTestata();
        //        myAnag = new DettaglioAnagrafica();
        //        myAvviso = new Cartella();
        //        listRuoli = new ArrayList();
        //        impAvviso = 0;
        //        //inserisco la dichiarazione
        //        Log.Debug("inserisco la dichiarazione");
        //        foreach (DichiarazioneTosapCosap myItem in listDichiarazioni)
        //        {
        //            myItem.TestataDichiarazione.NDichiarazione = MetodiDichiarazioneTosapCosap.GetNDichAutomatico(_ConnString, _IdEnte, _CodTributo);
        //            myItem.IdDichiarazione = MetodiDichiarazioneTosapCosap.SetDichiarazione(_ConnString, myItem);
        //        }
        //        Log.Debug("inserisco l'avviso");
        //        //inserisco l'avviso
        //        foreach (Cartella myItem in listAvvisi)
        //        {
        //            Cartella oSingleAvviso = myItem;
        //            oSingleAvviso.CodiceCartella = MetodiCartella.GetNAvvisoAutomatico(_ConnString, _IdEnte, _CodTributo, sAnnoAvviso);
        //            oSingleAvviso.IdCartella = -1;
        //            oSingleAvviso.DataEmissione = DateTime.Now;
        //            MetodiCartella.InsertCartella(ref oSingleAvviso, ref conn, false, 0, _Operatore);
        //        }
        //        Log.Debug("valorizzo il flusso ruolo elaborato");
        //        //valorizzo il flusso ruolo elaborato
        //        ElaborazioneEffettuata elab = new ElaborazioneEffettuata();
        //        elab.IdEnte = _IdEnte;
        //        elab.IdTributo = _CodTributo;
        //        elab.Anno = int.Parse(sAnnoAvviso);
        //        elab.TipoRuolo = (int)Ruolo.E_TIPO.ORDINARIO;
        //        elab.DataOraDocumentiStampati = DichiarazioneSession.MyDateMinValue;
        //        elab.DataOraDocumentiApprovati = DichiarazioneSession.MyDateMinValue;
        //        elab.DataOraMinutaApprovata = DichiarazioneSession.MyDateMinValue;
        //        elab.DataOraMinutaStampata = DichiarazioneSession.MyDateMinValue;
        //        elab.DataOraCalcoloRate = DichiarazioneSession.MyDateMinValue;
        //        elab.ImportoTotale = impRuolo;
        //        elab.NArticoli = nArticoliRuolo;
        //        elab.DataOraInizioElaborazione = DateTime.Now;
        //        elab.SogliaMinimaRate = 0;
        //        elab.DataOraFineElaborazione = DateTime.Now;
        //        elab.Note = "";
        //        elab.NUtenti = utenti.Keys.Count;
        //        elab.NDichiarazioni = utenti.Keys.Count;
        //        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref elab, ref conn);

        //        // Inserimento lotto
        //        Lotto l = new Lotto();
        //        l.IdEnte = _IdEnte;
        //        l.Anno = int.Parse(sAnnoAvviso);
        //        MetodiLotto.InsertLotto(l, ref conn);

        //        dao.CommitImportTransaction(ref conn);
        //    }
        //    catch (Exception Err)
        //    {
        //        //dao.RollbackCalcoloMassivoTransaction(ref _dbEngine);
        //        dao.RollbackImportTransaction(ref conn);
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.clsImportFlussi.StartImportThreadEntryPoint.errore: ", Err);
        //    }
        //    finally
        //    {
        //        connCmd.Connection.Close();
        //        CacheManager.RemoveImportInCorso();
        //    }
        //}
    }
}