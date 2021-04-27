using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using log4net;

namespace Business
{
    /// <summary>
    /// Classe per le funzioni generali di utilità
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class CoreUtility
	{
          private static readonly ILog log = LogManager.GetLogger(typeof(CoreUtility));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="DatiToPrint"></param>
        /// <returns></returns>
        public Boolean WriteFile(string sFile, string DatiToPrint)
        {
            System.IO.StreamWriter MyFileToWrite = System.IO.File.AppendText(sFile);
            string sDatiFile = "";
            try
            {
                sDatiFile = DatiToPrint;
                MyFileToWrite.WriteLine(sDatiFile.ToUpper());
                MyFileToWrite.Flush();
                return true;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.WriteFile.errore: ", Err);
                return false;
            }
            finally
            {
                MyFileToWrite.Close();
            }
        }



          /// <summary>
        /// Converte la stringa passata se composta di soli numeri altrimenti torna 0
        /// </summary>
        /// <param name="stringRappresentation"></param>
        /// <returns></returns>
        /// 
    public static int ConvertiNumero(string stringRappresentation)
        {
            try { 

            int retVal = 0;
            if (stringRappresentation != String.Empty && stringRappresentation != null)
            {
                bool Numerico = true;
                foreach (Char carattere in stringRappresentation)
                {
                    if (!Char.IsNumber(carattere))
                    {
                        Numerico = false;
                        break;
                    }
                }
                if (Numerico) retVal = int.Parse(stringRappresentation);
            }
            return retVal;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.ConvertiNumero.errore: ", Err);
                throw Err;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vInput"></param>
        /// <returns></returns>
        public int BoolToDb(bool vInput)
		{
            try { 
			string sValue = vInput.ToString();
			sValue = sValue.ToLower();
			if (sValue.CompareTo("true")==0)
			{
				return 1;
			}
			else
			{
				return 0;
			}
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.BoolToDb.errore: ", Err);
                throw Err;
            }
        }


        /// <summary>
        /// Funzione generica per popolare le combo
        /// </summary>
        /// <param name="ddl">dropdownlist da popolare</param>
        /// <param name="sSQL">query da eseguire per prelevare i valori per il popolamento</param>
        //public void LoadComboGenerale(DropDownList ddl, string sSQL )
        //{
        //    string WFErrore="";
        //    OPENUtility.CreateSessione WFSessione = null;

        //    try 
        //    {
        //        //inizializzo la connessione
        //        WFSessione = new OPENUtility.CreateSessione(HttpContext.Current.Session["PARAMETROENV"].ToString(), HttpContext.Current.Session["username"].ToString(), HttpContext.Current.Session["IDENTIFICATIVOAPPLICAZIONE"].ToString());
        //        if (!(WFSessione.CreaSessione(HttpContext.Current.Session["username"].ToString(), ref WFErrore ))) 
        //        { 
        //            throw new Exception("Errore durante l'apertura della sessione di WorkFlow"); 
        //        } 

        //        //eseguo la query
        //        SqlDataReader DrDati ;
        //        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL);

        //        loadCombo(ddl, DrDati);
        //    }
        //    catch (Exception ex) 
        //    {
        //        log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.LoadComboGenerale.errore: ", ex);
        //    }
        //    finally 
        //    {
        //        if ((WFSessione.oSession != null)) 
        //        {
        //            WFSessione.Kill();
        //        }
        //    }		
        //}
        public void LoadComboGenerale(DropDownList ddl, string sSQL)
        {
            try
            {
                SqlCommand SelectCommand = new SqlCommand();
                SelectCommand.CommandType = System.Data.CommandType.Text;
                SelectCommand.CommandText =sSQL;
                SelectCommand.Connection = new SqlConnection(Business.ConstWrapper.StringConnection);
                if (SelectCommand.Connection.State==ConnectionState.Closed)
                    SelectCommand.Connection.Open();
                //eseguo la query
                SqlDataReader DrDati;
                log.Debug(Utility.Costanti.LogQuery(SelectCommand));
                DrDati = SelectCommand.ExecuteReader();

                loadCombo(ddl, DrDati);
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.LoadComboGenerale.errore: ", ex);
            }
        }

		/// <summary>
		/// funzione di popolamento effettivo delle combo
		/// </summary>
		/// <param name="objCombo">dropdownlist da popolare</param>
		/// <param name="objDati">valori da immettere nella combo</param>
		public void loadCombo(DropDownList objCombo, SqlDataReader objDati)
		{
			ListItem myListItem=new ListItem();
			myListItem.Text ="...";
			myListItem.Value="";		
			objCombo.Items.Add(myListItem);
            try { 	
			if ((objDati != null)) 
			{
				while (objDati.Read()) 
				{
					if (!(objDati[0] == System.DBNull.Value)) 
					{
						ListItem myListItem1=new ListItem();
						myListItem1.Text =(string)objDati[0];
						myListItem1.Value=(string)objDati[1];
						objCombo.Items.Add(myListItem1);
					}
				}
			}
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.loadCombo.errore: ", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCombo"></param>
        /// <param name="objDati"></param>
        public void loadCombo(DropDownList objCombo, DataView objDati)
        {
            ListItem myListItem = new ListItem();
            myListItem.Text = "...";
            myListItem.Value = "";
            objCombo.Items.Add(myListItem);
            try { 
            if ((objDati != null))
            {
                if (objDati.Count > 0)
                {
                    int nColDescr = 1;
                    if (objDati.Table.Columns.Count == 1)
                        nColDescr = 0;
                        foreach (DataRow myRow in objDati.Table.Rows)
                    {
                        if (!(myRow[0] == System.DBNull.Value))
                        {
                            ListItem myListItem1 = new ListItem();
                            myListItem1.Text = myRow[0].ToString();
                                                        myListItem1.Value = myRow[nColDescr].ToString();
                            objCombo.Items.Add(myListItem1);
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.loadCombo.errore: ", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDato"></param>
        /// <returns></returns>
        public static string FormattaGrdInt(object objDato)
		{
            string myRet = string.Empty;
            try
            {
                if (objDato != null && objDato != DBNull.Value)
                {
                    double n = 0;
                    if (double.TryParse(objDato.ToString(), out n))
                    {
                        if (n == -1)
                        {
                            myRet = string.Empty;
                        }
                        else
                            myRet = n.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdInt.errore: ", ex);
                myRet = string.Empty;
            }
            return myRet;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDato"></param>
        /// <returns></returns>
        public static string FormattaGrdEuro(object objDato)
        {
            string myRet = string.Empty;
            try
            {
                if (objDato != null && objDato != DBNull.Value)
                {
                    float n = 0;
                    if (float.TryParse(objDato.ToString(), out n))
                    {
                        if (n == -1)
                        {
                            myRet = Convert.ToDecimal(0).ToString("N");
                        }
                        else
                            myRet = Convert.ToDecimal(n).ToString("N");
                    }
                }
            }
             catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdEuro.errore: ", ex);
                myRet = string.Empty;
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public static string FormattaDataGrd(object objData)
		{
            string myString = string.Empty;
            try
            {
                if (objData != null && objData != DBNull.Value)
                {
                    if ((objData.ToString().Length > 0) && (objData.ToString().Length == 8) && (objData.ToString() != "19000000"))
                    {
                        myString = objData.ToString().Substring(6, 2) + "/" + objData.ToString().Substring(4, 2) + "/" + objData.ToString().Substring(0, 4);
                    }
                    else
                    {
                        if (((DateTime)objData).CompareTo(DateTime.MinValue.Date) != 0 && ((DateTime)objData).CompareTo(DateTime.MaxValue.Date) != 0 )
                        {
                            DateTime data = (DateTime)objData;
                            if (data.ToShortDateString()!=DateTime.MaxValue.ToShortDateString())
                                myString = data.ToString("dd/MM/yyyy");
                            else
                                myString = string.Empty;
                        }
                        else
                            myString = string.Empty;
                    }
                }
                else
                    myString = string.Empty;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaDataGrd.errore: ", ex);
            }
            return myString;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public static string FormattaDataOraGrd(object objData)
        {
            string myString = string.Empty;
            try
            {
                if (objData != null && objData != DBNull.Value)
                {
                    if ((objData.ToString().Length > 0) && (objData.ToString().Length == 8) && (objData.ToString() != "19000000"))
                    {
                        myString = objData.ToString().Substring(6, 2) + "/" + objData.ToString().Substring(4, 2) + "/" + objData.ToString().Substring(0, 4);
                    }
                    else
                    {
                        if (((DateTime)objData).CompareTo(DateTime.MinValue.Date) != 0 && ((DateTime)objData).CompareTo(DateTime.MaxValue.Date) != 0)
                        {
                            DateTime data = (DateTime)objData;
                            if (data.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                                myString = data.ToString("dd/MM/yyyy HH:mm:ss");
                            else
                                myString = string.Empty;
                        }
                        else
                            myString = string.Empty;
                    }
                }
                else
                    myString = string.Empty;
            }
            catch
            {
                myString = string.Empty;
            }
            return myString;
        }
        //*** 20120828 - IMU adeguamento per importi statali ***
     /// <summary>
     /// 
     /// </summary>
     /// <param name="oImpComune"></param>
     /// <param name="oImpStato"></param>
     /// <returns></returns>
     public static string FormattaGrdSumEuro(object oImpComune, object oImpStato)
        {
            string ret = string.Empty;
            double myImp = 0;

            try
            {
                if (oImpComune != DBNull.Value)
                {

                    if ((oImpComune.ToString() != "-1") && (oImpComune.ToString() != "-1,00"))
                    {
                        myImp = Convert.ToDouble(oImpComune);
                    }
                }
                if (oImpStato != DBNull.Value)
                {

                    if ((oImpStato.ToString() != "-1") && (oImpStato.ToString() != "-1,00"))
                    {
                        myImp += Convert.ToDouble(oImpStato);
                    }
                }
                ret = Convert.ToDecimal(myImp).ToString("N");
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdSumEuro.errore: ", ex);
                ret = string.Empty;
            }
            return ret;
        }
        //*** ***
        //*** 20130213 - controllo con catasto ***
	/// <summary>
    /// 
    /// </summary>
    /// <param name="Via"></param>
    /// <param name="Civico"></param>
    /// <param name="Interno"></param>
    /// <param name="Esponente"></param>
    /// <param name="Scala"></param>
    /// <param name="Piano"></param>
    /// <returns></returns>
    public static object FormattaVia(object Via, object Civico, object Interno, object Esponente, object Scala, object Piano)
		{
			string ret = string.Empty;
            try { 			
			if (Via != null)
			{
				ret = Via.ToString();
			}
			else
			{
				ret = "";
			}
			if (Civico != null)
			{
				try
				{
					Civico=(int)Civico;
				}
				catch 
				{
					Civico="";
				}
				ret += " " + Civico.ToString();
			}
			if (Esponente != null)
			{
				if (Esponente.ToString() != "")
				{
					ret += " " + Esponente.ToString();
				}
				else
				{
					ret += "";
				}
			}
			else
			{
				ret += "";
			}
			if (Scala != null)
			{
				if (Scala.ToString() != "")
				{
					ret += " " + Scala.ToString();
				}
				else
				{
					ret += "";
				}
			}
			else
			{
				ret += "";
			}
			if (Piano != null)
			{
				if (Piano.ToString() != "")
				{
					ret += " " + Piano.ToString();
				}
				else
				{
					ret += "";
				}
			}
			else
			{
				ret += "";
			}
			if (Interno != null)
			{
				if (Interno.ToString() != "")
				{
					ret += " " + Interno.ToString();
				}
				else
				{
					ret += "";
				}
			}
			else
			{
				ret += "";
			}
									
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaVia.errore: ", ex);
                ret = string.Empty;
            }
 			return ret;
       }
		//*** ***
    /// <summary>
    /// 
    /// </summary>
    /// <param name="CodFiscale"></param>
    /// <param name="PIVA"></param>
    /// <returns></returns>
    public static object FormattaCFPIVA(object CodFiscale, object PIVA)
        {
            string ret = string.Empty;
            try {
            if (PIVA != null && PIVA.ToString()!=string.Empty)
            {
                ret = CodFiscale.ToString();
            }
            else
            {
                if (CodFiscale != null)
                {
                    ret = CodFiscale.ToString();
                }
            }

            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaCFPIVA.errore: ", ex);
                ret = string.Empty;
            }
              return ret;
      }
/// <summary>
/// 
/// </summary>
/// <param name="objDato"></param>
/// <param name="sMyUrlImg"></param>
/// <returns></returns>
		public static string FormattaGrdCheck(object objDato, string sMyUrlImg)
		{
            try { 
			if (objDato != null && objDato!= DBNull.Value && (objDato.ToString().ToUpper()=="X"))
			{
				return sMyUrlImg + "visto.png";
			}
			else
				return sMyUrlImg + "trasparente.png";
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdSumEuro.errore: ", ex);
                throw ex;
            }
        }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isReverse"></param>
    /// <param name="iInput"></param>
    /// <returns></returns>
    public static string FormattaGrdBoolToString(int isReverse, object iInput)
        {
            string myRet = string.Empty;

            try
            {
                if ((iInput.ToString() == "1") || (iInput.ToString().ToUpper() == "TRUE"))
                {
                    if (isReverse == 1)
                        myRet = "NO";
                    else
                        myRet = "SI";
                }
                else
                {
                    if (isReverse == 1)
                        myRet = "SI";
                    else
                        myRet = "NO";
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdBoolToString.errore: ", ex);
                myRet = string.Empty;
            }
            return myRet;
        }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="iInput"></param>
   /// <returns></returns>
   public static string FormattaGrdAbiPrinc(object iInput)
        {
            string myRet = string.Empty;

            try
            {
                if ((iInput.ToString() == "1"))
                {
                    myRet = "SI";
                }
                else if ((iInput.ToString() == "2"))
                {
                    myRet = "Pert";
                }
                else
                {
                    myRet = "NO";
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdBoolToString.errore: ", ex);
                myRet = string.Empty;
            }
            return myRet;
        }
        //*** 20140923 - GIS ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDato"></param>
        /// <returns></returns>
        public static bool FormattaGrdCheck(object objDato)
        {
            bool myRet = false;
            try
            {
                if (objDato != null && objDato != DBNull.Value)
                {
                    int n = 0;
                    if (int.TryParse(objDato.ToString(), out n))
                    {
                        if (n == 1 || n == -1)
                        {
                            myRet = true;
                        }
                        else
                            myRet = false;
                    }
                    else
                        bool.TryParse(objDato.ToString(), out myRet);
                }
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdCheck.errore: ", ex);
                myRet = false;
            }
            return myRet;
        }
        //*** ***
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saldo"></param>
        /// <param name="acconto"></param>
        /// <returns></returns>
        public static string FormattaGrdAccontoSaldo(object saldo, object acconto)
        {
            string valore = string.Empty;

            try
            {
                if (Convert.ToBoolean(acconto) == true)
                    valore = "Acconto";
                if (Convert.ToBoolean(saldo) == true)
                    valore = "Saldo";
                if ((Convert.ToBoolean(acconto) == true) && (Convert.ToBoolean(saldo) == true))
                    valore = "Unica Soluzione";
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdAccontoSaldo.errore: ", ex);
                valore = string.Empty;
            }
            return valore;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Violazione"></param>
        /// <returns></returns>
        public static string FormattaGrdViolazione(object Violazione)
        {
            string valore = string.Empty;
            try
            {
                if (Convert.ToBoolean(Violazione) == true)
                    valore = "SI";
                else
                    valore = "NO";
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdViolazione.errore: ", ex);
                valore = string.Empty;
            }
            return valore;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nFigli"></param>
        /// <param name="PercentCarico"></param>
        /// <returns></returns>
        public static string FormattaGrdCaricoFigli(object nFigli, object PercentCarico)
        {
            string ret = string.Empty;
            try
            {
                if (nFigli != DBNull.Value)
                {
                    if (int.Parse(nFigli.ToString()) > 0)
                    {
                        ret = nFigli.ToString();
                    }
                }
                if (PercentCarico != DBNull.Value)
                {
                    if (Convert.ToDecimal(PercentCarico) > 0)
                    {
                        ret += " rid. al " + Convert.ToDecimal(PercentCarico).ToString() + "%";
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.FormattaGrdCaricoFigli.errore: ", ex);
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DtAddRow"></param>
        /// <param name="sValueRow"></param>
        public static void AddRowStampa(ref DataTable DtAddRow, string sValueRow)
        {
            string[] sTextRow;
            DataRow DrAddRow;
            int x = 0;

            try
            {
                //aggiungo una nuova riga nel datarow
                DrAddRow = DtAddRow.NewRow();
                //controllo se la riga e\' scritta
                if (sValueRow != "")
                {
                    sTextRow = sValueRow.Split((char.Parse("|")));
                    for (x = 0; x <= sTextRow.GetUpperBound(0); x++)
                    {
                        //popolo la riga nel datarow
                        DrAddRow[x] = sTextRow[x];
                    }
                }
                //aggiorno la riga al datatable
                DtAddRow.Rows.Add(DrAddRow);
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.CoreUtility.AddRowStampa.errore: ", Err);
                throw Err;
            }
        }
    }
}
