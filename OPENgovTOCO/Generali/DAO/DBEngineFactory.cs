using Microsoft.VisualBasic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Drawing;
using DAL;
using OPENgovTOCO;
using log4net;

namespace DAO
{
    /// <summary>
    /// Classe per la gestione DB
    /// </summary>
    public class DBEngineFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DBEngineFactory));
        public static DBEngine GetDBEngine()
        {
            string connectionString_;
            DALEnum.eDBProvider dbProvider_;
            DBEngine dbEngine_;

            Log.Debug("GetDBEngine::valorizzo stringa connessione::" + DichiarazioneSession.StringConnection);
            connectionString_ = DichiarazioneSession.StringConnection;
            dbProvider_ = DALEnum.eDBProvider.SqlClient;
            dbEngine_ = new DBEngine(connectionString_, dbProvider_);

            return dbEngine_;

        }

        public static DBEngine GetDBEngine(string StringConnection)
        {
            DALEnum.eDBProvider dbProvider_;
            DBEngine dbEngine_;

            dbProvider_ = DALEnum.eDBProvider.SqlClient;
            dbEngine_ = new DBEngine(StringConnection, dbProvider_);

            return dbEngine_;
        }

    }

}

