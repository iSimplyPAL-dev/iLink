using log4net;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DichiarazioniICI.Database
{
	/// <summary>
	/// 
	/// </summary>
	public enum Questionario
	{
		Tutte = -1,
		ConQuestionario,
		SenzaQuestionario
	}

	/// <summary>
	/// Rappresenta una riga della tabella viewDichiarazioniErrate.
	/// </summary>
	public struct DichiarazioniErrateRow 
	{
        
		public int ID;
		public string Ente;
		public int NumeroDichiarazione;
		public string AnnoDichiarazione;
		public string NumeroProtocollo;
		public System.DateTime DataProtocollo;
		public string TotaleModelli;
		public System.DateTime DataInizio;
		public System.DateTime DataFine;
		public int IDContribuente;
		public int IDDenunciante;
		public bool Bonificato;
		public bool Annullato;
		public System.DateTime DataInizioValidità;
		public System.DateTime DataFineValidità;
		public string Operatore;
		public bool Storico;
		public int IdErrore;
		public int IDQuestionario;
	}
    
	/// <summary>
	/// Classe di gestione della vista viewDichiarazioniErrate.
	/// </summary>
	public class DichiarazioniErrateView : Database 
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(DichiarazioniErrateView));
        private string _username;
        
		public DichiarazioniErrateView(string UserName) 
		{
			this.TableName = "viewDichiarazioniErrate";
			_username = UserName;
		}

		/// <summary>
		/// Dichiarazione errate apparteneti ad un anno e ente con codice d'errore specifico
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <param name="idErrore"></param>
		/// <returns></returns>
		public DataTable List(string ente, string annoRiferimento, int idErrore)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE Ente=@ente AND Annullato<>1 AND Storico<>1";

			if(annoRiferimento != "0")
				SelectCommand.CommandText += " AND AnnoDichiarazione=@annoRiferimento";
			if(idErrore > 0)
				SelectCommand.CommandText += " AND IDErrore=@idErrore";

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			SelectCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;

            DataTable dt = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
			kill();
			return dt;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DichiarazioniErrateView.List.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Torna una DataView con l'elenco delle dichiarazioni errate filtrate
		/// per ente, anno di riferimento, errore e questionario.
		/// </summary>
		/// <param name="ente"></param>
		/// <param name="annoRiferimento"></param>
		/// <param name="idErrore"></param>
		/// <param name="questionario"></param>
		/// <returns></returns>
		public DataView List(string ente, string annoRiferimento, int idErrore, Questionario questionario)
		{
			SqlCommand SelectCommand = new SqlCommand();
			SelectCommand.CommandText = "SELECT * FROM " + this.TableName +
				" WHERE Ente=@ente AND Annullato<>1 AND Storico<>1";
            try { 
			if(annoRiferimento != "0")
				SelectCommand.CommandText += " AND AnnoDichiarazione=@annoRiferimento";
			if(idErrore > 0)
				SelectCommand.CommandText += " AND IDErrore=@idErrore";

			switch(questionario)
			{
				case Questionario.ConQuestionario:
					SelectCommand.CommandText += " AND IDQuestionario<>0";
					break;

				case Questionario.SenzaQuestionario:
					SelectCommand.CommandText += " AND IDQuestionario=0";
					break;
			}

			SelectCommand.CommandText += " ORDER BY COGNOME_DENOMINAZIONE, NOME, ANNODICHIARAZIONE";

			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;
			SelectCommand.Parameters.Add("@annoRiferimento",SqlDbType.VarChar).Value = annoRiferimento;
			SelectCommand.Parameters.Add("@idErrore",SqlDbType.Int).Value = idErrore;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DichiarazioniErrateView.List.errore: ", Err);
                throw Err;
            }
        }
	
		/// <summary>
		/// Ritorna una riga identificata dall'identity.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DichiarazioniErrateRow GetRow(int id) 
		{
			DichiarazioniErrateRow riga = new DichiarazioniErrateRow();
			SqlCommand SelectCommand = PrepareGetRow(id);
            DataTable tabella = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
            try { 
			if (tabella.Rows.Count > 0) 
			{
				riga.ID = (System.Int32)tabella.Rows[0]["ID"];
				riga.Ente = (System.String)tabella.Rows[0]["Ente"];
				riga.NumeroDichiarazione = (System.Int32)tabella.Rows[0]["NumeroDichiarazione"];
				riga.AnnoDichiarazione = (System.String)tabella.Rows[0]["AnnoDichiarazione"];
				riga.NumeroProtocollo = (System.String)tabella.Rows[0]["NumeroProtocollo"];
				riga.DataProtocollo = (System.DateTime)tabella.Rows[0]["DataProtocollo"];
				riga.TotaleModelli = (System.String)tabella.Rows[0]["TotaleModelli"];
				riga.DataInizio = (System.DateTime)tabella.Rows[0]["DataInizio"];
				riga.DataFine = (System.DateTime)tabella.Rows[0]["DataFine"];
				riga.IDContribuente = (System.Int32)tabella.Rows[0]["IDContribuente"];
				riga.IDDenunciante = (System.Int32)tabella.Rows[0]["IDDenunciante"];
				riga.Bonificato = (System.Boolean)tabella.Rows[0]["Bonificato"];
				riga.Annullato = (System.Boolean)tabella.Rows[0]["Annullato"];
				riga.DataInizioValidità = (System.DateTime)tabella.Rows[0]["DataInizioValidità"];
				riga.DataFineValidità = (System.DateTime)tabella.Rows[0]["DataFineValidità"];
				riga.Operatore = (System.String)tabella.Rows[0]["Operatore"];
				riga.Storico = (System.Boolean)tabella.Rows[0]["Storico"];
				riga.IdErrore = (System.Int32)tabella.Rows[0]["IdErrore"];
				riga.IDQuestionario = (System.Int32)tabella.Rows[0]["IDQuestionario"];
			}
			return riga;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DichiarazioniErrateView.GetRow.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// Carica l'elenco degli anni presenti
		/// </summary>
		/// <param name="ente"></param>
		/// <returns></returns>
		public DataView AnniCaricati(string ente)
		{
			SqlCommand SelectCommand = new SqlCommand();
            try { 
			SelectCommand.CommandText = "Select AnnoDichiarazione From " + this.TableName + " where Ente=@ente AND Annullato<>1 AND Storico<>1 GROUP BY AnnoDichiarazione having not annodichiarazione is null";
			SelectCommand.Parameters.Add("@ente",SqlDbType.VarChar).Value = ente;

            DataView dv = Query(SelectCommand, new SqlConnection(Business.ConstWrapper.StringConnection)).DefaultView;
			kill();
			return dv;
            }
            catch (Exception Err)
            {
                Log.Debug(Business.ConstWrapper.CodiceEnte + " - DichiarazioniICI.DichiarazioniErrateView.AnniCaricati.errore: ", Err);
                throw Err;
            }
        }
	}
}
