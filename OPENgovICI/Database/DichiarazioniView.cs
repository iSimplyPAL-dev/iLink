using System;
using System.Data;
using System.Data.SqlClient;
using log4net;
using AnagInterface;
using Utility;

namespace DichiarazioniICI.Database
{

    /// <summary>
    /// Rappresenta una riga della tabella viewDichiarazioni.
    /// </summary>
    public struct DichiarazioniViewRow
    {
        /// 
        public int ID;
        /// 
        public string Ente;
        /// 
        public int NumeroDichiarazione;
        /// 
        public string AnnoDichiarazione;
        /// 
        public string NumeroProtocollo;
        /// 
        public string DataProtocollo;
        /// 
        public string TotaleModelli;
        /// 
        public string DataInizio;
        /// 
        public string DataFine;
        /// 
        public int IDContribuente;
        /// 
        public int IDDenunciante;
        /// 
        public bool Effettivo;
        /// 
        public bool Annullato;
        /// 
        public string DataInizioValidità;
        /// 
        public string DataFineValidità;
        /// 
        public string Operatore;
        /// 
        public string NumeroOrdine;
        /// 
        public string NumeroModello;
        /// 
        public int IdSoggetto;
        /// 
        //*** 20140509 - TASI ***
        public int TipoUtilizzo;
        /// 
        public int TipoPossesso;
        //*** ***
        public System.Double PercPossesso;
        /// 
        public short MesiEsclusioneEsenzione;
        /// 
        public short MesiPossesso;
        /// 
        public short MesiRiduzione;
        /// 
        public System.Decimal ImpDetrazAbitazPrincipale;
        /// 
        public bool Contitolare;
        /// 
        public int AbitazionePrincipale;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Bonificato
    {
        /// 
        Tutte = -1,
        /// 
        Bonificate,
        /// 
        DaBonificare
    }

    /// <summary>
    /// Classe di gestione della tabella viewDichiarazioni.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class DichiarazioniView : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DichiarazioniView));
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public DichiarazioniView()
        {
            this.TableName = "viewDichiarazioni";
            //*** 20131003 - gestione atti compravendita ***
            this._DbConnection = new SqlConnection(Business.ConstWrapper.StringConnectionOPENgov);
            //*** ***
        }

         /*public DataTable SerarchByContribuenteContitolare(int codiceContribuente, Bonificato bonificato)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand("Select * From " + this.TableName + " Where idContribuente=@codContribuente OR idSoggetto=@codContribuente", this._DbConnection);
                switch (bonificato)
                {
                    case Bonificato.Bonificate:
                        SelectCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE (IdContribuente=@codContribuente AND Bonificato=1 AND Storico <> 1) OR (IdSoggetto=@codContribuente AND Bonificato=1 AND Storico <> 1)";
                        break;

                    case Bonificato.DaBonificare:
                        SelectCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE (IdContribuente=@codContribuente AND Bonificato=0 AND Storico <> 1) OR (IdSoggetto=@codContribuente AND Bonificato=0 AND Storico <> 1)";
                        break;
                }
                SelectCommand.Parameters.Add("@codContribuente", SqlDbType.Int).Value = codiceContribuente;
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.SerarchByContribuenteContitolare.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }*/


