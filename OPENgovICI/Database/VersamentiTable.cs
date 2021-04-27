using System;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace DichiarazioniICI.Database
{
    ///// <summary>
    ///// Struttura che rappresenta il singolo record della tabella TblVersamenti.
    ///// </summary>
    ///// <revisionHistory>
    ///// <revision date="28/08/2012">
    ///// <strong>IMU adeguamento per importi statali</strong>
    ///// </revision>
    ///// </revisionHistory>
    ///// <revisionHistory>
    ///// <revision date="22/04/2013">
    ///// <strong>aggiornamento IMU</strong>
    ///// </revision>
    ///// </revisionHistory>
    ///// <revisionHistory>
    ///// <revision date="30/04/2014">
    ///// <strong>TASI</strong>
    ///// </revision>
    ///// </revisionHistory>
    ///// <revisionHistory>
    ///// <revision date="12/04/2019">
    ///// <strong>Qualificazione AgID-analisi_rel01</strong>
    ///// <em>Analisi eventi</em>
    ///// </revision>
    ///// </revisionHistory>
    //public struct VersamentiRow
    //{
    //    public int ID;
    //    public string Ente;
    //    public int IdAnagrafico;
    //    //*** 20140630 - TASI ***
    //    public string CodTributo;
    //    //*** ***
    //    public string AnnoRiferimento;
    //    public string CodiceFiscale;
    //    public string PartitaIva;
    //    public decimal ImportoPagato;
    //    public DateTime DataPagamento;
    //    public string NumeroBollettino;
    //    public int NumeroFabbricatiPosseduti;
    //    public bool Acconto;
    //    public bool Saldo;
    //    public bool RavvedimentoOperoso;
    //    public decimal ImportoTerreni;
    //    public decimal ImportoAreeFabbric;
    //    public decimal ImportoAbitazPrincipale;
    //    public decimal ImportoAltriFabbric;
    //    public decimal DetrazioneAbitazPrincipale;
    //    /**** 20120828 - IMU adeguamento per importi statali ****/
    //    public decimal ImportoTerreniStatale;
    //    public decimal ImportoAreeFabbricStatale;
    //    public decimal ImportoAltriFabbricStatale;
    //    public decimal ImportoFabRurUsoStrum;
    //    /**** ****/
    //    /**** 20130422 - aggiornamento IMU ****/
    //    public decimal ImportoFabRurUsoStrumStatale;
    //    public decimal ImportoUsoProdCatD;
    //    public decimal ImportoUsoProdCatDStatale;
    //    /**** ****/
    //    public string ContoCorrente;
    //    public string ComuneUbicazioneImmobile;
    //    public string ComuneIntestatario;
    //    public bool Bonificato;
    //    public DateTime DataInizioValidità;
    //    public DateTime DataFineValidità;
    //    public string Operatore;
    //    public bool Annullato;
    //    public decimal ImportoSoprattassa;
    //    public decimal ImportoPenaPecuniaria;
    //    public decimal Interessi;
    //    public bool Violazione;
    //    public int IDProvenienza;
    //    public string NumeroAttoAccertamento;
    //    public DateTime DataProvvedimentoViolazione;
    //    public decimal ImportoPagatoArrotondamento;
    //    public DateTime DataRiversamento;
    //    public bool FlagFabbricatiExRurali;
    //    public string NumeroProvvedimentoViolazione;
    //    public decimal ImportoImposta;
    //    public string Note;
    //    public decimal DetrazioneStatale;
    //    public string Provenienza;
    //}

    /// <summary>
    /// Classe di gestione della tabella TblVersamenti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class VersamentiTable : Database
	{
		private string _username;
		private static readonly ILog log = LogManager.GetLogger(typeof(VersamentiTable));

		public VersamentiTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "TblVersamenti";
		}


        ///// <summary>
        ///// Inserisce un nuovo record a partire dai singoli campi.
        ///// </summary>
        ///// <param name="ente"></param>
        ///// <param name="idAnagrafico"></param>
        ///// <param name="annoRiferimento"></param>
        ///// <param name="codiceFiscale"></param>
        ///// <param name="partitaIva"></param>
        ///// <param name="importoPagato"></param>
        ///// <param name="dataPagamento"></param>
        ///// <param name="numeroBollettino"></param>
        ///// <param name="numeroFabbricatiPosseduti"></param>
        ///// <param name="acconto"></param>
        ///// <param name="saldo"></param>
        ///// <param name="ravvedimentoOperoso"></param>
        ///// <param name="impoTerreni"></param>
        ///// <param name="importoAreeFabbric"></param>
        ///// <param name="importoAbitazPrincipale"></param>
        ///// <param name="importoAltriFabbric"></param>
        ///// <param name="detrazioneAbitazPrincipale"></param>
        ///// <param name="contoCorrente"></param>
        ///// <param name="comuneUbicazioneImmobile"></param>
        ///// <param name="comuneIntestatario"></param>
        ///// <param name="bonificato"></param>
        ///// <param name="dataInizioValidità"></param>
        ///// <param name="dataFineValidità"></param>
        ///// <param name="operatore"></param>
        ///// <param name="annullato"></param>
        ///// <param name="importoSoprattassa"></param>
        ///// <param name="importoPenaPecuniaria"></param>
        ///// <param name="interessi"></param>
        ///// <param name="violazione"></param>
        ///// <param name="idProvenienza"></param>
        ///// <param name="NumeroAttoAccertamento"></param>
        ///// <param name="DataProvvedimentoViolazione"></param>
        ///// <param name="ImportoPagatoArrotondamento"></param>
        ///// <param name="idVersamento"></param>
        ///// <returns>
        ///// Restituisce un valore booleano:
        ///// true: se l'operazione è terminata correttamente
        ///// false: se si sono verificati errori.
        ///// </returns>
        ////*** 20140630 - TASI *** /**** 20130422 - aggiornamento IMU ****/
        //**** 20120828 - IMU adeguamento per importi statali ****/
        //public bool Insert(string ente, int idAnagrafico, string annoRiferimento, string codiceFiscale,
        //    string partitaIva, decimal importoPagato, DateTime dataPagamento, string numeroBollettino, int numeroFabbricatiPosseduti,
        //    bool acconto, bool saldo, bool ravvedimentoOperoso
        //    , decimal impoTerreni, decimal importoAreeFabbric, decimal importoAbitazPrincipale, decimal importoAltriFabbric, decimal detrazioneAbitazPrincipale
        //    , decimal impoTerreniStatale, decimal importoAreeFabbricStatale, decimal importoAltriFabbricStatale, decimal importoFabRurUsoStrum
        //    , decimal importoFabRurUsoStrumStatale, decimal importoUsoProdCatD, decimal importoUsoProdCatDStatale
        //    , string contoCorrente, string comuneUbicazioneImmobile,
        //    string comuneIntestatario, bool bonificato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore, bool annullato,
        //    decimal importoSoprattassa, decimal importoPenaPecuniaria, decimal interessi, bool violazione, int idProvenienza, string NumeroAttoAccertamento, DateTime DataProvvedimentoViolazione, decimal ImportoPagatoArrotondamento, DateTime MyDataRiversamento, out int idVersamento)
        //{
        //    SqlCommand cmdMyCommand = new SqlCommand();
        //try{
        //    cmdMyCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, IdAnagrafico, AnnoRiferimento, " +
        //        "CodiceFiscale, PartitaIva, ImportoPagato, DataPagamento, NumeroBollettino, NumeroFabbricatiPosseduti, Acconto, " +
        //        "Saldo, RavvedimentoOperoso" +
        //        ", ImpoTerreni, ImportoAreeFabbric, ImportoAbitazPrincipale, ImportoAltriFabbric, DetrazioneAbitazPrincipale" +
        //        ", IMPORTOTERRENISTATALE, IMPORTOAREEFABBRICSTATALE, IMPORTOALTRIFABBRICSTATALE, IMPORTOFABRURUSOSTRUM" +
        //        ", IMPORTOFABRURUSOSTRUMSTATALE,IMPORTOUSOPRODCATD,IMPORTOUSOPRODCATDSTATALE" +
        //        ", ContoCorrente, ComuneUbicazioneImmobile, " +
        //        "ComuneIntestatario, Bonificato, DataInizioValidità, DataFineValidità, Operatore, Annullato, " +
        //        "ImportoSoprattassa, ImportoPenaPecuniaria, Interessi, Violazione, IDProvenienza, NumeroAttoAccertamento, DataProvvedimentoViolazione, ImportoPagatoArrotondamento, DATARIVERSAMENTO) " +
        //        "VALUES (@ente, @idAnagrafico, @annoRiferimento, @codiceFiscale, @partitaIva, @importoPagato, " +
        //        "@dataPagamento, @numeroBollettino, @numeroFabbricatiPosseduti, @acconto, @saldo, @ravvedimentoOperoso, " +
        //        "@impoTerreni, @importoAreeFabbric, @importoAbitazPrincipale, @importoAltriFabbric, @detrazioneAbitazPrincipale, " +
        //        "@impoTerreniStatale, @importoAreeFabbricStatale, @importoAltriFabbricStatale, @importoFabRurUsoStrum " +
        //        ", @importoFabRurUsoStrumStatale,@importoUsoProdCatD,@importoUsoProdCatDStatale" +
        //        ",@contoCorrente, @comuneUbicazioneImmobile, @comuneIntestatario, @bonificato, @dataInizioValidità, " +
        //        "@dataFineValidità, @operatore, @annullato, @importoSoprattassa, @importoPenaPecuniaria, @interessi, @violazione, @idProvenienza, @numeroAttoAccertamento, @dataProvvedimentoViolazione, @ImportoPagatoArrotondamento, @DataRiversamento)";

        //    cmdMyCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
        //    cmdMyCommand.Parameters.Add("@idAnagrafico", SqlDbType.Int).Value = idAnagrafico;
        //    cmdMyCommand.Parameters.Add("@annoRiferimento", SqlDbType.VarChar).Value = annoRiferimento;
        //    cmdMyCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale;
        //    cmdMyCommand.Parameters.Add("@partitaIva", SqlDbType.VarChar).Value = partitaIva;
        //    cmdMyCommand.Parameters.Add("@importoPagato", SqlDbType.Decimal).Value = importoPagato;
        //    cmdMyCommand.Parameters.Add("@dataPagamento", SqlDbType.DateTime).Value = dataPagamento;
        //    cmdMyCommand.Parameters.Add("@numeroBollettino", SqlDbType.VarChar).Value = numeroBollettino;
        //    cmdMyCommand.Parameters.Add("@numeroFabbricatiPosseduti", SqlDbType.Int).Value = numeroFabbricatiPosseduti;
        //    cmdMyCommand.Parameters.Add("@acconto", SqlDbType.Bit).Value = acconto;
        //    cmdMyCommand.Parameters.Add("@saldo", SqlDbType.Bit).Value = saldo;
        //    cmdMyCommand.Parameters.Add("@ravvedimentoOperoso", SqlDbType.Bit).Value = ravvedimentoOperoso;
        //    cmdMyCommand.Parameters.Add("@impoTerreni", SqlDbType.Decimal).Value = impoTerreni;
        //    cmdMyCommand.Parameters.Add("@importoAreeFabbric", SqlDbType.Decimal).Value = importoAreeFabbric;
        //    cmdMyCommand.Parameters.Add("@importoAbitazPrincipale", SqlDbType.Decimal).Value = importoAbitazPrincipale;
        //    cmdMyCommand.Parameters.Add("@importoAltriFabbric", SqlDbType.Decimal).Value = importoAltriFabbric;
        //    cmdMyCommand.Parameters.Add("@detrazioneAbitazPrincipale", SqlDbType.Decimal).Value = detrazioneAbitazPrincipale;
        //    cmdMyCommand.Parameters.Add("@impoTerreniStatale", SqlDbType.Decimal).Value = impoTerreniStatale;
        //    cmdMyCommand.Parameters.Add("@importoAreeFabbricStatale", SqlDbType.Decimal).Value = importoAreeFabbricStatale;
        //    cmdMyCommand.Parameters.Add("@importoAltriFabbricStatale", SqlDbType.Decimal).Value = importoAltriFabbricStatale;
        //    cmdMyCommand.Parameters.Add("@importoFabRurUsoStrum", SqlDbType.Decimal).Value = importoFabRurUsoStrum;
        //    cmdMyCommand.Parameters.Add("@importoFabRurUsoStrumStatale", SqlDbType.Decimal).Value = importoFabRurUsoStrumStatale;
        //    cmdMyCommand.Parameters.Add("@importoUsoProdCatD", SqlDbType.Decimal).Value = importoUsoProdCatD;
        //    cmdMyCommand.Parameters.Add("@importoUsoProdCatDStatale", SqlDbType.Decimal).Value = importoUsoProdCatDStatale;
        //    cmdMyCommand.Parameters.Add("@contoCorrente", SqlDbType.VarChar).Value = contoCorrente;
        //    cmdMyCommand.Parameters.Add("@comuneUbicazioneImmobile", SqlDbType.VarChar).Value = comuneUbicazioneImmobile;
        //    cmdMyCommand.Parameters.Add("@comuneIntestatario", SqlDbType.VarChar).Value = comuneIntestatario;
        //    cmdMyCommand.Parameters.Add("@bonificato", SqlDbType.Bit).Value = bonificato;
        //    cmdMyCommand.Parameters.Add("@dataInizioValidità", SqlDbType.DateTime).Value = dataInizioValidità == DateTime.MinValue ? DBNull.Value : (object)dataInizioValidità;
        //    cmdMyCommand.Parameters.Add("@dataFineValidità", SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
        //    cmdMyCommand.Parameters.Add("@operatore", SqlDbType.VarChar).Value = operatore;
        //    cmdMyCommand.Parameters.Add("@annullato", SqlDbType.Bit).Value = annullato;
        //    cmdMyCommand.Parameters.Add("@importoSoprattassa", SqlDbType.Decimal).Value = importoSoprattassa;
        //    cmdMyCommand.Parameters.Add("@importoPenaPecuniaria", SqlDbType.Decimal).Value = importoPenaPecuniaria;
        //    cmdMyCommand.Parameters.Add("@interessi", SqlDbType.Decimal).Value = interessi;
        //    cmdMyCommand.Parameters.Add("@violazione", SqlDbType.Bit).Value = violazione;
        //    cmdMyCommand.Parameters.Add("@idProvenienza", SqlDbType.Int).Value = idProvenienza;
        //    // ALESSIO
        //    cmdMyCommand.Parameters.Add("@numeroAttoAccertamento", SqlDbType.VarChar).Value = NumeroAttoAccertamento;
        //    cmdMyCommand.Parameters.Add("@dataProvvedimentoViolazione", SqlDbType.VarChar).Value = DataProvvedimentoViolazione.ToShortDateString();
        //    //ALEP
        //    cmdMyCommand.Parameters.Add("@ImportoPagatoArrotondamento", SqlDbType.Decimal).Value = ImportoPagatoArrotondamento;
        //    cmdMyCommand.Parameters.Add("@DataRiversamento", SqlDbType.DateTime).Value = MyDataRiversamento == DateTime.MinValue ? DBNull.Value : (object)MyDataRiversamento;

        //    return Execute(cmdMyCommand, out idVersamento);
        // }
        //  catch (Exception Err)
        // {
        //   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.Insert.errore: ", Err);
        //  throw Err;
        // }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Item">VersamentiRow oggetto da inserire</param>
        ///// <param name="idVersamento">out int identificativo riga</param>
        ///// <returns>bool false in caso di errore altrimenti true</returns>
        ///// <revisionHistory>
        ///// <revision date="12/04/2019">
        ///// <strong>Qualificazione AgID-analisi_rel01</strong>
        ///// <em>Analisi eventi</em>
        ///// </revision>
        ///// </revisionHistory>
        //public bool SetVersamenti(VersamentiRow Item, out int idVersamento)
        //{
        //    SqlCommand cmdMyCommand = new SqlCommand();
        //    try
        //    {
        //        cmdMyCommand.CommandText = "prc_TBLVERSAMENTI_IU";
        //        cmdMyCommand.CommandType = CommandType.StoredProcedure;
        //        cmdMyCommand.Parameters.Add("@id", SqlDbType.Int).Value = Item.ID;
        //        cmdMyCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = Item.Ente;
        //        cmdMyCommand.Parameters.Add("@idAnagrafico", SqlDbType.Int).Value = Item.IdAnagrafico;
        //        cmdMyCommand.Parameters.Add("@Tributo", SqlDbType.NVarChar).Value = Item.CodTributo;
        //        cmdMyCommand.Parameters.Add("@annoRiferimento", SqlDbType.VarChar).Value = Item.AnnoRiferimento;
        //        cmdMyCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = Item.CodiceFiscale;
        //        cmdMyCommand.Parameters.Add("@partitaIva", SqlDbType.VarChar).Value = Item.PartitaIva;
        //        cmdMyCommand.Parameters.Add("@importoPagato", SqlDbType.Decimal).Value = Item.ImportoPagato;
        //        cmdMyCommand.Parameters.Add("@dataPagamento", SqlDbType.DateTime).Value = Item.DataPagamento;
        //        cmdMyCommand.Parameters.Add("@numeroBollettino", SqlDbType.VarChar).Value = Item.NumeroBollettino;
        //        cmdMyCommand.Parameters.Add("@numeroFabbricatiPosseduti", SqlDbType.Int).Value = Item.NumeroFabbricatiPosseduti;
        //        cmdMyCommand.Parameters.Add("@acconto", SqlDbType.Bit).Value = Item.Acconto;
        //        cmdMyCommand.Parameters.Add("@saldo", SqlDbType.Bit).Value = Item.Saldo;
        //        cmdMyCommand.Parameters.Add("@ravvedimentoOperoso", SqlDbType.Bit).Value = Item.RavvedimentoOperoso;
        //        cmdMyCommand.Parameters.Add("@impoTerreni", SqlDbType.Decimal).Value = Item.ImportoTerreni;
        //        cmdMyCommand.Parameters.Add("@importoAreeFabbric", SqlDbType.Decimal).Value = Item.ImportoAreeFabbric;
        //        cmdMyCommand.Parameters.Add("@importoAbitazPrincipale", SqlDbType.Decimal).Value = Item.ImportoAbitazPrincipale;
        //        cmdMyCommand.Parameters.Add("@importoAltriFabbric", SqlDbType.Decimal).Value = Item.ImportoAltriFabbric;
        //        cmdMyCommand.Parameters.Add("@detrazioneAbitazPrincipale", SqlDbType.Decimal).Value = Item.DetrazioneAbitazPrincipale;
        //        cmdMyCommand.Parameters.Add("@IMPORTOTERRENISTATALE", SqlDbType.Decimal).Value = Item.ImportoTerreniStatale;
        //        cmdMyCommand.Parameters.Add("@importoAreeFabbricStatale", SqlDbType.Decimal).Value = Item.ImportoAreeFabbricStatale;
        //        cmdMyCommand.Parameters.Add("@importoAltriFabbricStatale", SqlDbType.Decimal).Value = Item.ImportoAltriFabbricStatale;
        //        cmdMyCommand.Parameters.Add("@importoFabRurUsoStrum", SqlDbType.Decimal).Value = Item.ImportoFabRurUsoStrum;
        //        cmdMyCommand.Parameters.Add("@importoFabRurUsoStrumStatale", SqlDbType.Decimal).Value = Item.ImportoFabRurUsoStrumStatale;
        //        cmdMyCommand.Parameters.Add("@importoUsoProdCatD", SqlDbType.Decimal).Value = Item.ImportoUsoProdCatD;
        //        cmdMyCommand.Parameters.Add("@importoUsoProdCatDStatale", SqlDbType.Decimal).Value = Item.ImportoUsoProdCatDStatale;
        //        cmdMyCommand.Parameters.Add("@contoCorrente", SqlDbType.VarChar).Value = Item.ContoCorrente;
        //        cmdMyCommand.Parameters.Add("@comuneUbicazioneImmobile", SqlDbType.VarChar).Value = Item.ComuneUbicazioneImmobile;
        //        cmdMyCommand.Parameters.Add("@comuneIntestatario", SqlDbType.VarChar).Value = Item.ComuneIntestatario;
        //        cmdMyCommand.Parameters.Add("@bonificato", SqlDbType.Bit).Value = Item.Bonificato;
        //        cmdMyCommand.Parameters.Add("@dataInizioValidità", SqlDbType.DateTime).Value = Item.DataInizioValidità == DateTime.MinValue ? DBNull.Value : (object)Item.DataInizioValidità;
        //        cmdMyCommand.Parameters.Add("@dataFineValidità", SqlDbType.DateTime).Value = Item.DataFineValidità == DateTime.MinValue ? DBNull.Value : (object)Item.DataFineValidità;
        //        cmdMyCommand.Parameters.Add("@operatore", SqlDbType.VarChar).Value = Item.Operatore;
        //        cmdMyCommand.Parameters.Add("@annullato", SqlDbType.Bit).Value = Item.Annullato;
        //        cmdMyCommand.Parameters.Add("@importoSoprattassa", SqlDbType.Decimal).Value = Item.ImportoSoprattassa;
        //        cmdMyCommand.Parameters.Add("@importoPenaPecuniaria", SqlDbType.Decimal).Value = Item.ImportoPenaPecuniaria;
        //        cmdMyCommand.Parameters.Add("@interessi", SqlDbType.Decimal).Value = Item.Interessi;
        //        cmdMyCommand.Parameters.Add("@violazione", SqlDbType.Bit).Value = Item.Violazione;
        //        cmdMyCommand.Parameters.Add("@idProvenienza", SqlDbType.Int).Value = Item.IDProvenienza;
        //        cmdMyCommand.Parameters.Add("@FlagFabbricatiExRurali", SqlDbType.Bit).Value = Item.FlagFabbricatiExRurali;
        //        cmdMyCommand.Parameters.Add("@ImportoImposta", SqlDbType.Decimal).Value = Item.ImportoImposta;
        //        cmdMyCommand.Parameters.Add("@Note", SqlDbType.VarChar).Value = Item.Note;
        //        cmdMyCommand.Parameters.Add("@DetrazioneStatale", SqlDbType.Decimal).Value = Item.DetrazioneStatale;
        //        // ALESSIO
        //        cmdMyCommand.Parameters.Add("@numeroAttoAccertamento", SqlDbType.VarChar).Value = Item.NumeroAttoAccertamento;
        //        cmdMyCommand.Parameters.Add("@dataProvvedimentoViolazione", SqlDbType.VarChar).Value = Item.DataProvvedimentoViolazione.ToShortDateString();
        //        cmdMyCommand.Parameters.Add("@NumeroProvvedimentoViolazione", SqlDbType.VarChar).Value = Item.NumeroProvvedimentoViolazione;
        //        //ALEP
        //        cmdMyCommand.Parameters.Add("@ImportoPagatoArrotondamento", SqlDbType.Decimal).Value = Item.ImportoPagatoArrotondamento;
        //        cmdMyCommand.Parameters.Add("@DataRiversamento", SqlDbType.DateTime).Value = Item.DataRiversamento == DateTime.MinValue ? DBNull.Value : (object)Item.DataRiversamento;
        //        cmdMyCommand.Parameters.Add("@Provenienza", SqlDbType.VarChar).Value = Item.Provenienza;

        //        cmdMyCommand.Parameters["@id"].Direction = ParameterDirection.InputOutput;
        //        return Execute(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection), "@ID", out idVersamento);
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.SetVersamenti.errore: ", Err);
        //        throw Err;
        //    }
        //}

        /*public bool Insert(string ente, int idAnagrafico, string annoRiferimento, string codiceFiscale, 
			string partitaIva, decimal importoPagato, DateTime dataPagamento, string numeroBollettino, int numeroFabbricatiPosseduti, 
			bool acconto, bool saldo, bool ravvedimentoOperoso
			, decimal impoTerreni, decimal importoAreeFabbric, decimal importoAbitazPrincipale, decimal importoAltriFabbric, decimal detrazioneAbitazPrincipale
			, string contoCorrente, string comuneUbicazioneImmobile, 
			string comuneIntestatario, bool bonificato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore, bool annullato,
			decimal importoSoprattassa, decimal importoPenaPecuniaria, decimal interessi, bool violazione, int idProvenienza,string NumeroAttoAccertamento, DateTime DataProvvedimentoViolazione, decimal ImportoPagatoArrotondamento, DateTime MyDataRiversamento,  out int idVersamento)
		{
			SqlCommand InsertCommand = new SqlCommand();
            try{
			InsertCommand.CommandText = "INSERT INTO " + this.TableName + " (Ente, IdAnagrafico, AnnoRiferimento, " +
				"CodiceFiscale, PartitaIva, ImportoPagato, DataPagamento, NumeroBollettino, NumeroFabbricatiPosseduti, Acconto, " +
				"Saldo, RavvedimentoOperoso, ImpoTerreni, ImportoAreeFabbric, ImportoAbitazPrincipale, " +
				"ImportoAltriFabbric, DetrazioneAbitazPrincipale, ContoCorrente, ComuneUbicazioneImmobile, " +
				"ComuneIntestatario, Bonificato, DataInizioValidità, DataFineValidità, Operatore, Annullato, " +
				"ImportoSoprattassa, ImportoPenaPecuniaria, Interessi, Violazione, IDProvenienza, NumeroAttoAccertamento, DataProvvedimentoViolazione, ImportoPagatoArrotondamento, DATARIVERSAMENTO) " +
				"VALUES (@ente, @idAnagrafico, @annoRiferimento, @codiceFiscale, @partitaIva, @importoPagato, " +
				"@dataPagamento, @numeroBollettino, @numeroFabbricatiPosseduti, @acconto, @saldo, @ravvedimentoOperoso, " +
				"@impoTerreni, @importoAreeFabbric, @importoAbitazPrincipale, @importoAltriFabbric, @detrazioneAbitazPrincipale, " +
				"@contoCorrente, @comuneUbicazioneImmobile, @comuneIntestatario, @bonificato, @dataInizioValidità, " +
				"@dataFineValidità, @operatore, @annullato, @importoSoprattassa, @importoPenaPecuniaria, @interessi, @violazione, @idProvenienza, @numeroAttoAccertamento, @dataProvvedimentoViolazione, @ImportoPagatoArrotondamento, @DataRiversamento)";

			InsertCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			InsertCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;
			InsertCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			InsertCommand.Parameters.Add("@codiceFiscale",SqlDbType.VarChar).Value = codiceFiscale;
			InsertCommand.Parameters.Add("@partitaIva",SqlDbType.VarChar).Value = partitaIva;
			InsertCommand.Parameters.Add("@importoPagato",SqlDbType.Decimal).Value = importoPagato;
			InsertCommand.Parameters.Add("@dataPagamento",SqlDbType.DateTime).Value = dataPagamento;
			InsertCommand.Parameters.Add("@numeroBollettino",SqlDbType.VarChar).Value = numeroBollettino;
			InsertCommand.Parameters.Add("@numeroFabbricatiPosseduti",SqlDbType.Int).Value = numeroFabbricatiPosseduti;
			InsertCommand.Parameters.Add("@acconto",SqlDbType.Bit).Value = acconto;
			InsertCommand.Parameters.Add("@saldo",SqlDbType.Bit).Value = saldo;
			InsertCommand.Parameters.Add("@ravvedimentoOperoso",SqlDbType.Bit).Value = ravvedimentoOperoso;
			InsertCommand.Parameters.Add("@impoTerreni",SqlDbType.Decimal).Value = impoTerreni;
			InsertCommand.Parameters.Add("@importoAreeFabbric",SqlDbType.Decimal).Value = importoAreeFabbric;
			InsertCommand.Parameters.Add("@importoAbitazPrincipale",SqlDbType.Decimal).Value = importoAbitazPrincipale;
			InsertCommand.Parameters.Add("@importoAltriFabbric",SqlDbType.Decimal).Value = importoAltriFabbric;
			InsertCommand.Parameters.Add("@detrazioneAbitazPrincipale",SqlDbType.Decimal).Value = detrazioneAbitazPrincipale;
			InsertCommand.Parameters.Add("@contoCorrente",SqlDbType.VarChar).Value = contoCorrente;
			InsertCommand.Parameters.Add("@comuneUbicazioneImmobile",SqlDbType.VarChar).Value = comuneUbicazioneImmobile;
			InsertCommand.Parameters.Add("@comuneIntestatario",SqlDbType.VarChar).Value = comuneIntestatario;
			InsertCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
			InsertCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità == DateTime.MinValue ? DBNull.Value : (object)dataInizioValidità;
			InsertCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;
			InsertCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
			InsertCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
			InsertCommand.Parameters.Add("@importoSoprattassa",SqlDbType.Decimal).Value = importoSoprattassa;
			InsertCommand.Parameters.Add("@importoPenaPecuniaria",SqlDbType.Decimal).Value = importoPenaPecuniaria;
			InsertCommand.Parameters.Add("@interessi",SqlDbType.Decimal).Value = interessi;
			InsertCommand.Parameters.Add("@violazione",SqlDbType.Bit).Value = violazione;
			InsertCommand.Parameters.Add("@idProvenienza",SqlDbType.Int).Value = idProvenienza;
			// ALESSIO
			InsertCommand.Parameters.Add("@numeroAttoAccertamento", SqlDbType.VarChar).Value = NumeroAttoAccertamento;
			InsertCommand.Parameters.Add("@dataProvvedimentoViolazione", SqlDbType.VarChar).Value = DataProvvedimentoViolazione.ToShortDateString();
			//ALEP
			InsertCommand.Parameters.Add("@ImportoPagatoArrotondamento",SqlDbType.Decimal).Value = ImportoPagatoArrotondamento;
			InsertCommand.Parameters.Add("@DataRiversamento",SqlDbType.DateTime).Value = MyDataRiversamento == DateTime.MinValue ? DBNull.Value : (object)MyDataRiversamento;


			return Execute(InsertCommand, out idVersamento);
             }
                  catch (Exception Err)
                 {
                   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.Insert.errore: ", Err);
                  throw Err;
                 }
		}*/


        /**** ****/

        /// <summary>
        /// Inserisce un nuovo record a partire da una struttura row.
        /// </summary>
        /// <param name="TypeOperation">int tipo di operazione</param>
        /// <param name="Item">VersamentiRow oggetto da gestire</param>
        /// <param name="idVersamento">out int identificativo riga</param>
        /// <returns>
        /// Restituisce un valore booleano:
        /// true: se l'operazione è terminata correttamente
        /// false: se si sono verificati errori.
        /// </returns>
        /// <revisionHistory>
        /// <revision date="30/06/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public bool Insert(int TypeOperation,Utility.DichManagerICI.VersamentiRow Item, out int idVersamento)
		{
               try
            {
                return new Utility.DichManagerICI(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection).SetVersamenti(TypeOperation, Item, out idVersamento);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.Insert.errore: ", Err);
                throw Err;
            }
		}
        //public bool Insert(VersamentiRow Item, out int idVersamento)
        //{
        //    //*** 20140630 - TASI ***
        //    //return Insert(Item.Ente, Item.IdAnagrafico, Item.AnnoRiferimento, Item.CodiceFiscale,
        //    //    Item.PartitaIva, Item.ImportoPagato, Item.DataPagamento, Item.NumeroBollettino,
        //    //    Item.NumeroFabbricatiPosseduti, Item.Acconto, Item.Saldo, Item.RavvedimentoOperoso,
        //    //    Item.ImportoTerreni, Item.ImportoAreeFabbric, Item.ImportoAbitazPrincipale, Item.ImportoAltriFabbric, Item.DetrazioneAbitazPrincipale
        //    //    , Item.ImportoTerreniStatale, Item.ImportoAreeFabbricStatale, Item.ImportoAltriFabbricStatale, Item.ImportoFabRurUsoStrum
        //    //    , Item.ImportoFabRurUsoStrumStatale, Item.ImportoUsoProdCatD, Item.ImportoUsoProdCatDStatale
        //    //    , Item.ContoCorrente,
        //    //    Item.ComuneUbicazioneImmobile, Item.ComuneIntestatario, Item.Bonificato,
        //    //    Item.DataInizioValidità, Item.DataFineValidità, Item.Operatore, Item.Annullato,
        //    //    Item.ImportoSoprattassa, Item.ImportoPenaPecuniaria, Item.Interessi, Item.Violazione,
        //    //    Item.IDProvenienza, Item.NumeroAttoAccertamento, Item.DataProvvedimentoViolazione, Item.ImportoPagatoArrotondamento, Item.DataRiversamento, out idVersamento);
        //    return SetVersamenti(Item, out idVersamento);
        //    //*** ***
        //}
        ///// <summary>
        ///// Aggiorna un record individuato dall'identity a partire dai singoli campi.
        ///// </summary>
        ///// <param name="Item">VersamentiRow oggetto da gestire</param>
        ///// <param name="idVersamento">out int identificativo riga</param>
        ///// <returns>
        ///// Restituisce un valore booleano:
        ///// true: se l'operazione è terminata correttamente
        ///// false: se si sono verificati errori.
        ///// </returns>
        ///// <revisionHistory>
        ///// <revision date="28/08/2012">
        ///// <strong>IMU adeguamento per importi statali</strong>
        ///// </revision>
        ///// <revisionHistory>
        ///// <revision date="22/04/2013">
        ///// <strong>aggiornamento IMU</strong>
        ///// </revision>
        ///// </revisionHistory>
        ///// </revisionHistory>
        ///// <revisionHistory>
        ///// <revisionHistory>
        ///// <revision date="12/04/2019">
        ///// <strong>Qualificazione AgID-analisi_rel01</strong>
        ///// <em>Analisi eventi</em>
        ///// </revision>
        ///// </revisionHistory>
        //      public bool Modify(int id, string ente, int idAnagrafico, string annoRiferimento, string codiceFiscale, 
        //	string partitaIva, decimal importoPagato, DateTime dataPagamento, string numeroBollettino, int numeroFabbricatiPosseduti, 
        //	bool acconto, bool saldo, bool ravvedimentoOperoso
        //	, decimal impoTerreni, decimal importoAreeFabbric, decimal importoAbitazPrincipale, decimal importoAltriFabbric, decimal detrazioneAbitazPrincipale
        //	, decimal impoTerreniStatale, decimal importoAreeFabbricStatale, decimal importoAltriFabbricStatale, decimal importoFabRurUsoStrum
        //	, decimal importoFabRurUsoStrumStatale, decimal importoUsoProdCatD, decimal importoUsoProdCatDStatale
        //	, string contoCorrente, string comuneUbicazioneImmobile, 
        //	string comuneIntestatario, bool bonificato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore, bool annullato,
        //	decimal importoSoprattassa, decimal importoPenaPecuniaria, decimal interessi, bool violazione, string NumeroAttoAccertamento, DateTime DataProvvedimentoViolazione,int idProvenienza, DateTime MyDataRiversamento)
        //{
        //	SqlCommand ModifyCommand = new SqlCommand();
        //          try { 
        //	ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, IdAnagrafico=@idAnagrafico, " +
        //		"AnnoRiferimento=@annoRiferimento, CodiceFiscale=@codiceFiscale, " +
        //		"PartitaIva=@partitaIva, ImportoPagato=@importoPagato, DataPagamento=@dataPagamento, " +
        //		"NumeroBollettino=@numeroBollettino, NumeroFabbricatiPosseduti=@numeroFabbricatiPosseduti, Acconto=@acconto, " +
        //		"Saldo=@saldo, RavvedimentoOperoso=@ravvedimentoOperoso"+
        //		", ImpoTerreni=@impoTerreni, ImportoAreeFabbric=@importoAreeFabbric, ImportoAbitazPrincipale=@importoAbitazPrincipale, ImportoAltriFabbric=@importoAltriFabbric, DetrazioneAbitazPrincipale=@detrazioneAbitazPrincipale" +
        //		", IMPORTOTERRENISTATALE=@impoTerreniStatale, IMPORTOAREEFABBRICSTATALE=@importoAreeFabbricStatale,IMPORTOALTRIFABBRICSTATALE =@importoAltriFabbricStatale, IMPORTOFABRURUSOSTRUM=@importoFabRurUsoStrum" +
        //		", IMPORTOFABRURUSOSTRUMSTATALE=@importoFabRurUsoStrumStatale" +
        //		", IMPORTOUSOPRODCATD=@importoUsoProdCatD" +
        //		", IMPORTOUSOPRODCATDSTATALE=@importoUsoProdCatDStatale" +
        //		", ContoCorrente=@contoCorrente, ComuneUbicazioneImmobile=@comuneUbicazioneImmobile, ComuneIntestatario=@comuneIntestatario, " +
        //		"Bonificato=@bonificato, DataInizioValidità=@dataInizioValidità, DataFineValidità=@dataFineValidità, " +
        //		"Operatore=@operatore, Annullato=@annullato, ImportoSoprattassa=@importoSoprattassa, ImportoPenaPecuniaria=@importoPenaPecuniaria, " +
        //		"Interessi=@interessi, Violazione=@violazione, IDProvenienza=@idProvenienza, NumeroAttoAccertamento=@numeroAttoAccertamento, DataProvvedimentoViolazione=@dataProvvedimentoViolazione," +
        //		"DATARIVERSAMENTO=@DataRiversamento" +
        //		" WHERE ID=@id";

        //	ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
        //	ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
        //	ModifyCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;
        //	ModifyCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
        //	ModifyCommand.Parameters.Add("@codiceFiscale",SqlDbType.VarChar).Value = codiceFiscale;
        //	ModifyCommand.Parameters.Add("@partitaIva",SqlDbType.VarChar).Value = partitaIva;
        //	ModifyCommand.Parameters.Add("@importoPagato",SqlDbType.Decimal).Value = importoPagato;
        //	ModifyCommand.Parameters.Add("@dataPagamento",SqlDbType.DateTime).Value = dataPagamento;
        //	ModifyCommand.Parameters.Add("@numeroBollettino",SqlDbType.VarChar).Value = numeroBollettino;
        //	ModifyCommand.Parameters.Add("@numeroFabbricatiPosseduti",SqlDbType.Int).Value = numeroFabbricatiPosseduti;
        //	ModifyCommand.Parameters.Add("@acconto",SqlDbType.Bit).Value = acconto;
        //	ModifyCommand.Parameters.Add("@saldo",SqlDbType.Bit).Value = saldo;
        //	ModifyCommand.Parameters.Add("@ravvedimentoOperoso",SqlDbType.Bit).Value = ravvedimentoOperoso;
        //	ModifyCommand.Parameters.Add("@impoTerreni",SqlDbType.Decimal).Value = impoTerreni;
        //	ModifyCommand.Parameters.Add("@importoAreeFabbric",SqlDbType.Decimal).Value = importoAreeFabbric;
        //	ModifyCommand.Parameters.Add("@importoAbitazPrincipale",SqlDbType.Decimal).Value = importoAbitazPrincipale;
        //	ModifyCommand.Parameters.Add("@importoAltriFabbric",SqlDbType.Decimal).Value = importoAltriFabbric;
        //	ModifyCommand.Parameters.Add("@impoTerreniStatale",SqlDbType.Decimal).Value = impoTerreniStatale;
        //	ModifyCommand.Parameters.Add("@importoAreeFabbricStatale",SqlDbType.Decimal).Value = importoAreeFabbricStatale;
        //	ModifyCommand.Parameters.Add("@importoAltriFabbricStatale",SqlDbType.Decimal).Value = importoAltriFabbricStatale;
        //	ModifyCommand.Parameters.Add("@importoFabRurUsoStrum",SqlDbType.Decimal).Value = importoFabRurUsoStrum;
        //	ModifyCommand.Parameters.Add("@importoFabRurUsoStrumStatale",SqlDbType.Decimal).Value = importoFabRurUsoStrumStatale;
        //	ModifyCommand.Parameters.Add("@importoUsoProdCatD",SqlDbType.Decimal).Value = importoUsoProdCatD;
        //	ModifyCommand.Parameters.Add("@importoUsoProdCatDStatale",SqlDbType.Decimal).Value = importoUsoProdCatDStatale;
        //	ModifyCommand.Parameters.Add("@detrazioneAbitazPrincipale",SqlDbType.Decimal).Value = detrazioneAbitazPrincipale;
        //	ModifyCommand.Parameters.Add("@contoCorrente",SqlDbType.VarChar).Value = contoCorrente;
        //	ModifyCommand.Parameters.Add("@comuneUbicazioneImmobile",SqlDbType.VarChar).Value = comuneUbicazioneImmobile;
        //	ModifyCommand.Parameters.Add("@comuneIntestatario",SqlDbType.VarChar).Value = comuneIntestatario;
        //	ModifyCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
        //	ModifyCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
        //	ModifyCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;;
        //	ModifyCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
        //	ModifyCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
        //	ModifyCommand.Parameters.Add("@importoSoprattassa",SqlDbType.Decimal).Value = importoSoprattassa;
        //	ModifyCommand.Parameters.Add("@importoPenaPecuniaria",SqlDbType.Decimal).Value = importoPenaPecuniaria;
        //	ModifyCommand.Parameters.Add("@interessi",SqlDbType.Decimal).Value = interessi;
        //	ModifyCommand.Parameters.Add("@violazione",SqlDbType.Bit).Value = violazione;
        //	ModifyCommand.Parameters.Add("@idProvenienza",SqlDbType.Int).Value = idProvenienza;
        //	ModifyCommand.Parameters.Add("@dataProvvedimentoViolazione", SqlDbType.VarChar).Value = DataProvvedimentoViolazione.ToShortDateString();
        //	ModifyCommand.Parameters.Add("@numeroAttoAccertamento", SqlDbType.VarChar).Value = NumeroAttoAccertamento;
        //	ModifyCommand.Parameters.Add("@DataRiversamento",SqlDbType.DateTime).Value = MyDataRiversamento == DateTime.MinValue ? DBNull.Value : (object)MyDataRiversamento;;

        //	log.Debug("VersamentiTable::Modify::query::"+ ModifyCommand.CommandText+"::@id::"+id.ToString()+
        //		"::@ente::"+ente.ToString()+
        //		"::@idAnagrafico::"+idAnagrafico.ToString()+
        //		"::@annoRiferimento::"+annoRiferimento.ToString()+
        //		"::@codiceFiscale::"+codiceFiscale.ToString()+
        //		"::@partitaIva::"+partitaIva.ToString()+
        //		"::@importoPagato::"+importoPagato.ToString()+
        //		"::@dataPagamento::"+dataPagamento.ToString()+
        //		"::@numeroBollettino::"+numeroBollettino.ToString()+
        //		"::@numeroFabbricatiPosseduti::"+numeroFabbricatiPosseduti.ToString()+
        //		"::@acconto::"+acconto.ToString()+
        //		"::@saldo::"+saldo.ToString()+
        //		"::@ravvedimentoOperoso::"+ravvedimentoOperoso.ToString()+
        //		"::@impoTerreni::"+impoTerreni.ToString()+
        //		"::@importoAreeFabbric::"+importoAreeFabbric.ToString()+
        //		"::@importoAbitazPrincipale::"+importoAbitazPrincipale.ToString()+
        //		"::@importoAltriFabbric::"+importoAltriFabbric.ToString()+
        //		"::@impoTerreniStatale::"+impoTerreniStatale.ToString()+
        //		"::@importoAreeFabbricStatale::"+importoAreeFabbricStatale.ToString()+
        //		"::@importoAltriFabbricStatale::"+importoAltriFabbricStatale.ToString()+
        //		"::@importoFabRurUsoStrum::"+importoFabRurUsoStrum.ToString()+
        //		"::@importoFabRurUsoStrumStatale::"+importoFabRurUsoStrumStatale.ToString()+
        //		"::@importoUsoProdCatD::"+importoUsoProdCatD.ToString()+
        //		"::@importoUsoProdCatDStatale::"+importoUsoProdCatDStatale.ToString()+
        //		"::@detrazioneAbitazPrincipale::"+detrazioneAbitazPrincipale.ToString()+
        //		"::@contoCorrente::"+contoCorrente.ToString()+
        //		"::@comuneUbicazioneImmobile::"+comuneUbicazioneImmobile.ToString()+
        //		"::@comuneIntestatario::"+comuneIntestatario.ToString()+
        //		"::@bonificato::"+bonificato.ToString()+
        //		"::@dataInizioValidità::"+dataInizioValidità.ToString()+
        //		"::@dataFineValidità::"+dataFineValidità.ToString()+
        //		"::@operatore::"+operatore.ToString()+
        //		"::@annullato::"+annullato.ToString()+
        //		"::@importoSoprattassa::"+importoSoprattassa.ToString()+
        //		"::@importoPenaPecuniaria::"+importoPenaPecuniaria.ToString()+
        //		"::@interessi::"+interessi.ToString()+
        //		"::@violazione::"+violazione.ToString()+
        //		"::@idProvenienza::"+idProvenienza.ToString()+
        //		"::@dataProvvedimentoViolazione::"+DataProvvedimentoViolazione.ToShortDateString()+
        //		"::@numeroAttoAccertamento::"+NumeroAttoAccertamento.ToString()+
        //		"::@DataRiversamento::"+MyDataRiversamento);
        //          return Execute(ModifyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //          }
        //          catch (Exception Err)
        //          {
        //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.Modify.errore: ", Err);
        //              throw Err;
        //          }
        //      }

        /*public bool Modify(int id, string ente, int idAnagrafico, string annoRiferimento, string codiceFiscale, 
			string partitaIva, decimal importoPagato, DateTime dataPagamento, string numeroBollettino, int numeroFabbricatiPosseduti, 
			bool acconto, bool saldo, bool ravvedimentoOperoso, decimal impoTerreni, decimal importoAreeFabbric, decimal importoAbitazPrincipale, 
			decimal importoAltriFabbric, decimal detrazioneAbitazPrincipale, string contoCorrente, string comuneUbicazioneImmobile, 
			string comuneIntestatario, bool bonificato, DateTime dataInizioValidità, DateTime dataFineValidità, string operatore, bool annullato,
			decimal importoSoprattassa, decimal importoPenaPecuniaria, decimal interessi, bool violazione, string NumeroAttoAccertamento, DateTime DataProvvedimentoViolazione,int idProvenienza, DateTime MyDataRiversamento)
		{
			SqlCommand ModifyCommand = new SqlCommand();
            try{
			ModifyCommand.CommandText = "UPDATE " + this.TableName + " SET Ente=@ente, IdAnagrafico=@idAnagrafico, " +
				"AnnoRiferimento=@annoRiferimento, CodiceFiscale=@codiceFiscale, " +
				"PartitaIva=@partitaIva, ImportoPagato=@importoPagato, DataPagamento=@dataPagamento, " +
				"NumeroBollettino=@numeroBollettino, NumeroFabbricatiPosseduti=@numeroFabbricatiPosseduti, Acconto=@acconto, " +
				"Saldo=@saldo, RavvedimentoOperoso=@ravvedimentoOperoso, ImpoTerreni=@impoTerreni, " +
				"ImportoAreeFabbric=@importoAreeFabbric, ImportoAbitazPrincipale=@importoAbitazPrincipale, " +
				"ImportoAltriFabbric=@importoAltriFabbric, DetrazioneAbitazPrincipale=@detrazioneAbitazPrincipale, " +
				"ContoCorrente=@contoCorrente, ComuneUbicazioneImmobile=@comuneUbicazioneImmobile, ComuneIntestatario=@comuneIntestatario, " +
				"Bonificato=@bonificato, DataInizioValidità=@dataInizioValidità, DataFineValidità=@dataFineValidità, " +
				"Operatore=@operatore, Annullato=@annullato, ImportoSoprattassa=@importoSoprattassa, ImportoPenaPecuniaria=@importoPenaPecuniaria, " +
				"Interessi=@interessi, Violazione=@violazione, IDProvenienza=@idProvenienza, NumeroAttoAccertamento=@numeroAttoAccertamento, DataProvvedimentoViolazione=@dataProvvedimentoViolazione," +
				"DATARIVERSAMENTO=@DataRiversamento" +
				" WHERE ID=@id";

			ModifyCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
			ModifyCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			ModifyCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;
			ModifyCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			ModifyCommand.Parameters.Add("@codiceFiscale",SqlDbType.VarChar).Value = codiceFiscale;
			ModifyCommand.Parameters.Add("@partitaIva",SqlDbType.VarChar).Value = partitaIva;
			ModifyCommand.Parameters.Add("@importoPagato",SqlDbType.Decimal).Value = importoPagato;
			ModifyCommand.Parameters.Add("@dataPagamento",SqlDbType.DateTime).Value = dataPagamento;
			ModifyCommand.Parameters.Add("@numeroBollettino",SqlDbType.VarChar).Value = numeroBollettino;
			ModifyCommand.Parameters.Add("@numeroFabbricatiPosseduti",SqlDbType.Int).Value = numeroFabbricatiPosseduti;
			ModifyCommand.Parameters.Add("@acconto",SqlDbType.Bit).Value = acconto;
			ModifyCommand.Parameters.Add("@saldo",SqlDbType.Bit).Value = saldo;
			ModifyCommand.Parameters.Add("@ravvedimentoOperoso",SqlDbType.Bit).Value = ravvedimentoOperoso;
			ModifyCommand.Parameters.Add("@impoTerreni",SqlDbType.Decimal).Value = impoTerreni;
			ModifyCommand.Parameters.Add("@importoAreeFabbric",SqlDbType.Decimal).Value = importoAreeFabbric;
			ModifyCommand.Parameters.Add("@importoAbitazPrincipale",SqlDbType.Decimal).Value = importoAbitazPrincipale;
			ModifyCommand.Parameters.Add("@importoAltriFabbric",SqlDbType.Decimal).Value = importoAltriFabbric;
			ModifyCommand.Parameters.Add("@detrazioneAbitazPrincipale",SqlDbType.Decimal).Value = detrazioneAbitazPrincipale;
			ModifyCommand.Parameters.Add("@contoCorrente",SqlDbType.VarChar).Value = contoCorrente;
			ModifyCommand.Parameters.Add("@comuneUbicazioneImmobile",SqlDbType.VarChar).Value = comuneUbicazioneImmobile;
			ModifyCommand.Parameters.Add("@comuneIntestatario",SqlDbType.VarChar).Value = comuneIntestatario;
			ModifyCommand.Parameters.Add("@bonificato",SqlDbType.Bit).Value = bonificato;
			ModifyCommand.Parameters.Add("@dataInizioValidità",SqlDbType.DateTime).Value = dataInizioValidità;
			ModifyCommand.Parameters.Add("@dataFineValidità",SqlDbType.DateTime).Value = dataFineValidità == DateTime.MinValue ? DBNull.Value : (object)dataFineValidità;;
			ModifyCommand.Parameters.Add("@operatore",SqlDbType.VarChar).Value = operatore;
			ModifyCommand.Parameters.Add("@annullato",SqlDbType.Bit).Value = annullato;
			ModifyCommand.Parameters.Add("@importoSoprattassa",SqlDbType.Decimal).Value = importoSoprattassa;
			ModifyCommand.Parameters.Add("@importoPenaPecuniaria",SqlDbType.Decimal).Value = importoPenaPecuniaria;
			ModifyCommand.Parameters.Add("@interessi",SqlDbType.Decimal).Value = interessi;
			ModifyCommand.Parameters.Add("@violazione",SqlDbType.Bit).Value = violazione;
			ModifyCommand.Parameters.Add("@idProvenienza",SqlDbType.Int).Value = idProvenienza;
			ModifyCommand.Parameters.Add("@dataProvvedimentoViolazione", SqlDbType.VarChar).Value = DataProvvedimentoViolazione.ToShortDateString();
			ModifyCommand.Parameters.Add("@numeroAttoAccertamento", SqlDbType.VarChar).Value = NumeroAttoAccertamento;
			ModifyCommand.Parameters.Add("@DataRiversamento",SqlDbType.DateTime).Value = MyDataRiversamento == DateTime.MinValue ? DBNull.Value : (object)MyDataRiversamento;;
			
			return Execute(ModifyCommand);
             }
                  catch (Exception Err)
                 {
                   log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.Modify.errore: ", Err);
                  throw Err;
                 }
		}

		*/
        /**** ****/

        ///// <summary>
        ///// Aggiorna un record individuato dall'identity a partire da una struttura row.
        ///// </summary>
        ///// <param name="Item"></param>
        ///// <returns>
        ///// Restituisce un valore booleano:
        ///// true: se l'operazione è terminata correttamente
        ///// false: se si sono verificati errori.
        ///// </returns>
        ///// <revisionHistory>
        ///// <revision date="30/06/2014">
        ///// <strong>TASI</strong>
        ///// </revision>
        ///// </revisionHistory>
        ///// <revisionHistory>
        ///// <revision date="12/04/2019">
        ///// <strong>Qualificazione AgID-analisi_rel01</strong>
        ///// <em>Analisi eventi</em>
        ///// </revision>
        ///// </revisionHistory>
        //public bool Modify(VersamentiRow Item)
        //{
        //    //*** 20140630 - TASI ***
        //    //return Modify(Item.ID, Item.Ente, Item.IdAnagrafico, Item.AnnoRiferimento, Item.CodiceFiscale,
        //    //    Item.PartitaIva, Item.ImportoPagato, Item.DataPagamento, Item.NumeroBollettino,
        //    //    Item.NumeroFabbricatiPosseduti, Item.Acconto, Item.Saldo, Item.RavvedimentoOperoso,
        //    //    Item.ImportoTerreni, Item.ImportoAreeFabbric, Item.ImportoAbitazPrincipale, Item.ImportoAltriFabbric, Item.DetrazioneAbitazPrincipale
        //    //    , Item.ImportoTerreniStatale, Item.ImportoAreeFabbricStatale, Item.ImportoAltriFabbricStatale, Item.ImportoFabRurUsoStrum
        //    //    , Item.ImportoFabRurUsoStrumStatale, Item.ImportoUsoProdCatD, Item.ImportoUsoProdCatDStatale
        //    //    , Item.ContoCorrente,
        //    //    Item.ComuneUbicazioneImmobile, Item.ComuneIntestatario, Item.Bonificato,
        //    //    Item.DataInizioValidità, Item.DataFineValidità, Item.Operatore, Item.Annullato,
        //    //    Item.ImportoSoprattassa, Item.ImportoPenaPecuniaria, Item.Interessi, Item.Violazione, Item.NumeroAttoAccertamento, Item.DataProvvedimentoViolazione, Item.IDProvenienza, Item.DataRiversamento);
        //    return SetVersamenti(Item, out Item.ID);
        //    //*** ****
        //}


        /// <summary>
        /// Ritorna una struttura row che rappresenta un record individuato dall'identity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Restituisce un oggetto di tipo VersamentiRow
        /// </returns>
        /// <revisionHistory>
        /// <revision date="28/08/2012">
        /// <strong>IMU adeguamento per importi statali</strong>
        /// </revision>
        /// <revisionHistory>
        /// <revision date="22/04/2013">
        /// <strong>aggiornamento IMU</strong>
        /// </revision>
        /// </revisionHistory>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="30/06/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Analisi eventi</em>
        /// </revision>
        /// </revisionHistory>
        public Utility.DichManagerICI.VersamentiRow GetRow(int id)
		{
            Utility.DichManagerICI.VersamentiRow Versamento = new Utility.DichManagerICI.VersamentiRow();
			try
			{
				SqlCommand cmdMyCommand = PrepareGetRow(id);
                DataTable Versamenti = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (Versamenti.Rows.Count > 0)
				{
					Versamento.ID = (int)Versamenti.Rows[0]["ID"];
					Versamento.Ente = (string)Versamenti.Rows[0]["Ente"];
					Versamento.IdAnagrafico = (int)Versamenti.Rows[0]["IdAnagrafico"];
                    Versamento.CodTributo = (string)Versamenti.Rows[0]["codtributo"];
					Versamento.AnnoRiferimento = (string)Versamenti.Rows[0]["AnnoRiferimento"];
					Versamento.CodiceFiscale = (string)Versamenti.Rows[0]["CodiceFiscale"];
					Versamento.PartitaIva = (string)Versamenti.Rows[0]["PartitaIva"];
					Versamento.ImportoPagato = Convert.ToDecimal(Versamenti.Rows[0]["ImportoPagato"]);
					Versamento.DataPagamento = Convert.ToDateTime(Versamenti.Rows[0]["DataPagamento"]);
					Versamento.NumeroBollettino = (string)Versamenti.Rows[0]["NumeroBollettino"];
					Versamento.NumeroFabbricatiPosseduti =  Versamenti.Rows[0]["NumeroFabbricatiPosseduti"] == DBNull.Value ? 0 : (int)Versamenti.Rows[0]["NumeroFabbricatiPosseduti"];
					Versamento.Acconto = (bool)Versamenti.Rows[0]["Acconto"];
					Versamento.Saldo = (bool)Versamenti.Rows[0]["Saldo"];
					Versamento.RavvedimentoOperoso = (bool)Versamenti.Rows[0]["RavvedimentoOperoso"];
					Versamento.ImportoTerreni = Versamenti.Rows[0]["ImpoTerreni"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImpoTerreni"]);
					Versamento.ImportoAreeFabbric = Versamenti.Rows[0]["ImportoAreeFabbric"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImportoAreeFabbric"]);
					Versamento.ImportoAbitazPrincipale = Versamenti.Rows[0]["ImportoAbitazPrincipale"] == DBNull.Value ? 0 :Convert.ToDecimal(Versamenti.Rows[0]["ImportoAbitazPrincipale"]);
					Versamento.ImportoAltrifabbric = Versamenti.Rows[0]["ImportoAltriFabbric"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImportoAltriFabbric"]);
					Versamento.DetrazioneAbitazPrincipale = Versamenti.Rows[0]["DetrazioneAbitazPrincipale"] == DBNull.Value ? 0 :Convert.ToDecimal(Versamenti.Rows[0]["DetrazioneAbitazPrincipale"]);
					Versamento.ImportoTerreniStatale = Versamenti.Rows[0]["IMPORTOTERRENISTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOTERRENISTATALE"]);
					Versamento.ImportoAreeFabbricStatale = Versamenti.Rows[0]["IMPORTOAREEFABBRICSTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOAREEFABBRICSTATALE"]);
					Versamento.ImportoAltrifabbricStatale = Versamenti.Rows[0]["IMPORTOALTRIFABBRICSTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOALTRIFABBRICSTATALE"]);
					Versamento.ImportoFabRurUsoStrum = Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUM"] == DBNull.Value ? 0 :Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUM"]);
					Versamento.ImportoFabRurUsoStrumStatale = Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUMSTATALE"] == DBNull.Value ? 0 :Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUMSTATALE"]);
					Versamento.ImportoUsoProdCatD = Versamenti.Rows[0]["IMPORTOUSOPRODCATD"] == DBNull.Value ? 0 :Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOUSOPRODCATD"]);
					Versamento.ImportoUsoProdCatDStatale = Versamenti.Rows[0]["IMPORTOUSOPRODCATDSTATALE"] == DBNull.Value ? 0 :Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOUSOPRODCATDSTATALE"]);
					Versamento.ContoCorrente = (string)Versamenti.Rows[0]["ContoCorrente"];
					Versamento.ComuneUbicazioneImmobile = (string)Versamenti.Rows[0]["ComuneUbicazioneImmobile"];
					Versamento.ComuneIntestatario = (string)Versamenti.Rows[0]["ComuneIntestatario"];
					Versamento.Bonificato = (bool)Versamenti.Rows[0]["Bonificato"];
					Versamento.DataInizioValidità = Convert.ToDateTime(Versamenti.Rows[0]["DataInizioValidità"]);
					Versamento.DataFineValidità = Versamenti.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Versamenti.Rows[0]["DataFineValidità"]);
					Versamento.Operatore = (string)Versamenti.Rows[0]["Operatore"];
					Versamento.Annullato = (bool)Versamenti.Rows[0]["Annullato"];
					Versamento.ImportoSoprattassa = Convert.ToDecimal(Versamenti.Rows[0]["ImportoSoprattassa"]);
					Versamento.ImportoPenaPecuniaria = Convert.ToDecimal(Versamenti.Rows[0]["ImportoPenaPecuniaria"]);
					Versamento.Interessi = Convert.ToDecimal(Versamenti.Rows[0]["Interessi"]);
					Versamento.Violazione = (bool)Versamenti.Rows[0]["Violazione"];
					Versamento.DataProvvedimentoViolazione = (Versamenti.Rows[0]["DataProvvedimentoViolazione"] == DBNull.Value || (string)Versamenti.Rows[0]["DataProvvedimentoViolazione"] == "") ? DateTime.MinValue  : Convert.ToDateTime(Versamenti.Rows[0]["DataProvvedimentoViolazione"]);
					Versamento.NumeroAttoAccertamento =Versamenti.Rows[0]["NumeroAttoAccertamento"]== DBNull.Value ? String.Empty  : (string)(Versamenti.Rows[0]["NumeroAttoAccertamento"]);
					Versamento.IDProvenienza = (int)Versamenti.Rows[0]["IDProvenienza"];
					Versamento.DataRiversamento = Versamenti.Rows[0]["DataRiversamento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Versamenti.Rows[0]["DataRiversamento"]);
                    Versamento.DataVariazione= Versamenti.Rows[0]["data_variazione"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(Versamenti.Rows[0]["data_variazione"]);
                }
            }
			catch(Exception ex)
			{
                //kill();
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetRow.errore: ", ex);
                Versamento = new Utility.DichManagerICI.VersamentiRow();
			}
			finally{
				//kill();
			}
			return Versamento;
		}
        //public VersamentiRow GetRow(int id)
        //{
        //    VersamentiRow Versamento = new VersamentiRow();
        //    try
        //    {
        //        SqlCommand cmdMyCommand = PrepareGetRow(id);
        //        //*** 20140630 - TASI ***
        //        //DataTable Versamenti = Query(SelectCommand)
        //        DataTable Versamenti = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
        //        if (Versamenti.Rows.Count > 0)
        //        {
        //            Versamento.ID = (int)Versamenti.Rows[0]["ID"];
        //            Versamento.Ente = (string)Versamenti.Rows[0]["Ente"];
        //            Versamento.IdAnagrafico = (int)Versamenti.Rows[0]["IdAnagrafico"];
        //            //*** 20140630 - TASI ***
        //            Versamento.CodTributo = (string)Versamenti.Rows[0]["codtributo"];
        //            //*** ***
        //            Versamento.AnnoRiferimento = (string)Versamenti.Rows[0]["AnnoRiferimento"];
        //            Versamento.CodiceFiscale = (string)Versamenti.Rows[0]["CodiceFiscale"];
        //            Versamento.PartitaIva = (string)Versamenti.Rows[0]["PartitaIva"];
        //            Versamento.ImportoPagato = Convert.ToDecimal(Versamenti.Rows[0]["ImportoPagato"]);
        //            Versamento.DataPagamento = Convert.ToDateTime(Versamenti.Rows[0]["DataPagamento"]);
        //            Versamento.NumeroBollettino = (string)Versamenti.Rows[0]["NumeroBollettino"];
        //            Versamento.NumeroFabbricatiPosseduti = Versamenti.Rows[0]["NumeroFabbricatiPosseduti"] == DBNull.Value ? 0 : (int)Versamenti.Rows[0]["NumeroFabbricatiPosseduti"];
        //            Versamento.Acconto = (bool)Versamenti.Rows[0]["Acconto"];
        //            Versamento.Saldo = (bool)Versamenti.Rows[0]["Saldo"];
        //            Versamento.RavvedimentoOperoso = (bool)Versamenti.Rows[0]["RavvedimentoOperoso"];
        //            Versamento.ImportoTerreni = Versamenti.Rows[0]["ImpoTerreni"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImpoTerreni"]);
        //            Versamento.ImportoAreeFabbric = Versamenti.Rows[0]["ImportoAreeFabbric"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImportoAreeFabbric"]);
        //            Versamento.ImportoAbitazPrincipale = Versamenti.Rows[0]["ImportoAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImportoAbitazPrincipale"]);
        //            Versamento.ImportoAltriFabbric = Versamenti.Rows[0]["ImportoAltriFabbric"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["ImportoAltriFabbric"]);
        //            Versamento.DetrazioneAbitazPrincipale = Versamenti.Rows[0]["DetrazioneAbitazPrincipale"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["DetrazioneAbitazPrincipale"]);
        //            /**** 20120828 - IMU adeguamento per importi statali ****/
        //            Versamento.ImportoTerreniStatale = Versamenti.Rows[0]["IMPORTOTERRENISTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOTERRENISTATALE"]);
        //            Versamento.ImportoAreeFabbricStatale = Versamenti.Rows[0]["IMPORTOAREEFABBRICSTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOAREEFABBRICSTATALE"]);
        //            Versamento.ImportoAltriFabbricStatale = Versamenti.Rows[0]["IMPORTOALTRIFABBRICSTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOALTRIFABBRICSTATALE"]);
        //            Versamento.ImportoFabRurUsoStrum = Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUM"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUM"]);
        //            /**** ****/
        //            /**** 20130422 - aggiornamento IMU ****/
        //            Versamento.ImportoFabRurUsoStrumStatale = Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUMSTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOFABRURUSOSTRUMSTATALE"]);
        //            Versamento.ImportoUsoProdCatD = Versamenti.Rows[0]["IMPORTOUSOPRODCATD"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOUSOPRODCATD"]);
        //            Versamento.ImportoUsoProdCatDStatale = Versamenti.Rows[0]["IMPORTOUSOPRODCATDSTATALE"] == DBNull.Value ? 0 : Convert.ToDecimal(Versamenti.Rows[0]["IMPORTOUSOPRODCATDSTATALE"]);
        //            /**** ****/
        //            Versamento.ContoCorrente = (string)Versamenti.Rows[0]["ContoCorrente"];
        //            Versamento.ComuneUbicazioneImmobile = (string)Versamenti.Rows[0]["ComuneUbicazioneImmobile"];
        //            Versamento.ComuneIntestatario = (string)Versamenti.Rows[0]["ComuneIntestatario"];
        //            Versamento.Bonificato = (bool)Versamenti.Rows[0]["Bonificato"];
        //            Versamento.DataInizioValidità = Convert.ToDateTime(Versamenti.Rows[0]["DataInizioValidità"]);
        //            Versamento.DataFineValidità = Versamenti.Rows[0]["DataFineValidità"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Versamenti.Rows[0]["DataFineValidità"]);
        //            Versamento.Operatore = (string)Versamenti.Rows[0]["Operatore"];
        //            Versamento.Annullato = (bool)Versamenti.Rows[0]["Annullato"];
        //            Versamento.ImportoSoprattassa = Convert.ToDecimal(Versamenti.Rows[0]["ImportoSoprattassa"]);
        //            Versamento.ImportoPenaPecuniaria = Convert.ToDecimal(Versamenti.Rows[0]["ImportoPenaPecuniaria"]);
        //            Versamento.Interessi = Convert.ToDecimal(Versamenti.Rows[0]["Interessi"]);
        //            Versamento.Violazione = (bool)Versamenti.Rows[0]["Violazione"];
        //            //DIPE
        //            //Versamento.DataProvvedimentoViolazione = (string)Versamenti.Rows[0]["DataProvvedimentoViolazione"] == String.Empty ? DateTime.MinValue : Convert.ToDateTime(Versamenti.Rows[0]["DataProvvedimentoViolazione"]);
        //            Versamento.DataProvvedimentoViolazione = (Versamenti.Rows[0]["DataProvvedimentoViolazione"] == DBNull.Value || (string)Versamenti.Rows[0]["DataProvvedimentoViolazione"] == "") ? DateTime.MinValue : Convert.ToDateTime(Versamenti.Rows[0]["DataProvvedimentoViolazione"]);
        //            //Versamento.NumeroAttoAccertamento = (string)(Versamenti.Rows[0]["NumeroAttoAccertamento"]);
        //            Versamento.NumeroAttoAccertamento = Versamenti.Rows[0]["NumeroAttoAccertamento"] == DBNull.Value ? String.Empty : (string)(Versamenti.Rows[0]["NumeroAttoAccertamento"]);
        //            Versamento.IDProvenienza = (int)Versamenti.Rows[0]["IDProvenienza"];
        //            Versamento.DataRiversamento = Versamenti.Rows[0]["DataRiversamento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Versamenti.Rows[0]["DataRiversamento"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //kill();
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetRow.errore: ", ex);
        //        Versamento = new VersamentiRow();
        //    }
        //    finally
        //    {
        //        //kill();
        //    }
        //    return Versamento;
        //}


        /// <summary>
        /// Esegue la cancellazione logica dei dati.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Restituisce un valore booleano:
        /// true: se l'operazione è terminata correttamente
        /// false: se si sono verificati errori.
        /// </returns>
        public bool CancellazioneLogica(int id)
		{
			SqlCommand DeleteCommand = new SqlCommand();
            try { 
			DeleteCommand.CommandText = "UPDATE " + this.TableName + " SET " +
				"Annullato=1 WHERE ID=@id";

			DeleteCommand.Parameters.Add("@id",SqlDbType.Int).Value = id;
            //*** 20140630 - TASI ***
            //return (Execute(DeleteCommand);
            return Execute(DeleteCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
                //*** ***
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.CancellazioneLogica.errore: ", Err);
                throw Err;
            }
        }

		
		
		/// <summary>
		/// Torna una DataView con tutti i versamenti in saldo che filtrato per codice contribuente e anno di riferimento.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <param name="idAnagrafico"></param>
		/// <returns>
		/// Restituisce un DataView
		/// </returns>
		public DataView GetVersamenti(string ente,
			string annoRiferimento, int idAnagrafico)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE Ente=@ente" +
				" AND IdAnagrafico=@idAnagrafico";// AND Annullato<>1";

			if(annoRiferimento != String.Empty)
			{
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
				SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			}

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			//kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetVersamenti.errore: ", Err);
                throw Err;
            }
        }


        /// <summary>
        /// Torna tutti i versamenti in base alla tipologia di versamento
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="annoRiferimento"></param>
        /// <param name="idAnagrafico"></param>
        /// <param name="bAcconto"></param>
        /// <param name="bSaldo"></param>
        /// <param name="bIsViolazione"></param>
        /// <param name="Tributo"></param>
        /// <returns>
        /// Restituisce un DataView
        /// </returns>
        public DataView GetVersamentiPerTipologia(string ente,string annoRiferimento, int idAnagrafico, bool bAcconto, bool bSaldo,bool bIsViolazione, string Tributo)
        {
            SqlCommand SelectCommand = new SqlCommand();
            try { 
            SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
                " WHERE Ente=@ente" +
                " AND IdAnagrafico=@idAnagrafico" +// AND Annullato<>1" + 
                " AND Acconto=@Acconto AND Saldo=@Saldo" +
                " AND VIOLAZIONE=@Violazione" +
                " AND CODTRIBUTO=@TRIBUTO";
            if (annoRiferimento != String.Empty)
            {
                SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
                SelectCommand.Parameters.Add("@annoRiferimento", SqlDbType.VarChar).Value = annoRiferimento;
            }

            SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
            SelectCommand.Parameters.Add("@idAnagrafico", SqlDbType.Int).Value = idAnagrafico;

            SelectCommand.Parameters.Add("@Acconto", SqlDbType.Bit).Value = bAcconto;
            SelectCommand.Parameters.Add("@Saldo", SqlDbType.Bit).Value = bSaldo;
            SelectCommand.Parameters.Add("@Violazione", SqlDbType.Bit).Value = bIsViolazione;
            SelectCommand.Parameters.Add("@TRIBUTO", SqlDbType.VarChar).Value = Tributo;
            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
            //kill();
            return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetVersamentiPeTipologia.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// Torna tutti i versamenti in base all'anno, all'ente e all'idAnagrafico passato
        /// </summary>
        /// <param name="ente"></param>
        /// <param name="annoRiferimento"></param>
        /// <param name="idAnagrafico"></param>
        /// <returns>
        /// Restituisce un DataView
        /// </returns>
        public DataView GetVersamentiPerInformativa(string ente, string annoRiferimento,int idAnagrafico)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Acconto, Saldo, SUM(ImportoPagato) AS SUMImportoPagato, SUM(ImpoTerreni) AS SUMImpoTerreni, SUM(ImportoAreeFabbric) AS SUMImportoAreeFabbric, " +
			" SUM(ImportoAbitazPrincipale) AS SUMImportoAbitazPrincipale, SUM(ImportoAltriFabbric) AS SUMImportoAltriFabbric, " +
            " SUM(DetrazioneAbitazPrincipale) AS SUMDetrazioneAbitazPrincipale " +
			" FROM TblVersamenti " +
			" GROUP BY Ente, IdAnagrafico, AnnoRiferimento, Acconto, Saldo, Annullato " +
            " HAVING Ente=@ente AND IdAnagrafico =@idAnagrafico";// AND Annullato<>1 ";

			if(annoRiferimento != String.Empty)
			{
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
				SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			}

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			//kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetVersamentiPerInformativa.errore: ", Err);
                throw Err;
            }

        }

		
		/// <summary>
		/// Restituisce gli importi totali dei versamenti per l'informativa, in base ai parametri passati.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <param name="idAnagrafico"></param>
		/// <returns>
		/// Restituisce un DataView
		/// </returns>
		public DataView GetImportiTotaliVersamentiPerInformativa(string ente, string annoRiferimento,int idAnagrafico)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT SUM(ImportoPagato) AS SUMImportoPagato, SUM(ImpoTerreni) AS SUMImpoTerreni, SUM(ImportoAreeFabbric) AS SUMImportoAreeFabbric, " +
				" SUM(ImportoAbitazPrincipale) AS SUMImportoAbitazPrincipale, SUM(ImportoAltriFabbric) AS SUMImportoAltriFabbric, " +
				" SUM(DetrazioneAbitazPrincipale) AS SUMDetrazioneAbitazPrincipale " +
				" FROM TblVersamenti " +
				" GROUP BY Ente, IdAnagrafico, AnnoRiferimento, Annullato " +
                " HAVING Ente=@ente AND IdAnagrafico =@idAnagrafico";// AND Annullato<>1 ";

			if(annoRiferimento != String.Empty)
			{
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
				SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			}

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			//kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetImportiTotaliVersamentiPerInformativa.errore: ", Err);
                throw Err;
            }
        }

		
		/// <summary>
		/// Restituisce la data di pagamento del versamento per l'informativa
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <param name="idAnagrafico"></param>
		/// <returns>Restituisce un DataView</returns>
		public DataView GetDataVersamentoPerInformativa(string ente, string annoRiferimento,int idAnagrafico)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT Acconto, Saldo,  MAX(DataPagamento) as MaxData" +
					" FROM TblVersamenti " +
					" GROUP BY Ente, IdAnagrafico, AnnoRiferimento, Acconto, Saldo, Annullato " +
                    " HAVING Ente=@ente AND IdAnagrafico =@idAnagrafico";// AND Annullato<>1 ";

			if(annoRiferimento != String.Empty)
			{
				SelectCommand.CommandText += " AND AnnoRiferimento=@annoRiferimento";
				SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			}

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@idAnagrafico",SqlDbType.Int).Value = idAnagrafico;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			//kill();
			return dv;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.GetDataVersamentoPerInformativa.errore: ", Err);
                throw Err;
            }

        }

		
		/// <summary>
		/// Esegue la bonifica del versamento selezionato.
		/// </summary>
		/// <param name="idVersamento"></param>
		/// <returns>
		/// Restituisce un valore booleano:
		/// true: se l'operazione è terminata correttamente
		/// false: se si sono verificati errori.
		/// </returns>
		public bool Bonifica(int idVersamento)
		{
			SqlCommand cmdMyCommand = new SqlCommand();
            try { 
			cmdMyCommand.CommandText = "UPDATE " + this.TableName + " SET Bonificato=1" +
				" WHERE ID=@idVersamento";

			cmdMyCommand.Parameters.Add("@idVersamento",SqlDbType.Int).Value = idVersamento;
            //*** 20140630 - TASI ***
			//return Execute(ModifyCommand);
            return Execute(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiTable.Bonifica.errore: ", Err);
                throw Err;
            }
        }
	}
}
