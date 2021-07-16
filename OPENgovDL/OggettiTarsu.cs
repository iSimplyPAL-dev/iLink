using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione della transcodifica da TARSU a TARI degli oggetti della testata
    /// </summary>
    public class OggettiTarsu : DbObject<OggettiTarsu>
    {
        #region Variables and constructor

        public OggettiTarsu()
        {
            Reset();
        }

        public OggettiTarsu(int iD)
        {
            Reset();
            ID = iD;
        }
        #endregion

        #region Public properties
        public int ID { get; set; }
        public int IdDettaglioTestata { get; set; }
        public string IdCategoria { get; set; }
        public string IdTipoVano { get; set; }
        public int NumeroVani { get; set; }
        public double MQ { get; set; }
        public string Provenienza { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime? DataVariazione { get; set; }
        public DateTime? DataCessazione { get; set; }
        public string Operatore { get; set; }
        public string Note { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is OggettiTarsu) &&
                ((obj as OggettiTarsu).ID == ID);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(ID);
        }

        public override void Reset()
        {
            ID = default(int);
            IdDettaglioTestata = default(int);
            IdCategoria = default(string);
            IdTipoVano = default(string);
            NumeroVani = default(int);
            MQ = default(double);
            Provenienza = default(string);
            DataInserimento = Global.MinDateTime();
            DataVariazione = default(DateTime?);
            DataCessazione = default(DateTime?);
            Operatore = default(string);
            Note = default(string);
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
                sqlCmd.CommandText = "TBLOGGETTITARSU_S";
                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    IdDettaglioTestata = DbValue<int>.Get(sqlRead["IDDETTAGLIOTESTATA"]);
                    IdCategoria = DbValue<string>.Get(sqlRead["IDCATEGORIA"]);
                    IdTipoVano = DbValue<string>.Get(sqlRead["IDTIPOVANO"]);
                    NumeroVani = DbValue<int>.Get(sqlRead["NVANI"]);
                    MQ = DbValue<double>.Get(sqlRead["MQ"]);
                    Provenienza = DbValue<string>.Get(sqlRead["PROVENIENZA"]);
                    DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]);
                    DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]);
                    DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]);
                    Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]);
                    Note = DbValue<string>.Get(sqlRead["NOTE"]);
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

        public override OggettiTarsu[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLOGGETTITARSU_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<OggettiTarsu> list = new List<OggettiTarsu>();
                while (sqlRead.Read())
                {
                    OggettiTarsu item = new OggettiTarsu
                        {
                            ID = DbValue<int>.Get(sqlRead["ID"]),
                            IdDettaglioTestata = DbValue<int>.Get(sqlRead["IDDETTAGLIOTESTATA"]),
                            IdCategoria = DbValue<string>.Get(sqlRead["IDCATEGORIA"]),
                            IdTipoVano = DbValue<string>.Get(sqlRead["IDTIPOVANO"]),
                            NumeroVani = DbValue<int>.Get(sqlRead["NVANI"]),
                            MQ = DbValue<double>.Get(sqlRead["MQ"]),
                            Provenienza = DbValue<string>.Get(sqlRead["PROVENIENZA"]),
                            DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]),
                            DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]),
                            DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]),
                            Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]),
                            Note = DbValue<string>.Get(sqlRead["NOTE"])
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
                sqlCmd.CommandText = "TBLOGGETTITARSU_IU";

                sqlCmd.Parameters.AddWithValue("@ID", DbParam.Get(ID));
                sqlCmd.Parameters.AddWithValue("@IDDETTAGLIOTESTATA", DbParam.Get(IdDettaglioTestata));
                sqlCmd.Parameters.AddWithValue("@IDCATEGORIA", DbParam.Get(IdCategoria));
                sqlCmd.Parameters.AddWithValue("@IDTIPOVANO", DbParam.Get(IdTipoVano));
                sqlCmd.Parameters.AddWithValue("@NVANI", DbParam.Get(NumeroVani));
                sqlCmd.Parameters.AddWithValue("@MQ", DbParam.Get(MQ));
                sqlCmd.Parameters.AddWithValue("@PROVENIENZA", DbParam.Get(Provenienza));
                sqlCmd.Parameters.AddWithValue("@DATA_INSERIMENTO", DbParam.Get(DataInserimento));
                sqlCmd.Parameters.AddWithValue("@DATA_VARIAZIONE", DbParam.Get(DataVariazione));
                sqlCmd.Parameters.AddWithValue("@DATA_CESSAZIONE", DbParam.Get(DataCessazione));
                sqlCmd.Parameters.AddWithValue("@OPERATORE", DbParam.Get(Operatore));
                sqlCmd.Parameters.AddWithValue("@NOTE", DbParam.Get(Note));

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
                sqlCmd.CommandText = "TBLOGGETTITARSU_D";
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

        public OggettiTarsu[] LoadByDettaglio(int idDettaglio)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "TBLOGGETTITARSU_F";
                sqlCmd.Parameters.AddWithValue("@IDDETTAGLIOTESTATA", DbParam.Get(idDettaglio));
                sqlRead = sqlCmd.ExecuteReader();

                List<OggettiTarsu> list = new List<OggettiTarsu>();
                while (sqlRead.Read())
                {
                    OggettiTarsu item = new OggettiTarsu
                    {
                        ID = DbValue<int>.Get(sqlRead["ID"]),
                        IdDettaglioTestata = DbValue<int>.Get(sqlRead["IDDETTAGLIOTESTATA"]),
                        IdCategoria = DbValue<string>.Get(sqlRead["IDCATEGORIA"]),
                        IdTipoVano = DbValue<string>.Get(sqlRead["IDTIPOVANO"]),
                        NumeroVani = DbValue<int>.Get(sqlRead["NVANI"]),
                        MQ = DbValue<double>.Get(sqlRead["MQ"]),
                        Provenienza = DbValue<string>.Get(sqlRead["PROVENIENZA"]),
                        DataInserimento = DbValue<DateTime>.Get(sqlRead["DATA_INSERIMENTO"]),
                        DataVariazione = DbValue<DateTime?>.Get(sqlRead["DATA_VARIAZIONE"]),
                        DataCessazione = DbValue<DateTime?>.Get(sqlRead["DATA_CESSAZIONE"]),
                        Operatore = DbValue<string>.Get(sqlRead["OPERATORE"]),
                        Note = DbValue<string>.Get(sqlRead["NOTE"])
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
    }
}
