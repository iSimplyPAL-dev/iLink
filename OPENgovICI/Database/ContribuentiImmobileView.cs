using System;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe di gestione della vista viewContribuentiImmobile.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ContribuentiImmobileView : Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContribuentiImmobileView));
        /// <summary>
        /// Costruttore della classe
        /// </summary>
        public ContribuentiImmobileView()
        {
            this.TableName = "viewContribuentiImmobile";
        }

        /*public DataTable ListContribuenti(string partitaCatastale, string foglio, string numero, string subalterno,
			string sezione, string caratteristica, string anno, string protocollo, string via,
			string categoria, string classe, string ente, Bonificato bonificato)
		{
			string sSQL = string.Empty;
			string sSQL= string.Empty;
			SqlCommand SelectCommand = new SqlCommand();
			string NomeDbAnag= ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();
            try{
			//SelectCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE 1 = 1 ";

			SelectCommand.CommandText += " SELECT " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AS CodContribuente, ";
			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE AS Cognome, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME, ";
			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE AS CodiceFiscale, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA AS PartitaIva, ";
			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.DATA_NASCITA AS DataNascita, ";// + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE, ";
			SelectCommand.CommandText += " TblOggetti.Foglio as foglio, TblOggetti.Numero as Numero, TblOggetti.Subalterno as Subalterno,  ";
			//SelectCommand.CommandText += " TblOggetti.id as idOggetto ";
			
			//			SelectCommand.CommandText += " FROM dbo.TblClasse RIGHT OUTER JOIN dbo.TblTestata INNER JOIN ";
			//            SelectCommand.CommandText += " dbo.TblOggetti ON dbo.TblTestata.ID = dbo.TblOggetti.IdTestata INNER JOIN ";
			//            SelectCommand.CommandText += " dbo.TblDettaglioTestata ON dbo.TblTestata.ID = dbo.TblDettaglioTestata.IdTestata AND ";
			//            SelectCommand.CommandText += " dbo.TblOggetti.ID = dbo.TblDettaglioTestata.IdOggetto INNER JOIN ";
			//            SelectCommand.CommandText += " Open_Anagrafica.dbo.ANAGRAFICA ON dbo.TblTestata.IDContribuente = Open_Anagrafica.dbo.ANAGRAFICA.COD_CONTRIBUENTE AND ";
			//            SelectCommand.CommandText += " dbo.TblTestata.Ente = Open_Anagrafica.dbo.ANAGRAFICA.COD_ENTE LEFT OUTER JOIN ";
			//            SelectCommand.CommandText += " dbo.TblCategoriaCatastale ON dbo.TblOggetti.CodCategoriaCatastale = dbo.TblCategoriaCatastale.CategoriaCatastale ON ";
			//            SelectCommand.CommandText += " dbo.TblClasse.Classe = dbo.TblOggetti.CodClasse ";

			SelectCommand.CommandText += " FROM dbo.TblClasse RIGHT OUTER JOIN dbo.TblTestata INNER JOIN ";
			SelectCommand.CommandText += " dbo.TblOggetti ON dbo.TblTestata.ID = dbo.TblOggetti.IdTestata INNER JOIN ";
			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA ON dbo.TblTestata.IDContribuente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AND ";
			SelectCommand.CommandText += " dbo.TblTestata.Ente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE LEFT OUTER JOIN ";
			SelectCommand.CommandText += " dbo.TblCategoriaCatastale ON dbo.TblOggetti.CodCategoriaCatastale = dbo.TblCategoriaCatastale.CategoriaCatastale ON ";
			SelectCommand.CommandText += " dbo.TblClasse.Classe = dbo.TblOggetti.CodClasse ";

			
			//SelectCommand.CommandText += " WHERE (TblDettaglioTestata.IdSoggetto = 0) AND (Open_Anagrafica.dbo.ANAGRAFICA.DATA_FINE_VALIDITA IS NULL or Open_Anagrafica.dbo.ANAGRAFICA.DATA_FINE_VALIDITA='') ";
			SelectCommand.CommandText += " WHERE (" + NomeDbAnag + ".dbo.ANAGRAFICA.DATA_FINE_VALIDITA IS NULL or " + NomeDbAnag + ".dbo.ANAGRAFICA.DATA_FINE_VALIDITA='') ";
			SelectCommand.CommandText += " and (dbo.TblTestata.Annullato <> 1) and (dbo.TblTestata.Ente= " + ente +") ";

			//			SelectCommand.CommandText = "SELECT * FROM " + this.TableName ;
			//			SelectCommand.CommandText +=  " WHERE 1 = 1 ";
			
			if (partitaCatastale != string.Empty) 
			{
				SelectCommand.CommandText += " AND (TblOggetti.PartitaCatastale =@partitaCatastale)";
				SelectCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale;						
			}
			if (foglio != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.Foglio =@foglio) ";
				SelectCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio;						
			}
			if (numero != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.Numero =@numero)";
				SelectCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;
			}
			if (subalterno != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.Subalterno =@subalterno) ";
				SelectCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno;
			}
			if (sezione != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.Sezione = @sezione)";
				SelectCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione;
			}
			if (caratteristica != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.Caratteristica =@caratteristica) ";
				SelectCommand.Parameters.Add("@caratteristica", SqlDbType.VarChar).Value = caratteristica;
			}
			if (anno != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.AnnoDenunciaCatastale =@anno) ";
				SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
			}
			if (protocollo != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.NumeroProtCatastale =@protocollo)";
				SelectCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo;
			}
			if (via != string.Empty)
			{
				SelectCommand.CommandText += " AND (TblOggetti.Via LIKE @via) ";
				SelectCommand.Parameters.Add("@via", SqlDbType.VarChar).Value = via + "%";
			}
			if (categoria.CompareTo("-1")!=0)
			{
				SelectCommand.CommandText += " AND (TblCategoriaCatastale.CategoriaCatastale =@categoria) ";
				SelectCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
			}
			if (classe.CompareTo("-1")!=0)
			{ 
				SelectCommand.CommandText += " AND (TblClasse.Classe =@classe) ";
				SelectCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
			}
			//			if (ente != string.Empty){
			//				SelectCommand.CommandText += " AND (Ente = @ente)";
			//				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
			//			}	

			switch(bonificato)
			{
				case Bonificato.Bonificate:
					SelectCommand.CommandText += " AND Bonificato=1";
					break;

				case Bonificato.DaBonificare:
					SelectCommand.CommandText += " AND Bonificato=0";
					break;
			}

			SelectCommand.CommandText += " GROUP BY " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE, " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, ";
			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.NOME, " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA, ";
			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.DATA_NASCITA, " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE, ";
			SelectCommand.CommandText += "TblOggetti.Foglio, TblOggetti.Numero, TblOggetti.Subalterno, TblOggetti.id ";
			SelectCommand.CommandText += " ORDER BY " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME ";

			sSQL="SELECT * FROM " + this.TableName + " WHERE ";
			//SelectCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale;
			sSQL+= "(PartitaCatastale LIKE '" + partitaCatastale +"%') ";			
			//SelectCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio;
			sSQL+= " AND (Foglio LIKE '" + foglio + "%') " ;				
			//SelectCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;
			sSQL+= " AND (Numero LIKE '"+numero+"%')";
			//SelectCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno;
			sSQL+= " AND (Subalterno LIKE '"+subalterno+"%') "; 
			//SelectCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione;
			sSQL+= " AND (Sezione LIKE '"+sezione+"%')";
			//SelectCommand.Parameters.Add("@caratteristica", SqlDbType.VarChar).Value = caratteristica;
			sSQL+= " AND (Caratteristica LIKE '"+ caratteristica + "%')"; 
			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
			sSQL+= " and (AnnoDenunciaCatastale LIKE '" + anno + "%') ";
			//SelectCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo;
			sSQL+= " AND (NumeroProtCatastale LIKE '"+protocollo+"%') ";
			//SelectCommand.Parameters.Add("@via", SqlDbType.VarChar).Value = via + "%";
			sSQL+= " AND (Via LIKE'"+via+"%')";
			//SelectCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
			sSQL+= " AND (CategoriaCatastale LIKE '"+categoria+"%')";
			//SelectCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
			sSQL+= " AND (Classe LIKE '"+classe+"%')";
			//SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
			sSQL+= " AND (Ente LIKE '"+ente+"%')";

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribuentiImmobileView.ListContribuenti.errore: ", Err);
            }
		}*/

        /// <summary>
        /// Restituisce una DataTable valorizzata con l'elenco dei contribuenti filtrati per immobile con l'aggiunta di parametri di ricerca avanzata
        /// (Percentuale possesso e valore immobile) e possibilità di ricerca immobili non agganciati a stradario.
        /// </summary>
        /// <param name="myStringConnection">string</param>
        /// <param name="Ambiente">string</param>
        /// <param name="sListContrib"></param>
        /// <param name="ProvenienzaDich"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="partitaCatastale">Parametro di tipo <c>String</c> che identifica la Partita Catastale</param>
        /// <param name="foglio">Parametro di tipo <c>String</c> che identifica il Foglio</param>
        /// <param name="numero">Parametro di tipo <c>String</c> che identifica il Numero</param>
        /// <param name="subalterno">Parametro di tipo <c>String</c> che identifica il Subalterno</param>
        /// <param name="sezione">Parametro di tipo <c>String</c> che identifica la Sezione</param>
        /// <param name="caratteristica">Parametro di tipo <c>String</c> che identifica la Caratteristica</param>
        /// <param name="anno">Parametro di tipo <c>String</c> che identifica l'Anno</param>
        /// <param name="protocollo">Parametro di tipo <c>String</c> che identifica il Protocollo</param>
        /// <param name="via">Parametro di tipo <c>String</c> che identifica la Via</param>
        /// <param name="categoria">Parametro di tipo <c>String</c> che identifica la Categoria</param>
        /// <param name="classe">Parametro di tipo <c>String</c> che identifica la Classe</param>
        /// <param name="ente">Parametro di tipo <c>String</c> che identifica l'Ente</param>
        /// <param name="bonificato">Parametro di tipo <c>Bonificato</c> che identifica se l'immobile è stato bonificato</param>
        /// <param name="percPos">Parametro di tipo <c>float</c> che identifica la Percentuale di Possesso</param>
        /// <param name="ImportoP">Parametro di tipo <c>double</c> che identifica il Valore dell'Immobile</param>
        /// <param name="IdConfrontoImporto">Parametro di tipo <c>int</c> che identifica se è stato selezionato il confronto di importo</param>
        /// <param name="NoStradario"></param>
        /// <param name="AbiPrinc"></param>
        /// <param name="IsPertinenza"></param>
        /// <param name="TipoUtilizzo"></param>
        /// <param name="TipoPossesso"></param>
        /// <param name="Coltivatori"></param>
        /// <returns>Restituisce una DataTable valorizzata</returns>
        /// <revisionHistory>
        /// <revision date="23/09/2014">
        /// <strong>GIS</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="11/2015">
        /// <strong>Funzioni Sovracomunali</strong>
        /// </revision>
        /// </revisionHistory>
        /// <revisionHistory>
        /// <revision date="12/04/2019">
        /// <strong>Qualificazione AgID-analisi_rel01</strong>
        /// <em>Esportazione completa dati</em>
        /// </revision>
        /// </revisionHistory>
        public DataTable ListContribuenti(string myStringConnection,string Ambiente, string sListContrib, int ProvenienzaDich, string Dal, string Al, string partitaCatastale, string foglio, string numero, string subalterno, string sezione, int caratteristica, string anno, string protocollo, string via, string categoria, string classe, string ente, Bonificato bonificato, double percPos, double ImportoP, int IdConfrontoImporto, int NoStradario, int AbiPrinc, int IsPertinenza, int TipoUtilizzo, int TipoPossesso, int Coltivatori)
        {
            DataTable dt = null;

            try
            {
                SqlCommand cmdMyCommand = new SqlCommand();
                cmdMyCommand.CommandType = CommandType.StoredProcedure;
                cmdMyCommand.CommandText = "prc_GetDichFromUI";
                cmdMyCommand.Parameters.Add("@IdEnte", SqlDbType.VarChar).Value = ente;
                cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.VarChar).Value = Ambiente;
                cmdMyCommand.Parameters.Add("@ProvenienzaDich", SqlDbType.Int).Value = ProvenienzaDich;
                if (Dal != string.Empty)
                {
                    cmdMyCommand.Parameters.Add("@Dal", SqlDbType.DateTime).Value = DateTime.Parse(Dal);
                }
                else
                {
                    cmdMyCommand.Parameters.Add("@Dal", SqlDbType.DateTime).Value = DateTime.MaxValue;
                }
                if (Al != string.Empty)
                {
                    cmdMyCommand.Parameters.Add("@Al", SqlDbType.DateTime).Value = DateTime.Parse(Al);
                }
                else
                {
                    cmdMyCommand.Parameters.Add("@Al", SqlDbType.DateTime).Value = DateTime.MaxValue;
                }
                switch (bonificato)
                {
                    case Bonificato.Bonificate:
                        cmdMyCommand.Parameters.Add("@Bonificate", SqlDbType.Int).Value = 1;
                        break;

                    case Bonificato.DaBonificare:
                        cmdMyCommand.Parameters.Add("@Bonificate", SqlDbType.Int).Value = 0;
                        break;
                }
                cmdMyCommand.Parameters.Add("@via", SqlDbType.VarChar).Value = via.Replace("*", "%");
                cmdMyCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio;
                cmdMyCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;
                cmdMyCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno;
                cmdMyCommand.Parameters.Add("@caratteristica", SqlDbType.Int).Value = caratteristica;
                cmdMyCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
                cmdMyCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
                cmdMyCommand.Parameters.Add("@AbiPrinc", SqlDbType.Int).Value = AbiPrinc;
                cmdMyCommand.Parameters.Add("@IsPertinenza", SqlDbType.Int).Value = IsPertinenza;
                cmdMyCommand.Parameters.Add("@TipoUtilizzo", SqlDbType.Int).Value = TipoUtilizzo;
                cmdMyCommand.Parameters.Add("@TipoPossesso", SqlDbType.Int).Value = TipoPossesso;
                cmdMyCommand.Parameters.Add("@percPos", SqlDbType.Real).Value = percPos;
                cmdMyCommand.Parameters.Add("@IdConfrontoImporto", SqlDbType.Int).Value = IdConfrontoImporto;
                cmdMyCommand.Parameters.Add("@ImportoP", SqlDbType.Decimal).Value = ImportoP;
                cmdMyCommand.Parameters.Add("@Coltivatori", SqlDbType.Int).Value = Coltivatori;
                cmdMyCommand.Parameters.Add("@ImmobiliNoAgg", SqlDbType.Int).Value = NoStradario;
                cmdMyCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale;
                cmdMyCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione;
                cmdMyCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo;
                cmdMyCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
                cmdMyCommand.Parameters.Add("@ListContrib", SqlDbType.VarChar).Value = sListContrib;
                dt = Query(cmdMyCommand, new SqlConnection(myStringConnection));
            }
            catch (Exception Err)
            {
                log.Debug(ente + " - DichiarazioniICI.ContribuentiImmobileView.ListContribuenti.errore: ", Err);
            }
            finally
            {
                kill();
            }
            return dt;
        }
    }
    //public DataTable ListContribuenti(string sListContrib, int ProvenienzaDich, string Dal, string Al, string partitaCatastale, string foglio, string numero, string subalterno,
    //    string sezione, int caratteristica, string anno, string protocollo, string via,
    //    string categoria, string classe, string ente, Bonificato bonificato, double percPos,
    //    double ImportoP, int IdConfrontoImporto, int NoStradario, int AbiPrinc, int IsPertinenza, int TipoUtilizzo, int TipoPossesso, int Coltivatori)
    //{
    //    //public DataTable ListContribuenti(string sListContrib,string Dal,string Al,string partitaCatastale, string foglio, string numero, string subalterno,
    //    //    string sezione, string caratteristica, string anno, string protocollo, string via,
    //    //    string categoria, string classe, string ente, Bonificato bonificato, float percPos, 
    //    //    double ImportoP, int IdConfrontoImporto, string ImmobileNonAgg)
    //    //{
    //    //			string sSQL = string.Empty;
    //    //			string sSQL= string.Empty;
    //    SqlCommand cmdMyCommand = new SqlCommand();
    //    //			string NomeDbAnag= ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();
    //    //try{
    //    //SelectCommand.CommandText = "SELECT * FROM " + this.TableName + " WHERE 1 = 1 ";

    //    //			SelectCommand.CommandText += "SELECT " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE AS Cognome, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME, ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE AS CodiceFiscale, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA AS PartitaIva, ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.DATA_NASCITA AS DataNascita, ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AS CodContribuente, " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE, ";
    //    //			SelectCommand.CommandText += " TblOggetti.Foglio as foglio, TblOggetti.Numero as Numero, TblOggetti.Subalterno as Subalterno, ";
    //    //			SelectCommand.CommandText += " TblClasse.Classe as Classe, TblCategoriaCatastale.CategoriaCatastale as Categoria ";
    //    //			//SelectCommand.CommandText += ,TblDettaglioTestata.IdOggetto ";
    //    //			
    //    //			//			SelectCommand.CommandText += " FROM dbo.TblClasse RIGHT OUTER JOIN dbo.TblTestata INNER JOIN ";
    //    //			//            SelectCommand.CommandText += " dbo.TblOggetti ON dbo.TblTestata.ID = dbo.TblOggetti.IdTestata INNER JOIN ";
    //    //			//            SelectCommand.CommandText += " dbo.TblDettaglioTestata ON dbo.TblTestata.ID = dbo.TblDettaglioTestata.IdTestata AND ";
    //    //			//            SelectCommand.CommandText += " dbo.TblOggetti.ID = dbo.TblDettaglioTestata.IdOggetto INNER JOIN ";
    //    //			//            SelectCommand.CommandText += " Open_Anagrafica.dbo.ANAGRAFICA ON dbo.TblTestata.IDContribuente = Open_Anagrafica.dbo.ANAGRAFICA.COD_CONTRIBUENTE AND ";
    //    //			//            SelectCommand.CommandText += " dbo.TblTestata.Ente = Open_Anagrafica.dbo.ANAGRAFICA.COD_ENTE LEFT OUTER JOIN ";
    //    //			//            SelectCommand.CommandText += " dbo.TblCategoriaCatastale ON dbo.TblOggetti.CodCategoriaCatastale = dbo.TblCategoriaCatastale.CategoriaCatastale ON ";
    //    //			//            SelectCommand.CommandText += " dbo.TblClasse.Classe = dbo.TblOggetti.CodClasse ";
    //    //
    //    //		/*	SelectCommand.CommandText += " FROM dbo.TblClasse RIGHT OUTER JOIN dbo.TblTestata INNER JOIN ";
    //    //			SelectCommand.CommandText += " dbo.TblOggetti ON dbo.TblTestata.ID = dbo.TblOggetti.IdTestata INNER JOIN ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA ON dbo.TblTestata.IDContribuente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AND ";
    //    //			SelectCommand.CommandText += " dbo.TblTestata.Ente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE LEFT OUTER JOIN ";
    //    //			SelectCommand.CommandText += " dbo.TblCategoriaCatastale ON dbo.TblOggetti.CodCategoriaCatastale = dbo.TblCategoriaCatastale.CategoriaCatastale ON ";
    //    //			SelectCommand.CommandText += " dbo.TblClasse.Classe = dbo.TblOggetti.CodClasse ";*/
    //    //
    //    //
    //    //			SelectCommand.CommandText +=" FROM TblCategoriaCatastale ";
    //    //			SelectCommand.CommandText +=" RIGHT OUTER JOIN TblTestata ";
    //    //			SelectCommand.CommandText +=" INNER JOIN TblOggetti ";
    //    //			SelectCommand.CommandText +=" ON TblTestata.ID = TblOggetti.IdTestata ";
    //    //			SelectCommand.CommandText +=" INNER JOIN TblDettaglioTestata ";
    //    //			SelectCommand.CommandText +=" ON TblOggetti.ID = TblDettaglioTestata.IdOggetto ";
    //    //			SelectCommand.CommandText +=" INNER JOIN " + NomeDbAnag + ".dbo.ANAGRAFICA ";
    //    //			SelectCommand.CommandText +=" ON TblDettaglioTestata.Ente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE ";
    //    //			SelectCommand.CommandText +=" AND TblDettaglioTestata.IdSoggetto = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE ";
    //    //			SelectCommand.CommandText +=" ON TblCategoriaCatastale.CategoriaCatastale = TblOggetti.CodCategoriaCatastale ";
    //    //			SelectCommand.CommandText +=" LEFT OUTER JOIN TblClasse ";
    //    //			SelectCommand.CommandText +=" ON TblOggetti.CodClasse = TblClasse.Classe";
    //    //
    //    //
    //    //*
    //    //			SelectCommand.CommandText +=" FROM TblCategoriaCatastale RIGHT OUTER JOIN ";
    //    //			SelectCommand.CommandText +=" TblTestata INNER JOIN ";
    //    //			SelectCommand.CommandText +=" TblOggetti ON TblTestata.ID = TblOggetti.IdTestata INNER JOIN ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA ON TblTestata.IDContribuente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE AND ";
    //    //			SelectCommand.CommandText +=" TblTestata.Ente = " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE INNER JOIN ";
    //    //			SelectCommand.CommandText +=" TblDettaglioTestata ON TblOggetti.ID = TblDettaglioTestata.IdOggetto ON ";
    //    //			SelectCommand.CommandText +=" TblCategoriaCatastale.CategoriaCatastale = TblOggetti.CodCategoriaCatastale LEFT OUTER JOIN ";
    //    //			SelectCommand.CommandText +=" TblClasse ON TblOggetti.CodClasse = TblClasse.Classe ";
    //    //*/
    //    //			
    //    //			//SelectCommand.CommandText += " WHERE (TblDettaglioTestata.IdSoggetto = 0) AND (Open_Anagrafica.dbo.ANAGRAFICA.DATA_FINE_VALIDITA IS NULL or Open_Anagrafica.dbo.ANAGRAFICA.DATA_FINE_VALIDITA='') ";
    //    //			SelectCommand.CommandText += " WHERE (" + NomeDbAnag + ".dbo.ANAGRAFICA.DATA_FINE_VALIDITA IS NULL or " + NomeDbAnag + ".dbo.ANAGRAFICA.DATA_FINE_VALIDITA='') ";
    //    //			SelectCommand.CommandText += " and (dbo.TblTestata.Annullato <> 1) and (dbo.TblTestata.Ente= " + ente +") ";
    //    //
    //    //			//			SelectCommand.CommandText = "SELECT * FROM " + this.TableName ;
    //    //			//			SelectCommand.CommandText +=  " WHERE 1 = 1 ";
    //    //			
    //    //			if (partitaCatastale != string.Empty) 
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.PartitaCatastale =@partitaCatastale)";
    //    //				SelectCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale;						
    //    //			}
    //    //			if (foglio != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.Foglio =@foglio) ";
    //    //				SelectCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio;						
    //    //			}
    //    //			if (numero != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.Numero =@numero)";
    //    //				SelectCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;
    //    //			}
    //    //			if (subalterno != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.Subalterno =@subalterno) ";
    //    //				SelectCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno;
    //    //			}
    //    //			if (sezione != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.Sezione = @sezione)";
    //    //				SelectCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione;
    //    //			}
    //    //			if (caratteristica != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.Caratteristica =@caratteristica) ";
    //    //				SelectCommand.Parameters.Add("@caratteristica", SqlDbType.VarChar).Value = caratteristica;
    //    //			}
    //    //			if (anno != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.AnnoDenunciaCatastale =@anno) ";
    //    //				SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
    //    //			}
    //    //			if (protocollo != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.NumeroProtCatastale =@protocollo)";
    //    //				SelectCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo;
    //    //			}
    //    //			if (via != string.Empty)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblOggetti.Via LIKE @via) ";
    //    //				SelectCommand.Parameters.Add("@via", SqlDbType.VarChar).Value = via + "%";
    //    //			}
    //    //			if (categoria.CompareTo("-1")!=0)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblCategoriaCatastale.CategoriaCatastale =@categoria) ";
    //    //				SelectCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
    //    //			}
    //    //			if (classe.CompareTo("-1")!=0)
    //    //			{ 
    //    //				SelectCommand.CommandText += " AND (TblClasse.Classe =@classe) ";
    //    //				SelectCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
    //    //			}
    //    //
    //    //			if (IdConfrontoImporto != -1)
    //    //			{    // se è stato selezionato il confronto di importo
    //    //				if (IdConfrontoImporto == 0)
    //    //				{
    //    //					SelectCommand.CommandText += " AND (ValoreImmobile > @ImportoP)";					
    //    //				}
    //    //				if (IdConfrontoImporto == 1)
    //    //				{
    //    //					SelectCommand.CommandText += " AND (ValoreImmobile < @ImportoP)";
    //    //				}
    //    //				if (IdConfrontoImporto == 2)
    //    //				{
    //    //					SelectCommand.CommandText += " AND (ValoreImmobile = @ImportoP)";
    //    //				}
    //    //				SelectCommand.Parameters.Add("@ImportoP", SqlDbType.Decimal).Value = ImportoP;
    //    //			}
    //    //
    //    //			if (percPos != 0)
    //    //			{
    //    //				SelectCommand.CommandText += " AND (TblDettaglioTestata.PercPossesso LIKE @percPos) ";
    //    //				SelectCommand.Parameters.Add("@percPos", SqlDbType.Real).Value = percPos;
    //    //			}
    //    //
    //    //			//			if (ente != string.Empty){
    //    //			//				SelectCommand.CommandText += " AND (Ente = @ente)";
    //    //			//				SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
    //    //			//			}	
    //    //
    //    //			if (ImmobileNonAgg =="1")
    //    //				SelectCommand.CommandText += " and ((TblOggetti.CodVia IS NULL) OR (TblOggetti.CodVia = '0') OR (TblOggetti.CodVia = '-1') OR (TblOggetti.CodVia = '')) ";
    //    //
    //    //			switch(bonificato)
    //    //			{
    //    //				case Bonificato.Bonificate:
    //    //					SelectCommand.CommandText += " AND TblOggetti.Bonificato=1";
    //    //					break;
    //    //
    //    //				case Bonificato.DaBonificare:
    //    //					SelectCommand.CommandText += " AND TblOggetti.Bonificato=0";
    //    //					break;
    //    //			}
    //    //
    //    //			SelectCommand.CommandText += " GROUP BY " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_CONTRIBUENTE, " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.NOME, " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_FISCALE, " + NomeDbAnag + ".dbo.ANAGRAFICA.PARTITA_IVA, ";
    //    //			SelectCommand.CommandText += NomeDbAnag + ".dbo.ANAGRAFICA.DATA_NASCITA, " + NomeDbAnag + ".dbo.ANAGRAFICA.COD_ENTE, ";
    //    //			SelectCommand.CommandText += "TblOggetti.Foglio, TblOggetti.Numero, TblOggetti.Subalterno,";
    //    //			SelectCommand.CommandText += " TblClasse.Classe, TblCategoriaCatastale.CategoriaCatastale ";
    //    //			//SelectCommand.CommandText += " ,TblDettaglioTestata.IdOggetto ";
    //    //			//SelectCommand.CommandText += " ORDER BY " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME";
    //    //			SelectCommand.CommandText += " ORDER BY " + NomeDbAnag + ".dbo.ANAGRAFICA.COGNOME_DENOMINAZIONE, " + NomeDbAnag + ".dbo.ANAGRAFICA.NOME, ";
    //    //			SelectCommand.CommandText += "TblOggetti.Foglio, TblOggetti.Numero, TblOggetti.Subalterno";
    //    cmdMyCommand.CommandType = CommandType.StoredProcedure;
    //    cmdMyCommand.CommandText = "prc_GetDichFromUI";
    //    cmdMyCommand.Parameters.Add("@IdEnte", SqlDbType.VarChar).Value = ente;
    //    //*** 201511 - Funzioni Sovracomunali ***
    //    cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.VarChar).Value = Business.ConstWrapper.Ambiente;
    //    //*** ***
    //    cmdMyCommand.Parameters.Add("@ProvenienzaDich", SqlDbType.Int).Value = ProvenienzaDich;
    //    if (Dal != string.Empty)
    //    {
    //        cmdMyCommand.Parameters.Add("@Dal", SqlDbType.DateTime).Value = DateTime.Parse(Dal);
    //    }
    //    else
    //    {
    //        cmdMyCommand.Parameters.Add("@Dal", SqlDbType.DateTime).Value = DateTime.MaxValue;
    //    }
    //    if (Al != string.Empty)
    //    {
    //        cmdMyCommand.Parameters.Add("@Al", SqlDbType.DateTime).Value = DateTime.Parse(Al);
    //    }
    //    else
    //    {
    //        cmdMyCommand.Parameters.Add("@Al", SqlDbType.DateTime).Value = DateTime.MaxValue;
    //    }
    //    switch (bonificato)
    //    {
    //        case Bonificato.Bonificate:
    //            cmdMyCommand.Parameters.Add("@Bonificate", SqlDbType.Int).Value = 1;
    //            break;

    //        case Bonificato.DaBonificare:
    //            cmdMyCommand.Parameters.Add("@Bonificate", SqlDbType.Int).Value = 0;
    //            break;
    //    }
    //    cmdMyCommand.Parameters.Add("@via", SqlDbType.VarChar).Value = via.Replace("*", "%");
    //    cmdMyCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio;
    //    cmdMyCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;
    //    cmdMyCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno;
    //    cmdMyCommand.Parameters.Add("@caratteristica", SqlDbType.Int).Value = caratteristica;
    //    cmdMyCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
    //    cmdMyCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
    //    cmdMyCommand.Parameters.Add("@AbiPrinc", SqlDbType.Int).Value = AbiPrinc;
    //    cmdMyCommand.Parameters.Add("@IsPertinenza", SqlDbType.Int).Value = IsPertinenza;
    //    cmdMyCommand.Parameters.Add("@TipoUtilizzo", SqlDbType.Int).Value = TipoUtilizzo;
    //    cmdMyCommand.Parameters.Add("@TipoPossesso", SqlDbType.Int).Value = TipoPossesso;
    //    cmdMyCommand.Parameters.Add("@percPos", SqlDbType.Real).Value = percPos;
    //    cmdMyCommand.Parameters.Add("@IdConfrontoImporto", SqlDbType.Int).Value = IdConfrontoImporto;
    //    cmdMyCommand.Parameters.Add("@ImportoP", SqlDbType.Decimal).Value = ImportoP;
    //    cmdMyCommand.Parameters.Add("@Coltivatori", SqlDbType.Int).Value = Coltivatori;
    //    cmdMyCommand.Parameters.Add("@ImmobiliNoAgg", SqlDbType.Int).Value = NoStradario;
    //    cmdMyCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale;
    //    cmdMyCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione;
    //    cmdMyCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo;
    //    cmdMyCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
    //    cmdMyCommand.Parameters.Add("@ListContrib", SqlDbType.VarChar).Value = sListContrib;

    //    //			sSQL="SELECT * FROM " + this.TableName + " WHERE ";
    //    //			//SelectCommand.Parameters.Add("@partitaCatastale", SqlDbType.VarChar).Value = partitaCatastale;
    //    //			sSQL+= "(PartitaCatastale LIKE '" + partitaCatastale +"%') ";			
    //    //			//SelectCommand.Parameters.Add("@foglio", SqlDbType.VarChar).Value = foglio;
    //    //			sSQL+= " AND (Foglio LIKE '" + foglio + "%') " ;				
    //    //			//SelectCommand.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;
    //    //			sSQL+= " AND (Numero LIKE '"+numero+"%')";
    //    //			//SelectCommand.Parameters.Add("@subalterno", SqlDbType.VarChar).Value = subalterno;
    //    //			sSQL+= " AND (Subalterno LIKE '"+subalterno+"%') "; 
    //    //			//SelectCommand.Parameters.Add("@sezione", SqlDbType.VarChar).Value = sezione;
    //    //			sSQL+= " AND (Sezione LIKE '"+sezione+"%')";
    //    //			//SelectCommand.Parameters.Add("@caratteristica", SqlDbType.VarChar).Value = caratteristica;
    //    //			sSQL+= " AND (Caratteristica LIKE '"+ caratteristica + "%')"; 
    //    //			//SelectCommand.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno;
    //    //			sSQL+= " and (AnnoDenunciaCatastale LIKE '" + anno + "%') ";
    //    //			//SelectCommand.Parameters.Add("@protocollo", SqlDbType.VarChar).Value = protocollo;
    //    //			sSQL+= " AND (NumeroProtCatastale LIKE '"+protocollo+"%') ";
    //    //			//SelectCommand.Parameters.Add("@via", SqlDbType.VarChar).Value = via + "%";
    //    //			sSQL+= " AND (Via LIKE'"+via+"%')";
    //    //			//SelectCommand.Parameters.Add("@categoria", SqlDbType.VarChar).Value = categoria;
    //    //			sSQL+= " AND (CategoriaCatastale LIKE '"+categoria+"%')";
    //    //			//SelectCommand.Parameters.Add("@classe", SqlDbType.VarChar).Value = classe;
    //    //			sSQL+= " AND (Classe LIKE '"+classe+"%')";
    //    //			//SelectCommand.Parameters.Add("@ente", SqlDbType.VarChar).Value = ente;
    //    //			sSQL+= " AND (Ente LIKE '"+ente+"%')";

    //    //DataTable dt= Query(SelectCommand);
    //    DataTable dt = Query(cmdMyCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
    //    kill();
    //    return dt;
    //    //  }
    //    // catch (Exception Err)
    //    // {
    //    //    log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.ContribuentiImmobileView.ListContribuenti.errore: ", Err);
    //    // }
    //}
}