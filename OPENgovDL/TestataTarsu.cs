using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione della transcodifica da TARSU a TARI della testata
    /// </summary>
    public class TestataTarsu : DbObject<TestataTarsu>
    {
        #region Variables and constructor

        public TestataTarsu()
        {
            Reset();
        }

        public TestataTarsu(int iD)
        {
            Reset();
            ID = iD;
        }
        #endregion

        #region Public properties
        public int ID { get; set; }
        public int IdTestata { get; set; }
        public string IdEnte { get; set; }
        public int IdContribuente { get; set; }
        public DateTime DataDichiarazione { get; set; }
        public string NumeroDichiarazione { get; set; }
        public DateTime? DataProtocollo { get; set; }
        public string NumeroProtocollo { get; set; }
        public int? NComponenti { get; set; }
        public string IdProvenienza { get; set; }
        public string NoteDichiarazione { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime? DataVariazione { get; set; }
        public DateTime? DataCessazione { get; set; }
        public string Operatore { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is TestataTarsu) &&
                ((obj as TestataTarsu).ID == this.ID);
        }

        public override int GetHashCode()
        {
            return base.GenerateHashCode(ID);
        }

        public override void Reset()
        {
            ID = default(int);
            IdTestata = default(int);
            IdEnte = default(string);
            IdContribuente = default(int);
            DataDichiarazione = Global.MinDateTime();
            NumeroDichiarazione = default(string);
            DataProtocollo = default(DateTime?);
            NumeroProtocollo = default(string);
            NComponenti = default(int?);
            IdProvenienza = default(string);
            NoteDichiarazione = default(string);
            DataInserimento = Global.MinDateTime();
            DataVariazione = default(DateTime?);
            DataCessazione = default(DateTime?);
            Operatore = default(string);
        }

        public override bool Load()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLTESTATATARSU_S";
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    IdTestata = DbValue<int>.Get(sqlRead["IDTESTATA"]);
                    IdEnte = DbValue<string>.Get(sqlRead["IDENTE"]);
                    IdContribuente = DbValue<int>.Get(sqlRead["IDCONTRIBUENTE"]);
                    DataDichiarazione = DbValue<DateTime>.Get(sqlRead["DATA_DICHIARAZIONE"]);
                    NumeroDichiarazione = DbValue<string>.Get(sqlRead["NUMERO_DICHIARAZIONE"]);
                    DataProtocollo = DbValue<DateTime?>.Get(sqlRead["DATA_PROTOCOLLO"]);
                    NumeroProtocollo = DbValue<string>.Get(sqlRead["NUMERO_PROTOCOLLO"]);
                    NComponenti = DbValue<int?>.Get(sqlRead["NCOMPONENTI"]);
                    IdProvenienza = DbValue<string>.Get(sqlRead["IDPROVENIENZA"]);
                    NoteDichiarazione = DbValue<string>.Get(sqlRead["NOTE_DICHIARAZIONE"]);
                    DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]);
                    DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]);
                    DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]);
                    Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]);
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

        public override TestataTarsu[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLTESTATATARSU_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<TestataTarsu> list = new List<TestataTarsu>();
                while (sqlRead.Read())
                {
                    TestataTarsu item = new TestataTarsu
                        {
                            ID = DbValue<int>.Get(sqlRead["ID"]),
                            IdTestata = DbValue<int>.Get(sqlRead["IDTESTATA"]),
                            IdEnte = DbValue<string>.Get(sqlRead["IDENTE"]),
                            IdContribuente = DbValue<int>.Get(sqlRead["IDCONTRIBUENTE"]),
                            DataDichiarazione = DbValue<DateTime>.Get(sqlRead["DATA_DICHIARAZIONE"]),
                            NumeroDichiarazione = DbValue<string>.Get(sqlRead["NUMERO_DICHIARAZIONE"]),
                            DataProtocollo = DbValue<DateTime?>.Get(sqlRead["DATA_PROTOCOLLO"]),
                            NumeroProtocollo = DbValue<string>.Get(sqlRead["NUMERO_PROTOCOLLO"]),
                            NComponenti = DbValue<int?>.Get(sqlRead["NCOMPONENTI"]),
                            IdProvenienza = DbValue<string>.Get(sqlRead["IDPROVENIENZA"]),
                            NoteDichiarazione = DbValue<string>.Get(sqlRead["NOTE_DICHIARAZIONE"]),
                            DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]),
                            DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]),
                            DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]),
                            Operatore = DbValue<string>.Get(sqlRead["OPERATORE"])
                        };
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

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLTESTATATARSU_IU";

                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlCmd.Parameters.AddWithValue("@IDTESTATA", DbParam.Get(IdTestata));
                sqlCmd.Parameters.AddWithValue("@IDENTE", DbParam.Get(IdEnte));
                sqlCmd.Parameters.AddWithValue("@IDCONTRIBUENTE", DbParam.Get(IdContribuente));
                sqlCmd.Parameters.AddWithValue("@DATA_DICHIARAZIONE", DbParam.Get(DataDichiarazione));
                sqlCmd.Parameters.AddWithValue("@NUMERO_DICHIARAZIONE", DbParam.Get(NumeroDichiarazione));
                sqlCmd.Parameters.AddWithValue("@DATA_PROTOCOLLO", DbParam.Get(DataProtocollo));
                sqlCmd.Parameters.AddWithValue("@NUMERO_PROTOCOLLO", DbParam.Get(NumeroProtocollo));
                sqlCmd.Parameters.AddWithValue("@NCOMPONENTI", DbParam.Get(NComponenti));
                sqlCmd.Parameters.AddWithValue("@IDPROVENIENZA", DbParam.Get(IdProvenienza));
                sqlCmd.Parameters.AddWithValue("@NOTE_DICHIARAZIONE", DbParam.Get(NoteDichiarazione));
                sqlCmd.Parameters.AddWithValue("@DATA_INSERIMENTO", DbParam.Get(DataInserimento));
                sqlCmd.Parameters.AddWithValue("@DATA_VARIAZIONE", DbParam.Get(DataVariazione));
                sqlCmd.Parameters.AddWithValue("@DATA_CESSAZIONE", DbParam.Get(DataCessazione));
                sqlCmd.Parameters.AddWithValue("@OPERATORE", DbParam.Get(Operatore));

                sqlCmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                ID = (int)sqlCmd.Parameters["@ID"].Value;
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
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLTESTATATARSU_D";
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
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
    }
}
