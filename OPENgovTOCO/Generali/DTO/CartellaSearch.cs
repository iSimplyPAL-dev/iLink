using System;
using OPENgovTOCO;

namespace DTO
{
	/// <summary>
	/// Definizione oggetto ricerca Avviso
	/// </summary>
	public class CartellaSearch
	{
		// Non utilizzati direttamente nella ricerca ma per
		// riapplicare i filtri tornando alla pagina di ricerca
		private string _cognomeContribuente;
		private string _nomeContribuente;
		private string _codFiscaleContribuente;
		private string _pIVAContribuente;

        //private string _CodContribuenti;
		private string _numeroAvviso;
		private int _anno = -1;
		//*** 20130610 - ruolo supplettivo ***
		private int _IdFlusso=-1;
        //*** ***
        private bool _IsSgravate = false;
		// Utilizzato per il calcolo rate dove non ho l'HttpContext
		private string _IdEnteNoSession;
        private string _Tributo;
        public string IdTributo
        {
            get { return _Tributo; }
            set { _Tributo = value; }
        }
        public string CognomeContribuente
		{
			get { return _cognomeContribuente; }
			set { _cognomeContribuente = value; }
		}
		
		public string NomeContribuente
		{
			get { return _nomeContribuente; }
			set { _nomeContribuente = value; }
		}
		
		public string CodFiscaleContribuente
		{
			get { return _codFiscaleContribuente; }
			set { _codFiscaleContribuente = value; }
		}
		
		public string PIVAContribuente
		{
			get { return _pIVAContribuente; }
			set { _pIVAContribuente = value; }
		}
		
        //public string CodContribuenti
        //{
        //    get { return _CodContribuenti; }
        //    set { _CodContribuenti = value; }
        //}

		public string IdEnte
		{
			get
			{
				if (IdEnteNoSession == null || IdEnteNoSession.CompareTo (string.Empty) == 0)
					return DichiarazioneSession.IdEnte;
				else
					return IdEnteNoSession;
			}
		}

		public string IdEnteNoSession
		{
			get { return _IdEnteNoSession; }
			set { _IdEnteNoSession = value; }
		}

		public string NumeroAvviso
		{
			get { return _numeroAvviso; }
			set { _numeroAvviso = value; }
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
        public bool IsSgravate
        {
            get { return _IsSgravate; }
            set { _IsSgravate = value; }
        }
    }
}
