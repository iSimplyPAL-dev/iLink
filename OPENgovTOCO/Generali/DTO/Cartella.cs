using System;
using System.Data;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using AnagInterface;
using OPENUtility;
using IRemInterfaceOSAP;
using OPENgovTOCO;
using System.Data.SqlClient;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione Avviso
    /// </summary>
    public class MetodiCartella
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiCartella));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codiceCartella"></param>
        /// <param name="codContribuente"></param>
        /// <param name="idEnte"></param>
        /// <returns></returns>
        public static Cartella GetCartellaByCodiceCartella(string codiceCartella, int codContribuente, string idEnte)
        {
            if (codiceCartella == null || codiceCartella == "" || idEnte == "")
                return null;

            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DataTable dt = DAO.GetCartellaByCodiceCartella(codiceCartella, codContribuente, idEnte);
                if (dt != null)
                {
                    Cartella[] cartelleList = new DAO.ElaborazioniDAO().FillCartellaFromDataTable(dt);
                    if (cartelleList != null && cartelleList.Length == 1)
                        return cartelleList[0];
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "." + DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCartella.GetCartellaByCodiceCartella.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IdCartella"></param>
        /// <param name="IdEnte"></param>
        /// <param name="FillInfoAnagrafica"></param>
        /// <returns></returns>
        public static Cartella GetCartella(string ConnectionString, int IdCartella, string IdEnte, bool FillInfoAnagrafica)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetCartella(IdCartella, IdEnte);
                Cartella[] cart = new DAO.ElaborazioniDAO().FillCartellaFromDataTable(dt);

                if (FillInfoAnagrafica)
                {
                    DAO.AnagraficheDAO anagDAO = new DAO.AnagraficheDAO();
                    cart[0].DettaglioContribuente = anagDAO.GetAnagraficaContribuente(cart[0].CodContribuente);//, DichiarazioneSession.CodTributo("")
                }

                cart[0].Ruoli = MetodiRuolo.GetRuoliCartella(ConnectionString, IdCartella, DichiarazioneSession.IdEnte, cart[0]);

                return cart[0];
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "." + DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCartella.GetCartella.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSearch"></param>
        /// <param name="cmdMyCommand"></param>
        /// <returns></returns>
        public static Cartella[] CartelleSearch(CartellaSearch objSearch, SqlCommand cmdMyCommand)
        {
            //*** 20140410
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();
            try
            {
                DataTable dt = null;
                dt = DAO.CartelleSearch(objSearch, cmdMyCommand);
                Cartella[] cart = new DAO.ElaborazioniDAO().FillCartellaFromDataTable(dt);

                Hashtable anagraficaTable = new Hashtable();

                // Se fornisco la connessione dall'esterno sto agendo nel
                // thread di calcolo rate, per cui non posso accedere a
                // DichiarazioneSession, HttpContext, ecc.
                if (cmdMyCommand == null)
                {
                    foreach (Cartella c in cart)
                    {
                        if (anagraficaTable.ContainsKey(c.CodContribuente))
                        {
                            c.DettaglioContribuente = (DettaglioAnagrafica)anagraficaTable[c.CodContribuente];
                        }
                        else
                        {
                            DAO.AnagraficheDAO anagDAO = new DAO.AnagraficheDAO();
                            DettaglioAnagrafica contrib = anagDAO.GetAnagraficaContribuente(c.CodContribuente);//, DichiarazioneSession.CodTributo("")
                            c.DettaglioContribuente = contrib;
                            anagraficaTable.Add(c.CodContribuente, contrib);
                        }
                    }
                }

                return cart;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "." + DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCartella.CartellaSearch.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListAvvisi"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public static DataTable PrintAvvisi(Cartella[] ListAvvisi)
        {
            try
            {
                DataTable dtStampa = new DataTable();
                DataSet dsStampa = new DataSet();
                int nCampi = 0;
                string sDatiStampa = "";

                nCampi = 8;
                dsStampa.Tables.Add("STAMPA");
                //carico le colonne nel dataset
                checked
                {
                    for (int x = 0; x <= nCampi + 1; x++)
                    {
                        dsStampa.Tables["STAMPA"].Columns.Add("Col" + x.ToString().PadLeft(3, char.Parse("0")));
                    }
                }
                //carico il datatable
                dtStampa = dsStampa.Tables["STAMPA"];
                //inserisco l//intestazione dell//ente
                sDatiStampa = DichiarazioneSession.IdEnte + " - " + DichiarazioneSession.DescrizioneEnte; ;
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco l//intestazione del report
                sDatiStampa = "Elenco Avvisi";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco le intestazioni di colonna
                sDatiStampa = "";
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA";
                sDatiStampa += "|Anno|Data Emissione|N. Avviso";
                sDatiStampa += "|Imp.Carico|Imp.Pagato|Imp.Pre Sgravio";
                if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                if (ListAvvisi != null)
                {
                    //ciclo sui dati da stampare
                    foreach (Cartella myRow in ListAvvisi)
                    {
                        sDatiStampa = "";
                        sDatiStampa += myRow.DettaglioContribuente.Cognome;
                        sDatiStampa += "|" + myRow.DettaglioContribuente.Nome;
                        if (myRow.DettaglioContribuente.PartitaIva != "")
                            sDatiStampa += "|'" + myRow.DettaglioContribuente.PartitaIva;
                        else
                            sDatiStampa += "|'" + myRow.DettaglioContribuente.CodiceFiscale;
                        sDatiStampa += "|" + myRow.Anno;
                        sDatiStampa += "|" + SharedFunction.FormattaData(myRow.DataEmissione);
                        sDatiStampa += "|'" + myRow.CodiceCartella;
                        sDatiStampa += "|" + myRow.ImportoCarico.ToString();
                        sDatiStampa += "|" + myRow.ImpPagato.ToString();
                        if (myRow.ImpPreSgravio > 0)
                            sDatiStampa += "|" + myRow.ImpPreSgravio.ToString();
                        else
                            sDatiStampa += "|";
                        if (MetodiMinuta.AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                            return null;
                    }
                }
                else
                {
                    Log.Debug("PrintAvvisi::dataset vuoto causa errore non posso stampare");
                }
                return dtStampa;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "." + DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCartella.PrintAvvisi.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <returns></returns>
        public static string PrintAvvisiRowDati(Cartella myRow)
        {
            string sDatiStampa = "";

            try
            {
                sDatiStampa += myRow.DettaglioContribuente.Cognome;
                sDatiStampa += "|" + myRow.DettaglioContribuente.Nome;
                if (myRow.DettaglioContribuente.PartitaIva != "")
                    sDatiStampa += "|'" + myRow.DettaglioContribuente.PartitaIva;
                else
                    sDatiStampa += "|'" + myRow.DettaglioContribuente.CodiceFiscale;
                sDatiStampa += "|" + myRow.Anno;
                sDatiStampa += "|" + SharedFunction.FormattaData(myRow.DataEmissione);
                sDatiStampa += "|'" + myRow.CodiceCartella;
                sDatiStampa += "|" + myRow.ImportoCarico.ToString();
                sDatiStampa += "|" + myRow.ImpPagato.ToString();
                if (myRow.ImpPreSgravio > 0)
                    sDatiStampa += "|" + myRow.ImpPreSgravio.ToString();
                else
                    sDatiStampa += "|";

                return sDatiStampa;
            }
            catch (Exception Err)
            {
                Log.Debug(myRow.IdEnte + " - OPENgovOSAP.MetodiCartella.PrintAvvisi.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="cmdMyCommand"></param>
        /// <param name="HasArrotondamento"></param>
        /// <param name="IsSgravio"></param>
        /// <param name="Operatore"></param>
        public static void InsertCartella(ref Cartella cart, ref SqlCommand cmdMyCommand, bool HasArrotondamento, int IsSgravio, string Operatore)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DAO.InsertCartella(ref cart, ref cmdMyCommand, HasArrotondamento, IsSgravio, Operatore);
            }
            catch (Exception Err)
            {
                Log.Debug(cart.IdEnte + " - OPENgovOSAP.MetodiCartella.InsertCartella.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IdEnte"></param>
        /// <param name="CodTributo"></param>
        /// <param name="Anno"></param>
        /// <returns></returns>
        public static string GetNAvvisoAutomatico(string ConnectionString, string IdEnte, string CodTributo, string Anno)
        {
            DAO.CartelleDAO objDAO = new DAO.CartelleDAO();

            try
            {
                return objDAO.GetNAvvisoAutomatico(ConnectionString, IdEnte, CodTributo, Anno);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "." + DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCartella.GetNAvvisoAutomatico.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="cmdMyCommand"></param>
        /// <param name="Operatore"></param>
        public static void UndoSgravio(ref Cartella cart, ref SqlCommand cmdMyCommand, string Operatore)
        {
            try
            {
                new DAO.CartelleDAO().UndoSgravio(ref cart, ref cmdMyCommand, Operatore);
            }
            catch (Exception Err)
            {
                Log.Debug(cart.IdEnte + " - OPENgovOSAP.MetodiCartella.UndoSgravio.errore: ", Err);
                throw (Err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="cmdMyCommand"></param>
        public static void SetCodiceCartella(Cartella cart, ref SqlCommand cmdMyCommand)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DAO.SetCodiceCartella(cart, ref cmdMyCommand);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "." + DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiCartella.SetCodiceCartella.errore: ", Err);
                throw (Err);
            }
        }
    }
}