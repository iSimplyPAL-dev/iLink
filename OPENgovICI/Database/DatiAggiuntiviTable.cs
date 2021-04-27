using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{

	/// <summary>
	/// Struttura dati Aggiuntivi
	/// </summary>
	public struct DatiAggiuntiviRow{
		public int Idtestata;
		public int IdOggetto;
		public string AnnoFine;
		public string NoteContribuente;
		public string NoteIci;
		public string N_Progressivo;
		public string N_Sogg_Ici;
		public string PercDetraz;
		public string Vani94;
		public string Zona94;
		public string _DovFarCorrCorrStCalc;
		public string _FlagDetPrimaCasa;
		public string C_Fiscale;
		public string C_FISC_DICH_CONG;
		public string Ente;
		public string RenditaDominicale;
		public string CategoriaCatasto;
		public string ClasseCatasto;
		public string Consistenza;
		public double ImportoIciImmobile;
		public double DetrazioneCalcolata;
		public double NumeroAbitanti;
		public string FlagConduzioneDiretta;
		public string FlagTipoBeneDichiar93;
		public string GruppoDetrazionePerAbitazionePrincipale;
		//public string CodFiscaleContribuente;
		public string RenditaCalcolata;
		public string Qualità;
		public string EstremiTitoloAcquisto;
		public string EstremiTitoloCessione;
		public string EstremiTitoloPossesso;
		public string FlagImmobileStorico;
		public string FlagPossesso3112;
		public string FlagRiduzione;
		public string FlagAbitazionePrincipale;
		//public string NumeroRecord;
		public string TipoPrimario;
		public string AttributoEdificabilità;
	}

	/// <summary>
	/// Classe DatiAggiuntivi
	/// </summary>
	public class DatiAggiuntiviTable : Database
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(DatiAggiuntiviTable));

        private string _username;

		/// <summary>
		/// Costruttore della classe
		/// </summary>
		/// <param name="UserName"></param>
		public DatiAggiuntiviTable(string UserName)
		{
			this._username = UserName;
			this.TableName = "DATI_AGGIUNTIVI";
		}

		/// <summary>
		/// Ritorna un oggetto DatiAggiuntiviRow
		/// </summary>
		/// <param name="IdOggetto">Parametro di tipo <c>int</c> che identifica l'Immobile</param>
		/// <returns>Ritorna un oggetto DatiAggiuntiviRow</returns>
		public DatiAggiuntiviRow GetDatiAggiuntivi(int IdOggetto){
			
			DatiAggiuntiviRow Details = new DatiAggiuntiviRow();
			
			try
			{
				
				SqlCommand SelectRowCmd;
				SelectRowCmd = new SqlCommand();
				SelectRowCmd.CommandText = "Select * from " + this.TableName + " where IdOggetto = @IdOggetto";
		
				SelectRowCmd.Parameters.Add("@IdOggetto", SqlDbType.Int).Value = IdOggetto;

                DataTable DatiAggiuntivi = Query(SelectRowCmd, new SqlConnection(Business.ConstWrapper.StringConnection));

				if (DatiAggiuntivi.Rows.Count > 0)
				{
					Details._DovFarCorrCorrStCalc = DatiAggiuntivi.Rows[0]["_DovFabCorrCorrStCalc"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["_DovFabCorrCorrStCalc"];
					Details._FlagDetPrimaCasa = DatiAggiuntivi.Rows[0]["_FlagDetPrimaCasa"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["_FlagDetPrimaCasa"];
					Details.AnnoFine=DatiAggiuntivi.Rows[0]["AnnoFine"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["AnnoFine"];
					Details.AttributoEdificabilità = DatiAggiuntivi.Rows[0]["AttributoEdificabilita"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["AttributoEdificabilita"];
					Details.C_FISC_DICH_CONG=DatiAggiuntivi.Rows[0]["C_FISC_DICH_CONG"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["C_FISC_DICH_CONG"];
					Details.C_Fiscale = DatiAggiuntivi.Rows[0]["C_Fiscale"] == DBNull.Value ? "" :(string)DatiAggiuntivi.Rows[0]["C_Fiscale"];
					Details.ClasseCatasto = DatiAggiuntivi.Rows[0]["ClasseCatasto"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["ClasseCatasto"];
					Details.CategoriaCatasto = DatiAggiuntivi.Rows[0]["CategoriaCatasto"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["CategoriaCatasto"];
					Details.Consistenza = DatiAggiuntivi.Rows[0]["Consistenza"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["Consistenza"];
					Details.DetrazioneCalcolata = (((string)DatiAggiuntivi.Rows[0]["DetrazioneCalcolata"] == "")||(DatiAggiuntivi.Rows[0]["DetrazioneCalcolata"] == DBNull.Value)) ? 0 : double.Parse((string)DatiAggiuntivi.Rows[0]["DetrazioneCalcolata"]);
					Details.Ente = DatiAggiuntivi.Rows[0]["Ente"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["Ente"];
					Details.FlagConduzioneDiretta = DatiAggiuntivi.Rows[0]["FlagConduzioneDiretta"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["FlagConduzioneDiretta"];
					Details.FlagImmobileStorico = DatiAggiuntivi.Rows[0]["Consistenza"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["FlagImmobileStorico"];
					Details.FlagTipoBeneDichiar93 = DatiAggiuntivi.Rows[0]["FlagTipoBeneDichiar93"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["FlagTipoBeneDichiar93"];
					Details.GruppoDetrazionePerAbitazionePrincipale = DatiAggiuntivi.Rows[0]["GruppoDetrazionePerAbitazionePrincipale"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["GruppoDetrazionePerAbitazionePrincipale"];
					Details.IdOggetto = int.Parse((string)DatiAggiuntivi.Rows[0]["IdOggetto"]);
					Details.Idtestata = (int)DatiAggiuntivi.Rows[0]["Idtestata"];
					Details.ImportoIciImmobile = ((string)DatiAggiuntivi.Rows[0]["ImportoIciImmobile"] == "") || (DatiAggiuntivi.Rows[0]["ImportoIciImmobile"] == DBNull.Value) ? 0 : (double)DatiAggiuntivi.Rows[0]["ImportoIciImmobile"];
					Details.N_Progressivo = DatiAggiuntivi.Rows[0]["N_Progressivo"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["N_Progressivo"];
					Details.N_Sogg_Ici = DatiAggiuntivi.Rows[0]["N_Sogg_Ici"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["N_Sogg_Ici"];
					Details.NoteContribuente = DatiAggiuntivi.Rows[0]["NoteContribuente"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["NoteContribuente"];
					Details.NoteIci = DatiAggiuntivi.Rows[0]["NoteIci"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["NoteIci"];
					Details.NumeroAbitanti = (string)DatiAggiuntivi.Rows[0]["NumeroAbitanti"] == "" ? 0 : (double)DatiAggiuntivi.Rows[0]["NumeroAbitanti"];
					//Details.NumeroRecord = (string)DatiAggiuntivi.Rows[0]["NumeroRecord"];
					Details.PercDetraz = DatiAggiuntivi.Rows[0]["PercDetraz"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["PercDetraz"];
					Details.Qualità = (string)DatiAggiuntivi.Rows[0]["Qualita"];
					Details.RenditaCalcolata = DatiAggiuntivi.Rows[0]["RenditaCalcolata"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["RenditaCalcolata"];
					Details.RenditaDominicale = DatiAggiuntivi.Rows[0]["RenditaDominicale"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["RenditaDominicale"];
					Details.TipoPrimario = DatiAggiuntivi.Rows[0]["TipoPrimario"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["TipoPrimario"];
					Details.Vani94 = DatiAggiuntivi.Rows[0]["Vani94"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["Vani94"];
					Details.Zona94 = DatiAggiuntivi.Rows[0]["Zona94"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["Zona94"];
					Details.EstremiTitoloAcquisto = DatiAggiuntivi.Rows[0]["EstremiTitoloAcquisto"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["EstremiTitoloAcquisto"];
					Details.EstremiTitoloCessione = DatiAggiuntivi.Rows[0]["EstremiTitoloCessione"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["EstremiTitoloCessione"];
					Details.FlagAbitazionePrincipale = DatiAggiuntivi.Rows[0]["FlagAbitazPrincipale"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["FlagAbitazPrincipale"];
					Details.FlagRiduzione = DatiAggiuntivi.Rows[0]["FlagRiduzione"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["FlagRiduzione"];
					//Details.EstremiTitoloPossesso = DatiAggiuntivi.Rows[0]["EstremiTitoloPossesso"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["EstremiTitoloPossesso"];
					Details.FlagPossesso3112 = DatiAggiuntivi.Rows[0]["FlagPossesso3112"] == DBNull.Value ? "" : (string)DatiAggiuntivi.Rows[0]["FlagPossesso3112"];
				}
			}
			catch(Exception Err)
			{
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DatiAggiuntiviTable.GetDatiAggiuntivi.errore: ", Err);
                kill();
				Details = new DatiAggiuntiviRow();	
			}
			finally{
				kill();
			}
			
			return Details;

			}
	}

}