        /// <summary>
        /// Ritorna una riga identificata dall'identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DichiarazioniViewRow GetRow(int id)
        {
            DichiarazioniViewRow riga = new DichiarazioniViewRow();
            SqlCommand SelectCommand = PrepareGetRow(id);
            DataTable tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            kill();
            try
            {
                if (tabella.Rows.Count > 0)
                {
                    riga.ID = (System.Int32)tabella.Rows[0]["ID"];
                    riga.Ente = (System.String)tabella.Rows[0]["Ente"];
                    riga.NumeroDichiarazione = (System.Int32)tabella.Rows[0]["NumeroDichiarazione"];
                    riga.AnnoDichiarazione = (System.String)tabella.Rows[0]["AnnoDichiarazione"];
                    riga.NumeroProtocollo = (System.String)tabella.Rows[0]["NumeroProtocollo"];
                    riga.DataProtocollo = (System.String)tabella.Rows[0]["DataProtocollo"];
                    riga.TotaleModelli = (System.String)tabella.Rows[0]["TotaleModelli"];
                    riga.DataInizio = (System.String)tabella.Rows[0]["DataInizio"];
                    riga.DataFine = (System.String)tabella.Rows[0]["DataFine"];
                    riga.IDContribuente = (System.Int32)tabella.Rows[0]["IDContribuente"];
                    riga.IDDenunciante = (System.Int32)tabella.Rows[0]["IDDenunciante"];
                    riga.Effettivo = (System.Boolean)tabella.Rows[0]["Effettivo"];
                    riga.Annullato = (System.Boolean)tabella.Rows[0]["Annullato"];
                    riga.DataInizioValidità = (System.String)tabella.Rows[0]["DataInizioValidità"];
                    riga.DataFineValidità = (System.String)tabella.Rows[0]["DataFineValidità"];
                    riga.Operatore = (System.String)tabella.Rows[0]["Operatore"];
                    riga.NumeroOrdine = (System.String)tabella.Rows[0]["NumeroOrdine"];
                    riga.NumeroModello = (System.String)tabella.Rows[0]["NumeroModello"];
                    riga.IdSoggetto = (System.Int32)tabella.Rows[0]["IdSoggetto"];
                    //*** 20140509 - TASI ***
                    riga.TipoUtilizzo = (System.Int32)tabella.Rows[0]["IdTipoUtilizzo"];
                    riga.TipoPossesso = (System.Int32)tabella.Rows[0]["IdTipoPossesso"];
                    //*** ***
                    riga.PercPossesso = (System.Double)tabella.Rows[0]["PercPossesso"];
                    riga.MesiEsclusioneEsenzione = (System.Int16)tabella.Rows[0]["MesiEsclusioneEsenzione"];
                    riga.MesiPossesso = (System.Int16)tabella.Rows[0]["MesiPossesso"];
                    riga.MesiRiduzione = (System.Int16)tabella.Rows[0]["MesiRiduzione"];
                    riga.ImpDetrazAbitazPrincipale = (System.Decimal)tabella.Rows[0]["ImpDetrazAbitazPrincipale"];
                    riga.Contitolare = (System.Boolean)tabella.Rows[0]["Contitolare"];
                    riga.AbitazionePrincipale = (System.Int32)tabella.Rows[0]["AbitazionePrincipale"];
                }
                return riga;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetRow.errore: ", Err);
                throw Err;
            }
        }

