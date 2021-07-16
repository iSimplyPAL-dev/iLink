using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;

namespace OPENgovDL
{
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto dell'immobile 
    /// </summary>
    public class AnagrafeUi : DbObject<AnagrafeUi>
    {
		#region Variables and constructor
        public AnagrafeUi()
		{
			Reset();
		}

        public AnagrafeUi(int idAnagrafeUI)
		{
			Reset();
			IdAnagrafeUI = idAnagrafeUI;
		}
		#endregion

		#region Public properties
        public int IdAnagrafeUI { get; set; }
        public string CodEnte { get; set; }
        public int IdentificativoImmobile { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public string TipoImmobile { get; set; }
        public string Zona { get; set; }
        public int FkIdEnteCat { get; set; }
        public int Classe { get; set; }
        public double Consistenza { get; set; }
        public double Superficie { get; set; }
        public double Rendita { get; set; }
        public int FkIdStrada { get; set; }
        public int FkIdToponimo { get; set; }
        public string Indirizzo { get; set; }
        public string Civico { get; set; }
        public bool IsActive { get; set; }

        #region Movimento
        public AnagrafeUiMovimenti Movimento { get; set; }
        #endregion

        #region Graffati
        public string Sezioneurbana { get; set; }
        public int Denominatore { get; set; }
        public string Edificialita { get; set; }
        #endregion
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
		{
			return
                (obj is AnagrafeUi) &&
                ((obj as AnagrafeUi).IdAnagrafeUI == IdAnagrafeUI);
		}

		public override int GetHashCode()
		{
			return GenerateHashCode(IdAnagrafeUI);
		}

		public override sealed void Reset()
		{
			IdAnagrafeUI = default(int);
            CodEnte = string.Empty;
			IdentificativoImmobile = default(int);
            Foglio = string.Empty;
            Numero = string.Empty;
            Subalterno = string.Empty;
            TipoImmobile = string.Empty;
			Zona = string.Empty;
			FkIdEnteCat = default(int);
			Classe = default(int);
			Consistenza = default(double);
			Superficie = default(double);
			Rendita = default(double);
			FkIdStrada = default(int);
			FkIdToponimo = default(int);
			Indirizzo = Civico = string.Empty;
		    IsActive = true;
		}

		public override bool Load()
		{
			SqlCommand sqlCmd = null;
			SqlDataReader sqlRead = null;

			try
			{
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "ANAGRAFE_UI_S";
				sqlCmd.Parameters.AddWithValue("@IdAnagrafeUI", DbParam.Get(IdAnagrafeUI));
				sqlRead = sqlCmd.ExecuteReader();

				if(sqlRead.Read())
				{
					CodEnte = DbValue<string>.Get(sqlRead["Cod_Ente"]);
					IdentificativoImmobile = DbValue<int>.Get(sqlRead["Identificativo_Immobile"]);
					Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
					Numero = DbValue<string>.Get(sqlRead["Numero"]);
					Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
					TipoImmobile = DbValue<string>.Get(sqlRead["Tipo_Immobile"]);
					Zona = DbValue<string>.Get(sqlRead["Zona"]);
					FkIdEnteCat = DbValue<int>.Get(sqlRead["fk_IdEnteCat"]);
					Classe = DbValue<int>.Get(sqlRead["Classe"]);
					Consistenza = DbValue<double>.Get(sqlRead["Consistenza"]);
					Superficie = DbValue<double>.Get(sqlRead["Superficie"]);
					Rendita = DbValue<double>.Get(sqlRead["Rendita"]);
					FkIdStrada = DbValue<int>.Get(sqlRead["fk_IdStrada"]);
					FkIdToponimo = DbValue<int>.Get(sqlRead["fk_IdToponimo"]);
					Indirizzo = DbValue<string>.Get(sqlRead["Indirizzo"]);
					Civico = DbValue<string>.Get(sqlRead["Civico"]);
                    IsActive = DbValue<bool>.Get(sqlRead["IsActive"]);
				}
				else
				{
					Reset();
				}

				return true;
			}
			catch(Exception ex)
			{
				Global.Log.Write2(LogSeverity.Critical, ex);
				return false;
			}
			finally
			{
				Disconnect(sqlCmd, sqlRead);
			}
		}

        public override AnagrafeUi[] LoadAll()
		{
			SqlCommand sqlCmd = null;
			SqlDataReader sqlRead = null;

			try
			{
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "ANAGRAFE_UI_S";
				sqlRead = sqlCmd.ExecuteReader();

                List<AnagrafeUi> list = new List<AnagrafeUi>();
				while(sqlRead.Read())
				{
                    AnagrafeUi item = new AnagrafeUi();
					item.IdAnagrafeUI = DbValue<int>.Get(sqlRead["IdAnagrafeUI"]);
					item.CodEnte = DbValue<string>.Get(sqlRead["Cod_Ente"]);
					item.IdentificativoImmobile = DbValue<int>.Get(sqlRead["Identificativo_Immobile"]);
					item.Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
					item.Numero = DbValue<string>.Get(sqlRead["Numero"]);
					item.Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
					item.TipoImmobile = DbValue<string>.Get(sqlRead["Tipo_Immobile"]);
					item.Zona = DbValue<string>.Get(sqlRead["Zona"]);
					item.FkIdEnteCat = DbValue<int>.Get(sqlRead["fk_IdEnteCat"]);
					item.Classe = DbValue<int>.Get(sqlRead["Classe"]);
					item.Consistenza = DbValue<double>.Get(sqlRead["Consistenza"]);
					item.Superficie = DbValue<double>.Get(sqlRead["Superficie"]);
					item.Rendita = DbValue<double>.Get(sqlRead["Rendita"]);
					item.FkIdStrada = DbValue<int>.Get(sqlRead["fk_IdStrada"]);
					item.FkIdToponimo = DbValue<int>.Get(sqlRead["fk_IdToponimo"]);
					item.Indirizzo = DbValue<string>.Get(sqlRead["Indirizzo"]);
					item.Civico = DbValue<string>.Get(sqlRead["Civico"]);
                    item.IsActive = DbValue<bool>.Get(sqlRead["IsActive"]);
					list.Add(item);
				}

				return list.ToArray();
			}
			catch(Exception ex)
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
			SqlCommand sqlCmd = null;

			try
			{
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "ANAGRAFE_UI_IU";

				sqlCmd.Parameters.AddWithValue("@IdAnagrafeUI", DbParam.Get(IdAnagrafeUI));
				sqlCmd.Parameters.AddWithValue("@Cod_Ente", DbParam.Get(CodEnte));
				sqlCmd.Parameters.AddWithValue("@Identificativo_Immobile", DbParam.Get(IdentificativoImmobile));
				sqlCmd.Parameters.AddWithValue("@Foglio", DbParam.Get(Foglio));
				sqlCmd.Parameters.AddWithValue("@Numero", DbParam.Get(Numero));
				sqlCmd.Parameters.AddWithValue("@Subalterno", DbParam.Get(Subalterno));
				sqlCmd.Parameters.AddWithValue("@Tipo_Immobile", DbParam.Get(TipoImmobile));
				sqlCmd.Parameters.AddWithValue("@Zona", DbParam.Get(Zona));
				sqlCmd.Parameters.AddWithValue("@fk_IdEnteCat", DbParam.Get(FkIdEnteCat));
                sqlCmd.Parameters.AddWithValue("@Classe", DbParam.Get(Classe));
				sqlCmd.Parameters.AddWithValue("@Consistenza", DbParam.Get(Consistenza));
				sqlCmd.Parameters.AddWithValue("@Superficie", DbParam.Get(Superficie));
				sqlCmd.Parameters.AddWithValue("@Rendita", DbParam.Get(Rendita));
				sqlCmd.Parameters.AddWithValue("@fk_IdStrada", DbParam.Get(FkIdStrada));
				sqlCmd.Parameters.AddWithValue("@fk_IdToponimo", DbParam.Get(FkIdToponimo));
				sqlCmd.Parameters.AddWithValue("@Indirizzo", DbParam.Get(Indirizzo));
				sqlCmd.Parameters.AddWithValue("@Civico", DbParam.Get(Civico));
                sqlCmd.Parameters.AddWithValue("@IsActive", DbParam.Get(IsActive));

				sqlCmd.Parameters["@IdAnagrafeUI"].Direction = ParameterDirection.InputOutput;
				sqlCmd.ExecuteNonQuery();
				IdAnagrafeUI = (int)sqlCmd.Parameters["@IdAnagrafeUI"].Value;

                if (Movimento != null)
                {
                    Movimento.FKIdAnagrafeUI = IdAnagrafeUI;
                    Movimento.Save(ref error);
                }

				return true;
			}
			catch(Exception ex)
			{
				Global.Log.Write2(LogSeverity.Critical, ex);
                error = ex.Message;
				return false;
			}
			finally
			{
				Disconnect(sqlCmd);
			}
		}

		public override bool Delete()
		{
			SqlCommand sqlCmd = null;

			try
			{
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "ANAGRAFE_UI_D";
				sqlCmd.Parameters.AddWithValue("@IdAnagrafeUI", DbParam.Get(IdAnagrafeUI));
				sqlCmd.ExecuteNonQuery();
				return true;
			}
			catch(Exception ex)
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

        #region Public Methods
        public bool LoadByIdentificativoImmobile(int immobile)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAGRAFE_UI_S_IMMOBILE";
                sqlCmd.Parameters.AddWithValue("@Identificativo_Immobile", DbParam.Get(immobile));
                sqlRead = sqlCmd.ExecuteReader();

                if (!sqlRead.Read()) return false;

                IdAnagrafeUI = DbValue<int>.Get(sqlRead["IdAnagrafeUI"]);
                CodEnte = DbValue<string>.Get(sqlRead["Cod_Ente"]);
                IdentificativoImmobile = DbValue<int>.Get(sqlRead["Identificativo_Immobile"]);
                Foglio = DbValue<string>.Get(sqlRead["Foglio"]);
                Numero = DbValue<string>.Get(sqlRead["Numero"]);
                Subalterno = DbValue<string>.Get(sqlRead["Subalterno"]);
                TipoImmobile = DbValue<string>.Get(sqlRead["Tipo_Immobile"]);
                Zona = DbValue<string>.Get(sqlRead["Zona"]);
                FkIdEnteCat = DbValue<int>.Get(sqlRead["fk_IdEnteCat"]);
                Classe = DbValue<int>.Get(sqlRead["Classe"]);
                Consistenza = DbValue<double>.Get(sqlRead["Consistenza"]);
                Superficie = DbValue<double>.Get(sqlRead["Superficie"]);
                Rendita = DbValue<double>.Get(sqlRead["Rendita"]);
                FkIdStrada = DbValue<int>.Get(sqlRead["fk_IdStrada"]);
                FkIdToponimo = DbValue<int>.Get(sqlRead["fk_IdToponimo"]);
                Indirizzo = DbValue<string>.Get(sqlRead["Indirizzo"]);
                Civico = DbValue<string>.Get(sqlRead["Civico"]);
                IsActive = DbValue<bool>.Get(sqlRead["IsActive"]);

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

        public int GetCategory(string categoria, string ente)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            try
            {
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
				sqlCmd.CommandType = CommandType.StoredProcedure;
				sqlCmd.CommandText = "sp_GetEnteCategoriaId";
				sqlCmd.Parameters.AddWithValue("@Category", DbParam.Get(categoria));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(ente));
				sqlRead = sqlCmd.ExecuteReader();

                int code = (sqlRead.Read())? DbValue<int>.Get(sqlRead["IdEnteCat"]):-1;
                return code;
            }
            catch (Exception ex)
            {
                Global.Log.Write2(LogSeverity.Critical, ex);
                return -1;
            }
            finally
            {
                Disconnect(sqlCmd, sqlRead);
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione dei dati esterni da Catasto delle categorie
    /// </summary>
    public class CategorieCatastali : DbObject<CategorieCatastali>
    {
        #region Variables and constructor

        public CategorieCatastali()
        {
            Reset();
        }

        public CategorieCatastali(string CodCategoria)
        {
            Reset();
            CodiceCategoria = CodCategoria;
        }
        #endregion

        #region Public properties
        public string CodiceCategoria { get; set; }
        public string Definizione { get; set; }
        public string Descrizione { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is CategorieCatastali) &&
                ((obj as CategorieCatastali).CodiceCategoria == CodiceCategoria);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(CodiceCategoria);
        }

        public override sealed void Reset()
        {
            CodiceCategoria = default(string);
            Definizione = default(string);
            Descrizione = default(string);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override CategorieCatastali[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovICI");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_TBLCATEGORIACATASTALE_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<CategorieCatastali> list = new List<CategorieCatastali>();
                while (sqlRead.Read())
                {
                    CategorieCatastali item = new CategorieCatastali();
                    item.CodiceCategoria = DbValue<string>.Get(sqlRead["DropValue"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["CategoriaCatastale"]);
                    item.Descrizione = DbValue<string>.Get(sqlRead["Descrizione"]);
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
    /// Classe per la gestione dei dati esterni da Catasto degli stati di occupazione
    /// </summary>
    public class StatoOccupazione : DbObject<StatoOccupazione>
    {
        #region Variables and constructor

        public StatoOccupazione()
        {
            Reset();
        }

        public StatoOccupazione(string Cod)
        {
            Reset();
            Codice = Cod;
        }
        #endregion

        #region Public properties
        public string Codice { get; set; }
        public string Definizione { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is StatoOccupazione) &&
                ((obj as StatoOccupazione).Codice == Codice);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(Codice);
        }

        public override sealed void Reset()
        {
            Codice = default(string);
            Definizione = default(string);
        }

        public override bool Load()
        {
            throw new NotImplementedException();
        }

        public override StatoOccupazione[] LoadAll()
        {
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;

            try
            {
                Connect("OPENgovTARSU");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "prc_STATOOCCUPAZIONE_S";
                sqlRead = sqlCmd.ExecuteReader();

                List<StatoOccupazione> list = new List<StatoOccupazione>();
                while (sqlRead.Read())
                {
                    StatoOccupazione item = new StatoOccupazione();
                    item.Codice = DbValue<string>.Get(sqlRead["Codice"]);
                    item.Definizione = DbValue<string>.Get(sqlRead["Descrizione"]);
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
}
