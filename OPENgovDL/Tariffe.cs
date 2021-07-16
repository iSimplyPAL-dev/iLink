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
    /// Classe per la gestione delle tariffe dell'ente
    /// </summary>
    public class TariffeEnte : DbObject<TariffeEnte>
    {
        public const int TypeTassazione_TariffeTributo = 1;
        public const int TypeTassazione_TariffeMaggiorazioni = 2;
        public const int TypeTassazione_TariffeConferimenti = 3;

        #region Variables and constructor
        public TariffeEnte()
        {
            Reset();
        }

        public TariffeEnte(int idTariffa)
        {
            Reset();
            IdTariffa = idTariffa;
        }
        #endregion

        #region Public properties
        public string FromVariabile { get; set; }
        public int IdTariffa { get; set; }
        public string Ente { get; set; }
        public string Anno { get; set; }
        public int TypeTassazione { get; set; }
        public string Descrizione { get; set; }
        public string IdCategoria { get; set; }
        public double impPF { get; set; }
        public double impPV { get; set; }
        public string nComponenti { get; set; }
        public double MoltiplicatoreMinimo { get; set; }
        public double impMinimo { get; set; }
        public string TypeCalcolo { get; set; }
        public int TypeUtenze { get; set; }
        public bool IsNewAnno { get; set; }
       //*** 201712 - gestione tipo conferimento ***
       public string TipoConferimento { get; set; }
        public string IDTipoConferimento { get; set; }
        //*** ***
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is TariffeEnte) &&
                ((obj as TariffeEnte).IdTariffa == IdTariffa);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdTariffa);
        }

        public override sealed void Reset()
        {
            FromVariabile = default(string);
            IdTariffa = default(int);
            Ente = default(string);
            Anno = default(string);
            TypeTassazione = default(int);
            Descrizione = default(string);
            IdCategoria = default(string);
            impPF = default(double);
            impPV = default(double);
            nComponenti = "00";
            MoltiplicatoreMinimo = 1;
            impMinimo = default(double);
            TypeCalcolo = default(string);
            TypeUtenze = -1;
            IsNewAnno = false;
            //*** 201712 - gestione tipo conferimento ***
            TipoConferimento = default(string);
            //*** ***
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFE_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TypeCalcolo", DbParam.Get(TypeCalcolo));
                sqlCmd.Parameters.AddWithValue("@IdTariffa", DbParam.Get(IdTariffa));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    IdTariffa = DbValue<int>.Get(sqlRead["IdTariffa"]);
                    Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    Anno = DbValue<string>.Get(sqlRead["Anno"]);
                    TypeTassazione = DbValue<int>.Get(sqlRead["TypeTassazione"]);
                    Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    IdCategoria = DbValue<string>.Get(sqlRead["IdCategoria"]);
                    impPF = DbValue<double>.Get(sqlRead["PF"]);
                    impPV = DbValue<double>.Get(sqlRead["PV"]);
                    nComponenti = DbValue<string>.Get(sqlRead["NComponenti"]);
                    MoltiplicatoreMinimo = DbValue<double>.Get(sqlRead["MoltiplicatoreMinimo"]);
                    impMinimo = DbValue<double>.Get(sqlRead["importoMinimo"]);
                    //*** 201712 - gestione tipo conferimento ***
                    TipoConferimento = DbValue<string>.Get(sqlRead["tipoconferimento"]);
                    IDTipoConferimento = DbValue<string>.Get(sqlRead["idtipoconferimento"]);
                    //*** ***
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TariffeEnte[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFE_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@IdCategoria", DbParam.Get(IdCategoria));
                sqlCmd.Parameters.AddWithValue("@TypeTassazione", DbParam.Get(TypeTassazione));
                sqlCmd.Parameters.AddWithValue("@TypeCalcolo", DbParam.Get(TypeCalcolo));
                sqlCmd.Parameters.AddWithValue("@TypeUtenze", DbParam.Get(TypeUtenze));
                sqlCmd.Parameters.AddWithValue("@IsNewAnno", DbParam.Get(IsNewAnno));
                Global.Log.Write2(LogSeverity.Debug, sqlCmd.Connection.ConnectionString + sqlCmd.CommandText + FromVariabile.ToString() + "," + Ente + "," + Anno + "," +IdCategoria.ToString() + "," + TypeTassazione.ToString() + "," + TypeCalcolo + "," + TypeUtenze.ToString() + "," +IsNewAnno.ToString());
                sqlRead = sqlCmd.ExecuteReader();
                List<TariffeEnte> list = new List<TariffeEnte>();
                while (sqlRead.Read())
                {
                    TariffeEnte item = new TariffeEnte();
                    item.IdTariffa = DbValue<int>.Get(sqlRead["IdTariffa"]);
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<string>.Get(sqlRead["Anno"]);
                    item.TypeTassazione= DbValue<int>.Get(sqlRead["TypeTassazione"]);
                    item.Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    item.IdCategoria = DbValue<string>.Get(sqlRead["IdCategoria"]);
                    item.impPF = doubleParser(DbValue<decimal>.Get(sqlRead["PF"]).ToString());
                    item.impPV = doubleParser(DbValue<decimal>.Get(sqlRead["PV"]).ToString());
                    item.nComponenti = DbValue<string>.Get(sqlRead["NComponenti"]);
                    item.MoltiplicatoreMinimo =doubleParser(DbValue<decimal>.Get(sqlRead["MoltiplicatoreMinimo"]).ToString());
                    item.impMinimo = doubleParser(DbValue<decimal>.Get(sqlRead["importoMinimo"]).ToString());
                    //*** 201712 - gestione tipo conferimento ***
                    item.TipoConferimento = DbValue<string>.Get(sqlRead["tipoconferimento"]);
                    item.IDTipoConferimento = DbValue<string>.Get(sqlRead["idtipoconferimento"]);
                    //*** ***

                    list.Add(item);
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Tariffe già presenti impossibile inserire")
                    throw ex;
                else
                {
                    Global.Log.Write2(LogSeverity.Critical, ex);
                    return null;
                }
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet LoadPrint()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            DataSet myResult = new DataSet();

            try
            {
                Connect("OPENgov");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFE_ENTE_PRINT";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@IdCategoria", DbParam.Get(IdCategoria));
                sqlCmd.Parameters.AddWithValue("@TypeTassazione", DbParam.Get(TypeTassazione));
                sqlCmd.Parameters.AddWithValue("@TypeCalcolo", DbParam.Get(TypeCalcolo));
                sqlCmd.Parameters.AddWithValue("@TypeUtenze", DbParam.Get(TypeUtenze));
                sqlCmd.Parameters.AddWithValue("@IsNewAnno", DbParam.Get(IsNewAnno));
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
/// <summary>
/// 
/// </summary>
/// <returns></returns>
        public override bool Save()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFE_ENTE_IU";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdTariffa", DbParam.Get(IdTariffa));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@TypeTassazione", DbParam.Get(TypeTassazione));
                sqlCmd.Parameters.AddWithValue("@IdCategoria", DbParam.Get(IdCategoria));
                sqlCmd.Parameters.AddWithValue("@nComponenti", DbParam.Get(nComponenti));
                sqlCmd.Parameters.AddWithValue("@impPF", DbParam.Get(impPF));
                sqlCmd.Parameters.AddWithValue("@impPV", DbParam.Get(impPV));
                sqlCmd.Parameters.AddWithValue("@MoltiplicatoreMinimo", DbParam.Get(MoltiplicatoreMinimo));
                sqlCmd.Parameters.AddWithValue("@impMinimo", DbParam.Get(impMinimo));
                //*** 201712 - gestione tipo conferimento ***
                sqlCmd.Parameters.AddWithValue("@TIPOCONFERIMENTO", DbParam.Get(TipoConferimento));
                //*** ***

                sqlCmd.Parameters["@IdTariffa"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdTariffa = (int)sqlCmd.Parameters["@IdTariffa"].Value;
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
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFE_ENTE_D";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdTariffa", DbParam.Get(IdTariffa));
                sqlCmd.Parameters.AddWithValue("@Ente",DbParam.Get(Ente));
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
        public bool CopyYear(string YearFrom, string YearTo)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_RIBALTA_TARIFFE_ENTE";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdTariffa", DbParam.Get(IdTariffa));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@TYPETASSAZIONE", DbParam.Get(TypeTassazione));
                sqlCmd.Parameters.AddWithValue("@AnnoOrg", DbParam.Get(YearFrom));
                sqlCmd.Parameters.AddWithValue("@AnnoDest", DbParam.Get(YearTo));
                Global.Log.Write2(LogSeverity.Debug, sqlCmd.Connection.ConnectionString + sqlCmd.CommandText + FromVariabile.ToString() + "," + IdTariffa.ToString() + "," + Ente + "," + TypeTassazione.ToString() + "," + YearFrom.ToString() + "," + YearTo.ToString());

                sqlCmd.Parameters["@IdTariffa"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdTariffa = (int)sqlCmd.Parameters["@IdTariffa"].Value;
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
        public TariffeEnte[] LoadSimulazione(int IdSimulazione)
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
                sqlCmd.CommandText = "prc_TBLSIMULAZIONE_TARIFFE_S";
                sqlCmd.Parameters.AddWithValue("@IdSimulazione", DbParam.Get(IdSimulazione));
                Global.Log.Write2(LogSeverity.Debug, myTypeConn + "prc_TBLSIMULAZIONE_TARIFFE_S " + IdSimulazione.ToString());
                sqlRead = sqlCmd.ExecuteReader();   
                List<TariffeEnte> list = new List<TariffeEnte>();
                while (sqlRead.Read())
                {
                    TariffeEnte item = new TariffeEnte();
                    item.IdTariffa = DbValue<int>.Get(sqlRead["Id"]);
                    item.Ente = DbValue<string>.Get(sqlRead["IdEnte"]);
                    item.Anno = DbValue<string>.Get(sqlRead["Anno"]);
                    item.TypeTassazione = DbValue<int>.Get(sqlRead["TypeTassazione"]);
                    item.Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    item.IdCategoria = DbValue<string>.Get(sqlRead["IdCategoria"]);
                    item.impPF = doubleParser(DbValue<decimal>.Get(sqlRead["PF"]).ToString());
                    item.impPV = doubleParser(DbValue<decimal>.Get(sqlRead["PV"]).ToString());
                    item.nComponenti = DbValue<string>.Get(sqlRead["NComponenti"]);

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
    }
    /// <summary>
    /// Classe per la gestione delle tariffe riduzioni/esenzioni dell'ente
    /// </summary>
    public class RidEseTariffeEnte : DbObject<RidEseTariffeEnte>
    {
        #region Variables and constructor

        public RidEseTariffeEnte()
        {
            Reset();
        }

        public RidEseTariffeEnte(int idTariffa)
        {
            Reset();
            IdTariffa = idTariffa;
        }
        #endregion

        #region Public properties
        public string FromVariabile { get; set; }
        public int IdTariffa { get; set; }
        public string Ente { get; set; }
        public string myType { get; set; }
        public string Anno { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        public string Categoria { get; set; }
        public bool hasPF { get; set; }
        public bool hasPV { get; set; }
        public bool hasPC { get; set; }
        public bool hasPM { get; set; }
        public bool hasImponibile { get; set; }
        public string Tipo  { get; set; }
        public string DescrTipo { get; set; }
        public string Valore { get; set; }
        public bool IsNewAnno { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is RidEseTariffeEnte) &&
                ((obj as RidEseTariffeEnte).IdTariffa == IdTariffa);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdTariffa);
        }

        public override sealed void Reset()
        {
            FromVariabile = default(string);
            IdTariffa = default(int);
            Ente = default(string);
            myType = default(string);
            Anno = default(string);
            Codice = default(string);
            Descrizione = default(string);
            Categoria = default(string);
            hasPF = default(bool);
            hasPV = default(bool);
            hasPC = default(bool);
            hasPM = default(bool);
            hasImponibile = default(bool);
            Tipo = default(string);
            DescrTipo = default(string);
            Valore = default(string);
            IsNewAnno = default(bool);
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFERIDESE_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TypeRidEse", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@IdTariffa", DbParam.Get(IdTariffa));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    Anno = DbValue<string>.Get(sqlRead["Anno"]);
                    Codice = DbValue<string>.Get(sqlRead["Codice"]);
                    Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    //Categoria = DbValue<string>.Get(sqlRead["Cat"]);
                    hasPF = DbValue<bool>.Get(sqlRead["pf"]);
                    hasPV = DbValue<bool>.Get(sqlRead["pv"]);
                    hasPC = DbValue<bool>.Get(sqlRead["pc"]);
                    hasPM = DbValue<bool>.Get(sqlRead["pm"]);
                    hasImponibile = DbValue<bool>.Get(sqlRead["imponibile"]);
                    Tipo = DbValue<string>.Get(sqlRead["tipo"]);
                    DescrTipo = DbValue<string>.Get(sqlRead["descrtipo"]);
                    Valore = DbValue<string>.Get(sqlRead["valore"]);
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

        public override RidEseTariffeEnte[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TARIFFERIDESE_ENTE_S";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TypeRidEse", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@IsNewAnno", DbParam.Get(IsNewAnno));
                sqlRead = sqlCmd.ExecuteReader();

                List<RidEseTariffeEnte> list = new List<RidEseTariffeEnte>();
                while (sqlRead.Read())
                {
                    RidEseTariffeEnte item = new RidEseTariffeEnte();
                    item.IdTariffa = DbValue<int>.Get(sqlRead["IdTariffa"]);
                    item.Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    item.Anno = DbValue<string>.Get(sqlRead["Anno"]);
                    item.Codice = DbValue<string>.Get(sqlRead["Codice"]);
                    item.Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
                    //item.Categoria = DbValue<string>.Get(sqlRead["Cat"]);
                    item.hasPF= DbValue<bool>.Get(sqlRead["pf"]);
                    item.hasPV = DbValue<bool>.Get(sqlRead["pv"]);
                    item.hasPC = DbValue<bool>.Get(sqlRead["pc"]);
                    item.hasPM = DbValue<bool>.Get(sqlRead["pm"]);
                    item.hasImponibile = DbValue<bool>.Get(sqlRead["imponibile"]);
                    item.Tipo = DbValue<string>.Get(sqlRead["tipo"]);
                    item.DescrTipo = DbValue<string>.Get(sqlRead["descrtipo"]);
                    item.Valore= DbValue<string>.Get(sqlRead["valore"]);
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
        /// <returns></returns>
        public override bool Save()
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLRIDUZIONI_IU";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(IdTariffa));
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@ANNO", DbParam.Get(Anno));
                sqlCmd.Parameters.AddWithValue("@CODICE", DbParam.Get(Codice));
                sqlCmd.Parameters.AddWithValue("@TIPO", DbParam.Get(Tipo));
                sqlCmd.Parameters.AddWithValue("@VALORE", DbParam.Get(Valore));
                sqlCmd.Parameters.AddWithValue("@HASPF", DbParam.Get(hasPF));
                sqlCmd.Parameters.AddWithValue("@HASPV", DbParam.Get(hasPV));
                sqlCmd.Parameters.AddWithValue("@HASPC", DbParam.Get(hasPC));
                sqlCmd.Parameters.AddWithValue("@HASPM", DbParam.Get(hasPM));

                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdTariffa = (int)sqlCmd.Parameters["@ID"].Value;
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
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLRIDUZIONI_D";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@TIPORIDESE", DbParam.Get(myType));
                sqlCmd.Parameters.AddWithValue("@Id", DbParam.Get(IdTariffa));
                sqlCmd.Parameters.AddWithValue("@IdEnte", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Anno", DbParam.Get(Anno));
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
        public bool CopyYear(string YearFrom, string YearTo)
        {
            SqlCommand sqlCmd = null;

            try
            {
                Connect();
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_RIBALTA_TARIFFERIDESE_ENTE";
                sqlCmd.Parameters.AddWithValue("@IsFromVariabile", DbParam.Get(FromVariabile));
                sqlCmd.Parameters.AddWithValue("@IdTariffa", DbParam.Get(IdTariffa));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@Tipo", DbParam.Get(Tipo));
                sqlCmd.Parameters.AddWithValue("@AnnoOrg", DbParam.Get(YearFrom));
                sqlCmd.Parameters.AddWithValue("@AnnoDest", DbParam.Get(YearTo));

                sqlCmd.Parameters["@IdTariffa"].Direction = ParameterDirection.InputOutput;
                if (sqlCmd.ExecuteNonQuery() <= 0) return false;
                IdTariffa = (int)sqlCmd.Parameters["@IdTariffa"].Value;
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
}
