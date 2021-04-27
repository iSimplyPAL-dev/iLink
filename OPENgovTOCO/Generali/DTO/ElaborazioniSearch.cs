using System;
using OPENgovTOCO;

namespace DTO
{
	/// <summary>
	/// Definizione oggetto Ricerca Elaborazioni.
	/// </summary>
	public class ElaborazioniSearch
	{
		private string _nomeDA=null;
		private string _nomeA=null;
		private string _codiceCartella=null;
		private int _anno=DateTime.Now.Year;
		//*** 20130610 - ruolo supplettivo ***
		public int _IdFlusso;
		//*** ***
        public string _Tributo = Utility.Costanti.TRIBUTO_OSAP;
        public string IdTributo
        {
            get { return _Tributo; }
            set { _Tributo = value; }
        }
		public string IdEnte
		{
			get { return DichiarazioneSession.IdEnte; }
		}
		public string DBName
		{
			get{return System.Configuration.ConfigurationManager.AppSettings["NOME_DATABASE_ANAGRAFICA"].ToString();}
		}

		public string Nome
		{
			get {
				if ((_nomeDA != null) && (_nomeA == null))
					return _nomeDA; 
				else
					return null;
			}
		}

		public string NomeDA
		{
			get { return _nomeDA; }
			set { _nomeDA = value; }
		}

		public string NomeA
		{
			get { return _nomeA; }
			set { _nomeA = value; }
		}
		
		public string CodiceCartella
		{
			get { return _codiceCartella; }
			set { _codiceCartella = value; }
		}
		
		public int Anno
		{
			get { return _anno; }
			set { _anno = value; }
		}
		//*** 20130610 - ruolo supplettivo ***
		public int IdFlusso
		{
			get { return _IdFlusso; }
			set { _IdFlusso = value; }
		}
		//*** ***
	}
}
