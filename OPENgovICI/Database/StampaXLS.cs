using log4net;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Web;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione delle stampe in formato CSV/XLS
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class StampaXLS
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StampaXLS));

        private int AddRowStampa(DataTable DtAddRow, string sValueRow)
        {
            string[] sTextRow;
            DataRow DrAddRow;

            try
            {
                //aggiungo una nuova riga nel datarow
                DrAddRow = DtAddRow.NewRow();
                //controllo se la riga e\' scritta
                if (sValueRow != "")
                {
                    sTextRow = sValueRow.Split(char.Parse("|"));
                    checked
                    {
                        for (int x = 0; x < sTextRow.GetUpperBound(0); x++)
                        {
                            //popolo la riga nel datarow
                            DrAddRow[x] = sTextRow[x];
                        }
                    }
                }
                //aggiorno la riga al datatable
                DtAddRow.Rows.Add(DrAddRow);

                return 1;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaXLS.AddRowStampa.errore: ", Err);
                return 0;
            }
        }
        public DataTable PrintRimborsi(DataView DvDati, string sIntestazioneEnte, int nCampi)
        {
            string sDatiStampa = "";
            DataSet DsStampa = new DataSet();
            DataTable DtStampa = new DataTable();
            int nTotArticoli = 0;

            try
            {
                DsStampa.Tables.Add("STAMPA");
                checked
                {
                    for (int x = 0; x < nCampi + 1; x++)
                    {
                        DsStampa.Tables["STAMPA"].Columns.Add("Col" + x.ToString().PadLeft(3, char.Parse("0")));
                    }
                }
                DtStampa = DsStampa.Tables["STAMPA"];
                //inserisco l'intestazione dell'ente
                sDatiStampa = sIntestazioneEnte;
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(DtStampa, sDatiStampa) == 0)
                {
                    return null;
                }
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(DtStampa, sDatiStampa) == 0)
                {
                    return null;
                }
                //inserisco l'intestazione del report
                sDatiStampa = "Rimborsi Anno " + DvDati[0]["anno"].ToString();
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(DtStampa, sDatiStampa) == 0)
                {
                    return null;
                }
                //inserisco una riga vuota
                sDatiStampa = "";
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, char.Parse("|"));
                if (AddRowStampa(DtStampa, sDatiStampa) == 0)
                {
                    return null;
                }
                //inserisco le intestazioni di colonna
                sDatiStampa = "";
                sDatiStampa = "Nominativo|Cod.Fiscale/P.Iva";
                sDatiStampa += "|Altri Fabbricati|Aree Fabbricabili|Terreni|Fab.Rur.Uso Strum|Uso Prod. Cat.D";
                sDatiStampa += "|Dovuto|Pagato|Rimborso";
                if (AddRowStampa(DtStampa, sDatiStampa) == 0)
                {
                    return null;
                }
                //ciclo sui dati da stampare
                foreach (DataRowView myRow in DvDati)
                {
                    sDatiStampa = "";
                    nTotArticoli += 1;
                    sDatiStampa += (myRow["Nominativo"] != DBNull.Value ? myRow["Nominativo"].ToString()+"|" : "|");
                    sDatiStampa += (myRow["cfpiva"] != DBNull.Value ? "'"+myRow["cfpiva"].ToString() + "|" : "|");
                    sDatiStampa += (myRow["altrifab"] != DBNull.Value ? myRow["altrifab"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["areefab"] != DBNull.Value ? myRow["areefab"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["terreni"] != DBNull.Value ? myRow["terreni"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["fabrurusostrum"] != DBNull.Value ? myRow["fabrurusostrum"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["UsoProdCatD"] != DBNull.Value ? myRow["UsoProdCatD"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["dovuto"] != DBNull.Value ? myRow["dovuto"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["pagato"] != DBNull.Value ? myRow["pagato"].ToString().Replace(".", "") + "|" : "|");
                    sDatiStampa += (myRow["rimborso"] != DBNull.Value ? myRow["rimborso"].ToString().Replace(".", "") + "|" : "|");
                    if (AddRowStampa(DtStampa, sDatiStampa) == 0)
                    {
                        return null;
                    }
                }

                return DtStampa;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.StampaXLS.PrintRimborsi.errore: ", Err);
                return null;
            }
        }
    }
}