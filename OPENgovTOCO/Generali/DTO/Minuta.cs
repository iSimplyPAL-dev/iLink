using System;
using System.Data;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using log4net;
using Anagrafica.DLL;
using IRemInterfaceOSAP;
using OPENgovTOCO;

namespace DTO
{
    /// <summary>
    /// Classe per la gestione Minuta.
    /// </summary>
    public class MetodiMinuta
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MetodiMinuta));

        private enum E_COLUMNS
        {
            [DescriptionAttribute("COGNOME")]
            COGNOME = 0,
            [DescriptionAttribute("NOME")]
            NOME = 1,
            [DescriptionAttribute("C.F./P.IVA")]
            CFPIVA = 2,
            [DescriptionAttribute("INDIRIZZO RESIDENZA")]
            INDIRIZZORESIDENZA = 3,
            [DescriptionAttribute("CIVICO RESIDENZA")]
            CIVICORESIDENZA = 4,
            [DescriptionAttribute("ESPONENTE RESIDENZA")]
            ESPONENTERESIDENZA = 5,
            [DescriptionAttribute("INTERNO RESIDENZA")]
            INTERNORESIDENZA = 6,
            [DescriptionAttribute("CAP RESIDENZA")]
            CAPRESIDENZA = 7,
            [DescriptionAttribute("COMUNE RESIDENZA")]
            COMUNERESIDENZA = 8,
            [DescriptionAttribute("PROVINCIA RESIDENZA")]
            PROVINCIARESIDENZA = 9,

            [DescriptionAttribute("N. DICHIARAZIONE")]
            NDICHIARAZIONE = 10,
            [DescriptionAttribute("UBICAZIONE")]
            UBICAZIONE = 11,
            [DescriptionAttribute("CIVICO")]
            CIVICO = 12,
            [DescriptionAttribute("ESPONENTE")]
            ESPONENTE = 13,
            [DescriptionAttribute("INTERNO")]
            INTERNO = 14,
            [DescriptionAttribute("TIPO OCCUPAZIONE")]
            TIPOOCCUPAZIONE = 15,
            [DescriptionAttribute("CATEGORIA")]
            CATEGORIA = 16,
            [DescriptionAttribute("ATTRAZIONE")]
            ATTRAZIONE = 17,
            [DescriptionAttribute("CONSISTENZA")]
            CONSISTENZA = 18,
            [DescriptionAttribute("DATA INIZIO OCCUPAZIONE")]
            DATAINIZIOOCCUPAZIONE = 19,
            [DescriptionAttribute("DURATA OCCUPAZIONE")]
            DURATAOCCUPAZIONE = 20,
            [DescriptionAttribute("DATA FINE OCCUPAZIONE")]
            DATAFINEOCCUPAZIONE = 21,
            [DescriptionAttribute("TARIFFA APPLICATA EURO")]
            TARIFFAAPPLICATA = 22,
            [DescriptionAttribute("AGEVOLAZIONE %")]
            AGEVOLAZIONEPERC = 23,
            [DescriptionAttribute("MAGGIORAZIONE %")]
            MAGGIORAZIONEPERC = 24,
            [DescriptionAttribute("MAGGIORAZIONE EURO")]
            MAGGIORAZIONEIMPORTO = 25,
            [DescriptionAttribute("DETRAZIONI EURO")]
            DETRAZIONI = 26,
            [DescriptionAttribute("IMPORTO TOTALE EURO")]
            IMPORTOTOTALE = 27
        }

        #region "Public methods"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myStringConnection"></param>
        /// <param name="IdEnte"></param>
        /// <param name="DescrizioneEnte"></param>
        /// <param name="IdFlusso"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="10/06/2013">
        /// <strong>ruolo supplettivo</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public static DataTable GetMinuta(string myStringConnection, string IdEnte,string DescrizioneEnte, int IdFlusso)
        {
            try
            {
                DataTable dtStampa = new DataTable();
                DataSet dsStampa = new DataSet();
                DataView dvDati = MetodiMinuta.GetMinutaRecords(myStringConnection,IdEnte,"",IdFlusso, false);
                int nCampi = 0;
                string sDatiStampa = "";

                nCampi = 28;
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
                sDatiStampa = IdEnte + " - " + DescrizioneEnte; 
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco l//intestazione del report
                sDatiStampa = "Minuta";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco le intestazioni di colonna
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA";
                sDatiStampa += "|Via Res.|Civico Res.|Cap Res.|Comune Res.|Provincia Res.";
                sDatiStampa += "|Nominativo C/O|Indirizzo C/O|Civico C/O|Cap C/O|Comune C/O|Pv C/O";
                sDatiStampa += "|Codice Cartella|Imponibile|Arrotondamento|Carico";
                sDatiStampa += "|Descrizione|Consistenza|Data Inizio|Durata|Data Fine|Tipologia Occupazionec|Categoria|Tariffa|Importo|";
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                if (dvDati != null)
                {
                    //ciclo sui dati da stampare
                    foreach (DataRowView myRow in dvDati)
                    {
                        sDatiStampa = GetMinutaRowDati(myRow);
                        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                            return null;
                    }
                }
                else
                {
                    Log.Debug(IdEnte + " - OPENgovOSAP.MetodiMinuta.Minuta::dataset vuoto causa errore non posso stampare");
                }
                return dtStampa;
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.MetodiMinuta.GetMinuta.errore: ", Err);
                throw (Err);
            }
        }
        //public static DataTable GetMinuta(int IdFlusso)
        //{
        //    try
        //    {
        //        DataTable dtStampa = new DataTable();
        //        DataSet dsStampa = new DataSet();
        //        DataView dvDati = MetodiMinuta.GetMinutaRecords(IdFlusso,false);
        //        int nCampi = 0;
        //        string sDatiStampa = "";

        //        nCampi = 28;
        //        dsStampa.Tables.Add("STAMPA");
        //        //carico le colonne nel dataset
        //        checked
        //        {
        //            for (int x = 0; x <= nCampi + 1; x++)
        //            {
        //                dsStampa.Tables["STAMPA"].Columns.Add("Col" + x.ToString().PadLeft(3, char.Parse("0")));
        //            }
        //        }
        //        //carico il datatable
        //        dtStampa = dsStampa.Tables["STAMPA"];
        //        //inserisco l//intestazione dell//ente
        //        sDatiStampa = DichiarazioneSession.IdEnte + " - " + DichiarazioneSession.DescrizioneEnte; ;
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco una riga vuota
        //        sDatiStampa = "";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco l//intestazione del report
        //        sDatiStampa = "Minuta";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco una riga vuota
        //        sDatiStampa = "";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco le intestazioni di colonna
        //        sDatiStampa = "";
        //        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA";
        //        sDatiStampa += "|Via Res.|Civico Res.|Cap Res.|Comune Res.|Provincia Res.";
        //        sDatiStampa += "|Nominativo C/O|Indirizzo C/O|Civico C/O|Cap C/O|Comune C/O|Pv C/O";
        //        sDatiStampa += "|Codice Cartella|Imponibile|Arrotondamento|Carico";
        //        sDatiStampa += "|Descrizione|Consistenza|Data Inizio|Durata|Data Fine|Tipologia Occupazionec|Categoria|Tariffa|Importo|";
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        if (dvDati != null)
        //        {
        //            //ciclo sui dati da stampare
        //            foreach (DataRowView myRow in dvDati)
        //            {
        //                sDatiStampa = "";
        //                if (myRow["COGNOME"] != null)
        //                    sDatiStampa += myRow["COGNOME"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["NOME"] != null)
        //                    sDatiStampa += "|" + myRow["NOME"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["cfpiva"] != null)
        //                    sDatiStampa += "|'" + myRow["cfpiva"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["VIA_RES"] != null)
        //                    sDatiStampa += "|" + myRow["VIA_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CIVICO_RES"] != null)
        //                    sDatiStampa += "|" + myRow["CIVICO_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CAP_RES"] != null)
        //                    sDatiStampa += "|" + myRow["CAP_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";

        //                if (myRow["COMUNE_RES"] != null)
        //                    sDatiStampa += "|" + myRow["COMUNE_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["PROVINCIA_RES"] != null)
        //                    sDatiStampa += "|" + myRow["PROVINCIA_RES"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["NOMINATIVOCO"] != null)
        //                    sDatiStampa += "|" + myRow["NOMINATIVOCO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["INDIRIZZOCO"] != null)
        //                    sDatiStampa += "|" + myRow["INDIRIZZOCO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CIVICOCO"] != null)
        //                    sDatiStampa += "|" + myRow["CIVICOCO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CAPCO"] != null)
        //                    sDatiStampa += "|" + myRow["CAPCO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["COMUNECO"] != null)
        //                    sDatiStampa += "|" + myRow["COMUNECO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["PVCO"] != null)
        //                    sDatiStampa += "|" + myRow["PVCO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CODICE_CARTELLA"] != null)
        //                    sDatiStampa += "|'" + myRow["CODICE_CARTELLA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["IMPORTO_PF"] != null)
        //                    sDatiStampa += "|" + myRow["IMPORTO_PF"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["IMPORTO_ARROTONDAMENTO"] != null)
        //                    sDatiStampa += "|" + myRow["IMPORTO_ARROTONDAMENTO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["IMPORTO_CARICO"] != null)
        //                    sDatiStampa += "|" + myRow["IMPORTO_CARICO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DESCRIZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["DESCRIZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CONSISTENZA"] != null)
        //                    sDatiStampa += "|" + myRow["CONSISTENZA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DATAINIZIOOCCUPAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["DATAINIZIOOCCUPAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DURATA"] != null)
        //                    sDatiStampa += "|" + myRow["DURATA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DATAFINEOCCUPAZIONE"] != null)
        //                    sDatiStampa += "|" + myRow["DATAFINEOCCUPAZIONE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["TIPOLOGIAOCCUPAZIONE_DESC"] != null)
        //                    sDatiStampa += "|" + myRow["TIPOLOGIAOCCUPAZIONE_DESC"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CATEGORIA_DESC"] != null)
        //                    sDatiStampa += "|" + myRow["CATEGORIA_DESC"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["TARIFFA_APPLICATA"] != null)
        //                    sDatiStampa += "|" + myRow["TARIFFA_APPLICATA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["IMPORTO"] != null)
        //                    sDatiStampa += "|" + myRow["IMPORTO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                sDatiStampa += "|";
        //                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //                    return null;
        //            }
        //        }
        //        else
        //        {
        //            Log.Debug("Minuta::dataset vuoto causa errore non posso stampare");
        //        }
        //        return dtStampa;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.GetMinuta.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <returns></returns>
        public static string GetMinutaRowDati(DataRowView myRow)
        {
            string sDatiStampa = "";
            try
            {
                if (myRow["COGNOME"] != null)
                    sDatiStampa += myRow["COGNOME"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["NOME"] != null)
                    sDatiStampa += "|" + myRow["NOME"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["cfpiva"] != null)
                    sDatiStampa += "|'" + myRow["cfpiva"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["VIA_RES"] != null)
                    sDatiStampa += "|" + myRow["VIA_RES"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CIVICO_RES"] != null)
                    sDatiStampa += "|" + myRow["CIVICO_RES"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CAP_RES"] != null)
                    sDatiStampa += "|" + myRow["CAP_RES"].ToString();
                else
                    sDatiStampa += "|";

                if (myRow["COMUNE_RES"] != null)
                    sDatiStampa += "|" + myRow["COMUNE_RES"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["PROVINCIA_RES"] != null)
                    sDatiStampa += "|" + myRow["PROVINCIA_RES"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["NOMINATIVOCO"] != null)
                    sDatiStampa += "|" + myRow["NOMINATIVOCO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["INDIRIZZOCO"] != null)
                    sDatiStampa += "|" + myRow["INDIRIZZOCO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CIVICOCO"] != null)
                    sDatiStampa += "|" + myRow["CIVICOCO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CAPCO"] != null)
                    sDatiStampa += "|" + myRow["CAPCO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["COMUNECO"] != null)
                    sDatiStampa += "|" + myRow["COMUNECO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["PVCO"] != null)
                    sDatiStampa += "|" + myRow["PVCO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CODICE_CARTELLA"] != null)
                    sDatiStampa += "|'" + myRow["CODICE_CARTELLA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["IMPORTO_PF"] != null)
                    sDatiStampa += "|" + myRow["IMPORTO_PF"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["IMPORTO_ARROTONDAMENTO"] != null)
                    sDatiStampa += "|" + myRow["IMPORTO_ARROTONDAMENTO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["IMPORTO_CARICO"] != null)
                    sDatiStampa += "|" + myRow["IMPORTO_CARICO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DESCRIZIONE"] != null)
                    sDatiStampa += "|" + myRow["DESCRIZIONE"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CONSISTENZA"] != null)
                    sDatiStampa += "|" + myRow["CONSISTENZA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DATAINIZIOOCCUPAZIONE"] != null)
                    sDatiStampa += "|" + myRow["DATAINIZIOOCCUPAZIONE"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DURATA"] != null)
                    sDatiStampa += "|" + myRow["DURATA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DATAFINEOCCUPAZIONE"] != null)
                    sDatiStampa += "|" + myRow["DATAFINEOCCUPAZIONE"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["TIPOLOGIAOCCUPAZIONE_DESC"] != null)
                    sDatiStampa += "|" + myRow["TIPOLOGIAOCCUPAZIONE_DESC"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CATEGORIA_DESC"] != null)
                    sDatiStampa += "|" + myRow["CATEGORIA_DESC"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["TARIFFA_APPLICATA"] != null)
                    sDatiStampa += "|" + myRow["TARIFFA_APPLICATA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["IMPORTO"] != null)
                    sDatiStampa += "|" + myRow["IMPORTO"].ToString();
                else
                    sDatiStampa += "|";
                sDatiStampa += "|";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return sDatiStampa;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myStringConnection"></param>
        /// <param name="IdEnte"></param>
        /// <param name="IdFlusso"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public static DataTable GetMinutaRate(string myStringConnection, string IdEnte,int IdFlusso)
        {
            try
            {
                DataTable dtStampa = new DataTable();
                DataSet dsStampa = new DataSet();
                DataView dvDati = MetodiMinuta.GetMinutaRecords(myStringConnection,IdEnte,"",IdFlusso, true);
                int nCampi = 0;
                string sDatiStampa = "";

                nCampi = 12;
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
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco l//intestazione del report
                sDatiStampa = "Minuta Rate";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                //inserisco le intestazioni di colonna
                sDatiStampa = "";
                sDatiStampa = "Codice Cartella";
                sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata";
                sDatiStampa += "|Codice Bollettino|Codeline|Barcode";
                sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2|";
                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                    return null;
                if (dvDati != null)
                {
                    //ciclo sui dati da stampare
                    foreach (DataRowView myRow in dvDati)
                    {
                        sDatiStampa = GetMinutaRateRowDati(myRow);
                        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
                            return null;
                    }
                }
                else
                {
                    Log.Debug("MinutaRate::dataset vuoto causa errore non posso stampare");
                }
                return dtStampa;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.GetMinutaRate.errore: ", Err);
                throw (Err);
            }
        }
        //public static DataTable GetMinutaRate(int IdFlusso)
        //{
        //    try
        //    {
        //        DataTable dtStampa = new DataTable();
        //        DataSet dsStampa = new DataSet();
        //        DataView dvDati = MetodiMinuta.GetMinutaRecords(IdFlusso,true);
        //        int nCampi = 0;
        //        string sDatiStampa = "";

        //        nCampi = 12;
        //        dsStampa.Tables.Add("STAMPA");
        //        //carico le colonne nel dataset
        //        checked
        //        {
        //            for (int x = 0; x <= nCampi + 1; x++)
        //            {
        //                dsStampa.Tables["STAMPA"].Columns.Add("Col" + x.ToString().PadLeft(3, char.Parse("0")));
        //            }
        //        }
        //        //carico il datatable
        //        dtStampa = dsStampa.Tables["STAMPA"];
        //        //inserisco l//intestazione dell//ente
        //        sDatiStampa = DichiarazioneSession.IdEnte + " - " + DichiarazioneSession.DescrizioneEnte; ;
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco una riga vuota
        //        sDatiStampa = "";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco l//intestazione del report
        //        sDatiStampa = "Minuta Rate";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco una riga vuota
        //        sDatiStampa = "";
        //        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        //inserisco le intestazioni di colonna
        //        sDatiStampa = "";
        //        sDatiStampa = "Codice Cartella";
        //        sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata";
        //        sDatiStampa += "|Codice Bollettino|Codeline|Barcode";
        //        sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2|";
        //        if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //            return null;
        //        if (dvDati != null)
        //        {
        //            //ciclo sui dati da stampare
        //            foreach (DataRowView myRow in dvDati)
        //            {
        //                sDatiStampa = "";
        //                if (myRow["CODICE_CARTELLA"] != null)
        //                    sDatiStampa += "'"+myRow["CODICE_CARTELLA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["NUMERO_RATA"] != null)
        //                    sDatiStampa += "|" + myRow["NUMERO_RATA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DESCRIZIONE_RATA"] != null)
        //                    sDatiStampa += "|" + myRow["DESCRIZIONE_RATA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DATA_SCADENZA"] != null)
        //                    sDatiStampa += "|" + DateTime.Parse(myRow["DATA_SCADENZA"].ToString()).ToShortDateString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["IMPORTO_RATA"] != null)
        //                    sDatiStampa += "|" + myRow["IMPORTO_RATA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CODICE_BOLLETTINO"] != null)
        //                    sDatiStampa += "|'" + myRow["CODICE_BOLLETTINO"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CODELINE"] != null)
        //                    sDatiStampa += "|" + myRow["CODELINE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["BARCODE"] != null)
        //                    sDatiStampa += "|'" + myRow["BARCODE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["CONTO_CORRENTE"] != null)
        //                    sDatiStampa += "|'" + myRow["CONTO_CORRENTE"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DESCRIZIONE_1_RIGA"] != null)
        //                    sDatiStampa += "|" + myRow["DESCRIZIONE_1_RIGA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                if (myRow["DESCRIZIONE_2_RIGA"] != null)
        //                    sDatiStampa += "|" + myRow["DESCRIZIONE_2_RIGA"].ToString();
        //                else
        //                    sDatiStampa += "|";
        //                sDatiStampa += "|";
        //                if (AddRowStampa(ref dtStampa, sDatiStampa) == 0)
        //                    return null;
        //            }
        //        }
        //        else
        //        {
        //            Log.Debug("MinutaRate::dataset vuoto causa errore non posso stampare");
        //        }
        //        return dtStampa;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.GetMinutaRate.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <returns></returns>
        public static string GetMinutaRateRowDati(DataRowView myRow)
        {
            string sDatiStampa = "";
            try
            {
                if (myRow["CODICE_CARTELLA"] != null)
                    sDatiStampa += "'" + myRow["CODICE_CARTELLA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["NUMERO_RATA"] != null)
                    sDatiStampa += "|" + myRow["NUMERO_RATA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DESCRIZIONE_RATA"] != null)
                    sDatiStampa += "|" + myRow["DESCRIZIONE_RATA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DATA_SCADENZA"] != null)
                    sDatiStampa += "|" + DateTime.Parse(myRow["DATA_SCADENZA"].ToString()).ToShortDateString();
                else
                    sDatiStampa += "|";
                if (myRow["IMPORTO_RATA"] != null)
                    sDatiStampa += "|" + myRow["IMPORTO_RATA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CODICE_BOLLETTINO"] != null)
                    sDatiStampa += "|'" + myRow["CODICE_BOLLETTINO"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CODELINE"] != null)
                    sDatiStampa += "|" + myRow["CODELINE"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["BARCODE"] != null)
                    sDatiStampa += "|'" + myRow["BARCODE"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["CONTO_CORRENTE"] != null)
                    sDatiStampa += "|'" + myRow["CONTO_CORRENTE"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DESCRIZIONE_1_RIGA"] != null)
                    sDatiStampa += "|" + myRow["DESCRIZIONE_1_RIGA"].ToString();
                else
                    sDatiStampa += "|";
                if (myRow["DESCRIZIONE_2_RIGA"] != null)
                    sDatiStampa += "|" + myRow["DESCRIZIONE_2_RIGA"].ToString();
                else
                    sDatiStampa += "|";
                sDatiStampa += "|";
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return sDatiStampa;
        }
        public static int[] GetColumnsToExport()
        {
            try {
                int[] Columns = new int[Enum.GetValues(typeof(E_COLUMNS)).Length];
                checked
                {
                    for (int i = 0; i < Columns.Length; i++)
                        Columns[i] = (int)Enum.GetValues(typeof(E_COLUMNS)).GetValue(i);
                }
                return Columns;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.GetColumnsToExport.errore: ", Err);
                throw (Err);
            }
        }
        public static int AddRowStampa(ref DataTable DtAddRow, string sValueRow)
        {
            string[] sTextRow;
            DataRow DrAddRow;
            int x = 0;

            try
            {
                //aggiungo una nuova riga nel datarow
                DrAddRow = DtAddRow.NewRow();
                //controllo se la riga e\' scritta
                if (sValueRow != "")
                {
                    sTextRow = sValueRow.Split(char.Parse("|"));
                    for (x = 0; x < sTextRow.GetUpperBound(0); x++)
                    {
                        //popolo la riga nel datarow
                        DrAddRow[x] = sTextRow[x];
                    }
                }
                //aggiorno la riga al datatable
                DtAddRow.Rows.Add(DrAddRow);
                return 1;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.AddRowStampa.errore: ", Err);
                return 0;
            }
        }
        #endregion

        #region "Private Method"
        //*** 20130610 - ruolo supplettivo ***
        /*private static Minuta[] GetMinutaRecords (int IdFlusso)
        //private static Minuta[] GetMinutaRecords (int Anno)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try 
            {
                DataTable dt = null;
                dt = DAO.GetMinuta (IdFlusso);
                Minuta[] minuta = FillMinutaFromDataTable(dt);

                Hashtable htContribuenti = new Hashtable ();
                Hashtable htStrade = new Hashtable ();

                foreach (Minuta m in minuta)
                {
                    string key = m.CodContribuente.ToString();
                    if (!htContribuenti.ContainsKey (key))
                    {
                        DAO.AnagraficheDAO objAnagrafica = new DAO.AnagraficheDAO();
                        m.AnagraficaContribuente = objAnagrafica.GetAnagraficaContribuente(m.CodContribuente);//, m.IdTributo
                        htContribuenti.Add (key, m.AnagraficaContribuente);
                    }
                    else
                        m.AnagraficaContribuente = (DettaglioAnagrafica)htContribuenti [key];

                    if (!htStrade.ContainsKey (m.CodVia))
                    {
                        m.SVia = MetodiArticolo.GetDescrizioneVia (m.CodVia, DichiarazioneSession.IdEnte);
                        htStrade.Add (m.CodVia, m.SVia);
                    }
                    else
                        m.SVia = (string)htStrade [m.CodVia];
                }

                return minuta;
            } 
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.GetMinutaRecords.errore: ", Err);
                throw (Err);
            }
        }*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myStringconnection"></param>
        /// <param name="IdEnte"></param>
        /// <param name="IdTributo"></param>
        /// <param name="IdFlusso"></param>
        /// <param name="IsRate"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public static DataView GetMinutaRecords(string myStringconnection,string IdEnte,string IdTributo,int IdFlusso, bool IsRate)
        {
            DAO.CartelleDAO DAO = new DAO.CartelleDAO();

            try
            {
                DataTable dt = null;
                dt = DAO.GetMinuta(myStringconnection, IdEnte,IdTributo,IdFlusso, IsRate);
                return dt.DefaultView;
            }
            catch (Exception Err)
            {
                Log.Debug(IdEnte + " - OPENgovOSAP.MetodiMinuta.GetMinutaRecords.errore: ", Err);
                throw (Err);
            }
        }
        //private static DataView GetMinutaRecords(int IdFlusso,bool IsRate)
        //{
        //    DAO.CartelleDAO DAO = new DAO.CartelleDAO();

        //    try
        //    {
        //        DataTable dt = null;
        //        dt = DAO.GetMinuta(IdFlusso,IsRate);
        //        return dt.DefaultView;
        //    }
        //    catch (Exception Err)
        //    {
        //        Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.GetMinutaRecords.errore: ", Err);
        //        throw (Err);
        //    }
        //}
        //*** ***

        private static Minuta[] FillMinutaFromDataTable(DataTable dt)
        {
            try {
                ArrayList MyArray = new ArrayList();
                Minuta CurrentItem = null;

                foreach (DataRow myRow in dt.Rows)
                {
                    CurrentItem = new Minuta();
                    CurrentItem.IdTributo = (string)myRow["IDTRIBUTO"];
                    CurrentItem.CodContribuente = (int)myRow["COD_CONTRIBUENTE"];
                    CurrentItem.NDichiarazione = (string)myRow["NDICHIARAZIONE"];
                    CurrentItem.CodVia = (int)myRow["CODVIA"];
                    CurrentItem.Civico = (string)myRow["CIVICO"];
                    CurrentItem.Esponente = (string)myRow["ESPONENTE"];
                    CurrentItem.Interno = (string)myRow["INTERNO"];
                    CurrentItem.Consistenza = (string)myRow["CONSISTENZA"];
                    CurrentItem.DataInizioOccupazione = (string)myRow["DATAINIZIOOCCUPAZIONE"];
                    CurrentItem.Durata = (string)myRow["DURATA"];
                    CurrentItem.DataFineOccupazione = (string)myRow["DATAFINEOCCUPAZIONE"];
                    CurrentItem.MaggiorazioneImporto = double.Parse(myRow["MAGGIORAZIONE_IMPORTO"].ToString());
                    CurrentItem.MaggiorazionePerc = double.Parse(myRow["MAGGIORAZIONE_PERC"].ToString());
                    CurrentItem.DetrazioneImporto = double.Parse(myRow["DETRAZIONE_IMPORTO"].ToString());
                    CurrentItem.Attrazione = (string)myRow["ATTRAZIONE"];
                    CurrentItem.TipologiaOccupazione = (string)myRow["TIPOLOGIAOCCUPAZIONE_DESC"];
                    CurrentItem.Categoria = (string)myRow["CATEGORIA_DESC"];
                    //*** 20130416 - deve essere l'elenco di tutte le % di agevolazione applicate
                    //CurrentItem.AgevolazionePerc = double.Parse (myRow["AGEVOLAZIONE_PERC"].ToString());
                    CurrentItem.AgevolazionePerc = (string)myRow["AGEVOLAZIONE_PERC"];
                    //*** ***
                    CurrentItem.TariffaApplicata = double.Parse(myRow["TARIFFA_APPLICATA"].ToString());
                    CurrentItem.ImportoLordo = double.Parse(myRow["IMPORTO_LORDO"].ToString());
                    CurrentItem.Importo = double.Parse(myRow["IMPORTO"].ToString());
                    CurrentItem.IdDichiarazione = (int)myRow["IDDICHIARAZIONE"];

                    MyArray.Add(CurrentItem);
                }

                return (Minuta[])MyArray.ToArray(typeof(Minuta));
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.FillMinutaFromDataTable.errore: ", Err);
                throw (Err);
            }
        }

        private static DataTable CreateMinutaDataTable()
        {
            try {
                DataTable dtMinuta = new DataTable();

                // Dati contribuente
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.COGNOME));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.NOME));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.CFPIVA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.INDIRIZZORESIDENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.CIVICORESIDENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.ESPONENTERESIDENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.INTERNORESIDENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.CAPRESIDENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.COMUNERESIDENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.PROVINCIARESIDENZA));

                // Dati articolo
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.NDICHIARAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.UBICAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.CIVICO));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.ESPONENTE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.INTERNO));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.TIPOOCCUPAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.CATEGORIA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.ATTRAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.CONSISTENZA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.DATAINIZIOOCCUPAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.DURATAOCCUPAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.DATAFINEOCCUPAZIONE));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.TARIFFAAPPLICATA));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.AGEVOLAZIONEPERC));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.MAGGIORAZIONEPERC));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.MAGGIORAZIONEIMPORTO));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.DETRAZIONI));
                dtMinuta.Columns.Add(SharedFunction.EnumStringValueOf(E_COLUMNS.IMPORTOTOTALE));

                return dtMinuta;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.MetodiMinuta.CreateMinuteDataTable.errore: ", Err);
                throw (Err);
            }
        }

        #endregion

    }
}
