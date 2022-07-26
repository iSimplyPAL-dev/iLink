using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using log4net;
using Utility;
using Business;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per la gestione delle Aliquote.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class Aliquote : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Aliquote));
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public Aliquote()
        {
            this.TableName = "TP_ALIQUOTE_ICI";
        }

        //public DataView AnniCaricati()
        //{
        //    SqlCommand SelectCommand = new SqlCommand();
        //    SelectCommand.CommandText = "SELECT ANNO FROM " + this.TableName + " WHERE COD_ENTE=" + ConstWrapper.CodiceEnte + " GROUP BY ANNO ORDER BY ANNO DESC";

        //    DataView dv = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
        //    kill();
        //    return dv;
        //}

        //*** 20140509 - TASI ***
        /// <summary>
		/// Carica l'elenco delle aliquote presenti nella tabella TBLALIQUOTE
		/// </summary>
		/// <param name="Tipo">Parametro di tipo <c>String</c> che determina il tipo di aliquota.
		/// Per visualizzare tutti i record della tabella valorizzare il parametro a "-1"
		/// </param>
		/// <returns>Viene restituito un DataView con l'elenco delle aliquote presenti nella tabella TBLALIQUOTE</returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataView ListaTipoAliquote(string Tipo)
        {
            DataView myDataView = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetTipoAliquote", "TIPO");
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TIPO", Tipo));
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaTipoAliquote.errore: ", ex);
                myDataView = null;
            }
            return myDataView;
        }
        //public DataView ListaTipoAliquote(string Tipo)
        //{
        //    try
        //    {
        //        SqlCommand selectCommand = this.PrepareListaTipoAliquote(Tipo);

        //        DataView dvAliquote = Query(selectCommand).DefaultView;
        //        kill();
        //        return dvAliquote;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaTipoAliquote.errore: ", ex);
        //        return null;
        //    }
        //}

        //      /// <summary>
        ///// Prepara una SqlCommand per effttuare il caricamento dell'elenco delle aliquote presenti nella tabella TBLALIQUOTE
        ///// </summary>
        ///// <param name="Tipo">Parametro di tipo <c>String</c> che determina il tipo di aliquota.</param>
        ///// <returns>Viene restituito un oggetto SqlCommand</returns>
        //protected SqlCommand PrepareListaTipoAliquote(string Tipo)
        //{
        //    SqlCommand SelectCommand = new SqlCommand();
        //    try
        //    {
        //        SelectCommand.Connection = oSession.oAppDB.GetConnection();
        //        SelectCommand.CommandText = "Select TIPO, TIPO + ' - ' + DESCRIZIONE as DESCR From TBLALIQUOTE WHERE 1=1";
        //        if (Tipo.ToString().CompareTo("-1") != 0)
        //        {
        //            SelectCommand.CommandText += " AND TIPO=@Tipo";
        //            SelectCommand.Parameters.Add("@Tipo", SqlDbType.NVarChar).Value = Tipo;
        //        }
        //        SelectCommand.CommandText += "ORDER BY TIPO, DESCRIZIONE";
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.PrepareListaTipoAliquote.errore: ", ex);
        //    }
        //    return SelectCommand;
        //}
        //protected SqlCommand PrepareListaTipoAliquote(string Tipo)
        //{
        //    SqlCommand SelectCommand = new SqlCommand();
        //    try
        //    {
        //        SelectCommand.CommandType = CommandType.StoredProcedure;
        //        SelectCommand.CommandText = "prc_GetTipoAliquote";
        //        SelectCommand.Parameters.Add("@Tipo", SqlDbType.NVarChar).Value = Tipo;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.PrepareListaTipoAliquote.errore: ", ex);
        //    }
        //    return SelectCommand;
        //}
        //*** ***


        /// <summary>
        /// Carica l'elenco delle aliquote e dei corrispondenti valori
        /// </summary>
        /// <param name="Tributo"></param>
        /// <param name="IdAliquota">Parametro di tipo <c>Integer</c> che identifica l'aliquota</param>
        /// <param name="Anno">Parametro di tipo <c>String</c> che determina l'anno dell'aliquota</param>
        /// <param name="Tipo">Parametro di tipo <c>String</c> che determina il tipo di aliquota</param>
        /// <param name="Valore">Parametro di tipo <c>Double</c> che determina il valore dell'aliquota</param>
        /// <param name="CodiceEnte">Parametro di tipo <c>String</c> che determina l'ente al quale appartiene l'aliquota</param>
        /// <param name="ID_ALIQUOTA_OLD">Parametro di tipo <c>Integer</c>  che identifica l'aliquota da non selezionare</param>
        /// <param name="CodContrib"></param>
        /// <returns>Viene restituito una DataTable con l'elenco delle aliquote e dei corrispondenti valori filtrate in base ai parametri di input</returns>
        /// <revisionHistory>
        /// <revision date="22/04/2013">
        /// aggiornamento IMU
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="09/05/2014">
        /// TASI
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataTable ListaAliquote(string Tributo, int IdAliquota, string Anno, string Tipo, double Valore, string CodiceEnte, int ID_ALIQUOTA_OLD, int CodContrib)
        {
            DataTable dtAliquote = new DataTable();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetAliquoteVSImpCalc", "Tributo"
                            , "Anno"
                            , "codiceente"
                            , "IdAliquota"
                            , "IdAliquota_old"
                            , "Tipo"
                            , "Valore"
                            , "IdContribuente"
                        );
                    DataSet myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("Tributo", Tributo)
                            , ctx.GetParam("Anno", Anno)
                            , ctx.GetParam("codiceente", CodiceEnte)
                            , ctx.GetParam("IdAliquota", IdAliquota)
                            , ctx.GetParam("IdAliquota_old", ID_ALIQUOTA_OLD)
                            , ctx.GetParam("Tipo", Tipo)
                            , ctx.GetParam("Valore", Valore)
                            , ctx.GetParam("IdContribuente", CodContrib)
                        );
                    dtAliquote = myDataSet.Tables[0];
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaAliquote.errore: ", ex);
                dtAliquote = null;
            }
            return dtAliquote;
        }
        //public DataTable ListaAliquote(string Tributo, int IdAliquota, string Anno, string Tipo, double Valore, string CodiceEnte, int ID_ALIQUOTA_OLD, int CodContrib)
        //{
        //    SqlCommand selectCommand = this.PrepareListaAliquote(Tributo, IdAliquota, Anno, Tipo, Valore, CodiceEnte, ID_ALIQUOTA_OLD, CodContrib);

        //    DataTable dtAliquote = Query(selectCommand, new SqlConnection(ConstWrapper.StringConnection));
        //    kill();
        //    return dtAliquote;
        //}
        //*** ***
        //*** ***


        /*/// <returns></returns>
        /// <summary>
        /// Prepara una SqlCommand per effttuare il caricamento dell'elenco delle aliquote e dei corrispondenti valori
        /// </summary>
        /// <param name="Tributo"></param>
        /// <param name="IdAliquota">Parametro di tipo <c>Integer</c> che identifica l'aliquota</param>
        /// <param name="Anno">Parametro di tipo <c>String</c> che determina l'anno dell'aliquota</param>
        /// <param name="Tipo">Parametro di tipo <c>String</c> che determina il tipo di aliquota</param>
        /// <param name="Valore">Parametro di tipo <c>Double</c> che determina il valore dell'aliquota</param>
        /// <param name="CodiceEnte">Parametro di tipo <c>String</c> che determina l'ente al quale appartiene l'aliquota</param>
        /// <param name="ID_ALIQUOTA_OLD">Parametro di tipo <c>Integer</c>  che identifica l'aliquota da non selezionare</param>
        /// <param name="CodContrib"></param>
        /// <returns>Viene restituito un oggetto SqlCommand</returns>
        protected SqlCommand PrepareListaAliquote(string Tributo, int IdAliquota, string Anno, string Tipo, double Valore, string CodiceEnte, int ID_ALIQUOTA_OLD, int CodContrib)
        {
            SqlCommand SelectCommand = new SqlCommand();
            SelectCommand.CommandType = CommandType.StoredProcedure;
            SelectCommand.CommandText = "prc_GetAliquoteVSImpCalc";
            SelectCommand.Parameters.Add("@Tributo", SqlDbType.VarChar).Value = Tributo;
            try
            {
                if (IdAliquota.CompareTo(-1) != 0)
                {
                    SelectCommand.Parameters.Add("@IdAliquota", SqlDbType.Int).Value = IdAliquota;
                }
                if (ID_ALIQUOTA_OLD.CompareTo(-1) != 0)
                {
                    SelectCommand.Parameters.Add("@IdAliquota_old", SqlDbType.Int).Value = ID_ALIQUOTA_OLD;
                }
                if (Anno.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
                }
                if (Tipo.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@Tipo", SqlDbType.NVarChar).Value = Tipo;
                }
                if (Valore.CompareTo(double.Parse("-1")) != 0)
                {
                    SelectCommand.Parameters.Add("@Valore", SqlDbType.Decimal).Value = Valore;
                }
                if (CodiceEnte.ToString().CompareTo("-1") != 0)
                {
                    SelectCommand.Parameters.Add("@codiceente", SqlDbType.NVarChar).Value = CodiceEnte;
                }
                if (CodContrib.CompareTo(-1) != 0)
                {
                    SelectCommand.Parameters.Add("@IdContribuente", SqlDbType.Int).Value = CodContrib;
                }
                return SelectCommand;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.PrepareListaAliquote.errore: ", ex);
                throw ex;
            }
        }*/
        //*** 20140509 - TASI ***
        //*** 20130422 - aggiornamento IMU ***
        //protected SqlCommand PrepareListaAliquote(int IdAliquota, string Anno, string Tipo, double Valore, string CodiceEnte, int ID_ALIQUOTA_OLD, int CodContrib)
        ////protected SqlCommand PrepareListaAliquote(int IdAliquota, string Anno, string Tipo, double Valore, string CodiceEnte, int ID_ALIQUOTA_OLD)
        //{
        //    SqlCommand SelectCommand = new SqlCommand();
        //    SelectCommand.Connection = oSession.oAppDB.GetConnection();
        //    SelectCommand.CommandText = "Select *, TP_ALIQUOTE_ICI.TIPO + ' - ' + TBLALIQUOTE.DESCRIZIONE AS DESCR FROM TBLALIQUOTE INNER JOIN ";
        //    SelectCommand.CommandText += " TP_ALIQUOTE_ICI ON TBLALIQUOTE.TIPO = TP_ALIQUOTE_ICI.TIPO WHERE 1=1";
        //    //From " + this.TableName + " WHERE 1=1" ;
        //try{
        //    if (IdAliquota.CompareTo(-1) != 0)
        //    {
        //        SelectCommand.CommandText += " AND TP_ALIQUOTE_ICI.ID_ALIQUOTA=@IdAliquota";
        //        SelectCommand.Parameters.Add("@IdAliquota", SqlDbType.Int).Value = IdAliquota;
        //    }
        //    if (ID_ALIQUOTA_OLD.CompareTo(-1) != 0)
        //    {
        //        SelectCommand.CommandText += " AND TP_ALIQUOTE_ICI.ID_ALIQUOTA<>@IdAliquota_old";
        //        SelectCommand.Parameters.Add("@IdAliquota_old", SqlDbType.Int).Value = ID_ALIQUOTA_OLD;
        //    }

        //    if (Anno.ToString().CompareTo("-1") != 0)
        //    {
        //        SelectCommand.CommandText += " AND TP_ALIQUOTE_ICI.ANNO=@Anno";
        //        SelectCommand.Parameters.Add("@Anno", SqlDbType.NVarChar).Value = Anno;
        //    }

        //    if (Tipo.ToString().CompareTo("-1") != 0)
        //    {
        //        SelectCommand.CommandText += " AND TP_ALIQUOTE_ICI.TIPO=@Tipo";
        //        SelectCommand.Parameters.Add("@Tipo", SqlDbType.NVarChar).Value = Tipo;
        //    }

        //    if (Valore.CompareTo(double.Parse("-1")) != 0)
        //    {
        //        SelectCommand.CommandText += " AND TP_ALIQUOTE_ICI.VALORE=@Valore";
        //        SelectCommand.Parameters.Add("@Valore", SqlDbType.Decimal).Value = Valore;
        //    }

        //    if (CodiceEnte.ToString().CompareTo("-1") != 0)
        //    {
        //        SelectCommand.CommandText += " AND TP_ALIQUOTE_ICI.COD_ENTE=@codiceente";
        //        SelectCommand.Parameters.Add("@codiceente", SqlDbType.NVarChar).Value = CodiceEnte;
        //    }
        //    SelectCommand.CommandText += " ORDER BY TP_ALIQUOTE_ICI.ANNO,TP_ALIQUOTE_ICI.TIPO,TP_ALIQUOTE_ICI.VALORE";
        //}
        //  catch (Exception ex)
        //    {
        //    log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.PrepareListaAliquote.errore: ", ex);
        //  }
        //    return SelectCommand;
        //}
        //*** ***
        //*** ***
        /// <summary>
        /// Permette di inserire una nuova aliquota, di modificarne e cancellane una esistente in base al valore del parametro dbOperation
        /// </summary>
        /// <param name="Tributo"></param>
        /// <param name="IdAliquota">Parametro di tipo <c>Integer</c> che identifica l'aliquota</param>
        /// <param name="Anno">Parametro di tipo <c>String</c> che determina l'anno dell'aliquota</param>
        /// <param name="Tipo">Parametro di tipo <c>String</c> che determina il tipo di aliquota</param>
        /// <param name="Valore">Parametro di tipo <c>Double</c> che determina il valore dell'aliquota</param>
        /// <param name="AliquotaStatale"></param>
        /// <param name="PercInquilino"></param>
        /// <param name="SogliaRendita"></param>
        /// <param name="sTipoSoglia"></param>
        /// <param name="CodiceEnte">Parametro di tipo <c>String</c> che determina l'ente al quale appartiene l'aliquota</param>
        /// <param name="TettoMax">
        /// <list type="bullet">
        /// <item >0 per inserire una nuova aliquota</item>
        /// <item >1 per modificare un'aliquota</item>
        /// <item >2 per cancellare un'aliquota</item>
        /// </list>
        /// </param>
        /// <returns>La funzione restituisce un valore <c>booleano</c> 
        /// <list type="bullet">
        /// <item ><c>true</c> se l'operazione è andata a buon fine</item>
        /// <item ><c>false</c> se si sono verificati degli errori</item>
        /// </list>
        /// </returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public bool InsertAliquota(string Tributo, int IdAliquota, string Anno, string Tipo, double Valore, double AliquotaStatale, double PercInquilino, double SogliaRendita, string sTipoSoglia, string CodiceEnte, string TettoMax)
        {
            bool MyReturn = false;

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_ALIQUOTE_ICI_IU", "ID_ALIQUOTA"
                            , "COD_ENTE"
                            , "CODTRIBUTO"
                            , "ANNO"
                            , "TIPO"
                            , "VALORE"
                            , "ALIQUOTA_STATALE"
                            , "PERCINQUILINO"
                            , "SOGLIARENDITA"
                            , "TIPOSOGLIA"
                        );
                    DataView myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID_ALIQUOTA", IdAliquota)
                            , ctx.GetParam("COD_ENTE", CodiceEnte)
                            , ctx.GetParam("CODTRIBUTO", Tributo)
                            , ctx.GetParam("ANNO", Anno)
                            , ctx.GetParam("TIPO", Tipo)
                            , ctx.GetParam("VALORE", Valore)
                            , ctx.GetParam("ALIQUOTA_STATALE", AliquotaStatale)
                            , ctx.GetParam("PERCINQUILINO", PercInquilino)
                            , ctx.GetParam("SOGLIARENDITA", SogliaRendita)
                            , ctx.GetParam("TIPOSOGLIA", sTipoSoglia)
                        );
                    ctx.Dispose();
                    foreach (DataRowView myRow in myDataView)
                    {
                        if (StringOperation.FormatInt(myRow["id"]) <= 0)
                            MyReturn = false;
                        else
                            MyReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.InsertAliquota.errore: ", ex);
                MyReturn = false;
            }
            return MyReturn;
        }
        //public bool InsertAliquota(string Tributo, int IdAliquota, string Anno, string Tipo, double Valore, double AliquotaStatale, double PercInquilino, double SogliaRendita, string sTipoSoglia, string CodiceEnte, string TettoMax)
        //{
        //    int nMyReturn;
        //    SqlCommand insertCommand = new SqlCommand();

        //    try
        //    {
        //        insertCommand.CommandType = CommandType.StoredProcedure;
        //        insertCommand.Connection = new SqlConnection(ConstWrapper.StringConnection);
        //        insertCommand.Connection.Open();
        //        insertCommand.Parameters.Add("@ID_ALIQUOTA", SqlDbType.Int).Value = IdAliquota;
        //        insertCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = CodiceEnte;
        //        insertCommand.Parameters.Add("@CODTRIBUTO", SqlDbType.VarChar).Value = Tributo;
        //        insertCommand.Parameters.Add("@ANNO", SqlDbType.SmallInt).Value = Anno;
        //        insertCommand.Parameters.Add("@TIPO", SqlDbType.NVarChar).Value = Tipo;
        //        insertCommand.Parameters.Add("@VALORE", SqlDbType.Decimal).Value = Valore;
        //        insertCommand.Parameters.Add("@ALIQUOTA_STATALE", SqlDbType.Decimal).Value = AliquotaStatale;
        //        insertCommand.Parameters.Add("@PERCINQUILINO", SqlDbType.Decimal).Value = PercInquilino;
        //        insertCommand.Parameters.Add("@SOGLIARENDITA", SqlDbType.Decimal).Value = SogliaRendita;
        //        insertCommand.Parameters.Add("@TIPOSOGLIA", SqlDbType.NVarChar).Value = sTipoSoglia;
        //        insertCommand.Parameters.Add("@NOTE", SqlDbType.NVarChar).Value = string.Empty;
        //        insertCommand.Parameters.Add("@DEFAULT", SqlDbType.Bit).Value = 1;
        //        insertCommand.Parameters.Add("@DATA_TERMINE_VERSAMENTO_ACCONTO", SqlDbType.SmallDateTime).Value = DBNull.Value;
        //        insertCommand.Parameters.Add("@DATA_TERMINE_VERSAMENTO_SALDO", SqlDbType.SmallDateTime).Value = DBNull.Value;
        //        insertCommand.Parameters.Add("@TETTO_MASSIMO", SqlDbType.Decimal).Value = DBNull.Value;
        //        insertCommand.Parameters.Add("@ESENTE", SqlDbType.Decimal).Value = DBNull.Value;
        //        insertCommand.CommandText = "prc_TP_ALIQUOTE_ICI_IU";
        //        insertCommand.Parameters["@ID_ALIQUOTA"].Direction = ParameterDirection.InputOutput;
        //        //eseguo la query
        //        log.Debug(Utility.Costanti.LogQuery(insertCommand));
        //        insertCommand.ExecuteNonQuery();
        //        nMyReturn = int.Parse(insertCommand.Parameters["@ID_ALIQUOTA"].Value.ToString());
        //        if (nMyReturn <= 0)
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.InsertAliquota.errore: ", ex);
        //        return false;
        //    }
        //    finally
        //    {
        //        insertCommand.Dispose();
        //    }
        //}
        //*** 20140509 - TASI ***
        //public bool InsertAliquota(int IdAliquota, string Anno, string Tipo, double Valore,double AliquotaStatale, string CodiceEnte, int dbOperation, string TettoMax)
        //{
        //    bool InsertRetVal; 
        //    string SQL=string.Empty; 					
        //    SqlCommand insertCommand = new SqlCommand();
        //    bool blnVal = true;

        //    try
        //    {
        //        if (dbOperation == 0)
        //        {
        //            //*** 20130422 - aggiornamento IMU ***
        //            //long lngID=0;
        //            //lngID = this.getNewID(this.TableName); 
        //            //*** ***

        //            SQL = "INSERT INTO TP_ALIQUOTE_ICI (";
        //            //*** 20130422 - aggiornamento IMU ***
        //            //SQL+="ID_ALIQUOTA,";
        //            //*** ***
        //            SQL += "ANNO,TIPO,VALORE,[DEFAULT], COD_ENTE, TETTO_MASSIMO";
        //            //*** 20120530 - IMU ***
        //            SQL += ", ALIQUOTA_STATALE";
        //            //*** ***
        //            SQL += ")";

        //            SQL = SQL + " VALUES(";
        //            //*** 20130422 - aggiornamento IMU ***
        //            //SQL+= lngID+ ","; 
        //            //*** ***
        //            SQL = SQL + this.CStrToDB(Anno, ref blnVal, false);
        //            SQL = SQL + "," + this.CStrToDB(Tipo, ref blnVal, false);
        //            SQL = SQL + "," + this.CDoubleToDB(Valore);
        //            SQL = SQL + ", 1," + this.CStrToDB(CodiceEnte, ref blnVal, false);
        //            if (TettoMax != "")
        //            {
        //                SQL = SQL + "," + this.CDoubleToDB(TettoMax);
        //            }
        //            else
        //            {
        //                SQL = SQL + "," + "null";
        //            }
        //            //*** 20120530 - IMU ***
        //            SQL = SQL + "," + this.CDoubleToDB(AliquotaStatale);
        //            //*** ***
        //            SQL = SQL + ")";
        //        }

        //        else if (dbOperation == 1)
        //        {

        //            SQL = "UPDATE TP_ALIQUOTE_ICI SET ";

        //            SQL = SQL + " ANNO =" + this.CStrToDB(Anno, ref blnVal, false);
        //            SQL = SQL + ", TIPO =" + this.CStrToDB(Tipo, ref blnVal, false);
        //            SQL = SQL + ", VALORE =" + this.CDoubleToDB(Valore);
        //            //*** 20120530 - IMU ***
        //            SQL += ", ALIQUOTA_STATALE =" + this.CDoubleToDB(AliquotaStatale);
        //            //*** ***
        //            if (TettoMax != "")
        //            {
        //                SQL = SQL + ", TETTO_MASSIMO=" + this.CDoubleToDB(TettoMax);
        //            }
        //            else
        //            {
        //                SQL = SQL + ", TETTO_MASSIMO=null";
        //            }
        //            SQL = SQL + " WHERE ID_ALIQUOTA =" + IdAliquota;

        //        }

        //        else if (dbOperation == 2)
        //        {
        //            SQL = "DELETE FROM TP_ALIQUOTE_ICI ";
        //            SQL = SQL + " WHERE ID_ALIQUOTA =" + IdAliquota;
        //        }
        //        insertCommand.CommandText = SQL;
        //        InsertRetVal = Execute(insertCommand);
        //        if (InsertRetVal == false)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //           log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.InsertAliquota.errore: ", ex);

        //        return false;
        //    }
        //}
        //*** 20150430 - TASI Inquilino ***
        //public bool InsertAliquota(string Tributo, int IdAliquota, string Anno, string Tipo, double Valore, double AliquotaStatale,double SogliaRendita,string sTipoSoglia, string CodiceEnte, string TettoMax)
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdAliquota"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public bool DeleteAliquota(int IdAliquota)
        {
            bool DeleteRetVal = false;
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_ALIQUOTE_ICI_D", "ID_ALIQUOTA");
                    DataView myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID_ALIQUOTA", IdAliquota));
                    ctx.Dispose();
                    foreach (DataRowView myRow in myDataView)
                    {
                        if (StringOperation.FormatInt(myRow["id"]) <= 0)
                            DeleteRetVal = false;
                        else
                            DeleteRetVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.DeleteAliquota.errore: ", ex);
                DeleteRetVal = false;
            }
            return DeleteRetVal;
        }
        //public bool DeleteAliquota(int IdAliquota)
        //{
        //    SqlCommand insertCommand = new SqlCommand();

        //    try
        //    {
        //        insertCommand.CommandType = CommandType.StoredProcedure;
        //        insertCommand.Connection = new SqlConnection(ConstWrapper.StringConnection);
        //        insertCommand.Connection.Open();
        //        insertCommand.Parameters.Add("@ID_ALIQUOTA", SqlDbType.Int).Value = IdAliquota;
        //        insertCommand.CommandText = "prc_TP_ALIQUOTE_ICI_D";
        //        Execute(insertCommand, new SqlConnection(ConstWrapper.StringConnection));
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.DeleteAliquota.errore: ", ex);
        //        return false;
        //    }
        //    finally
        //    {
        //        insertCommand.Dispose();
        //    }
        //}
        //*** ***

        /// <summary>
        /// Modifica il valore in input per essere salvato in un campo di tipo stringa
        /// </summary>
        /// <param name="vInput">Oggetto in input</param>
        /// <param name="blnClearSpace">Valore booleano, se settato a true effettua un Trim del valore in input</param>
        /// <param name="blnUseNull">Valore booleano, se settato a true restituisce il valore Null se valore in input è Null</param>
        /// <returns>Valore di tipo stringa</returns>
        public string CStrToDB(object vInput, ref bool blnClearSpace, bool blnUseNull)
        {
            string sTesto;
            string stringa = string.Empty;
            try
            {
                if (blnUseNull)
                {
                    stringa = "Null";
                }
                else
                {
                    stringa = "''";
                }
                if (vInput.ToString() != DBNull.Value.ToString())
                {
                    sTesto = System.Convert.ToString(vInput);
                    if (blnClearSpace)
                    {
                        sTesto = sTesto.Trim();
                    }
                    if (sTesto.Trim() != "")
                    {
                        stringa = "'" + sTesto.Replace("'", "''") + "'";
                    }
                }

                return stringa;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.CStrToDB.errore: ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Modifica il valore in input per essere salvato in un campo di tipo double
        /// </summary>
        /// <param name="vInput">Oggetto in input</param>
        /// <returns>Valore di tipo stringa</returns>
        public string CDoubleToDB(object vInput)
        {
            string strToDbl = "Null";
            try
            {
                if (vInput.ToString() != DBNull.Value.ToString())

                {
                    strToDbl = System.Convert.ToString(vInput);
                    strToDbl = strToDbl.Replace(",", ".");
                }
                return strToDbl;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.CDoubleToDB.errore: ", ex);
                throw ex;
            }

        }

        ///// <summary>
        ///// Restituisce un nuovo contatore univoco per la tabella in passata input
        ///// </summary>
        ///// <param name="strNomeTabella">Paramtro di tipo <c>String</c> che indica la tabella della quale si vuole avere un contatore</param>
        ///// <returns>Restituisce un valore di tipo <c>long</c> con il nuovo contatore</returns>
        //public long getNewID(string strNomeTabella)
        //{
        //    try
        //    {


        //        string sSQL;
        //        long lngMaxId = 0;

        //        bool blnVal = true;
        //        SqlCommand selCommand = new SqlCommand();
        //        sSQL = "SELECT MAXID FROM CONTATORI WHERE NOME_TABELLA =" + this.CStrToDB(strNomeTabella, ref blnVal, false);
        //        selCommand.CommandText = sSQL;
        //        DataTable dt = Query(selCommand, new SqlConnection(ConstWrapper.StringConnection));

        //        if (dt.Rows.Count == 1)
        //        {
        //            lngMaxId = long.Parse(dt.Rows[0]["MAXID"].ToString());
        //            lngMaxId = lngMaxId + 1;
        //        }
        //        dt.Dispose();

        //        sSQL = "UPDATE CONTATORI SET MAXID=" + lngMaxId + " WHERE NOME_TABELLA ='" + strNomeTabella + "'";
        //        SqlCommand UpdateCommand = new SqlCommand();
        //        UpdateCommand.CommandText = sSQL;
        //        Execute(UpdateCommand, new SqlConnection(ConstWrapper.StringConnection));

        //        return lngMaxId;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.getNewID.errore: ", ex);
        //        throw new Exception("Application::COMPlusOPENgovProvvedimenti::Function::getNewID::DBOPENgovProvvedimentiSelect" + "::" + " " + ex.Message);
        //    }
        //}

        //*** 20140509 - TASI ***
        /// <summary>
        /// Restituisce elenco tributi
        /// </summary>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataView ListaTributo()
        {
            DataView dvAliquote = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetTributo");
                    dvAliquote = ctx.GetDataView(sSQL, "TBL");
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaTributo.errore: ", ex);
                dvAliquote = null;
            }
            return dvAliquote;
        }
        //public DataView ListaTributo()
        //{
        //    try
        //    {
        //        SqlCommand SelectCommand = new SqlCommand();
        //        SelectCommand.CommandType = CommandType.StoredProcedure;
        //        SelectCommand.CommandText = "prc_GetTributo";

        //        DataView dvAliquote = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnection)).DefaultView;
        //        kill();
        //        return dvAliquote;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug(ConstWrapper.CodiceEnte + "."+ ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaTributo.errore: ", ex);
        //        return null;
        //    }
        //}
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="30/04/2015">
        /// TASI Inquilino
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="06/02/2020">
        /// Revisione perchè la gestione lascia connessioni aperte al db
        /// </revision>
        /// </revisionHistory>
        public DataView ListaAnni(string IdEnte)
        {
            DataView myDataView = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    ConstWrapper.nTry = 0;
                    ReDo:
                    try
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetAnniAliquote", "IDENTE");
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", IdEnte));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") && ConstWrapper.nTry <= 3)
                        {
                            ConstWrapper.nTry += 1;
                            goto ReDo;
                        }
                        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaAnni.errore: ", ex);
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.ListaAnni.errore: ", ex);
                myDataView = null;
            }
            return myDataView;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="AnnoFrom"></param>
        /// <param name="AnnoTo"></param>
        /// <returns></returns>
        public bool RibaltaAliquota(string IdEnte, string AnnoFrom, string AnnoTo)
        {
            bool myRet = false;
            DataView myDataView = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(ConstWrapper.DBType, ConstWrapper.StringConnection))
                {
                    ConstWrapper.nTry = 0;
                    ReDo:
                    try
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_RibaltaAliquote", "IDENTE", "ANNOFROM", "ANNOTO");
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", IdEnte)
                                , ctx.GetParam("ANNOFROM", AnnoFrom)
                                , ctx.GetParam("ANNOTO", AnnoTo)
                            );
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToUpper().Contains("AN EXISTING CONNECTION WAS FORCIBLY CLOSED BY THE REMOTE HOST") && ConstWrapper.nTry <= 3)
                        {
                            ConstWrapper.nTry += 1;
                            goto ReDo;
                        }
                        log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.RibaltaAliquota.errore: ", ex);
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                    foreach (DataRowView myRow in myDataView)
                    {
                        if (myRow["ID"].ToString() == "1")
                            myRet = true;
                        else
                            myRet = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.Aliquote.RibaltaAliquota.errore: ", ex);
            }
            return myRet;
        }
    }
    #region "TariffeEstimo"
    /// <summary>
    /// 
    /// </summary>
    public struct TariffeEstimoRow
    {
        /// 
        public int ID;
        /// 
        public string Ente;
        /// 
        public string Zona;
        /// 
        public string Categoria;
        /// 
        public string Classe;
        /// 
        public decimal TariffaEuro;
        /// 
        public DateTime DataInizioValidita;
        /// 
        public DateTime DataFineValidita;
        /// 
        public string Note;
    }
    /// <summary>
    /// 
    /// </summary>
    public struct TariffeEstimoFabRow
    {
        /// 
        public int ID;
        /// 
        public string Ente;
        /// 
        public string Zona;
        /// 
        public decimal TariffaEuro;
        /// 
        public DateTime DataInizioValidita;
        /// 
        public DateTime DataFineValidita;
        /// 
        public string Note;
    }
    /// <summary>
    /// Summary description for TariffeEstimoTable.
    /// </summary>
    public class TariffeEstimoAFTable : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TariffeEstimoAFTable));
        private string _username;

        /// <summary>
        /// Costruttore della classe
        /// </summary>
        /// <param name="UserName"></param>
        public TariffeEstimoAFTable(string UserName)
        {
            this._username = UserName;
            this.TableName = "TARIFFE_ESTIMO_CATASTALE_FAB";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codEnte"></param>
        /// <returns></returns>
        public DataTable ListDistinct(string codEnte)
        {
            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "Select distinct ZONA,note from " + this.TableName + " where COD_ENTE = @ente ";
                SelectCommand.CommandText += " ORDER BY ZONA";

                SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = codEnte;
                DataTable dt = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
                kill();
                return dt;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TariffaEstimoAFTable.ListDistinct.errore: ", ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codEnte"></param>
        /// <param name="zona"></param>
        /// <param name="DataDal"></param>
        /// <returns></returns>
        public DataTable SelectTariffa(string codEnte, string zona, DateTime DataDal)
        {
            SqlCommand SelectCommand = new SqlCommand();
            SelectCommand.CommandText = "Select * from " + this.TableName;
            SelectCommand.CommandText += " where zona = @zona ";
            SelectCommand.CommandText += " AND COD_ENTE = @codente";
            SelectCommand.CommandText += " AND DATADAL <= @datadal";
            SelectCommand.CommandText += " AND DATAAL >= @datadal";

            SelectCommand.Parameters.Add("@codente", SqlDbType.VarChar).Value = codEnte;
            SelectCommand.Parameters.Add("@datadal", SqlDbType.DateTime).Value = DataDal;
            SelectCommand.Parameters.Add("@zona", SqlDbType.NVarChar).Value = zona;

            DataTable dt = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
            kill();
            return dt;
        }


        /// <summary>
        /// Inserisce un nuovo record a partire dai singoli campi.
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="zona"></param>
        /// <param name="dataInizioValidita"></param>
        /// <param name="dataFineValidita"></param>
        /// <param name="TariffaEuro"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool Insert(string ente, string zona, DateTime dataInizioValidita, DateTime dataFineValidita, decimal TariffaEuro, string note)
        {
            SqlCommand InsertCommand = new SqlCommand();
            InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (COD_Ente, Zona, Tariffa_Euro, DataDal, DataAl" +
                " ,Note) VALUES (@ente, @zona, @tariffaEuro, @dataDal, @dataAl, @note)";

            InsertCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
            InsertCommand.Parameters.Add("@zona", SqlDbType.VarChar).Value = zona;
            InsertCommand.Parameters.Add("@tariffaEuro", SqlDbType.Decimal).Value = TariffaEuro;
            InsertCommand.Parameters.Add("@dataDal", SqlDbType.DateTime).Value = dataInizioValidita;
            InsertCommand.Parameters.Add("@dataAl", SqlDbType.DateTime).Value = dataFineValidita;
            InsertCommand.Parameters.Add("@note", SqlDbType.NText).Value = note;

            return Execute(InsertCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
        }

        /// <summary>
        /// Inserisce un nuovo record a partire da una struttura row.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Insert(TariffeEstimoFabRow Item)
        {
            return Insert(Item.Ente, Item.Zona, Item.DataInizioValidita, Item.DataFineValidita, Item.TariffaEuro, Item.Note);
        }


        /// <summary>
        /// Effettua la modifica di un record identificato dall'identity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ente"></param>
        /// <param name="zona"></param>
        /// <param name="dataIniziovalidita"></param>
        /// <param name="dataFineValidita"></param>
        /// <param name="TariffaEuro"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        public bool Modify(int id, string ente, string zona, DateTime dataIniziovalidita, DateTime dataFineValidita, decimal TariffaEuro, string Note)
        {
            SqlCommand ModifyCommand = new SqlCommand();
            ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET cod_Ente=@ente, " +
                "zona=@zona, Tariffa_euro=@tariffaEuro, DataDal=@datadal, DataAl=@dataal, Note=@note WHERE ID=@id";

            ModifyCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            ModifyCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
            ModifyCommand.Parameters.Add("@zona", SqlDbType.VarChar).Value = zona;
            ModifyCommand.Parameters.Add("@tariffaEuro", SqlDbType.Decimal).Value = TariffaEuro;
            ModifyCommand.Parameters.Add("@dataDal", SqlDbType.DateTime).Value = dataIniziovalidita;
            ModifyCommand.Parameters.Add("@dataAl", SqlDbType.DateTime).Value = dataFineValidita;
            ModifyCommand.Parameters.Add("@note", SqlDbType.NText).Value = Note;

            return Execute(ModifyCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
        }

        /// <summary>
        /// Aggiorna un record individuato dall'identity a partire dai singoli campi.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Modify(TariffeEstimoFabRow Item)
        {
            return Modify(Item.ID, Item.Ente, Item.Zona, Item.DataInizioValidita, Item.DataFineValidita, Item.TariffaEuro, Item.Note);
        }

        /// <summary>
        /// Elimina un record identificato dall'ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(int ID)
        {
            SqlCommand DeleteCommand = new SqlCommand();
            DeleteCommand.CommandText = "Delete from " + this.TableName + " where ID=@id";
            DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = ID;

            return Execute(DeleteCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTariffa"></param>
        /// <returns></returns>
        public DataTable Ricerca(TariffeEstimoFabRow objTariffa)
        {
            SqlCommand cmdMyCommand = new SqlCommand();

            cmdMyCommand.Connection = new SqlConnection(ConstWrapper.StringConnectionOPENgov);

            cmdMyCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE 1 = 1 ";
            try
            {
                if (objTariffa.Ente.CompareTo(string.Empty) != 0)
                {
                    cmdMyCommand.CommandText += " AND COD_ENTE = @codEnte";
                    cmdMyCommand.Parameters.Add("@codEnte", SqlDbType.VarChar).Value = objTariffa.Ente;
                }
                if (objTariffa.TariffaEuro.ToString() != "0")
                {
                    cmdMyCommand.CommandText += " AND TARIFFA_EURO = @tariffaEuro";
                    cmdMyCommand.Parameters.Add("@tariffaEuro", SqlDbType.Decimal).Value = objTariffa.TariffaEuro;
                }
                if (objTariffa.Zona.CompareTo(string.Empty) != 0)
                {
                    cmdMyCommand.CommandText += " AND ZONA = @zona";
                    cmdMyCommand.Parameters.Add("@zona", SqlDbType.VarChar).Value = objTariffa.Zona;
                }
                if (objTariffa.DataInizioValidita.CompareTo(DateTime.MinValue) != 0)
                {
                    cmdMyCommand.CommandText += " AND DATADAL = @datadal";
                    cmdMyCommand.Parameters.Add("@datadal", SqlDbType.DateTime).Value = objTariffa.DataInizioValidita;
                }
                if (objTariffa.DataFineValidita.CompareTo(DateTime.MinValue) != 0)
                {
                    cmdMyCommand.CommandText += " AND DATAAL = @dataal";
                    cmdMyCommand.Parameters.Add("@dataal", SqlDbType.DateTime).Value = objTariffa.DataFineValidita;
                }
                DataTable dt = Query(cmdMyCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
                kill();
                return dt;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TariffaEstimoAFTable.Ricerca.errore: ", ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodEnte"></param>
        /// <returns></returns>
        public DataTable Ricerca(string CodEnte)
        {
            TariffeEstimoFabRow objTariffa = new TariffeEstimoFabRow();
            objTariffa.Ente = ConstWrapper.CodiceEnte;
            objTariffa.Zona = "";
            objTariffa.DataInizioValidita = DateTime.MinValue;
            objTariffa.DataFineValidita = DateTime.MinValue;
            return Ricerca(objTariffa);
        }
    }
    /// <summary>
    /// Summary description for TariffeEstimoTable.
    /// </summary>
    public class TariffeEstimoTable : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TariffeEstimoTable));
        private string _username;

        /// <summary>
        /// Costruttore della Classe
        /// </summary>
        /// <param name="UserName"></param>
        public TariffeEstimoTable(string UserName)
        {
            this._username = UserName;
            this.TableName = "TARIFFE_ESTIMO_CATASTALE";
        }

        /// <summary>
        /// Fornisce l'elenco di tutte le zone configurate per l'ente
        /// </summary>
        /// <param name="codEnte"></param>
        /// <returns></returns>
        public DataTable ListDistinct(string codEnte)
        {
            SqlCommand SelectCommand = new SqlCommand();
            SelectCommand.CommandText = "Select distinct ZONA,note from " + this.TableName + " where COD_ENTE = @ente ";
            SelectCommand.CommandText += " ORDER BY ZONA";

            SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = codEnte;
            DataTable dt = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
            kill();
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codEnte"></param>
        /// <param name="zona"></param>
        /// <param name="Categoria"></param>
        /// <param name="Classe"></param>
        /// <param name="DataDal"></param>
        /// <returns></returns>
        public DataTable SelectTariffa(string codEnte, string zona, string Categoria, string Classe, DateTime DataDal)
        {
            SqlCommand SelectCommand = new SqlCommand();
            SelectCommand.CommandText = "select * from dbo.TARIFFE_ESTIMO_CATASTALE where 1 = 1";
            SelectCommand.CommandText += " and cod_ente = @ente";
            SelectCommand.CommandText += " and categoria = @categoria";
            SelectCommand.CommandText += "  and classe = @classe";
            SelectCommand.CommandText += " and zona = @zona";
            SelectCommand.CommandText += " and datadal <= @datadal";
            SelectCommand.CommandText += " and dataal >= @datadal";

            SelectCommand.Parameters.Add("@ente", SqlDbType.NVarChar).Value = codEnte;
            SelectCommand.Parameters.Add("@zona", SqlDbType.NVarChar).Value = zona;
            SelectCommand.Parameters.Add("@categoria", SqlDbType.NVarChar).Value = Categoria;
            SelectCommand.Parameters.Add("@classe", SqlDbType.NVarChar).Value = Classe;
            SelectCommand.Parameters.Add("@datadal", SqlDbType.DateTime).Value = DataDal;

            DataTable dt = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
            kill();
            return dt;

        }


        /// <summary>
        /// Inserisce un nuovo record a partire dai singoli campi.
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="zona"></param>
        /// <param name="dataInizioValidita"></param>
        /// <param name="dataFineValidita"></param>
        /// <param name="TariffaEuro"></param>
        /// <param name="categoria"></param>
        /// <param name="classe"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        public bool Insert(string ente, string zona, DateTime dataInizioValidita, DateTime dataFineValidita, decimal TariffaEuro, string categoria, string classe, String Note)
        {
            SqlCommand InsertCommand = new SqlCommand();
            InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (cod_Ente, Zona, Tariffa_Euro, DataDal, DataAl, Categoria, Classe, Note" +
                ") VALUES (@ente, @zona, @tariffaEuro, @dataDal, @dataAl, @categoria, @classe, @note)";

            InsertCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
            InsertCommand.Parameters.Add("@zona", SqlDbType.VarChar).Value = zona;
            InsertCommand.Parameters.Add("@tariffaEuro", SqlDbType.Decimal).Value = TariffaEuro;
            InsertCommand.Parameters.Add("@dataDal", SqlDbType.DateTime).Value = dataInizioValidita;
            InsertCommand.Parameters.Add("@dataAl", SqlDbType.DateTime).Value = dataFineValidita;
            InsertCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
            InsertCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
            InsertCommand.Parameters.Add("@note", SqlDbType.NText).Value = Note;

            return Execute(InsertCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
        }

        /// <summary>
        /// Inserisce un nuovo record a partire da una struttura row.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Insert(TariffeEstimoRow Item)
        {
            return Insert(Item.Ente, Item.Zona, Item.DataInizioValidita, Item.DataFineValidita, Item.TariffaEuro, Item.Categoria, Item.Classe, Item.Note);
        }


        /// <summary>
        /// Effettua la modifica di un record identificato dall'identity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ente"></param>
        /// <param name="zona"></param>
        /// <param name="dataIniziovalidita"></param>
        /// <param name="dataFineValidita"></param>
        /// <param name="TariffaEuro"></param>
        /// <param name="Categoria"></param>
        /// <param name="Classe"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        public bool Modify(int id, string ente, string zona, DateTime dataIniziovalidita, DateTime dataFineValidita, decimal TariffaEuro, string Categoria, string Classe, string Note)
        {
            SqlCommand ModifyCommand = new SqlCommand();
            ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET COD_Ente=@ente, " +
                "zona=@zona, Tariffa_euro=@tariffaEuro, DataDal=@datadal, DataAl=dataal, Categoria=@categoria, Classe=@classe, note=@note WHERE ID=@id";

            ModifyCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            ModifyCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
            ModifyCommand.Parameters.Add("@zona", SqlDbType.VarChar).Value = Classe;
            ModifyCommand.Parameters.Add("@tariffaEuro", SqlDbType.Decimal).Value = TariffaEuro;
            ModifyCommand.Parameters.Add("@dataDal", SqlDbType.DateTime).Value = dataIniziovalidita;
            ModifyCommand.Parameters.Add("@dataAl", SqlDbType.DateTime).Value = dataFineValidita;
            ModifyCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = Categoria;
            ModifyCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = Classe;
            ModifyCommand.Parameters.Add("@note", SqlDbType.NText).Value = Note;

            return Execute(ModifyCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
        }

        /// <summary>
        /// Aggiorna un record individuato dall'identity a partire dai singoli campi.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Modify(TariffeEstimoRow Item)
        {
            return Modify(Item.ID, Item.Ente, Item.Zona, Item.DataInizioValidita, Item.DataFineValidita, Item.TariffaEuro, Item.Categoria, Item.Classe, Item.Note);
        }

        /// <summary>
        /// Elimina un record identificato dall'ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(int ID)
        {
            SqlCommand DeleteCommand = new SqlCommand();
            DeleteCommand.CommandText = "Delete from " + this.TableName + " where ID=@id";
            DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = ID;

            return Execute(DeleteCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTariffa"></param>
        /// <returns></returns>
        public DataTable Ricerca(TariffeEstimoRow objTariffa)
        {
            SqlCommand cmdMyCommand = new SqlCommand();
            cmdMyCommand.Connection = new SqlConnection(ConstWrapper.StringConnectionOPENgov);
            cmdMyCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE 1 = 1 ";
            try
            {
                if (objTariffa.Ente.CompareTo(string.Empty) != 0)
                {
                    cmdMyCommand.CommandText += " AND COD_ENTE = @codEnte";
                    cmdMyCommand.Parameters.Add("@codEnte", SqlDbType.VarChar).Value = objTariffa.Ente;
                }
                if (objTariffa.TariffaEuro.ToString() != "0")
                {
                    cmdMyCommand.CommandText += " AND TARIFFA_EURO = @tariffaEuro";
                    cmdMyCommand.Parameters.Add("@tariffaEuro", SqlDbType.Decimal).Value = objTariffa.TariffaEuro;
                }
                if (objTariffa.Zona.CompareTo(string.Empty) != 0)
                {
                    cmdMyCommand.CommandText += " AND ZONA = @zona";
                    cmdMyCommand.Parameters.Add("@zona", SqlDbType.VarChar).Value = objTariffa.Zona;
                }
                if (objTariffa.DataInizioValidita.CompareTo(DateTime.MinValue) != 0)
                {
                    cmdMyCommand.CommandText += " AND DATADAL = @datadal";
                    cmdMyCommand.Parameters.Add("@datadal", SqlDbType.DateTime).Value = objTariffa.DataInizioValidita;
                }
                if (objTariffa.DataInizioValidita.CompareTo(DateTime.MinValue) != 0)
                {
                    cmdMyCommand.CommandText += " AND DATADAL = @dataal";
                    cmdMyCommand.Parameters.Add("@dataal", SqlDbType.DateTime).Value = objTariffa.DataFineValidita;
                }
                if (objTariffa.Categoria.CompareTo(string.Empty) != 0)
                {
                    cmdMyCommand.CommandText += " AND CATEGORIA = @Categoria";
                    cmdMyCommand.Parameters.Add("@Categoria", SqlDbType.VarChar).Value = objTariffa.Categoria;
                }
                if (objTariffa.Classe.CompareTo(string.Empty) != 0)
                {
                    cmdMyCommand.CommandText += " AND CLASSE = @Classe";
                    cmdMyCommand.Parameters.Add("@Classe", SqlDbType.VarChar).Value = objTariffa.Classe;
                }

                DataTable dt = Query(cmdMyCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
                kill();
                return dt;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TariffaEstimoTable.Ricerca.errore: ", ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodiceEnte"></param>
        /// <returns></returns>
        public DataTable Ricerca(string CodiceEnte)
        {
            TariffeEstimoRow objRicerca = new TariffeEstimoRow();
            objRicerca.Categoria = "";
            objRicerca.Classe = "";
            objRicerca.Zona = "";
            objRicerca.DataFineValidita = DateTime.MinValue;
            objRicerca.DataInizioValidita = DateTime.MinValue;
            objRicerca.Ente = CodiceEnte;
            objRicerca.Note = "";
            return Ricerca(objRicerca);
        }

    }
    #endregion
    /// <summary>
    /// Summary description for TributiTable.
    /// </summary>
    public class TributiTable : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TributiTable));
        private string _username;

        /// <summary>
        /// Costruttore della classe
        /// </summary>
        /// <param name="UserName"></param>
        public TributiTable(string UserName)
        {
            this._username = UserName;
            this.TableName = "TAB_TRIBUTI";
        }

        /// <summary>
        /// Ritorna tutti gli elementi della tabella
        /// </summary>
        /// <returns> La DataTable valorizzata o null </returns>
        public DataTable TributiList()
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "Select * from " + this.TableName + " order by descrizione ";
                DataTable dt = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
                kill();
                return dt;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TributiTable.TributiList.errore: ", ex);
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

        /// <summary>
        /// Ritorna tutti gli elementi della tabella filtrati per codice
        /// </summary>
        /// <param name="codiceTributo"></param>
        /// <returns> La DataTable valorizzata o null </returns>
        public DataTable TributiDescrizione(string codiceTributo)
        {
            DataTable Tabella;

            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandText = "Select * from " + this.TableName + " where COD_ENTE = @tributo ";
                SelectCommand.Parameters.Add("@tributo", SqlDbType.VarChar).Value = codiceTributo;
                DataTable dt = Query(SelectCommand, new SqlConnection(ConstWrapper.StringConnectionOPENgov));
                kill();
                return dt;
            }
            catch (Exception ex)
            {
                log.Debug(ConstWrapper.CodiceEnte + "." + ConstWrapper.sUsername + " - DichiarazioniICI.TributiTable.TributiDescrizione.errore: ", ex);
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
    }
}
