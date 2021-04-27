using System;
using System.Data;
using DichiarazioniICI.Database;
using log4net;
using Utility;

namespace Business
{
    /// <summary>
    /// Classe per l'incapsulamento di tutti i metodi necessari
    /// alla gestione del modulo ici
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ModuloIci
	{
		private string _frameworkUserName;
        private static readonly ILog log = LogManager.GetLogger(typeof(ModuloIci));

		/// <summary>
		/// Costruttore del modulo ICI
		/// </summary>
		/// <param name="userName">L'utente per l'accesso al Framework Ribes</param>
		public ModuloIci(string userName)
		{
			_frameworkUserName = userName;
		}

        /// <summary>
        /// 
        /// </summary>
        public enum ErroriBonifica
        {
            /// <summary>
            /// 
            /// </summary>
            Caratteristica = 1,
            /// <summary>
            /// 
            /// </summary>
            Indirizzo_Immobile = 2,
            /// <summary>
            /// 
            /// </summary>
            Foglio_Numero = 3,
            /// <summary>
            /// 
            /// </summary>
            Categoria = 4,
            /// <summary>
            /// 
            /// </summary>
            Valore_Immobile = 5,
            /// <summary>
            /// 
            /// </summary>
            Percentuale_Possesso = 6,
            /// <summary>
            /// 
            /// </summary>
            Mesi_Possesso = 7,
            /// <summary>
            /// 
            /// </summary>
            Codice_Fiscale = 8,
            /// <summary>
            /// 
            /// </summary>
            Cognome = 9,
            /// <summary>
            /// 
            /// </summary>
            Domicilio_Fiscale = 10,
            /// <summary>
            /// 
            /// </summary>
            Detrazione = 11,
            /// <summary>
            /// 
            /// </summary>
            SenzaImmobili = 12,
            /// <summary>
            /// 
            /// </summary>
            AnnoDichMancante = 13,
            /// <summary>
            /// 
            /// </summary>
            Percentuale_Possesso_Contit = 777,
            /// <summary>
            /// 
            /// </summary>
            Mesi_Possesso_Contit = 888,
            /// <summary>
            /// 
            /// </summary>
            Detrazione_Contit = 999
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTestata"></param>
        /// <param name="IdOggetto"></param>
        /// <returns></returns>
        public bool BonificaImmobile(int idTestata, int IdOggetto)
		{
			bool Retval = true;

			// prima cancello tutti gli errori presenti nella tabella TblDichiarazioniDaBonificare per idTestata e IdOggetto
			new DichiarazioniDaBonificareTable(_frameworkUserName).DeleteByIDDichiarazione(idTestata, IdOggetto);			
			DichiarazioniDaBonificareTable Errori = new DichiarazioniDaBonificareTable(_frameworkUserName);


			DataTable Oggetti = new OggettiTable(_frameworkUserName).GetImmobileByIDTestata(idTestata,IdOggetto);
            // eseguo la ricerca degli errori per l'immobile
            try {
			foreach(DataRow RigaImmobile in Oggetti.Rows)
			{
				int IdImmobile = (int)RigaImmobile["ID"];

				if((string)RigaImmobile["Foglio"] == String.Empty || (string)RigaImmobile["Numero"] == String.Empty)
				{
					Errori.Insert(idTestata, (int)ErroriBonifica.Foglio_Numero, 0, 0, IdImmobile);
					Retval = false;
				}
				if((int)RigaImmobile["Caratteristica"] == 0)
				{
					Errori.Insert(idTestata, (int)ErroriBonifica.Caratteristica, 0, 0, IdImmobile);
					Retval = false;
				}
				if(RigaImmobile["CodCategoriaCatastale"].ToString() == String.Empty)
				{
					Errori.Insert(idTestata, (int)ErroriBonifica.Categoria, 0, 0, IdImmobile);
					Retval = false;
				}
				if((string)RigaImmobile["Comune"] == String.Empty || (string)RigaImmobile["Via"] == String.Empty)
				{
					Errori.Insert(idTestata, (int)ErroriBonifica.Indirizzo_Immobile, 0, 0, IdImmobile);
					Retval = false;
				}
				if(Convert.ToDecimal(RigaImmobile["ValoreImmobile"]) == 0 || Convert.ToDecimal(RigaImmobile["ValoreImmobile"]) == -1)
				{
					Errori.Insert(idTestata, (int)ErroriBonifica.Valore_Immobile, 0, 0, IdImmobile);
					Retval = false;
				}

				DataTable Dettaglio = new DettaglioTestataTable(String.Empty).ListDettagli(idTestata, IdImmobile);

				foreach(DataRow RigaDettaglio in Dettaglio.Rows)
				{										

					int IdDettaglio = (int)RigaDettaglio["ID"];

					// prima cancello tutti gli errori presenti nella tabella TblDichiarazioniDaBonificare per idTestata e IDDettaglioTestata
					new DichiarazioniDaBonificareTable(_frameworkUserName).DeleteByIDDichiarazione (idTestata, IdOggetto, IdDettaglio);			

					if((int)RigaDettaglio["MesiPossesso"] == 0 || (int)RigaDettaglio["MesiPossesso"] == -1)
					{
						Errori.Insert(idTestata, (int)ErroriBonifica.Mesi_Possesso, 0, IdDettaglio, 0);
						Retval = false;
					}
					if(Convert.ToDecimal(RigaDettaglio["PercPossesso"]) == 0 || Convert.ToDecimal(RigaDettaglio["PercPossesso"]) == -1)
					{
						Errori.Insert(idTestata, (int)ErroriBonifica.Percentuale_Possesso, 0, IdDettaglio, 0);
						Retval = false;
					}
					if((Convert.ToDecimal(RigaDettaglio["ImpDetrazAbitazPrincipale"]) == 0 || Convert.ToDecimal(RigaDettaglio["ImpDetrazAbitazPrincipale"]) == -1) && (int)RigaDettaglio["AbitazionePrincipale"] == 0)
					{
						Errori.Insert(idTestata, (int)ErroriBonifica.Detrazione, 0, IdDettaglio, 0);
						Retval = false;
					}
				}
			}
			
			if(Retval)
			{
				//new TestataTable(_frameworkUserName).Bonifica(idTestata);
				new OggettiTable(_frameworkUserName).Bonifica(idTestata, IdOggetto);
				new DettaglioTestataTable(_frameworkUserName).Bonifica(idTestata);
			}

			return Retval;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ModuloIci.BonificaImmobile.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="IdOggetto"></param>
		/// <returns></returns>
		public bool BonificaContitolare(int idTestata, int IdOggetto)
		{
			bool Retval = true;

			DichiarazioniDaBonificareTable Errori = new DichiarazioniDaBonificareTable(_frameworkUserName);
				//Prelevo tutti i contitolari di un immobile di una determinata dichiarazione.
				DataTable Dettaglio = new DettaglioTestataTable(String.Empty).List(idTestata, IdOggetto);
            try { 
				foreach(DataRow RigaDettaglio in Dettaglio.Rows)
				{										

					int IdDettaglio = (int)RigaDettaglio["ID"];

					// prima cancello tutti gli errori presenti nella tabella TblDichiarazioniDaBonificare per idTestata e IDDettaglioTestata
					new DichiarazioniDaBonificareTable(_frameworkUserName).DeleteByIDDichiarazione (idTestata, IdOggetto, IdDettaglio);			

					if((int)RigaDettaglio["MesiPossesso"] == 0)
					{
						Errori.Insert(idTestata, (int)ErroriBonifica.Mesi_Possesso_Contit, 0, IdDettaglio, 0);
					}
					if(Convert.ToDecimal(RigaDettaglio["PercPossesso"]) == 0)
					{
						Errori.Insert(idTestata, (int)ErroriBonifica.Percentuale_Possesso_Contit, 0, IdDettaglio, 0);
						Retval = false;
					}
					if(Convert.ToDecimal(RigaDettaglio["ImpDetrazAbitazPrincipale"]) == 0 && (int)RigaDettaglio["AbitazionePrincipale"] == 0)
					{
						Errori.Insert(idTestata, (int)ErroriBonifica.Detrazione_Contit, 0, IdDettaglio, 0);
						Retval = false;
				}
			}
			
			if(Retval)
			{
				//new TestataTable(_frameworkUserName).Bonifica(idTestata);
				//new OggettiTable(_frameworkUserName).Bonifica(idTestata);
				new DettaglioTestataTable(_frameworkUserName).Bonifica(idTestata);
			}

			return Retval;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ModuloIci.BonificaContitolare.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Imposta l'id questionario della dichiarazione passata.
		/// </summary>
		/// <param name="idDichiarazione"></param>
		/// <param name="idQuestionario"></param>
		public void SetQuestionario(int idDichiarazione, int idQuestionario)
		{
			new TestataTable(_frameworkUserName).SetQuestionario(idDichiarazione, idQuestionario);
		}
	}
    /// <summary>
    /// oggetto per il calcolo del ravvedimento
    /// </summary>
    public class objRavvedimento {
        /// <summary>
        /// da
        /// </summary>
        public int da { get; set; }
        /// <summary>
        /// a
        /// </summary>
        public int a { get; set; }
        /// <summary>
        /// giorni ritardo
        /// </summary>
        public int giorni { get; set; }
        /// <summary>
        /// aliquota sanzione
        /// </summary>
        public double aliquota { get; set; }
        /// <summary>
        /// importo calcolato
        /// </summary>
        public double importo { get; set; }
        /// <summary>
        /// richiama inizializzazione oggetto
        /// </summary>
        public objRavvedimento()
        {
            Reset();
        }
        /// <summary>
        /// inizializzazione oggetto
        /// </summary>
        public void Reset()
        {
            da =1;
            a=1;
            giorni=1;
            aliquota=0;
            importo=0;
        }
    }
}
