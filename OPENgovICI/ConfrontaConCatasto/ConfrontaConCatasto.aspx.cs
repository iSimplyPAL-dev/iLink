using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient; 
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration; 
using System.Globalization;
using log4net;
using DichiarazioniICI.Database;
using Business;

namespace DichiarazioniICI.ConfrontaConCatasto
{
    /// <summary>
    /// Pagina per il confronto fra il catasto e la banca dati tributaria di CMGC.
    /// Contiene i parametri di ricerca e le funzioni della comandiera.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ConfrontaConCatasto : BasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfrontaConCatasto));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    ListItem myListItem = new ListItem();
                    log.Debug("ConfrontaConCatasto::devo caricare gli anni");
                    for (int x = DateTime.Today.Year; x > 2005; x--)
                    {
                        myListItem = new ListItem();
                        myListItem.Text = x.ToString();
                        myListItem.Value = x.ToString();
                        ddlAnnoRiferimento.Items.Add(myListItem);
                    }
                    System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();

                    strBuilder.Append("");
                    strBuilder.Append("parent.parent.nascosto.location.href='./ElaborazioniCatasto.aspx';");
                    strBuilder.Append("parent.document.getElementById('frameVisualizza').rows = '45,*,0,50';");
                    strBuilder.Append("");
                    RegisterScript(strBuilder.ToString(), this.GetType());
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.Page_Load.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
            }
        }

        #region "Proprietari diversi Catasto/ICI"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        protected void btnPassaggioProprieta_Click(object sender, System.EventArgs e)
        {
            DataTable dtDifferenze = null;
            string[] arraystr;
            string NameXLS = string.Empty;

            try
            {
                int nCol = 8;
                DataRow dr;
                DataSet ds = new DataSet();
                ArrayList arratlistNomiColonne = new ArrayList();

                //prelevo i dati da db
                ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();
                log.Debug("eseguo CatVSICI_DifProp");
                DataTable dtDiff = MyFnc.CatVSICI_DifProp(ConstWrapper.CodiceEnte, ConstWrapper.CodBelfiore, ddlAnnoRiferimento.SelectedItem.Value, ConstWrapper.StringConnection);
                log.Debug("fatto");
                checked
                {
                    for (int x = 0; x <= nCol; x++)
                        arratlistNomiColonne.Add("");
                }
                arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

                NameXLS = ConstWrapper.CodiceEnte + "_CONFRONTO_PROPRIETARI_CONCATASTO_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                ds.Tables.Add("CONFRONTO_PROPRIETARI_CONCATASTO");
                checked
                {
                    for (int x = 0; x <= nCol; x++)
                        ds.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
                }
                dtDifferenze = ds.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"];
                //inserisco intestazione - titolo prospetto + data stampa
                dr = dtDifferenze.NewRow();
                dr[0] = ConstWrapper.CodiceEnte + " - " + ConstWrapper.DescrizioneEnte;
                dtDifferenze.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //*** 20120704 - IMU ***
                dr = dtDifferenze.NewRow();
                dr[0] = "Prospetto Confronto ICI/IMU - Catasto per Proprietari";
                dr[8] = "Data Stampa " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
                dtDifferenze.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //inserisco intestazione di colonna
                dr = dtDifferenze.NewRow();
                dr[0] = "Foglio";
                dr[1] = "Numero";
                dr[2] = "Subalterno";
                dr[3] = "Generazione Data Validita'";
                dr[4] = "Conclusione Data Validita'";
                dr[5] = "Generazione Data Registrazione";
                dr[6] = "Conclusione Data Registrazione";
                //*** 20120704 - IMU ***
                dr[7] = "Proprietari Vert. ICI/IMU";
                dr[8] = "Proprietari Cat.";
                dtDifferenze.Rows.Add(dr);
                foreach (DataRow rStampa in dtDiff.Rows)
                {
                    dr = dtDifferenze.NewRow();
                    dr[0] = rStampa["FOGLIO"].ToString();
                    dr[1] = rStampa["NUMERO"].ToString();
                    if (rStampa["SUBALTERNO"].ToString() != "-1")
                        dr[2] = rStampa["SUBALTERNO"].ToString();
                    else
                        dr[2] = "";
                    dr[3] = " " + rStampa["dataEfficacia"].ToString();
                    dr[4] = " " + rStampa["dataEfficacia1"].ToString();
                    dr[5] = " " + rStampa["dataRegAtti"].ToString();
                    dr[6] = " " + rStampa["dataRegAtti1"].ToString();
                    if (rStampa["PROPRIETARIICI"] != null)
                        dr[7] = rStampa["PROPRIETARIICI"].ToString();
                    else
                        dr[7] = "";
                    if (rStampa["PROPRIETARICAT"] != null)
                        dr[8] = rStampa["PROPRIETARICAT"].ToString();
                    else
                        dr[8] = "";
                    dtDifferenze.Rows.Add(dr);
                }
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //inserisco numero totali di contribuenti
                dr = dtDifferenze.NewRow();
                dr[0] = "Totale Record nel File: " + (dtDiff.Rows.Count);
                dtDifferenze.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnPassaggioProprieta_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                //*** 20120704 - IMU ***
                string strscript = " GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni di Catasto.');";
                RegisterScript(strscript, this.GetType());
                return;
            }
            if (dtDifferenze != null)
            {
                //definisco l'insieme delle colonne da esportare
                int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                //esporto i dati in excel
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                objExport.ExportDetails(dtDifferenze, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }
        }
        //protected void btnPassaggioProprieta_Click(object sender, System.EventArgs e)
        //{
        //    log.Debug("ConfrontaConCatasto::btnPassaggioProprieta_Click::INIZIO");
        //    DataTable dtConfronto = new DataTable();
        //    DataTable dtAnagrafica = new DataTable();
        //    string strscript = string.Empty;
        //    string annoConfronto = ddlAnnoRiferimento.SelectedItem.Value;

        //    SqlConnection oConn = new SqlConnection();
        //    SqlConnection oConnCatasto = new SqlConnection();
        //    SqlCommand oCMDCatasto = new SqlCommand();

        //    string NomeDbAnag = ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();

        //    string sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();
        //    string sqlAccess2000 = ConfigurationManager.AppSettings["sqlAccessCatasto"].ToString();

        //    string connectionServer2000 = string.Empty;
        //    string connectionAccess2000 = string.Empty;

        //    string ID_IMMOBILE = string.Empty;
        //    string CLASSE = String.Empty;
        //    string CATEGORIA = String.Empty;
        //    string CONSISTENZA = String.Empty;

        //    string CLASSEACCESS = String.Empty;
        //    string CATEGORIAACCESS = String.Empty;
        //    string RENDITAACCESS = string.Empty;

        //    string DATAEFFICACIAACCESS = String.Empty;
        //    string DATAEFFICACIA1ACCESS = String.Empty;
        //    string DATA_REG_ATTIACCESS = String.Empty;
        //    string DATA_REG_ATTI1ACCESS = String.Empty;
        //    string VALOREACCESS = string.Empty;

        //    string FOGLIO = String.Empty;
        //    string NUMERO = String.Empty;
        //    string SUBALTERNO = String.Empty;
        //    string FOGLIOC = String.Empty;
        //    string NUMEROC = String.Empty;
        //    string SUBALTERNOC = String.Empty;

        //    string BELFIORE = String.Empty;

        //    string ConfrontoACCESS = string.Empty;
        //    string ConfrontoSQL = string.Empty;

        //    string Giorno = string.Empty;
        //    string Mese = string.Empty;
        //    string Anno = string.Empty;
        //    string result = string.Empty;

        //    string nomeC = string.Empty;
        //    string cognomeC = string.Empty;
        //    string cod_fiscaleC = string.Empty;
        //    string data_nascitaC = string.Empty;
        //    string comuneC = string.Empty;
        //    string provinciaC = string.Empty;
        //    string codContribuenteIci = string.Empty;
        //    string nominativoIci = string.Empty;
        //    string nomeIci = string.Empty;
        //    string cognomeIci = string.Empty;
        //    string dataNascitaIci = string.Empty;
        //    string comuneIci = string.Empty;
        //    string provinciaIci = string.Empty;
        //    string codicefiscaleIci = string.Empty;

        //    string nomeTabellaAnagrafica = string.Empty;
        //    string sqlEliminaTabella = string.Empty;

        //    string sqlCreaTabella = string.Empty;

        //    try
        //    {
        //        log.Debug("Anno selezionato " + annoConfronto);
        //        BELFIORE = Session["COD_BELFIORE"].ToString();
        //        log.Debug("Sessione belfiore");

        //        //APERTURA CONNESSIONE A OPENGOVICI
        //        //LEGGO LA STRINGA di connessione da web config 
        //        //apro la connessione
        //        oConn.ConnectionString = sqlServer2000;
        //        oConn.Open();

        //        oConnCatasto.ConnectionString = sqlAccess2000;
        //        oConnCatasto.Open();

        //        log.Debug("Apro connessione access per anagrafiche");

        //        /********** CREAZIONE E POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/
        //        if (CreaTabSoggetti(oConn, oConnCatasto, BELFIORE) == false)
        //        {
        //            throw new Exception("Errore in popolamento tabella soggetti.");
        //        }
        //        /********** FINE POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/

        //        log.Error("fine popolamento tabella anagrafica");

        //        connectionServer2000 = "";

        //        DataSet ds = new DataSet();

        //        DataTable dtIci = new ImmobiliConfrontoCatastoIci().ListImmobiliContribuenti(annoConfronto, ConstWrapper.CodiceEnte);

        //        ds.Tables.Add(dtIci);
        //        //*** 20120704 - IMU ***
        //        log.Error("dopo query in viewContribImmobiliCatasto per immobili ICI/IMU");

        //        connectionAccess2000 = "SELECT FAB.FOGLIO, FAB.NUMERO, ISNUMERIC(FAB.NUMERO) AS MAPPALE_ALFANUMERICO, FAB.SUBALTERNO, UI.CATEGORIA, UI.CLASSE, FAB.COD_AMM_COMUNE, UI.RENDITA_EURO, UI.DATA_EFFICACIA, UI.DATA_REG_ATTI, UI.DATA_EFFICACIA1, UI.DATA_REG_ATTI1, FAB.ID_IMMOBILE";
        //        connectionAccess2000 += " , FAB.SEZIONE, ID_SOGGETTO";
        //        connectionAccess2000 += " FROM FAB_IDENTIFICATIVI FAB";
        //        connectionAccess2000 += " INNER JOIN FAB_UNITA_IMMOBILIARE UI ON FAB.COD_AMM_COMUNE = UI.COD_AMM_COMUNE AND FAB.SEZIONE = UI.SEZIONE AND FAB.ID_IMMOBILE = UI.ID_IMMOBILE AND FAB.TIPO_IMMOBILE = UI.TIPO_IMMOBILE";
        //        connectionAccess2000 += " INNER JOIN FAB_TITOLARITA TIT ON UI.COD_AMM_COMUNE = TIT.COD_AMM_COMUNE AND UI.ID_IMMOBILE = TIT.ID_IMMOBILE";
        //        connectionAccess2000 += " WHERE 1=1";
        //        connectionAccess2000 += " AND ISNUMERIC(FAB.NUMERO) <> 0";
        //        //connectionAccess2000+=" AND CATEGORIA NOT LIKE 'E%' AND CATEGORIA NOT LIKE 'F%' AND CATEGORIA NOT LIKE 'D10%'";
        //        connectionAccess2000 += " AND (CATEGORIA NOT LIKE 'E%' AND CATEGORIA NOT LIKE 'F01%' AND CATEGORIA NOT LIKE 'F05%')";
        //        connectionAccess2000 += " AND (UI.DATA_EFFICACIA1 Is Null Or UI.DATA_EFFICACIA1='')";
        //        connectionAccess2000 += " AND (UI.DATA_REG_ATTI1 Is Null Or UI.DATA_REG_ATTI1='')";
        //        connectionAccess2000 += " AND (FAB.COD_AMM_COMUNE='" + BELFIORE + "')";

        //        log.Error("dopo query access in GRANDCOMBINCATASTO per immobili e anagrafiche catasto");

        //        SqlCommand Cmd = new SqlCommand();
        //        SqlDataAdapter Adptr = new SqlDataAdapter();

        //        Cmd.CommandText = connectionAccess2000;
        //        Cmd.CommandType = CommandType.Text;
        //        Cmd.Connection = oConnCatasto;
        //        DataTable dtCatasto = new DataTable("Catasto");
        //        Adptr.SelectCommand = Cmd;
        //        Adptr.Fill(dtCatasto);
        //        ds.Tables.Add(dtCatasto);

        //        ds.Relations.Add(new System.Data.DataRelation("Foglio", ds.Tables[0].Columns["FoglioFormattato"], ds.Tables[1].Columns["FOGLIO"], false));
        //        ds.Relations.Add(new System.Data.DataRelation("Numero", ds.Tables[0].Columns["NumeroFormattato"], ds.Tables[1].Columns["NUMERO"], false));
        //        ds.Relations.Add(new System.Data.DataRelation("Subalterno", ds.Tables[0].Columns["SubalternoFormattato"], ds.Tables[1].Columns["SUBALTERNO"], false));


        //        connectionAccess2000 = "SELECT TOTALE_SOGGETTI_" + BELFIORE + ".*, COMUNE.IDENTIFICATIVO, COMUNE.PV";
        //        connectionAccess2000 += " FROM TOTALE_SOGGETTI_" + BELFIORE;
        //        connectionAccess2000 += " LEFT JOIN " + ConfigurationManager.AppSettings["NOME_DATABASE_OPENGOV"].ToString() + ".DBO.COMUNI COMUNE ON TOTALE_SOGGETTI_" + BELFIORE + ".LUOGO_NASCITA = COMUNE.IDENTIFICATIVO COLLATE LATIN1_GENERAL_CI_AS";
        //        connectionAccess2000 += " WHERE 1=1";
        //        connectionAccess2000 += " AND (COD_AMM='" + BELFIORE + "')";
        //        Cmd.CommandText = connectionAccess2000;
        //        Cmd.CommandType = CommandType.Text;
        //        Cmd.Connection = oConn;
        //        DataTable dtAnag = new DataTable("Anagrafe");
        //        Adptr.SelectCommand = Cmd;
        //        Adptr.Fill(dtAnag);
        //        ds.Tables.Add(dtAnag);

        //        ds.Relations.Add(new System.Data.DataRelation("COD_AMM_COMUNE", ds.Tables[1].Columns["COD_AMM_COMUNE"], ds.Tables[2].Columns["COD_AMM"], false));
        //        ds.Relations.Add(new System.Data.DataRelation("SEZIONE", ds.Tables[1].Columns["SEZIONE"], ds.Tables[2].Columns["SEZIONE"], false));
        //        ds.Relations.Add(new System.Data.DataRelation("ID_SOGGETTO", ds.Tables[1].Columns["ID_SOGGETTO"], ds.Tables[2].Columns["ID_SOGGETTO"], false));

        //        string fg = "", num = "", sub = "", subalternoPerCatasto;
        //        string foglioO, NumO, subO;
        //        string elencoCodiciFiscaliIci = "", elencoCodiciFiscaliIci2 = "";
        //        string elencoNominativiIci = "";
        //        string dataEfficacia = "";
        //        string dataEfficacia1 = "";
        //        string dataRegAtti = "";
        //        string dataRegAtti1 = "";
        //        string idImmobile = "";

        //        string codFisc = "";

        //        //Dati immobili
        //        DataColumn colonna;
        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "foglio";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "numero";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "subalterno";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "dataEfficacia";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "dataEfficacia1";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "dataRegAtti";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "dataRegAtti1";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        colonna = new DataColumn();
        //        colonna.DataType = System.Type.GetType("System.String");
        //        colonna.ColumnName = "idImmobile";
        //        colonna.ReadOnly = false;
        //        dtConfronto.Columns.Add(colonna);

        //        //Dati Proprietari
        //        DataColumn colonnaA;
        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "idImmobile";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "cognomeIci";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "nomeIci";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "codFiscaleIci";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "cognomeCatasto";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "nomeCatasto";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        colonnaA = new DataColumn();
        //        colonnaA.DataType = System.Type.GetType("System.String");
        //        colonnaA.ColumnName = "codFiscaleCatasto";
        //        colonnaA.ReadOnly = false;
        //        dtAnagrafica.Columns.Add(colonnaA);

        //        DataRow row;
        //        DataRow rowAnagr;

        //        DataColumn colonnaIci;
        //        colonnaIci = new DataColumn();
        //        colonnaIci.ColumnName = "controllato";
        //        colonnaIci.DataType = System.Type.GetType("System.Int16");
        //        colonnaIci.ReadOnly = false;
        //        colonnaIci.DefaultValue = 0;
        //        ds.Tables[0].Columns.Add(colonnaIci);

        //        DataColumn colonnaCat;
        //        colonnaCat = new DataColumn();
        //        colonnaCat.ColumnName = "AnagrControllate";
        //        colonnaCat.DataType = System.Type.GetType("System.Int16");
        //        colonnaCat.ReadOnly = false;
        //        colonnaCat.DefaultValue = 0;
        //        ds.Tables[1].Columns.Add(colonnaCat);

        //        log.Error("Inizio ciclo dati - passaggio proprietà");

        //        foreach (DataRow riga in ds.Tables[0].Rows)
        //        {
        //            DataRow[] rigaTrovataIci;

        //            if ((riga["FoglioFormattato"].ToString().CompareTo(fg) != 0) || (riga["NumeroFormattato"].ToString().CompareTo(num) != 0) || (riga["SubalternoFormattato"].ToString().CompareTo(sub) != 0))
        //            {
        //                fg = riga["FoglioFormattato"].ToString().Trim();
        //                num = riga["NumeroFormattato"].ToString().Trim();
        //                sub = riga["SubalternoFormattato"].ToString().Trim();

        //                foglioO = riga["Foglio"].ToString().Trim();
        //                NumO = riga["Numero"].ToString().Trim();
        //                subO = riga["subalternoStringa"].ToString().Trim();

        //                string expressionIci = "Foglio like '" + foglioO + "' and Numero like '" + NumO + "' and subalternoStringa like '" + subO + "'";
        //                string expressioncatasto = "Foglio like '" + fg + "' and Numero like '" + num + "' and subalterno like '" + sub + "'";

        //                rigaTrovataIci = ds.Tables[0].Select(expressionIci);
        //                elencoCodiciFiscaliIci = "";
        //                codFisc = "";
        //                elencoNominativiIci = "";
        //                elencoCodiciFiscaliIci2 = "";
        //                nominativoIci = "";
        //                foreach (DataRow rowIci in ds.Tables[0].Select(expressionIci))
        //                {
        //                    if (rowIci[2].ToString().CompareTo("") != 0)
        //                        codFisc = rowIci[2].ToString().ToUpper();
        //                    else
        //                    {
        //                        if (rowIci[3].ToString().CompareTo("") != 0)
        //                            codFisc = rowIci[3].ToString().ToUpper();
        //                        else
        //                            codFisc = "";
        //                    }

        //                    nominativoIci = rowIci[0].ToString() + " " + rowIci[1].ToString();
        //                    elencoNominativiIci = elencoNominativiIci + nominativoIci + ",";

        //                    elencoCodiciFiscaliIci = elencoCodiciFiscaliIci + codFisc + ",";
        //                    elencoCodiciFiscaliIci2 = elencoCodiciFiscaliIci2 + codFisc + ",";

        //                    rowIci[21] = 1;
        //                }

        //                foreach (DataRow rowIci in ds.Tables[1].Select(expressioncatasto))
        //                {
        //                    idImmobile = rowIci[12].ToString();
        //                    dataEfficacia = rowIci[8].ToString();
        //                    dataEfficacia1 = rowIci[10].ToString();
        //                    dataRegAtti = rowIci[9].ToString();
        //                    dataRegAtti1 = rowIci[11].ToString();
        //                }
        //                if (elencoNominativiIci.Length > 0)
        //                    elencoNominativiIci = elencoNominativiIci.Substring(0, (elencoNominativiIci.Length) - 1);
        //                if (elencoCodiciFiscaliIci.Length > 0)
        //                    elencoCodiciFiscaliIci = elencoCodiciFiscaliIci.Substring(0, (elencoCodiciFiscaliIci.Length) - 1);
        //                elencoCodiciFiscaliIci = elencoCodiciFiscaliIci.Replace(",", "','");

        //                if (sub == "00-1")
        //                    subalternoPerCatasto = "";
        //                else
        //                    subalternoPerCatasto = sub;

        //                string expression = "cod_fiscale in('" + elencoCodiciFiscaliIci + "')";//= "foglio like '" + fg + "' and numero like '"+ num + "' and subalterno like '" + subalternoPerCatasto + "'";
        //                string giorno, mese, anno;
        //                string nomi = "";
        //                string nominativiCatasto = "";
        //                string foglio1 = "", num1 = "", sub1 = "";
        //                string codiceFiscaleCat = "", elencoCodFiscaliCat = "";
        //                string codFiscaleNonTrovato = "";

        //                foglio1 = foglioO;
        //                num1 = NumO;
        //                sub1 = subO;

        //                DataRow[] nrighe = ds.Tables[2].Select(expression);
        //                if (nrighe.Length > 0)
        //                {
        //                    elencoCodFiscaliCat = "";
        //                    codiceFiscaleCat = "";
        //                    for (int k = 0; k < nrighe.Length; k++)
        //                    {
        //                        nomi = nrighe[k].ItemArray[4].ToString() + " " + nrighe[k].ItemArray[5].ToString();
        //                        nominativiCatasto = nominativiCatasto + nomi + ",";
        //                        codiceFiscaleCat = nrighe[k].ItemArray[6].ToString();
        //                        elencoCodFiscaliCat = elencoCodFiscaliCat + codiceFiscaleCat + ",";
        //                    }

        //                    string trova = ",";

        //                    string codiceFIci;
        //                    int count = 0;
        //                    int pos = elencoCodiciFiscaliIci.IndexOf(trova);
        //                    while (pos != -1)
        //                    {
        //                        count++;
        //                        elencoCodiciFiscaliIci = elencoCodiciFiscaliIci.Substring(pos + 1);
        //                        pos = elencoCodiciFiscaliIci.IndexOf(trova);
        //                    }

        //                    string[] elencoProprietariIci = elencoCodiciFiscaliIci2.Split(',');

        //                    for (int h = 0; h < count; h++)
        //                    {
        //                        codiceFIci = elencoProprietariIci[h].ToString();
        //                        int codiceTrovato = 0;
        //                        codiceTrovato = elencoCodFiscaliCat.IndexOf(codiceFIci);

        //                        if (codiceTrovato == -1)
        //                            codFiscaleNonTrovato = codFiscaleNonTrovato + codiceFIci;
        //                        //else
        //                        //    codFiscaleNonTrovato = codFiscaleNonTrovato;
        //                    }
        //                }

        //                if (codFiscaleNonTrovato != "")
        //                {
        //                    // generazione data efficacia
        //                    if (dataEfficacia.Length > 0)
        //                    {
        //                        giorno = "";
        //                        mese = "";
        //                        anno = "";

        //                        giorno = dataEfficacia.Substring(0, 2);
        //                        mese = dataEfficacia.Substring(2, 2);
        //                        anno = dataEfficacia.Substring(4, 4);
        //                        dataEfficacia = " " + giorno + "/" + mese + "/" + anno;
        //                    }
        //                    else
        //                    {
        //                        dataEfficacia = "";
        //                    }

        //                    //conclusione data efficacia
        //                    if (dataEfficacia1.Length > 0)
        //                    {
        //                        giorno = "";
        //                        mese = "";
        //                        anno = "";

        //                        giorno = dataEfficacia1.Substring(0, 2);
        //                        mese = dataEfficacia1.Substring(2, 2);
        //                        anno = dataEfficacia1.Substring(4, 4);
        //                        dataEfficacia1 = " " + giorno + "/" + mese + "/" + anno;
        //                    }
        //                    else
        //                    {
        //                        dataEfficacia1 = "";
        //                    }

        //                    //generazione data registrazione
        //                    if (dataRegAtti.Length > 0)
        //                    {
        //                        giorno = "";
        //                        mese = "";
        //                        anno = "";

        //                        giorno = dataRegAtti.Substring(0, 2);
        //                        mese = dataRegAtti.Substring(2, 2);
        //                        anno = dataRegAtti.Substring(4, 4);
        //                        dataRegAtti = " " + giorno + "/" + mese + "/" + anno;
        //                    }
        //                    else
        //                    {
        //                        dataRegAtti = "";
        //                    }

        //                    //conclusione data registrazione
        //                    if (dataRegAtti1.Length > 0)
        //                    {
        //                        giorno = "";
        //                        mese = "";
        //                        anno = "";

        //                        giorno = dataRegAtti1.Substring(0, 2);
        //                        mese = dataRegAtti1.Substring(2, 2);
        //                        anno = dataRegAtti1.Substring(4, 4);
        //                        dataRegAtti1 = " " + giorno + "/" + mese + "/" + anno;
        //                    }
        //                    else
        //                    {
        //                        dataRegAtti1 = "";
        //                    }

        //                    rowAnagr = dtAnagrafica.NewRow();

        //                    rowAnagr["idImmobile"] = idImmobile;
        //                    rowAnagr["cognomeIci"] = elencoNominativiIci;
        //                    rowAnagr["nomeIci"] = "";
        //                    rowAnagr["codFiscaleIci"] = elencoCodiciFiscaliIci2;
        //                    rowAnagr["cognomeCatasto"] = nominativiCatasto;
        //                    rowAnagr["nomeCatasto"] = "";
        //                    rowAnagr["codFiscaleCatasto"] = elencoCodFiscaliCat;

        //                    dtAnagrafica.Rows.Add(rowAnagr);

        //                    row = dtConfronto.NewRow();
        //                    row["foglio"] = foglio1;
        //                    row["numero"] = num1;
        //                    row["subalterno"] = sub1;
        //                    row["dataEfficacia"] = dataEfficacia;
        //                    row["dataEfficacia1"] = dataEfficacia1;
        //                    row["dataRegAtti"] = dataRegAtti;
        //                    row["dataRegAtti1"] = dataRegAtti1;
        //                    row["idImmobile"] = idImmobile;

        //                    dtConfronto.Rows.Add(row);

        //                }
        //            }
        //        }
        //    }

        //    catch (OleDbException ex)
        //    {
        //        if (ex.ErrorCode == -2147217900)
        //        {
        //            log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnPassaggioProprieta_Click.errore: ", ex);
        //             Response.Redirect("../../PaginaErrore.aspx");

        //            strscript = strscript + " alert('Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni con passaggio di proprietà.');";
        //            RegisterScript(sScript,this.GetType());,"", "" + strscript + "");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //            log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnPassaggioProprieta_Click.errore: ", ex);
        //             Response.Redirect("../../PaginaErrore.aspx");

        //        strscript = strscript + " alert('Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni con passaggio di proprietà.');";
        //        RegisterScript(sScript,this.GetType());,"", "" + strscript + "");
        //        return;
        //    }
        //    finally
        //    {
        //        if (sqlEliminaTabella != String.Empty)
        //        {
        //            oCMDCatasto = new SqlCommand();
        //            oCMDCatasto.CommandText = sqlEliminaTabella;
        //            oCMDCatasto.Connection = oConn;
        //            oCMDCatasto.ExecuteNonQuery();
        //            log.Debug("Elimino tabella Totale_soggetti - solo in catasto");
        //        }
        //        oConn.Close();

        //        oCMDCatasto.Dispose();
        //    }

        //    stampaExcelCONFRONTO_PROPRIETARI_CONCATASTO(dtConfronto, dtAnagrafica);
        //}

        //private bool CreaTabSoggetti(SqlConnection oConn, SqlConnection oConnCatasto, string Belfiore)
        //{
        //    string nomeTabellaAnagrafica, sqlEliminaTabella, sqlCreaTabella, sSQL, sSezione, sIdSoggetto, sTipoSoggetto, sCognome, sNome, sDataNascita, sLuogoNascita, sCodFiscale, sSede;
        //    SqlCommand oCMD = new SqlCommand();
        //    DataView dvMyDati;
        //    ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();

        //    try
        //    {
        //        /********** CREAZIONE E POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/
        //        nomeTabellaAnagrafica = "TOTALE_SOGGETTI_" + Belfiore;
        //        sqlEliminaTabella = "DROP TABLE " + nomeTabellaAnagrafica;
        //        //Dipe 13/05/2009
        //        try
        //        {
        //            oCMD = new SqlCommand();
        //            oCMD.CommandText = sqlEliminaTabella;
        //            oCMD.Connection = oConn;
        //            oCMD.ExecuteNonQuery();
        //        }
        //        catch
        //        {
        //            log.Debug("Tabella " + nomeTabellaAnagrafica + " non trovata");
        //        }

        //        sqlCreaTabella = "CREATE TABLE " + nomeTabellaAnagrafica + " (COD_AMM NVARCHAR(255), SEZIONE NVARCHAR(255), ID_SOGGETTO NVARCHAR(255), TIPO_SOGGETTO NVARCHAR(255), COGNOME NVARCHAR(255), NOME NVARCHAR(255), COD_FISCALE NVARCHAR(255), DATA_NASCITA  NVARCHAR(255), LUOGO_NASCITA NVARCHAR(255), SEDE NVARCHAR(255))";
        //        oCMD.CommandText = sqlCreaTabella;
        //        oCMD.Connection = oConn;
        //        oCMD.ExecuteNonQuery();

        //        oCMD.Dispose();

        //        log.Debug("Creato tabella Access Totale_soggetti - confronto classe catasto");

        //        sSQL = "SELECT COD_AMM_COMUNE, SEZIONE, ID_SOGGETTO, TIPO_SOGGETTO, DENOMINAZIONE, SEDE, COD_FISCALE";
        //        sSQL += " FROM FAB_PGIURIDICA";
        //        sSQL += " WHERE (COD_AMM_COMUNE='" + Belfiore + "')";
        //        dvMyDati = MyFnc.getDataView(oConnCatasto, sSQL);
        //        for (int x = 0; x < dvMyDati.Count; x++)
        //        {
        //            sSezione = dvMyDati.Table.Rows[x]["SEZIONE"].ToString();
        //            sIdSoggetto = dvMyDati.Table.Rows[x]["ID_SOGGETTO"].ToString();
        //            sTipoSoggetto = dvMyDati.Table.Rows[x]["TIPO_SOGGETTO"].ToString();
        //            sCognome = dvMyDati.Table.Rows[x]["DENOMINAZIONE"].ToString();
        //            sSede = dvMyDati.Table.Rows[x]["SEDE"].ToString();
        //            sCodFiscale = dvMyDati.Table.Rows[x]["COD_FISCALE"].ToString();

        //            sSQL = "INSERT INTO TOTALE_SOGGETTI_" + Belfiore + " (COD_AMM, SEZIONE, ID_SOGGETTO, TIPO_SOGGETTO, COGNOME, SEDE, COD_FISCALE) ";
        //            sSQL += " VALUES('" + Belfiore + "','" + sSezione + "','" + sIdSoggetto + "','" + sTipoSoggetto + "','" + sCognome + "','" + sSede + "','" + sCodFiscale + "')";

        //            oCMD = new SqlCommand();
        //            oCMD.CommandText = sSQL;
        //            oCMD.Connection = oConn;
        //            oCMD.ExecuteNonQuery();

        //            oCMD.Dispose();

        //            log.Debug("Inserito in tabella totale_soggetti persone giuridiche - confronto classe catasto");
        //        }
        //        dvMyDati.Dispose();

        //        sSQL = "SELECT COD_AMM_COMUNE, SEZIONE, ID_SOGGETTO, TIPO_SOGGETTO, COGNOME, NOME";
        //        sSQL += ", DATA_NASCITA, LUOGO_NASCITA, COD_FISCALE";
        //        sSQL += " FROM FAB_PFISICA";
        //        sSQL += " WHERE (COD_AMM_COMUNE='" + Belfiore + "')";
        //        dvMyDati = MyFnc.getDataView(oConnCatasto, sSQL);
        //        for (int x = 0; x < dvMyDati.Count; x++)
        //        {
        //            sSezione = dvMyDati.Table.Rows[x]["SEZIONE"].ToString();
        //            sIdSoggetto = dvMyDati.Table.Rows[x]["ID_SOGGETTO"].ToString();
        //            sTipoSoggetto = dvMyDati.Table.Rows[x]["TIPO_SOGGETTO"].ToString();
        //            sCognome = dvMyDati.Table.Rows[x]["COGNOME"].ToString();
        //            sNome = dvMyDati.Table.Rows[x]["NOME"].ToString();
        //            sDataNascita = dvMyDati.Table.Rows[x]["DATA_NASCITA"].ToString();
        //            sLuogoNascita = dvMyDati.Table.Rows[x]["LUOGO_NASCITA"].ToString();
        //            sCodFiscale = dvMyDati.Table.Rows[x]["COD_FISCALE"].ToString();

        //            sSQL = "INSERT INTO TOTALE_SOGGETTI_" + Belfiore + " ( COD_AMM, SEZIONE, ID_SOGGETTO, TIPO_SOGGETTO, COGNOME, NOME, DATA_NASCITA, LUOGO_NASCITA, COD_FISCALE ) ";
        //            sSQL += " VALUES('" + Belfiore + "','" + sSezione + "','" + sIdSoggetto + "','" + sTipoSoggetto + "','" + sCognome + "','" + sNome + "','" + sDataNascita + "','" + sLuogoNascita + "','" + sCodFiscale + "')";

        //            oCMD = new SqlCommand();
        //            oCMD.CommandText = sSQL;
        //            oCMD.Connection = oConn;
        //            oCMD.ExecuteNonQuery();

        //            oCMD.Dispose();

        //            log.Debug("Inserito in tabella totale_soggetti persone fisiche - confronto classe catasto");
        //        }
        //        dvMyDati.Dispose();

        //        return true;
        //        /********** FINE POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/
        //    }
        //    catch (Exception Err)
        //    {
        //     log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.CreaTabSoggetti.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //        return false;
        //    }
        //}

        //private void stampaExcelCONFRONTO_PROPRIETARI_CONCATASTO(DataTable dt, DataTable dtAnagrafica)
        //{
        //    DataRow dr;
        //    DataSet ds;
        //    DataTable dtConfrontoPCatasto;
        //    string sPathProspetti = string.Empty;
        //    string NameXLS = string.Empty;
        //    string nomeComune = "";
        //    nomeComune = ConstWrapper.DescrizioneEnte;
        //    string nomeStampa = "";

        //    ArrayList arratlistNomiColonne;
        //    string[] arraystr;

        //    arratlistNomiColonne = new ArrayList();

        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");

        //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

        //    sPathProspetti = System.Configuration.ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"].ToString();
        //    NameXLS = nomeComune + "_CONFRONTO_PROPRIETARI_CONCATASTO" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";
        //    nomeStampa = NameXLS;


        //    ds = CreateDataSetCONFRONTO_PROPRIETARI_CONCATASTO();

        //    dtConfrontoPCatasto = ds.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"];
        //    //ds_CONFRONTO_PROPRIETARI_CONCATASTO .Tables[0];

        //    dr = dtConfrontoPCatasto.NewRow();
        //    dr[0] = ConstWrapper.DescrizioneEnte;
        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    //inserisco intestazione - titolo prospetto + data stampa
        //    //*** 20120704 - IMU ***
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dr[0] = "Prospetto Confronto ICI/IMU - Catasto per Proprietari";
        //    dr[8] = "Data Stampa " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dtConfrontoPCatasto.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    //inserisco intestazione di colonna
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dr[0] = "Foglio";
        //    dr[1] = "Numero";
        //    dr[2] = "Subalterno";
        //    dr[3] = "Generazione Data Validita'";
        //    dr[4] = "Conclusione Data Validita'";
        //    dr[5] = "Generazione Data Registrazione";
        //    dr[6] = "Conclusione Data Registrazione";
        //    //*** 20120704 - IMU ***
        //    dr[7] = "Proprietari Vert. ICI/IMU";
        //    dr[8] = "Proprietari Cat.";

        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    string ID_IMMOBILE = string.Empty;
        //    string strProprietariCatasto;
        //    string strProprietariIci;
        //    DataRow[] rigaProprietari;
        //    string filtroRicerca = "";
        //    string subalterno = "";

        //    log.Error("Inizio ciclo dei dataset di anagrafica e immobili - passaggio proprietà");
        //try{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        strProprietariCatasto = "";
        //        strProprietariIci = "";
        //        ID_IMMOBILE = dt.Rows[i]["idImmobile"].ToString();

        //        if (dt.Rows[i]["subalterno"].ToString() == "-1")
        //            subalterno = "";
        //        else
        //            subalterno = dt.Rows[i]["subalterno"].ToString();

        //        dr = dtConfrontoPCatasto.NewRow();
        //        dr[0] = dt.Rows[i]["foglio"].ToString();
        //        dr[1] = dt.Rows[i]["numero"].ToString();
        //        dr[2] = subalterno;
        //        dr[3] = dt.Rows[i]["dataEfficacia"].ToString();
        //        dr[4] = dt.Rows[i]["dataEfficacia1"].ToString();
        //        dr[5] = dt.Rows[i]["dataRegAtti"].ToString();
        //        dr[6] = dt.Rows[i]["dataRegAtti1"].ToString();

        //        filtroRicerca = "idImmobile like '" + ID_IMMOBILE + "'";
        //        rigaProprietari = dtAnagrafica.Select(filtroRicerca);
        //        for (int j = 0; j < rigaProprietari.Length; j++)
        //        {
        //            strProprietariCatasto = rigaProprietari[j].ItemArray[4].ToString();
        //            strProprietariCatasto = strProprietariCatasto.Substring(0, (strProprietariCatasto.Length) - 1);

        //            strProprietariIci = rigaProprietari[j].ItemArray[1].ToString();
        //            //strProprietariIci = strProprietariIci.Substring(0,(strProprietariIci.Length)-1);

        //        }
        //        dr[7] = strProprietariIci;
        //        dr[8] = strProprietariCatasto;
        //        dtConfrontoPCatasto.Rows.Add(dr);
        //    }


        //    //inserisco riga vuota
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dtConfrontoPCatasto.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    //inserisco numero totali di contribuenti
        //    dr = dtConfrontoPCatasto.NewRow();
        //    dr[0] = "Totale Record nel File: " + (dt.Rows.Count);
        //    dtConfrontoPCatasto.Rows.Add(dr);

        //    log.Debug("Fase di stampa - passaggio proprietà");

        //    //definisco l'insieme delle colonne da esportare
        //    int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        //    //esporto i dati in excel
        //    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
        //    objExport.ExportDetails(dtConfrontoPCatasto, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, nomeStampa);
        //}

        //private DataSet CreateDataSetCONFRONTO_PROPRIETARI_CONCATASTO()
        //{
        //    DataSet dsTmp = new DataSet();

        //    dsTmp.Tables.Add("CONFRONTO_PROPRIETARI_CONCATASTO");
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_PROPRIETARI_CONCATASTO"].Columns.Add("").DataType = typeof(string);

        //    return dsTmp;
        //}
        //Catch (Exception Err){
        // log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.stampaExcelCONFRONTO_PROPRIETARI_CONCATASTO.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //}
        //}
        #endregion
        #region "Posizioni di Catasto non presenti in ICI"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        protected void btnICI_Click(object sender, System.EventArgs e)
        {
            DataTable dtDifferenze = null;
            string[] arraystr;
            string NameXLS = string.Empty;

            try
            {
                int nCol = 10;
                DataRow dr;
                DataSet ds = new DataSet();
                ArrayList arratlistNomiColonne = new ArrayList();

                //prelevo i dati da db
                ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();
                log.Debug("eseguo CatVSICI_NoICI");
                DataTable dtDiff = MyFnc.CatVSICI_NoICI(ConstWrapper.CodiceEnte, ConstWrapper.CodBelfiore, ddlAnnoRiferimento.SelectedItem.Value, ConstWrapper.StringConnection);
                log.Debug("fatto");
                checked
                {
                    for (int x = 0; x <= nCol; x++)
                        arratlistNomiColonne.Add("");
                }
                arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

                NameXLS = ConstWrapper.CodiceEnte + "_FGS_PRESENTE_SOLO_CATASTO_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                ds.Tables.Add("FGS_PRESENTE_SOLO_CATASTO");
                checked
                {
                    for (int x = 0; x <= nCol; x++)
                        ds.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
                }
                dtDifferenze = ds.Tables["FGS_PRESENTE_SOLO_CATASTO"];
                //inserisco intestazione - titolo prospetto + data stampa
                dr = dtDifferenze.NewRow();
                dr[0] = ConstWrapper.CodiceEnte + " - " + ConstWrapper.DescrizioneEnte;
                dtDifferenze.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //*** 20120704 - IMU ***
                dr = dtDifferenze.NewRow();
                dr[0] = "Prospetto Immobili presenti in catasto e non presenti nel verticale ICI/IMU";
                dr[10] = "Data Stampa " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
                dtDifferenze.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //inserisco intestazione di colonna
                dr = dtDifferenze.NewRow();
                dr[0] = "Foglio";
                dr[1] = "Numero";
                dr[2] = "Subalterno";
                dr[3] = "Categoria";
                dr[4] = "Classe";
                dr[5] = "Rendita Euro";
                dr[6] = "Generazione Data Validita";
                dr[7] = "Conclusione Data Validita";
                dr[8] = "Generazione Data Registrazione";
                dr[9] = "Generazione Data Registrazione";
                dr[10] = "Elenco Proprietari";
                dtDifferenze.Rows.Add(dr);
                foreach (DataRow rStampa in dtDiff.Rows)
                {
                    dr = dtDifferenze.NewRow();
                    dr[0] = rStampa["FOGLIOC"].ToString();
                    dr[1] = rStampa["NUMEROC"].ToString();
                    if (rStampa["SUBALTERNOC"].ToString() != "-1")
                        dr[2] = rStampa["SUBALTERNOC"].ToString();
                    else
                        dr[2] = "";
                    dr[3] = rStampa["CATEGORIA_CATASTO"].ToString();
                    dr[4] = rStampa["CLASSE_CATASTO"].ToString();
                    dr[5] = " " + DecimaliForStampa(rStampa["RENDITA_EURO"].ToString());
                    dr[6] = " " + rStampa["GENERAZIONE_DATA_VALIDITA"].ToString();
                    dr[7] = " " + rStampa["CONCLUSIONE_DATA_VALIDITA"].ToString();
                    dr[8] = " " + rStampa["GENERAZIONE_DATA_REGISTRAZIONE"].ToString();
                    dr[9] = " " + rStampa["CONCLUSIONE_DATA_REGISTRAZIONE"].ToString();
                    if (rStampa["PROPRIETARI"] != null)
                        dr[10] = rStampa["PROPRIETARI"].ToString();
                    else
                        dr[10] = "";
                    dtDifferenze.Rows.Add(dr);
                }
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //inserisco numero totali di contribuenti
                dr = dtDifferenze.NewRow();
                dr[0] = "Totale Record nel File: " + (dtDiff.Rows.Count);
                dtDifferenze.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnICI_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                //*** 20120704 - IMU ***
                string strscript = " GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni di Catasto non presenti in ICI/IMU.');";
                RegisterScript(strscript, this.GetType());
                return;
            }
            if (dtDifferenze != null)
            {
                //definisco l'insieme delle colonne da esportare
                int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                //esporto i dati in excel
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                objExport.ExportDetails(dtDifferenze, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }
        }
        //protected void btnICI_Click(object sender, System.EventArgs e)
        //{
        //    string strscript = string.Empty;
        //    string connectionServer2000 = string.Empty;
        //    string connectionCatasto2000 = string.Empty;

        //    string ID_IMMOBILE = string.Empty;
        //    string CLASSECatasto = String.Empty;
        //    string CATEGORIACatasto = String.Empty;
        //    string RENDITACatasto = string.Empty;
        //    string DATAEFFICACIACatasto = String.Empty;
        //    string DATAEFFICACIA1Catasto = String.Empty;
        //    string DATA_REG_ATTICatasto = String.Empty;
        //    string DATA_REG_ATTI1Catasto = String.Empty;

        //    string FOGLIOC = String.Empty;
        //    string NUMEROC = String.Empty;
        //    string SUBALTERNOC = String.Empty;

        //    string BELFIORE = String.Empty;

        //    string Giorno = string.Empty;
        //    string Mese = string.Empty;
        //    string Anno = string.Empty;
        //    string resultData = string.Empty;

        //    string nomeTabellaAnagrafica = string.Empty;
        //    string sqlCreaTabella = string.Empty;
        //    string sqlEliminaTabella = string.Empty;

        //    string sqlPgiuridiche = string.Empty;
        //    string sqlPFisiche = string.Empty;

        //    SqlConnection oConn = new SqlConnection();
        //    SqlCommand oCMD = new SqlCommand();
        //    SqlCommand oCMDCatasto;
        //    SqlConnection oConnCatasto = new SqlConnection();
        //    SqlDataReader drCatasto;
        //    DataView dvCatasto;
        //    ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();

        //   string sqlServer2000="";
        //   string sqlCatasto2000 = "";
        //   string annoConfronto = "";

        //   try
        //    {
        //       if (ConfigurationManager.AppSettings["sqlServerCatasto"] != null)
        //           sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();

        //        if (ConfigurationManager.AppSettings["sqlAccessCatasto"]!=null)
        //            sqlCatasto2000= ConfigurationManager.AppSettings["sqlAccessCatasto"].ToString();

        //        annoConfronto = ddlAnnoRiferimento.SelectedItem.Value;

        //        oConn.ConnectionString = sqlServer2000;
        //        oConn.Open();

        //        oConnCatasto.ConnectionString = sqlCatasto2000;
        //        oConnCatasto.Open();

        //        log.Error("Apro le connessioni ai database - solo in catasto");

        //        BELFIORE = ConstWrapper.CodBelfiore;
        //        log.Error("Codice belfiore valorizzato - solo catasto");

        //        /********** CREAZIONE E POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/
        //        if (CreaTabSoggetti(oConn, oConnCatasto, BELFIORE) == false)
        //        {
        //            throw new Exception("Errore in popolamento tabella soggetti.");
        //        }
        //        /********** FINE POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/

        //        string sqlDelete = "DELETE FROM TB_FGS_PRESENTE_SOLO_CATASTO where ENTE = '" + ConstWrapper.CodiceEnte + "' AND ANNO='" + annoConfronto + "'";
        //        sqlDelete = sqlDelete + "; DELETE FROM TB_FGS_PRESENTE_SOLO_CATASTO_PROPRIETARI where ENTE = '" + ConstWrapper.CodiceEnte + "' AND ANNO='" + annoConfronto + "'";

        //        oCMD = new SqlCommand();
        //        oCMD.CommandText = sqlDelete;
        //        oCMD.Connection = oConn;

        //        oCMD.ExecuteNonQuery();
        //        log.Error("cancello dati da tabelle TB_FGS_PRESENTE_SOLO_CATASTO e TB_FGS_PRESENTE_SOLO_CATASTO_PROPRIETARI - solo catasto");

        //        oCMD.Dispose();

        //        connectionCatasto2000 = "SELECT FAB.FOGLIO, FAB.NUMERO, ISNUMERIC(FAB.NUMERO) AS MAPPALE_ALFANUMERICO, FAB.SUBALTERNO, UI.CATEGORIA, UI.CLASSE, FAB.COD_AMM_COMUNE, UI.RENDITA_EURO, UI.DATA_EFFICACIA, UI.DATA_REG_ATTI, UI.DATA_EFFICACIA1, UI.DATA_REG_ATTI1, FAB.ID_IMMOBILE";
        //        connectionCatasto2000 += " FROM FAB_IDENTIFICATIVI FAB";
        //        connectionCatasto2000 += " INNER JOIN FAB_UNITA_IMMOBILIARE UI ON FAB.COD_AMM_COMUNE = UI.COD_AMM_COMUNE AND FAB.SEZIONE = UI.SEZIONE AND FAB.ID_IMMOBILE = UI.ID_IMMOBILE AND FAB.TIPO_IMMOBILE = UI.TIPO_IMMOBILE";
        //        connectionCatasto2000 += " WHERE 1=1";
        //        connectionCatasto2000 += " AND (ISNUMERIC(FAB.NUMERO) <> 0)";
        //        connectionCatasto2000 += " AND (DATA_EFFICACIA1 IS NULL OR DATA_EFFICACIA1='')";
        //        connectionCatasto2000 += " AND (DATA_REG_ATTI1 IS NULL OR DATA_REG_ATTI1='')";
        //        //connectionCatasto2000+=" AND (CATEGORIA NOT LIKE 'E%' AND CATEGORIA NOT LIKE 'F%' AND CATEGORIA NOT LIKE 'D10%')";
        //        connectionCatasto2000 += " AND (CATEGORIA NOT LIKE 'E%' AND CATEGORIA NOT LIKE 'F01%' AND CATEGORIA NOT LIKE 'F05%')";
        //        connectionCatasto2000 += " AND (FAB.COD_AMM_COMUNE='" + BELFIORE + "')";
        //        //escludo gli immobili con mappale alfanumerico
        //        oCMDCatasto = new SqlCommand();
        //        oCMDCatasto.CommandText = connectionCatasto2000;
        //        oCMDCatasto.Connection = oConnCatasto;
        //        drCatasto = oCMDCatasto.ExecuteReader();

        //        log.Error("Query in GRANDCOMBINCATASTO - solo in catasto");

        //        bool bRetVal;

        //        while (drCatasto.Read())
        //        {
        //            ID_IMMOBILE = drCatasto["ID_IMMOBILE"].ToString();

        //            FOGLIOC = drCatasto["FOGLIO"].ToString();

        //            //try
        //            //{
        //            //    FOGLIOC = (long.Parse(FOGLIOC)).ToString();
        //            //}
        //            //catch (FormatException)
        //            //{

        //            //}

        //            NUMEROC = drCatasto["NUMERO"].ToString();

        //            //try
        //            //{
        //            //    NUMEROC = (long.Parse(NUMEROC)).ToString();
        //            //}
        //            //catch (FormatException)
        //            //{

        //            //}

        //            SUBALTERNOC = drCatasto["SUBALTERNO"].ToString();
        //            try
        //            {
        //                SUBALTERNOC = (long.Parse(SUBALTERNOC)).ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Debug("Posizioni di Catasto non presenti in ICI::IDIMMOBILE::" + ID_IMMOBILE + ":: subalterno non numerico::" + SUBALTERNOC, ex);
        //                SUBALTERNOC = "-1";
        //            }

        //            if (SUBALTERNOC == "")
        //                SUBALTERNOC = "-1";

        //            CLASSECatasto = drCatasto["CLASSE"].ToString();
        //            CLASSECatasto = CLASSECatasto.Replace("0", "");

        //            CATEGORIACatasto = drCatasto["CATEGORIA"].ToString();
        //            if (CATEGORIACatasto.Length > 1)
        //            {
        //                CATEGORIACatasto = CATEGORIACatasto.Substring(0, 1) + "/" + int.Parse(CATEGORIACatasto.Substring(1, (CATEGORIACatasto.Length - 1)));
        //            }

        //            RENDITACatasto = drCatasto["RENDITA_EURO"].ToString();

        //            DATAEFFICACIACatasto = drCatasto["DATA_EFFICACIA"].ToString();
        //            if (DATAEFFICACIACatasto.Length > 7)
        //            {
        //                Giorno = DATAEFFICACIACatasto.Substring(0, 2);
        //                Mese = DATAEFFICACIACatasto.Substring(2, 2);
        //                Anno = DATAEFFICACIACatasto.Substring(4, 4);
        //                resultData = Giorno + "/" + Mese + "/" + Anno;

        //                DATAEFFICACIACatasto = resultData;
        //            }

        //            DATAEFFICACIA1Catasto = drCatasto["DATA_EFFICACIA1"].ToString();
        //            if (DATAEFFICACIA1Catasto.Length > 7)
        //            {
        //                Giorno = DATAEFFICACIA1Catasto.Substring(0, 2);
        //                Mese = DATAEFFICACIA1Catasto.Substring(2, 2);
        //                Anno = DATAEFFICACIA1Catasto.Substring(4, 4);
        //                resultData = Giorno + "/" + Mese + "/" + Anno;
        //                DATAEFFICACIA1Catasto = resultData;
        //            }

        //            DATA_REG_ATTICatasto = drCatasto["DATA_REG_ATTI"].ToString();
        //            if (DATA_REG_ATTICatasto.Length > 7)
        //            {
        //                Giorno = DATA_REG_ATTICatasto.Substring(0, 2);
        //                Mese = DATA_REG_ATTICatasto.Substring(2, 2);
        //                Anno = DATA_REG_ATTICatasto.Substring(4, 4);
        //                resultData = Giorno + "/" + Mese + "/" + Anno;
        //                DATA_REG_ATTICatasto = resultData;
        //            }

        //            DATA_REG_ATTI1Catasto = drCatasto["DATA_REG_ATTI1"].ToString();
        //            if (DATA_REG_ATTI1Catasto.Length > 7)
        //            {
        //                Giorno = DATA_REG_ATTI1Catasto.Substring(0, 2);
        //                Mese = DATA_REG_ATTI1Catasto.Substring(2, 2);
        //                Anno = DATA_REG_ATTI1Catasto.Substring(4, 4);
        //                resultData = Giorno + "/" + Mese + "/" + Anno;
        //                DATA_REG_ATTI1Catasto = resultData;
        //            }


        //            connectionServer2000 = " SELECT COUNT(*) AS NRC";
        //            connectionServer2000 = connectionServer2000 + " FROM TBLOGGETTI ";
        //            connectionServer2000 = connectionServer2000 + " WHERE (YEAR(DBO.TBLOGGETTI.DATAINIZIO) <= " + annoConfronto + ")" + " AND (YEAR(DBO.TBLOGGETTI.DATAFINE) >= " + annoConfronto + ")" + " AND TBLOGGETTI.ENTE = '" + ConstWrapper.CodiceEnte + "'";
        //            connectionServer2000 = connectionServer2000 + " AND FOGLIO ='" + FOGLIOC + "' AND NUMERO='" + NUMEROC + "' AND SUBALTERNO='" + SUBALTERNOC + "'";
        //            dvCatasto = MyFnc.getDataView(oConn, connectionServer2000);
        //            for (int x = 0; x < dvCatasto.Count; x++)
        //            {
        //                if ((int)dvCatasto.Table.Rows[x]["NRC"] == 0)
        //                {
        //                    //INSERISCO NELLA TABELLA PIATTA TB_CONFRONTO_CONCATASTO
        //                    //CAMPI CONTRIBUENTE CLASSECatasto, CATEGORIACatasto, CLASSE, CATEGORIA, FOGLIO NUMERO SUBALTERNO ESISTENTE
        //                    connectionServer2000 = "INSERT INTO TB_FGS_PRESENTE_SOLO_CATASTO (ENTE,ANNO,ID_IMMOBILE,FOGLIOC,NUMEROC,SUBALTERNOC, PRESENTE_SOLO_CATASTO, CLASSE_CATASTO, CATEGORIA_CATASTO,";
        //                    connectionServer2000 = connectionServer2000 + " RENDITA_EURO, GENERAZIONE_DATA_VALIDITA,CONCLUSIONE_DATA_VALIDITA,GENERAZIONE_DATA_REGISTRAZIONE,CONCLUSIONE_DATA_REGISTRAZIONE)";
        //                    connectionServer2000 = connectionServer2000 + " VALUES('" + ConstWrapper.CodiceEnte + "'," +
        //                        "'" + annoConfronto + "'," +
        //                        "'" + ID_IMMOBILE + "'," +
        //                        "'" + FOGLIOC + "'," +
        //                        "'" + NUMEROC + "'," +
        //                        "'" + SUBALTERNOC + "',1," +
        //                        "'" + CLASSECatasto.ToString().Replace("0", "") + "'," +
        //                        "'" + CATEGORIACatasto + "'," +
        //                        "'" + RENDITACatasto + "'," +
        //                        "'" + DATAEFFICACIACatasto + "'," +
        //                        "'" + DATAEFFICACIA1Catasto + "'," +
        //                        "'" + DATA_REG_ATTICatasto + "'," +
        //                        "'" + DATA_REG_ATTI1Catasto + "')";

        //                    SqlConnection oConnDappoggio = new SqlConnection();
        //                    oConnDappoggio.ConnectionString = sqlServer2000;
        //                    oConnDappoggio.Open();
        //                    oCMD.CommandText = connectionServer2000;
        //                    oCMD.Connection = oConnDappoggio;
        //                    oCMD.ExecuteNonQuery();
        //                    oConnDappoggio.Close();

        //                    log.Error("Inserimento dati in tabella TB_FGS_PRESENTE_SOLO_CATASTO - solo in catasto");
        //                }
        //            }
        //            oCMD.Dispose();
        //        }
        //        drCatasto.Close();
        //        oCMDCatasto.Dispose();
        //        //inserisco i proprietari
        //        bRetVal = this.SetProprietari(oConn, oConnCatasto, "", BELFIORE, annoConfronto, "D");
        //    }
        //    catch (SqlException ex)
        //    {
        //        log.Error("Si è verificato un errore in btnICI_Click::", ex);
        //        if (ex.Number == -2147217900)
        //        {
        //             //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnICI_Click.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //            //*** 20120704 - IMU ***
        //            strscript = strscript + " alert('Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni di Catasto non presenti in ICI/IMU.');";
        //            RegisterScript(sScript,this.GetType());,"", "" + strscript + "");
        //            return;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnICI_Click.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //        //*** 20120704 - IMU ***
        //        strscript = strscript + " alert('Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni di Catasto non presenti in ICI/IMU.');";
        //        RegisterScript(sScript,this.GetType());,"", "" + strscript + "");
        //        return;
        //    }
        //    finally
        //    {
        //        if (sqlEliminaTabella != String.Empty)
        //        {
        //            oCMD = new SqlCommand();
        //            oCMD.CommandText = sqlEliminaTabella;
        //            oCMD.Connection = oConn;
        //            oCMD.ExecuteNonQuery();
        //            log.Debug("Elimino tabella Totale_soggetti - solo in catasto");
        //        }
        //        oConnCatasto.Close();
        //        oConn.Close();
        //        oCMD.Dispose();
        //    }

        //    this.stampaExcelTB_FGS_PRESENTE_SOLO_CATASTO(annoConfronto);
        //}

        //private bool SetProprietari(SqlConnection oConn, SqlConnection oConnCatasto, string ID_IMMOBILE, string BELFIORE, string ANNO, string TIPO)
        //{
        //    SqlCommand oCDMAccess;
        //    SqlCommand oCDM;
        //    DataView dvAccess;
        //    DataView dvCatasto;
        //    string sSQL;
        //    string COGNOME = string.Empty;
        //    string NOME = string.Empty;
        //    string COD_FISCALE = string.Empty;
        //    string DATA_NASCITA = string.Empty;
        //    string COMUNE = string.Empty;
        //    string PROVINCIA = string.Empty;

        //    string Giorno = string.Empty;
        //    string Mese = string.Empty;
        //    string Anno = string.Empty;
        //    string resultData = string.Empty;

        //    string ID_SOGGETTO = string.Empty;
        //    ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();

        //    try
        //    {
        //        log.Error("Query per recupero anagrafiche access");

        //        oCDMAccess = new SqlCommand();
        //        oCDMAccess.Connection = oConnCatasto;

        //        oCDM = new SqlCommand();
        //        oCDM.Connection = oConn;

        //        sSQL = "SELECT ID_SOGGETTO, ID_IMMOBILE";
        //        sSQL += " FROM FAB_TITOLARITA FAB";
        //        sSQL += " WHERE 1=1";
        //        sSQL += " AND (COD_AMM_COMUNE='" + BELFIORE + "')";
        //        if (ID_IMMOBILE != "")
        //        {
        //            sSQL += " AND (ID_IMMOBILE='" + ID_IMMOBILE + "')";
        //        }
        //        oCDMAccess.CommandText = sSQL;
        //        dvCatasto = MyFnc.getDataView(oConnCatasto, sSQL);
        //        for (int y = 0; y < dvCatasto.Count; y++)
        //        {
        //            ID_SOGGETTO = dvCatasto.Table.Rows[y]["ID_SOGGETTO"].ToString();
        //            ID_IMMOBILE = dvCatasto.Table.Rows[y]["ID_IMMOBILE"].ToString();

        //            sSQL = "SELECT TOTALE_SOGGETTI_" + BELFIORE + ".COGNOME, TOTALE_SOGGETTI_" + BELFIORE + ".NOME, TOTALE_SOGGETTI_" + BELFIORE + ".COD_FISCALE, TOTALE_SOGGETTI_" + BELFIORE + ".DATA_NASCITA, COMUNE.COMUNE, COMUNE.PV";
        //            sSQL += " FROM TOTALE_SOGGETTI_" + BELFIORE;
        //            sSQL += " LEFT JOIN " + ConfigurationManager.AppSettings["NOME_DATABASE_OPENGOV"].ToString() + ".DBO.COMUNI COMUNE ON TOTALE_SOGGETTI_" + BELFIORE + ".LUOGO_NASCITA = COMUNE.IDENTIFICATIVO COLLATE LATIN1_GENERAL_CI_AS";
        //            sSQL += " WHERE (COD_AMM='" + BELFIORE + "' AND ID_SOGGETTO='" + ID_SOGGETTO + "')";
        //            oCDM.CommandText = sSQL;
        //            dvAccess = MyFnc.getDataView(oConn, sSQL);
        //            for (int x = 0; x < dvAccess.Count; x++)
        //            {
        //                COGNOME = dvAccess.Table.Rows[x]["COGNOME"].ToString();
        //                NOME = dvAccess.Table.Rows[x]["NOME"].ToString();
        //                COD_FISCALE = dvAccess.Table.Rows[x]["COD_FISCALE"].ToString();
        //                DATA_NASCITA = dvAccess.Table.Rows[x]["DATA_NASCITA"].ToString();
        //                if (DATA_NASCITA.Length > 7)
        //                {
        //                    Giorno = DATA_NASCITA.Substring(0, 2);
        //                    Mese = DATA_NASCITA.Substring(2, 2);
        //                    Anno = DATA_NASCITA.Substring(4, 4);
        //                    resultData = Giorno + "/" + Mese + "/" + Anno;

        //                    DATA_NASCITA = resultData;
        //                }
        //                COMUNE = dvAccess.Table.Rows[x]["COMUNE"].ToString();
        //                PROVINCIA = dvAccess.Table.Rows[x]["PV"].ToString();
        //                //confronto
        //                if (TIPO.CompareTo("C") == 0)
        //                {
        //                    //INSERISCO NELLA TABELLA PIATTA TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO_PROPRIETARI
        //                    //ENTE,ID_IMMOBILE,COGNOME,NOME,COD_FISCALE,DATA_NASCITA,COMUNE,PROVINCIA
        //                    sSQL = "INSERT INTO TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO_PROPRIETARI (ENTE,ANNO,ID_IMMOBILE,COGNOME,NOME,COD_FISCALE,DATA_NASCITA,COMUNE,PROVINCIA)";
        //                    sSQL = sSQL + " VALUES('" + ConstWrapper.CodiceEnte + "'," +
        //                        "'" + ANNO + "'," +
        //                        "'" + ID_IMMOBILE + "'," +
        //                        "'" + COGNOME.Replace("'", "''") + "'," +
        //                        "'" + NOME.Replace("'", "''") + "'," +
        //                        "'" + COD_FISCALE + "'," +
        //                        "'" + DATA_NASCITA + "'," +
        //                        "'" + COMUNE.Replace("'", "''") + "'," +
        //                        "'" + PROVINCIA + "')";

        //                    log.Error("query Inserimento anagrafiche in tabella TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO_PROPRIETARI - confronto classe catatsto");
        //                }
        //                else if (TIPO.CompareTo("D") == 0)//differenze
        //                {

        //                    //INSERISCO NELLA TABELLA PIATTA TB_FGS_PRESENTE_SOLO_CATASTO_PROPRIETARI
        //                    //ENTE,ID_IMMOBILE,COGNOME,NOME,COD_FISCALE,DATA_NASCITA,COMUNE,PROVINCIA
        //                    sSQL = "INSERT INTO TB_FGS_PRESENTE_SOLO_CATASTO_PROPRIETARI (ENTE,ANNO,ID_IMMOBILE,COGNOME,NOME,COD_FISCALE,DATA_NASCITA,COMUNE,PROVINCIA)";
        //                    sSQL = sSQL + " VALUES('" + ConstWrapper.CodiceEnte + "'," +
        //                        "'" + ANNO + "'," +
        //                        "'" + ID_IMMOBILE + "'," +
        //                        "'" + COGNOME.Replace("'", "''") + "'," +
        //                        "'" + NOME.Replace("'", "''") + "'," +
        //                        "'" + COD_FISCALE + "'," +
        //                        "'" + DATA_NASCITA + "'," +
        //                        "'" + COMUNE.Replace("'", "''") + "'," +
        //                        "'" + PROVINCIA + "')";

        //                    log.Error("query Inserimento anagrafiche in tabella TB_FGS_PRESENTE_SOLO_CATASTO_PROPRIETARI - presenti solo in catasto");
        //                }

        //                //					oCDM=new SqlCommand(); 
        //                oCDM.CommandText = sSQL;
        //                //					oCDM.Connection =oConn;
        //                oCDM.ExecuteNonQuery();
        //                //						oCDM.Dispose();

        //                log.Error("Inserimento anagrafiche");

        //            }
        //            dvAccess.Dispose();
        //            oCDM.Dispose();
        //        }
        //        dvCatasto.Dispose();
        //        oCDMAccess.Dispose();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.SetProprietari.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //        throw new Exception(ex.Message);
        //    }
        //}

        //private void stampaExcelTB_FGS_PRESENTE_SOLO_CATASTO(string ANNO)
        //{

        //    DataRow dr;
        //    DataSet ds;
        //    DataTable dtDifferenze;
        //    string sPathProspetti = string.Empty;
        //    string NameXLS = string.Empty;

        //    SqlConnection oConn = new SqlConnection();
        //    SqlCommand oCMD;

        //    /****************************************************
        //    prelevo i dati da stampare
        //    *****************************************************/

        //    string sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();
        //    sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();

        //    oConn.ConnectionString = sqlServer2000;
        //    oConn.Open();

        //    string sql = "select * from TB_FGS_PRESENTE_SOLO_CATASTO";
        //    sql = sql + " where ente='" + ConstWrapper.CodiceEnte + "' AND ANNO='" + ANNO + "'";
        //    sql = sql + " ORDER BY CAST(FOGLIOC AS integer), CAST(NUMEROC AS integer), CAST(SUBALTERNOC AS integer)";

        //    oCMD = new SqlCommand();
        //    oCMD.CommandText = sql;
        //    oCMD.Connection = oConn;

        //    log.Error("Query in tabella TB_FGS_PRESENTE_SOLO_CATASTO per stampa excel - solo in catasto");

        //    SqlDataAdapter da1 = new SqlDataAdapter(oCMD);
        //    DataSet ds_FGS_PRESENTE_SOLO_CATASTO = new DataSet();
        //    da1.Fill(ds_FGS_PRESENTE_SOLO_CATASTO);

        //    /*****************************************************/

        //    ArrayList arratlistNomiColonne;
        //    string[] arraystr;
        //    string nomeComune = "";
        //    nomeComune = ConstWrapper.DescrizioneEnte;

        //    arratlistNomiColonne = new ArrayList();

        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    //arratlistNomiColonne.Add("");


        //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

        //    sPathProspetti = System.Configuration.ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"].ToString();
        //    NameXLS = nomeComune + "_FGS_PRESENTE_SOLO_CATASTO " + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";


        //    ds = CreateDataSetFGS_PRESENTE_SOLO_CATASTO();

        //    dtDifferenze = ds.Tables["FGS_PRESENTE_SOLO_CATASTO"];
        //    //ds_FGS_PRESENTE_SOLO_CATASTO.Tables[0];

        //    //inserisco intestazione - titolo prospetto + data stampa
        //    dr = dtDifferenze.NewRow();
        //    dr[0] = ConstWrapper.DescrizioneEnte;
        //    dtDifferenze.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtDifferenze.NewRow();
        //    dtDifferenze.Rows.Add(dr);

        //    //*** 20120704 - IMU ***
        //    dr = dtDifferenze.NewRow();
        //    dr[0] = "Prospetto Immobili presenti in catasto e non presenti nel verticale ICI/IMU";
        //    dr[10] = "Data Stampa " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
        //    dtDifferenze.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtDifferenze.NewRow();
        //    dtDifferenze.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtDifferenze.NewRow();
        //    dtDifferenze.Rows.Add(dr);

        //    //inserisco intestazione di colonna
        //    dr = dtDifferenze.NewRow();
        //    dr[0] = "Foglio";
        //    dr[1] = "Numero";
        //    dr[2] = "Subalterno";
        //    dr[3] = "Categoria";
        //    dr[4] = "Classe";
        //    dr[5] = "Rendita Euro";

        //    dr[6] = "Generazione Data Validita";
        //    dr[7] = "Conclusione Data Validita";
        //    dr[8] = "Generazione Data Registrazione";
        //    dr[9] = "Generazione Data Registrazione";
        //    //dr[10] = "Presente solo in Catasto";

        //    dr[10] = "Elenco Proprietari";


        //    dtDifferenze.Rows.Add(dr);
        //    DataView dvDiff = new DataView();
        //    dvDiff = ds_FGS_PRESENTE_SOLO_CATASTO.Tables[0].DefaultView;

        //    string ID_IMMOBILE = string.Empty;
        //    SqlDataReader drSQL;
        //    string strProprietari;
        //    string subalterno = "";

        //    log.Error("Inizio ciclo di stampa - solo in catasto");
        //try{
        //    for (int i = 0; i < dvDiff.Count; i++)
        //    {
        //        if (dvDiff[i].Row["SUBALTERNOC"].ToString() != "-1")
        //            subalterno = dvDiff[i].Row["SUBALTERNOC"].ToString();
        //        else
        //            subalterno = "";

        //        dr = dtDifferenze.NewRow();
        //        dr[0] = dvDiff[i].Row["FOGLIOC"].ToString();
        //        dr[1] = dvDiff[i].Row["NUMEROC"].ToString();
        //        //dr[2] = dvDiff[i].Row["SUBALTERNOC"].ToString();
        //        dr[2] = subalterno;
        //        dr[3] = dvDiff[i].Row["CATEGORIA_CATASTO"].ToString();
        //        dr[4] = dvDiff[i].Row["CLASSE_CATASTO"].ToString();
        //        //dr[5] = " " + dvDiff[i].Row["RENDITA_EURO"].ToString().Replace(",",".") ;				
        //        dr[5] = " " + DecimaliForStampa(dvDiff[i].Row["RENDITA_EURO"].ToString());
        //        dr[6] = " " + dvDiff[i].Row["GENERAZIONE_DATA_VALIDITA"].ToString();
        //        dr[7] = " " + dvDiff[i].Row["CONCLUSIONE_DATA_VALIDITA"].ToString();
        //        dr[8] = " " + dvDiff[i].Row["GENERAZIONE_DATA_REGISTRAZIONE"].ToString();
        //        dr[9] = " " + dvDiff[i].Row["CONCLUSIONE_DATA_REGISTRAZIONE"].ToString();
        //        dr[10] = "SI";
        //        //dvDiff[i].Row["PRESENTE_SOLO_CATASTO"].ToString() ;

        //        ID_IMMOBILE = dvDiff[i].Row["ID_IMMOBILE"].ToString();
        //        strProprietari = "";


        //        sql = "select * from TB_FGS_PRESENTE_SOLO_CATASTO_PROPRIETARI";
        //        sql = sql + " where ente='" + ConstWrapper.CodiceEnte + "' AND ID_IMMOBILE='" + ID_IMMOBILE + "' AND ANNO='" + ANNO + "'";

        //        oCMD = new SqlCommand();
        //        oCMD.CommandText = sql;
        //        oCMD.Connection = oConn;

        //        drSQL = oCMD.ExecuteReader();

        //        //				if (drSQL.HasRows==true)
        //        //				{
        //        //inserisco riga vuota
        //        //					dr = dtDifferenze.NewRow();
        //        //					dtDifferenze.Rows.Add(dr);
        //        //
        //        //inserisco Titolo proprietari
        //        //					dr = dtDifferenze.NewRow();
        //        //					dr[0]="Elenco Proprietari";
        //        //					dtDifferenze.Rows.Add(dr);
        //        //
        //        //inserisco riga vuota
        //        //					dr = dtDifferenze.NewRow();
        //        //					dtDifferenze.Rows.Add(dr);
        //        //
        //        //inserisco intestazioni proprietari
        //        //					dr = dtDifferenze.NewRow();
        //        //
        //        //					dr[0] = "Cognome";
        //        //					dr[1] = "Nome";
        //        //					dr[2] = "Codice Fiscale/Partita Iva";
        //        //					dr[3] = "Data Nascita";
        //        //					dr[4] = "Comune Nascita";
        //        //					dr[5] = "Provincia Nascita";
        //        //
        //        //					dtDifferenze.Rows.Add(dr);
        //        //				}

        //        while (drSQL.Read())
        //        {
        //            //dr = dtDifferenze.NewRow();
        //            if (strProprietari.CompareTo("") == 0)
        //            {
        //                strProprietari = strProprietari + drSQL["COGNOME"].ToString() + " " + drSQL["NOME"].ToString();
        //            }
        //            else
        //            {
        //                strProprietari = strProprietari + "," + drSQL["COGNOME"].ToString() + " " + drSQL["NOME"].ToString();
        //            }
        //            //dr[2] = drSQL["COD_FISCALE"].ToString ();
        //            //dr[3] = drSQL["DATA_NASCITA"].ToString ();
        //            //dr[4] = drSQL["COMUNE"].ToString ();
        //            //dr[5] = drSQL["PROVINCIA"].ToString ();

        //            //dtDifferenze.Rows.Add(dr);

        //        }

        //        //inserisco riga vuota
        //        //				dr = dtDifferenze.NewRow();
        //        //				dtDifferenze.Rows.Add(dr);

        //        drSQL.Close();
        //        oCMD.Dispose();

        //        dr[10] = strProprietari;
        //        dtDifferenze.Rows.Add(dr);


        //    }
        //    //inserisco riga vuota
        //    dr = dtDifferenze.NewRow();
        //    dtDifferenze.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtDifferenze.NewRow();
        //    dtDifferenze.Rows.Add(dr);

        //    //inserisco numero totali di contribuenti
        //    dr = dtDifferenze.NewRow();
        //    dr[0] = "Totale Record nel FIle: " + (dvDiff.Count);
        //    dtDifferenze.Rows.Add(dr);

        //    log.Error("Fase di stampa - solo in catasto");

        //    //definisco l'insieme delle colonne da esportare
        //    int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        //    //esporto i dati in excel
        //    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
        //    objExport.ExportDetails(dtDifferenze, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);

        //}

        //private DataSet CreateDataSetFGS_PRESENTE_SOLO_CATASTO()
        //{
        //    DataSet dsTmp = new DataSet();

        //    dsTmp.Tables.Add("FGS_PRESENTE_SOLO_CATASTO");
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    //dsTmp.Tables["FGS_PRESENTE_SOLO_CATASTO"].Columns.Add("").DataType = typeof(string);
        //    return dsTmp;
        //}
        //Catch(Exception Err){
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.stampaExcelTB_FGS_PRESENTE_SOLO_CATASTO.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //}
        //}
        #endregion
        #region "Confronto ICI/CASTASTO"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory>
        /// <revision date="04/07/2012">
        /// <strong>IMU</strong>
        /// passaggio tributo da ICI a IMU
        /// </revision>
        /// </revisionHistory>
        protected void btnClasseCatastato_Click(object sender, System.EventArgs e)
        {
            DataTable dtDifferenze = null;
            string[] arraystr;
            string NameXLS = string.Empty;

            try
            {
                int nCol = 17;
                DataRow dr;
                DataSet ds = new DataSet();
                ArrayList arratlistNomiColonne = new ArrayList();

                //prelevo i dati da db
                ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();
                log.Debug("eseguo CatVSICI_DifCat");
                DataTable dtDiff = MyFnc.CatVSICI_DifCat(ConstWrapper.CodiceEnte, ConstWrapper.CodBelfiore, ddlAnnoRiferimento.SelectedItem.Value, ConstWrapper.StringConnection);
                log.Debug("fatto");
                checked
                {
                    for (int x = 0; x <= nCol; x++)
                        arratlistNomiColonne.Add("");
                }
                arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

                NameXLS = ConstWrapper.CodiceEnte + "_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";

                ds.Tables.Add("CONFRONTO_CLASSE_CATEGORIA_CONCATASTO");
                checked
                {
                    for (int x = 0; x <= nCol; x++)
                        ds.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
                }
                dtDifferenze = ds.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"];
                //inserisco intestazione - titolo prospetto + data stampa
                dr = dtDifferenze.NewRow();
                dr[0] = ConstWrapper.CodiceEnte + " - " + ConstWrapper.DescrizioneEnte;
                dtDifferenze.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //*** 20120704 - IMU ***
                dr = dtDifferenze.NewRow();
                dr[0] = "Prospetto Confronto ICI/IMU - Catasto per Categoria, Classe e Rendita";
                dr[17] = "Data Stampa " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
                dtDifferenze.Rows.Add(dr);
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //inserisco intestazione di colonna
                dr = dtDifferenze.NewRow();
                dr[0] = "Foglio Vert. ICI/IMU";
                dr[1] = "Numero Vert. ICI/IMU";
                dr[2] = "Subalterno Vert. ICI/IMU";
                dr[3] = "Categoria Vert. ICI/IMU";
                dr[4] = "Classe Vert. ICI/IMU";
                dr[5] = "Valore Vert. ICI/IMU Euro";
                dr[6] = "Foglio Cat.";
                dr[7] = "Numero Cat.";
                dr[8] = "Subalterno Cat.";
                dr[9] = "Categoria Cat.";
                dr[10] = "Classe Cat.";
                dr[11] = "Rendita Cat. Euro";
                dr[12] = "Valore Cat. Euro";
                dr[13] = "Generazione Data Validita";
                dr[14] = "Conclusione Data Validita";
                dr[15] = "Generazione Data Registrazione";
                dr[16] = "Conclusione Data Registrazione";
                dr[17] = "Elenco Proprietari";
                dtDifferenze.Rows.Add(dr);
                foreach (DataRow rStampa in dtDiff.Rows)
                {
                    dr = dtDifferenze.NewRow();
                    dr[0] = rStampa["FOGLIOICI"].ToString();
                    dr[1] = rStampa["NUMEROICI"].ToString();
                    if (rStampa["SUBALTERNOICI"].ToString() != "-1")
                        dr[2] = rStampa["SUBALTERNOICI"].ToString();
                    else
                        dr[2] = "";
                    dr[3] = rStampa["CATEGORIAICI"].ToString();
                    dr[4] = rStampa["CLASSEICI"].ToString();
                    dr[5] = " " + DecimaliForStampa(rStampa["VALOREICI"].ToString());
                    dr[6] = rStampa["FOGLIOC"].ToString();
                    dr[7] = rStampa["NUMEROC"].ToString();
                    if (rStampa["SUBALTERNOC"].ToString() != "-1")
                        dr[8] = rStampa["SUBALTERNOC"].ToString();
                    else
                        dr[8] = "";
                    dr[9] = rStampa["CATEGORIA_CATASTO"].ToString();
                    dr[10] = rStampa["CLASSE_CATASTO"].ToString();
                    dr[11] = " " + DecimaliForStampa(rStampa["RENDITA_EURO"].ToString());
                    dr[12] = " " + DecimaliForStampa(rStampa["VALORE_ACCESS"].ToString());
                    dr[13] = " " + rStampa["GENERAZIONE_DATA_VALIDITA"].ToString();
                    dr[14] = " " + rStampa["CONCLUSIONE_DATA_VALIDITA"].ToString();
                    dr[15] = " " + rStampa["GENERAZIONE_DATA_REGISTRAZIONE"].ToString();
                    dr[16] = " " + rStampa["CONCLUSIONE_DATA_REGISTRAZIONE"].ToString();
                    if (rStampa["PROPRIETARI"] != null)
                        dr[17] = rStampa["PROPRIETARI"].ToString();
                    else
                        dr[17] = "";
                    dtDifferenze.Rows.Add(dr);
                }
                //inserisco riga vuota
                dr = dtDifferenze.NewRow();
                dtDifferenze.Rows.Add(dr);
                //inserisco numero totali di contribuenti
                dr = dtDifferenze.NewRow();
                dr[0] = "Totale Record nel File: " + (dtDiff.Rows.Count);
                dtDifferenze.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnClasseCatastato_Click.errore: ", ex);
                Response.Redirect("../../PaginaErrore.aspx");
                //*** 20120704 - IMU ***
                string strscript = " GestAlert('a', 'danger', '', '', 'Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel dell\\'elenco delle posizioni di Catasto.');";
                RegisterScript(strscript, this.GetType());
                return;
            }
            if (dtDifferenze != null)
            {
                //definisco l'insieme delle colonne da esportare
                int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
                //esporto i dati in excel
                RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                objExport.ExportDetails(dtDifferenze, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
            }
        }
        //protected void btnClasseCatastato_Click(object sender, System.EventArgs e)
        //{
        //    string strscript = string.Empty;
        //    string annoConfronto = ddlAnnoRiferimento.SelectedItem.Value;

        //    SqlConnection oConn = new SqlConnection();
        //    SqlCommand oCMD;
        //    SqlConnection oConnCatasto = new SqlConnection();
        //    DataView dvCatasto;
        //    SqlDataReader dr;
        //    ClsConfrontoCatastoDB MyFnc = new ClsConfrontoCatastoDB();

        //    string sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();
        //    string sqlAccess2000 = ConfigurationManager.AppSettings["sqlAccessCatasto"].ToString();

        //    string connectionServer2000 = string.Empty;
        //    string connectionAccess2000 = string.Empty;

        //    string ID_IMMOBILE = string.Empty;
        //    string CLASSE = String.Empty;
        //    string CATEGORIA = String.Empty;
        //    string CONSISTENZA = String.Empty;
        //    decimal VALORE;
        //    decimal RIVALUTAZIONE;

        //    string CLASSEACCESS = String.Empty;
        //    string CATEGORIAACCESS = String.Empty;
        //    string RENDITAACCESS = string.Empty;
        //    decimal DecRenditaAccess = 0;

        //    string DATAEFFICACIAACCESS = String.Empty;
        //    string DATAEFFICACIA1ACCESS = String.Empty;
        //    string DATA_REG_ATTIACCESS = String.Empty;
        //    string DATA_REG_ATTI1ACCESS = String.Empty;
        //    string VALOREACCESS = string.Empty;

        //    string FOGLIO = String.Empty;
        //    string NUMERO = String.Empty;
        //    string SUBALTERNO = String.Empty;
        //    string FOGLIOC = String.Empty;
        //    string NUMEROC = String.Empty;
        //    string SUBALTERNOC = String.Empty;

        //    string BELFIORE = String.Empty;

        //    int ANNOIMMOBILE;

        //    string ConfrontoACCESS = string.Empty;
        //    string ConfrontoSQL = string.Empty;

        //    BELFIORE = Session["COD_BELFIORE"].ToString();

        //    string Giorno = string.Empty;
        //    string Mese = string.Empty;
        //    string Anno = string.Empty;
        //    string result = string.Empty;

        //    string sqlEliminaTabella = String.Empty;
        //    string nomeTabellaAnagrafica = String.Empty;
        //    string sqlCreaTabella = String.Empty;

        //    oCMD = new SqlCommand();
        //    try
        //    {
        //        //APERTURA CONNESSIONE A OPENGOVICI
        //        //LEGGO LA STRINGA di connessione da web config 
        //        //apro la connessione
        //        oConn.ConnectionString = sqlServer2000;
        //        oConn.Open();

        //        oConnCatasto.ConnectionString = sqlAccess2000;
        //        oConnCatasto.Open();

        //        log.Debug("Aperto connessione Access - confronto classe catasto");
        //        /********** CREAZIONE E POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/
        //        if (CreaTabSoggetti(oConn, oConnCatasto, BELFIORE) == false)
        //        {
        //            throw new Exception("Errore in popolamento tabella soggetti.");
        //        }
        //        /********** FINE POPOLAMENTO DELLA TABELLA TOTALE_SOGGETTI CON TUTTI I SOGGETTI FISICI E GIURIDICI DEL COMUNE SELEZIONATO *************/

        //        string sqlDelete = "DELETE FROM TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO where ENTE = '" + ConstWrapper.CodiceEnte + "' AND ANNO='" + annoConfronto + "'";
        //        sqlDelete = sqlDelete + "; DELETE FROM TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO_PROPRIETARI where ENTE = '" + ConstWrapper.CodiceEnte + "' AND ANNO='" + annoConfronto + "'";

        //        log.Debug("Cancello contenuto tabella TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO - confronto classe catasto");

        //        oCMD = new SqlCommand();
        //        oCMD.CommandText = sqlDelete;
        //        oCMD.Connection = oConn;

        //        oCMD.ExecuteNonQuery();
        //        oCMD.Dispose();

        //        connectionServer2000 = "";

        //        connectionServer2000 = " SELECT DISTINCT TOP 10 FOGLIO, NUMERO, SUBALTERNO, CODCATEGORIACATASTALE, CODCLASSE, DATAINIZIO, VALOREIMMOBILE";
        //        connectionServer2000 = connectionServer2000 + " FROM TBLOGGETTI ";
        //        connectionServer2000 = connectionServer2000 + " WHERE (YEAR(DATAINIZIO) <= " + annoConfronto + ")" + " AND (YEAR(DATAFINE) >= " + annoConfronto + ")" + " AND ENTE = '" + ConstWrapper.CodiceEnte + "'";
        //        connectionServer2000 = connectionServer2000 + " AND (CARATTERISTICA=3)";
        //        //*** 20120704 - IMU ***
        //        log.Debug("Query in TblOggetti per elenco immobili ICI/IMU - confronto classe catasto");

        //        //filtrare per caratteristica  3
        //        //estrarre anno dal
        //        //se >=1997 moltiplicare per 1.05

        //        oCMD = new SqlCommand();
        //        oCMD.CommandText = connectionServer2000;
        //        oCMD.Connection = oConn;

        //        dr = oCMD.ExecuteReader();

        //        RIVALUTAZIONE = (decimal)1.05;
        //        bool bRetVal;

        //        log.Debug("Inizio ciclo - confronto classe catasto");

        //        while (dr.Read())
        //        {
        //            //*** 20120704 - IMU ***
        //            log.Debug("Inizio ciclo dati verticale ICI/IMU");

        //            CATEGORIA = dr["CodCategoriaCatastale"].ToString();

        //            if (CATEGORIA.Length >= 4)
        //            {
        //                CATEGORIA = dr["CodCategoriaCatastale"].ToString().Replace("/", "");
        //            }
        //            else
        //            {
        //                CATEGORIA = dr["CodCategoriaCatastale"].ToString().Replace("/", "0");
        //            }

        //            CLASSE = dr["CodClasse"].ToString();

        //            try
        //            {
        //                CLASSE = (int.Parse(CLASSE)).ToString().PadLeft(2, '0');
        //            }
        //            catch (FormatException)
        //            {
        //                //CLASSE = CLASSE;
        //            }

        //            //DB OPENGOVICI
        //            FOGLIO = dr["FOGLIO"].ToString().PadLeft(4, '0');
        //            NUMERO = dr["NUMERO"].ToString().PadLeft(5, '0');
        //            if (dr["SUBALTERNO"].ToString().CompareTo("-1") != 0)
        //            {
        //                SUBALTERNO = dr["SUBALTERNO"].ToString().PadLeft(4, '0');
        //            }
        //            else
        //            {
        //                SUBALTERNO = "";
        //            }

        //            ANNOIMMOBILE = DateTime.Parse(dr["DataInizio"].ToString()).Year;

        //            VALORE = decimal.Parse(dr["ValoreImmobile"].ToString());

        //            if (ANNOIMMOBILE < 1997)
        //            {
        //                VALORE = (VALORE * RIVALUTAZIONE);
        //            }

        //            connectionAccess2000 = "SELECT FAB.FOGLIO, FAB.NUMERO, ISNUMERIC(FAB.NUMERO) AS MAPPALE_ALFANUMERICO, FAB.SUBALTERNO, UI.CATEGORIA, UI.CLASSE, FAB.COD_AMM_COMUNE, UI.RENDITA_EURO, UI.DATA_EFFICACIA, UI.DATA_REG_ATTI, UI.DATA_EFFICACIA1, UI.DATA_REG_ATTI1, FAB.ID_IMMOBILE";
        //            connectionAccess2000 += " FROM FAB_IDENTIFICATIVI FAB";
        //            connectionAccess2000 += " INNER JOIN FAB_UNITA_IMMOBILIARE UI ON FAB.COD_AMM_COMUNE = UI.COD_AMM_COMUNE AND FAB.SEZIONE = UI.SEZIONE AND FAB.ID_IMMOBILE = UI.ID_IMMOBILE AND FAB.TIPO_IMMOBILE = UI.TIPO_IMMOBILE";
        //            connectionAccess2000 += " WHERE 1=1";
        //            connectionAccess2000 += " AND (ISNUMERIC(FAB.NUMERO) <> 0)";
        //            connectionAccess2000 += " AND (DATA_EFFICACIA1 IS NULL OR DATA_EFFICACIA1='')";
        //            connectionAccess2000 += " AND (DATA_REG_ATTI1 IS NULL OR DATA_REG_ATTI1='')";
        //            //connectionAccess2000+=" AND (CATEGORIA NOT LIKE 'E%' AND CATEGORIA NOT LIKE 'F%' AND CATEGORIA NOT LIKE 'D10%')";
        //            connectionAccess2000 += " AND (CATEGORIA NOT LIKE 'E%' AND CATEGORIA NOT LIKE 'F01%' AND CATEGORIA NOT LIKE 'F05%')";
        //            connectionAccess2000 += " AND (FOGLIO ='" + FOGLIO + "' AND NUMERO='" + NUMERO + "' AND SUBALTERNO='" + SUBALTERNO + "')";
        //            connectionAccess2000 += " AND (FAB.COD_AMM_COMUNE='" + BELFIORE + "')";
        //            dvCatasto = MyFnc.getDataView(oConnCatasto, connectionAccess2000);
        //            for (int x = 0; x < dvCatasto.Count; x++)
        //            {
        //                ID_IMMOBILE = dvCatasto.Table.Rows[x]["ID_IMMOBILE"].ToString();

        //                CLASSEACCESS = dvCatasto.Table.Rows[x]["CLASSE"].ToString();
        //                CLASSEACCESS = CLASSEACCESS.Replace("0", "");
        //                CATEGORIAACCESS = dvCatasto.Table.Rows[x]["CATEGORIA"].ToString();

        //                FOGLIOC = dvCatasto.Table.Rows[x]["FOGLIO"].ToString();
        //                NUMEROC = dvCatasto.Table.Rows[x]["NUMERO"].ToString();
        //                SUBALTERNOC = dvCatasto.Table.Rows[x]["SUBALTERNO"].ToString();

        //                RENDITAACCESS = dvCatasto.Table.Rows[x]["RENDITA_EURO"].ToString();
        //                DecRenditaAccess = decimal.Parse(RENDITAACCESS.ToString());

        //                DATAEFFICACIAACCESS = dvCatasto.Table.Rows[x]["DATA_EFFICACIA"].ToString();
        //                if (DATAEFFICACIAACCESS.Length > 0)
        //                {
        //                    Giorno = DATAEFFICACIAACCESS.Substring(0, 2);
        //                    Mese = DATAEFFICACIAACCESS.Substring(2, 2);
        //                    Anno = DATAEFFICACIAACCESS.Substring(4, 4);
        //                    result = Giorno + "/" + Mese + "/" + Anno;

        //                    DATAEFFICACIAACCESS = result;//this.FormattaDataItaliano(DATAEFFICACIAACCESS);
        //                }

        //                DATAEFFICACIA1ACCESS = dvCatasto.Table.Rows[x]["DATA_EFFICACIA1"].ToString();
        //                if (DATAEFFICACIA1ACCESS.Length > 0)
        //                {
        //                    Giorno = DATAEFFICACIA1ACCESS.Substring(0, 2);
        //                    Mese = DATAEFFICACIA1ACCESS.Substring(2, 2);
        //                    Anno = DATAEFFICACIA1ACCESS.Substring(4, 4);
        //                    result = Giorno + "/" + Mese + "/" + Anno;
        //                    DATAEFFICACIA1ACCESS = result;//DateTime.Parse(DATAEFFICACIA1ACCESS).ToString("dd/MM/yyyy");
        //                }

        //                DATA_REG_ATTIACCESS = dvCatasto.Table.Rows[x]["DATA_REG_ATTI"].ToString();
        //                if (DATA_REG_ATTIACCESS.Length > 0)
        //                {
        //                    Giorno = DATA_REG_ATTIACCESS.Substring(0, 2);
        //                    Mese = DATA_REG_ATTIACCESS.Substring(2, 2);
        //                    Anno = DATA_REG_ATTIACCESS.Substring(4, 4);
        //                    result = Giorno + "/" + Mese + "/" + Anno;
        //                    DATA_REG_ATTIACCESS = result;//DateTime.Parse(DATA_REG_ATTIACCESS).ToString("dd/MM/yyyy");
        //                }

        //                DATA_REG_ATTI1ACCESS = dvCatasto.Table.Rows[x]["DATA_REG_ATTI1"].ToString();
        //                if (DATA_REG_ATTI1ACCESS.Length > 0)
        //                {
        //                    Giorno = DATA_REG_ATTI1ACCESS.Substring(0, 2);
        //                    Mese = DATA_REG_ATTI1ACCESS.Substring(2, 2);
        //                    Anno = DATA_REG_ATTI1ACCESS.Substring(4, 4);
        //                    result = Giorno + "/" + Mese + "/" + Anno;
        //                    DATA_REG_ATTI1ACCESS = result;//DateTime.Parse(DATA_REG_ATTI1ACCESS).ToString("dd/MM/yyyy");
        //                }

        //                VALOREACCESS = GetValore(DecRenditaAccess, CATEGORIAACCESS);

        //                VALOREACCESS = VALOREACCESS.Replace(".", "");

        //                ConfrontoSQL = CLASSE.Trim().ToUpper() + CATEGORIA.Trim().ToUpper() + VALORE;
        //                ConfrontoACCESS = CLASSEACCESS.Trim().ToUpper() + CATEGORIAACCESS.Trim().ToUpper() + VALOREACCESS;

        //                if (ConfrontoSQL.CompareTo(ConfrontoACCESS) != 0)
        //                {
        //                    //INSERISCO NELLA TABELLA PIATTA TB_CONFRONTO_CONCATASTO
        //                    //CAMPI CONTRIBUENTE CLASSEACCESS, CATEGORIAACCESS, CLASSE, CATEGORIA, FOGLIO NUMERO SUBALTERNO ESISTENTE
        //                    connectionServer2000 = "INSERT INTO TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO (ENTE,ANNO,ID_IMMOBILE,FOGLIOC,NUMEROC,SUBALTERNOC,FOGLIOICI,NUMEROICI,SUBALTERNOICI, CLASSE_CATASTO, CATEGORIA_CATASTO, ";
        //                    connectionServer2000 = connectionServer2000 + " RENDITA_EURO,VALORE_ACCESS, VALOREICI, GENERAZIONE_DATA_VALIDITA,CONCLUSIONE_DATA_VALIDITA,GENERAZIONE_DATA_REGISTRAZIONE,CONCLUSIONE_DATA_REGISTRAZIONE, CLASSEICI, CATEGORIAICI) ";
        //                    connectionServer2000 = connectionServer2000 + " VALUES('" + ConstWrapper.CodiceEnte + "'," +

        //                        "'" + annoConfronto + "'," +
        //                        "'" + ID_IMMOBILE + "'," +
        //                        "'" + FOGLIOC + "'," +
        //                        "'" + NUMEROC + "'," +
        //                        "'" + SUBALTERNOC + "'," +
        //                        "'" + FOGLIO + "'," +
        //                        "'" + NUMERO + "'," +
        //                        "'" + SUBALTERNO + "'," +
        //                        "'" + CLASSEACCESS.Replace("0", "") + "'," +
        //                        "'" + CATEGORIAACCESS.ToString().Replace("0", "/") + "'," +

        //                        "'" + RENDITAACCESS + "'," +
        //                        "'" + VALOREACCESS + "'," +
        //                        "'" + VALORE + "'," +
        //                        "'" + DATAEFFICACIAACCESS + "'," +
        //                        "'" + DATAEFFICACIA1ACCESS + "'," +
        //                        "'" + DATA_REG_ATTIACCESS + "'," +
        //                        "'" + DATA_REG_ATTI1ACCESS + "'," +
        //                        "'" + CLASSE.ToString().Replace("0", "") + "'," +
        //                        "'" + CATEGORIA.ToString().Replace("0", "/") + "')";

        //                    oCMD.Dispose();
        //                    oCMD = new SqlCommand();

        //                    SqlConnection oConnDappoggio = new SqlConnection();
        //                    oConnDappoggio.ConnectionString = sqlServer2000;
        //                    oConnDappoggio.Open();
        //                    oCMD.CommandText = connectionServer2000;
        //                    oCMD.Connection = oConnDappoggio;
        //                    oCMD.ExecuteNonQuery();
        //                    oConnDappoggio.Close();

        //                    log.Debug("Inserimento anagrafiche in tabella TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO - confronto classe catasto");
        //                }
        //            }
        //            dvCatasto.Dispose();
        //            //					oCMDCatasto.Dispose ();	
        //        }
        //        dr.Close();
        //        oCMD.Dispose();
        //        //inserisco i proprietari
        //        ID_IMMOBILE = "";
        //        bRetVal = this.SetProprietari(oConn, oConnCatasto, ID_IMMOBILE, BELFIORE, annoConfronto, "C");
        //    }
        //    catch (SqlException ex)
        //    {
        //         log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnClasseCatastato_Click.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //        if (ex.Number == -2147217900)
        //        {
        //            log.Error("Problema elaborazione tabella Totale_Soggetti procedura confronto Classe Catastato ", ex);
        //            //*** 20120704 - IMU ***
        //            strscript = strscript + " alert('Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel del confronto ICI/IMU - Catasto.');";
        //            RegisterScript(sScript,this.GetType());,"", "" + strscript + "");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //string strscript=string.Empty;
        //        //strscript = "parent.Visualizza.attesaCarica.style.display='none';" ;
        //               log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.btnClasseCatastato_Click.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //        //*** 20120704 - IMU ***
        //        strscript = strscript + " alert('Si sono verificati dei problemi durante l\\'elaborazione e l\\'estrazione del file Excel del confronto ICI/IMU - Catasto.');";
        //        RegisterScript(sScript,this.GetType());,"", "" + strscript + "");
        //        return;
        //    }
        //    finally
        //    {
        //        if (nomeTabellaAnagrafica != String.Empty)
        //        {
        //            sqlEliminaTabella = "drop table " + nomeTabellaAnagrafica;
        //            oCMD = new SqlCommand();
        //            oCMD.CommandText = sqlEliminaTabella;
        //            oCMD.Connection = oConn;
        //            oCMD.ExecuteNonQuery();
        //            log.Debug("Elimino tabella Totale_soggetti - confronto classe catasto");
        //        }
        //        oConnCatasto.Close();
        //        oConn.Close();
        //        oCMD.Dispose();
        //    }

        //    stampaExcelCONFRONTO_CLASSE_CATEGORIA_CONCATASTO(annoConfronto);
        //}

        //private void stampaExcelCONFRONTO_CLASSE_CATEGORIA_CONCATASTO(string ANNO)
        //{

        //    DataRow dr;
        //    DataSet ds;
        //    DataTable dtConfrontoCatClasRen;
        //    string sPathProspetti = string.Empty;
        //    string NameXLS = string.Empty;


        //    /*****************************************************
        //    prelevo i dati da stampare
        //    *****************************************************/

        //    string sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();
        //    sqlServer2000 = ConfigurationManager.AppSettings["sqlServerCatasto"].ToString();

        //    SqlConnection oConn = new SqlConnection();
        //    SqlCommand oCMD;

        //    oConn.ConnectionString = sqlServer2000;
        //    oConn.Open();

        //    string sql = "select * from TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO";
        //    sql = sql + " where ente='" + ConstWrapper.CodiceEnte + "' AND ANNO='" + ANNO + "'";
        //    sql = sql + " ORDER BY CAST(FOGLIOC AS integer), CAST(NUMEROC AS integer), CAST(SUBALTERNOC AS integer)";

        //    oCMD = new SqlCommand();
        //    oCMD.CommandText = sql;
        //    oCMD.Connection = oConn;

        //    log.Error("Query in tabella TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO per stampa excel - confronto classe catasto");

        //    SqlDataAdapter da = new SqlDataAdapter(oCMD);
        //    DataSet ds_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO = new DataSet();
        //    da.Fill(ds_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO);

        //    /*****************************************************/


        //    ArrayList arratlistNomiColonne;
        //    string[] arraystr;
        //    string nomeComune = "";
        //    nomeComune = ConstWrapper.DescrizioneEnte;

        //    arratlistNomiColonne = new ArrayList();

        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");
        //    arratlistNomiColonne.Add("");

        //    arraystr = (string[])arratlistNomiColonne.ToArray(typeof(string));

        //    sPathProspetti = System.Configuration.ConfigurationManager.AppSettings["PATH_PROSPETTI_EXCEL"].ToString();
        //    NameXLS = nomeComune + "_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xls";


        //    ds = CreateDataSetCONFRONTO_CLASSE_CATEGORIA_CONCATASTO();

        //    dtConfrontoCatClasRen = ds.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"];
        //    //ds_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO .Tables[0];

        //    //inserisco intestazione - titolo prospetto + data stampa
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dr[0] = ConstWrapper.DescrizioneEnte;
        //    dtConfrontoCatClasRen.Rows.Add(dr);

        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dtConfrontoCatClasRen.Rows.Add(dr);

        //    //*** 20120704 - IMU ***
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dr[0] = "Prospetto Confronto ICI/IMU - Catasto per Categoria, Classe e Rendita";
        //    dr[17] = "Data Stampa " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
        //    dtConfrontoCatClasRen.Rows.Add(dr);

        //    //inserisco riga vuota
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dtConfrontoCatClasRen.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dtConfrontoCatClasRen.Rows.Add(dr);

        //    //inserisco intestazione di colonna
        //    //*** 20120704 - IMU ***
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dr[0] = "Foglio Vert. ICI/IMU";
        //    dr[1] = "Numero Vert. ICI/IMU";
        //    dr[2] = "Subalterno Vert. ICI/IMU";
        //    dr[3] = "Categoria Vert. ICI/IMU";
        //    dr[4] = "Classe Vert. ICI/IMU";
        //    dr[5] = "Valore Vert. ICI/IMU Euro";

        //    dr[6] = "Foglio Cat.";
        //    dr[7] = "Numero Cat.";
        //    dr[8] = "Subalterno Cat.";
        //    dr[9] = "Categoria Cat.";
        //    dr[10] = "Classe Cat.";
        //    dr[11] = "Rendita Cat. Euro";
        //    dr[12] = "Valore Cat. Euro";

        //    dr[13] = "Generazione Data Validita";
        //    dr[14] = "Conclusione Data Validita";
        //    dr[15] = "Generazione Data Registrazione";
        //    dr[16] = "Conclusione Data Registrazione";
        //    dr[17] = "Elenco Proprietari";

        //    dtConfrontoCatClasRen.Rows.Add(dr);
        //    DataView dvCONFRONTO = new DataView();
        //    dvCONFRONTO = ds_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO.Tables[0].DefaultView;

        //    string ID_IMMOBILE = string.Empty;
        //    SqlDataReader drSQL;
        //    string strProprietari;

        //    log.Error("Inizio ciclo dati per stampa - confronto Classe catasto");
        //try{
        //    for (int i = 0; i < dvCONFRONTO.Count; i++)
        //    {
        //        dr = dtConfrontoCatClasRen.NewRow();
        //        dr[0] = dvCONFRONTO[i].Row["FOGLIOICI"].ToString();
        //        dr[1] = dvCONFRONTO[i].Row["NUMEROICI"].ToString();
        //        dr[2] = dvCONFRONTO[i].Row["SUBALTERNOICI"].ToString();
        //        dr[3] = dvCONFRONTO[i].Row["CATEGORIAICI"].ToString();
        //        dr[4] = dvCONFRONTO[i].Row["CLASSEICI"].ToString();
        //        //dr[5] = " " + dvCONFRONTO[i].Row["VALOREICI"].ToString().Replace(",",".") ;
        //        dr[5] = " " + DecimaliForStampa(dvCONFRONTO[i].Row["VALOREICI"].ToString());

        //        dr[6] = dvCONFRONTO[i].Row["FOGLIOC"].ToString();
        //        dr[7] = dvCONFRONTO[i].Row["NUMEROC"].ToString();
        //        dr[8] = dvCONFRONTO[i].Row["SUBALTERNOC"].ToString();

        //        dr[9] = dvCONFRONTO[i].Row["CATEGORIA_CATASTO"].ToString();
        //        dr[10] = dvCONFRONTO[i].Row["CLASSE_CATASTO"].ToString();
        //        //dr[11] = " " + dvCONFRONTO[i].Row["RENDITA_EURO"].ToString().Replace(",",".") ;
        //        //dr[12] = " " + dvCONFRONTO[i].Row["VALORE_ACCESS"].ToString().Replace(",",".") ;
        //        dr[11] = " " + DecimaliForStampa(dvCONFRONTO[i].Row["RENDITA_EURO"].ToString());
        //        dr[12] = " " + DecimaliForStampa(dvCONFRONTO[i].Row["VALORE_ACCESS"].ToString());

        //        dr[13] = " " + dvCONFRONTO[i].Row["GENERAZIONE_DATA_VALIDITA"].ToString();
        //        dr[14] = " " + dvCONFRONTO[i].Row["CONCLUSIONE_DATA_VALIDITA"].ToString();
        //        dr[15] = " " + dvCONFRONTO[i].Row["GENERAZIONE_DATA_REGISTRAZIONE"].ToString();
        //        dr[16] = " " + dvCONFRONTO[i].Row["CONCLUSIONE_DATA_REGISTRAZIONE"].ToString();

        //        //				dtConfrontoCatClasRen.Rows.Add(dr);

        //        ID_IMMOBILE = dvCONFRONTO[i].Row["ID_IMMOBILE"].ToString();
        //        strProprietari = "";

        //        sql = "select * from TB_CONFRONTO_CLASSE_CATEGORIA_CONCATASTO_PROPRIETARI";
        //        sql = sql + " where ente='" + ConstWrapper.CodiceEnte + "' AND ID_IMMOBILE='" + ID_IMMOBILE + "' AND ANNO='" + ANNO + "'";

        //        oCMD = new SqlCommand();
        //        oCMD.CommandText = sql;
        //        oCMD.Connection = oConn;

        //        drSQL = oCMD.ExecuteReader();

        //        while (drSQL.Read())
        //        {
        //            if (strProprietari.CompareTo("") == 0)
        //            {
        //                strProprietari = strProprietari + drSQL["COGNOME"].ToString() + " " + drSQL["NOME"].ToString();
        //            }
        //            else
        //            {
        //                strProprietari = strProprietari + "," + drSQL["COGNOME"].ToString() + " " + drSQL["NOME"].ToString();
        //            }

        //        }

        //        drSQL.Close();
        //        oCMD.Dispose();

        //        dr[17] = strProprietari;
        //        dtConfrontoCatClasRen.Rows.Add(dr);
        //    }
        //    //inserisco riga vuota
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dtConfrontoCatClasRen.Rows.Add(dr);
        //    //inserisco riga vuota
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dtConfrontoCatClasRen.Rows.Add(dr);

        //    //inserisco numero totali di contribuenti
        //    dr = dtConfrontoCatClasRen.NewRow();
        //    dr[0] = "Totale Record nel FIle: " + (dvCONFRONTO.Count);
        //    dtConfrontoCatClasRen.Rows.Add(dr);

        //    log.Error("Fase di stampa - confronto classe catasto");

        //    //definisco l'insieme delle colonne da esportare
        //    int[] iColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        //    //esporto i dati in excel
        //    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
        //    objExport.ExportDetails(dtConfrontoCatClasRen, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS);
        //}
        //Catch(Exception Err){
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.stampaExcelCONFRONTO_CLASSE_CATEGORIA_CONCATASTO.errore: ", Err);
        //    Response.Redirect("../../PaginaErrore.aspx");
        //}
        //}

        //private DataSet CreateDataSetCONFRONTO_CLASSE_CATEGORIA_CONCATASTO()
        //{
        //    DataSet dsTmp = new DataSet();

        //    dsTmp.Tables.Add("CONFRONTO_CLASSE_CATEGORIA_CONCATASTO");
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);
        //    dsTmp.Tables["CONFRONTO_CLASSE_CATEGORIA_CONCATASTO"].Columns.Add("").DataType = typeof(string);

        //    return dsTmp;
        //}
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iInput"></param>
        /// <returns></returns>
        protected string DecimaliForStampa(object iInput)
        {
            string ret = string.Empty;
            try
            {
                if ((iInput.ToString() == "-1") || (iInput.ToString() == "-1,00") || (iInput.ToString() == ""))
                {
                    ret = string.Empty;
                }
                else
                {
                    ret = Convert.ToDecimal(iInput).ToString("N");
                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.DecimaliForStampa.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
            }
            return ret;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RENDITAACCESS"></param>
        /// <param name="CATEGORIAACCESS"></param>
        /// <returns></returns>
        private string GetValore(decimal RENDITAACCESS, string CATEGORIAACCESS)
        {

            decimal RIVALUTAZIONE = (decimal)1.05;
            try
            {
                if (RENDITAACCESS == 0)
                {
                    return "0";
                }
                else
                {
                    decimal Percentuale = 0;
                    decimal ValoreCalcolato = 0;

                    if (CATEGORIAACCESS == "C01")
                    {
                        Percentuale = 34;

                    }
                    else if (CATEGORIAACCESS == "A10")
                    {
                        Percentuale = 50;
                    }
                    else if (CATEGORIAACCESS == "D01" | CATEGORIAACCESS == "D02" | CATEGORIAACCESS == "D03" | CATEGORIAACCESS == "D04" | CATEGORIAACCESS == "D05" | CATEGORIAACCESS == "D06" | CATEGORIAACCESS == "D07" | CATEGORIAACCESS == "D08" | CATEGORIAACCESS == "D09" | CATEGORIAACCESS == "D010" | CATEGORIAACCESS == "D012")
                    {
                        Percentuale = 50;
                    }
                    else
                    {
                        Percentuale = 100;
                    }

                    ValoreCalcolato = RENDITAACCESS * Percentuale * RIVALUTAZIONE;

                    return ValoreCalcolato.ToString("N");

                }
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ConfrontaConCatasto.GetValore.errore: ", Err);
                Response.Redirect("../../PaginaErrore.aspx");
                throw Err;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}

