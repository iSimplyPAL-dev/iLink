using System;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using Ribes;
using log4net;


namespace DichiarazioniICI.Database
{

    /// <summary>
    /// Classe base per la gestione del database
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class Database
    {
        /// <summary>
        /// Oggetto di tipo SqlConnection
        /// </summary>
        protected SqlConnection _DbConnection;
        /// <summary>
        /// Nome della Tabella sulla quale effettuare le query
        /// </summary>
        protected string TableName;
        /// <summary>
        /// Nome della StoredProcedure sulla quale effettuare le query
        /// </summary>
        protected string ProcedureName;
        /// <summary>
		/// Flag per gestire la rientranza dei metodi
		/// </summary>
		protected bool AlredyOpened;

        private static readonly ILog log = LogManager.GetLogger(typeof(ImmobileDettaglio));

        /// <summary>
        /// Costruttore della Classe
        /// </summary>
        public Database()
        {
        }

        /// <summary>
        /// Distugge le istanze di sessione
        /// </summary>
        public void kill()
        {
        }


        /// <summary>
        /// Ritorna tutti gli elementi della tabella
        /// </summary>
        /// <returns> La DataTable valorizzata o null </returns>
        //*** 20140509 - TASI ***
        //public DataTable List()
        //{
        //    DataTable Tabella;

        //    try
        //    {
        //        SqlCommand SelectCommand = oSession.oAppDB.CmdCreate("Select * From " + this.TableName);
        //        DataSet ds = oSession.oAppDB.GetPrivateDataSet(SelectCommand);
        //        Tabella = ds.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        // log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.List.errore: ", ex);
        //        kill();
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        Tabella = new DataTable();
        //    }
        //    finally
        //    {
        //        kill();
        //    }

        //    return Tabella;
        //}
        public DataTable List()
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.Text;
                SelectCommand.CommandText = "SELECT * FROM " + this.TableName;
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.List.errore: ", ex);
                kill();
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        public DataTable List(string myConn)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.Text;
                SelectCommand.CommandText = "SELECT * FROM " + this.TableName;
                Tabella = Query(SelectCommand, new SqlConnection(myConn));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.List.errore: ", ex);
                kill();
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        public DataTable ListFromSP()
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = this.ProcedureName;
                Tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.ListFromSP.errore: ", ex);
                kill();
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }
        //*** ***
        /// <summary>
        /// Prepara il command di selezione sull'identity
        /// </summary>
        /// <param name="id"> id del record da cercare </param>
        /// <returns>DataTable valorizzato o Null</returns>
        public DataTable List(int id)
        {
            DataTable Tabella;

            try
            {
                SqlCommand cmdMyCommand = PrepareGetRow(id);
                //*** 20140630 - TASI ***
                //selectCommand.Connection = oSession.oAppDB.GetConnection();
                //Tabella = oSession.oAppDB.GetPrivateDataSet(selectCommand).Tables[0];
                cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                Tabella = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch(Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.List.errore: ", ex);
                kill();
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }
            return Tabella;
        }

        /// <summary>
        /// Esegue query di selezione
        /// </summary>
        /// <param name="myCommand"> Il command con i parameters e la query </param>
        /// <param name="oMyConn"></param>
        /// <returns> DataTable valorizzata con l'esito </returns>
        protected DataTable Query(SqlCommand myCommand, SqlConnection oMyConn)
        {
            DataTable Tabella;

            try
            {
                if (TableName == null)
                    TableName = "";
                Tabella = new DataTable(TableName);
                myCommand.Connection = oMyConn;
                myCommand.CommandTimeout = 0;
                //SqlDataAdapter SelectAdapter = oSession.oAppDB.GetPrivateDataAdapter(selectCommand);
                log.Debug(Utility.Costanti.LogQuery(myCommand));
                SqlDataAdapter SelectAdapter = new SqlDataAdapter();
                SelectAdapter.SelectCommand = myCommand;
                log.Debug(Utility.Costanti.LogQuery(myCommand));
                SelectAdapter.Fill(Tabella);
                myCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.Query.errore: ", ex);
                kill();
                string msg = ex.ToString();
                log.Error("Errore QUERY :: ", ex);
                Tabella = new DataTable();
            }
            finally
            {
                kill();
            }

            return Tabella;
        }

        /// <summary>
        /// Esegue una query di selezione con singolo campo
        /// </summary>
        /// <param name="selectCommand"> Il command con i parameters e la query </param>
        /// <param name="myConn"></param>
        /// <returns> Valore del campo specificato nella query </returns>
        protected object QueryScalar(SqlCommand selectCommand, SqlConnection myConn)
        {
            object retVal;

            try
            {
                if (myConn.State == ConnectionState.Open)
                    AlredyOpened = true;
                else
                    myConn.Open();
                selectCommand.Connection = myConn;

                retVal = selectCommand.ExecuteScalar();
            }
            catch(Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.QueryScalar.errore: ", ex);
                kill();
                retVal = null;
            }
            finally
            {
                kill();
            }

            return retVal;
        }

        /// <summary>
        /// Esegue una query di comando
        /// </summary>
        /// <param name="executeCommand"> Il command con i parametri e la query </param>
        /// <param name="myConn"></param>
        /// <returns> Esito dell'operazione </returns>
        protected bool Execute(SqlCommand executeCommand, SqlConnection myConn)
        {
            bool retVal;

            try
            {
                if (myConn.State == ConnectionState.Open)
                    AlredyOpened = true;
                else
                    myConn.Open();
                executeCommand.Connection = myConn;

                log.Debug(Utility.Costanti.LogQuery(executeCommand));
                retVal = (executeCommand.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.Execute.errore: ", ex);
                log.Error("Errore QUERY :: ", ex);
                retVal = false;
            }
            finally
            {
                if (!AlredyOpened)
                    myConn.Close();
            }
            return retVal;
        }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="executeCommand"></param>
   /// <param name="myConn"></param>
   /// <param name="identity"></param>
   /// <returns></returns>
   protected bool Execute(SqlCommand executeCommand, SqlConnection myConn, out int identity)
        {
            identity = 0;
            bool retVal;

            try
            {
                if (myConn.State == ConnectionState.Open)
                    AlredyOpened = true;
                else
                    myConn.Open();
                executeCommand.Connection = myConn;

                log.Debug(Utility.Costanti.LogQuery(executeCommand));
                retVal = (executeCommand.ExecuteNonQuery() > 0);
                identity = Convert.ToInt32(GetIdentity(myConn));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.Execute.errore: ", ex);

                log.Debug("Execute::si è verificato il seguente errore::conn::" + myConn + "::SQL::" + executeCommand.CommandText, ex);
                retVal = false;
            }
            finally
            {
                executeCommand.Connection.Close();
                executeCommand.Dispose();
            }
            return retVal;
        }
        protected bool Execute(SqlCommand executeCommand, SqlConnection myConn, string OutParam, out int identity)
        {
            identity = 0;
            bool retVal;

            try
            {
                if (myConn.State == ConnectionState.Open)
                    AlredyOpened = true;
                else
                    myConn.Open();
                executeCommand.Connection = myConn;

                log.Debug(Utility.Costanti.LogQuery(executeCommand));
                retVal = (executeCommand.ExecuteNonQuery() > 0);
                identity = int.Parse(executeCommand.Parameters[OutParam].Value.ToString());
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.Execute.errore: ", ex);
                //kill();
                string msg = ex.ToString();
                retVal = false;
            }

            /*if(oSession.oAppDB.GetConnection().State == ConnectionState.Open)
                oSession.oAppDB.GetConnection().Close();*/

            //kill();
            return retVal;
        }

        /// <summary>
        /// Ritorna l'identity dell'ultimo elemento inserito
        /// </summary>
        /// <returns></returns>
        protected object GetIdentity(SqlConnection myConn)
        {
            SqlCommand SelectCommand = new SqlCommand();

            try
            {
                if (myConn.State == ConnectionState.Open)
                    AlredyOpened = true;
                else
                    myConn.Open();
                SelectCommand.Connection = myConn;

                SelectCommand.CommandText = "Select @@IDENTITY As 'Identity'";
                return SelectCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //kill();
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.GetIdentity.errore: ", ex);
                log.Debug("GetIdentity::si è verificato il seguente errore::conn::" + myConn + "::SQL::" + SelectCommand.CommandText, ex);
                return -1;
            }
            finally
            {
                SelectCommand.Connection.Close();
                SelectCommand.Dispose();
            }
        }

        /// <summary>
        /// Prepara il command di selezione sull'identity
        /// </summary>
        /// <param name="id"> id del record da cercare </param>
        /// <returns>Oggetto di tipo SqlCommand</returns>
        protected virtual SqlCommand PrepareGetRow(int id)
        {
            //log.Debug ("INIZIO PrepareGetRow");
            SqlCommand SelectCommand = new SqlCommand();
            //*** 20140630 - TASI ***
            //SelectCommand.Connection = oSession.oAppDB.GetConnection();
            SelectCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
            SelectCommand.CommandText = "Select * From " + this.TableName + " Where ID=@id";
            SelectCommand.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;
            //log.Debug ("SQL: Select * From " + this.TableName + " Where ID="+id);
            return SelectCommand;
        }

        //*** 20120629 - IMU ***
        protected virtual SqlCommand PrepareGetRowCaricoFigli(int id, string myConn)
        {
            SqlCommand SelectCommand = new SqlCommand();
            SelectCommand.Connection = new SqlConnection(myConn);//oSession.oAppDB.GetConnection();
            SelectCommand.CommandText = "SELECT *";
            SelectCommand.CommandText += " FROM TBLPERCENTUALECARICOFIGLI";
            SelectCommand.CommandText += " WHERE IDDETTAGLIOTESTATA=@ID";
            SelectCommand.CommandText += " ORDER BY NFIGLIO";
            SelectCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            return SelectCommand;
        }
        //*** ***
 
        /*public bool DeleteItem(int id)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            //*** 20140630 - TASI ***
			//DeleteCommand.Connection = oSession.oAppDB.GetConnection();
            DeleteCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
			DeleteCommand.CommandText = "Delete From " + TableName + " Where ID=@id";
			DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;
            //return Execute(DeleteCommand);
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            //*** ***
		}*/

        /*public string GetValParamCmd(SqlCommand MyCMD)
        {
            string sReturn = "";
            try{
            foreach (SqlParameter myParam in MyCMD.Parameters)
            {
                sReturn += " " + myParam.ParameterName + "=";
                if (myParam.DbType == DbType.String || myParam.DbType == DbType.AnsiString || myParam.DbType == DbType.DateTime)
                    sReturn += "'" + myParam.Value + "',";
                else
                    sReturn += myParam.Value + ",";
            }
            return sReturn;
              }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.Database.GetValParamCmd.errore: ", ex);
            }
        }*/
    }
}