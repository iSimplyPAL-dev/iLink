using System;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace DichiarazioniICI.Database
{
    /*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI ***
	/// <summary>
	/// Struttura che rappresenta il singolo record della tabella TblDettaglioTestata.
	/// </summary>
	public struct DettaglioTestataRow
	{
		public int ID;
		public string Ente;
		public int IdTestata;
		public string NumeroOrdine;
		public string NumeroModello;
		public int IdOggetto;
		public int IdSoggetto;
        //*** 20140509 - TASI ***
        public int TipoUtilizzo;
        public int TipoPossesso;
        //*** ***
		public decimal PercPossesso;
		public int MesiPossesso;
		public int MesiEsclusioneEsenzione;
		public int MesiRiduzione;
		public decimal ImpDetrazAbitazPrincipale;
		public bool Contitolare;
		//public bool AbitazionePrincipale;
		public int AbitazionePrincipale;
		public bool Bonificato;
		public bool Annullato;
		public DateTime DataInizioValidità;
		public DateTime DataFineValidità;
		public string Operatore;
		public int Riduzione;
		public int Possesso;
		public int EsclusioneEsenzione;
		public int NumeroUtilizzatori;
		public int AbitazionePrincipaleAttuale;
		public bool ColtivatoreDiretto;
		public int NumeroFigli;
		//*** 20120629 - IMU ***
		public CaricoFigliRow[] ListCaricoFigli;
		//*** ***
		//		public bool Riduzione;
		//		public bool Possesso;
		//		public bool EsclusioneEsenzione;
	}
	
	//*** 20120629 - IMU ***
	public struct CaricoFigliRow
	{
		public int IdDettaglioTestata;
		public int nFiglio;
		public double Percentuale;
	}
	//*** ***
    */
    /// <summary>
    /// Classe di gestione della tabella TblDettaglioTestata.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class DettaglioTestataTable : Database
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DettaglioTestataTable));
		private string _username;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UserName"></param>
		public DettaglioTestataTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblDettaglioTestata";
            this.ProcedureName = "prc_TBLDETTAGLIOTESTATA_S";
		}
        /**** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI
        /// <summary>
        /// Inserisce un nuovo record a partire dai singoli campi.
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="idTestata"></param>
        /// <param name="numeroOrdine"></param>
        /// <param name="numeroModello"></param>
        /// <param name="idOggetto"></param>
        /// <param name="idSoggetto"></param>
        /// <param name="tipoPossesso"></param>
        /// <param name="percPossesso"></param>
        /// <param name="mesiPossesso"></param>
        /// <param name="mesiEsclusioneEsenzione"></param>
        /// <param name="mesiRiduzione"></param>
        /// <param name="impDetrazAbitazPrincipale"></param>
        /// <param name="contitolare"></param>
        /// <param name="abitazionePrincipale"></param>
        /// <param name="bonificato"></param>
        /// <param name="annullato"></param>
        /// <param name="dataInizioValidità"></param>
        /// <param name="dataFineValidità"></param>
        /// <param name="operatore"></param>
        /// <param name="riduzione"></param>
        /// <param name="possesso"></param>
        /// <param name="esclusioneEsenzione"></param>
        /// <param name="NumeroUtilizzatori"></param>
        /// <param name="AbitazionePrincipaleAttuale"></param>
        /// <param name="id"></param>
        /// <returns></returns>


        //*** 20140509 - TASI ***


        //public bool Insert(string ente, int idTestata,
        //    string numeroOrdine, string numeroModello, int idOggetto, int idSoggetto, int tipoPossesso, 
        //    decimal percPossesso, int mesiPossesso, int mesiEsclusioneEsenzione, int mesiRiduzione, 
        //    decimal impDetrazAbitazPrincipale, bool contitolare, int abitazionePrincipale, 
        //    bool bonificato, bool annullato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore,
        //    int riduzione, int possesso, int esclusioneEsenzione, int NumeroUtilizzatori, int AbitazionePrincipaleAttuale, 
        //    bool ColtivatoreDiretto, int NumeroFigli, out int id)
        //{
        //    string sLog;
        //    SqlCommand InsertCommand = new SqlCommand();
        //try{
        //    InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, IdTestata, " +
        //        "numeroOrdine, numeroModello, idOggetto,"+
        //        "IdSoggetto, TipoPossesso, PercPossesso, " +
        //        "MesiPossesso, MesiEsclusioneEsenzione, MesiRiduzione, ImpDetrazAbitazPrincipale, Contitolare, " +
        //        "AbitazionePrincipale, Bonificato, Annullato, DataInizioValidità, DataFineValidità, Operatore, " +
        //        "Riduzione, Possesso, EsclusioneEsenzione, NumeroUtilizzatori, AbitazionePrincipaleAttuale,ColtivatoreDiretto, NumeroFigli) " +
        //        "VALUES (@ente, @idTestata, @numeroOrdine, @numeroModello, " +
        //        "@idOggetto, @idSoggetto, @tipoPossesso, @percPossesso, @mesiPossesso, @mesiEsclusioneEsenzione, " +
        //        "@mesiRiduzione, @impDetrazAbitazPrincipale, @contitolare, @abitazionePrincipale, @bonificato, " +
        //        "@annullato, @dataInizioValidità, @dataFineValidità, @operatore, @riduzione, @possesso, @esclusioneEsenzione, @numeroUtilizzatori," +
        //        "@abitazionePrincipaleAttuale,@ColtivatoreDiretto, @NumeroFigli)";

        //    sLog=InsertCommand.CommandText;
        //    InsertCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
        //    sLog+=",'"+ ente.ToString();
        //    InsertCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
        //    sLog+=",'"+ idTestata.ToString();
        //    InsertCommand.Parameters.Add("@numeroOrdine",SqlDbType.VarChar).Value = numeroOrdine;
        //    sLog+=",'"+ numeroOrdine.ToString();
        //    InsertCommand.Parameters.Add("@numeroModello",SqlDbType.VarChar).Value = numeroModello;
        //    sLog+=",'"+ numeroModello.ToString();
        //    InsertCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;
        //    sLog+=",'"+ idOggetto.ToString();
        //    InsertCommand.Parameters.Add("@idSoggetto",SqlDbType.Int).Value = idSoggetto;
        //    sLog+=",'"+ idSoggetto.ToString();
        //    InsertCommand.Parameters.Add("@tipoPossesso",SqlDbType.Int).Value = tipoPossesso == 0 ? DBNull.Value : (object)tipoPossesso;
        //    sLog+=",'"+ tipoPossesso.ToString();
        //    InsertCommand.Parameters.Add("@percPossesso",SqlDbType.Real).Value = percPossesso;
        //    sLog+=",'"+ percPossesso.ToString();
        //    InsertCommand.Parameters.Add("@mesiPossesso",SqlDbType.Int).Value = mesiPossesso;
        //    sLog+=",'"+ mesiPossesso.ToString();
        //    InsertCommand.Parameters.Add("@mesiEsclusioneEsenzione",SqlDbType.Int).Value = mesiEsclusioneEsenzione;
        //    sLog+=",'"+ mesiEsclusioneEsenzione.ToString();
        //    InsertCommand.Parameters.Add("@mesiRiduzione",SqlDbType.Int).Value = mesiRiduzione;
        //    sLog+=",'"+ mesiRiduzione.ToString();
        //    InsertCommand.Parameters.Add("@impDetrazAbitazPrincipale",SqlDbType.Decimal).Value = impDetrazAbitazPrincipale;
        //    sLog+=",'"+ impDetrazAbitazPrincipale.ToString();
        //    InsertCommand.Parameters.Add("@contitolare",SqlDbType.Bit).Value = contitolare;
        //    sLog+=",'"+ contitolare.ToString();
        //    InsertCommand.Parameters.Add("@abitazionePrincipale",SqlDbType.Int).Value = abitazionePrincipale;
        //    sLog+=",'"+ abitazionePrincipale.ToString();
        ////	InsertCommand.Parameters.Add("@abitazionePrincipale",SqlDbType.Bit).Value = abitazionePrincipale;
        //    InsertCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
        //    sLog+=",'"+ bonificato.ToString();
        //    InsertCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
        //    sLog+=",'"+ annullato.ToString();
        //    InsertCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
        //    sLog+=",'"+ dataInizioValidità.ToString();
        //    InsertCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
        //    sLog+=",'"+ dataFineValidità.ToString();
        //    InsertCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
        //    sLog+=",'"+ operatore.ToString();
        //    InsertCommand.Parameters.Add("@riduzione",SqlDbType.Int).Value = riduzione;
        //    sLog+=",'"+ riduzione.ToString();
        //    InsertCommand.Parameters.Add("@possesso",SqlDbType.Int).Value = possesso;
        //    sLog+=",'"+ possesso.ToString();
        //    InsertCommand.Parameters.Add("@esclusioneEsenzione",SqlDbType.Int).Value = esclusioneEsenzione;
        //    sLog+=",'"+ esclusioneEsenzione.ToString();
        //    InsertCommand.Parameters.Add("@abitazionePrincipaleAttuale", SqlDbType.Int).Value = AbitazionePrincipaleAttuale;
        //    sLog+=",'"+ AbitazionePrincipaleAttuale.ToString();
        //    InsertCommand.Parameters.Add("@numeroUtilizzatori", SqlDbType.Int).Value = NumeroUtilizzatori;
        //    sLog+=",'"+ NumeroUtilizzatori.ToString();
        //    InsertCommand.Parameters.Add("@ColtivatoreDiretto", SqlDbType.Bit).Value = ColtivatoreDiretto;
        //    sLog+=",'"+ ColtivatoreDiretto.ToString();
        //    InsertCommand.Parameters.Add("@NumeroFigli", SqlDbType.Int).Value = NumeroFigli;
        //    sLog+=",'"+ NumeroFigli.ToString();
			
        ////	InsertCommand.Parameters.Add("@riduzione",SqlDbType.Bit).Value = riduzione;
        ////	InsertCommand.Parameters.Add("@possesso",SqlDbType.Bit).Value = possesso;
        ////	InsertCommand.Parameters.Add("@esclusioneEsenzione",SqlDbType.Bit).Value = esclusioneEsenzione;
        //    log.Debug("DettaglioTestataTable::Insert::SQL::" + sLog);
        //    return Execute(InsertCommand, out id);
                 }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.Insert.errore: ", Err);
            }

        //}

        public bool SetDettaglioTestata(DettaglioTestataRow myItem, out int id)
        {
        try{
            string sLog;
            SqlCommand InsertCommand = new SqlCommand();
            InsertCommand.CommandType = CommandType.StoredProcedure;
            InsertCommand.CommandText = "prc_TBLDETTAGLIOTESTATA_IU";
            sLog = InsertCommand.CommandText;
            InsertCommand.Parameters.Add("@id", SqlDbType.Int).Value = myItem.ID;
            sLog += "," + myItem.ID.ToString();
            InsertCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = myItem.Ente;
            sLog += ",'" + myItem.Ente.ToString();
            InsertCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = myItem.IdTestata;
            sLog += ",'" + myItem.IdTestata.ToString();
            InsertCommand.Parameters.Add("@numeroOrdine", SqlDbType.VarChar).Value = myItem.NumeroOrdine;
            sLog += ",'" + myItem.NumeroOrdine.ToString();
            InsertCommand.Parameters.Add("@numeroModello", SqlDbType.VarChar).Value = myItem.NumeroModello;
            sLog += ",'" + myItem.NumeroModello.ToString();
            InsertCommand.Parameters.Add("@idOggetto", SqlDbType.Int).Value = myItem.IdOggetto;
            sLog += ",'" + myItem.IdOggetto.ToString();
            InsertCommand.Parameters.Add("@idSoggetto", SqlDbType.Int).Value = myItem.IdSoggetto;
            sLog += ",'" + myItem.IdSoggetto.ToString();
            //*** 20140509 - TASI ***
            InsertCommand.Parameters.Add("@IdTipoUtilizzo", SqlDbType.Int).Value = myItem.TipoUtilizzo == 0 ? DBNull.Value : (object)myItem.TipoUtilizzo;
            sLog += ",'" + myItem.TipoUtilizzo.ToString();
            InsertCommand.Parameters.Add("@IdTipoPossesso", SqlDbType.Int).Value = myItem.TipoPossesso == 0 ? DBNull.Value : (object)myItem.TipoPossesso;
            sLog += ",'" + myItem.TipoPossesso.ToString();
            //*** ***
            InsertCommand.Parameters.Add("@percPossesso", SqlDbType.Real).Value = myItem.PercPossesso;
            sLog += ",'" + myItem.PercPossesso.ToString();
            InsertCommand.Parameters.Add("@mesiPossesso", SqlDbType.Int).Value = myItem.MesiPossesso;
            sLog += ",'" + myItem.MesiPossesso.ToString();
            InsertCommand.Parameters.Add("@mesiEsclusioneEsenzione", SqlDbType.Int).Value = myItem.MesiEsclusioneEsenzione;
            sLog += ",'" + myItem.MesiEsclusioneEsenzione.ToString();
            InsertCommand.Parameters.Add("@mesiRiduzione", SqlDbType.Int).Value = myItem.MesiRiduzione;
            sLog += ",'" + myItem.MesiRiduzione.ToString();
            InsertCommand.Parameters.Add("@impDetrazAbitazPrincipale", SqlDbType.Decimal).Value = myItem.ImpDetrazAbitazPrincipale;
            sLog += ",'" + myItem.ImpDetrazAbitazPrincipale.ToString();
            InsertCommand.Parameters.Add("@contitolare", SqlDbType.Bit).Value = myItem.Contitolare;
            sLog += ",'" + myItem.Contitolare.ToString();
            InsertCommand.Parameters.Add("@abitazionePrincipale", SqlDbType.Int).Value = myItem.AbitazionePrincipale;
            sLog += ",'" + myItem.AbitazionePrincipale.ToString();
            //	InsertCommand.Parameters.Add("@abitazionePrincipale",SqlDbType.Bit).Value = abitazionePrincipale;
            InsertCommand.Parameters.Add("@bonificato", SqlDbType.Bit).Value = myItem.Bonificato;
            sLog += ",'" + myItem.Bonificato.ToString();
            InsertCommand.Parameters.Add("@annullato", SqlDbType.Bit).Value = myItem.Annullato;
            sLog += ",'" + myItem.Annullato.ToString();
            InsertCommand.Parameters.Add("@dataInizioValidità", SqlDbType.DateTime).Value = myItem.DataInizioValidità;
            sLog += ",'" + myItem.DataInizioValidità.ToString();
            InsertCommand.Parameters.Add("@dataFineValidità", SqlDbType.DateTime).Value = myItem.DataFineValidità == DateTime.MinValue ? DBNull.Value : (object)myItem.DataFineValidità;
            sLog += ",'" + myItem.DataFineValidità.ToString();
            InsertCommand.Parameters.Add("@operatore", SqlDbType.VarChar).Value = myItem.Operatore;
            sLog += ",'" + myItem.Operatore.ToString();
            InsertCommand.Parameters.Add("@riduzione", SqlDbType.Int).Value = myItem.Riduzione;
            sLog += ",'" + myItem.Riduzione.ToString();
            InsertCommand.Parameters.Add("@possesso", SqlDbType.Int).Value = myItem.Possesso;
            sLog += ",'" + myItem.Possesso.ToString();
            InsertCommand.Parameters.Add("@esclusioneEsenzione", SqlDbType.Int).Value = myItem.EsclusioneEsenzione;
            sLog += ",'" + myItem.EsclusioneEsenzione.ToString();
            InsertCommand.Parameters.Add("@abitazionePrincipaleAttuale", SqlDbType.Int).Value = myItem.AbitazionePrincipaleAttuale;
            sLog += ",'" + myItem.AbitazionePrincipaleAttuale.ToString();
            InsertCommand.Parameters.Add("@numeroUtilizzatori", SqlDbType.Int).Value = myItem.NumeroUtilizzatori;
            sLog += ",'" + myItem.NumeroUtilizzatori.ToString();
            InsertCommand.Parameters.Add("@ColtivatoreDiretto", SqlDbType.Bit).Value = myItem.ColtivatoreDiretto;
            sLog += ",'" + myItem.ColtivatoreDiretto.ToString();
            InsertCommand.Parameters.Add("@NumeroFigli", SqlDbType.Int).Value = myItem.NumeroFigli;
            sLog += ",'" + myItem.NumeroFigli.ToString();
            log.Debug("DettaglioTestataTable::Insert::SQL::" + sLog);
            return Execute(InsertCommand, new SqlConnection(Business.ConstWrapper.StringConnection), "@ID", out id);
                   }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.SetDettaglioTestata.errore: ", Err);
            }
        }
        //*** ***

        /// <summary>
        /// Inserisce un nuovo record a partire da una struttura row.
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Insert(DettaglioTestataRow Item, out int id)
        {
            bool retVal= false;

            //*** 20140509 - TASI ***
            //retVal = Insert(Item.Ente, Item.IdTestata, 
            //    Item.NumeroOrdine, Item.NumeroModello, Item.IdOggetto, Item.IdSoggetto, Item.TipoUtilizzo,
            //    Item.PercPossesso, Item.MesiPossesso, Item.MesiEsclusioneEsenzione, Item.MesiRiduzione,
            //    Item.ImpDetrazAbitazPrincipale, Item.Contitolare, Item.AbitazionePrincipale, Item.Bonificato,
            //    Item.Annullato, Item.DataInizioValidità, Item.DataFineValidità, Item.Operatore,
            //    Item.Riduzione, Item.Possesso, Item.EsclusioneEsenzione, Item.NumeroUtilizzatori, Item.AbitazionePrincipaleAttuale,
            //    Item.ColtivatoreDiretto,Item.NumeroFigli,out id);
            retVal = SetDettaglioTestata(Item, out id);
            //*** ***
            try{
            if (retVal==true)
            {
                //*** 20120629 - IMU ***
                if(Item.ListCaricoFigli!=null)
                {
                    //inserisco le % correnti
                    retVal=SetPercentualeCaricoFigli(0,Item.ListCaricoFigli, id);
                }
            }
            //*** ***
            return retVal;
                   }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.Insert.errore: ", Err);
            }
        }
		
        /// <summary>
        /// Aggiorna un record individuato dall'identity a partire dai singoli campi.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ente"></param>
        /// <param name="idTestata"></param>
        /// <param name="numeroOrdine"></param>
        /// <param name="numeroModello"></param>
        /// <param name="idOggetto"></param>
        /// <param name="idSoggetto"></param>
        /// <param name="tipoPossesso"></param>
        /// <param name="percPossesso"></param>
        /// <param name="mesiPossesso"></param>
        /// <param name="mesiEsclusioneEsenzione"></param>
        /// <param name="mesiRiduzione"></param>
        /// <param name="impDetrazAbitazPrincipale"></param>
        /// <param name="contitolare"></param>
        /// <param name="abitazionePrincipale"></param>
        /// <param name="bonificato"></param>
        /// <param name="annullato"></param>
        /// <param name="dataInizioValidità"></param>
        /// <param name="dataFineValidità"></param>
        /// <param name="operatore"></param>
        /// <param name="riduzione"></param>
        /// <param name="possesso"></param>
        /// <param name="esclusioneEsenzione"></param>
        /// <param name="NumeroUtilizzatori"></param>
        /// <param name="AbitazionePrincipaleAttuale"></param>
        /// <returns></returns>
        //*** 20140509 - TASI ***
//        public bool Modify(int id, string ente, int idTestata,
//            string numeroOrdine, string numeroModello, int idOggetto, int idSoggetto, int tipoPossesso, 
//            decimal percPossesso, int mesiPossesso, int mesiEsclusioneEsenzione, int mesiRiduzione, 
//            decimal impDetrazAbitazPrincipale, bool contitolare, int abitazionePrincipale, 
//            bool bonificato, bool annullato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore,
//            int riduzione, int possesso, int esclusioneEsenzione, int NumeroUtilizzatori, int AbitazionePrincipaleAttuale,
//            bool ColtivatoreDiretto,  int NumeroFigli)
//        {
//            SqlCommand ModifyCommand = new SqlCommand();
                try{
//            ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, IdTestata=@idTestata, " +
//                "NumeroOrdine=@numeroOrdine, NumeroModello=@numeroModello, IdOggetto=@idOggetto, " +
//                "IdSoggetto=@idSoggetto, TipoPossesso=@tipoPossesso, PercPossesso=@percPossesso, " +
//                "MesiPossesso=@mesiPossesso, MesiEsclusioneEsenzione=@mesiEsclusioneEsenzione, " +
//                "MesiRiduzione=@mesiRiduzione, ImpDetrazAbitazPrincipale=@impDetrazAbitazPrincipale, " +
//                "Contitolare=@contitolare, AbitazionePrincipale=@abitazionePrincipale, Bonificato=@bonificato, " +
//                "Annullato=@annullato, DataInizioValidità=@dataInizioValidità, DataFineValidità=@dataFineValidità, " +
//                "Operatore=@operatore, Riduzione=@riduzione, Possesso=@possesso, EsclusioneEsenzione=@esclusioneEsenzione, NumeroUtilizzatori=@numeroUtilizzatori," + 
//                "AbitazionePrincipaleAttuale=@abitazionePrincipaleAttuale, " + 
//                "ColtivatoreDiretto=@ColtivatoreDiretto, NumeroFigli=@NumeroFigli  WHERE ID=@id";

//            ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
//            ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
//            ModifyCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
//            ModifyCommand.Parameters.Add("@numeroOrdine",SqlDbType.VarChar).Value = numeroOrdine;
//            ModifyCommand.Parameters.Add("@numeroModello",SqlDbType.VarChar).Value = numeroModello;
//            ModifyCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;
//            ModifyCommand.Parameters.Add("@idSoggetto",SqlDbType.Int).Value = idSoggetto;
//            ModifyCommand.Parameters.Add("@tipoPossesso",SqlDbType.Int).Value = tipoPossesso == 0 ? DBNull.Value : (object)tipoPossesso;
//            ModifyCommand.Parameters.Add("@percPossesso",SqlDbType.Real).Value = percPossesso;
//            ModifyCommand.Parameters.Add("@mesiPossesso",SqlDbType.Int).Value = mesiPossesso;
//            ModifyCommand.Parameters.Add("@mesiEsclusioneEsenzione",SqlDbType.Int).Value = mesiEsclusioneEsenzione;
//            ModifyCommand.Parameters.Add("@mesiRiduzione",SqlDbType.Int).Value = mesiRiduzione;
//            ModifyCommand.Parameters.Add("@impDetrazAbitazPrincipale",SqlDbType.Decimal).Value = impDetrazAbitazPrincipale;
//            ModifyCommand.Parameters.Add("@contitolare",SqlDbType.Bit).Value = contitolare;
//            //ModifyCommand.Parameters.Add("@abitazionePrincipale",SqlDbType.Bit).Value = abitazionePrincipale;
//            ModifyCommand.Parameters.Add("@abitazionePrincipale",SqlDbType.Int).Value = abitazionePrincipale;
//            ModifyCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
//            ModifyCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
//            ModifyCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
//            ModifyCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
//            ModifyCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
//            ModifyCommand.Parameters.Add("@riduzione",SqlDbType.Int).Value = riduzione;
//            ModifyCommand.Parameters.Add("@possesso",SqlDbType.Int).Value = possesso;
//            ModifyCommand.Parameters.Add("@esclusioneEsenzione",SqlDbType.Int).Value = esclusioneEsenzione;
//            ModifyCommand.Parameters.Add("@abitazionePrincipaleAttuale", SqlDbType.Int).Value = AbitazionePrincipaleAttuale;
//            ModifyCommand.Parameters.Add("@numeroUtilizzatori", SqlDbType.Int).Value = NumeroUtilizzatori;
//            ModifyCommand.Parameters.Add("@ColtivatoreDiretto",SqlDbType.Bit).Value = ColtivatoreDiretto;
//            ModifyCommand.Parameters.Add("@NumeroFigli", SqlDbType.Int).Value = NumeroFigli;
////			ModifyCommand.Parameters.Add("@riduzione",SqlDbType.Bit).Value = riduzione;
////			ModifyCommand.Parameters.Add("@possesso",SqlDbType.Bit).Value = possesso;
////			ModifyCommand.Parameters.Add("@esclusioneEsenzione",SqlDbType.Bit).Value = esclusioneEsenzione;
			
//            return Execute(ModifyCommand);
       }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.Modify.errore: ", Err);
            }
//        }
        //*** ***

        /// <summary>
        /// Aggiorna un record individuato dall'identity a partire da una struttura row.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Modify(DettaglioTestataRow Item)
        {
            bool retVal=false;

            //*** 20140509 - TASI ***
            //retVal = Modify(Item.ID, Item.Ente, Item.IdTestata,
            //    Item.NumeroOrdine, Item.NumeroModello, Item.IdOggetto, Item.IdSoggetto, Item.TipoUtilizzo, Item.TipoPossesso,
            //    Item.PercPossesso, Item.MesiPossesso, Item.MesiEsclusioneEsenzione, Item.MesiRiduzione,
            //    Item.ImpDetrazAbitazPrincipale, Item.Contitolare, Item.AbitazionePrincipale, Item.Bonificato,
            //    Item.Annullato, Item.DataInizioValidità, Item.DataFineValidità, Item.Operatore,
            //    Item.Riduzione, Item.Possesso, Item.EsclusioneEsenzione, Item.NumeroUtilizzatori, Item.AbitazionePrincipaleAttuale, 
            //    Item.ColtivatoreDiretto, Item.NumeroFigli);
            retVal = SetDettaglioTestata(Item, out Item.ID);
            //*** ***
            try{
            if (retVal==true)
            {
                //elimino le % vecchie
                retVal=SetPercentualeCaricoFigli(2,null,Item.ID);
                if (retVal==true)
                {
                    //*** 20120629 - IMU ***
                    if(Item.ListCaricoFigli!=null)
                    {
                        //inserisco le % correnti
                        retVal=SetPercentualeCaricoFigli(0,Item.ListCaricoFigli, Item.ID);
                    }
                }
            }
            //*** ***
            return retVal;
                   }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.Modify.errore: ", Err);
            }
        }
        */
        /// <summary>
        /// Ritorna una struttura row che rappresenta un record individuato dall'identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public Utility.DichManagerICI.DettaglioTestataRow GetRow(int id)
		{
            Utility.DichManagerICI.DettaglioTestataRow Details = new Utility.DichManagerICI.DettaglioTestataRow();
			try
			{
				SqlCommand SelectCommand = PrepareGetRow(id);
                DataTable Dettagli = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (Dettagli.Rows.Count > 0)
				{
					Details.ID = (int)Dettagli.Rows[0]["ID"];
					Details.Ente = (string)Dettagli.Rows[0]["Ente"];
					Details.IdTestata = (int)Dettagli.Rows[0]["IdTestata"];
					Details.NumeroOrdine = (string)Dettagli.Rows[0]["NumeroOrdine"];
					Details.NumeroModello = (string)Dettagli.Rows[0]["NumeroModello"];
					Details.IdOggetto = (int)Dettagli.Rows[0]["IdOggetto"];
					Details.IdSoggetto = (int)Dettagli.Rows[0]["IdSoggetto"];
                    //*** 20140509 - TASI ***
                    Details.TipoUtilizzo = Dettagli.Rows[0]["IdTipoUtilizzo"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoUtilizzo"];
                    Details.TipoPossesso = Dettagli.Rows[0]["IdTipoPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoPossesso"];
                    //*** ***
					Details.PercPossesso = Dettagli.Rows[0]["PercPossesso"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["PercPossesso"]);
					Details.MesiPossesso = Dettagli.Rows[0]["MesiPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiPossesso"];
					Details.MesiEsclusioneEsenzione = Dettagli.Rows[0]["MesiEsclusioneEsenzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiEsclusioneEsenzione"];
					Details.MesiRiduzione = Dettagli.Rows[0]["MesiRiduzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiRiduzione"];
					Details.ImpDetrazAbitazPrincipale = Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"]);
					Details.Contitolare = Dettagli.Rows[0]["Contitolare"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Contitolare"];
					Details.AbitazionePrincipale = (int)Dettagli.Rows[0]["AbitazionePrincipale"];
					Details.Bonificato = Dettagli.Rows[0]["Bonificato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Bonificato"];
					Details.Annullato = Dettagli.Rows[0]["Annullato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Annullato"];
					Details.DataInizioValidità = (DateTime)Dettagli.Rows[0]["DataInizioValidità"];
					Details.DataFineValidità = Dettagli.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : (DateTime)Dettagli.Rows[0]["DataFineValidità"];
                    Details.DataUltimaModifica = Dettagli.Rows[0]["DataUltimaModifica"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Dettagli.Rows[0]["DataUltimaModifica"];
                    Details.Operatore = (string)Dettagli.Rows[0]["Operatore"];
					Details.Riduzione = (int)Dettagli.Rows[0]["Riduzione"];
					Details.Possesso = (int)Dettagli.Rows[0]["Possesso"];
					Details.EsclusioneEsenzione = (int)Dettagli.Rows[0]["EsclusioneEsenzione"];
					Details.AbitazionePrincipaleAttuale = Dettagli.Rows[0]["AbitazionePrincipaleAttuale"] == DBNull.Value ? -1 : (int)Dettagli.Rows[0]["AbitazionePrincipaleAttuale"];
					Details.NumeroUtilizzatori = Dettagli.Rows[0]["NumeroUtilizzatori"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["NumeroUtilizzatori"];
					//*** 20120629 - IMU ***
					Details.ColtivatoreDiretto = Dettagli.Rows[0]["coltivatorediretto"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["coltivatorediretto"];
					Details.NumeroFigli= Dettagli.Rows[0]["numerofigli"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["numerofigli"];
                    SqlCommand MyCmd = PrepareGetRowCaricoFigli(Details.ID, Business.ConstWrapper.StringConnection);
                    DataTable dtCaricoFigli = Query(MyCmd, new SqlConnection(Business.ConstWrapper.StringConnection));
					if (dtCaricoFigli.Rows.Count>0)
					{
                        int x = 0;
                        Utility.DichManagerICI.CaricoFigliRow[] ListCarico = new Utility.DichManagerICI.CaricoFigliRow[dtCaricoFigli.Rows.Count];
                        foreach (DataRow myRow in dtCaricoFigli.Rows)
                        {
                            Utility.DichManagerICI.CaricoFigliRow CaricoFigli = new Utility.DichManagerICI.CaricoFigliRow();
							CaricoFigli.IdDettaglioTestata = (int)myRow["IDDETTAGLIOTESTATA"];
							CaricoFigli.nFiglio = (int)myRow["NFIGLIO"];
							CaricoFigli.Percentuale = (int)myRow["PERCENTUALE"];
							ListCarico[x]=CaricoFigli;
                            x++;
						}
						Details.ListCaricoFigli=ListCarico;
					}
					//*** ***
				}
			}
			catch(Exception Err)
			{
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.GetRow.errore: ", Err);
                kill();
                Details = new Utility.DichManagerICI.DettaglioTestataRow();
			}
			finally{
				kill();
			}
			return Details;
		}
        //public Utility.DichManagerICI.DettaglioTestataRow GetRow(int id)
        //{
        //    Utility.DichManagerICI.DettaglioTestataRow Details = new Utility.DichManagerICI.DettaglioTestataRow();
        //    try
        //    {
        //        SqlCommand SelectCommand = PrepareGetRow(id);
        //        DataTable Dettagli = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //        if (Dettagli.Rows.Count > 0)
        //        {
        //            Details.ID = (int)Dettagli.Rows[0]["ID"];
        //            Details.Ente = (string)Dettagli.Rows[0]["Ente"];
        //            Details.IdTestata = (int)Dettagli.Rows[0]["IdTestata"];
        //            Details.NumeroOrdine = (string)Dettagli.Rows[0]["NumeroOrdine"];
        //            Details.NumeroModello = (string)Dettagli.Rows[0]["NumeroModello"];
        //            Details.IdOggetto = (int)Dettagli.Rows[0]["IdOggetto"];
        //            Details.IdSoggetto = (int)Dettagli.Rows[0]["IdSoggetto"];
        //            //*** 20140509 - TASI ***
        //            Details.TipoUtilizzo = Dettagli.Rows[0]["IdTipoUtilizzo"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoUtilizzo"];
        //            Details.TipoPossesso = Dettagli.Rows[0]["IdTipoPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoPossesso"];
        //            //*** ***
        //            Details.PercPossesso = Dettagli.Rows[0]["PercPossesso"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["PercPossesso"]);
        //            Details.MesiPossesso = Dettagli.Rows[0]["MesiPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiPossesso"];
        //            Details.MesiEsclusioneEsenzione = Dettagli.Rows[0]["MesiEsclusioneEsenzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiEsclusioneEsenzione"];
        //            Details.MesiRiduzione = Dettagli.Rows[0]["MesiRiduzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiRiduzione"];
        //            Details.ImpDetrazAbitazPrincipale = Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"]);
        //            Details.Contitolare = Dettagli.Rows[0]["Contitolare"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Contitolare"];
        //            Details.AbitazionePrincipale = (int)Dettagli.Rows[0]["AbitazionePrincipale"];
        //            Details.Bonificato = Dettagli.Rows[0]["Bonificato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Bonificato"];
        //            Details.Annullato = Dettagli.Rows[0]["Annullato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Annullato"];
        //            Details.DataInizioValidità = (DateTime)Dettagli.Rows[0]["DataInizioValidità"];
        //            Details.DataFineValidità = Dettagli.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : (DateTime)Dettagli.Rows[0]["DataFineValidità"];
        //            Details.Operatore = (string)Dettagli.Rows[0]["Operatore"];
        //            Details.Riduzione = (int)Dettagli.Rows[0]["Riduzione"];
        //            Details.Possesso = (int)Dettagli.Rows[0]["Possesso"];
        //            Details.EsclusioneEsenzione = (int)Dettagli.Rows[0]["EsclusioneEsenzione"];
        //            Details.AbitazionePrincipaleAttuale = Dettagli.Rows[0]["AbitazionePrincipaleAttuale"] == DBNull.Value ? -1 : (int)Dettagli.Rows[0]["AbitazionePrincipaleAttuale"];
        //            Details.NumeroUtilizzatori = Dettagli.Rows[0]["NumeroUtilizzatori"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["NumeroUtilizzatori"];
        //            //*** 20120629 - IMU ***
        //            Details.ColtivatoreDiretto = Dettagli.Rows[0]["coltivatorediretto"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["coltivatorediretto"];
        //            Details.NumeroFigli = Dettagli.Rows[0]["numerofigli"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["numerofigli"];
        //            SqlCommand MyCmd = PrepareGetRowCaricoFigli(Details.ID, Business.ConstWrapper.StringConnection);
        //            DataTable dtCaricoFigli = Query(MyCmd, new SqlConnection(Business.ConstWrapper.StringConnection));
        //            if (dtCaricoFigli.Rows.Count > 0)
        //            {
        //                int x = 0;
        //                Utility.DichManagerICI.CaricoFigliRow[] ListCarico = new Utility.DichManagerICI.CaricoFigliRow[dtCaricoFigli.Rows.Count];
        //                foreach (DataRow myRow in dtCaricoFigli.Rows)
        //                {
        //                    Utility.DichManagerICI.CaricoFigliRow CaricoFigli = new Utility.DichManagerICI.CaricoFigliRow();
        //                    CaricoFigli.IdDettaglioTestata = (int)myRow["IDDETTAGLIOTESTATA"];
        //                    CaricoFigli.nFiglio = (int)myRow["NFIGLIO"];
        //                    CaricoFigli.Percentuale = (int)myRow["PERCENTUALE"];
        //                    ListCarico[x] = CaricoFigli;
        //                    x++;
        //                }
        //                Details.ListCaricoFigli = ListCarico;
        //            }
        //            //*** ***
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.GetRow.errore: ", Err);
        //        kill();
        //        Details = new Utility.DichManagerICI.DettaglioTestataRow();
        //    }
        //    finally
        //    {
        //        kill();
        //    }
        //    return Details;
        //}

        //*** 20140509 - TASI ***
        /// <summary>
        /// Prende il dettaglio a partire dall'id dell'immobile e dall'id della testata.
        /// </summary>
        /// <param name="idImmobile"></param>
        /// <param name="idTestata"></param>
        /// <param name="contitolare"></param>
        /// <param name="myConn"></param>
        /// <returns></returns>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public Utility.DichManagerICI.DettaglioTestataRow GetRow(int idImmobile, int idTestata, bool contitolare, string myConn)
        {
            Utility.DichManagerICI.DettaglioTestataRow Details = new Utility.DichManagerICI.DettaglioTestataRow();
            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = CommandType.StoredProcedure;
                SelectCommand.CommandText = this.ProcedureName;
                SelectCommand.Parameters.Add("@IdOggetto", SqlDbType.Int).Value = idImmobile;
                SelectCommand.Parameters.Add("@IdTestata", SqlDbType.Int).Value = idTestata;
                SelectCommand.Parameters.Add("@Contitolare", SqlDbType.Bit).Value = contitolare.ToString();
                DataTable Dettagli = Query(SelectCommand, new SqlConnection(myConn));
                if (Dettagli.Rows.Count > 0)
                {
                    Details.ID = (int)Dettagli.Rows[0]["ID"];
                    Details.Ente = (string)Dettagli.Rows[0]["Ente"];
                    Details.IdTestata = (int)Dettagli.Rows[0]["IdTestata"];
                    Details.NumeroOrdine = (string)Dettagli.Rows[0]["NumeroOrdine"];
                    Details.NumeroModello = (string)Dettagli.Rows[0]["NumeroModello"];
                    Details.IdOggetto = (int)Dettagli.Rows[0]["IdOggetto"];
                    Details.IdSoggetto = (int)Dettagli.Rows[0]["IdSoggetto"];
                    Details.TipoUtilizzo = Dettagli.Rows[0]["IdTipoUtilizzo"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoUtilizzo"];
                    Details.TipoPossesso = Dettagli.Rows[0]["IdTipoPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoPossesso"];
                    Details.PercPossesso = Dettagli.Rows[0]["PercPossesso"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["PercPossesso"]);
                    Details.MesiPossesso = Dettagli.Rows[0]["MesiPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiPossesso"];
                    Details.MesiEsclusioneEsenzione = Dettagli.Rows[0]["MesiEsclusioneEsenzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiEsclusioneEsenzione"];
                    Details.MesiRiduzione = Dettagli.Rows[0]["MesiRiduzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiRiduzione"];
                    Details.ImpDetrazAbitazPrincipale = Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"]);
                    Details.Contitolare = Dettagli.Rows[0]["Contitolare"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Contitolare"];
                    Details.AbitazionePrincipale = (int)Dettagli.Rows[0]["AbitazionePrincipale"];
                    Details.Bonificato = Dettagli.Rows[0]["Bonificato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Bonificato"];
                    Details.Annullato = Dettagli.Rows[0]["Annullato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Annullato"];
                    Details.DataInizioValidità = (DateTime)Dettagli.Rows[0]["DataInizioValidità"];
                    Details.DataFineValidità = Dettagli.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : (DateTime)Dettagli.Rows[0]["DataFineValidità"];
                    Details.DataUltimaModifica = Dettagli.Rows[0]["DataUltimaModifica"] == DBNull.Value ? DateTime.MaxValue : (DateTime)Dettagli.Rows[0]["DataUltimaModifica"];
                    Details.Operatore = (string)Dettagli.Rows[0]["Operatore"];
                    Details.Riduzione = (int)Dettagli.Rows[0]["Riduzione"];
                    Details.Possesso = (int)Dettagli.Rows[0]["Possesso"];
                    Details.EsclusioneEsenzione = (int)Dettagli.Rows[0]["EsclusioneEsenzione"];
                    Details.AbitazionePrincipaleAttuale = (int)Dettagli.Rows[0]["AbitazionePrincipaleAttuale"];
                    Details.NumeroUtilizzatori = Dettagli.Rows[0]["NumeroUtilizzatori"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["NumeroUtilizzatori"];
                    //*** 20120629 - IMU ***
                    Details.ColtivatoreDiretto = Dettagli.Rows[0]["coltivatorediretto"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["coltivatorediretto"];
                    Details.NumeroFigli = Dettagli.Rows[0]["numerofigli"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["numerofigli"];
                    SqlCommand MyCmd = PrepareGetRowCaricoFigli(Details.ID,myConn);
                    DataTable dtCaricoFigli = Query(MyCmd, new SqlConnection(Business.ConstWrapper.StringConnection));
                    if (dtCaricoFigli.Rows.Count > 0)
                    {
                        int x = 0;
                        Utility.DichManagerICI.CaricoFigliRow[] ListCarico = new Utility.DichManagerICI.CaricoFigliRow[dtCaricoFigli.Rows.Count];
                        foreach (DataRow myRow in dtCaricoFigli.Rows)
                        {
                            Utility.DichManagerICI.CaricoFigliRow CaricoFigli = new Utility.DichManagerICI.CaricoFigliRow();
                            CaricoFigli.IdDettaglioTestata = (int)myRow["IDDETTAGLIOTESTATA"];
                            CaricoFigli.nFiglio = (int)myRow["NFIGLIO"];
                            CaricoFigli.Percentuale = Double.Parse(myRow["PERCENTUALE"].ToString());
                            ListCarico[x] = CaricoFigli;
                            x++;
                        }
                        Details.ListCaricoFigli = ListCarico;
                    }
                    //*** ***
                }
            }
            catch(Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.GetRow.errore: ", Err);
                kill();
                Details = new Utility.DichManagerICI.DettaglioTestataRow();
            }
            finally
            {
                kill();
            }
            return Details;
        }
        //public Utility.DichManagerICI.DettaglioTestataRow GetRow(int idImmobile, int idTestata, bool contitolare, string myConn)
        //{
        //    Utility.DichManagerICI.DettaglioTestataRow Details = new Utility.DichManagerICI.DettaglioTestataRow();
        //    try
        //    {
        //        SqlCommand SelectCommand = new SqlCommand();
        //        SelectCommand.CommandType = CommandType.StoredProcedure;
        //        SelectCommand.CommandText = this.ProcedureName;
        //        SelectCommand.Parameters.Add("@IdOggetto", SqlDbType.Int).Value = idImmobile;
        //        SelectCommand.Parameters.Add("@IdTestata", SqlDbType.Int).Value = idTestata;
        //        SelectCommand.Parameters.Add("@Contitolare", SqlDbType.Bit).Value = contitolare.ToString();
        //        DataTable Dettagli = Query(SelectCommand, new SqlConnection(myConn));
        //        if (Dettagli.Rows.Count > 0)
        //        {
        //            Details.ID = (int)Dettagli.Rows[0]["ID"];
        //            Details.Ente = (string)Dettagli.Rows[0]["Ente"];
        //            Details.IdTestata = (int)Dettagli.Rows[0]["IdTestata"];
        //            Details.NumeroOrdine = (string)Dettagli.Rows[0]["NumeroOrdine"];
        //            Details.NumeroModello = (string)Dettagli.Rows[0]["NumeroModello"];
        //            Details.IdOggetto = (int)Dettagli.Rows[0]["IdOggetto"];
        //            Details.IdSoggetto = (int)Dettagli.Rows[0]["IdSoggetto"];
        //            Details.TipoUtilizzo = Dettagli.Rows[0]["IdTipoUtilizzo"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoUtilizzo"];
        //            Details.TipoPossesso = Dettagli.Rows[0]["IdTipoPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["IdTipoPossesso"];
        //            Details.PercPossesso = Dettagli.Rows[0]["PercPossesso"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["PercPossesso"]);
        //            Details.MesiPossesso = Dettagli.Rows[0]["MesiPossesso"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiPossesso"];
        //            Details.MesiEsclusioneEsenzione = Dettagli.Rows[0]["MesiEsclusioneEsenzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiEsclusioneEsenzione"];
        //            Details.MesiRiduzione = Dettagli.Rows[0]["MesiRiduzione"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["MesiRiduzione"];
        //            Details.ImpDetrazAbitazPrincipale = Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Dettagli.Rows[0]["ImpDetrazAbitazPrincipale"]);
        //            Details.Contitolare = Dettagli.Rows[0]["Contitolare"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Contitolare"];
        //            Details.AbitazionePrincipale = (int)Dettagli.Rows[0]["AbitazionePrincipale"];
        //            Details.Bonificato = Dettagli.Rows[0]["Bonificato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Bonificato"];
        //            Details.Annullato = Dettagli.Rows[0]["Annullato"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["Annullato"];
        //            Details.DataInizioValidità = (DateTime)Dettagli.Rows[0]["DataInizioValidità"];
        //            Details.DataFineValidità = Dettagli.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : (DateTime)Dettagli.Rows[0]["DataFineValidità"];
        //            Details.Operatore = (string)Dettagli.Rows[0]["Operatore"];
        //            Details.Riduzione = (int)Dettagli.Rows[0]["Riduzione"];
        //            Details.Possesso = (int)Dettagli.Rows[0]["Possesso"];
        //            Details.EsclusioneEsenzione = (int)Dettagli.Rows[0]["EsclusioneEsenzione"];
        //            Details.AbitazionePrincipaleAttuale = (int)Dettagli.Rows[0]["AbitazionePrincipaleAttuale"];
        //            Details.NumeroUtilizzatori = Dettagli.Rows[0]["NumeroUtilizzatori"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["NumeroUtilizzatori"];
        //            //*** 20120629 - IMU ***
        //            Details.ColtivatoreDiretto = Dettagli.Rows[0]["coltivatorediretto"] == DBNull.Value ? false : (bool)Dettagli.Rows[0]["coltivatorediretto"];
        //            Details.NumeroFigli = Dettagli.Rows[0]["numerofigli"] == DBNull.Value ? 0 : (int)Dettagli.Rows[0]["numerofigli"];
        //            SqlCommand MyCmd = PrepareGetRowCaricoFigli(Details.ID, myConn);
        //            DataTable dtCaricoFigli = Query(MyCmd, new SqlConnection(Business.ConstWrapper.StringConnection));
        //            if (dtCaricoFigli.Rows.Count > 0)
        //            {
        //                int x = 0;
        //                Utility.DichManagerICI.CaricoFigliRow[] ListCarico = new Utility.DichManagerICI.CaricoFigliRow[dtCaricoFigli.Rows.Count];
        //                foreach (DataRow myRow in dtCaricoFigli.Rows)
        //                {
        //                    Utility.DichManagerICI.CaricoFigliRow CaricoFigli = new Utility.DichManagerICI.CaricoFigliRow();
        //                    CaricoFigli.IdDettaglioTestata = (int)myRow["IDDETTAGLIOTESTATA"];
        //                    CaricoFigli.nFiglio = (int)myRow["NFIGLIO"];
        //                    CaricoFigli.Percentuale = Double.Parse(myRow["PERCENTUALE"].ToString());
        //                    ListCarico[x] = CaricoFigli;
        //                    x++;
        //                }
        //                Details.ListCaricoFigli = ListCarico;
        //            }
        //            //*** ***
        //        }
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.GetRow.errore: ", Err);
        //        kill();
        //        Details = new Utility.DichManagerICI.DettaglioTestataRow();
        //    }
        //    finally
        //    {
        //        kill();
        //    }
        //    return Details;
        //}

        /// <summary>
        /// Prende tutti i dettagli che ha la testata, ma non i contitolari.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
        public DataTable ListDettagli(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try {
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Contitolare=0 AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.ListDettagli.errore: ", Err);
                throw Err;
            }
        }


		/// <summary>
		/// Prende tutti i dettagli - sia contitolari sia dettaglio non contitolare -
		/// di un immobile di una determinata dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public DataTable ListDettagli(int idTestata, int idOggetto)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND IDOggetto=@idOggetto" +
				" AND Contitolare=0 AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			SelectCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.ListDettagli.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Prende tutti i contitolari di un immobile di una determinata dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public DataTable List(int idTestata, int idOggetto)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND IDOggetto=@idOggetto" +
				" AND Contitolare=1 AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			SelectCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.List.errore: ", Err);
                throw Err;
            }
        }



		/// <summary>
		/// Calcola il numero dei contitolari appartenenti a un immobile.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public object CountContitolariByIDTestataIDOggetto(int idTestata, int idOggetto)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND IDOggetto=@idOggetto AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			SelectCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;

            return QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CountContitolariByIDTestatIDOggetto.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Calcola il numero dei contitolari appartenenti a una dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public object CountContitolariByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Annullato <> 1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

            return QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CountContitolaryByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Modifica soltanto l'id della testata (Per esempio dopo la storicizzazione).
		/// </summary>
		/// <param name="idTestataNuova"></param>
		/// <param name="idTestataVecchia"></param>
		/// <returns></returns>
		public bool Modify(int idTestataNuova, int idTestataVecchia)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try { 
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET IDTestata=@idTestataNuova " +
				"WHERE IDTestata=@idTestataVecchia";

			ModifyCommand.Parameters.Add("@idTestataNuova", SqlDbType.Int).Value = idTestataNuova;
			ModifyCommand.Parameters.Add("@idTestataVecchia", SqlDbType.Int).Value = idTestataVecchia;
            return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.Modify.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Esegue la cancellazione logica dei dati.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool CancellazioneLogica(int id)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "UPDATE " + this.TableName + " SET " +
				"Annullato=1 WHERE ID=@id";

			DeleteCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CancellazioneLogica.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Esegue la cancellazione logica dei dati.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public bool CancellazioneLogica(int idTestata, int idOggetto)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "UPDATE " + this.TableName + " SET " +
				"Annullato=1 WHERE IDTestata=@idTestata AND IDOggetto=@idOggetto";

			DeleteCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			DeleteCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;
                return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CancellazioneLogica.errore: ", Err);
                throw Err;
            }

        }

		/// <summary>
		/// Imposta il flag bonificato a true.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public bool Bonifica(int idTestata)
		{
			SqlCommand BonificaCommand = new SqlCommand();
            try { 
			BonificaCommand.CommandText = "UPDATE " + this.TableName +
				" SET Bonificato=1 WHERE IDTestata=@idTestata";

			BonificaCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
            return Execute(BonificaCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.Bonifica.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Calcola la quantità dei contitolari già bonificati che appartengono a un certo immobile.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <param name="idOggetto"></param>
		/// <returns></returns>
		public int CountContitolariBonificatiByIDTestataIDOggetto(int idTestata, int idOggetto)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND IDOggetto=@idOggetto AND Bonificato=1 AND Annullato<>1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;
			SelectCommand.Parameters.Add("@idOggetto",SqlDbType.Int).Value = idOggetto;

            return (int)QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CountContitolariBonificatiByIDTestataIDOggetto.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Calcola la quantità dei contitolari già bonificati vche appartengono a una certa dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public int CountContitolariBonificatiByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Bonificato=1 AND Annullato<>1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

            return (int)QueryScalar(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CountContitolariBonificatiByIDTestata.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Calcola la quantità dei contitolari già bonificati vche appartengono a una certa dichiarazione.
		/// </summary>
		/// <param name="idTestata"></param>
		/// <returns></returns>
		public int CountDettaglioNonBonificatiByIDTestata(int idTestata)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Count(*) FROM " + this.TableName +
				" WHERE IDTestata=@idTestata AND Bonificato=0 AND Annullato<>1";

			SelectCommand.Parameters.Add("@idTestata",SqlDbType.Int).Value = idTestata;

			return (int)QueryScalar(SelectCommand,new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.CountDettaglioNonBonificatiByIDTestats.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// Cancella la riga identificata dall'id dell'immobile e dall'id della testata.
        /// Così viene cancellata l'associazione tra un immobile e una dichiarazione.
        /// </summary>
        /// <param name="idTestata"></param>
        /// <returns></returns>
         public bool DeleteItemByTestata( int idTestata)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
				" WHERE IDTestata=@idTestata";
			
			DeleteCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;

            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.DeleteItemByTestata.errore: ", Err);
                throw Err;
            }
        }

        /*public bool DeleteItem(int idImmobile, int idTestata)
         {
             SqlCommand DeleteCommand = new SqlCommand();
             try{
             DeleteCommand.CommandText = "DELETE FROM " + this.TableName +
                 " WHERE IDOggetto=@idImmobile AND IDTestata=@idTestata";

             DeleteCommand.Parameters.Add("@idImmobile", SqlDbType.Int).Value = idImmobile;
             DeleteCommand.Parameters.Add("@idTestata", SqlDbType.Int).Value = idTestata;

             return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
              }
             catch (Exception Err)
             {
                 log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.DeleteItem.errore: ", Err);
                 throw Err;
             }
         }*/



        /// <summary>
        /// Creazione di una riga della tabella vuota.
        /// </summary>
        /// <param name="tabella"></param>
        /// <param name="codiceEnte"></param>
        /// <param name="idTestata"></param>
        /// <param name="operatore"></param>
        /// <param name="idOggetto"></param>
        /// <param name="numeroOrdine"></param>
        /// <param name="numeroModello"></param>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public static void Add_EmptyRow(ref DataTable tabella, string codiceEnte, int idTestata, string operatore,int idOggetto, string numeroOrdine, string numeroModello)
		{
            Utility.DichManagerICI.DettaglioTestataRow Item = new Utility.DichManagerICI.DettaglioTestataRow();
			Item.Ente = codiceEnte;
			Item.IdTestata = idTestata;
			Item.NumeroOrdine = numeroOrdine;
			Item.NumeroModello = numeroModello;
			Item.IdOggetto = idOggetto;
			Item.IdSoggetto = 0;
            //*** 20140509 - TASI ***
            Item.TipoUtilizzo = 1;
            Item.TipoPossesso = 1;
            //*** ***
			Item.PercPossesso = 0;
			Item.MesiPossesso = 0;
			Item.MesiEsclusioneEsenzione = 0;
			Item.MesiRiduzione = 0;
			Item.ImpDetrazAbitazPrincipale = 0;
			Item.Contitolare = true;
			Item.AbitazionePrincipale = 2; // non compilato
			Item.Bonificato = false;
			Item.Annullato = false;
			Item.DataInizioValidità = DateTime.Now;
			Item.DataFineValidità = DateTime.Now;
            Item.DataUltimaModifica = DateTime.MaxValue;
            Item.Operatore = operatore;
			Item.Riduzione = 2; // non compilato
			Item.Possesso = 2; // non compilato
			Item.EsclusioneEsenzione = 2; // non compilato
			Item.AbitazionePrincipaleAttuale = 0;
			Item.NumeroUtilizzatori = 0;

			DataRow Riga = tabella.NewRow();

			Riga["Ente"] = Item.Ente;
			Riga["IDTestata"] = Item.IdTestata;
			Riga["NumeroOrdine"] = Item.NumeroOrdine;
			Riga["NumeroModello"] = Item.NumeroModello;
			Riga["IDOggetto"] = Item.IdOggetto;
			Riga["IDSoggetto"] = Item.IdSoggetto;
            //*** 20140509 - TASI ***
            Riga["IdTipoUtilizzo"] = Item.TipoUtilizzo;
            Riga["IdTipoPossesso"] = Item.TipoPossesso;
            //*** ***
			Riga["PercPossesso"] = Item.PercPossesso;
			Riga["MesiPossesso"] = Item.MesiPossesso;
			Riga["MesiEsclusioneEsenzione"] = Item.MesiEsclusioneEsenzione;
			Riga["MesiRiduzione"] = Item.MesiRiduzione;
			Riga["ImpDetrazAbitazPrincipale"] = Item.ImpDetrazAbitazPrincipale;
			Riga["Contitolare"] = Item.Contitolare;
			Riga["AbitazionePrincipale"] = Item.AbitazionePrincipale;
			Riga["Bonificato"] = Item.Bonificato;
			Riga["Annullato"] = Item.Annullato;
			Riga["DataInizioValidità"] = Item.DataInizioValidità;
			Riga["DataFineValidità"] = Item.DataFineValidità;
			Riga["Operatore"] = Item.Operatore;
			Riga["Riduzione"] = Item.Riduzione;
			Riga["Possesso"] = Item.Possesso;
			Riga["EsclusioneEsenzione"] = Item.EsclusioneEsenzione;
			Riga["AbitazionePrincipaleAttuale"] = Item.AbitazionePrincipaleAttuale;
			Riga["NumeroUtilizzatori"] = Item.NumeroUtilizzatori;

			tabella.Rows.Add(Riga);
			return;
		}
        //public static void AddEmptyRow(ref DataTable tabella, string codiceEnte, int idTestata, string operatore, int idOggetto, string numeroOrdine, string numeroModello)
        //{
        //    Utility.DichManagerICI.DettaglioTestataRow Item = new Utility.DichManagerICI.DettaglioTestataRow();
        //    Item.Ente = codiceEnte;
        //    Item.IdTestata = idTestata;
        //    Item.NumeroOrdine = numeroOrdine;
        //    Item.NumeroModello = numeroModello;
        //    Item.IdOggetto = idOggetto;
        //    Item.IdSoggetto = 0;
        //    //*** 20140509 - TASI ***
        //    Item.TipoUtilizzo = 1;
        //    Item.TipoPossesso = 1;
        //    //*** ***
        //    Item.PercPossesso = 0;
        //    Item.MesiPossesso = 0;
        //    Item.MesiEsclusioneEsenzione = 0;
        //    Item.MesiRiduzione = 0;
        //    Item.ImpDetrazAbitazPrincipale = 0;
        //    Item.Contitolare = true;
        //    Item.AbitazionePrincipale = 2; // non compilato
        //    Item.Bonificato = false;
        //    Item.Annullato = false;
        //    Item.DataInizioValidità = DateTime.Now;
        //    Item.DataFineValidità = DateTime.Now;
        //    Item.Operatore = operatore;
        //    Item.Riduzione = 2; // non compilato
        //    Item.Possesso = 2; // non compilato
        //    Item.EsclusioneEsenzione = 2; // non compilato
        //    Item.AbitazionePrincipaleAttuale = 0;
        //    Item.NumeroUtilizzatori = 0;

        //    DataRow Riga = tabella.NewRow();

        //    Riga["Ente"] = Item.Ente;
        //    Riga["IDTestata"] = Item.IdTestata;
        //    Riga["NumeroOrdine"] = Item.NumeroOrdine;
        //    Riga["NumeroModello"] = Item.NumeroModello;
        //    Riga["IDOggetto"] = Item.IdOggetto;
        //    Riga["IDSoggetto"] = Item.IdSoggetto;
        //    //*** 20140509 - TASI ***
        //    Riga["IdTipoUtilizzo"] = Item.TipoUtilizzo;
        //    Riga["IdTipoPossesso"] = Item.TipoPossesso;
        //    //*** ***
        //    Riga["PercPossesso"] = Item.PercPossesso;
        //    Riga["MesiPossesso"] = Item.MesiPossesso;
        //    Riga["MesiEsclusioneEsenzione"] = Item.MesiEsclusioneEsenzione;
        //    Riga["MesiRiduzione"] = Item.MesiRiduzione;
        //    Riga["ImpDetrazAbitazPrincipale"] = Item.ImpDetrazAbitazPrincipale;
        //    Riga["Contitolare"] = Item.Contitolare;
        //    Riga["AbitazionePrincipale"] = Item.AbitazionePrincipale;
        //    Riga["Bonificato"] = Item.Bonificato;
        //    Riga["Annullato"] = Item.Annullato;
        //    Riga["DataInizioValidità"] = Item.DataInizioValidità;
        //    Riga["DataFineValidità"] = Item.DataFineValidità;
        //    Riga["Operatore"] = Item.Operatore;
        //    Riga["Riduzione"] = Item.Riduzione;
        //    Riga["Possesso"] = Item.Possesso;
        //    Riga["EsclusioneEsenzione"] = Item.EsclusioneEsenzione;
        //    Riga["AbitazionePrincipaleAttuale"] = Item.AbitazionePrincipaleAttuale;
        //    Riga["NumeroUtilizzatori"] = Item.NumeroUtilizzatori;

        //    tabella.Rows.Add(Riga);
        //    return;
        //}

        //*** 20120629 - IMU ***	
        public bool SetPercentualeCaricoFigli(int TypeOperation, Utility.DichManagerICI.CaricoFigliRow[] ListCarico, int IdDettaglio)
		{
			string sLog;
			SqlCommand MyCmd = new SqlCommand();
			bool retVal=false;
            try { 
			if (TypeOperation==0)
			{
                    foreach (Utility.DichManagerICI.CaricoFigliRow myRow in ListCarico)
                    {
					MyCmd.CommandText = "INSERT INTO TBLPERCENTUALECARICOFIGLI(IDDETTAGLIOTESTATA, NFIGLIO, PERCENTUALE)";
					MyCmd.CommandText += "VALUES (@IDDETTAGLIOTESTATA, @NFIGLIO, @PERCENTUALE)";

					MyCmd.Parameters.Clear();
					sLog=MyCmd.CommandText;
					MyCmd.Parameters.Add("@IDDETTAGLIOTESTATA",SqlDbType.Int).Value = IdDettaglio;
					sLog+=",'"+ IdDettaglio.ToString();
					MyCmd.Parameters.Add("@NFIGLIO",SqlDbType.Int).Value = myRow.nFiglio;
					sLog+=",'"+ myRow.nFiglio.ToString();
					MyCmd.Parameters.Add("@PERCENTUALE",SqlDbType.Int).Value = myRow.Percentuale;
					sLog+=",'"+  myRow.Percentuale.ToString();
			
					log.Debug("SetPercentualeCaricoFigli::Insert::SQL::" + sLog);
                    retVal = Execute(MyCmd, new SqlConnection(Business.ConstWrapper.StringConnection));
				}
			}
			else if (TypeOperation==2)
			{
				MyCmd.CommandText = "DELETE";
				MyCmd.CommandText +="  FROM TBLPERCENTUALECARICOFIGLI";
				MyCmd.CommandText +="  WHERE IDDETTAGLIOTESTATA=@IDDETTAGLIOTESTATA";

				MyCmd.Parameters.Clear();
				sLog=MyCmd.CommandText;
				MyCmd.Parameters.Add("@IDDETTAGLIOTESTATA",SqlDbType.Int).Value = IdDettaglio;
		
				log.Debug("SetPercentualeCaricoFigli::Delete::SQL::" + sLog);
                retVal = Execute(MyCmd, new SqlConnection(Business.ConstWrapper.StringConnection));
			}
			else
			{
				retVal= false;
			}
			return retVal;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.DettaglioTestataTable.SetPercentualeCaricoFigli.errore: ", Err);
                throw Err;
            }
        }

		//*** ***
	}
}
