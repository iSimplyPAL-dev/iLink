using System;
using System.Data;
using System.Collections;
using log4net;
using IRemInterfaceOSAP;
using System.ComponentModel;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione pagamenti
    /// </summary>
    public class MetodiPagamento
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiPagamento));

        /// <summary>
        /// 
        /// </summary>
        public static string[] COL_HEADERS_SinglePag = new string[]{ "Nominativo", "Cod.Fiscale/P.IVA"
            , "Anno", "Codice Cartella", "Data emissione", "Importo carico"
			, "Numero rata", "Data pagamento", "Data riversamento", "Importo pagato"};
        /// <summary>
        /// 
        /// </summary>
        public static string[] COL_HEADERS_MagPag = new string[]{ "Nominativo", "Cod.Fiscale/P.IVA"
            , "Anno", "Codice Cartella", "Data emissione", "Importo carico"
			, "Importo pagato"};
        /// <summary>
        /// 
        /// </summary>
        public static string[] COL_HEADERS_MinPag = new string[]{ "Nominativo", "Cod.Fiscale/P.IVA"
	        , "Via Res.", "Civico Res.", "CAP Res.", "Comune Res.", "Prov. Res."
	        , "Nominativo Invio"
	        , "Via Invio", "Civico Invio", "CAP Invio", "Comune Invio", "Prov. Invio"
            , "Anno", "Codice Cartella", "Data emissione", "Importo carico"
            , "Importo pagato", "Importo Insoluto"};
        /// <summary>
        /// 
        /// </summary>
        public enum E_COLUMNS_SinglePag
        {
            NOMINATIVO = 0,
            CFPIVA = 1,
            ANNO = 2,
            CODICE_CARTELLA = 3,
            DATA_EMISSIONE = 4,
            IMPORTO_CARICO = 5,
            NUMERO_RATA = 6,
            DATA_PAGAMENTO = 7,
            DATA_ACCREDITO = 8,
            IMPORTO_PAGATO = 9
        }
        /// <summary>
        /// 
        /// </summary>
        public enum E_COLUMNS_TotPag
        {
            NOMINATIVO = 0,
            CFPIVA = 1,
            ANNO = 2,
            CODICE_CARTELLA = 3,
            DATA_EMISSIONE = 4,
            IMPORTO_CARICO = 5,
            IMPORTO_PAGATO = 6
        }
        /// <summary>
        /// 
        /// </summary>
        public enum E_COLUMNS_NoPag
        {
            NOMINATIVO = 0,
            CFPIVA = 1,
            VIA_RES = 2, CIVICO_RES = 3, CAP_RES = 4, COMUNE_RES = 5, PROVINCIA_RES = 6,
            NOMINATIVOCO = 7,
            INDIRIZZOCO = 8, CIVICOCO = 9, CAPCO = 10, COMUNECO = 11, PVCO = 12,
            ANNO = 13,
            CODICE_CARTELLA = 14,
            DATA_EMISSIONE = 15,
            IMPORTO_CARICO = 16,
            IMPORTO_PAGATO = 17,
            IMPORTO_INSOLUTO = 18
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeToExport"></param>
        /// <returns></returns>
        public static int[] GetColumnsToExport(string TypeToExport)
        {
            try {
                Type myType = typeof(E_COLUMNS_SinglePag);
                switch (TypeToExport)
                {
                    case "_ExpImportoMaggiore":
                        myType = typeof(E_COLUMNS_TotPag);
                        break;
                    case "_ExpNonPagati":
                    case "_ExpImportoMinore":
                        myType = typeof(E_COLUMNS_NoPag);
                        break;
                    default:
                        myType = typeof(E_COLUMNS_SinglePag);
                        break;
                }
                int[] Columns = new int[Enum.GetValues(myType).Length];
                checked
                {
                    for (int i = 0; i < Columns.Length; i++)
                        Columns[i] = (int)Enum.GetValues(myType).GetValue(i);
                }
                return Columns;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetColumnsToExport.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEnte"></param>
        /// <param name="idContribuente"></param>
        /// <param name="codiceCartella"></param>
        /// <returns></returns>
        public static double GetTotalePagatoPerCatella(string idEnte, int idContribuente, string codiceCartella)
        {
            DAO.PagamentiDAO DAO = new DAO.PagamentiDAO();

            try
            {
                double res = 0;
                res = DAO.GetTotalePagatoPerCatella(idEnte, idContribuente, codiceCartella);
                return res;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetTotalePagatoPerCartella.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCartella"></param>
        /// <param name="IdEnte"></param>
        /// <returns></returns>
        public static PagamentoExt[] GetPagamentiCartella(int IdCartella, string IdEnte)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetPagamentiCartella(IdCartella, IdEnte);
                //return FillPagamentiFromDataTable(dt);
                return FillPagamentiExtFromDataTable(dt);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetPagamentiCartella.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// Metodo per recuperare i pagamenti 
        /// </summary>
        /// <param name="idPagamento"></param>
        /// <returns></returns>
        public static PagamentoExt GetPagamentoById(int idPagamento)
        {
            if (idPagamento <= 0)
                throw new Exception("L'oggetto 'PagamentiSearch' non può essere nullo");

            DAO.PagamentiDAO DAO = new DAO.PagamentiDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetPagamentoById(idPagamento);
                if (dt != null)
                {
                    PagamentoExt[] res = FillPagamentiExtFromDataTable(dt);
                    if (res != null && res.Length > 0)
                        return res[0];
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetPagamentoById.errore: ", Err);
                throw (Err);
            }
        }


        /// <summary>
        /// Metodo per recuperare i pagamenti 
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <returns></returns>
        public static PagamentoExt[] GetPagamenti(PagamentiSearch searchParameter)
        {
            try {
                DataTable tmp;
                return GetPagamenti(searchParameter, null, out tmp);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetPagamenti.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="sufxxExp"></param>
        /// <returns></returns>
        public static PagamentoExt[] GetPagamenti(PagamentiSearch searchParameter, string sufxxExp)
        {
            try {
                DataTable tmp;
                return GetPagamenti(searchParameter, sufxxExp, out tmp);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetPagamenti.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// Metodo per recuperare i pagamenti 
        /// </summary>
        /// <param name="searchParameter"></param>
        /// <param name="sufxxExp"></param>
        /// <param name="dtRes"></param>
        /// <returns></returns>
        public static PagamentoExt[] GetPagamenti(PagamentiSearch searchParameter, string sufxxExp, out DataTable dtRes)
        {
            if (searchParameter == null)
                throw new Exception("L'oggetto 'PagamentiSearch' non può essere nullo");

            DAO.PagamentiDAO DAO = new DAO.PagamentiDAO();

            try
            {
                DataTable dt = null;
                dtRes = null;
                dt = DAO.RicercaPagamenti(searchParameter, sufxxExp);
                if (dt != null)
                {
                    dtRes = dt;
                    if (!SharedFunction.IsNullOrEmpty(sufxxExp))
                        return new PagamentoExt[0];
                    else
                        return FillPagamentiExtFromDataTable(dt);
                }
                else
                    return new PagamentoExt[0];
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.GetPagamenti.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// Inserisce o modifica un pagamento
        /// </summary>
        /// <param name="objToInsertOrUpdate"></param>
        /// <param name="operatore"></param>
        /// <returns></returns>
        public static int InsertUpdatePagamento(PagamentoExt objToInsertOrUpdate, string operatore)
        {
            if (objToInsertOrUpdate == null)
                throw new Exception("L'oggetto 'objToInsertOrUpdate' non può essere nullo");

            DAO.PagamentiDAO DAO = new DAO.PagamentiDAO();
            int res = 0;

            try
            {
                res = DAO.InsertUpdatePagamento(objToInsertOrUpdate, operatore);
            }

            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.InsertUpdatePagamento.errore: ", Err);
                throw (Err);
            }
            return res;
        }

        /// <summary>
        /// Inserisce o modifica un pagamento
        /// </summary>
        /// <param name="idPagamento"></param>
        /// <returns></returns>
        public static int DeletePagamento(int idPagamento)
        {
            DAO.PagamentiDAO DAO = new DAO.PagamentiDAO();
            int res = 0;

            try
            {
                res = DAO.DeletePagamento(idPagamento);
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.DeletePagamento.errore: ", Err);
                throw (Err);
            }

            return res;
        }

        /// <summary>
        /// S.T. Riscrittura del metodo
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static PagamentoExt[] FillPagamentiExtFromDataTable(DataTable dt)
        {
            try
            {
                ArrayList MyArray = new ArrayList();
                PagamentoExt CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new PagamentoExt();
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    CurrentItem.IdPagamento = (int)myRow["IDPAGAMENTO"];
                    CurrentItem.CodContribuente = (int)myRow["COD_CONTRIBUENTE"];
                    CurrentItem.IdEnte = (string)myRow["IDENTE"];

                    CurrentItem.Anno = (string)myRow["ANNO"];
                    CurrentItem.CartellaTOCO = new Cartella();

                    if (myRow["IDCARTELLA"] != DBNull.Value)
                        CurrentItem.CartellaTOCO.IdCartella = (int)myRow["IDCARTELLA"];

                    CurrentItem.CartellaTOCO.CodiceCartella = (string)myRow["CODICE_CARTELLA"];

                    CurrentItem.CodiceCartella = CurrentItem.CartellaTOCO.CodiceCartella;

                    //CurrentItem.NumeroRata = (int)myRow["NUMERO_RATA"];
                    if (myRow["NUMERO_RATA"] != DBNull.Value)
                        CurrentItem.NumeroRataString = (string)myRow["NUMERO_RATA"];

                    CurrentItem.Nominativo = (string)myRow["COGNOME_DENOMINAZIONE"] + " " +
                        (string)myRow["NOME"];
                    CurrentItem.CFPIVA = (string)myRow["cfpiva"];
                    CurrentItem.ImportoPagato = double.Parse(myRow["IMPORTO_PAGATO"].ToString());
                    if (myRow["DATA_ACCREDITO"] != DBNull.Value)
                        CurrentItem.DataAccredito = (DateTime)myRow["DATA_ACCREDITO"];
                    else
                        CurrentItem.DataAccredito = DateTime.MaxValue;
                    CurrentItem.DataPagamento = (DateTime)myRow["DATA_PAGAMENTO"];
                    if (myRow["PROVENIENZA"] != DBNull.Value)
                        CurrentItem.Provenienza = (string)myRow["PROVENIENZA"];
                    else
                        CurrentItem.Provenienza = string.Empty;
                    if (myRow["CODICE_BOLLETTINO"] != DBNull.Value)
                        CurrentItem.CodiceBollettino = (string)myRow["CODICE_BOLLETTINO"];
                    else
                        CurrentItem.CodiceBollettino = string.Empty;

                    MyArray.Add(CurrentItem);
                }

                return (PagamentoExt[])MyArray.ToArray(typeof(PagamentoExt));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.FillPagamentiExtFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Pagamento[] FillPagamentiFromDataTable(DataTable dt)
        {
            try {
                ArrayList MyArray = new ArrayList();
                Pagamento CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Pagamento();

                    CurrentItem.IdPagamento = (int)myRow["IDPAGAMENTO"];
                    CurrentItem.CodContribuente = (int)myRow["COD_CONTRIBUENTE"];
                    CurrentItem.IdEnte = (string)myRow["IDENTE"];
                    CurrentItem.CartellaTOCO = new Cartella();

                    if (myRow["IDCARTELLA"] != DBNull.Value)
                        CurrentItem.CartellaTOCO.IdCartella = (int)myRow["IDCARTELLA"];

                    CurrentItem.NumeroRata = (int)myRow["NUMERO_RATA"];
                    CurrentItem.ImportoPagato = double.Parse(myRow["IMPORTO_PAGATO"].ToString());
                    if (myRow["DATA_ACCREDITO"] != DBNull.Value)
                        CurrentItem.DataAccredito = (DateTime)myRow["DATA_ACCREDITO"];
                    else
                        CurrentItem.DataAccredito = DateTime.MaxValue;
                    CurrentItem.DataPagamento = (DateTime)myRow["DATA_PAGAMENTO"];
                    if (myRow["PROVENIENZA"] != DBNull.Value)
                        CurrentItem.Provenienza = (string)myRow["PROVENIENZA"];
                    else
                        CurrentItem.Provenienza = string.Empty;
                    if (myRow["CODICE_BOLLETTINO"] != DBNull.Value)
                        CurrentItem.CodiceBollettino = (string)myRow["CODICE_BOLLETTINO"];
                    else
                        CurrentItem.CodiceBollettino = string.Empty;

                    MyArray.Add(CurrentItem);
                }

                return (Pagamento[])MyArray.ToArray(typeof(Pagamento));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiPagamento.FillPagamentiFromDataTable.errore: ", Err);
                throw (Err);
            }
        }
    }
}