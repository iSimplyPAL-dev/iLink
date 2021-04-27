using System;

namespace DTO
{
	/// <summary>
	/// Definizione oggetto ricerca rate
	/// </summary>
	public class RataSearch
	{
		private string _IdEnte;
		private string _CodiceCartella;
		private int _Anno;
		private int _IdContribuente;
        public string _Tributo = Utility.Costanti.TRIBUTO_OSAP;
        public int _IdCartella;
        public string IdTributo
        {
            get { return _Tributo; }
            set { _Tributo = value; }
        }
        public string IdEnte 
		{ 
			get	{ return _IdEnte; }			
			set	{ _IdEnte = value; }
		}

		public int Anno
		{ 
			get	{ return _Anno; }			
			set	{ _Anno = value; }
		}

		public string CodiceCartella 
		{ 
			get	{ return _CodiceCartella; }			
			set	{ _CodiceCartella = value; }
		}

		public int IdContribuente 
		{ 
			get	{ return _IdContribuente; }			
			set	{ _IdContribuente = value; }
		}
        public int IdCartella
        {
            get { return _IdCartella; }
            set { _IdCartella = value; }
        }
        public RataSearch()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
