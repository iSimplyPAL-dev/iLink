using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita dello stato lavori
    /// </summary>
    public class StatoLavoroAtti : DbObject<StatoLavoroAtti>
    {
        #region Public properties
        public string Ente { get; set; }
        public int IdStato { get; set; }
        public string Definizione { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdStato);
        }

        public override sealed void Reset()
        {
            IdStato = default(int);
            Definizione = default(string);
            Ente = default(string);
        }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Save(ref string error)
        {
            throw new NotImplementedException();
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }

        public override StatoLavoroAtti[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetStatoLavoroAtti";
                sqlRead = sqlCmd.ExecuteReader();

                List<StatoLavoroAtti> list = new List<StatoLavoroAtti>();
                while (sqlRead.Read())
                {
                    StatoLavoroAtti item = new StatoLavoroAtti();
                    item.IdStato = DbValue<int>.Get(sqlRead["IdStato"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["Definizione"]);
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

        public StatoOccupazione[] LoadTipoImmobile()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetTipoImmobile";
                sqlRead = sqlCmd.ExecuteReader();

                List<StatoOccupazione> list = new List<StatoOccupazione>();
                while (sqlRead.Read())
                {
                    StatoOccupazione item = new StatoOccupazione();
                    item.Codice = DbValue<string>.Get(sqlRead["IDTIPOIMMOBILE"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["DESCRIZIONE"]);
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
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita
    /// </summary>
    public class CompraVendita : DbObject<CompraVendita>
    {
        #region Public properties
        public int IdAtto { get; set; }
        public int IdImmobile { get; set; }
        public string Ente { get; set; }
        public string RifCatastali { get; set; }
        public string Ubicazione { get; set; }
        public string DataValidita { get; set; }
        public string DataPresentazione { get; set; }
        public string NumNota { get; set; }
        public string DescrAtto { get; set; }
        public string Stato { get; set; }
        public string NotaTrascrizione { get; set; }
        public string RifNota { get; set; }
        public string CatNota { get; set; }
        public string UbicazioneNota { get; set; }
        public string UbicazioneCatasto { get; set; }
        public string Nominativo { get; set; }
        public string CFPIVA { get; set; }
        public string Diritto { get; set; }
        public string TipoImmobile { get; set; }
        public SoggettiNota[] ListAcquirenti { get; set; }
        public SoggettiNota[] ListCessionari { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdAtto);
        }

        public override sealed void Reset()
        {
            IdAtto = default(int);
            IdImmobile = default(int);
            Ente = default(string);
            RifCatastali = default(string);
            Ubicazione = default(string);
            DataValidita = default(string);
            DataPresentazione = default(string);
            NumNota = default(string);
            DescrAtto = default(string);
            Stato = default(string);
            NotaTrascrizione = default(string);
            RifNota = default(string);
            CatNota = default(string);
            UbicazioneNota = default(string);
            UbicazioneCatasto = default(string);
            Nominativo = default(string);
            CFPIVA = default(string);
            Diritto = default(string);
            TipoImmobile = default(string);
            ListAcquirenti = null;
            ListCessionari = null;
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

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetAttiCompravendita";
                sqlCmd.Parameters.AddWithValue("@idImmobile", DbParam.Get(IdImmobile));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    IdAtto = DbValue<int>.Get(sqlRead["IdAtto"]);
                    IdImmobile = DbValue<int>.Get(sqlRead["IdImmobile"]);
                    RifCatastali = DbValue<string>.Get(sqlRead["RifCatastali"]);
                    Ubicazione = DbValue<string>.Get(sqlRead["Ubicazione"]);
                    DataValidita = DbValue<string>.Get(sqlRead["DataValidita"]);
                    DataPresentazione = DbValue<string>.Get(sqlRead["DataPresentazione"]);
                    NumNota = DbValue<string>.Get(sqlRead["NumNota"]);
                    DescrAtto = DbValue<string>.Get(sqlRead["DescrAtto"]);
                    Stato = DbValue<string>.Get(sqlRead["Stato"]);
                    NotaTrascrizione = DbValue<string>.Get(sqlRead["NotaTrascrizione"]);
                    RifNota = DbValue<string>.Get(sqlRead["RifNota"]);
                    CatNota = DbValue<string>.Get(sqlRead["CatNota"]);
                    UbicazioneNota = DbValue<string>.Get(sqlRead["UbicazioneNota"]);
                    UbicazioneCatasto = DbValue<string>.Get(sqlRead["UbicazioneCatasto"]);                   
                    ListAcquirenti = new SoggettiNota { Tipo = "A", IdImmobile = IdImmobile, Diritto="", IdSoggetto=0, idStato=-1, CfPIva="", Nominativo="" }.LoadAll();
                    ListCessionari = new SoggettiNota { Tipo = "C", IdImmobile = IdImmobile, Diritto = "", IdSoggetto = 0, idStato = -1, CfPIva = "", Nominativo = "" }.LoadAll();
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

        public override CompraVendita[] LoadAll()
        {
            DataSet data = new DataSet();
            return LoadAtti(Ente,-1,-1,-1,"","","","","","", 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEnte"></param>
        /// <param name="IdAtto"></param>
        /// <param name="idImmobile"></param>
        /// <param name="Stato"></param>
        /// <param name="Foglio"></param>
        /// <param name="Numero"></param>
        /// <param name="Subalterno"></param>
        /// <param name="Ubicazione"></param>
        /// <param name="Nominativo"></param>
        /// <param name="TipoImmobile"></param>
        /// <param name="IsSoggetto"></param>
        /// <returns></returns>
        public CompraVendita[] LoadAtti(string idEnte, int IdAtto, int idImmobile, int Stato, string Foglio, string Numero, string Subalterno, string Ubicazione, string Nominativo, string TipoImmobile, int IsSoggetto)
        {            
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;


            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetAttiCompravendita";
                sqlCmd.Parameters.AddWithValue("@idAtto", DbParam.Get(IdAtto));
                sqlCmd.Parameters.AddWithValue("@idImmobile", DbParam.Get(idImmobile));
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                sqlCmd.Parameters.AddWithValue("@Stato", DbParam.Get(Stato));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@Ubicazione", DbParam.Get(Ubicazione));
                sqlCmd.Parameters.AddWithValue("@Nominativo", DbParam.Get(Nominativo));
                sqlCmd.Parameters.AddWithValue("@TipoImmobile", DbParam.Get(TipoImmobile));
                sqlCmd.Parameters.AddWithValue("@IsSoggetto", DbParam.Get(IsSoggetto));
                sqlRead = sqlCmd.ExecuteReader();                 

                List<CompraVendita> list = new List<CompraVendita>();
                while (sqlRead.Read())          
                {
                    CompraVendita item = new CompraVendita();
                    item.IdAtto = DbValue<int>.Get(sqlRead["IdAtto"]);
                    item.IdImmobile = DbValue<int>.Get(sqlRead["IdImmobile"]);
                    item.RifCatastali = DbValue<string>.Get(sqlRead["RifCatastali"]);
                    item.Ubicazione = DbValue<string>.Get(sqlRead["Ubicazione"]);
                    item.DataValidita = DbValue<string>.Get(sqlRead["DataValidita"]);
                    item.DataPresentazione = DbValue<string>.Get(sqlRead["DataPresentazione"]);
                    item.NumNota = DbValue<string>.Get(sqlRead["NumNota"]);
                    item.DescrAtto = DbValue<string>.Get(sqlRead["DescrAtto"]);
                    item.Stato = DbValue<string>.Get(sqlRead["Stato"]);
                    item.Nominativo = DbValue<string>.Get(sqlRead["Nominativo"]);
                    item.CFPIVA = DbValue<string>.Get(sqlRead["CFPIVA"]);
                    item.Diritto = DbValue<string>.Get(sqlRead["Diritto"]);
                    item.TipoImmobile = DbValue<string>.Get(sqlRead["TipoImmobile"]);
                    //item.IsSoggetto = DbValue<int>.Get(sqlRead["IsSoggetto"]);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEnte"></param>
        /// <param name="Foglio"></param>
        /// <param name="Numero"></param>
        /// <param name="Subalterno"></param>
        /// <param name="Ubicazione"></param>
        /// <param name="Nominativo"></param>
        /// <param name="TipoImmobile"></param>
        /// <param name="Stato"></param>
        /// <returns></returns>
        public DataSet LoadAttiStampa(string idEnte, string Foglio, string Numero, string Subalterno, string Ubicazione, string Nominativo, string TipoImmobile,int Stato)
        {
            SqlCommand sqlCmd = new SqlCommand();            
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            DataSet myResult = new DataSet();

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetAttiCompravenditaStampa";
                sqlCmd.Parameters.AddWithValue("@idEnte", DbParam.Get(idEnte));
                sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
                sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
                sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
                sqlCmd.Parameters.AddWithValue("@Ubicazione", DbParam.Get(Ubicazione));
                sqlCmd.Parameters.AddWithValue("@Nominativo", DbParam.Get(Nominativo));
                sqlCmd.Parameters.AddWithValue("@TipoImmobile", DbParam.Get(TipoImmobile));
                sqlCmd.Parameters.AddWithValue("@Stato", DbParam.Get(Stato));
                myAdapter = new SqlDataAdapter(sqlCmd);
                myAdapter.Fill(myResult, "c");

                return myResult;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return null;
            }
            finally
            {
                Disconnect(sqlCmd, myResult);
            }

        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Compravendita dei soggetti
    /// </summary>
    public class SoggettiNota : DbObject<SoggettiNota>
    {
        #region Public properties
        public string Tipo { get; set; }
        public int IdSoggetto { get; set; }
        public int IdImmobile { get; set; }
        public string Nominativo { get; set; }
        public string CfPIva { get; set; }
        public string Diritto { get; set; }
        public string Stato { get; set; }
        public int idStato { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdSoggetto);
        }

        public override sealed void Reset()
        {
            Tipo = default(string);
            IdSoggetto = default(int);
            IdImmobile = default(int);
            Tipo = default(string);
            CfPIva = default(string);
            Diritto = default(string);
            Stato = default(string);
            idStato = default(int);
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

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetAttiSoggettiNota";
                sqlCmd.Parameters.AddWithValue("@IdSoggetto", DbParam.Get(IdSoggetto));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    Tipo = DbValue<string>.Get(sqlRead["Tipo"]);
                    IdSoggetto = DbValue<int>.Get(sqlRead["IdSoggetto"]);
                    IdImmobile = DbValue<int>.Get(sqlRead["IdAtto"]);
                    Nominativo = DbValue<string>.Get(sqlRead["Nominativo"]);
                    CfPIva = DbValue<string>.Get(sqlRead["CfPIva"]);
                    Diritto = DbValue<string>.Get(sqlRead["Diritto"]);
                    Stato = DbValue<string>.Get(sqlRead["Stato"]);
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

        public override SoggettiNota[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_GetAttiSoggettiNota";
                sqlCmd.Parameters.AddWithValue("@Tipo", DbParam.Get(Tipo));
                sqlCmd.Parameters.AddWithValue("@IdSoggetto", DbParam.Get(IdSoggetto));
                sqlCmd.Parameters.AddWithValue("@idAtto", DbParam.Get(IdImmobile));
                sqlCmd.Parameters.AddWithValue("@Stato", DbParam.Get(idStato));
                sqlCmd.Parameters.AddWithValue("@CfPIva", DbParam.Get(CfPIva));
                sqlCmd.Parameters.AddWithValue("@Nominativo", DbParam.Get(Nominativo));
                sqlRead = sqlCmd.ExecuteReader();

                List<SoggettiNota> list = new List<SoggettiNota>();
                while (sqlRead.Read())
                {
                    SoggettiNota item = new SoggettiNota();
                    item.Tipo = DbValue<string>.Get(sqlRead["Tipo"]);
                    item.IdSoggetto = DbValue<int>.Get(sqlRead["IdSoggetto"]);
                    item.IdImmobile = DbValue<int>.Get(sqlRead["IdAtto"]);
                    item.Nominativo = DbValue<string>.Get(sqlRead["Nominativo"]);
                    item.CfPIva = DbValue<string>.Get(sqlRead["CfPIva"]);
                    item.Diritto = DbValue<string>.Get(sqlRead["Diritto"]);
                    item.Stato = DbValue<string>.Get(sqlRead["Stato"]);
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
        #endregion
    }
}
