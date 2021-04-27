using System;
using System.Data;
using System.Collections;
using System.Web.Services;
using log4net;
using OggettiComuniStrade;
using IRemInterfaceOSAP;
using OPENgovTOCO;
using System.Data.SqlClient;
using Utility;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione Articolo
    /// </summary>
    public class MetodiArticolo
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Articolo));
        //		private int _idArticolo = -1;
        //		private int _idDichiarazione=-1;
        //		private string _idTributo="";
        //		private string _sVia = String.Empty;
        //		private int _codVia = -1;
        //		private int _civico = -1;
        //		private string _esponente = String.Empty;
        //		private string _interno = String.Empty;
        //		private string _scala = String.Empty;
        //		private Categorie _categoria;
        //		private TipologieOccupazioni _tipologiaOccupazione;
        //		private double _consistenza = 0;
        //		private TipoConsistenza _tipoConsistenza;
        //		private DateTime _dataInizioOccupazione;
        //		private DateTime _dataFineOccupazione;
        //		private Durata _tipoDurata;
        //		private int _durataOccupazione;
        //		private double _maggiorazioneImporto;
        //		private double _maggiorazionePerc;
        //		private string _note = String.Empty;
        //		private Agevolazione[] _agevolazione;
        //		private double _detrazioneImporto;
        //		private bool _attrazione;
        //		private string _Operatore;
        //		private DateTime _dataInserimento;
        //		private DateTime _dataVariazione;
        //		private DateTime _dataCessazione;
        //
        //		public int IdArticolo
        //		{
        //			get { return _idArticolo; }
        //			set { _idArticolo = value; }
        //		}
        //
        //		public int IdDichiarazione
        //		{
        //			get { return _idDichiarazione; }
        //			set { _idDichiarazione = value; }
        //		}
        //		public string IdTributo
        //		{
        //			get { return _idTributo; }
        //			set { _idTributo = value; }
        //		}
        //		public string SVia
        //		{
        //			get { return _sVia; }
        //			set { _sVia = value; }
        //		}
        //
        //		public int CodVia
        //		{
        //			get { return _codVia; }
        //			set { _codVia = value; }
        //		}
        //
        //		public int Civico
        //		{
        //			get { return _civico; }
        //			set { _civico = value; }
        //		}
        //
        //		public string Esponente
        //		{
        //			get { return _esponente; }
        //			set { _esponente = value; }
        //		}
        //
        //		public string Interno
        //		{
        //			get { return _interno; }
        //			set { _interno = value; }
        //		}
        //
        //		public string Scala
        //		{
        //			get { return _scala; }
        //			set { _scala = value; }
        //		}
        //
        //		public DateTime DataInizioOccupazione
        //		{
        //			get { return _dataInizioOccupazione; }
        //			set { _dataInizioOccupazione = value; }
        //		}
        //
        //		public DateTime DataFineOccupazione
        //		{
        //			get { return _dataFineOccupazione; }
        //			set { _dataFineOccupazione = value; }
        //		}
        //
        //		public Durata TipoDurata
        //		{
        //			get { return _tipoDurata; }
        //			set { _tipoDurata = value; }
        //		}
        //
        //		public int DurataOccupazione
        //		{
        //			get { return _durataOccupazione; }
        //			set { _durataOccupazione = value; }
        //		}
        //
        //		public double Consistenza
        //		{
        //			get { return _consistenza; }
        //			set { _consistenza = value; }
        //		}
        //
        //		public TipoConsistenza TipoConsistenzaTOCO
        //		{
        //			get { return _tipoConsistenza; }
        //			set { _tipoConsistenza = value; }
        //		}
        //
        //		public string Note
        //		{
        //			get { return _note; }
        //			set { _note = value; }
        //		}
        //
        //		public double MaggiorazioneImporto
        //		{
        //			get { return _maggiorazioneImporto; }
        //			set { _maggiorazioneImporto = value; }
        //		}
        //
        //		public double MaggiorazionePerc
        //		{
        //			get { return _maggiorazionePerc; }
        //			set { _maggiorazionePerc = value; }
        //		}
        //
        //		public TipologieOccupazioni TipologiaOccupazione
        //		{
        //			get { return _tipologiaOccupazione; }
        //			set { _tipologiaOccupazione = value; }
        //		}
        //
        //		public Categorie Categoria
        //		{
        //			get { return _categoria; }
        //			set { _categoria = value; }
        //		}
        //
        //		public Agevolazione[] AgevolazioneTOCO
        //		{
        //			get { return _agevolazione; }
        //			set { _agevolazione = value; }
        //		}
        //
        //		public double DetrazioneImporto
        //		{
        //			get { return _detrazioneImporto; }
        //			set { _detrazioneImporto = value; }
        //		}
        //
        //		public bool Attrazione
        //		{
        //			get { return _attrazione; }
        //			set { _attrazione = value; }
        //		}
        //		public string Operatore
        //		{
        //			get { return _Operatore; }
        //			set { _Operatore = value; }
        //		}
        //		public DateTime DataInserimento
        //		{
        //			get { return _dataInserimento; }
        //			set { _dataInserimento = value; }
        //		}
        //		public DateTime DataVariazione
        //		{
        //			get { return _dataVariazione; }
        //			set { _dataVariazione = value; }
        //		}
        //		public DateTime DataCessazione
        //		{
        //			get { return _dataCessazione; }
        //			set { _dataCessazione = value; }
        //		}
        //
        #region "Public methods"
        public static Articolo GetArticolo(string ConnectionString, int IdRuolo, string IdEnte, string CodTributo)
        {
            DAO.DichiarazioniDAO DAO = new DAO.DichiarazioniDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetArticolo(IdRuolo);
                Articolo[] arts = FillArticoloFromDataTable(ConnectionString, dt, IdEnte, CodTributo,IdRuolo, false);

                Articolo myArt = new Articolo();
                if (arts.Length > 0)
                    myArt = arts[0];

                return myArt;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiArticolo.GetArticolo.errore: ", Err);
                throw (Err);
            }
        }

        //*** 20130610 - ruolo supplettivo

        //public static CalcoloResult GetArticoloPrecedente(string IdEnte, int IdArticolo, int Anno, ref DBEngine dbEngine_)
        //{
        //    DAO.DichiarazioniDAO DAO = new DAO.DichiarazioniDAO();

        //    try 
        //    {
        //        DataTable dt = null;
        //        dt = DAO.GetArticoloPrecedente(IdEnte, IdArticolo, Anno, ref dbEngine_);
        //        CalcoloResult  myArt= FillArticoloPrecedenteFromDataTable(dt,IdEnte);

        //        return myArt;
        //    } 
        //    catch (Exception Err) 
        //    {
        //      Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiArticolo.GetArticoloPrecedente.errore: ", Err);
        //        throw Err;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="ListOccupazioni"></param>
        /// <param name="cmdMyCommand"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="10/2018">
        /// <strong>Calcolo Puntuale</strong>
        /// </revision>
        /// <revision date="24/03/2020">In caso di supplettivo devo considerare solo l'avviso precedente per le stesse tipologie di occupazioni</revision>
        /// </revisionHistory>
        public static CalcoloResult GetArticoloPrecedente(string IdEnte, int IdContribuente, int Anno, string ListOccupazioni, ref SqlCommand cmdMyCommand)
        {
            DAO.DichiarazioniDAO DAO = new DAO.DichiarazioniDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetArticoloPrecedente(IdEnte, IdContribuente, Anno, ListOccupazioni, ref cmdMyCommand);
                CalcoloResult myArt = FillArticoloPrecedenteFromDataTable(dt, IdEnte);

                return myArt;
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.MetodiArticolo.GetArticoloPrecedente.errore: ", Err);
                throw (Err);
            }
        }
        //public static CalcoloResult GetArticoloPrecedente(string IdEnte, int IdContribuente, int Anno,int IdDichiarazione, ref SqlCommand cmdMyCommand)
        //{
        //    DAO.DichiarazioniDAO DAO = new DAO.DichiarazioniDAO();

        //    try
        //    {
        //        DataTable dt = null;
        //        dt = DAO.GetArticoloPrecedente(IdEnte, IdContribuente, Anno,IdDichiarazione, ref cmdMyCommand);
        //        CalcoloResult myArt = FillArticoloPrecedenteFromDataTable(dt, IdEnte);

        //        return myArt;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(IdEnte + " - OPENgovOSAP.MetodiArticolo.GetArticoloPrecedente.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEnte"></param>
        /// <returns></returns>
        public static ArrayList GetMyArray(string idEnte)
        {
            try
            {
                ArrayList MyArray = new ArrayList();
                Articolo CurrentItem;

                DichiarazioneTosapCosap objDichiarazione = DichiarazioneSession.SessionDichiarazioneTosapCosap;
                foreach (Articolo myRow in objDichiarazione.ArticoliDichiarazione)
                {
                   CurrentItem = new Articolo();
                    //Immobile Dichiarazione
                    CurrentItem.DataInserimento = myRow.DataInserimento;
                    CurrentItem.IdTributo = myRow.IdTributo;

                    CurrentItem.Civico = myRow.Civico;
                    CurrentItem.CodVia = myRow.CodVia;
                    CurrentItem.DataInizioOccupazione = myRow.DataInizioOccupazione;
                    //ARTCAVA 24/10/2012 - Ma se la data c'è perché non inserirla?
                    //Prima era: CurrentItem.DataFineOccupazione = myRow.DataInizioOccupazione;
                    if (myRow.DataFineOccupazione == DateTime.MaxValue)
                        CurrentItem.DataFineOccupazione = myRow.DataInizioOccupazione;
                    else
                        CurrentItem.DataFineOccupazione = myRow.DataFineOccupazione;
                    CurrentItem.Esponente = myRow.Esponente;
                    CurrentItem.IdArticolo = myRow.IdArticolo;
                    CurrentItem.Interno = myRow.Interno;
                    CurrentItem.MaggiorazionePerc = myRow.MaggiorazionePerc;
                    CurrentItem.MaggiorazioneImporto = myRow.MaggiorazioneImporto;
                    CurrentItem.Consistenza = myRow.Consistenza;

                    TipoConsistenza objTipoConsistenza = new TipoConsistenza();
                    objTipoConsistenza.IdTipoConsistenza = myRow.TipoConsistenzaTOCO.IdTipoConsistenza;
                    objTipoConsistenza.Descrizione = myRow.TipoConsistenzaTOCO.Descrizione;
                    CurrentItem.TipoConsistenzaTOCO = objTipoConsistenza;

                    CurrentItem.Note = myRow.Note;
                    CurrentItem.Scala = myRow.Scala;
                    CurrentItem.SVia = myRow.SVia;
                    //CurrentItem.Riduzioni = objDichiarazione.ImmobiliDichiarazione[i].Riduzioni;
                    //CurrentItem.IdDichiarazione = objDichiarazione.ImmobiliDichiarazione[i].IdDichiarazione;

                    Categorie objCategoria = new Categorie();
                    objCategoria.IdCategoria = myRow.Categoria.IdCategoria;
                    objCategoria.Descrizione = myRow.Categoria.Descrizione;
                    CurrentItem.Categoria = objCategoria;

                    TipologieOccupazioni objTipologiaOccupazione = new TipologieOccupazioni();
                    objTipologiaOccupazione.IdTipologiaOccupazione = myRow.TipologiaOccupazione.IdTipologiaOccupazione;
                    objTipologiaOccupazione.Descrizione = myRow.TipologiaOccupazione.Descrizione;
                    CurrentItem.TipologiaOccupazione = objTipologiaOccupazione;

                    CurrentItem.ListAgevolazioni = MetodiAgevolazione.GetAgevolazioniVSArticolo(DichiarazioneSession.StringConnection, myRow.IdArticolo,-1, idEnte, objDichiarazione.CodTributo);

                    CurrentItem.DurataOccupazione = myRow.DurataOccupazione;
                    Durata objDurata = new Durata();
                    objDurata.IdDurata = myRow.TipoDurata.IdDurata;
                    objDurata.Descrizione = myRow.TipoDurata.Descrizione;
                    CurrentItem.TipoDurata = objDurata;

                    CurrentItem.Attrazione = myRow.Attrazione;

                    CurrentItem.Operatore = myRow.Operatore;
                    CurrentItem.DataInserimento = myRow.DataInserimento;
                    //*** 20130610 - ruolo supplettivo ***
                    CurrentItem.IdArticoloPadre = myRow.IdArticoloPadre;
                    //Log.Debug("MetodiArticolo::GetMyArray::IdArticoloPadre::"+CurrentItem.IdArticoloPadre.ToString());
                    //*** ***

                    MyArray.Add(CurrentItem);
                }

                return MyArray;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.GetMyArray.GetArticolo.errore: ", Err);
                throw (Err);
            }
        }

        public static Articolo[] FillArticoloFromDataTable(string ConnectionString, DataTable dt, string sIdEnte, string CodTributo, int IdRuolo, bool IsForMotore)
        {
            ArrayList MyArray = new ArrayList();
            try
            {
                Articolo CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Articolo();

                    CurrentItem.IdArticolo = StringOperation.FormatInt(myRow["IDARTICOLO"]);
                    CurrentItem.IdDichiarazione = StringOperation.FormatInt(myRow["IDDICHIARAZIONE"]);
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    CurrentItem.DataInserimento = (DateTime)myRow["DATA_INSERIMENTO"];
                    CurrentItem.CodVia = StringOperation.FormatInt(myRow["CODVIA"]);
                    if (CurrentItem.CodVia > 0 && (string)myRow["VIA"] == string.Empty)
                        CurrentItem.SVia = MetodiArticolo.GetDescrizioneVia(CurrentItem.CodVia, sIdEnte);
                    else
                        CurrentItem.SVia = (string)myRow["VIA"];
                    if (myRow["CIVICO"] != DBNull.Value)
                        CurrentItem.Civico = StringOperation.FormatInt(myRow["CIVICO"]);
                    else
                        CurrentItem.Civico = -1;
                    CurrentItem.Esponente = StringOperation.FormatString(myRow["ESPONENTE"]);
                    CurrentItem.Interno = StringOperation.FormatString(myRow["INTERNO"]);
                    CurrentItem.Scala = StringOperation.FormatString(myRow["SCALA"]);
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::devo prelevare categoria");
                    CurrentItem.Categoria = MetodiCategorie.GetCategoria(ConnectionString, StringOperation.FormatInt(myRow["IDCATEGORIA"]), "", sIdEnte, CodTributo);
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::devo prelevare TipologiaOccupazione");
                    CurrentItem.TipologiaOccupazione = MetodiTipologieOccupazioni.GetTipologiaOccupazione(ConnectionString, StringOperation.FormatInt(myRow["IDTIPOLOGIAOCCUPAZIONE"]), "", sIdEnte, CodTributo);
                    CurrentItem.Consistenza = StringOperation.FormatDouble(myRow["CONSISTENZA"]);
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::devo prelevare TipoConsistenza");
                    CurrentItem.TipoConsistenzaTOCO = MetodiTipoConsistenza.GetTipoConsistenza(ConnectionString, StringOperation.FormatInt(myRow["IDTIPOCONSISTENZA"]));
                    CurrentItem.DataInizioOccupazione = (DateTime)myRow["DATAINIZIOOCCUPAZIONE"];
                    if (myRow["DATAFINEOCCUPAZIONE"] != DBNull.Value)
                        CurrentItem.DataFineOccupazione = (DateTime)myRow["DATAFINEOCCUPAZIONE"];
                    else
                        CurrentItem.DataFineOccupazione = DateTime.MaxValue;
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::devo prelevare durata");
                    CurrentItem.TipoDurata = MetodiDurata.GetDurata(ConnectionString, StringOperation.FormatInt(myRow["IDDURATA"]));
                    //*** 20130801 - accertamento OSAP ***
                    if (!IsForMotore)
                        CurrentItem.DurataOccupazione = StringOperation.FormatInt(myRow["DURATAOCCUPAZIONE"]);
                    else
                        CurrentItem.DurataOccupazione = StringOperation.FormatInt(myRow["DURATACALCOLO"]);
                    //*** ***
                    CurrentItem.MaggiorazionePerc = StringOperation.FormatDouble(myRow["MAGGIORAZIONE_PERC"]);
                    CurrentItem.MaggiorazioneImporto = StringOperation.FormatDouble(myRow["MAGGIORAZIONE_IMPORTO"]);
                    CurrentItem.Note = StringOperation.FormatString(myRow["NOTE"]);
                    CurrentItem.Operatore = StringOperation.FormatString(myRow["operatore"]);
                    if (myRow["DATA_INSERIMENTO"] != DBNull.Value)
                        CurrentItem.DataInserimento = (DateTime)myRow["DATA_INSERIMENTO"];
                    else
                        CurrentItem.DataInserimento = DateTime.Now;
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::devo prelevare categAgevolazioniVSArticolooria");
                    CurrentItem.ListAgevolazioni = MetodiAgevolazione.GetAgevolazioniVSArticolo(ConnectionString, StringOperation.FormatInt(myRow["IDARTICOLO"]),IdRuolo, sIdEnte, CodTributo);
                    CurrentItem.DetrazioneImporto = StringOperation.FormatDouble(myRow["DETRAZIONE_IMPORTO"].ToString());
                    CurrentItem.Attrazione = StringOperation.FormatBool(myRow["ATTRAZIONE"]);
                    //*** 20130610 - ruolo supplettivo ***
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::devo prelevare ElencoPercAgevolazioni");
                    CurrentItem.ElencoPercAgevolazioni = MetodiAgevolazione.GetElencoPercAgevolazioni(CurrentItem.ListAgevolazioni);
                    CurrentItem.IdArticoloPadre = StringOperation.FormatInt(myRow["IDARTICOLOPADRE"]);
                    //Log.Debug("MetodiArticolo::FillArticoloFromDataTable::IdArticoloPadre::"+CurrentItem.IdArticoloPadre.ToString());
                    //*** ***
                    MyArray.Add(CurrentItem);
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiArticolo.FillArticoloFromDataTable.errore: ", Err);

            }
            return (Articolo[])MyArray.ToArray(typeof(Articolo));
        }

        /// <summary>
        /// popolo oggetto con gli importi precedentemente calcolati
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sIdEnte"></param>
        /// <returns></returns>
        /// <revisionHistory><revision date="10/06/2013">ruolo supplettivo</revision></revisionHistory>
        public static CalcoloResult FillArticoloPrecedenteFromDataTable(DataTable dt, string sIdEnte)
        {
            try
            {
                CalcoloResult CurrentItem = null;
                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new CalcoloResult();

                    CurrentItem.ImportoCalcolato = StringOperation.FormatDouble(myRow["IMPORTO"]);
                    CurrentItem.ImportoLordo = StringOperation.FormatDouble(myRow["IMPORTO_LORDO"]);
                }
                return CurrentItem;
            }
            catch (Exception Err)
            {
                Log.Debug(sIdEnte + " - OPENgovOSAP.MetodiArticolo.FillArticoloPrecedenteFromDataTable.errore: ", Err);
                throw (Err);
            }
        }
        //public static CalcoloResult FillArticoloPrecedenteFromDataTable(DataTable dt, string sIdEnte)
        //{
        //    try
        //    {
        //        CalcoloResult CurrentItem = null;
        //        foreach (DataRow myRow in dt.Rows)
        //        {
        //            CurrentItem = new CalcoloResult();

        //            CurrentItem.ImportoCalcolato = StringOperation.FormatDouble(myRow["IMPORTO"].ToString());
        //            CurrentItem.ImportoLordo = StringOperation.FormatDouble(myRow["IMPORTO_LORDO"].ToString());
        //        }
        //        return CurrentItem;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(sIdEnte + " - OPENgovOSAP.MetodiArticolo.FillArticoloPrecedenteFromDataTable.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        //*** ***
        //*** 20130801 - accertamento OSAP ***
        //		public static Articolo[] FillArticoloFromDataTableForMotore(DataTable dt,string IdEnte, ref DBEngine dbEngine_)
        //		{
        //        try{
        //			ArrayList MyArray = new ArrayList();
        //			Articolo CurrentItem = null;
        //
        //			for (int i = 0; i < dt.Rows.Count; i++) 
        //			{
        //				CurrentItem = new Articolo();
        //
        //				CurrentItem.IdArticolo = StringOperation.FormatInt(dt.Rows[i]["IDARTICOLO"];
        //				CurrentItem.Dichiarazione = new DichiarazioneTosapCosap ();
        //				CurrentItem.Dichiarazione.IdDichiarazione = StringOperation.FormatInt(dt.Rows[i]["IDDICHIARAZIONE"];
        //				CurrentItem.CodVia = StringOperation.FormatInt(dt.Rows[i]["CODVIA"];
        //
        //				//if (CurrentItem.SVia.CompareTo ("") != 0)
        //				CurrentItem.SVia = MetodiArticolo.GetDescrizioneVia (CurrentItem.CodVia, IdEnte);
        //
        //				if (dt.Rows[i]["CIVICO"] != DBNull.Value)
        //					CurrentItem.Civico = StringOperation.FormatInt(dt.Rows[i]["CIVICO"];
        //				else
        //					CurrentItem.Civico = -1;
        //			
        //				CurrentItem.Esponente = StringOperation.FormatString(dt.Rows[i]["ESPONENTE"]);
        //				CurrentItem.Interno = StringOperation.FormatString(dt.Rows[i]["INTERNO"]);
        //				CurrentItem.Scala = StringOperation.FormatString(dt.Rows[i]["SCALA"]);
        //
        //				CurrentItem.Categoria = new Categorie ();
        //				CurrentItem.Categoria.IdCategoria = StringOperation.FormatInt(dt.Rows[i]["IDCATEGORIA"];
        //				CurrentItem.TipologiaOccupazione = new TipologieOccupazioni ();
        //				CurrentItem.TipologiaOccupazione.IdTipologiaOccupazione = StringOperation.FormatInt(dt.Rows[i]["IDTIPOLOGIAOCCUPAZIONE"];
        //				CurrentItem.Consistenza = StringOperation.FormatDouble (dt.Rows[i]["CONSISTENZA"].ToString ());
        //				CurrentItem.TipoConsistenzaTOCO = new TipoConsistenza ();
        //				CurrentItem.TipoConsistenzaTOCO.IdTipoConsistenza = StringOperation.FormatInt(dt.Rows[i]["IDTIPOCONSISTENZA"];
        //				CurrentItem.DataInizioOccupazione = (DateTime)dt.Rows[i]["DATAINIZIOOCCUPAZIONE"];
        //				if (dt.Rows[i]["DATAFINEOCCUPAZIONE"] != DBNull.Value)
        //					CurrentItem.DataFineOccupazione = (DateTime)dt.Rows[i]["DATAFINEOCCUPAZIONE"];
        //				else
        //					CurrentItem.DataFineOccupazione = DateTime.MaxValue;
        //				CurrentItem.TipoDurata = new Durata ();
        //				CurrentItem.TipoDurata.IdDurata = StringOperation.FormatInt(dt.Rows[i]["IDDURATA"];
        //				CurrentItem.DurataOccupazione = StringOperation.FormatInt(dt.Rows[i]["DURATACALCOLO"];
        //				CurrentItem.MaggiorazionePerc = StringOperation.FormatDouble (dt.Rows[i]["MAGGIORAZIONE_PERC"].ToString ());
        //				CurrentItem.MaggiorazioneImporto = StringOperation.FormatDouble (dt.Rows[i]["MAGGIORAZIONE_IMPORTO"].ToString ());
        //				CurrentItem.Note = StringOperation.FormatString(dt.Rows[i]["NOTE"]);
        //
        //				CurrentItem.ListAgevolazioni = MetodiAgevolazione.GetAgevolazioniVSArticolo(IdEnte, StringOperation.FormatInt(dt.Rows[i]["IDARTICOLO"], ref dbEngine_);
        //				
        //				CurrentItem.DetrazioneImporto = StringOperation.FormatDouble (dt.Rows[i]["DETRAZIONE_IMPORTO"].ToString());
        //				CurrentItem.Attrazione = StringOperation.FormatBool(dt.Rows[i]["ATTRAZIONE"];
        //				//*** 20130610 - ruolo supplettivo ***
        //				CurrentItem.IdArticoloPadre = StringOperation.FormatInt(dt.Rows[i]["IDARTICOLOPADRE"];
        //				//*** ***
        //
        //				MyArray.Add(CurrentItem);
        //			}
        //
        //			return (Articolo[])MyArray.ToArray(typeof(Articolo));
        //     }
        //           catch (Exception Err)
        //         {
        //      Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiArticolo.FillArticoloFromDataTableForMotore.errore: ", Err);
        //       throw (Err);
        //   }
        //		}
        //
        //*** ***
        //Rimuove un articolo
        public static Articolo[] RemoveArticolo(int ArticoloId, string idEnte)
        {
            try
            {
                Articolo art;
                ArrayList MyArray = new ArrayList();

                MyArray = GetMyArray(idEnte);

                checked {
                    for (int i = 0; i < MyArray.Count; i++)
                    {
                        art = (Articolo)MyArray[i];
                        if (art.IdArticolo == ArticoloId)
                        {
                            MyArray.RemoveAt(i);
                            break;
                        }
                    }
                }

                return (Articolo[])MyArray.ToArray(typeof(Articolo));
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiArticolo.RemoveArticolo.errore: ", Err);
                throw (Err);
            }

        }


        public static string GetDescrizioneVia(int CodVia, string IdEnte)
        {
            OggettoStrada[] arrStrade = null;
            try
            {
                OggettoStrada Strada = new OggettoStrada();
                Strada.CodiceStrada = CodVia;
                Strada.CodiceEnte = IdEnte;

                //if (System.Configuration.ConfigurationManager.AppSettings["TipoStradario"].ToString() == "WS" )
                //{
                //*** richiamando il ws ****
                //WsStradario.Stradario objStradario = new WsStradario.Stradario ();
                //arrStrade = objStradario.GetStrade(Strada);
                //}
                //else
                //{
                //*** richiamando direttamente il servizio ***
                Type typeofRI = typeof(RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario);
                RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario RemStradario = (RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario)Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings["URLServizioStradario"].ToString());

                arrStrade = RemStradario.GetArrayOggettoStrade(System.Configuration.ConfigurationManager.AppSettings["DBType"].ToString(), System.Configuration.ConfigurationManager.AppSettings["ConnessioneDBComuniStrade"], Strada);
                //}
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiArticolo.GetDescrizioneVia.errore: ", Err);
            }
            if (arrStrade != null && arrStrade.Length > 0)
            {
                return (arrStrade[0].Strada);
            }
            else
                return string.Empty;
        }
        #endregion
        #region "Private Method"


        #endregion
    }
}