        //*** 20131003 - gestione atti compravendita ***
        /// <summary>
        /// 
        /// </summary>
        public const int ProvenienzaDich_Conservatoria = 42;
        /// <summary>
        /// Funzione per il reperimento degli atti di compravendita per il soggetto
        /// </summary>
        /// <param name="idContribuente"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        public DataTable GetCompraVenditeFromSoggetto(int idContribuente)
        {
            DataTable Tabella;

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnectionOPENgov))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetAttiFromSoggettoICI", "idSoggetto");
                    DataSet myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("idSoggetto", idContribuente));
                    Tabella = myDataSet.Tables[0];
                    ctx.Dispose();
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVenditeFromSoggetto.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCompraVendita"></param>
        /// <returns></returns>
        public DataTable GetCompraVendita(int idCompraVendita)
        {
            DataTable Tabella;

            try
            {
                log.Debug("GetCompraVendita::_DbConnection::" + this._DbConnection.ConnectionString + "::idCompraVendita::" + idCompraVendita.ToString());
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.Connection = this._DbConnection;
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetAttiCompraVendita";
                SelectCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idCompraVendita;
                Tabella = Query(SelectCommand, this._DbConnection);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVendita.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCompraVendita"></param>
        /// <param name="idContribuente"></param>
        /// <param name="idSoggettoAtto"></param>
        /// <returns></returns>
        public DataTable GetCompraVenditaSoggetto(int idCompraVendita, int idContribuente, int idSoggettoAtto)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.Connection = this._DbConnection;
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetAttiSoggettiNota";
                SelectCommand.Parameters.Add("@idAtto", SqlDbType.Int).Value = idCompraVendita;
                SelectCommand.Parameters.Add("@IdSoggetto", SqlDbType.Int).Value = idContribuente;
                SelectCommand.Parameters.Add("@IdSoggettoAtto", SqlDbType.Int).Value = idSoggettoAtto;
                Tabella = Query(SelectCommand, this._DbConnection);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVenditaSoggetto.errore: ", Err);

                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCompraVendita"></param>
        /// <param name="idSoggetto"></param>
        /// <returns></returns>
        public DataTable GetCompraVenditaImmobile(int idCompraVendita, int idSoggetto)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.Connection = this._DbConnection;
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetAttiCompraVenditaImmobile";
                SelectCommand.Parameters.Add("@idAtto", SqlDbType.Int).Value = idCompraVendita;
                SelectCommand.Parameters.Add("@idSoggetto", SqlDbType.Int).Value = idSoggetto;
                Tabella = Query(SelectCommand, this._DbConnection);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVenditaImmobile.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idTestata"></param>
        /// <param name="idSoggetto"></param>
        /// <returns></returns>
        public Utility.DichManagerICI.OggettiRow GetCompraVenditaOggetti(int id, int idTestata, int idSoggetto)
        {
            Utility.DichManagerICI.OggettiRow Oggetto = new Utility.DichManagerICI.OggettiRow();
            DataTable Tabella;

            try
            {
                Tabella = GetCompraVenditaImmobile(id, idSoggetto);
                if (Tabella.Rows.Count > 0)
                {
                    Oggetto.ID = (int)Tabella.Rows[0]["IDoggetto"];
                    Oggetto.Ente = (string)Tabella.Rows[0]["IdEnte"];
                    Oggetto.IdTestata = idTestata;
                    Oggetto.NumeroOrdine = (string)Tabella.Rows[0]["NumeroOrdine"];
                    Oggetto.NumeroModello = (string)Tabella.Rows[0]["NumeroModello"];
                    Oggetto.CodUI = Tabella.Rows[0]["CodUI"] == DBNull.Value ? string.Empty : Tabella.Rows[0]["CodUI"].ToString();
                    Oggetto.TipoImmobile = Tabella.Rows[0]["TipoImmobile"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["TipoImmobile"];
                    Oggetto.PartitaCatastale = Tabella.Rows[0]["PartitaCatastale"] == DBNull.Value ? 0 : int.Parse(Tabella.Rows[0]["PartitaCatastale"].ToString());
                    Oggetto.Foglio = Tabella.Rows[0]["Foglio"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Foglio"];
                    Oggetto.Numero = Tabella.Rows[0]["Numero"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Numero"];
                    Oggetto.Subalterno = Tabella.Rows[0]["Subalterno"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["Subalterno"];
                    Oggetto.Caratteristica = Tabella.Rows[0]["Caratteristica"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["Caratteristica"];
                    Oggetto.Sezione = Tabella.Rows[0]["Sezione"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Sezione"];
                    Oggetto.NumeroProtCatastale = Tabella.Rows[0]["NumeroProtCatastale"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["NumeroProtCatastale"];
                    Oggetto.AnnoDenunciaCatastale = Tabella.Rows[0]["AnnoDenunciaCatastale"] == DBNull.Value ? String.Empty : Tabella.Rows[0]["AnnoDenunciaCatastale"].ToString();
                    Oggetto.CodCategoriaCatastale = Tabella.Rows[0]["CodCategoriaCatastale"] == DBNull.Value ? "0" : (string)Tabella.Rows[0]["CodCategoriaCatastale"];
                    Oggetto.CodClasse = Tabella.Rows[0]["CodClasse"] == DBNull.Value ? "0" : Tabella.Rows[0]["CodClasse"].ToString();
                    Oggetto.CodRendita = Tabella.Rows[0]["CodRendita"] == DBNull.Value ? String.Empty : Tabella.Rows[0]["CodRendita"].ToString();
                    Oggetto.Storico = Tabella.Rows[0]["Storico"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["Storico"];
                    Oggetto.ValoreImmobile = Convert.ToDecimal(Tabella.Rows[0]["ValoreImmobile"].ToString());
                    Oggetto.IDValuta = Tabella.Rows[0]["IDValuta"] == DBNull.Value ? 1 : (int)Tabella.Rows[0]["IDValuta"];
                    Oggetto.FlagValoreProvv = Tabella.Rows[0]["FlagValoreProvv"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["FlagValoreProvv"];
                    Oggetto.CodComune = Tabella.Rows[0]["CodComune"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["CodComune"];
                    Oggetto.Comune = Tabella.Rows[0]["Comune"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Comune"];
                    Oggetto.CodVia = Tabella.Rows[0]["CodVia"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["CodVia"];
                    Oggetto.Via = Tabella.Rows[0]["Via"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Via"];
                    Oggetto.NumeroCivico = Tabella.Rows[0]["NumeroCivico"] == DBNull.Value ? 0 : int.Parse(Tabella.Rows[0]["NumeroCivico"].ToString());
                    Oggetto.EspCivico = Tabella.Rows[0]["EspCivico"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["EspCivico"];
                    Oggetto.Scala = Tabella.Rows[0]["Scala"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Scala"];
                    Oggetto.Interno = Tabella.Rows[0]["Interno"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Interno"];
                    Oggetto.Piano = Tabella.Rows[0]["Piano"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Piano"];
                    Oggetto.Barrato = Tabella.Rows[0]["Barrato"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["Barrato"];
                    Oggetto.NumeroEcografico = Tabella.Rows[0]["NumeroEcografico"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["NumeroEcografico"];
                    Oggetto.TitoloAcquisto = (int)Tabella.Rows[0]["TitoloAcquisto"];
                    Oggetto.TitoloCessione = (int)Tabella.Rows[0]["TitoloCessione"];
                    Oggetto.DescrUffRegistro = Tabella.Rows[0]["DescrUffRegistro"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["DescrUffRegistro"];
                    Oggetto.DataInizioValidità = (DateTime)Tabella.Rows[0]["DataInizioValidita"];
                    Oggetto.DataFineValidità = Tabella.Rows[0]["DataFineValidita"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[0]["DataFineValidita"];
                    Oggetto.DataInizio = Tabella.Rows[0]["DataInizio"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[0]["DataInizio"];
                    Oggetto.DataFine = Tabella.Rows[0]["DataFine"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Tabella.Rows[0]["DataFine"];
                    Oggetto.Bonificato = Tabella.Rows[0]["Bonificato"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["Bonificato"];
                    Oggetto.Annullato = Tabella.Rows[0]["Annullato"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["Annullato"];
                    Oggetto.DataUltimaModifica = Tabella.Rows[0]["DataUltimaModifica"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[0]["DataUltimaModifica"];
                    Oggetto.Operatore = (string)Tabella.Rows[0]["Operatore"];
                    Oggetto.IDImmobilePertinente = Tabella.Rows[0]["IdImmobilePertinente"] == DBNull.Value ? -1 : (int)Tabella.Rows[0]["IdImmobilePertinente"];
                    Oggetto.NoteIci = Tabella.Rows[0]["NoteIci"] == DBNull.Value ? String.Empty : (string)Tabella.Rows[0]["NoteIci"];
                    Oggetto.Zona = Tabella.Rows[0]["Zona"] == DBNull.Value ? string.Empty : Tabella.Rows[0]["Zona"].ToString();
                    Oggetto.Rendita = Tabella.Rows[0]["Rendita"] == DBNull.Value ? -1 : decimal.Parse(Tabella.Rows[0]["Rendita"].ToString());
                    Oggetto.Consistenza = Tabella.Rows[0]["Consistenza"] == DBNull.Value ? -1 : decimal.Parse(Tabella.Rows[0]["Consistenza"].ToString());
                    Oggetto.ExRurale = Tabella.Rows[0]["ExRurale"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["ExRurale"];

                }
            }
            catch (Exception ex)
            {
                kill();
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVenditaOggetti.errore: ", ex);
                Oggetto = new Utility.DichManagerICI.OggettiRow();
            }
            finally
            {
                kill();
            }
            return Oggetto;
        }
        /// <summary>
        /// Funzione che preleva il dettaglio testata dalla compravendita
        /// </summary>
        /// <param name="id">int identificativo</param>
        /// <param name="idTestata">int identificativo testata</param>
        /// <param name="idSoggetto">int identificativo contribuente</param>
        /// <returns>DettaglioTestataRow oggetto che risponde ai criteri</returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public Utility.DichManagerICI.DettaglioTestataRow GetCompraVenditaDettaglioTestata(int id, int idTestata, int idSoggetto)
        {
            DataTable Tabella;
            Utility.DichManagerICI.DettaglioTestataRow Details = new Utility.DichManagerICI.DettaglioTestataRow();

            try
            {
                Tabella = GetCompraVenditaImmobile(id, idSoggetto);
                if (Tabella.Rows.Count > 0)
                {
                    Details.ID = (int)Tabella.Rows[0]["IDdettaglio"];
                    Details.Ente = (string)Tabella.Rows[0]["IdEnte"];
                    Details.IdTestata = idTestata;
                    Details.NumeroOrdine = (string)Tabella.Rows[0]["NumeroOrdine"];
                    Details.NumeroModello = (string)Tabella.Rows[0]["NumeroModello"];
                    Details.IdOggetto = (int)Tabella.Rows[0]["IdOggetto"];
                    Details.IdSoggetto = (int)Tabella.Rows[0]["IdSoggetto"];
                    //*** 20140509 - TASI ***
                    Details.TipoUtilizzo = Tabella.Rows[0]["IdTipoUtilizzo"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["IdTipoUtilizzo"];
                    Details.TipoPossesso = Tabella.Rows[0]["IdTipoPossesso"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["IdTipoPossesso"];
                    //*** ***
                    Details.PercPossesso = Tabella.Rows[0]["PercPossesso"] == DBNull.Value ? 0 : Convert.ToDecimal(Tabella.Rows[0]["PercPossesso"]);
                    Details.MesiPossesso = Tabella.Rows[0]["MesiPossesso"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["MesiPossesso"];
                    Details.MesiEsclusioneEsenzione = Tabella.Rows[0]["MesiEsclusioneEsenzione"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["MesiEsclusioneEsenzione"];
                    Details.MesiRiduzione = Tabella.Rows[0]["MesiRiduzione"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["MesiRiduzione"];
                    Details.ImpDetrazAbitazPrincipale = Tabella.Rows[0]["ImpDetrazAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Tabella.Rows[0]["ImpDetrazAbitazPrincipale"]);
                    Details.Contitolare = Tabella.Rows[0]["Contitolare"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["Contitolare"];
                    Details.AbitazionePrincipale = (int)Tabella.Rows[0]["AbitazionePrincipale"];
                    Details.Bonificato = Tabella.Rows[0]["Bonificato"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["Bonificato"];
                    Details.Annullato = Tabella.Rows[0]["Annullato"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["Annullato"];
                    Details.DataInizioValidità = (DateTime)Tabella.Rows[0]["DataInizioValidita"];
                    Details.DataFineValidità = Tabella.Rows[0]["DataFineValidita"] == DBNull.Value ? DateTime.MinValue : (DateTime)Tabella.Rows[0]["DataFineValidita"];
                    Details.DataUltimaModifica = Tabella.Rows[0]["DataUltimaModifica"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Tabella.Rows[0]["DataUltimaModifica"];
                    Details.Operatore = (string)Tabella.Rows[0]["Operatore"];
                    Details.Riduzione = (int)Tabella.Rows[0]["Riduzione"];
                    Details.Possesso = (int)Tabella.Rows[0]["Possesso"];
                    Details.EsclusioneEsenzione = (int)Tabella.Rows[0]["EsclusioneEsenzione"];
                    Details.AbitazionePrincipaleAttuale = (int)Tabella.Rows[0]["AbitazionePrincipaleAttuale"];
                    Details.NumeroUtilizzatori = Tabella.Rows[0]["NumeroUtilizzatori"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["NumeroUtilizzatori"];
                    //*** 20120629 - IMU ***
                    Details.ColtivatoreDiretto = Tabella.Rows[0]["coltivatorediretto"] == DBNull.Value ? false : (bool)Tabella.Rows[0]["coltivatorediretto"];
                    Details.NumeroFigli = Tabella.Rows[0]["numerofigli"] == DBNull.Value ? 0 : (int)Tabella.Rows[0]["numerofigli"];
                    //*** ***
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVenditaDettaglioTestata.errore: ", Err);
                kill();
                Details = new Utility.DichManagerICI.DettaglioTestataRow();
            }
            finally
            {
                kill();
            }

            return Details;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCompraVendita"></param>
        /// <param name="idContribuente"></param>
        /// <param name="TipoSoggettoInNota"></param>
        /// <returns></returns>
        public bool SetCompraVenditaSoggetto(int idCompraVendita, int idContribuente, string TipoSoggettoInNota)
        {
            bool bRet;

            try
            {
                log.Debug("SetCompraVenditaSoggetto::_DbConnection::" + this._DbConnection.ConnectionString + "::TipoSoggettoInNota::" + TipoSoggettoInNota + "::idCompraVendita::" + idCompraVendita.ToString() + "::idContribuente::" + idContribuente.ToString());
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.Connection = this._DbConnection;
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_SetStatoSoggettiNota";
                SelectCommand.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = TipoSoggettoInNota;
                SelectCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idCompraVendita;
                SelectCommand.Parameters.Add("@idSoggetto", SqlDbType.Int).Value = idContribuente;
                SelectCommand.Parameters.Add("@Stato", SqlDbType.Int).Value = 1; //per ora fisso = trattato
                bRet = Execute(SelectCommand, this._DbConnection);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.SetCompraVenditaSoggetto.errore: ", Err);
                kill();
                bRet = false;
            }
            finally
            {
                kill();
            }

            return bRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idAttoCompraVendita"></param>
        /// <param name="idAttoSoggetto"></param>
        /// <returns></returns>
        public DataTable GetCompravenditaAnagrafica(int idAttoCompraVendita, int idAttoSoggetto)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.Connection = this._DbConnection;
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetAttiCompraVenditaAnagrafica";
                SelectCommand.Parameters.Add("@IdAttoCompravendita", SqlDbType.Int).Value = idAttoCompraVendita;
                SelectCommand.Parameters.Add("@idAttoSoggetto", SqlDbType.Int).Value = idAttoSoggetto;
                Tabella = Query(SelectCommand, this._DbConnection);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompravenditaAnagrafica.errore: ", Err);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idAttoCompraVendita"></param>
        /// <param name="idAttoSoggetto"></param>
        /// <returns></returns>
        public DettaglioAnagrafica GetCompraVenditaAnagrafica(int idAttoCompraVendita, int idAttoSoggetto)
        {
            DettaglioAnagrafica Oggetto = new DettaglioAnagrafica();
            DataTable Tabella;

            try
            {
                Tabella = GetCompravenditaAnagrafica(idAttoCompraVendita, idAttoSoggetto);
                if (Tabella.Rows.Count > 0)
                {
                    Oggetto.Nome = (string)Tabella.Rows[0]["Nome"];
                    Oggetto.Cognome = (string)Tabella.Rows[0]["Cognome"];
                    Oggetto.Sesso = (string)Tabella.Rows[0]["Sesso"];
                    Oggetto.DataNascita = Business.CoreUtility.FormattaDataGrd(Tabella.Rows[0]["DataNascita"].ToString());
                    Oggetto.ComuneNascita = (string)Tabella.Rows[0]["Comune_Nascita"];
                    Oggetto.ProvinciaNascita = (string)Tabella.Rows[0]["Provincia_Nascita"];
                    Oggetto.CodiceFiscale = (string)Tabella.Rows[0]["CodiceFiscale"];
                    Oggetto.ComuneResidenza = (string)Tabella.Rows[0]["Comune_Residenza"];
                    Oggetto.ProvinciaResidenza = (string)Tabella.Rows[0]["Provincia_Residenza"];
                    Oggetto.ViaResidenza = (string)Tabella.Rows[0]["Indirizzo_Residenza"];
                    Oggetto.CapResidenza = (string)Tabella.Rows[0]["Cap"];
                    Oggetto.CodEnte = Business.ConstWrapper.CodiceEnte;
                    Oggetto.Concurrency = DateTime.Now;
                    Oggetto.DataInizioValidita = DateTime.Now.ToString();
                    Oggetto.Operatore = Business.ConstWrapper.sUsername;
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.GetCompraVenditaAnagrafica.errore: ", Err);
                kill();
                Oggetto = new DettaglioAnagrafica();
            }
            finally
            {
                kill();
            }
            return Oggetto;
        }
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Foglio"></param>
        /// <param name="Numero"></param>
        /// <param name="Subalterno"></param>
        /// <param name="dtMyDati"></param>
        /// <returns></returns>
        public bool SearchAltriProprietari(string IdEnte, string Foglio, string Numero, string Subalterno, out DataTable dtMyDati)
        {
            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.Connection =new SqlConnection( Business.ConstWrapper.StringConnection);
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = "prc_GetUIAltriProprietari";
                SelectCommand.Parameters.Add("@IDENTE", SqlDbType.VarChar).Value = IdEnte;
                SelectCommand.Parameters.Add("@FOGLIO", SqlDbType.VarChar).Value = Foglio;
                SelectCommand.Parameters.Add("@NUMERO", SqlDbType.VarChar).Value = Numero;
                SelectCommand.Parameters.Add("@SUBALTERNO", SqlDbType.VarChar).Value = Subalterno;
                dtMyDati = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
                return true;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DichiarazioniView.SearchAltriProprietari.errore: ", Err);
                kill();
                dtMyDati = new DataTable();
                return false;
            }
            finally
            {
                kill();
            }
        }
    }
}
