using System;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Utility;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione della vista viewContribuentiTutti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ContribuentiTuttiView : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ContribuentiTuttiView));
        /// <summary>
		/// Costruttore della classe
		/// </summary>
		public ContribuentiTuttiView()
		{
			this.TableName = "viewContribuentiTutti";
            this.ProcedureName = "prc_GetDichFromSoggetti";
		}

        /// <summary>
        /// Restituisce una DataTable con la lista dei contribuenti filtrati per cognome, nome,
        /// codice fiscale e/o partita Iva.
        /// </summary>
        /// <param name="ProvenienzaDich"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="cognome">Parametro di tipo <c>String</c> che identifica il Cognome del Contribuente</param>
        /// <param name="nome">Parametro di tipo <c>String</c> che identifica il Nome del Contribuente</param>
        /// <param name="codiceFiscale">Parametro di tipo <c>String</c> che identifica il Codice Fiscle del Contribuente</param>
        /// <param name="partitaIVA">Parametro di tipo <c>String</c> che identifica la Partita IVA del Contribuente</param>
        /// <param name="ente">Parametro di tipo <c>String</c> che identifica l'Ente</param>
        /// <param name="StatoDich">Parametro di tipo <c>int</c> che identifica se la dichiarazione è aperta o chiusa</param>
        /// <returns>estituisce una DataTable con la lista dei contribuenti</returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// Modifiche da revisione manuale
        /// </revision>
        /// </revisionHistory>
        public DataTable ListContribuenti(int ProvenienzaDich, string Dal, string Al, string cognome, string nome, string codiceFiscale, string partitaIVA, string ente, int StatoDich)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                    string sSQL = string.Empty;
                    using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                    {
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetDichFromSoggetti","ENTE", "AMBIENTE","COGNOME", "NOME", "CODICEFISCALE","PARTITAIVA", "DAL","AL", "STATO","PROVENIENZADICH");
                        DataSet myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("ENTE", ente)
                                , ctx.GetParam("AMBIENTE", Business.ConstWrapper.Ambiente)
                                , ctx.GetParam("COGNOME", cognome.Replace("*", "%") + "%")
                                , ctx.GetParam("NOME", nome.Replace("*", "%") + "%")
                                , ctx.GetParam("CODICEFISCALE", codiceFiscale + "%")
                                , ctx.GetParam("PARTITAIVA", partitaIVA + "%")
                                , ctx.GetParam("DAL", Utility.StringOperation.FormatDateTime(Dal))
                                , ctx.GetParam("AL", Utility.StringOperation.FormatDateTime(Al))
                                , ctx.GetParam("STATO", StatoDich)
                                , ctx.GetParam("PROVENIENZADICH", ProvenienzaDich)
                            );
                        myDataTable = myDataSet.Tables[0];
                        ctx.Dispose();
                    }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribuentiTuttiView.ListContribuenti.errore: ", ex);
                myDataTable = null;
            }
            return myDataTable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="sListContrib"></param>
        /// <param name="ProvenienzaDich"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="cognome"></param>
        /// <param name="nome"></param>
        /// <param name="codiceFiscale"></param>
        /// <param name="partitaIVA"></param>
        /// <param name="partitaCatastale"></param>
        /// <param name="foglio"></param>
        /// <param name="numero"></param>
        /// <param name="subalterno"></param>
        /// <param name="sezione"></param>
        /// <param name="caratteristica"></param>
        /// <param name="anno"></param>
        /// <param name="protocollo"></param>
        /// <param name="via"></param>
        /// <param name="categoria"></param>
        /// <param name="classe"></param>
        /// <param name="percPos"></param>
        /// <param name="ImportoP"></param>
        /// <param name="IdConfrontoImporto"></param>
        /// <param name="NoStradario"></param>
        /// <param name="AbiPrinc"></param>
        /// <param name="IsPertinenza"></param>
        /// <param name="TipoUtilizzo"></param>
        /// <param name="TipoPossesso"></param>
        /// <param name="Coltivatori"></param>
        /// <returns></returns>
        public DataTable ListContribuentiStampa( string ente,string sListContrib,int ProvenienzaDich, string Dal, string Al
            , string cognome, string nome, string codiceFiscale, string partitaIVA
            , string partitaCatastale, string foglio, string numero, string subalterno, string sezione, int caratteristica, string anno, string protocollo, string via, string categoria, string classe
            , double percPos, double ImportoP, int IdConfrontoImporto, int NoStradario, int AbiPrinc, int IsPertinenza, int TipoUtilizzo, int TipoPossesso, int Coltivatori)
        {
            DataTable myDataTable = new DataTable();

            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetStampaDich", "ENTE", "DAL", "AL", "COGNOME", "NOME", "CODICEFISCALE", "PARTITAIVA", "PROVENIENZADICH", "VIA", "FOGLIO", "NUMERO", "SUBALTERNO", "CARATTERISTICA", "CATEGORIA", "CLASSE", "ABIPRINC", "ISPERTINENZA", "TIPOUTILIZZO", "TIPOPOSSESSO", "PERCPOS", "IDCONFRONTOIMPORTO", "IMPORTOP", "COLTIVATORI", "IMMOBILINOAGG", "PARTITACATASTALE", "SEZIONE", "PROTOCOLLO", "ANNO", "LISTCONTRIB");
                    DataSet myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("ENTE", ente)
                            , ctx.GetParam("DAL", Utility.StringOperation.FormatDateTime(Dal))
                            , ctx.GetParam("AL", Utility.StringOperation.FormatDateTime(Al))
                            , ctx.GetParam("COGNOME", cognome.Replace("*", "%") + "%")
                            , ctx.GetParam("NOME", nome.Replace("*", "%") + "%")
                            , ctx.GetParam("CODICEFISCALE", codiceFiscale + "%")
                            , ctx.GetParam("PARTITAIVA", partitaIVA + "%")
                            , ctx.GetParam("PROVENIENZADICH", ProvenienzaDich)
                            , ctx.GetParam("VIA", via.Replace("*", "%"))
                            , ctx.GetParam("FOGLIO", foglio)
                            , ctx.GetParam("NUMERO", numero)
                            , ctx.GetParam("SUBALTERNO", subalterno)
                            , ctx.GetParam("CARATTERISTICA", caratteristica)
                            , ctx.GetParam("CATEGORIA", categoria)
                            , ctx.GetParam("CLASSE", classe)
                            , ctx.GetParam("ABIPRINC", AbiPrinc)
                            , ctx.GetParam("ISPERTINENZA", IsPertinenza)
                            , ctx.GetParam("TIPOUTILIZZO", TipoUtilizzo)
                            , ctx.GetParam("TIPOPOSSESSO", TipoPossesso)
                            , ctx.GetParam("PERCPOS", percPos)
                            , ctx.GetParam("IDCONFRONTOIMPORTO", IdConfrontoImporto)
                            , ctx.GetParam("IMPORTOP", ImportoP)
                            , ctx.GetParam("COLTIVATORI", Coltivatori)
                            , ctx.GetParam("IMMOBILINOAGG", NoStradario)
                            , ctx.GetParam("PARTITACATASTALE", partitaCatastale)
                            , ctx.GetParam("SEZIONE", sezione)
                            , ctx.GetParam("PROTOCOLLO", protocollo)
                            , ctx.GetParam("ANNO", anno)
                            , ctx.GetParam("LISTCONTRIB", sListContrib)
                        );
                    myDataTable = myDataSet.Tables[0];
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "." + Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribuentiTuttiView.ListContribuentiStampa.errore: ", ex);
                myDataTable = null;
            }
            return myDataTable;
        }
    }
}
