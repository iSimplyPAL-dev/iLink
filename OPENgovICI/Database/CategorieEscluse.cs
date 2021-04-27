using System;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Utility;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione delle Categorie Escluse.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class CategorieEscluse: Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(CategorieEscluse));
        private string _username;

		/// <summary>
		/// Costruttore della classe
		/// </summary>
		/// <param name="UserName">Parametro di tipo <c>String</c> che determina l'utente</param>
		public CategorieEscluse(string UserName)
		{
			this._username = UserName;
			this.TableName = "Tbl_Cat_Escluse_DSAAP";
            this.ProcedureName = "prc_TBL_CAT_ESCLUSE_DSAAP_S";
		}

        //*** 20140509 - TASI ***
        /// <summary>
        /// Carica l'elenco delle Categorie Escluse
        /// </summary>
        /// <param name="COD_ENTE">Parametro di tipo <c>String</c> che determina l'ente al quale appartiene la categoria esclusa</param>
        /// <param name="ANNO">Parametro di tipo <c>String</c> che determina l'anno della categoria esclusa</param>
        /// <param name="COD_ALIQUOTA">Parametro di tipo <c>String</c> che determina il codice della categoria esclusa</param>
        /// <param name="Tributo"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataView ListaCategorieEscluse(string COD_ENTE, string ANNO, string COD_ALIQUOTA, string Tributo)
        {
            DataView dv = new DataView();

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TBL_CAT_ESCLUSE_DSAAP_S", "ID", "COD_ENTE"
                            , "ANNO"
                            , "TIPO_ALIQUOTA"
                            , "Tributo"
                        );
                    dv = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", -1)
                            , ctx.GetParam("COD_ENTE", COD_ENTE)
                            , ctx.GetParam("ANNO", ANNO)
                            , ctx.GetParam("TIPO_ALIQUOTA", COD_ALIQUOTA)
                            , ctx.GetParam("Tributo", Tributo)
                        );
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.ListaCateogorieEscluse.errore: ", ex);
                dv = null;
            }
            return dv;
        }
        //public DataView ListaCategorieEscluse(string COD_ENTE, string ANNO, string COD_ALIQUOTA, string Tributo)
        //{
        //    SqlCommand cmdMyCommand = new SqlCommand();
        //    DataView dv = new DataView();

        //    try
        //    {
        //        cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.CommandText = this.ProcedureName;
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = COD_ENTE;
        //        cmdMyCommand.Parameters.Add("@ANNO", SqlDbType.SmallInt).Value = ANNO;
        //        cmdMyCommand.Parameters.Add("@TIPO_ALIQUOTA", SqlDbType.NVarChar).Value = COD_ALIQUOTA;
        //        cmdMyCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
        //        dv = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.ListaCateogorieEscluse.errore: ", ex);
        //        dv = null;
        //    }
        //    finally
        //    {
        //        cmdMyCommand.Dispose();
        //    }
        //    return dv;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name = "COD_ENTE" > Parametro di tipo<c>String</c> che determina l'ente al quale appartiene la categoria esclusa</param>
        ///// <param name = "ANNO" > Parametro di tipo <c>String</c> che determina l'anno della categoria esclusa</param>
        ///// <returns>La funzione restituisce un valore<c>bool</c> 
        ///// <list type = "bullet" >

        ////    / < item >< c > true </ c > se l'operazione è andata a buon fine</item>
        ///// <item ><c>false</c> se si sono veriricati degli errori</item>
        ///// </list>
        ///// </returns>
        //public bool delete(string COD_ENTE, string ANNO, string TIPO_ALIQUOTA)
        //{

        //    bool RetVal=true; 

        //    SqlCommand DeleteCommand = new SqlCommand();
        //    DeleteCommand.Connection = oSession.oAppDB.GetConnection();

        //    DeleteCommand.CommandText = "delete  From "+ this.TableName +"" ;
        //    DeleteCommand.CommandText += " where COD_ENTE=@COD_ENTE";
        //    DeleteCommand.CommandText += " and ANNO=@ANNO";
        //    // aggiungo l'eventuale filtro sul tipo di aliquota da eliminare
        // try{
        //    if (TIPO_ALIQUOTA != "TUTTE")
        //    {
        //        DeleteCommand.CommandText += " and TIPO_ALIQUOTA=@TIPO_ALIQUOTA";
        //    }


        //    DeleteCommand.Parameters.Add("@COD_ENTE",SqlDbType.NVarChar).Value = COD_ENTE;
        //    DeleteCommand.Parameters.Add("@ANNO",SqlDbType.SmallInt).Value = ANNO;
        //    if (TIPO_ALIQUOTA != "TUTTE")
        //    {
        //        DeleteCommand.Parameters.Add("@TIPO_ALIQUOTA", SqlDbType.NVarChar).Value = TIPO_ALIQUOTA;
        //    }
        //    RetVal=Execute(DeleteCommand);

        //    return RetVal;
        // }
        //    catch (Exception ex)
        //    {

        //      log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.Delete.errore: ", ex);
        //  }
        //}
        /// <summary>
        /// Permette di elimanare una categoria esclusa
        /// </summary>
        /// <param name="COD_ENTE"></param>
        /// <param name="ANNO"></param>
        /// <param name="TIPO_ALIQUOTA"></param>
        /// <param name="Tributo"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public bool Delete(string COD_ENTE, string ANNO, string TIPO_ALIQUOTA, string Tributo)
        {
            bool RetVal = true;

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TBL_CAT_ESCLUSE_DSAAP_D", "COD_ENTE"
                            ,"ANNO"
                            , "TIPO_ALIQUOTA"
                            , "Tributo"
                        );
                    DataView myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("COD_ENTE", COD_ENTE)
                            , ctx.GetParam("ANNO", ANNO)
                            , ctx.GetParam("TIPO_ALIQUOTA", TIPO_ALIQUOTA)
                            , ctx.GetParam("Tributo", Tributo)
                        );
                    ctx.Dispose();
                    foreach (DataRowView myRow in myDataView)
                    {
                        if (StringOperation.FormatInt(myRow["id"]) <= 0)
                            RetVal= false;
                        else
                            RetVal= true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.Delete.errore: ", ex);

                RetVal = false;
            }
            return RetVal;
        }
        //public bool Delete(string COD_ENTE, string ANNO, string TIPO_ALIQUOTA, string Tributo)
        //{
        //    bool RetVal = true;
        //    SqlCommand cmdMyCommand = new SqlCommand();

        //    try
        //    {
        //        cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.CommandText = "prc_TBL_CAT_ESCLUSE_DSAAP_D";
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = COD_ENTE;
        //        cmdMyCommand.Parameters.Add("@ANNO", SqlDbType.SmallInt).Value = ANNO;
        //        cmdMyCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Tributo;
        //        if (TIPO_ALIQUOTA != "TUTTE")
        //        {
        //            cmdMyCommand.Parameters.Add("@TIPO_ALIQUOTA", SqlDbType.NVarChar).Value = TIPO_ALIQUOTA;
        //        }
        //        Execute(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.Delete.errore: ", ex);

        //        RetVal = false;
        //    }
        //    finally
        //    {
        //        cmdMyCommand.Dispose();
        //    }
        //    return RetVal;
        //}

        /// <summary>
        /// Permette di inserire una nuova categoria esclusa
        /// </summary>
        /// <param name="COD_ENTE">Parametro di tipo <c>String</c> che determina l'ente al quale appartiene la categoria esclusa</param>
        /// <param name="ANNO">Parametro di tipo <c>String</c> che determina l'anno della categoria esclusa</param>
        /// <param name="COD_CAT">Parametro di tipo <c>String</c> che determina il codice della categoria esclusa</param>
        /// <param name="TIPO_ALIQUOTA">Parametro di tipo <c>String</c> che determina la tipologia della categoria esclusa</param>
        /// <param name="Tributo"></param>
        /// <returns>La funzione restituisce un valore <c>bool</c> 
        /// <list type="bullet">
        /// <item ><c>true</c> se l'operazione è andata a buon fine</item>
        /// <item ><c>false</c> se si sono veriricati degli errori</item>
        /// </list>
        /// </returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public bool Insert(string COD_ENTE, string ANNO, string COD_CAT, string TIPO_ALIQUOTA, string Tributo)
        {
            bool RetVal = true;

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TBL_CAT_ESCLUSE_DSAAP_IU", "COD_ENTE"
                            , "ANNO"
                            , "COD_CAT"
                            , "TIPO_ALIQUOTA"
                            , "Tributo"
                        );
                    DataView myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("COD_ENTE", COD_ENTE)
                            , ctx.GetParam("ANNO", ANNO)
                            , ctx.GetParam("COD_CAT", COD_CAT)
                            , ctx.GetParam("TIPO_ALIQUOTA", TIPO_ALIQUOTA)
                            , ctx.GetParam("Tributo", Tributo)
                        );
                    ctx.Dispose();
                    foreach (DataRowView myRow in myDataView)
                    {
                        if (StringOperation.FormatInt(myRow["id"]) <= 0)
                            RetVal = false;
                        else
                            RetVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.Insert.errore: ", ex);
                RetVal = false;
            }
            return RetVal;
        }
        //public bool Insert(string COD_ENTE, string ANNO, string COD_CAT, string TIPO_ALIQUOTA, string Tributo)
        //{
        //    bool RetVal = true;
        //    SqlCommand cmdMyCommand = new SqlCommand();

        //    try
        //    {
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
        //        cmdMyCommand.Connection.Open();
        //        cmdMyCommand.CommandText = "prc_TBL_CAT_ESCLUSE_DSAAP_IU";
        //        cmdMyCommand.Parameters.Clear();
        //        cmdMyCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = COD_ENTE;
        //        cmdMyCommand.Parameters.Add("@ANNO", SqlDbType.SmallInt).Value = ANNO;
        //        cmdMyCommand.Parameters.Add("@COD_CAT", SqlDbType.NVarChar).Value = COD_CAT;
        //        cmdMyCommand.Parameters.Add("@TIPO_ALIQUOTA", SqlDbType.NVarChar).Value = TIPO_ALIQUOTA;
        //        cmdMyCommand.Parameters.Add("@TRIBUTO", SqlDbType.NVarChar).Value = Tributo;
        //        RetVal = Execute(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CategorieEscluse.Insert.errore: ", ex);
        //        RetVal = false;
        //    }
        //    finally
        //    {
        //        cmdMyCommand.Dispose();
        //    }
        //    return RetVal;
        //}
        //public bool Insert(string COD_ENTE, string ANNO, string COD_CAT, string TIPO_ALIQUOTA)
        //{

        //    bool RetVal=true; 

        //    SqlCommand InsertCommand = new SqlCommand();
        //    InsertCommand.Connection = oSession.oAppDB.GetConnection();

        //    InsertCommand.CommandText = "Insert INTO "+ this.TableName +"" ;
        //    InsertCommand.CommandText += " (COD_ENTE,ANNO,COD_CAT, TIPO_ALIQUOTA)";
        //    InsertCommand.CommandText += " values (";
        //    InsertCommand.CommandText += " @COD_ENTE,";
        //    InsertCommand.CommandText += " @ANNO,";
        //    InsertCommand.CommandText += " @COD_CAT,";
        //    InsertCommand.CommandText += " @TIPO_ALIQUOTA";
        //    InsertCommand.CommandText += ")";

        //    InsertCommand.Parameters.Add("@COD_ENTE",SqlDbType.NVarChar).Value = COD_ENTE;
        //    InsertCommand.Parameters.Add("@ANNO",SqlDbType.SmallInt).Value = ANNO;
        //    InsertCommand.Parameters.Add("@COD_CAT",SqlDbType.NVarChar).Value = COD_CAT;
        //    InsertCommand.Parameters.Add("@TIPO_ALIQUOTA", SqlDbType.NVarChar).Value = TIPO_ALIQUOTA;

        //    RetVal=Execute(InsertCommand);

        //    return RetVal;

        //}
        //*** ***
    }
}
