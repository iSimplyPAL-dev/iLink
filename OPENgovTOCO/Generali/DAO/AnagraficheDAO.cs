using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;
using DAL;
//using RIBESFrameWork;
using AnagInterface;using Anagrafica.DLL;
using log4net;
using OPENgovTOCO;

namespace DAO
{
    /// <summary>
    /// Classe interfacciamento DB per la gestione configurazioni
    /// </summary>
    public class AnagraficheDAO
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AnagraficheDAO));
        private SqlCommand cmdMyCommand;
        private SqlDataReader myDataReader;
        private DataTable dtMyDati;

        public AnagraficheDAO()
        {
        }
        #region "RIBESFRAMEWORK"
        public DataTable GetCategorie(int IdCategoria, int Anno, string idEnte, DBEngine dbEngine_)
        {
            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;
            DataTable dt = new DataTable();
            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetCategorie";
                dbEngine_.AddParameter("@IdEnte", idEnte, ParameterDirection.Input);//DichiarazioneSession.IdEnte
                dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
                dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetCategorie.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;

        }

        public DataTable GetTipologieOccupazioni(int IdTipologiaOccupazione, int Anno, string idEnte, DBEngine dbEngine_)
        {
            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetTipologiaOccupazioni";
                dbEngine_.AddParameter("@IdEnte", idEnte, ParameterDirection.Input);//DichiarazioneSession.IdEnte
                dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
                dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTipologieOccupazioni.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;
        }

        public DataTable GetUffici(int IdUfficio, DBEngine dbEngine_)
        {
            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetUffici";
                dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
                dbEngine_.AddParameter("@IdUfficio", IdUfficio, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetUffici.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;
        }

        public DataTable GetTitoloRichiedente(int IdTitoloRichiedente, DBEngine dbEngine_)
        {
            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetTitoliRichiedenti";
                dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
                dbEngine_.AddParameter("@IdTitoloRichiedente", IdTitoloRichiedente, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTitoloRichiedente.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;
        }

        public DataTable GetEsenzioni(DBEngine dbEngine_)
        {
            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetRiduzioni";
                dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetEsenzioni.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;
        }

        //public DataTable GetAgevolazioni(int IdAgevolazione, string Anno, int nIdArticolo, bool bIsSelezionato, string idEnte, DBEngine dbEngine_)
        //{
        //    //				IDataParameterCollection parameterCollection_;
        //    //				IDataParameter parameter_;
        //    DataTable dt = new DataTable();

        //    dbEngine_ = DBEngineFactory.GetDBEngine();
        //    dbEngine_.OpenConnection();

        //    try
        //    {
        //        string commandText_ = "sp_GetAgevolazioni";
        //        dbEngine_.AddParameter("@IdEnte", idEnte, ParameterDirection.Input);//DichiarazioneSession.IdEnte
        //        if (IdAgevolazione > 0)
        //        {
        //            dbEngine_.AddParameter("@IdAgevolazione", IdAgevolazione, ParameterDirection.Input);
        //        }
        //        else
        //        {
        //            dbEngine_.AddParameter("@IdAgevolazione", string.Empty, ParameterDirection.Input);
        //        }
        //        dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdArticolo", nIdArticolo, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IsSelezionato", Convert.ToInt32(bIsSelezionato), ParameterDirection.Input);
        //        dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetAgevolazioni.errore: ", Err);
        //        throw (Err);
        //    }
        //    finally
        //    {
        //        dbEngine_.CloseConnection();
        //    }

        //    return dt;

        //}
        //public DataTable GetAgevolazioni(string sIdEnte, int IdAgevolazione, string Anno, int nIdArticolo, bool bIsSelezionato, ref DBEngine dbEngine_)
        //{
        //    //				IDataParameterCollection parameterCollection_;
        //    //				IDataParameter parameter_;
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        string commandText_ = "sp_GetAgevolazioni";
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdEnte", sIdEnte, ParameterDirection.Input);
        //        if (IdAgevolazione > 0)
        //        {
        //            dbEngine_.AddParameter("@IdAgevolazione", IdAgevolazione, ParameterDirection.Input);
        //        }
        //        else
        //        {
        //            dbEngine_.AddParameter("@IdAgevolazione", string.Empty, ParameterDirection.Input);
        //        }
        //        dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdArticolo", nIdArticolo, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IsSelezionato", Convert.ToInt32(bIsSelezionato), ParameterDirection.Input);
        //        dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetAgevolazioni.errore: ", Err);
        //        throw (Err);
        //    }

        //    return dt;
        //}

        public DataTable GetTariffa(int Anno, int IdCategoria, int IdTipologiaOccupazione, DBEngine dbEngine_)
        {
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetTariffe";
                dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
                dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
                dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
                dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTariffa.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;
        }

        public DataTable GetDurata(int IdDurata, DBEngine dbEngine_)
        {
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetDurate";
                dbEngine_.AddParameter("@IdDurata", IdDurata, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetDurata.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;
        }

        //public string GetIdContribuentiFromAnagrafica(string TxtCognome, string TxtNome, string TxtCodFiscale, string TxtPIva, OPENUtility.CreateSessione WFSessione)
        //{
        //    String WFErrore = String.Empty;
        //    WFSessione = new OPENUtility.CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(), HttpContext.Current.Session["username"].ToString(), HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"].ToString());
        //    if (!WFSessione.CreaSessione((HttpContext.Current.Session["username"].ToString()), ref WFErrore))
        //    {
        //        Log.Debug("AnagraficheDAO::GetIdContribuentiFromAnagrafica::si è verificato il seguente errore::Errore durante l\'apertura della sessione di WorkFlow");
        //        throw (new Exception("Errore durante l\'apertura della sessione di WorkFlow"));
        //    }

        //    Anagrafica.DLL.GestioneAnagrafica objSearchAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, DichiarazioneSession.ApplAnagrafica);
        //    Anagrafica.DLL.GestioneAnagrafica objSearchAnagrafica = new Anagrafica.DLL.GestioneAnagrafica();
        //    DataTable dt = objSearchAnagrafica.GetListaPersone(TxtCognome, TxtNome, TxtCodFiscale, TxtPIva, DichiarazioneSession.IdEnte).Tables[0];

        //    string idContribuenti = "";

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //        idContribuenti += dt.Rows[i]["CODICE"] + " ";

        //    if (idContribuenti.Length == 0)
        //        idContribuenti = "-1"; // Nessun contribuente trovato forzo un idContribuente inesistente
        //    else
        //        idContribuenti = idContribuenti.Substring(0, idContribuenti.Length - 1);

        //    return idContribuenti;
        //}

        public DataTable GetTipiConsistenza(int IdTipoConsistenza, DBEngine dbEngine_)
        {
            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;
            DataTable dt = new DataTable();

            dbEngine_ = DBEngineFactory.GetDBEngine();
            dbEngine_.OpenConnection();

            try
            {
                string commandText_ = "sp_GetTipiConsistenza";
                dbEngine_.AddParameter("@IdTipoConsistenza", IdTipoConsistenza, ParameterDirection.Input);
                dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);

            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTipiConsistenza.errore: ", Err);
                throw (Err);
            }
            finally
            {
                dbEngine_.CloseConnection();
            }

            return dt;

        }
        
        public string SetCodContribuentiTempTable(string codContribuenti, DBEngine dbEngine_)
        {

            //				IDataParameterCollection parameterCollection_;
            //				IDataParameter parameter_;

            string tableName = String.Empty;

            try
            {

                int length;

                if (codContribuenti.Length < 7000)
                    length = codContribuenti.Length;
                else
                    length = 7000;

                if (length > 0)
                {
                    string[] strArray = SharedFunction.SplitStringAt(length, codContribuenti).Split('§');

                    tableName = "TempCodContribuenti" + HttpContext.Current.Session.SessionID;

                    foreach (string str in strArray)
                    {
                        dbEngine_.ClearParameters();
                        string commandText_ = "sp_SetTempTableAnagrafica";
                        dbEngine_.AddParameter("@TempTableName", tableName, ParameterDirection.Input);
                        dbEngine_.AddParameter("@codContribuenti", str, ParameterDirection.Input);
                        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                        dbEngine_.ExecuteNonQuery(commandText_, CommandType.StoredProcedure);
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.SetCodContribuentiTempTable.errore: ", Err);
                throw (Err);
            }

            return tableName;
        }
        
        //public DettaglioAnagrafica GetAnagraficaContribuente(int CodContribuente, string CodTributo, OPENUtility.CreateSessione WFSession)
        //{
        //    DettaglioAnagrafica oMyAnag = new DettaglioAnagrafica();
        //    //			OPENgovOSAP.MotoreTOCO.DettaglioAnagrafica oMyDetAnag=new OPENgovOSAP.MotoreTOCO.DettaglioAnagrafica();
        //    string WfErrore = "";
        //    WFSession = new OPENUtility.CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(),
        //        HttpContext.Current.Session["username"].ToString(), HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"].ToString());
        //    if (!WFSession.CreaSessione(HttpContext.Current.Session["username"].ToString(), ref WfErrore))
        //    {
        //        Log.Debug("AnagraficheDAO::GetAnagraficaContribuente::si è verificato il seguente errore::Errore durante l\'apertura della sessione di WorkFlow");
        //        throw (new Exception("Errore durante l\'apertura della sessione di WorkFlow"));
        //    }

        //    Anagrafica.DLL.GestioneAnagrafica gAnagrafica = new GestioneAnagrafica(WFSession.oSession, DichiarazioneSession.ApplAnagrafica);

        //    oMyAnag = gAnagrafica.GetAnagrafica(CodContribuente, "0434", -1);
        //    //			oMyDetAnag.CapRCP=oMyAnag.CapRCP;
        //    //			oMyDetAnag.CapResidenza=oMyAnag.CapResidenza;
        //    //			oMyDetAnag.CivicoRCP=oMyAnag.CivicoRCP;
        //    //			oMyDetAnag.CivicoResidenza=oMyAnag.CivicoResidenza;
        //    //			oMyDetAnag.COD_CONTRIBUENTE=oMyAnag.COD_CONTRIBUENTE;
        //    //			oMyDetAnag.CodComuneRCP=oMyAnag.CodComuneRCP;
        //    //			oMyDetAnag.CodContribuenteRappLegale=oMyAnag.CodContribuenteRappLegale;
        //    //			oMyDetAnag.CodEnte=oMyAnag.CodEnte;
        //    //			oMyDetAnag.CodFamiglia=oMyAnag.CodFamiglia;
        //    //			oMyDetAnag.CodiceComuneNascita=oMyAnag.CodiceComuneNascita;
        //    //			oMyDetAnag.CodiceComuneResidenza=oMyAnag.CodiceComuneResidenza;
        //    //			oMyDetAnag.CodiceFiscale=oMyAnag.CodiceFiscale;
        //    //			oMyDetAnag.CodIndividuale=oMyAnag.CodIndividuale;
        //    //			oMyDetAnag.CodTributo=oMyAnag.CodTributo;
        //    //			oMyDetAnag.CodViaRCP=oMyAnag.CodViaRCP;
        //    //			oMyDetAnag.CodViaResidenza=oMyAnag.CodViaResidenza;
        //    //			oMyDetAnag.Cognome=oMyAnag.Cognome;
        //    //			oMyDetAnag.CognomeInvio=oMyAnag.CognomeInvio;
        //    //			oMyDetAnag.ComuneNascita=oMyAnag.ComuneNascita;
        //    //			oMyDetAnag.ComuneRCP=oMyAnag.ComuneRCP;
        //    //			oMyDetAnag.ComuneResidenza=oMyAnag.ComuneResidenza;
        //    //			oMyDetAnag.Concurrency=oMyAnag.Concurrency;
        //    //			oMyDetAnag.DaRicontrollare=oMyAnag.DaRicontrollare;
        //    //			oMyDetAnag.DataFineValidita=oMyAnag.DataFineValidita;
        //    //			oMyDetAnag.DataFineValiditaSpedizione=oMyAnag.DataFineValiditaSpedizione;
        //    //			oMyDetAnag.DataInizioValidita=oMyAnag.DataInizioValidita;
        //    //			oMyDetAnag.DataInizioValiditaSpedizione=oMyAnag.DataInizioValiditaSpedizione;
        //    //			oMyDetAnag.DataMorte=oMyAnag.DataMorte;
        //    //			oMyDetAnag.DataNascita=oMyAnag.DataNascita;
        //    //			oMyDetAnag.DataUltimaModifica=oMyAnag.DataUltimaModifica;
        //    //			oMyDetAnag.DataUltimaModificaSpedizione=oMyAnag.DataUltimaModificaSpedizione;
        //    //			oMyDetAnag.DataUltimoAggAnagrafe=oMyAnag.DataUltimoAggAnagrafe;
        //    //			oMyDetAnag.DataUltimoAggTributi=oMyAnag.DataUltimoAggTributi;
        //    //			oMyDetAnag.DatiRiferimento=oMyAnag.DatiRiferimento;
        //    //			//oMyDetAnag.dsContatti=oMyAnag.dsContatti;
        //    //			oMyDetAnag.dsTipiContatti=oMyAnag.dsTipiContatti;
        //    //			oMyDetAnag.EsponenteCivicoRCP=oMyAnag.EsponenteCivicoRCP;
        //    //			oMyDetAnag.EsponenteCivicoResidenza=oMyAnag.EsponenteCivicoResidenza;
        //    //			oMyDetAnag.FrazioneRCP=oMyAnag.FrazioneRCP;
        //    //			oMyDetAnag.FrazioneResidenza=oMyAnag.FrazioneResidenza;
        //    //			oMyDetAnag.ID=oMyAnag.ID;
        //    //			oMyDetAnag.ID_DATA_ANAGRAFICA=oMyAnag.ID_DATA_ANAGRAFICA;
        //    //			oMyDetAnag.ID_DATA_SPEDIZIONE=oMyAnag.ID_DATA_SPEDIZIONE;
        //    //			oMyDetAnag.InternoCivicoRCP=oMyAnag.InternoCivicoRCP;
        //    //			oMyDetAnag.InternoCivicoResidenza=oMyAnag.InternoCivicoResidenza;
        //    //			oMyDetAnag.LocRCP=oMyAnag.LocRCP;
        //    //			oMyDetAnag.NazionalitaNascita=oMyAnag.NazionalitaNascita;
        //    //			oMyDetAnag.NazionalitaResidenza=oMyAnag.NazionalitaResidenza;
        //    //			oMyDetAnag.NCAnagraficaRes=oMyAnag.NCAnagraficaRes;
        //    //			oMyDetAnag.NCTributari=oMyAnag.NCTributari;
        //    //			oMyDetAnag.Nome=oMyAnag.Nome;
        //    //			oMyDetAnag.NomeInvio=oMyAnag.NomeInvio;
        //    //			oMyDetAnag.Note=oMyAnag.Note;
        //    //			oMyDetAnag.NucleoFamiliare=oMyAnag.NucleoFamiliare;
        //    //			oMyDetAnag.Operatore=oMyAnag.Operatore;
        //    //			oMyDetAnag.OperatoreSpedizione=oMyAnag.OperatoreSpedizione;
        //    //			oMyDetAnag.PartitaIva=oMyAnag.PartitaIva;
        //    //			oMyDetAnag.PosizioneCivicoRCP=oMyAnag.PosizioneCivicoRCP;
        //    //			oMyDetAnag.PosizioneCivicoResidenza=oMyAnag.PosizioneCivicoResidenza;
        //    //			oMyDetAnag.Professione=oMyAnag.Professione;
        //    //			oMyDetAnag.ProvinciaNascita=oMyAnag.ProvinciaNascita;
        //    //			oMyDetAnag.ProvinciaRCP=oMyAnag.ProvinciaRCP;
        //    //			oMyDetAnag.ProvinciaResidenza=oMyAnag.ProvinciaResidenza;
        //    //			oMyDetAnag.RappresentanteLegale=oMyAnag.RappresentanteLegale;
        //    //			oMyDetAnag.ScalaCivicoRCP=oMyAnag.ScalaCivicoRCP;
        //    //			oMyDetAnag.ScalaCivicoResidenza=oMyAnag.ScalaCivicoResidenza;
        //    //			oMyDetAnag.Sesso=oMyAnag.Sesso;
        //    //			oMyDetAnag.TipoRiferimento=oMyAnag.TipoRiferimento;
        //    //			oMyDetAnag.ViaRCP=oMyAnag.ViaRCP;
        //    //			oMyDetAnag.ViaResidenza=oMyAnag.ViaResidenza;
        //    //			return oMyDetAnag;
        //    return oMyAnag;
        //}
        #endregion

        #region "NO RIBESFRAMEWORK"
        public DataTable GetCategorie(string ConnectionString, int IdCategoria, string Descrizione, int Anno, string idEnte, string IdTributo)
        {
            //*** 20140409
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", idEnte);//DichiarazioneSession.IdEnte
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.CommandText = "sp_GetCategorie";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetCategorie.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetCategorie";
				dbEngine_.AddParameter("@IdEnte", idEnte , ParameterDirection.Input);//DichiarazioneSession.IdEnte
				dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			catch (Exception Err)
			{
				Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetCategorie.errore: ", Err);
				throw (Err);
			}
			finally
			{
				dbEngine_.CloseConnection();
			}
            return dt;
			*/
            return dtMyDati;

        }

        public DataTable GetTipologieOccupazioni(string ConnectionString, int IdTipologiaOccupazione,string Descrizione, int Anno, string idEnte, string IdTributo)
        {
            //*** 20140409
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", idEnte);//DichiarazioneSession.IdEnte
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", IdTipologiaOccupazione);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.CommandText = "sp_GetTipologiaOccupazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTipologieOccupazioni.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetTipologiaOccupazioni";
				dbEngine_.AddParameter("@IdEnte", idEnte , ParameterDirection.Input);//DichiarazioneSession.IdEnte
				dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
		 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTipologieOccupazioni.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
            return dt;
			*/
            return dtMyDati;
        }

        public DataTable GetUffici(int IdUfficio)
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdUfficio", IdUfficio);
                Log.Debug("AnagraficheDAO::GetUffici::esecuzione sp_GetUffici");
                cmdMyCommand.CommandText = "sp_GetUffici";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetUffici.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetUffici";
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdUfficio", IdUfficio, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			  catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetUffici.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
            return dt;
			*/

        }
        public DataTable GetUffici(string ConnectionString, string IdEnte, string Descrizione)
        {
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.CommandText = "sp_GetUffici";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetUffici.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
            }

        public DataTable GetTitoloRichiedente(int IdTitoloRichiedente)
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTitoloRichiedente", IdTitoloRichiedente);
                Log.Debug("AnagraficheDAO::GetTitoloRichiedente::esecuzione sp_GetTitoliRichiedenti");
                cmdMyCommand.CommandText = "sp_GetTitoliRichiedenti";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTitoloRichiedente.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetTitoliRichiedenti";
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTitoloRichiedente", IdTitoloRichiedente, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);				
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTitoloRichiedente.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
             */
        }
        public DataTable GetTitoloRichiedente(string ConnectionString, string IdEnte, string Descrizione)
        {
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@Descrizione", Descrizione);
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                cmdMyCommand.CommandText = "sp_GetTitoliRichiedenti";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTitoloRichiedente.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
        }

        public DataTable GetEsenzioni()
        {
            //*** 20140409
            connessioneDB();

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", DichiarazioneSession.IdEnte);
                Log.Debug("AnagraficheDAO::GetEsenzioni::esecuzione sp_GetRiduzioni");
                cmdMyCommand.CommandText = "sp_GetRiduzioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetEsenzioni.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetRiduzioni";
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetEsenzioni.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        public DataTable GetAgevolazioni(string ConnectionString, int IdAgevolazione, string Anno, int nIdArticolo, int IdRuolo, bool bIsSelezionato, string idEnte, string IdTributo)
        {
            //*** 20140409
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", idEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                if (IdAgevolazione > 0)
                {
                    cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", IdAgevolazione);
                }
                else
                {
                    cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", string.Empty);
                }

                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdArticolo", nIdArticolo);
                cmdMyCommand.Parameters.AddWithValue("@IdRuolo", IdRuolo);
                cmdMyCommand.Parameters.AddWithValue("@IsSelezionato", Convert.ToInt32(bIsSelezionato));
                Log.Debug("AnagraficheDAO::GetAgevolazioni::esecuzione sp_GetAgevolazioni");
                cmdMyCommand.CommandText = "sp_GetAgevolazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetAgevolazioni.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;


            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
				
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
				
			try
			{
				string commandText_ = "sp_GetAgevolazioni";
				dbEngine_.AddParameter("@IdEnte", idEnte , ParameterDirection.Input);//DichiarazioneSession.IdEnte
				if (IdAgevolazione>0)
				{
					dbEngine_.AddParameter("@IdAgevolazione", IdAgevolazione, ParameterDirection.Input);
				}
				else
				{
					dbEngine_.AddParameter("@IdAgevolazione", string.Empty, ParameterDirection.Input);
				}
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdArticolo", nIdArticolo, ParameterDirection.Input);
				dbEngine_.AddParameter("@IsSelezionato", Convert.ToInt32(bIsSelezionato), ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);					
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetAgevolazioni.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
				
			return dt;
	    	*/
        }

        //public DataTable GetAgevolazioni(string sIdEnte, int IdAgevolazione, string Anno, int nIdArticolo,bool bIsSelezionato, ref DBEngine dbEngine_)
        //{
        //    //				IDataParameterCollection parameterCollection_;
        //    //				IDataParameter parameter_;
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        string commandText_ = "sp_GetAgevolazioni";
        //        dbEngine_.ClearParameters();
        //        dbEngine_.AddParameter("@IdEnte", sIdEnte, ParameterDirection.Input);
        //        if (IdAgevolazione>0)
        //        {
        //            dbEngine_.AddParameter("@IdAgevolazione", IdAgevolazione, ParameterDirection.Input);
        //        }
        //        else
        //        {
        //            dbEngine_.AddParameter("@IdAgevolazione", string.Empty, ParameterDirection.Input);
        //        }
        //        dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IdArticolo", nIdArticolo, ParameterDirection.Input);
        //        dbEngine_.AddParameter("@IsSelezionato", Convert.ToInt32(bIsSelezionato), ParameterDirection.Input);
        //        dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);					
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("AnagraficheDAO::GetAgevolazioni::si è verificato il seguente errore::" + ex.Message);
        //        throw (ex);
        //    }

        //    return dt;
        //}

        public DataTable GetAgevolazioni(string sIdEnte, int IdAgevolazione, string Anno, int nIdArticolo, bool bIsSelezionato, ref SqlCommand cmdMyCommand)
        {
            //*** 20140409
            dtMyDati = new DataTable();

            try
            {
                cmdMyCommand.Parameters.Clear();
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte);
                if (IdAgevolazione > 0)
                {
                    cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", IdAgevolazione);
                }
                else
                {
                    cmdMyCommand.Parameters.AddWithValue("@IdAgevolazione", string.Empty);
                }

                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdArticolo", nIdArticolo);
                cmdMyCommand.Parameters.AddWithValue("@IsSelezionato", Convert.ToInt32(bIsSelezionato));
                Log.Debug("AnagraficheDAO::GetAgevolazioni::esecuzione sp_GetAgevolazioni");
                cmdMyCommand.CommandText = "sp_GetAgevolazioni";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetAgevolazioni.errore: ", Err);
                throw (Err);
            }

            return dtMyDati;
        }

        public DataTable GetTariffa(string ConnectionString,string IdEnte, int Anno, int IdCategoria, int IdTipologiaOccupazione, string IdTributo)
        {
            //*** 20140409
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte);
                cmdMyCommand.Parameters.AddWithValue("@IdTributo", IdTributo);
                cmdMyCommand.Parameters.AddWithValue("@Anno", Anno);
                cmdMyCommand.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                cmdMyCommand.Parameters.AddWithValue("@IdTipologiaOccupazione", IdTipologiaOccupazione);
                Log.Debug("AnagraficheDAO::GetTariffa::esecuzione sp_GetTariffe");
                cmdMyCommand.CommandText = "sp_GetTariffe";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTariffa.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;
            /*
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetTariffe";
				dbEngine_.AddParameter("@IdEnte", DichiarazioneSession.IdEnte, ParameterDirection.Input);
				dbEngine_.AddParameter("@Anno", Anno, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdCategoria", IdCategoria, ParameterDirection.Input);
				dbEngine_.AddParameter("@IdTipologiaOccupazione", IdTipologiaOccupazione, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			  catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetTariffa.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
            */
        }

        public DataTable GetDurata(string ConnectionString, int IdDurata)
        {
            //*** 20140409
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdDurata", IdDurata);
                Log.Debug("AnagraficheDAO::GetDurata::esecuzione sp_GetDurate");
                cmdMyCommand.CommandText = "sp_GetDurate";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetDurata.errore: ", Err);
                throw (Err);
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;

            /*
			DBEngine dbEngine_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetDurate";
				dbEngine_.AddParameter("@IdDurata", IdDurata, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
			}
			catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.GetDurata.errore: ", Err);
                throw (Err);
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
             */
        }

        public string GetIdContribuentiFromAnagrafica(string TxtCognome, string TxtNome, string TxtCodFiscale, string TxtPIva, string connessione)
        {

            //OPENUtility.CreateSessione WFSessione;
            String WFErrore = String.Empty;
            //WFSessione = new OPENUtility.CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(), HttpContext.Current.Session["username"].ToString(), HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"].ToString());
            //if (!WFSessione.CreaSessione((HttpContext.Current.Session["username"].ToString()), ref WFErrore))
            //{
            //    Log.Debug("AnagraficheDAO::GetIdContribuentiFromAnagrafica::si è verificato il seguente errore::Errore durante l\'apertura della sessione di WorkFlow");
            //    throw (new Exception("Errore durante l\'apertura della sessione di WorkFlow"));
            //}

            //Anagrafica.DLL.GestioneAnagrafica objSearchAnagrafica = new Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, DichiarazioneSession.ApplAnagrafica );
            Anagrafica.DLL.GestioneAnagrafica objSearchAnagrafica = new Anagrafica.DLL.GestioneAnagrafica();
            DataTable dt = objSearchAnagrafica.GetListaPersone(-1,TxtCognome, TxtNome, TxtCodFiscale, TxtPIva, DichiarazioneSession.IdEnte, DichiarazioneSession.DBType, connessione).Tables[0];

            string idContribuenti = "";

            foreach (DataRow myRow in dt.Rows)
            {
                idContribuenti += myRow["CODICE"] + " ";
            }
            if (idContribuenti.Length == 0)
                idContribuenti = "-1"; // Nessun contribuente trovato forzo un idContribuente inesistente
            else
                idContribuenti = idContribuenti.Substring(0, idContribuenti.Length - 1);

            return idContribuenti;
        }

        public DataTable GetTipiConsistenza(string ConnectionString, int IdTipoConsistenza)
        {
            //*** 2014049
            connessioneDB(ConnectionString);

            try
            {
                cmdMyCommand.Parameters.AddWithValue("@IdTipoConsistenza", IdTipoConsistenza);
                Log.Debug("AnagraficheDAO::GetTipiConsistenza::esecuzione sp_GetTipiConsistenza");
                cmdMyCommand.CommandText = "sp_GetTipiConsistenza";
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand));
                myDataReader = cmdMyCommand.ExecuteReader();
                dtMyDati.Load(myDataReader);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.anagraficheDAO.GetTipiConsistenza.errore: ", Err);
                
            }
            finally
            {
                cmdMyCommand.Connection.Close();
            }

            return dtMyDati;

            /*
			DBEngine dbEngine_;
			//				IDataParameterCollection parameterCollection_;
			//				IDataParameter parameter_;
			DataTable dt = new DataTable();
			
			dbEngine_ = DBEngineFactory.GetDBEngine();
			dbEngine_.OpenConnection();
			
			try
			{
				string commandText_ = "sp_GetTipiConsistenza";
				dbEngine_.AddParameter("@IdTipoConsistenza", IdTipoConsistenza, ParameterDirection.Input);
				dbEngine_.ExecuteQuery(ref dt, commandText_, CommandType.StoredProcedure);
				
			}
			 catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.anagraficheDAO.GetTipiConsistenza.errore: ", Err);
                
            }
			finally
			{
				dbEngine_.CloseConnection();
			}
			
			return dt;
			*/
        }

        //public string SetCodContribuentiTempTable(string codContribuenti, DBEngine dbEngine_)
        //{

        //    //				IDataParameterCollection parameterCollection_;
        //    //				IDataParameter parameter_;

        //    string tableName = String.Empty;

        //    try
        //    {

        //        int length;

        //        if (codContribuenti.Length < 7000) 
        //            length = codContribuenti.Length;
        //        else
        //            length = 7000;

        //        if (length > 0)
        //        {
        //            string[] strArray = SharedFunction.SplitStringAt(length, codContribuenti).Split('§');

        //            tableName = "TempCodContribuenti" + HttpContext.Current.Session.SessionID;    

        //            foreach (string str in strArray)
        //            {
        //                dbEngine_.ClearParameters(); 
        //                string commandText_ = "sp_SetTempTableAnagrafica";
        //                dbEngine_.AddParameter("@TempTableName", tableName, ParameterDirection.Input);
        //                dbEngine_.AddParameter("@codContribuenti", str, ParameterDirection.Input);
        //                dbEngine_.ExecuteNonQuery (commandText_, CommandType.StoredProcedure);    
        //            }
        //        }
        //    }
        //    catch (Exception Err)
        //  {
        //    Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.anagraficheDAO.SetCodContribuentiTempTable.errore: ", Err);
        //   }

        //    return tableName;
        //}

        public string SetCodContribuentiTempTable(string codContribuenti, SqlCommand cmdMyCommand)
        {

            string tableName = String.Empty;
            try
            {

                int length;

                if (codContribuenti.Length < 7000)
                    length = codContribuenti.Length;
                else
                    length = 7000;

                if (length > 0)
                {
                    string[] strArray = SharedFunction.SplitStringAt(length, codContribuenti).Split('§');

                    tableName = "TempCodContribuenti" + HttpContext.Current.Session.SessionID;

                    foreach (string str in strArray)
                    {
                        cmdMyCommand.Parameters.Clear();
                        cmdMyCommand.Parameters.AddWithValue("@TempTableName", tableName);
                        cmdMyCommand.Parameters.AddWithValue("@codContribuenti", str);
                        Log.Debug("AnagraficheDAO::SetCodContribuentiTempTable::esecuzione sp_SetTempTableAnagrafica");
                        cmdMyCommand.CommandText = "sp_SetTempTableAnagrafica";
                        cmdMyCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.anagraficheDAO.SetCodContribuentiTempTable.errore: ", Err);

            }
            return tableName;
        }

        public DettaglioAnagrafica GetAnagraficaContribuente(int CodContribuente)//, string CodTributo
        {
            DettaglioAnagrafica oMyAnag = new DettaglioAnagrafica();
            //			OPENgovOSAP.MotoreTOCO.DettaglioAnagrafica oMyDetAnag=new OPENgovOSAP.MotoreTOCO.DettaglioAnagrafica();
            //OPENUtility.CreateSessione WFSession;
            //string WfErrore = "";
            //WFSession = new OPENUtility.CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(),
            //    HttpContext.Current.Session["username"].ToString(), HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"].ToString());
            //if (! WFSession.CreaSessione(HttpContext.Current.Session["username"].ToString() , ref WfErrore))
            //{
            //    Log.Debug("AnagraficheDAO::GetAnagraficaContribuente::si è verificato il seguente errore::Errore durante l\'apertura della sessione di WorkFlow");
            //    throw (new Exception("Errore durante l\'apertura della sessione di WorkFlow"));
            //}

            //Anagrafica.DLL.GestioneAnagrafica gAnagrafica = new GestioneAnagrafica (WFSession.oSession, DichiarazioneSession.ApplAnagrafica);
            Anagrafica.DLL.GestioneAnagrafica gAnagrafica = new GestioneAnagrafica();

            oMyAnag = gAnagrafica.GetAnagrafica(CodContribuente, -1, string.Empty, DichiarazioneSession.DBType, DichiarazioneSession.StringConnectionAnagrafica,false);
            //			oMyDetAnag.CapRCP=oMyAnag.CapRCP;
            //			oMyDetAnag.CapResidenza=oMyAnag.CapResidenza;
            //			oMyDetAnag.CivicoRCP=oMyAnag.CivicoRCP;
            //			oMyDetAnag.CivicoResidenza=oMyAnag.CivicoResidenza;
            //			oMyDetAnag.COD_CONTRIBUENTE=oMyAnag.COD_CONTRIBUENTE;
            //			oMyDetAnag.CodComuneRCP=oMyAnag.CodComuneRCP;
            //			oMyDetAnag.CodContribuenteRappLegale=oMyAnag.CodContribuenteRappLegale;
            //			oMyDetAnag.CodEnte=oMyAnag.CodEnte;
            //			oMyDetAnag.CodFamiglia=oMyAnag.CodFamiglia;
            //			oMyDetAnag.CodiceComuneNascita=oMyAnag.CodiceComuneNascita;
            //			oMyDetAnag.CodiceComuneResidenza=oMyAnag.CodiceComuneResidenza;
            //			oMyDetAnag.CodiceFiscale=oMyAnag.CodiceFiscale;
            //			oMyDetAnag.CodIndividuale=oMyAnag.CodIndividuale;
            //			oMyDetAnag.CodTributo=oMyAnag.CodTributo;
            //			oMyDetAnag.CodViaRCP=oMyAnag.CodViaRCP;
            //			oMyDetAnag.CodViaResidenza=oMyAnag.CodViaResidenza;
            //			oMyDetAnag.Cognome=oMyAnag.Cognome;
            //			oMyDetAnag.CognomeInvio=oMyAnag.CognomeInvio;
            //			oMyDetAnag.ComuneNascita=oMyAnag.ComuneNascita;
            //			oMyDetAnag.ComuneRCP=oMyAnag.ComuneRCP;
            //			oMyDetAnag.ComuneResidenza=oMyAnag.ComuneResidenza;
            //			oMyDetAnag.Concurrency=oMyAnag.Concurrency;
            //			oMyDetAnag.DaRicontrollare=oMyAnag.DaRicontrollare;
            //			oMyDetAnag.DataFineValidita=oMyAnag.DataFineValidita;
            //			oMyDetAnag.DataFineValiditaSpedizione=oMyAnag.DataFineValiditaSpedizione;
            //			oMyDetAnag.DataInizioValidita=oMyAnag.DataInizioValidita;
            //			oMyDetAnag.DataInizioValiditaSpedizione=oMyAnag.DataInizioValiditaSpedizione;
            //			oMyDetAnag.DataMorte=oMyAnag.DataMorte;
            //			oMyDetAnag.DataNascita=oMyAnag.DataNascita;
            //			oMyDetAnag.DataUltimaModifica=oMyAnag.DataUltimaModifica;
            //			oMyDetAnag.DataUltimaModificaSpedizione=oMyAnag.DataUltimaModificaSpedizione;
            //			oMyDetAnag.DataUltimoAggAnagrafe=oMyAnag.DataUltimoAggAnagrafe;
            //			oMyDetAnag.DataUltimoAggTributi=oMyAnag.DataUltimoAggTributi;
            //			oMyDetAnag.DatiRiferimento=oMyAnag.DatiRiferimento;
            //			//oMyDetAnag.dsContatti=oMyAnag.dsContatti;
            //			oMyDetAnag.dsTipiContatti=oMyAnag.dsTipiContatti;
            //			oMyDetAnag.EsponenteCivicoRCP=oMyAnag.EsponenteCivicoRCP;
            //			oMyDetAnag.EsponenteCivicoResidenza=oMyAnag.EsponenteCivicoResidenza;
            //			oMyDetAnag.FrazioneRCP=oMyAnag.FrazioneRCP;
            //			oMyDetAnag.FrazioneResidenza=oMyAnag.FrazioneResidenza;
            //			oMyDetAnag.ID=oMyAnag.ID;
            //			oMyDetAnag.ID_DATA_ANAGRAFICA=oMyAnag.ID_DATA_ANAGRAFICA;
            //			oMyDetAnag.ID_DATA_SPEDIZIONE=oMyAnag.ID_DATA_SPEDIZIONE;
            //			oMyDetAnag.InternoCivicoRCP=oMyAnag.InternoCivicoRCP;
            //			oMyDetAnag.InternoCivicoResidenza=oMyAnag.InternoCivicoResidenza;
            //			oMyDetAnag.LocRCP=oMyAnag.LocRCP;
            //			oMyDetAnag.NazionalitaNascita=oMyAnag.NazionalitaNascita;
            //			oMyDetAnag.NazionalitaResidenza=oMyAnag.NazionalitaResidenza;
            //			oMyDetAnag.NCAnagraficaRes=oMyAnag.NCAnagraficaRes;
            //			oMyDetAnag.NCTributari=oMyAnag.NCTributari;
            //			oMyDetAnag.Nome=oMyAnag.Nome;
            //			oMyDetAnag.NomeInvio=oMyAnag.NomeInvio;
            //			oMyDetAnag.Note=oMyAnag.Note;
            //			oMyDetAnag.NucleoFamiliare=oMyAnag.NucleoFamiliare;
            //			oMyDetAnag.Operatore=oMyAnag.Operatore;
            //			oMyDetAnag.OperatoreSpedizione=oMyAnag.OperatoreSpedizione;
            //			oMyDetAnag.PartitaIva=oMyAnag.PartitaIva;
            //			oMyDetAnag.PosizioneCivicoRCP=oMyAnag.PosizioneCivicoRCP;
            //			oMyDetAnag.PosizioneCivicoResidenza=oMyAnag.PosizioneCivicoResidenza;
            //			oMyDetAnag.Professione=oMyAnag.Professione;
            //			oMyDetAnag.ProvinciaNascita=oMyAnag.ProvinciaNascita;
            //			oMyDetAnag.ProvinciaRCP=oMyAnag.ProvinciaRCP;
            //			oMyDetAnag.ProvinciaResidenza=oMyAnag.ProvinciaResidenza;
            //			oMyDetAnag.RappresentanteLegale=oMyAnag.RappresentanteLegale;
            //			oMyDetAnag.ScalaCivicoRCP=oMyAnag.ScalaCivicoRCP;
            //			oMyDetAnag.ScalaCivicoResidenza=oMyAnag.ScalaCivicoResidenza;
            //			oMyDetAnag.Sesso=oMyAnag.Sesso;
            //			oMyDetAnag.TipoRiferimento=oMyAnag.TipoRiferimento;
            //			oMyDetAnag.ViaRCP=oMyAnag.ViaRCP;
            //			oMyDetAnag.ViaResidenza=oMyAnag.ViaResidenza;
            //			return oMyDetAnag;
            return oMyAnag;
        }

        public void connessioneDB()
        {
            //*** 20140409
            try {
                Log.Debug("AnagraficheDAO::connessioneDB::apertura della connessione al DB");

                cmdMyCommand = new SqlCommand();
                myDataReader = null;
                dtMyDati = new DataTable();

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(DichiarazioneSession.StringConnection);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.connessioneDB.errore: ", Err);

            }

            
        }
        public void connessioneDB(string ConnectionString)
        {
            //*** 20140409
            try {
                Log.Debug("AnagraficheDAO::connessioneDB::apertura della connessione al DB su " + ConnectionString);

                cmdMyCommand = new SqlCommand();
                myDataReader = null;
                dtMyDati = new DataTable();

                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.Connection = new SqlConnection(ConnectionString);
                cmdMyCommand.CommandTimeout = 0;
                if (cmdMyCommand.Connection.State == ConnectionState.Closed)
                {
                    cmdMyCommand.Connection.Open();
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.AnagraficheDAO.connessioneDB.errore: ", Err);
              
            }
          
        }
        #endregion
    }
}



