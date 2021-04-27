using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione della vista viewVersamenti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class VersamentiView : Database
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(VersamentiView));
        /// <summary>
        /// 
        /// </summary>
  //      public VersamentiView()
		//{
		//	this.TableName = "viewVersamenti";
		//}

		/// <summary>
		/// Torna una DataView con la lista dei versamenti filtrati per cognome, nome,
		/// codice fiscale e/o partita Iva.
		/// </summary>
		/// <param name="cognome"></param>
		/// <param name="nome"></param>
		/// <param name="codiceFiscale"></param>
		/// <param name="partitaIVA"></param>
		/// <param name="anno"></param>
		/// <param name="ente"></param>
		/// <returns>Restituisce un DataView</returns>
		//public DataView List(string cognome, string nome, string codiceFiscale, string partitaIVA, string anno, string ente)
		//{
		//	SqlCommand SelectCommand = new SqlCommand();

  //          try { 
		//	SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
		//		" WHERE (Cognome LIKE @cognome) AND (Nome LIKE @nome) AND (CodiceFiscale" +
		//		" LIKE @codiceFiscale) AND (PartitaIva LIKE @partitaIVA) AND (AnnoRiferimento" +
  //              " LIKE @anno) AND (Ente LIKE @ente";//) AND Annullato<>1";
			
		//	SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = cognome + "%";
		//	SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome + "%";
		//	SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale + "%";
		//	SelectCommand.Parameters.Add("@partitaIVA", SqlDbType.VarChar).Value = partitaIVA + "%";
		//	SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno + "%";
		//	SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente + "%";

  //          DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
		//	kill();
		//	return dv;
  //          }
  //          catch (Exception Err)
  //          {
  //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiView.List.errore: ", Err);
  //              throw Err;
  //          }
  //      }

        /// <summary>
        /// Torna una DataView con la lista dei versamenti filtrati.
        /// </summary>
        /// <param name="myStringConnection">string</param>
        /// <param name="Tributo"></param>
        /// <param name="IdContrib"></param>
        /// <param name="cognome"></param>
        /// <param name="nome"></param>
        /// <param name="codiceFiscale"></param>
        /// <param name="partitaIVA"></param>
        /// <param name="anno"></param>
        /// <param name="ente"></param>
        /// <param name="TipoPagamento"></param>
        /// <param name="Tipologia"></param>
        /// <param name="IdConfrontoImporto"></param>
        /// <param name="ImportoP"></param>
        /// <param name="DataRiversamentoDa"></param>
        /// <param name="DataRiversamentoA"></param>
        /// <param name="Flusso"></param>
        /// <returns>Restituisce un dataView</returns>
        /// <revisionHistory>
        /// <revision date="30/06/2014">
        /// <strong>TASI</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public DataView List(string myStringConnection,string Tributo,int IdContrib, string cognome, string nome, string codiceFiscale, string partitaIVA, string anno, string ente, int TipoPagamento, int Tipologia, int IdConfrontoImporto, double ImportoP, string DataRiversamentoDa, string DataRiversamentoA,string Flusso)
        {
            DataView myDataView = new DataView();
            try
            {
                string sSQL = string.Empty;
                using (DBModel ctx = new DBModel(Business.ConstWrapper.DBType, Business.ConstWrapper.StringConnection))
                {
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetVersamenti", "ENTE", "ANNO", "IDCONTRIBUENTE", "COGNOME", "NOME", "CODICEFISCALE", "PARTITAIVA", "TRIBUTO", "TIPO","TIPOLOGIA", "CONFRONTOIMPORTO", "IMPORTO", "DAL", "AL","FLUSSO");
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ENTE", ente)
                            , ctx.GetParam("ANNO", anno)
                            , ctx.GetParam("IDCONTRIBUENTE", IdContrib)
                            , ctx.GetParam("COGNOME", cognome.Replace("*", "%") + "%")
                            , ctx.GetParam("NOME", nome.Replace("*", "%") + "%")
                            , ctx.GetParam("CODICEFISCALE", codiceFiscale + "%")
                            , ctx.GetParam("PARTITAIVA", partitaIVA + "%")
                            , ctx.GetParam("TRIBUTO", Tributo)
                            , ctx.GetParam("TIPO", TipoPagamento)
                            , ctx.GetParam("TIPOLOGIA", Tipologia)
                            , ctx.GetParam("CONFRONTOIMPORTO", IdConfrontoImporto)
                            , ctx.GetParam("IMPORTO", ImportoP)
                            , ctx.GetParam("DAL", Utility.StringOperation.FormatDateTime(DataRiversamentoDa))
                            , ctx.GetParam("AL", Utility.StringOperation.FormatDateTime(DataRiversamentoA))
                            , ctx.GetParam("FLUSSO", Flusso)
                        );
                    ctx.Dispose();
                }
            }
            catch (Exception Err)
            {
                log.Debug(ente + " - DichiarazioniICI.VersamentiView.List.errore: ", Err);
                throw Err;
            }
            return myDataView;
        }
        //public DataView List(string Tributo, int IdContrib, string cognome, string nome, string codiceFiscale, string partitaIVA, string anno, string ente, int TipoPagamento, int Tipologia, int IdConforntoImporto, double ImportoP, string DataRiversamentoDa, string DataRiversamentoA)
        //{

        //    SqlCommand cmdMyCommand = new SqlCommand();
        //    try
        //    {
        //        cmdMyCommand.CommandText = "SELECT * FROM " + this.TableName;
        //        cmdMyCommand.CommandText += " WHERE 1=1"; //and ANNULLATO<>1";
        //        cmdMyCommand.CommandText += " AND (ENTE=@ente)";
        //        cmdMyCommand.CommandText += " AND (@anno='' OR ANNORIFERIMENTO=@anno)";
        //        cmdMyCommand.CommandText += " AND (COD_CONTRIBUENTE=@IdContribuente OR (@IdContribuente<=0 AND";
        //        cmdMyCommand.CommandText += " (@cognome='' OR COGNOME LIKE @cognome +'%')";
        //        cmdMyCommand.CommandText += " AND (@nome='' OR NOME LIKE @nome +'%')";
        //        cmdMyCommand.CommandText += " AND (@codiceFiscale='' OR CODICEFISCALE LIKE @codiceFiscale)";
        //        cmdMyCommand.CommandText += " AND (@partitaIVA='' OR PARTITAIVA LIKE @partitaIVA)))";
        //        //*** 20140630 - TASI ***
        //        cmdMyCommand.CommandText += " AND (@TRIBUTO='' OR CODTRIBUTO=@TRIBUTO)";
        //        //*** ***
        //        if (TipoPagamento != -1)
        //        {   // Se è stato selezionato il tipo Pagamento
        //            cmdMyCommand.CommandText += " AND (acconto LIKE @acconto) AND (saldo LIKE @saldo)";
        //        }
        //        if (Tipologia != -1)
        //        {   // se è stata selezionata la tipologia
        //            cmdMyCommand.CommandText += " AND (RavvedimentoOperoso LIKE @ravvedimentoOperoso) AND (Violazione LIKE @violazione)";
        //        }
        //        if (IdConforntoImporto != -1)
        //        {    // se è stato selezionato il confronto di importo
        //            if (IdConforntoImporto == 0)
        //            {
        //                cmdMyCommand.CommandText += " AND (ImportoPagato > @ImportoP)";
        //            }
        //            if (IdConforntoImporto == 1)
        //            {
        //                cmdMyCommand.CommandText += " AND (ImportoPagato < @ImportoP)";
        //            }
        //            if (IdConforntoImporto == 2)
        //            {
        //                cmdMyCommand.CommandText += " AND (ImportoPagato = @ImportoP)";
        //            }
        //        }
        //        if (DataRiversamentoDa != string.Empty)
        //        {
        //            cmdMyCommand.CommandText += " AND (DATARIVERSAMENTO >= @DataRiversamentoDa AND DATARIVERSAMENTO <= @DataRiversamentoA)";
        //        }
        //        cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CODICEFISCALE, PARTITAIVA, ANNORIFERIMENTO DESC, CODTRIBUTO, DATAPAGAMENTO";

        //        cmdMyCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
        //        cmdMyCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
        //        cmdMyCommand.Parameters.Add("@IdContribuente", SqlDbType.Int).Value = IdContrib;
        //        //*** 20140630 - TASI ***
        //        cmdMyCommand.Parameters.Add("@TRIBUTO", SqlDbType.VarChar).Value = Tributo;
        //        //*** ***
        //        cmdMyCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = cognome.Replace("*", "%");
        //        cmdMyCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = nome.Replace("*", "%");
        //        cmdMyCommand.Parameters.Add("@codiceFiscale", SqlDbType.VarChar).Value = codiceFiscale.Replace("*", "%");
        //        cmdMyCommand.Parameters.Add("@partitaIVA", SqlDbType.VarChar).Value = partitaIVA.Replace("*", "%");
        //        if (TipoPagamento == 0)  // Acconto
        //        {
        //            cmdMyCommand.Parameters.Add("@acconto", SqlDbType.Bit).Value = 1;
        //            cmdMyCommand.Parameters.Add("@saldo", SqlDbType.Bit).Value = 0;
        //        }
        //        if (TipoPagamento == 1)  // Saldo
        //        {
        //            cmdMyCommand.Parameters.Add("@acconto", SqlDbType.Bit).Value = 0;
        //            cmdMyCommand.Parameters.Add("@saldo", SqlDbType.Bit).Value = 1;
        //        }
        //        if (TipoPagamento == 2)  // Unica soluzione
        //        {
        //            cmdMyCommand.Parameters.Add("@acconto", SqlDbType.Bit).Value = 1;
        //            cmdMyCommand.Parameters.Add("@saldo", SqlDbType.Bit).Value = 1;
        //        }
        //        if (Tipologia == 0)  // Ordinario
        //        {
        //            cmdMyCommand.Parameters.Add("@ravvedimentoOperoso", SqlDbType.Bit).Value = 0;
        //            cmdMyCommand.Parameters.Add("@violazione", SqlDbType.Bit).Value = 0;
        //        }
        //        if (Tipologia == 1)  // Ravvedimento Operoso
        //        {
        //            cmdMyCommand.Parameters.Add("@ravvedimentoOperoso", SqlDbType.Bit).Value = 1;
        //            cmdMyCommand.Parameters.Add("@violazione", SqlDbType.Bit).Value = 0;
        //        }
        //        if (Tipologia == 2)  // Violazione
        //        {
        //            cmdMyCommand.Parameters.Add("@ravvedimentoOperoso", SqlDbType.Bit).Value = 0;
        //            cmdMyCommand.Parameters.Add("@violazione", SqlDbType.Bit).Value = 1;
        //        }

        //        cmdMyCommand.Parameters.Add("@ImportoP", SqlDbType.Decimal).Value = ImportoP;
        //        if (DataRiversamentoDa != string.Empty)
        //        {
        //            cmdMyCommand.Parameters.Add("@DataRiversamentoDa", SqlDbType.DateTime).Value = DataRiversamentoDa;
        //            cmdMyCommand.Parameters.Add("@DataRiversamentoA", SqlDbType.DateTime).Value = DataRiversamentoA;
        //        }
        //        //*** 20140630 - TASI ***
        //        //DataView dv=Query(SelectCommand).DefaultView;
        //        DataView dv = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
        //        kill();
        //        return dv;
        //    }
        //    catch (Exception Err)
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiView.List.errore: ", Err);
        //        throw Err;
        //    }
        //}


        /// <summary>
        /// Restituisce tutti i versamenti in base all'anno e al codice contribuente passato.
        /// </summary>
        /// <param name="CodContribuente">Codice Contribuente per il quale si vogliono ottenere i versamenti.</param>
        /// <param name="AnnoRiferimento">Anno di Riferimento per il quale si vogliono ottenere i versamenti. Non Obbligatorio</param>
        /// <returns>Restituisce un DataView</returns>
  //      public DataView List(string CodContribuente, int AnnoRiferimento)
		//{
		//	SqlCommand SelectCmd = new SqlCommand();
  //          try { 
		//	if (CodContribuente != string.Empty){
				
		//		SelectCmd.CommandText = "Select * from " + this.TableName;
		//		SelectCmd.CommandText += " WHERE Cod_Contribuente = @CodContribuente" ;

		//		SelectCmd.Parameters.Add("@CodContribuente", SqlDbType.NVarChar).Value = CodContribuente;

		//		// se è presente l'anno di riferimento filtro anche per quello. 
		//		if (AnnoRiferimento != 0)
		//		{
		//			SelectCmd.CommandText +=  " and AnnoRiferimento = @AnnoRiferimento";
		//			SelectCmd.Parameters.Add("@AnnoRiferimento", SqlDbType.Int).Value=AnnoRiferimento;
		//		}
		//	}

  //          DataView dv = Query(SelectCmd, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
		//	kill();
		//	return dv;
  //          }
  //          catch (Exception Err)
  //          {
  //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiView.List.errore: ", Err);
  //              throw Err;
  //          }
  //      }

	
		/// <summary>
		/// Restituisce tutti gli anni di riferimento per i quali sono presenti versamenti.
		/// </summary>
		/// <param name="CodiceEnte"></param>
		/// <returns>Restituisce un DataView</returns>
		//public DataView GetAnniVersamenti(string CodiceEnte){
			
		//	SqlCommand SelectCommand = new SqlCommand();
  //          try { 
		//	SelectCommand.CommandText = "SELECT DISTINCT AnnoRiferimento FROM " + this.TableName + " WHERE Ente = @Ente";
		//	SelectCommand.CommandText += " AND IDProvenienza <> 100 ";
		//	SelectCommand.CommandText += " order by AnnoRiferimento desc";

		//	SelectCommand.Parameters.Add("@Ente", SqlDbType.NVarChar).Value = CodiceEnte;

		//	DataView dv=Query(SelectCommand,new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
		//	kill();
		//	return dv;
  //          }
  //          catch (Exception Err)
  //          {
  //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiView.GetAnniVersamenti.errore: ", Err);
  //              throw Err;
  //          }

  //      }


		/// <summary>
		/// ListOrdinari - estrae tutti i versamenti ordinari per il contribuente in questione.
		/// </summary>
		/// <param name="CodContribuente"></param>
		/// <param name="AnnoRiferimento"></param>
		/// <returns>Restituisce un DataView</returns>
		//public DataView ListOrdinari(string CodContribuente, int AnnoRiferimento)
		//{
		//	SqlCommand SelectCmd = new SqlCommand();
  //          try { 
		//	if (CodContribuente != string.Empty)
		//	{
				
		//		SelectCmd.CommandText = "Select * from " + this.TableName;
		//		SelectCmd.CommandText += " WHERE Cod_Contribuente = @CodContribuente" ;

		//		SelectCmd.Parameters.Add("@CodContribuente", SqlDbType.NVarChar).Value = CodContribuente;

		//		// se è presente l'anno di riferimento filtro anche per quello. 
		//		if (AnnoRiferimento != 0)
		//		{
		//			SelectCmd.CommandText +=  " and AnnoRiferimento = @AnnoRiferimento";
		//			SelectCmd.Parameters.Add("@AnnoRiferimento", SqlDbType.Int).Value=AnnoRiferimento;
		//		}

		//		// imposto il filtro per escudere i versamenti da violazione e ravvedimento operoso
		//		SelectCmd.CommandText += " AND RavvedimentoOperoso = 0 AND Violazione = 0";
		//	}

  //          DataView dv = Query(SelectCmd, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
		//	kill();
		//	return dv;
  //          }
  //          catch (Exception Err)
  //          {
  //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiView.ListOrdinari.errore: ", Err);
  //              throw Err;
  //          }
  //      }


		/// <summary>
		/// ListOrdinari - estrae tutti i versamenti ordinari per il contribuente in questione.
		/// </summary>
		/// <param name="AnnoRiferimento"></param>
		/// <returns>Restituisce un DataView</returns>
		//public DataView ListVersCompensativi(int AnnoRiferimento)
		//{
		//	SqlCommand SelectCmd = new SqlCommand();
  //          try { 
		//	SelectCmd.CommandText = "Select * from ViewVersamenti where idProvenienza = 100 and ImportoPagato > 0 ";

		//	// se è presente l'anno di riferimento filtro anche per quello. 
		//	if (AnnoRiferimento != 0)
		//	{
		//		SelectCmd.CommandText +=  " and AnnoRiferimento = @AnnoRiferimento";
		//		SelectCmd.Parameters.Add("@AnnoRiferimento", SqlDbType.Int).Value=AnnoRiferimento;
		//	}

		//	SelectCmd.CommandText += " order by AnnoRiferimento, DataPagamento ";
  //          DataView dv = Query(SelectCmd, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
		//	kill();
		//	return dv;
  //          }
  //          catch (Exception Err)
  //          {
  //              log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.VersamentiView.ListVersCompensativi.errore: ", Err);
  //              throw Err;
  //          }
  //      }

	}

	

}
