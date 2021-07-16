using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;
using System.Threading;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione della simulazione
    /// </summary>
    public class Simula : DbObject<Simula>
    {
        #region Variables and constructor
        public Simula()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int IdSimulazione { get; set; }
        public bool IsReadOnly { get; set; }
        public ParametriCalcoloEnte ParamCalcolo { get; set; }
        public DatiPEF ParamPEF { get; set; }
        public CoefficienteKA[] KA { get; set; }
        public CoefficienteKB[] KB { get; set; }
        public CoefficienteKC[] KC { get; set; }
        public CoefficienteKD[] KD { get; set; }
        public Articolo[] ListArticoli { get; set; }
        public TotaliSimulazione[] ListTotali { get; set; }
        public ToDefineTariffe[] ListDefineTariffe { get; set; }
        public TariffeEnte[] ListTariffe { get; set; }
        public DateTime DataConferma { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is Simula) &&
                ((obj as Simula).IdSimulazione == IdSimulazione);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdSimulazione);
        }

        public override sealed void Reset()
        {
            IdSimulazione = default(int);
            IsReadOnly = default(bool);
            ParamCalcolo = new ParametriCalcoloEnte();
            ParamPEF = new DatiPEF();
            KA = null;
            KB = null;
            KC = null;
            KD = null;
            ListArticoli = null;
            ListTotali = null;
            ListDefineTariffe = null;
            ListTariffe = null;
            DataConferma = DateTime.MaxValue;
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (ParamCalcolo.FromVariabile != string.Empty || ParamCalcolo.FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(ParamCalcolo.Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(ParamCalcolo.Anno));
                Global.Log.Write2(LogSeverity.Debug, myTypeConn + " prc_TBLSIMULAZIONE_S " + ParamCalcolo.Ente +","+ ParamCalcolo.Anno);
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    IdSimulazione = DbValue<int>.Get(sqlRead["Id"]);
                    DataConferma = DbValue<DateTime>.Get(sqlRead["dataconferma"]);
                    ParamCalcolo.FromVariabile = ParamCalcolo.FromVariabile;
                    ParamCalcolo.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    ParamCalcolo.Anno = DbValue<int>.Get(sqlRead["Anno"]).ToString();
                    ParamCalcolo.TypeCalcolo = DbValue<string>.Get(sqlRead["TipoCalcolo"]);
                    ParamCalcolo.HasMaggiorazione = DbValue<Boolean>.Get(sqlRead["HasMaggiorazione"]);
                    ParamCalcolo.HasConferimenti = DbValue<Boolean>.Get(sqlRead["HasConferimenti"]);
                    ParamCalcolo.TypeMQ = DbValue<string>.Get(sqlRead["TipoMQ"]);
                    ParamCalcolo.TypeNCNonRes = DbValue<string>.Get(sqlRead["TipoNCNonRes"]);
                    ParamCalcolo.NCNonRes = DbValue<int>.Get(sqlRead["NCNonRes"]);
                    ParamCalcolo.TypeValiditaUpdateRes = DbValue<string>.Get(sqlRead["TipoValiditaAggRes"]);
                    ParamCalcolo.DescrTypeMQ = DbValue<string>.Get(sqlRead["DESCRTYPEMQ"]);
                    ParamCalcolo.DescrTypeNCNonRes = DbValue<string>.Get(sqlRead["DESCRNCNONRES"]);
                    ParamCalcolo.DescrTypeValiditaUpdateRes = DbValue<string>.Get(sqlRead["DESCRVALIDITAUPDATERES"]);

                    ParamPEF = new DatiPEF { IdSimulazione = IdSimulazione, FromVariabile = ParamCalcolo.FromVariabile, Ente = ParamCalcolo.Ente, Anno = ParamCalcolo.Anno };
                    ParamPEF.Load();

                    KA = new CoefficienteKA { FromVariabile = ParamCalcolo.FromVariabile, Ente = ParamCalcolo.Ente }.LoadAll();
                    if (KA == null)
                        return false;
                    KB = new CoefficienteKB { FromVariabile = ParamCalcolo.FromVariabile, Ente = ParamCalcolo.Ente }.LoadAll();
                    if (KB == null)
                        return false;
                    KC = new CoefficienteKC { FromVariabile = ParamCalcolo.FromVariabile, Ente = ParamCalcolo.Ente }.LoadAll();//, IsKCMax = ParamPEF.IsKCMax
                    if (KC == null)
                        return false;
                    KD = new CoefficienteKD { FromVariabile = ParamCalcolo.FromVariabile, Ente = ParamCalcolo.Ente }.LoadAll();//, IsKDMax = ParamPEF.IsKDMax
                    if (KD == null)
                        return false;

                    ListArticoli = new Articolo { IdSimulazione = IdSimulazione, FromVariabile = ParamCalcolo.FromVariabile }.LoadAll();
                    ListTotali = new TotaliSimulazione { IdSimulazione = IdSimulazione, FromVariabile = ParamCalcolo.FromVariabile }.LoadAll();

                    IsReadOnly = true;
                    DefineTariffe();

                    ListTariffe = new TariffeEnte { Ente = ParamCalcolo.Ente, FromVariabile = ParamCalcolo.FromVariabile, Anno = ParamCalcolo.Anno }.LoadSimulazione(IdSimulazione);
                }
                else
                {
                    ParamPEF = new DatiPEF { IdSimulazione = IdSimulazione, FromVariabile = ParamCalcolo.FromVariabile, Ente = ParamCalcolo.Ente, Anno = ParamCalcolo.Anno };
                    ParamPEF.Load();
                    //Reset();
                }
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override Simula[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (ParamCalcolo.FromVariabile != string.Empty || ParamCalcolo.FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_PARAMCALCOLO_IU";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@Id", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(ParamCalcolo.Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(ParamCalcolo.Anno));
                sqlCmd.Parameters.AddWithValue("@TipoCalcolo", DbParam.Get(ParamCalcolo.TypeCalcolo));
                sqlCmd.Parameters.AddWithValue("@HasMaggiorazione", DbParam.Get(ParamCalcolo.HasMaggiorazione));
                sqlCmd.Parameters.AddWithValue("@HasConferimenti", DbParam.Get(ParamCalcolo.HasConferimenti));
                sqlCmd.Parameters.AddWithValue("@TipoMQ", DbParam.Get(ParamCalcolo.TypeMQ));
                sqlCmd.Parameters.AddWithValue("@DataConferma", DbParam.Get(DataConferma));
                sqlCmd.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                string sValParam="";
                foreach (SqlParameter myparam in sqlCmd.Parameters)
                    sValParam += " ," + myparam.ParameterName + "='" + myparam.Value.ToString()+"'";
                Global.Log.Write2(LogSeverity.Debug, "query::" + sqlCmd.CommandText + " " + sValParam);
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdSimulazione = (int)sqlCmd.Parameters["@Id"].Value;
                if (IdSimulazione >= 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (ParamCalcolo.FromVariabile != string.Empty || ParamCalcolo.FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_PARAMCALCOLO_D";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@Id", DbParam.Get(IdSimulazione));
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
        #endregion
        public void StartAnalyze()
        {
            ThreadStart threadDelegate = new ThreadStart(AnalyzeThreadEntryPoint);
            Thread t = new Thread(threadDelegate);
            t.Start();
        }
        private void AnalyzeThreadEntryPoint()
        {
            CacheManager.SetElaborazioneInCorso("Elaborazione Ente:" + ParamCalcolo.Ente + " Anno:" + ParamCalcolo.Anno);
            Global.Log.Write2(LogSeverity.Debug, "settato elaborazione in corso");
            string sAvanzamento = "";
            int nAvanzamento = 0;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            SqlDataReader sqlMyRead = null;
            List<int> listIdTestata = new List<int>();
            Articolo item = new Articolo();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            int idPrec = 0;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (ParamCalcolo.FromVariabile != string.Empty || ParamCalcolo.FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }

                //salvo i parametri di calcolo
                if (!Save())
                {
                    Global.Log.Write2(LogSeverity.Debug, "non salvato");
                    return;
                }
                //salvo i dati PEF
                ParamPEF.IdSimulazione = IdSimulazione;
                if (!ParamPEF.Save())
                {
                    Global.Log.Write2(LogSeverity.Debug, "non salvato parampef");
                    return;
                }

                //prelevo le dichiarazioni/utenti validi per l'anno
                try
                {
                    Connect(myTypeConn);
                    sqlCmd = CreateCommand();
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "prc_GetAvvisi";
                    sqlCmd.Parameters.Clear();
                    sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(ParamCalcolo.Ente));
                    sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(ParamCalcolo.Anno));
                    string sValParam = "";
                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                        sValParam += " ," + myparam.ParameterName + "='" + myparam.Value.ToString() + "'";
                    Global.Log.Write2(LogSeverity.Debug, "query::" + sqlCmd.CommandText + " " + sValParam);
                    sqlRead = sqlCmd.ExecuteReader();
                    while (sqlRead.Read())
                    {
                        listIdTestata.Add(DbValue<int>.Get(sqlRead["idtestata"]));
                    }
                }
                catch (Exception ex)
                {
                    Global.Log.Write2(LogSeverity.Critical, ex);
                    return;
                }
                finally
                {
                    Disconnect(sqlCmd, sqlRead);
                }
                //se ho dichiarazioni/utenti ciclo e per ognuno prelevo le UI
                foreach (int myId in listIdTestata)
                {
                    nAvanzamento += 1;
                    sAvanzamento = "Elaborazione posizione " + nAvanzamento + " su " + listIdTestata.Count.ToString();
                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento);
                    try
                    {
                        Connect(myTypeConn);
                        sqlCmd = CreateCommand();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "prc_GetSIMULAUtenze";
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@IDTESTATA", DbParam.Get(myId));
                        sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(DbValue<string>.Get(ParamCalcolo.Anno)));
                        sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(ParamCalcolo.Ente));
                        sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                        string sValParam = "";
                        foreach (SqlParameter myparam in sqlCmd.Parameters)
                            sValParam += " ," + myparam.ParameterName + "='" + myparam.Value.ToString() + "'";
                        Global.Log.Write2(LogSeverity.Debug, "query::" + sqlCmd.CommandText + " " + sValParam);
                        sqlMyRead = sqlCmd.ExecuteReader();
                        while (sqlMyRead.Read())
                        {
                            //if (DbValue<int>.Get(sqlMyRead["id"]) != idPrec)
                            //{
                            if (item.Partita.Id > 0)
                                if (!item.Save())
                                    return;

                            item = new Articolo();
                            MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
                            MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
                            item.IdSimulazione = IdSimulazione;
                            item.FromVariabile = ParamCalcolo.FromVariabile;
                            item.IdEnte = ParamCalcolo.Ente;

                            item.Anagrafe.COD_CONTRIBUENTE = DbValue<int>.Get(sqlMyRead["IdContribuente"]);

                            item.Partita.Id = DbValue<int>.Get(sqlMyRead["id"]);
                            item.Partita.sCodVia = DbValue<string>.Get(sqlMyRead["idvia"]);
                            item.Partita.sVia = DbValue<string>.Get(sqlMyRead["via"]);
                            item.Partita.sCivico = DbValue<string>.Get(sqlMyRead["civico"]);
                            item.Partita.sEsponente = DbValue<string>.Get(sqlMyRead["esponente"]);
                            item.Partita.sScala = DbValue<string>.Get(sqlMyRead["scala"]);
                            item.Partita.sInterno = DbValue<string>.Get(sqlMyRead["interno"]);
                            item.Partita.sFoglio = DbValue<string>.Get(sqlMyRead["foglio"]);
                            item.Partita.sNumero = DbValue<string>.Get(sqlMyRead["numero"]);
                            item.Partita.sSubalterno = DbValue<string>.Get(sqlMyRead["subalterno"]);
                            item.Partita.tDataInizio = DbValue<DateTime>.Get(sqlMyRead["data_inizio"]);
                            item.Partita.tDataFine = DbValue<DateTime>.Get(sqlMyRead["data_fine"]);
                            item.Partita.sIdStatoOccupazione = DbValue<string>.Get(sqlMyRead["idstatooccupazione"]);
                            item.Partita.IdCatAteco = DbValue<int>.Get(sqlMyRead["idcategoria"]);
                            item.Partita.nNComponenti = DbValue<int>.Get(sqlMyRead["ncpf"]);
                            item.Partita.nComponentiPV = DbValue<int>.Get(sqlMyRead["ncpv"]);
                            item.Partita.nMQTassabili = doubleParser(sqlMyRead["mq"].ToString());
                            item.Partita.tDataVariazione = item.Partita.tDataCessazione = DateTime.MaxValue;
                            item.UtenteUtile = doubleParser(sqlMyRead["utenteutile"].ToString());
                            item.UtenteUtileLordo = doubleParser(sqlMyRead["UTENTEUTILELORDO"].ToString());
                            item.MQUtiliPF = doubleParser(sqlMyRead["mqutilipf"].ToString());
                            item.MQUtiliPV = doubleParser(sqlMyRead["mqutilipv"].ToString());
                            item.MQUtiliLordo = doubleParser(sqlMyRead["mqutililordo"].ToString());
                            //}
                            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentRid = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                            CurrentRid.sCodice = DbValue<string>.Get(sqlMyRead["CODRID"]);
                            if (CurrentRid.sCodice != string.Empty)
                            {
                                MyArrayRid.Add(CurrentRid);
                                item.Partita.oRiduzioni = MyArrayRid.ToArray();
                            }
                            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentDet = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                            CurrentDet.sCodice = DbValue<string>.Get(sqlMyRead["CODDET"]);
                            if (CurrentDet.sCodice != string.Empty)
                            {
                                MyArrayDet.Add(CurrentDet);
                                item.Partita.oDetassazioni = MyArrayDet.ToArray();
                            }

                            idPrec = DbValue<int>.Get(sqlMyRead["id"]);
                        }
                        if (item.Partita.Id > 0)
                            if (!item.Save())
                                return;
                        item = new Articolo(); idPrec = 0;
                    }
                    catch (Exception ex)
                    {
                        Global.Log.Write2(LogSeverity.Critical, ex);
                        return;
                    }
                    finally
                    {
                        Disconnect(sqlMyRead);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
            }
            finally
            {
                CacheManager.RemoveAvanzamentoElaborazione();
                CacheManager.SetElaborazioneInCorso("Elaborazione Finita|"+ParamCalcolo.Anno+"|"+ParamCalcolo.TypeCalcolo+"|");
            }
        }
        public bool Analyze()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            SqlDataReader sqlMyRead = null;
            List<int> listIdTestata = new List<int>();
            Articolo item = new Articolo();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            int idPrec = 0;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (ParamCalcolo.FromVariabile != string.Empty || ParamCalcolo.FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }

                //salvo i parametri di calcolo
                if (!Save())
                {
                    return false;
                }
                //salvo i dati PEF
                ParamPEF.IdSimulazione = IdSimulazione;
                if (!ParamPEF.Save())
                {
                    return false;
                }

                //prelevo le dichiarazioni/utenti validi per l'anno
                try
                {
                    Connect(myTypeConn);
                    sqlCmd = CreateCommand();
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "prc_GetAvvisi";
                    sqlCmd.Parameters.Clear();
                    sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(ParamCalcolo.Ente));
                    sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(ParamCalcolo.Anno));
                    sqlRead = sqlCmd.ExecuteReader();
                    while (sqlRead.Read())
                    {
                        listIdTestata.Add(DbValue<int>.Get(sqlRead["idtestata"]));
                    }
                }
                catch (Exception ex)
                {
                    Global.Log.Write2(LogSeverity.Critical, ex);
                    return false;
                }
                finally
                {
                    Disconnect(sqlCmd, sqlRead);
                }
                //se ho dichiarazioni/utenti ciclo e per ognuno prelevo le UI
                foreach (int myId in listIdTestata)
                {
                    try
                    {
                        Connect(myTypeConn);
                        sqlCmd = CreateCommand();
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "prc_GetSIMULAUtenze";
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@IDTESTATA", DbParam.Get(myId));
                        sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(DbValue<string>.Get(ParamCalcolo.Anno)));
                        sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(ParamCalcolo.Ente));
                        sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                        sqlMyRead = sqlCmd.ExecuteReader();
                        while (sqlMyRead.Read())
                        {
                            //if (DbValue<int>.Get(sqlMyRead["id"]) != idPrec)
                            //{
                                if (item.Partita.Id > 0)
                                    if (!item.Save())
                                        return false;

                                item = new Articolo();
                                MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
                                MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
                                item.IdSimulazione = IdSimulazione;
                                item.FromVariabile = ParamCalcolo.FromVariabile;
                                item.IdEnte = ParamCalcolo.Ente;

                                item.Anagrafe.COD_CONTRIBUENTE = DbValue<int>.Get(sqlMyRead["IdContribuente"]);

                                item.Partita.Id = DbValue<int>.Get(sqlMyRead["id"]);
                                item.Partita.sCodVia = DbValue<string>.Get(sqlMyRead["idvia"]);
                                item.Partita.sVia = DbValue<string>.Get(sqlMyRead["via"]);
                                item.Partita.sCivico = DbValue<string>.Get(sqlMyRead["civico"]);
                                item.Partita.sEsponente = DbValue<string>.Get(sqlMyRead["esponente"]);
                                item.Partita.sScala = DbValue<string>.Get(sqlMyRead["scala"]);
                                item.Partita.sInterno = DbValue<string>.Get(sqlMyRead["interno"]);
                                item.Partita.sFoglio = DbValue<string>.Get(sqlMyRead["foglio"]);
                                item.Partita.sNumero = DbValue<string>.Get(sqlMyRead["numero"]);
                                item.Partita.sSubalterno = DbValue<string>.Get(sqlMyRead["subalterno"]);
                                item.Partita.tDataInizio = DbValue<DateTime>.Get(sqlMyRead["data_inizio"]);
                                item.Partita.tDataFine = DbValue<DateTime>.Get(sqlMyRead["data_fine"]);
                                item.Partita.sIdStatoOccupazione = DbValue<string>.Get(sqlMyRead["idstatooccupazione"]);
                                item.Partita.IdCatAteco = DbValue<int>.Get(sqlMyRead["idcategoria"]);
                                item.Partita.nNComponenti = DbValue<int>.Get(sqlMyRead["ncpf"]);
                                item.Partita.nComponentiPV = DbValue<int>.Get(sqlMyRead["ncpv"]);
                                item.Partita.nMQTassabili = doubleParser(sqlMyRead["mq"].ToString());
                                item.Partita.tDataVariazione = item.Partita.tDataCessazione = DateTime.MaxValue;
                                item.UtenteUtile = doubleParser(sqlMyRead["utenteutile"].ToString());
                                item.UtenteUtileLordo = doubleParser(sqlMyRead["UTENTEUTILELORDO"].ToString());
                                item.MQUtiliPF = doubleParser(sqlMyRead["mqutilipf"].ToString());
                                item.MQUtiliPV = doubleParser(sqlMyRead["mqutilipv"].ToString());
                                item.MQUtiliLordo = doubleParser(sqlMyRead["mqutililordo"].ToString());
                            //}
                            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentRid = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                            CurrentRid.sCodice = DbValue<string>.Get(sqlMyRead["CODRID"]);
                            if (CurrentRid.sCodice != string.Empty)
                            {
                                MyArrayRid.Add(CurrentRid);
                                item.Partita.oRiduzioni = MyArrayRid.ToArray();
                            }
                            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentDet = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                            CurrentDet.sCodice = DbValue<string>.Get(sqlMyRead["CODDET"]);
                            if (CurrentDet.sCodice != string.Empty)
                            {
                                MyArrayDet.Add(CurrentDet);
                                item.Partita.oDetassazioni = MyArrayDet.ToArray();
                            }

                            idPrec = DbValue<int>.Get(sqlMyRead["id"]);
                        }
                        if (item.Partita.Id > 0)
                            if (!item.Save())
                                return false;
                        item = new Articolo(); idPrec = 0;
                    }
                    catch (Exception ex)
                    {
                        Global.Log.Write2(LogSeverity.Critical, ex);
                        return false;
                    }
                    finally
                    {
                        Disconnect(sqlMyRead);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
        }
        public bool DefineTariffe()
        {
            SqlCommand sqlCmd = null;
            DataSet dsMyDati = new DataSet();
            string myTypeConn = "OPENgovTARSU";
            List<ToDefineTariffe> list = new List<ToDefineTariffe>();

            try
            {
                //*** PARTE FISSA ***
                //--- DOMESTICA ---
                //MQNORM=i metri per NC devono essere moltiplicati per il KA rispettivo
                //QUF=costi fissi dom/sum(MQNORM)
                //TARIFFA=QUF*KA

                //--- NON DOMESTICA ---
                //MQNORM=i metri per CAT devono essere moltiplicati per il KC rispettivo
                //QUF=costi fissi non dom/sum(MQNORM)
                //TARIFFA=QUF*KC

                //*** ***

                //*** PARTE VARIABILE ***
                //--- DOMESTICA ---
                //NUTNORM=il numero di utenze per NC deve essere moltiplicato per il KB rispettivo
                //QUF=kg rifiuti dom/sum(NUTNORM)
                //CU=costi variabili dom/kg rifiuti dom
                //TARIFFA=CU*QUF*KB

                //--- NON DOMESTICA ---
                //MQNORM=i metri per CAT deve essere moltiplicato per il KD rispettivo
                //QUF=costi non dom/sum(MQNORM)
                //TARIFFA=QUF*KD

                //*** ***
                if (ParamCalcolo.FromVariabile != string.Empty || ParamCalcolo.FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Disconnect(sqlCmd);
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetSIMULADefineTariffe";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IsReadOnly", DbParam.Get(IsReadOnly));
                sqlCmd.Parameters.AddWithValue("@IMPCFDOM", DbParam.Get(ParamPEF.CostiPFDOM));
                sqlCmd.Parameters.AddWithValue("@IMPCFNONDOM", DbParam.Get(ParamPEF.CostiPFNONDOM));
                sqlCmd.Parameters.AddWithValue("@IMPCVDOM", DbParam.Get(ParamPEF.CostiPVDOM));
                sqlCmd.Parameters.AddWithValue("@IMPCVNONDOM", DbParam.Get(ParamPEF.CostiPVNONDOM));
                sqlCmd.Parameters.AddWithValue("@KG", DbParam.Get(ParamPEF.KGRifiuti));
                sqlCmd.Parameters.AddWithValue("@PERCKGDOM", DbParam.Get(ParamPEF.PercKGDOM));
                sqlCmd.Parameters.AddWithValue("@PERCKGNONDOM", DbParam.Get(ParamPEF.PercKGNONDOM));
                Global.Log.Write2(LogSeverity.Debug, myTypeConn + " prc_GetSIMULADefineTariffe " + IdSimulazione.ToString()+ "," + IsReadOnly.ToString() + "," + ParamPEF.CostiPFDOM.ToString() + "," + ParamPEF.CostiPFNONDOM.ToString() + "," + ParamPEF.CostiPVDOM.ToString() + "," + ParamPEF.CostiPVNONDOM.ToString() + "," + ParamPEF.KGRifiuti.ToString() + "," + ParamPEF.PercKGDOM.ToString() + "," + ParamPEF.PercKGNONDOM.ToString());

                SqlDataAdapter myAdapter = new SqlDataAdapter(sqlCmd);
                myAdapter.TableMappings.Add("Table1", "PF_DOM");
                myAdapter.TableMappings.Add("Table2", "PF_NONDOM");
                myAdapter.TableMappings.Add("Table3", "PV_DOM");
                myAdapter.TableMappings.Add("Table4", "PV_NONDOM");

                myAdapter.Fill(dsMyDati);

                foreach (DataTable myTable in dsMyDati.Tables)
                {
                    foreach (DataRow myRow in myTable.Rows)
                    {
                        ToDefineTariffe item = new ToDefineTariffe();
                        item.Tipo = DbValue<string>.Get(myRow["tipo"]);
                        item.IdCategoria = DbValue<int>.Get(myRow["idcategoria"]);
                        item.Descrizione = DbValue<string>.Get(myRow["definizione"]);
                        item.NC = DbValue<int>.Get(myRow["nc"]);
                        item.MQ = doubleParser(myRow["mq"].ToString());
                        item.CoeffK = doubleParser(myRow["coef"].ToString());
                        item.MQNormalizzati = doubleParser(myRow["mqnormalizzati"].ToString());
                        item.QUF = doubleParser(myRow["quf"].ToString());
                        item.Tariffa = doubleParser(myRow["tariffa"].ToString());
                        list.Add(item);
                    }
                }
                ListDefineTariffe = list.ToArray();
                ListTariffe = new TariffeEnte { FromVariabile = ParamCalcolo.FromVariabile }.LoadSimulazione(IdSimulazione);

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
   }
    /// <summary>
    /// Classe per la gestione dei dati PEF della simulazione
    /// </summary>
    public class DatiPEF : DbObject<DatiPEF>
    {
        #region Variables and constructor
        public DatiPEF()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public string Anno { get; set; }
        public double NUtenze { get; set; }
        public double NUtenzeDOM { get; set; }
        public double NUtenzeNONDOM { get; set; }
        public double PercUtenze { get; set; }
        public double PercUtenzeDOM { get; set; }
        public double PercUtenzeNONDOM { get; set; }
        public double MQ { get; set; }
        public double MQDOM { get; set; }
        public double MQNONDOM { get; set; }
        public double PercMQ { get; set; }
        public double PercMQDOM { get; set; }
        public double PercMQNONDOM { get; set; }
        public double KGRifiuti { get; set; }
        public double KGRifiutiDOM { get; set; }
        public double KGRifiutiNONDOM { get; set; }
        public double PercKG { get; set; }
        public double PercKGDOM { get; set; }
        public double PercKGNONDOM { get; set; }
        public double CostiPF { get; set; }
        public double CostiPFDOM { get; set; }
        public double CostiPFNONDOM { get; set; }
        public double CostiPV { get; set; }
        public double CostiPVDOM { get; set; }
        public double CostiPVNONDOM { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            Ente = default(string);
            Anno = default(string);
            NUtenze = default(int);
            NUtenzeDOM = default(int);
            NUtenzeNONDOM = default(int);
            PercUtenze = default(int);
            PercUtenzeDOM = default(int);
            PercUtenzeNONDOM = default(int);
            MQ = default(int);
            MQDOM = default(int);
            MQNONDOM = default(int);
            PercMQ = default(int);
            PercMQDOM = default(int);
            PercMQNONDOM = default(int);
            KGRifiuti = default(int);
            KGRifiutiDOM = default(int);
            KGRifiutiNONDOM = default(int);
            PercKG = default(int);
            PercKGDOM = default(int);
            PercKGNONDOM = default(int);
            CostiPF = default(int);
            CostiPFDOM = default(int);
            CostiPFNONDOM = default(int);
            CostiPV = default(int);
            CostiPVDOM = default(int);
            CostiPVNONDOM = default(int);
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_PEF_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                Global.Log.Write2(LogSeverity.Debug, myTypeConn + " prc_TBLSIMULAZIONE_PEF_S " + IdSimulazione.ToString()+ "," + Ente + "," + Anno);
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    Id = DbValue<int>.Get(sqlRead["ID"]);
                    IdSimulazione = DbValue<int>.Get(sqlRead["fkIdSimulazione"]);

                    NUtenze = doubleParser(sqlRead["NUtenze"].ToString());
                    NUtenzeDOM = doubleParser(sqlRead["NUtenzeDOM"].ToString());
                    NUtenzeNONDOM = doubleParser(sqlRead["NUtenzeNONDOM"].ToString());
                    PercUtenze = doubleParser(sqlRead["PercUtenze"].ToString());
                    PercUtenzeDOM = doubleParser(sqlRead["PercUtenzeDOM"].ToString());
                    PercUtenzeNONDOM = doubleParser(sqlRead["PercUtenzeNONDOM"].ToString());

                    MQ = doubleParser(sqlRead["MQ"].ToString());
                    MQDOM = doubleParser(sqlRead["MQDOM"].ToString());
                    MQNONDOM = doubleParser(sqlRead["MQNONDOM"].ToString());
                    PercMQ = doubleParser(sqlRead["PercMQ"].ToString());
                    PercMQDOM = doubleParser(sqlRead["PercMQDOM"].ToString());
                    PercMQNONDOM = doubleParser(sqlRead["PercMQNONDOM"].ToString());
                    
                    KGRifiuti = doubleParser(sqlRead["KGRifiuti"].ToString());
                    KGRifiutiDOM = doubleParser(sqlRead["KGRifiutiDOM"].ToString());
                    KGRifiutiNONDOM = doubleParser(sqlRead["KGRifiutiNONDOM"].ToString());
                    PercKG = doubleParser(sqlRead["PercKG"].ToString());
                    PercKGDOM = doubleParser(sqlRead["PercKGDOM"].ToString());
                    PercKGNONDOM = doubleParser(sqlRead["PercKGNONDOM"].ToString());
                    
                    CostiPF = doubleParser(sqlRead["CostiPF"].ToString());
                    CostiPFDOM = doubleParser(sqlRead["CostiPFDOM"].ToString());
                    CostiPFNONDOM = doubleParser(sqlRead["CostiPFNONDOM"].ToString());
                    
                    CostiPV = doubleParser(sqlRead["CostiPV"].ToString());
                    CostiPVDOM = doubleParser(sqlRead["CostiPVDOM"].ToString());
                    CostiPVNONDOM = doubleParser(sqlRead["CostiPVNONDOM"].ToString());
                }
                else
                {
                    Reset();
                }

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override DatiPEF[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_PEF_IU";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(Id));
                sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@NUTENZE", DbParam.Get(NUtenze));
                sqlCmd.Parameters.AddWithValue("@NUTENZEDOM", DbParam.Get(NUtenzeDOM));
                sqlCmd.Parameters.AddWithValue("@NUTENZENONDOM", DbParam.Get(NUtenzeNONDOM));
                sqlCmd.Parameters.AddWithValue("@PERCUTENZE", DbParam.Get(PercUtenze));
                sqlCmd.Parameters.AddWithValue("@PERCUTENZEDOM", DbParam.Get(PercUtenzeDOM));
                sqlCmd.Parameters.AddWithValue("@PERCUTENZENONDOM", DbParam.Get(PercUtenzeNONDOM));

                sqlCmd.Parameters.AddWithValue("@MQ", DbParam.Get(MQ));
                sqlCmd.Parameters.AddWithValue("@MQDOM", DbParam.Get(MQDOM));
                sqlCmd.Parameters.AddWithValue("@MQNONDOM", DbParam.Get(MQNONDOM));
                sqlCmd.Parameters.AddWithValue("@PERCMQ", DbParam.Get(PercMQ));
                sqlCmd.Parameters.AddWithValue("@PERCMQDOM", DbParam.Get(PercMQDOM));
                sqlCmd.Parameters.AddWithValue("@PERCMQNONDOM", DbParam.Get(PercMQNONDOM));
                
                sqlCmd.Parameters.AddWithValue("@KGRIFIUTI", DbParam.Get(KGRifiuti));
                sqlCmd.Parameters.AddWithValue("@KGRIFIUTIDOM", DbParam.Get(KGRifiutiDOM));
                sqlCmd.Parameters.AddWithValue("@KGRIFIUTINONDOM", DbParam.Get(KGRifiutiNONDOM));
                sqlCmd.Parameters.AddWithValue("@PERCKG", DbParam.Get(PercKG));
                sqlCmd.Parameters.AddWithValue("@PERCKGDOM", DbParam.Get(PercKGDOM));
                sqlCmd.Parameters.AddWithValue("@PERCKGNONDOM", DbParam.Get(PercKGNONDOM));
                
                sqlCmd.Parameters.AddWithValue("@COSTIPF", DbParam.Get(CostiPF));
                sqlCmd.Parameters.AddWithValue("@COSTIPFDOM", DbParam.Get(CostiPFDOM));
                sqlCmd.Parameters.AddWithValue("@COSTIPFNONDOM", DbParam.Get(CostiPFNONDOM));
                
                sqlCmd.Parameters.AddWithValue("@COSTIPV", DbParam.Get(CostiPV));
                sqlCmd.Parameters.AddWithValue("@COSTIPVDOM", DbParam.Get(CostiPVDOM));
                sqlCmd.Parameters.AddWithValue("@COSTIPVNONDOM", DbParam.Get(CostiPVNONDOM));
                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                Id = (int)sqlCmd.Parameters["@ID"].Value;
                if (Id >= 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione degli articoli della simulazione
    /// </summary>
    public class Articolo : DbObject<Articolo>
    {
        #region Variables and constructor
        public Articolo()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string IdEnte { get; set; }
        public AnagInterface.DettaglioAnagrafica Anagrafe { get; set; }
        public RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjUnitaImmobiliare Partita { get; set; }
        public double UtenteUtile { get; set; }
        public double UtenteUtileLordo { get; set; }
        public double MQUtiliPF { get; set; }
        public double MQUtiliPV { get; set; }
        public double MQUtiliLordo { get; set; }
        public int IdCatAtecoOld { get; set; }
        public int nComponentiPFOld { get; set; }
        public int nComponentiPVOld { get; set; }
        public RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata ParamSearch { get; set; }
        public bool IsSel { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
               (obj is Articolo) &&
               ((obj as Articolo).Id == Id);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(Id);
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            IdEnte = default(string);
            Anagrafe = new AnagInterface.DettaglioAnagrafica();
            Partita = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjUnitaImmobiliare();
            UtenteUtile = default(double);
            UtenteUtileLordo = default(double);
            MQUtiliPF = default(double);
            MQUtiliPV = default(double);
            MQUtiliLordo = default(double);
            ParamSearch = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjSearchTestata();
            IsSel = false;
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            int idPrec = 0;
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLI_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(IdEnte));
                sqlCmd.Parameters.AddWithValue("@Id", DbParam.Get(Id));
                sqlCmd.Parameters.AddWithValue("@VIA", DbParam.Get(ParamSearch.sVia));
                sqlCmd.Parameters.AddWithValue("@CIVICO", DbParam.Get(ParamSearch.sCivico));
                sqlCmd.Parameters.AddWithValue("@INTERNO", DbParam.Get(ParamSearch.sInterno));
                sqlCmd.Parameters.AddWithValue("@FOGLIO", DbParam.Get(ParamSearch.sFoglio));
                sqlCmd.Parameters.AddWithValue("@NUMERO", DbParam.Get(ParamSearch.sNumero));
                sqlCmd.Parameters.AddWithValue("@SUBALTERNO", DbParam.Get(ParamSearch.sSubalterno));
                sqlCmd.Parameters.AddWithValue("@DAL", DbParam.Get(ParamSearch.Dal));
                sqlCmd.Parameters.AddWithValue("@AL", DbParam.Get(ParamSearch.Al));
                sqlCmd.Parameters.AddWithValue("@IDPROVENIENZA", "");
                sqlCmd.Parameters.AddWithValue("@TYPESOGRES", DbParam.Get(ParamSearch.TypeSogRes));
                sqlCmd.Parameters.AddWithValue("@CATCATASTALE", DbParam.Get(ParamSearch.IdCatCatastale));
                sqlCmd.Parameters.AddWithValue("@IDCATEGORIA", DbParam.Get(ParamSearch.IdCatTARES));
                sqlCmd.Parameters.AddWithValue("@NC", DbParam.Get(ParamSearch.nComponenti));
                sqlCmd.Parameters.AddWithValue("@ISPF", DbParam.Get(ParamSearch.IsPF));
                sqlCmd.Parameters.AddWithValue("@ISPV", DbParam.Get(ParamSearch.IsPV));
                sqlCmd.Parameters.AddWithValue("@ISESENTE", DbParam.Get(ParamSearch.IsEsente));
                sqlCmd.Parameters.AddWithValue("@HASMOREUI", DbParam.Get(ParamSearch.HasMoreUI));
                sqlCmd.Parameters.AddWithValue("@IDRIDUZIONE", DbParam.Get(ParamSearch.IdRiduzione));
                sqlCmd.Parameters.AddWithValue("@IDDETASSAZIONE", DbParam.Get(ParamSearch.IdDetassazione));
                sqlCmd.Parameters.AddWithValue("@IDSTATOOCCUPAZIONE", DbParam.Get(ParamSearch.IdStatoOccupazione));
                sqlRead = sqlCmd.ExecuteReader();
                while (sqlRead.Read())
                {
                    if (DbValue<int>.Get(sqlRead["id"]) != idPrec)
                    {
                        Id = DbValue<int>.Get(sqlRead["id"]);
                        IdEnte = DbValue<string>.Get(sqlRead["idente"]);

                        Anagrafe.COD_CONTRIBUENTE = DbValue<int>.Get(sqlRead["IdContribuente"]);
                        Anagrafe.Cognome = DbValue<string>.Get(sqlRead["Cognome"]);
                        Anagrafe.Nome = DbValue<string>.Get(sqlRead["Nome"]);
                        Anagrafe.CodiceFiscale = DbValue<string>.Get(sqlRead["Cod_Fiscale"]);
                        Anagrafe.PartitaIva = DbValue<string>.Get(sqlRead["Partita_IVA"]);

                        Partita.Id = DbValue<int>.Get(sqlRead["fkidoggetto"]);
                        Partita.sCodVia = DbValue<string>.Get(sqlRead["idvia"]);
                        Partita.sVia = DbValue<string>.Get(sqlRead["via"]);
                        Partita.sCivico = DbValue<string>.Get(sqlRead["civico"]);
                        Partita.sEsponente = DbValue<string>.Get(sqlRead["esponente"]);
                        Partita.sScala = DbValue<string>.Get(sqlRead["scala"]);
                        Partita.sInterno = DbValue<string>.Get(sqlRead["interno"]);
                        Partita.sFoglio = DbValue<string>.Get(sqlRead["foglio"]);
                        Partita.sNumero = DbValue<string>.Get(sqlRead["numero"]);
                        Partita.sSubalterno = DbValue<string>.Get(sqlRead["subalterno"]);
                        Partita.tDataInizio = DbValue<DateTime>.Get(sqlRead["data_inizio"]);
                        Partita.tDataFine = DbValue<DateTime>.Get(sqlRead["data_fine"]);
                        Partita.sCatCatastale = DbValue<string>.Get(sqlRead["catcatastale"]);
                        Partita.sIdStatoOccupazione = DbValue<string>.Get(sqlRead["idstatooccupazione"]);
                        Partita.sDescrOccupazione = DbValue<string>.Get(sqlRead["descroccupazione"]);
                        Partita.IdCatAteco = DbValue<int>.Get(sqlRead["idcategoria"]);
                        Partita.sCatAteco = DbValue<string>.Get(sqlRead["cattares"]);
                        Partita.nNComponenti = DbValue<int>.Get(sqlRead["ncpf"]);
                        Partita.nComponentiPV = DbValue<int>.Get(sqlRead["ncpv"]);
                        Partita.nMQTassabili = doubleParser(sqlRead["mq"].ToString());
                        UtenteUtile = doubleParser(sqlRead["UtenteUtile"].ToString());
                        UtenteUtileLordo = doubleParser(sqlRead["UtenteUtileLordo"].ToString());
                        MQUtiliPF = doubleParser(sqlRead["mqutiliPF"].ToString());
                        MQUtiliPV = doubleParser(sqlRead["mqutiliPV"].ToString());
                        MQUtiliLordo = doubleParser(sqlRead["mqutililordo"].ToString());
                    }
                    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentRid = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                    CurrentRid.sCodice = DbValue<string>.Get(sqlRead["CODRID"]);
                    CurrentRid.sDescrizione = DbValue<string>.Get(sqlRead["DESCRRID"]);
                    if (CurrentRid.sCodice != string.Empty)
                    {
                        MyArrayRid.Add(CurrentRid);
                        Partita.oRiduzioni = MyArrayRid.ToArray();
                    }
                    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentDet = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                    CurrentDet.sCodice = DbValue<string>.Get(sqlRead["CODDET"]);
                    CurrentDet.sDescrizione = DbValue<string>.Get(sqlRead["DESCRDET"]);
                    if (CurrentDet.sCodice != string.Empty)
                    {
                        MyArrayDet.Add(CurrentDet);
                        Partita.oDetassazioni = MyArrayDet.ToArray();
                    }

                    idPrec = DbValue<int>.Get(sqlRead["id"]);
                }

                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override Articolo[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            Articolo item = new Articolo();
            int idPrec = 0;
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati> MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLI_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(IdEnte));
                sqlCmd.Parameters.AddWithValue("@Via", DbParam.Get(ParamSearch.sVia));
                sqlCmd.Parameters.AddWithValue("@Civico", DbParam.Get(ParamSearch.sCivico));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(ParamSearch.sFoglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(ParamSearch.sNumero));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(ParamSearch.sSubalterno));
                sqlCmd.Parameters.AddWithValue("@CatCatastale", DbParam.Get(ParamSearch.IdCatCatastale));
                sqlCmd.Parameters.AddWithValue("@Dal", DbParam.Get(dateTimeParser(ParamSearch.Dal.Replace("/",""))));
                sqlCmd.Parameters.AddWithValue("@Al", DbParam.Get(dateTimeParser(ParamSearch.Al.Replace("/", ""))));
                sqlCmd.Parameters.AddWithValue("@IdCategoria", DbParam.Get(ParamSearch.IdCatTARES));
                sqlCmd.Parameters.AddWithValue("@NC", DbParam.Get(ParamSearch.nComponenti));
                sqlCmd.Parameters.AddWithValue("@IdStatoOccupazione", DbParam.Get(ParamSearch.IdStatoOccupazione));
                sqlCmd.Parameters.AddWithValue("@TYPESOGRES", DbParam.Get(ParamSearch.TypeSogRes));
                sqlCmd.Parameters.AddWithValue("@IsPF", DbParam.Get(ParamSearch.IsPF));
                sqlCmd.Parameters.AddWithValue("@IsPV", DbParam.Get(ParamSearch.IsPV));
                sqlCmd.Parameters.AddWithValue("@IsEsente", DbParam.Get(ParamSearch.IsEsente));
                sqlCmd.Parameters.AddWithValue("@HasMoreUI", DbParam.Get(ParamSearch.HasMoreUI));
                sqlCmd.Parameters.AddWithValue("@IdRiduzione", DbParam.Get(ParamSearch.IdRiduzione));
                sqlCmd.Parameters.AddWithValue("@IdDetassazione", DbParam.Get(ParamSearch.IdDetassazione));
                Global.Log.Write2(LogSeverity.Debug, "query:: prc_TBLSIMULAZIONE_ARTICOLI_S");
                string sValParam = "";
                foreach (SqlParameter myparam in sqlCmd.Parameters)
                    sValParam += " parametro "+ myparam.ParameterName +"::" + myparam.Value.ToString();
                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                sqlRead = sqlCmd.ExecuteReader();
                List<Articolo> list = new List<Articolo>();
                while (sqlRead.Read())
                {
                    if (DbValue<int>.Get(sqlRead["id"]) != idPrec)
                    {
                        if (item.Partita.Id > 0)
                            list.Add(item);

                        item = new Articolo();
                        MyArrayRid = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
                        MyArrayDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati>();
                        item.Id = DbValue<int>.Get(sqlRead["id"]);
                        item.IdEnte = DbValue<string>.Get(sqlRead["idente"]);

                        item.Anagrafe.COD_CONTRIBUENTE = DbValue<int>.Get(sqlRead["IdContribuente"]);
                        item.Anagrafe.Cognome = DbValue<string>.Get(sqlRead["Cognome"]);
                        item.Anagrafe.Nome = DbValue<string>.Get(sqlRead["Nome"]);
                        item.Anagrafe.CodiceFiscale = DbValue<string>.Get(sqlRead["Cod_Fiscale"]);
                        item.Anagrafe.PartitaIva = DbValue<string>.Get(sqlRead["Partita_IVA"]);

                        item.Partita.Id = DbValue<int>.Get(sqlRead["fkidoggetto"]);
                        item.Partita.sCodVia = DbValue<string>.Get(sqlRead["idvia"]);
                        item.Partita.sVia = DbValue<string>.Get(sqlRead["via"]);
                        item.Partita.sCivico = DbValue<string>.Get(sqlRead["civico"]);
                        item.Partita.sEsponente = DbValue<string>.Get(sqlRead["esponente"]);
                        item.Partita.sScala = DbValue<string>.Get(sqlRead["scala"]);
                        item.Partita.sInterno = DbValue<string>.Get(sqlRead["interno"]);
                        item.Partita.sFoglio = DbValue<string>.Get(sqlRead["foglio"]);
                        item.Partita.sNumero = DbValue<string>.Get(sqlRead["numero"]);
                        item.Partita.sSubalterno = DbValue<string>.Get(sqlRead["subalterno"]);
                        item.Partita.tDataInizio = DbValue<DateTime>.Get(sqlRead["data_inizio"]);
                        item.Partita.tDataFine = DbValue<DateTime>.Get(sqlRead["data_fine"]);
                        item.Partita.sCatCatastale = DbValue<string>.Get(sqlRead["catcatastale"]);
                        item.Partita.sIdStatoOccupazione = DbValue<string>.Get(sqlRead["idstatooccupazione"]);
                        item.Partita.sDescrOccupazione = DbValue<string>.Get(sqlRead["descroccupazione"]);
                        item.Partita.IdCatAteco = DbValue<int>.Get(sqlRead["idcategoria"]);
                        item.Partita.sCatAteco = DbValue<string>.Get(sqlRead["cattares"]);
                        item.Partita.nNComponenti = DbValue<int>.Get(sqlRead["ncpf"]);
                        item.Partita.nComponentiPV = DbValue<int>.Get(sqlRead["ncpv"]);
                        item.Partita.nMQTassabili = doubleParser(sqlRead["mq"].ToString());
                        item.UtenteUtile = doubleParser(sqlRead["UtenteUtile"].ToString());
                        item.UtenteUtileLordo = doubleParser(sqlRead["UtenteUtileLordo"].ToString());
                        item.MQUtiliPF = doubleParser(sqlRead["mqutiliPF"].ToString());
                        item.MQUtiliPV = doubleParser(sqlRead["mqutiliPV"].ToString());
                        item.MQUtiliLordo = doubleParser(sqlRead["mqutililordo"].ToString());
                    }
                    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentRid = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                    CurrentRid.sCodice = DbValue<string>.Get(sqlRead["CODRID"]);
                    CurrentRid.sDescrizione = DbValue<string>.Get(sqlRead["DESCRRID"]);
                    if (CurrentRid.sCodice != string.Empty)
                    {
                        MyArrayRid.Add(CurrentRid);
                        item.Partita.oRiduzioni = MyArrayRid.ToArray();
                    }
                    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati CurrentDet = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati();
                    CurrentDet.sCodice = DbValue<string>.Get(sqlRead["CODDET"]);
                    CurrentDet.sDescrizione = DbValue<string>.Get(sqlRead["DESCRDET"]);
                    if (CurrentDet.sCodice != string.Empty)
                    {
                        MyArrayDet.Add(CurrentDet);
                        item.Partita.oDetassazioni = MyArrayDet.ToArray();
                    }

                    idPrec = DbValue<int>.Get(sqlRead["id"]);
                }
                if (item.Partita.Id > 0)
                    list.Add(item);

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyArrayRid"></param>
        /// <param name="MyArrayDet"></param>
        /// <returns></returns>
        public bool Save(RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati[] MyArrayRid,RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati[] MyArrayDet)
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLI_IU";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(Id));
                sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(IdEnte));
                sqlCmd.Parameters.AddWithValue("@FKIDOGGETTO", DbParam.Get(Partita.Id));
                sqlCmd.Parameters.AddWithValue("@IDCONTRIBUENTE", DbParam.Get(Anagrafe.COD_CONTRIBUENTE));
                sqlCmd.Parameters.AddWithValue("@IDVIA", DbParam.Get(Partita.sCodVia));
                sqlCmd.Parameters.AddWithValue("@VIA", DbParam.Get(Partita.sVia));
                sqlCmd.Parameters.AddWithValue("@CIVICO", DbParam.Get(Partita.sCivico));
                sqlCmd.Parameters.AddWithValue("@ESPONENTE", DbParam.Get(Partita.sEsponente));
                sqlCmd.Parameters.AddWithValue("@SCALA", DbParam.Get(Partita.sScala));
                sqlCmd.Parameters.AddWithValue("@INTERNO", DbParam.Get(Partita.sInterno));
                sqlCmd.Parameters.AddWithValue("@FOGLIO", DbParam.Get(Partita.sFoglio));
                sqlCmd.Parameters.AddWithValue("@NUMERO", DbParam.Get(Partita.sNumero));
                sqlCmd.Parameters.AddWithValue("@SUBALTERNO", DbParam.Get(Partita.sSubalterno));
                sqlCmd.Parameters.AddWithValue("@DATA_INIZIO", DbParam.Get(Partita.tDataInizio));
                sqlCmd.Parameters.AddWithValue("@DATA_FINE", DbParam.Get(Partita.tDataFine));
                sqlCmd.Parameters.AddWithValue("@IDSTATOOCCUPAZIONE", DbParam.Get(Partita.sIdStatoOccupazione));
                sqlCmd.Parameters.AddWithValue("@MQ", DbParam.Get(Partita.nMQTassabili));
                sqlCmd.Parameters.AddWithValue("@IDCATEGORIA", DbParam.Get(Partita.IdCatAteco));
                sqlCmd.Parameters.AddWithValue("@NCPF", DbParam.Get(Partita.nNComponenti));
                sqlCmd.Parameters.AddWithValue("@NCPV", DbParam.Get(Partita.nComponentiPV));
                sqlCmd.Parameters.AddWithValue("@IDCATEGORIAOLD", DbParam.Get(IdCatAtecoOld));
                sqlCmd.Parameters.AddWithValue("@NCPFOLD", DbParam.Get(nComponentiPFOld));
                sqlCmd.Parameters.AddWithValue("@NCPVOLD", DbParam.Get(nComponentiPVOld));
                sqlCmd.Parameters.AddWithValue("@UTENTEUTILE", DbParam.Get(UtenteUtile));
                sqlCmd.Parameters.AddWithValue("@UTENTEUTILELORDO", DbParam.Get(UtenteUtileLordo));
                sqlCmd.Parameters.AddWithValue("@MQUTILIPF", DbParam.Get(MQUtiliPF));
                sqlCmd.Parameters.AddWithValue("@MQUTILIPV", DbParam.Get(MQUtiliPV));
                sqlCmd.Parameters.AddWithValue("@MQUTILILORDO", DbParam.Get(MQUtiliLordo));
                sqlCmd.Parameters.AddWithValue("@OPERATORE", DbParam.Get(Partita.sOperatore));
                sqlCmd.Parameters.AddWithValue("@DATA_INSERIMENTO", DbParam.Get(DateTime.Now));
                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                string sValParam = "";
                foreach (SqlParameter myparam in sqlCmd.Parameters)
                    sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                Global.Log.Write2(LogSeverity.Debug, sValParam);
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                Id = (int)sqlCmd.Parameters["@ID"].Value;
                if (Id >= 0)
                {
                    if (MyArrayRid != null)
                    {
                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myRid in MyArrayRid)
                        {
                            if (myRid.sCodice != string.Empty)
                            {
                                if (myRid.sCodice == RidEseEnte.DELETERidEse)
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_D";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@IDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("R"));
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                }
                                else
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_IU";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(myRid.ID));
                                    sqlCmd.Parameters.AddWithValue("@FKIDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("R"));
                                    sqlCmd.Parameters.AddWithValue("@IDCODICE", DbParam.Get(myRid.sCodice));
                                    sqlCmd.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                    myRid.ID = (int)sqlCmd.Parameters["@Id"].Value;
                                    if (myRid.ID <= 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    if (MyArrayDet != null)
                    {
                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myDet in MyArrayDet)
                        {
                            if (myDet.sCodice != string.Empty)
                            {
                                if (myDet.sCodice == RidEseEnte.DELETERidEse)
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_D";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@IDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("D"));
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                }
                                else
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_IU";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(myDet.ID));
                                    sqlCmd.Parameters.AddWithValue("@FKIDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("D"));
                                    sqlCmd.Parameters.AddWithValue("@IDCODICE", DbParam.Get(myDet.sCodice));
                                    sqlCmd.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                    myDet.ID = (int)sqlCmd.Parameters["@Id"].Value;
                                    if (myDet.ID <= 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLI_IU";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(Id));
                sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(IdEnte));
                sqlCmd.Parameters.AddWithValue("@FKIDOGGETTO", DbParam.Get(Partita.Id));
                sqlCmd.Parameters.AddWithValue("@IDCONTRIBUENTE", DbParam.Get(Anagrafe.COD_CONTRIBUENTE));
                sqlCmd.Parameters.AddWithValue("@IDVIA", DbParam.Get(Partita.sCodVia));
                sqlCmd.Parameters.AddWithValue("@VIA", DbParam.Get(Partita.sVia));
                sqlCmd.Parameters.AddWithValue("@CIVICO", DbParam.Get(Partita.sCivico));
                sqlCmd.Parameters.AddWithValue("@ESPONENTE", DbParam.Get(Partita.sEsponente));
                sqlCmd.Parameters.AddWithValue("@SCALA", DbParam.Get(Partita.sScala));
                sqlCmd.Parameters.AddWithValue("@INTERNO", DbParam.Get(Partita.sInterno));
                sqlCmd.Parameters.AddWithValue("@FOGLIO", DbParam.Get(Partita.sFoglio));
                sqlCmd.Parameters.AddWithValue("@NUMERO", DbParam.Get(Partita.sNumero));
                sqlCmd.Parameters.AddWithValue("@SUBALTERNO", DbParam.Get(Partita.sSubalterno));
                sqlCmd.Parameters.AddWithValue("@DATA_INIZIO", DbParam.Get(Partita.tDataInizio));
                sqlCmd.Parameters.AddWithValue("@DATA_FINE", DbParam.Get(Partita.tDataFine));
                sqlCmd.Parameters.AddWithValue("@IDSTATOOCCUPAZIONE", DbParam.Get(Partita.sIdStatoOccupazione));
                sqlCmd.Parameters.AddWithValue("@MQ", DbParam.Get(Partita.nMQTassabili));
                sqlCmd.Parameters.AddWithValue("@IDCATEGORIA", DbParam.Get(Partita.IdCatAteco));
                sqlCmd.Parameters.AddWithValue("@NCPF", DbParam.Get(Partita.nNComponenti));
                sqlCmd.Parameters.AddWithValue("@NCPV", DbParam.Get(Partita.nComponentiPV));
                sqlCmd.Parameters.AddWithValue("@IDCATEGORIAOLD", DbParam.Get(IdCatAtecoOld));
                sqlCmd.Parameters.AddWithValue("@NCPFOLD", DbParam.Get(nComponentiPFOld));
                sqlCmd.Parameters.AddWithValue("@NCPVOLD", DbParam.Get(nComponentiPVOld));
                sqlCmd.Parameters.AddWithValue("@UTENTEUTILE", DbParam.Get(UtenteUtile));
                sqlCmd.Parameters.AddWithValue("@UTENTEUTILELORDO", DbParam.Get(UtenteUtileLordo));
                sqlCmd.Parameters.AddWithValue("@MQUTILIPF", DbParam.Get(MQUtiliPF));
                sqlCmd.Parameters.AddWithValue("@MQUTILIPV", DbParam.Get(MQUtiliPV));
                sqlCmd.Parameters.AddWithValue("@MQUTILILORDO", DbParam.Get(MQUtiliLordo));
                sqlCmd.Parameters.AddWithValue("@OPERATORE", DbParam.Get(Partita.sOperatore));
                sqlCmd.Parameters.AddWithValue("@DATA_INSERIMENTO", DbParam.Get(DateTime.Now));
                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                string sValParam = "";
                foreach (SqlParameter myparam in sqlCmd.Parameters)
                    sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                Global.Log.Write2(LogSeverity.Debug, sValParam);
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                Id = (int)sqlCmd.Parameters["@ID"].Value;
                if (Id >= 0)
                {
                    if (Partita.oRiduzioni != null)
                    {
                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myRid in Partita.oRiduzioni)
                        {
                            if (myRid.sCodice != string.Empty)
                            {
                                if (myRid.sCodice == RidEseEnte.DELETERidEse)
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_D";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@IDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("R"));
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                }
                                else
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_IU";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(myRid.ID));
                                    sqlCmd.Parameters.AddWithValue("@FKIDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("R"));
                                    sqlCmd.Parameters.AddWithValue("@IDCODICE", DbParam.Get(myRid.sCodice));
                                    sqlCmd.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                    myRid.ID = (int)sqlCmd.Parameters["@Id"].Value;
                                    if (myRid.ID <= 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    if (Partita.oDetassazioni != null)
                    {
                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEseApplicati myDet in Partita.oDetassazioni)
                        {
                            if (myDet.sCodice != string.Empty)
                            {
                                if (myDet.sCodice == RidEseEnte.DELETERidEse)
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_D";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@IDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("D"));
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                }
                                else
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_IU";
                                    sqlCmd.Parameters.Clear();
                                    sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                                    sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(myDet.ID));
                                    sqlCmd.Parameters.AddWithValue("@FKIDARTICOLO", DbParam.Get(Id));
                                    sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get("D"));
                                    sqlCmd.Parameters.AddWithValue("@IDCODICE", DbParam.Get(myDet.sCodice));
                                    sqlCmd.Parameters["@Id"].Direction = ParameterDirection.InputOutput;
                                    Global.Log.Write2(LogSeverity.Debug, "query:: " + sqlCmd.CommandText);
                                    sValParam = "";
                                    foreach (SqlParameter myparam in sqlCmd.Parameters)
                                        sValParam += " parametro " + myparam.ParameterName + "::" + myparam.Value.ToString();
                                    Global.Log.Write2(LogSeverity.Debug, sValParam);
                                    if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                                    myDet.ID = (int)sqlCmd.Parameters["@Id"].Value;
                                    if (myDet.ID <= 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLI_D";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@Id", DbParam.Get(IdSimulazione));
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
        #endregion
        public bool DeleteRidEse()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_ARTICOLIRIDDET_D";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@FKIDSIMULAZIONE", DbParam.Get(IdSimulazione));
                sqlCmd.Parameters.AddWithValue("@IDARTICOLO", DbParam.Get(Id));
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }
    }
    /// <summary>
    /// Classe per la gestione dei totali della simulazione
    /// </summary>
    public class TotaliSimulazione : DbObject<TotaliSimulazione>
    {
        #region Variables and constructor
        public TotaliSimulazione()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string IdCategoria { get; set; }
        public string DescrCategoria { get; set; }
        public int nComponentiPF { get; set; }
        public int nComponentiPV { get; set; }
        public string IdRiduzione { get; set; }
        public string DescrRiduzione { get; set; }
        public string IdDetassazione { get; set; }
        public string DescrDetassazione { get; set; }
        public int nUtenze { get; set; }
        public double nMQ { get; set; }
        public double UtenzeUtili { get; set; }
        public double MQUtiliPF { get; set; }
        public double MQUtiliPV { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is TotaliSimulazione) &&
                ((obj as TotaliSimulazione).IdSimulazione == IdSimulazione);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdSimulazione);
        }

        public override sealed void Reset()
        {
            IdSimulazione = default(int);
            FromVariabile = default(string);
            IdCategoria = default(string);
            DescrCategoria = default(string);
            nComponentiPF = default(int);
            nComponentiPV = default(int);
            IdRiduzione = default(string);
            DescrRiduzione = default(string);
            IdDetassazione = default(string);
            DescrDetassazione = default(string);
            nUtenze = default(int);
            nMQ = default(double);
            UtenzeUtili = default(double);
            MQUtiliPF = default(double);
            MQUtiliPV = default(double);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override TotaliSimulazione[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetSIMULATotali";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                Global.Log.Write2(LogSeverity.Debug, myTypeConn + " prc_GetSIMULATotali " + IdSimulazione.ToString());
                sqlRead = sqlCmd.ExecuteReader();
                List<TotaliSimulazione> list = new List<TotaliSimulazione>();
                while (sqlRead.Read())
                {
                    TotaliSimulazione item = new TotaliSimulazione();
                    item.IdSimulazione = DbValue<int>.Get(sqlRead["fkIdSimulazione"]);
                    item.IdCategoria = DbValue<string>.Get(sqlRead["IdCategoria"]);
                    item.DescrCategoria = DbValue<string>.Get(sqlRead["DescrCat"]);
                    item.nComponentiPF = DbValue<int>.Get(sqlRead["NCPF"]);
                    item.nComponentiPV = DbValue<int>.Get(sqlRead["NCPV"]);
                    item.IdRiduzione = DbValue<string>.Get(sqlRead["codrid"]);
                    item.DescrRiduzione = DbValue<string>.Get(sqlRead["DescrRid"]);
                    item.IdDetassazione = DbValue<string>.Get(sqlRead["CodDet"]);
                    item.DescrDetassazione = DbValue<string>.Get(sqlRead["DescrDet"]);
                    item.nUtenze = DbValue<int>.Get(sqlRead["Utenze"]);
                    item.nMQ = doubleParser(sqlRead["MQ"].ToString());
                    item.UtenzeUtili = doubleParser(sqlRead["utenzeutili"].ToString());
                    item.MQUtiliPF = doubleParser(sqlRead["MQutiliPF"].ToString());
                    item.MQUtiliPV = doubleParser(sqlRead["MQutiliPV"].ToString());
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei KA della simulazione
    /// </summary>
    public class CoefficienteKA : DbObject<CoefficienteKA>
    {
        #region Variables and constructor
        public CoefficienteKA()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public int IdCategoria { get; set; }
        public string DescrCategoria { get; set; }
        public int NC { get; set; }
        public int TipoComune { get; set; }
        public double Nord { get; set; }
        public double Sud { get; set; }
        public double Centro { get; set; }
        public int IdUsato { get; set; }
        public double CoefficienteUsato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is CoefficienteKA) &&
                ((obj as CoefficienteKA).IdUsato == IdUsato);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUsato);
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            Ente = default(string);
            IdCategoria = default(int);
            DescrCategoria = default(string);
            NC = default(int);
            TipoComune = default(int);
            Nord = default(double);
            Sud = default(double);
            Centro = default(double);
            IdUsato = default(int);
            CoefficienteUsato = default(double);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override CoefficienteKA[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";
            CoefficienteKA item = new CoefficienteKA();

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KA_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                string sValParam = "";
                sValParam = sqlCmd.CommandText+" ";
                foreach (SqlParameter myparam in sqlCmd.Parameters)
                    sValParam += " ," + myparam.ParameterName + "='" + myparam.Value.ToString()+"'";
                Global.Log.Write2(LogSeverity.Debug, sValParam);
                sqlRead = sqlCmd.ExecuteReader();
                List<CoefficienteKA> list = new List<CoefficienteKA>();
                while (sqlRead.Read())
                {
                    item = new CoefficienteKA();
                    item.Id = DbValue<int>.Get(sqlRead["ID"]);
                    item.IdCategoria = DbValue<int>.Get(sqlRead["FKIdCategoria"]);
                    item.DescrCategoria = DbValue<string>.Get(sqlRead["DescrCategoria"]);
                    item.NC = DbValue<int>.Get(sqlRead["nc"]);
                    item.TipoComune = DbValue<int>.Get(sqlRead["FKIDType"]);
                    item.Nord = doubleParser(sqlRead["Nord"].ToString());
                    item.Sud = doubleParser(sqlRead["Sud"].ToString());
                    item.Centro = doubleParser(sqlRead["Centro"].ToString());
                    item.IdUsato = DbValue<int>.Get(sqlRead["IDUsato"]);
                    item.CoefficienteUsato = doubleParser(sqlRead["CoefficienteUsato"].ToString());
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KA_IU";
                sqlCmd.Parameters.AddWithValue("@IdUsato", DbParam.Get(IdUsato));
                sqlCmd.Parameters.AddWithValue("@Valore", DbParam.Get(CoefficienteUsato));
                
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei KB della simulazione
    /// </summary>
    public class CoefficienteKB : DbObject<CoefficienteKB>
    {
        #region Variables and constructor
        public CoefficienteKB()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public int IdCategoria { get; set; }
        public string DescrCategoria { get; set; }
        public int NC { get; set; }
        public int TipoComune { get; set; }
        public double Minimo { get; set; }
        public double Massimo { get; set; }
        public double Medio { get; set; }
        public int IdUsato { get; set; }
        public double CoefficienteUsato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is CoefficienteKB) &&
                ((obj as CoefficienteKB).IdUsato == IdUsato);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUsato);
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            Ente = default(string);
            IdCategoria = default(int);
            DescrCategoria = default(string);
            NC = default(int);
            TipoComune = default(int);
            Minimo = default(double);
            Massimo = default(double);
            Medio = default(double);
            IdUsato = default(int);
            CoefficienteUsato = default(double);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override CoefficienteKB[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";
            CoefficienteKB item = new CoefficienteKB();

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KB_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlRead = sqlCmd.ExecuteReader();
                List<CoefficienteKB> list = new List<CoefficienteKB>();
                while (sqlRead.Read())
                {
                    item = new CoefficienteKB();
                    item.Id = DbValue<int>.Get(sqlRead["ID"]);
                    item.IdCategoria = DbValue<int>.Get(sqlRead["FKIdCategoria"]);
                    item.DescrCategoria = DbValue<string>.Get(sqlRead["DescrCategoria"]);
                    item.NC = DbValue<int>.Get(sqlRead["nc"]);
                    item.TipoComune = DbValue<int>.Get(sqlRead["FKIDType"]);
                    item.Minimo = doubleParser(sqlRead["Minimo"].ToString());
                    item.Massimo = doubleParser(sqlRead["Massimo"].ToString());
                    item.Medio = doubleParser(sqlRead["Medio"].ToString());
                    item.IdUsato = DbValue<int>.Get(sqlRead["IDUsato"]);
                    item.CoefficienteUsato = doubleParser(sqlRead["CoefficienteUsato"].ToString());
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KB_IU";
                sqlCmd.Parameters.AddWithValue("@IdUsato", DbParam.Get(IdUsato));
                sqlCmd.Parameters.AddWithValue("@Valore", DbParam.Get(CoefficienteUsato));

                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei KC della simulazione
    /// </summary>
    public class CoefficienteKC : DbObject<CoefficienteKC>
    {
        #region Variables and constructor
        public CoefficienteKC()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public bool IsKCMax { get; set; }
        public int IdCategoria { get; set; }
        public string DescrCategoria { get; set; }
        public int TipoComune { get; set; }
        public double NordMin { get; set; }
        public double SudMin { get; set; }
        public double CentroMin { get; set; }
        public double NordMax { get; set; }
        public double SudMax { get; set; }
        public double CentroMax { get; set; }
        public int IdUsato { get; set; }
        public double CoefficienteUsato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is CoefficienteKC) &&
                ((obj as CoefficienteKC).IdUsato == IdUsato);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUsato);
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            IsKCMax = default(bool);
            Ente = default(string);
            IdCategoria = default(int);
            DescrCategoria = default(string);
            TipoComune = default(int);
            NordMin = default(double);
            SudMin = default(double);
            CentroMin = default(double);
            NordMax = default(double);
            SudMax = default(double);
            CentroMax = default(double);
            IdUsato = default(int);
            CoefficienteUsato = default(double);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override CoefficienteKC[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";
            CoefficienteKC item = new CoefficienteKC();

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KC_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@IsKCMax", DbParam.Get(IsKCMax));
                sqlRead = sqlCmd.ExecuteReader();
                List<CoefficienteKC> list = new List<CoefficienteKC>();
                while (sqlRead.Read())
                {
                    item = new CoefficienteKC();
                    item.Id = DbValue<int>.Get(sqlRead["ID"]);
                    item.IdCategoria = DbValue<int>.Get(sqlRead["FKIdCategoria"]);
                    item.DescrCategoria = DbValue<string>.Get(sqlRead["DescrCategoria"]);
                    item.TipoComune = DbValue<int>.Get(sqlRead["FKIDType"]);
                    item.NordMin = doubleParser(sqlRead["Nord_Min"].ToString());
                    item.SudMin = doubleParser(sqlRead["Sud_Min"].ToString());
                    item.CentroMin = doubleParser(sqlRead["Centro_Min"].ToString());
                    item.NordMax = doubleParser(sqlRead["Nord_Max"].ToString());
                    item.SudMax = doubleParser(sqlRead["Sud_Max"].ToString());
                    item.CentroMax = doubleParser(sqlRead["Centro_Max"].ToString());
                    item.IdUsato = DbValue<int>.Get(sqlRead["IDUsato"]);
                    item.CoefficienteUsato = doubleParser(sqlRead["CoefficienteUsato"].ToString());
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KC_IU";
                sqlCmd.Parameters.AddWithValue("@IdUsato", DbParam.Get(IdUsato));
                sqlCmd.Parameters.AddWithValue("@Valore", DbParam.Get(CoefficienteUsato));

                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei KD della simulazione
    /// </summary>
    public class CoefficienteKD : DbObject<CoefficienteKD>
    {
        #region Variables and constructor
        public CoefficienteKD()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string Ente { get; set; }
        public bool IsKDMax { get; set; }
        public int IdCategoria { get; set; }
        public string DescrCategoria { get; set; }
        public int TipoComune { get; set; }
        public double NordMin { get; set; }
        public double SudMin { get; set; }
        public double CentroMin { get; set; }
        public double NordMax { get; set; }
        public double SudMax { get; set; }
        public double CentroMax { get; set; }
        public int IdUsato { get; set; }
        public double CoefficienteUsato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is CoefficienteKD) &&
                ((obj as CoefficienteKD).IdUsato == IdUsato);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdUsato);
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            Ente = default(string);
            IsKDMax = default(bool);
            IdCategoria = default(int);
            DescrCategoria = default(string);
            TipoComune = default(int);
            NordMin = default(double);
            SudMin = default(double);
            CentroMin = default(double);
            NordMax = default(double);
            SudMax = default(double);
            CentroMax = default(double);
            IdUsato = default(int);
            CoefficienteUsato = default(double);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override CoefficienteKD[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string myTypeConn = "OPENgovTARSU";
            CoefficienteKD item = new CoefficienteKD();

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KD_S";
                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@IsKDMax", DbParam.Get(IsKDMax));
                sqlRead = sqlCmd.ExecuteReader();
                List<CoefficienteKD> list = new List<CoefficienteKD>();
                while (sqlRead.Read())
                {
                    item = new CoefficienteKD();
                    item.Id = DbValue<int>.Get(sqlRead["ID"]);
                    item.IdCategoria = DbValue<int>.Get(sqlRead["FKIdCategoria"]);
                    item.DescrCategoria = DbValue<string>.Get(sqlRead["DescrCategoria"]);
                    item.TipoComune = DbValue<int>.Get(sqlRead["FKIDType"]);
                    item.NordMin = doubleParser(sqlRead["Nord_Min"].ToString());
                    item.SudMin = doubleParser(sqlRead["Sud_Min"].ToString());
                    item.CentroMin = doubleParser(sqlRead["Centro_Min"].ToString());
                    item.NordMax = doubleParser(sqlRead["Nord_Max"].ToString());
                    item.SudMax = doubleParser(sqlRead["Sud_Max"].ToString());
                    item.CentroMax = doubleParser(sqlRead["Centro_Max"].ToString());
                    item.IdUsato = DbValue<int>.Get(sqlRead["IDUsato"]);
                    item.CoefficienteUsato = doubleParser(sqlRead["CoefficienteUsato"].ToString());
                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }

        public override bool Save()
        {
            SqlCommand sqlCmd = null;
            string myTypeConn = "OPENgovTARSU";

            try
            {
                if (FromVariabile != string.Empty || FromVariabile == "1")
                {
                    myTypeConn = "OPENgovTIA";
                }
                Connect(myTypeConn);
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_KD_IU";
                sqlCmd.Parameters.AddWithValue("@IdUsato", DbParam.Get(IdUsato));
                sqlCmd.Parameters.AddWithValue("@Valore", DbParam.Get(CoefficienteUsato));

                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return false;
            }
            finally
            {
                Disconnect(sqlCmd);
            }
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione della definizione tariffe della simulazione
    /// </summary>
    public class ToDefineTariffe : DbObject<ToDefineTariffe>
    {
        #region Variables and constructor
        public ToDefineTariffe()
        {
            Reset();
        }
        #endregion

        #region Public properties
        public int Id { get; set; }
        public int IdSimulazione { get; set; }
        public string FromVariabile { get; set; }
        public string Tipo { get; set; }
        public int IdCategoria { get; set; }
        public string Descrizione { get; set; }
        public int NC { get; set; }
        public double MQ { get; set; }
        public double CoeffK { get; set; }
        public double MQNormalizzati { get; set; }
        public double CU { get; set; }
        public double QUF { get; set; }
        public double Tariffa { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is ToDefineTariffe) &&
                ((obj as ToDefineTariffe).IdCategoria == IdCategoria) &&
                ((obj as ToDefineTariffe).NC == NC);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdCategoria, NC);
        }

        public override sealed void Reset()
        {
            Id = default(int);
            IdSimulazione = default(int);
            FromVariabile = default(string);
            Tipo = default(string);
            IdCategoria = default(int);
            Descrizione = default(string);
            NC = default(int);
            MQ = default(double);
            CoeffK = default(double);
            MQNormalizzati = default(double);
            CU = default(double);
            QUF = default(double);
            Tariffa = default(double);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override ToDefineTariffe[] LoadAll()
        {
            throw new NotImplementedException();
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
