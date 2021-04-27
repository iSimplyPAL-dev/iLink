using System;
using System.Data;
using DichiarazioniICI.Database;
using RemotingInterfaceAnater;
using Anater.Oggetti;
using Utility;
using log4net;

namespace Business
{
	/// <summary>
	/// Classe per la trasformazione degli oggetti di Dichiarazioni ICI per passarli ad Anater
	/// </summary>
	public class TrasformatoreAnater
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(TrasformatoreAnater));

        /// <summary>
        /// Anater
        /// </summary>
        /// <param name="RigaTestata"></param>
        /// <returns></returns>
        public TestataRowICI TrasformaRigaTestata(DichManagerICI.TestataRow RigaTestata)
		{
			TestataRowICI oTestataANATER=new Anater.Oggetti.TestataRowICI();
            try { 
			oTestataANATER.AnnoDichiarazione=RigaTestata.AnnoDichiarazione ;
			oTestataANATER.Annullato=RigaTestata.Annullato ;
			oTestataANATER.Bonificato=RigaTestata.Bonificato ;
			oTestataANATER.DataFine=RigaTestata.DataFine ;
			oTestataANATER.DataFineValidità=RigaTestata.DataFineValidità ;
			oTestataANATER.DataInizio=RigaTestata.DataInizio ;
			oTestataANATER.DataInizioValidità=RigaTestata.DataInizioValidità ;
			oTestataANATER.DataProtocollo=RigaTestata.DataProtocollo ;
			oTestataANATER.Ente=RigaTestata.Ente ;
			oTestataANATER.ID=RigaTestata.ID ;
			oTestataANATER.IDContribuente=RigaTestata.IDContribuente ;
			oTestataANATER.IDDenunciante=RigaTestata.IDDenunciante ;
			oTestataANATER.IDProvenienza=RigaTestata.IDProvenienza ;
			oTestataANATER.IDQuestionario=RigaTestata.IDQuestionario ;
			oTestataANATER.NumeroDichiarazione=RigaTestata.NumeroDichiarazione ;
			oTestataANATER.NumeroProtocollo=RigaTestata.NumeroProtocollo ;
			oTestataANATER.Operatore=RigaTestata.Operatore ;
			oTestataANATER.Storico=RigaTestata.Storico ;
			oTestataANATER.TotaleModelli =RigaTestata.TotaleModelli ;

			return oTestataANATER;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TrasformatoreAnater.TrasformaRigaTestata.errore: ", Err);
                throw Err;
            }
        }



		/// <summary>
		/// TrasformaRigaDettaglio
		/// </summary>
		/// <param name="rowDettaglioTestata"></param>
		/// <returns></returns>
        public DettaglioTestataRowICI TrasformaRigaDettaglio(DichManagerICI.DettaglioTestataRow rowDettaglioTestata)
		{
			DettaglioTestataRowICI oDettaglioANATER=new Anater.Oggetti.DettaglioTestataRowICI();
            try { 
			//oDettaglioANATER.AbitazionePrincipale = bool.Parse(rowDettaglioTestata.AbitazionePrincipale.ToString());
			oDettaglioANATER.AbitazionePrincipale = rowDettaglioTestata.AbitazionePrincipale;

			oDettaglioANATER.Annullato = rowDettaglioTestata.Annullato;
			oDettaglioANATER.Bonificato = rowDettaglioTestata.Bonificato;
			oDettaglioANATER.Contitolare = rowDettaglioTestata.Contitolare;
			oDettaglioANATER.DataFineValidità = rowDettaglioTestata.DataFineValidità;
			oDettaglioANATER.DataInizioValidità = rowDettaglioTestata.DataInizioValidità;
			oDettaglioANATER.Ente = rowDettaglioTestata.Ente;

			//oDettaglioANATER.EsclusioneEsenzione = bool.Parse(rowDettaglioTestata.EsclusioneEsenzione.ToString());
			oDettaglioANATER.EsclusioneEsenzione = rowDettaglioTestata.EsclusioneEsenzione;

			oDettaglioANATER.ID = rowDettaglioTestata.ID;
			oDettaglioANATER.IdOggetto = rowDettaglioTestata.IdOggetto;
			oDettaglioANATER.IdSoggetto = rowDettaglioTestata.IdSoggetto;
			oDettaglioANATER.IdTestata = rowDettaglioTestata.IdTestata;
			oDettaglioANATER.ImpDetrazAbitazPrincipale = rowDettaglioTestata.ImpDetrazAbitazPrincipale;
			oDettaglioANATER.MesiEsclusioneEsenzione = rowDettaglioTestata.MesiEsclusioneEsenzione;
			oDettaglioANATER.MesiPossesso = rowDettaglioTestata.MesiPossesso;
			oDettaglioANATER.MesiRiduzione = rowDettaglioTestata.MesiRiduzione;
			oDettaglioANATER.NumeroModello = rowDettaglioTestata.NumeroModello;
			oDettaglioANATER.NumeroOrdine = rowDettaglioTestata.NumeroOrdine;
			oDettaglioANATER.Operatore = rowDettaglioTestata.Operatore;
			oDettaglioANATER.PercPossesso = rowDettaglioTestata.PercPossesso;

//			oDettaglioANATER.Possesso = bool.Parse(rowDettaglioTestata.Possesso.ToString());
//			oDettaglioANATER.Riduzione = bool.Parse(rowDettaglioTestata.Riduzione.ToString());

			oDettaglioANATER.Possesso =rowDettaglioTestata.Possesso;
			oDettaglioANATER.Riduzione = rowDettaglioTestata.Riduzione;

			oDettaglioANATER.TipoPossesso = rowDettaglioTestata.TipoUtilizzo;


			return oDettaglioANATER;

            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TrasformatoreAnater.TrasformaRigaDettaglio.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// TrasformaRigaOggetto
		/// </summary>
		/// <param name="oImmobileRow"></param>
		/// <returns></returns>
        public OggettiRowICI TrasformaRigaOggetto(DichManagerICI.OggettiRow oImmobileRow)
		{
			OggettiRowICI oImmobileANATER=new Anater.Oggetti.OggettiRowICI();
            try { 
			oImmobileANATER.AnnoDenunciaCatastale = oImmobileRow.AnnoDenunciaCatastale;
			oImmobileANATER.Annullato = oImmobileRow.Annullato;
			oImmobileANATER.Barrato = oImmobileRow.Barrato;
			oImmobileANATER.Bonificato = oImmobileRow.Bonificato;
			oImmobileANATER.Caratteristica = oImmobileRow.Caratteristica;
			oImmobileANATER.CodCategoriaCatastale = oImmobileRow.CodCategoriaCatastale;
			oImmobileANATER.CodClasse = oImmobileRow.CodClasse;
			oImmobileANATER.CodComune = oImmobileRow.CodComune;
			oImmobileANATER.CodRendita = oImmobileRow.CodRendita;
            int myCodUI = 0;
            int.TryParse(oImmobileRow.CodUI, out myCodUI);
			oImmobileANATER.CodUI = myCodUI;
			oImmobileANATER.CodVia = oImmobileRow.CodVia;
			oImmobileANATER.Comune = oImmobileRow.Comune;
			oImmobileANATER.DataFineValidità = oImmobileRow.DataFineValidità;
			oImmobileANATER.DataInizioValidità = oImmobileRow.DataInizioValidità;
			oImmobileANATER.DataUltimaModifica = oImmobileRow.DataUltimaModifica;
			oImmobileANATER.DescrUffRegistro = oImmobileRow.DescrUffRegistro;
			oImmobileANATER.Ente = oImmobileRow.Ente;
			oImmobileANATER.EspCivico = oImmobileRow.EspCivico;
			oImmobileANATER.FlagValoreProvv = oImmobileRow.FlagValoreProvv;
			oImmobileANATER.Foglio = oImmobileRow.Foglio;
			oImmobileANATER.ID = oImmobileRow.ID;
			oImmobileANATER.IdTestata = oImmobileRow.IdTestata;
			oImmobileANATER.IDValuta = oImmobileRow.IDValuta;
			oImmobileANATER.Interno = oImmobileRow.Interno;
			oImmobileANATER.Numero = oImmobileRow.Numero;
			oImmobileANATER.NumeroCivico = oImmobileRow.NumeroCivico;
			oImmobileANATER.NumeroEcografico = oImmobileRow.NumeroEcografico;
			oImmobileANATER.NumeroModello = oImmobileRow.NumeroModello;
			oImmobileANATER.NumeroOrdine = oImmobileRow.NumeroOrdine;
			oImmobileANATER.NumeroProtCatastale = oImmobileRow.NumeroProtCatastale;
			oImmobileANATER.Operatore = oImmobileRow.Operatore;
			oImmobileANATER.PartitaCatastale = oImmobileRow.PartitaCatastale;
			oImmobileANATER.Piano = oImmobileRow.Piano;
			oImmobileANATER.Scala = oImmobileRow.Scala;
			oImmobileANATER.Sezione = oImmobileRow.Sezione;
			oImmobileANATER.Storico = oImmobileRow.Storico;
			oImmobileANATER.Subalterno = oImmobileRow.Subalterno;
			oImmobileANATER.TipoImmobile = oImmobileRow.TipoImmobile;
			oImmobileANATER.TitoloAcquisto = int.Parse(oImmobileRow.TitoloAcquisto.ToString());
			oImmobileANATER.TitoloCessione = int.Parse(oImmobileRow.TitoloCessione.ToString());
			oImmobileANATER.ValoreImmobile = oImmobileRow.ValoreImmobile;
			oImmobileANATER.Via = oImmobileRow.Via;
				
			return oImmobileANATER;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TrasformatoreAnater.TrasformaRigaOggetto.errore: ", Err);
                throw Err;
            }
        }

	}
}