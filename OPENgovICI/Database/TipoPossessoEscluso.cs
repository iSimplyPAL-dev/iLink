using System;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione della tabella TipoPossessoEscluso.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class TipoPossessoEscluso : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoPossessoEscluso));
        private string _username;

		public TipoPossessoEscluso(string UserName)
		{
			//
			// TODO: Add constructor logic here
			//
			this._username = UserName;
			this.TableName = "tbl_TipoPoss_Escluso_DSAAP";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="COD_ENTE"></param>
		/// <param name="ANNO"></param>
        /// <param name="TIPO_ALIQUOTA"></param>
		/// <returns></returns>
		public DataView ListaTipoPossessoEscluse(string COD_ENTE, string ANNO, string TIPO_ALIQUOTA)
		{
			
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.Connection =  new SqlConnection(Business.ConstWrapper.StringConnection);
			SelectCommand.CommandText = "Select * From "+ this.TableName +"" ;			
			SelectCommand.CommandText += " where COD_ENTE=@COD_ENTE";
			SelectCommand.CommandText += " and ANNO=@ANNO";
			SelectCommand.CommandText += " and TIPO_ALIQUOTA=@TIPO_ALIQUOTA";			
			SelectCommand.Parameters.Add("@COD_ENTE",SqlDbType.NVarChar).Value = COD_ENTE;
			SelectCommand.Parameters.Add("@ANNO",SqlDbType.SmallInt).Value = ANNO;
			SelectCommand.Parameters.Add("@TIPO_ALIQUOTA", SqlDbType.NVarChar).Value = TIPO_ALIQUOTA;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoPossessoEscluso.ListaTipoPossessoEscluse.errore: ", Err);
                throw Err;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="COD_ENTE"></param>
        /// <param name="ANNO"></param>
        /// <returns></returns>
        //*** 20140509 - TASI ***
        //public bool delete(string COD_ENTE, string ANNO)
        //{

        //    bool DeleteRetVal=true; 

        //    SqlCommand DeleteCommand = new SqlCommand();
        //    DeleteCommand.Connection = oSession.oAppDB.GetConnection();

        //    DeleteCommand.CommandText = "delete  From "+ this.TableName +"" ;
        //    DeleteCommand.CommandText += " where COD_ENTE=@COD_ENTE";
        //    DeleteCommand.CommandText += " and ANNO=@ANNO";

        //    DeleteCommand.Parameters.Add("@COD_ENTE",SqlDbType.NVarChar).Value = COD_ENTE;
        //    DeleteCommand.Parameters.Add("@ANNO",SqlDbType.SmallInt).Value = ANNO;

        //    DeleteRetVal=Execute(DeleteCommand);

        //    return DeleteRetVal;
        // }
        //   catch (Exception ex)
        //  {
        //     log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoPossessoEscluso.delete.errore: ", ex);
        // }
        //}
        public bool delete(string COD_ENTE, string ANNO)
        {
            bool DeleteRetVal = true;
            SqlCommand DeleteCommand = new SqlCommand();

            try
            {
                DeleteCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                DeleteCommand.CommandType = CommandType.StoredProcedure;
                DeleteCommand.CommandText = "prc_TBL_TIPOPOSS_ESCLUSO_DSAAP_D";
                DeleteCommand.Parameters.Clear();
                DeleteCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = COD_ENTE;
                DeleteCommand.Parameters.Add("@ANNO", SqlDbType.SmallInt).Value = ANNO;
                Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception ex) {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoPossessoEscluso.delete.errore: ", ex);
                DeleteRetVal = false;
            }
            finally
            {
                DeleteCommand.Dispose();
            }
            return DeleteRetVal;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="COD_ENTE"></param>
        /// <param name="ANNO"></param>
        /// <param name="ID_POSS"></param>
        /// <returns></returns>
        //public bool Insert(string COD_ENTE, string ANNO, string ID_POSS)
        //{

        //    bool InsertRetVal=true; 

        //    SqlCommand InsertCommand = new SqlCommand();
        //try{
        //    InsertCommand.Connection = oSession.oAppDB.GetConnection();

        //    InsertCommand.CommandText = "Insert INTO "+ this.TableName +"" ;
        //    InsertCommand.CommandText += " (COD_ENTE,ANNO,ID_POSS)";
        //    InsertCommand.CommandText += " values (";
        //    InsertCommand.CommandText += " @COD_ENTE,";
        //    InsertCommand.CommandText += " @ANNO,";
        //    InsertCommand.CommandText += " @ID_POSS";
        //    InsertCommand.CommandText += ")";

        //    InsertCommand.Parameters.Add("@COD_ENTE",SqlDbType.NVarChar).Value = COD_ENTE;
        //    InsertCommand.Parameters.Add("@ANNO",SqlDbType.SmallInt).Value = ANNO;
        //    InsertCommand.Parameters.Add("@ID_POSS",SqlDbType.Int).Value = ID_POSS;

        //    InsertRetVal=Execute(InsertCommand);

        //    return InsertRetVal;
         // }
         //   catch (Exception ex)
          //  {
           //     log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoPossessoEscluso.Insert.errore: ", ex);
           // }
//}//
public bool Insert(string COD_ENTE, string ANNO, string ID_POSS)
        {
            bool InsertRetVal = true;
            SqlCommand InsertCommand = new SqlCommand();

            try
            {
                InsertCommand.CommandType = CommandType.StoredProcedure;
                InsertCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                InsertCommand.Connection.Open();
                InsertCommand.CommandText = "prc_TBL_TIPOPOSS_ESCLUSO_DSAAP_IU";
                InsertCommand.Parameters.Clear();
                InsertCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = COD_ENTE;
                InsertCommand.Parameters.Add("@ANNO", SqlDbType.SmallInt).Value = ANNO;
                InsertCommand.Parameters.Add("@ID_POSS", SqlDbType.Int).Value = ID_POSS;
                InsertRetVal = Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TipoPossessoEscluso.Insert.errore: ", ex);
                InsertRetVal = false;
            }
            finally
            {
                InsertCommand.Dispose();
            }
            return InsertRetVal;
        }
        //*** ***

	}
}
