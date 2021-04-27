using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Business;
using System.Configuration;
using log4net;

namespace DichiarazioniICI.Database
{
    /// <summary>
    /// Classe per il calcolo del ravvedimento operoso.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class TblRavvedimentoOperoso:Database
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TblRavvedimentoOperoso));


        public TblRavvedimentoOperoso()
		{
			//
			// TODO: Add constructor logic here
			//

			this.TableName = "TblRavvedimentoOperoso";
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="objDSRavvOper"></param>
		/// <returns></returns>
		public bool InsertRavvedimentoOperoso(DataSet objDSRavvOper)
		{
			bool InsertRetVal; 
			string SQL; 	
			bool blnVal=true;
            try {
                foreach (DataRow myRow in objDSRavvOper.Tables[0].Rows)
                {
                    string sTipoVers= myRow["TIPO_VERSAMENTO"].ToString();

				SQL = "INSERT INTO TblRavvedimentoOperoso (ENTE,ANNO,COD_CONTRIBUENTE,ACCONTO,SALDO,DATA_SCADENZA,"; 
				SQL = SQL + " GIORNI_RITARDO,IMPORTO_TOTALE,IMPORTO_NON_VERSATO,IMPORTO_SANZIONE,IMPORTO_INTERESSI,"; 
				SQL = SQL + " COD_VOCE,ID_VALORE_VOCE,VALORE_VOCE,TIPO_MISURA_VOCE,COD_TIPO_INTERESSE,VALORE_INTERESSE,NOTE)"; 

				SQL = SQL + " VALUES("; 
				SQL = SQL + this.CStrToDB(myRow["ENTE"], ref blnVal, false); 
				SQL = SQL + "," + this.CStrToDB(myRow["ANNO"], ref blnVal, false); 
				SQL = SQL + "," + this.cToInt(myRow["COD_CONTRIBUENTE"]); 

				if (sTipoVers.CompareTo("1")==0)
				{
					SQL = SQL + ",1,0"; 
				}
				else if(sTipoVers.CompareTo("2")==0)
				{
					SQL = SQL + ",0,1"; 
				}
				else if(sTipoVers.CompareTo("3")==0)
				{
					SQL = SQL + ",1,1"; 
				}

				SQL = SQL + "," + this.CStrToDB( DateTime.Parse( myRow["DATA_SCADENZA"].ToString()).ToString("yyyyMMdd"), ref blnVal, false); 

				SQL = SQL + "," + this.cToInt(myRow["GIORNI_RITARDO"]); 

				SQL = SQL + "," + this.CDoubleToDB(myRow["IMPORTO_TOTALE"]); 
				SQL = SQL + "," + this.CDoubleToDB(myRow["IMPORTO_NON_VERSATO"]); 
				SQL = SQL + "," + this.CDoubleToDB(myRow["IMPORTO_SANZIONI"]); 
				SQL = SQL + "," + this.CDoubleToDB(myRow["IMPORTO_INTERESSI"]); 

				SQL = SQL + "," + this.CStrToDB(myRow["COD_VOCE"], ref blnVal, false); 
				SQL = SQL + "," + this.cToInt(myRow["ID_VALORE_VOCE"]); 
				SQL = SQL + "," + this.CStrToDB(myRow["VALORE_VOCE"], ref blnVal, false); 
				SQL = SQL + "," + this.CStrToDB(myRow["TIPO_MISURA_VOCE"], ref blnVal, false); 
				SQL = SQL + "," + this.CStrToDB(myRow["COD_TIPO_INTERESSE"], ref blnVal, false); 
				SQL = SQL + "," + this.CStrToDB(myRow["VALORE_INTERESSE"],  ref blnVal, false);

				SQL = SQL + "," + this.CStrToDB(myRow["NOTE"], ref blnVal, false); 

				SQL = SQL + ")"; 

				SqlCommand insertCommand=new SqlCommand();
				insertCommand.CommandText=SQL;

                InsertRetVal = Execute(insertCommand, new SqlConnection(Business.ConstWrapper.StringConnection));
				if (InsertRetVal==false)
				{
					return false;
				}
			}

			return true;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TblRavvedimentoOperoso.InsertRavvedimentoOperoso.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vInput"></param>
		/// <returns></returns>
		public string CIdToDB(object vInput) 
		{ 
			string stringa=string.Empty;
			stringa = "Null";
            try { 
			if (vInput.ToString() != DBNull.Value.ToString()) 		
			{ 
				
				double result;
				if (double.TryParse(vInput.ToString(),NumberStyles.Integer,null,out result)==true)
				//if (IsNumeric(vInput)) 
				{ 
					if (System.Convert.ToDouble(vInput) > 0) 
					{ 
						stringa = System.Convert.ToString(System.Convert.ToDouble(vInput)); 
					} 
				} 
			} 
			return stringa;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TblRavvedimentoOperoso.CldToDB.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vInput"></param>
		/// <param name="blnClearSpace"></param>
		/// <param name="blnUseNull"></param>
		/// <returns></returns>
		public string CStrToDB(object vInput, ref bool blnClearSpace, bool blnUseNull) 
		{ 
			string sTesto; 
			string stringa=string.Empty;
            try { 
			if (blnUseNull) 
			{ 
				stringa = "Null"; 
			} 
			else 
			{ 
				stringa = "''"; 
			} 
			if (vInput.ToString() != DBNull.Value.ToString()) 
			{ 
				sTesto = System.Convert.ToString(vInput); 
				if (blnClearSpace) 
				{ 
					sTesto = sTesto.Trim(); 
				} 
				if (sTesto.Trim() != "") 
				{ 
					stringa = "'" + sTesto.Replace("'", "''") + "'"; 
				} 
			} 

			return stringa;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TblRavvedimentoOperoso.CStrToDB.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vInput"></param>
		/// <returns></returns>
		public short CToBit(object vInput) 
		{ 
			short myBool;
			myBool = 0;
            try { 
			if (vInput.ToString() != DBNull.Value.ToString()) 
			
			{ 
				if (System.Convert.ToBoolean(vInput)) 
				{ 
					myBool = 1; 
				} 
				else 
				{ 
					myBool = 0; 
				} 
			} 

			return myBool;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TblRavvedimentoOperoso.CToBit.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vInput"></param>
		/// <returns></returns>
		public string CDoubleToDB(object vInput) 
		{ 
			string strToDbl = "Null";
            try { 
			if (vInput.ToString() != DBNull.Value.ToString()) 
			
			{ 
				strToDbl = System.Convert.ToString(vInput); 
				strToDbl = strToDbl.Replace( ",", "."); 
			} 
			return strToDbl;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TblRavvedimentoOperoso.CDoubleToDB.errore: ", Err);
                throw Err;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objInput"></param>
		/// <returns></returns>
		public int cToInt(object objInput) 
		{ 
			int intero=0;
            try { 
			if (objInput.ToString() != DBNull.Value.ToString()) 			 
			{ 
				double result;
				if (double.TryParse(objInput.ToString(),NumberStyles.Integer,null,out result)==true)
				//if (IsNumeric(objInput)) 
				{ 
					intero = Convert.ToInt32(objInput); 
				} 
			} 
			return intero;
            }
            catch (Exception Err)
            {
                log.Debug(Business.ConstWrapper.CodiceEnte + "."+ Business.ConstWrapper.sUsername + " - DichiarazioniICI.TblRavvedimentoOperoso.cToInt.errore: ", Err);
                throw Err;
            }
        }


	}
}
