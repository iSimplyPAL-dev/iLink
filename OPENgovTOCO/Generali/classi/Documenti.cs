using System;
using RIBESElaborazioneDocumentiInterface;
using System.Configuration;
using log4net;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace OPENgovTOCO.Generali.classi
{
    /// <summary>
    /// Classe per l'elaborazione dei documenti
    /// </summary>
    public class Documenti
    {
        private RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] _StampaDocumenti;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Documenti));
        //
        // TODO: Add constructor logic here
        //
        //*** 20150903 - stampa unica ***
        //        public int ElaboraDocumenti(int sTipoElab, int nMaxDocPerFile, string sTipoordinamento, int IdFRuolo, string[] arrayCodiciCartella, bool bElaboraBollettini,bool bCreaPDF,string TipoBollettino) 
        //        {
        ////				RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] retStampaDocumenti;

        //            try 
        //            {
        ////					// ********************************************************************
        ////					// controllo che ci siano per l'elaborazione effettiva ancora dei doc da elaborare e
        ////					// quindi vanno elaborati solo quelli.
        ////					// ********************************************************************
        ////					Session.Remove("ELENCO_DOCUMENTI_STAMPATI");
        ////					// **************************************************************
        ////					// devo risalire all'ultimo file usato per l'elaborazione effettiva in corso
        ////					// **************************************************************
        ////					nNumFileDaElaborare = clsElabDoc.GetNumFileDocDaElaborare(IdFRuolo);
        ////					if ((nNumFileDaElaborare != -1)) 
        ////					{
        ////						nNumFileDaElaborare++;
        ////					}
        ////					if ((ControlloElementiSelezionati() > 0)) 
        ////					{
        ////						oArrayOggettoCartelle = ((OggettoCartella[])(Session("ListCartelle")));
        ////						for (x = 0; (x <= oArrayOggettoCartelle.GetUpperBound(0)); x++) 
        ////						{
        ////							if ((oArrayOggettoCartelle[x].Selezionato == true)) 
        ////							{
        ////								object Preserve;
        ////								arrayCodiciCartella[nIndiceArrayCartelleDaElaborare];
        ////								arrayCodiciCartella[nIndiceArrayCartelleDaElaborare] = oArrayOggettoCartelle[x].CodiceCartella;
        ////								nIndiceArrayCartelleDaElaborare++;
        ////							}
        ////						}
        ////					}
        ////					else if ((sTipoElab == 1)) 
        ////					{
        ////						strscript = "<script language=\'javascript\'>alert(\'Non sono stati selezionati documenti da elaborare!\');</script>";
        ////						ClientScript.RegisterStartupScript(this.GetType(), "msg", strscript);
        ////						// TODO: Exit Function: Warning!!! Need to return the value
        ////						return;
        ////					}
        ////					else 
        ////					{
        ////						// **************************************************************
        ////						// se è già stata fatta un'elaborazione parziale devo solo elaborare le cartelle rimanenti
        ////						// **************************************************************
        ////						if (((Session("nDocDaElaborare") > 0) 
        ////							&& (Session("DocElaborati") > 0))) 
        ////						{
        ////							oArrayDocDaElaborare = clsElabDoc.GetDocDaElaborare(IdFRuolo);
        ////							if ((oArrayDocDaElaborare.Length > 0)) 
        ////							{
        ////								for (x = 0; (x 
        ////									<= (oArrayDocDaElaborare.Length - 1)); x++) 
        ////								{
        ////									object Preserve;
        ////									arrayCodiciCartella[nIndiceArrayCartelleDaElaborare];
        ////									arrayCodiciCartella[nIndiceArrayCartelleDaElaborare] = oArrayDocDaElaborare(x).CodiceCartella;
        ////									nIndiceArrayCartelleDaElaborare++;
        ////								}
        ////							}
        ////						}
        ////					}
        ////					if ((Session("OrdinamentoDoc") == 0)) 
        ////					{
        ////						sTipoordinamento = "Indirizzo";
        ////					}
        ////					else 
        ////					{
        ////						sTipoordinamento = "Nominativo";
        ////					}
        //                Log.Debug("ElaboraDocumenti::inizio");
        //                //  recupero i dati per la chiamata al servizio di elaborazione delle stampe
        //                string strConnessioneOSAP = ConfigurationManager.AppSettings["connectionStringOpenGovOSAP"].ToString();
        //                string strConnessioneRepository = ConfigurationManager.AppSettings["connectionStringSQLOPENgov"].ToString();
        //                string strConnessioneAnagrafica = ConfigurationManager.AppSettings["connectionStringSQLOPENAnagrafica"].ToString();
        //                //Log.Debug("ElaboraDocumenti::prelevato stringhe connessione");

        ////					if ((sTipoElab == 1)) 
        ////					{
        ////						//  elaborazione di prova
        ////						// '' CHIAMO IL SERVIZIO DI ELABORAZIONE DEI DOCUMENTI.
        //                    ElaborazioneDatiStampeInterface.IElaborazioneStampeOSAP oElaborazioneDati;
        //                    oElaborazioneDati = (ElaborazioneDatiStampeInterface.IElaborazioneStampeOSAP)Activator.GetObject(typeof(ElaborazioneDatiStampeInterface.IElaborazioneStampeOSAP), ConfigurationManager.AppSettings["URLElaborazioneDatiStampeOSAP"].ToString());
        //                    oElaborazioneDati = (ElaborazioneDatiStampeInterface.IElaborazioneStampeOSAP)Activator.GetObject(typeof(ElaborazioneDatiStampeInterface.IElaborazioneStampeOSAP), ConfigurationManager.AppSettings["URLElaborazioneDatiStampeOSAP"].ToString());
        //                    if (oElaborazioneDati == null)
        //                    {
        //                        Log.Debug("oElaborazioniDati è nullo");
        //                    }
        //                    _StampaDocumenti = oElaborazioneDati.ElaborazioneMassivaDocumentiOSAP(strConnessioneOSAP, strConnessioneAnagrafica, strConnessioneRepository,DichiarazioneSession.IdEnte, sTipoElab, nMaxDocPerFile, sTipoordinamento, IdFRuolo,  arrayCodiciCartella, bElaboraBollettini,bCreaPDF,TipoBollettino);
        //                    if (_StampaDocumenti == null)
        //                    {
        //                        Log.Debug("_StampaDocumenti è nullo; numero cartelle: ");
        //                    }
        //                //					}
        ////					else 
        ////					{
        ////						//  elaborazione effettiva
        ////						StampaMassivaTarsuAsync del = new StampaMassivaTarsuAsync(new System.EventHandler(this.ChiamaElaborazioneAsincrona));
        ////						del.BeginInvoke(strConnessioneTarsu, strConnessioneAnagrafica, strConnessioneRepository, HttpContext.Current.Session("COD_ENTE"), sTipoElab, nMaxDocPerFile, sTipoordinamento, IdFRuolo, TipoRuolo, arrayCodiciCartella, Session("ElaboraBollettini"), null, null);
        ////						return 2;
        ////					}
        ////					if (!(retStampaDocumenti == null)) 
        ////					{
        ////						Session.Add("ELENCO_DOCUMENTI_STAMPATI", retStampaDocumenti);
        ////					}
        //                return 1;
        //            }
        //            catch (Exception ex) 
        //            { Log.Debug(DichiarazioneSession.IdEnte +" - OPENgovOSAP.Documenti.ElaboraDocumenti.errore: ", ex );
        //               
        ////					strscript = "<script language=\'javascript\'>alert(\'Si è verificato un errore!\');</script>";
        //                return 0;
        //            }
        //        }
        /**** 201810 - Calcolo puntuale ****/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBType"></param>
        /// <param name="CodTributo"></param>
        /// <param name="arrayCodiciCartella"></param>
        /// <param name="Anno"></param>
        /// <param name="sIdEnte"></param>
        /// <param name="IdRuolo"></param>
        /// <param name="StringConnection"></param>
        /// <param name="StringConnectionOPENgov"></param>
        /// <param name="StringConnectionAnagrafica"></param>
        /// <param name="PathTemplate"></param>
        /// <param name="PathVirtualTemplate"></param>
        /// <param name="nMaxDocPerFile"></param>
        /// <param name="sTipoElab"></param>
        /// <param name="ImpostazioneBollettini"></param>
        /// <param name="TipoBollettino"></param>
        /// <param name="bElabBollettini"></param>
        /// <param name="bCreaPDF"></param>
        /// <param name="bSendByMail"></param>
        /// <param name="IsSoloBollettino"></param>
        /// <param name="TemplateFile"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="05/11/2020">
        /// devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
        /// </revision>
        /// </revisionHistory>
        public int ElaboraDocumenti(string DBType, string CodTributo, IRemInterfaceOSAP.Cartella[] arrayCodiciCartella, int Anno, string sIdEnte, int IdRuolo, string StringConnection, string StringConnectionOPENgov, string StringConnectionAnagrafica, string PathTemplate, string PathVirtualTemplate, int nMaxDocPerFile, string sTipoElab, string ImpostazioneBollettini, string TipoBollettino, bool bElabBollettini, bool bCreaPDF, bool bSendByMail, bool IsSoloBollettino,string TemplateFile)
        {
            try
            {
                Log.Debug("ElaboraDocumenti::inizio");
                ElaborazioneDatiStampeInterface.IElaborazioneStampeICI oElaborazioneDati = (ElaborazioneDatiStampeInterface.IElaborazioneStampeICI)Activator.GetObject(typeof(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), DichiarazioneSession.UrlServizioStampeICI);
                string[] Esclusione = null;
                string TipoCalcolo = "OSAP";
                int nDecimal = 2;
                int[,] IdDocToElab = GetArrayContribuenti(arrayCodiciCartella);

                if (DichiarazioneSession.CodTributo("") == Utility.Costanti.TRIBUTO_SCUOLE)
                    TipoCalcolo = "SCUOLE";
                if (oElaborazioneDati == null)
                    Log.Debug("oElaborazioniDati è nullo");
                Log.Debug("ElaboraDocumenti.richiamo servizio percorso=" + DichiarazioneSession.UrlServizioStampeICI);
                Log.Debug("parametri.CodTributo=" + CodTributo.ToString());
                Log.Debug(", IdDocToElab=" + IdDocToElab.ToString());
                Log.Debug(", Anno=" + Anno.ToString());
                Log.Debug(", sIdEnte=" + sIdEnte.ToString());
                Log.Debug(", IdRuolo=" + IdRuolo.ToString());
                Log.Debug(", Esclusione=" + (Esclusione == null ? "" : Esclusione.ToString()));
                Log.Debug(", StringConnection=" + StringConnection.ToString());
                Log.Debug(", StringConnectionOPENgov=" + StringConnectionOPENgov.ToString());
                Log.Debug(", StringConnectionAnagrafica=" + StringConnectionAnagrafica.ToString());
                Log.Debug(", PathTemplate=" + PathTemplate.ToString());
                Log.Debug(",PathVirtualTemplate=" + PathVirtualTemplate.ToString());
                Log.Debug(", nMaxDocPerFile=" + nMaxDocPerFile.ToString());
                Log.Debug(", sTipoElab=" + sTipoElab.ToString());
                Log.Debug(", ImpostazioneBollettini=" + ImpostazioneBollettini.ToString());
                Log.Debug(", TipoCalcolo=" + TipoCalcolo.ToString());
                Log.Debug(", TipoBollettino=" + TipoBollettino.ToString());
                Log.Debug(", bElabBollettini=" + bElabBollettini.ToString());
                Log.Debug(", bCreaPDF=" + bCreaPDF.ToString());
                Log.Debug(", false=" + false.ToString());
                Log.Debug(", nDecimal=" + nDecimal.ToString());
                Log.Debug(", bSendByMail=" + bSendByMail.ToString() + ".");
                Log.Debug(", IsSoloBollettino=" + IsSoloBollettino.ToString() + ".");
                //*** 201511 - template documenti per ruolo ***
                _StampaDocumenti = oElaborazioneDati.ElaborazioneMassivaDocumenti(DichiarazioneSession.DBType, CodTributo, IdDocToElab, Anno, sIdEnte, IdRuolo.ToString(), Esclusione, StringConnection, StringConnectionOPENgov, StringConnectionAnagrafica, PathTemplate, PathVirtualTemplate, nMaxDocPerFile, sTipoElab, ImpostazioneBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, false, nDecimal, bSendByMail, IsSoloBollettino,TemplateFile,CodTributo);
                if (_StampaDocumenti == null)
                    Log.Debug("_StampaDocumenti è nullo; numero cartelle: ");
                return 1;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.Documenti.ElaboraDocumenti.errore: ", Err);
                return 0;
            }
        }

        private int[,] GetArrayContribuenti(IRemInterfaceOSAP.Cartella[] lista)
        {
            try
            {
                int[,] IdDocToElab = new int[3, lista.Length];
                int nList = -1;
                // ciclo il datatable e mi prendo i contribuenti selezionati
                foreach (IRemInterfaceOSAP.Cartella myItem in lista)
                {
                    nList++;
                    IdDocToElab[0, nList] = myItem.IdCartella;
                    IdDocToElab[1, nList] = myItem.CodContribuente;
                    IdDocToElab[2, nList] = myItem.IdTipoDoc;
                }
                return IdDocToElab;
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.Documenti.GetArrayContribuenti.errore: ", Err);
                return null;
            }
        }

        public RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] GetDocumentiElaborati(string IDEnte, string IDTributo, int IDRuolo)
        {
            try
            {
                if (_StampaDocumenti != null)
                {
                    return _StampaDocumenti;
                }
                else
                {
                    RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL oOggettoGruppoURL;
                    RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[] ListDocumenti = null;
                    RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL oOggettoURL;
                    SqlCommand myCommand = new SqlCommand();
                    DataTable Tabella = new DataTable("TBL");
                    ArrayList ListDocElabEff = new ArrayList();

                    myCommand.Connection = new SqlConnection(DichiarazioneSession.StringConnectionOPENgov);
                    myCommand.CommandTimeout = 0;
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "prc_GetDocEffettiviElaborati";
                    myCommand.Parameters.Add("@IDENTE", SqlDbType.NVarChar).Value = IDEnte;
                    myCommand.Parameters.Add("@TRIBUTO", SqlDbType.NVarChar).Value = IDTributo;
                    myCommand.Parameters.Add("@IDFLUSSORUOLO", SqlDbType.Int).Value = IDRuolo;

                    Log.Debug("GetDocElaboratiEffettivi::query::" + myCommand.CommandText + "::parametri::@codente" + IDEnte + "::Tributo::" + IDTributo + "::idFlussoRuolo::" + IDRuolo.ToString());
                    SqlDataAdapter SelectAdapter = new SqlDataAdapter();
                    SelectAdapter.SelectCommand = myCommand;
                    Log.Debug(Utility.Costanti.LogQuery(myCommand));
                    SelectAdapter.Fill(Tabella);
                    myCommand.Connection.Close();
                    if (Tabella.Rows.Count > 0)
                    {
                        foreach (DataRow myRow in Tabella.Rows)
                        {
                            oOggettoURL = new RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL();

                            oOggettoURL.Name = myRow["NOME_FILE"].ToString();
                            oOggettoURL.Path = myRow["PATH"].ToString();
                            oOggettoURL.Url = myRow["PATH_WEB"].ToString();

                            oOggettoGruppoURL = new RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL();
                            oOggettoGruppoURL.URLComplessivo = oOggettoURL;

                            ListDocElabEff.Add(oOggettoGruppoURL);
                        }
                        ListDocumenti = (RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL[])ListDocElabEff.ToArray(typeof(RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL));
                    }
                    return ListDocumenti;
                }
            }
            catch (Exception Err)
            {
                Log.Debug(DichiarazioneSession.IdEnte + "."+ DichiarazioneSession.sOperatore + " - OPENgovOSAP.Documenti.GetDocumentiElaborati.errore: ", Err);
                return null;
            }
        }
    }
}
