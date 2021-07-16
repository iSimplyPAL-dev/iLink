using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FutureFog.U4N.General;
using Ribes.OPENgov.Utilities;
using log4net;
using Utility;

namespace OPENgovDL
{
    /// <summary>
	/// Classe per la gestione dei flussi di carico da dati esterni
	/// </summary>
	public class AnagFile : DbObject<AnagFile>
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(AnagFile));
        #region Variables and constructor

        private byte[] _postedFile;

        /*public AnagFile()
        {
            Reset();
        }*/
        public AnagFile(string typeDB, string connectionString)
        {
            Reset();
            TypeDB =typeDB;
            ConnectionString = connectionString;
        }

        public AnagFile(int idAnagFile)
        {
            Reset();
            IdAnagFile = idAnagFile;
        }
        #endregion

        #region Public properties

        public int IdAnagFile { get; set; }

        public byte[] PostedFile
        {
            get 
            {
                if ((_postedFile == null) && (IdAnagFile != default(int)))
                    GetFile();
                return _postedFile; 
            }
            set { _postedFile = value; }
        }
        /// <summary>
        /// Il codice ISTAT che rappresenta l'Ente
        /// </summary>
        public string Ente { get; set; }
        /// <summary>
        /// Il codice Belfiore che rappresenta l'Ente
        /// </summary>
        public string CodEnte { get; set; }

        public string FileName { get; set; }

        public string FileMIMEType { get; set; }

        public int IdAnagFileType { get; set; }

        public string AnagFileType { get; set; }

        public int Rows { get; set; }

        public DateTime InsertDateTime { get; private set; }

        public DateTime? ReadDateTime { get; set; }

        public string AnagFileLog { get; set; }
        public string PathFile { get; set; }
        private string TypeDB { get; set; }
        private string ConnectionString { get; set; }
        #endregion

        #region DbObject overridden methods
        public override bool Equals(object obj)
        {
            return
                (obj is AnagFile) &&
                ((obj as AnagFile).IdAnagFile == IdAnagFile);
        }

        public override int GetHashCode()
        {
            return GenerateHashCode(IdAnagFile);
        }

        public override sealed void Reset()
        {
            IdAnagFile = default(int);
            _postedFile = null;
            Ente = default(string);
            FileName = default(string);
            FileMIMEType = default(string);
            IdAnagFileType = default(int);
            AnagFileType = default(string);
            Rows = default(int);
            InsertDateTime = Global.MinDateTime();
            ReadDateTime = DateTime.MaxValue;
            AnagFileLog = string.Empty;
            PathFile = string.Empty;
        }

        public override bool Load()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            try
            {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_S";
                sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
                sqlRead = sqlCmd.ExecuteReader();

                if (sqlRead.Read())
                {
                    //_PostedFile = DbValue<byte[]>.Get(sqlRead["PostedFile"]);
                    Ente = DbValue<string>.Get(sqlRead["Ente"]);
                    FileName = DbValue<string>.Get(sqlRead["FileName"]);
                    FileMIMEType = DbValue<string>.Get(sqlRead["FileMIMEType"]);
                    IdAnagFileType = DbValue<int>.Get(sqlRead["IdAnagFileType"]);
                    AnagFileType = DbValue<string>.Get(sqlRead["AnagFileName"]);
                    Rows = DbValue<int>.Get(sqlRead["Rows"]);
                    InsertDateTime = DbValue<DateTime>.Get(sqlRead["InsertDateTime"]);
                    ReadDateTime = DbValue<DateTime?>.Get(sqlRead["ReadDateTime"]);
                    AnagFileLog = DbValue<string>.Get(sqlRead["AnagFileLog"]);
                }
                else
                {
                    Reset();
                }*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_S", "IdAnagFile");
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdAnagFile", IdAnagFile));
                    if (dvMyView.Count > 0)
                    {
                        foreach (DataRowView myRow in dvMyView)
                        {
                            Ente = DbValue<string>.Get(myRow["Ente"]);
                            FileName = DbValue<string>.Get(myRow["FileName"]);
                            FileMIMEType = DbValue<string>.Get(myRow["FileMIMEType"]);
                            IdAnagFileType = DbValue<int>.Get(myRow["IdAnagFileType"]);
                            AnagFileType = DbValue<string>.Get(myRow["AnagFileName"]);
                            Rows = DbValue<int>.Get(myRow["Rows"]);
                            InsertDateTime = DbValue<DateTime>.Get(myRow["InsertDateTime"]);
                            ReadDateTime = DbValue<DateTime?>.Get(myRow["ReadDateTime"]);
                            AnagFileLog = DbValue<string>.Get(myRow["AnagFileLog"]);
                            PathFile = DbValue<string>.Get(myRow["PathFile"]);
                        }
                    }
                    else
                        Reset();
                    ctx.Dispose();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in AnagFile.Load::", ex);
                return false;
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }

        public override AnagFile[] LoadAll()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            List<AnagFile> list = new List<AnagFile>();
                try
            {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_S";
                sqlRead = sqlCmd.ExecuteReader();

                while (sqlRead.Read())
                {
                    AnagFile item = new AnagFile
                        {
                            IdAnagFile = DbValue<int>.Get(sqlRead["IdAnagFile"]),
                            Ente = DbValue<string>.Get(sqlRead["Ente"]),
                            FileName = DbValue<string>.Get(sqlRead["FileName"]),
                            FileMIMEType = DbValue<string>.Get(sqlRead["FileMIMEType"]),
                            IdAnagFileType = DbValue<int>.Get(sqlRead["IdAnagFileType"]),
                            AnagFileType = DbValue<string>.Get(sqlRead["AnagFileName"]),
                            Rows = DbValue<int>.Get(sqlRead["Rows"]),
                            InsertDateTime = DbValue<DateTime>.Get(sqlRead["InsertDateTime"]),
                            ReadDateTime = DbValue<DateTime?>.Get(sqlRead["ReadDateTime"]),
                            AnagFileLog = DbValue<string>.Get(sqlRead["AnagFileLog"])
                        };
                    list.Add(item);
                }*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_S", "IdAnagFile");
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdAnagFile", -1));
                    foreach (DataRowView myRow in dvMyView)
                    {
                        AnagFile item = new AnagFile(TypeDB, ConnectionString)
                        {
                            IdAnagFile = DbValue<int>.Get(myRow["IdAnagFile"]),
                            Ente = DbValue<string>.Get(myRow["Ente"]),
                            FileName = DbValue<string>.Get(myRow["FileName"]),
                            FileMIMEType = DbValue<string>.Get(myRow["FileMIMEType"]),
                            IdAnagFileType = DbValue<int>.Get(myRow["IdAnagFileType"]),
                            AnagFileType = DbValue<string>.Get(myRow["AnagFileName"]),
                            Rows = DbValue<int>.Get(myRow["Rows"]),
                            InsertDateTime = DbValue<DateTime>.Get(myRow["InsertDateTime"]),
                            ReadDateTime = DbValue<DateTime?>.Get(myRow["ReadDateTime"]),
                            AnagFileLog = DbValue<string>.Get(myRow["AnagFileLog"]),
                            PathFile = DbValue<string>.Get(myRow["PathFile"])
                        };
                        list.Add(item);
                    }
                    ctx.Dispose();
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in AnagFile.LoadAll::", ex);
                return null;
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }

        public override bool Save()
        {
            //SqlCommand sqlCmd = null;

            try
            {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_IU";

                sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
                if (_postedFile != null)
                    sqlCmd.Parameters.AddWithValue("@PostedFile", DbParam.Get(_postedFile));
                sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlCmd.Parameters.AddWithValue("@FileName", DbParam.Get(FileName));
                sqlCmd.Parameters.AddWithValue("@FileMIMEType", DbParam.Get(FileMIMEType));
                sqlCmd.Parameters.AddWithValue("@IdAnagFileType", DbParam.Get(IdAnagFileType));
                sqlCmd.Parameters.AddWithValue("@Rows", DbParam.Get(Rows));
                sqlCmd.Parameters.AddWithValue("@ReadDateTime", DbParam.Get(ReadDateTime));
                sqlCmd.Parameters.AddWithValue("@AnagFileLog", DbParam.Get(AnagFileLog));

                sqlCmd.Parameters["@IdAnagFile"].Direction = ParameterDirection.InputOutput;
                sqlCmd.ExecuteNonQuery();
                IdAnagFile = (int)sqlCmd.Parameters["@IdAnagFile"].Value;*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_IU", "IdAnagFile"
                            ,"PostedFile"
                            ,"Ente"
                            ,"FileName"
                            ,"FileMIMEType"
                            ,"IdAnagFileType"
                            ,"Rows"
                            ,"ReadDateTime"
                            ,"AnagFileLog"
                            , "PathFile"
                        );
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdAnagFile", IdAnagFile)
                            , ctx.GetParam("PostedFile", _postedFile)
                            , ctx.GetParam("Ente", Ente)
                            , ctx.GetParam("FileName", FileName)
                            , ctx.GetParam("FileMIMEType", FileMIMEType)
                            , ctx.GetParam("IdAnagFileType", IdAnagFileType)
                            , ctx.GetParam("Rows", Rows)
                            , ctx.GetParam("ReadDateTime", ReadDateTime)
                            , ctx.GetParam("AnagFileLog", AnagFileLog)
                            , ctx.GetParam("PathFile", PathFile)
                        );
                    foreach (DataRowView myRow in dvMyView)
                    {
                        IdAnagFile=int.Parse(myRow["ID"].ToString());
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in AnagFile.Save::", ex);
                return false;
            }
            /*finally
            {
                Disconnect(sqlCmd);
            }*/
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
                Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_D";
                sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
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

        #region Public methods
        /// <summary>
        /// Loads all anagraphic files to be read
        /// </summary>
        /// <returns></returns>
        public AnagFile[] LoadNew()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            List<AnagFile> list = new List<AnagFile>();
                try
            {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_L";
                sqlRead = sqlCmd.ExecuteReader();

                while (sqlRead.Read())
                {
                    AnagFile item = new AnagFile
                        {
                            IdAnagFile = DbValue<int>.Get(sqlRead["IdAnagFile"]),
                            Ente = DbValue<string>.Get(sqlRead["Ente"]),
                            FileName = DbValue<string>.Get(sqlRead["FileName"]),
                            FileMIMEType = DbValue<string>.Get(sqlRead["FileMIMEType"]),
                            IdAnagFileType = DbValue<int>.Get(sqlRead["IdAnagFileType"]),
                            AnagFileType = DbValue<string>.Get(sqlRead["AnagFileName"]),
                            Rows = DbValue<int>.Get(sqlRead["Rows"]),
                            InsertDateTime = DbValue<DateTime>.Get(sqlRead["InsertDateTime"]),
                            ReadDateTime = DbValue<DateTime?>.Get(sqlRead["ReadDateTime"]),
                            AnagFileLog = DbValue<string>.Get(sqlRead["AnagFileLog"])
                        };
                    list.Add(item);
                }*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_L");
                    dvMyView = ctx.GetDataView(sSQL, "TBL");
                    foreach (DataRowView myRow in dvMyView)
                    {
                        AnagFile item = new AnagFile(TypeDB, ConnectionString)
                        {
                            IdAnagFile = DbValue<int>.Get(myRow["IdAnagFile"]),
                            Ente = DbValue<string>.Get(myRow["Ente"]),
                            FileName = DbValue<string>.Get(myRow["FileName"]),
                            FileMIMEType = DbValue<string>.Get(myRow["FileMIMEType"]),
                            IdAnagFileType = DbValue<int>.Get(myRow["IdAnagFileType"]),
                            AnagFileType = DbValue<string>.Get(myRow["AnagFileName"]),
                            Rows = DbValue<int>.Get(myRow["Rows"]),
                            InsertDateTime = DbValue<DateTime>.Get(myRow["InsertDateTime"]),
                            ReadDateTime = DbValue<DateTime?>.Get(myRow["ReadDateTime"]),
                            AnagFileLog = DbValue<string>.Get(myRow["AnagFileLog"]),
                            PathFile = DbValue<string>.Get(myRow["PathFile"])
                        };
                        list.Add(item);
                    }
                    ctx.Dispose();
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in AnagFile.LoadNew::", ex);
                return null;
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }

        public AnagFile[] LoadFiltered()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            List<AnagFile> list = new List<AnagFile>();
                try
            {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_S";
                if (IdAnagFile != default(int))
                    sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
                if(!string.IsNullOrEmpty(Ente))
                    sqlCmd.Parameters.AddWithValue("@Ente", DbParam.Get(Ente));
                sqlRead = sqlCmd.ExecuteReader();

                while (sqlRead.Read())
                {
                    AnagFile item = new AnagFile
                    {
                        IdAnagFile = DbValue<int>.Get(sqlRead["IdAnagFile"]),
                        Ente = DbValue<string>.Get(sqlRead["Ente"]),
                        FileName = DbValue<string>.Get(sqlRead["FileName"]),
                        FileMIMEType = DbValue<string>.Get(sqlRead["FileMIMEType"]),
                        IdAnagFileType = DbValue<int>.Get(sqlRead["IdAnagFileType"]),
                        AnagFileType = DbValue<string>.Get(sqlRead["AnagFileName"]),
                        Rows = DbValue<int>.Get(sqlRead["Rows"]),
                        InsertDateTime = DbValue<DateTime>.Get(sqlRead["InsertDateTime"]),
                        ReadDateTime = DbValue<DateTime?>.Get(sqlRead["ReadDateTime"]),
                        AnagFileLog = DbValue<string>.Get(sqlRead["AnagFileLog"])
                    };
                    list.Add(item);
                }*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_S", "IdAnagFile","Ente");
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdAnagFile", IdAnagFile)
                            , ctx.GetParam("Ente", Ente)
                        );
                    foreach (DataRowView myRow in dvMyView)
                    {
                        AnagFile item = new AnagFile(TypeDB, ConnectionString)
                        {
                            IdAnagFile = DbValue<int>.Get(myRow["IdAnagFile"]),
                            Ente = DbValue<string>.Get(myRow["Ente"]),
                            FileName = DbValue<string>.Get(myRow["FileName"]),
                            FileMIMEType = DbValue<string>.Get(myRow["FileMIMEType"]),
                            IdAnagFileType = DbValue<int>.Get(myRow["IdAnagFileType"]),
                            AnagFileType = DbValue<string>.Get(myRow["AnagFileName"]),
                            Rows = DbValue<int>.Get(myRow["Rows"]),
                            InsertDateTime = DbValue<DateTime>.Get(myRow["InsertDateTime"]),
                            ReadDateTime = DbValue<DateTime?>.Get(myRow["ReadDateTime"]),
                            AnagFileLog = DbValue<string>.Get(myRow["AnagFileLog"]),
                            PathFile = DbValue<string>.Get(myRow["PathFile"])
                        };
                        list.Add(item);
                    }
                    ctx.Dispose();
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in AnagFile.LoadFiltered::", ex);
                return null;
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }

            #region Log management
            public bool SaveLog(RowLog error)
            {
                //SqlCommand sqlCmd = null;

                try
                {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_LOG_IU";

                sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
                sqlCmd.Parameters.AddWithValue("@Row", DbParam.Get(error.Row));
                sqlCmd.Parameters.AddWithValue("@Severity", DbParam.Get(error.Severity));
                sqlCmd.Parameters.AddWithValue("@Error", DbParam.Get(error.Error));

                sqlCmd.ExecuteNonQuery();*/
                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_LOG_IU", "IdAnagFile"
                            ,"Row"
                            ,"Severity"
                            ,"Error"
                        );
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdAnagFile", IdAnagFile)
                            , ctx.GetParam("Row", error.Row)
                            , ctx.GetParam("Severity", error.Severity)
                            , ctx.GetParam("Error", error.Error)
                        );
                    ctx.Dispose();
                }
                return true;
                }
                catch (Exception ex)
                {
                log.Debug("Si è verificato un errore in AnagFile.SaveLog::", ex);
                    return false;
                }
                /*finally
                {
                    Disconnect(sqlCmd);
                }*/
            }

		    public RowLog[] LoadAllLogs()
		    {
			    SqlCommand sqlCmd = null;
			    SqlDataReader sqlRead = null;

			    try
			    {
                    Connect("OPENgovANAGRAFICA");
				    sqlCmd = CreateCommand();
				    sqlCmd.CommandType = CommandType.StoredProcedure;
				    sqlCmd.CommandText = "ANAG_FILES_LOG_S";
                    sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
                    sqlRead = sqlCmd.ExecuteReader();

				    List<RowLog> list = new List<RowLog>();
				    while(sqlRead.Read())
				    {
					    RowLog item = new RowLog
					        {
					            Row = DbValue<int>.Get(sqlRead["Row"]),
					            Severity = (LogSeverity) DbValue<int>.Get(sqlRead["Severity"]),
					            Error = DbValue<string>.Get(sqlRead["Error"])
					        };
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
            #endregion
        #endregion

        #region Private methods

        private void GetFile()
        {
            /*SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;*/

            try
            {
                /*Connect("OPENgovANAGRAFICA");
                sqlCmd = CreateCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "ANAG_FILES_GETFILE";
                sqlCmd.Parameters.AddWithValue("@IdAnagFile", DbParam.Get(IdAnagFile));
                sqlRead = sqlCmd.ExecuteReader();
                if (sqlRead.Read())
                {
                    _postedFile = DbValue<byte[]>.Get(sqlRead["PostedFile"]);
                }
                else
                {
                    Reset();
                }*/

                DataView dvMyView = new DataView();
                using (DBModel ctx = new DBModel(TypeDB, ConnectionString))
                {
                    string sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"ANAG_FILES_GETFILE", "IdAnagFile");
                    dvMyView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdAnagFile", IdAnagFile));
                    if (dvMyView.Count > 0)
                    {
                        foreach (DataRowView myRow in dvMyView)
                        {
                            _postedFile = DbValue<byte[]>.Get(myRow["PostedFile"]);
                        }
                    }
                    else
                        Reset();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Si è verificato un errore in AnagFile.GetFile::", ex);
            }
            /*finally
            {
                Disconnect(sqlCmd, sqlRead);
            }*/
        }
        #endregion
    }
    /// <summary>
    /// Classe per la gestione degli errori 
    /// </summary>
    public class RowLog
    {
        public string Error;
        public LogSeverity Severity;
        public int Row;
    }
}