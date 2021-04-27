using System;
using OPENgovTOCO;

namespace DTO
{
	/// <summary>
	/// Definizione oggetto ricerca Dichiarazione.
	/// </summary>
    /// <revisionHistory>
    /// <revision date="12/04/2019">
    /// <strong>Qualificazione AgID-analisi_rel01</strong>
    /// <em>Esportazione completa dati</em>
    /// </revision>
    /// </revisionHistory>
	public class DichiarazioneSearch
	{
        private string _Ente = "";
        private string _Tributo = "";
		private string _cognomeContribuente="";
		private string _nomeContribuente="";
		private string _codFiscaleContribuente="";
		private string _pIVAContribuente="";
		
		public string IdEnte
		{
			get { return (_Ente==string.Empty?DichiarazioneSession.IdEnte: _Ente); }
            set { _Ente = value; }
		}
        public string CodTributo
        {
            get { return (_Tributo==String.Empty? DichiarazioneSession.CodTributo(""):_Tributo); }
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
        private string _NDichiarazione = "";
        public string NDichiarazione
        {
            get { return _NDichiarazione; }
            set { _NDichiarazione = value; }
        }
    }
    //public class DichiarazioneSearch
    //{
    //    private string _Ente = "";
    //    private string _cognomeContribuente = "";
    //    private string _nomeContribuente = "";
    //    private string _codFiscaleContribuente = "";
    //    private string _pIVAContribuente = "";

    //    public string IdEnte
    //    {
    //        get { return DichiarazioneSession.IdEnte; }
    //        set { _Ente = value; }
    //    }
    //    public string CodTributo
    //    {
    //        get { return DichiarazioneSession.CodTributo(""); }
    //    }
    //    public string CognomeContribuente
    //    {
    //        get { return _cognomeContribuente; }
    //        set { _cognomeContribuente = value; }
    //    }

    //    public string NomeContribuente
    //    {
    //        get { return _nomeContribuente; }
    //        set { _nomeContribuente = value; }
    //    }

    //    public string CodFiscaleContribuente
    //    {
    //        get { return _codFiscaleContribuente; }
    //        set { _codFiscaleContribuente = value; }
    //    }

    //    public string PIVAContribuente
    //    {
    //        get { return _pIVAContribuente; }
    //        set { _pIVAContribuente = value; }
    //    }
    //    private string _NDichiarazione = "";
    //    public string NDichiarazione
    //    {
    //        get { return _NDichiarazione; }
    //        set { _NDichiarazione = value; }
    //    }
    //}
}
