using System;
using System.Threading;
using System.Collections;
using log4net;
using DTO;
using DAO;
using IRemInterfaceOSAP;
using OPENUtility;
using System.Data.SqlClient;

namespace OPENgovTOCO
{
	/// <summary>
	/// Classe per la gestione rate.
	/// </summary>
	public class CalcoloRateOSAP
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CalcoloRateOSAP));
		private int _Anno;
		private string _IdEnte;
        private string _IdTributo;
		private ElaborazioneEffettuata _Elab;
		private Rate[] _ConfigRate;

        /*
		private DBEngine _dbEngine;
		private DBEngine _dbEngineNoTrans;
        */

        private SqlCommand conn = new SqlCommand();
        private SqlCommand connCmd = new SqlCommand();

		private objContoCorrente _ContoCorrente;
		private Lotto _Lotto;
        /**** 201810 - Calcolo Puntuale ****/
        private bool _isSimula = false;
        private string _Username = "";

        public CalcoloRateOSAP()
		{
		}
        /// <summary>
        /// Metodo che richiama il calcolo rate.
        /// Inizio la transazione e recupero l'istanza della connessione al db. Devo farlo qui visto che sto eseguendo codice ancora nel thread della PostBack da FE, per cui ho il HttpContext disponibile con tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre nel thread non ho queste informazioni
        /// </summary>
        /// <param name="Anno"></param>
        /// <param name="IdEnte"></param>
        /// <param name="Elab"></param>
        /// <param name="ConfigRate"></param>
        /// <param name="Username"></param>
        /// <param name="CC"></param>
        /// <param name="IdTributo"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public void StartCalcolo(int Anno, string IdEnte, ElaborazioneEffettuata Elab, Rate[] ConfigRate, string Username, objContoCorrente CC, string IdTributo)
        {
            try
            {
                Log.Debug("CalcoloRateTOCO::StartCalcolo::inizio thread");
                ThreadStart threadDelegate = new ThreadStart(this.StartCalcoloRateThreadEntryPoint);
                Thread t = new Thread(threadDelegate);

                _Anno = Anno;
                _IdEnte = IdEnte;
                _Elab = Elab;
                _ConfigRate = ConfigRate;
                _IdTributo = IdTributo;
                _ContoCorrente = CC;
                _Username = Username;

                CartelleDAO dao = new CartelleDAO();

                conn = dao.StartCalcoloMassivoTransaction();
                connCmd = dao.OpenCalcoloMassivoConnection();

                _Lotto = MetodiLotto.GetLotto(IdEnte, Anno, -1, ref connCmd);

                t.Start();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.StartCalcolo.errore: ", Err);

            }
        }
        //public void StartCalcolo (int Anno, string IdEnte, ElaborazioneEffettuata Elab,			Rate[] ConfigRate, string Username, objContoCorrente CC,string IdTributo)
        //{
        //          try {
        //              Log.Debug("CalcoloRateTOCO::StartCalcolo::inizio thread");
        //              ThreadStart threadDelegate = new ThreadStart(this.StartCalcoloRateThreadEntryPoint);
        //              Thread t = new Thread(threadDelegate);

        //              _Anno = Anno;
        //              _IdEnte = IdEnte;
        //              _Elab = Elab;
        //              _ConfigRate = ConfigRate;
        //              _IdTributo = IdTributo;
        //              _ContoCorrente = CC;

        //              // Inizio la transazione e recupero l'istanza della connessione al db
        //              // Devo farlo qui visto che sto eseguendo codice ancora nel thread
        //              // della PostBack da FE, per cui ho il HttpContext disponibile con
        //              // tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre
        //              // nel thread non ho queste informazioni
        //              CartelleDAO dao = new CartelleDAO();

        //              //_dbEngine = dao.StartCalcoloMassivoTransaction ();
        //              conn = dao.StartCalcoloMassivoTransaction();

        //              //_dbEngineNoTrans = dao.OpenCalcoloMassivoConnection ();
        //              connCmd = dao.OpenCalcoloMassivoConnection();

        //              _Lotto = MetodiLotto.GetLotto(IdEnte,Anno, -1,ref connCmd);

        //              t.Start();
        //          }
        //          catch (Exception Err)
        //          {
        //              Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.StartCalcolo.errore: ", Err);

        //          }
        //      }
        //*** 20130610 - ruolo supplettivo ***
        //private void StartCalcoloRateThreadEntryPoint ()
        //{
        //    CacheManager.SetCalcoloMassivoInCorso (_Anno);

        //    CartelleDAO dao = new CartelleDAO();

        //    try
        //    {
        //        bool Error = false;
        //        string ErrorMessage = string.Empty;
        //        Hashtable utenti = new Hashtable ();

        //        Rate SolUnica = null;

        //        for (int i = 0; i < _ConfigRate.Length && SolUnica == null; i++)
        //        {
        //            if (_ConfigRate [i].NRata.CompareTo ("U") == 0)
        //                SolUnica = _ConfigRate [i];
        //        }

        //        CartellaSearch objSearch = new CartellaSearch ();
        //        //objSearch.Anno = _Anno;
        //        objSearch.IdFlusso=_Elab.IdFlusso;
        //        objSearch.IdEnteNoSession = _IdEnte;
        //        objSearch.CodContribuenti = string.Empty;
        //        objSearch.CodFiscaleContribuente = string.Empty;
        //        objSearch.CognomeContribuente = string.Empty;
        //        objSearch.NomeContribuente = string.Empty;
        //        objSearch.NumeroAvviso = string.Empty;
        //        objSearch.PIVAContribuente = string.Empty;

        //        // Recupero gli ID di tutte le dichiarazioni da elaborare
        //        Cartella[] ListaCartelle = MetodiCartella.CartelleSearch (objSearch, _dbEngineNoTrans);
        //        for (int i = 0; i < ListaCartelle.Length && !Error; i++)
        //        {
        //            Cartella c = ListaCartelle [i];

        //            c.CodiceCartella = GetCodiceCartella (_Lotto.CodiceConcessione, _Lotto.PrimaCartella + i);

        //            MetodiCartella.SetCodiceCartella (c, ref _dbEngine);

        //            // Unica soluzione
        //            Rata rUnica = CreaRata (c, SolUnica, c.ImportoCarico, _ContoCorrente.ContoCorrente);
        //            //MetodiRata.InsertRata (rUnica, ref _dbEngine);
        //            MetodiRata.InsertRata(rUnica, ref _dbEngine);

        //            if (c.ImportoCarico >= _Elab.SogliaMinimaRate)
        //            {
        //                // Rate
        //                double ImportoRataCalcolato = c.ImportoCarico / (_ConfigRate.Length - 1);
        //                ImportoRataCalcolato = double.Parse (string.Format("{0:0.00}", ImportoRataCalcolato));
        //                for (int j = 1; j < _ConfigRate.Length; j++)
        //                {
        //                    double ImportoRataEsatto;
        //                    if (j == (_ConfigRate.Length - 1))
        //                        ImportoRataEsatto = CalcolaImportoRataArrotondato (ImportoRataCalcolato, c.ImportoCarico);
        //                    else
        //                        ImportoRataEsatto = ImportoRataCalcolato;

        //                    Rata r = CreaRata (c, _ConfigRate [j], ImportoRataEsatto, _ContoCorrente.ContoCorrente);
        //                    MetodiRata.InsertRata (r, ref _dbEngine);
        //                }
        //            }

        //        } // end for (i < ListaCartelle.Length)

        //        _Elab.DataOraCalcoloRate = DateTime.Now;
        //        _Elab.Note = string.Empty;

        //        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata (_Elab, ref _dbEngine);

        //        if (!Error)
        //        {
        //            dao.CommitCalcoloMassivoTransaction (ref _dbEngine);
        //        }
        //        else
        //        {
        //            dao.RollbackCalcoloMassivoTransaction (ref _dbEngine);
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        dao.RollbackCalcoloMassivoTransaction (ref _dbEngine);

        //       Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.StartCalcoloRateThreadEntryPoint.errore: ", Err);

        //        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata (_Elab, ref _dbEngineNoTrans);
        //         
        //    }
        //    finally
        //    {
        //        _dbEngineNoTrans.CloseConnection ();
        //    }
        //    CacheManager.RemoveCalcoloMassivoInCorso ();
        //}
        //*** ***
        /**** 201810 - Calcolo Puntuale ****/
        /// <summary>
        /// Metodo sincrono che richiama il calcolo rate.
        /// </summary>
        /// <param name="Simula"></param>
        /// <param name="Anno"></param>
        /// <param name="IdEnte"></param>
        /// <param name="Elab"></param>
        /// <param name="ConfigRate"></param>
        /// <param name="Username"></param>
        /// <param name="CC"></param>
        /// <param name="IdTributo"></param>
        public void CalcoloSync(bool Simula, int Anno, string IdEnte, ElaborazioneEffettuata Elab, Rate[] ConfigRate, string Username, objContoCorrente CC, string IdTributo)
        {
            try
            {
                _Anno = Anno;
                _IdEnte = IdEnte;
                _Elab = Elab;
                _ConfigRate = ConfigRate;
                _IdTributo = IdTributo;
                _ContoCorrente = CC;
                _isSimula = Simula;
                _Username = Username;

                CartelleDAO dao = new CartelleDAO();

                conn = dao.StartCalcoloMassivoTransaction();
                connCmd = dao.OpenCalcoloMassivoConnection();

                _Lotto = MetodiLotto.GetLotto(IdEnte, Anno, -1, ref connCmd);
                StartCalcoloRateThreadEntryPoint();
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.CalcoloSync.errore: ", Err);
            }
        }
        //public void CalcoloSync(bool Simula,int Anno, string IdEnte, ElaborazioneEffettuata Elab, Rate[] ConfigRate, string Username, objContoCorrente CC, string IdTributo)
        //{
        //    try
        //    {
        //        _Anno = Anno;
        //        _IdEnte = IdEnte;
        //        _Elab = Elab;
        //        _ConfigRate = ConfigRate;
        //        _IdTributo = IdTributo;
        //        _ContoCorrente = CC;
        //        _isSimula = Simula;

        //        CartelleDAO dao = new CartelleDAO();

        //        //_dbEngine = dao.StartCalcoloMassivoTransaction ();
        //        conn = dao.StartCalcoloMassivoTransaction();

        //        //_dbEngineNoTrans = dao.OpenCalcoloMassivoConnection ();
        //        connCmd = dao.OpenCalcoloMassivoConnection();

        //        _Lotto = MetodiLotto.GetLotto(IdEnte, Anno, -1, ref connCmd);
        //        StartCalcoloRateThreadEntryPoint();
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.CalcoloSync.errore: ", Err);
        //    }
        //}
        /// <summary>
        /// Metodo che effettua il calcolo delle rate.
        /// se sono in scuole devo solo suddividere in rate perché il codice cartella ce l'ho già quindi:
        /// - elimino le rate precedentemente calcolate; ricalcolo le rate
        /// altrimenti:
        ///  - recupero gli ID di tutte le dichiarazioni da elaborare; calcolo codice cartella; creo l'unica soluzione; calcolo le rate.
        /// </summary>
        /// <revisionHistory>
        /// <revision date="10/2018">
        /// <strong>Calcolo Puntuale</strong>
        /// </revision>
        /// </revisionHistory>
        private void StartCalcoloRateThreadEntryPoint()
        {
            CacheManager.SetCalcoloMassivoInCorso(_Anno);

            CartelleDAO dao = new CartelleDAO();

            try
            {
                bool Error = false;
                string ErrorMessage = string.Empty;
                Hashtable utenti = new Hashtable();

                Rate SolUnica = null;
                if (_IdTributo == Utility.Costanti.TRIBUTO_SCUOLE)
                {
                    MetodiRata.CalcolaRate(3, _Elab.IdFlusso, -1, _Elab.SogliaMinimaRate, 0, ref conn);
                    MetodiRata.CalcolaRate(4, _Elab.IdFlusso, -1, _Elab.SogliaMinimaRate, 0, ref conn);
                }
                else
                {
                    checked
                    {
                        for (int i = 0; i < _ConfigRate.Length && SolUnica == null; i++)
                        {
                            if (_ConfigRate[i].NRata.CompareTo("U") == 0)
                                SolUnica = _ConfigRate[i];
                        }
                    }
                    CartellaSearch objSearch = new CartellaSearch();
                    objSearch.IdFlusso = _Elab.IdFlusso;
                    objSearch.IdEnteNoSession = _IdEnte;
                    objSearch.IdTributo = _IdTributo;
                    objSearch.CodFiscaleContribuente = string.Empty;
                    objSearch.CognomeContribuente = string.Empty;
                    objSearch.NomeContribuente = string.Empty;
                    objSearch.NumeroAvviso = string.Empty;
                    objSearch.PIVAContribuente = string.Empty;

                    Cartella[] ListaCartelle = MetodiCartella.CartelleSearch(objSearch, connCmd);
                    if (!Error)
                    {
                        int i = 0;
                        foreach (Cartella c in ListaCartelle)
                        {
                            CacheManager.SetAvanzamentoElaborazione("Posizione " + i.ToString() + " di " + ListaCartelle.Length.ToString());
                            c.CodiceCartella = GetCodiceCartella(_Lotto.CodiceConcessione, _Anno.ToString(), _Lotto.PrimaCartella + i);

                            if (!_isSimula)
                                MetodiCartella.SetCodiceCartella(c, ref conn);

                            Rata rUnica = CreaRata(c, SolUnica, c.ImportoCarico, _ContoCorrente.ContoCorrente);
                            if (!_isSimula)
                                MetodiRata.InsertRata(rUnica, ref conn);

                            if (c.ImportoCarico >= _Elab.SogliaMinimaRate)
                            {
                                double ImportoRataCalcolato = c.ImportoCarico / (_ConfigRate.Length - 1);
                                ImportoRataCalcolato = double.Parse(string.Format("{0:0.00}", ImportoRataCalcolato));
                                checked
                                {
                                    for (int j = 1; j < _ConfigRate.Length; j++)
                                    {
                                        double ImportoRataEsatto;
                                        if (j == (_ConfigRate.Length - 1))
                                            ImportoRataEsatto = CalcolaImportoRataArrotondato(ImportoRataCalcolato, c.ImportoCarico);
                                        else
                                            ImportoRataEsatto = ImportoRataCalcolato;

                                        Rata r = CreaRata(c, _ConfigRate[j], ImportoRataEsatto, _ContoCorrente.ContoCorrente);
                                        if (!_isSimula)
                                            MetodiRata.InsertRata(r, ref conn);
                                    }
                                }
                            }
                            i++;
                        } 
                    }
                }
                _Elab.DataOraCalcoloRate = DateTime.Now;
                _Elab.Note = string.Empty;

                if (!_isSimula)
                    MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref _Elab, ref conn, _Username);
                if (!Error)
                {
                    dao.CommitCalcoloMassivoTransaction(ref conn);
                }
                else
                {
                    dao.RollbackCalcoloMassivoTransaction(ref conn);
                }
            }
            catch (Exception ex)
            {
                dao.RollbackCalcoloMassivoTransaction(ref conn);
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.StartCalcolo.errore: ", ex);

                MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref _Elab, ref connCmd,_Username);
            }
            finally
            {
                connCmd.Connection.Close();
            }
            CacheManager.RemoveCalcoloMassivoInCorso();
            CacheManager.RemoveAvanzamentoElaborazione();
        }
        //private void StartCalcoloRateThreadEntryPoint()
        //{
        //    CacheManager.SetCalcoloMassivoInCorso(_Anno);

        //    CartelleDAO dao = new CartelleDAO();

        //    try
        //    {
        //        bool Error = false;
        //        string ErrorMessage = string.Empty;
        //        Hashtable utenti = new Hashtable();

        //        Rate SolUnica = null;
        //        //se sono in scuole devo solo suddividere in rate perché il codice cartella ce l'ho già
        //        if (_IdTributo ==Utility.Costanti.TRIBUTO_SCUOLE)
        //        {
        //            //elimino le rate precedentemente calcolate
        //            MetodiRata.CalcolaRate(3, _Elab.IdFlusso, -1, _Elab.SogliaMinimaRate, 0, ref conn);
        //            //ricalcolo le rate
        //            MetodiRata.CalcolaRate(4, _Elab.IdFlusso, -1, _Elab.SogliaMinimaRate, 0, ref conn);
        //        }
        //        else
        //        {
        //            checked
        //            {
        //                for (int i = 0; i < _ConfigRate.Length && SolUnica == null; i++)
        //                {
        //                    if (_ConfigRate[i].NRata.CompareTo("U") == 0)
        //                        SolUnica = _ConfigRate[i];
        //                }
        //            }
        //            CartellaSearch objSearch = new CartellaSearch();
        //            //objSearch.Anno = _Anno;
        //            objSearch.IdFlusso = _Elab.IdFlusso;
        //            objSearch.IdEnteNoSession = _IdEnte;
        //            objSearch.IdTributo = _IdTributo;
        //            //objSearch.CodContribuenti = string.Empty;
        //            objSearch.CodFiscaleContribuente = string.Empty;
        //            objSearch.CognomeContribuente = string.Empty;
        //            objSearch.NomeContribuente = string.Empty;
        //            objSearch.NumeroAvviso = string.Empty;
        //            objSearch.PIVAContribuente = string.Empty;

        //            // Recupero gli ID di tutte le dichiarazioni da elaborare
        //            //Cartella[] ListaCartelle = MetodiCartella.CartelleSearch(objSearch, _dbEngineNoTrans);
        //            Cartella[] ListaCartelle = MetodiCartella.CartelleSearch(objSearch, connCmd);
        //            if (!Error)
        //            {
        //                int i = 0;
        //                foreach (Cartella c in ListaCartelle)
        //                {
        //                    CacheManager.SetAvanzamentoElaborazione("Posizione " + i.ToString() + " di " + ListaCartelle.Length.ToString());
        //                    c.CodiceCartella = GetCodiceCartella(_Lotto.CodiceConcessione, _Anno.ToString(), _Lotto.PrimaCartella + i);

        //                    //MetodiCartella.SetCodiceCartella(c, ref _dbEngine);
        //                    /**** 201810 - Calcolo Puntuale ****/
        //                    if (!_isSimula)
        //                        MetodiCartella.SetCodiceCartella(c, ref conn);

        //                    // Unica soluzione
        //                    Rata rUnica = CreaRata(c, SolUnica, c.ImportoCarico, _ContoCorrente.ContoCorrente);
        //                    //MetodiRata.InsertRata (rUnica, ref _dbEngine);
        //                    /**** 201810 - Calcolo Puntuale ****/
        //                    if (!_isSimula)
        //                        MetodiRata.InsertRata(rUnica, ref conn);

        //                    if (c.ImportoCarico >= _Elab.SogliaMinimaRate)
        //                    {
        //                        // Rate
        //                        double ImportoRataCalcolato = c.ImportoCarico / (_ConfigRate.Length - 1);
        //                        ImportoRataCalcolato = double.Parse(string.Format("{0:0.00}", ImportoRataCalcolato));
        //                        checked
        //                        {
        //                            for (int j = 1; j < _ConfigRate.Length; j++)
        //                            {
        //                                double ImportoRataEsatto;
        //                                if (j == (_ConfigRate.Length - 1))
        //                                    ImportoRataEsatto = CalcolaImportoRataArrotondato(ImportoRataCalcolato, c.ImportoCarico);
        //                                else
        //                                    ImportoRataEsatto = ImportoRataCalcolato;

        //                                Rata r = CreaRata(c, _ConfigRate[j], ImportoRataEsatto, _ContoCorrente.ContoCorrente);
        //                                //MetodiRata.InsertRata(r, ref _dbEngine);
        //                                /**** 201810 - Calcolo Puntuale ****/
        //                                if (!_isSimula)
        //                                    MetodiRata.InsertRata(r, ref conn);
        //                            }
        //                        }
        //                    }
        //                    i++;
        //                } // end for (i < ListaCartelle.Length)
        //            }
        //        }
        //        _Elab.DataOraCalcoloRate = DateTime.Now;
        //        _Elab.Note = string.Empty;

        //        //MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(_Elab, ref _dbEngine);
        //        /**** 201810 - Calcolo Puntuale ****/
        //        if(!_isSimula)
        //        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref _Elab, ref conn);
        //        if (!Error)
        //        {
        //            //dao.CommitCalcoloMassivoTransaction(ref _dbEngine);
        //            dao.CommitCalcoloMassivoTransaction(ref conn);
        //        }
        //        else
        //        {
        //            //dao.RollbackCalcoloMassivoTransaction(ref _dbEngine);
        //            dao.RollbackCalcoloMassivoTransaction(ref conn);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //dao.RollbackCalcoloMassivoTransaction(ref _dbEngine);
        //        dao.RollbackCalcoloMassivoTransaction(ref conn);
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.StartCalcolo.errore: ", ex);

        //        //MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(_Elab, ref _dbEngineNoTrans);
        //        MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(ref _Elab, ref connCmd);
        //    }
        //    finally
        //    {
        //        connCmd.Connection.Close();
        //    }
        //    CacheManager.RemoveCalcoloMassivoInCorso();
        //    CacheManager.RemoveAvanzamentoElaborazione();
        //}

        private Rata CreaRata (Cartella cart, Rate ConfigRata, double Importo,
			string NumeroContoCorrente)
		{
            try {
                Rata r = new Rata();

                r.IdCartella = cart.IdCartella;
                r.NumeroRata = ConfigRata.NRata;
                r.ImportoRata = Importo;
                r.DescrizioneRata = ConfigRata.Descrizione;
                r.DataScadenza = ConfigRata.DataScadenza;
                r.NumeroContoCorrente = NumeroContoCorrente;
                r.CodiceBollettino = GetCodiceBollettino(ConfigRata.NRata, cart.CodiceCartella);
                r.Codeline = GetCodeline(r.CodiceBollettino, r.NumeroContoCorrente, r.ImportoRata);
                r.Barcode = GetBarcode(r.CodiceBollettino, r.ImportoRata, r.NumeroContoCorrente);

                return r;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.CreaRata.errore: ", Err);
                throw Err;
                
            }
        }

		// Sulla prima rata aggiungo eventuali centesimi
		// persi nella divisione ImportoCarico / NumeroRate
		private double CalcolaImportoRataArrotondato (double ImportoRataCalcolato, double ImportoCarico)
		{
            try {
                double ImportoTotaleReverse = ImportoRataCalcolato * (_ConfigRate.Length - 1);
                ImportoTotaleReverse = double.Parse(string.Format("{0:0.00}", ImportoTotaleReverse));

                double Resto = ImportoCarico - ImportoTotaleReverse;

                double ImportoCorretto = ImportoRataCalcolato + Resto;

                return double.Parse(string.Format("{0:0.00}", ImportoCorretto));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.CalcolaImportoRataArrontondato.errore: ", Err);
                throw Err;

            }
        }

		public string GetCodiceCartella (string sCodiceConcessione,string Anno, int nProgressivo)
        {
            try {
                string sCodiceCartella = sCodiceConcessione + Anno + nProgressivo.ToString().PadLeft(8, '0');

                long cin;

                cin = Int64.Parse(sCodiceCartella) % 93;

                sCodiceCartella += cin.ToString().PadLeft(2, '0');

                return sCodiceCartella;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.GetCodiceCartella.errore: ", Err);
                throw Err;

            }
        }

		private string GetBarcode (string sCodBollettino, double ImportoRata, string sContoCorrente)
		{
			try
			{
				string sLenIVCampo = "18";
				string sLenConto = "12";
				string sLenImporto = "10";
				string sLenTipoDoc = "3";
				string sTipoDoc = "896";

				string sImpRata = string.Format("{0:0.00}", ImportoRata).Replace (",", ".");
				sImpRata = sImpRata.Replace(".", "").PadLeft(10, '0');
				sContoCorrente = sContoCorrente.PadLeft(12, '0');
				
				string sDataBarcode = sLenIVCampo + sCodBollettino + sLenConto + sContoCorrente + sLenImporto +
					sImpRata + sLenTipoDoc + sTipoDoc;

				return sDataBarcode;
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.GetBarcode.errore: ", Err);
                throw Err;

            }
        }

		private string GetCodiceBollettino(string sNumeroRata, string sCodiceCartella)
		{
            try {
                // logica di popolamento del codice bollettino
                // ********************************************************************
                // 5 chr	codice ente
                // 2 chr	numero rata (Unica soluzione = '00', Prima rata = '01, Seconda rata = '02', ecc')
                // 2 chr	anno riferimento avviso di pagamento (ultimi due caratteri dell'anno)
                // 7 chr	numero avviso
                // 2 chr	caratteri di controllo
                // 2 chr	CIN (resto della divisione del codice bollettino fin'ora calcolato e 93)
                // ********************************************************************
                if (sNumeroRata.CompareTo("U") == 0)
                    sNumeroRata = "0";
                else
                    sNumeroRata = sNumeroRata.PadLeft(1, '0');

                long cin;

                string CodiceBollettino = _IdEnte.PadLeft(6, '0');
                CodiceBollettino += sNumeroRata;
                CodiceBollettino += _Anno.ToString().Substring(2, 2);
                CodiceBollettino += sCodiceCartella.Substring(8, 7);

                cin = Int64.Parse(CodiceBollettino) % 93;

                CodiceBollettino += cin.ToString().PadRight(2, '0');

                return CodiceBollettino;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.GetCodiceBollettino.errore: ", Err);
                throw Err;

            }
        }

		private string GetCodeline (string sCodiceBollettino, string sContoCorrente, double ImportoRata)
		{
            try {
                // La code line è così formata:
                // <codice bollettino>
                // 9 spazi
                // parte intera dell'importo formattato a 8 caratteri
                // +
                // parte decimale dell'importo>
                // conto corrente formattato a 11 caratteri (utilizzare spazi come riempimento)<
                // 2 spazi
                // 896>
                // ESEMPIO: <069920107000000177>         00000096+75>   73682981<  896>
                string sImportoRata = string.Format("{0:0.00}", ImportoRata).Replace(",", ".");

                string Codeline = "<" + sCodiceBollettino + ">";
                Codeline += "         ";
                Codeline += sImportoRata.Substring(0, sImportoRata.IndexOf(".")).PadLeft(8, '0');
                Codeline += "+" + sImportoRata.Substring(sImportoRata.IndexOf(".") + 1) + ">";
                Codeline += sContoCorrente.PadLeft(11, ' ');
                Codeline += "<  896>";

                return Codeline;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.CalcoloRateOSAP.GetCodeline.errore: ", Err);
                throw Err;

            }
        }
	}
}
