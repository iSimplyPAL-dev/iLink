using System;
using System.Data;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using System.Data.SqlClient;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione elaborazione 
    /// </summary>
	public class MetodiElaborazioneEffettuata
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiElaborazioneEffettuata));

		#region "Public Method"
        public static ElaborazioneEffettuata GetElaborazioneEffettuata(int Anno, Ruolo.E_TIPO TipoRuolo, string IdTributo)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetElaborazioneEffettuata(Anno,TipoRuolo,IdTributo);
				ElaborazioneEffettuata[] elab = FillElaborazioneEffettuataFromDataTable(dt);
				if (elab.Length > 0)
					return elab[0];
				else
					return null; // Non è ancora stata fatta alcuna elaborazione
								 // per l'anno considerato
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiElaborazioneEffettuata.GetElaborazioneEffettuata.errore: ", Err);
                throw (Err);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem">ref ElaborazioneEffettuata</param>
        /// <param name="cmdMyCommand">ref SqlCommand</param>
        /// <param name="Operatore">string utente</param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public static void SetElaborazioneEffettuata(ref ElaborazioneEffettuata myItem, ref SqlCommand cmdMyCommand,string Operatore)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();
            try
            {
                DAO.SetElaborazioneEffettuata(ref myItem, ref cmdMyCommand,Operatore);             
            }
            catch (Exception Err)
            {
                Log.Debug(myItem.IdEnte + " - OPENgovOSAP.MetodiElaborazioneEffettuata.SetElaborazioneEffettuata.errore: ", Err);
                throw (Err);
            }
        }
        //public static void SetElaborazioneEffettuata(ref ElaborazioneEffettuata myItem, ref SqlCommand cmdMyCommand)
        //{
        //    DAO.CartelleDAO DAO = new DAO.CartelleDAO();
        //    try
        //    {
        //        //DAO.SetElaborazioneEffettuata(elab, ref dbEngine);
        //        DAO.SetElaborazioneEffettuata(ref myItem, ref cmdMyCommand);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(myItem.IdEnte + " - OPENgovOSAP.MetodiElaborazioneEffettuata.SetElaborazioneEffettuata.errore: ", Err);
        //        throw (Err);
        //    }
        //}

        public static ElaborazioneEffettuata GetStatistichePreElaborazione(int Anno, Ruolo.E_TIPO TipoRuolo, string IdTributo)
		//public static ElaborazioneEffettuata GetStatistichePreElaborazione(int Anno)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();

			try 
			{
				DataTable dt = null;
				dt = DAO.GetStatistichePreElaborazione(Anno,TipoRuolo,IdTributo);
				ElaborazioneEffettuata[] elab = FillElaborazioneEffettuataFromDataTable(dt);
				return elab[0];
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiElaborazioneEffettuata.GetStatistichePreElaborazione.errore: ", Err);
                throw (Err);
            }
        }

        public static string CheckTariffe(int Anno, string IdTributo)
		{
			DAO.CartelleDAO DAO = new DAO.CartelleDAO();
			string sTariffaMancante=string.Empty;
			try
			{
				DataTable dt=null;
				dt=DAO.CheckTariffa(Anno,IdTributo);
                foreach (DataRow myRow in dt.Rows)
                {
                    if (myRow[0] != DBNull.Value)
						sTariffaMancante=(string)myRow[0];
				}

			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiElaborazioneEffettuata.CheckTariffe.errore: ", Err);
                throw (Err);
            }
            return sTariffaMancante;
		}
		//*** ***

		#endregion

		#region "Private Method"

		private static ElaborazioneEffettuata[] FillElaborazioneEffettuataFromDataTable(DataTable dt)
		{

			ArrayList MyArray = new ArrayList();
			try
			{
				ElaborazioneEffettuata CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new ElaborazioneEffettuata();
				
					CurrentItem.IdEnte = (string)myRow["IDENTE"];
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
					CurrentItem.Anno = (int)myRow["ANNO"];
					CurrentItem.IdFlusso = (int)myRow["ID"];
					CurrentItem.DataOraInizioElaborazione = (DateTime)myRow["DATAORAINIZIOELABORAZIONE"];
					CurrentItem.DataOraFineElaborazione = (DateTime)myRow["DATAORAFINEELABORAZIONE"];
					CurrentItem.DataOraMinutaStampata = (DateTime)myRow["DATAORAMINUTASTAMPATA"];
					CurrentItem.DataOraMinutaApprovata = (DateTime)myRow["DATAORAMINUTAAPPROVATA"];
					CurrentItem.DataOraCalcoloRate = (DateTime)myRow["DATAORARATECALCOLATE"];
					CurrentItem.DataOraDocumentiStampati = (DateTime)myRow["DATAORADOCUMENTISTAMPATI"];
					CurrentItem.DataOraDocumentiApprovati = (DateTime)myRow["DATAORADOCUMENTIAPPROVATI"];
					CurrentItem.NUtenti = (int)myRow["NUTENTI"];
					CurrentItem.NDichiarazioni = (int)myRow["NDICHIARAZIONI"];
					CurrentItem.NArticoli = (int)myRow["NARTICOLI"];
					CurrentItem.ImportoTotale = double.Parse (myRow["IMPORTOTOTALE"].ToString ());
					CurrentItem.SogliaMinimaRate = double.Parse (myRow["SOGLIAMINIMARATE"].ToString ());
					CurrentItem.Note = (string)myRow["NOTE"];			
					//*** 20130610 - ruolo supplettivo ***
					CurrentItem.TipoRuolo=(int)myRow["TIPORUOLO"];
                    //*** ***
                    //*** 201806 ***
                    CurrentItem.ListOccupazioni = (string)myRow["ListOccupazioni"];

                    MyArray.Add(CurrentItem);
				}
			}
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiElaborazioneEffettuata.FillElaborazioneEffettuataFromDataTable.errore: ", Err);                
            }

            return (ElaborazioneEffettuata[])MyArray.ToArray(typeof(ElaborazioneEffettuata));
		}

		#endregion

	}
}
