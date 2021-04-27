using System;

namespace DTO
{
	/// <summary>
	/// Defininizione oggetto ricerca Pagamenti.
	/// </summary>
	public class PagamentiSearch
	{
		private string _IdEnte;
		private string _Cognome;
		private string _Nome;
		private string _CF;
		private string _PIVA;
		private string _AnnoRif;
		private string _NAvviso;
		private int _IdContribuente;
		private DateTime _DataAccreditoDal;
		private DateTime _DataAccreditoAl;
		// {0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
		private int _nTipoStampa;
        public string _Tributo = Utility.Costanti.TRIBUTO_OSAP;
        public string IdTributo
        {
            get { return _Tributo; }
            set { _Tributo = value; }
        }
        public PagamentiSearch()
		{
			_IdContribuente = -1;
			_nTipoStampa = 0;
		}

		public string IdEnte 
		{ 
			get	{ return _IdEnte; }			
			set	{ _IdEnte = value; }
		}
		public string Cognome 
		{ 
			get	{ return _Cognome; }			
			set	{ _Cognome = value; }
		}
		public string Nome 
		{ 
			get	{ return _Nome; }			
			set	{ _Nome = value; }
		}
		public string CF 
		{ 
			get	{ return _CF; }			
			set	{ _CF = value; }
		}
		public string PIVA 
		{ 
			get	{ return _PIVA; }			
			set	{ _PIVA = value; }
		}
		public string AnnoRif 
		{ 
			get	{ return _AnnoRif; }			
			set	{ _AnnoRif = value; }
		}
		public string NAvviso 
		{ 
			get	{ return _NAvviso; }			
			set	{ _NAvviso = value; }
		}
		public int IdContribuente 
		{ 
			get	{ return _IdContribuente; }			
			set	{ _IdContribuente = value; }
		}
		public DateTime DataAccreditoDal 
		{ 
			get	{ return _DataAccreditoDal; }			
			set	{ _DataAccreditoDal = value; }
		}
		public DateTime DataAccreditoAl 
		{ 
			get	{ return _DataAccreditoAl; }			
			set	{ _DataAccreditoAl = value; }
		}
		public int TipoStampa 
		{ 
			get	{ return _nTipoStampa; }			
			set	{ _nTipoStampa = value; }
		}
	}
}
